using System;
using System.Collections.Generic;

namespace Juegos.Serios.Principal.A;

public partial class User
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
}
