<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPBatchUpload.aspx.vb" Inherits="eProcure.IPPBatchUpload" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>E2P Batch Upload</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
       <% Response.Write(Session("JQuery")) %> 
        <% response.write(Session("Block")) %>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
     <script type="text/javascript" >
     function UploadProgress()
            {
                //$("#cmdUpload").click();
                $.blockUI({ 
                message: '<h1>Uploading in process...</h1>',
                css: { 
                border: 'none',                 
                padding: '15px', 
                backgroundColor: '#000', 
                '-webkit-border-radius': '10px', 
                '-moz-border-radius': '10px', 
                opacity: .5, 
                color: '#fff' 
                },
                overlayCSS: { backgroundColor: '#fff',
                opacity: 0.2,
		        cursor:	'wait' 
                }
                 }); 
            }
    function UnBlockUploadProgress()
            {              
              $.unblockUI();
            }
     </script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">

			<table class="alltable" id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header"><asp:label id="Label3" runat="server" CssClass="header">Batch Upload of multiple GL Debits</asp:label></td>
				</tr>
				
                <tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="tableheader"  colspan="3">
					<asp:label id="lblTitle" runat="server">Document Info.</asp:label>
					</td>
				</tr>
			
			    <tr>
			        <td class="tablecol" style="width:35%;"><strong>&nbsp;
			        <asp:Label ID="Label1" runat="server" Text="Document No. :"  CssClass="lbl" Font-Bold="True"></asp:Label></strong>&nbsp;
			        <asp:Label ID="lblDocNo" runat="server"  CssClass="lbl"></asp:Label>
			        </td>
				  <td class="tablecol"><strong>&nbsp;
				  <asp:Label ID="Label4" runat="server" Text="Document Date :"  CssClass="lbl" Font-Bold="True"></asp:Label></strong>&nbsp;
				  <asp:Label ID="lblDocDate" runat="server"  CssClass="lbl"></asp:Label>
				  </td>                 
				  
                </tr>
                 <tr>
				    <td class="tablecol"><strong>&nbsp;
				    <asp:Label ID="Label2" runat="server" Text="Vendor Name :" CssClass="lbl" Font-Bold="True" ></asp:Label></strong>&nbsp;
				    <asp:Label ID="lblVendorName" runat="server" CssClass="lbl" ></asp:Label>
				    </td>
					<td class="tablecol"></td>
			    </tr>

         </table>
				<%--<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>--%>
				<br/>
		<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="tableheader" colspan="3">&nbsp;<asp:label id="lblHeader" runat="server">Multi GL Upload</asp:label></td>							
				</tr>
					
				<tr >
					<td class="tablecol" colspan="3"></td>
				</tr>
				<tr>
					<td  class="tablecol" style="width:20%;">&nbsp;<b>File Location :</b><br/>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
					<td class="tablecol">&nbsp;&nbsp;
					<input class="button" id="cmdBrowse" style="FONT-SIZE: 8pt; WIDTH: 320px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" runat="server"/>&nbsp;</td>					
				</tr>
				<tr>
					<td class="tablecol"></td>
					<td class="tablecol">&nbsp;&nbsp;<asp:label id="lblPath" CssClass="txtbox" Runat="server" Height="5px"></asp:label></td>
				
				</tr>	
				<tr>
					<td class="emptycol" colspan = "2"></td>
				</tr>										
				<tr>
					<td class="emptycol" colspan="2">Download Multi GL Upload template - 
						<asp:linkbutton id="cmdDownloadTemplate" Runat="server">MultipleGLDebits.xls [93KB]</asp:linkbutton>
					</td>
				</tr>	
					
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td><asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload"></asp:button>
					</td>
				</tr>				
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
								
				<tr>
				    <td class="emptycol" colspan="2"><ul class="errormsg" id="vldsum" runat="server" style="width:100%"></ul>
				</td>
				</tr>
			</table>
			
		</form>
	</body>
</HTML>
