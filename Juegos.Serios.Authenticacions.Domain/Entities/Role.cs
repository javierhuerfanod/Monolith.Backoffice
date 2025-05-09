﻿using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Common;


namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class Role : BaseDomainModel
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
