<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GLCode.aspx.vb" Inherits="eProcure.GLCode"
    Trace="False" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GLCode</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <% Response.Write(Session("JQuery")) %>
    <% Response.Write(CSS)%>
    <% Response.Write(Session("AutoComplete")) %>

    <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCodeOnly") <%--Jules 2014.03.18--%>
    </script>

    <%response.write(Session("WheelScript"))%>

    <script type="text/javascript">
		    //Jules 2014.03.18
		    $(document).ready(function(){		    
		    $("#txtAddGLCode2").autocomplete("<% Response.write(typeahead) %>", {
                width: 250,
                scroll: true,
                selectFirst: false                
            });
            $("#txtAddGLCode2").result(function(event, data, formatted) {
                if (data)
                {
                    $("#hidGLCode2").val(data[1]);       
                }
                else
                {
                    $("#hidGLCode2").val("");    
                }    
                });
            $("#txtAddGLCode2").blur(function() {
            
            var hidglcode2 = document.getElementById("hidGLCode2").value;                        
            if(hidglcode2 == "")
            {                
                $("#txtAddGLCode2").val("");
            }
            else
            {                
                //document.all("btnhidden").click();         
                var glcode = document.getElementById("txtAddGLCode2").value;           
                var table = document.getElementById('dgGLCategory');                             
                for (var i = 1; i < table.rows.length-2; i++) //the rows we want minus the header and the "extra" 2 rows at the end.
                {
                    if (table.rows[i].cells[1].innerText == glcode)
                    {
                        //alert(table.rows[i].cells[1].innerText + " = " + glcode);
                        alert("Duplicate record found.");    
                        $("#txtAddGLCode2").val("");
                        $("#hidGLCode2").val("");                                               
                    }    
                }
            }
            });                
            });
            
			function clearValue(id){
				if( document.getElementById(id) ){
					document.getElementById(id).value = "";	
				}
				//Jules 2014.03.18
				var optGLCatCode = document.getElementById('optGLCodeCategoryCode'); //Jules 2014.03.18			
			    if ( optGLCatCode.checked ){
				    //clearValue('txtCategoryCode');
				    document.getElementById('txtCategoryCode').value = "";	
				} //Jules end.
			}	
			function selectAll(){
				SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
			}
			
			function selectAllGLCategory(){			 
				SelectAllG("dgGLCategory_ctl02_chkAll","chkSelection");
			}
			
			function checkChildGLCategory(id)
			{			    
				checkChildG(id,"dgGLCategory_ctl02_chkAll","chkSelection");
			}
			
			function selectAllCategory(){			 
				SelectAllG("dgCategory_ctl02_chkAll","chkSelection");
			}
			
			function checkChildCategory(id)
			{			    
				checkChildG(id,"dgCategory_ctl02_chkAll","chkSelection");
			}
			
			function DisplayAddPanel(){
    			var optGLCatCode = document.getElementById('optGLCodeCategoryCode'); //Jules 2014.03.18
	            //Jules 2014.03.18
				if ( optGLCatCode.checked ){
                    //$("input:checkbox").attr('checked', false);	                    	            
				    document.all("btnhidden2").click();		
				    //document.getElementById('btnhidden2').click(); 
				    
				} //Jules end.
				else {			
				var div_add = document.getElementById("hide");
				div_add.style.display ="";
				document.getElementById('vldSumm').innerHTML = "";
				var catRadioBtn = document.getElementById('catCodeRadioBtn'); 
				
				if( catRadioBtn.checked ){
					if( document.getElementById('glDesclbl') ){
						document.getElementById('glDesclbl').style.display="none";
					}
					if( document.getElementById('txtGLCodeDescription') ){
						document.getElementById('txtGLCodeDescription').style.display="none";
					}
				}else {
					if( document.getElementById('glDesclbl') ){
						document.getElementById('glDesclbl').style.display="";
					}
					if( document.getElementById('txtGLCodeDescription') ){
						document.getElementById('txtGLCodeDescription').style.display="";
					}
					
				}
				if ( !optGLCatCode.checked ){
				    if( document.getElementById('txtGLCodeDescription') ){
    					document.getElementById('txtGLCodeDescription').value = "";
				    }
				    var txtGL = document.getElementById('txtAddGLCode');
				    if( txtGL ){
    					txtGL.value = "";
					    txtGL.disabled = false ;
					    txtGL.focus();
				    }
				}
				}
			}
			function HideAddPanel(){
				clearValue('txtAddGLCode');
				clearValue('txtGLCodeDescription');
				var div_add = document.getElementById("hide");
				document.getElementById('btnClrGL').value = 'Clear' ; 
				div_add.style.display ="none";
				document.getElementById('txtAddGLCode').disabled = false ;
                
				//Jules 2014.03.18
                var btnModify = document.getElementById('cmd_modify');
    			var btnDelete = document.getElementById('cmd_delete');				
    		    btnModify.style.display=""; 
	        	btnDelete.style.display="";
	        	document.all("btnhidden3").click();		   
                
				
				var optGLCatCode = document.getElementById('optGLCodeCategoryCode'); 
				if ( optGLCatCode.checked ){				    
				    var div_Category = document.getElementById('divCategory');
				    var div_hideGLCategory = document.getElementById('divGLCategory');
				    var div_AddGLCategory = document.getElementById('divAddGLCategory');
				    div_Category.style.display ="none";
				    div_hideGLCategory.style.display ="none";
				    div_AddGLCategory.style.display ="none";
				    $("input:checkbox").attr('checked', false);
//				    var btnModify = document.getElementById('cmd_modify');
//    			    var btnDelete = document.getElementById('cmd_delete');				
//    		    	btnModify.style.display=""  
//	        		btnDelete.style.display=""	        		
				} //Jules end.
			}
			function ValidateGLCode(){
					var catRadioBtn = document.getElementById('catCodeRadioBtn');
					var glCode = document.getElementById('txtAddGLCode').value ;
					if( glCode == '' ){
						if( !catRadioBtn.checked ){
								alert('GL Code  required.');
						}else {
								alert('Category Code  required.');
						}
						document.getElementById('txtAddGLCode').focus();
						return false ;
					}
					if( glCode.indexOf( '(' ) >= 0 || glCode.indexOf( ')' ) >= 0 ){
						if( !catRadioBtn.checked ){
							alert( 'GL Code cannot contain \"(\" or \")\" . ');
							document.getElementById('txtAddGLCode').focus();
							return false ;
						}
					}
					if( !catRadioBtn.checked ){
							var desc = document.getElementById('txtGLCodeDescription').value ;
							if( desc == '' ){
								alert('GL Description required.');
								document.getElementById('txtGLCodeDescription').focus();
								return false ;
							}
					}
					return true ;
			}
			function Clear(){
			    var optGLCatCode = document.getElementById('optGLCodeCategoryCode'); //Jules 2014.03.18		
			    if ( optGLCatCode.checked ){				      
				        document.getElementById('txtAddGLCode2').value = "";	
				    } //Jules end.
			    else {
    				if( !document.getElementById('txtAddGLCode').disabled ){
					    clearValue('txtAddGLCode');
				    }
				    clearValue('txtGLCodeDescription');				
    				//Jules 2014.03.18
		    	}					   
			}
    </script>

</head>
<body onload="document.getElementById('txtGLCode').focus();" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table class="alltable" id="table1" cellspacing="0" cellpadding="0" width="100%"
            border="0">
            <tr>
                <td class="header" style="height: 25px">
                    <asp:Label ID="headerlbl" runat="server" Font-Bold="True" Width="300px">GL Code</asp:Label></td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant GL Code(s). Click Add button to add new GL Code. Select GL Code and click Modify button to modify."></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                    <p>
                        <asp:RadioButton ID="glRadioBtn" runat="server" AutoPostBack="True" GroupName="criteria"
                            Text="GL Code"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="catCodeRadioBtn" runat="server" AutoPostBack="True" GroupName="criteria"
                            Text="Category Code"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="optGLCodeCategoryCode" runat="server" AutoPostBack="True" GroupName="criteria"
                            Text="GL Code - Category Code Assignment"></asp:RadioButton><br>
                        <br>
                    </p>
                </td>
            </tr>
            <tr>
                <td class="tablecol">
                    <table class="alltable" id="table2" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td class="TableHeader" colspan="2">
                                &nbsp;Search Criteria</td>
                        </tr>
                        <tr>
                            <td class="TableCol">
                                &nbsp;<strong>
                                    <asp:Label ID="srchCriLbl" runat="server">GL Code</asp:Label></strong>&nbsp;:
                                <asp:TextBox ID="txtGLCode" CssClass="txtbox" runat="server"></asp:TextBox>&nbsp;
                                &nbsp;<strong><asp:Label ID="srchCriLb2" runat="server" Visible="false">Category Code :</asp:Label></strong>&nbsp;
                                <asp:TextBox ID="txtCategoryCode" CssClass="txtbox" runat="server" Visible="false"></asp:TextBox>&nbsp;
                            </td>
                            <td align="right">
                                <asp:Button ID="cmd_search" Text="Search" CssClass="button" runat="server" CausesValidation="False">
                                </asp:Button>&nbsp;
                                <input class="button" id="btnclr" onclick="clearValue('txtGLCode');" type="button"
                                    value="Clear">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="rowspacing">
                </td>
            </tr>
            <tr>
                <td>
                    <div id="hide" style="display: none" runat="server">
                        <table class="alltable" id="table3" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="tableheader" colspan="2">
                                    &nbsp;Please&nbsp;enter&nbsp;the&nbsp;following&nbsp;value
                                </td>
                            </tr>
                            <tr>
                                <td class="tablecol" style="height: 25px" nowrap>
                                    &nbsp;<strong>
                                        <asp:Label ID="addLbl" runat="server">GL Code</asp:Label></strong><asp:Label ID="Label1"
                                            runat="server" CssClass="errormsg">*</asp:Label>&nbsp;:
                                    <asp:TextBox ID="txtAddGLCode" CssClass="txtbox" runat="server"></asp:TextBox>&nbsp;
                                    <strong>
                                        <asp:Label ID="glDesclbl" runat="server">GL Description</asp:Label></strong><asp:Label
                                            ID="Label2" runat="server" CssClass="errormsg">*</asp:Label>&nbsp;
                                    <asp:TextBox ID="txtGLCodeDescription" CssClass="txtbox" runat="server" Text="txtGLCodeDescription"></asp:TextBox>&nbsp;
                                </td>
                                <td class="tablecol" align="right">
                                    <asp:Button ID="saveGL" Text="Save" CssClass="button" runat="server"></asp:Button>&nbsp;
                                    <input class="button" id="btnClrGL" onclick="Clear();" type="button" value="Clear"
                                        runat="server">&nbsp;<input class="button" id="btncancel" onclick="HideAddPanel();"
                                            type="button" value="Cancel" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td class="emptycol">
                                    <asp:Label ID="Label3" runat="server" CssClass="errormsg">*</asp:Label>&nbsp;indicates
                                    required field<%--<asp:requiredfieldvalidator id="rfv_code" runat="server" Display="None" ErrorMessage="Required field missing"
											ControlToValidate="txtAddGLCode"></asp:requiredfieldvalidator>--%></td>
                            </tr>
                            <tr>
                                <td class="emptycol">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg"></asp:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--Jules 2014.03.18--%>
                    <div id="divAddGLCategory" style="display: none" runat="server">
                        <table class="alltable" id="table4" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="tableheader" colspan="2">
                                    <asp:Label ID="lblGLCategoryHeader" runat="server" Text="Add GL Code - Category Code"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tablecol" style="height: 25px" nowrap>
                                    &nbsp;<strong>
                                        <asp:Label ID="Label4" runat="server">GL Code</asp:Label></strong><asp:Label ID="Label5"
                                            runat="server" CssClass="errormsg">*</asp:Label>&nbsp;:
                                    <asp:TextBox ID="txtAddGLCode2" CssClass="txtbox" runat="server"></asp:TextBox>&nbsp;
                                    <input id="hidGLCode2" type="hidden" runat="server"  />
                                </td>
                                <td class="tablecol" align="right">
                                    <asp:Button ID="saveGLCategory" Text="Save" CssClass="button" runat="server"></asp:Button>&nbsp;
                                    <input class="button" id="clearGLCategory" onclick="Clear();" type="button" value="Clear"
                                        runat="server">&nbsp;<input class="button" id="btnCancelGLCagetory" onclick="HideAddPanel();"
                                            type="button" value="Cancel" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td class="emptycol">
                                    <asp:Label ID="Label8" runat="server" CssClass="errormsg">*</asp:Label>&nbsp;indicates
                                    required field</td>
                            </tr>
                            <tr>
                                <td class="emptycol">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vldSumm2" runat="server" CssClass="errormsg"></asp:ValidationSummary>
                                </td>
                                <asp:Button ID="btnhidden" runat="server" CssClass="Button" Text="btnhidden" Style="display: none">
                                </asp:Button>
                                <asp:Button ID="btnhidden2" runat="server" CssClass="Button" Text="btnhidden2" Style="display: none">
                                </asp:Button>
                                <asp:Button ID="btnhidden3" runat="server" CssClass="Button" Text="btnhidden3" Style="display: none">
                                </asp:Button>
                            </tr>
                            <tr>
                                <td class="emptycol">
                                    <div id="divCategory" style="display: none" runat="server">
                                        <asp:DataGrid ID="dgCategory" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False"
                                            DataSource="<%# LoadCategoryDataSource() %>" DataKeyField="DATA_COL" OnSortCommand="dgCategory_SortCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Delete">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                                        </asp:CheckBox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="DATA_COL" SortExpression="DATA_COL">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--Jules 2014.03.18 end--%>
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                    <p>
                        <asp:DataGrid ID="MyDataGrid" runat="server" Width="100%" OnSortCommand="SortCommand_Click"
                            AllowSorting="True" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False"
                            DataSource="<%# LoadDataSource() %>" DataKeyField="DATA_COL">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                        </asp:CheckBox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="DATA_COL" SortExpression="DATA_COL">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="CBG_B_GL_DESC" SortExpression="CBG_B_GL_DESC"
                                    HeaderText="GL Description"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid></p>
                </td>
            </tr>
            <%--Jules #2657 2013.12.09--%>
            <tr>
                <td class="emptycol">
                    <div id="divGLCategory" style="display: none" runat="server">
                        <asp:DataGrid ID="dgGLCategory" runat="server" Width="100%" OnSortCommand="SortCommand2_Click"
                            AllowSorting="True" OnPageIndexChanged="dgGLCategory_PageIndexChanged" AutoGenerateColumns="False" DataSource="<%# LoadGLCategoryDataSource() %>"
                            DataKeyField="DATA_COL">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                        </asp:CheckBox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="DATA_COL" SortExpression="DATA_COL">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="CBGC_B_CATEGORY_CODE" HeaderText="Category Code"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td class="emptycol">
                    <div id="divCategory" style="display: none" runat="server">
                       <asp:DataGrid ID="dgCategory" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False"
                            DataSource="<%# LoadCategoryDataSource() %>" DataKeyField="DATA_COL">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All">
                                        </asp:CheckBox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="DATA_COL" SortExpression="DATA_COL">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>                                                    
                            </Columns>
                        </asp:DataGrid>                        
                      </div>
                </td>
            </tr>--%>
            <%--#2657 end--%>
            <tr>
                <td class="emptycol">
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                    <input class="button" id="btnAdd" onclick="DisplayAddPanel();" type="button" value="Add">&nbsp;
                    <asp:Button ID="cmd_modify" Text="Modify" CssClass="button" runat="server" CausesValidation="False"
                        Enabled="False"></asp:Button>&nbsp;
                    <asp:Button ID="cmd_delete" runat="server" Width="64px" Text="Delete" CssClass="button"
                        CausesValidation="False"></asp:Button></td>
            </tr>
            <tr>
                <td class="emptycol">
                </td>
            </tr>
            <tr>
                <td class="emptycol">
                    <strong></strong>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
