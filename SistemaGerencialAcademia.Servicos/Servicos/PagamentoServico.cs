using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.Dominio.DTOs.Base;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;
using SistemaGerencialAcademia.Dominio.Interfaces.Servicos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Servicos.Servicos
{
    public class PagamentoServico : IPagamentoServico
    {

        private readonly IInformacoesPagamentoRepositorio _informacoesPagamentoRepositorio;

        public PagamentoServico(IInformacoesPagamentoRepositorio informacoesPagamentoRepositorio)
        {
            _informacoesPagamentoRepositorio = informacoesPagamentoRepositorio;
        }

        public async Task<DateTime> BuscarUltimoPagamentoDoCliente(Guid clienteId)
        {
            var todosOsPagamentos = await _informacoesPagamentoRepositorio.Search(i => i.ClienteId == clienteId);

            if(!todosOsPagamentos.Any())
            {
                return new DateTime();
            }

            var ultimoPagamento = todosOsPagamentos.Select(t => t.DataPagamento)
                .OrderBy(dataPagamento => dataPagamento)
                .TakeLast(1)
                .Single();

            return ultimoPagamento;
        }

        public async Task<ResponseDto> SalvarNovoPagamento(Guid clienteId, PlanoDePagamento planoDoCliente)
        {
            var ultimoPagamento = await BuscarUltimoPagamentoDoCliente(clienteId);

            if(planoDoCliente == PlanoDePagamento.Mensal && 
                (ultimoPagamento.Month == DateTime.Now.Month && 
                 ultimoPagamento.Year == DateTime.Now.Year))
            {
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = InformacoesPagamentoMensagens.PAGAMENTO_JA_REALIZADO
                };
            }
            else if(planoDoCliente == PlanoDePagamento.Anual && DateTime.Now.Date < ultimoPagamento.AddDays(360).Date)
            {
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = InformacoesPagamentoMensagens.PAGAMENTO_JA_REALIZADO
                };
            }

            var novoPagamento = new InformacoesPagamento
            {
                ClienteId = clienteId,
                DataPagamento = DateTime.Now.Date
            };

            await _informacoesPagamentoRepositorio.Add(novoPagamento);

            return new ResponseDto
            {
                Sucesso = true,
                Mensagem = InformacoesPagamentoMensagens.PAGAMENTO_SALVO
            };
        }
    }
}
