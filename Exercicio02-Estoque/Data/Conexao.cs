using System;
using MySql.Data.MySqlClient;

namespace Exercicio02Estoque.Data
{
    public class Conexao
    {
        private const string StringConexao =
            "Server=localhost;Port=3306;Database=estoque;Uid=root;Pwd=;";

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