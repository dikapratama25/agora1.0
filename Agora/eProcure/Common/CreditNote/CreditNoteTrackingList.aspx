<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreditNoteTrackingList.aspx.vb" Inherits="eProcure.CreditNoteTrackingList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>CreditNoteTrackingList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
        </script> 
		
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
		function Reset(){
			var oform = document.forms(0);			
			oform.txtCnNo.value="";
			oform.txtVendor.value="";
			oform.ddlCurr.selectedIndex=0; 
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
		                        Text="Fill in the search criteria and click Search button to list the relevant new Credit Note."
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
					<td class="tablecol" width="15%" style="height: 24px"><strong><asp:Label ID="Label1" runat="server" Text="Vendor Name"></asp:Label></strong> :</td>
					<td class="Tableinput" width="35%" style="height: 24px"><asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
					<td style="height: 24px">&nbsp;</td>	
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Currency</strong> :</td>
					<td class="Tableinput"><asp:DropDownList ID="ddlCurr" CssClass="ddl" Width="150px" runat="server"></asp:DropDownList></td>
					<td></td>
                    <td></td>
				    <td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5"></td>
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
								<asp:BoundColumn DataField="CNM_CN_DATE" SortExpression="CNM_CN_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Credit Note Date">
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
