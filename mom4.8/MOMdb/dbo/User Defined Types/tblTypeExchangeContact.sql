CREATE TYPE [dbo].[tblTypeExchangeContact] AS TABLE(
	[MemberName] NVARCHAR(255) NULL, 
    [MemberEmail] NVARCHAR(255) NULL, 
    [GroupName] VARCHAR(255) NULL, 
    [Type] VARCHAR(255) NULL
)