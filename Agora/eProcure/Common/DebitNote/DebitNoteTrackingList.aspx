<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DebitNoteTrackingList.aspx.vb" Inherits="eProcure.DebitNoteTrackingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html>
<head>
    <title>DebitNoteTrackingList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PrintWindow as string = dDispatcher.direct("ExtraFunc","FramePrinting.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            dim PDFWindow as string = dDispatcher.direct("ExtraFunc","GeneratePDF.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalImage as string = "<IMG src=" & dDispatcher.direct("Plugins/images","i_Calendar2.gif")& " border=""0"">"
        </script> 
		<%response.write(Session("WheelScript"))%>
		<%response.write(Session("JQuery"))%>
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
			SelectAllG("dtgInvoice_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgInvoice_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);			
			oform.txtDocNo.value="";
			oform.txtVendorName.value="";
			oform.ddlCurr.selectedIndex=0; 
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
		-->
		</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_DnTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr>
				    <td colspan="5">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant new Debit Note"
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
					<td noWrap width="15%">&nbsp;<strong>Debit Note No.</strong> :</td>
					<td width="20%"><asp:textbox id="txtDocNo" runat="server" MaxLength="50" Width="150px" CssClass="txtbox"></asp:textbox></td>
					<td noWrap width="15%">&nbsp;<strong><asp:Label ID="lblField" runat="server" Text="Vendor Name"></asp:Label></strong> :</td>
					<td width="35%"><asp:textbox id="txtVendorName" runat="server" MaxLength="50" Width="150px" CssClass="txtbox"></asp:textbox>
					    <asp:textbox id="txtInvNo" runat="server" MaxLength="50" Width="150px" CssClass="txtbox"></asp:textbox></td>
					<td>&nbsp;</td>					
				</tr>
				<tr class="tablecol" id="tr1" runat="server">
				    <td>&nbsp;<strong>Currency</strong> :</td>
				    <td>
                        <asp:DropDownList ID="ddlCurr" CssClass="ddl" Width="150px" runat="server"></asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                    <td align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr class="tablecol" id="tr2" runat="server">
				    <td>&nbsp;<strong>Start Date</strong> :</td>
				    <td><asp:textbox id="txtDateFr" runat="server" Width="150px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%response.write(CalImage) %></a>
				    </td>
                    <td>&nbsp;<strong>End Date</strong> :</td>
                    <td><asp:textbox id="txtDateTo" runat="server" Width="150px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%response.write(CalImage) %></a></td>
                    <td align="right"><asp:button id="cmdSearch2" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear2" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5"></td>
				</tr>							
				<tr>
					<td class="emptycol" colspan="5"><asp:datagrid id="dtgDebitNote" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="DNM_DN_NO" HeaderText="Debit Note No.">
									<HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDnNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DNM_DN_DATE" SortExpression="DNM_DN_DATE" HeaderText="Debit Note Date">
								    <HeaderStyle Width="14%"/>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="DNM_INV_NO" HeaderText="Invoice No.">
									<HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
								    <HeaderStyle Width="25%"/>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNM_CURRENCY_CODE" SortExpression="DNM_CURRENCY_CODE" HeaderText="Currency">
								    <HeaderStyle Width="10%"/>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
									<HeaderStyle Width="15%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="DNM_DN_INDEX" SortExpression="DNM_DN_INDEX" HeaderText="Index"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False" Visible="false"></asp:button>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Mass Verify"
							CausesValidation="False" Visible="false"></asp:button>
						<asp:button id="cmdApprove" runat="server" Width="96px" CssClass="button" Text="Mass Approve"
							CausesValidation="False" Visible="false"></asp:button>
						<asp:button id="cmdMark" runat="server" Width="80px" CssClass="button" Text="Mark As Paid" CausesValidation="False" Visible="false"></asp:button>
						<input class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset" name="cmdReset" Visible="false" runat="server"/> 
					    <input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>
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
