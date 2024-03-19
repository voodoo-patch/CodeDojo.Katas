using FluentAssertions;
using Moq;

namespace TestProject1;

public class ThrottledExecutorTests
{
    [Fact]
    public async void Invoke_FooBar()
    {
        var executor = new ThrottledExecutor();

        var foo = new Foo();
        await executor.Invoke(() => foo.Bar());

        foo.Counter.Should().Be(1);
    }
    
    [Fact]
    public async void Invoke_Foo2Bar2()
    {
        var executor = new ThrottledExecutor();

        var foo2 = new Foo2();
        await executor.Invoke(() => foo2.Bar2());

        foo2.Counter.Should().Be(1);
    }
}


public class Foo
{
    public int Counter { get; set; } = 0;
    public void Bar()
    {
        Counter++;
    }
}

public class Foo2
{
    public int Counter { get; set; } = 0;
    public void Bar2()
    {
        Counter++;
    }
}