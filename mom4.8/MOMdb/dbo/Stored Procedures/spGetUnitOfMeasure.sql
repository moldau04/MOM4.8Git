



CREATE PROCEDURE [dbo].[spGetUnitOfMeasure]
@ID INT= NULL,
@UnitOfMeasureCode VARCHAR(12)= NULL,
@UnitOfMeasureDesc VARCHAR(75)= NULL

 

as
	begin
			IF @ID IS NULL AND @UnitOfMeasureCode IS NULL AND @UnitOfMeasureDesc IS NULL
			SELECT UnitOfMeasure.ID,UnitOfMeasure.UnitOfMeasureCode,UnitOfMeasure.UnitOfMeasureDesc
			FROM UnitOfMeasure

			IF @ID IS NOT NULL AND @UnitOfMeasureCode IS NULL AND @UnitOfMeasureDesc IS NULL
			SELECT UnitOfMeasure.ID,UnitOfMeasure.UnitOfMeasureCode,UnitOfMeasure.UnitOfMeasureDesc
			FROM UnitOfMeasure WHERE ID=@ID
		
		IF @ID IS  NULL AND @UnitOfMeasureCode IS NOT NULL AND @UnitOfMeasureDesc IS NULL
			SELECT UnitOfMeasure.ID,UnitOfMeasure.UnitOfMeasureCode,UnitOfMeasure.UnitOfMeasureDesc
			FROM UnitOfMeasure WHERE UnitOfMeasureCode=@UnitOfMeasureCode

			IF @ID IS  NULL AND @UnitOfMeasureCode IS NULL AND @UnitOfMeasureDesc IS NOT NULL
			SELECT UnitOfMeasure.ID,UnitOfMeasure.UnitOfMeasureCode,UnitOfMeasure.UnitOfMeasureDesc
			FROM UnitOfMeasure WHERE UnitOfMeasureDesc=@UnitOfMeasureDesc

	end
