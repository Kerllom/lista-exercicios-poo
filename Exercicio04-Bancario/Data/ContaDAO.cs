using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio04Bancario.Models;

namespace Exercicio04Bancario.Data
{
    public class ContaDAO
    {
        // Cria uma conta nova (saldo começa em 0)
        public void CriarConta(ContaBancaria conta)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO contas (numero, saldo, correntista_id) " +
                             "VALUES (@numero, @saldo, @correntistaId);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@numero", conta.Numero);
                    cmd.Parameters.AddWithValue("@saldo", conta.Saldo);
                    cmd.Parameters.AddWithValue("@correntistaId", conta.CorrentistaId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        conta.Id = (int)cmd.LastInsertedId;
                    }
                    catch (MySqlException ex) when (ex.Number == 1452)
                    {
                        throw new Exception("Correntista inexistente.");
                    }
                    catch (MySqlException ex) when (ex.Number == 1062)
                    {
                        throw new Exception("Já existe uma conta com este número.");
                    }
                }
            }
        }

        // Consulta o saldo atual de uma conta
        public decimal ConsultarSaldo(int contaId)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT saldo FROM contas WHERE id = @id;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", contaId);
                    object resultado = cmd.ExecuteScalar();

                    if (resultado == null)
                        throw new Exception("Conta não encontrada.");

                    return Convert.ToDecimal(resultado);
                }
            }
        }

        // Depósito: valor positivo entra na conta
        public void Depositar(int contaId, decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser positivo.");
            ExecutarOperacao(contaId, valor, "DEPOSITO");
        }

        // Saque: valor sai da conta (passa como negativo)
        public void Sacar(int contaId, decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser positivo.");
            ExecutarOperacao(contaId, -valor, "SAQUE");
        }

        // === O CORAÇÃO DO EXERCÍCIO: a transação ===
        private void ExecutarOperacao(int contaId, decimal delta, string tipo)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            using (MySqlTransaction transacao = conexao.BeginTransaction())
            {
                try
                {
                    // 1) Lê o saldo atual, travando a linha (FOR UPDATE)
                    decimal saldoAtual;
                    string sqlSaldo = "SELECT saldo FROM contas WHERE id = @id FOR UPDATE;";
                    using (MySqlCommand cmd = new MySqlCommand(sqlSaldo, conexao, transacao))
                    {
                        cmd.Parameters.AddWithValue("@id", contaId);
                        object r = cmd.ExecuteScalar();
                        if (r == null)
                            throw new Exception("Conta não encontrada.");
                        saldoAtual = Convert.ToDecimal(r);
                    }

                    decimal novoSaldo = saldoAtual + delta;

                    // 2) Regra de negócio: impede saque com saldo insuficiente
                    if (novoSaldo < 0)
                        throw new Exception("Saldo insuficiente. Saldo atual: R$ " + saldoAtual.ToString("F2"));

                    // 3) Atualiza o saldo
                    string sqlUpdate = "UPDATE contas SET saldo = @saldo WHERE id = @id;";
                    using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, conexao, transacao))
                    {
                        cmd.Parameters.AddWithValue("@saldo", novoSaldo);
                        cmd.Parameters.AddWithValue("@id", contaId);
                        cmd.ExecuteNonQuery();
                    }

                    // 4) Registra no extrato (valor sempre positivo; o tipo indica entrada/saída)
                    string sqlExtrato = "INSERT INTO extrato (conta_id, tipo, valor, data_hora) " +
                                        "VALUES (@contaId, @tipo, @valor, @dataHora);";
                    using (MySqlCommand cmd = new MySqlCommand(sqlExtrato, conexao, transacao))
                    {
                        cmd.Parameters.AddWithValue("@contaId", contaId);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@valor", Math.Abs(delta));
                        cmd.Parameters.AddWithValue("@dataHora", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // 5) Deu tudo certo: CONFIRMA tudo de uma vez
                    transacao.Commit();
                }
                catch (Exception)
                {
                    // Algo falhou: DESFAZ tudo, como se nada tivesse acontecido
                    transacao.Rollback();
                    throw;
                }
            }
        }

        // Lista o extrato (histórico) de uma conta
        public List<Movimentacao> ObterExtrato(int contaId)
        {
            List<Movimentacao> lista = new List<Movimentacao>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, conta_id, tipo, valor, data_hora FROM extrato " +
                             "WHERE conta_id = @id ORDER BY data_hora;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", contaId);

                    using (MySqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Movimentacao mov = new Movimentacao();
                            mov.Id = leitor.GetInt32("id");
                            mov.ContaId = leitor.GetInt32("conta_id");
                            mov.Tipo = leitor.GetString("tipo");
                            mov.Valor = leitor.GetDecimal("valor");
                            mov.DataHora = leitor.GetDateTime("data_hora");
                            lista.Add(mov);
                        }
                    }
                }
            }

            return lista;
        }
    }
}