
CREATE PROCEDURE [dbo].[spUpdateClearItems]
(	@Batch INT ,
	@Sel SMALLINT,
	@Type INT

)
AS
BEGIN

	SET NOCOUNT ON;


	UPDATE [dbo].[Trans] SET [Sel] = @Sel  
	WHERE [Type] = @Type AND [Batch] = @Batch 
	

END
