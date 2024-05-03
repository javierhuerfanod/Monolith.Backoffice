using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts;
using Juegos.Serios.Shared.Domain.Common;

namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class UserAvatarBodyPart : BaseDomainModel
{
    public int UserAvatarBodyPartsId { get; set; }

    public int? UserId { get; set; }

    public string? BodyPartName { get; set; }

    public int? BodyPartAnimationId { get; set; }

    public virtual User? User { get; set; }

    public static List<UserAvatarBodyPart> UpdateUserAvatarBodyParts(UserBodyPartsModel model)
    {
        List<UserAvatarBodyPart> userAvatarBodyParts = [];

        foreach (var bodyPartModel in model.BodyParts)
        {
            UserAvatarBodyPart avatarBodyPart = InitializeFromModel(model.UserId, bodyPartModel);
            userAvatarBodyParts.Add(avatarBodyPart);
        }

        return userAvatarBodyParts;
    }
    private static UserAvatarBodyPart InitializeFromModel(int userId, BodyPartModel model)
    {
        return new UserAvatarBodyPart
        {
            UserId = userId,
            BodyPartName = model.BodyPartName,
            BodyPartAnimationId = model.BodyPartAnimationId
        };
    }
}
