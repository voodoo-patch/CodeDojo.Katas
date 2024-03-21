using FluentAssertions;
using Moq;

namespace TestProject1;

public class ThrottledExecutorTests
{
    [Fact]
    public void Invoke_FooBar()
    {
        var executor = new ThrottledExecutor();

        var foo = new Foo();
        executor.Invoke(() => foo.Bar());

        foo.Counter.Should().Be(1);
    }
    
    [Fact]
    public void Invoke_Foo2Bar2()
    {
        var executor = new ThrottledExecutor();

        var foo2 = new Foo2();
        executor.Invoke(() => foo2.Bar2());

        foo2.Counter.Should().Be(1);
    }
    
    [Fact]
    public void Invoke_Foo3Bar3()
    {
        var executor = new ThrottledExecutor();

        var foo3 = new Foo3();
        // assign a return value from invoke and assert is equal to 1
        int counter = executor.Invoke(() => foo3.Bar3());

        counter.Should().Be(1);
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
public class Foo3
{
    public int Counter { get; set; } = 0;
    public int Bar3()
    {
        return ++Counter;
    }
}