<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BIM.aspx.vb" Inherits="eProcure.BIMFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>CatalogueNewProduct</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=BIM")
        </script> 
        <%response.write(Session("WheelScript"))%>
        <% Response.Write(CSS)%>
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		<script type="text/javascript">
		<!--
            
            
            $(document).ready(function(){
            $("#txtPreferVendor").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
            $("#txtPreferVendor1st").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
            $("#txtPreferVendor2nd").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
            $("#txtPreferVendor3rd").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
            
            $("#txtPreferVendor").result(function(event, data, formatted) {
            if (data)
            $("#hidPreferVendor").val(data[1]);
            });
            $("#txtPreferVendor1st").result(function(event, data, formatted) {
            if (data)
            $("#hidPreferVendor1st").val(data[1]);
            });
            $("#txtPreferVendor2nd").result(function(event, data, formatted) {
            if (data)
            $("#hidPreferVendor2nd").val(data[1]);
            });
            $("#txtPreferVendor3rd").result(function(event, data, formatted) {
            if (data)
            $("#hidPreferVendor3rd").val(data[1]);
            });
            });
                        
		    function showHide(lnkdesc)
            {
                if (document.getElementById(lnkdesc).style.display == 'none')
                {
	                document.getElementById(lnkdesc).style.display = '';
	                document.getElementById("Image2").src = '<%response.write(collapse_up) %>';
                } 
                else 
                {
	                document.getElementById(lnkdesc).style.display = 'none';
	                document.getElementById("Image2").src = '<%response.write(collapse_down) %>';
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
			
			function show()
			{
//				if (document.getElementById(rd1).value == 'SP')
//                {
//	                document.getElementById(rd1).style.display = 'None';

//                } 
//                else 
//                {
	                document.getElementById(dr2).style.display = '';
//                }
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
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">		
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header" colspan="2" style="width: 838px"><asp:label id="lblTitle" runat="server" CssClass="header">Item Master</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2" style="width: 838px">&nbsp;</td>
				</tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the required field(s) and click the Add button to add new item or Save button to save the changes."
		                ></asp:label>

	                </td>
                </tr>
				<tr>
					<td class="tableheader" colspan="5" width="100%" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server">Item Information</asp:label>
					</td>
				</tr>
				</table>
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr valign="top">
					            <td class="tablecol" align="left" width="20%">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Item Code"></asp:Label></strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label9" runat="server" Text=":"></asp:Label></td>
					            <td class="TableInput" width="30%"><asp:textbox id="txtVendorItemCode"  width="100%" runat="server" CssClass="txtbox" MaxLength="100" ></asp:textbox><asp:requiredfieldvalidator id="vldVItemCode" runat="server" Display="None" ErrorMessage="Item Code is required." controlToValidate="txtVendorItemCode" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidVendorItemCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorItemCode" runat="server"/></td>
					            <td class="TableInput" width="1%"></td>
					            <td class="TableInput" width="25%" ><strong><asp:checkbox id="chkStatus" Text="Active" Runat="server" Checked="True"></asp:checkbox></strong></td>
					            <td class="tablecol" width="24%" ></td>
				 </tr>
				<tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Item Name"></asp:Label></strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label10" runat="server" Text=":"></asp:Label></td>
					            <td class="TableInput" ><asp:textbox id="txtItemName"  width="280px" runat="server" CssClass="txtbox" MaxLength="500" ></asp:textbox><asp:requiredfieldvalidator id="vldItemName" runat="server" Display="None" ErrorMessage="Item Name is required." controlToValidate="txtItemName" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidItemName" style="display:none; WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidItemName" runat="server"/></td>
					            <td class="TableInput"></td>
					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Reference No. :" ></asp:Label></strong></td>
					            <td class="TableInput" ><asp:textbox id="txtRefNo" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>
					        </tr>
					        <tr valign="top">
					        <td class="tablecol" align="left"  >&nbsp;<strong><asp:Label ID="Label24" runat="server" Text="Item Type:" ></asp:Label></strong></td>
					        <td class="tablecol" align="left" >
                                    <asp:radiobuttonlist ID="rd1"   RepeatDirection="Vertical" runat="server" AutoPostBack="true"> 
                                    <asp:ListItem Value="SP" Selected="True">Spot (Non-Inventoried item)</asp:ListItem>
										<asp:ListItem Value="ST">Stock (Direct material - Inventoried item)</asp:ListItem>
										<asp:ListItem Value="MI">MRO, M&E and IT (Inventoried item)</asp:ListItem>
									</asp:radiobuttonlist></td>
					        <td class="TableInput" style="width: 1px"></td>
					        <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label26" runat="server" Text="Need QC/Verification :" ></asp:Label></strong></td>
							<td class="TableInput" >                                                                       
                                    <div><asp:radiobuttonlist ID="rd2" RepeatDirection="Horizontal" runat="server" Enabled="False" > 
                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
										<asp:ListItem Value="Y">Yes</asp:ListItem>
									</asp:radiobuttonlist></div>&nbsp;	</td>		
					        </tr>
				<tr valign="top">
					            <td class="tablecol" align="left"  rowspan="2" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Description:" ></asp:Label></strong></td>
					            <td class="TableInput" rowspan="2" ><asp:textbox id="txtDesc"  width="100%" runat="server" CssClass="txtbox" MaxLength="4000" TextMode="MultiLine" Height="37px" ></asp:textbox></td>
					            <td class="TableInput"rowspan="2" ></td>
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label18" runat="server" Text="Account Code :"></asp:Label></strong>
					            </td>
			                    <td class="TableInput" style="width: 291px" ><asp:textbox id="txtAccCode" width="100%"  runat="server" CssClass="txtbox" MaxLength="30" ></asp:textbox></td>
				</tr>
				<tr valign="top">			
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Order Quantity (Min) :"></asp:Label></strong></td>
                                <td class="TableInput" style="width: 291px" ><asp:textbox id="txtMin" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" Display="None" ErrorMessage="Order Quantity (Min) is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMin" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
                </tr>
					        <tr valign="top">
					            <td class="tablecol" align="left" rowspan="1"  >&nbsp;<strong><asp:Label ID="Label13" runat="server" Text="Commodity Type"></asp:Label></strong><asp:label id="Label14" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label15" runat="server" Text=":"></asp:Label></td>
                                <td class="TableInput"  rowspan="1"  width="20%"><asp:textbox id="txtCommodityType" width="70%" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><asp:requiredfieldvalidator id="vldCommodityType" runat="server" Display="None" ErrorMessage="Commodity Type is required." controlToValidate="txtCommodityType" EnableClientScript="False" ></asp:requiredfieldvalidator>
                                <input class="txtbox" id="hidCommodityType" style="HEIGHT: 18px" type="hidden" size="2" name="hidCommodityType" runat="server">
                                <input class="button" id="cmdSelect" onclick="PopWindow('../../Common/Catalogue/CommodityType.aspx')" type="button" value="Search"></td>
                                <td class="TableInput"></td>
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label25" runat="server" Text="Order Quantity (Max) :"></asp:Label></strong></td>
                                <td class="TableInput" style="width: 291px" ><asp:textbox id="txtMax" runat="server" CssClass="numerictxtbox"></asp:textbox><asp:regularexpressionvalidator id="revMax" runat="server" Display="None" ErrorMessage="Order Quantity (Max) is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMax" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
		                        </tr>
					        <tr valign="top">
					        	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label23" runat="server" Text="UOM"></asp:Label></strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label16" runat="server" Text=":"></asp:Label></td>
					            <td class="TableInput" ><asp:dropdownlist id="cboUOM" width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldUOM" runat="server" Display="None" ErrorMessage="Unit of Measurement is required." ControlToValidate="cboUOM" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidUOM" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidUOM" runat="server"></td>
 								 <td class="TableInput"></td>
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="txtInv" runat="server" Text="Safety Level (Min Inventory) :"></asp:Label></strong></td>
                                <td class="TableInput" style="width: 291px" ><asp:textbox id="txtMinInv" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="revMinInv" runat="server" Display="None" ErrorMessage="Safety Level is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMinInv" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
                           </tr>
				        <tr valign="top">
				                <td class="tablecol" colspan="3"></td>
 					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label50" runat="server" Text="Max Inventory Quantity :"></asp:Label></strong></td>
                                <td class="TableInput" ><asp:textbox id="txtMaxInvQty" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="revMaxInvQty" runat="server" Display="None" ErrorMessage="Max inventory quantity is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMaxInvQty" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>                                
                          </tr>
				        <tr valign="top">
					            <td class="tablecol" noWrap>&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Picture Attachment (JPG & GIF):"></asp:Label></strong>&nbsp;<br/>&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
					            <td class="tableinput" colspan="4">
					            <input class="button" id="FileProImage" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff; width: 290px;" type="file" name="FileProImage" runat="server"/>&nbsp;<asp:button id="cmdUploadImage" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button>
					            <input class="txtbox" id="hidImageAttached" style="HEIGHT: 18px" type="hidden" size="2" name="hidImageAttached" runat="server"/>
					            <input class="txtbox" id="hidImageFile" style="HEIGHT: 18px" type="hidden" size="2" name="hidImageFile" runat="server"/></td>
				            </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label27" runat="server" Text="Picture Attached :"></asp:Label></strong></td>
				            <td class="tableinput" colspan="4"><asp:panel id="pnlImage" width="100%" runat="server"></asp:panel></td>
				            </tr>
				            <tr valign="top">
			                    <td class="tablecol" noWrap align="left">&nbsp;<strong><asp:Label ID="Label28" runat="server" Text="File Attachment :"></asp:Label></strong>&nbsp;<br>&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                    <td class="tableinput" colspan="4">
			                    <input class="button" id="FileDoc" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 290px;" type="file" size="20" name="uploadedFile3" runat="server"/>&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
			                </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label29" runat="server" Text="File Attached :"></asp:Label></strong></td>
					            <td class="tableinput" ><asp:panel id="pnlAttach" width="100%" runat="server"></asp:panel></td>
					            <td class="TableInput"></td>
					            <td class="TableInput"></td>
					            <td class="TableInput" style="width: 291px"></td>
				            </tr>
				        </table>

					    <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Item Specification</asp:label>
                            <asp:Image ID="Image2" runat="server" ImageUrl="#" /></div>
					    <div id="ItemSpec" style="display:inline"  >
				        <table class="alltable" id="Table5" border="0" width="100%" cellspacing="0" cellpadding="0" >
			           <tr><td colspan="5"></td></tr> 
			           <tr valign="top">				               
			                    <td class="tablecol" width="23%" align="left" >&nbsp;<strong><asp:Label ID="Label31" runat="server" Text="Brand :"></asp:Label></strong></td>
			                    <td class="TableInput" width="28%" ><asp:textbox id="txtBrand" width="100%" runat="server" CssClass="txtbox" MaxLength="256"></asp:textbox></td>			                				                
				               	<td class="TableInput" style="width: 8px"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="Label47" runat="server" Text="Manufacturer Name :"></asp:Label></strong></td>
				                <td class="TableInput" style="width: 188px"  ><asp:textbox id="txtManu" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>				
				       </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtDrawingNumber" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="TableInput" style="width: 8px"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="Label32" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="TableInput" style="width: 188px"  ><asp:textbox id="txtModel" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtGrossWeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="TableInput" style="width: 8px"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="TableInput" style="width: 188px"><asp:textbox id="txtNetWeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox>
</td>		
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtLength" width="100%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
				               	<td class="TableInput" style="width: 8px"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Version No. :"></asp:Label></strong></td>
				                <td class="TableInput" style="width: 188px"><asp:textbox id="txtVersionNo" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label42" runat="server" Text="Packing Specification:"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtPacking" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>			                				                
				               	<td class="TableInput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="TableInput" style="width: 188px"><asp:textbox id="txtWidth" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:textbox id="txtColorInfo" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>			                				                
				               	<td class="TableInput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label38" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
			                    <td class="TableInput" style="width: 188px" ><asp:textbox id="txtVolume" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                 
				            </tr>
				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>
				                <td class="TableInput" ><asp:textbox id="txtHSCode" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>
				                <td class="TableInput" style="width: 8px">
			                    </td><td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label37" runat="server" Text="Height (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" style="width: 188px" ><asp:textbox id="txtHeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				               	<%--<td class="TableInput" colspan="3" style="width: 12px"></td>--%>
 				               	</tr>
				            <tr>
				                <td class="tablecol"  rowspan="2" align="left" style="height: 19px; ">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td class="tableinput" rowspan="2" colspan="4"><asp:textbox id="txtRemarks" width="100%" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox></td>
				            </tr>
				        </table>
	                    </div>			              
					    <div style="width:100%; cursor:pointer;"  class="tableheader" onclick="showHide1('Vendor')"><asp:label id="Label17" runat="server">Vendor</asp:label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
				        <div id="Vendor" style="display:inline;">
				        <table class="alltable" id="Table4" width="100%"cellspacing="0" cellpadding="0" border="0">
			       <tr>
			           <td class="tablecol"></td>
                       <td class="tablecol" style="width: 3638px">
                            <strong>&nbsp;<asp:Label ID="Label41" runat="server" Text="Use type ahead feature to select vendor"></asp:Label></strong></td>
                       <td class="tablecol"></td>
                       <td class="tablecol"></td>
                       <td class="tablecol">
                            <strong>&nbsp;<asp:Label ID="lblGST" runat="server" Text="GST/VAT" Width="50px"></asp:Label></strong></td>
                       <td class="tablecol" style="width: 80px">
                            <strong>&nbsp;<asp:Label ID="Label49" runat="server" Text="Order Lead Time(Days)" Height="15px" Width="78px"></asp:Label></strong></td>
                       <td class="tablecol">
                            <strong>&nbsp;<asp:Label ID="Label51" runat="server" Text="Vendor Item Code"></asp:Label></strong></td>
                   </tr> 
                   
			       <tr>
					<td class="tablecol" width="20%">
                        <strong>&nbsp;<asp:Label ID="Label19" runat="server" Text="Preferred Vendor : "></asp:Label></strong></td>
                    <td class="TableInput" style="width: 3638px" colspan="3">
                        <asp:TextBox ID="txtPreferVendor" runat="server" Width="100%" CssClass="txtbox"></asp:TextBox><asp:TextBox id="hidPreferVendor" runat="server" style="display: none"></asp:TextBox></td>
                    <td class="TableInput">&nbsp;<asp:dropdownlist id="cboPreferTax" runat="server" CssClass="ddl" Width="50px" Enabled="true"></asp:dropdownlist></td>
                    <td class="tablecol" style="width: 80px">
                        <strong>&nbsp;<asp:TextBox ID="txtLeadP" runat="server" CssClass="numerictxtbox" Width="97%"></asp:TextBox></strong></td>                        
                    <td class="TableInput"  width="10%">&nbsp;<asp:TextBox ID="txtVenCodeP" runat="server" CssClass="txtbox" Width="94%"></asp:TextBox></td>
               </tr>
                 <tr>
					<td class="tablecol">
                        <strong>&nbsp;<asp:Label ID="Label20" runat="server" Text="1st Alternative Vendor : "></asp:Label></strong></td>
                    <td class="TableInput" style="width: 3638px" colspan="3">
                    <asp:TextBox ID="txtPreferVendor1st" runat="server" Width="100%" CssClass="txtbox"></asp:TextBox><asp:TextBox id="hidPreferVendor1st" runat="server" style="display: none"></asp:TextBox></td>
                    <td class="TableInput">&nbsp;<asp:dropdownlist id="cbo1stTax" runat="server" CssClass="ddl" Width="50px" Enabled="true"></asp:dropdownlist></td>
                    <td class="tablecol" style="width: 80px">
                        <strong>&nbsp;<asp:TextBox ID="txtLead1" runat="server" CssClass="numerictxtbox" Width="97%"></asp:TextBox></strong></td>
                    <td class="TableInput" width="15%">&nbsp;<asp:TextBox ID="txtVenCode1" runat="server" CssClass="txtbox" Width="94%"></asp:TextBox></td>
               </tr>
                <tr>
					<td class="tablecol">
                        <strong>&nbsp;<asp:Label ID="Label21" runat="server" Text="2nd Alternative Vendor : " ></asp:Label></strong></td>
                    <td class="TableInput" style="width: 3638px" colspan="3">
                    <asp:TextBox ID="txtPreferVendor2nd" runat="server" Width="100%" CssClass="txtbox"></asp:TextBox><asp:TextBox id="hidPreferVendor2nd" runat="server" style="display: none"></asp:TextBox></td>
                    <td class="TableInput">&nbsp;<asp:dropdownlist id="cbo2ndTax" runat="server" CssClass="ddl" Width="50px" Enabled="true"></asp:dropdownlist></td>
                    <td class="tablecol" style="width: 80px" >
                        <strong>&nbsp;<asp:TextBox ID="txtLead2" runat="server" CssClass="numerictxtbox" Width="97%"></asp:TextBox></strong></td>
                    <td class="TableInput" width="15%" >&nbsp;<asp:TextBox ID="txtVenCode2" runat="server" CssClass="txtbox" Width="94%"></asp:TextBox></td>
               </tr>
                <tr>
					<td class="tablecol">
                        <strong>&nbsp;<asp:Label ID="Label22" runat="server" Text="3rd Alternative Vendor : "></asp:Label></strong></td>
                    <td class="TableInput" style="width: 3638px" colspan="3">
                    <asp:TextBox ID="txtPreferVendor3rd" runat="server" Width="100%" CssClass="txtbox"></asp:TextBox><asp:TextBox id="hidPreferVendor3rd" runat="server" style="display: none"></asp:TextBox></td>
                    <td class="TableInput">&nbsp;<asp:dropdownlist id="cbo3rdTax" runat="server" CssClass="ddl" Width="50px" Enabled="true"></asp:dropdownlist></td>
                    <td class="tablecol" style="width: 80px">
                        <strong>&nbsp;<asp:TextBox ID="txtLead3" runat="server" CssClass="numerictxtbox" Width="97%"></asp:TextBox></strong></td>
                    <td class="TableInput" >&nbsp;<asp:TextBox ID="txtVenCode3" runat="server" CssClass="txtbox" Width="94%"></asp:TextBox></td>
               </tr>
				        </table>
				        </div>
				       
				        <table class="alltable" id="Table3" width="100%"cellspacing="0" cellpadding="0" border="0">
				
				<tr class="emptycol">
					<td colspan="2" style="width: 838px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2" style="width: 838px">&nbsp;</td>
				</tr>
				<tr>
					<td style="HEIGHT: 18px; width: 838px;" align="left" colspan="2">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
                        &nbsp;
                        <input class="button" id="cmdAdd" type="button" value="Add" name="cmdAdd" runat="server"/>&nbsp;
<input class="button" id="cmdReset" onclick="resetPostBack();" type="button" value="Reset"
							name="cmdReset" runat="server"/>
					</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2" style="width: 838px">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2" style="width: 838px">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label><input id="hidPostBack" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidPostBack"
							runat="server"/><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/>
                        <asp:RegularExpressionValidator ID="revL3" runat="server" ControlToValidate="txtLead3"
                            Display="None" EnableClientScript="False" ErrorMessage="3rd Alternative Vendor lead time is expecting numeric value."
                            ValidationExpression="^\d+$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revL2" runat="server" ControlToValidate="txtLead2"
                            Display="None" EnableClientScript="False" ErrorMessage="2nd Alternative Vendor lead time is expecting numeric value."
                            ValidationExpression="^\d+$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revL1" runat="server" ControlToValidate="txtLead1"
                            Display="None" EnableClientScript="False" ErrorMessage="1st Alternative Vendor lead time is expecting numeric value."
                            ValidationExpression="^\d+$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revLP" runat="server" ControlToValidate="txtLeadP"
                            Display="None" EnableClientScript="False" ErrorMessage="Preferred Vendor lead time is expecting numeric value."
                            ValidationExpression="^\d+$" Width="60px"></asp:RegularExpressionValidator></td>
				</tr>
				<tr class="emptycol">
					<td colspan="2" style="height: 6px; width: 838px;">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2" style="width: 838px"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
