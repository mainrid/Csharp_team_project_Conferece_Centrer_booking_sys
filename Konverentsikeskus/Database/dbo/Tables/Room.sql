CREATE TABLE [dbo].[Room] (
    [RoomID] INT         IDENTITY (1, 1) NOT NULL,
    [Name]   NCHAR (100) NOT NULL,
    [Seats]  INT         NOT NULL,
    [Active] BIT         NOT NULL,
    CONSTRAINT [PK_Ruum] PRIMARY KEY CLUSTERED ([RoomID] ASC)
);



