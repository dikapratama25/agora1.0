<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Policy.aspx.vb" Inherits="eProcurement.Policy" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Policy Agreement</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	    <script runat="server">
	       Dim dDispatcher As New AgoraLegacy.dispatcher
	        Dim sCSS As String = "<LINK href = """ & dDispatcher.direct("Plugins/CSS", "Styles.css") & """  type=""text/css"" rel=""stylesheet"">"
        </script>
        <% Response.Write(sCSS)%> 
        <% Response.Write(Session("JQuery")) %>	
        <script language="javascript">
        
        
            function Reset(){
				var oform = document.forms(0);
				oform.txtUserId.value="";
				oform.txtCompID.value="";
                oform.txtEmail.value="";
                oform.txtQuestion.value="";
                oform.txtAns.value="";
			}
//        function txtEl_onBlur() 
//        { 
//        var bt = document.getElementById("btClick"); 
//        bt.click(); 
//        }

        </script>

		 
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="form1" method="post" runat="server">
		    <TABLE class="table" cellSpacing="0" cellPadding="0" width="100%" border="0">
					
		        <iframe runat="server" id="frame1" src="Policy.html" border="0" frameborder="0" height="700" width="650" ></iframe>
                
                <tr runat="server" id="trSave">
				    <TD align="left" colspan="2" style="height: 9px"><asp:button id="cmdAgree" runat="server" CssClass="button" Text="Agree"></asp:button>&nbsp;
				    <asp:button id="cmdDecline" runat="server" CssClass="button" Text="Decline"></asp:button>
				    </TD>
			    </tr>
			</TABLE>
		</form>
	</body>
</HTML>
