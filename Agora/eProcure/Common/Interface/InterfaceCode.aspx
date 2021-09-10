<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InterfaceCode.aspx.vb"
    Inherits="eProcure.InterfaceCode" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>InterfaceCode</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
    </script>

    <% Response.Write(Session("WheelScript"))%>
    <%response.write(Session("JQuery"))%>

    <script language="javascript">
            		
		function Reset(){
			var oform = document.forms(0);								
			oform.txtBranchCode.value="";
			oform.txtGLCode.value="";
			oform.txtCostCenter.value="";
			oform.txtInterfaceCode.value="";			
		}
    </script>

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InterfaceCode_tabs"))%>
         <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <td class="header" style="height: 3px; width: 838px;">
                </td>
            </tr>
            <tr>
                <td class="header">
                    <font size="1">
                        <asp:Label ID="lblTitle" runat="server" CssClass="header">Interface Code</asp:Label></font></td>
            </tr>
        </table>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <tr>
                    <td class="rowspacing">
                    </td>
                </tr>
                </td>
            </tr>
            <tr>
                <td class="EmptyCol" colspan="6">
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Fill in the relevant interface code for mapping and click the Save button to save the changes."></asp:Label>
                </td>
            </tr>
            <tr>
                <tr>
                    <td class="rowspacing">
                    </td>
                </tr>
                </td>
            </tr>
            <tr>
                <td class="tableheader" width="100%" align="left" colspan="6" style="height: 19px">
                    &nbsp;Search Criteria</td>
            </tr>
            <tr>
                <td class="tableCOL" width="15%">
                    &nbsp;<strong>Branch Code</strong> :</td>
                <td class="tableCOL" width="25%">
                    <asp:TextBox ID="txtBranchCode" runat="server" CssClass="TXTBOX" MaxLength="10"></asp:TextBox></td>
                <td class="tableCOL" width="5%">
                </td>
                <td class="tableCOL" width="15%">
                    <strong>BR GL Code&nbsp;</strong>:
                </td>
                <td class="tableCOL" width="25%">
                    <asp:TextBox ID="txtGLCode" runat="server" CssClass="TXTBOX" MaxLength="30"></asp:TextBox></td>
                <td class="tableCOL">
                </td>
            </tr>
            <tr>
                <td class="tableCOL" width="15%">
                    &nbsp;<strong>Cost Center</strong> :</td>
                <td class="tableCOL" width="25%">
                    <asp:TextBox ID="txtCostCenter" runat="server" CssClass="TXTBOX" MaxLength="30"></asp:TextBox></td>
                <td class="tableCOL" width="5%">
                </td>
                <td class="tableCOL" width="15%">
                    <strong>Interface Code&nbsp;</strong>:
                </td>
                <td class="tableCOL" width="25%">
                    <asp:TextBox ID="txtInterfaceCode" runat="server" CssClass="TXTBOX" MaxLength="20"></asp:TextBox>
                </td>
                <td class="tableCOL">
                </td>
            </tr>
            <tr>
                <td class="tablecol">
                </td>
                <td class="tablecol" colspan="4">
                </td>
                <td class="tablecol">
                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:Button>
                    &nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear"
                        name="cmdClear"></td>
            </tr>
            <tr>
                <td class="rowspacing">
                </td>
            </tr>
        </table>
        <table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
            border="0" runat="server">
            <tr>
                <td class="emptycol" colspan="5">
                </td>
            </tr>
            <tr>
                <td class="EmptyCol" colspan="6">
                    <asp:DataGrid ID="dtgInterfaceCode" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtgInterfaceCode_Page"
                        AutoGenerateColumns="False">
                        <Columns>                                                 
                            <asp:BoundColumn DataField="Branch Code" SortExpression="Branch Code" HeaderText="Branch Code">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="GL Code" SortExpression="GL Code" HeaderText="BR GL Code">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Cost Center" SortExpression="Cost Center" HeaderText="Cost Center">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Cost Center Description" SortExpression="Cost Center Description"
                                HeaderText="Cost Center Description"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Interface Code" SortExpression="Interface Code">
                                <HeaderStyle Width="15%" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInterfaceCodeInput" CssClass="txtbox" runat="Server" MaxLength="20"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="Acct Index" HeaderText="Account Index"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
           <%-- <tr>
                <td class="emptycol" colspan="5">
                </td>
            </tr>
            <tr>
                <td class="emptycol" colspan="5">
                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                        ShowMessageBox="True" ShowSummary="False" Width="15%" />
                </td>
            </tr>--%>
            <tr>
                <td class="emptycol">
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                    <asp:Button ID="cmd_save" Text="Save" CssClass="button" runat="server" Visible="False"></asp:Button>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
