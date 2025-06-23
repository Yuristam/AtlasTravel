CREATE TABLE Landmarks (
	LandmarkID INT IDENTITY(1, 1),
	Name NVARCHAR(250) NOT NULL,
	Description NVARCHAR(MAX) NULL,
	BestVisitTimeID INT NOT NULL DEFAULT 1, -- "All-Season"
	LandscapeTypeID INT NOT NULL,

	CONSTRAINT PK_Landmarks PRIMARY KEY (LandmarkID),
	CONSTRAINT FK_Landmarks_BestTimesForVisit FOREIGN KEY (BestVisitTimeID)
		REFERENCES BestTimesForVisit(BestVisitTimeID),
	CONSTRAINT FK_Landmarks_LandscapeTypes FOREIGN KEY (LandscapeTypeID)
		REFERENCES LandscapeTypes (LandscapeTypeID)
)