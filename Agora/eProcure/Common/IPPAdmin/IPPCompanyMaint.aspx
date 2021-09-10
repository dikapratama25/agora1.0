<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPCompanyMaint.aspx.vb" Inherits="eProcure.IPPCompanyMaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>E2P_Company_Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script> 
        <%Response.Write(css)%> 
        <%response.write(Session("WheelScript"))%>
         <script language="javascript">
			//debugger;
			
			function selectAll()
			{
				SelectAllG2("dtgIPPCompany_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG2(id,"dtgIPPCompany_ctl02_chkAll","chkSelection");
			}
			
			function SelectAllG2(pChkAllID,pChkSelName) 
			{
			    var oform = document.forms[0];
			    var num = 0;
			    var chkAllBox = document.getElementById(pChkAllID);
			    CheckAllDataGridCheckBoxesG2(pChkSelName,chkAllBox.checked);
		    }
		    
		    function CheckAllDataGridCheckBoxesG2(pChkSelName, checkVal) 
		    {
			    re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon
			    for(i = 0; i < document.forms[0].elements.length; i++) 
				{
				    elm = document.forms[0].elements[i]
				    if (elm.type == 'checkbox') 
						//if (re.test(elm.name)){ 
						 if ((elm.name.substr(3)=="VCI") || (elm.name.substr(3)=="PCI") || (elm.name.substr(3)=="FreeForm") || (elm.name.substr(3)=="_compare") || (elm.name.substr(3)=="HubPending") || (elm.name.substr(3)=="HubApprove") || (elm.name.substr(3)=="Reject") || (elm.name.substr(3)=="Spot") || (elm.name.substr(3)=="Stock") || (elm.name.substr(3)=="MRO") || (elm.name.substr(3)=="CustomPR") || (elm.name.substr(3)=="RemarkPR"))
						 {
						 }
						 else
						{ if ((!elm.disabled) && (elm.name!='chkVendor') && (elm.name!='chkOtherBCoy') && (elm.name!='chkEmployee') && (elm.name!='chkActive') && (elm.name!='chkInactive'))
						    {
								elm.checked = checkVal; //checkRow(elm.id);
							}
						}
				}
			}
			
			function checkChildG2(id,pChkAllID,pChkSelName) 
			{
			    var oform = document.forms[0];
			    //checkRow(id);
			    var chkAllBox = document.getElementById(pChkAllID); 
			    CheckAll(chkAllBox,pChkSelName);
		    }
		    
            function CheckAll(pChkAllID,pChkSelName)
	        {
    			var oform = document.forms[0];
			    re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			    for (var i=0;i<oform.elements.length;i++)
			    {
    				var e = oform.elements[i];  
				    if (e.type=="checkbox" && e.name != pChkAllID.name && e.name !='chkVendor' && e.name !='chkOtherBCoy' && e.name !='chkEmployee' && e.name !='chkActive' && e.name !='chkInactive')
				    { 
				        if (e.checked==false) 
					    {
					        pChkAllID.checked=false;return;
					    }
					}
			     }
			    pChkAllID.checked=true;
	        }
	        
		    function CheckAtLeastOneDtg(pChkSelName,pButFunc)
		    {
    		    var oform = document.forms[0];
			    re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			    for (var i=0;i<oform.elements.length;i++)
			    {
    				var e = oform.elements[i];
				    if ((e.type=="checkbox") && (e.name!='chkVendor') && (e.name!='chkOtherBCoy') && (e.name !='chkEmployee') && (e.name!='chkActive') && (e.name!='chkInactive')) 
				    {
    					if (e.checked==true)
				        {
							if ((e.name.substr(e.name.length-6)=="chkAll"))
			       	        {
				            }
				            else
				            {
					           if (pButFunc=="delete")
							        return confirm('Are you sure that you want to permanently delete this item(s)?');
							   else if (pButFunc=="modify")
							        if (CheckOnlyOne(pChkSelName)==false) 
    							        return false; 
							        else
							            return true;
							    else
							        return true;
				             }
				        }
				    }				   
			    }
			    alert('Please make at least one selection!');
			    return false;
	        }
			
			

			
			function Reset()
			{
			    var oform = document.forms(0);
			    var a = document.getElementById('txtCompanyCode');
			    if(a)
			    {
				    a.value = "";
			    }
			    var b = document.getElementById('txtCompanyName');
			    if(b)
			    {
				    b.value = "";
			    }		
			    var c = document.getElementById('chkActive');
			    if(c)
			    {
				    c.checked = false;
			    }	
			    var d = document.getElementById('chkInactive');
			    if(d)
			    {
				    d.checked = false;
			    }
			    var e = document.getElementById('chkOtherBCoy');
			    if(e)
			    {
				    e.checked = false;
			    }	
			    var f = document.getElementById('chkVendor');
			    if(f)
			    {
				    f.checked = false;
			    }	
			    var h = document.getElementById('chkEmployee');
			    if(h)
			    {
				    h.checked = false;
			    }	
			    var g = document.getElementById('txtBusinessRegNo');
			    if(g)
			    {
				    g.value = "";
			    }			   
		    }		    
		</script>
</head>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server">
        <table class="AllTable" id="table1"  cellSpacing="0" cellPadding="0" width="100%">
            <tr>
			    <TD class="Header" colSpan="5" style="height: 19px"><asp:label id="lblTitle" runat="server" CssClass="header">Company Maintenance</asp:label></TD>
			</TR>			   
			<tr><td class="rowspacing" style="width: 286px" colspan="5"></td></tr>	
			<tr>
			    <td class="TableHeader" colSpan="5">Search Criteria</td>
			</tr>
			<tr>
			     <td class="TableCol" style="width: 150px"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Company Abbreviation"  CssClass="lbl"></asp:Label><asp:Label ID="Label2" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				 <td class="TableCol" style="width: 220px">&nbsp;<asp:textbox id="txtCompanyCode" runat="server" MaxLength="30" CssClass="txtbox" Width="100%"></asp:textbox></td>
                 <td class="TableCol" style="width: 171px"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Company Name/Staff Name"  CssClass="lbl"></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				 <td class="TableCol" colspan="2">&nbsp;<asp:textbox id="txtCompanyName" runat="server" Width="255px" MaxLength="100" CssClass="txtbox"></asp:textbox></td>
            </tr>
               <tr>
             <td class="TableCol" style="width: 171px"><strong>&nbsp;<asp:Label ID="Label9" runat="server" Text="Business Reg. No./Staff ID"  CssClass="lbl"></asp:Label><asp:Label ID="Label10" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				 <td class="TableCol" colspan="4">&nbsp;<asp:textbox id="txtBusinessRegNo" runat="server" Width="210px" MaxLength="100" CssClass="txtbox"></asp:textbox></td>
            </tr>	
            <tr>
                 <td class="TableCol" style="width: 119px">&nbsp;
                     <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Company Type :"></asp:Label></td>
                 <td class="TableCol" style="height: 38px; width: 226px;">
                    <asp:CheckBox ID="chkVendor" runat="server" Text="Vendor" />
                    <asp:CheckBox ID="chkOtherBCoy" runat="server" Text="Billing to RC" />
                    <asp:CheckBox ID="chkEmployee" runat="server" Text="Employee" />   
                 </td>
                 <td class="TableCol" style="width: 171px">&nbsp;
                     <asp:Label ID="Label5" runat="server" Text="Status" CssClass="lbl" Font-Bold="True" ></asp:Label><asp:Label ID="Label6" runat="server" Text=" :" CssClass="lbl" Font-Bold="True"></asp:Label>&nbsp;</td>
                 <td class="TableCol" align="left" style="height: 38px">
					<asp:CheckBox ID="chkActive" runat="server" text="Active"/>&nbsp;<asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" />
				 <td class="TableColSearBtn" align="right" >
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
					<input type="button" class="button" id="cmdClear" runat="server" onclick="Reset();" value="Clear" name="cmdClear"></td>
			</tr>	
			<tr>
			    <td class="rowspacing" style="width: 119px"></td>
			</tr>
        </table>
        
        <table class="AllTable" id="table2"  cellSpacing="0" cellPadding="0" width="100%">
         <TR>				   
			 <TD class="EmptyCol">
			<%--This is how to make rowspacing inside TD tag--%>					    
				<div id="IPPCompany" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						<asp:datagrid id="dtgIPPCompany" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="IC_INDEX">
						<%--<asp:datagrid id="dtgIPPCompany" runat="server">--%>
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
								    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    <HeaderTemplate>
								        <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
								    </HeaderTemplate>
								    <ItemTemplate>
									    <asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="IC_INDEX" SortExpression="IC_INDEX" HeaderText="IC_INDEX">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_COY_TYPE" SortExpression="IC_COY_TYPE" HeaderText="Company Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_OTHER_B_COY_CODE" SortExpression="IC_OTHER_B_COY_CODE" HeaderText="Company Abbreviation">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="IC_COY_NAME" SortExpression="IC_COY_NAME" HeaderText="Company Name">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>--%>
								<asp:TemplateColumn SortExpression="IC_COY_NAME" HeaderText="Company Name/Employee Name">
									<HeaderStyle Width="17%"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkItem"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IC_BUSINESS_REG_NO" SortExpression="IC_BUSINESS_REG_NO" HeaderText="Company Reg. No./Staff ID">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								
								<%--Zulham 29062015 - HLB-IPP Stage 4(CR)--%>
								<%--Additional column for BILLING GL CODE--%>
								<asp:BoundColumn DataField="IC_BILL_GL_CODE" SortExpression="IC_BILL_GL_CODE" HeaderText="Billing GL Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
                                <%--END--%>
								
								<asp:BoundColumn DataField="IC_TAX_REG_NO" SortExpression="IC_TAX_REG_NO" HeaderText="GST Reg. No.">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_GST_INPUT_TAX_CODE" SortExpression="IC_GST_INPUT_TAX_CODE" HeaderText="GST Input Tax Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								
								<%--Zulham 29062015 - HLB-IPP Stage 4(CR)--%>
								<%--Additional column for BILLING GL CODE--%>
								<asp:BoundColumn DataField="IC_BILL_GL_CODE" SortExpression="IC_BILL_GL_CODE" HeaderText="Billing GL Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
                                <%--END--%>
								
								<asp:BoundColumn DataField="IC_ADDITIONAL_2" SortExpression="IC_ADDITIONAL_2" HeaderText="Staff Cessation Effective Date">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_ADDITIONAL_1" SortExpression="IC_ADDITIONAL_1" HeaderText="Job Grade">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn> 
								<asp:BoundColumn DataField="IC_ADDITIONAL_3" SortExpression="IC_ADDITIONAL_3" HeaderText="Branch Code">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_ADDITIONAL_4" SortExpression="IC_ADDITIONAL_4" HeaderText="Cost Centre Code">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn> 
								<asp:BoundColumn DataField="IC_PAYMENT_METHOD" SortExpression="IC_PAYMENT_METHOD" HeaderText="Payment Mode">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BC_BANK_NAME" SortExpression="BC_BANK_NAME" HeaderText="Bank Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_BANK_ACCT" SortExpression="IC_BANK_ACCT" HeaderText="Bank Account No.">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_CON_IBS_CODE" SortExpression="IC_CON_IBS_CODE" HeaderText="Conventional IBS GL Code">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IC_NON_CON_IBS_CODE" SortExpression="IC_NON_CON_IBS_CODE" HeaderText="Non Conventional IBS GL Code">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>  
							    <asp:BoundColumn DataField="IC_STATUS" SortExpression="IC_STATUS" HeaderText="Status">
									    <HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>   
							    </Columns>
						    </asp:datagrid>
				        </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblCoyType" runat="server" Text="Company Type" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy" Width="70px"></asp:Label>
		                            <asp:Label ID="lblCoyCode" runat="server" Text="Company Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblCoyName" runat="server" Text="Company Name" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy" Width="119px"></asp:Label>						    
		                            <asp:Label ID="lblPaymentMethod" runat="server" Text="Payment Mode" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy" Width="137px"></asp:Label>
		                            <asp:Label ID="lblConIBSCode" runat="server" Text="Conventional IBS GL Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblNonConIBSCode" runat="server" Text="Non Conventional IBS GL Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" ForeColor="Navy"></asp:Label>						    
		                        </div>			                                                       
                                <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>				       
				    </TD>
				</TR>   
              
            <tr>
				<td class="EmptyCol">
					<asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>
                </td>
			</tr>	
        </table> 
    </form>
</body>
</html>
