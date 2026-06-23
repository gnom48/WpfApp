using ClassLib.models;
using FluentNHibernate.Mapping;

namespace ClassLib
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("employees");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();

            Map(x => x.FirstName)
                .Column("first_name")
                .Length(200)
                .Not.Nullable();

            Map(x => x.Surname)
                .Column("surname")
                .Length(200)
                .Not.Nullable();

            Map(x => x.LastName)
                .Column("last_name")
                .Length(200)
                .Not.Nullable();

            Map(x => x.Position)
                .Column("position")
                .CustomType<Position>() 
                .Not.Nullable();

            Map(x => x.BirthDate)
                .Column("birthdate")
                .Not.Nullable();

            HasMany(x => x.Orders)
                .KeyColumn("employee_id")
                .Inverse() 
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsSet(); 
        }
    }
}
