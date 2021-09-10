<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LotDeliveryOrder.aspx.vb" Inherits="eProcure.LotDeliveryOrder" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>LotDeliveryOrder</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            
        </script>
         <% Response.Write(css)%>  
         <% Response.Write(Session("JQuery")) %>        
         <% Response.Write(Session("WheelScript"))%> 
		<script type="text/javascript">
        
        function fireHid()
        {
            var bt2 = document.getElementById("hidButtonClose"); 
            bt2.click();          
			
        }
        
        
        
        function Close()
        {
            
	        window.close();				
				
        }
        
        function isNumberKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

             return true;
        }
        
        function isAlphaNumericKey(evt)
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
        
        function fireHid2()
        {
            var bt3 = document.getElementById("hidBtnDisplay"); 
            bt3.click();          
			
        }
        
        function fireHid3(id)
        {
            Form1.hidAttachIndex.value = id;
            var bt4 = document.getElementById("hidBtnDisplay2"); 
            bt4.click();          
			
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
			//window.location.reload;
			fireHid2();
			
			if (retval == "1" || retval =="" || retval==null)
			{  window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
			
			alert('ok');
		}   

        
		</script>
	</head>
	<body class="body">
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
				<td class="tablecol" colspan="4" style="height: 19px; border:0; width:20%;" ><strong><asp:label id="lblItem" runat="server">Selected Item Code:</asp:label></strong>&nbsp;
				<asp:label id="lblItemCode" runat="server"></asp:label><asp:Label ID="hidShipQty" runat="server" Visible="false"></asp:Label>
				<asp:Label ID="hiditemline" runat="server" Visible="false"></asp:Label><asp:Label ID="hidpoline" runat="server" Visible="false"></asp:Label></td>						
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
					<input class="button" id="cmd_Close"  onclick="fireHid()"  type="button" value="Close" name="Close" runat="server"/>&nbsp;
					<input class="button" id="hidButtonClose" type="button" value="hidButtonClose" name="hidButtonClose" runat="server" style=" display :none"/>
					<input class="button" id="hidBtnDisplay" type="button" value="hidBtnDisplay" name="hidBtnDisplay" runat="server" style=" display :none"/>
					<input class="button" id="hidBtnDisplay2" type="button" value="hidBtnDisplay2" name="hidBtnDisplay2" runat="server" style=" display :none"/>
					<input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server" />
					<input id="hidAttachIndex" type="hidden" size="1" name="hidAttachIndex" runat="server" />
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
