CREATE PROCEDURE [dbo].[spGetExistTestInLocByTestTypeAndChargable]
@Loc INT,
@TestType Int,
@YearProposal INT,
@Chargeable bit,
@Classification VARCHAR(100)

AS
Declare @TestTypeParent int
Declare @TestTypeChild int
SET @TestTypeChild = ISNULL((SELECT TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@TestType),0)
SET @TestTypeParent = ISNULL((SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType),0)

IF (@TestTypeChild=0)
BEGIN
	IF (@TestTypeParent=0)
	BEGIN
		PRINT	'Test Type does not have parent and child'
		select 
		t.LID
		,e.ID
		,e.Unit
		,ISNULL(e.Classification,'')  AS Classification
		,CASE WHEN ISNULL(h.OverrideAmount,0) <>0 THEN ISNULL(h.OverrideAmount,0) ELSE ISNULL(h.DefaultAmount,0) End AS Amount
		,ISNULL(h.OverrideAmount,0) AS OverrideAmount
		,ISNULL(h.ThirdPartyName,0) AS ThirdPartyName
		,ISNULL(h.ThirdPartyPhone,0) AS ThirdPartyPhone
		,ISNULL(h.ThirdPartyRequired,0) AS ThirdPartyRequired
		,ISNULL(h.Chargeable,0) AS Chargeable
		from LoadTestItem t
		INNER JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=t.LID AND itemHistory.TestYear=@YearProposal
		inner join Elev e on e.Id=t.Elev
		inner join Loc l on l.Loc=e.Loc
		left join LoadTestItemHistoryPrice h on h.LID=t.LID and h.PriceYear=@YearProposal
		where l.Status=0 and e.Status=0 and t.Loc=@Loc
		and e.Classification=@Classification
		--and Year(t.Next)=@YearProposal
		and t.ID=@TestType
		AND ISNULL(h.Chargeable,1)=@Chargeable
		AND t.LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=t.LID AND YearProposal=@YearProposal AND EquipmentId=t.Elev ) 
	END
	ELSE
	BEGIN
		PRINT	'Test Type is test parent '
		select 
		t.LID
		,e.ID
		,e.Unit
		,ISNULL(e.Classification,'')  AS Classification
		,CASE WHEN ISNULL(h.OverrideAmount,0) <>0 THEN ISNULL(h.OverrideAmount,0) ELSE ISNULL(h.DefaultAmount,0) End AS Amount
		,ISNULL(h.OverrideAmount,0) AS OverrideAmount
		,ISNULL(h.ThirdPartyName,0) AS ThirdPartyName
		,ISNULL(h.ThirdPartyPhone,0) AS ThirdPartyPhone
		,ISNULL(h.ThirdPartyRequired,0) AS ThirdPartyRequired
		,ISNULL(h.Chargeable,0) AS Chargeable
		from LoadTestItem t
		INNER JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=t.LID AND itemHistory.TestYear=@YearProposal
		inner join Elev e on e.Id=t.Elev
		inner join Loc l on l.Loc=e.Loc
		left join LoadTestItemHistoryPrice h on h.LID=t.LID and h.PriceYear=@YearProposal
		where l.Status=0 and e.Status=0 and t.Loc=@Loc
		and e.Classification=@Classification
		--and Year(t.Next)=@YearProposal
		and t.ID=@TestType
		AND ISNULL(h.Chargeable,1)=@Chargeable
		AND t.LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=t.LID AND YearProposal=@YearProposal AND EquipmentId=t.Elev ) 
		AND (select COUNT(1) FROM LoadTestItem item2
				INNER JOIN  LoadTestItemHistory itemHistory2 ON item2.LID=itemHistory2.LID AND itemHistory2.TestYear= @YearProposal
				WHERE item2.ID= @TestTypeParent AND Item2.Elev=t.Elev )=0
	END

END
ELSE
BEGIN
	PRINT	'Test Type is parent Test Type '
	select 
		t.LID
		,e.ID
		,e.Unit
		,ISNULL(e.Classification,'')  AS Classification
		,CASE WHEN ISNULL(h.OverrideAmount,0) <>0 THEN ISNULL(h.OverrideAmount,0) ELSE ISNULL(h.DefaultAmount,0) End AS Amount
		,ISNULL(h.OverrideAmount,0) AS OverrideAmount
		,ISNULL(h.ThirdPartyName,0) AS ThirdPartyName
		,ISNULL(h.ThirdPartyPhone,0) AS ThirdPartyPhone
		,ISNULL(h.ThirdPartyRequired,0) AS ThirdPartyRequired
		,ISNULL(h.Chargeable,0) AS Chargeable
		from LoadTestItem t
		INNER JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=t.LID AND itemHistory.TestYear=@YearProposal
		inner join Elev e on e.Id=t.Elev
		inner join Loc l on l.Loc=e.Loc
		left join LoadTestItemHistoryPrice h on h.LID=t.LID and h.PriceYear=@YearProposal
		where l.Status=0 and e.Status=0 and t.Loc=@Loc
		and e.Classification=@Classification
		--and Year(t.Next)=@YearProposal
		and t.ID=@TestType
		AND ISNULL(h.Chargeable,1)=@Chargeable
		AND t.LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=t.LID AND YearProposal=@YearProposal AND EquipmentId=t.Elev ) 
		AND (select COUNT(1) FROM LoadTestItem item2
				INNER JOIN  LoadTestItemHistory itemHistory2 ON item2.LID=itemHistory2.LID AND itemHistory2.TestYear= @YearProposal
				WHERE item2.ID= @TestTypeParent AND Item2.Elev=t.Elev )=0
END




select
l.Consult,
l.ID ,
Tag,
LTRIM(RTRIM(l.Address))  as locAddress,
l.City as locCity,
l.State as locState,
l.Zip as locZip,
l.Rol,
l.Type ,
isnull(l.Route,0) Route,
Terr,
Terr2,
r.City,
r.State,
r.Zip,
r.Country,
LTRIM(RTRIM(r.Address)) as Address,
l.Remarks,
r.Contact,
r.Contact as Name,
r.Phone,
r.Website,
r.EMail,
r.Cellular,
r.Fax,
(Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = @Loc) AS EN,
(Select Name from  Branch  where ID = (Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = @Loc)) AS Company,
l.owner,
(select top 1 name from rol where id=(select top 1 rol from owner where id= l.owner)) as custname,
l.stax,
l.STax2,
l.UTax,
l.Zone,
l.Country As locCountry,
r.Lat,r.Lng,l.custom1,l.custom2,l.custom14,l.custom15,l.custom12,l.custom13,l.status,

 l.PrintInvoice,
 l.EmailInvoice,
 l.Balance,
 l.Loc,
 isnull(l.NoCustomerStatement,0) as NoCustomerStatement,
 l.Address + ', '+ l.City + ', ' + l.State + ' ' + l.Zip   As LocationName,
 tr.Name AS Salesperson,
 rt.Name AS RouteName,
 o.Custom1 AS OwnerName,
 rl.name as Customer,
 (select count(1) from  elev e with (nolock) where e.loc=l.loc) as Elevs,
 l.BusinessType as BusinessTypeID,
 stax.Type as sTaxType

from Loc l
left outer JOIN Owner o ON o.id = l.owner
left outer join Rol rl ON o.rol = rl.id
left outer join Rol r on l.Rol=r.ID and r.Type=4
left outer join stax on stax.name = l.stax
left outer join Branch B on B.ID = r.EN
left outer join Terr tr with (nolock)  ON l.Terr = tr.ID 
left outer join Route rt with (nolock) ON l.Route = rt.ID 
where l.loc=@loc



