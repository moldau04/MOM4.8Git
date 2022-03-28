CREATE PROCEDURE [dbo].[spUpdateChartBalanceForInvAdjRemoval]			
         
			@ID [int],
			@Balance [numeric](30, 2)= NULL
			
as
	begin
		
		if (@ID=(select top 1 id from Chart where Chart.fDesc='inventory'))
			UPDATE Chart SET Chart.Balance = Chart.Balance - @Balance WHERE Chart.ID=@ID
		else
			UPDATE Chart SET Chart.Balance = Chart.Balance + @Balance WHERE Chart.ID=@ID

	end
GO