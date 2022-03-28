-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	Vendors
-- =============================================

-----IMPORT Vendors

BEGIN TRAN

-----------Commit tran

-----------Rollback tran

------------$$$$$$$$$$$---------------

DECLARE @Row INT =1;

DECLARE @RowCount INT =0;

SELECT @RowCount = Max(PK) FROM  Cas_4627.dbo.[vendors]  

WHILE( @Row <= @RowCount )

  BEGIN--->0

    IF EXISTS (SELECT 1 FROM Cas_4627.dbo.[vendors]  v where  v.PK=@Row  AND V.MOM_Vendor_ID IS NULL)
    
	BEGIN--->1

    DECLARE  @Rol_Name VARCHAR(75)
	,@Rol_City VARCHAR(50) = NULL
	,@Rol_State VARCHAR(2) = NULL
	,@Rol_Zip VARCHAR(10) = NULL
	,@Rol_Phone VARCHAR(28)=NULL
	,@Rol_Fax VARCHAR(28) =  NULL
	,@Rol_Contact VARCHAR(50) =  NULL
	,@Rol_Address VARCHAR(255) =  NULL
	,@Rol_Email VARCHAR(50) = NULL
	,@Rol_Website VARCHAR(50) =  NULL
	,@Rol_Country VARCHAR(50) =  NULL
	,@Rol_Cellular VARCHAR(28)
	--,@Rol_Remarks TEXT = NULL
	,@Rol_Type SMALLINT =  1
	,@Rol_fLong INT =  NULL
	,@Rol_Latt INT =  NULL
	,@Rol_GeoLock SMALLINT = 0
	,@Rol_Since DATETIME =  NULL
	,@Rol_Last DATETIME =  NULL
	,@Rol_EN INT =  Null
	,@Rol_Category VARCHAR(15) =  NULL
	,@Rol_Position VARCHAR(255) =  NULL
	,@Rol_Lat VARCHAR(50) =  0
	,@Rol_Lng VARCHAR(50) =  0
	,@Rol_LastUpdateDate DATETIME =  NULL
	,@Rol_coords SMALLINT =  NULL
	 
	SELECT @Rol_Name=v.name,
	@Rol_City=v.city,
	@Rol_State=v.state,
	@Rol_Address=v.address_1,
	@Rol_Phone=v.phone_no,
	@Rol_Zip=v.zip_code,
	@Rol_Fax=v.fax_no,
	@Rol_Email=v.email_address,
	@Rol_Contact=v.contact
	FROM Cas_4627.dbo.[vendors]  v where  v.PK=@Row

    EXECUTE [dbo].[spAddRolDetails]
     @Name =@Rol_Name
	,@City =@Rol_City
	,@State =@Rol_State
	,@Zip =@Rol_Zip
	,@Phone =@Rol_Phone
	,@Fax =@Rol_Fax
	,@Contact= @Rol_Contact
	,@Address =@Rol_Address
	,@Email =@Rol_Email
	,@Website =@Rol_Website
	,@Country =@Rol_Country
	,@Cellular =@Rol_Cellular
	,@Remarks =null
	,@Type =@Rol_Type
	,@fLong =@Rol_fLong
	,@Latt =@Rol_Latt
	,@GeoLock =@Rol_GeoLock
	,@Since =@Rol_Since
	,@Last =@Rol_Last
	,@EN  =  @Rol_EN
	,@Category  =  @Rol_Category
	,@Position =  @Rol_Position
	,@Lat  =  @Rol_Lat
	,@Lng  = @Rol_Lng
	,@LastUpdateDate  =  @Rol_LastUpdateDate
	,@coords  =  @Rol_coords 

    DECLARE	@Rol	int,	
@Acct	nvarchar(31),
@Type nvarchar(15)=1,
@Status [smallint]=0 ,
@ShipVia [varchar](50),
@Balance[numeric](30, 2),
@CLimit [numeric](30, 2),
@Terms[smallint]=3,
@Days [smallint]=0,
@Vendor1099 [smallint]=0,
@InUse [smallint]=0,
@DA [int] =0,
@Remit [varchar](255),
@Vendor1099Box  [tinyint]=7,
@FID varchar(15),
@AcctNumber [varchar](25),
@ContactName VARCHAR(50),
@Phone Varchar(50),
@Email Varchar(50),
@Cell Varchar(50),
@Fax varchar(50),
@EmailRecPO bit=0,
@VendorData As [dbo].[tblTypeVendorContact] 

    SET @Rol = (SELECT MAX(ID) FROM ROL where Name=@Rol_Name and Type =1)

	SELECT @AcctNumber=v.vendor_id ,
	@Acct=v.name
	FROM Cas_4627.dbo.[vendors]  v where  v.PK=@Row

    EXECUTE [dbo].[spAddVendor]
    @Rol=@Rol,	
	@Acct=@Acct,
	@Type =@Type,
	@Status =@Status ,
	@ShipVia=@ShipVia,
	@Balance=@Balance,
	@CLimit =@CLimit,
	@Terms=@Terms,
	@Days =@Days,
	@Vendor1099 =@Vendor1099,
	@InUse =@InUse,
	@DA =@DA ,
	@Remit =@Remit,
	@Vendor1099Box  =@Vendor1099Box,
	@FID =@FID,
	@AcctNumber =@AcctNumber,
	@ContactName =@ContactName,
	@Phone=@Phone,
	@Email =@Email,
	@Cell =@Cell,
	@Fax =@Fax,
	@EmailRecPO =@EmailRecPO,
	@VendorData=@VendorData

	update v set V.MOM_Vendor_ID=(select ID from Vendor where Rol=@Rol )
	 from Cas_4627.dbo.[vendors]  v where  v.PK=@Row  AND V.MOM_Vendor_ID IS NULL 
	 
	END--->1
	
	   PRINT(@Row)

      SET @Row=@Row + 1
  END--->0