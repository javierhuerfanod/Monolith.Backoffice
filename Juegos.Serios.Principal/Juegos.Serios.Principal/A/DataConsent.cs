using System;
using System.Collections.Generic;

namespace Juegos.Serios.Principal.A;

public partial class DataConsent
{
    public int ConsentId { get; set; }

    public int UserId { get; set; }

    public DateTime ConsentDate { get; set; }

    public bool ConsentStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
