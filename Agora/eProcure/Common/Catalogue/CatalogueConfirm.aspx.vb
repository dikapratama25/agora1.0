Imports AgoraLegacy
Imports eProcure.Component

Public Class CatalogueConfirm
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher = New dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblPR As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDetail As System.Web.UI.WebControls.Label
    Protected WithEvents trHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDetails As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents trRemark As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNotHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblNotDetail As System.Web.UI.WebControls.Label
    Protected WithEvents trNotHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNotDetail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHeaderBlank As System.Web.UI.HtmlControls.HtmlTableRow

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
        Dim strCode As String
        Dim strRole As String
        Dim strAct, strMasterDeleted, strIndex As String
        strCode = Request.QueryString("code")
        strRole = Request.QueryString("role")

        trRemark.Visible = False
        trDetails.Visible = False
        trNotHeader.Visible = False
        trNotDetail.Visible = False
        trCode.Visible = False

        Select Case strRole
            Case "B" ' buyer side
                lblTitle.Text = "Contract Catalogue"
                strAct = Request.QueryString("act")
                Select Case strAct
                    Case "0" ' rejected
                        lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been rejected. "
                    Case "1" ' approved
                        lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been approved. "
                    Case "2" ' discarded
                        lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been discarded. "
                End Select
                'lnkBack.NavigateUrl = "ContractCatalogueApproval.aspx?pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogueApproval.aspx", "pageid=" & strPageId)

            Case "V"
                lblTitle.Text = "Contract Catalogue"
                lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been submitted for Approval From Buyer. "
                'lnkBack.NavigateUrl = "ContractCatalogueList.aspx?type=D&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogueList.aspx", "type=D&pageid=" & strPageId)

            Case "L", "D", "C"
                strAct = Request.QueryString("act")
                strMasterDeleted = Request.QueryString("MasterDeleted")
                lblTitle.Text = "List Price Catalogue"
                trCode.Visible = True
                Select Case strAct
                    Case "0" ' approval
                        lblPR.Text = "The List Price Item Code : <b>" & strCode & "</b> has been submitted for Approval From Hub Admin. "
                        'lnkBack.NavigateUrl = "ListPriceCatalogue.aspx?pageid=" & strPageId
                        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId)

                    Case "1" ' delete
                        trHeader.Attributes.Add("class", "tableheader")
                        trDetails.Attributes.Add("class", "tablecol")
                        trNotHeader.Attributes.Add("class", "tableheader")
                        trNotDetail.Attributes.Add("class", "tablecol")

                        Dim strItem, strDetail, strRemark As String
                        Dim strAryItem(), strAryNot() As String
                        Dim i As Integer = 0

                        ' item deleted successfully
                        If strRole = "L" Then
                            'lblTitle.Text = "List Price Catalogue"
                            lblTitle.Text = "Vendor Item"
                            'lnkBack.NavigateUrl = "ListPriceCatalogue.aspx?pageid=" & strPageId
                            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId)
                        ElseIf strRole = "D" Then
                            strIndex = Request.QueryString("index")
                            trCode.Visible = True
                            lblTitle.Text = "Discount Catalogue"
                            lblCode.Text &= "Discount Group Code : <b>" & strCode & "</b>"
                            If strMasterDeleted = "1" Then
                                'lnkBack.NavigateUrl = "DiscountCatalogueList.aspx?cattype=D&pageid=" & strPageId
                                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "DiscountCatalogueList.aspx", "cattype=D&pageid=" & strPageId)
                            Else
                                'lnkBack.NavigateUrl = "ContractCatalogue.aspx?mode=mod&type=DC&index=" & strIndex & "&pageid=" & strPageId
                                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "mode=mod&type=DC&index=" & strIndex & "&pageid=" & strPageId)
                            End If

                        End If

                        lblPR.Text &= "Item(s) deleted successfully : "
                        strItem = Session("ItemDeleted")
                        strAryItem = strItem.Split(",")
                        If strAryItem.Length > 1 Then
                            trDetails.Visible = True
                            strDetail = "<ul type='disc'>"
                            For i = 0 To strAryItem.Length - 2
                                strDetail &= "<li>" & strAryItem(i) & ".<ul type='disc'></ul></li>"
                            Next
                            strDetail &= "</ul>"
                            lblDetail.Text = strDetail
                        Else
                            trHeader.Visible = False
                            trDetails.Visible = False
                            trHeaderBlank.Visible = False
                        End If

                        ' item not deleted successfully
                        strItem = Session("ItemNotDeleted")
                        strAryNot = strItem.Split(",")
                        If strAryNot.Length > 1 Then
                            trNotHeader.Visible = True
                            trNotDetail.Visible = True
                            trRemark.Visible = True
                            lblNotHeader.Text = "Item(s) not deleted successfully : "
                            strDetail = "<ul type='disc'>"
                            For i = 0 To strAryNot.Length - 2
                                strDetail &= "<li>" & strAryNot(i) & ".<ul type='disc'></ul></li>"
                            Next
                            strDetail &= "</ul>"
                            lblNotDetail.Text = strDetail
                            lblNotDetail.Attributes.Add("class", "errormsg")
                            strRemark = "Items cannot be deleted as :"
                            strRemark &= "<ul type='disc'>"
                            If strRole = "L" Then
                                strRemark &= "<li>it has outstanding PR(s)/PO(s).<ul type='disc'></ul></li>"
                                'strRemark &= "<li>it is under contract/discount period.<ul type='disc'></ul></li>"
                            Else
                                strRemark &= "<li>it has outstanding PR(s).<ul type='disc'></ul></li>"
                                'strRemark &= "<li>it is under contract/discount period.<ul type='disc'></ul></li>"
                            End If
                            strRemark &= "</ul>"
                            lblRemark.Text = strRemark
                        End If
                End Select

        End Select
    End Sub
End Class
