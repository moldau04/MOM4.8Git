CREATE PROCEDURE [dbo].[sp_ChkStatusOfChart]
	-- Add the parameters for the stored procedure here
   @ID int
AS
BEGIN
	
	SELECT ID,fDesc, Isnull(status,0) as status from Chart  where ID = @ID	

END
GO