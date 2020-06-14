using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.CrossCutting.Util;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;
using SistemaGerencialAcademia.Dominio.DTOs.PeriodoDeFerias;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Servicos;
using SistemaGerencialAcademia.Extensoes;
using SistemaGerencialAcademia.Models;
using SistemaGerencialAcademia.Models.Base;
using SistemaGerencialAcademia.Models.Cliente;
using SistemaGerencialAcademia.Models.InformacoesPagamento;
using SistemaGerencialAcademia.Models.PeriodoDeFerias;

namespace SistemaGerencialAcademia.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteServico _clienteServico;
        private readonly IPagamentoServico _pagamentoServico;
        private readonly IPeriodoDeFeriasServico _periodoDeFeriasServico;


        public ClienteController(ILogger<ClienteController> logger,
                                 IClienteServico clienteServico,
                                 IPagamentoServico pagamentoServico,
                                 IPeriodoDeFeriasServico periodoDeFeriasServico)
        {
            _logger = logger;

            _clienteServico = clienteServico;
            _pagamentoServico = pagamentoServico;
            _periodoDeFeriasServico = periodoDeFeriasServico;
        }

        public async Task<IActionResult> Index()
        {
            var pesquisarClienteModel = new PesquisarClienteModel
            {
                PesquisaRealizada = false,
                Clientes = new List<ClienteModel>()
            };

            return View(pesquisarClienteModel);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientes(PesquisarClienteModel pesquisarClienteModel)
        {
            var pesquisaClientesRequest = new PesquisarClienteRequestDto
            {
                Nome = pesquisarClienteModel.Nome,
                CPF = pesquisarClienteModel.CPF.RemoverDigitosNaoNumericos(),
            };

            var clientes = await _clienteServico.PesquisarClientes(pesquisaClientesRequest);

            var clientesModel = new List<ClienteModel>();

            if (clientes.Any())
            {
                clientesModel = clientes
                  .Select(cliente => MontarClineteModel(cliente))
                  .OrderBy(c => c.Nome)
                  .ToList();

                HttpContext.Session.SetObject("clientes", clientesModel);

                HttpContext.Session.SetInt32("pagina", 1);

            }

            pesquisarClienteModel.Clientes = clientesModel.Take(10);

            pesquisarClienteModel.PesquisaRealizada = true;

            return View("Index", pesquisarClienteModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaginarMaisClientes()
        {
            var pagina = HttpContext.Session.GetInt32("pagina").HasValue ? (int)HttpContext.Session.GetInt32("pagina") + 1 : 1;

            var clientes = HttpContext.Session.GetObject<IEnumerable<ClienteModel>>("clientes");

            HttpContext.Session.SetInt32("pagina", pagina);

            var clientesPaginados = clientes.Take(pagina * 10);

            return PartialView("_resultadoPesquisa", clientesPaginados);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(NovoClienteModel novoCliente)
        {
            var infoClientes = new NovoClienteRequestDto
            {
                Nome = novoCliente.Nome,
                CPF = novoCliente.CPF,
                Identidade = novoCliente.Identidade,
                Bairro = novoCliente.Bairro,
                Rua = novoCliente.Rua,
                Numero = novoCliente.Numero,
                Complemento = novoCliente.Complemento
            };

            var resultado = await _clienteServico.CadastrarCliente(infoClientes);

            if (resultado.Sucesso)
            {
                await _pagamentoServico.SalvarNovoPagamento(resultado.ClienteId, PlanoDePagamento.Mensal);
            }

            return PartialView("_resultadoOperacao", new ResponseBaseModel
            {
                Sucesso = resultado.Sucesso,
                Mensagens = MontarListaMensagens(resultado.Mensagem)
            });     
        }

        [HttpGet]
        public async Task<IActionResult> BuscarInformacoesDePagamento(Guid id)
        {
            var ultimoPagamento = await _pagamentoServico.BuscarUltimoPagamentoDoCliente(id);

            var cliente = await _clienteServico.BuscarClientePorId(id);

            var informacoesPagamentoModelo = new InformacoesPagamentoModel
            {
                IdCliente = id,
                ProximoPagamento = cliente.VencimentoPagamento.Date,
                UltimoPagamento = ultimoPagamento.Date,
                PlanoDePagamento = cliente.PlanoDePagamento
            };

            return PartialView("_informacoesDePagamento", informacoesPagamentoModelo);
        }

        [HttpPut]
        public async Task<IActionResult> AlterarPlanoDePagamento(Guid id, PlanoDePagamento planoDePagamento)
        {
            var resultado = await _clienteServico.MudarPlanoDePagamentoDoCliente(new MudarPlanoDePagamentoRequestDto 
            {
                ClienteId = id,
                PlanoDePagamento = planoDePagamento
            });

            var cliente = await _clienteServico.BuscarClientePorId(id);

            var ultimoPagamento = await _pagamentoServico.BuscarUltimoPagamentoDoCliente(id);

            var informacoesPagamentoModelo = new InformacoesPagamentoModel
            {
                Sucesso = resultado.Sucesso,
                Mensagens = MontarListaMensagens(resultado.Mensagem),
                IdCliente = cliente.Id,
                ProximoPagamento = cliente.VencimentoPagamento,
                UltimoPagamento = ultimoPagamento,
                PlanoDePagamento = cliente.PlanoDePagamento
            };

            return PartialView("_informacoesDePagamento", informacoesPagamentoModelo);
        }

        [HttpPost]
        public async Task<IActionResult> InserirNovoPagamento(Guid id)
        {
            var dataPagamento = DateTime.Now.Date;

            var cliente = await _clienteServico.BuscarClientePorId(id);

            var resultado = await _pagamentoServico.SalvarNovoPagamento(id, cliente.PlanoDePagamento);

            if (resultado.Sucesso)
            {
                await _clienteServico.AtualizarDataVencimentoPagamento(id);
            }

            var informacoesPagamentoModelo = new InformacoesPagamentoModel
            {
                Sucesso = resultado.Sucesso,
                Mensagens = MontarListaMensagens(resultado.Mensagem),
                IdCliente = cliente.Id,
                ProximoPagamento = cliente.VencimentoPagamento,
                UltimoPagamento = dataPagamento,
                PlanoDePagamento = cliente.PlanoDePagamento
            };


            return PartialView("_informacoesDePagamento", informacoesPagamentoModelo);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarInformacoesDePeriodoDeFerias(Guid id)
        {
            var informacoesPeriodoDeFerias = await _periodoDeFeriasServico.BuscarPeriodosDeFeriasDoAno(id);

            var periodoDeFeriasModel = new PeriodoDeFeriasModel
            {
                ClienteId = id,
                Sucesso = true,
                PeriodosDeFeriasAnteriores = new List<InformacoesPeriodoDeFeriasModel>()
            };

            foreach (var periodo in informacoesPeriodoDeFerias)
                periodoDeFeriasModel.PeriodosDeFeriasAnteriores.Add(new InformacoesPeriodoDeFeriasModel
                {
                    DataInicio = periodo.DataInicio,
                    DataFim = periodo.DataFim
                });

            periodoDeFeriasModel.PeriodosDeFeriasAnteriores = periodoDeFeriasModel.PeriodosDeFeriasAnteriores.OrderBy(p => p.DataInicio).ToList();

            return PartialView("_periodoDeFerias", periodoDeFeriasModel);
        }

        [HttpPost]
        public async Task<IActionResult> InserirNovoPeriodoDeFerias(PeriodoDeFeriasModel periodoDeFeriasModel)
        {
            var periodoDeFeriasDTO = new NovoPeriodoDeFeriasRequestDto
            {
                ClienteId = periodoDeFeriasModel.ClienteId,
                DataInicio = periodoDeFeriasModel.DataInicioNovoPeriodo,
                DataFim = periodoDeFeriasModel.DataFimNovoPeriodo
            };

            var dataUltimoPagamento = (await _clienteServico.BuscarClientePorId(periodoDeFeriasModel.ClienteId))
                .UltimaAvaliacaoFisica;

            var diasDeFerias = (periodoDeFeriasModel.DataFimNovoPeriodo - periodoDeFeriasModel.DataInicioNovoPeriodo).Days;

            var resultado = await _periodoDeFeriasServico.SalvarNovoPeriodoDeFerias(periodoDeFeriasDTO, dataUltimoPagamento);

            if(resultado.Sucesso)
                await _clienteServico.AtualizarDataVencimentoPagamento(periodoDeFeriasModel.ClienteId, diasDeFerias);

            var informacoesPeriodoDeFerias = await _periodoDeFeriasServico.BuscarPeriodosDeFeriasDoAno(periodoDeFeriasModel.ClienteId);

            periodoDeFeriasModel.PeriodosDeFeriasAnteriores = new List<InformacoesPeriodoDeFeriasModel>();

            foreach (var periodo in informacoesPeriodoDeFerias)
                periodoDeFeriasModel.PeriodosDeFeriasAnteriores.Add(new InformacoesPeriodoDeFeriasModel
                {
                    DataInicio = periodo.DataInicio,
                    DataFim = periodo.DataFim
                });

            periodoDeFeriasModel.PeriodosDeFeriasAnteriores = periodoDeFeriasModel.PeriodosDeFeriasAnteriores.OrderBy(p => p.DataInicio).ToList();

            periodoDeFeriasModel.Sucesso = resultado.Sucesso;
            periodoDeFeriasModel.Mensagens = MontarListaMensagens(resultado.Mensagem);


            return PartialView("_periodoDeFerias", periodoDeFeriasModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private ClienteModel MontarClineteModel(Cliente cliente)
        {
            var modelo = new ClienteModel
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                Identidade = cliente.Identidade,
                Endereco = string.IsNullOrEmpty(cliente.Endereco.Complemento) ? 
                    cliente.Endereco.Rua + ", " + cliente.Endereco.Numero : 
                    cliente.Endereco.Rua + ", " + cliente.Endereco.Numero + "/" + cliente.Endereco.Complemento,
                PodeTerPeriodoDeFerias = cliente.PodeTerPeriodoDeFerias()
            };

            return modelo;
        }

        private List<string> MontarListaMensagens(string mensagens)
        {
            return mensagens.Split("\n").ToList();
        }
    }
}
