using System;
using System.Collections.Generic;

namespace Juegos.Serios.Principal.A;

public partial class DocumentType
{
    public int DocumentTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
