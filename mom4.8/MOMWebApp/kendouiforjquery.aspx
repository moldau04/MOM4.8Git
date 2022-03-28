 
 <%@ Page Title="kendouiforjquery || MOM" Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="kendouiforjquery" CodeBehind="~/kendouiforjquery.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
      

    <title>Filter row</title>
    <meta charset="utf-8">
 


     <!-- add the at least required css-->    
    <!-- start of Kendo UI jQuery related CSS-->    
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.1.219/styles/kendo.common.min.css">    
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.1.219/styles/kendo.rtl.min.css">    
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.1.219/styles/kendo.default.min.css">    
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.1.219/styles/kendo.mobile.all.min.css">    
    <!-- end of Kendo UI Jquery related CSS-->  
    <!-- add the kendo ui library-->    
    <!-- start of Kendo UI jQuery Scripts-->  
    <script src="https://kendo.cdn.telerik.com/2020.1.219/js/jszip.min.js"></script>    
    <script src="https://kendo.cdn.telerik.com/2020.1.219/js/kendo.all.min.js"></script>    
    <!-- end of Kendo UI jQuery Scripts-->   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"> 
    <div class="page-title"><i class="mdi-calender"></i>&nbsp;kendoui for jquery</div>
    <div class="btnclosewrap"><a  href="Home.aspx" ><i class="mdi-content-clear"></i></a></div>
     
 
   
    <div id="grid"></div>
    <script>
        $(document).ready(function () {

            var JobID = 100;

            var WebConfig = 100;

            var Status = 100;

            $("#grid").kendoGrid({
                dataSource: {
                    type: "json",
                    transport: { 
                        read:"http://localhost:2018/api/ProjectAPI?JobID=0&Status=0&WebConfig=12"
                    },
                    schema: {
                        model: {
                            fields: {
                             
                                Assigned: { type: "string" },
                                ID: { type: "string" },
                                Comp: { type: "string" },
                                dwork: { type: "string" },
                                Cat: { type: "string" },
                                description: { type: "string" },
                                assignname: { type: "string" },
                                edate: { type: "string" },
                                Est: { type: "string" },
                                Tottime: { type: "string" },
                                Reg: { type: "string" },
                                OT: { type: "string" },
                                NT: { type: "string" },
                                DT: { type: "string" },
                                TT: { type: "string" },
                                laborexp: { type: "string" },
                                expenses: { type: "string" },
                            }
                        }
                    },
                    pageSize: 50,
                    serverPaging: true,
                    serverFiltering: true,
                },
                height: 550,
                scrollable: true,
                sortable: true,
                filterable: {
                    mode: "row"
                },
                pageable: {
                    input: true,
                    numeric: false
                },
                columns:
                    [{
                        field: "Assigned",
                        width: 225,
                        filterable: {
                            cell: {
                                showOperators: false
                            }
                        }
                    },
                    {
                        field: "ID",
                        width: 500,
                        title: "ID",
                        filterable: {
                            cell: {
                                showOperators: false
                            }
                        }
                    }, {
                        field: "Comp",
                        width: 255,
                        filterable: {
                            cell: {
                                showOperators: false
                            }
                        }
                        }

                        , {
                            field: "dwork",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }



                        , {
                            field: "Cat",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }

                        , {
                            field: "description",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "assignname",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }

                        , {
                            field: "edate",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "Est",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }



                        , {
                            field: "Tottime",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }

                        , {
                            field: "Reg",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }

                        , {
                            field: "OT",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "DT",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "NT",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "TT",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }

                        , {
                            field: "laborexp",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }


                        , {
                            field: "expenses",
                            width: 255,
                            filterable: {
                                cell: {
                                    showOperators: false
                                }
                            }
                        }
                    ]
            });
        });
    </script>
 
     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server"></asp:Content>