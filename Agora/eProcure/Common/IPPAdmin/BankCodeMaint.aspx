<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BankCodeMaint.aspx.vb" Inherits="eProcure.BankCodeMaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
		<title>Bank_Code_Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
        </script>         
        <%response.write(Session("WheelScript"))%>
         <script type="text/javascript">
			//debugger;
			function PopWindow(myBankCode)
			{
				window.open(myBankCode,"Wheel","width=500,height=300,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function selectAll()
			{
				SelectAllG2("dtgBankCode_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG2(id,"dtgBankCode_ctl02_chkAll","chkSelection");
			}
			
			function SelectAllG2(pChkAllID,pChkSelName) 
			{
			    var oform = document.forms[0];
			    var num = 0;
			    var chkAllBox = document.getElementById(pChkAllID);
			    CheckAllDataGridCheckBoxesTry(pChkSelName,chkAllBox.checked);
		    }
		    
		    function CheckAllDataGridCheckBoxesTry(pChkSelName, checkVal) 
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
						{ if ((!elm.disabled) && (elm.name!='chkActive') && (elm.name!='chkInactive'))
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
				    if (e.type=="checkbox" && e.name != pChkAllID.name && e.name !='chkActive' && e.name !='chkInactive')
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
				    if ((e.type=="checkbox") && (e.name!='chkActive') && (e.name!='chkInactive')) 
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
			
			function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;
				} 
				else 
				{
				    window.close;
					return true;
				}
			}
			
			function Reset()
			{
			    var oform = document.forms(0);
			    var a = document.getElementById('txtBankCode');
			    if(a)
			    {
				    a.value = "";
			    }
			    var b = document.getElementById('txtName');
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
		    }		    
		</script>
</head>
<body class="body" ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        <table class="AllTable" id="table1" cellspacing="0" cellpadding="0">
            <tr>
			    <td class="Header" colspan="5"><asp:label id="lblTitle" runat="server" CssClass="header">Bank Code Maintenance</asp:label></td>
			</tr>			   
			<tr>
				<td class="EmptyCol" colspan="5">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Click the Add button to add new bank."></asp:label>
				</td>
			</tr>
			<tr><td class="rowspacing" colspan="5"></td></tr>	
			<tr>
			    <td class="TableHeader" colspan="5">Search Criteria</td>
			</tr>
			<tr>
			     <td class="TableCol"><strong><asp:Label ID="Label1" runat="server" Text="Bank Code"  CssClass="lbl"></asp:Label><asp:Label ID="Label2" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td>
				 <td class="TableCol"><asp:textbox id="txtBankCode" Width="200px" runat="server" MaxLength="30" CssClass="txtbox"></asp:textbox></td>
                 <td class="TableCol"><strong><asp:Label ID="Label3" runat="server" Text="Name"  CssClass="lbl"></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td>
				 <td class="TableCol"><asp:textbox id="txtName" runat="server" Width="200px" MaxLength="100" CssClass="txtbox"></asp:textbox></td>
				 <td class="TableCol">&nbsp;</td>
            </tr>	
            <tr>
                 <td class="TableCol"><strong><asp:Label ID="Label5" runat="server" Text="Status" CssClass="lbl"></asp:Label><asp:Label ID="Label6" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></td>
                 <td class="TableCol" colspan="3">
                    <asp:CheckBox ID="chkActive" runat="server" text="Active"/>&nbsp;<asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" /></td>                 
                 <td class="TableColSearBtn" align="right">
					<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
					<input type="button" class="button" id="cmdClear" runat="server" onclick="Reset();" value="Clear" name="cmdClear"/>
					
                </td>
			</tr>	
			<tr>
			    <td class="rowspacing"></td>
			</tr>
        </table>
        
        <table class="AllTable" id="table2"  cellspacing="0" cellpadding="0">
         <tr>				   
			 <td class="EmptyCol">
			<%--This is how to make rowspacing inside TD tag--%>					    
				<div id="BankCode" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						<%--<asp:datagrid id="dtgBankCode" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="CC_COSTCENTRE_INDEX">--%>
						<asp:datagrid id="dtgBankCode" runat="server">
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
								<asp:BoundColumn Visible="False"><%--DataField="CC_COSTCENTRE_INDEX" SortExpression="CC_COSTCENTRE_INDEX" HeaderText="CC_COSTCENTRE_INDEX">--%>
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BC_BANK_CODE" SortExpression="BC_BANK_CODE" HeaderText="Bank Code">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BC_BANK_NAME" SortExpression="BC_BANK_NAME" HeaderText="Name">
									    <HeaderStyle Width="35%"></HeaderStyle>
								    </asp:BoundColumn>
								<asp:BoundColumn DataField="BC_USAGE" SortExpression="BC_USAGE" HeaderText="Usage">
									    <HeaderStyle Width="18%"></HeaderStyle>
								    </asp:BoundColumn>  
							    <asp:BoundColumn DataField="BC_STATUS" SortExpression="BC_STATUS" HeaderText="Status">
									    <HeaderStyle Width="17%"></HeaderStyle>
								    </asp:BoundColumn>   
							    </Columns>
						    </asp:datagrid>
				        </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblBankCodeLabel" runat="server" Text="Bank Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblNameLabel" runat="server" Text="Name" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="38%" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblUsageLabel" runat="server" Text="Usage" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="15%" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblStatusLabel" runat="server" Text="Status" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="15%" ForeColor="Navy"></asp:Label>						    
		                        </div>			                                                       
                                <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>				       
				    </td>
				</tr>   
              
            <tr>
				<td class="EmptyCol">
					<asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" ></asp:button>&nbsp;
					<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>
                </td>
			</tr>	
        </table> 
    </form>
</body>
</html>
