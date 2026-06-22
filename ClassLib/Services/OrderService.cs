using ClassLib.models;
using ClassLib.Repositories.Interfaces;
using WpfApp.Data.Services.interfaces;

namespace WpfApp.Data.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IRepository<Counterparty> _counterpartyRepository;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<Employee> employeeRepository,
        IRepository<Counterparty> counterpartyRepository)
    {
        _orderRepository = orderRepository;
        _employeeRepository = employeeRepository;
        _counterpartyRepository = counterpartyRepository;
    }

    public void CreateOrder(Order order)
    {
        if (order.Amount <= 0)
            throw new ArgumentException("Сумма заказа должна быть больше 0");

        if (order.Employee == null || order.Employee.Id <= 0)
            throw new ArgumentException("Сотрудник не указан");

        if (order.Counterparty == null || order.Counterparty.Id <= 0)
            throw new ArgumentException("Контрагент не указан");

        order.Date = DateTime.Now;
        _orderRepository.Add(order);
    }

    public IList<Order> GetOrdersByEmployee(int employeeId)
    {
        return _orderRepository.Find(o => o.Employee.Id == employeeId);
    }

    public IList<Order> GetOrdersByCounterparty(int counterpartyId)
    {
        return _orderRepository.Find(o => o.Counterparty.Id == counterpartyId);
    }

    public decimal GetTotalAmountByEmployee(int employeeId)
    {
        return _orderRepository.Find(o => o.Employee.Id == employeeId)
            .Sum(o => o.Amount);
    }
}