CREATE PROCEDURE [dbo].[spAddProjectTeamMemberTitle]
	@Title VARCHAR(255),
	@IsDefault bit = 0,
	@Remarks VARCHAR(Max)
AS
DECLARE @orderNo int = 0
SELECT @orderNo = MAX(OrderNo) FROM tblTeamMemberTitle

IF NOT EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE Title = @Title )
BEGIN
	INSERT INTO tblTeamMemberTitle (Title, IsDefault, Remarks, OrderNo) VALUES (@Title, @IsDefault, @Remarks, ISNULL(@orderNo,0) + 1)
END
ELSE
BEGIN
	RAISERROR ('This title existed in database. Please use another one',16,1)
    RETURN
END	
