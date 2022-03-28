CREATE PROCEDURE [dbo].[spAddViolationStatus]
	@Type VARCHAR(50),
	@Remarks text
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM VioStatus WHERE [Type] = @Type

IF @IsExisted = 0
BEGIN
	INSERT INTO VioStatus
		(
		[Type],
		Remarks,
		[Count]
		)
		VALUES
		(
		@Type,
		@Remarks,
		0		
		)
END
ELSE
BEGIN
	RAISERROR ('This violation status already exists in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END