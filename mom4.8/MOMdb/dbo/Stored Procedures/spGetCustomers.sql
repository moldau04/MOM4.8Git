CREATE PROCEDURE [dbo].[spGetCustomers] 
	@SearchBy       VARCHAR(20)= NULL,
    @SearchValue    VARCHAR(100) = NULL,
    @DbName         VARCHAR(50)=NULL,
    @IsSalesAsigned INT =0,
	@EN int				=0,
	@UserID int		= 0,
	@incInactive int = 0
AS
DECLARE @StatusId INT = 0
DECLARE @Text VARCHAR(max)
DECLARE @equipStatus varchar(10)
DECLARE @IsAssignedProject INT;
SET	@IsAssignedProject =isnull((SELECT TOP 1 IsAssignedProject FROM tblUser WHERE ID=@UserID),0);
DECLARE @EmpID INT;
SET @EmpID = ISNULL((SELECT e.ID FROM tblUser INNER JOIN Emp e ON e.CallSign= tblUser.fUser WHERE tblUser.ID=@UserID),0)

set @equipStatus='0'
if @incInactive=1
begin
	set @equipStatus='0,1'
end
DECLARE @SalesAsignedTerrID int = 0
IF( @IsSalesAsigned > 0)----If USER IS Salesperson
	BEGIN
		SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
	END
    SET @DbName='[' + @DbName + '].[dbo].'
    SET @Text= 'select distinct o.ID, LTRIM(RTRIM(r.Name)) as Name,r.EN, LTRIM(RTRIM(B.Name)) As Company, fLogin,CASE WHEN o.Status=0 THEN ''Active'' ELSE  ''Inactive'' END AS Status, r.Address, r.Zip, isnull(Balance,0) as Balance,o.type,r.city,r.State,r.phone,website,email,cellular,
(select count(1) from ' + @DbName
               + 'loc l where l.owner=o.id) as loc,
(select count(1) from ' + @DbName
               + 'elev e where e.owner=o.id and e.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','',''))) as equip,
(select count(1) from ' + @DbName
               + 'ticketo t where t.owner=o.id) + (select count(1) from '
               + @DbName
               + 'ticketd t where (select owner from '
               + @DbName + 'loc l where l.Loc = t.Loc)=o.ID) as opencall
, sageid,qbcustomerid
from ' + @DbName + '[Owner] o 
left outer join ' + @DbName + 'Rol r on o.Rol=r.ID  '
IF(@EN = 1)
BEGIN 
 SET @Text += 'left outer join ' + @DbName + 'tblUserCo UC on UC.CompanyID = r.EN '
 END
SET @Text += 'left outer join ' + @DbName + 'Branch B on B.ID = r.EN'

IF @IsAssignedProject=1
BEGIN

 SET @Text += ' left outer join ' + @DbName + 'Job j on j.Owner = o.ID '
END

    IF @SearchBy IS NOT NULL
      BEGIN
          --set @Text += ' where '+@SearchBy +' like '''+@SearchValue+'%''' 
          IF ( @SearchBy = 'r.Address'
                OR @SearchBy = 'r.name' )
            BEGIN
                SET @Text += ' where  o.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and ' + @SearchBy + ' like ''%'
                             + @SearchValue + '%''';
				IF(@EN = 1)
					  BEGIN
						  SET @Text+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
					  END
				IF( @IsSalesAsigned > 0  and @SalesAsignedTerrID > 0) ----If USER IS Salesperson
					BEGIN
						SET @Text+=' and o.ID in ([dbo].[Getsalesasignecustomer]'+ convert(nvarchar(50),@SalesAsignedTerrID)+ ') )'
					END
				IF @IsAssignedProject=1
					BEGIN
						SET @Text += ' and j.ProjectManagerUserID= ' + +convert(nvarchar(50),@EmpID)
					END
            END
		ELSE IF ( @SearchBy = 'B.Name' and @EN = 1 )
				BEGIN
				SET @Text+=' where  o.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and UC.IsSel = 1 and r.EN =' +convert(nvarchar(50),@SearchValue) + ' and UC.UserID ='+convert(nvarchar(50),@UserID) 
				END
          ELSE
            BEGIN
                SET @Text += ' where  o.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and ' + @SearchBy + ' like ''' + @SearchValue
                             + '%''';
				IF(@EN = 1)
				  BEGIN
					  SET @Text+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
				  END					
				IF( @IsSalesAsigned > 0  and @SalesAsignedTerrID > 0)----If USER IS Salesperson
				  BEGIN
					  SET @Text+=' and o.ID in ([dbo].[Getsalesasignecustomer]('+  convert(nvarchar(50),@SalesAsignedTerrID)+ ') )'
				  END
				  IF @IsAssignedProject=1
				BEGIN
				 SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
				END
            END
      END
	  ELSE IF( @IsSalesAsigned > 0  and @SalesAsignedTerrID > 0)----If USER IS Salesperson
		  BEGIN
			  SET @Text+=' where  o.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and o.ID in ([dbo].[Getsalesasignecustomer]('+ convert(nvarchar(50),@SalesAsignedTerrID)+')  )' 
			  IF @IsAssignedProject=1
				BEGIN
				 SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
				END
		  END
	  ELSE IF(@EN = 1)
		  BEGIN
			  SET @Text+=' where o.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)  IF @IsAssignedProject=1
				BEGIN
				 SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
				END                  
		  END
		  ELSE IF(@IsAssignedProject = 1)
		  BEGIN
			  
				
				 SET @Text += ' where j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
			           
		  END


    SET @Text +=' order by name'
	
	
	--select @Text
    EXEC (@Text)
	