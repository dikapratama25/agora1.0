<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CatalogueUpload.aspx.vb" Inherits="eProcure.CatalogueUpload" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CatalogueUpload</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
<!--
function Test()
{
Form1.uploadedFile.click();
Form1.TextBox1.value=Form1.uploadedFile.value;
}
//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
			<INPUT id="uploadedFile" style="Z-INDEX: 102; LEFT: 54px; POSITION: absolute; TOP: 57px"
				type="file" size="32" name="uploadedFile" runat="server"><INPUT id="File2" type="file" size="32" name="uploadedFile2" runat="server" class="button"><INPUT id="File1" style="Z-INDEX: 105; LEFT: 67px; POSITION: absolute; TOP: 135px" type="file"
				size="32" name="uploadedFile3" runat="server" class="button">
			<asp:Button id="Button1" style="Z-INDEX: 101; LEFT: 232px; POSITION: absolute; TOP: 232px" runat="server"
				Text="Button"></asp:Button>
			<asp:TextBox id="TextBox1" style="Z-INDEX: 103; LEFT: 64px; POSITION: absolute; TOP: 232px" runat="server"></asp:TextBox>
			<asp:Button id="Button2" style="Z-INDEX: 104; LEFT: 351px; POSITION: absolute; TOP: 222px" runat="server"
				Text="Upload"></asp:Button>
			<asp:Button id="Button3" style="Z-INDEX: 106; LEFT: 368px; POSITION: absolute; TOP: 96px" runat="server"
				Text="upload multiple file" CssClass="button"></asp:Button><INPUT style="Z-INDEX: 107; LEFT: 152px; POSITION: absolute; TOP: 264px" type="button"
				value="Test" onclick="Test()">
			<asp:Button id="Button4" style="Z-INDEX: 108; LEFT: 80px; POSITION: absolute; TOP: 360px" runat="server"
				Text="Send Mail" Width="128px"></asp:Button>
			<asp:TextBox id="TextBox2" style="Z-INDEX: 109; LEFT: 80px; POSITION: absolute; TOP: 320px" runat="server"
				Width="248px"></asp:TextBox>
		</form>
	</body>
</HTML>
