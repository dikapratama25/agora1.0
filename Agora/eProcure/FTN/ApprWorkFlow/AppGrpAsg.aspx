<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppGrpAsg.aspx.vb" Inherits="eProcure.AppGrpAsgFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AppGrpAsg</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
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
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWFAsg_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="3"></TD>
			    </TR>
				<TR>
	                <TD colSpan="3">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Step 1: Create, delete or modify Approval Group.<br /><b>=></b> Step 2: Assign Approving Officer to the Selected Approval Group.<br>Step 3: Assign Purchaser to the Selected Approval Group."></asp:label>

	                </TD>
                </TR>
              <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
            <tr>
					<TD align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Please select Approval Group Type and the related Approval Group."
						></asp:label>
                        </div>
					</TD>
			</TR>
               <tr>
					<TD class="linespacing2" colSpan="3"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" colspan="2" width="100%">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
							<TR class="tablecol" id="trGrpType" runat="server">
								<TD style="WIDTH: 124px">&nbsp;<STRONG>Group Type</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<td>&nbsp;<asp:dropdownlist id="cboType" runat="server" CssClass="txtbox" Width="100px" AutoPostBack="True">
										<asp:ListItem Value="INV">INV</asp:ListItem>
										<asp:ListItem Value="PO">PO</asp:ListItem>
										<asp:ListItem Value="PR">PR</asp:ListItem>
										<asp:ListItem Value="IPP">IPP</asp:ListItem>
									</asp:dropdownlist></td>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 116px"><STRONG>&nbsp;Approval Group </STRONG>
									:</TD>
								<TD class="TableCol" >&nbsp;<asp:dropdownlist id="cboGroup" runat="server" AutoPostBack="True" CssClass="txtbox" Width="300px"></asp:dropdownlist>
								<asp:checkbox id="chkConsol" runat="server" AutoPostBack="True" Text="Consolidation Required"
										Enabled="False"></asp:checkbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				</TABLE>
						<div id="Div_AppGrp" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0" width="100%">
								<TBODY>
									<TR>
										<TD class="EmptyCol">&nbsp;
											<asp:datagrid id="MyDataGrid" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyData_Page">
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
													<asp:BoundColumn DataField="AGA_SEQ" SortExpression="AGA_SEQ"  readonly="true"    HeaderText="Level">
														<HeaderStyle Width="10%"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AO_ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="AO_NAME" SortExpression="AO_NAME"  readonly="true"    HeaderText="Approving Officer">
														<HeaderStyle Width="33%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="UM_MASS_APP" SortExpression="UM_MASS_APP" HeaderText="Mass Approval">
														<HeaderStyle Width="5%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AAO_ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="AAO_NAME" SortExpression="AAO_NAME" readonly="true"    HeaderText="Alternative Approving Officer">
														<HeaderStyle Width="33%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AGA_RELIEF_IND" SortExpression="AGA_RELIEF_IND" readonly="true"   HeaderText="Relief Staff Control">
														<HeaderStyle Width="14%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AGA_GRP_INDEX"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD class="emptycol">
											<asp:Label id="lblRed" runat="server" Visible="False"><font color="red">*</font>&nbsp;deleted or inactive user is displayed in red colour</asp:Label></TD>
									</TR>
									<tr>
										<td class="emptycol">&nbsp;</td>
									</tr>
									<TR>
										<TD class="emptycol" style="HEIGHT: 7px">
											<asp:button id="cmd_Add" runat="server" CssClass="button" Text="Add"></asp:button>
											<asp:button id="btn_Add3" runat="server" CssClass="FreeWidthButton" Text="Add Finance Officer" 
											visible="False"></asp:button>
											<asp:button id="btn_Add2" runat="server" CssClass="FreeWidthButton" Text="Add Finance Manager" 
												visible="False"></asp:button>											
											<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify"></asp:button>
											<asp:button id="cmd_Delete" runat="server" CssClass="button" Text="Delete"></asp:button>
								                <asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
								<INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
												runat="server"> <INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
												runat="server">&nbsp;
										</TD>
	            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
								</TR>
								</TBODY>
							</TABLE>
						</div>
			<TABLE class="alltable" cellSpacing="0" cellPadding="0">
				<TR id="trhid" runat="server">
					<TD align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) Please click the Add button to assign new Approving Officer to the Approval Group.<br>b) To remove/un-assign Approving Officer, tick the checkbox and click Delete button."></asp:label>
                        </div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
