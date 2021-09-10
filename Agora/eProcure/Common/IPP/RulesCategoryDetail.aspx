<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RulesCategoryDetail.aspx.vb" Inherits="eProcure.RulesCategoryDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Rules Category Detail</title>
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
					<td class="TableHeader" colSpan="5" style="height: 19px">
                        GL Code Detail
                    </td>    
				</tr>				
			    <tr>
			        <td class="TableCol" style="width: 165px;"><strong><asp:Label ID="Label1" runat="server" Text="GL Code :"  CssClass="lbl" Width="150px"></asp:Label></strong></td>
				    <td class="TableCol" style="width: 180px;">
                        <asp:Label ID="lblGLCode" runat="server"></asp:Label></td>
                    <td class="TableCol" style="width: 1%;"></td>    
                    <td class="TableCol" style="width: 111px;">
                        <strong></strong>
                    </td>
                    <td class="TableCol" style="width: 180px;"></td>
                    
                    
                </tr>				
			    <tr>
				    <td class="TableCol" style="width: 33%"><strong><asp:Label ID="Label2" runat="server" Text=" Description : " CssClass="lbl" ></asp:Label></strong></td>
				    <td class="TableCol" colspan="4" style="width: 100%"><asp:Label id="lblGLCodeDesc" runat="server"></asp:Label>				        
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
								<asp:BoundColumn DataField="igc_glrule_category" SortExpression="igc_glrule_category" HeaderText="Sub Description">
									    <HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="igc_glrule_category_remark" SortExpression="igc_glrule_category_remark" HeaderText="Remarks">
									<HeaderStyle Width="70%"></HeaderStyle>
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
