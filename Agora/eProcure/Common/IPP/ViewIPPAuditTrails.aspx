<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ViewIPPAuditTrails.aspx.vb" Inherits="eProcure.ViewIPPAuditTrails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cost Centre Selection</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
<%--         <% Response.Write(css)%> 
--%>          <script language="javascript">
  		      function closed()
			{			
			
                window.close();                                               
			}
	    </script>   
</head>

    <body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
<%--        <% Response.Write(Session("w_User_tabs"))%>--%>
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
      <%--          <tr>
                    <td class="rowspacing" colspan="5">
                         <asp:Label ID="lblScreenName" runat="server" Text="Audit" CssClass="Header"></asp:Label></td>
                </tr>--%>
								
                <tr><td class="rowspacing" colspan="4" style="height: 19px"></td></tr>	    
				<tr>
					<td class="TableHeader" colspan="4">
                        Document Info.
                    </td>    
				</tr>								
			    <tr>
			        <td class="tablecol" style="width:108px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text=" Document No. :"  CssClass="lbl" Font-Bold="True"></asp:Label></strong>&nbsp;</td>
				    <td class="tablecol" style="width:150px;">&nbsp;<asp:Label id="lblDocNo" runat="server" CssClass="lbl"></asp:Label>                            
                    </td>                 
                    <td class="tablecol" style="width:80px;"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Status :" CssClass="lbl" Font-Bold="True" ></asp:Label></strong>&nbsp;</td>
				    <td class="tablecol" style="width:300px;">&nbsp;<asp:Label id="lblStatus" runat="server" CssClass="lbl"></asp:Label>				        
				    </td>
                </tr>				
			    <tr>
			        <td class="emptycol" colspan="4"></td>
                </tr>	  
		<%--		<tr>
					<td colSpan="2" class="TableCol" style="background:none">
                    <td class="TableCol" colspan="1" style="background: none transparent scroll repeat 0% 0%;
                        width: 100px">
                    </td>
				</tr>--%>
				
			</table>
			
			<table class="AllTable" id="table2"  cellSpacing="0" cellPadding="0" width="100%">
         <TR>				   
			 <TD class="EmptyCol">				    
				<div id="CostCentre" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						
						<asp:datagrid id="dtgIPPAudit" runat="server">
							<Columns>	
								<asp:BoundColumn DataField="ITL_PERFORMED_BY" SortExpression="ITL_PERFORMED_BY" HeaderText="Performed By">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ITL_USER_ID" SortExpression="ITL_USER_ID" HeaderText="User ID">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ITL_TRANS_DATE" SortExpression="ITL_TRANS_DATE" HeaderText="Date Time">
									    <HeaderStyle Width="15%"></HeaderStyle>
								    </asp:BoundColumn>	
								<asp:BoundColumn DataField="ITL_REMARKS" SortExpression="ITL_REMARKS" HeaderText="Remarks">
									    <HeaderStyle Width="50%"></HeaderStyle>
								    </asp:BoundColumn>		    							   
							    </Columns>
						    </asp:datagrid>
				        </div>
				       <%-- <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblCostCentreCode" runat="server" Text="Cost Centre Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblCostCentreDesc" runat="server" Text="Cost Centre Description" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>						    
		                         </div>			                                                       
                                <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>			--%>	       
				    </TD>
				</TR>   
              
            <tr>
				<td class="EmptyCol">					
					<asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="False"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>
                </td>
			</tr>	
        </table> 
		<%--<input type="hidden" id="hidCC" name="hidCC" runat="server"/>
        <input type="hidden" id="hidopenerID" name="hidopenerID" runat="server"/>
        <input type="hidden" id="hidopenerHIDID" name="hidopenerHIDID" runat="server"/>
        <input type="hidden" id="hidopenerbtn" name="hidopenerbtn" runat="server"/> 
        <input type="hidden" id="hidopenerValID" name="hidopenerValID" runat="server"/>   	--%>
		</form>
</body>
</html>