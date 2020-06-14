using SistemaGerencialAcademia.CrossCutting.Constantes;
using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.CrossCutting.Enum;
using SistemaGerencialAcademia.CrossCutting.Util;
using SistemaGerencialAcademia.Dominio.DTOs.Base;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;
using SistemaGerencialAcademia.Dominio.Interfaces.Servicos;
using SistemaGerencialAcademia.Dominio.Validadores.DTOs.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Servicos.Servicos
{
    public class ClienteServico : IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;

        public ClienteServico(IClienteRepositorio clienteRepositorio,
                              IEnderecoRepositorio enderecoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _enderecoRepositorio = enderecoRepositorio;
        }

        public async Task<Cliente> BuscarClientePorId(Guid clienteId)
        {
            var cliente = await _clienteRepositorio.GetById(clienteId);

            if (cliente is null)
                return new Cliente();

            return cliente;
        }

        public async Task<NovoClienteResponseDto> CadastrarCliente(NovoClienteRequestDto novoCliente)
        {
            var validacao = Validacoes.ValidarObjeto(new NovoClienteRequestDtoValidador().Validate(novoCliente));

            if (!validacao.objetoValido)
                return new NovoClienteResponseDto
                {
                    ClienteId = Guid.Empty,
                    Sucesso = false,
                    Mensagem = validacao.erros
                };

            var clienteExistente = await _clienteRepositorio.Search(c => c.CPF == novoCliente.CPF.RemoverDigitosNaoNumericos() || c.Identidade == novoCliente.Identidade.RemoverDigitosNaoNumericos());

            if(!clienteExistente.Any())
            {
                var numeroDeClientes = (await _clienteRepositorio.GetAll()).Count();

                var cliente = new Cliente
                {
                    Nome = novoCliente.Nome,
                    NumeroDeMatricula = Convert.ToInt64(Constantes.CODIGO_DA_UNIDADE + numeroDeClientes.ToString()),
                    CPF = novoCliente.CPF.RemoverDigitosNaoNumericos(),
                    Identidade = novoCliente.Identidade.RemoverDigitosNaoNumericos(),
                    PlanoDePagamento = PlanoDePagamento.Mensal,
                    UltimaAvaliacaoFisica = DateTime.Now.Date,
                    VencimentoPagamento = DateTime.Now.AddMonths(1).Date
                };

                await _clienteRepositorio.Add(cliente);

                var endereco = new Endereco
                {
                    ClienteId = cliente.Id,
                    Rua = novoCliente.Rua,
                    Bairro = novoCliente.Bairro,
                    Numero = novoCliente.Numero,
                    Complemento = novoCliente.Complemento
                };

                await _enderecoRepositorio.Add(endereco);

                cliente.EnderecoId = endereco.Id;

                await _clienteRepositorio.Update(cliente);

                return new NovoClienteResponseDto
                {
                    ClienteId = cliente.Id,
                    Sucesso = true,
                    Mensagem = ClienteMensagens.CLIENTE_CADASTRADO
                };
            }

            return new NovoClienteResponseDto
            {
                ClienteId = Guid.Empty,
                Sucesso = false,
                Mensagem = ClienteMensagens.DOCUMENTO_JA_CADASTRADO
            };
        }

        public async Task<ResponseDto> MudarPlanoDePagamentoDoCliente(MudarPlanoDePagamentoRequestDto planoDePagamento)
        {
            var validacao = Validacoes.ValidarObjeto(new MudarPlanoDePagamentoRequestDtoValidador().Validate(planoDePagamento));

            if (!validacao.objetoValido)
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = validacao.erros
                };

            var cliente = await _clienteRepositorio.GetById(planoDePagamento.ClienteId);

            if(cliente is null)
                return new ResponseDto
                {
                    Sucesso = false,
                    Mensagem = ClienteMensagens.CLIENTE_NAO_ENCONTRADO
                };

            if(cliente.PlanoDePagamento != planoDePagamento.PlanoDePagamento)
            {
                cliente.PlanoDePagamento = planoDePagamento.PlanoDePagamento;

                cliente.VencimentoPagamento = NovaDataDePagamento(planoDePagamento.PlanoDePagamento);

                await _clienteRepositorio.Update(cliente);
            }

            return new ResponseDto
            {
                Sucesso = true,
                Mensagem = ClienteMensagens.PLANO_DE_PAGAMAENTO_ALTERADO
            };
        }

        public async Task<IEnumerable<Cliente>> PesquisarClientes(PesquisarClienteRequestDto pesquisarCliente)
        {
            var validacao = Validacoes.ValidarObjeto(new PesquisarClienteRequestDtoValidador().Validate(pesquisarCliente));

            var clientes = new List<Cliente>();

            if (!validacao.objetoValido)
                return clientes;

            if (string.IsNullOrEmpty(pesquisarCliente.Nome) && string.IsNullOrEmpty(pesquisarCliente.CPF))
            {
                clientes = (await _clienteRepositorio.GetAll()).ToList();
                clientes = await RetornarCleintesComEndereco(clientes);
            }
            else if (string.IsNullOrEmpty(pesquisarCliente.Nome))
            {
                clientes = (await _clienteRepositorio.Search(c => c.CPF == pesquisarCliente.CPF)).ToList();
                clientes = await RetornarCleintesComEndereco(clientes);
            }
            else if(string.IsNullOrEmpty(pesquisarCliente.CPF))
            {
                clientes = (await _clienteRepositorio.Search(c => c.Nome.Contains(pesquisarCliente.Nome))).ToList();
                clientes = await RetornarCleintesComEndereco(clientes);
            }              
            else
            {
                clientes = (await _clienteRepositorio.Search(c => c.Nome.Contains(pesquisarCliente.Nome) &&
                                                         c.CPF == pesquisarCliente.CPF)).ToList();
                clientes = await RetornarCleintesComEndereco(clientes);
            }

            return clientes;

        }

        public async Task AtualizarDataVencimentoPagamento(Guid clienteId)
        {
            var cliente = await BuscarClientePorId(clienteId);

            cliente.VencimentoPagamento = NovaDataDePagamento(cliente.PlanoDePagamento);

            await _clienteRepositorio.Update(cliente);
        }

        public async Task AtualizarDataVencimentoPagamento(Guid clienteId, int numeroDeDias)
        {
            var cliente = await BuscarClientePorId(clienteId);

            cliente.VencimentoPagamento = cliente.VencimentoPagamento.AddDays(numeroDeDias);

            await _clienteRepositorio.Update(cliente);
        }

        private async Task<List<Cliente>> RetornarCleintesComEndereco(List<Cliente> clientes)
        {
            foreach(var cliente in clientes)
            {
                var endereco = await _enderecoRepositorio.GetById(cliente.EnderecoId);

                cliente.Endereco = endereco;
            }

            return clientes;
        }

        private DateTime NovaDataDePagamento(PlanoDePagamento planoDePagamento)
        {
            return planoDePagamento switch
            {
                PlanoDePagamento.Mensal => DateTime.Now.AddMonths(1).Date,
                PlanoDePagamento.Anual => DateTime.Now.AddMonths(12).Date
            };
        }
    }
}
