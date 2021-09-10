<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryVerification.aspx.vb" Inherits="eProcure.InventoryVerification" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Inventory Verification</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
			function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
				return false;
			}	
			
				
			
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" >
		<form id="Form1" method="post" runat="server">
            <% Response.Write(Session("w_InvList_tabs")) %>
		    <TABLE class="AllTable" id="Table4" cellSpacing="0" cellPadding="0">
		    <%--This row is for tabs blue line--%>
		    <tr><td></td></tr>	        
			    <TR>
				    <TD class="Header"><asp:label id="lblTitle" runat="server">Status Update For Outstanding GRN</asp:label></TD>
			    </TR>
                <TR>
				    <TD class="EmptyCol" style="padding-bottom:4px;" >
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the GRN Number to go to Status Update page."></asp:label>
				    </TD>
			    </TR>
			    <TR>
			        <TD class="EmptyCol">
			            <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						    <Columns>
							    <asp:TemplateColumn SortExpression="IV_GRN_NO" HeaderText="GRN Number">
								    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								    <ItemTemplate>
									    <asp:HyperLink Runat="server" ID="lnkGRNNo"></asp:HyperLink>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor">
								    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="POM_PO_NO" SortExpression="POM_PO_NO" HeaderText="PO Number">
									<HeaderStyle HorizontalAlign="Left" Width="15%" ></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOM_DO_NO" SortExpression="DOM_DO_NO" HeaderText="DO Number">
									<HeaderStyle HorizontalAlign="Left" Width="15%" ></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
							    <asp:BoundColumn DataField="GM_CREATED_DATE" SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
								    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
								    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    </asp:BoundColumn>	
							    <asp:BoundColumn Visible="False" DataField="IV_VERIFY_INDEX"></asp:BoundColumn>	
						    </Columns>
					    </asp:datagrid>
				    </TD>
			    </TR>			    
            </TABLE>              
		</form>
	</body>
</HTML>
