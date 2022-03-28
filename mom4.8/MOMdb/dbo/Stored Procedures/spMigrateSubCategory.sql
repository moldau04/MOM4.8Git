-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spMigrateSubCategory] 
	-- Add the parameters for the stored procedure here
	@SortOrder     INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @SortOrder IS NULL SELECT @SortOrder=ISNULL(MAX(SortOrder),0)+1 FROM SubCat
    -- Insert statements for procedure here
declare @tempMigrateSubCat table
(
tId int IDENTITY(1,1),
[CType] [int] NOT NULL,
[SubType] [varchar] NULL,
[SortOrder] [smallint] NULL
)

--insert into @tempMigrateSubCat (CType, [SubType],[SortOrder])
--(SELECT DISTINCT  [Type], [Sub], @SortOrder
--FROM [dbo].[Chart]
--WHERE [Sub] <> ''
--)

insert into tempSubCat (CType, [SubType],[SortOrder])
(SELECT DISTINCT  [Type], [Sub], @SortOrder
FROM [dbo].[Chart]
WHERE [Sub] <> ''
)

END
