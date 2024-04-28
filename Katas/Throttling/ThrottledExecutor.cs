namespace CodeDojo.Katas.Throttling;

public class ThrottledExecutor(int threshold, TimeProvider timeProvider)
{
    private int _threshold = threshold;
    private DateTimeOffset _firstRequestInWindowTime;
    private readonly long _windowSize = 5; // value in seconds

    public void Invoke(Action lambda)
    {
        CheckAndDecreaseThreshold();
        lambda.Invoke();
    }

    public T Invoke<T>(Func<T> lambda)
    {
        CheckAndDecreaseThreshold();
        return lambda.Invoke();
    }

    private void CheckAndDecreaseThreshold()
    {
        ResetWindowIfExpired();

        if (_threshold <= 0)
            throw new ApplicationException("Can't accept requests rn");

        _threshold--;
    }

    private void ResetWindowIfExpired()
    {
        if (IsWindowExpired())
        {
            _threshold = threshold;
            _firstRequestInWindowTime = timeProvider.GetUtcNow();
        }
    }

    private bool IsWindowExpired() =>
        timeProvider.GetUtcNow() >= _firstRequestInWindowTime + TimeSpan.FromSeconds(_windowSize);
}