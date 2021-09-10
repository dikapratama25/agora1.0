<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StaffClaimTrackingList.aspx.vb" Inherits="eProcure.StaffClaimTrackingList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>StaffClaimTrackingList</title>
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
			oform.txtScNo.value="";		
			oform.chkDraft.checked=false;
			oform.chkSubmit.checked=false;
			oform.chkPending.checked=false;
			oform.chkApproved.checked=false;
			oform.chkRejected.checked=false;
		}
		
		function CheckAll()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkDraft.checked=checked;
            oform.chkSubmit.checked=checked;
            oform.chkPending.checked=checked;
            oform.chkApproved.checked=checked;
            oform.chkRejected.checked=checked;
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
		
		-->
		</script>
	</head>
	<body onload="document.forms[0].txtScNo.focus();">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_Staff_Claim_tabs"))%>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr>
	                <td colspan="5">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant new Staff Claim."
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
					<td class="tablecol" width="15%" style="height: 24px">&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Staff Claim No."></asp:Label></strong> :</td>
					<td class="Tableinput" width="20%" style="height: 24px">
						<asp:textbox id="txtScNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
					<td class="tablecol" width="15%" style="height: 24px"></td>
					<td class="Tableinput" width="35%" style="height: 24px"></td>
					<td class="tablecol" style="height: 24px">&nbsp;</td>	
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Status</strong> :</td>
					<td class="Tableinput" colspan="3"><asp:CheckBox ID="chkDraft" runat="server" Text="Draft"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkSubmit" runat="server" Text="Submitted"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkPending" runat="server" Text="Pending Approval"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkApproved" runat="server" Text="Approved"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkRejected" runat="server" Text="Rejected"></asp:CheckBox></td>
				    <td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
								<input class="button" id="cmdSelectAll" onclick="CheckAll();" type="button" value="Select All"
									name="cmdSelectAll"/>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5"></td>
				</tr>					
			</table>
			<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr>
					<td colspan="5">
					    <asp:datagrid id="dtgScList" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="SCM_CLAIM_DOC_NO" HeaderText="Staff Claim No.">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkScNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SCM_CREATED_DATE" SortExpression="SCM_CREATED_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Creation Date">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCM_SUBMIT_DATE" SortExpression="SCM_SUBMIT_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Submitted Date">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CLAIM_AMT" SortExpression="CLAIM_AMT" HeaderText="Total Claimed Amount">
									<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign ="Right"/>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
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
