<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GLCode.aspx.vb" Inherits="eProcure.GLCodeFTN" trace="False"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GLCode</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
			function clearValue(id){
				if( document.getElementById(id) ){
					document.getElementById(id).value = "";	
				}
			}	
			function selectAll(){
				SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
			}
			
			function DisplayAddPanel(){
				var div_add = document.getElementById("hide");
				div_add.style.display ="";
				document.getElementById('vldSumm').innerHTML = "";
				var catRadioBtn = document.getElementById('catCodeRadioBtn');
				if( catRadioBtn.checked ){
					if( document.getElementById('glDesclbl') ){
						document.getElementById('glDesclbl').style.display="none";
					}
					if( document.getElementById('txtGLCodeDescription') ){
						document.getElementById('txtGLCodeDescription').style.display="none";
					}
				}else {
					if( document.getElementById('glDesclbl') ){
						document.getElementById('glDesclbl').style.display="";
					}
					if( document.getElementById('txtGLCodeDescription') ){
						document.getElementById('txtGLCodeDescription').style.display="";
					}
				}
				if( document.getElementById('txtGLCodeDescription') ){
					document.getElementById('txtGLCodeDescription').value = "";
				}
				var txtGL = document.getElementById('txtAddGLCode');
				if( txtGL ){
					txtGL.value = "";
					txtGL.disabled = false ;
					txtGL.focus();
				}
			}
			function HideAddPanel(){
				clearValue('txtAddGLCode');
				clearValue('txtGLCodeDescription');
				var div_add = document.getElementById("hide");
				document.getElementById('btnClrGL').value = 'Clear' ; 
				div_add.style.display ="none";
				document.getElementById('txtAddGLCode').disabled = false ;
			}
			function ValidateGLCode(){
					var catRadioBtn = document.getElementById('catCodeRadioBtn');
					var glCode = document.getElementById('txtAddGLCode').value ;
					if( glCode == '' ){
						if( !catRadioBtn.checked ){
								alert('GL Code  required.');
						}else {
								alert('Category Code  required.');
						}
						document.getElementById('txtAddGLCode').focus();
						return false ;
					}
					if( glCode.indexOf( '(' ) >= 0 || glCode.indexOf( ')' ) >= 0 ){
						if( !catRadioBtn.checked ){
							alert( 'GL Code cannot contain \"(\" or \")\" . ');
							document.getElementById('txtAddGLCode').focus();
							return false ;
						}
					}
					if( !catRadioBtn.checked ){
							var desc = document.getElementById('txtGLCodeDescription').value ;
							if( desc == '' ){
								alert('GL Description required.');
								document.getElementById('txtGLCodeDescription').focus();
								return false ;
							}
					}
					return true ;
			}
			function Clear(){
				if( !document.getElementById('txtAddGLCode').disabled ){
						clearValue('txtAddGLCode');
				}
				clearValue('txtGLCodeDescription');
			}
		</script>
	</HEAD>
	<body onload="document.getElementById('txtGLCode').focus();" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td class="header" style="HEIGHT: 25px">
						<asp:label id="headerlbl" runat="server" Font-Bold="True" Width="144px">GL Code</asp:label></td>
				</tr>
				<TR>
	                    <TD colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Fill in the search criteria and click Search button to list the relevant GL Code(s). Click Add button to add new GL Code. Select GL Code and click Modify button to modify."
		                    ></asp:label>

	                    </TD>
                    </TR>
				<tr>
					<td><br>
						<P><asp:radiobutton id="glRadioBtn" runat="server" AutoPostBack="True" GroupName="criteria" Text="GL Code"></asp:radiobutton>&nbsp;&nbsp;
							<asp:radiobutton id="catCodeRadioBtn" runat="server" AutoPostBack="True" GroupName="criteria" Text="Category Code"></asp:radiobutton><BR>
						</P>
					</td>
				</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="table2" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td class="TableHeader" colspan="2">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="TableCol">&nbsp;<strong>
										<asp:label id="srchCriLbl" runat="server">GL Code</asp:label></strong>&nbsp;:
									<asp:textbox id="txtGLCode" CssClass="txtbox" Runat="server"></asp:textbox>&nbsp;
								</td>
								<td align="right">
								    <asp:button id="cmd_search" Text="Search" CssClass="button" Runat="server" CausesValidation="False"></asp:button>&nbsp;
									<input class="button" id="btnclr" onclick="clearValue('txtGLCode');" type="button" value="Clear">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr><td class="rowspacing"></td></tr>
				<tr>
					<td>
						<div id="hide" style="DISPLAY: none" runat="server">
							<table class="alltable" id="table3" cellSpacing="0" cellPadding="0">
								<TR>
									<TD class="tableheader" colspan="2">&nbsp;Please&nbsp;enter&nbsp;the&nbsp;following&nbsp;value
									</TD>
								</TR>
								<tr>
									<td class="tablecol" style="HEIGHT: 25px" noWrap>&nbsp;<STRONG>
											<asp:label id="addLbl" runat="server">GL Code</asp:label></STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txtAddGLCode" CssClass="txtbox" Runat="server"></asp:textbox>&nbsp;
										<strong>
											<asp:label id="glDesclbl" Runat="server">GL Description</asp:label></strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;
										<asp:textbox id="txtGLCodeDescription" CssClass="txtbox" Runat="server"></asp:textbox>&nbsp;
									</td>
									<td class="tablecol" align="right">
									    <asp:button id="saveGL" Text="Save" CssClass="button" Runat="server"></asp:button>&nbsp;
										<input class="button" id="btnClrGL" onclick="Clear();" type="button" value="Clear" runat="server">&nbsp;<input class="button" id="btncancel" onclick="HideAddPanel();" type="button" value="Cancel"
											runat="server">
									</td>
								</tr>
								<TR>
									<TD class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field<asp:requiredfieldvalidator id="rfv_code" runat="server" Display="None" ErrorMessage="Required field missing"
											ControlToValidate="txtAddGLCode"></asp:requiredfieldvalidator></TD>
								</TR>
								<TR>
									<TD class="emptycol">&nbsp;</TD>
								</TR>
								<TR>
									<TD><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></TD>
								</TR>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td class="emptycol">
						<p><asp:datagrid id=MyDataGrid runat="server" Width="100%" OnSortCommand="SortCommand_Click" AllowSorting="True" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False" DataSource="<%# LoadDataSource() %>" DataKeyField="DATA_COL">
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
									<asp:BoundColumn DataField="DATA_COL" SortExpression="DATA_COL" >
										<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CBG_B_GL_DESC" SortExpression="CBG_B_GL_DESC" HeaderText="GL Description"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><input class="button" id="btnAdd" onclick="DisplayAddPanel();" type="button" value="Add">&nbsp;
						<asp:button id="cmd_modify" Text="Modify" CssClass="button" Runat="server" CausesValidation="False"
							Enabled="False"></asp:button>&nbsp;
						<asp:button id="cmd_delete" runat="server" Width="64px" Text="Delete" CssClass="button" CausesValidation="False"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG></STRONG></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
