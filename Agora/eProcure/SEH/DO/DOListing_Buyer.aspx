<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DOListing_Buyer.aspx.vb" Inherits="eProcure.DOListing_Buyer" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
	    <title>DOListing_Buyer</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim calStartDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"                       
            Dim calEndDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox='+val+'&seldate= '+ txtVal.value +'")
        </script>
		<%Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--
			
		function Reset(){
			var oform = document.forms(0);
			oform.txtDoNo.value = ""
			oform.txtStartDate.value="";
			oform.txtEndDate.value="";
		}
	
		function selectAll()
		{
			SelectAllG("dtgDO_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgDO_ctl02_chkAll","chkSelection");
		}
		
	
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<% Response.Write(sOpen)%>','cal','status=no,resizable=no,width=200,height=180,left=270,top=180');			
		}				
	
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">

        <%  Response.Write(Session("w_SearchDO_tabs"))%>

                <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
					<td class="header" style="HEIGHT: 25px; width: 115px;" colspan="4"><strong><font size="1">&nbsp;</font>Delivery Order</strong></td>
				</tr>
				<tr>
					<td class="linespacing1" colspan="4"></td>
			    </tr>
			    <tr>
				    <td colspan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant DO. "
					    ></asp:label>

				    </td>
			    </tr>
                <tr>
					    <td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="tableheader" align="left" colspan="5" style="height: 20px">&nbsp;<asp:label id="lblHeader" runat="server">Search Criteria</asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="DO No :" Width="100px"></asp:Label></strong></td>
					<td class="Tableinput" style="WIDTH: 295px; HEIGHT: 19px;"><asp:textbox id="txtDoNo" runat="server" Width="120px" CssClass="txtbox"></asp:textbox></td>
					<td class="tablecol" align="left" style="height: 19px; width: 100px;"></td>
					<td class="Tableinput" style="WIDTH: 206px; HEIGHT: 19px;"></td>
					<td class="tablecol" align="left" style="height: 19px; width: 518px;" rowspan=""></td>
				</tr>				
				<tr>
					<td class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="lblStartDt" runat="server" Text="Start Date :" Width="100px"></asp:Label></strong></td>
					<td class="Tableinput" style=" width: 295px;"><asp:textbox id="txtStartDate" runat="server" CssClass="txtbox" contentEditable="false" Width="120px" ></asp:textbox><% Response.Write(calStartDate)%></td>
					<td class="tablecol" align="left" style="height: 19px; width: 100px;"><strong><asp:label id="lblEndDt" runat="server" Text="End Date :"></asp:label></strong></td>
					<td class="Tableinput" style="WIDTH: 206px;"><asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" contentEditable="false" Width="120px" ></asp:textbox><% Response.Write(calEndDate)%></td>
					<td class="tablecol" align="right" style="height: 19px; width: 518px;" rowspan="">
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtStartDate"
                            ControlToValidate="txtEndDate" Display="None" ErrorMessage="End Date must greater than or equal to Start Date "
                            Operator="GreaterThanEqual" Type="Date">*</asp:CompareValidator><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
                            <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>					
				</table>	
						<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
				<tr>
					<td class="emptycol" colspan="6" ><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label>
					<asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" Width="100%" /></td>
				</tr>
				<tr>
					<td ><asp:datagrid id="dtgDO" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="DOM_DO_NO" HeaderText="DO Number">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DOM_S_Ref_No" SortExpression="DOM_S_Ref_No" HeaderText="Our Ref. No.">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_S_Ref_Date" SortExpression="DOM_S_Ref_Date" HeaderText="Our Ref. Date">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_Created_Date" SortExpression="DOM_Created_Date" HeaderText="DO Creation Date">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_DO_Date" SortExpression="DOM_DO_Date" HeaderText="DO Submitted On">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			</table>      
		</form>
	</body>
</html>
