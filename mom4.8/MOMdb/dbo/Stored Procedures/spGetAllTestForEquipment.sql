CREATE PROCEDURE [dbo].[spGetAllTestForEquipment]			
	@EquipmentID int
	,@YearProposal int		
	
AS 
	BEGIN 
	Select  
	  LoadTestItem.[LID] AS [LID]
      ,LoadTestItem.[ID]
      ,[Loc]
      ,[Elev]
      ,[Last]
      ,[Next]
      ,LoadTestItem.[Status]
      ,[Ticket]
      ,ISNULL([Remarks],'') AS Remarks
      ,[LastDue]
      ,[idRolCustomContact]
      ,ISNULL([Custom1],'') AS Custom1
      ,ISNULL([Custom2],'') AS Custom2
      ,ISNULL([Custom3],'') AS Custom3
      ,ISNULL([Custom4],'') AS Custom4
      ,ISNULL([Custom5],'') AS Custom5
      ,ISNULL([Custom6],'') AS Custom6
      ,ISNULL([Custom7],'') AS Custom7
      ,ISNULL([Custom8],'') AS Custom8
      ,ISNULL([Custom9],'') AS Custom9
      ,ISNULL([Custom10],'') AS Custom10
      ,[JobId]
          ,ISNULL(LoadTestItemHistoryPrice.[DefaultAmount],0) AS [Amount]
	    ,ISNULL(LoadTestItemHistoryPrice.[OverrideAmount],0) AS [OverrideAmount]
	  ,ISNULL(LoadTestItemHistoryPrice.[ThirdPartyName],'') AS [ThirdPartyName]
      ,ISNULL(LoadTestItemHistoryPrice.[ThirdPartyPhone],'') AS [ThirdPartyPhone]
      ,[TestDueBy]
      ,ISNULL(LoadTestItemHistoryPrice.[Chargeable],0) AS [Chargeable]
      ,ISNULL(LoadTestItemHistoryPrice.[ThirdPartyRequired],0) AS [ThirdParty]
		from LoadTestItem 	
		LEFT JOIN LoadTestItemHistoryPrice ON LoadTestItemHistoryPrice.LID=LoadTestItem.LID 
		WHERE  		
		LoadTestItem.LID NOT IN (select TestID from ProposalFormDetail where EquipmentID=[Elev] AND ISNULL(YearProposal,YEAR(GETDATE())) =@YearProposal )
		and [Elev]=@EquipmentID	
		AND LoadTestItemHistoryPrice.PriceYear=@YearProposal
	END 