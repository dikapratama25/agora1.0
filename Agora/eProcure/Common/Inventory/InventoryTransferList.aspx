<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryTransferList.aspx.vb" Inherits="eProcure.InventoryTransferList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Inventory Transfer List</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox="" + val + ""&seldate="" + txtVal.value")
        </script>
        <% Response.Write(css)%>  
        <% Response.write(Session("typeahead")) %>
		<% Response.Write(Session("WheelScript"))%>
        
		<script language="javascript">
		<!--		
		
		    function selectAll()
		    {
			    SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
		    }
					
		    function checkChild(id)
		    {
			    checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
		    }
		
		    function Reset(){
			    var oform = document.forms(0);
			    oform.txtITNumber.value="";
			    oform.txt_startdate.value="";
			    oform.txt_enddate.value="";
		    }
			
			function popCalendar(val)
		    {
			    txtVal= document.getElementById(val);
			    window.open('<% Response.Write(sOpen)%>' ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
		    }
		
	        function PopWindow(myLoc)
	        {
		        //window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
		        window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
		        return false;
	        }
	        
	        function checkDateTo(source, arguments)
	        {
	        if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
		        { 
		        arguments.IsValid=false;}
	        else
	        { 
		        arguments.IsValid=true;}
	        }
		
	        function checkDateFr(source, arguments)
		    {
		    if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			    { 
			    arguments.IsValid=false;}
		    else
		    { 
			    arguments.IsValid=true;}
		    }

        -->
        </script>
</HEAD>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
    <%  Response.Write(Session("w_InventoryTran_tabs"))%>
        <TABLE class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
			<tr>
					<TD class="linespacing1" colSpan="4"></TD>
		    </TR>
			<TR>
                <TD class="emptycol"  colSpan="4">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Fill in the search criteria and click Search button to list the relevant inventory transfer."
	                ></asp:label>

                </TD>
            </TR>
			<TR>
				<TD class="linespacing1" colspan="4"></TD>
			</TR>
			<TR>
			<TD class="TableHeader" colSpan="4">Search Criteria</TD>
		    </TR>
			<TR class="tablecol">
			    <TD class="TableCol" style="height: 18px" width="15%"><strong><asp:Label ID="Label1" runat="server" Text="IT Number :" CssClass="lbl"></asp:Label></strong></TD>
			    <TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtITNumber" runat="server" CssClass="txtbox" Width="125px"></asp:textbox></TD>
			    <TD class="TableCol" style="height: 18px" width="15%"></TD>
                <TD class="TableCol" style="height: 18px" width="30%"></TD>				
		    </TR>
		    <TR>
				<TD class="tableCOL"><STRONG>Start Date :</STRONG></td>
				<td class="tableCOL">
				    <asp:textbox id="txt_startdate" width ="100px" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
				<td class="tableCOL"><STRONG>End Date :</STRONG></td>
				<td class="tableCOL">
				    <asp:textbox id="txt_enddate" width ="100px" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%>
					<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual" Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</TD>
			</TR>	
		    <TR class="tablecol">
		        <TD class="TableCol" style="height: 24px"></TD>
		        <TD class="TableCol" style="height: 24px"></TD>
		        <TD class="TableCol" style="height: 24px"></TD>
		        <TD class="TableCol" style="height: 24px; text-align:right;">
			        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" ></asp:button>&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
			    </TD>
		    </TR>

			<TR>
				<TD class="emptycol" colSpan="6" ></TD>
			</TR>
			
            <tr>
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
            <td class="emptycol" colspan="6"><asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
            <Columns>
            <asp:TemplateColumn SortExpression="IT_TRANS_REF_NO" HeaderText="IT Number" >
				<HeaderStyle ></HeaderStyle>
				<ItemTemplate>
					<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateColumn>
								
            <asp:BoundColumn SortExpression="IT_TRANS_DATE" DataField="IT_TRANS_DATE" HeaderText="IT Date"></asp:BoundColumn>
            <asp:BoundColumn SortExpression="IT_REF_NO" DataField="IT_REF_NO" HeaderText="Reference No"></asp:BoundColumn>
            <asp:BoundColumn SortExpression="IT_REMARK" DataField="IT_REMARK" HeaderText="Remark"></asp:BoundColumn>
            </Columns>
            </asp:datagrid></td>
            
            <TR>
				<TD class="emptycol" colSpan="4">
                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                        ShowMessageBox="True" ShowSummary="False"/>
                </TD>
			</TR>
        </TABLE>

    </form>
</body>
</html>
