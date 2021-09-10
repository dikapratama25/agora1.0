<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DOLotAttachment.aspx.vb" Inherits="eProcure.DOLotAttachment" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>DOLotAttachment</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
		<% Response.Write(Session("JQuery")) %>        
        <% Response.Write(Session("WheelScript"))%>
    	<script type="text/javascript">
		<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
			
		-->
		</script>	
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header"><font size="1">&nbsp;</font><asp:label id="lblTitle" runat="server">Attachment</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
				    <td class="tablecol" noWrap align="left">&nbsp;<strong>Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			        <td class="tableinput" colspan="5">
			        <input class="button" id="FileDoc" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" type="file" name="uploadedFile3" runat="server" />
			        <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
				</tr>
				<tr valign="top">
					<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
					<td class="tableinput" style="HEIGHT: 19px" colspan="5"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</td>
				</tr>
				
			</table>
		</form>
	</body>
</html>
