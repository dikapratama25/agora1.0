<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BatchExchangeRates.aspx.vb" Inherits="eProcure.BatchExchangeRates" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>BatchExchangeRates</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim SFromCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDtFrom") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim SToCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDtTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
        
        </script>
         <% Response.Write(css)%>   
		<script type="text/javascript">
        
        function isNumberKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode!=46)
                return false;

             return true;
        }
        
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table  class="alltable"  width="100%" id="Table1" cellspacing="0" cellpadding="0">
			<tr>
				<td class="Header" style="padding:0" colspan="4"><asp:label id ="lblTitle" runat="server" Text="Add Batch Exchange Rate"></asp:label></td>
			</tr>
			<tr>
			    <td class="rowspacing" style="height: 5px;" colspan="4"></td>
			</tr>
			<tr>
				<td class="EmptyCol" colspan="4">
					<asp:label id="lblAction" runat="server" CssClass="lblInfo"
					Text="Click Save button to save record."
					></asp:label>

				</td>				
			</tr>
			<tr>
			    <td class="EmptyCol" style="height: 5px;" colspan="4"></td>
			</tr>
			<tr>
				<td class="tablecol" style="width:15%;" ><strong>Valid Date From</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>				
				<td class="tablecol" style="width:35%;" >
                    <asp:TextBox ID="txtDtFrom" runat="server" CssClass="txtbox" Width="120px" contentEditable="false"></asp:TextBox><% Response.Write(SFromCalendar)%></td>				
				<td class="tablecol" style="width:15%;" ><strong>Valid Date To</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				<td class="tablecol" style="width:35%;" >
                    <asp:TextBox ID="txtDtTo" runat="server" CssClass="txtbox" Width="120px" contentEditable="false"></asp:TextBox><% Response.Write(SToCalendar)%></td>				
			</tr>
			<tr>
			    <td class="EmptyCol"  style="height: 5px;" colspan="4"></td>
			</tr>
			<tr>
			    <td class="EmptyCol" colspan="4"></td>
			</tr>
			<tr>
				<td colspan = "4" class="EmptyCol">
				    <asp:datagrid id="dtgExRate" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="30">  
				    <Columns>
				        <asp:BoundColumn HeaderText="No">
							<HeaderStyle Width="5%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" Width="5%"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="CODE_ABBR" HeaderText="Currency Code">
							<HeaderStyle Width="25%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="CODE_DESC" HeaderText="Currency Name">
							<HeaderStyle Width="50%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Rate *">
							<HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
							<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:textbox ID="txtRate" Runat="server" cssclass="numerictxtbox" Width="140px"></asp:textbox>
								<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red" ReadOnly="true" Visible="false"></asp:TextBox>
								<asp:RegularExpressionValidator id="valRate" Display="none" ErrorMessage="Invalid quantity." ControlToValidate="txtRate" Runat="server"></asp:RegularExpressionValidator>
								<asp:RequiredFieldValidator ID="reqValRate" runat="server" Display="none" ControlToValidate="txtRate" ErrorMessage="Require Quantity"></asp:RequiredFieldValidator>
							</ItemTemplate>
						</asp:TemplateColumn>
				    </Columns>   
				        
				    </asp:datagrid> 
				</td>
			</tr>
			<tr>
				<td colspan = "4" class="EmptyCol"><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field<asp:requiredfieldvalidator id="revDateFrom" runat="server" ControlToValidate="txtDtFrom" ErrorMessage="Valid Date From is required." Display="None"></asp:requiredfieldvalidator>
                            <asp:requiredfieldvalidator id="revDateTo" runat="server" ControlToValidate="txtDtTo" ErrorMessage="Valid Date To is required." Display="None"></asp:requiredfieldvalidator>
                            <asp:comparevalidator id="cvDateNow" runat="server" ControlToValidate="txtDtFrom" ErrorMessage="Valid Date From should be >= today's date."
							Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>       
                            <asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txtDtTo" ErrorMessage="Valid Date From should be < Valid Date To."
							Display="None" ControlToCompare="txtDtFrom" Operator="GreaterThan" Type="Date"></asp:comparevalidator></td>
			</tr>
			<tr>
				<td colspan = "4" class="EmptyCol"><br/>
					<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
					<input class="button" id="cmd_Close" type="button" value="Close" name="Close" runat="server"/>
					</td>
			</tr>
			<tr>
			    <td colspan="4" class="EmptyCol">
				</td>
			</tr> 
			<tr>
				<td colspan = "4" class="EmptyCol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
				<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			</tr>
			
			</table>
		</form>
	</body>
</html>
