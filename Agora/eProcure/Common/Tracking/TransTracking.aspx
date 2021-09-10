<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TransTracking.aspx.vb" Inherits="eProcure.TransTracking" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Transaction Tracking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PDFWindow as string = dDispatcher.direct("ExtraFunc","GeneratePDF.aspx", "pageid="" + strPageId + ""&type=INV")
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
            
        </script> 
	    <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
				
		function Reset(){
			ValidatorReset();
			var oform = document.forms(0);
			var e = document.getElementById("txtVendor");
		    var lblName = document.getElementById("lblName");
		    var lblCoyType = document.getElementById("lblCoyType");

			oform.cboViewBy.selectedIndex=0;
			oform.txtDocNo.value="";
			oform.txtVendor.value="";
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
			oform.txtDept.value="";
			oform.txtBuyer.value="";
		    lblName.value = "Buyer Name :";
		    lblName.innerText = "Buyer Name :";
		    lblCoyType.style.display = "block";
		    e.style.display = "block";

				}
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
						
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}				

		function selectAll()
		{
			SelectAllG("dtgTrans_ctl02_chkAll","chkSelection");
		}
		
		function selectAll2()
		{
			SelectAllG("dtgRFQ_ctl02_chkAll2","chkSelection2");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgTrans_ctl02_chkAll","chkSelection");
		}
		
		function checkChild2(id)
		{
			checkChildG(id,"dtgRFQ_ctl02_chkAll2","chkSelection2");
		}
		
		function PDFWindow(strPageId)
		{
			window.open('<%response.write(PDFWindow) %>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}

		function chkViewBy()
		{
		var sel = document.getElementById("cboViewBy");
		var e = document.getElementById("txtVendor");
		var lblName = document.getElementById("lblName");
		var lblCoyType = document.getElementById("lblCoyType");
		if ((sel.value == "PR") || (sel.value == "NPR"))
		{
		e.value="";
		lblCoyType.style.display = "none";
		e.style.display = "none";
		lblName.value = "Buyer Name :";
		lblName.innerText = "Buyer Name :";
		}
		else 
		{
		lblName.value = "Purchaser Name :";
		lblName.innerText = "Purchaser Name :";
		lblCoyType.style.display = "block";
		e.style.display = "block";
		}
		}
		function CheckAtLeastOnePdf(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{						
						return true;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}		
		
			function validateInput(oSrc, args)
			{
				//debugger;
				if ((Form1.txtDocNo.value != '') || (Form1.txtDateFr.value != '') || (Form1.txtDateTo.value != '') || (Form1.txtDept.value != '') || (Form1.txtVendor.value != '') || (Form1.txtBuyer.value != '')) {
					if (((Form1.txtDateFr.value != '') && (Form1.txtDateTo.value != '')) || ((Form1.txtDateFr.value == '') && (Form1.txtDateTo.value == ''))){
						args.IsValid = true;
					}
					else {
						if (Form1.txtDateFr.value == '')
							Form1.document.getElementById("cvSearch").errormessage = 'Start Date is required.';
						else
							Form1.document.getElementById("cvSearch").errormessage = 'End Date is required.';
						args.IsValid = false;
					}
				}
				else {
					Form1.document.getElementById("cvSearch").errormessage = 'At least one search criteria is required.';
					args.IsValid = false;
				}
								
				return args.IsValid;
			}	
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header">Transaction Tracking</td>
				</tr>
            <tr>
					<td class="linespacing1" colspan="4" ></td>
			</tr>
				<tr>
					<td>
						<p><asp:Label id="Label4" runat="server">This list provides a summary of the purchasing activity in your company.</asp:Label></P>
					</td>
				</tr>
                <tr>
					<td class="linespacing2" colspan="4" ></td>
			    </tr>
				<tr>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="tableheader" colspan="5">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol">
								<td width="15%" style="height: 22px"><strong>View By</strong> :</td>
								<td colspan="4" style="height: 22px"><asp:DropDownList id="cboViewBy" runat="server" CssClass="ddl" Width="140px"></asp:DropDownList></td>
							</tr>
							<tr class="tablecol">
								<td width="15%" style="height: 24px"><strong>Document No.</strong> :</td>
								<td style="height: 24px"><asp:textbox id="txtDocNo" runat="server" Width="140px" CssClass="txtbox"></asp:textbox></td>
								<td style="height: 24px"><strong><asp:Label id="lblCoyType" runat="server">Vendor Name:</asp:Label></strong></td>
								<td colspan="2" style="height: 24px"><asp:textbox id="txtVendor" runat="server" Width="252px" CssClass="txtbox"></asp:textbox></td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol" width="15%"> 
                                    <asp:Label ID="lblName" runat="server" Text="Buyer Name : " CssClass="lbl" Font-Bold="True"></asp:Label></td>
								<td><asp:textbox id="txtBuyer" runat="server" Width="140px" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"><strong>Dept.</strong> :</td>
								<td colspan="2" ><asp:textbox id="txtDept" runat="server"  Width="252px"  CssClass="txtbox"></asp:textbox></td>
							</tr>
							<tr class="tablecol">
								<td><strong>Start Date</strong>:</td>
								<td><asp:textbox id="txtDateFr" runat="server" Width="140px" CssClass="txtbox" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%response.write(CalImage) %></a>
								</td>
								<td>	<strong>End Date</strong>:</td>
								<td>	<asp:textbox id="txtDateTo" runat="server" Width="140px" CssClass="txtbox" contentEditable="false"></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%response.write(CalImage) %></a>
								</td>
								<td align="right">&nbsp;<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
									<input  class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;
								</td>
								</tr>
							<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>
						</table>
				</tr>
				<tr>
					<td><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
						<asp:requiredfieldvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Start Date is required."
							Enabled="False"></asp:requiredfieldvalidator>
						<asp:requiredfieldvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="End Date is required."
							Enabled="False"></asp:requiredfieldvalidator><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date should be >= Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>
						<asp:CustomValidator id="cvSearch" runat="server" ErrorMessage="At least one search criteria is required."
							Display="None" ClientValidationFunction="validateInput"></asp:CustomValidator></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgTrans" runat="server" DataKeyField="DOC_INDEX">
							<Columns>
								<%--<asp:TemplateColumn HeaderText="Delete" Visible="False" >
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:TemplateColumn SortExpression="DOC_No" HeaderText="PR Number">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDocNo"></asp:HyperLink>
										<input type="hidden" runat="server" id="hidDocNo" name="hidDocNo">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DOC_Date" SortExpression="DOC_Date" HeaderText="Creation Date">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="80px" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COY_NAME" SortExpression="COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COST" SortExpression="COST" HeaderText="Amount">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Related Documents">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BUYER" SortExpression="BUYER" HeaderText="Buyer Name">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Dept" SortExpression="Dept" HeaderText="Dept.">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="BuyerCoy" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn HeaderText="Hide/Show" Visible="False">
									<ItemStyle VerticalAlign="Top" Width="15%"></ItemStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn HeaderText="PO Number" Visible="false" >
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
								
							</Columns>
						</asp:datagrid>
						<%--For Buyer PO--%>
						<asp:datagrid id="dtgTrans1" runat="server" DataKeyField="DOC_INDEX" Visible="false">
							<Columns>
								<asp:BoundColumn DataField="DOC_No" SortExpression="DOC_No" HeaderText="PO No.">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="80px" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="PR No">
						            <HeaderStyle Width="13%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left"/>
					            </asp:BoundColumn>						            
								<%--<asp:TemplateColumn SortExpression="PRNo" HeaderText="PR No" >
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>										
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:BoundColumn HeaderText="Buyer Name">
						            <HeaderStyle Width="20%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left"/>
					            </asp:BoundColumn>	
								<%--<asp:TemplateColumn SortExpression="BuyerName" HeaderText="Buyer Name" >
									<HeaderStyle HorizontalAlign="Left" Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblBuyerName" Runat="server"></asp:Label>										
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:BoundColumn DataField="DOC_Date" SortExpression="DOC_Date" HeaderText="Creation Date">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="80px" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COY_NAME" SortExpression="COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COST" SortExpression="COST" HeaderText="Amount">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="BUYER" SortExpression="BUYER" HeaderText="Purchaser Name">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Dept" SortExpression="Dept" HeaderText="Dept.">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle ></ItemStyle>
								</asp:BoundColumn>								
                                <asp:BoundColumn DataField="PCM_CR_NO" HeaderText="CR Number" SortExpression="PCM_CR_NO" >
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CDM_DO_NO" HeaderText="DO Number" SortExpression="CDM_DO_NO" >
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CDM_GRN_NO" HeaderText="GRN Number" SortExpression="CDM_GRN_NO" >
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CDM_INVOICE_NO" HeaderText="INV Number" SortExpression="CDM_INVOICE_NO" >
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="DN / DA Number">
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="CN / CA Number">
                                <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				
				<tr>
					<td><asp:datagrid id="dtgRFQ" runat="server">
							<Columns>							
								<asp:TemplateColumn SortExpression="RM_RFQ_NO" HeaderText="RFQ Number">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkRFQNo"></asp:HyperLink>
										<input type="hidden" runat="server" id="hidDocNo2" name="hidDocNo2">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn HeaderText="PR No">
						            <HeaderStyle Width="13%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left"/>
					            </asp:BoundColumn>	
								<%--<asp:TemplateColumn SortExpression="PRNo" HeaderText="PR No" >
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>										
									</ItemTemplate>									
								</asp:TemplateColumn>--%>
								<asp:BoundColumn HeaderText="Buyer Name">
						            <HeaderStyle Width="14%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left"/>
					            </asp:BoundColumn>
								<asp:BoundColumn DataField="RM_RFQ_NAME" SortExpression="RM_RFQ_NAME" HeaderText="RFQ Name">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn HeaderText="Buyer Name">
						            <HeaderStyle Width="14%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left"/>
					            </asp:BoundColumn>--%>	
								<%--<asp:TemplateColumn SortExpression="BuyerName" HeaderText="Buyer Name" >
									<HeaderStyle HorizontalAlign="Left" Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblBuyerName" Runat="server"></asp:Label>										
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:BoundColumn DataField="RM_CREATED_ON" SortExpression="RM_CREATED_ON" HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_EXPIRY_DATE" SortExpression="RM_EXPIRY_DATE" HeaderText="Expiry Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRM_ACTUAL_QUOT_NUM" SortExpression="RRM_ACTUAL_QUOT_NUM" HeaderText="Quotation No.">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_PO_NO" SortExpression="POM_PO_NO" HeaderText="PO No.">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME" HeaderText="Purchaser">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Dept.">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_ID" SortExpression="POM_S_COY_ID" HeaderText="Dept." Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
			</table>
		</form>
			
	</body>
</html>
