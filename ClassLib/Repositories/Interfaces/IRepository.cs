using System.Linq.Expressions;

namespace ClassLib.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    T GetById(int id);
    IList<T> GetAll();
    IList<T> Find(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void SaveOrUpdate(T entity);
}
