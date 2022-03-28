CREATE PROC [dbo].[Sppingdevice] @deviceId AS VARCHAR(200),
                                 @randomId AS VARCHAR(100),
                                 @isrunning smallint,
                                 @GPS smallint,
								 @backgroundRefresh int
AS
    INSERT INTO tblpingdevice
                (deviceid,
                 randomId,
                 date,
                 isrunning,
                 isGPSenabled,
				 backgroundRefresh)
    VALUES      (@deviceId,
                 @randomId,
                 GETDATE(),
                 @isrunning,@GPS,
				 @backgroundRefresh
                 )
