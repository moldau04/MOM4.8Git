CREATE PROCEDURE [dbo].[sp_CheckWarehouseIsActive]
	-- Add the parameters for the stored procedure here
   @WarehouseID varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- Insert statements for procedure here
	
	DECLARE @isAcive int 
	SET @isAcive = (SELECT Isnull(status,0) as status from Warehouse where ID = @WarehouseID)
	--print @isAcive
	IF(@isAcive = 0)
	 BEGIN
	   select @isAcive
	 END
	 ELSE
	     select @isAcive
	   
	
	
	

END
GO