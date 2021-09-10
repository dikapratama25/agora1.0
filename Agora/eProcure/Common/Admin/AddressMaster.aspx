<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddressMaster.aspx.vb" Inherits="eProcure.AddressMaster" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Billing_Address</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
        
		<script language="javascript">
		<!--
			//debugger;
			function selectAll()
			{
				SelectAllG("dtgAddress_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgAddress_ctl02_chkAll","chkSelection");
			}
			
			function ResetSearch(){
				//debugger;
				Form1.txt_Code.value = "";
				Form1.txt_Address.value = "";
				Form1.txt_City.value = "";
				Form1.cbo_State.selectedIndex = 0;
				Form1.cbo_Country.selectedIndex = 0;
				
			}

			function Test()
			{
				alert(window.opener.document.Form1.ddl.options[0].value);
				window.opener.document.Form1.ddl.selectedIndex=2;
				var r =eval("window.opener.document.Form1." + document.Form1.hidID.value);
				alert(r.selectedIndex);
				//r.selectedIndex=document.Form1.j.value;
				alert(window.opener.document.Form1.dtgSelect_ctl02_ddl2.selectedIndex);
				window.close();				
			}

			function Select()
			{	
				if (document.Form1.hidMode.value=='all')
				{
					selectAllItem(document.Form1.hidID.value,document.Form1.hidItem.value);
					}
				else{
					selectOne();
					}
			}

			function selectOne()
			{
//				var r =eval("window.opener.document.Form1." + document.Form1.hidID.value);
//				alert( document.Form1.hidID.value);
//				alert(document.Form1.hidItem.value);
//				var i;
//				for (i=0; i<r.length; i++){
//				alert("come"); //dtgShopping_ctl02_txtDelivery document.Form1.hidID.value
//					if (r.options[i].value == document.Form1.hidItem.value){
//						r.selectedIndex = i;	
//					}					
//				}
//				window.close();

                var r = (eval("window.opener.document.Form1." + document.Form1.hidID.value));
                r.value = document.Form1.hidItem.value;
                var lmtAddrDesc = document.Form1.hidAddrDesc.value;
                window.opener.document.getElementById(document.Form1.hidAddrId.value).innerHTML = lmtAddrDesc.substring(0,9);
                window.opener.document.getElementById(document.Form1.hidAddrId.value).title = lmtAddrDesc;
                window.close();
			}
			
			function selectAllItem(val,v)
			{ 
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp('$' ) 
				for (var i=0;i<oform.elements.length;i++)
				{
				    var foo = "$";
					var e = oform.elements[i];
					var sEvents = e.name;
					if (sEvents.indexOf("$") > 0)
					{
						if (sEvents.substring(sEvents.lastIndexOf("$")+1) == "hidDelCode")
					    {
					        var lmtAddrDesc = document.Form1.hidAddrDesc.value;
					        var sDelivery = sEvents.substring(0, sEvents.indexOf("$")) + "_" + sEvents.substring(sEvents.indexOf("$")+1 , sEvents.lastIndexOf("$")) + "_txtDelivery";
					        
               
					        window.opener.document.getElementById(sDelivery).innerHTML = lmtAddrDesc.substring(0,9);
					        window.opener.document.getElementById(sDelivery).title = lmtAddrDesc;
					        var r = (eval("window.opener.document.Form1." + sEvents));
                            r.value = document.Form1.hidItem.value;
					    }
					    
					    //alert (sEvents + ": " + sEvents.substring(sEvents.lastIndexOf("$")+1));
					    //to get dtgShopping      alert ("dtgShopping: " + sEvents.substring(0, sEvents.indexOf("$")));
					    //alert ("current looping: " + sEvents.substring(sEvents.indexOf("$")+1, sEvents.substring(sEvents.indexOf("$") + 1)));
					    //sEvents.substring(sEvents.indexOf("$") + 1)
					    //to get dtgShopping sEvents.substring(0, sEvents.indexOf("$"));
					    //to get current looping sEvents.substring(sEvents.indexOf("$"), sEvents.substring(sEvents.indexOf("$") + 1));
					    //to get last after $ alert(sEvents + ": " + sEvents.substring(sEvents.lastIndexOf("$")+1));
					    //dtgShopping$ctl02$hidDelCode
					    //dtgShopping_ctl02_txtDelivery
					}
					
//					if (e.type=="select-one" && re.test(e.name)){
//						if (Form1.hidIndex.value == ''){
//							for (j=0; j<e.length; j++){
//								if (e.options[j].value == v){
//									e.selectedIndex = j;
//									Form1.hidIndex.value = j;
//								}	
//							}
//						}
//						else
//							e.selectedIndex = Form1.hidIndex.value;
//					}
                    //window.opener.document.getElementById(document.Form1.hidAddrId.value).innerHTML = lmtAddrDesc.substring(0,9);
                  
				}
				window.close();
			}

			function Chk(hidId, hidDev)
			{
				document.Form1.hidItem.value=hidId;
				document.Form1.hidAddrDesc.value=hidDev;

			}

		-->
        </script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Address_tabs"))%>
			<DIV id="hiddiv" style="DISPLAY: none" runat="server">
			<table>
			<tr>
					<TD class="header" style="HEIGHT: 19px"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
            </tr>
            </table>
			</DIV>
			<DIV id="hidAction" style="DISPLAY: inline" runat="server">
			<table class="alltable" id="Table11" cellSpacing="0" cellPadding="0" border="0">
			<tr>
					<TD class="linespacing1" colSpan="4" ></TD>
            </tr>
				<TR>
					<TD align="left" colSpan="4" >
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text=""></asp:label>
                        
					</TD>
				</TR>
            <tr>
					<TD class="linespacing2" colSpan="6" ></TD>
			</TR>
            </table>
			</DIV>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="6">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" width="10%">&nbsp;<STRONG>Code</STRONG>: </TD>
								<TD class="TableInput"  width="30%"><asp:textbox id="txt_Code" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox></TD>
								<td class="tablecol"  width="5%"></td>
								<td class="tablecol"  width="15%">&nbsp;<strong>Address</strong>: </td>
								<td class="TableInput"  width="30%"><asp:TextBox ID="txt_Address" Runat="server"  CssClass="txtbox" MaxLength="20" Width="156px"></asp:TextBox>&nbsp;
								</td>
								<td class="tablecol"  width="10%"></td>
							</TR>
							<tr>
								<td class="tablecol">
									&nbsp;<STRONG>City</STRONG>:</td>
								<TD class="TableInput"  >	<asp:textbox id="txt_City" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox>&nbsp
								<td class="tablecol">
								<td class="tablecol" valign="top">
									&nbsp;<STRONG>State</STRONG>:</td>
								<TD class="tablecol"  >	<asp:dropdownlist id="cbo_State" runat="server" CssClass="ddl" Width="156px" ></asp:dropdownlist>&nbsp;
								</td>
								<td class="tablecol">
							</tr>
							<tr>
							    <td class="tablecol" valign="top">&nbsp;<STRONG>Country</STRONG>:</td>
								<TD class="tablecol"  ><asp:dropdownlist id="cbo_Country" runat="server" AutoPostBack="True" CssClass="ddl" Width="156px" ></asp:dropdownlist>&nbsp;</td>
								<td class="tablecol" colspan="3" ></td>
								<td class="tablecol" align="left">
									<asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search">
									</asp:button><input class="button" id="cmd_Clear" onclick="ResetSearch();" type="button" value="Clear">
								</td>
							</tr>
						</TABLE>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgAddress" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label id="lblSelection" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="CustomerID">
									<ItemTemplate>
										<asp:Label ID="lblAddrCode" Text='<%# DataBinder.Eval(Container.DataItem,"AM_ADDR_CODE") %>' Runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="AM_ADDR_CODE" HeaderText="Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkAddrCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="AM_Addr_Code" SortExpression="AM_Addr_Code"  readonly="true"   
									HeaderText="Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Address" SortExpression="Address"  readonly="true"    HeaderText="Address">
									<HeaderStyle Width="29%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_CITY" SortExpression="AM_CITY"  readonly="true"    HeaderText="City">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATE" SortExpression="STATE"  readonly="true"    HeaderText="State">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_POSTCODE" SortExpression="AM_POSTCODE"  readonly="true"   HeaderText="Post Code">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COUNTRY" SortExpression="COUNTRY"  readonly="true"    HeaderText="Country">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR id="trT" runat="server">
					<TD><asp:button id="cmd_Add" runat="server" CssClass="Button" Text="Add"></asp:button>&nbsp;<asp:button id="cmd_Modify" runat="server" CssClass="Button" Text="Modify" Enabled="False"></asp:button>&nbsp;<asp:button id="cmd_Delete" runat="server" CssClass="Button" Text="Delete" Enabled="False"></asp:button>&nbsp;<input class="button" id="cmdReset" onclick="DeselectAllG('dtgAddress_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY:none"></TD>
				</TR>
				<TR id="trP" runat="server">
					<TD><input class="button" id="cmdSelect" disabled onclick="Select();" type="button" value="Save"
							name="cmdSelect" runat="server">&nbsp; 
							<input class="button" id="cmdClose" onclick="window.close();" type="button" value="Close" name="cmdClose" runat="server">
							<input id="hidID" type="hidden" size="1" name="hidID" runat="server" /> 
							<input id="hidMode" type="hidden" size="2" name="hidMode" runat="server" /> 
							<input id="hidItem" type="hidden" size="1" name="Hidden1" runat="server" /> 
							<input id="hidIndex" type="hidden" size="1" name="Hidden1" runat="server" />
							<input id="hidAddrId" type="hidden" size="1" name="hidAddrId" runat="server" /> 
							<input id="hidAddrDesc" type="hidden" size="1" name="hidAddrDesc" runat="server" /> 
							</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
