using System;
using System.Collections.Generic;
using Exercicio01Clientes.Models;
using Exercicio01Clientes.Data;

namespace Exercicio01Clientes
{
    public class Program
    {
        // Cria UMA instância do DAO pra ser usada por todos os métodos abaixo.
        private static readonly ClienteDAO dao = new ClienteDAO();

        public static void Main(string[] args)
        {
            bool executando = true;

            while (executando)
            {
                Console.WriteLine("\n===== CADASTRO DE CLIENTES =====");
                Console.WriteLine("1 - Cadastrar cliente");
                Console.WriteLine("2 - Listar todos");
                Console.WriteLine("3 - Buscar por CPF");
                Console.WriteLine("4 - Atualizar e-mail/telefone");
                Console.WriteLine("5 - Remover cliente");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1": Cadastrar(); break;
                        case "2": Listar(); break;
                        case "3": Buscar(); break;
                        case "4": Atualizar(); break;
                        case "5": Remover(); break;
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

        // Opção 1 — lê os dados digitados e cadastra o cliente
        private static void Cadastrar()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("CPF (somente números): ");
            string cpf = Console.ReadLine();
            Console.Write("E-mail: ");
            string email = Console.ReadLine();
            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Cliente cliente = new Cliente();
            cliente.Nome = nome;
            cliente.Cpf = cpf;
            cliente.Email = email;
            cliente.Telefone = telefone;
            cliente.DataCadastro = DateTime.Now;

            dao.Inserir(cliente);
            Console.WriteLine("Cliente cadastrado com sucesso! ID: " + cliente.Id);
        }

        // Opção 2 — lista todos os clientes
        private static void Listar()
        {
            List<Cliente> clientes = dao.ListarTodos();

            if (clientes.Count == 0)
            {
                Console.WriteLine("Nenhum cliente cadastrado.");
                return;
            }

            foreach (Cliente c in clientes)
            {
                Console.WriteLine("[" + c.Id + "] " + c.Nome + " | CPF: " + c.Cpf +
                                  " | E-mail: " + c.Email + " | Tel: " + c.Telefone);
            }
        }

        // Opção 3 — busca um cliente pelo CPF
        private static void Buscar()
        {
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            Cliente c = dao.BuscarPorCpf(cpf);

            if (c != null)
                Console.WriteLine("[" + c.Id + "] " + c.Nome + " | E-mail: " + c.Email + " | Tel: " + c.Telefone);
            else
                Console.WriteLine("Cliente não encontrado.");
        }

        // Opção 4 — atualiza e-mail e telefone
        private static void Atualizar()
        {
            Console.Write("CPF do cliente: ");
            string cpf = Console.ReadLine();
            Console.Write("Novo e-mail: ");
            string email = Console.ReadLine();
            Console.Write("Novo telefone: ");
            string telefone = Console.ReadLine();

            bool atualizou = dao.AtualizarContato(cpf, email, telefone);
            Console.WriteLine(atualizou ? "Dados atualizados." : "Cliente não encontrado.");
        }

        // Opção 5 — remove um cliente
        private static void Remover()
        {
            Console.Write("CPF do cliente a remover: ");
            string cpf = Console.ReadLine();

            bool removeu = dao.Remover(cpf);
            Console.WriteLine(removeu ? "Cliente removido." : "Cliente não encontrado.");
        }
    }
}