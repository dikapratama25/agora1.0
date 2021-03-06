<%@ Page Language="vb" AutoEventWireup="false" Codebehind="EligibilitySetup.aspx.vb" Inherits="eAdmin.EligiblitySetup"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>EligibilitySetup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<link href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"


        </script> 
        <% Response.Write(css)%>
        <% Response.Write(Session("WheelScript"))%>
        

		
		<script language="javascript" type="text/javascript" >
			function selectAll()
			{
				SelectAllG("dtgVendors__ctl2_chkAll","chkSelection");
                
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtgVendors__ctl2_chkAll","chkSelection");
			}
			
		</script>
		
		
	</HEAD>
	  
	<body MS_POSITIONING="GridLayout">
	
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;Eligibility Setup</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="Tablecol" style="WIDTH: 20%">&nbsp;<STRONG>Vendor Name</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtVendorName" runat="server" CssClass="txtbox"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="btnSearch" runat="server" CssClass="button" Text="Search" Width="56px"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" Width="67px"></asp:button></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="LV" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="LVlbl" runat="server"><asp:label id="lblListOfVendors" runat="server" Visible="False">LIST OF VENDORS</asp:label></TD>
				</TR>
				<TR>
					<TD id="dtg" runat="server"><asp:datagrid id="dtgVendors" runat="server" CssClass="grid" OnPageIndexChanged="OnPageIndexChanged_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" DataKeyField="CM_COY_ID">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="CheckBox">
									<HeaderStyle Width="5%" HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:CheckBox id="chkAll" runat="server" ToolTip="select / deselect All"></asp:CheckBox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="chkSelection" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="ES" runat="server"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdEligibilitySetup" runat="server" CssClass="button" Text="Eligibility Setup"
							Width="89px" Visible="False"></asp:button>&nbsp;<INPUT class="button" id="Reset1" type="reset" value="Reset" name="Reset1" runat="server"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="EE" runat="server"></TD>
				</TR>
				<TR>
					<td></td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
