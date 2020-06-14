using SistemaGerencialAcademia.Data.Context;
using SistemaGerencialAcademia.Data.Repositorios.Base;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;

namespace SistemaGerencialAcademia.Data.Repositorios
{
    public class InformacoesPagamentoRepositorio : RepositorioBase<InformacoesPagamento>, IInformacoesPagamentoRepositorio
    {
        public InformacoesPagamentoRepositorio(AcademiaDbContext context) : base(context)
        {
        }
    }
}
