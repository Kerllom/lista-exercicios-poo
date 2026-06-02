using System;

namespace Exercicio02Estoque.Models
{
    public class Produto
    {
        private int _id;
        private string _nome;
        private decimal _preco;
        private int _quantidade;
        private int _categoriaId;
        private Categoria _categoria;

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
                    throw new ArgumentException("O nome do produto não pode ser vazio.");
                _nome = value;
            }
        }

        public decimal Preco
        {
            get { return _preco; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("O preço não pode ser negativo.");
                _preco = value;
            }
        }

        public int Quantidade
        {
            get { return _quantidade; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("A quantidade não pode ser negativa.");
                _quantidade = value;
            }
        }

        public int CategoriaId
        {
            get { return _categoriaId; }
            set { _categoriaId = value; }
        }

        public Categoria Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }
    }
}