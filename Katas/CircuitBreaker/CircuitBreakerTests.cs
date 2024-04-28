using FluentAssertions;

namespace NewDay.Katas.CircuitBreaker;

public class CircuitBreakerTests
{
    private readonly Action _failingAction = () => throw new NotImplementedException();
    private readonly Action _nonFailingAction = () => Thread.Sleep(0);


    // Scenario 1 - Monitoring failure
    // As a developer
    // I want to count failures when I make calls
    // A failure count is incremented
    [Theory]
    [InlineData(2)]
    public void InvokeFailingEndpoint_IncreasesCountMultiple(int retries)
    {
        var sut = new CircuitBreaker(_failingAction);

        var initialFailures = sut.Failures;

        for (int i = 0; i < retries; i++)
        {
            sut.Invoke();
        }

        sut.Failures.Should().Be(initialFailures + retries);
    }

    // Scenario 2 - Opening a circuit
    // When my failure count exceeds a configured tolerance
    // All subsequent calls throw an exception and block the call
    [Fact]
    public void Scenario2FailureLessThanTolerance_DoesNotThrow()
    {
        var sut = new CircuitBreaker(_nonFailingAction, 1);
        sut.Invoking(x => x.Invoke())
            .Should().NotThrow<ToleranceExceedException>();
    }

    [Fact]
    public void HappyPath()
    {
        // call method
        // ensure we get expected result

        var ran = false;
        Action actionTmp = () => ran = true;
        var sut = new CircuitBreaker(actionTmp);

        sut.Invoke();

        ran.Should().BeTrue();
    }

    [Fact]
    public void Invoke_WhenClosed_RanDoesNotChange()
    {
        // call method
        // ensure we get expected result

        var ran = false;
        Action actionTmp = () => ran = true;

        // set tol to 0
        var sut = new CircuitBreaker(actionTmp, 0);

        sut.Invoking(x => x.Invoke());

        ran.Should().BeFalse();
    }

    // Scenario 2 - Opening a circuit
    // When my failure count exceeds a configured tolerance
    // All subsequent calls throw an exception and block the call
    [Fact]
    public void Scenario2FailureCountExceedsTolerance()
    {
        var sut = new CircuitBreaker(_failingAction, 0);
        sut.Invoking(x => x.Invoke())
            .Should().Throw<ToleranceExceedException>();
    }

    // Scenario 3 - Half opened state
    // As a developer
    // When my code is blocking calls and a configured timeout passes
    // I allow a single call to proceed
    //  - Single call succeeding closes circuit
    //  - Single call failing resets some kind of timeout
    [Fact]
    public void Scenario3FailuteCountExceedsTolerance()
    {
        // make a call that will take 

        int secondsBeforeHalfOpened = 1;
        var sut = new CircuitBreaker(_failingAction, 0, secondsBeforeHalfOpened);

        // calling too quickly, with no cooldown should fail
        sut.Invoking(x => x.Invoke())
            .Should().Throw<ToleranceExceedException>();

        // waiting for timeout to expire will allow a new request to be made
        Thread.Sleep(secondsBeforeHalfOpened * 1000);
        sut.Invoking(x => x.Invoke())
            .Should().NotThrow();
        sut.Failures.Should().Be(1);
    }
}