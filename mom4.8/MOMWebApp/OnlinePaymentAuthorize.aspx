<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlinePaymentAuthorize.aspx.cs" Inherits="MOMWebApp.OnlinePaymentAuthorize" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">


<!--
    This file is a standalone HTML page demonstrating usage of the Authorize.net
    Accept JavaScript library using the integrated payment information form.

    For complete documentation for the Accept JavaScript library, see
    https://developer.authorize.net/api/reference/features/acceptjs.html
-->

<head runat="server">
    <title>Sample form</title>
    <style>
        .acc-button {
            border: 0.5px solid #1C5FB1;
            color: #1C5FB1;
            padding: 5px 20px 5px 20px !important;
            border-radius: 3px;
            font-size: 0.9em;
            background-image: url(../images/accrd.gif);
            background-repeat: repeat-x;
        }

        .card-title h3 {
            margin-top: 15px;
            font-weight: 500;
            font-size: 33px;
        }

        .card-title img {
            width: 192px;
        }

        .bg-crd {
            background-color: #fff;
            padding: 27px 34px;
            /* margin-top: 62px; */
            border-radius: 11px;
            border: 1px solid #e2e2e2;
        }
    </style>
</head>

<body style="background-color: #f9f9f9;">



    <%--  <script type="text/javascript"
        src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css"
        charset="utf-8">
    </script>--%>

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous" />




    <script type="text/javascript"
        src="https://jstest.authorize.net/v3/AcceptUI.js"
        charset="utf-8">
    </script>
    <section class="tab" style="margin-top: 24px;">
        <div class="container">
            <div class="row">
                <div class="col-lg-6 m-auto bg-crd">
                    <div class="card-title text-center">
                        <img src="Design/images/logo-final-210x104.jpg" class="img-fluid" />
                        <h3>Invoice Details</h3>
                    </div>

                    <form id="paymentForm" method="post" runat="server" action="">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="editlabel">
                                        <%--<label for="txtInvoiceNo" id="lblInv" runat="server">Invoice #</label>--%>
                                        <asp:Label ID="lblInv" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lblcust" AssociatedControlID="txtCustomer">Customer Name</asp:Label>
                                    <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" class="form-control"></asp:TextBox>

                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lbltxtAddress" AssociatedControlID="txtAddress">Bill To</asp:Label>
                                    <asp:TextBox ID="txtAddress" runat="server" ToolTip="Address"
                                        TextMode="MultiLine" MaxLength="8000" CssClass="materialize-textarea pd-negate form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <%--<div class="row d-none">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label ID="lblInv" Visible="false" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label for="txtInvoiceDate" id="Label5" runat="server">Invoice Date</label>

                                    <asp:TextBox ID="txtInvoiceDate" CssClass="datepicker_mom form-control" runat="server" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lblGstTaxTotal" AssociatedControlID="lblGstTax">GST Tax</asp:Label>
                                    <asp:TextBox ID="lblGstTax" runat="server" disabled="disabled" ReadOnly="true" autocomplete="off" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>--%>

                       <%-- <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lblPstTaxTotal" AssociatedControlID="lblPstTax">PST Tax</asp:Label>
                                    <asp:TextBox ID="lblPstTax" runat="server" class="form-control" disabled="disabled" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label for="txtjobamt" id="Label3" runat="server">Pre Tax Amount</label>
                                    <asp:TextBox ID="txtjobamt" disabled="disabled" ReadOnly="true" runat="server" AutoCompleteType="None"
                                        MaxLength="15" class="form-control"></asp:TextBox>

                                </div>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="txtamount" AssociatedControlID="txtamount">Amount</asp:Label>
                                    <asp:TextBox ID="txtamnt" runat="server" class="form-control" disabled="disabled" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">

                                   <input type="hidden" name="dataDescriptor" id="dataDescriptor" />
                                   <input type="hidden" name="dataValue" id="dataValue" />
                                   <%--   <input type="hidden" name="encryptedCardData" id="encryptedCardData" />
                                    <input type="hidden" name="customerInformation" id="customerInformation" />--%>

                                    <input type="hidden" name="gatewayResponse" id="gatewayResponse" />

                                    

                                    <button type="button"
                                        class="AcceptUI btn btn-info "
                                        data-billingaddressoptions='{"show":true, "required":false}'
                                        data-apiloginid="67QzSpk3tj"
                                        data-clientkey="8L2ER98p7Tsqrq64V7Ru7qA7wXZ76pUutN7r7HxMVhv3W2Tmb2k5jvVsfS9pGn77"
                                        data-acceptuiformbtntxt="Submit"
                                        data-acceptuiformheadertxt="Bank Account Information"
                                        data-paymentoptions='{"cardCodeRequired": false, "showCreditCard": true, "showBankAccount": true, "customerProfileId": false}'
                                        data-responsehandler="responseHandler">
                                        Make Payment
                                    </button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>
    <%--<form id="paymentForm" name="paymentForm" method="post" runat="server" action="Pay.aspx?id=1">

    <input type="hidden" name="dataValue" id="dataValue" />
    <input type="hidden" name="dataDescriptor" id="dataDescriptor" />
    <input type="hidden" name="encryptedCardData" id="encryptedCardData" />
    <input type="hidden" name="customerInformation" id="customerInformation" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <button type="button"
        class="AcceptUI"
        data-billingAddressOptions='{"show":true, "required":false}' 
        data-apiLoginID="49Q6RajF" 
        data-clientKey="8LFT63czDk2Y4eZVfPc3WBukxH878Cmqf2r64ZJmkwVMBFt8zR8JD43438Adatbm"
        data-acceptUIFormBtnTxt="Submit" 
        data-acceptUIFormHeaderTxt="Card Information" 
        data-responseHandler="responseHandler">Pay
    </button>
    
</form>--%>
    <p id="successMessage">You submitted the form, good job!</p>
   

    <script type="text/javascript">

        function responseHandler(response) {
            alert('response received');
           // console.log(' result 10: ', response.opaqueData.dataValue);
            console.log(' result 10: ', JSON.stringify(response));


            if (response.messages.resultCode === "Error") {
                var i = 0;
                while (i < response.messages.message.length) {
                    console.log(
                        response.messages.message[i].code + ": " +
                        response.messages.message[i].text
                    );
                    i = i + 1;
                }
            } else {

                document.getElementById("gatewayResponse").value = JSON.stringify(response);
              
                paymentFormUpdate(response.opaqueData);
               // response.messages.text();
                
                window.alert("successfully Paid");
            }
        }


        function paymentFormUpdate(opaqueData) {
            alert("Working fine");
           document.getElementById("dataDescriptor").value = opaqueData.dataDescriptor;
            document.getElementById("dataValue").value = opaqueData.dataValue;
            //document.getElementById("encryptedCardData").value = opaqueData.encryptedCardData;
            //document.getElementById("customerInformation").value = opaqueData.customerInformation;

            

            console.log(' result 2: ', opaqueData.encryptedCardData);
            console.log(' result 3: ', opaqueData.customerInformation);

            document.getElementById("paymentForm").submit();
           // document.getElementById("paymentForm").style.display = "none";
            

            alert("Working fine 2");


        }


        ////for Bank data object
        //function sendPaymentDataToAnet() {
        //    var authData = {};
        //    authData.clientKey = "YOUR PUBLIC CLIENT KEY";
        //    authData.apiLoginID = "YOUR API LOGIN ID";

        //    var bankData = {};
        //    bankData.accountNumber = document.getElementById('accountNumber').value;
        //    bankData.routingNumber = document.getElementById('routingNumber').value;
        //    bankData.nameOnAccount = document.getElementById('nameOnAccount').value;
        //    bankData.accountType = document.getElementById('accountType').value;
        //}
    </script>

   <%--<%--Complete set--
    <script type="text/javascript">
                  function sendPaymentDataToAnet() {
                      var authData = {};
                      authData.clientKey = "YOUR PUBLIC CLIENT KEY";
                      authData.apiLoginID = "YOUR API LOGIN ID";

                      var cardData = {};
                      cardData.cardNumber = document.getElementById("cardNumber").value;
                      cardData.month = document.getElementById("expMonth").value;
                      cardData.year = document.getElementById("expYear").value;
                      cardData.cardCode = document.getElementById("cardCode").value;

                      // If using banking information instead of card information,
                      // build a bankData object instead of a cardData object.
                      //
                      // var bankData = {};
                      //     bankData.accountNumber = document.getElementById('accountNumber').value;
                      //     bankData.routingNumber = document.getElementById('routingNumber').value;
                      //     bankData.nameOnAccount = document.getElementById('nameOnAccount').value;
                      //     bankData.accountType = document.getElementById('accountType').value;

                      var secureData = {};
                      secureData.authData = authData;
                      secureData.cardData = cardData;
                      // If using banking information instead of card information,
                      // send the bankData object instead of the cardData object.
                      //
                      // secureData.bankData = bankData;

                      Accept.dispatchData(secureData, responseHandler);
                  }
    </script>--%>




</body>

</html>
