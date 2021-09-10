<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PaymentTypeSearch.aspx.vb" Inherits="eProcure.PaymentTypeSearch" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Payment Type Maintenance</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
        </script>
        <% Response.write(Session("WheelScript")) %>
        <script language="javascript">
        function selectAll()
		{
			SelectAllG("dtgPaymentType_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgPaymentType_ctl02_chkAll","chkSelection");
		}
         function Reset()
		{
		    var oform = document.forms(0);
		    var a = document.getElementById('txtDesc');
		    if(a)
		    {
			    a.value = "";
		    }
		    var b = document.getElementById('txtPaymentType');
		    if(b)
		    {
			    b.value = "";
		    }
		    //uncheck the checkbox
		    document.getElementById('chkStatus_0').checked = false;
		    document.getElementById('chkStatus_1').checked = false; 					    
	    }
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
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
                <tr>
                    <td class="emptycol" colspan="5">
                        <asp:Label ID="lblheader" runat="server" Text="Payment Type Maintenance" CssClass="Header"></asp:Label>
                    </td>
                </tr>
								
                <tr>
                    <td class="rowspacing"></td>
                </tr>
                <tr>
                    <td class="linespacing2"></td>
                </tr>
                <tr>
                    <td class="emptycol" colspan="5">
                        <asp:Label ID="Label3" runat="server" Text="Click the Add button to add new payment type" CssClass="lblinfo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="linespacing2"></td>
                </tr>                	    
				<tr>
					<td class="TableHeader" colspan="5">
                        <asp:Label ID="Label4" runat="server" Text="Search Criteria"></asp:Label>
                    </td>    
				</tr>				
			    <tr>
			        <td class="TableCol" width="15%">
			            <strong><asp:Label ID="Label1" runat="server" Text="Payment Type :"  CssClass="lbl"></asp:Label></strong>
			        </td>
				    <td class="TableCol" width="15%">
				        <asp:textbox id="txtPaymentType" runat="server" MaxLength="30" CssClass="txtbox"></asp:textbox>                            
                    </td>
                    <td class="TableCol" width="15%">
                        <strong><asp:Label ID="Label5" runat="server" Text="Description :"  CssClass="lbl" Width="170px"></asp:Label></strong>
                    </td>
                    <td class="TableCol">
                        <asp:textbox id="txtDesc" runat="server" MaxLength="30" CssClass="txtbox"></asp:textbox>
                    </td>
                    <td class="TableCol">
                        &nbsp;
                    </td>                    
                </tr>				
			    <tr>
				    <td class="TableCol">
				        <strong><asp:Label ID="Label2" runat="server" Text="Status :" CssClass="lbl" ></asp:Label></strong>
				    </td>
				    <td class="TableCol">
                        <asp:CheckBoxList ID="chkStatus" RepeatDirection="Horizontal" runat="server" CssClass="chklist">
                        <asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                        </asp:CheckBoxList>
				    </td>                                    
                    <td class="TableColSearBtn" align="right" colspan="3">
                        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
					    <asp:button cssclass="button" id="cmdClear" runat="server" Text="Clear" name="cmdClear"></asp:button>
					</td>
			    </tr>
			    <tr>
			        <td class="rowspacing"></td>
                </tr>	  								
			</table>
			
			<table class="AllTable" id="table2"  cellSpacing="0" cellPadding="0">
                 <TR>				   
			         <TD class="EmptyCol">				    
					    <div class="rowspacing"></div>
						<asp:datagrid id="dtgPaymentType" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtgPaymentType_PageIndexChanged">
					        <Columns>
					            <asp:TemplateColumn HeaderText="Delete">
					                <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
					                <ItemStyle HorizontalAlign="Center"></ItemStyle>
					                <HeaderTemplate>
					                    <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
					                </HeaderTemplate>
					                <ItemTemplate>
						                <asp:checkbox id="chkSelection" runat="server" AutoPostBack="false"></asp:checkbox>
					                </ItemTemplate>
					            </asp:TemplateColumn>					            
					            <asp:BoundColumn DataField="PT_PT_CODE" SortExpression="PT_PT_CODE" HeaderText="Payment Type">
						            <HeaderStyle Width="30%"></HeaderStyle>
					            </asp:BoundColumn>
					            <asp:BoundColumn DataField="PT_PT_DESC" SortExpression="PT_PT_DESC" HeaderText="Description">
						            <HeaderStyle Width="50%"></HeaderStyle>
					            </asp:BoundColumn>
					            <asp:BoundColumn DataField="PT_STATUS" HeaderText="Status">
						            <HeaderStyle Width="20%"></HeaderStyle>
					            </asp:BoundColumn>
					            <asp:BoundColumn DataField="PT_INDEX" HeaderText="index" Visible = "false">
					            </asp:BoundColumn>									   
							</Columns>
						</asp:datagrid>
				    </TD>
				</TR>   
                <tr>
				    <td class="EmptyCol">
			            <asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>
			            <asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>
			            <asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>			            
			            <asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none;"></asp:button>
			            <asp:Button ID="btnHidden" runat="server" OnClick="btnHidden_Click" Style="display: none" />
                    </td>
			    </tr>	
        </table> 
		</form>
</body>
</html>