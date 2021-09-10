<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OverTimeClaim.aspx.vb" Inherits="eProcure.OverTimeClaim" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>OverTimeClaim</title>
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
			        $('#txtDateFr' + i).val('');
			        $('#txtDateTo' + i).val('');
			        $('#cboHourFrom' + i).val('00');
			        $('#cboMinFrom' + i).val('00');
			        $('#cboHourTo' + i).val('00');
			        $('#cboMinTo' + i).val('00');
			        $('#txtPurpose' + i).val('');
			        $('#lblHour' + i).text('');
			        $('#cboTimes' + i).val('');
			        $('#txtMA' + i).val('');
			    }
			}
		}
        
        function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
		}
		
		function hourMinCalculation(id) {
		    var dtFrom, dtF, dtD2, dtTo, dtT, dtR2;
            var tmFrom, tmDepart2, tmTo, tmReturn2;
            var timeDiff;
            var d, h, m, s;
            
		    //get value from both field
            dtFrom = $('#txtDateFr' + id).val();
            dtTo = $('#txtDateTo' + id).val();
            tmFrom = $('#cboHourFrom' + id).val() + ':' + $('#cboMinFrom' + id).val() + ':00';
            tmTo = $('#cboHourTo' + id).val() + ':' + $('#cboMinTo' + id).val() + ':00';
            
            //By default to empty
            $('#lblHour' + id).text('');
            if (dtFrom != '' && dtTo != '')
            {
                dtF = dtFrom.substring(3,5) + '/' + dtFrom.substring(0,2) + '/' + dtFrom.substring(6,10) + ' ' + tmFrom;
                dtT = dtTo.substring(3,5) + '/' + dtTo.substring(0,2) + '/' + dtTo.substring(6,10) + ' ' + tmTo;
                var startDate = new Date(dtF);
                var endDate = new Date(dtT);
                
                if (endDate > startDate)
                {
                    timeDiff = endDate - startDate; //in ms
                    s = Math.floor(timeDiff / 1000);
                    m = Math.floor(s / 60);
                    s = s % 60;
                    h = Math.floor(m / 60);
                    m = m % 60;
                    //d = Math.floor(h / 24);
                    //h = h % 24;

                    if (m < 10) {
                        $('#lblHour' + id).text(h + ':0' + m);
                    }
                    else {
                        $('#lblHour' + id).text(h + ':' + m);
                    }
                }
            }
		} 
		
		function hourCalculation(id) {
            var hourFrom, minFrom, hourTo, minTo;
            var hours, mins;
            //get value from dropdownlist
            hourFrom = $('#cboHourFrom' + id).val();
            hourFrom = parseFloat(hourFrom);
            minFrom = $('#cboMinFrom' + id).val();
            minFrom = parseFloat(minFrom);
            hourTo = $('#cboHourTo' + id).val();
            hourTo = parseFloat(hourTo);
            minTo = $('#cboMinTo' + id).val();
            minTo = parseFloat(minTo);
            
            $('#lblHour' + id).text('');
            if (hourFrom == 0 && minFrom == 0 && hourTo == 0 && minTo == 0){
                $('#lblHour' + id).text('');
            }
            else if (hourFrom == hourTo && minFrom > minTo){
                $('#lblHour' + id).text('');
            }
            else{   
                if (hourTo >= hourFrom){
                    hours = hourTo - hourFrom;
                    
                    if(minTo >= minFrom) {
                        mins = minTo - minFrom;
                    }
                    else {
                        mins = (minTo + 60) - minFrom;
                        hours--;
                    }
                    if (mins < 10) {
                        $('#lblHour' + id).text(hours + ':0' + mins);
                    }
                    else {
                        $('#lblHour' + id).text(hours + ':' + mins);
                    }
                }
            }
        }
        
        function GrandTotalHourCalculation()
		{
		    //Calculate total of Total Hours & Total Amt
			var totalhours = 0;
			var totalmins = 0;
			var hourMin, hours, mins;
			for (i=0; i < Form1.hidItemLine.value; i++)
			{
			    hourMin = $('#lblHour' + i).text();
			    if (hourMin != ''){
			        if (hourMin.length > 4){
			            hours = hourMin.substring(0, 2);
			            mins = hourMin.substring(3, 5);
			        }
			        else {
			            hours = hourMin.substring(0, 1);
			            mins = hourMin.substring(2, 4);
			        }

			        totalhours = totalhours + parseInt(hours);
			        totalmins = totalmins + parseInt(mins);
			        if (totalmins >= 60) {
			            totalmins = totalmins - 60;
			            totalhours++;    
			        }
			    }
			}
			
			if (totalhours == 0 && totalmins == 0){
			    $('#lblTotalHours').text('');
			}
			else{
			    if (totalmins < 10) {
                    $('#lblTotalHours').text(totalhours + ':0' + totalmins);
                }
                else {
                    $('#lblTotalHours').text(totalhours + ':' + totalmins);
                }
			}
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
			<table  class="alltable" width="1500px" id="Table1" cellspacing="0" cellpadding="0">
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
			    <td class="emptycol" colspan="5"></td>
			</tr>
			</table>
			<% Response.Write(Session("ConstructTableOverTime")) %>
			<%--<tr>
				<td colspan = "5" class="EmptyCol">
				    <% Response.Write(Session("ConstructTableOverTime")) %>
				</td>
			</tr>--%>
			<table  class="alltable" width="100%" id="Table2" cellspacing="0" cellpadding="0">
			<tr>
			    <td class="linespacing1"></td>
			</tr>
			<tr class="emptycol">
				<td style="HEIGHT: 7px"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;
				1.5 times on Normal and 1st Off Days<br/>
				&nbsp;&nbsp;&nbsp;&nbsp;2.0 times on Rest Days / 2nd Off Days, P.Holidays</td>
			</tr>
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
