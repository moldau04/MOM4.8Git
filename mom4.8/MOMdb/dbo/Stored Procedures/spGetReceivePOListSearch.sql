CREATE proc [dbo].[spGetReceivePOListSearch]
@Vendor int,
@PO int,
@SearchText varchar(50)
as

declare @WOspacialchars varchar(50) 
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
declare @querystr varchar(MAX);
SET @querystr = 'SELECT r.ID, CAST(r.ID as varchar(40)) as Value, r.Amount as ReceivedAmount,CONVERT(varchar, r.fDate , 101) As ReceiveDate, r.Ref
                        ,v.Type VendorType
                    FROM ReceivePO As r INNER JOIN PO AS p ON p.PO = r.PO INNER JOIN Vendor v ON v.ID = p.Vendor
                    WHERE isnull(r.Status,0) <> 1 '

IF(@Vendor > 0)
BEGIN 
    SET @querystr +=  ' AND p.Vendor = ''' + Convert(varchar(50),@Vendor) + ''''
END
IF(@PO > 0)
BEGIN 
    SET @querystr +=  ' AND r.PO = ''' + Convert(varchar(50),@PO) + ''''
END
IF(@WOspacialchars is not null AND @WOspacialchars != '')
BEGIN
    SET @querystr += ' AND (r.ID like ''%' +@WOspacialchars+ '%'' OR r.Ref like ''%' + @WOspacialchars + '%'') ' 
END

SET @querystr +=  ' ORDER BY r.ID'
EXEC (@querystr)