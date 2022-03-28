<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" Inherits="TestMap" Codebehind="TestMap.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            initialize();
        });

        function initialize() {
            var latlng = new google.maps.LatLng(57.0442, 9.9116);
            var settings = {
                zoom: 15,
                center: latlng,
                mapTypeControl: true,
                mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                navigationControl: true,
                navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL },
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), settings);
            var contentString = '<div id="content">' +
					'<div id="siteNotice">' +
					'</div>' +
					'<h1 id="firstHeading" class="firstHeading">Høgenhaug</h1>' +
					'<div id="bodyContent">' +
					'<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>' +
					'</div>' +
					'</div>';
            var infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            var companyImage = new google.maps.MarkerImage('images/logo.png',
					new google.maps.Size(100, 50),
					new google.maps.Point(0, 0),
					new google.maps.Point(50, 50)
				);

            var companyShadow = new google.maps.MarkerImage('images/logo_shadow.png',
					new google.maps.Size(130, 50),
					new google.maps.Point(0, 0),
					new google.maps.Point(65, 50));


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "testmap.aspx/getmap",
                data: '{}',
                dataType: "json",
                async: true,
                success: function(data) {
                    // alert(data.d);
                $.each(data.d, function() {
                    //alert(JSON.parse(data.d));
                $.each(JSON.parse(data.d), function(i, marker) {
                            //alert(marker.latitude);

                            var companyPos = new google.maps.LatLng(marker.latitude, marker.latitude);

                            var companyMarker = new google.maps.Marker({
                                position: companyPos,
                                map: map,
                                //icon: companyImage,
                                // shadow: companyShadow,
                                title: "Høgenhaug",
                                zIndex: 3
                            });
                            google.maps.event.addListener(companyMarker, 'click', function() {
                                infowindow.open(map, companyMarker);
                            });
                        });
                    });
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    var err = eval("(" + XMLHttpRequest.responseText + ")");
                    alert(err.Message);
                }
            });   
            

            
        }


//        $('#map_canvas').gmap().bind('init', function() {
//            // This URL won't work on your localhost, so you need to change it
//            // see http://en.wikipedia.org/wiki/Same_origin_policy
//            $.getJSON('http://jquery-ui-map.googlecode.com/svn/trunk/demos/json/demo.json', function(data) {
//                $.each(data.markers, function(i, marker) {
//                    $('#map_canvas').gmap('addMarker', {
//                        'position': new google.maps.LatLng(marker.latitude, marker.longitude),
//                        'bounds': true
//                    }).click(function() {
//                        $('#map_canvas').gmap('openInfoWindow', { 'content': marker.content }, this);
//                    });
//                });
//            });
//        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="map_canvas" style="width: 500px; height: 300px">
    </div>
</asp:Content>
