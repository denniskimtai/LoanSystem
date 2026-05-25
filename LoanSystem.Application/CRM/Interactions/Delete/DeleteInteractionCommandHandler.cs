using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.CRM.Interactions.Delete;

public sealed class DeleteInteractionCommandHandler : ICommandHandler<DeleteInteractionCommand>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInteractionCommandHandler(
        IInteractionRepository interactionRepository,
        IUnitOfWork unitOfWork)
    {
        _interactionRepository = interactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteInteractionCommand request, CancellationToken cancellationToken)
    {
        var interaction = await _interactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (interaction is null)
        {
            return Result.Failure(new Error("Interaction.NotFound", "The specified interaction does not exist."));
        }

        interaction.MarkAsDeleted();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
