namespace ClassLib.models;

public class Order
{
    public virtual int Id { get; set; }
    public virtual DateTime Date { get; set; }
    public virtual decimal Amount { get; set; }

    public virtual Employee Employee { get; set; }
    public virtual Counterparty Counterparty { get; set; }

    public virtual decimal Vat => Amount * 0.20m;
    public virtual decimal AmountWithoutVat => Amount - Vat;
}