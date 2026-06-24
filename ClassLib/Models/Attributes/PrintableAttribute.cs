namespace WpfApp.Data.Models.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class PrintableAttribute : Attribute
{
    public string DisplayName { get; }

    public PrintableAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}