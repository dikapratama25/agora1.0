Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class PR_Ext
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals

        Public Function insertPR(ByVal dsPR As DataSet, ByRef strPRNo As String, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True) As Integer
            Dim strPrefix As String
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i, j As Integer
            Dim strDeptIndex As String = ""
            Dim intIncrementNo As Integer = 0

            strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PR' "
            Common.Insert2Ary(strAryQuery, strsql)

            'GetLatestDocNo
            '' ''objGlobal.GetLatestDocNo("PR", strAryQuery, strPRNo, strPrefix)

            intIncrementNo = 1

            strPRNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PR' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            strPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PR') "

            strsql = "SELECT * FROM PR_MSTR WHERE PRM_PR_NO = " & strPRNo & " "
            strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If objDb.Exist(strsql) > 0 Then
                insertPR = WheelMsgNum.Duplicate
                Exit Function
            End If

            ' to check whether vendor company is inactive
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
            strsql &= "AND CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPR = -1
                Exit Function
            End If

            For Each dr As DataRow In dsPR.Tables(1).Rows
                'to check whether vendor company is being deleted
                strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
                strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPR.Tables(1).Rows(0)("VendorID")) & "' "
                If objDb.Exist(strsql) > 0 Then
                    insertPR = -2
                    Exit Function
                End If
            Next

            ' get dept index
            strsql = "SELECT CDM_DEPT_INDEX FROM USER_MSTR "
            strsql &= "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
            strsql &= "AND UM_COY_ID = CDM_COY_ID "
            strsql &= "WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strsql &= "AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
            Next

            ' PR_MSTR table
            strsql = "INSERT INTO PR_MSTR (PRM_PR_NO, PRM_COY_ID, PRM_REQ_NAME, PRM_REQ_PHONE, "
            strsql &= "PRM_PR_DATE, PRM_CREATED_DATE, PRM_STATUS_CHANGED_BY, PRM_STATUS_CHANGED_ON, "
            strsql &= "PRM_BUYER_ID, PRM_S_COY_ID, PRM_S_ATTN, PRM_S_COY_NAME, PRM_S_ADDR_LINE1, "
            strsql &= "PRM_S_ADDR_LINE2, PRM_S_ADDR_LINE3, PRM_S_POSTCODE, PRM_S_CITY, PRM_S_STATE, "
            strsql &= "PRM_S_COUNTRY, PRM_S_PHONE, PRM_S_FAX, PRM_S_EMAIL, PRM_FREIGHT_TERMS, "
            strsql &= "PRM_PAYMENT_TYPE, PRM_SHIPMENT_TERM, PRM_SHIPMENT_MODE, PRM_CURRENCY_CODE, "
            strsql &= "PRM_EXCHANGE_RATE, PRM_PAYMENT_TERM, PRM_SHIP_VIA, PRM_INTERNAL_REMARK, "
            strsql &= "PRM_EXTERNAL_REMARK, PRM_PR_STATUS, PRM_PO_INDEX, PRM_ARCHIVE_IND, "
            strsql &= "PRM_PRINT_CUSTOM_FIELDS, PRM_PRINT_REMARK, PRM_PR_PREFIX, PRM_B_ADDR_CODE, "
            strsql &= "PRM_B_ADDR_LINE1, PRM_B_ADDR_LINE2, PRM_B_ADDR_LINE3, PRM_B_POSTCODE, "
            strsql &= "PRM_B_STATE, PRM_B_CITY, PRM_B_COUNTRY, "
            strsql &= "PRM_DUP_FROM, PRM_EXTERNAL_IND, PRM_REFERENCE_NO, PRM_CONSOLIDATOR, PRM_GST, "
            strsql &= "PRM_PR_COST, PRM_RFQ_INDEX, PRM_DEPT_INDEX, PRM_QUOTATION_NO, PRM_PR_TYPE, PRM_URGENT) SELECT "
            strsql &= "" & strPRNo & ", " ' PR_No
            strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) <> "" Then
                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', " ' REQ_NAME
            Else
                Dim PR_Req_Name As String = objDb.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                strsql &= "'" & Common.Parse(PR_Req_Name) & "', " ' REQ_NAME
            End If

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) <> "" Then
                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', " 'REQ_PHONE
            Else
                Dim PR_Req_Tel As String = objDb.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                strsql &= "'" & Common.Parse(PR_Req_Tel) & "', " 'REQ_PHONE
            End If

            strsql &= "NULL, " 'Common.ConvertDate(Now) & ", "  'PR_DATE
            strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', " 'STATUS_CHANGED_BY
            strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
            strsql &= "'" & HttpContext.Current.Session("UserId") & "', " 'BUYER_ID

            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("VendorID")) & "', " 'S_COY_ID
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("Attn")) & "', " 'S_ATTN
            strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
            strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "

            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentType")) & "', " ' PAYMENT_TYPE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipmentTerm")) & "', " ' SHIPMENT_TERM
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipmentMode")) & "', " ' SHIPMENT_MODE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("CurrencyCode")) & "', " ' CURRENCY_CODE
            strsql &= "NULL, " 'Common.Parse(dsPR.Tables(0).Rows(0)("ExchangeRate")) & ", "  ' EXCHANGE_RATE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentTerm")) & "', " ' PAYMENT_TERM
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipVia")) & "', " ' SHIP_VIA
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("InternalRemark")) & "', " ' INTERNAL_REMARK
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ExternalRemark")) & "', " 'EXTERNAL_REMARK
            strsql &= "'" & PRStatus.Draft & "', " ' PR_STATUS
            strsql &= "NULL, '', " ' PO_INDEX, ARCHIVE_IND
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintCustom")) & "', " 'PRINT_CUSTOM_FIELDS
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintRemark")) & "', " 'PRINT_REMARK
            strsql &= "" & strPrefix & ", "
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrCode")) & "', " ' B_ADDR_CODE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrLine1")) & "', " ' B_ADDR_LINE1
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrLine2")) & "', " ' B_ADDR_LINE2
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrLine3")) & "', " ' B_ADDR_LINE3
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrPostCode")) & "', " ' B_POSTCODE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrState")) & "', " ' B_STATE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrCity")) & "', " ' B_CITY
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrCountry")) & "', " ' B_COUNTRY
            strsql &= "'', '', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO, PRM_CONSOLIDATOR
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("GST")) & "', " ' PRM_GST
            strsql &= dsPR.Tables(0).Rows(0)("PRCost") & ", " ' PRM_PR_COST
            strsql &= "NULL, " 'dsPR.Tables(0).Rows(0)("RfqIndex") & ", " ' PRM_RFQ_INDEX

            If strDeptIndex = "" Then ' PRM_DEPT_INDEX
                strsql &= "NULL, "
            Else
                strsql &= strDeptIndex & ", "
            End If
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("QuoNo")) & "' " ' PRM_QUOTATION_NO
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PRType")) & "',  '" & Common.Parse(dsPR.Tables(0).Rows(0)("Urgent")) & "' "
            strsql &= "FROM COMPANY_MSTR " ', USER_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

            Common.Insert2Ary(strAryQuery, strsql)

            ' PR_DETAILS table
            For i = 0 To dsPR.Tables(1).Rows.Count - 1
                strsql = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, "
                strsql &= "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, "
                strsql &= "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, PRD_GST_RATE, PRD_GST_INPUT_TAX_CODE, PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, "
                strsql &= "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, "

                If blnEnterpriseVersion = True Then
                    strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP, PRD_LEAD_TIME, PRD_ITEM_TYPE, PRD_DEL_CODE, PRD_OVERSEA) SELECT "
                Else
                    strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_LEAD_TIME, PRD_ITEM_TYPE, PRD_DEL_CODE, PRD_OVERSEA) SELECT "
                End If

                strsql &= "" & strPRNo & ", " ' PR_No
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
                strsql &= Common.Parse(dsPR.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPR.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPR.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPR.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= dsPR.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                strsql &= "1, 1, " 'PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
                strsql &= dsPR.Tables(1).Rows(i)("GST") & ", " ' GST
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTRate")) & "', " ' GST_RATE
                'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' GST Input Tax Code

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "

                If Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) <> "" Then '_Yap
                    strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                Else
                    strsql &= "null, " ' ACCT_INDEX
                End If

                ' strsql &= "NULL, " ' ACCT_INDEX '_Yap
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Source")) & "', " ' SOURCE

                If Common.Parse(dsPR.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPR.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                End If

                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
                'strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "' " ' PRD_S_COY_ID
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorID")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Commodity")) & "', "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Currency")) & "' "

                If blnEnterpriseVersion = True Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("AssetGroup")) & "' "
                End If

                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("LeadTime")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemType")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryTerm")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("Oversea")) & "' "

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If modePR = True Then
                ' PR_CUSTOM_FIELD_MSTR table 
                For i = 0 To dsPR.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    strsql &= "(SELECT MAX(PRM_PR_Index) FROM PR_MSTR), "
                    strsql &= dsPR.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(2).Rows(i)("FieldName")) & "', " ' FIELD_NAME
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table 
                For i = 0 To dsPR.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    strsql &= "(SELECT MAX(PRM_PR_Index) FROM PR_MSTR), "
                    strsql &= dsPR.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPR.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(3).Rows(i)("FieldValue")) & "', " ' FIELD_VALUE
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            ' update COMPANY_DOC_ATTACHMENT table
            strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
            strsql &= "CDA_DOC_NO = " & strPRNo & " "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PR' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = " SET @T_NO = " & strPRNo & "; "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'PR' "
            Common.Insert2Ary(strAryQuery, strsql)

            'If objDb.BatchExecute(strAryQuery) Then
            Dim strTPRNo As String = ""
            If objDb.BatchExecuteNew(strAryQuery, , strTPRNo, "T_NO") Then
                strPRNo = strTPRNo
                insertPR = WheelMsgNum.Save
            Else
                insertPR = WheelMsgNum.NotSave
            End If
        End Function

        Public Function updatePR(ByVal dsPR As DataSet, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strsql = "UPDATE PR_MSTR SET "
            'strsql &= "PRM_REQ_NAME = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', "

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) <> "" Then
                strsql &= "PRM_REQ_NAME = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', " ' REQ_NAME
            Else
                Dim PR_Req_Name As String = objDb.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                strsql &= "PRM_REQ_NAME = '" & Common.Parse(PR_Req_Name) & "', " ' REQ_NAME
            End If

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) <> "" Then
                strsql &= "PRM_REQ_PHONE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', " 'REQ_PHONE
            Else
                Dim PR_Req_Tel As String = objDb.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                strsql &= "PRM_REQ_PHONE = '" & Common.Parse(PR_Req_Tel) & "', " 'REQ_PHONE
            End If

            'strsql &= "PRM_REQ_PHONE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', "
            'strsql &= "PRM_PR_DATE = " & Common.ConvertDate(Now) & ", "
            strsql &= "PRM_S_ATTN = '" & Common.Parse(dsPR.Tables(0).Rows(0)("Attn")) & "', "
            strsql &= "PRM_INTERNAL_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("InternalRemark")) & "', "
            strsql &= "PRM_EXTERNAL_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ExternalRemark")) & "', "
            strsql &= "PRM_PRINT_CUSTOM_FIELDS = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintCustom")) & "', "
            strsql &= "PRM_PRINT_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintRemark")) & "', "
            strsql &= "PRM_URGENT = '" & Common.Parse(dsPR.Tables(0).Rows(0)("Urgent")) & "', "
            strsql &= "PRM_PR_COST = " & Common.Parse(dsPR.Tables(0).Rows(0)("PRCost")) & ", "

            strsql &= "PRM_PAYMENT_TYPE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentType")) & "', "
            strsql &= "PRM_PAYMENT_TERM = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentTerm")) & "', "
            strsql &= "PRM_CURRENCY_CODE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("CurrencyCode")) & "' "

            strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
            strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete from PR_DETAILS table
            strsql = "DELETE FROM PR_DETAILS "
            strsql &= "WHERE PRD_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
            strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' PR_DETAILS table

            For i = 0 To dsPR.Tables(1).Rows.Count - 1

                strsql = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, "
                strsql &= "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, "
                strsql &= "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, PRD_GST_RATE, PRD_GST_INPUT_TAX_CODE, PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, "
                strsql &= "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, "

                If blnEnterpriseVersion = True Then
                    strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP, PRD_LEAD_TIME, PRD_ITEM_TYPE, PRD_DEL_CODE, PRD_OVERSEA) SELECT "
                Else
                    strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_LEAD_TIME, PRD_ITEM_TYPE, PRD_DEL_CODE, PRD_OVERSEA) SELECT "
                End If

                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "', " ' PR_No
                strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
                strsql &= Common.Parse(dsPR.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPR.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPR.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPR.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= dsPR.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                strsql &= "1, 1, " 'PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
                strsql &= dsPR.Tables(1).Rows(i)("GST") & ", " ' GST
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTRATE")) & "', " ' GSTRATE
                'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' GSTTaxCode

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "

                If Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) <> "" Then '_Yap
                    strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                Else
                    strsql &= "null, " ' ACCT_INDEX
                End If

                'strsql &= "NULL, " ' ACCT_INDEX '_Yap
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Source")) & "', " ' SOURCE

                If Common.Parse(dsPR.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPR.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                End If

                'strsql &= "NULL, " 'PRD_CD_GROUP_INDEX
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE

                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorID")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Commodity")) & "', "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Currency")) & "' "

                If blnEnterpriseVersion = True Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("AssetGroup")) & "' "
                End If

                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("LeadTime")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemType")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryTerm")) & "' "
                strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("Oversea")) & "' "

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If modePR = True Then
                ' delete from PR_CUSTOM_FIELD_DETAILS
                strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PR' AND PCD_PR_INDEX IN "
                strsql &= "(SELECT PRM_PR_INDEX FROM PR_MSTR "
                strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)

                ' delete from PR_CUSTOM_FIELD_MSTR
                strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PR' AND PCM_PR_INDEX IN "
                strsql &= "(SELECT PRM_PR_INDEX FROM PR_MSTR "
                strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)

                ' PR_CUSTOM_FIELD_MSTR table
                For i = 0 To dsPR.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    strsql &= "(SELECT PRM_PR_Index FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                    strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), "
                    strsql &= dsPR.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(2).Rows(i)("FieldName")) & "', " ' FIELD_NAME
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table
                For i = 0 To dsPR.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    strsql &= "(SELECT PRM_PR_Index FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                    strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), "
                    strsql &= dsPR.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPR.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(3).Rows(i)("FieldValue")) & "', " ' FIELD_VALUE
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If
            objDb.BatchExecute(strAryQuery)
        End Function
    End Class
End Namespace