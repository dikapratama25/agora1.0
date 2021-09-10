Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class DeliveryOrder_Ext
        Dim objDb As New EAD.DBCom

        Dim objGlobal As New AppGlobals
        Public Function GetDOFromVendor(ByVal strDoNo As String, ByVal strStartDt As String, ByVal strEndDt As String) As DataSet
            Dim dsDO As DataSet
            Dim strSqlDO As String
            Dim strTemp As String

            'strSqlDO = "SELECT DISTINCT DOM_DO_INDEX, DOM_DO_NO, POM_PO_INDEX, POM_S_COY_NAME, DOM_S_REF_NO, DOM_S_COY_ID, " & _
            '        "POM_PO_NO, DOM_CREATED_DATE, DOM_DO_DATE, DOM_DO_STATUS, DOM_S_REF_DATE " & _
            '        "FROM DO_MSTR " & _
            '        "INNER JOIN DO_DETAILS ON DOM_DO_NO = DOD_DO_NO AND DOM_S_COY_ID = DOD_S_COY_ID " & _
            '        "INNER JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX " & _
            '        "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND DOD_PO_LINE = POD_PO_LINE " & _
            '        "INNER JOIN PRODUCT_MSTR ON POD_COY_ID = PM_S_COY_ID AND POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
            '        "WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_ITEM_TYPE = 'ST' AND PM_IQC_IND = 'Y' " & _
            '        "AND (DOM_DO_STATUS = '2' OR DOM_DO_STATUS = '3' OR DOM_DO_STATUS = '4' OR DOM_DO_STATUS = '6') "

            strSqlDO = "SELECT DISTINCT DOM_DO_INDEX, DOM_DO_NO, POM_PO_INDEX, POM_S_COY_NAME, DOM_S_REF_NO, DOM_S_COY_ID, POM_PO_NO, " &
                    "DOM_CREATED_DATE, DOM_DO_DATE, DOM_DO_STATUS, DOM_S_REF_DATE FROM PO_DETAILS " &
                    "INNER JOIN PRODUCT_MSTR ON POD_COY_ID = PM_S_COY_ID AND POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE " &
                    "INNER JOIN PO_MSTR ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                    "INNER JOIN DO_MSTR ON POM_PO_INDEX = DOM_PO_INDEX " &
                    "WHERE POD_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND POD_ITEM_TYPE = 'ST' " &
                    "AND PM_IQC_IND = 'Y' " &
                    "AND (DOM_DO_STATUS = '2' OR DOM_DO_STATUS = '3' OR DOM_DO_STATUS = '4' OR DOM_DO_STATUS = '6') "

            If strDoNo <> "" Then
                strTemp = Common.BuildWildCard(strDoNo)
                strSqlDO = strSqlDO & " AND DOM_DO_NO" & Common.ParseSQL(strTemp)
            End If

            If strStartDt <> "" Then
                strSqlDO = strSqlDO & " AND DOM_DO_DATE >= " & Common.ConvertDate(strStartDt)
            End If

            If strEndDt <> "" Then
                strSqlDO = strSqlDO & " AND DOM_DO_DATE <= " & Common.ConvertDate(strEndDt & " 23:59:59.000")
            End If

            dsDO = objDb.FillDs(strSqlDO)
            Return dsDO
        End Function
    End Class
End Namespace
