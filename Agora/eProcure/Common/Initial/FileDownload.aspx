<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FileDownload.aspx.vb" Inherits="eProcure.FileDownload" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>FileDownload</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<script language="javascript">
	<!--
	function Test1(i,f)       
			{	
						
			//msg=window.open("../PO/POReport.aspx?pageid=18&po_from=admin&img=" + temp,"","Width=700,Height=500,resizable=yes,scrollbars=yes");		 	                                 
			//msg.document.clear();
			 /* Note that the word SCRIPT was not
			    kept intact on one line in the
			    write() below.  A bug will parse 
			    it and will not compile the script
			    correctly if you don't break that
			    word when it appears within write()
			    statements. */
			document.clear();
			history.back();
			//history.go(-1 - parseInt(i));
			
			msg=window.open("","","Width=300,Height=300,resizable=yes,scrollbars=yes");
		 	msg.document.clear();
			 /* Note that the word SCRIPT was not
			    kept intact on one line in the
			    write() below.  A bug will parse 
			    it and will not compile the script
			    correctly if you don't break that
			    word when it appears within write()
			    statements. */
			   
			 msg.document.write('<HTML><HEAD><TITLE'
			  +'>Image Preview</TITLE>'
			  +'</HEAD><BODY>'
			  +'<img src="'
        	  + f + '"></img>'
        	   + '</BODY></H'
			  +'TML><P>');
			  
			
		}

//-->
		</script>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE BORDER="0" CELLSPACING="0" CELLPADDING="0">
				<TR class="emptycol">
					<TD></TD>
				</TR>
				<TR class="emptycol">
					<TD><asp:Label id="lblMsg" runat="server" CssClass="errormsg"></asp:Label></TD>
				</TR>
				<TR class="emptycol">
					<TD></TD>
				</TR>
				<TR class="emptycol">
					<TD>
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
