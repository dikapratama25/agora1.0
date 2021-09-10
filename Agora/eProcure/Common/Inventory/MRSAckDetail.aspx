<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MRSAckDetail.aspx.vb" Inherits="eProcure.MRSAckDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>MRSAckDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
		    Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
		    Dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            Dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
            
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery"))%>
		<script type="text/javascript">
        <!--
        function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
        
        function confirmCancel()
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to cancel this MRS ?");
	        if (ans){	            
		        document.getElementById("cmdCancelMRS").style.display= "none";
		        document.getElementById("cmdAckMRS").style.display= "none";
		        return result;
		        }
	        else{
		        return false;
		        }
        }
        
        function confirmAcknowledge()
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to acknowledge this MRS ?");
	        if (ans){	            
	            document.getElementById("cmdCancelMRS").style.display= "none";
		        document.getElementById("cmdAckMRS").style.display= "none";
		        return result;
		        }
	        else{
		        return false;
		        }
        } 
        
        function ShowDialog(filename,height)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 650px");
			//retval=window.open(filename);
			if (retval == "1" || retval =="" || retval==null)
			{  window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
		}
                
        //-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_MRSAckDetail_tabs"))%>
			    <table class="alltable" id="Table10" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
                <td colspan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Acknowledge button to acknowledge the MRS or Cancel button to cancel the MRS."
	                ></asp:label>

                </td>
            </tr>
            <tr>
				<td class="linespacing2" colspan="4"></td>
		    </tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
	                <td class="tableheader" width="100%" colspan="5">&nbsp;MRS Details</td>
                </tr>
			    <tr valign="top">
				    <td class="tablecol" align="left" width="20%">&nbsp;<strong>MRS Number</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblMRSNo" runat="server" Width="202px"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Status</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblStatus" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;MRS Date</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblMRSDate" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px"><strong>&nbsp;MRS Issued Date</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblIssuedDate" runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Requestor Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblReqName" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Section</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSection" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Issue To</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblIssue" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Department</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblDept" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Reference No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblRefNo" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px"></td>
				    <td class="Tableinput" width="25%"></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Internal File(s) Attached</strong>&nbsp;:</td>
				    <td class="Tableinput" colspan="2"><asp:label id="lblFileInt" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px"></td>
			    </tr>  
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Remark</strong>&nbsp;:</td>
				    <td class="Tableinput" colspan="2"><asp:label id="lblRemark" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px"></td>
			    </tr>  
			</table>
							
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">		
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="tableheader">Approval Workflow</td>
				</tr>
				<tr>
					<td width="100%"><asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="IRA_SEQ" HeaderText="Level">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
								        <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "IRA_AO") , DataBinder.Eval( Container.DataItem, "IRA_A_AO") , DataBinder.Eval( Container.DataItem, "IRA_SEQ" ), DataBinder.Eval( Container.DataItem, "TB" ) ) %>' ShowFooter="False" Width="100%" BorderColor="#0000ff" ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
				    <td class="emptycol" style="height: 19px"></td>
			    </tr>
			    <tr>
                    <td width="100%">
                        <asp:datagrid id="dtgItem" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>            
                                <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code">
                                    <HeaderStyle Width="14%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IRSD_INVENTORY_NAME" HeaderText="Item Name">
                                    <HeaderStyle Width="14%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IRSD_UOM" HeaderText="UOM">
                                    <HeaderStyle Width="8%"></HeaderStyle>
                                </asp:BoundColumn>
								<asp:BoundColumn HeaderText="Status" Visible="false">
                                    <HeaderStyle Width="8%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IRSD_IRS_MTHISSUE" HeaderText="Monthly Stock Issued Accumulative">
                                    <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IRSD_IRS_LAST3MTH" HeaderText="Last 3 Mths Ave">
                                    <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundColumn> 
                                <asp:BoundColumn DataField="IRSD_QTY" HeaderText="Qty">
                                    <HeaderStyle HorizontalAlign="Right" Width="12%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundColumn>  
                            </Columns>
                        </asp:datagrid>
                    </td>
                </tr>
			 </table>
			 <table class="AllTable" id="tblSearchResult" cellspacing="0" cellpadding="0" border="0"
				runat="server">
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td>
						<table class="AllTable" id="tblApproval" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle">&nbsp;<strong>Remarks</strong>&nbsp;:</td>
								<td><asp:textbox id="txtRemark" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="900"
										Rows="3" CssClass="listtxtbox"></asp:textbox></td>
							</tr>
							<tr valign="top">
								<td colspan="1" style="height: 38px"><br/>
								</td>
								<td style="height: 38px"></td>
							</tr>
							<tr id="trButton" runat="server">
								<td colspan="2"><asp:button id="cmdAckMRS" runat="server" Width="100px" CssClass="button" Text="Acknowledge"></asp:button><asp:button id="cmdCancelMRS" runat="server" Width="100px" CssClass="button" Text="Cancel"></asp:button><asp:button id="btnhidden2" runat="server" CssClass="Button"  Text="btnhidden2" style=" display :none"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
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
		</form>
	</body>
</html>
