using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio02Estoque.Models;

namespace Exercicio02Estoque.Data
{
    public class ProdutoDAO
    {
        // CREATE — cadastra um produto vinculado a uma categoria
        public void Inserir(Produto produto)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO produtos (nome, preco, quantidade, categoria_id) " +
                             "VALUES (@nome, @preco, @quantidade, @categoriaId);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    cmd.Parameters.AddWithValue("@categoriaId", produto.CategoriaId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        produto.Id = (int)cmd.LastInsertedId;
                    }
                    catch (MySqlException ex) when (ex.Number == 1452)
                    {
                        throw new Exception("A categoria informada não existe.");
                    }
                }
            }
        }

        // READ — lista os produtos de UMA categoria (usando JOIN)
        public List<Produto> ListarPorCategoria(int categoriaId)
        {
            List<Produto> lista = new List<Produto>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql =
                    "SELECT p.id, p.nome, p.preco, p.quantidade, p.categoria_id, c.nome AS categoria_nome " +
                    "FROM produtos p " +
                    "INNER JOIN categorias c ON c.id = p.categoria_id " +
                    "WHERE p.categoria_id = @categoriaId " +
                    "ORDER BY p.nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@categoriaId", categoriaId);

                    using (MySqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Produto produto = new Produto();
                            produto.Id = leitor.GetInt32("id");
                            produto.Nome = leitor.GetString("nome");
                            produto.Preco = leitor.GetDecimal("preco");
                            produto.Quantidade = leitor.GetInt32("quantidade");
                            produto.CategoriaId = leitor.GetInt32("categoria_id");

                            // Preenche o objeto Categoria com os dados que vieram do JOIN
                            Categoria categoria = new Categoria();
                            categoria.Id = leitor.GetInt32("categoria_id");
                            categoria.Nome = leitor.GetString("categoria_nome");
                            produto.Categoria = categoria;

                            lista.Add(produto);
                        }
                    }
                }
            }

            return lista;
        }

        // UPDATE — atualiza a quantidade em estoque de um produto
        public bool AtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade < 0)
                throw new ArgumentException("A quantidade não pode ser negativa.");

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "UPDATE produtos SET quantidade = @quantidade WHERE id = @id;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@quantidade", novaQuantidade);
                    cmd.Parameters.AddWithValue("@id", produtoId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}