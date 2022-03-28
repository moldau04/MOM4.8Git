 CREATE PROCEDURE [dbo].[SppingdeviceNew]  
                                 @deviceId AS VARCHAR(200),
                                 @randomId AS VARCHAR(100),
								 @date AS datetime,
                                 @isrunning smallint,
                                 @GPS smallint,
								 @backgroundRefresh int,
								 @FUser  VARCHAR(50),
								 @UserId AS int,								 
								 @database VARCHAR(50)
AS
BEGIN

  DECLARE @fuserCount int   
  DECLARE @SQLCount AS NVARCHAR(MAX)
  DECLARE @outPut NVARCHAR(50);
  SET @SQLCount = N'Select @CountOUT=COUNT(1) From '+ @database+'.[dbo].tblPingDevice where FUser = @FUser ' 
  SET @outPut = N'@CountOUT int OUTPUT,@FUser varchar(50)';
  EXEC sp_executesql @SQLCount, @outPut, @CountOUT=@fuserCount OUTPUT,@FUser = @FUser;
  DECLARE @sql nvarchar(max);
   IF(@fuserCount = 0)
    BEGIN	  
	    SET @sql = N'INSERT INTO '+ @database+'.[dbo].tblPingDevice(deviceID,randomID,date,IsRunning,IsGPSEnabled,BackgroundRefresh,FUser,UserId) 
		                                                         VALUES (@deviceID,@randomID,@date,@IsRunning,@IsGPSEnabled,@BackgroundRefresh,@FUser,@UserId) ';
        EXECUTE sp_executesql @sql, 
        N'@deviceID nvarchar(max), @randomID varchar(100),@date datetime, @IsRunning int, @IsGPSEnabled int, @BackgroundRefresh int, @FUser varchar(50), @UserId int',
        @deviceID = @deviceID,
        @randomID = @randomID,
		@date = @date,
        @IsRunning = @IsRunning,
		@IsGPSEnabled = @GPS ,
		@BackgroundRefresh = @BackgroundRefresh,
		@FUser = @FUser, 
		@UserId = @UserId ;
    END
   ELSE
    BEGIN
	    SET @sql = N'Update ' + @database + '.[dbo].tblPingDevice set deviceID = @deviceID , randomID =  @randomID , date =  @date , IsRunning = @IsRunning , IsGPSEnabled = @IsGPSEnabled ,BackgroundRefresh =@BackgroundRefresh, UserId = @UserId where fuser =   @FUser ' 
	    EXECUTE sp_executesql @sql,  N'@deviceID nvarchar(max), @randomID varchar(100),@date datetime, @IsRunning int, @IsGPSEnabled int, @BackgroundRefresh int, @FUser varchar(50), @UserId int',
	    @deviceID = @deviceID,
        @randomID = @randomID,
		@date = @date,
        @IsRunning = @IsRunning,
		@IsGPSEnabled = @GPS ,
		@BackgroundRefresh = @BackgroundRefresh,
		@FUser = @FUser, 
		@UserId = @UserId ;
 END
 




        
	   
END
    --INSERT INTO tblpingdevice
    --            (deviceid,
    --             randomId,
    --             date,
    --             isrunning,
    --             isGPSenabled,
				-- backgroundRefresh,
				-- FUser,
				-- UserId
				-- )
    --VALUES      (@deviceId,
    --             @randomId,
    --             GETDATE(),
    --             @isrunning,@GPS,
				-- @backgroundRefresh,
				-- @FUser,
				-- @UserId
				-- )



  

				
