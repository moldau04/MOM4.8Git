CREATE PROCEDURE [dbo].[Log2_Insert] (
  @fUser varchar(50)
 ,@Screen varchar(50)
 ,@Ref bigint
 ,@Field varchar(75) = NULL
 ,@OldVal varchar(1000) = NULL
 ,@NewVal varchar(1000) = NULL
) AS
INSERT INTO [Log2] (
            [fUser]
           ,[Screen]
           ,[Ref]
           ,[Field]
           ,[OldVal]
           ,[NewVal])
    VALUES (@fUser
           ,@Screen
           ,@Ref
           ,@Field
           ,@OldVal
           ,@NewVal)