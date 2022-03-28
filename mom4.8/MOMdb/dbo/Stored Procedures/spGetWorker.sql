Create proc spgetworker
@searchby varchar(50),
@value varchar(50)
as
select fDesc, id from tblWork where Status= 0
and fdesc like '%'+ @value +'%'