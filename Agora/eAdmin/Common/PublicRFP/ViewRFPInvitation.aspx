<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewRFPInvitation.aspx.vb" Inherits="eAdmin.ViewRFPInvitation"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ViewRFPInvitation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="../include/eRFPCDClock.js"></script>
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" />"
        </script>
        <% Response.Write(sCSS)%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;View RFP Invitation<INPUT id="timeNow" type="hidden" runat="server"><INPUT id="CloseTime" type="hidden" runat="server"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;RFP Information</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Reference</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblRFPRef" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Title</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblRFPTitle" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Description</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblDesc" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Procurement Procedure</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblProcProcedure" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Approach</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblApproach" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Form Of Tender Type</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblFOTType" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Fee Currency</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblCurrency" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Fee</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblFee" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Earnest Money</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblEarnestMoney" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" id="EAR" style="WIDTH: 35.62%" runat="server">&nbsp;<STRONG>Earnest 
										Money Amount</STRONG>:</TD>
								<TD class="TableInput" id="EARI" colSpan="3" runat="server">&nbsp;&nbsp;<asp:label id="lblEarnestAmount" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;RFP Payment Mode</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblPaymentMode" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Company Name</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblCoyName" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Calling Entity</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblCallingEntity" runat="server" Height="60%" Width="50%"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%" valign="top"><STRONG>&nbsp;Awarding Entity</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblAwardingEnt" runat="server" Height="60%" Width="50%"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%" valign="top"><STRONG>&nbsp;Collection Point 
										For RFP Document</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblCollec" runat="server" Height="60%" Width="50%"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%" valign="top"><STRONG>&nbsp;Comment</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblComment" runat="server" Height="60%" Width="50%"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Contact Person</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblContactPerson" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Document Publication Date</STRONG>:</TD>
								<TD class="TableInput" style="WIDTH: 20%">&nbsp;
									<asp:label id="lblDocPubDate" runat="server"></asp:label></TD>
								<TD class="TableCol" style="WIDTH: 10%"><STRONG>&nbsp;Time</STRONG>:</TD>
								<TD class="TableInput">&nbsp;<asp:label id="lblDocPubTime" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Opening Date</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:label id="lblOpenDate" runat="server"></asp:label></TD>
								<TD class="TableCol" style="WIDTH: 10%"><STRONG>&nbsp;Time</STRONG>:</TD>
								<TD class="TableInput">&nbsp;<asp:label id="lblOpenTime" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="title" id="brieft" runat="server"><asp:label id="lblRFPBriefingInfo" runat="server" Visible="False">RFP BRIEFING INFORMATION</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgBrief" runat="server" OnPageIndexChanged="OnPageIndexChanged_Page" AutoGenerateColumns="False"
							CssClass="GRID">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn SortExpression="RB_SDATE" HeaderText="Date">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RB_SDATE" HeaderText="Time">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RB_VENUE" SortExpression="RB_VENUE" HeaderText="Venue">
									<HeaderStyle Width="50%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="rfpsub" runat="server"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;RFP Submission</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%" id="CD" runat="server">&nbsp;<STRONG>Count 
										Down</STRONG>:</TD>
								<TD class="TableInput" style="WIDTH: 20%" colSpan="3" id="CDI" runat="server"><script language="javascript"> Countdown(Form1.CloseTime.value,Form1.timeNow.value,''); </script>
									&nbsp;&nbsp;</TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Closing Date</STRONG>:</TD>
								<TD class="TableInput" style="WIDTH: 20%">
									<P>&nbsp;
										<asp:label id="lblCloseDate" runat="server"></asp:label></P>
								</TD>
								<TD class="TableCol" style="WIDTH: 10%"><STRONG>&nbsp;Time</STRONG>:</TD>
								<td class="TableInput" style="WIDTH: 34.38%">&nbsp;
									<asp:label id="lblCloseTime" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD class="TableCol" id="EXT" style="WIDTH: 35.62%" runat="server">&nbsp;<STRONG>Revised 
										Extension Date</STRONG>:</TD>
								<TD class="TableInput" id="EXTI" style="WIDTH: 20%" runat="server">&nbsp;
									<asp:label id="lblExtDate" runat="server" Visible="False"></asp:label></TD>
								<TD class="TableCol" id="EXTT" style="WIDTH: 10%" runat="server"><STRONG>&nbsp;Time</STRONG>:</TD>
								<td class="TableInput" id="EXTTI" runat="server">&nbsp;
									<asp:label id="lblExtTime" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 35.62%"><STRONG>&nbsp;Submission Location</STRONG>:</TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:label id="lblSubLocation" runat="server" Height="60%" Width="50%"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" id="ee" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="TITLE" runat="server"><asp:label id="lblEligibility" runat="server" Visible="False">ELIGIBILITY CRITERIA</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgEligibility" runat="server" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RQ_REG_AUTHORITY" SortExpression="RQ_REG_AUTHORITY" HeaderText="Registration Authority"></asp:BoundColumn>
								<asp:BoundColumn DataField="RQ_CLASSIFICATION" SortExpression="RQ_CLASSIFICATION" HeaderText="Classification"></asp:BoundColumn>
								<asp:BoundColumn DataField="RQ_GRADE_ID" SortExpression="RQ_GRADE_ID" HeaderText="Grade"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="HID" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="CONTYPE" runat="server"><asp:label id="lblFOTCONT" runat="server" Visible="False">FORM OF TENDER (CONTRACT TYPE)</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgFOTCONT" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RS_RFP_LINE" HeaderText="S/N">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" HeaderText="Contract Type">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RS_FOT_DESC" HeaderText="Description">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RS_UOM" HeaderText="UOM">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RS_QUANTITY" HeaderText="Qty">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RS_REMARKS" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="CAT" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="CATTYPE" runat="server"><asp:label id="lblFOTCAT" runat="server" Visible="False">FORM OF TENDER (CATALOGUE TYPE)</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgFOTCAT" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RS_RFP_LINE" HeaderText="S/N"></asp:BoundColumn>
								<asp:BoundColumn DataField="RS_FOT_DESC" HeaderText="Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="RS_UOM" HeaderText="UOM"></asp:BoundColumn>
								<asp:BoundColumn DataField="RS_QUANTITY" HeaderText="Qty"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="TECH" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="TECHDOC" runat="server"><asp:label id="lblTechnical" runat="server" Visible="False">TECHNICAL DOCUMENTS</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgTechnical" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RD_DOC_NAME" HeaderText="File Name">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_DOC_DESC" HeaderText="Description">
									<HeaderStyle Width="70%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="FIN" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="FINDOC" runat="server"><asp:label id="lblFinancial" runat="server" Visible="False">FINANCIAL DOCUMENTS</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgFinancial" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RD_DOC_NAME" HeaderText="File Name">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_DOC_DESC" HeaderText="Description">
									<HeaderStyle Width="70%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="DR" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="DRDOC" runat="server"><asp:label id="lblDrawings" runat="server" Visible="False">RFP DRAWINGS</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgDrawings" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RD_DOC_NAME" HeaderText="File Name">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_DOC_DESC" HeaderText="Description">
									<HeaderStyle Width="70%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="COMP" runat="server"></TD>
				</TR>
				<TR>
					<TD class="title" id="COMDOC" runat="server"><asp:label id="lblCompliance" runat="server" Visible="False">COMPLIANCE LIST</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgCompliance" runat="server" Visible="False" OnPageIndexChanged="OnPageIndexChanged_Page"
							AutoGenerateColumns="False" CssClass="grid">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="RD_DOC_NAME" HeaderText="File Name">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_DOC_DESC" HeaderText="Description">
									<HeaderStyle Width="70%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="II" runat="server"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;
						<asp:label id="lblInvitationInfo" runat="server">Invitation Information</asp:label></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table5" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 27.59%">&nbsp;<STRONG>Invited By</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:label id="lblUserId" runat="server"></asp:label>&nbsp;of
									<asp:label id="lblCompanyName" runat="server"></asp:label>&nbsp;on
									<asp:label id="lblDate" runat="server"></asp:label>&nbsp;at
									<asp:label id="lblTime" runat="server"></asp:label></TD>
							</TR>
							<tr>
								<TD class="TableCol" style="WIDTH: 27.59%">&nbsp;<STRONG>Status</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:label id="lblStatus" runat="server"></asp:label></TD>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="IA" runat="server"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" id="lblInvitationAccept1" runat="server">&nbsp;
						<asp:label id="lblInvitationAccept" runat="server" Visible="False">Invitation Acceptance</asp:label></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD class="TableCol" id="Accb" style="WIDTH: 27.59%" runat="server">&nbsp;<STRONG>Accepted 
										By</STRONG>:</TD>
								<TD class="TableInput" id="Acci" runat="server">&nbsp;
									<asp:label id="lblAccUserID" runat="server"></asp:label>&nbsp;of
									<asp:label id="lblAccCompanyName" runat="server"></asp:label>&nbsp;on
									<asp:label id="lblAccDate" runat="server"></asp:label>&nbsp;at
									<asp:label id="lblAccTime" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="TableCol" id="Rejb" style="WIDTH: 27.59%" runat="server">&nbsp;<STRONG>Rejected 
										By</STRONG>:</TD>
								<TD class="TableInput" id="Reji" runat="server">&nbsp;
									<asp:label id="lblRejUserID" runat="server"></asp:label>&nbsp;of
									<asp:label id="lblRejCompanyName" runat="server"></asp:label>&nbsp;on
									<asp:label id="lblRejDate" runat="server"></asp:label>&nbsp;at
									<asp:label id="lblRejTime" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="RI" runat="server"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" id="lblResponseInfo1" runat="server">&nbsp;
						<asp:label id="lblResponseInfo" runat="server" Visible="False">Response Information</asp:label></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table7" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD class="TableCol" style="WIDTH: 27.59%">&nbsp;<STRONG>Responded By</STRONG>:</TD>
								<TD class="TableInput">&nbsp;
									<asp:label id="lblResUserID" runat="server"></asp:label>&nbsp;of
									<asp:label id="lblResCompanyName" runat="server"></asp:label>&nbsp;on
									<asp:label id="lblResDate" runat="server"></asp:label>&nbsp;at
									<asp:label id="lblResTime" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="display1" runat="server"></TD>
				</TR>
				<TR>
					<TD id="lbl3" runat="server"><asp:label id="lbl_display" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="display2" runat="server"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table6" runat="server" cellSpacing="0" cellPadding="0" width="300"
							border="0">
							<TR>
								<TD style="WIDTH: 50%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:button id="cmdAgree" runat="server" Width="96px" CssClass="button" Text="Agree"></asp:button></TD>
								<TD style="WIDTH: 50%">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
									<asp:button id="cmdNotAgree" runat="server" Width="96px" CssClass="button" Text="Not Agree"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD align="center" id="nores" runat="server"><asp:label id="lblNoResponseDetails" runat="server" Visible="False" Font-Size="Larger" Font-Bold="True">No Response details available for this RFP</asp:label></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="EMP" runat="server"></TD>
				</TR>
				<TR>
					<TD id="buttonline" runat="server">
						<asp:button id="cmdCreateResponse" runat="server" Width="137px" Visible="False" CssClass="button"
							Text="Create / Modify Response"></asp:button>
						<asp:button id="cmdViewClarification" runat="server" Width="104px" Visible="False" CssClass="button"
							Text="View Clarification"></asp:button>
						<asp:button id="cmdCreateQuery" runat="server" Width="83px" Visible="False" CssClass="button"
							Text="Create Query"></asp:button>
						<asp:button id="cmdViewResponse" runat="server" Width="96px" Visible="False" CssClass="button"
							Text="View Response"></asp:button>
						<asp:button id="cmdUpdateFOT" runat="server" Width="126px" Visible="False" CssClass="button"
							Text="Update Form Of Tender"></asp:button>
						<asp:button id="cmdViewAward" runat="server" Width="77px" Visible="False" CssClass="button"
							Text="View Award"></asp:button></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" id="bac" runat="server"></TD>
				</TR>
				<TR>
					<TD><A id="back" href="#" runat="server"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
