-- =============================================
-- Author		:	NK
-- Create date	:	26 DEC, 2019
-- Description	:	To insert Customer
-- =============================================


---    Add New Customer

BEGIN TRAN

----   COMMIT TRAN
---    ROLLBACK TRAN

Declare @ROW_Count int ;
Declare @ROW_NO int =1;
select  @ROW_Count =max(PK) from dbo.WestCoastCustomers$ where PK is not null 


Declare @UserName	nvarchar(50)='';	
Declare @Password	nvarchar(50)='';
Declare @status smallint=0;
Declare @FName varchar(500);
Declare @Address varchar(8000);
Declare @City varchar(50);
Declare @State varchar(2);
Declare @Zip varchar(10);
Declare @country varchar(50);
Declare @Remarks varchar(8000)='';
Declare @Mapping int=0;
Declare @Schedule int=0;
Declare @Internet int=0;
Declare @contact varchar(50);
Declare @phone varchar(28)='';
Declare @Website varchar(50)='';
Declare @email varchar(50)='';
Declare @Cell varchar(28)='';
Declare @Type varchar(50);
Declare @Equipment smallint=0;
Declare @SageID varchar(50);
Declare @Billing smallint=0;
Declare @grpbywo smallint=0;
Declare @openticket smallint=0;
Declare @ContactData As [dbo].[tblTypeCustContact];
Declare @BillRate numeric(30,2)=0;
Declare @OT numeric(30,2)=0;
Declare @NT numeric(30,2)=0;
Declare @DT numeric(30,2)=0;
Declare @Travel numeric(30,2)=0;
Declare @Mileage numeric(30,2)=0;
Declare @Fax varchar(28) = '';
Declare @EN int=0;
Declare @Lat varchar(50)='';
Declare @Lng varchar(50)='';



while(@ROW_NO <=@ROW_Count)

Begin

print(@ROW_NO)
 
 
-------------------------
select @FName=cust.[Customer Name],
       @Address= cust.[Customer Address],	   
       @City=cust.City,
       @state=cust.State, 
       @Zip=cust.ZIP,
       @contact=cust.[Main Contact],
	   @phone=cust.[Phone #],
	   @Cell=cust.Cellular,
	   --@Fax=Null,
	   @email=cust.Email,
	   @Type=cust.Type,
	   @country=cust.Country,
	   @Website=cust.Website	   
	   from  dbo.WestCoastCustomers$ cust where cust.PK=@ROW_NO 



	  
	   INSERT INTO @ContactData (Name,Phone,Email,Cell)

       SELECT c.[2nd Contact],c.[Phone #1],c.Email1 ,c.Cellular1   FROM  dbo.WestCoastCustomers$ c where c.PK=@ROW_NO 
	   and ( isnull(c.[2nd Contact],'') <> ''  or isnull(c.[Phone #1],'') <> '' or isnull(c.Email1,'')  <> '' or isnull(c.Cellular1,'') <> '' )
        

	   UNION ALL

       SELECT c.[3rd Contact] ,c.[Phone #2],c.Email2 ,c.Cellular2   FROM  dbo.WestCoastCustomers$ c where c.PK=@ROW_NO 
	   and ( isnull(c.[3rd Contact],'') <> ''  or isnull(c.[Phone #2],'') <> '' or isnull(c.Email2,'')  <> '' or isnull(c.Cellular2,'') <> '' )

	    UNION ALL

       SELECT c.[4th Contact],c.[Phone #3],c.Email3	 ,c.Cellular3   FROM  dbo.WestCoastCustomers$ c where c.PK=@ROW_NO 
	   and ( isnull(c.[4th Contact],'') <> ''  or isnull(c.[Phone #3],'') <> '' or isnull(c.Email3,'')  <> '' or isnull(c.Cellular3,'') <> '' )

	   UNION ALL

       SELECT c.[5th Contact],c.[Phone #4],c.Email4 ,c.Cellular4   FROM  dbo.WestCoastCustomers$ c where c.PK=@ROW_NO 
	   and ( isnull(c.[5th Contact],'') <> ''  or isnull(c.[Phone #4],'') <> '' or isnull(c.Email4,'')  <> '' or isnull(c.Cellular4,'') <> '' )

IF(isnull(@FName,'') !='')
BEGIN
IF NOT EXISTS (select 1 from rol inner join owner on Owner.Rol=rol.ID where rol.name=@FName) 
BEGIN


EXECUTE  [dbo].[spAddCustomer]
@UserName =@UserName,	
@Password=@Password,
@status =@status,
@FName =@FName,
@Address =@Address,
@City =@City,
@State =@State,
@Zip =@Zip,
@country =@country,
@Remarks =@Remarks,
@Mapping =@Mapping,
@Schedule =@Schedule ,
@Internet =@Internet,
@contact =@contact,
@phone =@phone,
@Website =@Website,
@email =@email,
@Cell =@Cell,
@Type =@Type,
@Equipment =@Equipment,
@SageID =@SageID,
@Billing =@Billing,
@grpbywo =@grpbywo,
@openticket =@openticket,
@ContactData =@ContactData,
@BillRate =@BillRate,
@OT =@OT,
@NT =@NT,
@DT =@DT,
@Travel =@Travel,
@Mileage =@Mileage,
@Fax = @Fax,
@EN =@EN,
@Lat =@Lat,
@Lng =@Lng,
@UpdatedBy='IMPORT FROM EXCELL',
@custom1=null,
@custom2=null

UPDATE dbo.WestCoastCustomers$  
SET MOMCustID =(select  top 1 owner.ID from rol inner join owner on Owner.Rol=rol.ID where rol.name=@FName ) 
WHERE pk=@row_no and  [Customer Name] = @FName
  
 
DELETE FROM @ContactData

END

ELSE
 
SET @ROW_NO+=1; 

END

END


--COMMIT TRAN