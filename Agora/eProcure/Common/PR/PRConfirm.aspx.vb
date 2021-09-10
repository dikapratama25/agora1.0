Imports AgoraLegacy
Imports eProcure.Component
Public Class PRConfirm
    Inherits AgoraLegacy.AppBaseClass

    Dim strType As String
    Dim PRid As String
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblPR As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblItemExist As System.Web.UI.WebControls.Label
    Protected WithEvents lblExist As System.Web.UI.WebControls.Label
    Protected WithEvents trExistHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trExistItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblConfirm As System.Web.UI.WebControls.Label
    Protected WithEvents trConfirm As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblItem As System.Web.UI.WebControls.Label

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


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
        ' S - Save without Approval; 
        ' I - Add Item to PR
        ' A - Approval
        Dim strMsg As String
        strType = Request.QueryString("type")
        PRid = Request.QueryString("prid")
        strMsg = Request.QueryString("msg")
        lblTitle.Text = "Purchase Requisition"


        'Dim dsItem As New DataSet
        Dim i As Integer
        'dsItem = objPR.getRPVendorItemList(PRid)

        lblItem.Text = ""
        Select Case strType
            Case "S"
                trExistHeader.Visible = False
                trExistItem.Visible = False
                Dim objPR As New PR
                Dim dsItem As New DataSet
                dsItem = objPR.getRPVendorItemList(PRid)
                lblPR.Text = "Purchase Requisition Number : <b>" & PRid & "</b>"
                lblItem.Text = "<ul type='disc'>"
                For i = 0 To dsItem.Tables(0).Rows.Count - 1
                    lblItem.Text = lblItem.Text & "<li>Purchase Requisition has been <B>raised</B> for item <B>" & dsItem.Tables(0).Rows(i)(0) & "</B><ul type='disc'></ul></li>"
                Next
                lblItem.Text = lblItem.Text & "</ul>"
                lnkBack.NavigateUrl = "../PO/SearchPR_all.aspx?caller=buyer&pageid=7"

            Case "I"
                Dim strItemExist() As String
                Dim strItemAdd() As String
                strItemExist = CType(Session("ItemExist"), String).Split(",")
                strItemAdd = CType(Session("ItemAdd"), String).Split(",")
                lblPR.Text = "Purchase Requisition Number : <b>" & PRid & "</b>"
                lblItem.Text = "<ul type='disc'>"
                If strItemAdd.Length > 1 Then
                    For i = 0 To strItemAdd.Length - 2
                        lblItem.Text &= "<li>Item <B>" & strItemAdd(i) & "</B> has been added.<ul type='disc'></ul></li>"
                    Next
                Else
                    trHeader.Visible = False
                    trItem.Visible = False
                    trConfirm.Visible = False
                End If
                lblItem.Text &= "</ul>"

                lblExist.Text = "Items already exist in Purchase Requisition Number : <b>" & PRid & "</b>"
                lblItemExist.Text = "<ul type='disc'>"
                If strItemExist.Length > 1 Then
                    For i = 0 To strItemExist.Length - 2
                        lblItemExist.Text &= "<li>Item <B>" & strItemExist(i) & "</B> already exists.<ul type='disc'></ul></li>"
                    Next
                Else
                    trExistHeader.Visible = False
                    trExistItem.Visible = False
                End If
                lblItemExist.Text &= "</ul>"

                lnkBack.NavigateUrl = "viewShoppingCart.aspx?type=tab&pageid=4"

            Case "A"
                trExistHeader.Visible = False
                trExistItem.Visible = False
                trHeader.Visible = False
                trItem.Visible = False
                Dim strConfirm As String
                ' strMsg : 0 - No message
                '        : 1 - PR value more than operating budget amount
                '        : 2 - PR already submitted (use 2 different browser)
                '        : 3 - PR already deleted
                Select Case strMsg
                    Case "0"
                        strConfirm = "The PR Number : <b>" & PRid & "</b> has been submitted for Approval. "
                    Case "1"
                        strConfirm = "The PR Number : <b>" & PRid & "</b> has been submitted for Approval. "
                        strConfirm &= "<BR><font color='red'><B>PR value is more than the Operating Budget Amount.</B></font>"
                    Case "2"
                        strConfirm = "The PR Number : <b>" & PRid & "</b> has already been submitted. "
                    Case "3"
                        strConfirm = "The PR Number : <b>" & PRid & "</b> has already been deleted. "
                End Select
                If strMsg = "1" Then

                End If
                lblConfirm.Text = strConfirm
                lnkBack.NavigateUrl = "../PO/SearchPR_all.aspx?caller=buyer&pageid=7"
        End Select
    End Sub
End Class
