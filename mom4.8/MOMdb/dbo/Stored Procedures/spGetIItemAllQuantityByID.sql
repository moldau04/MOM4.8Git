CREATE PROCEDURE[dbo].[spGetIItemAllQuantityByID]

    @Id int ,
    @UserID int,
    @EN int= null
AS
BEGIN
	
	Select 
	   Inv.Hand As Hand
	  ,Inv.fOrder As fOrder
	  ,Inv.Committed As Committed 
	  ,Inv.Available As Available
	  ,dbo.GetIssuesToOpenJobcount(@Id) As IssuesToOpenJobs
	  ,dbo.GetfOrderValue(@Id) As OOValue
	  ,dbo.GetOnHandValue(@Id,@EN,@UserID) As OHValue
	  ,dbo.GetCommittedValue(@Id) As CommittedValue
	  ,(CASE dbo.GetOnHandcount(@Id,@EN,@UserID) WHEN 0 THEN 0 ELSE dbo.GetOnBalancecount(@Id,@EN,@UserID)/dbo.GetOnHandcount(ID,@EN,@UserID) END) 
	  AS UnitCost from Inv where Inv.Type=0 and Inv.ID=@Id
END

