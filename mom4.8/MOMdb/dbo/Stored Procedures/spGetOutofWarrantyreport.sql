CREATE PROCEDURE [dbo].[spGetOutofWarrantyreport] 
--- Last Updated BY :- NK
--- Date :28 March 2019 
--- Desc :- Modify Report request on Cilent
@StartDate Datetime,
@EndDate Datetime 

AS
 BEGIN
 SET NOCOUNT ON;

 Declare @tbl table
(
       
   HO_Name varchar(200),
   LocationName varchar(200),
   HO_BilltoAddress varchar(200) , 
   HO_EMail varchar(200),
   HO_State varchar(200),
   HO_City varchar(200),
   HO_Zip varchar(200),
   PI_Date date,
   HO_Phone varchar(200)
) 

  INSERT into @tbl 

		SELECT 
		case HOwnerRol.Name when null then OwnerRol.Name else HOwnerRol.Name end HO_Name,
		l.Tag LocationName ,
		case HOwnerRol.Name when null then OwnerRol.Address else HOwnerRol.Address end  HO_BilltoAddress, 
		case HOwnerRol.Name when null then OwnerRol.EMail else HOwnerRol.EMail end HO_EMail,
		case HOwnerRol.Name when null then OwnerRol.State else HOwnerRol.State end HO_State, 
		case HOwnerRol.Name when null then OwnerRol.City else HOwnerRol.City end HO_City,
		case HOwnerRol.Name when null then OwnerRol.Zip else HOwnerRol.Zip end HO_Zip,
		cast (Cj.Value as date) PI_Date ,
		case HOwnerRol.Name when null then OwnerRol.Phone else HOwnerRol.Phone end HO_Phone  
	    FROM job j
		INNER JOIN loc l on l.loc= j.Loc 
		INNER JOIN owner o  on o.ID= l.Owner
		INNER JOIN Rol OwnerRol   ON o.Rol = OwnerRol.ID 
		left JOIN tblLocAddlContact LOCADDCON  on LOCADDCON.RolID=l.HomeOwnerID
		left JOIN Rol HOwnerRol   ON LOCADDCON.RolID = HOwnerRol.ID  and LOCADDCON.LocContactTypeID=2 		 	 
		INNER JOIN tblCustomJob Cj on Cj.JobID=j.id 
		INNER JOIN [tblCustomFields] CF on CF.ID=cj.tblCustomFieldsID  and  CF.tblTabID=4
		AND CF.Label='Passed Inspection'
		WHERE isnull(Cj.Value,'') !='' AND ISDATE(Cj.Value)=1


   SELECT  
   HO_Name ,
   LocationName ,
   HO_BilltoAddress  , 
   HO_EMail ,
   (select top 1 fdesc from state where name=HO_State) HO_State ,
   HO_City  ,
   HO_Zip   ,
   cast(PI_Date as varchar(10)) PI_Date ,
   HO_Phone   
   FROM @tbl t
   WHERE  (t.PI_Date ) > = ( @StartDate )  and (t.PI_Date ) < = ( @EndDate )


END