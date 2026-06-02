using System;

namespace Exercicio03Agendamento.Models
{
    public class Paciente
    {
        private int _id;
        private string _nome;
        private string _telefone;

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
                    throw new ArgumentException("O nome do paciente não pode ser vazio.");
                _nome = value;
            }
        }

        public string Telefone
        {
            get { return _telefone; }
            set { _telefone = value; }
        }
    }
}