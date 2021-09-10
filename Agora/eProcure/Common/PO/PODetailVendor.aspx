<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PODetailVendor.aspx.vb" Inherits="eProcure.PODetailVendor" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PODetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
<!--
function confirmReject()
{	
	result=resetSummary(1,0);	
	ans=confirm("Are you sure that you want to reject this PO ?");
	//alert(ans);
	//return false;	
	if (ans){			
		return result;
		}
	else
		return false;
}

function PopWindow(myLoc)
{
	window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
	return false;
}
//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddDO_tabs"))%>
            	<TABLE class="alltable" id="tblDOHeader1" width="100%" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="4" style="height: 3px"></TD>
				</TR>
				<TR>
					<TD class="tableheader" align="left" colSpan="5" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server" Font-Bold="True" Width="273px">Purchase Order Details</asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" align="left" width="18%"><STRONG>&nbsp;PO 
							Number</STRONG> :</TD>
					<TD class="TableInput"  vAlign="top" width="30%">&nbsp;<asp:label id="lblPoNo" runat="server"></asp:label></TD>
					<td class="tablecol"> </TD>
					<TD class="tablecol" width="18%"><STRONG>&nbsp;Status</STRONG> 
						:</TD>
					<TD class="TableInput" vAlign="top" width="30%">&nbsp;<asp:label id="lblStatus" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;PO Type </STRONG>:</TD>
					<TD class="TableInput" vAlign="top">&nbsp;<asp:label id="lblPOType" runat="server"></asp:label></TD>
					<TD class="tablecol"><STRONG>&nbsp;Order Date</STRONG> :</TD>
					<TD class="TableInput" vAlign="top">&nbsp;<asp:label id="lblOrderDate" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 1px" vAlign="top" colSpan="4">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
							<TR>
								<TD class="tablecol" style="HEIGHT: 24px" vAlign="top" width="20%" height="24"><STRONG>&nbsp;CR 
										Number :</STRONG></TD>
								<TD class="TableInput" style="HEIGHT: 24px" vAlign="top" height="24">&nbsp;<asp:label id="lblCRNum" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" vAlign="top" width="20%" height="26"><STRONG>&nbsp;Associated PR<BR>
										&nbsp;Numbers</STRONG> :</TD>
								<TD class="TableInput" vAlign="top" height="26">&nbsp;<asp:label id="lblAssoPRNo" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<TR vAlign="top">
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendor" runat="server"></asp:label></TD>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Ship To</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblShipTo" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor Address</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorAddr" runat="server"></asp:label></TD>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Currency Code</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblCurrCode" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor Tel</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorTel" runat="server"></asp:label></TD>
					<TD class="tablecol"><STRONG>&nbsp;Tel</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblTel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor Fax</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorFax" runat="server"></asp:label></TD>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Fax</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblFax" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor&nbsp;Email</STRONG> :</TD>
					<TD class="tableinput" style="WIDTH: 462px" vAlign="top" width="462">&nbsp;<asp:label id="lblVendorEmail" runat="server"></asp:label></TD>
					<TD class="tablecol"><STRONG>&nbsp;Contact</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblContact" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tableinput">&nbsp;</TD>
					<TD class="tableinput" style="WIDTH: 462px" vAlign="top" width="462">&nbsp;</TD>
					<TD class="tablecol"><STRONG>&nbsp;Email</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" width="25%">&nbsp;<asp:label id="lblEmail" runat="server"></asp:label></TD>
				</TR>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;&nbsp;</td>
				</tr>
				<TR vAlign="top">
					<TD class="tablecol" style="HEIGHT: 23px"><STRONG>&nbsp;Payment Terms</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentTerm" runat="server"></asp:label></TD>
					<TD class="tablecol" style="HEIGHT: 23px"><STRONG>&nbsp;Payment Method</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentMethod" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Shipment Terms</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblshipTerm" runat="server"></asp:label></TD>
					<TD class="tablecol"><STRONG>&nbsp;Shipment Mode</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblShipMethod" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Ship Via</STRONG>&nbsp;:</TD>
					<TD class="tableinput" vAlign="top">&nbsp;
						<asp:label id="lblShipVia" runat="server"></asp:label></TD>
					<TD class="tablecol"><STRONG>&nbsp;Exchange Rate</STRONG> :</TD>
					<TD class="tableinput">&nbsp;<asp:label id="lblExcRate" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor's Remarks</STRONG> :</TD>
					<TD class="tableinput" colSpan="2">&nbsp;<asp:textbox id="txtRemark" Runat="server" CssClass="listtxtbox" Rows="3" MaxLength="1000" TextMode="MultiLine"
							Width="100%" Enabled="False"></asp:textbox></TD>
					<td class="tableinput"></td>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;File(s) attached</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" colSpan="2">&nbsp;<asp:label id="lblFileAttac" runat="server"></asp:label></TD>
					<TD class="tableinput"></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="height: 18px"></TD>
					<TD class="tableinput" colSpan="2" style="height: 18px"></TD>
					<TD class="tableinput" style="height: 18px"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD colSpan="4">
						<div id="term_con" runat="server"><A id="link_term" href="#" runat="server">Click Here</A>
							to download the Term &amp; Conditions document</div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 16px" colSpan="4"><A id="detail" href="#" runat="server">Click 
							Here</A> To View
						<asp:label id="lbltitle1" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD align="left" style="height: 24px"><asp:button id="cmd_accept" runat="server" CssClass="button" Visible="False" Text="Accept PO"
										Height="19px" width="75px"></asp:button>
									<asp:button id="cmd_reject" runat="server" CssClass="button" Visible="False" Text="Reject PO"
										Height="19px" width="75px"></asp:button>
									<INPUT class="button" id="cmd_preview" type="button" value="View PO" name="cmd_preview" style="WIDTH: 75px" runat="server">
									<asp:button id="cmd_ack" runat="server" CssClass="button" Visible="False" Text="Acknowledge"
										Height="19px" width="75px"></asp:button>
									<INPUT class="button" id="cmd_cr" style="VISIBILITY: hidden; WIDTH: 75px" type="button" value="View CR" 
										name="cmd_cr" runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"><A id="back" href="#" runat="server"><strong>&lt; Back</strong></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
