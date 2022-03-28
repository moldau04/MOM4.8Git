Create PROCEDURE [dbo].[spGetPreferences]
	 
	@PageID int,
	@PreferenceID int,
	@UserID int	
AS
BEGIN 
  select [Values] as Preferencevalue from tblJoinPrefrenceAndPages where PreferenceID=PreferenceID and UserID=@UserID and PageID=@PageID
END


	 
	