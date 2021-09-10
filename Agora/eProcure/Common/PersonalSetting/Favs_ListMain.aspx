<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Favs_ListMain.aspx.vb" Inherits="eProcure.Favs_ListMain" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Favs_ListMain</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
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
		
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}

		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("hide");
				//var cmd_add = document.getElementById("add");
				//var cmd_mod = document.getElementById("modify");
				var cmd_delete = document.getElementById("cmd_delete");
				var hidMode = document.getElementById("hidMode");
				var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				if (check==1)
				{
				
				//cmd_mod.style.display ="none";
				//div_add.style.display ="";
				//cmd_add.style.display ="";
				cmd_delete.style.display = "none";
				Form1.hidMode.value = 'm';
				add_mod.value='add';
				}
				
				
				else if (check==0)
				{
				//div_add.style.display ="";
				cmd_delete.style.display = "none";
				Form1.hidMode.value = 'a';
				//cmd_add.style.display ="none";
				//cmd_mod.style.display ="";
				}
				//if(div_add.style.display == "none"){
				 //   div_add.style.display ="";
				    
											
				//}
				//else{		
				//	div_add.style.display ="none";	
				
				//}

	
	}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" style="HEIGHT: 25px"><FONT size="1">&nbsp;</FONT><STRONG>Favourite 
							List Maintenance</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Favourite List Name</STRONG>&nbsp;:
									<asp:textbox id="txtsearch" runat="server" MaxLength="50" CssClass="txtbox"></asp:textbox>&nbsp;<asp:button id="cmd_search" runat="server" CssClass="button" CausesValidation="False" Text="Search"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<DIV id="hide" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader">&nbsp;<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;</TD>
								</TR>
								<TR>
									<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<STRONG>Favourite List Name</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txt_add_mod" runat="server" MaxLength="30" CssClass="txtbox" Width="136px"></asp:textbox>&nbsp;<asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmd_clear" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button>&nbsp;<asp:button id="cmd_cancel" runat="server" CssClass="button" CausesValidation="False" Text="Cancel"></asp:button></TD>
								</TR>
								<TR>
									<TD class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field</TD>
								</TR>
								<TR>
									<TD class="emptycol"><BR><asp:requiredfieldvalidator id="vldFavList" runat="server" ControlToValidate="txt_add_mod" ErrorMessage="<ul><li>Favourite List Name is required.</li></ul>"></asp:requiredfieldvalidator></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<P><asp:datagrid id="MyDataGrid" runat="server" CssClass="Grid" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
								OnPageIndexChanged="MyDataGrid_Page" DataKeyField="FLM_LIST_INDEX">
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
									<asp:TemplateColumn SortExpression="FLM_LIST_NAME" HeaderText="Favourite List Name">
										<HeaderStyle HorizontalAlign="Left" Width="95%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink Runat="server" ID="lnkListName"></asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 16px"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdAdd" runat="server" CssClass="button" CausesValidation="False" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" CausesValidation="False" Text="Modify"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" CausesValidation="False" Text="Delete"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Reset" name="Reset1" runat="server"
							style="DISPLAY:none">&nbsp;&nbsp;<INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" runat="server">
						<INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
			<DIV id="Div1" style="DISPLAY: none" runat="server">&nbsp;</DIV>
		</form>
	</body>
</HTML>
