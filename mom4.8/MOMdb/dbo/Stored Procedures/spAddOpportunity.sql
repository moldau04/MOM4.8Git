CREATE PROC [dbo].[spAddOpportunity] @ID          INT,
                                    @fdesc       VARCHAR(75),
                                    @rol         INT,
                                    @Probability INT,
                                    @Status      SMALLINT,
                                    @Remarks     VARCHAR(max),
                                    @closedate   DATETIME,
                                    @Mode        SMALLINT,
                                    @owner int,
                                    @NextStep varchar(50),
                                    @desc varchar(max),
                                    @Source varchar(70),
                                    @Amount numeric(30,2),
                                    @Fuser varchar(50),
									@AssignedToID int,
                                    @UpdateUser varchar(50),
                                    @closed smallint,
                                    @TicketID int,
									@BusinessType varchar(50),
									@Product varchar(50),
									@OpportunityStageID int,
									@CompanyName VARCHAR(75),
									@IsSendMailToSalesPer bit=0,
                                    @Department Int
                                    
AS
	Declare @Currentrol varchar(150) = ''
	Declare @CurrentCompanyName varchar(100) = ''
	Declare @Currentfdesc VARCHAR(100) = ''
	Declare @CurrentStatus VARCHAR(100) = ''
	Declare @Currentclosedate VARCHAR(50) = ''
	Declare @CurrentOpportunityStageID VARCHAR(100) = ''
	Declare @CurrentSource varchar(100) = ''
	Declare @CurrentProbability varchar(50) = ''
	Declare @CurrentProduct varchar(100) = ''
	Declare @CurrentBusinessType varchar(100) = ''
	Declare @CurrentAmount varchar(30) = ''
	Declare @CurrentFuser varchar(100) = ''
	Declare @CurrentRemarks varchar(1000) = ''
	Declare @CurrentNextStep varchar(100) = ''
	Declare @Currentdesc varchar(1000) = ''
	
	
	Select @CurrentCompanyName = Lead.CompanyName 
		, @Currentfdesc = Lead.fDesc
		, @Currentclosedate = CONVERT(varchar(50), Lead.closedate, 101)
		, @CurrentSource = Lead.[Source]
		, @CurrentProbability = Case Lead.Probability WHEN 0 THEN 'Excellent' WHEN 1 THEN 'Very Good' WHEN 2 THEN 'Good' WHEN 3 THEN 'Average' WHEN 4 THEN 'Poor' END
		, @CurrentBusinessType = Lead.BusinessType
		, @CurrentAmount = Lead.Revenue
		--, @CurrentFuser = Lead.fuser
		, @CurrentFuser = u.SDesc
		, @CurrentRemarks = Lead.Remarks
		, @CurrentNextStep = Lead.NextStep
		, @Currentdesc = Lead.[Desc]
		, @Currentrol = rol.Name
		, @CurrentStatus = oe.Name
		, @CurrentOpportunityStageID = stage.Description
		, @CurrentProduct = s.Description
	from Lead 
	left join rol on rol.ID = Lead.Rol
	left join OEStatus oe ON lead.Status = oe.ID
	left join Stage on Stage.ID = lead.OpportunityStageID
	left join Service s on s.ID = Lead.Product
	left join Terr u on u.ID = lead.AssignedToID
	where Lead.ID =@ID

	--Select @Currentrol = Tag From Loc Where Rol = (Select Top 1 Rol from Lead where ID =@ID)	
	--Select @CurrentStatus = Name from OEStatus Where ID = (Select Status from Lead where ID =@ID)
	--Select @CurrentOpportunityStageID = Description FROM Stage Where ID = (Select OpportunityStageID from Lead where ID =@ID)
	--Select @CurrentProduct = Description from Service Where ID = (Select Product from Lead where ID =@ID)

    DECLARE @address VARCHAR(250)
    DECLARE @city VARCHAR(50)
    DECLARE @state CHAR(2)
    DECLARE @zip VARCHAR(28)
    Declare @RolType smallint

    SELECT @address = Address,
           @city = City,
           @state = State,
           @zip = Zip,
           @RolType=Type           
    FROM   Rol
    WHERE  ID = @rol

    IF ( @Mode = 0 )-- add new opprt
    BEGIN
		IF EXISTS(select 1 from Lead where fDesc=@fdesc and Rol=@rol)
		BEGIN
			RAISERROR('Opportunity with this name already exists for the Contact.',16,1)
			RETURN
		END

		-- Casse add new opportunity (no estimate)
		-- if user set status = Quoted (7) or Sold (5), system we reset it to Open
		IF @Status = 5 OR @Status = 7
			SET @Status = 1

		DECLARE @OppID INT
		SELECT @OppID = Isnull(Max(ID), 0) + 1 FROM   Lead

        INSERT INTO Lead
                      (ID,
                       fDesc,
                       RolType,
                       Rol,
                       Type,
                       Address,
                       City,
                       State,
                       Zip,
                       Owner,
                       Status,
                       Probability,
                       Level,
                       Remarks,
                       closedate,
                       GeoLock,
                       NextStep,
                       [Desc],
                       [Source],
                       Revenue,
                       Cost,Labor,Profit,Ratio, fuser,
					   AssignedToID,
                       CreateDate,
                       CreatedBy,
                       LastUpdateDate,
                       LastUpdatedBy,
                       Closed,
                       TicketID,
					   BusinessType,
					   Product,
					   OpportunityStageID,
					   CompanyName,
					   IsSendMailToSalesPer,
					   Department
                       )
          VALUES      ( @OppID,
                        @fdesc,
                       case @RolType when 4 then 2 when 3 then 0 end,
                        @rol,
                        'General',
                        @address,
                        @city,
                        @state,
                        @zip,
                        case @RolType
						  when 4 then (select top 1 Loc from Loc where Rol = @rol)
						  when 3 then (select top 1 ID from Prospect where Rol = @rol)
						end,
                        @Status,
                        @Probability,
                        1,
                        @Remarks,
                        @closedate,
                        0 ,
                        @NextStep,
                        @desc,
                        @Source,
                        @Amount,0,0,@Amount,100, @Fuser,
						@AssignedToID,
                        GETDATE(),
                        @UpdateUser,
                         GETDATE(),
                        @UpdateUser,
                        @closed,
                        @TicketID,
						@BusinessType,
						@Product,
						@OpportunityStageID,
						@CompanyName,
						@IsSendMailToSalesPer,
						@Department
                        )

		IF @@ROWCOUNT > 0
			SET @ID = @OppID;
    END
    ELSE IF( @Mode = 1 )
    BEGIN
		IF EXISTS(select 1 from Lead where fDesc=@fdesc and Rol=@rol and ID<>@ID)
		BEGIN
			RAISERROR('Opportunity with this name already exists for the Contact.',16,1)
			RETURN      
		END

		IF NOT EXISTS (SELECT TOP 1 1 from Estimate where Opportunity = @ID)
        BEGIN --Allow update department
			-- Casse no estimate link to opportunity
			-- if user set status = Quoted (7) or Sold (5), system we reset it to Open
			IF @Status = 5 OR @Status = 7
				SET @Status = 1

			UPDATE Lead
			SET    fDesc = @fdesc,
					 RolType = case @RolType when 4 then 2 when 3 then 0 end,
					 Rol = @rol,
					 Address = @address,
					 City = @city,
					 State = @state,
					 Zip = @zip,
					 Status = @Status,
					 Probability = @Probability,
					 Remarks = @Remarks,
					 closedate = @closedate,
					 NextStep=@NextStep,
					 LastUpdateDate=GETDATE(),
					 LastUpdatedBy=@UpdateUser,
					 [Desc]=@desc,
					 [Source]=@Source,
					 fuser=@Fuser,
					 AssignedToID=@AssignedToID,
					 closed=@closed,
					 Revenue=@Amount,
					 BusinessType=@BusinessType,
					 Product=@Product,
					 OpportunityStageID=@OpportunityStageID,
					 CompanyName=@CompanyName,
					 profit = (Revenue-(Cost+Labor)),
					 Ratio = case  Revenue when 0 then Ratio else (Profit/Revenue)*100 end,
					 Owner=(case @RolType
							  when 4 then (select top 1 Loc from Loc where Rol = @rol)
							  when 3 then (select top 1 ID from Prospect where Rol = @rol)
							end),
					 Department = @Department
			WHERE  ID = @ID
		END
		ELSE--Ignore updating department
		BEGIN
			-- Check and update Opportunity after updating Estimate Status
			IF EXISTS (
				SELECT TOP 1 1
				from lead ld
				inner join Estimate e on e.Opportunity = ld.ID 
				where e.Status = 5 
				and ld.id = @ID
				and e.Opportunity is not null
			) -- There is a Sold estimate link to this opportunity: Set Opprt status to 'Sold'
			BEGIN
				SET @Status = 5
			END
			ELSE IF EXISTS (
				SELECT TOP 1 1
				from lead ld
				inner join Estimate e on e.Opportunity = ld.ID 
				where e.Status = 1 
				and ld.id = @ID
				and e.Opportunity is not null
			)
			-- There is no Sold estimate link to this opportunity but have an open estimate link to this opportunity. Set Opprt status to 'Quoted'
			BEGIN
				SET @Status = 7
			END

			UPDATE Lead
			SET    fDesc = @fdesc,
					 RolType = case @RolType when 4 then 2 when 3 then 0 end,
					 Rol = @rol,
					 Address = @address,
					 City = @city,
					 State = @state,
					 Zip = @zip,
					 Status = @Status,
					 Probability = @Probability,
					 Remarks = @Remarks,
					 closedate = @closedate,
					 NextStep=@NextStep,
					 LastUpdateDate=GETDATE(),
					 LastUpdatedBy=@UpdateUser,
					 [Desc]=@desc,
					 [Source]=@Source,
					 fuser=@Fuser,
					 AssignedToID=@AssignedToID,
					 closed=@closed,
					 Revenue=@Amount,
					 BusinessType=@BusinessType,
					 Product=@Product,
					 OpportunityStageID=@OpportunityStageID,
					 CompanyName=@CompanyName,
					 profit = (Revenue-(Cost+Labor)),
					 Ratio = case  Revenue when 0 then Ratio else (Profit/Revenue)*100 end,
					 Owner=(case @RolType
							  when 4 then (select top 1 Loc from Loc where Rol = @rol)
							  when 3 then (select top 1 ID from Prospect where Rol = @rol)
							end),
					Department = CASE WHEN Department is null THEN @Department
									ELSE Department END
			WHERE  ID = @ID
		END
    END 

	/********Start Logs************/
	if(@CompanyName is not null AND @CurrentCompanyName is not null AND @CompanyName != @CurrentCompanyName)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Customer Name',@CurrentCompanyName,@CompanyName

	Declare @CurrentName varchar(150)
	Select @CurrentName = name From rol Where id = @rol
	if(@CurrentName is not null AND @Currentrol is not null AND @CurrentName != @Currentrol)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Location Name',@Currentrol,@CurrentName

	if(@fdesc is not null AND @Currentfdesc is not null AND @fdesc != @Currentfdesc)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Opportunity Name',@Currentfdesc,@fdesc

	Declare @CurrentStatusVal VARCHAR(100)
	Select @CurrentStatusVal = Name from OEStatus Where ID = @Status
	if(@CurrentStatusVal is not null AND @CurrentStatus is not null AND @CurrentStatusVal != @CurrentStatus)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Status',@CurrentStatus,@CurrentStatusVal

	Declare @ClosedateCurrent varchar(50)
	Select @ClosedateCurrent = CONVERT(varchar(50), @closedate, 101) 
	if(@ClosedateCurrent is not null AND @Currentclosedate is not null AND @Currentclosedate != @ClosedateCurrent)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Close Date',@Currentclosedate,@ClosedateCurrent

	Declare @OpportunityStageIDVal VARCHAR(100)
	Select @OpportunityStageIDVal = Description FROM Stage Where ID = @OpportunityStageID
	if(@OpportunityStageIDVal is not null AND @CurrentOpportunityStageID is not null AND @OpportunityStageIDVal != @CurrentOpportunityStageID)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Stage',@CurrentOpportunityStageID,@OpportunityStageIDVal

	if(@Source is not null AND @CurrentSource is not null AND @CurrentSource != @Source)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Source',@CurrentSource,@Source

	Declare @CurrentProbabilityVal varchar(50)
	Select @CurrentProbabilityVal = Case @Probability WHEN 0 THEN 'Excellent' WHEN 1 THEN 'Very Good' WHEN 2 THEN 'Good' WHEN 3 THEN 'Average' WHEN 4 THEN 'Poor' END
	if(@CurrentProbability is not null AND @CurrentProbabilityVal is not null AND @CurrentProbability != @CurrentProbabilityVal)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Probability',@CurrentProbability,@CurrentProbabilityVal

	Declare @CurrentProductVal varchar(100)
	Select @CurrentProductVal = Description from Service Where ID = @Product
	if(@CurrentProduct is not null AND @CurrentProductVal is not null AND @CurrentProduct != @CurrentProductVal)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Product',@CurrentProduct,@CurrentProductVal

	if(@CurrentBusinessType is not null AND @BusinessType is not null AND @CurrentBusinessType != @BusinessType)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Business',@CurrentBusinessType,@BusinessType

	if(@Amount is not null AND @CurrentAmount is not null AND @CurrentAmount != CONVERT(varchar(30),@Amount))
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Amount',@CurrentAmount,@Amount

	Declare @AssignedToName varchar(255) = ''
	SELECT @AssignedToName = SDesc FROM Terr WHERE ID = @AssignedToID
	if(@CurrentFuser is not null AND @AssignedToName is not null AND @CurrentFuser != @AssignedToName)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Assigned To',@CurrentFuser,@AssignedToName
	
	if(@CurrentRemarks is not null AND @Remarks is not null AND @CurrentRemarks != @Remarks)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Remarks',@CurrentRemarks,@Remarks

	if(@CurrentNextStep is not null AND @NextStep is not null AND @CurrentNextStep != @NextStep)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Next Step',@CurrentNextStep,@NextStep
	
	if(@Currentdesc is not null AND @desc is not null AND @Currentdesc != @desc)
		exec log2_insert @UpdateUser,'Opportunity',@ID,'Description',@Currentdesc,@desc
	/********End Logs************/

	Return @ID

GO