<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DeptSetup.aspx.vb" Inherits="eProcure.DeptSetupFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Department Setup</title>
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
			SelectAllG("dtgDept_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgDept_ctl02_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtDeptCode.value="";
			oform.txtDeptName.value="";
		}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		
        <%  Response.Write(Session("w_DeptSetup_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Fill in the search criteria and click Search button to list the relevant departments. Click the Add button to add new department."></asp:label>
                        </div>
					</TD>
				</TR>
            <tr>
					<TD class="linespacing2" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colspan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Dept. Code</STRONG> :
									<asp:textbox id="txtDeptCode" runat="server" CssClass="txtbox" Width="94px" MaxLength="10"></asp:textbox>&nbsp;<STRONG>Dept. 
										Name</STRONG> :
									<asp:textbox id="txtDeptName" runat="server" CssClass="txtbox" Width="128px" MaxLength="50"></asp:textbox>&nbsp;&nbsp;</TD>
							    <TD class="tablecol" align="right">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgDept" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="CDM_DEPT_INDEX">
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
								<asp:TemplateColumn SortExpression="CDM_DEPT_CODE" HeaderText="Department Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDeptCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CDM_DEPT_CODE" SortExpression="CDM_DEPT_CODE" HeaderText="Dept. Code"></asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department Name">
									<HeaderStyle Width="75%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDM_APPROVAL_GRP_INDEX" SortExpression="CDM_APPROVAL_GRP_INDEX"
									HeaderText="Approval Grop"></asp:BoundColumn>
								<asp:BoundColumn DataField="AGM_GRP_NAME" SortExpression="AGM_GRP_NAME" HeaderText="Approval Group"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgDept_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" runat="server" style="DISPLAY:none"><!--meilai--><INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" runat="server"
							NAME="hidMode"> <INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" runat="server"
							NAME="hidIndex"><!--meilai--></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
