<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnalysisCodeSearch.aspx.vb" Inherits="eProcure.AnalysisCodeSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Analysis Code Selection</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
    </script>
    <script language="javascript">

        function SelectOneOnly(objRadioButton, grdName, AnalysisCode, GLDesc) {

            var i, obj;
            for (i = 0; i < document.all.length; i++) {
                obj = document.all(i);

                if (obj.type == "radio") {
                    if (objRadioButton.id.substr(0, grdName.length) == grdName)
                        if (objRadioButton.id == obj.id)
                            obj.checked = true;
                        else
                            obj.checked = false;
                }
            }

            document.Form1.hidAnalysisCode.value = AnalysisCode + ":" + GLDesc.replace("'", "");
        }
        function selectOne() {
            var AnalysisCode = document.Form1.hidAnalysisCode.value;
            window.opener.document.getElementById(document.Form1.hidopenerID.value).value = AnalysisCode;
            window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = AnalysisCode;
            window.opener.document.getElementById(document.Form1.hidopenerID.value).focus();
            opener.updatebtnURL(AnalysisCode, document.Form1.hidopenerHIDID.value, document.Form1.hidopenerbtn.value, "AnalysisCodeSearch.aspx?", "AnalysisCodeSearch.aspx?id");
            window.close();
        }

        function Select() {
            var val, v;
            var oform = window.opener.document.Form1;
            var j;

            v = Form1.txtValue.value;

            re = new RegExp(':' + val + '$');

            for (var i = 0; i < oform.elements.length; i++) {
                var e = oform.elements[i];

                if (e.type == "text" && e.readOnly == false && e.id.indexOf("txtAnalysisCode") != -1) {
                    e.value = v;
                }
            }
            window.close();
        }

        function Reset() {
            var oform = document.forms(0);
            var a = document.getElementById('txtAnalysisCode');
            if (a) {
                a.value = "";
            }
            var b = document.getElementById('txtAnalysisCodeDesc');
            if (b) {
                b.value = "";
            }
        }
    </script>
</head>
<%--<body>--%>
<body class="body" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table class="AllTable" id="Table1" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td class="rowspacing" colspan="3">
                    <asp:Label ID="lblScreenName" runat="server" Text="Select Analysis Code" CssClass="Header"></asp:Label></td>
            </tr>
            <tr>
                <td class="EmptyCol" colspan="3"></td>
            </tr>
            <tr>
                <td class="TableHeader" colspan="3">Search Criteria
                </td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 23%; height: 24px;"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Code :" CssClass="lbl" Font-Bold="True"></asp:Label></strong>&nbsp;</td>
                <td class="TableCol" style="width: 300px; height: 24px;">&nbsp;<asp:TextBox ID="txtAnalysisCode" runat="server" Width="160px" MaxLength="30" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="TableCol" style="width: 100%; height: 24px;"></td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 23%"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Description :" CssClass="lbl" Font-Bold="True"></asp:Label></strong>&nbsp;</td>
                <td class="TableCol" style="width: 300px">&nbsp;<asp:TextBox ID="txtAnalysisCodeDesc" runat="server" Width="225px" MaxLength="100" CssClass="txtbox"></asp:TextBox>
                </td>
                <td class="TableCol" align="right" style="width: 100%">
                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:Button>&nbsp;
					    <input type="button" class="button" id="cmdClear" runat="server" onclick="Reset();" value="Clear" name="cmdClear">&nbsp;
                </td>
            </tr>
            <tr>
                <td class="rowspacing" colspan="3"></td>
            </tr>
            <tr>
                <td colspan="2" class="TableCol" style="background: none; height: 19px;">
                    <td class="TableCol" colspan="1" style="background: none transparent scroll repeat 0% 0%; width: 100px; height: 19px;"></td>
            </tr>

        </table>

        <table class="AllTable" id="table2" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td class="EmptyCol">
                    <div id="AnalysisCode" style="display: none" runat="server">
                        <div class="rowspacing"></div>
                        <asp:DataGrid ID="dtgAnalysisCode" runat="server">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton GroupName="rbl" ID="rbtnSelection" runat="server" AutoPostBack="false" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn Visible="False">
                                    <HeaderStyle Width="5%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="AC_ANALYSIS_CODE" SortExpression="AC_ANALYSIS_CODE" HeaderText="Code">
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="AC_ANALYSIS_CODE_DESC" SortExpression="AC_ANALYSIS_CODE_DESC" HeaderText="Description">
                                    <HeaderStyle Width="25%"></HeaderStyle>
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                    <div id="NoRecord" style="display: none" runat="server">
                        <div class="rowspacing"></div>
                        <div class="db_wrapper">
                            <div class="db_tbl_hd" style="background-color: #D1E1EF">
                                <asp:Label ID="lblAnalysisCode" runat="server" Text="Analysis Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>
                                <asp:Label ID="lblAnalysisCodeDesc" runat="server" Text="Analysis Name" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>
                            </div>
                            <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>

                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td class="EmptyCol">
                    <asp:Button ID="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="False"></asp:Button>
                    <asp:Button ID="btnHidden1" CausesValidation="false" runat="server" Style="display: none"></asp:Button>
                </td>
            </tr>
        </table>
        <input type="hidden" id="hidAnalysisCode" name="hidAnalysisCode" runat="server" />
        <input type="hidden" id="hidopenerID" name="hidopenerID" runat="server" />
        <input type="hidden" id="hidopenerHIDID" name="hidopenerHIDID" runat="server" />
        <input type="hidden" id="hidopenerbtn" name="hidopenerbtn" runat="server" />
        <input type="hidden" id="hidopenerValID" name="hidopenerValID" runat="server" />
    </form>
</body>
</html>
