CREATE TABLE [dbo].[Customer] (
    [CustomerID]     INT           IDENTITY (1, 1) NOT NULL,
    [CompanyName]    NVARCHAR (50) NOT NULL,
    [CompanyPICName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Klient] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);



