CREATE TABLE Permissions (
	PermissionID INT IDENTITY(1, 1),
	PermissionName NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_Permissions PRIMARY KEY (PermissionID),
	CONSTRAINT UQ_Permissions_Name UNIQUE (PermissionName)
)