<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreditNoteAckTrackingList.aspx.vb" Inherits="eProcure.CreditNoteAckTrackingList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>CreditNoteAckTrackingList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            Dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
            dim a as string = "<script src='" & dDispatcher.direct("Plugins/include","date.js") & "' type='text/javascript'>"
        </script> 
		<%Response.Write(a & "</script>") %>
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtCnNo.value="";
			oform.txtInvNo.value="";
			var currentTime = new Date();
			        
            oform.txtDateTo.value = Date.today().toString('dd/MM/yyyy');
            oform.txtDateFr.value = Date.today().addMonths(-6).toString('dd/MM/yyyy');
            oform.txtAckDateTo.value = Date.today().toString('dd/MM/yyyy');
            oform.txtAckDateFr.value = Date.today().addMonths(-6).toString('dd/MM/yyyy');
		}
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		-->
		</script>
	</head>
	<body onload="document.forms[0].txtCnNo.focus();">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_CnTrackingList_tabs"))%>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr>
	                <td colspan="5">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant acknowledged Credit Note."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="5"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="5">&nbsp;Search Criteria</td>
				</tr>
				<tr>
					<td class="tablecol" width="15%" style="height: 24px">&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Credit Note No."></asp:Label></strong> :</td>
					<td class="Tableinput" width="20%" style="height: 24px">
						<asp:textbox id="txtCnNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
					<td class="tablecol" width="15%" style="height: 24px"><strong><asp:Label ID="Label1" runat="server" Text="Invoice No."></asp:Label></strong> :</td>
					<td class="Tableinput" width="35%" style="height: 24px"><asp:textbox id="txtInvNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
					<td style="height: 24px">&nbsp;</td>	
				</tr>
				<tr>
					<td class="tablecol" width="15%" style="height: 24px">&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Start Date"></asp:Label></strong> :</td>
					<td class="Tableinput" width="20%" style="height: 24px">
						<asp:textbox id="txtDateFr" runat="server" CssClass="txtbox" Width="150px" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%response.write(CalImage) %></a></td>
					<td class="tablecol" width="15%" style="height: 24px"><strong><asp:Label ID="Label3" runat="server" Text="End Date"></asp:Label></strong> :</td>
					<td class="Tableinput" width="35%" style="height: 24px"><asp:textbox id="txtDateTo" runat="server" CssClass="txtbox" Width="150px" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%response.write(CalImage) %></a></td>
					<td style="height: 24px">&nbsp;</td>	
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Acknowledge Start Date</strong> :</td>
					<td class="Tableinput"><asp:textbox id="txtAckDateFr" runat="server" CssClass="txtbox" Width="150px" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtAckDateFr');" href="javascript:;"><%response.write(CalImage) %></a></td>
					<td class="tablecol"><strong>Acknowledge End Date</strong> :</td>
                    <td class="Tableinput"><asp:textbox id="txtAckDateTo" runat="server" CssClass="txtbox" Width="150px" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtAckDateTo');" href="javascript:;"><%response.write(CalImage) %></a></td>
				    <td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5">
					    <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<asp:requiredfieldvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Start Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:requiredfieldvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="End Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date should be >= Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>
						<asp:requiredfieldvalidator id="vldPayDateFr" runat="server" Display="None" ControlToValidate="txtAckDateFr" ErrorMessage="Acknowledge Start Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:requiredfieldvalidator id="vldPayDateTo" runat="server" Display="None" ControlToValidate="txtAckDateTo" ErrorMessage="Acknowledge End Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="vldPayDateFtDateTo" runat="server" ErrorMessage="Acknowledge End Date should be >= Acknowledge Start Date"
							ControlToCompare="txtAckDateFr" ControlToValidate="txtAckDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>	
						<asp:CustomValidator id="cvSearch" runat="server" ErrorMessage="At least one search criteria is required."
							Display="None" ClientValidationFunction="validateinput"></asp:CustomValidator>
					</td>
				</tr>
				<tr>				
				<td class="emptycol" colspan="4"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>					
			</table>
			<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr>
					<td colspan="5">
					    <asp:datagrid id="dtgCnList" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="CNM_CN_NO" HeaderText="Credit Note No.">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCnNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CNM_CREATED_DATE" SortExpression="CNM_CREATED_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Creation Date">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CNM_INV_NO" HeaderText="Invoice No.">
									<HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderStyle-HorizontalAlign="Left" HeaderText="Vendor Name">
									<HeaderStyle Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CNM_CURRENCY_CODE" SortExpression="CNM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CNM_CN_INDEX" SortExpression="CNM_CN_INDEX" HeaderText="Index"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
					</tr>
					<tr>
						<td class="emptycol" colspan="5"></td>
					</tr>
					<tr>
						<td class="emptycol" colspan="5">&nbsp;&nbsp;</td>
					</tr>
				</table>
		</form>
	</body>
</html>
