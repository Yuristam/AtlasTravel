CREATE TABLE LandscapeTypes (
	LandscapeTypeID TINYINT IDENTITY(1, 1),
	LandscapeType NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_LandscapeTypes PRIMARY KEY (LandscapeTypeID),
	CONSTRAINT UQ_LandscapeType UNIQUE (LandscapeType)
)