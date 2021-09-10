<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GLAnalysisRule.aspx.vb" Inherits="eProcure.GLAnalysisRule" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>GLAnalysisRule</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgGL_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgGL_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
	        var oform = document.forms(0);
		    oform.txtRuleCode.value='';
		}
		
		function ShowDialog(filename,height,width)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth:" + width);
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
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="header" colspan="4" style="height: 3px" width="100"></td>
			</tr>
			<tr>
				<td class="header" colspan="4"><asp:label id="lbl_title" runat="server">GL Analysis Maintenance</asp:label></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant GL analysis information."></asp:label>
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
								<td class="tableheader" colspan="3" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" width="25%">&nbsp;<strong>GL Analysis Description Code </strong>:</td>
								<td class="tablecol" width="35%"><asp:textbox id="txtRuleCode" runat="server" MaxLength="50" CssClass="txtbox" width="300px"></asp:textbox></td>							
								<td class="tablecol" width="40%" align="right" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;
								<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgGL" runat="server" OnPageIndexChanged="dtgGL_Page" AllowSorting="True" AutoGenerateColumns="False" Width="100%" OnSortCommand="SortCommand_Click" Visible="False">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="IG_GLRULE_CODE" SortExpression="IG_GLRULE_CODE" HeaderText="GL Analysis Description Code">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="IGC_GLRULE_CATEGORY" HeaderText="Sub Description">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn HeaderText="Remark">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="IGG_GL_CODE" HeaderText="GL Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="CBG_B_GL_DESC" HeaderText="Description">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>		
									<asp:BoundColumn Visible="False" DataField="IG_GLRULE_INDEX"></asp:BoundColumn>							
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button>
					<input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/>
					<input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
