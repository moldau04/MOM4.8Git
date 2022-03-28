CREATE PROCEDURE [dbo].[spInsertCRUDUserRolePermissionLogs]
	@UpdatedBy varchar(100),
	@RefId int,
	@Field varchar(75),
	@PermissionString varchar(255),
	@ShowingColumns varchar(6) = '1234'
AS
Declare @logPermissionString varchar(255)-- = UpdatePermissionStringForLogs(@CustomerPermissions)
IF(ISNULL(@PermissionString,'') != '')
BEGIN
	IF (@ShowingColumns is not null AND LEN(@ShowingColumns) > 1)
	BEGIN
		IF (@ShowingColumns = '1234')
		BEGIN 	
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
		ELSE IF (@ShowingColumns = '134')
		BEGIN 	
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
		ELSE IF (@ShowingColumns = '13')
		BEGIN 	
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										--+ ', View - ' + SUBSTRING(@PermissionString,4,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
		ELSE IF (@ShowingColumns = '14')
		BEGIN 	
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										--+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
		ELSE IF (@ShowingColumns = '12346')
		BEGIN 	
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
										+ ', Report - ' + SUBSTRING(@PermissionString,6,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
	END
	ELSE
		Declare @colIndex int = Convert(int,@ShowingColumns)

		IF(Len(@PermissionString) >= @colIndex)
		BEGIN 	
			Set @logPermissionString = SUBSTRING(@PermissionString,@colIndex,1)
	
			EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,'',@logPermissionString
		END
END