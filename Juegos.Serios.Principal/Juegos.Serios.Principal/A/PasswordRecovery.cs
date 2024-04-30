using System;
using System.Collections.Generic;

namespace Juegos.Serios.Principal.A;

public partial class PasswordRecovery
{
    public int RecoveryId { get; set; }

    public int UserId { get; set; }

    public byte[] RecoveryPassword { get; set; } = null!;

    public DateTime RecoveryPasswordExpiration { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
