CREATE PROCEDURE spDeleteCollectionNote
@noteId INT  
AS                  
BEGIN                
 DELETE FROM CollectionNotes WHERE ID=@noteId
END