<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPCompanyDetail.aspx.vb" Inherits="eProcure.IPPCompanyDetail" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Com_Profile</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
            Dim sDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSCEDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
            Dim sDt2 As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtGstRegDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
            Dim AutoCompleteCSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCodeOnly")

            'Zulham 10072018 - PAMB
            Dim PopCalendar As String = dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox='+val+'&seldate='+txtVal.value+'")
            Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"

        </script> 
        <%response.write(Session("JQuery"))%>
		<%response.write(Session("WheelScript"))%>
		<% response.write(Session("AutoComplete")) %>
		<% Response.Write(AutoCompleteCSS)%>
		
		<script type="text/javascript">
		$(document).ready(function(){
//		    $("#txtCoyName").blur(function() {
//		    if ($("#txtCoyName").val() != "") 
//		    {		    
//		        $("#hidbtn").trigger('click'); 
//		    }
//		    }); 
          <%response.write(Session("checkCoy"))%>
		  <%response.write(Session("checkEmp"))%>
		  <%response.write(Session("checkGst"))%>
		}); 
        </script>
        
        <script language="javascript">
		<!--
		
		$(document).ready(function(){	  
            $("#txtBillGL").autocomplete("<% Response.write(typeahead) %>", {
            width: 300,
            scroll: true,                
            selectFirst: false
            });
            });
		-->
		</script>
		
		<script type="text/javascript">
	function isNumberKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                 return true;
            }    
            
	
    function isNumberCharKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122))
                    return false;

                 return true;
            }  
          
    function showHide1(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';	                   
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
                    }
            }

        		    
 function isValidCheck()
            {
                 var charCode = Form1.txtRegNo.value;//(evt.which) ? evt.which : event.keyCode
                var splChars = "*|,\":<>[]{}`\';()@&$#%- )";
               for (var i = 0; i < charCode.length; i++) {
                if (splChars.indexOf(charCode.charAt(i)) != -1){
                alert ("Invalid characters detected!"); 
                $("#txtRegNo").val("");
                return false;
                  
                }
               }
           }
 function isValidCheck2()
            {
                 var charCode = Form1.txtBankAcc.value;//(evt.which) ? evt.which : event.keyCode
                var splChars = "*|,\":<>[]{}`\';()@&$#%- )";
               for (var i = 0; i < charCode.length; i++) {
                if (splChars.indexOf(charCode.charAt(i)) != -1){
                alert ("Invalid characters detected!"); 
                $("#txtBankAcc").val("");
                return false;
                  
                }
               }
           }   
  function isValidCheck3()
            {
                 var charCode = Form1.txtPostcode.value;//(evt.which) ? evt.which : event.keyCode
                var splChars = "*|,\":<>[]{}`\';()@&$#%- ).";
               for (var i = 0; i < charCode.length; i++) {
                if (splChars.indexOf(charCode.charAt(i)) != -1){
                alert ("Invalid characters detected!"); 
                $("#txtPostcode").val("");
                return false;
                  
                }
               }
           }          
 function isValidCheckNumericOnly(str1, str2)
            {           
                 var charCode = str1;      
                var splChars = "*|,\":<>[]{}`\';()@&$#%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ[ X]";
               for (var i = 0; i < charCode.length; i++) {             
                if (splChars.indexOf(charCode.charAt(i)) != -1){
                alert ("Invalid characters detected!"); 
                $(str2).val("");
                return false;
                  
                }
               }
           }

            //Zulham 10072018 - PAMB
            function popCalendar(val) {
                txtVal = document.getElementById(val);
                window.open('<%Response.Write(PopCalendar) %>', 'cal', 'status=no,resizable=no,width=180,height=155,left=270,top=180'); }


 function confirmUpdate()
    
			{			     
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm('GST Registration No. does not match existing data. Would you like to update?'))
                {
//                    confirm_value.value = "1";
                    Form1.hidUpdate.value = "1"
                    document.getElementById("HideLinkBt").click();                   
                } 
                else {
                    confirm_value.value = "0";
                }
			}
           
//-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
	           <%--<%  Response.Write(Session("w_CompanyDet_tabs"))%>--%>
            <table class="AllTable" id="TABLE1" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="5" class="Header">
                        <asp:Label ID="Label42" runat="server" CssClass="Header" Text="Company Detail"></asp:Label></td>                    
                </tr>
                <tr>
                    <td colspan="5" class="TableHeader">
                        <asp:Label ID="lblHeader" runat="server"></asp:Label></td>                    
                </tr>
                <tr>
                    <td class="TableCol" style="width: 20%;">
                        <strong><asp:Label ID="Label1" CssClass="lbl" Font-Bold="True" runat="server" Text="Company Type"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label2" runat="server" Text=" :"></asp:Label></strong></td>
                    <td class="TableCol" colspan="4">
                        <asp:RadioButtonList ID="rbtnCoyType" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" Width="45%" AutoPostBack="True">
                            <asp:ListItem Value="V" Selected="True">Vendor</asp:ListItem>
                            <asp:ListItem Value="B">Billing to RC</asp:ListItem>
                            <asp:ListItem Value="E">Employee</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <%--<td class="TableCol" style="width: 20px"></td>
                    <td class="TableCol" style="width: 253px"></td>
                    <td class="TableCol" style="width: 288px"></td>--%>
                </tr>
                <tr runat="server" id="trCompType">
                    <td class="TableCol" style="width: 20%;">
                        <asp:Label ID="Label9" runat="server" Text="Company Abbreviation" Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label10" runat="server" Text=" :"></asp:Label></td>
                    <td class="TableCol" style="width: 30%;">
                        <asp:TextBox cssclass="txtbox" ID="txtCoyCode" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqCoyCode" runat="server" ControlToValidate="txtCoyCode"
                            Display="None" EnableClientScript="False" ErrorMessage="Company Abbreviation is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol" style="width: 5%;">
                    </td>
                    <td class="TableCol" style="width: 15%;">
                    </td>
                    <td class="TableCol" style="width: 30%;">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol" style="width: 20%;">
                        <strong><asp:Label ID="Label3" runat="server" Text="Company Name"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label4" runat="server" Text=" :" Font-Bold="True"></asp:Label></strong>
                    </td>
                    <td class="TableCol" colspan="4">
                        <asp:TextBox cssclass="txtbox" ID="txtCoyName" Width="73%" runat="server" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqCoyName" runat="server" ControlToValidate="txtCoyName"
                            Display="None" EnableClientScript="False" ErrorMessage="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="TableCol" style="width: 20%;">
                        <asp:Label ID="Label7" runat="server" Text="Business Registration No." Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label8" runat="server" Text=" :" Font-Bold="True"></asp:Label></td>
                    <td class="TableCol" style="width: 30%;">
                        <asp:TextBox cssclass="txtbox" ID="txtRegNo" runat="server" MaxLength="50" width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqRegNo" runat="server" ControlToValidate="txtRegNo"
                            Display="None" EnableClientScript="False" ErrorMessage="" ValidationGroup="Submit"></asp:RequiredFieldValidator></td>                             
                    <td class="TableCol" style="width: 5%;">
                    </td>
                    <td class="TableCol" style="width: 15%;">
                        <strong><asp:Label ID="Label5" runat="server" Text="Status"></asp:Label><span class="ErrorMsg"></span><asp:Label ID="Label6" runat="server" Text=" :"></asp:Label></strong>
                    </td>
                    <td class="TableCol" style="width: 30%;">
                        <asp:RadioButtonList ID="rbtnStatus" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" AutoPostBack="True">
                            <asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="I">Inactive</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trIPP3" runat="server">
                    <td class="TableCol">
                        <asp:Label ID="Label50" runat="server" Text="Job Grade" Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label54" runat="server" Text=" :" Font-Bold="True"></asp:Label></td> 
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtJobGrade" runat="server" MaxLength="50" width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqJobGrade" runat="server" ControlToValidate="txtJobGrade"
                            Display="None" EnableClientScript="False" ErrorMessage="Job Grade is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                       <asp:Label ID="Label51" runat="server" Text="Branch Code" Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label55" runat="server" Text=" :" Font-Bold="True"></asp:Label></td> 
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlBC" runat="server" CssClass="ddl" Width="80%"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqBC" runat="server" ControlToValidate="ddlBC"
                            Display="None" EnableClientScript="False" ErrorMessage="Branch Code is required." ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td> 
                </tr>
                <tr id="trIPP4" runat="server">
                    <td class="TableCol">
                        <asp:Label ID="Label52" runat="server" Text="Staff Cessation Effective Date :" Font-Bold="True"></asp:Label></td> 
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtSCEDate" runat="server" MaxLength="50" width="45%" contentEditable="false"></asp:TextBox><% response.write (sDt) %></td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                       <asp:Label ID="Label56" runat="server" Text="Cost Centre" Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label57" runat="server" Text=" :" Font-Bold="True"></asp:Label></td> 
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlCC" runat="server" CssClass="ddl" Width="80%"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqCC" runat="server" ControlToValidate="ddlCC"
                            Display="None" EnableClientScript="False" ErrorMessage="Cost Centre is required." ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td> 
                </tr>
                <tr id="trIPPGST1" runat="server">
                    <td class="TableCol">
                        <%--Zulham 10102018 - PAMB SST--%>
                        <strong><asp:Label ID="Label46" runat="server" Text="SST Registration No. :" Font-Bold="True"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtGstRegNo" runat="server" MaxLength="50" width="90%"></asp:TextBox></td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                        <%--Zulham 10102018 - PAMB SST--%>
                        <strong><asp:Label ID="Label47" runat="server" Text="SST Input Tax Code :"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlGstInputTaxCode" runat="server" CssClass="ddl" Width="80%" AutoPostBack="True"></asp:DropDownList>
                    </td> 
                </tr>
                <tr id="trIPPGST2" runat="server">
                    <td class="TableCol">
                        <%--Zulham 10102018 - PAMB SST--%>
                        <strong><asp:Label ID="Label48" runat="server" Text="SST Effective Date (dd/mm/yyyy):" Font-Bold="True"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtGstRegDate" runat="server" MaxLength="50" width="45%"></asp:TextBox><%--<% response.write (sDt2) %>--%><%--Removed by Jules 2015.08.21 for IPP Stage 4 Phase 2--%>
                        <a onclick="popCalendar('txtGstRegDate');" href="javascript:;"><% Response.Write(sCal)%></a> 
                        </td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                        <%--Zulham 10102018 - PAMB SST--%>
                        <strong><asp:Label ID="Label49" runat="server" Text="SST Output Tax Code :"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlGstOutputTaxCode" runat="server" CssClass="ddl" Width="80%"></asp:DropDownList>
                    </td> 
                </tr>
                
                <%--Jules 2015.08.13 - IPP Stage 4 Phase 2--%>
                <tr id="trIPPS4P2" runat="server">
                    <td class="TableCol">
                        <strong><asp:Label ID="Label59" runat="server" Text="System Validation Date (dd/mm/yyyy):" Font-Bold="True"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtSystemValDate" runat="server" MaxLength="50" width="45%" ></asp:TextBox>
                        <a onclick="popCalendar('txtSystemValDate');" href="javascript:;"><% Response.Write(sCal)%></a> 
                        </td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol"></td> 
                </tr>
                
                <%--Zulham 29062015 - IPP GST Stage 4 (CR)--%>
                 <tr id="trEmployeeInputTaxCode" runat="server" visible="false">
                    <td class="TableCol">
                    </td> 
                    <td class="TableCol">
                    </td> 
                    <td class="TableCol" ></td> 
                    <td class="TableCol" style="height: 21px">
                        <strong><asp:Label ID="Label62" runat="server" Text="GST Input Tax Code :"></asp:Label></strong></td> 
                    <td class="TableCol" style="height: 21px">
                        <asp:DropDownList ID="ddlEmpGstInputTaxCode" runat="server" CssClass="ddl" Width="80%"></asp:DropDownList>
                    </td> 
                </tr>
                <tr id="trEmployeeOutputTaxCode" runat="server" visible="false">
                    <td class="TableCol">
                    </td> 
                    <td class="TableCol">
                    </td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol" style="height: 24px">
                        <strong><asp:Label ID="Label60" runat="server" Text="GST Output Tax Code :"></asp:Label></strong></td> 
                    <td class="TableCol" style="height: 24px">
                        <asp:DropDownList ID="ddlEmpGstOutputTaxCode" runat="server" CssClass="ddl" Width="80%"></asp:DropDownList>
                    </td> 
                </tr>
                <%--End--%>
                
                <tr id="trIPP1" runat="server">
                    <td class="TableCol">
                        <%--<strong><asp:Label ID="Label43" runat="server" Text="Date of Last Status Check :" Font-Bold="True"></asp:Label></strong> --%>
                        <strong><asp:Label ID="Label63" runat="server" Text="Resident Country :"></asp:Label></strong>
                    </td>
                    <td class="TableCol">
                        <%--<asp:Button ID="cmdValidate" CssClass="button" runat="server" Text="Validate" CausesValidation="false"/><asp:textbox id="txtDtLastStatus" runat="server" CssClass="txtbox" Width="80px" visible="false" contentEditable="false" MaxLength="30"></asp:textbox>
								<asp:label id="lblDayOfLastStatus" runat="server"></asp:label>--%>
                        <asp:DropDownList ID="ddlResidentCountry" runat="server" CssClass="ddl" Enabled="false" Width="90%"></asp:DropDownList>
                    </td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                        <strong><asp:Label ID="Label44" runat="server" Text="Resident Type :"></asp:Label></strong></td> 
                    <td class="TableCol">
                        <asp:RadioButtonList ID="rbtnResidentType" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" AutoPostBack="True">
                            <asp:ListItem Value="Y" Selected="True">Resident</asp:ListItem>
                            <asp:ListItem Value="N">Non-Resident</asp:ListItem>
                        </asp:RadioButtonList>
                    </td> 
                </tr>
                <tr id="trIPP2" runat="server">
                    <td class="TableCol">
                        <strong><asp:Label ID="Label53" runat="server" Text="Category :" Font-Bold="True"></asp:Label></strong></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ddl" Width="90%"></asp:DropDownList></td>
                    <td class="TableCol"></td> 
                    <td class="TableCol">
                        <%--<strong><asp:Label ID="Label45" runat="server" Text="Resident Country :"></asp:Label></strong>--%>

                        <strong><asp:Label ID="Label64" runat="server" Text="Billing GL Code :" Font-Bold="True"></asp:Label></strong>

                    </td> 
                    <td class="TableCol">
                        <%--<asp:DropDownList ID="ddlResidentCountry" runat="server" CssClass="ddl" Enabled="false" Width="80%"></asp:DropDownList>--%>
                        <asp:TextBox cssclass="txtbox" ID="txtBillGL" runat="server" MaxLength="50" width="90%"></asp:TextBox>
                    </td> 
                </tr>
                <tr id="trIPPGST3" runat="server">
                    <td class="TableCol">
                        <%--<strong><asp:Label ID="Label58" runat="server" Text="Billing GL Code :" Font-Bold="True"></asp:Label></strong>--%>
                    </td>
                    <td class="TableCol">
                        <%--<asp:TextBox cssclass="txtbox" ID="txtBillGL" runat="server" MaxLength="50" width="90%"></asp:TextBox>--%>
                    </td>
                    <td class="TableCol"></td> 
                    <td class="TableCol"></td> 
                    <td class="TableCol"></td> 
                </tr>
                <tr>
                    <td class="TableCol" colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label11" runat="server" Text="Address" Font-Bold="True"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label12" runat="server" Text=" :" Font-Bold="True"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtAddrLine1" runat="server" MaxLength="255" Width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqAddr" runat="server" ControlToValidate="txtAddrLine1"
                            Display="None" EnableClientScript="False" ErrorMessage="Address is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtAddrLine2" runat="server" MaxLength="255" Width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtAddrLine3" runat="server" MaxLength="255" Width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label21" runat="server" Font-Bold="True" Text="Contact Person :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtContact" runat="server" MaxLength="100" Width="80%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="City"></asp:Label><span class="ErrorMsg"></span><asp:Label ID="Label14" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtCity" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                        
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label22" runat="server" Font-Bold="True" Text="Phone :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtPhone" runat="server" MaxLength="20" Width="80%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label15" runat="server" Font-Bold="True" Text="State"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label16" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="ddl" Width="90%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="ddlState"
                            Display="None" EnableClientScript="False" ErrorMessage="State is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label23" runat="server" Font-Bold="True" Text="Fax :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtFax" runat="server" MaxLength="20" Width="80%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label17" runat="server" Font-Bold="True" Text="Postcode"></asp:Label><span class="ErrorMsg"></span><asp:Label ID="Label18" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtPostcode" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                        
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label24" runat="server" Font-Bold="True" Text="Email :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtEmail" runat="server" MaxLength="100" Width="80%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Text="Country"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label20" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" CssClass="ddl" Width="90%">
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="reqCountry" runat="server" ControlToValidate="ddlCountry"
                            Display="None" EnableClientScript="False" ErrorMessage="Country is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label25" runat="server" Font-Bold="True" Text="Website :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtWebsite" runat="server" MaxLength="50" Width="80%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol" colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label26" runat="server" Font-Bold="True" Text="Payment Mode"></asp:Label>
                        <asp:Label ID="lblerr1" class="ErrorMsg" runat="server" Font-Bold="True" Text="*"/>
                        <asp:Label ID="Label27" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlPayMethod" runat="server" AutoPostBack="True" CssClass="ddl" Width="90%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqPayMethod" runat="server" ControlToValidate="ddlPayMethod"
                            Display="None" EnableClientScript="False" ErrorMessage="Payment Mode is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <strong><asp:Label ID="lblBankCharge" runat="server" Text="Waive Bank Charges? :" Visible="false"></asp:Label></strong>
                     </td>
                    <td class="TableCol" >
                        <asp:RadioButtonList ID="rbtnBankCharge" runat="server"  Visible="false" RepeatDirection="Horizontal" CssClass="rbtn" Width="80%">
                            <asp:ListItem Value="N" Text="No"></asp:ListItem>
                            <asp:ListItem Value="Y" Selected="True" Text="Yes"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="TableCol" style="height: 22px;">
                        <asp:Label ID="Label28" runat="server" Font-Bold="True" Text="Bank Code"></asp:Label>
                        <asp:Label ID="lblerr2" class="ErrorMsg" runat="server" Font-Bold="True" Text="*"/><asp:Label ID="Label29" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol" style="height: 22px;">
                        <asp:DropDownList ID="ddlBankCode" runat="server" CssClass="ddl" AutoPostBack="True" width="90%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqBankCode" runat="server" ControlToValidate="ddlBankCode"
                            Display="None" EnableClientScript="False" ErrorMessage="Bank Code is required." ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqBankCode2" runat="server" ControlToValidate="ddlBankCode"
                            Display="None" EnableClientScript="False" ErrorMessage="Bank Code is required." InitialValue="n.a." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol" style="height: 22px;">
                    </td>
                    <td class="TableCol">
                        <strong><asp:Label ID="lblCTerm" runat="server" Text="Credit Terms (Days) :"></asp:Label></strong> 
                     </td>
                    <td class="TableCol" style="height: 22px">
                        <asp:TextBox cssclass="txtbox" ID="txtCterm" runat="server" MaxLength="3" Width="15%" Text="30"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="TableCol" style="height: 22px">
                    </td>
                    <td class="TableCol" style="height: 22px">
                        <asp:TextBox cssclass="txtbox" ID="txtBankName" runat="server" width="90%"></asp:TextBox></td>
                    <td class="TableCol" style="height: 22px">
                    </td>
                  <%--  <td class="TableCol" style="width: 93px; height: 22px">
                    </td>
                    <td class="TableCol" style="width: 198px; height: 22px">
                    </td>--%>
                    <td class="TableCol">
                        <asp:Label ID="lblCurrency" runat="server" Font-Bold="True" Text="Currency"></asp:Label>
                        <asp:Label ID="lblErrCurrency" class="ErrorMsg" runat="server" Font-Bold="True" Text="*"/>
                        <asp:Label ID="lblSemiCurrency" runat="server" Font-Bold="True" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" CssClass="ddl" width="80%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqCurrency" runat="server" ControlToValidate="ddlCurrency"
                            Display="None" EnableClientScript="False" ErrorMessage="Currency is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="TableCol"">
                        <asp:Label ID="Label30" runat="server" Font-Bold="True" Text="Bank Account No."></asp:Label>
                        <%--<span class="ErrorMsg">*</span>--%>
                        <asp:Label ID="lblerr3" class="ErrorMsg" runat="server" Font-Bold="True" Text="*"/><asp:Label ID="Label31" runat="server" Font-Bold="True" Text=" :"></asp:Label><br />
                        <asp:Label ID="Label32" runat="server" Text="(Not applicable to Banker's Cheque)"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBankAcc" runat="server" MaxLength="30" width="90%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqBankAcc" runat="server" ControlToValidate="txtBankAcc"
                            Display="None" EnableClientScript="False" ErrorMessage="Bank account no. is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="lblNostro" runat="server" Font-Bold="True" Text="Nostro Income :" Visible="False"></asp:Label>
                    </td>
                    <td class="TableCol">
                        <asp:RadioButtonList ID="rbtnNostro" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" Visible="False">
                            <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label33" runat="server" Font-Bold="True" Text="Bank Address :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBAddrLine1" runat="server" MaxLength="255" width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBAddrLine2" runat="server" MaxLength="255" width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBAddrLine3" runat="server" MaxLength="255" width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label34" runat="server" Font-Bold="True" Text="City :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBCity" runat="server" MaxLength="50" width="90%"></asp:TextBox></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label35" runat="server" Font-Bold="True" Text="State :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlBState" runat="server" CssClass="ddl" width="90%">
                        </asp:DropDownList></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label36" runat="server" Font-Bold="True" Text="Postcode :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:TextBox cssclass="txtbox" ID="txtBPostcode" runat="server" MaxLength="9" width="90%"></asp:TextBox></td>
                        
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                        <asp:RegularExpressionValidator ID="valBPostcode" runat="server" ControlToValidate="txtBPostcode"
                                                        Display="none" EnableClientScript="false" ErrorMessage="Minimum length of Postcode is 5."
                                                        ValidationExpression=".{5}.*" ValidationGroup="Submit"/>
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label37" runat="server" Font-Bold="True" Text="Country :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlBCountry" runat="server" Autopostback="true" CssClass="ddl" width="90%">
                        </asp:DropDownList></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol" colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label38" runat="server" Font-Bold="True" Text="Conventional IBS GL Code"></asp:Label><span class="ErrorMsg">*</span><asp:Label ID="Label39" runat="server" Text=" :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlConGLCode" runat="server" CssClass="ddl" width="90%">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqConIBSGLCode" runat="server" ControlToValidate="ddlConGLCode"
                            Display="None" EnableClientScript="False" ErrorMessage="Conventional IBS GL Code is required." ValidationGroup="Submit"></asp:RequiredFieldValidator></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label40" runat="server" Font-Bold="True" Text="Non Conventional IBS GL Code :"></asp:Label></td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlNonConGLCode" runat="server" CssClass="ddl" width="90%">
                        </asp:DropDownList></td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                    <td class="TableCol">
                    </td>
                </tr>
                <tr>
                    <td class="TableCol" colspan="5" style="height: 19px">
                    </td>
                </tr>
                <div id="divInactiveReason" runat="server">
                <tr valign="top" >
					<td class="tablecol">&nbsp;<strong>Reason to Deactivate</strong>&nbsp;:</td>
					<td class="tablecol" colspan = "4"><asp:textbox id="txtInactiveReason" runat="server" CssClass="txtbox"  MaxLength="1000"
					TextMode="MultiLine" Height="57px" Width="95%"></asp:textbox></td>
				</tr>
				</div>
			<%--	<tr>
                    <td class="TableCol" colspan="5" style="height: 19px">
                    </td>
                </tr>--%>
                <tr>
                    <td class="EmptyCol" colspan="5">
                        <span class="ErrorMsg">*</span><asp:Label ID="Label41" runat="server" Text=" indicates required field"></asp:Label></td>
                </tr>
                <tr>
                    <td class="EmptyCol" colspan="5">
                    </td>
                </tr>
                <tr>
                    <td class="EmptyCol" colspan="2">
                        <asp:Button ID="cmdSave" CssClass="button" runat="server" Text="Save" ValidationGroup="Submit" />
                        <asp:Button ID="cmdAdd" CssClass="button" runat="server" Text="Add" ValidationGroup="Add" />&nbsp;
                        <asp:Button ID="hidbtn" runat="server" Text=""/>
                        <asp:Button ID="hidbtn2" runat="server" Text=""/>
                        <asp:Button ID="hidbtn3" runat="server" Text=""/>
                        <asp:LinkButton ID="HideLinkBt" runat="server"></asp:LinkButton>
                        <%--<asp:Button ID="btnhidUpdate" runat="server" Text="Update GST" OnClick = "btnHidUpdate_Click" OnClientClick="confirmUpdate()"/>--%>
                        <asp:TextBox ID="hidTxtPrevGst" runat="server" Visible="false"></asp:TextBox>                        
                        <input class="txtbox" id="hidUpdate" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidUpdate" runat="server"/>
                        </td>
                    <td class="EmptyCol">
                    </td>
                    <td class="EmptyCol">
                    </td>
                    <td class="EmptyCol">
                    </td>
                </tr>
                <tr>
                    <td class="EmptyCol" colspan="5">
                        <asp:ValidationSummary EnableClientScript="false" ID="vldsumm" runat="server" Width="80%" ValidationGroup="Submit" />
                    </td>
                </tr>
                <tr>
                    <td class="EmptyCol" colspan="5">
                    </td>
                </tr>
                <tr>
                    <td class="EmptyCol" colspan="5">
                        <asp:HyperLink ID="lnkBack" runat="server"><strong>&lt; Back</strong></asp:HyperLink></td>
                </tr>
            </table>
           
			
		</FORM>
	</BODY>
</HTML>
