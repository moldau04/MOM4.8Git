CREATE PROCEDURE [dbo].[spCreateDB]
@DbName varchar(50)
as
declare @Text varchar(max)
declare @rc int, @dir nvarchar(4000) 

exec @rc = master.dbo.xp_instance_regread
      N'HKEY_LOCAL_MACHINE',
      N'Software\Microsoft\MSSQLServer\Setup',
      N'SQLPath', 
      @dir output, 'no_output'

set @Text='
CREATE DATABASE ['+@DbName+'] ON  PRIMARY 
( NAME = N'''+@DbName+''', FILENAME = N'''+@dir+'\DATA\'+@DbName+'.mdf'' , SIZE = 12288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'''+@DbName+'_log'', FILENAME = N'''+@dir+'\DATA\'+@DbName+'.ldf'' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
'

exec (@Text)
