using Juegos.Serios.Shared.Domain.Common;
namespace Juegos.Serios.Bathroom.Domain.Entities;

public partial class QuestionnaireQuestion : BaseDomainModel
{
    public int QuestionId { get; set; }

    public int? QuestionnaireId { get; set; }

    public string? Question { get; set; }

    public virtual Questionnaire? Questionnaire { get; set; }

    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; } = new List<QuestionnaireAnswer>();
}
