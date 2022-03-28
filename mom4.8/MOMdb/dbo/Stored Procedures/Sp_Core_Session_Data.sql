CREATE PROC Sp_Core_Session_Data
 @User_ID      int 
,@User_Token   nvarchar(500)
,@Session_Key  nvarchar(50)  
,@Session_Data nvarchar(MAX)
AS
BEGIN
IF  EXISTS(SELECT 1 FROM [Core_Session_Data] WHERE [User_Token]=@User_Token and @Session_Key=@Session_Key)
BEGIN
DELETE FROM [Core_Session_Data] WHERE [User_Token]=@User_Token and @Session_Key=@Session_Key
END
INSERT INTO [dbo].[Core_Session_Data] ([User_ID],[User_Token],[Session_Key] ,[Session_Data])
VALUES (@User_ID ,@User_Token ,@Session_Key,@Session_Data )		 
END