<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Menu1.aspx.vb" Inherits="eProcure.Menu2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Menu</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
		<!--
			
			function Display()
			{
				var tree = document.getElementById("tree");
				var t = document.getElementById("label");
				
				var image = document.getElementById("imgMenu");
				//var arrow = document.getElementById("arrow1");
				if(tree.style.display == "none"){
					tree.style.display ="";
					t.style.display ="";
					parent.second.cols = "265,*";
					image.lowsrc = 'images/Arrowup.gif';
					image.src = 'images/Arrowup.gif';
					
						
				}
				else{		
					parent.menu.scrolling="no";
					tree.style.display ="none";
					t.style.display ="none";
					parent.second.cols = "50,*";
					image.lowsrc = 'images/arrowdown.gif';
					image.src = 'images/arrowdown.gif';
					//lbl1.visible = "true";	
				}
				//image.style.display = "";
				//arrow.style.display = "none";
			}
			
			//-->
		</script>
	</HEAD>
	<BODY style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
		bgColor="#fff8dd" leftMargin="0" topMargin="0" ms_positioning="GridLayout">
		<FORM id="Form1" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
			method="post" runat="server">
			<TABLE id="Table1" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
				height="100%" cellSpacing="0" cellPadding="0" bgColor="#fff8dd" border="0" runat="server">
				<TR style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px">
					<TD style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
						vAlign="top">&nbsp;&nbsp;</TD>
					<TD style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
						vAlign="top"><IMG id="imgMenu" style="CURSOR: hand" alt="Click here to expand the menu" src="images/Arrowup.gif"
							width="19" border="0" runat="server" lowsrc="images/arrowdown.gif">
						<DIV id="label" style="DISPLAY: none">
							<DIV id="tree" style="DISPLAY: none">
								<IEWC:TREEVIEW id="TreeView1" runat="server" height="100%" backcolor="#FFF8DD" selectexpands="True"
									selectedstyle="color:#FFF8DD;background-color:black;" hoverstyle="color:Black;background-color:#cccccc;"
									indent="0" defaultstyle="font-family:Verdana;fore-color:black;font-size:11px;" bordercolor="Black"
									width="250px">
									<iewc:TreeNode ImageUrl="images/CodeSetup.gif" Text="Start" ID="startMenu" Target="main">
										<iewc:TreeNode Text="Catalogue Search" ID="SCat_Search"></iewc:TreeNode>
										<iewc:TreeNode Text="Favourites/Buyer Catalogue" ID="SFavourites"></iewc:TreeNode>
										<iewc:TreeNode Text="Shopping Cart" ID="SShopCart"></iewc:TreeNode>
										<iewc:TreeNode Text="RFQ" ID="SRFQs" NavigateUrl="" Target="body"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/enter.gif" Text="Manage" ID="ManageMenu">
										<iewc:TreeNode Text="Purchase Requisition" ID="MPurchaseRequis"></iewc:TreeNode>
										<iewc:TreeNode Text="PR Approval" ID="MPRApproval"></iewc:TreeNode>
										<iewc:TreeNode Text="PR Approved List" ID="MPRApprList"></iewc:TreeNode>
										<iewc:TreeNode Text="PR Consolidation" ID="MPRConsol"></iewc:TreeNode>
										<iewc:TreeNode Text="Purchase Order" ID="MPO"></iewc:TreeNode>
										<iewc:TreeNode Text="PO Cancellation Request" ID="MPOCancelReq"></iewc:TreeNode>
										<iewc:TreeNode Text="GRN Generation" ID="MGenGRN"></iewc:TreeNode>
										<iewc:TreeNode Text="GRN Acknowledgement" ID="MAckGRN"></iewc:TreeNode>
										<iewc:TreeNode Text="Invoice Tracking" ID="MInvoiceTrack"></iewc:TreeNode>
										<iewc:TreeNode Text="Transaction Tracking" ID="MTransTrack"></iewc:TreeNode>
										<iewc:TreeNode Text="View All PR" ID="MVAllPR"></iewc:TreeNode>
										<iewc:TreeNode Text="View All PO" ID="MVAllPO"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/SysAudit.gif" Text="Orders" ID="OrderMenu">
										<iewc:TreeNode Text="Order List" ID="OOrderList"></iewc:TreeNode>
										<iewc:TreeNode Text="Request For Quotation" ID="ORFQ"></iewc:TreeNode>
										<iewc:TreeNode Text="Delivery Order" ID="ODO"></iewc:TreeNode>
										<iewc:TreeNode Text="Invoice" ID="OInvGen"></iewc:TreeNode>
										<iewc:TreeNode Text="PO Cancellation Acknowledgement" ID="OPOCanlAck"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Report" ID="ReportMenu"></iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Personal Setting" ID="PersonalSettingMenu">
										<iewc:TreeNode Text="Personal Details" ID="PSPersonalSet"></iewc:TreeNode>
										<iewc:TreeNode Text="Favourite List Maintenance" ID="PSFavListMaint"></iewc:TreeNode>
										<iewc:TreeNode Text="Vendor List (RFQ) Maintenance" ID="PSVenListRFQ"></iewc:TreeNode>
										<iewc:TreeNode Text="Configure Default Values" ID="PSConfDefaultValue"></iewc:TreeNode>
										<iewc:TreeNode Text="Relief Staff Assignment" ID="PSReliefStaffAssig"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Company Setup" ID="ComSetUpMenu">
										<iewc:TreeNode Text="Company Details" ID="CSComDetails"></iewc:TreeNode>
										<iewc:TreeNode Text="Company Parameters" ID="CSComParam"></iewc:TreeNode>
										<iewc:TreeNode Text="Delivery Address" ID="CSDeliveryAddr"></iewc:TreeNode>
										<iewc:TreeNode Text="Billing Address" ID="CSBillingAddr"></iewc:TreeNode>
										<iewc:TreeNode Text="Department" ID="CSDepartment"></iewc:TreeNode>
										<iewc:TreeNode Text="Custom Fields" ID="CSCustomField"></iewc:TreeNode>
										<iewc:TreeNode Text="Exchange Rate" ID="CSExchangeRate"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="User Account Maintenance" ID="UserAcctMainMenu"></iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Approval Setup" ID="ApprSetUpMenu">
										<iewc:TreeNode Text="Approval Workflow" ID="ASWorkFlow"></iewc:TreeNode>
										<iewc:TreeNode Text="Approval Group Assisgnment" ID="ASGroupAssign"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Catalogue" ID="CatalogueUpMenu">
										<iewc:TreeNode Text="List Price Catalogue" ID="CListPriceCat"></iewc:TreeNode>
										<iewc:TreeNode Text="Contract Catalogue Maintenance" ID="CContrtCatMain"></iewc:TreeNode>
										<iewc:TreeNode Text="Discount Catalogue" ID="ClDiscCat"></iewc:TreeNode>
										<iewc:TreeNode Text="Contract Catalogue" ID="CContractCat"></iewc:TreeNode>
										<iewc:TreeNode Text="Batch Upload/Download" ID="CBatchUploadDownload"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Vendor And Catalogue" ID="VenCatMenu">
										<iewc:TreeNode Text="Buying Activity" ID="VCBuyingActivity"></iewc:TreeNode>
										<iewc:TreeNode Text="Approved Vendor" ID="VCApprVendor"></iewc:TreeNode>
										<iewc:TreeNode Text="Buyer Catalogue" ID="VCBuyerCat"></iewc:TreeNode>
										<iewc:TreeNode Text="Buyer Item Code Assignment" ID="VCBICAssign"></iewc:TreeNode>
										<iewc:TreeNode Text="Contract Catalogue Approval" ID="VCContractCatAppr"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Budget Control Management" ID="BudgetControlMgn">
										<iewc:TreeNode Text="General Setting" ID="BCMGeneralSet"></iewc:TreeNode>
										<iewc:TreeNode Text="Account Setup" ID="BCMAccCodeSet"></iewc:TreeNode>
										<iewc:TreeNode Text="BCM Assignment" ID="BCMAssign"></iewc:TreeNode>
										<iewc:TreeNode Text="View Budget" ID="BCMViewBudget"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/Verify.gif" Text="Admin">
										<iewc:TreeNode Text="Department" ID="ds" NavigateUrl="Admin/DeptSetup.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Delivery Address" ID="d" NavigateUrl="Admin/AddressMaster.aspx?type=D&mod=T"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Billing Address" ID="b" NavigateUrl="Admin/AddressMaster.aspx?type=B&mod=T"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Buying Activity" ID="b11" NavigateUrl="Admin/BuyActivity.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Company Param(B)" ID="b12" NavigateUrl="Admin/BComParam.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Company Param(V)" ID="b112" NavigateUrl="Admin/BComVendor.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Company Profile(V)" ID="b132" NavigateUrl="companies/coCompanyDetail.aspx?mode=V"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Company Profile(B)" ID="b13" NavigateUrl="companies/coCompanyDetail.aspx?mode=B"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Catalogue Search(PM)" ID="b131" NavigateUrl="Product/SearchCatalogue.aspx?role=PM"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Catalogue Search(B)" ID="b1311" NavigateUrl="Product/SearchCatalogue.aspx?role=B"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Fav List" ID="b11311" NavigateUrl="PersonalSetting/Favs_ListMain.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="config default value" ID="bfw1s311" NavigateUrl="admin/ConfigDefValue.aspx"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="custom field" ID="b1w1ffs311" NavigateUrl="admin/CustomFields.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="DO" ID="b1w1s3f11" NavigateUrl="do/searchDO.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="RFQ" ID="b1fw1s3f11" NavigateUrl="rfq/Create_RFQ.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="exchange rate" ID="bd1efw1s3f11" NavigateUrl="admin/ExchangeRate.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="New Requisitions" ID="bd1fwe1s3f11" NavigateUrl="PO/SearchPR_ao.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Approved List" ID="cwcw" NavigateUrl="PO/SearchPR_all.aspx?caller=ao" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="View PR(B)" ID="cwc" NavigateUrl="PO/SearchPR_all.aspx?caller=buyer" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="View PR(BA)" ID="c1wc" NavigateUrl="PO/SearchPR_All.aspx?caller=admin" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Consolidate PR" ID="h" NavigateUrl="PO/PRConsolidation.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Buyer Cat" ID="hrr" NavigateUrl="BuyerCat/BuyerCatalogue.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Fav List/Buyer Cat Maint" ID="hrr1" NavigateUrl="PersonalSetting/Favs_BuyerList.aspx"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Buyer Item Code" ID="hrr11" NavigateUrl="admin/Buyer_ItemCode.aspx" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Raise PR" ID="h2rr11" NavigateUrl="pr/viewShoppingCart.aspx?type=tab" Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Approved Vendor" ID="h2rr1w1" NavigateUrl="admin/approvedvendor.aspx?type=AV"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Approve Work Flow" ID="h2xrr1w1" NavigateUrl="apprworkflow/ApprovalWorkFlow.aspx"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Approve Work Flow Assignment" ID="h2xrr1ww1" NavigateUrl="apprworkflow/AppGrpAsg.aspx"
											Target="body"></iewc:TreeNode>
										<iewc:TreeNode Text="Transaction Tracking" ID="h2xrd1ww1" NavigateUrl="tracking/TransTracking.aspx"
											Target="body"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode text="Comp. Data Setup" ImageUrl="images/icon_toolbox.gif">
										<iewc:TreeNode text="Company Details" NavigateUrl="Companies\coCompanyDetail.aspx?mode=modify"
											target="body"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode text="User Data Setup" ImageUrl="images/icon_toolbox.gif">
										<iewc:TreeNode text="User Acc. Maint" NavigateUrl="Users\usSearchUser.aspx" target="body"></iewc:TreeNode>
									</iewc:TreeNode>
									<iewc:TreeNode ImageUrl="images/icon_toolbox.gif" Text="Maint" ID="Mantianent" Target="_top"></iewc:TreeNode>
									<iewc:TreeNode NavigateUrl="Login.aspx" ImageUrl="images/logout.gif" Text="Logout" ID="logout"
										Target="_top"></iewc:TreeNode>
								</IEWC:TREEVIEW></DIV>
						</DIV>
						<ASP:PANEL id="Panel1" runat="server" backcolor="#FFF8DD" height="100%"></ASP:PANEL></TD>
				</TR>
			</TABLE>
		</FORM>
	</BODY>
</HTML>
