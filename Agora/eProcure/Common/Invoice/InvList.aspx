<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvList.aspx.vb" Inherits="eProcure.InvList" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<html>
	<head>
		<title>Add Invoice</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<%response.write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery"))%>
		
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sPopCalendar As String = "window.open('" & dDispatcher.direct("Initial", "popCalendar.aspx", "pageid=' + strPageId + '&textbox=' + val + '&seldate=' + txtVal.value") & ",'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');"
        </script>	
        <script type="text/javascript">
		<!--	
		$(document).ready(function(){
            $('#cmd_submit').click(function() {
                if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
                {		    
		            return false;
		          }
		        else
                {       
                    document.getElementById("cmd_submit").style.display= "none";
                    document.getElementById("cmdReset").style.display= "none";
                    return true;
                }
            });       
        });
        	
		function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
			{
				if (CheckAtLeastOne(p1,p2)== true) {
					if (resetSummary(cnt1,cnt2)==true){
					//document.getElementById('cmd_submit').style.display='none';			
						return true;}
					else					
						return false;
				}
				else {
					return false;
				}				
			}
			
		function abc(href, shipAmount){
		    alert(document.getElementById(shipAmount).value);
			window.location = href + "&";
			document.forms[0].txt_bcom.value="";
			
		}	
			
		function PromptMsg(msg){
            var result = confirm (msg,"OK", "Cancel");		
					if(result == true)
						Form1.hidresult.value = "1";
					else 
						Form1.hidresult.value = "0";
        }	
			
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtg_InvList_ctl02_chkAll","chkSelection");
		}

		function checkChild(id)
		{
			checkChildG(id,"dtg_InvList_ctl02_chkAll","chkSelection");
		}
		
		function checkChild2(id)
		{
			checkChildG(id,"dtg_inv2_ctl01_chkAll2","chkSelection2");
		}
		
		function selectAll2()
		{
			SelectAllG("dtg_inv2_ctl01_chkAll2","chkSelection2");
		}
				
		function Reset_Step1(){
			//var oform = document.forms[0];
			DeselectAllG("dtg_InvList_ctl02_chkAll","chkSelection");
		}
		
		function Reset_Step2(){
			document.forms[0].reset();
		}
		
		function Reset(){
			document.forms[0].txt_DocNo.value="";
			document.forms[0].txt_bcom.value="";
		}		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}		
		
			function PopWindow2(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		
		function popCalendar(val)
		{
		    debugger;
			txtVal= document.getElementById(val);
//			window.open('../popCalendar.aspx?pageid=' + strPageId + '&textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			<% Response.Write("sPopCalendar") %>
		}
		
        function isNumberKey(evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return (charCode<=31 ||  charCode==46 || (charCode>=48 && charCode<=57));
        }
        
        function isNumericKey(evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return (charCode<=31 || (charCode>=48 && charCode<=57));
        }
        
        function validCurrency(amt)
        {
            return amt.match(/^\d*(.\d{0,2})?$/);
        }
         
        function validateForm(amt)
        {
            if(!validCurrency(amt))
            {
                alert('Invalid Shipping & Handling.');
                return false;
            }
            return true;
        }
        -->
		</script>

	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
              <%  Response.Write(Session("w_SearchGInv_tabs"))%>
					<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing1"></td>
			</tr>
				<tr>
					<td>
						<p>
							<asp:Label id="lblStep1" runat="server" CssClass="lblInfo" Text="Please select the documents to create an invoice to Purchaser. Click 'Next' to proceed"></asp:Label>
							<asp:Label id="lblStep2" CssClass="lblInfo" runat="server" Text="input details to attach to the respective invoices to be created. Select the line items and click on the 'Submit' button to create the invoices."></asp:Label></p>
					</td>
				</tr>
            <tr>
					<td class="linespacing2"></td>
			</tr>
				<tr>
					<td>
						<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<tr>
								<td valign="top" colspan="5" >
								<asp:datagrid id="dtg_InvList" runat="server" CssClass="grid" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
										OnPageIndexChanged="dtg_InvList_Page">
										<Columns>
											<asp:TemplateColumn HeaderText="Delete">
												<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderTemplate>
													<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
												</HeaderTemplate>
												<ItemTemplate>
													<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn SortExpression="POM_BILLING_METHOD" HeaderText="Invoice On">
												<HeaderStyle Width="6%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_PO_NO" HeaderText="PO Number">
												<HeaderStyle Width="17%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="CDM_DO_No" HeaderText="DO Number">
												<HeaderStyle Width="17%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="CDM_GRN_NO" HeaderText="GRN Number">
												<HeaderStyle Width="17%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Purchaser Company">
												<HeaderStyle Width="22%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
												<HeaderStyle Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn HeaderText="Amount">
												<HeaderStyle Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_NO"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="GRN Number"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="DO Number"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_BILLING_METHOD"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_B_COY_ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_INDEX"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PAY_DAY"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="BALSHIP"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
									<asp:datagrid id="dtg_inv2" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:TemplateColumn HeaderText="Delete">
												<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderTemplate>
													<asp:checkbox id="chkAll2" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
												</HeaderTemplate>
												<ItemTemplate>
													<asp:checkbox id="chkSelection2" Runat="server"></asp:checkbox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn HeaderText="Document">
												<HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Reference">
												<HeaderStyle Width="10%" ></HeaderStyle>
												<ItemTemplate>
													<asp:TextBox id="txt_ref" runat="server" CssClass="txtbox" Width="150px" MaxLength="50"></asp:TextBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Add Remarks">
												<HeaderStyle Width="10%" ></HeaderStyle>
												<ItemStyle Wrap="False" ></ItemStyle>
												<ItemTemplate>
													<asp:TextBox id="txt_remark" runat="server" CssClass="Remarks" Width="250px" TextMode="MultiLine"
														Height="35px" MaxLength="1000"></asp:TextBox>
													<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
														 contentEditable="false" ></asp:TextBox>
													<input class="txtbox" id="hidCode" type="hidden" runat="server" name="hidCode"/>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" HeaderText="Details">
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
                                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
                                            </asp:BoundColumn>
											<asp:ButtonColumn Text="See Details" HeaderText="Details" CommandName="Select">
												<HeaderStyle Width="20%" ></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:ButtonColumn>
											<asp:ButtonColumn HeaderText="Invoice No" CommandName="SelectInv">
												<HeaderStyle Width="30%" ></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:ButtonColumn>
											<asp:BoundColumn DataField="Currency" HeaderText="Currency">
												<HeaderStyle Width="10%" ></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left" />
											</asp:BoundColumn>
											<asp:BoundColumn HeaderText="Amount">
												<HeaderStyle Width="10%" ></HeaderStyle>
												<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Tax%" >
									            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									            <ItemStyle HorizontalAlign="Right"></ItemStyle>
									            <ItemTemplate>
										            <asp:TextBox id="txttax" CssClass="numerictxtbox" Width="60px" Runat="server" ></asp:TextBox>
						   			                <asp:RegularExpressionValidator id="revtax" Runat="server" ControlToValidate="txttax" Display="Dynamic" ValidationExpression="^\d{1,4}$"></asp:RegularExpressionValidator>
									            </ItemTemplate>
								            </asp:TemplateColumn>
										    <asp:TemplateColumn HeaderText="Shipping & Handling" >
									            <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									            <ItemStyle HorizontalAlign="Right"></ItemStyle>
									            <ItemTemplate>
										            <asp:TextBox id="txtship" CssClass="numerictxtbox" Width="60px" Runat="server" ></asp:TextBox>
						   			                <asp:RegularExpressionValidator id="revship" Runat="server" ControlToValidate="txtship" Display="Dynamic" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
									            </ItemTemplate>
								            </asp:TemplateColumn>
									        <asp:BoundColumn Visible="False" HeaderText="ref_doc"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="bill_meth"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="po_no"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="grn_no"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="do_no"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="b_com_id"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_INDEX"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PAY_DAY"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PAY_DAY"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="BalShipAmt"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
				<tr>
								<td class="emptycol" colspan="5"><asp:button id="cmd_createInv" runat="server" CssClass="button" Text="Next >" Visible="False"></asp:button>&nbsp;
								<asp:button id="cmd_submit" runat="server" CssClass="button" Text="Submit" Visible="False" ></asp:button>&nbsp;
									<input class="button" id="cmdReset" size="20" type="button" value="Reset" onclick="ValidatorReset();"
										runat="server"/>
								<asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
                                <input class="txtbox" id="hidresult" type="hidden" name="hidresult" runat="server" />							</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/></td>
				</tr>
				<tr>
					<td><a id="back_view" href="#" runat="server"><strong>&lt;Back</strong></a>
					<a id="back" href="#" runat="server"><strong>&lt;Back</strong></a></td>
				</tr>
			</table>
		</form>
	</body>
</html>