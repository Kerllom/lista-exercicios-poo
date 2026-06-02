using System;

namespace Exercicio03Agendamento.Models
{
    public class Consulta
    {
        private int _id;
        private int _pacienteId;
        private int _medicoId;
        private DateTime _dataHora;
        private string _status;

        // Referências aos objetos completos (preenchidas via JOIN)
        private Paciente _paciente;
        private Medico _medico;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int PacienteId
        {
            get { return _pacienteId; }
            set { _pacienteId = value; }
        }

        public int MedicoId
        {
            get { return _medicoId; }
            set { _medicoId = value; }
        }

        public DateTime DataHora
        {
            get { return _dataHora; }
            set { _dataHora = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Paciente Paciente
        {
            get { return _paciente; }
            set { _paciente = value; }
        }

        public Medico Medico
        {
            get { return _medico; }
            set { _medico = value; }
        }
    }
}