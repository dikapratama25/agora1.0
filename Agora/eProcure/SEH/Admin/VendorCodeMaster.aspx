<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorCodeMaster.aspx.vb" Inherits="eProcure.VendorCodeMaster" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>VendorCodeMaster</title>
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
        
        function isDecimalKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                return false;

                return true;
        }   

        
		</script>
	</head>
	<body class="body">
		<form id="Form1" method="post" runat="server">
			<table  class="alltable"  width="100%" id="Table1" cellSpacing="0" cellPadding="0">
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
				<td class="tablecol" colspan="4" style="height: 19px; border:0; width:20%;" ><strong><asp:label id="lblItem" runat="server">Selected Vendor ID :</asp:label></strong>&nbsp;
				<asp:label id="lblVenID" runat="server"></asp:label><asp:Label ID="hidVenID" runat="server" Visible="false"></asp:Label>
				<asp:Label ID="hidType" runat="server" Visible="false"></asp:Label></td>						
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
