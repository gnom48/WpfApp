using System.Linq.Expressions;
using ClassLib.Repositories.Interfaces;
using NHibernate;

namespace ClassLib.Repositories;

public class BaseRepository<T>(ISession session) : IRepository<T> where T : class
{
    public ISession Session { get; } = session;

    public virtual T GetById(int id)
    {
        return Session.Get<T>(id);
    }

    public virtual IList<T> GetAll()
    {
        return Session.Query<T>().ToList();
    }

    public virtual IList<T> Find(Expression<Func<T, bool>> predicate)
    {
        return Session.Query<T>().Where(predicate).ToList();
    }

    public virtual void Add(T entity)
    {
        using (var transaction = Session.BeginTransaction())
        {
            Session.Save(entity);
            transaction.Commit();
        }
    }

    public virtual void Update(T entity)
    {
        using (var transaction = Session.BeginTransaction())
        {
            Session.Update(entity);
            transaction.Commit();
        }
    }

    public virtual void Delete(T entity)
    {
        using (var transaction = Session.BeginTransaction())
        {
            Session.Delete(entity);
            transaction.Commit();
        }
    }

    public virtual void SaveOrUpdate(T entity)
    {
        using (var transaction = Session.BeginTransaction())
        {
            Session.SaveOrUpdate(entity);
            transaction.Commit();
        }
    }
}
