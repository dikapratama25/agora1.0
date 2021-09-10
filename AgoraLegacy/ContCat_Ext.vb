Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy

Namespace AgoraLegacy
    Public Class ContCat_Ext

        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Dim objGlobal As New AppGlobals

        Public Function getSingleProduct(ByVal strCode As String, ByVal blnTemp As Boolean) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If blnTemp Then
                strsql = "SELECT PM_PRODUCT_INDEX, PM_S_COY_ID, PM_PRODUCT_CODE, CT_NAME, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC, "
                strsql &= "PM_REF_NO,PM_LONG_DESC, PM_CATEGORY_NAME,PM_GST_CODE, "
                strsql &= "PM_TAX_ID,"
                strsql &= objDb.Concat3(" ", "%", "PM_GST_CODE", "PM_TAX_PERC") & " AS TAX_CODE,"
                strsql &= "PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE,"
                strsql &= "PM_PRODUCT_IMAGE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO,PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT, "
                strsql &= "PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_PRODUCT_TYPE, "
                strsql &= "PM_STATUS, PM_REMARK, PM_REMARKS, PM_ACTION, CM_COY_NAME "
                strsql &= "FROM PRODUCT_MSTR_TEMP "
                strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
                strsql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID "
                strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
            Else
                strsql = "SELECT PM_PRODUCT_INDEX, PM_S_COY_ID, PM_PRODUCT_CODE, CT_NAME, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC, "
                strsql &= "PM_REF_NO,PM_LONG_DESC, PM_CATEGORY_NAME,PM_GST_CODE, "
                strsql &= "PM_TAX_ID,"
                strsql &= objDb.Concat3(" ", "%", "PM_GST_CODE", "PM_TAX_PERC") & " AS TAX_CODE,"
                strsql &= "PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_PREFER_S_COY_ID_TAX_ID, PM_1ST_S_COY_ID_TAX_ID, PM_2ND_S_COY_ID_TAX_ID, PM_3RD_S_COY_ID_TAX_ID, "
                strsql &= "PM_PRODUCT_IMAGE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO,PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT, "
                strsql &= "PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_PRODUCT_TYPE, "
                strsql &= " '' as PM_STATUS, PM_REMARK, PM_REMARKS, e.CM_COY_NAME, CT_NAME, PM_ACCT_CODE, PM_SAFE_QTY, PM_ORD_MIN_QTY, PM_ORD_MAX_QTY, PM_DELETED, "
                strsql &= "PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID,  a.CM_COY_NAME AS S_COY_NAME_P,b.CM_COY_NAME AS S_COY_NAME_1,c.CM_COY_NAME AS S_COY_NAME_2,d.CM_COY_NAME AS S_COY_NAME_3, "
                strsql &= "PM_ITEM_TYPE, PM_IQC_IND, PM_MAX_INV_QTY, PM_MANUFACTURER, PM_CAT_CODE, PM_OVERSEA, PM_IQC_TYPE, "
                strsql &= "PM_EOQ, PM_RATIO, PM_PARTIAL_CD, PM_REORDER_QTY, PM_BUDGET_PRICE, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_PACKING_TYPE, "
                strsql &= "PM_PACKING_QTY, PM_MANUFACTURER2, PM_MANUFACTURER3,PM_SECTION, PM_LOCATION, PM_NEW_ITEM_CODE "
                strsql &= "FROM PRODUCT_MSTR "
                strsql &= "LEFT JOIN COMPANY_MSTR e ON PM_S_COY_ID = e.CM_COY_ID AND e.CM_DELETED <> 'Y' AND e.CM_STATUS = 'A' "
                strsql &= "LEFT JOIN COMPANY_MSTR a ON PM_PREFER_S_COY_ID = a.CM_COY_ID LEFT JOIN COMPANY_MSTR b ON PM_1ST_S_COY_ID = b.CM_COY_ID LEFT JOIN COMPANY_MSTR c ON PM_2ND_S_COY_ID = c.CM_COY_ID LEFT JOIN COMPANY_MSTR d ON PM_3RD_S_COY_ID = d.CM_COY_ID "
                strsql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID "
                strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
            End If

            ds = objDb.FillDs(strsql)
            getSingleProduct = ds

        End Function

        Public Sub GetLatestItemNo(ByRef strDocNo As String)
            Dim strSql, strPrefix, strLastUsedNo, strReplicate As String
            Dim intLeadingZero, intLen As Integer
            Dim ds As New DataSet

            strLastUsedNo = "0"
            strPrefix = ""
            strSql = "SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_NAME = 'ItemPrefix'; "
            strSql &= "SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_NAME = 'ItemLastUsedNo';"

            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                strPrefix = ds.Tables(0).Rows(0)(0)
            End If

            If ds.Tables(1).Rows.Count > 0 Then
                strLastUsedNo = ds.Tables(1).Rows(0)(0)
            End If

            If strLastUsedNo.Substring(0, 1) = "0" Then
                intLen = strLastUsedNo.Length
                strLastUsedNo = Convert.ToInt32(strLastUsedNo) + 1
                intLeadingZero = intLen - strLastUsedNo.Length
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            Else
                strLastUsedNo = Convert.ToInt32(strLastUsedNo) + 1
                intLeadingZero = 0
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            End If

            strDocNo = strPrefix & strReplicate & strLastUsedNo
        End Sub

        ' inserted by Hub Admin

        Public Function BIM(ByVal dsProduct As DataSet, ByVal strMode As String, ByVal strImageIndex As String, ByRef strNewProductCode As String, ByVal OldVendor As String, ByVal aryVendor As ArrayList, Optional ByVal OldVendorItemCode As String = "", Optional ByVal OldItemName As String = "", Optional ByVal strUpload As String = "", Optional ByVal aryPrice As ArrayList = Nothing, Optional ByVal blnGST As Boolean = False) As String
            Dim strsql, strsql2 As String
            Dim strProductCode As String
            Dim strAryQuery(0) As String
            Dim tempNull As String
            Dim i, j As Integer
            Dim dtr As DataRow

            Dim docFileName As String
            Dim imgFileName As String
            Dim docFileName2 As String

            Dim INDEX As String


            Select Case strMode
                Case "add"
                    GetLatestItemNo(strProductCode)
                    strNewProductCode = strProductCode

                    strsql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    strsql = "INSERT INTO PRODUCT_MSTR (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
                    strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_ACCT_CODE, PM_UOM,"
                    strsql &= "PM_SAFE_QTY,PM_ORD_MIN_QTY,PM_ORD_MAX_QTY,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
                    strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
                    strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_REMARKS, "
                    strsql &= "PM_DELETED,PM_PRODUCT_FOR, PM_ENT_BY, PM_ENT_DT, "
                    strsql &= "PM_ITEM_TYPE, PM_IQC_IND, PM_MAX_INV_QTY, PM_MANUFACTURER, PM_CAT_CODE,"
                    strsql &= "PM_REORDER_QTY, PM_BUDGET_PRICE, PM_IQC_TYPE, PM_EOQ, PM_RATIO, PM_OVERSEA, PM_PARTIAL_CD,"
                    strsql &= "PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_PACKING_TYPE, PM_PACKING_QTY, PM_MANUFACTURER2, PM_MANUFACTURER3,"
                    strsql &= "PM_SECTION, PM_LOCATION, PM_NEW_ITEM_CODE) VALUES ("
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
                    strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("AccCode")) & "', " ' PM_ACCT_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("MinInv")) & "', " ' SAFE_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Min")) & "', " ' PM_ORD_MIN_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Max")) & "', " ' PM_ORD_MAX_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) & "', " ' PM_DELETED
                    strsql &= "'B', '" & HttpContext.Current.Session("UserId") & "', getdate(), " ' PM_PRODUCT_FOR
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd2")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("MaxInvQty")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CatCode")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("RQL")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("BudgetPrice")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("IQCType")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("EOQ")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Ratio")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd3")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PartialDelivery")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec2")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec3")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PackType")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PackQty")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu2")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu3")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("SectionCode")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LocationCode")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NewItemCode")) & "') "

                    Common.Insert2Ary(strAryQuery, strsql)

                    For i = 0 To aryVendor.Count - 1
                        If Common.Parse(aryVendor(i)(2)) <> "" Then
                            strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, "
                            strsql &= "PV_S_COY_ID, "

                            If blnGST = False Then
                                strsql &= "PV_S_COY_ID_TAX_ID, "
                            End If

                            strsql &= "PV_LEAD_TIME, PV_VENDOR_CODE, PV_DELIVERY_TERM, PV_SUPP_CODE, PV_PAYMENT_CODE, PV_CURR, "
                            strsql &= "PV_PUR_SPEC_NO, PV_REVISION, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                            strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
                            strsql &= "'" & Common.Parse(aryVendor(i)(1)) & "', " 'Vendor Type
                            strsql &= "'" & Common.Parse(aryVendor(i)(3)) & "', " 'Vendor ID

                            If blnGST = False Then
                                strsql &= "'" & Common.Parse(aryVendor(i)(10)) & "', " 'Tax ID
                            End If

                            If Common.Parse(aryVendor(i)(11)) = "" Then
                                tempNull = "NULL"
                            Else
                                tempNull = "'" & Common.Parse(aryVendor(i)(11)) & "'"
                            End If

                            strsql &= "" & tempNull & ", " '
                            strsql &= "'" & Common.Parse(aryVendor(i)(12)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(5)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(4)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(6)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(7)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(8)) & "', "
                            strsql &= "'" & Common.Parse(aryVendor(i)(9)) & "', "
                            strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                            strsql &= "getdate()) "

                            Common.Insert2Ary(strAryQuery, strsql)
                        End If
                    Next

                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'D'"
                    docFileName = objDb.GetVal(strsql)

                    'For Document attachment
                    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                    strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    strsql = "UPDATE PRODUCT_ATTACHMENT SET PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'InsertAuditTrailBIM("" & INDEX & "", "Item Code", "Modify", "File Attachment", "''", "'" & docFileName & "'", strAryQuery)

                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
                    strsql &= "AND PA_TYPE = 'D' AND PA_STATUS = 'T' AND PA_SOURCE = 'B' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    If docFileName <> "" Then
                        InsertAuditTrailBIM(0, "Item Master", "Add", "File Attachment", "", "" & docFileName & "", strAryQuery, True)
                    End If

                    ''Insert Volume Price
                    If Not aryPrice Is Nothing Then
                        InsertUnitPrice(strMode, strProductCode, aryPrice, strAryQuery)
                    End If

                Case "mod"
                    'Michelle (14/5/2011) - To update the inventory mstr if for user change the Item code or name
                    If OldVendorItemCode <> Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) Or OldItemName <> Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) Then
                        strsql = "UPDATE INVENTORY_MSTR SET "
                        strsql &= "IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', "
                        strsql &= "IM_INVENTORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "' "
                        strsql &= "WHERE IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' AND IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "'"
                    End If

                    strProductCode = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))

                    strsql = "UPDATE PRODUCT_MSTR SET "
                    strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
                    strsql &= "PM_PRODUCT_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
                    strsql &= "PM_REF_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
                    strsql &= "PM_LONG_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
                    strsql &= "PM_CATEGORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
                    strsql &= "PM_ACCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("AccCode")) & "', " ' PM_ACCT_CODE
                    strsql &= "PM_UOM = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
                    strsql &= "PM_SAFE_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("MinInv")) & "', " ' PM_SAFE_QTY
                    strsql &= "PM_ORD_MIN_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Min")) & "', " ' PM_ORD_MIN_QTY
                    strsql &= "PM_ORD_MAX_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Max")) & "', " ' PM_ORD_MAX_QTY
                    strsql &= "PM_REORDER_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("RQL")) & "', "
                    strsql &= "PM_BUDGET_PRICE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("BudgetPrice")) & "', "
                    strsql &= "PM_EOQ = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("EOQ")) & "', "
                    strsql &= "PM_RATIO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Ratio")) & "', "
                    strsql &= "PM_IQC_TYPE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("IQCType")) & "', "
                    strsql &= "PM_OVERSEA = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd3")) & "', "
                    strsql &= "PM_PRODUCT_BRAND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
                    strsql &= "PM_PRODUCT_MODEL = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
                    strsql &= "PM_DRAW_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
                    strsql &= "PM_VERS_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
                    strsql &= "PM_GROSS_WEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
                    strsql &= "PM_NET_WEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
                    strsql &= "PM_LENGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
                    strsql &= "PM_WIDTH = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
                    strsql &= "PM_HEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
                    strsql &= "PM_VOLUME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
                    strsql &= "PM_COLOR_INFO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
                    strsql &= "PM_HSC_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
                    strsql &= "PM_REMARKS = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
                    strsql &= "PM_DELETED = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) & "', " ' PM_DELETED
                    strsql &= "PM_PARTIAL_CD = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("PartialDelivery")) & "', "
                    strsql &= "PM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', " ' PM_MOD_BY
                    strsql &= "PM_MOD_DT = getdate(), " ' PM_MOD_DT
                    strsql &= "PM_MAX_INV_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("MaxInvQty")) & "', "
                    strsql &= "PM_MANUFACTURER = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu")) & "', "
                    strsql &= "PM_MANUFACTURER2 = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu2")) & "', "
                    strsql &= "PM_MANUFACTURER3 = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu3")) & "', "
                    strsql &= "PM_SPEC1 = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec")) & "', "
                    strsql &= "PM_SPEC2 = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec2")) & "', "
                    strsql &= "PM_SPEC3 = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Spec3")) & "', "
                    strsql &= "PM_PACKING_TYPE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("PackType")) & "', "
                    strsql &= "PM_PACKING_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("PackQty")) & "', "
                    strsql &= "PM_SECTION = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("SectionCode")) & "', "
                    strsql &= "PM_LOCATION = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("LocationCode")) & "', "
                    strsql &= "PM_NEW_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("NewItemCode")) & "', "
                    strsql &= "PM_ITEM_TYPE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) & "', "
                    strsql &= "PM_CAT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CatCode")) & "', "
                    strsql &= "PM_IQC_IND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd2")) & "' "
                    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
                    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

                    INDEX = objDb.GetVal(strsql)

                    strsql = "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "

                    If objDb.Exist(strsql) Then
                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D'"

                        docFileName = objDb.GetVal(strsql)

                        'strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                        'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"

                        'imgFileName = Common.parseNull(objDb.GetVal(strsql))
                        'For Document attachment
                        strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                        strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "
                        Common.Insert2Ary(strAryQuery, strsql)

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Add", "File Attachment", "", "" & docFileName & "", strAryQuery)
                    End If

                    strsql = "SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'D' "

                    If objDb.Exist(strsql) Then
                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D'"


                        docFileName = objDb.GetVal(strsql)

                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"

                        imgFileName = Common.parseNull(objDb.GetVal(strsql))

                        strsql = "DELETE FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_ATTACH_INDEX IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'D') "
                        Common.Insert2Ary(strAryQuery, strsql)

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Delete", "File Attachment", "" & docFileName & "", "", strAryQuery)

                    End If

                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    strsql &= "AND PA_TYPE = 'D' AND (PA_STATUS = 'T' OR PA_STATUS = 'D') AND PA_SOURCE = 'B' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'Dim INDEX As String
                    'strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
                    'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

                    'INDEX = objDb.GetVal(strsql)

                    For i = 0 To aryVendor.Count - 1
                        If Common.Parse(aryVendor(i)(2)) <> "" Then
                            strsql = "SELECT '*' FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '" & aryVendor(i)(1) & "' AND PV_PRODUCT_INDEX = '" & INDEX & "'"

                            If objDb.Exist(strsql) > 0 Then
                                strsql = "UPDATE PIM_VENDOR SET "
                                strsql &= "PV_S_COY_ID = '" & Common.Parse(aryVendor(i)(3)) & "', "

                                If blnGST = False Then
                                    strsql &= "PV_S_COY_ID_TAX_ID = '" & Common.Parse(aryVendor(i)(10)) & "', "
                                End If

                                If Common.Parse(aryVendor(i)(11)) = "" Then
                                    tempNull = "NULL"
                                Else
                                    tempNull = "'" & Common.Parse(aryVendor(i)(11)) & "'"
                                End If

                                strsql &= "PV_LEAD_TIME = " & tempNull & ", "
                                strsql &= "PV_VENDOR_CODE = '" & Common.Parse(aryVendor(i)(12)) & "', "
                                strsql &= "PV_DELIVERY_TERM = '" & Common.Parse(aryVendor(i)(5)) & "', "
                                strsql &= "PV_SUPP_CODE = '" & Common.Parse(aryVendor(i)(4)) & "', "
                                strsql &= "PV_PAYMENT_CODE = '" & Common.Parse(aryVendor(i)(6)) & "', "
                                strsql &= "PV_CURR = '" & Common.Parse(aryVendor(i)(7)) & "', "
                                strsql &= "PV_PUR_SPEC_NO = '" & Common.Parse(aryVendor(i)(8)) & "', "
                                strsql &= "PV_REVISION = '" & Common.Parse(aryVendor(i)(9)) & "', "
                                strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                                strsql &= "PV_MOD_DATETIME = getdate() "
                                strsql &= "WHERE PV_VENDOR_TYPE = '" & aryVendor(i)(1) & "' "
                                strsql &= "AND PV_PRODUCT_INDEX = '" & INDEX & "'"

                                Common.Insert2Ary(strAryQuery, strsql)

                            Else
                                strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, "
                                strsql &= "PV_S_COY_ID, "

                                If blnGST = False Then
                                    strsql &= "PV_S_COY_ID_TAX_ID, "
                                End If

                                strsql &= "PV_LEAD_TIME, PV_VENDOR_CODE, PV_DELIVERY_TERM, PV_SUPP_CODE, PV_PAYMENT_CODE, PV_CURR, "
                                strsql &= "PV_PUR_SPEC_NO, PV_REVISION, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                                strsql &= "" & INDEX & ", "
                                strsql &= "'" & Common.Parse(aryVendor(i)(1)) & "', " 'Vendor Type
                                strsql &= "'" & Common.Parse(aryVendor(i)(3)) & "', " 'Vendor ID

                                If blnGST = False Then
                                    strsql &= "'" & Common.Parse(aryVendor(i)(10)) & "', " 'Tax ID
                                End If

                                If Common.Parse(aryVendor(i)(11)) = "" Then
                                    tempNull = "NULL"
                                Else
                                    tempNull = "'" & Common.Parse(aryVendor(i)(11)) & "'"
                                End If

                                strsql &= "" & tempNull & ", " '
                                strsql &= "'" & Common.Parse(aryVendor(i)(12)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(5)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(4)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(6)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(7)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(8)) & "', "
                                strsql &= "'" & Common.Parse(aryVendor(i)(9)) & "', "
                                strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                                strsql &= "getdate()) "

                                Common.Insert2Ary(strAryQuery, strsql)
                            End If

                        Else
                            strsql = "DELETE FROM PIM_VENDOR WHERE "
                            strsql &= "PV_VENDOR_TYPE = '" & aryVendor(i)(1) & "' "
                            strsql &= "AND PV_PRODUCT_INDEX = '" & INDEX & "'"

                            Common.Insert2Ary(strAryQuery, strsql)

                            strsql = "SELECT '*' FROM PRODUCT_VOLUME_PRICE WHERE PVP_VENDOR_TYPE = '" & aryVendor(i)(1) & "' "
                            strsql &= "AND PVP_PRODUCT_CODE = (SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_PRODUCT_INDEX = " & INDEX & ")"

                            If objDb.Exist(strsql) > 0 Then
                                strsql = "DELETE FROM PRODUCT_VOLUME_PRICE WHERE "
                                strsql &= "PVP_VENDOR_TYPE = '" & aryVendor(i)(1) & "' "
                                strsql &= "AND PVP_PRODUCT_CODE = (SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_PRODUCT_INDEX = " & INDEX & ")"

                                Common.Insert2Ary(strAryQuery, strsql)
                            End If
                        End If

                    Next



                    'strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR  "
                    'strsql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "'"

                    'If objDb.Exist(strsql) = 0 Then


                    '    'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
                    '    If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
                    '        tempNull = "NULL"
                    '    Else
                    '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
                    '    End If
                    '    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    '    strsql &= "" & INDEX & ", "
                    '    strsql &= "'P', "
                    '    strsql &= "" & tempNull & ", "
                    '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
                    '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    '    strsql &= "getdate()) "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    '    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
                    '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
                    '        tempNull = "NULL"
                    '    Else
                    '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
                    '    End If
                    '    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    '    strsql &= "" & INDEX & ", "
                    '    strsql &= "'1', "
                    '    strsql &= "" & tempNull & ", "
                    '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
                    '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    '    strsql &= "getdate()) "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    '    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
                    '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
                    '        tempNull = "NULL"
                    '    Else
                    '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
                    '    End If
                    '    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    '    strsql &= "" & INDEX & ", "
                    '    strsql &= "'2', "
                    '    strsql &= "" & tempNull & ", "
                    '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
                    '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    '    strsql &= "getdate()) "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    '    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
                    '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
                    '        tempNull = "NULL"
                    '    Else
                    '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
                    '    End If
                    '    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    '    strsql &= "" & INDEX & ", "
                    '    strsql &= "'3', "
                    '    strsql &= "" & tempNull & ", "
                    '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
                    '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    '    strsql &= "getdate()) "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    '    'End If

                    'End If

                    ''If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
                    '    tempNull = "NULL"
                    'Else
                    '    tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
                    'End If
                    'strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = " & tempNull & ", "
                    'strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
                    'strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    'strsql &= "PV_MOD_DATETIME = getdate() "
                    'strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    'strsql &= "And PV_VENDOR_TYPE = 'P' "
                    'Common.Insert2Ary(strAryQuery, strsql)

                    ''ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
                    '    tempNull = "NULL"
                    'Else
                    '    tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
                    'End If
                    'strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    'strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
                    'strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    'strsql &= "PV_MOD_DATETIME = getdate() "
                    'strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    'strsql &= "And PV_VENDOR_TYPE = '1' "
                    'Common.Insert2Ary(strAryQuery, strsql)
                    ''ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
                    '    tempNull = "NULL"
                    'Else
                    '    tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
                    'End If
                    'strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    'strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
                    'strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    'strsql &= "PV_MOD_DATETIME = getdate() "
                    'strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    'strsql &= "And PV_VENDOR_TYPE = '2' "
                    'Common.Insert2Ary(strAryQuery, strsql)
                    ''ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
                    '    tempNull = "NULL"
                    'Else
                    '    tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
                    'End If
                    'strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    'strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
                    'strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    'strsql &= "PV_MOD_DATETIME = getdate() "
                    'strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    'strsql &= "And PV_VENDOR_TYPE = '3' "
                    'Common.Insert2Ary(strAryQuery, strsql)
                    ''End If

                    'insert into audit trail - edit

                    Dim strsql1, strVen As String
                    Dim dtTemp As New DataTable
                    Dim dtTemp2 As New DataTable

                    dtTemp = objDb.FillDt("SELECT * FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & INDEX & " ")
                    If dtTemp.Rows.Count > 0 Then
                        For i = 0 To aryVendor.Count - 1
                            strsql1 = "SELECT * FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & INDEX & " AND PV_VENDOR_TYPE = '" & aryVendor(i)(1) & "' "
                            dtTemp2 = objDb.FillDt(strsql1)
                            If Common.Parse(aryVendor(i)(1)) = "P" Then
                                strVen = "Preferred"
                            ElseIf Common.Parse(aryVendor(i)(1)) = "1" Then
                                strVen = "1st"
                            ElseIf Common.Parse(aryVendor(i)(1)) = "2" Then
                                strVen = "2nd"
                            ElseIf Common.Parse(aryVendor(i)(1)) = "3" Then
                                strVen = "3rd"
                            Else
                                strVen = i & "th"
                            End If

                            If Common.Parse(aryVendor(i)(3)) <> "" Then
                                If dtTemp2.Rows.Count > 0 Then

                                    'Check Company ID
                                    If Common.Parse(aryVendor(i)(3)) <> Common.parseNull(dtTemp2.Rows(0)("PV_S_COY_ID")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Company ID", Common.parseNull(dtTemp2.Rows(0)("PV_S_COY_ID")), Common.Parse(aryVendor(i)(3)), strAryQuery)
                                    End If

                                    'Check Tax ID
                                    If blnGST = False Then
                                        If Common.Parse(aryVendor(i)(10)) <> Common.parseNull(dtTemp2.Rows(0)("PV_S_COY_ID_TAX_ID")) Then
                                            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Tax ID", Common.parseNull(dtTemp2.Rows(0)("PV_S_COY_ID_TAX_ID")), Common.Parse(aryVendor(i)(10)), strAryQuery)
                                        End If
                                    End If

                                    'Check Order Lead Time
                                    If Common.Parse(aryVendor(i)(11)) <> CStr(Common.parseNull(dtTemp2.Rows(0)("PV_LEAD_TIME"))) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Order Lead Time", Common.parseNull(dtTemp2.Rows(0)("PV_LEAD_TIME")), Common.Parse(aryVendor(i)(11)), strAryQuery)
                                    End If

                                    'Vendor Item Code
                                    If Common.Parse(aryVendor(i)(12)) <> Common.parseNull(dtTemp2.Rows(0)("PV_VENDOR_CODE")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Item Code", Common.parseNull(dtTemp2.Rows(0)("PV_VENDOR_CODE")), Common.Parse(aryVendor(i)(12)), strAryQuery)
                                    End If

                                    'Delivery Term
                                    If Common.Parse(aryVendor(i)(5)) <> Common.parseNull(dtTemp2.Rows(0)("PV_DELIVERY_TERM")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Delivery Term", Common.parseNull(dtTemp2.Rows(0)("PV_DELIVERY_TERM")), Common.Parse(aryVendor(i)(5)), strAryQuery)
                                    End If

                                    'Supplier Code
                                    If Common.Parse(aryVendor(i)(4)) <> Common.parseNull(dtTemp2.Rows(0)("PV_SUPP_CODE")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Supplier Code", Common.parseNull(dtTemp2.Rows(0)("PV_SUPP_CODE")), Common.Parse(aryVendor(i)(4)), strAryQuery)
                                    End If

                                    'Payment Code
                                    If Common.Parse(aryVendor(i)(6)) <> Common.parseNull(dtTemp2.Rows(0)("PV_PAYMENT_CODE")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Payment Code", Common.parseNull(dtTemp2.Rows(0)("PV_PAYMENT_CODE")), Common.Parse(aryVendor(i)(6)), strAryQuery)
                                    End If

                                    'Currency 
                                    If Common.Parse(aryVendor(i)(7)) <> Common.parseNull(dtTemp2.Rows(0)("PV_CURR")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Currency", Common.parseNull(dtTemp2.Rows(0)("PV_CURR")), Common.Parse(aryVendor(i)(7)), strAryQuery)
                                    End If

                                    'Purchaser Spec No 
                                    If Common.Parse(aryVendor(i)(8)) <> Common.parseNull(dtTemp2.Rows(0)("PV_PUR_SPEC_NO")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Purchaser Spec. No.", Common.parseNull(dtTemp2.Rows(0)("PV_PUR_SPEC_NO")), Common.Parse(aryVendor(i)(8)), strAryQuery)
                                    End If

                                    'Revision 
                                    If Common.Parse(aryVendor(i)(9)) <> Common.parseNull(dtTemp2.Rows(0)("PV_REVISION")) Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Revision", Common.parseNull(dtTemp2.Rows(0)("PV_REVISION")), Common.Parse(aryVendor(i)(9)), strAryQuery)
                                    End If

                                Else
                                    If Common.Parse(aryVendor(i)(3)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Company ID", "", Common.Parse(aryVendor(i)(3)), strAryQuery) 'Company ID
                                    End If
                                    If Common.Parse(aryVendor(i)(4)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Supplier Code", "", Common.Parse(aryVendor(i)(4)), strAryQuery) 'Supp Code
                                    End If
                                    If Common.Parse(aryVendor(i)(5)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Delivery Term", "", Common.Parse(aryVendor(i)(5)), strAryQuery) 'Delivery Term
                                    End If
                                    If Common.Parse(aryVendor(i)(6)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Payment Code", "", Common.Parse(aryVendor(i)(6)), strAryQuery) 'Payment Term
                                    End If
                                    If Common.Parse(aryVendor(i)(7)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Currency", "", Common.Parse(aryVendor(i)(7)), strAryQuery) 'Currency
                                    End If
                                    If Common.Parse(aryVendor(i)(8)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Purchaser Spec. No.", "", Common.Parse(aryVendor(i)(8)), strAryQuery) 'Purchaser Spec. No.
                                    End If
                                    If Common.Parse(aryVendor(i)(9)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Revision", "", Common.Parse(aryVendor(i)(9)), strAryQuery) 'Revision
                                    End If
                                    If blnGST = False Then
                                        If Common.Parse(aryVendor(i)(10)) <> "" Then
                                            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Tax", "", Common.Parse(aryVendor(i)(10)), strAryQuery) 'Tax
                                        End If
                                    End If
                                    If Common.Parse(aryVendor(i)(11)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Order Lead Time", "", Common.Parse(aryVendor(i)(11)), strAryQuery) 'Order Lead Time
                                    End If
                                    If Common.Parse(aryVendor(i)(12)) <> "" Then
                                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", strVen & " Vendor Item Code", "", Common.Parse(aryVendor(i)(12)), strAryQuery) 'Item Code
                                    End If
                                End If
                            Else
                                If dtTemp2.Rows.Count > 0 Then
                                    InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "Remove " & strVen & " Vendor Item Code", Common.parseNull(dtTemp2.Rows(0)("PV_S_COY_ID")), "", strAryQuery) 'Item Code
                                End If
                            End If

                        Next

                    End If

                    'Dim strsql1 As String
                    'Dim dtTemp As New DataTable
                    'strsql1 = "SELECT * FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    'dtTemp = objDb.FillDt(strsql1)
                    'If dtTemp.Rows.Count > 0 Then
                    '    If dsProduct.Tables(0).Rows(0)("Lead1") <> CStr(Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME"))) Or _
                    '       dsProduct.Tables(0).Rows(0)("Lead2") <> CStr(Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME"))) Or _
                    '       dsProduct.Tables(0).Rows(0)("Lead3") <> CStr(Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME"))) Or _
                    '       dsProduct.Tables(0).Rows(0)("LeadP") <> CStr(Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME"))) Then

                    '        If dsProduct.Tables(0).Rows(0)("Lead1") <> CStr(Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME"))) Then

                    '            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "1st Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead1"), strAryQuery)
                    '        End If
                    '        If dsProduct.Tables(0).Rows(0)("Lead2") <> CStr(Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME"))) Then

                    '            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "2nd Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead2"), strAryQuery)
                    '        End If
                    '        If dsProduct.Tables(0).Rows(0)("Lead3") <> CStr(Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME"))) Then

                    '            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "3rd Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead3"), strAryQuery)
                    '        End If
                    '        If dsProduct.Tables(0).Rows(0)("LeadP") <> CStr(Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME"))) Then

                    '            InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "Preferred Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("LeadP"), strAryQuery)
                    '        End If
                    '    End If
                    'End If

                    ''Insert Volume Price
                    If Not aryPrice Is Nothing Then
                        InsertUnitPrice(strMode, strProductCode, aryPrice, strAryQuery)
                    End If

            End Select



            'Check whether the item should add/remove to/from Default Purchaser Catalogue
            If Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) <> "Y" Then
                Dim objDBAccess As New EAD.DBCom
                If objDBAccess.GetCount("BUYER_CATALOGUE_MSTR, BUYER_CATALOGUE_ITEMS", "WHERE BCU_CAT_INDEX = BCM_CAT_INDEX AND BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' AND BCU_PRODUCT_CODE='" & Common.Parse(strProductCode) & "' ") = 0 And
                    objDBAccess.GetCount("BUYER_CATALOGUE_MSTR", "WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' ") > 0 Then
                    'Insert into Default Buyer Catalogue
                    strsql = "INSERT INTO BUYER_CATALOGUE_ITEMS (BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
                    strsql &= "VALUES ((SELECT BCM_CAT_INDEX FROM BUYER_CATALOGUE_MSTR WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "'), "
                    strsql &= "'" & Common.Parse(strProductCode) & "', '', '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate())"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Else
                'physical delete from Default Buyer Catalogue for deactivated item
                strsql = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "AND BCU_CAT_INDEX IN (SELECT BCM_CAT_INDEX "
                strsql &= "FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)
            End If

            If Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "ST" Or Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "MI" Then
                strsql = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE "
                strsql &= "IM_ITEM_CODE = '" & OldVendor & "' "
                strsql &= "AND IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "' "
                If objDb.Exist(strsql) > 0 Then
                    Dim IM_INVENTORY_INDEX As String
                    strsql = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR  "
                    strsql &= "WHERE IM_ITEM_CODE = '" & OldVendor & "' "
                    strsql &= "AND IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "' "

                    IM_INVENTORY_INDEX = objDb.GetVal(strsql)

                    strsql = "UPDATE INVENTORY_MSTR SET IM_IQC_IND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("RD2")) & "', IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VENDORITEMCODE")) & "', IM_INVENTORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ITEMNAME")) & "' "
                    strsql &= "WHERE IM_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Else
                    strsql = "INSERT INTO INVENTORY_MSTR (IM_COY_ID, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_IQC_IND) VALUES ("
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VENDORITEMCODE")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ITEMNAME")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("RD2")) & "') "
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "SP" Then
                strsql = "SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = " &
                        "(SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = '" & Common.Parse(OldVendor) & "' " &
                        "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' LIMIT 1) "

                If objDb.Exist(strsql) = 0 Then
                    strsql = "DELETE FROM INVENTORY_MSTR "
                    strsql &= "WHERE IM_ITEM_CODE = '" & Common.Parse(OldVendor) & "' "
                    strsql &= "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            End If

            ' Should Use Function
            'Dim INDEX As String
            'strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
            'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

            'INDEX = objDb.GetVal(strsql)

            'strsql = "DELETE FROM PIM_VENDOR "
            'strsql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "' "
            'Common.Insert2Ary(strAryQuery, strsql)


            'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = 'P' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 And Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = 'P' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'P', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = 'P' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '1' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '1' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'1', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = '1' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '2' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '2' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'2', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = '2' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '3' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '3' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'3', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = '" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & "' "
            '    strsql &= "And PV_VENDOR_TYPE = '3' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If
            Dim strImg As String

            If strMode = "mod" Then
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
            Else
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
            End If

            strImg = Common.parseNull(objDb.GetVal(strsql))

            If strImg = "" Then
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"
            Else
                If strMode = "mod" Then
                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
                Else
                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'I'"
                End If

            End If

            imgFileName = Common.parseNull(objDb.GetVal(strsql))

            If strImageIndex <> "" Then
                strsql = "DELETE FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "AND PA_TYPE = 'I' "
                strsql &= "AND PA_PRODUCT_CODE IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                'strsql &= "AND PA_SOURCE = 'B' AND PA_STATUS = 'T' AND PA_TYPE = 'I') "
                strsql &= "AND PA_SOURCE = 'B' AND PA_TYPE = 'I') "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"
                strsql &= " AND PA_SOURCE = 'B' AND PA_TYPE = 'I'"


                If objDb.Exist(strsql) Then
                    If strImg = "" And strMode = "mod" Then

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Delete", "Picture Attachment", "" & imgFileName & "", "", strAryQuery)

                    End If
                End If


                strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' AND PA_STATUS <> 'D' AND PA_SOURCE = 'B' "
                Common.Insert2Ary(strAryQuery, strsql)

                If strImg <> "" And strMode = "mod" Then
                    InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Add", "Picture Attachment", "", "" & imgFileName & "", strAryQuery)
                ElseIf strImg <> "" And strMode = "add" Then
                    InsertAuditTrailBIM(0, "Item Master", "Add", "Picture Attachment", "", "" & imgFileName & "", strAryQuery, True)
                End If

                strsql = "UPDATE PRODUCT_ATTACHMENT SET PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "WHERE PA_PRODUCT_CODE = (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' AND PA_STATUS <> 'D' AND PA_SOURCE = 'B') "
                Common.Insert2Ary(strAryQuery, strsql)


                'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & objDb.GetVal("SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' ")
                strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = "
                strsql &= "(SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I') "
                strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' "
                strsql &= "AND PA_TYPE = 'I' AND PA_STATUS = 'T' AND PA_SOURCE = 'B' "
                Common.Insert2Ary(strAryQuery, strsql)

            End If

            If objDb.BatchExecute(strAryQuery) Then
                BIM = True
            Else
                BIM = False
            End If

        End Function

        Public Function getConCatItemList(ByVal intIndex As Integer, ByVal strItemCode As String, ByVal strItemName As String, ByVal strCommType As String, ByVal strItemType As ArrayList, ByVal strBlnSP As String, ByVal strBlnST As String, ByVal strBlnMI As String) As DataSet

            Dim strsql, strTemp As String
            Dim dsProduct As DataSet
            Dim strType As String

            strsql = "SELECT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_ITEM_TYPE,PM_UOM,CT_NAME,CM_CURRENCY_CODE " _
                & "FROM PRODUCT_MSTR " _
                & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME " _
                & "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID " _
                & "AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
                & "WHERE PM_DELETED <> 'Y' AND PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                & "AND PM_PRODUCT_CODE NOT IN (SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
                & "WHERE CDI_GROUP_INDEX = " & intIndex & ")"

            '//Vendor Item Code
            If strItemCode <> "" Then
                strsql &= " AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strItemCode) & "%' "
            End If

            '//Product Desc
            If strItemName <> "" Then
                strsql &= " AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(strItemName) & "%' "
            End If

            '//Commodity Type
            If strCommType <> "" Then
                strsql &= " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommType) & "' "
            End If

            'Item Type
            If strItemType.Count > 0 Then
                For i As Integer = 0 To strItemType.Count - 1
                    If strType = "" Then
                        strType = "'" & strItemType(i) & "'"
                    Else
                        strType = strType & "," & "'" & strItemType(i) & "'"
                    End If
                Next
                strType = "(" & strType & ")"
                If strType <> "" Then
                    strsql &= " AND PM_ITEM_TYPE IN " & strType
                End If
            Else
                If strBlnSP = "Y" Or strBlnST = "Y" Or strBlnMI = "Y" Then
                    strsql &= " AND PM_ITEM_TYPE IN ("
                    strTemp = ""

                    If strBlnSP = "Y" Then
                        strTemp = "'SP'"
                    End If

                    If strBlnST = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'ST'"
                        Else
                            strTemp &= "'ST'"
                        End If
                    End If

                    If strBlnMI = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'MI'"
                        Else
                            strTemp &= "'MI'"
                        End If
                    End If

                    strsql &= strTemp & ")"

                End If
            End If

            dsProduct = objDb.FillDs(strsql)
            Return dsProduct
        End Function

        Public Function InsertUnitPrice(ByVal strMode As String, ByRef strProductCode As String, ByVal aryVolume As ArrayList, ByRef pQuery() As String)
            Dim strAryQuery(0) As String
            Dim arySetUnitPrice, aryTempUnitPrice As New ArrayList()
            arySetUnitPrice = aryVolume
            Dim i As Integer = 0
            Dim iTotal As Integer = 0
            Dim strQuery As String = ""
            Dim objdb As New EAD.DBCom

            If strMode = "mod" Then
                For i = 0 To arySetUnitPrice.Count - 1
                    strQuery = "DELETE FROM PRODUCT_VOLUME_PRICE "
                    strQuery &= "WHERE PVP_PRODUCT_CODE = '" & strProductCode & "' AND PVP_VENDOR_TYPE = '" & arySetUnitPrice(i)(3) & "'"
                    Common.Insert2Ary(pQuery, strQuery)
                Next
            End If

            If arySetUnitPrice.Count > 0 Then
                For i = 0 To arySetUnitPrice.Count - 1
                    If arySetUnitPrice(i)(0) <> "" And arySetUnitPrice(i)(1) <> "" Then
                        strQuery = "INSERT INTO PRODUCT_VOLUME_PRICE "
                        strQuery &= "(PVP_PRODUCT_CODE, PVP_VENDOR_TYPE, PVP_VOLUME, PVP_VOLUME_PRICE, PVP_ENT_BY, PVP_ENT_DT) VALUES "
                        strQuery &= "(" & Common.Parse(strProductCode) & ", "
                        strQuery &= "'" & arySetUnitPrice(i)(3) & "', "
                        strQuery &= "'" & arySetUnitPrice(i)(0) & "', "
                        strQuery &= "'" & arySetUnitPrice(i)(1) & "', "
                        strQuery &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                        strQuery &= "getdate()) "

                        Common.Insert2Ary(pQuery, strQuery)
                    End If
                Next
            End If

        End Function

        Public Function GetUnitPrice(ByVal strProductCode As String, Optional ByVal strVLine As String = "") As DataSet
            Dim strsql As String = ""
            Dim ds As New DataSet

            strsql = "SELECT PVP_VOLUME, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE WHERE PVP_PRODUCT_CODE = '" & strProductCode & "' AND PVP_VENDOR_TYPE = '" & strVLine & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetUnitPrice2(ByVal strProductCode As String, ByVal strVenline As String) As DataSet
            Dim strsql As String = ""
            Dim ds As New DataSet
            'Dim strBCompID As String = Common.Parse(HttpContext.Current.Session("CompanyId"))

            strsql = "SELECT PVP_VOLUME, PVP_VOLUME_PRICE, PVP_VENDOR_TYPE "
            strsql &= "FROM PRODUCT_VOLUME_PRICE "
            strsql &= "WHERE PVP_PRODUCT_CODE = '" & strProductCode & "' AND PVP_VENDOR_TYPE = '" & strVenline & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function getConCatSearchList(ByVal intIndex As Integer, ByVal strItemCode As String, ByVal strItemName As String, ByVal strCommType As String, ByVal strItemType As ArrayList, ByVal strVendor As String, ByVal strChkSP As String, ByVal strChkST As String, ByVal strChkMI As String, Optional ByVal catDesc As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal strOversea As String = "", Optional ByVal strCurr As String = "", Optional ByVal strVendor2 As String = "", Optional ByVal strChkVendor As String = "") As DataSet
            Dim strsql, strCondition As String
            Dim dsProduct As DataSet
            Dim strType, strTemp As String

            strsql = "SELECT IFNULL(CDI_VENDOR_ITEM_CODE,'') AS CDI_VENDOR_ITEM_CODE, IFNULL(CDI_PRODUCT_CODE,'') AS CDI_PRODUCT_CODE," _
                & "IFNULL(CDI_PRODUCT_DESC,'') AS CDI_PRODUCT_DESC, CDM_GROUP_CODE,CDM_GROUP_INDEX,CDI_GROUP_INDEX," _
                & "CM_COY_NAME,CDM_S_COY_ID,IFNULL(CDI_UOM,'') AS CDI_UOM,IFNULL(CDI_CURRENCY_CODE,'') AS CDI_CURRENCY_CODE," _
                & "IFNULL(CDI_UNIT_COST,0) AS CDI_UNIT_COST,IFNULL(CDI_GST,0) AS CDI_GST,IFNULL(CDI_REMARK,'') AS CDI_REMARK," _
                & "IFNULL(PM_CATEGORY_NAME,0) AS PM_CATEGORY_NAME,IFNULL(CT_NAME,'') AS CT_NAME,IFNULL(PM_ITEM_TYPE,'') AS PM_ITEM_TYPE, " _
                & "IFNULL(CDM_GROUP_DESC,'') AS CDM_GROUP_DESC, PM_SPEC1, PM_SPEC2, PM_SPEC3, PM_OVERSEA, IF(CDI_GST_TAX_CODE IS NULL OR CDI_GST_TAX_CODE = '', 'N/A',CDI_GST_TAX_CODE) AS CDI_GST_TAX_CODE, " _
                & "CASE WHEN CDI_GST_RATE = 'N/A' THEN CDI_GST_RATE ELSE " _
                & "IF((TAX_PERC IS NULL OR TAX_PERC = ''), IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, '(', TAX_PERC,'%)')) END AS CDI_GST_RATE " _
                & "FROM CONTRACT_DIST_COY " _
                & "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
                & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
                & "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX " _
                & "INNER JOIN contract_dist_user ON cdu_group_index = CDM_GROUP_INDEX " _
                & "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND PM_DELETED <> 'Y' " _
                & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME " _
                & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND CODE_ABBR = CDI_GST_RATE " _
                & "LEFT JOIN TAX ON TAX_CODE = CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
                & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                & "AND cdu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                & "AND CDM_TYPE = 'C' AND CDM_START_DATE<= CURRENT_DATE()  AND CDM_END_DATE>=CURRENT_DATE() " _
                & "AND PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

            '//Vendor Item Code
            If strItemCode <> "" Then
                strsql &= " AND CDI_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strItemCode) & "%' "
            End If

            '//Product Desc
            If strItemName <> "" Then
                strsql &= " AND CDI_PRODUCT_DESC LIKE '%" & Common.Parse(strItemName) & "%' "
            End If

            'Contract catalogue description
            If catDesc <> "" Then
                strsql &= " AND CDM_GROUP_DESC = '" & Common.Parse(catDesc) & "'"
            End If

            '//Contract Ref. No.
            If intIndex > 0 Then
                strsql &= " AND CDM_GROUP_INDEX = " & intIndex
            End If

            If strVendor <> "" Then
                strsql &= " AND CDM_S_COY_ID = '" & strVendor & "' "
            End If

            If strVendor2 <> "" Then
                strsql &= " AND CDM_S_COY_ID = '" & strVendor2 & "' "
            End If

            '//Commodity Type
            If strCommType <> "" Then
                strsql &= " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommType) & "' "
            End If

            'Item Type
            If strItemType.Count > 0 Then
                For i As Integer = 0 To strItemType.Count - 1
                    If strType = "" Then
                        strType = "'" & Common.Parse(strItemType(i)) & "'"
                    Else
                        strType = strType & "," & "'" & Common.Parse(strItemType(i)) & "'"
                    End If
                Next
                strType = "(" & strType & ")"
                If strType <> "" Then
                    strsql &= " AND PM_ITEM_TYPE IN " & strType
                End If
            Else
                If strChkSP = "Y" Or strChkST = "Y" Or strChkMI = "Y" Then
                    strsql &= " AND PM_ITEM_TYPE IN ("

                    strTemp = ""

                    If strChkSP = "Y" Then
                        strTemp = "'SP'"
                    End If

                    If strChkST = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'ST'"
                        Else
                            strTemp &= "'ST'"
                        End If
                    End If

                    If strChkMI = "Y" Then
                        If strTemp <> "" Then
                            strTemp &= ",'MI'"
                        Else
                            strTemp &= "'MI'"
                        End If
                    End If

                    strsql &= strTemp & ")"

                End If


            End If


            'If pItemType IsNot Nothing Then
            '    If pItemType.Count > 0 Then
            '        For i As Integer = 0 To pItemType.Count - 1
            '            If strType = "" Then
            '                strType = "'" & pItemType(i) & "'"
            '            Else
            '                strType = strType & "," & "'" & pItemType(i) & "'"
            '            End If
            '        Next
            '        strType = "(" & strType & ")"
            '        If strType <> "" Then
            '            strsql &= " AND PM_ITEM_TYPE IN " & strType
            '        End If
            '    End If
            'End If

            If strOversea <> "" Then
                strsql &= "AND PM_OVERSEA = '" & strOversea & "' "
            End If

            'If strVendor <> "" Then
            '    strsql &= "AND PM_LAST_TXN_S_COY_ID = '" & strVendor & "' "
            'End If

            If strCurr <> "" Then
                strsql &= "AND CDI_CURRENCY_CODE = '" & strCurr & "' "
            End If

            If strChkVendor = "no" Then
                strsql &= " AND CM_CURRENCY_CODE = '" & strCurr & "' "
            End If

            dsProduct = objDb.FillDs(strsql)
            Return dsProduct
        End Function

        Public Function InsertAuditTrailBIM(ByVal intRefID As Integer, ByVal strModule As String, ByVal strAction As String, ByVal strFieldName As String, ByVal strBefore As String, ByVal strAfter As String, ByRef pQuery() As String, Optional ByVal blnGetLastIndex As Boolean = False)
            Dim strsql As String

            If blnGetLastIndex = True Then
                strsql = "INSERT INTO au_product_log "
                strsql &= "(aup_refer_ID,aup_module,aup_action,aup_fieldName,aup_old_value,aup_new_value,aup_enterby,aup_coy_id, aup_changed_date) VALUES ("
                strsql &= "(SELECT ISNULL(MAX(PM_PRODUCT_INDEX),1) FROM PRODUCT_MSTR),"
                strsql &= "'" & strModule & "', "
                strsql &= "'" & strAction & "', "
                strsql &= "'" & strFieldName & "', "
                strsql &= "'" & strBefore & "', "
                strsql &= "'" & strAfter & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
                strsql &= Common.ConvertDate(Now()) & ")"
            Else
                strsql = "INSERT INTO au_product_log "
                strsql &= "(aup_refer_ID,aup_module,aup_action,aup_fieldName,aup_old_value,aup_new_value,aup_enterby,aup_coy_id,aup_changed_date) VALUES ("
                strsql &= intRefID & ","
                strsql &= "'" & strModule & "', "
                strsql &= "'" & strAction & "', "
                strsql &= "'" & strFieldName & "', "
                strsql &= "'" & Common.Parse(strBefore) & "', "
                strsql &= "'" & Common.Parse(strAfter) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
                strsql &= Common.ConvertDate(Now()) & ")"
            End If
            Common.Insert2Ary(pQuery, strsql)
            'objDb.Execute(strsql)
        End Function

        Public Function UpdProductBudgetPrice(ByVal strItemCode As String, ByVal strNewBP As String, ByVal strComp As String, Optional ByVal blnTrue As Boolean = True) As Boolean
            Dim strsql, strOldBP As String
            Dim intIndex As Integer
            Dim bTrue As Boolean = True
            Dim strAryQuery(0) As String

            If blnTrue = True Then
                strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
                strsql &= "WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"
                intIndex = objDb.GetVal(strsql)

                strsql = "SELECT IFNULL(PM_BUDGET_PRICE,'') AS PM_BUDGET_PRICE FROM PRODUCT_MSTR  "
                strsql &= "WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"
                strOldBP = objDb.GetVal(strsql)

                'InsertAuditTrailBIM(intIndex, "Item Master", "Modify", "Budget Price", strOldBP, strNewBP, strAryQuery)

                strsql = "UPDATE PRODUCT_MSTR SET PM_PREVIOUS_BUDGET_PRICE = PM_BUDGET_PRICE, PM_BUDGET_PRICE = " & CDbl(strNewBP) & " "
                strsql &= "WHERE PM_VENDOR_ITEM_CODE = '" & strItemCode & "' AND PM_S_COY_ID = '" & strComp & "'"
                Common.Insert2Ary(strAryQuery, strsql)

                objDb.BatchExecute(strAryQuery)

            Else
                strsql = "UPDATE PRODUCT_MSTR SET PM_PREVIOUS_BUDGET_PRICE = NULL "
                strsql &= "WHERE PM_VENDOR_ITEM_CODE = '" & strItemCode & "' AND PM_S_COY_ID = '" & strComp & "'"

                objDb.Execute(strsql)
            End If

        End Function

        Public Function GetVendorInfo(ByVal strCode As String) As DataSet
            Dim strsql As String
            Dim dsVen As New DataSet

            strsql = "SELECT PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            strsql &= "a.CM_COY_NAME AS S_COY_NAME_P,b.CM_COY_NAME AS S_COY_NAME_1,c.CM_COY_NAME AS S_COY_NAME_2,d.CM_COY_NAME AS S_COY_NAME_3, "
            strsql &= "PM_PREFER_S_COY_ID_TAX_ID, PM_1ST_S_COY_ID_TAX_ID, PM_2ND_S_COY_ID_TAX_ID, PM_3RD_S_COY_ID_TAX_ID, "
            strsql &= "t_a.TAX_PERC AS S_COY_TAX_P, t_b.TAX_PERC AS S_COY_TAX_1, t_c.TAX_PERC AS S_COY_TAX_2, t_d.TAX_PERC AS S_COY_TAX_3 "
            strsql &= "FROM PRODUCT_MSTR "
            strsql &= "LEFT JOIN COMPANY_MSTR a ON PM_PREFER_S_COY_ID = a.CM_COY_ID "
            strsql &= "LEFT JOIN COMPANY_MSTR b ON PM_1ST_S_COY_ID = b.CM_COY_ID "
            strsql &= "LEFT JOIN COMPANY_MSTR c ON PM_2ND_S_COY_ID = c.CM_COY_ID "
            strsql &= "LEFT JOIN COMPANY_MSTR d ON PM_3RD_S_COY_ID = d.CM_COY_ID "
            strsql &= "LEFT JOIN TAX t_a ON PM_PREFER_S_COY_ID_TAX_ID = t_a.TAX_AUTO_NO "
            strsql &= "LEFT JOIN TAX t_b ON PM_1ST_S_COY_ID_TAX_ID = t_b.TAX_AUTO_NO "
            strsql &= "LEFT JOIN TAX t_c ON PM_2ND_S_COY_ID_TAX_ID = t_c.TAX_AUTO_NO "
            strsql &= "LEFT JOIN TAX t_d ON PM_3RD_S_COY_ID_TAX_ID = t_d.TAX_AUTO_NO "
            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "'"

            dsVen = objDb.FillDs(strsql)
            GetVendorInfo = dsVen

        End Function

        Public Function GetVendorInfo2(ByVal strCode As String, ByVal strVenType As String) As DataSet
            Dim strsql As String
            Dim dsVen2 As New DataSet

            'strsql = "SELECT PV_VENDOR_TYPE, a.CM_COY_NAME AS PV_S_COY_NAME, PV_S_COY_ID, PV_S_COY_ID_TAX_ID, PV_LEAD_TIME, PV_VENDOR_CODE, "
            'strsql &= "PV_DELIVERY_TERM, PV_SUPP_CODE, PV_PAYMENT_CODE, PV_CURR, PV_PUR_SPEC_NO, PV_REVISION FROM PIM_VENDOR "
            'strsql &= "LEFT JOIN COMPANY_MSTR a ON PV_S_COY_ID = a.CM_COY_ID "
            'strsql &= "WHERE PV_VENDOR_TYPE = '" & strVenType & "' AND PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "')"

            strsql = "SELECT PV_VENDOR_TYPE, a.CM_COY_NAME AS PV_S_COY_NAME, PV_S_COY_ID, PV_S_COY_ID_TAX_ID, t.TAX_PERC AS PV_S_COY_ID_TAX, "
            strsql &= "PV_LEAD_TIME, PV_VENDOR_CODE, PV_DELIVERY_TERM, PV_SUPP_CODE, cm_b.CODE_DESC AS PV_PAYMENT,PV_PAYMENT_CODE, "
            strsql &= "cm_a.CODE_DESC AS PV_CURR_NAME, PV_CURR, PV_PUR_SPEC_NO, PV_REVISION "
            strsql &= "FROM PIM_VENDOR "
            strsql &= "LEFT JOIN COMPANY_MSTR a ON PV_S_COY_ID = a.CM_COY_ID "
            strsql &= "LEFT JOIN CODE_MSTR cm_a ON PV_CURR = cm_a.CODE_ABBR AND cm_a.CODE_CATEGORY = 'CU' "
            strsql &= "LEFT JOIN CODE_MSTR cm_b ON PV_PAYMENT_CODE = cm_b.CODE_ABBR AND cm_b.CODE_CATEGORY = 'PT' "
            strsql &= "LEFT JOIN TAX t ON PV_S_COY_ID_TAX_ID = t.TAX_AUTO_NO "
            strsql &= "WHERE PV_VENDOR_TYPE = '" & strVenType & "' AND PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "')"

            dsVen2 = objDb.FillDs(strsql)
            GetVendorInfo2 = dsVen2

        End Function

        Public Sub FillGLCode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView

            strSql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC FROM COMPANY_B_GL_CODE " _
                   & "WHERE CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CBG_B_GL_CODE", "CBG_B_GL_CODE", drw)
                'lstItem.Value = ""
                'lstItem.Text = "N/A"
                'strDefaultValue = lstItem.Text
                'pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Value = ""
                lstItem.Text = "N/A"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            'objDb = Nothing
        End Sub

        Public Function checkItemCodeDup(ByVal strItemCode As String) As Boolean
            Dim strSql As String

            strSql = "SELECT '*' FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "AND PM_VENDOR_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"

            If objDb.Exist(strSql) > 0 Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class
End Namespace
