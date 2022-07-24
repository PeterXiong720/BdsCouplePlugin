using MC;

namespace BdsCP.Model;

public class Couple
{
    public Guid Id { get; set; }
    
    public int Index { get; set; }

    public bool IsDeleted { get; set; }

    public string Husband { get; set; }
    
    public string Wife { get; set; }

    public string Name { get; set; }

    public Couple(string husband, string wife, string? nick = null)
    {
        IsDeleted = false;
        Name = nick ?? $"cp_{Index}";

        Husband = husband;
        Wife = wife;
    }
    
#pragma warning disable CS8618
    public Couple() {}
#pragma warning restore CS8618
}