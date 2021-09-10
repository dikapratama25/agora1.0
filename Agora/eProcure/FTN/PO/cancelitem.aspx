<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cancelitem.aspx.vb" Inherits="eProcure.cancelitemFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>cancelitem</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery"))%>
		<script language="javascript">
	<!--
	    $(document).ready(function(){	    
            $('#cmd_submit').click(function() {
            if (Page_IsValid)   
            { 
                if (confirmation() == false)
                {		    
		            return false;
		        }
		        else
		        {
                    resetSummary(1,1);
                    document.getElementById("cmd_submit").style.display= "none";
                    return true;
                }
            }
            });            
        });
        
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		
		function confirmation() {
	        var answer = confirm('Are you sure that you want to cancel this PO ?');
	        if (answer){
	            return true;
	        }
	        else{
		        return false;
	        }
        }
	//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_POCancel_tabs"))%>
			<TABLE class="ALLTABLE" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="linespacing1"></TD>
				</TR>			
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the CR Remarks and click the Submit button for PO Cancellation."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR>
					<TD class="tablecol"><STRONG>PO Number </STRONG>:<STRONG> </STRONG>
						<asp:label id="lbl_Po_No" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG>Order Date </STRONG>:
						<asp:label id="lbl_date" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="header" style="height: 3px"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_POList" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="POD_PO_LINE" SortExpression="POD_PO_LINE" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="LineNo"  Visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive Qty" Visible="false">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive Qty">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Reject Qty">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Outstd">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Qty To Cancel">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qtycancel" runat="server" CssClass="numerictxtbox" MaxLength="10"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_qtycancel" runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="rv_qtycancel" runat="server"></asp:RangeValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Wrap="False" Width="100%"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txt_remarkdetail" runat="server" MaxLength="1000" Height="32px" TextMode="MultiLine"
											CssClass="listtxtbox" ></asp:textbox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="1px"
											 contentEditable="false" ></asp:TextBox>
										<INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 45px" vAlign="top">&nbsp;
						<TABLE class="ALLTABLE" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD style="WIDTH: 100px"><STRONG>CR Remarks</STRONG>
									<asp:label id="Label3" runat="server" CssClass="errormsg" Enabled="True">*</asp:label>
									<STRONG> : </STRONG></TD>
								<TD><asp:textbox id="txt_remark" runat="server" Width="432px" CssClass="listtxtbox" TextMode="MultiLine"
										Height="64px" MaxLength="1000"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol" vAlign="top" align="left"><asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmd_fulcancel" runat="server" Width="104px" CssClass="button" Text="Full Cancellation"></asp:button>
					    <asp:button id="cmd_submit" runat="server" Width="56px" CssClass="button" Text="Submit"></asp:button>
					    <asp:button id="cmd_view" runat="server" Width="66px" CssClass="button" Text="View CR" Visible="False"></asp:button>
						<%--<asp:RequiredFieldValidator id="revRemark" runat="server" ControlToValidate="txt_remark" ErrorMessage="CR Remarks is required."	Display="None"></asp:RequiredFieldValidator>--%>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><STRONG>
                        <asp:HyperLink ID="lnkBack" runat="server">
							<STRONG>&lt; Back</STRONG></asp:HyperLink></STRONG></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
