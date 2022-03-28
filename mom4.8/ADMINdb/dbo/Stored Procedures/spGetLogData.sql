-- exec spGetLocation 'David G','2020-04-15','webstageQAE'
CREATE PROCEDURE spGetLogData
  @fuser varchar(25),
  @Date DATE,
  @database varchar(50)
AS
BEGIN
  DECLARE @SQL NVARCHAR(MAX)= N'Select deviceId,latitude,longitude,date,ID,Accuracy,fuser,userId,battery,speed From '+ @database+'.dbo.[MapDataNew] 
  WHERE fuser = '''+ @fuser +''' and  date >= @Date order by id asc ' ;
    
	--print @SQL
  EXECUTE sp_executesql @SQL,
                        N'@Date DATE,@fuser varchar(25),@database varchar(50)',
                        @Date,@fuser,@database;
                        
  
END



