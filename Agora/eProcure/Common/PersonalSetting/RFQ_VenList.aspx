<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQ_VenList.aspx.vb" Inherits="eProcure.RFQ_VenList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Vendor List Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            
        </script>
        <%-- Response.Write(css)--%> 		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		function selectAll()
		{
			SelectAllG("dtgVendList_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgVendList_ctl02_chkAll","chkSelection");
		}
		
		
		function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 700px");
				//retval=window.open(filename);
								if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		
		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("hide");
				var cmd_delete = document.getElementById("cmd_delete");
				var hidMode = document.getElementById("hidMode");
				var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				if (check==1)
				{
				
				
				cmd_delete.style.display = "none";
				Form1.hidMode.value = 'm';
				add_mod.value='add';
				}
				
				
				else if (check==0)
				{
				
				cmd_delete.style.display = "none";
				Form1.hidMode.value = 'a';
				
				}
				
			}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		    
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px"><FONT size="1"></FONT>
                        <asp:Label ID="Label2" runat="server" Text="Vendor List (RFQ) Maintenance"></asp:Label></TD>
				</TR>
		        <tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
			    </TR>
			    <TR>
					<TD align="center">
						<div align="left"><asp:label id="Label1" runat="server"  CssClass="lblInfo"
						Text="Fill in the search criteria and click Search button to list the relevant vendor list. Click the Add button to add new vendor list."></asp:label>
                        </div>
					</TD>
				</TR>
				 <tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
			    </TR>
				<TR>
					<TD>
					<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
					<TR>
								<TD class="tableheader" colSpan="2">&nbsp;Search Criteria :</TD>
							</TR>
			
				<TR class="tablecol">
 
                     <TD>&nbsp;<STRONG>List Name </STRONG>:
                    <asp:textbox id="txtSearch" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox>
									
                   
						</td>
				   
				    <td class="tablecol" align="right">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
				    <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button>
				    </td>
				    </td>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				</Table>
				</td>
				</tr>
				<%--<TR>
					<TD>
						<DIV id="hide" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader">&nbsp;<asp:label id="lblAddVendor" Runat="server"></asp:label>&nbsp;
									</TD>
								</TR>
								<TR>
									<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<STRONG>List Name</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txtAddVendor" runat="server" MaxLength="50" CssClass="txtbox" Width="128px"></asp:textbox>&nbsp;<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button>&nbsp;<asp:button id="cmdCancel" runat="server" CssClass="button" CausesValidation="False" Text="Cancel"></asp:button></TD>
								</TR>
								<TR>
									<TD class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field&nbsp;
										<asp:requiredfieldvalidator id="vldVenList" runat="server" ErrorMessage="List Name is required." ControlToValidate="txtAddVendor"
											Display="None"></asp:requiredfieldvalidator></TD>
								</TR>
								<TR>
									<TD><BR>
										<asp:ValidationSummary id="vldSumm" runat="server" CssClass="errormsg"></asp:ValidationSummary></TD>
								</TR>
								<TR>
									<TD class="emptycol"></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>--%>
				<TR>
					<TD class="emptycol" colspan="3"><asp:datagrid id="dtgVendList" runat="server" OnSortCommand="SortCommand_Click"
							OnPageIndexChanged="dtgVendList_Page" DataKeyField="RVDLM_LIST_INDEX">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RVDLM_LIST_NAME" SortExpression="RVDLM_LIST_NAME" HeaderText="List Name">
									<HeaderStyle Width="45%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
                                <%--<asp:BoundColumn HeaderText="Vendor" DataField="CM_COY_NAME" SortExpression="CM_COY_NAME">
                                <HeaderStyle Width="45%" HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn> --%>
                                <asp:BoundColumn HeaderText="Vendor">
									<HeaderStyle Width="45%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="3">
					    <asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>
					    <asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify"></asp:button>
					    <asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					    <asp:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" Style="display: none" />
					    <INPUT class="button" id="cmdReset" type="reset" value="Reset" name="Reset1" runat="server"
							style="DISPLAY:none"><INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
							runat="server"> <INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
							runat="server">
					</TD>
				</TR>
				<!--
				<TR>
					<TD class="emptycol"><A href="#"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
				--></TABLE>
			<!--<DIV id="Div1" style="DISPLAY: none" runat="server">&nbsp;</DIV>--></form>
	</body>
</HTML>
