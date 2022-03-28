CREATE PROCEDURE [dbo].[spAddEstimateTemplateForm]
	@ID int,
	@JobTID int,
	@Name varchar(100),
	@FileName VARCHAR(100),
	@FilePath  VARCHAR(500),
	@MIME  VARCHAR(50),
	@user  VARCHAR(50)
AS
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[EstimateTemplate]
           ([JobTID]
           ,[Name]
           ,[FileName]
           ,[FilePath]
           ,[MIME]
           ,[AddedBy]
           ,[AddedOn]
           ,[UpdatedBy]
           ,[UpdatedOn])
		VALUES (
			@JobTID,
			@Name,
			@FileName,
			@FilePath,
			@MIME,
			@user,
			GETDATE(),
			@user,
			getdate()
		)
		SET @ID	= SCOPE_IDENTITY()
	END

	ELSE

	BEGIN
		UPDATE [dbo].[EstimateTemplate]
           SET [JobTID] = @JobTID,
           [Name] = @Name,
           [FileName] = @FileName,
           [FilePath] = @FilePath,
           [MIME] = @MIME,
           [UpdatedBy] = @user,
           [UpdatedOn] = GETDATE()
		WHERE ID = @ID
	END

RETURN @ID
