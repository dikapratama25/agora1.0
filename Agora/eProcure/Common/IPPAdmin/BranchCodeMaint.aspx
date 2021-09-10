<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BranchCodeMaint.aspx.vb" Inherits="eProcure.BranchCodeMaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
		<title>Branch_Code_Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script> 
<%--        <%Response.Write(css)%> --%>
        <%response.write(Session("WheelScript"))%>
         <script type="text/javascript">
			//debugger;
			function PopWindow(myCoyBranch)
			{
				window.open(myCoyBranch,"Wheel","width=500,height=300,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function selectAll()
			{
				SelectAllG2("dtgCoyBranch_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildTry(id,"dtgCoyBranch_ctl02_chkAll","chkSelection");
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
						{ if ((!elm.disabled) && (elm.name!='chkActive') && (elm.name!='chkInactive'))
						    {
								elm.checked = checkVal; //checkRow(elm.id);
							}
						}
				}
			}
			
			function checkChildTry(id,pChkAllID,pChkSelName) 
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
							if ((e.name.substr(3)=="HubPending") || (e.name.substr(3)=="Reject") || (e.name.substr(3)=="HubApprove") || (e.name.substr(3)=="Active") || (e.name.substr(3)=="InActive") || (e.name.substr(e.name.length-6)=="chkAll") || (e.name.substr(3)=="Spot") || (e.name.substr(3)=="Stock") || (e.name.substr(3)=="MRO") || (e.name.substr(3)=="CustomPR") || (e.name.substr(3)=="RemarkPR"))
			       	        {
				            }
				            else
				            {
					            if (pButFunc=="unlock")
							        return confirm('Are you sure that you want to unlock this item(s)?');
					            if (pButFunc=="activate")
							        return confirm('Are you sure that you want to activate this item(s)?');
					            else if (pButFunc=="deactive")
							        return confirm('Are you sure that you want to deactivate  this item(s)?');
					            else if (pButFunc=="delete")
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
			function Reset(){
				var oform = document.forms(0);
				oform.txtHOBRCode.value="";
				oform.txtHOBRName.value="";
				oform.chkActive.checked="";
				oform.chkInactive.checked="";
				oform.ddlHOBR.value=0;
				oform.ddlCompType.value=0;
				oform.txtGLCode.value="";
				
			}
		
//			function Reset()
//			{
//			    var oform = document.forms(0);
//			    var a = document.getElementById('txtBranchCode');
//			    if(a)
//			    {
//				    a.value = "";
//			    }
//			    var b = document.getElementById('txtBranchName');
//			    if(b)
//			    {
//				    b.value = "";
//			    }		
//			    var c = document.getElementById('chkActive');
//			    if(c)
//			    {
//				    c.checked = false;
//			    }	
//			    var d = document.getElementById('chkInactive');
//			    if(d)
//			    {
//				    d.checked = false;
//			    }		   
//		    }		    
		</script>
</head>
<body class="body" ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        <table class="AllTable" id="table1"  cellspacing="0" cellpadding="0" width="100%">
            <tr>
			    <td class="Header" colspan="4" style="height: 19px"><asp:label id="lblTitle" runat="server" CssClass="header">Branch Code Maintenance</asp:label></td>
			</tr>			   
			<tr>
				<td class="EmptyCol" colspan="4">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Click the Add button to add new branch."></asp:label>
				</td>
			</tr>
			<tr><td class="rowspacing" style="width: 286px" colspan="4"></td></tr>	
			<tr>
			    <td class="TableHeader" colspan="4">Search Criteria</td>
			</tr>
			<tr>
			    <td class="TableCol" style="width:360px"><strong>&nbsp;<asp:Label runat="server" Text="Company" ID="lbl1" cssclass="lbl"></asp:Label><asp:Label Text=":" runat="server" ID="lbl2" /></strong>&nbsp;</td>
			    <td class="TableCol" style="width:360px"><strong>
			        <asp:dropdownlist runat="server" ID="ddlCompType" CssClass="ddl" Width = "26%">
			            <asp:ListItem Value="0" Text="--Select--" />
			            <asp:ListItem Value="1" Text="HLB" />
			            <asp:ListItem Value="2" Text="HLISB" /> 
			        </asp:dropdownlist></strong>
			    </td>
			    <td class="TableCol" style="width:360px"><strong>&nbsp;<asp:Label runat="server" Text="HO/BR" ID="Label8" cssclass="lbl"></asp:Label><asp:Label Text=":" runat="server" ID="Label9" /></strong>&nbsp;</td>
			    <td class="TableCol" style="width:360px"><strong>
			        <asp:dropdownlist runat="server" ID="ddlHOBR" CssClass="ddl" Width = "22%">
			            <asp:ListItem Value="0" Text="--Select--" />
			            <asp:ListItem Value="1" Text="HO" />
			            <asp:ListItem Value="2" Text="BR" /> 
			        </asp:dropdownlist></strong>
			    </td>
			</tr>
			<tr> 
			     <td class="TableCol" style="width: 360px" colspan=""><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="HO/BR Code"  CssClass="lbl"></asp:Label><asp:Label ID="Label2" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				 <td class="TableCol" style="width: 304px"><asp:textbox id="txtHOBRCode" runat="server" Width="210px" MaxLength="30" CssClass="txtbox"></asp:textbox></td>
                 <td class="TableCol" style="width: 360px"><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="HO/BR Abbreviation & Name"  CssClass="lbl"></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
				 <td class="TableCol"><asp:textbox id="txtHOBRName" runat="server" Width="300px" MaxLength="100" CssClass="txtbox"></asp:textbox></td>
            </tr>	
            <tr>
                 <td class="TableCol" style="height: 24px; width: 138px;"><strong>&nbsp;<asp:Label ID="Label10" runat="server" Text="GL Code" CssClass="lbl"></asp:Label><asp:Label ID="Label11" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
                 <td class="TableCol" style="height: 24px"><asp:TextBox ID="txtGLCode" runat="server" Width="210px" maxlength="11" CssClass="txtbox" /></td>
                 <td class="TableCol" style="height: 24px; width: 360px;"><strong>&nbsp;<asp:Label ID="Label5" runat="server" Text="Status" CssClass="lbl"></asp:Label><asp:Label ID="Label6" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong>&nbsp;</td>
                 <td class="TableCol" style="height: 24px; width: 304px;">
                 &nbsp;<asp:CheckBox ID="chkActive" runat="server" text="Active"/>&nbsp;<asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" /></td>
			</tr>	
			<tr>
			    <td class="TableCol" colspan="4" align="right">
			        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
					<input type="button" class="button" id="cmdClear" runat="server" onclick="Reset();" value="Clear" name="cmdClear"/>&nbsp;
				</td>
			</tr>
			<tr>
			    <td class="rowspacing" style="width: 360px"></td>
			</tr>
        </table>
        
        <table class="AllTable" id="table2"  cellspacing="0" cellpadding="0" width="100%">
         <tr>				   
			 <td class="EmptyCol">
			<%--This is how to make rowspacing inside TD tag--%>					    
				<div id="CoyBranch" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						<asp:datagrid id="dtgCoyBranch" runat="server">
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
								<asp:BoundColumn Visible="False">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cbm_coy_id" SortExpression="cbm_coy_id" HeaderText="Company">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cbm_branch_type" SortExpression="cbm_branch_type" HeaderText="HO/BR">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cbm_branch_code" SortExpression="cbm_branch_code" HeaderText="HO/BR Code">
									    <HeaderStyle Width="10%"></HeaderStyle>
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="cbm_branch_name" SortExpression="cbm_branch_name" HeaderText="HO/BR Abbreviation & Name">
									    <HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cbm_gl_code" SortExpression="cbm_gl_code" HeaderText="GL Code">
									    <HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cbm_STATUS" SortExpression="cbm_STATUS" HeaderText="Status">
									    <HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn> 
								<asp:BoundColumn DataField="cbm_branch_index" Visible="false" />   
							    </Columns>
						    </asp:datagrid>
				        </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblBranchCodeLabel" runat="server" Text="Branch Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblBranchNameLabel" runat="server" Text="Name" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="40%" ForeColor="Navy"></asp:Label>						    
		                            <asp:Label ID="lblBranchStatus" runat="server" Text="Status" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>						    
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

