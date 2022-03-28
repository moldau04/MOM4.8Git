
CREATE PROCEDURE [dbo].[Spaddcategory] @type    VARCHAR(30),
                               @remarks VARCHAR(8000),
                               @icon    IMAGE,
							   @chargeable bit,
							   @default smallint,
							   @Status bit
AS
	IF not exists( select 1 from category where Type=@type )
	Begin
	if(isnull(@default,0) = 1)
	begin
	update Category set ISDefault = 0  
	end
	
			INSERT INTO category
						(type,
						 remarks,
						 icon,
						 Chargeable,
						 ISDefault,
						 Status)
			VALUES      (@type,
						 @remarks,
						 @icon,
						 @chargeable,
						 @default,
						 @Status)
	End
	Else
	Begin
			RAISERROR ('Category already exists, please use different name !',16,1)
			RETURN
	End

	
