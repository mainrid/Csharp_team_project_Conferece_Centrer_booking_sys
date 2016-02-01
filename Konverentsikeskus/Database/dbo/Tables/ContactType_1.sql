CREATE TABLE [dbo].[ContactType] (
    [ContactTypeID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (500) NOT NULL,
    [Compulsory]    BIT            NOT NULL,
    CONSTRAINT [PK_Kontaktliik] PRIMARY KEY CLUSTERED ([ContactTypeID] ASC)
);

