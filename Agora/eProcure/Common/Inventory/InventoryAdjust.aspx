<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryAdjust.aspx.vb" Inherits="eProcure.InventoryAdjust" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Adjustment</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%>   
		<script language="javascript">
		    function Reset(){
			    var oform = document.forms(0);			    
			    oform.txtItemCode.value = ""
			    oform.txtItemName.value=""	
			    oform.ddl_Loc.selectedIndex=0
			    oform.ddl_SubLoc.selectedIndex=0;  
		    }
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Adj_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="EmptyCol" colSpan="4">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant verified inventory. Please save before going to next page."></asp:label>
				</TD>
			</TR>
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="4">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="104px"></asp:textbox></TD>
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label4" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" Width="104px"></asp:textbox></TD>				
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblLoc" runat="server" Text="Location" CssClass="lbl"></asp:Label><asp:Label ID="Label2" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_Loc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblSubLoc" runat="server" Text="Sub Location" CssClass="lbl"></asp:Label><asp:Label ID="Label3" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_SubLoc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="False"></asp:dropdownlist></TD>				
			</TR>	
			<TR class="tablecol">
			    <TD class="TableCol" style="height: 24px"></TD>
			    <TD class="TableCol" style="height: 24px"></TD>
			    <TD class="TableCol" style="height: 24px"></TD>
			    <TD class="TableCol" style="height: 24px; text-align:right;">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="false"></asp:button>&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
				</TD>
			</TR>
			<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="4">
						    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						        <Columns>						            					             					       
							        <asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
								        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderText="Item Name">
									    <HeaderStyle HorizontalAlign="Left" Width="20%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle HorizontalAlign="Left" Width="15%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
							        <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
								        <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="ID_INVENTORY_QTY" SortExpression="ID_INVENTORY_QTY" HeaderText="Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:TemplateColumn HeaderText="Physical Qty *">
									    <HeaderStyle HorizontalAlign="Right" Width="11%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									    <ItemTemplate>
										    <asp:textbox ID="txtPhysicalQty" Runat="server" cssclass="numerictxtbox" Width="40px"></asp:textbox>
										    <asp:RegularExpressionValidator id="revPhysicalQty" runat="server" ControlToValidate="txtPhysicalQty" Display="Dynamic" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
										    <asp:RequiredFieldValidator ID="reqVal_Qty" runat="server" Display="none" ControlToValidate="txtPhysicalQty"></asp:RequiredFieldValidator>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Remarks *">
									    <HeaderStyle HorizontalAlign="Left" Width="22%" ></HeaderStyle>
									    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									    <ItemTemplate>
										    <asp:textbox ID="txtRemarks" Runat="server" cssclass="txtbox" TextMode="MultiLine"
											    Rows="2" Height="30px" MaxLength="400" style="width: 150px"></asp:textbox>
											<asp:RequiredFieldValidator ID="reqRemarks" runat="server" Display="none" ControlToValidate="txtRemarks"></asp:RequiredFieldValidator>

										    <asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="1%" ForeColor="Red"
											     contentEditable="false" Visible="false" ></asp:TextBox>
										    <INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									    </ItemTemplate>
								    </asp:TemplateColumn>
							        <asp:BoundColumn Visible="False" DataField="ID_INVENTORY_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="ID_LOCATION_INDEX"></asp:BoundColumn>								        
							        
						        </Columns>
					        </asp:datagrid>																	
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"><br>
						<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>
					</TD>
				</TR>	
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
						<INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary" runat="server">
						<INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl" runat="server">
					</TD>
				</TR>			
			</TABLE>
		</form>
	</body>
</HTML>
