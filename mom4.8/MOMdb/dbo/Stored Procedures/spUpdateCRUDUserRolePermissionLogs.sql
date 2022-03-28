CREATE PROCEDURE [dbo].[spUpdateCRUDUserRolePermissionLogs]
	@UpdatedBy varchar(100),
	@RefId int,
	@Field varchar(75),
	@CurrPermissionString varchar(255),
	@PermissionString varchar(255),
	@ShowingColumns varchar(6) = '1234'
AS
Declare @logPermissionString varchar(255)-- = UpdatePermissionStringForLogs(@CustomerPermissions)
Declare @logCurrPermissionString varchar(255)
Declare @comPermissionString varchar(255)
Declare @comCurrPermissionString varchar(255)
IF(ISNULL(@PermissionString,'') != '')
BEGIN
	IF (@ShowingColumns is not null AND LEN(@ShowingColumns) > 1)
	BEGIN
		IF (@ShowingColumns = '1234')
		BEGIN 	
			SET @comPermissionString = SUBSTRING(@PermissionString,1,4)
			SET @comCurrPermissionString = SUBSTRING(@CurrPermissionString,1,4)
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)

			Set @logCurrPermissionString = 'Add - ' + SUBSTRING(@CurrPermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@CurrPermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@CurrPermissionString,3,1)
										+ ', View - ' + SUBSTRING(@CurrPermissionString,4,1)
			IF(@comPermissionString != @comCurrPermissionString)
				EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,@logCurrPermissionString,@logPermissionString
		END
		ELSE IF (@ShowingColumns = '134')
		BEGIN 	
			SET @comPermissionString = SUBSTRING(@PermissionString,1,1) + SUBSTRING(@PermissionString,3,1) + SUBSTRING(@PermissionString,4,1)
			SET @comCurrPermissionString = SUBSTRING(@CurrPermissionString,1,1) + SUBSTRING(@CurrPermissionString,3,1) + SUBSTRING(@CurrPermissionString,4,1)
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)

			Set @logCurrPermissionString = 'Add - ' + SUBSTRING(@CurrPermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@CurrPermissionString,3,1)
										+ ', View - ' + SUBSTRING(@CurrPermissionString,4,1)
			IF(@comPermissionString != @comCurrPermissionString)
				EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,@logCurrPermissionString,@logPermissionString
		END
		ELSE IF (@ShowingColumns = '14')
		BEGIN 	
			SET @comPermissionString = SUBSTRING(@PermissionString,1,1) + SUBSTRING(@PermissionString,4,1)
			SET @comCurrPermissionString = SUBSTRING(@CurrPermissionString,1,1) + SUBSTRING(@CurrPermissionString,4,1)
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										--+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
			Set @logCurrPermissionString = 'Add - ' + SUBSTRING(@CurrPermissionString,1,1)
										--+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										--+ ', Delete - ' + SUBSTRING(@CurrPermissionString,3,1)
										+ ', View - ' + SUBSTRING(@CurrPermissionString,4,1)
			IF(@comPermissionString != @comCurrPermissionString)
				EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,@logCurrPermissionString,@logPermissionString
		END
		ELSE IF (@ShowingColumns = '12346')
		BEGIN 	
			SET @comPermissionString = SUBSTRING(@PermissionString,1,4) + SUBSTRING(@PermissionString,6,1)
			SET @comCurrPermissionString = SUBSTRING(@CurrPermissionString,1,4) + SUBSTRING(@CurrPermissionString,6,1)
			Set @logPermissionString = 'Add - ' + SUBSTRING(@PermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@PermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@PermissionString,3,1)
										+ ', View - ' + SUBSTRING(@PermissionString,4,1)
										+ ', Report - ' + SUBSTRING(@PermissionString,6,1)

			Set @logCurrPermissionString = 'Add - ' + SUBSTRING(@CurrPermissionString,1,1)
										+ ', Edit - ' + SUBSTRING(@CurrPermissionString,2,1)
										+ ', Delete - ' + SUBSTRING(@CurrPermissionString,3,1)
										+ ', View - ' + SUBSTRING(@CurrPermissionString,4,1)
										+ ', Report - ' + SUBSTRING(@CurrPermissionString,6,1)
			IF(@comPermissionString != @comCurrPermissionString)
				EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,@logCurrPermissionString,@logPermissionString
		END
	END
	ELSE
		Declare @colIndex int = Convert(int,@ShowingColumns)

		IF(Len(@PermissionString) >= @colIndex AND Len(@CurrPermissionString) >= @colIndex)
		BEGIN 	
			Set @logPermissionString = SUBSTRING(@PermissionString,@colIndex,1)
			Set @logCurrPermissionString = SUBSTRING(@CurrPermissionString,@colIndex,1)
			IF(@logPermissionString != @logCurrPermissionString)
				EXEC log2_insert @UpdatedBy,'User Role',@RefId,@Field,@logCurrPermissionString,@logPermissionString
		END
END