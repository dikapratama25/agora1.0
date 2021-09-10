<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GLCodeMaint.aspx.vb" Inherits="eProcure.GLCodeMtn" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
		<title>GL Code Maintenance</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
           
      </script>
        <%--Response.Write(css)--%>   
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		    function reloadPage()
            {
                document.all("cmdSearch").click();
            }
            
			function Reset(){
				var oform = document.forms(0);
				oform.txtCode.value="";
				oform.txtDesc.value="";
//				oform.txtVendor.value="";
//				oform.txtDateFr.value="";
//				oform.txtDateTo.value="";
			}
		
			function selectAll()
			{
				SelectAllG("dtgGLCode_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgGLCode_ctl02_chkAll","chkSelection");
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
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0">
	        <tr >
			    <td class="header" colspan="5">
			        <asp:label id="lblTitle" runat="server" Text="GL Code Maintenance"></asp:label>
			    </td>
			</tr>				
			<tr>
				<td class = "emptycol" colspan="5">
				    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="Click the Add button to add new GL Code"></asp:label>					    
				</td>
			</tr>
			<tr>
			    <td class="linespacing1"></td>
			</tr>	            
			<tr>
				<td class="tableheader" colspan="5">Search Criteria</td>
			</tr>
			<tr>
				<td class="tablecol" style="width:63px;"><strong>GL Code</strong>:</td>
				<td class="tablecol" style="width:260px;">
				    <asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>
				</td>
				<td class="tablecol" style="width:66px;"><strong>GL Name</strong>:</td>
				<td class="tablecol" style="width:200px;">
				    <asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>
				</td>
				<td class="tablecol"></td>
			
			</tr>				
			<tr class="tablecol">
				<td class="tablecol"><strong>Status</strong>:</td>
				<td class="tablecol" colspan="3">
				    <asp:RadioButtonList ID="rdbStatus" CssClass="rbtn" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="A" Selected="True" >Active</asp:ListItem>
                    <asp:ListItem Value="I">Inactive</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
			
				<td class="TableColSearBtn" align="right">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
					<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" />
			    </td>
			</tr>
			<tr>
			    <td class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
					ShowSummary="False"></asp:validationsummary></td>
		    </tr>		    
		</table>
		<div class="rowspacing"></div>
		<table class="alltable" id="Table3" width="100%" cellspacing="0" cellpadding="0">				
		<tr>
			<td class="EmptyCol">
			    <asp:datagrid id="dtgGLCode" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
					DataKeyField="CBG_B_GL_CODE" Width="100%">
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
						<asp:TemplateColumn SortExpression="CBG_B_GL_CODE" HeaderText="GL Code">
							<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
								<asp:Label ID="lblIndex" Runat="server" Visible="False"></asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="CBG_B_GL_DESC" SortExpression="CBG_B_GL_DESC" ReadOnly="True" HeaderText="GL Name">
							<HeaderStyle HorizontalAlign="Left" Width="38%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="CBG_CC_REQ" SortExpression="CBG_CC_REQ" ReadOnly="True" visible =  "false" HeaderText="Cost Center Require">
							<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="CBG_AG_REQ" SortExpression="CBG_AG_REQ" ReadOnly="True" HeaderText="Asset Group Require" Visible="false">
							<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" ></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="CBG_STATUS" SortExpression="CBG_STATUS" ReadOnly="True" HeaderText="Status" >
							<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" ></ItemStyle>
						</asp:BoundColumn>
						
					</Columns>
				</asp:datagrid>
			</td>
		</tr>								
		<tr runat="server" id="trDiscount">
			<td class="emptycol">
				<input type="button" value="Add" id="cmdAdd" runat="server" class="button"/>
				<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
				<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button>
				<asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none"></asp:button> 
			</td>
		</tr>
	</table>
	</form>
	</body>
</HTML>