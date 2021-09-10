<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommodityType.aspx.vb" Inherits="eProcure.CommodityType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Commodity Type</title>
    <% Response.Write(Session("WheelScript"))%>
    <% Response.Write(Session("JQuery")) %>
    
    <script language="javascript">
		<!--
		
		$(document).ready(function(){
		if($("#txtSearch").val().length >= 3)
		{
		    document.getElementById("cmdSearch").disabled = false;
		}
		else
		{
		    document.getElementById("cmdSearch").disabled = true;
		}
		$("#txtSearch").keyup(function(event){		
		if($("#txtSearch").val().length >= 3)
		{
		    document.getElementById("cmdSearch").disabled = false;
		}
		else
		{
		    document.getElementById("cmdSearch").disabled = true;

		}
		});
		});
		
			function pass_target(sel_id, sel_text)
		    {
		        //var selected_str = sel_text;
			    //window.opener.document.Form1.txtCommodityType.value = selected_str.substring(6, selected_str.length - 4);
			    window.opener.document.Form1.txtCommodityType.value = sel_text;
			    window.opener.document.Form1.hidCommodityType.value = sel_id;
			    window.close();
		    }
		    function Reset()
		    {
		        document.getElementById("txtSearch").value = "";
		    }
		        
		    		
		-->
		</script>
</head>
<body>
	<form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
			<TABLE class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
 				<TR>
					<TD class="header" colspan="3">
					    <asp:label id ="lblHeader" runat="server" Font-Bold="True"  Text="Commodity Search"></asp:label>
                </TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="3">
					</TD>
				</TR>
				<tr>
				    <td colspan="3">
                        <asp:Label ID="lblInfo" runat="server" CssClass="lblInfo" Text="Please enter at least 3 character"></asp:Label>
                    </td>
				</tr>
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR class="tablecol" width="100%">
				    <TD class="tablecol" width="20%"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
					<TD class="TableCol" width="30%"><asp:textbox id="txtSearch" width="99%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></TD>
			        <TD class="TableCol" align="left"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></TD>
			        <td  class="tablecol" ></td>
				</TR>
				<tr><td colSpan="6"><asp:requiredfieldvalidator id="vldSearch" runat="server" Display="None" ErrorMessage="Search criteria cannot be empty." controlToValidate="txtSearch"></asp:requiredfieldvalidator></td></tr>
				<TR>
					<TD class="emptycol" colSpan="6" ></TD>
				</TR>
								<tr>
									<td class="emptycol" colspan="6"><asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
							<Columns>
								
								<asp:TemplateColumn SortExpression="CT_CODE" HeaderText="UNSPSC Code" >
									<HeaderStyle ></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:BoundColumn DataField="Level_1" SortExpression="Level_1" HeaderText="Level 1 Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="Level_2" SortExpression="Level_2" HeaderText="Level 2 Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="Level_3" SortExpression="Level_3" HeaderText="Level 3 Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="Level_4" SortExpression="Level_4" HeaderText="Level 4 Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="CT_ID" Visible="false" HeaderText="CT_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                                        <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" Height="24px" />
                                    </td>
						</tr>
				<tr>
					<td colspan="6">
					    <input type="button" name="cmd_back" value="Close" onclick="window.close(); " id="cmd_back" class="Button" />
                    </td>
				</tr>				
			</TABLE>

    </form>
</body>
</html>
