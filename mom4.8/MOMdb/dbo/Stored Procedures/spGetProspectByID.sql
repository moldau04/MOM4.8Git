CREATE Procedure  [dbo].[spGetProspectByID]
@prospectid int
as

SELECT p.ID,        
       p.rol, 
       p.status, 
       p.type, 
       p.Address AS billaddress, 
       p.City AS billcity, 
       p.State AS billstate, 
       p.Zip AS billzip, 
       p.phone AS billphone, 
       p.CustomerName, 
       p.Terr, 
       r.Name, 
       r.Address , 
       r.City    , 
       r.State   , 
       r.Zip     , 
       r.Phone   , 
       r.Cellular, 
       r.email, 
       r.Website, 
       r.Fax, 
       r.Contact, 
       r.Remarks, 
       r.lat, r.lng ,
       p.CreatedBy	,
		p.CreateDate	,
		p.LastUpdatedBy	,
		p.LastUpdateDate	,
		p.Source,
		r.Country,
		p.Country as billCountry,
		p.Referral,
		p.ReferralType,
		p.BusinessType,
		(SELECT TOP 1 ISNULL(ID,0) FROM BusinessType WHERE Description=p.BusinessType) AS BusinessTypeID,
		r.EN,
		B.Name As Company
FROM   Prospect p 
       INNER JOIN Rol r ON r.ID = p.Rol 
	   LEFT Outer join Branch B on B.ID = r.EN  	   
			   WHERE p.ID=@prospectid
	  
         
declare @rol int
select top 1 @rol = p.Rol 
		FROM   Prospect p 
		INNER JOIN Rol r ON r.ID = p.Rol 
        where p.ID=@prospectid

exec spGetContactByRol @rol

--select ID as contactid,fDesc as name, Phone,Fax,Cell,Email from Phone 
--where 
--Rol =(select top 1 p.Rol 
--		FROM   Prospect p 
--		INNER JOIN Rol r ON r.ID = p.Rol 
--        where p.ID=@prospectid)