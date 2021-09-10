<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryAddLocation.aspx.vb" Inherits="eProcure.InventoryAddLocation" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
  <head>
		<title>Location</title>
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
        <%  Response.Write(Session("w_User_tabs"))%>
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="50%">
             <tr><td class="rowspacing"></td></tr>		
				<tr>
	                <td class="EmptyCol" colSpan="2">
		                <asp:label id="Label3" runat="server"  CssClass="lblInfo"
		                        Text="Click Save button to save record and Add button to add a new location."
		                ></asp:label>

	                </td>
                </tr>						
                <tr><td class="rowspacing"></td></tr>	    
				<tr>
					<td class="TableHeader" colSpan="2">Location</td>
				</tr>				
			    <tr class="tablecol">
			        <td class="tablecol"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Location"  CssClass="lbl"></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong><span class="errorMsg">*</span>&nbsp;</td>
				    <td class="tablecol">&nbsp;<asp:textbox id="txtLocation" runat="server" Width="160px" MaxLength="30" CssClass="txtbox"></asp:textbox>
                            <asp:RequiredFieldValidator ID="reqLocation" runat="server" ErrorMessage="Location is required." ControlToValidate="txtLocation" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>				
			    <tr class="tablecol">
				    <td class="tablecol"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Sub Location" CssClass="lbl" ></asp:Label><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				    <td class="tablecol">&nbsp;<asp:textbox id="txtSubLocation" runat="server" Width="160px" MaxLength="30" CssClass="txtbox"></asp:textbox></td>
			    </tr>
			    <tr><td class="rowspacing" colSpan="2"></td></tr>	  
				<tr>
					<td class="EmptyCol"  colSpan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;					    
					    <asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="false"></asp:button>&nbsp;
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</td>
				</tr>
				<tr>
					<td colSpan="2" class="tablecol" style="background:none;">
                        <asp:ValidationSummary ID="vldsummary" runat="server" CssClass="errormsg"></asp:validationsummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				
			</table>
			
		</form>
	</body>
</html>
