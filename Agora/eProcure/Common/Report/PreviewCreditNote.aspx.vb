Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewCreditNote1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewCreditNote()
    End Sub

    Private Sub PreviewCreditNote()
        Dim ds, ds1 As New DataSet
        Dim objGst As New GST
        'Dim blnSEH As Boolean = False
        Dim blnGst As Boolean = False
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim da1 As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strNo As String = ""
        Dim strTaxInv As String = "N"

        'If objDb.Exist("SELECT '*' FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID = '" & Request.QueryString("rfqid") & "' AND RRD_V_Coy_ID='" & Request.QueryString("SCoyID") & "' AND (RRD_DEL_CODE <> '' AND RRD_DEL_CODE IS NOT NULL)") Then
        '    blnSEH = True
        'End If

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))


        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand

            With cmd
                .Connection = conn
                .CommandText = CommandType.Text
                .CommandText = "SELECT im_gst_invoice, CNM_CN_NO FROM credit_note_mstr " &
                                "INNER JOIN invoice_mstr ON im_invoice_no = cnm_inv_no AND im_s_coy_Id = cnm_cn_s_coy_id AND im_b_coy_Id = cnm_cn_b_coy_id " &
                                "WHERE (cnm_cn_b_coy_id =  '" & Request.QueryString("BCoyID") & "') AND (cnm_cn_s_coy_id = '" & Request.QueryString("SCoyID") & "') " &
                                "AND (cnm_cn_no = '" & Request.QueryString("CN_No") & "')"
            End With

            da1 = New MySqlDataAdapter(cmd)
            da1.Fill(ds1)

            If ds1.Tables(0).Rows.Count > 0 Then
                strNo = ds1.Tables(0).Rows(0)("CNM_CN_NO")
                strTaxInv = ds1.Tables(0).Rows(0)("im_gst_invoice")

                With cmd
                    .Connection = conn
                    .CommandType = CommandType.Text

                    If strTaxInv = "Y" Then
                        '.CommandText = "SELECT CM_S.CM_COY_NAME AS V_COY_NAME, CM_S.CM_ADDR_LINE1 AS V_ADDR_LINE1, CM_S.CM_ADDR_LINE2 AS V_ADDR_LINE2, CM_S.CM_ADDR_LINE3 AS V_ADDR_LINE3, " & _
                        '            "CM_S.CM_POSTCODE AS V_POSTCODE, CM_S.CM_CITY AS V_CITY, CM_S_STATE.CODE_DESC AS V_STATE, CM_S_COUNTRY.CODE_DESC AS V_COUNTRY, CM_S.CM_TAX_REG_NO AS V_TAX_REG_NO, " & _
                        '            "CM_S.CM_BUSINESS_REG_NO AS V_BUSINESS_REG_NO, CM_S.CM_EMAIL AS V_EMAIL, CM_S.CM_PHONE AS V_PHONE, CNM_CN_NO, CNM_CREATED_DATE, CNM_CURRENCY_CODE, CNM_CN_TYPE, CM_B.CM_COY_NAME AS B_COY_NAME, " & _
                        '            "CNM_ADDR_LINE1 AS B_ADDR_LINE1, CNM_ADDR_LINE2 AS B_ADDR_LINE2, CNM_ADDR_LINE3 AS B_ADDR_LINE3, CNM_POSTCODE AS B_POSTCODE, " & _
                        '            "CNM_CITY AS B_CITY, CM_B_STATE.CODE_DESC AS B_STATE, CM_B_COUNTRY.CODE_DESC AS B_COUNTRY, IM_SHIP_AMT, IM_WITHHOLDING_TAX, " & _
                        '            "CNM_REMARKS, CNM_EXCHANGE_RATE, CND_CN_LINE, ID_PRODUCT_DESC, ID_UOM, ID_GST, CND_GST_RATE AS ID_GST_RATE, CND_QTY, CND_UNIT_COST, CND_REMARKS, " & _
                        '            "((CND_QTY * CND_UNIT_COST) * IFNULL(ID_GST, 0) / 100) AS GST, " & _
                        '            "(CND_QTY * CND_UNIT_COST) + ((CND_QTY * CND_UNIT_COST) * IFNULL(ID_GST, 0) / 100) AS Amount " & _
                        '            "FROM CREDIT_NOTE_MSTR " & _
                        '            "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " & _
                        '            "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " & _
                        '            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " & _
                        '            "INNER JOIN COMPANY_MSTR CM_S ON CNM_CN_S_COY_ID = CM_S.CM_COY_ID " & _
                        '            "INNER JOIN COMPANY_MSTR CM_B ON CNM_CN_B_COY_ID = CM_B.CM_COY_ID " & _
                        '            "INNER JOIN CODE_MSTR CM_S_STATE ON CM_S.CM_STATE = CM_S_STATE.CODE_ABBR AND CM_S_STATE.CODE_CATEGORY = 'S' " & _
                        '            "INNER JOIN CODE_MSTR CM_S_COUNTRY ON CM_S.CM_COUNTRY = CM_S_COUNTRY.CODE_ABBR AND CM_S_COUNTRY.CODE_CATEGORY = 'CT' " & _
                        '            "INNER JOIN CODE_MSTR CM_B_STATE ON CM_B.CM_STATE = CM_B_STATE.CODE_ABBR AND CM_B_STATE.CODE_CATEGORY = 'S' " & _
                        '            "INNER JOIN CODE_MSTR CM_B_COUNTRY ON CM_B.CM_COUNTRY = CM_B_COUNTRY.CODE_ABBR AND CM_B_COUNTRY.CODE_CATEGORY = 'CT' " & _
                        '            "WHERE CNM_CN_NO = @prmCnNum AND CNM_CN_S_COY_ID = @prmVCoyID AND CNM_CN_B_COY_ID = @prmBCoyID"
                        .CommandText = "SELECT CM_S.CM_COY_NAME AS V_COY_NAME, CM_S.CM_ADDR_LINE1 AS V_ADDR_LINE1, " &
                                "CM_S.CM_ADDR_LINE2 AS V_ADDR_LINE2, CM_S.CM_ADDR_LINE3 AS V_ADDR_LINE3, CM_S.CM_POSTCODE AS V_POSTCODE, " &
                                "CM_S.CM_CITY AS V_CITY, " &
                                "CM_S.CM_TAX_REG_NO AS V_TAX_REG_NO, CM_S.CM_BUSINESS_REG_NO AS V_BUSINESS_REG_NO, CM_S.CM_EMAIL AS V_EMAIL, " &
                                "CM_S.CM_PHONE AS V_PHONE, CNM_CN_NO, CNM_CREATED_DATE, CNM_CURRENCY_CODE, CNM_CN_TYPE, CM_B.CM_COY_NAME AS B_COY_NAME, " &
                                "CNM_ADDR_LINE1 AS B_ADDR_LINE1, CNM_ADDR_LINE2 AS B_ADDR_LINE2, CNM_ADDR_LINE3 AS B_ADDR_LINE3, CNM_POSTCODE AS B_POSTCODE, " &
                                "CNM_CITY AS B_CITY, IM_SHIP_AMT, IM_WITHHOLDING_TAX, IM_INVOICE_NO, " &
                                "CNM_REMARKS, CNM_EXCHANGE_RATE, CND_CN_LINE, ID_PRODUCT_DESC, ID_UOM, ID_GST, CND_GST_RATE AS ID_GST_RATE, CND_QTY, " &
                                "CND_UNIT_COST, CND_REMARKS, " &
                                "((CND_QTY * CND_UNIT_COST) * IFNULL(ID_GST, 0) / 100) AS GST, (CND_QTY * CND_UNIT_COST) + ((CND_QTY * CND_UNIT_COST) * IFNULL(ID_GST, 0) / 100) AS Amount, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CM_S.CM_STATE) AND (CODE_CATEGORY = 's') " &
                                "AND (CODE_VALUE = CM_S.CM_COUNTRY)) AS V_STATE, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR =CM_S.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS V_COUNTRY, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CNM_STATE) AND (CODE_CATEGORY = 's') " &
                                "AND (CODE_VALUE = CNM_COUNTRY)) AS B_STATE, " &
                                "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CNM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS B_COUNTRY " &
                                "FROM CREDIT_NOTE_MSTR " &
                                "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " &
                                "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " &
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                                "AND CND_INV_LINE = ID_INVOICE_LINE " &
                                "INNER JOIN COMPANY_MSTR CM_S ON CNM_CN_S_COY_ID = CM_S.CM_COY_ID " &
                                "INNER JOIN COMPANY_MSTR CM_B ON CNM_CN_B_COY_ID = CM_B.CM_COY_ID " &
                                "WHERE CNM_CN_NO = @prmCnNum AND CNM_CN_S_COY_ID = @prmVCoyID AND CNM_CN_B_COY_ID =@prmBCoyID"
                    Else
                        '.CommandText = "SELECT CM_S.CM_COY_NAME AS V_COY_NAME, CM_S.CM_ADDR_LINE1 AS V_ADDR_LINE1, CM_S.CM_ADDR_LINE2 AS V_ADDR_LINE2, CM_S.CM_ADDR_LINE3 AS V_ADDR_LINE3, " & _
                        '            "CM_S.CM_POSTCODE AS V_POSTCODE, CM_S.CM_CITY AS V_CITY, CM_S_STATE.CODE_DESC AS V_STATE, CM_S_COUNTRY.CODE_DESC AS V_COUNTRY, CM_S.CM_TAX_REG_NO AS V_TAX_REG_NO, " & _
                        '            "CM_S.CM_BUSINESS_REG_NO AS V_BUSINESS_REG_NO, CM_S.CM_EMAIL AS V_EMAIL, CM_S.CM_PHONE AS V_PHONE, CNM_CN_NO, CNM_CREATED_DATE, CNM_CURRENCY_CODE, CNM_CN_TYPE, CM_B.CM_COY_NAME AS B_COY_NAME, " & _
                        '            "CNM_ADDR_LINE1 AS B_ADDR_LINE1, CNM_ADDR_LINE2 AS B_ADDR_LINE2, CNM_ADDR_LINE3 AS B_ADDR_LINE3, CNM_POSTCODE AS B_POSTCODE, " & _
                        '            "CNM_CITY AS B_CITY, CM_B_STATE.CODE_DESC AS B_STATE, CM_B_COUNTRY.CODE_DESC AS B_COUNTRY, IM_SHIP_AMT, IM_WITHHOLDING_TAX, " & _
                        '            "CNM_REMARKS, CNM_EXCHANGE_RATE, CND_CN_LINE, ID_PRODUCT_DESC, ID_UOM, ID_GST, CND_GST_RATE AS ID_GST_RATE, CND_QTY, CND_UNIT_COST, CND_REMARKS " & _
                        '            "FROM CREDIT_NOTE_MSTR " & _
                        '            "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " & _
                        '            "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " & _
                        '            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " & _
                        '            "INNER JOIN COMPANY_MSTR CM_S ON CNM_CN_S_COY_ID = CM_S.CM_COY_ID " & _
                        '            "INNER JOIN COMPANY_MSTR CM_B ON CNM_CN_B_COY_ID = CM_B.CM_COY_ID " & _
                        '            "INNER JOIN CODE_MSTR CM_S_STATE ON CM_S.CM_STATE = CM_S_STATE.CODE_ABBR AND CM_S_STATE.CODE_CATEGORY = 'S' " & _
                        '            "INNER JOIN CODE_MSTR CM_S_COUNTRY ON CM_S.CM_COUNTRY = CM_S_COUNTRY.CODE_ABBR AND CM_S_COUNTRY.CODE_CATEGORY = 'CT' " & _
                        '            "INNER JOIN CODE_MSTR CM_B_STATE ON CM_B.CM_STATE = CM_B_STATE.CODE_ABBR AND CM_B_STATE.CODE_CATEGORY = 'S' " & _
                        '            "INNER JOIN CODE_MSTR CM_B_COUNTRY ON CM_B.CM_COUNTRY = CM_B_COUNTRY.CODE_ABBR AND CM_B_COUNTRY.CODE_CATEGORY = 'CT' " & _
                        '            "WHERE CNM_CN_NO = @prmCnNum AND CNM_CN_S_COY_ID = @prmVCoyID AND CNM_CN_B_COY_ID = @prmBCoyID"
                        .CommandText = "SELECT CM_S.CM_COY_NAME AS V_COY_NAME, CM_S.CM_ADDR_LINE1 AS V_ADDR_LINE1, " &
                                    "CM_S.CM_ADDR_LINE2 AS V_ADDR_LINE2, CM_S.CM_ADDR_LINE3 AS V_ADDR_LINE3, CM_S.CM_POSTCODE AS V_POSTCODE, " &
                                    "CM_S.CM_CITY AS V_CITY, " &
                                    "CM_S.CM_TAX_REG_NO AS V_TAX_REG_NO, CM_S.CM_BUSINESS_REG_NO AS V_BUSINESS_REG_NO, CM_S.CM_EMAIL AS V_EMAIL, " &
                                    "CM_S.CM_PHONE AS V_PHONE, CNM_CN_NO, CNM_CREATED_DATE, CNM_CURRENCY_CODE, CNM_CN_TYPE, CM_B.CM_COY_NAME AS B_COY_NAME, " &
                                    "CNM_ADDR_LINE1 AS B_ADDR_LINE1, CNM_ADDR_LINE2 AS B_ADDR_LINE2, CNM_ADDR_LINE3 AS B_ADDR_LINE3, CNM_POSTCODE AS B_POSTCODE, " &
                                    "CNM_CITY AS B_CITY, IM_SHIP_AMT, IM_WITHHOLDING_TAX, IM_INVOICE_NO, " &
                                    "CNM_REMARKS, CNM_EXCHANGE_RATE, CND_CN_LINE, ID_PRODUCT_DESC, ID_UOM, ID_GST, CND_GST_RATE AS ID_GST_RATE, CND_QTY, CND_UNIT_COST, CND_REMARKS, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CM_S.CM_STATE) AND (CODE_CATEGORY = 's') " &
                                    "AND (CODE_VALUE = CM_S.CM_COUNTRY)) AS V_STATE, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR =CM_S.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS V_COUNTRY, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CNM_STATE) AND (CODE_CATEGORY = 's') " &
                                    "AND (CODE_VALUE = CNM_COUNTRY)) AS B_STATE, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = CNM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS B_COUNTRY " &
                                    "FROM CREDIT_NOTE_MSTR " &
                                    "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " &
                                    "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " &
                                    "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                                    "AND CND_INV_LINE = ID_INVOICE_LINE " &
                                    "INNER JOIN COMPANY_MSTR CM_S ON CNM_CN_S_COY_ID = CM_S.CM_COY_ID " &
                                    "INNER JOIN COMPANY_MSTR CM_B ON CNM_CN_B_COY_ID = CM_B.CM_COY_ID " &
                                    "WHERE CNM_CN_NO = @prmCnNum AND CNM_CN_S_COY_ID = @prmVCoyID AND CNM_CN_B_COY_ID =@prmBCoyID"
                    End If

                End With

                da = New MySqlDataAdapter(cmd)
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("SCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCnNum", Request.QueryString("CN_No")))

                da.Fill(ds)

                If ds.Tables(0).Rows.Count > 0 Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("CNM_CN_TYPE")) = "CN" Then
                        blnGst = True
                    Else
                        blnGst = False
                    End If
                End If

                Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewCN_DataTable1", ds.Tables(0))
                Dim localreport As New LocalReport
                localreport.DataSources.Clear()
                localreport.DataSources.Add(rptDataSource)
                If blnGst = True Then
                    'Jules 2018.10.08 - SST
                    'localreport.ReportPath = dispatcher.direct("Report", "PreviewCreditNote.rdlc", "Report") ' appPath & "RFQ\PreviewQuotation-FTN.rdlc"
                    Dim objSST As New GST
                    Dim strDocType As String = "Service"
                    Dim blnSST As Boolean = False

                    blnSST = objSST.chkDocumentType(Request.QueryString("CN_NO"), "CN",, Request.QueryString("SCoyID"), Request.QueryString("BCoyID"),, strDocType)
                    If blnSST Then
                        localreport.ReportPath = dispatcher.direct("Report", "PreviewCreditNote-" & strDocType & ".rdlc", "Report")
                    Else
                        localreport.ReportPath = dispatcher.direct("Report", "PreviewCreditNote.rdlc", "Report") ' appPath & "RFQ\PreviewQuotation-FTN.rdlc"
                    End If
                    'End modification.
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewCreditAdvice.rdlc", "Report") ' appPath & "RFQ\PreviewQuotation-FTN.rdlc"
                End If
                localreport.EnableExternalImages = True

                Dim I As Byte
                Dim GetParameter As String = ""
                Dim TotalParameter As Byte
                TotalParameter = localreport.GetParameters.Count
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                'Dim paramlist As New Generic.List(Of ReportParameter)
                Dim strID As String = ""

                strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "par1"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

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
                Dim strFileName As String
                If blnGst = True Then
                    strFileName = "CN_" & Request.QueryString(Trim("CN_No")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
                Else
                    strFileName = "CA_" & Request.QueryString(Trim("CN_No")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
                End If
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
            End If

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