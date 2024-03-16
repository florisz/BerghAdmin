namespace BerghAdmin.Services.DateTimeProvider;

//
// This class is used for testing purposes only.
//
public class TestDateTimeProvider : IDateTimeProvider
{
    static private DateTime _now = DateTime.Now;
    private readonly ILogger<TestDateTimeProvider> _logger;

    public TestDateTimeProvider(ILogger<TestDateTimeProvider> logger)
    {
        _logger = logger;
        _logger.LogDebug($"TestDateTimeProvider created; current data/time: {_now.ToString("dd MMMM yyyy HH:mm:ssdddd, dd MMMM yyyy HH:mm")}");
    }

    public DateTime Now { 
        get => _now;
    }

    public DateTime Set
    {
        set
        {
            _now = value;
            _logger.LogDebug($"Time set; new data/time: {_now.ToString("dd MMMM yyyy HH:mm:ssdddd, dd MMMM yyyy HH:mm")}");
        }
    }
}

