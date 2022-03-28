-- =============================================
-- Author:		Nitin
-- Create date: 10-June-2015
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[spGetGroupedCustomersLocation]
	-- Add the parameters for the stored procedure here
	@DbName varchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @Query1 nvarchar(max)
	declare @Query2 nvarchar(max)
	set @DbName+='.dbo.'
	
	
	  -- Insert statements for procedure here
	  set @Query1 =  'select loc.loc, loc.[Owner],  (SELECT Name FROM  '+@DbName+'rol where ID = o.rol) as CustomerName, loc.ID, loc.Tag, loc.[Address], loc.City, loc.[Type]
		FROM '+@DbName+'[Loc] inner join '+@DbName+'owner o on loc.owner = o.id
		 where loc.owner is not null
		group by loc.loc, loc.[owner], loc.ID, loc.Tag, loc.Address, loc.City, loc.Type, o.Rol'
		
				
		 exec(@Query1)
		 
		  select Loc as LocId, Unit as EquipmentName, Manuf, [Type], Cat as ServiceType, Price, (case when Status=0 then 'Active' else 'Inactive' end) as Status 
   from [MSSample].[dbo].[Elev] order by Loc
		
		
END
