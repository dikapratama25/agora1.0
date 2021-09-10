<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MaterialGroup.aspx.vb" Inherits="eProcure.CMaterialGroup"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Category</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<!--#include file = "../include/WheelScript.js"-->
		<script language="javascript">
		<!--
		
			function Reset()
			{
				var oform = document.forms(0);
				oform.txtItemId.value="";
				oform.txtDesc.value="";				
			}
			
			function checkSearchCriteria()
			{
				var oform = document.forms(0);
				var strName, strDesc;
				strName = oform.txtItemId.value;
				strDesc = oform.txtDesc.value;				
				
				if (strName == '' && strDesc == ''){
					alert('No search criteria specified.');
					return false;
				}
				else
					return true;
			}

			function Select()
			{	
				var r =eval("window.opener.document.Form1.txtCategory");
				var s =eval("window.opener.document.Form1.hidCategoryId");
				var strCat, i, strB1, strB2;
				strB1 = '<B><I>';
				strB2 = '</I></B>';
				strCat = Form1.hidCat.value;
				i = strCat.indexOf(' ');
				strCat = strCat.substring(i+1, strCat.length);
				strCat = strCat.replace(strB1, '');
				strCat = strCat.replace(strB2, '');
				
				//r.value = Form1.hidCat.value;				
				r.value = strCat;
				s.value = Form1.hidCatId.value;
				
				if (Form1.hidCat.value == '')
					alert('Please select a category.');
				else 
					window.close();
			}
						
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="5"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label>
							<asp:Label id="lblTest" runat="server" Visible="False"></asp:Label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="5">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="5">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" noWrap width="5%">&nbsp;<STRONG>Material Group Code </STRONG>&nbsp;:</TD>
					<TD class="tablecol" noWrap width="15%"><asp:textbox id="txtItemId" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" noWrap width="5%">&nbsp;<STRONG>Material Desc.</STRONG>&nbsp;:</TD>
					<TD class="tablecol" noWrap width="20%"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></TD>
					<TD class="tablecol" noWrap width="55%"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="5">&nbsp;You can either search by Material Group Code 
						or any part of the description.<BR>
						&nbsp;Otherwise, you can click on the respective category headers to drill 
						down.
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="5">&nbsp;</TD>
				</TR>
				<tr>
					<td colSpan="5"><iewc:treeview id="tvCategory" runat="server" DefaultStyle="font-size: 8pt;font-family: Verdana;background:white;color:black"
							HoverStyle="background:transparent;color:black;font-weight:bold" SelectedStyle="background:white;color:black;font-weight:bold"
							ShowPlus="False" autopostback="true" ShowToolTip="False"></iewc:treeview></td>
				</tr>
				<TR>
					<TD colSpan="5"><INPUT class="button" id="cmdSelect" onclick="Select();" type="button" value="Save" name="cmdSelect"
							runat="server">&nbsp;<INPUT class="button" id="cmdClose" onclick="window.close();" type="button" value="Close">&nbsp;&nbsp;<INPUT id="hidCat" type="hidden" name="hidCat" runat="server"><INPUT id="hidId" type="hidden" name="hidId" runat="server"><INPUT id="hidCatId" type="hidden" name="hidCatId" runat="server"><INPUT id="hidSelected" type="hidden" name="hidSelected" runat="server"></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
