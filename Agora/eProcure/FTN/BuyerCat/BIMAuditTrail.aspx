<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BIMAuditTrail.aspx.vb" Inherits="eProcure.BIMAuditTrailFTN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>Audit Trail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
          Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
           Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
        Dim itemcode As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=itemcode")
        Dim itemname As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=itemname")
      </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
        <% Response.Write(Session("BgiFrame"))%>
        <script language="javascript">
		<!--		
			$(document).ready(function(){
        
            $("#txtCode").autocomplete("<% Response.write(itemcode) %>", {
            width: 262,
            scroll: true,
            selectFirst: false
            });
            $("#txtCode").result(function(event, data, formatted) {
            if (data)
            $("#hidCode").val(data[1]);
            });        
            $("#txtCode").blur(function() {
            var hidCode = document.getElementById("hidCode").value;                        
            if(hidCode == "")
            {
                $("#txtCode").val("");
            }
            
            });                
            $("#txtDesc").autocomplete("<% Response.write(itemname) %>", {
            width: 262,
            scroll: true,
            selectFirst: false
            });
            $("#txtDesc").result(function(event, data, formatted) {
            if (data)
            $("#hidDesc").val(data[1]);
            });  
            $("#txtDesc").blur(function() {
            var hidDesc = document.getElementById("hidDesc").value;                        
            if(hidDesc == "")
            {
                $("#txtDesc").val("");
            }
            
            });                       
            });
            
                           
        	-->
		</script>
</head>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server">
    <%  Response.Write(Session("w_BIM_tabs"))%>
    <TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0">
        <tr><td class="rowspacing" colspan="6"></td></tr>
        <TR>
			<TD class="EmptyCol" colspan="6">
			    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="Microsoft Excel is required in order to open the report in Excel format. "></asp:label>					    
			</TD>
		</TR>
		<tr><td class="rowspacing"></td></tr>
		<TR>
			<TD class="tableheader" colspan="6">Report Criteria</TD>
		</TR>
		<tr class="tablecol">
		    <TD class="tablecol" width="15%"><STRONG>&nbsp;<asp:label id="Label3" runat="server" Text="Start Date " CssClass="lbl"></asp:label></STRONG>:</TD>
		    <TD class="tablecol" width="30%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
		    <TD class="tablecol" width="15%"><STRONG>&nbsp;<asp:label id="Label40" runat="server" Text="End Date " CssClass="lbl"></asp:label></STRONG>:</TD>
		    <TD class="tablecol" width="30%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%></TD>
		    <TD class="tablecol"></TD>
	    </tr>
	    <TR class="tablecol">
	        <TD class="tablecol" width="15%"><STRONG>&nbsp;<asp:Label ID="Label5" runat="server" Text="Item Code : "></asp:Label></STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
			    </STRONG>
		    </TD>
		    <TD class="tablecol" width="30%"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="243px"></asp:textbox><input type="hidden" id="hidCode" runat="server" /><asp:requiredfieldvalidator id="valGC" runat="server" ErrorMessage="Item Code is Required." ControlToValidate="txtCode"
				    Display="None"></asp:requiredfieldvalidator>
		    </TD>
		    <TD class="tablecol"  nowrap>
                        <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Item Name : "></asp:Label></strong></TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDesc" width="96%" runat="server" CssClass="txtbox" ></asp:textbox><input type="hidden" id="hidDesc" runat="server" /></TD>
            	<%--<td class="tablecol" colspan="2" nowrap style="height: 20px;">
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkActive" Text="Active" Runat="server" Checked="True"></asp:checkbox>
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkInActive" Text="Inactive" Runat="server" Checked="False"></asp:checkbox>
					</td>    --%>   
            <td class="tablecol" colspan="2" nowrap style="height: 20px;">
	    </TR>
	
		 <tr class="tablecol">
                   <%-- <td class="tablecol" nowrap="nowrap">
                        <strong>&nbsp;<asp:Label ID="Label4" runat="server" Text="Item Type : "></asp:Label></strong></td>
                         <TD class="tablecol">
                             &nbsp;&nbsp;<asp:CheckBox ID="chkSpot" runat="server" Text="Spot" />
                             &nbsp;<asp:CheckBox ID="chkStock" runat="server" Text="Stock" />
                             <asp:CheckBox ID="chkMRO" runat="server" Text="MRO" /></TD> --%>
                             		    <TD class="tablecol" width="15%"><STRONG>&nbsp;Report Type</STRONG><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
			    </STRONG>
		    </TD>
		    <TD class="tablecol" width="30%"><asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                    <asp:ListItem Selected="True">Excel</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
				    Display="None"></asp:requiredfieldvalidator>
		    </TD>
                    <td class="tablecol" colspan="3" nowrap style="height: 20px;">    
                    
                </tr>
	    <TR class="tablecol">
			<TD class="tablecol" colSpan="4"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>indicates required field
			</TD>
			<TD class="tablecol" ></TD>
		</TR>
		<TR>
			<TD class="emptycol" colSpan="4"></TD>
		</TR>
	<TR>
			<TD class="emptycol" >
			    <asp:button id="cmdView" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"	runat="server" name="cmdClear">
				<asp:comparevalidator id="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"
							Display="None" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtDateTo"></asp:comparevalidator>
			</TD>
		</TR>
		<TR>
			<TD class="emptycol" colSpan="4" ><BR>
				<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Width="600px"></asp:validationsummary>
			</TD>
		</TR>
    </TABLE>
    </form>
</body>
</html>
