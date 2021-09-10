<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvoiceTrackingList.aspx.vb" Inherits="eProcure.InvoiceTrackingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html>
<head>
    <title>Invoice Tracking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PrintWindow as string = dDispatcher.direct("ExtraFunc","FramePrinting.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            Dim PDFWindow As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            'mimi 2018-06-06 : inv processing
            Dim PopCalendar As String = dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox='+val+'&seldate='+txtVal.value+'")
            Dim CalImage As String = "<IMG src=" & dDispatcher.direct("Plugins/images", "i_Calendar2.gif") & " border=""0"">"
            Dim FundType As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L1") 'mimi 2018-06-27 : inv processing
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
        </script> 
		<%response.write(Session("WheelScript"))%>
        <% Response.Write(CSS)%>
		<%response.write(Session("JQuery"))%>
        <% Response.Write(Session("AutoComplete")) %>
        
		<script type="text/javascript">
		<!--
		  $(document).ready(function(){
		  $('#cmdApprove').click(function() {
		  if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
		  {		    
		    return false;
		  }
		  else
		  {
		      if (document.getElementById("cmdApprove"))
              { document.getElementById("cmdApprove").style.display= "none"; }
              if (document.getElementById("cmdSubmit"))
              { document.getElementById("cmdSubmit").style.display= "none"; }
              if (document.getElementById("cmdSave"))
              { document.getElementById("cmdSave").style.display= "none"; }          
              if (document.getElementById("cmdMark"))
              { document.getElementById("cmdMark").style.display= "none"; }
              return true;       
		  }             
          });
          $('#cmdSubmit').click(function() {
          if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
		  {		    
		    return false;
		  }
		  else
		  {
		      if (document.getElementById("cmdApprove"))
              { document.getElementById("cmdApprove").style.display= "none"; }
              if (document.getElementById("cmdSubmit"))
              { document.getElementById("cmdSubmit").style.display= "none"; }
              if (document.getElementById("cmdSave"))
              { document.getElementById("cmdSave").style.display= "none"; }          
              if (document.getElementById("cmdMark"))
              { document.getElementById("cmdMark").style.display= "none"; }
              return true;       
		  }        
          });
          $('#cmdPreviewInvoice').click(function() {
          if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
		  {		    
		    return false;
		  }
		  });
          $('#cmdSave').click(function() {
          if (document.getElementById("cmdApprove"))
          { document.getElementById("cmdApprove").style.display= "none"; }
          if (document.getElementById("cmdSubmit"))
          { document.getElementById("cmdSubmit").style.display= "none"; }
          if (document.getElementById("cmdSave"))
          { document.getElementById("cmdSave").style.display= "none"; }          
          if (document.getElementById("cmdMark"))
          { document.getElementById("cmdMark").style.display= "none"; }          
          });
          $('#cmdMark').click(function() {
          if (checkAtLeastOneResetSummary('chkSelection','',0,1) == false)
		  {		    
		    return false;
		  }
		  else
		  {
		      if (document.getElementById("cmdApprove"))
              { document.getElementById("cmdApprove").style.display= "none"; }
              if (document.getElementById("cmdSubmit"))
              { document.getElementById("cmdSubmit").style.display= "none"; }
              if (document.getElementById("cmdSave"))
              { document.getElementById("cmdSave").style.display= "none"; }          
              if (document.getElementById("cmdMark"))
              { document.getElementById("cmdMark").style.display= "none"; }
              return true;       
		  }        
          });          
		  });
		function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
			{
				if (CheckAtLeastOne(p1,p2)== true) {
					if (resetSummary(cnt1,cnt2)==true)
					{
					    var cmdSubmitEle = document.getElementById("cmdSubmit");
					    if (cmdSubmitEle == null)
                        {
                            document.getElementById("cmdApprove").style.display= "none";
                        }
                        else
                        {
                            document.getElementById("cmdSubmit").style.display= "none";
                        }

					    // document.getElementById("cmdSubmit").style.display= "none";
						return true;
					}
					else
					{
						return false;
						}
				}
				else {				
					return false;
				}				
			}
			
		function selectAll()
        {
            //Jules 2018.07.16 - ctl02 not defined
			//SelectAllG("dtgInvoice_ctl02_chkAll","chkSelection");           
            if(document.getElementById("dtgInvoice_ctl01_chkAll") != null)
            {                
                SelectAllG("dtgInvoice_ctl01_chkAll", "chkSelection");
            }
            else
            {                
                SelectAllG("dtgInvoice_ctl02_chkAll", "chkSelection");
            }            
		}
				
		function checkChild(id)
        {
            //Jules 2018.07.16 - ctl02 not defined
			//checkChildG(id,"dtgInvoice_ctl02_chkAll","chkSelection");    
            if (document.getElementById("dtgInvoice_ctl01_chkAll") != null) {
                checkChildG(id, "dtgInvoice_ctl01_chkAll", "chkSelection"); 
            }
            else {
                checkChildG(id, "dtgInvoice_ctl02_chkAll", "chkSelection"); 
            }   
		}

            //mimi 2018-06-06 : inv processing
		function Reset(){            
			var oform = document.forms(0);			
			oform.txtDocNo.value="";
			oform.txtVendorName.value="";
			oform.ddldocType.selectedIndex=0;   
            oform.ddlCurr.selectedIndex = 0; 
            oform.txtAmountFrom.value = "";
            oform.txtAmountTo.value = "";
            oform.txtDueDate.value = "";
            //oform.txtFundtype.value = "";
            oform.ddlPaymentMode.selectedIndex = 0;
            oform.ddlCompResident.selectedIndex = 0;
            oform.cboFundType.selectedIndex = 0; //Jules 2018.10.16           
        }	
             
        //mimi 2018-06-06 : inv processing
        function popCalendar(val) {
                txtVal = document.getElementById(val);

                window.open('<%Response.Write(PopCalendar) %>', 'cal', 'status=no,resizable=no,width=180,height=155,left=270,top=180');
        }
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		
		function CheckAtLeastOnePrint(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{
						var strMsg;
						strMsg = "Warning :\nClicking OK will update the status of the document to 'Printed', even though you may cancel the print job at the last minute.\nDo you wish to continue ?";
						var result = confirm(strMsg);
						if (result==true){							
							//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
							return true;
						}
						else
							return false;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}
		
		function PrintWindow(strPageId)
		{
			//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
			window.open('<%response.write(PrintWindow) %>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		function PDFWindow(strPageId)
		{
			window.open('<%response.write(PDFWindow) %>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		
		function CheckAtLeastOnePdf(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{						
						//window.open('../ExtraFunc/GeneratePDF.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
						//window.open('../Template/TestPrint.aspx?type=INV','Spool','status=no,toolbar=no,location=no,menu=no,scrollbars=no,width=1,height=1');
						return true;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
            }	

        //mimi 2018-06-27 : inv processing
            <%--$(document).ready(function () {

                $("#txtFundtype").autocomplete("<% Response.Write(FundType) %>", {
                     width: 200,
                     scroll: true,
                     selectFirst: false
                 });
                $("#txtFundtype").result(function (event, data, formatted) {
                     if (data)
                         $("#hidFundType").val(data[1]);
                 });
                $("#txtFundtype").blur(function () {
                    var txtFundtype = document.getElementById("txtFundtype").value;
                    if (txtFundtype == "") {
                        $("#hidFundType").val("");
                     }
                    var hidFundType = document.getElementById("hidFundType").value;
                    if (hidFundType == "") {
                         $("#txtFundtype").val("");
                     }

                 });
            });--%>
            //end modification
		-->
		</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_InvTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr>
				    <td colspan="5">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant new Invoice. Select the invoice and click the Approve button to submit for approval."
					    ></asp:label>

				    </td>
			    </tr>
			    <tr>
					<td class="linespacing2" colspan="5"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="5">&nbsp;Search Criteria</td>
				</tr>
				<tr class="tablecol">
					<td noWrap width="15%">&nbsp;<strong>Document No.</strong> :</td>
					<td width="20%"><asp:textbox id="txtDocNo" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></td>
					<td noWrap width="15%">&nbsp;<strong>Vendor Name</strong> :</td>
					<td width="35%"><asp:textbox id="txtVendorName" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></td>
					<td>&nbsp;</td>					
				</tr>
				<tr class="tablecol">
				    <td>&nbsp;<strong>Document Type</strong> :</td>
				    <td>
                        <asp:DropDownList ID="ddldocType" CssClass="ddl" Width="200px" runat="server">
                        <asp:ListItem Value="">--- Select ---</asp:ListItem>
                        <asp:ListItem Value="INV">Invoice</asp:ListItem>
                        <asp:ListItem Value="BILL">Bill</asp:ListItem>
                        <asp:ListItem Value="CN">Credit Note</asp:ListItem>
                        <asp:ListItem Value="DN">Debit Note</asp:ListItem>
                        <asp:ListItem Value="LETTER">Letter</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;<strong>Currency</strong> :</td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCurr" CssClass="ddl" Width="200px" runat="server"></asp:DropDownList>
                    </td>
				</tr>
                <%--mimi 2018-06-04 : invoice processing--%>
                  <tr class="tablecol">
					<td noWrap width="15%">&nbsp;<strong>Payment Mode</strong> :</td>
					<td>
                        <%--Zulham 23112018--%>
                        <asp:DropDownList ID="ddlPaymentMode" CssClass="ddl" Width="200px" runat="server">
                        <asp:ListItem Value="">--- Select ---</asp:ListItem>
                        <asp:ListItem Value="TT">TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)</asp:ListItem>
                        <asp:ListItem Value="BC">CHEQUE-(RM)</asp:ListItem>
                        <asp:ListItem Value="IBG">LOCAL BANK TRANSFER-(RM)</asp:ListItem>
                        <asp:ListItem Value="BD">BANK DRAFT-(FOREIGN CURRENCY)</asp:ListItem>
                        <asp:ListItem Value="CO">CASHIER'S ORDER-(RM)</asp:ListItem>
                        </asp:DropDownList>
                    </td>
					<td noWrap width="15%">&nbsp;<strong>Payment Due Date</strong> :</td>
					<td width="35%">
                        <asp:textbox id="txtDueDate" runat="server" Width="140px" CssClass="txtbox" contentEditable="false"></asp:textbox>
                        <a onclick="popCalendar('txtDueDate');" href="javascript:;"><%response.Write(CalImage) %></a>
					</td>
                     <td>&nbsp;</td>
				</tr>
                 <tr class="tablecol">
					 <td>&nbsp;<strong>Company Resident</strong> :</td>
				    <td>
                        <asp:DropDownList ID="ddlCompResident" CssClass="ddl" Width="200px" runat="server">
                        <asp:ListItem Value="">--- Select ---</asp:ListItem>
                        <asp:ListItem Value="Y">Resident</asp:ListItem>
                        <asp:ListItem Value="N">Non-Resident</asp:ListItem>
                        </asp:DropDownList>
                    </td>
					<td noWrap width="15%">&nbsp;<strong>Fund Type</strong> :</td>
					<td width="35%">
                        <asp:DropDownList id="cboFundType" CssClass="ddl" Runat="server"></asp:DropDownList>
                        <%--<asp:textbox id="txtFundtype" runat="server" MaxLength="50" Width="200px" CssClass="txtbox2"></asp:textbox>--%>
                        <input type="hidden" id="hidFundType" runat="server" />
					</td>
					<td>&nbsp;</td>					
				</tr>
                <%--mimi 2018-06-08 : inv processing--%>
                 <tr class="tablecol">
					<td noWrap width="15%">&nbsp;<strong>Amount From</strong> :</td>
					<td width="20%"><asp:textbox id="txtAmountFrom" runat="server" MaxLength="10" Width="200px" CssClass="txtbox"></asp:textbox></td>
                     <td noWrap width="15%">&nbsp;<strong>Amount To</strong> :</td>
					<td width="20%"><asp:textbox id="txtAmountTo" runat="server" MaxLength="10" Width="200px" CssClass="txtbox"></asp:textbox></td>	
                     <td align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
                <%--end--%>
				<tr>
					<td class="emptycol" colspan="5"></td>
				</tr>			
				<tr class="tablecol" style="display:none;">
					<td noWrap style="height: 23px">&nbsp;<strong>Folder</strong> :</td>
					<td align="left" colspan="2" style="height: 23px"><asp:dropdownlist id="cboFolder" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist></td>
					<td class="emptycol" align="right" colspan="3" style="height: 23px"><asp:imagebutton id="cmdDelete" runat="server" ImageUrl="#" ToolTip="Archive Selected" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPdf" runat="server" ImageUrl="#" ToolTip="Get Printable PDF" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdSaveInv" runat="server" Width="17px" ImageUrl="#" ToolTip="Download Invoice"
							Height="17px" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPrint" runat="server" ImageUrl="#" ToolTip="Spool to Printer" Visible="False"></asp:imagebutton>&nbsp;
					</td>
				</tr>				
				<tr>
					<td class="emptycol" colspan="5">
                        <div style="width: 100%; height: 450px; overflow: scroll">
                       <asp:datagrid id="dtgInvoice" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
                                        <%--mimi 20180711 : total for checked all--%>
										<asp:checkbox id="chkAll"  runat="server" AutoPostBack="true" OnCheckedChanged="totalamount" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server" OnCheckedChanged="totalamount" AutoPostBack="true"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="IM_INVOICE_NO" HeaderText="Document No.">
									<HeaderStyle Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IM_INVOICE_TYPE" SortExpression="IM_INVOICE_TYPE" HeaderText="Document Type">
								<HeaderStyle Width="10%" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" HeaderText="Index"></asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PAYMENT_DATE" SortExpression="IM_PAYMENT_DATE"  readonly="True"   HeaderText="Due Date">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_PO_INDEX" SortExpression="IM_PO_INDEX" readonly="True"  
									HeaderText="IM_PO_INDEX"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_S_COY_ID" SortExpression="IM_S_COY_ID" readonly="True"  
									HeaderText="Vendor Name">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME"  readonly="True"    HeaderText="Vendor Name">
									<HeaderStyle Width="17%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>

                                <%--Jules 2018.10.16--%>
                                <asp:BoundColumn DataField="INVAMT_INMYR" SortExpression="INVAMT_INMYR"  readonly="True"    HeaderText="Base Amount (MYR)">
									<HeaderStyle Width="7%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
                                <%--End modification--%>
								<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE"  readonly="True"  
									HeaderText="Currency">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL"  readonly="True"    HeaderText="Amount">
									<HeaderStyle Width="7%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn readonly="True"   HeaderText="Related Document">
									<HeaderStyle Width="15%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Method">
									<ItemStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboPay" CssClass="ddl" Runat="server"></asp:DropDownList>
										<asp:Label Runat="server" ID="lblPay"></asp:Label>
									</ItemTemplate>
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME"  readonly="True"    HeaderText="Purchaser/Teller">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DEPT" SortExpression="DEPT"  readonly="True"   HeaderText="Department" Visible="False">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PRINTED" SortExpression="IM_PRINTED" HeaderText="Printed" Visible="False">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_STATUS" SortExpression="IM_INVOICE_STATUS"
									HeaderText="Status">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC"  readonly="True"   HeaderText="Status" Visible="False">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
                                <%--Zulham 06082018 - PAMB--%>
								<asp:TemplateColumn HeaderText="Approval Remarks *">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblRemarks" Runat="server"></asp:Label><BR>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" Width="150px" MaxLength="400" Runat="server"
											TextMode="MultiLine" Rows="2"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px"  contentEditable="false" 
											ForeColor="Red"></asp:TextBox><input class="txtbox" id="hidCode" type="hidden" name="hidCode" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IM_FM_APPROVED_DATE" SortExpression="IM_FM_APPROVED_DATE"  readonly="True"  
									HeaderText="Approved Payment Date">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="POM_BILLING_METHOD" SortExpression="POM_BILLING_METHOD"
									 readonly="True"    HeaderText="POM_BILLING_METHOD"></asp:BoundColumn>
							    <asp:BoundColumn Visible="False" DataField="POM_PAYMENT_METHOD" SortExpression="POM_PAYMENT_METHOD"
									 HeaderText="POM_PAYMENT_METHOD"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="DOC_TYPE" SortExpression="DOC_TYPE"
									 HeaderText="DOC_TYPE"></asp:BoundColumn>
							    <asp:BoundColumn Visible="False" HeaderText="Contract"></asp:BoundColumn>
							    <asp:BoundColumn Visible="False" HeaderText="Inv No" DataField="IM_INVOICE_NO"></asp:BoundColumn>
							    <asp:BoundColumn Visible="False" HeaderText="SST Inv" DataField="IM_GST_INVOICE"></asp:BoundColumn>
                                <%--mimi 2018-06-05 : invoice processing--%>
                                <asp:BoundColumn DataField="IM_RESIDENT_TYPE" SortExpression="IM_RESIDENT_TYPE" HeaderText="Company Resident">
								<HeaderStyle Width="7%" />
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="IM_PAYMENT_TERM" SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Mode">
								<HeaderStyle Width="20%" />
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="ID_ANALYSIS_CODE1" SortExpression="ID_ANALYSIS_CODE1" HeaderText="Fund Type">
								<HeaderStyle Width="20%" />
								</asp:BoundColumn>
                                <%--end--%>
							</Columns>
						</asp:datagrid>
                            </div>
					</td>
				</tr>
                <%--mimi 2018-06-07 : inv processing--%>
				<tr id="trTotalAmt" runat="server" visible="false">
					<td class="emptycol" width="10%"></td>
                    <td width="30%" align="right"><strong>Total Amount</strong> :</td>
					<td width="20%"><asp:textbox id="txtTotalAmt" runat="server" MaxLength="10" Width="100px" CssClass="txtbox"></asp:textbox></td>
				</tr>
                <%--end--%>
                <tr>
					<td class="emptycol" colspan="4"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Mass Verify"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdApprove" runat="server" Width="96px" CssClass="button" Text="Mass Approve"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdMark" runat="server" Width="80px" CssClass="button" Text="Mark As Paid" CausesValidation="False"></asp:button>
						<input class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset" name="cmdReset" runat="server"/> 
					    <input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
					    <asp:button id="cmdPreviewInvoice" runat="server" CssClass="button" Width="100px" Text="View Invoice" CausesValidation="False"></asp:button>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary" runat="server"/>
						<input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl" runat="server"/></td>
				</tr>
				</table>
		</form>
</body>
</html>
