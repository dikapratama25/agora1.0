<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GRNList.aspx.vb" Inherits="eProcure.GRNList" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
		<title>Add GRN</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--
//			function PopWindow(myLoc)
//			{
//				window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
//				return false;
//			}	
			
				
			
		//-->
		</script>
	</head>
	<body >
		<form id="Form1" method="post" runat="server">

              <%  Response.Write(Session("w_SearchGRN_tabs"))%>
					<td class="emptycol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
                            <tr>
					            <td class="header" colspan="4" style="height: 7px"></td>
					        </tr>
                            <tr>
					            <td class="header" colspan="4" style="height: 20px"><font size="1">&nbsp;</font><asp:label id="lblTitle" runat="server" Height="20px">Incoming Delivery Order</asp:label>
					            </td>
					        </tr>
                       </table>
                    </td>
                    <td class="emptycol">
                        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td style="width:100%" colspan="2">
                                    <asp:Label ID="Label1" runat="server" Text="Select/fill in the search criteria and click Search button to list the relevant PO/DO." CssClass="lblInfo"></asp:Label>
                                </td>
                            </tr>
                            <tr>
								<td class="tableheader" style="WIDTH:100%" colspan="2">&nbsp;Search Criteria</td>
							</tr>
							<tr>
							    <td class="tablecol" style="width:111%; HEIGHT:24px">&nbsp;<strong>PO Number</strong>&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtPONumber" runat="server" CssClass="txtbox" Width="200px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                    <strong>DO Number</strong>&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtDONumber" runat="server" CssClass="txtbox" Width="200px"></asp:TextBox>&nbsp;
                                </td>
                                <td align="right" width= "20%">
                                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search"/>&nbsp;
                                    <asp:Button ID="cmdClear" runat="server" CssClass="button" Text="Clear"/>
                                </td>
							</tr>
                        </table>
                   </td>
                    <br/>
               
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
  				<tr>
					<td class="emptycol" style="width: 751px; height: 24px;" colspan="6" ></td>
				</tr>          
					<asp:DataGrid ID="dtgGRNList" runat="server" width="100%" OnSortCommand="SortCommandGRN_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="DOM_DO_NO" HeaderText="DO Number">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="DOM_DO_DATE" HeaderText="DO Date" SortExpression="DOM_DO_DATE">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DOM_DO_Index" HeaderText="DOM_DO_Index" SortExpression="DOM_DO_Index" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_NO" HeaderText="PO Number" SortExpression="POM_PO_NO">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_DATE" HeaderText="PO Date" SortExpression="POM_PO_DATE">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor Name" SortExpression="CM_COY_NAME">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>	            
		</form>
	</body>
</html>
