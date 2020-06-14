using SistemaGerencialAcademia.Dominio.DTOs.Base;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;
using SistemaGerencialAcademia.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Dominio.Interfaces.Servicos
{
    public interface IClienteServico
    {
        Task<NovoClienteResponseDto> CadastrarCliente(NovoClienteRequestDto novoCliente);
        Task<IEnumerable<Cliente>> PesquisarClientes(PesquisarClienteRequestDto pesquisarCliente);
        Task<ResponseDto> MudarPlanoDePagamentoDoCliente(MudarPlanoDePagamentoRequestDto planoDePagamento);
        Task<Cliente> BuscarClientePorId(Guid clienteId);
        Task AtualizarDataVencimentoPagamento(Guid clienteId);
        Task AtualizarDataVencimentoPagamento(Guid clienteId, int numeroDeDias);
    }
}
