<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SalesInfo.aspx.vb" Inherits="eProcure.SalesInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Sales Information</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
				    function showHide(lnkdesc)
            {
                if (document.getElementById(lnkdesc).style.display == 'none')
                {
	                document.getElementById(lnkdesc).style.display = '';
	                 document.getElementById("Image1").src = '<%response.write(collapse_up) %>';
	               
                } 
                else 
                {
	                document.getElementById(lnkdesc).style.display = 'none';
	                document.getElementById("Image1").src = '<%response.write(collapse_down) %>';
	               
                }
            }
		function selectAll()
		{
			SelectAllG("dtgSalesTurnOver_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgSalesTurnOver_ctl02_chkAll","chkSelection");
		}
		
		function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 700px");
				//retval=window.open(filename);
								if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}

		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_SalesInfo_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Please update your company Sales Turnover.</br>Click the Add button to add new Sales Turnover."
					></asp:label>

				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				<TR>
					<TD class="tablecol" style="height: 81px">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colSpan="3" width="100%"></TD>
				</TR>
							<TR>
								<TD class="tableheader" colspan="3" width="100%" style="height: 19px">&nbsp;Previous Year Sales Area</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="width: 20%; height: 24px;" >&nbsp;<STRONG>Local/Domestic
                                    Sales (%)&nbsp; :</STRONG></TD>
								<td class="tablecol" width="80%" style="height: 24px" colspan="3" ><asp:textbox id="txtLocalSales" runat="server" MaxLength="3" CssClass="txtbox" Width="20%" ></asp:textbox>&nbsp;&nbsp;<asp:RegularExpressionValidator
                                        ID="revLocalSales" runat="server" ControlToValidate="txtLocalSales" Display="None"
                                        EnableClientScript="False" ErrorMessage="Invalid Sales Area Percentage" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>&nbsp;
                                    </td>
							</TR>
							
							<TR>
								<TD class="tablecol" style="width: 20%" >&nbsp;<STRONG>Export Sales (%)&nbsp; :</STRONG></TD>
								<td class="tablecol" width="70%" ><asp:textbox id="txtExportSales" runat="server" MaxLength="3" CssClass="txtbox" Width="20%" ></asp:textbox>&nbsp;&nbsp;&nbsp;
                                    <asp:RegularExpressionValidator ID="revExportSales" runat="server" ControlToValidate="txtExportSales"
                                        Display="None" EnableClientScript="False" ErrorMessage="Invalid Sales Area Percentage"
                                        ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
                                        </td>
                                 <td class="tablecol" width="10%" align="right">
                                        <asp:Button ID="cmd_save" runat="server" Text="Save" CssClass="Button" />
                                    </td>
							</TR>
							<tr>
							<td>
                                </td>
							
							
							
							</tr>
						</TABLE>
                        <strong></strong></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 19px">
                        <asp:ValidationSummary ID="vldSalesArea" runat="server" CssClass="errormsg" />
                    </TD>
				</TR>
			</TABLE>
			<DIV id="Hide_Add2" style="DISPLAY: none" runat="server">
                <br />
                &nbsp;</DIV>
                <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Sales TurnOver</asp:label>
                    <asp:Image ID="Image1" runat="server" ImageUrl="#" />
                    </div>
					    <div id="ItemSpec" style="display:inline"  >
			<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="emptycol" style="height: 19px">
						<P>
                            <asp:DataGrid ID="dtgSalesTurnover" runat="server" OnSortCommand="SortCommand_Click">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Delete">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All" /><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelection" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="CS_YEAR" HeaderText="Year" ReadOnly="True" SortExpression="CS_YEAR">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="30%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CS_CURRENCY_CODE" HeaderText="Currency" ReadOnly="True"
                                        SortExpression="CS_CURRENCY_CODE">
                                        <HeaderStyle Width="40%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CS_AMOUNT" HeaderText="Amount" ReadOnly="True" SortExpression="CS_AMOUNT">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" Width="40%" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>&nbsp;</P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" Style="display: none" />
                        </TD>
				</TR>
			</TABLE>
			</div> 
            
		</form>
	</body>
</HTML>
