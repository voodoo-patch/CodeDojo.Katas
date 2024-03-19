namespace TestProject1;

public class ThrottledExecutor{
    public Task<object> Invoke(IExecutable executable)
    {
        return executable.Execute();
    }
}

public interface IExecutable
{
    Task<object> Execute();
}