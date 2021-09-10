<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ForgotPwd.aspx.vb" Inherits="eProcurement.forgotPwd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Forgot Password</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	    <script runat="server">
	       Dim dDispatcher As New AgoraLegacy.dispatcher
	        Dim sCSS As String = "<LINK href = """ & dDispatcher.direct("Plugins/CSS", "Styles.css") & """  type=""text/css"" rel=""stylesheet"">"
        </script>
        <% Response.Write(sCSS)%> 
        
        
        <script language="javascript">
            function Reset(){
				var oform = document.forms(0);
				oform.txtUserId.value="";
				oform.txtCompID.value="";
                oform.txtEmail.value="";
                oform.txtQuestion.value="";
                oform.txtAns.value="";
			}
//        function txtEl_onBlur() 
//        { 
//        var bt = document.getElementById("btClick"); 
//        bt.click(); 
//        }     
        </script>

		 
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="form1" method="post" runat="server">
			<TABLE class="table" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colspan="2">Forgot your password</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<td class="tableheader" colSpan="2" style="height: 19px">
						&nbsp;Forgot your password
					</td>
				</TR>
				<tr class="tablecol">
					<td colSpan="2">&nbsp;<asp:label id="lblInfo" Runat="server"></asp:label></td>
				</tr>
				<tr class="emptycol">
					<td colSpan="2">&nbsp;</td>
				</tr>
				<TR>
					<td class="tableheader" borderColorDark="black" colSpan="2">
						&nbsp;Confirm Your Identity
					</td>
				</TR>
				<tr class="tablecol">
					<td align="left" width="20%" class="tablecol"><STRONG>&nbsp;User ID</STRONG><span class="errormsg">*</span>&nbsp;:</td>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtUserId" runat="server" CssClass="txtbox" Width="160px" MaxLength="20" AutoPostBack="True" ontextchanged="txtUserID_TextChanged"></asp:textbox>
						<asp:Label id="lblUserId" runat="server"></asp:Label></TD>
				</tr>
				<tr class="tablecol">
					<td align="left" class="tablecol"><STRONG>&nbsp;Company ID</STRONG><span class="errormsg">*</span>&nbsp;:</td>
					<TD class="tableinput">&nbsp;
                        <asp:textbox id="txtCompID" runat="server" Width="160px" CssClass="txtbox" MaxLength="20" AutoPostBack="True" ontextchanged="txtCompID_TextChanged"></asp:textbox>
						<asp:Label id="lblCompanyId" runat="server"></asp:Label>
						</TD>
				</tr>
				<tr class="tablecol">
					<td class="tablecol" style="height: 25px"><STRONG>&nbsp;Email</STRONG><span class="errormsg">*</span>&nbsp;:</td>
					<TD class="tableinput" style="height: 25px">&nbsp;
                        <asp:textbox id="txtEmail" runat="server" CssClass="txtbox" Width="160px" MaxLength="160"></asp:textbox>
						<asp:Label id="lblEmail" runat="server"></asp:Label>
                        <%--<input id="btClick" runat="server" type="button" value="button" style="DISPLAY:none" visible="true" />--%>&nbsp;
                        <%--<input id="txtEmail2" type="text" onblur="txtEl_onBlur()" style="width: 160px" class="txtbox" tabindex="0" />--%></TD>
				</tr>
				<tr class="tablecol">
					<td class="tablecol">&nbsp;</td>
					<td class="tableinput">&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td class="tablecol" style="height: 12px"><STRONG>&nbsp;Challenge Phrase</STRONG>&nbsp;:</td>
					<TD class="tableinput" style="height: 12px">&nbsp; 
                        <asp:TextBox ID="txtQuestion" runat="server" CssClass="txtbox" Width="250px" ReadOnly="True"></asp:TextBox>
						<asp:Label id="lblQuestion" runat="server"></asp:Label>
						<asp:DropDownList id="cboQuestion" style="DISPLAY:none" runat="server" CssClass="ddl" Width="79px"></asp:DropDownList>
                        &nbsp;
						</TD>
				</tr>
				<tr class="tablecol">
					<td class="tablecol"><STRONG>&nbsp;Answer</STRONG><span class="errormsg">*</span>&nbsp;:</td>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtAns" runat="server" CssClass="ddl" Width="250px" MaxLength="150"></asp:textbox>
						<asp:Label  id="lblAnswer" runat="server"></asp:Label></TD>
				</tr>
                &nbsp;<TR class="emptycol">
					<TD style="HEIGHT: 7px" colspan="2"><span class="errormsg">*</span>&nbsp;indicates 
						required field</TD>
				</TR>
				<tr>
					<td align="left" colspan="2">&nbsp;</td>
				</tr>
				<tr runat="server" id="trSave">
					<TD align="left" colspan="2" style="height: 9px"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
					<%--<INPUT class="button" id="cmdReset" type="reset" value="Clear" name="cmdReset" runat="server" tabindex="0">--%>
					<INPUT class="button" id="cmdReset" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
					</TD>
				</tr>
				<TR runat="server" id="trRemark">
					<TD colSpan="2"><table border="1" cellpadding="0" cellspacing="0" bordercolorlight="black">
							<tr>
								<td><asp:label id="lblMsg3" CssClass="lblmsg" Runat="server"></asp:label></td>
							</tr>
						</table>
					</TD>
				</TR>
				<tr class="emptyCol">
					<td colSpan="2">&nbsp;</td>
				</tr>
				<tr class="emptyCol" runat="server" id="trBack">
					<td colspan="2">
						<asp:button id="cmdBack" runat="server" Width="128px" CssClass="button" Text="Back to Login Screen"></asp:button></td>
				</tr>
				<tr>
					<td align="left" colspan="2"><asp:label id="lblMsg2" Runat="server" ForeColor="Red" CssClass="errormsg"></asp:label>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
