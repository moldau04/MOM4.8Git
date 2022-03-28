Create PROCEDURE [dbo].[spAddPreferences]
	 
	@PageID int,
	@PreferenceID int,
	@UserID int,
	@Values varchar(100)
	
AS
BEGIN 
  if not exists(select 1 from tblJoinPrefrenceAndPages where PreferenceID=PreferenceID and UserID=@UserID and PageID=@PageID)
   insert into tblJoinPrefrenceAndPages(PageID,PreferenceID,UserID,[Values]) values(@PageID,@PreferenceID,@UserID,@Values)
   else
   update tblJoinPrefrenceAndPages set [Values]=@Values where PreferenceID=PreferenceID and UserID=@UserID and PageID=@PageID
END


	 
	