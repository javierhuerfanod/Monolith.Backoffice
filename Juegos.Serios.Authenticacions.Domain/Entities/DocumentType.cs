using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class DocumentType : BaseDomainModel
{
    public int DocumentTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
