<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryListing.aspx.vb" Inherits="eProcure.InventoryListing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Inventory Listing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")
            Dim typeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")

        </script>
         <% Response.Write(css)%>   
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.Write(Session("BgiFrame")) %>
         
 
		<script language="javascript">
		<!--
		    function PopWindow(myLoc)
		    {
			    window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
		    }
		
		    function Reset(){
			    var oform = document.forms(0);			    
			    oform.txtItemCode.value = ""
			    oform.txtItemName.value=""	
			    oform.ddl_Loc.selectedIndex=0
			    oform.ddl_SubLoc.selectedIndex=0;
//			    oform.rd2.SelectedValue = "N";
//			    oform.rd2.Items.FindByValue("N").Selected = True;   
		    }
		    
		    $(document).ready(function(){
            $("#txtItemCode").autocomplete("<% Response.write(typeahead) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });    
            $("#txtItemName").autocomplete("<% Response.write(typeahead1) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });        
            });
            
            function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable:Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 680px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<TR>
			    <TD class="Header"  colSpan="6"><asp:label id="lblTitle" runat="server">Inventory Listing</asp:label></TD>
		    </TR>
			<tr><td class="rowspacing" colSpan="6"></td></tr>
			<TR>
				<TD class="EmptyCol" colSpan="6">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant inventory."></asp:label>
				</TD>
			</TR>
			<tr><td class="rowspacing"  colSpan="6"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="6">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="180px"></asp:textbox></TD>
				<TD class="TableCol" style="height: 18px"></TD>
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="Label4" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
               <TD class="TableCol" style="height: 18px"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" Width="180px"></asp:textbox></TD>				
 			   <TD class="TableCol" style="height: 18px"></TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px;"><strong><asp:Label ID="lblLoc" runat="server" Text="Location" CssClass="lbl"></asp:Label><asp:Label ID="Label3" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px;"><asp:dropdownlist id="ddl_Loc" runat="server" Width="180px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
				<TD class="TableCol" style="height: 18px" ></TD>
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="lblSubLoc" runat="server" Text="Sub Location" CssClass="lbl"></asp:Label><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px"><asp:dropdownlist id="ddl_SubLoc" runat="server" Width="180px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="False"></asp:dropdownlist></TD>				
				<TD class="TableCol" style="height: 18px"></TD>
			</TR>	
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="18%"><strong><asp:Label ID="Label2" runat="server" Text="Sorted By :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px" width="20%">
				    <asp:RadioButtonList ID="optSort" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="IM_ITEM_CODE">Item Code</asp:ListItem>
                        <asp:ListItem Value="LM_LOCATION">Location</asp:ListItem>
                    </asp:RadioButtonList>				    
			    </TD>
				<TD class="TableCol" style="height: 18px" width="5%"></TD>
			    <TD class="TableCol" style="height: 18px" width="23%"><strong><asp:Label ID="Label26" runat="server" Text="Need QC/Verification :" ></asp:Label></strong></TD>
			    <TD class="TableCol" style="height: 18px" width="20%">
				    <asp:radiobuttonlist ID="rd2" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal" AutoPostBack="true"> 
                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
							<asp:ListItem Value="Y">Yes</asp:ListItem>
						</asp:radiobuttonlist>			    
			    </TD>
				    <TD class="TableCol" style="height: 18px width=15% text-align:right">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="false"></asp:button>&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
				</TD>
			</TR>				
			</TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%">
							<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
					        <Columns>	
					            <asp:TemplateColumn SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
							        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
							        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        <ItemTemplate>
								        <asp:LinkButton ID="lnkItemCode" runat="server" OnCommand="LinkButton_Click"></asp:LinkButton>									        
							        </ItemTemplate>
						        </asp:TemplateColumn>		            					             					       
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
						        <asp:BoundColumn Visible="False" DataField="ID_INVENTORY_INDEX"></asp:BoundColumn>	
						        <asp:BoundColumn Visible="False" DataField="ID_LOCATION_INDEX"></asp:BoundColumn>								        
					        </Columns>
				        </asp:datagrid>																	
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" style="height: 24px; width: 119px;">
					    <INPUT type="button" value="Print Stock Count List" id="cmdPrint" runat="server" Class="button" style="width: 140px">
					</TD>
				</TR>					
			</TABLE>
		</form>
	</body>
</HTML>
