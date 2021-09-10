<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DailyPostingFileRecoveryConfirmList.aspx.vb" Inherits="eAdmin.DailyPostingFileRecoveryConfirmList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File Recovery Confirm List</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        'Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        Dim sPaySDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtPaymentSDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ & "" align=""absBottom"" vspace=""0""></A>"
        Dim sPayEDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtPaymentEDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ & "" align=""absBottom"" vspace=""0""></A>"
        Dim sRecSDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtRecoverySDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ & "" align=""absBottom"" vspace=""0""></A>"
        Dim sRecEDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtRecoveryEDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ & "" align=""absBottom"" vspace=""0""></A>"
        </script>
         
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript"  >
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgComp<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgComp<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtPaymentSDate.value="";
			oform.txtPaymentEDate.value="";
			oform.txtRecoverySDate.value="";
			oform.txtRecoveryEDate.value="";
		}
		-->
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <% Response.Write(Session("w_FileRecovery_tabs"))%>
		<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
			<tr>
                <td class="linespacing1" colspan="6">
                </td>
            </tr>

			<tr>
				    <td class="emptycol" colspan="5">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click on Search button to list the relevant File Recovery."></asp:label>

				    </td>
			    </tr>
	          <tr>
                <td class="linespacing1" colspan="6">
                </td>
            </tr>

			<tr>
				<td class="emptycol">
					<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
						<tr>
							<td class="tableheader" colspan="5" >&nbsp;Search Criteria</td>
						</tr>
						<tr>
				            <td class="tablecol" style="width:140px;">&nbsp;<strong>Payment Start Date</strong><strong>&nbsp;</strong>:<strong></strong>
				            </td>
				            <td class="tablecol" >&nbsp;
					        <asp:textbox id="txtPaymentSDate1" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" style="width:120px;"></asp:textbox><% Response.Write(sPaySDt)%>					        
						    </td>
						     <td class="tablecol" style="width:140px;">&nbsp;<strong>Payment End Date</strong><strong>&nbsp;</strong>:<strong></strong>
				            </td>
						    <td class="tablecol" colspan="2">&nbsp;
					        <asp:textbox id="txtPaymentEDate1" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" style="width:120px;"></asp:textbox><% Response.Write(sPayEDt)%>					        
                            </td>						   
			            </tr>
			            <tr>
				            <td class="tablecol" style="width:140px;">&nbsp;<strong>Recovery Start Date</strong><strong>&nbsp;</strong>:<strong></strong>
				            </td>
				            <td class="tablecol" >&nbsp;
					        <asp:textbox id="txtRecoverySDate1" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" style="width:120px;"></asp:textbox><% Response.Write(sRecSDt)%>					        
						    </td>
						     <td class="tablecol" style="width:140px;">&nbsp;<strong>Recovery End Date</strong><strong>&nbsp;</strong>:<strong></strong>
				            </td>
						    <td class="tablecol" >&nbsp;
					        <asp:textbox id="txtRecoveryEDate1" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" style="width:120px;"></asp:textbox><% Response.Write(sRecEDt)%>					        
					        </td>
						    <td class="tablecol" align="right">
						   	<asp:button id="cmdSearch1" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" />&nbsp;													
						   </td>
			            </tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
        <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
			<tr>
				<td class="emptycol" width="100%">
				    <asp:datagrid id="dgRecoveryConfirmList" CssClass="Grid" AutoGenerateColumns="False" runat="server">
						<Columns>
	
							<asp:BoundColumn DataField="frt_gl_date" SortExpression="frt_gl_date" HeaderText="Payment Date">
								<HeaderStyle Width="15%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="frt_posted_date" SortExpression="frt_posted_date" HeaderText="Recovery Date">
								<HeaderStyle Width="15%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Number_of_Record" SortExpression="Number_of_Record" HeaderText="No. of Record" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Total_Debit" SortExpression="Total_Debit" HeaderText="Total Debit" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Total_Credit" SortExpression="Total_Credit" HeaderText="Total Credit" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>													
							<asp:BoundColumn DataField="frt_index" SortExpression="frt_index" HeaderText="Status" Visible = "false">
								<HeaderStyle Width="6%"></HeaderStyle>
							</asp:BoundColumn>
						</Columns>
					</asp:datagrid></td>
			</tr>
			<tr>
				<td class = "emptycol" colspan="2"><br/>
					<asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>

