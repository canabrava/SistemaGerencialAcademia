using Microsoft.EntityFrameworkCore;
using SistemaGerencialAcademia.Data.Context;
using SistemaGerencialAcademia.Dominio.Entidades.Base;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGerencialAcademia.Data.Repositorios.Base
{
     public abstract class RepositorioBase<TEntity> : IDisposable, IRepositorioBase<TEntity> where TEntity : Entititade, new()
     {
        protected readonly AcademiaDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public RepositorioBase(AcademiaDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task Add(TEntity obj)
        {
            DbSet.Add(obj);
            await SaveChanges();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task Remove(Guid id)
        {
            DbSet.Remove(new TEntity {Id = id});
            await SaveChanges();
        }

        public virtual async Task Update(TEntity obj)
        {
            DbSet.Update(obj);
            await SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        protected async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
      
    }
}
