CREATE TABLE [dbo].[Record] (
    [ID]        UNIQUEIDENTIFIER NOT NULL,
    [StudentNo] INT              NOT NULL,
    [Record]    NVARCHAR (2)     NOT NULL,
    [IndexID]   UNIQUEIDENTIFIER NOT NULL,
    [Remark]    NVARCHAR (32)    NULL
);

