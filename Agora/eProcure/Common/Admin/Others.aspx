<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Others.aspx.vb" Inherits="eProcure.Others" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Others</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgDel_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgDel_ctl02_chkAll","chkSelection");
		}


		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("Hide_Add1");
				var div_add = document.getElementById("Hide_Add2");
				var div_add = document.getElementById("Hide_Add3");
				
				var cmd_delete = document.getElementById("cmd_delete");
				var hidMode = document.getElementById("hidMode");
				var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				if (check==1)
				{
					cmd_delete.style.display = "none";
					Form1.hidMode.value = 'm';
					add_mod.value='add';
				}
				else if (check==0)
				{
					cmd_delete.style.display = "none";
				}
			}
	
		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
					<td class="linespacing1" colspan="4"></td>
			    </tr>
			    <tr>
				    <td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Please select related setup from drop-down list. (e.g. Delivery Term...)"></asp:label>

				    </td>
			    </tr>
                <tr>
					<td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="tablecol">
					    <strong>Setting </strong>:&nbsp;
					    <asp:dropdownlist id="ddl_Select" runat="server" Width="170px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true" >
					        <asp:ListItem Value="" Selected="True">---Select---</asp:ListItem>
							<asp:ListItem Value="1">Delivery Term</asp:ListItem>
							<asp:ListItem Value="2">Packing Type</asp:ListItem>
							<asp:ListItem Value="3">Section</asp:ListItem>
							<asp:ListItem Value="4">User Assignment (Section)</asp:ListItem>
					    </asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
