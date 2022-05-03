namespace ModU.Abstract.Messaging.Contracts;

public class MessageContractValidationResult
{
    private static readonly MessageContractValidationResult ValidInstance = new(Array.Empty<MessageContractValidationError>());

    private MessageContractValidationResult(IEnumerable<MessageContractValidationError> errors) 
        => Errors = errors as IReadOnlyList<MessageContractValidationError> ?? errors.ToList();

    public bool IsValid => !Errors.Any();

    public IReadOnlyList<MessageContractValidationError> Errors { get; }

    public static MessageContractValidationResult Valid() => ValidInstance;

    public static MessageContractValidationResult Invalid(IEnumerable<MessageContractValidationError> errors) => new(errors);

    public override string ToString()
        => $"IsValid: {IsValid}. Errors: '" + string.Join(",", Errors.Select(e => e.ToString())) +"'";
}