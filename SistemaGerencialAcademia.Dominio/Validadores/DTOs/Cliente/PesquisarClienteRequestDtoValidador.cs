using FluentValidation;
using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;
using System.Linq;

namespace SistemaGerencialAcademia.Dominio.Validadores.DTOs.Cliente
{
    public class PesquisarClienteRequestDtoValidador : AbstractValidator<PesquisarClienteRequestDto>
    {
        public PesquisarClienteRequestDtoValidador()
        {
            RuleFor(p => p.CPF).ValidarPesquisaPorCpf();
        }
    }

    public static class PesquisarClienteRequestDtoValidationRules
    {
        public static IRuleBuilder<T, string> ValidarPesquisaPorCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((cpf, context) =>
            {
                if (!(cpf is null) && !(string.Empty == cpf))
                {
                    if (!cpf.All(char.IsDigit))
                        context.AddFailure(ClienteMensagens.CPF_INVALIDO);
                }

            });
        }
    }

}
