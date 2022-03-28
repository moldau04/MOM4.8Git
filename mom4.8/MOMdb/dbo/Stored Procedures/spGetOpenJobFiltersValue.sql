CREATE PROCEDURE [dbo].[spGetOpenJobFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN
  
  select distinct Customer from vw_OpenJobReport where Customer != '' order by Customer

  select distinct Location from vw_OpenJobReport where Location != '' order by Location
  
  select distinct City from vw_OpenJobReport where City != '' order by City

  select distinct State from vw_OpenJobReport where State != '' order by State

  select distinct SalesPerson from vw_OpenJobReport where Status != '' order by SalesPerson

END
