using SistemaGerencialAcademia.Data.Context;
using SistemaGerencialAcademia.Data.Repositorios.Base;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;

namespace SistemaGerencialAcademia.Data.Repositorios
{
    public class PeriodoDeFeriasRepositorio : RepositorioBase<PeriodoDeFerias>, IPeriodoDeFeriasRepositorio
    {
        public PeriodoDeFeriasRepositorio(AcademiaDbContext context) : base(context)
        {
        }
    }
}
