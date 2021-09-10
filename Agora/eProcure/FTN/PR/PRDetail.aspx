<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PRDetailFTN.aspx.vb" Inherits="eProcure.PRDetailFTN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCollapseUp As String = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
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
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPR_tabs"))%>
			    <table class="alltable" id="Table10" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing2" colSpan="5"></TD>
			</TR>
			<TR>
                <TD colSpan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Duplicate PR button to duplicate the PR or Cancel PR button to cancel the PR."
	                ></asp:label>

                </TD>
            </TR>
            <tr>
				<TD class="linespacing2" colSpan="4"></TD>
		    </TR>
			</table>
			<table class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" runat="server">
                <TR>
	                <TD class="tableheader" WIdTH="100%" colSpan="5">&nbsp;Purchase Request Header</TD>
                </TR>
			    <TR vAlign="top">
				    <TD class="tablecol" align="left" width="20%">&nbsp;<STRONG>PR Number</STRONG>&nbsp;:</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblPR" runat="server" Width="202px"></asp:label></TD>
				    <TD class="tablecol" width="20%">&nbsp;<STRONG>Status</STRONG>&nbsp;:</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblStatus" Runat="server"></asp:label></TD><%--<TD class="TableInput" width="30%"><asp:label id="lblDate" runat="server"></asp:label></TD>--%>
			    </TR>
			    <TR vAlign="top">
				    <TD class="tablecol" width="20%"><strong>&nbsp;Requestor Name</strong>&nbsp;:</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblReqName" Runat="server"></asp:label></TD>
				    <TD class="tablecol" width="20%">&nbsp;<strong>Submitted Date</strong> :&nbsp;</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblPRDate" Runat="server"></asp:label></TD>
			    </TR>
			    <TR vAlign="top">
				    <TD class="tablecol" width="20%">&nbsp;<strong>Requestor Contact</strong>&nbsp;:</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblReqCon" Runat="server"></asp:label></TD>
				    <TD class="tablecol" width="20%">&nbsp;<strong>Attention To</strong> :&nbsp;</TD>
				    <TD class="TableInput" width="30%"><asp:label id="lblAtt" Runat="server"></asp:label></TD>
			    </TR>
			    <TR vAlign="top">
				    <TD class="tablecol" width="20%"><strong>&nbsp;Internal Remarks</strong>&nbsp;:</TD>
				    <TD class="tableinput" width="30%"><asp:textbox id="txtInternal" ReadOnly="True" runat="server" CssClass="listtxtbox" Width="95%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></TD>
				    <TD class="tablecol" width="20%">&nbsp;<strong>External Remarks</strong>&nbsp;:</TD>
				    <TD class="tableinput" width="30%"><asp:textbox id="txtExternal" ReadOnly="True" runat="server" CssClass="listtxtbox" Width="95%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></TD>
			    </TR>   
			    
			    <TR vAlign="top">
					<TD class="tablecol" vAlign="top" >&nbsp;<STRONG>External File(s) Attached</STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="4"><asp:label id="lblFile" Runat="server"></asp:label></TD>
				</TR>			
			    
			</table>						
				<div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ApprFlow')">
					    <asp:label id="Label30" runat="server">Approval Flow</asp:label>
                <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
					    <div id="ApprFlow" style="display:inline"  >
				        <table class="alltable" id="Table5" border="0" width="100%" cellSpacing="0"  >
				<TR>
					<TD><asp:datagrid id="dtgAppFlow" runat="server" CssClass="Grid" AutoGenerateColumns="False" Width="100%">
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
						</TD>
				</TR>			
				           </table>
	                    </div>			
				
			    <TABLE id="TABLE11" class="tableheader" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				    <TR id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide1('PRLine')">
				        <TD vAlign="top" class="tableheader" >Purchase Request Line Detail<asp:Image ID="Image2" runat="server" ImageUrl="#" />
			            </td>
			        </tr>
			    </table>
	      		<div id="PRLine" style="display:inline"  >
				    <table class="alltable" id="Table12" border="0" width="100%" cellSpacing="0" cellPadding="0" >
				        <TR>
					        <TD width="100%"><asp:datagrid id="dtgPRList" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>							
								<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="PO. No.">
									<HeaderStyle Width="15%"></HeaderStyle>
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
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="AMOUNT" HeaderText="Amount">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>		
								<%--Jules 2014.0.23 GST Enhancement--%>
								<asp:BoundColumn DataField="PRD_GST_RATE" HeaderText="GST Rate" Visible="false">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>											
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle Width="3%" HorizontalAlign="right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--Jules 2014.0.23 GST Enhancement end--%>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH begin--%>
								<asp:BoundColumn DataField="PRD_GST_INPUT_TAX_CODE" HeaderText="GST Tax Code (Purchase)"  Visible="false">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH end--%>
								<asp:BoundColumn HeaderText="Budget Account">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery <br>Address">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ETD" HeaderText="Est. Date <br>of Delivery <br>(dd/mm/yyyy)">
									<HeaderStyle Width="5%"></HeaderStyle>									
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_WARRANTY_TERMS" HeaderText="Warranty <br>Terms <br>(mths)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
				        </TD>
		            </TR>
	            </TABLE>
            </div>
			<table class="alltable" id="Table7" border="0" width="100%" cellSpacing="0" cellPadding="0" >
			    <TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>	
				<TR>
					<TD class="emptycol" colSpan="4">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
						    <TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD vAlign="middle">&nbsp;
								    <STRONG><asp:label id="lblRemarkCR" runat="server" Visible="False">Cancel Remarks</asp:label></STRONG>
								    <asp:label id="lblerrormsg" runat="server" CssClass="errormsg" Enabled="True">* : </asp:label></TD>
								<TD><asp:textbox id="txtRemarkCR" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox" Visible="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2">
								    <input class="button" id="cmdPreview" type="button" value="View PR" name="cmdPreview" style="WIDTH: 75px" runat="server"/>
								    <asp:button id="cmdDup" runat="server" Width="100px" CssClass="button" Text="Duplicate PR"></asp:button>
								    <asp:button id="cmdCancel" runat="server" Width="100px" CssClass="button" Text="Cancel PR" Visible="False"></asp:button>&nbsp;&nbsp;&nbsp;
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>	
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>			
				<TR>
					<TD class="emptycol">
						<P><asp:hyperlink id="lnkBack" Runat="server">
								<STRONG>&lt; Back</STRONG></asp:hyperlink></P>
					</TD>
				</TR>
			</TABLE>
			<asp:button id="cmdPrReport" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 1200px"
				runat="server" Text="View PR Report" Visible="False"></asp:button></form>
	</body>
</HTML>
