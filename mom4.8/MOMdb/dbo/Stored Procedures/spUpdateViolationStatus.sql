CREATE PROCEDURE [dbo].[spUpdateViolationStatus]
	@ID INT,
	@Type VARCHAR(50),
	@Remarks text
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM VioStatus WHERE ID = @ID
IF @IsExisted > 0
BEGIN
	SET @IsExisted = 0
	SELECT @IsExisted = Count(*) FROM VioStatus WHERE ID <> @ID AND [Type] = @Type
	IF @IsExisted = 0
	BEGIN
		UPDATE VioStatus SET
			[Type] =@Type,
			Remarks = @Remarks			
		WHERE ID = @ID
	END
	ELSE
	BEGIN
		RAISERROR ('This violation status already exists in the database. Please check and try again!',16,1)
		RETURN
	END
END
ELSE
BEGIN
	RAISERROR ('Cannot find this violation status in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END