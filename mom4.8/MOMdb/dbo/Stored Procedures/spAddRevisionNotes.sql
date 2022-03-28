CREATE PROCEDURE [dbo].[spAddRevisionNotes]
	@EstimateID INT,
	@Notes Varchar(3000),
	@Version Varchar(100),
	@CreatedDate Datetime,
	@CreatedBy Varchar(100)
AS
BEGIN
	INSERT INTO [dbo].[EstimateRevisionNotes]
           ([Notes]
           ,[Version]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[EstimateID])
     VALUES
           (@Notes
           ,@Version
           ,@CreatedDate
           ,@CreatedBy
           ,@EstimateID)
END
