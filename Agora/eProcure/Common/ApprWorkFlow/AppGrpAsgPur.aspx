<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppGrpAsgPur.aspx.vb" Inherits="eProcure.AppGrpAsgPur" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AppGrpAsg</title>
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
			SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
		}
	
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWFAsgPur_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
	                <td colspan="2" style="height: 57px">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br><b>=></b> Step 3: Assign User to the Selected Approval Group."
		                ></asp:label>

	                </td>
                </tr>
                          <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
            <tr>
					<td align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Please select Approval Group Type and the related Approval Group."
						></asp:label>
                        </div>
					</td>
			</tr>
    <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="6" width="100%">&nbsp;Search Criteria</td>
				</tr>
				<tr>
				<tr >
					<td colspan="4">
						<table class="alltable" cellspacing="0" cellpadding="0" border="0" >
							<tr class="tablecol" id="trGrpType" runat="server">
								<td style="WIDTH: 124px">&nbsp;<strong>Group Type</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
								<td colspan="3">&nbsp;<asp:dropdownlist id="cboType" runat="server" CssClass="txtbox" Width="100px" AutoPostBack="True">
										<asp:ListItem Value="PO">PO</asp:ListItem>
										<asp:ListItem Value="PR">PR</asp:ListItem>
										<asp:ListItem Value="E2P">E2P</asp:ListItem>
										<asp:ListItem Value="MRS">eMRS</asp:ListItem>
										<asp:ListItem Value="BIL">Billing</asp:ListItem>
										<asp:ListItem Value="SC">Staff Claim</asp:ListItem>
									</asp:dropdownlist></td>									
							</tr>
							<tr>
								<td class="TableCol" style="WIDTH: 116px"><strong>&nbsp;Approval Group </strong>
									:</td>
								<td class="TableCol">&nbsp;<asp:dropdownlist id="cboGroup" runat="server" AutoPostBack="True" CssClass="txtbox" Width="300px"></asp:dropdownlist>
								</td>
								<td class="TableCol"><strong>&nbsp;<asp:Label ID="lbldept" text="Department :" runat="server"></asp:Label></strong></td>
								<td class="TableCol">
                                    <asp:Label ID="lbldeptname" runat="server" Text=""></asp:Label>
                                </td>
							</tr>
						</table>
					</td>
				</tr>
	</table>
						<div id="Div_AA" style="DISPLAY: none" runat="server">
							<table class="alltable" cellspacing="0" cellpadding="0" border="0">
								<tr>
									<td class="TableCol" style="WIDTH: 314px"><strong><asp:Label ID="lblLeftPanel" text="Available Purchasers " runat="server"></asp:Label></strong></td><%--Available Purchasers </strong>:</td>--%>
									<td class="TableCol" style="WIDTH: 81px"></td>
									<td class="TableCol" style="width: 350px"><strong><asp:Label ID="lblRightPanel" text="Selected Purchasers " runat="server"></asp:Label></strong></td><%--Selected Purchasers </strong>:</td>--%>
								</tr>
								<tr>
									<td class="TableCol" style="WIDTH: 314px">
										<asp:listbox id="lstbox1" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></td>
									<td class="TableCol" style="WIDTH: 81px">
										<P><asp:button id="cmdAdd1" runat="server" CssClass="button" Text="Assign"></asp:button></P>
										<P><asp:button id="cmdAdd2" runat="server" CssClass="button" Text="Remove"></asp:button></P>
									</td>
									<td class="TableCol" style="width: 350px"><asp:listbox id="lstBox2" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></td>
								</tr>
							</table>
							<table class="alltable" cellspacing="0" cellpadding="0" border="0">
								<tr>
									<td class="TableCol" ></td>
								</tr>
								<tr>
									<td><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button></td>
								</tr>
    <tr>
					<td class="linespacing2" colspan="3"></td>
			    </tr>
				<tr id="trhid" runat="server">
					<td align="center" >
						<div align="left"><asp:label id="lblBottomNote" runat="server"  CssClass="lblInfo"
						Text="a) To assign users to the Approval Group, choose the name from 'Available' panel and click Assign button.<br>b) To remove/unassign user from the Approval Group, choose the name from 'Selected' panel and click Remove button."></asp:label>
                        </div>
					</td>
				</tr>
							</table>
						</div>
			<table class="alltable" cellspacing="0" cellpadding="0">
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
