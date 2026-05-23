using LoanSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string FullName { get; private set; }
    public UserRole Role { get; private set; }
    public Guid BranchId { get; private set; }
    public Branch Branch { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public User(string email, string fullName, UserRole role, Guid branchId)
    {
        Email = email;
        UserName = email;
        FullName = fullName;
        Role = role;
        BranchId = branchId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    private User() { } // EF Core
}
