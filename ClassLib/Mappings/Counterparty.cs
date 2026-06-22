using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib;

public class CounterpartyMap : ClassMap<Counterparty>
{
    public CounterpartyMap()
    {
        Table("Counterparties");

        Id(x => x.Id)
            .Column("Id")
            .GeneratedBy.Identity();

        Map(x => x.Name)
            .Column("Name")
            .Length(200)
            .Not.Nullable();

        Map(x => x.Inn)
            .Column("INN")
            .Length(12)
            .Not.Nullable();

        References(x => x.Curator)
            .Column("CuratorId")
            .ForeignKey("FK_Counterparty_Employee")
            .LazyLoad(Laziness.Proxy);

        HasMany(x => x.Orders)
            .KeyColumn("CounterpartyId")
            .Inverse()
            .Cascade.AllDeleteOrphan()
            .LazyLoad()
            .AsSet();
    }
}
