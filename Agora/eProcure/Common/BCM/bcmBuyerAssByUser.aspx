<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmBuyerAssByUser.aspx.vb" Inherits="eProcure.bcmBuyerAssByUser" %>
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
		/*
		function selectAll()
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
						<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="BCM Assignment by User"></asp:label></TD>
					</TR>
					<tr><td class="rowspacing"></td></tr>
				    <TR>
	                    <TD class="EmptyCol" colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Click the Assign button to assign new account or Remove button to remove the acocunt."></asp:label>
	                    </TD>
                    </TR>
                    <tr><td class="rowspacing"></td></tr>
					<TR>
						<TD class="emptycol">
							<TABLE class="alltable" id="Table4" style="WIDTH: 743px; HEIGHT: 448px" cellSpacing="0"
								cellPadding="0" border="0">
								<TR>
									<TD class="tableheader" colSpan="2">&nbsp;Buyer Assigment&nbsp;Maintenance</TD>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" style="WIDTH: 117px; HEIGHT: 6px" width="117"></TD>
									<TD class="TableCol" style="HEIGHT: 6px"></TD>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" style="WIDTH: 117px" width="117">&nbsp;<STRONG>Dept.&nbsp;Name</STRONG>&nbsp;:</TD>
									<td class="TableCol" ><asp:label id="lblDeptName" Runat="server" CssClass="lblInfo"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" style="WIDTH: 117px">&nbsp;<STRONG>User Name</STRONG>&nbsp;:</TD>
									<td class="TableCol" ><asp:label id="lblUserName" Runat="server" CssClass="lblInfo"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="TableCol" style="WIDTH: 117px">&nbsp;<STRONG>User ID</STRONG>&nbsp;:</TD>
									<td class="TableCol" ><asp:label id="lblUserID" Runat="server" CssClass="lblInfo"></asp:label></td>
								</TR>
								<TR class="tablecol">
									<td class="TableCol" style="HEIGHT: 1px" colSpan="2">&nbsp;</td>
								</TR>
								<tr class="tablecol">
									<td class="TableCol" style="WIDTH: 117px; HEIGHT: 4px"><STRONG>Account Codes</STRONG><span class="errorMsg">&nbsp;</span>:</td>
									<TD class="TableCol" style="HEIGHT: 0px">
										<table style="WIDTH: 600px; HEIGHT: 222px">
											<tr>
												<td class="TableCol" style="WIDTH: 306px" align="right"><asp:listbox id="lstAccAvail" Runat="server" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
														Height="174px" Width="592px"></asp:listbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
													&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="cmdRight" Runat="server" CssClass="button" CausesValidation="False" text="Assign"
														width="58px"></asp:button>&nbsp;&nbsp;
													<asp:button id="cmdLeft" Runat="server" CssClass="button" CausesValidation="False" text="Remove"
														width="58px"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
												<td class="TableCol" align="center" width="70"><br>
													<br>
												</td>
												<td></td>
											</tr>
										</table>
										<asp:listbox id="lstAccSelected" Runat="server" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
											Height="174px" Width="592px"></asp:listbox></TD>
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
					<TD class="emptycol" colSpan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button>
				    </TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
