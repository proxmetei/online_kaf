CREATE TABLE [dbo].[Documents] (
    [Id]   INT             IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50)    NOT NULL,
    [Data] VARBINARY (MAX) NOT NULL,
    [GUID] VARCHAR (100)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

