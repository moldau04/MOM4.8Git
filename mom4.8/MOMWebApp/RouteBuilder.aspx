<%@ Page Title="Route Builder || MOM" Language="C#"
    MasterPageFile="Mom.master"
    AutoEventWireup="true"
    Inherits="RouteBuilder"
    ValidateRequest="false"
    EnableEventValidation="false"
    CodeBehind="RouteBuilder.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="Design/css/grid.css" rel="stylesheet" />

    <style>
        .RadGrid a {
            padding: initial;
        }

        .tabs .active {
            font-weight: bold;
        }

        .btnlinks a[disabled='disabled'] {
            color: grey;
            pointer-events:none;
        }
    .work-css {
        width: 100%!important;
    }
    </style>



    <style>
        .context_menu_item {
            padding: 3px 6px;
            background-color: whitesmoke;
            border: 1px solid black;
        }
        .btncontainer {
             min-height: 17px;
            margin-bottom: 10px;
        }
            .context_menu_item:hover {
                background-color: #CCCCCC;
                font-weight: 700;
            }

        .context_menu_separator {
            background-color: gray;
            height: 1px;
            margin: 0.5px;
            padding: 0.5px;
        }
         .excel-ex-css {
             max-height: 313px;
         }
         .excel-ex-css .rgDataDiv{
             max-height:180px;
         }
         .basi-class .rgDataDiv{
             max-height:180px;
         }
    </style>

    <script type="text/javascript" src="js/ContextMenu.js"></script>
    <script src="js/maps.google.polygon.containsLatLng.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/markerwithlabel_packed.js"></script>
    <script type="text/javascript">

        var GooglemarkersArray = [];
        var GooglemarkersArrayAssigned = [];
        var markerselected = new Array();
        var map;
        var coord = new Array();
        var directionsDisplay;
        var directionsService = new google.maps.DirectionsService();
        directionsDisplay = new google.maps.DirectionsRenderer();
        var cityCircle;
        var contextMenu;
        var contextMenuCircle;
        var infoWindow;
        var bounds;
        var GooglePolygonsArray = new Array();
        var polycount = 0;
        var IsPolygonEdited = 0;

        function initialize() {
            cityCircle = new google.maps.Circle();
            GooglemarkersArray = [];
            GooglePolygonsArray = [];
            markerselected = new Array();
            coord = new Array();
            infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });
            bounds = new google.maps.LatLngBounds();

            var mapOptions = {
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: new google.maps.LatLng(0, 0),
                scrollwheel: true
            }
            map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            AddMarker(markers, 1, 'NONE');

            // alert(markers.length);

            AddContextMenu();
        }

        google.maps.event.addDomListener(window, 'load', initialize);


        function AddMarker(markers, boundmap, animation) {

            for (var i = 0; i < markers.length; i++) {
                AddSingleMarker(markers[i], infoWindow, bounds);
            }
            if (boundmap == 1) {
                map.fitBounds(bounds);
            }
        }

        function AddSingleMarker(markeradd) {
            var data = markeradd;
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title,
                icon: "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=|" + GetColor(data.worker) + "|ffffff"
                //                    ,animation: google.maps.Animation.DROP
            });
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    infoWindow.setContent('<div STYLE=width:200px; height:150px><DIV  width:200px; height:150px><a style="cursor:pointer;" onclick=" openAssignMarkerPopup(' + data.loc + ');"><i>' + data.worker + '</i></a></br><b>' + data.title + '</b></br>' + data.description + '</DIV></DIV>');
                    infoWindow.open(map, marker);
                });

            })(marker, data);
            GooglemarkersArray[data.loc] = marker;
            bounds.extend(myLatlng);
        }

        function openAssignMarkerPopup(loc) {
            $("#<%=hdnMarkerLoc.ClientID%>").val(loc);
            $find('PMPMarker').show();
        }

        function AddAssignedMarker() {
            GooglemarkersArrayAssigned = [];
            var markerValues = document.getElementById('<%=hdnAssignedMarkers.ClientID%>').value;
            if (markerValues == '') {
                return;
            }
            var markers = JSON.parse(markerValues);

            var infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });
            for (var i = 0; i < markers.length; i++) {
                var data = markers[i];
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    animation: google.maps.Animation.DROP,
                    position: myLatlng,
                    map: map,
                    title: data.title,
                    optimized: false,
                    icon: "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=T|" + GetColor(data.assdwrkr) + "|000"
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent('<div STYLE=width:200px; height:150px><DIV  width:200px; height:150px>Newly assigned worker: <i>' + data.assdwrkr + '</i></br><b>' + data.title + '</b></br>' + data.description + '</DIV></DIV>');
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
                GooglemarkersArrayAssigned.push(marker);
            }
        }

        function AddLocationMarker() {
            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            var marker;

            var loc = $("#<%=hdnMarkerLoc.ClientID%>").val();
            deleteMarker(GooglemarkersArray[loc]);

            $(markers).each(function (ind) {
                if (this["loc"] == loc) {
                    marker = this;
                }
            });
            AddSingleMarker(marker);
        }

        function AddAllMarkers() {
            deleteAllMarkers();
            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            AddMarker(markers, 0, 'NONE');
        }


        function deleteAllMarkers() {
            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            $.each(markers, function (index) {
                if (typeof GooglemarkersArray[this["loc"]] != "undefined") {
                    deleteMarker(GooglemarkersArray[this["loc"]]);
                }
            });
            GooglemarkersArray = [];
        }

        function deleteMarker(marker) {
            marker.setMap(null);
        }

        function clearOverlays(markersArray) {
            for (var i = 0; i < markersArray.length; i++) {
                markersArray[i].setMap(null);
            }
            markersArray = [];
        }

        function ClearPolygons() {
            $.each(GooglePolygonsArray, function (index) {
                this.setMap(null);
            });
            GooglePolygonsArray = [];
            polycount = 0;
            $('#<%=hdnLocsInPolygon.ClientID%>').val('');
            $('#<%=hdnPolyID.ClientID%>').val('');
            $('#<%=hdnAssignedWorker.ClientID%>').val('');
            bounds = new google.maps.LatLngBounds();
        }

        function GetColor(worker) {
            var color;
            workers = JSON.parse(document.getElementById('<%=hdnColor.ClientID%>').value);
            $(workers).each(function (ind) {
                if (this["worker"] == worker) {
                    color = this["color"];
                }
            });
            return color;
        }

        function GridHover(i) {
            google.maps.event.trigger(GooglemarkersArray[i], "click");
        }

        function DeleteTemplate() {
            var ddltemplate = $("#<%=ddlTemplates.ClientID%>").val();
            if (ddltemplate == "0" || ddltemplate == "-1") {
                alert('Please select template to delete.');
                return false;
            }
            else {
                return confirm('Are you sure you want to delete the selected template?');
            }
        }

        function AddContextMenu() {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItems = [];
            menuItems.push({ className: 'context_menu_item', eventName: 'add_polygon_here', label: 'Add Polygon Here' });
            menuItems.push({});
            //            menuItems.push({ className: 'context_menu_item', eventName: 'add_circle_here', label: 'Add Circle Here' });
            //            menuItems.push({});
            menuItems.push({ className: 'context_menu_item', eventName: 'center_map_click', label: 'Center map here' });
            contextMenuOptions.menuItems = menuItems;

            contextMenu = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(map, 'rightclick', function (mouseEvent) {
                contextMenu.show(mouseEvent.latLng);
            });

            eventRightClick(contextMenu);
        }

        function AddContextMenuCircle() {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItemsCircle = [];
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'clear_circle', label: 'Clear Circle' });
            menuItemsCircle.push({});
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'assign_worker', label: 'Assign Worker' });
            contextMenuOptions.menuItems = menuItemsCircle;

            contextMenuCircle = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(cityCircle, 'rightclick', function (mouseEvent) {
                contextMenuCircle.show(mouseEvent.latLng);
            });

            google.maps.event.addListener(cityCircle, 'click', function (mouseEvent) {
                contextMenuCircle.hide();
            });

            eventRightClick(contextMenuCircle);
        }

        function AddContextMenuPolygon(polygon) {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItemsPolygon = [];
            menuItemsPolygon.push({ className: 'context_menu_item', eventName: 'clear_polygon', label: 'Clear Polygon' });
            menuItemsPolygon.push({});
            menuItemsPolygon.push({ className: 'context_menu_item', eventName: 'assign_worker', label: 'Assign Worker' });
            menuItemsPolygon.push({});
            menuItemsPolygon.push({ className: 'context_menu_item', eventName: 'update_worker', label: 'Update' });
            contextMenuOptions.menuItems = menuItemsPolygon;

            var contextMenuPolygon = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(polygon, 'rightclick', function (mouseEvent) {
                contextMenuPolygon.show(mouseEvent.latLng);
                contextMenu.hide();
            });

            google.maps.event.addListener(polygon, 'click', function (mouseEvent) {
                contextMenuPolygon.hide();
                contextMenu.hide();
            });

            eventRightClick(contextMenuPolygon, polygon);
        }

        function eventRightClick(contextMenu, polygon) {
            google.maps.event.addListener(contextMenu, 'menu_item_selected', function (latLng, eventName) {
                switch (eventName) {
                    case 'add_circle_here':
                        var zoomLevel = map.getZoom();
                        var radius = 12000;
                        if (zoomLevel > 12) {
                            radius = 5000 / zoomLevel
                        }
                        AddCircle(latLng, radius);
                        break;
                    case 'center_map_click':
                        map.panTo(latLng);
                        break;
                    case 'clear_circle':
                        cityCircle.setMap(null);
                        break;
                    case 'clear_polygon':
                        //polygon.setMap(null);
                        ClearPolygon(polygon);
                        break;
                    case 'assign_worker':
                        var polygonpts = PointWithinPolygon(polygon);
                        if (polygonpts.length == 0) {
                            alert('No locations found.');
                        }
                        else {
                            $find('PMPBehaviour').show();
                            $("#Button2").unbind("click");
                            $('#Button2').click(function () {
                                assignClick(polygon, 0);
                            });
                        }
                        break;
                    case 'update_worker':
                        if (IsExistingPolygon(polygon) == 1) {
                            var polygonpts = PointWithinPolygon(polygon);
                            if (polygonpts.length == 0) {
                                alert('No locations found.');
                            }
                            else {
                                polygon.MarkerWithLabel.setVisible(false);
                                $("#<%=lstWorker.ClientID%>").val(polygon.msmrb_worker);
                                assignClick(polygon, 0);
                            }
                        }
                        break;
                    case 'add_polygon_here':
                        AddPolygon(latLng);
                        break;
                }
            });
        }

        function AddCircleLatLng() {

            var latLng = $('#<%=hdnCenter.ClientID%>').val();
            var radius = $('#<%=hdnRadius.ClientID%>').val();
            var lat = latLng.split(",")[0];
            var lng = latLng.split(",")[1];
            AddCircle(new google.maps.LatLng(lat, lng), parseFloat(radius));
            map.fitBounds(cityCircle.getBounds());
        }

        function AddCircle(latLng, radius) {
            $('#<%=hdnOverlay.ClientID%>').val("circle");
            cityCircle.setMap(null);

            var populationOptions = {
                strokeColor: '#000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FF0',
                fillOpacity: 0.35,
                map: map,
                center: latLng,
                radius: radius,
                draggable: true,
                editable: true
            };
            cityCircle = new google.maps.Circle(populationOptions);

            AddContextMenuCircle();
            //            map.fitBounds(cityCircle.getBounds());
        }

        function CreatePolygon(PointArray) {
            $('#<%=hdnOverlay.ClientID%>').val("polygon");
            cityCircle.setMap(null);

            var polygon = new google.maps.Polygon({
                paths: PointArray,
                strokeColor: '#000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FFF',
                editable: true,
                draggable: true,
                fillOpacity: 0.35
            });
            polygon.setMap(map);
            AddContextMenuPolygon(polygon);
            return polygon;
        }

        function AddPolygon(latlng) {
            var zoomLevel = map.getZoom();

            var lat = latlng.lat();
            var lon = latlng.lng();
            var latOffset = 0.5;
            var lonOffset = 0.5;
            if (zoomLevel > 9) {
                latOffset = 0.1 / zoomLevel;
                lonOffset = 0.1 / zoomLevel;
            }

            var v = [];
            v[0] = new google.maps.LatLng(lat, lon - lonOffset);
            v[1] = new google.maps.LatLng(lat + latOffset, lon);
            v[2] = new google.maps.LatLng(lat, lon + lonOffset);
            v[3] = new google.maps.LatLng(lat - latOffset, lon);
            //            v[4] = new google.maps.LatLng(lat, lon - lonOffset)
            CreatePolygon(v);
        }


        function ClearPolygon(polygon) {
            var existing = IsExistingPolygon(polygon);
            if (existing != 0) {
                assignClick(polygon, 1);
            }
            else {
                polygon.setMap(null);
            }
        }

        function IsExistingPolygon(polygon) {
            var existing = 0;
            $(GooglePolygonsArray).each(function (ind) {
                var polid = this["msmrb_id"];
                if (polid == polygon.msmrb_id) {
                    existing = 1;
                }
            });

            console.log("existing:" + existing);
            return existing;
        }

        function PolygonCoordsJson() {

            var arrayList = [];

            $(GooglePolygonsArray).each(function (ind) {
                var polid = this["msmrb_id"];
                var workerid = this["msmrb_worker"];
                var workername = this["msmrb_workername"];
                //var polyJSON = JSON.stringify(this.getPath().getArray());
                //var latlngArray = JSON.parse(polyJSON);
                var latlngArray = this.getPath().getArray();

                var newAr = [];
                newAr.push({ polid: polid, workerid: workerid, workername: workername });

                for (var i = 0; i < latlngArray.length; i++) {
                    var obj = new Object();


                    obj['lat'] = latlngArray[i].lat();
                    obj['lon'] = latlngArray[i].lng();

                    newAr.push(obj);
                }
                arrayList.push(newAr);
            });
            $('#<%=hdnPolygon.ClientID%>').val(JSON.stringify(arrayList));
        }

        function AddPolygonFromArray() {

            ClearPolygons();

            $('#view1').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');
            $('#view2').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');

            var poygonsarray = JSON.parse($('#<%=hdnPolygon.ClientID%>').val());
            var locs = [];
            var locsworker = { locations: [] };

            $(poygonsarray).each(function (index) {
                var latlngArray = this;
                var v = [];
                var polyid;
                var workerid;
                var workername;
                $(latlngArray).each(function (ind) {
                    if (ind == 0) {
                        polyid = this["polid"];
                        workerid = this["workerid"];
                        workername = this["workername"];
                        $("#<%=lstWorker.ClientID%>").val(workerid);
                        //$('#<%=lstWorker.ClientID%> option:select').val(workerid);
                    }
                    else if (ind != 0) {
                        var latlng = new google.maps.LatLng(this["lat"], this["lon"]);
                        v[ind - 1] = latlng;
                        bounds.extend(latlng);
                    }
                });
                polycount++;
                var polygon = CreatePolygon(v);
                polygon.setOptions({ fillColor: '#' + GetColor(workername) });
                attachPolygonInfoWindow(polygon, workername);
                polygon.msmrb_id = polycount;
                polygon.msmrb_worker = workerid;
                polygon.msmrb_workername = workername;
                GooglePolygonsArray.push(polygon);

                var locsarray = PointWithinPolygon(polygon);
                $(locsarray).each(function (locindex) {
                    locs.push(this);
                    locsworker.locations.push({ locid: this, workerid: workerid, workername: workername, polid: polycount });
                });
            });
            map.fitBounds(bounds);
            $('#<%=hdnAssignedWorker.ClientID%>').val(JSON.stringify(locsworker));
            $('#<%=hdnLocsInPolygon.ClientID%>').val(locs);
            $('#<%=btnAssignMethodCall.ClientID%>').click();
        }


        function UpdateLocationPolygonArray() {
            $('#<%=hdnAssignedWorker.ClientID%>').val('');
            $('#<%=hdnLocsInPolygon.ClientID%>').val('');
            $('#view1').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');
            $('#view2').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');

            polycount = 0;

            var locs = [];
            var locsworker = { locations: [] };

            $(GooglePolygonsArray).each(function (index) {
                var latlngArray = this;
                var polyid;
                var workerid;
                var workername;
                var polygon = this;
                polycount = polygon.msmrb_id;
                workerid = polygon.msmrb_worker;
                workername = polygon.msmrb_workername;

                var locsarray = PointWithinPolygon(polygon);
                $(locsarray).each(function (locindex) {
                    locs.push(this);
                    locsworker.locations.push({ locid: this, workerid: workerid, workername: workername, polid: polycount });
                });
            });
            $('#<%=hdnAssignedWorker.ClientID%>').val(JSON.stringify(locsworker));
            $('#<%=hdnLocsInPolygon.ClientID%>').val(locs);
            $('#<%=btnAssignMethodCall.ClientID%>').click();
        }

        function assignClick(polygon, remove) {
            var overlay = $('#<%=hdnOverlay.ClientID%>').val();
            $('#<%=hdnLocsInPolygon.ClientID%>').val('');
            $('#<%=hdnPolyID.ClientID%>').val('');
            if (overlay == "polygon") {
                if (remove == 0) {

                    $('#<%=hdnLocsInPolygon.ClientID%>').val(PointWithinPolygon(polygon));
                    var lstw = $("#<%=lstWorker.ClientID%> option:selected").text();
                    polygon.setOptions({ fillColor: '#' + GetColor(lstw) });
                    polygon.setOptions({ strokeColor: '#000' });

                    attachPolygonInfoWindow(polygon, lstw);
                }
                else {
                    $('#<%=hdnLocsInPolygon.ClientID%>').val('');
                }

                var ex = 0;
                if (polycount != 0) {
                    $(GooglePolygonsArray).each(function (ind) {
                        var polid = this["msmrb_id"];
                        if (polid == polygon.msmrb_id) {
                            ex = 1;
                            $('#<%=hdnPolyID.ClientID%>').val(polid);
                            polygon.msmrb_worker = $("#<%=lstWorker.ClientID%> option:selected").val();
                            polygon.msmrb_workername = $("#<%=lstWorker.ClientID%> option:selected").text();
                        }
                    });
                }
                if (polycount == 0 || ex == 0) {
                    polycount++;
                    $('#<%=hdnPolyID.ClientID%>').val(polycount);
                    polygon.msmrb_id = polycount;
                    polygon.msmrb_worker = $("#<%=lstWorker.ClientID%> option:selected").val();
                    polygon.msmrb_workername = $("#<%=lstWorker.ClientID%> option:selected").text();
                }
                if (ex == 0) {
                    GooglePolygonsArray.push(polygon);
                }
                if (remove == 1) {
                    polygon.MarkerWithLabel.setMap(null);
                    polygon.setMap(null);
                    var index = GooglePolygonsArray.indexOf(polygon);
                    if (index > -1) {
                        GooglePolygonsArray.splice(index, 1);

                    }
                }
            }
            else if (overlay == "circle") {
                $('#<%=hdnCenter.ClientID%>').val(cityCircle.getCenter().lat() + ',' + cityCircle.getCenter().lng());
                $('#<%=hdnRadius.ClientID%>').val((cityCircle.getRadius()));
            }

            $('#<%=btnAssign.ClientID%>').click();
        }

        function attachPolygonInfoWindow(polygon, polyLabel) {

            polygon.MarkerWithLabel = new MarkerWithLabel({
                position: new google.maps.LatLng(0, 0),
                draggable: false,
                raiseOnDrag: false,
                map: map,
                labelContent: polyLabel,
                labelAnchor: new google.maps.Point(30, 20),
                labelClass: "labels", // the CSS class for the label
                labelStyle: { opacity: 1.0 },
                icon: "http://placehold.it/1x1",
                visible: false
            });

            google.maps.event.addListener(polygon, "mousemove", function (event) {
                polygon.MarkerWithLabel.setPosition(event.latLng);
                polygon.MarkerWithLabel.setVisible(true);
            });
            google.maps.event.addListener(polygon, "mouseout", function (event) {
                polygon.MarkerWithLabel.setVisible(false);
            });


            google.maps.event.addListener(polygon.getPath(), 'set_at', function (index) {
                polygon.setOptions({ strokeColor: 'red' });

            });

            google.maps.event.addListener(polygon.getPath(), 'insert_at', function (index) {
                polygon.setOptions({ strokeColor: 'red' });

            });

            google.maps.event.addListener(polygon, "dragstart", function () {
                google.maps.event.clearListeners(polygon.getPath(), 'set_at');
            });

            google.maps.event.addListener(polygon, "dragend", function () {
                google.maps.event.addListener(polygon.getPath(), 'set_at', function (index) {
                    polygon.setOptions({ strokeColor: 'red' });

                });
                polygon.setOptions({ strokeColor: 'red' });

            });

        }

        function assignCall() {
            $('#view1').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');
            $('#view2').css('background', '#ffffff url(images/loader_wheel.gif) no-repeat center');

            $('#<%=btnAssignMethodCall.ClientID%>').click();
        }

        function PointWithinPolygon(polygon) {

            $('#<%=hdnLocsInPolygon.ClientID%>').val('');
            var locs = new Array();
            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            $(markers).each(function (ind) {
                if (polygon.containsLatLng(parseFloat(this["lat"]), parseFloat(this["lng"]))) {
                    locs.push(this["loc"]);
                }
            });

            return locs;
        }

        function validateSave() {

            var txtTemplate = $("#<%=txtTemplate.ClientID%>").val();
            if (txtTemplate == "") {
                alert('Please enter the template name.');
                return false;
            }
            else {
                var ret = confirm('Are you sure you want to save the template?');
                if (ret == true) {
                    PolygonCoordsJson();
                    return true;
                }
                else {
                    return false;
                }
            }

        }

        function ClickTemplateDropdown() {
            var hdnEdited = $("#<%=hdnEdited.ClientID%>").val();
            if (hdnEdited == "1") {
                if ($("#<%=hdnPreviousTempl.ClientID%>").val() == "0" || $("#<%=hdnPreviousTempl.ClientID%>").val() == "-1") {
                    return confirm('You have not saved the new template. Do you want to continue without saving and move to existing template?');
                }
                else {
                    return confirm('There are unsaved changes to the current template. Do you want to continue without saving?');
                }
            }
            else {
                return true;
            }
        }

        function DisplayConfirmation() {
            if (ClickTemplateDropdown() == true) {
                __doPostBack("<%=ddlTemplates.ClientID%>", '');
            }
            else {
                $("#<%=ddlTemplates.ClientID%>").val($('#<%= hdnPreviousTempl.ClientID%>').val());
            }
        }

        function ClickAssignCheck() {

        }

        function AssignedMarker() {
            if ($('#<%=chkMap.ClientID%>').is(":checked")) {
                AddAssignedMarker();
            }
            else {
                clearOverlays(GooglemarkersArrayAssigned);
            }
        }

        function sizeContent() {
            var contentHeight = $(".content").height();
            var newHeight = $("html").height() - ($("#footer").height() + $("#header_main").height()) - 50; // + "px";

            if (contentHeight < newHeight) {
                $(".content").css("height", newHeight + "px");
            }
        }

        function tabState(link) {
            $("#<%=tabState.ClientID%>").val(link);

            var select = $('#' + link);
            $active = $(select.attr('href'));

            $('.show-hide-item').each(function () {
                $(this).hide();
            });

            $('.tabs .tab-item').each(function () {
                $(this).removeClass('active');
            });
            $('#' + link).addClass('active');

            $active.show();
        }

        $(document).ready(sizeContent);

        $(window).resize(sizeContent);


        function UpdatehdnPreviousTempl() {
            $('#<%= hdnPreviousTempl.ClientID%>').val($('#<%= ddlTemplates.ClientID%>').val());
        }

        function OnLocationExportExcel() {
            document.getElementById('<%= lnkLocExportPostback.ClientID %>').click();
            return false;
        }

        function OnWorkerExportExcel() {
            document.getElementById('<%= lnkWorkerExportPostback.ClientID %>').click();
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-maps-pin-drop"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Route Builder</asp:Label></div>
                                    <div class="btnlinks"><a href="setup.aspx?rw=true" id="anAdd" runat="server">Setup</a></div>
                                    <div class="btnclosewrap">
                                        <a href="Home.aspx"><i class="mdi-content-clear"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAssignWorkerMarker">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="Panel1" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="Panel4" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="RadPanelBar1" LoadingPanelID="RadAjaxLoadingPanel2" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="gvLocations">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecLoc" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="gvWorkers">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="ddlWorker" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecWork" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecLoc" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSaveTemplate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="RadPanelBar1" LoadingPanelID="RadAjaxLoadingPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnClearClick">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="ddlWorker" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="chkNulls" LoadingPanelID="RadAjaxLoadingPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkNulls">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtSearchLoc" />
                    <telerik:AjaxUpdatedControl ControlID="ddlWorker" />
                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecLoc" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlWorker">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="chkNulls" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecLoc" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLocations" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalRecLoc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnMarkers" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAssign">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveTemplate" />
                    <telerik:AjaxUpdatedControl ControlID="chkMap" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="gvLocChanges" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="gvWorkerChanges" LoadingPanelID="RadAjaxLoadingPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnAssignMethodCall">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveTemplate" />

                    <telerik:AjaxUpdatedControl ControlID="gvLocChanges" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="gvWorkerChanges" LoadingPanelID="RadAjaxLoadingPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="container">
        <div class="row">

            <telerik:RadSplitter RenderMode="Auto" ID="RadSplitter1" Skin="Material" runat="server" Width="100%" Height="725px">
                <telerik:RadPane ID="LeftPane" runat="server" Width="22" Scrolling="none">
                    <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="22">
                        <telerik:RadSlidingPane ID="Pane1" runat="server" Width="700" MinWidth="700">

                            <input id="tabState" runat="server" type="hidden" value="tabLoc" />
                            <asp:HiddenField ID="hdnSwitchMethods" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRouteSeq" runat="server" />
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

                                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                        <script type="text/javascript">
                                            function pageLoad() {


                                            }
                                            var requestInitiator = null;
                                            var selectionStart = null;

                                            function requestStart(sender, args) {
                                                requestInitiator = document.activeElement.id;
                                                if (document.activeElement.tagName == "INPUT" && document.activeElement.type == 'text') {
                                                    selectionStart = document.activeElement.selectionStart;
                                                }
                                            }

                                            function responseEnd(sender, args) {
                                                var element = document.getElementById(requestInitiator);
                                                if (element && element.tagName == "INPUT" && element.type == 'text') {
                                                    element.focus();
                                                    element.selectionStart = selectionStart;
                                                }
                                            }

                                        </script>
                                    </telerik:RadCodeBlock>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                                        <asp:HiddenField ID="hdnMarkers" runat="server" />
                                        <asp:HiddenField ID="hdnAssignedMarkers" runat="server" />
                                        <asp:HiddenField ID="hdnColor" runat="server" />
                                        <telerik:RadPanelBar RenderMode="Auto" runat="server" ID="RadPanelBar1" Width="100%" ExpandMode="MultipleExpandedItems" CollapseAnimation-Duration="200" ExpandAnimation-Duration="200">
                                            <Items>
                                                <telerik:RadPanelItem Expanded="False">
                                                    <HeaderTemplate>
                                                        <div class="">

                                                            <span class="worker-name"><b>&nbsp; 
                                                                <asp:Label ID="Label2" runat="server">Locations</asp:Label></b></span>

                                                            <span class="title-check-text text-css">
                                                                <asp:Label ID="lblTotalRecLoc" runat="server"></asp:Label>
                                                                &nbsp;
                                                            </span>
                                                        </div>

                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <div class="form-content-wrap">
                                                            <div class="srchinputwrap">
                                                                <asp:DropDownList ID="ddlSuper" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged"
                                                                    CssClass="browser-default selectst selectsml" Visible="False">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <asp:DropDownList ID="ddlWorker" runat="server" ToolTip="Select default worker" AutoPostBack="true"
                                                                    Visible="true" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <asp:TextBox ID="txtSearchLoc" runat="server" CssClass="srchcstm" placeholder="Search"></asp:TextBox>
                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <div class="btnlinksicon">
                                                                    <asp:LinkButton ID="btnClear" runat="server" OnClick="btnClear_Click1"><i class="mdi-content-clear"></i></asp:LinkButton>
                                                                </div>

                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <div class="btnlinksicon">
                                                                    <asp:LinkButton ID="btnSearch"
                                                                        ToolTip="Search" OnClick="btnSearch_Click1" runat="server" CausesValidation="true"><i class="mdi-notification-sync"></i></asp:LinkButton>

                                                                </div>
                                                            </div>

                                                            <div class="col lblsz2 lblszfloat">
                                                                <div class="row">
                                                                    <span class="tro trost">

                                                                        <asp:CheckBox ID="chkNulls" runat="server" CssClass="filled-in" AutoPostBack="true" OnCheckedChanged="chkNulls_CheckedChanged"
                                                                            ToolTip="Show locations not having proper address." Text="Find Geo"></asp:CheckBox>

                                                                    </span>

                                                                </div>
                                                            </div>

                                                            <div class="grid_container mt-10">

                                                                <div class="RadGrid RadGrid_Material">

                                                                    <telerik:RadPersistenceManager ID="RadPersistence1" runat="server">
                                                                        <PersistenceSettings>
                                                                            <telerik:PersistenceSetting ControlID="gvLocations" />
                                                                            <telerik:PersistenceSetting ControlID="gvWorkers" />
                                                                            <telerik:PersistenceSetting ControlID="gvWorkerChanges" />
                                                                            <telerik:PersistenceSetting ControlID="gvLocChanges" />
                                                                        </PersistenceSettings>
                                                                    </telerik:RadPersistenceManager>
                                                                    <telerik:RadGrid RenderMode="Auto" ID="gvLocations" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                                        OnNeedDataSource="gvLocations_NeedDataSource"
                                                                        OnPreRender="gvLocations_PreRender"
                                                                        OnItemEvent="gvLocations_ItemEvent"
                                                                        OnItemCreated="gvLocations_ItemCreated" >
                                                                        <CommandItemStyle />
                                                                        <GroupingSettings CaseSensitive="false" />
                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                        </ClientSettings>
                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                            <Columns>
                                                                                <telerik:GridTemplateColumn ShowFilterIcon="false" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkSelect" Visible="false" runat="server" />
                                                                                        <asp:HiddenField ID="hdnCoordinate" runat="server" Value='<%# Bind("coordinates") %>' />
                                                                                        <asp:Label ID="lblLoc" CssClass="loc" Style="display: none" runat="server" Text='<%# Eval("loc") %>' />
                                                                                        <asp:Label ID="lblworker" CssClass="worker" Style="display: none" runat="server"
                                                                                            Text='<%# Eval("worker") %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="job" HeaderText="Contract#" SortExpression="job" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" HeaderStyle-Width="100" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="lnkContract" NavigateUrl='<%# Eval("job","AddRecContract.aspx?uid={0}") %>'
                                                                                            runat="server" Font-Underline="true" Target="_blank" Text='<%# Eval("job")%>'></asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="tag" HeaderText="Name" SortExpression="tag" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" HeaderStyle-Width="100" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblName" Style="display: none" runat="server" Text='<%# Eval("tag")%>'></asp:Label>
                                                                                        <asp:HyperLink ID="lnkLoc" NavigateUrl='<%# Eval("job","AddRecContract.aspx?uid={0}") %>'
                                                                                            runat="server" Target="_blank" Text='<%# Eval("tag")%>'></asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="FullAddress" HeaderText="Address" SortExpression="FullAddress" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" HeaderStyle-Width="100" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAddress" ForeColor='<%# Eval("lat").ToString().Trim() == String.Empty ? System.Drawing.Color.Red : System.Drawing.Color.Black %>' runat="server" Text='<%# Eval("address")+", "+ Eval("City")%>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>


                                                                                <telerik:GridTemplateColumn UniqueName="Company" DataField="Company" HeaderText="Company" SortExpression="Company" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" HeaderStyle-Width="100" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderText="Origin" Visible="false"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkGridOrigin" ToolTip="Origin" runat="server" CssClass="origin" Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderText="Dest." Visible="false" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkGridDest" ToolTip="Destination" runat="server" CssClass="dest" Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="worker" UniqueName="DRoute" SortExpression="worker" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" HeaderStyle-Width="100" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblworkername" runat="server" Text='<%# Eval("worker") %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="MonthlyBill" HeaderText="Monthly Billing" SortExpression="MonthlyBill" Aggregate="Sum" FooterAggregateFormatString="{0:c}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMonthlyBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}")%></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="Freqency" HeaderText="Billing Frequency" SortExpression="Freqency" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("Freqency") %>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="BAmt" HeaderText="Total Period Billing" SortExpression="BAmt" Aggregate="Sum" FooterAggregateFormatString="{0:c}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTotPerBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "BAmt", "{0:c}")%></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>


                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="MonthlyHours" HeaderText="Monthly Hours" SortExpression="MonthlyHours" Aggregate="Sum" FooterAggregateFormatString="{0:n}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMonthlyHours" runat="server"><%#DataBinder.Eval(Container.DataItem,"MonthlyHours", "{0:n}")%></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="TicketFreq" HeaderText="Ticket Frequency" SortExpression="TicketFreq" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("TicketFreq") %>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="Hours" HeaderText="Total Period Hours" SortExpression="Hours" Aggregate="Sum" FooterAggregateFormatString="{0:n}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTotPeriodHours" runat="server"><%#Eval("Hours")%></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="elevcount" HeaderText="Equipment" SortExpression="elevcount" Aggregate="Sum" FooterAggregateFormatString="{0}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblunits" runat="server"><%#Eval("elevcount")%></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>
                                                                            </Columns>
                                                                        </MasterTableView>
                                                                        <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                        </FilterMenu>
                                                                    </telerik:RadGrid>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>

                                                </telerik:RadPanelItem>

                                                <telerik:RadPanelItem Expanded="False">
                                                    <HeaderTemplate>
                                                        <span class="worker-name"><b>&nbsp;
                                                            <asp:Label ID="Label3" runat="server">Workers</asp:Label></b></span>
                                                        <span class="text-css">
                                                            <asp:Label CssClass="title-check-text" ID="lblTotalRecWork" runat="server" Text="Label"></asp:Label>
                                                            &nbsp;</span>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <div class="form-content-wrap">
                                                            <div class="grid_container mt-10">
                                                                <div class="RadGrid RadGrid_Material">
                                                                    <telerik:RadGrid RenderMode="Auto" ID="gvWorkers" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                                        OnNeedDataSource="gvWorkers_NeedDataSource"
                                                                        OnPreRender="gvWorkers_PreRender"
                                                                        OnItemCreated="gvWorkers_ItemCreated"
                                                                        OnItemEvent="gvWorkers_ItemEvent"
                                                                        OnItemCommand="gvWorkers_ItemCommand">
                                                                        <CommandItemStyle />
                                                                        <GroupingSettings CaseSensitive="false" />
                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                        </ClientSettings>
                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                            <Columns>
                                                                                <telerik:GridTemplateColumn ShowFilterIcon="false" HeaderStyle-Width="30px" AllowFiltering="false">
                                                                                    <ItemTemplate>
                                                                                        <img width="12px" src='<%# getWorkerColor(Eval("Name")) %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn DataField="Name" UniqueName="WRoute" SortExpression="Name" DataType="System.String"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblName" runat="server" CommandName="select" CommandArgument='<%# Bind("id") %>'
                                                                                            Text='<%# Bind("Name") %>'></asp:LinkButton>
                                                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("id") %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="contr" HeaderText="Contracts" SortExpression="contr" Aggregate="Sum" FooterAggregateFormatString="{0}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblContracts" runat="server" Text='<%#Eval("contr")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <%--<FooterTemplate>
                                                                                            <asp:Label ID="lblContractsTotal" runat="server" Text=""></asp:Label>
                                                                                        </FooterTemplate>--%>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="units" HeaderText="Equipment" SortExpression="units" Aggregate="Sum" FooterAggregateFormatString="{0}" DataType="System.Int32"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("units")%>'></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="MonthlyHours" HeaderText="Hours" SortExpression="MonthlyHours" Aggregate="Sum" FooterAggregateFormatString="{0:n}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyHours", "{0:n}") %>'></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="MonthlyBill" HeaderText="Amount" SortExpression="MonthlyBill" Aggregate="Sum" FooterAggregateFormatString="{0:c}"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                                                    ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}") %>'></asp:Label>
                                                                                    </ItemTemplate>

                                                                                </telerik:GridTemplateColumn>

                                                                            </Columns>
                                                                        </MasterTableView>
                                                                        <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                        </FilterMenu>
                                                                    </telerik:RadGrid>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <!--test -->




                                                    </ContentTemplate>
                                                </telerik:RadPanelItem>

                                                <telerik:RadPanelItem Expanded="False">
                                                    <HeaderTemplate>

                                                        <span class="worker-name "><b>&nbsp;
                                                            <asp:Label ID="Label4" runat="server">Template</asp:Label></b>
                                                        </span>
                                                        &nbsp;
                                                               <span>
                                                                   <asp:CheckBox ID="chkMap" class="m-t-5" runat="server" 
                                                                       onclick="AssignedMarker();" ToolTip="Show template changes on map" />
                                                               </span>
                                                        <label>Show Changes</label>

                                                        <asp:LinkButton ID="lnkUpdateLocs" runat="server" class="link-css" 
                                                            ToolTip="Update Locations" OnClientClick="return confirm('Are you sure you want to update the locations with the template?');"
                                                            OnClick="lnkUpdateLocs_Click"><img height="25" alt="Save" src="images/update.png"/>
                                                        </asp:LinkButton>


                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <div class="form-content-wrap">
                                                            <div class="srchinputwrap">

                                                                <div id="divTemplateOuter" onclick="UpdatehdnPreviousTempl();">
                                                                    <asp:DropDownList ID="ddlTemplates" runat="server" AutoPostBack="false" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged" TabIndex="14" ToolTip="Select Template" onchange="DisplayConfirmation()">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="srchinputwrap">
                                                                <asp:TextBox ID="txtTemplate" runat="server" placeholder="Template Name" CssClass="srchcstm"
                                                                    ToolTip="Template Name"></asp:TextBox>
                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkSaveTemplate" runat="server" OnClick="lnkSaveTemplate_Click"
                                                                        OnClientClick="if(!validateSave()) return false;"
                                                                        ToolTip="Save Template"> Save</asp:LinkButton>
                                                                    <%--    
                                                                    --%>
                                                                </div>
                                                            </div>
                                                            <div class="srchinputwrap">
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkDelTemplate" runat="server" OnClick="lnkDeleteRoute_Click"
                                                                        OnClientClick="if(!DeleteTemplate()) return false;"
                                                                        ToolTip="Delete Template"> Delete</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="form-section-row">
                                                                <asp:TextBox ID="txtRemarks" CssClass="materialize-textarea" TextMode="MultiLine" placeholder="Remarks" ToolTip="Remarks" runat="server"></asp:TextBox>
                                                            </div>

                                                            <div class="form-section-row tabs">
                                                                <div class="btnlinks">
                                                                    <%--<a id="tabLoc" class="tab-item" onclick="tabState(this.id);" href="#view1">Location Changes</a>--%>
                                                                    <asp:LinkButton runat="server" ID="btnLocationChange" OnClick="btnLocationChange_Click">Location Changes</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <%--<a id="tabWork" class="tab-item" onclick="tabState(this.id);" href="#view2">Worker Changes</a>--%>
                                                                    <asp:LinkButton runat="server" ID="btnWorkerChange" OnClick="btnWorkerChange_Click">Worker Changes</asp:LinkButton>

                                                                </div>
                                                            </div>
                                                            <div class="form-section-row">
                                                                <div runat="server" id="locationChangesView" class="excel-ex-css" >
                                                                    <div class="btncontainer">
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="btnLocExportToExcel" runat="server" Text="Export" OnClientClick="OnLocationExportExcel();" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-content-wrap">
                                                                        <div class="grid_container mt-10" >
                                                                            <div class="RadGrid RadGrid_Material">
                                                                                <telerik:RadGrid RenderMode="Auto" ID="gvLocChanges" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%"
                                                                                    PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                                                    OnNeedDataSource="gvLocChanges_NeedDataSource"
                                                                                    OnExcelMLExportRowCreated="gvLocChanges_ExcelMLExportRowCreated">
                                                                                    <CommandItemStyle />
                                                                                    <GroupingSettings CaseSensitive="false" />
                                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                    </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                        <Columns>
                                                                                            <telerik:GridTemplateColumn  DataField="tag" HeaderText="Location" SortExpression="tag" DataType="System.String"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("tag") %>'>
                                                                                                    </asp:Label>
                                                                                                    <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("loc") %>' />
                                                                                                    <asp:HiddenField ID="hdnHours" runat="server" Value='<%# Bind("MonthlyHours") %>' />
                                                                                                    <asp:HiddenField ID="hdnAmt" runat="server" Value='<%# Bind("MonthlyBill") %>' />
                                                                                                    <asp:HiddenField ID="hdnPolyid" runat="server" Value='<%# Bind("polyid") %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="worker" UniqueName="CurrentWorker" AllowFiltering="false" AllowSorting="false" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblCurWorker" runat="server" Text='<%# Bind("worker") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="assdwrkr" UniqueName="NewWoker" AllowFiltering="false" AllowSorting="false" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblNewWorker" Style="color: Red" runat="server" Text='<%# Bind("assdwrkr") %>'>
                                                                                                    </asp:Label>
                                                                                                    <asp:HiddenField ID="hdnWorker" runat="server" Value='<%# Bind("assdwrkrid") %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="address" HeaderText="Address" SortExpression="address" AllowFiltering="false" AllowSorting="false" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("address")+", "+ Eval("City")%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="MonthlyBill" HeaderText="Monthly Billing" SortExpression="MonthlyBill" Aggregate="Sum" FooterAggregateFormatString="{0:c}"
                                                                                                AutoPostBackOnFilter="true" AllowFiltering="false" AllowSorting="false"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblMonthlyBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}")%></asp:Label>
                                                                                                </ItemTemplate>

                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="BAmt" HeaderText="Total Period Billing" SortExpression="BAmt" Aggregate="Sum" FooterAggregateFormatString="{0:c}"
                                                                                                AutoPostBackOnFilter="true" AllowFiltering="false" AllowSorting="false"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTotPerBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "BAmt", "{0:c}")%>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>

                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="MonthlyHours" HeaderText="Monthly Hours" SortExpression="MonthlyHours" Aggregate="Sum" FooterAggregateFormatString="{0:n}"
                                                                                                AutoPostBackOnFilter="true" AllowFiltering="false" AllowSorting="false"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblMonthlyHours" runat="server"><%#DataBinder.Eval(Container.DataItem,"MonthlyHours", "{0:n}")%>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>

                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="Hours" HeaderText="Total Period Hours" SortExpression="Hours" Aggregate="Sum" FooterAggregateFormatString="{0:n}"
                                                                                                AutoPostBackOnFilter="true" AllowFiltering="false" AllowSorting="false"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTotPeriodHours" runat="server"><%#Eval("Hours")%>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>

                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="elevcount" HeaderText="Equipment" SortExpression="elevcount" Aggregate="Sum" FooterAggregateFormatString="{0}"
                                                                                                AutoPostBackOnFilter="true" AllowFiltering="false" AllowSorting="false"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblunitsGd" runat="server"><%#Eval("elevcount")%>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>

                                                                                            </telerik:GridTemplateColumn>

                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                    <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                    </FilterMenu>
                                                                                </telerik:RadGrid>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div id="workerChangesView" visible="false" class="work-css" runat="server" style="max-height: 313px; overflow-y: scroll; min-height: 93px; background: #fff; border: solid 1px #ccc; position: relative;">
                                                                    <div class="btncontainer">
                                                                        <div class="btnlinks">
                                                                            <asp:Button ID="btnExportToExcel" runat="server" class="add-btn" Text="Export" OnClientClick="OnWorkerExportExcel();" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-content-wrap">
                                                                        <div class="grid_container basi-class" style="margin-top: 10px">
                                                                            <div class="RadGrid RadGrid_Material">
                                                                                <telerik:RadGrid RenderMode="Auto" ID="gvWorkerChanges" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList">
                                                                                    <CommandItemStyle />
                                                                                    <GroupingSettings CaseSensitive="false" />
                                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                    </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                        <Columns>    
                                                                                            <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="12px">
                                                                                                <ItemTemplate>
                                                                                                    <img width="12px" src='<%# getWorkerColor(Eval("Name")) %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn DataField="name" HeaderText="Worker" SortExpression="name" DataType="System.String"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblCurWorker" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn HeaderText="Contracts" AllowSorting="false" AllowFiltering="false"
                                                                                                AutoPostBackOnFilter="true"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblContr" runat="server" Text='<%# Eval("contr") %>'></asp:Label>
                                                                                                    -
                                                                                                <asp:Label ID="lblNewContr" Style="color: Red" runat="server" Text=""></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lbltotalCU" runat="server"></asp:Label>
                                                                                                </FooterTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn HeaderText="Units" AllowSorting="false" AllowFiltering="false"
                                                                                                AutoPostBackOnFilter="true"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblUnits" runat="server" Text='<%# Eval("units") %>'></asp:Label>
                                                                                                    -
                                                                                                <asp:Label ID="lblNewUnits" Style="color: Red" runat="server" Text=""></asp:Label>
                                                                                                    <asp:Label ID="lblunitsGd" runat="server"> 
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lbltotalNCU" runat="server"></asp:Label>
                                                                                                </FooterTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn HeaderText="Hours(Monthly)" AllowSorting="false" AllowFiltering="false"
                                                                                                AutoPostBackOnFilter="true"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyHours", "{0:n}") %>'></asp:Label>
                                                                                                    -
                                                                                                <asp:Label ID="lblNewHours" Style="color: Red" runat="server" Text=""></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lbltotalHA" runat="server"></asp:Label>
                                                                                                </FooterTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn HeaderText="Amount(Monthly)" AllowSorting="false" AllowFiltering="false"
                                                                                                AutoPostBackOnFilter="true"
                                                                                                ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}") %>'></asp:Label>
                                                                                                    <asp:HiddenField ID="hdnAmt" runat="server" Value='<%# Eval("MonthlyBill") %>' />
                                                                                                    -
                                                                                                <asp:Label ID="lblNewamt" Style="color: Red" runat="server" Text=""></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lbltotalNHA" runat="server"></asp:Label>
                                                                                                </FooterTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                    <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                    </FilterMenu>
                                                                                </telerik:RadGrid>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>

                                                </telerik:RadPanelItem>
                                            </Items>
                                        </telerik:RadPanelBar>

                                    </telerik:RadAjaxPanel>

                                </div>
                            </div>
                        </telerik:RadSlidingPane>
                    </telerik:RadSlidingZone>
                </telerik:RadPane>
                <telerik:RadPane ID="MiddlePane" runat="server">
                    <div id="map-canvas" "
                        class="roundCorner shadow map-cav-css">
                    </div>

                </telerik:RadPane>
                <telerik:RadSplitBar ID="Radsplitbar2" runat="server">
                </telerik:RadSplitBar>

            </telerik:RadSplitter>

            <div class="btnlinks">
                <asp:LinkButton ID="lnkWorkerExportPostback" runat="server" CausesValidation="False" Style="display: none" OnClick="lnkWorkerExportPostback_Click" />
                <asp:LinkButton ID="lnkLocExportPostback" runat="server" CausesValidation="False" Style="display: none" OnClick="lnkLocExportPostback_Click" />
            </div>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">--%>
                    <asp:Button ID="btnAssign" runat="server" Text="btnAssign" OnClick="btnAssign_Click"
                        Style="display: none" OnClientClick="ClickAssignCheck();" />
                    <asp:Button ID="btnAssignMethodCall" runat="server" Text="btnAssignMethodCall" Style="display: none"
                        OnClick="btnAssignMethodCall_Click" />
                    <asp:HiddenField ID="hdnCenter" runat="server" />
                    <asp:HiddenField ID="hdnRadius" runat="server" />
                    <asp:HiddenField ID="hdnEdited" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnPreviousTempl" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnAssignedWorker" runat="server" />
                    <asp:HiddenField ID="hdnPolygon" runat="server" />
                    <asp:HiddenField ID="hdnOverlay" runat="server" />
                    <asp:HiddenField ID="hdnLocsInPolygon" runat="server" />
                    <asp:HiddenField ID="hdnMarkerLoc" runat="server" />
                    <asp:HiddenField ID="hdnPolyID" runat="server" />
                    <%--</telerik:RadAjaxPanel>--%>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel2" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll" PopupDragHandleControlID="Panel3">
            </asp:ModalPopupExtender>
            <div runat="server" id="Panel2" style="display: none; background: #fff; border: solid;"
                cssclass="roundCorner shadow">
                <div runat="server" id="Panel3" style="background: #ccc; width: 100%; height: 20px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; cursor: move">
                    <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                        ID="Label5" runat="server">Workers</asp:Label>
                </div>
                <div>


                    <asp:ListBox ID="lstWorker" runat="server" Style="max-height: 200px; min-height: 200px; width: 100%"></asp:ListBox>

                </div>
                <input id="Button2" type="button" value="Assign" style="width: 80px" />
                <input id="Button1" type="button" value="Cancel" style="width: 80px" onclick="$find('PMPBehaviour').hide();" />
            </div>

            <div runat="server" id="Panel1" style="display: none; background: #fff; border: solid;"
                cssclass="roundCorner shadow">
                <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender2" BehaviorID="PMPMarker"
                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
                    RepositionMode="RepositionOnWindowResizeAndScroll" PopupDragHandleControlID="Panel4">
                </asp:ModalPopupExtender>
                <div runat="server" id="Panel4" style="background: #ccc; width: 100%; height: 20px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; cursor: move">
                    <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                        ID="Label1" runat="server">Assign Worker</asp:Label>
                </div>
                <div>


                    <asp:ListBox ID="lstWorkerAssignMarker" runat="server" Style="max-height: 200px; min-height: 200px; width: 100%"></asp:ListBox>
                    <asp:Button ID="btnAssignWorkerMarker" runat="server" Text="Assign" Style="width: 80px"
                        OnClick="btnAssignWorkerMarker_Click" OnClientClick="if(!confirm('This will assign worker to the selected location. Do you want to continue?')) return false;" />
                    <input id="btnCloseAssignMarker" type="button" value="Cancel" style="width: 80px"
                        onclick="$find('PMPMarker').hide();" />

                </div>
            </div>

            <div runat="server" id="Panel5" style="display: none; background: #fff; border: solid;"
                cssclass="roundCorner shadow">
                <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender3" BehaviorID="PMPAddress"
                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel5" BackgroundCssClass="pnlUpdateoverlay"
                    RepositionMode="RepositionOnWindowResizeAndScroll">
                </asp:ModalPopupExtender>
                <div>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate> 
                            <input id="btnCloseAdd" type="button" value="Cancel" style="width: 80px; display: none"
                                onclick="$find('PMPAddress').hide();" />
                            <asp:Button ID="btnClearClick" Text="Button" CausesValidation="false" Style="display: none"
                                runat="server" OnClick="btnClearClick_Click" />
                            <iframe id="iframeAddr" runat="server" frameborder="0" width="900px " height="451px"></iframe>
                             
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        $(window).scroll(function () {
            if ($(window).scrollTop() >= 0) {
                $('#divButtons').addClass('fixed-header');
            }
            if ($(window).scrollTop() <= 0) {
                $('#divButtons').removeClass('fixed-header');
            }
        });

        $(function () {
            var tabval = $("#<%=tabState.ClientID%>").val();
            $("#" + tabval).click();
        });
    </script>
</asp:Content>
