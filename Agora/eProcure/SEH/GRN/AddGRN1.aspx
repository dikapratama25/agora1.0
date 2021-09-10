<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddGRN1.aspx.vb" Inherits="eProcure.AddGRN1SEH" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
		<title>AddGRN</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            Dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
        </script> 
        <% Response.Write(Session("JQuery")) %>        
        <% Response.Write(Session("WheelScript"))%>
        <script type="text/javascript">
		<!--
		
		$(document).ready(function(){
        $('#cmdSubmit').click(function() {   
        document.getElementById("cmdSubmit").style.display= "none";
        });
        });
		
		
		

        function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
				
		function ResetDG()
		{
			var oform = document.forms[0];
			re = new RegExp('dtg')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				//alert(e.name);
				//alert(e.type);
				//alert(re.test(e.name));
				if (e.type=="text" && re.test(e.name))
					e.value=0;
				if (e.type=="textarea" && re.test(e.name))
					e.value="";
			}			
		}
		
		function removeQuot(str)
		{
			var r = new RegExp(",","gi");
			var newstr = str.replace(r, '') ;
			return newstr;
		}
		
		function focusControl(othcharge, price, unitcost, amtf, curr, grnfac, shipqty, rejqty) 
		{					
			Form1.hid1.value = othcharge;
			Form1.hid2.value = price
			Form1.hid3.value = unitcost;
			Form1.hid4.value = amtf;
			Form1.hid5.value = curr;
			Form1.hid6.value = grnfac;
			Form1.hid7.value = shipqty;
			Form1.hid8.value = rejqty;
		}
		 
		function calculateTotal(othcharge, price, unitcost, amtf, amtm, fac, curr, grnfac, inland, duties, cif, land, shipqty, rejqty) 
		{				
			Form1.hid1.value = othcharge; //Oth Charges
			Form1.hid2.value = price //Unit Price
			Form1.hid3.value = unitcost; 
			Form1.hid4.value = amtf;
			Form1.hid5.value = curr;
			Form1.hid6.value = grnfac;
			Form1.hid7.value = shipqty;
			Form1.hid8.value = rejqty;


			var ctlAmountF, ctlAmountM, ctlGRNFac, ctlUnitCost, ctlCIF, ctlLand, Quantity;
			var dblAmtF, dblAmtM, dblGRNFac, dblUnitCost, dblCIF, dblLand;
						
			var OthCharges = removeQuot(eval("Form1." + othcharge + ".value"));
			var RejectQty = removeQuot(eval("Form1." + rejqty + ".value"));
            var InlandCharges = removeQuot(eval("Form1." + inland + ".value"));
            var Duty = removeQuot(eval("Form1." + duties + ".value"));
            
            if (OthCharges=="") { OthCharges=0; }
            if (RejectQty=="") { RejectQty=0; }
            if (InlandCharges=="") { InlandCharges=0; }
            if (Duty=="") { Duty=0; }
            if (parseFloat(shipqty) < parseFloat(RejectQty))
            {
                Quantity = parseFloat(shipqty)
            }
            else
            {
                Quantity = parseFloat(shipqty) - parseFloat(RejectQty)
            }
            
            //Calculate Amount(F)
            ctlAmountF = document.getElementById(amtf);
            dblAmtF = (parseFloat(price) * parseFloat(Quantity)) + parseFloat(OthCharges)
            ctlAmountF.Value = parseFloat(dblAmtF).toFixed(2);
            $("#" + amtf + "").html(addCommas(ctlAmountF.Value, 2));    
            
            //Calculate Amount(M)
            ctlAmountM = document.getElementById(amtm);
            dblAmtM = parseFloat(dblAmtF) * parseFloat(curr)
            ctlAmountM.Value = parseFloat(dblAmtM).toFixed(2);       
            $("#" + amtm + "").html(addCommas(ctlAmountM.Value, 2)); 
            
            //Calculate GRN Factor
            ctlGRNFac = document.getElementById(fac);
            dblGRNFac = parseFloat(dblAmtM) * parseFloat(grnfac) / 100
            ctlGRNFac.Value = parseFloat(dblGRNFac).toFixed(2);
            $("#" + fac + "").html(addCommas(ctlGRNFac.Value, 2)); 
            
            //Calculate Unit Cost(M)
            ctlUnitCost = document.getElementById(unitcost);
            if (Quantity == 0) 
            {
                dblUnitCost = (parseFloat(price) * parseFloat(curr)) + 0  
            }
            else
            {
                dblUnitCost = (parseFloat(price) * parseFloat(curr)) + (parseFloat(dblGRNFac).toFixed(2) / parseFloat(Quantity))
            }
            ctlUnitCost.Value = parseFloat(dblUnitCost).toFixed(2);
            $("#" + unitcost + "").html(addCommas(ctlUnitCost.Value, 2));
            
            //Calculate CIF Values
            ctlCIF = document.getElementById(cif);
            dblCIF = parseFloat(dblGRNFac) + parseFloat(InlandCharges)
            ctlCIF.Value = parseFloat(dblCIF).toFixed(2);
            $("#" + cif + "").html(addCommas(ctlCIF.Value, 2));
            
            //Calculate LandCost
            ctlLand = document.getElementById(land);
            dblLand = parseFloat(dblCIF) + parseFloat(Duty)
            ctlLand.Value = parseFloat(dblLand).toFixed(2);
            $("#" + land + "").html(addCommas(ctlLand.Value, 2));
            
			calculateGrandTotal();
			}
			
		function calculateGrandTotal()
		{
		    //Calculate total of CIF, Landed Cost(GRN Value)
		    var sAllClient, iPos, sCurrentClientId;
			var dtotalCIF=0, dTotalGrnVal=0;
			var dtgGRNDtStk, totalCostid, totalCIFid;
			var temp;
			sAllClient = Form1.hidClientId.value;
			for (i=0; i < Form1.hidTotalClientId.value; i++)
			{
			    iPos = sAllClient.indexOf('|');	
			    sCurrentClientId = sAllClient.substring(0, iPos);
			    //alert(sCurrentClientId);
			    
			    dtgGRNDtStk = "dtgGRNDtStk_" + sCurrentClientId+"_lblCIF";
			    dtgGRNDtStk = $("#" + dtgGRNDtStk + "").html();
			    dtgGRNDtStk = dtgGRNDtStk.replace(",","");
			    dtgGRNDtStk = dtgGRNDtStk.replace(",","");
			    dtotalCIF = dtotalCIF + parseFloat(dtgGRNDtStk);
			    
			    dtgGRNDtStk = "dtgGRNDtStk_" + sCurrentClientId+"_lblLandCost";
			    dtgGRNDtStk = $("#" + dtgGRNDtStk + "").html();
			    dtgGRNDtStk = dtgGRNDtStk.replace(",","");
			    dtgGRNDtStk = dtgGRNDtStk.replace(",","");
			    dTotalGrnVal = dTotalGrnVal + parseFloat(dtgGRNDtStk);
			  
			    sAllClient = sAllClient.substring(iPos+1);
			}
			
			totalCIFid = Form1.hidTotalCIFID.value;
//			alert(totalCIFid);
		    var ctlTotalCIF = document.getElementById(totalCIFid);
		    ctlTotalCIF.Value = parseFloat(dtotalCIF).toFixed(2);
			$("#" + totalCIFid + "").html(addCommas(ctlTotalCIF.Value, 2));
			
			totalCostid = Form1.hidTotalLandCostID.value;
		    var ctlTotalCost = document.getElementById(totalCostid);
		    ctlTotalCost.Value = parseFloat(dTotalGrnVal).toFixed(2);
			$("#" + totalCostid + "").html(addCommas(ctlTotalCost.Value, 2));
	
			$("#lblGRNValue").html(addCommas(ctlTotalCost.Value, 2));
		
		
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
							
		function clear()
		{
			validatorReset();
			Form1.document.getElementById("vldSumm").style.display = "inline";	
		}
		
		
		function PopWindow(myLoc)
        {
            window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
            return false;
        }
        
        function ShowDialog(filename,height)
		{
			
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
//-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGRN_tabs"))%>
			<div id="DivGrn" style="DISPLAY: inline" runat="server">
				<table class="alltable" id="tblGrnHeader" cellspacing="0" cellpadding="0" border="0" style="width: 100%">
					<tr>
					<td class="linespacing1" colspan="5"></td>
			        </tr>					
					
					<tr>
	                    <td colspan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Click the Loc button to change the default location and Submit button to issue GRN."
		                    ></asp:label>

	                    </td>
                    </tr>
                    
                    <tr>
					<td class="linespacing2" colspan="6"></td>
			        </tr>
                    
					<tr>
						<td class="tableheader" colspan="6" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></td>
					</tr>
										
                    <tr>
					    <td class="tablecol" style="height: 17px; width:22%;"><strong>&nbsp;<asp:Label ID="Label3" runat="server">Vendor :</asp:Label></strong></td>
						<td class="tablecol" colspan="2" style="height: 17px; width:35%;" ><asp:label id="lblVendor" runat="server"></asp:label></td>
						
						<td class="tablecol" colspan="3" style="height: 17px; width:38%;" ></td>
						
			        </tr>
					<tr>
						<td class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:Label ID="Label2" runat="server">PO Number :</asp:Label></strong></td>
						<td class="tablecol" style="height: 17px;" ><asp:label id="lblPONo" runat="server"></asp:label></td>
						<td class="tablecol" align="left" style="height: 17px;">
                            <strong><asp:Label ID="Label4" runat="server" >PO Date :</asp:Label></strong></td>
                        <td class="tablecol" style="height: 17px;" ><asp:label id="lblPODate" runat="server"></asp:label></td>
                        <td class="tablecol" align="left" style="height: 17px;">
                            <strong><asp:Label ID="lblGRNVal" runat="server" >GRN Value:</asp:Label></strong></td>
                        <td class="tablecol" style="height: 17px;" ><asp:label id="lblGRNValue" CssClass="numerictxtbox"  runat="server"></asp:label></td>
					</tr>
					<tr>
						<td class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server">DO Number :</asp:Label></strong></td>
						<td class="tablecol" style="height: 17px;" ><asp:label id="lblDONum" runat="server"></asp:label></td>
						<td class="tablecol" align="left" style="height: 17px;"></td>
                        <td class="tablecol" colspan="3" style="height: 17px;" ></td>
					</tr>
					<tr>
					    <td class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:label id="lblActDt" runat="server">Actual Goods Received Date</asp:label></strong><strong>&nbsp;<asp:label id="Label5" runat="server">:</asp:label></strong></td>
						<td	 class="tablecol" style="height: 17px;"><asp:textbox id="txtReceivedDate" runat="server" Width="60px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtReceivedDate');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>						
						<td class="tablecol" style="height: 17px;"></td>
						<td class="tablecol" colspan="3" style="height: 17px;" ></td>
					</tr>	
					<tr id="tr_SEH1" style="display:none" runat="server">
						<td class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:Label ID="lblDL" runat="server">Default Location :</asp:Label></strong></td>
						<td class="tablecol" style="height: 17px;" ><asp:label id="lblDefaultLocation" runat="server"></asp:label></td>
						<td class="tablecol" align="left" style="height: 17px;">
                            <strong><asp:Label ID="lblSDL" runat="server" >Default Sub Location :</asp:Label></strong></td>
                        <td class="tablecol" colspan="3" style="height: 17px;" ><asp:label id="lblDefaultSubLocation" runat="server"></asp:label></td>
					</tr>				
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;<asp:label id="lblExtAttach" text="DO File(s) Attached" runat="server"></asp:label></strong> :</td>
					<td class="tableinput" valign="top" colspan="3">&nbsp;<asp:label id="lblFileAttach" runat="server"></asp:label></td>
					<td class="tableinput"></td>
					<td class="tableinput"></td>
				</tr>
				</table>
			</div>
			<div id="DODtl" style="DISPLAY: none" runat="server">
				<table width="100%">
					<tr>
						<td align="right">&nbsp;&nbsp;<%--<strong>Date Received</strong>--%> &nbsp;
							<asp:label id="txtReceivedDateBak" runat="server" Visible="False"></asp:label><!--<A onclick="popCalendar('txtReceivedDate');" href="javascript:;"></A>--></td>
					</tr>
					<tr>
						<td><asp:datagrid id="dtgGRNDtStk" runat="server"  AutoGenerateColumns="False" OnItemCommand="dtgGRNDtStk_ItemCommand"  OnSortCommand="SortCommand_Click" AllowPaging="false">
								<Columns>
									<asp:BoundColumn DataField="POD_Po_Line" SortExpression="POD_Po_Line" HeaderText="Line">
										<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Line" SortExpression="LineNo"></asp:TemplateColumn>
									<asp:BoundColumn DataField="POD_B_ITEM_CODE" SortExpression="POD_B_ITEM_CODE" HeaderText="Buyer Item Code"
										visible="false">
										<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Vendor_Item_Code" SortExpression="POD_Vendor_Item_Code" HeaderText="Item Code">
										<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Item Name">
										<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_B_GL_CODE" SortExpression="POD_B_GL_CODE" HeaderText="(GL Code) <br>GL Description">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Percent of Factor" Visible="false">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblPerFac" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="ExchangeRate" Visible="false">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblRate" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>
									<asp:BoundColumn DataField="POD_UNIT_COST" SortExpression="POD_UNIT_COST" HeaderText="Unit Price">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Disc Amt">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblDiscAmt" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Oth Charges(F)">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:textbox id="txtOthCharges" CssClass="numerictxtbox"  style="WIDTH: 80px" Runat="server"></asp:textbox>
								            <asp:RegularExpressionValidator id="rev_othcharges" runat="server"></asp:RegularExpressionValidator>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Unit Cost(M)">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblUnitCost" Width="50px" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Amount(F)" >
									    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									    <ItemTemplate>
										    <asp:label id="lblAmountF" Width="50px" Runat="server"></asp:label>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Amount(M)" >
									    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									    <ItemTemplate>
										    <asp:label id="lblAmountM" Width="50px" Runat="server"></asp:label>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="GRN Factor" >
									    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									    <ItemTemplate>
										    <asp:label id="lblGRNFac" Width="50px" Runat="server"></asp:label>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Inland Charges(M)">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:textbox id="txtInlandCharges" CssClass="numerictxtbox"  style="WIDTH: 80px" Runat="server"></asp:textbox>
								            <asp:RegularExpressionValidator id="rev_inlandcharges" runat="server"></asp:RegularExpressionValidator>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="CIF Values" >
									    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									    <ItemTemplate>
										    <asp:label id="lblCIF" Width="50px" Runat="server"></asp:label>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Duties(M)">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:textbox id="txtDuties" CssClass="numerictxtbox"  style="WIDTH: 80px" Runat="server"></asp:textbox>
								            <asp:RegularExpressionValidator id="rev_duties" runat="server"></asp:RegularExpressionValidator>
								        </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Landed Cost" >
									    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									    <ItemTemplate>
										    <asp:label id="lblLandCost" Width="50px" Runat="server"></asp:label>
									    </ItemTemplate>
								    </asp:TemplateColumn>
									<asp:BoundColumn DataField="POD_SPEC1" SortExpression="POD_SPEC1" HeaderText="Spec 1">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_SPEC2" SortExpression="POD_SPEC2" HeaderText="Spec 2">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_SPEC3" SortExpression="POD_SPEC3" HeaderText="Spec 3">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Lot No">
								        <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblLotNo" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>
									<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Ordered_Qty" SortExpression="POD_Ordered_Qty" HeaderText="Order Qty">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Outstanding" SortExpression="POD_Outstanding" HeaderText="Outstanding">
										<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Receive Qty">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GD_REJECTED_QTY" SortExpression="GD_REJECTED_QTY" HeaderText="Reject Qty (1st Level)">
										<HeaderStyle HorizontalAlign="Left" Width="1%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Reject Qty">
										<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
										<ItemTemplate>
											<asp:textbox id="txtReject" CssClass="numerictxtbox"  style="WIDTH: 60px" Runat="server"></asp:textbox>
										<asp:RegularExpressionValidator id="rev_qtycancel" runat="server"></asp:RegularExpressionValidator>
										</ItemTemplate>
									</asp:TemplateColumn>
																		
									<asp:TemplateColumn HeaderText="Location">
										<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>	
										<ItemTemplate>
											<%--<asp:textbox id="hidSub" Runat="server" style="display:none;"></asp:textbox>--%>
										    <%--<input class="button"  id="cmdSub"  type="button"
											value=">" name="cmdSub" style="WIDTH: 15px; HEIGHT: 22px" runat="server" />--%>
											<%--<asp:LinkButton CssClass ="button" id="lnkImage" runat="server" >delete</asp:LinkButton>--%>
											<center><asp:Button CssClass ="button" style="WIDTH: 26px; HEIGHT: 20px" Text = "Set" id="cmdSub" runat="server" ></asp:Button></center>
											
										</ItemTemplate>									
									</asp:TemplateColumn>
									
									<asp:TemplateColumn HeaderText="Remarks">
										<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
										<ItemTemplate>
											<asp:textbox ID="txtDtlRemarks" Runat="server" cssclass="txtbox" TextMode="MultiLine"
												Rows="2" Height="30px" MaxLength="400"></asp:textbox>
											<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
												 contentEditable="false" Visible="false"></asp:TextBox>
											<input class="txtbox" id="hidCode" type="hidden" runat="server" name="hidCode"/>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="DOD_SHIPPED_QTY" HeaderText="Shiped Qty">
										<HeaderStyle HorizontalAlign="Center" Width="5px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
					<tr>
						<td class="emptycol" style="HEIGHT: 6px">&nbsp;&nbsp;</td>
					</tr>
				</table>
			</div>
			<div id="PODtl" style="DISPLAY: inline" runat="server">
				<table class="alltable" cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td><asp:label id="lblSummPO" runat="server" Visible="False" Font-Bold="True">GRN History For Purchase Order : </asp:label>&nbsp;&nbsp;
							<asp:label id="lblPONum" runat="server" Visible="False"></asp:label></td>
					</tr>
					<tr>
						<td><asp:datagrid id="DtgGRNSumm" runat="server" OnSortCommand="SortCommand2_Click" AllowSorting="True">
								<Columns>
									<asp:BoundColumn DataField="GM_CREATED_DATE" SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
										<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
										<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GM_GRN_No" SortExpression="GM_GRN_No" HeaderText="GRN Number">
										<HeaderStyle HorizontalAlign="Left" Width="28%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Created By">
										<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
					<tr>
						<td class="emptycol">&nbsp;&nbsp;</td>
					</tr>
					<tr>
						<td><asp:button id="cmdAcceptAll" runat="server" CssClass="button" Text="Accept All" Visible="False"
								CausesValidation="False" Enabled="False"></asp:button>&nbsp;
							<asp:button id="cmdRejectAll" runat="server" CssClass="button" Text="Reject All" Visible="False"
								CausesValidation="False" Enabled="False"></asp:button>&nbsp;
							<asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
							<asp:button id="cmdReset" runat="server" CssClass="BUTTON" Text="Clear" CausesValidation="False"></asp:button>&nbsp;
							<input class="button" id="cmdPreviewPO" type="button" value="View PO" name="cmdPreviewPO"  style="Width:70px;"
								runat="server"/>
							<input type="button" value="View GRN" id="cmdPreviewGRN" runat="server" class="button" style="width: 75px" visible="false"/>
							<asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button>
						</td>
					</tr>
					<tr>
						<td class="emptycol" colspan="4"></td>
					</tr>
					<tr>
						<td><asp:validationsummary id="VldSumQty" runat="server" CssClass="errormsg"></asp:validationsummary><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
								runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
								runat="server"/><input class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid1" runat="server" /><input class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid2" runat="server" /><input class="txtbox" id="hid3" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid3" runat="server" /><input class="txtbox" id="hid4" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid4" runat="server" /><input class="txtbox" id="hid5" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid5" runat="server" /><input class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid6" runat="server" /><input class="txtbox" id="hid7" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid8" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid9" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid10" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid11" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid12" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" /><input class="txtbox" id="hid13" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" />
							<input id="hidClientId" type="hidden" name="hidClientId" runat="server" />
							<input id="hidOneVendor" type="hidden" name="hidOneVendor" runat="server" />
							<input id="hidTotalCIFID" type="hidden" name="hidTotalCIFID" runat="server" />
							<input id="hidTotalLandCostID" type="hidden" name="hidTotalLandCostID" runat="server" />
							<input id="hidTotalClientId" type="hidden" name="hidTotalClientId" value="0" runat="server" />
							
                            <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></td>
					</tr>
					<tr>
						<td class="emptycol" colspan="4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <asp:label id="lbl_check" runat="server" CssClass="errormsg" Width="400px" ForeColor="Red"></asp:label></td>
					</tr>
					<tr>
						<td class="emptycol" colspan="4"></td>
					</tr>
				</table>
			</div>
			<div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><strong>&lt; Back</strong></asp:hyperlink></div>
		</form>
	</body>
</html>
