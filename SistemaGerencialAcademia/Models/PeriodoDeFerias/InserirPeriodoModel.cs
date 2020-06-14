using SistemaGerencialAcademia.Models.Base;
using System;
using System.Collections.Generic;

namespace SistemaGerencialAcademia.Models.PeriodoDeFerias
{
    public class PeriodoDeFeriasModel : ResponseBaseModel
    {
        public Guid ClienteId { get; set; }
        public DateTime DataInicioNovoPeriodo { get; set; }
        public DateTime DataFimNovoPeriodo { get; set; }

        public List<InformacoesPeriodoDeFeriasModel> PeriodosDeFeriasAnteriores { get; set; }
    }
}
