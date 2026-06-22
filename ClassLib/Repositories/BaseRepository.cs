using System.Linq.Expressions;
using ClassLib.Repositories.Interfaces;
using MySqlX.XDevAPI;
using NHibernate;

namespace ClassLib.Repositories;

public class BaseRepository<T>(ISession session) : IRepository<T> where T : class
{
    public virtual T GetById(int id)
    {
        return session.Get<T>(id);
    }

    public virtual IList<T> GetAll()
    {
        return session.Query<T>().ToList();
    }

    public virtual IList<T> Find(Expression<Func<T, bool>> predicate)
    {
        return session.Query<T>().Where(predicate).ToList();
    }

    public virtual void Add(T entity)
    {
        using (var transaction = session.BeginTransaction())
        {
            session.Save(entity);
            transaction.Commit();
        }
    }

    public virtual void Update(T entity)
    {
        using (var transaction = session.BeginTransaction())
        {
            session.Update(entity);
            transaction.Commit();
        }
    }

    public virtual void Delete(T entity)
    {
        using (var transaction = session.BeginTransaction())
        {
            session.Delete(entity);
            transaction.Commit();
        }
    }

    public virtual void SaveOrUpdate(T entity)
    {
        using (var transaction = session.BeginTransaction())
        {
            session.SaveOrUpdate(entity);
            transaction.Commit();
        }
    }
}
