using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Tests.Messaging.TestData;

public class TestMessageContract : MessageContract<TestMessage>
{
    public void SetUpToRequireAll()
    {
        RequireAll();
    }
    
    public void SetUpToRequireName()
    {
        Require(x => x.Name);
    }
}