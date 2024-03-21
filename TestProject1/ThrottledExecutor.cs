namespace TestProject1;

public class ThrottledExecutor(int threshold)
{
    public void Invoke(Action lambda)
    {
        lambda.Invoke();
    }

    public T Invoke<T>(Func<T> lambda)
    {
        if (threshold <= 0)
            throw new ApplicationException("Can't accept requests rn");
        threshold--;
        return lambda.Invoke();
    }
}