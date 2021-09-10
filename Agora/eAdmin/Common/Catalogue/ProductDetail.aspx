<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ProductDetail.aspx.vb" Inherits="eAdmin.ProductDetail"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Item Details</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim SelectVendor as string = dDispatcher.direct("Search","BuyerCatalogueSearchPopup.aspx","selVendor='+val+'&selSingleVendor='+val2+'")
        </script> 
		<% Response.Write(Session("WheelScript"))%>
		<SCRIPT language="JavaScript">
			
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
		</SCRIPT>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">
						<P><STRONG>Item Details</STRONG></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="4">&nbsp;Item Information</TD>
							</TR>
							<TR vAlign="top" >
								<TD class="tablecol" width="25%" >&nbsp;<STRONG>Item Code </STRONG>
									:</TD>
								<TD class="TableInput" width="50%" ><asp:label id="lbl_venitemcode" runat="server"></asp:label></TD>
								<TD class="TableInput" id="tdImage" width="25%" rowSpan="11" runat="server">&nbsp;
									<asp:label id="lblProdImage" width= "100%" runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top" >
								<TD class="tablecol" >&nbsp;<STRONG>Item</STRONG> <STRONG>
										Name </STRONG>:&nbsp;
								</TD>
								<TD class="TableInput" ><asp:label id="lbl_prodesc" runat="server" ></asp:label></TD>
			</TR>
							<TR vAlign="top" >
								<TD class="tablecol" style="height: 19px" >&nbsp;<STRONG>Item</STRONG> <STRONG>
										Description </STRONG>:&nbsp;
								</TD>
								<TD class="TableInput" style="height: 19px" ><asp:label id="lbl_desc" runat="server" width="100%" ></asp:label></TD>
							</TR>
							<TR vAlign="top" id="trItemType" runat="server">
								<TD class="tablecol" >&nbsp;<STRONG>Item</STRONG> <STRONG>
										Type </STRONG>:&nbsp;
								</TD>
								<TD class="TableInput" ><asp:label id="lbl_itemtype" runat="server" width="98%" ></asp:label></TD>
								    
							</TR>
							<TR vAlign="top" id="trQC" runat="server">
							<TD class="tablecol" >&nbsp;<STRONG>Need </STRONG> <STRONG>
										QC/Verification </STRONG>:&nbsp;
								</TD>
							        <TD class="TableInput" >
                                    <asp:label id="lbl_needqc" runat="server" width="9%" ></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<STRONG>Commodity Type </STRONG>
									:</TD>
								<TD class="TableInput"><asp:label id="lbl_unspsc" runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<STRONG>UOM </STRONG>
									:</TD>
								<TD class="TableInput"><asp:label id="lbl_UOM" runat="server"></asp:label></TD>
						</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld1txt" runat="server" Text=""></asp:Label></strong> :</TD>
								<TD class="TableInput" ><asp:label id="lbl_fld1" runat="server"></asp:label></TD>
						</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld2txt" runat="server" Text=""></asp:Label></strong> :</TD>
								<TD class="TableInput"><asp:label id="lbl_fld2" runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld3txt" runat="server" Text=""></asp:Label></strong> :</TD>
								<TD class="TableInput"><asp:label id="lbl_fld3" runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top" id="trfld4" runat="server">
								<TD class="tablecol" >&nbsp;<strong><asp:Label ID="lbl_fld4txt" runat="server" Text=""></asp:Label></strong> :</TD>
								<TD class="TableInput"><asp:label id="lbl_fld4" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
					    <div id="divItemSpec" style="display:none" runat="server" >
				        <table class="alltable" id="Table7" border="0" width="100%" cellSpacing="0" cellPadding="0" >
							<TR>
								<TD class="tableheader" colSpan="5">&nbsp;Item Specification</TD>
							</TR>
			                <tr valign="top">				               
			                    <td class="tablecol" width="23%" align="left" >&nbsp;<strong><asp:Label ID="Label31" runat="server" Text="Brand :"></asp:Label></strong></td>
			                    <td class="TableInput" width="28%" ><asp:label id="txtBrand" width="100%" runat="server" ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput" width="1%"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="Label32" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="TableInput" width="24%"  ><asp:label id="txtModel" width="100%" runat="server"  ReadOnly="True"  ></asp:label>
</td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtDrawingNumber" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Version No. :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtVersionNo" width="100%" runat="server"  ReadOnly="True" ></asp:label>
				                </td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtGrossWeight" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtNetWeight" width="100%" runat="server"  ReadOnly="True" ></asp:label>
</td>		
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtLength" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
				               	<td class="TableInput"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="TableInput"><asp:label id="txtWidth" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label42" runat="server" Text="Packing Specification:"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtPacking" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label38" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtVolume" width="100%" runat="server" ReadOnly="True" ></asp:label></td>			                				                
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtColorInfo" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				               	<td class="TableInput"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label37" runat="server" Text="Height (meter) :"></asp:Label></strong></td>
			                    <td class="TableInput" ><asp:label id="txtHeight" width="100%" runat="server"  ReadOnly="True" ></asp:label></td>			                				                
				            </tr>
				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>
				                <td class="TableInput" ><asp:label id="txtHSCode" width="100%" runat="server"  ReadOnly="True" ></asp:label>
 				               	<td class="TableInput" colspan="3"></td>                           </tr>
				            <tr>
				                <td class="tablecol"  rowspan="2" align="left" style="height: 19px; ">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td class="tableinput" rowspan="2" colspan="4"><asp:label id="txtRemarks" width="100%" runat="server" CssClass="txtbox"  ReadOnly="True" Height="37px" Rows="2" TextMode="MultiLine" ></asp:label></td>
				            </tr>
				        </table>
	                    </div>			              
				<DIV id="divVendor" style="DISPLAY: none" runat="server">
						<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="4">&nbsp;Vendor</TD>
								<TD class="tableheader" style="width: 21px">&nbsp;GST/VAT</TD>
								<TD class="tableheader" style="width: 101px">&nbsp;Order Lead Time (Days)</TD>
								<TD class="tableheader" style="width: 188px">Vendor Item code</TD>
							</TR>
			       <tr>
					<TD class="tablecol" width="25%" style="height: 19px" >
                        <strong>&nbsp;<asp:Label ID="Label19" runat="server" Text="Preferred Vendor : "></asp:Label></strong></TD>
                    <TD class="TableInput" style="height: 19px; width: 285px;" >&nbsp;<asp:label id="lbl_Prefer" runat="server" ></asp:label></TD>
 					<td class="tablecol" width="5%" style="height: 19px"></td>
 					<TD class="tablecol" style="height: 19px">
                        <strong>&nbsp;<%--<asp:Label ID="Label24" runat="server" Text="GST/VAT : "></asp:Label>--%></strong></TD>
                    <TD class="TableInput"  align="right" style="height: 19px; width: 21px;" >&nbsp;<asp:label id="lbl_PreferTax"  runat="server" ></asp:label></TD>
                    <TD class="TableInput"  align="center"style="height: 19px; width: 101px;" >&nbsp;<asp:label id="lbl_leadP" runat="server" ></asp:label></TD>
                    <TD class="TableInput" style="height: 19px; width: 188px;" >&nbsp;<asp:label id="lbl_vendorP" runat="server" ></asp:label></TD>
               </tr>
                 <tr>
					<TD class="tablecol" >
                        <strong>&nbsp;<asp:Label ID="Label20" runat="server" Text="1st Alternative Vendor : "></asp:Label></strong></TD>
                    <TD class="TableInput" style="width: 285px" >&nbsp;<asp:label id="lbl_1st" runat="server" ></asp:label></TD>
  					<td class="tablecol" ></td>
 					<TD class="tablecol">
                        <strong>&nbsp;<%--<asp:Label ID="Label26" runat="server" Text="GST/VAT : "></asp:Label>--%></strong></TD>
                    <TD class="TableInput"  align="right" style="width: 21px" >&nbsp;<asp:label id="lbl_1stTax"   runat="server" ></asp:label></TD>
                    <TD class="TableInput"  align="center" style="width: 101px" >&nbsp;<asp:label id="lbl_lead1" runat="server" ></asp:label></TD>
                    <TD class="TableInput" style="width: 188px" >&nbsp;<asp:label id="lbl_vendor1" runat="server" ></asp:label></TD>
               </tr>
                <tr>
					<TD class="tablecol">
                        <strong>&nbsp;<asp:Label ID="Label21" runat="server" Text="2nd Alternative Vendor : " ></asp:Label></strong></TD>
                    <TD class="TableInput" style="width: 285px">&nbsp;<asp:label id="lbl_2nd" runat="server" CssClass="ddl" Width="100%" ></asp:label></TD>
  					<td class="tablecol" ></td>
 					<TD class="tablecol" >
                        <strong>&nbsp;<%--<asp:Label ID="Label41" runat="server" Text="GST/VAT : "></asp:Label>--%></strong></TD>
                    <TD class="TableInput"  align="right" style="width: 21px"  >&nbsp;<asp:label id="lbl_2ndTax" runat="server"></asp:label></TD>
                    <TD class="TableInput"  align="center" style="width: 101px"  >&nbsp;<asp:label id="lbl_lead2" runat="server"></asp:label></TD>
                    <TD class="TableInput" style="width: 188px"  >&nbsp;<asp:label id="lbl_vendor2" runat="server"></asp:label></TD>
               </tr>
                <tr>
					<TD class="tablecol" >
                        <strong>&nbsp;<asp:Label ID="Label22" runat="server" Text="3rd Alternative Vendor : "></asp:Label></strong></TD>
                    <TD class="TableInput" style="width: 285px" >&nbsp;<asp:label id="lbl_3rd" runat="server" CssClass="ddl" Width="100%"></asp:label></TD>
  					<td class="tablecol" ></td>
 					<TD class="tablecol">
                        <strong>&nbsp;<%--<asp:Label ID="Label46" runat="server" Text="GST/VAT : "></asp:Label>--%></strong></TD>
                    <TD class="TableInput" align="right" style="width: 21px"  >&nbsp;<asp:label id="lbl_3rdTax" runat="server" ></asp:label></TD>
                    <TD class="TableInput" align="center" style="width: 101px"  >&nbsp;<asp:label id="lbl_lead3"  runat="server" ></asp:label></TD>
                    <TD class="TableInput" style="width: 188px"  >&nbsp;<asp:label id="lbl_vendor3" runat="server" ></asp:label></TD>
               </tr>
						</TABLE>
				</div>
			<DIV id="divCatalogue" style="DISPLAY: none" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD vAlign="middle" align="left"><asp:datagrid id="MyDataGrid" runat="server">
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
						</asp:datagrid></TD>
				</TR>
				<TR style="HEIGHT: 5px">
				</TR>
						</TABLE>
				</div>
			<DIV id="divHist" style="DISPLAY: none" class="TableInput"  runat="server" >
			<TABLE class="AllTable" id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="5">&nbsp;Historical Transaction</TD>
							</TR>
				<TR>
					<TD class="alltable" ><asp:datagrid id="dtgTrx" runat="server" AutoGenerateColumns="false"  BorderStyle="None">
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
						</asp:datagrid></TD>
				</TR>
				<TR style="HEIGHT: 5px">
				</TR>
						</TABLE>
				</div>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader">&nbsp;File Attachments</TD>
							</TR>
							<TR>
								<TD class="tablecol" id="tdAttach" runat="server">&nbsp;</TD>
							</TR>
							<tr>
							    <td style="height: 19px">
                                <%--<A id="cmd_back" href="#" runat="server"><STRONG>&lt; Back</STRONG></A>--%>
                                <%--<asp:button id="cmd_back" runat="server" CssClass="Button" Text="Close"></asp:button>--%>
                                <input type=button style=" font-size:11; height:18px;width:50px;" id="cmd_back" runat="server" CssClass="Button" value='Close'>
                                </td>
							</tr>
						</TABLE>
		</form>
	</body>
</HTML>
