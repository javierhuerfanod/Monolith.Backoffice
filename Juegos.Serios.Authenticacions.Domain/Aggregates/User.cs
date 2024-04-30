using Juegos.Serios.Authenticacions.Domain.Common;
using Juegos.Serios.Authenticacions.Domain.Entities.DataConsent;
using Juegos.Serios.Authenticacions.Domain.Entities.DocumentType;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.SessionLog;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using System.Text;

namespace Juegos.Serios.Authenticacions.Domain.Aggregates;

public partial class User : BaseDomainModel
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int DocumentTypeId { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsTemporaryPassword { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<DataConsent> DataConsentCreatedByNavigations { get; set; } = new List<DataConsent>();

    public virtual ICollection<DataConsent> DataConsentUpdatedByNavigations { get; set; } = new List<DataConsent>();

    public virtual ICollection<DataConsent> DataConsentUsers { get; set; } = new List<DataConsent>();

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryCreatedByNavigations { get; set; } = new List<PasswordRecovery>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryUpdatedByNavigations { get; set; } = new List<PasswordRecovery>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryUsers { get; set; } = new List<PasswordRecovery>();

    public virtual Role Role { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }

    public static User CreateNewUser(UserAggregateModel userModel)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
        var passwordHashBytes = Encoding.UTF8.GetBytes(passwordHash);

        return new User
        {
            IsActive = true,
            DocumentNumber = userModel.DocumentNumber,
            FirstName = userModel.FirstName,
            Email = userModel.Email,
            DocumentTypeId = userModel.DocumentTypeId,
            LastName = userModel.LastName,
            RoleId = userModel.RoleId,
            Username = userModel.Username,
            PasswordHash = passwordHashBytes
        };
    }
    public void UpdatePassword(UpdatePasswordModel updatePasswordModel)
    {
        if (string.IsNullOrWhiteSpace(updatePasswordModel.NewPassword))
            throw new DomainException(AppMessages.Domain_User_Password_Empty);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(updatePasswordModel.NewPassword);
        this.PasswordHash = Encoding.UTF8.GetBytes(passwordHash);
        this.IsTemporaryPassword = false;
    }
}
