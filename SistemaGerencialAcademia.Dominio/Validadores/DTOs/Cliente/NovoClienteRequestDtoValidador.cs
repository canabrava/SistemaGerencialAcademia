using FluentValidation;
using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.CrossCutting.Util;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;
using System.Linq;

namespace SistemaGerencialAcademia.Dominio.Validadores.DTOs.Cliente
{
    public class NovoClienteRequestDtoValidador : AbstractValidator<NovoClienteRequestDto>
    {
        public NovoClienteRequestDtoValidador()
        {
            RuleFor(n => n.Nome).NotEmpty();
            RuleFor(n => n.CPF).ValidarCPF();
            RuleFor(n => n.Identidade).NotEmpty();
            RuleFor(n => n.Rua).NotEmpty();
            RuleFor(n => n.Bairro).NotEmpty();
            RuleFor(n => n.Numero).ValidarNumero();
        }
    }

    public static class NovoClienteValidationRules
    {
        public static IRuleBuilderInitial<T, string> ValidarCPF<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((cpf, context) =>
            {
                if (string.IsNullOrEmpty(cpf))
                {
                    context.AddFailure(ClienteMensagens.CAMPO_CPF_OBRIGATORIO);
                }
                else if (!Validacoes.ValidaCPF(cpf))
                    context.AddFailure(ClienteMensagens.CPF_INVALIDO);
            });
        }

        public static IRuleBuilder<T, string>ValidarNumero<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((numero, context) =>
            {
                if (string.IsNullOrEmpty(numero))
                {
                    context.AddFailure(ClienteMensagens.CAMPO_NUMERO_OBRIGADORIO);
                }
                else if (!numero.All(char.IsDigit))
                    context.AddFailure(ClienteMensagens.APENAS_VALORES_NUMERICOS);
            });
        }
    }
}
