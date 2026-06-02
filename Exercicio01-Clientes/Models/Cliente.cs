using System;

namespace Exercicio01Clientes.Models
{
    public class Cliente
    {
        // ===== Campos privados (os dados protegidos) =====
        private int _id;
        private string _nome;
        private string _cpf;
        private string _email;
        private string _telefone;
        private DateTime _dataCadastro;

        // ===== Propriedades (as "portas" de acesso aos dados) =====
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
                    throw new ArgumentException("O nome não pode ser vazio.");
                _nome = value;
            }
        }

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Telefone
        {
            get { return _telefone; }
            set { _telefone = value; }
        }

        public DateTime DataCadastro
        {
            get { return _dataCadastro; }
            set { _dataCadastro = value; }
        }
    }
}