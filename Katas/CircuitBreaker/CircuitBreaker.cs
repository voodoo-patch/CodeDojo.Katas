namespace CodeDojo.Katas.CircuitBreaker;

public class CircuitBreaker(
    Action action,
    int tolerance = int.MaxValue,
    int seconds = 10)
{
    private CircuitState _state = CircuitState.Closed;
    public int Failures { get; private set; }
    private int Tolerance { get; } = tolerance;

    public void Invoke()
    {
        if (Failures >= Tolerance &&
            _state != CircuitState.HalfClosed)
        {
            _state = CircuitState.Closed;
            StartCountdown();
            throw new ToleranceExceedException();
        }

        try
        {
            action.Invoke();
        }
        catch (Exception)
        {
            if (_state == CircuitState.HalfClosed)
            {
                _state = CircuitState.Closed;
                StartCountdown();
            }

            Failures++;
        }
    }

    private void StartCountdown()
    {
        Task.Run(() =>
        {
            Thread.Sleep(seconds * 1000);
            _state = CircuitState.HalfClosed;
        });
    }
}

public enum CircuitState
{
    Open = 0,
    Closed,
    HalfClosed
}

public class ToleranceExceedException : ApplicationException;