using System;

namespace Exercicio04Bancario.Models
{
    public class ContaBancaria
    {
        private int _id;
        private string _numero;
        private decimal _saldo;
        private int _correntistaId;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Numero
        {
            get { return _numero; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O número da conta não pode ser vazio.");
                _numero = value;
            }
        }

        public decimal Saldo
        {
            get { return _saldo; }
            set { _saldo = value; }
        }

        public int CorrentistaId
        {
            get { return _correntistaId; }
            set { _correntistaId = value; }
        }
    }
}