namespace ClassLib.models;

public class Counterparty
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Inn { get; set; }

    public virtual Employee Curator { get; set; }

    public virtual ISet<Order> Orders { get; set; } = new HashSet<Order>();

    public virtual bool IsInnValid
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Inn)) return false;
            return (Inn.Length == 10 || Inn.Length == 12) && long.TryParse(Inn, out _);
        }
    }
}