<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppGrpAsgPur.aspx.vb" Inherits="eProcure.AppGrpAsgPurFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AppGrpAsg</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
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
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWFAsgPur_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="2"></TD>
			    </TR>
				<TR>
	                <TD colSpan="2">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br><b>=></b> Step 3: Assign Purchaser to the Selected Approval Group."
		                ></asp:label>

	                </TD>
                </TR>
                          <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
            <tr>
					<TD align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Please select Approval Group Type and the related Approval Group."
						></asp:label>
                        </div>
					</TD>
			</TR>
    <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" colspan="6" width="100%">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
				<TR >
					<TD colspan="4">
						<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0" >
							<TR class="tablecol" id="trGrpType" runat="server">
								<TD style="WIDTH: 124px">&nbsp;<STRONG>Group Type</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<td >&nbsp;<asp:dropdownlist id="cboType" runat="server" CssClass="txtbox" Width="100px" AutoPostBack="True">
										<asp:ListItem Value="PO">PO</asp:ListItem>
										<asp:ListItem Value="PR">PR</asp:ListItem>
									</asp:dropdownlist></td>
									<td></td>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 116px"><STRONG>&nbsp;Approval Group </STRONG>
									:</TD>
								<TD  colspan="3" class="TableCol" >&nbsp;<asp:dropdownlist id="cboGroup" runat="server" AutoPostBack="True" CssClass="txtbox" Width="300px"></asp:dropdownlist>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
	</TABLE>
						<div id="Div_AA" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="TableCol" style="WIDTH: 314px"><STRONG>Available Purchasers </STRONG>:</TD>
									<TD class="TableCol" style="WIDTH: 81px"></TD>
									<TD class="TableCol" style="width: 350px"><STRONG>Selected Purchasers </STRONG>:</TD>
								</TR>
								<TR>
									<TD class="TableCol" style="WIDTH: 314px">
										<asp:listbox id="lstbox1" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></TD>
									<TD class="TableCol" style="WIDTH: 81px">
										<P><asp:button id="cmdAdd1" runat="server" CssClass="button" Text="Assign"></asp:button></P>
										<P><asp:button id="cmdAdd2" runat="server" CssClass="button" Text="Remove"></asp:button></P>
									</TD>
									<TD class="TableCol" style="width: 350px"><asp:listbox id="lstBox2" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></TD>
								</TR>
							</TABLE>
							<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="TableCol" ></TD>
								</TR>
								<TR>
									<TD><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button></TD>
								</TR>
    <tr>
					<TD class="linespacing2" colSpan="3"></TD>
			    </TR>
				<TR id="trhid" runat="server">
					<TD align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) To assign Purchasers to the Approval Group, choose the name from 'Available Purchasers' and click Assign button.<br>b) To remove/unassign purchaser from the Approval Group, choose the 'Selected Purchasers' and click Remove button."></asp:label>
                        </div>
					</TD>
				</TR>
							</TABLE>
						</div>
			<TABLE class="alltable" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
