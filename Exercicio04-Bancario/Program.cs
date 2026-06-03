using System;
using System.Collections.Generic;
using System.Globalization;
using Exercicio04Bancario.Models;
using Exercicio04Bancario.Data;

namespace Exercicio04Bancario
{
    public class Program
    {
        private static readonly CorrentistaDAO correntistaDao = new CorrentistaDAO();
        private static readonly ContaDAO contaDao = new ContaDAO();

        public static void Main(string[] args)
        {
            bool executando = true;

            while (executando)
            {
                Console.WriteLine("\n===== SISTEMA BANCÁRIO =====");
                Console.WriteLine("1 - Cadastrar correntista");
                Console.WriteLine("2 - Abrir conta");
                Console.WriteLine("3 - Depositar");
                Console.WriteLine("4 - Sacar");
                Console.WriteLine("5 - Consultar saldo");
                Console.WriteLine("6 - Ver extrato");
                Console.WriteLine("7 - Listar correntistas");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1": CadastrarCorrentista(); break;
                        case "2": AbrirConta(); break;
                        case "3": Depositar(); break;
                        case "4": Sacar(); break;
                        case "5": ConsultarSaldo(); break;
                        case "6": VerExtrato(); break;
                        case "7": ListarCorrentistas(); break;
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

        private static void CadastrarCorrentista()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            Correntista c = new Correntista();
            c.Nome = nome;
            c.Cpf = cpf;

            correntistaDao.Inserir(c);
            Console.WriteLine("Correntista cadastrado! ID: " + c.Id);
        }

        private static void AbrirConta()
        {
            ListarCorrentistas();
            Console.Write("ID do correntista: ");
            int correntistaId = int.Parse(Console.ReadLine());
            Console.Write("Número da conta: ");
            string numero = Console.ReadLine();

            ContaBancaria conta = new ContaBancaria();
            conta.Numero = numero;
            conta.Saldo = 0;
            conta.CorrentistaId = correntistaId;

            contaDao.CriarConta(conta);
            Console.WriteLine("Conta aberta! ID: " + conta.Id);
        }

        private static void Depositar()
        {
            Console.Write("ID da conta: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Valor: ");
            decimal valor = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            contaDao.Depositar(id, valor);
            Console.WriteLine("Depósito realizado. Novo saldo: R$ " + contaDao.ConsultarSaldo(id).ToString("F2"));
        }

        private static void Sacar()
        {
            Console.Write("ID da conta: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Valor: ");
            decimal valor = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            contaDao.Sacar(id, valor);
            Console.WriteLine("Saque realizado. Novo saldo: R$ " + contaDao.ConsultarSaldo(id).ToString("F2"));
        }

        private static void ConsultarSaldo()
        {
            Console.Write("ID da conta: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Saldo: R$ " + contaDao.ConsultarSaldo(id).ToString("F2"));
        }

        private static void VerExtrato()
        {
            Console.Write("ID da conta: ");
            int id = int.Parse(Console.ReadLine());

            List<Movimentacao> movimentacoes = contaDao.ObterExtrato(id);

            if (movimentacoes.Count == 0)
            {
                Console.WriteLine("Sem movimentações.");
                return;
            }

            Console.WriteLine("--- Extrato ---");
            foreach (Movimentacao m in movimentacoes)
            {
                Console.WriteLine(m.DataHora.ToString("dd/MM/yyyy HH:mm") +
                                  " | " + m.Tipo +
                                  " | R$ " + m.Valor.ToString("F2"));
            }
        }

        private static void ListarCorrentistas()
        {
            List<Correntista> correntistas = correntistaDao.ListarTodos();

            if (correntistas.Count == 0)
            {
                Console.WriteLine("Nenhum correntista cadastrado.");
                return;
            }

            Console.WriteLine("--- Correntistas ---");
            foreach (Correntista c in correntistas)
                Console.WriteLine("[" + c.Id + "] " + c.Nome + " | CPF: " + c.Cpf);
        }
    }
}