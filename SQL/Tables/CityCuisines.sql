CREATE TABLE CityCuisines (
	CityID INT NOT NULL,
	CuisineID INT NOT NULL,

	CONSTRAINT PK_CityCuisines PRIMARY KEY (CityID, CuisineID),
	CONSTRAINT FK_CityCuisines_Cities FOREIGN KEY (CityID) REFERENCES Cities(CityID),
	CONSTRAINT FK_CityCuisines_Cuisines FOREIGN KEY (CuisineID) REFERENCES Cuisines(CuisineID)
)
