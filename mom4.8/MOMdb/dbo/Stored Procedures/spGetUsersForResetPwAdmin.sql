CREATE PROCEDURE [dbo].[spGetUsersForResetPwAdmin]
AS
SELECT  --e.ID
	--, LTRIM(RTRIM(e.fFirst)) as fFirst
	--, LTRIM(RTRIM(e.Last)) as lLast
	u.ID as userid
	, fUser
	--,u.Status
	--,w.super
	, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
	, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
	, r.EMail
	, m.OutUsername SenderAddress
FROM tblUser u 
LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
--LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
LEFT OUTER JOIN tblEmailAccounts m on m.UserId = u.ID
LEFT OUTER JOIN Rol r on e.Rol=r.ID 
WHERE u.Status = 0 --active
and u.EmailAccount = 1
and Substring(u.UserS, 1, 1) = 'y'

ORDER BY fUser
