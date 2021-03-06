<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreateRegAuthorityCode.aspx.vb" Inherits="eAdmin.RegAuthoritySetup"%>
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
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(Session("WheelScript"))%>
        <% Response.Write(css)%>  
		<script language="javascript">
		<!--
			function selectAll()
			{
				SelectAllG("dtgRegAutho__ctl2_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtgRegAutho__ctl2_chkAll","chkSelection");
			}
			-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;Create Registration Authority Code</TD>
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
								<TD class="TableCol" style="WIDTH: 214px"><STRONG>&nbsp;Reg. Authority Abbreviation</STRONG>:</TD>
								<TD class="TableCol" style="WIDTH: 157px">&nbsp;
									<asp:textbox id="txtSRegAuthoAbbr" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox></TD>
								<TD class="TableCol">&nbsp;
									<asp:button id="cmdSearch" runat="server" CssClass="Button" Text="Search" CausesValidation="False"></asp:button>
									<asp:button id="cmdClearSearch" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" id="BAdd" runat="server"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" id="RAI" runat="server">&nbsp;Registration Authority 
						Information</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD class="tablecol" style="WIDTH: 214px">&nbsp;<STRONG>Reg. Authority Abbreviation </STRONG>
									<asp:label id="Label18" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtRegAbbr" runat="server" CssClass="txtbox" MaxLength="10" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_txtRegAbbr" runat="server" ControlToValidate="txtRegAbbr" ErrorMessage="Registration Authority Abbreviation is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 214px">&nbsp;<STRONG>Reg. Authority Description </STRONG>
									<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtRegDesc" runat="server" CssClass="txtbox" MaxLength="30" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_txtRegDesc" runat="server" ControlToValidate="txtRegDesc" ErrorMessage="Registration Authority Description is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<tr>
								<TD class="emptycol"><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="ASave" runat="server"></TD>
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
				<TR>
					<TD class="EmptyCol" id="BSave" runat="server"></TD>
				</TR>
				<tr>
					<TD id="smr" runat="server"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</tr>
				<TR>
					<TD class="title" id="tt" runat="server"><asp:label id="lblRegAutho" runat="server" Width="249px" Visible="False">REGISTRATION AUTHORITY LISTING</asp:label></TD>
				</TR>
				<TR>
					<TD id="dtg" runat="server"><asp:datagrid id="dtgRegAutho" runat="server" CssClass="grid" OnPageIndexChanged="OnPageIndexChanged_Page"
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
								<asp:BoundColumn DataField="CODE_ABBR" SortExpression="CODE_ABBR" HeaderText="Reg. Authority Abbreviation">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CODE_DESC" SortExpression="CODE_DESC" HeaderText="Reg. Authority Description">
									<HeaderStyle Width="55%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="AAdd" runat="server"></TD>
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
