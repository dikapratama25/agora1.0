'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component

Public Class TrackConfirm
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strType As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblItem As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPR As System.Web.UI.WebControls.Label
    Protected WithEvents lblPRFail As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemFail As System.Web.UI.WebControls.Label
    Protected WithEvents trSpace As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHeaderFail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItemFail As System.Web.UI.HtmlControls.HtmlTableRow

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        ' strType :
        ' S - Submitted for approval
        ' A - Approved
        ' P - Paid
        Dim strMsg As String
        Dim i As Integer
        Dim strList As String
        Dim strAryList() As String
        strType = Request.QueryString("type")
        lblTitle.Text = "Invoice Tracking"
        lblItem.Text = ""
        trSpace.Visible = False
        trHeaderFail.Visible = False
        trItemFail.Visible = False

        Select Case strType
            Case "S"
                lblPR.Text = "The following invoice(s) has/have been submitted for approval."
            Case "A"
                lblPR.Text = "The following invoice(s) has/have been approved."
            Case "P"
                lblPR.Text = "The following invoice(s) has/have been marked as paid."
                If Session("invListFail") <> "" Then
                    lblPRFail.Text = "The following invoice(s) has/have already marked as paid previously."
                    trSpace.Visible = True
                    trHeaderFail.Visible = True
                    trItemFail.Visible = True
                End If
            Case "T"
                lblPR.Text = "The following invoice(s) has/have been put in archive." ' Michelle (CR0007) - rename 'trash' to 'archive'
                If Session("invListFail") <> "" Then
                    lblPRFail.Text = "The following invoice(s) has/have not been paid yet."
                    trSpace.Visible = True
                    trHeaderFail.Visible = True
                    trItemFail.Visible = True
                End If
        End Select

        strAryList = CType(Session("invList"), String).Split(",")
        lblItem.Text = "<ul type='disc'>"
        If strAryList.Length > 1 Then
            For i = 0 To strAryList.Length - 2
                lblItem.Text &= "<li>" & strAryList(i) & "<ul type='disc'></ul></li>"
            Next
        End If
        lblItem.Text &= "</ul>"

        If Session("invListFail") <> "" Then
            strAryList = CType(Session("invListFail"), String).Split(",")
            lblItemFail.Text = "<ul type='disc'>"
            If strAryList.Length > 1 Then
                For i = 0 To strAryList.Length - 2
                    lblItemFail.Text &= "<li>" & strAryList(i) & "<ul type='disc'></ul></li>"
                Next
            End If
            lblItemFail.Text &= "</ul>"
        End If

        lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceTracking.aspx", "pageid=" & strPageId)
        Session("invList") = ""
        Session("invListFail") = ""
    End Sub

End Class
