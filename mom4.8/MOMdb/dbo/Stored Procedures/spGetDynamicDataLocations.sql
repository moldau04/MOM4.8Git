 CREATE PROCEDURE [dbo].[spGetDynamicDataLocations]  
 @SearchBy varchar(70) = NULL,  
 @SearchValue varchar(6000) = NULL,  
 @DbName varchar(50) = NULL,  
 @IsSalesAsigned int = 0,  
 @EN int = 0,  
 @UserID int = 0 ,
 @incInactive int = 0
AS  
BEGIN  
  

    DECLARE @StatusId int = 0  
    DECLARE @Active varchar(10) = 'Active'  
    DECLARE @Inactive varchar(10) = 'Inactive'  
    DECLARE @Text varchar(max)  
    DECLARE @SalesAsignedTerrID int = 0  
    IF (@IsSalesAsigned > 0)  
    BEGIN  
        SELECT  
            @SalesAsignedTerrID = ISNULL(id, 0)  
        FROM Terr  
        WHERE Name = (SELECT  
            fUser  
        FROM tblUser  
        WHERE id = @IsSalesAsigned)  
    END  
	
	Declare @equipStatus varchar(10)
	set @equipStatus='0'
	if @incInactive=1
	begin
		set @equipStatus='0,1'
	end
    --- $$$$$ Removed Unused Column from select Statement $$$ Changed By NK    
  
	DECLARE @IsAssignedProject INT;
	SET	@IsAssignedProject =isnull((SELECT TOP 1 IsAssignedProject FROM tblUser WHERE ID=@UserID),0);
	DECLARE @EmpID INT;
	SET @EmpID = ISNULL((SELECT e.ID FROM tblUser INNER JOIN Emp e ON e.CallSign= tblUser.fUser WHERE tblUser.ID=@UserID),0)


    SET @Text =  
    ' SELECT    
  l.Loc ,     
  LTRIM(RTRIM(l.ID))  as locid,     
  LTRIM(RTRIM(r.Name)) as Name,     
   l.Status,
  (select count(ID) from  elev e with (nolock) where e.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and e.loc=l.loc) as Elevs, 
  isnull(l.Balance,0) as Balance,    
  LTRIM(RTRIM(l.Tag)) as Tag,    
  l.Address,    
  l.City,    
  l.Type,    
  (select top 1 fDesc from  State where Name=l.State) State,  
  isnull(l.credit,0) as credit,    
  (select count(1) from   ticketo t with (nolock) where t.lid=l.loc and ltype=0) + (select count(1) from   ticketd t with (nolock) where t.loc=l.loc) as opencall,     
  qblocid,  
  r.name as Customer,    
  r.EN,     
  (Select B.Name from  Branch B with (nolock) where ID =(r.EN )) As Company,    
  LTRIM(RTRIM(l.Tag)) as Location,  
  l.NoCustomerStatement,    
  r.Address + '',''+ r.City + '','' + r.State + '','' + r.Zip + '','' + r.Phone  As CustomerName,     
  l.Address + '', ''+ l.City + '', '' + l.State + '' '' + l.Zip   As LocationName,    
  tr.Name AS Salesperson, 
  
  isnull((select top 1 Name from Terr where Terr.ID=L.Terr2),'''') as Salesperson2,

  isnull(rt.Name  + 
  
  (select ( case   when tblwork.fdesc=rt.Name then '''' else''-''+ tblwork.fdesc   end) 
  
  from tblwork where tblwork.id=rt.mech   ),''Unassigned'')  AS RouteName,
  
  ISNULL(cst.Name, ''None'') AS ConsultantName, 
  
  o.Custom1 AS OwnerName   ,isnull(l.SageID,'''') as  SageID     
 ,  ISNULL(lr.EMail,'''') as Email
 ,  ISNULL(lr.Contact,'''') as ContactName
 , ISNULL(z.Name,'''') as Zone
 , l.BusinessType
 ,ISNULL(bt.Description,'''') as BusinessTypeName
 ,o.id  as CusID
  ,case l.Status when 0 then ''Active'' else ''Inactive'' end as locStatus
  ,l.zip
 FROM    loc l with (nolock)    
 INNER JOIN owner o with (nolock) ON o.id = l.owner    
 INNER JOIN rol r with (nolock)   ON o.rol = r.id    
 INNER JOIN rol lr with (nolock)  ON l.rol = lr.id     
 LEFT JOIN Zone z with (nolock)  ON z.ID=l.zone
 LEFT JOIN  STax S with (nolock)   ON L.STax = S.Name AND s.UType=0    
 LEFT JOIN  Terr tr with (nolock)  ON L.Terr = tr.ID     
 LEFT JOIN  Route rt with (nolock) ON L.Route = rt.ID     
 LEFT JOIN  tblConsult cst with (nolock) ON cst.ID = l.Consult 
 LEFT JOIN  BusinessType bt with (nolock) ON bt.ID =  l.BusinessType '  

    IF (@EN = 1)  
    BEGIN  
        SET @Text += ' left outer join tblUserCo UC  on UC.CompanyID = r.EN '  
    END  

    SET @Text += ' left outer join Branch B  on B.ID= r.EN and r.Type=4'   
  
	IF @IsAssignedProject=1
	BEGIN

	 SET @Text += ' left outer join  Job j on j.loc = l.Loc '
	END

    IF (ISNULL(@SearchBy, '') <> '' )  
    BEGIN  
        IF (@SearchBy = 'l.Address'  
            OR @SearchBy = 'l.ID'  
            OR @SearchBy = 'tag'  
            OR @SearchBy = 'l.City'  
            OR @SearchBy = 'l.ID'  
            OR @SearchBy = 'l.tag'  
            OR @SearchBy = 'Customer'  
            OR @SearchBy = 'l.State'  
            OR @SearchBy = 'l.PriceL'  
            --OR @SearchBy = 'l.Zone'  
            OR @SearchBy = 'l.Status'  
           -- OR @SearchBy = 'l.type'  
            OR @SearchBy = 'Balance'  
            --OR @SearchBy = 'rt.Name'         
            OR @SearchBy = 'Mech'  
            OR @SearchBy = 'sTax'  
            OR @SearchBy = 'sTaxRate'  
            OR @SearchBy = 'sTaxRemarks'  
            OR @SearchBy = 'l.Custom12'  
            OR @SearchBy = 'l.Custom13'  
            OR @SearchBy = 'l.Custom14'  
            OR @SearchBy = 'l.Custom15'  
            OR @SearchBy = 'LocationCountry'  
            OR @SearchBy = 'l.EmailInvoice'  
            OR @SearchBy = 'l.NoCustomerStatement'
			OR @SearchBy = 'l.BusinessType' 
			OR @SearchBy = 'l.Zip')  
        BEGIN  	
			
			 SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  ' + @SearchBy + ' like ''%' + @SearchValue + '%''';  
          
			  IF (@EN = 1)  
            BEGIN  
                SET @Text += ' and UC.IsSel = 1 and UC.UserID =' + CONVERT(nvarchar(50), @UserID)  
            END  
            IF (@IsSalesAsigned > 0  
                AND @SalesAsignedTerrID > 0)  
                SET @Text += ' and    l.Terr=' + CONVERT(nvarchar(10), (@SalesAsignedTerrID));  
			IF @IsAssignedProject=1
			BEGIN
				SET @Text += ' and j.ProjectManagerUserID= ' + +convert(nvarchar(50),@EmpID)
			END
           
        END  
        ELSE IF (@SearchBy = 'B.Name' AND @EN = 1)  
			BEGIN  
				SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  UC.IsSel = 1 and r.EN =' + CONVERT(nvarchar(50), @SearchValue) + ' and UC.UserID =' + CONVERT(nvarchar(50), @UserID)  
			END   
		ELSE IF (@SearchBy = 'l.elevs')  
			BEGIN  
				IF (ISNUMERIC(@SearchValue) = 1)
				BEGIN
					 SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  (select count(ID) from elev e where e.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and     e.loc=l.Loc)=' + @SearchValue; 
				END
				ELSE
				BEGIN
					SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  (select count(ID) from elev e where e.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and    e.loc=l.Loc)= -1';  
				END
            
			END   
		ELSE IF (@SearchBy = 'l.zone')  
			BEGIN  
			   IF (ISNULL(@SearchValue,'') <> '')  
				SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  l.zone in (' + @SearchValue + ')';  
			END  
			ELSE IF (@SearchBy = 'l.type')  
			BEGIN  
			   IF (ISNULL(@SearchValue,'') <> '')  
				SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  l.type in  ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@SearchValue+''','',''))';  
			END
			ELSE IF (@SearchBy = 'rt.Name')  
			BEGIN  
			   IF (ISNULL(@SearchValue,'') <> '')  
				SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and  L.Route in  ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@SearchValue+''','',''))';  
			END    
        ELSE  
        BEGIN  
            SET @Text += ' where  l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and ' + @SearchBy + ' like ''' + @SearchValue + '%''';  
            IF (@EN = 1)  
            BEGIN  
                SET @Text += ' and UC.IsSel = 1 and UC.UserID =' + CONVERT(nvarchar(50), @UserID)  
            END  
            IF (@IsSalesAsigned > 0 AND @SalesAsignedTerrID > 0)  
                SET @Text += ' and    l.Terr=' + CONVERT(nvarchar(10), (@SalesAsignedTerrID));  

			IF @IsAssignedProject=1
				BEGIN
					SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
				END
        END  
  
    END  
    ELSE IF (@IsSalesAsigned > 0 AND @SalesAsignedTerrID > 0)  
    BEGIN  
        SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and    l.Terr=' + CONVERT(nvarchar(10), (@SalesAsignedTerrID)) + ' or isnull(l.Terr2,0)=' + CONVERT(nvarchar(10), (@SalesAsignedTerrID));  
		
		IF @IsAssignedProject=1
			BEGIN
				SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
			END
    END  
    ELSE IF (@EN = 1)  
    BEGIN  
        SET @Text += ' where l.Status in ( SELECT SplitValue FROM [dbo].[fnSplit]('''+@equipStatus+''','','')) and    UC.IsSel = 1 and UC.UserID =' + CONVERT(nvarchar(50), @UserID)  
		IF @IsAssignedProject=1
			BEGIN
				SET @Text += ' and j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)
			END
    END  
	ELSE IF(@IsAssignedProject = 1)
		BEGIN				
				SET @Text += ' where j.ProjectManagerUserID= ' + convert(nvarchar(50),@EmpID)			           
		END

  
  
    SET @Text += '  order by locid'  
  
    PRINT (@Text)  
  
    EXEC (@Text)  
  
  
END