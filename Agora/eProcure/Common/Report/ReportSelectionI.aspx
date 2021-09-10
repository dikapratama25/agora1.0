<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportSelectionI.aspx.vb" Inherits="eProcure.ReportSelectionI" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReportSelection</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script language="javascript">
		<!--  
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}

//Modified by Jules 2015-Mar-03 for IPP GST Stage 2A
//Zulham 08/08/2017 - Ipp Stage 3
// Added 3 more parameters: blnBillingFT, blnBillingST & blnBillingSO 		
function chkRole(blnFM,blnFO,blnIPPAdmin,blnIPPO,blnIPPOF,blnBillingAO,lnkReportName, blnBillingFT, blnBillingST, blnBillingSO)
{
	if (blnFM == "True")
	{
	    if ((lnkReportName == "IPPD0002 - Daily Online GL Entries Listing") || (lnkReportName == "IPPD0004 - Summary Invoices Pending Approval Report") || (lnkReportName == "IPPD0005 - Summary Invoices Released Report") || (lnkReportName == "IPPA0014 - Staff Claim By GL Account Code") || (lnkReportName == "IPPA0016 - Payment To Top 100 Vendor") || (lnkReportName == "Payment Advice")|| (lnkReportName == "Debit Note")|| (lnkReportName == "Debit Advice") || (lnkReportName == "Tax Invoice") || (lnkReportName == "IPPS0017 - Paid Staff Claim Report"))
	        {  
	            return; 
	         }
	 }
	else 
	{
	    if (blnFO == "True") 
	    {
	     if ((lnkReportName == "IPPD0002 - Daily Online GL Entries Listing") || (lnkReportName == "IPPD0004 - Summary Invoices Pending Approval Report") || (lnkReportName == "IPPD0005 - Summary Invoices Released Report") || (lnkReportName == "IPPA0014 - Staff Claim By GL Account Code") || (lnkReportName == "IPPA0016 - Payment To Top 100 Vendor") || (lnkReportName == "Payment Advice")|| (lnkReportName == "Debit Note")|| (lnkReportName == "Debit Advice")|| (lnkReportName == "Tax Invoice") || (lnkReportName == "IPPS0017 - Paid Staff Claim Report"))
	            {  
	                return; 
	            }
        }	            
	    else if (blnIPPAdmin == "True") 
	       {
	         if ((lnkReportName == "IPPA0012 - List Of Active Vendors Report") || (lnkReportName == "IPPA0013 - List Of Inactive Vendors Report"))	    
	             {
	                return;
	             }
	        }
	    else if ((blnIPPO == "True") && (blnIPPAdmin == "False"))
	        {
	         if ((lnkReportName == "IPPA0015 - Daily Outstanding Invoice Ageing Report By Source Department"))
	            {
	                return;
	            }
            }	            
	    else if ((blnIPPOF == "True") && (blnIPPAdmin == "False"))
	        {
	        if ((lnkReportName == "IPPA0014 - Staff Claim By GL Account Code") || (lnkReportName == "Payment Advice") || (lnkReportName == "IPPS0017 - Paid Staff Claim Report"))
	            {
	                return;
	            }
	        }
	    else if ((blnBillingAO == "True") && (blnIPPAdmin == "False"))
	        {
	        if ((lnkReportName == "Daily Billing Summary Report") || (lnkReportName == "Billing Pending Approval Report") || (lnkReportName == "Tax Invoice") )
	            {
	                return;
	            }
	        }  
        //Zulham 08/08/2017 - ipp stage 3       
        else if ((blnBillingAO == "True" || blnBillingFT == "True" || blnBillingST == "True" || blnBillingSO == "True") && (blnIPPAdmin == "False"))
	        {
	        if ((lnkReportName == "Billing Tax Invoice") || (lnkReportName == "Billing Credit Note") || (lnkReportName == "Billing Debit Note") || (lnkReportName == "Billing Debit Advice") || (lnkReportName == "Billing Credit Advice") )
	            {
	                return;
	            }
	        }      
        ////
        else{
            alert('You are not authorised to view this report!');
	        return false;
        }
	 }
	    //alert('You are not authorised to view this report!');
	    //return false;
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
