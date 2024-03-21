using FluentAssertions;
using Moq;

namespace TestProject1;

public class ThrottledExecutorTests
{
    [Fact]
    public void Invoke_ActionWithoutReturnValue()
    {
        var executor = new ThrottledExecutor(1);

        int counter = 0;
        executor.Invoke(() =>
        {
            counter++;
            Console.WriteLine("updated");
        });

        counter.Should().Be(1);
    }
    
    [Fact]
    public void Invoke_FunctionThatReturnsInt()
    {
        var executor = new ThrottledExecutor(1);

        int counter = executor.Invoke(() => 1);

        counter.Should().Be(1);
    }
    
    [Fact]
    public void Invoke_FunctionThatReturnsString()
    {
        var executor = new ThrottledExecutor(1);

        string result = executor.Invoke(() => "a string");

        result.Should().Be("a string");
    }
    
    
    [Fact]
    public void Invoke_FunctionIsExecutedNoMoreThanOnce_WhenThresholdIsOne()
    {
        var executor = new ThrottledExecutor(1);

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
        var executor = new ThrottledExecutor(1);

        int counter = 0;
        Action action = () =>
        {
            counter++;
            Console.WriteLine("updated");
        };
        executor.Invoke(action);
        executor.Invoking(e => e.Invoke(action))
            .Should().Throw<ApplicationException>();
        counter.Should().Be(1);
    }
}