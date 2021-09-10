<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BuyerCatalogue.aspx.vb" Inherits="eProcure.BuyerCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BuyerCatalogue</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
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


		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("hide");
				//var cmd_add = document.getElementById("add");
				//var cmd_mod = document.getElementById("modify");
			    //var cmd_delete = document.getElementById("cmd_delete");
				//var hidMode = document.getElementById("hidMode");
				//var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				
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
        <%  Response.Write(Session("w_BuyerCatalogue_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="<b>=></b> Step 1: Create user defined Purchase Catalogue.<br>Step 2: Assign item master to Purchaser Catalogue.<br>Step 3: Assign purchaser to Purchaser Catalogue."
						></asp:label>
                        </div>
					</TD>
				</TR>
             <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
            <tr>
					<TD align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Note: A Purchaser Catalogue consists of item lists that are assigned to the purchaser. Purchaser can only place PO on these allocated items."
						></asp:label>
                        </div>
					</TD>
			</TR>
           <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colspan="2" style="width: 641px">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" nowrap style="height: 25px; width: 641px;"><strong><asp:Label ID="Label1" runat="server" Text="Purchaser Catalogue" CssClass="lblname"></asp:Label></strong> :&nbsp; &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="cboCatalogueBuyer" runat="server" CssClass="txtbox" Width="322px" AutoPostBack="True">
                                    </asp:DropDownList></TD>
                                <TD class="tablecol" align="right">
                                   <asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
                                   </TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 18px"></TD>
				</TR>
				<TR>
					<TD>
						<DIV id="hide" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
								<TR>
									<TD class="tableheader" style="width: 641px" colspan="2">&nbsp;Please
										<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the 
										following&nbsp;value
									</TD>
								</TR>
								<TR>
									<TD class="tablecol" style="HEIGHT: 20px" nowrap>&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Purchaser Catalogue"></asp:Label></strong>
									    <asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txt_add_mod2" runat="server" MaxLength="60" Width="323px" CssClass="txtbox"></asp:textbox>&nbsp;</td>
									<td class="tablecol" align="right">
									    <asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_cancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button></TD>
								</TR>
								<TR>
									<TD class="emptycol" style="width: 641px" colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field<asp:requiredfieldvalidator id="rfv_cat_name" runat="server" ControlToValidate="txt_add_mod2" ErrorMessage="Purchaser Catalogue is required."
											Display="None"></asp:requiredfieldvalidator></TD>
								</TR>
								<TR>
									<TD class="emptycol" style="width: 641px" colspan="2"></TD>
								</TR>
								<TR>
									<TD style="width: 641px" colspan="2">
										<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
									</TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<P><asp:datagrid id="MyDataGrid" runat="server" DataKeyField="BCM_CAT_INDEX" OnPageIndexChanged="MyDataGrid_Page"
								AllowSorting="True" OnSortCommand="SortCommand_Click">
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
									<asp:BoundColumn DataField="BCM_GRP_DESC" SortExpression="BCM_GRP_DESC" HeaderText="Purchaser Catalogue">
										<HeaderStyle Width="40%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Purchaser">
										<HeaderStyle Width="30%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblBuyer"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="BCM_CAT_INDEX"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 25px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdBuyerAsg" runat="server" Width="120px" CssClass="button" Text="Buyer Assignment" Enabled="False" Visible="False"></asp:button>&nbsp;<asp:button id="cmdItemAsg" runat="server" Width="120px" CssClass="button" Text="Items Assignment" Enabled="False" Visible="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Reset" name="Reset1" runat="server"
							style="DISPLAY:none">&nbsp;<INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
							runat="server"><INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
							runat="server"></TD>
				</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) Default Purchaser Catalogue contains all item master that is added to the system. The Default Purchaser Catalogue cannot be deleted or modified.<br />b) Click Add button to add new Purchaser Catalogue.</br>c) Click Modify button to modify the user created Purchaser Catalogue (Not applicable to Default Purchaser Catalogue).</br>d) Click Delete button to delete the user created Purchaser Catalogue (Not applicable to Default Purchaser Catalogue).</br>e) To view item(s) assigned to the Purchaser Catalogue, click 'Item Assignment' Tab.</br>f) To view Purchaser(s) assigned to the Purchaser Catalogue, click 'Purchaser Assignment' Tab."></asp:label>
                        </div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG></STRONG></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
