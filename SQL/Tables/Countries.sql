CREATE TABLE Countries (
	CountryID TINYINT IDENTITY(1, 1),
	CountryName NVARCHAR(250) NOT NULL
	
	CONSTRAINT UQ_Countries_Name UNIQUE (CountryName),
	CONSTRAINT PK_Countries PRIMARY KEY (CountryID)
)
