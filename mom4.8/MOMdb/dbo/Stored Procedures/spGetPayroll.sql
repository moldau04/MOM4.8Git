CREATE PROCEDURE spGetPayroll
	@fDate DATETIME,
	@eDate DATETIME	
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION  
	    DECLARE @coCode varchar(100)
	    DECLARE @Batch varchar(100)
		DECLARE @c_EmpRef Varchar(200)
		DECLARE @c_EmpID int
		DECLARE @c_EmpName varchar(200)

		CREATE TABLE #temp(
			CoCode            Varchar(200),
			BatchID           Varchar(200),
			EmpRef            Varchar(200),
			[Shift]           INT,
			TempDept          Varchar(200),
			RateCode          Varchar(10),
			RegHours          NUMERIC (30, 2),	 
			OTHours           NUMERIC (30, 2),	 
			Hours3Code        Varchar (10),	
			Hours3Amount      Varchar (10)
	  
		)
		-- Variable for Infor
		DECLARE @i_Remarks varchar(100)
		DECLARE @i_WageC NUMERIC (30, 2)
		DECLARE @i_QReg NUMERIC (30, 2)
		DECLARE @i_QOT NUMERIC (30, 2)
		DECLARE @i_QNT NUMERIC (30, 2)
		DECLARE @i_QDT NUMERIC (30, 2)
		DECLARE @i_QTT NUMERIC (30, 2)

		-- Variable Export		
		DECLARE @Shift		INT
		DECLARE	@TempDept   Varchar(200)
		DECLARE @RateCode   Varchar(10)
		DECLARE @QReg		NUMERIC (30, 2)
		DECLARE @QOT		NUMERIC (30, 2)
		DECLARE @Hours3Code Varchar (10)
		DECLARE @Hours3Amount NUMERIC (30, 2)


		SET @Batch=(SElect replace(convert(varchar, getdate(),101),'/','')) 
		SET @coCode=(SELECT TOP 1 coCode FROM Control)
		DECLARE db_cursor CURSOR FOR 
			SELECT Emp.Ref, Emp.ID, Emp.Name
			FROM Emp WHERE Emp.Status=0  ORDER BY Emp.ACH, Emp.Name

		OPEN db_cursor  	
		FETCH NEXT FROM db_cursor INTO  @c_EmpRef,@c_EmpID,@c_EmpName
	
		WHILE @@FETCH_STATUS = 0
		BEGIN
	
			DECLARE db_cursor_Infor CURSOR FOR 
				SELECT (LType.Remarks), TicketD.WageC, Sum(TicketD.Reg) AS QReg, Sum(TicketD.OT) AS QOT, Sum(TicketD.NT) AS QNT, Sum(TicketD.DT) AS QDT, Sum(TicketD.TT) AS QTT
				FROM TicketD 
				INNER JOIN Emp ON TicketD.fWork = Emp.fWork 
				INNER JOIN Job ON TicketD.Job=Job.ID 
				INNER JOIN LType ON Job.CType=LType.Type  
				WHERE TicketD.EDate>=@fDate AND TicketD.EDate<=@eDate  AND TicketD.Job NOT IN(10850,10851,10852) 
				AND Emp.ID= @c_EmpID AND TicketD.ClearCheck=1
				GROUP BY TicketD.WageC, Emp.Ref, LType.Remarks
			OPEN db_cursor_Infor  
			FETCH NEXT FROM db_cursor_Infor INTO @i_Remarks,@i_WageC,@i_QReg,@i_QOT,@i_QNT,@i_QDT,@i_QTT

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @Shift=1
				SET @TempDept=@i_Remarks	
				SET @QReg=@i_QReg
				SET @RateCode=''
				SET @Hours3Code=''
				SET @Hours3Amount=0
				SET @Hours3Amount=@i_QNT+@i_QDT+@i_QTT
				
		

				IF @i_WageC=4 					
				BEGIN					
					SET @Hours3Code='H'
					SET @TempDept=''
					SET @Hours3Amount=@Hours3Amount +@QReg
					SET @QReg=0
					 
				END					

				IF @i_WageC=5
				BEGIN					
					SET @Hours3Code='H'
					SET @TempDept='' 
					SET @RateCode='2'
					SET @Shift=2
				END

				
				IF @i_WageC=3
				BEGIN					
					SET @TempDept='300'
					SET @Shift=2
					SET @RateCode='2'	
				END

			    IF @Hours3Amount>0
				BEGIN					
					SET @Hours3Code='D'
				END
				IF @i_Remarks= '300'
				BEGIN 
					SET @Shift=2
					SET @RateCode='2'				
					IF @i_WageC=4 or @i_WageC=5					
						BEGIN	
							SET @Hours3Amount=@Hours3Amount +@QReg
							SET @QReg=0
						END				
				END
				
			

				INSERT INTO #temp (CoCode,BatchID,EmpRef,[Shift],TempDept,RateCode,RegHours,OTHours,Hours3Code,Hours3Amount)
						Values(@coCode,@Batch,@c_EmpRef,@Shift,@TempDept,@RateCode,@QReg,@i_QOT,@Hours3Code,@Hours3Amount)
			

			-- update
			
			UPDATE TicketD SET ClearPR=1  WHERE TicketD.EDate>=@fDate AND TicketD.EDate<=@eDate  AND fwork=(SELECT top 1 fwork as FValue FROM Emp WHERE ID=@c_EmpID)

			
			FETCH NEXT FROM db_cursor_Infor INTO @i_Remarks,@i_WageC,@i_QReg,@i_QOT,@i_QNT,@i_QDT,@i_QTT
			END
			CLOSE db_cursor_Infor  
			DEALLOCATE db_cursor_Infor
		
		FETCH NEXT FROM db_cursor INTO  @c_EmpRef,@c_EmpID,@c_EmpName
		END
		CLOSE db_cursor  
		DEALLOCATE db_cursor
	
	 --For all hours for Dept 300 make Shift = 3 and all else set to 2. There should be no value where shift = 1
	
	
	  UPDATE #temp
	 SET [Shift]=3
	 WHERE TempDept='300'
	 UPDATE #temp
	 SET [Shift]=2
	-- WHERE TempDept<>'300' AND TempDept<>''
	 WHERE [Shift]=1

	 SELECT *  FROM #temp
		 DROP  TABLE #temp
		COMMIT
	END TRY
	BEGIN CATCH	
	CLOSE db_cursor  
		DEALLOCATE db_cursor
		CLOSE db_cursor_Infor  
			DEALLOCATE db_cursor_Infor

		SELECT ERROR_MESSAGE() AS ErrorMessage; 
		IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

	END CATCH



END
