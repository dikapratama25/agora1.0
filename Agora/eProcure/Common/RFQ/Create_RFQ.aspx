<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Create_RFQ.aspx.vb" Inherits="eProcure.Create_RFQ" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Create_RFQ</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim ValCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_validity") & "','cal','width=190,height=165,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"		    
		    Dim ExpCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_exp") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"
        </script>
        <% Response.Write(Session("JQuery")) %>        
        <% Response.Write(Session("WheelScript"))%>        
		<script language="javascript">
		
		$(document).ready(function(){
        $('#cmd_Submit').click(function() {
        if (Page_IsValid)  
        var x=document.getElementById("lblMsg");
        if (x=="") {
            document.getElementById("cmd_Submit").style.display= "none";
            document.getElementById("cmd_save").style.display= "none";
            
            if (document.getElementById("cmd_addcath"))
            { document.getElementById("cmd_addcath").style.display= "none"; }
            if (document.getElementById("cmd_delete"))
            { document.getElementById("cmd_delete").style.display= "none"; }}
        });
        
        $('#cmd_save').click(function() {
        if (Page_IsValid)  
        var x=document.getElementById("lblMsg");
        if (x=="") {        
            document.getElementById("cmd_Submit").style.display= "none";
            document.getElementById("cmd_save").style.display= "none";
        
            if (document.getElementById("cmd_addcath"))
            { document.getElementById("cmd_addcath").style.display= "none"; }
            if (document.getElementById("cmd_delete"))
            { document.getElementById("cmd_delete").style.display= "none"; }}
        });
        });
        
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		
		function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 830px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
			
			function hideButton()
			{
				var formValidate;
				if (typeof(Page_Validators) == "undefined")
					formValidate = true;
				else	
					formValidate = Page_ClientValidate();	
				
				if (formValidate == true){			
					document.getElementById("cmd_save").style.display = 'none';
					Page_IsValid = true;
				}
				else{	
					document.getElementById("lbl_check").innerHTML = '';
					Page_IsValid = false;
				}		
				return Page_IsValid;
				
			}
			
		function selectAll()
			{
				SelectAllG("dgviewitem_ctl01_chkAll","chkSelection");
		}
					
		function checkChild(id)
			{
				checkChildG(id,"dgviewitem_ctl01_chkAll","chkSelection");
			}
			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}

		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CreateRFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0"  width="100" cellPadding="0" border="0" tabindex="30">
				<TR>
					<TD class="header" colSpan="6" style="height: 3px" width="100"></TD>
				</TR>
				<TR>
					<TD class="header"><asp:label id="lbl_title" runat="server">Raise RFQ</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 3px" ></TD>
				</TR>
				<TR>
					<TD align="center">
						<div align="left">Determine the RFQ Option, fill the RFQ Header, set the vendor list and add item to raise your RFQ. You can save the RFQ as a draft copy by pressing the Save button and submit it to the vendor once it is ready by pressing the Submit button.
                        </div>
					</TD>
				</TR>
				<TR class="TABLECOL" >
				<TD class="tablecol" vAlign="middle" width="100%" >&nbsp;
				        <STRONG><asp:Label id="lblRFQOpt" Text="RFQ Option : " runat="server" Width="126px"></asp:Label> </STRONG>
						<asp:radiobuttonlist id="opt_RFQ_option" runat="server" CssClass="Rbtn" Width="243px" RepeatDirection="Horizontal"
						BorderColor="Silver" BorderWidth="1px" BorderStyle="None" RepeatLayout="Flow">
						<asp:ListItem Value="0">Open RFQ</asp:ListItem>
						<asp:ListItem Value="1">Close RFQ</asp:ListItem>
									</asp:radiobuttonlist></TD>
				</TR>

				<TR>
					<TD class="emptycol" style="height: 3px"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 52px">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader" colSpan="5">&nbsp;RFQ Header</TD>
							</TR>
							<TR>
								<TD class="tablecol" width="20%"><STRONG>&nbsp;RFQ Number </STRONG>:</TD>
								<TD class="tablecol" width="40%">
									<asp:label id="lbl_rfq_number" runat="server"></asp:label></TD>
								<TD class="tablecol" width="5%"></TD>
								<TD class="tablecol" width="20%">
								    <STRONG><asp:label id="lblCurr" Text="Currency :" runat="server" ></asp:label></STRONG></TD>
								<TD class="tablecol" width="15%">
									<asp:dropdownlist id="ddl_cur" runat="server" width="80%" CssClass="ddl"></asp:dropdownlist><asp:label id="lbl_cur" runat="server"></asp:label></TD>							
							</TR>
							<TR class="TABLECOL" valign ="top">
								<TD class="tablecol" align="left"  rowspan="2" >&nbsp;<STRONG>RFQ Description </STRONG>
								    <asp:label id="Label1" runat="server" ForeColor="Red">*</asp:label>&nbsp;:
								    <asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="1%" ForeColor="Red" contentEditable="false" Visible="false" ></asp:TextBox>
								</TD> 
								<TD class="tablecol" rowspan="2" colspan="2">
									<asp:textbox id="txt_name" width="300px" runat="server" TextMode="MultiLine" CssClass="txtbox" 
										Height="32px" MaxLength="100"></asp:textbox>
								<TD class="tablecol" rowspan="2" ></TD>
								<TD class="tablecol" rowspan="2" ></TD>
							</TR>
							<tr></tr>
							<tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label4" Text="RFQ Expiry Date : " runat="server"></asp:Label> </strong></TD> 
				                <TD class="tableinput" > 
									<asp:textbox id="txt_exp" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(ExpCalendar)%>
									<asp:comparevalidator id="CompareValidator1" runat="server" ErrorMessage="Expire Date must be earlier than Validity Date."
										ControlToCompare="txt_validity" ControlToValidate="txt_exp" Operator="LessThan" Type="Date">?</asp:comparevalidator><asp:rangevalidator id="rvl_date" runat="server" ErrorMessage="Date must not be earlier than today's date."
										ControlToValidate="txt_exp" Type="Date">?</asp:rangevalidator></TD>
								<TD class="tablecol" ></TD>
				                <TD class="tablecol" ><STRONG><asp:Label id="Label5" Text="Quotation Validity Date : " runat="server"></asp:Label></STRONG></TD> 
								<TD class="tableinput">
									<asp:textbox id="txt_validity" runat="server" CssClass="txtbox"   contentEditable="false" ></asp:textbox><% Response.Write(ValCalendar)%></TD>
							</tr> 
							<tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label6" Text="Contact Person : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
									<asp:textbox id="txt_cont_person" width="80%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox>&nbsp;
									<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txt_cont_person"
										ErrorMessage="Contact Person  is required.">?</asp:RequiredFieldValidator></TD>
								<TD class="tablecol" ></TD>
				                <TD class="tablecol" ><strong><asp:Label id="Label7" Text="Contact Number : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
											<asp:textbox id="txt_num" runat="server" CssClass="txtbox"  MaxLength="20"></asp:textbox></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label8" Text="Email : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
										<asp:textbox id="txt_email" width="80%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></TD>
								<TD class="tablecol" colspan="3"></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label9" Text="Payment Term : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
									<asp:dropdownlist id="ddl_pt" width="80%" runat="server" CssClass="ddl" >
										<asp:ListItem Value=" --- Select ---" Selected="True"> --- Select ---</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD class="tablecol" ></TD>
				                <TD class="tablecol" ><strong><asp:Label id="Label10" Text="Payment Method : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
									<asp:dropdownlist id="ddl_pm" width="80%" runat="server" CssClass="ddl" >
										<asp:ListItem Value="--- Select ---" Selected="True">--- Select ---</asp:ListItem>
									</asp:dropdownlist></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label11" Text="Shipment Term : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
											<asp:dropdownlist id="ddl_st" width="80%" runat="server" CssClass="ddl" >
										<asp:ListItem Value="--- Select ---" Selected="True">--- Select ---</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD class="tablecol" ></TD>
				                <TD class="tablecol" ><strong><asp:Label id="Label12" Text="Shipment Mode : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" > 
									<asp:dropdownlist id="ddl_sm" width="80%" runat="server" CssClass="ddl" >
										<asp:ListItem Value="--- Select ---" Selected="True">--- Select ---</asp:ListItem>
									</asp:dropdownlist></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label13" Text="External Remarks : " runat="server"></asp:Label></strong></TD> 
				                <TD class="tableinput" colspan="2"> 
									<asp:textbox id="txt_remark" runat="server" Height="32px" width="300px" TextMode="MultiLine" CssClass="txtbox" MaxLength="1000" ></asp:textbox></TD>
								<TD class="tablecol" colspan="2"></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol"  noWrap align="left">&nbsp;<strong><asp:Label id="Label14" Text="External Attachment : " runat="server"></asp:Label></strong><br>&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></TD> 
								<TD class="tableinput" colspan="4">
								<INPUT class="button" id="File1" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 290px;" type="file" size="20"
										name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmd_upload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></TD>
					    </tr> 
					    <tr>
				                <TD class="tablecol" >&nbsp;<strong><asp:Label id="Label15" Text="External File Attached : " runat="server"></asp:Label></strong></TD> 
								<TD class="tableinput" colspan="4">
									<asp:panel id="pnlAttach" runat="server"></asp:panel></TD>
                       </tr> 
                       </table>
                       

					</TD>
				</TR>

				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" style="HEIGHT: 18px" colSpan="2">&nbsp;Set Vendor List</TD>
							</TR>
							<tr>
				                <TD class="tablecol" width="40%">&nbsp;<strong>
				                    <asp:Label id="Label17" Text="Select Vendor from Pre-Defined List : " runat="server"></asp:Label></strong></TD> 
								<TD class="tablecol" ><asp:dropdownlist id="cboVendor" width="30%" runat="server" CssClass="ddl" AutoPostBack="true" CausesValidation="false"></asp:dropdownlist></TD>
							</tr>
							<tr>
				                <TD class="tablecol" width="40%">&nbsp;<strong>
				                    <asp:Label id="Label16" Text="Select Vendor from Vendor List : " runat="server"></asp:Label></strong></TD> 
								<TD class="tablecol" ><asp:button id="cmd_addSV" CausesValidation="False" runat="server" CssClass="Button" Text="Select" ></asp:button></TD>
							</tr>
							<%--<tr>
							<td class="tablecol">
                                &nbsp;<strong><asp:Label ID="Label17" runat="server" Text="Select Vendor from Pre-Defined List :"></asp:Label></strong>
							</td>
							<td class="tablecol">
                                <asp:DropDownList ID="ddlVendorList" runat="server" CssClass="ddl">
                                </asp:DropDownList>
							</td>
							</tr>--%>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR class="tablecol">
								<TD width="100%" >&nbsp;
									<asp:datagrid id="dtg_vendor" runat="server" CssClass="grid" AutoGenerateColumns="False">
										<Columns>											
											<asp:TemplateColumn HeaderText="Selected Vendors: ">
							                    <HeaderStyle HorizontalAlign="Left" Width="92%"></HeaderStyle>
							                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							                    <ItemTemplate>
							                        <asp:LinkButton ID="lnkVendor" runat="server" OnCommand="LinkButton_Click"></asp:LinkButton>
                                                    <asp:Label id="lblVendor" runat="server"></asp:Label>
							                    </ItemTemplate>
						                    </asp:TemplateColumn>	
											<asp:TemplateColumn HeaderText="Delete">
												<HeaderStyle Width="8%"></HeaderStyle>
												<ItemTemplate>
													&nbsp;
													<asp:ImageButton id="img_delete" runat="server" CausesValidation="false" ImageUrl="#"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="RVDLM_List_Index"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="TYPE"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Added"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</TD>
							</TR>
						
						</TABLE>
					</TD>
				</TR>
				<TR>
				<TD><asp:datagrid id="dgviewitem" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RD_Product_Desc" HeaderText="Item Name *">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" Width="367px" TextMode="MultiLine"
											Height="40px" MaxLength="250" Rows="3"></asp:TextBox>
										<asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:RequiredFieldValidator id="val_ItemDesc" runat="server" Display="none" ErrorMessage="Item Name is required."
											ControlToValidate="txt_desc"></asp:RequiredFieldValidator>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="UOM ">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" runat="server" CssClass="ddl"></asp:DropDownList>
										<asp:Label id="lbl_uom" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="QTY *">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qty" CssClass="numerictxtbox" Width="55px" Runat="server" Rows="2"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_Qty" Display="none" ErrorMessage="Invalid quantity." ControlToValidate="txt_qty"
											Runat="server"></asp:RegularExpressionValidator>
										<asp:RequiredFieldValidator ID="reqVal_Qty"
                                                runat="server" Display="none" ControlToValidate="txt_qty" ErrorMessage="Require Quantity"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead &lt;BR&gt;Time (days)+ ">
									<HeaderStyle Width="12%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_delivery" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_delivery" Display="none" ControlToValidate="ddl_cur" Runat="server"></asp:RegularExpressionValidator>
										<asp:RequiredFieldValidator ID="reqVal_delivery" runat="server" Display="none" ControlToValidate="txt_delivery" ErrorMessage="Require EDD"></asp:RequiredFieldValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Warranty &lt;BR&gt;Terms (mths) ">
									<HeaderStyle Width="19%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_warranty" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_warranty" Display="none" ControlToValidate="txt_warranty" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
																
								<asp:BoundColumn Visible="False" DataField="RD_PRODUCT_CODE" HeaderText="PRODUCT_CODE"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RD_COY_ID" HeaderText="COY_ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RD_VENDOR_ITEM_CODE" HeaderText="VENDOR_ITEM_CODE"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="TYPE" ></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RD_RFQ_LINE" ></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				<TR>
		<TR>
					<TD style="HEIGHT: 10px"></TD>
				</TR>
				<TR>
					<TD><STRONG>+0 denotes Ex-Stock.</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_save" runat="server" CssClass="BUTTON" Width="47px" Text="Save" ></asp:button>
					    <asp:button id="cmd_Submit" runat="server" CssClass="Button" Text="Submit" ></asp:button>
					    <asp:button id="cmd_addcath" runat="server" CssClass="BUTTON" Width="74px" Text="Add Item" CausesValidation="False" ></asp:button>
						&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Remove Item" Width="74px"></asp:button>
						<asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> 
						<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button> 
						<INPUT type="button" value="View RFQ" id="cmd_View" runat="server" Class="button" style="width: 75px" visible="false">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<%--<TR>
					<TD><asp:requiredfieldvalidator id="rvl_RFQName" runat="server" ControlToValidate="txt_name" ErrorMessage="RFQ Description is required."
							Display="None" Enabled="False"></asp:requiredfieldvalidator></TD>
				</TR>--%>
				<TR>
					<TD><asp:label id="lbl_check" runat="server" ForeColor="Red"></asp:label>
						<br>
						<asp:label id="Label2" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<tr>
					<td>
					    <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Height="24px"></asp:validationsummary>
					    <asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
					</td>					
				</tr>
				<TR>
					<TD><STRONG>
                        <asp:HyperLink ID="lnkBack" runat="server">
							<STRONG>&lt; Back</STRONG></asp:HyperLink></STRONG></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
