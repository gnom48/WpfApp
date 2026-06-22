using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib;

public class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Table("Orders");

        Id(x => x.Id)
            .Column("Id")
            .GeneratedBy.Identity();

        Map(x => x.Date)
            .Column("Date")
            .Not.Nullable()
            .Default("CURRENT_DATE");

        Map(x => x.Amount)
            .Column("Amount")
            .Precision(18)
            .Scale(2)
            .Not.Nullable();

        References(x => x.Employee)
            .Column("EmployeeId")
            .ForeignKey("FK_Order_Employee")
            .Not.Nullable()
            .LazyLoad(Laziness.Proxy);

        References(x => x.Counterparty)
            .Column("CounterpartyId")
            .ForeignKey("FK_Order_Counterparty")
            .Not.Nullable()
            .LazyLoad(Laziness.Proxy);
    }
}