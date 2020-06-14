using SistemaGerencialAcademia.Data.Context;
using SistemaGerencialAcademia.Data.Repositorios.Base;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;

namespace SistemaGerencialAcademia.Data.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(AcademiaDbContext context) : base(context)
        {
        }
    }
}
