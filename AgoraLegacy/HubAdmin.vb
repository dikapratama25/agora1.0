Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class HubAdmin
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))


        Public Function updateApprRules(ByVal strValue As String, Optional ByVal strPkg As String = "")
            'update value into database table company_setting (cs_flag_value)


            Dim strSql As String
            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' AND CS_FLAG_NAME = 'Approval Rule' AND CS_FLAG_TYPE ='CoyParam' AND CS_APP_PKG = '" & strPkg & "'") > 0 Then
                strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE ='" & strValue & "' "
                strSql &= "where CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
                strSql &= "AND CS_FLAG_NAME = 'Approval Rule' "
                strSql &= "AND CS_APP_PKG = '" & strPkg & "'"
            Else
                strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE, CS_APP_PKG ) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyIdToken") & "', "
                strSql &= "'Approval Rule', "
                strSql &= "'" & strValue & "', "
                strSql &= "'CoyParam',"
                strSql &= "'" & strPkg & "')"
            End If
            objDb.Execute(strSql)
        End Function

        Public Function updateInvApprRules(ByVal strValue As String, Optional ByVal strPkg As String = "")
            'update value into database table company_setting (cs_flag_value)


            Dim strSql As String
            If objDb.Exist("SELECT '*' FROM COMPANY_SETTING WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' AND CS_FLAG_NAME = 'Invoice Approval Rule' AND CS_FLAG_TYPE ='CoyParam' AND CS_APP_PKG = '" & strPkg & "'") > 0 Then
                strSql = "UPDATE COMPANY_SETTING SET CS_FLAG_VALUE ='" & strValue & "' "
                strSql &= "where CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
                strSql &= "AND CS_FLAG_NAME = 'Invoice Approval Rule' "
                strSql &= "AND CS_APP_PKG = '" & strPkg & "'"
            Else
                strSql = "INSERT INTO COMPANY_SETTING(CS_COY_ID, CS_FLAG_NAME, CS_FLAG_VALUE, CS_FLAG_TYPE, CS_APP_PKG ) VALUES ("
                strSql &= "'" & HttpContext.Current.Session("CompanyIdToken") & "', "
                strSql &= "'Invoice Approval Rule', "
                strSql &= "'" & strValue & "', "
                strSql &= "'CoyParam',"
                strSql &= "'" & strPkg & "')"
            End If
            objDb.Execute(strSql)
        End Function

        Public Function getApprRules(Optional ByVal strPkg As String = "") As DataSet
            'get default value for last selected value



            Dim strGet As String
            Dim dsApprRules As New DataSet

            strGet = "SELECT CS_FLAG_VALUE FROM COMPANY_SETTING "
            strGet &= "where CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
            strGet &= "And CS_FLAG_NAME = 'Approval Rule' "

            If strPkg <> "" Then
                strGet &= "AND CS_APP_PKG = '" & strPkg & "' "
            End If

            dsApprRules = objDb.FillDs(strGet)
            getApprRules = dsApprRules
        End Function

        Public Function getInvApprRules(Optional ByVal strPkg As String = "") As DataSet
            'get default value for last selected invoice approval rules value

            Dim strGet As String
            Dim dsInvApprRules As New DataSet

            strGet = "SELECT CS_FLAG_VALUE FROM COMPANY_SETTING "
            strGet &= "where CS_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
            strGet &= "And CS_FLAG_NAME = 'Invoice Approval Rule' "

            If strPkg <> "" Then
                strGet &= "AND CS_APP_PKG = '" & strPkg & "' "
            End If

            dsInvApprRules = objDb.FillDs(strGet)
            getInvApprRules = dsInvApprRules
        End Function

        Public Function getVendorMap() As DataSet
            Dim strsql As String
            Dim dsVendorMap As DataSet

            'strsql = "Select CM_COY_ID, CM_COY_NAME, VM_VENDOR_MAPPING "
            'strsql &= "from COMPANY_MSTR left join VENDOR_MAPPING "
            'strsql &= "on CM_COY_ID = VM_S_COY_ID "
            'strsql &= "where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strsql &= "And CM_COY_TYPE = 'Vendor'"
            strsql = "select CM_COY_ID, CM_COY_NAME, VM_VENDOR_MAPPING "
            strsql &= "from company_mstr left join VENDOR_MAPPING ON "
            strsql &= "CM_COY_ID = VM_S_COY_ID and VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
            strsql &= "WHERE CM_COY_TYPE = 'VENDOR' and CM_DELETED <> 'Y' "
            strsql &= "and CM_COY_ID <> '" & HttpContext.Current.Session("CompanyIdToken") & "' "



            '"select v.*,CM_COY_ID,m.CM_COY_NAME, m.CM_BUSINESS_REG_NO " & _
            '    " from Company_Vendor v, Company_Mstr m " & _
            '    " where v.CV_S_COY_ID=m.CM_COY_ID and v.CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"

            dsVendorMap = objDb.FillDs(strsql)
            getVendorMap = dsVendorMap


        End Function

        Public Function updateVendorCode(ByVal strCode As String, ByVal strName As String) As String

            Dim strSql As String

            strSql = "Delete from VENDOR_MAPPING "
            strSql &= "WHERE VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
            strSql &= "AND VM_S_COY_ID = '" & Common.Parse(strName) & "'; "
            strSql &= "insert into VENDOR_MAPPING Values ("
            strSql &= "'" & HttpContext.Current.Session("CompanyIdToken") & "', "
            strSql &= "'" & Common.Parse(strName) & "', "
            strSql &= "'" & Common.Parse(strCode) & "') "

            updateVendorCode = strSql

            'If objDb.Exist("Select * From VENDOR_MAPPING Where VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND VM_VENDOR_MAPPING = '" & Common.Parse(strCode) & "'") > 0 Then
            '    updateVendorCode = WheelMsgNum.Duplicate
            '    Exit Function

            'Else
            '    If objDb.Exist("Select * From VENDOR_MAPPING Where VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND VM_S_COY_ID = '" & Common.Parse(strName) & "'") > 0 Then
            '        strSql = "UPDATE VENDOR_MAPPING set "
            '        strSql &= "VM_VENDOR_MAPPING = '" & Common.Parse(strCode) & "' "
            '        strSql &= "WHERE VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '        strSql &= "AND VM_S_COY_ID = '" & Common.Parse(strName) & "' "

            '    Else
            '        strSql = "insert into VENDOR_MAPPING Values ("
            '        strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '        strSql &= "'" & Common.Parse(strCode) & "', "
            '        strSql &= "'" & Common.Parse(strName) & "') "
            '    End If
            'End If

            'If objDb.Exist("Select * From VENDOR_MAPPING Where VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND VM_S_COY_ID = '" & Common.Parse(strName) & "'") > 0 Then
            '    strSql = "UPDATE VENDOR_MAPPING set "
            '    strSql &= "VM_VENDOR_MAPPING = '" & Common.Parse(strCode) & "' "
            '    strSql &= "WHERE VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '    strSql &= "AND VM_S_COY_ID = '" & Common.Parse(strName) & "' "

            'Else
            '    strSql = "insert into VENDOR_MAPPING Values ("
            '    strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '    strSql &= "'" & Common.Parse(strName) & "', "
            '    strSql &= "'" & Common.Parse(strCode) & "') "
            'End If


            'If objDb.Execute(strSql) Then
            '    updateVendorCode = WheelMsgNum.Save
            'Else
            '    updateVendorCode = WheelMsgNum.NotSave
            'End If




        End Function



        'If objDb.Exist("Select * From COMPANY_B_ITEM_CODE Where CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBC_PRODUCT_CODE = '" & Common.Parse(strPCode) & "'") > 0 Then
        '    strSql = "UPDATE COMPANY_B_ITEM_CODE "
        '    strSql &= "SET CBC_B_ITEM_CODE ='" & Common.Parse(strBCode) & "' "
        '    strSql &= "where CBC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
        '    strSql &= "and CBC_PRODUCT_CODE = '" & Common.Parse(strPCode) & "';"
        'Else
        '    strSql = "insert into COMPANY_B_ITEM_CODE VALUES ("
        '    strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
        '    strSql &= "'" & Common.Parse(strPCode) & "', "
        '    strSql &= "'" & Common.Parse(strBCode) & "'); "

        '    objDb.Execute(strSql)

        'End If

        'updateItemCode = strSql

        'Public Function delVendorMap(ByVal strname As String)

        '    Dim strSQLdel As String

        '    'strSQLdel = "Delete BUYER_CATALOGUE_USER where BCU_CAT_INDEX='" & strCatName & "'"
        '    strSQLdel = "Delete VENDOR_MAPPING "
        '    strSQLdel &= "WHERE VM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strSQLdel &= "AND VM_S_COY_ID = '" & Common.Parse(strname) & "' "
        'End Function

        ''*************************************************************************************
        'Created By:  Ya Li
        'Date:  24/05/2005
        'Screen:  View Public Vendor Registration Approval 
        'Purpose:  to view the public vendor registration

        '**************************************************************************************

        Function getVendorRegAppr(ByVal comID As String, ByVal comName As String, ByVal status As String) As DataSet

            Dim objDBeRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
            Dim ds As New DataSet
            Dim objComp As New Companies
            Dim strsql As String
            Dim statusTemp As String
            Dim PENDING As Integer = VendorRegApprStatus.Pending
            Dim APPROVED As Integer = VendorRegApprStatus.Approved
            Dim REJECT As Integer = VendorRegApprStatus.Reject
            Dim sCompType As String = objComp.GetCompanyType
            statusTemp = "(" & PENDING & "," & APPROVED & "," & REJECT & ")"
            If sCompType = "HUB" Then
                strsql = "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_REG_DATE, STATUS_DESC " & _
                "FROM COMPANY_MSTR_TEMP, STATUS_MSTR " & _
                "WHERE CM_COY_TYPE='VENDOR' " & _
                "AND STATUS_TYPE='RFPREG' AND CM_HUB_APPR_STATUS=STATUS_NO "
            ElseIf sCompType = "BUYER" Then
                strsql = "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_REG_DATE, STATUS_DESC " & _
                "FROM COMPANY_MSTR_TEMP, STATUS_MSTR " & _
                "WHERE CM_COY_TYPE='VENDOR' " & _
                "AND STATUS_TYPE='RFPREG' AND CM_BUYER_APPR_STATUS=STATUS_NO "
                '-------------------New Code added for ScompType="Vendor" by Praveen on 27/07/2007
            ElseIf sCompType = "VENDOR" Then
                strsql = "SELECT CM_COY_ID, CM_COY_NAME, CM_COY_TYPE, CM_REG_DATE, STATUS_DESC " & _
                "FROM COMPANY_MSTR_TEMP, STATUS_MSTR " & _
                "WHERE CM_COY_TYPE='VENDOR' " & _
                "AND STATUS_TYPE='RFPREG' AND CM_BUYER_APPR_STATUS=STATUS_NO "
                '--------------------End The Code ----
            End If

            If comID <> "" Then
                '------Here Gave Single Quotes to CM_COY_ID by praveen on 27/07/2007
                strsql = strsql & " AND CM_COY_ID " & Common.ParseSQL(comID) & " "
                '-----------------end 
            End If

            If comName <> "" Then
                '------Here Gave Single Quotes to CM_COY_ID by praveen on 27/07/2007
                strsql = strsql & " AND CM_COY_NAME " & Common.ParseSQL(comName) & " "
                '----End the code 
            End If

            If status <> "" Then
                strsql = strsql & " AND STATUS_NO IN " & status
            Else
                strsql = strsql & " AND STATUS_NO IN " & statusTemp
            End If
            ds = objDBeRFP.FillDs(strsql)
            objDBeRFP = Nothing

            Return ds

        End Function


        Public Function getLocMapping(ByVal AddrCode As String) As DataSet
            Dim strsql As String
            Dim dsAddrCode As DataSet

            strsql = "select LM_ADDR_CODE, LM_ACCT_CODE, LM_ADDR_MAPPING, LM_LOC_INDEX from LOC_MAPPING "
            strsql &= "WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "' "

            If AddrCode <> "" Then
                strsql &= " and LM_ADDR_CODE = '" & AddrCode & "'"
            End If

            dsAddrCode = objDb.FillDs(strsql)
            getLocMapping = dsAddrCode
        End Function
        Public Function saveLocMapping(ByVal Action As String, ByVal AddrCode As String, ByVal AcctCode As String, ByVal LocCode As String, Optional ByVal LocIndex As Integer = 0)
            Dim strsql As String
            Dim dsAddrCode As DataSet

            If Action = "M" Then
                strsql = "UPDATE LOC_MAPPING SET LM_ADDR_CODE ='" & AddrCode & "', "
                strsql &= "LM_ACCT_CODE = '" & AcctCode & "', "
                strsql &= "LM_ADDR_MAPPING ='" & LocCode & "' "
                strsql &= "where LM_LOC_INDEX = " & LocIndex
            ElseIf Action = "D" Then
                strsql = "Delete FROM LOC_MAPPING where LM_LOC_INDEX = " & LocIndex
            Else
                strsql = "INSERT INTO LOC_MAPPING(LM_COY_ID, LM_ADDR_CODE, LM_ACCT_CODE, LM_ADDR_MAPPING) VALUES ("
                strsql &= "'" & HttpContext.Current.Session("CompanyIdToken") & "', '" & AddrCode & "', '" & AcctCode & "', '" & LocCode & "')"
            End If
            objDb.Execute(strsql)

        End Function

        Function chkLocDuplicate(ByVal AddrCode As String, ByVal AcctCode As String, ByVal LocCode As String, Optional ByVal strMode As Char = "A", Optional ByVal intLocIndex As Integer = 0) As String
            Dim strSQL As String

            strSQL = "Select * FROM LOC_MAPPING where LM_COY_ID='" & HttpContext.Current.Session("CompanyIdToken") & "' "
            strSQL &= "and LM_ADDR_CODE = '" & AddrCode & "' "
            strSQL &= "and LM_ACCT_CODE = '" & AcctCode & "' "
            If strMode = "M" Then
                strSQL &= "and LM_LOC_INDEX <> " & intLocIndex
            End If

            If objDb.Exist(strSQL) > 0 Then
                chkLocDuplicate = 1
            Else
                chkLocDuplicate = 0
            End If

        End Function
    End Class

End Namespace
