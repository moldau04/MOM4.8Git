CREATE PROCEDURE [dbo].[spUpdateCommodity]
@Id int,
@Code nvarchar(15)=null ,
@Desc nvarchar(75)=null,
@isActive int

as
	begin

	

		update Commodity set CommodityCode=@Code,CommodityDesc=@Desc,CommodityIsActive=@isActive where id=@Id
		
	end
