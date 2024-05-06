namespace CodeDojo.Katas.Shared;

public class TestTimeProvider : TimeProvider
{
    private DateTimeOffset _currentDateTime =
        new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public void SetCurrentTime(DateTimeOffset date)
    {
        _currentDateTime = date;
    }

    public override DateTimeOffset GetUtcNow() => _currentDateTime;
}