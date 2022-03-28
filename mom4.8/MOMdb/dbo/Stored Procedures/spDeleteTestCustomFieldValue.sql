Create  PROCEDURE [dbo].[spDeleteTestCustomFieldValue]			
         @TestID INT
         ,@EquimentID INT	
as
	begin
		DELETE FROM tblTestCustomFieldsValue
		WHERE TestID=@TestID AND EquipmentID=@EquimentID

	end
