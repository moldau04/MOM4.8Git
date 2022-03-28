CREATE function [dbo].[SortNumber](@indexnum varchar(50))
	returns varchar(50)
as
begin
	declare @curlength int, @length int
	declare @rtnstring varchar(50)
	select @rtnstring = '', @curlength = 0
	while (@curlength < len(@indexnum))
	begin
		set @length = charindex('.', @indexnum, @curlength + 1)
		if @length = 0
			set @length = len(@indexnum) + 1
		set @rtnstring = @rtnstring + right('0000' + substring(@indexnum, @curlength + 1, @length - @curlength - 1), 4) + '.'
		set @curlength = @length
	end
	return @rtnstring
end