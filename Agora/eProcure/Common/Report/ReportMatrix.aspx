<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportMatrix.aspx.vb" Inherits="eProcure.ReportMatrix" %>
<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Report Matrix</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="VBScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script> 
        <% Response.Write(css)%>
	    <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
		function Reset(){
			var oform = document.forms(0);					
			oform.txtUserName.value="";
			oform.txtUserID.value="";
			oform.txtRpt.value="";
		}
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="Report Matrix"></asp:label></td>
				</tr>
				<tr><td class="rowspacing"></td></tr>
				<tr>
	                <td class="EmptyCol" colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant Report Matrix information. Click the Assign button to assign Report to User/User to Report. Click on the function icon for editing/deleting."></asp:label>
	                </td>
                </tr>
                <tr><td class="rowspacing"></td></tr>
				<tr>
					<td class="emptycol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" colspan="10">&nbsp;Search Criteria</td>
							</tr>							
							<tr>
								<td class="tablecol" colspan="7"><strong>&nbsp;<asp:Label ID="Label5" runat="server"  Width="10%" Text="Assign By :"  CssClass="lbl" ></asp:Label></strong>
									<asp:radiobutton id="rdUser" GroupName="Ass" Width="70" Checked="true" CssClass="Rbtn" Text="User"
										Runat="server" AutoPostBack="true"></asp:radiobutton><asp:radiobutton id="rdRpt" GroupName="Ass" CssClass="Rbtn" Text="Report" Runat="server"
										AutoPostBack="true"></asp:radiobutton></td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol" style="HEIGHT: 7px" width="100%"></td>
							</tr>
						</table>
						<div id="tbUser" runat="server" >
							<table  colspan="7" class="alltable" cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr>
									<td class="tablecol" Width="10%"><strong>&nbsp;<asp:Label ID="Label8" runat="server" Text="User Name"  CssClass="lbl" ></asp:Label><asp:Label ID="Label9" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td> 
				                    <td class="TableCol" Width="20%"><asp:textbox id="txtUserName" runat="server" Width="100%"  CssClass="txtbox" ></asp:textbox></td>
									<td class="tablecol" Width="5%"></td>
									<td class="tablecol" Width="8%"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="User ID" CssClass="lbl" ></asp:Label><asp:Label ID="Label2" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td> 
				                    <td class="TableCol" Width="22%"><asp:textbox id="txtUserID" runat="server" Width="100%"  CssClass="txtbox"></asp:textbox></td>
									<td class="tablecol" Width="3%"></td>
									<td class="tablecol" Width="20%" align="right">
										<asp:button id="cmdSearchUser" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
										<input class="button" id="cmdClearUser" onclick="Reset();" type="button" value="Clear"
											name="cmdClear"/>&nbsp;
									</td>
								</tr>
							<tr class="tablecol">
								<td class="tablecol" style="HEIGHT: 7px" colspan="10"></td>
							</tr>
							</table>
						</div>
						<div id="tbRpt" style="DISPLAY: none" runat="server">
							<table class="alltable" cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr>
									<td class="tablecol" Width="10%"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Report Name"  CssClass="lbl" ></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td> 
				                    <td class="TableCol" Width="45%"><asp:textbox id="txtRpt" runat="server" Width="100%"  CssClass="txtbox"></asp:textbox></td>
				                    <td class="tablecol" Width="13%"></td>
									<td class="tablecol" Width="20%" align="right">
										<asp:button id="cmdSearchRpt" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
										<input class="button" id="cmdClearAcc" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;
								    </td>
								</tr>
							<tr class="tablecol">
								<td class="tablecol" style="HEIGHT: 7px" colspan="10"></td>
							</tr>
							</table>
						</div>
					</td>
				</tr>				
				<tr>
					<td class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></td>
				</tr>
				<tr id="Rpt" runat="server">
					<td class="emptycol"><asp:datagrid id="dtgRpt" runat="server" OnPageIndexChanged="PageIndexChanged" OnItemCreated="ItemCreated">
							<Columns>
								<asp:BoundColumn DataField="RM_REPORT_NAME" HeaderText="Report">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="450px"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="User Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Wrap="False" Width="180px"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptUserName" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "UM_USER_NAME")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="User ID">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle Width="150px"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptUserID" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "UM_USER_ID")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='modify'&gt;"
									CommandName="Modify" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Remove'&gt;"
									CommandName="Delete" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
								<asp:BoundColumn DataField="RM_REPORT_INDEX" HeaderText="Report" Visible="false">
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr id="Usr" runat="server">
					<td class="emptycol"><asp:datagrid id="dtgUser" runat="server" OnPageIndexChanged="PageIndexChanged" OnItemCreated="ItemCreated">
							<Columns>
								<asp:BoundColumn DataField="UM_USER_NAME" HeaderText="User Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="100px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_USER_ID" HeaderText="User ID">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Width="100px"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Assigned Report">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="sub" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "RM_REPORT_NAME")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='modify'&gt;"
									CommandName="Modify" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Remove'&gt;"
									CommandName="Delete" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
		
				<tr>
					<td class="emptycol"><asp:button id="cmdAssign" runat="server" CssClass="Button" Text="Assign"></asp:button></td>
				</tr>
			</table>
		</form>
	</body>
</html>
