CREATE PROC Sp_GetCore_Session_Data
@User_ID      int 
,@User_Token   nvarchar(500)
,@Session_Key  nvarchar(50)  
,@Session_Data nvarchar(MAX)
AS
BEGIN 
SELECT * FROM [Core_Session_Data] WHERE [User_Token]=@User_Token AND @Session_Key=@Session_Key 
END
