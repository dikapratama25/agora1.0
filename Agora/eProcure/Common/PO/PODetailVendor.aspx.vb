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

Public Class PODetailVendor
    Inherits AgoraLegacy.AppBaseClass
    Dim objFile As New FileManagement
    Dim PO_STATUS As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblContact As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblExcRate As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrderDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPoNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblPOType As System.Web.UI.WebControls.Label
    Protected WithEvents lblCRNum As System.Web.UI.WebControls.Label
    Protected WithEvents lblAssoPRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblshipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipVia As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFileAttac As System.Web.UI.WebControls.Label
    Protected WithEvents term_con As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents link_term As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents detail As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lbltitle1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_accept As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_reject As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_preview As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmd_ack As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cr As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table3 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

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
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        MyBase.Page_Load(sender, e)
        strCaller = UCase(Request.QueryString("caller"))
        Dim objpo As New PurchaseOrder
        Dim objpo1 As New PurchaseOrder_Vendor
        Dim objval As New POValue

        If Not IsPostBack Then
            GenerateTab()
            Session("backtodetail") = strCallFrom
            If Session("side") = "" Then
                Session("side") = Request(Trim("side"))
            Else
                If Request(Trim("side")) <> "" Then
                    Session("side") = Request(Trim("side"))
                End If
            End If
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
                Session("PO_STATUS1") = Request(Trim("status"))
                Session("status_dis") = objpo.get_status(Session("PO_STATUS1"))
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

                    'If Session("filetype") = "1" Then
                    '    If Request(Trim("cr_no")) <> "" Then
                    '        cmd_ack.Visible = False
                    '        Me.addCmdCrAttribute("b")
                    '    End If
                    '    Me.addCmdCrAttribute("b")
                    'End If
                    'Case "b"
                    '    Me.addCmdPreviewAttribute("b")
                    '    Me.addCmdCrAttribute("b")
                    '    setControlProp(Session("filetype"))
                    '    Session("strurl") = "POViewB2.aspx?pageid=" & strPageId & "&filetype=" & Session("filetype") & "&side=b"

                Case "u"
                    If Session("filetype") = "2" Then
                        'Me.addCmdPreviewAttribute("b")
                        'Me.addCmdCrAttribute("b")
                        setControlProp(Session("filetype"))
                        Session("strurl") = dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & "&filetype=2&side=u")
                    End If

                Case "v"
                    'Me.addCmdPreviewAttribute("v")
                    'Me.addCmdCrAttribute("v")

                    If Session("filetype") = "2" Then
                        Dim ds As New DataSet
                        Dim dtr As DataRow
                        Dim POstatus As New DataTable
                        POstatus.Columns.Add("status", Type.GetType("System.String"))
                        POstatus.Columns.Add("datakey", Type.GetType("System.String"))
                        POstatus.Columns.Add("BCoyID", Type.GetType("System.String"))
                        POstatus.Columns.Add("remark", Type.GetType("System.String"))
                        Session("strurl") = dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & " ")
                        Me.lblTitle.Text = "Purchase Order Details"
                        Me.lblHeader1.Text = "Purchase Order Details"
                        Me.lbltitle1.Text = "PO Line Detail"
                        Me.Table3.Rows(0).Visible = False
                        'Dim PO_STATUS As Integer = Convert.ToDecimal(Session("PO_STATUS1"))

                        'Dim PO_STATUS As Integer = objpo.get_po_StatusNo(Request(Trim("PO_INDEX")))
                        '------New Code Added To Make CMD Reject,AcceptPo Visisble="True" whlie login by userid=Carol By Praveen  on 08.08.2007
                        Dim PO_STATUS As Integer = objpo1.get_po_StatusNo(Session("po_index"))
                        '---------End
                        If PO_STATUS = POStatus_new.Open Or PO_STATUS = POStatus_new.NewPO Then
                            Me.cmd_accept.Visible = True
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
                        setControlProp(Session("filetype"))
                        Session("strurl") = dDispatcher.direct("PO", "ViewCancel.aspx", "pageid=" & strPageId & "&filetype=1&side=v")
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

            'Michelle (23/10/2007) - To cater for mutliple POs
            'dt = objTRa.getRelatedPR(Session("po_index"))
            dt = objTRa.getRelatedPR_PO(Request(Trim("PO_No")), Request(Trim("BCoyID")))

            For i = 0 To dt.Rows.Count - 1
                If strPR = "" Then
                    strPR = dt.Rows(i)("PRM_PR_NO")
                Else
                    strPR = strPR & "," & dt.Rows(i)("PRM_PR_NO")
                End If
            Next
            objTRa = Nothing
            lblAssoPRNo.Text = strPR
            objval.PO_Number = Request(Trim("PO_No"))
            objval.buyer_coy = Request(Trim("BCoyID"))
            ' objval.PO_Status = Request(Trim("status"))
            objpo.get_PODetail(objval, Session("side"), False)
            If Session("status_dis") = "" Or IsNothing(Session("status_dis")) Then
                Session("status_dis") = objpo.get_postatus(Session("po_index"))
            End If

            Me.lblStatus.Text = Session("status_dis")
            Session("status_dis") = ""
            Me.lblPoNo.Text = Request(Trim("PO_No"))
            Me.lblContact.Text = objval.buyer_contact
            Me.lblCurrCode.Text = objval.cur
            Me.lblEmail.Text = objval.buyer_email
            Me.lblExcRate.Text = objval.ex_rate
            Me.lblFax.Text = objval.buyer_fax
            'Me.lblFileAttac.Text = objval.buyer_fa
            ' Me.lblGST.Text = objval.tax
            ' Me.lblGSTCode.Text = objval.gst_code
            Me.lblOrderDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.PO_Date)
            Me.lblPaymentMethod.Text = objval.pay_meth
            Me.lblPaymentTerm.Text = objval.pay_term
            '  Me.lblPOType.Text = objval.PO_type
            Me.lblShipMethod.Text = objval.ship_meth
            Me.lblshipTerm.Text = objval.ship_term
            Me.lblShipTo.Text = Replace(objpo.get_delivery_add(Request(Trim("BCoyID")), Request(Trim("PO_No"))), "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;" 'Replace(objval.buyer_adds, "<BR>", "<BR>&nbsp;")
            Me.lblTel.Text = objval.buyer_tel
            Me.lblVendor.Text = objval.vendor_Coy
            Me.lblVendorAddr.Text = Replace(objval.vendor_adds, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"
            'Me.lblVendorContact.Text = objval.vendor_contact
            Me.lblVendorEmail.Text = objval.vendor_email
            Me.lblVendorFax.Text = objval.vendor_fax
            Me.lblVendorTel.Text = objval.vendor_tel
            Me.txtRemark.Text = objval.Vendor_remark
            Me.lblPOType.Text = objval.PO_type
            Me.lblEmail.Text = objval.buyer_email
            Me.lblShipVia.Text = objval.ship_via
            Dim dvFile As DataView
            Dim intLoop As Integer
            Dim strFile, strFile1, strURL, strTemp As String
            dvFile = objpo.getPoAttachment(Request(Trim("PO_No")), Request(Trim("BCoyID"))).Tables(0).DefaultView
            If dvFile.Count > 0 Then
                For intLoop = 0 To dvFile.Count - 1
                    strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")
                    '*************************meilai 25/2/05****************************
                    'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PO>" & strFile & "</A>"
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PO", EnumUploadFrom.FrontOff)
                    '*************************meilai************************************
                    If strTemp = "" Then
                        strTemp = "&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                    Else
                        strTemp = strTemp & "<BR>&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                    End If
                Next
            Else
                strTemp = "No Files Attached"
            End If
            lblFileAttac.Text = strTemp
            '//add by Moo, for Term and Condition File Download

            '*************************meilai 25/2/05****************************
            'Me.link_term.HRef = "../FileDownload.aspx?file=" & Server.UrlEncode(objval.TermAndCond) & "&actualfile=" & Server.UrlEncode(objval.TermAndCond) & "&type=" & EnumDownLoadType.TermAndCond & "&doctype="
            'Me.link_term.HRef = objFile.getAttachPath(Server.UrlEncode(objval.TermAndCond), objval.TermAndCond, Server.UrlEncode(objval.TermAndCond), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff)
            Me.link_term.InnerHtml = objFile.getAttachPath(Server.UrlEncode(objval.TermAndCond), "Click Here", Server.UrlEncode(objval.TermAndCond), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff)
            '*************************meilai************************************

        End If
        hidSummary.Value = "Remarks-" & txtRemark.ClientID
        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmd_accept.Attributes.Add("onClick", "return resetSummary(1,0);")
        cmd_reject.Attributes.Add("onClick", "return confirmReject();")
        'lnkBack.NavigateUrl = "javascript:history.back();"
        Me.cmd_preview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_No=" & Request(Trim("PO_No")) & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")
        Me.cmd_cr.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "PO_No=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")

        'Me.cmd_cr.Attributes.Add("onclick", "PopWindow('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
    End Sub

    'Private Sub addCmdCrAttribute(ByVal strSide As String)
    '    Me.cmd_cr.Attributes.Add("onclick", "PopWindow('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
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
                            & "(SELECT CODE_DESC " _
                            & "FROM CODE_MSTR AS a " _
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
            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
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

    'Private Sub addCmdPreviewAttribute(ByVal strSide As String)
    '    Me.cmd_preview.Attributes.Add("onclick", "PopWindow('POReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")
    '    'Me.cmd_preview.Attributes.Add("onclick", "PopWindow('PreviewPO.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&side=" & strSide & "&BCoyID=" & Request(Trim("BCoyID")) & "')")

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
        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)
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
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId & ""), MsgBoxStyle.Information)

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
        'Common.NetMsgbox(Me, strsql, "ViewCancel.aspx?filetype=1&side=v&pageid=" & strPageId & "", MsgBoxStyle.Information, "Wheel")
        If InStr(strsql, "Error occurs") > 0 Then
            Common.NetMsgbox(Me, strsql, MsgBoxStyle.Information)
        Else
            Response.Redirect(dDispatcher.direct("PO", "errorpage.aspx", "action=ack&item=" & Server.UrlEncode(strsql) & "&pageid=" & strPageId))
        End If
    End Sub

    Sub setControlProp(ByVal strType As String)
        If strType = "2" Then         'PO Detail             
            Me.lblTitle.Text = "Purchase Order Details"
            Me.lblHeader1.Text = "Purchase Order Details"
            Me.lbltitle1.Text = "PO Line Detail"
            Me.Table3.Rows(0).Visible = False
            Me.cmd_accept.Visible = False
            Me.cmd_ack.Visible = False
            'Me.cmd_cr.Visible = False
            Me.cmd_cr.Style("VISIBILITY") = "hidden"
            Me.cmd_reject.Visible = False
            Me.cmd_preview.Visible = True
        ElseIf strType = "1" Then 'PO CR Detail
            Me.cmd_ack.Visible = True
            Session("ack") = Request(Trim("ack"))
            If Session("ack") = 2 Then
                cmd_ack.Visible = False
            End If
            Me.lblTitle.Text = "Cancellation Request Details"
            Me.lblHeader1.Text = "Cancellation Request Details"
            Me.lbltitle1.Text = "CR Line Detail"
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
        'Session("w_VendorPODetail_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""PODetailVendor.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '               "</ul><div></div></div>"
        Session("w_VendorPODetail_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "PODetailVendor.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                      "</ul><div></div></div>"
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
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\POReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('POReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    'Protected Sub cmd_preview_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_preview.ServerClick
    '    PreviewPO()
    'End Sub

    'Private Sub cmd_cr_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_cr.ServerClick
    '    PreviewCR()
    'End Sub
End Class
