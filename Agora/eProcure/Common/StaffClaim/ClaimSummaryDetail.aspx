<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ClaimSummaryDetail.aspx.vb" Inherits="eProcure.ClaimSummaryDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ClaimSummaryDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
        
        <script language="javascript">
        function confirmChanged(strmsg)
        {	
	    
	    ans=confirm(strmsg);
	//alert(ans);
	    if (ans){	
	    
		return true;
		    }
	    else
	    {
		
		return false;
		}
        }
        </script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Staff_Claim_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<%--<tr>
				<td class="emptycol" colspan="5">
					<asp:label id="Label4" runat="server"  CssClass="lblInfo"
					Text="Please select staff claim form from drop-down list. (e.g. Hardship Claim Form...)"></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="linespacing2" colspan="5"></td>
			</tr>--%>
			<tr style="display:none;">
			    <td class="emptycol" colspan="5">
					<asp:dropdownlist id="ddlSelect" runat="server" Width="180px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true" ></asp:dropdownlist>
			    </td>
			</tr>
			<%  Response.Write(Session("w_SC_Links"))%>
            <tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="5"></td>
			</tr>
			<tr id="trInfo" runat="server">
				<td colspan="5">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Click the Submit button to submit the staff claim."></asp:label>
				</td>
			</tr>
			<tr>
				<td class="linespacing2" colspan="5" ></td>
			</tr>
			<tr>
                    <td class="tableheader" WIdTH="100%" colspan="5">&nbsp;Staff Claim Header</td>
                </tr>
			<tr>
				<td class="tablecol" style="height:19px; width:18%;">&nbsp;<strong>Staff Claim No </strong>:</td>				
				<td class="tablecol" style="height:19px; width:25%;"><asp:Label id="lblSCNo" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" style="height:19px; width:8%;"></td>
				<td class="tablecol" style="height:19px; width:12%;"><strong>Status </strong>:</td>
				<td class="tablecol" style="height:19px; width:37%;"><asp:Label id="lblStatus" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px; width:18%;">&nbsp;<strong>User Name </strong>:</td>				
				<td class="tablecol" style="height:19px; width:25%;"><asp:Label id="lblUserName" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" style="height:19px; width:8%;"></td>
				<td class="tablecol" style="height:19px; width:12%;"><strong>Company </strong>:</td>
				<td class="tablecol" style="height:19px; width:37%;"><asp:Label id="lblCompName" runat="server" width="100%"></asp:Label></td>
			</tr>
			<tr>
				<td class="tablecol" style="height:19px; width:18%;">&nbsp;<strong>Business Division/Dept. </strong>:</td>				
				<td class="tablecol" style="height:19px; width:25%;"><asp:Label id="lblDept" runat="server" width="100%"></asp:Label></td>				
				<td class="tablecol" style="height:19px; width:8%;"></td>
				<td class="tablecol" style="height:19px; width:12%;"><strong>Document Date </strong>:</td>
				<td class="tablecol" style="height:19px; width:37%;"><asp:Label id="lblDocDate" runat="server" width="100%"></asp:Label></td>
			</tr>
			</table>
			<table class="alltable" id="tbApp" cellspacing="0" width="100%" cellpadding="0" border="0" runat="server">
			    <tr>
				    <td class="emptycol"></td>
			    </tr>
			    <tr>
					<td class="tableheader">&nbsp;Approval Workflow</td>
				</tr>
				<tr>
				    <td width="100%">
				        <asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" Width="100%">
				            <Columns>
				                <asp:BoundColumn DataField="SCA_SEQ" HeaderText="Level">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
				            </Columns>
				        </asp:datagrid>
				    </td>
				</tr>
			</table>
			<table class="alltable" id="Table3" cellspacing="0"width="100%"  cellpadding="0" border="0">
			    <tr>
				    <td class="emptycol" colspan="4"></td>
			    </tr>
			    <tr>
				    <td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;Staff Claim Details</td>
			    </tr>
				<tr>
					<td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;TRAVEL</td>
				</tr>
				<tr>
					<td class="tablecol" align="left" style="width:25%; height: 19px;"></td>
					<td class="tablecol" align="center" style="width:25%; height: 19px;"><u><strong>Local</strong></u></td>
					<td class="tablecol" align="center" style="width:25%; height: 19px;"><u><strong>Overseas</strong></u></td>
					<td class="tablecol" align="center" style="width:25%; height: 19px;"></td>
				</tr>
				<tr>
					<td class="tablecol" align="left" style="width:25%; height: 19px;">&nbsp;<strong>Mileage Claims</strong> :</td>
					<td class="TableInput" align="center" style="width:25%; height: 19px;"><asp:Label ID="lbl_N_Local_MC" runat="server"></asp:Label></td>
					<td class="TableInput" align="center" style="width:25%; height: 19px;"><asp:Label ID="lbl_N_Oversea_MC" runat="server"></asp:Label></td>
					<td class="TableInput" align="center" style="width:25%; height: 19px;"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Parking</strong> :</td>
					<td class="TableInput" align="center"><asp:Label ID="lbl_N_Local_PK" runat="server"></asp:Label></td>
					<td class="TableInput" align="center"><asp:Label ID="lbl_N_Oversea_PK" runat="server"></asp:Label></td>
					<td class="TableInput"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Public Transport Claims</strong> :</td>
					<td class="TableInput" align="center"><asp:Label ID="lbl_N_Local_PT" runat="server"></asp:Label></td>
					<td class="TableInput" align="center"><asp:Label ID="lbl_N_Oversea_PT" runat="server"></asp:Label></td>
					<td class="TableInput"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Subsistance Allowance</strong> :</td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Local_SA" runat="server"></asp:Label></td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Oversea_SA" runat="server"></asp:Label></td>
					<td class="tableinput"></td>
				</tr>						
				<tr>
					<td class="tablecol">&nbsp;<strong>Accommodation Claims</strong> :</td>
					<td class="TableInput" align="center"><asp:Label ID="lbl_N_Local_AC" runat="server"></asp:Label></td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Oversea_AC" runat="server"></asp:Label></td>
					<td class="TableInput"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Accommodation Allowance</strong> :</td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Local_AA" runat="server"></asp:Label></td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Oversea_AA" runat="server"></asp:Label></td>
					<td class="tableinput"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Airfare</strong> :</td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Local_AF" runat="server"></asp:Label></td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Oversea_AF" runat="server"></asp:Label></td>
					<td class="tableinput"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Toll</strong> :</td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Local_TL" runat="server"></asp:Label></td>
					<td class="tableinput" align="center"><asp:Label ID="lbl_N_Oversea_TL" runat="server"></asp:Label></td>
					<td class="tableinput"></td>
				</tr>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Transportation Claims</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Misc_TC" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Storage Allowance</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Misc_SA" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;OTHER CLAIMS</td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Medical Claims</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Misc_MC" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Dental</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Misc_Dental" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol" style="height: 19px">&nbsp;<strong>Laundry</strong> :</td>
					<td class="tableinput" style="height: 19px"><asp:Label ID="lbl_Misc_Laundry" runat="server"></asp:Label></td>
					<td class="tablecol" style="height: 19px"></td>
					<td class="tableinput" style="height: 19px"></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Standby Allowance</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Standby" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Meal Allowance</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_MA" runat="server"></asp:Label></td>
				</tr>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Entertainment Claims</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Ent" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Co. H/phone Off. Calls</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_CHP" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Shift Allowance</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Shift" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Personal H/phone Off. Calls</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_PHP" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Stationery</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Stationery" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Handphone Subsidy</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_HPS" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Postage</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Postage" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Data Plan</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_DP" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;WIP</td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Mileage</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_MC" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Accom. Allw. = Local</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_AA" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Accommodation</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_AC" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Accom. Allw. = Overseas</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Oversea_AA" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Parking</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_PK" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Entertainment</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Ent" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Toll</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_TL" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Handphone</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_HP" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Public Transport</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_PT" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Stationery</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Stationery" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling Local - Airfare</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Local_AF" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Standby Allow.</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Standby" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Travelling - Overseas</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Oversea_Amt" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Subsistance Allow.</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_SA" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Others</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_Others" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>WIP - Hardship</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Y_HS" runat="server"></asp:Label></td>
				</tr>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Hardship Claims</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_HS" runat="server"></asp:Label></td>
					<td class="tablecol"></td>
					<td class="tableinput"></td>
				</tr>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Overtime</strong> : 1.5 times x (HH:mm)</td>
					<td class="tableinput"><asp:Label ID="lbl_OT_A" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Overtime</strong> : 2.0 times x (HH:mm)</td>
					<td class="tableinput"><asp:Label ID="lbl_OT_B" runat="server"></asp:Label></td>
				</tr>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Others</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Others" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Gifts</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_Gifts" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Attendance Performance Reward</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_N_APR" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Smart Pay</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_SP" runat="server"></asp:Label></td>
				</tr>
				<%--mimi : 20/03/2017 - enhancement smart pay ref. --%>
				<tr>
					<td class="tablecol">&nbsp;</td>
					<td class="tableinput"><asp:Label ID="Label1" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>Smart Pay Ref</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_SP_REF" runat="server"></asp:Label></td>
				</tr>
				<%--end--%>
				<tr>
				    <td class="tablecol" style="HEIGHT: 6px" colspan="4"><hr/></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>LESS: Travelling Advance Taken</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_TAT" runat="server"></asp:Label></td>
					<td class="tablecol">&nbsp;<strong>TOTAL</strong> :</td>
					<td class="tableinput"><asp:Label ID="lbl_Total_Amt" runat="server"></asp:Label></td>
				</tr>
			    <tr>
				    <td class="emptycol"></td>
			    </tr>
			    <tr>
				    <td colspan="4"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>
				    </td>
    			
		        </tr>
		        <tr>
			        <td class="emptycol" valign="middle" align="center"></td>
		        </tr>
		        <tr>
					<td class="emptycol">
						<p><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></p>
					</td>
				</tr>
    	    </table>
	    </form>
    </body>
</html>
