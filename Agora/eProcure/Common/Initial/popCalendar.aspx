<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popCalendar.aspx.vb" Inherits="eProcure.calendar" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Wheel Calendar</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
		<% Response.Write(sCSS)%> 
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE BORDER="0" CELLSPACING="1" CELLPADDING="1" align="center">
				<TR>
					<TD><asp:Calendar id="Calendar1" runat="server" BorderColor="#C04000" BorderStyle="Solid" BorderWidth="2px" EnableViewState="False" PrevMonthText="&amp;lt;&amp;lt;" SelectWeekText="&amp;gt" NextMonthText="&amp;gt;&amp;gt;" FirstDayOfWeek="Sunday">
							<TodayDayStyle ForeColor="Black" BackColor="Linen"></TodayDayStyle>
							<DayStyle BackColor="Linen"></DayStyle>
							<NextPrevStyle BackColor="#FF8000"></NextPrevStyle>
							<DayHeaderStyle BackColor="PeachPuff"></DayHeaderStyle>
							<SelectedDayStyle BackColor="#FF8000"></SelectedDayStyle>
							<TitleStyle BackColor="#FF8000"></TitleStyle>
							<OtherMonthDayStyle BackColor="MistyRose"></OtherMonthDayStyle>
						</asp:Calendar>
					</TD>
				</TR>
				<TR>
					<TD align="middle"><a href="javascript:self.close()">Close</a></TD>
				</TR>
			</TABLE>
			<input type="hidden" id="control" runat="server" NAME="control">
		</form>
	</body>
</HTML>
