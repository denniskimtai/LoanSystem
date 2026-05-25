using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Identity;

public sealed class Branch : BaseEntity
{
    public string Name { get; private set; }
    public string Location { get; private set; }
    public ICollection<User> Users { get; private set; } = new List<User>();

    public Branch(string name, string location)
    {
        Name = name;
        Location = location;
    }

    public void Update(string name, string location)
    {
        Name = name;
        Location = location;
        UpdateTimestamp();
    }

    private Branch() { } // EF Core
}
