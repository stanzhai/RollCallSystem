CREATE TABLE [dbo].[ClassInfo] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [ClassName] NVARCHAR (32) NOT NULL,
    [Admin]     NVARCHAR (32) NOT NULL,
    [Phone]     NVARCHAR (32) NOT NULL,
    [Password]  NVARCHAR (16) NOT NULL
);

