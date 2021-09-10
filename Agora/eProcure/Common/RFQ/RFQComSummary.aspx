<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQComSummary.aspx.vb" Inherits="eProcure.RFQComSummary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RFQComSummary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim bubblepopupcss As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "jquery.bubblepopup.v2.3.1.css") & """ rel='stylesheet' type='text/css'>"
            Dim imgHigh As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","badmoney_icon.gif") & """>"     
            Dim imgDate As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","date_icon.gif") & """>"     
            Dim imgLowest As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","bestmoney_icon.gif") & """>"  
            Dim imgNoQuote As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","bad_icon.gif") & """>"  
            Dim imgIncomplete As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","average_icon.gif") & """>"  
            Dim imgComplete As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","good_icon.gif") & """>"  
            Dim imgSupply As String = "<IMG src=""" & dDispatcher.direct("Plugins/images","Unable To Supply Icon.gif") & """>"
            Dim bubblepopupjquery As String = "<script type=""text/javascript"" src="""& dDispatcher.direct("Plugins/include","jquery.bubblepopup.v2.3.1.min.js") &""">"            
          </script> 
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(bubblepopupcss)%>
		<% Response.Write(Session("JQuery")) %>
		<% Response.Write(bubblepopupjquery) %>
		<% Response.Write("</script>") %>
		
		<script language="javascript">
		
		$().ready(function(){
		    var strTemp;
		    var strPrev;
		    var strNew;
		    //var str;
		    
//		    var value1="10,233,122.34";
//               var value2="233.56";
//               alert(parseFloat(value1.replace(",","").replace(",","").replace(",","").replace(",","")));
//               alert(parseFloat(value2.replace(",","")));

//		    strPrev="r1_c1";
//    		alert($('#hidTotalItemDesc').val());
//    		alert($('#hidTotalVendor').val());
//		    alert($('#r1_c1').html());
    //		$('#r1_c1').css('background-color', 'red');
	        for (r=1; r <= $('#hidTotalItemDesc').val(); r++)
	        {
			     for (c=1; c <= $('#hidTotalVendor').val(); c++)
			     {
			        if (c==1)
			            {strPrev="r" + r + "_c" + c;}
			            //alert($('#' + strPrev).html().replace(",",""));
			            //alert(parseFloat($('#' + strPrev).html().replace(",","").replace(",","").replace(",","")));}
			            //str = parseFloat(strPrev.replace(",","")); 
			            //alert("1: " + $('#' + str).html());}
			        else			            
			            {strTemp="r" + r + "_c" + c;
			            //alert("2: " + $('#'+ strTemp).html());
			            //alert(parseFloat($('#' + strTemp).html().replace(",","").replace(",","").replace(",","")));			              
//			            alert("2: " + $('#'+ strTemp).html());
                            if (($('#' + strPrev).html()!="No Quote") && ($('#'+ strTemp).html()!="No Quote"))
                                {    
//                                    alert('a' + parseFloat($('#' + strPrev).html()));
//                                  alert(($('#' + strTemp).html()) <  ($('#' + strPrev).html())); 
                                    if (parseFloat($('#' + strTemp).html().replace(",","").replace(",","").replace(",","").replace(",","").replace(",","")) <  parseFloat($('#' + strPrev).html().replace(",","").replace(",","").replace(",","").replace(",","").replace(",","")))
//                                      if (($('#' + strTemp).html()) <  ($('#' + strPrev).html()))

	                                    {
	                                        strPrev=strTemp;	                                   
//	                                         alert('s' + parseFloat($('#' + strPrev).html()));
	                                    }	 
//	                                else {alert('n' + parseFloat($('#' + strPrev).html()));}                                              
                                }
                                
                            else if ($('#' + strPrev).html()=="No Quote" && $('#'+ strTemp).html()=="No Quote")
                                {
//                                alert('a' + parseFloat($('#' + strPrev).html()));
                                strPrev=strPrev;}
                            
                            else if ($('#' + strPrev).html()=="No Quote" && $('#'+ strTemp).html()!="No Quote")
                                {
//                                alert('b' + parseFloat($('#' + strPrev).html()));
                                strPrev=strTemp;}
                                
                            else if ($('#' + strPrev).html()!="No Quote" && $('#'+ strTemp).html()=="No Quote")
                                {
//                                alert('c' + parseFloat($('#' + strPrev).html()));
                                strPrev=strPrev;}
			            }
			    }
//			    alert('colored: ' + $('#' + strPrev).html());   
                if ($('#' + strPrev).html()!="No Quote"){$('#' + strPrev).css('background-color', '#C0C0FF');}
                
                for (c=1; c <= $('#hidTotalVendor').val(); c++)
                {			        			            
                    strNew ="r" + r + "_c" + c;			              
//                    alert('new: ' + $('#' + strNew).html());  
//                    alert('prev: ' + $('#' + strPrev).html());  
//                    alert('compare: ' + parseFloat($('#' + strNew).html()) ==  parseFloat($('#' + strPrev).html()))
//                    alert(($('#' + strNew).html()) =  ($('#' + strPrev).html()));  
                    if (parseFloat($('#' + strNew).html().replace(",","").replace(",","").replace(",","").replace(",","").replace(",","")) ==  parseFloat($('#' + strPrev).html().replace(",","").replace(",","").replace(",","").replace(",","").replace(",","")))
                        {
//                            strPrev = strNew;	 
                            $('#' + strNew).css('background-color', '#C0C0FF');                                  
                        }	 
                }
//                if ($('#' + strPrev).html()!="No Quote"){$('#' + strPrev).css('background-color', '#C0C0FF');}    
//                alert($('#' + strPrev).html());   
		    }  
		    
		   
//		   
//	        for (r=1; r <= $('#hidTotalItemDesc').val(); r++)
//	        {
//			     for (c=1; c <= $('#hidTotalVendor').val(); c++)
//			     {			        			            
//		            strNew ="r" + r + "_c" + c;			              
//                    alert($('#' + strNew).html());  
//                    alert(($('#' + strNew).html()) =  ($('#' + strPrev).html()));  
//                    if (parseFloat($('#' + strNew).html()) =  parseFloat($('#' + strPrev).html()))
//                        {
//                            strPrev = strNew;	                                   
//                        }	 
//			            
//			    }
//                if ($('#' + strPrev).html()!="No Quote"){$('#' + strPrev).css('background-color', '#C0C0FF');}    
//		    }

            $('#cmd_raise_pr').click(function() {
            document.getElementById("cmd_raise_pr").style.display= "none";
             });    		 
		});		
			
		function selectAll()
		{
		
			SelectAllG("dtg_itemdis_ctl01_chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
		var change = document.getElementById("hid1");
		    change.value ="1";
			checkChildG(id,"dtg_itemdis_ctl01_chkAll","chkSelection");
		}
		
		function selectItemDescAll()
		{
		    var iTotalItemDesc, chkItemDesc;
		    iTotalItemDesc = Form1.hidTotalItemDesc.value;
		    
		    var chkSelected;
		    if (eval("Form1.chkItemDescAll").checked)
		    { chkSelected = true; }
		    else
		    { chkSelected = false; }
		    
		    for (j=0; j < iTotalItemDesc; j++)
			{
			    chkItemDesc = eval("Form1.chkItemDesc" + j);
			    chkItemDesc.checked = chkSelected;
			}
		}
		
		function checkItemDescHeader()
		{
		    var iCountChecked=0, iTotalItemDesc;
		    
		    iTotalItemDesc = Form1.hidTotalItemDesc.value;
            for (j=0; j < iTotalItemDesc; j++)
		    {
		        chkItemDesc = eval("Form1.chkItemDesc" + j);
		        if (chkItemDesc.checked)
		        { iCountChecked++ }
		    }
		    
            if (iCountChecked == iTotalItemDesc) { eval("Form1.chkItemDescAll").checked = true; }
            else { eval("Form1.chkItemDescAll").checked = false; }  
		}
					
        $(document).ready(function(){
            <%  Response.Write(Session("jqPopup")) %>
            
        });


		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" onload="javascript:checkItemDescHeader();">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">All Quotation Response Summary <INPUT id="hid1" type="hidden" value="0" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader">&nbsp;Quotation Result</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>RFQ Number </STRONG>:
									<asp:label id="lbl_Num" runat="server"></asp:label>&nbsp;&nbsp; <STRONG>RFQ Name </STRONG>
									:<STRONG> </STRONG>
									<asp:label id="lbl_Name" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top"><asp:datagrid id="dtg_Qoute" runat="server" OnPageIndexChanged="dtg_VendorList_Page" AutoGenerateColumns="False"
										CssClass="grid">
										<Columns>											
											<asp:TemplateColumn HeaderText="Quotation Number ">
												<HeaderStyle HorizontalAlign="Left" Width="27%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lbl_qouteNum" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Quotation Validity ">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lbl_QuoValidity" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn HeaderText="Vendor (s)">
												<HeaderStyle HorizontalAlign="Left" Width="35%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Quotation Value">
												<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lbl_total" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Indicator">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
												<HeaderTemplate>
													<!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
													Indicator
												</HeaderTemplate>
												<ItemTemplate>
													<asp:ImageButton id="img_1" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="Complete Qoute"></asp:ImageButton>
													<asp:ImageButton id="img_2" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="InComplete Qoute"></asp:ImageButton>
													<asp:ImageButton id="img_3" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="No Qoute"></asp:ImageButton>
													<asp:ImageButton id="img_4" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="Lowest Overall"></asp:ImageButton>
													<asp:ImageButton id="img_5" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="Highest Overall"></asp:ImageButton>
													<asp:ImageButton id="img_6" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="Date Non-Compliance "></asp:ImageButton>
													<asp:ImageButton id="img_7" runat="server" Enabled="False" CausesValidation="False" Visible="False"
														ImageUrl="" ToolTip="Unable to Supply"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:datagrid>&nbsp;
								</TD>
								<TD width="10"></TD>
								<TD width="180">
									<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
										<TR>
											<TD class="tableheader">Indicator Legend</TD>
										</TR>
										<TR>
											<TD class="tablecol" style="HEIGHT: 23px"><% Response.Write(imgComplete) %>Complete 
												Quote</TD>
										</TR>
										<TR>
											<TD class="tablecol"><% Response.Write(imgIncomplete) %>InComplete Quote</TD>
										</TR>
										<TR>
											<TD class="tablecol" style="HEIGHT: 23px"><% Response.Write(imgNoQuote) %>No 
												Quote</TD>
										</TR>
										<TR>
											<TD class="tablecol" style="HEIGHT: 25px"><% Response.Write(imgLowest) %>Lowest 
												Overall</TD>
										</TR>
										<TR>
											<TD class="tablecol"><%  Response.Write(imgHigh)%> Highest Overall</TD>
										</TR>
										<TR>
											<TD class="tablecol"><% Response.Write(imgDate) %>Quote Date Non-<br>
												&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Compliance</TD>
										</TR>
										<TR>
											<TD class="tablecol"><%  Response.Write(imgSupply)%> Unable to Supply</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>								
				<TR>
					<TD class="tableheader">&nbsp;<STRONG>Detailed Quotation Response Comparison</STRONG></TD>
				</TR>
			    <TR>
					<TD><asp:label id="lbl_title" runat="server" Font-Bold="true">Step 1: Review details quotation response. Select item(s) for comparison in Step 2.</asp:label></TD>
				</TR>
				<TR style="display:none;">
					<TD style="height: 116px"><asp:datagrid id="dtg_itemdis" runat="server" OnPageIndexChanged="dtg_VendorList_Page"
							CssClass="grid" DataKeyField="RD_RFQ_Line">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="10px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RD_Product_Desc" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="20px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="8px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Quantity" HeaderText="QTY">
									<HeaderStyle HorizontalAlign="Right" Width="8px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_RFQ_Line" HeaderText="Line">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>				
				<tr><td><%  Response.Write(Session("w_RFQ_Item"))%> </td></tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>		
				<TR>
					<TD><asp:label id="Label2" runat="server" Font-Bold="true">Step 2: Click Compare button to display total value on selected item(s) by vendor 'Compare' button.</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>		
				<TR>
					<TD>
						<asp:button id="cmd_compare" runat="server" CssClass="button" Text="Compare" AutoPostBack="false" CausesValidation="False" ></asp:button>
					</TD>
				</TR>	
				<tr><td><%  Response.Write(Session("w_RFQ_Rank"))%></td></tr>
				
				<TR style="display:none;">
					<TD class="tableheader">&nbsp;<STRONG>Comparison Summary &gt;
							<asp:label id="Label1" runat="server"></asp:label>&nbsp;&gt; Generate PO for selected item(s) </STRONG>
					</TD>
				</TR>
				<TR style="display:none;">
					<TD><asp:datagrid id="dtg_rank" runat="server" OnPageIndexChanged="dtg_VendorList_Page" AutoGenerateColumns="False"
							CssClass="grid">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
									<HeaderTemplate>
										<!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection2" Enabled="False" Visible="False" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Rank">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemTemplate>
										<asp:label id="lbl_rank" runat="server" ForeColor="Black"></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Vendor Name">
									<HeaderStyle HorizontalAlign="Left" Width="40%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_supplier" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RRM_Actual_Quot_Num" HeaderText="Quotation Number">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_unitprice" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn HeaderText="Total Value">
									<HeaderStyle HorizontalAlign="Right" Width="10%" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RRM_V_Company_ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CM_COY_NAME"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RRM_Currency_Code"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RRM_RFQ_ID" SortExpression="RRM_RFQ_ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RRM_Actual_Quot_Num"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						<asp:Label id="lbl_rankerror" runat="server" ForeColor="Red"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 14px">&nbsp;</TD>
				</TR>				
				<TR>
					<TD><asp:label id="Label3" runat="server" Font-Bold="true">Step 3: Select vendor and click Raise PO button to continue.</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>		
				<TR>
					<TD style="HEIGHT: 18px"><asp:button id="cmd_raise_pr" runat="server" CssClass="button" Text="Raise PO" Enabled="False" Visible="true"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 14px">&nbsp;</TD>
				</TR>
				<TR>
					<TD><A id="cmd_Previous" href="#" runat="server"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>