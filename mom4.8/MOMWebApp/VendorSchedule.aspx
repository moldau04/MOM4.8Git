<%@ Page Title="" Language="C#" MasterPageFile="~/mom.master" AutoEventWireup="true" Inherits="VendorSchedule" Codebehind="VendorSchedule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <style >
        @media only screen and (min-height: 895px){
            #gantt_chart{
                height:95vh !important;
            }
            #main{
                height:95vh !important;
            }
        }

        @media only screen and (max-height: 894px) and (min-height: 801px){
            #gantt_chart{
                height:94vh !important;
            }
            #main{
                height:94vh !important;
            }
        }

        @media only screen and (max-height: 800px) and (min-height: 722px) {
            #gantt_chart {
                height: 93vh !important;
            }

            #main {
                height: 93vh !important;
            }
        }

        @media only screen and (max-height: 721px) and (min-height: 657px) {
            #gantt_chart {
                height: 92vh !important;
            }

            #main {
                height: 92vh !important;
            }
        }

        @media only screen and (max-height: 657px) and (min-height: 597px) {
            #gantt_chart {
                height: 91vh !important;
            }

            #main {
                height: 91vh !important;
            }
        }

        @media only screen and (max-height: 596px) and (min-height: 553px) {
            #gantt_chart {
                height: 90vh !important;
            }

            #main {
                height: 90vh !important;
            }
        }

        @media only screen and (max-height: 552px) and (min-height: 511px) {
            #gantt_chart {
                height: 89vh !important;
            }

            #main {
                height: 89vh !important;
            }
        }

        @media only screen and (max-height: 510px) {
            #gantt_chart {
                height: 88vh !important;
            }

            #main {
                height: 88vh !important;
            }
        }
        #main > .wrapper {
            height: 100% !important;
        }
        #main > .wrapper {
            height: 100% !important;
        }
            #main > .wrapper > #content {
                height: 100% !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content" style="height:100%;width:100%">
        <div class="row" style="height:100%;width:100%; margin: 0;">
            <div id="gantt_chart">
                <iframe style="border: none;height:100%;width:100%" src="TelerikCustomGantt.aspx"></iframe>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
</asp:Content>


