<%@ Page Language="vb" AutoEventWireup="false" Codebehind="quoteTerm.aspx.vb" Inherits="eProcure.quoteTerm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>quoteTerm</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">Quotation Terms
					</TD>
					<td></td>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader" style="HEIGHT: 19px" colSpan="2">Quotation&nbsp; Terms</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px"><STRONG>&nbsp;Quotation 
										Validity&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG></STRONG>&nbsp; :</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_QuoteVal" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px"><STRONG>&nbsp;Contact Person</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<FONT size="3">&nbsp; 
										&nbsp;</FONT></STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_ContactPer" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px"><STRONG>&nbsp;Contact Number</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<FONT size="2">&nbsp;&nbsp;&nbsp;</FONT>
									</STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_ContNum" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px"><STRONG>&nbsp;Email 
										Address&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									</STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_Email" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px">&nbsp;<STRONG>Payment Terms</STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
									:
								</TD>
								<TD class="tableinput ">&nbsp;
									<asp:label id="lbl_pay_term" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px">&nbsp;<STRONG>Payment 
										Method&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG> &nbsp;:
								</TD>
								<TD class="tableinput ">&nbsp;
									<asp:label id="lbl_paymeth" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px; HEIGHT: 24px">&nbsp;<STRONG>Shipment 
										Terms&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG> &nbsp;:
								</TD>
								<TD class="tableinput " style="HEIGHT: 24px">&nbsp;
									<asp:label id="lbl_ship_term" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px">&nbsp;<STRONG>Shipment 
										Mode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG>&nbsp;:</TD>
								<TD class="tableinput ">&nbsp;
									<asp:label id="lbl_shipmode" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px">&nbsp;<STRONG>Remarks </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:textbox id="txt_remark" runat="server" CssClass="lblinfo" Width="186px" TextMode="MultiLine"
										Height="44px"  contentEditable="false" ></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 191px"><STRONG>&nbsp;File attachment(s) </STRONG>
									:</TD>
								<TD class="tableinput ">&nbsp;<asp:panel id="pnlAttach2" runat="server"></asp:panel></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><A onclick="window.close();" href="#"><STRONG>&lt; Back</STRONG></A>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
