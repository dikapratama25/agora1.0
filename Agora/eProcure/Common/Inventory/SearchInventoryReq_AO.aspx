<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchInventoryReq_AO.aspx.vb" Inherits="eProcure.SearchInventoryReq_AO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>SearchInventoryReq_AO</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
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
        
		<script type="text/javascript">
		<!--		
		    $(document).ready(function(){
                $('#cmdMassApp').click(function() {
                    if (CheckAtLeastOne('chkSelection') == false)
                    {		    
		                return false;
		            }
		            else
                    {  
                        document.getElementById("cmdMassApp").style.display= "none";
                        return true;
                    }             
                });   
            });
            
		    function selectAll()
		    {
			    SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
		    }
					
		    function checkChild(id)
		    {
			    checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
		    }
		
		    function Reset(){
//		        var today = new Date();
//		        var today_f = today.format("mm/dd/yyyy");
			    var oform = document.forms(0);
			    oform.txtIRNumber.value='';
			    oform.txtReqName.value='';
			    oform.txtIssue.value='';
                oform.txtDepartment.value='';
                oform.txt_startdate.value=oform.hidDateS.value;
                oform.txt_enddate.value=oform.hidDateE.value;
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
</head>
<body class="body">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
    <%  Response.Write(Session("w_InventoryReq_AO_tabs"))%>
        <table class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
			<tr>
					<td class="linespacing1" colspan="4"></td>
		    </tr>
			<tr>
                <td class="emptycol"  colspan="4">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Fill in the search criteria and click Search button to list the relevant Inventory requisition."
	                ></asp:label>

                </td>
            </tr>
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
			<td class="TableHeader" colspan="4">Search Criteria</td>
		    </tr>
			<tr class="tablecol">
			    <td class="TableCol" style="height: 18px; width: 15%;"><strong><asp:Label ID="Label1" runat="server" Text="IR Number :" CssClass="lbl"></asp:Label></strong></td>
			    <td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtIRNumber" runat="server" CssClass="txtbox" Width="125px"></asp:textbox></td>
			    <td class="TableCol" style="height: 18px" width="15%"><strong><asp:Label ID="Label4" runat="server" Text="Requestor Name :" CssClass="lbl"></asp:Label></strong></td>
                <td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtReqName" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>				
		    </tr>
		    <tr class="tablecol">
			    <td class="TableCol" style="height: 18px; width: 15%;"><strong><asp:Label ID="Label2" runat="server" Text="Issue To :" CssClass="lbl"></asp:Label></strong></td>
			    <td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtIssue" runat="server" CssClass="txtbox" Width="125px"></asp:textbox></td>
			    <td class="TableCol" style="height: 18px" width="15%"><strong><asp:Label ID="Label3" runat="server" Text="Department :" CssClass="lbl"></asp:Label></strong></td>
                <td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtDepartment" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>				
		    </tr>
		    <tr>
				<td class="tableCOL" style="width: 156px"><strong>Start Date :</strong></td>
				<td class="tableCOL">
				    <asp:textbox id="txt_startdate" width ="100px" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%><asp:requiredfieldvalidator id="rfv_txt_startdate" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txt_startdate"></asp:requiredfieldvalidator></td>
				<td class="tableCOL"><strong>End Date :</strong>
				<input id="hidDateS" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidDateS"
							runat="server"/></td>
				<td class="tableCOL">
				    <asp:textbox id="txt_enddate" width ="100px" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%><asp:requiredfieldvalidator id="rfv_txt_enddate" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txt_enddate"></asp:requiredfieldvalidator>
					<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual" Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;
					<input id="hidDateE" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidDateE"
							runat="server"/></td>
			</tr>
		    <tr class="tablecol">
		        <td class="TableCol" style="height: 24px; width: 156px;"></td>
		        <td class="TableCol" style="height: 24px" colspan="2"></td>
		        <td class="TableCol" style="height: 24px; text-align:right;">
			        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" ></asp:button>&nbsp;
                    <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server"/>
			    </td>
		    </tr>

			<tr>
				<td class="emptycol" colspan="6" ></td>
			</tr>
			
            <tr>
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
            <td class="emptycol" colspan="6">
                <asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
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
                        <asp:TemplateColumn SortExpression="IRM_IR_NO" HeaderText="IR Number">
                            <HeaderStyle Width="10%"></HeaderStyle>
				            <ItemTemplate>
					            <asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
				            </ItemTemplate>
			            </asp:TemplateColumn>					
                        <asp:BoundColumn SortExpression="IRM_IR_DATE" DataField="IRM_IR_DATE" HeaderText="IR Date">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="IRM_IR_REQUESTOR_NAME" DataField="IRM_IR_REQUESTOR_NAME" HeaderText="Requestor Name">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="IRM_IR_ISSUE_TO" DataField="IRM_IR_ISSUE_TO" HeaderText="Issue To">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="CDM_DEPT_NAME" DataField="CDM_DEPT_NAME" HeaderText="Department">
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="IRM_IR_REF_NO" DataField="IRM_IR_REF_NO" HeaderText="Reference No">
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="IRM_IR_REMARK" DataField="IRM_IR_REMARK" HeaderText="Remark">
                            <HeaderStyle Width="25%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn SortExpression="IRM_IR_INDEX" DataField="IRM_IR_INDEX" Visible="false"></asp:BoundColumn>
                    </Columns>
                </asp:datagrid>
                </td>
            </tr>
            <tr>
				<td class="emptycol" colspan="4">
                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                        ShowMessageBox="True" ShowSummary="False"/>
                </td>
			</tr>
			<tr>
				<td class="emptycol" colspan="4"><asp:button id="cmdMassApp" runat="server" Width="147px" CssClass="button" Text="Mass Approval"></asp:button>&nbsp;
				</td>
			</tr>
        </table>
        
    </form>
</body>
</html>
