<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_NinetyPlusAR" Codebehind="NinetyPlusAR.ascx.cs" %>

<div>
 <div id="sixtyPlusAR">
     <div class="row">
         <div class="col s2">
             <img id="imgNinetyPlusUp" src="../images/upArrow.png"  height="30px" width="30px">
             <img  id="imgNinetyPlusDown" src="../images/downArrow.png"  height="30px" width="30px">

         </div>
         <div class="col s4" id="countsNinetyPlus"></div>         
     </div>
      <div class="row">
          <div class="col s5">+90 AR</div>
      </div>     
     <script>
         function createGetNinetyPlusAR() {
             $.ajax({
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 url: "KPI/KPIWebService.asmx/GetNinetyPlusAR",
                 data: {},
                 dataType: "json",
                 success: function (data) {
                     $("#countsNinetyPlus").text(data.d[1].toFixed(2))
                     if (data.d[0] > data.d[1]) {
                         $("#imgNinetyPlusUp").hide()
                         $("#imgNinetyPlusDown").show()
                     }
                     else {
                         $("#imgNinetyPlusUp").show()
                         $("#imgNinetyPlusDown").hide()
                     }
                 },
                 error: function (result) {     
                     //show no data messages here
                 }
             })
         }
    </script>
 </div>  
</div>