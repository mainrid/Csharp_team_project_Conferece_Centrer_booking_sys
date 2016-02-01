CREATE TABLE [dbo].[Customer] (
    [CustomertID]   INT           IDENTITY (1, 1) NOT NULL,
    [CompanyName]   NVARCHAR (50) NOT NULL,
    [ContactPerson] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Klient] PRIMARY KEY CLUSTERED ([CustomertID] ASC)
);

