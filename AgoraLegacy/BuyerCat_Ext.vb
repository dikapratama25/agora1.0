Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class BuyerCat_Ext
        Dim objDb As New EAD.DBCom

        Function getBuyerCatItems(Optional ByVal pCatIndex As String = "", Optional ByVal pItemCode As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal strSP As String = "", Optional ByVal strST As String = "", Optional ByVal strMI As String = "", Optional ByVal strVendorName As String = "") As Object 'Michelle (25/10/2010)
            Dim strSQL, strTemp As String
            Dim ds As DataSet
            Dim strType As String = ""

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_OVERSEA, PM_ITEM_TYPE " & _
                    "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE "

            If strVendorName <> "" Then
                strSQL &= "INNER JOIN (SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR " & _
                        "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PV_S_COY_ID " & _
                        "WHERE CM_COY_NAME LIKE '%" & strVendorName & "%' " & _
                        "GROUP BY PV_PRODUCT_INDEX) tb ON tb.PV_PRODUCT_INDEX = PM_PRODUCT_INDEX "
            End If

            strSQL &= "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                    "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                    "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                    "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'  AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex <> "" Then
                strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
            End If
            If pName <> "" Then
                strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
            End If
            If pComType <> "" Then
                strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
            End If
            If pItemCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & pItemCode & "%' "
            End If
            If pItemType IsNot Nothing Then
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
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                Else
                    If strSP = "Y" Or strST = "Y" Or strMI = "Y" Then
                        strSQL &= " AND PM_ITEM_TYPE IN ("
                        strTemp = ""

                        If strSP = "Y" Then
                            strTemp = "'SP'"
                        End If

                        If strST = "Y" Then
                            If strTemp <> "" Then
                                strTemp &= ",'ST'"
                            Else
                                strTemp &= "'ST'"
                            End If
                        End If

                        If strMI = "Y" Then
                            If strTemp <> "" Then
                                strTemp &= ",'MI'"
                            Else
                                strTemp &= "'MI'"
                            End If
                        End If

                        strSQL &= strTemp & ")"

                    End If
                End If
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItems = ds
        End Function

        Function getBuyerCatItems1(Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal pCode As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal strOversea As String = "", Optional ByVal strVendor As String = "") As Object 'Joon (02/06/2011)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String = ""

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ITEM_TYPE, PM_OVERSEA, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ITEM_TYPE, PM_OVERSEA, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id, (SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id1, CAST(IFNULL(PM_LAST_TXN_TAX, 0) AS UNSIGNED) AS PM_LAST_TXN_TAX, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                    "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                    "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                    "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                    "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                    "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'  AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex <> "" Then
                strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
            End If
            If pName <> "" Then
                strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
            End If
            If pCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(pCode) & "%' "
            End If
            If pComType <> "" Then
                strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
            End If
            If pItemType IsNot Nothing Then
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
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
            End If
            If strOversea <> "" Then
                strSQL &= "AND PM_OVERSEA = '" & strOversea & "' "
            End If
            If strVendor <> "" Then
                strSQL &= "AND PM_LAST_TXN_S_COY_ID = '" & strVendor & "' "
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItems1 = ds
        End Function

        Function getBuyerCatItemsByCombo(Optional ByVal pCatIndex As String = "", Optional ByVal pItemCode As String = "", Optional ByVal strSP As String = "", Optional ByVal strST As String = "", Optional ByVal strMI As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal strVendorName As String = "") As Object 'Created by Joon (01 Apr 2011)
            Dim strSQL, strTemp As String
            Dim ds As DataSet
            Dim strType As String = ""

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_OVERSEA, PM_ITEM_TYPE " & _
                    "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE "

            If strVendorName <> "" Then
                strSQL &= "INNER JOIN (SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR " & _
                        "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PV_S_COY_ID " & _
                        "WHERE CM_COY_NAME LIKE '%" & strVendorName & "%' " & _
                        "GROUP BY PV_PRODUCT_INDEX) tb ON tb.PV_PRODUCT_INDEX = PM_PRODUCT_INDEX "
            End If

            strSQL &= "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                    "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                    "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                    "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                    "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex = "0" Then
                strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "

            Else
                If pCatIndex <> "" Then
                    strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "
                End If
                If pName <> "" Then
                    strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
                End If
                If pComType <> "" Then
                    strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
                End If
            End If

            If pItemCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & pItemCode & "%' "
            End If

            If pItemType IsNot Nothing Then
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
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If

                Else
                    If strSP = "Y" Or strST = "Y" Or strMI = "Y" Then
                        strSQL &= " AND PM_ITEM_TYPE IN ("
                        strTemp = ""

                        If strSP = "Y" Then
                            strTemp = "'SP'"
                        End If

                        If strST = "Y" Then
                            If strTemp <> "" Then
                                strTemp &= ",'ST'"
                            Else
                                strTemp &= "'ST'"
                            End If
                        End If

                        If strMI = "Y" Then
                            If strTemp <> "" Then
                                strTemp &= ",'MI'"
                            Else
                                strTemp &= "'MI'"
                            End If
                        End If

                        strSQL &= strTemp & ")"

                    End If
                End If

            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItemsByCombo = ds
        End Function

        Function getBuyerCatItemsByCombo1(Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal pCode As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal strOversea As String = "", Optional ByVal strVendor As String = "") As Object 'Joon (02/06/2011)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ITEM_TYPE, PM_OVERSEA, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ITEM_TYPE, PM_OVERSEA, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id, (SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id1, CAST(IFNULL(PM_LAST_TXN_TAX, 0) AS UNSIGNED) AS PM_LAST_TXN_TAX, PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                             "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                             "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                             "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                             "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                             "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                             "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                             "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex = "0" Then
                strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "
                If strOversea <> "" Then
                    strSQL &= "AND PM_OVERSEA = '" & strOversea & "' "
                End If
                If strVendor <> "" Then
                    strSQL &= "AND (PM_LAST_TXN_S_COY_ID = '" & strVendor & "' OR PM_LAST_TXN_S_COY_ID = '' OR PM_LAST_TXN_S_COY_ID IS NULL) "
                End If
            Else
                If pCatIndex <> "" Then
                    strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "
                End If
                If pName <> "" Then
                    strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
                End If
                If pCode <> "" Then
                    strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(pCode) & "%' "
                End If
                If pComType <> "" Then
                    strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
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
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
                If strOversea <> "" Then
                    strSQL &= "AND PM_OVERSEA = '" & strOversea & "' "
                End If
                If strVendor <> "" Then
                    strSQL &= "AND (PM_LAST_TXN_S_COY_ID = '" & strVendor & "' OR PM_LAST_TXN_S_COY_ID = '' OR PM_LAST_TXN_S_COY_ID IS NULL) "
                End If
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItemsByCombo1 = ds
        End Function

        'Michelle (27/10/2010) - To get the Items from Product Mstr
        Public Function getItems(ByVal strCoyId As String, ByVal strItemType As String, Optional ByVal strBCItemIdx As String = "", Optional ByVal strCode As String = "", Optional ByVal strName As String = "", Optional ByVal strComType As String = "", Optional ByVal strDel As String = "", Optional ByVal strSP As String = "", Optional ByVal strST As String = "", Optional ByVal strMI As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal txtVenCode As String = "") As DataSet
            Dim strsql, strTemp As String
            Dim ds As New DataSet
            Dim strType As String

            strsql = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_DELETED "
            strsql &= "FROM PRODUCT_MSTR "

            If txtVenCode <> "" Then
                strsql &= "INNER JOIN (SELECT DISTINCT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE PV_SUPP_CODE LIKE '%" & txtVenCode & "%') tb ON tb.PV_PRODUCT_INDEX = PM_PRODUCT_INDEX "
            End If

            strsql &= "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME "
            strsql &= "WHERE PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strsql &= "AND PM_PRODUCT_FOR = '" & Common.Parse(strItemType) & "' "

            If strDel = "A" Then
                strsql = strsql
            Else
                strsql &= "AND PM_DELETED = '" & Common.Parse(strDel) & "' "
            End If

            If strCode <> "" Then
                strsql &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strCode) & "%' "
            End If
            If strName <> "" Then
                strsql &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(strName) & "%' "
            End If

            If strComType <> "" Then
                strsql &= "AND PM_CATEGORY_NAME ='" & Common.Parse(strComType) & "' "
            End If

            If strBCItemIdx <> "" Then
                strsql &= "AND PM_PRODUCT_CODE NOT IN "
                strsql &= "(SELECT BCU_PRODUCT_CODE FROM BUYER_CATALOGUE_ITEMS "
                strsql &= "WHERE BCU_CAT_INDEX = '" & Common.Parse(strBCItemIdx) & "') "
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
                    strsql &= " AND PM_ITEM_TYPE IN " & strType
                End If
            Else
                If strSP = "Y" Then
                    strTemp = " AND (PM_ITEM_TYPE='SP'"
                Else
                    strTemp = ""
                End If
                If strST = "Y" Then
                    If strTemp <> "" Then
                        strTemp &= " OR PM_ITEM_TYPE='ST'"
                    Else
                        strTemp &= " AND (PM_ITEM_TYPE='ST'"
                    End If
                Else
                    strTemp &= ""
                End If
                If strMI = "Y" Then
                    If strTemp <> "" Then
                        strTemp &= " OR PM_ITEM_TYPE='MI'"
                    Else
                        strTemp &= " AND (PM_ITEM_TYPE='MI'"
                    End If
                Else
                    strTemp &= ""
                End If

                If strSP <> "Y" And strST <> "Y" And strMI <> "Y" Then
                    strTemp = ""
                Else
                    strTemp &= ")"
                End If

                strsql &= strTemp
            End If

            ds = objDb.FillDs(strsql)
            getItems = ds
        End Function

        Function getBuyerCatItemsFiltered(ByVal sCompanyId As String, ByVal sSelectedProductCode As String, Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal sItemType As String = "", Optional ByVal sOversea As String = "", Optional ByVal sVendorCode As String = "") As Object
            Dim strSQL As String
            Dim ds As DataSet

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_PRODUCT_INDEX, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "

                'strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_PRODUCT_INDEX, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If

            If sSelectedProductCode <> "" Then
                strSQL &= "'' AS PREFER, '' AS 1ST, '' AS 2ND, '' AS 3RD, PM_LAST_TXN_PRICE FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                     "INNER JOIN PIM_VENDOR ON PM_PRODUCT_INDEX = PV_PRODUCT_INDEX " & _
                     "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                     "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

                If sCompanyId <> "" Then
                    strSQL &= "AND PV_S_COY_ID = '" & sCompanyId & "' "
                End If

                If sItemType <> "" Then
                    strSQL &= "AND PM_ITEM_TYPE = '" & sItemType & "' "
                End If

                If sOversea <> "" Then
                    strSQL &= "AND PM_OVERSEA = '" & sOversea & "' "
                End If

                If sVendorCode <> "" Then
                    strSQL &= "AND PV_SUPP_CODE = '" & sVendorCode & "' "
                End If

                strSQL &= "AND NOT PM_PRODUCT_CODE IN (" & sSelectedProductCode & ")"

                'strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE " & _
                '                 "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                '                 "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                '                 "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'" & _
                '                 " AND (PM_PREFER_S_COY_ID = '" & sCompanyId & "' OR PM_1ST_S_COY_ID = '" & sCompanyId & "' OR PM_2ND_S_COY_ID = '" & sCompanyId & "' OR  PM_3RD_S_COY_ID= '" & sCompanyId & "')" & _
                '                " AND NOT PM_PRODUCT_CODE IN (" & sSelectedProductCode & ")"
            Else
                strSQL &= "'' AS PREFER, '' AS 1ST, '' AS 2ND, '' AS 3RD, PM_LAST_TXN_PRICE FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                        "INNER JOIN PIM_VENDOR ON PM_PRODUCT_INDEX = PV_PRODUCT_INDEX " & _
                        "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                        "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

                If sCompanyId <> "" Then
                    strSQL &= "AND PV_S_COY_ID = '" & sCompanyId & "' "
                End If

                If sItemType <> "" Then
                    strSQL &= "AND PM_ITEM_TYPE = '" & sItemType & "' "
                End If

                If sOversea <> "" Then
                    strSQL &= "AND PM_OVERSEA = '" & sOversea & "' "
                End If

                If sVendorCode <> "" Then
                    strSQL &= "AND PV_SUPP_CODE = '" & sVendorCode & "' "
                End If

                'strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE " & _
                '                 "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
                '                 "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
                '                 "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
                '                 "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'" & _
                '                 " AND (PM_PREFER_S_COY_ID = '" & sCompanyId & "' OR PM_1ST_S_COY_ID = '" & sCompanyId & "' OR PM_2ND_S_COY_ID = '" & sCompanyId & "' OR  PM_3RD_S_COY_ID= '" & sCompanyId & "')"

            End If
            If pCatIndex <> "" Then
                strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
            End If
            If pName <> "" Then
                strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
            End If
            If pComType <> "" Then
                strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItemsFiltered = ds
        End Function

        Public Function chkCommonVendor(ByVal aryProdCode As ArrayList) As ArrayList
            Dim aryTemp As New ArrayList
            Dim strSql As String = ""
            Dim strSql2 As String = ""
            Dim iProdIndex As String = ""
            Dim i As Integer
            Dim ds As New DataSet
            Dim dt As New DataTable

            If aryProdCode.Count > 0 Then
                For i = 0 To aryProdCode.Count - 1
                    iProdIndex = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & aryProdCode(i) & "'")

                    If i = 0 Then
                        strSql = "PV_PRODUCT_INDEX = '" & iProdIndex & "' "
                        strSql2 = "PM_PRODUCT_INDEX = '" & iProdIndex & "' "
                    Else
                        strSql &= "OR PV_PRODUCT_INDEX = '" & iProdIndex & "' "
                        strSql2 &= "OR PM_PRODUCT_INDEX = '" & iProdIndex & "' "
                    End If
                Next
            End If

            dt = objDb.FillDt("SELECT '*' FROM PRODUCT_MSTR WHERE (" & strSql2 & ")")

            If aryProdCode.Count = 1 Then
                strSql = "SELECT PV_S_COY_ID, NUM FROM " & _
                        "(SELECT CASE WHEN PV_VENDOR_TYPE = 'P' THEN '0' ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE, " & _
                        "PV_S_COY_ID, '1' AS NUM FROM PIM_VENDOR WHERE " & strSql & _
                        "ORDER BY PV_VENDOR_TYPE) tb " & _
                        "GROUP BY PV_S_COY_ID "
            Else
                strSql = "SELECT PV_S_COY_ID, COUNT(PV_S_COY_ID) AS NUM, PV_VENDOR_TYPE FROM " & _
                        "(SELECT tb.* FROM " & _
                        "(SELECT PV_PRODUCT_INDEX, PV_S_COY_ID, CASE WHEN PV_VENDOR_TYPE = 'P' THEN '0' " & _
                        "ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE FROM PIM_VENDOR WHERE (" & strSql & ") " & _
                        "ORDER BY PV_VENDOR_TYPE) tb " & _
                        "GROUP BY PV_PRODUCT_INDEX, PV_S_COY_ID ORDER BY PV_VENDOR_TYPE) tb " & _
                        "GROUP BY PV_S_COY_ID ORDER BY PV_VENDOR_TYPE "
            End If
            
            ds = objDb.FillDs(strSql)

            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i)("NUM") = dt.Rows.Count Then
                    aryTemp.Add(ds.Tables(0).Rows(i)("PV_S_COY_ID"))
                End If
            Next

            chkCommonVendor = aryTemp

        End Function

        Public Function chkCommonVenCode(ByVal aryProdCode As ArrayList, ByVal aryProdVen As ArrayList) As ArrayList
            Dim aryTemp As New ArrayList
            Dim strSql As String = ""
            Dim iProdIndex As String = ""
            Dim i, j, k As Integer
            Dim dsItem As New DataSet

            For i = 0 To aryProdCode.Count - 1
                If i = 0 Then
                    strSql = "PM_PRODUCT_CODE = '" & aryProdCode(i) & "' "
                Else
                    strSql &= "OR PM_PRODUCT_CODE = '" & aryProdCode(i) & "' "
                End If
            Next

            dsItem = objDb.FillDs("SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_ITEM_TYPE = 'ST' AND (" & strSql & ")")
            'strSql = "SELECT '*' FROM PRODUCT_MSTR WHERE PM_ITEM_TYPE = 'ST' AND (" & strSql & ")"
            If dsItem.Tables(0).Rows.Count > 0 Then
                For i = 0 To aryProdVen.Count - 1
                    Dim ds As New DataSet

                    For j = 0 To dsItem.Tables(0).Rows.Count - 1
                        iProdIndex = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & dsItem.Tables(0).Rows(j)(0) & "'")

                        If j = 0 Then
                            strSql = "PV_PRODUCT_INDEX = '" & iProdIndex & "' "
                        Else
                            strSql &= "OR PV_PRODUCT_INDEX = '" & iProdIndex & "' "
                        End If
                    Next

                    strSql = "SELECT PV_SUPP_CODE, PV_CURR, COUNT('*') AS NUM FROM PIM_VENDOR WHERE (" & strSql & ") AND PV_S_COY_ID = '" & aryProdVen(i) & "'" & _
                            "GROUP BY PV_SUPP_CODE, PV_CURR "

                    ds = objDb.FillDs(strSql)

                    For k = 0 To ds.Tables(0).Rows.Count - 1
                        If ds.Tables(0).Rows(k)("NUM") >= dsItem.Tables(0).Rows.Count Then
                            aryTemp.Add(aryProdVen(i))
                            Exit For
                        End If
                    Next
                Next
            Else
                aryTemp = aryProdVen
            End If

            'If objDb.Exist(strSql) Then
            '    For i = 0 To aryProdVen.Count - 1
            '        Dim ds As New DataSet

            '        For j = 0 To aryProdCode.Count - 1
            '            iProdIndex = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & aryProdCode(j) & "'")

            '            If j = 0 Then
            '                strSql = "PV_PRODUCT_INDEX = '" & iProdIndex & "' "
            '            Else
            '                strSql &= "OR PV_PRODUCT_INDEX = '" & iProdIndex & "' "
            '            End If
            '        Next

            '        strSql = "SELECT PV_SUPP_CODE, PV_CURR, COUNT('*') AS NUM FROM PIM_VENDOR WHERE (" & strSql & ") AND PV_S_COY_ID = '" & aryProdVen(i) & "'" & _
            '                "GROUP BY PV_SUPP_CODE, PV_CURR "

            '        ds = objDb.FillDs(strSql)

            '        For k = 0 To ds.Tables(0).Rows.Count - 1
            '            If ds.Tables(0).Rows(k)("NUM") >= 2 Then
            '                aryTemp.Add(aryProdVen(i))
            '                Exit For
            '            End If
            '        Next
            '    Next
            'Else
            '    aryTemp = aryProdVen
            'End If

            chkCommonVenCode = aryTemp

        End Function
    End Class

End Namespace
