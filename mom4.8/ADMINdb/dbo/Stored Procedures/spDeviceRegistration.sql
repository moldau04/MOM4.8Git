CREATE Proc [dbo].[spDeviceRegistration]

@deviceId as varchar(200),
@regId as varchar(max),
@DeviceType  as varchar(max)

as
begin 
      
     Declare @DeviceIdCount as int
     
     Select @DeviceIdCount = count(1) from PushNotifications where deviceid = @deviceId
     
     if(@DeviceIdCount = 0)
     begin
          Insert into PushNotifications (deviceid,tokenid,DeviceType) values (@deviceId,@regId,@DeviceType)
     end  
     else
     begin
          Update PushNotifications set tokenid = @regId, DeviceType = @DeviceType
                               where deviceid = @deviceId
     end

End
GO

