Imports AgoraLegacy

Public Class HubCatalogueConfirm
    Inherits AgoraLegacy.AppBaseClass

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
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        Dim strAct, strType, strMasterDelete, strIndex As String
        strCode = Request.QueryString("code")
        strAct = Request.QueryString("act")
        trRemark.Visible = False
        trDetails.Visible = False
        trCode.Visible = False
        trNotHeader.Visible = False
        trNotDetail.Visible = False
        Select Case strAct
            Case "0" ' rejected
                lblTitle.Text = "Contract Catalogue"
                lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been rejected. "
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueApproval.aspx", "pageid=" & strPageId)
            Case "1" ' approved
                lblTitle.Text = "Contract Catalogue"
                lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been approved. "
                'lnkBack.NavigateUrl = "HubCatalogueApproval.aspx?pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueApproval.aspx", "pageid=" & strPageId)
            Case "2" ' discarded
                lblTitle.Text = "Contract Catalogue"
                lblPR.Text = "The Contract Ref. No : <b>" & strCode & "</b> has been discarded. "
                'lnkBack.NavigateUrl = "HubCatalogueApproval.aspx?pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueApproval.aspx", "pageid=" & strPageId)
            Case "3" ' list price approved
                lblTitle.Text = "List Price Catalogue"
                lblPR.Text = "The List Price Item Id : <b>" & strCode & "</b> has been approved. "
                'lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=A&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=A&pageid=" & strPageId)
            Case "4" ' list price rejected
                lblTitle.Text = "List Price Catalogue"
                lblPR.Text = "The List Price Item Id : <b>" & strCode & "</b> has been rejected. "
                'lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=A&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=A&pageid=" & strPageId)
            Case "5" ' list price published (list price created/modified by hub admin)
                lblTitle.Text = "List Price Catalogue"
                lblPR.Text = "The List Price Item Id : <b>" & strCode & "</b> has been published. "
                lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=O&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=O&pageid=" & strPageId)
            Case "6" ' displays not deleted list price item
                strType = Request.QueryString("type")
                strMasterDelete = Request.QueryString("MasterDelete")
                strIndex = Request.QueryString("index")
                trDetails.Visible = True
                trRemark.Visible = True
                trCode.Visible = True
                trHeader.Attributes.Add("class", "tableheader")
                trDetails.Attributes.Add("class", "tablecol")
                trNotHeader.Attributes.Add("class", "tableheader")
                trNotDetail.Attributes.Add("class", "tablecol")
                Select Case strType
                    Case "L"
                        lblTitle.Text = "List Price Catalogue"
                        'lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=O&pageid=" & strPageId
                        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=O&pageid=" & strPageId)
                    Case "D"
                        lblTitle.Text = "Discount Catalogue"
                        If strMasterDelete = "1" Then
                            'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=D&pageid=" & strPageId
                            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=D&pageid=" & strPageId)
                        Else
                            'lnkBack.NavigateUrl = "HubCatalogueDetail.aspx?cattype=D&mode=mod&index=" & strIndex & "&pageid=" & strPageId
                            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=D&mode=mod&index=" & strIndex & "&pageid=" & strPageId)
                        End If
                    Case "C"
                        lblTitle.Text = "Contract Catalogue"
                        If strMasterDelete = "1" Then
                            'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=C&pageid=" & strPageId
                            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=C&pageid=" & strPageId)
                        Else
                            'lnkBack.NavigateUrl = "HubCatalogueDetail.aspx?cattype=C&mode=mod&index=" & strIndex & "&pageid=" & strPageId
                            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=C&mode=mod&index=" & strIndex & "&pageid=" & strPageId)
                        End If
                End Select

                Dim strItem, strDetail, strRemark As String
                Dim strAryItem(), strAryNot() As String
                Dim i As Integer = 0

                strItem = Session("ItemDeleted")
                strAryItem = strItem.Split(",")
                If strAryItem.Length > 1 Then
                    lblPR.Text &= "Item(s) deleted successfully : "
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
                    If strType = "L" Then
                        strRemark &= "<li>it has outstanding PR(s)/PO(s).<ul type='disc'></ul></li>"
                        strRemark &= "<li>it is under contract/discount period.<ul type='disc'></ul></li>"
                    Else
                        strRemark &= "<li>it has outstanding PR(s).<ul type='disc'></ul></li>"
                    End If
                    strRemark &= "</ul>"
                    lblRemark.Text = strRemark
                End If

                'lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=O&pageid=" & strPageId
        End Select

    End Sub
End Class
