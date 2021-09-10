<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddPaymentType.aspx.vb" Inherits="eProcure.AddPaymentType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Payment Type</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
        </script>
          <script language="javascript">
         function clearvld()
         {
            document.getElementById("vldsummary").innerHTML = "";
         }
         function Reset()
			{
			    var oform = document.forms(0);
			    var a = document.getElementById('txtAssetGroupCode');
			    if(a)
			    {
				    a.value = "";
			    }
			    var b = document.getElementById('txtAssetGroupDesc');
			    if(b)
			    {
				    b.value = "";
			    }					    
		    }
		function Close()
		{
		    window.close();
		}				   
	    </script>   
</head>
    <body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">				
                <tr>
                    <td class="rowspacing"></td>
                </tr>
                <tr>
                    <td class="emptycol" colspan="2">
                        <asp:Label ID="Label3" runat="server" Text="Click Save button to save record and Add button to add a new payment type" CssClass="lblinfo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="linespacing2"></td>
                </tr>                	    
				<tr>
					<td class="TableHeader" colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Payment Type"></asp:Label>
                    </td>    
				</tr>				
			    <tr>
			        <td class="TableCol" style="width:25%">
			            <strong>
			                <asp:Label ID="Label1" runat="server" Text="Payment Type Code"  CssClass="lbl"></asp:Label>
			                <asp:Label ID="Label6" runat="server" Text="*" CssClass="errormsg"></asp:Label>
                            <asp:Label ID="Label7" runat="server" Text=":" CssClass="lbl"></asp:Label>
			            </strong>
			        </td>
				    <td class="TableCol">
				        <asp:textbox id="txtPaymentType" runat="server" MaxLength="30" CssClass="txtbox"></asp:textbox>                            
                    </td>
                </tr>
                <tr >
                    <td class="TableCol" >
                        <strong>
                            <asp:Label ID="Label5" runat="server" Text="Description"  CssClass="lbl"></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text="*" CssClass="errormsg"></asp:Label>
                            <asp:Label ID="Label9" runat="server" Text=":" CssClass="lbl"></asp:Label>
                        </strong>
                    </td>
                    <td class="TableCol">
                        <asp:textbox id="txtDesc" runat="server" MaxLength="30" CssClass="txtbox"></asp:textbox>
                    </td>                                  
                </tr>				
			    <tr >
				    <td class="TableCol">
				        <strong><asp:Label ID="Label2" runat="server" Text="Status :" CssClass="lbl" ></asp:Label></strong>
				    </td>
				    <td class="TableCol">
                        <asp:RadioButtonList ID="rdbStatus" CssClass="rbtn" runat="server" RepeatDirection="Horizontal" >
                        <asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                        </asp:RadioButtonList>
				    </td>                                                        
			    </tr>
			    <tr>
			        <td class="rowspacing"></td>
                </tr>
  								
			</table>
			<tr>
                    <td class="emptycol">
                        <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="button" />
                        <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="button" />
                        <input id="cmdClose" type="button" runat="server" value="Close" onclick="Close();" Class="button" />
                    </td>
                </tr>
                <tr>
                    <td class="emptycol">
                        <ul runat="server" id="vldsummary" style="list-style-type:square;color:Red;"></ul>
                    </td>
                </tr>				
		</form>
</body>
</html>