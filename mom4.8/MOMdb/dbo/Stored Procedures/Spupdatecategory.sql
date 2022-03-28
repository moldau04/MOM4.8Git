
CREATE PROCEDURE [dbo].[Spupdatecategory] @type    VARCHAR(30),
                                  @remarks VARCHAR(8000),
                                  @icon    IMAGE = null,
								  @chargeable bit,
								  @default smallint,
								  @Status Bit
AS
if(@icon is not null)
begin

if(isnull(@default,0) = 1)
	begin
	update Category set ISDefault = 0  
	end
	
    UPDATE category
    SET    remarks = @remarks,
           icon = @icon,
		   Chargeable = @chargeable,
		   ISDefault = @default,
		   Status = @Status
    WHERE  type = @type
end
else
begin
if(isnull(@default,0) = 1)
	begin
	update Category set ISDefault = 0  
	end 
 UPDATE category
    SET    remarks = @remarks, Chargeable = @chargeable,ISDefault = @default, Status = @Status
    WHERE  type = @type
end
