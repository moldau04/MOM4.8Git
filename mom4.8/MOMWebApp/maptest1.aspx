<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" Inherits="maptest1" Codebehind="maptest1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

<script type="text/javascript">
    var bounds;
    var map;
    var infoWindow;
    var geocoder;

function initialize(jsondata) {
    geocoder = new google.maps.Geocoder();
    infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });
    bounds = new google.maps.LatLngBounds();

            var mapOptions = {
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: new google.maps.LatLng(0, 0),
                scrollwheel: true
            }
            map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

            //            var markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            var markers = JSON.parse(jsondata);
            AddPolylineArray(markers);
        }
//        google.maps.event.addDomListener(window, 'load', initialize);

       

        function AddPolylineArray(markers) {
            var v = [];
            for (var i = 0; i < markers.length; i++) {
                var data = markers[i];
                var latlng = new google.maps.LatLng(data.latitude, data.longitude);
                bounds.extend(latlng);
                v[i] = latlng;
                
                if (i == 0) {
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        icon: "http://www.googlemapsmarkers.com/v1/S/0099FF/FFFFFF/FF0000/",
                        title: "Starting point - " + data.parsedate
                    });
                    (function(marker, data) {
                        google.maps.event.addListener(marker, "click", function(e) {
                            infoWindow.setContent("<div> <span> Starting point  " + String(data.parsedate) + " </span></DIV>");
                            infoWindow.open(map, marker);
                        });
                    })(marker, data);
                }

                if (i == markers.length - 1) {
                    var marker = new google.maps.Marker({
                        position: latlng,
                        map: map,
                        icon: "http://www.googlemapsmarkers.com/v1/E/0099FF/FFFFFF/FF0000/",
                        title: "End point - " + data.parsedate
                    });
                    (function(marker, data) {
                        google.maps.event.addListener(marker, "click", function(e) {
                            infoWindow.setContent("<div> <span> End point  " + String(data.parsedate) + " </span></DIV>");
                            infoWindow.open(map, marker);
                        });
                    })(marker, data);
                }

             /*   if (data.timestm == "1" || data.timestm == "2" || data.timestm == "3") {
                    var icon;
                    if (data.timestm == "1")
                        icon = "http://www.googlemapsmarkers.com/v1/CT/0099FF/";
                    else if (data.timestm == "2")
                        icon = "http://www.googlemapsmarkers.com/v1/OS/0099FF/";
                    else if (data.timestm == "3")
                        icon = "http://www.googlemapsmarkers.com/v1/ER/0099FF/";
                        
                    var marker = new google.maps.Marker({                    
                        position: latlng,
                        map: map,
                        icon: icon,
                        title: data.parsedate
                    });
                    (function(marker, data) {
                        google.maps.event.addListener(marker, "click", function(e) {
                            var GPSaddress = Geocode(latlng);
                            var info = "<strong> Ticket #: " + data.id + "</strong></BR></BR><strong> Name: " + data.Name + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + data.parsedate;
                            infoWindow.setContent(info);
                            infoWindow.open(map, marker);
                        });
                    })(marker, data);
                }*/

            }
            AddPolyLine(v);
            map.fitBounds(bounds);                

        }

        function AddPolyLine(Coordinates) {
            var polyPath = new google.maps.Polyline({
                path: Coordinates,
                geodesic: true,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2                
            });

            polyPath.setMap(map);
            return polyPath;
        }
        

        function AddMarker(markers, boundmap) {

            for (var i = 0; i < markers.length; i++) {
                AddSingleMarker(markers[i], infoWindow, bounds);
            }
            if (boundmap == 1) {
                map.fitBounds(bounds);
            }
        }

        function AddSingleMarker(markeradd) {
            var data = markeradd;
            var myLatlng = new google.maps.LatLng(data.latitude, data.longitude);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map                
            });
            //bounds.extend(myLatlng);
        }

        function Geocode(latlng) {
            var address;
            geocoder.geocode({ 'latLng': latlng }, function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        results[1].formatted_address;
                    }
                }

            });
            return address;
        }
        
        </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:HiddenField ID="hdnMarkers" runat="server" />
 <div id="map-canvas" style="margin-left: 5px; width: 100%; height: 100%; min-width: 1024px;
                    min-height: 800px" class="roundCorner shadow">
                </div>
</asp:Content>

