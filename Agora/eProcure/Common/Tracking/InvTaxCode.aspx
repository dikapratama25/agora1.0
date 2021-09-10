<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvTaxCode.aspx.vb" Inherits="eProcure.InvTaxCode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>InvTaxCode</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
			function Select()
			{	
			    //alert(document.getElementById("ddlTaxCode").value);
			    document.Form1.hidTCValue.value= document.getElementById("ddlTaxCode").value;
				selectAllItem(document.Form1.hidID.value);	
			}
			
			function selectAllItem(val)
			{ 
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp('$')				 
				for (var i=0;i<oform.elements.length;i++)
				{
				    var foo = "$";
					var e = oform.elements[i];
					var sEvents = e.name;
				 
					if (sEvents.indexOf("$") > 0)
					{					 
						if (sEvents.substring(sEvents.lastIndexOf("$")+1) == val)
						//alert(sEvents);
					    {
					        var r = (eval("window.opener.document.Form1." + sEvents));
					        //alert(r.disabled);
					        if (r.disabled == false)
					        {
					            r.value = document.Form1.hidTCValue.value;
					        }
					    }
					}
				}
				window.close();
			}
									
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td colspan="2" class="header" style="WIDTH: 529px"><strong><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></strong></td>
				</tr>
				<tr>
					<td colspan="2" class="emptycol" style="WIDTH: 529px">&nbsp;</td>
				</tr>
				<tr>
					<td class="tableheader" style="WIDTH: 529px" colspan="3">&nbsp;SST Tax Code</td>
				</tr>
				<tr>
					<td class="tablecol" width="40%">&nbsp; <strong>SST Tax Code (Purchase)</strong>&nbsp;:</td>
					<td class="tablecol" width="60%" align="left">
                        <asp:DropDownList ID="ddlTaxCode" runat="server" CssClass="ddl" Width="80%">
                        </asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="emptycol" style="WIDTH: 529px">&nbsp;</td>
				</tr>
				<tr id="trP" runat="server">
					<td colspan="2" style="WIDTH: 211px"><input class="button" id="cmdSelect" onclick="Select();" type="button" value="Save"
							name="cmdSelect" runat="server"/> <input class="button" id="cmdClose" onclick="window.close();" type="button" value="Close"/>
						<input id="hidID" style="WIDTH: 35px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server"/><input id="hidTCValue" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidTCValue"
							runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol" style="WIDTH: 203px"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
