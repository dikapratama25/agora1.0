<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQ_Outstg_List.aspx.vb" Inherits="eProcure.RFQ_Outstg_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>RFQ_List</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sOpen As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid=" + strPageId + "&type=INV")
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
				window.open('<% Response.Write(sOpen)%>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
				
			}
				-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_CreateRFQ_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">		
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
				<TR><TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD></tr>
				<TR class="tablecol">
					<TD width="80%"><STRONG>&nbsp;RFQ Number </STRONG>:
						<asp:textbox id="txt_DocNum" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;&nbsp;
						<STRONG>Vendor </STRONG>:
						<asp:textbox id="txt_VenName" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;
					</TD>
					<TD align="right" width= "20%">&nbsp;
						<asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
			</TABLE>
		    <TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0" border="0" runat="server">				
		        <TR>
				<TD colSpan="2">
					<asp:datagrid id="dtg_VendorList" runat="server" CssClass="grid" OnSortCommand="SortCommand_Click"  OnPageIndexChanged="dtg_VendorList_Page"
						AutoGenerateColumns="False">
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
							<asp:BoundColumn Visible="False" HeaderText="rfq_no">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="RM_Expiry_Date"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
						</Columns>
					</asp:datagrid>
				</TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
			</TABLE>
			<TR>
				<TD vAlign="top" colSpan="2">
					<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
						<TR>
							<TD align="left" width="70%">
                                &nbsp;<asp:button id="cmdDuplicate" runat="server" CssClass="button" Text="Duplicate" Width="104px"></asp:button>
							    <asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Width="104px"></asp:button>
							</TD>								
						</TR>
					</TABLE>
					<asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label>
				</TD>
			</TR>
		</form>
	</body>
</HTML>
