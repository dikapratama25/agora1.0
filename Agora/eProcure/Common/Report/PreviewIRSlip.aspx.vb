
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewIRSlip1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewIRSlip()
    End Sub

    Private Sub PreviewIRSlip()
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
        Dim strLoc, strSLoc, strChkLoc, strUrgent As String
        Dim objInv As New Inventory
        Dim objDb As New EAD.DBCom

        strChkLoc = objDb.GetVal("SELECT CM_LOCATION_STOCK FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("CoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
        objInv = Nothing

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT '1' AS TB, IRM_IR_NO AS IT_TRANS_REF_NO, IRM_CREATED_DATE AS IT_TRANS_DATE, IRM_IR_ISSUE_TO AS IT_ADDITION_INFO, " &
                            "CDM_DEPT_NAME AS IT_ADDITION_INFO1, IRM_IR_REF_NO AS IT_REF_NO, IRM_IR_REMARK AS IT_REMARK, " &
                            "CONCAT(CS_SEC_CODE, ' : ', CS_SEC_NAME) AS SECTION, IRM_IR_URGENT AS URGENT, IRM_IR_REQUESTOR_NAME AS REQUESTOR_NAME, " &
                            "STATUS_DESC, DATE_FORMAT(IRM_IR_APPROVED_DATE, '%d/%m/%Y') AS APPROVED_DATE, " &
                            "IM_ITEM_CODE, IRD_INVENTORY_NAME AS IM_INVENTORY_NAME, IRD_QTY AS IT_TRANS_QTY, IRD_IR_MTHISSUE, IRD_IR_BEFOREAPP_MTHISSUE, IRD_IR_LAST3MTH, IRD_UOM, LM_LOCATION, LM_SUB_LOCATION, " &
                            "NULL AS AO, NULL AS AAO, NULL AS SEQ, NULL AS ACTION_DATE, NULL AS REMARK " &
                            "FROM INVENTORY_REQUISITION_DETAILS " &
                            "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRD_IR_COY_ID = IRM_IR_COY_ID AND IRD_IR_NO = IRM_IR_NO " &
                            "INNER JOIN COMPANY_DEPT_MSTR ON IRM_IR_COY_ID = CDM_COY_ID AND IRM_IR_DEPARTMENT = CDM_DEPT_CODE " &
                            "INNER JOIN INVENTORY_MSTR ON IRD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                            "LEFT JOIN INVENTORY_TRANS ON IRD_INVENTORY_INDEX = IT_INVENTORY_INDEX AND IRD_IR_NO = IT_TRANS_REF_NO AND IT_TRANS_TYPE = 'IR' " &
                            "LEFT JOIN LOCATION_MSTR ON IT_FRM_LOCATION_INDEX = LM_LOCATION_INDEX " &
                            "LEFT JOIN COMPANY_SECTION ON IRM_IR_COY_ID = CS_COY_ID AND IRM_IR_SECTION = CS_SEC_CODE " &
                            "LEFT JOIN STATUS_MSTR ON STATUS_TYPE = 'IR' AND IRM_IR_STATUS = STATUS_NO " &
                            "WHERE IRD_IR_NO = @prmIRNo AND IRD_IR_COY_ID = @prmCoyID " &
                            "UNION ALL " &
                            "SELECT '2' AS TB, NULL AS IT_TRANS_REF_NO, NULL AS IT_TRANS_DATE, NULL AS IT_ADDITION_INFO, NULL AS IT_ADDITION_INFO1, " &
                            "NULL AS IT_REF_NO, NULL AS IT_REMARK, NULL AS SECTION, NULL AS URGENT, NULL AS REQUESTOR_NAME, NULL AS STATUS_DESC, " &
                            "NULL AS APPROVED_DATE, NULL AS IM_ITEM_CODE, NULL AS IM_INVENTORY_NAME, NULL AS IT_TRANS_QTY, " &
                            "NULL AS IRD_IR_MTHISSUE, NULL AS IRD_IR_BEFOREAPP_MTHISSUE, NULL AS IRD_IR_LAST3MTH, NULL AS IRD_UOM, NULL AS LM_LOCATION, NULL AS LM_SUB_LOCATION, " &
                            "UM_A.UM_USER_NAME AS AO, UM_B.UM_USER_NAME AS AAO, IRA_SEQ AS SEQ, IRA_ACTION_DATE AS ACTION_DATE, IRA_AO_REMARK AS REMARK " &
                            "FROM IR_APPROVAL " &
                            "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRA_IR_INDEX = IRM_IR_INDEX " &
                            "LEFT JOIN USER_MSTR UM_A ON IRM_IR_COY_ID = UM_A.UM_COY_ID AND IRA_AO = UM_A.UM_USER_ID " &
                            "LEFT JOIN USER_MSTR UM_B ON IRM_IR_COY_ID = UM_B.UM_COY_ID AND IRA_A_AO = UM_B.UM_USER_ID " &
                            "WHERE IRM_IR_NO = @prmIRNo AND IRM_IR_COY_ID = @prmCoyID "

                '.CommandText = "SELECT IT_TRANS_REF_NO,IT_TRANS_DATE,IT_ADDITION_INFO,IT_ADDITION_INFO1," _
                '            & "IT_REF_NO,IT_REMARK,IM_ITEM_CODE,IT_INVENTORY_NAME AS IM_INVENTORY_NAME,IT_TRANS_QTY," _
                '            & "IT_FRM_LOCATION_INDEX, LM_LOCATION, LM_SUB_LOCATION " _
                '            & "FROM inventory_trans " _
                '            & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                '            & "INNER JOIN location_mstr ON IT_FRM_LOCATION_INDEX=LM_LOCATION_INDEX " _
                '            & "WHERE IT_TRANS_REF_NO=@prmIRNo " _
                '            & "AND IT_TRANS_TYPE = 'IR' AND IM_COY_ID = @prmCoyID"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmIRNo", Request.QueryString("IRNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("CoyID")))
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                strUrgent = ds.Tables(0).Rows(0)("URGENT")
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewIRSlip_DataSetPreviewIRSlip", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If strChkLoc = "N" Then
                localreport.ReportPath = dispatcher.direct("Report", "PreviewIRSlip_WithoutLoc.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
            Else
                If intLocIndicator > 1 Then 'Sub Location is defined
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewIRSlip.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewIRSlip_WithoutSubLoc.rdlc", "Report") ' appPath & "DO\PreveiwDO-FTN.rdlc"
                End If

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
                    Case "pmloc"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strLoc)

                    Case "pmsubloc"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSLoc)

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
            Dim strFileName As String = "IR_" & Request.QueryString(Trim("IRNo")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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