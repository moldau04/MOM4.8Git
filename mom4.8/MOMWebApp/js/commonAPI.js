var AccessControler = {
    Get: function (jsondata, timeOut, methodName, callback) {
        $.ajax({
            type: "Get",
            data: jsondata,
            dataType: 'Json',
            timeout: timeOut,
            contentType: "application/json",
            url: methodName,
            success: function (response) {
                callback(response);
            }, error: function (result, status, error) {
                alert(result.responseText);
            },
            failure: function (result, status, error) {
                alert(result.responseText);
            }
        });
    },
    Post: function (jsondata, timeOut, methodName, callback) {
        $.ajax({
            type: "Post",
            data: jsondata,
            dataType: 'Json',
            timeout: timeOut,
            url: methodName,
            success: function (response) {
                callback(response);
            }, error: function (result, status, error) {
                alert(result.responseText);
            },
            failure: function (result, status, error) {
                alert(result.responseText);
            }
        });
    },
}