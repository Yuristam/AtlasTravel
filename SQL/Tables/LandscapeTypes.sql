CREATE TABLE LandscapeTypes (
	LandscapeTypeID INT IDENTITY(1, 1),
	LandscapeType NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_LandscapeTypes PRIMARY KEY (LandscapeTypeID)
)