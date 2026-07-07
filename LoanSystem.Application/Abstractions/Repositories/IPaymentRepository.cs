using LoanSystem.Domain.Entities.Payments;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface IPaymentRepository
{
    void Add(Payment payment);
}
