<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usCommodity.aspx.vb" Inherits="eProcure.usCommodity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
		<!--					
			function selectAll()
			{
				SelectAllG("dgComm_ctl01_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dgComm_ctl01_chkAll","chkSelection");
			}			
		-->
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
				<TD class="EmptyCol" colspan="6">
				    <asp:label id="lblAction" runat="server" CssClass="lblInfo" Text=""></asp:label>					    
				</TD>
			</TR>
            <tr>
				<TD class="linespacing2" colSpan="6"></TD>
		    </TR>
			<TR>
			    <TD class="TableHeader" colSpan="6">Search Criteria</TD>
			</TR>
			<TR>
			    <TD class="TableCol" width="15%" ><STRONG>&nbsp;User Name </STRONG>: </TD>
			    <td class="TableCol" colspan="4"><asp:dropdownlist id="cboUser" runat="server" AutoPostBack="True" CssClass="txtbox" Width="320px" ></asp:dropdownlist></TD>
			    <td class="TableCol"></td>
			</TR>
			<tr><td class="rowspacing"></td></tr>
        </TABLE>
						
	    <TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="EmptyCol"><asp:datagrid id="dgComm" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
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
							<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Types">
								<HeaderStyle Width="80%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CT_ID" SortExpression="CT_ID" HeaderText="CT_ID" Visible="false">
								<HeaderStyle Width="10%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CT_ROOT_PREFIX" SortExpression="CT_ROOT_PREFIX" HeaderText="CT_ROOT_PREFIX" Visible="false">
								<HeaderStyle Width="10%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="AM_SELECTED" SortExpression="AM_SELECTED" HeaderText="Selected"  ItemStyle-HorizontalAlign="Center">
							    <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
							</asp:BoundColumn>							
						</Columns>
					</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" ><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
