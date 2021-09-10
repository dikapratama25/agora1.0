<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POApprDetail.aspx.vb" Inherits="eProcure.POApprDetailFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery")) %>        
		<script language="javascript">
        <!--



        function confirmReject()
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to reject this PO ?");
	        //alert(ans);
	        if (ans){	
	            document.getElementById("cmdRejectPO").style.display= "none";
	            document.getElementById("cmdAppPO").style.display= "none";		
		        return result;
		        }
	        else
	        {
	            document.getElementById("cmdRejectPO").style.display= "";
	            document.getElementById("cmdAppPO").style.display= "";
		        return false;
		        }
        }
        
        function fireHid()
        {
            var bt2 = document.getElementById("btnHidden1"); 
            bt2.click();   
        }
        
        function confirmApprove(action)
        {	
            var sAllClient, iPos, sCurrentClientId;
            var temp;
            var temp2;
            var ONE_DAY = 1000 * 60 * 60 * 24;
            
	        result=resetSummary(1,0);
	        /*var btn = document.getElementById(btnHidden1);
            btn.click();*/
	        ans=confirm("Are you sure that you want to " + action + " this PO ?");
	        if (ans){	
	            sAllClient = Form1.hidClientId.value;
				for (i=0; i < Form1.hidTotalClientId.value; i++)
				{				
				    iPos = sAllClient.indexOf('|');	
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    temp = eval("Form1.dtgPOList_" + sCurrentClientId+"_txtEstDate.value");
				    temp2 = eval("Form1.dtgPOList_" + sCurrentClientId+"_lblCDate.value");  
  
                    date1 = temp.split("/");
                    date2 = temp2.split("/"); 
                    var sDate = new Date(date1[2]+"/"+date1[1]+"/"+date1[0]);
                    var eDate = new Date(date2[2]+"/"+date2[1]+"/"+date2[0]); 
                    if (sDate>eDate) {
                        var daysApart = Math.abs(Math.round((sDate-eDate)/86400000));
                        if (daysApart > 2) {
                            document.getElementById("cmdAppPO").style.display= "none";
                            document.getElementById("cmdRejectPO").style.display= "none";		
	                        //return result;
                        
                        }
                        else
                        {
                            ans2=confirm("Est. Date of Delivery Less Than 2 Days. Do you want to proceed?");
	                        if (ans2){
	                            document.getElementById("cmdAppPO").style.display= "none";
                                document.getElementById("cmdRejectPO").style.display= "none";		
	                            return result;	                                                
	                        }
	                        else
	                        {
	                            document.getElementById("cmdAppPO").style.display= "";
		                        document.getElementById("cmdRejectPO").style.display= "";
		                        return false;
	                        }                            
                        }
                        
                    }
                    else
                    {
                        ans2=confirm("Est. Date of Delivery Less Than 2 Days. Do you want to proceed?");
                        if (ans2){
                            document.getElementById("cmdAppPO").style.display= "none";
                            document.getElementById("cmdRejectPO").style.display= "none";		
                            return result;	                                                
                        }
                        else
                        {
                            document.getElementById("cmdAppPO").style.display= "";
	                        document.getElementById("cmdRejectPO").style.display= "";
	                        return false;
                        }                   
                    }
                    sAllClient = sAllClient.substring(iPos+1);
					
				}
				//Stage 3 Bug fix (GST-0023) - 7/7/2015 - CH
				return result;
//	            if (check=="1") {
//	                ans2=confirm("Est. Date of Delivery Less Than 2 Days. Do you want to proceed?");
//	                if (ans2){	
//                        document.getElementById("cmdAppPO").style.display= "none";
//                        document.getElementById("cmdRejectPO").style.display= "none";		
//	                    return result;		        
//	                    }
//	                else
//	                {
//		                document.getElementById("cmdAppPO").style.display= "";
//		                document.getElementById("cmdRejectPO").style.display= "";
//		                return false;
//		            }
//		        }
		    }
	        else
	        {
		        document.getElementById("cmdAppPO").style.display= "";
		        document.getElementById("cmdRejectPO").style.display= "";
		        return false;
		    }
        }
        //-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_POApprDetail_tabs"))%>
			<TABLE class="alltable" id="Table10" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing2" colSpan="5"></TD>
			</TR>
			<TR>
                <TD colSpan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Approve PO button to approve the PO or Reject PO button to reject the PO."
	                ></asp:label>

                </TD>
            </TR>
            <tr>
				<TD class="linespacing2" colSpan="4"></TD>
		    </TR>
			</Table>
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" WIdTH="100%" colSpan="5">&nbsp;Purchase Order Header</TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="20%">&nbsp;<STRONG>PO Number</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%"><asp:label id="lblPO" Runat="server"></asp:label></TD>
					            <TD class="tablecol"  width="5%"></TD>	
								<TD class="tablecol" width="15%">&nbsp;<STRONG>Purchaser</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="35%"><asp:label id="lblReqName" Runat="server"></asp:label></TD>
					        </TR>
							<TR vAlign="top">
								<TD class="tablecol">&nbsp;<STRONG>Submission Date</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblPODate" Runat="server"></asp:label></TD>
					            <TD class="tablecol"></TD>	
								<TD class="tablecol" >&nbsp;<STRONG>Status</STRONG>&nbsp;:</TD>
								<TD class="TableInput" ><asp:label id="lblStatus" Runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" >&nbsp;<STRONG>Billing Address</STRONG>&nbsp;:</TD>
								<TD class="TableInput" colSpan="4"><asp:label id="lblBillAddr" Runat="server">
								12, Jalan 13/4(Bersatu)<br>
								46200 Petaling Jaya<br>
								Selangor Malaysia<br>
								</asp:label></TD>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" >&nbsp;<STRONG>Internal Remarks</STRONG>&nbsp;:</TD>
								<TD class="TableInput" colSpan="4"><asp:label id="lblInternalRemark"  Runat="server">
								</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<STRONG>Vendor</STRONG>&nbsp;:</TD>
								<TD class="TableInput" ><asp:label id="lblVendor" Runat="server">Kompakar Group of companies</asp:label></TD>
					            <TD class="tablecol"></TD>	
								<TD class="tablecol" >&nbsp;<STRONG>Currency</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblCurrency" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" >&nbsp;<STRONG>External Remarks</STRONG>&nbsp;:</TD>
								<TD class="TableInput" colSpan="4"><asp:label id="lblPORemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST<br>
								</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" >&nbsp;<STRONG>External File(s) Attached</STRONG>&nbsp;:</TD>
								<TD class="TableInput" colSpan="4"><asp:label id="lblFile" Runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol">&nbsp;<STRONG>Payment Terms</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblPT" Runat="server">MOO</asp:label></TD>
					            <TD class="tablecol"></TD>	
								<TD class="tablecol" >&nbsp;<STRONG>Payment&nbsp;Method</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblPM" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<STRONG>Shipment Terms</STRONG>&nbsp;:</TD>
								<TD class="TableInput" ><asp:label id="lblST" Runat="server">MOO</asp:label></TD>
					            <TD class="tablecol"></TD>	
								<TD class="tablecol">&nbsp;<STRONG>Shipment Mode</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblSM" Runat="server">MOO</asp:label></TD>
							</TR>
						</TABLE>
						<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader" width="100%">Approval Workflow</TD>
				</TR>
				<TR>
					<TD class="alltable" cellSpacing="0" cellPadding="0" border="0" ><asp:datagrid id="dtgAppFlow" class="alltable" runat="server" AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="PRA_SEQ" HeaderText="Level">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
										<asp:DataList id=DataList1 runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO") , DataBinder.Eval( Container.DataItem, "PRA_A_AO") , DataBinder.Eval( Container.DataItem, "PRA_SEQ" ) ) %>' ShowFooter="False" Width="100%" BorderColor=#0000ff ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
			<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" border="0"
				runat="server">
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgPOList" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="POD_PO_LINE" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderTemplate>
										(GL Code) GL Description
									</HeaderTemplate>
									<ItemTemplate>
										<%# GenerateGLString( DataBinder.Eval( Container.DataItem , "POD_B_GL_CODE" ) , DataBinder.Eval( Container.DataItem , "CBG_B_GL_DESC" )  ) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" HeaderText="Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" HeaderText="Sub Total">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--Jules 2014.08.08 GST Enhancement--%>
								<asp:BoundColumn DataField="GST_RATE" HeaderText="GST Rate">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn HeaderText="GST Amount">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--Jules 2014.08.08 GST Enhancement end--%>
								<%--Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH begin--%>	
								<asp:BoundColumn DataField="POD_GST_INPUT_TAX_CODE" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle Width="5%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH end--%>
								<asp:BoundColumn HeaderText="Budget Account">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery Address">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="POD_ETD" HeaderText="Est. Date of Delivery (dd/mm/yyyy)">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								
								<asp:TemplateColumn SortExpression="POD_ETD" HeaderText="Est. Date of Delivery (dd/mm/yyyy)">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" CssClass="numerictxtbox" Width="70" Runat="server"></asp:TextBox>
										<asp:TextBox id="lblCDate" CssClass="numerictxtbox" Width="70" Runat="server" style="display:none"></asp:TextBox>
										<%--<asp:Label ID="lblCDate" Runat="server" style="display:none" ></asp:Label>--%>
										<asp:Label ID="lblProductCode" Runat="server" Visible="false" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:BoundColumn DataField="POD_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="tblApproval" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
				                <td rowspan="2" width= "15%" align="left" style="height: 19px; ">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td rowspan="2" width= "85%" ><asp:textbox id="txtRemark" width="100%" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox></td>
				            </tr>

							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD style="HEIGHT: 17px">&nbsp;<STRONG>Internal Attachment </STRONG>&nbsp;:</TD>
								<TD style="HEIGHT: 22px" colSpan="4" rowSpan="2"><INPUT class="button" id="File1" style="WIDTH: 448px; HEIGHT: 17px" type="file"
										name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><asp:label id="lblFileAO" Runat="server" Visible="False"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD>&nbsp;<asp:label id="lblAttach" runat="server" Width="176px"  CssClass="small_remarks">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD style="HEIGHT: 19px">&nbsp;<STRONG>Internal File Attached </STRONG>:</TD>
								<TD style="HEIGHT: 19px" colSpan="3"><asp:panel id="pnlAttach" runat="server"></asp:panel></TD>
							</TR>
							<TR vAlign="top">
								<td colSpan="1"><br>
								</td>
								<td></td>
							</TR>
							<TR id="trButton" runat="server">
								<TD colSpan="2"><asp:button id="cmdAppPO" runat="server" Width="100px" CssClass="button" Text="Approve PO"></asp:button><asp:button id="cmdRejectPO" runat="server" Width="100px" CssClass="button" Text="Reject PO"></asp:button><asp:button id="cmdHoldPO" runat="server" Width="100px" CssClass="button" Text="Hold PO" Visible="False"></asp:button><INPUT class="button" id="cmdClear" onclick="document.forms(0).txtRemark.value=''" type="button"
										value="Clear" name="cmdClear" runat="server">
										<%--<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"></asp:button>--%>
										<INPUT class="button" id="btnHidden1" type="button" value="btnHidden1" style="display:none" name="btnHidden1" runat="server" ><asp:label id="lblCheck" Runat="server" style="display:none"></asp:label> 
										</TD>
										
							</TR>
							<TR id="trMessage" runat="server">
								<TD colSpan="2">
								<asp:label id="lblMsg" Runat="server" CssClass="ErrorMsg">The amount has exceeded your approval limit.</asp:label>
								</TD>								
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD colSpan="2">
								<asp:label id="lblMsg2" runat="server" CssClass="errormsg"></asp:label>
								</TD>								
							</TR>
							
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="tblBuyer" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD vAlign="middle">&nbsp;<STRONG>
										<asp:label id="lblRemarkCR" runat="server" Visible="False">Cancel Remarks :</asp:label></STRONG></TD>
								<TD><asp:textbox id="txtRemarkCR" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox" Visible="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"><asp:button id="cmdDup" runat="server" Width="100px" CssClass="button" Text="Duplicate PO"></asp:button><asp:button id="cmdCancel" runat="server" Width="100px" CssClass="button" Text="Cancel PO" Visible="False"></asp:button>&nbsp;&nbsp;&nbsp;
								</TD>
							</TR>
						</TABLE>
						<P><asp:textbox id="TextBox1" runat="server" Visible="False"></asp:textbox></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server">
							
							<input id="hidClientId" type="hidden" name="hidClientId" runat="server" />
							<input id="hidTotalClientId" type="hidden" name="hidTotalClientId" value="0" runat="server" />
							
							</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<P><asp:hyperlink id="lnkBack" Runat="server">
								<STRONG>&lt; Back</STRONG></asp:hyperlink></P>
					</TD>
				</TR>
			</TABLE>
			</form>
	</body>
</HTML>
