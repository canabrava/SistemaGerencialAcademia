using SistemaGerencialAcademia.CrossCutting.Enum;
using System;

namespace SistemaGerencialAcademia.Dominio.DTOs.Cliente
{
    public class MudarPlanoDePagamentoRequestDto
    {
        public Guid ClienteId { get; set; }
        public PlanoDePagamento PlanoDePagamento { get; set; }
    }
}
