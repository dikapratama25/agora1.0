Imports AgoraLegacy
Imports eProcure.Component
Public Class PRTemplate
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents pr As System.Web.UI.WebControls.Repeater

    Dim dt As New DataTable()
    Dim intTotRecord As Integer
    Dim intRow As Integer
    Dim intRemarkCol As Integer = 13
    Dim intCnt As Integer
    Dim strC() As String
    Dim dsAllInfo, ds As DataSet
    Dim blnCustomField As Boolean = False
    Public Enum EnumPR
        icSNo = 0
        icVendorItem = 1
        icProdDesc = 2
        icMOQ = 3
        icMPQ = 4
        icQty = 5
        icUOM = 6
        icCost = 7
        icSubTotal = 8
        icBCM = 9
        icDAddr = 10
        icETD = 11
        icWTerm = 12
    End Enum
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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strBillAddr As String
        Dim objPR As New Invoice()
        Dim dtHeader As New DataTable()
        MyBase.Page_Load(sender, e)
    End Sub

End Class
