namespace NewDay.Katas.Shared;

public class Clock : IDisposable
{
    private static DateTime? _startTime;
    private static DateTime? _nowForTest;

    public static DateTime Now
    {
        get
        {
            if (_nowForTest == null)
            {
                return DateTime.Now;
            }
            else
            {
                //freeze time
                if (_startTime == null)
                {
                    return _nowForTest.GetValueOrDefault();
                }
                //keep running
                else
                {
                    TimeSpan elapsedTime = DateTime.Now.Subtract(_startTime.GetValueOrDefault());
                    return _nowForTest.GetValueOrDefault().Add(elapsedTime);
                }
            }
        }
    }

    public static IDisposable NowIs(DateTime dateTime, bool keepTimeRunning = false)
    {
        _nowForTest = dateTime;
        if (keepTimeRunning)
        {
            _startTime = DateTime.Now;
        }
        return new Clock();
    }

    public static IDisposable ResetNowIs()
    {
        _startTime = null;
        _nowForTest = null;
        return new Clock();
    }

    public void Dispose()
    {
        _startTime = null;
        _nowForTest = null;
    }
}