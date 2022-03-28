function SelectRow(id, row, chk, gridview, event) {
    var hdn = document.getElementById(id)
    var rowst = document.getElementById(row)
    var chks = document.getElementById(chk);

    if (hdn.value == '1') {
        if (event.target.type == 'checkbox') {
            hdn.value = '0';
            chks.checked = false;
            $(rowst).removeAttr('style');
        }
    }
    else {
        SelectChk(gridview);
        hdn.value = '1';
        chks.checked = true;
        rowst.style.background = '#F9BE7A';
    }
}

function SelectRowChk(row, chk, gridview, event) {
    var rowst = document.getElementById(row)
    var chks = document.getElementById(chk);

    if (event.target.type == 'checkbox') {
        if (chks.checked == false) {
            $(rowst).removeAttr('style');
        }
        else {

            SelectChk(gridview);
            rowst.style.background = '#F9BE7A';
            chks.checked = true;
        }
    }
    else {
        if (chks.checked == true) {
            //            chks.checked = false;
            //            $(rowst).removeAttr('style');
        }
        else {

            SelectChk(gridview);
            chks.checked = true;
            rowst.style.background = '#F9BE7A';
        }
    }
}

function SelectRowChk1(row, chk, gridview, event) {
    var rowst = document.getElementById(row)
    var chks = document.getElementById(chk);

    if (event.target.type == 'checkbox') {
        if (chks.checked == false) {
            $(rowst).removeAttr('style');
        }
        else {

            SelectChk1(gridview);
            rowst.style.background = '#F9BE7A';
            chks.checked = true;
        }
    }
    else {
        if (chks.checked == true) {
            chks.checked = false;
            $(rowst).removeAttr('style');
        }
        else {

            SelectChk1(gridview);
            chks.checked = true;
            rowst.style.background = '#F9BE7A';
        }
    }
}
function SelectChk1(gridview) {
    var grid = document.getElementById(gridview);
    var cell;
    if (grid.rows.length > 0) {
        for (i = 1; i < grid.rows.length; i++) {
            cell = grid.rows[i].cells[0];
            for (j = 0; j < cell.childNodes.length; j++) {

                if (cell.childNodes[j].type == "checkbox") {

                    if (cell.childNodes[j].checked == true) {
                        cell.childNodes[j].checked = false;
                        $(grid.rows[i]).removeAttr('style');
                    }
                }
                if (cell.childNodes[j].type == "hidden") {
                    if (cell.childNodes[j].value == '1') {
                        //cell.childNodes[j].value = '0';
                        $(grid.rows[i]).removeAttr('style');
                    }
                }
            }
        }
    }
}
function SelectRowFill(gridview, id, name, hdn, hdnnames, DIV) {
    var rowid = document.getElementById(id);
    var rowname = document.getElementById(name);
    var grid = document.getElementById(gridview);
    var hidden = document.getElementById(hdn);
    var hdnName = document.getElementById(hdnnames);

    hdnName.value = rowname.innerHTML;
    hidden.value = rowid.innerHTML;

    $("#" + DIV).slideUp();
}

function SelectRows(gridview, names, hiddens) {
    var grid = document.getElementById(gridview);
    var Name = document.getElementById(names);
    var Hidden = document.getElementById(hiddens);
    var cell;
    var cell1;
    Name.value = '';
    if (grid.rows.length > 0) {
        for (i = 1; i < grid.rows.length; i++) {
            cell = grid.rows[i].cells[0];
            cell1 = grid.rows[i].cells[1];
            if (cell.childNodes[3].checked == true) {
                if (Name.value != '') {
                    Name.value = Name.value + ', ' + cell1.childNodes[1].innerHTML;
                }
                else {
                    Name.value = cell1.childNodes[1].innerHTML;
                }
            }
        }
    }
}

function SelectChk(gridview) {
    var grid = document.getElementById(gridview);
    var cell;
    if (grid.rows.length > 0) {
        for (i = 1; i < grid.rows.length; i++) {
            cell = grid.rows[i].cells[0];
            for (j = 0; j < cell.childNodes.length; j++) {

                if (cell.childNodes[j].type == "checkbox") {

                    if (cell.childNodes[j].checked == true) {
                        cell.childNodes[j].checked = false;
                        $(grid.rows[i]).removeAttr('style');
                    }
                }
                if (cell.childNodes[j].type == "hidden") {
                    if (cell.childNodes[j].value == '1') {
                        cell.childNodes[j].value = '0';
                        $(grid.rows[i]).removeAttr('style');
                    }
                }
            }
        }
    }
}

function SelectedRowStyle(gridview) {
    var grid = document.getElementById(gridview);
    var cell;
    if (grid != null) {
        if (grid.rows.length > 0) {
            for (i = 1; i < grid.rows.length; i++) {
                cell = grid.rows[i].cells[0];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        if (cell.childNodes[j].checked == true) {
                            grid.rows[i].style.background = '#F9BE7A';
                        }
                    }
                }
            }
        }
    }
}

function SelectedRowDelete(gridview, message) {
    var grid = document.getElementById(gridview);
    var cell;
    var cellName;
    if (grid.rows.length > 0) {
        for (i = 1; i < grid.rows.length; i++) {
            cell = grid.rows[i].cells[0];
            cellName = grid.rows[i].cells[1];
            for (j = 0; j < cell.childNodes.length; j++) {
                if (cell.childNodes[j].type == "checkbox") {
                    if (cell.childNodes[j].checked == true) {
                        return confirm('Are you sure you want to delete ' + message + ' "' + cellName.children[0].innerHTML + '" ?');

                    }
                }
            }
        }
    }
    alert('Please select ' + message + '.');
    return false;
}


function SelectRowmail(id, row, mail, chk, gridview, lnkMail) {

    var hdn = document.getElementById(id);
    var rowst = document.getElementById(row);
    var maillink = document.getElementById(lnkMail);
    var mailid = document.getElementById(mail);
    var chks = document.getElementById(chk);

    if (hdn.value == '1') {
        hdn.value = '0';
        chks.checked = false;
        $(rowst).removeAttr('style');
        $(maillink).removeAttr('href');
    }
    else {
        SelectChk(gridview);
        hdn.value = '1';
        chks.checked = true;
        rowst.style.background = '#F9BE7A';
        maillink.href = 'mailto:' + mailid.innerHTML;
    }
}

function clickEdit(id, chk, btnedit) {
    var editbtn = document.getElementById(btnedit);
    var hdn = document.getElementById(id);
    var chks = document.getElementById(chk);
    hdn.value = '1';
    chks.checked = true;
    editbtn.click();
}


function CheckAddRow(gridview, rowIndex, button) {
    var grid = document.getElementById(gridview);
    var button = document.getElementById(button);
    var rows = grid.rows.length - 2;
    var index = parseInt(rowIndex) + 1;
    if (rows.toString() == index.toString()) {
        button.click();
    }
}

function AutoCompleteText(URL, targetControl, valueID, clickPostback, Custom1, Custom2) {
    function parameters() {
        this.prefixText = null;
        this.con = null;
        this.custID = null;
    }
    $("#" + targetControl).autocomplete(
        {
            source: function (request, response) {
                var objParameters = new parameters();
                objParameters.prefixText = request.term;
                objParameters.custID = 0;
                query = request.term;

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: URL,
                    data: JSON.stringify(objParameters),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        response($.parseJSON(data.d));
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        var err = eval("(" + XMLHttpRequest.responseText + ")");
                        alert(err.Message);
                    }
                });
            },
            select: function (event, ui) {
                $("#" + targetControl).val(ui.item.label);
                $("#" + valueID).val(ui.item.value);

                if (Custom1 != null) {
                    $("#" + Custom1).val(ui.item.type);
                }

                if (Custom2 != null) {
                    $("#" + Custom2).val(ui.item.Custom2);
                }

                if (clickPostback != null) {
                    document.getElementById(clickPostback).click();
                }

                return false;
            },
            focus: function (event, ui) {
                //$("#" + targetControl).val(ui.item.label);
                return false;
            },
            minLength: 0,
            delay: 250
        })
    .data("autocomplete")._renderItem = function (ul, item) {

        var result_item = item.label;
        var result_desc = item.desc;
        var result_type = item.type;
        var result_Prospect = item.prospect;
        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
        result_item = result_item.replace(x, function (FullMatch, n) {
            return '<span class="highlight">' + FullMatch + '</span>'
        });
        if (result_desc != null) {
            result_desc = result_desc.replace(x, function (FullMatch, n) {
                return '<span class="highlight">' + FullMatch + '</span>'
            });
        }
        var color = 'gray';
        if (result_Prospect == 0) {
            color = 'Black';
        }
        else if (result_Prospect == 1) {
            color = 'green';
        }
        else if (result_Prospect == 2) {
            color = 'blue';
        }
        else if (result_Prospect == 3) {
            color = 'brown';
        }
        else if (result_Prospect == 4) {
            color = 'orange';
        }
        else if (result_Prospect == 5) {
            color = 'purple';
        }

        return $("<li></li>")
        .data("item.autocomplete", item)
        .append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'><i>[" + result_type + "]</i>, " + result_desc + "</span></a>")
        .appendTo(ul);
    };
}
function SelectedRowVoid(gridview, message) {
    var grid = document.getElementById(gridview);
    var cell;
    var cellName;
    if (grid.rows.length > 0) {
        for (i = 1; i < grid.rows.length; i++) {
            cell = grid.rows[i].cells[0];
            cellName = grid.rows[i].cells[1];
            for (j = 0; j < cell.childNodes.length; j++) {
                if (cell.childNodes[j].type == "checkbox") {
                    if (cell.childNodes[j].checked == true) {
                        return confirm('Are you sure you want to void ' + message + ' "' + cellName.children[0].innerHTML + '" ?');

                    }
                }
            }
        }
    }
    alert('Please select ' + message + '.');
    return false;
}
