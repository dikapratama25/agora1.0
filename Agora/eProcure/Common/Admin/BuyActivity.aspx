<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BuyActivity.aspx.vb" Inherits="eProcure.BuyActivity" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Buying Activitity</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TBODY>
					<TR>
						<TD class="header"><FONT size="1">&nbsp;</FONT>Buying Activity</TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
					<TR>
						<TD class="emptycol">
							<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="550" border="0">
								<TBODY>
									<TR>
										<TD colspan="2" class="tableheader">&nbsp;Buying Activity</TD>
									</TR>
									<TR>
										<TD class="tablecol" style="WIDTH: 166px">&nbsp;<STRONG>Catalogue Category</STRONG> 
											:
										</TD>
										<TD class="tableinput">&nbsp;<asp:checkbox id="chkList" runat="server" Text="List Price"></asp:checkbox>&nbsp;</TD>
									</TR>
									<tr>
										<TD class="tablecol" style="WIDTH: 166px">&nbsp;</TD>
										<TD class="tableinput">&nbsp;<asp:checkbox id="chkDisc" runat="server" Text="Discount Price"></asp:checkbox></TD>
									</tr>
									<tr>
										<TD class="tablecol" style="WIDTH: 166px">&nbsp;</TD>
										<TD class="tableinput">&nbsp;<asp:checkbox id="chkContract" runat="server" Text="Contract Price"></asp:checkbox>&nbsp;</TD>
									</tr>
									<tr>
										<TD class="tablecol" style="WIDTH: 166px">&nbsp;</TD>
										<TD class="tableinput">&nbsp;</TD>
									</tr>
								</TBODY>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD class="emptycol">&nbsp;&nbsp;</TD>
					</TR>
					<TR>
						<TD><asp:button id="cmdSave" runat="server" Text="Save" CssClass="button"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Reset" name="cmdReset" runat="server">&nbsp;</TD>
					</TR>
				</TBODY>
			</TABLE>
		</form>
		</TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></FORM>
	</body>
</HTML>
