'Imports System.Data.SqlClient
'Imports Wheel.Components
'***********************
'filetype=1 for cencalletion 
'filetype=2 for view 
'v = vendor
'otherv = - vendor (but for this page is same as other)
'         - it's used in POLineDetail
'b = buyer overall -> change to other
'u = buyer with UserId
'Session("ack") check ackownlegment
'when preview PO Report, side not make sense coz not used in the query to retrieved the report

Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PODetail
    Inherits AgoraLegacy.AppBaseClass
    Dim objFile As New FileManagement
    Dim PO_STATUS As Integer
    Dim aryPast3Month As New ArrayList()
    Dim aryNext3Month As New ArrayList()
    Dim intTotCnt As Integer
    Dim strLargestPOLine, strPrePOLine As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim crDate As Date
    Dim objDB As New EAD.DBCom
    Dim dtBCM As DataTable
    Protected WithEvents Image1 As New System.Web.UI.WebControls.Image
    Protected WithEvents Image2 As New System.Web.UI.WebControls.Image
    Protected WithEvents Image3 As New System.Web.UI.WebControls.Image
    Protected WithEvents Image4 As New System.Web.UI.WebControls.Image
    Protected WithEvents Image5 As New System.Web.UI.WebControls.Image


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_POListStock As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_doc As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_cr As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_apprflow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblContact As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrderDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblAcceptedDate As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPoNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCRNum As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblshipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipVia As System.Web.UI.WebControls.Label
    Protected WithEvents lblDelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFileAttac As System.Web.UI.WebControls.Label
    Protected WithEvents lblFileAttacInt As System.Web.UI.WebControls.Label
    Protected WithEvents term_con As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents link_term As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents detail As System.Web.UI.HtmlControls.HtmlAnchor
    'Protected WithEvents lbltitle1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_accept As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_reject As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_dupPO As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_preview As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmd_ack As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cr As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table8 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table9 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table11 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trdiv As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDivPRNo As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents hidCRNo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidDO As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidCR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidApprflow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmd_back As Global.System.Web.UI.WebControls.Button
    Protected WithEvents lblPRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblExtAttach As System.Web.UI.WebControls.Label
    Protected WithEvents lblExtRemark As System.Web.UI.WebControls.Label
    Protected WithEvents hidIntAttach As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblInternalRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblExternalRemark As System.Web.UI.WebControls.Label
    Protected WithEvents div1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents div2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents POLineForStock As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidInt1 As Global.System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents hidInt2 As Global.System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents hidInt3 As Global.System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tr_SEH1 As System.Web.UI.HtmlControls.HtmlTableRow

    Public Enum EnumPOList
        icPOLine = 0

        'Jules 2018.05.07 - PAMB Scrum 2 - Added Gift & Analysis Codes.
        icGift = 1
        icFundType = 2
        icPersonCode = 3
        icProjectCode = 4
        icItemCode = 5
        icGLCode = 6 '2
        icCatCode = 7 '3
        icAssetCode = 8 '4
        icProDesc = 9 '5
        icUOM = 10 '6
        icUnitPrice = 11 '7
        icGstRate = 12 '8
        icGstAmt = 13 '9
        icGstTaxCode = 14 '10 'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
        icExtDate = 15 '11 '10
        icMPQ = 16 '12 '11
        icWaranty = 17 '13 '12
        icOrderQty = 18 '14 '13
        icOutstanding = 19 '15 '14
        icRecQty = 20 '16 '15
        icRejQty = 21 '17 '16
        icBudgetAcc = 22 '18 '17
        icDelAddr = 23 '19 '18
        icRemarks = 24 '20 '19
        icFulFil = 25 '21 '20
    End Enum

    Public Enum EnumPOListStk
        icPOLine = 0
        icItemCode = 1
        icAssetCode = 2
        icProDesc = 3 '2
        icUOM = 4 '3
        icUnitPrice = 5
        icGstRate = 6
        icGstAmt = 7
        icGstTaxCode = 8 'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
        icExtDate = 9 '8
        icMPQ = 10 '9
        icWaranty = 11 '10
        icOrderQty = 12 '11
        icOutstanding = 13 '12
        icRecQty = 14 '13
        icRejQty = 15 '14
        icDelAddr = 16 '15
        icPast1stMthUsage = 17 '16
        icPast2ndMthUsage = 18 '17
        icPast3rdMthUsage = 19 '18
        icPast3MthAve = 20 '19
        icCurrMthQty = 21 '20
        icNext1stMthQty = 22 '21
        icNext2ndMthQty = 23 '22
        icNext3rdMthQty = 24 '23
        icStkOnHand = 25 '24
        icPOBalance = 26 '25
        icPOInProgress = 27 '26
        icForeCast = 28 '27
        icPurSpecNo = 29 '28
        icSpec1 = 30 '29
        icSpec2 = 31 '30
        icSpec3 = 32 '31
        icLeadTime = 33 '32
        icManu = 34 '33
        icRemarks = 35 '34
        icFulFil = 36 '35
    End Enum

    Public Enum EnumDoc
        icDONo = 0
        icCreateDate = 1
        icSubmitDate = 2
        icCreatedBy = 3
        icGRNNo = 4
        icGRDDate = 5
        icRecDate = 6
        icGRNCreateBy = 7
    End Enum

    Public Enum EnumCR
        icCRNo = 0
        icCreateDate = 1
        icCreatedBy = 2
        icVCRNo = 3
    End Enum

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ' strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    CheckButtonAccess(True)
    'End Sub
    Dim strCaller As String

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_POList)
        SetGridProperty(dtg_POListStock)
        strCaller = UCase(Request.QueryString("caller"))
        Dim objpo As New PurchaseOrder
        Dim objpo1 As New PurchaseOrder_Vendor
        Dim objval As New POValue
        Dim objDb As New EAD.DBCom
        Dim objGst As New GST

        If Not IsPostBack Then

            'Check for Delivery Term setup
            If objDb.Exist("SELECT '*' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Request(Trim("BCoyID")) & "'") Then
                Session("blnSEH") = True
            Else
                Session("blnSEH") = False
            End If

            'Chee Hong (28/5/2014) - Issue 1882 
            Dim objPR_PR As New PR
            ViewState("BCM") = CInt(objPR_PR.checkBCM)
            objPR_PR = Nothing

            Dim dtDate As DateTime
            Dim c As Integer
            Dim strMonth, strYear As String
            ' Status=12: Void PO
            Dim tempdate As Date = objDb.GetVal("SELECT IF(POM_PO_STATUS=12, POM_CREATED_DATE,POM_SUBMIT_DATE) AS POM_SUBMIT_DATE FROM PO_MSTR WHERE POM_PO_NO = '" & Request(Trim("PO_No")) & "' AND POM_B_COY_ID = '" & Request(Trim("BCoyID")) & "'")

            'Get last 3 Months date stored into array
            For c = 0 To 3 - 1
                If c = 0 Then
                    dtDate = tempdate.AddMonths(-3).AddDays(-Now.Date.Day + 1)
                ElseIf c = 1 Then
                    dtDate = tempdate.AddMonths(-2).AddDays(-Now.Date.Day + 1)
                Else
                    dtDate = tempdate.AddMonths(-1).AddDays(-Now.Date.Day + 1)
                End If

                strMonth = dtDate.Month 'Mid(dtDate, 1, 2)
                strYear = dtDate.Year 'Mid(dtDate, 7, 4)

                aryPast3Month.Add(New String() {strMonth, strYear})
            Next
            Session("aryPast3Month") = aryPast3Month

            'Get next 3 Months date stored into array
            For c = 0 To 3 - 1
                If c = 0 Then
                    dtDate = tempdate.AddMonths(+1).AddDays(-Now.Date.Day + 1)
                ElseIf c = 1 Then
                    dtDate = tempdate.AddMonths(+2).AddDays(-Now.Date.Day + 1)
                Else
                    dtDate = tempdate.AddMonths(+3).AddDays(-Now.Date.Day + 1)
                End If

                strMonth = dtDate.Month 'Mid(dtDate, 1, 2)
                strYear = dtDate.Year 'Mid(dtDate, 7, 4)

                aryNext3Month.Add(New String() {strMonth, strYear})
            Next
            Session("aryNext3Month") = aryNext3Month

            Session("backtodetail") = strCallFrom
            If Session("side") = "" Then
                Session("side") = Request(Trim("side"))
            Else
                If Request(Trim("side")) <> "" Then
                    Session("side") = Request(Trim("side"))
                End If
            End If
            GenerateTab()
            If Session("filetype") = "" Then
                Session("filetype") = Request(Trim("filetype"))
            Else
                If Request(Trim("filetype")) <> "" Then
                    Session("filetype") = Request(Trim("filetype"))
                End If
            End If

            If Session("po_index") = "" Then
                Session("po_index") = Request(Trim("PO_INDEX"))
            Else
                If Request(Trim("PO_INDEX")) <> "" Then
                    Session("po_index") = Request(Trim("PO_INDEX"))
                End If
            End If
            If Session("cr_no") = "" Then
                Session("cr_no") = Request(Trim("cr_no"))
            Else
                If Session("cr_no") <> Request(Trim("cr_no")) And Request(Trim("cr_no")) <> "" Then
                    Session("cr_no") = Request(Trim("cr_no"))
                End If
            End If

            If Session("PO_STATUS1") = "" Then
                If Request(Trim("status")) <> "" Then
                    Session("PO_STATUS1") = Request(Trim("status"))
                    Session("status_dis") = objpo.get_status(Session("PO_STATUS1"))
                End If
            Else
                If Session("PO_STATUS1") <> Request(Trim("status")) And Request(Trim("status")) <> "" Then
                    Session("PO_STATUS1") = Request(Trim("status"))
                    Session("status_dis") = objpo.get_status(Session("PO_STATUS1"))
                End If
            End If

            Select Case Session("side")
                Case "otherv", "other", "others"
                    'Me.addCmdPreviewAttribute("v")
                    setControlProp(Session("filetype"))
                    Me.dtg_POList.Columns(EnumPOList.icWaranty).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icRejQty).Visible = True
                    Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = True
                    Me.dtg_POList.Columns(EnumPOList.icRemarks).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = False
                    'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                    'Me.dtg_POList.Columns(EnumPOList.icUnitPrice).Visible = False
                    'Me.dtg_POList.Columns(EnumPOList.icGstRate).Visible = False
                    'Me.dtg_POList.Columns(EnumPOList.icGstAmt).Visible = False
                    'Me.dtg_POList.Columns(EnumPOList.icGstTaxCode).Visible = False
                    'Me.dtg_POList.Columns(EnumPOList.icDelAddr).Visible = False

                    Me.dtg_POListStock.Columns(EnumPOListStk.icWaranty).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icRejQty).Visible = True
                    Me.dtg_POListStock.Columns(EnumPOListStk.icExtDate).Visible = True
                    Me.dtg_POListStock.Columns(EnumPOListStk.icRemarks).Visible = False
                    'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                    'Me.dtg_POListStock.Columns(EnumPOListStk.icUnitPrice).Visible = False
                    'Me.dtg_POListStock.Columns(EnumPOListStk.icGstRate).Visible = False
                    'Me.dtg_POListStock.Columns(EnumPOListStk.icGstTaxCode).Visible = False
                    'Me.dtg_POListStock.Columns(EnumPOListStk.icGstAmt).Visible = False
                    'Me.dtg_POListStock.Columns(EnumPOListStk.icDelAddr).Visible = False      

                    'Jules 2018.05.07 - PAMB Scrum 2 & 3
                    Me.dtg_POList.Columns(EnumPOList.icGift).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = False
                    'End modification.

                    If Request(Trim("cr_no")) <> "" Then ' previously call as side=b and filetype = 1
                        cmd_ack.Visible = False
                        'Me.addCmdCrAttribute("b")
                        'Michelle (27/7/2007) - To return to Transaction Tracking if it is calling from there
                        Dim intpos As Integer
                        If InStr(Session("strurl"), "tracking") = 0 Then
                            Session("strurl") = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&filetype=" & Session("filetype") & "&side=b")
                        End If
                    Else
                        If Session("filetype") = "1" Then
                            'Me.addCmdCrAttribute("b")
                        ElseIf Request(Trim("poview")) <> "" Then ' previously call as side=b and filetype = 2
                            Session("strurl") = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&filetype=" & Session("filetype") & "&side=b")
                            'Session("strurl") = "POViewB3.aspx?pageid=" & strPageId & "&filetype=" & Session("filetype") & "&side=b"
                        End If
                    End If

                Case "b"
                    Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icExtDate).Visible = False

                    If Session("blnSEH") = True Then
                        Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = False

                        'Jules 2018.05.07 - PAMB Scrum 2 & 3
                        Me.dtg_POList.Columns(EnumPOList.icGift).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = False
                        'End modification.
                    Else
                        Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = True
                        If ViewState("BCM") <= 0 Then
                            Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                        Else
                            Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = True
                        End If

                        'Jules 2018.05.07 - PAMB Scrum 2
                        Me.dtg_POList.Columns(EnumPOList.icGift).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = True
                        'End modification.
                    End If

                    '    Me.addCmdCrAttribute("b")
                    '    setControlProp(Session("filetype"))
                    Session("strurl") = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&filetype=" & Session("filetype") & "&side=b")

                    If Session("urlreferer") = "PRAll" Then
                        Session("strurl") = dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId & "")
                    End If

                    If Session("urlreferer") = "PRRej" Then
                        Session("strurl") = dDispatcher.direct("PR", "SearchAPP_All.aspx", "pageid=" & strPageId & "")
                    End If

                    If Session("urlreferer") = "POViewTrx" Then
                        Session("strurl") = dDispatcher.direct("PO", "POViewTrx.aspx", "filetype=2&side=u&pageid=7&type=MyPO")
                    End If

                    If strCaller = "TRANSTRACK" Then
                        Session("strurl") = dDispatcher.direct("tracking", "transtracking.aspx", "pageid=16&coytype=buyer")
                    End If

                    If strCaller = "CONVERTPRLIST" Then
                        Session("strurl") = dDispatcher.direct("PR", "ConvertPRListing.aspx", "pageid=" & strPageId)
                    End If
                Case "u"
                    Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icExtDate).Visible = False

                    If Session("blnSEH") = True Then
                        Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = False

                        'Jules 2018.05.07 - PAMB Scrum 2 & 3
                        Me.dtg_POList.Columns(EnumPOList.icGift).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = False
                        'End modification.
                    Else
                        Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = True
                        If ViewState("BCM") <= 0 Then
                            Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                        Else
                            Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = True
                        End If

                        'Jules 2018.05.07 - PAMB Scrum 2 & 3
                        Me.dtg_POList.Columns(EnumPOList.icGift).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = True
                        Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = True
                        'End modification.
                    End If

                    If Session("filetype") = "2" Then
                        'Me.addCmdPreviewAttribute("b")
                        'Me.addCmdCrAttribute("b")
                        setControlProp(Session("filetype"))
                        Session("strurl") = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&filetype=2&side=u")
                    End If

                Case "v"
                    Me.hidApprflow.Style("display") = "none"
                    'Me.addCmdPreviewAttribute("v")
                    'Me.addCmdCrAttribute("v")
                    Me.dtg_POList.Columns(EnumPOList.icWaranty).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icRejQty).Visible = True
                    Me.dtg_POList.Columns(EnumPOList.icExtDate).Visible = True
                    Me.dtg_POList.Columns(EnumPOList.icRemarks).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icBudgetAcc).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icCatCode).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icGLCode).Visible = False
                    'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                    Me.dtg_POList.Columns(EnumPOList.icUnitPrice).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icGstRate).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icGstTaxCode).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icGstAmt).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icDelAddr).Visible = False

                    Me.dtg_POListStock.Columns(EnumPOListStk.icWaranty).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icRejQty).Visible = True
                    Me.dtg_POListStock.Columns(EnumPOListStk.icExtDate).Visible = True
                    Me.dtg_POListStock.Columns(EnumPOListStk.icRemarks).Visible = False
                    'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                    Me.dtg_POListStock.Columns(EnumPOListStk.icUnitPrice).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icGstRate).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icGstTaxCode).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icGstAmt).Visible = False
                    Me.dtg_POListStock.Columns(EnumPOListStk.icDelAddr).Visible = False

                    'Jules 2018.05.07 - PAMB Scrum 2 & 3
                    Me.dtg_POList.Columns(EnumPOList.icGift).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icFundType).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icPersonCode).Visible = False
                    Me.dtg_POList.Columns(EnumPOList.icProjectCode).Visible = False
                    'End modification.

                    Me.lblExtAttach.Text = "File(s) Attached"
                    Me.hidIntAttach.Style("display") = "none"

                    Me.lblExtRemark.Text = "Remarks"
                    Me.hidInt1.Style("display") = "none"
                    Me.hidInt2.Style("display") = "none"
                    Me.hidInt3.Style("display") = "none"

                    If strCaller = "POVIEWB" Or Request.QueryString("Frm") = "Dashboard" Then
                        'Session("strurl") = "POViewB.aspx?pageid=" & strPageId & " "
                        Me.hidDO.Style("display") = "none"
                        Me.Table8.Rows(0).Visible = False
                        Me.hidCR.Style("display") = "none"
                        Me.hidApprflow.Style("display") = "none"
                        Me.Table11.Rows(0).Visible = False
                        If strCaller = "POVIEWB" Then Session("strurl") = dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & " ") & " "
                    Else
                        Session("strurl") = dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId & " ")
                        Me.hidDO.Style("display") = "inline"
                        Me.hidCR.Style("display") = "inline"
                    End If
                    If Session("filetype") = "2" Then 'Buyer side
                        Dim ds As New DataSet
                        Dim dtr As DataRow
                        Dim POstatus As New DataTable
                        POstatus.Columns.Add("status", Type.GetType("System.String"))
                        POstatus.Columns.Add("datakey", Type.GetType("System.String"))
                        POstatus.Columns.Add("BCoyID", Type.GetType("System.String"))
                        POstatus.Columns.Add("remark", Type.GetType("System.String"))
                        'Session("strurl") = "POViewB.aspx?pageid=" & strPageId & " "
                        Me.lblHeader1.Text = "Purchase Order Header"
                        Me.trdiv.Style("display") = "none"
                        'Me.Table8.Style("display") = "none"

                        '------New Code Added To Make CMD Reject,AcceptPo Visisble="True" whlie login by userid=Carol By Praveen  on 08.08.2007
                        Dim PO_STATUS As Integer = objpo1.get_po_StatusNo(Session("po_index"))
                        '---------End
                        If PO_STATUS = POStatus_new.Open Or PO_STATUS = POStatus_new.NewPO Then
                            Me.cmd_accept.Visible = True
                            Me.lblAction.Visible = True
                            Me.cmd_reject.Visible = True
                            Dim status As Integer = POStatus_new.Open
                            dtr = POstatus.NewRow()
                            dtr("status") = status
                            dtr("datakey") = Request(Trim("PO_No"))
                            dtr("BCoyID") = Request(Trim("BCoyID"))
                            dtr("remark") = ""
                            POstatus.Rows.Add(dtr)
                            ds.Tables.Add(POstatus)
                            objpo1.update_POStatus(ds)
                            Me.txtRemark.Enabled = True
                        End If
                    ElseIf Session("filetype") = "1" Then
                        'If strCaller = "POVIEWB" Then Me.hidDO.Style("display") = "none"

                        setControlProp(Session("filetype"))
                        'Session("strurl") = "ViewCancel.aspx?pageid=" & strPageId & "&filetype=1&side=v"
                    End If
                    'If Session("filetype") = "2" And Session("side") = "v" And PO_STATUS = POStatus_new.Open Or PO_STATUS = POStatus_new.NewPO Then
                    '    Me.cmd_accept.Visible = True
                    '    Me.cmd_reject.Visible = True
                    'End If

            End Select
            '----New Code Added to PoViewB3 from Podetail by praveen  on 24/07/2007

            If Session("filetype") = "2" And Session("side") = "others" Then
                Session("strurl") = dDispatcher.direct("PO", "POViewB3.aspx", "caller=" & strCaller & "&pageid=" & strPageId & "&filetype=" & Session("filetype") & "&BCoyID=" & Request(Trim("BCoyID")) & "&PO_No=" & Request(Trim("PO_No")) & "&prid=" & Request(Trim("PRNum")) & "&side=others")
            End If
            '---end 
            Dim objTRa As New Tracking
            Dim dt As DataTable
            Dim i As Integer
            Dim strPR As String

            objTRa = Nothing
            objval.PO_Number = Request(Trim("PO_No"))
            objval.buyer_coy = Request(Trim("BCoyID"))
            ' objval.PO_Status = Request(Trim("status"))
            objpo.get_PODetail(objval, Session("side"), False)
            Session("PRNo") = objpo.getPRNo(objval.PO_Number) 'added by michael for issue 621
            If Session("status_dis") = "" Or IsNothing(Session("status_dis")) Then
                Session("status_dis") = objpo.get_postatus(Session("po_index"))
            End If
            Session("Fulfilment_status") = objpo.get_po_Fulfilment(Session("po_index"))

            If Session("side") = "v" Then
                Select Case Session("status_dis")
                    Case "New", "Open"
                        Me.lblStatus.Text = "New"
                    Case "Cancelled", "Rejected"
                        Me.lblStatus.Text = "Cancelled"
                    Case "Closed"
                        Me.lblStatus.Text = "Closed"
                    Case Else
                        If Session("Fulfilment_status") = "3" Then
                            Me.lblStatus.Text = "Closed"
                        Else
                            Me.lblStatus.Text = "Outstanding"
                        End If
                End Select
            Else
                Dim SName As String = objDb.GetVal("SELECT IFNULL((SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'),'') AS NAME FROM PO_MSTR WHERE POM_PO_NO = '" & Request(Trim("PO_No")) & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                If strCaller = "SEARCHPO_ALL" Then
                    Session("strurl") = dDispatcher.direct("PO", "SearchPO_ALL.aspx", "pageid=8")
                    If Session("status_dis") = "Rejected By" Then
                        Me.lblStatus.Text = "Rejected"
                    ElseIf Session("status_dis") = "Held By" Or Request(Trim("status")) = "11" Then
                        Me.lblStatus.Text = "Held By " & SName
                    Else
                        Me.lblStatus.Text = "Approved"
                    End If
                ElseIf strCaller = "PRAPPALL" Then
                    Select Case Session("status_dis")
                        Case "Draft"
                            Me.lblStatus.Text = "Draft"
                        Case "Submitted", "Pending Approval"
                            Me.lblStatus.Text = "Submitted for approval (Internal)"
                        Case "Held By"
                            Me.lblStatus.Text = "Held By " & SName
                        Case "New", "Open", "Approved"
                            Me.lblStatus.Text = "Approved by management (Official)"
                        Case "Accepted"
                            Me.lblStatus.Text = "Accepted by vendor"
                        Case "Closed"
                            Me.lblStatus.Text = "Completed delivery and paid"
                        Case "Cancelled", "Cancelled By"
                            Me.lblStatus.Text = "Cancelled by buyer"
                        Case "Rejected", "Rejected By"
                            Me.lblStatus.Text = "Rejected by management / vendor"
                        Case "Void"
                            Me.lblStatus.Text = "Void draft PO"
                        Case Else
                            Me.lblStatus.Text = Session("status_dis")
                    End Select
                ElseIf strCaller = "PRALL" Or strCaller = "CONVERTPRLIST" Then
                    Select Case Session("status_dis")
                        Case "Draft"
                            Me.lblStatus.Text = "Draft"
                        Case "Submitted", "Pending Approval"
                            Me.lblStatus.Text = "Submitted for approval (Internal)"
                        Case "Held By"
                            Me.lblStatus.Text = "Held By " & SName
                        Case "New", "Open", "Approved"
                            Me.lblStatus.Text = "Approved by management (Official)"
                        Case "Accepted"
                            Me.lblStatus.Text = "Accepted by vendor"
                        Case "Closed"
                            Me.lblStatus.Text = "Completed delivery and paid"
                        Case "Cancelled", "Cancelled By"
                            Me.lblStatus.Text = "Cancelled by buyer"
                        Case "Rejected", "Rejected By"
                            Me.lblStatus.Text = "Rejected by management / vendor"
                        Case "Void"
                            Me.lblStatus.Text = "Void draft PO"
                        Case Else
                            Me.lblStatus.Text = Session("status_dis")
                    End Select
                Else
                    Select Case Session("status_dis")
                        Case "Draft"
                            Me.lblStatus.Text = "Draft"
                        Case "Submitted", "Pending Approval"
                            Me.lblStatus.Text = "Submitted for approval (Internal)"
                        Case "Held By"
                            Me.lblStatus.Text = "Held By " & SName
                        Case "New", "Open", "Approved"
                            Me.lblStatus.Text = "Approved by management (Official)"
                        Case "Accepted"
                            Me.lblStatus.Text = "Accepted by vendor"
                        Case "Closed"
                            Me.lblStatus.Text = "Completed delivery and paid"
                        Case "Cancelled", "Cancelled By"
                            Me.lblStatus.Text = "Cancelled by buyer"
                        Case "Rejected", "Rejected By"
                            Me.lblStatus.Text = "Rejected by management / vendor"
                        Case "Void"
                            Me.lblStatus.Text = "Void draft PO"
                        Case Else
                            Me.lblStatus.Text = Session("status_dis")
                    End Select
                End If
            End If

            If strCaller = "PRAPP" Then
                back.Visible = False
                cmd_back.Visible = True
                Me.cmd_back.Attributes.Add("onclick", "window.close(); ")
            End If

            'Me.lblStatus.Text = Session("status_dis")
            Session("status_dis") = ""
            'Me.lblPoNo.Text = Request(Trim("PO_No"))
            Session("lblPoNo") = Request(Trim("PO_No"))
            If objval.urgent = "1" Then
                Me.lblPoNo.Text = Session("lblPoNo") + " (Urgent)"
            Else
                Me.lblPoNo.Text = Session("lblPoNo")
            End If

            ViewState("POIndex") = objval.POIndex
            Me.lblContact.Text = objval.buyer_contact
            Me.lblCurrCode.Text = objval.cur
            Me.lblFax.Text = objval.buyer_fax
            Me.lblOrderDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.PO_Date)
            Me.lblAcceptedDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.POM_ACCEPTED_DATE)
            Me.lblDelTerm.Text = objval.del_term
            Me.lblVendorCode.Text = objval.vendor_code
            Me.lblPaymentMethod.Text = objval.pay_meth
            Me.lblPaymentTerm.Text = objval.pay_term
            Me.lblShipMethod.Text = objval.ship_meth
            Me.lblshipTerm.Text = objval.ship_term
            Me.lblShipTo.Text = Replace(objpo.get_delivery_add_multiline(Request(Trim("BCoyID")), Request(Trim("PO_No"))), "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;" 'Replace(objval.buyer_adds, "<BR>", "<BR>&nbsp;")
            Me.lblTel.Text = objval.buyer_tel
            Me.lblVendor.Text = objval.vendor_Coy
            Me.lblVendorAddr.Text = Replace(objval.vendor_adds, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"
            Me.lblVendorEmail.Text = objval.vendor_email
            Me.lblVendorFax.Text = objval.vendor_fax
            Me.lblVendorTel.Text = objval.vendor_tel
            Me.txtRemark.Text = objval.Vendor_remark
            Me.lblEmail.Text = objval.buyer_email
            Me.lblShipVia.Text = objval.ship_via

            Me.lblInternalRemark.Text = objval.intremarks
            Me.lblExternalRemark.Text = objval.remarks
            'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
            'Zulham 13122018
            If Not Common.parseNull(objval.Submit_PO_Date).ToString.Trim = "" Then
                ViewState("GstPO") = objGst.chkGSTCOD(Format(CDate(objval.Submit_PO_Date), "dd/MM/yyyy"))
            Else
                ViewState("GstPO") = False
            End If

            ViewState("FFPO") = objval.FFPO_Type

            Select Case Session("side")
                Case "b", "u", "otherv", "other", "others"
                    If ViewState("GstPO") = False Then
                        Me.dtg_POList.Columns(EnumPOList.icGstRate).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icGstTaxCode).Visible = False
                        Me.dtg_POList.Columns(EnumPOList.icGstAmt).Visible = False
                        Me.dtg_POListStock.Columns(EnumPOListStk.icGstRate).Visible = False
                        Me.dtg_POListStock.Columns(EnumPOListStk.icGstTaxCode).Visible = False
                        Me.dtg_POListStock.Columns(EnumPOListStk.icGstAmt).Visible = False
                    End If

                    'Stage 3 (Enhancement) (GST-0028) - 109/07/2015 - CH
                    If ViewState("FFPO") = "Y" And UCase(objval.Created_By) = UCase(Session("UserId")) Then
                        cmd_dupPO.Style("display") = ""
                    End If
            End Select

            'If Session("blnSEH") = True Then
            '    Me.tr_SEH1.Style("display") = ""
            'End If

            crDate = objval.POM_CREATED_DATE
            ViewState("CRDt") = crDate

            Dim dvFile As DataView
            Dim intLoop, intCount As Integer
            Dim strFile, strFile1, strURL, strTemp, strTempInt As String
            dvFile = objpo.getPoAttachment(Request(Trim("PO_No")), Request(Trim("BCoyID"))).Tables(0).DefaultView
            If dvFile.Count > 0 Then
                For intLoop = 0 To dvFile.Count - 1
                    If Common.parseNull(dvFile(intLoop)("CDA_TYPE")) = "E" Then
                        strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                        strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")
                        '*************************meilai 25/2/05****************************
                        'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PO>" & strFile & "</A>"
                        strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PO", EnumUploadFrom.FrontOff)
                        '*************************meilai************************************
                        If strTemp = "" Then
                            strTemp = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                            intCount = intCount + 1
                        Else
                            strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                            intCount = intCount + 1
                        End If
                    End If

                Next

                If Session("side") <> "v" Then
                    intCount = 0
                    For intLoop = 0 To dvFile.Count - 1
                        If Common.parseNull(dvFile(intLoop)("CDA_TYPE")) = "I" Then
                            strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                            strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")
                            '*************************meilai 25/2/05****************************
                            'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PO>" & strFile & "</A>"
                            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PO", EnumUploadFrom.FrontOff)
                            '*************************meilai************************************
                            If strTempInt = "" Then
                                strTempInt = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                                intCount = intCount + 1
                            Else
                                strTempInt = strTempInt & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                                intCount = intCount + 1
                            End If
                        End If

                    Next
                End If
            Else
                strTemp = "No Files Attached"
                strTempInt = "No Files Attached"
            End If
            lblFileAttac.Text = strTemp
            lblFileAttacInt.Text = strTempInt

            Me.link_term.InnerHtml = objFile.getAttachPath(Server.UrlEncode(objval.TermAndCond), "Click Here", Server.UrlEncode(objval.TermAndCond), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff)
            '*************************meilai************************************
        End If

        'Chee Hong (20/05/2014) - Enhancement to display budget account column for Enterprise version
        Dim objBCM As New BudgetControl
        dtBCM = objBCM.getBCMListByCompanyNew()
        objBCM = Nothing

        'Get datagrid
        hidSummary.Value = "Remarks-" & txtRemark.ClientID
        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmd_accept.Attributes.Add("onClick", "return resetSummary(1,0);")
        cmd_reject.Attributes.Add("onClick", "return confirmReject();")

        Bindgrid(Me.dtg_POList, 1)
        Bindgrid(Me.dtg_POListStock, 1)
        Bindgrid(Me.dtg_doc, 2)
        Bindgrid(Me.dtg_cr, 3)
        If Session("side") = "b" Then
            Bindgrid(Me.dtg_apprflow, 4)
        End If

        If Request.QueryString("Frm") = "Dashboard" Then
            Session("strurl") = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("Frm") = "InvList" Then
            Session("strurl") = dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("Frm") = "GRNSearch" Then
            Session("strurl") = dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("Frm") = "ROListing" Then
            Session("strurl") = dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId)
        End If
        If Trim(Me.lblOrderDate.Text) = "" Then
            cmd_preview.Visible = False
        Else
            cmd_preview.Visible = True
        End If
        'added by michael for issue 621
        If Session("PRNo") <> "" Then
            Me.trDivPRNo.Style("display") = "inline"
            Me.lblPRNo.Text = Session("PRNo")
        End If

        '####Image######
        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image2.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image3.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image4.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image5.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Me.cmd_preview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_No=" & Request(Trim("PO_No")) & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")
        'Me.cmd_cr.Attributes.Add("onclick", "PopWindow('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
        Me.cmd_cr.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "PO_No=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")

        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtg_POList.Columns(EnumPOList.icAssetCode).Visible = False
            dtg_POListStock.Columns(EnumPOListStk.icAssetCode).Visible = False
        End If
    End Sub
    Private Function Bindgrid(ByVal dgid As DataGrid, ByVal turn As String, Optional ByVal pSorted As Boolean = False) As String
        Dim objPO As New PurchaseOrder
        Dim objPR As New PurchaseReq2

        '//Retrieve Data from Database
        Dim ds As DataSet

        'Dim objdb As New EAD.DBCom()
        If turn = "1" Then
            ds = objPO.getlineitem(Session("lblPoNo"), Session("side"), False, Request(Trim("BCoyID")), False)

        ElseIf turn = "2" Then
            ds = objPO.get_docitem(Session("lblPoNo"), Session("side"), Request(Trim("BCoyID")))
        ElseIf turn = "3" Then
            If Session("urlreferer") = "POViewTrx" Then
                ds = objPO.get_CRView(Session("lblPoNo"), "", "", Request(Trim("BCoyID")), "")
            Else
                If Request(Trim("checkid")) = "no" Then
                    ds = objPO.get_CRView(Session("lblPoNo"), "", "", Request(Trim("BCoyID")), "")
                Else
                    ds = objPO.get_CRView(Session("lblPoNo"), "", Session("side"), Request(Trim("BCoyID")), "")
                End If

            End If
        ElseIf turn = "4" Then
            ds = objPR.getApprFlow(ViewState("POIndex"), "PO")
        End If

        Dim PO_No As String = ""

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        ViewState("SortExpression") = ViewState("SortExpression_" & dgid.ID)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And Not ViewState("SortExpression") Is Nothing Then dvViewPR.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        'intPageRecordCnt = ds.Tables(0).Rows.Count
        'intPageRecordCnt = viewstate("intPageRecordCnt")
        If turn = "1" Then
            ViewState("intPageRecordCnt1") = ds.Tables(0).Rows.Count
        ElseIf turn = "2" Then
            ViewState("intPageRecordCnt2") = ds.Tables(0).Rows.Count
        ElseIf turn = "3" Then
            ViewState("intPageRecordCnt3") = ds.Tables(0).Rows.Count
        ElseIf turn = "4" Then
            ViewState("intPageRecordCnt4") = ds.Tables(0).Rows.Count
        End If
        'viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        'If intPageRecordCnt > 0 Then
        If ds.Tables(0).Rows.Count > 0 Then
            If dgid.ID.ToString.Trim = "dtg_POList" Or dgid.ID.ToString.Trim = "dtg_POListStock" Then
                intTotCnt = ds.Tables(0).Rows.Count
                strPrePOLine = "0"
                strLargestPOLine = ds.Tables(0).Rows(intTotCnt - 1).Item("POD_PO_LINE")
            End If
            'intTotPage = dtgDept.PageCount
            dgid.DataSource = dvViewPR
            dgid.DataBind()
            If dgid.ID.ToString.Trim = "dtg_POList" Then
                If Session("side") = "v" Or Session("side") = "otherv" Then
                    dgid.Columns(EnumPOList.icOutstanding).Visible = False
                ElseIf Session("side") = "b" Then
                    dgid.Columns(EnumPOList.icExtDate).Visible = False
                End If
            ElseIf dgid.ID.ToString.Trim = "dtg_POListStock" Then
                If Session("side") = "v" Or Session("side") = "otherv" Then
                    dgid.Columns(EnumPOListStk.icOutstanding).Visible = False
                ElseIf Session("side") = "b" Then
                    dgid.Columns(EnumPOListStk.icExtDate).Visible = False
                End If
            End If
        Else
            'dtgDept.DataSource = ""
            dgid.DataBind()
            If turn = "2" Then
                'Me.lbl_do_grn.Text = ""
            ElseIf turn = "3" Then
                'lbl_cancel.Text = ""
            End If
            ' Common.NetMsgbox(Me, "No record found.")
            'intTotPage = 0
        End If

        If ViewState("intPageRecordCnt2") = 0 Then
            Me.hidDO.Style("display") = "none"
            Me.Table8.Rows(0).Visible = False

        Else
            Me.hidDO.Style("display") = "inline"
            Me.Table8.Rows(0).Visible = True
        End If
        If ViewState("intPageRecordCnt3") = 0 Then
            Me.hidCR.Style("display") = "none"
            Me.Table9.Rows(0).Visible = False
        Else
            Me.hidCR.Style("display") = "inline"
            Me.Table9.Rows(0).Visible = True
        End If

        If ViewState("intPageRecordCnt4") = 0 Then
            Me.hidApprflow.Style("display") = "none"
            Me.Table11.Rows(0).Visible = False
        Else
            Me.hidApprflow.Style("display") = "inline"
            Me.Table11.Rows(0).Visible = True
            Me.dtg_apprflow.Columns(2).Visible = True 'changed by michael for issue 621
        End If

        'If Session("Env") = "FTN" Then
        '    Me.dtg_POList.Columns(5).Visible = False
        '    Me.dtg_POList.Columns(6).Visible = False
        '    Me.dtg_apprflow.Columns(2).Visible = False 'added by michael for issue 621
        'Else
        '    Me.dtg_POList.Columns(5).Visible = True
        '    Me.dtg_POList.Columns(6).Visible = True
        'End If
        Me.dtg_POList.Columns(EnumPOList.icMPQ).Visible = True
        Me.dtg_POList.Columns(EnumPOList.icWaranty).Visible = True
        Me.dtg_POListStock.Columns(EnumPOListStk.icMPQ).Visible = True
        Me.dtg_POListStock.Columns(EnumPOListStk.icWaranty).Visible = True
        If strCaller = "POVIEWB" Then
            Me.dtg_POList.Columns(EnumPOList.icRecQty).Visible = False
            Me.dtg_POList.Columns(EnumPOList.icRejQty).Visible = False
            Me.dtg_POListStock.Columns(EnumPOListStk.icRecQty).Visible = False
            Me.dtg_POListStock.Columns(EnumPOList.icRejQty).Visible = False
        End If

        If turn = "1" Then
            If Session("side") = "v" Then
                div1.Visible = True
                dtg_POList.Visible = True
                div2.Visible = False
                dtg_POListStock.Visible = False
            Else
                If objPO.chkPOItemStk(Session("lblPoNo"), Session("side"), Request(Trim("BCoyID"))) And Session("blnSEH") = True Then
                    div1.Style("display") = "none"
                    dtg_POList.Style("display") = "none"
                    div2.Style("display") = ""
                    dtg_POListStock.Style("display") = ""
                    tr_SEH1.Style("display") = ""
                    'dtg_POListStock.Style("width") = "130%"
                    'POLineForStock.Style("width") = "130%"
                    'div2.Style("width") = "130%"
                Else
                    div1.Style("display") = ""
                    dtg_POList.Style("display") = ""
                    div2.Style("display") = "none"
                    dtg_POListStock.Style("display") = "none"
                    dtg_POListStock.Style("width") = "100%"
                    POLineForStock.Style("width") = "100%"
                    div2.Style("width") = "100%"
                End If
            End If
        End If

    End Function
    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim
        If id = "dtg_POList" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_POListStock" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_doc" Then
            Bindgrid(sender, 2, True)
        ElseIf id = "dtg_cr" Then
            Bindgrid(sender, 3, True)
        ElseIf id = "dtg_apprflow" Then
            Bindgrid(sender, 4, True)
        End If
    End Sub

    Public Sub dtg_POListStock_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim
        If id = "dtg_POList" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_POListStock" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_doc" Then
            Bindgrid(sender, 2, True)
        ElseIf id = "dtg_cr" Then
            Bindgrid(sender, 3, True)
        ElseIf id = "dtg_apprflow" Then
            Bindgrid(sender, 4, True)
        End If
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand_WID(sender, e)
        ' dtg_POList.CurrentPageIndex = e.NewPageIndex
        ViewState("SortExpression_" & sender.ID) = ViewState("SortExpression")
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim
        If id = "dtg_POList" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_POListStock" Then
            Bindgrid(sender, 1, True)
        ElseIf id = "dtg_doc" Then
            Bindgrid(sender, 2, True)
        ElseIf id = "dtg_cr" Then
            Bindgrid(sender, 3, True)
        End If
    End Sub

    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt1")
        Grid_ItemCreated(dtg_POList, e)
    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objPO As New PurchaseOrder
            Dim strDAddr As String
            'New Code Added for Line No By Praveen  on 17/07/07 for gird values
            ' e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
            'Michelle (31/7/2007) - To allow sorting of the resequence PO line no
            If strLargestPOLine = dv("POD_PO_LINE") Then 'ie in descending order
                e.Item.Cells(EnumPOList.icPOLine).Text = intTotCnt
                intTotCnt = intTotCnt - 1
            Else
                If strPrePOLine > dv("POD_PO_LINE") Then
                    e.Item.Cells(EnumPOList.icPOLine).Text = intTotCnt
                    intTotCnt = intTotCnt - 1
                Else
                    e.Item.Cells(EnumPOList.icPOLine).Text = e.Item.DataSetIndex + 1
                End If
            End If
            strPrePOLine = dv("POD_PO_LINE")


            '     If viewstate("SortAscending") = "no" Then
            '     e.Item.Cells(0).Text = intTotCnt
            '     intTotCnt = intTotCnt - 1
            'End the code
            '   End If


            '   Dim objdb As New EAD.DBCom
            'If Session("side") = "b" Or Session("side") = "u" Then
            Dim cancel_item As Decimal
            If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                cancel_item = 0
            Else
                cancel_item = CDec(dv("POD_CANCELLED_QTY"))
            End If

            Dim intmax As Decimal = CDec(dv("POD_ORDERED_QTY")) - cancel_item - CDec(dv("POD_DELIVERED_QTY"))
            e.Item.Cells(EnumPOList.icOutstanding).Text = intmax
            'Added by Joon on 19 Sept 2011 for issue 827
            e.Item.Cells(EnumPOList.icRecQty).Text = CDec(dv("POD_RECEIVED_QTY")) - CDec(dv("POD_REJECTED_QTY"))
            'New Code Added for Line No By Praveen  on 16/07/07 for gird values
            'e.Item.Cells(EnumPOList.icPOLine).Text = Common.parseNull(dv("POD_PO_LINE"))
            'e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
            'End the code


            'e.Item.Cells(4).Visible = False

            'If IsDBNull(dv("POD_CANCELLED_QTY")) Then
            '    cancel_item = 0
            'Else
            '    cancel_item = CInt(dv("POD_CANCELLED_QTY"))

            'End If
            'Dim intmax As Integer = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))

            'Else
            If Session("side") = "v" Or Session("side") = "otherv" Then
                'e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""POLineListing.aspx?pageid=" & strPageId & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & " "" ><font color=#0000ff>" & dv("POD_PO_LINE") & "</font></A>"

                '-----New Code Added for Line No By Praveen  on 17/07/07 for gird values
                'e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
                'e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""POLineListing.aspx?pageid=" & strPageId & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & " "" ><font color=#0000ff>" & e.Item.Cells(0).Text & "</font></A>"
                e.Item.Cells(EnumPOList.icPOLine).Text = "<A href=""" & dDispatcher.direct("PO", "POLineListing.aspx", "pageid=" & strPageId & "&lineval=" & e.Item.Cells(0).Text & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID") & "")) & " "" ><font color=#0000ff>" & e.Item.Cells(0).Text & "</font></A>"
                '---End The Code               
            End If

            If Not IsDBNull(ViewState("CRDt")) Then
                If IsDBNull(dv("POD_ETD")) Or Common.parseNull(dv("POD_ETD")) = "0" Then
                    e.Item.Cells(EnumPOList.icExtDate).Text = "Ex-Stock"
                Else
                    Dim edd As Date
                    edd = Common.parseNull(ViewState("CRDt"))
                    e.Item.Cells(EnumPOList.icExtDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, edd.AddDays(Common.parseNull(dv("POD_ETD"))))
                End If
            End If

            'For BCM
            If Not IsDBNull(dv("POD_ACCT_INDEX")) Then
                If Not dtBCM Is Nothing Then
                    Dim drTemp As DataRow()
                    drTemp = dtBCM.Select("Acct_Index=" & dv("POD_ACCT_INDEX"))
                    If drTemp.Length > 0 Then
                        'e.Item.Cells(EnumPR.icBCM).Text = drTemp(0)("Acct_List")
                        e.Item.Cells(EnumPOList.icBudgetAcc).Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        e.Item.Cells(EnumPOList.icBudgetAcc).ToolTip = drTemp(0)("Acct_List")
                    End If
                End If
            End If

            'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
            'Unit Price, Gst Rate, Gst Amt
            e.Item.Cells(EnumPOList.icUnitPrice).Text = Format(Common.parseNull(dv("POD_UNIT_COST"), 0), "#,##0.0000")
            e.Item.Cells(EnumPOList.icGstRate).Text = Common.parseNull(dv("GST_RATE"))
            e.Item.Cells(EnumPOList.icGstAmt).Text = Format(Common.parseNull(dv("POD_TAX_VALUE"), 0), "#,##0.00")

            'Stage 3 (Enhancement) (GST-0006) - 16/07/2015 - CH
            e.Item.Cells(EnumPOList.icGstTaxCode).Text = Common.parseNull(dv("POD_GST_INPUT_TAX_CODE"))

            'Delivery Term
            strDAddr = Common.parseNull(dv("POD_D_ADDR_LINE1"))
            If Not IsDBNull(dv("POD_D_ADDR_LINE2")) AndAlso dv("POD_D_ADDR_LINE2") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE2")
            End If
            If Not IsDBNull(dv("POD_D_ADDR_LINE3")) AndAlso dv("POD_D_ADDR_LINE3") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE3")
            End If
            If Not IsDBNull(dv("POD_D_POSTCODE")) Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_POSTCODE")
            End If
            If Not IsDBNull(dv("POD_D_CITY")) Then
                strDAddr = strDAddr & " " & dv("POD_D_CITY")
            End If
            If Not IsDBNull(dv("STATE")) AndAlso dv("STATE") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("STATE")
            End If
            If Not IsDBNull(dv("CT")) Then
                strDAddr = strDAddr & " " & dv("CT")
            End If

            'Jules 2018.07.31 
            If Common.parseNull(dv("POD_B_CATEGORY_CODE")) = "" Then
                e.Item.Cells(EnumPOList.icCatCode).Text = "N/A"
            End If
            'End modification.

            e.Item.Cells(EnumPOList.icDelAddr).ToolTip = strDAddr
            e.Item.Cells(EnumPOList.icDelAddr).Text = Common.parseNull(dv("POD_D_ADDR_CODE"))
        End If

    End Sub

    Private Sub dtg_POListStock_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POListStock.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt1")
        Grid_ItemCreated(dtg_POListStock, e)

        If e.Item.ItemType = ListItemType.Header Then
            Dim i As Integer
            Dim strMonth As String
            Dim objpo_ext As New PurchaseOrder_Ext
            Dim aryTempList As New ArrayList()

            aryTempList = Session("aryPast3Month")
            For i = 0 To aryTempList.Count - 1
                strMonth = objpo_ext.get_month(CInt(aryTempList(i)(0)))
                If i = 0 Then
                    e.Item.Cells(EnumPOListStk.icPast1stMthUsage).Text = strMonth & " Qty Usage"
                ElseIf i = 1 Then
                    e.Item.Cells(EnumPOListStk.icPast2ndMthUsage).Text = strMonth & " Qty Usage"
                ElseIf i = 2 Then
                    e.Item.Cells(EnumPOListStk.icPast3rdMthUsage).Text = strMonth & " Qty Usage"
                End If
            Next

            aryTempList = Session("aryNext3Month")
            For i = 0 To aryTempList.Count - 1
                strMonth = objpo_ext.get_month(CInt(aryTempList(i)(0)))
                If i = 0 Then
                    e.Item.Cells(EnumPOListStk.icNext1stMthQty).Text = strMonth & " Qty Usage"
                ElseIf i = 1 Then
                    e.Item.Cells(EnumPOListStk.icNext2ndMthQty).Text = strMonth & " Qty Usage"
                ElseIf i = 2 Then
                    e.Item.Cells(EnumPOListStk.icNext3rdMthQty).Text = strMonth & " Qty Usage"
                End If
            Next
        End If

    End Sub

    Private Sub dtg_POListStock_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POListStock.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objPO As New PurchaseOrder
            Dim objpo_ext As New PurchaseOrder_Ext
            Dim arraylist As New ArrayList
            Dim dblQty, dbl1stMthQty, dbl2ndMthQty, dbl3rdMthQty, dbl3MthAve, dblStkOnHand, dblPOBalance, dblPOInProgress, dblCurrMthQty As Double
            Dim i, intMth, intYear As Integer
            Dim strMonth, strDAddr As String

            'New Code Added for Line No By Praveen  on 17/07/07 for gird values
            ' e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
            'Michelle (31/7/2007) - To allow sorting of the resequence PO line no
            If strLargestPOLine = dv("POD_PO_LINE") Then 'ie in descending order
                e.Item.Cells(EnumPOListStk.icPOLine).Text = intTotCnt
                intTotCnt = intTotCnt - 1
            Else
                If strPrePOLine > dv("POD_PO_LINE") Then
                    e.Item.Cells(EnumPOListStk.icPOLine).Text = intTotCnt
                    intTotCnt = intTotCnt - 1
                Else
                    e.Item.Cells(EnumPOListStk.icPOLine).Text = e.Item.DataSetIndex + 1
                End If
            End If
            strPrePOLine = dv("POD_PO_LINE")

            Dim cancel_item As Integer
            If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                cancel_item = 0
            Else
                cancel_item = CInt(dv("POD_CANCELLED_QTY"))
            End If

            Dim intmax As Decimal = CDec(dv("POD_ORDERED_QTY")) - cancel_item - CDec(dv("POD_DELIVERED_QTY"))
            e.Item.Cells(EnumPOListStk.icOutstanding).Text = intmax
            e.Item.Cells(EnumPOListStk.icRecQty).Text = CDec(dv("POD_RECEIVED_QTY")) - CDec(dv("POD_REJECTED_QTY"))

            If Session("side") = "v" Or Session("side") = "otherv" Then
                e.Item.Cells(EnumPOListStk.icPOLine).Text = "<A href=""" & dDispatcher.direct("PO", "POLineListing.aspx", "pageid=" & strPageId & "&lineval=" & e.Item.Cells(0).Text & "&PO_NO=" & dv("POD_PO_NO") & "&po_line=" & dv("POD_PO_LINE") & "&side=v&BCoyID=" & Request(Trim("BCoyID") & "")) & " "" ><font color=#0000ff>" & e.Item.Cells(0).Text & "</font></A>"
            End If

            If Not IsDBNull(ViewState("CRDt")) Then
                If IsDBNull(dv("POD_ETD")) Or Common.parseNull(dv("POD_ETD")) = "0" Then
                    e.Item.Cells(EnumPOListStk.icExtDate).Text = "Ex-Stock"
                Else
                    Dim edd As Date
                    edd = Common.parseNull(ViewState("CRDt"))
                    e.Item.Cells(EnumPOListStk.icExtDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, edd.AddDays(Common.parseNull(dv("POD_ETD"))))
                End If
            End If

            'arraylist = Session("aryPast3Month")
            'For i = 0 To arraylist.Count - 1
            '    intMth = CInt(arraylist(i)(0))  'Month
            '    intYear = CInt(arraylist(i)(1)) 'Year
            '    dblQty = objpo_ext.get_POQtyByMonth(intMth, intYear, Session("side"), Request(Trim("BCoyID")))
            '    strMonth = objpo_ext.get_month(intMth)

            '    If i = 0 Then
            '        e.Item.Cells(EnumPOListStk.icPast1stMthUsage).Text = Format(dblQty, "###0.00")
            '        dbl1stMthQty = dblQty
            '    ElseIf i = 1 Then
            '        e.Item.Cells(EnumPOListStk.icPast2ndMthUsage).Text = Format(dblQty, "###0.00")
            '        dbl2ndMthQty = dblQty
            '    ElseIf i = 2 Then
            '        e.Item.Cells(EnumPOListStk.icPast3rdMthUsage).Text = Format(dblQty, "###0.00")
            '        dbl3rdMthQty = dblQty
            '    End If

            'Next

            If Not IsDBNull(dv("POD_PREV1_QTY")) Then
                e.Item.Cells(EnumPOListStk.icPast1stMthUsage).Text = Format(dv("POD_PREV1_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPast1stMthUsage).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_PREV2_QTY")) Then
                e.Item.Cells(EnumPOListStk.icPast2ndMthUsage).Text = Format(dv("POD_PREV2_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPast2ndMthUsage).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_PREV3_QTY")) Then
                e.Item.Cells(EnumPOListStk.icPast3rdMthUsage).Text = Format(dv("POD_PREV3_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPast3rdMthUsage).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_PREV_AVG")) Then
                e.Item.Cells(EnumPOListStk.icPast3MthAve).Text = Format(dv("POD_PREV_AVG"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPast3MthAve).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_CURR_QTY")) Then
                e.Item.Cells(EnumPOListStk.icCurrMthQty).Text = Format(dv("POD_CURR_QTY"), "###0.00")
                dblCurrMthQty = Format(dv("POD_CURR_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icCurrMthQty).Text = Format(0, "###0.00")
                dblCurrMthQty = 0
            End If

            If Not IsDBNull(dv("POD_NEXT1_QTY")) Then
                e.Item.Cells(EnumPOListStk.icNext1stMthQty).Text = Format(dv("POD_NEXT1_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icNext1stMthQty).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_NEXT2_QTY")) Then
                e.Item.Cells(EnumPOListStk.icNext2ndMthQty).Text = Format(dv("POD_NEXT2_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icNext2ndMthQty).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_NEXT3_QTY")) Then
                e.Item.Cells(EnumPOListStk.icNext3rdMthQty).Text = Format(dv("POD_NEXT3_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icNext3rdMthQty).Text = Format(0, "###0.00")
            End If

            If Not IsDBNull(dv("POD_STOCK_ON_HAND_QTY")) Then
                e.Item.Cells(EnumPOListStk.icStkOnHand).Text = Format(dv("POD_STOCK_ON_HAND_QTY"), "###0.00")
                dblStkOnHand = Format(dv("POD_STOCK_ON_HAND_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icStkOnHand).Text = Format(0, "###0.00")
                dblStkOnHand = 0
            End If

            If Not IsDBNull(dv("POD_PO_BALANCE_QTY")) Then
                e.Item.Cells(EnumPOListStk.icPOBalance).Text = Format(dv("POD_PO_BALANCE_QTY"), "###0.00")
                dblPOBalance = Format(dv("POD_PO_BALANCE_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPOBalance).Text = Format(0, "###0.00")
                dblPOBalance = 0
            End If

            If Not IsDBNull(dv("POD_PO_IN_PROGRESS_QTY")) Then
                e.Item.Cells(EnumPOListStk.icPOInProgress).Text = Format(dv("POD_PO_IN_PROGRESS_QTY"), "###0.00")
                dblPOInProgress = Format(dv("POD_PO_IN_PROGRESS_QTY"), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icPOInProgress).Text = Format(0, "###0.00")
                dblPOInProgress = 0
            End If

            If dblCurrMthQty = 0 Then
                e.Item.Cells(EnumPOListStk.icForeCast).Text = Format((dblStkOnHand + dblPOBalance + dblPOInProgress), "###0.00")
            Else
                e.Item.Cells(EnumPOListStk.icForeCast).Text = Format((dblStkOnHand + dblPOBalance + dblPOInProgress) / dblCurrMthQty, "###0.00")
            End If


            'dblStkOnHand = objpo_ext.get_StkOnHandByItem(e.Item.Cells(EnumPOListStk.icItemCode).Text, Session("side"), Request(Trim("BCoyID")))
            'e.Item.Cells(EnumPOListStk.icStkOnHand).Text = Format(dblStkOnHand, "###0.00")

            'dblPOBalance = objpo_ext.get_POBalanceByItem(e.Item.Cells(EnumPOListStk.icItemCode).Text, Session("side"), Request(Trim("BCoyID")))
            'e.Item.Cells(EnumPOListStk.icPOBalance).Text = Format(dblPOBalance, "###0.00")

            'dblPOInProgress = objpo_ext.get_POInProgressByItem(e.Item.Cells(EnumPOListStk.icItemCode).Text, Session("side"), Request(Trim("BCoyID")))
            'e.Item.Cells(EnumPOListStk.icPOInProgress).Text = Format(dblPOInProgress, "###0.00")

            If Not IsDBNull(dv("POD_LEAD_TIME")) Then
                e.Item.Cells(EnumPOListStk.icLeadTime).Text = dv("POD_LEAD_TIME")
            Else
                e.Item.Cells(EnumPOListStk.icLeadTime).Text = 0
            End If

            'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
            'Unit Price, Gst Rate, Gst Amt
            e.Item.Cells(EnumPOListStk.icUnitPrice).Text = Format(Common.parseNull(dv("POD_UNIT_COST"), 0), "#,##0.0000")
            e.Item.Cells(EnumPOListStk.icGstRate).Text = Common.parseNull(dv("GST_RATE"))
            e.Item.Cells(EnumPOListStk.icGstAmt).Text = Format(Common.parseNull(dv("POD_TAX_VALUE"), 0), "#,##0.00")
            'Stage 3 (Enhancement) (GST-0010) - 16/07/2015 - CH
            e.Item.Cells(EnumPOListStk.icGstTaxCode).Text = Common.parseNull(dv("POD_GST_INPUT_TAX_CODE"))
            'Delivery Term
            strDAddr = Common.parseNull(dv("POD_D_ADDR_LINE1"))
            If Not IsDBNull(dv("POD_D_ADDR_LINE2")) AndAlso dv("POD_D_ADDR_LINE2") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE2")
            End If
            If Not IsDBNull(dv("POD_D_ADDR_LINE3")) AndAlso dv("POD_D_ADDR_LINE3") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE3")
            End If
            If Not IsDBNull(dv("POD_D_POSTCODE")) Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_POSTCODE")
            End If
            If Not IsDBNull(dv("POD_D_CITY")) Then
                strDAddr = strDAddr & " " & dv("POD_D_CITY")
            End If
            If Not IsDBNull(dv("STATE")) AndAlso dv("STATE") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("STATE")
            End If
            If Not IsDBNull(dv("CT")) Then
                strDAddr = strDAddr & " " & dv("CT")
            End If

            e.Item.Cells(EnumPOListStk.icDelAddr).ToolTip = strDAddr
            e.Item.Cells(EnumPOListStk.icDelAddr).Text = Common.parseNull(dv("POD_D_ADDR_CODE"))
        End If
    End Sub
    Private Sub dtg_doc_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_doc.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt2")
        Grid_ItemCreated(dtg_doc, e)
    End Sub

    Private Sub dtg_doc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_doc.ItemDataBound
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim side As String
            Dim objpo As New PurchaseOrder
            If Session("side") = "v" Or Session("side") = "otherv" Then
                side = "v"
            Else
                side = "b"
            End If
            e.Item.Cells(EnumDoc.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("CREATIONDATE")) 'dv("CREATIONDATE")
            e.Item.Cells(EnumDoc.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SUBMITIONDATE"))
            'e.Item.Cells(3).Text = objpo

            If IsDBNull(dv("GM_CREATED_DATE")) Then
                e.Item.Cells(EnumDoc.icGRDDate).Text = "-"
            Else
                e.Item.Cells(EnumDoc.icGRDDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_CREATED_DATE"))
            End If

            If IsDBNull(dv("GM_DATE_RECEIVED")) Then
                e.Item.Cells(EnumDoc.icRecDate).Text = "-"
            Else
                e.Item.Cells(EnumDoc.icRecDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_DATE_RECEIVED"))
            End If


            If Common.parseNull(dv("GM_GRN_NO")) <> "" Then
                'e.Item.Cells(EnumDoc.icGRNNo).Text = "<A href=""../GRN/GRNDetails.aspx?Frm=PODetail&SubFrm=" & Request.QueryString("Frm") & "&pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("GM_GRN_NO")) & _
                '                   "&poidx=" & Session("po_index") & "&PO_NO=" & Request(Trim("PO_No")) & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "&side=" & side & " &BCoyID=" & Common.parseNull(dv("GM_B_COY_ID")) & """ ><font color=#0000ff>" & _
                '                   Common.parseNull(dv("GM_GRN_NO")) & "</font></A>"
                e.Item.Cells(EnumDoc.icGRNNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(dv("GM_GRN_NO")) & "&PONo=" & Request(Trim("PO_No")) & "&DONo=" & dv("DOM_DO_NO") & "&side=v&BCoyID=" & Common.parseNull(dv("GM_B_COY_ID"))) & "')""><font color=#0000ff>" &
                Common.parseNull(dv("GM_GRN_NO")) & "</font></A>"
            Else
                e.Item.Cells(EnumDoc.icGRNNo).Text = "-"
            End If

            If Common.parseNull(dv("DOM_DO_NO")) <> "" Then

                'If Session("side") = "b" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Then  'buyer                

                '-----New code adding for side=others by praveen on 25/07/2007 getting side="others" from Podetails.aspx
                'Michelle (CR0050) 
                ' If Session("side") = "b" Or Session("side") = "others" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Then
                If Session("side") = "b" Or Session("side") = "others" Or Session("side") = "u" Or (Session("side") = "other" And Request.QueryString("caller") = "buyer") Or (Session("side") = "other" And Request.QueryString("caller") = "") Then
                    '-----end the code by praveen   
                    If strCaller = "SEARCHPO_ALL" Or strCaller = "PRAPP" Then
                        e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer&poidx=" & Request("index") & "&pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"

                    Else
                        e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer&poidx=" & Session("po_index") & "&pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"

                    End If
                    'e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "&PO_NO=" & Me.lblPoNo.Text) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"


                ElseIf Session("side") = "otherv" Or Session("side") = "v" Or (Session("side") = "other" And Request.QueryString("caller") <> "buyer") Then 'vendor               
                    'e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"
                    e.Item.Cells(EnumDoc.icDONo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("DOM_S_COY_ID") & "&PO_NO=" & Session("lblPoNo")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DOM_DO_NO")) & "</font></A>"


                End If
            Else
                e.Item.Cells(EnumDoc.icDONo).Text = "-"
            End If

            If e.Item.Cells(EnumDoc.icGRNCreateBy).Text = "&nbsp;" Then
                e.Item.Cells(EnumDoc.icGRNCreateBy).Text = "-"
            End If

            If e.Item.Cells(EnumDoc.icCreatedBy).Text = "&nbsp;" Then
                e.Item.Cells(EnumDoc.icCreatedBy).Text = "-"
            End If

            ' "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("grn_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & " "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"
        End If
    End Sub
    Private Sub dtg_cr_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_cr.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt3")
        Grid_ItemCreated(dtg_cr, e)
    End Sub
    Private Sub dtg_apprflow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_apprflow.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt4")
        Grid_ItemCreated(dtg_apprflow, e)
    End Sub

    Private Sub dtg_cr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_cr.ItemDataBound
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            'e.Item.Cells(EnumCR.icCRNo).Text = "<A href=""CR_DETAIL.aspx?Frm=" & Request.QueryString("Frm") & "&pageid=" & strPageId & "&PO_NO=" & Request(Trim("po_no")) & "&side=" & Session("side") & "&BCoyId=" & Common.parseNull(dv("PCM_B_COY_ID")) & "&cr_no=" & Common.parseNull(dv("PCM_CR_NO")) & " "" ><font color=#0000ff>" & dv("PCM_CR_NO") & "</font></A>"
            e.Item.Cells(EnumCR.icCRNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "pageid=" & strPageId & "&PO_No=" & Request(Trim("po_no")) & "&BCoyID=" & Common.parseNull(dv("PCM_B_COY_ID")) & "&cr_no=" & Common.parseNull(dv("PCM_CR_NO"))) & "')"" ><font color=#0000ff>" & dv("PCM_CR_NO") & "</font></A>"
            e.Item.Cells(EnumCR.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("PCM_REQ_DATE")))
        End If
    End Sub

    'Private Sub addCmdCrAttribute(ByVal strSide As String)
    '    Me.cmd_cr.Attributes.Add("onclick", "PopWindow('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
    'End Sub

    'Private Sub addCmdPreviewAttribute(ByVal strSide As String)
    '    Me.cmd_preview.Attributes.Add("onclick", "PopWindow('POReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
    'End Sub

    Private Sub cmd_accept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_accept.Click

        Dim objpo As New PurchaseOrder_Vendor
        Dim ds As New DataSet
        Dim dtr As DataRow
        Dim POstatus As New DataTable
        Dim status As Integer = POStatus_new.Accepted
        Dim strMsg As String

        POstatus.Columns.Add("status", Type.GetType("System.String"))
        POstatus.Columns.Add("datakey", Type.GetType("System.String"))
        POstatus.Columns.Add("BCoyID", Type.GetType("System.String"))
        POstatus.Columns.Add("remark", Type.GetType("System.String"))
        dtr = POstatus.NewRow()
        dtr("status") = status
        dtr("datakey") = Request(Trim("PO_No"))
        dtr("BCoyID") = Request(Trim("BCoyID"))
        dtr("remark") = Me.txtRemark.Text
        POstatus.Rows.Add(dtr)
        ds.Tables.Add(POstatus)
        strMsg = objpo.update_POStatus(ds)
        cmd_reject.Visible = False
        cmd_accept.Visible = False
        If Request.QueryString("Frm") = "Dashboard" Then
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
            'Common.NetMsgboxTest(Me, strMsg, "", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub cmd_reject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_reject.Click

        ' Michelle (CR0010) - Force user to enter Remarks
        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Vendor's Remarks.", MsgBoxStyle.Information)
        Else
            Dim objpo As New PurchaseOrder_Vendor
            Dim ds As New DataSet
            Dim dtr As DataRow
            Dim POstatus As New DataTable
            Dim strMsg As String
            Dim status As Integer = POStatus_new.Rejected
            POstatus.Columns.Add("status", Type.GetType("System.String"))
            POstatus.Columns.Add("datakey", Type.GetType("System.String"))
            POstatus.Columns.Add("BCoyID", Type.GetType("System.String"))
            POstatus.Columns.Add("remark", Type.GetType("System.String"))
            dtr = POstatus.NewRow()
            dtr("status") = status
            dtr("datakey") = Request(Trim("PO_No"))
            dtr("BCoyID") = Request(Trim("BCoyID"))
            dtr("remark") = Me.txtRemark.Text
            POstatus.Rows.Add(dtr)
            ds.Tables.Add(POstatus)
            strMsg = objpo.update_POStatus(ds)
            cmd_reject.Visible = False
            cmd_accept.Visible = False
            If Request.QueryString("Frm") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
            End If
        End If

    End Sub
    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        Dim strurl As String = Session("strurl")
        Session("strurl") = ""
        Session("status_dis") = ""
        Response.Redirect(strurl)

    End Sub

    Public Sub cmd_next2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles detail.ServerClick

        If Session("filetype") = "1" Then
            Response.Redirect(dDispatcher.direct("PO", "CR_DETAIL.aspx", "side=" & Session("side") & "&po_no=" & Request(Trim("PO_No")) & "&BCoyID=" & Request(Trim("BCoyID")) & "&date=" & lblOrderDate.Text & "&cr_no=" & Session("cr_no") & "&pageid=" & strPageId & ""))
        ElseIf Session("filetype") = "2" Then
            'Dim strCaller As String
            If IsNothing(Request.QueryString("caller")) OrElse LCase(Request.QueryString("caller")) = "" Then
                strCaller = ""
            Else
                strCaller = Request.QueryString("caller")
            End If
            Response.Redirect(dDispatcher.direct("PO", "POLineDetail.aspx", "foce=1&side=" & Session("side") & "&po_no=" & Request(Trim("PO_No")) & "&prid=" & Request(Trim("PRNum")) & "&BCoyID=" & Request(Trim("BCoyID")) & "&date=" & lblOrderDate.Text & "&pageid=" & strPageId & "&caller=" & strCaller & "&poidx=" & Session("po_index") & ""))
        End If
    End Sub
    Public Function CreateFileLinks(ByVal userID As Object, ByVal altUserID As Object, ByVal seq As String) As DataTable

        Dim id1 As String = ""
        '1. if the pass in user is is not same as current login user id, return nothing
        If Not IsDBNull(userID) Then
            id1 = CStr(userID)
        End If

        Dim id2 As String = ""
        If Not IsDBNull(altUserID) Then
            id2 = CStr(altUserID)
        End If

        Dim pr As PR = New PR
        '2. get data about the attachment
        Dim ds As DataSet = pr.getUserAttach("AO", "PO", CStr(ViewState("POIndex")), id1, id2, seq)

        'If userID.Trim.ToLower <> CStr(ds.Tables(0).Columns("UA_USER_ID")).Trim.ToLower Then
        '	Return Nothing
        'End If


        '3. get the first table in the returned data set
        Dim dt As DataTable = ds.Tables(0)

        '4. create a datatable, and add a column into the table
        Dim table As DataTable = New DataTable
        Dim urlCol As DataColumn = New DataColumn("Hyperlink")
        table.Columns.Add(urlCol)

        '5. loop each rows of the dataset
        Dim fileMgr As New FileManagement
        Dim count As Integer = 1
        For Each row As DataRow In dt.Rows

            '6. generate the url that download the file
            Dim strFile As String = row.Item("UA_ATTACH_FILENAME")
            Dim strFile1 As String = row.Item("UA_HUB_FILENAME")
            Dim url As String = fileMgr.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)

            '7. create a row from the newly created table, and add the hyperlink string inside
            Dim r As DataRow = table.NewRow
            r.Item("Hyperlink") = CStr(count) + ") " + url
            table.Rows.Add(r)
            count = count + 1
        Next

        Return table
    End Function

    Private Sub cmd_ack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_ack.Click
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim objdb As New EAD.DBCom
        Dim objpo As New PurchaseOrder_Vendor
        Dim strsql As String
        Dim strarray(0) As String
        Dim cr_num1 As String
        Dim i As Integer
        Dim crdetail As New DataTable
        Dim ds As New DataSet
        Dim dtr1 As DataRow
        Dim CRSTATUS1 As Integer = CRStatus.Acknowledged



        crdetail.Columns.Add("cr_num", Type.GetType("System.String"))
        crdetail.Columns.Add("CRStatus", Type.GetType("System.String"))
        crdetail.Columns.Add("bcomid", Type.GetType("System.String"))
        crdetail.Columns.Add("po_no", Type.GetType("System.String"))
        cr_num1 = Session("cr_no").ToString.Trim
        dtr1 = crdetail.NewRow()
        dtr1("cr_num") = cr_num1
        dtr1("CRStatus") = CRSTATUS1
        dtr1("bcomid") = Request(Trim("BCoyID"))
        dtr1("po_no") = Request(Trim("PO_No"))
        crdetail.Rows.Add(dtr1)
        ds.Tables.Add(crdetail)
        strsql = objpo.update_ack(ds)
        cmd_ack.Visible = False
        'Common.NetMsgbox(Me, strsql, "ViewCancel.aspx?filetype=1&side=v&pageid=" & strPageId & "", MsgBoxStyle.Information, "Wheel")
        If InStr(strsql, "Error occurs") > 0 Then
            Common.NetMsgbox(Me, strsql, MsgBoxStyle.Information)
        Else
            Dim strmsg As String = "CR Number " & cr_num1 & " has been acknowledged."
            If Request.QueryString("Frm") = "Dashboard" Then
                Common.NetMsgbox(Me, strmsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, strmsg, dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Sub setControlProp(ByVal strType As String)
        If strType = "2" Then         'PO Detail             
            'Me.lblTitle.Text = "Purchase Order Details"
            Me.lblHeader1.Text = "Purchase Order Header"
            'Me.lbltitle1.Text = "PO Line Detail"
            Me.trdiv.Visible = False
            Me.cmd_accept.Visible = False
            Me.cmd_ack.Visible = False
            'Me.cmd_cr.Visible = False
            Me.cmd_cr.Style("VISIBILITY") = "hidden"
            Me.cmd_reject.Visible = False
            Me.cmd_preview.Visible = True


        ElseIf strType = "1" Then 'PO CR Detail
            Me.hidCR.Style("display") = "inline"
            Me.hidDO.Style("display") = "none"
            Me.hidApprflow.Style("display") = "none"
            Me.cmd_ack.Visible = True
            Session("ack") = Request(Trim("ack"))
            If Session("ack") = 2 Then
                cmd_ack.Visible = False
            End If
            'Me.lblTitle.Text = "Cancellation Request Details"
            Me.lblHeader1.Text = "Cancellation Request Details"
            'Me.lbltitle1.Text = "CR Line Detail"
            Me.trdiv.Style("display") = "inline"
            Me.lblCRNum.Text = Request(Trim("cr_no"))
            Me.cmd_accept.Visible = False
            Me.cmd_reject.Visible = False
            'Me.cmd_cr.Visible = True
            Me.cmd_cr.Style("VISIBILITY") = "Visible"
            Me.cmd_preview.Visible = False
        End If
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Select Case Session("side")
            '    Case "v"
            '        If strCaller = "POVIEWB" Or Request.QueryString("Frm") = "Dashboard" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                             "<li><a class=""t_entity_btn_selected"" href=""POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                             "<li><a class=""t_entity_btn"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                             "</ul><div></div></div>"
            '        Else
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                 "<li><a class=""t_entity_btn"" href=""POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                 "<li><a class=""t_entity_btn_selected"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                 "</ul><div></div></div>"

            '        End If
            '    Case "b"
            '        If strCaller = "SEARCHPO_ALL" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""SearchPO_AO.aspx?pageid=" & strPageId & """><span>Approval List</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn_selected"" href=""SearchPO_ALL.aspx?pageid=" & strPageId & """><span>Approved / Rejected Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "</ul><div></div></div>"
            '        ElseIf strCaller = "POVIEWB2" Or Request.QueryString("Frm") = "Dashboard" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '             "<li><a class=""t_entity_btn_selected"" href=""POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '             "<li><a class=""t_entity_btn"" href=""POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "</ul><div></div></div>"
            '        End If

            '    Case "other"
            '        If Request.QueryString("Frm") = "GRNSearch" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                       "<li><a class=""t_entity_btn"" href=""../GRN/GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                       "<li><a class=""t_entity_btn_selected"" href=""../GRN/GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                       "</ul><div></div></div>"
            'ElseIf Request.QueryString("Frm") = "InvoiceVerified" Then
            '   Session("w_PODetail_tabs") = "<div class=""t_entity"">" & _
            '           "<a class=""t_entity_btn_selected"" href=""InvoiceVerified.aspx?pageid=" & strPageId & """><span>Verified Invoice</span></a>" & _
            '           "<a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a>" & _
            '                               "</div>"
            '        ElseIf Request.QueryString("Frm") = "InvoiceTrackingList" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "</ul><div></div></div>"

            '        ElseIf Request.QueryString("Frm") = "InvoiceVerifiedTrackingList" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "</ul><div></div></div>"

            '        ElseIf Request.QueryString("Frm") = "InvoicePaidTrackingList" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                        "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                       "</ul><div></div></div>"
            '        End If

            '    Case "otherv"
            '        If Request.QueryString("Frm") = "InvList" Then
            '            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                    "<li><a class=""t_entity_btn_selected"" href=""../Invoice/InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                    "<li><a class=""t_entity_btn"" href=""../Invoice/invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                    "</ul><div></div></div>"
            '        End If
            Case "v"
                If strCaller = "POVIEWB" Or Request.QueryString("Frm") = "Dashboard" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "</ul><div></div></div>"
                ElseIf Request.QueryString("Frm") = "InvList" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""../Invoice/InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""../Invoice/invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
                Else
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                         "<li><div class=""space""></div></li>" & _
                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                         "<li><div class=""space""></div></li>" & _
                         "</ul><div></div></div>"

                End If
            Case "b"
                If strPageId = Nothing Then strPageId = 7
                If strCaller = "SEARCHPO_ALL" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "SearchPO_ALL.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "</ul><div></div></div>"
                ElseIf strCaller = "POVIEWB2" Or Request.QueryString("Frm") = "Dashboard" Then
                    'Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    '"<li><div class=""space""></div></li>" & _
                    ' "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                    ' "<li><div class=""space""></div></li>" & _
                    ' "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                    ' "<li><div class=""space""></div></li>" & _
                    '            "</ul><div></div></div>"
                    Dim _role As New UserRoles
                    Dim showFFPO = objDB.GetVal("SELECT CM_FFPO_CONTROL FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")
                    Dim _compType = objDB.GetVal("SELECT CM_COY_TYPE FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")

                    Dim _userRole = _role.get_UserFixedRole()
                    If Session("POType") = "AllPO" Then
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "</ul><div></div></div>"
                    Else
                        If showFFPO.ToString.ToUpper = "Y" And (_userRole.ToString.ToUpper.Contains("PURCHASING MANAGER") Or _userRole.ToString.ToUpper.Contains("PURCHASING OFFICER")) And _compType.ToString.ToUpper = "BUYER" Then
                            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "</ul><div></div></div>"
                        Else
                            Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                                       "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
                        End If
                    End If
                    
                ElseIf Request.QueryString("Frm") = "ConvertPRList" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "ConvertPR.aspx", "pageid=" & strPageId) & """><span>Convert PR</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "ConvertPRListing.aspx", "pageid=" & strPageId) & """><span>Convert PR Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation_ByPO.aspx", "pageid=" & strPageId) & """><span>Cancelled PR</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
                ElseIf strCaller = "PRALL" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=new&mode=bc&frm=bc") & """><span>Purchase Request</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId) & """><span>Purchase Request Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId) & """><span>Purchase Request Cancellation</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
                ElseIf strCaller = "PRAPPALL" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchApp_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
                Else
                    Session("w_PODetail_tabs") = ""
                End If

            Case "other"
                If Request.QueryString("Frm") = "GRNSearch" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNList.aspx", "pageid=" & strPageId) & """><span>Issue GRN</span></a></li>" & _
                                               "<li><div class=""space""></div></li>" & _
                                               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId) & """><span>GRN Listing</span></a></li>" & _
                                               "<li><div class=""space""></div></li>" & _
                                               "</ul><div></div></div>"

                ElseIf Request.QueryString("Frm") = "ROListing" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"

                ElseIf Request.QueryString("Frm") = "InvoiceVerified" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                                                "</ul><div></div></div>"

                ElseIf Request.QueryString("Frm") = "InvoiceTrackingList" Then
                    If Request.QueryString("folder") = "N" Then
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                       "</ul><div></div></div>"
                    ElseIf Request.QueryString("folder") = "S" Then
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                               "</ul><div></div></div>"
                    Else
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                                                        "<li><div class=""space""></div></li>" & _
                                                                       "</ul><div></div></div>"
                    End If


                ElseIf Request.QueryString("Frm") = "InvoiceVerifiedTrackingList" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=N" & "&status=1") & """><span>New Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "</ul><div></div></div>"

                ElseIf Request.QueryString("Frm") = "InvoicePaidTrackingList" Then
                    If Request.QueryString("role") = "3" Then 'ie. FM (with only 2 tabs)
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                                    "<li><div class=""space""></div></li>" & _
                                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Payment Invoice</span></a></li>" & _
                                                    "<li><div class=""space""></div></li>" & _
                                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                                    "<li><div class=""space""></div></li>" & _
                                                  "</ul><div></div></div>"
                    Else
                        Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                               "<li><div class=""space""></div></li>" & _
                                               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                               "<li><div class=""space""></div></li>" & _
                                               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                               "<li><div class=""space""></div></li>" & _
                                              "</ul><div></div></div>"


                    End If
                End If

            Case "otherv"
                If Request.QueryString("Frm") = "InvList" Then
                    Session("w_PODetail_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
                End If

        End Select
    End Sub

    Private Sub dtg_apprflow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_apprflow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim intTotalCell, intLoop As Integer
            If dv("PRA_Seq") - 1 = dv("PRA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next
            End If

            If Not IsDBNull(dv("PRA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRA_ACTION_DATE"))
            End If
            If dv("PRA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            Else
                e.Item.Cells(3).Text = "Endorsement"
            End If

            If UCase(Common.parseNull(dv("PRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("PRA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("PRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("PRA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If
        End If

    End Sub

    Private Sub PreviewPO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3,  " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                            & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_DETAILS.POD_COY_ID, " _
                            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, " _
                            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, " _
                            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, " _
                            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, " _
                            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, " _
                            & "USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, " _
                            & "USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, " _
                            & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, " _
                            & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED, " _
                            & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT, " _
                            & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER, " _
                            & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY, " _
                            & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                            & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                            & "USER_MSTR.UM_INVOICE_MASS_APP, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                            & "(SELECT   CM_EMAIL " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                            & "(SELECT   CM_PHONE " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, PO_MSTR.POM_SHIP_AMT " _
                            & "FROM      PO_MSTR INNER JOIN " _
                            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND " _
                            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request(Trim("BCoyID"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request(Trim("PO_No"))))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPO_DataSetPreviewPO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewPO-FTN.rdlc"
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

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"\
            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String
            strFileName = "POReport.PDF"
            Dim fs As New FileStream(appPath & "PO\POReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Response.ContentType = "application/x-download"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
            Response.WriteFile(Server.MapPath(strFileName))
            Response.End()
            'Dim strJScript As String
            'strJScript = "<script language=javascript>"
            'strJScript += "window.open('../PO/POReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            'strJScript += "</script>"
            'Response.Write(strJScript)


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

    'Protected Sub cmd_preview_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_preview.ServerClick
    '    PreviewPO()
    'End Sub

    Private Sub PreviewCR()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX,  " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN,  " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2,  " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS,  " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON,  " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE,  " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX,  " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND,  " _
                            & "PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_PRINT_REMARK, PO_DETAILS.POD_COY_ID, " _
                            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE,  " _
                            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY,  " _
                            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS,  " _
                            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST,  " _
                            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX,  " _
                            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2,  " _
                            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE,  " _
                            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO,  " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE,  " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM,  " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION,  " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS,  " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT,  " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE,  " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING,  " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY,  " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT,  " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, PO_CR_MSTR.PCM_CR_NO, PO_CR_MSTR.PCM_B_COY_ID,  " _
                            & "PO_CR_MSTR.PCM_S_COY_ID, PO_CR_MSTR.PCM_PO_INDEX, PO_CR_MSTR.PCM_CR_STATUS,  " _
                            & "PO_CR_MSTR.PCM_REQ_BY, PO_CR_MSTR.PCM_REQ_DATE, PO_CR_MSTR.PCM_CR_REMARKS,  " _
                            & "PO_CR_DETAILS.PCD_CR_NO, PO_CR_DETAILS.PCD_COY_ID, PO_CR_DETAILS.PCD_PO_LINE,  " _
                            & "PO_CR_DETAILS.PCD_CANCELLED_QTY, PO_CR_DETAILS.PCD_REMARKS, USER_MSTR.UM_AUTO_NO,  " _
                            & "USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD,  " _
                            & "USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID,  " _
                            & "USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, USER_MSTR.UM_DESIGNATION,  " _
                            & "USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, USER_MSTR.UM_USER_SUSPEND_IND,  " _
                            & "USER_MSTR.UM_PASSWORD_LAST_CHANGED, USER_MSTR.UM_NEW_PASSWORD_IND,  " _
                            & "USER_MSTR.UM_NEXT_EXPIRE_DT, USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION,  " _
                            & "USER_MSTR.UM_ANSWER, USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS,  " _
                            & "USER_MSTR.UM_MOD_BY, USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY,  " _
                            & "USER_MSTR.UM_ENT_DATE, USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC,  " _
                            & "USER_MSTR.UM_INVOICE_APP_LIMIT, USER_MSTR.UM_INVOICE_MASS_APP, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                            & "(SELECT   CM_EMAIL " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                            & "(SELECT   CM_PHONE " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, " _
                            & "(SELECT   um_User_name FROM User_MSTR AS B WHERE   (PCM_REQ_By = UM_User_ID AND PCM_B_COY_ID = UM_coy_ID)) AS PCMCRBUYERNAME " _
                            & "FROM      PO_MSTR INNER JOIN " _
                            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "PO_CR_MSTR ON PO_MSTR.POM_PO_INDEX = PO_CR_MSTR.PCM_PO_INDEX AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_CR_MSTR.PCM_B_COY_ID INNER JOIN " _
                            & "PO_CR_DETAILS ON PO_CR_MSTR.PCM_CR_NO = PO_CR_DETAILS.PCD_CR_NO AND  " _
                            & "PO_CR_MSTR.PCM_B_COY_ID = PO_CR_DETAILS.PCD_COY_ID AND  " _
                            & "PO_DETAILS.POD_PO_LINE = PO_CR_DETAILS.PCD_PO_LINE INNER JOIN " _
                            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND  " _
                            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo) AND  " _
                            & "(PO_CR_MSTR.PCM_CR_NO = @prmCRNo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request(Trim("BCoyID"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request(Trim("PO_No"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCRNo", Request(Trim("cr_no"))))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewCR_FTN_DataTable1", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewCR-FTN.rdlc"
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

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\CRReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('CRReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Public Function GenerateGLString(ByVal glcode As Object, ByVal gldesc As Object) As String
        Dim strGLCode As String = String.Empty
        Dim strDesc As String = String.Empty

        If Not IsDBNull(glcode) Then
            strGLCode = CStr(glcode)
        End If

        If Not IsDBNull(gldesc) Then
            strDesc = CStr(gldesc)
        End If

        If strGLCode.Equals("") Then
            Return ""
        Else
            'Jules 2018.11.05 - Swapped GL Code display: "GL Description (GL Code)
            'Return " (" & strGLCode & ") " & strDesc                'default return value
            Return strDesc & " (" & strGLCode & ")"                'default return value
        End If

    End Function

    'Stage 3 (Enhancement) (GST-0018) - 09/07/2015 - CH
    'Function to duplicate selected PO number
    Private Sub cmd_dupPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_dupPO.Click
        Dim objPO_Buyer As New PurchaseOrder_Buyer
        Dim strMsg As String

        strMsg = objPO_Buyer.DuplicatePO(Request(Trim("PO_No")), Session("po_index"))
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)

    End Sub

    'Protected Sub cmd_cr_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_cr.ServerClick
    '    PreviewCR()
    'End Sub
End Class
