<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConCat_AssignUser.aspx.vb" Inherits="eProcure.ConCat_AssignUserFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BuyerCat_AsignBuyer</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ConCat_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Step 1: Create, delete or modify Contract Catalogue.<br>Step 2: Assign item master to Contract Catalogue.<br><b>=></b>Step 3: Assign User to Contract Catalogue."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
			<TR>
	                <TD colSpan="6">
		                <asp:label id="Label1" runat="server"  CssClass="lblInfo"
		                        Text="Please select Contract Catalogue."
		                ></asp:label>

	                </TD>
                </TR>
               <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
						<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" >
							<TR>
								<TD class="tablecol" nowrap style="height: 25px">&nbsp;<STRONG>Contract&nbsp;Catalogue</STRONG> :&nbsp;<asp:DropDownList ID="cboCatalogueBuyer" runat="server" CssClass="txtbox" Width="322px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                   </TD>
							</TR>
			</table>
			</table>
						<div id="Div_AA" style="DISPLAY: inline" runat="server">
							<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="TableCol" style="WIDTH: 314px">&nbsp;<STRONG>&nbsp;Available Purchasers </STRONG>:</TD>
									<TD class="TableCol" style="WIDTH: 81px"></TD>
									<TD class="TableCol" style="width: 353px"><STRONG>Selected Purchasers </STRONG>:</TD>
								</TR>
								<TR>
									<TD class="TableCol" style="WIDTH: 314px">&nbsp;
										<asp:listbox id="lstbox1" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></TD>
									<TD class="TableCol" style="WIDTH: 81px" >
										<P><asp:button id="cmdAdd1" runat="server" CssClass="button" Text="Assign"></asp:button></P>
										<P><asp:button id="cmdAdd2" runat="server" CssClass="button" Text="Remove"></asp:button></P>
									</TD>
									<TD class="TableCol" style="width: 353px"><asp:listbox id="lstBox2" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></TD>
								</TR>
							</TABLE>
							<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="emptycol"></TD>
								</TR>
								<TR>
									<TD><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button></TD>
								</TR>
               <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
							</TABLE>
						</div>
			<TABLE class="alltable" cellSpacing="0" cellPadding="0">
				<TR id="trhid" runat="server">
					<TD align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) To assign Purchasers to the Contract Catalogue, choose the name from 'Available Purchasers' and click Assign button.<br>b) To remove/unassigned purchaser from the Contract Catalogue, choose the 'Selected Purchasers' and click Remove button."></asp:label>
                        </div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
