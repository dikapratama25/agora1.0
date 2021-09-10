<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ProductDetail.aspx.vb" Inherits="eProcure.ProductDetailSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Item Details</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim bubblepopupcss As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "jquery.bubblepopup.v2.3.1.css") & """ rel='stylesheet' type='text/css'>"
            Dim bubblepopupjquery As String = "<script type=""text/javascript"" src="""& dDispatcher.direct("Plugins/include","jquery.bubblepopup.v2.3.1.min.js") &""">"            
            dim SelectVendor as string = dDispatcher.direct("Search","BuyerCatalogueSearchPopup.aspx","selVendor='+val+'&selSingleVendor='+val2+'")
        </script> 
        
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(bubblepopupcss)%>
		<% Response.Write(Session("JQuery")) %>
		<% Response.Write(bubblepopupjquery) %>
		<% Response.Write("</script>") %>
		
		<script type="text/JavaScript">
			
			function cmdAddClick(selVendor, selSingleVendor)
			{
			    var val = selVendor;
                var val2 = selSingleVendor;
			    window.open('<% Response.Write(SelectVendor) %>','Wheel','help:No,Height=580,Width=750,resizable=yes,scrollbars=yes'); 
			    window.close();
			    return false;
			}
			
			function PreviewImage(f)
			{		var temp;
					temp=f;
					//alert(eval(f));
					
					msg=window.open("","","Width=800,Height=600,resizable=yes,scrollbars=yes");
		 			msg.document.clear();
					/* Note that the word SCRIPT was not
						kept intact on one line in the
						write() below.  A bug will parse 
						it and will not compile the script
						correctly if you don't break that
						word when it appears within write()
						statements. */
					msg.document.write('<HTML><HEAD><TITLE'
					+'>Image Preview</TITLE>'
					+ '</HEAD><BODY><Center>'
					+'<img src="'
					+ f + '"></img>'
					+ '<P><a href="javascript:self.close()">Close Window</a>'
					+ '</Center></BODY></H'
					+'TML><P>');
			}
			
			$(document).ready(function(){
            <%  Response.Write(Session("jqPopup")) %>
            
            });
        
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header">
						<p><strong>Item Details</strong></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">
						<table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Item Information</td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%" >&nbsp;<strong>Item Code </strong>:&nbsp;</td>
								<td class="TableInput" width="50%" ><asp:label id="lbl_venitemcode" runat="server"></asp:label></td>
								<td class="TableInput" id="tdImage" width="25%" rowspan="14" runat="server">&nbsp;<asp:label id="lblProdImage" width= "100%" runat="server"></asp:label></td>		
							</tr>
							<tr valign="top" >
								<td class="tablecol" >&nbsp;<strong>Item Name </strong>:&nbsp;</td>
								<td class="TableInput" ><asp:label id="lbl_prodesc" runat="server" ></asp:label></td>
		                	</tr>
							<tr valign="top" >
								<td class="tablecol" style="height: 19px" >&nbsp;<strong>Item Description </strong>:&nbsp;</td>
								<td class="TableInput" style="height: 19px" ><asp:label id="lbl_desc" runat="server" width="100%" ></asp:label></td>
							</tr>
							<tr valign="top" id="trItemType" runat="server">
								<td class="tablecol" >&nbsp;<strong>Item Type </strong>:&nbsp;</td>
								<td class="TableInput" ><asp:label id="lbl_itemtype" runat="server" width="98%" ></asp:label></td>	    
							</tr>
							<tr valign="top">
								<td class="tablecol" >&nbsp;<strong>Partial Delivery CD </strong>:&nbsp;</td>
								<td class="TableInput"><asp:label id="lbl_pardelcd" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="trQC" runat="server">
							    <td class="tablecol" >&nbsp;<strong>Need QC/Verification </strong>:&nbsp;</td>
							    <td class="TableInput"><asp:label id="lbl_needqc" runat="server" width="9%" ></asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" >&nbsp;<strong>Commodity Type </strong>:&nbsp;</td>
								<td class="TableInput"><asp:label id="lbl_unspsc" runat="server"></asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" >&nbsp;<strong>UOM </strong>:&nbsp;</td>
								<td class="TableInput"><asp:label id="lbl_UOM" runat="server"></asp:label></td>
						    </tr>
						    <tr valign="top">
								<td class="tablecol" >&nbsp;<strong>Account Code </strong>:&nbsp;</td>
								<td class="TableInput"><asp:label id="lblAccCode" runat="server"></asp:label></td>
						    </tr>
						    <tr valign="top">
								<td class="tablecol" >&nbsp;<strong>Category Code </strong>:&nbsp;</td>
								<td class="TableInput"><asp:label id="lblCatCode" runat="server"></asp:label></td>						
						    </tr>
							<tr valign="top">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld1txt" runat="server" Text=""></asp:Label></strong> :</td>
								<td class="TableInput" ><asp:label id="lbl_fld1" runat="server"></asp:label></td>
						    </tr>
							<tr valign="top">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld2txt" runat="server" Text=""></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_fld2" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="trfld3" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld3txt" runat="server" Text=""></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_fld3" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="trfld4" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld4txt" runat="server" Text=""></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_fld4" runat="server"></asp:label></td>
							</tr>
						</table>
						<div id="divItemInfo2" style="display:none" runat="server" >
						<table id="Table8" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr valign="top" id="tr_RQL" runat="server">
								<td class="tablecol" width="25%" >&nbsp;<strong><asp:Label ID="lbl_RQL" runat="server" Text="Reorder Quantity Level"></asp:Label></strong> :</td>
								<td class="TableInput" width="50%"><asp:label id="lbl_RQL_d" runat="server"></asp:label></td>
								<td class="TableInput" width="25%" rowspan="13" runat="server"></td>		
							</tr>
							<tr valign="top" id="tr_BudgetPrice" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_BudgetPrice" runat="server" Text="Budget Price"></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_BudgetPrice_d" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="tr_IQC" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_IQCType" runat="server" Text="IQC Test Type"></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_IQCType_d" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="tr_EOQ" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_EOQ" runat="server" Text="EOQ"></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_EOQ_d" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="tr5" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_Ratio" runat="server" Text="Ratio"></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_Ratio_d" runat="server"></asp:label></td>
							</tr>
							<tr valign="top" id="tr6" runat="server">
								<td class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_Oversea" runat="server" Text="Oversea"></asp:Label></strong> :</td>
								<td class="TableInput"><asp:label id="lbl_Oversea_d" runat="server"></asp:label></td>
							</tr>
						</table>
						</div>	
					</td>
				</tr>
			</table>
					    <div id="divItemSpec" style="display:none" runat="server" >
				        <table class="alltable" id="Table7" border="0" width="100%" cellspacing="0" cellpadding="0" >
							<tr>
								<td class="tableheader" colspan="5">&nbsp;Item Specification</td>
							</tr>
			                <tr valign="top">				               
			                    <td class="tablecol" width="23%" align="left" >&nbsp;<strong><asp:Label ID="Label31" runat="server" Text="Brand :"></asp:Label></strong></td>
			                    <td class="TableInput" width="28%"><asp:label id="txtBrand" width="100%" runat="server" ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput" width="1%"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="lblManufacturerName" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="TableInput" width="24%"><asp:Label ID="txtModel" runat="server"  ReadOnly="True" ></asp:Label></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="TableInput"><asp:label id="txtDrawingNumber" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Version No. :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtVersionNo" width="100%" runat="server"  ReadOnly="True"  ></asp:label></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="TableInput"><asp:label id="txtGrossWeight" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtNetWeight" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput"><asp:label id="txtLength" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtWidth" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>	
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="lbl_fld5txt" runat="server"></asp:Label></strong></td>
			                    <td class="TableInput"><asp:label id="lbl_fld5" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtVolume" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>		                				                
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="TableInput"><asp:label id="txtColorInfo" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="lbl_fld6txt" runat="server" ></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="lbl_fld6" width="100%" runat="server" ReadOnly="True" ></asp:label></td>			                				                
				            </tr>
				            <tr>
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtHSCode" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="lbl_Spec2" runat="server" Text="Specification 2 :" visible="false"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtSpec2" width="100%" runat="server"  ReadOnly="True" visible="false"></asp:label></td>	                       
 				            </tr>
 				            <tr id="tr_ItemSpec1" style="display:none" runat="server">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Packing Type :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtPackType" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label4" runat="server" Text="Specification 3 :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtSpec3" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>	                       
 				            </tr>
 				            <tr id="tr_ItemSpec2" style="display:none" runat="server">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Packing Quantity :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtPackQty" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label8" runat="server" Text="Manufacturer 1 :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtManufacturerName" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>	                       
 				            </tr>
 				            <tr id="tr_ItemSpec3" style="display:none" runat="server">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label10" runat="server" Text="Section Code :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtSectionCode" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Manufacturer 2 :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtManufacturerName2" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>	                       
 				            </tr>
 				            <tr id="tr_ItemSpec4" style="display:none" runat="server">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Location Code :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtLocationCode" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label16" runat="server" Text="Manufacturer 3 :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtManufacturerName3" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>	                       
 				            </tr>
 				            <tr id="tr_ItemSpec5" style="display:none" runat="server">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label18" runat="server" Text="Equivalent Item :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtNewItemCode" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
 				               	<td class="TableInput"></td>    
 				               	<td class="tablecol" align="left"></td>
			                    <td class="TableInput" ></td>	                       
 				            </tr>
				            <tr>
				                <td class="tablecol" rowspan="2" align="left" style="height: 19px;">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td class="tableinput" rowspan="2" colspan="4"><asp:label id="txtRemarks" width="100%" runat="server" CssClass="txtbox"  ReadOnly="True" Height="37px" Rows="2" TextMode="MultiLine" ></asp:label></td>
				            </tr>
				        </table>
	                    </div>	
	             <div id="divContractInfo" style="display:none" class="TableInput" runat="server">
	                <table class="AllTable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="tableheader" colspan="5">&nbsp;Contract Information</td>
						</tr>
						<tr>
						    <td>
						    <asp:datagrid id="dtgContractInfo" runat="server" AutoGenerateColumns="false"  BorderStyle="None">
							    <Columns>
								    <asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor"   >
									    <HeaderStyle Width="30%" Font-Bold="true" ></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDM_GROUP_CODE" HeaderText="Contract Ref. No." >
									    <HeaderStyle Width="18%" Font-Bold="true"></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDM_START_DATE" ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" >
									    <HeaderStyle Width="11%" Font-Bold="true" HorizontalAlign="Center" ></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDM_END_DATE" ItemStyle-HorizontalAlign="Center" HeaderText="End Date" >
									    <HeaderStyle Width="11%" Font-Bold="true" HorizontalAlign="Center" ></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDI_CURRENCY_CODE" ItemStyle-HorizontalAlign="Center" HeaderText="Currency" >
								    	<HeaderStyle Width="8%" Font-Bold="true" HorizontalAlign="Center"  ></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDI_UNIT_COST"  HeaderText="Contract Price"  HeaderStyle-HorizontalAlign="Center" >
									    <HeaderStyle Width="14%" Font-Bold="true"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="CDI_GST"  HeaderText="Tax(%)"  HeaderStyle-HorizontalAlign="Right" >
									    <HeaderStyle Width="11%" Font-Bold="true"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="GST_RATE"  HeaderText="GST Rate"  HeaderStyle-HorizontalAlign="Right">
									    <HeaderStyle Width="11%" Font-Bold="true"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    </asp:BoundColumn>
							    </Columns>
						    </asp:datagrid>
						    </td>
				        </tr>
				     </table>
	             </div>		              
				<div id="divVendor" style="DISPLAY: none" runat="server" width="1200px">
				    <% Response.Write(Session("ConstructTable")) %>
						<%--<table id="Table5" cellspacing="0" cellpadding="0" width="1200px" border="0">
							<tr>
							    <td class="tableheader" style="width: 400px" colspan="2">Vendor</td>
								<td class="tableheader" style="width: 100px">Supplier Code</td>
								<td class="tableheader" style="width: 100px">Delivery Term</td>
								<td class="tableheader" style="width: 50px">Unit Price</td>
								<td class="tableheader" style="width: 100px">Payment Term</td>
								<td class="tableheader" style="width: 50px">Currency</td>
								<td class="tableheader" style="width: 100px">Purchaser Spec No.</td>
								<td class="tableheader" style="width: 100px">Revision</td>
								<td class="tableheader" style="width: 50px">GST/VAT</td>					
								<td class="tableheader" style="width: 50px">Order Lead Time (Days)</td>
								<td class="tableheader" style="width: 100px">Vendor Item code</td>
								
							</tr>
			                <tr>
					            <td class="tablecol" width="170px" style="height: 19px"><strong>&nbsp;<asp:Label ID="Label19" runat="server" Text="Preferred Vendor : "></asp:Label></strong></td>
                                <td class="TableInput" style="height: 19px; width: 230px;" >&nbsp;<asp:label id="lbl_Prefer" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_SuppCode" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_DelTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right" style="height: 19px;">&nbsp;<span style="cursor:default;" class="jqUnitPrice1"><asp:label id="lbl_Prefer_UnitPrice" runat="server" ></asp:label></span></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_PayTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_Currency" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_PurSpecNo" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_Prefer_Revision" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right" style="height: 19px;">&nbsp;<asp:label id="lbl_PreferTax"  runat="server" ></asp:label></td>
                                <td class="TableInput" align="center"style="height: 19px;">&nbsp;<asp:label id="lbl_leadP" runat="server" ></asp:label></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_vendorP" runat="server" ></asp:label></td>
                            </tr>
                            <tr>
					            <td class="tablecol"><strong>&nbsp;<asp:Label ID="Label20" runat="server" Text="1st Alternative Vendor : "></asp:Label></strong></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_1st" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_SuppCode" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_DelTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right" style="height: 19px;">&nbsp;<span style="cursor:default;" class="jqUnitPrice2"><asp:label id="lbl_1st_UnitPrice" runat="server" ></asp:label></span></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_PayTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_Currency" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_PurSpecNo" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_1st_Revision" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right">&nbsp;<asp:label id="lbl_1stTax"   runat="server" ></asp:label></td>
                                <td class="TableInput" align="center">&nbsp;<asp:label id="lbl_lead1" runat="server" ></asp:label></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_vendor1" runat="server" ></asp:label></td>
                            </tr>
                            <tr>
					            <td class="tablecol"><strong>&nbsp;<asp:Label ID="Label21" runat="server" Text="2nd Alternative Vendor : " ></asp:Label></strong></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_2nd" runat="server" CssClass="ddl" Width="100%" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_SuppCode" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_DelTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right" style="height: 19px;">&nbsp;<span style="cursor:default;" class="jqUnitPrice3"><asp:label id="lbl_2nd_UnitPrice" runat="server" ></asp:label></span></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_PayTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_Currency" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_PurSpecNo" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_2nd_Revision" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right">&nbsp;<asp:label id="lbl_2ndTax" runat="server"></asp:label></td>
                                <td class="TableInput" align="center">&nbsp;<asp:label id="lbl_lead2" runat="server"></asp:label></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_vendor2" runat="server"></asp:label></td>
                            </tr>
                             <tr>
					            <td class="tablecol" >
                                    <strong>&nbsp;<asp:Label ID="Label22" runat="server" Text="3rd Alternative Vendor : "></asp:Label></strong></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_3rd" runat="server" CssClass="ddl" Width="100%"></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_SuppCode" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_DelTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right" style="height: 19px;">&nbsp;<span style="cursor:default;" class="jqUnitPrice4"><asp:label id="lbl_3rd_UnitPrice" runat="server" ></asp:label></span></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_PayTerm" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_Currency" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_PurSpecNo" runat="server" ></asp:label></td>
                                <td class="TableInput" style="height: 19px;">&nbsp;<asp:label id="lbl_3rd_Revision" runat="server" ></asp:label></td>
                                <td class="TableInput" align="right">&nbsp;<asp:label id="lbl_3rdTax" runat="server" ></asp:label></td>
                                <td class="TableInput" align="center">&nbsp;<asp:label id="lbl_lead3"  runat="server" ></asp:label></td>
                                <td class="TableInput">&nbsp;<asp:label id="lbl_vendor3" runat="server" ></asp:label></td>
                            </tr>
						</table>--%>
				</div>
			<div id="divCatalogue" style="DISPLAY: none" runat="server">
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td valign="middle" align="left"><asp:datagrid id="MyDataGrid" runat="server">
							<Columns>
								<asp:BoundColumn DataField="TYPE" HeaderText="Catalogue Type">
									<HeaderStyle Width="23%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRICE" HeaderText="Unit Price">
									<HeaderStyle Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UOM" HeaderText="UOM">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GRP_CODE" HeaderText="Disc Grp Code/Contract Ref. No.">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TRANSDATE" HeaderText="Transdate">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="REMARKS" HeaderText="Remarks">
									<HeaderStyle Width="28%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Currency"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr style="HEIGHT: 5px">
				</tr>
						</table>
				</div>
			<div id="divHist" style="DISPLAY: none" class="TableInput"  runat="server" >
			<table class="AllTable" id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="tableheader" colspan="5">&nbsp;Historical Transaction</td>
							</tr>
				<tr>
					<td class="alltable" ><asp:datagrid id="dtgTrx" runat="server" AutoGenerateColumns="false"  BorderStyle="None">
							<Columns>
								<asp:BoundColumn DataField="POM_S_COY_NAME" HeaderText="Vendor"   >
									<HeaderStyle Width="40%" Font-Bold="true" ></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST"  HeaderText="Unit Price"  HeaderStyle-HorizontalAlign="Right" >
									<HeaderStyle Width="8%" Font-Bold="true"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_PO_DATE" HeaderText="PO Date" >
									<HeaderStyle Width="8%" Font-Bold="true"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" ItemStyle-HorizontalAlign="Right" HeaderText="Ordered Qty" >
									<HeaderStyle Width="8%" Font-Bold="true" HorizontalAlign="Right"  ></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr style="HEIGHT: 5px">
				</tr>
						</table>
				</div>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="tableheader">&nbsp;File Attachments</td>
							</tr>
							<tr>
								<td class="tablecol" id="tdAttach" runat="server">&nbsp;</td>
							</tr>
							<tr>
							    <td style="height: 19px">
                                <%--<A id="cmd_back" href="#" runat="server"><strong>&lt; Back</strong></A>--%>
                                <%--<asp:button id="cmd_back" runat="server" CssClass="Button" Text="Close"></asp:button>--%>
                                <input type="button" style=" font-size:11; height:18px;width:50px;" id="cmd_back" runat="server" CssClass="Button" value='Close'>
                                </td>
							</tr>
						</table>
		</form>
	</body>
</HTML>
