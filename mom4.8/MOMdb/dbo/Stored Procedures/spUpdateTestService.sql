CREATE PROCEDURE [dbo].[spUpdateTestService]	
	@id int	 
	,@ServiceDate VARCHAR(MAX)
	,@ServiceYear int  
	,@ServiceStatus INT
    ,@ServiceWorker VARCHAR(MAX)
	,@UpdatedBy varchar(200)
	
AS 
BEGIN 		
	IF (SELECT COUNT(1) FROM LoadTestItemService WHERE LID=@ID AND ServiceYear=@ServiceYear)=1
	BEGIN
		UPDATE [dbo].[LoadTestItemService]
		SET		
		[ServiceDate]=@ServiceDate
		,[ServiceStatus]=@ServiceStatus
		,[Worker]=@ServiceWorker
		,[CreatedBy]=@UpdatedBy
		WHERE LID=@ID AND ServiceYear=@ServiceYear
    END 
	ELSE
	BEGIN
		INSERT INTO LoadTestItemService ([LID],[ServiceYear],[ServiceDate],[Worker],[ServiceStatus], [CreatedBy])
		VALUES( @ID,@ServiceYear,@ServiceDate,@ServiceWorker,@ServiceStatus,@UpdatedBy)
	END
	
END 

