<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MRSDetail.aspx.vb" Inherits="eProcure.MRSDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>MRS Detail</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("JQuery")) %>	
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim bubblepopupcss As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "jquery.bubblepopup.v2.3.1.css") & """ rel='stylesheet' type='text/css'>"
            Dim bubblepopupjquery As String = "<script type=""text/javascript"" src="""& dDispatcher.direct("Plugins/include","jquery.bubblepopup.v2.3.1.min.js") &""">"            
        </script>
       
        <% Response.Write(Session("WheelScript"))%>
        <% Response.Write(bubblepopupcss)%>
		<% Response.Write(Session("JQuery")) %>
		<% Response.Write(bubblepopupjquery) %>
		<% Response.Write("</script>") %>
        
		<script type="text/javascript">
		
		$(document).ready(function(){
        <%  Response.Write(Session("jqPopup")) %>
            
       });
       
		    function PopWindow(myLoc)
	        {
		        window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		        return false;
	        }
		</script>
	</head>
	<body class="body">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_MRSDetail_tabs"))%>
			<table  class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%">
			<tr>
			    <td colspan="5" class="linespacing1"></td></tr>
			<tr>
			<tr>
			    <td colspan="5" class="rowspacing"></td></tr>
			<tr>
			    <td class="TableHeader" colspan="5">MRS Header</td>
		    </tr>			
			<tr>
				<td class="tablecol" align="left" width="20%" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >MRS Number</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="20%" style="height: 19px"><asp:label id="lblMRSNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="15%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label5" runat="server" >Status</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="20%" style="height: 19px"><asp:label id="lblStatus" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="25%"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label4" runat="server" >MRS Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblMRSDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label7" runat="server" >MRS Issued Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblIssuedDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label8" runat="server" >Requestor Name</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblRequestor" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >Section</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblSection" runat="server" Width="100%"></asp:label></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="lblIssue" runat="server" >Issue To</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblIssueTo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="lblDept" runat="server" >Department</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblDepartment" runat="server" Width="100%"></asp:label></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server">Reference No</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" colspan="3" style="height: 19px"><asp:label id="lblRefNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label6" runat="server" >Internal File(s) Attached</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" colspan="3" style="height: 19px"><asp:label id="lblFileInt" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" colspan="3" style="height: 19px"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="emptycol" colspan="5" ></td>
			</tr>
			</table>
			
            <table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%">
            <tr>
				<td class="tableheader">Approval Workflow</td>
			</tr>
			<tr>
				<td class="alltable" cellspacing="0" cellpadding="0" border="0" colspan="6"><asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" Width="100%">
					<Columns>
						<asp:BoundColumn DataField="IRA_SEQ" HeaderText="Level">
							<HeaderStyle Width="2%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
							<HeaderStyle Width="20%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
							<HeaderStyle Width="20%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_APPROVAL_TYPE" HeaderText="Approval Type">
							<HeaderStyle Width="10%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_ACTION_DATE" HeaderText="Action Date">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_AO_REMARK" HeaderText="Remarks">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Attachment">
							<HeaderStyle Width="20%"></HeaderStyle>
							<ItemTemplate>
						        <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "IRA_AO") , DataBinder.Eval( Container.DataItem, "IRA_A_AO") , DataBinder.Eval( Container.DataItem, "IRA_SEQ" ), DataBinder.Eval( Container.DataItem, "TB" ) ) %>' ShowFooter="False" Width="100%" BorderColor="#0000ff" ShowHeader="False">
								    <ItemTemplate>
									    <%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
							        </ItemTemplate>
								</asp:DataList>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></td>
			</tr>
			<tr>
				<td class="emptycol" style="height: 19px"></td>
			</tr>
			<tr>
				<td class="tableheader">MRS Line Detail</td>
			</tr>
			<tr>
            
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
                <td class="EmptyCol" colspan="6">
                <asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
                    <Columns>            
                        <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code">
                            <HeaderStyle Width="14%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRSD_INVENTORY_NAME" HeaderText="Item Name">
                            <HeaderStyle Width="14%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRSD_UOM" HeaderText="UOM">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Status" Visible="false">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRSD_IRS_MTHISSUE" HeaderText="Monthly Stock Issued Accumulative">
                            <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRSD_IRS_LAST3MTH" HeaderText="Last 3 Mths Ave">
                            <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>   
                        <asp:BoundColumn DataField="IRSD_QTY" HeaderText="Qty">
                            <HeaderStyle HorizontalAlign="Right" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>
                </asp:datagrid>
                </td>
            </tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%">
			    <tr>
				    <td class="tablecol" style="height: 19px" colspan="3"></td>
			    </tr>
			    <tr id="tr_IssueRemark" runat="server">
				    <td class="tablecol" width="15%" align="left" style="height: 19px">&nbsp;<strong>Issued Remark</strong>&nbsp;:</td>
				    <td class="tablecol" width="65%" style="height: 19px"><asp:textbox id="txtIssueRemark" Runat="server" Width="500px" TextMode="MultiLine" MaxLength="900"
										Rows="3" CssClass="listtxtbox" ReadOnly="true"></asp:textbox></td>
				    <td class="tablecol" width="20%"></td>
			    </tr>
			    <tr id="tr_AckRemark" runat="server">
				    <td class="tablecol" width="15%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label12" runat="server" >Acknowledged/ <br/> Cancelled Remark</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="65%" style="height: 19px"><asp:textbox id="txtAckRemark" Runat="server" Width="500px" TextMode="MultiLine" MaxLength="900"
										Rows="3" CssClass="listtxtbox" ReadOnly="true"></asp:textbox></td>
				    <td class="tablecol" width="20%"></td>
			    </tr>
			</table> 
			<tr>
				<td class="EmptyCol"><br/>
					<input type="button" value="View" id="cmdPrint" runat="server" class="button" style="width: 50px"/>
					
			    </td>
			</tr>
     		<div class="emptycol"></div>
			<div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><strong>&lt; Back</strong></asp:hyperlink></div>
			
			
		</form>
	</body>
</html>
