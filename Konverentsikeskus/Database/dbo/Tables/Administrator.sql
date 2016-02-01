CREATE TABLE [dbo].[Administrator] (
    [AdminID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50)  NOT NULL,
    [CanChange] BIT            NOT NULL,
    [Password]  NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Administraator] PRIMARY KEY CLUSTERED ([AdminID] ASC)
);

