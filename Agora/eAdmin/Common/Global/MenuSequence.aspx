<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuSequence.aspx.vb" Inherits="eAdmin.MenuSequence" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Menu Sequence Maintenance</title>
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
		<%Response.Write(Session("Menu_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="Header"><asp:label id="lblTitle" runat="server">Menu Sequence</asp:label></TD>
				</TR>
				<tr>
	            <td><asp:label id="lblAction" 
	            runat="server"  CssClass="lblInfo" 
	            Text="Select a module, assign a sequence number and click save to change the Menu Sequence">
	            </asp:label></td>
				</tr>
				</table>
            
            <TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
            <TR>
					<TD class="tableheader" >&nbsp;Search Criteria</TD>
			</TR>
            <TR>
            <TD class="tablecol" ><strong>New Module</strong> : &nbsp;  
                <asp:DropDownList ID="ddlModule" cssclass = "ddl" runat="server">
            </asp:DropDownList></TD>
          
            <tr></tr>
             <tr></tr>
         
             <TD class="tablecol"><strong>New Seq. No.</strong> :&nbsp;
             <asp:DropDownList ID="ddlSeqNo" cssclass = "ddl" runat="server">
            </asp:DropDownList></td>
            </tr>
            <br />
            </table>
            <br />
            <br />
            
				<table id="Table2" class="AllTable" cellpadding="0" cellspacing="0">
				<TR>
					<TD class="EmptyCol"><asp:datagrid id="dtgMenuSeq" runat="server">
							<Columns>
                                <asp:BoundColumn DataField="MM_MENU_IDX" HeaderText="SEQ. ID">
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Menu Name">
                                <ItemTemplate>
                                    <asp:TextBox id="txtPanelName" cssclass="txtbox" MaxLength="50" runat="server" Width="70%" disabled>
                                    </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"><asp:button id="cmdSave" runat="server" cssclass="button" Text="Save"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
