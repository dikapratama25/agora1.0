Imports eProcure.Component
Imports AgoraLegacy

Public Class calender
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar
    Protected WithEvents ddl_month As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_year As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            jj_ddl = 2021
            For ii_ddl = 1990 To jj_ddl
                ddl_year.Items.Insert(ii_ddl2, New ListItem(ii_ddl))

                'Me.ddl_year.Items.Add(New ListItem(ii_ddl, ii_ddl))
                ii_ddl2 = ii_ddl2 + 1
            Next
            ii_ddl = 1
            jj_ddl = 12
            For ii_ddl = 1 To jj_ddl
                ddl_month.Items.Insert(ii_ddl - 1, New ListItem(ii_ddl))
            Next
            Common.SelDdl(Now.Month, ddl_month)
            Common.SelDdl(Now.Year, ddl_year)
            'ddl_year.SelectedItem.Text = Now.Year
            'ddl_month.SelectedItem.Text = Now.Month
        End If
        Dim selectedDate As Date


        Try
            Me.Calendar1.TodaysDate = CDate("" & DateTime.Now.Day & "/" & ddl_month.SelectedItem.Text & "/" & ddl_year.SelectedItem.Text & "")
        Catch ex As Exception

            Me.Calendar1.TodaysDate = CDate("1/" & ddl_month.SelectedItem.Text & "/" & ddl_year.SelectedItem.Text & "")

        End Try




    End Sub

    Public Sub Calendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Dim strjscript As String = "<script language=""javascript"">"
        strjscript = strjscript & "window.opener.document.forms(0)." & Request.QueryString("TextBox").ToString & ".value = '" & Calendar1.SelectedDate & "';window.close();"
        strjscript = strjscript & "</script" & ">" 'Don't Ask, Tool Bug
        Literal1.Text = strjscript
    End Sub

    Public Sub Calendar1_DayRender(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs)
        If e.Day.Date = DateTime.Now().ToString("d") Then
            e.Cell.BackColor = System.Drawing.Color.LightGray
        Else
            ' do nothing
        End If
    End Sub

    Private Sub ddl_year_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddl_year.SelectedIndexChanged

    End Sub
End Class
