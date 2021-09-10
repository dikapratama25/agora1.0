Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class PurchaseReq2_Ext
        Inherits AppBaseClass
        Dim objDb As New EAD.DBCom

        Function isApproved(ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strUser As String, Optional ByVal strFor As String = "") As Boolean
            Dim strSql, strAO, strAAO, strActiveAO As String
            Dim intLastSeq As Integer
            intLastSeq = intCurrentSeq '- 1
            If strFor <> "" Then
                strSql = "SELECT PRA_AO,PRA_A_AO,PRA_ACTIVE_AO FROM PR_APPROVAL WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intLastSeq & " AND PRA_FOR = 'PO' "
            Else
                strSql = "SELECT PRA_AO,PRA_A_AO,PRA_ACTIVE_AO FROM PR_APPROVAL WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intLastSeq & " AND PRA_FOR <> 'PO' "
            End If
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_AO"))
                strAAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_A_AO"))
                strActiveAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_ACTIVE_AO"))
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

        Function getPRInfo(ByVal strPRNo As String, ByVal strPRIndex As String) As DataSet
            Dim strSql, strSqlPRM, strSqlPRD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPRField As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strPRField = "PM.PRM_PR_NO, PM.PRM_REQ_NAME,PM.PRM_REQ_PHONE, PM.PRM_PR_DATE, PM.PRM_SUBMIT_DATE, PM.PRM_B_ADDR_LINE1, PM.PRM_B_ADDR_LINE2, PM.PRM_B_ADDR_LINE3, " _
            & "PM.PRM_B_POSTCODE, PM.PRM_B_CITY, PM.PRM_INTERNAL_REMARK, PM.PRM_S_COY_ID, PM.PRM_S_COY_NAME, PM.PRM_CURRENCY_CODE,PM.PRM_BUYER_ID, " _
            & "PM.PRM_S_ADDR_LINE1,PM.PRM_S_ADDR_LINE2,PM.PRM_S_ADDR_LINE3,PM.PRM_S_POSTCODE,PM.PRM_S_CITY,PM.PRM_S_STATE,PM.PRM_S_COUNTRY,PM.PRM_S_PHONE,PM.PRM_S_FAX,PM.PRM_S_EMAIL,PM.PRM_SHIP_VIA,PM.PRM_FREIGHT_TERMS,PM.PRM_S_ATTN,PM.PRM_CREATED_DATE, " _
            & "PM.PRM_EXCHANGE_RATE, PM.PRM_External_Remark, PM.PRM_CONSOLIDATOR, PM.PRM_GST, PM.PRM_PR_Index, PM.PRM_PR_Status,PM.PRM_RFQ_INDEX,PM.PRM_PR_TYPE, PM.PRM_URGENT, PM.PRM_COY_ID, PM.PRM_STATUS_CHANGED_BY "

            strSqlPRM = "SELECT " & strPRField & ", CM.CM_TAX_CALC_BY," _
            & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.PRM_B_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.PRM_B_STATE AND CODE_VALUE=PM.PRM_B_COUNTRY) AS STATE, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR=PM.PRM_PAYMENT_TERM) AS PT," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR=PM.PRM_PAYMENT_TYPE) AS PM," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM) AS ST," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE) AS SC, " _
            & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(strCoyID) & "') AS NAME " _
            & "FROM PR_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.PRM_S_COY_ID=CM.CM_COY_ID " _
            & "WHERE " _
            & "PRM_COY_ID='" & strCoyID & "' AND PRM_PR_No='" & strPRNo & "'"

            'strSqlPRD = "SELECT PD.*, " _
            '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
            '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE " _
            '& "FROM PR_DETAILS PD " _
            '& "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"


            'Yik Foong
            '15 Oct 2009
            'Left join to return all rows from first table
            strSqlPRD = "SELECT PM.*, PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
            & "(SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PD.PRD_CT_ID) AS CT_NAME, " _
            & "(SELECT CONCAT(CDT_DEL_CODE, CONCAT(CONCAT(' (', CDT_DEL_NAME),')')) " _
            & "FROM company_delivery_term WHERE CDT_DEL_CODE = PD.PRD_DEL_CODE AND CDT_COY_ID = PD.PRD_COY_ID) AS DELIVERY_TERM, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE, PD.PRD_ORDERED_QTY * PD.PRD_UNIT_COST AS AMOUNT, CONCAT(PD.PRD_ASSET_GROUP, CONCAT(' ', IFNULL(PD.PRD_ASSET_NO,''))) AS ASSET_CODE, " _
            & "CASE WHEN PRD_GST_RATE = 'N/A' THEN PRD_GST_RATE ELSE " _
            & "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE " _
            & "FROM PR_DETAILS PD " _
            & "INNER JOIN PR_MSTR PM ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
            & "LEFT JOIN COMPANY_B_GL_CODE " _
            & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.PRD_B_GL_CODE AND CBG_B_COY_ID = PRM_COY_ID " _
            & "LEFT JOIN COMPANY_MSTR ON PRM_COY_ID = CM_COY_ID " _
            & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND CODE_ABBR = PRD_GST_RATE " _
            & "LEFT JOIN TAX ON TAX_CODE = PRD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
            & "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"



            'strSqlCustomF = "SELECT * FROM PR_CUSTOM_FIELD_MSTR A,PR_CUSTOM_FIELD_DETAILS B " _
            '& "WHERE B.PCD_PR_INDEX=A.PCM_PR_INDEX AND PCD_PR_INDEX=" & strPOIndex

            strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_PR_INDEX=" & strPRIndex & " AND PCD_TYPE='PR'"
            strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & strPRIndex & " AND PCM_TYPE='PR'"
            strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"


            strSql = strSqlPRM & ";" & strSqlPRD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
            ds = objDb.FillDs(strSql)
            'ds1 = getApprFlow(strPOIndex)

            ds.Tables(0).TableName = "PR_MSTR"
            ds.Tables(1).TableName = "PR_DETAILS"
            ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
            ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
            ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
            'Dim dt As New DataTable()
            'dt = ds1.Tables(0)
            'ds.Tables.Add(ds1.Tables(0))
            'ds.Tables(4).TableName = "PR_APPROVAL"
            Return ds
        End Function

        Function getPR(ByVal strPRNo As String, ByVal strPRIndex As String) As DataSet
            Dim strSql, strSqlPRM, strSqlPRD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPRField As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strPRField = "PM.PRM_PR_NO, PM.PRM_REQ_NAME,PM.PRM_REQ_PHONE, PM.PRM_PR_DATE, PM.PRM_B_ADDR_LINE1, PM.PRM_B_ADDR_LINE2, PM.PRM_B_ADDR_LINE3, PM.PRM_SUBMIT_DATE, " _
            & "PM.PRM_B_POSTCODE, PM.PRM_B_CITY, PM.PRM_INTERNAL_REMARK, PM.PRM_S_COY_ID, PM.PRM_S_COY_NAME, PM.PRM_CURRENCY_CODE,PM.PRM_BUYER_ID, " _
            & "PM.PRM_S_ADDR_LINE1,PM.PRM_S_ADDR_LINE2,PM.PRM_S_ADDR_LINE3,PM.PRM_S_POSTCODE,PM.PRM_S_CITY,PM.PRM_S_STATE,PM.PRM_S_COUNTRY,PM.PRM_S_PHONE,PM.PRM_S_FAX,PM.PRM_S_EMAIL,PM.PRM_SHIP_VIA,PM.PRM_FREIGHT_TERMS,PM.PRM_S_ATTN,PM.PRM_CREATED_DATE, " _
            & "PM.PRM_EXCHANGE_RATE, PM.PRM_External_Remark, PM.PRM_CONSOLIDATOR, PM.PRM_GST, PM.PRM_PR_Index, PM.PRM_PR_Status,PM.PRM_RFQ_INDEX, PM.PRM_PR_Type, PM.PRM_URGENT "

            strSqlPRM = "SELECT " & strPRField & ", CM.CM_TAX_CALC_BY," _
            & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.PRM_B_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.PRM_B_STATE AND CODE_VALUE=PM.PRM_B_COUNTRY) AS STATE, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR=PM.PRM_PAYMENT_TERM) AS PT," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR=PM.PRM_PAYMENT_TYPE) AS PM," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM) AS ST," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE) AS SC, " _
            & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME " _
            & "FROM PR_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.PRM_S_COY_ID=CM.CM_COY_ID " _
            & "WHERE " _
            & "PRM_COY_ID='" & strCoyID & "' AND PRM_PR_No='" & strPRNo & "'"

            'strSqlPRD = "SELECT PD.*, " _
            '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
            '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE " _
            '& "FROM PR_DETAILS PD " _
            '& "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"


            'Yik Foong
            '15 Oct 2009
            'Left join to return all rows from first table
            strSqlPRD = "SELECT PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
            & "(SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PD.PRD_CT_ID) AS CT_NAME, " _
            & "(SELECT CONCAT(CDT_DEL_CODE, CONCAT(CONCAT(' (', CDT_DEL_NAME),')')) " _
            & "FROM company_delivery_term WHERE CDT_DEL_CODE = PD.PRD_DEL_CODE AND CDT_COY_ID = PD.PRD_COY_ID) AS DELIVERY_TERM, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE, PD.PRD_ORDERED_QTY * PD.PRD_UNIT_COST AS AMOUNT,CM.CM_COY_NAME, " _
            & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME, CONCAT(PD.PRD_ASSET_GROUP, CONCAT(' ', PD.PRD_ASSET_NO)) AS ASSET_CODE, PD.PRD_GST_RATE AS GSTRATE, " _
            & "CASE WHEN PRD_GST_RATE = 'N/A' THEN PRD_GST_RATE ELSE " _
            & "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE " _
            & "FROM PR_DETAILS PD " _
            & "INNER JOIN PR_MSTR PM ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
            & "LEFT JOIN COMPANY_B_GL_CODE " _
            & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.PRD_B_GL_CODE AND CBG_B_COY_ID = PRM_COY_ID " _
            & "LEFT JOIN CONTRACT_DIST_MSTR CDM ON PRD_CD_GROUP_INDEX = CDM_GROUP_INDEX " _
            & "LEFT JOIN COMPANY_MSTR CM ON CDM_S_COY_ID = CM_COY_ID " _
            & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND CODE_ABBR = PRD_GST_RATE " _
            & "LEFT JOIN TAX ON TAX_CODE = PRD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
            & "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"



            'strSqlCustomF = "SELECT * FROM PR_CUSTOM_FIELD_MSTR A,PR_CUSTOM_FIELD_DETAILS B " _
            '& "WHERE B.PCD_PR_INDEX=A.PCM_PR_INDEX AND PCD_PR_INDEX=" & strPOIndex

            strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_PR_INDEX=" & strPRIndex & " AND PCD_TYPE = 'PR' "
            strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & strPRIndex & " AND PCM_TYPE = 'PR' "
            strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"


            strSql = strSqlPRM & ";" & strSqlPRD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
            ds = objDb.FillDs(strSql)
            'ds1 = getApprFlow(strPOIndex)

            ds.Tables(0).TableName = "PR_MSTR"
            ds.Tables(1).TableName = "PR_DETAILS"
            ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
            ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
            ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
            'Dim dt As New DataTable()
            'dt = ds1.Tables(0)
            'ds.Tables.Add(ds1.Tables(0))
            'ds.Tables(4).TableName = "PR_APPROVAL"
            Return ds
        End Function

        Function PRListForConvertPO(ByVal strPRNo As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strRole As String, ByVal strItemSP As String, ByVal strItemST As String, ByVal strItemMI As String, Optional ByVal strBuyer As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "") As DataSet
            Dim strSql As String
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
            Dim ds As DataSet
            Dim strType, strTemp As String

            strSql = "SELECT PM.*, PD.*, PMM.*, " _
            & " (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PMM.PM_LAST_TXN_S_COY_ID) AS PM_LAST_TXN_S_COY_NAME, " _
            & " (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = PM.PRM_COY_ID AND UM_USER_ID = PM.PRM_BUYER_ID) AS UM_USER_NAME " _
            & " FROM PR_MSTR PM " _
            & " INNER JOIN PR_DETAILS PD ON PM.PRM_PR_NO=PD.PRD_PR_NO AND PM.PRM_COY_ID=PD.PRD_COY_ID " _
            & " INNER JOIN COMMODITY_TYPE CM ON PD.PRD_CT_ID=CM.CT_ID " _
            & " INNER JOIN USER_ASSIGN UA ON PM.PRM_COY_ID=UA.UA_COY_ID AND CM.CT_ROOT_PREFIX=UA.UA_ASSIGN_VALUE  " _
            & " LEFT JOIN PRODUCT_MSTR PMM ON PD.PRD_PRODUCT_CODE=PMM.PM_PRODUCT_CODE AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strSql = strSql & " WHERE PM.PRM_PR_TYPE <> 'CC' AND PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "'"
            strSql = strSql & " AND PD.PRD_CONVERT_TO_IND IS NULL AND PM.PRM_PR_Status = " & PRStatus.Approved
            strSql = strSql & " AND UA.UA_TYPE = 'CT' AND UA.UA_USER_ID = '" & Common.Parse(strUser) & "' "

            If strPRNo <> "" Then
                strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
            End If

            If dteDateFr <> "" Then
                strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
            End If

            If dteDateTo <> "" Then
                strSql = strSql & " AND PRM_PR_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If pComType <> "" Then
                strSql &= "AND PD.PRD_CT_ID =" & pComType & " "
            End If

            If pItemType.Count > 0 Then
                For i As Integer = 0 To pItemType.Count - 1
                    If strType = "" Then
                        strType = "'" & pItemType(i) & "'"
                    Else
                        strType = strType & "," & "'" & pItemType(i) & "'"
                    End If
                Next
                strType = "(" & strType & ")"
                If strType <> "" Then
                    strSql &= " AND PRD_ITEM_TYPE IN " & strType
                End If

            Else
                If strItemSP = "Y" Or strItemST = "Y" Or strItemMI = "Y" Then
                    strSql &= " AND PRD_ITEM_TYPE IN ("

                    If strItemSP = "Y" Then
                        strTemp = "'SP'"
                    End If

                    If strItemST = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'ST'"
                        Else
                            strTemp &= "'ST'"
                        End If
                    End If

                    If strItemMI = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'MI'"
                        Else
                            strTemp &= "'MI'"
                        End If
                    End If

                    strSql &= strTemp & ")"

                End If
            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Function CreateRFQ(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
            Dim strTermFile, strAttachIndex As String
            Dim intIncrementNo As Integer = 0
            Dim objGlobal As New AppGlobals
            Dim strAryQuery(0) As String
            Dim dteNow As DateTime = Now()
            Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
            Dim strNewRFQNoList As String
            Dim strDelTerm As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            intIncrementNo += 1
            objGlobal.GetLatestDocNo("RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

            If strNewRFQNoList = "" Then
                strNewRFQNoList = strNewRFQNo & "!" & strVendor
            Else
                strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
            End If

            strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"
            If objDb.Exist(strSql) Then
                Return "error"
            End If

            objGlobal = Nothing

            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
                strEmail = objUser.Email
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
                strEmail = ""
            End If

            strSql = "SELECT IFNULL(PRD_DEL_CODE,'') FROM PR_DETAILS WHERE PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID='" & strCoyID & "' AND PRD_PR_LINE IN (" & strItemLine & ")"
            strDelTerm = objDb.GetVal(strSql)

            Dim txtDate1 As Date = CType(dteNow, Date)

            strSql = " INSERT INTO RFQ_MSTR " & _
                    " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " & _
                    " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " & _
                    " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " & _
                    " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " & _
                    " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK, RM_DEL_CODE, RM_CURRENCY_CODE) " & _
                    " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & HttpContext.Current.Session("CompanyID") & "', '', " & _
                    " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " & _
                    " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " & _
                    " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", (SELECT (CASE WHEN CM_RFQ_OPTION = 2 THEN 0 ELSE CM_RFQ_OPTION END) AS CM_RFQ_OPTION FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), '3', '0', " & _
                    " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, PRM_EXTERNAL_REMARK, '" & Common.Parse(strDelTerm) & "', PRM_CURRENCY_CODE" & _
                    " FROM PR_MSTR PM WHERE PRM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_No='" & strPRNo & "'"
            Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
            '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
            '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
            'Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
            'Common.Insert2Ary(pQuery, strSql)

            'Michelle (16/9/2011) - To include the Warranty term (871)
            strSql = " set @num  = 0; INSERT INTO RFQ_DETAIL " & _
                    " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " & _
                    " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " & _
                    " RD_UOM,RD_DELIVERY_LEAD_TIME, " & _
                    " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS, RD_ITEM_TYPE) " & _
                    " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " & _
                    " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " & _
                    " PRD_UOM, PR_DETAILS.PRD_ETD, " & _
                    " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS, PRD_ITEM_TYPE" & _
                    " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & "'" & _
                    " AND PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID = PRM_COY_ID "

            strSql &= " ORDER BY PRD_PR_LINE; set @num  = 0;"
            Common.Insert2Ary(pQuery, strSql)

            strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                    "PRD_GST = PM_LAST_TXN_TAX, " & _
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " & _
                    "AND PM_S_COY_ID = PRD_COY_ID " & _
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
            Common.Insert2Ary(pQuery, strSql)

            strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                     "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & _
                     "' AND PRD_COY_ID = '" & Common.Parse(strCoyID) & "' "
            Common.Insert2Ary(pQuery, strSql)

            Dim objWheelFile As New FileManagement
            'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
            strAttachIndex = objWheelFile.copyPRAttachToRFQ(strPRNo, strNewRFQNo)
            If strAttachIndex <> "" Then
                'If blnEnterpriseVersion = True Then
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'Else
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'End If

                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

                Common.Insert2Ary(pQuery, strSql)
            End If

            Return strNewRFQNoList
        End Function

        Function CreateRFQMulti(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
            Dim strTermFile, strAttachIndex As String
            Dim intIncrementNo As Integer = 0
            Dim objGlobal As New AppGlobals
            Dim strAryQuery(0) As String
            Dim dteNow As DateTime = Now()
            Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
            Dim strNewRFQNoList As String
            Dim strDelTerm As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            intIncrementNo += 1
            objGlobal.GetLatestDocNo("RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

            If strNewRFQNoList = "" Then
                strNewRFQNoList = strNewRFQNo & "!" & strVendor
            Else
                strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
            End If

            strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"

            If objDb.Exist(strSql) Then
                Return "error"
            End If

            objGlobal = Nothing

            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
                strEmail = objUser.Email
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
                strEmail = ""
            End If

            Dim txtDate1 As Date = CType(dteNow, Date)
            Dim strSQLPR, strSQLPR2 As String
            Dim strRemark As String

            Dim i As Integer
            For i = 0 To PRList.Count - 1
                If strSQLPR = "" Then
                    strSQLPR = "'" + PRList(i)(0) + "'"
                Else
                    strSQLPR = strSQLPR + ",'" + PRList(i)(0) + "'"
                End If

            Next

            strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSQLPR2 &= " AND (PRM_PR_NO IN (" & strSQLPR & ")) "
            strRemark = objDb.GetVal(strSQLPR2)

            strSql = "SELECT IFNULL(PRD_DEL_CODE,'') FROM PR_DETAILS WHERE PRD_PR_NO IN ('" & PRList(0)(0) & "') AND PRD_COY_ID='" & strCoyID & "' " & _
                    "AND PRD_PR_LINE IN (" & PRList(0)(2) & ")"
            strDelTerm = objDb.GetVal(strSql)

            Dim objWheelFile As New FileManagement
            'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
            strAttachIndex = objWheelFile.copyPRAttachToRFQMulti(strSQLPR, strNewRFQNo)
            If strAttachIndex <> "" Then
                'If blnEnterpriseVersion = True Then
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'Else
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'End If

                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

                Common.Insert2Ary(pQuery, strSql)
            End If

            strSql = " INSERT INTO RFQ_MSTR " & _
                    " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " & _
                    " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " & _
                    " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " & _
                    " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " & _
                    " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK, RM_DEL_CODE, RM_CURRENCY_CODE) " & _
                    " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & HttpContext.Current.Session("CompanyID") & "', '', " & _
                    " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " & _
                    " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " & _
                    " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", '0', '3', '0', " & _
                    " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, '" & Common.Parse(strRemark) & "', '" & Common.Parse(strDelTerm) & "', PRM_CURRENCY_CODE" & _
                    " FROM PR_MSTR PM WHERE PRM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_No='" & strPRNo & "'; "
            Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
            '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
            '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
            'Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
            'Common.Insert2Ary(pQuery, strSql)

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' Group All Duplicate
            'Dim tempItem As String
            'Dim tempQty As Integer = 0
            'Dim j As Integer
            'For i = 0 To PRList.Count - 1
            '    tempItem = PRList(i)(3)
            '    tempQty = PRList(i)(4)
            '    For j = 0 To PRList.Count - 1
            '        If tempItem = PRList(j)(3) Then
            '            tempQty = tempQty + IIf(PRList(j)(4) = "", 0, PRList(j)(4))
            '        End If
            '    Next
            '    PRList.Add(New String() {PRList(i)(0), PRList(i)(1), PRList(i)(2), PRList(i)(3), tempQty, "Done"})
            '    tempQty = 0
            'Next            

            For i = 0 To PRList.Count - 1
                'Michelle (16/9/2011) - To include the Warranty term (871)
                If strItemLine = "" Then
                    strSql = " INSERT INTO RFQ_DETAIL " & _
                            " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " & _
                            " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " & _
                            " RD_UOM,RD_DELIVERY_LEAD_TIME, " & _
                            " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS, RD_ITEM_TYPE ) " & _
                            " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " & _
                            " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " & _
                            " PRD_UOM, PR_DETAILS.PRD_ETD, " & _
                            " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS, PRD_ITEM_TYPE " & _
                            " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID ='" & strCoyID & "'" & _
                            " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

                    strSql &= " ORDER BY PRD_PR_LINE; "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                       "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                       "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                       "PRD_GST = PM_LAST_TXN_TAX, " & _
                       "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                       "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "' " & _
                       "AND PM_S_COY_ID = PRD_COY_ID " & _
                       "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                         "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
                    Common.Insert2Ary(pQuery, strSql)
                Else
                    strSql = " INSERT INTO RFQ_DETAIL " & _
                            " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " & _
                            " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " & _
                            " RD_UOM,RD_DELIVERY_LEAD_TIME, " & _
                            " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS, RD_ITEM_TYPE ) " & _
                            " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " & _
                            " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " & _
                            " PRD_UOM, PR_DETAILS.PRD_ETD, " & _
                            " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS, PRD_ITEM_TYPE " & _
                            " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID & "'" & _
                            " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

                    strSql &= " ORDER BY PRD_PR_LINE; "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                        "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                        "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                        "PRD_GST = PM_LAST_TXN_TAX, " & _
                        "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                        "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "' " & _
                        "AND PM_S_COY_ID = PRD_COY_ID " & _
                        "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                         "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
                    Common.Insert2Ary(pQuery, strSql)
                End If
            Next

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)

            Return strNewRFQNoList
        End Function

        Function DuplicatePR1(ByVal strPRNo As String, ByVal intPRIndex As Long) As String
            'Modified by Chee Hong on 17th April 2013
            Dim strSql, strArySql(0), strLoginUser As String
            Dim strNewPRNo, strPRPrefix, strCoyID As String
            Dim objGlobal As New AppGlobals
            Dim dblTotalCost As Double
            Dim dteNow As DateTime = Now()

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            'GetLatestDocNo
            objGlobal.GetLatestDocNo("PR", strArySql, strNewPRNo, strPRPrefix)

            ' user may change last used no in company param - may cause duplication of PR NO
            strSql = "SELECT '*' FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strNewPRNo) & "' "
            strSql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If objDb.Exist(strSql) > 0 Then
                ' msg retrieve eProcure modWheel
                Return "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem."
            End If

            ' ------- check item not deleted from List Price Catalogue ----------
            strSql = "SELECT '*' FROM PR_DETAILS "
            strSql &= "WHERE PRD_PR_NO = '" & Common.Parse(strPRNo) & "' "
            strSql &= "AND PRD_COY_ID = '" & strCoyID & "' "
            strSql &= "AND PRD_PRODUCT_CODE IN ("
            strSql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR "
            strSql &= "WHERE PM_DELETED = 'Y')"

            If objDb.Exist(strSql) > 0 Then
                Return "This PR cannot be duplicated because some of the items have been deleted from the List Price Catalogue."
            End If
            ' -------------------------------------------------------------------

            strNewPRNo = Common.Parse(strNewPRNo)
            strPRPrefix = Common.Parse(strPRPrefix)

            'Michelle (CR0010) - Blank off the remarks if duplication is from a canceled PR
            strSql = "INSERT INTO PR_MSTR(PRM_PR_NO,PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,PRM_PR_DATE,PRM_CREATED_DATE,PRM_SUBMIT_DATE," _
            & "PRM_STATUS_CHANGED_BY,PRM_STATUS_CHANGED_ON,PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,PRM_S_COY_NAME," _
            & "PRM_S_ADDR_LINE1,PRM_S_ADDR_LINE2,PRM_S_ADDR_LINE3,PRM_S_POSTCODE,PRM_S_CITY,PRM_S_STATE,PRM_S_COUNTRY," _
            & "PRM_S_PHONE,PRM_S_FAX,PRM_S_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,PRM_EXTERNAL_REMARK," _
            & "PRM_PR_STATUS,PRM_PO_INDEX,PRM_ARCHIVE_IND,PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,PRM_PR_PREFIX,PRM_B_ADDR_CODE," _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,PRM_RFQ_INDEX," _
            & "PRM_DEPT_INDEX,PRM_DUP_FROM,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,PRM_CONSOLIDATOR,PRM_PR_COST,PRM_GST,PRM_PR_TYPE) " _
            & "SELECT '" & strNewPRNo & "'," _
            & "PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,NULL," & Common.ConvertDate(dteNow) & ",NULL," _
            & "'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ",PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,CM_COY_NAME," _
            & "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY," _
            & "CM_PHONE,CM_FAX,CM_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,case PRM_PR_STATUS when '6' Then '' else PRM_EXTERNAL_REMARK end," _
            & PRStatus.Draft & ",NULL,'N',PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,'" & strPRPrefix & "',PRM_B_ADDR_CODE," _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,NULL,PRM_DEPT_INDEX," _
            & "PRM_PR_NO,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,NULL,PRM_PR_COST,PRM_GST,PRM_PR_TYPE FROM PR_MSTR A LEFT OUTER JOIN COMPANY_MSTR B " _
            & "ON A.PRM_S_COY_ID=B.CM_COY_ID WHERE PRM_PR_Index=" & intPRIndex
            Common.Insert2Ary(strArySql, strSql)

            '//need to check status of the product??
            '//need to get latest product info??
            'Michelle (16/6/2010) - To include Tax Code
            'Chee Hong (17/4/2013) - To include Delivery Code, Item Type, Oversea
            'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH - To include GST Tax Code
            strSql = "INSERT INTO PR_DETAILS(PRD_PR_NO,PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
            & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
            & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
            & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE," _
            & "PRD_CURRENCY_CODE,PRD_CT_ID,PRD_ASSET_GROUP,PRD_DEL_CODE,PRD_ITEM_TYPE,PRD_OVERSEA,PRD_LEAD_TIME) " _
            & "SELECT '" & strNewPRNo & "'," _
            & "PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
            & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
            & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
            & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE, " _
            & "PRD_CURRENCY_CODE,PRD_CT_ID,PRD_ASSET_GROUP,PRD_DEL_CODE,PRD_ITEM_TYPE,PRD_OVERSEA,PRD_LEAD_TIME " _
            & "FROM PR_DETAILS WHERE PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID='" & strCoyID & "'"
            Common.Insert2Ary(strArySql, strSql)

            '//need to check status of the custom field???
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR(PCM_PR_INDEX,PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE) SELECT " _
            & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & intPRIndex & " AND PCM_TYPE='PR'"
            Common.Insert2Ary(strArySql, strSql)

            strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS(PCD_PR_INDEX,PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE)  SELECT " _
            & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE FROM PR_CUSTOM_FIELD_DETAILS " _
            & "WHERE PCD_PR_INDEX=" & intPRIndex & " AND PCD_TYPE='PR'"
            Common.Insert2Ary(strArySql, strSql)

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
            Dim dtAttach As DataTable
            Dim dr As DataRow
            Dim objFile As New FileManagement
            Dim strHubFile, strActualFile As String
            Dim strBasePath, strPRPath As String
            Dim strSourceFile, strDestFile As String

            strBasePath = objFile.getBasePath(EnumUploadFrom.FrontOff)
            strPRPath = objFile.getSystemParam("DocAttachPath", "PR")
            objFile.checkDirectory(strBasePath & strPRPath)
            dtAttach = objDb.FillDs(strSql).Tables(0)
            For Each dr In dtAttach.Rows
                If Not IsDBNull(dr("CDA_HUB_FILENAME")) Then
                    strSourceFile = strBasePath & strPRPath & dr("CDA_HUB_FILENAME")
                    strHubFile = objFile.getLastDocAttachFileName(dr("CDA_HUB_FILENAME"))
                    strDestFile = strBasePath & strPRPath & strHubFile
                    'Michelle (17/6/2010) - To solve the problem where attachment is not create when duplicating a PR
                    'If System.IO.File.Exists(strDestFile) Then
                    If Not System.IO.File.Exists(strDestFile) Then
                        System.IO.File.Copy(strSourceFile, strDestFile)
                    End If
                    strSql = "INSERT INTO Company_Doc_Attachment(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) VALUES('" & strCoyID & "','" & _
                    strNewPRNo & "','PR','" & strHubFile & "','" & Common.Parse(dr("CDA_ATTACH_FILENAME")) & _
                    "'," & dr("CDA_FILESIZE") & ",'" & Common.Parse(dr("CDA_TYPE")) & "')"
                    Common.Insert2Ary(strArySql, strSql)
                End If
            Next

            objGlobal = Nothing
            objFile = Nothing
            If objDb.BatchExecute(strArySql) Then
                Return "Purchase Requisition Number " & strNewPRNo & " created."
            Else
                Return "Error occurs. Kindly contact the Administrator to resolve this. "
            End If

            ' strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
            '//return relevant parameter and redirect to Raise PR page
        End Function

        Function ApprovePR(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, _
        ByVal blnHighestLevel As Boolean, ByVal strConsolidator As String, ByVal strAORemark As String, _
        ByVal strBuyer As String, ByVal blnRelief As Boolean, ByVal strApprType As String, Optional ByVal strBC As String = "", Optional ByVal strAPP As String = "", Optional ByVal strRate As Decimal = 0.0) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strCoyID, strMsg, strPO, strPO1, strVendor, strLoginUser As String
            Dim intPRStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim strVendorNameList As String
            Dim arrPOInfo, arrPO As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strPoNo, strVen, strVenList As String
            Dim strPR_Type As String
            Dim strSqlLast As String

            If strApprType = "1" Then
                strApprType = "approved"
            Else
                strApprType = "endorsed"
            End If
            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId") 'is a AO
            '    strSql1 = "SELECT POM_S_COY_NAME FROM PO_MSTR WHERE  POM_B_COY_ID='" & strCoyID & "'"
            '   drdPR1 = objDb.GetReader(strSql1)
            '   If drdPR1.Read Then
            '   strvendorname = drdPR1("POM_S_COY_NAME")
            '    End If
            strSql = "SELECT PRM_PR_STATUS,PRM_STATUS_CHANGED_BY,PRM_S_COY_ID,PRM_S_COY_NAME, PRM_PR_TYPE FROM PR_MSTR WHERE PRM_PR_Index=" & intPRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intPRStatus = tDS.Tables(0).Rows(0).Item("PRM_PR_STATUS")
                strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRM_S_COY_ID"))
                If intPRStatus = PRStatus.CancelledBy Then
                    strMsg = "The PR has already been cancelled. Approving of this PR is not allowed. "
                ElseIf intPRStatus = PRStatus.RejectedBy Then 'And Common.parseNull(drdPR("PRM_STATUS_CHANGED_BY")) = strUserID Then
                    strMsg = "You have already rejected this PR. Approving of this PR is not allowed. "
                ElseIf intPRStatus = PRStatus.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved/endorsed this PR."
                ElseIf intPRStatus = PRStatus.ConvertedToPO Then
                    strMsg = "You have already converted this PR to PO. Approving of this PR is not allowed."
                End If
            End If
            strCoyID = HttpContext.Current.Session("CompanyId")
            strPR_Type = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRM_PR_TYPE"))
            'For intLoop = 0 To strAryPRNo.GetUpperBound(0)
            '    If intLoop = 0 Then
            '        strAllPR = "'" & strAryPRNo(intLoop) & "'"
            '        strAllPRIndex = strAryPRIndex(intLoop)
            '    Else
            '        strAllPR = strAllPR & ",'" & strAryPRNo(intLoop) & "'"
            '        strAllPRIndex = strAllPRIndex & "," & strAryPRIndex(intLoop)
            '    End If
            'Next

            If (intPRStatus = PRStatus.PendingApproval Or intPRStatus = PRStatus.Submitted) And isApproved(intPRIndex, intCurrentSeq, strLoginUser) Then
                strMsg = "You have already approved/endorsed this PR. Approving of this PR is not allowed."
            End If

            If strMsg <> "" Then
                Return strMsg
            End If
            If blnHighestLevel Then
                If strConsolidator = String.Empty And (strPR_Type = "CC") Then
                    '//update PR status to PO created, status_changed_by,status_changed_date
                    '//update PR_Approval
                    '//UPDATE PR_APPROVAL SET AO_REMARK=STRREMARK, ACTION_DATE=TODAY,AO_ACTION=CURRENT SEQ WHERE SEQ=CURRENT SEQ
                    '//UPDATE PR_APPROVAL SET AO_ACTION=CURRENT SEQ WHERE SEQ<>CURRENT SEQ
                    '//Create PO
                    '//send mail to Vendor, requestor   

                    Dim objDB As New EAD.DBCom
                    Dim Owner_Type As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_CONTR_PR_SETTING,'') AS CM_CONTR_PR_SETTING FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "' ")
                    If Owner_Type = "P" Then
                        Dim Owner_ID As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_CONTR_PR_PO_OWNER_ID,'') AS CM_CONTR_PR_PO_OWNER_ID FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "' ")
                        strBuyer = Owner_ID
                    End If

                    updateAOAction(intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PR")
                    strPO = CreatePO(strPRNo, intPRIndex, strVendor, strSqlAry, strBuyer, "", False, Nothing, strAPP, strSqlAryLast, , strRate)
                    If strPO = "error" Then
                        strMsg = Common.MsgTransDup
                        Return strMsg
                    End If

                    ' _Yap move down to after batch run
                    ' '' ''--Michelle Start
                    '' ''arrPOInfo = Split(strPO, ",")
                    '' ''intLowBound = LBound(arrPOInfo)
                    '' ''intUpBound = UBound(arrPOInfo)

                    ' '' ''    If UBound(arrPoNo) = 0 Then
                    ' '' ''   strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    ' '' ''      Else
                    '' ''For intLoop = 0 To intUpBound
                    '' ''    'To get the Vendor name
                    '' ''    arrPO = Split(arrPOInfo(intLoop), "!")
                    '' ''    strPoNo = arrPO(0) 'Capture the PO No.
                    '' ''    strVen = arrPO(1)  'Caputre the Vendor code

                    '' ''    strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                    '' ''    tDS = objDB.FillDs(strSql1)
                    '' ''    If tDS.Tables(0).Rows.Count > 0 Then
                    '' ''        strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                    '' ''    End If
                    '' ''    If intLoop = 0 Then
                    '' ''        strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '' ''        strVendorNameList = strvendorname
                    '' ''    Else
                    '' ''        strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '' ''        strVendorNameList = strVendorNameList & ", " & strvendorname
                    '' ''    End If

                    '' ''Next
                    '  End If
                    'strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    'strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    'If strMsg.Length > 0 Then
                    '    Dim intLoop, intCnt As Integer
                    '    intCnt = strMsg.Length
                    '    For intLoop = 0 To intCnt - 1
                    '        If intLoop = 0 Then
                    '            strMsg1 = strMsg
                    '            ' strMsg(0) = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    '        Else
                    '            strMsg1 = strMsg1 & """& vbCrLf & """ & strMsg
                    '        End If
                    '    Next
                    ''''Common.NetMsgbox(Me, strMsg)
                    'End If

                    '//return message = PO created
                Else
                    strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
                    Common.Insert2Ary(strSqlAry, strSql)

                    '//update PR status to Approved/Endorsed, status_changed_by,status_changed_date
                    strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.Approved & _
                    ", PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_BY='" & strLoginUser & "',PRM_STATUS_CHANGED_ON=" & _
                    Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex

                    Common.Insert2Ary(strSqlAry, strSql)
                    updateAOAction(intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                    strSqlLast = "SELECT 1"
                    Common.Insert2Ary(strSqlAryLast, strSqlLast)

                    '//send mail to consolidator
                    '//return message = PR Approved/endorsed
                    strMsg = "PR No. " & strPRNo & " has been " & strApprType & ". "
                End If
            Else
                strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
                Common.Insert2Ary(strSqlAry, strSql)

                '//update PR status, status_changed_by,status_changed_date
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.PendingApproval & _
                ",PRM_STATUS_CHANGED_BY='" & strLoginUser & "',PRM_STATUS_CHANGED_ON=" & _
                Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex

                Common.Insert2Ary(strSqlAry, strSql)
                updateAOAction(intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                strSqlLast = "SELECT 1"
                Common.Insert2Ary(strSqlAryLast, strSqlLast)

                '//notify next AO
                '//return message = PR Approved/endorsed
                strMsg = "PR No. " & strPRNo & " has been " & strApprType & ". "
            End If
            Dim objUsers As New Users
            objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_ApprovePR, strPRNo)
            objUsers = Nothing

            'If Not objDb.BatchExecute(strSqlAry) Then
            If Not objDb.BatchExecuteNew(strSqlAry, , strPO, "T_NO") Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                If strSqlAryLast(0) <> String.Empty Then
                    If Not objDb.BatchExecute(strSqlAryLast) Then
                        strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                    Else
                        ' _Yap: Move to here
                        '--Michelle Start
                        If strPO <> "" Then
                            arrPOInfo = Split(strPO, ",")
                            intLowBound = LBound(arrPOInfo)
                            intUpBound = UBound(arrPOInfo)

                            '    If UBound(arrPoNo) = 0 Then
                            '   strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                            '      Else
                            For intLoop = 0 To intUpBound
                                'To get the Vendor name
                                arrPO = Split(arrPOInfo(intLoop), "!")
                                strPoNo = arrPO(0) 'Capture the PO No.
                                strVen = arrPO(1)  'Caputre the Vendor code

                                strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                                tDS = objDb.FillDs(strSql1)
                                If tDS.Tables(0).Rows.Count > 0 Then
                                    strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                                End If
                                If intLoop = 0 Then
                                    strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                                    strVendorNameList = strvendorname
                                Else
                                    strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                                    strVendorNameList = strVendorNameList & ", " & strvendorname
                                End If

                            Next
                        End If
                        Common.NetMsgbox(Me, strMsg)

                        '//only send mail if transaction successfully created
                        Dim objMail As New Email
                        If blnHighestLevel And strPR_Type = "CC" Then
                            If strConsolidator = String.Empty Then
                                Dim objPR As New PR
                                Dim strName, strPOList As String
                                strName = objPR.getRequestorName("PR", strPRNo, strCoyID)

                                'Michelle (11/7/2007) - To cater for sending emails to multiple Vendors
                                '    If strPO.IndexOf(",") < 0 Then
                                '   objMail.sendNotification(EmailType.POCreated, strLoginUser, strCoyID, strVendor, strPRNo, strPO, strName)
                                '   objMail.sendNotification(EmailType.PORaised, "", strCoyID, strVendor, strPO, "")
                                '   objMail = Nothing
                                '  End If

                                For intLoop = 0 To intUpBound 'Sending email to vendors
                                    arrPO = Split(arrPOInfo(intLoop), "!")
                                    strPoNo = arrPO(0) 'Capture the PO No.
                                    strVen = arrPO(1)  'Caputre the Vendor code
                                    If intLoop = 0 Then
                                        strPOList = strPoNo
                                    Else
                                        strPOList = strPOList & ", " & strPoNo
                                    End If

                                    objMail.sendNotification(EmailType.PORaised, "", strCoyID, strVen, strPoNo, "")
                                Next
                                If intUpBound > 0 Then
                                    strVendorNameList = strVendorNameList & " respectively"
                                End If
                                objMail.sendNotification(EmailType.POCreated, strLoginUser, strCoyID, " ", strPRNo, strPOList, strName, strVendorNameList)

                                objPR = Nothing
                            Else
                                '//consolidator
                                '    objMail.sendNotification(EmailType.PRConsolidated, strLoginUser, strCoyID, strVendor, strPRNo, "")
                                objMail.sendNotification(EmailType.PRConsolidated, strLoginUser, strCoyID, strVenList, strPRNo, "")
                                objMail = Nothing
                            End If
                        ElseIf blnHighestLevel And strPR_Type <> "CC" Then
                            Dim objPR As New PR
                            objPR.sendMailToPO(strPRNo, intCurrentSeq + 1)
                            objPR.sendMailToBuyer(strPRNo, intCurrentSeq + 1)
                            objPR = Nothing
                        Else
                            '//next ao
                            Dim objPR As New PR
                            objPR.sendMailToAO(strPRNo, intPRIndex, intCurrentSeq + 1)
                            objPR = Nothing
                        End If
                    End If
                End If
            End If

            Return strMsg
        End Function

        Sub updateAOAction(ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByRef pQuery() As String, ByVal blnRelief As Boolean, Optional ByVal pFor As String = "")
            Dim strSql, strLoginUser As String
            strLoginUser = HttpContext.Current.Session("UserId")
            'PRA_RELIEF_AO
            If blnRelief Then
                strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" & _
                Common.ConvertDate(Now) & ",PRA_AO='" & strLoginUser & "',PRA_ON_BEHALFOF=PRA_AO,PRA_ACTIVE_AO='" & _
                strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
                If pFor = "PO" Then
                    strSql &= " AND PRA_FOR = 'PO'"
                Else
                    strSql &= " AND PRA_FOR <> 'PO'"
                End If
            Else
                strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" & _
                Common.ConvertDate(Now) & ",PRA_ACTIVE_AO='" & strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
                If pFor = "PO" Then
                    strSql &= " AND PRA_FOR = 'PO'"
                Else
                    strSql &= " AND PRA_FOR <> 'PO'"
                End If
            End If

            Common.Insert2Ary(pQuery, strSql)
            strSql = "UPDATE PR_APPROVAL SET PRA_AO_ACTION = " & intCurrentSeq & " WHERE PRA_PR_INDEX=" & intPRIndex '& " AND PRA_SEQ=" & intCurrentSeq
            If pFor = "PO" Then
                strSql &= " AND PRA_FOR = 'PO'"
            Else
                strSql &= " AND PRA_FOR <> 'PO'"
            End If
            Common.Insert2Ary(pQuery, strSql)
        End Sub

        Function CreatePOHeader(ByVal strPRNo As String, ByVal strVendor As String, ByVal strNewPONo As String, _
        ByVal strPOPrefix As String, ByRef pQuery() As String, ByVal dblPOCost As Double, _
        ByVal strUserID As String, ByVal blnPreviewPO As Boolean, ByVal strTermFile As String, _
        Optional ByVal blnConsolidate As Boolean = False, Optional ByVal strConsoRemark As String = "", Optional ByVal strCurr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByVal strIntR As String = "", Optional ByVal strExtR As String = "", Optional ByVal strRateHead As Decimal = 0.0)
            Dim strSql, strAryQuery(0), strCoyID, strLoginUser As String
            Dim strUserName As String, strPhone As String, strFax As String
            Dim strBillMethod, strTable, strCondition, strPayTerm, strPayMethod, strDelCode As String
            Dim dteNow As DateTime = Now()

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")
            '//If got consolidation, Requestor become AO
            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
            End If

            ' Michelle (CR0012) - To check whether Payment Terms and Payment Methods have been selected
            'strSql = "SELECT ISNULL(PRM_PAYMENT_TERM,'') FROM PR_MSTR WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'"
            'strPayTerm = objDb.GetVal(strSql)

            'strSql = "SELECT ISNULL(PRM_PAYMENT_TYPE,'') FROM PR_MSTR" & _
            '" WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'"
            'strPayMethod = objDb.GetVal(strSql)

            ''Get the default payment term and method
            'If strPayTerm = "" Then
            '    strPayTerm = objDb.GetVal("SELECT CV_PAYMENT_TERM FROM COMPANY_VENDOR" _
            '    & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")
            'End If

            'If strPayMethod = "" Then
            '    strPayMethod = objDb.GetVal("SELECT CV_PAYMENT_METHOD FROM COMPANY_VENDOR" _
            '& " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")
            'End If

            strPayTerm = objDb.GetVal("SELECT CV_PAYMENT_TERM FROM COMPANY_VENDOR" _
                & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")

            strPayMethod = objDb.GetVal("SELECT CV_PAYMENT_METHOD FROM COMPANY_VENDOR" _
                        & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")

            If strItemLine <> "" Then
                strSql = "SELECT PRD_DEL_CODE FROM PR_DETAILS  WHERE PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID = '" & strCoyID & "' AND PRD_PR_LINE IN (" & strItemLine & ")"
            Else
                strSql = "SELECT PRD_DEL_CODE FROM PR_DETAILS  WHERE PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID = '" & strCoyID & "'"
            End If

            strDelCode = objDb.GetVal(strSql)

            '//If got consolidation, Requestor become AO

            '//Get Billing Method
            '//GetVal = return one skalar value, value returned must not be null
            strSql = "SELECT ISNULL(CV_BILLING_METHOD,'FPO') FROM COMPANY_VENDOR WHERE CV_B_COY_ID='" & strCoyID & _
            "' AND CV_S_COY_ID='" & strVendor & "'"
            strBillMethod = objDb.GetVal(strSql)
            If Trim(strBillMethod) = "" Then strBillMethod = "FPO"
            '//Get Billing Method

            '//preview of PR consolidation           
            If blnPreviewPO Then
                strTable = "PO_MSTR_PREVIEW"
            Else
                strTable = "PO_MSTR"
            End If

            If blnConsolidate Then
                strCondition = "'" & Common.Parse(strIntR) & "', '" & Common.Parse(strConsoRemark) & "'"
            Else
                strCondition = "'" & Common.Parse(strIntR) & "', '" & Common.Parse(strExtR) & "'"
            End If

            strSql = "INSERT INTO " & strTable & "(" _
            & "POM_PO_NO,POM_B_COY_ID,POM_BUYER_ID,POM_BUYER_NAME,POM_BUYER_PHONE,POM_BUYER_FAX," _
            & "POM_S_COY_ID,POM_S_COY_NAME,POM_S_ATTN,POM_S_ADDR_LINE1," _
            & "POM_S_ADDR_LINE2,POM_S_ADDR_LINE3,POM_S_POSTCODE,POM_S_CITY," _
            & "POM_S_STATE,POM_S_COUNTRY,POM_S_PHONE,POM_S_FAX,POM_S_EMAIL,POM_PO_DATE,POM_CREATED_DATE," _
            & "POM_FREIGHT_TERMS,POM_PAYMENT_TERM,POM_PAYMENT_METHOD,POM_SHIPMENT_MODE," _
            & "POM_SHIPMENT_TERM,POM_CURRENCY_CODE,POM_EXCHANGE_RATE," _
            & "POM_SHIP_VIA,POM_PO_STATUS,POM_STATUS_CHANGED_BY," _
            & "POM_STATUS_CHANGED_ON,POM_INTERNAL_REMARK, POM_EXTERNAL_REMARK, POM_CREATED_BY, POM_PO_COST," _
            & "POM_BILLING_METHOD,POM_PO_PREFIX,POM_B_ADDR_CODE,POM_B_ADDR_LINE1,POM_B_ADDR_LINE2," _
            & "POM_B_ADDR_LINE3,POM_B_POSTCODE,POM_B_CITY,POM_B_STATE,POM_B_COUNTRY,POM_FULFILMENT," _
            & "POM_DEPT_INDEX,POM_TERMANDCOND, POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SUBMIT_DATE, POM_URGENT, POM_DEL_CODE) SELECT " & strNewPONo & ",'" & strCoyID & "','" & strUserID & "','" & _
            Common.Parse(strUserName) & "','" & strPhone & "','" & strFax & "'," _
            & "'" & strVendor & "',CM_COY_NAME, PRM_S_ATTN, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, " _
            & "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "

            If strAPP = "APP" Then
                strSql &= "" & Common.ConvertDate(dteNow) & ", "
            Else
                strSql &= " NULL, "
            End If


            strSql &= "" & Common.ConvertDate(dteNow) & ", PRM_FREIGHT_TERMS," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR='" & strPayTerm & "'), " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR='" & strPayMethod & "'), " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE)," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM)"

            Dim POStatus As String
            If strItemLine = "" Then
                POStatus = POStatus_new.NewPO
                ' strSql &= ",PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_SHIP_VIA,'" & _
                strSql &= ",'" & strCurr & "'," & strRateHead & ",PRM_SHIP_VIA,'" & _
                        POStatus & "','" & strLoginUser & "'," & Common.ConvertDate(dteNow)
            Else
                POStatus = POStatus_new.Draft
                strSql &= ",'" & strCurr & "'," & strRateHead & ",PRM_SHIP_VIA,'" & _
                        POStatus & "','" & strLoginUser & "'," & Common.ConvertDate(dteNow)
            End If

            strSql &= "," & strCondition & ",'" & strLoginUser & "'," & dblPOCost & ",'" & strBillMethod & "'," & strPOPrefix & ",PRM_B_ADDR_CODE, " _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_CITY," _
            & "PRM_B_STATE,PRM_B_COUNTRY," & Fulfilment.null & ",PRM_DEPT_INDEX,'" & strTermFile & "', PRM_PRINT_CUSTOM_FIELDS, PRM_PRINT_REMARK," & Common.ConvertDate(dteNow) & ", PRM_URGENT, '" & Common.Parse(strDelCode) & "' FROM PR_MSTR PM, (SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(strVendor) & "') AS CM WHERE PRM_COY_ID='" & strCoyID & _
            "' AND PRM_PR_No='" & strPRNo & "'"
            Common.Insert2Ary(pQuery, strSql)

            If Not blnPreviewPO Then
                Dim objTrack As New Tracking
                strSql = objTrack.updateDocMatchingNew(strNewPONo, "", "", "PO", strVendor, strCoyID)
                Common.Insert2Ary(pQuery, strSql)
                objTrack = Nothing
            End If

            objUsers = Nothing
            objUser = Nothing
            '//update RFQ_Replies
        End Function

        Function CreatePODetail(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByVal strNewPONo As String, _
        ByRef pQuery() As String, ByRef intPOLine As Integer, ByVal strUserID As String, Optional ByVal blnPreviewPO As Boolean = False, Optional ByVal strCurr As String = "", Optional ByVal strAddr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal strAPP As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strSql, strCoyID, strTable, strLoginUser As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            Dim objUsers As New Users
            Dim i As Integer

            If blnPreviewPO Then
                strTable = "PO_DETAILS_PREVIEW"
            Else
                strTable = "PO_DETAILS"
            End If

            '//preview of PR consolidation
            '//not suppose to update PR because no actual PO created
            If Not blnPreviewPO Then
                'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                '& "PRM_PR_STATUS = " & PRStatus.ConvertedToPO & ",PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                '"', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                'Common.Insert2Ary(pQuery, strSql)

                strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                        & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                        "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                Common.Insert2Ary(pQuery, strSql)


                'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
                'Common.Insert2Ary(pQuery, strSql)
            End If

            If strItemLine = "" Then
                'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH - Include Gst Tax Code
                strSql = "set @num  = 0; INSERT INTO " & strTable & "(" _
                    & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                    & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                    & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                    & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_GST_RATE,POD_GST_INPUT_TAX_CODE,POD_TAX_VALUE,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                    & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                    & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE, " _
                    & "POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_ITEM_TYPE,POD_OVERSEA,POD_SPEC1,POD_SPEC2,POD_SPEC3,POD_LEAD_TIME) SELECT " & _
                    strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                    & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PRD_UNIT_COST,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,(ROUND(ROUND(PRD_ORDERED_QTY*PRD_UNIT_COST,2)*PRD_GST/100,2))," & _
                    intPRIndex & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                    & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                    & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_ITEM_TYPE,PRD_OVERSEA, " _
                    & "(SELECT PM_SPEC1 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                    & "(SELECT PM_SPEC2 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                    & "(SELECT PM_SPEC3 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),PRD_LEAD_TIME " _
                    & "FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' "
            Else
                'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH - Include Gst Tax Code
                strSql = "set @num  = 0; INSERT INTO " & strTable & "(" _
                    & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                    & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                    & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                    & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_GST_RATE,POD_GST_INPUT_TAX_CODE,POD_TAX_VALUE,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                    & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                    & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE, " _
                    & "POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_ITEM_TYPE,POD_OVERSEA,POD_SPEC1,POD_SPEC2,POD_SPEC3,POD_LEAD_TIME) SELECT " & _
                    strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                    & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PM_LAST_TXN_PRICE,PRD_REMARK,PM_LAST_TXN_TAX,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,IF(PRD_GST IS NULL,0,(ROUND(ROUND(PRD_ORDERED_QTY*PRD_UNIT_COST,2)*PRD_GST/100,2))) AS PRD_GST," & _
                    intPRIndex & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                    & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                    & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_ITEM_TYPE,PRD_OVERSEA, " _
                    & "(SELECT PM_SPEC1 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                    & "(SELECT PM_SPEC2 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                    & "(SELECT PM_SPEC3 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),PRD_LEAD_TIME " _
                    & "FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' "
            End If

            If strVendor <> "" Then
                strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
            End If

            If strCurr <> "" Then
                strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
            End If

            If strAddr <> "" Then
                strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
            End If

            strSql &= " ORDER BY PRD_PR_LINE; set @num  = 0;"

            Common.Insert2Ary(pQuery, strSql)

            Dim ds As New DataSet
            'If blnEnterpriseVersion = True Then
            '    strSql = "SELECT PRD_PR_NO, PRD_PR_LINE, PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPRNo & "' "
            '    strSql &= "AND (PRD_ASSET_GROUP IS NOT NULL AND PRD_ASSET_GROUP <> '') AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '    ds = objDb.FillDs(strSql)

            '    Dim strAssetNo As String
            '    Dim intAssetIncrementNo As Integer
            '    intAssetIncrementNo = 1
            '    For i = 0 To ds.Tables(0).Rows.Count - 1
            '        Common.parseNull(ds.Tables(0).Rows(i)("PRD_ASSET_GROUP"))

            '        Dim strParam As String = objDb.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            '        If strParam = "" Then
            '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
            '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '            strSql &= "'Prefix', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'eProcure' ) "
            '            Common.Insert2Ary(pQuery, strSql)

            '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
            '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '            strSql &= "'Last Used No', "
            '            strSql &= "'00000000', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'eProcure' ) "
            '            Common.Insert2Ary(pQuery, strSql)
            '        End If

            '        strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
            '                    & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
            '                    & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
            '                    & " WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

            '        strSql = "UPDATE PO_DETAILS SET "
            '        strSql &= "POD_ASSET_NO = " & strAssetNo & " "
            '        strSql &= "WHERE POD_PO_NO = " & strNewPONo & " "
            '        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '        strSql &= "AND POD_PO_LINE = '" & ds.Tables(0).Rows(i)("PRD_PR_LINE") & "' "
            '        Common.Insert2Ary(pQuery, strSql)

            '        strSql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' "
            '        Common.Insert2Ary(pQuery, strSql)

            '        'intAssetIncrementNo += 1
            '    Next
            'End If

            If strItemLine = "" Then
                strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                     "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_NO = '" & strPRNo & "'"
                Common.Insert2Ary(pQuery, strSql)

                'strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                '    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                '    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                '    "PRD_GST = PM_LAST_TXN_TAX, " & _
                '    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                '    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "' " & _
                '    "AND PM_S_COY_ID = PRD_COY_ID " & _
                '    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                'Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                     "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "'"
                Common.Insert2Ary(pQuery, strSql)

                'strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                '    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                '    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                '    "PRD_GST = PM_LAST_TXN_TAX, " & _
                '    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                '    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " & _
                '    "AND PM_S_COY_ID = PRD_COY_ID " & _
                '    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                'Common.Insert2Ary(pQuery, strSql)
            End If

            'If strItemLine = "" Then
            '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
            '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "'"
            '    Common.Insert2Ary(pQuery, strSql)
            'Else
            '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
            '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "'"
            '    Common.Insert2Ary(pQuery, strSql)
            'End If

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID='" & Common.Parse(strCoyID) & "') = 0"
            'Common.Insert2Ary(pQuery, strSql)

            'Update last transacted price
            If strAPP = "APP" Then
                strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " & _
                                    "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " & _
                                    "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE, " & _
                                    "PM_LAST_TXN_TAX = POD_GST, " & _
                                    "PM_LAST_TXN_S_COY_ID = '" & strVendor & "' " & _
                                    "WHERE POM_PO_NO = " & strNewPONo & " AND POM_B_COY_ID = POD_COY_ID " & _
                                    "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " & _
                                    "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            End If

            ' Here new enhancement on PO Custom will be create from the PR Custom Field.
            ' 29/Aug/2011
            ' Create PO Custom Field from PR Custom Field
            ' PR_CUSTOM_FIELD_DETAILS table
            ' _Yap Backup this portion
            'If strItemLine = "" Then
            '    strSql = "set @num  = 1; SET @PCD_FIELD_NO = 0;INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
            '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
            '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
            '        & "@num := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num, @num+1) AS d4, " _
            '        & "@num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num, 1) AS LINE_NO, " _
            '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
            '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
            '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
            '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
            '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
            '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
            '        "' AND PRD_PR_NO='" & strPRNo & "' "
            'Else
            '    strSql = "set @num  = 1; SET @PCD_FIELD_NO = 0;INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
            '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
            '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
            '        & "@num := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num, @num+1) AS d4, " _
            '        & "@num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num, 1) AS LINE_NO, " _
            '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
            '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
            '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
            '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
            '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
            '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
            '        "' AND PRD_PR_NO='" & strPRNo & "' "
            'End If

            If strItemLine = "" Then
                strSql = "set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                    & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                    & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                    & "@num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, " _
                    & "@num AS LINE_NO, " _
                    & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                    & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                    & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                    & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                    & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                    & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' "

                '& "IF(@PCD_FIELD_NO=1,@num,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

            Else
                strSql = "set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                    & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                    & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                    & "@num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, " _
                    & "@num AS LINE_NO, " _
                    & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                    & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                    & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                    & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                    & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                    & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' "

                '& "IF(@PCD_FIELD_NO=1,@num,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

            End If

            If strVendor <> "" Then
                strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
            End If
            If strCurr <> "" Then
                strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
            End If
            If strAddr <> "" Then
                strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
            End If
            strSql &= " ORDER BY PRD_PR_LINE, PCD_FIELD_NO) XXX) ZZZ; set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' Here new enhancement on PO Custom will be create from the PR Custom Field.
            ' 29/Aug/2011
            ' Create PO Custom Field from PR Custom Field
            ' PR_CUSTOM_FIELD_MSTR table
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (" _
            & "PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT " _
            & "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), PCM_FIELD_NO, PCM_FIELD_NAME, 'PO' " _
            & "FROM PR_MSTR PM, PR_CUSTOM_FIELD_MSTR WHERE PRM_PR_INDEX = PCM_PR_INDEX AND PRM_COY_ID='" & strCoyID & _
            "' AND PRM_PR_No='" & strPRNo & "' AND PCM_TYPE = 'PR'"
            Common.Insert2Ary(pQuery, strSql)

            '//for PR Consolidation
            Dim intCnt As Integer = 0
            If strVendor <> "" Then
                intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' AND PRD_S_COY_ID = '" & strVendor & "'")
            Else
                intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                    "' AND PRD_PR_NO='" & strPRNo & "' ")
            End If

            intPOLine += intCnt

            If Not blnPreviewPO Then
                objUsers.Log_UserActivityNew(pQuery, WheelModule.PRMod, WheelUserActivity.AO_CreatePO, strNewPONo, strPRNo)
            End If
            objUsers = Nothing
        End Function

        Function CreatePODetailMulti(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByVal strNewPONo As String, _
        ByRef pQuery() As String, ByRef intPOLine As Integer, ByVal strUserID As String, Optional ByVal blnPreviewPO As Boolean = False, Optional ByVal strCurr As String = "", Optional ByVal strAddr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strSql, strCoyID, strTable, strLoginUser As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            Dim objUsers As New Users
            Dim i, j As Integer

            If blnPreviewPO Then
                strTable = "PO_DETAILS_PREVIEW"
            Else
                strTable = "PO_DETAILS"
            End If

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)
            strSql = " SET @num2  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            For i = 0 To PRList.Count - 1
                '//preview of PR consolidation
                '//not suppose to update PR because no actual PO created
                If Not blnPreviewPO Then
                    'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                    '& "PRM_PR_STATUS = " & PRStatus.ConvertedToPO & ",PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                    '"', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                    'Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                            & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                            "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & PRList(i)(1)
                    Common.Insert2Ary(pQuery, strSql)



                    'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
                    'Common.Insert2Ary(pQuery, strSql)
                End If

                If strItemLine = "" Then
                    strSql = " INSERT INTO " & strTable & "(" _
                        & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                        & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                        & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                        & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                        & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                        & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_SPEC1,POD_SPEC2,POD_SPEC3,POD_ITEM_TYPE,POD_OVERSEA) SELECT " & _
                        strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                        & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PRD_UNIT_COST,PRD_REMARK,PRD_GST," & _
                        PRList(i)(1) & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                        & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                        & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO," _
                        & "(SELECT PM_SPEC1 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "(SELECT PM_SPEC2 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "(SELECT PM_SPEC3 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "PRD_ITEM_TYPE,PRD_OVERSEA FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                Else
                    strSql = " INSERT INTO " & strTable & "(" _
                        & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                        & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                        & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                        & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                        & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                        & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_SPEC1,POD_SPEC2,POD_SPEC3,POD_ITEM_TYPE,POD_OVERSEA) SELECT " & _
                        strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                        & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PM_LAST_TXN_PRICE,PRD_REMARK,PM_LAST_TXN_TAX," & _
                        PRList(i)(1) & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                        & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                        & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO," _
                        & "(SELECT PM_SPEC1 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "(SELECT PM_SPEC2 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "(SELECT PM_SPEC3 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE), " _
                        & "PRD_ITEM_TYPE,PRD_OVERSEA " _
                        & "FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID & _
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "
                End If

                If strVendor <> "" Then
                    strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
                End If

                If strCurr <> "" Then
                    strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
                End If

                If strAddr <> "" Then
                    strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
                End If

                strSql &= " ORDER BY PRD_PR_LINE;"
                Common.Insert2Ary(pQuery, strSql)

                Dim ds As New DataSet
                'If blnEnterpriseVersion = True Then
                '    strSql = "SELECT PRD_PR_NO, PRD_PR_LINE, PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPRNo & "' "
                '    strSql &= "AND (PRD_ASSET_GROUP IS NOT NULL AND PRD_ASSET_GROUP <> '') AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                '    ds = objDb.FillDs(strSql)

                '    Dim strAssetNo As String
                '    Dim intAssetIncrementNo As Integer
                '    intAssetIncrementNo = 1
                '    For j = 0 To ds.Tables(0).Rows.Count - 1
                '        Common.parseNull(ds.Tables(0).Rows(j)("PRD_ASSET_GROUP"))

                '        Dim strParam As String = objDb.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                '        If strParam = "" Then
                '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                '            strSql &= "'Prefix', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'eProcure' ) "
                '            Common.Insert2Ary(pQuery, strSql)

                '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                '            strSql &= "'Last Used No', "
                '            strSql &= "'00000000', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'eProcure' ) "
                '            Common.Insert2Ary(pQuery, strSql)
                '        End If

                '        strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
                '                    & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
                '                    & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
                '                    & " WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

                '        strSql = "UPDATE PO_DETAILS SET "
                '        strSql &= "POD_ASSET_NO = " & strAssetNo & " "
                '        strSql &= "WHERE POD_PO_NO = " & strNewPONo & " "
                '        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                '        strSql &= "AND POD_PO_LINE = '" & ds.Tables(0).Rows(j)("PRD_PR_LINE") & "' "
                '        Common.Insert2Ary(pQuery, strSql)

                '        strSql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' "
                '        Common.Insert2Ary(pQuery, strSql)

                '        'intAssetIncrementNo += 1
                '    Next
                'End If

                If strItemLine = "" Then
                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                    "PRD_GST = PM_LAST_TXN_TAX, " & _
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "' " & _
                    "AND PM_S_COY_ID = PRD_COY_ID " & _
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                Else
                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                    "PRD_GST = PM_LAST_TXN_TAX, " & _
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "' " & _
                    "AND PM_S_COY_ID = PRD_COY_ID " & _
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)
                End If

                'If strItemLine = "" Then
                '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                '    Common.Insert2Ary(pQuery, strSql)
                'Else
                '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                '    Common.Insert2Ary(pQuery, strSql)
                'End If

                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & PRList(i)(1) & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & PRList(i)(0) & "' AND PRD_COY_ID='" & Common.Parse(strCoyID) & "') = 0"
                Common.Insert2Ary(pQuery, strSql)

                'Update last transacted price
                If strAPP = "APP" Then
                    strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " & _
                                            "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " & _
                                            "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE, " & _
                                            "PM_LAST_TXN_TAX = POD_GST, " & _
                                            "PM_LAST_TXN_S_COY_ID = '" & strVendor & "' " & _
                                            "WHERE POM_PO_NO = " & strNewPONo & " AND POM_B_COY_ID = POD_COY_ID " & _
                                            "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " & _
                                            "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE; "
                    Common.Insert2Ary(pQuery, strSql)
                End If

                ' Here new enhancement on PO Custom will be create from the PR Custom Field.
                ' 29/Aug/2011
                ' Create PO Custom Field from PR Custom Field
                ' PR_CUSTOM_FIELD_DETAILS table
                ' _Yap Backup this portion
                'If strItemLine = "" Then
                '    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                '        & "@num2 := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num2, @num2+1) AS d4, " _
                '        & "@num2 := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num2, 1) AS LINE_NO, " _
                '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
                '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
                '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' "
                'Else
                '    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                '        & "@num2 := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num2, @num2+1) AS d4, " _
                '        & "@num2 := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num2, 1) AS LINE_NO, " _
                '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
                '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
                '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' "
                'End If

                If strItemLine = "" Then
                    If i > 0 Then
                        If Left(PRList(i)(2), 3) = Left(PRList(i - 1)(2), 3) Then
                            strSql = " SET @num2  = IF(@PCD_PR_LINE = @num2, @num2 + 1, IF(@PCD_PR_LINE < @num2, @num2 + 1,@PCD_PR_LINE)); "
                            Common.Insert2Ary(pQuery, strSql)

                            strSql = " SET @PCD_PR_LINE = 1; "
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    End If

                    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                        & "@num2 := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num2 + 1, @num2) AS d4, " _
                        & "@num2 AS LINE_NO, " _
                        & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                        & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                Else
                    If i > 0 Then
                        If Left(PRList(i)(2), 3) = Left(PRList(i - 1)(2), 3) Then
                            strSql = " SET @num2  = IF(@PCD_PR_LINE = @num2, @num2 + 1, IF(@PCD_PR_LINE < @num2, @num2 + 1,@PCD_PR_LINE)); "
                            Common.Insert2Ary(pQuery, strSql)

                            strSql = " SET @PCD_PR_LINE = 1; "
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    End If
                    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                        & "@num2 := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num2 + 1, @num2) AS d4, " _
                        & "@num2 AS LINE_NO, " _
                        & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                        & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID & _
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                    '& "IF(@PCD_FIELD_NO=1,@num2,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

                End If

                If strVendor <> "" Then
                    strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
                End If
                If strCurr <> "" Then
                    strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
                End If
                If strAddr <> "" Then
                    strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
                End If
                strSql &= " ORDER BY PRD_PR_LINE, PCD_FIELD_NO) XXX) ZZZ;"
                Common.Insert2Ary(pQuery, strSql)

                '//for PR Consolidation
                'Dim intCnt As Integer = 0
                'If strVendor <> "" Then
                '    intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' AND PRD_S_COY_ID = '" & strVendor & "'")
                'Else
                '    intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' ")
                'End If
            Next
            'intPOLine += intCnt
            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)
            strSql = " SET @num2  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' PR_CUSTOM_FIELD_MSTR table
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (" _
            & "PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT " _
            & "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), PCM_FIELD_NO, PCM_FIELD_NAME, 'PO' " _
            & "FROM PR_MSTR PM, PR_CUSTOM_FIELD_MSTR WHERE PRM_PR_INDEX = PCM_PR_INDEX AND PRM_COY_ID='" & strCoyID & _
            "' AND PRM_PR_No='" & strPRNo & "' AND PCM_TYPE = 'PR'"
            Common.Insert2Ary(pQuery, strSql)

            If Not blnPreviewPO Then
                objUsers.Log_UserActivityNew(pQuery, WheelModule.PRMod, WheelUserActivity.AO_CreatePO, strNewPONo, strPRNo)
            End If
            objUsers = Nothing
        End Function

        '//Create PO without consolidation
        '//to be called by function ApprovePR
        Function CreatePO(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByRef pQueryLast() As String = Nothing, Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal strRate2 As Decimal = 0.0) As String
            Dim strSql, strNewPONo, strPOPrefix, strCoyID, strTermFile, strAttachIndex As String
            Dim dblTotalCost As Double
            Dim strNewPONoList As String
            Dim strVendorLast As String
            Dim strInternalRemark As String
            Dim strExternalRemark As String

            strCoyID = HttpContext.Current.Session("CompanyId")

            ' Dim dtVendor As DataTable = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID FROM PR_DETAILS WHERE PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "'")
            Dim dtVendor As DataTable

            If strItemLine = "" Then
                dtVendor = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE, (SELECT PM_LAST_TXN_PRICE_CURR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_CURR, (SELECT PM_LAST_TXN_PRICE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_PRICE,(SELECT PM_LAST_TXN_TAX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_TAX,(SELECT PM_LAST_TXN_S_COY_ID  FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_S_COY_ID FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "' GROUP BY PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE")
            Else
                dtVendor = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE, (SELECT PM_LAST_TXN_PRICE_CURR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_CURR, (SELECT PM_LAST_TXN_PRICE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_PRICE,(SELECT PM_LAST_TXN_TAX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_TAX,(SELECT PM_LAST_TXN_S_COY_ID  FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_S_COY_ID FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "' GROUP BY PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE")
            End If

            Dim intIncrementNo As Integer = 0
            Dim i As Integer

            strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(pQuery, strSql)

            Dim strVendorList As String = ""
            For Each dr As DataRow In dtVendor.Rows
                strVendor = dr("PRD_S_COY_ID")
                strVendorLast = Common.parseNull(dr("PM_LAST_TXN_S_COY_ID"))
                Dim strCURRENCY As String
                Dim strVenCURRENCY As String
                Dim strCURRENCYLast As String
                'Dim strCURRENCY = dr("PRD_CURRENCY_CODE")
                'Dim strCURRENCY = dr("PM_LAST_TXN_CURR")
                Dim strADDR = dr("PRM_B_ADDR_CODE")

                'Michelle (CR0026) - PO cost not correctly captured for multiple POs case
                'dblTotalCost = objDb.GetVal("SELECT ISNULL(PRM_PR_COST,0) FROM PR_MSTR" _
                '& " WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'")

                If strVendorList = "" Then
                    strVendorList = "'" & Common.Parse(strVendor) & "'"
                Else
                    strVendorList = strVendorList & ",'" & Common.Parse(strVendor) & "'"
                End If



                If multiPR = True Then
                    Dim strSQLPR As String
                    If strItemLine = "" Then
                        strVenCURRENCY = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'strSQLPR = "SELECT ISNULL(SUM( PRD_UNIT_COST * PRD_ORDERED_QTY + (ISNULL(PRD_GST,0) * (PRD_UNIT_COST * PRD_ORDERED_QTY))/100 ),0)  FROM PR_DETAILS, PR_MSTR " _
                        '         & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID "
                        strSQLPR = "SELECT ISNULL(SUM(ROUND(ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PRD_GST,0) * ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2))/100,2),2)),0) FROM PR_DETAILS, PR_MSTR " _
                                 & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID "
                        For i = 0 To PRList.Count - 1
                            If i = 0 Then
                                strSQLPR &= " AND ((PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            Else
                                strSQLPR &= " OR (PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            End If

                        Next
                        strSQLPR &= " ) "
                        dblTotalCost = objDb.GetVal(strSQLPR)
                    Else
                        strCURRENCYLast = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'") 'Common.parseNull(dr("PM_LAST_TXN_CURR"))
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'strSQLPR = "SELECT ISNULL(SUM( PM_LAST_TXN_PRICE * PRD_ORDERED_QTY + (ISNULL(PM_LAST_TXN_TAX,0) * (PM_LAST_TXN_PRICE * PRD_ORDERED_QTY))/100 ),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                        '         & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE"
                        strSQLPR = "SELECT ISNULL(SUM(ROUND(ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PM_LAST_TXN_TAX,0) * ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2))/100,2),2)),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                                 & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE"
                        For i = 0 To PRList.Count - 1
                            If i = 0 Then
                                strSQLPR &= " AND ((PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            Else
                                strSQLPR &= " OR (PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            End If
                        Next
                        strSQLPR &= " ) "
                        dblTotalCost = objDb.GetVal(strSQLPR)
                    End If

                    strSQLPR = "SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    For i = 0 To PRList.Count - 1
                        If i = 0 Then
                            strSQLPR &= " AND ((PRM_PR_NO ='" & PRList(i)(0) & "') "
                        Else
                            strSQLPR &= " OR (PRM_PR_NO ='" & PRList(i)(0) & "') "
                        End If
                    Next
                    strSQLPR &= " ) "
                    strInternalRemark = objDb.GetVal(strSQLPR)

                    strSQLPR = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    For i = 0 To PRList.Count - 1
                        If i = 0 Then
                            strSQLPR &= " AND ((PRM_PR_NO ='" & PRList(i)(0) & "') "
                        Else
                            strSQLPR &= " OR (PRM_PR_NO ='" & PRList(i)(0) & "') "
                        End If
                    Next
                    strSQLPR &= " ) "
                    strExternalRemark = objDb.GetVal(strSQLPR)
                Else
                    Dim strSQLPR2 As String
                    If strItemLine = "" Then
                        strVenCURRENCY = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM( PRD_UNIT_COST * PRD_ORDERED_QTY + (ISNULL(PRD_GST,0) * (PRD_UNIT_COST * PRD_ORDERED_QTY))/100 ),0)  FROM PR_DETAILS, PR_MSTR " _
                        '                & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                        dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM(ROUND(ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PRD_GST,0) * ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2))/100,2),2)),0) FROM PR_DETAILS, PR_MSTR " _
                                        & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                    Else
                        strCURRENCYLast = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM( PM_LAST_TXN_PRICE * PRD_ORDERED_QTY + (ISNULL(PM_LAST_TXN_TAX,0) * (PM_LAST_TXN_PRICE * PRD_ORDERED_QTY))/100 ),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                        '                & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                        dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM(ROUND(ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PM_LAST_TXN_TAX,0) * ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2))/100,2),2)),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                                        & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                    End If


                    strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSQLPR2 &= " AND ((PRM_PR_NO ='" & strPRNo & "')) "
                    strInternalRemark = objDb.GetVal(strSQLPR2)

                    strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSQLPR2 &= " AND ((PRM_PR_NO ='" & strPRNo & "')) "
                    strExternalRemark = objDb.GetVal(strSQLPR2)
                End If


                '' ''Dim objGlobal As New AppGlobals
                '' ''intIncrementNo += 1
                '' ''objGlobal.GetLatestDocNo("PO", pQuery, strNewPONo, strPOPrefix, , intIncrementNo)

                intIncrementNo += 1

                '' ''If strNewPONoList = "" Then
                '' ''    strNewPONoList = strNewPONo & "!" & strVendor
                '' ''Else
                '' ''    strNewPONoList &= "," & strNewPONo & "!" & strVendor
                '' ''End If

                '' ''strSql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO='" & strNewPONo & "' AND POM_B_COY_ID='" & strCoyID & "'"
                '' ''If objDb.Exist(strSql) Then
                '' ''    Return "error"
                '' ''End If

                '' ''strNewPONo = " (SELECT GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                '' ''       & " RIGHT(LEFT('10000000000000', LENGTH(cp_param_value)+1) + cp_param_value + '" & intIncrementNo & "',LENGTH(cp_param_value)) END " _
                '' ''       & " SEPARATOR '') AS cp_param_value FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO') "

                strNewPONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                strPOPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PO') "

                '' ''objGlobal = Nothing

                Dim objWheelFile As New FileManagement
                '//temporary remark by moo
                If multiPR = True Then
                    For i = 0 To PRList.Count - 1
                        strTermFile = objWheelFile.copyTermCondToPO(strNewPONo)
                        strAttachIndex = objWheelFile.copyPRAttachToPO(PRList(i)(0), strNewPONo)
                        If strAttachIndex <> "" Then
                            strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                            & "SELECT CDA_COY_ID," & strNewPONo & ",'PO',CDA_HUB_FILENAME," _
                            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                            "' AND CDA_DOC_NO='" & PRList(i)(0) & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    Next
                Else
                    strTermFile = objWheelFile.copyTermCondToPO(strNewPONo)
                    strAttachIndex = objWheelFile.copyPRAttachToPO(strPRNo, strNewPONo)
                    If strAttachIndex <> "" Then
                        strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                        & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                        & "SELECT CDA_COY_ID," & strNewPONo & ",'PO',CDA_HUB_FILENAME," _
                        & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                        "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                        Common.Insert2Ary(pQuery, strSql)
                    End If
                End If

                If strItemLine = "" Then
                    CreatePOHeader(strPRNo, strVendor, strNewPONo, strPOPrefix, pQuery, dblTotalCost, strUserID, False, strTermFile, False, "", strVenCURRENCY, strItemLine, multiPR, PRList, strAPP, strInternalRemark, strExternalRemark, strRate2)
                Else
                    CreatePOHeader(strPRNo, strVendorLast, strNewPONo, strPOPrefix, pQuery, dblTotalCost, strUserID, False, strTermFile, False, "", strCURRENCYLast, strItemLine, multiPR, PRList, "", strInternalRemark, strExternalRemark, strRate2)
                End If
                '//Pass in "strUserID" as parameter -> for consolidation
                '//Originally Last level of AO is consolidator

                If multiPR = True Then
                    CreatePODetailMulti(strPRNo, intPRIndex, strVendor, strNewPONo, pQuery, 0, strUserID, False, strCURRENCY, strADDR, strItemLine, multiPR, PRList, strAPP, blnEnterpriseVersion)
                Else
                    CreatePODetail(strPRNo, intPRIndex, strVendor, strNewPONo, pQuery, 0, strUserID, False, strCURRENCY, strADDR, strItemLine, strAPP, blnEnterpriseVersion)
                End If

                If intIncrementNo = 1 Then
                    strSql = " SET @T_NO = CONCAT(" & strNewPONo & ",'!" & strVendor & "'); "
                    Common.Insert2Ary(pQuery, strSql)
                Else
                    strSql = " SET @T_NO = CONCAT(CONCAT(@T_NO,','),CONCAT(" & strNewPONo & ",'!" & strVendor & "')); "
                    Common.Insert2Ary(pQuery, strSql)
                End If
            Next

            '' ''strSql = "UPDATE company_param SET cp_param_value = RIGHT(LEFT('10000000000000', LENGTH(cp_param_value)+1) + cp_param_value + '1', LENGTH(cp_param_value)) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "


            If strItemLine = "" Then
                strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                   "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                   "PRD_GST = PM_LAST_TXN_TAX, " & _
                   "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                   "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "' " & _
                   "AND PM_S_COY_ID = PRD_COY_ID " & _
                   "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                    "PRD_GST = PM_LAST_TXN_TAX, " & _
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " & _
                    "AND PM_S_COY_ID = PRD_COY_ID " & _
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            End If

            Dim strSqlLast As String
            If multiPR = False Then
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID IN ('" & Common.Parse(strCoyID) & "')) = 0"
                Common.Insert2Ary(pQuery, strSql)

                strSqlLast = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID IN ('" & Common.Parse(strCoyID) & "')) = 0"
                If pQueryLast Is Nothing Then
                    ReDim pQueryLast(0)
                    pQueryLast(0) = ""
                End If
                Common.Insert2Ary(pQueryLast, strSqlLast)
            End If

            strSql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(pQuery, strSql)

            Return strNewPONoList
            'objDb.BatchExecute(strAryQuery)
        End Function
    End Class
End Namespace

