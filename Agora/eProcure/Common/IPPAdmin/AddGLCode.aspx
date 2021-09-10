<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddGLCode.aspx.vb" Inherits="eProcure.AddGLCode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GL Code Maintenance</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            'Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            
       </script>
       <% Response.Write(Session("JQuery")) %>	

       <%-- Response.Write(css)--%>  
       <% Response.Write(Session("WheelScript"))%>
     
       
		<script language="javascript">
        window.onunload = function(){ window.opener.reloadPage(); };
		</script>
	</HEAD>
	<body onload="" class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td class="Header" colSpan="2"><asp:label id="lblTitle" runat="server" Text="Add GL Code"></asp:label></TD>
				</tr>
				<td class="header" colspan="4"></td>
				<tr>
					<td class = "emptycol" colspan = "6">
					    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="Click Save button to save record and Add button to add new GL Code"></asp:label>					    
					</td>
				</tr>
				<td class="header" colspan="4"></td>
				<tr>
					<td class="TableHeader" colSpan="2"><asp:label id="lblHeader" runat="server" Text="GL Code" CssClass="lbl"></asp:label>
					</td>
				</tr>
				<tr>
				
				<TR class="tablecol" id="tr1" vAlign="top" runat="server">
					<TD class="tablecol" width="14%">
					    <STRONG>GL Code</STRONG>
					    <asp:label id="Label6" runat="server" CssClass="errormsg" Text="*"></asp:label>:					  
					    
					</TD>
					<TD class="tablecol" width="70%">
					    <asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="150px" Rows="1" MaxLength="30"></asp:textbox>
					    <asp:requiredfieldvalidator id="revCode" runat="server" ControlToValidate="txtCode" ErrorMessage="GL Code is Required" Display="None"></asp:requiredfieldvalidator>
					</TD>
				</TR>
				<tr class="tablecol" id="trDesc" vAlign="top" runat="server">
					<td class="tablecol" width="14%">
					    <strong>GL Name</STRONG>
					    <asp:label id="Label8" runat="server" CssClass="errormsg" Text="*"></asp:label>:
					    
					</td>
			        <td class="tablecol" width="70%">
			            <asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="300px" MaxLength="100"></asp:textbox>
			            <asp:requiredfieldvalidator id="revDesc" runat="server" ControlToValidate="txtDesc" ErrorMessage="Description is required."
					        Display="None"></asp:requiredfieldvalidator>
			        </td>
				</tr>
				<tr class="tablecol" id="trCC" vAlign="top" runat="server" visible = "false">
					<td class="tablecol" width="14%">
					    <strong>Cost Center Require?</STRONG>
				        <asp:label id="Label10" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				        					    
					</td>
					<td class="tablecol" width="70%">
                        <asp:RadioButtonList ID="rdbCC" CssClass="rbtn" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                    </TD>
				</tr>
				<tr class="tablecol" id="trAG" vAlign="top" runat="server" visible="false">
					<td class="tablecol" width="14%">
					    <STRONG>Asset Group Require?</STRONG>
				        <asp:label id="lblAssetGrpReq" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				          					    
					</td>
					<td class="tablecol" width="70%">
                        <asp:RadioButtonList ID="rdbAG" CssClass="rbtn" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                    </TD>
				</tr>
				<tr class="tablecol" id="trStatus" vAlign="top" runat="server">
					<td class="tablecol" width="14%" style="height: 24px">
					    <STRONG>Status</STRONG>
				        <asp:label id="Label14" runat="server" CssClass="errormsg" Text="*"></asp:label>:
				           					    
					</td>
					<td class="tablecol" width="70%" style="height: 24px">
                        <asp:RadioButtonList ID="rdbStatus" CssClass="rbtn" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Selected="True" Value="A">Active</asp:ListItem>
                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                        </asp:RadioButtonList>
                    </TD>
				</TR>
				<tr>
					<TD class="EmptyCol" colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
				</TR>
				<td class="header" style="height: 7px" colspan="4"></td>  
				<tr>
					<TD class="EmptyCol" colSpan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>&nbsp;
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>
					  
					</TD>
				</TR>
				<tr><td class="rowspacing" colSpan="2"></td></tr>	  
				<TR>
					<TD class="emptycol" colSpan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>