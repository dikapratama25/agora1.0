<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InterfaceCodeAuditTrail.aspx.vb"
    Inherits="eProcure.InterfaceCodeAudit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Interface Code - Audit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <% Response.Write(Session("JQuery")) %>

    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
        Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"          
    </script>

    <%response.write(Session("WheelScript"))%>

    <script language="javascript">
		<!--						
            
		function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
		
			function Reset(){
//				var oform = document.forms(0);
//				oform.txtVendorItemCode.value="";
//				oform.txtDesc.value="";
//				Form1.txtCommodity.value="";
//				Form1.hidCommodity.value="";
//				if( document.getElementById('chkActive') ){
//				Form1.chkActive.checked=true;}
//				if( document.getElementById('chkInActive') ){
//				Form1.chkInActive.checked=false;}
//				oform.chkSpot.checked=false;
//				oform.chkStock.checked=false;
//				oform.chkMRO.checked=false;
			}

			function Close()
			{
	            window.close();
	         }
	         
	         function Reset(){
			var oform = document.forms(0);	
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
			oform.txtBranchCode.value="";
			oform.txtGLCode.value="";
			oform.txtCostCenter.value="";
			oform.txtInterfaceCode.value="";			
		}
		-->
    </script>

</head>
<body class="body" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InterfaceCode_tabs"))%>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <td class="header" style="height: 3px; width: 838px;">
                </td>
            </tr>
            <tr>
                <td class="header">
                    <font size="1">
                        <asp:Label ID="lblTitle" runat="server" CssClass="header">Audit</asp:Label></font></td>
            </tr>
        </table>
        <div id="hidAction" style="display: inline" runat="server">
            <table>
                <tr>
                    <td class="header" colspan="4" style="height: 7px">
                    </td>
                </tr>
                <tr>
                    <td class="emptycol" align="center" colspan="6">
                        <div align="left">
                            <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text=""></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="header" colspan="6" style="height: 7px">
                    </td>
                </tr>
            </table>
        </div>
        <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <td class="tableheader" colspan="6">
                    &nbsp;Search Criteria</td>
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol" width="15%">
                    <strong>&nbsp;<asp:Label ID="Label4" runat="server" Text="Start Date " CssClass="lbl"></asp:Label></strong>:</td>
                <td class="tablecol" width="20%">
                    <asp:TextBox ID="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false"></asp:TextBox><% Response.Write(calStartDate)%></td>
                <td class="tableCOL" width="5%">
                </td>    
                <td class="tablecol" width="15%">
                    <strong>&nbsp;<asp:Label ID="Label40" runat="server" Text="End Date " CssClass="lbl"></asp:Label></strong>:</td>
                <td class="tablecol" width="25%">
                    <asp:TextBox ID="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false"></asp:TextBox><% Response.Write(calEndDate)%></td>
                <td class="tableCOL">
                </td>    
            </tr>
            <tr class="tablecol">
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Branch Code :" CssClass="lbname"></asp:Label></strong></td>
                <td class="Tablecol">
                    <asp:TextBox ID="txtBranchCode" Width="80%" runat="server" CssClass="txtbox" Height="20px"></asp:TextBox></td>
                <td class="tableCOL" width="5%">
                </td>
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="BR GL Code :"></asp:Label></strong></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtGLCode" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="tableCOL">
                </td>    
            </tr>
            <tr class="tablecol">
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Cost Center : "></asp:Label></strong></td>
                <td class="tablecol">
                    <asp:TextBox ID="txtCostCenter" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox></td>
                <td class="tableCOL" width="5%">
                </td>
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label5" runat="server" Text="Interface Code :"></asp:Label></strong></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtInterfaceCode" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="tableCOL">
                </td>    
            </tr>
            <tr class="tablecol">
                <td class="tablecol" width="15%">
                    <strong>&nbsp;Report Type</strong><asp:Label ID="Label6" runat="server" CssClass="errormsg">*</asp:Label><strong>&nbsp;</strong>:<strong>
                    </strong>
                </td>
                <td class="tablecol" width="30%">
                    <asp:DropDownList ID="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                        <asp:ListItem Selected="True">Excel</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                    </asp:DropDownList><asp:RequiredFieldValidator ID="ValReportType" runat="server"
                        ErrorMessage="Report Type is required." ControlToValidate="cboReportType" Display="None"></asp:RequiredFieldValidator>
                </td>
                <td class="tablecol" colspan="4" nowrap style="height: 20px;">
            </tr>
            <tr class="tablecol">
                <td class="tablecol" nowrap="nowrap">
                    &nbsp;</td>
                <td class="tablecol" nowrap="nowrap">
                    &nbsp;
                </td>
                <td class="tablecol">
                    &nbsp;</td>
                <td class="tablecol" colspan="3" align="right">
                    <asp:Button ID="cmdView" runat="server" CssClass="button" Text="Submit"></asp:Button>&nbsp;<input
                        class="button" id="cmdClear" type="button" value="Clear" onclick="Reset();" runat="server"
                        name="cmdClear">
                    <asp:CompareValidator ID="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual"
                        ControlToCompare="txtDateFr" Display="None" ErrorMessage="Start Date should be <= End Date."
                        ControlToValidate="txtDateTo"></asp:CompareValidator>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="emptycol" colspan="4">
                    <br>
                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" Width="600px">
                    </asp:ValidationSummary>
                </td>
            </tr>
            <tr>
                <td class="emptycol" colspan="6">
                    &nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
