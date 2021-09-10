<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PR_DisplayMsg.aspx.vb" Inherits="eProcure.PR_DisplayMsg" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PR_DisplayMsg</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ rel=""stylesheet"" />"		    		    
		    
		    Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox=" + val + "&seldate=" + txtVal.value)
        </script>
        <% Response.Write(sCSS)%>
        
        <script language="javascript">
		<!--		
		
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtg_POList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chk_New.checked=checked;
			oform.chk_Open.checked=checked;
			oform.chk_Accept.checked=checked;
			oform.chk_Reject.checked=checked;
			oform.chk_Cancel.checked=checked;
			oform.chk_close.checked=checked;
			oform.ChK_open2.checked=checked;
			oform.chk_fully2.checked=checked;
			oform.chk_complete2.checked=checked;
			oform.chk_cancelorder2.checked=checked;
			oform.chk_pending2.checked=checked;
			oform.chk_part2.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtg_POList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			checkStatus(false);
		}
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<% Response.Write(sOpen)%>' ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');		
		}
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:button id="Button1" style="Z-INDEX: 101; LEFT: 16px; POSITION: absolute; TOP: 40px" runat="server"
				Text="BuyerCancel"></asp:button><asp:button id="Button3" style="Z-INDEX: 102; LEFT: 16px; POSITION: absolute; TOP: 80px" runat="server"
				Text="VenCancel"></asp:button><asp:button id="Button2" style="Z-INDEX: 103; LEFT: 24px; POSITION: absolute; TOP: 128px" runat="server"
				Text="Button"></asp:button></form>
	</body>
</HTML>
