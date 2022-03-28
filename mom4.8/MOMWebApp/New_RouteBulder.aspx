

 <%@ Page Title="New_RouteBulder || MOM" Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="New_RouteBulder" CodeBehind="~/New_RouteBulder.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
     <link href="Design/css/grid.css" rel="stylesheet" />
    <%--<script async src="//jsfiddle.net/swyk86ta/embed/"></script>--%>
    
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<%--    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
   --%>
    <style type="text/css">
        /*::-webkit-scrollbar {
            width:10px; 

        }
       

        ::-webkit-scrollbar-track {
            background-color: transparent;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #d6dee1;
            border-radius: 20px;
            border: 6px solid transparent;*/
           /* background-clip: content-box;*/
        /*}

            ::-webkit-scrollbar-thumb:hover {
                background-color: #a8bbbf;
      }*/
            .divbutton-container {
                height: 50px;
            }
       .lead {
                    font-weight: 600;
                    line-height: 20px;
                    transform: rotate( 0deg );
                    writing-mode: vertical-rl;
                    font-size: .75em;
                    font-family: inherit;
                    text-transform: uppercase;
                    padding: 8px 3px;
                    background-color: #ECEFF1;
                    color: #4b6673;
                    border-color: #ECEFF1;
                    letter-spacing: 0.71px;
        }
       .panel-heading{
           background-color: #e8e7e7;
           height:600px;
           width:26px;
       }
        .btnclosewrap {
            position: absolute;
             right: 0px;
        }
        .userinfoimg-btn {
            padding-top: 0px;
        }
       .location-p{
           
            position: absolute;
            top: 0px;
            left: 26px;
            border: 1px solid #e5e5e5;
       }
       .heading-area{
           display:flex;
           background: #1c5fb1;
           margin-top:-1px;
       }
       .heading h5{
           color: #fff;
           height: 39px;
           line-height: 45px;
            font-size: 16px;
            padding-left:10px;
       }
       .heading-area .control{
           position: absolute; right: 0; 
       }
       .control a{
           line-height: 3.42857em;
           margin: 0px 5px 0 0;
           padding: 7px 10px;
           border-radius: 45px;
           color: #272c32;
           background-color: #fff;
       }
       .control span{
           color: #fff;
            padding-right: 10px;
       }
       .location-details{
            width:500px;
            height:600px;
       }
       .panel-heading a:hover{
           color: #0056b3;
           text-decoration: none;
       }
        .panel-default{
            position:relative;
        }
        </style>

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
    <div class="container">
        <div class="row">
            <div class="col" style="width:2%">
               <div class="panel panel-default"> 
                    <div class="panel-heading" >
                         <a id="Locations" class="nodecoration panel-title lead" data-toggle="collapse" data-parent="#panel-814345" href="#panel-element-1">Locations</a><br />
                        <a  id="Workers" class="nodecoration panel-title lead" data-toggle="collapse" data-parent="#panel-814345" href="#panel-element-2">Workers</a><br />
                        <a  id="Template" class="nodecoration panel-title lead" data-toggle="collapse" data-parent="#panel-814345" href="#panel-element-3">Template</a>

                    </div>
                    <div id="panel-element-1" class="panel-collapse collapse  location-p" >
                        <div class="panel-body" style="border:none; font-size:14px; padding-bottom:0; margin-bottom:0;">
                            <div class="location-details">
                               <div class="heading-area">
                                  <div class="heading"> <h5>Locations</h5></div>
                                   <div class="control"><span>933 Record(s) found </span> <a href="#"> <i class="fa fa-thumb-tack" aria-hidden="true"></i></a>  <a href="#"> <i class="fa fa-times-circle" aria-hidden="true"></i></a></div>
                               </div>






                            </div>
                        </div>
                    </div>
                   <div id="panel-element-2" class="panel-collapse collapse  location-p" >
                        <div class="panel-body" style="border:none; font-size:14px; padding-bottom:0; margin-bottom:0;">
                            <div class="location-details">
                               <div class="heading-area">
                                  <div class="heading"> <h5> Workers</h5></div>
                                   <div class="control"><span>933 Record(s) found </span> <a href="#"> <i class="fa fa-thumb-tack" aria-hidden="true"></i></a>  <a href="#"> <i class="fa fa-times-circle" aria-hidden="true"></i></a></div>
                               </div>






                            </div>
                        </div>
                    </div>
                   <div id="panel-element-3" class="panel-collapse collapse  location-p" >
                        <div class="panel-body" style="border:none; font-size:14px; padding-bottom:0; margin-bottom:0;">
                            <div class="location-details">
                               <div class="heading-area">
                                  <div class="heading"> <h5> Template</h5></div>
                                   <div class="control"><span>933 Record(s) found </span> <a href="#"> <i class="fa fa-thumb-tack" aria-hidden="true"></i></a>  <a href="#"> <i class="fa fa-times-circle" aria-hidden="true"></i></a></div>
                               </div>






                            </div>
                        </div>
                    </div>
                    <%--<div id="panel-element-2" class="panel-collapse collapse" style="background-color:#039; color:#fff; position: absolute;top: 0;left: 28px;">
                        <div class="panel-body" style="border:none; font-size:14px; padding-bottom:0; margin-bottom:0;">
                            2We work for almost all web based application, database-driven systems, mapping and geo-spatial applications, and large content managed websites
                            <br /><br /><p style="font-style:italic; font-weight:700;">Find out more</p>
                        </div>
                    </div>
                    <div id="panel-element-3" class="panel-collapse collapse" style="background-color:#039; color:#fff; position: absolute;top: 0;left: 28px;">
                        <div class="panel-body" style="border:none; font-size:14px; padding-bottom:0; margin-bottom:0;">
                            3We work for almost all web based application, database-driven systems, mapping and geo-spatial applications, and large content managed websites
                            <br /><br /><p style="font-style:italic; font-weight:700;">Find out more</p>
                        </div>
                    </div>--%>
                   
               </div>
             </div>
            <div class="col" style="width:98%">
                 s
            </div>
        </div>
   



<script type="text/javascript">
    $("#Locations").hover(
        function () {
            $('#panel-element-1').collapse('show');
        }, function () {
            $('#panel-element-1').collapse('hide');
        }
    );
    $("#Workers").hover(
        function () {
            $('#panel-element-2').collapse('show');
        }, function () {
            $('#panel-element-2').collapse('hide');
        }
    );
    $("#Template").hover(
        function () {
            $('#panel-element-3').collapse('show');
        }, function () {
            $('#panel-element-3').collapse('hide');
        }
    );
    //$(function(){
    //    $('#panel-element-1').collapse('show');
    //})
</script>

</asp:Content>

