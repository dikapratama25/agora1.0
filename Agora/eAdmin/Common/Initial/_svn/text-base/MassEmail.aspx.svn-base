<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MassEmail.aspx.vb" Inherits="eAdmin.MassEmail" ValidateRequest="false"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Dissemination of New Information</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <% Response.Write(Session("WheelScript"))%>
		<style type="text/css">.advertisement { BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; FONT-SIZE: 12px; VERTICAL-ALIGN: middle; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid; FONT-FAMILY: verdana, tahoma; TEXT-ALIGN: center; arial: }
	.tblToolbar { BORDER-RIGHT: 1px outset; PADDING-RIGHT: 1px; BORDER-TOP: 1px outset; PADDING-LEFT: 1px; PADDING-BOTTOM: 1px; BORDER-LEFT: 1px outset; COLOR: menutext; PADDING-TOP: 1px; BORDER-BOTTOM: 1px outset; BACKGROUND-COLOR: buttonface }
	.raiseme { BORDER-RIGHT: 1px outset; BORDER-TOP: 1px outset; BORDER-LEFT: 1px outset; BORDER-BOTTOM: 1px outset }
	.raisemeleft { BORDER-LEFT: 2px groove }
	.cbtn { BORDER-RIGHT: buttonface 1px solid; BORDER-TOP: buttonface 1px solid; BORDER-LEFT: buttonface 1px solid; BORDER-BOTTOM: buttonface 1px solid }
	.codedisplay { FONT-SIZE: 10px; FONT-FAMILY: courier; TEXT-ALIGN: left }
	.selects { FONT-SIZE: 10px; FONT-FAMILY: tahoma, verdana, arial, courier, serif }
	.txtbtn { FONT-SIZE: 70%; COLOR: menutext; FONT-FAMILY: tahoma, verdana, arial, courier, serif }
	.DivMenu { BORDER-RIGHT: buttonface 1px groove; BORDER-TOP: buttonface 1px groove; Z-INDEX: 100; LEFT: -200px; BORDER-LEFT: buttonface 1px groove; WIDTH: 125px; BORDER-BOTTOM: buttonface 1px groove; POSITION: absolute; TOP: -1000px; BACKGROUND-COLOR: buttonface }
	.TDMenu { FONT-SIZE: 70%; WIDTH: 100%; CURSOR: default; COLOR: buttonface; font-familt: verdana }
	.AllTable { BORDER-RIGHT: 0px; BORDER-TOP: 0px; BORDER-LEFT: 0px; WIDTH: 100%; BORDER-BOTTOM: 0px }
	.header { FONT-WEIGHT: bold; FONT-SIZE: 12pt; FONT-FAMILY: Verdana }
	.EmptyCol { FONT-SIZE: 8pt; FONT-FAMILY: Verdana; HEIGHT: 15px }
	.TableHeader { BORDER-RIGHT: #ffffff 1px solid; BORDER-TOP: #ffffff 1px solid; FONT-WEIGHT: bold; FONT-SIZE: 8pt; BORDER-LEFT: #ffffff 1px solid; COLOR: white; BORDER-BOTTOM: #ffffff 1px solid; FONT-FAMILY: Verdana,Arial; HEIGHT: 19px; BACKGROUND-COLOR: #faa550 }
	.txtbox { FONT-SIZE: 8pt; FONT-FAMILY: Verdana; HEIGHT: 18px }
	.TableCol { FONT-SIZE: 8pt; FONT-FAMILY: Verdana; HEIGHT: 16px; BACKGROUND-COLOR: #fff8dd }
	.Button { FONT-SIZE: 8pt; WIDTH: 70px; CURSOR: hand; HEIGHT: 18px; BACKGROUND-COLOR: lightgrey }
	.ErrorMsg { FONT-SIZE: 10pt; COLOR: red; FONT-FAMILY: Verdana,Arial }
		</style>
		<script language="javascript">
		<!--	
		
		window.onload="doInit"; 
		
		// For EmotIcon Menu
		var isViewEmotIconMenu = false;
		
		function doInit()
		{
			for (i=0; i<document.all.length; i++) 
			document.all(i).unselectable = "on";
			TextEditor.unselectable = "off";
			TextEditor.focus();
		} 
		
		var isHTMLMode = false;
		var bShow = false;
		var sPersistValue;
		
		// button over effect
		function button_over(eButton)
		{
			eButton.style.backgroundColor = "#B5BDD6";
			eButton.style.borderColor = "darkblue darkblue darkblue darkblue";
		} 
		
		// go back to normal
		function button_out(eButton) 
		{
			eButton.style.backgroundColor = "threedface";
			eButton.style.borderColor = "threedface";
		} 
		
		// button down effect
		function button_down(eButton)
		{
			eButton.style.backgroundColor = "#8494B5";
			eButton.style.borderColor = "darkblue darkblue darkblue darkblue";
		} 
		
		// back to normal
		function button_up(eButton) 
		{
			eButton.style.backgroundColor = "#B5BDD6";
			eButton.style.borderColor = "darkblue darkblue darkblue darkblue";
			eButton = null;
		} 
		
		// Resets Style to default after selection
		function EditorOnStyle(select)
		{
			cmdExec("formatBlock", select[select.selectedIndex].value);
			select.selectedIndex = 0;
		}
		
		// Resets Font to default after selection
		function EditorOnFont(select)
		{
			cmdExec("fontname", select[select.selectedIndex].value);
			select.selectedIndex = 0;
		}
		
		// Resets Size to default after selection
		function EditorOnSize(select)
		{
			cmdExec("fontsize", select[select.selectedIndex].value);
			select.selectedIndex = 0;
		}
		
		// execute command and enter the HTML in the RTB
		function cmdExec(cmd,opt)
		{
			if (isHTMLMode){alert("Please uncheck 'Edit HTML'");return;}
			
			TextEditor.focus();
			TextEditor.document.execCommand(cmd,bShow,opt);
			bShow=false;
		} 
		
		// sets the mode for HTML or Text
		function setMode(bMode)
		{
				
			var sTmp;
  			isHTMLMode = bMode;
  			if (isHTMLMode)
			{
				sTmp=TextEditor.innerHTML;
				TextEditor.innerText=sTmp;
			} 
			else 
			{
				sTmp=TextEditor.innerText;
				TextEditor.innerHTML=sTmp;
			}
  			TextEditor.focus();
		}
		
		// Insert Image
		function insertImage()
		{
			if (isHTMLMode){alert("Please uncheck 'Edit HTML'");return;}
			bShow=true;
			cmdExec("InsertImage");
		} 
			
		// Insert Horizontal Rule
		function insertRuler()
		{
			if (isHTMLMode){alert("Please uncheck 'Edit HTML'");return;}
			cmdExec("InsertHorizontalRule","");
		} 
		
		// sets everything to vertical mode
		function VerticalMode()
		{
			if (TextEditor.style.writingMode == 'tb-rl') TextEditor.style.writingMode = 'lr-tb';
			else TextEditor.style.writingMode = 'tb-rl';
		} 
		
		// calls the color object
		function callColorDlg(sColorType)
		{
			var sColor = Form1.dlgHelper.ChooseColorDlg();sColor = sColor.toString(16);
			if (sColor.length < 6) 
			{
				var sTempString = "000000".substring(0,6-sColor.length);
				sColor = sTempString.concat(sColor);
			}
			cmdExec(sColorType, sColor);	
		} 
		
		// sets the text in the Div to a textbox which you can pull the 
		// data from to save in the database
		function getHTML()
		{
			var String = TextEditor;  // The TextEditor DIV can not be in a form or the Java error out
			Form1.txtRTB.value = String.innerHTML;   //you can't make the text box invisible or the Java will error out
		}
		
		// Load RTB info from the database into the DIV field on the web.
		// so one can edit the database info in the Rich Text Box.
		function LoadDiv()
		{
			var String = Form1.txtRTB.value;
			TextEditor.innerHTML = String;   // set the innerHTML of the DIV to the text of the textbox
		}	
		
		function Reset(){
			ValidatorReset();
			var oform = document.forms(0);					
			oform.txtTo.value="";
			oform.txtSubject.value="";
			oform.txtContent.value="";
			oform.optBuyers.checked = true;
			
		}
		-->
		</script>
	</HEAD>
	<body onload="LoadDiv()" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="HEIGHT: 0px">
						<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD class="header"><FONT face="Verdana"><STRONG>Mass Email</STRONG></FONT></TD>
								</TR>
								<TR>
									<TD class="emptycol"></TD>
								</TR>
								<!--radio button-->
								<TR>
									<TD style="HEIGHT: 48px">
										<TABLE class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="tableheader" colSpan="3">&nbsp;Send Email</TD>
											</TR>
											<TR class=tablecol>
												<TD width="30%"><asp:radiobutton id="optBuyers" runat="server" Text="All Buyers" GroupName="optUserGroup" AutoPostBack="True"></asp:radiobutton></TD>
												<TD width="50%"><asp:radiobutton id="optVendors" runat="server" Text="All Vendors" GroupName="optUserGroup" AutoPostBack="True"></asp:radiobutton></TD>
												<TD width="20%"><asp:radiobutton id="optUsers" runat="server" Text="All Users" GroupName="optUserGroup" AutoPostBack="True"></asp:radiobutton></TD>
											</TR>
											<TR class=tablecol>
												<td><asp:radiobutton id="optApprovingOfficer" runat="server" Text="Approving Officer" GroupName="optUserGroup"
														AutoPostBack="True"></asp:radiobutton></td>
												<td><asp:radiobutton id="optRoles" runat="server" Text="Roles" GroupName="optUserGroup" AutoPostBack="True"></asp:radiobutton>&nbsp;<asp:dropdownlist id="cboRoles" runat="server" Enabled="False" CssClass="txtbox" Width="258px"></asp:dropdownlist></td>
												<td><asp:radiobutton id="optOthers" runat="server" Text="Others" GroupName="optUserGroup" AutoPostBack="True"></asp:radiobutton></td>
											</TR>
										</TABLE>
									</TD>
								</TR> <!--close radio button-->
								<TR>
									<TD class="emptycol"></TD>
								</TR>
								<TR class="emptycol">
									<TD style="HEIGHT: 7px">Note: Use ";" in between each Email addresses</TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 7px"></TD>
								</TR>
							</TBODY>
						</TABLE>
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tablecol" style="WIDTH: 160px; HEIGHT: 19px" vAlign="top" width="160">&nbsp;<STRONG>From 
										:&nbsp; </STRONG>
								</TD>
								<TD class="tablecol" style="HEIGHT: 19px">&nbsp;
									<asp:textbox id="txtFrom" runat="server" Enabled="False" CssClass="txtbox" Width="360px" MaxLength="500"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfvFrom" runat="server" Enabled="False" Width="16px" EnableClientScript="False"
										ErrorMessage="From is required." ControlToValidate="txtFrom" Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revFrom" runat="server" Enabled="False" EnableClientScript="False" ErrorMessage="Invalid From."
										ControlToValidate="txtFrom" Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR id="trTo" runat="server">
								<TD class="tablecol" style="WIDTH: 160px; HEIGHT: 18px" vAlign="top" width="160">&nbsp;<STRONG>To 
										: </STRONG>
								</TD>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;
									<asp:textbox id="txtTo" runat="server" CssClass="txtbox" Width="361px" MaxLength="500"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfvSendTo" runat="server" EnableClientScript="False" ErrorMessage="To is required."
										ControlToValidate="txtTo" Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 160px; HEIGHT: 17px" vAlign="top" width="160">&nbsp;<STRONG>Subject 
										: </STRONG>
								</TD>
								<TD class="tablecol" style="HEIGHT: 17px">&nbsp;
									<asp:textbox id="txtSubject" runat="server" CssClass="txtbox" Width="361px" MaxLength="200"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="rfvSubject" runat="server" EnableClientScript="False" ErrorMessage="Subject is required."
										ControlToValidate="txtSubject" Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<tr>
								<td class="tablecol" style="WIDTH: 160px; HEIGHT: 17px" vAlign="top" width="160">&nbsp;<strong>Attachment 
										:</strong></td>
								<TD class="tablecol" style="HEIGHT: 17px">&nbsp; <INPUT class="txtBox" id="txtFile" style="WIDTH: 250px" type="file" runat="server">&nbsp;
									<asp:button id="btnAddAttachment" runat="server" Text="Add This Attachment" CssClass="button"
										Width="120px" CausesValidation="False"></asp:button></TD>
							</tr>
							<tr>
								<td class="tablecol" style="WIDTH: 160px; HEIGHT: 17px" vAlign="top" width="160"></td>
								<TD class="tablecol" style="HEIGHT: 17px" vAlign="top"><asp:checkboxlist id="chkAttach" runat="server" Width="350px" CellSpacing="0" CellPadding="0" RepeatDirection="Horizontal"
										RepeatColumns="3" Font-Size="Smaller" Height="17px"></asp:checkboxlist>&nbsp;
									<asp:button id="btnRemAttach" runat="server" Text="Remove Attachment" CssClass="button" Width="120px"></asp:button></TD>
							</tr>
							<tr>
								<td class="tablecol" colSpan="2">
									<table style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: white"
										height="250" cellSpacing="0" cellPadding="0" width="700" align="left" border="0">
										<tr class="tblToolbar" bgColor="darkgray">
											<td align="center" width="700">
												<table class="raiseme" id="TopToolBar" cellSpacing="0" cellPadding="0" width="100%" align="center"
													border="0">
													<tr>
														<td><IMG src="images/leader.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('bold')" onmouseout="button_out(this);"><IMG alt="Bold" hspace="1" src="images/Bold.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('italic')" onmouseout="button_out(this);"><IMG alt="Italic" hspace="1" src="images/Italic.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('underline')" onmouseout="button_out(this);"><IMG alt="Underline" hspace="1" src="images/Under.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td><select class="Selects" onchange="EditorOnStyle(this);"><option selected>Style</option>
																<option value="Normal">Normal</option>
																<option value="Formatted">Formatted</option>
																<option value="Address">Address</option>
																<option value="Heading 1">Heading 1</option>
																<option value="Heading 2">Heading 2</option>
																<option value="Heading 3">Heading 3</option>
																<option value="Heading 4">Heading 4</option>
																<option value="Heading 5">Heading 5</option>
																<option value="Heading 6">Heading 6</option>
																<option value="Numbered List">Numbered List</option>
																<option value="Bulleted List">Bulleted List</option>
																<option value="Directory List">Directory List</option>
																<option value="Menu List">Menu List</option>
																<option value="Definition Term">Definition Term</option>
																<option value="Definition">Definition</option>
															</select>
														</td>
														<td><select class="Selects" onchange="EditorOnFont(this);">
																<option selected>Font</option>
																<option value="Arial">Arial</option>
																<option value="Arial Black">Arial Black</option>
																<option value="Arial Narrow">Arial Narrow</option>
																<option value="Comic Sans MS">Comic Sans MS</option>
																<option value="Courier New">Courier New</option>
																<option value="System">System</option>
																<option value="Tahoma">Tahoma</option>
																<option value="Times New Roman">Times New Roman</option>
																<option value="Verdana">Verdana</option>
																<option value="Wingdings">Wingdings</option>
															</select>
														</td>
														<td><select class="Selects" onchange="EditorOnSize(this);">
																<option selected>Size</option>
																<option value="1">1, 10px, 7pt</option>
																<option value="2">2, 12px, 10pt</option>
																<option value="3">3, 16px, 12pt</option>
																<option value="4">4, 18px, 14pt</option>
																<option value="5">5, 24px, 18pt</option>
																<option value="6">6, 32px, 24pt</option>
																<option value="7">7, 48px, 36pt</option>
															</select>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('strikethrough')" onmouseout="button_out(this);"><IMG alt="Strike Through" hspace="1" src="images/Strikethrough.gif" align="absMiddle"
																	vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('superscript')" onmouseout="button_out(this);"><IMG alt="Superscript" hspace="1" src="images/superscript.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('subscript')" onmouseout="button_out(this);"><IMG alt="Subscript" hspace="1" src="images/subscript.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('undo')" onmouseout="button_out(this);"><IMG alt="Undo" hspace="1" src="images/Undo.gif" align="absMiddle" vspace="0">
															</div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('redo')" onmouseout="button_out(this);"><IMG alt="Redo" hspace="1" src="images/Redo.gif" align="absMiddle" vspace="0">
															</div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td vAlign="middle"><input id="checkbox2" onclick="setMode(this.checked)" type="checkbox" name="checkbox2"></td>
														<td style="FONT: 8pt verdana,arial,sans-serif" vAlign="middle" noWrap>Edit HTML
														</td>
														<td width="5"><IMG src="images/leader.gif" align="absMiddle"></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr class="tblToolbar" bgColor="darkgray">
											<td align="left" width="700">
												<table class="raiseme" id="BottomToolBar" cellSpacing="0" cellPadding="0" width="700" border="0">
													<tr>
														<td width="5"><IMG src="images/leader.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('cut')" onmouseout="button_out(this);"><IMG alt="Cut" hspace="1" src="images/Cut.gif" align="absMiddle" vspace="0">
															</div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('copy')" onmouseout="button_out(this);"><IMG alt="Copy" hspace="1" src="images/Copy.gif" align="absMiddle" vspace="0">
															</div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('paste')" onmouseout="button_out(this);"><IMG alt="Paste" hspace="1" src="images/Paste.gif" align="absMiddle" vspace="0">
															</div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('justifyleft')" onmouseout="button_out(this);"><IMG alt="Left Align" hspace="1" src="images/Left.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('justifycenter')" onmouseout="button_out(this);"><IMG alt="Center" hspace="1" src="images/Center.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('justifyright')" onmouseout="button_out(this);"><IMG alt="Right Align" hspace="1" src="images/Right.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('justifyfull')" onmouseout="button_out(this);"><IMG alt="Justify" hspace="1" src="images/justify.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="VerticalMode()" onmouseout="button_out(this);"><IMG alt="Change Text Direction" hspace="1" src="images/Vertical.gif" align="absMiddle"
																	vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('insertorderedlist')" onmouseout="button_out(this);"><IMG alt="Ordered List" hspace="2" src="images/numlist.GIF" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('insertunorderedlist')" onmouseout="button_out(this);"><IMG alt="Unordered List" hspace="2" src="images/bullist.GIF" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('outdent')" onmouseout="button_out(this);"><IMG alt="Decrease Indent" hspace="2" src="images/deindent.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('indent')" onmouseout="button_out(this);"><IMG alt="Increase Indent" hspace="2" src="images/inindent.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="callColorDlg('ForeColor');" onmouseout="button_out(this);"><IMG alt="Font Color" hspace="2" src="images/tpaint.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="callColorDlg('BackColor');" onmouseout="button_out(this);"><IMG alt="Background Color" hspace="2" src="images/parea.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('createLink')" onmouseout="button_out(this);"><IMG alt="Insert Link" hspace="2" src="images/wlink.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('unlink')" onmouseout="button_out(this);"><IMG alt="Remove Link" hspace="2" src="images/UnLink.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="cmdExec('inserthorizontalrule')" onmouseout="button_out(this);"><IMG alt="Horizontal Rule" hspace="2" src="images/hr.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td><IMG src="images/spacer.gif" align="absMiddle"></td>
														<td>
															<div onmouseup="button_up(this);" class="cbtn" onmousedown="button_down(this);" onmouseover="button_over(this);"
																onclick="insertImage()" onmouseout="button_out(this);"><IMG alt="Insert Image" hspace="2" src="images/Image.gif" align="absMiddle" vspace="0"></div>
														</td>
														<td width="5"><IMG src="images/leader.gif" align="absMiddle"></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
												<div id="TextEditor" contentEditable="true" style="BORDER-TOP-WIDTH: 1px; BORDER-LEFT-WIDTH: 1px; BORDER-LEFT-COLOR: black; BORDER-BOTTOM-WIDTH: 1px; BORDER-BOTTOM-COLOR: black; OVERFLOW: auto; WIDTH: 700px; BORDER-TOP-COLOR: black; HEIGHT: 350px; BORDER-RIGHT-WIDTH: 1px; WORD-WRAP: break-word; BORDER-RIGHT-COLOR: black"
													indicateeditable="true" height="250"></div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="tablecol" colSpan="2"><asp:requiredfieldvalidator id="rfvContent" runat="server" EnableClientScript="False" ErrorMessage="Content is required."
										ControlToValidate="txtRTB" Display="None"></asp:requiredfieldvalidator></td>
							</tr>
							<tr height="0">
								<td>
									<OBJECT height="0" width="1" classid="clsid:5220cb21-c88d-11cf-b347-00aa00a28331">
									</OBJECT>
									<OBJECT id="dlgHelper" height="0px" width="0px" classid="clsid:3050f819-98b5-11cf-bb82-00aa00bdce0b">
									</OBJECT>
									<OBJECT id="cDialog" codeBase="http://activex.microsoft.com/controls/vb5/comdlg32.cab" height="0px"
										width="0px" classid="CLSID:F9043C85-F6F2-101A-A3C9-08002B2F49FB">
										<PARAM NAME="_ExtentX" VALUE="847">
										<PARAM NAME="_ExtentY" VALUE="847">
										<PARAM NAME="_Version" VALUE="393216">
										<PARAM NAME="CancelError" VALUE="0">
										<PARAM NAME="Color" VALUE="0">
										<PARAM NAME="Copies" VALUE="1">
										<PARAM NAME="DefaultExt" VALUE="">
										<PARAM NAME="DialogTitle" VALUE="">
										<PARAM NAME="FileName" VALUE="">
										<PARAM NAME="Filter" VALUE="">
										<PARAM NAME="FilterIndex" VALUE="0">
										<PARAM NAME="Flags" VALUE="0">
										<PARAM NAME="FontBold" VALUE="0">
										<PARAM NAME="FontItalic" VALUE="0">
										<PARAM NAME="FontName" VALUE="">
										<PARAM NAME="FontSize" VALUE="8">
										<PARAM NAME="FontStrikeThru" VALUE="0">
										<PARAM NAME="FontUnderLine" VALUE="0">
										<PARAM NAME="FromPage" VALUE="0">
										<PARAM NAME="HelpCommand" VALUE="0">
										<PARAM NAME="HelpContext" VALUE="0">
										<PARAM NAME="HelpFile" VALUE="">
										<PARAM NAME="HelpKey" VALUE="">
										<PARAM NAME="InitDir" VALUE="">
										<PARAM NAME="Max" VALUE="0">
										<PARAM NAME="Min" VALUE="0">
										<PARAM NAME="MaxFileSize" VALUE="260">
										<PARAM NAME="PrinterDefault" VALUE="1">
										<PARAM NAME="ToPage" VALUE="0">
										<PARAM NAME="Orientation" VALUE="1">
									</OBJECT>
								</td>
							</tr>
							<tr height="0">
								<td height="0"><asp:textbox id="txtRTB" runat="server" Width="0px" Height="0px"></asp:textbox></td>
							</tr>
						</TABLE>
						<BR>
						<asp:button id="cmdSendMail" runat="server" Text="Send Mail" CssClass="button"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" onclick="Reset();" type="button" value="Clear" name="cmdReset"></TD>
				</TR>
				<TR>
					<TD><BR>
						<asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg" Visible="False"></asp:label></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
