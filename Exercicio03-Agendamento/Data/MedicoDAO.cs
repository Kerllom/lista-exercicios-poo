using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio03Agendamento.Models;

namespace Exercicio03Agendamento.Data
{
    public class MedicoDAO
    {
        public void Inserir(Medico medico)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO medicos (nome, especialidade) VALUES (@nome, @especialidade);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", medico.Nome);
                    cmd.Parameters.AddWithValue("@especialidade", medico.Especialidade);
                    cmd.ExecuteNonQuery();
                    medico.Id = (int)cmd.LastInsertedId;
                }
            }
        }

        public List<Medico> ListarTodos()
        {
            List<Medico> lista = new List<Medico>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome, especialidade FROM medicos ORDER BY nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Medico medico = new Medico();
                        medico.Id = leitor.GetInt32("id");
                        medico.Nome = leitor.GetString("nome");
                        medico.Especialidade = leitor.GetString("especialidade");
                        lista.Add(medico);
                    }
                }
            }

            return lista;
        }
    }
}