Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class ReliefAOValue
        Public AO_StartDate As String
        Public AO_EndDate As String
        Public AO_Index As String
    End Class
    Public Class ReliefConsolValue
        Public Consol_StartDate As String
        Public Consol_EndDate As String
        Public Consol_Index As String
    End Class
    Public Class ReliefconsolId
        Public Consol_id As String
    End Class

    Public Class PersonalSetting

        Dim objDb As New EAD.DBCom



        Public Function get_StartEndDate(ByVal items As ReliefAOValue)
            Dim strStartDate As String
            Dim strEndDate As String


            Dim strsql As String = "SELECT RAM_START_DATE,RAM_END_DATE, RAM_ASSIGN_INDEX FROM RELIEF_ASSIGNMENT_MSTR " & _
                                   "where RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                   "AND RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                                   "AND RAM_USER_ROLE='Approving Officer' AND " & Common.ConvertDate(Now.Today) & " <= RAM_END_DATE"
            'Common.ConvertDate(Now.Today) & " BETWEEN CDM_START_DATE AND CDM_END_DATE+1"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                items.AO_StartDate = tDS.Tables(0).Rows(0).Item("RAM_START_DATE").ToString.Trim
                items.AO_EndDate = tDS.Tables(0).Rows(0).Item("RAM_END_DATE").ToString.Trim
                items.AO_Index = tDS.Tables(0).Rows(0).Item("RAM_ASSIGN_INDEX").ToString.Trim
            End If

        End Function
        Public Function get_StartEndDateConsol(ByVal items As ReliefConsolValue)
            Dim strStartDate As String
            Dim strEndDate As String
            Dim strsql As String = "SELECT RAM_START_DATE,RAM_END_DATE,RAM_ASSIGN_INDEX FROM RELIEF_ASSIGNMENT_MSTR " & _
                                   "where RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                   "AND RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                                   "AND RAM_USER_ROLE='Consolidator' AND " & Common.ConvertDate(Now.Today) & " <= RAM_END_DATE"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                items.Consol_StartDate = tDS.Tables(0).Rows(0).Item("RAM_START_DATE").ToString.Trim
                items.Consol_EndDate = tDS.Tables(0).Rows(0).Item("RAM_END_DATE").ToString.Trim
                items.Consol_Index = tDS.Tables(0).Rows(0).Item("RAM_ASSIGN_INDEX").ToString.Trim
            End If

        End Function
        Public Function get_consolId(ByVal items As ReliefconsolId, ByVal valindex As String)

            Dim strsql As String = "SELECT RAU_ASSIGN_TO_USER FROM RELIEF_ASSIGNMENT_USER U ,RELIEF_ASSIGNMENT_MSTR M " & _
                                   "where RAU_ASSIGN_INDEX = '" & valindex & "' " & _
                                   "AND U.RAU_ASSIGN_INDEX = M.RAM_ASSIGN_INDEX " & _
                                   "AND M.RAM_USER_ID='" & HttpContext.Current.Session("UserId") & "' " & _
                                   "AND M.RAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                                   "AND M.RAM_USER_ROLE = 'Consolidator'"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                items.Consol_id = tDS.Tables(0).Rows(0).Item("RAU_ASSIGN_TO_USER").ToString.Trim
            End If

        End Function

        Function getAO_StaffRelief()
            Dim strsql As String
            Dim dsApp As New DataSet

            'strsql = "SELECT AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX, AGM_GRP_NAME,AGA_SEQ " & _
            '         "FROM APPROVAL_GRP_AO A inner JOIN " & _
            '         "APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX " & _
            '         "where M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '         "AND AGA_AO ='" & HttpContext.Current.Session("UserId") & "' " & _
            '         "AND AGA_A_AO <>''"

            'strsql = "SELECT AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX, AGM_GRP_NAME,C.UM_USER_ID as AAO_ID,C.UM_USER_NAME as AAO_NAME " & _
            '         "FROM APPROVAL_GRP_AO A inner JOIN APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX " & _
            '         "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
            '         "where M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '         "AND C.UM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
            '         "AND (AGA_A_AO <>'' AND AGA_A_AO IS NOT NULL) " & _
            '         "AND C.UM_DELETED<>'Y'"
            'ZULHAM 06122018
            strsql = "SELECT distinct AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX, AGM_GRP_NAME, " &
                     "B.UM_USER_ID as AAO_ID, B.UM_USER_NAME as AAO_NAME, C.UM_USER_ID " &
                     "FROM USER_MSTR C, APPROVAL_GRP_AO A " &
                     "left join USER_MSTR B ON A.AGA_A_AO = B.UM_USER_ID " &
                     "left JOIN APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX " &
                     "AND M.AGM_COY_ID = B.UM_COY_ID " &
                     "where M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND (AGA_A_AO <>'' AND AGA_A_AO IS NOT NULL) " &
                     "AND C.UM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " &
                     "AND AGA_AO = '" & HttpContext.Current.Session("UserId") & "' " &
                     "AND C.UM_DELETED<>'Y' " &
                        "UNION ALL  " &
                     "SELECT DISTINCT AGPAO_AO 'AGA_AO', AGPAO_A_AO 'AGA_A_AO', AGPAO_RELIEF_IND 'AGA_RELIEF_IND', AGPAO_GRP_INDEX AS 'AGA_GRP_INDEX', AGM_GRP_NAME,  " &
                     "B.UM_USER_ID AS AAO_ID, B.UM_USER_NAME AS AAO_NAME, C.UM_USER_ID  " &
                     "FROM USER_MSTR C, APPROVAL_GRP_PAO A " &
                     "LEFT JOIN USER_MSTR B ON A.AGPAO_A_AO = B.UM_USER_ID " &
                     "LEFT JOIN APPROVAL_GRP_MSTR M ON A.AGPAO_GRP_INDEX =M.AGM_GRP_INDEX " &
                     "AND M.AGM_COY_ID = B.UM_COY_ID " &
                     "WHERE M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND (AGPAO_A_AO <>'' AND AGPAO_A_AO IS NOT NULL) " &
                     "And C.UM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " &
                     "AND AGPAO_AO = '" & HttpContext.Current.Session("UserId") & "' " &
                     "AND C.UM_DELETED<>'Y'"

            'Jules 2018.12.18 - Added Finance Officers.
            strsql &= "UNION ALL " &
                    "SELECT DISTINCT AGA_AO 'AGA_AO', AGA_A_AO 'AGA_A_AO', AGA_RELIEF_IND 'AGA_RELIEF_IND', AGA_GRP_INDEX AS 'AGA_GRP_INDEX', AGM_GRP_NAME, " &
                    "B.UM_USER_ID As AAO_ID, B.UM_USER_NAME As AAO_NAME, C.UM_USER_ID " &
                    "FROM USER_MSTR C, APPROVAL_GRP_FINANCE F " &
                    "LEFT JOIN USER_MSTR B ON F.AGA_A_AO = B.UM_USER_ID " &
                    "LEFT JOIN APPROVAL_GRP_MSTR M ON F.AGA_GRP_INDEX =M.AGM_GRP_INDEX " &
                    "AND M.AGM_COY_ID = B.UM_COY_ID " &
                    "WHERE M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND (AGA_A_AO <>'' AND AGA_A_AO IS NOT NULL) AND AGA_TYPE='FO' " &
                    "AND C.UM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " &
                    "AND AGA_AO = '" & HttpContext.Current.Session("UserId") & "' " &
                    "AND C.UM_DELETED<>'Y'"
            'End modification.

            dsApp = objDb.FillDs(strsql)

            getAO_StaffRelief = dsApp
        End Function

        Function updateNewReliefDate(ByVal strnewdate As String)
            Dim strsql As String
            strsql = "UPDATE RELIEF_ASSIGNMENT_MSTR set " & _
                     "RAM_END_DATE = " & Common.ConvertDate(strnewdate) & " " & _
                     "WHERE RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                     "AND RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND RAM_USER_ROLE='Approving Officer'"
            objDb.Execute(strsql)
        End Function
        Function updateNewReliefDateConsol(ByVal strnewdate As String)
            Dim strsql As String
            strsql = "UPDATE RELIEF_ASSIGNMENT_MSTR set " & _
                     "RAM_END_DATE = " & Common.ConvertDate(strnewdate) & " " & _
                     "WHERE RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                     "AND RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND RAM_USER_ROLE='Consolidator'"
            objDb.Execute(strsql)
        End Function

        Function updateReliefConsolStaff(ByVal valRauIndex As String, ByVal valAAo As String, ByVal strdatefr As String, ByVal strdateto As String)
            Dim strsql As String
            Dim strCoyID As String
            Dim query(0) As String


            strsql = "INSERT INTO RELIEF_ASSIGNMENT_MSTR (RAM_USER_ID,RAM_USER_ROLE,RAM_COY_ID,RAM_START_DATE,RAM_END_DATE) VALUES ("
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            strsql &= "'Consolidator', "
            strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            strsql &= "" & Common.ConvertDate(strdatefr) & ", "
            strsql &= "" & Common.ConvertDate(strdateto) & ")"
            Common.Insert2Ary(query, strsql)

            'strsql = "DELETE RELIEF_ASSIGNMENT_USER where RAU_ASSIGN_INDEX='" & valRauIndex & "' " & _
            '                     "AND RAU_ASSIGN_TO_USER ='" & valAAo & "'"
            'Common.Insert2Ary(query, strsql)

            'strsql = "INSERT INTO RELIEF_ASSIGNMENT_USER(RAU_ASSIGN_INDEX,RAU_ASSIGN_TO_USER) values ('" & valRauIndex & "','" & valAAo & "')"
            'Common.Insert2Ary(query, strsql)

            strsql = "INSERT INTO RELIEF_ASSIGNMENT_USER (RAU_ASSIGN_INDEX,RAU_ASSIGN_TO_USER) SELECT "
            strsql &= "(SELECT max(RAM_ASSIGN_INDEX) from RELIEF_ASSIGNMENT_MSTR), "
            strsql &= "'" & valAAo & "' "
            Common.Insert2Ary(query, strsql)



            objDb.BatchExecute(query)
        End Function

        Function DelReliefStaff(ByVal strRauIndex As String)
            Dim strSQL As String
            Dim query(0) As String

            'strSQL = "DELETE RELIEF_ASSIGNMENT_USER where RAU_ASSIGN_INDEX='" & strRauIndex & "' " & _
            '         "AND RAU_ASSIGN_TO_USER ='" & valAAo & "'"
            strSQL = "DELETE FROM RELIEF_ASSIGNMENT_USER where RAU_ASSIGN_INDEX='" & strRauIndex & "'"
            Common.Insert2Ary(query, strSQL)



            objDb.BatchExecute(query)
        End Function

        Function DelReliefAO_MSTR(ByVal strRauIndex As String)
            Dim strSQL As String
            Dim query(0) As String

            strSQL = "DELETE FROM RELIEF_ASSIGNMENT_MSTR where RAM_ASSIGN_INDEX='" & strRauIndex & "' " & _
                     "AND RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                     "AND RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND RAM_USER_ROLE='Approving Officer'"
            Common.Insert2Ary(query, strSQL)


            objDb.BatchExecute(query)
        End Function
        Function DelReliefConsol_MSTR(ByVal strRauIndex As String)
            Dim strSQL As String
            Dim query(0) As String

            strSQL = "DELETE FROM RELIEF_ASSIGNMENT_MSTR where RAM_ASSIGN_INDEX='" & strRauIndex & "' " & _
                     "AND RAM_USER_ID ='" & HttpContext.Current.Session("UserId") & "' " & _
                     "AND RAM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND RAM_USER_ROLE='Consolidator'"
            Common.Insert2Ary(query, strSQL)


            objDb.BatchExecute(query)
        End Function

        Public Function updateReliefStaff(ByVal strdatefr As String, ByVal strdateto As String) 'ByVal dtRelief As DataTable, 
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strsql = "INSERT INTO RELIEF_ASSIGNMENT_MSTR (RAM_USER_ID,RAM_USER_ROLE,RAM_COY_ID,RAM_START_DATE,RAM_END_DATE) VALUES ("
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            strsql &= "'Approving Officer', "
            strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            strsql &= "" & Common.ConvertDate(strdatefr) & ", "
            strsql &= "" & Common.ConvertDate(strdateto) & ")"
            Common.Insert2Ary(strAryQuery, strsql)


            'strsql = "DELETE FROM RELIEF_ASSIGNMENT_USER "
            'strsql &= "WHERE RAU_ASSIGN_INDEX = '" & Common.Parse(dtRelief.Rows(0)("AGA_GRP_INDEX")) & "' "
            'strsql &= "AND RAU_ASSIGN_TO_USER = '" & Common.Parse(dtRelief.Rows(i)("AAO_ID")) & "'"
            'Common.Insert2Ary(strAryQuery, strsql)


            ' ai chu remark on 08/09/2005 start -----------------
            ' no need to save AAO into RELIEF_ASSIGNMENT_USER table
            'For i = 0 To dtRelief.Rows.Count - 1
            '    strsql = "INSERT INTO RELIEF_ASSIGNMENT_USER (RAU_ASSIGN_INDEX,RAU_ASSIGN_TO_USER) SELECT "
            '    strsql &= "(SELECT max(RAM_ASSIGN_INDEX) from RELIEF_ASSIGNMENT_MSTR), "
            '    strsql &= "'" & Common.Parse(dtRelief.Rows(i)("AAO_ID")) & "' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            '    'strsql &= "'" & Common.Parse(dtRelief.Rows(i)("AGA_GRP_INDEX")) & "', "
            'Next
            '--------------------- end --------------------------

            objDb.BatchExecute(strAryQuery)
        End Function
        Public Function bindReliefconsol() As DataView

            Dim strSql, strUserId As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strUserId = HttpContext.Current.Session("UserId")

            'strSql = "SELECT distinct M.AGM_CONSOLIDATOR, R.RAM_USER_ID, RAM_ASSIGN_INDEX " & _
            '         "FROM APPROVAL_GRP_MSTR M, RELIEF_ASSIGNMENT_MSTR R " & _
            '         "WHERE R.RAM_USER_ID='" & HttpContext.Current.Session("UserId") & "' " & _
            '         "AND R.RAM_USER_ROLE = 'Consolidator' " & _
            '         "AND M.AGM_COY_ID =R.RAM_COY_ID AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '         "AND M.AGM_CONSOLIDATOR <>'' " & _
            '         "AND M.AGM_CONSOLIDATOR <> '" & strUserId & "'"

            strSql = "Select R.UU_USRGRP_ID,M.UM_USER_ID,R.UU_USER_ID,M.UM_COY_ID, " &
                     objDB.Concat(" : ", "", "M.UM_USER_ID", "M.UM_USER_NAME") & " as two " &
                     "FROM USER_MSTR M, USERS_USRGRP R, USER_GROUP_MSTR GM " &
                     "where M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " &
                     "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND R.UU_USER_ID = M.UM_USER_ID " &
                     "AND R.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Consolidator' " &
                     "AND UM_USER_ID <> '" & HttpContext.Current.Session("UserId") & "'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function
        Public Function FavListMain() As DataSet

            Dim strsql As String
            Dim ds As DataSet
            Dim lsSql As String


            strsql = "Select FLM_LIST_NAME from FAVOURITE_LIST_MSTR " &
            "where FLM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            FavListMain = ds
            ds = objDb.FillDs(strsql)
            FavListMain = ds
        End Function
        'Dim objDb As New EAD.DBCom()
        Function modfavlist(ByVal strlistindex As String, ByVal strlistname As String, ByVal strstatus As String, Optional ByVal strOld As String = "") As Integer
            Dim strsql As String


            'If strOld <> strname Then
            '    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & _
            '    HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NAME ='" & _
            '    Common.Parse(strname) & "'") > 0 Then
            '        modCustomlist = WheelMsgNum.Duplicate
            '        Exit Function
            '    End If
            'End If
            'strsql = "update CUSTOM_FIELDS set "
            'strsql &= "CF_FIELD_NAME = '" & Common.Parse(strname) & "' where CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NO = '" & intNum & "' "
            If UCase(strOld) <> UCase(strlistname) Then
                If objDb.Exist("Select '*' from FAVOURITE_LIST_MSTR where FLM_BUYER_ID ='" & _
                                HttpContext.Current.Current.Session("UserId") & "' and FLM_B_COY_ID ='" & _
                                 HttpContext.Current.Current.Session("CompanyID") & "' and FLM_LIST_NAME ='" & _
                                 Common.Parse(strlistname) & "'") > 0 Then

                    modfavlist = WheelMsgNum.Duplicate
                    Exit Function
                End If
            End If

            strsql = "update FAVOURITE_LIST_MSTR set "
            strsql &= "FLM_LIST_NAME = '" & Common.Parse(strlistname) & "', FLM_STATUS ='" & Common.Parse(strstatus) & "' "
            strsql &= "WHERE FLM_LIST_INDEX ='" & Common.Parse(strlistindex) & "' "

            If objDb.Execute(strsql) Then
                modfavlist = WheelMsgNum.Save
            Else
                modfavlist = WheelMsgNum.NotSave

            End If
        End Function

        Function addfavlist(ByVal strlistname As String, ByVal strstatus As String) As Integer
            Dim strsql As String
            Dim strCoyID, strUserId As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            If objDb.Exist("Select '*' From FAVOURITE_LIST_MSTR Where FLM_B_COY_ID='" & strCoyID & _
            "' And FLM_LIST_NAME='" & Common.Parse(strlistname) & "' AND FLM_BUYER_ID='" & strUserId & "'") > 0 Then
                addfavlist = WheelMsgNum.Duplicate
                Exit Function
            End If
            '    If objDb.Execute(strSql_adddept) Then
            '        addDept = WheelMsgNum.Save
            '    Else
            '        addDept = WheelMsgNum.NotSave
            '    End If
            'End Function



            strsql = "insert Into FAVOURITE_LIST_MSTR(FLM_BUYER_ID,FLM_B_COY_ID,FLM_LIST_NAME,FLM_STATUS)values ('" & strUserId & "','" & strCoyID & "','" & Common.Parse(strlistname) & "','" & Common.Parse(strstatus) & "')"
            If objDb.Execute(strsql) Then
                addfavlist = WheelMsgNum.Save
            Else
                addfavlist = WheelMsgNum.NotSave
            End If

        End Function

        Function getfavlist(ByVal strlistname As String) As DataSet
            Dim strget As String
            Dim dsfavlist As DataSet

            strget = "SELECT FLM_LIST_INDEX, FLM_LIST_NAME,FLM_STATUS FROM FAVOURITE_LIST_MSTR " & _
                     "WHERE FLM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " & _
                     "AND FLM_BUYER_ID = '" & HttpContext.Current.Session("UserID") & "'"

            If strlistname <> "" Then
                strget = strget & " AND FLM_LIST_NAME " & Common.ParseSQL(strlistname) & ""
            End If

            dsfavlist = objDb.FillDs(strget)
            getfavlist = dsfavlist

        End Function

        Function delCustomlist(ByVal strNo As String) As String
            Dim strdel As String
            strdel = "Delete from CUSTOM_FIELDS where CF_FIELD_NO = '" & Common.Parse(strNo) & "' and CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "'; "
            strdel &= "Delete from USERS_DEFAULT_CUSTOMFIELDS where UDC_FIELD_NO = '" & Common.Parse(strNo) & "' and UDC_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "'"
            objDb.Execute(strdel)

            'If objDb.Exist("Select '*' From CUSTOM_FIELD Where CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & " and CF_FIELD_NAME = '" & Common.Parse(strname) & " and CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "'")
            'strsql = "update CUSTOM_FIELDS set "
            'strsql &= "CF_FIELD_NAME = '" & Common.Parse(strname) & "' "
            'strsql &= "WHERE CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NO ='" & intnum & "'"
        End Function

        Function delFieldValue(ByVal intIndex As Integer) As String

            Dim strdel As String
            strdel = "Delete from CUSTOM_FIELDS where CF_FIELD_INDEX = '" & intIndex & "' "
            objDb.Execute(strdel)

            'If objDb.Exist("Select '*' From CUSTOM_FIELD Where CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & " and CF_FIELD_NAME = '" & Common.Parse(strname) & " and CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "'")
            'strsql = "update CUSTOM_FIELDS set "
            'strsql &= "CF_FIELD_NAME = '" & Common.Parse(strname) & "' "
            'strsql &= "WHERE CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NO ='" & intnum & "'"
        End Function

        Function delfavlist(ByVal strlistindex As String) As String
            Dim strdel As String
            strdel = "Delete from FAVOURITE_LIST_MSTR where FLM_LIST_INDEX = '" & strlistindex & "'"
            objDb.Execute(strdel)
        End Function

        Public Function addCustomField(ByVal name As String, ByVal value As String, ByVal CFmodule As String, ByVal type As String, Optional ByVal modModule As String = Nothing, Optional ByVal modName As String = Nothing, Optional ByVal modValue As String = Nothing)
            Dim intno As Integer
            Dim fieldNo As Integer
            Dim intFIndex As Integer
            Dim strsql As String
            If type = "Add" Then
                'Modified by Joon on 21 Sept for issue 907
                If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME ='" & Common.Parse(name) & "' AND CF_MODULE ='" & CFmodule & "'") > 0 Then
                    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME ='" & Common.Parse(name) & "' AND CF_FIELD_VALUE ='" & Common.Parse(value) & "' AND CF_MODULE ='" & CFmodule & "'") > 0 Then
                        addCustomField = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                    fieldNo = objDb.Get1Column("CUSTOM_FIELDS", "CF_FIELD_NO", " WHERE CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME='" & Common.Parse(name) & "' AND CF_MODULE ='" & CFmodule & "'")
                    strsql = "INSERT INTO CUSTOM_FIELDS(CF_COY_ID,CF_FIELD_NO,CF_FIELD_NAME,CF_FIELD_VALUE,CF_MODULE) VALUES('" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "','" & fieldNo & "','" & Common.Parse(name) & "','" & Common.Parse(value) & "','" & CFmodule & "')"

                Else
                    intno = objDb.GetMax("CUSTOM_FIELDS", "CF_FIELD_NO", " WHERE CF_COY_ID='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_MODULE ='" & CFmodule & "'") + 1
                    strsql = "INSERT INTO CUSTOM_FIELDS(CF_COY_ID,CF_FIELD_NO,CF_FIELD_NAME,CF_FIELD_VALUE,CF_MODULE) VALUES('" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "','" & intno & "','" & Common.Parse(name) & "','" & Common.Parse(value) & "','" & CFmodule & "')"
                End If
            End If
            If type = "Modify" Then
                'If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CF_FIELD_NAME ='" & modName & "' AND CF_FIELD_VALUE ='" & modValue & "' AND CF_MODULE ='" & modModule & "'") > 0 Then
                intFIndex = objDb.Get1Column("CUSTOM_FIELDS", "CF_FIELD_INDEX", " Where CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME ='" & Common.Parse(modName) & "' AND CF_FIELD_VALUE ='" & Common.Parse(modValue) & "' AND CF_MODULE ='" & modModule & "'")
                If intFIndex > 0 Then
                    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME ='" & Common.Parse(name) & "' AND CF_FIELD_VALUE ='" & Common.Parse(value) & "' AND CF_MODULE ='" & CFmodule & "' AND CF_FIELD_INDEX<>" & intFIndex) > 0 Then
                        addCustomField = WheelMsgNum.Duplicate
                        Exit Function
                    Else
                        strsql = "UPDATE CUSTOM_FIELDS SET CF_FIELD_NAME='" & Common.Parse(name) & "',CF_FIELD_VALUE='" & Common.Parse(value) & "' WHERE CF_COY_ID ='" & Common.Parse(HttpContext.Current.Current.Session("CompanyID")) & "' AND CF_FIELD_NAME ='" & Common.Parse(modName) & "' AND CF_FIELD_VALUE ='" & Common.Parse(modValue) & "' AND CF_MODULE ='" & modModule & "'"
                    End If
                Else
                    addCustomField = WheelMsgNum.NotSave
                    Exit Function
                End If
                'Else
                '    addCustomField = WheelMsgNum.NotSave
                '    Exit Function
                'End If

            End If
            objDb.Execute(strsql)
            addCustomField = WheelMsgNum.Save
        End Function
        Public Function delCustomField(ByVal CFmodule As String, ByVal name As String, ByVal value As String)
            Dim strsql As String
            strsql = "DELETE FROM CUSTOM_FIELDS WHERE CF_FIELD_NAME = '" & Common.Parse(name) & "' AND CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "' AND CF_MODULE='" & Common.Parse(CFmodule) & "' AND CF_FIELD_VALUE='" & Common.Parse(value) & "'"
            objDb.Execute(strsql)
        End Function
        Public Function addCustomList(ByVal strname As String, ByVal strValue As String) As Integer

            Dim intno As Integer
            Dim strsql As String
            'strCoyID = HttpContext.Current.Session("CompanyId")
            'If objDb.Exist("Select '*' From COMPANY_DEPT_MSTR Where CDM_COY_ID='" & strCoyID & "' And CDM_DEPT_CODE='" & strDeptCode & "'") > 0 Then
            'addDept = RecordDuplicate
            If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NAME ='" & Common.Parse(strname) & "'") > 0 Then
                addCustomList = WheelMsgNum.Duplicate

                'If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NAME ='" & Common.Parse(strname) & "'") > 0 Then
                '    addCustomList = RecordDuplicate
                Exit Function
            End If
            intno = objDb.GetMax("CUSTOM_FIELDS", "CF_FIELD_NO", "WHERE CF_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "'") + 1

            'Michelle (22/9/2010) - Include the column names for MYSQL syntax
            'strsql = "insert Into CUSTOM_FIELDS values ('" & HttpContext.Current.Current.Session("CompanyID") & "', " & intno & ", '" & Common.Parse(strname) & "', '" & Common.Parse(strValue) & "')"
            strsql = "insert Into CUSTOM_FIELDS (CF_COY_ID,CF_FIELD_NO,CF_FIELD_NAME,CF_FIELD_VALUE) values ('" & HttpContext.Current.Current.Session("CompanyID") & "', " & intno & ", '" & Common.Parse(strname) & "', '" & Common.Parse(strValue) & "')"

            If objDb.Execute(strsql) Then
                addCustomList = WheelMsgNum.Save
            Else
                addCustomList = WheelMsgNum.NotSave
            End If

        End Function

        Function modCustomlist(ByVal intNum As Integer, ByVal strname As String, ByVal intIndex As Integer, ByVal strvalue As String, Optional ByVal strOld As String = "") As Integer

            Dim strsql As String

            If intIndex = 0 Then
                If UCase(strOld) <> UCase(strname) Then
                    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_COY_ID ='" & _
                    HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NAME ='" & _
                    Common.Parse(strname) & "'") > 0 Then
                        modCustomlist = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If
                strsql = "update CUSTOM_FIELDS set "
                strsql &= "CF_FIELD_NAME = '" & Common.Parse(strname) & "' where CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_NO = '" & intNum & "' "
            Else
                If UCase(strOld) <> UCase(strvalue) Then
                    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_FIELD_NO = " & intNum & _
                                    " and CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & _
                                    "' and CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "'") > 0 Then
                        modCustomlist = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If
                strsql = "update CUSTOM_FIELDS set "
                strsql &= "CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "' where  CF_FIELD_INDEX = '" & intIndex & "'"
                'modCustomlist = WheelMsgNum.Duplicate
            End If

            'If strvalue <> "" Then
            '    strsql = strsql & ", CF_FIELD_VALUE ='" & Common.Parse(strvalue) & "' "
            'End If

            'strsql &= "WHERE CF_FIELD_INDEX = " & intIndex   'CF_COY_ID ='" &  & "' and CF_FIELD_NO ='" & intnum & "'and CF_FIELD_INDEX ='" & Common.Parse(strvalue) & "'"
            'strsql = strsql & " AND CF_FIELD_INDEX ='" & Common.Parse(strvalue) & "'"

            'objDb.Execute(strsql)

            If objDb.Execute(strsql) Then
                modCustomlist = WheelMsgNum.Save
            Else
                modCustomlist = WheelMsgNum.NotSave
            End If
        End Function

        Public Function addValue(ByVal intno As Integer, ByVal strname As String, ByVal strvalue As String) As Integer
            'Dim intno As Integer
            Dim strsql As String
            'addValue = WheelMsgNum.Duplicate
            If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_FIELD_NO = " & intno & " and CF_COY_ID = '" & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "'") > 0 Then
                addValue = WheelMsgNum.Duplicate
                Exit Function
            End If

            'Michelle (22/9/2010) - Include the column names for MYSQL syntax
            'strsql = "insert Into CUSTOM_FIELDS values ('" & HttpContext.Current.Current.Session("CompanyID") & "', " & intno & ", '" & Common.Parse(strname) & "', '" & Common.Parse(strValue) & "')"
            strsql = "insert Into CUSTOM_FIELDS (CF_COY_ID,CF_FIELD_NO,CF_FIELD_NAME,CF_FIELD_VALUE) values ('" & HttpContext.Current.Current.Session("CompanyID") & "', " & intno & ", '" & Common.Parse(strname) & "', '" & Common.Parse(strvalue) & "')"
            If objDb.Execute(strsql) Then
                addValue = WheelMsgNum.Save
            Else
                addValue = WheelMsgNum.NotSave
            End If
        End Function

        'fill ddl at create RFQ
        Function getVendListRFQ() As DataSet
            Dim strget As String
            Dim dsvenlist As DataSet

            'strget = "SELECT RVDLM_List_Index, RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = 'demo'"
            'strget = "SELECT RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = '" & HttpContext.Current.Current.Session("CompanyID") & "'"
            'strget = "SELECT RVDLM_List_Index, RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = '" & _
            'HttpContext.Current.Current.Session("CompanyID") & "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'"
            strget = "SELECT DISTINCT RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX,RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_NAME, RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID" & _
                        " FROM RFQ_VEN_DISTR_LIST_MSTR LEFT JOIN RFQ_VEN_DISTR_LIST_DETAIL ON" & _
                        " RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX = RFQ_VEN_DISTR_LIST_DETAIL.RVDLD_LIST_INDEX" & _
                        " WHERE RVDLM_USER_ID='" & HttpContext.Current.Current.Session("UserID") & "'"

            dsvenlist = objDb.FillDs(strget)
            Return dsvenlist
        End Function

        'meilai 01/12/2004 - retrieve vendor list 
        'Modified by Craven 19/04/2011
        Function getVenList(ByVal strlistname As String) As DataSet
            Dim strget As String
            Dim dsvenlist As DataSet
            Dim strTemp As String

            'strget = "SELECT RVDLM_List_Index, RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = 'demo'"
            'strget = "SELECT RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = '" & HttpContext.Current.Current.Session("CompanyID") & "'"
            'strget = "SELECT RVDLM_List_Index, RVDLM_List_Name from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id = '" & _
            'HttpContext.Current.Current.Session("CompanyID") & "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'"
            strget = "SELECT DISTINCT RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX,RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_NAME" & _
                        " FROM RFQ_VEN_DISTR_LIST_MSTR LEFT JOIN RFQ_VEN_DISTR_LIST_DETAIL ON" & _
                        " RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX = RFQ_VEN_DISTR_LIST_DETAIL.RVDLD_LIST_INDEX" & _
                        " LEFT JOIN COMPANY_MSTR ON RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID = COMPANY_MSTR.CM_COY_ID" & _
                        " WHERE RVDLM_USER_ID='" & HttpContext.Current.Current.Session("UserID") & "' AND RVDLM_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "'"
            If strlistname <> "" Then
                strTemp = Common.BuildWildCard(strlistname)
                strget = strget & " AND RVDLM_LIST_NAME" & Common.ParseSQL(strTemp)
            End If

            dsvenlist = objDb.FillDs(strget)
            Return dsvenlist


        End Function
        Public Function get_vendorlist(ByVal INDEX_ID As String, ByRef list_name() As String, ByRef list_id() As String, ByRef i As Integer, ByRef com_str As String, ByRef vendid() As String)
            Dim comid_str As String
            Dim strsql As String = "SELECT COMPANY_MSTR.CM_COY_ID, RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX,RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_NAME, RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID, COMPANY_MSTR.CM_COY_NAME" & _
                                    " FROM RFQ_VEN_DISTR_LIST_MSTR LEFT JOIN RFQ_VEN_DISTR_LIST_DETAIL ON " & _
                                    " RFQ_VEN_DISTR_LIST_MSTR.RVDLM_LIST_INDEX = RFQ_VEN_DISTR_LIST_DETAIL.RVDLD_LIST_INDEX" & _
                                    " LEFT JOIN COMPANY_MSTR ON RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID = COMPANY_MSTR.CM_COY_ID" & _
                                    " WHERE RVDLM_USER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                                    " and RVDLM_LIST_INDEX='" & Common.Parse(INDEX_ID) & "'"

            Dim tDS As DataSet
            tDS = objDb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                list_name(i) = tDS.Tables(0).Rows(j).Item("CM_COY_Name").ToString()
                vendid(i) = tDS.Tables(0).Rows(j).Item("CM_COY_ID").ToString()
                list_id(i) = tDS.Tables(0).Rows(j).Item("RVDLM_List_Index")
                If comid_str = "" Then
                    comid_str = "'" & tDS.Tables(0).Rows(j).Item("RVDLM_List_Index") & "'"
                Else
                    comid_str = comid_str & ",'" & tDS.Tables(0).Rows(j).Item("RVDLM_List_Index") & "'"
                End If
                i = i + 1
            Next
            If comid_str <> "" Then
                Dim strsql2 As String
                strsql2 = "select RCDLD_V_COY_ID " & _
                        "from RFQ_VEN_DISTR_LIST_DETAIL, RFQ_VEN_DISTR_LIST_MSTR " & _
                        "where RVDLD_LIST_INDEX in(" & comid_str & ") " & _
                        "AND RVDLD_LIST_INDEX = RVDLM_LIST_INDEX " & _
                        "AND  RVDLM_LIST_INDEX = '" & Common.Parse(INDEX_ID) & "' "
                tDS = objDb.FillDs(strsql2)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    If com_str = "" Then
                        com_str = "'" & tDS.Tables(0).Rows(j).Item("RCDLD_V_COY_ID") & "'"
                    Else
                        com_str = com_str & ",'" & tDS.Tables(0).Rows(j).Item("RCDLD_V_COY_ID") & "'"
                    End If
                Next
            End If
        End Function
        Public Function get_vendorName(ByVal INDEX_ID As String, ByRef com_name() As String, ByRef com_id() As String, ByRef COUNT As Integer, ByVal com_str As String)
            COUNT = 0
            Dim strsql As String = "select DISTINCT CM.CM_COY_NAME,CM.CM_COY_ID " & _
                                   " FROM COMPANY_MSTR CM,RFQ_INVITED_VENLIST RIV" & _
                                   " WHERE CM.CM_COY_ID=RIV.RIV_S_Coy_ID AND RIV_RFQ_ID='" & Common.Parse(INDEX_ID) & " '" & _
                                   " AND CM_STATUS = 'A'"
            If com_str <> "" Then
                strsql = strsql & " and RIV_S_Coy_ID not in(" & com_str & ")"
            End If

            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                com_name(COUNT) = tDS.Tables(0).Rows(j).Item("CM_COY_NAME")
                com_id(COUNT) = tDS.Tables(0).Rows(j).Item("CM_COY_ID")
                COUNT = COUNT + 1
            Next
        End Function
        Function getAryID(ByVal listname As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                       "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")

            strsql = "SELECT RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID, COMPANY_MSTR.CM_COY_NAME FROM RFQ_VEN_DISTR_LIST_DETAIL " & _
                        "LEFT JOIN COMPANY_MSTR ON RFQ_VEN_DISTR_LIST_DETAIL.RCDLD_V_COY_ID=COMPANY_MSTR.CM_COY_ID " & _
                        "WHERE RVDLD_LIST_INDEX='" & listindex & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Function Ven_AddList(ByVal list_index As String, ByVal com_id As String) As String

            If objDb.Exist("select '*' from rfq_Ven_distr_list_detail where RVDLD_LIST_INDEX='" & list_index & _
            "' and RCDLD_V_COY_ID='" & com_id & "'") > 0 Then
                Ven_AddList = "Record Exist!!!"
                Exit Function
            End If

            Dim strsql As String = "insert into rfq_Ven_distr_list_detail(RVDLD_LIST_INDEX, RCDLD_V_COY_ID) Values ('" & list_index & "','" & com_id & "') "

            Ven_AddList = strsql
        End Function
        Function chkAddVenList(ByVal listname As String) As Integer
            Dim strCoyID, strUserId As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                        "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'") > 0 Then
                Return WheelMsgNum.Duplicate
                Exit Function
            End If
        End Function
        Function chkModVenList(ByVal listname As String, ByVal oldlistname As String) As Integer
            Dim strCoyID, strUserId As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            If listname <> oldlistname Then
                If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                    "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'") > 0 Then
                    Return WheelMsgNum.Duplicate
                    Exit Function
                End If
            End If
        End Function
        ' Modified by Craven 19/04/2011
        Function addVenList(ByVal listname As String, ByVal companyid As String) As Integer
            Dim strsql, strsql2 As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                        "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")

            If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "' AND (SELECT RVDLD_LIST_INDEX FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "' AND RCDLD_V_COY_ID='" & companyid & "')") > 0 Then
                Return WheelMsgNum.Duplicate
                Exit Function
            Else
                If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'") > 0 Then

                    strsql2 = "INSERT INTO RFQ_VEN_DISTR_LIST_DETAIL(RVDLD_LIST_INDEX,RCDLD_V_COY_ID) VALUES('" & listindex & "','" & companyid & "')"
                Else
                    strsql = "Insert Into RFQ_VEN_DISTR_LIST_MSTR(RVDLM_User_Id,RVDLM_Coy_Id,RVDLM_List_Name)values ('" & HttpContext.Current.Current.Session("UserID") & "','" & HttpContext.Current.Current.Session("CompanyID") & "','" & listname & "')"
                    objDb.Execute(strsql)

                    listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                    "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")

                    strsql2 = "INSERT INTO RFQ_VEN_DISTR_LIST_DETAIL(RVDLD_LIST_INDEX,RCDLD_V_COY_ID) VALUES('" & listindex & "','" & companyid & "')"
                End If
                objDb.Execute(strsql2)
                Return WheelMsgNum.Save
            End If
        End Function
        'Modified by Craven 20/4/2011
        Function delVendorList(ByVal listname As String, ByVal companyid As String) As Integer
            Dim strdel As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")


            strdel = "DELETE FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "' AND RCDLD_V_COY_ID='" & companyid & "'"
            If objDb.Execute(strdel) Then
                If objDb.Execute(strdel) Then
                    If objDb.Exist("SELECT * FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "'") > 0 Then
                        Return WheelMsgNum.Delete
                    Else
                        objDb.Execute("DELETE FROM RFQ_VEN_DISTR_LIST_MSTR WHERE RVDLM_LIST_INDEX='" & listindex & "' AND RVDLM_COY_ID='" & strCoyID & "' AND RVDLM_LIST_NAME='" & listname & "' AND RVDLM_User_Id='" & strUserId & "'")
                        Return 10 'represent change the session user action to add because there is no more data in RFQ_VEN_DISTR_LIST_MSTR
                    End If

                Else
                    Return WheelMsgNum.NotDelete
                End If
            End If
        End Function
        Function delVenList(ByVal listname As String, ByVal companyid As String) As Integer
            Dim strdel As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")


            strdel = "DELETE FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "'"
            If objDb.Execute(strdel) Then
                If objDb.Execute(strdel) Then
                    If objDb.Exist("SELECT * FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "'") > 0 Then
                        Return WheelMsgNum.Delete
                    Else
                        objDb.Execute("DELETE FROM RFQ_VEN_DISTR_LIST_MSTR WHERE RVDLM_LIST_INDEX='" & listindex & "' AND RVDLM_COY_ID='" & strCoyID & "' AND RVDLM_LIST_NAME='" & listname & "' AND RVDLM_User_Id='" & strUserId & "'")
                        Return 10 'represent change the session user action to add because there is no more data in RFQ_VEN_DISTR_LIST_MSTR
                    End If

                Else
                    Return WheelMsgNum.NotDelete
                End If
            End If
        End Function
        Function ClearVendorList(ByVal oldlistname As String)
            Dim strsql As String
            Dim updatename As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
           "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")
            'Clear database
            objDb.Execute("DELETE FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX ='" & listindex & "'")
        End Function
        Function modListName(ByVal listname As String, ByVal oldlistname As String)
            Dim updatename As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")
            'Update List name
            'if listname already exist
            If listname <> oldlistname Then
                updatename = "UPDATE RFQ_VEN_DISTR_LIST_MSTR SET RVDLM_LIST_NAME='" & listname & "' WHERE RVDLM_LIST_INDEX='" & listindex & "'"
                objDb.Execute(updatename)
                Return WheelMsgNum.Save
            End If
        End Function
        'modified by Craven 20/4/2011
        Function modVendorList(ByVal listname As String, ByVal oldlistname As String, ByVal companyid As String) As Integer
            Dim strsql As String
            Dim updatename As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")
            'check duplicate data
            If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
                        "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "' AND (SELECT RVDLD_LIST_INDEX FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "' AND RCDLD_V_COY_ID='" & companyid & "')") > 0 Then
                Return WheelMsgNum.Duplicate
                Exit Function
            End If
            'Update List Vendor
            strsql = "INSERT INTO RFQ_VEN_DISTR_LIST_DETAIL(RVDLD_LIST_INDEX,RCDLD_V_COY_ID) VALUES('" & listindex & "','" & companyid & "')"

            If objDb.Execute(strsql) Then
                Return WheelMsgNum.Save
            Else
                Return WheelMsgNum.NotSave
            End If


            'If UCase(strOld) <> UCase(strlistname) Then
            '    If objDb.Exist("Select '*' from RFQ_VEN_DISTR_LIST_MSTR where RVDLM_Coy_Id ='" & _
            '                    HttpContext.Current.Current.Session("CompanyID") & "' and RVDLM_User_Id ='" & _
            '                     HttpContext.Current.Current.Session("UserID") & "' and RVDLM_List_Name ='" & _
            '                     Common.Parse(strlistname) & "'") > 0 Then

            '        modVendorList = WheelMsgNum.Duplicate
            '        Exit Function
            '    End If
            'End If

            'strsql = "update RFQ_VEN_DISTR_LIST_MSTR set "
            'strsql &= "RVDLM_List_Name = '" & Common.Parse(strlistname) & "'"
            'strsql &= "WHERE RVDLM_List_Index ='" & Common.Parse(strlistindex) & "' "


            'If objDb.Execute(strsql) Then
            '    modVendorList = WheelMsgNum.Save
            'Else
            '    modVendorList = WheelMsgNum.NotSave


            'End If
        End Function
        Function modVendorList2(ByVal listname As String, ByVal oldlistname As String, ByVal companyid As String) As Integer
            Dim strsql, strsql2 As String
            Dim updatename As String
            Dim strCoyID, strUserId, listindex As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")
            listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
            "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")

            If listindex = "" Then
                'if listname doesn't exit
                strsql = "Insert Into RFQ_VEN_DISTR_LIST_MSTR(RVDLM_User_Id,RVDLM_Coy_Id,RVDLM_List_Name)values ('" & HttpContext.Current.Current.Session("UserID") & "','" & HttpContext.Current.Current.Session("CompanyID") & "','" & listname & "')"
                objDb.Execute(strsql)

                listindex = objDb.Get1Column("RFQ_VEN_DISTR_LIST_MSTR", "RVDLM_LIST_INDEX", " WHERE RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & listname & _
                "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "'")

                strsql2 = "INSERT INTO RFQ_VEN_DISTR_LIST_DETAIL(RVDLD_LIST_INDEX,RCDLD_V_COY_ID) VALUES('" & listindex & "','" & companyid & "')"
                objDb.Execute(strsql2)
                Return WheelMsgNum.Save
            Else
                'if listname already exist
                If objDb.Exist("Select * From RFQ_VEN_DISTR_LIST_MSTR Where RVDLM_COY_ID='" & strCoyID & "' And RVDLM_LIST_NAME='" & oldlistname & _
                        "' AND RVDLM_User_Id='" & HttpContext.Current.Current.Session("UserID") & "' AND (SELECT RVDLD_LIST_INDEX FROM RFQ_VEN_DISTR_LIST_DETAIL WHERE RVDLD_LIST_INDEX='" & listindex & "' AND RCDLD_V_COY_ID='" & companyid & "')") > 0 Then
                    Return WheelMsgNum.Duplicate
                    Exit Function
                End If

                strsql = "INSERT INTO RFQ_VEN_DISTR_LIST_DETAIL(RVDLD_LIST_INDEX,RCDLD_V_COY_ID) VALUES('" & listindex & "','" & companyid & "')"

                If objDb.Execute(strsql) Then
                    Return WheelMsgNum.Save
                Else
                    Return WheelMsgNum.NotSave
                End If
            End If
        End Function
        ' Date modified : 09/01/2006
        ' Description   : only display active company
        Public Function DisplayVenDetail(ByVal strName As String, ByVal strvalue As String) As DataSet
            Dim strSql As String
            Dim dsAddr As DataSet
            'strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & strName & "' " & _
            ' "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            'strSql = "select CM_COY_NAME, CM_ADDR_LINE1 from company_mstr, rfq_ven_distr_list_detail, rfq_ven_distr_list_mstr where RVDLM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and RVDLM_List_Index=" & strvalue & " and CM_Coy_Id = RCDLD_V_Coy_Id and RVDLM_List_Index=RVDLD_List_INDEX"
            '*******************meilai 19/1/2005**************
            'strSql = "select CM_Coy_Id,CM_COY_NAME, (CM_ADDR_LINE1 +','+ CM_ADDR_LINE2 +','+ CM_ADDR_LINE3 +','+ CM_postcode +','+ cm_city +','+ cm_state +','+ cm_country) as cm_addr from company_mstr, rfq_ven_distr_list_detail, rfq_ven_distr_list_mstr where RVDLM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and RVDLM_List_Index=" & strvalue & " and CM_Coy_Id = RCDLD_V_Coy_Id and RVDLM_List_Index=RVDLD_List_INDEX"
            'strSql = "select CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_postcode, cm_city, s.code_desc as State,b.code_desc as Country from company_mstr left join code_mstr as s on cm_country=s.code_abbr and s.code_category='CT' left join code_mstr as b on cm_state=b.code_abbr and b.code_category='S' left join rfq_ven_distr_list_detail on CM_Coy_Id=RCDLD_V_Coy_Id left join rfq_ven_distr_list_mstr on RVDLM_List_Index=RVDLD_List_INDEX where RVDLM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and RVDLM_List_Index=" & strvalue & " and CM_Coy_Id = RCDLD_V_Coy_Id and RVDLM_List_Index=RVDLD_List_INDEX"
            strSql = "select CM_Coy_Id,CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3," & _
            " CM_postcode, cm_city, s.code_desc as State, b.code_desc as Country from company_mstr " & _
            " left join code_mstr as s on cm_state=s.code_abbr and s.code_category='S' " & _
            " left join code_mstr as b on cm_country=b.code_abbr and b.code_category='CT' " & _
            " left join rfq_ven_distr_list_detail on CM_Coy_Id=RCDLD_V_Coy_Id left join rfq_ven_distr_list_mstr " & _
            " on RVDLM_List_Index=RVDLD_List_INDEX where RVDLM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            "  and RVDLM_List_Index=" & strvalue & " and CM_Coy_Id = RCDLD_V_Coy_Id and RVDLM_List_Index=RVDLD_List_INDEX AND CM_STATUS = 'A' "
            '*****************************************************"
            dsAddr = objDb.FillDs(strSql)
            DisplayVenDetail = dsAddr
        End Function

        Function delVenDetail(ByVal v_com_id As String, ByVal VenId As Integer) As String

            Dim strdel As String
            strdel = " delete from rfq_Ven_distr_list_detail where rvdld_list_index='" & VenId & "' and rcdld_v_coy_id='" & v_com_id & "'"
            objDb.Execute(strdel)

        End Function


        'Public Function modValue(ByVal intno As Integer, ByVal strname As String, ByVal strvalue As String) As String
        '    'Dim intno As Integer
        '    Dim strsql As String
        '    'addValue = WheelMsgNum.Duplicate
        '    If objDb.Exist("Select '*' From CUSTOM_FIELDS Where CF_FIELD_NO = " & intno & " and CF_COY_ID = ' " & HttpContext.Current.Current.Session("CompanyID") & "' and CF_FIELD_VALUE = ''") > 0 Then
        '        strsql = "update custom_fields set CF_FIELD_VALUE = '" & Common.Parse(strvalue) & "' where  CF_FIELD_NO =  " & intno
        '    Else

        '        strsql = "insert Into CUSTOM_FIELDS values ('" & HttpContext.Current.Current.Session("CompanyID") & "', " & intno & ", '" & Common.Parse(strname) & "', '" & Common.Parse(strvalue) & "')"
        '    End If
        '    If objDb.Execute(strsql) Then
        '        modValue = WheelMsgNum.Save
        '    Else
        '        modValue = WheelMsgNum.NotSave
        '    End If
        'End Function




    End Class
End Namespace

