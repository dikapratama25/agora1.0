<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryWOItemSearchPopup.aspx.vb" Inherits="eProcure.InventoryWOItemSearchPopup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add Item</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>  
        <% Response.write(Session("typeahead")) %>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("BgiFrame")) %>
        
		<script language="javascript">
		<!--		
		
		    function selectAll()
		    {
			    SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
		    }
					
		    function checkChild(id)
		    {
			    checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
		    }
		
		    function Reset(){
			    var oform = document.forms(0);
			    oform.txtItemName.value="";
			    oform.txtItemCode.value="";
			    form1.ddl_Loc.selectedIndex=0;
			    form1.ddl_SubLoc.selectedIndex=0;
		    }
			
	        function PopWindow(myLoc)
	        {
		        //window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
		        window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
		        return false;
	        }

        -->
        </script>
</HEAD>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
        <TABLE class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
			<TR>
				<TD class="header" colspan="3">
				<asp:label id ="lblHeader" runat="server" Font-Bold="True"  Text="Inventory Item Search"></asp:label>
                </TD>
			</TR>
			<TR>
				<TD class="emptycol" colspan="3">
				</TD>
			</TR>
			<TR>
			<TD class="TableHeader" colSpan="4">Search Criteria</TD>
		    </TR>
			<TR class="tablecol">
			    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
			    <TD class="TableCol" style="height: 18px" width="35%"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="104px"></asp:textbox></TD>
			    <TD class="TableCol" style="height: 18px" width="15%"></TD>
                <TD class="TableCol" style="height: 18px" width="20%"></TD>				
		    </TR>
		    <%--<TR class="tablecol">
			    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblLoc" runat="server" Text="Location :" CssClass="lbl"></asp:Label></strong></TD>
			    <TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_Loc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
			    <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblSubLoc" runat="server" Text="Sub Location :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_SubLoc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>				
		    </TR>--%>	
		    <TR class="tablecol">
		        <TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" Width="350px"></asp:textbox></TD>			
		        <TD class="TableCol" style="height: 24px"></TD>
		        <TD class="TableCol" style="height: 24px; text-align:right;">
			        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" ></asp:button>&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
			    </TD>
		    </TR>

			<TR>
				<TD class="emptycol" colSpan="6" ></TD>
			</TR>
			
            <tr>
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
            <td class="emptycol" colspan="6"><asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
            <Columns>
            <asp:TemplateColumn HeaderText="Delete">
                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderTemplate>
	                <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                </HeaderTemplate>
                <ItemTemplate>
	                <asp:checkbox id="chkSelection" Width="5%" Runat="server"></asp:checkbox>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code"></asp:BoundColumn>
            <asp:BoundColumn DataField="IM_INVENTORY_NAME" HeaderText="Item Name"></asp:BoundColumn>
            <asp:BoundColumn DataField="DOL_LOT_NO" HeaderText="Lot No"></asp:BoundColumn>
            <asp:BoundColumn DataField="LM_LOCATION" HeaderText="Location"></asp:BoundColumn>
            <asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="Sub Location"></asp:BoundColumn>
            <asp:BoundColumn DataField="ID_INVENTORY_QTY" HeaderText="Qty"></asp:BoundColumn>
            </Columns>
            </asp:datagrid></td>
            
            <tr>
            <td class="emptycol" colspan="6">
            <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save" Visible="false"></asp:button>
            <asp:Button ID="cmd_back" runat="server" Text="Close" cssclass="button" />
            </td>
            </tr>
        </TABLE>

    </form>
</body>
</html>