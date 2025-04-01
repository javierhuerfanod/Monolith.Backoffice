-- create database CardioSalinaDB
CREATE DATABASE CardioSalinaDB;
GO

USE CardioSalinaDB;
GO

CREATE TABLE Cities
(
  CityId    int      NOT NULL IDENTITY(1,1),
  CityName  nvarchar NOT NULL,
  CreatedAt datetime,
  UpdatedAt datetime,
  CONSTRAINT PK_Cities PRIMARY KEY (CityId)
)
GO

ALTER TABLE Cities
  ADD CONSTRAINT UQ_CityId UNIQUE (CityId)
GO

CREATE TABLE DataConsent
(
  ConsentId     int      NOT NULL IDENTITY(1,1),
  ConsentDate   datetime,
  ConsentStatus bit     ,
  CreatedAt     datetime,
  UpdatedAt     datetime,
  UserID        int      NOT NULL,
  CONSTRAINT PK_DataConsent PRIMARY KEY (ConsentId)
)
GO

ALTER TABLE DataConsent
  ADD CONSTRAINT UQ_ConsentId UNIQUE (ConsentId)
GO

CREATE TABLE DocumentTypes
(
  DocumentTypeId int      NOT NULL IDENTITY(1,1),
  IsActive       bit     ,
  TypeName       nvarchar NOT NULL,
  CreatedAt      datetime,
  UpdatedAt      datetime,
  CONSTRAINT PK_DocumentTypes PRIMARY KEY (DocumentTypeId)
)
GO

ALTER TABLE DocumentTypes
  ADD CONSTRAINT UQ_DocumentTypeId UNIQUE (DocumentTypeId)
GO

ALTER TABLE DocumentTypes
  ADD CONSTRAINT UQ_TypeName UNIQUE (TypeName)
GO

CREATE TABLE PasswordRecovery
(
  RecoveryId                 int      NOT NULL IDENTITY(1,1),
  RecoveryPassword           nvarchar NOT NULL,
  RecoveryPasswordExpiration datetime,
  CreatedAt                  datetime,
  UpdatedAt                  datetime,
  UserID                     int      NOT NULL,
  CONSTRAINT PK_PasswordRecovery PRIMARY KEY (RecoveryId)
)
GO

ALTER TABLE PasswordRecovery
  ADD CONSTRAINT UQ_RecoveryId UNIQUE (RecoveryId)
GO

CREATE TABLE Roles
(
  RoleId    int      NOT NULL IDENTITY(1,1),
  RoleName  nvarchar NOT NULL,
  CreatedAt datetime,
  UpdatedAt datetime,
  CONSTRAINT PK_Roles PRIMARY KEY (RoleId)
)
GO

ALTER TABLE Roles
  ADD CONSTRAINT UQ_RoleId UNIQUE (RoleId)
GO

ALTER TABLE Roles
  ADD CONSTRAINT UQ_RoleName UNIQUE (RoleName)
GO

CREATE TABLE SessionsLogs
(
  logid     int      NOT NULL IDENTITY(1,1),
  Action    nvarchar,
  IpAddress varchar ,
  Timestamp datetime,
  CreatedAt datetime,
  UpdatedAt datetime,
  UserID    int      NOT NULL
)
GO

ALTER TABLE SessionsLogs
  ADD CONSTRAINT UQ_logid UNIQUE (logid)
GO

CREATE TABLE User
(
  UserID              int      NOT NULL IDENTITY(1,1),
  FirstName           nvarchar NOT NULL,
  LastName            nvarchar NOT NULL,
  DocumentNumber      bigint   NOT NULL,
  UserName            nvarchar NOT NULL,
  PasswordHash        nvarchar NOT NULL,
  Email               nvarchar NOT NULL,
  IsTemporaryPassword bit     ,
  CreatedAt           datetime,
  UpdatedAt           datetime,
  Birtday             datetime,
  Weight              int     ,
  DocumentTypeId      int      NOT NULL,
  CityId              int      NOT NULL,
  RoleId              int      NOT NULL,
  CONSTRAINT PK_User PRIMARY KEY (UserID)
)
GO

ALTER TABLE User
  ADD CONSTRAINT UQ_UserID UNIQUE (UserID)
GO

ALTER TABLE User
  ADD CONSTRAINT UQ_DocumentNumber UNIQUE (DocumentNumber)
GO

ALTER TABLE User
  ADD CONSTRAINT UQ_UserName UNIQUE (UserName)
GO

ALTER TABLE User
  ADD CONSTRAINT UQ_Email UNIQUE (Email)
GO

CREATE TABLE UserAvatarBodyParts
(
  UserAvarBodyPartsID int      NOT NULL IDENTITY(1,1),
  BodyPartName        nvarchar NOT NULL,
  BodyPartAnimation   int      NOT NULL,
  CreatedAt           datetime,
  UpdatedAt           datetime,
  UserID              int      NOT NULL,
  CONSTRAINT PK_UserAvatarBodyParts PRIMARY KEY (UserAvarBodyPartsID)
)
GO

ALTER TABLE UserAvatarBodyParts
  ADD CONSTRAINT UQ_UserAvarBodyPartsID UNIQUE (UserAvarBodyPartsID)
GO

CREATE TABLE Game_economy
(
  EconomyId int      NOT NULL IDENTITY(1,1),
  UserID    int      NOT NULL,
  Coins     int     ,
  Badge     nvarchar,
  CreatedAt datetime,
  UpdatedAt datetime,
  CONSTRAINT PK_Game_economy PRIMARY KEY (EconomyId)
)
GO

ALTER TABLE Game_economy
  ADD CONSTRAINT UQ_EconomyId UNIQUE (EconomyId)
GO

CREATE TABLE Emotion
(
  EmotionId int      NOT NULL IDENTITY(1,1),
  Emotion   nvarchar,
  date      datetime,
  CreatedAt datetime,
  UpdatedAt datetime,
  UserID    int      NOT NULL,
  CONSTRAINT PK_Emotion PRIMARY KEY (EmotionId)
)
GO

ALTER TABLE Emotion
  ADD CONSTRAINT UQ_EmotionId UNIQUE (EmotionId)
GO

CREATE TABLE Questionaries
(
  QuestionaryId int      NOT NULL IDENTITY(1,1),
  Question      nvarchar,
  QuestionKind  nvarchar,
  createdAt     datetime,
  updatedBy     datetime,
  AnswerId      int      NOT NULL,
  CONSTRAINT PK_Questionaries PRIMARY KEY (QuestionaryId)
)
GO

ALTER TABLE Questionaries
  ADD CONSTRAINT UQ_QuestionaryId UNIQUE (QuestionaryId)
GO

CREATE TABLE QuestionaryAnswers
(
  AnswerId   int      NOT NULL IDENTITY(1,1),
  Answer     nvarchar,
  AnswerKind nvarchar,
  CreatedAt  datetime,
  updatedAt  datetime,
  WeightId   int      NOT NULL,
  EmotionId  int      NOT NULL,
  CONSTRAINT PK_QuestionaryAnswers PRIMARY KEY (AnswerId)
)
GO

ALTER TABLE QuestionaryAnswers
  ADD CONSTRAINT UQ_AnswerId UNIQUE (AnswerId)
GO

CREATE TABLE Weight
(
  WeightId  int      NOT NULL IDENTITY(1,1),
  UserID    int      NOT NULL,
  date      datetime,
  weight    int     ,
  CreatedAt datetime,
  UpdatedAt datetime,
  CONSTRAINT PK_Weight PRIMARY KEY (WeightId)
)
GO

ALTER TABLE Weight
  ADD CONSTRAINT UQ_WeightId UNIQUE (WeightId)
GO

CREATE TABLE Food_Item
(
  ItemId     int      NOT NULL IDENTITY(1,1),
  SupplyId   int      NOT NULL,
  FoodDishId int      NOT NULL,
  ItemName   nvarchar,
  Sodio      int      DEFAULT 0,
  Water      int      DEFAULT 0,
  CreatedAt  datetime,
  UpdatedAt  datetime,
  CONSTRAINT PK_Food_Item PRIMARY KEY (ItemId)
)
GO

ALTER TABLE Food_Item
  ADD CONSTRAINT UQ_ItemId UNIQUE (ItemId)
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  'could be on Gr or Mgr. the table should unfy the units', 'user', dbo, 'table', 'Food_Item', 'column', 'Sodio'
GO

CREATE TABLE Suppliers
(
  SupplyId  int      NOT NULL IDENTITY(1,1),
  SupplKind nvarchar,
  CreatedAt datetime,
  UpdatedAt datetime,
  CONSTRAINT PK_Suppliers PRIMARY KEY (SupplyId)
)
GO

ALTER TABLE Suppliers
  ADD CONSTRAINT UQ_SupplyId UNIQUE (SupplyId)
GO

CREATE TABLE FoodDish
(
  FoodDishId int      NOT NULL IDENTITY(1,1),
  UserID     int      NOT NULL,
  Sodio      int      DEFAULT 0,
  Whater     int      DEFAULT 0,
  FoodItems  nvarchar,
  CreatedAt  datetime,
  UpdatedAt  datetime,
  CONSTRAINT PK_FoodDish PRIMARY KEY (FoodDishId)
)
GO

ALTER TABLE FoodDish
  ADD CONSTRAINT UQ_FoodDishId UNIQUE (FoodDishId)
GO

CREATE TABLE greetings
(
  GreetingId int      NOT NULL IDENTITY(1,1),
  UserID     int      NOT NULL,
  WeightId   int      NOT NULL,
  EmotionId  int      NOT NULL,
  FoodDishId int      NOT NULL,
  CreatedAt  datetime,
  UpdatedAt  datetime,
  CONSTRAINT PK_greetings PRIMARY KEY (GreetingId)
)
GO

ALTER TABLE greetings
  ADD CONSTRAINT UQ_GreetingId UNIQUE (GreetingId)
GO

ALTER TABLE User
  ADD CONSTRAINT FK_DocumentTypes_TO_User
    FOREIGN KEY (DocumentTypeId)
    REFERENCES DocumentTypes (DocumentTypeId)
GO

ALTER TABLE User
  ADD CONSTRAINT FK_Cities_TO_User
    FOREIGN KEY (CityId)
    REFERENCES Cities (CityId)
GO

ALTER TABLE User
  ADD CONSTRAINT FK_Roles_TO_User
    FOREIGN KEY (RoleId)
    REFERENCES Roles (RoleId)
GO

ALTER TABLE DataConsent
  ADD CONSTRAINT FK_User_TO_DataConsent
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE UserAvatarBodyParts
  ADD CONSTRAINT FK_User_TO_UserAvatarBodyParts
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE PasswordRecovery
  ADD CONSTRAINT FK_User_TO_PasswordRecovery
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE SessionsLogs
  ADD CONSTRAINT FK_User_TO_SessionsLogs
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE Weight
  ADD CONSTRAINT FK_User_TO_Weight
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE Emotion
  ADD CONSTRAINT FK_User_TO_Emotion
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE QuestionaryAnswers
  ADD CONSTRAINT FK_Weight_TO_QuestionaryAnswers
    FOREIGN KEY (WeightId)
    REFERENCES Weight (WeightId)
GO

ALTER TABLE QuestionaryAnswers
  ADD CONSTRAINT FK_Emotion_TO_QuestionaryAnswers
    FOREIGN KEY (EmotionId)
    REFERENCES Emotion (EmotionId)
GO

ALTER TABLE Questionaries
  ADD CONSTRAINT FK_QuestionaryAnswers_TO_Questionaries
    FOREIGN KEY (AnswerId)
    REFERENCES QuestionaryAnswers (AnswerId)
GO

ALTER TABLE Food_Item
  ADD CONSTRAINT FK_Suppliers_TO_Food_Item
    FOREIGN KEY (SupplyId)
    REFERENCES Suppliers (SupplyId)
GO

ALTER TABLE Food_Item
  ADD CONSTRAINT FK_FoodDish_TO_Food_Item
    FOREIGN KEY (FoodDishId)
    REFERENCES FoodDish (FoodDishId)
GO

ALTER TABLE FoodDish
  ADD CONSTRAINT FK_User_TO_FoodDish
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE greetings
  ADD CONSTRAINT FK_User_TO_greetings
    FOREIGN KEY (UserID)
    REFERENCES User (UserID)
GO

ALTER TABLE greetings
  ADD CONSTRAINT FK_Weight_TO_greetings
    FOREIGN KEY (WeightId)
    REFERENCES Weight (WeightId)
GO

ALTER TABLE greetings
  ADD CONSTRAINT FK_Emotion_TO_greetings
    FOREIGN KEY (EmotionId)
    REFERENCES Emotion (EmotionId)
GO

ALTER TABLE greetings
  ADD CONSTRAINT FK_FoodDish_TO_greetings
    FOREIGN KEY (FoodDishId)
    REFERENCES FoodDish (FoodDishId)
GO

-- Indexes for Authentication Tables
CREATE INDEX IX_User_DocumentNumber ON User (DocumentNumber);
CREATE INDEX IX_User_Email ON User (Email);
CREATE INDEX IX_User_CityId ON User (CityId);
CREATE INDEX IX_User_RoleId ON User (RoleId);
CREATE INDEX IX_SessionsLogs_UserID ON SessionsLogs (UserID);

-- Indexes for Bathroom Tables
CREATE INDEX IX_Weight_UserID ON Weight (UserID);
CREATE INDEX IX_Weight_date ON Weight (date);
CREATE INDEX IX_Emotion_UserID ON Emotion (UserID);
CREATE INDEX IX_QuestionaryAnswers_WeightId ON QuestionaryAnswers (WeightId);
CREATE INDEX IX_QuestionaryAnswers_EmotionId ON QuestionaryAnswers (EmotionId);

-- Indexes for Kitchen Tables
CREATE INDEX IX_Food_Item_SupplyId ON Food_Item (SupplyId);
CREATE INDEX IX_Food_Item_FoodDishId ON Food_Item (FoodDishId);
CREATE INDEX IX_FoodDish_UserID ON FoodDish (UserID);

-- Indexes for LivingRoom Tables
CREATE INDEX IX_greetings_UserID ON greetings (UserID);
CREATE INDEX IX_greetings_WeightId ON greetings (WeightId);
CREATE INDEX IX_greetings_EmotionId ON greetings (EmotionId);
CREATE INDEX IX_greetings_FoodDishId ON greetings (FoodDishId);

-- Indexes for Foreign Key Columns
CREATE INDEX IX_DataConsent_UserID ON DataConsent (UserID);
CREATE INDEX IX_PasswordRecovery_UserID ON PasswordRecovery (UserID);
CREATE INDEX IX_UserAvatarBodyParts_UserID ON UserAvatarBodyParts (UserID);
CREATE INDEX IX_Questionaries_AnswerId ON Questionaries (AnswerId);
GO
-- Schemas

CREATE SCHEMA Authentication;
GO

CREATE SCHEMA Bathroom;
GO

CREATE SCHEMA Kitchen;
GO

CREATE SCHEMA LivingRoom;
GO

ALTER SCHEMA Authentication TRANSFER dbo.User;
ALTER SCHEMA Authentication TRANSFER dbo.Roles;
ALTER SCHEMA Authentication TRANSFER dbo.Cities;
ALTER SCHEMA Authentication TRANSFER dbo.DataConsent;
ALTER SCHEMA Authentication TRANSFER dbo.DocumentTypes;
ALTER SCHEMA Authentication TRANSFER dbo.PasswordRecovery;
ALTER SCHEMA Authentication TRANSFER dbo.SessionsLogs;
ALTER SCHEMA Authentication TRANSFER dbo.UserAvatarBodyParts;

ALTER SCHEMA Bathroom TRANSFER dbo.Emotion;
ALTER SCHEMA Bathroom TRANSFER dbo.Weight;
ALTER SCHEMA Bathroom TRANSFER dbo.Questionaries;
ALTER SCHEMA Bathroom TRANSFER dbo.QuestionaryAnswers;

ALTER SCHEMA Kitchen TRANSFER dbo.Food_Item;
ALTER SCHEMA Kitchen TRANSFER dbo.Suppliers;
ALTER SCHEMA Kitchen TRANSFER dbo.FoodDish;

ALTER SCHEMA LivingRoom TRANSFER dbo.greetings;

-- Authentication Schema
GRANT SELECT ON SCHEMA::Authentication TO ReadOnlyRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Authentication TO ReadWriteRole;
GRANT CONTROL ON SCHEMA::Authentication TO AdminRole;

-- Bathroom Schema
GRANT SELECT ON SCHEMA::Bathroom TO ReadOnlyRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Bathroom TO ReadWriteRole;
GRANT CONTROL ON SCHEMA::Bathroom TO AdminRole;

-- Kitchen Schema
GRANT SELECT ON SCHEMA::Kitchen TO ReadOnlyRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Kitchen TO ReadWriteRole;
GRANT CONTROL ON SCHEMA::Kitchen TO AdminRole;

-- LivingRoom Schema
GRANT SELECT ON SCHEMA::LivingRoom TO ReadOnlyRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::LivingRoom TO ReadWriteRole;
GRANT CONTROL ON SCHEMA::LivingRoom TO AdminRole;

-- Users
-- Create ReadOnlyUser
CREATE LOGIN ReadOnlyUser WITH PASSWORD = 'StrongPassword123!';
CREATE USER ReadOnlyUser FOR LOGIN ReadOnlyUser;

-- Grant SELECT permissions on all schemas
GRANT SELECT ON SCHEMA::Authentication TO ReadOnlyUser;
GRANT SELECT ON SCHEMA::Bathroom TO ReadOnlyUser;
GRANT SELECT ON SCHEMA::Kitchen TO ReadOnlyUser;
GRANT SELECT ON SCHEMA::LivingRoom TO ReadOnlyUser;

-- Create ReadWriteUser
CREATE LOGIN ReadWriteUser WITH PASSWORD = 'StrongPassword123!';
CREATE USER ReadWriteUser FOR LOGIN ReadWriteUser;

-- Grant SELECT, INSERT, UPDATE, DELETE permissions on all schemas
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Authentication TO ReadWriteUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Bathroom TO ReadWriteUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Kitchen TO ReadWriteUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::LivingRoom TO ReadWriteUser;

-- Create AdminUser
CREATE LOGIN AdminUser WITH PASSWORD = 'StrongPassword123!';
CREATE USER AdminUser FOR LOGIN AdminUser;

-- Grant CONTROL permissions on all schemas
GRANT CONTROL ON SCHEMA::Authentication TO AdminUser;
GRANT CONTROL ON SCHEMA::Bathroom TO AdminUser;
GRANT CONTROL ON SCHEMA::Kitchen TO AdminUser;
GRANT CONTROL ON SCHEMA::LivingRoom TO AdminUser;

-- Create SchemaSpecificUser for Kitchen schema
CREATE LOGIN KitchenUser WITH PASSWORD = 'StrongPassword123!';
CREATE USER KitchenUser FOR LOGIN KitchenUser;

-- Grant SELECT, INSERT, UPDATE, DELETE permissions on Kitchen schema
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Kitchen TO KitchenUser;

CREATE ROLE ReadOnlyRole;
GRANT SELECT ON SCHEMA::Authentication TO ReadOnlyRole;
GRANT SELECT ON SCHEMA::Bathroom TO ReadOnlyRole;
GRANT SELECT ON SCHEMA::Kitchen TO ReadOnlyRole;
GRANT SELECT ON SCHEMA::LivingRoom TO ReadOnlyRole;

-- Add ReadOnlyUser to ReadOnlyRole
ALTER ROLE ReadOnlyRole ADD MEMBER ReadOnlyUser;

CREATE ROLE KitchenRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Kitchen TO KitchenRole;

-- Add KitchenUser to KitchenRole
ALTER ROLE KitchenRole ADD MEMBER KitchenUser;