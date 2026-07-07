using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Payments;

namespace LoanSystem.Infrastructure.Database.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Payment payment)
    {
        _context.Payments.Add(payment);
    }
}
