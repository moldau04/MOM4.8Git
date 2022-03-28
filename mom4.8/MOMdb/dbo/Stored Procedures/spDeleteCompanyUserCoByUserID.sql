
CREATE PROCEDURE [dbo].[spDeleteCompanyUserCoByUserID]
@UserID int,
@CompanyID int

AS


BEGIN

Delete from tblUserCo Where UserID = @UserID AND CompanyID = @CompanyID

END


