CREATE TABLE BestTimesForVisit (
	BestVisitTimeID TINYINT IDENTITY(1, 1),
	BestVisitTime NVARCHAR(100) NOT NULL, 

	CONSTRAINT PK_BestTimesForVisit PRIMARY KEY (BestVisitTimeID),
	CONSTRAINT UQ_BestTimesForVisit UNIQUE (BestVisitTime)
)
