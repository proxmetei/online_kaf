CREATE TABLE [dbo].[Dependencies] (
    [ChatId] INT NOT NULL,
    [UserId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ChatId] ASC, [UserId] ASC),
    FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID])
);

