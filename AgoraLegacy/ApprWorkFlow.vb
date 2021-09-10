'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports AgoraLegacy

Namespace AgoraLegacy

    Public Enum ApprovalType
        PR = 0
        Invoice = 1
    End Enum

    Public Class AppAmountCurrent
        Public Appval As String
    End Class

    Public Class ApprWorkFlow
        Dim objDb As New EAD.DBCom

        Public Function get_AppLimitAO(ByVal items As AppAmountCurrent, ByVal strAOid As String, Optional ByVal strType As String = "")
            Dim strsql As String
            strsql = " SELECT UM_USER_ID "
            If strType = "PO" Then
                strsql &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
                'Arif,05062013 - For eMRS
            ElseIf strType = "MRS" Then
                strsql &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
                'End-Arif,05062013
                'Zulham 04072018 - PAMB
            ElseIf strType = "E2P" Then
                strsql &= ", um_invoice_app_limit AS UM_APP_LIMIT "
            Else
                strsql &= ", UM_APP_LIMIT "
            End If
            strsql &= " FROM USER_MSTR M "
            strsql &= " WHERE UM_USER_ID = '" & strAOid & "' "
            strsql &= " AND UM_STATUS ='A' "
            strsql &= " AND UM_DELETED='N' AND UM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                items.Appval = tDS.Tables(0).Rows(0).Item("UM_APP_LIMIT").ToString.Trim
            End If
        End Function

        Public Function get_AppLimitFO(ByVal items As AppAmountCurrent, ByVal strAOid As String)
            Dim strsql As String = "SELECT UM_USER_ID, UM_INVOICE_APP_LIMIT FROM USER_MSTR M " & _
                                    "WHERE UM_USER_ID = '" & strAOid & "' " & _
                                    "AND UM_STATUS ='A' " & _
                                    "AND UM_DELETED='N' AND UM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                items.Appval = tDS.Tables(0).Rows(0).Item("UM_INVOICE_APP_LIMIT").ToString.Trim
            End If
        End Function
        Public Function get_IPPAppLimit(ByVal items As AppAmountCurrent, ByVal strAOid As String)
#Region "mimi 2018-05-17 : remark for change e2p app limit"
            'Dim strsql As String = "SELECT UM_USER_ID, UM_IPP_APP_LIMIT FROM USER_MSTR M " & _
            '                        "WHERE UM_USER_ID = '" & strAOid & "' " & _
            '                        "AND UM_STATUS ='A' " & _
            '                        "AND UM_DELETED='N' AND UM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
#End Region
            'mimi 2018-05-17 : E2P invoice app limit
            Dim strsql As String = "SELECT UM_USER_ID, UM_INVOICE_APP_LIMIT FROM USER_MSTR M " &
                                    "WHERE UM_USER_ID = '" & strAOid & "' " &
                                    "AND UM_STATUS ='A' " &
                                    "AND UM_DELETED='N' AND UM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                'items.Appval = tDS.Tables(0).Rows(0).Item("UM_IPP_APP_LIMIT").ToString.Trim
                items.Appval = tDS.Tables(0).Rows(0).Item("UM_INVOICE_APP_LIMIT").ToString.Trim
            End If
        End Function

        Public Function get_AppLIMIT(ByVal strAOid As String, ByVal valindex As String, ByVal valseq As String, Optional ByVal strType As String = "") As DataSet
            Dim strSQL As String
            Dim dsAPP As New DataSet

            strSQL = "SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO "
            If strType = "PO" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
                'Arif,05062013 - For eMRS
            ElseIf strType = "MRS" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
                'End-Arif,05062013
            Else
                strSQL &= ", UM_APP_LIMIT "
            End If
            strSQL &= "FROM APPROVAL_GRP_AO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_TYPE<>'INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGA_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
            strSQL &= "WHERE AGA_SEQ = ( "
            strSQL &= "Select MAX(AGA_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_AO "
            strSQL &= "where AGA_GRP_INDEX = '" & valindex & "' AND AGA_SEQ < '" & valseq & "' "
            'Michelle (22/11/2010) - To cater for PO
            If strType = "PO" Then
                strSQL &= " AND AGM_TYPE = 'PO' "
            End If

            'Arif,05062013 - To cater for eMRS
            If strType = "MRS" Then
                strSQL &= " AND AGM_TYPE = 'MRS' "
            End If
            'End-Arif,05062013
            strSQL &= "GROUP BY AGA_GRP_INDEX) AND AGA_GRP_INDEX = '" & valindex & "' "
            strSQL &= "union "
            strSQL &= "SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO "
            If strType = "PO" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
            Else
                strSQL &= ", UM_APP_LIMIT "
            End If
            strSQL &= "FROM APPROVAL_GRP_AO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_TYPE<>'INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGA_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
            strSQL &= "WHERE AGA_SEQ = ( "
            strSQL &= "Select MIN(AGA_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_AO "
            strSQL &= "where AGA_GRP_INDEX = '" & valindex & "' AND AGA_SEQ > '" & valseq & "' "
            If strType = "PO" Then
                strSQL &= " AND AGM_TYPE = 'PO' "
            End If

            'Arif,05062013 - To cater for eMRS
            If strType = "MRS" Then
                strSQL &= " AND AGM_TYPE = 'MRS' "
            End If
            'End-Arif,05062013
            strSQL &= "GROUP BY AGA_GRP_INDEX) AND AGA_GRP_INDEX = '" & valindex & "'"
            
            dsAPP = objDb.FillDs(strSQL)
            get_AppLIMIT = dsAPP

        End Function

        Public Function get_AppFOLIMIT(ByVal strAOid As String, ByVal valindex As String, ByVal valseq As String) As DataSet
            Dim strSQL As String
            Dim dsAPP As New DataSet

            strSQL = "SELECT AGFO_GRP_INDEX AS AGA_GRP_INDEX, AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, UM_INVOICE_APP_LIMIT AS UM_APP_LIMIT "
            strSQL &= "FROM APPROVAL_GRP_FO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
            strSQL &= "WHERE AGFO_SEQ = ( "
            strSQL &= "Select MAX(AGFO_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_FO "
            strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ < '" & valseq & "' "
            strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "' "
            strSQL &= "union "
            strSQL &= "SELECT AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, UM_INVOICE_APP_LIMIT "
            strSQL &= "FROM APPROVAL_GRP_FO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
            strSQL &= "WHERE AGFO_SEQ = ( "
            strSQL &= "Select MIN(AGFO_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_FO "
            strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ > '" & valseq & "' "
            strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "'"

            dsAPP = objDb.FillDs(strSQL)
            get_AppFOLIMIT = dsAPP

        End Function
        Public Function get_AppIPPLIMIT(ByVal strAOid As String, ByVal valindex As String, ByVal valseq As String, ByVal type As String, ByVal role As String) As DataSet
            Dim strSQL As String
            Dim dsAPP As New DataSet

#Region "mimi 2018-05-17 :remark for change e2p app limit"
            'strSQL = "SELECT AGFO_GRP_INDEX AS AGA_GRP_INDEX, AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, UM_IPP_APP_LIMIT AS UM_APP_LIMIT "
            'strSQL &= "FROM APPROVAL_GRP_FO "
            'strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='IPP' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
            'strSQL &= "WHERE AGFO_SEQ = ( "
            'strSQL &= "Select MAX(AGFO_SEQ) "
            'strSQL &= "FROM APPROVAL_GRP_FO "
            'strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ < '" & valseq & "' "
            'strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "' "
            'strSQL &= "union "
            'strSQL &= "SELECT AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, UM_IPP_APP_LIMIT "
            'strSQL &= "FROM APPROVAL_GRP_FO "
            'strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='IPP' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
            'strSQL &= "WHERE AGFO_SEQ = ( "
            'strSQL &= "Select MIN(AGFO_SEQ) "
            'strSQL &= "FROM APPROVAL_GRP_FO "
            'strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ > '" & valseq & "' "
            'strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "'"
#End Region

            'Zulham 04072018 - PAMB
            If role.ToUpper = "FO" Then
                'mimi 2018-05-17 : E2P invoice app limit
                strSQL = "SELECT AGFO_GRP_INDEX AS AGA_GRP_INDEX, AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, UM_INVOICE_APP_LIMIT AS UM_APP_LIMIT "
                strSQL &= "FROM APPROVAL_GRP_FO "
                strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='" & type & "' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
                strSQL &= "WHERE AGFO_SEQ = ( "
                strSQL &= "Select MAX(AGFO_SEQ) "
                strSQL &= "FROM APPROVAL_GRP_FO "
                strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ < '" & valseq & "' "
                strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "' "
                strSQL &= "union "
                strSQL &= "SELECT AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, UM_INVOICE_APP_LIMIT "
                strSQL &= "FROM APPROVAL_GRP_FO "
                strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX AND AGM_TYPE='" & type & "' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGFO_FO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
                strSQL &= "WHERE AGFO_SEQ = ( "
                strSQL &= "Select MIN(AGFO_SEQ) "
                strSQL &= "FROM APPROVAL_GRP_FO "
                strSQL &= "where AGFO_GRP_INDEX = '" & valindex & "' AND AGFO_SEQ > '" & valseq & "' "
                strSQL &= "GROUP BY AGFO_GRP_INDEX) AND AGFO_GRP_INDEX = '" & valindex & "'"
            Else
                'mimi 2018-05-17 : E2P invoice app limit
                strSQL = "SELECT AGA_GRP_INDEX AS AGA_GRP_INDEX, AGA_SEQ AS AGA_SEQ, AGA_AO AS AGA_AO, UM_INVOICE_APP_LIMIT AS UM_APP_LIMIT "
                strSQL &= "FROM APPROVAL_GRP_AO "
                strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_TYPE='" & type & "' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGA_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
                strSQL &= "WHERE AGA_SEQ = ( "
                strSQL &= "Select MAX(AGA_SEQ) "
                strSQL &= "FROM APPROVAL_GRP_AO "
                strSQL &= "where AGA_GRP_INDEX = '" & valindex & "' AND AGA_SEQ < '" & valseq & "' "
                strSQL &= "GROUP BY AGA_GRP_INDEX) AND AGA_GRP_INDEX = '" & valindex & "' "
                strSQL &= "union "
                strSQL &= "SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO, UM_INVOICE_APP_LIMIT "
                strSQL &= "FROM APPROVAL_GRP_AO "
                strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_TYPE='" & type & "' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGA_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
                strSQL &= "WHERE AGA_SEQ = ( "
                strSQL &= "Select MIN(AGA_SEQ) "
                strSQL &= "FROM APPROVAL_GRP_AO "
                strSQL &= "where AGA_GRP_INDEX = '" & valindex & "' AND AGA_SEQ > '" & valseq & "' "
                strSQL &= "GROUP BY AGA_GRP_INDEX) AND AGA_GRP_INDEX = '" & valindex & "'"
            End If

            dsAPP = objDb.FillDs(strSQL)
            get_AppIPPLIMIT = dsAPP

        End Function
        Function getdeptname(ByVal index As String) As String
            Dim cond As String
            Dim deptname As String
            cond = " LEFT JOIN COMPANY_DEPT_MSTR ON " & _
            "CDM_DEPT_CODE = AGM_DEPT_CODE AND CDM_COY_ID = AGM_COY_ID WHERE AGM_GRP_INDEX = '" & index & "'"
            deptname = objDb.Get1ColumnCheckNull("APPROVAL_GRP_MSTR", "CDM_DEPT_NAME", cond)
            Return deptname
        End Function

        Function getIQCType(ByVal index As String) As String
            Dim strget As String
            Dim iqctype As String
            strget = "SELECT IFNULL(AGM_IQC_TYPE,'') AS AGM_IQC_TYPE FROM APPROVAL_GRP_MSTR " & _
                    "WHERE AGM_GRP_INDEX = '" & index & "'"
            iqctype = objDb.GetVal(strget)
            Return iqctype
        End Function

        Function getAppWorkFlow(ByVal strValGroup As String, Optional ByVal strWhere As String = Nothing) As DataSet
            Dim strget As String
            Dim dsApp As New DataSet
            Dim strTemp As String

            'Zulham 09072018 - PAMB
            'Added agm_resident TO the select STATEMENT
            strget = "SELECT distinct A.AGM_GRP_INDEX, A.AGM_COY_ID,A.AGM_GRP_NAME, A.AGM_CONSOLIDATOR, A.AGM_IQC_TYPE, A.AGM_MRS_EMAIL1, A.AGM_MRS_EMAIL2, A.AGM_TYPE, " &
                     "U.UM_USER_ID, U.UM_USER_NAME,CDM_DEPT_NAME,CDM_DEPT_CODE, AGM_RESIDENT " &
                     "FROM APPROVAL_GRP_MSTR A LEFT OUTER JOIN USER_MSTR U " &
                     "ON U.UM_USER_ID = A.AGM_CONSOLIDATOR AND U.UM_COY_ID = A.AGM_COY_ID " &
                     "LEFT JOIN COMPANY_DEPT_MSTR ON AGM_DEPT_CODE = CDM_DEPT_CODE AND AGM_COY_ID = CDM_COY_ID AND CDM_DELETED='N' " & 'Jules 2018.11.02 added CDM_DELETED.
                     "where A.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' "

            If strValGroup <> "" Then
                strTemp = Common.BuildWildCard(strValGroup)
                strget = strget & "AND AGM_GRP_NAME " & Common.ParseSQL(strTemp) & ""
            End If

            If strWhere <> "" Then
                strget = strget & " AND " & strWhere
            End If

            dsApp = objDb.FillDs(strget)

            getAppWorkFlow = dsApp
        End Function

        Public Function getPaymentAppGrpList() As DataSet
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT distinct A.AGM_GRP_INDEX, A.AGM_COY_ID,A.AGM_GRP_NAME, A.AGM_CONSOLIDATOR, A.AGM_TYPE, " & _
                     "U.UM_USER_ID, U.UM_USER_NAME " & _
                     "FROM APPROVAL_GRP_MSTR A LEFT OUTER JOIN USER_MSTR U " & _
                     "ON U.UM_USER_ID = A.AGM_CONSOLIDATOR AND U.UM_COY_ID = A.AGM_COY_ID " & _
                     "where A.AGM_TYPE='INV' AND A.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' "

            dsApp = objDb.FillDs(strget)

            getPaymentAppGrpList = dsApp
        End Function
        Public Function getIPPAppGrpList() As DataSet
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT distinct A.AGM_GRP_INDEX, A.AGM_COY_ID,A.AGM_GRP_NAME, A.AGM_CONSOLIDATOR, A.AGM_TYPE, " & _
                     "U.UM_USER_ID, U.UM_USER_NAME " & _
                     "FROM APPROVAL_GRP_MSTR A LEFT OUTER JOIN USER_MSTR U " & _
                     "ON U.UM_USER_ID = A.AGM_CONSOLIDATOR AND U.UM_COY_ID = A.AGM_COY_ID " & _
                     "where A.AGM_TYPE='IPP' AND A.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' "

            dsApp = objDb.FillDs(strget)

            getIPPAppGrpList = dsApp
        End Function
        'Zulham 09102018 - PAMB
        Public Function getPaymentAppList(ByVal intIndex As String, Optional ByVal strResidence As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'Zulham 09102018 - PAMB P2P
            If strResidence = "" Then
                strsql = "SELECT DISTINCT AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, ISNULL(AGFO_A_FO, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFO_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FO' AS type, AGFO_GRP_INDEX AS 'DeptIndex'  "
                strsql &= "FROM APPROVAL_GRP_FO "
                strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFO_FO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFO_A_FO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "WHERE AGFO_GRP_INDEX IN (" & intIndex & ") "

                strsql &= " UNION "

                strsql &= "SELECT DISTINCT AGFM_SEQ AS AGA_SEQ, AGFM_FM AS AGA_AO, ISNULL(AGFM_A_FM, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFM_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FM' AS type, AGFM_GRP_INDEX AS 'DeptIndex'  "
                strsql &= "FROM APPROVAL_GRP_FM "
                strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFM_FM AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFM_A_FM AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "WHERE AGFM_GRP_INDEX IN (" & intIndex & ") "
            Else
                If intIndex <> "" Then
                    'strsql = "SELECT DISTINCT AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, ISNULL(AGFO_A_FO, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFO_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FO' AS type, AGFO_GRP_INDEX AS 'DeptIndex'  "
                    'strsql &= "FROM APPROVAL_GRP_FO "
                    'strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFO_FO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFO_A_FO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'strsql &= "JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX "
                    'strsql &= "WHERE AGFO_GRP_INDEX IN (" & intIndex & ") "
                    'strsql &= "AND AGM_RESIDENT = '" & strResidence & "' "

                    'strsql &= " UNION "

                    'strsql &= "SELECT DISTINCT AGFM_SEQ AS AGA_SEQ, AGFM_FM AS AGA_AO, ISNULL(AGFM_A_FM, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFM_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FM' AS type, AGFM_GRP_INDEX AS 'DeptIndex' "
                    'strsql &= "FROM APPROVAL_GRP_FM "
                    'strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFM_FM AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFM_A_FM AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'strsql &= "JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFM_GRP_INDEX "
                    'strsql &= "WHERE AGFM_GRP_INDEX IN (" & intIndex & ") "
                    'strsql &= "AND AGM_RESIDENT = '" & strResidence & "' "

                    strsql = "Select DISTINCT agm_grp_name, AGFO_SEQ As AGA_SEQ, AGFO_FO As AGA_AO, ISNULL(AGFO_A_FO, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFO_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FO' AS TYPE, "
                    strsql &= "AGFO_GRP_INDEX AS 'DeptIndex', D.UM_USER_NAME AS FM_NAME, E.UM_USER_NAME as AFM_NAME "
                    strsql &= "From APPROVAL_GRP_FO "
                    strsql &= "INNER Join USER_MSTR AS B ON B.UM_USER_ID = AGFO_FO And B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "Left Join USER_MSTR AS C ON C.UM_USER_ID = AGFO_A_FO And C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "Join APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX "
                    strsql &= "Right Join APPROVAL_GRP_FM ON AGM_GRP_INDEX = AGFM_GRP_INDEX "
                    strsql &= "INNER Join USER_MSTR AS D ON D.UM_USER_ID = AGFM_FM And D.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "Left Join USER_MSTR AS E ON E.UM_USER_ID = AGFM_A_FM And E.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "WHERE AGFO_GRP_INDEX IN (" & intIndex & ") "
                    strsql &= "And AGM_RESIDENT = '" & strResidence & "' "
                Else
                    strsql = "SELECT DISTINCT AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, ISNULL(AGFO_A_FO, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFO_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FO' AS type, AGFO_GRP_INDEX AS 'DeptIndex'  "
                    strsql &= "FROM APPROVAL_GRP_FO "
                    strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFO_FO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFO_A_FO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFO_GRP_INDEX "
                    strsql &= "AND AGM_RESIDENT = '" & strResidence & "' "

                    strsql &= " UNION "

                    strsql &= "SELECT DISTINCT AGFM_SEQ AS AGA_SEQ, AGFM_FM AS AGA_AO, ISNULL(AGFM_A_FM, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGFM_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FM' AS type, AGFM_GRP_INDEX AS 'DeptIndex' "
                    strsql &= "FROM APPROVAL_GRP_FM "
                    strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFM_FM AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFM_A_FM AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strsql &= "JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGFM_GRP_INDEX "
                    strsql &= "AND AGM_RESIDENT = '" & strResidence & "' "
                End If
            End If


            'Michelle (23/9/2010) - To cater for MYSQL syntax
            ' strsql &= "ORDER BY type DESC, AGA_SEQ, B.UM_APP_LIMIT;"
            'Zulham 10102018 - PAMB
            strsql &= "ORDER BY DeptIndex, type DESC, AGA_SEQ, UM_APP_LIMIT;"

            'strsql &= "ORDER BY B.UM_APP_LIMIT; "

            ds = objDb.FillDs(strsql)

            getPaymentAppList = ds
        End Function
        Public Function getIPPAppList(ByVal intIndex As Integer) As DataSet
            Dim strsql As String
            Dim ds As New DataSet



            strsql = "SELECT DISTINCT AGA_SEQ AS AGA_SEQ, AGA_AO AS AGA_AO, ISNULL(AGA_A_AO, '') AS AGA_A_AO, B.UM_APP_LIMIT, AGA_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'AO' AS type "
            strsql &= "FROM APPROVAL_GRP_AO "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "WHERE AGA_GRP_INDEX = " & intIndex & " "

            strsql &= " UNION "

            strsql &= "SELECT DISTINCT AGFO_SEQ AS AGA_SEQ, AGFO_FO AS AGA_AO, ISNULL(AGFO_A_FO, '') AS AGA_A_AO, C.UM_APP_LIMIT, AGFO_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FO' AS type  "
            strsql &= "FROM APPROVAL_GRP_FO "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFO_FO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFO_A_FO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "WHERE AGFO_GRP_INDEX = " & intIndex & " "

            strsql &= " UNION "

            strsql &= "SELECT DISTINCT AGFM_SEQ AS AGA_SEQ, AGFM_FM AS AGA_AO, ISNULL(AGFM_A_FM, '') AS AGA_A_AO, C.UM_APP_LIMIT, AGFM_RELIEF_IND AS AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, 'FM' AS type "
            strsql &= "FROM APPROVAL_GRP_FM "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGFM_FM AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGFM_A_FM AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "WHERE AGFM_GRP_INDEX = " & intIndex & " "

            'Michelle (23/9/2010) - To cater for MYSQL syntax
            ' strsql &= "ORDER BY type DESC, AGA_SEQ, B.UM_APP_LIMIT;"
            strsql &= "ORDER BY TYPE DESC, AGA_SEQ, UM_APP_LIMIT;"

            'strsql &= "ORDER BY B.UM_APP_LIMIT; "

            ds = objDb.FillDs(strsql)

            getIPPAppList = ds
        End Function

        Function BindAppGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
			'Modified for IPP GST Stage 2A - CH
            strget = "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID,A.AGA_SEQ,A.AGA_GRP_INDEX, " & _
                     "B.UM_USER_NAME as AO_NAME,B.UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGA_RELIEF_IND, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE,'' as AAO_ID2,'' as AAO_ID3,'' as AAO_ID4,'' as AGA_OFFICER_TYPE, AGA_BRANCH_CODE, AGA_CC_CODE " & _
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "where M.AGM_TYPE<>'INV' AND A.AGA_GRP_INDEX ='" & valgroup & "' " & _
                     "AND M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "ORDER BY A.AGA_SEQ"
			'---------------------------------

            dsApp = objDb.FillDs(strget)
            BindAppGroupAsg = dsApp
        End Function

        Function BindFinanceAppGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
            Dim objDB As New EAD.DBCom
            'Michelle (23/9/2010) - To cater for MYSQL syntax
            'strget = "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFO_SEQ AS SEQ, 'FO - ' + CAST(A.AGFO_SEQ AS VARCHAR) AS AGA_SEQ,A.AGFO_GRP_INDEX AS AGA_GRP_INDEX, " & _
            strget = "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFO_SEQ AS SEQ, " & objDB.Concat3(" - ", "", "'FO'", "CAST(A.AGFO_SEQ AS CHAR)") & " AS AGA_SEQ,A.AGFO_GRP_INDEX AS AGA_GRP_INDEX, " & _
                      "B.UM_USER_NAME as AO_NAME,B.UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGFO_RELIEF_IND AS AGA_RELIEF_IND, " & _
                      "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                      "CASE WHEN AGFO_A_FO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, 'FO' AS type " & _
                      "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                      "APPROVAL_GRP_FO A ON M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                      "LEFT OUTER JOIN USER_MSTR B ON A.AGFO_FO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                      "LEFT OUTER JOIN USER_MSTR C ON A.AGFO_A_FO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                      "where M.AGM_TYPE='INV' AND A.AGFO_GRP_INDEX ='" & valgroup & "' " & _
                      "AND M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                      "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "

            strget = strget & " UNION "

            'Michelle (23/9/2010) - To cater for MYSQL syntax
            'strget = strget & "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFM_SEQ AS SEQ, 'FM - ' + CAST(A.AGFM_SEQ AS VARCHAR) AS AGA_SEQ,A.AGFM_GRP_INDEX AS AGA_GRP_INDEX, " & _
            strget = strget & "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFM_SEQ AS SEQ, " & objDB.Concat3(" - ", "", "'FM'", "CAST(A.AGFM_SEQ AS CHAR)") & "  AS AGA_SEQ,A.AGFM_GRP_INDEX AS AGA_GRP_INDEX, " & _
                      "B.UM_USER_NAME as AO_NAME,B.UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGFM_RELIEF_IND AS AGA_RELIEF_IND, " & _
                      "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                      "CASE WHEN AGFM_A_FM IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, 'FM' AS type " & _
                      "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                      "APPROVAL_GRP_FM A ON M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                      "LEFT OUTER JOIN USER_MSTR B ON A.AGFM_FM = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                      "LEFT OUTER JOIN USER_MSTR C ON A.AGFM_A_FM = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                      "where M.AGM_TYPE='INV' AND A.AGFM_GRP_INDEX ='" & valgroup & "' " & _
                      "AND M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                      "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                      "ORDER BY type DESC, SEQ"

            dsApp = objDb.FillDs(strget)
            BindFinanceAppGroupAsg = dsApp
        End Function
        Function BindInvAppGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
            Dim objDB As New EAD.DBCom
            'Michelle (23/9/2010) - To cater for MYSQL syntax
            'Modified for IPP GST Stage 2A - CH (30/1/2015)
            'strget = "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFO_SEQ AS SEQ, 'FO - ' + CAST(A.AGFO_SEQ AS VARCHAR) AS AGA_SEQ,A.AGFO_GRP_INDEX AS AGA_GRP_INDEX, " & _
            strget = "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFO_SEQ AS SEQ, " & objDB.Concat3(" - ", "", "'FO'", "CAST(A.AGFO_SEQ AS CHAR)") & " AS AGA_SEQ,A.AGFO_GRP_INDEX AS AGA_GRP_INDEX, " & _
                      "B.UM_USER_NAME as AO_NAME,B.UM_INVOICE_MASS_APP AS UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGFO_RELIEF_IND AS AGA_RELIEF_IND, " & _
                      "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                      "CASE WHEN AGFO_A_FO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, 'FO' AS type ,'' as AAO_ID2,'' as AAO_ID3,'' as AAO_ID4, '' as AGA_OFFICER_TYPE, '' as AGA_BRANCH_CODE, '' as AGA_CC_CODE " & _
                      "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                      "APPROVAL_GRP_FO A ON M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                      "LEFT OUTER JOIN USER_MSTR B ON A.AGFO_FO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                      "LEFT OUTER JOIN USER_MSTR C ON A.AGFO_A_FO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                      "where M.AGM_TYPE='INV' AND A.AGFO_GRP_INDEX ='" & valgroup & "' " & _
                      "AND M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                      "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "

            strget = strget & " UNION "

            'Michelle (23/9/2010) - To cater for MYSQL syntax
            'strget = strget & "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFM_SEQ AS SEQ, 'FM - ' + CAST(A.AGFM_SEQ AS VARCHAR) AS AGA_SEQ,A.AGFM_GRP_INDEX AS AGA_GRP_INDEX, " & _
            strget = strget & "SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID, A.AGFM_SEQ AS SEQ, " & objDB.Concat3(" - ", "", "'FM'", "CAST(A.AGFM_SEQ AS CHAR)") & "  AS AGA_SEQ,A.AGFM_GRP_INDEX AS AGA_GRP_INDEX, " & _
                      "B.UM_USER_NAME as AO_NAME,B.UM_INVOICE_MASS_APP AS UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGFM_RELIEF_IND AS AGA_RELIEF_IND, " & _
                      "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                      "CASE WHEN AGFM_A_FM IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, 'FM' AS type ,'' as AAO_ID2,'' as AAO_ID3,'' as AAO_ID4, '' as AGA_OFFICER_TYPE, '' as AGA_BRANCH_CODE, '' as AGA_CC_CODE " & _
                      "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                      "APPROVAL_GRP_FM A ON M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                      "LEFT OUTER JOIN USER_MSTR B ON A.AGFM_FM = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                      "LEFT OUTER JOIN USER_MSTR C ON A.AGFM_A_FM = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                      "where M.AGM_TYPE='INV' AND A.AGFM_GRP_INDEX ='" & valgroup & "' " & _
                      "AND M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                      "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                      "ORDER BY type DESC, SEQ"

            dsApp = objDB.FillDs(strget)
            Return dsApp
        End Function

        Function BindIQCAppGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
			'Modified for IPP GST Stage 2A - CH (30/1/2015)
            strget = "SELECT B.UM_USER_ID AS AO_ID,C.UM_USER_ID AS AAO_ID,A.AGI_SEQ AS AGA_SEQ,A.AGI_GRP_INDEX AS AGA_GRP_INDEX, " & _
                    "B.UM_USER_NAME AS AO_NAME, B.UM_MASS_APP, C.UM_USER_NAME AS AAO_NAME, AGI_RELIEF_IND AS AGA_RELIEF_IND, " & _
                    "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                    "CASE WHEN AGI_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END " & _
                    "ELSE 1 END AS AAO_ACTIVE,'' AS AAO_ID2,'' AS AAO_ID3,'' AS AAO_ID4, A.AGI_OFFICER_TYPE AS AGA_OFFICER_TYPE, '' AS AGA_BRANCH_CODE, '' AS AGA_CC_CODE " & _
                    "FROM APPROVAL_GRP_MSTR M INNER JOIN APPROVAL_GRP_IQC A ON M.AGM_GRP_INDEX = A.AGI_GRP_INDEX " & _
                    "LEFT OUTER JOIN USER_MSTR B ON A.AGI_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR C ON A.AGI_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                    "WHERE M.AGM_TYPE='IQC' AND A.AGI_GRP_INDEX ='" & valgroup & "' AND M.AGM_GRP_INDEX = A.AGI_GRP_INDEX " & _
                    "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' ORDER BY A.AGI_SEQ "
			'-------------------------------------------

            dsApp = objDb.FillDs(strget)
            BindIQCAppGroupAsg = dsApp
        End Function

        'Zulham 03082018 - PAMB
        Function GetGroup(ByVal valgroup As String, Optional ByVal strType As String = "")
            Dim strget As String
            Dim dsApp As New DataSet
            strget = "(SELECT AGA_SEQ, AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX,B.UM_USER_NAME as AO_NAME,C.UM_USER_NAME as AAO_NAME, " &
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " &
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " &
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " &
                     "APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " &
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " &
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " &
                     "where M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND A.AGA_GRP_INDEX ='" & valgroup & "' " &
                     "ORDER BY AGA_SEQ)"

            If strType = "PR" Or strType = "PO" Then
                strget &= " UNION ALL (Select AGPAO_SEQ 'AGA_SEQ', AGPAO_AO 'AGA_AO', AGPAO_A_AO 'AGA_A_AO',AGPAO_RELIEF_IND 'AGA_RELIEF_IND', AGPAO_GRP_INDEX 'AGA_GRP_INDEX',B.UM_USER_NAME AS AO_NAME,C.UM_USER_NAME AS AAO_NAME, " &
                "Case WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " &
                "Case WHEN AGPAO_A_AO Is Not NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " &
                "From APPROVAL_GRP_MSTR M INNER Join " &
                "APPROVAL_GRP_PAO A ON M.AGM_GRP_INDEX = A.AGPAO_GRP_INDEX  " &
                "Left OUTER JOIN USER_MSTR B ON A.AGPAO_AO = B.UM_USER_ID And M.AGM_COY_ID = B.UM_COY_ID  " &
                "Left OUTER JOIN USER_MSTR C ON A.AGPAO_A_AO = C.UM_USER_ID And M.AGM_COY_ID = C.UM_COY_ID  " &
                "WHERE M.AGM_TYPE <>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'  " &
                "And A.AGPAO_GRP_INDEX ='" & valgroup & "' " &
                "ORDER BY AGPAO_SEQ)"
            End If

            dsApp = objDb.FillDs(strget)
            GetGroup = dsApp
        End Function
        Function GetIPPFMGroup(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGFM_SEQ, AGFM_FM, AGFM_A_FM,AGFM_RELIEF_IND, AGFM_GRP_INDEX,B.UM_USER_NAME as FM_NAME,C.UM_USER_NAME as AFM_NAME, " & _
                    "D.UM_USER_NAME AS AFM_NAME2, E.UM_USER_NAME AS AFM_NAME3, F.UM_USER_NAME AS AFM_NAME4, " & _
                    "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS FM_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFM_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM_2 IS NOT NULL THEN CASE WHEN D.UM_STATUS = 'A' AND D.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFM2_ACTIVE, " & _
                    "CASE WHEN AGFM_A_FM_3 IS NOT NULL THEN CASE WHEN E.UM_STATUS = 'A' AND E.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFM3_ACTIVE, " & _
                    "CASE WHEN AGFM_A_FM_4 IS NOT NULL THEN CASE WHEN F.UM_STATUS = 'A' AND F.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFM4_ACTIVE " & _
                    "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_FM A ON M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGFM_FM = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGFM_A_FM = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR D ON A.AGFM_A_FM_2 = D.UM_USER_ID AND M.AGM_COY_ID = D.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR E ON A.AGFM_A_FM_3 = E.UM_USER_ID AND M.AGM_COY_ID = E.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR F ON A.AGFM_A_FM_4 = F.UM_USER_ID AND M.AGM_COY_ID = F.UM_COY_ID " & _
                     "where M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGFM_GRP_INDEX ='" & valgroup & "' " & _
                     "ORDER BY AGFM_SEQ"
            dsApp = objDb.FillDs(strget)
            GetIPPFMGroup = dsApp
        End Function
        Function GetIPPFOGroup(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGFO_SEQ, AGFO_FO, AGFO_A_FO,AGFO_RELIEF_IND, AGFO_GRP_INDEX,B.UM_USER_NAME as FO_NAME,C.UM_USER_NAME as AFO_NAME, " & _
                    "D.UM_USER_NAME AS AFO_NAME2, E.UM_USER_NAME AS AFO_NAME3, F.UM_USER_NAME AS AFO_NAME4, " & _
                    "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS FO_ACTIVE, " & _
                     "CASE WHEN AGFO_A_FO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFO_ACTIVE, " & _
                    "CASE WHEN AGFO_A_FO_2 IS NOT NULL THEN CASE WHEN D.UM_STATUS = 'A' AND D.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFO2_ACTIVE, " & _
                    "CASE WHEN AGFO_A_FO_3 IS NOT NULL THEN CASE WHEN E.UM_STATUS = 'A' AND E.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFO3_ACTIVE, " & _
                    "CASE WHEN AGFO_A_FO_4 IS NOT NULL THEN CASE WHEN F.UM_STATUS = 'A' AND F.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AFO4_ACTIVE " & _
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_FO A ON M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGFO_FO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGFO_A_FO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR D ON A.AGFO_A_FO_2 = D.UM_USER_ID AND M.AGM_COY_ID = D.UM_COY_ID " & _
                    "LEFT OUTER JOIN USER_MSTR E ON A.AGFO_A_FO_3 = E.UM_USER_ID AND M.AGM_COY_ID = E.UM_COY_ID  " & _
                    "LEFT OUTER JOIN USER_MSTR F ON A.AGFO_A_FO_4 = F.UM_USER_ID AND M.AGM_COY_ID = F.UM_COY_ID  " & _
                    "where M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGFO_GRP_INDEX ='" & valgroup & "' " & _
                     "ORDER BY AGFO_SEQ"
            dsApp = objDb.FillDs(strget)
            GetIPPFOGroup = dsApp
        End Function

        Function GetFinanceApprGroup(ByVal valgroup As String, Optional ByVal apprtype As String = Nothing)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGA_SEQ, AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX,B.UM_USER_NAME as AO_NAME,C.UM_USER_NAME as AAO_NAME, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " & _
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_FINANCE A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "where M.AGM_TYPE = 'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGA_GRP_INDEX ='" & valgroup & "' "

            If apprtype <> Nothing Then
                strget &= " AND A.AGA_TYPE = '" & apprtype & "' "
            End If

            strget &= "ORDER BY AGA_TYPE DESC, AGA_SEQ"

            dsApp = objDb.FillDs(strget)

            GetFinanceApprGroup = dsApp
        End Function

        Function GetIQCGroup(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGI_SEQ AS AGA_SEQ, AGI_AO, AGI_A_AO,AGI_RELIEF_IND, AGI_GRP_INDEX,B.UM_USER_NAME AS AO_NAME,C.UM_USER_NAME AS AAO_NAME, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGI_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " & _
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_IQC A ON M.AGM_GRP_INDEX = A.AGI_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGI_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGI_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE='IQC' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGI_GRP_INDEX ='" & valgroup & "' " & _
                     "ORDER BY AGI_SEQ "

            dsApp = objDb.FillDs(strget)
            GetIQCGroup = dsApp
        End Function
        Function GetMRSGroup(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGA_SEQ, AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX,B.UM_USER_NAME AS AO_NAME,C.UM_USER_NAME AS AAO_NAME, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " & _
                     "FROM APPROVAL_GRP_MSTR M " & _
                     "INNER JOIN APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE = 'MRS' AND M.AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGA_GRP_INDEX = '" & valgroup & "' " & _
                     "ORDER BY AGA_SEQ "

            dsApp = objDb.FillDs(strget)
            GetMRSGroup = dsApp
        End Function

		'Modified for IPP GST Stage 2A - CH (30/1/2015)
        Function GetBilGroup(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet

            strget = "SELECT AGA_SEQ, AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX,B.UM_USER_NAME AS AO_NAME,C.UM_USER_NAME AS AAO_NAME, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE " & _
                     "FROM APPROVAL_GRP_MSTR M " & _
                     "INNER JOIN APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE = 'BIL' AND M.AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGA_GRP_INDEX = '" & valgroup & "' " & _
                     "ORDER BY AGA_SEQ "

            dsApp = objDb.FillDs(strget)
            GetBilGroup = dsApp
        End Function
		'--------------------------------------
        Function bind_dtgApp_seqAOAAO(ByVal valgroup As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT AGA_SEQ, AGA_AO, AGA_A_AO,AGA_RELIEF_IND, AGA_GRP_INDEX " & _
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " & _
                     "APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "where M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND A.AGA_GRP_INDEX ='" & valgroup & "'"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function AddAppWorkFlow(ByVal strValGroup As String, ByVal strconsol As String, ByVal strType As String, Optional ByVal deptcode As String = "", Optional ByVal IQCType As String = "", Optional ByVal txtUrgentIR As String = "", Optional ByVal txtRejectMRS As String = "", Optional ByVal isResident As String = "")
            Dim strCoyId, strUserID, strSQL, strSqlAry(0) As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            strSQL = "SELECT '*' FROM APPROVAL_GRP_MSTR WHERE AGM_COY_ID='" & strCoyId &
            "' AND AGM_GRP_NAME ='" & Common.Parse(strValGroup) & "' AND AGM_TYPE='" & strType & "' "

            If objDb.Exist(strSQL) > 0 Then
                AddAppWorkFlow = WheelMsgNum.Duplicate
            Else
                If strType = "IQC" Then
                    IQCType = "'" & IQCType & "'"
                Else
                    IQCType = "NULL"
                End If
                'Zulham 26062018 - PAMB
                'Added new column: AGM_RESIDENT
                strSQL = "INSERT INTO APPROVAL_GRP_MSTR(AGM_GRP_NAME, AGM_COY_ID, AGM_CONSOLIDATOR, AGM_ENT_BY, AGM_ENT_DATETIME,AGM_TYPE,AGM_DEPT_CODE,AGM_IQC_TYPE,AGM_MRS_EMAIL1,AGM_MRS_EMAIL2,AGM_RESIDENT) VALUES('" & Common.Parse(strValGroup) & "','" & Common.Parse(strCoyId) & "','" & strconsol & "','" & strUserID & "'," & Common.ConvertDate(Now) & ",'" & strType & "','" & deptcode & "'," & IQCType & ",'" & txtUrgentIR & "','" & txtRejectMRS & "','" & isResident & "')"
                Common.Insert2Ary(strSqlAry, strSQL)
                If strSqlAry(0) <> String.Empty Then
                    objDb.BatchExecute(strSqlAry)
                    AddAppWorkFlow = WheelMsgNum.Save
                End If
            End If
        End Function

        Function DelAppBuyer(ByVal strvalofficerlist As String)
            Dim strSQL As String
            Dim query(0) As String
            strSQL = "Delete APPROVAL_GRP_BUYER where AGB_GRP_INDEX='" & strvalofficerlist & "'"
            Common.Insert2Ary(query, strSQL)
            objDb.BatchExecute(query)
        End Function

        Function AddAppBuyer(ByVal strvalofficerlist As String, ByVal strli() As String, ByVal strName() As String, ByRef strTemp As String)
            Dim strSQL As String
            Dim query(0) As String
            Dim intLoop As Integer
            Dim objDB As New EAD.DBCom
            Dim strcond As String

            'Michelle (23/9/2010) - To add in the 'From' for MYSQL
            'strSQL = "Delete APPROVAL_GRP_BUYER where AGB_GRP_INDEX='" & strvalofficerlist & "'"
            strSQL = "Delete From APPROVAL_GRP_BUYER where AGB_GRP_INDEX='" & strvalofficerlist & "'"
            Common.Insert2Ary(query, strSQL)

            For intLoop = 0 To strli.Length - 1
                'Michelle (23/9/2010) - To change syntax for MYSQL
                'strSQL = "SELECT TOP 1 * FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX=" & strvalofficerlist & _
                ' " AND (AGA_AO='" & strli(intLoop) & "' OR AGA_A_AO ='" & strli(intLoop) & "')"
                'If objDb.Exist(strSQL) Then
                strcond = " WHERE AGA_GRP_INDEX=" & strvalofficerlist & _
                      " AND (AGA_AO='" & strli(intLoop) & "' OR AGA_A_AO ='" & strli(intLoop) & "')"
                strSQL = objDB.Get1Column("APPROVAL_GRP_AO", "*", strcond)
                If strSQL <> "" Then
                    If strTemp <> "" Then

                        strTemp = strTemp & "$"
                    End If
                    strTemp = strTemp & strName(intLoop)
                Else
                    strSQL = "INSERT INTO APPROVAL_GRP_BUYER(AGB_GRP_INDEX,AGB_BUYER )VALUES('" & strvalofficerlist & "','" & strli(intLoop) & "')"
                    Common.Insert2Ary(query, strSQL)
                End If
            Next

            If objDb.BatchExecute(query) Then
                AddAppBuyer = WheelMsgNum.Save
            Else
                AddAppBuyer = WheelMsgNum.NotSave
            End If

        End Function

        Function AttachedToDept(ByVal strIndex As String)
            Dim strQuery As String = "SELECT 1 FROM COMPANY_DEPT_MSTR WHERE " & _
                " CDM_APPROVAL_GRP_INDEX = " & strIndex

            '" CDM_APPROVAL_GRP_INDEX = " & strIndex & " OR CDM_IPP_APPROVAL_GRP_INDEX = " & strIndex

            Return IIf(objDb.Exist(strQuery) > 0, True, False)
        End Function

        Function DelAppWorkFlow(ByVal strIndex As String)
            Dim strdelFav As String
            Dim strAryQuery(0) As String

            strdelFav = "Delete from APPROVAL_GRP_MSTR where AGM_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_AO where AGA_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_BUYER where AGB_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_FM where AGFM_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_FO where AGFO_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_IQC where AGI_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            strdelFav = "Delete from APPROVAL_GRP_PRODUCT where AGP_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            'Jules 2018.11.15
            strdelFav = "Delete from APPROVAL_GRP_PAO where AGPAO_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            'Chee Hong - 08/04/2015 - IPP GST Stage 2A Enhancement: No need include it
            'strdelFav = "Delete from APPROVAL_GRP_BILLING WHERE AGB_GRP_INDEX = '" & strIndex & "' "
            'Common.Insert2Ary(strAryQuery, strdelFav)
            '-----------------------------------------------------

            'objDb.Execute(strdelFav)
            objDb.BatchExecute(strAryQuery)
            If objDb.Execute(strdelFav) Then
                DelAppWorkFlow = WheelMsgNum.Delete
            Else
                DelAppWorkFlow = WheelMsgNum.NotDelete
            End If
        End Function

        Function DelAppOfficer(ByVal strIndex As String)
            Dim strdelApp As String
            strdelApp = "Delete from APPROVAL_GRP_AO where AGA_GRP_INDEX = '" & strIndex & "'"

            objDb.Execute(strdelApp)
            If objDb.Execute(strdelApp) Then
                DelAppOfficer = WheelMsgNum.Delete
            Else
                DelAppOfficer = WheelMsgNum.NotDelete
            End If
        End Function

        Function DelAppGrpAsg(ByVal valgroup As String, ByVal strlevel As String)
            Dim strdelApp As String

            strdelApp = "Delete from APPROVAL_GRP_AO where AGA_GRP_INDEX = '" & valgroup & "' AND AGA_SEQ='" & strlevel & "'"

            objDb.Execute(strdelApp)
            If objDb.Execute(strdelApp) Then
                DelAppGrpAsg = WheelMsgNum.Delete
            Else
                DelAppGrpAsg = WheelMsgNum.NotDelete
            End If
        End Function

        Function DelFOAppGrpAsg(ByVal valgroup As String, ByVal strlevel As String)
            Dim strdelApp As String

            strdelApp = "Delete from APPROVAL_GRP_FO where AGFO_GRP_INDEX = '" & valgroup & "' AND AGFO_SEQ='" & strlevel & "'"

            objDb.Execute(strdelApp)
            If objDb.Execute(strdelApp) Then
                DelFOAppGrpAsg = WheelMsgNum.Delete
            Else
                DelFOAppGrpAsg = WheelMsgNum.NotDelete
            End If
        End Function

        Function DelFMAppGrpAsg(ByVal valgroup As String, ByVal strlevel As String)
            Dim strdelApp As String

            strdelApp = "Delete from APPROVAL_GRP_FM where AGFM_GRP_INDEX = '" & valgroup & "' AND AGFM_SEQ='" & strlevel & "'"

            objDb.Execute(strdelApp)
            If objDb.Execute(strdelApp) Then
                DelFMAppGrpAsg = WheelMsgNum.Delete
            Else
                DelFMAppGrpAsg = WheelMsgNum.NotDelete
            End If
        End Function

        Function DelIQCAppGrpAsg(ByVal valgroup As String, ByVal strlevel As String, ByVal strIQCType As String) As Integer
            Dim strdelApp, strTemp, strOffType As String
            Dim strChkIQCV, strChkIQCPA, strChkIQCA As String
            Dim strAryQuery(0) As String
            Dim i, j As Integer
            Dim ds As DataSet

            strChkIQCV = "N"
            strChkIQCPA = "N"
            strChkIQCA = "N"

            strdelApp = "SELECT AGI_OFFICER_TYPE, AGI_SEQ FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & valgroup & "' AND NOT AGI_SEQ IN (" & strlevel & ")"
            ds = objDb.FillDs(strdelApp)

            If strIQCType = "TAS" Then  
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(i)(0) = "IQCV" Then
                        strChkIQCV = "Y"
                    ElseIf ds.Tables(0).Rows(i)(0) = "IQCPA" Then
                        strChkIQCPA = "Y"
                    ElseIf ds.Tables(0).Rows(i)(0) = "IQCA" Then
                        strChkIQCA = "Y"
                    End If
                Next

                If (strChkIQCV = "N" And strChkIQCPA = "N" And strChkIQCA = "N") Or (strChkIQCV = "Y" And strChkIQCPA = "N" And strChkIQCA = "N") Or (strChkIQCV = "Y" And strChkIQCPA = "Y" And strChkIQCA = "N") Or (strChkIQCV = "Y" And strChkIQCPA = "Y" And strChkIQCA = "Y") Then
                    strdelApp = "DELETE FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & valgroup & "' AND AGI_SEQ IN (" & strlevel & ")"
                    Common.Insert2Ary(strAryQuery, strdelApp)
                Else
                    If strChkIQCV = "N" Then
                        Return -1
                    ElseIf strChkIQCPA = "N" Then
                        Return -2
                    End If
                End If
            Else
                strdelApp = "DELETE FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & valgroup & "' AND AGI_SEQ IN (" & strlevel & ")"
                Common.Insert2Ary(strAryQuery, strdelApp)
            End If

            j = 1
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strdelApp = "UPDATE APPROVAL_GRP_IQC SET AGI_SEQ = " & j & " WHERE AGI_GRP_INDEX = '" & valgroup & "' " & _
                            "AND AGI_SEQ = " & ds.Tables(0).Rows(i)("AGI_SEQ")
                Common.Insert2Ary(strAryQuery, strdelApp)
                j = j + 1
            Next

            If objDb.BatchExecute(strAryQuery) Then
                DelIQCAppGrpAsg = WheelMsgNum.Delete
            Else
                DelIQCAppGrpAsg = WheelMsgNum.NotDelete
            End If
        End Function

        Public Function getcboApp(Optional ByVal strWhere As String = Nothing) As DataView

            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "Select AGM_GRP_NAME, AGM_GRP_INDEX " & _
                     "FROM APPROVAL_GRP_MSTR " & _
                     "where AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"

            If strWhere <> "" Then
                strSql = strSql & " AND " & strWhere
            End If

            strSql = strSql & " ORDER BY AGM_GRP_NAME"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboAppconsol() As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            strSql = "Select AGM_GRP_NAME, AGM_GRP_INDEX " & _
                     "FROM APPROVAL_GRP_MSTR " & _
                     "where AGM_TYPE<>'INV' AND AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND AGM_CONSOLIDATOR <> ''"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function bindcboconsol() As DataView

            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select R.UU_USRGRP_ID,M.UM_USER_ID,R.UU_USER_ID,M.UM_COY_ID, " & _
                     objDB.Concat(" : ", "", "M.UM_USER_ID", "M.UM_USER_NAME") & " as two " & _
                     "FROM USER_MSTR M, USERS_USRGRP R, USER_GROUP_MSTR GM " & _
                     "where M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                     "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND R.UU_USER_ID = M.UM_USER_ID " & _
                     "AND R.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Consolidator'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboAO(Optional ByVal type As String = "") As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                                          objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                                          "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                                          "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                          "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                                          "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                          "AND UU_USER_ID = M.UM_USER_ID " & _
                                          "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID "
            If type = "IPP" Or type = "E2P" Then
                strSql &= "AND GM.UGM_FIXED_ROLE = 'IPP Approving Officer'"
            ElseIf type = "BIL" Then 'Modified for IPP GST Stage 2A - CH (30/1/2015)
                strSql &= "AND (GM.UGM_FIXED_ROLE = 'Billing Approving Officer' OR GM.UGM_FIXED_ROLE = 'Billing Approving Officer(F)')"
            Else
                strSql &= "AND GM.UGM_FIXED_ROLE = 'Approving Officer'"
            End If

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboPO() As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                                 objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                                 "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                                 "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                                 "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND UU_USER_ID = M.UM_USER_ID " & _
                                 "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Purchasing Officer'"


            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboFO() As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                                 objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                                 "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                                 "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                                 "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND UU_USER_ID = M.UM_USER_ID " & _
                                 "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Finance Officer'"


            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboFM() As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                                 objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                                 "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                                 "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                                 "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND UU_USER_ID = M.UM_USER_ID " & _
                                 "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Finance Manager'"


            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Public Function getcboQCO() As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                                 objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                                 "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                                 "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                                 "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND UU_USER_ID = M.UM_USER_ID " & _
                                 "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Quality Control Officer'"


            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function modAppWorkFlow(ByVal strlistindex As String, ByVal strValGroup As String, ByVal strconsol As String, ByVal strType As String, Optional ByVal strOld As String = "", Optional ByVal deptcode As String = "", Optional ByVal IQCType As String = "", Optional ByVal txtUrgentIR As String = "", Optional ByVal txtRejectMRS As String = "", Optional ByVal isResident As String = "")
            Dim strsql, strUserID As String

            strUserID = HttpContext.Current.Session("UserId")

            If UCase(strOld) <> UCase(strValGroup) Then

                'Jules 2018.11.02 - Added Group Type.
                If objDb.Exist("Select '*' From APPROVAL_GRP_MSTR where AGM_COY_ID = '" &
                HttpContext.Current.Current.Session("CompanyID") & "' AND AGM_GRP_NAME ='" &
                Common.Parse(strValGroup) & "' AND AGM_TYPE ='" & strType & "'") > 0 Then
                    modAppWorkFlow = WheelMsgNum.Duplicate
                    Exit Function
                End If
            End If

            If strType = "MRS" Then
                strsql = "UPDATE APPROVAL_GRP_MSTR SET " &
                        "AGM_GRP_NAME = '" & Common.Parse(strValGroup) & "', AGM_CONSOLIDATOR ='" & Common.Parse(strconsol) & "',AGM_TYPE='" & strType & "',AGM_MOD_BY='" & strUserID & "', AGM_MOD_DATETIME=" & Common.ConvertDate(Now) & ", AGM_DEPT_CODE='" & deptcode & "', AGM_MRS_EMAIL1='" & txtUrgentIR & "', AGM_MRS_EMAIL2='" & txtRejectMRS & "' " &
                        "WHERE AGM_GRP_INDEX ='" & Common.Parse(strlistindex) & "' "
            ElseIf strType = "IQC" Then
                strsql = "UPDATE APPROVAL_GRP_MSTR SET " &
                        "AGM_GRP_NAME = '" & Common.Parse(strValGroup) & "', AGM_CONSOLIDATOR ='" & Common.Parse(strconsol) & "',AGM_TYPE='" & strType & "',AGM_MOD_BY='" & strUserID & "', AGM_MOD_DATETIME=" & Common.ConvertDate(Now) & ", AGM_DEPT_CODE='" & deptcode & "', AGM_IQC_TYPE='" & IQCType & "' " &
                        "WHERE AGM_GRP_INDEX ='" & Common.Parse(strlistindex) & "' "
            Else
                'Zulham 26062018 - PAMB
                'Added agm_resident
                strsql = "UPDATE APPROVAL_GRP_MSTR SET " &
                   "AGM_GRP_NAME = '" & Common.Parse(strValGroup) & "', AGM_CONSOLIDATOR ='" & Common.Parse(strconsol) & "',AGM_TYPE='" & strType & "',AGM_MOD_BY='" & strUserID & "', AGM_MOD_DATETIME=" & Common.ConvertDate(Now) & ", AGM_DEPT_CODE='" & deptcode & "',  AGM_RESIDENT = '" & isResident & "' " &
                   "WHERE AGM_GRP_INDEX ='" & Common.Parse(strlistindex) & "' "
            End If

            If objDb.Execute(strsql) Then
                modAppWorkFlow = WheelMsgNum.Save
            Else
                modAppWorkFlow = WheelMsgNum.NotSave
            End If
        End Function

        Function delAo_AAO(ByVal index As String, ByVal lev As String)
            Dim strdelItem As String
            strdelItem = "Delete from APPROVAL_GRP_AO where AGA_GRP_INDEX = '" & index & "' " & _
                         "AND AGA_SEQ = '" & lev & "'"
            objDb.Execute(strdelItem)
            If objDb.Execute(strdelItem) Then
                delAo_AAO = WheelMsgNum.Delete
            Else
                delAo_AAO = WheelMsgNum.NotDelete
            End If
        End Function

        Function delAo_AFO(ByVal index As String, ByVal lev As String)
            Dim strdelItem As String
            strdelItem = "Delete from APPROVAL_GRP_FO where AGFO_GRP_INDEX = '" & index & "' " & _
                         "AND AGFO_SEQ = '" & lev & "'"
            objDb.Execute(strdelItem)
            If objDb.Execute(strdelItem) Then
                delAo_AFO = WheelMsgNum.Delete
            Else
                delAo_AFO = WheelMsgNum.NotDelete
            End If
        End Function

        Function delAo_AFM(ByVal index As String, ByVal lev As String)
            Dim strdelItem As String
            strdelItem = "Delete from APPROVAL_GRP_FM where AGFM_GRP_INDEX = '" & index & "' " & _
                         "AND AGFM_SEQ = '" & lev & "'"
            objDb.Execute(strdelItem)
            If objDb.Execute(strdelItem) Then
                delAo_AFM = WheelMsgNum.Delete
            Else
                delAo_AFM = WheelMsgNum.NotDelete
            End If
        End Function

        Function checkMassApp(ByVal struserid As String, Optional ByVal strRole As String = "ao")
            Dim strSQL As String

            If strRole = "ao" Then
                strSQL = "SELECT UM_MASS_APP FROM USERS_USRGRP,USER_GROUP_MSTR GM, USER_MSTR M " & _
                        "where UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                        "AND UM_MASS_APP='Y' AND M.UM_USER_ID = '" & struserid & "' " & _
                        "AND UU_USER_ID = M.UM_USER_ID " & _
                        "AND M.UM_COY_ID = UU_COY_ID " & _
                        "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID " & _
                        "AND GM.UGM_FIXED_ROLE = 'Approving Officer'"

            ElseIf strRole = "sk" Then
                strSQL = "SELECT UM_MRS_MASS_APP FROM USERS_USRGRP,USER_GROUP_MSTR GM, USER_MSTR M " & _
                        "WHERE UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                        "AND UM_MRS_MASS_APP='Y' AND M.UM_USER_ID = '" & struserid & "' " & _
                        "AND UU_USER_ID = M.UM_USER_ID " & _
                        "AND M.UM_COY_ID = UU_COY_ID " & _
                        "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID " & _
                        "AND GM.UGM_FIXED_ROLE = 'Store Keeper'"

            End If

            If objDb.Exist(strSQL) > 0 Then
                checkMassApp = 1
            Else
                checkMassApp = 0
            End If
        End Function

        Function checkFOMassApp(ByVal struserid As String)
            Dim strSQL As String

            strSQL = "SELECT UM_INVOICE_MASS_APP FROM USERS_USRGRP,USER_GROUP_MSTR GM, USER_MSTR M " & _
                     "where UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND UM_INVOICE_MASS_APP='Y' AND M.UM_USER_ID = '" & struserid & "' " & _
                     "AND UU_USER_ID = M.UM_USER_ID " & _
                     "AND M.UM_COY_ID = UU_COY_ID " & _
                     "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID " & _
                     "AND GM.UGM_FIXED_ROLE = 'Finance Officer'"

            If objDb.Exist(strSQL) > 0 Then
                checkFOMassApp = 1
            Else
                checkFOMassApp = 0
            End If
        End Function



        Function checkconsolidatorApp(ByVal strGRPindex As String)
            Dim strSQL As String
            strSQL = "Select AGM_CONSOLIDATOR, AGM_GRP_INDEX " & _
                                 "FROM  APPROVAL_GRP_MSTR " & _
                                 "WHERE AGM_CONSOLIDATOR <> '' " & _
                                 "AND AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                                 "AND AGM_GRP_INDEX = '" & strGRPindex & "' " & _
                                 "AND AGM_TYPE<>'INV' "


            If objDb.Exist(strSQL) > 0 Then
                checkconsolidatorApp = 1
            Else
                checkconsolidatorApp = 0
            End If
        End Function

        Function checkconsolidatorParam()
            Dim strSQL As String
            strSQL = "SELECT CS_COY_ID,CS_FLAG_NAME, CS_FLAG_VALUE " & _
                     "FROM COMPANY_SETTING " & _
                     "WHERE CS_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "AND CS_FLAG_NAME ='Consolidation Required' " & _
                     "AND CS_FLAG_VALUE = '1'"

            If objDb.Exist(strSQL) > 0 Then
                checkconsolidatorParam = 1
            Else
                checkconsolidatorParam = 0
            End If
        End Function

        Function checkapprovaltype(ByVal strGRPindex As String) As ApprovalType
            Dim strSQL As String
            strSQL = "SELECT AGM_TYPE FROM APPROVAL_GRP_MSTR " & _
                "WHERE AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                "AND AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                "AND AGM_GRP_INDEX = '" & strGRPindex & "'"

            Dim strType As String = objDb.GetVal(strSQL)

            If strType = "INV" Then
                Return ApprovalType.Invoice
            Else
                Return ApprovalType.PR
            End If
        End Function

        Function save_Ao_AAO(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "", Optional ByVal strBC As String = "", Optional ByVal strCC As String = "")
            Dim strSQL, strcond, strSqlChkBuyer, strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String
            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX='" & index & "' " & _
                               "AND AGA_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX='" & index & "' " & _
                                 "AND AGA_AO = '" & AO & "'"

                strcond = " WHERE AGB_GRP_INDEX='" & index & "' AND AGB_BUYER='" & AO & "'"
                strSQL = objDb.Get1Column("APPROVAL_GRP_BUYER", "*", strcond)
                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_Ao_AAO = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_Ao_AAO = WheelMsgNum.Duplicate
                ElseIf strSQL <> "" Then
                    save_Ao_AAO = 0
                Else
                    '//WHEN USER NO SELECT ON AAO 
                    If AAO = "" Then
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_AO(AGA_GRP_INDEX, AGA_SEQ, AGA_AO, AGA_A_AO, AGA_RELIEF_IND"
						'Modified for IPP GST Stage 2A - CH (30/1/2015)
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", AGA_BRANCH_CODE, AGA_CC_CODE"
                        End If
                        strAddAo_AAO &= ") VALUES('" & index & "','" & lev & "','" & AO & "',NULL,'" & relief & "'"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", '" & Common.Parse(strBC) & "', '" & Common.Parse(strCC) & "'"
                        End If
                        strAddAo_AAO &= ")"
						'-----------------------------------
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_Ao_AAO = WheelMsgNum.Save

                        End If
                    Else
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_AO(AGA_GRP_INDEX, AGA_SEQ, AGA_AO, AGA_A_AO, AGA_RELIEF_IND"
						'Modified for IPP GST Stage 2A - CH (30/1/2015)
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", AGA_BRANCH_CODE, AGA_CC_CODE"
                        End If
                        strAddAo_AAO &= ") VALUES('" & index & "','" & lev & "','" & AO & "','" & AAO & "','" & relief & "'"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", '" & Common.Parse(strBC) & "', '" & Common.Parse(strCC) & "'"
                        End If
                        strAddAo_AAO &= ")"
						'------------------------------------
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_Ao_AAO = WheelMsgNum.Save
                        End If
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX='" & index & "' " & _
                                     "AND AGA_AO = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_Ao_AAO = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If

                If AAO = "" Then
					'Modified for IPP GST Stage 2A - CH (30/1/2015)
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_AO set AGA_SEQ = '" & lev & "', AGA_AO='" & AO & "', AGA_A_AO = NULL, AGA_RELIEF_IND = '" & relief & "' "
                    If strBC <> "" And strCC <> "" Then
                        strUpdateAo_AAO &= ", AGA_BRANCH_CODE = '" & Common.Parse(strBC) & "', AGA_CC_CODE = '" & Common.Parse(strCC) & "' "
                    End If
                    strUpdateAo_AAO &= "WHERE AGA_SEQ = '" & lev & "' AND AGA_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_Ao_AAO = WheelMsgNum.Save
                    End If
                Else
					'Modified for IPP GST Stage 2A - CH (30/1/2015)
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_AO set AGA_SEQ = '" & lev & "', AGA_AO='" & AO & "', AGA_A_AO='" & AAO & "', AGA_RELIEF_IND = '" & relief & "' "
                    If strBC <> "" And strCC <> "" Then
                        strUpdateAo_AAO &= ", AGA_BRANCH_CODE = '" & Common.Parse(strBC) & "', AGA_CC_CODE = '" & Common.Parse(strCC) & "' "
                    End If
                    strUpdateAo_AAO &= "WHERE AGA_SEQ = '" & lev & "' AND AGA_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_Ao_AAO = WheelMsgNum.Save
                    End If
                End If
            End If
        End Function

        Function save_FO_AFO(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "")
            Dim strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String
            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                               "AND AGFO_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                                 "AND AGFO_FO = '" & AO & "'"

                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_FO_AFO = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_FO_AFO = WheelMsgNum.Duplicate
                Else
                    '//WHEN USER NO SELECT ON AAO 
                    If AAO = "" Then
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FO(AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, AGFO_A_FO, AGFO_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "',NULL,'" & relief & "')"
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_FO_AFO = WheelMsgNum.Save

                        End If
                    Else
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FO(AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, AGFO_A_FO, AGFO_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "','" & AAO & "','" & relief & "')"
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_FO_AFO = WheelMsgNum.Save
                        End If
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                                     "AND AGFO_FO = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_FO_AFO = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If

                If AAO = "" Then
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FO set AGFO_SEQ = '" & lev & "', AGFO_FO='" & AO & "', AGFO_A_FO = NULL, AGFO_RELIEF_IND = '" & relief & "' " & _
                                     "WHERE AGFO_SEQ = '" & lev & "' AND AGFO_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FO_AFO = WheelMsgNum.Save
                    End If
                Else
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FO set AGFO_SEQ = '" & lev & "', AGFO_FO='" & AO & "', AGFO_A_FO='" & AAO & "', AGFO_RELIEF_IND = '" & relief & "' " & _
                                      "WHERE AGFO_SEQ = '" & lev & "' AND AGFO_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FO_AFO = WheelMsgNum.Save
                    End If
                End If
            End If
        End Function

        Function save_FM_AFM(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "")
            Dim strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String
            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                               "AND AGFM_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                                 "AND AGFM_FM = '" & AO & "'"

                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_FM_AFM = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_FM_AFM = WheelMsgNum.Duplicate
                Else
                    '//WHEN USER NO SELECT ON AAO 
                    If AAO = "" Then
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FM(AGFM_GRP_INDEX, AGFM_SEQ, AGFM_FM, AGFM_A_FM, AGFM_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "',NULL,'" & relief & "')"
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_FM_AFM = WheelMsgNum.Save

                        End If
                    Else
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FM(AGFM_GRP_INDEX, AGFM_SEQ, AGFM_FM, AGFM_A_FM, AGFM_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "','" & AAO & "','" & relief & "')"
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_FM_AFM = WheelMsgNum.Save
                        End If
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                                     "AND AGFM_FM = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_FM_AFM = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If

                If AAO = "" Then
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FM set AGFM_SEQ = '" & lev & "', AGFM_FM='" & AO & "', AGFM_A_FM = NULL, AGFM_RELIEF_IND = '" & relief & "' " & _
                                     "WHERE AGFM_SEQ = '" & lev & "' AND AGFM_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FM_AFM = WheelMsgNum.Save
                    End If
                Else
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FM set AGFM_SEQ = '" & lev & "', AGFM_FM='" & AO & "', AGFM_A_FM='" & AAO & "', AGFM_RELIEF_IND = '" & relief & "' " & _
                                      "WHERE AGFM_SEQ = '" & lev & "' AND AGFM_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FM_AFM = WheelMsgNum.Save
                    End If
                End If
            End If
        End Function

        Function save_Ao_AAO_IQC(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal relief As String, ByVal officertype As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "")
            Dim strSQL, strcond, strSqlChkBuyer, strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String
            Dim intLastLvl, intLvl As Integer
            Dim blnAdd As Boolean = True
            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & index & "' " & _
                               "AND AGI_SEQ = '" & lev & "'"

                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & index & "' " & _
                                 "AND AGI_AO = '" & AO & "'"

                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_Ao_AAO_IQC = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_Ao_AAO_IQC = WheelMsgNum.Duplicate
                Else

                    If officertype = "IQCPA" Then
                        intLastLvl = objDb.GetVal("SELECT MAX(AGI_SEQ) FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & index & "' AND AGI_OFFICER_TYPE = 'IQCV'")
                        If CInt(lev) < intLastLvl Then
                            blnAdd = False
                            save_Ao_AAO_IQC = -1
                        End If

                    ElseIf officertype = "IQCA" Then
                        intLastLvl = objDb.GetVal("SELECT MAX(AGI_SEQ) FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & index & "' AND AGI_OFFICER_TYPE = 'IQCPA'")
                        If CInt(lev) < intLastLvl Then
                            blnAdd = False
                            save_Ao_AAO_IQC = -2
                        End If

                    End If

                    If blnAdd Then
                        '//WHEN USER NO SELECT ON AAO 
                        If AAO = "" Then
                            strAddAo_AAO = "INSERT INTO APPROVAL_GRP_IQC(AGI_GRP_INDEX, AGI_SEQ, AGI_AO, AGI_A_AO, AGI_RELIEF_IND, AGI_OFFICER_TYPE) values('" & index & "','" & lev & "','" & AO & "',NULL,'" & relief & "','" & officertype & "')"
                            Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                            If strSqlAry(0) <> String.Empty Then
                                objDb.BatchExecute(strSqlAry)
                                save_Ao_AAO_IQC = WheelMsgNum.Save

                            End If
                        Else
                            strAddAo_AAO = "INSERT INTO APPROVAL_GRP_IQC(AGI_GRP_INDEX, AGI_SEQ, AGI_AO, AGI_A_AO, AGI_RELIEF_IND, AGI_OFFICER_TYPE) values('" & index & "','" & lev & "','" & AO & "','" & AAO & "','" & relief & "','" & officertype & "')"
                            Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                            If strSqlAry(0) <> String.Empty Then
                                objDb.BatchExecute(strSqlAry)
                                save_Ao_AAO_IQC = WheelMsgNum.Save
                            End If
                        End If
                    End If
                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX = '" & index & "' " & _
                                     "AND AGI_AO = '" & AO & "'"

                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_Ao_AAO_IQC = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If

                If AAO = "" Then
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_IQC SET AGI_SEQ = '" & lev & "', AGI_AO='" & AO & "', AGI_A_AO = NULL, AGI_RELIEF_IND = '" & relief & "', AGI_OFFICER_TYPE = '" & officertype & "' " & _
                                     "WHERE AGI_SEQ = '" & lev & "' AND AGI_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_Ao_AAO_IQC = WheelMsgNum.Save
                    End If
                Else
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_IQC SET AGI_SEQ = '" & lev & "', AGI_AO='" & AO & "', AGI_A_AO='" & AAO & "', AGI_RELIEF_IND = '" & relief & "', AGI_OFFICER_TYPE = '" & officertype & "' " & _
                                      "WHERE AGI_SEQ = '" & lev & "' AND AGI_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_Ao_AAO_IQC = WheelMsgNum.Save
                    End If
                End If
            End If
        End Function

        Function bindlistbox_AppOfficerData(ByVal valGroup As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT  A.AGA_GRP_INDEX, A.AGA_SEQ, A.AGA_AO, A.AGA_A_AO,M.AGM_COY_ID, " & _
                     "B.UM_USER_NAME as AO_NAME,C.UM_USER_NAME as AAO_NAME, " & _
                      objDB.Concat2(" : ", " / ", "CAST(A.AGA_SEQ AS CHAR(4))", "B.UM_USER_NAME", "ISNULL(C.UM_USER_NAME,'-')") & " as three " & _
                     "FROM APPROVAL_GRP_AO A INNER JOIN " & _
                     "APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX = M.AGM_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE<>'INV' AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
                     "AND A.AGA_GRP_INDEX ='" & valGroup & "' " & _
                     "ORDER BY  A.AGA_SEQ"
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function SelectedBuyerByAppList(ByVal strvalofficerlist As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT distinct U.UM_USER_ID, U.UM_USER_NAME, " & _
                                objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three, " & _
                                "CASE WHEN U.UM_STATUS = 'A' AND U.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS USER_STATUS " & _
                                "FROM APPROVAL_GRP_BUYER K, USER_MSTR U " & _
                                "where K.AGB_GRP_INDEX = '" & strvalofficerlist & "' " & _
                                "AND K.AGB_BUYER = U.UM_USER_ID " & _
                                "AND U.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "
            drw = objDB.GetView(strSql)

            Return drw
        End Function

        Function AvailBuyerByAppList(ByVal strvalofficerlist As String, Optional ByVal type As String = "") As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            If type <> "" Then
                strSql = "SELECT distinct U.UM_USER_ID, U.UM_USER_NAME, " & _
                                                          objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " & _
                                                          "FROM USER_MSTR U,USERS_USRGRP G,USER_GROUP_MSTR GM " & _
                                                          "where U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER from APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K where M.AGM_TYPE<>'INV' AND M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " & _
                                                          "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID=G.UU_COY_ID AND U.UM_DELETED<>'Y' AND U.UM_STATUS='A' " & _
                                                          "AND G.UU_USRGRP_ID =GM.UGM_USRGRP_ID "
                                                          
                If type = "PO" Then
                    strSql &= "AND (GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR GM.UGM_FIXED_ROLE= 'Purchasing Officer') " & _
                              "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"
                ElseIf type = "MRS" Then
                    strSql &= "AND GM.UGM_FIXED_ROLE='Requester' " & _
                              "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"
                Else
                    strSql &= "AND GM.UGM_FIXED_ROLE='Buyer' " & _
                              "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"
                End If
            Else
                strSql = "SELECT distinct U.UM_USER_ID, U.UM_USER_NAME, " & _
                                                          objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " & _
                                                          "FROM USER_MSTR U,USERS_USRGRP G,USER_GROUP_MSTR GM " & _
                                                          "where U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER from APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K where M.AGM_TYPE<>'INV' AND M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " & _
                                                          "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID=G.UU_COY_ID AND U.UM_DELETED<>'Y' AND U.UM_STATUS='A' " & _
                                                          "AND G.UU_USRGRP_ID =GM.UGM_USRGRP_ID " & _
                                                          "AND (GM.UGM_FIXED_ROLE='Buyer' OR GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR GM.UGM_FIXED_ROLE= 'Purchasing Officer') " & _
                                                          "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"
            End If
            drw = objDB.GetView(strSql)
            Return drw
        End Function

        '//To check whether assigned AO and AAO already assigned as Buyer for the same list
        Function isAssignedBuyer(ByVal strAppListNo As String, ByVal strAO As String, ByVal strAAO As String, ByRef strWhose As String) As Boolean
            Dim strSql, strcond As String
            Dim objDB As New EAD.DBCom
            'Michelle (23/9/2010) - To change syntax for MYSQL
            'strSql = "SELECT TOP 1 * FROM APPROVAL_GRP_Buyer WHERE AGB_GRP_INDEX=" & strAppListNo & _
            '   " AND AGB_BUYER ='" & strAO & "'"
            'If objDB.Exist(strSql) Then
            '    strWhose = "ao"
            'End If
            strcond = " WHERE AGB_GRP_INDEX=" & strAppListNo & " AND AGB_BUYER ='" & strAO & "'"
            strSql = objDB.Get1Column("APPROVAL_GRP_Buyer", "*", strcond)
            If strSql <> "" Then
                strWhose = "ao"
            End If

            'strSql = "SELECT TOP 1 * FROM APPROVAL_GRP_Buyer WHERE AGB_GRP_INDEX=" & strAppListNo & _
            '   " AND AGB_BUYER ='" & strAAO & "'"
            'If objDB.Exist(strSql) Then
            '    If strWhose <> "" Then
            '        strWhose = strWhose & ","
            '    End If
            '    strWhose = strWhose & "aao"
            'End If
            strcond = " WHERE AGB_GRP_INDEX=" & strAppListNo & " AND AGB_BUYER ='" & strAAO & "'"
            strSql = objDB.Get1Column("APPROVAL_GRP_Buyer", "*", strcond)
            If strSql <> "" Then
                If strWhose <> "" Then
                    strWhose = strWhose & ","
                End If
                strWhose = strWhose & "aao"
            End If

            If strWhose <> "" Then
                isAssignedBuyer = True
            Else
                isAssignedBuyer = False
            End If
        End Function
        Function BindIPPGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
			'Modified for IPP GST Stage 2A - CH (30/1/2015)
            strget = "SELECT B.UM_USER_ID AS AO_ID,C.UM_USER_ID AS AAO_ID,'' AS AAO_ID2,'' AS AAO_ID3,'' AS AAO_ID4,'' AS AGA_OFFICER_TYPE, " & _
                     "A.AGA_SEQ,A.AGA_GRP_INDEX, B.UM_USER_NAME AS AO_NAME, " & _
                     "B.UM_INVOICE_MASS_APP AS UM_MASS_APP, C.UM_USER_NAME AS AAO_NAME, " & _
                     "'' AS AAO_NAME2, '' AS AAO_NAME3, '' AS AAO_NAME4, " & _
                     "AGA_RELIEF_IND, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, " & _
                     "0 AS AAO2_ACTIVE, 0 AS AAO3_ACTIVE, 0 AS AAO4_ACTIVE, " & _
                     "'AO' AS ROLE, '' AS AGA_BRANCH_CODE, '' AS AGA_CC_CODE " & _
                     "FROM APPROVAL_GRP_MSTR M " & _
                     "INNER JOIN APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE<>'INV' AND A.AGA_GRP_INDEX ='" & valgroup & "' AND M.AGM_GRP_INDEX = A.AGA_GRP_INDEX AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "UNION " & _
                     "SELECT B.UM_USER_ID AS AO_ID,C.UM_USER_ID AS AAO_ID,D.UM_USER_ID AS AAO_ID2,E.UM_USER_ID AS AAO_ID3,F.UM_USER_ID AS AAO_ID4,'' AS AGA_OFFICER_TYPE, " & _
                     "A.AGFO_SEQ,A.AGFO_GRP_INDEX, B.UM_USER_NAME AS AO_NAME, " & _
                     "B.UM_INVOICE_MASS_APP AS UM_MASS_APP, C.UM_USER_NAME AS AAO_NAME, " & _
                     "D.UM_USER_NAME AS AAO_NAME2, E.UM_USER_NAME AS AAO_NAME3, F.UM_USER_NAME AS AAO_NAME4, " & _
                     "AGFO_RELIEF_IND, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGFO_A_FO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, " & _
                     "CASE WHEN AGFO_A_FO_2 IS NOT NULL THEN CASE WHEN D.UM_STATUS = 'A' AND D.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO2_ACTIVE, " & _
                     "CASE WHEN AGFO_A_FO_3 IS NOT NULL THEN CASE WHEN E.UM_STATUS = 'A' AND E.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO3_ACTIVE, " & _
                     "CASE WHEN AGFO_A_FO_4 IS NOT NULL THEN CASE WHEN F.UM_STATUS = 'A' AND F.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO4_ACTIVE, " & _
                     "'FO' AS ROLE, '' AS AGA_BRANCH_CODE, '' AS AGA_CC_CODE " & _
                     "FROM APPROVAL_GRP_MSTR M " & _
                     "INNER JOIN APPROVAL_GRP_FO A ON M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGFO_FO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGFO_A_FO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR D ON A.AGFO_A_FO_2 = D.UM_USER_ID AND M.AGM_COY_ID = D.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR E ON A.AGFO_A_FO_3 = E.UM_USER_ID AND M.AGM_COY_ID = E.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR F ON A.AGFO_A_FO_4 = F.UM_USER_ID AND M.AGM_COY_ID = F.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE<>'INV' AND A.AGFO_GRP_INDEX ='" & valgroup & "' AND M.AGM_GRP_INDEX = A.AGFO_GRP_INDEX AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " & _
                     "UNION " & _
                     "SELECT B.UM_USER_ID AS AO_ID,C.UM_USER_ID AS AAO_ID,D.UM_USER_ID AS AAO_ID2,E.UM_USER_ID AS AAO_ID3,F.UM_USER_ID AS AAO_ID4,'' AS AGA_OFFICER_TYPE, " & _
                     "A.AGFM_SEQ,A.AGFM_GRP_INDEX, B.UM_USER_NAME AS AO_NAME, " & _
                     "B.UM_INVOICE_MASS_APP AS UM_MASS_APP, C.UM_USER_NAME AS AAO_NAME, " & _
                     "D.UM_USER_NAME AS AAO_NAME2, E.UM_USER_NAME AS AAO_NAME3, F.UM_USER_NAME AS AAO_NAME4, " & _
                     "AGFM_RELIEF_IND, " & _
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM_2 IS NOT NULL THEN CASE WHEN D.UM_STATUS = 'A' AND D.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO2_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM_3 IS NOT NULL THEN CASE WHEN E.UM_STATUS = 'A' AND E.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO3_ACTIVE, " & _
                     "CASE WHEN AGFM_A_FM_4 IS NOT NULL THEN CASE WHEN F.UM_STATUS = 'A' AND F.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO4_ACTIVE, " & _
                     "'FM' AS ROLE, '' AS AGA_BRANCH_CODE, '' AS AGA_CC_CODE " & _
                     "FROM APPROVAL_GRP_MSTR M " & _
                     "INNER JOIN APPROVAL_GRP_FM A ON M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX " & _
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGFM_FM = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGFM_A_FM = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR D ON A.AGFM_A_FM_2 = D.UM_USER_ID AND M.AGM_COY_ID = D.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR E ON A.AGFM_A_FM_3 = E.UM_USER_ID AND M.AGM_COY_ID = E.UM_COY_ID " & _
                     "LEFT OUTER JOIN USER_MSTR F ON A.AGFM_A_FM_4 = F.UM_USER_ID AND M.AGM_COY_ID = F.UM_COY_ID " & _
                     "WHERE M.AGM_TYPE<>'INV' AND A.AGFM_GRP_INDEX ='" & valgroup & "' AND M.AGM_GRP_INDEX = A.AGFM_GRP_INDEX AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
            dsApp = objDb.FillDs(strget)
            BindIPPGroupAsg = dsApp
        End Function
        Function AvailTellerByAppList(ByVal strvalofficerlist As String, Optional ByVal appType As String = "") As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            'Zulham 05072018 - PAMB
            'Added argument for PAMB
            'Additional query for E@P app type
            If appType = "" Then
                strSql = "SELECT DISTINCT U.UM_USER_ID, U.UM_USER_NAME, CONCAT(U.UM_USER_ID, ' : ',U.UM_USER_NAME) AS three  " &
                "FROM USER_MSTR U,USERS_USRGRP G,USER_GROUP_MSTR GM  " &
                "WHERE U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER FROM APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K WHERE M.AGM_TYPE='IPP' AND M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " &
                "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID=G.UU_COY_ID AND U.UM_DELETED<>'Y' AND U.UM_STATUS='A'  " &
                "AND G.UU_USRGRP_ID =GM.UGM_USRGRP_ID " &
                "AND GM.UGM_FIXED_ROLE IN ('IPP Officer(F)','IPP Officer') " &
                "AND G.UU_USRGRP_ID IN ('IPP Officer(F)','IPP Officer') " &
                "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "
            ElseIf appType = "E2P" Then
                strSql = "SELECT DISTINCT U.UM_USER_ID, U.UM_USER_NAME, CONCAT(U.UM_USER_ID, ' : ',U.UM_USER_NAME) AS three  " &
                "FROM USER_MSTR U,USERS_USRGRP G,USER_GROUP_MSTR GM  " &
                "WHERE U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER FROM APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K WHERE M.AGM_TYPE='IPP' AND M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " &
                "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID=G.UU_COY_ID AND U.UM_DELETED<>'Y' AND U.UM_STATUS='A'  " &
                "AND G.UU_USRGRP_ID =GM.UGM_USRGRP_ID " &
                "AND GM.UGM_FIXED_ROLE IN ('IPP Officer','IPP Officer(F)') " &
                "AND G.UU_USRGRP_ID IN ('E2P Teller(S)','E2P Teller(F)') " &
                "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "
            End If

            drw = objDB.GetView(strSql)
            Return drw
        End Function
        'Modified for IPP GST Stage 2A - CH (30/1/2015)
        Function AvailBillTellerByAppList(ByVal strvalofficerlist As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT DISTINCT U.UM_USER_ID, U.UM_USER_NAME, CONCAT(U.UM_USER_ID, ' : ',U.UM_USER_NAME) AS three  " & _
                    "FROM USER_MSTR U,USERS_USRGRP G,USER_GROUP_MSTR GM  " & _
                    "WHERE U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER FROM APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K WHERE M.AGM_TYPE='BIL' AND M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " & _
                    "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID=G.UU_COY_ID AND U.UM_DELETED<>'Y' AND U.UM_STATUS='A'  " & _
                    "AND G.UU_USRGRP_ID =GM.UGM_USRGRP_ID " & _
                    "AND GM.UGM_FIXED_ROLE IN ('Billing Officer(F)','Billing Officer') " & _
                    "AND G.UU_USRGRP_ID IN ('Billing Officer(F)','Billing Officer') " & _
                    "AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "

            drw = objDB.GetView(strSql)
            Return drw
        End Function
		'------------------------------------------
        Function save_FO_AFO_IPP(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal AAO2 As String, ByVal AAO3 As String, ByVal AAO4 As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "")
            Dim strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String

            If AAO = "" Or AAO Is Nothing Then
                AAO = "NULL"
            Else
                AAO = "'" & AAO & "'"
            End If
            If AAO2 = "" Or AAO2 Is Nothing Then
                AAO2 = "NULL"
            Else
                AAO2 = "'" & AAO2 & "'"
            End If
            If AAO3 = "" Or AAO3 Is Nothing Then
                AAO3 = "NULL"
            Else
                AAO3 = "'" & AAO3 & "'"
            End If
            If AAO4 = "" Or AAO4 Is Nothing Then
                AAO4 = "NULL"
            Else
                AAO4 = "'" & AAO4 & "'"
            End If

            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                               "AND AGFO_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                                 "AND AGFO_FO = '" & AO & "'"

                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_FO_AFO_IPP = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_FO_AFO_IPP = WheelMsgNum.Duplicate
                Else
                    strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FO(AGFO_GRP_INDEX, AGFO_SEQ, AGFO_FO, AGFO_A_FO, AGFO_A_FO_2,AGFO_A_FO_3,AGFO_A_FO_4,AGFO_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "'," & AAO & "," & AAO2 & "," & AAO3 & "," & AAO4 & ",'" & relief & "')"
                    Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FO_AFO_IPP = WheelMsgNum.Save
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FO WHERE AGFO_GRP_INDEX='" & index & "' " & _
                                     "AND AGFO_FO = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_FO_AFO_IPP = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If
                strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FO set AGFO_SEQ = '" & lev & "', AGFO_FO='" & AO & "', AGFO_A_FO=" & AAO & ", AGFO_A_FO_2=" & AAO2 & ", AGFO_A_FO_3=" & AAO3 & ", AGFO_A_FO_4=" & AAO4 & ", AGFO_RELIEF_IND = '" & relief & "' " & _
                                  "WHERE AGFO_SEQ = '" & lev & "' AND AGFO_GRP_INDEX='" & index & "'"
                Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                If strSqlAry(0) <> String.Empty Then
                    objDb.BatchExecute(strSqlAry)
                    save_FO_AFO_IPP = WheelMsgNum.Save
                End If
            End If
        End Function

        Function save_FM_AFM_IPP(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal AAO2 As String, ByVal AAO3 As String, ByVal AAO4 As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "")
            Dim strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String

            If AAO = "" Or AAO Is Nothing Then
                AAO = "NULL"
            Else
                AAO = "'" & AAO & "'"
            End If
            If AAO2 = "" Or AAO2 Is Nothing Then
                AAO2 = "NULL"
            Else
                AAO2 = "'" & AAO2 & "'"
            End If
            If AAO3 = "" Or AAO3 Is Nothing Then
                AAO3 = "NULL"
            Else
                AAO3 = "'" & AAO3 & "'"
            End If
            If AAO4 = "" Or AAO4 Is Nothing Then
                AAO4 = "NULL"
            Else
                AAO4 = "'" & AAO4 & "'"
            End If

            '//ADD NEW AAO & AO
            If strModeRef = "A" Then

                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                               "AND AGFM_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                                 "AND AGFM_FM = '" & AO & "'"

                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_FM_AFM_IPP = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_FM_AFM_IPP = WheelMsgNum.Duplicate
                Else
                    strAddAo_AAO = "INSERT INTO APPROVAL_GRP_FM(AGFM_GRP_INDEX, AGFM_SEQ, AGFM_FM, AGFM_A_FM, AGFM_A_FM_2,AGFM_A_FM_3,AGFM_A_FM_4,AGFM_RELIEF_IND) values('" & index & "','" & lev & "','" & AO & "'," & AAO & "," & AAO2 & "," & AAO3 & "," & AAO4 & ",'" & relief & "')"
                    Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_FM_AFM_IPP = WheelMsgNum.Save
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_FM WHERE AGFM_GRP_INDEX='" & index & "' " & _
                                     "AND AGFM_FM = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_FM_AFM_IPP = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If
                strUpdateAo_AAO = "UPDATE APPROVAL_GRP_FM set AGFM_SEQ = '" & lev & "', AGFM_FM='" & AO & "', AGFM_A_FM=" & AAO & ", AGFM_A_FM_2=" & AAO2 & ", AGFM_A_FM_3=" & AAO3 & ", AGFM_A_FM_4=" & AAO4 & ", AGFM_RELIEF_IND = '" & relief & "' " & _
                                  "WHERE AGFM_SEQ = '" & lev & "' AND AGFM_GRP_INDEX='" & index & "'"
                Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                If strSqlAry(0) <> String.Empty Then
                    objDb.BatchExecute(strSqlAry)
                    save_FM_AFM_IPP = WheelMsgNum.Save
                End If
            End If
        End Function

        Function SearchItemForAssign(ByVal strItemCode As String, ByVal strItemName As String, ByVal strGrpindex As String, ByVal strRd As String, ByVal strIQCType As String) As DataSet
            Dim strSql, strTemp As String
            Dim ds As New DataSet

            If strRd = "1" Then
                strSql = "SELECT PM_PRODUCT_INDEX, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_PRODUCT_CODE, CT.CT_NAME, PM_UOM, PM_DELETED, " & _
                        "CASE WHEN AGP.AGP_GRP_INDEX IS NULL THEN 'N' ELSE 'Y' END AS CHECKED, CASE WHEN PM_DELETED = 'N' THEN 'Active' ELSE 'Inactive' END AS STATUS " & _
                        "FROM PRODUCT_MSTR PM " & _
                        "LEFT JOIN APPROVAL_GRP_PRODUCT AGP ON AGP.AGP_PRODUCT_CODE = PM.PM_PRODUCT_CODE, COMMODITY_TYPE CT WHERE " & _
                        "PM.PM_CATEGORY_NAME = CT.CT_ID " & _
                        "AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                        "AND PM_ITEM_TYPE = 'ST' AND PM_IQC_TYPE = '" & strIQCType & "' AND PM_IQC_IND = 'Y' " & _
                        "AND AGP_GRP_INDEX IS NULL "
            Else
                strSql = "SELECT PM_PRODUCT_INDEX, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_PRODUCT_CODE, CT.CT_NAME, PM_UOM, PM_DELETED, " & _
                        "CASE WHEN AGP.AGP_GRP_INDEX IS NULL THEN 'N' ELSE 'Y' END AS CHECKED, CASE WHEN PM_DELETED = 'N' THEN 'Active' ELSE 'Inactive' END AS STATUS " & _
                        "FROM PRODUCT_MSTR PM, APPROVAL_GRP_PRODUCT AGP , COMMODITY_TYPE CT " & _
                        "WHERE PM.PM_CATEGORY_NAME = CT.CT_ID " & _
                        "AND AGP.AGP_PRODUCT_CODE = PM.PM_PRODUCT_CODE " & _
                        "AND AGP_GRP_INDEX = '" & strGrpindex & "' "
            End If

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                strSql = strSql & " AND PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strItemName <> "" Then
                strTemp = Common.BuildWildCard(strItemName)
                strSql = strSql & " AND PM_PRODUCT_DESC" & Common.ParseSQL(strTemp)
            End If

            ds = objDb.FillDs(strSql)

            SearchItemForAssign = ds
        End Function

        Public Function InsertItemForApprGrp(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            'Insert new record(s)
            For i = 0 To dtItem.Rows.Count - 1
                strsql = "SELECT '*' " _
                   & "FROM APPROVAL_GRP_PRODUCT " _
                   & "WHERE AGP_GRP_INDEX='" & dtItem.Rows(i).Item("AGP_GRP_INDEX") & "' " _
                   & "AND AGP_PRODUCT_CODE='" & dtItem.Rows(i).Item("AGP_PRODUCT_CODE") & "' "

                If objDb.Exist(strsql) = 0 Then
                    strsql = "INSERT INTO APPROVAL_GRP_PRODUCT(" & _
                           "AGP_GRP_INDEX,AGP_PRODUCT_CODE) " & _
                           "VALUES('" & _
                           Common.Parse(dtItem.Rows(i).Item("AGP_GRP_INDEX")) & "','" & _
                           Common.Parse(dtItem.Rows(i).Item("AGP_PRODUCT_CODE")) & "')"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    InsertItemForApprGrp = WheelMsgNum.Save
                Else
                    InsertItemForApprGrp = WheelMsgNum.NotSave
                End If
            Else
                InsertItemForApprGrp = WheelMsgNum.Save
            End If

        End Function

        Public Function RemoveItemForApprGrp(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To dtItem.Rows.Count - 1
                strsql = "DELETE FROM APPROVAL_GRP_PRODUCT " _
                        & "WHERE AGP_GRP_INDEX='" & dtItem.Rows(i).Item("AGP_GRP_INDEX") & "' " _
                        & "AND AGP_PRODUCT_CODE='" & dtItem.Rows(i).Item("AGP_PRODUCT_CODE") & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    RemoveItemForApprGrp = WheelMsgNum.Delete
                Else
                    RemoveItemForApprGrp = WheelMsgNum.NotDelete
                End If
            End If

        End Function

        Public Function ChkIQCOfficerType(ByVal strGrpindex As String) As Boolean
            Dim strSql, strSql2, strSql3 As String
            Dim dsCount As New DataSet
            Dim i As Integer

            strSql = "SELECT '*' FROM APPROVAL_GRP_IQC " &
                    "WHERE AGI_GRP_INDEX = '" & strGrpindex & "' AND AGI_OFFICER_TYPE = 'IQCASTS'"

            If objDb.Exist(strSql) Then
                Return True
            Else
                strSql = "SELECT '*' FROM APPROVAL_GRP_IQC " &
                        "WHERE AGI_GRP_INDEX = '" & strGrpindex & "' AND AGI_OFFICER_TYPE = 'IQCV'"
                strSql2 = "SELECT '*' FROM APPROVAL_GRP_IQC " &
                        "WHERE AGI_GRP_INDEX = '" & strGrpindex & "' AND AGI_OFFICER_TYPE = 'IQCPA'"
                strSql3 = "SELECT '*' FROM APPROVAL_GRP_IQC " &
                        "WHERE AGI_GRP_INDEX = '" & strGrpindex & "' AND AGI_OFFICER_TYPE = 'IQCA'"

                If objDb.Exist(strSql) And objDb.Exist(strSql2) And objDb.Exist(strSql3) Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        Public Function getcboFAO(Optional ByVal type As String = "") As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " &
                                          objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " &
                                          "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " &
                                          "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                                          "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " &
                                          "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                                          "AND UU_USER_ID = M.UM_USER_ID " &
                                          "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'IPP Finance Approving Officer'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        'Jules 2018.07.23 - PAMB - To check whether user is tied to any active workflows.
        Function checkActiveWorkflow(ByVal strUserId As String, ByVal strApprGrp As String) As Boolean
            Dim strSQL As String

            If strApprGrp = "FO" Then
                strSQL &= "SELECT '*' FROM INVOICE_MSTR " &
                        "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_INVOICE_STATUS IN (1,2,5) AND IM_FOLDER = 0  AND IM_INVOICE_INDEX IN " &
                        "( SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL " &
                        "WHERE FA_SEQ - 1 = FA_AO_ACTION AND  (FA_AO = '" & strUserId & "' OR (FA_A_AO = '" & strUserId & "' AND FA_Relief_Ind='O'))) " &
                        "UNION SELECT '*' FROM INVOICE_MSTR " &
                        "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
                        "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_INVOICE_STATUS IN (11,12,19) " &
                        "AND (FA_AO = '" & strUserId & "' OR FA_A_AO='" & strUserId & "' OR FA_A_AO_2='" & strUserId & "' OR FA_A_AO_3='" & strUserId & "' OR FA_A_AO_4='" & strUserId & "')  AND FA_AO_ACTION = (FA_SEQ - 1) "
            Else
                strSQL &= "SELECT '*' FROM INVOICE_MSTR " &
                        "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_INVOICE_STATUS IN (2,5) AND IM_FOLDER = 0  AND IM_INVOICE_INDEX IN " &
                        "( SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL " &
                        "WHERE FA_SEQ - 1 = FA_AO_ACTION AND  (FA_AO = '" & strUserId & "' OR (FA_A_AO = '" & strUserId & "' AND FA_Relief_Ind='O')) " &
                        "AND (FA_AGA_TYPE = 'FM' OR (FA_AGA_TYPE = 'FO' AND FA_SEQ > FA_AO_ACTION))) " &
                        "UNION SELECT '*' FROM INVOICE_MSTR " &
                        "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
                        "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND IM_INVOICE_STATUS IN (12) AND (FA_AO = '" & strUserId & "' OR FA_A_AO='" & strUserId & "' OR FA_A_AO_2='" & strUserId & "' " &
                        "OR FA_A_AO_3='" & strUserId & "' OR FA_A_AO_4='" & strUserId & "') AND FA_AO_ACTION = (FA_SEQ - 1) "
            End If


            If objDb.Exist(strSQL) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        'Zulham 02082018 - PAMB
        Public Function getcboPAO(Optional ByVal type As String = "") As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "Select UM_USER_ID, UM_USER_NAME, " &
                                          objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " &
                                          "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " &
                                          "where R.UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                                          "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " &
                                          "AND M.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                                          "AND UU_USER_ID = M.UM_USER_ID " &
                                          "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Procurement Approving Officer'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        Function save_PAO_APAO(ByVal strModeRef As String, ByVal index As String, ByVal lev As String, ByVal AO As String, ByVal AAO As String, ByVal relief As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "", Optional ByVal strBC As String = "", Optional ByVal strCC As String = "")

            Dim strSQL, strcond, strSqlChkBuyer, strAddAo_AAO, strAddAo_AAO_2, strAddAo_AAO_3, strUpdateAo_AAO, strSqlAry(0) As String
            '//ADD NEW AAO & AO
            If strModeRef = "A" Then
                '//THIS CHECKING FOR LEVEL
                strAddAo_AAO = "SELECT '*' FROM APPROVAL_GRP_PAO WHERE AGPAO_GRP_INDEX='" & index & "' " &
                               "AND AGPAO_SEQ ='" & lev & "'"
                '//THIS CHECKING FOR AO
                strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_PAO WHERE AGPAO_GRP_INDEX='" & index & "' " &
                                 "AND AGPAO_AO = '" & AO & "'"

                strcond = " WHERE AGB_GRP_INDEX='" & index & "' AND AGB_BUYER='" & AO & "'"
                strSQL = objDb.Get1Column("APPROVAL_GRP_BUYER", "*", strcond)
                If objDb.Exist(strAddAo_AAO) > 0 Then
                    save_PAO_APAO = WheelMsgNum.Duplicate
                ElseIf objDb.Exist(strAddAo_AAO_2) > 0 Then
                    save_PAO_APAO = WheelMsgNum.Duplicate
                ElseIf strSQL <> "" Then
                    save_PAO_APAO = 0
                Else
                    If AAO = "" Then
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_PAO(AGPAO_GRP_INDEX, AGPAO_SEQ, AGPAO_AO, AGPAO_A_AO, AGPAO_RELIEF_IND"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", AGPAO_BRANCH_CODE, AGPAO_CC_CODE"
                        End If
                        strAddAo_AAO &= ") VALUES('" & index & "','" & lev & "','" & AO & "',NULL,'" & relief & "'"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", '" & Common.Parse(strBC) & "', '" & Common.Parse(strCC) & "'"
                        End If
                        strAddAo_AAO &= ")"
                        '-----------------------------------
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_PAO_APAO = WheelMsgNum.Save

                        End If
                    Else
                        strAddAo_AAO = "INSERT INTO APPROVAL_GRP_PAO(AGPAO_GRP_INDEX, AGPAO_SEQ, AGPAO_AO, AGPAO_A_AO, AGPAO_RELIEF_IND"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", AGPAO_BRANCH_CODE, AGPAO_CC_CODE"
                        End If
                        strAddAo_AAO &= ") VALUES('" & index & "','" & lev & "','" & AO & "','" & AAO & "','" & relief & "'"
                        If strBC <> "" And strCC <> "" Then
                            strAddAo_AAO &= ", '" & Common.Parse(strBC) & "', '" & Common.Parse(strCC) & "'"
                        End If
                        strAddAo_AAO &= ")"
                        '------------------------------------
                        Common.Insert2Ary(strSqlAry, strAddAo_AAO)
                        If strSqlAry(0) <> String.Empty Then
                            objDb.BatchExecute(strSqlAry)
                            save_PAO_APAO = WheelMsgNum.Save
                        End If
                    End If

                End If
            Else
                If strOld <> AO Then
                    strAddAo_AAO_2 = "SELECT '*' FROM APPROVAL_GRP_PAO WHERE AGPAO_GRP_INDEX='" & index & "' " &
                                     "AND AGPAO_AO = '" & AO & "'"
                    If objDb.Exist(strAddAo_AAO_2) > 0 Then
                        save_PAO_APAO = WheelMsgNum.Duplicate
                        Exit Function
                    End If
                End If

                If AAO = "" Then
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_PAO set AGPAO_SEQ = '" & lev & "', AGPAO_AO='" & AO & "', AGPAO_A_AO = NULL, AGPAO_RELIEF_IND = '" & relief & "' "
                    If strBC <> "" And strCC <> "" Then
                        strUpdateAo_AAO &= ", AGPAO_BRANCH_CODE = '" & Common.Parse(strBC) & "', AGPAO_CC_CODE = '" & Common.Parse(strCC) & "' "
                    End If
                    strUpdateAo_AAO &= "WHERE AGPAO_SEQ = '" & lev & "' AND AGPAO_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_PAO_APAO = WheelMsgNum.Save
                    End If
                Else
                    strUpdateAo_AAO = "UPDATE APPROVAL_GRP_PAO set AGPAO_SEQ = '" & lev & "', AGPAO_AO='" & AO & "', AGPAO_A_AO='" & AAO & "', AGPAO_RELIEF_IND = '" & relief & "' "
                    If strBC <> "" And strCC <> "" Then
                        strUpdateAo_AAO &= ", AGPAO_BRANCH_CODE = '" & Common.Parse(strBC) & "', AGPAO_CC_CODE = '" & Common.Parse(strCC) & "' "
                    End If
                    strUpdateAo_AAO &= "WHERE AGPAO_SEQ = '" & lev & "' AND AGPAO_GRP_INDEX='" & index & "'"
                    Common.Insert2Ary(strSqlAry, strUpdateAo_AAO)
                    If strSqlAry(0) <> String.Empty Then
                        objDb.BatchExecute(strSqlAry)
                        save_PAO_APAO = WheelMsgNum.Save
                    End If
                End If
            End If
        End Function

        Function BindPOAppGroupAsg(ByVal valgroup As String)
            Dim strget As String
            Dim dsApp As New DataSet
            'Modified for IPP GST Stage 2A - CH
            strget = "(SELECT B.UM_USER_ID as AO_ID,C.UM_USER_ID as AAO_ID,A.AGA_SEQ,A.AGA_GRP_INDEX, " &
                     "B.UM_USER_NAME as AO_NAME,B.UM_MASS_APP, C.UM_USER_NAME as AAO_NAME, AGA_RELIEF_IND, " &
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " &
                     "CASE WHEN AGA_A_AO IS NOT NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE,'' as AAO_ID2,'' as AAO_ID3,'' as AAO_ID4,'' as AGA_OFFICER_TYPE, AGA_BRANCH_CODE, AGA_CC_CODE " &
                     "FROM APPROVAL_GRP_MSTR M INNER JOIN " &
                     "APPROVAL_GRP_AO A ON M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " &
                     "LEFT OUTER JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND M.AGM_COY_ID = B.UM_COY_ID " &
                     "LEFT OUTER JOIN USER_MSTR C ON A.AGA_A_AO = C.UM_USER_ID AND M.AGM_COY_ID = C.UM_COY_ID " &
                     "where M.AGM_TYPE<>'INV' AND A.AGA_GRP_INDEX ='" & valgroup & "' " &
                     "AND M.AGM_GRP_INDEX = A.AGA_GRP_INDEX " &
                     "AND M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "ORDER BY A.AGA_SEQ)" &
                     "UNION ALL " &
                     "(SELECT B.UM_USER_ID AS AO_ID,C.UM_USER_ID AS AAO_ID,A.AGPAO_SEQ,A.AGPAO_GRP_INDEX, " &
                     "B.UM_USER_NAME AS AO_NAME, B.UM_MASS_APP, C.UM_USER_NAME As AAO_NAME, AGPAO_RELIEF_IND, " &
                     "CASE WHEN B.UM_STATUS = 'A' AND B.UM_DELETED <> 'Y' THEN 1 ELSE 0 END AS AO_ACTIVE, " &
                     "Case WHEN AGPAO_A_AO Is Not NULL THEN CASE WHEN C.UM_STATUS = 'A' AND C.UM_DELETED <> 'Y' THEN 1 ELSE 0 END ELSE 1 END AS AAO_ACTIVE,'' AS AAO_ID2,'' AS AAO_ID3,'' AS AAO_ID4,'' AS AGA_OFFICER_TYPE, AGPAO_BRANCH_CODE, AGPAO_CC_CODE " &
                     "From APPROVAL_GRP_MSTR M INNER Join " &
                     "APPROVAL_GRP_PAO A ON M.AGM_GRP_INDEX = A.AGPAO_GRP_INDEX " &
                     "Left OUTER JOIN USER_MSTR B ON A.AGPAO_AO = B.UM_USER_ID And M.AGM_COY_ID = B.UM_COY_ID " &
                     "Left OUTER JOIN USER_MSTR C ON A.AGPAO_A_AO = C.UM_USER_ID And M.AGM_COY_ID = C.UM_COY_ID " &
                     "WHERE M.AGM_TYPE <>'INV' AND A.AGPAO_GRP_INDEX ='" & valgroup & "' " &
                     "And M.AGM_GRP_INDEX = A.AGPAO_GRP_INDEX " &
                     "And M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "ORDER BY A.AGPAO_SEQ)"
            '---------------------------------

            dsApp = objDb.FillDs(strget)
            BindPOAppGroupAsg = dsApp
        End Function

        Public Function get_PAOAppLIMIT(ByVal strAOid As String, ByVal valindex As String, ByVal valseq As String, Optional ByVal strType As String = "") As DataSet
            Dim strSQL As String
            Dim dsAPP As New DataSet
            ' SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO
            strSQL = "SELECT AGPAO_GRP_INDEX 'AGA_GRP_INDEX', AGPAO_SEQ 'AGA_SEQ', AGPAO_AO 'AGA_AO' "
            If strType = "PO" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
            ElseIf strType = "MRS" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
            Else
                strSQL &= ", UM_APP_LIMIT "
            End If
            strSQL &= "FROM APPROVAL_GRP_PAO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGPAO_GRP_INDEX AND AGM_TYPE<>'INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGPAO_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID "
            strSQL &= "WHERE AGPAO_SEQ = ( "
            strSQL &= "Select MAX(AGPAO_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_PAO "
            strSQL &= "where AGPAO_GRP_INDEX = '" & valindex & "' AND AGPAO_SEQ < '" & valseq & "' "
            If strType = "PO" Then
                strSQL &= " AND AGM_TYPE = 'PO' "
            End If

            If strType = "MRS" Then
                strSQL &= " AND AGM_TYPE = 'MRS' "
            End If

            strSQL &= "GROUP BY AGPAO_GRP_INDEX) AND AGPAO_GRP_INDEX = '" & valindex & "' "
            strSQL &= "union "
            strSQL &= "SELECT AGPAO_GRP_INDEX 'AGA_GRP_INDEX', AGPAO_SEQ 'AGA_SEQ', AGPAO_AO 'AGA_AO' "
            If strType = "PO" Then
                strSQL &= ", UM_PO_APP_LIMIT AS UM_APP_LIMIT "
            Else
                strSQL &= ", UM_APP_LIMIT "
            End If
            strSQL &= "FROM APPROVAL_GRP_PAO "
            strSQL &= "LEFT JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGPAO_GRP_INDEX AND AGM_TYPE<>'INV' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQL &= "LEFT JOIN USER_MSTR ON UM_USER_ID = AGPAO_AO AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' AND AGM_COY_ID = UM_COY_ID  "
            strSQL &= "WHERE AGPAO_SEQ = ( "
            strSQL &= "Select MIN(AGPAO_SEQ) "
            strSQL &= "FROM APPROVAL_GRP_PAO "
            strSQL &= "where AGPAO_GRP_INDEX = '" & valindex & "' AND AGPAO_SEQ > '" & valseq & "' "
            If strType = "PO" Then
                strSQL &= " AND AGM_TYPE = 'PO' "
            End If

            If strType = "MRS" Then
                strSQL &= " AND AGM_TYPE = 'MRS' "
            End If
            strSQL &= "GROUP BY AGPAO_GRP_INDEX) AND AGPAO_GRP_INDEX = '" & valindex & "'"

            dsAPP = objDb.FillDs(strSQL)
            get_PAOAppLIMIT = dsAPP

        End Function

        'Zulham 04082018 - PAMB
        Function DelProcureAppGrpAsg(ByVal valgroup As String, ByVal strlevel As String) As Integer
            Dim strdelApp As String

            strdelApp = "Delete from APPROVAL_GRP_PAO where AGPAO_GRP_INDEX = '" & valgroup & "' AND AGPAO_SEQ='" & strlevel & "'"

            objDb.Execute(strdelApp)
            If objDb.Execute(strdelApp) Then
                DelProcureAppGrpAsg = WheelMsgNum.Delete
            Else
                DelProcureAppGrpAsg = WheelMsgNum.NotDelete
            End If
        End Function

    End Class
End Namespace