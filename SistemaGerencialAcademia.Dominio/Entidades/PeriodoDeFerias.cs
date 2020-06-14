using SistemaGerencialAcademia.Dominio.Entidades.Base;
using System;

namespace SistemaGerencialAcademia.Dominio.Entidades
{
    public class PeriodoDeFerias : Entititade
    {
        public Guid ClientId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public Cliente Cliente { get; set; }

        public int DiasDeFerias()
        {
            return (DataInicio - DataFim).Days;
        }
    }
}
