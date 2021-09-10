<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WithHoldingTax.aspx.vb" Inherits="eProcure.WithHoldingTax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Withholding Tax</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
    <script language="javascript">

        //function SelectOneOnly(objRadioButton, grdName, holdingTax, holdingRemark)
        //{

        //    var i, obj;
        //    for (i = 0; i < document.all.length; i++) {
        //        obj = document.all(i);

        //        if (obj.type == "radio") {
        //            if (objRadioButton.id.substr(0, grdName.length) == grdName)
        //                if (objRadioButton.id == obj.id)
        //                    obj.checked = true;
        //                else
        //                    obj.checked = false;
        //        }
        //    }

        //    document.Form1.hidHoldingTax.value = holdingTax + ":" + holdingRemark.replace("'", "");
        //} 

        function selectOne() {
            //alert(document.Form1.hidBudget.value);
            //var r = (eval("window.opener.document.Form1." + document.Form1.hidBudget.value));
            //alert(document.Form1.hidBudgetValue.value);
            //r.value = document.Form1.hidBudgetValue.value;

            ////ori
            //var holdingTax = document.Form1.txtWHT.value;
            //alert(holdingTax);
            //window.opener.document.getElementById(document.Form1.hidopenerID.value).value = holdingTax;
            //window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = holdingTax;
            //window.opener.document.getElementById(document.Form1.hidopenerID.value).focus();
            //opener.updatebtnURL(holdingTax, document.Form1.hidopenerHIDID.value, document.Form1.hidopenerbtn.value, "WithHoldingTax.aspx?", "WithHoldingTax.aspx?id");

            //var rdbWHTComp = document.getElementById("rdbWHTComp");
            //var rdbWHTVendor = document.getElementById("rdbWHTVendor");

            if (document.getElementById("rdbWHTComp").checked)
            {
                var holdingTax = document.Form1.txtWHT.value;      
                //holdingTax = "1:" + holdingTax   
                window.opener.document.getElementById(document.Form1.hidopenerID.value).value = holdingTax;
                window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = holdingTax;
                window.opener.document.getElementById(document.Form1.hidopenerID.value).focus();
                opener.updatebtnWithHoldTaxURL(holdingTax, document.Form1.hidopenerHIDID.value, document.Form1.hidopenerbtn.value, "WithHoldingTax.aspx?id=" + document.Form1.hidprevtax.value + "&opt=" + document.Form1.hidprevopt.value, "WithHoldingTax.aspx?id", "1");
            }
            else if (document.getElementById("rdbWHTVendor").checked)
            {
                var holdingTax = document.Form1.txtWHT.value;
                //holdingTax = "2:" + holdingTax 
                window.opener.document.getElementById(document.Form1.hidopenerID.value).value = holdingTax;
                window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = holdingTax;
                window.opener.document.getElementById(document.Form1.hidopenerID.value).focus();
                opener.updatebtnWithHoldTaxURL(holdingTax, document.Form1.hidopenerHIDID.value, document.Form1.hidopenerbtn.value, "WithHoldingTax.aspx?id=" + document.Form1.hidprevtax.value + "&opt=" + document.Form1.hidprevopt.value, "WithHoldingTax.aspx?id", "2");
            }
            else
            {
                var holdingRemark = document.Form1.txtNoWHtreason.value;
                //holdingRemark = "3:" + holdingRemark
                window.opener.document.getElementById(document.Form1.hidopenerID.value).value = holdingRemark;
                window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = holdingRemark;
                window.opener.document.getElementById(document.Form1.hidopenerID.value).focus();
                opener.updatebtnWithHoldTaxURL(holdingRemark, document.Form1.hidopenerHIDID.value, document.Form1.hidopenerbtn.value, "WithHoldingTax.aspx?id=" + document.Form1.hidprevtax.value + "&opt=" + document.Form1.hidprevopt.value, "WithHoldingTax.aspx?id", "3");
            }
            //alert("wrong")
           
            //alert(document.Form1.hidopenerHIDID.value);
            //alert(document.Form1.hidopenerbtn.value);
           
            window.close();
        }
    </script>
   <%-- <style type="text/css">
        .numerictxtbox {}
    </style>--%>
</head>
<body>
    <body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
            <table>
                <tr>
					<td class="TableHeader" colSpan="5">
                        Withholding Option
                    </td>    
				</tr>
                <tr valign="top">
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 134px;" >&nbsp;<strong>Withholding Tax</strong>&nbsp;:</td>
									<td class="tablecol" style="width: 35px" >
                                        <asp:TextBox ID="txtWHT" runat="server"  CssClass = "numerictxtbox" Enabled="false" Width="34px" ></asp:TextBox></td> 
                                        <td class="tablecol" style="width: 25px"><strong>(%)</strong></td>
                                    <td class="tablecol" >
                                        <asp:RadioButton ID="rdbWHTComp" runat="server"  Width="300px" cssclass ="rbtn td" autopostback="true"  Text="WHT applicable and payable by Company" /></td>
								</tr>
                <tr valign="top">
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px;; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height: 19px;"></td>
					               	 <td class="tablecol" style="height: 19px;">
                                       <asp:RadioButton ID="rdbWHTVendor" runat="server"  autopostback="true"  Width="282px" cssclass ="rbtn td" Text="WHT applicable and payable by Vendor" /></td>
								</tr>
                <tr valign="top">
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height:19px;"></td>
					                <td class="tablecol" style="height: 19px;">
                                        <%--Zulham 25102018--%>
                                       <asp:RadioButton ID="rdbNoWHT" runat="server" autopostback="true"  Text="Not Applicable" cssclass ="rbtn td" />
                                    </td>	
								</tr>
                <tr valign="top">
					                <td class="tablecol" style="width: 1px" ></td>
					               <td class="tablecol" style="width: 134px;"></td>
					               <td class="tablecol" style="width: 35px;"></td>
					               <td class="tablecol" ></td>
					                    <td class="tablecol" style="width: 256px" >
                                      <asp:label id = "lblwht" runat = "server" Width="280px">&nbsp;&nbsp;If no WHT, Please key in reason : </asp:label></td>
								</tr>
                <tr valign="top">
					                <td class="tablecol" style="width: 1px" ></td>
					                <td class="tablecol" style="width: 134px" ></td>
					               <td class="tablecol" style="width: 35px"></td>
					               <td class="tablecol"></td>
					                 <td class="tablecol" rowspan ="3">&nbsp;
                                      <asp:TextBox ID="txtNoWHtreason" runat="server" cssclass="txtbox2" Enabled="false" Rows = "4" MaxLength="1000" Width="98%" TextMode="MultiLine" ></asp:TextBox></td>
									
								</tr>
            </table>
            <table>
                <tr>
				        <td class="tablecol" style="height: 19px; width: 200px;"></td>
									<td class="tablecol" style="width: 226px" ></td>
					                <td class="tablecol" style="width: 1px" ></td>
					                 <td class="tablecol" rowspan ="3">&nbsp;				
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="False"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>
                        </td>
			        </tr>
            </table>
             

        <%-- <input type="hidden" id="hidHoldingTax" name="hidHoldingTax" runat="server"/>--%>
            <input type="hidden" id="hidprevopt" name="hidprevopt" runat="server"/>
            <input type="hidden" id="hidprevtax" name="hidprevtax" runat="server"/>
        <input type="hidden" id="hidopenerID" name="hidopenerID" runat="server"/>
        <input type="hidden" id="hidopenerHIDID" name="hidopenerHIDID" runat="server"/>
        <input type="hidden" id="hidopenerbtn" name="hidopenerbtn" runat="server"/>
        <input type="hidden" id="hidopenerValID" name="hidopenerValID" runat="server"/>                    			
		</form>
</body>
</html>
