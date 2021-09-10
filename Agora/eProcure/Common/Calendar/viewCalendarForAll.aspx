<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewCalendarForAll.aspx.vb" Inherits="eProcure.viewCalendarForAll" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>viewCalendarForAll</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
        </script> 
        <% Response.Write(css)%> 
        <script type="text/javascript">
		<!--			
			function Select(a)
			{
			    
			    document.Form1.hidDtValue.value=a;
			    selectAllItem(document.Form1.hidID.value);	
			}
			
			function selectAllItem(val)
			{ 
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp('$')				 
				for (var i=0;i<oform.elements.length;i++)
				{
				    var foo = "$";
					var e = oform.elements[i];
					var sEvents = e.name;
				 
					if (sEvents.indexOf("$") > 0)
					{					 
						if (sEvents.substring(sEvents.lastIndexOf("$")+1) == val)
					    {
					        var r = (eval("window.opener.document.Form1." + sEvents));
					        //alert(r.disabled);
					        if (r.disabled == false)
					        {
					            r.value = document.Form1.hidDtValue.value;
					        }
					    }
					}
				}
				window.close();
			}
									
		-->
		</script>
	</head>
	<body leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="300" border="0">
				<tr>
					<td class="tablecol">Month:
						<asp:dropdownlist id="ddl_month" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist>&nbsp;Year:
						<asp:dropdownlist id="ddl_year" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:calendar id="Calendar1" runat="server" ForeColor="Blue" BorderColor="Black" BackColor="Linen"
							OnSelectionChanged="Calendar1_SelectionChanged" OnDayRender="Calendar1_dayrender" FirstDayOfWeek="Monday"
							Width="180px" Height="125px" CellSpacing="1" NextPrevFormat="ShortMonth" Font-Names="Verdana"
							Font-Size="8pt" BorderStyle="Solid" CellPadding="1" EnableViewState=False>
							<TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
							<DayStyle BackColor="BlanchedAlmond"></DayStyle>
							<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
							<DayHeaderStyle Font-Size="7pt" Font-Bold="True" Height="8pt" ForeColor="#C00000"></DayHeaderStyle>
							<SelectedDayStyle ForeColor="White" BackColor="#526DA0"></SelectedDayStyle>
							<TitleStyle Font-Size="10pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="RoyalBlue"></TitleStyle>
							<OtherMonthDayStyle ForeColor="Red"></OtherMonthDayStyle>
						</asp:calendar></td>
				</tr>
				<tr>
					<td><input id="hidDtValue" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidDtValue"
							runat="server"/><input id="hidID" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server"/></td>
				</tr>
			</table>
		</form>
	</body>
</html>
