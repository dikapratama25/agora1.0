<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GRNSearch.aspx.vb" Inherits="eProcure.GRNSearch" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GRNSearch</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
        </script> 
		<script language="javascript">
	<!--
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
	//-->
		</script>
	</HEAD>
	<body onload="Check();" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGRN_tabs"))%>
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="linespacing1"></TD>
				</TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant PO/DO/GRN."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" style="WIDTH: 771px">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 771px; HEIGHT: 24px">&nbsp;<STRONG>Document Type</STRONG>
									:&nbsp;
									<asp:dropdownlist id="cboDocType" runat="server" CssClass="DDL" AutoPostBack="False" Width="80px">
										<asp:ListItem Value="0"> --Select--
									 </asp:ListItem>
										<asp:ListItem Value="DO">DO 
									 </asp:ListItem>
										<asp:ListItem Value="PO">PO
									 </asp:ListItem>
										<asp:ListItem Value="GRN">GRN
									 </asp:ListItem>
									</asp:dropdownlist>
                                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <STRONG>No.</STRONG> :
									<asp:textbox id="txtNo" runat="server" CssClass="txtbox" Width="104px"></asp:textbox>
                                    &nbsp;&nbsp; <STRONG>&nbsp; &nbsp; &nbsp; Creation 
										Date</STRONG> :
									<asp:textbox id="txtCreationDate" runat="server" CssClass="txtbox" Width="80px" contentEditable="false"> 
									</asp:textbox>
									<A onclick="popCalendar('txtCreationDate');" href="javascript:;"><%Response.Write(CalImage) %></A>
							</TR>
							<tr>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;&nbsp;<STRONG>Vendor Name </STRONG> &nbsp;&nbsp;&nbsp;:&nbsp;
						<asp:textbox id="txtVendorName" runat="server" CssClass="txtbox" Width="128px"></asp:textbox>&nbsp;&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<table width="100%">
							<tr>
								<td style="width: 15%"><STRONG>Status</STRONG> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                    :
								</td>
								<td style="width: 10%"><asp:checkbox id="chkUnInv" runat="server" text="Uninvoiced"></asp:checkbox>&nbsp;&nbsp;
								</td>
								<td style="width: 10%"><asp:checkbox id="chkInv" runat="server" Text="Invoiced"></asp:checkbox>&nbsp;&nbsp;
								</td>
								<td id="tdAck" style="DISPLAY: inline; width: 25%;" runat="server"><asp:checkbox id="chkPendAck" runat="server" Text="Pending Acknowledgement"></asp:checkbox>&nbsp;&nbsp;
								</td>
								<td style="width: 15%"></td>
								<td align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" width="35%"></asp:button>&nbsp;<INPUT class="button" id="Button2" onclick="Reset();" type="button" value="Clear" name="cmdClear"
							runat="server">
								</td>
							</tr>
						</table>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgGRN" runat="server" OnSortCommand="SortCommand_Click" AllowSorting="True">
							<Columns>
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
								<asp:BoundColumn Visible="False" DataField="Accepted_By" SortExpression="Accepted_By" HeaderText="Accepted By">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Status_Desc" SortExpression="Status_Desc" HeaderText="Status">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="GM_LEVEL2_USER" SortExpression="GM_LEVEL2_USER" HeaderText="Accepted By">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD style="height: 24px"><asp:button id="cmdNewGRN" runat="server" CssClass="button" Text="Create GRN" Width="120px" Visible="False"></asp:button>&nbsp;
					<asp:button id="cmdAck" runat="server" CssClass="button" Width="120px" Text="Acknowledge GRN"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
