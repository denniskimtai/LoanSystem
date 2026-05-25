using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers;

public sealed record CustomerResponse(
    Guid Id,
    string FullName,
    string NationalId,
    string Phone,
    string PhotoUrl,
    CustomerStatus Status,
    string PhysicalAddress,
    string Town,
    string County,
    decimal CurrentLimit,
    Guid BranchId,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
