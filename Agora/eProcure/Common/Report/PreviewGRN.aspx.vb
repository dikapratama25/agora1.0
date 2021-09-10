Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewGRN1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objGRN_Ext As New GRN_Ext
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewGRN()
    End Sub

    Private Sub PreviewGRN()
        Dim ds As New DataSet
        Dim blnSEH As Boolean = False
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim j As Integer
        Dim TotalLandCost As Decimal = 0

        If objDb.Exist("SELECT '*' FROM PO_MSTR WHERE POM_B_COY_ID = '" & Request.QueryString("BCoyID") & "' AND POM_PO_NO='" & Request.QueryString("PONo") & "' AND (POM_DEL_CODE <> '' AND POM_DEL_CODE IS NOT NULL)") Then
            blnSEH = True
        End If

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

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
                             & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                             & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                             & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                             & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                             & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                             & "PO_DETAILS.POD_COY_ID, PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, " _
                             & "PO_DETAILS.POD_PRODUCT_CODE, PO_DETAILS.POD_VENDOR_ITEM_CODE, " _
                             & "PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, PO_DETAILS.POD_ORDERED_QTY, " _
                             & "PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, PO_DETAILS.POD_DELIVERED_QTY, " _
                             & "PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, PO_DETAILS.POD_MIN_ORDER_QTY, " _
                             & "PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, PO_DETAILS.POD_UNIT_COST, " _
                             & "PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
                             & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
                             & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, PO_DETAILS.POD_D_ADDR_CODE, " _
                             & "PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, PO_DETAILS.POD_D_ADDR_LINE3, " _
                             & "PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, PO_DETAILS.POD_D_STATE, " _
                             & "PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, " _
                             & "PO_DETAILS.POD_SPEC1, PO_DETAILS.POD_SPEC2, PO_DETAILS.POD_SPEC3, " _
                             & "DO_MSTR.DOM_DO_INDEX, DO_MSTR.DOM_DO_NO, DO_MSTR.DOM_S_COY_ID, DO_MSTR.DOM_DO_DATE, " _
                             & "DO_MSTR.DOM_S_REF_NO, DO_MSTR.DOM_S_REF_DATE, DO_MSTR.DOM_PO_INDEX, " _
                             & "DO_MSTR.DOM_WAYBILL_NO, DO_MSTR.DOM_FREIGHT_CARRIER, DO_MSTR.DOM_FREIGHT_AMT, " _
                             & "DO_MSTR.DOM_DO_REMARKS, DO_MSTR.DOM_DO_STATUS, DO_MSTR.DOM_CREATED_DATE, " _
                             & "DO_MSTR.DOM_CREATED_BY, DO_MSTR.DOM_NOOFCOPY_PRINTED, DO_MSTR.DOM_DO_PREFIX, " _
                             & "DO_MSTR.DOM_D_ADDR_CODE, DO_MSTR.DOM_D_ADDR_LINE1, DO_MSTR.DOM_D_ADDR_LINE2, " _
                             & "DO_MSTR.DOM_D_ADDR_LINE3, DO_MSTR.DOM_D_POSTCODE, DO_MSTR.DOM_D_CITY, " _
                             & "DO_MSTR.DOM_D_STATE, DO_MSTR.DOM_D_COUNTRY, DO_MSTR.DOM_EXTERNAL_IND, " _
                             & "DO_MSTR.DOM_REFERENCE_NO, DO_DETAILS.DOD_S_COY_ID, DO_DETAILS.DOD_DO_NO, " _
                             & "DO_DETAILS.DOD_DO_LINE, DO_DETAILS.DOD_PO_LINE, DO_DETAILS.DOD_DO_QTY, " _
                             & "DO_DETAILS.DOD_SHIPPED_QTY, DO_DETAILS.DOD_REMARKS, GRN_MSTR.GM_GRN_INDEX, " _
                             & "GRN_MSTR.GM_GRN_NO, GRN_MSTR.GM_B_COY_ID, GRN_MSTR.GM_PO_INDEX, " _
                             & "GRN_MSTR.GM_DATE_RECEIVED, GRN_MSTR.GM_NOOFCOPY_PRINTED, GRN_MSTR.GM_DO_INDEX, " _
                             & "GRN_MSTR.GM_INVOICE_NO, GRN_MSTR.GM_GRN_PREFIX, GRN_MSTR.GM_S_COY_ID,  " _
                             & "GRN_MSTR.GM_GRN_STATUS, GRN_MSTR.GM_DOWNLOADED_DATE, GRN_MSTR.GM_GRN_LEVEL, " _
                             & "GRN_MSTR.GM_LEVEL2_USER, GRN_MSTR.GM_CREATED_BY, GRN_MSTR.GM_CREATED_DATE,  " _
                             & "GRN_DETAILS.GD_B_COY_ID, GRN_DETAILS.GD_GRN_NO, GRN_DETAILS.GD_PO_LINE,  " _
                             & "GRN_DETAILS.GD_RECEIVED_QTY, GRN_DETAILS.GD_REJECTED_QTY, GRN_DETAILS.GD_REMARKS,  " _
                             & "GRN_DETAILS.GD_OTH_CHARGE, GRN_DETAILS.GD_INLAND_CHARGE, GRN_DETAILS.GD_DUTIES, " _
                             & "GRN_DETAILS.GD_FACTOR, GRN_DETAILS.GD_EXCHANGE_RATE, " _
                             & "USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED,  " _
                             & "USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID,  " _
                             & "USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT,  " _
                             & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO,  " _
                             & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED,  " _
                             & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT,  " _
                             & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER,  " _
                             & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY,  " _
                             & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                             & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                             & "USER_MSTR.UM_INVOICE_MASS_APP, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
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
                             & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
                             & "COMPANY_MSTR.CM_BA_CANCEL, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = DO_MSTR.DOM_D_COUNTRY)) AS DelvAddrState, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM  CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                             & "AS DelvAddrCtry, " _
                             & "0.00 AS DiscAmt, 0.00 AS UnitCost, 0.00 AS AmtF, 0.00 AS AmtM, 0.00 AS GRNFactor, 0.00 AS CIFValues, 0.00 AS LandedCost " _
                             & "FROM PO_MSTR INNER JOIN " _
                             & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND " _
                             & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                             & "DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN " _
                             & "DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND  " _
                             & "DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND  " _
                             & "PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN " _
                             & "GRN_MSTR ON PO_MSTR.POM_PO_INDEX = GRN_MSTR.GM_PO_INDEX AND  " _
                             & "DO_MSTR.DOM_DO_INDEX = GRN_MSTR.GM_DO_INDEX INNER JOIN " _
                             & "GRN_DETAILS ON GRN_MSTR.GM_GRN_NO = GRN_DETAILS.GD_GRN_NO AND  " _
                             & "GRN_MSTR.GM_B_COY_ID = GRN_DETAILS.GD_B_COY_ID AND  " _
                             & "DO_DETAILS.DOD_PO_LINE = GRN_DETAILS.GD_PO_LINE INNER JOIN " _
                             & "USER_MSTR ON GRN_MSTR.GM_CREATED_BY = USER_MSTR.UM_USER_ID " _
                             & " AND GRN_MSTR.GM_B_COY_ID = user_mstr.UM_COY_ID INNER JOIN " _
                             & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                             & "WHERE (PO_MSTR.POM_PO_NO =@prmPONo) AND (GRN_MSTR.GM_GRN_NO = @prmGRN) AND " _
                             & "(DO_MSTR.DOM_DO_NO = @prmDONo) " _
                             & "AND GRN_DETAILS.GD_B_COY_ID=@prmBCoyID"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Me.Request.QueryString("PONo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGRN", Me.Request.QueryString("GRNNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Me.Request.QueryString("DONo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Me.Request.QueryString("BCoyID")))

            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 And blnSEH = True Then
                Dim DISC_AMT As Decimal
                Dim dblOthCharge As Decimal
                Dim dblAmtF As Decimal
                Dim dblAmtM As Decimal
                Dim dblRate As Decimal
                Dim dblFactor As Decimal
                Dim dblGRNFactor As Decimal
                Dim UNIT_COST As Decimal
                Dim Qty As Decimal
                Dim CIF_VAL As Decimal
                Dim dblInland As Decimal
                Dim LAND_COST As Decimal
                Dim dblDuties As Decimal

                For j = 0 To ds.Tables(0).Rows.Count - 1
                    'Disc Amt
                    DISC_AMT = CalDiscAmt(ds.Tables(0).Rows(j)("POD_VENDOR_ITEM_CODE"), ds.Tables(0).Rows(j)("POM_S_COY_ID"), ds.Tables(0).Rows(j)("POD_ORDERED_QTY"))
                    DISC_AMT = Format(DISC_AMT, "##0.00")
                    ds.Tables(0).Rows(j)("DiscAmt") = DISC_AMT

                    'Oth Charge
                    If IsDBNull(ds.Tables(0).Rows(j)("GD_OTH_CHARGE")) Then
                        dblOthCharge = 0
                    Else
                        dblOthCharge = CDec(ds.Tables(0).Rows(j)("GD_OTH_CHARGE"))
                    End If

                    'Amt F
                    dblAmtF = (CDec(ds.Tables(0).Rows(j)("POD_UNIT_COST")) * (CDec(ds.Tables(0).Rows(j)("GD_RECEIVED_QTY")) - CDec(ds.Tables(0).Rows(j)("GD_REJECTED_QTY")))) + dblOthCharge
                    ds.Tables(0).Rows(j)("AmtF") = Format(dblAmtF, "####0.00")

                    'Rate
                    If IsDBNull(ds.Tables(0).Rows(j)("GD_EXCHANGE_RATE")) Then
                        dblRate = 1
                    Else
                        dblRate = CDec(ds.Tables(0).Rows(j)("GD_EXCHANGE_RATE"))
                    End If

                    'Amt M
                    dblAmtM = dblAmtF * dblRate
                    ds.Tables(0).Rows(j)("AmtM") = Format(dblAmtM, "####0.00")

                    'GRN Factor
                    If IsDBNull(ds.Tables(0).Rows(j)("GD_FACTOR")) Then
                        dblGRNFactor = 0
                    Else
                        dblGRNFactor = CDec(ds.Tables(0).Rows(j)("GD_FACTOR"))
                    End If
                    dblFactor = dblAmtM * dblGRNFactor / 100
                    ds.Tables(0).Rows(j)("GRNFactor") = Format(dblFactor, "####0.00")

                    'Qty
                    Qty = CDec(ds.Tables(0).Rows(j)("GD_RECEIVED_QTY")) - CDec(ds.Tables(0).Rows(j)("GD_REJECTED_QTY"))

                    'Unit Cost(M)
                    If Qty = 0 Then
                        UNIT_COST = (CDec(ds.Tables(0).Rows(j)("POD_UNIT_COST")) * dblRate) + 0
                    Else
                        UNIT_COST = (CDec(ds.Tables(0).Rows(j)("POD_UNIT_COST")) * dblRate) + (dblFactor / Qty)
                    End If
                    ds.Tables(0).Rows(j)("UnitCost") = Format(UNIT_COST, "##0.00")

                    'Inland Charges
                    If IsDBNull(ds.Tables(0).Rows(j)("GD_INLAND_CHARGE")) Then
                        dblInland = 0
                    Else
                        dblInland = CDec(ds.Tables(0).Rows(j)("GD_INLAND_CHARGE"))
                    End If

                    'CIF
                    CIF_VAL = dblFactor + dblInland
                    ds.Tables(0).Rows(j)("CIFValues") = Format(CIF_VAL, "##0.00")

                    'Duties
                    If IsDBNull(ds.Tables(0).Rows(j)("GD_DUTIES")) Then
                        dblDuties = 0
                    Else
                        dblDuties = CDec(ds.Tables(0).Rows(j)("GD_DUTIES"))
                    End If

                    'Landed Cost
                    LAND_COST = Format(CIF_VAL, "##0.00") + dblDuties
                    ds.Tables(0).Rows(j)("LandedCost") = Format(LAND_COST, "##0.00")

                    TotalLandCost = Format(LAND_COST, "##0.00") + TotalLandCost
                Next
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewGRN_DataSetPreviewGRN", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If blnSEH = True Then
                localreport.ReportPath = dispatcher.direct("Report", "PreviewGRN-FTN-SEH.rdlc", "Report") 'appPath & "GRN\PreviewGRN-FTN.rdlc"
            Else
                localreport.ReportPath = dispatcher.direct("Report", "PreviewGRN-FTN.rdlc", "Report") 'appPath & "GRN\PreviewGRN-FTN.rdlc"
            End If
            localreport.EnableExternalImages = True

            ' If strImgSrc <> "" Then
            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "grn_logo"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "grn_value"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, TotalLandCost)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            'End If

            localreport.Refresh()

            'Dim deviceInfo As String = _
            '    "<DeviceInfo>" + _
            '        "  <OutputFormat>EMF</OutputFormat>" + _
            '        "  <PageWidth>8.27in</PageWidth>" + _
            '        "  <PageHeight>11in</PageHeight>" + _
            '        "  <MarginTop>0.25in</MarginTop>" + _
            '        "  <MarginLeft>0.25in</MarginLeft>" + _
            '        "  <MarginRight>0.25in</MarginRight>" + _
            '        "  <MarginBottom>0.25in</MarginBottom>" + _
            '        "</DeviceInfo>"
            Dim deviceInfo As String = _
                            "<DeviceInfo>" + _
                                "  <OutputFormat>EMF</OutputFormat>" + _
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "GRN_" & Request.QueryString(Trim("GRNNo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            Dim strTemp As String = dispatcher.direct("Report", "Temp", "Report")
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('Temp/" & strFileName & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Function CalDiscAmt(ByVal strItem As String, ByVal strVenId As String, ByVal dblOdrQty As Double) As Double
        Dim dsAllPrice As New DataSet
        Dim i As Integer
        Dim dbl1stPrice, dblPrice As Double
        dsAllPrice = objGRN_Ext.GetGRNVolPrice(strVenId, strItem)

        If dsAllPrice.Tables(0).Rows.Count > 1 Then
            For i = 0 To dsAllPrice.Tables(0).Rows.Count - 1
                If i = 0 Then
                    dbl1stPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                    dblPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                Else
                    If dblOdrQty >= CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME")) Then
                        dblPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                    End If
                End If
            Next

            CalDiscAmt = (dbl1stPrice - dblPrice) * dblOdrQty
        Else
            CalDiscAmt = 0
        End If

    End Function
End Class