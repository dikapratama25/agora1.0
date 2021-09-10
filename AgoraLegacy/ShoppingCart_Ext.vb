Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class ShoppingCart_Ext
        Dim objDb As New EAD.DBCom

        Public Function getPRItemList(ByVal strType As String, ByVal strIndex As String, ByVal strPRNo As String, Optional ByVal vcomid As String = "", Optional ByVal LineNumber As ArrayList = Nothing, Optional ByVal strDesc As String = "", Optional ByVal strUOM As String = "", Optional ByVal strComm As String = "", Optional ByVal strCatGroup As String = "", Optional ByVal ItemType As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Select Case strType
                Case "BuyerCat"
                    If strIndex <> "" And strIndex <> "&nbsp;" Then
                        strSql = "SELECT PM_PRODUCT_CODE AS PRODUCTCODE, PM_LAST_TXN_PRICE AS UNITCOST, PM_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "PM_UOM AS UOM, 'BC' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        'strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(PM_LAST_TXN_TAX,0) AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, PM_LAST_TXN_S_COY_ID AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, PM_ITEM_TYPE AS PRD_ITEM_TYPE, PM_OVERSEA AS PRD_OVERSEA "
                        ' yAP: '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE For FTN
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, '' AS GSTRateDesc, '' AS GSTRate, 0.00 AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, PM_LAST_TXN_S_COY_ID AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, PM_ITEM_TYPE AS PRD_ITEM_TYPE, PM_OVERSEA AS PRD_OVERSEA, '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE', "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "'' AS GstTaxCode "
                        strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                        strSql &= "WHERE PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PM_DELETED <> 'Y' "
                    Else
                        strSql = "SELECT '' AS PRODUCTCODE, 0.00 AS UNITCOST, '" & Common.Parse(strDesc) & "' AS PRODUCTDESC, '' AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "'" & strUOM & "' AS UOM, 'FF' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        'strSql &= "'' AS CATEGORYCODE, '" & Common.Parse(strComm) & "' AS COMMODITY, '' AS GLCODE, '' AS TAXCODE, 0.00 AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, '' AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, '" & Common.Parse(ItemType) & "' AS PRD_ITEM_TYPE, 'N' AS PRD_OVERSEA "
                        ' yAP: '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE For FTN
                        strSql &= "'' AS CATEGORYCODE, '" & Common.Parse(strComm) & "' AS COMMODITY, '' AS GLCODE, '' AS TAXCODE, '' AS GSTRateDesc, '' AS GSTRate, 0.00 AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, '' AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, '" & Common.Parse(ItemType) & "' AS PRD_ITEM_TYPE, 'N' AS PRD_OVERSEA, '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE', "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "'' AS GstTaxCode "
                        strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE LIMIT 1 "
                    End If

                Case "ConCat"
                    If strIndex <> "" And strIndex <> "&nbsp;" Then
                        strSql = "SELECT PM_PRODUCT_CODE AS PRODUCTCODE, CDI_UNIT_COST AS UNITCOST, CDI_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "CDI_UOM AS UOM, 'CP' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, "
                        'strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(CDI_GST,0) AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, PM_ITEM_TYPE AS PRD_ITEM_TYPE, PM_OVERSEA AS PRD_OVERSEA, "

                        strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, "

                        ' GST Rate Desc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRateDesc, "
                        ' GST Rate Code
                        strSql &= "IFNULL((SELECT CODE_ABBR "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRate, "
                        ' GST Perc
                        strSql &= "IFNULL((SELECT CAST(IF(TAX_PERC='', 0, TAX_PERC) AS DECIMAL) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 0) AS GST, "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "CDI_GST_TAX_CODE AS GstTaxCode, "
                        strSql &= "CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, PM_ITEM_TYPE AS PRD_ITEM_TYPE, PM_OVERSEA AS PRD_OVERSEA, "

                        strSql &= "'' as POD_MANUFACTURER, '' AS POD_PUR_SPEC_NO, PM_SPEC1 AS POD_SPEC1, PM_SPEC2 AS POD_SPEC2, PM_SPEC3 AS POD_SPEC3, 0.00 AS POD_NEXT1_QTY, 0.00 AS POD_NEXT2_QTY, 0.00 AS POD_NEXT3_QTY, 0.00 AS POD_CURR_QTY, PM_ITEM_TYPE AS POD_ITEM_TYPE, PM_OVERSEA AS POD_OVERSEA, "
                        strSql &= "PM_PACKING_TYPE AS POD_PACKING_TYPE, PM_PACKING_QTY AS POD_PACKING_QTY, 0.00 as 'POD_TAX_VALUE' "
                        strSql &= "FROM CONTRACT_DIST_COY "
                        strSql &= "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
                        strSql &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
                        strSql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX "
                        strSql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "WHERE CDC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                        strSql &= "AND CDM_GROUP_INDEX = '" & strCatGroup & "' "
                        strSql &= "AND PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_DELETED <> 'Y' "
                    Else
                        strSql = "SELECT PM_PRODUCT_CODE AS PRODUCTCODE, CDI_UNIT_COST AS UNITCOST, CDI_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "CDI_UOM AS UOM, 'CP' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, "
                        'strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(CDI_GST,0) AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, , '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, '" & Common.Parse(ItemType) & "' AS PRD_ITEM_TYPE, 'N' AS PRD_OVERSEA, "

                        strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, "

                        ' GST Rate Desc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRateDesc, "
                        ' GST Rate Code
                        strSql &= "IFNULL((SELECT CODE_ABBR "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRate, "
                        ' GST Perc
                        strSql &= "IFNULL((SELECT CAST(IF(TAX_PERC='', 0, TAX_PERC) AS DECIMAL) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 0) AS GST, "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "CDI_GST_TAX_CODE AS GstTaxCode, "
                        strSql &= "CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, , '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, '" & Common.Parse(ItemType) & "' AS PRD_ITEM_TYPE, 'N' AS PRD_OVERSEA, "

                        strSql &= "'' as POD_MANUFACTURER, '' AS POD_PUR_SPEC_NO, PM_SPEC1 AS POD_SPEC1, PM_SPEC2 AS POD_SPEC2, PM_SPEC3 AS POD_SPEC3, 0.00 AS POD_NEXT1_QTY, 0.00 AS POD_NEXT2_QTY, 0.00 AS POD_NEXT3_QTY, 0.00 AS POD_CURR_QTY, PM_ITEM_TYPE AS POD_ITEM_TYPE, PM_OVERSEA AS POD_OVERSEA, "
                        strSql &= "PM_PACKING_TYPE AS POD_PACKING_TYPE, PM_PACKING_QTY AS POD_PACKING_QTY, 0.00 as 'POD_TAX_VALUE' "
                        strSql &= "FROM CONTRACT_DIST_COY "
                        strSql &= "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
                        strSql &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
                        strSql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX "
                        strSql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "WHERE CDC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                        strSql &= "AND CDM_GROUP_INDEX = '" & strCatGroup & "' "
                        strSql &= "AND PM_DELETED <> 'Y' "
                    End If

                Case "PR"
                    strSql = "SELECT * FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strPRNo) & "' "
                    strSql &= "AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'; "

                    strSql &= "SELECT PRD_PR_NO, PRD_COY_ID AS COYID, CAST(PRD_PR_LINE AS SIGNED ) AS ITEMINDEX, PRD_PRODUCT_CODE AS PRODUCTCODE, PRD_VENDOR_ITEM_CODE AS VENDORITEMCODE, PRD_PRODUCT_DESC AS PRODUCTDESC, PRD_B_CATEGORY_CODE AS CATEGORYCODE, PRD_B_GL_CODE AS GLCODE, PRD_UOM AS UOM, "
                    'strSql &= "PRD_ORDERED_QTY AS QUANTITY, PRD_UNIT_COST AS UNITCOST, PRD_REMARK AS REMARK, IFNULL(PRD_GST,0) AS GST, "

                    strSql &= "PRD_ORDERED_QTY AS QUANTITY, PRD_UNIT_COST AS UNITCOST, PRD_REMARK AS REMARK, "

                    ' GST Rate Desc
                    strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                    strSql &= "FROM CODE_MSTR "
                    strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                    strSql &= "WHERE CODE_CATEGORY = 'GST' AND CODE_DELETED = 'N' AND CODE_ABBR = PR_DETAILS.PRD_GST_RATE), 'N/A') AS GSTRateDesc, "
                    ' GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                    strSql &= "PRD_GST_INPUT_TAX_CODE AS GstTaxCode, "
                    strSql &= "PRD_GST_RATE AS GSTRate, IFNULL(PRD_GST,0) AS GST, "
                    strSql &= "PRD_D_ADDR_CODE AS DADDRCODE, PRD_B_ITEM_CODE AS ITEMCODE, (PRD_UNIT_COST * PRD_ORDERED_QTY) AS AMOUNT, CAST(PRD_PR_LINE AS SIGNED) AS ITEMLINE, PRD_ETD AS ETD, PRD_WARRANTY_TERMS AS WARRANTYTERMS, CAST(PRD_ACCT_INDEX AS CHAR(1000)) AS ACCT, PRD_SOURCE AS SOURCE, PRD_COY_ID, '' AS TAXCODE, 0.00 AS RFQ_QTY, '' AS TOLERANCE, '' AS CDGROUP, "
                    strSql &= "PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PR_DETAILS.PRD_CT_ID) AS COMMODITY, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PRD_CURRENCY_CODE AS CURRENCY, PRD_S_COY_ID AS VENDOR, CAST(PRD_CD_GROUP_INDEX as CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, PRD_ASSET_GROUP, PRD_ASSET_NO, "
                    strSql &= "IFNULL(PRD_LEAD_TIME,0) AS PRD_LEAD_TIME, PRD_ITEM_TYPE, PRD_DEL_CODE, PRD_OVERSEA "
                    strSql &= "FROM PR_DETAILS "
                    strSql &= "WHERE PRD_PR_NO = '" & Common.Parse(strPRNo) & "' "
                    strSql &= "AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'; "

                    strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PR_MSTR ON "
                    strSql &= "PRM_PR_INDEX = PCD_PR_INDEX WHERE "
                    strSql &= "PRM_PR_NO = '" & Common.Parse(strPRNo) & "' "
                    strSql &= "AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PCD_TYPE = 'PR'; "

            End Select

            ds = objDb.FillDs(strSql)
            getPRItemList = ds
        End Function

        Public Function getPOItemList(ByVal strType As String, ByVal strIndex As String, ByVal strPONo As String, Optional ByVal vcomid As String = "", Optional ByVal LineNumber As ArrayList = Nothing, Optional ByVal RFQListing As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Select Case strType
                'Michelle (15/11/2010) - Shopping Cart is obselete, replace by Purchasing Catalogue (PC)
                Case "PC"
                    strSql = "SELECT PM_PRODUCT_CODE AS PRODUCTCODE, PM_LAST_TXN_PRICE AS UNITCOST, PM_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, 500 AS ITEMLINE, "
                    strSql &= "PM_UOM AS UOM, 'PC' AS SOURCE, 0.00 AS RFQ_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                    strSql &= "PM_CAT_CODE AS CATEGORYCODE, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, '' AS GSTRATE, 0.00 AS GST, CAST(1.00 AS CHAR(10)) AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, '' AS CDM_GROUP_INDEX, '' AS ACCT, '' AS POD_ASSET_GROUP, '' AS POD_ASSET_NO,  "
                    strSql &= "'' as POD_MANUFACTURER, '' AS POD_PUR_SPEC_NO, PM_SPEC1 AS POD_SPEC1, PM_SPEC2 AS POD_SPEC2, PM_SPEC3 AS POD_SPEC3, 0.00 AS POD_NEXT1_QTY, 0.00 AS POD_NEXT2_QTY, 0.00 AS POD_NEXT3_QTY, 0.00 AS POD_CURR_QTY, PM_ITEM_TYPE AS POD_ITEM_TYPE, PM_OVERSEA AS POD_OVERSEA, PM_PACKING_TYPE AS POD_PACKING_TYPE, PM_PACKING_QTY AS POD_PACKING_QTY, 0.00 AS POD_TAX_VALUE, "
                    ' GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    strSql &= "'' AS GstTaxCode "
                    strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "WHERE PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PM_DELETED <> 'Y' "

                Case "PO" ' from PO_DETAILS table
                    strSql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' "
                    strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                    strSql &= "SELECT POD_PO_NO, POD_COY_ID AS COYID, POD_PO_LINE AS ITEMINDEX, POD_PRODUCT_CODE AS PRODUCTCODE, POD_VENDOR_ITEM_CODE AS VENDORITEMCODE, POD_PRODUCT_DESC AS PRODUCTDESC, POD_B_CATEGORY_CODE AS CATEGORYCODE, POD_B_GL_CODE AS GLCODE, POD_UOM AS UOM, "
                    'strSql &= "POD_ORDERED_QTY AS QUANTITY, POD_UNIT_COST AS UNITCOST, POD_REMARK AS REMARK, ISNULL(POD_GST,0) AS GST, "
                    strSql &= "POD_ORDERED_QTY AS QUANTITY, POD_UNIT_COST AS UNITCOST, POD_REMARK AS REMARK, POD_GST_RATE AS GSTRATE, ISNULL(POD_GST,0) AS GST, "
                    strSql &= "POD_D_ADDR_CODE AS DADDRCODE, POD_B_ITEM_CODE AS ITEMCODE, (POD_UNIT_COST * POD_ORDERED_QTY) AS AMOUNT, POD_PO_LINE AS ITEMLINE, POD_ETD AS ETD, POD_WARRANTY_TERMS AS WARRANTYTERMS, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_SOURCE AS SOURCE, POD_COY_ID, '' AS TAXCODE, 0.00 AS RFQ_QTY, '' AS TOLERANCE, '' AS CDGROUP,  "
                    strSql &= "POD_D_ADDR_CODE, POD_D_ADDR_LINE1, POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, POD_D_COUNTRY, '' AS COMMODITY, "
                    strSql &= "CAST((CASE WHEN POD_MIN_ORDER_QTY IS NULL THEN 1.00 ELSE POD_MIN_ORDER_QTY END) AS CHAR(10)) AS MOQ, "
                    strSql &= "CAST((CASE WHEN POD_MIN_PACK_QTY IS NULL THEN 1.00 ELSE POD_MIN_PACK_QTY END) AS CHAR(10)) AS MPQ, "
                    strSql &= "CAST(IFNULL(POD_CD_GROUP_INDEX, '') AS CHAR(100)) AS CDM_GROUP_INDEX, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_RFQ_ITEM_LINE, POD_ASSET_GROUP, POD_ASSET_NO, "
                    strSql &= "POD_LEAD_TIME, POD_MANUFACTURER, POD_PUR_SPEC_NO, POD_SPEC1, POD_SPEC2, POD_SPEC3, POD_NEXT1_QTY, POD_NEXT2_QTY, POD_NEXT3_QTY, POD_CURR_QTY, POD_ITEM_TYPE, POD_OVERSEA, POD_PACKING_TYPE, POD_PACKING_QTY, IFNULL(POD_TAX_VALUE,0.00) AS POD_TAX_VALUE, "
                    'strSql &= "POD_MIN_ORDER_QTY AS MOQ,POD_MIN_PACK_QTY AS MPQ "
                    ' GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                    strSql &= "POD_GST_INPUT_TAX_CODE AS GstTaxCode "
                    strSql &= "FROM PO_DETAILS "
                    strSql &= "WHERE POD_PO_NO = '" & strPONo & "' "
                    strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                    strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PO_MSTR ON  "
                    strSql &= "POM_PO_INDEX = PCD_PR_INDEX WHERE "
                    strSql &= " POM_PO_NO = '" & strPONo & "' "
                    strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PO'; "

                Case "RFQ"
                    Dim i As Int16, sLineNumber As String = ""
                    If Not IsNothing(LineNumber) Then
                        For i = 0 To LineNumber.Count - 1
                            sLineNumber = sLineNumber & " OR RRD_Line_No = '" & LineNumber(i) & "' "
                        Next
                    End If

                    If sLineNumber.Length > 0 Then
                        sLineNumber = " AND (" & Mid(sLineNumber, 5) & ")"
                    End If

                    strSql = "SELECT distinct RRD_V_Coy_Id AS COYID, RRD_Line_No AS ITEMINDEX, RRD_Product_Code AS PRODUCTCODE, RRD_Vendor_Item_Code AS VENDORITEMCODE, RRD_Product_Desc AS PRODUCTDESC, "
                    strSql &= "RRD_UOM AS UOM, RRD_Quantity AS RFQ_QTY, RRD_Quantity AS QUANTITY, RRD_Unit_Price AS UNITCOST, RRD_Min_Order_Qty AS MOQ, CAST((CASE WHEN RRD_Min_Pack_Qty IS NULL THEN 1.00 ELSE RRD_Min_Pack_Qty END) AS CHAR(10)) AS MPQ, "
                    'If RFQListing <> "" Then
                    '    strSql &= " (SELECT PRD_REMARK FROM PR_MSTR, PR_DETAILS "
                    '    strSql &= " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_LINE_INDEX = RFQ_DETAIL.RD_PR_LINE_INDEX AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS REMARK, "
                    'Else
                    '    strSql &= " RRD_Remarks AS REMARK, "
                    'End If
                    strSql &= " RRD_Remarks AS REMARK, RM_Remark AS POM_EXTERNAL_REMARK, "
                    strSql &= " RRD_GST_RATE AS GSTRATE, "
                    strSql &= "ISNULL(CAST(RRD_GST AS DECIMAL),0.00) AS GST, "
                    strSql &= "CBC_B_ITEM_CODE AS ITEMCODE, (RRD_Unit_Price * RRD_Quantity) AS AMOUNT, RRD_Line_No AS ITEMLINE, RRD_Delivery_Lead_Time AS ETD, RRD_Warranty_Terms AS WARRANTYTERMS, "
                    'Michelle (16/6/2010) - To include the Tax Code
                    'strSql &= "CBC_B_CATEGORY_CODE AS CATEGORYCODE, CBC_B_GL_CODE AS GLCODE, "
                    'strSql &= "CBC_B_CATEGORY_CODE AS CATEGORYCODE, "
                    strSql &= "(SELECT PM_CAT_CODE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS CATEGORYCODE, "
                    strSql &= "(SELECT PM_ACCT_CODE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS GLCODE, "
                    strSql &= "CBC_B_TAX_CODE AS TAXCODE, "
                    strSql &= "CAST(RRD_Tolerance AS CHAR) AS TOLERANCE, 'RF' AS SOURCE, '' AS CDGROUP, '' AS CDM_GROUP_INDEX, '' AS ACCT, '' AS POD_ASSET_GROUP, '' AS POD_ASSET_NO, "
                    strSql &= "'' as POD_MANUFACTURER, '' AS POD_PUR_SPEC_NO, "
                    strSql &= "(SELECT PM_SPEC1 FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_SPEC1, "
                    strSql &= "(SELECT PM_SPEC2 FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_SPEC2, "
                    strSql &= "(SELECT PM_SPEC3 FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_SPEC3, "
                    strSql &= "0.00 AS POD_NEXT1_QTY, 0.00 AS POD_NEXT2_QTY, 0.00 AS POD_NEXT3_QTY, 0.00 AS POD_CURR_QTY, "
                    strSql &= "(SELECT PM_ITEM_TYPE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_ITEM_TYPE, "
                    strSql &= "(SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_OVERSEA, "
                    strSql &= "(SELECT PM_PACKING_TYPE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_PACKING_TYPE, "
                    strSql &= "(SELECT PM_PACKING_QTY FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS POD_PACKING_QTY, 0.00 AS POD_TAX_VALUE, "
                    ' GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                    strSql &= "'' AS GstTaxCode "
                    'strSql &= "(SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE = RFQ_REPLIES_DETAIL.RRD_Line_No) AS ACCT "
                    'strSql &= "(SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PRODUCT_CODE = RFQ_REPLIES_DETAIL.RRD_Product_Code) AS ACCT "
                    '' ''strSql &= "CAST(IF( RFQ_REPLIES_DETAIL.RRD_Product_Code = '&nbsp;' OR RFQ_REPLIES_DETAIL.RRD_Product_Code = '', (SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PRODUCT_DESC = RFQ_REPLIES_DETAIL.RRD_Product_Desc), (SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = RFQ_DETAIL.RD_PR_LINE_INDEX )) AS CHAR(100)) AS ACCT "
                    'If sLineNumber.Length > 0 Then
                    '    strSql &= " AND PRD_PR_LINE = RRD_Line_No )) AS CHAR(100)) AS ACCT "
                    'Else
                    '    strSql &= " )) AS CHAR(100)) AS ACCT "
                    'End If

                    strSql &= "FROM RFQ_MSTR "
                    '' ''strSql &= "INNER JOIN RFQ_DETAIL ON RD_RFQ_ID = RM_RFQ_ID "
                    strSql &= "INNER JOIN RFQ_REPLIES_MSTR ON RRM_RFQ_ID = RM_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "INNER JOIN RFQ_REPLIES_DETAIL ON RRM_RFQ_ID = RRD_RFQ_ID AND RRM_V_COMPANY_ID = '" & vcomid & "' "
                    strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON RRD_Product_Code = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'Michelle (23/2/2011) - To enable PO to raise when the unit price = 0 (ie. free)
                    'strSql &= "WHERE RRD_RFQ_ID = '" & strPONo & "' and  RRD_Unit_Price <> 0 "
                    strSql &= "WHERE RRD_RFQ_ID = '" & strPONo & "' and  RRD_Unit_Price >= 0 "
                    'strSql &= "AND RRD_Line_No IN (" & strIndex & ") and RRD_V_Coy_Id='" & vcomid & "' "
                    strSql &= "AND RRD_V_Coy_Id='" & vcomid & "' " & sLineNumber & "; "

                    If RFQListing <> "" Then
                        'strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PR_MSTR ON "
                        'strSql &= "PRM_PR_INDEX = PCD_PR_INDEX WHERE "
                        'strSql &= "PRM_PR_INDEX = '" & Common.Parse(RFQListing) & "' "
                        'strSql &= "AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PCD_TYPE = 'PR'; "
                        Dim sLineNumber2 As String = ""
                        If Not IsNothing(LineNumber) Then
                            For i = 0 To LineNumber.Count - 1
                                sLineNumber2 = sLineNumber2 & " OR PCD_PR_LINE = '" & LineNumber(i) & "' "
                            Next
                        End If

                        If sLineNumber2.Length > 0 Then
                            sLineNumber2 = " AND (" & Mid(sLineNumber2, 5) & ")"
                        End If

                        'strSql &= " SET @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
                        'strSql &= " SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE, "
                        'strSql &= " PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM ( "
                        'strSql &= " SELECT PCD_PR_INDEX, @num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, "
                        'strSql &= " @num AS LINE_NO, @PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, "
                        'strSql &= " @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE "
                        'strSql &= " FROM RFQ_DETAIL "
                        'strSql &= " INNER JOIN PR_DETAILS ON PRD_PR_LINE_INDEX = RD_PR_LINE_INDEX "
                        'strSql &= " INNER JOIN PR_MSTR ON PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO "
                        'strSql &= " INNER JOIN PR_CUSTOM_FIELD_DETAILS ON PCD_PR_INDEX = PRM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE "
                        'strSql &= " WHERE RD_RFQ_ID = '" & strPONo & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PR' " & sLineNumber2 & " "
                        'strSql &= " ORDER BY RD_RFQ_LINE, PCD_PR_LINE, PCD_FIELD_NO "
                        'strSql &= " ) ZZZZ "

                        strSql &= " SET @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
                        strSql &= " SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE, "
                        strSql &= " PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM ( "
                        strSql &= " SELECT PCD_PR_INDEX, @num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, "
                        strSql &= " @num AS LINE_NO, @PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, "
                        strSql &= " @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE "
                        strSql &= " FROM RFQ_DETAIL "
                        strSql &= " INNER JOIN PR_DETAILS ON PRD_PR_LINE_INDEX = RD_PR_LINE_INDEX "
                        strSql &= " INNER JOIN PR_MSTR ON PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO "
                        strSql &= " INNER JOIN PR_CUSTOM_FIELD_DETAILS ON PCD_PR_INDEX = PRM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE "
                        strSql &= " WHERE RD_RFQ_ID = '" & strPONo & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PR' "
                        strSql &= " ORDER BY RD_RFQ_LINE, PCD_PR_LINE, PCD_FIELD_NO "
                        strSql &= " ) ZZZZ "

                    End If
            End Select

            ds = objDb.FillDs(strSql)
            getPOItemList = ds
        End Function
    End Class
End Namespace
