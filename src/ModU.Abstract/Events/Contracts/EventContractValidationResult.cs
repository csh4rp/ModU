namespace ModU.Abstract.Events.Contracts;

public class EventContractValidationResult
{
    private static readonly EventContractValidationResult ValidInstance = new(Array.Empty<EventContractValidationError>());

    private EventContractValidationResult(IEnumerable<EventContractValidationError> errors) 
        => Errors = errors as IReadOnlyList<EventContractValidationError> ?? errors.ToList();

    public bool IsValid => !Errors.Any();

    public IReadOnlyList<EventContractValidationError> Errors { get; }

    public static EventContractValidationResult Valid() => ValidInstance;

    public static EventContractValidationResult Invalid(IEnumerable<EventContractValidationError> errors) => new(errors);

    public override string ToString()
        => $"IsValid: {IsValid}. Errors: '" + string.Join(",", Errors.Select(e => e.ToString())) +"'";
}