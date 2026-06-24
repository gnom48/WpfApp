using ClassLib.models;
using ClassLib.Repositories.Interfaces;
using NHibernate;

namespace ClassLib.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ISession session) : base(session) { }

    public IList<Employee> GetManagers()
    {
        return Session.Query<Employee>()
            .Where(e => e.Position == Position.Руководитель)
            .ToList();
    }
}