CREATE PROCEDURE [dbo].[spAddUserCompany]
      @tblUserCompany tblTypeUserCompany READONLY
AS
BEGIN
      SET NOCOUNT ON;
     
      INSERT INTO tblUserCo(UserID, CompanyID,OfficeID,IsSel)
      SELECT UserID, CompanyID,OfficeID,IsSel FROM @tblUserCompany
END