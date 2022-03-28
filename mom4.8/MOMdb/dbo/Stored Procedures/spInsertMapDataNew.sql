CREATE PROCEDURE [dbo].[spInsertMapDataNew] 
@DtMapDataNew As [dbo].[tblTypeMapDatanew] Readonly,
@database varchar(100)
AS
BEGIN
declare @strSQL varchar(max)
DECLARE @SQL NVARCHAR(MAX);

SELECT deviceId,
latitude,
longitude,
date,
fake,
Accuracy,
fUser,
userId,
battery,
speed
INTO #T1 FROM @DtMapDataNew


SET @sql = N'insert into ' + @database+' .dbo.[MapDataNew] (deviceId,
latitude,
longitude,
date,
fake,
Accuracy,
fUser,
userId,
battery,
speed)
Select Distinct
deviceId, latitude, longitude,date,fake,accuracy,fUser,userId,battery,speed
From #T1';
--print @SQL
EXEC sp_executesql @sql;
DROP TABLE #T1
END