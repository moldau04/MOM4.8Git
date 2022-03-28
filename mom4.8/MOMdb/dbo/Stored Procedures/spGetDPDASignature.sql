CREATE  proc [dbo].[spGetDPDASignature] 
 @Ticket int, @Workid int
as
declare @text as varchar(max)
set @text = '
IF EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = ''dbo''
                 AND  TABLE_NAME = ''PDA_'+convert(varchar(100), @Workid)+''')
BEGIN 
SELECT TOP 1 signature,pdaticketid FROM  PDA_'+convert(varchar(100), @Workid)+' WHERE  pdaticketid ='+convert(varchar(100), @Ticket)+'
 END
'
EXEC (@text)
