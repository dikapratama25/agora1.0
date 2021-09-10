Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports System.Security.Cryptography

Namespace AgoraLegacy

    Public Class eProcStaffClaim
        Dim objDb As New EAD.DBCom
        'Dim strCompId As String = HttpContext.Current.Session("CompanyId")
        'Dim strUserId As String = HttpContext.Current.Session("UserId")

        'Chee Hong - 16/11/2015 - Function to retrieve hardship details
        Public Function GetHardshipDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCHD_DATE_FROM, '%d/%m/%Y') AS SCHD_DATE_FROM, CAST(TIME_FORMAT(SCHD_FROM_TIME, '%H:%i') AS CHAR(5)) AS SCHD_FROM_TIME, DATE_FORMAT(SCHD_DATE_TO, '%d/%m/%Y') AS SCHD_DATE_TO, CAST(TIME_FORMAT(SCHD_TO_TIME, '%H:%i') AS CHAR(5)) AS SCHD_TO_TIME, " &
                    "SCHD_PROJ_CODE, SCHD_PURPOSE, SCHD_BREAK_HOUR, SCHD_TOTAL_HOUR, SCHD_RATE, SCHD_AMOUNT, " &
                    "SCHD_CSR_NO, SCHD_CALL_DAY, SCHD_CALL_PERIOD, SCHD_CALL_FOLLOW_UP " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_HARDSHIP_DETAIL ON SCHD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCHD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCHD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveHardship(ByVal dsHs As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim dsApp As DataSet
            Dim i, j As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsHs.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)


            For i = 0 To dsHs.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_HARDSHIP_DETAIL (SCHD_CLAIM_DOC_NO, SCHD_COY_ID, SCHD_LINE_NO, SCHD_DATE_FROM, SCHD_DATE_TO, " &
                        "SCHD_FROM_TIME, SCHD_TO_TIME, SCHD_PROJ_BASED, SCHD_PROJ_CODE, SCHD_PURPOSE, SCHD_BREAK_HOUR, SCHD_TOTAL_HOUR, SCHD_RATE, SCHD_AMOUNT, " &
                        "SCHD_CSR_NO, SCHD_CALL_DAY, SCHD_CALL_PERIOD, SCHD_CALL_FOLLOW_UP) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsHs.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsHs.Tables(1).Rows(i)("DATE_FROM")) & ", " & Common.ConvertDate(dsHs.Tables(1).Rows(i)("DATE_TO")) & ", " &
                        "'" & dsHs.Tables(1).Rows(i)("FROM_TIME") & "', '" & dsHs.Tables(1).Rows(i)("TO_TIME") & "', '" & dsHs.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsHs.Tables(1).Rows(i)("PROJ_CODE")) & "', " &
                        "'" & Common.Parse(dsHs.Tables(1).Rows(i)("PURPOSE")) & "', " & dsHs.Tables(1).Rows(i)("BREAK_HOUR") & ", " & dsHs.Tables(1).Rows(i)("TOTAL_HOUR") & ", " & dsHs.Tables(1).Rows(i)("RATE") & ", " &
                        dsHs.Tables(1).Rows(i)("AMOUNT") & ", '" & Common.Parse(dsHs.Tables(1).Rows(i)("CSR_NO")) & "', " & IIf(dsHs.Tables(1).Rows(i)("CALL_DAY") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_DAY")) & ", " &
                        IIf(dsHs.Tables(1).Rows(i)("CALL_PERIOD") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_PERIOD")) & ", " & IIf(dsHs.Tables(1).Rows(i)("CALL_FOLLOW_UP") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_FOLLOW_UP")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveHardship, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveHardship = True
            Else
                SaveHardship = False
            End If
        End Function

        Public Function UpdateHardship(ByVal dsHs As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String)
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsHs.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_HARDSHIP_DETAIL " &
                    "WHERE SCHD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCHD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsHs.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_HARDSHIP_DETAIL (SCHD_CLAIM_DOC_NO, SCHD_COY_ID, SCHD_LINE_NO, SCHD_DATE_FROM, SCHD_DATE_TO, " &
                       "SCHD_FROM_TIME, SCHD_TO_TIME, SCHD_PROJ_BASED, SCHD_PROJ_CODE, SCHD_PURPOSE, SCHD_BREAK_HOUR, SCHD_TOTAL_HOUR, SCHD_RATE, SCHD_AMOUNT, " &
                       "SCHD_CSR_NO, SCHD_CALL_DAY, SCHD_CALL_PERIOD, SCHD_CALL_FOLLOW_UP) " &
                       "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsHs.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsHs.Tables(1).Rows(i)("DATE_FROM")) & ", " & Common.ConvertDate(dsHs.Tables(1).Rows(i)("DATE_TO")) & ", " &
                       "'" & dsHs.Tables(1).Rows(i)("FROM_TIME") & "', '" & dsHs.Tables(1).Rows(i)("TO_TIME") & "', '" & dsHs.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsHs.Tables(1).Rows(i)("PROJ_CODE")) & "', " &
                       "'" & Common.Parse(dsHs.Tables(1).Rows(i)("PURPOSE")) & "', " & dsHs.Tables(1).Rows(i)("BREAK_HOUR") & ", " & dsHs.Tables(1).Rows(i)("TOTAL_HOUR") & ", " & dsHs.Tables(1).Rows(i)("RATE") & ", " &
                       dsHs.Tables(1).Rows(i)("AMOUNT") & ", '" & Common.Parse(dsHs.Tables(1).Rows(i)("CSR_NO")) & "', " & IIf(dsHs.Tables(1).Rows(i)("CALL_DAY") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_DAY")) & ", " &
                       IIf(dsHs.Tables(1).Rows(i)("CALL_PERIOD") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_PERIOD")) & ", " & IIf(dsHs.Tables(1).Rows(i)("CALL_FOLLOW_UP") = "", "NULL", dsHs.Tables(1).Rows(i)("CALL_FOLLOW_UP")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveHardship, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateHardship = True
            Else
                UpdateHardship = False
            End If
        End Function

        'Chee Hong - 17/11/2015 - Function to retrieve overtime details
        Public Function GetOverTimeDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCOD_DATE_FROM, '%d/%m/%Y') AS SCOD_DATE_FROM, CAST(TIME_FORMAT(SCOD_FROM_TIME, '%H:%i') AS CHAR(5)) AS SCOD_FROM_TIME, " &
                    "DATE_FORMAT(SCOD_DATE_TO, '%d/%m/%Y') AS SCOD_DATE_TO, CAST(TIME_FORMAT(SCOD_TO_TIME, '%H:%i') AS CHAR(5)) AS SCOD_TO_TIME, " &
                    "SCOD_PURPOSE, CAST(TIME_FORMAT(SCOD_TOTAL_HOUR_MIN, '%H:%i') AS CHAR(5)) AS SCOD_TOTAL_HOUR_MIN, SCOD_TIMES, SCOD_MEAL_ALLOWANCE " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_OVERTIME_DETAIL ON SCOD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCOD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCOD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveOverTime(ByVal dsOT As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim dsApp As DataSet
            Dim i, j As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsOT.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)


            For i = 0 To dsOT.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_OVERTIME_DETAIL (SCOD_CLAIM_DOC_NO, SCOD_COY_ID, SCOD_LINE_NO, SCOD_DATE_FROM, " &
                        "SCOD_FROM_TIME, SCOD_DATE_TO, SCOD_TO_TIME, SCOD_PURPOSE, SCOD_TOTAL_HOUR_MIN, SCOD_TIMES, SCOD_MEAL_ALLOWANCE) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsOT.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsOT.Tables(1).Rows(i)("DATE_FROM")) & ", " &
                        "'" & dsOT.Tables(1).Rows(i)("FROM_TIME") & "', " & Common.ConvertDate(dsOT.Tables(1).Rows(i)("DATE_TO")) & ", '" & dsOT.Tables(1).Rows(i)("TO_TIME") & "', '" & Common.Parse(dsOT.Tables(1).Rows(i)("PURPOSE")) & "', " &
                        "'" & dsOT.Tables(1).Rows(i)("TOTAL_HOUR_MIN") & "', '" & dsOT.Tables(1).Rows(i)("TIMES") & "', " &
                        IIf(dsOT.Tables(1).Rows(i)("MEAL_ALLOWANCE") = "", "NULL", dsOT.Tables(1).Rows(i)("MEAL_ALLOWANCE")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveOvertime, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveOverTime = True
            Else
                SaveOverTime = False
            End If
        End Function

        Public Function UpdateOverTime(ByVal dsOT As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String)
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsOT.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_OVERTIME_DETAIL " &
                    "WHERE SCOD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCOD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsOT.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_OVERTIME_DETAIL (SCOD_CLAIM_DOC_NO, SCOD_COY_ID, SCOD_LINE_NO, SCOD_DATE_FROM, " &
                        "SCOD_FROM_TIME, SCOD_DATE_TO, SCOD_TO_TIME, SCOD_PURPOSE, SCOD_TOTAL_HOUR_MIN, SCOD_TIMES, SCOD_MEAL_ALLOWANCE) " &
                        "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsOT.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsOT.Tables(1).Rows(i)("DATE_FROM")) & ", " &
                        "'" & dsOT.Tables(1).Rows(i)("FROM_TIME") & "', " & Common.ConvertDate(dsOT.Tables(1).Rows(i)("DATE_TO")) & ", '" & dsOT.Tables(1).Rows(i)("TO_TIME") & "', '" & Common.Parse(dsOT.Tables(1).Rows(i)("PURPOSE")) & "', " &
                        "'" & dsOT.Tables(1).Rows(i)("TOTAL_HOUR_MIN") & "', '" & dsOT.Tables(1).Rows(i)("TIMES") & "', " &
                        IIf(dsOT.Tables(1).Rows(i)("MEAL_ALLOWANCE") = "", "NULL", dsOT.Tables(1).Rows(i)("MEAL_ALLOWANCE")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveOvertime, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateOverTime = True
            Else
                UpdateOverTime = False
            End If
        End Function

        'Chee Hong - 18/11/2015 - Function to retrieve allowance details
        Public Function GetAllowanceDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCAD_DATE_FROM, '%d/%m/%Y') AS SCAD_DATE_FROM, DATE_FORMAT(SCAD_DATE_TO, '%d/%m/%Y') AS SCAD_DATE_TO, " &
                    "SCAD_PROJ_CODE, SCAD_PURPOSE, SCAD_STANDBY_ALLOW_RATE, SCAD_SHIFT_ALLOW_RATE " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_ALLOWANCE_DETAIL ON SCAD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCAD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCAD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveAllowance(ByVal dsAl As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim i, j As Integer

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsAl.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsAl.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_ALLOWANCE_DETAIL (SCAD_CLAIM_DOC_NO, SCAD_COY_ID, SCAD_LINE_NO, SCAD_DATE_FROM, " &
                        "SCAD_DATE_TO, SCAD_PROJ_BASED, SCAD_PROJ_CODE, SCAD_PURPOSE, SCAD_STANDBY_ALLOW_RATE, SCAD_SHIFT_ALLOW_RATE) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsAl.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsAl.Tables(1).Rows(i)("DATE_FROM")) & ", " &
                        Common.ConvertDate(dsAl.Tables(1).Rows(i)("DATE_TO")) & ", '" & dsAl.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsAl.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsAl.Tables(1).Rows(i)("PURPOSE")) & "', " &
                        "'" & dsAl.Tables(1).Rows(i)("STANDBY_ALLOW_RATE") & "', '" & dsAl.Tables(1).Rows(i)("SHIFT_ALLOW_RATE") & "')"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveAllowance, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveAllowance = True
            Else
                SaveAllowance = False
            End If
        End Function

        Public Function UpdateAllowance(ByVal dsAl As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String)
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsAl.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_ALLOWANCE_DETAIL " &
                    "WHERE SCAD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCAD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsAl.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_ALLOWANCE_DETAIL (SCAD_CLAIM_DOC_NO, SCAD_COY_ID, SCAD_LINE_NO, SCAD_DATE_FROM, " &
                        "SCAD_DATE_TO, SCAD_PROJ_BASED, SCAD_PROJ_CODE, SCAD_PURPOSE, SCAD_STANDBY_ALLOW_RATE, SCAD_SHIFT_ALLOW_RATE) " &
                        "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsAl.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsAl.Tables(1).Rows(i)("DATE_FROM")) & ", " &
                        Common.ConvertDate(dsAl.Tables(1).Rows(i)("DATE_TO")) & ", '" & dsAl.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsAl.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsAl.Tables(1).Rows(i)("PURPOSE")) & "', " &
                        "'" & dsAl.Tables(1).Rows(i)("STANDBY_ALLOW_RATE") & "', '" & dsAl.Tables(1).Rows(i)("SHIFT_ALLOW_RATE") & "')"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveAllowance, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateAllowance = True
            Else
                UpdateAllowance = False
            End If
        End Function

        'Chee Hong - 29/12/2015 - Function to retrieve outstation details
        Public Function GetOutstationDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCOOD_DEPART_DATE, '%d/%m/%Y') AS SCOOD_DEPART_DATE, DATE_FORMAT(SCOOD_RETURN_DATE, '%d/%m/%Y') AS SCOOD_RETURN_DATE, " &
                    "CAST(TIME_FORMAT(SCOOD_DEPART_TIME, '%H:%i') AS CHAR(5)) AS SCOOD_DEPART_TIME, CAST(TIME_FORMAT(SCOOD_RETURN_TIME, '%H:%i') AS CHAR(5)) AS SCOOD_RETURN_TIME, " &
                    "SCOOD_NO_OF_DAY, SCOOD_DESTINATION, SCOOD_PROJ_BASED, SCOOD_PROJ_CODE, SCOOD_CURRENCY_CODE, SCOOD_EXCHANGE_RATE, " &
                    "SCOOD_MEAL_RATE, SCOOD_MEAL_ENTITLED, SCOOD_FREE_MEAL, SCOOD_ACTUAL_MEAL_CLAIM, SCOOD_TOTAL_SUB_ALLW_CLAIM, " &
                    "SCOOD_TOTAL_ACC_CLAIM, SCOOD_GST_AMT, SCOOD_TOTAL_ACC_ALLW, SCOOD_TOTAL_CLAIM_AMT " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_OVER_OUTSTATION_DETAIL ON SCOOD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCOOD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCOOD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveOutstation(ByVal dsOut As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim i, j As Integer

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsOut.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsOut.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_OVER_OUTSTATION_DETAIL (SCOOD_CLAIM_DOC_NO, SCOOD_COY_ID, SCOOD_LINE_NO, SCOOD_DEPART_DATE, " &
                        "SCOOD_DEPART_TIME, SCOOD_RETURN_DATE, SCOOD_RETURN_TIME, SCOOD_NO_OF_DAY, SCOOD_DESTINATION, SCOOD_PROJ_BASED, SCOOD_PROJ_CODE, SCOOD_CURRENCY_CODE, SCOOD_EXCHANGE_RATE, " &
                        "SCOOD_MEAL_RATE, SCOOD_MEAL_ENTITLED, SCOOD_FREE_MEAL, SCOOD_ACTUAL_MEAL_CLAIM, SCOOD_TOTAL_SUB_ALLW_CLAIM, " &
                        "SCOOD_GST_AMT, SCOOD_TOTAL_ACC_CLAIM, SCOOD_TOTAL_ACC_ALLW, SCOOD_TOTAL_CLAIM_AMT) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsOut.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsOut.Tables(1).Rows(i)("DEPART_DATE")) & ", " &
                        "'" & dsOut.Tables(1).Rows(i)("DEPART_TIME") & "'," & Common.ConvertDate(dsOut.Tables(1).Rows(i)("RETURN_DATE")) & ", '" & dsOut.Tables(1).Rows(i)("RETURN_TIME") & "', " &
                        dsOut.Tables(1).Rows(i)("NO_OF_DAY") & ", '" & Common.Parse(dsOut.Tables(1).Rows(i)("DESTINATION")) & "', '" & dsOut.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsOut.Tables(1).Rows(i)("PROJ_CODE")) & "', " &
                        "'" & dsOut.Tables(1).Rows(i)("CURRENCY_CODE") & "', " & dsOut.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsOut.Tables(1).Rows(i)("MEAL_RATE") & ", " & dsOut.Tables(1).Rows(i)("MEAL_ENTITLED") & ", " &
                        dsOut.Tables(1).Rows(i)("FREE_MEAL") & ", " & dsOut.Tables(1).Rows(i)("ACTUAL_MEAL_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_SUB_ALLW_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("GST_AMT") & ", " &
                        dsOut.Tables(1).Rows(i)("TOTAL_ACC_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_ACC_ALLW") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_CLAIM_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveOutstation, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveOutstation = True
            Else
                SaveOutstation = False
            End If
        End Function

        Public Function UpdateOutstation(ByVal dsOut As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String) As Boolean
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsOut.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL " &
                    "WHERE SCOOD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCOOD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsOut.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_OVER_OUTSTATION_DETAIL (SCOOD_CLAIM_DOC_NO, SCOOD_COY_ID, SCOOD_LINE_NO, SCOOD_DEPART_DATE, " &
                        "SCOOD_DEPART_TIME, SCOOD_RETURN_DATE, SCOOD_RETURN_TIME, SCOOD_NO_OF_DAY, SCOOD_DESTINATION, SCOOD_PROJ_BASED, SCOOD_PROJ_CODE, SCOOD_CURRENCY_CODE, SCOOD_EXCHANGE_RATE, " &
                        "SCOOD_MEAL_RATE, SCOOD_MEAL_ENTITLED, SCOOD_FREE_MEAL, SCOOD_ACTUAL_MEAL_CLAIM, SCOOD_TOTAL_SUB_ALLW_CLAIM, " &
                        "SCOOD_GST_AMT, SCOOD_TOTAL_ACC_CLAIM, SCOOD_TOTAL_ACC_ALLW, SCOOD_TOTAL_CLAIM_AMT) " &
                        "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsOut.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsOut.Tables(1).Rows(i)("DEPART_DATE")) & ", " &
                        "'" & dsOut.Tables(1).Rows(i)("DEPART_TIME") & "'," & Common.ConvertDate(dsOut.Tables(1).Rows(i)("RETURN_DATE")) & ", '" & dsOut.Tables(1).Rows(i)("RETURN_TIME") & "', " &
                        dsOut.Tables(1).Rows(i)("NO_OF_DAY") & ", '" & Common.Parse(dsOut.Tables(1).Rows(i)("DESTINATION")) & "', '" & dsOut.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsOut.Tables(1).Rows(i)("PROJ_CODE")) & "', " &
                        "'" & dsOut.Tables(1).Rows(i)("CURRENCY_CODE") & "', " & dsOut.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsOut.Tables(1).Rows(i)("MEAL_RATE") & ", " & dsOut.Tables(1).Rows(i)("MEAL_ENTITLED") & ", " &
                        dsOut.Tables(1).Rows(i)("FREE_MEAL") & ", " & dsOut.Tables(1).Rows(i)("ACTUAL_MEAL_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_SUB_ALLW_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("GST_AMT") & ", " &
                        dsOut.Tables(1).Rows(i)("TOTAL_ACC_CLAIM") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_ACC_ALLW") & ", " & dsOut.Tables(1).Rows(i)("TOTAL_CLAIM_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveOutstation, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateOutstation = True
            Else
                UpdateOutstation = False
            End If
        End Function

        'Chee Hong - 19/11/2015 - Function to retrieve entertainment details
        Public Function GetEntertainDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCED_DATE, '%d/%m/%Y') AS SCED_DATE, SCED_PERSON_ENT, SCED_NO_PAX, SCED_ORG_REP, " &
                    "SCED_CLIENT_STATUS, SCED_PROJ_CODE, SCED_PURPOSE, SCED_TYPE, SCED_MEAL_PLACE, SCED_CURRENCY_CODE, " &
                    "SCED_AMT, SCED_GST_AMT, SCED_TOTAL_AMT, SCED_TAX_INV_TYPE, SCED_EXCHANGE_RATE, SCED_EXCEED_AMT " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_ENT_DETAIL ON SCED_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCED_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCED_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveEntertain(ByVal dsEnt As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim i, j As Integer

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsEnt.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsEnt.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_ENT_DETAIL (SCED_CLAIM_DOC_NO, SCED_COY_ID, SCED_LINE_NO, SCED_DATE, " &
                        "SCED_PERSON_ENT, SCED_NO_PAX, SCED_ORG_REP, SCED_CLIENT_STATUS, SCED_PROJ_BASED, SCED_PROJ_CODE, SCED_PURPOSE, SCED_TYPE, SCED_MEAL_PLACE, " &
                        "SCED_TAX_INV_TYPE, SCED_CURRENCY_CODE, SCED_EXCHANGE_RATE, SCED_AMT, SCED_GST_AMT, SCED_TOTAL_AMT, SCED_EXCEED_AMT) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsEnt.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsEnt.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & Common.Parse(dsEnt.Tables(1).Rows(i)("PERSON_ENT")) & "', " & dsEnt.Tables(1).Rows(i)("NO_PAX") & ", '" & Common.Parse(dsEnt.Tables(1).Rows(i)("ORG_REP")) & "', " &
                        "'" & dsEnt.Tables(1).Rows(i)("CLIENT_STATUS") & "', '" & dsEnt.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("PURPOSE")) & "', '" & dsEnt.Tables(1).Rows(i)("TYPE") & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("MEAL_PLACE")) & "', " &
                        "'" & dsEnt.Tables(1).Rows(i)("TAX_INV_TYPE") & "', '" & dsEnt.Tables(1).Rows(i)("CURRENCY_CODE") & "', " &
                        IIf(dsEnt.Tables(1).Rows(i)("EXCHANGE_RATE") = "", 1, dsEnt.Tables(1).Rows(i)("EXCHANGE_RATE")) & ", " &
                        dsEnt.Tables(1).Rows(i)("AMT") & ", " & dsEnt.Tables(1).Rows(i)("GST_AMT") & ", " & dsEnt.Tables(1).Rows(i)("TOTAL_AMT") & ", " &
                        IIf(dsEnt.Tables(1).Rows(i)("EXCEED_AMT") = "", "NULL", dsEnt.Tables(1).Rows(i)("EXCEED_AMT")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveEntertain, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveEntertain = True
            Else
                SaveEntertain = False
            End If
        End Function

        Public Function UpdateEntertain(ByVal dsEnt As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String)
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsEnt.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_ENT_DETAIL " &
                    "WHERE SCED_CLAIM_DOC_NO = '" & strSCNo & "' AND SCED_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsEnt.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_ENT_DETAIL (SCED_CLAIM_DOC_NO, SCED_COY_ID, SCED_LINE_NO, SCED_DATE, " &
                        "SCED_PERSON_ENT, SCED_NO_PAX, SCED_ORG_REP, SCED_CLIENT_STATUS, SCED_PROJ_BASED, SCED_PROJ_CODE, SCED_PURPOSE, SCED_TYPE, SCED_MEAL_PLACE, " &
                        "SCED_TAX_INV_TYPE, SCED_CURRENCY_CODE, SCED_EXCHANGE_RATE, SCED_AMT, SCED_GST_AMT, SCED_TOTAL_AMT, SCED_EXCEED_AMT) " &
                        "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsEnt.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsEnt.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & Common.Parse(dsEnt.Tables(1).Rows(i)("PERSON_ENT")) & "', " & dsEnt.Tables(1).Rows(i)("NO_PAX") & ", '" & Common.Parse(dsEnt.Tables(1).Rows(i)("ORG_REP")) & "', " &
                        "'" & dsEnt.Tables(1).Rows(i)("CLIENT_STATUS") & "', '" & dsEnt.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("PURPOSE")) & "', '" & dsEnt.Tables(1).Rows(i)("TYPE") & "', '" & Common.Parse(dsEnt.Tables(1).Rows(i)("MEAL_PLACE")) & "', " &
                        "'" & dsEnt.Tables(1).Rows(i)("TAX_INV_TYPE") & "', '" & dsEnt.Tables(1).Rows(i)("CURRENCY_CODE") & "', " &
                        IIf(dsEnt.Tables(1).Rows(i)("EXCHANGE_RATE") = "", 1, dsEnt.Tables(1).Rows(i)("EXCHANGE_RATE")) & ", " &
                        dsEnt.Tables(1).Rows(i)("AMT") & ", " & dsEnt.Tables(1).Rows(i)("GST_AMT") & ", " & dsEnt.Tables(1).Rows(i)("TOTAL_AMT") & ", " &
                        IIf(dsEnt.Tables(1).Rows(i)("EXCEED_AMT") = "", "NULL", dsEnt.Tables(1).Rows(i)("EXCEED_AMT")) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveEntertain, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateEntertain = True
            Else
                UpdateEntertain = False
            End If
        End Function

        'Chee Hong - 23/11/2015 - Function to retrieve transportation details
        Public Function GetTransportDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCTD_DATE, '%d/%m/%Y') AS SCTD_DATE, SCTD_LOC_FROM, SCTD_LOC_TO, SCTD_PROJ_CODE, SCTD_CSR_NO, " &
                    "SCTD_PURPOSE, SCTD_TAX_INV_TYPE, SCTD_CURRENCY_CODE, SCTD_EXCHANGE_RATE, SCTD_PT_AMT, SCTD_PARKING_AMT, " &
                    "SCTD_TOLL_AMT, SCTD_AIRFARE_AMT, ROUND(SCTD_CAR_MILEAGE,1) AS SCTD_CAR_MILEAGE, SCTD_CAR_AMT, " &
                    "ROUND(SCTD_BIKE_MILEAGE,1) AS SCTD_BIKE_MILEAGE, SCTD_BIKE_AMT, ROUND(SCTD_SOG_FE_AMT,1) AS SCTD_SOG_FE_AMT, SCTD_SMART_PAY_AMT, SCTD_FUEL_AMT, SCTD_TOTAL_AMT " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_TRANSPORTATION_DETAIL ON SCTD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCTD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCTD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveTransportation(ByVal dsTrans As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim i, j As Integer

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsTrans.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsTrans.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_TRANSPORTATION_DETAIL (SCTD_CLAIM_DOC_NO, SCTD_COY_ID, SCTD_LINE_NO, SCTD_DATE, " &
                        "SCTD_LOC_FROM, SCTD_LOC_TO, SCTD_PROJ_BASED, SCTD_PROJ_CODE, SCTD_CSR_NO, SCTD_PURPOSE, " &
                        "SCTD_TAX_INV_TYPE, SCTD_CURRENCY_CODE, SCTD_EXCHANGE_RATE, SCTD_PT_AMT, SCTD_PARKING_AMT, SCTD_TOLL_AMT, SCTD_AIRFARE_AMT, " &
                        "SCTD_CAR_MILEAGE, SCTD_CAR_AMT, SCTD_BIKE_MILEAGE, SCTD_BIKE_AMT, SCTD_SOG_FE_AMT, SCTD_SMART_PAY_AMT, SCTD_FUEL_AMT, SCTD_TOTAL_AMT) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsTrans.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsTrans.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("LOC_FROM")) & "', '" & Common.Parse(dsTrans.Tables(1).Rows(i)("LOC_TO")) & "', '" & dsTrans.Tables(1).Rows(i)("PROJ_BASED") & "', " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsTrans.Tables(1).Rows(i)("CSR_NO")) & "', " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("PURPOSE")) & "', '" & dsTrans.Tables(1).Rows(i)("TAX_INV_TYPE") & "', '" & dsTrans.Tables(1).Rows(i)("CURRENCY_CODE") & "', " &
                        dsTrans.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsTrans.Tables(1).Rows(i)("PT_AMT") & ", " & dsTrans.Tables(1).Rows(i)("PARKING_AMT") & ", " &
                        dsTrans.Tables(1).Rows(i)("TOLL_AMT") & ", " & dsTrans.Tables(1).Rows(i)("AIRFARE_AMT") & ", " & dsTrans.Tables(1).Rows(i)("CAR_MILEAGE") & ", " &
                        dsTrans.Tables(1).Rows(i)("CAR_AMT") & ", " & dsTrans.Tables(1).Rows(i)("BIKE_MILEAGE") & ", " & dsTrans.Tables(1).Rows(i)("BIKE_AMT") & ", " &
                        dsTrans.Tables(1).Rows(i)("SOG_FE_AMT") & ", " & dsTrans.Tables(1).Rows(i)("SMART_PAY_AMT") & ", " & dsTrans.Tables(1).Rows(i)("FUEL_AMT") & ", " & dsTrans.Tables(1).Rows(i)("TOTAL_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveTransport, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveTransportation = True
            Else
                SaveTransportation = False
            End If
        End Function

        Public Function UpdateTransportation(ByVal dsTrans As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String) As Boolean
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsTrans.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_TRANSPORTATION_DETAIL " &
                    "WHERE SCTD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCTD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsTrans.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_TRANSPORTATION_DETAIL (SCTD_CLAIM_DOC_NO, SCTD_COY_ID, SCTD_LINE_NO, SCTD_DATE, " &
                        "SCTD_LOC_FROM, SCTD_LOC_TO, SCTD_PROJ_BASED, SCTD_PROJ_CODE, SCTD_CSR_NO, SCTD_PURPOSE, " &
                        "SCTD_TAX_INV_TYPE, SCTD_CURRENCY_CODE, SCTD_EXCHANGE_RATE, SCTD_PT_AMT, SCTD_PARKING_AMT, SCTD_TOLL_AMT, SCTD_AIRFARE_AMT, " &
                        "SCTD_CAR_MILEAGE, SCTD_CAR_AMT, SCTD_BIKE_MILEAGE, SCTD_BIKE_AMT, SCTD_SOG_FE_AMT, SCTD_SMART_PAY_AMT, SCTD_FUEL_AMT, SCTD_TOTAL_AMT) " &
                        "VALUES ('" & strSCNo & "', '" & strCoyID & "', " & dsTrans.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsTrans.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("LOC_FROM")) & "', '" & Common.Parse(dsTrans.Tables(1).Rows(i)("LOC_TO")) & "', '" & dsTrans.Tables(1).Rows(i)("PROJ_BASED") & "', " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsTrans.Tables(1).Rows(i)("CSR_NO")) & "', " &
                        "'" & Common.Parse(dsTrans.Tables(1).Rows(i)("PURPOSE")) & "', '" & dsTrans.Tables(1).Rows(i)("TAX_INV_TYPE") & "', '" & dsTrans.Tables(1).Rows(i)("CURRENCY_CODE") & "', " &
                        dsTrans.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsTrans.Tables(1).Rows(i)("PT_AMT") & ", " & dsTrans.Tables(1).Rows(i)("PARKING_AMT") & ", " &
                        dsTrans.Tables(1).Rows(i)("TOLL_AMT") & ", " & dsTrans.Tables(1).Rows(i)("AIRFARE_AMT") & ", " & dsTrans.Tables(1).Rows(i)("CAR_MILEAGE") & ", " &
                        dsTrans.Tables(1).Rows(i)("CAR_AMT") & ", " & dsTrans.Tables(1).Rows(i)("BIKE_MILEAGE") & ", " & dsTrans.Tables(1).Rows(i)("BIKE_AMT") & ", " &
                        dsTrans.Tables(1).Rows(i)("SOG_FE_AMT") & ", " & dsTrans.Tables(1).Rows(i)("SMART_PAY_AMT") & ", " & dsTrans.Tables(1).Rows(i)("FUEL_AMT") & ", " & dsTrans.Tables(1).Rows(i)("TOTAL_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveTransport, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateTransportation = True
            Else
                UpdateTransportation = False
            End If
        End Function

        'Chee Hong - 04/12/2015 - Function to retrieve other (misc) details
        Public Function GetMiscDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR  " &
                    "INNER JOIN STATUS_MSTR ON STATUS_NO = SCM_STATUS AND STATUS_TYPE = 'SC' " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            strSqlD = "SELECT " &
                    "DATE_FORMAT(SCMD_DATE, '%d/%m/%Y') AS SCMD_DATE, SCMD_TYPE, " &
                    "SCMD_CURRENCY_CODE, SCMD_AMT, SCMD_GST_AMT, SCMD_REMARK, " &
                    "SCMD_TOTAL_AMT, SCMD_TAX_INV_TYPE, SCMD_EXCHANGE_RATE, SCMD_PROJ_CODE " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN STAFF_CLAIM_MISC_DETAIL ON SCMD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCMD_COY_ID = SCM_COY_ID " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " &
                    "ORDER BY SCMD_LINE_NO"

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function SaveMisc(ByVal dsMisc As DataSet, ByRef strSCNo As String, ByRef strSCIndex As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewSCNo As String
            Dim intIncrementNo As Integer = 0
            Dim i, j As Integer

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strSCNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO STAFF_CLAIM_MSTR (SCM_COY_ID, SCM_STAFF_ID, SCM_CLAIM_DOC_NO, SCM_DEPT_CODE, " &
                    "SCM_CREATED_DATE, SCM_ENT_BY, SCM_ENT_DT, SCM_STATUS) " &
                    "VALUES ('" & strCoyID & "', '" & strLoginUser & "', " & strSCNo & ", '" & Common.Parse(dsMisc.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "NOW(), '" & strLoginUser & "', NOW(), " & SCStatus.DraftSC & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsMisc.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_MISC_DETAIL (SCMD_CLAIM_DOC_NO, SCMD_COY_ID, SCMD_LINE_NO, SCMD_DATE, " &
                        "SCMD_PROJ_BASED, SCMD_PROJ_CODE, SCMD_TYPE, SCMD_TAX_INV_TYPE, SCMD_CURRENCY_CODE, SCMD_EXCHANGE_RATE, SCMD_AMT, SCMD_GST_AMT, " &
                        "SCMD_REMARK, SCMD_TOTAL_AMT) " &
                        "VALUES (" & strSCNo & ", '" & strCoyID & "', " & dsMisc.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsMisc.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & dsMisc.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("TYPE")) & "', " &
                        "'" & Common.Parse(dsMisc.Tables(1).Rows(i)("TAX_INV_TYPE")) & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("CURRENCY_CODE")) & "', " &
                        dsMisc.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsMisc.Tables(1).Rows(i)("AMT") & ", " & dsMisc.Tables(1).Rows(i)("GST_AMT") & ", " &
                        "'" & Common.Parse(dsMisc.Tables(1).Rows(i)("REMARK")) & "', " & dsMisc.Tables(1).Rows(i)("TOTAL_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveMisc, strSCNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strSCNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'SC' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewSCNo, "T_NO") Then
                strSCNo = strNewSCNo
                strSCIndex = objDb.GetVal("SELECT SCM_CLAIM_INDEX FROM STAFF_CLAIM_MSTR WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "'")
                SaveMisc = True
            Else
                SaveMisc = False
            End If
        End Function

        Public Function UpdateMisc(ByVal dsMisc As DataSet, ByVal strSCNo As String, ByVal strSCIndex As String)
            Dim SqlQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0) As String
            Dim aryItem As New ArrayList()
            Dim i As Integer
            Dim blnDetail As Boolean

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            'Update some fields to existing record
            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_DEPT_CODE = '" & Common.Parse(dsMisc.Tables(0).Rows(0)("DEPT_CODE")) & "', " &
                    "SCM_MOD_BY = '" & strLoginUser & "', SCM_MOD_DT = NOW() " &
                    "WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete details before re-insert new details 
            SqlQuery = "DELETE FROM STAFF_CLAIM_MISC_DETAIL " &
                    "WHERE SCMD_CLAIM_DOC_NO = '" & strSCNo & "' AND SCMD_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To dsMisc.Tables(1).Rows.Count - 1
                SqlQuery = "INSERT INTO STAFF_CLAIM_MISC_DETAIL (SCMD_CLAIM_DOC_NO, SCMD_COY_ID, SCMD_LINE_NO, SCMD_DATE, " &
                        "SCMD_PROJ_BASED, SCMD_PROJ_CODE, SCMD_TYPE, SCMD_TAX_INV_TYPE, SCMD_CURRENCY_CODE, SCMD_EXCHANGE_RATE, SCMD_AMT, SCMD_GST_AMT, " &
                        "SCMD_REMARK, SCMD_TOTAL_AMT) " &
                        "VALUES ('" & Common.Parse(strSCNo) & "', '" & strCoyID & "', " & dsMisc.Tables(1).Rows(i)("LINE_NO") & ", " & Common.ConvertDate(dsMisc.Tables(1).Rows(i)("DATE")) & ", " &
                        "'" & dsMisc.Tables(1).Rows(i)("PROJ_BASED") & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("PROJ_CODE")) & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("TYPE")) & "', " &
                        "'" & Common.Parse(dsMisc.Tables(1).Rows(i)("TAX_INV_TYPE")) & "', '" & Common.Parse(dsMisc.Tables(1).Rows(i)("CURRENCY_CODE")) & "', " &
                        dsMisc.Tables(1).Rows(i)("EXCHANGE_RATE") & ", " & dsMisc.Tables(1).Rows(i)("AMT") & ", " & dsMisc.Tables(1).Rows(i)("GST_AMT") & ", " &
                        "'" & Common.Parse(dsMisc.Tables(1).Rows(i)("REMARK")) & "', " & dsMisc.Tables(1).Rows(i)("TOTAL_AMT") & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SaveMisc, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                UpdateMisc = True
            Else
                UpdateMisc = False
            End If
        End Function

        'Chee Hong - 23/11/2015 - Function to retrieve summary details
        Public Function GetSummaryDetails(ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlD As String
            Dim ds As DataSet

            strSqlM = "SELECT SCM_CLAIM_DOC_NO, SCM_CREATED_DATE, SCM_STAFF_ID, SCM_STATUS, STATUS_DESC, " &
                    "UM_USER_NAME, SCM_COY_ID, CM_COY_NAME, SCM_DEPT_CODE, CDM_DEPT_NAME " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = SCM_COY_ID " &
                    "INNER JOIN USER_MSTR ON UM_COY_ID = SCM_COY_ID AND UM_USER_ID = SCM_STAFF_ID " &
                    "LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = SCM_DEPT_CODE AND CDM_COY_ID = SCM_COY_ID " &
                    "LEFT JOIN STATUS_MSTR ON STATUS_TYPE = 'SC' AND STATUS_NO = SCM_STATUS " &
                    "WHERE SCM_CLAIM_INDEX = '" & strIndex & "'"

            'strSqlD = "SELECT " & _
            '        "DATE_FORMAT(SCTD_DATE, '%d/%m/%Y') AS SCTD_DATE, SCTD_LOC_FROM, SCTD_LOC_TO, SCTD_CSR_NO, " & _
            '        "SCTD_PURPOSE, SCTD_PUBLIC_TRANSPORT_AMT, SCTD_PARKING_AMT, SCTD_TOLL_AMT, SCTD_VEHICLE_RATE, " & _
            '        "SCTD_MILEAGE, SCTD_VEHICLE_AMT " & _
            '        "FROM STAFF_CLAIM_MSTR " & _
            '        "INNER JOIN STAFF_CLAIM_TRANSPORTATION_DETAIL ON SCTD_CLAIM_DOC_NO = SCM_CLAIM_DOC_NO AND SCTD_COY_ID = SCM_COY_ID " & _
            '        "WHERE SCM_CLAIM_INDEX = '" & strIndex & "' " & _
            '        "ORDER BY SCTD_LINE_NO"

            'strSql = strSqlM & ";" & strSqlD & ";"
            strSql = strSqlM
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "Mstr"
            'ds.Tables(1).TableName = "DETAILS"
            Return ds
        End Function

        Public Function getSCApprFlow() As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGM_GRP_INDEX, AGM_GRP_NAME FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO " &
                    "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' AND AGM_TYPE = 'SC'"

            ds = objDb.FillDs(strsql)
            getSCApprFlow = ds
        End Function

        Public Function getSCAOList(ByVal intIndex As Integer) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGA_SEQ, AGA_AO, IFNULL(AGA_A_AO, '') AS AGA_A_AO, AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME " &
                    "FROM APPROVAL_GRP_AO " &
                    "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX " &
                    "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX " &
                    "WHERE AGA_GRP_INDEX = " & intIndex & " " &
                    "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' " &
                    "AND AGM_TYPE = 'SC' " &
                    "ORDER BY AGA_SEQ "

            ds = objDb.FillDs(strsql)
            getSCAOList = ds
        End Function
        'mimi : 28/03/2017
        Public Function submitSC(ByVal strSCIndex As String, ByVal strAppGrpIndex As String, ByVal decSM As Decimal) As Integer
            'Dim strPrefix
            Dim SqlQuery, PM_PRODUCT_INDEX, PM_LOC_INDEX, ID_INVENTORY_QTY, IRM_IR_INDEX, strMgr As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            Dim dsApp As New DataSet
            Dim strUserName, strCoyId, strUserId, strSCNo, strSmartPay As String 'mimi :27/03/2017 - eclaim enhancement

            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserName = HttpContext.Current.Session("UserName")
            strUserId = HttpContext.Current.Session("UserId")

            strSCNo = objDb.GetVal("SELECT SCM_CLAIM_DOC_NO FROM STAFF_CLAIM_MSTR WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'")
            'mimi :27/03/2017 - eclaim enhancement
            'strSmartPay = objDb.GetVal("SELECT CM_SMART_PAY FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyId & "'")

            SqlQuery = "UPDATE STAFF_CLAIM_MSTR SET SCM_SUBMIT_DATE = NOW(), SCM_STATUS = " & SCStatus.Submitted & ", " &
                    "SCM_STATUS_CHANGED_BY = '" & strUserId & "', SCM_STATUS_CHANGED_ON = NOW(), SCM_MOD_BY = '" & strUserId & "', " &
                    "SCM_MOD_DT = NOW(), SCM_SMART_PAY = '" & decSM & "' WHERE SCM_CLAIM_INDEX = '" & strSCIndex & "'"
            'end
            Common.Insert2Ary(strAryQuery, SqlQuery)

            'Delete record from sc_approval if status is rejected.
            If objDb.Exist("SELECT '*' FROM SC_APPROVAL WHERE SCA_CLAIM_INDEX = '" & strSCIndex & "'") > 0 Then
                SqlQuery = "DELETE FROM SC_APPROVAL WHERE SCA_CLAIM_INDEX = '" & strSCIndex & "'"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            End If

            SqlQuery = "INSERT INTO SC_APPROVAL(SCA_CLAIM_INDEX, SCA_AO, SCA_A_AO, SCA_SEQ, SCA_AO_ACTION, SCA_APPROVAL_TYPE, SCA_APPROVAL_GRP_INDEX, SCA_RELIEF_IND) " &
                        "SELECT " & strSCIndex & ", AGA_AO, AGA_A_AO, AGA_SEQ, 0, 1, AGA_GRP_INDEX, 'O' FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX='" & strAppGrpIndex & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strAryQuery, WheelModule.StaffClaimMod, WheelUserActivity.Staff_SubmitStaffClaim, strSCNo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                'send email to 1st level of approving officer after submit
                sendSCMailToAO(strSCNo, CLng(strSCIndex), 1, strUserName, strUserId)
                submitSC = WheelMsgNum.Save
            Else
                submitSC = WheelMsgNum.NotSave
            End If
        End Function

        Function getStaffClaimTracking(ByVal strScNo As String, ByVal strStatus As String) As DataSet
            Dim strSql, strCoyID, strUserID As String
            Dim strTemp As String
            Dim ds As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSql = "SELECT SCM_CLAIM_INDEX, SCM_CLAIM_DOC_NO, SCM_CREATED_DATE, SCM_TOTAL, SCM_SUBMIT_DATE, SCM_STATUS, STATUS_DESC " &
                    "FROM STAFF_CLAIM_MSTR " &
                    "LEFT JOIN STATUS_MSTR ON STATUS_TYPE = 'SC' AND SCM_STATUS = STATUS_NO " &
                    "WHERE SCM_COY_ID = '" & strCoyID & "' AND SCM_STAFF_ID = '" & strUserID & "' "

            If strScNo <> "" Then
                strTemp = Common.BuildWildCard(strScNo)
                strSql &= " AND SCM_CLAIM_DOC_NO" & Common.ParseSQL(strTemp)
            End If

            If strStatus <> "" Then
                strSql = strSql & " AND SCM_STATUS IN (" & strStatus & ") "
            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Function getFirstClaimForm(ByVal strScNo As String, Optional ByVal strType As String = "") As String
            Dim SqlQuery, strCoyID As String
            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            'getFirstClaimForm = ""

            'Check Transportation Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCTD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Transportation"
            End If

            'Check Allowance Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_ALLOWANCE_DETAIL WHERE SCAD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCAD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Allowance"
            End If

            'Check Entertain Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_ENT_DETAIL WHERE SCED_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCED_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Ent"
            End If

            'Check Hardship Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_HARDSHIP_DETAIL WHERE SCHD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCHD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Hardship"
            End If

            'Check Overtime Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_OVERTIME_DETAIL WHERE SCOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Overtime"
            End If

            'Outstation Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL WHERE SCOOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOOD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Outstation"
            End If

            'Other Form
            SqlQuery = "SELECT '*' FROM STAFF_CLAIM_MISC_DETAIL WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCMD_COY_ID = '" & strCoyID & "' "
            If objDb.Exist(SqlQuery) > 0 Then
                Return "Other"
            End If

            Return ""

        End Function

        Function chkClaimFormData(ByVal strScNo As String, ByVal strType As String) As Boolean
            Dim SqlQuery, strCoyID As String
            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            'getFirstClaimForm = ""

            'Check Transportation Form
            If strType <> "Transportation" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCTD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Check Allowance Form
            If strType <> "Allowance" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_ALLOWANCE_DETAIL WHERE SCAD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCAD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Check Entertain Form
            If strType <> "Ent" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_ENT_DETAIL WHERE SCED_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCED_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Check Hardship Form
            If strType <> "Hardship" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_HARDSHIP_DETAIL WHERE SCHD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCHD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Check Overtime Form
            If strType <> "Overtime" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_OVERTIME_DETAIL WHERE SCOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Outstation Form
            If strType <> "Outstation" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL WHERE SCOOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOOD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            'Other Form
            If strType <> "Other" Then
                SqlQuery = "SELECT '*' FROM STAFF_CLAIM_MISC_DETAIL WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCMD_COY_ID = '" & strCoyID & "' "
                If objDb.Exist(SqlQuery) > 0 Then
                    Return True
                End If
            End If

            Return False

        End Function

        Function getTotalClaimAmt(ByVal strScNo As String) As Decimal
            Dim SqlQuery, strCoyID, strSmartClaimStatus, strSmartPay As String
            Dim decTotalClaimAmt As Decimal = 0
            Dim decTotalClaimAmtTemp As Decimal = 0 'mimi : 27/03/2017
            Dim decCapLimit As Decimal = 0 'mimi : 27/03/2017
            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            'getFirstClaimForm = ""
            'mimi : 27/03/2017
            SqlQuery = "SELECT IFNULL(SCM_SMART_PAY,'') FROM STAFF_CLAIM_MSTR WHERE SCM_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCM_COY_ID = '" & strCoyID & "' "
            strSmartPay = objDb.GetVal(SqlQuery)
            'end

            'Check Transportation Form
            SqlQuery = "SELECT IFNULL(SUM(SCTD_TOTAL_AMT),0) FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCTD_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)
            'mimi : 27/03/2017
            SqlQuery = "SELECT CM_SMART_PAY AS SP_CL FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "';"
            decCapLimit = objDb.GetVal(SqlQuery)
            SqlQuery = "SELECT IFNULL(SUM(IFNULL(ROUND((SCTD_SOG_FE_AMT / 8) * SCTD_FUEL_AMT,2),0) - IFNULL(SCTD_SMART_PAY_AMT,0)),0) AS SP FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCTD_COY_ID = '" & strCoyID & "'"
            decTotalClaimAmtTemp = objDb.GetVal(SqlQuery)

            If strSmartPay <> "" And strSmartClaimStatus <> SCStatus.Rejected Then 'strSmartPay not empty mean stamp/save the document before, not draft.
                decTotalClaimAmt += CDec(strSmartPay)
            Else
                If strSmartClaimStatus = SCStatus.DraftSC Or strSmartClaimStatus = SCStatus.Rejected Then
                    If decTotalClaimAmtTemp >= decCapLimit Then
                        decTotalClaimAmt += decCapLimit
                    Else
                        decTotalClaimAmt += decTotalClaimAmtTemp
                    End If
                Else
                    decTotalClaimAmt += decTotalClaimAmtTemp
                End If
            End If

            'Check Allowance Form
            SqlQuery = "SELECT IFNULL(SUM(SCAD_STANDBY_ALLOW_RATE + SCAD_SHIFT_ALLOW_RATE),0) FROM STAFF_CLAIM_ALLOWANCE_DETAIL WHERE SCAD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCAD_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)

            'Check Entertain Form
            SqlQuery = "SELECT IFNULL(SUM(SCED_TOTAL_AMT),0) FROM STAFF_CLAIM_ENT_DETAIL WHERE SCED_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCED_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)

            'Check Hardship Form
            SqlQuery = "SELECT IFNULL(SUM(SCHD_AMOUNT),0) FROM STAFF_CLAIM_HARDSHIP_DETAIL WHERE SCHD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCHD_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)

            'Check Overtime Form
            SqlQuery = "SELECT IFNULL(SUM(SCOD_MEAL_ALLOWANCE),0) FROM STAFF_CLAIM_OVERTIME_DETAIL WHERE SCOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOD_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)

            'Outstation Form
            SqlQuery = "SELECT IFNULL(SUM(SCOOD_TOTAL_CLAIM_AMT),0) FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL WHERE SCOOD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCOOD_COY_ID = '" & strCoyID & "' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)

            'Other Form
            SqlQuery = "SELECT IFNULL(SUM(SCMD_TOTAL_AMT),0) FROM STAFF_CLAIM_MISC_DETAIL WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCMD_COY_ID = '" & strCoyID & "' AND SCMD_TYPE <> 'Travelling Advance Taken' "
            decTotalClaimAmt += objDb.GetVal(SqlQuery)
            SqlQuery = "SELECT IFNULL(SUM(SCMD_TOTAL_AMT),0) FROM STAFF_CLAIM_MISC_DETAIL WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strScNo) & "' AND SCMD_COY_ID = '" & strCoyID & "' AND SCMD_TYPE = 'Travelling Advance Taken' "
            decTotalClaimAmt -= objDb.GetVal(SqlQuery)

            Return decTotalClaimAmt

        End Function

        Function getDdlInfo(ByVal strType As String) As DataSet
            Dim ds As New DataSet
            Dim SqlQuery As String

            If strType = "1" Then 'Currency
                SqlQuery = "SELECT CODE_ABBR FROM CODE_MSTR " &
                    "WHERE CODE_CATEGORY = 'CU' AND CODE_DELETED = 'N' " &
                    "ORDER BY CODE_DESC"
            ElseIf strType = "2" Then 'Purchase Tax Code
                SqlQuery = "SELECT TM_TAX_CODE FROM TAX_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND TM_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE TM_DELETED <> 'Y' AND TM_CATEGORY = 'eProcure' AND TM_TAX_TYPE = 'P' " &
                    "ORDER BY TM_TAX_CODE"
            ElseIf strType = "3" Then
                SqlQuery = ""
            Else
                SqlQuery = ""
            End If

            ds = objDb.FillDs(SqlQuery)
            Return ds
        End Function

        Function getSCListForApproval(ByVal strScNo As String,
                ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strReliefOn As String,
                Optional ByVal strAction As String = "new", Optional ByVal strStatus As String = "", Optional ByVal strAOAction As String = "", Optional ByVal strInclude As String = "",
                Optional ByVal strStaffId As String = "", Optional ByVal strStaffName As String = "", Optional ByVal strDeptCode As String = "") As DataSet

            Dim strSql, strSqlReliefO, strSqlReliefC, strCondition, strCondition1 As String
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
            Dim ds As DataSet
            Dim strTemp As String

            If strAction = "new" Then
                strCondition = " AND (SA.SCA_Seq - 1 = SA.SCA_AO_Action AND SCM.SCM_STATUS IN (" & strStatus & "))"
                strCondition1 = "(SA.SCA_AO = '" & strUser & "' OR (SA.SCA_A_AO = '" & strUser & "' AND SA.SCA_RELIEF_IND = 'O'))"
            ElseIf strAction = "app" Then
                If strAOAction = "" Then
                    strCondition = " AND (SA.SCA_Seq <= SA.SCA_AO_Action)"
                Else
                    If strAOAction = "Approved" Then
                        strCondition = " AND (SA.SCA_Seq <= SA.SCA_AO_Action) AND (SUBSTRING(SCA_AO_REMARK,1,8) = '" & strAOAction & "' OR SUBSTRING(SCA_AO_REMARK,1,8) = 'Endorsed') "
                    Else
                        strCondition = " AND (SA.SCA_Seq <= SA.SCA_AO_Action) AND SUBSTRING(SCA_AO_REMARK,1,8) = '" & strAOAction & "' "
                    End If
                End If
                '//ownership already taken by AAO, no need to check for relief ind
                strCondition1 = "(((SA.SCA_AO = '" & strUser & "' " _
                & "OR SA.SCA_A_AO = '" & strUser & "') AND SCA_RELIEF_IND='O') OR ((SA.SCA_AO = '" & strUser & "' " _
                & "OR SA.SCA_ON_BEHALFOF = '" & strUser & "') AND SCA_RELIEF_IND<>'O'))"
                '//set to empty string to prevent mistake during parameter passing
                strReliefOn = ""
            End If

            strSqlReliefO = "SELECT DISTINCT SCM.SCM_CLAIM_INDEX, SCM.SCM_CLAIM_DOC_NO, SCM.SCM_CREATED_DATE, SCM.SCM_SUBMIT_DATE, " &
                            "SCM.SCM_STAFF_ID, SCM.SCM_YEAR, SCM.SCM_MONTH, SCM.SCM_STATUS, UM1.UM_USER_NAME AS STAFF_NAME, SCM_TOTAL, SM.STATUS_DESC, CDM.CDM_DEPT_NAME " &
                            "FROM SC_APPROVAL SA " &
                            "INNER JOIN STAFF_CLAIM_MSTR SCM ON SA.SCA_CLAIM_INDEX = SCM.SCM_CLAIM_INDEX " &
                            "INNER JOIN STATUS_MSTR SM ON SM.STATUS_TYPE = 'SC' AND SM.STATUS_NO = SCM.SCM_STATUS " &
                            "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = SCM.SCM_STAFF_ID AND UM1.UM_COY_ID='" & strCoyId & "' " &
                            "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID = SCM.SCM_STATUS_CHANGED_BY AND UM.UM_COY_ID='" & strCoyId & "' " &
                            "LEFT JOIN COMPANY_DEPT_MSTR CDM ON CDM.CDM_DEPT_CODE = SCM.SCM_DEPT_CODE AND CDM.CDM_COY_ID = SCM.SCM_COY_ID " &
                            "WHERE " & strCondition1 &
                            " AND SCM.SCM_COY_ID = '" & strCoyId & "' " & strCondition

            If strScNo <> "" Then
                strTemp = Common.BuildWildCard(strScNo)
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_CLAIM_DOC_NO" & Common.ParseSQL(strTemp)
            End If

            If strStaffId <> "" Then
                strTemp = Common.BuildWildCard(strStaffId)
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_STAFF_ID" & Common.ParseSQL(strTemp)
            End If

            If strStaffName <> "" Then
                strTemp = Common.BuildWildCard(strStaffName)
                strSqlReliefO = strSqlReliefO & " AND UM1.UM_USER_NAME" & Common.ParseSQL(strTemp)
            End If

            If strDeptCode <> "" Then
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "'"
            End If

            If dteDateFr <> "" Then
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_SUBMIT_DATE >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
            End If

            If dteDateTo <> "" Then
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_SUBMIT_DATE <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If strAction = "app" And strStatus <> "" Then
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_Status IN (" & strStatus & ")"
            End If

            If (strAOAction = "Approved" Or strAOAction = "") And strInclude = "" Then
                strSqlReliefO = strSqlReliefO & " AND SCM.SCM_Status NOT IN(10)"
            End If

            '//For Relief Ind=Open
            Dim strReliefList As String

            If UCase(strReliefOn) = "WHEELALL" Then '//in case need to show all PR
                Dim dvCheck As DataView
                Dim objPR2 As New PurchaseReq2
                '//For Relief Ind=Controlled             
                dvCheck = objPR2.getReliefList("SC")
                Dim intCnt, intLoop As Integer
                If Not dvCheck Is Nothing Then
                    intCnt = dvCheck.Count
                    For intLoop = 0 To intCnt - 1
                        If Not IsDBNull(dvCheck.Item(intLoop)("RAM_USER_ID")) Then
                            If strReliefList = "" Then
                                strReliefList = "'" & dvCheck.Item(intLoop)("RAM_USER_ID") & "'"
                            Else
                                strReliefList = strReliefList & ",'" & dvCheck(intLoop)("RAM_USER_ID") & "'"
                            End If
                        End If
                    Next
                End If
            ElseIf strReliefOn <> "" Then
                strReliefList = "'" & strReliefOn & "'"
            Else
                strReliefList = ""
            End If

            If strReliefList <> "" Then
                strSqlReliefC = "SELECT DISTINCT PM.POM_PO_Index,PM.POM_PO_No,PM.POM_S_Coy_ID, PM.POM_CREATED_Date,PM.POM_BUYER_ID,PM.POM_STATUS_CHANGED_ON, PM.POM_RFQ_INDEX, " _
                & "PM.POM_CURRENCY_CODE, PM.POM_S_COY_NAME, PM.POM_PO_STATUS,(CASE WHEN PM.POM_PO_STATUS = '10' THEN 'Rejected' WHEN PM.POM_PO_STATUS = '11' THEN 'On Hold' ELSE 'Approved' END) AS STAT,PM.POM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
                & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC," _
                & "PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME, PM.POM_URGENT, " _
                & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') AS NAME, PM.POM_SUBMIT_DATE " _
                & "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " _
                & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID " _
                & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
                & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY " _
                & "AND UM.UM_COY_ID='" & strCoyId & "' " _
                & "WHERE PA.PRA_AO IN(" & strReliefList & ") AND PA.PRA_FOR = 'PO' " _
                & "AND PA.PRA_A_AO = '" & strUser & "' " _
                & "And PA.PRA_Relief_Ind='C' AND PM.POM_B_COY_ID='" & strCoyId & "'" & strCondition


                If strScNo <> "" Then
                    strTemp = Common.BuildWildCard(strScNo)
                    strSqlReliefC = strSqlReliefC & " AND SCM.SCM_CLAIM_DOC_NO" & Common.ParseSQL(strTemp)
                End If

                If dteDateFr <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND SCM_SUBMIT_DATE >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
                End If

                If strStaffId <> "" Then
                    strTemp = Common.BuildWildCard(strStaffId)
                    strSqlReliefO = strSqlReliefO & " AND SCM.SCM_STAFF_ID" & Common.ParseSQL(strTemp)
                End If

                If strStaffName <> "" Then
                    strTemp = Common.BuildWildCard(strStaffName)
                    strSqlReliefO = strSqlReliefO & " AND UM1.UM_USER_NAME" & Common.ParseSQL(strTemp)
                End If

                If strDeptCode <> "" Then
                    strSql = strSql & " AND SCM.SCM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "'"
                End If

                If dteDateTo <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND SCM_SUBMIT_DATE <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
                End If


                If strAction = "app" And strStatus <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND SCM.SCM_PO_Status IN (" & strStatus & ")"
                End If
            End If
            '//For Relief Ind=Controlled


            If UCase(strReliefOn) = "WHEELALL" Then '//show all
                If strSqlReliefC <> "" Then
                    strSql = strSqlReliefO & " UNION " & strSqlReliefC
                Else
                    strSql = strSqlReliefO
                End If
            ElseIf strReliefOn <> "" Then '//show other ao's
                strSql = strSqlReliefC
            Else '//show own pr
                strSql = strSqlReliefO
            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function
        'mimi : 20/03/2017 - enhancement smart pay ref.
        Function getClaimSummaryDt(ByVal strSCNo As String, ByVal strClaimType As String) As DataSet
            Dim strSql, strSql2, strSql3, strSql4, strSql5, strSql6, strSql7 As String 'mimi : 27/03/2017
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")


            If strClaimType = "Al" Then
                'Get Allowance Info
                strSql = "SELECT IFNULL(SUM(A), 0) AS Y_STANDBY, IFNULL(SUM(B),0) AS N_STANDBY, " &
                        "IFNULL(SUM(C),0) AS Y_SHIFT, IFNULL(SUM(D),0) AS N_SHIFT " &
                        "FROM (SELECT " &
                        "CASE WHEN SCAD_PROJ_BASED = 'Y' THEN SCAD_STANDBY_ALLOW_RATE ELSE 0 END AS A, " &
                        "CASE WHEN SCAD_PROJ_BASED <> 'Y' THEN SCAD_STANDBY_ALLOW_RATE ELSE 0 END AS B, " &
                        "CASE WHEN SCAD_PROJ_BASED = 'Y' THEN SCAD_SHIFT_ALLOW_RATE ELSE 0 END AS C, " &
                        "CASE WHEN SCAD_PROJ_BASED <> 'Y' THEN SCAD_SHIFT_ALLOW_RATE ELSE 0 END AS D " &
                        "FROM STAFF_CLAIM_ALLOWANCE_DETAIL WHERE SCAD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCAD_COY_ID = '" & strCoyId & "') tb "

            ElseIf strClaimType = "Ent" Then
                'Get Entertain Info
                strSql = "SELECT IFNULL(SUM(A), 0) AS Y_ENT, IFNULL(SUM(B),0) AS N_ENT " &
                        "FROM (SELECT " &
                        "CASE WHEN SCED_PROJ_BASED = 'Y' THEN SCED_TOTAL_AMT ELSE 0 END AS A, " &
                        "CASE WHEN SCED_PROJ_BASED <> 'Y' THEN SCED_TOTAL_AMT ELSE 0 END AS B " &
                        "FROM STAFF_CLAIM_ENT_DETAIL WHERE SCED_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCED_COY_ID = '" & strCoyId & "') tb "

            ElseIf strClaimType = "Hs" Then
                'Get Hardship Info
                strSql = "SELECT IFNULL(SUM(A), 0) AS Y_HDSHIP, IFNULL(SUM(B),0) AS N_HDSHIP " &
                        "FROM (SELECT " &
                        "CASE WHEN SCHD_PROJ_BASED = 'Y' THEN SCHD_AMOUNT ELSE 0 END AS A, " &
                        "CASE WHEN SCHD_PROJ_BASED <> 'Y' THEN SCHD_AMOUNT ELSE 0 END AS B " &
                        "FROM STAFF_CLAIM_HARDSHIP_DETAIL WHERE SCHD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCHD_COY_ID = '" & strCoyId & "') tb "

            ElseIf strClaimType = "Trans" Then
                'Get Transportation Info
                'Get Currency is MYR and Non-Project Based
                strSql = "SELECT IFNULL(SUM(SCTD_PT_AMT),0) AS PT_AMT, IFNULL(SUM(SCTD_PARKING_AMT),0) AS PK_AMT, IFNULL(SUM(SCTD_TOLL_AMT),0) AS TL_AMT, " &
                        "IFNULL(SUM(SCTD_AIRFARE_AMT),0) AS AF_AMT, IFNULL(SUM(SCTD_CAR_AMT),0) AS CAR_AMT, IFNULL(SUM(SCTD_BIKE_AMT),0) AS BK_AMT, " &
                        "IFNULL(SUM(SCTD_SOG_FE_AMT),0) AS SF_AMT, IFNULL(SUM(SCTD_SMART_PAY_AMT),0) AS SP_AMT " &
                        "FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_PROJ_BASED = 'N' AND SCTD_CURRENCY_CODE = 'MYR' " &
                        "AND SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "';"
                'Get Currency not MYR and Non-Project Based
                strSql2 = "SELECT IFNULL(SUM(SCTD_PT_AMT*SCTD_EXCHANGE_RATE),0) AS PT_AMT, IFNULL(SUM(SCTD_PARKING_AMT*SCTD_EXCHANGE_RATE),0) AS PK_AMT, IFNULL(SUM(SCTD_TOLL_AMT*SCTD_EXCHANGE_RATE),0) AS TL_AMT, " &
                        "IFNULL(SUM(SCTD_AIRFARE_AMT*SCTD_EXCHANGE_RATE),0) AS AF_AMT, IFNULL(SUM(SCTD_CAR_AMT),0) AS CAR_AMT, IFNULL(SUM(SCTD_BIKE_AMT),0) AS BK_AMT, " &
                        "IFNULL(SUM(SCTD_SOG_FE_AMT*SCTD_EXCHANGE_RATE),0) AS SF_AMT, IFNULL(SUM(SCTD_SMART_PAY_AMT*SCTD_EXCHANGE_RATE),0) AS SP_AMT " &
                        "FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_PROJ_BASED = 'N' AND SCTD_CURRENCY_CODE <> 'MYR' " &
                        "AND SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "';"
                'Get Currency is MYR and Project Based
                strSql3 = "SELECT IFNULL(SUM(SCTD_PT_AMT),0) AS PT_AMT, IFNULL(SUM(SCTD_PARKING_AMT),0) AS PK_AMT, IFNULL(SUM(SCTD_TOLL_AMT),0) AS TL_AMT, " &
                        "IFNULL(SUM(SCTD_AIRFARE_AMT),0) AS AF_AMT, IFNULL(SUM(SCTD_CAR_AMT),0) AS CAR_AMT, IFNULL(SUM(SCTD_BIKE_AMT),0) AS BK_AMT, " &
                        "IFNULL(SUM(SCTD_SOG_FE_AMT),0) AS SF_AMT, IFNULL(SUM(SCTD_SMART_PAY_AMT),0) AS SP_AMT " &
                        "FROM STAFF_CLAIM_TRANSPORTATION_DETAIL WHERE SCTD_PROJ_BASED = 'Y' AND SCTD_CURRENCY_CODE = 'MYR' " &
                        "AND SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "';"
                'Get Currency not MYR and Project Based
                strSql4 = "SELECT IFNULL(SUM(SCTD_TOTAL_AMT),0) AS TOTAL_AMT FROM STAFF_CLAIM_TRANSPORTATION_DETAIL " &
                        "WHERE SCTD_PROJ_BASED = 'Y' AND SCTD_CURRENCY_CODE <> 'MYR' AND SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "';"
                strSql5 = "SELECT IFNULL(SUM(IFNULL(ROUND((SCTD_SOG_FE_AMT / 8) * SCTD_FUEL_AMT,2),0) - IFNULL(SCTD_SMART_PAY_AMT,0)),0) AS SP FROM STAFF_CLAIM_TRANSPORTATION_DETAIL " &
                        "WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "';"
                'mimi : 22/03/2017 - enhancement smart pay ref.
                strSql6 = "SELECT CM_SMART_PAY AS SP_CL FROM COMPANY_MSTR " &
                        "WHERE CM_COY_ID = '" & strCoyId & "';"
                'mimi : 27/03/2017
                strSql7 = "SELECT CAST(IFNULL(SCM_SMART_PAY,'') AS CHAR(100)) AS SCM_SP, SCM_STATUS FROM STAFF_CLAIM_MSTR " &
                        "WHERE SCM_COY_ID = '" & strCoyId & "' AND SCM_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "';"
                strSql = strSql & strSql2 & strSql3 & strSql4 & strSql5 & strSql6 & strSql7
                'end

            ElseIf strClaimType = "Out" Then
                'Get Outstation Info
                strSql = "SELECT IFNULL(SUM(IFNULL(N_LOCAL_SUB_ALLW,0)),0) AS N_LOCAL_SUB_ALLW, IFNULL(SUM(IFNULL(N_LOCAL_ACC,0))+SUM(IFNULL(N_LOCAL_GST_AMT,0)),0) AS N_LOCAL_ACC, IFNULL(SUM(IFNULL(N_LOCAL_ACC_ALLW,0)),0) AS N_LOCAL_ACC_ALLW, " &
                        "IFNULL(SUM(IFNULL(N_OVER_SUB_ALLW,0)),0) AS N_OVER_SUB_ALLW, IFNULL(SUM(IFNULL(N_OVER_ACC,0))+SUM(IFNULL(N_OVER_GST_AMT,0)),0) AS N_OVER_ACC, IFNULL(SUM(IFNULL(N_OVER_ACC_ALLW,0)),0) AS N_OVER_ACC_ALLW, " &
                        "IFNULL(SUM(IFNULL(Y_ACC,0))+SUM(IFNULL(Y_GST_AMT,0)),0) AS Y_ACC, IFNULL(SUM(IFNULL(Y_LOCAL_ACC_ALLW,0)),0) AS Y_LOCAL_ACC_ALLW, IFNULL(SUM(IFNULL(Y_OVER_ACC_ALLW,0)),0) AS Y_OVER_ACC_ALLW, IFNULL(SUM(IFNULL(Y_SUB_ALLW,0)),0) AS Y_SUB_ALLW " &
                        "FROM (SELECT " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE = 'MYR' THEN SCOOD_TOTAL_SUB_ALLW_CLAIM ELSE 0 END AS N_LOCAL_SUB_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE = 'MYR' THEN SCOOD_TOTAL_ACC_CLAIM ELSE 0 END AS N_LOCAL_ACC, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE = 'MYR' THEN SCOOD_GST_AMT ELSE 0 END AS N_LOCAL_GST_AMT, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE = 'MYR' THEN SCOOD_TOTAL_ACC_ALLW ELSE 0 END AS N_LOCAL_ACC_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE <> 'MYR' THEN SCOOD_TOTAL_SUB_ALLW_CLAIM*SCOOD_EXCHANGE_RATE ELSE 0 END AS N_OVER_SUB_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE <> 'MYR' THEN SCOOD_TOTAL_ACC_CLAIM*SCOOD_EXCHANGE_RATE ELSE 0 END AS N_OVER_ACC, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE <> 'MYR' THEN SCOOD_GST_AMT*SCOOD_EXCHANGE_RATE ELSE 0 END AS N_OVER_GST_AMT, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'N' AND SCOOD_CURRENCY_CODE <> 'MYR' THEN SCOOD_TOTAL_ACC_ALLW*SCOOD_EXCHANGE_RATE ELSE 0 END AS N_OVER_ACC_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'Y' THEN ROUND(SCOOD_TOTAL_ACC_CLAIM*SCOOD_EXCHANGE_RATE,2) ELSE 0 END AS Y_ACC, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'Y' THEN ROUND(SCOOD_GST_AMT*SCOOD_EXCHANGE_RATE,2) ELSE 0 END AS Y_GST_AMT, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'Y' AND SCOOD_CURRENCY_CODE = 'MYR' THEN SCOOD_TOTAL_ACC_ALLW ELSE 0 END AS Y_LOCAL_ACC_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'Y' AND SCOOD_CURRENCY_CODE <> 'MYR' THEN ROUND(SCOOD_TOTAL_ACC_ALLW*SCOOD_EXCHANGE_RATE,2) ELSE 0 END AS Y_OVER_ACC_ALLW, " &
                        "CASE WHEN SCOOD_PROJ_BASED = 'Y' THEN ROUND(SCOOD_TOTAL_SUB_ALLW_CLAIM*SCOOD_EXCHANGE_RATE,2) ELSE 0 END AS Y_SUB_ALLW " &
                        "FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL WHERE SCOOD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCOOD_COY_ID = '" & strCoyId & "') tb "

            ElseIf strClaimType = "Ot" Then
                'Get Overtime Info
                strSql = "SELECT TIME_FORMAT(SEC_TO_TIME(SUM(TIME_TO_SEC(A))), '%H:%i') AS TOTAL_HOUR_MIN_A, " &
                        "TIME_FORMAT(SEC_TO_TIME(SUM(TIME_TO_SEC(B))), '%H:%i') AS TOTAL_HOUR_MIN_B, IFNULL(SUM(SCOD_MEAL_ALLOWANCE),0) AS MEAL_ALLOWANCE " &
                        "FROM (SELECT " &
                        "CASE WHEN SCOD_TIMES = '1.5x' THEN SCOD_TOTAL_HOUR_MIN ELSE NULL END AS A, " &
                        "CASE WHEN SCOD_TIMES = '2.0x' THEN SCOD_TOTAL_HOUR_MIN ELSE NULL END AS B, " &
                        "SCOD_MEAL_ALLOWANCE " &
                        "FROM STAFF_CLAIM_OVERTIME_DETAIL " &
                        "WHERE SCOD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCOD_COY_ID = '" & strCoyId & "') tb "
            End If
            getClaimSummaryDt = objDb.FillDs(strSql)

        End Function

        Function getMiscClaimSummaryDt(ByVal strSCNo As String, ByVal strType As String, Optional ByVal strProjBased As String = "") As Decimal
            Dim strSql As String
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT IFNULL(SUM(SCMD_TOTAL_AMT),0) " &
                    "FROM STAFF_CLAIM_MISC_DETAIL WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCMD_COY_ID = '" & strCoyId & "'"
            If strType = "HP" Then
                strSql &= " AND (SCMD_TYPE = 'Company H/Phone Off. Calls' OR SCMD_TYPE = 'Personal H/Phone Off. Calls' OR SCMD_TYPE = 'Handphone Subsidy' OR SCMD_TYPE = 'Data Plan')"
            Else
                strSql &= " AND SCMD_TYPE = '" & strType & "'"
            End If
            If strProjBased <> "" Then
                strSql &= " AND SCMD_PROJ_BASED = '" & strProjBased & "'"
            End If
            getMiscClaimSummaryDt = objDb.GetVal(strSql)

        End Function

        Function chkPolicyDt(ByVal strSCNo As String, ByVal strCType As String) As Boolean
            Dim strSql As String
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
            Dim ds As DataSet
            Dim i As Integer

            If strCType = "Al" Then
                strSql = "SELECT SCAD_DATE_FROM AS DATE_FROM FROM STAFF_CLAIM_ALLOWANCE_DETAIL " &
                        "WHERE SCAD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCAD_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Ent" Then
                strSql = "SELECT SCED_DATE AS DATE_FROM FROM STAFF_CLAIM_ENT_DETAIL " &
                        "WHERE SCED_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCED_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Hs" Then
                strSql = "SELECT SCHD_DATE_FROM AS DATE_FROM FROM STAFF_CLAIM_HARDSHIP_DETAIL " &
                        "WHERE SCHD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCHD_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Misc" Then
                strSql = "SELECT SCMD_DATE AS DATE_FROM FROM STAFF_CLAIM_MISC_DETAIL " &
                        "WHERE SCMD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCMD_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Out" Then
                strSql = "SELECT SCOOD_DEPART_DATE AS DATE_FROM FROM STAFF_CLAIM_OVER_OUTSTATION_DETAIL " &
                        "WHERE SCOOD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCOOD_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Ot" Then
                strSql = "SELECT SCOD_DATE_FROM AS DATE_FROM FROM STAFF_CLAIM_OVERTIME_DETAIL " &
                        "WHERE SCOD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCOD_COY_ID = '" & strCoyId & "'"
            ElseIf strCType = "Tp" Then
                strSql = "SELECT SCTD_DATE AS DATE_FROM FROM STAFF_CLAIM_TRANSPORTATION_DETAIL " &
                        "WHERE SCTD_CLAIM_DOC_NO = '" & Common.Parse(strSCNo) & "' AND SCTD_COY_ID = '" & strCoyId & "'"
            Else
            End If

            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If DateDiff(DateInterval.Day, CDate(ds.Tables(0).Rows(i)("DATE_FROM")), Today.Now()) > 110 Then
                        Return False
                    End If
                Next
            End If
            Return True

        End Function

        Function chkDocExistMonth() As Boolean
            Dim strSql As String
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
            Dim strUserId As String = HttpContext.Current.Session("UserId")

            strSql = "SELECT '*' FROM STAFF_CLAIM_MSTR " &
                    "WHERE SCM_STAFF_ID = '" & strUserId & "' AND SCM_COY_ID = '" & strCoyId & "' " &
                    "AND MONTH(SCM_CREATED_DATE) = MONTH(CURDATE()) AND YEAR(SCM_CREATED_DATE) = YEAR(CURDATE())"
            If objDb.Exist(strSql) > 0 Then
                chkDocExistMonth = True
            Else
                chkDocExistMonth = False
            End If

        End Function

        Function MassApprovalSC(ByVal strArySCNo() As String, ByVal strArySCIndex() As String, ByVal strAO As String, ByRef strReturnMsg() As String, ByVal blnRelief As Boolean, ByVal strRelief As String) As Boolean
            Dim strSql, strSql1, strAryQuery(0), strApprType As String
            Dim strCoyID, strAllSC, strAllSCIndex As String
            Dim intLoop As Integer
            Dim ds As DataSet
            Dim strMsg As String
            Dim dsTemp As DataSet

            strCoyID = HttpContext.Current.Session("CompanyId")

            For intLoop = 0 To strArySCNo.GetUpperBound(0)
                If intLoop = 0 Then
                    strAllSC = "'" & strArySCNo(intLoop) & "'"
                    strAllSCIndex = strArySCIndex(intLoop)
                Else
                    strAllSC = strAllSC & ",'" & strArySCNo(intLoop) & "'"
                    strAllSCIndex = strAllSCIndex & "," & strArySCIndex(intLoop)
                End If
            Next

            strSql = "SELECT SCM_CLAIM_INDEX,SCM_CLAIM_DOC_NO,SCM_STATUS,SCM_STATUS_CHANGED_BY,SCM_STAFF_ID, SCM_COY_ID FROM STAFF_CLAIM_MSTR WHERE SCM_CLAIM_DOC_NO IN (" & strAllSC & ") AND SCM_COY_ID ='" & strCoyID & "'"
            strSql1 = "SELECT * FROM SC_APPROVAL WHERE SCA_CLAIM_INDEX IN (" & strAllSCIndex & ") "

            ds = objDb.FillDs(strSql & ";" & strSql1)
            ds.Tables(0).TableName = "STAFF_CLAIM_MSTR"
            ds.Tables(1).TableName = "SC_APPROVAL"

            If Not ds Is Nothing Then
                Dim parentCol As DataColumn
                Dim childCol As DataColumn
                Dim dvChildView, dvParentView As DataView
                Dim SCrow, SCApprrow As DataRow
                Dim intCurrentSeq, intLastSeq As Integer
                Dim blnHighestLevel, blnCanApprove As Boolean
                Dim strActiveAO As String

                parentCol = ds.Tables("STAFF_CLAIM_MSTR").Columns("SCM_CLAIM_INDEX")
                childCol = ds.Tables("SC_APPROVAL").Columns("SCA_CLAIM_INDEX")

                ' Create DataRelation.
                Dim relSC As DataRelation
                relSC = New DataRelation("acct", parentCol, childCol)
                ' Add the relation to the DataSet.
                ds.Relations.Add(relSC)
                For Each SCrow In ds.Tables("STAFF_CLAIM_MSTR").Rows
                    blnCanApprove = True
                    If SCrow("SCM_STATUS") = SCStatus.PendingAppr Or SCrow("SCM_STATUS") = SCStatus.Submitted Then
                        For Each SCApprrow In SCrow.GetChildRows(relSC)
                            dsTemp = getSCListForApproval(SCrow("SCM_CLAIM_DOC_NO"), "", "", strRelief, , SCStatus.Submitted & "," & SCStatus.PendingAppr, "")
                            If dsTemp.Tables(0).Rows.Count = 0 Then
                                strMsg = "You have already approved SC No. " & SCrow("SCM_CLAIM_DOC_NO") & ". Approving of this SC is not allowed."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit Function
                            End If
                            intLastSeq = SCApprrow("SCA_AO_Action")
                            intCurrentSeq = intLastSeq + 1

                            '//check whether the PO was already approved by the same AO at the
                            '//same approving level.
                            If SCApprrow("SCA_Seq") = intCurrentSeq Then
                                strApprType = SCApprrow("SCA_APPROVAL_TYPE")
                                strActiveAO = Common.parseNull(SCApprrow("SCA_ACTIVE_AO"))
                                If strActiveAO <> "" Then
                                    strMsg = "You have already approved SC No. " & SCrow("SCM_CLAIM_DOC_NO") & ". Approving of this SC is not allowed."
                                    Common.Insert2Ary(strReturnMsg, strMsg)
                                    blnCanApprove = False
                                    Exit For
                                Else
                                    If Not (UCase(SCApprrow("SCA_AO")) = UCase(strAO) Or
                                     UCase(Common.parseNull(SCApprrow("SCA_A_AO"))) = UCase(strAO)) Then
                                        strMsg = "You are not a authorised person to approve SC No. " & SCrow("SCM_CLAIM_DOC_NO")
                                        Common.Insert2Ary(strReturnMsg, strMsg)
                                        blnCanApprove = False
                                        Exit For
                                    End If
                                End If
                            End If

                            If intCurrentSeq = SCApprrow("SCA_SEQ") Then
                                blnHighestLevel = True
                            Else
                                blnHighestLevel = False
                            End If
                        Next
                        If blnCanApprove Then
                            If strApprType = "1" Then
                                strMsg = ApproveSC(SCrow("SCM_CLAIM_DOC_NO"), SCrow("SCM_CLAIM_INDEX"), intCurrentSeq, blnHighestLevel, "Approved : ", blnRelief, strApprType)
                            Else
                                strMsg = ApproveSC(SCrow("SCM_CLAIM_DOC_NO"), SCrow("SCM_CLAIM_INDEX"), intCurrentSeq, blnHighestLevel, "Endorsed : ", blnRelief, strApprType)
                            End If

                            Common.Insert2Ary(strReturnMsg, strMsg)
                        End If
                    Else 'ERROR
                        'SC APPROVED/REJECTED/CANCELLED                       
                        If SCrow("SCM_STATUS") = SCStatus.Rejected Then
                            strMsg = "You have already rejected SC No. " & SCrow("SCM_CLAIM_DOC_NO") & ". Approving of this SC is not allowed. "
                        ElseIf SCrow("SCM_STATUS") = SCStatus.Approved Then
                            strMsg = "You have already approved SC No. " & SCrow("SCM_CLAIM_DOC_NO") & ". Approving of this SC is not allowed."
                        End If
                        Common.Insert2Ary(strReturnMsg, strMsg)
                    End If
                Next
            End If
            Return True
        End Function

        Function ApproveSC(ByVal strSCNo As String, ByVal intSCIndex As Long, ByVal intCurrentSeq As Integer,
        ByVal blnHighestLevel As Boolean, ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal strApprType As String) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strCoyID, strMsg, strSC, strLoginUser, strLoginUserName As String
            Dim intSCStatus As Integer
            Dim strvendorname, strStaffId As String
            Dim strMsg1 As String
            'Dim strVendorNameList As String
            Dim arrSCInfo, arrSC As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strVen, strVenList As String
            Dim objPR2 As New PurchaseReq2

            If strApprType = "1" Then
                strApprType = "approved"
            Else
                strApprType = "endorsed"
            End If
            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId") 'is a AO
            strLoginUserName = HttpContext.Current.Session("UserName") 'is a AO
            strSql = "SELECT SCM_STATUS,SCM_STATUS_CHANGED_BY,SCM_STAFF_ID, SCM_COY_ID FROM STAFF_CLAIM_MSTR WHERE SCM_CLAIM_INDEX = " & intSCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)

            If tDS.Tables(0).Rows.Count > 0 Then
                intSCStatus = tDS.Tables(0).Rows(0).Item("SCM_STATUS")
                strStaffId = tDS.Tables(0).Rows(0).Item("SCM_STAFF_ID")
                'strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_COY_ID"))
                If intSCStatus = SCStatus.Rejected Then
                    strMsg = "You have already rejected this SC. Approving of this SC is not allowed. "
                ElseIf intSCStatus = SCStatus.Approved Then
                    strMsg = "You have already approved this SC."
                End If
            End If

            strCoyID = HttpContext.Current.Session("CompanyId")

            If strMsg <> "" Then
                Return strMsg
            End If
            If blnHighestLevel Then
                'Update SC status to Approved
                strSql = "UPDATE STAFF_CLAIM_MSTR SET SCM_STATUS=" & SCStatus.Approved &
                ",SCM_STATUS_CHANGED_BY='" & strLoginUser & "',SCM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE SCM_CLAIM_INDEX=" & intSCIndex
                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intSCIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)
                strMsg = "Staff Claim No. " & strSCNo & " has been " & strApprType & ". "
            Else
                'Update SC status, status_changed_by,status_changed_date
                strSql = "UPDATE STAFF_CLAIM_MSTR SET SCM_STATUS=" & SCStatus.PendingAppr &
                ",SCM_STATUS_CHANGED_BY='" & strLoginUser & "',SCM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE SCM_CLAIM_INDEX=" & intSCIndex
                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intSCIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)
                strMsg = "Staff Claim No. " & strSCNo & " has been " & strApprType & ". "
            End If
            Dim objUsers As New Users
            objUsers.Log_UserActivity(strSqlAry, WheelModule.StaffClaimMod, WheelUserActivity.AO_ApproveSC, strSCNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else

                '//only send mail if transaction successfully created
                Dim objMail As New Email
                If blnHighestLevel Then
                    Dim strEmailOnOff As String = objDb.GetVal("SELECT IFNULL(UM_STAFF_CLAIM_EMAIL,'Y') FROM USER_MSTR WHERE UM_COY_ID = '" & strCoyID & "' AND UM_USER_ID = '" & strStaffId & "'")
                    If strEmailOnOff = "Y" Then
                        objMail.sendNotification(EmailType.SCApproved, strLoginUser, strCoyID, "", strSCNo, "", strLoginUserName, strStaffId)
                    End If
                Else
                    Dim strStaffName As String = objDb.GetVal("SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = '" & strCoyID & "' AND UM_USER_ID = '" & strStaffId & "'")
                    sendSCMailToAO(strSCNo, intSCIndex, intCurrentSeq + 1, strStaffName, strStaffId)
                End If
            End If

            Return strMsg
        End Function

        Sub updateAOAction(ByVal intSCIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByRef pQuery() As String, ByVal blnRelief As Boolean)
            Dim strSql, strLoginUser As String
            strLoginUser = HttpContext.Current.Session("UserId")

            If blnRelief Then
                strSql = "UPDATE SC_APPROVAL SET SCA_AO_REMARK='" & Common.Parse(strAORemark) & "',SCA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",SCA_AO='" & strLoginUser & "',SCA_ON_BEHALFOF=SCA_AO,SCA_ACTIVE_AO='" &
                strLoginUser & "' WHERE SCA_CLAIM_INDEX=" & intSCIndex & " AND SCA_SEQ=" & intCurrentSeq
            Else
                strSql = "UPDATE SC_APPROVAL SET SCA_AO_REMARK='" & Common.Parse(strAORemark) & "',SCA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",SCA_ACTIVE_AO='" & strLoginUser & "' WHERE SCA_CLAIM_INDEX=" & intSCIndex & " AND SCA_SEQ=" & intCurrentSeq
            End If

            Common.Insert2Ary(pQuery, strSql)
            strSql = "UPDATE SC_APPROVAL SET SCA_AO_ACTION = " & intCurrentSeq & " WHERE SCA_CLAIM_INDEX=" & intSCIndex
            Common.Insert2Ary(pQuery, strSql)
        End Sub

        Function getApprFlow(ByVal intSCIndex As Long) As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT SA.*,UMA.UM_USER_NAME AS AO_NAME,UMB.UM_USER_NAME AS AAO_NAME," &
                    "UMA.UM_APP_LIMIT AS AO_LIMIT,UMB.UM_APP_LIMIT AS AAO_LIMIT FROM " &
                    "SC_APPROVAL SA LEFT OUTER JOIN USER_MSTR UMA ON " &
                    "SA.SCA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID= '" & strCoyId & "' LEFT OUTER JOIN USER_MSTR UMB ON " &
                    "SA.SCA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID= '" & strCoyId & "' " &
                    "WHERE SCA_CLAIM_INDEX=" & intSCIndex & " " &
                    "ORDER BY SA.SCA_SEQ"
            ds = objDb.FillDs(strSql)
            Return ds

        End Function

        Function RejectSC(ByVal strSCNo As String, ByVal intSCIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean) As String
            Dim strSql, strSqlAry(0) As String
            Dim strCoyID, strLoginUser, strLoginUserName, strStaffId As String
            Dim intSCStatus As Integer
            Dim strMsg As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")
            strLoginUserName = HttpContext.Current.Session("UserName")
            strSql = "SELECT SCM_CLAIM_INDEX,SCM_CLAIM_DOC_NO,SCM_STATUS,SCM_STATUS_CHANGED_BY,SCM_STAFF_ID, SCM_COY_ID FROM STAFF_CLAIM_MSTR WHERE SCM_CLAIM_INDEX = " & intSCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intSCStatus = tDS.Tables(0).Rows(0).Item("SCM_STATUS")
                strStaffId = tDS.Tables(0).Rows(0).Item("SCM_STAFF_ID")
                If intSCStatus = SCStatus.Rejected Then
                    strMsg = "You have already rejected this SC."
                ElseIf intSCStatus = SCStatus.Approved Then
                    strMsg = "You have already approved this SC. Rejecting of this SC is not allowed."
                End If
            End If

            If intSCStatus = SCStatus.PendingAppr And isApproved(intSCIndex, intCurrentSeq, strUserID) Then
                strMsg = "You have already approved this SC. Rejecting of this SC is not allowed."
            End If

            If strMsg = "" Then
                Dim objUsers As New Users
                objUsers.Log_UserActivity(strSqlAry, WheelModule.StaffClaimMod, WheelUserActivity.AO_RejectSC, strSCNo)
                objUsers = Nothing
                strSql = "UPDATE STAFF_CLAIM_MSTR SET SCM_STATUS=" & SCStatus.Rejected &
                ",SCM_STATUS_CHANGED_BY='" & strUserID & "',SCM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE SCM_CLAIM_INDEX=" & intSCIndex
                Common.Insert2Ary(strSqlAry, strSql)
                updateAOAction(intSCIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                If objDb.BatchExecute(strSqlAry) Then
                    Dim objMail As New Email
                    Dim strEmailOnOff As String = objDb.GetVal("SELECT IFNULL(UM_STAFF_CLAIM_EMAIL,'Y') FROM USER_MSTR WHERE UM_COY_ID = '" & strCoyID & "' AND UM_USER_ID = '" & strStaffId & "'")
                    If strEmailOnOff = "Y" Then
                        objMail.sendNotification(EmailType.SCRejected, strLoginUser, strCoyID, "", strSCNo, "", strLoginUserName, strStaffId)
                    End If
                    sendSCMailToPrevAO(strSCNo, intSCIndex, intCurrentSeq)
                    objMail = Nothing
                    strMsg = "Staff Claim Number " & strSCNo & " has been rejected."
                Else
                    strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                End If
                Return strMsg

            Else
                Return strMsg
            End If
        End Function

        Function isApproved(ByVal intSCIndex As Long, ByVal intCurrentSeq As Integer, ByVal strUser As String) As Boolean
            Dim strSql, strAO, strAAO, strActiveAO As String
            Dim intLastSeq As Integer
            intLastSeq = intCurrentSeq '- 1
            strSql = "SELECT SCA_AO,SCA_A_AO,SCA_ACTIVE_AO FROM SC_APPROVAL WHERE SCA_CLAIM_INDEX=" & intSCIndex & " AND SCA_SEQ=" & intLastSeq
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("SCA_AO"))
                strAAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("SCA_A_AO"))
                strActiveAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("SCA_ACTIVE_AO"))
            End If

            If strActiveAO <> "" Then
                '//already approved
                isApproved = True
            Else
                '//not approved yet
                '//check whether this is a valid user
                If UCase(strAO) = UCase(strUser) Or UCase(strAAO) = UCase(strUser) Then
                    '//valid user
                    isApproved = False
                Else
                    '//not a valid user
                    isApproved = True
                End If
                'isApproved = False
            End If
        End Function

        Public Function sendSCMailToAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, ByVal strStaffName As String, ByVal strStaffId As String)
            Dim strSql, strCond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom

            strBody &= "<P>You have an outstanding Staff Claim (" & strDocNo & ", " & strStaffName & ", " & strStaffId & ") waiting for approval. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strSql = "SELECT RAM_USER_ID FROM RELIEF_ASSIGNMENT_MSTR " & _
                    "WHERE RAM_USER_ROLE = 'Approving Officer' " & _
                    "AND RAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "AND GETDATE() BETWEEN RAM_START_DATE AND RAM_END_DATE + 1 " & _
                    "AND RAM_USER_ID = "

            strCond = " WHERE SCA_CLAIM_INDEX = " & intIndex & " AND SCA_SEQ = " & intSeq & " ORDER BY SCA_SEQ "
            strSql &= "('" & objDB.Get1Column("SC_APPROVAL", "SCA_AO", strCond) & "')"
            If objDB.Exist(strSql) > 0 Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            strSql = "SELECT SCA_AO, ISNULL(SCA_A_AO, '') AS SCA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, " & _
                    "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME " & _
                    "FROM SC_APPROVAL " & _
                    "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = SCA_AO AND B.UM_DELETED <> 'Y' AND B.UM_STAFF_CLAIM_EMAIL = 'Y' AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = SCA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_STAFF_CLAIM_EMAIL = 'Y' AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "WHERE SCA_CLAIM_INDEX = " & intIndex & " AND SCA_SEQ = " & intSeq
            ds = objDB.FillDs(strSql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                If blnRelief Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                    Else
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Approving Officer), <BR>" & strBody
                    End If
                Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                End If

                objMail.Subject = "Agora : Staff Claim Approval"
                objMail.SendMail()
            End If
            objCommon = Nothing
        End Function

        Public Function sendSCMailToPrevAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer)
            Dim strsql, strRole As String
            Dim i As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common

            strRole = "Approving Officer"
            strBody &= "<P>Staff Claim (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strsql = "SELECT SCA_ACTIVE_AO, UM_EMAIL, UM_USER_NAME FROM SC_APPROVAL "
            strsql &= "INNER JOIN USER_MSTR ON SCA_ACTIVE_AO = UM_USER_ID AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND UM_STAFF_CLAIM_EMAIL = 'Y' "
            strsql &= "WHERE SCA_CLAIM_INDEX = " & intIndex & " AND SCA_SEQ < " & intSeq
            ds = objDb.FillDs(strsql)

            Dim objMail As New AppMail
            For i = 0 To ds.Tables(0).Rows.Count - 1
                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Approving Officer), <BR>" & strBody

                objMail.Subject = "Agora : Staff Claim Rejected"
                objMail.SendMail()
            Next

            objMail = Nothing
            objCommon = Nothing
        End Function

        Function getStaffClaimTracking_All(ByVal strScNo As String, ByVal strStatus As String, ByVal dtDateFr As String, ByVal dtDateTo As String, _
            ByVal strStaffId As String, ByVal strStaffName As String, ByVal strDeptCode As String) As DataSet

            Dim strSql, strCoyID, strUserID As String
            Dim strTemp As String
            Dim ds As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSql = "SELECT SCM.SCM_CLAIM_INDEX, SCM.SCM_CLAIM_DOC_NO, SCM.SCM_CREATED_DATE, SCM.SCM_SUBMIT_DATE, " & _
                    "SCM.SCM_STAFF_ID, SCM.SCM_YEAR, SCM.SCM_MONTH, SCM.SCM_STATUS, UM1.UM_USER_NAME AS STAFF_NAME, SCM.SCM_TOTAL, SM.STATUS_DESC, CDM.CDM_DEPT_NAME " & _
                    "FROM STAFF_CLAIM_MSTR SCM " & _
                    "INNER JOIN STATUS_MSTR SM ON SM.STATUS_TYPE = 'SC' AND SM.STATUS_NO = SCM.SCM_STATUS " & _
                    "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = SCM.SCM_STAFF_ID AND UM1.UM_COY_ID='" & strCoyID & "' " & _
                    "LEFT JOIN COMPANY_DEPT_MSTR CDM ON CDM.CDM_DEPT_CODE = SCM.SCM_DEPT_CODE AND CDM.CDM_COY_ID = SCM.SCM_COY_ID " & _
                    "WHERE SCM.SCM_COY_ID = '" & strCoyID & "' "

            If strScNo <> "" Then
                strTemp = Common.BuildWildCard(strScNo)
                strSql &= " AND SCM.SCM_CLAIM_DOC_NO" & Common.ParseSQL(strTemp)
            End If

            If strStatus <> "" Then
                strSql = strSql & " AND SCM_STATUS IN (" & strStatus & ") "
            End If

            If strStaffId <> "" Then
                strTemp = Common.BuildWildCard(strStaffId)
                strSql = strSql & " AND SCM.SCM_STAFF_ID" & Common.ParseSQL(strTemp)
            End If

            If strStaffName <> "" Then
                strTemp = Common.BuildWildCard(strStaffName)
                strSql = strSql & " AND UM1.UM_USER_NAME" & Common.ParseSQL(strTemp)
            End If

            If strDeptCode <> "" Then
                strSql = strSql & " AND SCM.SCM_DEPT_CODE = '" & Common.Parse(strDeptCode) & "'"
            End If

            If dtDateFr <> "" Then
                strSql = strSql & " AND SCM.SCM_SUBMIT_DATE >= " & Common.ConvertDate(dtDateFr & " 00:00:00")
            End If

            If dtDateTo <> "" Then
                strSql = strSql & " AND SCM.SCM_SUBMIT_DATE <= " & Common.ConvertDate(dtDateTo & " 23:59:59")
            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function
    End Class

End Namespace
