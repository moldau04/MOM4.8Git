CREATE PROCEDURE [dbo].[spCreateTestTypes]	
			@Name varchar(50),
			@Authority varchar(25),
			@Frequency smallint,
			@Remarks varchar(8000),
			@Count smallint,
			@Cat varchar(25),
			@fDesc varchar(1000),
			@NextDateCalcMode TINYINT,
			@Charge smallint,
			@Status smallint,
			@TestCover VARCHAR(100),		
			@TicketCovered BIT 
as
	begin

		DECLARE @Id int
		set @Id=(SELECT ISNULL(MAX(ID),0)+1 AS ID FROM LoadTest)
		INSERT INTO [LoadTest]
           (
		   [ID]
           ,[Name]
           ,[Authority]
           ,[Frequency]
           ,[Remarks]
           ,[Count]
           ,[Level]
           ,[Cat]
           ,[fDesc]
           ,[NextDateCalcMode]
		   ,[Charge]
		   ,[Status]
		   )
     VALUES
           (
		   @Id
           ,@Name
           ,@Authority
           ,@Frequency
           ,@Remarks
           ,@Count
           ,0
           ,@Cat
           ,@fDesc
           ,@NextDateCalcMode
		   ,@Charge
		   ,@Status
		   )		
		IF @TestCover<>''
		BEGIN
			INSERT INTO TestTypeCover (TestTypeID, TestTypeCoverID)
			SELECT @Id,* from SplitString ( @TestCover , ',') 

			UPDATE TestTypeCover
			SET TicketCovered=@TicketCovered
			WHERE TestTypeID=@ID
		END 
		select @Id
	end
GO