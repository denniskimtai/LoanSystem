using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Payments;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Customers.PayRegistrationFee;

public sealed class PayRegistrationFeeCommandHandler : ICommandHandler<PayRegistrationFeeCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PayRegistrationFeeCommandHandler(
        ICustomerRepository customerRepository,
        IPaymentRepository paymentRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _paymentRepository = paymentRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PayRegistrationFeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Customer
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Verify Registration Fee is not already paid
        if (customer.RegistrationFeePaid)
        {
            return Result.Failure(new Error("Customer.RegistrationFeeAlreadyPaid", "The registration fee has already been paid for this customer."));
        }

        // 3. Get User who records this payment
        var user = await _userManager.FindByIdAsync(request.RecordedById.ToString());
        if (user is null)
        {
            return Result.Failure(new Error("User.NotFound", "The specified recording user does not exist."));
        }

        // 4. Update Customer state
        customer.PayRegistrationFee();

        // 5. Create Payment record
        var payment = new Payment(
            loanId: null,
            customerId: customer.Id,
            recordedById: user.Id,
            amount: request.Amount,
            transactionCode: request.TransactionCode,
            mpesaRef: request.MpesaRef,
            payMethod: request.PayMethod,
            recordType: Domain.Enums.RecordType.Manual,
            paidAt: DateTime.UtcNow);

        _paymentRepository.Add(payment);

        // 6. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
