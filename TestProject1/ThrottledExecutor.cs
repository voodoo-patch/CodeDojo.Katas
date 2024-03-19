namespace TestProject1;

public class ThrottledExecutor{
    public Task Invoke(Action lambda)
    {
        lambda.Invoke();
        return Task.CompletedTask;
    }
}