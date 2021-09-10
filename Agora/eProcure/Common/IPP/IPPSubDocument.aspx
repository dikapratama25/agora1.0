<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPSubDocument.aspx.vb" Inherits="eProcure.IPPSubDocument" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<HEAD>
		<title>Add Sub-Document</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
         <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+txtVal+'&seldate='+txtVal.value+'")
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.write(Session("typeahead")) %>
		
		<script type="text/javascript">
		<!--
		  function popCalendar(val)
		    {
		        txtVal = val
		        window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			}
			function GetClientId(strid)
                {
                     var count=document.forms[0].length;
                     var i=0;
                     var eleName; 
                     for (i=0; i < count; i++ )
                     {
                       eleName=document.forms[0].elements[i].id; 
                       pos=eleName.indexOf(strid);
                       if(pos>=0)  break;            
                     }
                    return eleName;
               }
            function isNumberCharKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122))
                    return false;

                 return true;
            }  

           	function isDecimalKey(evt)
                {
                     var charCode = (evt.which) ? evt.which : event.keyCode
                     if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                        return false;
                     return true;
                }  
          -->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="color: #000000">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="table1" cellSpacing="0" cellPadding="0" width="490px" border="0">
				<tr id="hiddtg_freeform" runat="server" class="alltable">
					<td class="emptycol" style="width:504px">
					<asp:datagrid id="dtg_SubDoc" runat="server" CssClass="Grid" AutoGenerateColumns="False">
							<Columns>
							    <asp:TemplateColumn HeaderText="S/NO" ItemStyle-HorizontalAlign="Right">	
							        <HeaderStyle Width="2px"></HeaderStyle>											                
							                <ItemTemplate>
                                                <asp:Label ID="LineNo" runat="server"></asp:Label>
                                                <asp:Label ID="index" runat="server" />
							                </ItemTemplate>
                                </asp:TemplateColumn>	
								<asp:TemplateColumn HeaderText="Document No *">
									<HeaderStyle Width="250px"></HeaderStyle>
									<ItemTemplate>
                                        <%--Jules 2018.07.11 - PAMB to allow "\" and "#"--%>
										<%--<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" style="margin-right:0px;width:100%" Rows="3" onkeypress="return isNumberCharKey(event);"></asp:TextBox>--%>
                                        <asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" style="margin-right:0px;width:100%" Rows="3"></asp:TextBox>
                                        <%--End modification.--%>

                                        <asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Headertext = "Document Date">
								<HeaderStyle Width="118px"></HeaderStyle>
							        <ItemTemplate>
							            <input id="txtdocDate" runat="server" style="margin-right:0px;width:80%" class="txtbox" readonly="readonly"/>
                                        <a style="margin-right:0px" onclick="popCalendar('<%#CType(Container,DataGridItem).FindControl("txtdocDate").ClientID%>');" href="javascript:;"><% Response.Write(sCal)%></a> 
                                        </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amount" ItemStyle-Width="80px">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemTemplate>
										<asp:textbox id="txtAmount" style="margin-right:0px;width:100%" runat="server" CssClass="numerictxtbox" autopostback="true"/>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<%--Zulham Aug 27, 2014--%>
								<asp:TemplateColumn HeaderText="GST Amount" ItemStyle-Width="80px">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:textbox id="txtGSTAmount" style="margin-right:0px;width:100%" runat="server" CssClass="numerictxtbox" autopostback="true"/>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<%--End--%>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
				    <td style="width:504px;border:none">
				        <table id="table44" class="alltable" width="490px" border="0">
                            <tr style="border:0">
                                <td style ="width:450px;"></td>
                                <td class = "emptycol" style="width: 80px; text-align:right; font-weight:bold; color:Black">Total :</td>
                                <td class = "emptycol" style="width: 100px;" align="center"> 
                                    <hr/>
                                        <asp:Label BorderColor="white" BorderStyle="None" ID="lblAmt" runat="server" Forecolor="black"/>
                                    <hr/>
                                </td>
                            </tr>
                             <tr style="border:0" id="trGSTAmount" runat="server">
                                <td style ="width:450px;"></td>
                                <td class = "emptycol" style="width: 80px; text-align:right; font-weight:bold; color:Black"><asp:Label ID="lbl1" runat="server" /></td>
                                <td class = "emptycol" style="width: 100px;" align="center"> 
                                    <hr/>
                                        <asp:Label BorderColor="white" BorderStyle="None" ID="lblGSTAmt" runat="server" Forecolor="black"/>
                                    <hr/>
                                </td>
                            </tr>
                        </table>
				    </td>
				</tr>

				<tr>
					<td class="emptycol" style="HEIGHT: 19px; width: 504px;"><asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save" ></asp:button>&nbsp;
                        <asp:Button ID="cmd_back" runat="server" CssClass="button" Text="Cancel" />
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="width: 504px"><asp:validationsummary id="ValidationSummary1" runat="server" CssClass="errormsg" Height="24px"></asp:validationsummary><asp:label id="lbl_check" runat="server" ForeColor="Red" CssClass="errormsg"></asp:label></td>
				</tr>
			</table>
			<div>
			</div>
		</form>
	</body>
</HTML>
