<%@ Page Language="vb" AutoEventWireup="false" Codebehind="QualityStd.aspx.vb" Inherits="eProcure.QualityStd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_QualityStd_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
				<TD>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Please update your company Quality Standards certification (eg. ISO9001, ISO9002, etc.)</br>Click on Browse button to attach the relevant certificates. The file name should reflect the certificate name for easy reference."
					></asp:label>

				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colSpan="2" width="100%"></TD>
				</TR>
							<TR>
								<TD class="tableheader" colspan="2" width="100%">&nbsp;Quality Standard Attachments</TD>
							</TR>
							<TR>
							<TD class="tablecol" width="25%"><STRONG> &nbsp;File Attachments&nbsp; </STRONG>:<br>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" Visible="true">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></TD>
							<TD class="tableinput" width="75%">
										<INPUT class="button" id="File1" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 288px;" type="file" name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmd_upload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button>
							</TR>
							<TR>
					<TD class="linespacing2" colSpan="2"></TD>
							</TR>
							<TR>
												<TD class="tablecol"><STRONG>&nbsp;File(s) Attached </STRONG>:</TD>
												<TD class="tableinput"><asp:panel id="pnlAttach" width="95%" runat="server"></asp:panel></TD>
											</TR>
											<TR>
												<TD class="tablecol" height="*"></TD>
												<TD class="tableinput"></TD>
											</TR>
				</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
