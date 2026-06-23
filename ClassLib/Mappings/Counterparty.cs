using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib;

public class CounterpartyMap : ClassMap<Counterparty>
{
    public CounterpartyMap()
    {
        Table("counterparties");

        Id(x => x.Id)
            .Column("id")
            .GeneratedBy.Identity();

        Map(x => x.Name)
            .Column("name")
            .Length(200)
            .Not.Nullable();

        Map(x => x.Inn)
            .Column("inn")
            .Length(12)
            .Not.Nullable();

        References(x => x.Curator)
            .Column("curator_id")
            .ForeignKey("FK_Counterparty_Employee")
            .LazyLoad(Laziness.Proxy);

        HasMany(x => x.Orders)
            .KeyColumn("counterparty_id")
            .Inverse()
            .Cascade.AllDeleteOrphan()
            .LazyLoad()
            .AsSet();
    }
}
