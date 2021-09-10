<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewCalendar.aspx.vb" Inherits="eAdmin.viewCalendar" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>calender</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim back As String = "<A href=""" & dDispatcher.direct("Common/AuthorityCodeSetup", "EligibilitySetup.aspx") & """>"
        </script>
        <% Response.Write(css)%>  
		
	</HEAD>
	<body leftMargin="0" topMargin="0" rightMargin="0">
		<form id="frmCal" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="tablecol">Month:
						<asp:dropdownlist id="ddl_month" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist>&nbsp;Year:
						<asp:dropdownlist id="ddl_year" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD><asp:calendar id="Calendar1" runat="server" ForeColor="Blue" BorderColor="Black" BackColor="Linen"
							OnSelectionChanged="Calendar1_SelectionChanged" OnDayRender="Calendar1_dayrender" FirstDayOfWeek="Monday"
							Width="180px" Height="125px" CellSpacing="1" NextPrevFormat="ShortMonth" Font-Names="Verdana"
							Font-Size="8pt" BorderStyle="Solid" CellPadding="1" EnableViewState=False>
							<TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
							<DayStyle BackColor="BlanchedAlmond"></DayStyle>
							<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
							<DayHeaderStyle Font-Size="7pt" Font-Bold="True" Height="8pt" ForeColor="#C00000"></DayHeaderStyle>
							<SelectedDayStyle ForeColor="White" BackColor="#526DA0"></SelectedDayStyle>
							<TitleStyle Font-Size="10pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="RoyalBlue"></TitleStyle>
							<OtherMonthDayStyle ForeColor="Red"></OtherMonthDayStyle>
						</asp:calendar></TD>
				</TR>
				<TR>
					<TD><asp:literal id="Literal1" runat="server"></asp:literal></TD>
				</TR>
			</TABLE>
		</form>
		<script language="JavaScript"><!--

				function functionName() {
					setTimeout("checkparent()",2000); // a 2 second delay
				}

				function closeWindow() {
					self.close();
				}

				function checkparent() {
					self.onerror = closeWindow;
					parentWindow = opener.location.href;
				}
		//--></script>
	</body>
</HTML>
