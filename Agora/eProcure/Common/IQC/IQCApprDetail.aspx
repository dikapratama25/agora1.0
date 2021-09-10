<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IQCApprDetail.aspx.vb" Inherits="eProcure.IQCApprDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>IQC Approval</title>
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
		
        function confirmReject()
        {	
	        result=resetSummary(1,0);
	        //alert(result);	
	        if (result)
	        {
	            document.getElementById('hidReject').value = 'w';
	            ans=confirm("Are you sure that you want to waive this IQC ?");
	            if (ans){		
	                document.getElementById("cmdVerifyIQC").style.display= "none";
		            document.getElementById("cmdRejectIQC").style.display= "none";
		            document.getElementById("cmdAppIQC").style.display= "none";
		            document.getElementById("cmdReTestIQC").style.display= "none";				
		            return result;
		            }
	            else
	            {
	                document.getElementById('hidReject').value = 'r';
	                ans=confirm('Are you sure that you want to do replacement for this IQC ?');
	                if (ans){
	                    document.getElementById('cmdRejectIQC').style.display='none';
	                    document.getElementById('cmdReTestIQC').style.display='none';
	                    document.getElementById('cmdAppIQC').style.display='none';
	                    document.getElementById('cmdVerifyIQC').style.display='none';
	                    return result;
	                }
	                else
	                {
	                    document.getElementById('hidReject').value = '';
	                    ans=confirm('Are you sure that you want reject this IQC with no replacement ?');
	                    if (ans){
	                        document.getElementById('cmdRejectIQC').style.display='none';
	                        document.getElementById('cmdReTestIQC').style.display='none';
	                        document.getElementById('cmdAppIQC').style.display='none';
	                        document.getElementById('cmdVerifyIQC').style.display='none';
	                        return result;
	                    }
	                    else
	                    {
	                        return false;
    	             
	                    }      
    	             
	                }
	            }
    	        
	        
	        }
	        else
	        {
	            return false;
	        }	        
        }
        
        
         function confirmRetest()
        {	
	        result=resetSummary(1,0);
	        if (result)
	        {
	            ans=confirm("Are you sure that you want to retest this IQC ?");
	            if (ans)
	            {	            
	                document.getElementById("cmdVerifyIQC").style.display= "none";
		            document.getElementById("cmdRejectIQC").style.display= "none";
		            document.getElementById("cmdAppIQC").style.display= "none";
		            document.getElementById("cmdReTestIQC").style.display= "none";	
		            return result;
		        }
	            else
	            {
		            return false;
		        }
	        }
	        else
	        {
		        return false;
		    }	
	        
        }
        
        function confirmApprove(action)
        {	
	        result=resetSummary(1,0);
	        if (result)
	        {
	            ans=confirm("Are you sure that you want to " + action + " this IQC ?");
	            if (ans)
	            {	            
	                document.getElementById("cmdVerifyIQC").style.display= "none";
		            document.getElementById("cmdRejectIQC").style.display= "none";
		            document.getElementById("cmdAppIQC").style.display= "none";
		            document.getElementById("cmdReTestIQC").style.display= "none";	
		            return result;
		        }
	            else
	            {
		            return false;
		        }
	        }	
	        else
	        {
		        return false;
		    }
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
		<%  Response.Write(Session("w_SearchIQCAO_tabs"))%>
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
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>PO Number</strong>&nbsp;:</td>
				    <td class="Tableinput" width="30%"><asp:label id="lblPONo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Item Code</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblItemCode" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>PO Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblPODate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Item Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblItemName" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>DO No.</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblDONo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Purchasing Spec No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblPurSpecNo" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Invoice No</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblInvNo" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Revision No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblRevision" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Invoice Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblInvDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Vendor Name</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblVendor" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Manufacturer</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblManu" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;IQC Type</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblIQCType" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Manufacturer Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblManuDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"><strong>&nbsp;Specification</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec1" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Expiry Date</strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblExpDate" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"></td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec2" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong><asp:Label ID="lblReceivedDate" runat="server"
                        Text="Received Date"></asp:Label><asp:Label ID="lblManuDate_M" runat="server"
                        Text="Manufacturer Date"></asp:Label></strong> :&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:label id="lblGRNDate" Runat="server"></asp:label>
				    <asp:textbox id="txtManuDate" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox>
				    <span id="span_ManuDate" runat="server"><a onclick="popCalendar('txtManuDate');" href="javascript:;"><% Response.Write(sCal)%></a></span></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol"></td>
				    <td class="Tableinput" width="25%"><asp:label id="lblSpec3" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong><asp:Label
                        ID="lblExpDate_M" runat="server" Text="Expiry Date :"></asp:Label></strong>&nbsp;</td>
				    <td class="Tableinput" width="25%"><asp:textbox id="txtExpDate" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><span id="span_ExpDate" runat="server"><a onclick="popCalendar('txtExpDate');" href="javascript:;"><% Response.Write(sCal)%></a></span></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol" style="height: 24px"><strong>&nbsp;Lot No</strong>&nbsp;:</td>
				    <td class="Tableinput" width="25%" style="height: 24px"><asp:label id="lblLotNo" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px; height: 24px;">&nbsp;<strong><asp:Label
                        ID="lblReceivedDate2" runat="server" Text="Received Date :"></asp:Label></strong>&nbsp;</td>
				    <td class="Tableinput" width="25%" style="height: 24px"><asp:label id="lblGRNDate2" Runat="server"></asp:label></td>
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
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr valign="top">
								<td style="HEIGHT: 17px">&nbsp;<strong>Attachment </strong>&nbsp;:</td>
								<td style="HEIGHT: 22px" colspan="4" rowspan="2"><input class="button" id="File1" style="WIDTH: 448px; HEIGHT: 17px" type="file"
										name="uploadedFile3" runat="server"/>&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><asp:label id="lblFileAO" Runat="server" Visible="False"></asp:label></td>
							</tr>
							<tr valign="top">
								<td>&nbsp;<asp:label id="lblAttach" runat="server" Width="176px"  CssClass="small_remarks">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:label></td>
							</tr>
							<tr valign="top">
								<td style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
								<td style="HEIGHT: 19px" colspan="3"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
							</tr>
							<tr valign="top">
								<td colspan="1" style="height: 38px"><br/>
								</td>
								<td style="height: 38px"></td>
							</tr>
							<tr id="trButton" runat="server">
								<td colspan="2"><asp:button id="cmdVerifyIQC" runat="server" Width="100px" CssClass="button" Text="Verify"></asp:button><asp:button id="cmdAppIQC" runat="server" Width="100px" CssClass="button" Text="Approve"></asp:button><asp:button id="cmdRejectIQC" runat="server" Width="100px" CssClass="button" Text="Reject"></asp:button><asp:button id="cmdReTestIQC" runat="server" Width="100px" CssClass="button" Text="Retest"></asp:button><input class="button" id="cmdClear" onclick="document.forms(0).txtRemark.value=''" type="button" 
										value="Clear" name="cmdClear" runat="server"/><input class="button" id="hidBtnContinue" type="button" value="hidBtnContinue" name="hidBtnContinue" runat="server" style=" display :none"/></td>
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
							runat="server"/><input id="hidAction" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidAction"
							runat="server"/></td>
				</tr>
				<tr>
				    <td class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
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
