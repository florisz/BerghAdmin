namespace BerghAdmin.Services.DateTimeProvider;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime Set { set; }
}
