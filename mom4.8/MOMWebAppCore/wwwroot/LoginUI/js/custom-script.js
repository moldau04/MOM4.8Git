/*================================================================================
	Item Name: Materialize - Material Design Admin Template
	Version: 3.1
	Author: GeeksLabs
	Author URL: http://www.themeforest.net/user/geekslabs
================================================================================

NOTE:
------
PLACE HERE YOUR OWN JS CODES AND IF NEEDED.
WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR CUSTOM SCRIPT IT'S BETTER LIKE THIS. */
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

function SelectRowsEq(gridview, names, hiddens) {
    var grid = document.getElementById(gridview);
    var Name = document.getElementById(names);
    var Hidden = document.getElementById(hiddens);
    var cell;
    var cell1;
    Name.value = '';
    if (grid.rows.length > 0) {
        document.getElementById("eqtag").innerHTML = "";
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
                AddTags(cell1.childNodes[1].innerHTML);
            }
        }
    }
}

function AddTags(tag) {
    var div = document.getElementById('eqtag');
    //var tag = "<div class='chip'>" + tag + "<i class='material-icons mdi-navigation-close'></i></div>"
    var tag = "<div class='chip'>" + tag + "</div>"
    div.innerHTML += tag;
}


//add the init activity
Sys.Application.add_init(appl_init);

//Do this on init
function appl_init() {
    var pagegReqManager = Sys.WebForms.PageRequestManager.getInstance();
    pagegReqManager.add_endRequest(EndHandler);
}

//Called after async postback
function EndHandler() {
    CustomChecks();
}


function CustomChecks() {
    $(".css-checkbox input:first-child").addClass('css-checkbox');
    $(".css-checkbox label").addClass('css-label');
}

$(document).ready(function () {
    CustomChecks();
});





function SelectRows_Telerik(gridview, names, hiddens) {
    var grid = $find(gridview);
    var Name = document.getElementById(names);
    var Hidden = document.getElementById(hiddens);
    var cell;
    var cell1;
    Name.value = '';
    if (grid.get_masterTableView().get_dataItems().length > 0) {
        for (i = 0; i < grid.get_masterTableView().get_dataItems().length; i++) {
            cell = grid.get_masterTableView().get_dataItems()[i]._element.cells[0];
            cell1 = grid.get_masterTableView().get_dataItems()[i]._element.cells[1];
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