using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class City : BaseDomainModel
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<User> UserCities { get; set; } = new List<User>();

    public virtual ICollection<User> UserCityHomes { get; set; } = new List<User>();
}
