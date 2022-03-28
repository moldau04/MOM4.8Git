CREATE proc [dbo].[spAddWageDeduction]
@Id int,
@fDesc varchar(50),
@Type smallint,
@ByW smallint ,
@BasedOn smallint ,
@AccruedOn smallint,
@Count int ,
@EmpRate numeric(30, 4) ,
@EmpTop numeric(30, 2),
@EmpGL int ,
@CompRate numeric(30, 4) ,
@CompTop numeric(30, 2) ,
@CompGL int ,
@CompGLE int ,
@Paid smallint ,
@Vendor int ,
@Balance numeric(30, 2) ,
@InUse smallint ,
@Remarks varchar(8000) ,
@DedType smallint ,
@Reimb smallint ,
@Job smallint ,
@Box smallint ,
@Frequency int ,
@Process smallint
AS
BEGIN

IF ISNULL(@Id,0) =0  ---------------------------- NEW -----------------------------
BEGIN
if not exists( select 1 from PRDed where fDesc=@fDesc)
begin
SELECT @Id = ISNULL(MAX(ID),0)+1 FROM PRDed
INSERT INTO [PRDed] ([ID],[fDesc],[Type],[ByW],[BasedOn],[AccruedOn],[Count],[EmpRate],[EmpTop],[EmpGL],[CompRate],[CompTop],[CompGL]
           ,[CompGLE],[Paid],[Vendor],[Balance],[InUse],[Remarks],[DedType],[Reimb],[Job],[Box],[Frequency],[Process])
     VALUES 
           (@Id, @fDesc ,@Type ,@ByW ,@BasedOn ,@AccruedOn ,@Count ,@EmpRate ,@EmpTop ,@EmpGL ,@CompRate ,@CompTop ,@CompGL ,@CompGLE ,@Paid ,@Vendor ,@Balance ,
		@InUse  ,@Remarks  ,@DedType  ,@Reimb  ,@Job  ,@Box  ,@Frequency  ,@Process )	  
end
else
BEGIN
  RAISERROR ('Wage Deduction already exists, please use different name !',16,1)
  RETURN
END
END
ELSE               ----------------------------- UPDATE ----------------------
BEGIN
UPDATE PRDed SET [fDesc] = @fDesc,[Type]= @Type,[ByW]= @ByW,[BasedOn] = @BasedOn,[AccruedOn] = @AccruedOn,[EmpRate] = @EmpRate,[EmpTop] =@EmpTop,
				[EmpGL] = @EmpGL,[CompRate] = @CompRate,[CompTop] = @CompTop,[CompGL] = @CompGL
           ,[CompGLE] = @CompGLE,[Paid] = @Paid,[Vendor]=@Vendor,[Remarks]= @Remarks,[DedType]=@DedType,[Reimb]= @Reimb,
		   [Job] = @Job,[Box] = @Box,[Frequency] = @Frequency,[Process] = @Process WHERE ID = @Id
END





END
GO

