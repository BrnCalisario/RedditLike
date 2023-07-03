USE MASTER
GO

if exists(select * from sys.databases where name = 'Reddit')
	drop database Reddit
go

CREATE DATABASE Reddit
GO

USE Reddit
GO

CREATE TABLE ImageData 
(
	ID INT IDENTITY PRIMARY KEY,
	Photo VARBINARY(MAX) NOT NULL
)
GO

CREATE TABLE [User]
(
    ID INT IDENTITY PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(200) NOT NULL UNIQUE,
    ProfilePicture INT REFERENCES ImageData(ID) NULL,
    BirthDate DATE NOT NULL,
    [Password] VARBINARY(150) NOT NULL,
    Salt VARCHAR(30) NOT NULL
)
GO

CREATE TABLE [Group]
(
    ID INT IDENTITY PRIMARY KEY,
    OwnerID INT REFERENCES [User](ID) NOT NULL,
    [Name] VARCHAR(50) NOT NULL UNIQUE,
    [Description] VARCHAR(150) NULL,
    [Image] INT REFERENCES ImageData(ID) NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT(GETDATE())
)
GO

CREATE TABLE [Post]
(
    ID INT IDENTITY PRIMARY KEY,
    AuthorID INT REFERENCES [User](ID) NOT NULL,
    GroupID INT REFERENCES [Group](ID) NOT NULL,
    Title VARCHAR(50) NOT NULL,
    Content VARCHAR(400) NOT NULL,
    IndexedImage INT REFERENCES ImageData(ID) NULL,
	LikeCount INT NOT NULL DEFAULT(0),
	PostDate DATETIME NOT NULL DEFAULT(GETDATE())
)
GO

CREATE TABLE [Comment]
(
	ID INT IDENTITY PRIMARY KEY,
	AuthorID INT REFERENCES [User](ID) NOT NULL,
	PostID INT REFERENCES [Post](ID) NOT NULL,
	Content VARCHAR(150) NOT NULL,
	PostDate DATETIME NOT NULL DEFAULT(GETDATE())
)

CREATE TABLE [Upvote]
(
    ID INT IDENTITY PRIMARY KEY,
    UserID INT REFERENCES [User](ID) NOT NULL,
    PostID INT REFERENCES [Post](ID) NOT NULL,
    [Value] BIT DEFAULT(1) NOT NULL
)
GO

CREATE TABLE [Role]
(
	ID INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(30) NOT NULL,
	[GroupID] INT REFERENCES [Group](ID)
)
GO

INSERT INTO [Role] ([Name], [GroupID])
VALUES
	('Membro', NULL),
	('Admin', NULL)
GO

CREATE TABLE [Permission]
(
	ID INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(30) NOT NULL,
)
GO

INSERT INTO [Permission] ([Name])
VALUES 
	('Post'), 
	('Delete'), 
	('EditPost'), 
	('Promote'), 
	('CreateRole'), 
	('Ban'), 
	('DropGroup')
GO

CREATE TABLE [RolePermission]
(
	ID INT IDENTITY PRIMARY KEY,	
	RoleID INT REFERENCES [Role](ID) NOT NULL,
	PermissionID INT REFERENCES [Permission](ID) NOT NULL
)
GO

INSERT INTO [RolePermission] (RoleID, PermissionID)
VALUES
	(1, 1),
	(2, 1),
	(2, 2),
	(2, 3),
	(2, 4),
	(2, 5),
	(2, 6),
	(2, 7)
GO	


CREATE TABLE [UserGroup]
(
    ID INT IDENTITY PRIMARY KEY,
    UserID INT NOT NULL REFERENCES [User](ID),
    GroupID INT NOT NULL REFERENCES [Group](ID),
	RoleID INT NULL REFERENCES [Role](ID)
)
GO


/* Like Trigger */

CREATE TRIGGER [LikeTrigger]
	ON [Upvote]
	AFTER INSERT
AS
BEGIN
	DECLARE
	@POST_ID INT,
	@LIKE_VALUE BIT

	SELECT @LIKE_VALUE = [Value], @POST_ID = [PostID] FROM INSERTED

	IF @LIKE_VALUE = 1
		UPDATE [Post] 
		SET [Post].LikeCount = [Post].LikeCount + 1 
		WHERE [Post].[ID] = @POST_ID
	ELSE 
		UPDATE [Post] 
		SET [Post].LikeCount = [Post].LikeCount -1
		WHERE [Post].[ID] = @POST_ID
END
GO


CREATE TRIGGER [LikeCancel]
	ON [Upvote]
	AFTER DELETE
AS
BEGIN
	DECLARE 
	@POST_ID INT,
	@LIKE_VALUE BIT

	SELECT @LIKE_VALUE = [Value], @POST_ID = [PostID] FROM deleted

	IF @LIKE_VALUE = 1
			UPDATE Post
			SET Post.LikeCount = Post.LikeCount - 1
			WHERE ID = @POST_ID
	ELSE IF @LIKE_VALUE = 0
		UPDATE Post
		SET POST.LikeCount = Post.LikeCount + 1
		WHERE ID = @POST_ID
END
GO

CREATE TRIGGER [OwnerRelation]
	ON [Group]
	AFTER INSERT
AS 
BEGIN
	DECLARE 
	@OWNER_ID INT,
	@GROUP_ID INT

	SELECT @GROUP_ID = ID, @OWNER_ID = OwnerID FROM inserted

	INSERT INTO UserGroup (UserID, GroupID, RoleID) VALUES (@OWNER_ID, @GROUP_ID, 2)
END
GO

SELECT * FROM [User]
SELECT * FROM [ImageData]
SELECT * FROM [Group]
SELECT * FROM [UserGroup]

SELECT * FROM [Post]
SELECT * FROM [Upvote]
SELECT * FROM [Comment]

SELECT r.ID, r.Name, p.Name FROM [RolePermission] rp
JOIN [Permission] p ON rp.PermissionID = p.ID
JOIN [Role] r ON rp.RoleID = r.ID