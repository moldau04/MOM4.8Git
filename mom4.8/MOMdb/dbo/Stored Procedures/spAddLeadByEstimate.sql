
CREATE PROCEDURE [dbo].[spAddLeadByEstimate] 
	  @OpportunityName VARCHAR(255),
	  @OpportunityStageID INT,
	  @AssignedToID INT,
	  @CloseDate datetime,
	  @fDesc varchar(500),
	  @Status INT,
	  @CompanyName Varchar(100),
	  @LocationName Varchar(100),
	  @Amount Numeric(30,2),
	  @Rol INT,
	  @UpdateUser varchar(50)
AS
BEGIN
DECLARE @LeadNo INT
SET @LeadNo = (SELECT (MAX(ISNULL(ID,0))+1) FROM Lead)
IF @LeadNo IS NULL
				BEGIN
					SET @LeadNo=1;
				END

	DECLARE @address VARCHAR(250)
    DECLARE @city VARCHAR(50)
    DECLARE @state CHAR(2)
    DECLARE @zip VARCHAR(28)
	DECLARE @fUser VARCHAR(28)
    Declare @RolType smallint

    SELECT @address = Address,
           @city = City,
           @state = State,
           @zip = Zip,
           @RolType=Type           
    FROM   Rol
    WHERE  ID = @Rol

	--select @fUser=fUser from tblUser where id=@AssignedToID
	select @fUser=SDesc from terr where id=@AssignedToID
INSERT INTO [dbo].[Lead]
          (ID
		   ,fDesc
           ,OpportunityStageID
		   ,Rol
		   ,GeoLock
		   ,AssignedToID
		   ,CloseDate
		   ,Remarks
		   ,Status
		   ,CompanyName
		   ,Revenue
		   ,RolType
		   ,Type
		    ,Address
		   ,City
           ,State
           ,Zip
           ,Owner
		   ,CreateDate
		   ,LastUpdateDate
		   ,CreatedBy
		   ,LastUpdatedBy
		   ,fuser
		   )
     VALUES
           (@LeadNo
		   ,@OpportunityName
           , @OpportunityStageID
		   ,@Rol
		   , 0
		   ,@AssignedToID
		   ,@CloseDate
		   ,@fDesc
		   ,7
		   ,@CompanyName
		   ,@Amount
		   , case @RolType when 4 then 2 when 3 then 0 end
		   , 'General'
		    ,@address
            ,@city
            ,@state
            ,@zip
            , case @RolType
				when 4 then (select top 1 Loc from Loc where Rol = @rol)
				when 3 then (select top 1 ID from Prospect where Rol = @rol)
			end
			,GETDATE()
			,GETDATE()
			,@UpdateUser
			,@UpdateUser
			,@fUser
          )
select @LeadNo 
END
