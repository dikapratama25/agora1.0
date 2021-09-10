<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="WebForm1.aspx.vb" Inherits="eAdmin.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <div>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="TextBox1" ServiceMethod="GetProducts" ServicePath="AutoComplete.asmx" MinimumPrefixLength="1" CompletionSetCount="10"></cc1:AutoCompleteExtender>
        </div>
    </div>
    </form>
</body>
</html>
