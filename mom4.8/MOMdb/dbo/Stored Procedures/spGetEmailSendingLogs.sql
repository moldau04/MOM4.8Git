CREATE PROCEDURE [dbo].[spGetEmailSendingLogs]
	@Screen varchar(50) = '',
	@Function varchar(50) = ''
AS
BEGIN

	IF(@Screen is null OR @Screen = '')
	BEGIN
		SELECT 
				Username,
				EmailDate,
				Case  
					when isnull([Status],0) = 0 then 'Failed'
					when isnull([Status],0) = 1 then 'Sent successfully'
					when isnull([Status],0) = 2 then 'Get Sent Items failed'
				End as [Status],
				Case
					when isnull([Status],0) = 2 then 0
					else Ref
				End as Ref,
				[Function] as EmailFunction,
				SessionNo,
				IsNull(UsrErrMessage,'') as UsrErrMessage,
				IsNull(SysErrMessage,'') as SysErrMessage
				, [To] as MailTo
		FROM		tblEmailSendingLog 
		ORDER BY EmailDate desc
	END
	ELSE
	BEGIN
		IF(@Function is null OR @Function = '')
		BEGIN
			SELECT 
					Username,
					EmailDate,
					Case  
						when isnull([Status],0) = 0 then 'Failed'
						when isnull([Status],0) = 1 then 'Sent successfully'
						when isnull([Status],0) = 2 then 'Get Sent Items failed'
					End as [Status],
					Case
						when isnull([Status],0) = 2 then 0
						else Ref
					End as Ref,
					[Function] as EmailFunction,
					SessionNo,
					IsNull(UsrErrMessage,'') as UsrErrMessage,
					IsNull(SysErrMessage,'') as SysErrMessage
					, [To] as MailTo
			FROM		tblEmailSendingLog 
			WHERE		Screen = @Screen 
			ORDER BY EmailDate desc
		END
		ELSE
		BEGIN
			SELECT 
					Username,
					EmailDate,
					Case  
						when isnull([Status],0) = 0 then 'Failed'
						when isnull([Status],0) = 1 then 'Sent successfully'
						when isnull([Status],0) = 2 then 'Get Sent Items failed'
					End as [Status],
					Case
						when isnull([Status],0) = 2 then 0
						else Ref
					End as Ref,
					[Function] as EmailFunction,
					SessionNo,
					IsNull(UsrErrMessage,'') as UsrErrMessage,
					IsNull(SysErrMessage,'') as SysErrMessage
					, [To] as MailTo
			FROM		tblEmailSendingLog 
			WHERE		Screen = @Screen AND [Function] = @Function
			ORDER BY EmailDate desc	
		END
	END
END
