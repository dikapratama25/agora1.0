<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportSelection.aspx.vb" Inherits="eAdmin.ReportSelection" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReportSelection</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		</script>
		<% Response.Write(sCSS) %>
		<script language="javascript">
		<!--  
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server">Report</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader"><asp:datagrid id="dtgReport" runat="server" AllowPaging="false" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Report Listing">
									<HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkReportName"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
			<asp:hyperlink id="lnkPO" Visible="False" Runat="server">
				<STRONG>DETAIL OF PO BY COMPANY</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkPR" Visible="False" Runat="server">
				<STRONG>DETAIL OF PR BY COMPANY</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkRFQ" Visible="False" Runat="server">
				<STRONG>SUMMARY OF PO, PR AND RFQ BY COMPANY</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkOPO" Visible="False" Runat="server">
				<STRONG>OUTSTANDING PURCHASE ORDER FROM VENDOR</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkOPR" Visible="False" Runat="server">
				<STRONG>OUTSTANDING PURCHASE REQUISITION BY STOCK ITEM</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkOGRN" Visible="False" Runat="server">
				<STRONG>OUTSTANDING GOOD RECEIPT NOTE FROM VENDORS</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkMthConsumption" Visible="False" Runat="server">
				<STRONG>MONTHLY CONSUMPTION REPORT</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkInvCycle" Visible="False" Runat="server">
				<STRONG>DETAILED REPORT ON PURCHASE TO INVOICE CYCLE</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkVenList" Visible="False" Runat="server">
				<STRONG>APPROVED/NOT APPROVED VENDOR LISTING</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkOPOSEH" Visible="False" Runat="server">
				<STRONG>OUTSTANDING PURCHASE ORDER BY VENDOR - S.E.H</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkPME" Visible="False" Runat="server">
				<STRONG>PME EXPENSES - S.E.H</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkPMED" Visible="False" Runat="server">
				<STRONG>PME EXPENSES DETAIL BY SECTION - S.E.H</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkDirectCharge" Visible="False" Runat="server">
				<STRONG>DIRECT CHARGE ITEM BY DEPARTMENT - S.E.H</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkMthInv" Visible="False" Runat="server">
				<STRONG>MONTHLY INVOICE CUMULATIVE (MIP)- S.E.H</STRONG></asp:hyperlink><BR>
			<asp:hyperlink id="lnkMthPO" Visible="False" Runat="server">
				<STRONG>MONTHLY PURCHASE ORDER CUMULATIVE SPECIAL PROJECT - S.H.E</STRONG></asp:hyperlink><BR>
			<BR>
			<TABLE>
				<TR>
					<TD class="emptycol">
						<div id="back" style="DISPLAY: inline" runat="server">&nbsp;</div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
