using MemberShip.Domain;
using System.Linq.Expressions;

namespace MemberShip.Application.IRepositories
{
    public interface IGenericrepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        bool IsExist(Expression<Func<TEntity, bool>> filter);
        TEntity GetEntityById(int entityid);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
