<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DebitNotePaidTrackingList.aspx.vb" Inherits="eProcure.DebitNotePaidTrackingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>DebitNotePaidTrackingList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PrintWindow as string = dDispatcher.direct("ExtraFunc","FramePrinting.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            dim PDFWindow as string = dDispatcher.direct("ExtraFunc","GeneratePDF.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
            dim a as string = "<script src='" & dDispatcher.direct("Plugins/include","date.js") & "' type='text/javascript'>"
        </script> 
        <%Response.Write(a & "</script>") %> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
			{
				if (CheckAtLeastOne(p1,p2)== true) {
					if (resetSummary(cnt1,cnt2)==true)
						return true;
					else
						return false;
				}
				else {
					return false;
				}				
			}
			
		function selectAll()
		{
			SelectAllG("dtgInvoice_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgInvoice_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);			
			oform.txtDnNo.value="";
			oform.txtInvNo.value="";
			var currentTime = new Date();
			        
            oform.txtDateTo.value = Date.today().toString('dd/MM/yyyy');
            oform.txtDateFr.value = Date.today().addMonths(-6).toString('dd/MM/yyyy');
            oform.txtPayDateTo.value = Date.today().toString('dd/MM/yyyy');
            oform.txtPayDateFr.value = Date.today().addMonths(-6).toString('dd/MM/yyyy');
		}	
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		
		function CheckAtLeastOnePrint(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{
						var strMsg;
						strMsg = "Warning :\nClicking OK will update the status of the document to 'Printed', even though you may cancel the print job at the last minute.\nDo you wish to continue ?";
						var result = confirm(strMsg);
						if (result==true){							
							//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
							return true;
						}
						else
							return false;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}
				function popCalendar(val)
		{
			txtVal= document.getElementById(val);
						
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
		function PrintWindow(strPageId)
		{
			//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
			window.open('<%response.write(PrintWindow) %>','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		function PDFWindow(strPageId)
		{
			window.open('<%response.write(PDFWindow) %>','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		
		function CheckAtLeastOnePdf(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{						
						//window.open('../ExtraFunc/GeneratePDF.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
						//window.open('../Template/TestPrint.aspx?type=INV','Spool','status=no,toolbar=no,location=no,menu=no,scrollbars=no,width=1,height=1');
						return true;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}	
		function validateinput(oSrc, args)
			{
				//debugger;
				if ((Form1.txtDnNo.value != '') || (Form1.txtInvNo.value != '') || (Form1.txtDateFr.value != '') || (Form1.txtDateTo.value != '') || (Form1.txtPayDateTo.value != '') || (Form1.txtPayDateFr.value != '')) {
					if (((Form1.txtDateFr.value != '') && (Form1.txtDateTo.value != '')) || ((Form1.txtDateFr.value == '') && (Form1.txtDateTo.value == ''))){
						args.IsValid = true;
					}
					else {
						if (Form1.txtDateFr.value == '')
							Form1.document.getElementById("cvSearch").errormessage = 'Start Date is required.';
						else
							Form1.document.getElementById("cvSearch").errormessage = 'End Date is required.';
						args.IsValid = false;
					}
				}
				else {
					Form1.document.getElementById("cvSearch").errormessage = 'At least one search criteria is required.';
					args.IsValid = false;
				}
								
				return args.IsValid;
			}	
		-->
		</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_DnTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">				
				<tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr>
				    <td colspan="5">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant paid Debit Note."
					    ></asp:label>

				    </td>
			    </tr>
			    <tr>
					<td class="linespacing2" colspan="5"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="5">&nbsp;Search Criteria</td>
				</tr>
				<tr class="tablecol">
					<td noWrap width="15%">&nbsp;<strong>Debit Note No.</strong> :</td>
					<td width="25%"><asp:textbox id="txtDnNo" runat="server" MaxLength="50" Width="150px" CssClass="txtbox"></asp:textbox></td>
					<td noWrap width="15%">&nbsp;<strong>Invoice No.</strong> :</td>
					<td width="45%"><asp:textbox id="txtInvNo" runat="server" MaxLength="50" Width="150px" CssClass="txtbox"></asp:textbox></td>
					<td>&nbsp;</td>
				</tr>				
				<tr class="tablecol" >
				    <td width="20%">&nbsp;<strong>Start Date</strong> :</td>
				    <td><asp:textbox id="txtDateFr" runat="server" Width="125px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%response.write(CalImage) %></a>
                    </td>
                    <td>&nbsp;<strong>End Date</strong>:</td>
                    <td><asp:textbox id="txtDateTo" runat="server" Width="125px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%response.write(CalImage) %></a></td>
                    <td></td>
                 </tr>
                 <tr class="tablecol">
                    <td width="25%">&nbsp;<strong>Payment Start Date</strong>:</td>
				    <td><asp:textbox id="txtPayDateFr" runat="server" Width="125px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtPayDateFr');" href="javascript:;"><%response.write(CalImage) %></a>
					</td>
					<td width="25%">&nbsp;<strong>Payment End Date</strong>:</td>
					<td><asp:textbox id="txtPayDateTo" runat="server" Width="125px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtPayDateTo');" href="javascript:;"><%response.write(CalImage) %></a>
					</td>
					<td align="right" colspan="3"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="Button2" onclick="Reset();" type="button" value="Clear" name="cmdClear"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<asp:requiredfieldvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Start Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:requiredfieldvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="End Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date should be >= Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>
						<asp:requiredfieldvalidator id="vldPayDateFr" runat="server" Display="None" ControlToValidate="txtPayDateFr" ErrorMessage="Payment Start Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:requiredfieldvalidator id="vldPayDateTo" runat="server" Display="None" ControlToValidate="txtPayDateTo" ErrorMessage="Payment End Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="vldPayDateFtDateTo" runat="server" ErrorMessage="Payment End Date should be >= Payment Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>	
						<asp:CustomValidator id="cvSearch" runat="server" ErrorMessage="At least one search criteria is required."
							Display="None" ClientValidationFunction="validateinput"></asp:CustomValidator>
						<input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary" runat="server"/>
						<input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl" runat="server"/>
				    </td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5"></td>
				</tr>	
				<tr>				
				<td class="emptycol" colspan="4"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>					
				<tr>
					<td class="emptycol" colspan="5"><asp:datagrid id="dtgDebitNote" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="DNM_DN_NO" HeaderText="Debit Note No.">
									<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDnNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left"/>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DNM_DN_DATE" SortExpression="DNM_DN_DATE" HeaderText="Debit Note Date">
								    <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="DNM_INV_NO" HeaderText="Invoice No.">
									<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkInvNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left"/>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="20%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNM_CURRENCY_CODE" SortExpression="DNM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="10%" HorizontalAlign="Left" />
									<ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNM_PAYMENT_TERM" SortExpression="DNM_PAYMENT_TERM" HeaderText="Payment Method">
									<HeaderStyle Width="10%" HorizontalAlign="Left" />
									<ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNM_PAYMENT_DATE" SortExpression="DNM_PAYMENT_DATE" HeaderText="Approved Payment Date">
									<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>				
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>
					
				<tr>
					<td class="emptycol" colspan="4">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" Visible="false" CausesValidation="False"></asp:button>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Verify"
							CausesValidation="False" Visible="false"></asp:button>
						<asp:button id="cmdApprove" runat="server" Width="96px" CssClass="button" Text="Verify"
							CausesValidation="False" Visible="false"></asp:button>
						<asp:button id="cmdMark" runat="server" Width="80px" CssClass="button" Text="Mark As Paid" CausesValidation="False" Visible="false"></asp:button>
					    <input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
					</td>
				</tr>
			</table>
		</form>
</body>
</html>