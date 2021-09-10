<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRDetail.aspx.vb" Inherits="eProcure.PRDetail_SEH" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCollapseUp As String = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
        <!--
        function confirmReject()
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to reject this PR ?");
	        //alert(ans);
	        //return false;	
	        if (ans){			
		        return result;
		        }
	        else
		        return false;
        }
        function confirmApprove(action)
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to " + action + " this PR ?");
	        //alert(ans);
	        //return false;	
	        if (ans){			
		        return result;
		        }
	        else
		        return false;
        }
        
        function showHide(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image1").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image1").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
 
 		    function showHide1(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image2").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image2").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide2(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide3(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
        //-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPR_tabs"))%>
			    <table class="alltable" id="Table10" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
                <td colspan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Duplicate PR button to duplicate the PR or Cancel PR button to cancel the PR."
	                ></asp:label>

                </td>
            </tr>
            <tr>
				<td class="linespacing2" colspan="4"></td>
		    </tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" runat="server">
                <tr>
	                <td class="tableheader" WIdTH="100%" colspan="5">&nbsp;Purchase Request Header</td>
                </tr>
			    <tr valign="top">
				    <td class="tablecol" align="left" width="20%">&nbsp;<strong>PR Number</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblPR" runat="server" Width="202px"></asp:label></td>
				    <td class="tablecol" width="20%">&nbsp;<strong>Status</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblStatus" Runat="server"></asp:label></td><%--<td class="Tableinput" width="30%"><asp:label id="lblDate" runat="server"></asp:label></td>--%>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" width="20%"><strong>&nbsp;Requestor Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblReqName" Runat="server"></asp:label></td>
				    <td class="tablecol" width="20%">&nbsp;<strong>Submitted Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblPRDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" width="20%">&nbsp;<strong>Requestor Contact</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblReqCon" Runat="server"></asp:label></td>
				    <td class="tablecol" width="20%">&nbsp;<strong>Attention To</strong> :&nbsp;</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblAtt" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" width="20%">&nbsp;<strong>Currency</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblCurr" Runat="server"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="Tableinput"></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" width="20%">&nbsp;<strong>Payment Term</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblPayTerm" Runat="server"></asp:label></td>
				    <td class="tablecol" width="20%">&nbsp;<strong>Payment Method</strong> :&nbsp;</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblPayMethod" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" width="20%"><strong>&nbsp;Internal Remarks</strong>&nbsp;:</td>
				    <td class="tableinput" width="30%"><asp:textbox id="txtInternal" ReadOnly="True" runat="server" CssClass="listtxtbox" Width="95%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></td>
				    <td class="tablecol" width="20%">&nbsp;<strong>External Remarks</strong>&nbsp;:</td>
				    <td class="tableinput" width="30%"><asp:textbox id="txtExternal" ReadOnly="True" runat="server" CssClass="listtxtbox" Width="95%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></td>
			    </tr>   
			    
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>Internal File(s) Attached</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblFileInt" Runat="server"></asp:label></td>
				</tr>
			    
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>External File(s) Attached</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblFile" Runat="server"></asp:label></td>
				</tr>			
			    
			</table>						
				<div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ApprFlow')">
					    <asp:label id="Label30" runat="server">Approval Flow</asp:label>
                <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
					    <div id="ApprFlow" style="display:inline"  >
				        <table class="alltable" id="Table5" border="0" width="100%" cellspacing="0"  >
				<tr>
					<td><asp:datagrid id="dtgAppFlow" runat="server" CssClass="Grid" AutoGenerateColumns="False" Width="100%">
							    <Columns>
						            <asp:BoundColumn DataField="PRA_SEQ" HeaderText="Level">
							            <HeaderStyle Width="2%" CssClass="GridHeader"></HeaderStyle>
							            <ItemStyle HorizontalAlign="Right"></ItemStyle>
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
							            <HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
							            <HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PRA_APPROVAL_TYPE" HeaderText="Approval Type">
							            <HeaderStyle Width="12%" CssClass="GridHeader"></HeaderStyle>
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PRA_ACTION_DATE" HeaderText="Action Date">
							            <HeaderStyle Width="12%" CssClass="GridHeader"></HeaderStyle>
							            <ItemStyle HorizontalAlign="Left"></ItemStyle>
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PRA_AO_REMARK" HeaderText="Remarks">
							            <HeaderStyle Width="18%" CssClass="GridHeader"></HeaderStyle>
						            </asp:BoundColumn>
						            <asp:TemplateColumn HeaderText="Attachment">
							            <HeaderStyle Width="18%" CssClass="GridHeader"></HeaderStyle>
							            <ItemTemplate>
								            <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO") , DataBinder.Eval( Container.DataItem, "PRA_A_AO") , DataBinder.Eval( Container.DataItem, "PRA_SEQ" ) ) %>' ShowFooter="False" Width="100%" BorderColor=#0000ff ShowHeader="False">
									            <ItemTemplate>
										            <%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
									            </ItemTemplate>
								            </asp:DataList>
							            </ItemTemplate>
						            </asp:TemplateColumn>
					            </Columns>
				            </asp:datagrid>
						</td>
				</tr>			
				           </table>
	                    </div>			
				
			    <table id="TABLE11" class="tableheader" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
				    <tr id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide1('PRLine')">
				        <td valign="top" class="tableheader" >Purchase Request Line Detail<asp:Image ID="Image2" runat="server" ImageUrl="#" />
			            </td>
			        </tr>
			    </table>
	      		<div id="PRLine" style="display:inline"  >
				    <table class="alltable" id="Table12" border="0" width="100%" cellspacing="0" cellpadding="0" >
				        <tr>
					        <td width="100%"><asp:datagrid id="dtgPRList" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>							
								<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderTemplate>
										(GL Code) GL Description
									</HeaderTemplate>
									<ItemTemplate>
										<%# GenerateGLString( DataBinder.Eval( Container.DataItem , "PRD_B_GL_CODE" ) , DataBinder.Eval( Container.DataItem , "CBG_B_GL_DESC" )  ) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PRD_B_CATEGORY_CODE" HeaderText="Category Code"></asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="PO. No.">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle Width="16%"></HeaderStyle>
								</asp:BoundColumn>
								
								<%--<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label1"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<%--<asp:TemplateColumn HeaderText="MPQ">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label2"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<%--<asp:BoundColumn DataField="PRD_RFQ_QTY" HeaderText="RFQ Qty">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<%--<asp:BoundColumn DataField="PRD_QTY_TOLERANCE" HeaderText="Quotation Qty Tolerance">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<asp:BoundColumn DataField="PRD_ORDERED_QTY" HeaderText="Quantity">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CT_NAME" HeaderText="Commodity <br>Type">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DELIVERY_TERM" HeaderText="Delivery <br>Term">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UOM" HeaderText="UOM">
									<HeaderStyle Width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Unit <br>Price">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Amount">
									<HeaderStyle Width="7%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="AMOUNT" HeaderText="Amount">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>													
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Rate">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Amount">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH begin--%>
								<asp:BoundColumn DataField="PRD_GST_INPUT_TAX_CODE" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH end--%>
								<asp:BoundColumn HeaderText="Budget Account">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery <br>Address">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Item <br>Type">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_LEAD_TIME" HeaderText="Lead Time <br>(Days)">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ETD" HeaderText="Est. Date <br>of Delivery <br>(dd/mm/yyyy)">
									<HeaderStyle Width="5%"></HeaderStyle>									
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_WARRANTY_TERMS" HeaderText="Warranty <br>Terms <br>(mths)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
				        </td>
		            </tr>
	            </table>
            </div>
			<table class="alltable" id="Table7" border="0" width="100%" cellspacing="0" cellpadding="0" >
			    <tr>
					<td class="emptycol" colspan="4"></td>
				</tr>	
				<tr>
					<td class="emptycol" colspan="4">
						<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0">
						    <tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle">&nbsp;
								    <strong><asp:label id="lblRemarkCR" runat="server" Visible="False">Cancel Remarks</asp:label></strong>
								    <asp:label id="lblerrormsg" runat="server" CssClass="errormsg" Enabled="True" Visible="False">* : </asp:label></td>
								<td><asp:textbox id="txtRemarkCR" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox" Visible="False"></asp:textbox></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2">
								    <input class="button" id="cmdPreview" type="button" value="View PR" name="cmdPreview" style="WIDTH: 75px" runat="server"/>
								    <asp:button id="cmdDup" runat="server" Width="100px" CssClass="button" Text="Duplicate PR"></asp:button>
								    <asp:button id="cmdCancel" runat="server" Width="100px" CssClass="button" Text="Cancel PR" Visible="False"></asp:button>&nbsp;&nbsp;&nbsp;
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>	
				<tr>
					<td class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>			
				<tr>
					<td class="emptycol">
						<p><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></p>
					</td>
				</tr>
			</table>
			<asp:button id="cmdPrReport" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 1200px"
				runat="server" Text="View PR Report" Visible="False"></asp:button></form>
	</body>
</html>
