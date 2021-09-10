<%@ Page Language="vb" AutoEventWireup="false" Codebehind="searchDO.aspx.vb" Inherits="eProcure.searchDO" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim calStartDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtCreationDate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"                       
            Dim calEndDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSubmittedDt") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox='+val+'&seldate= '+ txtVal.value +'")
        </script>
		<%Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
		function SelectAll_chk()
		{
			checkStatus(true);
		}
			
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkDraft.checked=checked;
			oform.chkSummited.checked=checked;
			oform.chkPartially.checked=checked;
			oform.chkFully.checked=checked;
			oform.chkrejected.checked=checked;
//			oform.chkInvoiced.checked=checked; 'Michelle (7/10/2010) - Combined with 'Fully Delivered'
		}
				
		function Reset(){
			var oform = document.forms(0);
			oform.cboDocType.value = "DO"
			oform.txtDocNo.value = ""
			oform.txtCreationDate.value="";
			oform.txtOurRefNo.value="";
			oform.txtBuyerComp.value="";
			oform.txtVenItemCode.value="";
			oform.txtSubmittedDt.value="";
			checkStatus(false);
			divDtesubmit1.style.display="";
			divDtesubmit.style.display="";
		}
		
//		function popCalendar(val)
//		{
	//		txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
	//		window.open('../Calendar/viewCalendar.aspx?TextBox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
	//	}
	
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
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">

        <%  Response.Write(Session("w_SearchDO_tabs"))%>

                <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				 <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			    </TR>
			    <TR>
				    <TD colSpan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant DO. "
					    ></asp:label>

				    </TD>
			    </TR>
                <tr>
					    <TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" align="left" colSpan="5" style="height: 20px">&nbsp;<asp:label id="lblHeader" runat="server">Search Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Document Type :" Width="100px"></asp:Label></strong></TD>
					<TD class="TableInput" style="WIDTH: 295px; HEIGHT: 19px;"><asp:dropdownlist id="cboDocType" runat="server" AutoPostBack="True" Width="120px" CssClass="DDL">
							<asp:ListItem Value="0"> ---Select---
						 </asp:ListItem>
							<asp:ListItem Value="DO">DO 
						 </asp:ListItem>
							<asp:ListItem Value="PO">PO
						 </asp:ListItem>
						</asp:dropdownlist>&nbsp;</TD>
					<TD class="tablecol" align="left" style="height: 19px; width: 100px;"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="No. :"></asp:Label></strong></TD>
					<TD class="TableInput" style="WIDTH: 206px; HEIGHT: 19px;"><asp:textbox id="txtDocNo" runat="server" Width="120px" CssClass="txtbox"></asp:textbox>&nbsp;<STRONG></STRONG>&nbsp;</TD>
					<TD class="tablecol" align="left" style="height: 19px; width: 518px;" rowspan=""></TD>

				</TR>				
				<TR>
					<TD class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Creation Date :" Width="100px"></asp:Label></strong></TD>
					<TD class="TableInput" style=" width: 295px;"><asp:textbox id="txtCreationDate" runat="server" CssClass="txtbox" contentEditable="false" Width="120px" ></asp:textbox> <% Response.Write(calStartDate)%>
                    </TD>
					<TD class="tablecol" align="left" style="height: 19px; width: 100px;">
					    <span id="divDtesubmit" style="DISPLAY: none" runat="server"><STRONG>
						<asp:label id="lblSubmittedDt" runat="server">Submission Date</asp:label></STRONG>
						:
					    </span>
					</TD>
					<TD class="TableInput" style="WIDTH: 206px;">
                        <span id="divDtesubmit1" style="DISPLAY: none" runat="server"><STRONG>
                            <asp:textbox id="txtSubmittedDt" runat="server" CssClass="txtbox" contentEditable="false" Width="120px" ></asp:textbox><% Response.Write(calEndDate)%>
                        </span>
                    </TD>
					<TD class="tablecol" align="left" style="height: 19px; width: 518px;" rowspan="">
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtCreationDate"
                            ControlToValidate="txtSubmittedDt" Display="None" ErrorMessage="Submission Date must greater than or equal to Creation Date "
                            Operator="GreaterThanEqual" Type="Date">*</asp:CompareValidator></TD>
				</TR>				
				<TR>
					<TD class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="Label4" runat="server" Text="Our Ref. No. :" Height="12px" Width="100px"></asp:Label></STRONG></TD>
					<TD class="TableInput" style="WIDTH: 295px; HEIGHT: 19px;"><asp:textbox id="txtOurRefNo" runat="server" CssClass="txtbox" Width="120px"></asp:textbox>&nbsp;</TD>
					<TD class="tablecol" align="left" style="height: 19px; width: 100px;"><STRONG>&nbsp;<asp:Label ID="Label5" runat="server" Text="Buyer Company :" Width="118px"></asp:Label></STRONG></TD>
					<TD class="TableInput" colSpan="2" style="WIDTH: 194px; HEIGHT: 19px;"><asp:textbox id="txtBuyerComp" runat="server" Width="120px" CssClass="txtbox"></asp:textbox>&nbsp;</TD>									
				</TR>											
				<tr>
					<TD class="tablecol" align="left" style="height: 19px; width: 115px;"><strong>&nbsp;<asp:Label ID="Label6" runat="server" Text=" Item Code :" Height="12px" Width="100px"></asp:Label></STRONG></TD>
					<TD class="TableInput" colSpan="5" style="WIDTH: 194px; HEIGHT: 19px;"><asp:textbox id="txtVenItemCode" runat="server" CssClass="txtbox" Width="120px"></asp:textbox>&nbsp;</TD>
					</tr>
				<tr>
					<td class="tablecol" align="left" style="height: 19px; width: 115px;"><STRONG>&nbsp;<asp:Label ID="Label7" runat="server" Text="Status :"></asp:Label></STRONG> 
					</td>
					<td class="TableInput" colSpan="1" style="width: 290px; height: 19px;"><asp:checkbox id="chkDraft" runat="server" text="Open" CssClass="txtbox"></asp:checkbox>&nbsp;&nbsp;
					</td>
								<td class="TableInput"colSpan="1" style="width: 100px; height: 19px;"><asp:checkbox id="chkSummited" runat="server" Text="Issued" CssClass="txtbox"></asp:checkbox>&nbsp;&nbsp;
								</td>
								<td class="TableInput"colSpan="5" style="width: 202px; height: 19px;"><asp:checkbox id="chkPartially" runat="server" Text="Partially Rejected" CssClass="txtbox"></asp:checkbox>&nbsp;&nbsp;
								</td>
				</tr>	
				<tr>
				    <td class="TableInput" colSpan="" style="width: 115px; height: 19px;">
				        <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" Width="100%" /></td>
					<td class="TableInput" colSpan="" style="width: 295px; height: 19px;"><asp:checkbox id="chkFully" runat="server" Text="Fully Delivered" CssClass="txtbox"></asp:checkbox>&nbsp;&nbsp;</td>
					<td class="TableInput" colSpan="1" style="width: 100px; height: 19px;"><asp:checkbox id="chkrejected" runat="server" Text="Fully Rejected" CssClass="txtbox"></asp:checkbox>&nbsp;&nbsp;</td>
					<td class="TableInput" colSpan="" style="width: 206px; height: 19px;">&nbsp;&nbsp;</td>
				    <td align="right" class="TableInput" style="width: 518px; height: 19px;" colSpan="" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
				        <INPUT class="button" id="cmdSelectAll" onclick="SelectAll_chk();" type="button" value="Select All"
						name="cmdSelectAll">&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></td>
				</tr>
				</Table>	
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="6" ><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="6" ><asp:datagrid id="dtgDO" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="DOM_DO_NO" HeaderText="DO Number">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DOM_S_Ref_No" SortExpression="DOM_S_Ref_No" HeaderText="Our Ref. No.">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_Created_Date" SortExpression="DOM_Created_Date" HeaderText="DO Creation Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_DO_Date" SortExpression="DOM_DO_Date" HeaderText="DO Submitted On">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkPONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_PO_Date" SortExpression="POM_PO_Date" HeaderText="PO Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_Coy_Name" SortExpression="CM_Coy_Name" HeaderText="Buyer Company">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="8%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Status_desc" SortExpression="Status_desc" HeaderText="Status">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>

              
		</form>
	</body>
</HTML>
