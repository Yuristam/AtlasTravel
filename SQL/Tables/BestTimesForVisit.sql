CREATE TABLE BestTimesForVisit (
	BestVisitTimeID INT IDENTITY(1, 1),
	BestVisitTime NVARCHAR(100) NOT NULL, 

	CONSTRAINT PK_BestTimesForVisit PRIMARY KEY (BestVisitTimeID)
)
