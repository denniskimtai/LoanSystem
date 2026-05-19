namespace LoanSystem.Domain.Primitives;

public class ValidationError : Error
{
    public ValidationError(string[] errors) 
        : base("Validation.Error", "One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public string[] Errors { get; }
}
