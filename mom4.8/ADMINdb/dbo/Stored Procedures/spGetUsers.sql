CREATE PROCEDURE [dbo].[spGetUsers] @SearchBy    VARCHAR(100)= NULL,
                                   @SearchValue VARCHAR(100) = NULL,
                                   @DbName      VARCHAR(50),
                                   @Issuper     INT,
                                   @super       VARCHAR(50)
AS
    set @DbName='['+ @DbName+'].[dbo].'

    DECLARE @StatusId INT = 0
    DECLARE @Text VARCHAR(max)
    DECLARE @uniontext VARCHAR(max)

    SET @Text=' select  e.ID, LTRIM(RTRIM(e.fFirst)) as fFirst, LTRIM(RTRIM(e.Last)) as lLast,u.ID as userid, fUser,u.Status,w.super,
  case when isnull(fWork,'''')='''' then ''Office'' else ''Field''  end as usertype ,
 case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid, 
case when isnull(fWork,'''')='''' then ''0_''+convert(varchar(50),u.id) else ''1_''+convert(varchar(50),u.id)  end as userkey
	from ' + @DbName + 'tblUser u 
	left outer join ' + @DbName
              + 'Emp e  on u.fUser=e.CallSign
	left outer join ' + @DbName
              + 'tblwork w on u.fuser=w.fdesc'

    --where u.Status='+CONVERT(varchar(20),@StatusID)
    IF @SearchBy IS NOT NULL
      BEGIN
          IF( @Issuper = 1 )
            BEGIN
                SET @Text += ' where w.super=''' + @super + ''' and '
                             + @SearchBy + ' like ''' + @SearchValue
                             + '%'''                                                      
                             
            END
          ELSE
            BEGIN
                IF( @SearchBy = 'usertype' )
                  BEGIN
                      SET @Text += ' where (select case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid) like '''
                                   + @SearchValue + '%'''
                  END
                ELSE
                  BEGIN
                      SET @Text += ' where ' + @SearchBy + ' like ''' + @SearchValue
                                   + '%'''
                  END
            END
      END

 IF( @Issuper = 1 )
 begin
 SET @Text += ' union  select  e.ID,LTRIM(RTRIM(e.fFirst)) as fFirst, LTRIM(RTRIM(e.Last)) as lLast,u.ID as userid, fUser,u.Status,w.super,
  case when isnull(fWork,'''')='''' then ''Office'' else ''Field''  end as usertype ,
 case when isnull(fWork,'''')='''' then 0 else 1  end as usertypeid, 
case when isnull(fWork,'''')='''' then ''0_''+convert(varchar(50),u.id) else ''1_''+convert(varchar(50),u.id)  end as userkey
	from ' + @DbName + 'tblUser u 
	left outer join ' + @DbName
              + 'Emp e  on u.fUser=e.CallSign
	left outer join ' + @DbName
              + 'tblwork w on u.fuser=w.fdesc where w.fDesc=''' + @super + ''''      
              
              IF @SearchBy IS NOT NULL 
               BEGIN
               if(@SearchBy<>'w.super')
               begin
               SET @Text += ' and '
                             + @SearchBy + ' like ''' + @SearchValue
                             + '%'''   
                             end                                      
               END
 end

    SET @uniontext=' union 
select  o.ID,r.Name,r.Name,o.ID as userid, fLogin,o.Status,'''' as super,''Customer'' as usertype, 2 as usertypeid ,
''2_''+convert(varchar(50),o.id) as userkey
from ' + @DbName + 'Owner o 
left outer join ' + @DbName
                   + 'Rol r on o.Rol=r.ID where internet=1 '

    --where o.Status='+CONVERT(varchar(20),@StatusID)
    IF ( @SearchBy IS NULL )
      BEGIN
          SET @Text+=@uniontext
      END
    ELSE IF @SearchBy IS NOT NULL
      BEGIN
          IF( @SearchBy <> 'w.super'
              AND @Issuper = 0 )
            BEGIN
                BEGIN
                    IF( @SearchBy <> 'usertype' )
                      BEGIN
                          SET @Text+=@uniontext

                          IF( @SearchBy = 'fUser' )
                            BEGIN
                                SET @SearchBy='flogin'
                            END
                          ELSE IF( @SearchBy = 'fFirst' )
                            BEGIN
                                SET @SearchBy='name'
                            END
                          ELSE IF( @SearchBy = 'e.Last' )
                            BEGIN
                                SET @SearchBy='name'
                            END
                          ELSE IF( @SearchBy = 'u.Status' )
                            BEGIN
                                SET @SearchBy='o.Status'
                            END

                          IF( @SearchBy <> 'usertype'
                              AND @SearchBy <> 'w.super' )
                            BEGIN
                                SET @Text += ' and ' + @SearchBy + ' like ''' + @SearchValue
                                             + '%'''
                            END
                      END
                    ELSE IF( @SearchBy = 'usertype'
                        AND @SearchValue = '2' )
                      BEGIN
                          SET @Text+=@uniontext
                      END
                END
            END
      END

    SET @Text +=' order by fUser'

    EXEC (@Text)
