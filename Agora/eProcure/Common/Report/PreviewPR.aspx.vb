Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class PreviewPR1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewPR()
    End Sub

    Private Sub PreviewPR()
        Dim ds As New DataSet
        Dim objGst As New GST
        Dim blnGst As Boolean = False
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc, strDelCode, strItemType As String
        Dim objRpt As New PurchaseOrder
        Dim decSubTotal As Decimal
        Dim decTax As Decimal
        Dim objDb As New EAD.DBCom
        Dim strChkAccCode As String
        Dim strsql As String = ""
        Dim strQuery As String = ""
        Dim err As String = ""
        ' strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        'Check Account Code Setting
        'strsql = "SELECT CM_DISPLAY_ACCT FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Request.QueryString("BCoyID") & "' "
        'strChkAccCode = objDb.GetVal(strsql)

        'Check buyer 
        'strsql = "SELECT '*' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID ='" & Request.QueryString("BCoyID") & "' "
        'strDelCode = objDb.GetVal(strsql)

        'Check Item Type 
        'strsql = "SELECT '*' FROM PO_DETAILS WHERE POD_COY_ID ='" & Request.QueryString("BCoyID") & "' " & _
        '        "AND POD_PO_NO ='" & Request.QueryString(Trim("PO_No")) & "' AND POD_ITEM_TYPE ='ST'"
        'strItemType = objDb.GetVal(strsql)

        Try
            strQuery = "SELECT PRM_PR_NO, PRM_REQ_NAME, PRM_REQ_PHONE, PRM_PR_DATE, PRM_SUBMIT_DATE, PRM_B_ADDR_LINE1, PRM_B_ADDR_LINE2, PRM_B_ADDR_LINE3, " &
                    "PRM_B_POSTCODE, PRM_B_CITY, PRM_INTERNAL_REMARK, PRM_S_COY_ID, PRM_S_COY_NAME, PRM_CURRENCY_CODE, PRM_BUYER_ID, " &
                    "PRM_S_ADDR_LINE1, PRM_S_ADDR_LINE2, PRM_S_ADDR_LINE3, PRM_S_POSTCODE, PRM_S_CITY, PRM_S_STATE, PRM_S_COUNTRY, PRM_S_PHONE, PRM_S_FAX, PRM_S_EMAIL, PRM_SHIP_VIA, PRM_FREIGHT_TERMS, PRM_S_ATTN, PRM_CREATED_DATE, " &
                    "PRM_EXCHANGE_RATE, PRM_EXTERNAL_REMARK, PRM_CONSOLIDATOR, PRM_GST, PRM_PR_TYPE, PRM_URGENT, PRM_COY_ID, PRM_STATUS_CHANGED_BY, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PRM_B_COUNTRY) AS CT, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PRM_B_STATE AND CODE_VALUE=PRM_B_COUNTRY) AS STATE, " &
                    "PRD_PR_LINE, PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_ORDERED_QTY, PRD_UOM, PRD_CURRENCY_CODE, PRD_UNIT_COST, " &
                    "PRD_GST, PRD_GST_INPUT_TAX_CODE, PRD_ETD, PRD_WARRANTY_TERMS, PRD_REMARK, " &
                    "PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PRD_D_STATE AND CODE_VALUE=PRD_D_COUNTRY) AS PRD_D_STATE, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PRD_D_COUNTRY) AS PRD_D_CT, " &
                    "CASE WHEN PRD_GST_RATE = 'N/A' THEN PRD_GST_RATE ELSE " &
                    "IF(TAX.TAX_PERC IS NULL OR TAX.TAX_PERC = '', IFNULL(gst_cm.CODE_DESC,'N/A'), CONCAT(gst_cm.CODE_DESC, ' (', TAX.TAX_PERC, '%)')) END AS PRD_GST_RATE, " &
                    "CM_COY_NAME, CM_BUSINESS_REG_NO, CM_TAX_REG_NO, " &
                    "UM_USER_NAME, UM_EMAIL, UM_TEL_NO, PRM_PRINT_CUSTOM_FIELDS, " &
                    "PCM_FIELD_NAME,PCD_FIELD_VALUE,PRM_PRINT_CUSTOM_FIELDS AS POM_PRINT_CUSTOM_FIELDS " &
                    "FROM PR_MSTR " &
                    "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " &
                    "LEFT JOIN CODE_MSTR gst_cm ON gst_cm.CODE_CATEGORY = 'SST' AND gst_cm.CODE_ABBR = PRD_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = PRD_GST_RATE AND TAX_COUNTRY_CODE = 'MY' " &
                    "INNER JOIN COMPANY_MSTR ON PRM_COY_ID = CM_COY_ID " &
                    "INNER JOIN USER_MSTR ON PRM_BUYER_ID = UM_USER_ID AND PRM_COY_ID = UM_COY_ID " &
                    "LEFT JOIN pr_custom_field_mstr ON PCM_PR_INDEX=PRM_PR_INDEX AND PCM_TYPE='PR' " &
                    "LEFT JOIN pr_custom_field_details ON PCM_PR_INDEX=PCD_PR_INDEX AND PCD_PR_LINE=PRD_PR_LINE " &
                    "WHERE PRM_PR_INDEX = @prmPRIndex"
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = strQuery
            End With

            da = New MySqlDataAdapter(cmd)
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString(Trim("BCoyID"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPRIndex", Request.QueryString(Trim("PR_Index"))))

            da.Fill(ds)

            'If ds.Tables(0).Rows.Count > 0 Then
            '    If objGst.chkGSTCOD(Format(ds.Tables(0).Rows(0)("POM_PO_DATE"), "dd/MM/yyyy")) = True Then
            '        blnGst = True
            '    Else
            '        blnGst = False
            '    End If
            'End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPR_PreviewPRDataSet", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            'Jules 2018.10.08 - SST
            'localreport.ReportPath = dispatcher.direct("Report", "PreviewPR-GST.rdlc", "Report")
            Dim objSST As New GST
            Dim strDocType As String = "Service"
            Dim blnSST As Boolean = False

            blnSST = objSST.chkDocumentType(Request.QueryString(Trim("PR_Index")), "PR",,,,, strDocType)
            localreport.ReportPath = dispatcher.direct("Report", "PreviewPR-" & strDocType & ".rdlc", "Report")
            'End modification.

            'If strDelCode <> "" And strItemType <> "" Then
            '    localreport.ReportPath = dispatcher.direct("Report", "PreviewPO-SEH.rdlc", "Report") 'appPath & "PO\PreviewPO-FTN.rdlc"
            'Else
            '    If blnGst = True Then
            '        localreport.ReportPath = dispatcher.direct("Report", "PreviewPO-FTN-GST.rdlc", "Report")
            '    Else
            '        If Common.parseNull(ds.Tables(0).Rows(0)("POM_DEL_CODE")) <> "" Then
            '            localreport.ReportPath = dispatcher.direct("Report", "PreviewPO-FTN-DT.rdlc", "Report") 'appPath & "PO\PreviewPO-FTN.rdlc"
            '        Else
            '            localreport.ReportPath = dispatcher.direct("Report", "PreviewPO-FTN.rdlc", "Report") 'appPath & "PO\PreviewPO-FTN.rdlc"
            '        End If
            '    End If
            'End If

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            Dim strID As String = ""
            Dim strBCoyName As String = Session("CompanyName")
            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            'objRpt.GetReportTotal(Request.QueryString(Trim("PR_No")), Request.QueryString(Trim("BCoyID")), decSubTotal, decTax)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "parbcoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strBCoyName)

                        'Case "prmtax"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, decTax)

                        'Case "prmacccode"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strChkAccCode)
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
            Dim strFileName As String = "PR_" & Request.QueryString(Trim("PR_No")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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
            err = ex.Message
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