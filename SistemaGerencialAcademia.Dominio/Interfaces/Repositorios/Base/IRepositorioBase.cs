using SistemaGerencialAcademia.Dominio.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Dominio.Interfaces.Repositorios.Base
{
    public interface IRepositorioBase<TEntity> where TEntity : Entititade
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity obj);
        Task Remove(Guid id);
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
        void Dispose();
    }
}
