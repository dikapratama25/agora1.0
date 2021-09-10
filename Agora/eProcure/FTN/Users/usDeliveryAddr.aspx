<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usDeliveryAddr.aspx.vb" Inherits="eProcure.usDeliveryAddrFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<style>
.ButtonCSS { FONT-SIZE: 8pt; CURSOR: hand; HEIGHT: 18px; BACKGROUND-COLOR: lightgrey }
		</style>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
				
				
				function selectAll()
				{
					SelectAllG("dgAddr_ctl02_chkAll","chkSelection");
				}
						
				function checkChild(id)
				{
					checkChildG(id,"dgAddr_ctl02_chkAll","chkSelection");
				}
						
				function ResetSearch(){
					//debugger;
					Form1.txt_Code.value = "";
					Form1.txt_City.value = "";
					Form1.cbo_State.selectedIndex = 0;
					Form1.cboUser.selectedIndex = 0;
					Form1.cbo_Country.selectedIndex = 0;
				}
				/*function CheckAtLeastOneNotAll(pChkSelName){
					if (Form1.hidAll.value == '0'){
						var oform = document.forms[0];
						re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
						for (var i=0;i<oform.elements.length;i++)
						{
							var e = oform.elements[i];
							if (e.type=="checkbox"){
								if (re.test(e.name) && e.checked==true)
									return true;
							}
						}
						//alert('Please make at least one selection!');
						//return false;
					}
					else
						return true;
				}*/

		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_UserAddr_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0"  cellPadding="0" width="100%" border="0">
            <tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant address. Click the Save button to save the changes."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
				<TD class="TableHeader" colSpan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
				<TD class="TableCol" width="15%" ><STRONG>&nbsp;User Name </STRONG>: </TD>
				<td class="TableCol" colspan="4"><asp:dropdownlist id="cboUser" runat="server" AutoPostBack="True" CssClass="txtbox" Width="100%" ></asp:dropdownlist></TD>
				<td class="TableCol"></td>
				</TR>
				<tr>
				<TD class="tablecol" width="10%" >&nbsp;<STRONG>Code </STRONG>: </td>
				<TD class="tablecol" width="5%" ><asp:textbox id="txt_Code" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox></td>
				<td class="tablecol" width="1%"></td>
				<TD class="tablecol" width="10%" >&nbsp;<STRONG>City </STRONG>:</td>
				<TD class="tablecol" width="10%" ><asp:textbox id="txt_City" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox>&nbsp;<STRONG></td>
				<td class="tablecol" width="65%"></td>
				</TR>
				<tr>
				<TD class="tablecol">&nbsp;<STRONG>State </STRONG>:</td>
				<TD class="tablecol" colspan="3"><asp:dropdownlist id="cbo_State" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist></TD>
				
				<tr>
				    <td class="tablecol">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Country"></asp:Label> </strong>:</td>
				    <td class="tablecol" colspan="3"><asp:dropdownlist id="cbo_Country" AutoPostBack="True" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist></td>
				    <td class="tablecol" width="1%" colspan="2"></td>
				</tr>
				
             <TD class="tablecol" colspan="2" align="right">
                <asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
                <asp:button id="cmd_Clear" runat="server" CssClass="button" Text="Clear"></asp:button>
			</TD>
				</TR>
	</TABLE>
						
	<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD><asp:datagrid id="dgAddr" runat="server" DataKeyField="AM_ADDR_CODE">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All" Checked="True"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="AM_ADDR_CODE" SortExpression="AM_ADDR_CODE" HeaderText="Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_ADDR_LINE1" SortExpression="AM_ADDR_LINE1" HeaderText="Address">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_CITY" SortExpression="AM_CITY" HeaderText="City">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATE" SortExpression="STATE" HeaderText="State">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_POSTCODE" SortExpression="AM_POSTCODE" HeaderText="Post Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COUNTRY" SortExpression="COUNTRY" HeaderText="Country">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_SELECTED" SortExpression="AM_SELECTED" HeaderText="Selected" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
