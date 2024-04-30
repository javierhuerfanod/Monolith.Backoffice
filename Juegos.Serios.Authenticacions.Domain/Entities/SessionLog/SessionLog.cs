using Juegos.Serios.Authenticacions.Domain.Common;


namespace Juegos.Serios.Authenticacions.Domain.Entities.SessionLog;

public partial class SessionLog : BaseDomainModel
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string? Action { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime? Timestamp { get; set; }
  

    public static SessionLog CreateNewSessionLog(int userId, string action, string ipAddress, DateTime timestamp)
    {
        return new SessionLog
        {
            UserId = userId,
            Action = action,
            Ipaddress = ipAddress,
            Timestamp = timestamp
        };
    }
}
