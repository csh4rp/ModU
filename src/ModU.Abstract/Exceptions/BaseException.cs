namespace ModU.Abstract.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string message) : base(message)
    {
    }
    
    public abstract string Code { get; }
}