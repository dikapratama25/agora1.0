<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AllowanceClaim.aspx.vb" Inherits="eProcure.AllowanceClaim" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AllowanceClaim</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		
		function clearFields()
		{
		    for (i=0; i < Form1.hidItemLine.value; i++)
			{
			    if (document.getElementById('chkSelection' + i).checked == true){
			        $('#txtDateFr' + i).val('');
			        $('#txtDateTo' + i).val('');
			        $('#txtPC' + i).val('');
			        $('#txtPurpose' + i).val('');
			        $('#cboStandby' + i).val('');
			        $('#cboShift' + i).val('');
			    }
			}
		}
        
        function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
		}  
        
        function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
        
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		    <%  Response.Write(Session("w_Staff_Claim_tabs"))%>
			<table  class="alltable"  width="1500px" id="Table1" cellspacing="0" cellpadding="0">
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<%--<tr>
				<td class="emptycol" colspan="5">
					<asp:label id="Label4" runat="server"  CssClass="lblInfo"
					Text="Please select staff claim form from drop-down list. (e.g. Hardship Claim Form...)"></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="linespacing2" colspan="5"></td>
			</tr>--%>
			<tr style="display:none;">
			    <td class="emptycol" colspan="5">
					<asp:dropdownlist id="ddlSelect" runat="server" Width="160px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true"></asp:dropdownlist>
			    </td>
			</tr>
			<%  Response.Write(Session("w_SC_Links"))%>
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr id="trInfo" runat="server">
				<td class="emptycol" colspan="5">
					<asp:label id="Label1" runat="server"  CssClass="lblInfo"
					Text="Click Save button to save record and Add line button to add a new row."></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>Staff Claim No </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblScNo" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Status </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblStatus" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>User Name </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblUserName" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Company </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblCompName" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px;" width="18%">&nbsp;<strong>Business Division/Dept. </strong>:</td>				
				<td class="tablecol" style="height:19px;" width="25%"><asp:Label id="lblDeptId" runat="server" Visible="false"></asp:Label><asp:Label id="lblDept" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" width="8%"></td>
				<td class="tablecol" width="12%"><strong>Document Date </strong>:</td>
				<td class="tablecol" width="37%"><asp:Label id="lblDocDate" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
			    <td class="emptycol" colspan="5"></td>
			</tr>
			</table>
			<% Response.Write(Session("ConstructTableAllowance")) %>
			<table  class="alltable"  width="1500px" id="Table2" cellspacing="0" cellpadding="0">
			<tr>
			    <td class="linespacing1"></td>
			</tr>	
			<tr>
				<td class="EmptyCol"><br/>
				    <asp:button id="cmdAddClaim" runat="server" CssClass="button" Text="Add Line"></asp:button>
				    <input class="button" id="cmdClear" onclick="clearFields()" runat="server" type="button" value="Clear Line" name="cmdClear"/> 
				    <asp:button id="cmdDupLine" runat="server" width="90px" CssClass="button" Text="Duplicate Line"></asp:button>
					<asp:button id="cmdSave" runat="server" width="90px" CssClass="button" Text="Save & Next"></asp:button>
					<asp:button id="cmdSaveSummary" runat="server" width="110px" CssClass="button" Text="Save & Summary"></asp:button>&nbsp;
					<input id="hidItemLine" type="hidden" name="hidItemLine" runat="server" />
					</td>
			</tr>
			<tr>
				<td class="linespacing1"></td>
			</tr>
			<tr>
				<td class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			</tr>
			</table>
		</form>
	</body>
</html>
