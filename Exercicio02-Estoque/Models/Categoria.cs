using System;

namespace Exercicio02Estoque.Models
{
    public class Categoria
    {
        private int _id;
        private string _nome;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O nome da categoria não pode ser vazio.");
                _nome = value;
            }
        }
    }
}