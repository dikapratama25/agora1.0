<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CostCentreAddCode.aspx.vb" Inherits="eProcure.CostCentreAddCode" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
  <head>
		<title>Code Centre</title>
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
			
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 700px");
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
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" width="50%">
				<tr>
	                <td class="EmptyCol" colSpan="2">
		                <asp:label id="Label3" runat="server"  CssClass="lblInfo"
		                        Text="Click Save button to save record and Add button to add a new cost centre."
		                ></asp:label>

	                </td>
                </tr>						
                <tr><td class="rowspacing"></td></tr>	    
				<tr>
					<td class="TableHeader" colSpan="2">Cost Centre</td>
				</tr>	
				<tr>
			        <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label8" runat="server" Text="Company ID"  CssClass="lbl"></asp:Label><span class="errorMsg">*</span><asp:Label ID="Label9" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol">
				        <asp:dropdownlist ID="dlCompany" runat="server"  Font-Size="11px" Font-Names="Arial;Verdana">
				            <asp:ListItem Text="--Select--" Value = "0" />
				            <asp:ListItem Text="HLB" Value="1" />
				            <asp:ListItem text="HLISB" Value="2" />
				        </asp:dropdownlist>  
                    </td>
                </tr>				
			    <tr>
			        <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Cost Centre Code"  CssClass="lbl"></asp:Label><span class="errorMsg">*</span><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <%--Zulham 14/05/2018 - PAMB: 
                    Removed maxlength--%>
                    <td class="TableCol"><asp:textbox id="txtCostCentreCode" runat="server" Width="160px" CssClass="txtbox"></asp:textbox> 
                            <asp:RequiredFieldValidator ID="reqCostCentreCode" runat="server" ErrorMessage="Cost centre code is required." ControlToValidate="txtCostCentreCode" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>				
			    <tr>
				    <td class="TableCol"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Cost Centre Description" CssClass="lbl" ></asp:Label><span class="errorMsg">*</span><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol"><asp:textbox id="txtCostCentreDesc" runat="server" Width="264px" MaxLength="100" CssClass="txtbox"></asp:textbox>
				        <asp:RequiredFieldValidator ID="reqCostCentreDesc" runat="server" ErrorMessage="Cost centre description is required." ControlToValidate="txtCostCentreDesc" Display="none"></asp:RequiredFieldValidator>
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
					<td colSpan="2" class="TableCol" style="background:none; height: 57px;">
                        <asp:ValidationSummary ID="vldsummary" runat="server" CssClass="errormsg"></asp:ValidationSummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				
			</table>
			
		</form>
	</body>
</html>
