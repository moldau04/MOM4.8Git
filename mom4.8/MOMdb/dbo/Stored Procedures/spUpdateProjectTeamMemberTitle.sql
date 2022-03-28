CREATE PROCEDURE [dbo].[spUpdateProjectTeamMemberTitle]
	@Id int,
	@Title VARCHAR(255),
	@IsDefault bit = 0,
	@Remarks VARCHAR(Max)
AS
IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID = @Id )
BEGIN
	IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID = @Id AND Title = 'Project Manager' AND Title != @Title)
	BEGIN
		RAISERROR ('Cannot update this default title',16,1)
		RETURN
	END

	IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID != @Id AND Title = @Title)
	BEGIN
		RAISERROR ('This title existed in database. Please use another one',16,1)
		RETURN
	END

	UPDATE tblTeamMemberTitle SET Title = @Title, IsDefault = @IsDefault , Remarks = @Remarks WHERE Id = @Id
END
ELSE
BEGIN
	RAISERROR ('Cannot find this title in database.',16,1)
    RETURN
END	