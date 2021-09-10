Imports AgoraLegacy
Imports eProcure.Component

Public Class errorpage2
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Rfq_num As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemExist As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trExistHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trExistItem As System.Web.UI.HtmlControls.HtmlTableRow
    Dim strType As String
    Dim PO_NO As String
    Protected WithEvents trConfirm As System.Web.UI.HtmlControls.HtmlTableRow
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        ' strType :
        ' 1 - RFQ NUMBER CREATED; 
        ' 3 - RFQ SENT
        '2 - RFQ EXIST
        '   strType = Request.QueryString("type")
        Dim strDetail As String
        Dim strAry() As String
        Dim i As Integer
        If Request.QueryString("action") = "ack" Then
            lblTitle.Text = "Cancellation Acknowledgment"
            lbl_Rfq_num.Text = "CR Acknowledged"
            strAry = Split(Request.QueryString("item"), ",")
            strDetail = "<ul type='disc'>"
            For i = 0 To strAry.Length - 1
                strDetail &= "<li>" & strAry(i) & ".<ul type='disc'></ul></li>"
            Next
            strDetail &= "</ul>"
            lblItemExist.Text = strDetail
            'lblItemExist.CssClass = ""
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "ViewCancel.aspx", "filetype=1&side=v&pageid=" & strPageId)
        Else
            PO_NO = Request.QueryString("PO_NO")
            lblTitle.Text = "Purchase Requisition"


            'Dim dsItem As New DataSet
            'Dim i As Integer
            'dsItem = objPR.getRPVendorItemList(PRid)

            Dim strItemExist() As String
            Dim strItemAdd() As String
            '//remark by MOO ????
            'strItemExist = CType(Session("ItemExist"), String).Split(",")
            'strItemAdd = CType(Session("ItemAdd"), String).Split(",")
            lbl_Rfq_num.Text = "Purchase Order Number : <b>" & PO_NO & "</b>"
            lblItemExist.Text = "Quantity Cancel is Greater then Outstanding Quantity"
            '*******************meilai 5/1/2005 pass in page id****************************
            'lnkBack.NavigateUrl = "POViewB2.aspx?filetype=1&side=b"
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2.aspx", "filetype=1&side=b&pageid=" & strPageId)
            '******************************************************************************
        End If
    End Sub

End Class


