<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DebitNoteListing.aspx.vb"
    Inherits="eProcure.DebitNoteListing" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>invoice</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
	    </script>
    <%Response.Write(Session("WheelScript"))%>

    <script language="javascript">
		<!--		
				
		function Reset()
		{
			var oform = document.forms(0);					
			oform.txtDNNo.value="";
			oform.txtInvNo.value="";
			oform.txtStartDate.value="";
			oform.txtEndDate.value="";
			oform.chk_New.checked=false;
			oform.chk_Pending.checked=false;
			oform.chk_Approved.checked=false;
			oform.chk_paid.checked=false;
		}
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}		
		
	
		-->
    </script>

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Debit_Credit_tabs"))%>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td class="linespacing1" colspan="4">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant Debit Note."></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="linespacing2" colspan="4">
                </td>
            </tr>         
            <tr>
                <td class="tableheader" style="height: 19px;" colspan="6">
                    &nbsp;Search Criteria</td>
            </tr>
            <tr>
                <td class="tableCOL" align="left" width="10%">
                    &nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Debit Note No. :"></asp:Label></strong></td>
                <td class="TableInput" width="20%">
                    <asp:TextBox ID="txtDNNo" runat="server" CssClass="TXTBOX"></asp:TextBox></td>
                <td class="tableCOL" align="left" width="10%">
                    <strong>
                        <asp:Label ID="Label2" runat="server" Text="Invoice No. :"></asp:Label></strong></td>
                <td class="TableInput" width="30%">
                    <asp:TextBox ID="txtInvNo" runat="server" CssClass="TXTBOX"></asp:TextBox><strong>&nbsp;</strong></td>
            </tr>
            <tr>
                <td class="tableCOL" align="left" width="10%">
                    &nbsp;<strong><asp:Label ID="Label3" runat="server" Text="Start Date :"></asp:Label></strong></td>
                <td class="TableInput" width="20%">
                    <asp:textbox id="txt_startdate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
                <td class="tableCOL" align="left" width="10%">
                    <strong>
                        <asp:Label ID="Label4" runat="server" Text="End Date :"></asp:Label></strong></td>
                <td class="TableInput" width="30%">
                    <asp:textbox id="txt_enddate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%>
                    <asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
                    Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>
									
            </tr>
            <tr>
                <td class="tablecol" colspan="6">
                    <table class="AllTable" id="Table2" cellspacing="0" cellpadding="0" border="0" runat="server"
                        width="100%">
                        <tr class="tablecol">
                            <td class="tableCOL" align="left" width="13%">
                                <strong>
                                    <asp:Label ID="Label11" runat="server" Text="Status :"></asp:Label></strong></td>
                            <td>
                                <asp:CheckBox ID="chk_New" runat="server" Text="New "></asp:CheckBox></td>
                            <td>
                                <asp:CheckBox ID="chk_Pending" runat="server" Text="Pending Approval"></asp:CheckBox></td>
                            <td>
                                <asp:CheckBox ID="chk_Approved" runat="server" Text="Approved"></asp:CheckBox></td>
                            <td>
                                <asp:CheckBox ID="chk_paid" runat="server" Text="Paid"></asp:CheckBox></td>
                            <td class="tableCOL" align="right">
                                <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:Button><strong>&nbsp;</strong>
                                <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button></td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                        ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                </td>
            </tr>           
            <tr>
                <td>                 
                    <tr>
                        <td class="emptycol" colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:DataGrid ID="dtg_DNList" runat="server" OnPageIndexChanged="dtg_DNList_Page"
                                AllowSorting="True" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
                                <Columns>
                                    <asp:BoundColumn SortExpression="DNM_DN_NO" HeaderText="Debit Note Number">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn SortExpression="DNM_CREATED_DATE" HeaderText="Creation Date">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn SortExpression="DNM_INV_NO" HeaderText="Invoice Number">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
                                        <HeaderStyle HorizontalAlign="Left" Width="32%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="DNM_CURRENCY_CODE" SortExpression="DNM_CURRENCY_CODE"
                                        HeaderText="Currency">
                                        <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn SortExpression="DNM_DN_TOTAL" HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Right" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid></td>
                    </tr>
                    <tr>
                        <td class="emptycol" colspan="5" style="width: 793px">
                        </td>
                    </tr>
                    <tr>
                        <td class="emptycol" colspan="5" style="width: 793px">
                            <asp:Button ID="cmd_createInv" runat="server" CssClass="button" Text="Create Invoice"
                                Width="96px" Visible="False"></asp:Button></td>
                    </tr>
                    <%--</TABLE>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
