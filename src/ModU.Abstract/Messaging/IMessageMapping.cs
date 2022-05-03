namespace ModU.Abstract.Messaging;

public interface IMessageMapping<in TSource, out TDest> where TDest : IMessage
{
    TDest Map(TSource source);
}