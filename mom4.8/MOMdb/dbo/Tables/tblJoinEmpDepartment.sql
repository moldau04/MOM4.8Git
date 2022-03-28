CREATE TABLE [dbo].[tblJoinEmpDepartment] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [Emp]        INT NULL,
    [Department] INT NULL, 
    CONSTRAINT [PK_tblJoinEmpDepartment] PRIMARY KEY ([ID])
);

