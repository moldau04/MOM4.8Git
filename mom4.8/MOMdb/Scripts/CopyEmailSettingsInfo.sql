DECLARE @DestDB Varchar(255) = '';
DECLARE @SourDB Varchar(255) = '';

DECLARE @SQL1 NVARCHAR(MAX)
DECLARE @SQL2 NVARCHAR(MAX)
DECLARE @SQL3 NVARCHAR(MAX)

IF @DestDB != '' AND @SourDB != ''
BEGIN
	-- 1. For update tblUser.EmailAccount
	SET @SQL1 = 
	'
	Update u1
	set u1.EmailAccount = u2.EmailAccount
	from ' + @DestDB + '.tbluser u1 
	inner join '+ @SourDB +'.tbluser u2 on u1.fuser = u2.fuser
	inner join '+ @SourDB +'.tblemailaccounts m2 on m2.UserId = u2.ID
	left join ' + @DestDB + '.tblemailaccounts m1 on m1.UserId = m2.UserId 
	where m1.UserId is null
	'

	PRINT @SQL1

	-- 2. For update rol.Email
	SET @SQL2 = 
	'
	Update r1
	set r1.Email = r2.EMail
	from ' + @DestDB + '.tbluser u1 
	inner join ' + @SourDB + '.tbluser u2 on u1.fuser = u2.fuser
	inner join ' + @SourDB + '.emp e2 on e2.CallSign = u2.fUser
	inner join ' + @DestDB + '.emp e1 on e1.CallSign = u2.fUser
	inner join ' + @DestDB + '.Rol r1 on r1.ID = e1.Rol
	inner join ' + @SourDB + '.Rol r2 on r2.ID = e2.Rol
	inner join ' + @SourDB + '.tblemailaccounts m2 on m2.UserId = u2.ID
	left join ' + @DestDB + '.tblemailaccounts m1 on m1.UserId = m2.UserId 
	where m1.UserId is not null
	'

	PRINT @SQL2

	-- 1. For insert new row to transel.dbo.tblemailaccounts
	SET @SQL3 = 
	'
	insert into ' + @DestDB + '.tblemailaccounts
	(InServer, InServerType, InPassword, InPort, OutServer, OutUsername, OutPassword, OutPort, [SSL], UserId, LastFetch, BccEmail, TakeASentEmailCopy)
	select m2.InServer
		, m2.InServerType
		, m2.InPassword
		, m2.InPort
		, m2.OutServer
		, m2.OutUsername
		, m2.OutPassword
		, m2.OutPort
		, m2.[SSL]
		, m2.UserId
		, m2.LastFetch
		, m2.BccEmail
		, m2.TakeASentEmailCopy
	from ' + @DestDB + '.tbluser u1 
	inner join ' + @SourDB + '.tbluser u2 on u1.fuser = u2.fUser
	inner join ' + @SourDB + '.emp e2 on e2.CallSign = u2.fUser
	inner join ' + @SourDB + '.tblemailaccounts m2 on m2.UserId = u2.ID
	left join ' + @DestDB + '.tblemailaccounts m1 on m1.UserId = m2.UserId 
	where m1.UserId is null
	'

	PRINT @SQL3
	BEGIN TRANSACTION
		EXEC sys.sp_executesql @SQL1
		EXEC sys.sp_executesql @SQL2
		EXEC sys.sp_executesql @SQL3
	COMMIT
END
ELSE
BEGIN
	Print 'Please update @DestDB and @SourDB following format. EX: @DestDB = ''Migrateddb.dbo''; @SourDB = ''TEIMOM.dbo'''
END