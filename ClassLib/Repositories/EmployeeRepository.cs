using ClassLib.models;
using ClassLib.Repositories.Interfaces;
using NHibernate;

namespace ClassLib.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ISession session) : base(session) { }

    public IList<Employee> GetManagers()
    {
        return _session.Query<Employee>()
            .Where(e => e.Position == Position.Manager)
            .ToList();
    }

    public Employee GetByFullName(string fullName)
    {
        return _session.Query<Employee>()
            .FirstOrDefault(e => e.FullName == fullName);
    }
}