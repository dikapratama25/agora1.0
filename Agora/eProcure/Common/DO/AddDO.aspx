<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddDO.aspx.vb" Inherits="eProcure.AddDO" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AddDO</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
       <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim RefDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtOurRefDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
      <% Response.Write(Session("JQuery")) %>        
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--
		
		$(document).ready(function(){
        $('#cmdsubmit').click(function() {
        if (Page_IsValid)
        {    
            if (document.getElementById("cmdsubmit"))   
            {document.getElementById("cmdsubmit").style.display= "none";}
            if (document.getElementById("cmdsave"))
            {document.getElementById("cmdsave").style.display= "none";}
            if (document.getElementById("cmdReset"))
            {document.getElementById("cmdReset").style.display= "none";}
            if (document.getElementById("cmdDeleteDO"))
            {document.getElementById("cmdDeleteDO").style.display= "none";}
        }
        });
        $('#cmdsave').click(function() {
        if (Page_IsValid)
        {       
           if (document.getElementById("cmdsubmit"))   
            {document.getElementById("cmdsubmit").style.display= "none";}
            if (document.getElementById("cmdsave"))
            {document.getElementById("cmdsave").style.display= "none";}
            if (document.getElementById("cmdReset"))
            {document.getElementById("cmdReset").style.display= "none";}
            if (document.getElementById("cmdDeleteDO"))
            {document.getElementById("cmdDeleteDO").style.display= "none";}
        }
        });
        });
		
			function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
				return false;
			}	
			
			function ShowDialog(filename,height)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 750px");
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
        <%  Response.Write(Session("w_AddDO_tabs"))%>
            
             	<table class="alltable" id="tblDOHeader1" width="100%" cellspacing="0" cellpadding="0" border="0">
				 <tr>
					<td class="linespacing1" colspan="4"></td>
			    </tr>
			    <tr>
				    <td colspan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the Save button to save the new PO as draft DO. Click the Submit button to submit the DO to the buyer."
					    ></asp:label>

				    </td>
			    </tr>
                <tr>
					    <td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server" Font-Bold="True" Width="273px">Delivery Order Details</asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="DO Number :" ></asp:Label></strong></td>
					<td class="Tableinput" style="height: 19px; width: 191px;" ><asp:label id="lblDONo" runat="server" ></asp:label>
                        <asp:Label ID="lblDraft" runat="server" ForeColor="Red" Text="(Open)"></asp:Label></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label9" runat="server" Text="Delivery Date :" Width="106px"></asp:Label></strong></td>
					<td class="Tableinput" Width="50%" style="height: 19px" ><asp:label id="lblDevlDate" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="PO Number :" ></asp:Label></strong></td>
					<td class="Tableinput" style="width: 191px; height: 19px;" ><asp:label id="lblPONo" runat="server"></asp:label></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label5" runat="server" Text="Customer Name :"></asp:Label></strong></td>
					<td class="tableinput" style="height: 19px"><asp:label id="lblCustName" runat="server" Width="337px" ></asp:label></td>
				</tr>
		        <tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Payment Terms :"></asp:Label></strong></td>
					<td class="Tableinput" style="width: 191px" ><asp:label id="lblPayTerm" runat="server"></asp:label></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label4" runat="server" Text="Shipment Terms :"></asp:Label></strong></td>
					<td class="tableinput"><asp:label id="lblShipTerm" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label11" runat="server" Text="Payment Method :"></asp:Label></strong></td>
					<td class="Tableinput" style="height: 19px; width: 191px;"><asp:label id="lblPayMthd" runat="server"></asp:label></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label12" runat="server" Text="Shipment Method :"></asp:Label></strong></td>
					<td class="tableinput" width="25%" style="height: 19px"><asp:label id="lblShipMthd" runat="server"></asp:label></td>
				</tr>
				<tr id="tr_delTerm" runat="server">
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label16" runat="server" Text="Delivery Term :"></asp:Label></strong></td>
					<td class="Tableinput" style="height: 19px; width: 191px;"><asp:label id="lblDelTerm" runat="server"></asp:label></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"></td>
					<td class="tableinput" width="25%" style="height: 19px"></td>
				</tr>
                <tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px" valign="top"><strong>&nbsp;<asp:Label ID="Label10" runat="server" Text="Ship To :"></asp:Label></strong></td>
					<td class="Tableinput" colspan="4" width="25%"><asp:dropdownlist id="cboDelvAdd" runat="server" CssClass="ddl" AutoPostBack="True" Width="130px"></asp:dropdownlist>
                        <br />
                        <asp:label id="lblDelvAdd" runat="server" >lblDelvAdd</asp:label></td>
                </tr>
				<tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px" ><strong>&nbsp;<asp:Label ID="Label6" runat="server" Text="Air Way Bill No. :" ></asp:Label></strong></td>
					<td class="tableinput" style="width: 191px" ><asp:textbox id="txtAWBillNo" runat="server" CssClass="txtbox" MaxLength="30"></asp:textbox></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label13" runat="server" Text="Our Ref. No. :"></asp:Label></strong></td>
					<td class="tableinput" style="width: 194px"><asp:textbox id="txtOurRefNo" runat="server" CssClass="txtbox" MaxLength="30"></asp:textbox></td>
				</tr>
				<tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 24px"><strong>&nbsp;<asp:Label ID="Label7" runat="server" Text="Freight Carrier :" ></asp:Label></strong></td>
					<td class="tableinput" style="width: 191px; height: 24px;"><asp:textbox id="txtFreCarier" runat="server" CssClass="txtbox" MaxLength="30"></asp:textbox></td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 24px"><strong>&nbsp;<asp:Label ID="Label14" runat="server" Text="Our Ref. Date :"></asp:Label></strong></td>
					<td class="tableinput" style="width: 194px; height: 24px;"><asp:textbox id="txtOurRefDate" runat="server" CssClass="txtbox"  contentEditable="false" ></asp:textbox><% Response.Write(RefDate)%></td>
				</tr>
				<tr>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px" valign="top"><strong>&nbsp;<asp:Label ID="Label15" runat="server" Text="Freight Amount :"></asp:Label></strong></td>
					<td class="tableinput" valign="top" style="width: 191px">
					<asp:textbox id="txtFreAmt" runat="server" CssClass="txtbox"></asp:textbox>
					<asp:regularexpressionvalidator id="revFreight" runat="server" ValidationExpression="^\d{0,10}(\.\d{1,2})?$" Display="None"
							ControlToValidate="txtFreAmt" ErrorMessage="Freight Amount is over limit/expecting numeric value."></asp:regularexpressionvalidator>
					</td>
					<td class="tablecol"  align="left"  Width="15%" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label8" runat="server" Text="Remarks :"></asp:Label></strong></td>
					<td class="tableinput" ><asp:textbox id="txtRemarks" runat="server" CssClass="txtbox" MaxLength="1000"
							Height="37px" Rows="2" TextMode="MultiLine" Width="400px"></asp:textbox></td>
				</tr>
				<tr>
				
								<td class="tablecol" noWrap align="left">&nbsp;<strong>Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                    <td class="tableinput" colspan="5">
			                    <input class="button" id="FileDoc" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" 
			                    type="file" name="uploadedFile3" runat="server" />&nbsp;
			                    <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
								</tr>
								<tr valign="top">
									<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
									<td class="tableinput" style="HEIGHT: 19px" colspan="5"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
								</tr>
				
				
			</table>
			<table class="alltable" id="tblDOHeader" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol1">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgDODtl" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:BoundColumn DataField="POD_Po_Line" SortExpression="POD_Po_Line" HeaderText="POLine" Visible="False">
									<HeaderStyle HorizontalAlign="Right" Width="1%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="Line" >
								    <HeaderStyle HorizontalAlign="Right" Width="4%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" />
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Vendor_Item_Code" SortExpression="POD_Vendor_Item_Code" HeaderText="Item Code">
									<HeaderStyle Width="6%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Item Name">
									<HeaderStyle Width="13%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="4%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ETD" SortExpression="POD_ETD" HeaderText="EDD (Date)">
									<HeaderStyle HorizontalAlign="Left" Width="5%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Warranty_Terms" SortExpression="POD_Warranty_Terms" HeaderText="Warranty&lt;BR&gt;Terms(Mths)">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Pack_Qty" SortExpression="POD_Min_Pack_Qty" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Order_Qty" SortExpression="POD_Min_Order_Qty" HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Ordered_Qty" SortExpression="POD_Ordered_Qty" HeaderText="Ordered">
									<HeaderStyle HorizontalAlign="Right" Width="4%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Outstanding" SortExpression="POD_Outstanding" HeaderText="Outstd">
									<HeaderStyle HorizontalAlign="Right" Width="4%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Ship">
									<HeaderStyle HorizontalAlign="Right" Width="3%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
										<asp:textbox ID="txtShiped" Runat="server" cssclass="numerictxtbox" Width="35px"></asp:textbox>
										<asp:RegularExpressionValidator id="revShipped" runat="server" ControlToValidate="txtShiped" Display="Dynamic"></asp:RegularExpressionValidator>
										<asp:CompareValidator Runat="server" ID="cvShipped" Display="Dynamic" ControlToValidate="txtshiped"></asp:CompareValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Total Lot No">
									<HeaderStyle HorizontalAlign="Left" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
										<asp:label ID="lblLotNo" Runat="server" cssclass="numerictxtbox" Width="54%"></asp:label>
										<asp:Button ID="btn_lot" runat="server" CssClass="Button" Text="Set" width="40%"/>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Remarks" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="5%" ></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:textbox ID="txtDtlRemarks" Runat="server" cssclass="txtbox" TextMode="MultiLine"
											Rows="2" Height="30px" MaxLength="400" style="width: 150px"></asp:textbox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="1%" ForeColor="Red"
											 contentEditable="false" Visible="false" ></asp:TextBox>
										<input class="txtbox" id="hidCode" type="hidden" runat="server" name="hidCode"/>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_CODE" SortExpression="POD_PRODUCT_CODE" HeaderText="ProductCode" Visible="False">
									<HeaderStyle HorizontalAlign="Right" Width="1%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 19px">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td><asp:label id="lblSummPO" runat="server" Font-Bold="True">Delivery Order Summary For Purchase Order: </asp:label>&nbsp;&nbsp;
						<asp:label id="lblPONum" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:datagrid id="DtgDoSumm" runat="server">
							<Columns>
								<asp:BoundColumn DataField="date_created" HeaderText="DO Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="DO Number">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" HeaderText="Created By">
									<HeaderStyle HorizontalAlign="Left" Width="55%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>						
				</tr>
				<tr>
					<td style="height: 24px"><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save" Enabled="False" width="75px"></asp:button><asp:button id="cmdsubmit" runat="server" CssClass="button" Text="Submit" Enabled="False" width="75px"></asp:button><asp:button id="cmdDeleteDO" runat="server" CssClass="button" Text="Delete DO" CausesValidation="False" width="75px"></asp:button><input class="button" id="cmdReset" disabled onclick="ValidatorReset();" type="button" style="WIDTH: 75px" 
							value="Reset" name="cmdReset" runat="server"/> <input class="button" id="cmdPreviewDO" type="button" value="View DO" name="cmdPreviewDO"
							runat="server" style="width: 75px"/><asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/></td>
				</tr>
				<tr>
						<td class="emptycol" colspan="4"> <asp:label id="lbl_check" runat="server" CssClass="errormsg" Width="400px" ForeColor="Red"></asp:label></td>
					</tr>
				<tr>
					<td class="emptycol" style="height: 19px">
    				    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
			    
             
		</form>
	</body>
</html>
