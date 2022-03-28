CREATE PROCEDURE [dbo].[spAddCompanyOffice]
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

declare @CompID int
declare @DucplicateCompName int = 0

select @DucplicateCompName = COUNT(1) from Branch  where ID = @CompanyID  OR Name =@Name 

if(@DucplicateCompName=0)
begin

BEGIN TRANSACTION
  
Insert into Branch
(
ID,Name,Manager,Address,City,State,Zip,Phone,Fax,CostCenter,InvRemarks,Logo,LogoPath,BillRemit,PORemit,
LocDTerr,LocDRoute,LocDZone,LocDStax,LocType,ARTerms,ADP,CB,ARContact,OType,DArea,DState,MileRate,
PriceD1,PriceD2,PriceD3,PriceD4,PriceD5,UTaxR,UTax,Status,DInvAcct,Longitude,Latitude,Country
)
values
(
@CompanyID,@Name,@Manager,@Address,@City,@State,@Zip,@Phone,@Fax,@CostCenter,@InvRemarks,@Logo,@LogoPath,@BillRemit,@PORemit,
@LocDTerr,@LocDRoute,@LocDZone,@LocDStax,@LocType,@ARTerms,@ADP,@CB,@ARContact,@OType,@DArea,@DState,@MileRate,
@PriceD1,@PriceD2,@PriceD3,@PriceD4,@PriceD5,@UTaxR,@UTax,@Status,@DInvAcct,@Longitude,@Latitude,@Country
)
set @CompID=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
   end 
else
begin
 RAISERROR ('CompanyID or Name already exists, please use different CompanyID or Name !', 16, 1)  
 RETURN
end
 
 
 return (@CompID)

GO

