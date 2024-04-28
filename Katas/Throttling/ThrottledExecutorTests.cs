namespace CodeDojo.Katas.Throttling;

public class ThrottledExecutorTests
{
    [Fact]
    public void Invoke_ActionWithoutReturnValue()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = 0;
        executor.Invoke(() =>
        {
            counter++;
        });

        counter.Should().Be(1);
    }

    [Fact]
    public void Invoke_FunctionThatReturnsInt()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = executor.Invoke(() => 1);

        counter.Should().Be(1);
    }

    [Fact]
    public void Invoke_FunctionThatReturnsString()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        string result = executor.Invoke(() => "a string");

        result.Should().Be("a string");
    }

    [Fact]
    public void Invoke_FunctionIsExecutedNoMoreThanOnce_WhenThresholdIsOne()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = 0;
        var func = () => counter++;
        executor.Invoke(func);

        executor.Invoking(e => e.Invoke(func))
            .Should().Throw<ApplicationException>();
        counter.Should().Be(1);
    }

    [Fact]
    public void Invoke_ActionIsExecutedNoMoreThanOnce_WhenThresholdIsOne()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = 0;
        Action action = () =>
        {
            counter++;
        };
        executor.Invoke(action);
        executor.Invoking(e => e.Invoke(action))
            .Should().Throw<ApplicationException>();
        counter.Should().Be(1);
    }

    [Fact]
    public void Invoke_FunctionIsExecutedMoreThanOnce_WhenThresholdIsOne_AndWindowExpired()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = 0;
        var func = () => counter++;
        executor.Invoke(func);

        timeProvider.SetCurrentTime(timeProvider.GetUtcNow() + TimeSpan.FromSeconds(5));
        executor.Invoke(func);
        counter.Should().Be(2);
    }

    [Fact]
    public void Invoke_ActionIsExecutedMoreThanOnce_WhenThresholdIsOne_AndWindowExpired()
    {
        TestTimeProvider timeProvider = new();
        var executor = new ThrottledExecutor(1, timeProvider);

        int counter = 0;
        Action action = () =>
        {
            counter++;
        };
        executor.Invoke(action);

        timeProvider.SetCurrentTime(timeProvider.GetUtcNow() + TimeSpan.FromSeconds(5));
        executor.Invoke(action);
        counter.Should().Be(2);
    }
}

public class TestTimeProvider : TimeProvider
{
    private DateTimeOffset _currentDateTime =
        new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public void SetCurrentTime(DateTimeOffset date)
    {
        _currentDateTime = date;
    }

    public override DateTimeOffset GetUtcNow() => _currentDateTime;
}