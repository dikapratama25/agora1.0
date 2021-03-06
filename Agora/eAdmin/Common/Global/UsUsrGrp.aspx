<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UsUsrGrp.aspx.vb" Inherits="eAdmin.usUsrGrp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>UserGroup</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>        
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("WheelScript"))%>
        
        <script language="javascript">
		<!--
			function selectAll()
			{
				SelectAllG("dgAR__ctl2_chkAll","chkSelection");
			}
						
			function checkChild(id)
			{
				checkChildG(id,"dgAR__ctl2_chkAll","chkSelection");
			}
						
		function checking()
	{ 

		var myClassName = event.srcElement.parentElement.className;
		//alert(myClassName);

	//defunct checkbox was clicked...
	if(myClassName == "defunct")	
	{
		var myRow = getParent(event.srcElement, "TR")
		var myTbl = getParent(event.srcElement, "TABLE")
		var cnt = myTbl.rows.length;
		var i;	
	
		//event on group...
		if(myRow.className == "group")
		{	//alert(myRow.className);
			var chkbox = getChildren(myRow, "INPUT");

			if(chkbox.checked == false)
			{
				uncheckRow(myRow);
					
				var grpName = myRow.children(1).innerText;	
				//alert(grpName);	
				var child;
				var row;
				for(i=0; i<cnt; i++)
				{
					row = myTbl.rows(i)
					
					if(row.className == grpName)
					{	//alert("in" + row.className);	
						uncheckRow(row);
					}
				}
			}
			
			if(chkbox.checked == true)
			{
				checkRow(myRow);
				var grpName = myRow.children(1).innerText;
				var child;
				var row;
				for(i=0; i<cnt; i++)
				{
					row = myTbl.rows(i)
					
					if(row.className == grpName)
					{
						checkRow(row);
					}
				}
			}
		}
		//event on link...
		//if link is checked, auto check its group...
		else
		{
			var chkbox = getChildren(myRow, "INPUT");
			if(chkbox.checked == true)
			{
				checkRow(myRow);
				var grpName = myRow.className;
				//alert(grpName);
				//var grpName = myRow.children(1).innerText;	
				var child;
				for(i=0; i<cnt; i++)
				{
					row = myTbl.rows(i)
					//checkRow(row);
					if(row.children(1).innerText == grpName)
					{
						child = getChildren(row, "INPUT");
						child.checked = true;
						//alert(chkbox.name);
						checkRow(row);
					}
				}
			}
			else
			{
				uncheckRow(myRow);
			}
		}
	}

	//other checkbox was clicked...
	//if the defunct checkbox was not checked, cancel event...
	else
	{	
		var obj = getParent(event.srcElement, "TR")
		var chkbox = getChildren(obj, "INPUT");
		if(chkbox.checked == false)
			event.srcElement.checked = false;
	}
}

function selectAll()
{
	var oform = document.form1;
	var num = 0;
	
}

function checkRow(row)
{
	child = getChildren(row.children(0), "INPUT");
	child.checked = true;
	child = getChildren(row.children(2), "INPUT");
	child.checked = true;
	child = getChildren(row.children(3), "INPUT");
	child.checked = true;
	child = getChildren(row.children(4), "INPUT");
	child.checked = true;
	child = getChildren(row.children(5), "INPUT");
	child.checked = true;
}

function uncheckRow(row)
{	
	var child;

	child = getChildren(row.children(0), "INPUT");
	child.checked = false;
	child = getChildren(row.children(2), "INPUT");
	child.checked = false;
	child = getChildren(row.children(3), "INPUT");
	child.checked = false;
	child = getChildren(row.children(4), "INPUT");
	child.checked = false;
	child = getChildren(row.children(5), "INPUT");
	child.checked = false;
}

function getParent(obj, tag)
{
	while(obj.tagName != tag)
		obj = obj.parentElement;
	return obj;
}

function getChildren(obj, tag)
{
	while(obj.tagName != tag)
		obj = obj.children(0);
	return obj;
}

	function rolechangedsaveconfirm() {
		var rolechangedstatus;
		
		if (Page_ClientValidate() == false) return false;

		if (document.getElementById("cboRole").value != document.getElementById("txtOriRole").value) {
			return confirm('Are you sure that you want to change the user role?\nAll the previous access rights will be gone.');
		}
		return true
	}

	function onchange() {
		var blnChanged = false;
		
		if (document.getElementById("txUsrGrpName").value != document.getElementById("txOriUsrGrpName").value) {
			blnChanged = true;
		} else if (document.getElementById("cboRole").value != document.getElementById("txtOriRole").value) {
			blnChanged = true;
		} else if (document.getElementById("cboType").value != document.getElementById("txtOriType").value) {
			blnChanged = true;
		}
		
		if (blnChanged == true) {
			document.getElementById("cmdGrant").disabled = true;
			document.getElementById("onchange").value = "1";
		} else {
			document.getElementById("cmdGrant").disabled = false;
			document.getElementById("onchange").value = "0";
		}
	}
	
	function CustomReset() {
		ValidatorReset();
		document.getElementById("cmdGrant").disabled = false;
		document.getElementById("onchange").value = "0";
	}
-->		
		</script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			<INPUT id="rolechangedstatus" type="hidden" value="false" name="rolechangedstatus" runat="server">
			<INPUT id="originalrole" type="hidden" name="originalrole" runat="server"> <INPUT id="onchange" type="hidden" value="0" name="onchange" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="3"><FONT size="3">User&nbsp;Group Details&nbsp;Maintenance</FONT></TD>
				</TR>
				<tr>
					<td colSpan="3">&nbsp;</td>
				</tr>
				<TR>
					<TD class="tableheader" colSpan="3">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				<tr class="tablecol">
					<TD width="160"><STRONG>&nbsp;User Group&nbsp;ID</STRONG><span class="errorMsg">*</span>&nbsp; 
						:</TD>
					<TD width="160"><asp:textbox id="txtUsrGrpId" runat="server" Enabled="False" CssClass="txtbox" MaxLength="30"
							Width="160px"></asp:textbox></TD>
					<TD class="tablecol"><asp:requiredfieldvalidator id="rfv_txtUsrGrpId" runat="server" ControlToValidate="txtUsrGrpId" ErrorMessage="User group Id is required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 2px"><STRONG>&nbsp;User Group Name</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD style="HEIGHT: 2px"><asp:textbox id="txUsrGrpName" runat="server" CssClass="txtbox" MaxLength="100" Width="160px"></asp:textbox><asp:textbox id="txOriUsrGrpName" style="DISPLAY: none" runat="server" Width="0"></asp:textbox></TD>
					<TD class="tablecol" style="HEIGHT: 2px"><asp:requiredfieldvalidator id="rfv_txUsrGrpName" runat="server" ControlToValidate="txUsrGrpName" ErrorMessage="User group name is required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<TR class="tablecol">
					<TD height="23">&nbsp;<STRONG>Application&nbsp;</STRONG><STRONG>Package</STRONG><span class="errorMsg">*</span>&nbsp;:</TD>
					<TD><asp:dropdownlist id="cboPackage" runat="server" CssClass="ddl" Width="160px" AutoPostBack="True"></asp:dropdownlist></TD>
					<TD><asp:requiredfieldvalidator id="rfv_cboPackage" runat="server" ControlToValidate="cboPackage" ErrorMessage="Application Package is required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<tr class="tablecol">
					<td height="23"><STRONG>&nbsp;Role</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD><asp:dropdownlist id="cboRole" CssClass="ddl" Width="160" Runat="server">
							<asp:ListItem Value="" Selected="True">---Select---</asp:ListItem>
						</asp:dropdownlist><asp:textbox id="txtOriRole" style="DISPLAY: none" runat="server" Width="0"></asp:textbox></TD>
					<td><asp:requiredfieldvalidator id="rfv_cboRole" runat="server" ControlToValidate="cboRole" ErrorMessage="Role is required"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr class="tablecol">
					<td height="23"><STRONG>&nbsp;Type</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD><asp:dropdownlist id="cboType" CssClass="ddl" Width="160" Runat="server">
							<asp:ListItem Value="" Selected="True">---Select---</asp:ListItem>
							<asp:ListItem Value="BUYER">Buyer</asp:ListItem>
							<asp:ListItem Value="HUB">Hub</asp:ListItem>
							<asp:ListItem Value="VENDOR">Vendor</asp:ListItem>
						</asp:dropdownlist><asp:textbox id="txtOriType" style="DISPLAY: none" runat="server" Width="0"></asp:textbox></TD>
					<td><asp:requiredfieldvalidator id="rfv_cboType" runat="server" ControlToValidate="cboType" ErrorMessage="Type is required"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<TR class="tablecol">
					<TD style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				<TR class="emptycol">
					<TD style="HEIGHT: 7px"><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
					<td style="HEIGHT: 7px" colSpan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
			<div id="tblButton" runat="server">
				<table class="alltable" id="tbltable4" cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdGrant" runat="server" CssClass="button" Width="120" Text="Grant Access Rights"
								Visible="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="button" value="Clear" name="cmdReset" runat="server"></TD>
					</TR>
				</table>
			</div>
			<table class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD><asp:datagrid id="dgAR" runat="server">
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle Width="20px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" onclick="checking()" CssClass ="defunct" checked='<%# IsDefunct(DataBinder.Eval(Container.DataItem, "UAR_DELETE_IND"))%>' ID="Checkbox1" >
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="MM_MENU_NAME" HeaderText="Screen Name">
									<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="MM_MENU_PARENT" HeaderText="Menu Parent">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="MM_MENU_LEVEL" HeaderText="Menu Level"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="MM_MENU_ID" HeaderText="Menu">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Allow &lt;BR&gt; Add">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" checked='<%# IsCheck(DataBinder.Eval(Container.DataItem, "UAR_ALLOW_INSERT"))%>' onclick="checking()" ID="ChkInsert">
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Allow &lt;BR&gt; Update">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" checked='<%# IsCheck(DataBinder.Eval(Container.DataItem, "UAR_ALLOW_UPDATE"))%>' onclick="checking()" ID="ChkUpdate">
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Allow &lt;BR&gt; Delete">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" checked='<%# IsCheck(DataBinder.Eval(Container.DataItem, "UAR_ALLOW_DELETE"))%>' onclick="checking()" ID="ChkDelete">
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Allow &lt;BR&gt; View">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" checked='<%# IsCheck(DataBinder.Eval(Container.DataItem, "UAR_ALLOW_VIEW"))%>' onclick="checking()" ID="ChkView">
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<tr>
					<td><asp:button id="cmdSaveAR" runat="server" CssClass="button" Text="Save" Visible="False"></asp:button>&nbsp;<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" Visible="False"></asp:button></td>
				</tr>
				</tr>
				<tr>
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary></TD>
				</TR>
				<TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
						</TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
