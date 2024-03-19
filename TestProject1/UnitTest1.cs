using Moq;

namespace TestProject1;

public class ThrottledExecutorTests
{
    [Fact]
    public async void Execute_WhenWithinThreshold_ExecuteIsExecuted()
    {
        var executor = new ThrottledExecutor();

        var mock = new Mock<IExecutable>();
        mock
            .Setup(e => e.Execute())
            .Returns(() => Task.FromResult<object>("hello"));
        await executor.Invoke(mock.Object);
        
        mock.Verify(m => m.Execute(), Times.Once());
    }
}

