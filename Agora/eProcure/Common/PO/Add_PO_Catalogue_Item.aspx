<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Add_PO_Catalogue_Item.aspx.vb" Inherits="eProcure.Add_PO_Catalogue_Item" ValidateRequest="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add_Catalogue_Item_</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
         <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.write(Session("typeahead")) %>
		
		<script language="javascript">
		<!--
	    
	    $(document).ready(function(){
        
            $("#txtCommodity").autocomplete("<% Response.write(commodity) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });
            $("#txtCommodity").result(function(event, data, formatted) {
            if (data)
            $("#hidCommodity").val(data[1]);
            });
            $("#txtCommodity").blur(function() {
            var hidcommodity = document.getElementById("hidCommodity").value;                        
            if(hidcommodity == "")
            {
                $("#txtCommodity").val("");
            }
            });                       
            });
            
			function selectAll()
			{
				SelectAllG("dtg_Cat_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtg_Cat_ctl02_chkAll","chkSelection");
			}
			

			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
						
			function PopWindow(myLoc)
		    {
			    window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			    return false;
		    }
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
			
				<TR id="hiddtg_freeform" runat="server" class="alltable">
					<TD class="emptycol"><asp:datagrid id="dtg_freeform" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Item Name *">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" Width="367px" TextMode="MultiLine"
											Height="40px" MaxLength="250" Rows="3"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="reqDesc" runat="server" ControlToValidate="txt_desc" Display="None" ></asp:RequiredFieldValidator>--%>
										<asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="UOM *">
									<HeaderStyle Width="25%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" style="width:170px" runat="server" CssClass="ddl"></asp:DropDownList>
										<asp:Label id="lbl_uom" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Commodity Type" Visible="false">
									<HeaderStyle Width="25%"></HeaderStyle>
									<ItemTemplate>
										<%--<asp:DropDownList id="ddl_comm" style="width:200px" runat="server" CssClass="ddl"></asp:DropDownList>--%>
										<asp:textbox id="txtCommodityFree" style="width:170px" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodityFree" runat="server" /></td>
										<asp:Label id="lbl_comm" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
						
				</TR>
				<%--<TR>
					<TD><STRONG>+0 denotes Ex-Stock.</STRONG></TD>
				</TR>--%>
				<TR>
					<TD class="emptycol" style="HEIGHT: 19px"><asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save" ></asp:button>&nbsp;
                        <asp:Button ID="cmd_back" runat="server" CssClass="button" Text="Close" />
					</TD>
				</TR>
				<tr>
					<td class="emptycol"><asp:validationsummary id="ValidationSummary1" runat="server" CssClass="errormsg" Height="24px"></asp:validationsummary><asp:label id="lbl_check" runat="server" ForeColor="Red" CssClass="errormsg"></asp:label></td>
				</tr>

                <%--Jules 2018.08.01--%>
                <tr>
				    <td class="emptycol">
                        <ul class="errormsg" id="vldsum" runat="server"></ul>
				    </td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
