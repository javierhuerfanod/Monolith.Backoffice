using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Common;

namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class PasswordRecovery : BaseDomainModel
{
    public int RecoveryId { get; set; }

    public int UserId { get; set; }

    public byte[] RecoveryPassword { get; set; } = null!;

    public DateTime RecoveryPasswordExpiration { get; set; }  

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
