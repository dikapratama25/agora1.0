<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DebitCreditNoteList.aspx.vb"
    Inherits="eProcure.DebitCreditNoteList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head runat="server">
    <title>DebitCreditNoteList</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <% Response.Write(Session("WheelScript"))%>
</head>
<body class="body" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Debit_Credit_tabs"))%>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%"
            border="0">
            <tr>
                <td class="linespacing1">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Fill in Invoice No and click Raise button to create Debit Note or Credit Note"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="linespacing2">
                </td>
            </tr>
            <tr>
                <td>
                    <table class="AllTable" id="Table2" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td class="TableHeader" colspan="2">
                                &nbsp;Search Criteria</td>
                        </tr>
                        <tr class="tablecol">
                            <td width="12%">
                                &nbsp;<strong>Invoice No.</strong><asp:Label ID="Label1" runat="server" CssClass="errormsg">*</asp:Label>:
                            </td>
                            <td>
                                <asp:TextBox ID="txt_InvNum" runat="server" CssClass="txtbox" Width="140px"></asp:TextBox></td>
                        </tr>
                        <tr class="tablecol">
                            <td class="linespacing1" colspan="2">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="tablecol">
                <td>
                    <asp:Button ID="cmd_RaiseDN" runat="server" CssClass="Button" Width="100px" Text="Raise Debit Note">
                    </asp:Button>&nbsp;
                    <asp:Button ID="cmd_RaiseCN" runat="server" CssClass="Button" Width="100px" Text="Raise Credit Note">
                    </asp:Button>
                </td>
            </tr>         
        </table>
    </form>
</body>
</html>
