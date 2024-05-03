using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Bathroom.Domain.Entities;

public partial class Questionnaire : BaseDomainModel
{
    public int QuestionnaireId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; } = new List<QuestionnaireQuestion>();
}
