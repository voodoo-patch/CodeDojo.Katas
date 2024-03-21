namespace TestProject1;

public class ThrottledExecutor(int threshold)
{
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