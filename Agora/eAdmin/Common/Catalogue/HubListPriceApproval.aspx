<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubListPriceApproval.aspx.vb" Inherits="eAdmin.HubListPriceApproval"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>List Price Catalogue Approval Details</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
           Dim popup As String = "<INPUT class=""button"" id=""cmdSelect"" onclick=""PopWindow('" & dDispatcher.direct("Catalogue", "CommodityType.aspx") & "')"" type=""button"" value=""Search""" & ">"
               </script>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>        
        <% Response.Write(css)%>                                               
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
		$(document).ready(function(){
        $('#cmdApprove').click(function() {
        var formValidate; 
        formValidate = Page_ClientValidate();
        if (formValidate)       
        {
        document.getElementById("cmdApprove").style.display= "none";
        document.getElementById("cmdReject").style.display= "none";
        }
        });
        });
		
		
            function confirmReject()
            {	
	            ans=confirm("Are you sure that you want to reject this item?");
	            //alert(ans);
	            if (ans){	
	                document.getElementById("cmdApprove").style.display= "none";
	                document.getElementById("cmdReject").style.display= "none";		
		            return true;
		            }
	            else
	            {
	                document.getElementById("cmdApprove").style.display= "";
	                document.getElementById("cmdReject").style.display= "";
		            return false;
		            }
            }	
		 function showHide(lnkdesc)
            {
                if (document.getElementById(lnkdesc).style.display == 'none')
                {
	                document.getElementById(lnkdesc).style.display = '';
                } 
                else 
                {
	                document.getElementById(lnkdesc).style.display = 'none';
                }
            }
		function resetPostBack()
			{
				ValidatorReset();
				//Form1.hidPostBack.value = "1";				
			}	
		
			function clearMsg()
			{
				document.getElementById("lblMsg").innerText="";
			}
		
			function PreviewImage(bln)       
			{	
				//bln: 0 - Preview non-uploaded Image
				//     1 - Preview uploaded Image
				var temp;
				if (bln == "0"){
					if (Form1.FileProImage.value != ""){	
						temp='file:///' + Form1.FileProImage.value;
						msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 				msg.document.clear();
						msg.document.write('<HTML><HEAD><TITLE>Image Preview</TITLE></HEAD>'
						+ '<BODY><img src="' + temp + '"></img></BODY></HTML>');
					}
					else {	
						alert("Please select an image !");
					}
				}
				else {
					if (Form1.hidImageFile.value != ""){	
						//temp='file:///' + Form1.hidImageFile.value;
						temp = Form1.hidImageFile.value;
						msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 				msg.document.clear();
						msg.document.write('<HTML><HEAD><TITLE>Image Preview</TITLE></HEAD>'
						+ '<BODY><img src="' + temp + '"></img></BODY></HTML>');
					}
				}
			}	
			
			function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
				return false;
			}	
				
		-->
		
		
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		    
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header" colspan="5"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5">&nbsp;</td>
				</tr>
				<tr>
					<td class="tableheader" colspan="5">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label>
					</td>
				</tr>
				<tr valign="top">
					<td class="tablecol">&nbsp;<strong>Vendor Name</strong>&nbsp;:</td>
					<td class="TableInput"><asp:label id="lblVendorName" runat="server"></asp:label></td>
					<td class="TableInput"></td>
					<td class="tablecol"></td>
					<td class="TableInput"><input id="lblItemId" type="hidden" name="lblItemId" runat="server" /></td>
				</tr>
				<tr valign="top">
	                <td class="tablecol" align="left" width="20%">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Item Code"></asp:Label></strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label9" runat="server" Text=":"></asp:Label></td>
	                <td class="TableInput" width="30%"><asp:textbox id="txtVendorItemCode"  runat="server" CssClass="txtbox" MaxLength="256" Width="100%"></asp:textbox><asp:requiredfieldvalidator id="vldVItemCode" runat="server" Display="None" ErrorMessage="Item Code is required." controlToValidate="txtVendorItemCode"></asp:requiredfieldvalidator><INPUT class="txtbox" id="hidVendorItemCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorItemCode" runat="server"></td>
	                <td class="TableInput" width="1%"></td>
	                <td class="tablecol" width="20%"align="left">&nbsp;<strong><asp:Label ID="Label18" runat="server" Text="Tax"></asp:Label></strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label17" runat="server" Text=":"></asp:Label></td>
	                <td class="TableInput" width="29%"><asp:dropdownlist id="cboTax" Width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldTax" runat="server" Display="None" ErrorMessage="Tax is required." ControlToValidate="cboTax"></asp:requiredfieldvalidator><INPUT class="txtbox" id="hidTax" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidTax" runat="server"></td>
	            </tr>
	            <tr valign="top">
	                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Item Name"></asp:Label></strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label10" runat="server" Text=":"></asp:Label></td>
	                <td class="TableInput" ><asp:textbox id="txtItemName" width="100%" runat="server" CssClass="txtbox" MaxLength="500" ></asp:textbox><asp:requiredfieldvalidator id="vldItemName" runat="server" Display="None" ErrorMessage="Item Name is required." controlToValidate="txtItemName"></asp:requiredfieldvalidator>
	                <INPUT class="txtbox" id="hidItemName" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidItemName" runat="server"></td>
				     <td class="TableInput"></td>
	                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label19" runat="server" Text="Price"></asp:Label></strong><asp:label id="Label20" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label21" runat="server" Text=":"></asp:Label></td>
                    <td class="TableInput" ><asp:textbox id="txtPrice"  Width="100%" runat="server" CssClass="numerictxtbox" MaxLength="50" ></asp:textbox><asp:requiredfieldvalidator id="vldPrice" runat="server" Display="None" ErrorMessage="Price is required." controlToValidate="txtPrice"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revPrice" runat="server" Display="None" ErrorMessage="Catalogue Price is over limit/expecting numeric value." controlToValidate="txtPrice" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:regularexpressionvalidator><INPUT class="txtbox" id="hidPrice" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidPrice" runat="server"></td>                                
	            </tr>
	            <tr valign="top">
	                <td class="tablecol" align="left" rowspan="2">&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Description:"></asp:Label></strong></td>
	                <td class="TableInput" rowspan="2" ><asp:textbox id="txtDesc" width="100%" runat="server" CssClass="txtbox" MaxLength="4000" TextMode="MultiLine" Height="37px" ></asp:textbox>
	                <INPUT class="txtbox" id="hidItemDesc" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidItemDesc" runat="server"></td>
	                <td class="TableInput"rowspan="2" ></td>
	                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label23" runat="server" Text="UOM"></asp:Label></strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label16" runat="server" Text=":"></asp:Label></td>
	                <td class="TableInput"><asp:dropdownlist id="cboUOM"  Width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldUOM" runat="server" Display="None" ErrorMessage="Unit of Measurement is required." ControlToValidate="cboUOM"></asp:requiredfieldvalidator><INPUT class="txtbox" id="hidUOM" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidUOM" runat="server"></td>
	                </tr>
	            <tr valign="top">
                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label25" runat="server" Text="Currency"></asp:Label></strong><asp:label id="Label24" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label26" runat="server" Text=":"></asp:Label></td>
	                <td class="TableInput"><asp:dropdownlist id="cboCurrencyCode"  Width="100%" runat="server" CssClass="ddl"></asp:dropdownlist> <asp:requiredfieldvalidator id="vldCurrencyCode" runat="server" Display="None" ErrorMessage="Currency Code is required." ControlToValidate="cboCurrencyCode"></asp:requiredfieldvalidator><INPUT class="txtbox" id="hidCurrencyCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidCurrencyCode" runat="server"></td>
		        </tr>
					        <tr valign="top">
					            <td class="tablecol" align="left" rowspan="1" >&nbsp;<strong><asp:Label ID="Label13" runat="server" Text="Commodity Type"></asp:Label></strong><asp:label id="Label14" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label15" runat="server" Text=":"></asp:Label></td>
                                <td class="TableInput" rowspan="1"  width="20%"><asp:textbox id="txtCommodityType" runat="server" CssClass="txtbox" MaxLength="50" Width="160px"  contentEditable="false" ></asp:textbox><asp:requiredfieldvalidator id="vldCommodityType" runat="server" Display="None" ErrorMessage="Commodity Type is required." controlToValidate="txtCommodityType"></asp:requiredfieldvalidator><INPUT class="txtbox" id="hidCommodityType" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidCommodityType" runat="server" /> <%Response.Write(popup)%> </td>
								 <td class="TableInput"></td>
		                         <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Reference No. :" ></asp:Label></strong></td>
					            <td class="TableInput"><asp:textbox id="txtRefNo" width="100%" runat="server" CssClass="txtbox" MaxLength="256"></asp:textbox>
					            <INPUT class="txtbox" id="hidRefNo" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidRefNo" runat="server"></td>
					        </tr>
					        <tr valign="top" id="trHidUploadImage" runat="server">
					            <td class="tablecol" noWrap>&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Picture Attachment (JPG & GIF):"></asp:Label></strong>&nbsp;<BR>&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
								<td class="tableinput" colspan="4">
	                        <INPUT class="button" id="FileProImage" style="WIDTH: 249px; HEIGHT: 18px; BACKGROUND-COLOR: #ffffff" type="file" size="20" name="FileProImage" runat="server"><asp:button id="cmdUploadImage" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button>
	                        <INPUT class="button" id="cmdPreview" onclick="PreviewImage('0')" type="button" value="Preview" name="cmdPreview" style="display:none;"><INPUT class="txtbox" id="hidImageAttached" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidImageAttached" runat="server"><INPUT class="txtbox" id="hidImageFile" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidImageFile" runat="server"></td>
				            </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label27" runat="server" Text="Picture Attached :"></asp:Label></strong></td>
					            <td class="tableinput" colspan="4"><asp:panel id="pnlImage" width="100%"  runat="server"></asp:panel></td>
				            </tr>
				            <tr valign="top" id="trHidUploadAttach" runat="server">
			                    <td class="tablecol" noWrap align="left">&nbsp;<strong><asp:Label ID="Label28" runat="server" Text="File Attachment :"></asp:Label></strong>&nbsp;<br>&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" Width="160px">Recommended file size is 300KB</asp:Label></td>
			                    <td class="tableinput" colspan="4"><INPUT class="button" id="File1" style="WIDTH: 249px; HEIGHT: 18px; BACKGROUND-COLOR: #ffffff" type="file" size="20" name="uploadedFile3" runat="server"><asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
			                </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label29" runat="server" Text="File Attached :"></asp:Label></strong></td>
					            <td class="tableinput" ><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
								<td class="TableInput"></td>
					            <td class="TableInput"></td>
					            <td class="TableInput"></td>
	                        </tr>
				<tr id="trDis1" valign="top" runat="server">
					<td class="tablecol" colspan="5" style="height: 20px">
						<hr>
					</td>
				</tr>
				<tr id="trRemark" valign="top" runat="server">
					<td class="tablecol"><strong>&nbsp;Hub Remarks<asp:label id="Label22" runat="server" CssClass="errormsg">*</asp:label></strong>&nbsp;:</td>
					<td class="TableInput" colspan="4" ><asp:textbox id="txtRemark" width="610px" runat="server" CssClass="txtbox" MaxLength="400" Height="37px" Rows="2" TextMode="MultiLine"></asp:textbox></td>
				</tr><%--
				<tr id="trDis2" runat="server">
					<td class="tablecol" colspan="2">&nbsp;<strong>Discount Group</strong></td>
				</tr>
				<tr id="trDis3" valign="top" runat="server">
					<td class="tablecol" colspan="2">&nbsp;<asp:label id="lblDiscGrp" runat="server"></asp:label></td>
				</tr>
				<tr id="trDis4" valign="top" runat="server">
					<td colspan="2"><asp:datagrid id="dtgCatalogue" runat="server" Width="100%" AutoGenerateColumns="false">
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<input runat="server" id="hidCheck" type="hidden" NAME="hidCheck">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PDT_DISC_INDEX" SortExpression="PDT_DISC_INDEX" HeaderText="Index" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="PDT_DISC_CODE" SortExpression="PDT_DISC_CODE" HeaderText="Discount Group Code">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="PDT_DISC_PRICE" HeaderText="Discount Price">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="numerictxtbox" Width="100px" Runat="server" Rows="1" MaxLength="14"></asp:TextBox>
										<asp:TextBox id="txtP" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
										<input runat="server" id="hidPrice" type="hidden" name="hidPrice">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PDT_DISC_REMARK" HeaderText="Discount Remarks">
									<HeaderStyle Width="55%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" Width="400px" CssClass="listtxtbox" Rows="2" Runat="server" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
										<input class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr id="trCat1" valign="top" runat="server">
					<td class="tablecol" colspan="2">
						<hr>
					</td>
				</tr>
				<tr id="trCat2" valign="top" runat="server">
					<td class="tablecol" colspan="2">&nbsp;<strong>Contract Group</strong></td>
				</tr>
				<tr id="trCat3" valign="top" runat="server">
					<td class="tablecol" colspan="2">&nbsp;<asp:label id="lblContGrp" runat="server"></asp:label></td>
				</tr>
				<tr id="trCat4" valign="top" runat="server">
					<td colspan="2"><asp:datagrid id="dtgCat" runat="server" Width="100%" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox id="chkSelection2" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="PDT_DISC_INDEX" SortExpression="PDT_DISC_INDEX" HeaderText="Index"></asp:BoundColumn>
								<asp:BoundColumn DataField="PDT_DISC_CODE" SortExpression="PDT_DISC_CODE" HeaderText="Contract Ref. No.">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="PDT_DISC_PRICE" HeaderText="Contract Price">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtContPrice" CssClass="numerictxtbox" Width="100px" Runat="server" Rows="1"
											MaxLength="14"></asp:TextBox>
										<asp:TextBox id="txtP2" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PDT_DISC_REMARK" HeaderText="Contract Remarks">
									<HeaderStyle Width="55%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtContRemark" Width="400px" CssClass="listtxtbox" Rows="2" Runat="server" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ2" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
										<input class="txtbox" id="hidcode2" type="hidden" runat="server" NAME="hidCode">
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr class="tablecol" id="trRemark2" runat="server">
					<td colspan="2">
						<hr>
					</td>
				</tr>
				<tr id="trRemark" valign="top" runat="server">
					<td class="tablecol">&nbsp;<strong>Remarks<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></strong>&nbsp;:</td>
					<td class="TableInput"><asp:textbox id="txtRemarks" runat="server" CssClass="listtxtbox" Width="400px" MaxLength="400"
							TextMode="MultiLine"></asp:textbox></td>
				</tr>--%>
			</table>
			<div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Item Specification</asp:label></div>
				        <div id="ItemSpec" style="display:inline;">
				        <table class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
			           <tr><td colspan="5"></td></tr> 
				            <tr valign="top">				               
			                    <td class="tablecol" width="23%" align="left" >&nbsp;<strong><asp:Label ID="Label31" runat="server" Text="Brand :"></asp:Label></strong></td>
			                    <td class="TableInput" width="28%" ><asp:textbox id="txtBrand" runat="server" CssClass="txtbox" MaxLength="60" Width="100%"></asp:textbox>
			                        <INPUT class="txtbox" id="hidBrand" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidBrand" runat="server"></td>			                				                
				               	<td class="TableInput" width="1%"></td>
				                <td class="tablecol" width="18%" align="left" >&nbsp;<strong><asp:Label ID="Label32" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="TableInput" width="31%"  ><asp:textbox id="txtModel"  Width="100%" runat="server" CssClass="txtbox" MaxLength="70" ></asp:textbox>
				                    <INPUT class="txtbox" id="hidModel" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							                name="hidModel" runat="server"></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtDrawingNumber"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidDrawingNumber" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidDrawingNumber" runat="server"></td>			                				                
				    			<td class="TableInput"></td>
                            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Version No. :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:textbox id="txtVersionNo"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox>
				                    <INPUT class="txtbox" id="hidVersionNo" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							                name="hidVersionNo" runat="server"></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtGrossWeight"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidGrossWeight" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidGrossWeight" runat="server"></td>			                				                
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:textbox id="txtNetWeight" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox>
				                    <INPUT class="txtbox" id="hidNetWeight" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							                name="hidNetWeight" runat="server"></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtLength" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox><INPUT class="txtbox" id="hidLength" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidLength" runat="server"></td>
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:textbox id="txtWidth" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox><INPUT class="txtbox" id="hidWidth" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidWidth" runat="server"></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label42" runat="server" Text="Packing Specification :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtPacking" runat="server" CssClass="txtbox" MaxLength="256" width="100%" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidPacking" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidPacking" runat="server"></td>			                				                
			              		<td class="TableInput"></td>
                              <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label38" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtVolume" runat="server" CssClass="txtbox" MaxLength="50" width="100%" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidVolume" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVolume" runat="server"></td>			                				                
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtColorInfo" runat="server" CssClass="txtbox" MaxLength="256" width="100%" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidColorInfo" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidColorInfo" runat="server"></td>			                				                
				               	<td class="TableInput"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label37" runat="server" Text="Height (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtHeight" runat="server" CssClass="txtbox" MaxLength="50" width="100%" ></asp:textbox>
			                        <INPUT class="txtbox" id="hidHeight" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidHeight" runat="server"></td>			                				                
				            </tr>
				            <tr valign="top">				               
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>
				                <td class="TableInput" ><asp:textbox id="txtHSCode" runat="server" CssClass="txtbox" MaxLength="256" width="100%"></asp:textbox>
				                    <INPUT class="txtbox" id="hidHSCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidHSCode" runat="server"></td>				
 				               	<td class="TableInput" colspan="3"></td>                           </tr>
				            <tr>
				                <td class="tablecol" rowspan="2"  align="left" style="height: 19px; width: 21%;">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Item Remarks :"></asp:Label></strong></td>
					            <td class="tableinput"  rowspan="2" colspan="4"><asp:textbox id="txtRemarks"  width="100%" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox></td>
							    <td><INPUT class="txtbox" id="hidRemarks" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidRemarks" runat="server"></td>
				            </tr>
				        </table>
				        </div>
				        
				<table class="alltable" id="Table4" width="100%"cellSpacing="0" cellPadding="0" border="0">
				<tr class="emptycol">
					<td colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr id="trSave" runat="server">
					<td align="left" colspan="2" style="height: 18px"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<input class="button" id="cmdReset" onclick="resetPostBack();" type="button" value="Reset"
							name="cmdReset" runat="server">&nbsp;
					</td>
				</tr>
				<tr id="trApprove" runat="server">
					<td align="left" colspan="2"><asp:button id="cmdApprove" runat="server" CssClass="button" Text="Approve"></asp:button>&nbsp;<asp:button id="cmdReject" runat="server" CssClass="button" CausesValidation="False" Text="Reject"></asp:button></td>
				</tr>
				<tr class="emptycol">
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label><input id="hidPostBack" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidPostBack"
							runat="server"><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server">
					</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
