Imports AgoraLegacy
Imports eProcure.Component
Public Class DisplayAddedItem
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblOK As System.Web.UI.WebControls.Label
    Protected WithEvents lblDup As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblOKHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblDupHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lnkHere As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHere As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoPriceHdr As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoPrice As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        Dim dtProduct As DataTable
        Dim drData As DataRow
        Dim strTemp, strTemp1, strTemp2, strType As String
        dtProduct = CType(Session("dt"), DataTable)
        strType = Request.QueryString("type")
        lnkBack.NavigateUrl = dDispatcher.direct("Product", "SearchCatalogue.aspx", "pageId=" & strPageId)
        Select Case strType
            Case "SC"
                'has been successfully added into your shopping cart
                'is currently inside your shopping cart. 
                lblHeader.Text = "Add Items to Shopping Cart"
                lblOKHeader.Text = "Items successfully added to Shopping Cart"
                lblDupHeader.Text = "Items already exist in Shopping Cart"
                lblNoPriceHdr.Text = "Only those items with item price can be added into the Shopping Cart"
                lblHere.Text = "to view your Shopping Cart."
                lnkHere.NavigateUrl = dDispatcher.direct("PR", "viewShoppingCart.aspx", "pageId=" & strPageId)
                If Request.QueryString("type1") = "BF" Then  'From Buyer cat
                    lnkBack.NavigateUrl = "javascript:history.back();"
                End If
            Case "B"
                lblHeader.Text = "Add Items to Buyer Catalogue"
                lblOKHeader.Text = "Items successfully added to Buyer Catalogue"
                lblDupHeader.Text = "Items already exist in Buyer Catalogue"
                lblNoPriceHdr.Text = "Only those items with item price can be added into the Buyer Catalogue"
                lblHere.Text = "to view your Buyer Catalogue."
                lnkHere.NavigateUrl = dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "index=" & Request.QueryString("id") & "&code=" & Request.QueryString("code") & "&name=" & Request.QueryString("name") & "&pageId=57")
                If Request.QueryString("type1") = "BF" Then
                    lnkBack.NavigateUrl = "javascript:history.back();"
                End If
            Case "F"
                lblHeader.Text = "Add Items to Favourite List"
                lblOKHeader.Text = "Items successfully added to Favourite List"
                lblDupHeader.Text = "Items already exist in Favourite List"
                lblNoPriceHdr.Text = "Only those items with item price can be added into the Favourite List"
                lblHere.Text = "to view your Favourite List."
                lnkHere.NavigateUrl = dDispatcher.direct("PersonalSetting", "Favs_BuyerList.aspx", "type=" & Request.QueryString("type") & "&id=" & Request.QueryString("id") & "&pageId=3")
        End Select
        strTemp = "<UL>"
        strTemp1 = "<UL>"
        strTemp2 = "<UL>"
        For Each drData In dtProduct.Rows
            If drData("msg") = "1" Then 'ok
                strTemp = strTemp & "<LI>Item """ & drData("PRODUCT_CODE") & """"
            ElseIf drData("msg") = "0" Then 'dup
                strTemp1 = strTemp1 & "<LI>Item """ & drData("PRODUCT_CODE") & """<BR>"
            ElseIf drData("msg") = "2" Then 'no price
                strTemp2 = strTemp2 & "<LI>Item """ & drData("PRODUCT_CODE") & """<BR>"
            End If
        Next
        lblOK.Text = strTemp & "</UL>"
        lblDup.Text = strTemp1 & "</UL>"
        lblNoPrice.Text = strTemp2 & "</UL>"
        lblDup.CssClass = "errormsg"

        If strType = "B" And Request.QueryString("type1") = "BF" Then
            lblOK.Text = "-"
        End If
        Session("dt") = Nothing

        lnkHere.NavigateUrl = dDispatcher.direct("PR", "test.aspx")
        lnkBack.NavigateUrl = dDispatcher.direct("PR", "test.aspx")
    End Sub

    

End Class
