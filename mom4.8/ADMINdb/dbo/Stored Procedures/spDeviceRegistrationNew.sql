CREATE PROCEDURE [dbo].[spDeviceRegistrationNew]

@deviceId as text,
@regId as nvarchar(max),
@DeviceType  as Nvarchar(max) = null,
@FUser varchar(50),
@UserID int,
@database varchar(50) 
 
AS
BEGIN
  DECLARE @fuserCount int   
  DECLARE @SQLCount AS NVARCHAR(MAX)
  DECLARE @outPut NVARCHAR(50);
  SET @SQLCount = N'Select @CountOUT=COUNT(1) From '+ @database+'.[dbo].PushNotifications where FUser = @FUser ' 
  SET @outPut = N'@CountOUT int OUTPUT,@FUser varchar(50)';
  EXEC sp_executesql @SQLCount, @outPut, @CountOUT=@fuserCount OUTPUT,@FUser = @FUser;
  DECLARE @sql nvarchar(max);
   IF(@fuserCount = 0)
    BEGIN	  
	    SET @sql = N'INSERT INTO '+ @database+'.[dbo].PushNotifications(deviceid,tokenid,DeviceType,FUser,UserID) 
		            VALUES (@deviceid,@regId,@DeviceType,@FUser,@UserID)';
        EXECUTE sp_executesql @sql, 
        N'@deviceid nvarchar(max), @regId Nvarchar(MAX), @DeviceType Nvarchar(MAX), @FUser varchar(50), @UserID int',
        @deviceId = @deviceId,
        @regId = @regId,
		@DeviceType = @DeviceType,
        @FUser = @FUser,
		@UserID = @UserID ;
    END
   ELSE
    BEGIN
	    SET @sql = N'Update ' + @database + '.[dbo].PushNotifications set tokenid = @regId , DeviceType =  @DeviceType , deviceid =  @deviceid , UserID = @UserID  where fuser =   @FUser ' 
	    EXECUTE sp_executesql @sql,  N'@deviceid nvarchar(max), @regId Nvarchar(MAX), @DeviceType varchar(50), @FUser varchar(50), @UserID int',
	    @deviceId = @deviceId,
        @regId = @regId,
		@DeviceType = @DeviceType,
        @FUser = @FUser,
		@UserID = @UserID ;	   
 END
END

    

