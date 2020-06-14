using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.Models.Base;
using System;

namespace SistemaGerencialAcademia.Models.InformacoesPagamento
{
    public class InformacoesPagamentoModel : ResponseBaseModel
    {
        public Guid IdCliente { get; set; }
        public DateTime UltimoPagamento { get; set; }
        public DateTime ProximoPagamento { get; set; }
        public PlanoDePagamento PlanoDePagamento { get; set; }
    }
}
