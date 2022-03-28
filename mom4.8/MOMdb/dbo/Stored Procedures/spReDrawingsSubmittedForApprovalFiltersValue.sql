CREATE PROCEDURE [dbo].[spReDrawingsSubmittedForApprovalFiltersValue]
@DbName varchar(50)
	
AS
BEGIN
  
  select distinct Customer from vw_ReDrawingsSubmittedForApproval where Customer != '' order by Customer

  select distinct Location from vw_ReDrawingsSubmittedForApproval where Location != '' order by Location
  
  select distinct City from vw_ReDrawingsSubmittedForApproval where City != '' order by City

  select distinct State from vw_ReDrawingsSubmittedForApproval where State != '' order by State

  select distinct Zip from vw_ReDrawingsSubmittedForApproval where Zip != '' order by Zip

  select distinct Status from vw_ReDrawingsSubmittedForApproval where Status != '' order by Status

END
