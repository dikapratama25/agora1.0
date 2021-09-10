<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AccCodeAudit.aspx.vb" Inherits="eProcure.AccountCodeAudit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Account Code - Audit</title>
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
		
			function Reset()
			{
			    var oform = document.forms(0);	
			    oform.txtDateFr.value="";
			    oform.txtDateTo.value="";
			    oform.txtBranchCode.value="";
			    oform.txtBranchCodeTo.value="";
			    oform.txtGLCode.value="";
			    oform.txtGLCodeTo.value="";
			    oform.txtCostCenter.value="";
			    oform.txtCostCenterTo.value="";
			    oform.txtInterfaceCode.value=""
			    oform.txtInterfaceCodeTo.value=""
			}

			function Close()
			{
	            window.close();
	         }
		-->
    </script>

</head>
<body class="body" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_AccountCode_tabs"))%>
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
            <tr class="tablecol">
                <td class="tablecol" width="15%">
                    &nbsp;<asp:Label ID="Label4" runat="server" Text="Start Date " CssClass="lbname"></asp:Label>:</td>
                <td class="tablecol" width="20%">
                    <asp:TextBox ID="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false"></asp:TextBox><% Response.Write(calStartDate)%></td>
                <td class="tableCOL" width="5%">
                </td>  
                <td class="tablecol" width="15%">
                    &nbsp;<asp:Label ID="Label40" runat="server" Text="End Date " CssClass="lbname"></asp:Label>:</td>
                <td class="tablecol" width="25%">
                    <asp:TextBox ID="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false"></asp:TextBox><% Response.Write(calEndDate)%></td>                
            </tr>
             <tr class="tablecol" width="100%">
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label11" runat="server" Text="From" CssClass="lbname"></asp:Label></strong></td>
                <td class="tableCOL">
                </td>  
                <td class="tableCOL">
                </td>                 
                <td class="tablecol">
                    <strong>&nbsp;<asp:Label ID="Label12" runat="server" Text="To"></asp:Label></strong></td>
                <td class="tableCOL">
                </td>  
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Branch Code :" CssClass="lbname"></asp:Label></td>
                <td class="Tablecol">
                    <asp:TextBox ID="txtBranchCode" Width="100%" runat="server" CssClass="txtbox"
                        Height="20px"></asp:TextBox></td>
                <td class="tableCOL">
                </td>                 
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label3" runat="server" Text="Branch Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtBranchCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label7" runat="server" Text="BR GL Code :" CssClass="lbname"></asp:Label></td>
                <td class="Tablecol">
                    <asp:TextBox ID="txtGLCode" Width="100%" runat="server" CssClass="txtbox"
                        Height="20px"></asp:TextBox></td>
                <td class="tableCOL">
                </td>                                                        
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label8" runat="server" Text="BR GL Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtGLCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Cost Center : "></asp:Label></td>
                <td class="tablecol">
                    <asp:TextBox ID="txtCostCenter" Width="100%" runat="server" CssClass="txtbox"></asp:TextBox></td>
                <td class="tableCOL">
                </td>                                                            
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Cost Center :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtCostCenterTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
            </tr>
             <tr class="tablecol">
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label9" runat="server" Text="Interface Code : "></asp:Label></td>
                <td class="tablecol">
                    <asp:TextBox ID="txtInterfaceCode" Width="100%" runat="server" CssClass="txtbox"></asp:TextBox></td>
                <td class="tableCOL">
                </td>                                                        
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label10" runat="server" Text="Interface Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtInterfaceCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
            </tr>
            <tr class="tablecol">
                <td class="tablecol">
                    &nbsp;Report Type<asp:Label ID="Label6" runat="server" CssClass="errormsg">*</asp:Label>&nbsp;:
                    
                </td>
                <td class="tablecol">
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
                <td class="tableCOL">
                </td>                                                        
                <td class="tablecol">
                    &nbsp;</td>
                <td class="tablecol" align="right">
                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Submit"></asp:Button>&nbsp;
                    <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear"
                        name="cmdClear">
                </td>
            </tr>
            <tr class="tablecol">
            </tr>
            <tr>
                <td class="emptycol" colspan="6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="emptycol" colspan="7">
                    <%--<asp:DataGrid ID="dtgInterfaceCode" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Delete">
                                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                    </asp:CheckBox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="PM_PRODUCT_INDEX" SortExpression="PM_PRODUCT_INDEX"
                                HeaderText="Item Index"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code">
                                <HeaderStyle Width="9%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="lnkItem"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
                                <HeaderStyle Width="17%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Type">
                                <HeaderStyle Width="11%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
                                <HeaderStyle Width="7%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="PM_PRODUCT_CODE" SortExpression="PM_PRODUCT_INDEX"
                                HeaderText="PRODUCT_CODE"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Status" SortExpression="PM_DELETED">
                                <HeaderStyle Width="4%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>--%>
                </td>
            </tr>
            <tr visible="false">
                <td class="emptycol" colspan="6">
                    <%--<asp:Button ID="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:Button>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
