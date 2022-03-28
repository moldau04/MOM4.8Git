
CREATE PROCEDURE [dbo].[spGetCommodity]

@Id int= null 

as
	begin

	if @id is null
		begin
			select * from commodity with (nolock)
		end
	
	else
		begin
		select * from commodity with (nolock) where commodity.Id=@id
		end

	end

go

