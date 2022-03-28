CREATE FUNCTION [dbo].[MergePermissionString](@str1 varchar(10), @str2 varchar(10))    
RETURNS Varchar(10)
AS
BEGIN
	declare @str varchar(10) = ''
	declare @diffStr varchar(10) = ''
	declare @len1 int = len(@str1)
	declare @len2 int = len(@str2)
	declare @len int = @len1
	declare @lendif int = 0--@len1 - @len2
	if(@len1 > @len2)
	begin
		set @lendif = @len1 - @len2
		set @len = @len2
		set @diffStr = SUBSTRING(@str1,@len2+1,@lendif)
	end
	else if(@len1 < @len2)
	begin
		set @lendif = @len2 - @len1
		set @len = @len1
		set @diffStr = SUBSTRING(@str2,@len1+1,@lendif)
	end
	--select @diffStr
	declare @i int = 1
	WHILE @i <= @len
	BEGIN
		if((SUBSTRING(@str1,@i,1) != SUBSTRING(@str2,@i,1)) AND (SUBSTRING(@str2,@i,1) = 'y'))
		BEGIN
			SET @str += 'y'
		END
		else
		BEGIN
			SET @str += SUBSTRING(@str1,@i,1)
		END
		SET @i += 1
	END

	set @str += @diffStr
	if(@str='')
		Set @str = null
	
	return @str
END
