namespace Benchmarks;

public class NamingConventions
{
    private const string PrivateConstField = "Private Const Field";

    private static string PrivateStaticField = "Private Static Field";

    private string _privateField = "Private Field";

    public string PublicProperty { get; set; } = "Public Property";

    public void PublicMethod()
    {
        Console.WriteLine(PrivateConstField);
        Console.WriteLine(PrivateStaticField);
        Console.WriteLine(_privateField);
        Console.WriteLine(PublicProperty);
    }
}
