CREATE TABLE Cuisines (
	CuisineID INT IDENTITY(1, 1),
	CuisineType NVARCHAR(250) NOT NULL

	CONSTRAINT PK_Cuisines PRIMARY KEY (CuisineID),
	CONSTRAINT UQ_Cuisines_Type UNIQUE (CuisineType)
)