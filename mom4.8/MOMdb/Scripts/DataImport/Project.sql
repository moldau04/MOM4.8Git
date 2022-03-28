 


-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert Project
-- =============================================
--- Import JOB 
 
 GO
 
 CREATE TABLE [dbo].[ImportJob](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[MOM_Loc_ID] [int] NULL,
	[MOM_Cust_ID] [int] NULL,
	[MOM_Job_id] [int] NULL,
 CONSTRAINT [PK_ImportJob] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


 BEGIN TRAN

-----------COMMIT TRAN

-----------ROLLBACK TRAN

DECLARE @loc   INT,
        @owner            INT,
        @date             DATETIME,
        @Status           INT=0, 
        @Remarks          nvarchar(max), 
        @SDate            INT,
        @Route            VARCHAR(75),
        @hours            NUMERIC(30, 2),
        @fdesc            VARCHAR(75),
        @CType            VARCHAR(15),
        @ExpirationDate   DATETIME,
        @ExpirationFreq   SMALLINT,
        @Expiration       SMALLINT,
        @ContractBill     SMALLINT,
        @CustomerBill     SMALLINT,
        @Central          INT, 
        @JobT             INT =2,
		@type   int=3
       	
 

DECLARE @Row INT =1;

DECLARE @RowCount INT =0;

SELECT @RowCount = Max(pk) FROM  dbo.ImportJob

WHILE( @Row <= @RowCount )
  BEGIN ---1

     
	 IF NOT EXISTS(SELECT 1  FROM   dbo.ImportJob rc inner join Job j on rc.MOM_Loc_ID=j.Loc 
	 and j.Type =@type and j.Template=@JobT and rc.PK=@Row  )
        
		BEGIN ----2

             PRINT ( 'Not Exists Contract' )

		     PRINT ('Row No.-') PRINT (@Row) 
			 
             SELECT @Loc=rc.MOM_Loc_ID , @owner=rc.MOM_Cust_ID
			 
			 FROM dbo.ImportJob rc   WHERE rc.PK=@Row 
		 
		       
		

		  IF((isnull(@Loc,'') !='') and ( isnull(@owner,'') !=''))
	      
		   begin -----3
		   
		   SELECT                 
                @Date=GETDATE(), 
                @Route=(select route from Loc where loc=t.MOM_Loc_ID),
                @hours=1,
                @fdesc='Service & Repair-' + (select Tag from Loc where loc=t.MOM_Loc_ID),
                @CType='Serv & Rep',
                @ExpirationDate=Null,
                @ExpirationFreq=null,
                @Expiration=null,
                @ContractBill=0,
                @CustomerBill=0,
                @Central=0   
               
			FROM   dbo.ImportJob t

			where t.PK= @Row 

		 

           PRINT ('LOC -')PRINT ( @Loc)

	       PRINT ('Owner -')PRINT (@owner)

	       PRINT( 'Begin EXECUTE [[spAddProject]]' )
		  
DECLARE @Job INT  
DECLARE @tblCustomFieldsId INT  
DECLARE @tblTabID INT  
DECLARE @Label VARCHAR(50)  
DECLARE @TabLine SMALLINT  
DECLARE @Value VARCHAR(50)  
DECLARE @Format VARCHAR(50)  
DECLARE @ProjDate as DATETIME = GETDATE();  

  
  
    DECLARE   
            @projremark      VARCHAR(75),  
            @projname        VARCHAR(75),  
            @bomitems        TBLTYPEBOMITEM,  
            @MilestonItem    TBLTYPEMILESTONEITEM,  
            @servicetype     VARCHAR(15),  
            @InvExp          INT,  
            @InvServ         INT,  
            @WageS           INT,  
            @GLInt           INT,  
            @Post            TINYINT,  
            @Charges         TINYINT,  
            @JobClose        TINYINT,  
            @fInt            TINYINT,  
            @types           INT  
  
 
  
    SELECT @projremark = Remarks + CONVERT(VARCHAR(MAX), @Remarks),  
           @projname = fDesc,  
           @servicetype = CASE  
                            WHEN @CType = ''  
                            THEN  
                              CType  
                            ELSE  
                              @CType  
                          END,  
           @InvExp = InvExp,  
           @InvServ = InvServ,  
           @Wages = Wage,  
           @GLInt = GLInt,  
           @Post = Post,  
           @Charges = Charge,  
           @JobClose = JobClose,  
           @fInt = fInt,  
           @types = [Type]  
    FROM   JobT  
    WHERE  ID = @JobT  

	 
  
    INSERT INTO @bomitems  
                (JobT,  
                 Job,  
                 JobTItemID,  
                 fDesc,  
                 Code,  
                 Line,  
                 BType,  
                 QtyReq,  
                 UM,  
                 BudgetUnit,  
                 BudgetExt,  
                 LabItem,  
                 MatItem,  
                 MatMod,  
                 LabMod,  
                 LabExt,  
                 LabRate,  
                 LabHours,  
                 SDate,  
                 VendorId,GroupID)  
    SELECT JI.JobT,  
           JI.Job,  
           B.JobTItemID,  
           --ji.[Type] ,   
           fdesc,  
           JI.Code,  
           --ji.Budget,   
           Line,  
           B.[Type],  
           --b.Item,   
           B.QtyRequired,  
           UM,  
           --b.ScrapFactor ,   
           B.BudgetUnit,  
           B.BudgetExt,  
           B.LabItem,  
           B.MatItem,  
           JI.Modifier,--b.MatMod  
           JI.ETCMod,-- b.LabMod  
           JI.ETC,-- b.LabExt  
           B.LabRate,  
           JI.BHours,--b.LabHours  
           B.SDate,  
           B.Vendor,GroupID  
    --ji.Actual ,   
    --ji.[Percent]  
    FROM   BOM B  
           INNER JOIN jobtitem JI  
                   ON JI.ID = B.JobTItemID  
    WHERE  JI.JobT = @JobT  
           AND ( JI.job = 0  
                  OR JI.job IS NULL )  
  
    INSERT INTO @MilestonItem  
    SELECT [JobT],  
           [Job],  
           M.[JobTItemID],  
           JI.[Type],  
           [fdesc],  
           JI.[Code],  
           [Line],  
           M.[MilestoneName],  
           [RequiredBy],  
           0,  
           [ProjAcquistDate],  
           [ActAcquistDate],  
           [Comments],  
           M.[Type],  
           [Amount],  
     NULL,  
     GroupId  
    FROM   Milestone M  
           INNER JOIN jobtitem JI  
                   ON JI.ID = M.JobTItemID  
    WHERE  JI.JobT = @JobT         AND ( JI.job = 0    OR JI.job IS NULL )  
     
   DECLARE @UpdatedByUserId INT =0; 
 


    EXEC @Job = Spaddproject  
      @job =0,  
      @owner=NULL,  
      @loc=@loc,  
      @fdesc=@fdesc,  
      @status=@Status,  
      @type=@type,  
      @Remarks= @projremark,  
      @ctype =@servicetype,  
      @ProjCreationDate= @ProjDate,  
      @PO =null,  
      @SO =NULL,  
      @Certified = 0,  
      @Custom1 =NULL,  
      @Custom2 =NULL,  
      @Custom3 =NULL,  
      @Custom4 =NULL,  
      @Custom5 =NULL,  
      @template =@JobT,  
      @RolName=NULL,  
      @city =NULL,  
      @state =NULL,  
      @zip =NULL,  
      @country =NULL,  
      @phone =NULL,  
      @cellular =NULL,  
      @fax =NULL,  
      @contact =NULL,  
      @email =NULL,  
      @rolRemarks =NULL,  
      @rolType =NULL,  
      @InvExp =@InvExp,  
      @InvServ =@InvServ,  
      @Wage =@Wages,  
      @GLInt =@GLInt,  
      @jobtCType =NULL,  
      @Post =@Post,  
      @Charge =@Charges,  
      @JobClose =@JobClose,  
      @fInt =@fInt,   
      @BomItem = @bomitems,  
      @MilestonItem = @MilestonItem,  
      @BillRate=0,  
      @RateOT=0,  
      @RateNT=0,  
      @RateDT=0,  
      @RateTravel=0,  
      @Mileage=0,  
      @SPHandle =0,  
      @SPRemarks =null,  
      @IsRenewalNotes =null,  
      @RenewalNotes =null,  
	  @taskcategory =null,
	  @UpdatedByUserId=@UpdatedByUserId,
	  @TargetHPermission=0
				 
		   PRINT( 'End EXECUTE [[Spaddproject]]' )
		    
	       UPDATE e set e.MOM_Job_id=(SELECT max(id)  FROM   job j where owner=@owner and loc=@loc and j.Type =@type and j.Template=@JobT) 
		   FROM  dbo.ImportJob e 
		   where PK= @Row 

		   delete from @bomitems

		   delete from @MilestonItem

end ---3

		 end ----2

     SET @Row=@Row + 1 
  
  END ---1

  
 