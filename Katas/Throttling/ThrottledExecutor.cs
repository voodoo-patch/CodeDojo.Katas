namespace NewDay.Katas.Throttling;

public class ThrottledExecutor(int threshold, TimeProvider timeProvider)
{
    private int _threshold = threshold;
    private DateTimeOffset _firstRequestInWindowTime;
    private readonly long _windowSize = 5; // value in seconds

    // think about how to abstract the "time" so we don't have to
    // really wait for X seconds to complete a test
    public void Invoke(Action lambda)
    {
        CheckAndDecreaseThreshold();
        lambda.Invoke();
    }

    private void CheckAndDecreaseThreshold()
    {
        /*
         * if (window is expired)
         *  reset window aka reset threshold
         * check if i can execute
         */
        if (IsWindowExpired())
        {
            _threshold = threshold;
            _firstRequestInWindowTime = timeProvider.GetUtcNow();
        }
        
        if (_threshold <= 0)
            throw new ApplicationException("Can't accept requests rn");
        
        _threshold--;
    }

    private bool IsWindowExpired() => 
        timeProvider.GetUtcNow() >= _firstRequestInWindowTime + TimeSpan.FromSeconds(_windowSize);

    public T Invoke<T>(Func<T> lambda)
    {
        CheckAndDecreaseThreshold();
        return lambda.Invoke();
    }
}