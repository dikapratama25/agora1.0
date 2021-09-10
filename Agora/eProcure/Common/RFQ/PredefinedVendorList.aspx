<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PredefinedVendorList.aspx.vb" Inherits="eProcure.PredefinedVendorList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>VendorList</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		Dim dDispatcher As New AgoraLegacy.dispatcher
		</script>
		<% Response.Write(Session("WheelScript"))%>
		
		<script language="javascript">
		<!--
            function RemoveItemCheck(pChkSelName){
			    var oform = document.forms[0];
			    var iTotalCheckbox=0, iTotalChecked=0;
		        for (var i=0;i<oform.elements.length;i++)
			    {
				    var e = oform.elements[i];
				    if (e.type=="checkbox")
				    {
				        if (e.checked==true)
				        {
				            iTotalChecked+=1;
				        }
				        iTotalCheckbox+=1;
				    }
			    }
			    //if 4, dont allow to delete, at least have 1 record.
			    // if (iTotalCheckbox == 4)
			    // YapCL: 2011Mar08 - cater multiple items
			    if (iTotalCheckbox == iTotalChecked)
				{
				    alert ('You must have at least 1 row of record!');
				    return false;
				}
				if (iTotalChecked == 2)
				{
				    alert ('Please make at least one selection!');
				    return false;
				}
				else
				{
				    // YapCL: 2011Mar08 - cater yes no			    
				    var checkyesno ;
				    checkyesno = CheckAtLeastOne('chkSelection','delete');
				    if (checkyesno == true)
				    {
				        return true;
				    }
				    else
				    {
				        return false;
				    }
				}
			}
			
			function selectAll()
			{
				SelectAllG("dtg_vendor_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtg_vendor_ctl02_chkAll","chkSelection");
			}
			
			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<TABLE id="Table2" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<TR>
								<TD class="header"><A></A><asp:label id="lbl_title" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader">&nbsp;Vendor List</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG><asp:label id="lbl_List_Name" runat="server"></asp:label></STRONG>&nbsp;:&nbsp;<asp:label id="lbl_name" runat="server"></asp:label></TD>
							</TR>							
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>				
				<TR>
					<TD><asp:datagrid id="dtg_vendor" runat="server" AutoGenerateColumns="False" CssClass="grid" OnSortCommand="SortCommand_Click"
							DataKeyField="CM_COY_ID">
							<Columns>
								<asp:TemplateColumn HeaderText="Add">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" ToolTip="Select/Deselect All" AutoPostBack="false"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor Company Name">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Contact Details ">
									<HeaderStyle Width="60%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_adds" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CM_COY_ID" SortExpression="CM_COY_ID" HeaderText="com_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px">
					    <asp:button id="cmdRemove" runat="server" CssClass="Button" Width="94px" CausesValidation="False" Text="Remove" ></asp:button>&nbsp;
					    <asp:button id="cmdClose" runat="server" CssClass="Button" Text="Close" CausesValidation="false" ></asp:button>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>				
			</TABLE>
		</form>
	</body>
</HTML>
