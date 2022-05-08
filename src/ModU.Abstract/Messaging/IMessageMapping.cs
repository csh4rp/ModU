namespace ModU.Abstract.Messaging;

public interface IMessageMapping<in TSource, out TDestination> where TDestination : IMessage
{
    TDestination Map(TSource source);
}