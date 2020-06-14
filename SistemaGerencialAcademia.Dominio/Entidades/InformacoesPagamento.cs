using SistemaGerencialAcademia.Dominio.Entidades.Base;
using System;

namespace SistemaGerencialAcademia.Dominio.Entidades
{
    public class InformacoesPagamento : Entititade
    {
        public Guid ClienteId { get; set; }
        public DateTime DataPagamento { get; set; }

        public Cliente Cliente { get; set; }
    }
}
