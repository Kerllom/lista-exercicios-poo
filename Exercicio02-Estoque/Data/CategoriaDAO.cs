using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Exercicio02Estoque.Models;

namespace Exercicio02Estoque.Data
{
    public class CategoriaDAO
    {
        // CREATE — cadastra uma categoria
        public void Inserir(Categoria categoria)
        {
            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "INSERT INTO categorias (nome) VALUES (@nome);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", categoria.Nome);
                    cmd.ExecuteNonQuery();
                    categoria.Id = (int)cmd.LastInsertedId;
                }
            }
        }

        // READ — lista todas as categorias
        public List<Categoria> ListarTodas()
        {
            List<Categoria> lista = new List<Categoria>();

            using (MySqlConnection conexao = Conexao.ObterConexao())
            {
                string sql = "SELECT id, nome FROM categorias ORDER BY nome;";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                using (MySqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Categoria categoria = new Categoria();
                        categoria.Id = leitor.GetInt32("id");
                        categoria.Nome = leitor.GetString("nome");
                        lista.Add(categoria);
                    }
                }
            }

            return lista;
        }
    }
}