<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvTransHistory.aspx.vb" Inherits="eProcure.InvTransHistory" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
  <head>
		<title>Location Description</title>
	    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%> 
		<script language="javascript">
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
		</script>
</head>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_User_tabs"))%>
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="50%">          	
                <tr><td class="rowspacing"></td></tr>			    
				<tr>
					<td class="TableHeader" colSpan="4">Transaction History</td>
				</tr>				
			    <TR class="tablecol">
				    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol"  width="30%"><asp:Label ID="lblItemCode" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>

				    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol" width="30%"><asp:Label ID="lblItemName" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
			    </TR>
			    <TR class="tablecol">
				    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblLoc" runat="server" Text="Location" CssClass="lbl"></asp:Label><asp:Label ID="Label3" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol"  width="30%"><asp:Label ID="lblLoc1" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>

				    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblSubLoc" runat="server" Text="Sub Location" CssClass="lbl"></asp:Label><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol" width="30%"><asp:Label ID="lblSubLoc1" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
			    </TR>
			    <TR class="tablecol">
				    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label4" runat="server" Text="Quantity :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol"  width="30%"><asp:Label ID="lblQty" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
				     <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label6" runat="server" Text="Need QC/Verification :" CssClass="lbl"></asp:Label></strong></TD>
				    <TD class="TableCol"  width="30%"><asp:Label ID="lbl_needqc" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
			    </TR>
			    <tr><td class="rowspacing"></td></tr>
			    <TR>
					<TD class="EmptyCol" colspan="4">
						    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						        <Columns>							       
							        <asp:BoundColumn DataField="IT_TRANS_DATE" SortExpression="IT_TRANS_DATE" HeaderText="Trans Date">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="CODE_DESC" SortExpression="CODE_DESC" HeaderText="Trans Type">
									    <HeaderStyle HorizontalAlign="Left" Width="18%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="TRANS_QTY" SortExpression="TRANS_QTY" HeaderText="Trans Qty">
									    <HeaderStyle HorizontalAlign="Left" Width="10%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
							        <asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Action By">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>							        
							        <asp:BoundColumn Visible="False" DataField="IT_TRANS_INDEX"></asp:BoundColumn>							        
						        </Columns>
					        </asp:datagrid>																	
					</TD>
				</TR>
				<tr><td class="rowspacing" colSpan="2"></td></tr>	
				<tr>
					<td class="EmptyCol" colSpan="2">
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close"></asp:button>&nbsp;
					</td>
				</tr>
			</table>			
		
		</form>
	</body>
</html>
