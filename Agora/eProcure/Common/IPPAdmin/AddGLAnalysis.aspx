<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddGLAnalysis.aspx.vb" Inherits="eProcure.AddGLAnalysis" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AddGLAnalysis</title>
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
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.Write(Session("WheelScript"))%>
        <% Response.Write(Session("typeahead"))%>
		<script type="text/javascript">
		
//        function onCheck() 
//        { 
//            //alert($('#txtGLCode1').val);
//            var userPass = document.getElementById('txtGLCode1');
//            alert(userPass.value);
////            if ($('#txtGLCode1').val() == '')
////            {
////                $('#txtDesc1').val('');
////            }
//        } 
        
        function selectAll()
		{
			SelectAllG("chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"chkAll","chkSelection");
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
				<td class="header" colspan="2"><asp:label id="lblTitle" runat="server">Add New GL Analysis Description</asp:label></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="2"></td>
			</tr>
			<tr>
				<td class="emptycol" colspan="2">
					<asp:label id="Label1" runat="server"  CssClass="lblInfo"
					Text="Click Save button to save record and Add line button to add a new row."></asp:label>
				</td>
			</tr>
			<tr>
			    <td class="rowspacing" style="height: 5px;" colspan="2"></td>
			</tr>
			<tr>
				<td class="tablecol" width="35%">&nbsp;<strong>GL Analysis Description Code </strong>:</td>				
				<td class="tablecol" width="65%"><asp:textbox id="txtRuleCode" runat="server" MaxLength="20" CssClass="txtbox" width="300px"></asp:textbox></td>				
			</tr>
			<tr>
			    <td class="rowspacing"  style="height: 1px;" colspan="2"></td>
			</tr>
			<tr>
				<td colspan = "2" class="EmptyCol">
				    <% Response.Write(Session("ConstructTableRule")) %>
				</td>
			</tr>
			<tr>
			    <td colspan = "2" class="EmptyCol">
			        <asp:button id="cmdAddRule" runat="server" CssClass="button" Text="Add Line"></asp:button>
			    </td> 
			</tr>
			<tr>
			    <td class="rowspacing"  style="height: 1px;" colspan="2"></td>
			</tr>
			<tr>
				<td colspan = "2" class="EmptyCol">
				    <% Response.Write(Session("ConstructTableGL")) %>
				</td>
			</tr>
			<tr>
			    <td colspan = "2" class="EmptyCol">
			        <asp:button id="cmdAddGL" runat="server" CssClass="button" Text="Add Line"></asp:button>
			    </td> 
			</tr>		
			<tr>
				<td colspan = "2" class="EmptyCol"><br/>
					<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
					<input class="button" id="cmdClose"  onclick="fireHid()"  type="button" value="Close" name="Close" runat="server"/>&nbsp;
					<input class="button" id="hidButton" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none"/>&nbsp;
					<input class="button" id="hidButtonClose" type="button" value="hidButtonClose" name="hidButtonClose" runat="server" style=" display :none"/>
					</td>
			</tr>
			<tr>
				<td colspan = "2"></td>
			</tr>
			<tr>
				<td colspan = "2" class="EmptyCol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
			</tr>
			</table>
		</form>
	</body>
</html>
