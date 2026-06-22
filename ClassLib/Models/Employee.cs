namespace ClassLib.models;

public class Employee
{
    public virtual int Id { get; set; }
    public virtual string FullName { get; set; }
    public virtual Position Position { get; set; }
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