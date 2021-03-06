<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CompanyEligibilitySetup.aspx.vb" Inherits="eAdmin.CompanyEligibilitySetup"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CompanyEligibilitySetup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim back As String = "<A href=""" & dDispatcher.direct("AuthorityCodeSetup", "EligibilitySetup.aspx") & """>"
        </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
 


		<script language="javascript">
		<!--
			function selectAll()
			{
				SelectAllG("dtgCoyEligibility__ctl2_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtgCoyEligibility__ctl2_chkAll","chkSelection");
			}
			-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;Company Eligibility Setup</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;Eligibility Information</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 180px">&nbsp;<STRONG>Vendor Name</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtVendorName" runat="server" Enabled="False" CssClass="txtbox" Width="236px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 180px">&nbsp;<STRONG>Registration Authority</STRONG>
									<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboRegAutho" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="rfv_cboRegAutho" runat="server" Display="None" ErrorMessage="Registration Authority is required."
										ControlToValidate="cboRegAutho"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 150px">&nbsp;<STRONG>Classification</STRONG>
									<asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboClassification" runat="server" Enabled="False" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="rfv_Classi" runat="server" Display="None" ErrorMessage="Classification is required."
										ControlToValidate="cboClassification"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 150px">&nbsp;<STRONG>Grade/Class</STRONG>
									<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboGrade" runat="server" Enabled="False" CssClass="ddl"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="rfv_Grade" runat="server" Display="None" ErrorMessage="Grade is required." ControlToValidate="cboGrade"></asp:requiredfieldvalidator></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD id="SS" runat="server">
						<asp:button id="cmdSave" runat="server" CssClass="button" Width="39px" Text="Save"></asp:button>
						<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD id="UU" runat="server">
						<asp:button id="cmdUpdate" runat="server" CssClass="button" Width="55px" Text="Update" Visible="False"></asp:button>
						<asp:button id="cmdReset" runat="server" CssClass="button" Width="57px" Text="Reset" CausesValidation="False"
							Visible="False"></asp:button>
						<asp:button id="cmdCancel" runat="server" CssClass="button" Width="63px" Text="Cancel" CausesValidation="False"
							Visible="False"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<tr>
					<TD><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</tr>
				<TR id="trCoyEligibility" runat="server">
					<TD class="emptycol">
						<TABLE class="alltable" id="table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="title"><asp:label id="lblCoyEligibility" runat="server" Visible="False">COMPANY ELIGIBILITY CRITERIA</asp:label></TD>
							</TR>
							<TR>
								<TD><asp:datagrid id="dtgCoyEligibility" runat="server" CssClass="grid" OnPageIndexChanged="OnPageIndexChanged_Page"
										OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
										<HeaderStyle CssClass="gridheader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="CheckBox">
												<HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderTemplate>
													<asp:CheckBox id="chkAll" runat="server" ToolTip="select / deselect All"></asp:CheckBox>
												</HeaderTemplate>
												<ItemTemplate>
													<asp:CheckBox id="chkSelection" runat="server"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="RQC_COY_ID" SortExpression="RQC_COY_ID" HeaderText="Vendor ID">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="RQC_REG_AUTHORITY" SortExpression="RQC_REG_AUTHORITY" HeaderText="Registration Authority">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="RQC_CLASSIFICATION" SortExpression="RQC_CLASSIFICATION" HeaderText="Classification">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="RQC_GRADE_ID" SortExpression="RQC_GRADE_ID" HeaderText="Grade">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="EmptyCol"></TD>
							</TR>
							<TR>
								<TD>
									<asp:button id="cmdModify" runat="server" CssClass="button" Width="55px" Text="Modify" CausesValidation="False"
										Visible="False"></asp:button>
									<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"
										Visible="False"></asp:button>
								</TD>
							</TR>
							<TR>
								<TD class="EmptyCol"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD><% Response.Write(back)%><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
