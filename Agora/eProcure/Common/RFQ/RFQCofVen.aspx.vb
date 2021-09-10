Imports AgoraLegacy
Imports eProcure.Component
Imports System.Collections
Public Class RFQCofVen
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Rfq_num As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trConfirm As System.Web.UI.HtmlControls.HtmlTableRow
    Dim strType As String
    Dim Qut_num As String

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
        MyBase.Page_Load(sender, e)
        strType = Request.QueryString("strType")
        Qut_num = Request.QueryString("Qt_num")
        lblTitle.Text = "Quotation"

        Dim i As Integer

        Select Case strType
            Case "1"
                '' trExistHeader.Visible = False
                ' trExistItem.Visible = False
                trHeader.Visible = False
                trItem.Visible = False
                lbl_Rfq_num.Text = "Duplicate transaction number found. Please contact your Administrator to rectify the problem."
                'lnkBack.NavigateUrl = "VendorRFQList.aspx?rfq_no=" & Request(Trim("rfq_no")) & "&edit=2"
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "rfq_no=" & Request(Trim("rfq_no")) & "&edit=2&pageid=" & strPageId & " ")

            Case "2"
                Dim strItemExist() As String
                Dim strItemAdd() As String
                ' strItemExist = CType(Session("ItemExist"), String).Split(",")
                ' strItemAdd = CType(Session("ItemAdd"), String).Split(",")
                lbl_Rfq_num.Text = "Quotation : <b>" & Qut_num & "</b> has already been sent to selected Buyers."

                ' lblConfirm.Text = "Items already exist in Purchase Requisition Number : <b>" & Rfq_num & "</b>"

                'lnkBack.NavigateUrl = "RFQ_List.aspx?rfq_no=" & Rfq_num & "&edit=1"
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "rfq_no=" & Request(Trim("rfq_no")) & "&edit=2&pageid=" & strPageId & " ")


                'Case "3"
                '    'trExistHeader.Visible = False
                '    '  trExistItem.Visible = False
                '    trHeader.Visible = False
                '    trItem.Visible = False
                '    lbl_Rfq_num.Text = "The RFQ Number : <b>" & Rfq_num & "</b> has been successfully sent to the selected vendors. "
                '    'lnkBack.NavigateUrl = "RFQ_List.aspx?rfq_no=" & Request(Trim("rfq_no")) & "&edit=2"
                '    lnkBack.NavigateUrl = "RFQ_List.aspx?rfq_no=" & Request(Trim("rfq_no")) & "&edit=2&pageid=" & strPageId & " "
                '    '   Dim items As New DrillDownReportCollection

            Case "4"
                ' trExistHeader.Visible = False
                ' trExistItem.Visible = False
                trHeader.Visible = False
                trItem.Visible = False
                lbl_Rfq_num.Text = "Quotation : <b>" & Qut_num & "</b> has been successfully sent to the selected buyers. "
                'lnkBack.NavigateUrl = "VendorRFQList.aspx?rfq_no=" & Request(Trim("rfq_no")) & "&edit=2"
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "rfq_no=" & Request(Trim("rfq_no")) & "&edit=2&pageid=" & strPageId & " ")

            Case "5"
                ' trExistHeader.Visible = False
                ' trExistItem.Visible = False
                trHeader.Visible = False
                trItem.Visible = False
                lbl_Rfq_num.Text = "Quotation : <b>" & Qut_num & "</b> fail to update.Please contact your administrator."
                'lnkBack.NavigateUrl = "VendorRFQList.aspx?rfq_no=" & Request(Trim("rfq_no")) & "&edit=2"
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "rfq_no=" & Request(Trim("rfq_no")) & "&edit=2&pageid=" & strPageId & " ")
        End Select
        '//********carol add in this**********
        trHeader.Visible = True
        trItem.Visible = True
        '//
    End Sub


End Class
