<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmBuyerAssByAccCode.aspx.vb" Inherits="eProcure.bcmBuyerAssByAccCode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Account Setup</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>
	    <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		/*function selectAll()
		{
			SelectAllG("dgDept__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgDept__ctl2_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			//oform.txtUserID.value="";
			oform.txtDeptName.value="";
		}*/
		-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TBODY>
					<TR>
						<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="BCM Assignment by Account Code"></asp:label></TD>
					</TR>
					<tr><td class="rowspacing"></td></tr>
				    <TR>
	                    <TD class="EmptyCol" colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Click the Assign button to assign new user or Remove button to remove the user."></asp:label>
	                    </TD>
                    </TR>
                    <tr><td class="rowspacing"></td></tr>
					<TR>
						<TD class="emptycol">
							<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader" colSpan="2">&nbsp;Buyer Assigment&nbsp;Maintenance</TD>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
									<TD class="TableCol" style="HEIGHT: 6px"></TD>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" width="140">&nbsp;<STRONG>Acc. Code</STRONG>&nbsp;:</TD>
									<td class="TableCol"><asp:label id="lblAccCode" CssClass="lblInfo" Runat="server"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
									<td class="TableCol"><asp:label id="lblDesc" CssClass="lblInfo" Runat="server"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol">&nbsp;<STRONG>Dept. Name</STRONG>&nbsp;:</TD>
									<td class="TableCol"><asp:label id="lblDeptName" CssClass="lblInfo" Runat="server"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<td class="TableCol" style="HEIGHT: 7px" colSpan="2">&nbsp;</td>
								</TR>
								<tr class="tablecol">
									<td class="TableCol" style="HEIGHT: 384px"><STRONG> &nbsp;User</STRONG><span class="errorMsg">&nbsp;</span>:</td>
									<TD class="TableCol" style="HEIGHT: 384px">
										<table>
											<tr>
												<td class="TableCol" style="WIDTH: 306px" align="center">
													<P style="WIDTH: 594px; HEIGHT: 204px" align="center"><asp:listbox id="lstUserAvail" Width="592px" Height="174px" Runat="server" SelectionMode="Multiple"
															CssClass="listbox" AutoPostBack="false"></asp:listbox>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="cmdRight" CssClass="button" Runat="server" width="58px" text="Assign" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
														<asp:button id="cmdLeft" CssClass="button" Runat="server" width="58px" text="Remove" CausesValidation="False"></asp:button></P>
												</td>
												<td class="TableCol" align="center" width="70"><br>
													<br>
												</td>
												<td></td>
											</tr>
										</table>
										<asp:listbox id="lstUserSelected" Width="592px" Height="174px" Runat="server" SelectionMode="Multiple"
											CssClass="listbox" AutoPostBack="false"></asp:listbox>
									</TD>
								</tr>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD class="emptycol" colSpan="2"></TD>
					</TR>
				</TBODY>
			</TABLE>
			<table class="alltable" id="tblButton" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="emptycol" colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
