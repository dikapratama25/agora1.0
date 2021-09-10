<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AccCodePop.aspx.vb"
    Inherits="eProcure.AccCodePop" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Add_Catalogue_Item_</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <% Response.Write(Session("JQuery")) %>
    <% Response.Write(Session("AutoComplete")) %>

    <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
    </script>

    <% Response.Write(Session("WheelScript"))%>
    <% Response.write(Session("typeahead")) %>

    <script language="javascript">
		<!--
		    function updateparam(oldstr,newstr1,newstr2)
            {                               
                //return '../../Common/Initial/TypeAhead.aspx?from=BRGL&type=cc&branch=' + newstr;        
                var returnVal = oldstr + '&branch=' + newstr1 + '&glcode=' + newstr2;
                return returnVal;
            }
//            
//			function selectAll()
//			{
//				SelectAllG("dtg_Cat_ctl02_chkAll","chkSelection");
//			}
//					
//			function checkChild(id)
//			{
//				checkChildG(id,"dtg_Cat_ctl02_chkAll","chkSelection");
//			}
			

//			function check(){
//			var change = document.getElementById("onchange");
//			change.value ="1";
//			}
						
//			function PopWindow(myLoc)
//		    {
//			    window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
//			    return false;
//		    }
		-->
    </script>

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%--  <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%"
            border="0">--%>
        <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <td class="tableheader" width="100%" align="left" colspan="6" style="height: 19px">
                    <asp:Label ID="lblHeader" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="EmptyCol" colspan="6">
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Click Save button to save the mapping and Close button to return to the main screen."></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="rowspacing">
                </td>
            </tr>
        </table>
        <div id="divInvDetail" runat="server">
            <% response.write(Session("ConstructTable"))%>
        </div>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%"
            border="0">
            <%--<TR id="hiddtg_accountmapping" runat="server" class="alltable">
					<TD class="emptycol"><asp:datagrid id="dtgaccountmapping" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="From Branch Code">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txtFromBRCode" runat="server" CssClass="txtbox" MaxLength="250"></asp:TextBox>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn DataField="From BR GL Code" SortExpression="From BR GL Code" HeaderText="From BR GL Code"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="From Branch Code">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txtFromBRCode" runat="server" CssClass="txtbox" MaxLength="250"></asp:TextBox>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn DataField="From Cost Center" SortExpression="From Cost Center" HeaderText="From Cost Center"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="From Branch Code">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txtFromCostCenter" runat="server" CssClass="txtbox" MaxLength="250"></asp:TextBox>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn DataField="From Interface Code" SortExpression="From Interface Code" HeaderText="From Interface Code"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="To Branch Code">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txtToBRCode" runat="server" CssClass="txtbox" MaxLength="250"></asp:TextBox>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn DataField="To BR GL Code" SortExpression="To BR GL Code" HeaderText="To BR GL Code"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="To Cost Center">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txtToCostCenter" runat="server" CssClass="txtbox" MaxLength="250"></asp:TextBox>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn DataField="To Interface Code" SortExpression="To Interface Code" HeaderText="To Interface  Code">
                            </asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
						
				</TR>	--%>
            <tr>
                <td class="emptycol" style="height: 19px">
                    <asp:Button ID="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:Button>&nbsp;
                    <asp:Button ID="cmd_back" runat="server" CssClass="button" Text="Close" />
                    <asp:Button ID="btnhidden" runat="server" CssClass="Button" Text="btnhidden" Style="display: none"></asp:Button>
                    <asp:HiddenField ID="hidText" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errormsg"
                        Height="24px"></asp:ValidationSummary>
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" CssClass="errormsg"></asp:Label></td>
            </tr>
        </table>
        <tr>
            <td class="emptycol">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormsg"
                    Height="24px"></asp:ValidationSummary>
                <asp:Label ID="lbl_check" runat="server" ForeColor="Red" CssClass="errormsg"></asp:Label></td>
        </tr>
        </table>
    </form>
</body>
</html>
