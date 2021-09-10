Public Class calendar
    Inherits System.Web.UI.Page
    Protected WithEvents control As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        control.Value = Request.QueryString("textbox").ToString()
        If Request.QueryString("seldate") <> String.Empty Then
            Calendar1.SelectedDate = Request.QueryString("seldate")
        Else
            Calendar1.SelectedDate = Today.Date
        End If
    End Sub

    Private Sub Calendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Dim myCulture As System.Globalization.CultureInfo
        'Dim df As New System.Globalization.DateTimeFormatInfo()
        'df = myCulture.DateTimeFormat()
        Dim strScript As String = "<script>window.opener.document.forms(0)." + control.Value + ".value = '"
        strScript += Calendar1.SelectedDate.ToString("d", myCulture) 'Wheel.Components.Common.FormatWheelDate(Components.WheelDateFormat.LongDate, Calendar1.SelectedDate) 
        strScript += "';self.close()"
        strScript += "</" + "script>"
        RegisterClientScriptBlock("anything", strScript)
    End Sub
End Class
