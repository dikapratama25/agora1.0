<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryTransfer.aspx.vb" Inherits="eProcure.InventoryTransfer" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Inventory Transfer</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            dim Search as string = dDispatcher.direct("Search","InventoryItemSearchPopup.aspx","type=trans")
        </script>
        <% Response.Write(css)%>
        <% Response.write(Session("typeahead")) %>
        <% Response.Write(Session("WheelScript"))%>
        
		<script language="javascript">
		
		$(document).ready(function(){
        $('#cmd_Submit').click(function() { 
        document.getElementById("cmd_Submit").style.display= "none";
        });
        });
		
		
		
		    function onClick() 
            { 
                var bt = document.getElementById("hidButton"); 
                bt.click(); 
            }
                        
            function Search()
			{
			    { window.open('<% Response.Write(Search) %>','Wheel','help:No,Height=580,Width=750,resizable=yes,scrollbars=yes'); return true; }
            }
            
            function isNumberKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode!=46)
                    return false;

                 return true;
            }
            
            function PopWindow(myLoc)
	        {
		        window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		        return false;
	        }
	        
	        function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 750px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
		    }
		    
		    function Reset()
		    {
		        var oform = document.forms(0);
		        oform.txtRefNo.value="";
		        oform.txtRemark.value="";
		    }
            
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InventoryTran_tabs"))%>
			<TABLE  class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td colspan="2" class="rowspacing"></td></tr>
						
			<tr >
				<td class="tablecol" align="left" width="18%" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >IT Number</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="50%" style="height: 19px"><asp:label id="lblITNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" style="height: 19px"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label4" runat="server" >Transfer Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblTransferDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >Reference No</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:textbox id="txtRefNo" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
				<td class="tablecol"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:textbox id="txtRemark" runat="server" Height="32px" width="390px" TextMode="MultiLine" CssClass="txtbox" MaxLength="400"></asp:textbox></td>
				<td class="tablecol">&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server"></td>
			</tr>
			
			</table>
						
			<TABLE class="alltable"  width="100%" id="Table3" cellSpacing="0" cellPadding="0">
				<tr>
				<td  class="EmptyCol">
				    <% Response.Write(Session("ConstructTable")) %>
				</td>
			    </tr>
				<tr>
					<td class="EmptyCol"><br>
						<asp:button id="cmd_Submit" runat="server" CssClass="button" Text="Submit"></asp:button><INPUT type="button" value="View" id="cmdPrint" runat="server" Class="button" style="width: 50px" visible="false"><asp:button id="cmd_Add" runat="server" CssClass="button" Text="Add Line"></asp:button><asp:button id="cmd_Search" runat="server" CssClass="button" Width="75px" Text="Search Item"></asp:button>
						<INPUT class="button" id="hidButton" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none">
						<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button> 
						<input runat="server" onfocus="onClick()" style=" border-style:none;background-color:Transparent;width:92px;margin-right:0px;" class="txtbox2" type="text" id="txtTemp" name="txtTemp">
				    </TD>
				</tr>
				<tr>
				    <td colspan = "4" ></td>
			    </tr>
				<tr>
				    <td colspan = "4" class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			    </tr>
			</TABLE>
		</form>
	</body>
</HTML>
