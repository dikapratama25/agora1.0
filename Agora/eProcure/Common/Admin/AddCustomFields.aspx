<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddCustomFields.aspx.vb" Inherits="eProcure.AddCustomFields" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CustomFields</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
        <script language="javascript">
		<!--
		function Close()
			{
	            window.close();
	         }
	         -->
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
            <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="tableheader" colspan="3" style="height: 19px" >
                        <asp:Label ID="lblHeader" runat="server" Text="Add Custom Field :"></asp:Label></TD>
				</TR>
				<tr class="tablecol" width="100%">
				<td class="tablecol" width="15%" style="height: 24px">
                    <strong>&nbsp;Module :<asp:Label ID="Label2" runat="server" Text="*" CssClass="errormsg"></asp:Label></strong></TD>
				<td class="TableInput" width="20%" style="height: 24px">
                    <asp:DropDownList ID="ddlModule" cssclass="ddl" runat="server">
                    </asp:DropDownList><input type="hidden" id="hidModule" runat="server" /></td>
                    <td  width="60%" style="height: 24px">
                        </td>
				</tr>
								<tr class="tablecol" width="100%">
				<td class="tablecol" width="15%" style="height: 24px">
                    <strong>&nbsp;Field Name :<asp:Label ID="Label1" runat="server" Text="*" CssClass="errormsg"></asp:Label></strong></TD>
				<td class="TableInput" width="20%" style="height: 24px">
                    <asp:TextBox ID="txtName" runat="server" CssClass="txtbox" MaxLength="20"></asp:TextBox><input type="hidden" id="hidName" runat="server" />
                    </td>
                    <td  width="60%" style="height: 24px">
                        </td>
				</tr>
				<tr class="tablecol" >
				<td class="tablecol" >
                     <strong>&nbsp;Field Value :<asp:Label ID="Label3" runat="server" Text="*" CssClass="errormsg"></asp:Label></strong></TD>
				<td class="TableInput" >
                    <asp:TextBox ID="txtValue" runat="server" CssClass="txtbox" MaxLength="100"></asp:TextBox><input type="hidden" id="hidValue" runat="server" />
                    </td>
                    <td >
                        &nbsp;</td>
				</tr>
										</TABLE>

			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="HEIGHT: 19px">
                        &nbsp;&nbsp;<br />
                        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" />
                        <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="Close()" /></TD>
				</TR>
				<tr>
				<td><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
