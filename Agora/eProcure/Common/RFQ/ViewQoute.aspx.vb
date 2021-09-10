Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ViewQoute
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Dim objrfq As New RFQ
    Dim objval As New RFQ_User    
    Protected WithEvents lbl_Currency As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_CreateDate As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_QuoteVal As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_From As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_PhyAdds As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ContactPer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ContNum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Email As System.Web.UI.WebControls.Label
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents dg_viewitem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_quoteNum As System.Web.UI.WebControls.Label
    Protected WithEvents pnlAttach2 As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdView As System.Web.UI.HtmlControls.HtmlInputButton 'System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents tr_curr As System.Web.UI.HtmlControls.HtmlTableRow

    Dim vcomid As String
    Dim strFrm As String
    Dim Court As Integer = 0
    Dim objFile As New FileManagement
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum ItemEnum
        ProName = 0
        ProDesc = 1
        UOM = 2
        QTY = 3
        Tolerance = 4
        UnitPrice = 5
        Price = 6
        GstRate = 7
        GstAmt = 8
        Tax = 9
        DelTerm = 10
        PackQty = 11
        Min = 12
        Time = 13
        Warranty = 14
        Remarks = 15
    End Enum
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' cmd_pre2 = false rfq buyer side 
        ' cmd_pre=true  history.back can use in both side 
        blnSorting = False
        Me.blnPaging = False
        Me.blnFooter = False
        MyBase.Page_Load(sender, e)
        SetGridProperty(dg_viewitem)
        strFrm = Me.Request.QueryString("Frm")
        'If Not Page.IsPostBack Then
        Dim state As String
        Dim country As String
        Dim vcomid3 As String = Request(Trim("vcomid"))

        Dim objDb As New EAD.DBCom
        Dim objGst As New GST
        'Check for Delivery Term setup
        If objDb.Exist("SELECT '*' FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID = '" & Me.Request(Trim("RFQ_ID")) & "' AND RRD_V_Coy_ID='" & vcomid3 & "' AND (RRD_DEL_CODE <> '' AND RRD_DEL_CODE IS NOT NULL)") Then
            Session("blnTrue") = True
            tr_curr.Style("display") = ""
        Else
            Session("blnTrue") = False
            tr_curr.Style("display") = "none"
        End If

        Dim objval As New RFQ_User
        objrfq.get_qoute1(objval, Me.Request(Trim("RFQ_ID")), vcomid3)
        ViewState("BCoyID") = objval.bcom_id
        ViewState("RFQNo") = objval.RFQ_Num
        ViewState("QuoNo") = objval.quo_num
        ViewState("VCoyID") = objval.V_com_ID
        Me.lbl_Currency.Text = objval.cur_code
        Me.lbl_ContactPer.Text = objval.con_person
        Me.lbl_quoteNum.Text = objval.quo_num
        Me.lbl_ContNum.Text = objval.phone
        Me.lbl_CreateDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
        Me.lbl_Email.Text = objval.email
        Me.lbl_From.Text = objval.V_Com_Name
        If objGst.chkGSTCOD(Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)) = True Or objGst.chkGSTByRate("QUO", Me.Request(Trim("RFQ_ID")), ViewState("VCoyID")) = True Then
            ViewState("GstQuo") = True
        Else
            ViewState("GstQuo") = False
        End If
        state = objrfq.get_codemstr(objval.state, "S")
        country = objrfq.get_codemstr(objval.country, "CT")

        Dim stradds As String

        If objval.addsline1 <> "" Then
            stradds = objval.addsline1
        End If

        If objval.addsline2 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline2 & ""
            Else
                stradds = stradds & "<br> " & "&nbsp;" & objval.addsline2 & ""
            End If
        End If

        If objval.addsline3 <> "" Then
            If stradds = "" Then
                stradds = " " & objval.addsline3 & ""
            Else
                stradds = stradds & "<br> " & "&nbsp;" & objval.addsline3 & ""
            End If
        End If

        If objval.postcode <> "" Or objval.city <> "" Then
            If stradds = "" Then
                stradds = "" & objval.postcode & " " & objval.city
            Else
                stradds = stradds & "<br>" & "&nbsp;" & objval.postcode & " " & objval.city
            End If

        End If

        If state <> "" Then
            If stradds = "" Then
                stradds = "" & state & ""
            Else
                stradds = stradds & "<br>" & "&nbsp;" & state & ""
            End If
        End If

        If country <> "" Then
            If stradds = "" Then
                stradds = "" & country & ""
            Else
                stradds = stradds & ", " & country & ""
            End If

        End If

        Me.lbl_PhyAdds.Text = stradds
        Me.lbl_QuoteVal.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.qoute_valDate)
        Me.txt_remark.Text = objval.remark
        ViewState("vcomid") = objval.V_com_ID
        Bindgrid()
        AddRow(4, "Sub Total", True)
        If ViewState("GstQuo") = True Then
            AddRow(4, "SST Amount", True)
        Else
            AddRow(4, "Tax", True)
        End If
        AddRow(4, "Grand Total", False)
        displayAttachFile2()

        'If strFrm = "RFQComSummary" Then
        '    Session("disable") = "N"
        'End If

        If strFrm = "RFQSearch" Or strFrm = "RFQ_Quote" Then
            GenerateTab()
        End If
        'End If
        intPageRecordCnt = ViewState("intPageRecordCnt")

        strFrm = Me.Request.QueryString("Frm")
        If strFrm = "RFQComSummary" Then
            If Me.Request.QueryString("SubFrm") = "SearchPO_AO" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            ElseIf Me.Request.QueryString("SubFrm") = "Dashboard" Then
                If Request.QueryString("Appr") = "Y" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=dashboard&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                Else
                    lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=dashboard&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                End If
            ElseIf Me.Request.QueryString("SubFrm") = "POViewB2" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            ElseIf Me.Request.QueryString("SubFrm") = "POViewTrx" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            ElseIf Me.Request.QueryString("SubFrm") = "POViewB2Cancel" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            ElseIf Me.Request.QueryString("SubFrm") = "RFQ_List" Then
                If Request.QueryString("Appr") = "Y" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                Else
                    lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                End If
            Else
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & "&side=" & Me.Request.QueryString("Subside") & "&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            End If

        ElseIf strFrm = "RFQ_Quote" Then
            lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId & "&side=rfqsum&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&qou_num=" & Me.Request(Trim("qou_num")))

        ElseIf strFrm = "RFQSearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&qou_num=" & Me.Request(Trim("qou_num")))

        ElseIf strFrm = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))

        ElseIf strFrm = "POViewB" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))

        ElseIf strFrm = "POVendorList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))

        ElseIf strFrm = "POViewB2" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))

        ElseIf strFrm = "POViewTrx" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewTrx.aspx", "filetype=2&side=u&pageid=7&type=MyPO")

        ElseIf strFrm = "POViewB2Cancel" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2Cancel.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))
        ElseIf strFrm = "transtracking" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "TransTracking.aspx", "coytype=" & Request.QueryString("coytype") & "&pageid=" & Request.QueryString("pageid"))

        ElseIf strFrm = "SearchPO_AO" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))

        ElseIf strFrm = "SearchPO_All" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPO_All.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")))


            'Michelle (30/9/2010) - To link back to Dashboard if calling page is from Dashboard
            'ElseIf strFrm = "Dashboard" Then
            '    lnkBack.NavigateUrl = "AddGRN1.aspx?Frm=Dashboard&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode")

            'ElseIf strFrm = "AddGRN" Then
            '    lnkBack.NavigateUrl = "AddGRN.aspx?Frm=GRNDetails&pageid=" & strPageId

            'ElseIf strFrm = "InvoiceTrackingList" Then
            '    lnkBack.NavigateUrl = "../Tracking/InvoiceTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId

            'ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
            '    lnkBack.NavigateUrl = "../Tracking/InvoiceVerifiedTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId

            'ElseIf strFrm = "InvoicePaidTrackingList" Then
            '    lnkBack.NavigateUrl = "../Tracking/InvoicePaidTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId

            'ElseIf strFrm = "PODetail" Then
            '    If Me.Request.QueryString("SubFrm") = "GRNSearch" Then
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=GRNSearch&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"

            '    ElseIf Me.Request.QueryString("SubFrm") = "InvList" Then
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=otherv&filetype=2"

            '    ElseIf Me.Request.QueryString("SubFrm") = "InvoiceTrackingList" Then
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvoiceTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"

            '    ElseIf Me.Request.QueryString("SubFrm") = "InvoiceVerifiedTrackingList" Then
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvoiceVerifiedTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"

            '    ElseIf Me.Request.QueryString("SubFrm") = "InvoicePaidTrackingList" Then
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvoicePaidTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"

            '    ElseIf Me.Request.QueryString("SubFrm") = "POVIEWB2" Then
            '        'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1"
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&Caller=POviewB2&side=b&filetype=2" & "&poview=1"

            '    ElseIf Me.Request.QueryString("SubFrm") = "POVendorList" Then
            '        'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1"
            '        lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POVendorList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=v&filetype=2"

            '    End If

            'ElseIf strFrm = "AddGRN1" Then
            '    lnkBack.NavigateUrl = "AddGRN1.aspx?Frm=GRNDetails&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode")
        End If

        Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewQuotation.aspx", "SCoyID=" & ViewState("VCoyID") & "&BCoyID=" & ViewState("BCoyID") & "&quo_no=" & ViewState("QuoNo") & "&rfqid=" & Me.Request(Trim("RFQ_ID")) & "&rfq_no=" & ViewState("RFQNo")) & "')")
    End Sub

    '  Private Sub cmd_pre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
    '      Dim strurl As String
    '      If Request(Trim("side")) = "quote" Then
    '          strurl = Session("quoteurl")
    '      Else
    '          strurl = Session("strurl")
    '      End If
    '      If strFrm = "RFQSearch" Then
    '          strurl = "RFQSearch.aspx?&pageid=" & strPageId
    '      ElseIf strFrm = "RFQ_Quote" Then
    '          strurl = "RFQ_Quote.aspx?&pageid=" & strPageId
    '      End If
    ''Session("strurl") = ""
    '      Response.Redirect(strurl)
    '  End Sub

    Private Sub displayAttachFile2()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objRFQ.getRFQ_quoteTempAttach(objRFQ.rfqnum(Request(Trim("RFQ_ID"))), ViewState("vcomid"))

        pnlAttach2.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "Quotation", EnumUploadFrom.FrontOff)

                Dim lblBr As New Label
                Dim lblFile As New Label

                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                pnlAttach2.Controls.Add(lblFile)
                pnlAttach2.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach2.Controls.Add(lblFile)
        End If

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal check As Boolean)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim dblTotal As Double
        Dim dblTax As Double
        Dim dblGTotal As Double

        'row.Cells(0).ColumnSpan = intCell - 1
        For intL = 0 To 15
            addCell(row)
        Next

        If strLabel = "Sub Total" Then
            Dim txtTotal As New TextBox
            txtTotal.ID = strLabel
            dblTotal = CDbl(ViewState("Sub Total"))
            txtTotal.Text = Format(ViewState(strLabel), "#,##0.00")
            txtTotal.CssClass = "lblnumerictxtbox"
            txtTotal.ReadOnly = True
            txtTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
            txtTotal.Font.Bold = True
            row.Cells(6).Controls.Add(txtTotal)
            row.Cells(5).Text = strLabel
            row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            row.Cells(5).Font.Bold = True

        ElseIf strLabel = "Tax" Or strLabel = "SST Amount" Then
            If check = True Then
                Dim txtTax As New TextBox
                dblTax = CDbl(ViewState("tax"))
                txtTax.ID = "Tax"
                txtTax.CssClass = "lblnumerictxtbox"
                txtTax.ReadOnly = True
                txtTax.Width = System.Web.UI.WebControls.Unit.Pixel(120)
                txtTax.Font.Bold = True
                row.Cells(6).HorizontalAlign = HorizontalAlign.Right
                txtTax.Text = Format(ViewState("tax"), "#,##0.00")
                row.Cells(5).Text = strLabel
                row.Cells(5).Font.Bold = True
                row.Cells(6).Controls.Add(txtTax)
            End If

        ElseIf strLabel = "Grand Total" Then
            dblGTotal = CDbl(ViewState("Sub Total")) + CDbl(ViewState("tax"))
            Dim txtGrandTotal As New TextBox
            txtGrandTotal.ID = strLabel
            txtGrandTotal.Text = Format(dblGTotal, "#,##0.00")
            txtGrandTotal.CssClass = "lblnumerictxtbox"
            txtGrandTotal.ReadOnly = True
            txtGrandTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
            txtGrandTotal.Font.Bold = True
            row.Cells(6).Controls.Add(txtGrandTotal)
            row.Cells(5).Text = strLabel
            row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            row.Cells(5).Font.Bold = True
        End If

        row.BackColor = Color.FromName("#f4f4f4")
        Me.dg_viewitem.Controls(0).Controls.Add(row)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objview As New RFQ
        Dim ds As New DataSet
        Dim RFQ_ID As String
        Dim vcomid3 As String = Request(Trim("vcomid"))
        RFQ_ID = Request(Trim("RFQ_ID"))
        ds = objview.get_quotation2(RFQ_ID, vcomid3)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dg_viewitem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_viewitem.PageSize = 0 Then
                dg_viewitem.CurrentPageIndex = dg_viewitem.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dg_viewitem.DataSource = dvViewSample
        dg_viewitem.DataBind()

        If Session("blnTrue") = True Then
            Me.dg_viewitem.Columns(ItemEnum.DelTerm).Visible = True
        Else
            Me.dg_viewitem.Columns(ItemEnum.DelTerm).Visible = False
        End If

        If ViewState("GstQuo") = True Then
            Me.dg_viewitem.Columns(ItemEnum.GstRate).Visible = True
            Me.dg_viewitem.Columns(ItemEnum.GstAmt).Visible = True
            Me.dg_viewitem.Columns(ItemEnum.Tax).Visible = False
        Else
            Me.dg_viewitem.Columns(ItemEnum.GstRate).Visible = False
            Me.dg_viewitem.Columns(ItemEnum.GstAmt).Visible = False
            Me.dg_viewitem.Columns(ItemEnum.Tax).Visible = True
        End If

        If Session("Env") = "FTN" Then
            Me.dg_viewitem.Columns(ItemEnum.Warranty).Visible = False
        Else
            Me.dg_viewitem.Columns(ItemEnum.Warranty).Visible = True
        End If
    End Function

    Private Sub dg_viewitem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemCreated
        Grid_ItemCreated(dg_viewitem, e)

    End Sub

    Private Sub dg_viewitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box

            Dim lbl_moq As Label
            lbl_moq = e.Item.FindControl("lbl_moq")
            lbl_moq.Text = Common.parseNull(dv("RRD_Min_Order_Qty"))

            Dim lbl_mpq As Label
            lbl_mpq = e.Item.FindControl("lbl_mpq")
            lbl_mpq.Text = Common.parseNull(dv("RRD_Min_Pack_Qty"))

            Dim lbl_Delivery As Label
            lbl_Delivery = e.Item.FindControl("lbl_Delivery")
            If dv("RRD_Delivery_Lead_Time") = "0" Then
                lbl_Delivery.Text = "Ex-Stock"
            Else
                lbl_Delivery.Text = Common.parseNull(dv("RRD_Delivery_Lead_Time"))
            End If

            Dim lbl_warranty As Label
            lbl_warranty = e.Item.FindControl("lbl_warranty")

            Dim lbl_unit_price As Label
            lbl_unit_price = e.Item.FindControl("lbl_unit_price")
            Dim lbl_price As Label
            lbl_price = e.Item.FindControl("lbl_price")

            Court = Court + 1
            Dim strJQPrice As String = "jqPrice" & Court
            Dim a As Decimal = 0
            If IsDBNull(dv("RRD_Unit_Price")) Then
                lbl_price.Text = "No Quote"
                lbl_unit_price.Text = "No Quote"
            Else
                '2015-06-18: CH: Rounding issue (Prod issue)
                'a = dv("RRD_Unit_Price") * dv("RRD_Quantity")
                a = CDec(Format(dv("RRD_Unit_Price") * dv("RRD_Quantity"), "###0.00"))

                If a = 0 Then
                    lbl_price.Text = Format(0, "###,###,##0.00")
                    lbl_unit_price.Text = Format(0, "###,###,##0.00")
                Else
                    If Session("blnTrue") = True Then
                        lbl_price.Text = Format(a, "###,###,##0.00")
                        If DisplayQuoPricePopup(strJQPrice, dv("RRD_RFQ_ID"), dv("RRD_LINE_NO"), dv("RRD_V_COY_ID")) Then
                            lbl_unit_price.Text = Format(dv("RRD_Unit_Price"), "###,###,##0.0000") & "<span style=""cursor:default;"" class=""" & strJQPrice & """><IMG src=""" & dDispatcher.direct("Plugins/images", "d_icon.gif") & """></span>"
                        Else
                            lbl_unit_price.Text = Format(dv("RRD_Unit_Price"), "###,###,##0.0000")
                        End If
                    Else
                        lbl_price.Text = Format(a, "###,###,##0.00")
                        lbl_unit_price.Text = Format(dv("RRD_Unit_Price"), "###,###,##0.0000")
                    End If

                End If
            End If

            Dim lbl_gst_rate As Label
            lbl_gst_rate = e.Item.FindControl("lbl_gst_rate")
            lbl_gst_rate.Text = Common.parseNull(dv("GSTRATE"))

            Dim lbl_tax As Label
            Dim lbl_gst_amt As Label
            lbl_tax = e.Item.FindControl("lbl_tax")
            lbl_gst_amt = e.Item.FindControl("lbl_gst_amt")
            Dim i As Integer '= dv("RRD_GST") ' objrfq.get_gst(dv("RRD_GST_Code"))
            Dim j As Decimal

            If IsDBNull(dv("RRD_Unit_Price")) Then
                j = 0
            Else
                If ViewState("GstQuo") = True Then
                    If Common.parseNull(dv("TAX_PERC")) = "N/A" Or Common.parseNull(dv("TAX_PERC")) = "" Then
                        j = 0
                    Else
                        '2015-06-18: CH: Rounding issue (Prod issue)
                        'j = dv("RRD_Unit_Price") * dv("RRD_Quantity") * CInt(dv("TAX_PERC")) / 100
                        j = CDec(Format(a * CInt(dv("TAX_PERC")) / 100, "###0.00"))
                    End If
                Else
                    i = Common.parseNull(dv("RRD_GST"), 0)
                    '2015-06-18: CH: Rounding issue (Prod issue)
                    'j = dv("RRD_Unit_Price") * dv("RRD_Quantity") * i / 100
                    j = CDec(Format(a * i / 100, "###0.00"))
                End If
            End If

            lbl_tax.Text = Format(j, "###,###,##0.00")
            lbl_gst_amt.Text = Format(j, "###,###,##0.00")
            e.Item.Cells(ItemEnum.Tolerance).HorizontalAlign = HorizontalAlign.Right
            e.Item.Cells(ItemEnum.Price).HorizontalAlign = HorizontalAlign.Right
            ViewState("tax") = ViewState("tax") + j
            ViewState("Sub Total") = ViewState("Sub Total") + a
            ViewState("Total(W/Tax)") = ViewState("Total(W/Tax)") + a + j

            If dv("RRD_Warranty_Terms") = "0" Then
                lbl_warranty.Text = "0"
            Else
                lbl_warranty.Text = Common.parseNull(dv("RRD_Warranty_Terms"))
            End If

            Dim strDelName As String
            Dim objDb2 As New EAD.DBCom
            If IsDBNull(dv("RRD_DEL_CODE")) Then
                e.Item.Cells(ItemEnum.DelTerm).Text = ""
            Else
                strDelName = objDb2.GetVal("SELECT IFNULL(CDT_DEL_NAME,'') FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & ViewState("BCoyID") & "' AND CDT_DEL_CODE = '" & Common.Parse(dv("RRD_DEL_CODE")) & "'")

                If strDelName <> "" Then
                    e.Item.Cells(ItemEnum.DelTerm).Text = Common.parseNull(dv("RRD_DEL_CODE")) & " (" & strDelName & ")"
                Else
                    e.Item.Cells(ItemEnum.DelTerm).Text = Common.parseNull(dv("RRD_DEL_CODE"))
                End If

            End If

        End If
    End Sub

    Function DisplayQuoPricePopup(ByVal strJQ As String, ByVal RFQId As String, ByVal intRFQLine As Integer, ByVal VCompId As String) As Boolean
        Dim ds As New DataSet
        Dim i As Integer
        Dim aryTemp As New ArrayList()
        Dim decQty, decPrice As Decimal

        strJQ = Replace(strJQ, "jq", "")
        ds = objrfq.GetUnitPriceQuotation(RFQId, intRFQLine, VCompId)
        aryTemp.Clear()

        If ds.Tables(0).Rows.Count > 0 Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                decQty = ds.Tables(0).Rows(i).Item("RRVP_VOLUME")
                decPrice = ds.Tables(0).Rows(i).Item("RRVP_VOLUME_PRICE")

                aryTemp.Add(New String() {decQty, decPrice})
            Next

            ContructRow(strJQ, aryTemp)
        Else
            Return False
        End If

        DisplayQuoPricePopup = True

    End Function

    Private Function ContructRow(ByVal strTemp As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer

        For i = 0 To aryVolume.Count - 1
            'If aryVolume(i)(3) = strVCompLine Then
            strrow &= " Volume " & aryVolume(i)(0) & " : " & aryVolume(i)(1) & "<BR>"
            'End If
        Next

        strtable = strrow

        Session("jqPopup") = Session("jqPopup") & "$('.jq" & strTemp & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function

    '//Here code
    'Private Sub cmdquoReport_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim val As String = Me.Request(Trim("RFQ_ID"))
    '    Dim val1 As String = Trim(val)
    '    Dim idval As String = Request(Trim("vcomid"))
    '    Response.Redirect("QuoReport.aspx?pageid=" & strPageId & "&RFQ_ID=" & val1 & "&vcomid=" & idval & "")
    'End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim val As String = Me.Request(Trim("RFQ_ID"))
    '    Dim val1 As String = Trim(val)
    '    Dim idval As String = Request(Trim("vcomid"))
    '    Response.Redirect("QuoReport.aspx?pageid=" & strPageId & " &RFQ_ID=" & val1 & "&vcomid=" & idval & "")
    'End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'If strFrm = "RFQ_Quote" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"
        'Else
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                               "<li><a class=""t_entity_btn"" href=""VendorRFQList.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                               "<li><a class=""t_entity_btn_selected"" href=""RFQSearch.aspx?pageid=" & strPageId & """><span>Quotation Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                               "</ul><div></div></div><br/>"
        'End If
        If strFrm = "RFQ_Quote" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"
        Else
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQListExp.aspx", "pageid=" & strPageId) & """><span>Expired / Rejected RFQ</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId) & """><span>Quotation Listing</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div><br/>"
        End If

    End Sub

  Private Sub PreviewQuotation()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, ViewState("VCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT (SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)) AS CMState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS SupplierAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                '            & "AS SupplierAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) AND (CODE_CATEGORY = 'pt')) " _
                '            & "AS PaymentTerm, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                '            & "AS PaymentMethod, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) AND (CODE_CATEGORY = 'St')) " _
                '            & "AS Ship_Term, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) AND (CODE_CATEGORY = 'sm')) " _
                '            & "AS Ship_Mode, COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, " _
                '            & "COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                '            & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                '            & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                '            & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                '            & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                '            & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, " _
                '            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                '            & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, " _
                '            & "COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, " _
                '            & "RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, " _
                '            & "RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                '            & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, " _
                '            & "RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, " _
                '            & "RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, " _
                '            & "RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, RFQ_MSTR.RM_RFQ_OPTION, " _
                '            & "RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Offer_Till, RFQ_REPLIES_MSTR.RRM_ETD, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Ship_Term, RFQ_REPLIES_MSTR.RRM_Created_On, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Status, RFQ_REPLIES_MSTR.RRM_B_Display_Status, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Display_Status, RFQ_REPLIES_MSTR.RRM_Indicator, " _
                '            & "RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Product_Code, RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Quantity, RFQ_REPLIES_DETAIL.RRD_Unit_Price, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Desc, RFQ_REPLIES_DETAIL.RRD_Product_Desc, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_UOM, RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, RFQ_REPLIES_DETAIL.RRD_Remarks, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID, " _
                '            & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                '            & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                '            & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                '            & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                '            & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, " _
                '            & "COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                '            & "FROM      RFQ_MSTR INNER JOIN " _
                '            & "RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID INNER JOIN " _
                '            & "RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID AND " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id INNER JOIN " _
                '            & "COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                '            & "COMPANY_MSTR AS COMPANY_MSTR_1 ON " _
                '            & "RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                '            & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                '            & "(RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND " _
                '            & "(RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
                .CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState,d.CODE_DESC AS SupplierAddrCtry," _
                        & "e.CODE_DESC AS PaymentTerm,f.CODE_DESC AS PaymentMethod,g.CODE_DESC AS Ship_Term,h.CODE_DESC AS Ship_Mode," _
                        & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                        & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                        & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                        & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                        & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                        & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                        & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                        & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
                        & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
                        & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                        & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
                        & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
                        & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
                        & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                        & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
                        & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                        & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
                        & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                        & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                        & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                        & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
                        & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
                        & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                        & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
                        & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
                        & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
                        & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                        & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                        & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                        & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                        & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                        & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                        & "FROM RFQ_MSTR " _
                        & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
                        & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
                        & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
                        & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                        & "INNER JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
                        & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
                        & "INNER JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
                        & "AND (b.CODE_CATEGORY = 'ct') " _
                        & "INNER JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
                        & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
                        & "INNER JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
                        & "AND (d.CODE_CATEGORY = 'ct') " _
                        & "INNER JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
                        & "AND (e.CODE_CATEGORY = 'pt') " _
                        & "INNER JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
                        & "AND (f.CODE_CATEGORY = 'pm') " _
                        & "INNER JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
                        & "AND (g.CODE_CATEGORY = 'St') " _
                        & "INNER JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
                        & "AND (h.CODE_CATEGORY = 'sm') " _
                        & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
                        & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", ViewState("VCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", ViewState("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmQuoNum", ViewState("QuoNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", ViewState("RFQNo")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewQuotation_FTN_DataTablePreviewQuotation", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewQuotation-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
               "<DeviceInfo>" + _
                   "  <OutputFormat>EMF</OutputFormat>" + _
                   "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "RFQ\Quotation.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            'strJScript += "window.open('Quotation.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            'strJScript += "window.opener.location.reload(false); "
            strJScript += "window.open('Quotation.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    'Private Sub cmdView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdView.Click
    '    PreviewQuotation()
    'End Sub

End Class

