﻿CREATE TABLE [USERS] (
	[ID] int IDENTITY(1,1),
	[NAME] varchar(64) NOT NULL,
	[CREATED_DATE] datetime NOT NULL,

	CONSTRAIN [PK_USERS] PRIMARY KEY NONCLUSTERED ([ID])
)
GO

CREATE TABLE [USER_LOCATIONS] (
	[ID] int IDENTITY(1,1),
	[USER_ID] int NOT NULL,
	[UPDATED_DATE] datetime NOT NULL,
	[LATITUDE] float NOT NULL,
	[LONGTITUDE] float NOT NULL,

	CONSTRAINT [PK_USER_LOCATIONS] PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_USER_LOCATIONS_USERS] FOREIGN KEY ([USER_ID]) REFERENCES [USERS]([ID])
)
