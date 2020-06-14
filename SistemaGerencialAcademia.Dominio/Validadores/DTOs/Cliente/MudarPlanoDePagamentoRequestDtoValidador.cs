using FluentValidation;
using SistemaGerencialAcademia.Dominio.DTOs.Cliente;

namespace SistemaGerencialAcademia.Dominio.Validadores.DTOs.Cliente
{
    public class MudarPlanoDePagamentoRequestDtoValidador : AbstractValidator<MudarPlanoDePagamentoRequestDto>
    {
        public MudarPlanoDePagamentoRequestDtoValidador()
        {
            RuleFor(m => m.ClienteId).NotEmpty();
        }
    }
}
