Create proc SpGetJobCodeBy_DeptID
@DeptID int=0,
@Searchvalues varchar(100)=null
AS
if (isnull(@Searchvalues,'') !='')

BEGIN

	SELECT * FROM (
		SELECT ID as value
			, Code as label 
			,(select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where  JobCodeID=jc.ID and JobTypeID=@DeptID) as CodeDesc
		FROM JobCode jc 
		) X
	where 
		((x.label like '%'+@Searchvalues+'%'  or   x.CodeDesc like '%'+@Searchvalues+'%' ))

END
ELSE 
Begin
	SELECT ID as value
		, Code as label 
		,(select top 1 JobCodeDesc 	from tblJobCodeDesc_ByJobType 	where  JobCodeID=jc.ID and JobTypeID=@DeptID) as CodeDesc
	FROM JobCode jc 
END