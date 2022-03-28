CREATE PROCEDURE [dbo].[spGetJobProjectFiltersValue]
	@DbName varchar(50)
As 
Begin
  
  Select distinct [Location]  from vw_GetJobProjectDetails where [Location] != '' order by [Location]

  Select distinct [Project#]  from vw_GetJobProjectDetails where [Project#] != '' order by [Project#]

  Select distinct [Description]  from vw_GetJobProjectDetails where [Description] != '' order by [Description]

  Select distinct [Status]  from vw_GetJobProjectDetails where [Status] != '' order by [Status]

  Select distinct [Type]  from vw_GetJobProjectDetails where [Type] != '' order by [Type]

  Select distinct [Date Created] from vw_GetJobProjectDetails where [Date Created] != ''  order by [Date Created]

  Select distinct [Hours]  from vw_GetJobProjectDetails where [Hours] >= 0 order by [Hours]

  Select distinct [Total On Order]  from vw_GetJobProjectDetails where [Total On Order] >= 0 order by [Total On Order]

  Select distinct [Total Billed] from vw_GetJobProjectDetails where [Total Billed] >= 0  order by [Total Billed]

  Select distinct [Labor Expense]  from vw_GetJobProjectDetails where [Labor Expense] >= 0 order by [Labor Expense]

  Select distinct [Material Expense]  from vw_GetJobProjectDetails where [Material Expense] >= 0 order by [Material Expense]

  Select distinct Expenses from vw_GetJobProjectDetails where Expenses >= 0  order by Expenses

  Select distinct [Total Expenses]  from vw_GetJobProjectDetails where [Total Expenses] >= 0 order by [Total Expenses]

  Select distinct Net from vw_GetJobProjectDetails where Net >= 0  order by Net

  Select distinct [% in Profit]  from vw_GetJobProjectDetails where [% in Profit] >= 0 order by [% in Profit]

  Select distinct Customer  from vw_GetJobProjectDetails where Customer != '' order by Customer

End