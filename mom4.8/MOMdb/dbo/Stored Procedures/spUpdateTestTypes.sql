CREATE PROCEDURE [dbo].[spUpdateTestTypes]	
	@ID INT,
	@Name varchar(50),
	@Authority varchar(25),
	@Frequency smallint,
	@Remarks varchar(8000),
	@Count smallint,
	@Cat varchar(25),
	@fDesc varchar(1000),
	@NextDateCalcMode TINYINT,
	@Charge smallint,
	@Status SMALLINT,
	@TestCover VARCHAR(100)	,
	@TicketCovered bit =0
AS 
BEGIN 		
	UPDATE [dbo].[LoadTest]
SET 
    [Name] = @Name
    ,[Authority] = @Authority
    ,[Frequency] = @Frequency
    ,[Remarks] = @Remarks
    ,[Count] = @Count    
    ,[Cat] = @Cat
    ,[fDesc] = @fDesc
	,[NextDateCalcMode]=@NextDateCalcMode
	,[Charge]=@Charge
	,[Status]=@Status
	WHERE ID=@ID

	DELETE FROM TestTypeCover WHERE TestTypeID=@ID;
	IF @TestCover<>''
	BEGIN
		INSERT INTO TestTypeCover (TestTypeID, TestTypeCoverID)
		SELECT @Id,* FROM SplitString ( @TestCover , ',' ) 

		UPDATE TestTypeCover
		SET TicketCovered=@TicketCovered
		WHERE TestTypeID=@ID
	END 
END 
