CREATE PROCEDURE [dbo].[spAddEstimateForm]
	@ID int,
	@Estimate int,
	@JobTID int,
	@Name varchar(100),
	@FileName VARCHAR(100),
	@FilePath  VARCHAR(500),
	@PdfFilePath  VARCHAR(500),
	@Remark  VARCHAR(500),
	@MIME  VARCHAR(50),
	@user  VARCHAR(50)
AS
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[EstimateForm]
           ([Estimate]
		   ,[JobTID]
           ,[Name]
           ,[FileName]
           ,[FilePath]
           ,[PdfFilePath]
           ,[Remark]
           ,[MIME]
           ,[AddedBy]
           ,[AddedOn]
		) VALUES (
			@Estimate,
			@JobTID,
			@Name,
			@FileName,
			@FilePath,
			@PdfFilePath,
			@Remark,
			@MIME,
			@user,
			GETDATE()
		)
		SET @ID	= SCOPE_IDENTITY()
	END

	ELSE

	BEGIN
		UPDATE [dbo].[EstimateForm]
           SET [Estimate] = @Estimate,
		   [JobTID] = @JobTID,
           [Name] = @Name,
           [FileName] = @FileName,
           [FilePath] = @FilePath,
		   [PdfFilePath] = @PdfFilePath,
		   [Remark] = @Remark,
           [MIME] = @MIME,
           [AddedBy] = @user,
           [AddedOn] = GETDATE()
		WHERE ID = @ID
	END
RETURN @ID