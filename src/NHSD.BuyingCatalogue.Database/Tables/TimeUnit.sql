﻿CREATE TABLE dbo.TimeUnit
(
    TimeUnitId INT NOT NULL,
    [Name] NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(32) NOT NULL
    CONSTRAINT PK_TimeUnit PRIMARY KEY CLUSTERED (TimeUnitId)
)
