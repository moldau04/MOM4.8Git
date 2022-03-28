CREATE PROCEDURE [dbo].[spDeleteTeamMemberTitle]
	@Id int
AS

IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID = @Id )
BEGIN
	--IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID = @Id AND ISNULL(IsDefault,0) = 1)
	IF EXISTS (SELECT ID FROM tblTeamMemberTitle WHERE ID = @Id AND Title = 'Project Manager')
	BEGIN
		RAISERROR ('Cannot delete default Title.',16,1)
        RETURN
	END

	DELETE tblTeamMemberTitle WHERE ID = @Id
END
ELSE
BEGIN
	RAISERROR ('Cannot find this title in database.',16,1)
    RETURN
END


