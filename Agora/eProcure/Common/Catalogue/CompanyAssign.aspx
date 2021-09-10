<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CompanyAssign.aspx.vb" Inherits="eProcure.CompanyAssign" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
           Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
 
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
			
			function CheckDeleteMaster(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				itemCnt = 0;
				itemCheckCnt = 0;
				
				re = new RegExp(':' + pChkSelName + '$');  //generated control name starts with a colon	
				for (var i=0;i<oform.elements.length;i++){
					var e = oform.elements[i];
					if (e.type=="checkbox"){						
						if (re.test(e.name)){
							itemCnt ++;
							if (e.checked==true)
								itemCheckCnt ++;
						}
					}
				}
				
				if (itemCheckCnt == 0) {
					alert ('Please make at least one selection!');
					return false;
				}
				else{
					if (itemCnt == itemCheckCnt) {
						var result = confirm('Are you sure that you want to permanently delete this item(s) ?');
						if (result == true){
							var result2 = confirm('Delete Master record too ?');
							if (result2 == true) 
								Form1.hidDelete.value = "1";								
							else
								Form1.hidDelete.value = "0";
							return true;
						}
						else
							return false;
					}
					else {
						Form1.hidDelete.value = "0";
						return confirm('Are you sure that you want to permanently delete this item(s) ?');
					}
				}				
			}

		-->
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="4"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="4">&nbsp;Discount&nbsp;Group Header</TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Discount Group Code</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="TableInput" width="30%"><asp:label id="lblCode" runat="server"></asp:label></TD>
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="40%"><asp:label id="lblDesc" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblStartDate" runat="server"></asp:label><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" noWrap>&nbsp;<STRONG>End Date</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblEndDate" runat="server"></asp:label><% Response.Write(calEndDate)%></TD>
				</TR>
				<TR>
					<TD colSpan="4">
						<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 218px">&nbsp;<STRONG>Available Buyer Company</STRONG>&nbsp;:</TD>
								<TD class="TableCol" style="WIDTH: 81px"></TD>
								<TD class="TableCol"><STRONG>Selected Buyer Company</STRONG>&nbsp;:</TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 218px">&nbsp;
									<asp:listbox id="lstBuyer" runat="server" cssClass="listbox" SelectionMode="Multiple" Width="210px"></asp:listbox>
								<TD class="TableCol" style="WIDTH: 81px" align="center">
									<P><asp:button id="cmdAdd" runat="server" CssClass="button" Text=">>"></asp:button></P>
									<P><asp:button id="cmdRemove" runat="server" CssClass="button" Text="<<"></asp:button></P>
								</TD>
								<TD class="TableCol"><asp:listbox id="lstBuyerSelected" runat="server" cssClass="listbox" SelectionMode="Multiple"
										Width="210px"></asp:listbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
						<asp:button id="cmdItem" runat="server" CssClass="Button" Width="120px" Text="Discount Group Item"></asp:button>
						<asp:button id="cmdReset" runat="server" CssClass="Button" Text="Reset"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
