CREATE PROCEDURE [dbo].[spAddCollectionNotes]
	@OwnerID INT,
	@Notes Varchar(3000),
	@CreatedDate Datetime,
	@CreatedBy Varchar(100),
	@DefaultNotes Varchar(3000),
	 @locID int 
AS
BEGIN

IF @locID<>0
BEGIN	
	IF @Notes !=''
	BEGIN
			INSERT INTO [dbo].[CollectionNotes]
           ([Notes]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[OwnerID],locID)
     VALUES
           (@Notes
           ,@CreatedDate
           ,@CreatedBy
           ,@OwnerID,@LocID)
    END  
END
ELSE
BEGIN
	IF @Notes !=''
	BEGIN
			INSERT INTO [dbo].[CollectionNotes]
           ([Notes]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[OwnerID])
     VALUES
           (@Notes
           ,@CreatedDate
           ,@CreatedBy
           ,@OwnerID)
    END  
END

	  

 Declare @OldValue varchar(8000)
 set @OldValue=  (select ISNULL(CNotes,'') from Owner where ID=@OwnerID)


		 
	IF @DefaultNotes !=''
	BEGIN
		UPDATE Owner
		SET CNotes=@DefaultNotes
		WHERE ID= @OwnerID

		If  @OldValue != @DefaultNotes
		BEGIN
		  EXEC log2_insert @CreatedBy,'iCollections Popup',@OwnerID,'Default Notes',@OldValue,@DefaultNotes
		END
    END 
END