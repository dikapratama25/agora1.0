Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class Products_Ext
        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))

        Public Function Download_ProductExcel_Common(ByVal CompID As String, ByVal strStkType As String) As DataSet
            Dim dload As New DataSet
            Dim strSQL As String

            strSQL = "SELECT P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_DESC, " & _
                    "CASE P.PM_ITEM_TYPE WHEN 'SP' THEN 'Spot (Non-Inventoried item)' " & _
                    "WHEN 'ST' THEN 'Stock (Direct material - Inventoried item)' " & _
                    "WHEN 'MI' THEN 'MRO, M&E and IT (Inventoried item)' END AS PM_ITEM_TYPE, " & _
                    "IFNULL(P.PM_LONG_DESC,'') AS PM_LONG_DESC, " & _
                    "(SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = P.PM_CATEGORY_NAME) AS PM_CATEGORY_NAME, " & _
                    "P.PM_UOM, " & _
                    "IFNULL((SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = P.PM_CAT_CODE AND CBC_B_COY_ID = P.PM_S_COY_ID),'') AS PM_ACCT_CODE, " & _
                    "P.PM_REF_NO, " & _
                    "CASE P.PM_IQC_IND WHEN 'Y' THEN 'Yes' " & _
                    "WHEN 'N' THEN 'No' END AS PM_IQC_IND, " & _
                    "CASE P.PM_PARTIAL_CD WHEN 'Y' THEN 'Yes' " & _
                    "WHEN 'N' THEN 'No' END AS PM_PARTIAL_CD, " & _
                    "IFNULL((SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = P.PM_ACCT_CODE AND CBG_B_COY_ID = P.PM_S_COY_ID),'') AS PM_ACCT_CODE, " & _
                    "P.PM_ORD_MIN_QTY AS PM_ORD_MIN_QTY, " & _
                    "P.PM_ORD_MAX_QTY AS PM_ORD_MAX_QTY, P.PM_SAFE_QTY AS PM_SAFE_QTY, " & _
                    "P.PM_MAX_INV_QTY AS PM_MAX_INV_QTY, P.PM_REORDER_QTY AS PM_REORDER_QTY, " & _
                    "P.PM_BUDGET_PRICE AS PM_BUDGET_PRICE, P.PM_IQC_TYPE AS PM_IQC_TYPE, " & _
                    "P.PM_EOQ AS PM_EOQ, P.PM_RATIO AS PM_RATIO, " & _
                    "CASE P.PM_OVERSEA WHEN 'Y' THEN 'Yes' " & _
                    "WHEN 'N' THEN 'No' END AS PM_OVERSEA, " & _
                    "IFNULL(P.PM_PRODUCT_BRAND,'') AS PM_PRODUCT_BRAND, " & _
                    "IFNULL(P.PM_DRAW_NO,'') AS PM_DRAW_NO, IFNULL(P.PM_PRODUCT_MODEL,'') AS PM_PRODUCT_MODEL, " & _
                    "IFNULL(P.PM_GROSS_WEIGHT,'') AS PM_GROSS_WEIGHT, IFNULL(P.PM_NET_WEIGHT,'') AS PM_NET_WEIGHT, " & _
                    "IFNULL(P.PM_LENGHT,'') AS PM_LENGHT, IFNULL(P.PM_VERS_NO,'') AS PM_VERS_NO, " & _
                    "IFNULL(P.PM_WIDTH,'') AS PM_WIDTH, IFNULL(P.PM_COLOR_INFO,'') AS PM_COLOR_INFO, " & _
                    "IFNULL(P.PM_VOLUME,'') AS PM_VOLUME, IFNULL(P.PM_HSC_CODE,'') AS PM_HSC_CODE, " & _
                    "IFNULL(P.PM_HEIGHT,'') AS PM_HEIGHT, IFNULL(P.PM_SPEC1,'') AS PM_SPEC1, " & _
                    "IFNULL(P.PM_SPEC2,'') AS PM_SPEC2, IFNULL(P.PM_SPEC3,'') AS PM_SPEC3, " & _
                    "IFNULL(CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')'),'') AS PACKING_TYPE, P.PM_PACKING_QTY AS PM_PACKING_QTY, " & _
                    "IFNULL(P.PM_MANUFACTURER,'') AS PM_MANUFACTURER, IFNULL(P.PM_MANUFACTURER2,'') AS PM_MANUFACTURER2, " & _
                    "IFNULL(P.PM_MANUFACTURER3,'') AS PM_MANUFACTURER3, IFNULL(P.PM_SECTION,'') AS PM_SECTION, " & _
                    "IFNULL(P.PM_LOCATION,'') AS PM_LOCATION, IFNULL(P.PM_NEW_ITEM_CODE,'') AS PM_NEW_ITEM_CODE, " & _
                    "IFNULL(P.PM_REMARKS,'') AS PM_REMARKS " & _
                    "FROM PRODUCT_MSTR P " & _
                    "LEFT JOIN COMPANY_PACKING_TYPE ON CPT_COY_ID= P.PM_S_COY_ID AND CPT_PACK_CODE= P.PM_PACKING_TYPE " & _
                    "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' " & _
                    "AND P.PM_DELETED <> 'Y' "

            'If strStkType <> "" Then
            '    strSQL = strSQL & "AND P.PM_ITEM_TYPE IN " & strStkType
            'End If

            dload = objDb.FillDs(strSQL)
            Download_ProductExcel_Common = dload
        End Function

        Public Function Download_ProductExcel_Common2(ByVal CompID As String, ByVal strStkType As String) As DataSet
            Dim dload As New DataSet
            Dim strSQL As String

            strSQL = "SELECT PV_VENDOR_ITEM_CODE, PV_S_COY_NAME, PV_SUPP_CODE, DELIVERY_TERM, " & _
                    "CAST(UNIT_PRICE AS CHAR(1000)) AS UNIT_PRICE, " & _
                    "PV_PAYMENT_CODE, PV_CURR, PV_PUR_SPEC_NO, PV_REVISION, PV_LEAD_TIME, PV_VENDOR_CODE " & _
                    "FROM (SELECT PM_PRODUCT_CODE AS PV_PRODUCT_CODE, PM_VENDOR_ITEM_CODE AS PV_VENDOR_ITEM_CODE, CASE PV_VENDOR_TYPE WHEN 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE_NO, " & _
                    "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PV_S_COY_ID) AS PV_S_COY_NAME, " & _
                    "CAST((CASE PV_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' ELSE CAST(PV_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PV_S_COY_ID_TAX_ID, " & _
                    "PV_LEAD_TIME, PV_VENDOR_CODE, IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') AS DELIVERY_TERM, " & _
                    "(SELECT GROUP_CONCAT(PVP_VOLUME, ':', PVP_VOLUME_PRICE) FROM PRODUCT_VOLUME_PRICE WHERE PVP_PRODUCT_CODE = PM_PRODUCT_CODE AND PVP_VENDOR_TYPE = PV_VENDOR_TYPE) AS UNIT_PRICE, " & _
                    "PV_SUPP_CODE, PV_VENDOR_TYPE, " & _
                    "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_PAYMENT_CODE AND CODE_CATEGORY = 'PT'),'') AS PV_PAYMENT_CODE, " & _
                    "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_CURR),'') AS PV_CURR, " & _
                    "PV_PUR_SPEC_NO, PV_REVISION FROM PIM_VENDOR " & _
                    "INNER JOIN PRODUCT_MSTR ON PV_PRODUCT_INDEX = PM_PRODUCT_INDEX " & _
                    "LEFT JOIN COMPANY_DELIVERY_TERM ON CDT_COY_ID = PM_S_COY_ID AND CDT_DEL_CODE = PV_DELIVERY_TERM " & _
                    "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' AND PM_DELETED <> 'Y' AND PV_S_COY_ID IS NOT NULL "

            'strSQL = "SELECT PV_VENDOR_ITEM_CODE, PV_S_COY_NAME, PV_SUPP_CODE, DELIVERY_TERM, " & _
            '        "CAST(UNIT_PRICE AS CHAR(1000)) AS UNIT_PRICE, " & _
            '        "PV_PAYMENT_CODE, PV_CURR, PV_PUR_SPEC_NO, PV_REVISION, PV_S_COY_ID_TAX_ID, PV_LEAD_TIME, PV_VENDOR_CODE " & _
            '        "FROM (SELECT PM_PRODUCT_CODE AS PV_PRODUCT_CODE, PM_VENDOR_ITEM_CODE AS PV_VENDOR_ITEM_CODE, CASE PV_VENDOR_TYPE WHEN 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE_NO, " & _
            '        "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PV_S_COY_ID) AS PV_S_COY_NAME, " & _
            '        "CAST((CASE PV_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' ELSE CAST(PV_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PV_S_COY_ID_TAX_ID, " & _
            '        "PV_LEAD_TIME, PV_VENDOR_CODE, IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') AS DELIVERY_TERM, " & _
            '        "(SELECT GROUP_CONCAT(PVP_VOLUME, ':', PVP_VOLUME_PRICE) FROM PRODUCT_VOLUME_PRICE WHERE PVP_PRODUCT_CODE = PM_PRODUCT_CODE AND PVP_VENDOR_TYPE = PV_VENDOR_TYPE) AS UNIT_PRICE, " & _
            '        "PV_SUPP_CODE, PV_VENDOR_TYPE, " & _
            '        "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_PAYMENT_CODE AND CODE_CATEGORY = 'PT'),'') AS PV_PAYMENT_CODE, " & _
            '        "IFNULL((SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_ABBR = PV_CURR),'') AS PV_CURR, " & _
            '        "PV_PUR_SPEC_NO, PV_REVISION FROM PIM_VENDOR " & _
            '        "INNER JOIN PRODUCT_MSTR ON PV_PRODUCT_INDEX = PM_PRODUCT_INDEX " & _
            '        "LEFT JOIN COMPANY_DELIVERY_TERM ON CDT_COY_ID = PM_S_COY_ID AND CDT_DEL_CODE = PV_DELIVERY_TERM " & _
            '        "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' AND PM_DELETED <> 'Y' AND PV_S_COY_ID IS NOT NULL "

            'If strStkType <> "" Then
            '    strSQL = strSQL & "AND PM_ITEM_TYPE IN " & strStkType
            'End If

            strSQL = strSQL & ") AS zzz ORDER BY PV_VENDOR_ITEM_CODE, CAST(PV_VENDOR_TYPE_NO AS UNSIGNED) "

            dload = objDb.FillDs(strSQL)
            Download_ProductExcel_Common2 = dload

        End Function

        Public Function Download_BudgetPriceExcel_Common(ByVal CompID As String, ByVal UserID As String) As DataSet

            Dim dload As New DataSet
            Dim strSQL, strTemp As String

            'strSQL = "SELECT IFNULL(UM_STK_TYPE_STOCK,'') FROM USER_MSTR WHERE UM_USER_ID = '" & UserID & "' AND UM_COY_ID = '" & CompID & "'"
            'strTemp = objDb.GetVal(strSQL)

            strSQL = "SELECT PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, CONCAT(IFNULL(PM_SPEC1,''), ' ', IFNULL(PM_SPEC2,''), ' ' ,IFNULL(PM_SPEC3,'')) AS PM_SPEC, PM_UOM, PM_BUDGET_PRICE " & _
                    "FROM PRODUCT_MSTR WHERE PM_DELETED = 'N' AND PM_S_COY_ID = '" & Common.Parse(CompID) & "' AND PM_ITEM_TYPE = 'ST' "

            'If strTemp = "Y" Then
            '    strSQL &= "AND PM_ITEM_TYPE = 'ST' "
            'Else
            '    strSQL &= "AND PM_ITEM_TYPE = '' "
            'End If

            dload = objDb.FillDs(strSQL)
            Download_BudgetPriceExcel_Common = dload
        End Function

        Function search_Catalogue(ByVal strSearchCatType As String, ByVal strSearchType As String, ByVal strSearchVal() As String, ByVal strDesc As String, ByVal strCommodity As String, Optional ByVal strSearchMode As String = "normal", Optional ByVal strSelProdCode As String = "") As DataSet
            Dim strSqlVendor, strSqlBuyer, strCondition, strsql, strRole As String
            Dim dsProduct As DataSet
            Dim strCoyID As String = HttpContext.Current.Session("CompanyId")
            Dim strTemp As String
            Dim objUser As New Users
            Dim objUser_Ext As New Users_Ext
            Dim i As Integer

            If objUser.IsPurchasing Then
                strRole = "PM"
            Else
                strRole = "BUYER"
            End If
            objUser = Nothing

            Dim dsStk As New DataSet

            dsStk = objUser_Ext.GetUserStockType(HttpContext.Current.Session("UserId"), strCoyID)

            If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Or dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Or dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
                strTemp = ""

                strsql = " AND PM_ITEM_TYPE IN ("

                If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Then
                    strTemp = "'SP'"
                End If

                If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Then
                    If strTemp <> "" Then
                        strTemp &= ",'ST'"
                    Else
                        strTemp &= "'ST'"
                    End If
                End If

                If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
                    If strTemp <> "" Then
                        strTemp &= ",'MI'"
                    Else
                        strTemp &= "'MI'"
                    End If
                End If

                strsql = strsql & strTemp & ")"
            Else
                strsql = ""
            End If


            strSqlBuyer = "SELECT distinct PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                                & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM, COMMODITY_TYPE.CT_NAME, CM_COY_ID, '' as CM_COY_NAME, PM.PM_PRODUCT_FOR, PM.PM_ITEM_TYPE " _
                                & "FROM BUYER_CATALOGUE_USER BCU INNER JOIN BUYER_CATALOGUE_ITEMS BCI ON BCU.BCU_CAT_INDEX = BCI.BCU_CAT_INDEX " _
                                & "INNER JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = BCI.BCU_PRODUCT_CODE " _
                                & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PM.PM_S_COY_ID " _
                                & "LEFT OUTER JOIN COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE " _
                                & "BCU.BCU_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND PM.PM_DELETED <> 'Y' AND PM.PM_PRODUCT_FOR = 'B' AND BCI.BCU_S_COY_ID = '" & strCoyID & "' " _
                                & "AND NOT PM.PM_PRODUCT_CODE IN (" & strSelProdCode & ")" & strsql


            strSqlVendor = "SELECT PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                                 & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM, COMMODITY_TYPE.CT_NAME, CM_COY_ID, CM_COY_NAME, PM.PM_PRODUCT_FOR, '' AS PM_ITEM_TYPE FROM PRODUCT_MSTR PM " _
                                 & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PM.PM_S_COY_ID " _
                                 & "LEFT OUTER JOIN COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE " _
                                 & "PM.PM_DELETED <> 'Y' AND PM.PM_PRODUCT_FOR <> 'B' " _
                                 & "AND NOT PM.PM_PRODUCT_CODE IN (" & strSelProdCode & ") " _
                                 & "AND PM_S_COY_ID IN (SELECT CV_S_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "')"

            If strSearchCatType = "V" Then
                strsql = strSqlVendor
            ElseIf strSearchCatType = "B" Then
                strsql = strSqlBuyer
            Else
                strsql = strSqlVendor & " UNION " & strSqlBuyer
            End If

            strCondition = BuildSearchCondition(strSearchMode, strSearchVal, strSearchType, strRole)

            If strCommodity <> "" Then
                strCommodity = " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommodity) & "' "
            Else
                strCommodity = ""
            End If

            If strDesc <> "" Then
                strTemp = Common.BuildWildCard(strDesc)
                strDesc = " AND PM_PRODUCT_Desc" & Common.ParseSQL(strTemp)
            Else
                strDesc = ""
            End If

            strsql = strsql & strCondition & strDesc & strCommodity

            dsProduct = objDb.FillDs(strsql)
            Return dsProduct
        End Function

        Private Function BuildSearchCondition(ByVal strSearchMode As String, ByVal strSearchVal() As String, ByVal strSearchType As String, Optional ByVal strUserRole As String = "PM") As String
            Dim strCondition As String = ""
            Dim strTemp As String
            If strSearchMode = "normal" Then
                If strSearchVal(0) <> String.Empty Then
                    If strSearchType = "VE" And strUserRole <> "PM" Then
                        strTemp = strSearchVal(0)
                    Else
                        strTemp = BuildWildCard(strSearchVal(0))
                    End If

                    Select Case strSearchType
                        Case "PI" '//Product Id
                            strCondition = " AND PM_PRODUCT_CODE" & Common.ParseSQL(strTemp)
                        Case "PD" '//Product Desc
                            strCondition = " AND PM_PRODUCT_DESC" & Common.ParseSQL(strTemp)
                        Case "PC" '//Product Category
                            strCondition = " AND CM_CATEGORY_DESC" & Common.ParseSQL(strTemp)
                        Case "PB" '//Brand
                            strCondition = " AND PM_PRODUCT_BRAND" & Common.ParseSQL(strTemp)
                        Case "PM" '//Model
                            strCondition = " AND PM_PRODUCT_MODEL" & Common.ParseSQL(strTemp)
                        Case "VE" '//Vendor
                            If strUserRole = "PM" Then
                                strCondition = " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                            Else
                                '//from dropdownlist
                                strCondition = " AND CM_COY_ID" & Common.ParseSQL(strTemp)
                            End If
                        Case "VI" '//Vendor Item Code
                            strCondition = " AND PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strTemp)
                        Case "BI" '//Buyer Item Code
                            'strSqlBIC = "SELECT CBC_PRODUCT_CODE FROM COMPANY_B_ITEM_CODE WHERE CBC_B_COY_ID='" & strCoyID & "' AND CBC_B_ITEM_CODE" & Common.ParseSQL(strSearchVal)
                            'strCondition1 = " AND PM_PRODUCT_CODE IN (" & strSqlBIC & ")"
                            strCondition = " AND CBC_B_ITEM_CODE" & Common.ParseSQL(strTemp)
                    End Select
                End If
            Else
                If strSearchVal(0) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(0))
                    strCondition = strCondition & " AND PM_PRODUCT_CODE" & Common.ParseSQL(strTemp)
                End If
                '//Product Desc
                If strSearchVal(1) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(1))
                    strCondition = strCondition & " AND PM_PRODUCT_DESC" & Common.ParseSQL(strTemp)
                End If
                '//Product Category
                'If strSearchVal(2) <> String.Empty Then
                'strSearchVal(2) = BuildWildCard(strSearchVal(2))
                'strCondition = strCondition & " AND CM_CATEGORY_DESC" & Common.ParseSQL(strSearchVal(2))
                'End If
                '//Brand
                If strSearchVal(3) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(3))
                    strCondition = strCondition & " AND PM_PRODUCT_BRAND" & Common.ParseSQL(strTemp)
                End If
                '//Model
                If strSearchVal(4) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(4))
                    strCondition = strCondition & " AND PM_PRODUCT_MODEL" & Common.ParseSQL(strTemp)
                End If
                '//Vendor
                If strSearchVal(5) <> String.Empty Then
                    If strUserRole = "PM" Then
                        strTemp = BuildWildCard(strSearchVal(5))
                    Else
                        strTemp = strSearchVal(5)
                    End If
                    If strUserRole = "PM" Then
                        strCondition = strCondition & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                    Else
                        '//from dropdownlist
                        strCondition = strCondition & " AND CM_COY_ID" & Common.ParseSQL(strTemp)
                    End If
                End If

                '//Vendor Item Code
                If strSearchVal(6) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(6))
                    strCondition = strCondition & " AND PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strTemp)
                End If
                '//Buyer Item Code
                If strSearchVal(7) <> String.Empty Then
                    strTemp = BuildWildCard(strSearchVal(7))
                    strCondition = strCondition & " AND CBC_B_ITEM_CODE" & Common.ParseSQL(strTemp)
                End If
            End If
            Return strCondition
        End Function

        Function BuildWildCard(ByVal strSearchVal As String) As String
            Dim strTemp As String
            If InStr(strSearchVal, "*") <= 0 And InStr(strSearchVal, "?") <= 0 Then
                strTemp = "*" & strSearchVal & "*"
            Else
                strTemp = strSearchVal
            End If
            Return strTemp
        End Function

        Function GetItemInfoToExcel(ByVal strField As String, Optional ByVal strCompanyId As String = "") As DataSet
            Dim strSql, strCompId As String
            Dim ds As New DataSet

            If strCompanyId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            Else
                strCompId = strCompanyId
            End If

            If strField = "DT" Then 'Delivery Term
                strSql = "SELECT CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')') AS 'Delivery Term' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & strCompId & "' AND CDT_DELETED = 'N' ORDER BY CDT_DEL_CODE, CDT_DEL_NAME"
            ElseIf strField = "PT" Then 'Packing Type
                strSql = "SELECT CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')') AS 'Packing Type' FROM COMPANY_PACKING_TYPE WHERE CPT_COY_ID = '" & strCompId & "' AND CPT_DELETED = 'N' ORDER BY CPT_PACK_CODE, CPT_PACK_NAME"
            ElseIf strField = "CM" Then 'Commodity Type
                strSql = "SELECT CT_NAME AS 'Commodity Type' FROM COMMODITY_TYPE WHERE LEFT(CT_NAME, 3) = 'SEH' ORDER BY CT_NAME"
            Else
                strSql = ""
            End If

            ds = objDb.FillDs(strSql)
            GetItemInfoToExcel = ds
        End Function

    End Class
End Namespace

