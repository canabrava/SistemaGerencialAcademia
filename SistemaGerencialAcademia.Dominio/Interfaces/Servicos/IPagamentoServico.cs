using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.Dominio.DTOs.Base;
using System;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Dominio.Interfaces.Servicos
{
    public interface IPagamentoServico
    {
        Task<DateTime> BuscarUltimoPagamentoDoCliente(Guid clienteId);
        Task<ResponseDto> SalvarNovoPagamento(Guid clienteId, PlanoDePagamento planoDoCliente);
    }
}
