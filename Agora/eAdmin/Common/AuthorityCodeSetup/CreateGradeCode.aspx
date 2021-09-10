<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreateGradeCode.aspx.vb" Inherits="eAdmin.GradeSetup"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CreateGradeCode</title>
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
				SelectAllG("dtgGrade__ctl2_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtgGrade__ctl2_chkAll","chkSelection");
			}
			-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;Create Grade Code</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="Alltable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 214px"><STRONG>&nbsp;Registration Authority</STRONG>:</TD>
								<TD class="TableCol" style="WIDTH: 157px">&nbsp;
									<asp:textbox id="txtSRegAutho" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox></TD>
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
					<TD class="TableHeader" id="GI" runat="server">&nbsp;Grade Information</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" runat="server" cellSpacing="0" cellPadding="0" width="300"
							border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 200px">&nbsp;<STRONG>Registration Authority </STRONG>
									<asp:label id="Label18" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboRegAutho" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="rfv_cboRegAutho" runat="server" ControlToValidate="cboRegAutho" ErrorMessage="Registration Authority is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 200px">&nbsp;<STRONG>Grade ID</STRONG>
									<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>
									&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtGradeID" runat="server" CssClass="txtbox" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_GradeID" runat="server" ControlToValidate="txtGradeID" ErrorMessage="Grade ID is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 200px">&nbsp;<STRONG>Grade Capacity </STRONG>
									<asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtCapacity" runat="server" CssClass="txtbox" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_Capacity" runat="server" ControlToValidate="txtCapacity" ErrorMessage="Grade Capacity is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 200px">&nbsp;<STRONG>Grade Description </STRONG>
									<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="281px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfv_Desc" runat="server" ControlToValidate="txtDesc" ErrorMessage="Grade Description is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<tr>
								<TD class="emptycol"><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
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
						<asp:button id="cmdSave" runat="server" CssClass="button" Width="60px" Text="Save"></asp:button>
						<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
						<asp:button id="cmdCancelSave" runat="server" CssClass="button" CausesValidation="False" Text="Cancel"
							Width="66px" Visible="False"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD id="UU" runat="server">
						<asp:button id="cmdUpdate" runat="server" CssClass="button" Width="60px" Text="Save" Visible="False"></asp:button>
						<asp:button id="cmdReset" runat="server" CssClass="button" Width="65px" Text="Reset" Visible="False"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdCancel" runat="server" CssClass="button" Width="66px" Text="Cancel" Visible="False"
							CausesValidation="False"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="BSave" runat="server"></TD>
				</TR>
				<tr>
					<TD id="smr" runat="server"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</tr>
				<TR>
					<TD class="title" id="GL" runat="server"><asp:label id="lblGrade" runat="server">GRADE LISTING</asp:label></TD>
				</TR>
				<TR>
					<TD id="dtg" runat="server"><asp:datagrid id="dtgGrade" runat="server" CssClass="GRID" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							OnPageIndexChanged="OnPageIndexChanged_Page">
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
								<asp:BoundColumn DataField="RG_REG_ID" SortExpression="RG_REG_ID" HeaderText="Registration Authority">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RG_GRADE_ID" SortExpression="RG_GRADE_ID" HeaderText="Grade ID">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RG_CAPACITY" SortExpression="RG_CAPACITY" HeaderText="Grade Capacity">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RG_DESC" SortExpression="RG_DESC" HeaderText="Grade Description">
									<HeaderStyle Width="25%"></HeaderStyle>
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
						<asp:button id="cmdModify" runat="server" CssClass="button" Width="66px" Text="Modify" CausesValidation="False"
							Enabled="False"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"
							Enabled="False"></asp:button>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
