CREATE PROCEDURE [dbo].[spUpdateInventoryCategory]

@CategoryTypeID int,
@CategoryName varchar(15),
@CategoryCount int,
@CategoryRemarks Varchar(8000)
as

begin
update IType 
set type=@CategoryName,Count=@CategoryCount,Remarks=@CategoryRemarks where ID=@CategoryTypeID

end

