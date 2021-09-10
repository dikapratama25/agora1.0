<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BIMBatchUpload.aspx.vb" Inherits="eProcure.BIMBatchUploadSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>BIM Batch Upload</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
       <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_BIM_tabs"))%>
			<table class="alltable" id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header" style="height: 3px;"></td>
				</tr>
				<tr>
					<td class="header" style="HEIGHT: 17px"><asp:label id="lblTitle" runat="server" CssClass="header">Batch Upload / Download</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td>
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr>
								<td class="tableheader" colspan="4" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server">Batch Upload/Download</asp:label></td>
							</tr>
							<tr class="tablecol">
								<td style="WIDTH: 92px; HEIGHT: 6px" width="150"></td>
								<td style="HEIGHT: 6px; width: 333px;"></td>
								<td style="HEIGHT: 6px"></td>
								<td style="WIDTH: 92px; HEIGHT: 6px"></td>
							</tr>
							<tr class="tablecol">
								<td width="150">&nbsp;<b>File Location :</b><br/>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" Width="175px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
								<td style="width: 333px">&nbsp;&nbsp;<input class="button" id="cmdBrowse" style="FONT-SIZE: 8pt; WIDTH: 320px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" runat="server"/>&nbsp;</td>
								<td></td>
								<td><div style="width:200px">
                                        <asp:radiobuttonlist ID="rd1" RepeatDirection="Horizontal" runat="server" Width="195px" AutoPostBack="true"> 
                                        <asp:ListItem Value="I" Selected="True">Item</asp:ListItem>
									    <asp:ListItem Value="BP">Budget Price</asp:ListItem>
									    </asp:radiobuttonlist>
								    </div></td>
							</tr>
							<tr class="tablecol">
								<td style="height: 19px">&nbsp;</td>
								<td style="HEIGHT: 19px; width: 333px;">&nbsp;&nbsp;<asp:label id="lblPath" CssClass="txtbox" Runat="server" Height="5px"></asp:label></td>
								<td style="HEIGHT: 19px"></td>
								<td style="height: 19px"></td>
							</tr>
							<tr class="tablecol">
								<td style="WIDTH: 92px; HEIGHT: 6px" width="92"></td>
								<td style="HEIGHT: 6px; width: 333px;"></td>
								<td style="HEIGHT: 6px"></td>
								<td style="HEIGHT: 6px"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:datagrid id="dg" Runat="server">
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
								<asp:BoundColumn DataField="F4" HeaderText="Item Type">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F6" HeaderText="Commodity Type">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F7" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F47" HeaderText="Action">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Message" HeaderText="Message">
									<ItemStyle Wrap="False" ForeColor="Red"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:datagrid id="dg2" Runat="server">
							<Columns>
								<asp:BoundColumn DataField="F1" HeaderText="No">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F2" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left"  Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F3" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left"  Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="F7" HeaderText="Proposed Budget">
									<HeaderStyle HorizontalAlign="Right"  Width="1%"></HeaderStyle>	
									<ItemStyle HorizontalAlign="Right"></ItemStyle>		
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Message" HeaderText="Message">
									<ItemStyle Wrap="False" ForeColor="Red"  Width="30%"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">Download Batch Upload/Download template - 
						<asp:linkbutton id="cmdDownloadTemplate" Runat="server">ItemBIMTemplate.xls [232KB]</asp:linkbutton>
					</td>
				</tr>	
				<tr>
					<td class="emptycol">Download UNSPSC commodity reference codeset - 
						<asp:linkbutton id="cmdDownloadTemplateCode" Runat="server">UNSPSC v13.1201.xlsx [2MB]</asp:linkbutton> or  
						<asp:linkbutton id="cmdDownloadTemplateCodePDF" Runat="server">UNSPSC_English_v13.1201_3.pdf [6MB]</asp:linkbutton>
					</td>
				</tr>
				<tr>
					<td class="emptycol">Download Guide to UNSPSC (Segment or Category by Goods & Services Type) -  
						<asp:linkbutton id="cmdDownloadUNSPSCGuide" Runat="server">GUIDE TO UNSPSC (SEGMENT or CATEGORY BY GOODS & SERVICES TYPE).pdf [36KB]</asp:linkbutton>
					</td>
				</tr>	
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td><asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload"></asp:button>&nbsp;<asp:button id="cmdDownload" runat="server" CssClass="button" Text="Download"></asp:button>&nbsp;
					<asp:Button ID="cmdView" runat="server" Width="70px" CssClass="button" Text="Print Budget" enabled="false"/>&nbsp;&nbsp;&nbsp;&nbsp;
                        <strong><asp:Label ID="lblReportType" runat="server" Text="Report Type :" Visible="false"></asp:Label></strong>&nbsp;
                        <asp:DropDownList ID="cboReportType"  Width="70px" CssClass="ddl" runat="server" Visible="false">
                            <asp:ListItem Selected="True" Value="Excel">Excel</asp:ListItem>
                            <asp:ListItem Value="PDF">PDF</asp:ListItem>
                        </asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</html>
