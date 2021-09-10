Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewIQC1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewIQC()
    End Sub

    Private Sub PreviewIQC()
        Dim ds As New DataSet
        Dim ds2 As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strLoc As String
        Dim strSLoc As String
        Dim objInv As New Inventory
        Dim strHeader, strRevision, strSuppCode, strVendor, strContinueLot, strPurSpecNo, strSpec1, strSpec2, strSpec3, strTemp As String
        Dim blnLog As Boolean

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM (" &
                            "(SELECT '1' AS tb, IVL_IQC_NO, IM_ITEM_CODE, IM_INVENTORY_NAME, PM_IQC_TYPE, DOL_LOT_NO, SUM(IVL_LOT_QTY) AS IVL_LOT_QTY, " &
                            "PM_UOM, CASE WHEN IVL_STATUS IS NULL THEN 'Outstanding' WHEN IVL_STATUS=1 THEN 'Closed (Approved)' " &
                            "WHEN IVL_STATUS=2 THEN 'Closed (Waived)' WHEN IVL_STATUS=3 THEN 'Closed (Replacement)' WHEN IVL_STATUS=4 THEN 'Rejected' END AS IVL_STATUS, " &
                            "POM_PO_NO, POM_PO_DATE, DOM_DO_NO, IM_INVOICE_NO, IM_SUBMIT_DATE, GM_CREATED_DATE, " &
                            "DOL_DO_MANUFACTURER, DOL_IQC_MANU_DT, DOL_IQC_EXP_DT, POM_VENDOR_CODE, PM_PRODUCT_INDEX, DOL_COY_ID, " &
                            "NULL AS IQCA_SEQ, NULL AS AO_NAME, NULL AS AAO_NAME, " &
                            "NULL AS IQCA_APPROVAL_TYPE, NULL AS IQCA_ACTION_DATE, NULL AS IQCA_AO_REMARK " &
                            "FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV, INVENTORY_MSTR IM, DO_LOT DOL, GRN_MSTR GM " &
                            "LEFT JOIN INVOICE_MSTR IV_M ON IV_M.IM_INVOICE_NO = GM.GM_INVOICE_NO AND IV_M.IM_S_COY_ID = GM.GM_S_COY_ID, " &
                            "DO_MSTR DOM, PO_MSTR POM, PRODUCT_MSTR PM WHERE IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                            "AND IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX AND IVL.IVL_LOT_INDEX = DOL.DOL_LOT_INDEX " &
                            "AND IV.IV_GRN_NO = GM.GM_GRN_NO AND IM.IM_COY_ID = GM.GM_B_COY_ID AND GM.GM_DO_INDEX = DOM.DOM_DO_INDEX " &
                            "AND GM.GM_PO_INDEX = POM.POM_PO_INDEX AND IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE AND IM.IM_COY_ID = PM.PM_S_COY_ID " &
                            "AND IVL_IQC_NO = @prmIQCNo AND IM_COY_ID = @prmCoyID GROUP BY IVL_IQC_NO) " &
                            "UNION " &
                            "(SELECT '2' AS tb, NULL AS IVL_IQC_NO, NULL AS IM_ITEM_CODE, NULL AS IM_INVENTORY_NAME, NULL AS PM_IQC_TYPE, " &
                            "NULL AS DOL_LOT_NO, NULL AS IVL_LOT_QTY, NULL AS PM_UOM, NULL AS IVL_STATUS, NULL AS POM_PO_NO, NULL AS POM_PO_DATE, " &
                            "NULL AS DOM_DO_NO, NULL AS IM_INVOICE_NO, NULL AS IM_SUBMIT_DATE, NULL AS GM_CREATED_DATE, " &
                            "NULL AS DOL_DO_MANUFACTURER, NULL AS DOL_IQC_MANU_DT, NULL AS DOL_IQC_EXP_DT, " &
                            "NULL AS POM_VENDOR_CODE, NULL AS PM_PRODUCT_INDEX, NULL AS DOL_COY_ID, " &
                            "IQCA_SEQ, UMA.UM_USER_NAME AS AO_NAME, UMB.UM_USER_NAME AS AAO_NAME, " &
                            "CASE WHEN IQCA_APPROVAL_TYPE=1 THEN 'Approval' END AS IQCA_APPROVAL_TYPE, IQCA_ACTION_DATE, IQCA_AO_REMARK " &
                            "FROM IQC_APPROVAL IQCA " &
                            "LEFT OUTER JOIN USER_MSTR UMA ON IQCA.IQCA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID=@prmCoyID " &
                            "LEFT OUTER JOIN USER_MSTR UMB ON IQCA.IQCA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID=@prmCoyID " &
                            "WHERE IQCA_IQC_INDEX = @prmIQCIndex ORDER BY IQCA_SEQ) " &
                            "UNION " &
                            "(SELECT '3' AS tb, NULL AS IVL_IQC_NO, NULL AS IM_ITEM_CODE, NULL AS IM_INVENTORY_NAME, NULL AS PM_IQC_TYPE, " &
                            "NULL AS DOL_LOT_NO, NULL AS IVL_LOT_QTY, NULL AS PM_UOM, NULL AS IVL_STATUS, NULL AS POM_PO_NO, NULL AS POM_PO_DATE, " &
                            "NULL AS DOM_DO_NO, NULL AS IM_INVOICE_NO, NULL AS IM_SUBMIT_DATE, NULL AS GM_CREATED_DATE, " &
                            "NULL AS DOL_DO_MANUFACTURER, NULL AS DOL_IQC_MANU_DT, NULL AS DOL_IQC_EXP_DT, " &
                            "NULL AS POM_VENDOR_CODE, NULL AS PM_PRODUCT_INDEX, NULL AS DOL_COY_ID, " &
                            "IQCA_SEQ, UMA.UM_USER_NAME AS AO_NAME, UMB.UM_USER_NAME AS AAO_NAME, " &
                            "CASE WHEN IQCA_APPROVAL_TYPE=1 THEN 'Approval' END AS IQCA_APPROVAL_TYPE, IQCA_ACTION_DATE, IQCA_AO_REMARK " &
                            "FROM IQC_APPROVAL_LOG IQCA " &
                            "LEFT OUTER JOIN USER_MSTR UMA ON IQCA.IQCA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID=@prmCoyID " &
                            "LEFT OUTER JOIN USER_MSTR UMB ON IQCA.IQCA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID=@prmCoyID " &
                            "WHERE IQCA_IQC_INDEX = @prmIQCIndex ORDER BY IQCA_RETEST_COUNT) " &
                            ") tb "
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmIQCNo", Request.QueryString("IQCNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmIQCIndex", Request.QueryString("IQCIndex")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyId")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmUserID", Session("UserId")))

            da.Fill(ds)

            'Log
            If objDb.Exist("SELECT '*' FROM IQC_APPROVAL_LOG WHERE IQCA_IQC_INDEX =" & Request.QueryString("IQCIndex")) Then
                blnLog = True
            Else
                blnLog = False
            End If

            'Header
            If Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_TYPE")) = "TAS" Then
                strHeader = "Test Application Sheet"
            ElseIf Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_TYPE")) = "NTAS" Then
                strHeader = "Non-Test Application Sheet"
            ElseIf Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_TYPE")) = "STS" Then
                strHeader = "Ship To Stock"
            Else
                strHeader = ""
            End If



            'Vendor Name
            strTemp = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID='" & Common.parseNull(ds.Tables(0).Rows(0)("DOL_COY_ID")) & "' "
            strVendor = objDb.GetVal(strTemp)

            'Revision
            strSuppCode = Common.parseNull(ds.Tables(0).Rows(0)("POM_VENDOR_CODE"))
            strTemp = "SELECT PV_REVISION FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX=" & Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_INDEX")) &
                    " AND PV_S_COY_ID='" & Common.parseNull(ds.Tables(0).Rows(0)("DOL_COY_ID")) & "' AND PV_SUPP_CODE='" & Common.Parse(strSuppCode) & "' "
            strRevision = objDb.GetVal(strTemp)

            'Continue Lot
            strContinueLot = objInv.IQCChkLotContinue(Common.parseNull(ds.Tables(0).Rows(0)("DOL_COY_ID")), Common.parseNull(ds.Tables(0).Rows(0)("DOL_LOT_NO")), Common.parseNull(ds.Tables(0).Rows(0)("DOM_DO_NO")))

            ds2 = objInv.getIQCInfoFromPO(Common.parseNull(ds.Tables(0).Rows(0)("POM_PO_NO")), Common.parseNull(ds.Tables(0).Rows(0)("IM_ITEM_CODE")))
            If ds2.Tables(0).Rows.Count > 0 Then
                strPurSpecNo = Common.parseNull(ds2.Tables(0).Rows(0)("POD_PUR_SPEC_NO")) 'Purchasing Spec No
                strSpec1 = Common.parseNull(ds2.Tables(0).Rows(0)("POD_SPEC1")) 'Specification 1
                strSpec2 = Common.parseNull(ds2.Tables(0).Rows(0)("POD_SPEC2")) 'Specification 2
                strSpec3 = Common.parseNull(ds2.Tables(0).Rows(0)("POD_SPEC3")) 'Specification 3
            Else
                strPurSpecNo = ""
                strSpec1 = ""
                strSpec2 = ""
                strSpec3 = ""
            End If
            ds2 = Nothing
            objInv = Nothing

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewIQC_DataSetPreviewIQC", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If blnLog = True Then
                localreport.ReportPath = dispatcher.direct("Report", "PreviewIQCWithLog.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            Else
                localreport.ReportPath = dispatcher.direct("Report", "PreviewIQC.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            End If
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmlogo"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "pmcoysname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strVendor)

                    Case "pmpurspecno"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strPurSpecNo)

                    Case "pmrevision"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strRevision)

                    Case "pmcontinuelot"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strContinueLot)

                    Case "pmspec1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSpec1)

                    Case "pmspec2"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSpec2)

                    Case "pmspec3"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSpec3)

                    Case "pmheader"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strHeader)

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
            Dim strFileName As String = "IQC_" & Request.QueryString(Trim("IQCNo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")

            strTemp = dispatcher.direct("Report", "Temp", "Report")
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
End Class