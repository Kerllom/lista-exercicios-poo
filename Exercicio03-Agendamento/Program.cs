using System;
using System.Collections.Generic;
using System.Globalization;
using Exercicio03Agendamento.Models;
using Exercicio03Agendamento.Data;

namespace Exercicio03Agendamento
{
    public class Program
    {
        private static readonly PacienteDAO pacienteDao = new PacienteDAO();
        private static readonly MedicoDAO medicoDao = new MedicoDAO();
        private static readonly ConsultaDAO consultaDao = new ConsultaDAO();

        public static void Main(string[] args)
        {
            bool executando = true;

            while (executando)
            {
                Console.WriteLine("\n===== AGENDAMENTO DE CONSULTAS =====");
                Console.WriteLine("1 - Cadastrar paciente");
                Console.WriteLine("2 - Cadastrar médico");
                Console.WriteLine("3 - Agendar consulta");
                Console.WriteLine("4 - Cancelar consulta");
                Console.WriteLine("5 - Listar consultas por médico");
                Console.WriteLine("6 - Listar consultas por paciente");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1": CadastrarPaciente(); break;
                        case "2": CadastrarMedico(); break;
                        case "3": Agendar(); break;
                        case "4": Cancelar(); break;
                        case "5": ListarPorMedico(); break;
                        case "6": ListarPorPaciente(); break;
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

        private static void CadastrarPaciente()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Paciente paciente = new Paciente();
            paciente.Nome = nome;
            paciente.Telefone = telefone;

            pacienteDao.Inserir(paciente);
            Console.WriteLine("Paciente cadastrado! ID: " + paciente.Id);
        }

        private static void CadastrarMedico()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Especialidade: ");
            string especialidade = Console.ReadLine();

            Medico medico = new Medico();
            medico.Nome = nome;
            medico.Especialidade = especialidade;

            medicoDao.Inserir(medico);
            Console.WriteLine("Médico cadastrado! ID: " + medico.Id);
        }

        private static void Agendar()
        {
            ListarPacientes();
            Console.Write("ID do paciente: ");
            int pacienteId = int.Parse(Console.ReadLine());

            ListarMedicos();
            Console.Write("ID do médico: ");
            int medicoId = int.Parse(Console.ReadLine());

            Console.Write("Data e hora (dd/MM/yyyy HH:mm): ");
            DateTime dataHora = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            Consulta consulta = new Consulta();
            consulta.PacienteId = pacienteId;
            consulta.MedicoId = medicoId;
            consulta.DataHora = dataHora;

            consultaDao.Agendar(consulta);
            Console.WriteLine("Consulta agendada! ID: " + consulta.Id);
        }

        private static void Cancelar()
        {
            Console.Write("ID da consulta: ");
            int id = int.Parse(Console.ReadLine());

            bool cancelou = consultaDao.Cancelar(id);
            Console.WriteLine(cancelou ? "Consulta cancelada." : "Consulta não encontrada ou já cancelada.");
        }

        private static void ListarPorMedico()
        {
            Console.Write("ID do médico: ");
            int id = int.Parse(Console.ReadLine());
            Imprimir(consultaDao.ListarPorMedico(id));
        }

        private static void ListarPorPaciente()
        {
            Console.Write("ID do paciente: ");
            int id = int.Parse(Console.ReadLine());
            Imprimir(consultaDao.ListarPorPaciente(id));
        }

        private static void Imprimir(List<Consulta> consultas)
        {
            if (consultas.Count == 0)
            {
                Console.WriteLine("Nenhuma consulta encontrada.");
                return;
            }

            foreach (Consulta c in consultas)
            {
                Console.WriteLine("[" + c.Id + "] " + c.DataHora.ToString("dd/MM/yyyy HH:mm") +
                                  " | Paciente: " + c.Paciente.Nome +
                                  " | Médico: " + c.Medico.Nome +
                                  " | " + c.Status);
            }
        }

        private static void ListarPacientes()
        {
            Console.WriteLine("--- Pacientes ---");
            foreach (Paciente p in pacienteDao.ListarTodos())
                Console.WriteLine("[" + p.Id + "] " + p.Nome);
        }

        private static void ListarMedicos()
        {
            Console.WriteLine("--- Médicos ---");
            foreach (Medico m in medicoDao.ListarTodos())
                Console.WriteLine("[" + m.Id + "] " + m.Nome + " (" + m.Especialidade + ")");
        }
    }
}