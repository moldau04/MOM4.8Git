-- USP_UpdateReapproveFromFDESC
CREATE PROCEDURE USP_UpdateReapproveFromFDESC
As
BEGIN


	CREATE   TABLE  #Tmp_PO (RowID INT IDENTITY , PO INT , Fdesc VARCHAR(5000) );

	INSERT INTO #Tmp_PO(PO  , Fdesc )
	SELECT DISTINCT p.PO,   LEFT (Fdesc, 25)     
	FROM PO  p  Inner JOIN  ApprovalStatus a  ON p.PO = a.PO
	WHERE p.status in (0,3,4)

	DECLARE @Start INT  = 1 , @End INT ,@FDesc VARCHAR(25) = '',	@PO INT;

	SELECT @End = COUNT(PO) FROM #Tmp_PO;

	WHILE(@Start <= @End)
	BEGIN
		
		SELECT @FDesc = '',	@PO	=	0;

		SELECT @FDesc = Fdesc , @PO = PO FROM #Tmp_PO WHERE ROWID = @Start;
		IF CHARINDEX('ReApprove',@FDesc) > 0 
		BEGIN
			UPDATE ApprovalStatus 
				SET Status = 3 
			WHERE PO = @PO

			--Print  'Reapproved updated - ' + cast( @Start as varchar);
		END

		
		SET @Start+=1;
	END

 
	DROP TABLE #Tmp_PO;

	

END