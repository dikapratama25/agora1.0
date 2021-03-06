<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LocMapping.aspx.vb" Inherits="eAdmin.LocMapping"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CreateRegAuthorityCode</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("eRFPScript"))%>
        
		<script language="javascript">
		<!--
			function selectAll()
			{
				SelectAllG("dtgLocMapping__ctl2_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtgLocMapping__ctl2_chkAll","chkSelection");
			}
			-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">
						&nbsp;Create Location Mapping</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="Alltable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 214px"><STRONG> &nbsp;Address Code</STRONG>:</TD>
								<TD class="TableCol" style="WIDTH: 157px">&nbsp;
									<asp:textbox id="txtSAddrCode" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox></TD>
								<TD class="TableCol">&nbsp;
									<asp:button id="cmdSearch" runat="server" CssClass="Button" Text="Search" CausesValidation="False"></asp:button><asp:button id="cmdClearSearch" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="TableHeader" id="RAI" runat="server">
						&nbsp;Location Mapping&nbsp;Information</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD class="tablecol" style="WIDTH: 214px">&nbsp;<STRONG>Address Code</STRONG>
									<asp:label id="Label18" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtAddrCode" runat="server" CssClass="txtbox" MaxLength="20" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_txtAddrCode" runat="server" ControlToValidate="txtAddrCode" ErrorMessage="Address Code is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 214px; HEIGHT: 52px">
									<P>&nbsp;<STRONG>Account Code</STRONG>
										<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</P>
								</TD>
								<TD class="tableinput" style="HEIGHT: 52px">
									<P>&nbsp;
										<asp:textbox id="txtAcctCode" runat="server" CssClass="txtbox" MaxLength="30" Width="281px"></asp:textbox>&nbsp;
										<asp:requiredfieldvalidator id="rfv_txtAcctCode" runat="server" ControlToValidate="txtAcctCode" ErrorMessage="Account Code is required."
											Display="None"></asp:requiredfieldvalidator></P>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 214px; HEIGHT: 52px">
									<P>&nbsp;<STRONG>Location Code</STRONG>
										<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</P>
								</TD>
								<TD class="tableinput" style="HEIGHT: 52px">
									<P>&nbsp;
										<asp:textbox id="txtLocCode" runat="server" CssClass="txtbox" MaxLength="20" Width="281px"></asp:textbox>&nbsp;
										<asp:requiredfieldvalidator id="rfv_txtLocCode" runat="server" ControlToValidate="txtLocCode" ErrorMessage="Location Code is required."
											Display="None"></asp:requiredfieldvalidator></P>
								</TD>
							</TR>
							<tr>
								<TD class="emptycol"><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
							</tr>
						</TABLE>
						<P>&nbsp;</P>
					</TD>
				</TR>
				<TR>
					<TD id="SS" runat="server">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" Width="61px"></asp:button>
						<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
						<asp:button id="cmdCancelSave" runat="server" CssClass="button" CausesValidation="False" Text="Cancel"
							Width="67px" Visible="False"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD id="UU" runat="server">
						<asp:button id="cmdUpdate" runat="server" CssClass="button" Text="Save" Width="62px" Visible="False"></asp:button>
						<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset" Width="60px" CausesValidation="False"
							Visible="False"></asp:button>
						<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" Width="67px" CausesValidation="False"
							Visible="False"></asp:button>
					</TD>
				</TR>
				<tr>
					<TD id="smr" runat="server"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</tr>
				<TR>
					<TD class="title" id="tt" runat="server"><asp:label id="lblRegAutho" runat="server" Width="249px" Visible="False">LOCATION MAPPING LISTING</asp:label></TD>
				</TR>
				<TR>
					<TD id="dtg" runat="server"><asp:datagrid id="dtgLocMapping" runat="server" CssClass="grid" OnPageIndexChanged="OnPageIndexChanged_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Checkbox">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:CheckBox id="chkAll" runat="server" ToolTip="select / deselect All"></asp:CheckBox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="chkSelection" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="LM_ADDR_CODE" SortExpression="LM_ADDR_CODE" HeaderText="Address Code">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_ACCT_CODE" SortExpression="LM_ACCT_CODE" HeaderText="Account Code">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_ADDR_MAPPING" SortExpression="LM_ADDR_MAPPING" HeaderText="Location Code">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_LOC_INDEX" HeaderText="Location Index" Visible="False">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD>
						<asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" Width="69px" CausesValidation="False"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Width="63px" CausesValidation="False"
							Enabled="False"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Width="69px" CausesValidation="False"
							Enabled="False"></asp:button>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
