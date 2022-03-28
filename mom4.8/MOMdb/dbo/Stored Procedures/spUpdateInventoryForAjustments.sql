CREATE PROCEDURE [dbo].[spUpdateInventoryForAjustments]			
         
			@ID [int],			
			@OnHand [numeric](30, 2),
			@Balance [numeric](30, 2)= NULL
			
as
	begin
		

		--Declare @fOrder [numeric](30, 2)
		--Declare @Min [numeric](30, 2)
		Declare @total [numeric](30, 2)
		Declare @Req [numeric](30, 2)


		select @total= (ISNULL(inv.fOrder,0.00)+ISNULL(INV.Min,0.00)) from Inv where Inv.ID=@ID

		if (@total>@OnHand)
			begin
				set @Req=@total-@OnHand
			end
        else
			set @Req=0.00


		 UPDATE [dbo].[Inv]
	   SET 
		  [Hand] = @OnHand
		  ,[Balance] = @Balance
		  ,[Requ]=@Req
		
		WHERE ID=@ID
				
		

	end
GO