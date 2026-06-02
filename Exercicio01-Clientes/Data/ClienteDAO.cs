using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio01Clientes.Models;

namespace Exercicio01Clientes.Data
{
    public class ClienteDAO
    {
        // CREATE — cadastra um novo cliente no banco
        public void Inserir(Cliente cliente)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO clientes (nome, cpf, email, telefone, data_cadastro) " +
                             "VALUES (@nome, @cpf, @email, @telefone, @dataCadastro);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    cmd.Parameters.AddWithValue("@dataCadastro", cliente.DataCadastro);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // READ — lista todos os clientes cadastrados
        public List<Cliente> ListarTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome, cpf, email, telefone, data_cadastro FROM clientes ORDER BY nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Cliente cliente = new Cliente();
                        cliente.Id = leitor.GetInt32("id");
                        cliente.Nome = leitor.GetString("nome");
                        cliente.Cpf = leitor.GetString("cpf");
                        cliente.Email = leitor.GetString("email");
                        cliente.Telefone = leitor.GetString("telefone");
                        cliente.DataCadastro = leitor.GetDateTime("data_cadastro");

                        clientes.Add(cliente);
                    }
                }
            }

            return clientes;
        }

        // READ — busca um único cliente pelo CPF
        public Cliente BuscarPorCpf(string cpf)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome, cpf, email, telefone, data_cadastro FROM clientes WHERE cpf = @cpf;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);

                    using (MySqlDataReader leitor = cmd.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            Cliente cliente = new Cliente();
                            cliente.Id = leitor.GetInt32("id");
                            cliente.Nome = leitor.GetString("nome");
                            cliente.Cpf = leitor.GetString("cpf");
                            cliente.Email = leitor.GetString("email");
                            cliente.Telefone = leitor.GetString("telefone");
                            cliente.DataCadastro = leitor.GetDateTime("data_cadastro");
                            return cliente;
                        }
                    }
                }
            }
            return null; // não encontrou ninguém com esse CPF
        }

        // UPDATE — atualiza e-mail e telefone de um cliente (localizado pelo CPF)
        public bool AtualizarContato(string cpf, string novoEmail, string novoTelefone)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "UPDATE clientes SET email = @email, telefone = @telefone WHERE cpf = @cpf;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@email", novoEmail);
                    cmd.Parameters.AddWithValue("@telefone", novoTelefone);
                    cmd.Parameters.AddWithValue("@cpf", cpf);

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }

        // DELETE — remove um cliente pelo CPF
        public bool Remover(string cpf)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "DELETE FROM clientes WHERE cpf = @cpf;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }
    }
}