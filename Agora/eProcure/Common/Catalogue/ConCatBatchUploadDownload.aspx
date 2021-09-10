<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConCatBatchUploadDownload.aspx.vb" Inherits="eProcure.ConCatBatchUploadDownload" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Catalogue Batch Upload</title>
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
		<%  Response.Write(Session("w_ConCat_tabs"))%>
			<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="For batch upload, click on the Browse button to select the file, follow by the Upload button.<br>For batch download, select the Contract Catalogue and click on the Download button."></asp:label>					    
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="6">Batch Upload/Download</TD>
				</TR>
				<TR class="tablecol">
					<TD class="TableCol" width="20%"><strong>Option :</TD>
					<TD class="TableCol">
					    <asp:RadioButtonList ID="optUpDown" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="Upload" >Upload</asp:ListItem>
                            <asp:ListItem Value="Download">Download</asp:ListItem>
                        </asp:RadioButtonList>			
					</TD>
				</TR>
				<TR class="tablecol">
					<TD class="TableCol" width="20%"><B>File Location :</B><br>
						<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" Width="175px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></TD>
					<TD class="TableCol" ><input class="button" id="cmdBrowse" style="FONT-SIZE: 8pt; WIDTH: 320px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
							type="file" runat="server">&nbsp;</TD>
					
				</TR>
				<TR class="tablecol">
					<TD class="TableCol">&nbsp;</TD>
					<TD class="TableCol" style="HEIGHT: 5px">&nbsp;&nbsp;<asp:label id="lblPath" CssClass="txtbox" Runat="server" Height="5px"></asp:label></TD>
				</TR>
				<TR class="tablecol" id="trRefNo" runat="server">
		            <TD class="tablecol" width="20%">
		                <strong>Contract Ref. No. </strong>:</td>
		            <td class="TableCol"><asp:dropdownlist id="ddlCode" runat="server" Width="300px" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
	            </TR>
	            <TR>
					<TD class="emptycol"></TD>
				</TR>
	            <TR>
					<TD class="emptycol" colspan="2">Download Batch Upload/Download template - 
						<asp:linkbutton id="cmdDownloadTemplate" Runat="server">ContractCatalogueTemplate.xls [86KB]</asp:linkbutton>
					</TD>
				</TR>		
				<TR>
		            <TD class="emptycol" colspan="2">&nbsp;&nbsp;</TD>
	            </TR>
	            <TR>
		            <TD class="emptycol" colspan="2">
		                <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload"></asp:button>&nbsp;
		                <asp:button id="cmdDownload" runat="server" CssClass="button" Text="Download"></asp:button>
		            </TD>
	            </TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
			<tr><td class="rowspacing" colspan="6"></td></tr>
				<TR id="trResult" Runat="server">
					<TD class="tableheader" colspan="6">Result</TD>
				</TR>
				<tr id="tr_dg" runat="server">
					<td class="emptycol" colspan="2">
					    <asp:datagrid id="dg" Runat="server">
							<Columns>
								<asp:BoundColumn DataField="F1" HeaderText="No">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F2" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F3" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="F4" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F5" HeaderText="Currency Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F6" HeaderText="Contract Price">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F7" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F8" HeaderText="SST Tax Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F9" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Message" HeaderText="Message">
									<ItemStyle Wrap="False" ForeColor="Red"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr id="tr_dg2" runat="server">
					<td class="emptycol" colspan="2">
					    <asp:datagrid id="dg2" Runat="server">
							<Columns>
								<asp:BoundColumn DataField="F1" HeaderText="No">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F2" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F3" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="F4" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F5" HeaderText="Currency Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F6" HeaderText="Contract Price">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F7" HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F8" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Message" HeaderText="Message">
									<ItemStyle Wrap="False" ForeColor="Red"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>					
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
