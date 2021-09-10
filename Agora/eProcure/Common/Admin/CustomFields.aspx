<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CustomFields.aspx.vb" Inherits="eProcure.CustomFields" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CustomFields</title>
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
			SelectAllG("dtgCustomField_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgCustomField_ctl02_chkAll","chkSelection");
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
	<body>
		<form id="Form2" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tbody>
					<tr>
						<td class="header" colspan="5">
                            <asp:Label ID="Label3" runat="server" Text="Custom Fields"></asp:Label></td>
					</tr>
					<tr>
						<td class="emptycol" colspan="5"></td>
					</tr>
					<tr>
						<td class="tableheader" colspan="5">&nbsp;<asp:Label ID="Label4" runat="server" Text="Search Criteria"></asp:Label></td>
					</tr>
					<tr>
						<td class="tablecol" style="width:40px;">
						    <strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Module"></asp:Label> </strong>:
						</td>
						<td class="tablecol" style="width:40px;">
						    <asp:DropDownList ID="ddlModule" cssclass="ddl" runat="server" AutoPostBack="True">
                                <asp:ListItem>PO</asp:ListItem>
                                <asp:ListItem>PR</asp:ListItem>
                            </asp:DropDownList>
						</td>
						<td class="tablecol" style="width:80px;">
						    <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Field Name"></asp:Label> </strong>:
						</td>
						<td class="tablecol" style="width:40px;">						    
						    <asp:DropDownList ID="ddlSearch" cssclass="ddl" runat="server">
                            </asp:DropDownList>
						</td>                            		
						<td class="tablecol" align="right" >
							<asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;
							<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;
						</td>
					</tr>
					<%--<tr>
						<td>
							<table class="alltable" id="custom" style="DISPLAY: none" cellSpacing="0" cellPadding="0"
								border="0" runat="server">
								<tr>
									<td class="tableheader">&nbsp;<asp:label id="lbl_add_mod" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td class="tablecol">&nbsp;<STRONG>Field Name</STRONG><asp:label id="lblName" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:&nbsp;<asp:textbox id="txtName" runat="server" CssClass="txtbox" Width="128px" MaxLength="20"></asp:textbox>&nbsp; 
										&nbsp;
										<div id="value" runat="server"><STRONG>&nbsp;Field Value</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:&nbsp;<asp:textbox id="txtValue" runat="server" CssClass="txtbox" Width="128px" MaxLength="100"></asp:textbox>
											&nbsp;</div>
										<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button></td>
								</tr>
								<tr class="emptycol">
									<td><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>indicates 
										required field
									</td>
								</tr>
								<tr class="emptycol">
									<td><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary><asp:requiredfieldvalidator id="vldName" runat="server" ControlToValidate="txtName" ErrorMessage="Field Name is required."
											Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="vldValue" runat="server" ControlToValidate="txtValue" ErrorMessage="Field Value is required."
											Display="None"></asp:requiredfieldvalidator></td>
								</tr>
							</table>
						</td>
					</tr>--%>
					<tr>
						<td colspan="5" width="100%"><asp:datagrid Width="100%" id="dtgCustomField" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
								AllowSorting="True" OnPageIndexChanged="dtgCustomField_Page" DataKeyField="CF_FIELD_NO">
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
                                    <asp:BoundColumn DataField="CF_MODULE" HeaderText="Module"></asp:BoundColumn>
									<asp:BoundColumn DataField="CF_FIELD_NAME" SortExpression="CF_FIELD_NAME" HeaderText="Field Name">
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="CF_FIELD_VALUE" HeaderText="Values"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CF_FIELD_INDEX" Visible="false"  HeaderText="Field No"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
					<tr>
						<td colspan="5">&nbsp;
							<div><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button><!--<INPUT class="button" onclick="DeselectAllG('dtgCustomField__ctl2_chkAll','chkSelection')"type= "button"value="Reset"&gt;&nbsp;-->
								<input id="hidIndex" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="Hidden1"
									runat="server" /><input id="hidMode" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" runat="server"
									name="hidMode" />
									<asp:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" Style="display: none" />
									</div>
									
						</td>
					</tr>
				</tbody>
			</table>
		</form>
	</body>
</HTML>
