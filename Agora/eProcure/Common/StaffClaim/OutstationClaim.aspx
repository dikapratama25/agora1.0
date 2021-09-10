<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OutstationClaim.aspx.vb" Inherits="eProcure.OutstationClaim" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>OutstationClaim</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
        
        function clearFields()
		{
		    for (i=0; i < Form1.hidItemLine.value; i++)
			{
			    if (document.getElementById('chkSelection' + i).checked == true){
			        $('#txtDepartDt' + i).val('');
			        $('#cboDepartHr' + i).val('00');
			        $('#cboDepartMin' + i).val('00');
			        $('#txtReturnDt' + i).val('');
			        $('#cboReturnHr' + i).val('00');
			        $('#cboReturnMin' + i).val('00');
			        $('#lblNoOfDay' + i).text('');
			        $('#txtDestination' + i).val('');
			        $('#txtPC' + i).val('');
			        $('#cboCurr' + i).val('MYR');
			        $('#txtExRate' + i).val('');
			        $('#txtMP' + i).val('');
			        $('#lblME' + i).text('');
			        $('#txtFreeMeal' + i).val('');
			        $('#lblAMC' + i).text('');
			        $('#lblTSAC' + i).text('');
			        $('#txtTAC' + i).val('');
			        $('#txtGstAmt' + i).val('');
			        $('#txtTAA' + i).val('');
			        $('#lblTotalAmt' + i).text('');
			        document.getElementById('txtExRate' + i).disabled = true;
			    }
			}
		}
		
		function enableExRate(id) {
            var ExRate, Curr;
            Curr = $('#cboCurr' + id).val();
            ExRate = $('#txtExRate' + id).val();
            
            if (Curr == 'MYR'){
                $('#txtExRate' + id).val('');
                document.getElementById('txtExRate' + id).disabled = true;
            }
            else{
                document.getElementById('txtExRate' + id).disabled = false;
            }
            
            CalTotalAmt(id)
        }
		
        function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
		
		function isValidDate(fulldate) {
		    var dateValues = fulldate.split('/');
		    var status = false;
            //var result = new Date(dateValues[2],parseInt(dateValues[1]) - 1,dateValues[0]);
            var result = new Date(dateValues[2] + '-' + dateValues[1] + '-' + dateValues[0]);
            alert(result);
            if (result == 'Invalid Date') {
                status = false;
            } else {
                status = true;
            }
            return status;
        }
        
        function CalMeal(id){
            var dtDepart, dtD, dtD2, dtReturn, dtR, dtR2;
            var tmDepart, tmDepart2, tmReturn, tmReturn2;
            var timeDiff, hourDiff, dayDiff;
            var freeMeal, int_freeMeal, mealRate, dec_mealRate;
            var decimalOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$/;
            var numbersOnly = /^\d+$/;
            
            // The number of milliseconds in one day
            var ONE_DAY = 1000 * 60 * 60 * 24

            //get value from both field
            dtDepart = $('#txtDepartDt' + id).val();
            dtReturn = $('#txtReturnDt' + id).val();
            tmDepart = $('#cboDepartHr' + id).val() + ':' + $('#cboDepartMin' + id).val() + ':00';
            tmReturn = $('#cboReturnHr' + id).val() + ':' + $('#cboReturnMin' + id).val() + ':00';

            //By default to empty
            $('#lblNoOfDay' + id).text('');
            $('#lblME' + id).text('');
            $('#lblAMC' + id).text('');
            $('#lblTSAC' + id).text('');
            if (dtDepart != '' && dtReturn != '')
            {
                dtD = dtDepart.substring(3,5) + '/' + dtDepart.substring(0,2) + '/' + dtDepart.substring(6,10) + ' ' + tmDepart;
                dtR = dtReturn.substring(3,5) + '/' + dtReturn.substring(0,2) + '/' + dtReturn.substring(6,10) + ' ' + tmReturn;
                var startDate = new Date(dtD);
                var endDate = new Date(dtR);
                
                if (endDate > startDate)
                {
                    timeDiff = endDate - startDate; //in ms
                    hourDiff = timeDiff / 3600 / 1000; //in hours
                    dayDiff = parseFloat(hourDiff / 24).toFixed(2); //in days
                    //Display result of No Of Days
                    $('#lblNoOfDay' + id).text(dayDiff); 
                    
                    dtD2 = dtDepart.substring(3,5) + '/' + dtDepart.substring(0,2) + '/' + dtDepart.substring(6,10);
                    dtR2 = dtReturn.substring(3,5) + '/' + dtReturn.substring(0,2) + '/' + dtReturn.substring(6,10);
                    var startDate2 = new Date(dtD2);
                    var endDate2 = new Date(dtR2);
                    // Convert both dates to milliseconds
                    var date1_ms = startDate2.getTime();
                    var date2_ms = endDate2.getTime();

                    // Calculate the difference in milliseconds
                    var difference_ms = Math.abs(date1_ms - date2_ms)
                    var int_Day = Math.round(difference_ms/ONE_DAY);
                    
                    tmDepart2 = $('#cboDepartHr' + id).val() + '.' + $('#cboDepartMin' + id).val()
                    tmReturn2 = $('#cboReturnHr' + id).val() + '.' + $('#cboReturnMin' + id).val()
                    var dec_Depart = parseFloat(tmDepart2);
                    var dec_Return = parseFloat(tmReturn2);
                    var int_MealEnt = 0;

                    if (int_Day > 0)
                    {
                        int_MealEnt += (int_Day - 1) * 3;
                        if (dec_Depart <= 6.3) { int_MealEnt += 1; }
                        if (dec_Depart <= 10.0) { int_MealEnt += 1; }
                        if (dec_Depart <= 16.0) { int_MealEnt += 1; }
                        if (dec_Return >= 10.3) { int_MealEnt += 1; }
                        if (dec_Return >= 14.0) { int_MealEnt += 1; }
                        if (dec_Return >= 20.0) { int_MealEnt += 1; }
                    }
                    else
                    {
                        if (dec_Depart <= 6.3 && dec_Return >= 10.3) { int_MealEnt += 1; }
                        if (dec_Depart <= 10.0 && dec_Return >= 14.0) { int_MealEnt += 1; }
                        if (dec_Depart <= 16.0 && dec_Return >= 20.0) { int_MealEnt += 1; }
                    }
                    //Display result of Meal Ent
                    $('#lblME' + id).text(int_MealEnt);
                    
                    freeMeal = $('#txtFreeMeal' + id).val()
                    if (freeMeal != ''){
                        if (numbersOnly.test(freeMeal)){
                            int_freeMeal = parseInt(freeMeal);
                            if(int_freeMeal > int_MealEnt){
                                int_MealEnt = 0;
                            }
                            else {
                                int_MealEnt = int_MealEnt - int_freeMeal;
                            }
                        }
                    }
                    //Display result of Actual Meal Claimed
                    $('#lblAMC' + id).text(int_MealEnt);
                    
                    var dec_TSAC;
                    mealRate = $('#txtMP' + id).val()
                    if (mealRate != ''){
                        if (decimalOnly.test(mealRate)){
                            dec_mealRate = parseFloat(mealRate);
                            dec_mealRate = dec_mealRate * int_MealEnt;
                            //Display result of Actual Meal Claimed
                            $('#lblTSAC' + id).text(dec_mealRate.toFixed(2));  
                        }
                    }
                    else {
                        $('#lblTSAC' + id).text('0.00');  
                    }
                       
                }
            }
            
            CalTotalAmt(id)
        }
        
        function CalTotalAmt(id){
            var TSAC, TAC, TAA, GstAmt, ExRate, Curr;
            var dec_TAC, dec_TAA, dec_GstAmt, decExRate;
            var TotalAmt = 0;
            var decimalOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$/;
            TSAC = $('#lblTSAC' + id).text();
            TAC = $('#txtTAC' + id).val();
            GstAmt = $('#txtGstAmt' + id).val();
            TAA = $('#txtTAA' + id).val();
            Curr = $('#cboCurr' + id).val();
            ExRate = $('#txtExRate' + id).val();
            
            //By default to empty
            $('#lblTotalAmt' + id).text('');
            
            if (TSAC != '' || TAA != '' || TAC != ''){
                if (TSAC != ''){
                    TotalAmt = TotalAmt + parseFloat(TSAC);
                }
                
                if (TAC != ''){
                    if (decimalOnly.test(TAC)){
                        dec_TAC = parseFloat(TAC);
                        TotalAmt = TotalAmt + dec_TAC;
                    }
                }
                
                if (GstAmt != ''){
                    if (decimalOnly.test(GstAmt)){
                        dec_GstAmt = parseFloat(GstAmt);
                        TotalAmt = TotalAmt + dec_GstAmt;
                    }
                }
                
                if (TAA != ''){
                    if (decimalOnly.test(TAA)){
                        dec_TAA = parseFloat(TAA);
                        TotalAmt = TotalAmt + dec_TAA;
                    }
                }
                
                decExRate = 0;
                if (Curr == 'MYR'){
			        decExRate = 1;
                }
                else{
                    if (ExRate != '' && decimalOnly.test(ExRate)){
			            decExRate = parseFloat(ExRate);
			        }
                }
                TotalAmt = TotalAmt * decExRate;
                
                $('#lblTotalAmt' + id).text(TotalAmt.toFixed(2));
//                if (TotalAmt > 0){
//                    $('#lblTotalAmt' + id).text(TotalAmt.toFixed(2));
//                }
            }
            
        }
				
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
		}  
        
        function isNumberKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

             return true;
        }
        
        function isDecimalKey(evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                return false;

            return true;
        }
        
        function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
        
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		    <%  Response.Write(Session("w_Staff_Claim_tabs"))%>
			<table  class="alltable"  width="1500px" id="Table1" cellspacing="0" cellpadding="0">
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<%--<tr>
				<td class="emptycol" colspan="5">
					<asp:label id="Label4" runat="server"  CssClass="lblInfo"
					Text="Please select staff claim form from drop-down list. (e.g. Hardship Claim Form...)"></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="linespacing2" colspan="5"></td>
			</tr>--%>
			<tr style="display:none;">
			    <td class="emptycol" colspan="5">
					<asp:dropdownlist id="ddlSelect" runat="server" Width="180px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true"></asp:dropdownlist>
			    </td>
			</tr>
			<%  Response.Write(Session("w_SC_Links"))%>
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr id="trInfo" runat="server">
				<td class="emptycol" colspan="5">
					<asp:label id="Label1" runat="server"  CssClass="lblInfo"
					Text="Click Save button to save record and Add line button to add a new row."></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>Staff Claim No </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblScNo" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Status </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblStatus" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>User Name </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblUserName" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Company </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblCompName" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>Business Division/Dept. </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblDeptId" runat="server" Visible="false"></asp:Label><asp:Label id="lblDept" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Document Date </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblDocDate" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
			    <td class="emptycol" colspan="5"></td>
			</tr>
			</table>
			<% Response.Write(Session("ConstructTableOut")) %>
			<%--<tr>
				<td colspan = "5" class="EmptyCol">
				    <% Response.Write(Session("ConstructTableHardship")) %>
				</td>
			</tr>--%>
			<table  class="alltable"  width="100%" id="Table2" cellspacing="0" cellpadding="0">
			<tr>
			    <td class="linespacing1"></td>
			</tr>
			<tr>
				<td class="EmptyCol"><br/>
				    <asp:button id="cmdAddClaim" runat="server" CssClass="button" Text="Add Line"></asp:button>
				    <input class="button" id="cmdClear" onclick="clearFields()" runat="server" type="button" value="Clear Line" name="cmdClear"/> 
				    <asp:button id="cmdDupLine" runat="server" width="90px" CssClass="button" Text="Duplicate Line"></asp:button>
					<asp:button id="cmdSave" runat="server" width="90px" CssClass="button" Text="Save & Next"></asp:button>
					<asp:button id="cmdSaveSummary" runat="server" width="110px" CssClass="button" Text="Save & Summary"></asp:button>&nbsp;
					<input id="hidItemLine" type="hidden" name="hidItemLine" runat="server" />
					</td>
			</tr>
			<tr>
				<td class="linespacing1"></td>
			</tr>
			<tr>
				<td class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			</tr>
			</table>
		</form>
	</body>
</html>
