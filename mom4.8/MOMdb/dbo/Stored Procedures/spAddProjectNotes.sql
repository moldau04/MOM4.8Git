CREATE Procedure [dbo].[spAddProjectNotes]
@JobID int,
@Note VARCHAR (Max) NULL,
@CreatedBy int
AS
BEGIN
	INSERT INTO JobNotes(JobID,Note,CreatedDate,CreatedBy)
	Values (@JobID,@Note,GETDATE(),@CreatedBy)

	/* Start Logs */
	DECLARE @UpdatedBy Varchar(100)
	SELECT @UpdatedBy = fUser FROM tblUser WHERE ID = @CreatedBy
	IF (@Note is not null AND @Note != '')
	BEGIN 
		EXEC log2_insert @UpdatedBy,'Job',@JobID,'Project Notes' ,'' , @Note 
	END
	/* End Logs */
END