CREATE PROCEDURE [dbo].[spGetUserExchangeContacts]
	@UserID int
AS
	SELECT MemberName,MemberEmail,GroupName,[Type] from tblUserExchangeContacts WHERE UserID = @UserID
