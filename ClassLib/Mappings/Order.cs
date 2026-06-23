using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib;

public class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Table("orders");

        Id(x => x.Id)
            .Column("id")
            .GeneratedBy.Identity();

        Map(x => x.Date)
            .Column("date")
            .Not.Nullable()
            .Default("CURRENT_DATE");

        Map(x => x.Amount)
            .Column("amount")
            .Precision(18)
            .Scale(2)
            .Not.Nullable();

        References(x => x.Employee)
            .Column("employee_id")
            .ForeignKey("FK_Order_Employee")
            .Not.Nullable()
            .LazyLoad(Laziness.Proxy);

        References(x => x.Counterparty)
            .Column("counterparty_id")
            .ForeignKey("FK_Order_Counterparty")
            .Not.Nullable()
            .LazyLoad(Laziness.Proxy);
    }
}