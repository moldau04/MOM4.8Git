var content_one = '';
function replaceQuickCodes(e, cont_id, con) {

    if (e.keyCode != 191) {
        return;
    }

    var content = $("#" + cont_id).val();
    var content_arr = content.split("//");

    if (content_arr.length > 1) {
        for (var i = 0; i < content_arr.length; i++) {

            content_arr[i] = content_arr[i].replace(/\r\n|\n/g, " ");
            var temp_arr = content_arr[i].split(" ");
            content_arr[i] = temp_arr[temp_arr.length - 1];

            if (content_arr[i] != "") {

                content_one = content_arr[i];
                function dtaa() {
                    this.code = content_arr[i];
                    this.con = con;
                }

                var dtaaa = new dtaa();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "QuickCodesService.asmx/QuickCodes",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function(data) {
                        var result = $.parseJSON(data.d);
                        if (result != '') {
                            var content = $("#" + cont_id).val();
                            var search_tm = new RegExp(content_one + "//", 'i');
                            content = content.replace(search_tm, result + " ");
                            $("#" + cont_id).val(content);
                        }
                    },
                    error: function(result) {
                        alert("error occured while getting code.");
                    }
                    //                            error: function(XMLHttpRequest, textStatus, errorThrown) {
                    //                                var err = eval("(" + XMLHttpRequest.responseText + ")");
                    //                                alert(err.Message);
                    //                            }
                });
            }
        }
    }
    else {
        return;
    }
}       