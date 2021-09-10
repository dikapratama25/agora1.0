<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RaisePR.aspx.vb" Inherits="eProcure.RaisePR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Raise PR</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %> 
		<% Response.Write(Session("AutoComplete")) %>        
		<% Response.Write(Session("WheelScript"))%>
		<% Response.write(Session("typeahead")) %>
		
		<script language="javascript">
		<!--
		
		$(document).ready(function(){
        $('#cmdSubmit').click(function() {
        summary = Page_ValidationSummaries[0];                        
            if(summary.innerHTML == "")
            {                   
                if(document.getElementById("cmdSubmit"))
                { document.getElementById("cmdSubmit").style.display= "none"; }
                if(document.getElementById("cmdRaise"))
                { document.getElementById("cmdRaise").style.display= "none"; }
                if(document.getElementById("cmdAdd"))
                { document.getElementById("cmdAdd").style.display= "none"; }
                if(document.getElementById("cmdRemove"))
                { document.getElementById("cmdRemove").style.display= "none"; }
                if(document.getElementById("cmdDelete"))
                { document.getElementById("cmdDelete").style.display= "none"; }
                if(document.getElementById("cmdDupPRLine"))
                { document.getElementById("cmdDupPRLine").style.display= "none"; }
            }      
        });
        $('#cmdRaise').click(function() {
        summary = Page_ValidationSummaries[0];                        
            if(summary.innerHTML == "")
            {
                if(document.getElementById("cmdSubmit"))
                { document.getElementById("cmdSubmit").style.display= "none"; }
                if(document.getElementById("cmdRaise"))
                { document.getElementById("cmdRaise").style.display= "none"; }
                if(document.getElementById("cmdAdd"))
                { document.getElementById("cmdAdd").style.display= "none"; }
                if(document.getElementById("cmdRemove"))
                { document.getElementById("cmdRemove").style.display= "none"; }
                if(document.getElementById("cmdDelete"))
                { document.getElementById("cmdDelete").style.display= "none"; }
                if(document.getElementById("cmdDupPRLine"))
                { document.getElementById("cmdDupPRLine").style.display= "none"; }
            }      
        });
        });
        
			var strErrMsg;
			
			function validateQty()
			{
				var j, k, ctlName, Sno;
				strErrMsg = '';
				for(i = 0; i < Page_Validators.length; i++) {
					// validate Qty range
					j = Page_Validators[i].id.indexOf('revRange');
					k = Page_Validators[i].id.indexOf('revQty');
					
					if ((j>1)||(k>1)){ 
						if (j>1)
							ctlName = Page_Validators[i].id.substring(0,j) + 'hidItemLine';
						else
							ctlName = Page_Validators[i].id.substring(0,k) + 'hidItemLine';
													
						Sno = document.getElementById(ctlName).value; 
							
						if (Page_Validators[i].isvalid == false){
							if (j >1)
								strErrMsg = strErrMsg + '<li>' + Sno + '. Quantity outside tolerance range</li>';
							else	
								strErrMsg = strErrMsg + '<li>' + Sno + '. Invalid quantity</li>';
						}
					}					
				}
				
				if (strErrMsg == '')
					return true;
				else
					return false;	
			}
						
			function removeQuot(str)
			{
				var r = new RegExp(",","gi");
				var newstr = str.replace(r, '') ;
				return newstr;
			}
			
			function formatDec(amt, cnt)
			{
				var len = amt.length;
				switch (parseInt(cnt)) {
					case 4:
						switch (parseInt(len)) {
							case 1:
								return "000" + amt;
								
							case 2:
								return "00" + amt;
								
							case 3:
								return "0" + amt;
								
							case 4:
								return amt;						
						}
						break;					
				
					case 2:
						switch (parseInt(len)) {
							case 1:
								return "0" + amt;
								
							case 2:
								return amt;						
						}
						break;					
				}					
			}
			
			function formatDecimal(amt, cnt)
			{
				var len, posDot; 
				var num, numDec;
				posDot = amt.indexOf('.');
				
				if (posDot > 0) {
					num = amt.substring(0, posDot);
					numDec = amt.substring(posDot + 1, amt.length);
					len = numDec.length;
					
					switch (parseInt(cnt)) {
						case 4:
							if (len > 4)
								len = 5;
							switch (parseInt(len)) {
								case 1:
									return num + "." + numDec + "000";
									
								case 2:
									return num + "." + numDec + "00";
									
								case 3:
									return num + "." + numDec + "0";
									
								case 4:
									return num + "." + numDec;
									
								case 5:
									if (parseInt(numDec.substr(4,1))>4){
										numDec = parseInt(numDec.substr(0,4), 10) + 1;	
										if (numDec < 1000) 
											numDec = formatDec('' + numDec, cnt);
										else {
											if (numDec >= 10000){											
												var stringValue = String(numDec);
												return (parseInt(num) + 1) + "." + stringValue.substr(1,4);	
											}
										}		
									}
									else{
										numDec = numDec.substr(0,4);								
									}
									return num + "." + numDec;				
							}
							break;
							
						case 2:					
							if (len > 2)
								len = 3;
							switch (parseInt(len)) {
								case 1:
									return num + "." + numDec + "0";
									
								case 2:
									return num + "." + numDec;
									
								case 3:
									if (parseInt(numDec.substr(2,1))>4){
										numDec = parseInt(numDec.substr(0,2), 10) + 1;	
										if (numDec < 10) 
											numDec = formatDec('' + numDec, cnt);
										else{
											if (numDec >= 100){											
												var stringValue = String(numDec);
												num = num.replace(",","");
												return addCommas((parseInt(num) + 1) + "." + stringValue.substr(1,2),2);	
											}
										}
									}
									else{
										numDec = numDec.substr(0,2);								
									}
									return num + "." + numDec;				
							}
							break;
					}				
				}
				else {
					if (parseInt(cnt) == 4)
						return amt + '.' + '0000';
					else
						return amt + '.' + '00';
				}					
			}
			
			function round(number,X) {
				// rounds number to X decimal places, defaults to 2
				X = (!X ? 2 : X);
				val=Math.round(number*Math.pow(10,X))/Math.pow(10,X);
				return val;
			}
			
			function addCommas(argNum, cnt, argThouSeparator, argDecimalPoint)
			{
				//var argNum;
				//argNum = '' + round(argNum, 4);
				//alert(argNum);
				// default separator values (should resolve to local standard)
				var sThou = (argThouSeparator) ? argThouSeparator : ","
				var sDec = (argDecimalPoint) ? argDecimalPoint : "."
		 
				// split the number into integer & fraction
				var aParts = argNum.split(sDec)
		 
				// isolate the integer & add enforced decimal point
				var sInt = aParts[0] + sDec
		 	
				// tests for four consecutive digits followed by a thousands- or  decimal-separator
				var rTest = new RegExp("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))")
		 
				while (sInt.match(rTest))
				{
					// insert thousands-separator before the three digits
					sInt = sInt.replace(rTest, "$1" + sThou + "$2")
				}
	 
				// plug the modified integer back in, removing the temporary 	decimal point
				aParts[0] = sInt.replace(sDec, "");
				//alert(aParts.join(sDec));							
				return formatDecimal(aParts.join(sDec), cnt);				
				//return round(aParts.join(sDec),4);
				//return aParts.join(sDec);
			}
			
			function calculateTotal(type, qty, price, amt, tax, gst, taxAmt, indicator)
			{				
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax;
				Form1.hid6.value = gst;
				
				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST;
				var subTotal, taxTotal, taxVal;
				
				var Quantity = removeQuot(eval("Form1." + qty + ".value"));
				var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));
                var Taxation = removeQuot(eval("Form1." + tax + ".value"));
                var TaxPercent = removeQuot(eval("Form1." + gst + ".value"));

                if (UnitPrice=="") { UnitPrice=0; }
				ctlAmount = document.getElementById(amt);
				//2015-06-17: CH: Rounding issue (Prod issue)
				ctlAmount.value = (Quantity * UnitPrice).toFixed(2);	
														

				ctlTax = document.getElementById(tax);
				//2015-06-17: CH: Rounding issue (Prod issue)
				ctlTax.value = (ctlAmount.value  * TaxPercent / 100).toFixed(2);
							
				ctlAmount.value = addCommas(ctlAmount.value, 2); 
			    ctlTax.value = addCommas(ctlTax.value, 2);
			    calculateGrandTotal();			    
			}
			
			function calculateAllIndividualTotal()
			{
			    //Calculate subtotal, total tax, and Grand total
				var sAllClient, iPos, sCurrentClientId, iInd;
				var dtgShopping;
				sAllClient = Form1.hidClientId.value;
				for (iInd=0; iInd < Form1.hidTotalClientId.value; iInd++)
				{
					iPos = sAllClient.indexOf('|');	
					
				    sCurrentClientId = sAllClient.substring(0, iPos);		
				    calculateTotal(0,"dtgShopping_" + sCurrentClientId+"_txtQty","dtgShopping_" + sCurrentClientId+"_txtPrice","dtgShopping_" + sCurrentClientId+"_txtAmount","dtgShopping_" + sCurrentClientId+"_txtGSTAmt","dtgShopping_" + sCurrentClientId+"_hidtaxperc", "0","0");
					sAllClient = sAllClient.substring(iPos+1);
				}				
			}
		
		    function calculateGrandTotal()
			{
			    //Calculate subtotal, total tax, and Grand total
				var sAllClient, iPos, sCurrentClientId;
				var dSubtotal=0, dTotalTax=0, dGrandTotal=0;
				var dtgShopping;
				var temp;
				
				sAllClient = Form1.hidClientId.value;
				for (i=0; i < Form1.hidTotalClientId.value; i++)
				{			
					
					iPos = sAllClient.indexOf('|');	
			 
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    
				    dtgShopping = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtAmount.value");
				    dtgShopping = dtgShopping.replace(",","");
				    dtgShopping = dtgShopping.replace(",","");
				    dSubtotal = eval(dSubtotal + parseFloat(dtgShopping.replace(",","")));
				    dtgShopping = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtGSTAmt.value");
				    dtgShopping = dtgShopping.replace(",","");
				    dtgShopping = dtgShopping.replace(",","");
				    dTotalTax = eval(dTotalTax + parseFloat(dtgShopping.replace(",","")));
				    sAllClient = sAllClient.substring(iPos+1);
					
				}							
				document.getElementById('sSubTotal').innerHTML = addCommas(dSubtotal.toFixed(2), 2);
				document.getElementById('sTax').innerHTML = addCommas(dTotalTax.toFixed(2), 2);
					
				var sGrandtotal = eval(dSubtotal + dTotalTax);
				document.getElementById('sGrandTotal').innerHTML = addCommas(sGrandtotal.toFixed(2), 2);
				
			}
			
			function deleteAttach(a)
			{
			}
		
			function BrowseClick()
			{
				Form1.File1.click();
				Form1.txtAttached.value = Form1.File1.value;
			}
									
			function selectAll()
			{
				SelectAllG("dtgShopping_ctl01_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgShopping_ctl01_chkAll","chkSelection");
			}
			
			function focusControl(type, qty, price, amt, tax, gst, taxAmt) 
			{					
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax;
				Form1.hid6.value = gst;
				Form1.hid7.value = taxAmt;
			}
			
			function refreshDatagrid()
			{ 		
				if (Form1.hid2.value != ''){
										
					var type, qty, price, amt, tax, gst, taxAmt;
					type = Form1.hid1.value;
					qty = Form1.hid2.value;
					price = Form1.hid3.value;
					amt = Form1.hid4.value;
					tax = Form1.hid5.value;
					gst = Form1.hid6.value;
					taxAmt = Form1.hid7.value;
				}
			}
			
			function ShowDialog(filename,height)
		    {
                
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 780px");
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
		    
		    function PopWindow(myLoc)
		    {
			    window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			    return false;
		    }
		    
		    function ValidateInput()
			{ 
			    var sAllClient, iPos, sCurrentClientId;
				var iQty, iPrice, iEstDate, sDevAdd, i, j, strChar;
				var strValidChars = "0123456789.,";
				var intline = 0;
				strErrMsg = '';
				sAllClient = Form1.hidClientId.value;
				
				
				for (j=0; j < Form1.hidTotalClientId.value; j++)
				{
				    intline = intline + 1;
				    iPos = sAllClient.indexOf('|');	
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    
				    if (sCurrentClientId.length < 3)
				    {
				        return true;
				    }
				    
				    iQty = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtQty.value");
		            for (i = 0; i < iQty.length; i++)
                    {
                        strChar = iQty.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>' + intline + '. Invalid quantity. Range should be from 0.01 to 999999.99</li>'; return false; }
                    }
                    if (iQty <= 0) { strErrMsg = strErrMsg + '<li>' + intline + '. Invalid quantity. Range should be from 0.01 to 999999.99</li>'; return false };
                    
					sDevAdd = eval("Form1.dtgShopping_" + sCurrentClientId + "_hidDelCode.value");
				    if (sDevAdd.length == 0) {strErrMsg = strErrMsg + '<li>' + intline + '. Invalid delivery address.</li>'; return false;}
				    
					sAllClient = sAllClient.substring(iPos+1);
					
					
				}
				
				return true;
			}
			
		    function InitialValidation()
			{
				if (ValidateInput() == true){
				    summary = Page_ValidationSummaries[0];
					summary.innerHTML = "";
					summary.style.display = "none";
					return true;
				}
				else{
					summary = Page_ValidationSummaries[0];
					summary.innerHTML = '<ul>' + strErrMsg + '</ul>';	
					summary.style.display = "";
					return false;
				}
			}
		    
		    function RemoveItemCheck(pChkSelName){
			    var oform = document.forms[0];
			    var iTotalCheckbox=0, iTotalChecked=0;
		        for (var i=0;i<oform.elements.length;i++)
			    {
				    var e = oform.elements[i];
				    if (e.type=="checkbox")
				    {
				        if ((e.name.substr(3)=="CustomPR") || (e.name.substr(3)=="RemarkPR"))
				        {
				        }
				        else
				        {
				            if (e.checked==true)
				            {
				                iTotalChecked+=1;
				            }
				            iTotalCheckbox+=1;
				        }
				        
				        
				    }
			    }
			    //if 4, dont allow to delete, at least have 1 record.
			    // if (iTotalCheckbox == 4)
			    // YapCL: 2011Mar08 - cater multiple items
			    if (iTotalCheckbox == iTotalChecked)
				{
				    alert ('You must have at least 1 row of record!');
				    return false;
				}
				if (iTotalChecked == 0)
				{
				    alert ('Please make at least one selection!');
				    return false;
				}
				else
				{
				    // YapCL: 2011Mar08 - cater yes no			    
				    var checkyesno ;
				    checkyesno = CheckAtLeastOne('chkSelection','delete');
				    if (checkyesno == true)
				    {
				        return true;
				    }
				    else
				    {
				        return false;
				    }
				}
            }		

            //Jules 2018.08.06
            function setGiftDDLState(GLCode, gift) {               
                var ctlGLCode, ctlGift;
                ctlGLCode = document.getElementById(GLCode);
                ctlGift = document.getElementById(gift);              

                if (ctlGLCode.value.substring(0, 1) == '1') {
                    ctlGift.value = 'N';
                    ctlGift.disabled = true;
                }
                else ctlGift.disabled = false;               
            }
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" runat="server" id="body1">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_AddPR_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TBODY>
				
				    <tr>
					    <td class="linespacing1"></td>
				    </tr>					                    
					<TR>
						<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
					</TR>
					<TR>
	                    <TD class="EmptyCol" colSpan="6">
		                    <asp:label id="Label8" runat="server"  CssClass="lblInfo"
		                            Text="Fill in the required field(s) and click the Save button to create the PR or Submit button to submit the PR."
		                    ></asp:label>

	                    </TD>
                    </TR>
					<TR>
						<TD class="TableHeader" colSpan="2">&nbsp;Purchase Request Header</TD>
					</TR>
					<TR>
						<TD class="tablecol" vAlign="middle" align="left">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
								<TR vAlign="top">
									<TD class="tablecol" align="left" width="20%">&nbsp;<STRONG>PR Number</STRONG>&nbsp;:</TD>
									<TD class="tablecol" width="30%"><asp:label id="lblPRNo" runat="server" Width="202px"></asp:label></TD>
									<TD class="tablecol" width="20%">&nbsp;<STRONG>Date</STRONG>&nbsp;:</TD>
									<TD class="tablecol" width="30%"><asp:label id="lblDate" runat="server"></asp:label></TD>
									<TD class="tablecol" style="HEIGHT: 2px">&nbsp;<asp:checkbox id="chkUrgent" runat="server" Width="100px" Text="Urgent" Height="21px"></asp:checkbox></TD>
								</TR>
								<TR vAlign="top">
									<TD class="tablecol">
                                        <strong>&nbsp;Requester Name</strong>&nbsp;:</TD>
									<TD class="tablecol" width="25%"><asp:textbox id="txtRequestedName" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></TD>
									<TD class="tablecol">&nbsp;<strong>Attention To</strong> :&nbsp;</TD>
									<TD class="tablecol" width="25%" colSpan="2"><asp:textbox id="txtAttention" runat="server" CssClass="txtbox" Width="250px" MaxLength="50"></asp:textbox></TD>
								</TR>
								<TR vAlign="top">
									<TD class="tablecol">&nbsp;<strong>Requester Contact</strong>&nbsp;:</TD>
									<TD class="tablecol" width="25%"><asp:textbox id="txtRequestedContact" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></TD>
									<TD class="tablecol">&nbsp;</TD>
									<TD class="tablecol" width="25%" colSpan="2"></TD>
								</TR>
								<TR vAlign="top">
									<TD class="tablecol">
                                        <strong>&nbsp;Internal Remarks</strong>&nbsp;:</TD>
									<TD class="tablecol" width="25%"><asp:textbox id="txtInternal" runat="server" CssClass="listtxtbox" Width="250px" MaxLength="1000"
											Rows="2" TextMode="MultiLine"></asp:textbox></TD>
									<TD class="tablecol">&nbsp;<strong>External Remarks</strong>&nbsp;:</TD>
									<TD class="tablecol" width="25%" colSpan="2"><asp:textbox id="txtExternal" runat="server" CssClass="listtxtbox" Width="68%" MaxLength="1000"
											Rows="2" TextMode="MultiLine"></asp:textbox></TD>
								</TR>
								
								<TR vAlign="top">
									<TD class="tablecol" style="HEIGHT: 17px">&nbsp;<STRONG>Internal Attachment</STRONG>&nbsp;:</TD>
									<TD class="tablecol" style="HEIGHT: 22px" width="25%" colSpan="5" rowSpan="2"><INPUT class="button" id="File1Int" style="WIDTH: 400px; HEIGHT: 18px" type="file" size="17"
											name="uploadedFile3Int" runat="server">&nbsp;<asp:button id="cmdUploadInt" runat="server" CssClass="button" CausesValidation="False" Text="Upload"></asp:button><asp:label id="lblFileInt" Visible="False" Runat="server"></asp:label></TD>
								</TR>
								<TR vAlign="top">
									<TD class="tablecol" >&nbsp;<asp:label id="lblAttachInt" runat="server" CssClass="small_remarks" Width="176px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:label></TD>									
								</TR>
								<TR vAlign="top">
									<TD class="tablecol" style="HEIGHT: 19px">&nbsp;<STRONG>Internal File Attached </STRONG>:</TD>
									<TD class="tablecol" style="HEIGHT: 19px" width="25%" colSpan="5"><asp:panel id="pnlAttachInt" runat="server"></asp:panel></TD>
								</TR>
								
								<TR vAlign="top">
									<TD class="tablecol" style="HEIGHT: 17px">&nbsp;<STRONG>External Attachment</STRONG>&nbsp;:</TD>
									<TD class="tablecol" style="HEIGHT: 22px" width="25%" colSpan="5" rowSpan="2"><INPUT class="button" id="File1" style="WIDTH: 400px; HEIGHT: 18px" type="file" size="17"
											name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" CausesValidation="False" Text="Upload"></asp:button><asp:label id="lblFile" Visible="False" Runat="server"></asp:label></TD>
								</TR>
								<TR vAlign="top">
									<TD class="tablecol" >&nbsp;<asp:label id="lblAttach" runat="server" CssClass="small_remarks" Width="176px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:label></TD>									
								</TR>
								<TR vAlign="top">
									<TD class="tablecol" style="HEIGHT: 19px">&nbsp;<STRONG>External File Attached </STRONG>:</TD>
									<TD class="tablecol" style="HEIGHT: 19px" width="25%" colSpan="5"><asp:panel id="pnlAttach" runat="server"></asp:panel></TD>
								</TR>
								<TR vAlign="top" id = "Extra1" runat="server" >
									<TD class="tablecol" colSpan="6">&nbsp;If you do not want "Custom Fields" or 
										"Remark" to appear in PO, please uncheck the appropriate boxes.&nbsp;:</TD>
								</TR>
								<TR vAlign="top" id = "Extra2" runat="server" >
									<TD class="tablecol" style="HEIGHT: 2px" colSpan="6">&nbsp;<asp:checkbox id="chkCustomPR" runat="server" Width="136px" Text="Custom Fields" Height="21px"></asp:checkbox></TD>
								</TR>
								<TR vAlign="top" id = "Extra3" runat="server" >
									<TD class="tablecol" colSpan="6" style="height: 20px">&nbsp;<asp:checkbox id="chkRemarkPR" runat="server" Text="Remark"></asp:checkbox></TD>
								</TR>
								<%--<TR class="emptycol">
									<TD colSpan="4"><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field</TD>
								</TR>--%>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD class="emptycol" style="HEIGHT: 18px">&nbsp;&nbsp;&nbsp;</TD>
					</TR>
				</TBODY>
			</TABLE>
			<table class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<td class="emptycol"><asp:datagrid id="dtgShopping" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgShopping_Page"
							DataKeyField="PRODUCTCODE">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label ID="lblItemLine" Runat="server" style="display:none"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="ITEMINDEX" SortExpression="ITEMINDEX"  readonly="true"  HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>                                								
						        <%--<asp:BoundColumn DataField="VENDORITEMCODE" SortExpression="VENDORITEMCODE"  readonly="true"    HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="115px"></HeaderStyle>
								</asp:BoundColumn>--%>								

                                <%--Jules 2018.05.02 - PAMB Scrum 2 - --%>
                                <asp:BoundColumn Visible="False" DataField="GIFT" SortExpression="GIFT" readonly="true" HeaderText="GIFT">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
                                <asp:TemplateColumn SortExpression="GIFT" HeaderText="Gift">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboGift" Width="50px" CssClass="ddl" Runat="server">
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="No" Selected="True"></asp:ListItem>
										</asp:DropDownList>										
									</ItemTemplate>
								</asp:TemplateColumn>

                                <%--Jules 2018.10.17--%>
                                <%--<asp:TemplateColumn SortExpression="FUNDTYPE" HeaderText="Fund Type (L1)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboFundType" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="PERSONCODE" HeaderText="Person Code (L9)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboPersonCode" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="PROJECTCODE" HeaderText="Project / ACR (L8) Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboProjectCode" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
									</ItemTemplate>
								</asp:TemplateColumn>--%>
                                <asp:TemplateColumn SortExpression="FUNDTYPE" HeaderText="Fund Type (L1)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtFundType" Runat="server" ></asp:label>
										<asp:textbox id="hidFundType" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdFundType" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdFundType" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="PERSONCODE" HeaderText="Person Code (L9)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtPersonCode" Runat="server" ></asp:label>
										<asp:textbox id="hidPersonCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdPersonCode" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdPersonCode" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="PROJECTCODE" HeaderText="Project / ACR (L8) Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtProjectCode" Runat="server" ></asp:label>
										<asp:textbox id="hidProjectCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdProjectCode" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdProjectCode" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
                                <%--End modification--%>                                

                                <asp:BoundColumn Visible="False" DataField="PRODUCTCODE" SortExpression="PRODUCTCODE" HeaderText="Product Code">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
                                <asp:TemplateColumn SortExpression="VENDORITEMCODE" HeaderText="Item Code" >
								    <HeaderStyle Width="110px" HorizontalAlign="Left"></HeaderStyle>
								    <ItemTemplate>
									    <asp:HyperLink Runat="server" ID="VENDORITEMCODE"></asp:HyperLink>	
									    <asp:Label ID="lblVENDORITEMCODE" Runat="server" Visible="false" ></asp:Label>								    
								    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
							    </asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="GLCODE" SortExpression="GLCODE"  readonly="true"    HeaderText="GLCODE">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>

                                <%--Jules 2018.10.22 - U00019--%>
								<%--<asp:TemplateColumn SortExpression="GL CODE" HeaderText="(GL CODE) GL Description">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>

                                        <%--Jules 2018.05.04 - PAMB Scrum 2--%>
										<%--<asp:DropDownList id="cboGLCode" Width="100px" CssClass="ddl" Runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboGLCode_SelectedIndexChanged"></asp:DropDownList>--%>
                                        <%--<asp:DropDownList id="cboGLCode" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>--%>
                                        <%--End modification.--%>

										<%--<input type="button" runat="server" class="button" id="btnGLSearch" 
											value=">" />--%>
									<%--</ItemTemplate>
								</asp:TemplateColumn>--%>
                                <asp:TemplateColumn SortExpression="GL CODE" HeaderText="GL Description (GL CODE)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtGLCode" Runat="server" ></asp:label>
                                        <asp:textbox id="hidGLCodeStatus" Runat="server" style="display:none;"></asp:textbox>
										<asp:textbox id="hidGLCode" Runat="server" style="display:none;"></asp:textbox>                                                                                
										<input class="button" id="cmdGLCode" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdGLCode" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
                                <%--End modification--%>

								<asp:TemplateColumn SortExpression="CATEGORY CODE" HeaderText="Category Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboCategoryCode" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>
										<%--<input type="button" runat="server" class="button" id="btnCatSearch" 
											value=">" />--%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--<asp:BoundColumn DataField="TAXCODE" SortExpression="TAXCODE"  readonly="true"    HeaderText="Tax Code">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>--%>
								
								<asp:TemplateColumn SortExpression="ASSET GROUP" HeaderText="Asset Group">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboAssetGroup" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn SortExpression="PRODUCTDESC" HeaderText="Item Name" >
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<%--<asp:Label ID="lblProductDesc" style="width:90px" Runat="server"></asp:Label>--%>
										<asp:TextBox id="lblProductDesc" CssClass="listtxtbox" TextMode="MultiLine" Runat="server"></asp:TextBox>										
										<asp:Label ID="lblProductCode" Runat="server" Visible="false" ></asp:Label>
										<asp:Label ID="lblGLCode" Runat="server" Visible="false" ></asp:Label>
										<asp:Label ID="lblCategoryCode" Runat="server" Visible="false" ></asp:Label>
										<asp:Label ID="lblCDGroup" Runat="server" Visible="false" ></asp:Label>
										<asp:Label ID="lblVendor" Runat="server" Visible="false" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--<asp:BoundColumn DataField="RFQ_QTY" SortExpression="RFQ_QTY"  Visible="false" readonly="true"    HeaderText="RFQ Qty">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<%--<asp:BoundColumn DataField="TOLERANCE" SortExpression="TOLERANCE"  Visible="false" readonly="true"    HeaderText="Quotation Qty Tolerance">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<asp:TemplateColumn SortExpression="QUANTITY" HeaderText="Quantity">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="60px"  MaxLength="9" Runat="server" ></asp:TextBox>
										<asp:RegularExpressionValidator id="revQty" ControlToValidate="txtQty" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" Runat="server"></asp:RegularExpressionValidator>
										<%--<asp:CompareValidator ID="compValQty" ErrorMessage="?" ControlToValidate="txtQty"
                                            runat="server" Operator="NotEqual" Type="Double" ValueToCompare="0" />--%>
										<%--<asp:RangeValidator id="revRange" ControlToValidate="txtQty" Runat="server"></asp:RangeValidator>--%>
										<input class="txtbox" id="hidItemLine" type="hidden" runat="server" name="hidItemLine" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--<asp:BoundColumn DataField="CATEGORYCODE" SortExpression="CATEGORYCODE"  readonly="true"   
									HeaderText="Commodity Type">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>--%>
								<asp:TemplateColumn SortExpression="COMMODITY" HeaderText="Commodity Type" >
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<%--<asp:DropDownList id="ddl_comm" style="width:110px" CssClass="ddl" Runat="server"></asp:DropDownList>--%>										
										<asp:textbox id="txtCommodity" style="width:110px" runat="server" CssClass="txtbox"></asp:textbox>
										<input type="hidden" id="hidCommodity" runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>								
								<%--<asp:BoundColumn DataField="UOM" SortExpression="UOM"  readonly="true"    HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								</asp:BoundColumn>--%>	
								<asp:TemplateColumn SortExpression="UOM" HeaderText="UOM" >
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" style="width:56px" CssClass="ddl" Runat="server"></asp:DropDownList>										
										<asp:Label ID="lblUOM" Runat="server" Visible="false" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CURRENCY" HeaderText="Currency" >
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblCurrency" Runat="server" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>								
								<asp:TemplateColumn SortExpression="UNITCOST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="numerictxtbox" Width="55px" Runat="server" ReadOnly="True"></asp:TextBox>
										<%--<asp:RegularExpressionValidator id="revPrice" ControlToValidate="txtPrice" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Runat="server"></asp:RegularExpressionValidator>--%>
                                        <%--<asp:RangeValidator ID="revPriceRange" runat="server" ControlToValidate="txtPrice"></asp:RangeValidator>--%>                                        
                                        <asp:Label id="lblNoTax" CssClass="lbl" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Amount" HeaderText="Amount" >
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtAmount" Width="50px" CssClass="numerictxtbox" Runat="server"  readonly="true"   ></asp:TextBox><input id="hidGSTAmt" type="hidden" runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="GSTRate" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblGSTRate" Runat="server" Width="55px"></asp:Label>
										<asp:TextBox id="hidGSTRate" Runat="server" style="display:none;"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="GST" HeaderText="SST Amount">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtGST" CssClass="lblnumerictxtbox" Runat="server"  readonly="true" style="display:none"></asp:TextBox>
										<asp:TextBox id="txtGSTAmt" CssClass="numerictxtbox" Runat="server"  readonly="true"  Width="45px"></asp:TextBox>
										<asp:TextBox id="hidtaxperc" Runat="server" style="display:none;"></asp:TextBox>
										<asp:TextBox id="hidTaxID" Runat="server" style="display:none;"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH begin--%>	
								<asp:TemplateColumn SortExpression="GstTaxCode" HeaderText="SST Tax Code (Purchase) (L6)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:Label id="lblTaxCode" Runat="server" Width="55px"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH end--%>	
								<asp:BoundColumn Visible="False" DataField="SOURCE" SortExpression="SOURCE" HeaderText="Source">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CATEGORYCODE" SortExpression="CATEGORYCODE"  readonly="true"   
									HeaderText="CATEGORYCODE">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="VENDOR" SortExpression="VENDOR" HeaderText="Vendor">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn Visible="False" DataField="CATEGORYCODE" SortExpression="CATEGORYCODE"  readonly="true"   
									HeaderText="CATEGORYCODE">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>--%>
								<%--<asp:BoundColumn Visible="False" DataField="CDGROUP" SortExpression="CDGROUP" HeaderText="CDGROUP">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>--%>
								<%--<asp:TemplateColumn SortExpression="Budget Account" HeaderText="Budget Account" >
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboBudget" CssClass="ddl" Runat="server"></asp:DropDownList>
										<input class="button" id="cmdBudget" style="WIDTH: 15px; HEIGHT: 18px" type="button" value=">"
											name="cmdBudget" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:TemplateColumn SortExpression="Budget Account" HeaderText="Cost Centre Code (L7)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtBudget" Runat="server" ></asp:label>
										<asp:textbox id="hidBudgetCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdBudget" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdBudget" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Delivery Address">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtDelivery" Runat="server" ></asp:label>
										<asp:textbox id="hidDelCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdDelivery" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdDelivery" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Est" HeaderText="Est. Date of Delivery (dd/mm/yyyy)">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" CssClass="numerictxtbox" Width="60px" Runat="server"></asp:TextBox>
										<%--<asp:RegularExpressionValidator id="revETD" ValidationExpression="^\d+$" ControlToValidate="txtEstDate" Runat="server"></asp:RegularExpressionValidator>--%>
										<%--<asp:RequiredFieldValidator ID="reqETD" ControlToValidate="txtEstDate" Runat="server"></asp:RequiredFieldValidator>--%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Warranty" HeaderText="Warranty Terms (mths)">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtWarranty" style="WIDTH: 50px" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revWarranty" ValidationExpression="^\d+$" ControlToValidate="txtWarranty" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--<asp:BoundColumn DataField="MOQ" SortExpression="MOQ"  readonly="true"    HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								</asp:BoundColumn>--%>
								<%--<asp:BoundColumn DataField="MPQ" SortExpression="MPQ"  readonly="true"    HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								</asp:BoundColumn>--%>
								<asp:TemplateColumn SortExpression="REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" TextMode="MultiLine" Runat="server"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red"
											 contentEditable="false" ></asp:TextBox>										
									</ItemTemplate>
								</asp:TemplateColumn>
								
							</Columns>
						</asp:datagrid></td>
				</tr>		
						
				<tr><td class="emptycol" style="width:100%;"><div style="float:right;">
				    <table>
				        <tr><td class="emptycol" style="width: 150px; text-align:right; font-weight:bold; color:Black;">Sub Total :</td><td class="emptycol" style="width: 100px;"><div id="sSubTotal" name="sSubTotal" style="text-align:right; color:Black;" >0.00</div></td></tr>
				        <tr><td id="GSTLabel" runat="server" class="emptycol" style="text-align:right; font-weight:bold; color:Black;">SST Amount :</td><td class="emptycol"><div id="sTax" name="sTax" style="text-align:right; color:Black;" >0.00</div></td></tr>
				        <tr><td class="emptycol" colspan="2"><hr />
                        <tr><td class="emptycol" style="text-align:right; font-weight:bold; color:Black;">Grand Total :</td><td class="emptycol"><div id="sGrandTotal" style="text-align:right; font-weight:bold; color:Black;">0.00</div></td></tr>
				    </table>
				</div></td></tr>
								
				<%--<tr>
					<td><asp:datagrid id="dtgShopping" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgShopping_Page"
							DataKeyField="ITEMINDEX">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label ID="lblItemLine" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="ITEMINDEX" SortExpression="ITEMINDEX"  readonly="true"  HeaderText="S/No">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ITEMCODE" SortExpression="ITEMCODE"  readonly="true"  HeaderText="Buyer Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PRODUCTCODE" SortExpression="PRODUCTCODE"  readonly="true"  
									HeaderText="Product Code">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="VENDORITEMCODE" SortExpression="VENDORITEMCODE"  readonly="true"    HeaderText="Vendor Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="GLCODE" SortExpression="GLCODE"  readonly="true"    HeaderText="GLCODE">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="GL CODE" HeaderText="(GL CODE) GL Description">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboGLCode" CssClass="ddl" Runat="server"></asp:DropDownList>
										<input type="button" runat="server" class="button" id="btnGLSearch" style="width:20px"
											value=">" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CATEGORY CODE" HeaderText="Category Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboCategoryCode" CssClass="ddl" Runat="server"></asp:DropDownList>
										<input type="button" runat="server" class="button" id="btnCatSearch" style="width:20px"
											value=">" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="TAXCODE" SortExpression="TAXCODE"  readonly="true"    HeaderText="Tax Code">
									<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="PRODUCTDESC" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="200px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblProductDesc" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="MOQ" SortExpression="MOQ"  readonly="true"    HeaderText="Min Orderd Qty (MOQ)">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MPQ" SortExpression="MPQ"  readonly="true"    HeaderText="Min Packed Qty (MPQ)">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RFQ_QTY" SortExpression="RFQ_QTY"  readonly="true"    HeaderText="RFQ Qty">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TOLERANCE" SortExpression="TOLERANCE"  readonly="true"    HeaderText="Quotation Qty Tolerance">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="QUANTITY" HeaderText="PR Quantity">
									<HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="50px" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revQty" ControlToValidate="txtQty" ValidationExpression="^\d+$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revRange" ControlToValidate="txtQty" Runat="server"></asp:RangeValidator><INPUT class="txtbox" id="hidItemLine" type="hidden" runat="server" NAME="hidItemLine">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UOM" SortExpression="UOM"  readonly="true"    HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="UNITCOST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="lblnumerictxtbox" Width="100px" Runat="server"  readonly="true"   ></asp:TextBox>
										<asp:Label id="lblNoTax" CssClass="lbl" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Amount" HeaderText="Amount">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtAmount" CssClass="lblnumerictxtbox" Runat="server"  readonly="true"   ></asp:TextBox><INPUT id="hidGSTAmt" type="hidden" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="GST" HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtGST" CssClass="lblnumerictxtbox" Runat="server"  readonly="true"    Visible="False"></asp:TextBox>
										<asp:TextBox id="txtGSTAmt" CssClass="lblnumerictxtbox" Runat="server"  readonly="true"    Width="100px"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="SOURCE" SortExpression="SOURCE" HeaderText="SOURCE">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CATEGORYCODE" SortExpression="CATEGORYCODE"  readonly="true"   
									HeaderText="CATEGORYCODE">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDGROUP" SortExpression="CDGROUP" HeaderText="CDGROUP">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="Budget Account" HeaderText="Budget Account">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboBudget" CssClass="ddl" Runat="server"></asp:DropDownList>
										<INPUT class="button" id="cmdBudget" style="WIDTH: 12px; HEIGHT: 18px" type="button" value=">"
											name="cmdBudget" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Delivery Address">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboDelivery" CssClass="ddl" Runat="server"></asp:DropDownList>
										<INPUT class="button" id="cmdDelivery" style="WIDTH: 12px; HEIGHT: 18px" type="button"
											value=">" name="cmdDelivery" runat="server" width="30">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Est. Date of Delivery (days)">
									<HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" Width="40" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revETD" ValidationExpression="^\d+$" ControlToValidate="txtEstDate" Runat="server"></asp:RegularExpressionValidator>
										<asp:RequiredFieldValidator ID="reqETD" ControlToValidate="txtEstDate" Runat="server"></asp:RequiredFieldValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Warranty Terms (mths)">
									<HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtWarranty" Width="40" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revWarranty" ValidationExpression="^\d+$" ControlToValidate="txtWarranty" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="200px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" Width="100px" CssClass="listtxtbox" TextMode="MultiLine" Runat="server"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											 contentEditable="false" ></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>--%>
				<TR>
					<TD class="emptycol" style="HEIGHT: 20px"><INPUT class="txtbox" id="hidTotal" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidTotal" runat="server"> <INPUT class="txtbox" id="hidTax" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidTax" runat="server"> <INPUT class="txtbox" id="hidCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCnt" runat="server"> <INPUT class="txtbox" id="hidCost" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCost" runat="server">&nbsp;<INPUT class="txtbox" id="hidNewPR" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidNewPR" runat="server">&nbsp;<INPUT class="txtbox" id="hidAddItem" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidAddItem" runat="server">&nbsp;<INPUT class="txtbox" id="hidSupplier" style="WIDTH: 45px; HEIGHT: 18px" type="hidden"
							size="2" name="hidSupplier" runat="server">&nbsp;<INPUT class="txtbox" id="hidApproval" style="WIDTH: 45px; HEIGHT: 18px" type="hidden"
							size="2" name="hidApproval" runat="server"> <INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server"> <INPUT class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid1" runat="server"><INPUT class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid2" runat="server"><INPUT class="txtbox" id="hid3" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid3" runat="server"><INPUT class="txtbox" id="hid4" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid4" runat="server"><INPUT class="txtbox" id="hid5" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid5" runat="server"><INPUT class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid6" runat="server"><INPUT class="txtbox" id="hid7" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server">
							<input id="hidClientId" type="hidden" name="hidClientId" runat="server" />
							<%--<input id="hidOneVendor" type="hidden" name="hidOneVendor" runat="server" />--%>
							<input id="hidTotalClientId" type="hidden" name="hidTotalClientId" value="0" runat="server" />
					</TD>
				</TR>
				<%--<TR>
					<TD><asp:button id="cmdRaise" runat="server" CssClass="Button" Text="Save">
					</asp:button>&nbsp;<asp:button id="cmdSetup" runat="server" CssClass="Button" Width="94px" Text="Approval Setup">
					</asp:button>&nbsp;<asp:button id="cmdAdd" runat="server" CssClass="Button" CausesValidation="False" Text="Add Item">
					</asp:button>&nbsp;<asp:button id="cmdRemove" runat="server" CssClass="Button" CausesValidation="False" Text="Remove Item">
					</asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="Button" CausesValidation="False" Text="Delete PR">
					</asp:button>&nbsp;<asp:button id="cmdDupPRLine" runat="server" CssClass="Button" Width="100px" CausesValidation="False" Text="Duplicate Line Item"></asp:button>&nbsp;&nbsp;
					</TD>
				</TR>--%>
				<tr>
					<td class="emptycol" style="height: 24px">
					    <asp:button id="cmdRaise" runat="server" CssClass="Button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button>&nbsp;
					    <asp:button id="cmdSetup" runat="server" CssClass="Button" Width="94px" style="display:none" Text="Approval Setup"></asp:button>&nbsp;
					    <asp:button id="cmdAdd" runat="server" CssClass="Button" CausesValidation="false" Text="Add Item"></asp:button>&nbsp;
					    <asp:button id="cmdRemove" runat="server" CssClass="Button" Width="94px" CausesValidation="False" Text="Remove Item" ></asp:button>&nbsp;
					    <asp:button id="cmdDelete" runat="server" CssClass="Button" CausesValidation="False" Text="Void PR"></asp:button>&nbsp;
					    <asp:button id="cmdDupPRLine" runat="server" CssClass="Button" Width="100px" CausesValidation="False" Text="Duplicate Line Item"></asp:button>&nbsp;&nbsp;
					    <asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button> 
					</td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<tr>
					<td class="emptycol">&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
