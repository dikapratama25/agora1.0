
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewMRSSlip1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewMRSSlip()
    End Sub

    Private Sub PreviewMRSSlip()
        Dim ds, ds2 As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim intLocIndicator As Integer
        Dim strStatus, strUrgent As String
        Dim strMRSIndex, strIRNo, strIRIndex As String
        Dim objDb As New EAD.DBCom

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        strStatus = objDb.GetVal("SELECT IRSM_IRS_STATUS FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = '" & Request.QueryString("CoyID") & "' AND IRSM_IRS_NO = '" & Request.QueryString("MRSNo") & "'")
        strMRSIndex = objDb.GetVal("SELECT IRSM_IRS_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = '" & Request.QueryString("CoyID") & "' AND IRSM_IRS_NO = '" & Request.QueryString("MRSNo") & "'")
        strIRNo = objDb.GetVal("SELECT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = '" & strMRSIndex & "'")
        strIRIndex = objDb.GetVal("SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_NO = '" & strIRNo & "' AND IRM_IR_COY_ID = '" & Request.QueryString("CoyID") & "' ")
        objDb = Nothing

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If strStatus = "2" Or strStatus = "3" Or strStatus = "4" Or strStatus = "5" Or strStatus = "7" Then
                    .CommandText = "SELECT '1' AS TB, IRSM_IRS_NO, IRSM_IRS_DATE, IRSM_IRS_URGENT, IRSM_IRS_APPROVED_DATE, IRSM_IRS_REF_NO, IRSM_IRS_REQUESTOR_NAME, " &
                                "IRSM_IRS_ISSUE_TO, IRSM_IRS_ISSUE_REMARK, IRSM_IRS_ACKCANCEL_REMARK, CDM_DEPT_NAME, CONCAT(IRSM_IRS_SECTION, ' : ', CS_SEC_NAME) AS IRSM_IRS_SECTION, " &
                                "IRSM_IRS_REMARK, STATUS_DESC, IM_ITEM_CODE, IRSD_INVENTORY_NAME, IRSD_IRS_LINE, IRSD_QTY, IRSD_IRS_MTHISSUE, IRSD_IRS_LAST3MTH, IRSD_UOM, " &
                                "IRSL_LOT_QTY, DOL_LOT_NO, LM_LOCATION, LM_SUB_LOCATION, NULL AS AO, NULL AS  AAO, NULL AS SEQ, NULL AS ACTION_DATE, NULL AS REMARK, NULL AS APPROVAL_TYPE " &
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " &
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_LOT ON IRSD_IRS_COY_ID = IRSL_IRS_COY_ID AND IRSD_IRS_NO = IRSL_IRS_NO AND IRSD_IRS_LINE = IRSL_IRS_LINE " &
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                                "INNER JOIN STATUS_MSTR ON IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " &
                                "INNER JOIN DO_LOT ON IRSL_LOT_INDEX = DOL_LOT_INDEX " &
                                "INNER JOIN LOCATION_MSTR ON IRSL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                                "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                                "LEFT JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " &
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo " &
                                "UNION ALL " &
                                "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " &
                                "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS IRSM_IRS_ISSUE_REMARK, NULL AS IRSM_IRS_ACKCANCEL_REMARK, NULL AS CDM_DEPT_NAME, " &
                                "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " &
                                "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, NULL AS IRSD_IRS_MTHISSUE, NULL AS IRSD_IRS_LAST3MTH, NULL AS IRSD_UOM, NULL AS IRSL_LOT_QTY, NULL AS DOL_LOT_NO, NULL AS LM_LOCATION, NULL AS LM_SUB_LOCATION, " &
                                "UM_A.UM_USER_NAME AS AO, UM_B.UM_USER_NAME AS AAO, IRA_SEQ AS SEQ, IRA_ACTION_DATE AS ACTION_DATE, " &
                                "IRA_AO_REMARK AS REMARK, 'Approval' AS APPROVAL_TYPE " &
                                "FROM IR_APPROVAL " &
                                "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRA_IR_INDEX = IRM_IR_INDEX " &
                                "LEFT JOIN USER_MSTR UM_A ON IRM_IR_COY_ID = UM_A.UM_COY_ID AND IRA_AO = UM_A.UM_USER_ID " &
                                "LEFT JOIN USER_MSTR UM_B ON IRM_IR_COY_ID = UM_B.UM_COY_ID AND IRA_A_AO = UM_B.UM_USER_ID " &
                                "WHERE IRM_IR_NO = @prmIRNo AND IRM_IR_COY_ID = @prmCoyID " &
                                "UNION ALL " &
                                "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " &
                                "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS IRSM_IRS_ISSUE_REMARK, NULL AS IRSM_IRS_ACKCANCEL_REMARK, NULL AS CDM_DEPT_NAME, " &
                                "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " &
                                "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, NULL AS IRSD_IRS_MTHISSUE, NULL AS IRSD_IRS_LAST3MTH, NULL AS IRSD_UOM, NULL AS IRSL_LOT_QTY, NULL AS DOL_LOT_NO, NULL AS LM_LOCATION, NULL AS LM_SUB_LOCATION, " &
                                "UM_USER_NAME AS AO, NULL AS AAO, " &
                                "(SELECT MAX(IRA_SEQ) + 1 FROM IR_APPROVAL WHERE IRA_IR_INDEX = @prmIRIndex) AS SEQ, " &
                                "CASE WHEN IRSM_IRS_STATUS = '6' THEN IRSM_STATUS_CHANGED_ON ELSE IRSM_IRS_APPROVED_DATE END AS ACTION_DATE, " &
                                "CASE WHEN IRSM_IRS_STATUS = '6' THEN 'Rejected' " &
                                "WHEN (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4' " &
                                "OR IRSM_IRS_STATUS = '5') THEN 'Issued' ELSE '' END AS REMARK, " &
                                "'Issue MRS' AS APPROVAL_TYPE " &
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                                "LEFT OUTER JOIN USER_MSTR ON IRSM_BUYER_ID = UM_USER_ID AND IRSM_IRS_COY_ID = UM_COY_ID " &
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo "

                    '.CommandText = "SELECT '1' AS TB, IRSM_IRS_NO, IRSM_IRS_DATE, IRSM_IRS_URGENT, IRSM_IRS_APPROVED_DATE, IRSM_IRS_REF_NO, IRSM_IRS_REQUESTOR_NAME, " & _
                    '            "IRSM_IRS_ISSUE_TO, CDM_DEPT_NAME, CONCAT(IRSM_IRS_SECTION, ' : ', CS_SEC_NAME) AS IRSM_IRS_SECTION, " & _
                    '            "IRSM_IRS_REMARK, STATUS_DESC, IM_ITEM_CODE, IRSD_INVENTORY_NAME, IRSD_IRS_LINE, IRSD_QTY, " & _
                    '            "IRSL_LOT_QTY, DOL_LOT_NO, LM_LOCATION, LM_SUB_LOCATION, NULL AS AO, NULL AS  AAO, NULL AS SEQ, NULL AS ACTION_DATE, NULL AS REMARK " & _
                    '            "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_LOT ON IRSD_IRS_COY_ID = IRSL_IRS_COY_ID AND IRSD_IRS_NO = IRSL_IRS_NO AND IRSD_IRS_LINE = IRSL_IRS_LINE " & _
                    '            "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                    '            "INNER JOIN STATUS_MSTR ON IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " & _
                    '            "INNER JOIN DO_LOT ON IRSL_LOT_INDEX = DOL_LOT_INDEX " & _
                    '            "INNER JOIN LOCATION_MSTR ON IRSL_LOCATION_INDEX = LM_LOCATION_INDEX " & _
                    '            "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " & _
                    '            "LEFT JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " & _
                    '            "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo " & _
                    '            "UNION ALL " & _
                    '            "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " & _
                    '            "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS CDM_DEPT_NAME, " & _
                    '            "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " & _
                    '            "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, NULL AS IRSL_LOT_QTY, NULL AS DOL_LOT_NO, NULL AS LM_LOCATION, NULL AS LM_SUB_LOCATION, " & _
                    '            "UM_A.UM_USER_NAME AS AO, UM_B.UM_USER_NAME AS AAO, IRA_SEQ AS SEQ, IRA_ACTION_DATE AS ACTION_DATE, " & _
                    '            "IRA_AO_REMARK AS REMARK " & _
                    '            "FROM IR_APPROVAL " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRA_IR_INDEX = IRM_IR_INDEX " & _
                    '            "LEFT JOIN USER_MSTR UM_A ON IRM_IR_COY_ID = UM_A.UM_COY_ID AND IRA_AO = UM_A.UM_USER_ID " & _
                    '            "LEFT JOIN USER_MSTR UM_B ON IRM_IR_COY_ID = UM_B.UM_COY_ID AND IRA_A_AO = UM_B.UM_USER_ID " & _
                    '            "WHERE IRM_IR_NO = @prmIRNo AND IRM_IR_COY_ID = @prmCoyID"
                Else
                    .CommandText = "SELECT '1' AS TB, IRSM_IRS_NO, IRSM_IRS_DATE, IRSM_IRS_URGENT, IRSM_IRS_APPROVED_DATE, IRSM_IRS_REF_NO, IRSM_IRS_REQUESTOR_NAME, " &
                                "IRSM_IRS_ISSUE_TO, IRSM_IRS_ISSUE_REMARK, IRSM_IRS_ACKCANCEL_REMARK, CDM_DEPT_NAME, CONCAT(IRSM_IRS_SECTION, ' : ', CS_SEC_NAME) AS IRSM_IRS_SECTION, IRSM_IRS_REMARK, STATUS_DESC, " &
                                "IM_ITEM_CODE, IRSD_INVENTORY_NAME, IRSD_IRS_LINE, IRSD_QTY, IRSD_IRS_MTHISSUE, IRSD_IRS_LAST3MTH, IRSD_UOM, NULL AS AO, NULL AS  AAO, NULL AS SEQ, NULL AS ACTION_DATE, NULL AS REMARK, NULL AS APPROVAL_TYPE " &
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                                "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " &
                                "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                                "INNER JOIN STATUS_MSTR ON IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " &
                                "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                                "LEFT JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " &
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo " &
                                "UNION ALL " &
                                "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " &
                                "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS IRSM_IRS_ISSUE_REMARK, NULL AS IRSM_IRS_ACKCANCEL_REMARK, NULL AS CDM_DEPT_NAME, " &
                                "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " &
                                "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, NULL AS IRSD_IRS_MTHISSUE, NULL AS IRSD_IRS_LAST3MTH, NULL AS IRSD_UOM, " &
                                "UM_A.UM_USER_NAME AS AO, UM_B.UM_USER_NAME AS AAO, IRA_SEQ AS SEQ, IRA_ACTION_DATE AS ACTION_DATE, " &
                                "IRA_AO_REMARK AS REMARK, 'Approval' AS APPROVAL_TYPE " &
                                "FROM IR_APPROVAL " &
                                "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRA_IR_INDEX = IRM_IR_INDEX " &
                                "LEFT JOIN USER_MSTR UM_A ON IRM_IR_COY_ID = UM_A.UM_COY_ID AND IRA_AO = UM_A.UM_USER_ID " &
                                "LEFT JOIN USER_MSTR UM_B ON IRM_IR_COY_ID = UM_B.UM_COY_ID AND IRA_A_AO = UM_B.UM_USER_ID " &
                                "WHERE IRM_IR_NO = @prmIRNo AND IRM_IR_COY_ID = @prmCoyID " &
                                "UNION ALL " &
                                "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " &
                                "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS IRSM_IRS_ISSUE_REMARK, NULL AS IRSM_IRS_ACKCANCEL_REMARK, NULL AS CDM_DEPT_NAME, " &
                                "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " &
                                "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, NULL AS IRSD_IRS_MTHISSUE, NULL AS IRSD_IRS_LAST3MTH, NULL AS IRSD_UOM, " &
                                "UM_USER_NAME AS AO, NULL AS AAO, " &
                                "(SELECT MAX(IRA_SEQ) + 1 FROM IR_APPROVAL WHERE IRA_IR_INDEX = @prmIRIndex) AS SEQ, " &
                                "CASE WHEN IRSM_IRS_STATUS = '6' THEN IRSM_STATUS_CHANGED_ON ELSE IRSM_IRS_APPROVED_DATE END AS ACTION_DATE, " &
                                "CASE WHEN IRSM_IRS_STATUS = '6' THEN 'Rejected' " &
                                "WHEN (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4' " &
                                "OR IRSM_IRS_STATUS = '5') THEN 'Issued' ELSE '' END AS REMARK, " &
                                "'Issue MRS' AS APPROVAL_TYPE " &
                                "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                                "LEFT OUTER JOIN USER_MSTR ON IRSM_BUYER_ID = UM_USER_ID AND IRSM_IRS_COY_ID = UM_COY_ID " &
                                "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo "

                    '.CommandText = "SELECT '1' AS TB, IRSM_IRS_NO, IRSM_IRS_DATE, IRSM_IRS_URGENT, IRSM_IRS_APPROVED_DATE, IRSM_IRS_REF_NO, IRSM_IRS_REQUESTOR_NAME, " & _
                    '            "IRSM_IRS_ISSUE_TO, CDM_DEPT_NAME, CONCAT(IRSM_IRS_SECTION, ' : ', CS_SEC_NAME) AS IRSM_IRS_SECTION, IRSM_IRS_REMARK, STATUS_DESC, " & _
                    '            "IM_ITEM_CODE, IRSD_INVENTORY_NAME, IRSD_IRS_LINE, IRSD_QTY, NULL AS AO, NULL AS  AAO, NULL AS SEQ, NULL AS ACTION_DATE, NULL AS REMARK " & _
                    '            "FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " & _
                    '            "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                    '            "INNER JOIN STATUS_MSTR ON IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " & _
                    '            "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " & _
                    '            "LEFT JOIN COMPANY_SECTION ON IRSM_IRS_COY_ID = CS_COY_ID AND IRSM_IRS_SECTION = CS_SEC_CODE " & _
                    '            "WHERE IRSM_IRS_COY_ID = @prmCoyID AND IRSM_IRS_NO = @prmMRSNo " & _
                    '            "UNION ALL " & _
                    '            "SELECT '2' AS TB, NULL AS IRSM_IRS_NO, NULL AS IRSM_IRS_DATE, NULL AS IRSM_IRS_URGENT, NULL AS IRSM_IRS_APPROVED_DATE, NULL AS IRSM_IRS_REF_NO, " & _
                    '            "NULL AS IRSM_IRS_REQUESTOR_NAME, NULL AS IRSM_IRS_ISSUE_TO, NULL AS CDM_DEPT_NAME, " & _
                    '            "NULL AS IRSM_IRS_SECTION, NULL AS IRSM_IRS_REMARK, NULL AS STATUS_DESC, NULL AS IM_ITEM_CODE, NULL AS IRSD_INVENTORY_NAME, " & _
                    '            "NULL AS IRSD_IRS_LINE, NULL AS IRSD_QTY, " & _
                    '            "UM_A.UM_USER_NAME AS AO, UM_B.UM_USER_NAME AS AAO, IRA_SEQ AS SEQ, IRA_ACTION_DATE AS ACTION_DATE, " & _
                    '            "IRA_AO_REMARK AS REMARK " & _
                    '            "FROM IR_APPROVAL " & _
                    '            "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRA_IR_INDEX = IRM_IR_INDEX " & _
                    '            "LEFT JOIN USER_MSTR UM_A ON IRM_IR_COY_ID = UM_A.UM_COY_ID AND IRA_AO = UM_A.UM_USER_ID " & _
                    '            "LEFT JOIN USER_MSTR UM_B ON IRM_IR_COY_ID = UM_B.UM_COY_ID AND IRA_A_AO = UM_B.UM_USER_ID " & _
                    '            "WHERE IRM_IR_NO = @prmIRNo AND IRM_IR_COY_ID = @prmCoyID"

                End If

            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmMRSNo", Request.QueryString("MRSNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmIRNo", strIRNo))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmIRIndex", strIRIndex))
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                strUrgent = ds.Tables(0).Rows(0)("IRSM_IRS_URGENT")
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewMRSSlip_DataSetPreviewMRSSlip", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If strStatus = "2" Or strStatus = "3" Or strStatus = "4" Or strStatus = "5" Or strStatus = "7" Then
                localreport.ReportPath = dispatcher.direct("Report", "PreviewMRSSlip.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"

            Else
                localreport.ReportPath = dispatcher.direct("Report", "PreviewMRSSlip_WithoutLot.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"

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

                    Case "pmcoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Session("CompanyName").ToString)

                    Case "pmurgent"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUrgent)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
                            "<DeviceInfo>" + _
                                "<OutputFormat>EMF</OutputFormat>" + _
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "MRS_" & Request.QueryString(Trim("MRSNo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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

End Class