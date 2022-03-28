CREATE TABLE [dbo].[tblUserRole]
(
    [RoleId] INT NOT NULL, 
    [UserId] INT NOT NULL,
    [UpdatedDate] DateTime NULL, 
    [UpdatedBy] VARCHAR(50) NULL, 
    CONSTRAINT [PK_tblUserRole] PRIMARY KEY ([RoleId], [UserId]) 
)
