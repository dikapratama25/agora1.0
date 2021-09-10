<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPAnalysisCodeMatrix.aspx.vb" Inherits="eProcure.IPPAnalysisCodeMatrix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>E2P Analysis Code Matrix</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            'Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCode2&chk=N&na=Y&def=Y")
        </script> 
        <%response.write(Session("WheelScript"))%>
        <% Response.Write(CSS)%>
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.write(Session("ventypeahead")) %>
		<script type="text/javascript">
		<!--  
		
		$(document).ready(function(){
<%--            $("#txtAddGLCode").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
            
            $("#txtAddGLCode").result(function(event, data, formatted) {
            if (data)
            $("#hidGLCode").val(data[1]);
            });--%>
        });
            
		function selectAll()
		{
			//SelectAllG("dtgTax_ctl02_chkAll","chkSelection");
  		}
				
		function checkChild(id)
        {
            //checkChildG(id, "dtgTax_ctl02_chkAll", "chkSelection");
		}


		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("Hide_Add1");
				var div_add = document.getElementById("Hide_Add2");
				var div_add = document.getElementById("Hide_Add3");
				
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
				}
			}
	
		-->
		</script>
	    <style type="text/css">
            .tablecol {
                font-weight: 700;
            }
        </style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="Header" colspan="4">Analysis Code Matrix</td>
			</tr>
			<tr>
				<td>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant analysis code. Click the Add button to add new tax code."></asp:label>
				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
				            <tr>
					            <td class="header" colspan="3" width="100%"></td>
				            </tr>
							<tr>
								<td class="tableheader" colspan="5" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" width="12%"><strong>GL Code </strong>:</td>
								<td class="tablecol" width="20%"><asp:textbox id="txtGLCode" runat="server" MaxLength="20" CssClass="txtbox" width="150px"></asp:textbox></td>
								<td class="tablecol" width="36%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdClear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<div id="Hide_Add2" style="DISPLAY: none" runat="server">
                <%--Jules 2018.07.13 - Modified layout--%>
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td colspan="6" class="tableheader" style="height: 19px" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</td>
					</tr>
					<tr class="tablecol">
						<td width="12%" style="height: 24px">&nbsp;<strong>GL Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td style="height: 24px" colspan="5"><asp:textbox id="txtAddGLCode" runat="server" MaxLength="20" width="150px" CssClass="txtbox"></asp:textbox></td>
                    </tr>
                    <tr class="tablecol">                        
                        <td width="12%" style="height: 24px">&nbsp;<strong>Analysis Code</strong>:</td>
                        <td width="70%" style="height: 24px">
                            <asp:checkboxlist id="cbxAnalysisType" runat="server" RepeatDirection ="Horizontal" ToolTip="Check for mandatory. Uncheck for optional."></asp:checkboxlist>
                        </td>
                        <td align="right" colspan="4">
						    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<%--<tr class="tablecol">--%>
<%--					<td width="12%" style="height: 24px">&nbsp;<strong>Analysis Code Type</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px">
						<asp:DropDownList id="ddlAddCodeType" runat="server" CssClass="ddl" width="150px"></asp:DropDownList>
						</td>--%>
						<%--<td align="right" colspan="6">
						    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>--%>
                    <%--End Jules' modification--%>
					<tr>
						<td colspan="6" class="emptycol" style="height: 19px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field
                            <%--<asp:requiredfieldvalidator id="validateCodeType" runat="server" ControlToValidate="ddlAddCodeType" ErrorMessage="Code Type is required."
								Display="None"></asp:requiredfieldvalidator>--%>
                            <asp:requiredfieldvalidator id="validateGLCode" runat="server" ControlToValidate="txtAddGLCode" ErrorMessage="GL Code is required."
								Display="None"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td colspan="6">
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
							</td>
					</tr>
					<tr>
						<td colspan="6" class="emptycol"></td>
					</tr>
				</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgTax" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="true" ToolTip="Select/Deselect All" OnCheckedChanged="chkAll_CheckedChanged"></asp:checkbox>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CBGCAC_B_GL_CODE" SortExpression="CBGCAC_B_GL_CODE" HeaderText="GL Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Fund Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode1" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Product Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode2" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Channel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode3" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Reinsurance Company" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode4" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Asset Fund" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode5" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Tax Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode6" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional"  />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Cost Centre" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode7" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Project Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode8" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Person Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAnalysisCode9" runat="server" ToolTip="Check for Mandatory. Uncheck for Optional" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
                                    <%--<asp:BoundColumn Visible="False" DataField="AC_DEPT_CODE"></asp:BoundColumn>--%>
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button><input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/>
					    <input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/><input id="hidGLCode" type="hidden" size="1" name="hidGLCode" runat="server"/>&nbsp;
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
