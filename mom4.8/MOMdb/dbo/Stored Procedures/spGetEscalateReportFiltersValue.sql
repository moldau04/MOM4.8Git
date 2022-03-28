CREATE PROCEDURE [dbo].[spGetEscalateReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct [Location Id] from vw_EscalationContracts where [Location Id] != '' order by [Location Id]

  select distinct [Location Name] from vw_EscalationContracts where [Location Name]  != '' order by[Location Name] 
  
  select distinct [Service Type] from vw_EscalationContracts where [Service Type] != '' order by [Service Type]

  select distinct [Description] from vw_EscalationContracts where [Description] != '' order by [Description]

  select distinct [Billing Freqency] from vw_EscalationContracts where [Billing Freqency] != '' order by [Billing Freqency]

  select distinct [Esc Type] from vw_EscalationContracts where [Esc Type] != '' order by  [Esc Type]

  select distinct [Action] from vw_EscalationContracts where [Action] != '' order by [Action]

  select distinct [Esc Cycle] from vw_EscalationContracts where [Esc Cycle] != '' order by [Esc Cycle]
  
  select distinct [Esc Factor] from vw_EscalationContracts where [Esc Factor] >= 0 order by [Esc Factor]

  select distinct [Last Esc] from vw_EscalationContracts where [Last Esc] != '' order by [Last Esc]

  select distinct [Start Esc] from vw_EscalationContracts where [Start Esc] != '' order by [Start Esc]

  select distinct [Finish Esc] from vw_EscalationContracts where [Finish Esc] != '' order by [Finish Esc]

  select distinct [Next Due] from vw_EscalationContracts where [Next Due] != '' order by [Next Due]

   select distinct Amount from vw_EscalationContracts where Amount >= 0 order by Amount

  select distinct [New Amount] from vw_EscalationContracts where [New Amount] >= 0 order by [New Amount]

  select distinct [Length] from vw_EscalationContracts where [Length] != '' order by [Length]

  select distinct [Contract]  from vw_EscalationContracts where [Contract] != '' order by [Contract]

  select distinct [Expiration Date] from vw_EscalationContracts where [Expiration Date] != '' order by [Expiration Date]

  select distinct [Renewal Notes] from vw_EscalationContracts where [Renewal Notes] != '' order by [Renewal Notes]
 
END

