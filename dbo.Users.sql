CREATE TABLE [dbo].[Users] (
    [UserID]    INT           IDENTITY (1, 1) NOT NULL,
    [Email]     VARCHAR (50)  NOT NULL,
    [FirstName] VARCHAR (50)  NOT NULL,
    [LastName]  VARCHAR (50)  NOT NULL,
    [Password]  VARCHAR (100) NOT NULL,
    [Birthday]  DATE          NOT NULL,
    [Role]      VARCHAR (50)  NOT NULL,
    [UserName]  VARCHAR (50)  NOT NULL,
    [Comment]   VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

