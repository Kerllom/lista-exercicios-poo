using System;

namespace Exercicio03Agendamento.Models
{
    public class Medico
    {
        private int _id;
        private string _nome;
        private string _especialidade;

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
                    throw new ArgumentException("O nome do médico não pode ser vazio.");
                _nome = value;
            }
        }

        public string Especialidade
        {
            get { return _especialidade; }
            set { _especialidade = value; }
        }
    }
}