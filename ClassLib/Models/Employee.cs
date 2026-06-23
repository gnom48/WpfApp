using WpfApp.Data.Models.Attributes;

namespace ClassLib.models;

public class Employee
{
    [Printable("Id")]
    public virtual int Id { get; set; }

    [Printable("Фамилия")]
    public virtual string Surname { get; set; }

    [Printable("Имя")]
    public virtual string FirstName { get; set; }

    [Printable("Отчество")]
    public virtual string? LastName { get; set; }
    
    [Printable("Должность")]
    public virtual Position Position { get; set; }
    
    [Printable("Дата рождения")]
    public virtual DateTime BirthDate { get; set; }


    public virtual ISet<Order> Orders { get; set; } = new HashSet<Order>();

    public virtual int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}