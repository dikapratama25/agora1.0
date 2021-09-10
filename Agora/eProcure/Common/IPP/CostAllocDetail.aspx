<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CostAllocDetail.aspx.vb" Inherits="eProcure.CostAllocDetailSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cost Allocation Detail Selection</title>
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
		    function Close()
			{
	            window.close();
	         } 
	    </script>   
</head>
<body>
    <body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
<%--        <% Response.Write(Session("w_User_tabs"))%>--%>
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">            
				<tr>
					<td class="TableHeader" colSpan="5">
                        Cost Allocation Detail
                    </td>    
				</tr>				
			    <tr>
			        <td class="TableCol" style="width: 165px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text=" Cost Allocation Code :"  CssClass="lbl" Width="150px"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol" style="width: 180px;">&nbsp;
                        <asp:Label ID="lblCostAllocCode" runat="server"></asp:Label></td>
                    <td class="TableCol" style="width: 1%;">&nbsp;</td>    
                    <td class="TableCol" style="width: 111px;">
                        <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text=" Amount :"  CssClass="lbl"></asp:Label></strong>&nbsp;
                    </td>
                    <td class="TableCol" style="width: 180px;"><asp:Label ID="lblAmount" runat="server" Width="100%"></asp:Label></td>
                    
                    
                </tr>				
			    <tr>
				    <td class="TableCol" style="width: 33%"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Description :" CssClass="lbl" ></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol" colspan="4" style="width: 100%">&nbsp;<asp:Label id="lblCostAllocCodeDesc" runat="server"></asp:Label>				        
				    </td>
                   
			    </tr>
			    <tr>
			        <td class="rowspacing" colSpan="5"></td>
                </tr>	  
				<tr>					
                    <td class="TableCol" colspan="5" style="background: none transparent scroll repeat 0% 0%;
                        width: 565px">
                    </td>
				</tr>
				
			</table>
			
			<table class="AllTable" id="table2"  cellSpacing="0" cellPadding="0" width="100%">
         <TR>				   
			 <TD class="EmptyCol">
			<%--This is how to make rowspacing inside TD tag--%>					    
				<div id="CostAllocDetail" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						<%--<asp:datagrid id="dtgCostCentre" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="CC_COSTCENTRE_INDEX">--%>
						<asp:datagrid id="dtgCostAllocDetail" runat="server">
							<Columns>																
								<asp:BoundColumn DataField="Branch_Code" SortExpression="Branch_Code" HeaderText="HO/BR">
									    <HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="CC_Code" SortExpression="CC_Code" HeaderText="Cost Centre">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="false" DataField="CC_Desc" SortExpression="CC_Desc" HeaderText="CC_Desc">
									<HeaderStyle Width="1%"></HeaderStyle>
								</asp:BoundColumn>									
								<asp:BoundColumn Visible="False" DataField="Branch_Name" SortExpression="Branch_Name" HeaderText="Branch_Name">
									    <HeaderStyle Width="1%"></HeaderStyle>
								</asp:BoundColumn>
							    <asp:BoundColumn DataField="Percentage" SortExpression="Percentage" HeaderText="% Allocated">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CA_Amount" HeaderText="Allocated Amount">
									    <HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>								    
							</Columns>
						</asp:datagrid>
				    </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblCostCentre" runat="server" Text="Cost Centre" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblBranch" runat="server" Text="HO/BR" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblPercentage" runat="server" Text="% Allocated" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblAllocAmt" runat="server" Text="Allocated Amount" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>						    
		                         </div>			                                                       
                                <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>				       
				    </TD>
				</TR>   
              
            <tr>
				<td class="EmptyCol">
					<INPUT class="button" id="cmdClose" onclick="Close();" type="button" value="Close" >&nbsp;
                </td>
			</tr>	
        </table> 
			
		</form>
</body>
</html>
