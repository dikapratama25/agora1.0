<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PersonalDetails.aspx.vb" Inherits="eProcure.PersonalDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>PersonalDetails</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
			function resetForm()
			{		
				document.getElementById("lblMsg2").innerText="";
				ValidatorReset()
							
			}
		
			function validateEmail(oSrc, args)
			{
				//debugger;
				var rx = new RegExp(Form1.hidRegex.value); //'\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*');///^
				var matches, email;
				var i, cnt;
				var str = args.Value;
				var strEmail = str.split(String.fromCharCode(13));
				
				cnt = 0;

				for (i=0; i < strEmail.length; i++){
					email = trim(strEmail[i]); 
					matches = rx.exec(email);
					if (!(matches != null && email == matches[0]))
						cnt = cnt + 1;
				}
				
				if (cnt > 0) {
					args.IsValid = false;
					//Form1.document.getElementById('cvEmail').errormessage = 'Invalid CC List.';
				}
				else {
					args.IsValid = true;
					//Form1.document.getElementById('cvEmail').errormessage = '';
				}

				return args.IsValid;
			}
		
			function trim(str)
			{
			    return str.replace(/^\s*|\s*$/g,"");
			}
		-->
		</script>
		<script language="VBSCRIPT">
			Sub txtCCList_OnKeyUp()
				Dim iCounter
				Dim iIndex
			
				If instr(1, form1.txtCCList.value, chr(13)) Then
					iCounter = 0
					
					For iIndex = 1 to Len(form1.txtCCList.value)
						iIndex = instr(iIndex, form1.txtCCList.value, chr(13))
						If iIndex = 0 Then
							Exit For
						End If
						iCounter = iCounter + 1
					Next
				End If				
			End Sub

			Sub txtCCList_OnKeyPress()
				Dim iCounter
				Dim iIndex
			
				If instr(1, form1.txtCCList.value, chr(13)) Then
					iCounter = 0
					
					For iIndex = 1 to Len(form1.txtCCList.value)
						iIndex = instr(iIndex, form1.txtCCList.value, chr(13))
						If iIndex = 0 Then
							Exit For
						End If
						
						iCounter = iCounter + 1
					Next
				End If
				
				If iCounter >= 9 Then
					If window.event.keyCode = 13 Then
						window.event.keyCode = 0
					End If
				End If
			End Sub
	
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_PerDetail_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
				<td class="linespacing1"></td>
			</tr>
			<tr>
				    <td colspan="2">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Modify the relevant field and click the Save button to save the changes."
					    ></asp:label>

				    </td>
			    </tr>
			      <tr>
				<td class="linespacing1"></td>
			</tr>
				<tr class="tablecol">
					<td class="TableHeader" colspan="2">&nbsp;Modify Personal Details</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 3px" ></td>
				</tr>
				
				<tr class="tablecol">
					<td width="25%"><strong>&nbsp;User ID</strong>&nbsp;:</td>
					<td width="75%"><asp:label id="lblUserID" runat="server"></asp:label></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;User Name</strong><span class="errormsg">*</span>&nbsp;:</td>
					<td><asp:textbox id="txtUserName" runat="server" Width="210px" CssClass="txtbox" MaxLength="200"></asp:textbox><asp:requiredfieldvalidator id="vldName" runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name is Required."
							Display="None"></asp:requiredfieldvalidator></td></tr> 
				<tr id="trUserRole" class="tablecol" runat="server">
					<td align="left" ><strong>&nbsp;<asp:Label ID="lblUserGrp" runat="server" ></asp:label></strong>&nbsp;:
					</td>
					<td><asp:label id="lblUserRole" runat="server"></asp:label></td>
				</tr>
				<tr id="trDept" class="tablecol" runat="server">
					<td class="tablecol" align="left"><strong>&nbsp;Department Name</strong>&nbsp;:
					</td>
					<td><asp:label id="lblDeptName" runat="server"></asp:label></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Email</strong><span class="errormsg"><strong>*</strong></span>&nbsp;:</td>
					<td><asp:textbox id="txtEmail" runat="server" Width="210px" CssClass="txtbox" MaxLength="50"></asp:textbox>&nbsp;
						<asp:requiredfieldvalidator id="vldEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required."
							Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email."
							Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Phone</strong>&nbsp;:</td>
					<td><asp:textbox id="txtPhone" runat="server" Width="210px" CssClass="txtbox" MaxLength="50"></asp:textbox>&nbsp;
						<asp:regularexpressionvalidator id="revPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Invalid Phone."
							Display="None" ValidationExpression="^(\(?\+?[a-zA-Z0-9]*\)?)?[a-zA-Z0-9_\- \(\)]*$"></asp:regularexpressionvalidator></td> 
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Fax</strong>&nbsp;:</td>
					<td><asp:textbox id="txtFax" runat="server" Width="210px" CssClass="txtbox" MaxLength="50"></asp:textbox>&nbsp;
						<asp:regularexpressionvalidator id="revFax" runat="server" ControlToValidate="txtFax" ErrorMessage="Invalid Fax."
							Display="None" ValidationExpression="^(\(?\+?[a-zA-Z0-9]*\)?)?[a-zA-Z0-9_\- \(\)]*$"></asp:regularexpressionvalidator></td> 
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Designation</strong><span class="errormsg">*</span>&nbsp;:</td>
					<td><asp:textbox id="txtDesination" runat="server" Width="210px" CssClass="txtbox" MaxLength="50"></asp:textbox><asp:requiredfieldvalidator id="vldDesignation" runat="server" ControlToValidate="txtDesination" ErrorMessage="Designation is required."
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 19px"></td>
					<td style="HEIGHT: 19px"></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;New Password</strong>&nbsp;:</td>
					<td><asp:textbox id="txtNewPW" runat="server" Width="210px" CssClass="txtbox" MaxLength="20" TextMode="Password"></asp:textbox></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Confirm New Password</strong>&nbsp;:</td>
					<td><asp:textbox id="txtConfNPW" runat="server" Width="210px" CssClass="txtbox" MaxLength="20" TextMode="Password"></asp:textbox>&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Password Expiration</strong>&nbsp;:
					</td>
					<td><asp:label id="lblExpPW" runat="server"></asp:label></td>
				</tr>
				<tr class="tablecol">
					<td></td>
					<td></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Records display per page</strong>&nbsp;:</td>
					<td><asp:dropdownlist id="cboPageCnt" runat="server" CssClass="ddl"></asp:dropdownlist></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Staff Claim Email Notification</strong>&nbsp;:</td>
					<td><asp:radiobutton id="rdOn" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpSC" Checked="True"
							Text="On"></asp:radiobutton><asp:radiobutton id="rdOff" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpSC" Text="Off"></asp:radiobutton></td>
				</tr>
				<tr class="tablecol" id="trCCList" valign="top" runat="server">
					<td noWrap>&nbsp;<strong>CC List</strong>&nbsp;:</td>
					<td><asp:textbox id="txtCCList" runat="server" Width="410px" CssClass="listtxtbox" TextMode="MultiLine"
							Rows="2"></asp:textbox><br/>a)This CC field lets you send the carbon copy email to a list of people you specified.
							<br/>
							b)You may send up to 10 email addresses. Please enter one email account per line.<asp:customvalidator id="cvEmail" runat="server" ControlToValidate="txtCCList" ErrorMessage="Invalid CC List"
							Display="None" ClientValidationFunction="validateEmail"></asp:customvalidator></td>
				</tr>
				<tr class="emptycol">
					<td colspan="2"><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"></td>
				</tr>
				<tr valign="top">
					<td colspan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<input class="button" id="cmdReset" onclick="resetForm()" type="button" value="Reset" name="cmdReset"
							runat="server">&nbsp;</td>
				</tr>
				<tr valign="top">
					<td class="emptycol" colspan="2"><input class="txtbox" id="hidDeptID" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidDeptID" runat="server"/><input class="txtbox" id="hidStatus" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidDeptID" runat="server"/><input class="txtbox" id="hidDelInd" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidDeptID" runat="server"/><input class="txtbox" id="hidAppLimit" style="WIDTH: 43px; HEIGHT: 18px" type="hidden"
							name="hidPOAppLimitID" runat="server"/><input class="txtbox" id="hidPOAppLimit" style="WIDTH: 43px; HEIGHT: 18px" type="hidden"
							size="1" name="hidDeptID" runat="server"/><input class="txtbox" id="hidNewPW" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidNewPW" runat="server"/><input class="txtbox" id="hidConPW" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidConPW" runat="server"/><input class="txtbox" id="hidRegex" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" value="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
							name="hidRegex"/><input class="txtbox" id="hidInvAppLimit" style="WIDTH: 43px; HEIGHT: 18px" type="hidden"
							size="1" name="hidInvAppLimit" runat="server"/></td>
				</tr>
				<tr valign="top">
					<td colspan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary><asp:label id="lblMsg2" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
