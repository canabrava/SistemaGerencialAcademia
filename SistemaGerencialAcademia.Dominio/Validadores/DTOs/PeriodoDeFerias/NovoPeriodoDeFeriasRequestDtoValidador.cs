using FluentValidation;
using SistemaGerencialAcademia.CrossCutting.Constantes.Mensagens;
using SistemaGerencialAcademia.Dominio.DTOs.PeriodoDeFerias;
using System;

namespace SistemaGerencialAcademia.Dominio.Validadores.DTOs.PeriodoDeFerias
{
    public class NovoPeriodoDeFeriasRequestDtoValidador : AbstractValidator<NovoPeriodoDeFeriasRequestDto>
    {
        public NovoPeriodoDeFeriasRequestDtoValidador()
        {
            RuleFor(n => n.ClienteId).NotEmpty();
            RuleFor(n => n.DataInicio).NotEmpty();
            RuleFor(n => n.DataFim).NotEmpty();
            RuleFor(novoPeriodoDeFerias => novoPeriodoDeFerias).ValidarDataDoPeriodoDeFerias();
        }
    }

    public static class NovoPeriodoDeFeriasRequestDtoValidationRules
    {
        public static IRuleBuilder<T, NovoPeriodoDeFeriasRequestDto> ValidarDataDoPeriodoDeFerias<T>(this IRuleBuilder<T, NovoPeriodoDeFeriasRequestDto> ruleBuilder)
        {
            return ruleBuilder.Custom((periodoDeFerias, contexto) =>
            {
                if(periodoDeFerias.DataInicio < DateTime.Now.Date)
                {
                    contexto.AddFailure(PeriodoDeFeriasMensagens.DATA_PASSADA);
                }

                if(periodoDeFerias.DataInicio >= periodoDeFerias.DataFim)
                {
                    contexto.AddFailure(PeriodoDeFeriasMensagens.DATAS_INVALIDAS);
                }

                if((periodoDeFerias.DataFim - periodoDeFerias.DataInicio).Days > 30)
                {
                    contexto.AddFailure(PeriodoDeFeriasMensagens.PERIODO_DE_FERIAS_INVALIDO);
                }
            });
        }
    }
}
