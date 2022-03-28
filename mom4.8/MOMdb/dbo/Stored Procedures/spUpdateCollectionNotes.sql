CREATE PROCEDURE spUpdateCollectionNotes
@ID INT,
@Notes VARCHAR(500)	,
@UpdatedBy VARCHAR (100) 
AS                  
BEGIN                 
                  
UPDATE CollectionNotes
SET Notes=@Notes
,UpdatedBy=@UpdatedBy
,UpdatedDate=GETDATE()
WHERE ID=@ID

END