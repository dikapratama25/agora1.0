<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddGRN1.aspx.vb" Inherits="eProcure.AddGRN1FTN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>AddGRN</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            Dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
        </script> 
        <% Response.Write(Session("JQuery")) %>        
        <% Response.Write(Session("WheelScript"))%>
        <script language="javascript">
		<!--
		
		$(document).ready(function(){
        $('#cmdSubmit').click(function() {   
        document.getElementById("cmdSubmit").style.display= "none";
        });
        });
		
		
		

        function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
				
		function ResetDG()
		{
			var oform = document.forms[0];
			re = new RegExp('dtg')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				//alert(e.name);
				//alert(e.type);
				//alert(re.test(e.name));
				if (e.type=="text" && re.test(e.name))
					e.value=0;
				if (e.type=="textarea" && re.test(e.name))
					e.value="";
			}			
		}				
		function clear()
		{
			validatorReset();
			Form1.document.getElementById("vldSumm").style.display = "inline";	
		}
		
		
		function PopWindow(myLoc)
        {
            window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
            return false;
        }
        
        function ShowDialog(filename,height)
		{
			
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
			//retval=window.open(filename);
			if (retval == "1" || retval =="" || retval==null)
			{  
			    window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
		}
//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGRN_tabs"))%>
			<div id="DivGrn" style="DISPLAY: inline" runat="server">
				<TABLE class="alltable" id="tblGrnHeader" cellSpacing="0" cellPadding="0" border="0" style="width: 100%">
					<tr>
					<TD class="linespacing1" colSpan="5"></TD>
			        </TR>					
					
					<TR>
	                    <TD colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Click the Loc button to change the default location and Submit button to issue GRN."
		                    ></asp:label>

	                    </TD>
                    </TR>
                    
                    <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			        </TR>
                    
					<TR>
						<TD class="tableheader" colSpan="5">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
					</TR>
										
                    <tr>
					    <TD class="tablecol" style="height: 17px; width:22%;"><strong>&nbsp;<asp:Label ID="Label3" runat="server">Vendor :</asp:Label></strong></TD>
						<TD class="tablecol" colspan="2" style="height: 17px; width:35%;" ><asp:label id="lblVendor" runat="server"></asp:label></TD>
						
						<TD class="tablecol" colspan="2" style="height: 17px; width:38%;" >
			        </TR>
					<TR>
						<TD class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:Label ID="Label2" runat="server">PO Number :</asp:Label></strong></TD>
						<TD class="tablecol" style="height: 17px;" ><asp:label id="lblPONo" runat="server"></asp:label></TD>
						<TD class="tablecol" align="left" style="height: 17px;">
                            <strong><asp:Label ID="Label4" runat="server" >DO Number :</asp:Label><strong></TD>
                        <TD class="tablecol" colspan="2" style="height: 17px;" ><asp:label id="lblDONum" runat="server"></asp:label></TD>
					</TR>	
					<TR>
						<TD class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:Label ID="lblDL" runat="server">Default Location :</asp:Label></strong></TD>
						<TD class="tablecol" style="height: 17px;" ><asp:label id="lblDefaultLocation" runat="server"></asp:label></TD>
						<TD class="tablecol" align="left" style="height: 17px;">
                            <strong><asp:Label ID="lblSDL" runat="server" >Default Sub Location :</asp:Label><strong></TD>
                        <TD class="tablecol" colspan="2" style="height: 17px;" ><asp:label id="lblDefaultSubLocation" runat="server"></asp:label></TD>
					</TR>				
					<TR>
					    <TD class="tablecol" style="height: 17px;"><strong>&nbsp;<asp:label id="lblActDt" runat="server">Actual Goods Received Date</asp:label></strong><strong>&nbsp;<asp:label id="Label5" runat="server">:</asp:label></strong></TD>
						<TD	 class="tablecol" style="height: 17px;"><asp:textbox id="txtReceivedDate" runat="server" Width="60px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtReceivedDate');" href="javascript:;"><%Response.Write(CalPicture) %></td>						
						<TD class="tablecol" style="height: 17px;"></TD>
						<TD class="tablecol" colspan="2" style="height: 17px;" ></TD>
					</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;<asp:label id="lblExtAttach" text="DO File(s) Attached" runat="server"></asp:label></STRONG> :</TD>
					<TD class="tableinput" vAlign="top" colSpan="3">&nbsp;<asp:label id="lblFileAttach" runat="server"></asp:label></TD>
					<TD class="tableinput"></TD>
				</TR>
				</TABLE>
			</div>
			<div id="DODtl" style="DISPLAY: none" runat="server">
				<table width="100%">
					<TR>
						<TD align="right">&nbsp;&nbsp;<%--<STRONG>Date Received</STRONG>--%> &nbsp;
							<asp:label id="txtReceivedDateBak" runat="server" Visible="False"></asp:label><!--<A onclick="popCalendar('txtReceivedDate');" href="javascript:;"></A>--></TD>
					</TR>
					<TR>
						<TD><asp:datagrid id="dtgGRNDtl" runat="server"  OnItemCommand="dtgGRNDtl_ItemCommand"  OnSortCommand="SortCommand_Click" AllowPaging="false">
								<Columns>
									<asp:BoundColumn DataField="POD_Po_Line" SortExpression="POD_Po_Line" HeaderText="Line">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Line" SortExpression="LineNo"></asp:TemplateColumn>
									<asp:BoundColumn DataField="POD_B_ITEM_CODE" SortExpression="POD_B_ITEM_CODE" HeaderText="Buyer Item Code"
										visible="false">
										<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Vendor_Item_Code" SortExpression="POD_Vendor_Item_Code" HeaderText="Item Code">
										<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Item Name">
										<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
										<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
										<HeaderStyle HorizontalAlign="Left" Width="1%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Ordered_Qty" SortExpression="POD_Ordered_Qty" HeaderText="Order Qty">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="POD_Outstanding" SortExpression="POD_Outstanding" HeaderText="Outstanding">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Receive Qty">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GD_REJECTED_QTY" SortExpression="GD_REJECTED_QTY" HeaderText="Reject Qty (1st Level)">
										<HeaderStyle HorizontalAlign="Left" Width="1%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Reject Qty">
										<HeaderStyle HorizontalAlign="Left" Width="6%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
										<ItemTemplate>
											<asp:textbox id="txtReject" Width="62px" CssClass="numerictxtbox" Runat="server"></asp:textbox>
										<asp:RegularExpressionValidator id="rev_qtycancel" runat="server"></asp:RegularExpressionValidator>
										</ItemTemplate>
									</asp:TemplateColumn>
																		
									<asp:TemplateColumn HeaderText="Location">
										<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>	
										<ItemTemplate>
											<%--<asp:textbox id="hidSub" Runat="server" style="display:none;"></asp:textbox>--%>
										    <%--<input class="button"  id="cmdSub"  type="button"
											value=">" name="cmdSub" style="WIDTH: 15px; HEIGHT: 22px" runat="server" />--%>
											<%--<asp:LinkButton CssClass ="button" id="lnkImage" runat="server" >delete</asp:LinkButton>--%>
											<center><asp:Button CssClass ="button" style="WIDTH: 26px; HEIGHT: 20px" Text = "Set" id="cmdSub" runat="server" ></asp:Button></center>
											
										</ItemTemplate>									
									</asp:TemplateColumn>
									
									<asp:TemplateColumn HeaderText="Remarks">
										<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
										<ItemTemplate>
											<asp:textbox ID="txtDtlRemarks" Runat="server" cssclass="txtbox" Width="150px" TextMode="MultiLine"
												Rows="2" Height="30px" MaxLength="400"></asp:textbox>
											<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
												 contentEditable="false" Visible="false"></asp:TextBox>
											<INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="DOD_SHIPPED_QTY" HeaderText="Shiped Qty">
										<HeaderStyle HorizontalAlign="Center" Width="5px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="emptycol" style="HEIGHT: 6px">&nbsp;&nbsp;</TD>
					</TR>
				</table>
			</div>
			<div id="PODtl" style="DISPLAY: inline" runat="server">
				<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<td><asp:label id="lblSummPO" runat="server" Visible="False" Font-Bold="True">GRN History For Purchase Order : </asp:label>&nbsp;&nbsp;
							<asp:label id="lblPONum" runat="server" Visible="False"></asp:label></td>
					</tr>
					<tr>
						<td><asp:datagrid id="DtgGRNSumm" runat="server" OnSortCommand="SortCommand_Click" AllowSorting="True">
								<Columns>
									<asp:BoundColumn DataField="GM_CREATED_DATE" SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
										<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
										<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="GM_GRN_No" SortExpression="GM_GRN_No" HeaderText="GRN Number">
										<HeaderStyle HorizontalAlign="Left" Width="28%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Created By">
										<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></td>
					</tr>
					<TR>
						<TD class="emptycol">&nbsp;&nbsp;</TD>
					</TR>
					<TR>
						<TD><asp:button id="cmdAcceptAll" runat="server" CssClass="button" Text="Accept All" Visible="False"
								CausesValidation="False" Enabled="False"></asp:button>&nbsp;
							<asp:button id="cmdRejectAll" runat="server" CssClass="button" Text="Reject All" Visible="False"
								CausesValidation="False" Enabled="False"></asp:button>&nbsp;
							<asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
							<asp:button id="cmdReset" runat="server" CssClass="BUTTON" Text="Clear" CausesValidation="False"></asp:button>&nbsp;
							<INPUT class="button" id="cmdPreviewPO" type="button" value="View PO" name="cmdPreviewPO"  style="Width:70px;"
								runat="server">
							<INPUT type="button" value="View GRN" id="cmdPreviewGRN" runat="server" class="button" style="width: 75px" visible="false">
						</TD>
					</TR>
					<TR>
						<TD class="emptycol" colSpan="4"></TD>
					</TR>
					<tr>
						<td><asp:validationsummary id="VldSumQty" runat="server" CssClass="errormsg"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
								runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
								runat="server">
                            <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></td>
					</tr>
					<TR>
						<TD class="emptycol" colSpan="4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <asp:label id="lbl_check" runat="server" CssClass="errormsg" Width="400px" ForeColor="Red"></asp:label></TD>
					</TR>
				</TABLE>
			</div>
			<div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; Back</STRONG></asp:hyperlink></div>
		</form>
	</body>
</HTML>
