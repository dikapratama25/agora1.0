<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryLocMaint.aspx.vb" Inherits="eProcure.InventoryLocMaint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Location_Maintenance</title>
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
		<!--
			//debugger;
			function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=500,height=300,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}		
			
			function selectAll()
			{
				SelectAllG("dtgLocation_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgLocation_ctl02_chkAll","chkSelection");
			}
								
			function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		-->
        </script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Address_tabs"))%>	
			<table class="alltable" id="Table11" cellSpacing="0" cellPadding="0" width="100%">			   
			    <TR>
				    <TD class="Header" colSpan="2"><asp:label id="lblTitle" runat="server" CssClass="header">Storage Location Maintenance/Setup</asp:label></TD>
			    </TR>			   
			    <TR>
				    <TD class="EmptyCol" >
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the Add button to add new location."></asp:label>
				    </TD>
			    </TR>
               <TR>				   
				    <TD class="EmptyCol">
					    <%--This is how to make rowspacing inside TD tag--%>					    
					    <div id="Loc" style="DISPLAY: none" runat="server">
					        <div class="rowspacing"></div>
						    <asp:datagrid id="dtgLocation" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="LM_LOCATION_INDEX">
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
								    <asp:BoundColumn Visible="False" DataField="LM_LOCATION_INDEX" SortExpression="LM_LOCATION_INDEX" HeaderText="LM_LOCATION_INDEX">
									    <HeaderStyle Width="15%"></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle Width="40%"></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
									    <HeaderStyle Width="40%"></HeaderStyle>
								    </asp:BoundColumn>
							    </Columns>
						    </asp:datagrid>
				        </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblLocLabel" runat="server" Text="Location" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="40%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblSubLocLabel" runat="server" Text="Sub Location" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="50%" ForeColor="Navy"></asp:Label>						    
		                        </div>			                                                       
                                <asp:Label ID="Label1" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>				       
				    </TD>
				</TR>    
				    
				<tr>
					<td class="EmptyCol">
					    <asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;
					    <asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" ></asp:button>&nbsp;
					    <asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					    <asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"></asp:button> 

					</td>
				</tr>	
				<tr><td class="rowspacing"></td></tr>			 
				 <tr><td class="rowspacing"></td></tr>
			    <TR>
				    <TD class="EmptyCol" >
					    <asp:label id="Label2" runat="server"  CssClass="lblInfo"
					    Text="Click the Update button below to change location and sub location label description."></asp:label>
				    </TD>
			    </TR>
				<tr>
					<td class="EmptyCol">
					     <asp:button id="cmdUpdate" runat="server" CssClass="button" Text="Update" CausesValidation="False"></asp:button>
					</td>
				</tr>
            </table>	
					
			
		</form>
				
	</body>
</HTML>
