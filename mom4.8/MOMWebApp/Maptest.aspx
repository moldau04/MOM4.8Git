<%@ Page Language="C#" AutoEventWireup="true" Inherits="Maptest" Codebehind="Maptest.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <style>
        /*	start styles for the ContextMenu	*/.context_menu
        {
            background-color: white;
            border: 1px solid gray;
        }
        .context_menu_item
        {
            padding: 3px 6px;
        }
        .context_menu_item:hover
        {
            background-color: #CCCCCC;
        }
        .context_menu_separator
        {
            background-color: gray;
            height: 1px;
            margin: 0;
            padding: 0;
        }
        #clearDirectionsItem, #getDirectionsItem
        {
            display: none;
        }
        /*	end styles for the ContextMenu	*/</style>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

    <script type="text/javascript" src="js/ContextMenu.js"></script>

    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    
     <script type="text/javascript" src="http://google-maps-utility-library-v3.googlecode.com/svn/tags/markerwithlabel/1.1.5/src/markerwithlabel_packed.js"></script>

    <script type="text/javascript">
  var coord  =  [<asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>('<%# Eval("coordinates") %>')</ItemTemplate><SeparatorTemplate>,</SeparatorTemplate>                
                </asp:Repeater>];
    </script>

    <script type="text/javascript">

        var directionsDisplay;
        var directionsService = new google.maps.DirectionsService();
        var map;
//        var cityCircle;
        var contextMenu;
//        var contextMenuCircle;

        function initialize() {
//            cityCircle = new google.maps.Circle();
            directionsDisplay = new google.maps.DirectionsRenderer();
            var chicago = new google.maps.LatLng(41.850033, -87.6500523);
            var mapOptions = {
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: chicago
            }
            map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
            directionsDisplay.setMap(map);

            //calcRoute();
            //        }

            AddContextMenu();
        }

        function AddContextMenu() {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItems = [];
            menuItems.push({ className: 'context_menu_item', eventName: 'add_circle_here', label: 'Add Circle Here' });
            menuItems.push({});
            menuItems.push({ className: 'context_menu_item', eventName: 'center_map_click', label: 'Center map here' });
            contextMenuOptions.menuItems = menuItems;

            contextMenu = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(map, 'rightclick', function(mouseEvent) {
                contextMenu.show(mouseEvent.latLng);
            });
            eventRightClick(contextMenu);
            //            google.maps.event.addListener(contextMenu, 'menu_item_selected', function(latLng, eventName) {
            //                switch (eventName) {
            //                    case 'add_circle_here':
            //                        //map.setZoom(map.getZoom() + 1);
            //                        AddCircle(latLng);
            //                        break;
            //                    case 'center_map_click':
            //                        map.panTo(latLng);
            //                        break;
            //                    case 'clear_circle':
            //                        cityCircle.setMap(null);
            //                        break;
            //                    case 'assign_worker':
            //                        
            //                        break;
            //                }
            //            });
        }

        function AddContextMenuCircle(cityCircle) {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItemsCircle = [];
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'clear_circle', label: 'Clear Circle' });
            menuItemsCircle.push({});
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'assign_worker', label: 'Assign Worker' });
            contextMenuOptions.menuItems = menuItemsCircle;

            var contextMenuCircle = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(cityCircle, 'rightclick', function(mouseEvent) {
                contextMenuCircle.show(mouseEvent.latLng);
            });

            eventRightClick(contextMenuCircle, cityCircle);
        }

        function eventRightClick(contextMenu,cityCircle) {
            google.maps.event.addListener(contextMenu, 'menu_item_selected', function(latLng, eventName) {
                switch (eventName) {
                    case 'add_circle_here':
                        //map.setZoom(map.getZoom() + 1);
                        AddCircle(latLng);
                        break;
                    case 'center_map_click':
                        map.panTo(latLng);
                        break;
                    case 'clear_circle':
                        cityCircle.setMap(null);
                        break;
                    case 'assign_worker':
                        $find('PMPBehaviour').show();
                        break;
                }
            });
        }


        function AddCircle(latLng) {

//            cityCircle.setMap(null);

            var marker = new MarkerWithLabel({
                position: new google.maps.LatLng(0, 0),
                draggable: false,
                raiseOnDrag: false,
                map: map,
                labelContent: 'test',
                labelAnchor: new google.maps.Point(30, 20),
                labelClass: "labels", // the CSS class for the label
                labelStyle: { opacity: 1.0 },
                icon: "http://placehold.it/1x1",
                visible: false
            });


            var populationOptions = {
                strokeColor: '#000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FFF',
                //                                fillOpacity: 0.35,
                fillOpacity: 0.0,
                map: map,
                //center: citymap[city].center,
                center: latLng,
                radius: 10,
                //                                radius: citymap[city].population / 20,
                draggable: true,
                editable: true
            };
            var cityCircle = new google.maps.Circle(populationOptions);
            AddContextMenuCircle(cityCircle);

            google.maps.event.addListener(cityCircle, "mousemove", function(event) {
                marker.setPosition(event.latLng);
                marker.setVisible(true);
            });
            google.maps.event.addListener(cityCircle, "mouseout", function(event) {
                marker.setVisible(false);
            });
            
            //                google.maps.event.addListener(cityCircle, 'rightclick', function(mouseEvent) {
            //                    contextMenuCircle.show(mouseEvent.latLng);
            //                });
        }

        function assignClick() {
            $('#<%=hdnCenter.ClientID%>').val(cityCircle.getCenter().lat() + ',' + cityCircle.getCenter().lng());
            $('#<%=hdnRadius.ClientID%>').val((cityCircle.getRadius()));
            $('#<%=btnAssign.ClientID%>').click();
        }

//        function calcRoute() {
//            //            var start = document.getElementById('start').value;
//            //            var end = document.getElementById('end').value;
//            var waypts = [];
//            //            var checkboxArray = document.getElementById('waypoints');
//            //            for (var i = 0; i < checkboxArray.length; i++) {
//            //                if (checkboxArray.options[i].selected == true) {
//            //                    waypts.push({
//            //                        location: checkboxArray[i].value,
//            //                        stopover: true
//            //                    });
//            //                }
//            //            }


//            for (var i = 1; i < coord.length - 1; i++) {
//                waypts.push({
//                    location: coord[i],
//                    stopover: true
//                });
//            }
//            var start = coord[0];
//            var end = coord[coord.length - 1];

//            var request = {
//                origin: start,
//                destination: end,
//                waypoints: waypts,
//                optimizeWaypoints: true,
//                travelMode: google.maps.TravelMode.DRIVING
//            };

//            directionsService.route(request, function(response, status) {
//                if (status == google.maps.DirectionsStatus.OK) {
//                    directionsDisplay.setDirections(response);
//                }
//            });
        //        }






        google.maps.event.addDomListener(window, 'load', initialize);
       
    </script>

    <asp:Button ID="btnAssign" runat="server" Text="Button" 
        onclick="btnAssign_Click" style="display:none" />
    <asp:HiddenField ID="hdnCenter" runat="server" />
    <asp:HiddenField ID="hdnRadius" runat="server" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
        RepositionMode="RepositionOnWindowResizeAndScroll" PopupDragHandleControlID="Panel2">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;">
        <asp:Panel runat="server" ID="Panel2" Style="background: #ccc; width: 80px; height: 10px;
            cursor: move">
        </asp:Panel>
        <div>
            <a style="float: right; left: 72px; top: -18px; position: absolute" onclick="$find('PMPBehaviour').hide();">
                <img height="20px" src="images/close.png" /></a>
            <input id="Button2" type="button" value="button" onclick="assignClick();" />
            <asp:ListBox ID="lstWorker" runat="server" Style="max-height: 200px; min-height: 200px;
                width: 80px"></asp:ListBox>
        </div>
    </asp:Panel>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <div id="map-canvas" style="width: 1024px; height: 500px">
    </div>
    <input id="Button1" type="button" value="button" onclick="alert(cityCircle.getRadius()/1000);alert(cityCircle.getCenter())" />
    </form>
</body>
</html>
