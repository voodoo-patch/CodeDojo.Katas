using FluentAssertions;
using Moq;

namespace TestProject1;

public class ThrottledExecutorTests
{
    [Fact]
    public void Invoke_ActionWithoutReturnValue()
    {
        var executor = new ThrottledExecutor();

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
        var executor = new ThrottledExecutor();

        int counter = executor.Invoke(() => 1);

        counter.Should().Be(1);
    }
    
    [Fact]
    public void Invoke_FunctionThatReturnsString()
    {
        var executor = new ThrottledExecutor();

        string counter = executor.Invoke(() => "a string");

        counter.Should().Be("a string");
    }
}