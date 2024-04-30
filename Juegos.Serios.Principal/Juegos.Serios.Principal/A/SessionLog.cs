using System;
using System.Collections.Generic;

namespace Juegos.Serios.Principal.A;

public partial class SessionLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string? Action { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime? Timestamp { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}
