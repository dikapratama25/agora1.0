<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuAccessRight.aspx.vb" Inherits="eAdmin.MenuAccessRight" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Menu Access Right Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>   
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("MenuAdd_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="Header"><asp:label id="lblTitle" runat="server">Menu Access Right</asp:label></TD>
				</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Step 1: Add a new module.<br>Step 2: Add a new node under a module.<br><b>=></b>Step 3: Assign menu access right to a fixed role."
						></asp:label>
                        </div>
					</TD>
				</TR>
				 <tr>
					<TD align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Note: User may add a new fixed role and assign a module for that fixed role."
						></asp:label>
                        </div>
					</TD>
			</TR>
				 
			</table>
           
            <TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
            <TR>
					<TD class="tableheader" >&nbsp;Search Criteria&nbsp;
                        <asp:RadioButton ID="rbNewFixedRole" runat="server" Text="New Fixed Role" autopostback = "true" />
                        <asp:RadioButton ID="rbCurFixedRole" runat="server" Text="Current Fixed Role" autopostback = "true"/></TD>
			</TR>
            <tr>
            <TD class="tablecol" ><strong>Module</strong> 
            :&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:DropDownList ID="ddlModule" cssclass = "ddl" runat="server">
            </asp:DropDownList></TD>
            </tr>
            
            <tr>
             <TD class="tablecol"><strong>Fixed Role</strong> 
             :&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
             <asp:DropDownList ID="ddlFixedRole" cssclass = "ddl" runat="server">
            </asp:DropDownList></td>
            </tr>
          
            <%--TD class="tablecol"><strong>User Group&nbsp;&nbsp;</strong>&nbsp;:&nbsp;&nbsp;
             <asp:DropDownList ID="ddlUsrGrp" cssclass = "ddl" runat="server">
            </asp:DropDownList></td--%>
            <tr>
            <TD class="tablecol"><strong>New Fixed Role</strong> : &nbsp;  
             <asp:textbox ID="txtNewFixedRole" cssclass = "txtbox" runat="server">
            </asp:textbox></td>
            </tr>
            <br />
            </table>
            <br />
         
            
				<table id="Table2" class="AllTable" cellpadding="0" cellspacing="0">
				
				<TR>
					<TD class="EmptyCol"><asp:button id="cmdSave" runat="server" cssclass="button" Text="Save"></asp:button></TD>
				</TR>
				
				</table>
				<br />
				<table class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
				
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
						</TD>
				</TR>
			    </TABLE>
		</form>
           
	</BODY>
</HTML>
