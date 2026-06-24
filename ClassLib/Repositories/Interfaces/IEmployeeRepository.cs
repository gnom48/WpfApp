using ClassLib.models;

namespace ClassLib.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    IList<Employee> GetManagers();
}
