<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BankCodeAdd.aspx.vb" Inherits="eProcure.BankCodeAdd" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>Bank_Code_Maintenance</title>
	    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%> 
		<script language="javascript">
		function ShowDialog(filename,height)
			{
				
				var retval="";
			
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 1100px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;
				} else {
				    window.close;
					return true;
				}
			}
		</script>
</head>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <% Response.Write(Session("w_User_tabs"))%>
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
				<tr>
	                <td class="EmptyCol" colSpan="2">
		                <asp:label id="Label3" runat="server"  CssClass="lblInfo"
		                        Text="Click Save button to save record and Add button to add a new bank."
		                ></asp:label>

	                </td>
                </tr>						
                <tr><td class="rowspacing" style="width: 155px"></td></tr>	    
				<tr>
					<td class="TableHeader" colSpan="2">Bank</td>
				</tr>	
			    <tr>
			        <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Bank Code"  CssClass="lbl"></asp:Label><span class="errorMsg">*</span><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
                    <td class="TableCol" style="width: 900px">&nbsp;<asp:TextBox ID="txtBankCode" runat="server" Width="300px" MaxLength="30" CssClass="txtbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqBankCode" runat="server" ErrorMessage="Bank code is required." ControlToValidate="txtBankCode" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>				
			    <tr>
				    <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Bank Name" CssClass="lbl" ></asp:Label><span class="errorMsg">*</span><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol">&nbsp;<asp:textbox id="txtBankName" runat="server" Width="300px" MaxLength="100" CssClass="txtbox"></asp:textbox>
				        <asp:RequiredFieldValidator ID="reqBankName" runat="server" ErrorMessage="Bank name is required." ControlToValidate="txtBankName" Display="none"></asp:RequiredFieldValidator>
				    </td>
			    </tr>
			    <tr>
				    <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label8" runat="server" Text="Bank Code Usage" CssClass="lbl" ></asp:Label><asp:Label ID="Label9" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol">
				        <asp:RadioButtonList CssClass="rbtn" ID="rbtnBCodeUsage" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
				            <asp:ListItem Text="LOCAL BANK TRANSFER-(RM)" Value="IBG" Selected="True"></asp:ListItem>
				            <asp:ListItem Text="TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)" Value="TT"></asp:ListItem>
                            <%--Zulham 19102018 - PAMB--%>
				            <asp:ListItem Text="CHEQUE-(RM)" Value="BC"></asp:ListItem>
                            <%--Zulham 17102018 - PAMB--%>
                            <asp:ListItem Text="BANK DRAFT-(FOREIGN CURRENCY)" Value="BD"></asp:ListItem>
				            <asp:ListItem Text="CASHIER'S ORDER-(RM)" Value="CO"></asp:ListItem>
                            <%--Zulham 05072018 - PAMB
                            Removed nostro option--%>
				            <%--Zulham 16/3/2015 IPP SGT Stage 2b--%>
				            <%--<asp:ListItem Text="NOSTRO" Value="NT"></asp:ListItem>--%> 
				        </asp:RadioButtonList>
				    </td>		
				</tr>   
			    <tr>
				    <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label6" runat="server" Text="Status" CssClass="lbl" ></asp:Label><asp:Label ID="Label7" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol">
				        <asp:RadioButtonList CssClass="rbtn" ID="rbtnstatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
				            <asp:ListItem Text="Active" Value="A" Selected="True"></asp:ListItem>
				            <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
				        </asp:RadioButtonList>
				    </td>
			    </tr>
			    <tr><td class="rowspacing" colSpan="2"></td></tr>	  
				<tr>
					<td class="EmptyCol"  colSpan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;					    
					    <asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="false"></asp:button>&nbsp;
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false"></asp:button>&nbsp;
					</td>
				</tr>
				<tr>
					<td colSpan="2" class="TableCol" style="background:none;">
                        <asp:ValidationSummary ID="vldsummary" runat="server" CssClass="errormsg"></asp:ValidationSummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				
			</table>
			
		</form>
	</body>
</html>
