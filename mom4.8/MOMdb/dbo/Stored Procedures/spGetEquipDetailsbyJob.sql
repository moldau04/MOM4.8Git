create PROCEDURE [dbo].[spGetEquipDetailsbyJob]
@job varchar(max) ,
@result varchar(max) output
AS

set @result = ''
select @result = @result +'Unit'+ ' '+e.Unit +','
from elev e inner join  tbljoinElevJob ej on e.ID=ej.Elev 
inner join Job j on ej.Job=j.ID inner join   
contract c on j.ID=c.Job  where j.Loc=@job and  c.status=0

IF RIGHT(RTRIM(@result),1) = ','
BEGIN
SET @result = LEFT(@result, LEN(@result) - 1)
END
