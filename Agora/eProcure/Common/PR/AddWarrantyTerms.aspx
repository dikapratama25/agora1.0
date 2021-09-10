<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddWarrantyTerms.aspx.vb" Inherits="eProcure.AddWarrantyTerms" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Purchase Requisition</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script language="javascript">
		<!--		
			function Select()
			{
				var val, v;
				var oform = window.opener.document.Form1;
				var j;
				
				v = Form1.txtValue.value;
				val = document.Form1.hidID.value;
		
				
				if (v == ''){
					if (Form1.hidType.value == 'E')
						alert("Please enter Delivery Lead Time (days) !");
					else
						alert("Please enter Warranty Terms (months) !");
				}
				else if (isNaN(v)){
					alert("Invalid Input ! Please input a number ! ");
				}
				else{

					re = new RegExp(':' + val + '$'); 

					for (var i=0;i<oform.elements.length;i++)
					{
						var e = oform.elements[i];

						if (e.type=="text" && e.readOnly == false && e.id.indexOf("txtWarranty") != -1){
						//if (e.type=="text" && re.test(e.name) )
							e.value = v;
							}
					}
					window.close();
				}
			}

		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="712" border="0">
				<TR>
					<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="550" border="0">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 206px">&nbsp;<asp:label id="lblValue" runat="server">Label</asp:label></TD>
								<TD class="TableInput" style="HEIGHT: 19px"><asp:textbox id="txtValue" runat="server" CssClass="txtbox" MaxLength="10" Width="112px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<TR>
					<TD align="left"><INPUT class="button" id="cmdSelect" onclick="Select();" type="button" value="Save" name="cmdSelect"
							runat="server"> <INPUT class="button" id="cmdClose" onclick="window.close();" type="button" value="Close"
							name="cmdClose">&nbsp; <INPUT id="hidID" style="WIDTH: 35px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server"> <INPUT id="hidType" style="WIDTH: 35px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
