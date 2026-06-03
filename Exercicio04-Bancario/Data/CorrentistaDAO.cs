using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio04Bancario.Models;

namespace Exercicio04Bancario.Data
{
    public class CorrentistaDAO
    {
        public void Inserir(Correntista correntista)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO correntistas (nome, cpf) VALUES (@nome, @cpf);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", correntista.Nome);
                    cmd.Parameters.AddWithValue("@cpf", correntista.Cpf);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        correntista.Id = (int)cmd.LastInsertedId;
                    }
                    catch (MySqlException ex) when (ex.Number == 1062)
                    {
                        throw new Exception("Já existe um correntista com este CPF.");
                    }
                }
            }
        }

        public List<Correntista> ListarTodos()
        {
            List<Correntista> lista = new List<Correntista>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome, cpf FROM correntistas ORDER BY nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Correntista correntista = new Correntista();
                        correntista.Id = leitor.GetInt32("id");
                        correntista.Nome = leitor.GetString("nome");
                        correntista.Cpf = leitor.GetString("cpf");
                        lista.Add(correntista);
                    }
                }
            }

            return lista;
        }
    }
}