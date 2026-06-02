using System;
using MySql.Data.MySqlClient;

namespace Exercicio01Clientes.Data
{
    public class Conexao
    {
        // String de conexão: os "dados de acesso" ao banco.
        // ATENÇÃO: troque o Pwd pela SUA senha do MySQL!
        private const string StringConexao =
            "Server=localhost;Port=3306;Database=loja_clientes;Uid=root;Pwd=;";

        public static MySqlConnection ObterConexao()
        {
            try
            {
                MySqlConnection conexao = new MySqlConnection(StringConexao);
                conexao.Open();
                return conexao;
            }
            catch (MySqlException ex)
            {
                throw new Exception(
                    "Não foi possível conectar ao banco de dados. " +
                    "Verifique se o MySQL está rodando e se a senha está correta.\nDetalhe: " + ex.Message);
            }
        }
    }
}