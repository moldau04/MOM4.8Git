Create PROCEDURE [dbo].[getGCandHowerLocID]
@LocID int
 
 AS
------Get GC INFO---

	select  r.Name as RolName,
        r.City as city,
        r.State as state,
        r.Zip as zip,
        r.Phone as phone,
        r.fax as fax,
        r.Contact as   contact,       
        r.EMail as  email,           
        r.Country as  country,
        r.Cellular as  cellular,
        r.Remarks as  rolRemarks,
		t1.LocContactTypeID as LocContactType,
		r.ID as RolID,
		r.Address
		from rol r
		inner join tblLocAddlContact t1 on  t1.RolID=r.ID
		inner join Loc l on l.GContractorID=t1.RolID 
		where l.loc =@LocID

--Get Home Owner info
  select  r.Name as RolName,
        r.City as city,
        r.State as state,
        r.Zip as zip,
        r.Phone as phone,
        r.fax as fax,
        r.Contact as   contact,       
        r.EMail as  email,           
        r.Country as  country,
        r.Cellular as  cellular,
        r.Remarks as  rolRemarks,
		t1.LocContactTypeID as LocContactType,
		r.ID as RolID,
		r.Address
		from rol r
		inner join tblLocAddlContact t1 on  t1.RolID=r.ID
		inner join loc l on  l.HomeOwnerID=t1.RolID
		where l.loc =@LocID
