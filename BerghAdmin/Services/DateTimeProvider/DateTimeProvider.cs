namespace BerghAdmin.Services.DateTimeProvider;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now { 
        get => DateTime.Now;
    }

    public DateTime Set { set => throw new NotImplementedException(); }

}

