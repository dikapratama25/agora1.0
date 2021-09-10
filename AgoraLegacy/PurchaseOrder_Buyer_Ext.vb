Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO


Namespace AgoraLegacy
    Public Class PurchaseOrder_Buyer_Ext

        Dim objDb As New EAD.DBCom
        Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
        Dim com_id As String = HttpContext.Current.Session("CompanyId")
        Dim userid As String = HttpContext.Current.Session("UserId")

        Public Function insertPO(ByVal dsPO As DataSet, ByRef strPONo As String, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True) As Integer

            Dim strPrefix, strName, strPhone, strFax As String
            Dim strsql, strTermFile As String
            Dim strAryQuery(0) As String
            Dim strAryAdd(8) As String
            Dim i, j As Integer
            Dim blnAdd As Boolean = False
            Dim strDeptIndex As String = ""
            Dim objGlobal As New AppGlobals
            Dim intIncrementNo As Integer = 0

            strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(strAryQuery, strsql)

            'GetLatestDocNo
            '' ''objGlobal.GetLatestDocNo("PO", strAryQuery, strPONo, strPrefix)

            intIncrementNo = 1

            strPONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            strPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PO') "

            strsql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = " & strPONo & " "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If objDb.Exist(strsql) > 0 Then
                insertPO = WheelMsgNum.Duplicate
                Exit Function
            End If

            ' to check whether vendor company is inactive
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
            strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPO = -1
                Exit Function
            End If

            ' to check whether vendor company is being deleted
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
            strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPO = -2
                Exit Function
            End If

            ' get dept index
            'Michelle (26/4/2011) - To cater for those without department
            strsql = "SELECT CDM_DEPT_INDEX, UM_USER_NAME, UM_FAX_NO, UM_TEL_NO FROM USER_MSTR "
            'strsql &= "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
            strsql &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
            strsql &= "AND UM_COY_ID = CDM_COY_ID "
            strsql &= "WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strsql &= "AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
                strName = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_USER_NAME"))
                strPhone = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_TEL_NO"))
                strFax = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_FAX_NO"))
            Next

            'Michelle (27/12/2010) - To store the Term & Condition
            Dim objWheelFile As New FileManagement
            strTermFile = objWheelFile.copyTermCondToPO(strPONo)

            ' PO_MSTR table
            strsql = "INSERT INTO PO_MSTR (POM_PO_NO, POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, "
            strsql &= "POM_CREATED_DATE, POM_CREATED_BY, POM_STATUS_CHANGED_BY, POM_STATUS_CHANGED_ON, "
            strsql &= "POM_S_COY_ID, POM_S_ATTN, POM_S_COY_NAME, POM_S_ADDR_LINE1, "
            strsql &= "POM_S_ADDR_LINE2, POM_S_ADDR_LINE3, POM_S_POSTCODE, POM_S_CITY, POM_S_STATE, "
            strsql &= "POM_S_COUNTRY, POM_S_PHONE, POM_S_FAX, POM_S_EMAIL, "
            strsql &= "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, "
            strsql &= "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, POM_INTERNAL_REMARK, "
            strsql &= "POM_EXTERNAL_REMARK, POM_PO_STATUS, POM_FULFILMENT, POM_PO_INDEX, POM_ARCHIVE_IND, "
            strsql &= "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, POM_PO_PREFIX, POM_B_ADDR_CODE, "
            strsql &= "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, "
            strsql &= "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, "
            strsql &= "POM_DUP_FROM, POM_EXTERNAL_IND, POM_REFERENCE_NO, "
            strsql &= "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND, POM_URGENT, POM_DEL_CODE, POM_VENDOR_CODE) SELECT "
            strsql &= "" & strPONo & ", "
            strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            strsql &= "'" & Common.Parse(strName) & "', "
            strsql &= "'" & Common.Parse(strPhone) & "', "
            strsql &= "'" & Common.Parse(strFax) & "', " 'BUYER_FAX
            strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', " 'STATUS_CHANGED_BY
            strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', " 'S_COY_ID
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', " 'S_ATTN
            strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
            strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "
            'strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', " ' PAYMENT_TYPE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', " ' SHIPMENT_TERM
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', " ' SHIPMENT_MODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("CurrencyCode")) & "', " ' CURRENCY_CODE
            strsql &= Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & ", "  ' EXCHANGE_RATE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', " ' PAYMENT_TERM
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', " ' SHIP_VIA
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillingMethod")) & "', " ' Billing_Method
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', " ' INTERNAL_REMARK
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', " 'EXTERNAL_REMARK
            strsql &= "'" & POStatus_new.Draft & "', " ' PO_STATUS
            strsql &= "'" & Fulfilment.null & "', "
            strsql &= "NULL, '', "   ' PO_INDEX, ARCHIVE_IND
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintCustom")) & "', " 'PRINT_CUSTOM_FIELDS
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintRemark")) & "', " 'PRINT_REMARK
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', " 'POM_SHIP_AMT
            '' ''strsql &= "'" & Common.Parse(strPrefix) & "', "
            strsql &= "" & strPrefix & ", "
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', " ' B_ADDR_CODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', " ' B_ADDR_LINE1
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', " ' B_ADDR_LINE2
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', " ' B_ADDR_LINE3
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', " ' B_POSTCODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', " ' B_STATE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', " ' B_CITY
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', " ' B_COUNTRY
            strsql &= "'', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO
            strsql &= dsPO.Tables(0).Rows(0)("POCost") & ", " ' PO_COST
            strsql &= dsPO.Tables(0).Rows(0)("RfqIndex") & ", " ' RFQ_INDEX

            If strDeptIndex = "" Then ' PRM_DEPT_INDEX
                strsql &= "NULL, "
            Else
                strsql &= strDeptIndex & ", "
            End If
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("QuoNo")) & "' " ' QUOTATION_NO
            strsql &= ", '" & strTermFile & "', '" & Common.Parse(dsPO.Tables(0).Rows(0)("Urgent")) & "', '" & Common.Parse(dsPO.Tables(0).Rows(0)("DeliveryTerm")) & "', '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorCode")) & "' " 'Term & Condition
            strsql &= "FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

            Common.Insert2Ary(strAryQuery, strsql)

            ' PO_DETAILS table
            Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            For i = 0 To dsPO.Tables(1).Rows.Count - 1
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - Include Gst Tax Code
                strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
                strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
                strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_TAX_VALUE, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
                strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
                strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, POD_GST_RATE, POD_GST_INPUT_TAX_CODE, "
                strsql &= "POD_LEAD_TIME, POD_MANUFACTURER, POD_PUR_SPEC_NO, POD_SPEC1, POD_SPEC2, POD_SPEC3, "
                strsql &= "POD_NEXT1_QTY, POD_NEXT2_QTY, POD_NEXT3_QTY, "
                strsql &= "POD_PREV1_QTY, POD_PREV2_QTY, POD_PREV3_QTY, POD_PREV_AVG, POD_CURR_QTY, POD_ITEM_TYPE, POD_OVERSEA, POD_PACKING_TYPE, POD_PACKING_QTY, "
                strsql &= "POD_STOCK_ON_HAND_QTY, POD_PO_BALANCE_QTY, POD_PO_IN_PROGRESS_QTY "
                'If HttpContext.Current.Session("Env") <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO ) SELECT "
                Else
                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE) SELECT "
                End If
                strsql &= "" & strPONo & ", " ' PR_No
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
                strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPO.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPO.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPO.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Remark")) & "', " ' REMARK

                If Not IsDBNull(dsPO.Tables(1).Rows(i)("GST")) Then strsql &= dsPO.Tables(1).Rows(i)("GST") & ", " Else strsql &= "'0.00', " 'Tax
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxAmount")) & "', " 'GST Tax Value

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Source")) & "', " ' SOURCE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SelectedGST")) & "', " ' POD_GST_RATE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' POD_GST_INPUT_TAX_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("LeadTime")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("MfgName")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PurSpecNo")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec1")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec2")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec3")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage1")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage2")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage3")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage4")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage5")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage6")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage7")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage8")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ItemType")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Oversea")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PackingType")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PackingQty")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("StockOnHand")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("POBalance")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("POInProgress")) & "', "
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= IIf(dsPO.Tables(1).Rows(i)("MOQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MOQ")) & ", "
                strsql &= IIf(dsPO.Tables(1).Rows(i)("MPQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MPQ")) & ", "

                'strsql &= dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE") & ", "

                If Common.parseNull(dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE")) = "" Then
                    strsql &= "NULL, "
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE") & ", "
                End If

                If Common.parseNull(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & " " ' PRD_CD_GROUP_INDEX
                End If
                ' _Yap: For Interface
                strsql &= ",'" & Common.parseNull(dsPO.Tables(1).Rows(i)("ItemCode")) & "' " ' PRD_B_ITEM_CODE

                'If HttpContext.Current.Session("Env") <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                    'If Common.Parse(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    '    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                    'Else
                    '    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                    'End If
                    ' _Yap: For Interface
                    'strsql &= "'" & Common.parseNull(dsPO.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                    If Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) <> "" Then
                        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                    Else
                        strsql &= "null, " ' ACCT_INDEX
                    End If
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroup")) & "', "
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroupNo")) & "' "
                End If


                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

            Next

            If modePR = True Then
                ' PR_CUSTOM_FIELD_MSTR table
                For i = 0 To dsPO.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table
                For i = 0 To dsPO.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            If dsPO.Tables(0).Rows(0)("RfqIndex") = "NULL" Then
            Else
                strsql = "UPDATE RFQ_MSTR SET RM_Status = '2' "
                strsql &= "WHERE RM_RFQ_ID = " & dsPO.Tables(0).Rows(0)("RfqIndex")
                Common.Insert2Ary(strAryQuery, strsql)
            End If

            '' update COMPANY_DOC_ATTACHMENT table
            'strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
            'strsql &= "CDA_DOC_NO = " & strPONo & " "
            'strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            ' delete COMPANY_DOC_ATTACHMENT table
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            Common.Insert2Ary(strAryQuery, strsql)

            ' update COMPANY_DOC_ATTACHMENT_TEMP table
            strsql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
            strsql &= "CDA_DOC_NO = " & strPONo & " "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' insert COMPANY_DOC_ATTACHMENT table
            strsql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
            strsql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete COMPANY_DOC_ATTACHMENT_TEMP table
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = " SET @T_NO = " & strPONo & "; "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(strAryQuery, strsql)

            'If objDb.BatchExecute(strAryQuery) Then
            Dim strTPONo As String = ""
            If objDb.BatchExecuteNew(strAryQuery, , strTPONo, "T_NO") Then
                strPONo = strTPONo
                insertPO = WheelMsgNum.Save
            Else
                insertPO = WheelMsgNum.NotSave
            End If
        End Function

        Public Sub updatePO(ByVal dsPO As DataSet, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            Dim strCoyName As String = objDb.GetVal(" SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' ")

            strsql = "UPDATE PO_MSTR SET "
            strsql &= "POM_S_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', "
            strsql &= "POM_S_COY_NAME = '" & Common.Parse(strCoyName) & "', "
            strsql &= "POM_S_ATTN = '" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', "
            'strsql &= "POM_FREIGHT_TERMS = '" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', "
            strsql &= "POM_PAYMENT_METHOD = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', "
            strsql &= "POM_SHIPMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', "
            strsql &= "POM_SHIPMENT_MODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', "
            strsql &= "POM_EXCHANGE_RATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & "', "
            strsql &= "POM_PAYMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', "
            strsql &= "POM_SHIP_VIA = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', "
            strsql &= "POM_INTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', "
            strsql &= "POM_EXTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', "
            strsql &= "POM_PRINT_CUSTOM_FIELDS = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintCustom")) & "', "
            strsql &= "POM_PRINT_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintRemark")) & "', "
            strsql &= "POM_SHIP_AMT = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', "
            strsql &= "POM_B_ADDR_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', "
            strsql &= "POM_B_ADDR_LINE1 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', "
            strsql &= "POM_B_ADDR_LINE2 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', "
            strsql &= "POM_B_ADDR_LINE3 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', "
            strsql &= "POM_B_POSTCODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', "
            strsql &= "POM_B_STATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', "
            strsql &= "POM_B_CITY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', "
            strsql &= "POM_B_COUNTRY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', "
            strsql &= "POM_URGENT = '" & Common.Parse(dsPO.Tables(0).Rows(0)("Urgent")) & "', "
            strsql &= "POM_DEL_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("DeliveryTerm")) & "', "
            strsql &= "POM_CURRENCY_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("CurrencyCode")) & "', "
            strsql &= "POM_VENDOR_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorCode")) & "', "
            strsql &= "POM_PO_COST = " & Common.Parse(dsPO.Tables(0).Rows(0)("POCost")) & " "
            'strsql &= "POM_GST = " & Common.Parse(dsPO.Tables(0).Rows(0)("GST")) & " "
            strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete from PR_DETAILS table
            strsql = "DELETE FROM PO_DETAILS "
            strsql &= "WHERE POD_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Common.Insert2Ary(strAryQuery, strsql)


            ' PO_DETAILS table
            Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            For i = 0 To dsPO.Tables(1).Rows.Count - 1
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - Include GST Tax Code
                strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
                strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
                strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_GST_RATE, POD_GST_INPUT_TAX_CODE, POD_TAX_VALUE, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
                strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
                strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, "
                strsql &= "POD_LEAD_TIME, POD_MANUFACTURER, POD_PUR_SPEC_NO, POD_SPEC1, POD_SPEC2, POD_SPEC3, "
                strsql &= "POD_NEXT1_QTY, POD_NEXT2_QTY, POD_NEXT3_QTY, "
                strsql &= "POD_PREV1_QTY, POD_PREV2_QTY, POD_PREV3_QTY, POD_PREV_AVG, POD_CURR_QTY, POD_ITEM_TYPE, POD_OVERSEA, POD_PACKING_TYPE, POD_PACKING_QTY, "
                strsql &= "POD_STOCK_ON_HAND_QTY, POD_PO_BALANCE_QTY, POD_PO_IN_PROGRESS_QTY "
                'If HttpContext.Current.Session("Env") <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_WARRANTY_TERMS, POD_B_ITEM_CODE, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO ) SELECT "
                Else
                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX) SELECT "
                End If
                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "', " ' PR_No
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
                strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPO.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPO.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPO.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
                If Not IsDBNull(dsPO.Tables(1).Rows(i)("GST")) Then strsql &= dsPO.Tables(1).Rows(i)("GST") & ", " Else strsql &= "'0.00', " ' GST
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SelectedGST")) & "', " 'POD_GST_RATE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxCode")) & "', " 'POD_GST_INPUT_TAX_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxAmount")) & "', " 'POD_GST_TAX Amount
                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Source")) & "', " ' SOURCE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("LeadTime")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("MfgName")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PurSpecNo")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec1")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec2")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Spec3")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage1")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage2")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage3")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage4")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage5")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage6")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage7")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Usage8")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ItemType")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Oversea")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PackingType")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("PackingQty")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("StockOnHand")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("POBalance")) & "', "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("POInProgress")) & "', "
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= IIf(dsPO.Tables(1).Rows(i)("MOQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MOQ")) & ", "
                strsql &= IIf(dsPO.Tables(1).Rows(i)("MPQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MPQ")) & ", "

                If Common.parseNull(dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE")) = "" Then
                    strsql &= "NULL, "
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE") & ", "
                End If

                If Common.parseNull(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & " " ' PRD_CD_GROUP_INDEX
                End If

                'If HttpContext.Current.Session("Env") <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                    'If Common.Parse(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    '    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                    'Else
                    '    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                    'End If
                    strsql &= "'" & Common.parseNull(dsPO.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                    If Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) <> "" Then
                        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                    Else
                        strsql &= "null, " ' ACCT_INDEX
                    End If
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroup")) & "', "
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroupNo")) & "' "
                End If

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

            Next

            ' delete COMPANY_DOC_ATTACHMENT table
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' update COMPANY_DOC_ATTACHMENT_TEMP table
            strsql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
            strsql &= "CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' insert COMPANY_DOC_ATTACHMENT table
            strsql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
            strsql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete COMPANY_DOC_ATTACHMENT_TEMP table
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            If modePR = True Then
                ' delete from PR_CUSTOM_FIELD_MSTR
                strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX IN "
                strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)


                ' PR_CUSTOM_FIELD_MSTR table
                For i = 0 To dsPO.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    'strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= "(SELECT POM_PO_Index FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), "
                    strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' delete from PR_CUSTOM_FIELD_DETAILS
                strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX IN "
                strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)

                ' PR_CUSTOM_FIELD_DETAILS table
                For i = 0 To dsPO.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    'strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= "(SELECT POM_PO_Index FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), "
                    strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            objDb.BatchExecute(strAryQuery)
        End Sub

        Public Function getVendorList(ByVal aryVendor As ArrayList, ByVal aryProd As ArrayList, Optional ByVal strVendorSelected As String = "", Optional ByVal strPONo As String = "", Optional ByVal sPROD As String = "", Optional ByVal sPOMode As String = "", Optional ByVal sCurr As String = "") As DataView
            Dim drw As DataView
            'Dim aryVendorList As New ArrayList
            Dim strVendList As String = ""
            Dim strProdList As String = ""
            Dim i, k As Integer
            Dim objDB As New EAD.DBCom

            For i = 0 To aryVendor.Count - 1
                If strVendList = "" Then
                    strVendList = "'" & aryVendor(i) & "'"
                Else
                    strVendList &= ", '" & aryVendor(i) & "'"
                End If
            Next

            If sPOMode <> "cc" Then
                For i = 0 To aryProd.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProd(i) & "'"
                    Else
                        strProdList &= ", '" & aryProd(i) & "'"
                    End If
                Next
            Else
                For i = 0 To aryProd.Count - 1
                    For k = 0 To aryProd.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProd(i)(k) & "'"
                        Else
                            strProdList &= ", '" & aryProd(i)(k) & "'"
                        End If
                    Next
                Next
            End If

            Dim strSQL As String
            If strPONo <> "" Then
                strSQL = "SELECT POD_PRODUCT_CODE FROM po_details WHERE POD_PO_NO = '" & strPONo & "' "
                Dim tDS As DataSet = objDB.FillDs(strSQL)
                Dim sProductCode As String = ""

                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    sProductCode = sProductCode & "'" & tDS.Tables(0).Rows(j).Item("POD_PRODUCT_CODE") & "', "
                Next j

                If sProductCode = "" Then
                    sProductCode = sPROD
                Else
                    sProductCode = Mid(sProductCode, 1, Len(sProductCode) - 2)
                End If

                If strVendorSelected <> "" Then
                    strSQL = "SELECT DISTINCT(PV_SUPP_CODE) AS CM_COY_ID, PV_SUPP_CODE AS CM_COY_NAME FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendorSelected & "' " &
                                        " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & sProductCode & ")) " &
                                        " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) "
                Else
                    strSQL = "SELECT DISTINCT(PV_SUPP_CODE) AS CM_COY_ID, PV_SUPP_CODE AS CM_COY_NAME FROM PIM_VENDOR WHERE PV_S_COY_ID IN (" & strVendList & ") " &
                        " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & sProductCode & ")) " &
                        " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) "
                End If
            Else
                Dim strCurr As String = ""
                If strVendorSelected <> "" Then
                    If sPOMode <> "cc" Then
                        For i = 0 To aryProd.Count - 1
                            If strCurr = "" Then
                                strCurr = objDB.GetVal("SELECT IFNULL(GROUP_CONCAT('''', PV_CURR, ''''), '') AS PV_CURR FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendorSelected & "' " &
                                        " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & aryProd(i) & ")) " &
                                        " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) ")
                            Else
                                strCurr = objDB.GetVal("SELECT IFNULL(GROUP_CONCAT('''', PV_CURR, ''''), '') AS PV_CURR FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendorSelected & "' " &
                                        " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & aryProd(i) & ")) " &
                                        " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) AND PV_CURR IN(" & strCurr & ") ")
                            End If
                        Next
                    Else
                        strCurr = "'" & sCurr & "'"
                    End If

                    If strCurr <> "" Then
                        strSQL = "SELECT DISTINCT(PV_SUPP_CODE) AS CM_COY_ID, PV_SUPP_CODE AS CM_COY_NAME FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendorSelected & "' " &
                                " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & strProdList & ")) " &
                                " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) AND PV_CURR IN (" & strCurr & ") "
                    Else
                        Return Nothing
                    End If

                Else
                    If strCurr <> "" Then
                        strSQL = "SELECT DISTINCT(PV_SUPP_CODE) AS CM_COY_ID, PV_SUPP_CODE AS CM_COY_NAME FROM PIM_VENDOR WHERE PV_S_COY_ID IN (" & strVendList & ") " &
                            " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & strProdList & ")) " &
                            " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) AND PV_CURR IN (" & strCurr & ") "
                    Else
                        Return Nothing
                    End If
                End If
            End If

            drw = objDB.GetView(strSQL)
            Return drw
        End Function

        Public Function checkCommonPayTDelT(ByVal aryVendor As ArrayList, ByVal aryProd As ArrayList, Optional ByVal strVendorSelected As String = "", Optional ByVal strPONo As String = "", Optional ByVal sPROD As String = "", Optional ByVal sPOMode As String = "", Optional ByVal sCurr As String = "", Optional ByVal strVendorCodeSelected As String = "", Optional ByVal strPaymentSelected As String = "", Optional ByVal strDeliverySelected As String = "") As DataView
            Dim drw As DataView
            'Dim aryVendorList As New ArrayList
            Dim strVendList As String = ""
            Dim strProdList As String = ""
            Dim i, k As Integer
            Dim objDB As New EAD.DBCom

            For i = 0 To aryVendor.Count - 1
                If strVendList = "" Then
                    strVendList = "'" & aryVendor(i) & "'"
                Else
                    strVendList &= ", '" & aryVendor(i) & "'"
                End If
            Next

            If sPOMode <> "cc" Then
                For i = 0 To aryProd.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProd(i) & "'"
                    Else
                        strProdList &= ", '" & aryProd(i) & "'"
                    End If
                Next
            Else
                For i = 0 To aryProd.Count - 1
                    For k = 0 To aryProd.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProd(i)(k) & "'"
                        Else
                            strProdList &= ", '" & aryProd(i)(k) & "'"
                        End If
                    Next
                Next
            End If

            strVendorCodeSelected = IIf(strVendorCodeSelected = "---Select---", "", strVendorCodeSelected)
            strPaymentSelected = IIf(strPaymentSelected = "---Select---", "", strPaymentSelected)
            strDeliverySelected = IIf(strDeliverySelected = "---Select---", "", strDeliverySelected)

            Dim strSQL As String

            If strVendorSelected <> "" Then
                strSQL = "SELECT DISTINCT(PV_SUPP_CODE) AS CM_COY_ID, PV_SUPP_CODE AS CM_COY_NAME FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendorSelected & "' " &
                       " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & sPROD & "') " &
                       " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL AND PV_SUPP_CODE = '" & strVendorCodeSelected & "' ) " &
                       " AND PV_DELIVERY_TERM = '" & Common.Parse(strDeliverySelected) & "' AND PV_PAYMENT_CODE = '" & Common.Parse(strPaymentSelected) & "' "
            End If

            drw = objDB.GetView(strSQL)
            Return drw
        End Function

        Public Function GetCurr(ByVal aryProd As ArrayList, ByVal strVendor As String, ByVal strVendorCode As String, Optional ByVal sPOMode As String = "") As String
            Dim strSQL As String
            Dim ds1 As DataSet
            Dim strProdList As String = ""
            Dim i, k As Integer

            'For i = 0 To aryProd.Count - 1
            '    If strProdList = "" Then
            '        strProdList = "'" & aryProd(i) & "'"
            '    Else
            '        strProdList &= ", '" & aryProd(i) & "'"
            '    End If
            'Next

            If sPOMode <> "cc" Then
                For i = 0 To aryProd.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProd(i) & "'"
                    Else
                        strProdList &= ", '" & aryProd(i) & "'"
                    End If
                Next
            Else
                For i = 0 To aryProd.Count - 1
                    For k = 0 To aryProd.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProd(i)(k) & "'"
                        Else
                            strProdList &= ", '" & aryProd(i)(k) & "'"
                        End If
                    Next
                Next
            End If

            strSQL = "SELECT PV_CURR FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendor & "' " &
                    " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & strProdList & ")) " &
                    " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) " &
                    " AND PV_SUPP_CODE = '" & strVendorCode & "' "

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                GetCurr = Common.parseNull(ds1.Tables(0).Rows(0).Item("PV_CURR"))
            Else
                GetCurr = "N/A"
            End If

            ds1 = Nothing
        End Function

        Public Function GetDelivery(ByVal aryProd As ArrayList, ByVal strVendor As String, ByVal strVendorCode As String, Optional ByVal sPOMode As String = "") As String
            Dim strSQL As String
            Dim ds1 As DataSet
            Dim strProdList As String = ""
            Dim i, k As Integer

            If sPOMode <> "cc" Then
                For i = 0 To aryProd.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProd(i) & "'"
                    Else
                        strProdList &= ", '" & aryProd(i) & "'"
                    End If
                Next
            Else
                For i = 0 To aryProd.Count - 1
                    For k = 0 To aryProd.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProd(i)(k) & "'"
                        Else
                            strProdList &= ", '" & aryProd(i)(k) & "'"
                        End If
                    Next
                Next
            End If

            strSQL = "SELECT PV_DELIVERY_TERM FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendor & "' " &
                    " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & strProdList & ")) " &
                    " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) " &
                    " AND PV_SUPP_CODE = '" & strVendorCode & "' "

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                GetDelivery = Common.parseNull(ds1.Tables(0).Rows(0).Item("PV_DELIVERY_TERM"))
            Else
                GetDelivery = "N/A"
            End If

            ds1 = Nothing
        End Function

        Public Function GetPayment(ByVal aryProd As ArrayList, ByVal strVendor As String, ByVal strVendorCode As String, Optional ByVal sPOMode As String = "") As String
            Dim strSQL As String
            Dim ds1 As DataSet
            Dim strProdList As String = ""
            Dim i, k As Integer

            If sPOMode <> "cc" Then
                For i = 0 To aryProd.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProd(i) & "'"
                    Else
                        strProdList &= ", '" & aryProd(i) & "'"
                    End If
                Next
            Else
                For i = 0 To aryProd.Count - 1
                    For k = 0 To aryProd.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProd(i)(k) & "'"
                        Else
                            strProdList &= ", '" & aryProd(i)(k) & "'"
                        End If
                    Next
                Next
            End If

            strSQL = "SELECT PV_PAYMENT_CODE FROM PIM_VENDOR WHERE PV_S_COY_ID = '" & strVendor & "' " &
                    " AND PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & strProdList & ")) " &
                    " AND (PV_SUPP_CODE <> '' AND PV_SUPP_CODE IS NOT NULL) " &
                    " AND PV_SUPP_CODE = '" & strVendorCode & "' "

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                GetPayment = Common.parseNull(ds1.Tables(0).Rows(0).Item("PV_PAYMENT_CODE"))
            Else
                GetPayment = "N/A"
            End If

            ds1 = Nothing
        End Function

        Function getPrice(ByVal strQty As String, ByVal strProdCode As String, ByVal strVendorType As String) As String
            Dim strSQL As String
            Dim objDB As New EAD.DBCom
            Dim i As Integer

            strSQL = " SELECT PVP_VOLUME, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE WHERE PVP_PRODUCT_CODE = '" & strProdCode & "' and PVP_VENDOR_TYPE = '" & strVendorType & "' ORDER BY PVP_VOLUME "

            Dim tDS As DataSet = objDB.FillDs(strSQL)

            If IsNumeric(strQty) Then
                For i = 0 To tDS.Tables(0).Rows.Count - 1
                    If tDS.Tables(0).Rows(i).Item("PVP_VOLUME") > CDec(strQty) Then
                        Exit For
                    End If
                Next
            Else
                getPrice = "0.00"
            End If
            If tDS.Tables(0).Rows.Count > 0 Then
                If i = 0 Then
                    getPrice = tDS.Tables(0).Rows(i).Item("PVP_VOLUME_PRICE")
                Else
                    getPrice = tDS.Tables(0).Rows(i - 1).Item("PVP_VOLUME_PRICE")
                End If
            Else
                getPrice = "0.00"
            End If

        End Function

        Function get_EDDPerc(ByVal pProdCode As String, ByVal pCompID As String) As String
            Dim strsql As String, iEDD As String = ""
            Dim ds As New DataSet
            Dim INDEX As String
            strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.parseNull(pProdCode) & "'"

            INDEX = objDb.GetVal(strsql)

            strsql = " SELECT IFNULL(MAX(PV_LEAD_TIME), 0) AS PV_LEAD_TIME FROM PRODUCT_MSTR, PIM_VENDOR "
            strsql &= " WHERE PM_PRODUCT_INDEX = PV_PRODUCT_INDEX  AND PV_PRODUCT_INDEX= '" & INDEX & "' AND "
            strsql &= " PV_S_COY_ID = '" & Common.parseNull(pCompID) & "' "

            ds = objDb.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
            End If

            If iEDD = "" Then
                Return "0"
            Else
                Return iEDD
            End If
        End Function
    End Class
End Namespace
