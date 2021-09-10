<%@ Page Language="vb" AutoEventWireup="false" Codebehind="default.aspx.vb" Inherits="eProcurement._default" %>
<%@Import Namespace="System.Web.Security" %>
<HTML>
	<HEAD>
		<title>Forms Authentication</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<LINK href = """ & dDispatcher.direct("Plugins/CSS", "Styles.css") & """  type=""text/css"" rel=""stylesheet"">"

        </script>

		 <% Response.Write(sCSS)%> 
		<script language="vb" runat="server">
			Sub SignOut(objSender As Object, objArgs As EventArgs)
			'delete the users auth cookie and sign out
			FormsAuthentication.SignOut()
			'redirect the user to their referring page
			Response.Redirect(Request.UrlReferrer.ToString())
			End Sub
		</script>
	</HEAD>
	<body text="#000000" bgColor="#ffffff" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" runat="server">
			<table class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td align="center" width="22%" style="HEIGHT: 70px"><IMG height="60" src="Images/logo_tx123_2.jpg"></td>
					<TD id="oFilterDIV" style="FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=1, StartColorStr='#ffffff', EndColorStr='#F19402');HEIGHT: 70px"
						vAlign="middle" width="73%" colSpan="2"></TD>
					<td width="5%" bgColor="#f19402" style="HEIGHT: 70px"><IMG src="Images/eprocurementlogo.jpg"></td>
				</tr>
				<tr bgColor="lightgrey">
					<td colspan="2">
						<asp:Label Runat="server" ID="lblUserAndComp" Font-Bold="True" style="FONT-SIZE: 8pt; FONT-FAMILY: Verdana"></asp:Label>
					</td>
					<td align="right" colspan="2"><font color="#000000"><FONT style="FONT-SIZE: 8pt; FONT-FAMILY: Verdana"><b>Last 
									Log On</b> </FONT></font>
						<asp:label id="lblLastLogOn" Runat="server" Font-Bold="True" Font-Size="8pt" Font-Names="Verdana">N/A</asp:label>
					</td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;<br>
						<div id="divMenu" runat="server"></div>
						<div id="displayCredentials" runat="server"></div>
						&nbsp;
						<br>
						<br>
						<br>
						<p>
							&nbsp;<asp:Button Runat="server" ID="cmdSignOut" Text="Sign Out" Visible="False"></asp:Button></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
