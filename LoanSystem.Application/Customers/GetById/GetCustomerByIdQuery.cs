using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.GetById;

public sealed record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDetailsResponse>;
