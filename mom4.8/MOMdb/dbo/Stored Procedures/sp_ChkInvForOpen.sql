CREATE PROCEDURE [dbo].[sp_ChkInvForOpen]
	@invID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE @IsOpen BIT = 0;
	
	-- for PO
	IF((SELECT Count(PO.Status) FROM PO PO INNER JOIN POItem POI ON po.PO = POI.PO WHERE PO.Status = 0 AND POI.Inv = @invID) > 0)
	  BEGIN
	  SET @IsOpen = 1
	  END
	ELSE
	  SET @IsOpen = 0
	  

	-- for RPO
	IF(@IsOpen = 0)
		BEGIN
		IF((SELECT Count(RPO.Status) FROM ReceivePO RPO INNER JOIN POItem POI ON RPO.PO = POI.PO WHERE  RPO.Status = 0 AND POI.Inv = @invID) > 0)
		BEGIN
		  SET @IsOpen = 1	
		END
	END

	-- AP billls 
	IF(@IsOpen = 0)
		BEGIN
		IF((SELECT Count(pj.Status) FROM PJ PJ INNER JOIN Trans t ON pj.Batch = t.Batch AND t.type = 41 AND t.AcctSub = @invID) > 0)
		BEGIN
		  SET @IsOpen = 1		  
		END
	END
	
	IF @IsOpen <> 0
	BEGIN
		SELECT @IsOpen
	END
	
	
END
