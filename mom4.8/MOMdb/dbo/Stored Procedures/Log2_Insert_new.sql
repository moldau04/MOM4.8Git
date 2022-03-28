CREATE PROCEDURE [dbo].[Log2_Insert_new] (
  @fUser varchar(50)
 ,@Screen varchar(50)
 ,@Ref int
 ,@Field varchar(75) = NULL
 ,@Val varchar(1000) = NULL
) AS

DECLARE @OldVal varchar(1000) = NULL
SELECT TOP 1 @OldVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @Ref and Field=@Field ORDER BY CreatedStamp DESC

IF(ISNULL(@OldVal,'') != ISNULL(@Val,''))
BEGIN
	EXEC log2_insert @fUser,@Screen,@Ref,@Field,@OldVal,@Val
END