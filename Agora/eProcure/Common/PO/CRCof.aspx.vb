Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class CRCof
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
    Protected WithEvents cmd_cr As System.Web.UI.WebControls.Button

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
        ' strType = Request.QueryString("strType")
        Dim po_no As String
        '-------New Changes for LineNo In PO_CRREPORT by PRaveen on 18/07/2007
        'Dim lineval As String
        Dim objPO As New PurchaseOrder
        Dim objval As New POValue
        objval.linevalue(0) = Request(Trim("linevalue(0)"))
        '--------End

        po_no = Request.QueryString("PO_No")
        lblTitle.Text = "PO Cancellation"
        Dim i As Integer
        Dim objPR As New PR
        Dim dsItem As New DataSet
        lbl_Rfq_num.Text = "PO Number : <b>" & po_no & "</b> has been successfully cancelled"
        lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2.aspx", "filetype=1&side=b&pageid=" & strPageId)
        Me.cmd_cr.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "PO_No=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")

        '-------New Changes for LineNo In PO_CRREPORT by PRaveen on 18/07/2007
        'Me.cmd_cr.Attributes.Add("onclick", "window.open('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & "')")
        'Me.cmd_cr.Attributes.Add("onclick", "window.open('PO_CRReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&cr_no=" & Request(Trim("cr_no")) & "&linevalue(0)=" & Request(Trim("linevalue(0)")) & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & "')")
        '-----End
        '//********carol add in this**********
        trHeader.Visible = True
        trItem.Visible = True
    End Sub
    '"POViewB2.aspx?filetype=1&side=b&pageid=" & strPageId)

    'Private Sub cmd_cr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_cr.Click
    '    PreviewCR()
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
End Class
