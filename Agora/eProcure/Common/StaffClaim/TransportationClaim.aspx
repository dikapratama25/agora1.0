<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TransportationClaim.aspx.vb" Inherits="eProcure.TransportationClaim" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>TransportationClaim</title>
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
			        $('#txtDate' + i).val('');
			        $('#txtLocFr' + i).val('');
			        $('#txtLocTo' + i).val('');
			        $('#txtPC' + i).val('');
			        $('#txtCSR' + i).val('');
			        $('#txtPurpose' + i).val('');
			        $('#cboTaxInvType' + i).val('N');
			        $('#cboCurr' + i).val('MYR');
			        $('#txtExRate' + i).val('');
			        $('#txtPT' + i).val('');
			        $('#txtParking' + i).val('');
			        $('#txtToll' + i).val('');
			        $('#txtAF' + i).val('');
			        $('#txtCM' + i).val('');
			        $('#lblCA' + i).text('');
			        $('#txtBM' + i).val('');
			        $('#lblBA' + i).text('');
			        $('#txtSOG' + i).val('');
			        $('#txtSP' + i).val('');
			        $('#txtFP' + i).val('');
			        $('#lblTotalAmt' + i).text('');
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
            
            calTotalAmt(id)
        }
		
        function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
		}
        
        function CalCarAmt(id) {
            var Mileage, decMileage;
            var doubleOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,1})?$/;
            var decTotalAmt = 0;
            Mileage = $('#txtCM' + id).val();
            
            $('#lblCA' + id).text('');
            if (Mileage != ''){
                if (doubleOnly.test(Mileage)){
                    decMileage = parseFloat(Mileage);
                    decTotalAmt = 0.6 * decMileage;
                    $('#lblCA' + id).text(decTotalAmt.toFixed(2));
                }
            }
            
            calTotalAmt(id)
        }
        
        function CalBikeAmt(id) {
            var Mileage, decMileage;
            var doubleOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,1})?$/;
            var decTotalAmt = 0;
            Mileage = $('#txtBM' + id).val();
            
            $('#lblBA' + id).text('');
            if (Mileage != ''){
                decTotalAmt = 0;
                if (doubleOnly.test(Mileage)){
                    decMileage = parseFloat(Mileage);
                    decTotalAmt = 0.3 * decMileage;
                }
                $('#lblBA' + id).text(decTotalAmt.toFixed(2));
            }
            
            calTotalAmt(id)
        }
        
        function calTotalAmt(id) {
            var PTAmt, ParkingAmt, TollAmt, AFAmt, CarAmt, BikeAmt, ExRate, Curr;
            var decPTAmt, decParkingAmt, decTollAmt, decAFAmt, decCarAmt, decBikeAmt;
            var decExRate = 0;
            var decTotalAmt = 0;
            var numbersOnly = /^\d+$/;
            var doubleOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,1})?$/;
            var decimalOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$/;
            //var doubleOnly = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,1})?$/;
            PTAmt = $('#txtPT' + id).val();
            ParkingAmt = $('#txtParking' + id).val();
            TollAmt = $('#txtToll' + id).val();
            AFAmt = $('#txtAF' + id).val();
            CarAmt = $('#lblCA' + id).text();
            BikeAmt = $('#lblBA' + id).text();
            ExRate = $('#txtExRate' + id).val();
            Curr = $('#cboCurr' + id).val();
            
            $('#lblTotalAmt' + id).text('');
            
            if (PTAmt != '' || ParkingAmt != '' || TollAmt != '' || AFAmt != '' || CarAmt != '' || BikeAmt != '')
            {
                if (Curr == 'MYR') {
                    decExRate = 1;
                } else {
                    if (ExRate != '' && decimalOnly.test(ExRate)){
		                decExRate = parseFloat(ExRate);
		            }
                }
                if (PTAmt != '' && decimalOnly.test(PTAmt)){
                    decPTAmt = parseFloat(PTAmt);
                    decTotalAmt += (decPTAmt * decExRate);
                }
                if (ParkingAmt != '' && decimalOnly.test(ParkingAmt)){
                    decParkingAmt = parseFloat(ParkingAmt);
                    decTotalAmt += (decParkingAmt * decExRate);
                }
                if (TollAmt != '' && decimalOnly.test(TollAmt)){
                    decTollAmt = parseFloat(TollAmt);
                    decTotalAmt += (decTollAmt * decExRate);
                }
                if (AFAmt != '' && decimalOnly.test(AFAmt)){
                    decAFAmt = parseFloat(AFAmt);
                    decTotalAmt += (decAFAmt * decExRate);
                }
                if (CarAmt != ''){
                    decCarAmt = parseFloat(CarAmt);
                    decTotalAmt += decCarAmt;
                }
                if (BikeAmt != ''){
                    decBikeAmt = parseFloat(BikeAmt);
                    decTotalAmt += decBikeAmt;
                }
                $('#lblTotalAmt' + id).text(decTotalAmt.toFixed(2));
            }
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
			<table  class="alltable"  width="100%" id="Table1" cellspacing="0" cellpadding="0">
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
					<asp:dropdownlist id="ddlSelect" runat="server" Width="160px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true"></asp:dropdownlist>
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
			    <td class="emptycol"></td>
			</tr>
			</table>
			<% Response.Write(Session("ConstructTableTrans")) %>
			<table  class="alltable"  width="100%" id="Table2" cellspacing="0" cellpadding="0">
			<tr>
			    <td class="linespacing1"></td>
			</tr>
			<tr>
				<td class="EmptyCol"><br/>
				    <asp:button id="cmdAddClaim" runat="server" CssClass="button" Text="Add Line"></asp:button>
				    <input class="button" id="cmdClear" onclick="clearFields()" runat="server" type="button" value="Clear Line" name="cmdClear"/> 
				    <asp:button id="cmdDupLine" runat="server" width="90px" CssClass="button" Text="Duplicate Line"></asp:button>
					<asp:button id="cmdSave" runat="server" width="100px" CssClass="button" Text="Save & Next"></asp:button>
					<asp:button id="cmdSaveSummary" runat="server" width="120px" CssClass="button" Text="Save & Summary"></asp:button>&nbsp;
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
