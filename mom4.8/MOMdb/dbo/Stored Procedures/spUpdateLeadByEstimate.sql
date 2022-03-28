CREATE PROCEDURE [dbo].[spUpdateLeadByEstimate] 
	  @ID INT,
	 @OpportunityName VARCHAR(255),
	  @OpportunityStageID INT,
	  @AssignedToID INT,
	  @CloseDate datetime,
	  @fDesc varchar(500),
	  @Status INT,
	  @CompanyName Varchar(100),
	  @LocationName Varchar(100),
	  @Amount Decimal,
	  @Rol INT,
	  @UpdateUser varchar(50),
	  @EstimateID INT
AS
BEGIN

	DECLARE @address VARCHAR(250)
    DECLARE @city VARCHAR(50)
    DECLARE @state CHAR(2)
    DECLARE @zip VARCHAR(28)
	DECLARE @fUser VARCHAR(28)
    Declare @RolType smallint
	Declare @Owner INT

    SELECT @address = Address,
           @city = City,
           @state = State,
           @zip = Zip,
           @RolType=Type           
    FROM   Rol
    WHERE  ID = @Rol

	--select @fUser=fUser from tblUser where id=@AssignedToID
	select @fUser=SDesc from terr where id=@AssignedToID


	if @RolType	= 4 
		begin
			set @Owner= (select top 1 Loc from Loc where Rol = @rol) 
		end
	else
		begin
			set @Owner= (select top 1 ID from Prospect where Rol = @rol)
		end

UPDATE Lead
SET 

fDesc=@OpportunityName,
OpportunityStageID=@OpportunityStageID,
Rol=@Rol,
GeoLock=0,
AssignedToID=@AssignedToID,
CloseDate=@CloseDate,
Remarks=@fDesc,
CompanyName=@CompanyName,
Revenue=@Amount,
RolType= case @RolType when 4 then 2 when 3 then 0 end,
Address=@address,
City=@city,
State=@state,
Zip=@zip,
Owner=@Owner,
CreatedBy=@UpdateUser,
LastUpdatedBy=@UpdateUser,
fuser=@fUser,
EstimateID=@EstimateID

WHERE ID=@ID

END
