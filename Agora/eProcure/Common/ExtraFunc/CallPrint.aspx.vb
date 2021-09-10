Imports AgoraLegacy

Public Class CallPrint
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents test As System.Web.UI.HtmlControls.HtmlInputButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            Button1.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("ExtraFunc", "FramePrinting.aspx") & "','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');")
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dtPrint As New DataTable
        Dim dr As DataRow
        Session("dtprint") = Nothing
        dtPrint.Columns.Add("inv_no", Type.GetType("System.String")) '//product code
        dtPrint.Columns.Add("bcom", Type.GetType("System.String")) '//supplier id
        dr = dtPrint.NewRow
        dr("inv_no") = "PO/27"
        dr("bcom") = "moo3"
        dtPrint.Rows.Add(dr)

        dr = dtPrint.NewRow
        dr("inv_no") = "PO/26"
        dr("bcom") = "moo3"
        dtPrint.Rows.Add(dr)

        dr = dtPrint.NewRow
        dr("inv_no") = "PO/25"
        dr("bcom") = "moo3"
        dtPrint.Rows.Add(dr)

        Session("dtprint") = dtPrint
    End Sub
End Class
