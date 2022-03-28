<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" CodeBehind="WebDemo.aspx.cs" Inherits="MOMWebApp.WebDemo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <h2 style="background-color: Yellow;color: Blue; text-align: center; font-style: oblique">Shyam's Goolge Map View With Distance And Duration Using MVC and BOOTSTRAP</h2>  
<meta charset="utf-8">  
<meta name="viewport" content="width=device-width, initial-scale=1">  
     
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">  
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>  
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>  
  

    <style>  
    
    #dvMap {  
        height: 100%;  
    }  
  
    .button {  
        background-color: #4CAF50;  
        border: none;  
        color: white;  
        padding: 15px 32px;  
        text-align: center;  
        text-decoration: none;  
        display: inline-block;  
        font-size: 16px;  
        margin: 4px 2px;  
        cursor: pointer;  
    }  
  
    .button4 {  
        border-radius: 9px;  
    }  
  
    input[type=text], select {  
        width: 40%;  
        padding: 12px 20px;  
        margin: 10px 0;  
        display: inline-block;  
        border: 1px solid #ccc;  
        border-radius: 4px;  
        box-sizing: border-box;  
    }  
</style>  
   
 <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?libraries=places&key=AIzaSyBFYsRuGgMB5SJrz2RrwJs-BdsWmkCy05I"></script>

     <script type="text/javascript">  
         var source, destination;
         var directionsDisplay;
         var directionsService = new google.maps.DirectionsService();
         google.maps.event.addDomListener(window, 'load', function () {
             new google.maps.places.SearchBox(document.getElementById('txtSource'));
             new google.maps.places.SearchBox(document.getElementById('txtDestination'));
             directionsDisplay = new google.maps.DirectionsRenderer({ 'draggable': true });
         });

         function GetRoute() {
             debugger;
             var mumbai = new google.maps.LatLng(18.9750, 72.8258);
             var mapOptions = {
                 zoom: 7,
              center: mumbai
                 
             };
             map = new google.maps.Map(document.getElementById('dvMap'), mapOptions);
             directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('dvPanel'));

             source = document.getElementById("txtSource").value;
             destination = document.getElementById("txtDestination").value;

             var request = {
                 origin: source,
                 destination: destination,
                 travelMode: google.maps.TravelMode.DRIVING
             };
             directionsService.route(request, function (response, status) {
                 if (status == google.maps.DirectionsStatus.OK) {
                     directionsDisplay.setDirections(response);
                 }
             });


             var service = new google.maps.DistanceMatrixService();
             service.getDistanceMatrix({
                 origins: [source],
                 destinations: [destination],
                 travelMode: google.maps.TravelMode.DRIVING,
                 unitSystem: google.maps.UnitSystem.METRIC,
                 avoidHighways: false,
                 avoidTolls: false
             }, function (response, status) {
                 if (status == google.maps.DistanceMatrixStatus.OK && response.rows[0].elements[0].status != "ZERO_RESULTS") {
                     var distance = response.rows[0].elements[0].distance.text;
                     var duration = response.rows[0].elements[0].duration.text;
                     var dvDistance = document.getElementById("dvDistance");
                     dvDistance.innerHTML = "";
                     dvDistance.innerHTML += "Distance Is: " + distance + "<br />";
                     dvDistance.innerHTML += "Duration Is:" + duration;

                 } else {
                     alert("Your Request For Distance Not Available");
                 }
             });
         }
        </script>  
                                  
           
    
    <input type="text" id="txtSource" placeholder="Enter Source...." />  
                       
                    <input type="text" id="txtDestination" placeholder="Enter Destination...." />  
                    <br />  
                    <input type="button" value="Show" onclick="GetRoute()" class="button button4" />   
                    <hr />  
                    <div id="dvDistance" style="font-size:x-large; font-family: Arial Black; background-color: Yellow; color: Blue; text-align: center">  
                    </div>     
                    <div id="dvMap">  
                    </div>  
                    <div id="dvPanel" >  
                    </div>                                   
                    <hr />  
   <%--  <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
    <telerik:RadSkinManager ID="RadSkinManager2" runat="server" ShowChooser="true" />
    <div class="demo-container size-auto">
        <h2 class="mapTitle">TELERIK OFFICES</h2>
        <telerik:RadMap RenderMode="Auto" runat="server" ID="RadMap1" Zoom="2" CssClass="MyMap" OnItemDataBound="RadMap1_ItemDataBound">
            <CenterSettings Latitude="23" Longitude="10" />
            <DataBindings>
                <MarkerBinding DataShapeField="Shape" DataTitleField="City" DataLocationLatitudeField="Latitude" DataLocationLongitudeField="Longitude" />
            </DataBindings>
            <LayersCollection>
                <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                    UrlTemplate="https://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                    Attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>.">
                </telerik:MapLayer>
            </LayersCollection>
        </telerik:RadMap>
    </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="server">
</asp:Content>
