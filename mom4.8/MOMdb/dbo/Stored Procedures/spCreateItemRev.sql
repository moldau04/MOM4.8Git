CREATE PROCEDURE [dbo].[spCreateItemRev]

	@Date datetime,
	@Version  Varchar(200),
	@Comment Varchar(8000),
	@Eco Varchar(250),
	@Drawing Varchar(250),
	@InvID int
	
AS
BEGIN

	
				BEGIN
					INSERT INTO [dbo].[ItemRev]
				   (
				   [Date]
				   ,[Version]
				   ,[Comment]
				   ,[InvID]
				   ,[Eco]
				   ,[Drawing]
				   )
					 VALUES
				   (@Date,@Version,@Comment,@InvID,@Eco,@Drawing)
			   END
			
		

		
END
GO

