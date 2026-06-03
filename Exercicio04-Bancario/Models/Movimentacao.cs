using System;

namespace Exercicio04Bancario.Models
{
    public class Movimentacao
    {
        private int _id;
        private int _contaId;
        private string _tipo;
        private decimal _valor;
        private DateTime _dataHora;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int ContaId
        {
            get { return _contaId; }
            set { _contaId = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public decimal Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        public DateTime DataHora
        {
            get { return _dataHora; }
            set { _dataHora = value; }
        }
    }
}