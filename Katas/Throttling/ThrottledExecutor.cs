namespace NewDay.Katas.Throttling;

public class ThrottledExecutor(int threshold)
{
    // think about how to abstract the "time" so we don't have to
    // really wait for X seconds to complete a test
    public void Invoke(Action lambda)
    {
        CheckAndDecreaseThreshold();
        lambda.Invoke();
    }

    private void CheckAndDecreaseThreshold()
    {
        if (threshold <= 0)
            throw new ApplicationException("Can't accept requests rn");
        threshold--;
    }

    public T Invoke<T>(Func<T> lambda)
    {
        CheckAndDecreaseThreshold();
        return lambda.Invoke();
    }
}