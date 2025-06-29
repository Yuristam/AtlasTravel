CREATE TABLE RolePermissions (
	RoleID INT NOT NULL,
	PermissionID INT NOT NULL,

	CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleID, PermissionID),
	CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleID) 
		REFERENCES Roles(RoleID) ON DELETE CASCADE,
	CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionID) 
		REFERENCES Permissions(PermissionID)
)