using WpfApp.Data.Models.Attributes;

namespace ClassLib.models;

public class Counterparty
{
    [Printable("Id")]
    public virtual int Id { get; set; }

    [Printable("Наименование")]
    public virtual string Name { get; set; }

    [Printable("ИНН")]
    public virtual string Inn { get; set; }

    [Printable("Id куратора")]
    public virtual int CuratorId { get; set; }

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