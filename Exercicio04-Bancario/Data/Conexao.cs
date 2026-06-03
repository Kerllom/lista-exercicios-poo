using System;
using MySql.Data.MySqlClient;

namespace Exercicio04Bancario.Data
{
    public class Conexao
    {
        private const string StringConexao =
            "Server=localhost;Port=3306;Database=banco;Uid=root;Pwd=;";

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
                    "Verifique se o MySQL está rodando.\nDetalhe: " + ex.Message);
            }
        }
    }
}