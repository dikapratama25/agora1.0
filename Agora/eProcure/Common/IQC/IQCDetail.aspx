<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IQCDetail.aspx.vb" Inherits="eProcure.IQCDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>IQC Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
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
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
             
        function showHide1(lnkdesc)
        {
               if (document.getElementById(lnkdesc).style.display == 'none')
               {
	                document.getElementById(lnkdesc).style.display = '';
	                document.getElementById("Image1").src = '<%response.write(collapse_up) %>';
               } 
               else 
               {
    	            document.getElementById(lnkdesc).style.display = 'none';
	                document.getElementById("Image1").src = '<%response.write(collapse_down) %>';
               }
        }  
                
        //-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPRAO_tabs"))%>
			    <table class="alltable" id="Table10" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
                <td colspan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Approve button to approve the IQC or Reject button to reject the IQC."
	                ></asp:label>

                </td>
            </tr>
            <tr>
				<td class="linespacing2" colspan="4"></td>
		    </tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
	                <td class="tableheader" width="100%" colspan="5">&nbsp;IQC Header</td>
                </tr>
			    <tr valign="top">
				    <td class="tablecol" align="left" width="20%">&nbsp;<strong>IQC Number</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblIQCNo" runat="server" Width="202px"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Status</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblStatus" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Item Code</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblItemCode" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>PO Number</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblPONo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Item Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblItemName" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>PO Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblPODate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Purchasing Spec No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblPurSpecNo" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>DO No.</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblDONo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Revision No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblRevision" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Invoice No</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblInvNo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Vendor Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblVendor" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Invoice Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblInvDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;IQC Type</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblIQCType" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Manufacturer</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblManu" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Specification</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec1" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Manufacturer Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblManuDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"></td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec2" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Expiry Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblExpDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"></td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec3" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Received Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblGRNDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Lot No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblLotNo" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px"></td>
				    <td class="Tableinput" width="25%"></td>
			    </tr>
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>Continue Lot</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblContinueLot" Runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>Quantity Received</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblQtyReceived" Runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>UOM</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblUOM" Runat="server"></asp:label></td>
				</tr>
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>File(s) Attached</strong>&nbsp;:</td>
					<td class="Tableinput" colspan="4"><asp:label id="lblFile" Runat="server"></asp:label></td>
				</tr>	  
			</table>
			
			<div id="div_IQC1" style="width:100%; cursor:pointer;"  class="tableheader" onclick="showHide1('IQC')" runat="server"><asp:label id="Label17" runat="server">&nbsp;Approval Workflow Tracking</asp:label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
			<div id="div_IQC2" style="display:inline; width:100%" runat="server">
                        <asp:datagrid id="dtgAppFlowTracking" runat="server" AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="IQCA_SEQ" HeaderText="Level">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
									    <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "IQCA_AO") , DataBinder.Eval( Container.DataItem, "IQCA_A_AO") , DataBinder.Eval( Container.DataItem, "IQCA_SEQ" ), "IQCL", DataBinder.Eval( Container.DataItem, "IQCA_RETEST_COUNT" )) %>' ShowFooter="False" Width="100%" BorderColor="#0000ff" ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
			</div>	
					
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
								<asp:BoundColumn DataField="IQCA_SEQ" HeaderText="Level">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IQCA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
								        <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "IQCA_AO") , DataBinder.Eval( Container.DataItem, "IQCA_A_AO") , DataBinder.Eval( Container.DataItem, "IQCA_SEQ" ), "IQC" ) %>' ShowFooter="False" Width="100%" BorderColor="#0000ff" ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			 </table>
			<table class="AllTable" id="tblSearchResult" cellspacing="0" cellpadding="0" border="0"
				runat="server">
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td align="left">
									<input class="button" id="cmd_preview" type="button" value="View" name="cmd_preview" style="WIDTH: 75px" runat="server"/>
								</td>
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
							runat="server"/><input id="hidType" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidType"
							runat="server"/><input id="hidReject" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidReject"
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
