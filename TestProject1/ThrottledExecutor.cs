namespace TestProject1;

public class ThrottledExecutor {
    public void Invoke(Action lambda)
    {
        lambda.Invoke();
    }
    
    public int Invoke(Func<int> lambda)
    {
        return lambda.Invoke();
    }
    
    // public Task<T> Invoke<T>(Func<T> lambda)
    // {
    //     return Task.FromResult(lambda.Invoke());
    // }
}