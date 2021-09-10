Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy



    Public Class Products

        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Dim strMessage As String
        Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Property Message() As String
            Get
                Message = strMessage
            End Get
            Set(ByVal Value As String)
                strMessage = Value
            End Set
        End Property

        Public Function Download_ProductExcel_Common(ByVal CompID As String) As DataSet
            ' Dim objDb As New EAD.DBCom
            Dim dload As New DataSet
            Dim strSQL As String
            'strSQL = "SELECT P.PM_PRODUCT_CODE,P.PM_S_COY_ID, P.PM_PRODUCT_DESC, P.PM_CATEGORY_NAME,C.CM_CATEGORY_DESC, P.PM_UNIT_COST, P.PM_CURRENCY_CODE, " & _
            '         "P.PM_UOM, P.PM_GST_CODE, P.PM_MGMT_CODE, P.PM_MGMT_TEXT, P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_BRAND, P.PM_PRODUCT_MODEL " & _
            '         "FROM PRODUCT_MSTR P, CATEGORY_MSTR C " & _
            '         "WHERE C.CM_CATEGORY_NAME = P.PM_CATEGORY_NAME " & _
            '         "AND P.PM_S_COY_ID = '" & Common.Parse(CompID) & "' " & _
            '         "AND P.PM_DELETED <> 'Y' "

            strSQL = "SELECT P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_DESC, " &
                    "CASE P.PM_ITEM_TYPE WHEN 'SP' THEN 'Spot (Non-Inventoried item)' " &
                    "WHEN 'ST' THEN 'Stock (Direct material - Inventoried item)' " &
                    "WHEN 'MI' THEN 'MRO, M&E and IT (Inventoried item)'	END AS PM_ITEM_TYPE, " &
                    "IFNULL(P.PM_LONG_DESC,'') AS PM_LONG_DESC, " &
                    "(SELECT CT_CODE FROM COMMODITY_TYPE WHERE CT_ID = P.PM_CATEGORY_NAME) AS PM_CATEGORY_NAME, " &
                    "P.PM_UOM, " &
                    "IFNULL((SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = P.PM_CAT_CODE AND CBC_B_COY_ID = P.PM_S_COY_ID),'') AS PM_ACCT_CODE, " &
                    "P.PM_REF_NO, " &
                    "CASE P.PM_IQC_IND WHEN 'Y' THEN 'Yes' " &
                    "WHEN 'N' THEN 'No' END AS PM_IQC_IND, " &
                    "IFNULL((SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = P.PM_ACCT_CODE AND CBG_B_COY_ID = P.PM_S_COY_ID),'') AS PM_ACCT_CODE, " &
                    "P.PM_ORD_MIN_QTY AS PM_ORD_MIN_QTY, " &
                    "P.PM_ORD_MAX_QTY AS PM_ORD_MAX_QTY, P.PM_SAFE_QTY AS PM_SAFE_QTY, " &
                    "P.PM_MAX_INV_QTY AS PM_MAX_INV_QTY, IFNULL(P.PM_PRODUCT_BRAND,'') AS PM_PRODUCT_BRAND, " &
                    "IFNULL(P.PM_MANUFACTURER,'') AS PM_MANUFACTURER, IFNULL(P.PM_DRAW_NO,'') AS PM_DRAW_NO, " &
                    "IFNULL(P.PM_PRODUCT_MODEL,'') AS PM_PRODUCT_MODEL, IFNULL(P.PM_GROSS_WEIGHT,'') AS PM_GROSS_WEIGHT, " &
                    "IFNULL(P.PM_NET_WEIGHT,'') AS PM_NET_WEIGHT, IFNULL(P.PM_LENGHT,'') AS PM_LENGHT, " &
                    "IFNULL(P.PM_VERS_NO,'') AS PM_VERS_NO, IFNULL(P.PM_PACKING_REQ,'') AS PM_PACKING_REQ, " &
                    "IFNULL(P.PM_WIDTH,'') AS PM_WIDTH, IFNULL(P.PM_COLOR_INFO,'') AS PM_COLOR_INFO, " &
                    "IFNULL(P.PM_VOLUME,'') AS PM_VOLUME, IFNULL(P.PM_HSC_CODE,'') AS PM_HSC_CODE, " &
                    "IFNULL(P.PM_HEIGHT,'') AS PM_HEIGHT, IFNULL(P.PM_REMARKS,'') AS PM_REMARKS, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_PREFER_S_COY_ID),'') AS PM_PREFER_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_1ST_S_COY_ID),'') AS PM_1ST_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_2ND_S_COY_ID),'') AS PM_2ND_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_3RD_S_COY_ID),'') AS PM_3RD_S_COY_ID, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = 'P' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_P, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '1' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_1, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '2' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_2, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '3' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_3, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = 'P' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_P, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '1' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_1, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '2' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_2, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '3' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_3 " &
                    "FROM PRODUCT_MSTR P " &
                    "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' " &
                    "AND P.PM_DELETED <> 'Y' "

            '"CAST((CASE P.PM_PREFER_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '"ELSE CAST(P.PM_PREFER_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PM_PREFER_S_COY_ID_TAX_ID, " & _
            '"CAST((CASE P.PM_1ST_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '"ELSE CAST(P.PM_1ST_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PM_1ST_S_COY_ID_TAX_ID, " & _
            '"CAST((CASE P.PM_2ND_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '"ELSE CAST(P.PM_2ND_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PM_2ND_S_COY_ID_TAX_ID, " & _
            '"CAST((CASE P.PM_3RD_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '"ELSE CAST(P.PM_3RD_S_COY_ID_TAX_ID AS UNSIGNED) + 1 END) AS CHAR) AS PM_3RD_S_COY_ID_TAX_ID, " & _

            dload = objDb.FillDs(strSQL)
            Download_ProductExcel_Common = dload
        End Function

        Public Function Download_ProductExcel(ByVal CompID As String) As DataSet
            ' Dim objDb As New EAD.DBCom
            Dim dload As New DataSet
            Dim strSQL As String
            'strSQL = "SELECT P.PM_PRODUCT_CODE,P.PM_S_COY_ID, P.PM_PRODUCT_DESC, P.PM_CATEGORY_NAME,C.CM_CATEGORY_DESC, P.PM_UNIT_COST, P.PM_CURRENCY_CODE, " & _
            '         "P.PM_UOM, P.PM_GST_CODE, P.PM_MGMT_CODE, P.PM_MGMT_TEXT, P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_BRAND, P.PM_PRODUCT_MODEL " & _
            '         "FROM PRODUCT_MSTR P, CATEGORY_MSTR C " & _
            '         "WHERE C.CM_CATEGORY_NAME = P.PM_CATEGORY_NAME " & _
            '         "AND P.PM_S_COY_ID = '" & Common.Parse(CompID) & "' " & _
            '         "AND P.PM_DELETED <> 'Y' "

            strSQL = "SELECT P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_DESC, " &
                    "CASE P.PM_ITEM_TYPE WHEN 'SP' THEN 'Spot (Non-Inventoried item)' " &
                    "WHEN 'ST' THEN 'Stock (Direct material - Inventoried item)' " &
                    "WHEN 'MI' THEN 'MRO, M&E and IT (Inventoried item)'	END AS PM_ITEM_TYPE, " &
                    "IFNULL(P.PM_LONG_DESC,'') AS PM_LONG_DESC, " &
                    "(SELECT CT_CODE FROM COMMODITY_TYPE WHERE CT_ID = P.PM_CATEGORY_NAME) AS PM_CATEGORY_NAME, " &
                    "P.PM_UOM, " &
                    "P.PM_REF_NO, " &
                    "CASE P.PM_IQC_IND WHEN 'Y' THEN 'Yes' " &
                    "WHEN 'N' THEN 'No' END AS PM_IQC_IND, " &
                    "IFNULL((SELECT CBG_B_GL_DESC FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = P.PM_ACCT_CODE AND CBG_B_COY_ID = P.PM_S_COY_ID),'') AS PM_ACCT_CODE, " &
                    "P.PM_ORD_MIN_QTY AS PM_ORD_MIN_QTY, " &
                    "P.PM_ORD_MAX_QTY AS PM_ORD_MAX_QTY, P.PM_SAFE_QTY AS PM_SAFE_QTY, " &
                    "P.PM_MAX_INV_QTY AS PM_MAX_INV_QTY, IFNULL(P.PM_PRODUCT_BRAND,'') AS PM_PRODUCT_BRAND, " &
                    "IFNULL(P.PM_MANUFACTURER,'') AS PM_MANUFACTURER, IFNULL(P.PM_DRAW_NO,'') AS PM_DRAW_NO, " &
                    "IFNULL(P.PM_PRODUCT_MODEL,'') AS PM_PRODUCT_MODEL, IFNULL(P.PM_GROSS_WEIGHT,'') AS PM_GROSS_WEIGHT, " &
                    "IFNULL(P.PM_NET_WEIGHT,'') AS PM_NET_WEIGHT, IFNULL(P.PM_LENGHT,'') AS PM_LENGHT, " &
                    "IFNULL(P.PM_VERS_NO,'') AS PM_VERS_NO, IFNULL(P.PM_PACKING_REQ,'') AS PM_PACKING_REQ, " &
                    "IFNULL(P.PM_WIDTH,'') AS PM_WIDTH, IFNULL(P.PM_COLOR_INFO,'') AS PM_COLOR_INFO, " &
                    "IFNULL(P.PM_VOLUME,'') AS PM_VOLUME, IFNULL(P.PM_HSC_CODE,'') AS PM_HSC_CODE, " &
                    "IFNULL(P.PM_HEIGHT,'') AS PM_HEIGHT, IFNULL(P.PM_REMARKS,'') AS PM_REMARKS, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_PREFER_S_COY_ID),'') AS PM_PREFER_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_1ST_S_COY_ID),'') AS PM_1ST_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_2ND_S_COY_ID),'') AS PM_2ND_S_COY_ID, " &
                    "IFNULL((SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = P.PM_3RD_S_COY_ID),'') AS PM_3RD_S_COY_ID, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = 'P' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_P, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '1' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_1, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '2' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_2, " &
                    "(SELECT PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '3' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX) AS PV_LEAD_TIME_3, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = 'P' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_P, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '1' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_1, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '2' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_2, " &
                    "IFNULL((SELECT PV_VENDOR_CODE FROM PIM_VENDOR WHERE PV_VENDOR_TYPE = '3' AND PV_PRODUCT_INDEX = P.PM_PRODUCT_INDEX),'') AS PV_VENDOR_CODE_3 " &
                    "FROM PRODUCT_MSTR P " &
                    "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' " &
                    "AND P.PM_DELETED <> 'Y' "

            '"CASE P.PM_PREFER_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '        "ELSE P.PM_PREFER_S_COY_ID_TAX_ID + 1 END AS PM_PREFER_S_COY_ID_TAX_ID, " & _
            '        "CASE P.PM_1ST_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '        "ELSE P.PM_1ST_S_COY_ID_TAX_ID + 1 END AS PM_1ST_S_COY_ID_TAX_ID, " & _
            '        "CASE P.PM_2ND_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '        "ELSE P.PM_2ND_S_COY_ID_TAX_ID + 1 END AS PM_2ND_S_COY_ID_TAX_ID, " & _
            '        "CASE P.PM_3RD_S_COY_ID_TAX_ID WHEN '1' THEN 'N/A' " & _
            '        "ELSE P.PM_3RD_S_COY_ID_TAX_ID + 1 END AS PM_3RD_S_COY_ID_TAX_ID, " & _

            dload = objDb.FillDs(strSQL)
            Download_ProductExcel = dload
        End Function

        Public Function Download_ProductExcelVIM(ByVal CompID As String) As DataSet
            Dim dload As New DataSet
            Dim strSQL As String

            strSQL = "SELECT P.PM_VENDOR_ITEM_CODE, P.PM_PRODUCT_DESC, " &
                    "IFNULL(P.PM_LONG_DESC,'') AS PM_LONG_DESC, " &
                    "(SELECT CT_CODE FROM COMMODITY_TYPE WHERE CT_ID = P.PM_CATEGORY_NAME) AS PM_CATEGORY_NAME, " &
                    "CASE WHEN (P.PM_GST_CODE IS NULL OR P.PM_GST_CODE = 'N/A') THEN '' ELSE " &
                    "IF((PM_TAX_PERC = '' OR PM_TAX_PERC IS NULL), IFNULL(CODE_DESC,''), " &
                    "CONCAT(CODE_DESC, ' (', PM_TAX_PERC, '%)')) END AS PM_GST_CODE, " &
                    "P.PM_UNIT_COST, P.PM_UOM, " &
                    "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_ABBR = P.PM_CURRENCY_CODE) AS PM_CURRENCY_CODE, " &
                    "IFNULL(P.PM_REF_NO,'') AS PM_REF_NO, " &
                    "IFNULL(P.PM_PRODUCT_BRAND,'') AS PM_PRODUCT_BRAND, " &
                    "IFNULL(P.PM_DRAW_NO,'') AS PM_DRAW_NO, " &
                    "IFNULL(P.PM_PRODUCT_MODEL,'') AS PM_PRODUCT_MODEL, IFNULL(P.PM_GROSS_WEIGHT,'') AS PM_GROSS_WEIGHT, " &
                    "IFNULL(P.PM_NET_WEIGHT,'') AS PM_NET_WEIGHT, IFNULL(P.PM_LENGHT,'') AS PM_LENGHT, " &
                    "IFNULL(P.PM_VERS_NO,'') AS PM_VERS_NO, IFNULL(P.PM_PACKING_REQ,'') AS PM_PACKING_REQ, " &
                    "IFNULL(P.PM_WIDTH,'') AS PM_WIDTH, IFNULL(P.PM_COLOR_INFO,'') AS PM_COLOR_INFO, " &
                    "IFNULL(P.PM_VOLUME,'') AS PM_VOLUME, IFNULL(P.PM_HSC_CODE,'') AS PM_HSC_CODE, " &
                    "IFNULL(P.PM_HEIGHT,'') AS PM_HEIGHT, IFNULL(P.PM_REMARKS,'') AS PM_REMARKS " &
                    "FROM PRODUCT_MSTR P " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = P.PM_GST_CODE " &
                    "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' " &
                    "AND P.PM_DELETED <> 'Y' "

            'strSQL = "SELECT P.PM_VENDOR_ITEM_CODE " & _
            '        "FROM PRODUCT_MSTR P " & _
            '        "WHERE PM_S_COY_ID = '" & Common.Parse(CompID) & "' " & _
            '        "AND P.PM_DELETED <> 'Y' "

            dload = objDb.FillDs(strSQL)
            Download_ProductExcelVIM = dload
        End Function

        Public Function ValidateProductBatchUpload(ByVal pXML As String, ByVal pExcel As String, ByRef strE As String) As DataSet
            Dim a As New AppExcel
            Dim pMap As New myCollection
            Dim ds As DataSet
            Dim j, intTotalRow, intTotalCol, intTotalCol1, intLoop, intLoop1 As Integer
            Dim strColName, strAllowNull, strData, strErrMsg As String
            ds = a.ReadProductExcel_1(pXML, pExcel, pMap)

            'j = indexOfProductCol("Product_ID", pMap, 0, pMap.Count - 1)
            intTotalRow = ds.Tables(0).Rows.Count - 1
            intTotalCol = pMap.Count - 1
            intTotalCol1 = ds.Tables(0).Columns.Count - 1
            If intTotalCol <> intTotalCol1 Then
                Throw New CustomException("Number of Column not match")
            End If

            'Dim col As DataColumn

            ds.Tables(0).Columns.Add("ErrorMessage", Type.GetType("System.String"))

            For intLoop = 0 To intTotalRow 'For Each Row In Excel
                For intLoop1 = 0 To intTotalCol 'For Each Column of Every Row
                    strColName = CType(pMap(intLoop1), UploadRule).DBField
                    strAllowNull = CType(pMap(intLoop1), UploadRule).AllowNull
                    strData = Common.parseNull(ds.Tables(0).Rows(intLoop)(intLoop1))
                    If strAllowNull = "No" And strData = "" Then
                        strErrMsg = strColName & " is Empty as Row " & intLoop + 2.ToString
                        strE = strE & strErrMsg & "<BR>"
                    End If
                    '//at least validate data length
                    If Not ValidateColumn(strColName, strData) Then
                        ds.Tables(0).Rows(intLoop)(intTotalCol1 + 1) = "Error Occured"
                    Else
                        ds.Tables(0).Rows(intLoop)(intTotalCol1 + 1) = "No Error"
                    End If
                Next
            Next
            'a.WriteDsToExcel(ds)
            'a.DsToExcel(ds)
            Return ds
        End Function

        Private Function ValidateColumn(ByVal pColName As String, ByVal pColData As String) As Boolean
            Select Case pColName
                Case "Product_ID"
                    Return True
                    'If pColData.Length < 1000 Then

                    'End If
                Case "Product_Name"
                    Return True
                    'If pColData.Length < 1000 Then

                    'End If
                Case "Vendor_Item_Code"
                    Return True
                    'Check duplication in DB
                Case Else : Return True
            End Select
        End Function

        Private Function indexOfProductCol(ByVal Value As String,
                                          ByRef o As myCollection,
                                          ByVal startIndex As Long, ByVal count As Long) As Long
            Dim i, u As Long
            For i = startIndex To startIndex + count - 1
                If CType(o(i), UploadRule).DBField = Value Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Function getProduct(ByVal strCatType As String, ByVal strSearchType As String, ByVal strSearchVal() As String, ByRef intProductCnt As Int32, Optional ByVal strSearchMode As String = "normal", Optional ByVal strCommodity As String = "", Optional ByVal strFor As String = "") As DataSet
            '//strCatType=  Contract Price,Discount Price,List Price,Favurite List,Buyer Cat
            Dim dsProduct, dsBuyActivity As DataSet
            Dim intCount, intLoop, intFlagVal As Integer
            Dim blnBuyFixed, blnBuyDist, blnBuyCont As Boolean
            Dim strRole As String

            Dim objUser As New Users

            If objUser.IsPurchasing Then
                strRole = "PM"
            Else
                strRole = "BUYER"
            End If
            objUser = Nothing

            '//Get buying activity
            '//PM or PO can search the catalogue in all scenario
            '//Buyer can only search catalogue if buying activity is on and vendor as approved vendor
            If strRole = "PM" Then
                blnBuyFixed = True
                blnBuyDist = True
                blnBuyCont = True
            Else
                Dim objAdmin As New Admin
                dsBuyActivity = objAdmin.getFlag()
                intCount = dsBuyActivity.Tables(0).Rows.Count()
                For intLoop = 0 To intCount - 1
                    '  If dsFlag.Tables(0).Rows(i) = " Then
                    With dsBuyActivity.Tables(0).Rows(intLoop)
                        intFlagVal = .Item("CS_FLAG_VALUE")
                        Select Case Common.parseNull(.Item("CS_FLAG_NAME"))
                            Case "Buy Fixed Price"
                                blnBuyFixed = Convert.ToBoolean(intFlagVal)
                            Case "Buy Discount Price"
                                blnBuyDist = Convert.ToBoolean(intFlagVal)
                            Case "Buy Contract Price"
                                blnBuyCont = Convert.ToBoolean(intFlagVal)
                            Case "Free Format Item"
                        End Select
                    End With
                Next
                objAdmin = Nothing
            End If

            '//Get buying activity

            '//check buying activity
            '//check approved vendor
            '//check user role( if PM,see all vendor)
            Select Case strCatType
                Case "FP" '//Fixed Price
                    If blnBuyFixed Then
                        '//search product
                        dsProduct = searchListCat(strSearchType, strSearchVal, strSearchMode, , strCommodity, strFor)
                        intProductCnt = dsProduct.Tables(0).Rows.Count
                    Else
                        '//cannot buy, no search
                        intProductCnt = -1
                    End If
                Case "CP" '//Contract Price
                    If blnBuyCont Then
                        '//search product
                        dsProduct = searchConDistCat("CP", strSearchType, strSearchVal, strSearchMode)
                        intProductCnt = dsProduct.Tables(0).Rows.Count
                        '
                    Else
                        '//cannot buy, no search
                        intProductCnt = -1
                    End If
                Case "DP" '//Discount Price
                    If blnBuyDist Then
                        '//search product
                        dsProduct = searchConDistCat("DP", strSearchType, strSearchVal, strSearchMode)
                        intProductCnt = dsProduct.Tables(0).Rows.Count
                    Else
                        '//cannot buy, no search
                        intProductCnt = -1
                    End If
                Case "BC" '//Buyer Catalogue
            End Select
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
        Function searchListCat(ByVal strSearchType As String, ByVal strSearchVal() As String,
       Optional ByVal strSearchMode As String = "normal", Optional ByVal strCurrency As String = "", Optional ByVal strCommodity As String = "", Optional ByVal strFor As String = "") As DataSet
            '//PRODUCT_MSTR,COMPANY_VENDOR,PRODUCT_ATTR,COMPANY_MSTR,COMPANY_B_ITEM_CODE,CATEGORY_MSTR
            '//if Role=Purchasing Manager, he can see all Vendor, so no need to join with Company_Vendor
            ' Dim objDB As New EAD.DBCom
            Dim strSqlProduct, strCondition, strSqlCurrency, strRole As String
            Dim dsProduct As DataSet
            Dim strCoyID As String = HttpContext.Current.Session("CompanyId")

            '//For all SearchType Except Buyer Item Code
            '//Searching Buyer Item Code - still no sure
            '//how abt excluded category and inactive vendor company??????
            Dim objUser As New Users

            If objUser.IsPurchasing Then
                strRole = "PM"
            Else
                strRole = "BUYER"
            End If
            objUser = Nothing
            '//"CASE WHEN PM_PRODUCT_INDEX < 9999999999 THEN -1 ELSE -1 AS CDM_GROUP_INDEX" ==> 
            '//ORIGINALLY CODE LIKE THIS "-1 AS CDM_GROUP_INDEX"
            '//BUT SYSTEM CANNOT RECOGNIZE THIS AS INTEGER FIELD WHEN MERGE WITH OTHER DS
            If strRole = "BUYER" Then
                strSqlProduct = "SELECT 'LP' AS CAT,PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                          & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM, PM.PM_PRODUCT_BRAND, PM.PM_PRODUCT_MODEL, " _
                          & "'NIL' AS CDM_GROUP_DESC,PM_PRODUCT_INDEX AS CDM_GROUP_INDEX, " _
                          & "COMMODITY_TYPE.CT_NAME,'' AS CBC_B_ITEM_CODE,COMPANY_MSTR.CM_COY_ID, " _
                          & "COMPANY_MSTR.CM_COY_NAME FROM PRODUCT_MSTR PM INNER JOIN " _
                          & "COMPANY_MSTR ON PM.PM_S_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                          & "COMPANY_VENDOR ON PM.PM_S_COY_ID = COMPANY_VENDOR.CV_S_COY_ID LEFT OUTER JOIN " _
                          & "COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE PM.PM_DELETED <> 'Y' AND COMPANY_VENDOR.CV_B_COY_ID='" &
                          strCoyID & "' AND PM.PM_S_COY_ID<>'" & strCoyID & "' AND CM_DELETED <> 'Y' AND COMPANY_MSTR.CM_STATUS='A'"

                '& "COMPANY_B_ITEM_CODE CBC ON PM.PM_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
                '& "CBC.CBC_B_COY_ID ='" & strCoyID & "' " _

                If strFor <> "" Then strSqlProduct &= " AND PM_PRODUCT_FOR = '" & strFor & "' "
            Else 'PM or PO
                strSqlProduct = "SELECT 'LP' AS CAT,PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                         & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM,PM.PM_PRODUCT_BRAND,PM.PM_PRODUCT_MODEL, " _
                         & "'NIL' AS CDM_GROUP_DESC,PM_PRODUCT_INDEX AS CDM_GROUP_INDEX, " _
                         & "COMMODITY_TYPE.CT_NAME,'' AS CBC_B_ITEM_CODE,COMPANY_MSTR.CM_COY_ID, " _
                         & "COMPANY_MSTR.CM_COY_NAME " _
                         & "FROM PRODUCT_MSTR PM INNER JOIN " _
                         & "COMPANY_MSTR ON PM.PM_S_COY_ID = COMPANY_MSTR.CM_COY_ID LEFT OUTER JOIN " _
                         & "COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE PM.PM_DELETED <> 'Y' AND " _
                         & "PM.PM_S_COY_ID<>'" & strCoyID & "' AND CM_DELETED <> 'Y' AND COMPANY_MSTR.CM_STATUS='A'"

                '& "COMPANY_B_ITEM_CODE CBC ON PM.PM_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
                '& "CBC.CBC_B_COY_ID ='" & strCoyID & "' " _

                If strFor <> "" Then strSqlProduct &= " AND PM_PRODUCT_FOR = '" & strFor & "' "
            End If

            strCondition = BuildSearchCondition(strSearchMode, strSearchVal, strSearchType, strRole)

            If strCurrency <> "" Then
                strSqlCurrency = " AND PM_CURRENCY_CODE = '" & Common.Parse(strCurrency) & "' "
            Else
                strSqlCurrency = ""
            End If

            If strCommodity <> "" Then
                strCommodity = " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommodity) & "' "
            Else
                strCommodity = ""
            End If

            strSqlProduct = strSqlProduct & strCondition & strSqlCurrency & strCommodity

            dsProduct = objDb.FillDs(strSqlProduct)
            'objDB = Nothing
            Return dsProduct
        End Function
        'Michelle (3/12/2010) - replacing searchListCat
        Function search_Catalogue(ByVal strSearchCatType As String, ByVal strSearchType As String, ByVal strSearchVal() As String, ByVal strDesc As String, ByVal strCommodity As String, Optional ByVal strSearchMode As String = "normal", Optional ByVal strSelProdCode As String = "") As DataSet
            Dim strSqlVendor, strSqlBuyer, strCondition, strsql, strRole As String
            Dim dsProduct As DataSet
            Dim strCoyID As String = HttpContext.Current.Session("CompanyId")
            Dim strTemp As String
            Dim objUser As New Users

            If objUser.IsPurchasing Then
                strRole = "PM"
            Else
                strRole = "BUYER"
            End If
            objUser = Nothing

            strSqlBuyer = "SELECT distinct PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                                & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM, COMMODITY_TYPE.CT_NAME, CM_COY_ID, '' as CM_COY_NAME " _
                                & "FROM BUYER_CATALOGUE_USER BCU INNER JOIN BUYER_CATALOGUE_ITEMS BCI ON BCU.BCU_CAT_INDEX = BCI.BCU_CAT_INDEX " _
                                & "INNER JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = BCI.BCU_PRODUCT_CODE " _
                                & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PM.PM_S_COY_ID " _
                                & "LEFT OUTER JOIN COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE " _
                                & "BCU.BCU_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND PM.PM_DELETED <> 'Y' AND PM.PM_PRODUCT_FOR = 'B' AND BCI.BCU_S_COY_ID = '" & strCoyID & "' " _
                                & "AND NOT PM.PM_PRODUCT_CODE IN (" & strSelProdCode & ")"


            strSqlVendor = "SELECT PM.PM_PRODUCT_CODE,PM.PM_PRODUCT_DESC,PM.PM_VENDOR_ITEM_CODE, " _
                                 & "PM.PM_CURRENCY_CODE,PM.PM_UNIT_COST,PM.PM_UOM, COMMODITY_TYPE.CT_NAME, CM_COY_ID, CM_COY_NAME FROM PRODUCT_MSTR PM " _
                                 & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = PM.PM_S_COY_ID " _
                                 & "LEFT OUTER JOIN COMMODITY_TYPE ON PM.PM_CATEGORY_NAME = COMMODITY_TYPE.CT_ID WHERE " _
                                 & "PM.PM_DELETED <> 'Y' AND PM.PM_PRODUCT_FOR <> 'B' " _
                                 & "AND NOT PM.PM_PRODUCT_CODE IN (" & strSelProdCode & ")"

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
                'strDesc = " AND PM_PRODUCT_Desc = '" & Common.Parse(strDesc) & "' "
            Else
                strDesc = ""
            End If

            strsql = strsql & strCondition & strDesc & strCommodity

            dsProduct = objDb.FillDs(strsql)
            Return dsProduct
        End Function
        'Function searchListCat2(ByVal strSearchType As String, ByVal strSearchVal() As String, _
        '      Optional ByVal strSearchMode As String = "normal") As DataSet
        '    '//PRODUCT_MSTR,COMPANY_VENDOR,PRODUCT_ATTR,COMPANY_MSTR,COMPANY_B_ITEM_CODE,CATEGORY_MSTR
        '    '//if Role=Purchasing Manager, he can see all Vendor, so no need to join with Company_Vendor
        '    Dim objDB As New EAD.DBCom
        '    Dim strSqlProduct, strCondition, strRole As String
        '    Dim dsProduct As DataSet
        '    Dim strCoyID As String = HttpContext.Current.Session("CompanyId")

        '    '//For all SearchType Except Buyer Item Code
        '    '//Searching Buyer Item Code - still no sure
        '    '//how abt excluded category and inactive vendor company??????
        '    Dim objUser As New Users

        '    If objUser.IsPurchasing Then
        '        strRole = "PM"
        '    Else
        '        strRole = "BUYER"
        '    End If
        '    objUser = Nothing
        '    '//"CASE WHEN PM_PRODUCT_INDEX < 9999999999 THEN -1 ELSE -1 AS CDM_GROUP_INDEX" ==> 
        '    '//ORIGINALLY CODE LIKE THIS "-1 AS CDM_GROUP_INDEX"
        '    '//BUT SYSTEM CANNOT RECOGNIZE THIS AS INTEGER FIELD WHEN MERGE WITH OTHER DS
        '    If strRole = "BUYER" Then
        '        strSqlProduct = "select distinct COMPANY_MSTR.CM_COY_NAME FROM PRODUCT_MSTR PM INNER JOIN " _
        '                  & "CATEGORY_MSTR ON PM.PM_CATEGORY_NAME = CATEGORY_MSTR.CM_CATEGORY_NAME INNER JOIN " _
        '                  & "COMPANY_MSTR ON PM.PM_S_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
        '                  & "COMPANY_VENDOR ON PM.PM_S_COY_ID = COMPANY_VENDOR.CV_S_COY_ID LEFT OUTER JOIN " _
        '                  & "COMPANY_B_ITEM_CODE CBC ON PM.PM_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
        '                  & "CBC.CBC_B_COY_ID ='" & strCoyID & "' WHERE COMPANY_VENDOR.CV_B_COY_ID='" & _
        '                  strCoyID & "' AND CATEGORY_MSTR.CM_STATUS='A' AND PM.PM_S_COY_ID<>'" & strCoyID & "' AND CM_DELETED <> 'Y' AND COMPANY_MSTR.CM_STATUS='A'"
        '    Else 'PM or PO
        '        strSqlProduct = "select distinct COMPANY_MSTR.CM_COY_NAME " _
        '                 & "FROM PRODUCT_MSTR PM INNER JOIN " _
        '                 & "CATEGORY_MSTR ON PM.PM_CATEGORY_NAME = CATEGORY_MSTR.CM_CATEGORY_NAME INNER JOIN " _
        '                 & "COMPANY_MSTR ON PM.PM_S_COY_ID = COMPANY_MSTR.CM_COY_ID LEFT OUTER JOIN " _
        '                 & "COMPANY_B_ITEM_CODE CBC ON PM.PM_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
        '                 & "CBC.CBC_B_COY_ID ='" & strCoyID & "' WHERE CATEGORY_MSTR.CM_STATUS='A' AND " _
        '                 & "PM.PM_S_COY_ID<>'" & strCoyID & "' AND CM_DELETED <> 'Y' AND COMPANY_MSTR.CM_STATUS='A'"

        '    End If

        '    strCondition = BuildSearchCondition(strSearchMode, strSearchVal, strSearchType, strRole)

        '    strSqlProduct = strSqlProduct & strCondition
        '    dsProduct = objDB.FillDs(strSqlProduct)
        '    objDB = Nothing
        '    Return dsProduct
        'End Function

        Function searchConDistCat(ByVal strGrpType As String, ByVal strSearchType As String,
        ByVal strSearchVal() As String, Optional ByVal strSearchMode As String = "normal") As DataSet
            '//PRODUCT_MSTR,COMPANY_VENDOR,PRODUCT_ATTR,COMPANY_MSTR,COMPANY_B_ITEM_CODE,CATEGORY_MSTR
            'Dim objDB As New EAD.DBCom
            Dim strSqlProduct, strCondition, strRole As String
            Dim dsProduct As DataSet
            Dim strCoyID As String = HttpContext.Current.Session("CompanyId")

            '//For all SearchType Except Buyer Item Code
            '//Searching Buyer Item Code - still no sure
            '//ASSUME THAT "START DATE" AND "END DATE" STORE ONLY DATE WITH NO TIME
            Dim objUser As New Users
            If objUser.IsPurchasing Then
                strRole = "PM"
            Else
                strRole = "BUYER"
            End If
            objUser = Nothing

            ' ai chu modified on 26/09/2005
            ' catalogue item belongs to not approved vendor cannot be viewed by user
            If strRole = "BUYER" Then
                strSqlProduct = "SELECT '" & strGrpType & "' as CAT, CDI.CDI_PRODUCT_CODE AS PM_PRODUCT_CODE,CDI.CDI_PRODUCT_DESC AS PM_PRODUCT_DESC, " _
                            & "CDI.CDI_VENDOR_ITEM_CODE AS PM_VENDOR_ITEM_CODE,CDI.CDI_CURRENCY_CODE AS PM_CURRENCY_CODE," _
                            & "CDI.CDI_UNIT_COST AS PM_UNIT_COST,CDI.CDI_UOM AS PM_UOM, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,CONTRACT_DIST_MSTR.CDM_GROUP_DESC, " _
                            & "CONTRACT_DIST_MSTR.CDM_GROUP_INDEX,CATEGORY_MSTR.CM_CATEGORY_DESC,CBC.CBC_B_ITEM_CODE, " _
                            & "COMPANY_MSTR.CM_COY_ID,COMPANY_MSTR.CM_COY_NAME FROM CONTRACT_DIST_MSTR INNER JOIN " _
                            & "CONTRACT_DIST_COY ON CONTRACT_DIST_MSTR.CDM_GROUP_INDEX = CONTRACT_DIST_COY.CDC_GROUP_INDEX INNER JOIN " _
                            & "CONTRACT_DIST_ITEMS CDI ON CONTRACT_DIST_COY.CDC_GROUP_INDEX = CDI.CDI_GROUP_INDEX INNER JOIN " _
                            & "COMPANY_MSTR ON CONTRACT_DIST_MSTR.CDM_S_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "COMPANY_VENDOR ON CDM_S_COY_ID = COMPANY_VENDOR.CV_S_COY_ID LEFT OUTER JOIN " _
                            & "PRODUCT_MSTR ON CDI.CDI_PRODUCT_CODE = PRODUCT_MSTR.PM_PRODUCT_CODE AND PRODUCT_MSTR.PM_DELETED <> 'Y' INNER JOIN " _
                            & "CATEGORY_MSTR ON PRODUCT_MSTR.PM_CATEGORY_NAME = CATEGORY_MSTR.CM_CATEGORY_NAME LEFT OUTER JOIN " _
                            & "COMPANY_B_ITEM_CODE CBC ON CDI.CDI_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
                            & "CBC.CBC_B_COY_ID ='" & strCoyID & "' WHERE CONTRACT_DIST_COY.CDC_B_COY_ID='" & strCoyID & "' AND CATEGORY_MSTR.CM_STATUS='A' " _
                            & "AND CDM_TYPE='" & Left(strGrpType, 1) & "' AND (" & Common.ConvertDate(Now.Today) & " BETWEEN CDM_START_DATE AND CDM_END_DATE " _
                            & "OR (" & Common.ConvertDate(Now.Today) & ">= CDM_START_DATE AND CDM_END_DATE IS NULL))" _
                            & "AND COMPANY_VENDOR.CV_B_COY_ID = '" & strCoyID & "' AND COMPANY_MSTR.CM_STATUS = 'A'"
            Else
                strSqlProduct = "SELECT '" & strGrpType & "' as CAT, CDI.CDI_PRODUCT_CODE AS PM_PRODUCT_CODE,CDI.CDI_PRODUCT_DESC AS PM_PRODUCT_DESC, " _
                            & "CDI.CDI_VENDOR_ITEM_CODE AS PM_VENDOR_ITEM_CODE,CDI.CDI_CURRENCY_CODE AS PM_CURRENCY_CODE," _
                            & "CDI.CDI_UNIT_COST AS PM_UNIT_COST,CDI.CDI_UOM AS PM_UOM, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,CONTRACT_DIST_MSTR.CDM_GROUP_DESC, " _
                            & "CONTRACT_DIST_MSTR.CDM_GROUP_INDEX,CATEGORY_MSTR.CM_CATEGORY_DESC,CBC.CBC_B_ITEM_CODE, " _
                            & "COMPANY_MSTR.CM_COY_ID,COMPANY_MSTR.CM_COY_NAME FROM CONTRACT_DIST_MSTR INNER JOIN " _
                            & "CONTRACT_DIST_COY ON CONTRACT_DIST_MSTR.CDM_GROUP_INDEX = CONTRACT_DIST_COY.CDC_GROUP_INDEX INNER JOIN " _
                            & "CONTRACT_DIST_ITEMS CDI ON CONTRACT_DIST_COY.CDC_GROUP_INDEX = CDI.CDI_GROUP_INDEX INNER JOIN " _
                            & "COMPANY_MSTR ON CONTRACT_DIST_MSTR.CDM_S_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "PRODUCT_MSTR ON CDI.CDI_PRODUCT_CODE = PRODUCT_MSTR.PM_PRODUCT_CODE AND PRODUCT_MSTR.PM_DELETED <> 'Y' INNER JOIN " _
                            & "CATEGORY_MSTR ON PRODUCT_MSTR.PM_CATEGORY_NAME = CATEGORY_MSTR.CM_CATEGORY_NAME LEFT OUTER JOIN " _
                            & "COMPANY_B_ITEM_CODE CBC ON CDI.CDI_PRODUCT_CODE = CBC.CBC_PRODUCT_CODE AND " _
                            & "CBC.CBC_B_COY_ID ='" & strCoyID & "' WHERE CONTRACT_DIST_COY.CDC_B_COY_ID='" & strCoyID & "' AND CATEGORY_MSTR.CM_STATUS='A' AND COMPANY_MSTR.CM_STATUS = 'A' " _
                            & "AND CDM_TYPE='" & Left(strGrpType, 1) & "' AND (" & Common.ConvertDate(Now.Today) & " BETWEEN CDM_START_DATE AND CDM_END_DATE " _
                            & "OR (" & Common.ConvertDate(Now.Today) & ">= CDM_START_DATE AND CDM_END_DATE IS NULL))"
            End If

            strCondition = BuildSearchCondition(strSearchMode, strSearchVal, strSearchType, strRole)
            strSqlProduct = strSqlProduct & strCondition
            dsProduct = objDb.FillDs(strSqlProduct)
            'objDB = Nothing
            Return dsProduct
        End Function

        Function getBICodeCnt(ByVal pCoyId As String) As Integer

            Dim intReturnCnt As Integer
            intReturnCnt = objDb.GetCount("COMPANY_B_ITEM_CODE", "CBC_B_COY_ID='" & pCoyId & "'")
            objDb = Nothing
            Return intReturnCnt
        End Function

        '//Temporary, will called personalsetting later
        'Function getFavList() As DataView
        '    Dim objDB As New EAD.DBCom()
        '    Dim dv As DataView
        '    dv = objDB.GetView("SELECT * FROM FAVOURITE_LIST_MSTR WHERE FLM_BUYER_ID='" & HttpContext.Current.Session("UserId") & "' AND FLM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'")
        '    Return dv
        'End Function

        Function getBuyerCat() As DataView

            Dim dv As DataView
            dv = objDb.GetView("SELECT BCM_CAT_INDEX," & objDb.Concat(" : ", "", "BCM_GRP_CODE", "BCM_GRP_DESC") & " as BCM_GRP_DESC FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'")
            Return dv
        End Function

        Function addToShopCart(ByVal intCartIndex As Long, ByRef dtProd As DataTable) As String
            Dim dr As DataRow
            Dim strSql, strSqlAry(0), strCoyId, strUserID As String

            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            For Each dr In dtProd.Rows
                '//check duplication first
                '//build query
                If dr("SC_UNIT_COST") = -1 Then
                    dr("msg") = 2
                Else
                    strSql = "SELECT '*' FROM SHOPPING_CART WHERE SC_USER_ID='" & strUserID &
                    "' AND SC_B_COY_ID='" & strCoyId &
                    "' AND SC_PRODUCT_CODE='" & dr("PRODUCT_CODE") & "'"
                    If objDb.Exist(strSql) Then
                        dr("msg") = "0" 'duplicate
                    Else
                        If dr("GRP_INDEX") = -1 Then
                            strSql = "INSERT INTO SHOPPING_CART(SC_B_COY_ID,SC_USER_ID,SC_S_COY_ID,SC_PRODUCT_CODE," _
                            & "SC_VENDOR_ITEM_CODE,SC_PRODUCT_DESC,SC_UNIT_COST, SC_QUANTITY, SC_CURRENCY_CODE, SC_UOM,SC_SOURCE)" _
                            & " VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strUserID) & "','" &
                            Common.Parse(dr("S_COY_ID")) & "','" & Common.Parse(dr("PRODUCT_CODE")) & "','" &
                            Common.Parse(dr("VENDOR_ITEM_CODE")) & "','" & Common.Parse(dr("SC_PRODUCT_DESC")) & "'," &
                            IIf(dr("SC_UNIT_COST") = -1, "Null", dr("SC_UNIT_COST")) & "," & IIf(dr("SC_QUANTITY") = -1, "NULL", dr("SC_QUANTITY")) & ",'" &
                            Common.Parse(dr("SC_CURRENCY_CODE")) & "','" & Common.Parse(dr("SC_UOM")) & "','" & Common.Parse(dr("CAT_TYPE")) & "')"
                        Else
                            strSql = "INSERT INTO SHOPPING_CART(SC_B_COY_ID,SC_USER_ID,SC_S_COY_ID,SC_PRODUCT_CODE," _
                           & "SC_VENDOR_ITEM_CODE,SC_PRODUCT_DESC,SC_UNIT_COST, SC_QUANTITY, SC_CURRENCY_CODE, SC_UOM,SC_SOURCE,SC_CD_GROUP_INDEX)" _
                           & " VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strUserID) & "','" &
                           Common.Parse(dr("S_COY_ID")) & "','" & Common.Parse(dr("PRODUCT_CODE")) & "','" &
                           Common.Parse(dr("VENDOR_ITEM_CODE")) & "','" & Common.Parse(dr("SC_PRODUCT_DESC")) & "'," &
                           IIf(dr("SC_UNIT_COST") = -1, "Null", dr("SC_UNIT_COST")) & "," & IIf(dr("SC_QUANTITY") = -1, "NULL", dr("SC_QUANTITY")) & ",'" &
                           Common.Parse(dr("SC_CURRENCY_CODE")) & "','" & Common.Parse(dr("SC_UOM")) & "','" &
                           Common.Parse(dr("CAT_TYPE")) & "'," & dr("GRP_INDEX") & ")"
                        End If
                        Common.Insert2Ary(strSqlAry, strSql)
                        dr("msg") = "1" 'ok
                    End If
                End If
            Next
            If strSqlAry(0) <> String.Empty Then
                objDb.BatchExecute(strSqlAry)
            End If
            'objDB = Nothing
        End Function

        Function addToFavList(ByVal intFavIndex As Long, ByRef dtProd As DataTable) As String
            Dim dr As DataRow
            Dim strSql, strSqlAry(0) As String

            For Each dr In dtProd.Rows
                '//check duplication first
                '//build query
                If dr("SC_UNIT_COST") = -1 Then
                    dr("msg") = 2
                Else
                    strSql = "SELECT '*' FROM FAVOURITE_LIST_ITEMS WHERE FLI_LIST_INDEX=" & intFavIndex & " AND FLI_PRODUCT_CODE='" & Common.Parse(dr(0)) & "'"
                    If objDb.Exist(strSql) Then
                        dr("msg") = "0" 'duplicate
                    Else
                        If dr("GRP_INDEX") = -1 Then
                            strSql = "INSERT INTO FAVOURITE_LIST_ITEMS(FLI_LIST_INDEX,FLI_PRODUCT_CODE,FLI_SOURCE,FLI_S_COY_ID)" _
                            & " VALUES(" & intFavIndex & ",'" & Common.Parse(dr("PRODUCT_CODE")) & "','" & Common.Parse(dr("CAT_TYPE")) & "','" & Common.Parse(dr("S_COY_ID")) & "')"
                            Common.Insert2Ary(strSqlAry, strSql)
                            dr("msg") = "1" 'ok
                        Else
                            strSql = "INSERT INTO FAVOURITE_LIST_ITEMS(FLI_LIST_INDEX,FLI_PRODUCT_CODE,FLI_SOURCE,FLI_S_COY_ID,FLI_CD_GROUP_INDEX)" _
                            & " VALUES(" & intFavIndex & ",'" & Common.Parse(dr("PRODUCT_CODE")) & "','" & Common.Parse(dr("CAT_TYPE")) &
                            "','" & Common.Parse(dr("S_COY_ID")) & "'," & dr("GRP_INDEX") & ")"
                            Common.Insert2Ary(strSqlAry, strSql)
                            dr("msg") = "1" 'ok
                        End If
                    End If
                End If
            Next
            If strSqlAry(0) <> String.Empty Then
                objDb.BatchExecute(strSqlAry)
            End If
            objDb = Nothing
        End Function

        Function addToBuyerCat(ByVal intBCIndex As Long, ByRef dtProd As DataTable) As String
            Dim dr As DataRow
            Dim strSql, strSqlAry(0) As String


            For Each dr In dtProd.Rows
                '//check duplication first
                If dr("SC_UNIT_COST") = -1 Then
                    dr("msg") = 2
                Else
                    strSql = "SELECT '*' FROM BUYER_CATALOGUE_ITEMS WHERE BCU_CAT_INDEX=" & intBCIndex & " AND BCU_PRODUCT_CODE='" & dr("PRODUCT_CODE") & "'"
                    If objDb.Exist(strSql) Then
                        dr("msg") = "0" 'duplicate
                    Else
                        '//build query
                        If dr("GRP_INDEX") = -1 Then
                            strSql = "INSERT INTO BUYER_CATALOGUE_ITEMS(BCU_CAT_INDEX,BCU_PRODUCT_CODE,BCU_SOURCE,BCU_S_COY_ID)" _
                             & " VALUES(" & intBCIndex & ",'" & Common.Parse(dr("PRODUCT_CODE")) & "','" &
                            Common.Parse(dr("CAT_TYPE")) & "','" & Common.Parse(dr("S_COY_ID")) & "')"
                            Common.Insert2Ary(strSqlAry, strSql)
                            dr("msg") = "1" 'ok
                        Else
                            strSql = "INSERT INTO BUYER_CATALOGUE_ITEMS(BCU_CAT_INDEX,BCU_PRODUCT_CODE,BCU_SOURCE,BCU_S_COY_ID,BCU_CD_GROUP_INDEX)" _
                            & " VALUES(" & intBCIndex & ",'" & Common.Parse(dr("PRODUCT_CODE")) & "','" &
                            Common.Parse(dr("CAT_TYPE")) & "','" & Common.Parse(dr("S_COY_ID")) & "'," & dr("GRP_INDEX") & ")"
                            Common.Insert2Ary(strSqlAry, strSql)
                            dr("msg") = "1" 'ok
                        End If
                    End If
                End If
            Next
            If strSqlAry(0) <> String.Empty Then
                objDb.BatchExecute(strSqlAry)
            End If
            'objDB = Nothing
        End Function

        Function getSCSchema() As DataTable

            Dim strSql As String
            Dim ds As New DataSet
            strSql = "SC_S_COY_ID as S_COY_ID,SC_PRODUCT_CODE AS PRODUCT_CODE,SC_VENDOR_ITEM_CODE as VENDOR_ITEM_CODE,SC_PRODUCT_DESC,SC_UNIT_COST, SC_QUANTITY, SC_CURRENCY_CODE, SC_UOM"
            objDb.FillSchema("SHOPPING_CART", ds, strSql, "SC")
            'objDB = Nothing
            Return ds.Tables(0)
        End Function

        Public Function getProDetails(ByVal strtxproid As String, ByVal strCoyId As String, ByVal blnBuyerCompany As Boolean, ByVal strGrpIndex As String, ByVal blnDraft As Boolean) As DataSet

            Dim strSql, strSql1, strSql2, strSql3, strDistinct, strCondition2a, strCondition1b, strCondition2b As String
            Dim dsprodet As DataSet

            If blnBuyerCompany Then
                strDistinct = " DISTINCT "
                strCondition1b = "AND CDC_B_COY_ID='" & strCoyId & "' AND A.CDM_TYPE='C'"
                strCondition2b = "AND CDC_B_COY_ID='" & strCoyId & "' AND A.CDM_TYPE='D'"
            Else
                strDistinct = " DISTINCT "
                strCondition1b = "AND A.CDM_S_COY_ID = '" & strCoyId & "' AND A.CDM_TYPE='C'"
                strCondition2b = "AND A.CDM_S_COY_ID = '" & strCoyId & "' AND A.CDM_TYPE='D'"
            End If

            ' ai chu modified on 13/10/2005
            ' item description should be retrieved from respective table
            ' if it is list price catalogue, retrieve from Product_Mstr table
            ' else retrieve from CONTRACT_DIST_ITEMS table
            If strGrpIndex = "" Then
                'Michelle (23/12/2010) - CATEGORY_MSTR no longer in use
                'strSql = "SELECT CM_CATEGORY_DESC,PM_PRODUCT_DESC,PM_PRODUCT_CODE,PM_VENDOR_ITEM_CODE,CM_COY_NAME,PM_PRODUCT_BRAND,PM_PRODUCT_MODEL,PM_PRODUCT_IMAGE,PM_S_COY_ID from CATEGORY_MSTR "
                'strSql &= "LEFT JOIN PRODUCT_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME AND PM_DELETED <> 'Y' "
                'strSql = "SELECT CT_NAME,CM_COY_NAME, TAX_PERC, PRODUCT_MSTR.* from "
                'strSql &= "PRODUCT_MSTR LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID AND PM_DELETED <> 'Y' "
                'strSql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
                'strSql &= "LEFT JOIN TAX ON PM_TAX_ID = TAX_AUTO_NO "
                'strSql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "' "

                'Chee Hong - 30/09/2014 - GST Enhancement
                strSql = "SELECT CT_NAME,CM_COY_NAME, a.TAX_PERC, " &
                        "CASE WHEN PM_GST_CODE = 'N/A' THEN PM_GST_CODE ELSE " &
                        "IF(b.TAX_PERC IS NULL OR b.TAX_PERC = '', IFNULL(CODE_DESC,''), CONCAT(CODE_DESC, ' (', b.TAX_PERC, '%)')) END AS GST_RATE, " &
                        "PRODUCT_MSTR.* FROM " &
                        "PRODUCT_MSTR LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID AND PM_DELETED <> 'Y' " &
                        "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID " &
                        "LEFT JOIN TAX a ON PM_TAX_ID = a.TAX_AUTO_NO " &
                        "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = PM_GST_CODE " &
                        "LEFT JOIN TAX b ON PM_GST_CODE = b.TAX_CODE AND CM_COUNTRY = b.TAX_COUNTRY_CODE " &
                        "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "' "
            Else
                If blnDraft Then ' contract item uploaded by vendor admin
                    'strSql = "SELECT CM_CATEGORY_DESC,CDUI_Product_Desc AS PM_PRODUCT_DESC,PM_PRODUCT_CODE, "
                    strSql = "SELECT CT_NAME,CDUI_Product_Desc AS PM_PRODUCT_DESC,PM_PRODUCT_CODE, "
                    strSql &= "CDUI_Vendor_Item_Code AS PM_VENDOR_ITEM_CODE,CM_COY_NAME,PM_PRODUCT_BRAND, "
                    strSql &= "PM_PRODUCT_MODEL,PM_PRODUCT_IMAGE,PM_S_COY_ID, "
                    'strSql &= "FROM CATEGORY_MSTR "
                    'strSql &= "LEFT JOIN PRODUCT_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME AND PM_DELETED <> 'Y' "
                    strSql &= "PM_ITEM_TYPE, PM_IQC_IND,PM_MAX_INV_QTY "
                    strSql &= "FROM PRODUCT_MSTR "
                    strSql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID AND PM_DELETED <> 'Y' "
                    strSql &= "LEFT JOIN CONT_DIST_UPLOADITEMS ON PM_PRODUCT_CODE = CDUI_Product_Code "
                    strSql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
                    strSql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "' "
                    strSql &= "AND CDUI_Upload_Index = '" & Common.Parse(strGrpIndex) & "' "
                Else
                    'strSql = "SELECT CM_CATEGORY_DESC,CDI_PRODUCT_DESC AS PM_PRODUCT_DESC,PM_PRODUCT_CODE, "
                    strSql = "SELECT CT_NAME,CDI_PRODUCT_DESC AS PM_PRODUCT_DESC,PM_PRODUCT_CODE, "
                    strSql &= "CDI_VENDOR_ITEM_CODE AS PM_VENDOR_ITEM_CODE,CM_COY_NAME,PM_PRODUCT_BRAND, "
                    strSql &= "PM_PRODUCT_MODEL,PM_PRODUCT_IMAGE,PM_S_COY_ID, "
                    'strSql &= "FROM CATEGORY_MSTR "
                    'strSql &= "LEFT JOIN PRODUCT_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME AND PM_DELETED <> 'Y' "
                    strSql &= "PM_ITEM_TYPE, PM_IQC_IND,PM_MAX_INV_QTY "
                    strSql &= "FROM PRODUCT_MSTR "
                    strSql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID AND PM_DELETED <> 'Y' "
                    strSql &= "LEFT JOIN CONTRACT_DIST_ITEMS ON PM_PRODUCT_CODE = CDI_PRODUCT_CODE "
                    strSql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
                    strSql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "' "
                    strSql &= "AND CDI_GROUP_INDEX = '" & Common.Parse(strGrpIndex) & "' "
                End If
            End If

            dsprodet = objDb.FillDs(strSql)

            strSql1 = "SELECT 'Catalogue Unit Price' AS TYPE,'' AS GRP_CODE,PM_ENT_DT AS TRANSDATE,PM_UNIT_COST AS PRICE," _
            & "PM_CURRENCY_CODE AS CURRENCY,'' AS REMARKS,PM_UOM as UOM " _
            & "FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(strtxproid) & "' "

            strSql2 = "SELECT " & strDistinct & " 'Contract Unit Price' AS TYPE,A.CDM_GROUP_CODE AS GRP_CODE,B.CDI_ENT_DT AS TRANSDATE,B.CDI_UNIT_COST AS PRICE," _
            & "B.CDI_CURRENCY_CODE AS CURRENCY,B.CDI_REMARK AS REMARKS,CDI_UOM AS UOM " _
            & "FROM CONTRACT_DIST_MSTR A,CONTRACT_DIST_ITEMS B,CONTRACT_DIST_COY C " _
            & "WHERE A.CDM_GROUP_INDEX=B.CDI_GROUP_INDEX " _
            & "And A.CDM_GROUP_INDEX = C.CDC_GROUP_INDEX " _
            & "AND B.CDI_PRODUCT_CODE='" & Common.Parse(strtxproid) & "' AND " &
            Common.ConvertDate(Now.Today) & " BETWEEN CDM_START_DATE AND CDM_END_DATE " _
            & strCondition1b
            '& "AND CDC_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND A.CDM_TYPE='C'"

            strSql3 = "SELECT " & strDistinct & " 'Discount Unit Price' AS TYPE,A.CDM_GROUP_CODE AS GRP_CODE,B.CDI_ENT_DT AS TRANSDATE,B.CDI_UNIT_COST AS PRICE," _
            & "B.CDI_CURRENCY_CODE AS CURRENCY,B.CDI_REMARK AS REMARKS,CDI_UOM AS UOM " _
            & "FROM CONTRACT_DIST_MSTR A,CONTRACT_DIST_ITEMS B,CONTRACT_DIST_COY C " _
            & "WHERE A.CDM_GROUP_INDEX=B.CDI_GROUP_INDEX " _
            & "AND A.CDM_GROUP_INDEX = C.CDC_GROUP_INDEX " _
            & "AND (" & Common.ConvertDate(Now.Today) & " BETWEEN CDM_START_DATE AND CDM_END_DATE " _
            & "OR (" & Common.ConvertDate(Now.Today) & ">= CDM_START_DATE AND CDM_END_DATE IS NULL)) " _
            & "AND B.CDI_PRODUCT_CODE='" & Common.Parse(strtxproid) & "' " _
            & strCondition2b
            '& "AND CDC_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND A.CDM_TYPE='D'"

            Dim ds, dsTemp As DataSet
            strSql = strSql1 & ";" & strSql2 & ";" & strSql3
            ds = objDb.FillDs(strSql)


            Dim drNew As DataRow
            '//NO NEED TO CHECK,PRODUCT_MSTR MUST GOT DATE
            'If ds.Tables(0).Rows.Count < 0 Then
            '    drNew = ds.Tables(0).NewRow
            '    drNew("TYPE") = "CATALOGUE UNIT PRICE"
            '    drNew("GRP_CODE") = "-"
            '    drNew("TRANSDATE") = "-"
            '    drNew("PRICE") = "NA"
            '    drNew("CURRENCY") = "-"
            '    drNew("REMARKS") = "-"
            '    ds.Tables(0).Rows.Add(drNew)
            'End If
            'dsTemp = ds.Tables(0)
            Dim dt As DataTable
            dt = ds.Tables(0).Copy()
            '
            If ds.Tables(1).Rows.Count <= 0 Then
                drNew = ds.Tables(1).NewRow
                drNew("TYPE") = "Contract Unit Price"
                drNew("GRP_CODE") = "-"
                drNew("TRANSDATE") = Now
                drNew("PRICE") = 0
                drNew("CURRENCY") = "-"
                drNew("REMARKS") = "-"
                ds.Tables(1).Rows.Add(drNew)
            End If

            Dim dr As DataRow
            For Each dr In ds.Tables(1).Rows
                'dr1 = ds.Tables(1).ImportRow
                dt.ImportRow(dr)
            Next
            If ds.Tables(2).Rows.Count <= 0 Then
                drNew = ds.Tables(2).NewRow
                drNew("TYPE") = "Discount Unit Price"
                drNew("GRP_CODE") = "-"
                drNew("TRANSDATE") = Now
                drNew("PRICE") = 0
                drNew("CURRENCY") = "-"
                drNew("REMARKS") = "-"
                ds.Tables(2).Rows.Add(drNew)
            End If
            'dt = ds.Tables(2).Copy()
            For Each dr In ds.Tables(2).Rows
                'dr1 = ds.Tables(1).ImportRow
                dt.ImportRow(dr)
            Next
            dt.TableName = "grid"
            dsprodet.Tables.Add(dt)

            strSql = "SELECT * FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE='" & Common.Parse(strtxproid) & "'"
            objDb.FillDsIn(dsprodet, strSql, "attach")

            'objDb = Nothing
            'For Prefer Venodr
            strSql = "SELECT PREFER.CM_COY_NAME AS 'PREFER_VENDOR', "
            strSql &= "1ST.CM_COY_NAME AS '1ST_VENDOR', "
            strSql &= "2ND.CM_COY_NAME AS '2ND_VENDOR', "
            strSql &= "3RD.CM_COY_NAME AS '3RD_VENDOR', "
            strSql &= "(SELECT TAX_PERC FROM TAX WHERE TAX_AUTO_NO = PM_PREFER_S_COY_ID_TAX_ID) AS 'PREFER_VENDOR_TAX',"
            strSql &= "(SELECT TAX_PERC FROM TAX WHERE TAX_AUTO_NO = PM_1ST_S_COY_ID_TAX_ID) AS '1ST_VENDOR_TAX',"
            strSql &= "(SELECT TAX_PERC FROM TAX WHERE TAX_AUTO_NO = PM_2ND_S_COY_ID_TAX_ID) AS '2ND_VENDOR_TAX',"
            strSql &= "(SELECT TAX_PERC FROM TAX WHERE TAX_AUTO_NO = PM_3RD_S_COY_ID_TAX_ID) AS '3RD_VENDOR_TAX' "
            strSql &= "FROM PRODUCT_MSTR "
            strSql &= "LEFT OUTER JOIN COMPANY_MSTR PREFER ON PM_PREFER_S_COY_ID = PREFER.CM_COY_ID "
            strSql &= "LEFT OUTER JOIN COMPANY_MSTR 1ST ON PM_1ST_S_COY_ID = 1ST.CM_COY_ID "
            strSql &= "LEFT OUTER JOIN COMPANY_MSTR 2ND ON PM_2ND_S_COY_ID = 2ND.CM_COY_ID "
            strSql &= "LEFT OUTER JOIN COMPANY_MSTR 3RD ON PM_3RD_S_COY_ID = 3RD.CM_COY_ID "
            strSql &= "WHERE PM_PRODUCT_CODE='" & Common.Parse(strtxproid) & "'"
            objDb.FillDsIn(dsprodet, strSql, "vendor")

            Dim INDEX As String
            strSql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
            strSql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "'"

            INDEX = objDb.GetVal(strSql)

            strSql = "SELECT PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE "
            strSql &= "FROM PIM_VENDOR "
            strSql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "' "
            objDb.FillDsIn(dsprodet, strSql, "vendorlead")

            getProDetails = dsprodet
        End Function
        Public Function getTxnPrice(ByVal strtxproid As String) As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT POM_S_COY_NAME, POD_UNIT_COST, POM_PO_DATE, POD_ORDERED_QTY FROM PO_DETAILS, PO_MSTR, COMPANY_MSTR "
            strSql &= "WHERE POD_PRODUCT_CODE = '" & Common.Parse(strtxproid) & "' AND POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO AND POD_COY_ID = CM_COY_ID "
            'Michelle (21/10/2011) - Issue 1071
            strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "
            strSql &= "AND ISNULL(POM_PO_DATE,'') <> '' ORDER BY POM_PO_DATE DESC LIMIT 5"
            ds = objDb.FillDs(strSql)

            getTxnPrice = ds
        End Function

        Public Function UpdateProdByExcel(ByRef pProdDetails As ExcelProduct) As Boolean
            Dim strSQL As String
            Dim Query(0) As String



            Select Case pProdDetails.Action
                Case "A"
                    Dim ItemNo As String
                    Dim objContcat As New ContCat

                    objContcat.GetLatestItemNo(ItemNo)
                    strSQL = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
                    Common.Insert2Ary(Query, strSQL)
                    'pProdDetails.ItemID = ItemNo

                    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(ItemNo) & "' AND PM_DELETED <> 'Y' "
                    If objDb.Exist(strSQL) = 1 Then 'no exist
                        strMessage = "Duplicated product found."
                        Return False
                    End If

                    'strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(pProdDetails.VendorItemCode) & "'"
                    'If objDb.Exist(strSQL) = 1 Then ' 0= no exist
                    '    strMessage = RecordNotSave & " ,same vendor item code with previous product."
                    '    Return False
                    'End If

                    strSQL = " INSERT INTO PRODUCT_MSTR(PM_S_COY_ID,PM_PRODUCT_CODE,PM_CATEGORY_NAME, " &
                             " PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC,PM_UOM, " &
                             " PM_UNIT_COST,PM_MGMT_CODE,PM_MGMT_TEXT, " &
                             " PM_CURRENCY_CODE,PM_PRODUCT_BRAND,PM_PRODUCT_MODEL, " &
                             " PM_GST_CODE, " &
                             " PM_ENT_BY,PM_ENT_DT,PM_DELETED)" &
                             " VALUES('" &
                             Common.Parse(pProdDetails.CoyId) & "','" & Common.Parse(pProdDetails.ItemID) & "','" & Common.Parse(pProdDetails.UNSPSCCode) & "','" &
                             Common.Parse(pProdDetails.VendorItemCode) & "','" & Common.Parse(pProdDetails.ItemDesc) & "','" & Common.Parse(pProdDetails.UOM) & "'," &
                             IIf(pProdDetails.UnitCost > 0, Common.ConvertMoney(pProdDetails.UnitCost), "NULL") & ",'" & Common.Parse(pProdDetails.MgmtCode) & "','" & Common.Parse(pProdDetails.MgmtText) & "','" &
                             Common.Parse(pProdDetails.CurrencyCode) & "','" & Common.Parse(pProdDetails.Brand) & "','" & Common.Parse(pProdDetails.Model) & "','" &
                             Common.Parse(pProdDetails.TaxCode) & "','" &
                             ctx.Session("UserID") & "'," & Common.ConvertDate(Now()) & ",'N')"
                    Common.Insert2Ary(Query, strSQL)

                Case "M"
                    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "' AND PM_DELETED <> 'Y' "
                    If objDb.Exist(strSQL) = 1 Then 'no exist
                        strSQL = " UPDATE PRODUCT_MSTR SET " &
                                 " PM_VENDOR_ITEM_CODE='" & Common.Parse(pProdDetails.VendorItemCode) & "'," &
                                 " PM_PRODUCT_DESC='" & Common.Parse(pProdDetails.ItemDesc) & "'," &
                                 " PM_CATEGORY_NAME='" & Common.Parse(pProdDetails.UNSPSCCode) & "'," &
                                 " PM_UOM='" & Common.Parse(pProdDetails.UOM) & "'," &
                                 " PM_GST_CODE='" & Common.Parse(pProdDetails.TaxCode) & "'," &
                                 " PM_UNIT_COST=" & IIf(pProdDetails.UnitCost > 0, Common.ConvertMoney(pProdDetails.UnitCost), "NULL") & "," &
                                 " PM_MGMT_CODE='" & Common.Parse(pProdDetails.MgmtCode) & "'," &
                                 " PM_MGMT_TEXT='" & Common.Parse(pProdDetails.MgmtText) & "'," &
                                 " PM_CURRENCY_CODE='" & Common.Parse(pProdDetails.CurrencyCode) & "'," &
                                 " PM_PRODUCT_BRAND='" & Common.Parse(pProdDetails.Brand) & "'," &
                                 " PM_PRODUCT_MODEL='" & Common.Parse(pProdDetails.Model) & "'," &
                                 " PM_MOD_BY='" & ctx.Session("UserID") & "'," &
                                 " PM_MOD_DT=" & Common.ConvertDate(Now()) &
                                 " WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "'" &
                                 " AND PM_S_COY_ID='" & Common.Parse(pProdDetails.CoyId) & "'"

                        Common.Insert2Ary(Query, strSQL)
                    Else
                        strMessage = Common.RecordNotSave
                        Return False
                    End If
                Case "D"
                    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "' AND PM_DELETED <> 'Y' "
                    If objDb.Exist(strSQL) = 1 Then 'no exist
                        ' physical delete
                        'strSQL = " DELETE PRODUCT_MSTR " & _
                        '         " WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "'" & _
                        '         " AND PM_S_COY_ID='" & Common.Parse(pProdDetails.CoyId) & "'"

                        ' logical delete
                        strSQL = "UPDATE PRODUCT_MSTR "
                        strSQL &= "SET PM_DELETED = 'Y' "
                        strSQL &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(pProdDetails.ItemID) & "' "
                        strSQL &= "AND PM_S_COY_ID = '" & Common.Parse(pProdDetails.CoyId) & "' "
                        Common.Insert2Ary(Query, strSQL)
                    Else
                        strMessage = Common.RecordNotSave
                        Return False
                    End If
            End Select


            If objDb.BatchExecute(Query) Then
                strMessage = Common.RecordSave
                Return True
            Else
                strMessage = Common.RecordNotSave
                Return False
            End If
        End Function

        Public Function UpdateProdByExcel(ByRef pProdDetails As ExcelBIMProduct) As Boolean
            Dim strSQL As String
            Dim Query(0) As String



            Select Case pProdDetails.Action
                Case "A"
                    Dim ItemNo As String
                    Dim objContcat As New ContCat

                    objContcat.GetLatestItemNo(ItemNo)
                    strSQL = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
                    Common.Insert2Ary(Query, strSQL)
                    'pProdDetails.ItemID = ItemNo

                    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(ItemNo) & "' AND PM_DELETED <> 'Y' "
                    If objDb.Exist(strSQL) = 1 Then 'no exist
                        strMessage = "Duplicated product found."
                        Return False
                    End If

                    'strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(pProdDetails.VendorItemCode) & "'"
                    'If objDb.Exist(strSQL) = 1 Then ' 0= no exist
                    '    strMessage = RecordNotSave & " ,same vendor item code with previous product."
                    '    Return False
                    'End If

                    strSQL = " INSERT INTO PRODUCT_MSTR(PM_S_COY_ID,PM_PRODUCT_CODE,PM_CATEGORY_NAME, " &
                             " PM_ITEM_TYPE, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC,PM_UOM, " &
                             " PM_PRODUCT_FOR, PM_ENT_BY,PM_ENT_DT,PM_DELETED)" &
                             " VALUES('" &
                             HttpContext.Current.Session("CompanyID") & "','" & Common.Parse(ItemNo) & "','6','" &
                             "SP" & "','" & Common.Parse(pProdDetails.ItemCode) & " ','" & Common.Parse(pProdDetails.Description) & "','" & Common.Parse(pProdDetails.UOM) & "','" &
                             "B" & "','" & ctx.Session("UserID") & "'," & Common.ConvertDate(Now()) & ",'N')"
                    Common.Insert2Ary(Query, strSQL)

                    'Case "M"
                    '    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "' AND PM_DELETED <> 'Y' "
                    '    If objDb.Exist(strSQL) = 1 Then 'no exist
                    '        strSQL = " UPDATE PRODUCT_MSTR SET " & _
                    '                 " PM_VENDOR_ITEM_CODE='" & Common.Parse(pProdDetails.VendorItemCode) & "'," & _
                    '                 " PM_PRODUCT_DESC='" & Common.Parse(pProdDetails.ItemDesc) & "'," & _
                    '                 " PM_CATEGORY_NAME='" & Common.Parse(pProdDetails.UNSPSCCode) & "'," & _
                    '                 " PM_UOM='" & Common.Parse(pProdDetails.UOM) & "'," & _
                    '                 " PM_GST_CODE='" & Common.Parse(pProdDetails.TaxCode) & "'," & _
                    '                 " PM_UNIT_COST=" & IIf(pProdDetails.UnitCost > 0, Common.ConvertMoney(pProdDetails.UnitCost), "NULL") & "," & _
                    '                 " PM_MGMT_CODE='" & Common.Parse(pProdDetails.MgmtCode) & "'," & _
                    '                 " PM_MGMT_TEXT='" & Common.Parse(pProdDetails.MgmtText) & "'," & _
                    '                 " PM_CURRENCY_CODE='" & Common.Parse(pProdDetails.CurrencyCode) & "'," & _
                    '                 " PM_PRODUCT_BRAND='" & Common.Parse(pProdDetails.Brand) & "'," & _
                    '                 " PM_PRODUCT_MODEL='" & Common.Parse(pProdDetails.Model) & "'," & _
                    '                 " PM_MOD_BY='" & ctx.Session("UserID") & "'," & _
                    '                 " PM_MOD_DT=" & Common.ConvertDate(Now()) & _
                    '                 " WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "'" & _
                    '                 " AND PM_S_COY_ID='" & Common.Parse(pProdDetails.CoyId) & "'"

                    '        Common.Insert2Ary(Query, strSQL)
                    '    Else
                    '        strMessage = Common.RecordNotSave
                    '        Return False
                    '    End If
                    'Case "D"
                    '    strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "' AND PM_DELETED <> 'Y' "
                    '    If objDb.Exist(strSQL) = 1 Then 'no exist
                    '        ' physical delete
                    '        'strSQL = " DELETE PRODUCT_MSTR " & _
                    '        '         " WHERE PM_PRODUCT_CODE='" & Common.Parse(pProdDetails.ItemID) & "'" & _
                    '        '         " AND PM_S_COY_ID='" & Common.Parse(pProdDetails.CoyId) & "'"

                    '        ' logical delete
                    '        strSQL = "UPDATE PRODUCT_MSTR "
                    '        strSQL &= "SET PM_DELETED = 'Y' "
                    '        strSQL &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(pProdDetails.ItemID) & "' "
                    '        strSQL &= "AND PM_S_COY_ID = '" & Common.Parse(pProdDetails.CoyId) & "' "
                    '        Common.Insert2Ary(Query, strSQL)
                    '    Else
                    '        strMessage = Common.RecordNotSave
                    '        Return False
                    '    End If
            End Select


            If objDb.BatchExecute(Query) Then
                strMessage = Common.RecordSave
                Return True
            Else
                strMessage = Common.RecordNotSave
                Return False
            End If
        End Function

        Public Function GetContractInfo(ByVal strTxProId As String, ByVal strRef As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'Fali - 11102011 (Filter by Contract Ref. No - Case 996)
            'Michelle (22/11/2011) - Issue 1273
            If strRef <> "" Then
                strsql = "SELECT CDM.CDM_GROUP_INDEX, CM.CM_COY_NAME, CDM.CDM_GROUP_CODE, CDM.CDM_START_DATE,CDM.CDM_END_DATE, CDI.CDI_CURRENCY_CODE, CDI.CDI_UNIT_COST, CDI.CDI_GST, " &
                         "CASE WHEN (CDI.CDI_GST_RATE IS NULL OR CDI.CDI_GST_RATE = 'N/A') THEN 'N/A' ELSE " &
                         "IF((TAX_PERC = '' OR TAX_PERC IS NULL), IFNULL(CODE_DESC,'N/A'), " &
                         "CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, IFNULL(CDI.CDI_GST_TAX_CODE,'') AS CDI_GST_TAX_CODE " &
                         "FROM CONTRACT_DIST_ITEMS CDI " &
                         "INNER JOIN CONTRACT_DIST_MSTR CDM ON CDM.CDM_GROUP_INDEX = CDI.CDI_GROUP_INDEX " &
                         "INNER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID = CDM.CDM_S_COY_ID " &
                         "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI.CDI_GST_RATE " &
                         "LEFT JOIN TAX ON TAX_CODE = CDI.CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                         "WHERE cdi_product_code = '" & strTxProId & "' AND CDM_END_DATE >= CURRENT_DATE() AND CDM_GROUP_CODE = '" & strRef & "'"
            Else
                strsql = "SELECT CDM.CDM_GROUP_INDEX, CM.CM_COY_NAME, CDM.CDM_GROUP_CODE, CDM.CDM_START_DATE,CDM.CDM_END_DATE, CDI.CDI_CURRENCY_CODE, CDI.CDI_UNIT_COST, CDI.CDI_GST, " &
                        "CASE WHEN (CDI.CDI_GST_RATE IS NULL OR CDI.CDI_GST_RATE = 'N/A') THEN 'N/A' ELSE " &
                        "IF((TAX_PERC = '' OR TAX_PERC IS NULL), IFNULL(CODE_DESC,'N/A'), " &
                        "CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, IFNULL(CDI.CDI_GST_TAX_CODE,'') AS CDI_GST_TAX_CODE  " &
                        "FROM CONTRACT_DIST_ITEMS CDI " &
                        "INNER JOIN CONTRACT_DIST_MSTR CDM ON CDM.CDM_GROUP_INDEX = CDI.CDI_GROUP_INDEX " &
                        "INNER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID = CDM.CDM_S_COY_ID " &
                        "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI.CDI_GST_RATE " &
                        "LEFT JOIN TAX ON TAX_CODE = CDI.CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                        "WHERE cdi_product_code = '" & strTxProId & "' AND CDM_END_DATE >= CURRENT_DATE()"
            End If

            ds = objDb.FillDs(strsql)

            Return ds
        End Function

        Public Function GetConCatGroupCode(ByVal strProdCode As String, ByVal strBCoyID As String) As String
            Dim strsql As String

            'Joon - 20th Oct 2011 (Case 1046)
            strsql = "SELECT CDM_GROUP_CODE " _
                & "FROM contract_dist_mstr " _
                & "INNER JOIN contract_dist_items ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
                & "INNER JOIN contract_dist_coy ON CDC_GROUP_INDEX=CDM_GROUP_INDEX " _
                & "WHERE CDI_PRODUCT_CODE='" & strProdCode & "' AND CDC_B_COY_ID='" & Common.Parse(strBCoyID) & "' " _
                & "AND CDM_END_DATE >= CURRENT_DATE() "

            If objDb.GetVal(strsql) <> "" Then
                GetConCatGroupCode = objDb.GetVal(strsql)

            Else
                GetConCatGroupCode = ""
            End If

        End Function

        Function GetItemInfoToExcel(ByVal strField As String, Optional ByVal strCompanyId As String = "") As DataSet
            Dim strSql, strCompId As String
            Dim ds As New DataSet

            If strCompanyId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            Else
                strCompId = strCompanyId
            End If

            If strField = "TC" Then 'GST Tax Code
                strSql = "SELECT TM_TAX_CODE AS 'GST Tax Code' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_COUNTRY_CODE = 'MY' AND TM_DELETED <> 'Y' AND TM_CATEGORY = 'eProcure' ORDER BY TM_TAX_CODE"
            ElseIf strField = "GST" Then 'GST Rate
                strSql = "SELECT IF((TAX_PERC = '' OR TAX_PERC IS NULL), CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS 'GST Rate' " &
                        "FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED <> 'Y'"
            ElseIf strField = "SST" Then 'SST Rate
                strSql = "SELECT IF((TAX_PERC = '' OR TAX_PERC IS NULL), CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS 'SST Rate' " &
                        "FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED <> 'Y'"
            ElseIf strField = "SSTTC" Then 'SST Tax Code
                strSql = "SELECT TM_TAX_CODE AS 'SST Tax Code' FROM TAX_MSTR INNER JOIN CODE_MSTR ON CODE_ABBR=TM_TAX_RATE " &
                        "WHERE TM_TAX_TYPE = 'P' AND TM_COUNTRY_CODE = 'MY' AND TM_DELETED <> 'Y' AND TM_CATEGORY = 'eProcure' " &
                        "AND CODE_CATEGORY='SST' AND CODE_DELETED='N' ORDER BY TM_TAX_CODE "
            Else
                strSql = ""
            End If

            ds = objDb.FillDs(strSql)
            GetItemInfoToExcel = ds
        End Function
    End Class
End Namespace

