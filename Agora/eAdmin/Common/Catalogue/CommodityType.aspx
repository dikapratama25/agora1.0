<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommodityType.aspx.vb" Inherits="eAdmin.CommodityType" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Commodity Type</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <% Response.Write(Session("WheelScript"))%>

	    <style type="text/css">
	        TD {FONT-SIZE: 12px; COLOR: #000; FONT-FAMILY: Arial, Verdana; }
	        A:link { text-decoration:none; color:#333399; -moz-outline-style: none;  outline-style:none;}	
            A:visited {text-decoration:	none; color:#333399; -moz-outline-style:none;outline-style:none;}	
		    A:active { text-decoration:none; color:#333399;-moz-outline-style: none;outline-style:none;}	
		    A:hover	{text-decoration:underline; color:#3399ff; -moz-outline-style:none; outline-style:none;}
            .TableHeader{BORDER: #ffffff 1px solid;FONT-WEIGHT: bold;FONT-SIZE: 12px;COLOR: #000;FONT-FAMILY: Arial, Verdana;BACKGROUND-COLOR: #d1e1ef;height : 19px;}
		    
        </style>

		<script language="javascript">
		<!--
		
			function Reset()
			{
				var oform = document.forms(0);
				oform.txtItemId.value="";
				oform.txtDesc.value="";				
			}
			
			function checkSearchCriteria()
			{
				var oform = document.forms(0);
				var strName, strDesc;
				strName = oform.txtItemId.value;
				strDesc = oform.txtDesc.value;				
				
				if (strName == '' && strDesc == ''){
					alert('No search criteria specified.');
					return false;
				}
				else
					return true;
			}

			function Select()
			{	
				var r =eval("window.opener.document.Form1.txtCommodityType");
				var s =eval("window.opener.document.Form1.hidCommodityType");
				var strCat, i, strB1, strB2;
				strB1 = '<B><I>';
				strB2 = '</I></B>';
				strCat = Form1.hidCat.value;
				i = strCat.indexOf(' ');
				strCat = strCat.substring(i+1, strCat.length);
				strCat = strCat.replace(strB1, '');
				strCat = strCat.replace(strB2, '');
				
				//r.value = Form1.hidCat.value;				
				r.value = strCat;
				s.value = Form1.hidCatId.value;
				
				if (Form1.hidCat.value == '')
					alert('Please select a category.');
				else 
					window.close();
			}
			
			function pass_target(sel_id, sel_text)
		    {
		        var selected_str = sel_text;
			    window.opener.document.Form1.txtCommodityType.value = selected_str.substring(6, selected_str.length - 4);
			    window.opener.document.Form1.hidCommodityType.value = sel_id;
			    window.close();
		    }
						
		-->
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				
				<TR>
					<TD class="TableHeader">&nbsp;Search Criteria</TD>
				</TR>
				<tr>
				    <td ><%--<iewc:treeview id="tvCategory" runat="server" DefaultStyle="font-size: 8pt;font-family: Arial;background:white;color:black"
							HoverStyle="background:transparent;color:black;font-weight:bold" SelectedStyle="background:white;color:black;font-weight:bold"
							ShowPlus="True" autopostback="true" ShowToolTip="True"></iewc:treeview>--%>
							
					<asp:TreeView ID="TreeView1" ExpandDepth="0" PopulateNodesFromClient="true" ShowLines="True" runat="server" BorderColor="White" Font-Overline="False">
                        <RootNodeStyle Font-Overline="False"  />
                    </asp:TreeView>		
							
					</td>
				</tr>
			
			</table>
		</form>
	</body>
</HTML>
