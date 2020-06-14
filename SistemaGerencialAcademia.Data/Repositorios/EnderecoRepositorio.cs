using SistemaGerencialAcademia.Data.Context;
using SistemaGerencialAcademia.Data.Repositorios.Base;
using SistemaGerencialAcademia.Dominio.Entidades;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;

namespace SistemaGerencialAcademia.Data.Repositorios
{
    public class EnderecoRepositorio : RepositorioBase<Endereco>, IEnderecoRepositorio
    {
        public EnderecoRepositorio(AcademiaDbContext context) : base(context)
        {
        }
    }
}
