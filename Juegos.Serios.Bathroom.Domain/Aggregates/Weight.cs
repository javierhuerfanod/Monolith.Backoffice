using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Bathroom.Domain.Aggregates;

public partial class Weight : BaseDomainModel
{
    public int WeightId { get; set; }

    public int UserId { get; set; }

    public DateOnly? Date { get; set; }

    public int? Weight1 { get; set; }

    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; } = new List<QuestionnaireAnswer>();
}
