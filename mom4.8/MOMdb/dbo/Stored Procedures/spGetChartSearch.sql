CREATE PROCEDURE [dbo].[spGetChartSearch]
	@SearchValue int =  NULL,
	@SearchText varchar(75) = NULL,
	@BalCondition varchar(5) = NULL,
	@Type smallint = NULL ,
	@SubCat varchar(50) = NULL,
	@Status smallint = NULL,
	@EN int,
	@UserID int=0
AS
	
DECLARE @WOspacialchars varchar(75) 
SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	SET NOCOUNT ON;
	DECLARE @text varchar(max)

	set @text = 'SELECT  Ch.ID,      
           Ch.Acct,       
           Ch.fDesc, 
			(select CentralName from Central where id = Department)  
			as [Department], 
					 CASE WHEN ISNULL(Ch.Balance,0) > 0     
               THEN ISNULL(Ch.Balance,0)          
               ELSE 0                          
           END AS Debit,                       
           CASE WHEN ISNULL(Ch.Balance,0) < 0     
               THEN ISNULL(Ch.Balance,0) * -1     
               ELSE 0                          
           END AS Credit,                      
           ISNULL(Ch.Balance,0) as Balance,       
		   Ch.Type,                               
           Ch.Sub,                                
           Ch.InUse,                              
           Ch.Status,    
					(CASE isnull(Ch.Status,0) WHEN 0 THEN ''Active''                
						WHEN 1 THEN ''InActive''                           
						WHEN 2 THEN ''Hold'' END) AS AcctStatus,
					(CASE Ch.Type WHEN 0 THEN ''Asset'' 
						WHEN 1 THEN ''Liability''   
						WHEN 2 THEN ''Equity''        
						WHEN 3 THEN ''Revenue''       
						WHEN 4 THEN ''Cost''          
						WHEN 5 THEN ''Expense''       
						WHEN 6 THEN ''Bank'' 
						WHEN 7 THEN ''Non-Posting'' END) AS AcctType ,
					Sub2,Rol.EN, LTRIM(RTRIM(B.Name)) As Company

					FROM Chart Ch
					 left join Bank bk on ch.ID=bk.Chart 
					 Left Join Rol on bk.Rol=Rol.ID '
		IF(@EN=1)
		BEGIN
			SET @text +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN or UC.CompanyID = Ch.EN '
		END

		SET @text +='  left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN '
		SET @text += '	WHERE  1 = 1 '

		IF(@Type IS NOT NULL)
		BEGIN
			SET @text +=' AND Ch.Type = '+ convert(varchar(10),@Type) +''
		END

		IF(@SubCat IS NOT NULL)
		BEGIN
			SET @text +=' AND Sub LIKE ''%'+@SubCat+'%'''
		END

		IF(@Status = 0 OR @Status = 1 OR @Status = 2)
		BEGIN
			SET @text +=' AND Ch.Status = '+convert(varchar(10),@Status)+'' 
		END

		IF(@SearchValue = 1 OR @SearchValue = 2 OR @SearchValue = 3 OR @SearchValue = 4)
		BEGIN
			SET @text +='	INTERSECT 
							SELECT  Ch.ID,      
							Ch.Acct,       
							Ch.fDesc,
										(select CentralName from Central where id = Department)  
										as [Department], 
										 CASE WHEN ISNULL(Ch.Balance,0) > 0     
										THEN ISNULL(Ch.Balance,0)          
									   ELSE 0                          
										 END AS Debit,                       
										CASE WHEN ISNULL(Ch.Balance,0) < 0     
									   THEN ISNULL(Ch.Balance,0) * -1     
									   ELSE 0                          
									  END AS Credit,                      
									   ISNULL(Ch.Balance,0) as Balance,       
									   Ch.Type,                               
									  Ch.Sub,                                
								     Ch.InUse,                              
								    Ch.Status,    
										(CASE isnull(Ch.Status,0) WHEN 0 THEN ''Active''                
											WHEN 1 THEN ''InActive''                           
											WHEN 2 THEN ''Hold'' END) AS AcctStatus,
										(CASE Ch.Type WHEN 0 THEN ''Asset'' 
											WHEN 1 THEN ''Liability''   
											WHEN 2 THEN ''Equity''        
											WHEN 3 THEN ''Revenue''       
											WHEN 4 THEN ''Cost''          
											WHEN 5 THEN ''Expense''       
											WHEN 6 THEN ''Bank'' 
											WHEN 7 THEN ''Non-Posting'' END) AS AcctType ,
										Sub2,Rol.EN, LTRIM(RTRIM(B.Name)) As Company
						FROM Chart Ch 
					 left join Bank bk on ch.ID=bk.Chart 
					 Left Join Rol on bk.Rol=Rol.ID '
					 IF(@EN=1)
					BEGIN
						SET @text +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN or UC.CompanyID = Ch.EN '
						END
				SET @text +=' left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN '
		END
		IF(@SearchValue = 1 AND @SearchText <> '')
		BEGIN
			SET @text +='	WHERE (dbo.RemoveSpecialChars(Ch.Acct) LIKE ''%'+@WOspacialchars+'%'')	'
		END
		ELSE IF(@SearchValue = 2 AND @SearchText <> '')
		BEGIN
			SET @text +='	WHERE (dbo.RemoveSpecialChars(Ch.fDesc) LIKE ''%'+@WOspacialchars+'%'')	'
		END
		ELSE IF(@SearchValue = 3 AND @SearchText <> '')
		BEGIN
			IF (@BalCondition = '<>')
			BEGIN
				SET @text +='	WHERE (Ch.Balance <> '+@SearchText+') '
			END
			ELSE IF(@BalCondition = '<')
			BEGIN
				SET @text +='	WHERE (Ch.Balance < '+@SearchText+') '
			END
			ELSE IF(@BalCondition = '>')
			BEGIN
				SET @text +='	WHERE (Ch.Balance > '+@SearchText+') '
			END
			ELSE IF(@BalCondition = '<=')
			BEGIN
				SET @text +='	WHERE (Ch.Balance <= '+@SearchText+') '
			END
			ELSE IF(@BalCondition = '>=')
			BEGIN
				SET @text +='	WHERE (Ch.Balance >= '+@SearchText+')'
			END
		END
		ELSE IF(@SearchValue = 4 AND @SearchText <> '')
		BEGIN
			SET @text +=' WHERE (
							(select CentralName from Central where id = Department)
								LIKE ''%'+@SearchText+'%'')	'
		END

		IF(@EN=1)
		BEGIN
			SET @text += ' AND	UC.IsSel=1 and UC.UserID ='+convert(nvarchar(50),@UserID)  
		END

		SET @text +=' ORDER BY Ch.Acct '
	EXEC (@text)
	
END
GO