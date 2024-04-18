// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Aggregate
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="User.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Dawn;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using System.Data;
using System.Xml.Linq;
namespace Juegos.Serios.Authenticacions.Domain.Aggregates;

public class UserAggregate
{
    private int _userId;
    private string _firstName = default!;
    private string _lastName = default!;
    private int _documentTypeId;
    private string _documentNumber = default!;
    private string _username = default!;
    private byte[] _passwordHash = default!;
    private string _email = default!;
    private int _roleId;
    private bool _isActive;
    private bool _isTemporaryPassword;
    private DateTime _createdAt = DateTime.Now;
    private DateTime _updatedAt = DateTime.Now;
    private int _createdBy;
    private int _updatedBy;

    // Propiedades de navegación
    public virtual UserAggregate? CreatedByNavigation { get; set; }

    public virtual DocumentTypeEntity DocumentType { get; set; } = null!;

    public virtual ICollection<UserAggregate> InverseCreatedByNavigation { get; set; } = new List<UserAggregate>();

    public virtual ICollection<UserAggregate> InverseUpdatedByNavigation { get; set; } = new List<UserAggregate>();

    public virtual ICollection<PasswordRecoveryEntity> PasswordRecoveryCreatedByNavigations { get; set; } = new List<PasswordRecoveryEntity>();

    public virtual ICollection<PasswordRecoveryEntity> PasswordRecoveryUpdatedByNavigations { get; set; } = new List<PasswordRecoveryEntity>();

    public virtual ICollection<PasswordRecoveryEntity> PasswordRecoveryUsers { get; set; } = new List<PasswordRecoveryEntity>();

    public virtual RolEntity Rol { get; set; } = null!;

    public virtual ICollection<SessionLogEntity> SessionLogCreatedByNavigations { get; set; } = new List<SessionLogEntity>();

    public virtual ICollection<SessionLogEntity> SessionLogUpdatedByNavigations { get; set; } = new List<SessionLogEntity>();

    public virtual ICollection<SessionLogEntity> SessionLogUsers { get; set; } = new List<SessionLogEntity>();

    public virtual UserAggregate? UpdatedByNavigation { get; set; }

    public UserAggregate(int userId) => _userId = userId;

    public int UserId => _userId;

    // Define properties as before, using Guard for validation

    public string FirstName
    {
        get => _firstName;
        set => _firstName = Guard.Argument(value, nameof(FirstName)).NotEmpty().NotNull().Value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = Guard.Argument(value, nameof(LastName)).NotEmpty().NotNull().Value;
    }

    public int DocumentTypeId
    {
        get => _documentTypeId;
        set => _documentTypeId = Guard.Argument(value, nameof(DocumentTypeId)).NotZero().Value;
    }

    public string DocumentNumber
    {
        get => _documentNumber;
        set => _documentNumber = Guard.Argument(value, nameof(DocumentNumber)).NotEmpty().NotNull().Value;
    }

    public string Username
    {
        get => _username;
        set => _username = Guard.Argument(value, nameof(Username)).NotEmpty().NotNull().Value;
    }

    public byte[] PasswordHash
    {
        get => _passwordHash;
        set => _passwordHash = Guard.Argument(value, nameof(PasswordHash)).NotNull().Value;
    }

    public string Email
    {
        get => _email;
        set => _email = Guard.Argument(value, nameof(Email)).NotEmpty().NotNull().Value;
    }

    public int RoleId
    {
        get => _roleId;
        set => _roleId = Guard.Argument(value, nameof(RoleId)).NotZero().Value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public bool IsTemporaryPassword
    {
        get => _isTemporaryPassword;
        set => _isTemporaryPassword = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = Guard.Argument(value, nameof(CreatedAt)).NotDefault().Value;
    }

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = Guard.Argument(value, nameof(UpdatedAt)).NotDefault().Value;
    }

    public int CreatedBy
    {
        get => _createdBy;
        set => _createdBy = Guard.Argument(value, nameof(CreatedBy)).NotZero().Value;
    }

    public int UpdatedBy
    {
        get => _updatedBy;
        set => _updatedBy = Guard.Argument(value, nameof(UpdatedBy)).NotZero().Value;
    }
}

