Imports System.Configuration
Imports System.Web
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class Report
        Dim ctx As Web.HttpContext = Web.HttpContext.Current
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))

        'Public Function DisplayVendor() As DataSet
        '    Dim strsql_dvendor As String
        '    Dim dsdvendor As DataSet
        '    'strsql_dvendor = "select CV_S_COY_ID from Company_Vendor, Company_mstr where CV_S_COY_ID = CM_COY_ID and CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        '    strsql_dvendor = "select CV_S_COY_ID from Company_Vendor, Company_mstr where CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and CV_S_COY_ID = CM_COY_ID"

        '    dsdvendor = objDb.FillDs(strsql_dvendor)
        '    DisplayVendor = dsdvendor
        'End Function

        Public Function DisplayVendor(ByVal pHubLevel As Boolean) As DataSet

            Dim strComp As String
            If pHubLevel Then
                strComp = ctx.Session("CompanyIdToken")
            Else
                strComp = ctx.Session("CompanyId")
            End If

            Dim strsql_dvendor As String
            Dim dsdvendor As DataSet

            strsql_dvendor = "select CV_S_COY_ID from Company_Vendor, Company_mstr where CV_B_COY_ID='" & strComp & "' and CV_S_COY_ID = CM_COY_ID"

            dsdvendor = objDb.FillDs(strsql_dvendor)
            DisplayVendor = dsdvendor
        End Function

        Public Function getReportUrl(ByVal index As Integer) As String
            Dim strsql As String
            Dim dsdvendor As DataSet

            ' strsql = "select rm_report_url from report_mstr rms, report_matrix rma " & _
            '         "where rma.rm_coy_id='" & HttpContext.Current.Session("CompanyId") & "' and rma.rm_index=" & index & " and " & _
            '        "rms.rm_report_index=rma.rm_report_index"

            strsql = "select rm_report_url from report_mstr rms, report_matrix rma where rma.rm_coy_id='" & HttpContext.Current.Session("CompanyId") & "'and rms.rm_report_index=" & index & " and rms.rm_report_index=rma.rm_report_index"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                getReportUrl = tDS.Tables(0).Rows(j).Item("rm_report_url").ToString.Trim
            Next
        End Function

        Public Function getReportType(ByVal strCoyID As String, Optional ByVal strRptType As String = "", Optional ByVal strReport As String = "") As DataSet
            Dim strsql, strUserId As String
            Dim dsReport As DataSet
            Dim blnRA, blnRV As Boolean

            strUserId = HttpContext.Current.Session("UserId")

            If strRptType = "" Then
                'Get Fixed Role
                strsql = "SELECT '*' FROM USERS_USRGRP INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID WHERE UU_COY_ID = '" & strCoyID & "' AND UU_USER_ID = '" & strUserId & "' AND UGM_FIXED_ROLE = 'Report Administrator'"

                If objDb.GetVal(strsql) <> "" Then
                    strsql = "SELECT * FROM REPORT_MSTR RMS, REPORT_MATRIX RMA WHERE RMA.RM_COY_ID='" & strCoyID & "' AND RMS.RM_REPORT_INDEX=RMA.RM_REPORT_INDEX ORDER BY RMS.RM_REPORT_NAME "
                Else
                    strsql = "SELECT '*' FROM USERS_USRGRP INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID WHERE UU_COY_ID = '" & strCoyID & "' AND UU_USER_ID = '" & strUserId & "' AND UGM_FIXED_ROLE = 'Report Viewer'"

                    If objDb.GetVal(strsql) <> "" Then
                        strsql = "SELECT * FROM REPORT_MSTR RMS INNER JOIN REPORT_MATRIX RMA ON  RMS.RM_REPORT_INDEX = RMA.RM_REPORT_INDEX WHERE RMA.RM_COY_ID='" & strCoyID & "' AND RMS.RM_REPORT_INDEX IN " &
                                "(SELECT RUM_REPORT_INDEX FROM REPORT_USER_MATRIX WHERE RUM_COY_ID = '" & strCoyID & "' AND RUM_USER_ID = '" & strUserId & "') ORDER BY RMS.RM_REPORT_NAME "
                    Else
                        strsql = "SELECT * FROM REPORT_MSTR RMS, REPORT_MATRIX RMA WHERE RMA.RM_COY_ID='" & strCoyID & "' AND RMS.RM_REPORT_INDEX=RMA.RM_REPORT_INDEX ORDER BY RMS.RM_REPORT_NAME "
                    End If
                End If

                'Michelle (3/5/2012) - Issue 1603
            ElseIf strRptType = "I" Then 'ie IPP reports
                strsql = "SELECT * FROM REPORT_MSTR RMS WHERE RMS.RM_REPORT_TYPE = '" & strRptType & "' " &
                "AND RMS.RM_REPORT_NAME IN " & strReport & " " &
                "ORDER BY RMS.RM_REPORT_NAME "
            Else
                strsql = "select * from report_mstr rms, report_matrix rma where rma.rm_coy_id='" & strCoyID & "' and rms.rm_report_index=rma.rm_report_index and rms.rm_report_type = '" & strRptType & "' ORDER BY rms.RM_REPORT_NAME "
            End If

            dsReport = objDb.FillDs(strsql)
            getReportType = dsReport

        End Function

        Public Function getReportMatrix(ByVal strCoyId As String) As DataSet
            Dim strsql As String
            Dim ds As DataSet
            strsql = "SELECT R.RM_REPORT_INDEX, R.RM_REPORT_NAME, 1 AS CHK, "
            strsql &= "(SELECT COUNT(*) FROM REPORT_MATRIX WHERE RM_COY_ID = '" & Common.Parse(strCoyId) & "') AS CNT "
            strsql &= "FROM REPORT_MSTR AS R LEFT JOIN REPORT_MATRIX AS X ON R.RM_REPORT_INDEX = X.RM_REPORT_INDEX "
            strsql &= "WHERE RM_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strsql &= "UNION "
            strsql &= "SELECT R.RM_REPORT_INDEX, R.RM_REPORT_NAME, 0 AS CHK, "
            strsql &= "(SELECT COUNT(*) FROM REPORT_MATRIX WHERE RM_COY_ID = '" & Common.Parse(strCoyId) & "') AS CNT "
            strsql &= "FROM REPORT_MSTR AS R LEFT JOIN REPORT_MATRIX AS X ON R.RM_REPORT_INDEX = X.RM_REPORT_INDEX "
            strsql &= "WHERE R.RM_REPORT_INDEX NOT IN "
            strsql &= "(SELECT RM_REPORT_INDEX FROM REPORT_MATRIX WHERE RM_COY_ID = '" & Common.Parse(strCoyId) & "') "
            ds = objDb.FillDs(strsql)
            getReportMatrix = ds
        End Function
        Public Function getReportMatrixByType(ByVal strByType As String, Optional ByVal strParam1 As String = "", Optional ByVal strParam2 As String = "") As DataSet
            Dim strsql As String
            Dim ds As DataSet

            If strByType = "U" Then 'Get all the reports by User
                strsql = "SELECT DISTINCT UM_USER_NAME, UM_USER_ID "
                strsql &= "FROM REPORT_USER_MATRIX INNER JOIN USER_MSTR ON RUM_USER_ID = UM_USER_ID AND RUM_COY_ID = UM_COY_ID "
                strsql &= "WHERE RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

                If strParam1 <> "" Then
                    strsql &= " AND UPPER(UM_USER_Name)" & Common.ParseSQL("*" & strParam1 & "*")
                End If
                If strParam2 <> "" Then
                    strsql &= " AND UPPER(UM_USER_ID)" & Common.ParseSQL("*" & strParam2 & "*")
                End If
                strsql &= " ORDER BY UM_USER_NAME"
            ElseIf strByType = "R" Then 'Get all the reports by Report Name
                strsql = "SELECT RM.RM_REPORT_INDEX, RM_REPORT_NAME "
                strsql &= "FROM REPORT_MATRIX RM INNER JOIN REPORT_MSTR ON REPORT_MSTR.RM_REPORT_INDEX = RM.RM_REPORT_INDEX "
                strsql &= "WHERE RM.RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                If strParam1 <> "" Then
                    strsql &= " AND UPPER(RM_REPORT_NAME)" & Common.ParseSQL("*" & strParam1 & "*")
                End If
                strsql &= " ORDER BY RM_REPORT_NAME"
            ElseIf strByType = "URpt" Then 'Get all the reports for the particular user
                strsql = "SELECT RM_REPORT_NAME "
                strsql &= "FROM REPORT_USER_MATRIX INNER JOIN REPORT_MSTR ON RM_REPORT_INDEX = RUM_REPORT_INDEX "
                strsql &= "WHERE RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                If strParam1 <> "" Then
                    strsql &= " AND UPPER(RUM_USER_ID) " & Common.ParseSQL(strParam1)
                End If
                strsql &= " ORDER BY RM_REPORT_NAME"
            ElseIf strByType = "RUser" Then 'Get all the users for the particular report
                strsql = "SELECT DISTINCT UM_USER_NAME, UM_USER_ID "
                strsql &= "FROM REPORT_USER_MATRIX INNER JOIN USER_MSTR ON RUM_USER_ID = UM_USER_ID AND RUM_COY_ID = UM_COY_ID "
                strsql &= "WHERE RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                If strParam1 <> "" Then
                    strsql &= " AND UPPER(RUM_REPORT_INDEX) " & Common.ParseSQL(strParam1)
                End If
                strsql &= " ORDER BY UM_USER_NAME"
            End If
            ds = objDb.FillDs(strsql)
            getReportMatrixByType = ds
        End Function

        Function DelRptMatrix(ByVal strByType As String, ByVal strParam As String) As Boolean
            Dim strSQL As String
            Dim query(0) As String
            If strByType = "R" Then
                strSQL = "DELETE FROM REPORT_USER_MATRIX WHERE RUM_REPORT_INDEX= '" & strParam & "' AND RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            ElseIf strByType = "U" Then
                strSQL = "DELETE FROM REPORT_USER_MATRIX WHERE RUM_USER_ID= '" & strParam & "' AND RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            End If

            Common.Insert2Ary(query, strSQL)
            If objDb.BatchExecute(query) Then
                DelRptMatrix = True
            Else
                DelRptMatrix = False
            End If

        End Function

        Public Function updateReportMatrix(ByVal strCoyId As String, ByVal dt As DataTable) As Integer
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            strsql = "DELETE FROM REPORT_MATRIX WHERE RM_COY_ID = '" & Common.Parse(strCoyId) & "' "
            Common.Insert2Ary(strAryQuery, strsql)
            For i = 0 To dt.Rows.Count - 1
                strsql = "INSERT INTO REPORT_MATRIX(RM_COY_ID, RM_REPORT_INDEX) VALUES ("
                strsql &= "'" & Common.Parse(strCoyId) & "', "
                strsql &= Common.Parse(dt.Rows(i)("Index")) & ")"
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    updateReportMatrix = WheelMsgNum.Save
                Else
                    updateReportMatrix = WheelMsgNum.NotSave
                End If
            End If
        End Function

        Function BindListBox_ReportMatrixSearchDataUsr() As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, CONCAT(UM_USER_ID, ' : ', UM_USER_NAME) AS THREE " &
                    "FROM USER_MSTR " &
                    "INNER JOIN USERS_USRGRP ON UM_COY_ID = UU_COY_ID AND UM_USER_ID = UU_USER_ID " &
                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID " &
                    "WHERE UM_DELETED <> 'Y' AND UM_STATUS= 'A' " &
                    "AND UM_COY_ID = '" & strCoyId & "' AND UGM_FIXED_ROLE <> 'Report Administrator' " &
                    "ORDER BY UM_USER_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function BindListBox_ReportMatrixSearchDataRpt() As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT RM.RM_REPORT_INDEX, RM_REPORT_NAME FROM REPORT_MSTR RM " &
                    "INNER JOIN REPORT_MATRIX RMX ON RM.RM_REPORT_INDEX = RMX.RM_REPORT_INDEX " &
                    "WHERE RMX.RM_COY_ID = '" & strCoyId & "' " &
                    "ORDER BY RM_REPORT_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        'To get those users that not assigned to report
        Function BindListBox_ReportMatrixSearchUserData(ByVal strRptIndex As String) As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, CONCAT(UM_USER_ID, ' : ', UM_USER_NAME) AS THREE " &
                    "FROM USER_MSTR " &
                    "INNER JOIN USERS_USRGRP ON UM_COY_ID = UU_COY_ID AND UM_USER_ID = UU_USER_ID " &
                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID " &
                    "WHERE UM_DELETED <> 'Y' AND UM_STATUS= 'A' " &
                    "AND UM_COY_ID = '" & strCoyId & "' AND UGM_FIXED_ROLE <> 'Report Administrator' " &
                    "AND UM_USER_ID NOT IN " &
                    "(SELECT RUM_USER_ID FROM REPORT_USER_MATRIX WHERE RUM_COY_ID = '" & strCoyId & "' AND RUM_REPORT_INDEX = '" & strRptIndex & "') " &
                    "ORDER BY UM_USER_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        'To get those users that assigned to report
        Function BindListBox_ReportMatrixSelectedUserData(ByVal strRptIndex As String) As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, CONCAT(UM_USER_ID, ' : ', UM_USER_NAME) AS THREE " &
                    "FROM USER_MSTR " &
                    "INNER JOIN USERS_USRGRP ON UM_COY_ID = UU_COY_ID AND UM_USER_ID = UU_USER_ID " &
                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID " &
                    "WHERE UM_DELETED <> 'Y' AND UM_STATUS= 'A' " &
                    "AND UM_COY_ID = '" & strCoyId & "' AND UGM_FIXED_ROLE <> 'Report Administrator' " &
                    "AND UM_USER_ID IN " &
                    "(SELECT RUM_USER_ID FROM REPORT_USER_MATRIX WHERE RUM_COY_ID = '" & strCoyId & "' AND RUM_REPORT_INDEX = '" & strRptIndex & "') " &
                    "ORDER BY UM_USER_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        'To get those reports that not tied to user
        Function BindListBox_ReportMatrixSearchReportData(ByVal strUserId As String) As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT RM.RM_REPORT_INDEX, RM_REPORT_NAME FROM REPORT_MSTR RM " &
                    "INNER JOIN REPORT_MATRIX RMX ON RM.RM_REPORT_INDEX = RMX.RM_REPORT_INDEX " &
                    "WHERE RMX.RM_COY_ID = '" & strCoyId & "' AND RMX.RM_REPORT_INDEX NOT IN " &
                    "(SELECT RUM_REPORT_INDEX FROM REPORT_USER_MATRIX WHERE RUM_COY_ID = '" & strCoyId & "' AND RUM_USER_ID = '" & strUserId & "') " &
                    "ORDER BY RM_REPORT_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        'To get those reports that tied to user
        Function BindListBox_ReportMatrixSelectedReportData(ByVal strUserId As String) As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT RM.RM_REPORT_INDEX, RM_REPORT_NAME FROM REPORT_MSTR RM " &
                    "INNER JOIN REPORT_MATRIX RMX ON RM.RM_REPORT_INDEX = RMX.RM_REPORT_INDEX " &
                    "WHERE RMX.RM_COY_ID = '" & strCoyId & "' AND RMX.RM_REPORT_INDEX IN " &
                    "(SELECT RUM_REPORT_INDEX FROM REPORT_USER_MATRIX WHERE RUM_COY_ID = '" & strCoyId & "' AND RUM_USER_ID = '" & strUserId & "') " &
                    "ORDER BY RM_REPORT_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function DelReportMatrixAsg(ByVal strValue As String, ByVal strAction As String)
            Dim strSQL As String
            Dim query(0) As String
            If strAction = "r" Then
                strSQL = "DELETE FROM REPORT_USER_MATRIX WHERE RUM_REPORT_INDEX= '" & strValue & "' AND RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            ElseIf strAction = "u" Then
                strSQL = "DELETE FROM REPORT_USER_MATRIX WHERE RUM_USER_ID= '" & strValue & "' AND RUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            End If

            Common.Insert2Ary(query, strSQL)
            objDb.BatchExecute(query)
        End Function

        Function AddReportMatrixAsg(ByVal strRptIndex As String, ByVal strUserId As String)
            Dim strSQL As String
            Dim query(0) As String
            strSQL = "INSERT INTO REPORT_USER_MATRIX (RUM_COY_ID, RUM_REPORT_INDEX, RUM_USER_ID) VALUES ('" & HttpContext.Current.Session("CompanyId") & "','" & strRptIndex & "','" & strUserId & "')"
            Common.Insert2Ary(query, strSQL)
            objDb.BatchExecute(query)
        End Function

        Function CheckReportMatrixAsgUser(ByVal strRptIndex As String, ByVal strUserId As String) As DataView
            Dim strSql, strCoyId As String
            Dim drw As DataView

            strCoyId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT '*' FROM REPORT_USER_MATRIX " &
                    "WHERE RUM_COY_ID = '" & strCoyId & "' AND RUM_USER_ID = '" & strUserId & "' AND RUM_REPORT_INDEX = '" & strRptIndex & "'"
            drw = objDb.GetView(strSql)
            Return drw
        End Function
    End Class

End Namespace
