<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CostAllocMaint.aspx.vb" Inherits="eProcure.CostAllocMaint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Cost Alloc. Code Maintenance</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
           
      </script>
        <%Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		    function reloadPage()
            {
                document.all("cmdSearch").click();
            }
            
			function Reset(){
				var oform = document.forms(0);
				oform.txtCode.value="";
				oform.txtDesc.value="";
				oform.txtVendor.value="";
				oform.txtDateFr.value="";
				oform.txtDateTo.value="";
			}
		
			function selectAll()
			{
				SelectAllG("dtgCostAllocMtn_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCostAllocMtn_ctl02_chkAll","chkSelection");
			}
		    
		    function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=500,height=280,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function ShowDialog(filename,height)
			{				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 600px");
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
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("CostAlloc_tabs"))%>
		<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0">
			   <td class="header" style="height: 7px" colspan="4"></td>
				
				<TR>
					<TD class = "emptycol" align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="<b>=></b> Step 1: Create Cost Allocation Code.<br>Step 2: Setup Cost Allocation Details."></asp:label>
                        </div>
					</TD>
				</TR>
	            <td class="header" style="height: 7px" colspan="4"></td>
				<tr>
					<td class="tableheader" colspan="6" style="height: 19px">Search Criteria</td>
				</TR>
				<TR>
					<TD class="tablecol"><strong>Cost Alloc. Code</strong>:
					<asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol"><strong>Description</STRONG>:
					<asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="265px"></asp:textbox></TD>
				
				</TR>
				
				<tr class="tablecol">
				
					<td class="TableCol" colspan="6" align="right" style="height: 24px">
						<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
				    </td>
				</tr>
				</table>
				<TR>
					<TD class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</TR>
				
				<br>
				
				<TABLE class="alltable" id="Table3" width="100%" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:datagrid id="dtgCostAllocMtn" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CAM_CA_CODE" Width="100%">
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
								<asp:TemplateColumn SortExpression="CAM_CA_CODE" HeaderText="Cost Alloc. Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label Runat="server" ID="lblCode"></asp:label>
										<asp:Label ID="lblIndex" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CAM_CA_DESC" SortExpression="CAM_CA_DESC" ReadOnly="True" HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="38%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>	
				</table>	
				<TABLE class="alltable" id="Table4" width="100%" cellSpacing="0" cellPadding="0">					
				<tr runat="server" id="trDiscount">
					<td class="emptycol" colspan="6">
						<input type="button" value="Add" id="cmdAdd" runat="server" class="button"/>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button>
						<asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none"></asp:button> 
					</td>
				</tr>
				 <td class="header" style="height: 7px" colspan="4"></td>
				<TR>
					<TD class = "emptycol" align="center">
						<div align="left"><asp:label id="Label1" runat="server"  CssClass="lblInfo">
						 a) Click Add button to add new Cost Allocation Code.<br>b) Click Modify button to modify Cost Allocation Code.
						 <br>c) Click Delete button to delete Cost Allocation Code and Detail.<br>d) To view cost allocation detail,click 'Cost Alloc. Detail' Tab.</asp:label>
                        </div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>