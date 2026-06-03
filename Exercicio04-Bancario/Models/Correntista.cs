using System;

namespace Exercicio04Bancario.Models
{
    public class Correntista
    {
        private int _id;
        private string _nome;
        private string _cpf;

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
                    throw new ArgumentException("O nome do correntista não pode ser vazio.");
                _nome = value;
            }
        }

        public string Cpf
        {
            get { return _cpf; }
            set
            {
                string digitos = (value ?? "").Replace(".", "").Replace("-", "").Trim();
                if (digitos.Length != 11)
                    throw new ArgumentException("O CPF deve conter 11 dígitos.");
                _cpf = digitos;
            }
        }
    }
}