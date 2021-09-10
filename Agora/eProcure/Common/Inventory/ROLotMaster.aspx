<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ROLotMaster.aspx.vb" Inherits="eProcure.ROLotMaster" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ROLotMaster</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"       
        </script>
         <% Response.Write(css)%>   
		<script type="text/javascript">
		
		function onClick() 
        { 
            var bt = document.getElementById("hidButton"); 
            bt.click(); 
        }  
        
        function fireHid()
        {
            var bt2 = document.getElementById("hidButtonClose"); 
            bt2.click();          
			
        }
        
        function Close()
        {
            window.close();					
        }
              
        function terminate()
        {
            
		    var bt3 = document.getElementById("hidButtonClear"); 
            bt3.click();								        
	        		
				
        }
        
        function isNumberKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode!=46)
                return false;

             return true;
        }
        
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table  class="alltable"  width="100%" id="Table1" cellspacing="0" cellpadding="0">
			<tr>
				<td class="EmptyCol" colspan="4">
					<asp:label id="lblAction" runat="server" CssClass="lblInfo"
					Text="Click Save button to save record and Add line button to add a new row."
					></asp:label>

				</td>				
			</tr>
			<tr>
			    <td class="rowspacing" style="height: 5px;" colspan="4"></td>
			</tr>
			<tr>
				<td class="tablecol" style="height: 19px; border:0; width:11%;"><strong><asp:label id="lblItem" runat="server">Item Code:</asp:label></strong></td>				
				<td class="tablecol" style="height: 19px; border:0; width:20%;"><asp:label id="lblItemCode" runat="server"></asp:label></td>				
				<td class="tablecol" style="height: 19px; border:0; width:11%;"><strong><asp:label id="lblItemN" runat="server">Item Name:</asp:label></strong></td>
				<td class="tablecol" style="height: 19px; border:0; width:20%;"><asp:label id="lblItemName" runat="server"></asp:label></td>				
			</tr>
			<tr>
				<td class="tablecol" style="height: 19px; border:0;"><strong><asp:label id="Label5" runat="server">Remaining Qty:</asp:label></strong></td>
				<td class="tablecol" style="height: 19px; border:0;"><asp:label id="lblRemainQty" runat="server"></asp:label></td>				
				<td class="tablecol" style="height: 19px; border:0;"></td>
				<td class="tablecol" style="height: 19px; border:0;"></td>				
			</tr>
			
			<tr>
			    <td class="rowspacing"  style="height: 1px;" colspan="4"></td>
			</tr>
			<tr>
				<td colspan = "4" class="EmptyCol">
				    <% Response.Write(Session("ConstructTable")) %>
				</td>
			</tr>	
			<tr>
				<td colspan = "4" class="EmptyCol"><br/>
					<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
					<asp:button id="cmd_Add" runat="server" CssClass="button" Text="Add Line"></asp:button>&nbsp;
					<%--<asp:button id="cmd_Close" runat="server" CssClass="button" Text="Close"></asp:button>--%>
					<input class="button" id="cmd_Close"  onclick="fireHid()"  type="button" value="Close" name="Close" runat="server"/>&nbsp;
					<%--<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="Close()" />--%>
					<input class="button" id="hidButton" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none"/>&nbsp;
					<input class="button" id="hidButtonClose" type="button" value="hidButtonClose" name="hidButtonClose" runat="server" style=" display :none"/>
					<input class="button" id="hidButtonClear" type="button" value="hidButtonClear" name="hidButtonClear" runat="server" style=" display :none"/>
					<input id="hidItemLine" type="hidden" size="1" name="hidItemLine" runat="server"/>
					<%--<select class="ddl"  onchange onserverchange ="Select1_onserverchange" id="Reset1" type="label" value="Close" name="Close" runat="server">
					</select>--%>
					<asp:label id="hidLbl" runat="server" style="display:none "></asp:label>
					</td>
			</tr>
			<tr>
				<td colspan = "4" ></td>
			</tr>
			<tr>
				<td colspan = "4" class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			</tr>
			</table>
		</form>
	</body>
</html>
