<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExchangeRate.aspx.vb" Inherits="eProcure.ExchangeRate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add_Catalogue_Item_</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
<!--
/*
function resetForm()
{
	//Form1.reset();
	alert("aa");
	form1.
	
}

function Test()
{
	if (Page_IsValid)
	//if (IsValid)
	//if (document.Form1.dtg_exrate__ctl2_vldRate.isvalid)
		alert("valid");
	else{
		//Page_IsValid=true;
		alert("not valid");}
}

function clientvalidate()
{
	alert("can!!!!!")
    //var errormsgctrl = document.getElementById(vldRate);
    //errormsgctrl set to invisible;
  //errormsgctrl.visible=false
  
  }
*/
//-->

		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header">Exchange Rate</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;Search Criteria :</TD>
							</TR>
							<TR class="tablecol">
								<TD colSpan="2">&nbsp;<STRONG>Code </STRONG>:<STRONG> </STRONG>
									<asp:textbox id="txt_code" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><STRONG>&nbsp;Description
									</STRONG>:<STRONG> </STRONG>
									<asp:textbox id="txt_description" runat="server" CssClass="txtbox" Width="121px" MaxLength="30"></asp:textbox><STRONG>&nbsp;&nbsp;
									</STRONG>
									<asp:button id="cmd_search" runat="server" CssClass="button" CausesValidation="False" Text="Search"></asp:button><STRONG>&nbsp;
									</STRONG>
									<asp:button id="cmd_clear" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">--></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_exrate" runat="server" OnPageIndexChanged="dtg_exrate_PageIndexChanged"
							OnSortCommand="SortCommand_Click" DataKeyField="CE_CURRENCY_CODE">
							<Columns>
								<asp:BoundColumn DataField="CE_CURRENCY_CODE" SortExpression="CE_CURRENCY_CODE"  readonly="true"    HeaderText="Code">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CODE_DESC" SortExpression="CODE_DESC" HeaderText="Description">
									<HeaderStyle Width="60%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CE_RATE" HeaderText="Rate">
									<HeaderStyle Width="28%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_rate" runat="server" CssClass="numerictxtbox" Width="130px"></asp:TextBox>
										<asp:regularexpressionvalidator id="vldRate" runat="server" Display="Static" ErrorMessage="Rate is over limit/expecting numeric value."
											ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" ControlToValidate="txt_rate"></asp:regularexpressionvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD>&nbsp;<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmd_reset" type="button" value="Reset" runat="server" onclick="ValidatorReset();"></TD>
				</TR>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
