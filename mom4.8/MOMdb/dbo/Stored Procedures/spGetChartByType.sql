



CREATE PROCEDURE [dbo].[spGetChartByType]
@Type INT

 

as
	begin
			select C.ID,C.Acct,C.fDesc from Chart C where C.Type=@Type
		
	
	end
