<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CatalogueNewProduct.aspx.vb" Inherits="eProcure.CatalogueNewProduct" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CatalogueNewProduct</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim popup As String = "<INPUT class=""button"" id=""cmdSelect"" onclick=""PopWindow('" & dDispatcher.direct("Catalogue", "CommodityType.aspx") & "')"" type=""button"" value=""Search""" & ">"
		    Dim collapse_up As String = dDispatcher.direct("Plugins/images", "collapse_up.gif")
		    Dim collapse_down As String = dDispatcher.direct("Plugins/images", "collapse_down.gif")
               </script>
	
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--	
		    function showHide(lnkdesc)
            {
                if (document.getElementById(lnkdesc).style.display == 'none')
                {
	                document.getElementById(lnkdesc).style.display = '';
	                document.getElementById("Image1").src = '<% Response.Write(collapse_up) %>';
                } 
                else 
                {
	                document.getElementById(lnkdesc).style.display = 'none';
	                document.getElementById("Image1").src = '<% Response.Write(collapse_down) %>';
                }
            }

		function PromptMsg(msg){
            var result = confirm (msg,"OK", "Cancel");		
					if(result == true)
						Form1.hidresult.value = "1";
					else 
						Form1.hidresult.value = "0";
            }	
			
			function resetPostBack()
			{			
				//alert('11');
				//summary = Page_ValidationSummaries[0];
				//summary.style.display = "";
				//summary.innerHTML = "";
				//Page_IsValid = true;
				ValidatorReset();		
				//Form1.hidPostBack.value = "1";							
			}
		
			function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
				return false;
			}		
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}		
				
			function PreviewImage(bln)       
			{	
				//bln: 0 - Preview non-uploaded Image
				//     1 - Preview uploaded Image
				var temp;
				if (bln == "0"){
					if (Form1.FileProImage.value != ""){	
						temp='file:///' + Form1.FileProImage.value;
						//temp='..' + Form1.FileProImage.value;
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
						temp= Form1.hidImageFile.value;
						//temp='..' + Form1.hidImageAttached.value;
						msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 				msg.document.clear();
						msg.document.write('<HTML><HEAD><TITLE>Image Preview</TITLE></HEAD>'
						+ '<BODY><img src="' + temp + '"></img></BODY></HTML>');
					}
				}
			}	
			
			/*function PreviewAttach(fle)       
			{	
				//bln: 0 - Preview non-uploaded Image
				//     1 - Preview uploaded Image
				var temp;
				if (fle != ""){	
					//temp='file:///' + fle;
					temp= fle;
					//temp='..' + Form1.hidImageAttached.value;
					msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 			msg.document.clear();
					msg.document.write('<HTML><HEAD><TITLE>Image Preview</TITLE></HEAD>'
					+ '<BODY><img src="' + temp + '"></img></BODY></HTML>');
				}
			}*/	
						
								
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		    <input class="txtbox" id="hidProductCode" type="hidden" name="hidProductCode" runat="server" />
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td class="header" colspan="2"><asp:label id="lblTitle" runat="server" CssClass="header">Item Master</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">&nbsp;</td>
				</tr>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the required field(s) and click the Submit button to add new item."
		                ></asp:label>

	                </TD>
                </TR>
				<tr>
					<td class="tableheader" colspan="5" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server">Item Information</asp:label>
					</td>
				</tr>
				<tr>
				    </table>
				        <table class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
				            <tr valign="top">
					            <td class="tablecol" align="left" width="20%">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Item Code"></asp:Label></strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label9" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput" width="30%"><asp:textbox id="txtVendorItemCode"  runat="server" CssClass="txtbox" MaxLength="256" Width="100%"></asp:textbox><asp:requiredfieldvalidator id="vldVItemCode" runat="server" Display="None" ErrorMessage="Item Code is required." controlToValidate="txtVendorItemCode" EnableClientScript="False"></asp:requiredfieldvalidator><input id="hidVendorItemCode" type="hidden" name="hidVendorItemCode" runat="server" /></td>
					            <td class="Tableinput" width="1%"></td>
					            <td class="tablecol" width="20%"align="left">&nbsp;<strong><asp:Label ID="lblTax" runat="server" Text="SST Rate"></asp:Label></strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label17" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput" width="29%"><asp:dropdownlist id="cboGSTRate" Width="98%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldTax" runat="server" Display="None" ErrorMessage="SST Rate is required." ControlToValidate="cboGSTRate" EnableClientScript="False"></asp:requiredfieldvalidator><input class="txtbox" id="hidTax" type="hidden" name="hidTax" runat="server" /></td>
					        </tr>
					        <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Item Name"></asp:Label></strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label10" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput" ><asp:textbox id="txtItemName" width="309px" runat="server" CssClass="txtbox" MaxLength="500" ></asp:textbox><asp:requiredfieldvalidator id="vldItemName" runat="server" Display="None" ErrorMessage="Item Name is required." controlToValidate="txtItemName" EnableClientScript="False"></asp:requiredfieldvalidator><input class="txtbox" id="hidItemName" type="hidden" name="hidItemName" runat="server" /></td>
								<td class="Tableinput"></td>
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label19" runat="server" Text="Price"></asp:Label></strong><asp:label id="Label20" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label21" runat="server" Text=":"></asp:Label></td>
                                <td class="Tableinput" ><asp:textbox id="txtPrice"  Width="98%" runat="server" CssClass="numerictxtbox" MaxLength="50" ></asp:textbox><asp:requiredfieldvalidator id="vldPrice" runat="server" Display="None" ErrorMessage="Price is required." controlToValidate="txtPrice" EnableClientScript="False"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revPrice" runat="server" Display="None" ErrorMessage="Catalogue Price is over limit/expecting numeric value." controlToValidate="txtPrice" EnableClientScript="False" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:regularexpressionvalidator><input id="hidPrice" type="hidden" name="hidPrice" runat="server" /></td>
					        </tr>
					        <tr valign="top">
					            <td class="tablecol" align="left" rowspan="2">&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Description:"></asp:Label></strong></td>
					            <td class="Tableinput" rowspan="2" ><asp:textbox id="txtDesc" width="100%" runat="server" CssClass="txtbox" MaxLength="4000" TextMode="MultiLine" Height="37px" ></asp:textbox><input class="txtbox" id="hidItemDesc" type="hidden" name="hidItemDesc" runat="server" /></td>
					            <td class="Tableinput"rowspan="2" ></td>
					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label23" runat="server" Text="UOM"></asp:Label></strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label16" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput"><asp:dropdownlist id="cboUOM"  Width="98%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldUOM" runat="server" Display="None" ErrorMessage="Unit of Measurement is required." ControlToValidate="cboUOM" EnableClientScript="False"></asp:requiredfieldvalidator><input class="txtbox" id="hidUOM" type="hidden" name="hidUOM" runat="server" /></td>
					            </tr>
					        <tr valign="top">
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label25" runat="server" Text="Currency"></asp:Label></strong><asp:label id="Label24" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label26" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput"><asp:dropdownlist id="cboCurrencyCode"  Width="98%" runat="server" CssClass="ddl"></asp:dropdownlist> <asp:requiredfieldvalidator id="vldCurrencyCode" runat="server" Display="None" ErrorMessage="Currency Code is required." ControlToValidate="cboCurrencyCode" EnableClientScript="False"></asp:requiredfieldvalidator><input class="txtbox" id="hidCurrencyCode" type="hidden" name="hidCurrencyCode" runat="server" /></td>
		                    </tr>
					        <tr valign="top">
                                <td class="tablecol" align="left" rowspan="1" >&nbsp;<strong><asp:Label ID="Label13" runat="server" Text="Commodity Type"></asp:Label></strong><asp:label id="Label14" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label15" runat="server" Text=":"></asp:Label></td>
                                <td class="Tableinput" rowspan="1" width="20%"><asp:textbox id="txtCommodityType" runat="server" CssClass="txtbox" MaxLength="50" Width="249px"  contentEditable="false" ></asp:textbox><asp:requiredfieldvalidator id="vldCommodityType" runat="server" Display="None" ErrorMessage="Commodity Type is required." controlToValidate="txtCommodityType" EnableClientScript="False"></asp:requiredfieldvalidator><input class="txtbox" id="hidCommodityType" type="hidden" name="hidCommodityType" runat="server" /><input class="txtbox" id="hidCurrentCommodityType" type="hidden" name="hidCurrentCommodityType" runat="server" /><%Response.Write(popup)%></td>
                                <td class="Tableinput"></td>
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Reference No. :" ></asp:Label></strong></td>
                                <td class="Tableinput"><asp:textbox id="txtRefNo" width="98%" runat="server" CssClass="txtbox" MaxLength="256"></asp:textbox><input id="hidRefNo" type="hidden" name="hidRefNo" runat="server" /></td>
					        </tr>
					        <tr valign="top">
                                <td class="tablecol" noWrap>&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Picture Attachment (JPG & GIF):"></asp:Label></strong>&nbsp;<br />&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
                                <td class="tableinput" colspan="4"><input class="button" id="FileProImage" style="WIDTH: 249px; HEIGHT: 18px; BACKGROUND-COLOR: #ffffff" type="file" size="20" name="FileProImage" runat="server" /><asp:button id="cmdUploadImage" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><input class="button" style="display:none; " id="cmdPreview" onclick="PreviewImage('0')" type="button" value="Preview" name="cmdPreview" /><input id="hidImageAttached" type="hidden" name="hidImageAttached" runat="server" /><input class="txtbox" id="hidImageFile" type="hidden" name="hidImageFile" runat="server" /></td>
				            </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label27" runat="server" Text="Picture Attached :"></asp:Label></strong></td>
					            <td class="tableinput" colspan="4"><asp:panel id="pnlImage" width="100%"  runat="server"></asp:panel></td>
				            </tr>
				            <tr valign="top">
			                    <td class="tablecol" noWrap align="left">&nbsp;<strong><asp:Label ID="Label28" runat="server" Text="File Attachment :"></asp:Label></strong>&nbsp;<br>&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" Width="160px">Recommended file size is 300KB</asp:Label></td>
			                    <td class="tableinput" colspan="4"><input class="button" id="File1" style="WIDTH: 249px; HEIGHT: 18px; BACKGROUND-COLOR: #ffffff" type="file" size="20" name="uploadedFile3" runat="server" /><asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>
			                </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label29" runat="server" Text="File Attached :"></asp:Label></strong></td>
					            <td class="tableinput" ><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
								<td class="Tableinput"></td>
					            <td class="Tableinput"></td>
					            <td class="Tableinput"></td>
	                        </tr>
				        </table>
					    <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Item Specification</asp:label>
                            <asp:Image ID="Image1" runat="server" /></div>
				        <div id="ItemSpec" style="display:inline;">
				        <table class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
			            <tr><td colspan="5"></td></tr> 
				            <tr valign="top">				               
			                    <td class="tablecol" width="23%" align="left" >&nbsp;<strong><asp:Label ID="Label31" runat="server" Text="Brand :"></asp:Label></strong></td>
			                    <td class="Tableinput" width="28%" ><asp:textbox id="txtBrand" runat="server" CssClass="txtbox" MaxLength="60" Width="100%"></asp:textbox><input id="hidBrand" type="hidden" name="hidBrand" runat="server" /></td>
				               	<td class="Tableinput" width="1%"></td>
				                <td class="tablecol" width="18%" align="left" >&nbsp;<strong><asp:Label ID="Label32" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="Tableinput" width="31%"  ><asp:textbox id="txtModel"  Width="100%" runat="server" CssClass="txtbox" MaxLength="70" ></asp:textbox><input id="hidModel" type="hidden" name="hidModel" runat="server" /></td>
				            </tr>
				            <tr valign="top">
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtDrawingNumber"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox><input id="hidDrawingNumber" type="hidden" name="hidDrawingNumber" runat="server" /></td>
				    			<td class="Tableinput"></td>
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Version No. :"></asp:Label></strong></td>
                                <td class="Tableinput"><asp:textbox id="txtVersionNo"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox><input id="hidVersionNo" type="hidden" name="hidVersionNo" runat="server" /></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtGrossWeight"  Width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox><input id="hidGrossWeight" type="hidden" name="hidGrossWeight" runat="server" /></td>			                				                
				               	<td class="Tableinput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="Tableinput"><asp:textbox id="txtNetWeight" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox><input id="hidNetWeight" type="hidden" name="hidNetWeight" runat="server" /></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtLength" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox><input id="hidLength" type="hidden" name="hidLength" runat="server" /></td>
				               	<td class="Tableinput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="Tableinput"><asp:textbox id="txtWidth" runat="server" CssClass="txtbox" MaxLength="50" width="100%"></asp:textbox><input id="hidWidth" type="hidden" name="hidWidth" runat="server" /></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label42" runat="server" Text="Packing Specification :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtPacking" runat="server" CssClass="txtbox" MaxLength="256" width="100%" ></asp:textbox><input id="hidPacking" type="hidden" name="hidPacking" runat="server" /></td>
			              		<td class="Tableinput"></td>
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label38" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtVolume" runat="server" CssClass="txtbox" MaxLength="50" width="100%" ></asp:textbox><input id="hidVolume" type="hidden" name="hidVolume" runat="server" /></td>			                				                
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtColorInfo" runat="server" CssClass="txtbox" MaxLength="256" width="100%" ></asp:textbox><input id="hidColorInfo" type="hidden" name="hidColorInfo" runat="server" /></td>			                				                
				               	<td class="Tableinput"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label37" runat="server" Text="Height (meter) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtHeight" runat="server" CssClass="txtbox" MaxLength="50" width="100%" ></asp:textbox><input id="hidHeight" type="hidden" name="hidHeight" runat="server" /></td>			                				                
				            </tr>
				            <tr valign="top">				               
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>				                
				                <td class="Tableinput" ><asp:textbox id="txtHSCode" runat="server" CssClass="txtbox" MaxLength="256" width="100%"></asp:textbox><input id="hidHSCode" type="hidden" name="hidHSCode" runat="server" /></td>				
 				               	<td class="Tableinput" colspan="3"></td>                           
 				            </tr>
				            <tr>
				                <td class="tablecol" rowspan="2"  align="left" style="height: 19px; width: 21%;">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td class="tableinput"  rowspan="2" colspan="4"><asp:textbox id="txtRemarks"  width="500px" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox></td>
							    <td><input id="hidRemarks" type="hidden" name="hidRemarks" runat="server" /></td>
				            </tr>
				        </table>
				        </div>
				<table class="alltable" id="Table4" width="100%" cellSpacing="0" border="0">
				<tr class="emptycol">
					<td colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2"><asp:label id="Label22" runat="server" CssClass="lblInfo" Text="Submit button will be disabled if there is a pending record for Hub Admin to approve"></asp:label>
					</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td style="HEIGHT: 18px" align="left" colspan="2" runat="server" >
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Submit"></asp:button>
						<input class="button" id="cmdReset" onclick="resetPostBack();" type="button" value="Reset"
							name="cmdReset" runat="server">
						<asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
                        <input class="txtbox" id="hidresult" type="hidden" name="hidresult" runat="server" />							
					</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label><input id="hidPostBack" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidPostBack"
							runat="server"><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></td>
				</tr>
				<tr class="emptycol">
					<td colspan="2" style="height: 6px">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
