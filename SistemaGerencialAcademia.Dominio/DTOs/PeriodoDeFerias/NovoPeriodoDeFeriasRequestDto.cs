using System;

namespace SistemaGerencialAcademia.Dominio.DTOs.PeriodoDeFerias
{
    public class NovoPeriodoDeFeriasRequestDto
    {
        public Guid ClienteId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
