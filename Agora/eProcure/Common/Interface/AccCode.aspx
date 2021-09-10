<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AccCode.aspx.vb" Inherits="eProcure.AccCode" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Account Code</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <% Response.Write(Session("JQuery")) %>
    <% Response.Write(Session("WheelScript"))%>
    <script runat="server">
         Dim dDispatcher As New AgoraLegacy.dispatcher                 
    </script>

    <script language="javascript">
		<!--						
            
		function selectAll()
			{
				SelectAllG("dtgAccountCode_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgAccountCode_ctl02_chkAll","chkSelection");
			}
		
			function Reset(){
				var oform = document.forms(0);
				oform.txtBranchCode.value="";
				oform.txtGLCode.value="";
				Form1.txtCostCenter.value="";
				Form1.txtInterfaceCode.value="";
			    oform.txtBranchCodeTo.value="";
				oform.txtGLCodeTo.value="";
				oform.txtCostCenterTo.value="";
				oform.txtInterfaceCodeTo.value="";
				if( document.getElementById('chkSelection') ){
				Form1.chkSelection.checked=false;}
				if( document.getElementById('chkAll') ){
				Form1.chkAll.checked=false;}			
			}
            
         function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 1070px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
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
                        <asp:Label ID="lblTitle" runat="server" CssClass="header">Account Code</asp:Label></font></td>
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
                            <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" 
                            Text="Fill in the search criteria and click Search button to list the relevant mapping. Click Add to add new mapping."></asp:Label>
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
                <td class="tablecol" width="10%">
                    <strong>&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="From" CssClass="lbname"></asp:Label></strong></td>
                <td class="Tablecol" width="25%">
                    &nbsp;</td>
                <td class="Tablecol" width="5%">
                </td>
                <td class="tablecol" width="10%">
                    <strong>&nbsp;<asp:Label ID="Label6" runat="server" Text="To"></asp:Label></strong></td>
                <td class="Tablecol" width="25%" colspan="1">
                    &nbsp;
                </td>
                <td class="Tablecol">
                </td>
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol">
                    &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Branch Code :" CssClass="lbname"></asp:Label></td>
                <td class="Tablecol">
                    <asp:TextBox ID="txtBranchCode" Width="80%" runat="server" CssClass="txtbox" Height="20px"></asp:TextBox></td>
                <td class="Tablecol" width="5%">
                </td>
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label3" runat="server" Text="Branch Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtBranchCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="Tablecol">
                </td>
            </tr>
            <tr class="tablecol" width="100%">
                <td class="tablecol">
                    &nbsp;&nbsp;<asp:Label ID="Label7" runat="server" Text="BR GL Code :" CssClass="lbname"></asp:Label></td>
                <td class="Tablecol">
                    <asp:TextBox ID="txtGLCode" Width="80%" runat="server" CssClass="txtbox" Height="20px"></asp:TextBox></td>
                <td class="Tablecol" width="5%">
                </td>
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label8" runat="server" Text="BR GL Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtGLCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="Tablecol">
                </td>
            </tr>
            <tr class="tablecol">
                <td class="tablecol" nowrap>
                    &nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="Cost Center : "></asp:Label></td>
                <td class="tablecol" nowrap>
                    <asp:TextBox ID="txtCostCenter" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox></td>
                <td class="Tablecol" width="5%">
                </td>
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Cost Center :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtCostCenterTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="Tablecol">
                </td>
            </tr>
            <tr class="tablecol">
                <td class="tablecol" nowrap>
                    &nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="Interface Code : "></asp:Label></td>
                <td class="tablecol" nowrap>
                    <asp:TextBox ID="txtInterfaceCode" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox></td>
                <td class="Tablecol" width="5%">
                </td>
                <td class="tablecol">
                    &nbsp;<asp:Label ID="Label10" runat="server" Text="Interface Code :"></asp:Label></td>
                <td class="Tablecol" colspan="1">
                    <asp:TextBox ID="txtInterfaceCodeTo" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="Tablecol">
                </td>
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
                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:Button>&nbsp;
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
                <td class="emptycol" colspan="6">
                    <asp:DataGrid ID="dtgAccountCode" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtgAccountCode_Page"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Delete">
                                <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                    </asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="false" DataField="row_index" HeaderText="row_index"></asp:BoundColumn>
                         <%--   <asp:TemplateColumn Visible="True" HeaderText="Idx">
									<ItemTemplate>
										<asp:Label ID="lblrowidx" Text='<%# DataBinder.Eval(Container.DataItem,"row_index") %>' Runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>--%>							
                            <asp:BoundColumn Visible="False" DataField="AM_F_ACCT_INDEX" HeaderText=""></asp:BoundColumn>
                            <asp:BoundColumn DataField="FROMCODE" SortExpression="FROMCODE" HeaderText="From (Branch Code:BR GL Code:Cost Center:Interface Code) ">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AM_T_ACCT_INDEX" HeaderText=""></asp:BoundColumn>
                            <asp:BoundColumn DataField="TOCODE" SortExpression="TOCODE" HeaderText="To (Branch Code:BR GL Code:Cost Center:Interface Code)">
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AM_ACCT_MAP_INDEX" HeaderText=""></asp:BoundColumn>	                           
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
            <tr visible="false">
                <td class="emptycol" colspan="6">
                    <asp:Button ID="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:Button>
                    <asp:Button ID="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:Button>
                    <asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
