<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQAddVendList.aspx.vb" Inherits="eProcure.RFQAddVendListFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Add Vendor List</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>
        <% Response.Write(Session("AutoComplete")) %>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%> 		
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("typeahead"))%>
		
		<script language="javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
		}
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="margin-top:0px;">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
			<tr>
			<td class="header" colspan="2">
                <asp:Label ID="lbltitle" runat="server" Text="Add Vendor List"></asp:Label>
			</td>
			</tr>
			<TR>
				<TD class="EmptyCol" colspan="2">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="To add or modify vendor list, type the company name to choose an option and click Save button. Click Add Line button for new line."
					></asp:label>

				</TD>
			</TR>
			<tr>
			<td class="tableheader" colspan="2">
                <asp:Label ID="lblHeader" runat="server" Text="Add Vendor List"></asp:Label>
			</td>
			</tr>
			<tr>
			<td class="TableCol" style="height: 24px">
                <asp:Label ID="Label1" runat="server" Text="List Name" CssClass="lbl"></asp:Label><asp:Label
                    ID="Label2" runat="server" CssClass="errormsg" Text="*"></asp:Label><asp:Label ID="Label3"
                        runat="server" CssClass="lbl" Text=":"></asp:Label></td>
			<td class="TableCol" style="height: 24px">
                <asp:TextBox ID="txtName" runat="server" CssClass="txtbox"></asp:TextBox>
                <asp:TextBox ID="hidName" runat="server" style="display:none;" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqtxtName" runat="server" ErrorMessage="Require List Name." ControlToValidate="txtName" Display="None" EnableClientScript="False"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="reqCharLimit" runat="server"  ControlToValidate="txtName" Display="None" EnableClientScript="False" ValidationExpression="^.{1,50}$"
                    ErrorMessage="List name must not exceed 50 character."></asp:RegularExpressionValidator></td>
			</tr>
			<tr>
			<td class="EmptyCol" colspan="2">
			<% response.write(Session("ConstructTable"))%>
			</td>
			</tr>
			<tr><td class="rowspacing"></td></tr>
			<tr>
			<td class="EmptyCol" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" />
                <asp:Button ID="btnAddLine" runat="server" Text="Add Line" CssClass="button" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" />
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" />
			</td>
			</tr>
			<tr>
			<td class="EmptyCol">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
			</td>
			</tr>
			</TABLE> 
		</form>
	</body>
</HTML>
