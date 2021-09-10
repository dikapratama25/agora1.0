Imports AgoraLegacy
Imports eProcure.Component

Public Class PRList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents trHeader As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents lblItem As System.Web.UI.WebControls.Label
    'Protected WithEvents trItem As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents lblQuoNo As System.Web.UI.WebControls.Label

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

        If Not IsPostBack Then
            Dim strSCoyId, strDocNo, strType, strIndex As String
            strSCoyId = Request.QueryString("CoyId")
            strDocNo = Request.QueryString("DocNo")
            strType = Request.QueryString("type")
            strIndex = Request.QueryString("index")

            Select Case UCase(strType)
                Case "PR"
                    lblTitle.Text = "Purchase Requisition Listing"
                    lblQuoNo.Text = "Purchase Requisition raised from Quotation Number " & strDocNo
                    Dim objPR As New PR
                    Dim ds As New DataSet
                    Dim i As Integer
                    ds = objPR.getPRFromQuot(strDocNo, strSCoyId, Session("CompanyId"))
                    lblItem.Text = ""
                    If Not ds Is Nothing Then
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            lblItem.Text &= "<A href='" & dDispatcher.direct("PO", "PRDetail.aspx", "caller=OTHER&PageId=" & strPageId & "&index=" & ds.Tables(0).Rows(i)("PRM_PR_Index") & "&PRNo=" & ds.Tables(0).Rows(i)("PRM_PR_NO")) & "'>" & ds.Tables(0).Rows(i)("PRM_PR_NO") & "</A><BR>"
                        Next
                    End If
                    'lnkBack.NavigateUrl = "../RFQ/RFQ_List.aspx?pageid=" & strPageId

                Case "QUO"
                    Session("quoteurl") = strCallFrom
                    lblTitle.Text = "Quotation Listing"
                    lblQuoNo.Text = "Quotation converted to Purchase Order " & strDocNo
                    Dim objPO As New PurchaseOrder
                    Dim ds As New DataSet
                    Dim i As Integer
                    lblItem.Text = ""
                    If objPO.isConvertedFromRFQ(strIndex, ds) Then
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            lblItem.Text &= "<A href='" & dDispatcher.direct("RFQ", "viewQoute.aspx", "side=quote&PageId=" & strPageId & "&RFQ_Id=" & ds.Tables(0).Rows(i)("PRM_RFQ_INDEX") & "&vcomid=" & ds.Tables(0).Rows(i)("PRM_S_COY_ID")) & "'>" & ds.Tables(0).Rows(i)("RRM_Actual_Quot_Num") & "</A><BR>"
                        Next
                    End If
            End Select
            lnkBack.NavigateUrl = "javascript:history.back();"
        End If
    End Sub

End Class
