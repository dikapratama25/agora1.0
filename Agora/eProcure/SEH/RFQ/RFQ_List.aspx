<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQ_List.aspx.vb" Inherits="eProcure.RFQ_List_SEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RFQ_List</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sOpen As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid=" + strPageId + "&type=INV")
		    Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
        </script>
        <% Response.Write(Session("JQuery")) %>
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--

            $(document).ready(function(){
            $('#cmdDuplicate').click(function() {
            
                if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
                {		    
                    return false;
                }
                else
                {       
                    if (document.getElementById("cmdDuplicate"))   
                        {document.getElementById("cmdDuplicate").style.display= "none";}
                    if (document.getElementById("cmdDelete"))
                        {document.getElementById("cmdDelete").style.display= "none";}
                    return true;
                }
            });
            $('#cmdDelete').click(function() {
                if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
                {		    
	                return false;
	            }
	            else
                {       
                    if (document.getElementById("cmdDuplicate"))   
                        {document.getElementById("cmdDuplicate").style.display= "none";}
                    if (document.getElementById("cmdDelete"))
                        {document.getElementById("cmdDelete").style.display= "none";}
                    return true;
                }
            });
            });
            
            function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
		    {
			    if (CheckAtLeastOne(p1,p2)== true) {
				    return true;
			    }
			    else {
				    return false;
			    }				
		    }
        
		    function cmdAddClick()
			{
				var result = confirm("Are you sure that you want to permanently delete this item(s)?", "Yes", "No");
				if(result == true)
					Form1.hidAddItem.value = "1";
				else 
					Form1.hidAddItem.value = "0";
			}
			
	
			function selectAll()
			{
				SelectAllG("dtg_VendorList_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtg_VendorList_ctl02_chkAll","chkSelection");
			}
				
			function selectAll2()
			{
				SelectAllG("dtg_Draft_ctl02_chkAll2","chkSelection2");
			}
			
			function checkChild2(id)
			{
				checkChildG(id,"dtg_Draft_ctl02_chkAll2","chkSelection2");
			}
				
			function selectAll3()
			{
				SelectAllG("dtg_Qoute_ctl02_chkAll3","chkSelection3");
			}
			
			function checkChild3(id)
			{
				checkChildG(id,"dtg_Qoute_ctl02_chkAll3","chkSelection3");
			}
			
				function selectAll4()
			{
				SelectAllG("dtg_trash_ctl02_chkAll4","chkSelection4");
			}
											
			function clear()
			{
			var oform = document.forms(0);
			alert("ok");
			oform.txt_DocNum.value="";
			oform.txt_VenName.value="";
			}
			
			function checkChild4(id)
			{
				checkChildG(id,"dtg_trash_ctl02_chkAll4","chkSelection4");
			}
			
			function PDFWindow(strPageId)
			{
				window.open('<% Response.Write(sOpen)%>' ,'Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
				
			}
			function PopWindow(myLoc)
			{
                //window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
                window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
				return false;
			}
				-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_CreateRFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">				
				<TR>
					<TD class="linespacing1" colSpan="2"></TD>
				</TR>
				<TR>
	                <TD >
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant RFQ. Select the RFQ and click the Duplicate button to duplicate the selected RFQ. Click the View Response link to view the quotation results summary page."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR>
					<TD colSpan="2">
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="TableHeader" colSpan="8">&nbsp;Search Criteria</TD>
							</TR>
							<TR class="tablecol">
								<TD class="tablecol" Width="15%"><STRONG>&nbsp;RFQ Number </STRONG>:</td>
								<TD class="tablecol" Width="20%">
									<asp:textbox id="txt_DocNum" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;&nbsp;</TD> 
								<TD class="tablecol" Width="15%"><STRONG>Vendor </STRONG>:</TD> 
								<TD class="tablecol" Width="20%">
									<asp:textbox id="txt_VenName" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;</TD> 
								</TD>
								<TD class="tablecol" Width="30%" colSpan="4"></TD>
							</TR>
							
							<TR>
								<TD class="tablecol"><STRONG>&nbsp;Start Date</STRONG>&nbsp;: </td>
								<td class="tablecol">
								    <asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%><asp:requiredfieldvalidator id="rfv_txt_startdate" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txt_startdate"></asp:requiredfieldvalidator></td>
								<td class="tablecol"><STRONG>End Date</STRONG>&nbsp;:</td>
								<td class="tablecol">
								    <asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%><asp:requiredfieldvalidator id="rfv_txt_enddate" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txt_enddate"></asp:requiredfieldvalidator>
								 	<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
									Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator></TD>								
								<TD class="tablecol" colSpan="4"></TD>
								
							</TR>
							
							<TR class="tablecol">
								<td class="tablecol"><STRONG>&nbsp;RFQ Validity </STRONG>&nbsp;:</TD>
								<td class="tablecol"><asp:checkbox id="chkValid" Text="Non-Expired" Runat="server" Checked="false"></asp:checkbox>&nbsp;</TD>
								<td class="tablecol"><asp:checkbox id="chkExpired" Text="Expired" Runat="server" Checked="false"></asp:checkbox>&nbsp;</TD>
                                <TD align="right" width="20%" colspan="6">&nbsp;
									<asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear"></asp:button></TD>
							</TR>
							
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"><asp:datagrid id="dtg_VendorList" runat="server" CssClass="grid" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="13%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId" name="hidRfqId" runat="server">
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Description">
									<HeaderStyle Width="23%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="True"   HeaderText="Creation Date">
									<HeaderStyle Width="14%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date">
									<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Vendor List(s)/Vendor(s)">
									<HeaderStyle Width="24%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="11%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status" runat="server"></asp:Label>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_Expiry_Date"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>		
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD align="left" width="70%">
   								    <asp:button id="cmdDuplicate" runat="server" CssClass="button" Text="Duplicate" Width="104px"></asp:button>
								    <asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Width="104px"></asp:button>
                                    </TD>								
							</TR>
						</TABLE>
						<asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label></TD>
				</TR>
				
				<TR>
					<TD class="emptycol" colSpan="5">
                        <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" Width="15%" />
                    </TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
