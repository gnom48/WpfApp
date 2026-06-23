using WpfApp.Data.Models.Attributes;

namespace ClassLib.models;

public class Order
{
    [Printable("Id")]
    public virtual int Id { get; set; }
    
    [Printable("Дата")]
    public virtual DateTime Date { get; set; }

    [Printable("Сумма")]
    public virtual decimal Amount { get; set; }

    [Printable("Id сотрудника")]
    public virtual int EmployeeId { get; set; }
    
    public virtual Employee Employee { get; set; }
    
    [Printable("Id контрагента")]
    public virtual int CounterpartyId { get; set; }
    
    public virtual Counterparty Counterparty { get; set; }

    public virtual decimal Vat => Amount * 0.20m;
    public virtual decimal AmountWithoutVat => Amount - Vat;
}