using Juegos.Serios.Authenticacions.Domain.Common;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Entities.DocumentType;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
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

    public virtual User? CreatedByNavigation { get; set; }

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryCreatedByNavigations { get; set; } = new List<PasswordRecovery>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryUpdatedByNavigations { get; set; } = new List<PasswordRecovery>();

    public virtual ICollection<PasswordRecovery> PasswordRecoveryUsers { get; set; } = new List<PasswordRecovery>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SessionLog> SessionLogCreatedByNavigations { get; set; } = new List<SessionLog>();

    public virtual ICollection<SessionLog> SessionLogUpdatedByNavigations { get; set; } = new List<SessionLog>();

    public virtual ICollection<SessionLog> SessionLogUsers { get; set; } = new List<SessionLog>();

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
}
