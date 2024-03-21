namespace TestProject1;

public class ThrottledExecutor {
    public void Invoke(Action lambda)
    {
        lambda.Invoke();
    }
    
    public T Invoke<T>(Func<T> lambda)
    {
        return lambda.Invoke();
    }
}