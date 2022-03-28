CREATE PROCEDURE [dbo].[spGetCompanyUserCoID]
@UserID int

AS

BEGIN

IF(@UserID > 0)
BEGIN
create table #TempCompany
(
    ID int, 
    Name Varchar(50),
    IsActive Bit 
)

Insert Into #TempCompany
select B.ID, B.Name, 1 from Branch B inner Join tblUserCo UC on B.ID=UC.CompanyID Where UC.UserID = @UserID

create table #TempBranch
(
    ID int, 
    Name Varchar(50),
    IsActive Bit 
)

Insert Into #TempBranch 
select ID, Name, 0 from Branch 

Select TB.ID, TB.Name, IsNULL(TC.IsActive,0) As IsSel from #TempBranch TB Left Outer Join #TempCompany TC on TB.ID = TC.ID order By TB.Name 

Drop Table #TempCompany

Drop Table #TempBranch

END
ELSE
BEGIN
select Distinct
B.ID, B.Name, IsNULL(UC.IsSel,0) As IsSel from Branch B Left Outer Join tblUserCo UC on B.ID=UC.CompanyID 
END


END

