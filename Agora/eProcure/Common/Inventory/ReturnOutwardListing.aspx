<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnOutwardListing.aspx.vb" Inherits="eProcure.ReturnOutwardListing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ReturnOutwardListing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <% Response.Write(Session("WheelScript"))%>
	    <% Response.Write(Session("JQuery")) %>
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            Dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
        </script> 
		<script type="text/javascript">
	    function popCalendar(val)
		    {
			    txtVal= document.getElementById(val);
			    //window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			    window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
    			
	    }
	    function PopWindow(myLoc)
	    {
		    window.open(myLoc,"Wheel","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		    return false;
	    }

	/*
	function Check()
	{
		val=document.getElementById("cboDocType").selectedIndex;
		if (val==0){
		//document.getElementById("txtCreationDate").disabled=true;
		document.getElementById("txtNo").disabled=true;
	
		}
		else
		{
		//document.getElementById("txtCreationDate").disabled=false;
		document.getElementById("txtNo").disabled=false;
		//document.getElementById("imgC").disabled=false;
	
		}
	}
	
	function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkUnInv.checked=checked;
			oform.chkInv.checked=checked;
			oform.chkPendAck.checked=checked;			
		}
		
	function Reset(){
		var oform = document.forms(0);
		oform.cboDocType.selectedIndex=3;
		oform.txtNo.value="";
		oform.txtCreationDate.value="";
		oform.txtVendorName.value="";
		checkStatus(false);
		}
	*/
		</script>
	</head>
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchROListing_tabs"))%>
			<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0" border="0">
			
			    <tr>
					<td class="linespacing1" colspan="6"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="Label1" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant GRN. Click the GRN No. to go to raise Return Outward page."></asp:label>
	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
				<td class="tableheader" width="100%" align="left" colspan="6" style="height: 19px">&nbsp;Search Criteria</td>
				</tr>
				<tr >
					<td class="tableCOL" width="15%" >&nbsp;<strong>GRN No</strong> :</td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtGRNNo" runat="server" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "5%" ></td>
					<td class="tableCOL" width= "15%" >	<strong>PO No&nbsp;</strong>: </td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtPONo" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "15%"></td>
				</tr>
				<tr>
					<td class="tableCOL" width="15%"><strong>&nbsp;Start Date</strong>&nbsp;: </td>
					<td class="tableCOL" width= "25%">
					    <asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%><asp:requiredfieldvalidator id="rfv_txt_startdate" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txt_startdate"></asp:requiredfieldvalidator></td>
					<td class="tableCOL" width= "5%"></td>
					<td class="tableCOL" width= "15%"><strong>End Date</strong>&nbsp;:</td>
					<td class="tableCOL" width= "25%"><asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%><asp:requiredfieldvalidator id="rfv_txt_enddate" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txt_enddate"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
							Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</td>
				    <td class="tableCOL" width= "15%"></td>
				</tr>				
				
				<tr >
					<td class="tableCOL" width="15%" >&nbsp;<strong>Vendor Name</strong>&nbsp;:</td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtVendorName" runat="server" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "5%" ></td>
					<td class="tableCOL" width= "15%"></td>
					<td class="tableCOL" width= "40%" colspan="2" align="right">
				        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" width="75px"></asp:button>&nbsp;
				        <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" width="75px"></asp:button>
					</td>
				</tr>
				
				
				<tr>
					<td class="emptycol"></td>
				</tr>
			</table>
			<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
				border="0" runat="server">
				<tr>
					<td class="emptycol" colspan="5" style="width: 703px"></td>
				</tr>
				<tr>
					<td colspan="5" style="width: 100%;"><asp:datagrid id="dtgGRN" runat="server" OnSortCommand="SortCommand_Click" AllowSorting="True">
							<Columns>
								<asp:TemplateColumn SortExpression="GM_GRN_NO" HeaderText="GRN No.">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkGRNNum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GM_CREATED_DATE" SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO No.">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkPONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_PO_DATE" SortExpression="POM_PO_DATE" HeaderText="PO Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="DOM_DO_NO" HeaderText="DO No.">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DOM_DO_DATE" SortExpression="DOM_DO_DATE" HeaderText="DO Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Accepted_By" SortExpression="Accepted_By" HeaderText="Accepted By">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="Status_Desc" SortExpression="Status_Desc" HeaderText="Status">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>--%>
								<asp:BoundColumn Visible="False" DataField="GM_LEVEL2_USER" SortExpression="GM_LEVEL2_USER" HeaderText="Accepted By">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</html>
