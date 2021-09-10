<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AssetGroupSearch.aspx.vb" Inherits="eProcure.AssetGroupSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Asset Group Selection</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <%--<% Response.Write(css)%> --%>
          <script language="javascript">
          function SelectOneOnly(objRadioButton, grdName,AssetGrp) 
        { 
      
            var i, obj; 
            for (i=0; i<document.all.length; i++) 
            { 
                obj = document.all(i); 
             
                if (obj.type == "radio") 
                { 
                    if (objRadioButton.id.substr(0, grdName.length) == grdName) 
                        if (objRadioButton.id == obj.id) 
                            obj.checked = true; 
                         else 
                            obj.checked = false; 
                } 
            }
            document.Form1.hidAssetGrp.value =  AssetGrp;    
        } 
   function selectOne()
			{			
			//alert(document.Form1.hidBudget.value);
                //var r = (eval("window.opener.document.Form1." + document.Form1.hidBudget.value));
                //alert(document.Form1.hidBudgetValue.value);
                //r.value = document.Form1.hidBudgetValue.value;
                var AssetGrp = document.Form1.hidAssetGrp.value;               
                window.opener.document.getElementById(document.Form1.hidopenerID.value).value = AssetGrp;
                window.opener.document.getElementById(document.Form1.hidopenerValID.value).value = AssetGrp;
                opener.updatebtnURL(AssetGrp,document.Form1.hidopenerHIDID.value,document.Form1.hidopenerbtn.value,"AssetGroupSearch.aspx?","AssetGroupSearch.aspx?AssetGroup");
                window.close();                                               
			}
			
			function Select()
			{
				var val, v;
				var oform = window.opener.document.Form1;
				var j;
				
				v = Form1.txtValue.value;

				re = new RegExp(':' + val + '$'); 

				for (var i=0;i<oform.elements.length;i++)
				{
					var e = oform.elements[i];

					if (e.type=="text" && e.readOnly == false && e.id.indexOf("txtAssetGroup") != -1){
					//if (e.type=="text" && re.test(e.name) )
						e.value = v;
						}
				}
				window.close();
				
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
		  
	    </script>   
</head>
<body>
    <body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
<%--        <% Response.Write(Session("w_User_tabs"))%>--%>
			<table class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
                <tr>
                    <td class="rowspacing" colspan="5" style="height: 19px">
                        <asp:Label ID="lblScreenName" runat="server" Text="Select Asset Group" CssClass="Header"></asp:Label></td>
                </tr>
								
                <tr><td class="rowspacing" colspan="5"></td></tr>	    
				<tr>
					<td class="TableHeader" colSpan="5">
                        Search Criteria
                    </td>    
				</tr>				
			    <tr>
			        <td class="TableCol" style="width: 100%"><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text=" Asset Group Code :"  CssClass="lbl" Width="138px"></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol" style="width: 100%">&nbsp;<asp:textbox id="txtAssetGroupCode" runat="server" Width="160px" MaxLength="30" CssClass="txtbox"></asp:textbox>                            
                    </td>
                    <td class="TableCol" colspan="3" style="width: 100%">
                    </td>
                </tr>				
			    <tr>
				    <td class="TableCol" style="width: 100%"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Description :" CssClass="lbl" ></asp:Label></strong>&nbsp;</td>
				    <td class="TableCol" style="width: 100%">&nbsp;<asp:textbox id="txtAssetGroupDesc" runat="server" Width="265px" MaxLength="100" CssClass="txtbox"></asp:textbox>				        
				    </td>                                    
                    <td class="TableCol" align="right">
                        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
					    <input type="button" class="button" id="cmdClear" runat="server" onclick="Reset();" value="Clear" name="cmdClear">&nbsp;
					</td>
			    </tr>
			    <tr>
			        <td class="rowspacing" colSpan="5"></td>
                </tr>	  
				<tr>
					<td class="TableCol" style="background:none; width: 145px;">
                        <td class="TableCol" colspan="1" style="background: none transparent scroll repeat 0% 0%;
                            width: 300px">
                        </td>
                        <td class="TableCol" colspan="2" style="background: none transparent scroll repeat 0% 0%">
                        </td>
				</tr>
				
			</table>
			
			<table class="AllTable" id="table2"  cellSpacing="0" cellPadding="0" width="100%">
         <TR>				   
			 <TD class="EmptyCol">
			<%--This is how to make rowspacing inside TD tag--%>					    
				<div id="AssetGroup" style="DISPLAY: none" runat="server">
					<div class="rowspacing"></div>
						<%--<asp:datagrid id="dtgCostCentre" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="CC_COSTCENTRE_INDEX">--%>
						<asp:datagrid id="dtgAssetGroup" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
								    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    <HeaderTemplate>
								        <%--<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->--%>
								    </HeaderTemplate>
								    <ItemTemplate>
									    <asp:RadioButton ID="rbtnSelection" runat="server" />
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False"><%--DataField="CC_COSTCENTRE_INDEX" SortExpression="CC_COSTCENTRE_INDEX" HeaderText="CC_COSTCENTRE_INDEX">--%>
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AG_GROUP" SortExpression="AG_GROUP" HeaderText="Asset Group Code">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AG_GROUP_DESC" SortExpression="AG_GROUP_DESC" HeaderText="Description">
									    <HeaderStyle Width="50%"></HeaderStyle>
								    </asp:BoundColumn>								   
							    </Columns>
						    </asp:datagrid>
				        </div>
				        <div id="NoRecord" style="DISPLAY: none" runat="server">	
				            <div class="rowspacing"></div>			            
			                <div class="db_wrapper">
		                        <div class="db_tbl_hd" style="background-color:#D1E1EF" > 
		                            <asp:Label ID="lblAssetGRoupCode" runat="server" Text="Asset Group Code" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="20%" ForeColor="Navy"></asp:Label>
		                            <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="lbl" BackColor="#D1E1EF" Font-Bold="True" Font-Size="11px" Font-Names="Arial" Width="30%" ForeColor="Navy"></asp:Label>						    
		                         </div>			                                                       
                                <asp:Label ID="Label7" runat="server" Text="0 record found." CssClass="lbl" Font-Bold="false"></asp:Label>						    
                           
			                </div>
				        </div>				       
				    </TD>
				</TR>   
              
            <tr>
				<td class="EmptyCol">					
					<asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="False"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>
                </td>
			</tr>	
        </table> 
		<input type="hidden" id="hidAssetGrp" name="hidAssetGrp" runat="server"/>
        <input type="hidden" id="hidopenerID" name="hidopenerID" runat="server"/>   
        <input type="hidden" id="hidopenerHIDID" name="hidopenerHIDID" runat="server"/>
        <input type="hidden" id="hidopenerbtn" name="hidopenerbtn" runat="server"/>
        <input type="hidden" id="hidopenerValID" name="hidopenerValID" runat="server"/>  
		</form>
</body>
</html>