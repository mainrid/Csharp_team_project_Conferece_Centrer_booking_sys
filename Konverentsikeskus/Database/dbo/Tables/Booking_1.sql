CREATE TABLE [dbo].[Booking] (
    [BookingID]      INT            IDENTITY (1, 1) NOT NULL,
    [Date]           DATE           NOT NULL,
    [RoomID]         INT            NOT NULL,
    [CustomerID]     INT            NOT NULL,
    [Participants]   INT            NOT NULL,
    [Created]        DATETIME       NOT NULL,
    [Confirmed]      DATETIME       NULL,
    [AdminID]        INT            NOT NULL,
    [AdditionalInfo] NVARCHAR (500) NOT NULL,
    [Archived]       DATETIME       NULL,
    CONSTRAINT [PK_Broneering] PRIMARY KEY CLUSTERED ([BookingID] ASC),
    CONSTRAINT [FK_Broneering_Administraator] FOREIGN KEY ([AdminID]) REFERENCES [dbo].[Administrator] ([AdminID]),
    CONSTRAINT [FK_Broneering_Klient] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomertID]),
    CONSTRAINT [FK_Broneering_Ruum] FOREIGN KEY ([RoomID]) REFERENCES [dbo].[Room] ([RoomID])
);

