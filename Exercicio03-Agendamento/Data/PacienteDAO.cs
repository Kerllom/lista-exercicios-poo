using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio03Agendamento.Models;

namespace Exercicio03Agendamento.Data
{
    public class PacienteDAO
    {
        public void Inserir(Paciente paciente)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO pacientes (nome, telefone) VALUES (@nome, @telefone);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", paciente.Nome);
                    cmd.Parameters.AddWithValue("@telefone", paciente.Telefone);
                    cmd.ExecuteNonQuery();
                    paciente.Id = (int)cmd.LastInsertedId;
                }
            }
        }

        public List<Paciente> ListarTodos()
        {
            List<Paciente> lista = new List<Paciente>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome, telefone FROM pacientes ORDER BY nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Paciente paciente = new Paciente();
                        paciente.Id = leitor.GetInt32("id");
                        paciente.Nome = leitor.GetString("nome");
                        paciente.Telefone = leitor.GetString("telefone");
                        lista.Add(paciente);
                    }
                }
            }

            return lista;
        }
    }
}