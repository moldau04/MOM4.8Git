<%@ Page Title="" Language="C#" MasterPageFile="~/mom.master" AutoEventWireup="true" Inherits="AddPlanner" Codebehind="AddPlanner.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <style >
        @media only screen and (min-height: 895px){
            #gantt_chart{
                height:91vh !important;
            }
            /*#main{
                height:95vh !important;
            }*/
        }

        @media only screen and (max-height: 894px) and (min-height: 825px){
            #gantt_chart{
                height:90vh !important;
            }
            /*#main{
                height:94vh !important;
            }*/
        }

        @media only screen and (max-height: 824px) and (min-height: 698px) {
            #gantt_chart {
                height: 88vh !important;
            }

            /*#main {
                height: 88vh !important;
            }*/
        }

        @media only screen and (max-height: 697px) and (min-height: 657px) {
            #gantt_chart {
                height: 87vh !important;
            }

            /*#main {
                height: 92vh !important;
            }*/
        }

        @media only screen and (max-height: 657px) and (min-height: 597px) {
            #gantt_chart {
                height: 86vh !important;
            }

            /*#main {
                height: 91vh !important;
            }*/
        }

        @media only screen and (max-height: 596px) and (min-height: 553px) {
            #gantt_chart {
                height: 85vh !important;
            }

            /*#main {
                height: 90vh !important;
            }*/
        }

        @media only screen and (max-height: 552px) and (min-height: 511px) {
            #gantt_chart {
                height: 83vh !important;
            }

            /*#main {
                height: 89vh !important;
            }*/
        }

        @media only screen and (max-height: 510px) and (min-height: 461px) {
            #gantt_chart {
                height: 82vh !important;
            }

            /*#main {
                height: 88vh !important;
            }*/
        }
        @media only screen and (max-height: 460px) {
            #gantt_chart {
                height: 81vh !important;
            }

            /*#main {
                height: 88vh !important;
            }*/
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
    <div id="breadcrumbs-wrapper">
        <header>
            <div class="container row-color-grey">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="page-title">
                                <i class="mdi-action-trending-up"></i>&nbsp;
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Edit Planner</asp:Label>
                            </div>
                            
                            <div class="btnclosewrap">
                                <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                            </div>
                            <div class="rght-content">
                                <div class="editlabel" id="trProj" runat="server">
                                </div>
                                <div class="editlabel">
                                    <asp:Label CssClass="title_text_Name" ID="lblPlannerNo" Text="-" runat="server">ssss</asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </header>
    </div>
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


