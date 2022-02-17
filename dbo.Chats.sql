CREATE TABLE [dbo].[Chats] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (30)  NOT NULL,
    [GUID] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

