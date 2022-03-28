CREATE PROCEDURE [dbo].[spUpdateCompanyOffice]
@CompanyID	int,
@Name	varchar(75),
@Manager	varchar(50),
@Address	varchar(100),
@City	varchar(50),
@State	varchar(2),
@Zip	varchar(10),
@Phone	varchar(25),
@Fax	varchar(25),
@CostCenter	varchar(20),
@InvRemarks	varchar(8000),
@Logo	image,
@LogoPath	varchar(255),
@BillRemit	varchar(8000),
@PORemit	varchar(8000),
@LocDTerr	varchar(50),
@LocDRoute	varchar(50),
@LocDZone	varchar(50),
@LocDStax	varchar(50),
@LocType	varchar(50),
@ARTerms	varchar(50),
@ADP	varchar(3),
@CB	numeric(30, 2),
@ARContact	varchar(75),
@OType	varchar(50),
@DArea	varchar(3),
@DState	varchar(2),
@MileRate	numeric(30, 4),
@PriceD1	numeric(30, 4),
@PriceD2	numeric(30, 4),
@PriceD3	numeric(30, 4),
@PriceD4	numeric(30, 4),
@PriceD5	numeric(30, 4),
@UTaxR	tinyint,
@UTax	varchar(25),
@Status	int	,
@DInvAcct int,
@Longitude nvarchar(100),
@Latitude nvarchar(100),
@Country nvarchar(50)

As

BEGIN TRANSACTION
  
UPDATE Branch
SET

Manager			=	@Manager,
Address			=	@Address,
City			=	@City,
State			=	@State,
Zip				=	@Zip,
Phone			=	@Phone,
Fax				=	@Fax,
CostCenter		=	@CostCenter,
InvRemarks		=	@InvRemarks,
Logo			=	@Logo,
LogoPath		=	@LogoPath,
BillRemit		=	@BillRemit,
PORemit			=	@PORemit,
LocDTerr		=	@LocDTerr,
LocDRoute		=	@LocDRoute,
LocDZone		=	@LocDZone,
LocDStax		=	@LocDStax,
LocType			=	@LocType,
ARTerms			=	@ARTerms,
ADP				=	@ADP,
CB				=	@CB,
ARContact		=	@ARContact,
OType			=	@OType,
DArea			=	@DArea,
DState			=	@DState,
MileRate		=	@MileRate,
PriceD1			=	@PriceD1,
PriceD2			=	@PriceD2,
PriceD3			=	@PriceD3,
PriceD4			=	@PriceD4,
PriceD5			=	@PriceD5,
UTaxR			=	@UTaxR,
UTax			=	@UTax,
Status			=	@Status,
DInvAcct		=	@DInvAcct,
Longitude		=	@Longitude,
Latitude		=	@Latitude,
Country		=	@Country

Where ID = @CompanyID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
 return (@CompanyID)

GO