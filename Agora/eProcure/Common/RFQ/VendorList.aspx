<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorList.aspx.vb" Inherits="eProcure.VendorList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>VendorList</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		Dim dDispatcher As New AgoraLegacy.dispatcher
		Dim AutoCompleteCSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
		<%--Dim typeaheadname As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=VendorName")--%>
		Dim typeaheadname As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=EntVendorName")
		Dim typeaheadcode As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=VendorCode")
		</script>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(AutoCompleteCSS)%>		
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" type="text/javascript"></script>
		<% response.write(Session("AutoComplete")) %>

		<script language="javascript">
		<!--
		
		$(document).ready(function(){	  
            $("#txtVendorName").autocomplete("<% Response.write(typeaheadname) %>", {
            width: 300,
            scroll: true,                
            selectFirst: false
            });
            });

			function selectAll()
			{
				SelectAllG("dtg_vendor_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtg_vendor_ctl02_chkAll","chkSelection");
			}
			
			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<TABLE id="Table2" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<TR>
								<TD class="header"><A></A><asp:label id="lbl_title" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<tr>
							<td colspan="5"><asp:Label ID="lbl1" Text="Select and Add Vendor Companies to participates in RFQ" runat="server"></asp:Label></td>
							</tr>
							<tr>
							<td class="tableheader" colspan="5">
							Search Criteria
							</td>
							</tr>
							<TR>								
								<TD class="tablecol">
                                    <asp:Label ID="Label2" runat="server" Text="Vendor Name:"></asp:Label>
								</TD>
								<td class="tablecol">
								<asp:TextBox ID="txtVendorName" CssClass="txtbox" runat="server" style="width:300px;"></asp:TextBox>								
								</td>
							    <td align="right" class="tablecol">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" />
                                    <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" />
							    </td> 																
							</TR>
                               
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;
						<asp:table id="dt_V_com" runat="server"></asp:table></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 14px"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_vendor" runat="server" AutoGenerateColumns="False" CssClass="grid" 
							DataKeyField="CM_COY_ID">
							<Columns>
								<asp:TemplateColumn HeaderText="Add">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" ToolTip="Select/Deselect All" AutoPostBack="false"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor Company Name">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Contact Details ">
									<HeaderStyle Width="60%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_adds" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CM_COY_ID" SortExpression="CM_COY_ID" HeaderText="com_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px"><asp:button id="cmd_save" runat="server" CssClass="Button" Text="Save"></asp:button>
                        <asp:Button ID="cmd_exit" runat="server" Text="Close" CssClass="button" />
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
