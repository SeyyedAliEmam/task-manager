using MemberShip.Application.IRepositories;
using MemberShip.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemberShip.Infrastructure.Repositories
{
    public class Genericrepository<TEntity> : IGenericrepository<TEntity> where TEntity : BaseEntity
    {

        private readonly AppContext context;
        private readonly DbSet<TEntity> dbset;
        public Genericrepository(AppContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

       public bool IsExist(Expression<Func<TEntity, bool>> filter)
        {
            return dbset.Any(filter);
        }

        public TEntity GetEntityById(int entityid)
        {
            return dbset.Find(entityid);
        }

        public void Insert(TEntity entity)
        {
            dbset.Add(entity);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            dbset?.Remove(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
