<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DailyPostingFileRecovery.aspx.vb" Inherits="eAdmin.DailyPostingFileRecovery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File Recovery Confirm List</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        'Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        Dim sPayDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtPaymentDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ & "" align=""absBottom"" vspace=""0""></A>"
        </script>
        
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript"  >
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgComp<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgComp<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtPaymentDate.value="";
		
		}
		 function remove(line)
		    {
		        var check=confirm("Are you sure that you want to delete this item?","");
		        if (check)
		        {
		            document.getElementById("hidlinepointer").value = line;
		            document.all("btnremoveline").click();
		        }
		    }
        function enter_email()
            {
 
              var email=prompt("Please enter email address.","");
               
                if (email!=null && email!="")
                {
                Form1.hidEmail.value = email;
               
                }
               
              else {
              // indicator = 1;
               // alert("Email address is required.");
                return false;
                }                             
            }
			
				    
		-->
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <% Response.Write(Session("w_FileRecovery_tabs"))%>

		<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
			<%--<tr>
				<td class="header"><font size="3">File Recovery</font></td>
			</tr>--%>
			<tr>
                <td class="linespacing1">
                </td>
            </tr>

			<tr>
				<td class="EmptyCol" colspan="5">
				<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
				Text="Select the Payment Date and click on Add To List button to add to Recovery List."></asp:label>
             </td>
			</tr>
			    
		    <tr>
                <td class="linespacing1">
                </td>
            </tr>
        </table>
        
	    <table class="AllTable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="tableheader" colspan="4">&nbsp;Select Date</td>
			</tr>
			
			<tr>
				<td class="tablecol" style="width:120px;">&nbsp;<strong>Payment Date</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong></strong>
				</td>
				<td class="tablecol" style="width:200px;">&nbsp;
				    <asp:textbox id="txtPaymentDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" style="width:120px;"></asp:textbox><% Response.Write(sPayDt)%>
                </td>
				<td class="tablecol"><asp:button id="cmdAddtoList1" runat="server" CssClass="button" Text="Add to List"></asp:button><%--&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" />&nbsp;--%></td>
	        </tr>
			
			
			
		</table>
		<br />
        <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
         
			<tr>
				<td class="tableheader">Recovery List</td>
			</tr>
			<tr>
				<td class="emptycol" width="100%">
				    <asp:datagrid id="dgRecoveryList" CssClass="Grid" AutoGenerateColumns="False" runat="server">
						<Columns>
							 <asp:TemplateColumn HeaderText="Action">						                
							    <ItemStyle Width="5%" />
							       <ItemTemplate>
                                      <span runat="server" style="cursor:pointer;" id="cmdremove"></span>
                                               <%-- <span runat="server" style="cursor:pointer;" id="cmdedit"></span>--%>                                                                                  
							        </ItemTemplate>
						         </asp:TemplateColumn>
							<asp:BoundColumn DataField="frt_gl_date" SortExpression="frt_gl_date" HeaderText="Payment Date">
								<HeaderStyle Width="15%"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Number_of_Record" SortExpression="Number_of_Record" HeaderText="No. of Record" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Total_Debit" SortExpression="Total_Debit" HeaderText="Total Debit" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Total_Credit" SortExpression="Total_Credit" HeaderText="Total Credit" ItemStyle-HorizontalAlign = "Right">
								<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
							</asp:BoundColumn>							
							<%--<asp:BoundColumn DataField="CM_SUB_START_DT" SortExpression="CM_SUB_START_DT" HeaderText="Subscription Start Date"
								DataFormatString="{0:dd/MM/yyyy}">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CM_SUB_END_DT" SortExpression="CM_SUB_END_DT" HeaderText="Subscription End Date"
								DataFormatString="{0:dd/MM/yyyy}">
								<HeaderStyle Width="10%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>--%>
							<asp:BoundColumn DataField="frt_index" SortExpression="frt_index" HeaderText="Status" Visible = "false">
								<HeaderStyle Width="6%"></HeaderStyle>
							</asp:BoundColumn>
						</Columns>
					</asp:datagrid></td>
			</tr>
			<tr>
				<td class="emptycol">&nbsp;&nbsp;</td>
			</tr>
			<tr>
				<td class="emptycol"><%--<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;--%>
				    <asp:button id="cmdSendPreview" runat="server" CssClass="button" Text="Send Preview"></asp:button>&nbsp;
				    <asp:button id="cmdConfirmRecovery" runat="server" CssClass="button" Text="Confirm Recovery"></asp:button>&nbsp;
				    <asp:button id="btnremoveline" runat="server" style="display :none" Text=""></asp:button>
				    <input id="hidlinepointer" type="hidden" size="2" name="hidlinepointer" runat="server" />				
				    <input id="hidEmail" type="hidden" name="hidEmail" runat="server" />				
					<%--<input class="button" id="cmdReset" type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY:none"/>--%></td>
			</tr>
			<tr>
					<td class="emptycol" style="height: 27px">&nbsp;</td>
				</tr>
			<tr>
			<%--	<td colspan="2" class="emptycol"><br/>
					<asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary>
				</td>--%>
				<td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
			</tr>
	
			<!--<tr>
				<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
						<strong>&lt; Back</strong></asp:hyperlink></td>
			</tr>-->
		</table>

	</form>
</body>
</html>
