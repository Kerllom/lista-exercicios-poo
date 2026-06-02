using System;
using System.Collections.Generic;
using System.Globalization;
using Exercicio02Estoque.Models;
using Exercicio02Estoque.Data;

namespace Exercicio02Estoque
{
    public class Program
    {
        private static readonly CategoriaDAO categoriaDao = new CategoriaDAO();
        private static readonly ProdutoDAO produtoDao = new ProdutoDAO();

        public static void Main(string[] args)
        {
            bool executando = true;

            while (executando)
            {
                Console.WriteLine("\n===== CONTROLE DE ESTOQUE =====");
                Console.WriteLine("1 - Cadastrar categoria");
                Console.WriteLine("2 - Cadastrar produto");
                Console.WriteLine("3 - Listar produtos por categoria");
                Console.WriteLine("4 - Atualizar quantidade em estoque");
                Console.WriteLine("5 - Listar categorias");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1": CadastrarCategoria(); break;
                        case "2": CadastrarProduto(); break;
                        case "3": ListarPorCategoria(); break;
                        case "4": AtualizarEstoque(); break;
                        case "5": ListarCategorias(); break;
                        case "0": executando = false; break;
                        default: Console.WriteLine("Opção inválida."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n[ERRO] " + ex.Message);
                }
            }

            Console.WriteLine("Encerrando o sistema...");
        }

        private static void CadastrarCategoria()
        {
            Console.Write("Nome da categoria: ");
            string nome = Console.ReadLine();

            Categoria categoria = new Categoria();
            categoria.Nome = nome;

            categoriaDao.Inserir(categoria);
            Console.WriteLine("Categoria cadastrada! ID: " + categoria.Id);
        }

        private static void CadastrarProduto()
        {
            ListarCategorias(); // mostra as categorias pra ajudar o usuário a escolher
            Console.Write("ID da categoria: ");
            int categoriaId = int.Parse(Console.ReadLine());

            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine();
            Console.Write("Preço (ex: 19.90): ");
            decimal preco = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Quantidade inicial: ");
            int quantidade = int.Parse(Console.ReadLine());

            Produto produto = new Produto();
            produto.Nome = nome;
            produto.Preco = preco;
            produto.Quantidade = quantidade;
            produto.CategoriaId = categoriaId;

            produtoDao.Inserir(produto);
            Console.WriteLine("Produto cadastrado! ID: " + produto.Id);
        }

        private static void ListarPorCategoria()
        {
            Console.Write("ID da categoria: ");
            int categoriaId = int.Parse(Console.ReadLine());

            List<Produto> produtos = produtoDao.ListarPorCategoria(categoriaId);

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto nesta categoria.");
                return;
            }

            foreach (Produto p in produtos)
            {
                Console.WriteLine("[" + p.Id + "] " + p.Nome +
                                  " | R$ " + p.Preco.ToString("F2") +
                                  " | Estoque: " + p.Quantidade +
                                  " | Categoria: " + p.Categoria.Nome);
            }
        }

        private static void AtualizarEstoque()
        {
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Nova quantidade: ");
            int quantidade = int.Parse(Console.ReadLine());

            bool atualizou = produtoDao.AtualizarQuantidade(id, quantidade);
            Console.WriteLine(atualizou ? "Estoque atualizado." : "Produto não encontrado.");
        }

        private static void ListarCategorias()
        {
            List<Categoria> categorias = categoriaDao.ListarTodas();

            if (categorias.Count == 0)
            {
                Console.WriteLine("Nenhuma categoria cadastrada.");
                return;
            }

            Console.WriteLine("--- Categorias ---");
            foreach (Categoria c in categorias)
                Console.WriteLine("[" + c.Id + "] " + c.Nome);
        }
    }
}