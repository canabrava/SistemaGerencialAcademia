using SistemaGerencialAcademia.Dominio.DTOs.Base;
using SistemaGerencialAcademia.Dominio.DTOs.PeriodoDeFerias;
using SistemaGerencialAcademia.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Dominio.Interfaces.Servicos
{
    public interface IPeriodoDeFeriasServico
    {
        public Task<IEnumerable<PeriodoDeFerias>> BuscarPeriodosDeFeriasDoAno(Guid clienteId);
        public Task<ResponseDto> SalvarNovoPeriodoDeFerias(NovoPeriodoDeFeriasRequestDto novoPeriodoDeFerias, DateTime dataUltimoPagamento);
    }
}
