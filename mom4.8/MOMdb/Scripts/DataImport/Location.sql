-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert Location
-- =============================================

---- ADD NEW LOCATION 
 

BEGIN TRAN
-----------commit tran
-----------rollback tran
DECLARE @Account VARCHAR(50);
DECLARE @LocName VARCHAR(100);
DECLARE @Address VARCHAR(255);
DECLARE @RolAddress VARCHAR(255);
DECLARE @RolCity VARCHAR(255);
DECLARE @RolState VARCHAR(255);
DECLARE @RolZip VARCHAR(255);
DECLARE @Type VARCHAR(255);
DECLARE @Owner int;
DECLARE @City varchar(50);
DECLARE @State varchar(2);
DECLARE @Zip varchar(10);
DECLARE @Route int;
DECLARE @Terr int;
DECLARE @Remarks nvarchar(max);
DECLARE @ContactName varchar(50);
DECLARE @Phone varchar(50);
DECLARE @fax varchar(28);
DECLARE @Cellular varchar(28);
DECLARE @Email varchar(50);
DECLARE @Website varchar(50);
DECLARE @stax varchar(50);
--DECLARE @Zone varchar(50);
------------$$$$$$$$$$$---------------
DECLARE @Row INT =1;

DECLARE @RowCount INT =0;

SELECT @RowCount = Max(PK) FROM  dbo.WestCoastLocations$

WHILE( @Row <= @RowCount )

  BEGIN

         


      ---------------Create New Location-------------
	   
     
	    IF  EXISTS(SELECT 1  FROM   dbo.WestCoastLocations$ where pk=@Row  and  MOM_LocID is null)
        BEGIN
            PRINT ( 'Not Exists Location' )

			SELECT 	@Account =cast((l.[Acct #]) as varchar(10)),
			@Owner=l.MOMCustID,
			@LocName= l.[Location Name],  
			@Address = l.[Location Address],
			@RolAddress = c.[Customer Address],
			@RolCity  = c.City,
			@RolState  = c.state,
			@RolZip  = c.zip,
			@Type  = l.Type,
			@City  = l.City,
			@State  = l.State,
			@Zip  =l.Zip,-- t.z
			@Route  =(select ID from Route where Name =l.[Route #]),
			@Terr  = 2,
			@Phone=c.[Phone #],
			@ContactName=c.[Main Contact],
			@fax=null,
			@Email=c.Email,
			@Remarks=l.Remarks ,  
			@Website=c.Website,
			@Cellular=c.Cellular,
			@stax='BC PST' 		
				 
            FROM   dbo.WestCoastLocations$ l
			INNER JOIN dbo.WestCoastCustomers$ c on  l.MOMCustID=c.MOMCustID
            WHERE  l.PK = @Row

            PRINT ( @Account )

            PRINT( 'Begin EXECUTE Spaddlocation' )

			IF(ISNULL(@Owner,0) !=0)

	   BEGIN

            EXECUTE [dbo].[Spaddlocation]
              @Account =@Account,
              @LocName =@LocName,
              @Address =@Address,
              @status =0,
              @City =@City,
              @State =@State,
              @Zip =@Zip,
              @Route =@Route,
              @Terr=@Terr,
              @Remarks =@Remarks,
              @ContactName =@ContactName,
              @Phone =@Phone,
              @fax =@fax,
              @Cellular =@Cellular,
              @Email =@Email,
              @Website =@Website,
              @RolAddress =@RolAddress,
              @RolCity =@RolCity,
              @RolState =@RolState,
              @RolZip =@RolZip,
              @Type =@Type,
              @Owner = @Owner,
              @Stax =@Stax,
              @Lat ='',
              @Lng ='',
              @Custom1 =NULL,
              @Custom2 =NULL,
              @To =NULL,
              @CC =NULL,
              @ToInv =NULL,
              @CCInv =NULL,
              @CreditHold =0,
              @DispAlert =0,
              @CreditReason =NULL,
              @prospectID =0,
              @ContractBill =0,
              @Terms =3,
            --@ContactData =@ContactData ,
              @BillRate =0,
              @OT =0,
              @NT =0,
              @DT =0,
              @Travel =0,
              @Mileage =0,            
              @EmailInvoice =0,
              @PrintInvoice =1,
              @EN = 0,
              @Terr2 =0,
			  @STax2= null,
			  @UTax = null,
              @Zone = 0,
			  @consult =null,
			  @UpdatedBy='IMPORT FROM EXCELL',
			  @country='Canada',
			  @rolcountry='Canada',
			  @NoCustomerStatement=1
			  

			update s set s.MOM_LocID=loc.Loc  FROM   Loc  INNER JOIN rol
            ON rol.ID = loc.Rol
			INNER JOIN dbo.WestCoastLocations$ s
			on s.[Location Name]=rol.NAME 
            WHERE s.PK = @Row and ROL.TYPE=4 

			 
END

            PRINT( 'End EXECUTE Spaddlocation' )
        END
	    
	    
       PRINT(@Row)

      SET @Row=@Row + 1
  END

 
