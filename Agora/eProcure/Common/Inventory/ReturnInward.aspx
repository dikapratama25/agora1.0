<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnInward.aspx.vb" Inherits="eProcure.ReturnInward" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Return Inward</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
		    <% Response.Write(Session("WheelScript"))%>
		    <% Response.Write(Session("JQuery")) %>
            <% Response.Write(Session("AutoComplete")) %>
		    <script type="text/javascript">
		    $(document).ready(function(){
            $('#cmdSubmit').click(function() {
            document.getElementById("cmdSubmit").style.display= "none";
            });
            });
				
		    </script> 
	</HEAD>	
	
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ReturnInward_tabs"))%>
            <TABLE class="alltable" id="TABLE2" cellSpacing="0" cellPadding="0" border="0" style="width: 100%">
                <tr>
			    <TD class="linespacing1" colSpan="5"></TD>
	            </TR>					  			
			    
                
                <tr>
			    <TD class="linespacing2" colSpan="6"></TD>
	            </TR>
                
			    <TR>
				    <TD class="tableheader" colspan="6">&nbsp;<asp:label id="Label7" runat="server">Return Inward Detail</asp:label></TD>
			    </TR>
    								
                <tr >
				<td class="tablecol" width="15%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label9" runat="server" >MRS Number</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="25%" style="height: 19px"><asp:label id="lblMRSNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="15%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >Status</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="25%" style="height: 19px"><asp:label id="lblStatus" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="10%"></td>
				<td class="tablecol" width="10%"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >MRS Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblMRSDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >MRS Issued Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblMRSIssueDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >Requestor Name</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblRequestorName" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label13" runat="server" >Issue To</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblIssueTo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label15" runat="server" >Section</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblSection" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label17" runat="server" >Reference No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblRefNo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label19" runat="server" >Department</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblDept" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
			    </tr>			
			    <tr>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label21" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" colspan="3" style="height: 19px"><asp:textbox id="txtRemark" runat="server" MaxLength="400" CssClass="txtbox" Height="40px" width="500px" TextMode="MultiLine" ReadOnly=true></asp:textbox></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
		    </TABLE>
			
			<table class="AllTable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="5" style="width: 703px"></TD>
				</TR>
				<TR>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtgMRSDtl" runat="server" DataKeyField="IRSD_INVENTORY_INDEX" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSD_INVENTORY_NAME" HeaderText="Item Name">
									<HeaderStyle Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSD_UOM" HeaderText="UOM">
									<HeaderStyle Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOL_LOT_NO" HeaderText="Lot No">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_LOCATION" HeaderText="Loc">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="Sub-Loc">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSL_LOT_QTY" HeaderText="Issued Qty">
									<HeaderStyle Width="13%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSL_LOT_RETURN_QTY" HeaderText="Returned Qty">
									<HeaderStyle Width="14%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSD_RETURN_QTY" HeaderText="Remaining Qty">
									<HeaderStyle Width="14%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								
								<asp:TemplateColumn HeaderText="Return Qty">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txtReject" CssClass="numerictxtbox"  style="WIDTH: 60px" Runat="server"></asp:textbox>
									<asp:RegularExpressionValidator id="rev_qtycancel" runat="server"></asp:RegularExpressionValidator>
									<asp:Label ID="lblItemLine" Runat="server" Visible="false" ></asp:Label>
									<asp:Label ID="lblInvIndex" Runat="server" Visible="false" ></asp:Label>
									<asp:Label ID="lblLocIndex" Runat="server" Visible="false" ></asp:Label>
									<asp:Label ID="lblLotIndex" Runat="server" Visible="false" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:textbox ID="txtDtlRemarks" Runat="server" cssclass="txtbox" TextMode="MultiLine" Rows="2" Height="30px" MaxLength="400"></asp:textbox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red" ReadOnly="true" Visible="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
                    </TD>
				</TR>
			</table> 
	            
		    <table class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">		 				
		    <TR>
			    <TD class="emptycol">&nbsp;&nbsp;</TD>
		    </TR>
		    <TR>
			    <TD class="emptycol">
			    <asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
			    <asp:button id="cmdReset" runat="server" CssClass="BUTTON" Text="Clear" CausesValidation="False"></asp:button>
		    </TD>
		    </tr>
		    <TR>
				<TD class="emptycol" colSpan="4"></TD>
			</TR>
			<tr>
			    <td><asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary></td>
			</tr>
			<TR>
				<TD class="emptycol" colSpan="4">&nbsp;&nbsp;<asp:label id="lbl_check" runat="server" CssClass="errormsg" Width="400px" ForeColor="Red"></asp:label></TD>
			</TR>
		    </table>
		    <div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; Back</STRONG></asp:hyperlink></div>
		</form>
	</body> 
</HTML>
