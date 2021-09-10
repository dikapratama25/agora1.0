<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ApprovalRules.aspx.vb" Inherits="eAdmin.ApprovalRules" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ApprovalRules</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="WIDTH: 529px"><FONT size="1">&nbsp;</FONT><STRONG>PR Approval 
							Rule </STRONG>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;PR Approval Rule</TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol">&nbsp;<STRONG>PR Approval Rule&nbsp;:</STRONG></TD>
								<TD class="TableInput"><asp:radiobuttonlist id="opl_RulesOp" runat="server" RepeatDirection="Vertical" Height="14px">
										<asp:ListItem Value="A" Selected="True">Automated Approval<br></asp:ListItem>
										<asp:ListItem Value="B">Allow Lower Limit Endorsement</asp:ListItem>
										<asp:ListItem Value="C">Cut PO Before End of Approval List</asp:ListItem>
										<asp:ListItem Value="B+C">Allow Lower Limit Endorsement
										& Cut PO Before End of Approval List</asp:ListItem>
									</asp:radiobuttonlist>&nbsp;&nbsp;</TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol">&nbsp;<STRONG>Invoice Approval Rule&nbsp;:</STRONG></TD>
								<TD class="TableInput">&nbsp;<asp:CheckBox id="chkInvAppRule" runat="server" Text="Cut Invoice Before End of Approval List"></asp:CheckBox></TD>
							</TR>
						</TABLE>
					</TD>
					<td></td>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_Save" runat="server" Text="Save" CssClass="Button"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset2" type="reset" value="Reset" name="Reset1" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<td></td>
				</TR>
				<TR>
					<TD><strong>Note:</strong><br>
						<br>
						<u>Automate Approval</u><br>
						If this feature is enabled, the system selects the valid approval list(s) by 
						comparing the PR value against the minimum approval limit. Only approval limit 
						that is higher than the PR value will be displayed.
						<p></p>
						<u>Allow Lower Limit Endorsement</u><br>
						If this feature is enabled, the approving officers with approval limit lower 
						than the PR value will act as endorsement officers and allows to endorse on the 
						PR even if the PR value is higher. Officer who has the higher approval limit 
						than the PR value will act as approving officer. Only approval limit that is 
						higher than the PR value will be displayed.
						<P></P>
						<u>Cut PO Before End of Approval List</u><br>
						If this feature is enabled, the system will route the PR to the 1<sup>st</sup> authorised 
						approving officers who has the approval limit. There is no endorsement officers 
						on the PR. Once the PR is approved by the 1<sup>st</sup> level authorised 
						approving officer, the PO will be cut. Only approval limit that is higher than 
						the PR value will be displayed.
						<P></P>
						<u>Allow Lower Limit Endorsement &amp; Cut PO Before End of Approval List</u><br>
						If this feature is enabled, the system converts the PR to a PO upon approval by 
						the 1<sup>st</sup> authorized approving officer. If the officer is an 
						endorsement officer, the endorsed PR will be sent to the next officer and this 
						process continues until it reaches an officer who is authorized for approving 
						the PRs.
						<p></p>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
