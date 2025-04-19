using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Bathroom.Domain.Entities;

public partial class QuestionnaireAnswer : BaseDomainModel
{
    public int AnswerId { get; set; }

    public int? WeightId { get; set; }

    public int? QuestionId { get; set; }

    public bool Answer { get; set; }

    public virtual QuestionnaireQuestion? Question { get; set; }

    public virtual Weight? Weight { get; set; }
}
