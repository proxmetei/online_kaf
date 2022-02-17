CREATE TABLE [dbo].[Messages] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Color]  INT          NOT NULL,
    [Text]   VARCHAR (50) NOT NULL,
    [ChatId] INT          NOT NULL,
    [UserId] INT          NOT NULL,
    [DocId]  INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]),
    FOREIGN KEY ([DocId]) REFERENCES [dbo].[Documents] ([Id])
);

