<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CatBatchUpload.aspx.vb" Inherits="eAdmin.CatBatchUpload" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>test_by_kk</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 17px"><asp:label id="lblTitle" runat="server" CssClass="header">Batch Upload / Download</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<td>
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader" colSpan="3">&nbsp;<asp:label id="lblHeader" runat="server">Batch Upload/Download</asp:label></TD>
							</TR>
							<TR class="tablecol">
								<TD style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD style="HEIGHT: 6px"></TD>
							</TR>
							<TR class="tablecol">
								<TD width="100">&nbsp;<B>File Location :</B><br>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="div" Width="176px">Recommended file size is 300KB</asp:Label></TD>
								<TD>&nbsp;&nbsp;<input class="button" id="cmdBrowse" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" runat="server">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR class="tablecol">
								<TD>&nbsp;</TD>
								<TD style="HEIGHT: 5px">&nbsp;&nbsp;<asp:label id="lblPath" CssClass="txtbox" Runat="server" Height="5px"></asp:label></TD>
							</TR>
							<TR class="tablecol">
								<TD style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD style="HEIGHT: 6px"></TD>
							</TR>
						</TABLE>
					</td>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<tr>
					<td><asp:datagrid id="dg" Runat="server">
							<Columns>
								<asp:BoundColumn DataField="F1" HeaderText="No">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F2" HeaderText="Item Id">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F3" HeaderText="Company Id">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F4" HeaderText="Item Desc">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F5" HeaderText="Category Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F6" HeaderText="Category Desc">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F7" HeaderText="Catalogue Price">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F8" HeaderText="Currency Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F9" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F10" HeaderText="Tax Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F11" HeaderText="Mgmt Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F12" HeaderText="Mgmt Text">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F13" HeaderText="Vendor Item Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F14" HeaderText="Brand">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F15" HeaderText="Model">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F16" HeaderText="Action">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Message" HeaderText="Message">
									<ItemStyle Wrap="False" ForeColor="Red"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">Click
						<asp:linkbutton id="cmdDownloadTemplate" Runat="server">here</asp:linkbutton>&nbsp;to 
						download template.</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload"></asp:button>&nbsp;<asp:button id="cmdDownload" runat="server" CssClass="button" Text="Download"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
