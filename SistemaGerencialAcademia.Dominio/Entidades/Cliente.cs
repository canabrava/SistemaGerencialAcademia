using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;

namespace SistemaGerencialAcademia.Dominio.Entidades
{
    public class Cliente : Entititade
    {
        public Guid EnderecoId { get; set; }
        public long NumeroDeMatricula { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Identidade { get; set; }
        public DateTime VencimentoPagamento { get; set; }
        public DateTime UltimaAvaliacaoFisica { get; set; }
        public PlanoDePagamento PlanoDePagamento { get; set; }

        public Endereco Endereco { get; set; }

        public ICollection<InformacoesPagamento> InformacoesPagamentos { get; set; }

        public ICollection<PeriodoDeFerias> PeriodosDeFerias { get; set; }

        public bool EstaInadinplente()
        {
            return VencimentoPagamento < DateTime.Now;
        }

        public bool DeveRealizarAvaliacaoFisca()
        {
            return UltimaAvaliacaoFisica < DateTime.Now.AddMonths(-6);
        }

        public bool PodeTerPeriodoDeFerias()
        {
            return PlanoDePagamento == PlanoDePagamento.Anual;
        }
    }
}
