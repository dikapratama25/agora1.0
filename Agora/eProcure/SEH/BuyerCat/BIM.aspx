<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BIM.aspx.vb" Inherits="eProcure.BIMSEH" %>
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
        </script> 
        <%response.write(Session("WheelScript"))%>
        <% Response.Write(CSS)%>
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.Write(Session("vendortypeahead")) %>
       
		<script type="text/javascript">
		<!--
            
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
			
			function ShowDialog(filename,height)
		    {
//			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			    window.close;
				return true;

			    }
		    }	
		    
		    function updateparam(oldstr,keystr,newstr)
            {            
                var tempstr = keystr.replace("=","%3d")  
                var newstrr = oldstr.replace(tempstr,newstr)   
                return newstrr    
//                return oldstr.replace(keystr,newstr)
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
			
			
			function isNumberKey(evt)
            {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

             return true;
            }
            
            function checkDup()
            {
                var d = document.getElementById("hidBtn").click();
                d.click();
            }
            
            function onClick() 
            { 
                var bt = document.getElementById("hidBtn2"); 
                bt.click(); 
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
	<body>
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
					            <td class="Tableinput" style="width: 218px" width="25%"><asp:textbox id="txtVendorItemCode"  width="100%" runat="server" CssClass="txtbox" MaxLength="100" ></asp:textbox><asp:requiredfieldvalidator id="vldVItemCode" runat="server" Display="None" ErrorMessage="Item Code is required." controlToValidate="txtVendorItemCode" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidVendorItemCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorItemCode" runat="server"/></td>
					            <td class="Tableinput" align="left" width="6%"></td>
					            <td class="Tableinput" width="25%" ><strong><asp:checkbox id="chkStatus" Text="Active" Runat="server" Checked="True"></asp:checkbox></strong></td>
					            <td class="tablecol" width="24%" ><strong><asp:checkbox id="chkPartialDel" Text="Partial Delivery CD" Runat="server" Checked="True"></asp:checkbox></strong></td>
				 </tr>
				<tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Item Name"></asp:Label></strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label10" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput" style="width: 218px" ><asp:textbox id="txtItemName"  width="218px" runat="server" CssClass="txtbox" MaxLength="500" ></asp:textbox><asp:requiredfieldvalidator id="vldItemName" runat="server" Display="None" ErrorMessage="Item Name is required." controlToValidate="txtItemName" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidItemName" style="display:none; WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidItemName" runat="server"/></td>
					            <td class="Tableinput"></td>
					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Reference No." ></asp:Label></strong> :</td>
					            <td class="Tableinput" ><asp:textbox id="txtRefNo" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>
					        </tr>
					        <tr valign="top">
					        <td class="tablecol" align="left"  >&nbsp;<strong><asp:Label ID="Label24" runat="server" Text="Item Type" ></asp:Label></strong>:</td>
					        <td class="tablecol" align="left" style="width: 220px" >
                                    <asp:radiobuttonlist ID="rd1"   RepeatDirection="Vertical" runat="server" AutoPostBack="true"> 
                                    <asp:ListItem Value="SP" Selected="True" >Spot (Non-Inventoried item)</asp:ListItem>
										<asp:ListItem Value="ST">Stock (Direct material - Inventoried item)</asp:ListItem>
										<asp:ListItem Value="MI">MRO, M&E and IT (Inventoried item)</asp:ListItem>
									</asp:radiobuttonlist></td>
					        <td class="Tableinput" style="width: 1px"></td>
					        <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label26" runat="server" Text="Need QC/Verification" ></asp:Label></strong> :</td>
							<td class="Tableinput" >                                                                       
                                    <div><asp:radiobuttonlist ID="rd2" RepeatDirection="Horizontal" runat="server" Enabled="False" AutoPostBack="true"> 
                                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
										<asp:ListItem Value="Y">Yes</asp:ListItem>
									</asp:radiobuttonlist></div>&nbsp;	</td>		
					        </tr>
				<tr valign="top">
					            <td class="tablecol" align="left"  rowspan="2" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Description" ></asp:Label></strong> :</td>
					            <td class="Tableinput" rowspan="2" style="width: 218px" ><asp:textbox id="txtDesc"  width="100%" runat="server" CssClass="txtbox" MaxLength="4000" TextMode="MultiLine" Height="37px" ></asp:textbox></td>
					            <td class="Tableinput"rowspan="2" ></td>
					           <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label23" runat="server" Text="UOM"></asp:Label></strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label16" runat="server" Text=":"></asp:Label></td>
					            <td class="Tableinput" ><asp:dropdownlist id="cboUOM" width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="vldUOM" runat="server" Display="None" ErrorMessage="Unit of Measurement is required." ControlToValidate="cboUOM" EnableClientScript="False"></asp:requiredfieldvalidator>
					            <input class="txtbox" id="hidUOM" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidUOM" runat="server"/></td>
				</tr>
				<tr valign="top">			
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Order Quantity (Min)"></asp:Label></strong> :</td>
                                <td class="Tableinput" style="width: 291px" ><asp:textbox id="txtMin" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" Display="None" ErrorMessage="Order Quantity (Min) is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMin" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
                </tr>
					   <tr valign="top">
					            <td class="tablecol" align="left" rowspan="1"  >&nbsp;<strong><asp:Label ID="Label13" runat="server" Text="Commodity Type"></asp:Label></strong><asp:label id="Label14" runat="server" CssClass="errormsg">*</asp:label><asp:Label ID="Label15" runat="server" Text=":"></asp:Label></td>
                                <td class="Tableinput"  rowspan="1" style="width: 218px"><asp:textbox id="txtCommodityType" width="70%" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox>
                                <input class="button" id="cmdSelect" onclick="PopWindow('../Catalogue/CommodityType.aspx')" type="button" value="Search" style="width: 57px"/>
                                    <asp:requiredfieldvalidator id="vldCommodityType" runat="server" Display="None" ErrorMessage="Commodity Type is required." controlToValidate="txtCommodityType" EnableClientScript="False" ></asp:requiredfieldvalidator>
                                <input class="txtbox" id="hidCommodityType" style="HEIGHT: 18px" type="hidden" size="2" name="hidCommodityType" runat="server"/>
                                </td>
                                <td class="Tableinput"></td>
                                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label25" runat="server" Text="Order Quantity (Max)"></asp:Label></strong> :</td>
                                <td class="Tableinput" style="width: 291px" ><asp:textbox id="txtMax" runat="server" CssClass="numerictxtbox"></asp:textbox><asp:regularexpressionvalidator id="revMax" runat="server" Display="None" ErrorMessage="Order Quantity (Max) is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMax" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
		                </tr>
					    <tr valign="top">
					         <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label18" runat="server" Text="Account Code"></asp:Label></strong><asp:label id="Label19" runat="server" CssClass="errormsg">*</asp:label>:
					            </td>
			                    <td class="Tableinput" style="width: 218px" ><asp:dropdownlist id="ddlAccCode" width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist></td><%--<asp:textbox id="txtAccCode" width="100%"  runat="server" CssClass="txtbox" MaxLength="30" ></asp:textbox>--%>
					        	
 								 <td class="Tableinput"></td>
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="txtInv" runat="server" Text="Safety Level (Min Inventory) :"></asp:Label></strong></td>
                                <td class="Tableinput" style="width: 291px" ><asp:textbox id="txtMinInv" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="revMinInv" runat="server" Display="None" ErrorMessage="Safety Level is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMinInv" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>
                        </tr>
				        <tr valign="top">
				        <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label46" runat="server" Text="Category Code"></asp:Label></strong> :</td>
					            <td class="Tableinput" style="width: 218px" ><asp:dropdownlist id="ddlCatCode" width="100%" runat="server" CssClass="ddl"></asp:dropdownlist>
					            </td>
 								 <td class="Tableinput"></td>
				                
 					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label50" runat="server" Text="Max Inventory Quantity"></asp:Label></strong> :</td>
                                <td class="Tableinput" ><asp:textbox id="txtMaxInvQty" runat="server" CssClass="numerictxtbox" ></asp:textbox><asp:regularexpressionvalidator id="revMaxInvQty" runat="server" Display="None" ErrorMessage="Max inventory quantity is expecting numeric value. Range should be from 0 to 999999.99" controlToValidate="txtMaxInvQty" EnableClientScript="False" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"></asp:regularexpressionvalidator></td>                                
                        </tr>
                        <tr valign="top">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label69" runat="server"  Text="Reorder Quantity Level"></asp:Label></strong> :</td>
					            <td class="Tableinput" style="width: 218px"><asp:TextBox ID="txtRQL" runat="server" width="100%" CssClass="numerictxtbox"></asp:TextBox></td>
 						        <td class="Tableinput"></td>
 					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label70" runat="server" Text="EOQ"></asp:Label></strong> :</td>
                                <td class="Tableinput" ><asp:textbox id="txtEOQ" runat="server" CssClass="txtbox"></asp:textbox></td>                                
                        </tr>
                        <tr valign="top">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label71" runat="server"  Text="Budget Price"></asp:Label></strong> :</td>
					            <td class="Tableinput" style="width: 218px"><asp:TextBox ID="txtBudgetPrice" runat="server" width="100%" CssClass="numerictxtbox"></asp:TextBox></td>
 						        <td class="Tableinput"></td>
 					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label72" runat="server" Text="Ratio :"></asp:Label></strong></td>
                                <td class="Tableinput" ><asp:textbox id="txtRatio" runat="server" CssClass="txtbox"></asp:textbox></td>                                
                        </tr>
                        <tr valign="top">
				                <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label73" runat="server"  Text="IQC Test Type"></asp:Label></strong><asp:label id="Label20" runat="server" CssClass="errormsg">*</asp:label>:</td>
					            <td class="Tableinput" style="width: 218px">
					                <asp:dropdownlist id="ddlIQCType" width="100%" runat="server" CssClass="ddl" enabled="False">
					                </asp:dropdownlist>
					            </td>
 						        <td class="Tableinput"></td>
 					            <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label74" runat="server" Text="Oversea :"></asp:Label></strong></td>
                                <td class="Tableinput" >
                                    <div>
                                        <asp:radiobuttonlist ID="rd3" RepeatDirection="Horizontal" runat="server"> 
                                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
									    <asp:ListItem Value="Y">Yes</asp:ListItem>
									    </asp:radiobuttonlist>
								    </div>
					            </td>                                
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
			                    <td class="tablecol" noWrap align="left">&nbsp;<strong><asp:Label ID="Label28" runat="server" Text="File Attachment :"></asp:Label></strong>&nbsp;<br/>&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                    <td class="tableinput" colspan="4">
			                    <input class="button" id="FileDoc" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 290px;" type="file" size="20" name="uploadedFile3" runat="server"/>&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
			                </tr>
				            <tr valign="top">
					            <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label29" runat="server" Text="File Attached :"></asp:Label></strong></td>
					            <td class="tableinput" style="width: 218px" ><asp:panel id="pnlAttach" width="100%" runat="server"></asp:panel></td>
					            <td class="Tableinput"></td>
					            <td class="Tableinput"></td>
					            <td class="Tableinput" style="width: 291px"></td>
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
			                    <td class="Tableinput" width="28%" ><asp:textbox id="txtBrand" width="100%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></td>			                				                
				               	<td class="Tableinput" style="width: 8px"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="Label47" runat="server" Text="Model :"></asp:Label></strong></td>
				                <td class="Tableinput" style="width: 188px"  ><asp:textbox id="txtModel" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>				
				       </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label33" runat="server" Text="Drawing Number :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtDrawingNumber" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="Tableinput" style="width: 8px"></td>
				               	<td class="tablecol" width="25%" align="left" >&nbsp;<strong><asp:Label ID="Label32" runat="server" Text="Net Weight (kg) :"></asp:Label></strong></td>
				                <td class="Tableinput" style="width: 188px"  ><asp:textbox id="txtNetWeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>				
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label44" runat="server" Text="Gross Weight (kg) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtGrossWeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="Tableinput" style="width: 8px"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label45" runat="server" Text="Version No. :"></asp:Label></strong></td>
				                <td class="Tableinput" style="width: 188px"><asp:textbox id="txtVersionNo" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox>
</td>		
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label35" runat="server" Text="Length (meter) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtLength" width="100%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
				               	<td class="Tableinput" style="width: 8px"></td>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label34" runat="server" Text="Width (meter) :"></asp:Label></strong></td>
				                <td class="Tableinput" style="width: 188px"><asp:textbox id="txtWidth" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left">&nbsp;<strong><asp:Label ID="Label42" runat="server" Text="Color Info :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtColorInfo" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label36" runat="server" Text="Volume (liter) :"></asp:Label></strong></td>
				                <td class="Tableinput" style="width: 188px"><asp:textbox id="txtVolume" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				            </tr>
				            <tr valign="top">				               
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label39" runat="server" Text="Height (meter) :"></asp:Label></strong></td>
			                    <td class="Tableinput" ><asp:textbox id="txtHeight" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                
				               	<td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label38" runat="server" Text="Specification 1 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtSpecification1" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				                 
				            </tr>
				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label40" runat="server" Text="HS Code :"></asp:Label></strong></td>
				                <td class="Tableinput" ><asp:textbox id="txtHSCode" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				                <td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label37" runat="server" Text="Specification 2 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtSpecification2" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				            </tr>
 				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label52" runat="server" Text="Packing Type :"></asp:Label></strong></td>
				                <td class="Tableinput" ><asp:dropdownlist id="ddlPackType" width="100%" runat="server" CssClass="ddl"></asp:dropdownlist></td>
				                <td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label53" runat="server" Text="Specification 3 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtSpecification3" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				            </tr>
 				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label54" runat="server" Text="Packing Quantity :"></asp:Label></strong></td>
				                <td class="Tableinput" ><asp:textbox id="txtPackQty" width="100%" runat="server" CssClass="numerictxtbox" MaxLength="50" ></asp:textbox></td>
				                <td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label55" runat="server" Text="Manufacturer 1 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtManu" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				            </tr>
 				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label56" runat="server" Text="Section Code :"></asp:Label></strong></td>
				                <td class="Tableinput" ><asp:textbox id="txtSecCode" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				                <td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label57" runat="server" Text="Manufacturer 2 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtManu2" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				            </tr>
 				            <tr>
				                <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label58" runat="server" Text="Location Code :"></asp:Label></strong></td>
				                <td class="Tableinput" ><asp:textbox id="txtLocCode" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>
				                <td class="Tableinput" style="width: 8px"></td>
			                    <td class="tablecol" align="left" >&nbsp;<strong><asp:Label ID="Label59" runat="server" Text="Manufacturer 3 :"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px" ><asp:textbox id="txtManu3" width="100%" runat="server" CssClass="txtbox" MaxLength="50" ></asp:textbox></td>			                				               
 				            </tr>
 				            <tr>
				                <td class="tablecol" align="left" style="height: 24px" >&nbsp;<strong><asp:Label ID="Label60" runat="server" Text="Equivalent Item :"></asp:Label></strong></td>
				                <td class="Tableinput" style="height: 24px" ><asp:textbox id="txtNewItemCode" width="100%" runat="server" CssClass="txtbox" MaxLength="256" ></asp:textbox></td>
				                <td class="Tableinput" style="width: 8px; height: 24px;"></td>
			                    <td class="tablecol" align="left" style="height: 24px" >&nbsp;<strong><asp:Label ID="Label61" runat="server" Text="Packing Spec :" Visible="false"></asp:Label></strong></td>
			                    <td class="Tableinput" style="width: 188px; height: 24px;" ><asp:textbox id="txtPacking" width="100%" runat="server" CssClass="txtbox" MaxLength="50" Visible="false"></asp:textbox></td>			                				               
 				            </tr>
				            <tr>
				                <td class="tablecol"  rowspan="2" align="left" style="height: 19px; ">&nbsp;<strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong></td>
					            <td class="tableinput" rowspan="2" colspan="4"><asp:textbox id="txtRemarks" width="100%" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox></td>
				            </tr>
				        </table>
	                    </div>			              
					    <div style="width:130%; cursor:pointer;"  class="tableheader" onclick="showHide1('Vendor')"><asp:label id="Label17" runat="server">Vendor</asp:label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
				        <div id="Vendor" style="display:inline; width:130%">
				            <% Response.Write(Session("ConstructTableBIM")) %>
				        </div>
				       
				        <table class="alltable" id="Table3" width="100%"cellspacing="0" cellpadding="0" border="0">
				        <tr class="emptycol" id="trVendor" runat="server">
				            <td style="HEIGHT: 36px; width: 838px;" align="left">
                                <asp:Button ID="cmdVendor" runat="server" CssClass="button" Text="Add Line" /></td>
				        </tr>
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
							name="cmdReset" runat="server"/><input class="button" id="hidBtn" type="button" value="hidBtn" name="hidBtn" runat="server" style=" display :none"/>
							<input class="button" id="hidBtn2" type="button" value="hidBtn2" name="hidBtn2" runat="server" style=" display :none"/>
					</td>
				</tr>
				<tr class="emptycol">
					<td colspan="2" style="width: 838px">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2" style="width: 838px">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label><input id="hidPostBack" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidPostBack"
							runat="server"><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/>
						<asp:RegularExpressionValidator ID="revRQL" runat="server" ControlToValidate="txtRQL"
                            Display="None" EnableClientScript="False" ErrorMessage="Reorder Quantity Level is expecting numeric value. Range should be from 0 to 999999.99"
                            ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revBP" runat="server" ControlToValidate="txtBudgetPrice"
                            Display="None" EnableClientScript="False" ErrorMessage="Budget Price is expecting numeric value. Range should be from 0 to 999999.99"
                            ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revPackQty" runat="server" ControlToValidate="txtPackQty"
                            Display="None" EnableClientScript="False" ErrorMessage="Packing Quantity is expecting numeric value. Range should be from 0 to 999999.99"
                            ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Width="60px"></asp:RegularExpressionValidator>
                    <%--    <asp:RegularExpressionValidator ID="revL3" runat="server" ControlToValidate="txtLead3"
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
                            ValidationExpression="^\d+$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="rev_UP_P" runat="server" ControlToValidate="txtUnitPriceP"
                            Display="None" EnableClientScript="False" ErrorMessage="Preferred Vendor unit price is expecting numeric value." 
                            ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="rev_UP_1" runat="server" ControlToValidate="txtUnitPrice1"
                            Display="None" EnableClientScript="False" ErrorMessage="1st Alternative Vendor unit price is expecting numeric value." 
                            ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="rev_UP_2" runat="server" ControlToValidate="txtUnitPrice2"
                            Display="None" EnableClientScript="False" ErrorMessage="2nd Alternative Vendor unit price is expecting numeric value." 
                            ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Width="60px"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="rev_UP_3" runat="server" ControlToValidate="txtUnitPrice3"
                            Display="None" EnableClientScript="False" ErrorMessage="3rd Alternative Vendor unit price is expecting numeric value." 
                            ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Width="60px"></asp:RegularExpressionValidator></td>--%>
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
