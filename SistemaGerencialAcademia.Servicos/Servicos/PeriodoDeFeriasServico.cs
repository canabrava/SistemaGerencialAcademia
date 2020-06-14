using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.CrossCutting.Util;
using SistemaGerencialAcademia.Dominio.DTOs.Base;
using SistemaGerencialAcademia.Dominio.DTOs.PeriodoDeFerias;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;
using SistemaGerencialAcademia.Dominio.Interfaces.Servicos;
using SistemaGerencialAcademia.Dominio.Validadores.DTOs.PeriodoDeFerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Servicos.Servicos
{
    public class PeriodoDeFeriasServico : IPeriodoDeFeriasServico
    {

        private readonly IPeriodoDeFeriasRepositorio _periodoDeFeriasRepositorio;

        public PeriodoDeFeriasServico(IPeriodoDeFeriasRepositorio periodoDeFeriasRepositorio)
        {
            _periodoDeFeriasRepositorio = periodoDeFeriasRepositorio;
        }

        public async Task<IEnumerable<PeriodoDeFerias>> BuscarPeriodosDeFeriasDoAno(Guid clienteId)
        {
            var periodosDeFeriasDoCliente = await _periodoDeFeriasRepositorio.Search(p => p.ClientId == clienteId);

            if(periodosDeFeriasDoCliente is null)
            {
                return new List<PeriodoDeFerias>();
            }

            return periodosDeFeriasDoCliente.Where(p => p.DataInicio.Year == DateTime.Now.Year);
        }

        public async Task<ResponseDto> SalvarNovoPeriodoDeFerias(NovoPeriodoDeFeriasRequestDto novoPeriodoDeFerias, DateTime dataUltimoPagamento)
        {
            var validacao = Validacoes.ValidarObjeto(new NovoPeriodoDeFeriasRequestDtoValidador().Validate(novoPeriodoDeFerias));

            if (!validacao.objetoValido)
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = validacao.erros
                };

            if(novoPeriodoDeFerias.DataFim.Date > dataUltimoPagamento.AddDays(360).Date)
            {
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = PeriodoDeFeriasMensagens.PERIODO_DE_FERIAS_FORA_DO_PERIODO_CORRENTE
                };
            }

            var periodoDeFerias = new PeriodoDeFerias
            {
                ClientId = novoPeriodoDeFerias.ClienteId,
                DataInicio = novoPeriodoDeFerias.DataInicio,
                DataFim = novoPeriodoDeFerias.DataFim
            };

            var periodosDeFeriasDoAno = (await _periodoDeFeriasRepositorio.Search(p => p.ClientId == novoPeriodoDeFerias.ClienteId))
                .Where(p => p.DataInicio.Ticks >= dataUltimoPagamento.Ticks);

            if(periodosDeFeriasDoAno.Count() == 3)
            {
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = PeriodoDeFeriasMensagens.QUANDTIDADE_MAXIMA_DE_PERIODOS_DE_FERIAS
                };
            }

            if(NumeroDeDiasDeFeriasAposNovoPeriodo(periodosDeFeriasDoAno, periodoDeFerias) > 30)
            {
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = PeriodoDeFeriasMensagens.QUANTIDADE_MAXIMA_DE_DIAS_DE_FERIAS
                };
            }

            await _periodoDeFeriasRepositorio.Add(periodoDeFerias);

            return new ResponseDto
            {
                Sucesso = true,
                Mensagem = PeriodoDeFeriasMensagens.PERIODO_DE_FERIAS_SALVO
            };
        }

        private int NumeroDeDiasDeFeriasAposNovoPeriodo(IEnumerable<PeriodoDeFerias> feriasAnteriores, PeriodoDeFerias novoPeriodoDeFerias)
        {
            var diasDeFerias = 0;

            foreach(var periodo in feriasAnteriores)
            {
                diasDeFerias += (periodo.DataFim - periodo.DataInicio).Days;
            }

            diasDeFerias += (novoPeriodoDeFerias.DataFim - novoPeriodoDeFerias.DataInicio).Days;

            return diasDeFerias;
        }
    }
}
