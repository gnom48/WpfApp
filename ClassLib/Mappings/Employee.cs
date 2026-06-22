using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");

            Id(x => x.Id)
                .Column("Id")
                .GeneratedBy.Identity();

            Map(x => x.FullName)
                .Column("FullName")
                .Length(200)
                .Not.Nullable();

            Map(x => x.Position)
                .Column("Position")
                .CustomType<Position>() 
                .Not.Nullable();

            Map(x => x.BirthDate)
                .Column("BirthDate")
                .Not.Nullable();

            HasMany(x => x.Orders)
                .KeyColumn("EmployeeId")
                .Inverse() 
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsSet(); 
        }
    }
}
