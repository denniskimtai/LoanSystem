using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Loans.GetById;

public sealed class GetLoanByIdQueryHandler : IQueryHandler<GetLoanByIdQuery, LoanDetailsResponse>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoanByIdQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Result<LoanDetailsResponse>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure<LoanDetailsResponse>(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        var response = new LoanDetailsResponse(
            loan.Id,
            loan.Code,
            loan.CustomerId,
            loan.Customer?.FullName ?? string.Empty,
            loan.ProductId,
            new LoanProductResponse(
                loan.Product.Id,
                loan.Product.Name,
                loan.Product.MinAmount,
                loan.Product.MaxAmount,
                loan.Product.InterestRate,
                loan.Product.RepaymentDays
            ),
            loan.BranchId,
            loan.LoId,
            loan.CoId,
            loan.CreatedById,
            loan.Principal,
            loan.AddOnsTotal,
            loan.DeductionsTotal,
            loan.RepayableTotal,
            loan.RepaidTotal,
            loan.Balance,
            loan.InterestAmount,
            loan.PenaltyAmount,
            loan.Type,
            loan.Stage,
            loan.Status,
            loan.MpesaCode,
            loan.DisbursedAt,
            loan.DueDate,
            loan.LastRepayDate,
            loan.ClearedDate,
            loan.CreatedAt,
            loan.UpdatedAt,
            loan.Addons.Select(a => new LoanAddonResponse(
                a.Id,
                a.Name,
                a.Amount,
                a.IsApplied,
                a.AppliedAt,
                a.AppliedById
            )).ToList(),
            loan.Deductions.Select(d => new LoanDeductionResponse(
                d.Id,
                d.Name,
                d.Amount,
                d.IsApplied,
                d.AppliedAt,
                d.AppliedById
            )).ToList(),
            loan.PaySchedules.Select(p => new PayScheduleResponse(
                p.Id,
                p.ScheduledDate,
                p.Amount,
                p.Balance,
                p.Status
            )).ToList()
        );

        return Result.Success(response);
    }
}
