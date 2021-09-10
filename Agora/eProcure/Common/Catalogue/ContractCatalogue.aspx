<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ContractCatalogue.aspx.vb" Inherits="eProcure.ContractCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Contract Catalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		    function reloadPage()
            {
                document.all("cmdSearch").click();
            }
            
			function Reset(){
				var oform = document.forms(0);
				oform.txtCode.value="";
				oform.txtDesc.value="";
				oform.txtVendor.value="";
				oform.txtDateFr.value="";
				oform.txtDateTo.value="";
			}
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
		    
		    function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=500,height=280,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}
			
			function PopWindowCat(myLoc)
			{
				window.open(myLoc,"Wheel","width=720,height=450,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function ShowDialog(filename,height)
			{				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 600px");
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
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_ConCat_tabs"))%>
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0">
			    <tr><td class="rowspacing" colspan="6"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="<b>=></b> Step 1: Create, delete or modify Contract Catalogue.<br>Step 2: Assign item master to Contract Catalogue.<br>Step 3: Assign User to Contract Catalogue."></asp:label>					    
					</TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="tableheader" colspan="6">Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" width="14%"><strong><asp:label id="lblCodeLabel" runat="server" Text="Contract Ref. No. " CssClass="lbl"></asp:label></strong>:</TD>
					<TD class="tablecol" width="20%"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" width="11%"><STRONG><asp:label id="Label1" runat="server" Text="Description " CssClass="lbl"></asp:label></STRONG>:</TD>
					<TD class="tablecol" width="20%"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="130px"></asp:textbox></TD>
					<TD class="tablecol" width="15%"><STRONG><asp:label id="Label2" runat="server" Text="Vendor Company " CssClass="lbl"></asp:label></STRONG>:</TD>
					<TD class="tablecol" width="20%"><asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="140px"></asp:textbox></TD>
				</TR>
				<tr class="tablecol">
					<TD class="tablecol" width="14%"><STRONG><asp:label id="Label3" runat="server" Text="Start Date " CssClass="lbl"></asp:label></STRONG>:</TD>
					<TD class="tablecol" width="20%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" width="11%"><STRONG><asp:label id="Label4" runat="server" Text="End Date " CssClass="lbl"></asp:label></STRONG>:</TD>
					<TD class="tablecol" width="20%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%></TD>
					<TD class="tablecol" colspan="2" style="height: 24px">&nbsp;</TD>
				</tr>
				<tr class="tablecol">
					<td class="TableCol" colspan="6" align="right">
						<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
						<asp:comparevalidator id="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"
							Display="None" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtDateTo"></asp:comparevalidator>
				    </td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CDM_GROUP_INDEX" Width="100%">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CDM_GROUP_CODE" HeaderText="Contract Ref. No.">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label ID="lblIndex" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDM_GROUP_DESC" SortExpression="CDM_GROUP_DESC" ReadOnly="True" HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="32%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" ReadOnly="True" HeaderText="Vendor Company">
									<HeaderStyle HorizontalAlign="Left" Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_START_DATE" SortExpression="CDM_START_DATE" ReadOnly="True" HeaderText="Start Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_END_DATE" SortExpression="CDM_END_DATE" ReadOnly="True" HeaderText="End Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_S_COY_ID" ReadOnly="True" HeaderText="End Date" Visible="false">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" ></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>							
				<tr runat="server" id="trDiscount">
					<td class="emptycol" colspan="6">
						<input type="button" value="Add" id="cmdAdd" runat="server" class="button"/>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button>
						<asp:button id="cmdViewCon" runat="server" CssClass="Button" Width="120px" Text="View Contract Items"></asp:button>
						<asp:button id="cmdExtCon" runat="server" CssClass="Button" Width="120px" Text="Extend Contract"></asp:button>
						<asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none"></asp:button> 
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>