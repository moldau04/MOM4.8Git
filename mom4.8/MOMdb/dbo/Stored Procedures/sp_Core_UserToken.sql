-- =============================================
-- Author:		<PoojA>
-- Create date: <14-March-2020>
-- Description:	<User Authentication>
-- =============================================
CREATE PROCEDURE sp_Core_UserToken
	-- Add the parameters for the stored procedure here
    @Token nvarchar(500),
	@Domain_Name varchar (500),
	@User_Id int,
	@company varchar (500)
AS
BEGIN  

    -- Insert statements for procedure here
	if Exists (	select 1  from Core_UserToken where Token=@Token and Expiry_Date > =GETDATE() and Domain_Name=@Domain_Name 	and User_Id =@User_Id and company=@company )
	select 2020	else  select  404 

END