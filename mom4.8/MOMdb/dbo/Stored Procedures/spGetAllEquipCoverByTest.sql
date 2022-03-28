CREATE PROCEDURE [dbo].[spGetAllEquipCoverByTest]
@Loc INT,
@TestType Int,
@YearProposal INT,
@Chargeable bit,
@Classification VARCHAR(100)
AS

Declare @TestTypeParentID int

set @TestTypeParentID=(select  TestTypeID from TestTypeCover where TestTypeCoverID=@TestType)
select 
t.LID
,e.ID
,e.Unit
,isnull(e.Classification,'') as Classification
,ISNULL(h.DefaultAmount,0) AS Amount
,ISNULL(h.OverrideAmount,0) AS OverrideAmount
,ISNULL(h.ThirdPartyName,0) AS ThirdPartyName
,ISNULL(h.ThirdPartyPhone,0) AS ThirdPartyPhone
,ISNULL(h.ThirdPartyRequired,0) AS ThirdPartyRequired
,ISNULL(h.Chargeable,0) AS Chargeable
from LoadTestItem t
inner join Elev e on e.Id=t.Elev
inner join Loc l on l.Loc=e.Loc
left join LoadTestItemHistoryPrice h on h.LID=t.LID and h.PriceYear=@YearProposal
where l.Status=0 and e.Status=0 and t.Loc=@Loc
and e.Classification=@Classification
and Year(t.Next)=@YearProposal
and t.ID=@TestType
AND t.Chargeable=@Chargeable
and (select count(*) from LoadTestItem  ts where ts.Loc=@Loc and ts.ID= @TestTypeParentID and ts.Elev=t.Elev and Year(ts.Next)=@YearProposal)<>0
AND t.LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=t.LID AND YearProposal=@YearProposal AND EquipmentId=t.Elev ) 	



