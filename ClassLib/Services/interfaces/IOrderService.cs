using ClassLib.models;

namespace WpfApp.Data.Services.interfaces;

public interface IOrderService
{
    void CreateOrder(Order order);
    IList<Order> GetOrdersByEmployee(int employeeId);
    IList<Order> GetOrdersByCounterparty(int counterpartyId);
    decimal GetTotalAmountByEmployee(int employeeId);
}
