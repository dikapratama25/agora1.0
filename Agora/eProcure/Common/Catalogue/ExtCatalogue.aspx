<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExtCatalogue.aspx.vb" Inherits="eProcure.ExtCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=ConCat")
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"'"<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"'"<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            
       </script>
       <% Response.Write(Session("JQuery")) %>	

       <% Response.Write(css)%>  
       <% Response.Write(Session("WheelScript"))%>
       <% Response.Write(Session("AutoComplete")) %>
       
		<script language="javascript">
		<!--
		   	
			function clearEndDate()
			{
				Form1.txtEndDate.value = "";
			}
		
		    $(document).ready(function(){
            $("#txtVendor").autocomplete("<% Response.write(typeahead) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });   
            
            $("#txtVendor").result(function(event, data, formatted) {
            if (data)
            $("#hidVendor").val(data[1]);
            });                  
            });
			/*function resetForm()
			{
				Form1.reset();
				ValidatorReset();
			}
			
			function ValidatorReset() {	
				alert(Page_Validators.length);		
				for (i = 0; i < Page_Validators.length; i++) {		
					Page_Validators[i].isvalid = true;												
				}
				ValidationSummaryOnSubmit()			
			}*/
			
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
			
			/*function confirmApprove(action)
            {
                ans = alert("Record Saved.");
                if (ans){
                    window.close(); 
                    window.opener.reloadPage();
                }                            
            }*/
		-->
		</script>
	</HEAD>
	<body onload="" class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<TR>
					<TD class="Header" colSpan="2"><asp:label id="lblTitle" runat="server" Text="Extend Contract Catalogue"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing" colSpan="2"></td></tr>
				<TR>
					<TD class="TableHeader" colSpan="2"><asp:label id="lblHeader" runat="server" Text="Extend Contract Catalogue" CssClass="lbl"></asp:label>
					</TD>
				</TR>
				<%--<TR class="tablecol" id="trCode" vAlign="top" runat="server">
					<TD class="tablecol" width="30%">
					    <STRONG><asp:label id="lblCodeLabel" runat="server" Text="Contract Ref. No." CssClass="lbl"></asp:label></STRONG>
					    <asp:label id="Label2" runat="server" CssClass="errormsg" Text="*"></asp:label>:					  
					    
					</TD>
					<TD class="tablecol" width="70%">
					    <asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="250px" Rows="1" MaxLength="50"></asp:textbox>
					    <asp:requiredfieldvalidator id="revCode" runat="server" ControlToValidate="txtCode" ErrorMessage="Contract Ref. No. is Required" Display="None"></asp:requiredfieldvalidator>
					</TD>
				</TR>--%>
				<%--<TR class="tablecol" id="trDesc" vAlign="top" runat="server">
					<TD class="tablecol" width="30%">
					    <STRONG><asp:label id="Label7" runat="server" Text="Description" CssClass="lbl"></asp:label></STRONG>
					    <asp:label id="Label8" runat="server" CssClass="errormsg" Text="*"></asp:label>:
					    
					</TD>
			        <TD class="tablecol" width="70%">
			            <asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="300px" MaxLength="100"></asp:textbox>
			            <asp:requiredfieldvalidator id="revDesc" runat="server" ControlToValidate="txtDesc" ErrorMessage="Description is required."
					        Display="None"></asp:requiredfieldvalidator>
			        </TD>
				</TR>--%>
				<%--<TR class="tablecol" id="trStart" vAlign="top" runat="server">
					<TD class="tablecol" width="30%">
					    <STRONG><asp:label id="Label4" runat="server" Text="Start Date" CssClass="lbl"></asp:label></STRONG>
				        <asp:label id="Label10" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				        					    
					</TD>
					<TD class="tablecol" width="70%">
					    <asp:textbox id="txtStartDate" runat="server" CssClass="txtbox" contentEditable="false" width="80px"></asp:textbox><% Response.Write(calStartDate)%>
					    <asp:requiredfieldvalidator id="revStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Start Date is required." Display="None"></asp:requiredfieldvalidator>
					</TD>
				</TR>--%>
				<TR class="tablecol" id="trEnd" vAlign="top" runat="server">
					<TD class="tablecol" width="30%">
					    <STRONG><asp:label id="Label1" runat="server" Text="Extend Date" CssClass="lbl"></asp:label></STRONG>
				        <asp:label id="lblEndDateMsg" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				          					    
					</TD>
					<TD class="tablecol" width="70%">
					    <asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" contentEditable="false" width="80px"></asp:textbox><% Response.Write(calEndDate)%>
					    <%--<asp:label id="lblClear" runat="server"></asp:label>--%>
					    <asp:requiredfieldvalidator id="revEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Extend Date is required."
							Display="None">
						</asp:requiredfieldvalidator>
						<%--<asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Start Date should be < End Date."
							Display="None" ControlToCompare="txtStartDate" Operator="GreaterThan" Type="Date">
						</asp:comparevalidator>--%>
						<asp:comparevalidator id="cvDateNow" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Extend Date should be greater than or equal to today's date."
							Display="None" Operator="GreaterThanEqual" Type="Date">
						</asp:comparevalidator>
					</TD>
				</TR>
				<%--<TR class="tablecol" id="trBuyer" vAlign="top" runat="server">
					<TD class="tablecol" width="30%">
					    <STRONG><asp:label id="Label12" runat="server" Text="Vendor Company" CssClass="lbl"></asp:label></STRONG>
				        <asp:label id="Label14" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				           					    
					</TD>
					<TD class="tablecol" width="70%">
					    <%--<asp:dropdownlist id="cboBuyer" runat="server" CssClass="ddl" width="40%" AutoPostBack="false"></asp:dropdownlist>
					    <asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="300px" MaxLength="100"></asp:textbox>
					    <asp:TextBox id="hidVendor" runat="server" style="display: none"></asp:TextBox>
			            <asp:requiredfieldvalidator id="revVendor" runat="server" ControlToValidate="txtVendor" ErrorMessage="Vendor Company is required."
					        Display="None"></asp:requiredfieldvalidator>
					</TD>
				</TR>--%>
				
				<TR>
					<TD class="emptycol" colspan="6"></TD>
				</TR>
				
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CDM_GROUP_INDEX" Width="100%">
							<Columns>
							    <asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
							    <asp:TemplateColumn SortExpression="CDM_GROUP_CODE" HeaderText="Contract Ref. No.">
									<HeaderStyle HorizontalAlign="Left" Width="24%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label ID="lblIndex" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>				
								<asp:BoundColumn DataField="CDM_GROUP_DESC" SortExpression="CDM_GROUP_DESC" ReadOnly="True" HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" ReadOnly="True" HeaderText="Vendor Company">
									<HeaderStyle HorizontalAlign="Left" Width="38%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_START_DATE" SortExpression="CDM_START_DATE" ReadOnly="True" HeaderText="Start Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_END_DATE" SortExpression="CDM_END_DATE" ReadOnly="True" HeaderText="End Date">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" ></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>	
				
				<tr>
					<TD class="tablecol" colSpan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
				</TR>
				<tr><td class="rowspacing" colSpan="2"></td></tr>	  
				<tr>
					<TD class="EmptyCol" colSpan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>
					    <%--<asp:button id="cmdCompany" runat="server" CssClass="Button" Width="128px" Text="Company Assignment"></asp:button>&nbsp;
					    <asp:button id="cmdItem" runat="server" CssClass="Button" Width="120px" Text="Contract Group Item"
							CausesValidation="False"></asp:button>&nbsp;
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;
						<INPUT class="button" id="cmdReset" onclick="ValidatorReset()" type="button" value="Reset"
							name="cmdReset" runat="server"> 
						<INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server">--%>
					</TD>
				</TR>
				<tr><td class="rowspacing" colSpan="2"></td></tr>	  
				<TR>
					<TD class="emptycol" colSpan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>