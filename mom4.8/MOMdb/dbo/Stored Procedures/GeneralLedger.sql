CREATE PROCEDURE [dbo].[GeneralLedger] 
	 @cid   INT,
                                      @sdate DATETIME,
                                      @edate DATETIME
AS
  BEGIN
      SET NOCOUNT ON;

      DECLARE @id INT
      DECLARE @acctno VARCHAR(75)
      DECLARE @acct INT
	  DECLARE @ctype INT
      DECLARE @fDate DATETIME
      DECLARE @batch INT
      DECLARE @ref INT
      DECLARE @TypeText VARCHAR(75)
      DECLARE @type INT
      DECLARE @ChartName VARCHAR(75)
      DECLARE @fDesc VARCHAR(max)
      DECLARE @amount NUMERIC(30, 2)
      DECLARE @balance NUMERIC(30, 2)
      DECLARE @Debit NUMERIC(30, 2)
      DECLARE @Credit NUMERIC(30, 2)
      DECLARE @count INT = 0
      DECLARE @totalBalance NUMERIC(30, 2)
      DECLARE @prechartname VARCHAR(75)
      DECLARE @currchartname VARCHAR(75)
	  DECLARE @totalBalanceYE NUMERIC(30, 2)=0
	  Declare @Check int = 0;

      CREATE TABLE #tempChart
        (
           ID        INT,
           AcctNo    VARCHAR(75),
           Acct      INT,
           fDate     DATETIME,
           Batch     INT,
		   cType	 INT,
           Ref       INT,
           TypeText  VARCHAR(75),
           Type      INT,
           ChartName VARCHAR(75),
           fDesc     VARCHAR(max),
           Amount    NUMERIC(30, 2),
           Balance   NUMERIC(30, 2),
           Debit     NUMERIC(30, 2),
           Credit    NUMERIC(30, 2)
        )

      IF( @cid != 0 )
        BEGIN
            DECLARE db_cursor CURSOR FOR
              SELECT c.ID,
                     c.Acct,					 
                     t.Acct,
                     t.fDate,
                     t.Batch,c.type,
                     Isnull(t.Ref, 0)            AS Ref,
                     dbo.Transtypetotext(t.type) AS TypeText,
                     ( CASE t.Type
                         WHEN 50 THEN '1'
                         WHEN 40 THEN '2'
                         WHEN 41 THEN '2'
                         WHEN 21 THEN '3'
                         WHEN 20 THEN '3'
                         WHEN 5 THEN '4'
                         WHEN 6 THEN '4'
                         WHEN 5 THEN '5'
                         WHEN 6 THEN '5'
                         WHEN 1 THEN '6'
                         WHEN 2 THEN '6'
                         WHEN 3 THEN '6'
                         WHEN 40 THEN '8'
                         WHEN 41 THEN '8'
                         WHEN 98 THEN '9'
                         WHEN 99 THEN '9'
                         WHEN 30 THEN '7'
                         WHEN 31 THEN '7'
                         ELSE t.Type
                       END )                     AS Type,
                     Isnull(c.fDesc, '')         AS ChartName,
                     Isnull(t.fDesc, '')         AS fDesc,
                     Isnull(t.Amount, 0)         AS Amount,
                     0                           AS Balance,
                     ( CASE
                         WHEN t.Amount > 0 THEN t.Amount
                         ELSE 0
                       END )                     AS Debit,
                     ( CASE
                         WHEN t.Amount < 0 THEN ( t.Amount * -1 )
                         ELSE 0
                       END )                     AS Credit
              FROM   Chart c
                     INNER JOIN Trans t
                             ON c.ID = t.Acct
              WHERE  c.ID = @cid
                     AND fDate <= @edate
              ORDER  BY c.ID,
                        t.fDate
        END
      ELSE
        BEGIN
            DECLARE db_cursor CURSOR FOR
              SELECT c.ID,
                     c.Acct,
                     t.Acct,
                     t.fDate,
                     t.Batch,c.type,
                     Isnull(t.Ref, 0)            AS Ref,
                     dbo.Transtypetotext(t.type) AS TypeText,
                     ( CASE t.Type
                         WHEN 50 THEN '1'
                         WHEN 40 THEN '2'
                         WHEN 41 THEN '2'
                         WHEN 21 THEN '3'
                         WHEN 20 THEN '3'
                         WHEN 5 THEN '4'
                         WHEN 6 THEN '4'
                         WHEN 5 THEN '5'
                         WHEN 6 THEN '5'
                         WHEN 1 THEN '6'
                         WHEN 2 THEN '6'
                         WHEN 3 THEN '6'
                         WHEN 40 THEN '8'
                         WHEN 41 THEN '8'
                         WHEN 98 THEN '9'
                         WHEN 99 THEN '9'
                         WHEN 30 THEN '7'
                         WHEN 31 THEN '7'
                         ELSE t.Type
                       END )                     AS Type,
                     Isnull(c.fDesc, '')         AS ChartName,
                     Isnull(t.fDesc, '')         AS fDesc,
                     Isnull(t.Amount, 0)         AS Amount,
                     0                           AS Balance,
                     ( CASE
                         WHEN t.Amount > 0 THEN t.Amount
                         ELSE 0
                       END )                     AS Debit,
                     ( CASE
                         WHEN t.Amount < 0 THEN ( t.Amount * -1 )
                         ELSE 0
                       END )                     AS Credit
              FROM   Chart c
                     INNER JOIN Trans t
                             ON c.ID = t.Acct
              WHERE  fDate <= @edate
              ORDER  BY c.ID,
                        t.fDate
        END

      OPEN db_cursor

      FETCH NEXT FROM db_cursor INTO @id, @acctno, @acct, @fDate, @batch,@ctype, @ref, @TypeText, @type, @ChartName, @fDesc, @amount, @balance, @Debit, @Credit

      WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @currchartname =@ChartName

			If(@currchartname <> @prechartname)
			Begin
			set @check = 0
			End
			
			if @ctype in (3,4,5)
			  begin
				  declare @YE int			  
				  declare @startdate datetime
				  declare @enddate datetime
				  select @YE = isnull(YE,0) from control
				  
				  --set @enddate = cast((@YE+1)as varchar(2))+'/01/'+cast(year(@edate)as varchar(4))
				  --set @enddate = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@enddate)+1,0))
				  set @enddate = @edate

				  if(@YE=11)
				  Begin
				  --set @startdate = @sdate
				  set @startdate = '01'+'/01/'+cast(year(@sdate)as varchar(4))
				  End
				  
								  if(@fdate >= @startdate)
								  Begin
								  If(@Check=0)
								  
										  Begin
											set @totalBalanceYE = 0.00 + @amount
											IF ( @prechartname != @currchartname )
											  BEGIN
												  SET @totalBalanceYE = 0.00 + @amount
											  END

											SET @balance = @totalBalanceYE
											--set @Check = @Check + 1
										  End

										  Else										
								Begin
								set @totalBalanceYE = @totalBalanceYE+@amount
								IF ( @prechartname != @currchartname )
									BEGIN
										SET @totalBalanceYE = @amount
									END
								SET @balance = @totalBalanceYE											
							    End



										  End
									Else										
								Begin
								set @totalBalanceYE = @totalBalanceYE+@amount
								IF ( @prechartname != @currchartname )
									BEGIN
										SET @totalBalanceYE = @amount
									END
								SET @balance = @totalBalanceYE											
							    End
								
								
								
									   
			  End
			  else
			  begin

					IF( @count = 0 )
				  				  					  	  	  BEGIN
                  SET @totalBalance = @amount
              END
					ELSE
					  BEGIN
						  SET @totalBalance = @totalBalance + @amount
					  END

					IF ( @prechartname != @currchartname )
					  BEGIN
						  SET @totalBalance = @amount
					  END

					  SET @balance = @totalBalance

			  END
			if @ctype in (3,4,5)
			  begin
				if(@fdate >= @startdate)
				Begin
				If(@Check=0)
			    Begin
				INSERT INTO #tempChart
                VALUES  (@id,
                        @acctno,
                        @acct,
                        @fDate,
                        @batch,
						@ctype,
                        @ref,
                        @TypeText,
                        @type,
                        @ChartName,
                        @fDesc,
                        @amount,
                        @balance,						
                        @Debit,
                        @Credit)						
          		set @Check = @Check + 1
				--Set @balance = 0.00
				SET @prechartname = @ChartName
				End	
				Else
			Begin
            INSERT INTO #tempChart
            VALUES     (@id,
                        @acctno,
                        @acct,
                        @fDate,
                        @batch,
						@ctype,
                        @ref,
                        @TypeText,
                        @type,
                        @ChartName,
                        @fDesc,
                        @amount,
                        @balance,
                        @Debit,
                        @Credit)
            SET @count = @count + 1
            SET @prechartname = @ChartName
			End
			End	
			
			Else
			Begin
            INSERT INTO #tempChart
            VALUES     (@id,
                        @acctno,
                        @acct,
                        @fDate,
                        @batch,
						@ctype,
                        @ref,
                        @TypeText,
                        @type,
                        @ChartName,
                        @fDesc,
                        @amount,
                        @balance,
                        @Debit,
                        @Credit)
            SET @count = @count + 1
            SET @prechartname = @ChartName
			End
				
			End
			Else
			Begin
            INSERT INTO #tempChart
            VALUES     (@id,
                        @acctno,
                        @acct,
                        @fDate,
                        @batch,
						@ctype,
                        @ref,
                        @TypeText,
                        @type,
                        @ChartName,
                        @fDesc,
                        @amount,
                        @balance,
                        @Debit,
                        @Credit)
            SET @count = @count + 1
            SET @prechartname = @ChartName
			End
			
            FETCH NEXT FROM db_cursor INTO @id, @acctno, @acct, @fDate, @batch,@ctype, @ref, @TypeText, @type, @ChartName, @fDesc, @amount, @balance, @Debit, @Credit
        END

      CLOSE db_cursor

      DEALLOCATE db_cursor

      --For All Accounts
      --select * from #tempChart where (fDate >= '2016-01-01 00:00:00:000') AND ( fDate <= '2016-01-31 00:00:00:000')
      --select * from #tempChart where (fDate >= @sdate) AND ( fDate <= @edate)
      IF( @cid != 0 )
        BEGIN
            SELECT ID,
                   AcctNo,
                   NULL AS fdate,
                   Batch,
				   --cType,
                   NULL AS Ref,
                   NULL AS TypeText,
                   NULL AS Type,
                   ChartName,
                   NULL AS fDesc,
                   0.00 AS Amount,
                   Balance,
                   0.00 AS Debit,
                   0.00 AS Credit
            FROM   (SELECT ID,
                           AcctNo,
                           NULL                     AS fdate,
                           Batch,
						   --cType,
                           Ref,
                           NULL                     AS TypeText,
                           NULL                     AS Type,
                           ChartName,
                           NULL                     AS fDesc,
                           0.00                     AS Amount,
                           Balance,
                           0.00                     AS Debit,
                           0.00                     AS Credit,
                           Row_number() OVER (ORDER BY fdate DESC) RNK
                    FROM   #tempChart mt
                    WHERE  NOT EXISTS (SELECT ID
                                       FROM   #tempChart st
                                       WHERE  mt.ID = st.ID
                                              AND ( fDate >= @sdate )
                                              AND ( fDate <= @edate ))
                           AND Balance >= 0
                           AND ID = @cid
                   --Where ID NOT IN (select ID from #tempChart  where (fDate >= @sdate) AND (fDate <= @edate) ) and Balance <> 0 and ID=@cid
                   --And ID=@cid
                   ) old_trans
            --Where old_trans.RNK= (SELECT MAX(RNK) FROM #tempChart)
            WHERE  Ref = (SELECT Max(Ref)
                          FROM   #tempChart)
            UNION ALL
            SELECT ID,
                   AcctNo,
                   --Acct ,
                   fDate,
                   Batch,
				   --cType,
                   Ref,
                   TypeText,
                   Type,
                   ChartName,
                   fDesc,
                   Amount,
                   Balance,
                   Debit,
                   Credit
            FROM   #tempChart t
            WHERE  ( fDate >= @sdate )
                   AND ( fDate <= @edate )
                   AND ID = @cid
            ORDER  BY AcctNo
        END
      ELSE
        BEGIN
            SELECT ID,
                   AcctNo,
                   NULL AS fdate,
                   Batch,
				   --cType,
                   NULL AS Ref,
                   NULL AS TypeText,
                   NULL AS Type,
                   ChartName,
                   NULL AS fDesc,
                   0.00 AS Amount,
                   Balance,
                   0.00 AS Debit,
                   0.00 AS Credit
            FROM   (SELECT ID,
                           AcctNo,
                           NULL                   AS fdate,
                           Batch,
						   --cType,
                           Ref,
                           NULL                   AS TypeText,
                           NULL                   AS Type,
                           ChartName,
                           NULL                   AS fDesc,
                           0.00                   AS Amount,
                           Balance,
                           0.00                   AS Debit,
                           0.00                   AS Credit,
                           Row_number()
                             OVER (
                               PARTITION BY ID
                               ORDER BY Ref DESC) RNK
                    FROM   #tempChart mt
                    WHERE  NOT EXISTS (SELECT ID
                                       FROM   #tempChart st
                                       WHERE  mt.ID = st.ID
                                              AND ( fDate >= @sdate )
                                              AND ( fDate <= @edate ))
                           AND Balance >= 0) old_trans
            --Where ID NOT IN (select ID from #tempChart where (fDate >= @sdate) AND (fDate <= @edate) ) and Balance <> 0) old_trans
            --Where Batch=(SELECT MAX(Batch) FROM #tempChart)
            WHERE  old_trans.RNK = 1
            UNION ALL
            SELECT ID,
                   AcctNo,
                   --Acct ,
                   fDate,
                   Batch,
				   --cType,
                   Ref,
                   TypeText,
                   Type,
                   ChartName,
                   fDesc,
                   Amount,
                   Balance,
                   Debit,
                   Credit
            FROM   #tempChart t
            WHERE  ( fDate >= @sdate )
                   AND ( fDate <= @edate )
            ORDER  BY AcctNo
        END

      --Union All
      --Select  ID,Acct,ID,null,null ,null,null ,null,fDesc As ChartName,fDesc,0.00,Balance,0.00,0.00 from Chart where 
      --Acct Not in (select AcctNo from #tempChart where (fDate >= @sdate) AND (fDate <= @edate) ) Order By AcctNo
      --ORDER BY ID
	 
      DROP TABLE #tempChart
  END 
