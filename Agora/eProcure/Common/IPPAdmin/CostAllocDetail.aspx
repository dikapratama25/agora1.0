<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CostAllocDetail.aspx.vb" Inherits="eProcure.CostAllocDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Cost Alloc. Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
		 <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
           
      </script>
        <%Response.Write(css)%>   
        <% Response.write(Session("typeahead")) %>
        <% Response.Write(Session("WheelScript"))%>
       
		<script language="javascript">
		<!--						
		    function reloadPage()
            {
                document.all("cmdSearch").click();
            }
            
			function Reset(){
				var oform = document.forms(0);
				oform.txtCode.value="";
				oform.txtDesc.value="";
				oform.txtVendor.value="";
				oform.txtDateFr.value="";
				oform.txtDateTo.value="";
			}
		
			function selectAll()
			{
//				SelectAllG("dtgCostAllocDetail_ctl02_chkAll","chkSelection");
				SelectAllG("chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
//				checkChildG(id,"dtgCostAllocDetail_ctl02_chkAll","chkSelection");
            checkChildG(id,"chkAll","chkSelection");
			}
		    
		    function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=500,height=280,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function ShowDialog(filename,height)
			{				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 600px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
			function TtlPercent(j)
            {
//                 var charCode = (evt.which) ? evt.which : event.keyCode
//                 if (charCode > 31 && (charCode < 48 || charCode > 57))
//                    return false;
           
//                 return true;
             
             
                
            var sum = 0;
            var b = 0;
            

            for (var i = 0; i < j; i++) {
            var counter = 0;           
            var inputs = document.getElementById("txtPercent"+i);                                             
            for(b = 0; b <= inputs.value.length; b++ )
            {
                
                var check = inputs.value.charAt(b);                
                if(check == ".")
                {
                    counter = counter + 1;
                }
            }
            if(counter >= 2)
            {
                alert("Invalid % Allocated");
                return false;
            }                        
            sum += Number(inputs.value);
            
            }
                      
           document.getElementById("total").innerHTML = sum.toFixed(2);
           if (sum > 100){
           alert ("Total Percentage is more than 100.")
            }
            }
            
        function validatedec(j)
        {           
            var b = 0;
            var i = 0
            for (i = 0; i < j; i++) {
                var counter = 0;           
                var inputs = document.getElementById("txtPercent"+i);                                             
                for(b = 0; b <= inputs.value.length; b++ )
                {
                    
                    var check = inputs.value.charAt(b);                
                    if(check == ".")
                    {
                        counter = counter + 1;                     
                    }
                }
                if(counter >= 2)
                {
                    alert("Invalid % Allocated");
                    return false;
                }                                                                                                  
            }
            return true;
        }            
        function isNumberKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                return false;

             return true;
        }            
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("CostAlloc_tabs"))%>
		<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0">
			   <td class="header" style="height: 7px" colspan="4"></td>
				
				<TR>
					<TD class = "emptycol" align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Step 1: Create Cost Allocation Code.<br><b>=></b>Step 2: Setup Cost Allocation Details."></asp:label>
                        </div>
					</TD>
				</TR>
	            <td class="header" style="height: 7px" colspan="4"></td>
				<tr>
					<td class="tableheader" colspan="6" style="height: 19px">Search Criteria</td>
				</TR>
				<TR>
					<TD class="tablecol"><strong>Cost Alloc. Code</strong>:
                        <asp:DropDownList ID="ddlCostAllocCode" runat="server" CssClass = "ddl" AutoPostBack = "true" >
                        </asp:DropDownList>
					
					</TD>
				
				</TR>
				
				<%-->tr class="tablecol">
				
					<td class="TableCol" colspan="6" align="right" style="height: 24px">
						<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
				    </td>
				</tr--%>
				</table>
					
			<br>
			
			<% response.write(Session("ConstructTable"))%>
			<br>
			<tr runat="server" id="trNewBtn"><td class="emptycol" colspan="6">
            <asp:button id="cmdNewSave"  text="Save" runat="server" cssclass="button"></asp:button>
            <asp:button id="cmdNewAddLine" runat="server" CssClass="Button" Text="Add Line"  ></asp:button>
           
            </td></tr>
			
				<TR>
					<TD class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</TR>
				
				
				
				<TABLE class="alltable" id="Table3" width="100%" cellSpacing="0" cellPadding="0">
				<%--<TR>
					<TD class="EmptyCol" colspan="6">
                        &nbsp;</TD>
				</TR>	--%>
				</table>	
			<%--	<TABLE class="alltable" id="Table2" width="100%" cellSpacing="0" cellPadding="0">
				<tr class="tablecol" style ="line-height:20px;"  >
				<strong><asp:label runat="server" id = "lblTtlCADPercent" Visible = "false">Total % Allocated:</asp:label></strong>
				</tr>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:datagrid id="dtgCostAllocDetail" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CAD_CC_CODE" Width="100%">
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
								<asp:TemplateColumn SortExpression="CAD_CC_CODE" HeaderText="Cost Centre Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label ID="lblIndexDesc" Runat="server" ></asp:Label>
										<asp:Label ID="lblIndex" Runat="server" visible = "false" ></asp:Label>
										<asp:Label ID="lblBranchCode" Runat="server" visible = "false" ></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							
								<asp:BoundColumn DataField="CAD_BRANCH_CODE" SortExpression="CAD_BRANCH_CODE" ReadOnly="True" HeaderText="Br/HO Code">
									<HeaderStyle HorizontalAlign="Left" Width="38%"></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:TemplateColumn HeaderText="% Allocated">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
									
									<ItemTemplate>
										<asp:textbox id="txtAllocated" Runat="server" CssClass = "numerictxtbox"  Width="55px" MaxLength="5"></asp:textbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>	
				</table>--%>	
				<TABLE class="alltable" id="Table4" width="100%" cellSpacing="0" cellPadding="0">					
				<tr runat="server" id="trBtn">
					<td class="emptycol" colspan="6">
						<asp:button id="cmdSave"  text="Save" runat="server" cssclass="button"></asp:button>
						<asp:button id="cmdAddLine" runat="server" CssClass="Button" Text="Add Line"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button>
						<%--asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none"></asp:button--%> 
					</td>
				</tr>
				 <td class="header" style="height: 7px" colspan="4"></td>
				<TR>
					<TD class = "emptycol" align="center">
						<div align="left">
                            &nbsp;</div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>