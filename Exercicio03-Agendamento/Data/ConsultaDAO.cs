using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio03Agendamento.Models;

namespace Exercicio03Agendamento.Data
{
    public class ConsultaDAO
    {
        // Agenda uma nova consulta (status inicial = AGENDADA)
        public void Agendar(Consulta consulta)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO consultas (paciente_id, medico_id, data_hora, status) " +
                             "VALUES (@pacienteId, @medicoId, @dataHora, 'AGENDADA');";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@pacienteId", consulta.PacienteId);
                    cmd.Parameters.AddWithValue("@medicoId", consulta.MedicoId);
                    cmd.Parameters.AddWithValue("@dataHora", consulta.DataHora);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        consulta.Id = (int)cmd.LastInsertedId;
                    }
                    catch (MySqlException ex) when (ex.Number == 1452)
                    {
                        throw new Exception("Paciente ou médico inexistente.");
                    }
                }
            }
        }

        // Cancela uma consulta (muda o status em vez de apagar)
        public bool Cancelar(int consultaId)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "UPDATE consultas SET status = 'CANCELADA' " +
                             "WHERE id = @id AND status <> 'CANCELADA';";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", consultaId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Lista as consultas de um médico
        public List<Consulta> ListarPorMedico(int medicoId)
        {
            return Listar("WHERE c.medico_id = @id", medicoId);
        }

        // Lista as consultas de um paciente
        public List<Consulta> ListarPorPaciente(int pacienteId)
        {
            return Listar("WHERE c.paciente_id = @id", pacienteId);
        }

        // Método auxiliar PRIVADO: monta a listagem com JOIN das três tabelas.
        // O filtro (por médico ou por paciente) chega como parâmetro.
        private List<Consulta> Listar(string filtro, int id)
        {
            List<Consulta> lista = new List<Consulta>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql =
                    "SELECT c.id, c.paciente_id, c.medico_id, c.data_hora, c.status, " +
                    "       p.nome AS paciente_nome, p.telefone AS paciente_tel, " +
                    "       m.nome AS medico_nome, m.especialidade AS medico_esp " +
                    "FROM consultas c " +
                    "INNER JOIN pacientes p ON p.id = c.paciente_id " +
                    "INNER JOIN medicos m ON m.id = c.medico_id " +
                    filtro + " " +
                    "ORDER BY c.data_hora;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Consulta consulta = new Consulta();
                            consulta.Id = leitor.GetInt32("id");
                            consulta.PacienteId = leitor.GetInt32("paciente_id");
                            consulta.MedicoId = leitor.GetInt32("medico_id");
                            consulta.DataHora = leitor.GetDateTime("data_hora");
                            consulta.Status = leitor.GetString("status");

                            Paciente paciente = new Paciente();
                            paciente.Id = leitor.GetInt32("paciente_id");
                            paciente.Nome = leitor.GetString("paciente_nome");
                            paciente.Telefone = leitor.GetString("paciente_tel");
                            consulta.Paciente = paciente;

                            Medico medico = new Medico();
                            medico.Id = leitor.GetInt32("medico_id");
                            medico.Nome = leitor.GetString("medico_nome");
                            medico.Especialidade = leitor.GetString("medico_esp");
                            consulta.Medico = medico;

                            lista.Add(consulta);
                        }
                    }
                }
            }

            return lista;
        }
    }
}