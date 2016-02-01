CREATE TABLE [dbo].[Contact] (
    [ContactID]     INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT           NOT NULL,
    [ContactTypeID] INT           NOT NULL,
    [Value]         NVARCHAR (50) NULL,
    [Created]       DATETIME      NULL,
    CONSTRAINT [PK_Kontakt] PRIMARY KEY CLUSTERED ([ContactID] ASC),
    CONSTRAINT [FK_Kontakt_Klient] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomertID]),
    CONSTRAINT [FK_Kontakt_Kontaktliik] FOREIGN KEY ([ContactTypeID]) REFERENCES [dbo].[ContactType] ([ContactTypeID])
);

