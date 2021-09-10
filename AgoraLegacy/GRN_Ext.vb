Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class GRN_Ext
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals
        Public GRN_Id As String

        Public Function IsGRNCreated(ByVal strDONo As String, ByVal intDOIdx As Integer) As Boolean
            Dim SqlQuery As String
            SqlQuery = "Select 'X' from GRN_MSTR WHERE GM_DO_INDEX = " & intDOIdx
            If objDb.Exist(SqlQuery) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsIQCCreated(ByVal strIQCNo As String, ByVal strCoyId As String) As Boolean
            Dim SqlQuery As String
            SqlQuery = "SELECT 'X' FROM INVENTORY_VERIFY_LOT, INVENTORY_VERIFY, INVENTORY_MSTR " &
                        "WHERE IVL_VERIFY_INDEX = IV_VERIFY_INDEX " &
                        "AND IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                        "AND IVL_IQC_NO = " & strIQCNo & " AND IM_COY_ID = '" & strCoyId & "' "
            If objDb.Exist(SqlQuery) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GRNSubmit(ByVal dsGRN As DataSet, ByRef strNewGRNNo As String, ByVal strGRNType As String, ByVal blnRejectAll As Boolean, ByVal aryLoc As ArrayList, ByVal blnStk As Boolean, ByRef blnQC As Boolean, ByRef strNewIQCNum As String) As Boolean
            Dim strAryQuery(0) As String
            Dim SqlQuery As String
            Dim strGRNPrefix, strCoyID, strLoginUser As String
            Dim strIQCPrefix, strNewIQCNo As String
            Dim OutStanding, Ordered, TotalDo_Qty, Do_Quantity As Decimal
            Dim dteNow As DateTime = Now()
            Dim ds As DataSet
            Dim intTotRecord, intGRNStatus, intGRNLevel, intDOStatus As Integer
            Dim intIncrementNo As Integer = 0
            Dim blnFound As Boolean
            Dim aryCost As New ArrayList()

            ' Yap: Inventory Module 15/Apr/2011
            Dim arySetLocation As New ArrayList()
            arySetLocation = aryLoc
            Dim i As Integer = 0
            Dim PM_LOC_INDEX As String
            Dim PM_PRODUCT_INDEX, IM_INVENTORY_NAME As String
            Dim IV_VERIFY_INDEX, IVL_VERIFY_LOT_INDEX, APP_GRP_INDEX As String
            Dim IM_PRODUCT_INDEX, PM_PRODUCT, PM_PRODUCT_DESC, PM_IQC_TYPE, PM_ITEM_TYPE As String
            Dim QC As String

            ' Insert Default Location
            Dim storItem, storItem_temp, LocDesc, SubLocDesc As String
            Dim iTotal As Decimal = 0
            Dim objINV As New Inventory

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))

            '//ADD BY MOO
            Dim strSCoyID, strPONo, strDONo, strGRNNo, strFlag As String
            Dim intPOIdx, intDOIdx, intGRNIdx As Integer
            strSCoyID = Common.Parse(dsGRN.Tables(0).Rows(0)("SCoyID"))
            strPONo = dsGRN.Tables(0).Rows(0)("PONo")
            strDONo = dsGRN.Tables(0).Rows(0)("DONo")
            intPOIdx = dsGRN.Tables(0).Rows(0)("POIndex")
            intDOIdx = dsGRN.Tables(0).Rows(0)("DOIndex")

            If UCase(strGRNType) <> "GRNACK" Then
                'objGlobal.GetLatestDocNo("GRN", strAryQuery, strNewGRNNo, strGRNPrefix)
                'SqlQuery = "SELECT '*' FROM GRN_MSTR WHERE GM_GRN_NO='" & strNewGRNNo & "' AND GM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
                'If objDb.Exist(SqlQuery) Then
                '    strNewGRNNo = "dup"
                '    Return False
                'End If
                SqlQuery = " SET @DUPLICATE_CHK =''; SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                intIncrementNo = 1
                strNewGRNNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                strGRNPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN') "

                'strNewGRNNo = Common.Parse(strNewGRNNo)
                strGRNNo = strNewGRNNo

                '2 Level Receiving , CS_COY_ID , CS_FLAG_VALUE
                SqlQuery = "SELECT ISNULL(CS_FLAG_VALUE,0) from COMPANY_SETTING WHERE CS_COY_ID = '" & strCoyID &
                "' AND CS_FLAG_NAME = '2 Level Receiving'"
                strFlag = objDb.GetVal(SqlQuery)
                If strFlag = "0" Then
                    intGRNLevel = 1 '0 CHANGE BY MOO
                    intGRNStatus = GRNStatus.Uninvoice
                Else
                    If blnRejectAll Then
                        intGRNLevel = 1 '0 CHANGE BY MOO
                        intGRNStatus = GRNStatus.Uninvoice
                    Else
                        intGRNLevel = 2 '1 CHANGE BY MOO
                        intGRNStatus = GRNStatus.PendingACK
                    End If
                End If

                SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IFNULL(GM_GRN_NO,'') AS CHAR(1000)) FROM GRN_MSTR WHERE GM_B_COY_ID='" & Common.Parse(strCoyID) & "' AND GM_DO_INDEX = " & intDOIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)

                If IsGRNCreated(strDONo, intDOIdx) Then
                    strNewGRNNo = "exist1" 'GRN Created 
                    Return False
                End If

                SqlQuery = "Insert into GRN_Mstr(GM_GRN_NO,GM_B_COY_ID,GM_PO_INDEX,GM_DATE_RECEIVED," &
                    " GM_NOOFCOPY_PRINTED, GM_DO_INDEX, GM_GRN_PREFIX, GM_S_COY_ID," &
                    " GM_GRN_STATUS, GM_GRN_LEVEL, GM_CREATED_BY, GM_CREATED_DATE)" &
                    " VALUES(" & strNewGRNNo & ",'" & strCoyID & "'," & intPOIdx & "," & Common.ConvertDate(dsGRN.Tables(0).Rows(0)("GRNReceivedDt")) &
                    ", 0," & intDOIdx & "," & strGRNPrefix & ",'" & strSCoyID & "'," &
                    intGRNStatus & "," & intGRNLevel & ",'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Else
                SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(ISNULL(GM_GRN_STATUS,1)= " & GRNStatus.Uninvoice & ",'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM GRN_MSTR WHERE GM_GRN_INDEX = " & intGRNIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = "SELECT ISNULL(GM_GRN_STATUS,1) FROM GRN_MSTR WHERE GM_GRN_INDEX=" & intGRNIdx
                If objDb.GetVal(SqlQuery) = GRNStatus.Uninvoice Then
                    strNewGRNNo = "exist2" 'GRN already ack
                    Return False
                End If

                intGRNStatus = GRNStatus.Uninvoice
                strGRNNo = dsGRN.Tables(0).Rows(0)("GRNNo")
                intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
                SqlQuery = "UPDATE GRN_Mstr set GM_LEVEL2_USER = '" & strLoginUser & "',GM_GRN_STATUS=" &
                intGRNStatus & " Where GM_GRN_INDEX=" & intGRNIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)
            End If

            ''//DOBULE CHECK
            'If UCase(strGRNType) <> "GRNACK" Then
            '    SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IFNULL(GM_GRN_NO,'') AS CHAR(1000)) FROM GRN_MSTR WHERE GM_B_COY_ID='" & Common.Parse(strCoyID) & "' AND GM_DO_INDEX = " & intDOIdx
            '    Common.Insert2Ary(strAryQuery, SqlQuery)

            '    If IsGRNCreated(strDONo, intDOIdx) Then
            '        strNewGRNNo = "exist1" 'GRN Created 
            '        Return False
            '    End If

            'Else
            '    SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(ISNULL(GM_GRN_STATUS,1)= " & GRNStatus.Uninvoice & ",'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM GRN_MSTR WHERE GM_GRN_INDEX = " & intGRNIdx
            '    Common.Insert2Ary(strAryQuery, SqlQuery)

            '    SqlQuery = "SELECT ISNULL(GM_GRN_STATUS,1) FROM GRN_MSTR WHERE GM_GRN_INDEX=" & intGRNIdx
            '    If objDb.GetVal(SqlQuery) = GRNStatus.Uninvoice Then
            '        strNewGRNNo = "exist2" 'GRN already ack
            '        Return False
            '    End If
            'End If

            Dim dtGRNDtls As DataTable
            Dim drGRNDtl As DataRow
            Dim GRNLevel As Integer
            Dim dsGRNIdx As DataSet
            Dim intItemRow As Integer = 1
            Dim ifound As Integer

            Dim Received, Rejected, DiffQty, decLandCost As Decimal
            dtGRNDtls = dsGRN.Tables(1) ' fr dtGRNDtls
            For Each drGRNDtl In dtGRNDtls.Rows
                If UCase(strGRNType) <> "GRNACK" Then
                    GRNLevel = 1
                    SqlQuery = "INSERT into GRN_Details(GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE," &
                           "GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS "

                    'If blnStk = True Then
                    SqlQuery = SqlQuery & ",GD_OTH_CHARGE, GD_DUTIES, GD_INLAND_CHARGE, GD_EXCHANGE_RATE, GD_FACTOR"
                    'End If

                    SqlQuery = SqlQuery & ")VALUES('" & strCoyID & "'," & strNewGRNNo & "," & drGRNDtl("PO_LINE") &
                           "," & drGRNDtl("Received_Qty") & "," & drGRNDtl("Rejected_Qty") &
                           ",'" & Common.Parse(drGRNDtl("REMARKS")) & "'"

                    'If blnStk = True Then
                    SqlQuery = SqlQuery & "," & drGRNDtl("Oth_Charge") & ", " & drGRNDtl("Duties") &
                            ", " & drGRNDtl("Inland_Charge") & ", " & drGRNDtl("Exchange_rate") & ", " & drGRNDtl("Factor")
                    'End If

                    SqlQuery = SqlQuery & ")"

                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Else
                    GRNLevel = 2
                    SqlQuery = " UPDATE GRN_Details SET GD_REJECTED_QTY = " & drGRNDtl("Rejected_Qty") &
                            ",GD_REMARKS='" & Common.Parse(drGRNDtl("REMARKS")) & "' "

                    'If blnStk = True Then
                    SqlQuery = SqlQuery & ",GD_OTH_CHARGE = " & drGRNDtl("Oth_Charge") & ",GD_DUTIES = " & drGRNDtl("Duties") &
                        ",GD_INLAND_CHARGE=" & drGRNDtl("Inland_Charge") & ", GD_EXCHANGE_RATE=" & drGRNDtl("Exchange_rate") &
                        ",GD_FACTOR=" & drGRNDtl("Factor")
                    'End If

                    SqlQuery = SqlQuery & " where GD_B_COY_ID ='" & strCoyID &
                            "' and GD_GRN_NO = '" & strGRNNo & "'" &
                            " and GD_PO_LINE =" & drGRNDtl("PO_LINE")
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
                'intGRNLevel
                If UCase(strGRNType) <> "GRNACK" Then
                    If intGRNLevel = 2 Then
                        'Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY, GL_REJECTED_QTY,GL_REMARKS, 
                        'GL_ACTION_BY,GL_ACTION_DT) SELECT (Select max(GM_GRN_INDEX) from GRN_Mstr)+1,1,1,1,0,'','myongnc',
                        'Convert(DateTime,'03/01/2005 15:09:55',103)
                        SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," &
                        " GL_REJECTED_QTY,GL_REMARKS," &
                        " GL_ACTION_BY,GL_ACTION_DT) SELECT (SELECT ISNULL(MAX(GM_GRN_INDEX),1) From GRN_Mstr)" &
                        "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") &
                        "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser &
                        "'," & Common.ConvertDate(Now())
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Else
                    intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
                    SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," &
                    " GL_REJECTED_QTY,GL_REMARKS,GL_ACTION_BY,GL_ACTION_DT) " &
                    "VALUES(" &
                    intGRNIdx & "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") &
                    "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser &
                    "'," & Common.ConvertDate(Now()) & ")"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If

                If intGRNStatus = GRNStatus.Uninvoice Then
                    'Update PO_Details
                    'POD_DELIVERED_QTY
                    SqlQuery = "UPDATE PO_DETAILS SET" &
                    " POD_RECEIVED_QTY = POD_RECEIVED_QTY + " & drGRNDtl("Received_Qty") &
                    " , POD_REJECTED_QTY = POD_REJECTED_QTY + " & drGRNDtl("Rejected_Qty") &
                    " , POD_DELIVERED_QTY = POD_DELIVERED_QTY - " & drGRNDtl("Rejected_Qty") &
                    " WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strCoyID & "' " &
                    " And POD_PO_LINE = " & drGRNDtl("PO_LINE")
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    If drGRNDtl("Rejected_Qty") <> 0 Then
                        'Update DO_Details
                        SqlQuery = "UPDATE DO_DETAILS SET DOD_DO_QTY = DOD_DO_QTY - " & drGRNDtl("Rejected_Qty") &
                        " WHERE DOD_S_COY_ID ='" & strSCoyID & "'" &
                        " and DOD_DO_NO = '" & strDONo & "'" &
                        " And DOD_PO_LINE = " & drGRNDtl("PO_LINE") & ""
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                    End If
                    Received = Received + drGRNDtl("Received_Qty")
                    Rejected = Rejected + drGRNDtl("Rejected_Qty")

                    ' Yap: Inventory Module 15/Apr/2011
                    SqlQuery = " SELECT POD_VENDOR_ITEM_CODE FROM PO_DETAILS " &
                               " WHERE POD_PO_LINE = " & drGRNDtl("PO_LINE") & " AND POD_COY_ID = '" & strCoyID & "' AND POD_PO_NO = '" & strPONo & "'"
                    PM_PRODUCT = objDb.GetVal(SqlQuery)

                    'Chee Hong (For eMRS - Insert record into Inventory_cost table)
                    SqlQuery = "SELECT PM_ITEM_TYPE FROM PRODUCT_MSTR, PO_DETAILS " &
                            "WHERE PM_S_COY_ID = POD_COY_ID And PM_VENDOR_ITEM_CODE = POD_VENDOR_ITEM_CODE " &
                            "AND POD_PO_NO = '" & strPONo & "' AND POD_COY_ID = '" & strCoyID & "' AND POD_PO_LINE = " & drGRNDtl("PO_LINE")
                    PM_ITEM_TYPE = objDb.GetVal(SqlQuery)

                    If PM_ITEM_TYPE = "ST" And (drGRNDtl("Received_Qty") <> drGRNDtl("Rejected_Qty")) Then
                        'Calculate Qty & get PM_PRODUCT_INDEX, PM_PRODUCT_DESC
                        DiffQty = drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")
                        decLandCost = drGRNDtl("LandedCost") / DiffQty

                        SqlQuery = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & Common.Parse(PM_PRODUCT) & "' "
                        PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                        SqlQuery = "SELECT POD_PRODUCT_DESC FROM PO_DETAILS " &
                                "WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID = '" & strCoyID & "' AND POD_PO_LINE = " & drGRNDtl("PO_LINE")
                        PM_PRODUCT_DESC = objDb.GetVal(SqlQuery)

                        Dim blnCost As Boolean
                        If aryCost Is Nothing Then
                            aryCost.Add(New String() {DiffQty, decLandCost, PM_PRODUCT_INDEX, PM_PRODUCT_DESC})
                        Else
                            If aryCost.Count > 0 Then
                                blnCost = False
                                For i = 0 To aryCost.Count - 1
                                    If PM_PRODUCT_INDEX = aryCost(i)(2) Then
                                        blnCost = True
                                        Exit For
                                    End If
                                Next

                                If blnCost = True Then
                                    aryCost(i)(0) = CDec(aryCost(i)(0)) + DiffQty 'Add Qty
                                    aryCost(i)(1) = CDec(aryCost(i)(1)) + decLandCost 'Add LandCost
                                Else
                                    aryCost.Add(New String() {DiffQty, decLandCost, PM_PRODUCT_INDEX, PM_PRODUCT_DESC})
                                End If
                            Else
                                aryCost.Add(New String() {DiffQty, decLandCost, PM_PRODUCT_INDEX, PM_PRODUCT_DESC})
                            End If
                        End If

                        'SqlQuery = "SELECT '*' FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "'"
                        'If objDb.Exist(SqlQuery) > 0 Then
                        '    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " & _
                        '            "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " & _
                        '            "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'GRN', " & strGRNNo & ", '" & PM_PRODUCT_INDEX & "', " & _
                        '            "'" & Common.Parse(PM_PRODUCT_DESC) & "', '" & strCoyID & "', " & _
                        '            "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & DiffQty & ", " & decLandCost & ", " & decLandCost * DiffQty & ", " & _
                        '            "IC_COST_CLOSE_QTY+" & DiffQty & "," & _
                        '            "(IC_COST_CLOSE_COST+" & (decLandCost * DiffQty) & ") / (IC_COST_CLOSE_QTY+" & DiffQty & ")," & _
                        '            "IC_COST_CLOSE_COST+" & (decLandCost * DiffQty) & " " & _
                        '            "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                        '    Common.Insert2Ary(strAryQuery, SqlQuery)
                        'Else
                        '    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " & _
                        '            "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) VALUES " & _
                        '            "(YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'GRN', " & strGRNNo & ", '" & PM_PRODUCT_INDEX & "', " & _
                        '            "'" & Common.Parse(PM_PRODUCT_DESC) & "', '" & strCoyID & "', " & _
                        '            "0, 0, 0, " & DiffQty & ", " & decLandCost & ", " & decLandCost * DiffQty & ", " & DiffQty & ", " & decLandCost & ", " & decLandCost * DiffQty & ")"
                        '    Common.Insert2Ary(strAryQuery, SqlQuery)
                        'End If

                    End If

                    iTotal = 0
                    '' ''For i = 0 To arySetLocation.Count - 1
                    '' ''    If PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;" Then
                    '' ''        'If i = 0 Then
                    '' ''        '    storItem = arySetLocation(i)(3)
                    '' ''        '    storItem_temp = arySetLocation(i)(3)
                    '' ''        'End If
                    '' ''        'storItem_temp = arySetLocation(i)(3)
                    '' ''        'iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

                    '' ''        'If (storItem <> storItem_temp) Or (i = arySetLocation.Count - 1) Then
                    '' ''        '    storItem = arySetLocation(i)(3)
                    '' ''        '    If iTotal < (Received + Rejected) Then
                    '' ''        '        objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
                    '' ''        '        arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((Received + Rejected) - iTotal), arySetLocation(i)(3), ""})
                    '' ''        '    Else
                    '' ''        '        strNewGRNNo = "LocationError"
                    '' ''        '        Return False
                    '' ''        '    End If
                    '' ''        '    iTotal = 0
                    '' ''        'End If
                    '' ''        iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

                    '' ''    End If
                    '' ''Next

                    objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
                    ' yAP: This to check each item total quantity inside the SET.
                    For i = 0 To arySetLocation.Count - 1
                        If blnStk = True Then
                            If (PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;") _
                                And arySetLocation(i)(2) <> "" And arySetLocation(i)(9) <> "" And (CInt(IIf(arySetLocation(i)(8) = "", 0, arySetLocation(i)(8))) = drGRNDtl("PO_LINE")) Then
                                iTotal = iTotal + CDec(arySetLocation(i)(2))
                            ElseIf (PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;") _
                                And arySetLocation(i)(2) <> "" And arySetLocation(i)(9) = "" And (CInt(IIf(arySetLocation(i)(8) = "", 0, arySetLocation(i)(8))) = drGRNDtl("PO_LINE")) Then
                                arySetLocation(i)(2) = ""
                            End If
                        Else
                            If (PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;") _
                                And arySetLocation(i)(2) <> "" And (CInt(IIf(arySetLocation(i)(8) = "", 0, arySetLocation(i)(8))) = drGRNDtl("PO_LINE")) Then
                                iTotal = iTotal + CDec(arySetLocation(i)(2))
                            End If
                        End If
                    Next

                    ' yAP: This to put the balance quantity to default location.
                    If iTotal < (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
                        If blnStk = True Then
                            If iTotal = 0 Then
                                ifound = 0

                                'Clear record for item
                                Do While ifound < arySetLocation.Count
                                    If arySetLocation(ifound)(3) = PM_PRODUCT And (CInt(IIf(arySetLocation(ifound)(8) = "", 0, arySetLocation(ifound)(8))) = drGRNDtl("PO_LINE")) Then
                                        arySetLocation.RemoveAt(ifound)
                                        ifound = 0
                                    Else
                                        ifound = ifound + 1
                                    End If
                                Loop

                                'Insert records for item
                                Dim dsLot As New DataSet
                                dsLot = GetLocLot(strDONo, PM_PRODUCT, strSCoyID, drGRNDtl("PO_LINE"))

                                For i = 0 To dsLot.Tables(0).Rows.Count - 1
                                    'arySetLocation.Add(New String() {LocDesc, SubLocDesc, dsLot.Tables(0).Rows(i)(1), PM_PRODUCT, "", "", "", "", intItemRow, dsLot.Tables(0).Rows(i)(2)})
                                    arySetLocation.Add(New String() {LocDesc, SubLocDesc, dsLot.Tables(0).Rows(i)(1), PM_PRODUCT, "", "", "", "", drGRNDtl("PO_LINE"), dsLot.Tables(0).Rows(i)(2)})
                                Next
                            Else
                                strNewGRNNo = "SetLocError"
                                Return False
                            End If

                        Else
                            arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) - iTotal), PM_PRODUCT, "", "", "", "", drGRNDtl("PO_LINE")})
                            'arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) - iTotal), PM_PRODUCT, "", "", "", "", intItemRow})
                        End If

                        'objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
                    ElseIf iTotal > (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
                        'strNewGRNNo = "LocationError"
                        strNewGRNNo = "RejectError"
                        Return False
                    End If

                    If blnStk = True Then
                        SqlQuery = "SELECT IM_IQC_IND FROM Inventory_mstr WHERE IM_ITEM_CODE = '" & PM_PRODUCT & "' AND IM_COY_ID = '" & strCoyID & "'"
                        QC = objDb.GetVal(SqlQuery)

                        If QC = "Y" Then
                            SqlQuery = "SELECT AGP_GRP_INDEX FROM APPROVAL_GRP_PRODUCT " &
                                       "WHERE AGP_PRODUCT_CODE = (SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(PM_PRODUCT) & "' AND PM_S_COY_ID = '" & strCoyID & "') "
                            APP_GRP_INDEX = objDb.GetVal(SqlQuery)

                            If APP_GRP_INDEX = "" Then
                                strNewGRNNo = "NoApproval"
                                Return False
                            End If
                        End If
                    End If
                    'Michelle (10/1/2013) - Issue 1835
                    'intItemRow += intItemRow
                    intItemRow += 1
                End If
            Next

            'Insert/ Update into Costing table
            If aryCost.Count > 0 Then
                For i = 0 To aryCost.Count - 1
                    SqlQuery = "SELECT '*' FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & aryCost(i)(2) & "'"
                    If objDb.Exist(SqlQuery) > 0 Then
                        SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                                "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                                "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'GRN', " & strGRNNo & ", '" & aryCost(i)(2) & "', " &
                                "'" & Common.Parse(aryCost(i)(3)) & "', '" & strCoyID & "', " &
                                "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & CDec(aryCost(i)(0)) & ", " & CDec(aryCost(i)(1)) & ", " & CDec(aryCost(i)(1)) * CDec(aryCost(i)(0)) & ", " &
                                "IC_COST_CLOSE_QTY+" & CDec(aryCost(i)(0)) & "," &
                                "(IC_COST_CLOSE_COST+" & (CDec(aryCost(i)(1)) * CDec(aryCost(i)(0))) & ") / (IC_COST_CLOSE_QTY+" & CDec(aryCost(i)(0)) & ")," &
                                "IC_COST_CLOSE_COST+" & (CDec(aryCost(i)(1)) * CDec(aryCost(i)(0))) & " " &
                                "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & aryCost(i)(2) & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    Else
                        SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                                "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) VALUES " &
                                "(YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'GRN', " & strGRNNo & ", '" & aryCost(i)(2) & "', " &
                                "'" & Common.Parse(aryCost(i)(3)) & "', '" & strCoyID & "', " &
                                "0, 0, 0, " & CDec(aryCost(i)(0)) & ", " & CDec(aryCost(i)(1)) & ", " & CDec(aryCost(i)(1)) * CDec(aryCost(i)(0)) & ", " & CDec(aryCost(i)(0)) & ", " & CDec(aryCost(i)(1)) & ", " & CDec(aryCost(i)(1)) * CDec(aryCost(i)(0)) & ")"
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Next
            End If

            If intGRNStatus = GRNStatus.Uninvoice Then
                If Rejected = 0 Then
                    intDOStatus = DOStatus.FullyAccepted
                ElseIf Received <> Rejected Then
                    intDOStatus = DOStatus.PartiallyAccepted
                ElseIf Received = Rejected Then
                    intDOStatus = DOStatus.Rejected
                End If

                '//pending. if there is rejection. need to update fulfilment or not
                If intDOStatus = DOStatus.PartiallyAccepted Or intDOStatus = DOStatus.Rejected Then
                    Dim objDO As New DeliveryOrder
                    SqlQuery = objDO.SetPOFulFilment(strPONo, strCoyID)
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
                'Updata DO_Mstr
                'remark by Moo, DOM_GRN_Index is taken out
                SqlQuery = "UPDATE DO_MSTR SET DOM_DO_Status =" & intDOStatus &
                " WHERE DOM_PO_Index = " & intPOIdx &
                " AND DOM_DO_INDEX = " & intDOIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)
            End If

            Dim objTrans As New Tracking
            If UCase(strGRNType) <> "GRNACK" Then
                SqlQuery = objTrans.updateDocMatchingNew(strPONo, strNewGRNNo, strDONo, "GRN", strSCoyID, strCoyID)
            Else
                SqlQuery = objTrans.updateDocMatching(strPONo, strNewGRNNo, strDONo, "GRN", strSCoyID, strCoyID)
            End If

            Common.Insert2Ary(strAryQuery, SqlQuery)
            objTrans = Nothing

            Dim objUsers As New Users
            If UCase(strGRNType) <> "GRNACK" Then
                objUsers.Log_UserActivityNew(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRN, strNewGRNNo, strDONo)
            Else
                objUsers.Log_UserActivity(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRNACK, strNewGRNNo, strDONo)
            End If
            objUsers = Nothing

            ' Yap: Inventory Module 15/Apr/2011 
            ' Check Duplicate
            Dim tempItem, tempLoc, tempSLoc, tempLot, tempLine As String
            Dim tempQty As Decimal = 0
            Dim j As Integer
            Dim aryLocation As New ArrayList()
            Dim aryTemp As New ArrayList()
            Dim arySetLoc As New ArrayList()
            Dim aryLot As New ArrayList()
            If blnStk = True Then
                'Chee Hong eProcurement Module 18/Jun/2013
                'Check Duplicate for item, loc, sub loc, lot no, item line
                For i = 0 To arySetLocation.Count - 1
                    tempItem = arySetLocation(i)(3)
                    tempLoc = arySetLocation(i)(0)
                    tempSLoc = arySetLocation(i)(1)
                    tempLot = arySetLocation(i)(9)
                    tempLine = arySetLocation(i)(8)
                    For j = 0 To arySetLocation.Count - 1
                        If (arySetLocation(j)(4) <> "Done") And tempItem = arySetLocation(j)(3) And tempLoc = arySetLocation(j)(0) And tempSLoc = arySetLocation(j)(1) And tempLot = arySetLocation(j)(9) And tempLine = arySetLocation(j)(8) Then
                            arySetLocation(j)(0) = "---Select---"
                            arySetLocation(j)(4) = "Done"
                            tempQty = tempQty + IIf(arySetLocation(j)(2) = "", 0, arySetLocation(j)(2))
                        End If
                    Next
                    aryTemp.Add(New String() {tempLoc, tempSLoc, tempQty, tempItem, "Done", "", "", "", tempLine, tempLot, ""})
                    tempQty = 0
                Next

                arySetLocation.Clear()

                'Rearrange records
                For i = 0 To aryTemp.Count - 1
                    If Common.parseNull(aryTemp(i)(8)) <> "" Then
                        If (aryTemp(i)(4) = "Done" And aryTemp(i)(0) <> "---Select---") Then
                            arySetLocation.Add(New String() {aryTemp(i)(0), aryTemp(i)(1), aryTemp(i)(2), aryTemp(i)(3), "Save", "", "", "", aryTemp(i)(8), aryTemp(i)(9), ""})
                        End If
                    End If
                Next

                aryTemp.Clear()

                'Insert records into GRN Lot table (new)
                For i = 0 To arySetLocation.Count - 1
                    If Common.parseNull(arySetLocation(i)(8)) <> "" Then
                        If Common.parseNull(arySetLocation(i)(1)) = "" Then
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        Else
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' AND LM_SUB_LOCATION = '" & arySetLocation(i)(1) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        End If

                        SqlQuery = " INSERT INTO GRN_LOT (GL_B_COY_ID, GL_GRN_NO, GL_PO_LINE, GL_LOCATION_INDEX, GL_LOT_INDEX, GL_LOT_RECEIVED_QTY) " &
                            " VALUES ('" & strCoyID & "', " & strNewGRNNo & ", " & arySetLocation(i)(8) & ", '" & PM_LOC_INDEX & "', " & arySetLocation(i)(9) & ", " & arySetLocation(i)(2) & ") "

                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Next

                'Chee Hong IQC Module 21/May/2013
                'Check duplicate record and get columns: Loc, Sub Loc, Item, Lot, Qty
                For i = 0 To arySetLocation.Count - 1
                    blnFound = False
                    If i = 0 Then
                        aryLocation.Add(New String() {arySetLocation(i)(0), arySetLocation(i)(1), arySetLocation(i)(3), arySetLocation(i)(9), IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2))})
                    Else
                        If arySetLocation(i)(0) <> "---Select---" And arySetLocation(i)(3) <> "" Then
                            For j = 0 To aryLocation.Count - 1
                                If aryLocation(j)(0) = arySetLocation(i)(0) And aryLocation(j)(1) = arySetLocation(i)(1) And aryLocation(j)(2) = arySetLocation(i)(3) And aryLocation(j)(3) = arySetLocation(i)(9) Then
                                    If arySetLocation(i)(2) = "" Then
                                        aryLocation(j)(4) = CDec(aryLocation(j)(4)) + 0
                                    Else
                                        aryLocation(j)(4) = CDec(aryLocation(j)(4)) + CDec(arySetLocation(i)(2))
                                    End If
                                    blnFound = True
                                    Exit For
                                End If
                            Next
                            If blnFound = False Then aryLocation.Add(New String() {arySetLocation(i)(0), arySetLocation(i)(1), arySetLocation(i)(3), arySetLocation(i)(9), IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2))})
                        End If
                    End If
                Next

                'Check duplicate record and get columns: Loc, Sub Loc, Item, Qty
                For i = 0 To aryLocation.Count - 1
                    blnFound = False
                    If i = 0 Then
                        arySetLoc.Add(New String() {aryLocation(i)(0), aryLocation(i)(1), aryLocation(i)(2), aryLocation(i)(4)})
                    Else
                        For j = 0 To arySetLoc.Count - 1
                            If arySetLoc(j)(0) = aryLocation(i)(0) And arySetLoc(j)(1) = aryLocation(i)(1) And arySetLoc(j)(2) = aryLocation(i)(2) Then
                                arySetLoc(j)(3) = CDec(arySetLoc(j)(3)) + CDec(aryLocation(i)(4))
                                blnFound = True
                                Exit For
                            End If
                        Next
                        If blnFound = False Then arySetLoc.Add(New String() {aryLocation(i)(0), aryLocation(i)(1), aryLocation(i)(2), aryLocation(i)(4)})
                    End If
                Next

                'Check duplicate record and get columns: Item, Lot No
                For i = 0 To arySetLocation.Count - 1
                    blnFound = False
                    If i = 0 Then
                        aryLot.Add(New String() {arySetLocation(i)(3), arySetLocation(i)(9)})
                    Else
                        If arySetLocation(i)(0) <> "---Select---" And arySetLocation(i)(3) <> "" Then
                            For j = 0 To aryLot.Count - 1
                                If aryLot(j)(0) = arySetLocation(i)(3) And aryLot(j)(1) = arySetLocation(i)(9) Then
                                    blnFound = True
                                    Exit For
                                End If
                            Next
                            If blnFound = False Then aryLot.Add(New String() {arySetLocation(i)(3), arySetLocation(i)(9)})
                        End If
                    End If
                Next

            Else
                For i = 0 To arySetLocation.Count - 1
                    tempItem = arySetLocation(i)(3)
                    tempLoc = arySetLocation(i)(0)
                    tempSLoc = arySetLocation(i)(1)
                    For j = 0 To arySetLocation.Count - 1
                        If (arySetLocation(j)(4) <> "Done") And tempItem = arySetLocation(j)(3) And tempLoc = arySetLocation(j)(0) And tempSLoc = arySetLocation(j)(1) Then
                            arySetLocation(j)(0) = "---Select---"
                            arySetLocation(j)(4) = "Done"
                            tempQty = tempQty + IIf(arySetLocation(j)(2) = "", 0, arySetLocation(j)(2))
                        End If
                    Next
                    arySetLocation.Add(New String() {tempLoc, tempSLoc, tempQty, tempItem, "Done", "", "", "", "", "", ""})
                    tempQty = 0
                Next
            End If

            blnQC = False
            'Chee Hong IQC Module 21/May/2013
            If blnStk = True Then
                Dim k As Integer = 0
                'Update/Insert record into INVENTORY_TRANS, INVENTORY_DETAIL
                For i = 0 To arySetLoc.Count - 1
                    SqlQuery = "SELECT * FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLoc(i)(2) & "'"

                    If objDb.Exist(SqlQuery) Then
                        'Get PM_PRODUCT_INDEX, PM_LOC_INDEX, IM_INVENTORY_NAME, QC
                        SqlQuery = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                   "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLoc(i)(2) & "'"
                        PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                        If Common.parseNull(arySetLoc(i)(1)) = "" Then
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLoc(i)(0) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        Else
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLoc(i)(0) & "' AND LM_SUB_LOCATION = '" & arySetLoc(i)(1) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        End If

                        SqlQuery = "SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                   "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLoc(i)(2) & "'"
                        IM_INVENTORY_NAME = objDb.GetVal(SqlQuery)

                        SqlQuery = "SELECT IM_IQC_IND FROM INVENTORY_MSTR " &
                                   "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLoc(i)(2) & "'"
                        QC = objDb.GetVal(SqlQuery)

                        'Insert record into INVENTORY_TRANS table
                        If UCase(strGRNType) <> "GRNACK" Then
                            SqlQuery = "INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                       "VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLoc(i)(3) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", " & strGRNNo & ", '" & strLoginUser & "','" & Common.Parse(IM_INVENTORY_NAME) & "') "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        Else
                            SqlQuery = "INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                       "VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLoc(i)(3) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", '" & strGRNNo & "', '" & strLoginUser & "','" & Common.Parse(IM_INVENTORY_NAME) & "') "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        End If

                        'Insert/ Update record into INVENTORY_DETAIL table
                        If QC = "Y" Then
                            SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                            'If objDb.Exist(SqlQuery) > 0 Then
                            'Else
                            '    SqlQuery = "INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " & _
                            '               "VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", 0) "
                            '    Common.Insert2Ary(strAryQuery, SqlQuery)
                            'End If

                            If objDb.Exist(SqlQuery) > 0 Then
                                SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & arySetLoc(i)(3) &
                                           " ,ID_IQC_QTY = IFNULL(ID_IQC_QTY,0) + " & arySetLoc(i)(3) &
                                           " WHERE ID_INVENTORY_INDEX = " & PM_PRODUCT_INDEX &
                                           " AND ID_LOCATION_INDEX = " & PM_LOC_INDEX
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                SqlQuery = "INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY, ID_IQC_QTY) " &
                                           "VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & arySetLoc(i)(3) & ", " & arySetLoc(i)(3) & ") "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If

                            'Insert record into INVENTORY_VERIFY
                            If UCase(strGRNType) <> "GRNACK" Then
                                SqlQuery = "INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                           "VALUES(" & strGRNNo & ", '" & PM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLoc(i)(3) & ")"
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                SqlQuery = "INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                           "VALUES('" & strGRNNo & "', '" & PM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLoc(i)(3) & ")"
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If
                        Else
                            SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                            If objDb.Exist(SqlQuery) > 0 Then
                                SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & arySetLoc(i)(3) &
                                           " ,ID_IQC_QTY = IFNULL(ID_IQC_QTY,0) + 0 " &
                                           " WHERE ID_INVENTORY_INDEX = " & PM_PRODUCT_INDEX &
                                           " AND ID_LOCATION_INDEX = " & PM_LOC_INDEX
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                SqlQuery = "INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY, ID_IQC_QTY) " &
                                           "VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & arySetLoc(i)(3) & ", 0) "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If
                        End If

                        'Insert record into INVENTORY_LOT table
                        If QC = "Y" Then
                            For j = 0 To aryLocation.Count - 1
                                If aryLocation(j)(0) = arySetLoc(i)(0) And aryLocation(j)(1) = arySetLoc(i)(1) And aryLocation(j)(2) = arySetLoc(i)(2) Then
                                    SqlQuery = " INSERT INTO INVENTORY_LOT(IL_INVENTORY_INDEX, IL_LOCATION_INDEX, IL_LOT_INDEX, IL_LOT_QTY, IL_IQC_QTY) " &
                                            " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & aryLocation(j)(3) & ", " & aryLocation(j)(4) & ", " & aryLocation(j)(4) & ") "
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                End If
                            Next
                        Else
                            For j = 0 To aryLocation.Count - 1
                                If aryLocation(j)(0) = arySetLoc(i)(0) And aryLocation(j)(1) = arySetLoc(i)(1) And aryLocation(j)(2) = arySetLoc(i)(2) Then
                                    SqlQuery = " INSERT INTO INVENTORY_LOT(IL_INVENTORY_INDEX, IL_LOCATION_INDEX, IL_LOT_INDEX, IL_LOT_QTY, IL_IQC_QTY) " &
                                            " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & aryLocation(j)(3) & ", " & aryLocation(j)(4) & ", 0) "
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                End If
                            Next
                        End If
                    End If
                Next

                SqlQuery = "SET @IQC_T_NO = ''; "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = "SET @IVL_VERIFY_LOT_INDEX = '';"
                Common.Insert2Ary(strAryQuery, SqlQuery)

                Dim iCount As Integer = 0

                'Generate IQC Number
                For i = 0 To aryLot.Count - 1

                    'Get PM_PRODUCT_INDEX, QC, IQC Type
                    SqlQuery = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                               "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & aryLot(i)(0) & "'"
                    IM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                    SqlQuery = "SELECT IM_IQC_IND FROM INVENTORY_MSTR " &
                               "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & aryLot(i)(0) & "'"
                    QC = objDb.GetVal(SqlQuery)

                    SqlQuery = " SELECT IFNULL(PM_IQC_TYPE,'') AS PM_IQC_TYPE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(aryLot(i)(0)) & "' AND PM_S_COY_ID = '" & strCoyID & "' "
                    PM_IQC_TYPE = objDb.GetVal(SqlQuery)

                    If QC = "Y" Then
                        blnQC = True
                        intIncrementNo = 1

                        ' Chee Hong: This to get MAX + 1 from INVENTORY_VERIFY_LOT and store into DB Variable, so can get the latest IVL_VERIFY_LOT_INDEX.
                        SqlQuery = "SET @IVL_VERIFY_LOT_INDEX = (SELECT IFNULL(MAX(IVL_VERIFY_LOT_INDEX), 0) + 1 FROM INVENTORY_VERIFY_LOT);"
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        'IVL_VERIFY_LOT_INDEX = objDb.GetVal("SELECT MAX(IVL_VERIFY_LOT_INDEX) + 1 FROM INVENTORY_VERIFY_LOT")
                        strNewIQCNo = " (SELECT CAST(CONCAT(cpa_param_prefix, REPEAT('0', LENGTH(cpa_param_value) - LENGTH(cpa_param_value + 1)), (cpa_param_value + 1)) " &
                                    "AS CHAR(1000)) cpa_param_value " &
                                    "FROM company_param_additional WHERE CPA_COY_ID = '" & strCoyID & "' " &
                                    "AND CPA_PARAM_TYPE = 'IQC' AND cpa_param_label = '" & PM_IQC_TYPE & "') "

                        If iCount = 0 Then
                            SqlQuery = " SET @IQC_T_NO = " & strNewIQCNo & "; "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        Else
                            'SqlQuery = " SET @IQC_T_NO = CONCAT(CONCAT(@IQC_T_NO,',')," & strNewIQCNo & "); "
                            SqlQuery = " SET @IQC_T_NO = CONCAT(@IQC_T_NO,','," & strNewIQCNo & "); "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        End If

                        For j = 0 To aryLocation.Count - 1
                            If aryLot(i)(0) = aryLocation(j)(2) And aryLot(i)(1) = aryLocation(j)(3) Then

                                'Get PM_LOC_INDEX
                                If Common.parseNull(aryLocation(j)(1)) = "" Then
                                    SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & aryLocation(j)(0) & "' "
                                    PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                                Else
                                    SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & aryLocation(j)(0) & "' AND LM_SUB_LOCATION = '" & aryLocation(j)(1) & "' "
                                    PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                                End If

                                ''Insert record into INVENTORY_VERIFY
                                'If UCase(strGRNType) <> "GRNACK" Then
                                '    SqlQuery = "INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " & _
                                '               "VALUES(" & strGRNNo & ", '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & aryLocation(j)(4) & ")"
                                '    Common.Insert2Ary(strAryQuery, SqlQuery)
                                'Else
                                '    SqlQuery = "INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " & _
                                '               "VALUES('" & strGRNNo & "', '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & aryLocation(j)(4) & ")"
                                '    Common.Insert2Ary(strAryQuery, SqlQuery)
                                'End If

                                IV_VERIFY_INDEX = " (SELECT IV_VERIFY_INDEX FROM INVENTORY_VERIFY WHERE IV_GRN_NO = " & strGRNNo & " AND IV_INVENTORY_INDEX = '" & IM_PRODUCT_INDEX & "' AND IV_LOCATION_INDEX = " & PM_LOC_INDEX & ") "

                                SqlQuery = "INSERT INTO INVENTORY_VERIFY_LOT(IVL_VERIFY_LOT_INDEX, IVL_VERIFY_INDEX, IVL_IQC_NO, IVL_LOT_INDEX, IVL_LOT_QTY) " &
                                           "VALUES (@IVL_VERIFY_LOT_INDEX, " & IV_VERIFY_INDEX & ", " & strNewIQCNo & ", " & aryLot(i)(1) & ", " & aryLocation(j)(4) & ") "
                                Common.Insert2Ary(strAryQuery, SqlQuery)

                                'SqlQuery = "INSERT INTO INVENTORY_VERIFY_LOT(IVL_VERIFY_LOT_INDEX, IVL_VERIFY_INDEX, IVL_IQC_NO, IVL_LOT_INDEX, IVL_LOT_QTY) " & _
                                '           "VALUES (@IVL_VERIFY_LOT_INDEX, " & objDb.GetLatestInsertedID("INVENTORY_VERIFY") & ", " & strNewIQCNo & ", " & aryLot(i)(1) & ", " & aryLocation(j)(4) & ") "
                                'Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If
                        Next

                        SqlQuery = "SELECT AGP_GRP_INDEX FROM APPROVAL_GRP_PRODUCT " &
                                   "WHERE AGP_PRODUCT_CODE = (SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(aryLot(i)(0)) & "' AND PM_S_COY_ID = '" & strCoyID & "') "
                        APP_GRP_INDEX = objDb.GetVal(SqlQuery)

                        'If APP_GRP_INDEX = "" Then
                        '    strNewGRNNo = "NoApproval" 'GRN Created 
                        '    Return False
                        'End If

                        SqlQuery = "INSERT INTO IQC_APPROVAL(IQCA_IQC_INDEX, IQCA_AO, IQCA_A_AO, IQCA_SEQ, IQCA_AO_ACTION, IQCA_APPROVAL_TYPE, IQCA_APPROVAL_GRP_INDEX, IQCA_RELIEF_IND, IQCA_OFFICER_TYPE) " &
                                   "SELECT @IVL_VERIFY_LOT_INDEX, AGI_AO, AGI_A_AO, AGI_SEQ, 0, 1, AGI_GRP_INDEX, 'O', AGI_OFFICER_TYPE FROM APPROVAL_GRP_IQC WHERE AGI_GRP_INDEX=" & APP_GRP_INDEX
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        'If iCount = 0 Then
                        '    SqlQuery = " SET @IQC_T_NO = " & strNewIQCNo & "; "
                        '    Common.Insert2Ary(strAryQuery, SqlQuery)
                        'Else
                        '    SqlQuery = " SET @IQC_T_NO = CONCAT(CONCAT(@IQC_T_NO,',')," & strNewIQCNo & "); "
                        '    Common.Insert2Ary(strAryQuery, SqlQuery)
                        'End If

                        SqlQuery = "UPDATE COMPANY_PARAM_ADDITIONAL SET CPA_PARAM_VALUE = " &
                                "CONCAT(REPEAT('0', LENGTH(cpa_param_value) - LENGTH(cpa_param_value + 1)), (cpa_param_value + 1)) " &
                                "WHERE CPA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                                "AND CPA_PARAM_TYPE = 'IQC' AND cpa_param_label = '" & PM_IQC_TYPE & "' "
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        iCount = iCount + 1
                    End If
                Next

                SqlQuery = "SET @IVL_VERIFY_LOT_INDEX = '';"
                Common.Insert2Ary(strAryQuery, SqlQuery)

            Else
                ' Yap: Inventory Module 15/Apr/2011
                For i = 0 To arySetLocation.Count - 1
                    SqlQuery = " SELECT * FROM INVENTORY_MSTR " &
                                   " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"

                    If objDb.Exist(SqlQuery) Then
                        If arySetLocation(i)(0) <> "---Select---" And arySetLocation(i)(0) <> "" And arySetLocation(i)(2) <> "0" Then

                            SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                       " WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                            PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                            If Common.parseNull(arySetLocation(i)(1)) = "" Then
                                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' "
                                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                            Else
                                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' AND LM_SUB_LOCATION = '" & arySetLocation(i)(1) & "' "
                                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                            End If

                            Dim ItemName As String
                            SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                       "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                            ItemName = objDb.GetVal(SqlQuery)

                            If UCase(strGRNType) <> "GRNACK" Then
                                SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                        " VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLocation(i)(2) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", " & strGRNNo & ", '" & strLoginUser & "','" & Common.Parse(ItemName) & "') "
                                Common.Insert2Ary(strAryQuery, SqlQuery)

                            Else
                                SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                        " VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLocation(i)(2) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", '" & strGRNNo & "', '" & strLoginUser & "','" & Common.Parse(ItemName) & "') "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If

                            SqlQuery = " SELECT IM_IQC_IND FROM INVENTORY_MSTR " &
                                       " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                            QC = objDb.GetVal(SqlQuery)

                            If QC = "Y" Then

                                SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " &
                                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                                If objDb.Exist(SqlQuery) Then

                                Else
                                    SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " &
                                               " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", 0) "
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                End If

                                SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                       " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                                IM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                                If UCase(strGRNType) <> "GRNACK" Then
                                    SqlQuery = " INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                            " VALUES(" & strGRNNo & ", '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLocation(i)(2) & ")"
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                Else
                                    SqlQuery = " INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                            " VALUES('" & strGRNNo & "', '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLocation(i)(2) & ")"
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                End If

                            Else
                                SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " &
                                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                                If objDb.Exist(SqlQuery) Then
                                    SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & arySetLocation(i)(2) &
                                               " WHERE ID_INVENTORY_INDEX = " & PM_PRODUCT_INDEX &
                                               " AND ID_LOCATION_INDEX = " & PM_LOC_INDEX
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                Else
                                    SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " &
                                               " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & arySetLocation(i)(2) & ") "
                                    Common.Insert2Ary(strAryQuery, SqlQuery)
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            SqlQuery = " SET @T_NO = " & strNewGRNNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim strNewGRNNum As String = ""
            Dim strIQC() As String
            Dim intIQCIndex As Integer
            strNewIQCNum = ""
            If UCase(strGRNType) <> "GRNACK" Then
                If objDb.BatchExecuteForGRNDup(strAryQuery, , strNewGRNNum, "T_NO", strNewIQCNum, "IQC_T_NO") Then

                    strNewGRNNo = strNewGRNNum
                    If strNewGRNNum <> "Generated" Then
                        If strNewGRNNum <> "exist2" Then
                            Dim objMail As New Email
                            If intGRNLevel = "1" Then
                                If intDOStatus = DOStatus.Rejected Then
                                    objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                                Else
                                    objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                                End If

                                If blnStk = True And blnQC = True Then
                                    strIQC = Split(strNewIQCNum, ",")
                                    For i = 0 To strIQC.Length - 1
                                        SqlQuery = "SELECT IVL_VERIFY_LOT_INDEX FROM INVENTORY_VERIFY_LOT, INVENTORY_VERIFY, INVENTORY_MSTR " &
                                                "WHERE IVL_VERIFY_INDEX = IV_VERIFY_INDEX " &
                                                "AND IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                                                "AND IVL_IQC_NO = '" & strIQC(i) & "' AND IM_COY_ID = '" & strCoyID & "' "
                                        intIQCIndex = objDb.GetVal(SqlQuery)

                                        'Send email to first level of approving officer
                                        objINV.sendMailToAO(strIQC(i), intIQCIndex, 1)
                                    Next
                                End If
                            Else
                                objMail.sendNotification(EmailType.AckGRN, strLoginUser, strCoyID, strSCoyID, strNewGRNNo, "")
                            End If
                            objMail = Nothing
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    Return False
                End If

            Else
                If objDb.BatchExecute(strAryQuery) Then
                    Dim objMail As New Email
                    If intDOStatus = DOStatus.Rejected Then
                        objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                    Else
                        objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                    End If
                    objMail = Nothing
                    Return True
                Else
                    Return False
                End If

            End If
            'If objDb.BatchExecute(strAryQuery) Then
            '    strNewGRNNum = strNewGRNNo
            '    Dim objMail As New Email
            '    If UCase(strGRNType) <> "GRNACK" Then
            '        If intGRNLevel = "1" Then
            '            If intDOStatus = DOStatus.Rejected Then
            '                objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNum)
            '            Else
            '                objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNum)
            '            End If
            '        Else
            '            objMail.sendNotification(EmailType.AckGRN, strLoginUser, strCoyID, strSCoyID, strNewGRNNum, "")
            '        End If
            '    Else
            '        If intDOStatus = DOStatus.Rejected Then
            '            objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
            '        Else
            '            objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
            '        End If
            '    End If
            '    objMail = Nothing
            '    Return True
            'Else
            '    Return False
            'End If
        End Function

        Public Function ShowDOdetails(ByVal strDONo As String, ByVal intDOIdx As Integer, ByVal IntPOIdx As Integer) As DataSet
            Dim SQLQuery As String
            Dim dsDODtl As DataSet
            'DO_Mstr.DOM_GRN_INDEX ," & _
            SQLQuery = "SELECT DISTINCT POD.POD_CANCELLED_QTY, POM.POM_PO_INDEX, POM.POM_PO_NO, POM.POM_PO_STATUS, POD.POD_PRODUCT_CODE, " &
                    "POD.POD_VENDOR_ITEM_CODE, POD.POD_B_ITEM_CODE, POD.POD_PRODUCT_DESC, POD.POD_ITEM_TYPE, POD.POD_UNIT_COST, " &
                    "POM.POM_EXCHANGE_RATE, POM.POM_DEL_CODE, POD.POD_SPEC1, POD.POD_SPEC2, POD.POD_SPEC3, POD.POD_B_GL_CODE, " &
                    "POM.POM_CURRENCY_CODE, POD.POD_UOM,POD.POD_PO_LINE, CM.CM_COY_NAME, POD.POD_PRODUCT_CODE, POM.POM_S_COY_ID, " &
                    "POD.POD_DELIVERED_QTY, POD.POD_ORDERED_QTY, POD.POD_RECEIVED_QTY, POD.POD_REJECTED_QTY, " &
                    "POD.POD_ETD,POD.POD_MIN_PACK_QTY, POD.POD_MIN_ORDER_QTY, POD.POD_WARRANTY_TERMS, " &
                    "DOM.*, DOD.*,0 AS POD_OUTSTANDING, 0 AS GD_REJECTED_QTY " &
                    "FROM PO_MSTR POM, PO_DETAILS POD ,COMPANY_MSTR CM, DO_MSTR DOM, DO_DETAILS DOD " &
                    "WHERE DOM.DOM_DO_INDEX  = '" & intDOIdx & "' AND POM.POM_PO_INDEX = " & IntPOIdx & " " &
                    "AND POM.POM_B_COY_ID  = '" & HttpContext.Current.Session("CompanyID") & "' " &
                    "AND DOM.DOM_PO_INDEX = POM.POM_PO_INDEX AND DOM.DOM_DO_NO = DOD.DOD_DO_NO " &
                    "AND DOM.DOM_S_COY_ID = DOD.DOD_S_COY_ID AND POM.POM_PO_No = POD.POD_Po_No " &
                    "AND POM.POM_B_COY_ID = POD.POD_Coy_ID AND DOD.DOD_PO_LINE = POD.POD_PO_LINE " &
                    "AND CM.CM_COY_ID = DOM.DOM_S_COY_ID ORDER BY POD_PO_LINE "


            'SQLQuery = " SELECT Distinct PO_Details.POD_CANCELLED_QTY,PO_MSTR.POM_PO_Index,PO_Mstr.POM_PO_No,PO_Mstr.POM_PO_Status," & _
            '    " PO_Details.POD_Product_Code, PO_Details.POD_Vendor_Item_Code, PO_Details.POD_B_Item_Code, PO_Details.POD_Product_Desc, " & _
            '    " PO_Details.POD_UOM,PO_Details.POD_Po_Line,COMPANY_MSTR.CM_Coy_Name," & _
            '    " PO_Details.POD_Product_Code, PO_Mstr.POM_S_Coy_ID, PO_Details.POD_Delivered_Qty," & _
            '    " PO_Details.POD_Ordered_Qty, PO_Details.POD_Received_Qty,PO_Details.POD_Rejected_Qty, " & _
            '    " PO_Details.POD_ETD,PO_Details.POD_Min_Pack_Qty,PO_Details.POD_Min_Order_Qty,PO_Details.POD_Warranty_Terms," & _
            '    " DO_Mstr.*,DO_Details.*,0 as POD_Outstanding,0 as GD_REJECTED_QTY " & _
            '    " FROM PO_Mstr, PO_Details ,COMPANY_MSTR , DO_Mstr, DO_Details " & _
            '    " WHERE DO_Mstr.DOM_DO_Index  = '" & intDOIdx & "'" & _
            '    " And PO_Mstr.POM_PO_Index = " & IntPOIdx & _
            '    " And PO_Mstr.POM_B_Coy_ID  = '" & HttpContext.Current.Session("CompanyID") & "'" & _
            '    " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" & _
            '    " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" & _
            '    " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" & _
            '    " And PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" & _
            '    " And PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID" & _
            '    " And DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line" & _
            '    " And COMPANY_MSTR.CM_Coy_ID = DO_Mstr.DOM_S_Coy_ID ORDER BY POD_Po_Line"

            dsDODtl = objDb.FillDs(SQLQuery)
            Return dsDODtl
        End Function

        '//this Function also called during GRN ACK, so hv to pass in DO Index because Dropdownlist can only store 2 values

        Public Function GetGRNDetails(ByVal intDOIdx As Integer) As DataSet
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "select PO_Mstr.POM_PO_Index,POM_BUYER_NAME,PO_Mstr.POM_PO_NO,PO_Details.POD_VENDOR_ITEM_CODE,PO_Details.POD_PRODUCT_DESC," &
                        "PO_Details.POD_UOM,DOM_DO_NO,DOM_D_ADDR_LINE1,DOM_D_ADDR_LINE2,DOM_D_ADDR_LINE3,DOM_D_POSTCODE,DOM_D_CITY,DOM_D_STATE,DOM_D_COUNTRY,POD_MIN_PACK_QTY,PO_Details.POD_ORDERED_QTY,GM_GRN_LEVEL," &
                        "GM_PO_INDEX,GM_LEVEL2_USER,GM_GRN_INDEX,GM_GRN_NO,GM_DATE_RECEIVED,GM_CREATED_BY,GM_CREATED_DATE,UM_TEL_NO,UM_EMAIL,UM_USER_NAME AS GRN_Created_Name," &
                        "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,GRN_Mstr.GM_GRN_STATUS," &
                        "PO_Details.POD_Po_Line,PO_Details.POD_PR_INDEX,PO_Details.POD_B_ITEM_CODE,DOM_DO_INDEX,GM_GRN_LEVEL,0 as POD_Outstanding, " &
                        "DO_Details.DOD_SHIPPED_QTY,PO_Details.POD_DELIVERED_QTY,PO_Details.POD_CANCELLED_QTY,DO_DEtails.DOD_S_COY_ID,PO_Mstr.POM_B_Coy_ID,POM_S_Coy_ID " &
                        " from GRN_DEtails, GRN_Mstr, PO_Mstr, PO_Details,DO_Mstr,DO_Details,User_Mstr " &
                        " where GRN_Details.GD_GRN_NO = GRN_Mstr.GM_GRN_NO" &
                        " And GRN_Mstr.GM_B_COY_ID = GRN_Details.GD_B_COY_ID" &
                        " And GRN_Mstr.GM_PO_INDEX = PO_Mstr.POM_PO_INDEX" &
                        " And PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" &
                        " And PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID" &
                        " And GRN_DEtails.GD_PO_LINE = PO_Details.POD_Po_Line" &
                        " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                        " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" &
                        " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                        " And DO_Mstr.DOM_DO_INDEX = GRN_Mstr.GM_DO_INDEX" &
                        " And DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line" &
                        " And GRN_MSTR.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='" &
                        HttpContext.Current.Session("CompanyId") & "'" &
                        " And DO_Mstr.DOM_DO_INDEX = " & intDOIdx & " ORDER BY GD_PO_LINE"
            dsGRN = objDb.FillDs(SqlQuery)
            Return dsGRN
        End Function

        Public Function GetGRNVolPrice(ByVal CompId As String, ByVal ProdCode As String) As DataSet
            Dim ven_type, Item_Code, strSql As String
            Dim dsDiscAmt As DataSet

            'strSql = "SELECT CASE WHEN PV_VENDOR_TYPE='P' THEN '0' ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE FROM PIM_VENDOR " & _
            '        "WHERE PV_PRODUCT_INDEX = " & _
            '        "(SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(ItemCode) & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') " & _
            '        "AND PV_S_COY_ID = '" & Common.Parse(CompId) & "' ORDER BY PV_VENDOR_TYPE "

            strSql = "SELECT CASE WHEN PV_VENDOR_TYPE='P' THEN '0' ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE FROM PIM_VENDOR " &
                    "WHERE PV_PRODUCT_INDEX = " &
                    "(SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(ProdCode) & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') " &
                    "AND PV_S_COY_ID = '" & Common.Parse(CompId) & "' ORDER BY PV_VENDOR_TYPE "

            ven_type = objDb.GetVal(strSql)

            If ven_type = "0" Then
                ven_type = "P"
            End If

            'strSql = "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(ItemCode) & "' " & _
            '        "AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            'Item_Code = objDb.GetVal(strSql)

            'strSql = "SELECT PVP_VOLUME, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE " & _
            '        "WHERE PVP_PRODUCT_CODE = '" & Item_Code & "' AND PVP_VENDOR_TYPE = '" & ven_type & "'"

            strSql = "SELECT PVP_VOLUME, PVP_VOLUME_PRICE FROM PRODUCT_VOLUME_PRICE " &
                    "WHERE PVP_PRODUCT_CODE = '" & ProdCode & "' AND PVP_VENDOR_TYPE = '" & ven_type & "'"

            dsDiscAmt = objDb.FillDs(strSql)

            Return dsDiscAmt
        End Function

        Public Function GetGRNHistory(ByVal strGRNNo As String, ByVal strBCoyID As String) As DataSet
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "SELECT C.POM_PO_NO,C.POM_S_COY_ID,DATE_FORMAT(C.POM_PO_DATE,'%d/%m/%Y') AS POM_PO_DATE,C.POM_CURRENCY_CODE," &
                    "D.POD_ITEM_TYPE,D.POD_B_GL_CODE,D.POD_UNIT_COST,D.POD_SPEC1,D.POD_SPEC2,D.POD_SPEC3," &
                    "D.POD_VENDOR_ITEM_CODE,D.POD_PRODUCT_DESC,D.POD_UOM,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL, " &
                    "GM_PO_INDEX,GM_GRN_INDEX,GM_GRN_NO,GM_CREATED_DATE,GM_DATE_RECEIVED,GM_CREATED_BY,UM_USER_NAME AS GRN_Created_Name," &
                    "GD_OTH_CHARGE, GD_INLAND_CHARGE, GD_DUTIES, GD_EXCHANGE_RATE, GD_FACTOR,DD.DOD_DO_LOT_NO," &
                    "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS,D.POD_Po_Line,D.POD_B_ITEM_CODE," &
                    "GM_GRN_LEVEL,0 AS POD_Outstanding,E.DOM_DO_NO,DOD_SHIPPED_QTY   FROM GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D," &
                    "DO_Mstr E,User_Mstr,DO_DETAILS DD  WHERE A.GD_GRN_NO = B.GM_GRN_NO AND B.GM_B_COY_ID = A.GD_B_COY_ID " &
                    "AND B.GM_PO_INDEX = C.POM_PO_INDEX AND C.POM_PO_No = D.POD_Po_No AND C.POM_B_Coy_ID = D.POD_Coy_ID " &
                    "AND A.GD_PO_LINE = D.POD_Po_Line AND E.DOM_PO_Index = C.POM_PO_Index AND E.DOM_DO_INDEX = B.GM_DO_INDEX " &
                    "AND E.DOM_DO_NO = DD.DOD_DO_NO AND  E.DOM_S_COY_ID = DD.DOD_S_COY_ID AND  D.POD_PO_LINE = DD.DOD_PO_LINE " &
                    "AND B.GM_CREATED_BY = User_Mstr.UM_User_ID AND UM_Coy_ID='" & strBCoyID & "' AND B.GM_GRN_NO='" & strGRNNo & "' AND A.GD_B_COY_ID='" & strBCoyID & "' " &
                    "ORDER BY GD_PO_LINE "

            'SqlQuery = "select C.POM_PO_NO,D.POD_VENDOR_ITEM_CODE,D.POD_PRODUCT_DESC," & _
            '            "D.POD_UOM,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL," & _
            '            "GM_PO_INDEX,GM_GRN_INDEX,GM_GRN_NO,GM_CREATED_DATE,GM_DATE_RECEIVED,GM_CREATED_BY,UM_USER_NAME AS GRN_Created_Name," & _
            '            "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS," & _
            '            "D.POD_Po_Line,D.POD_B_ITEM_CODE,GM_GRN_LEVEL,0 as POD_Outstanding,E.DOM_DO_NO,DOD_SHIPPED_QTY  " & _
            '            " From GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D,DO_Mstr E,User_Mstr,DO_DETAILS DD " & _
            '            " where A.GD_GRN_NO = B.GM_GRN_NO" & _
            '            " And B.GM_B_COY_ID = A.GD_B_COY_ID" & _
            '            " And B.GM_PO_INDEX = C.POM_PO_INDEX" & _
            '            " And C.POM_PO_No = D.POD_Po_No" & _
            '            " And C.POM_B_Coy_ID = D.POD_Coy_ID" & _
            '            " And A.GD_PO_LINE = D.POD_Po_Line" & _
            '            " And E.DOM_PO_Index = C.POM_PO_Index" & _
            '            " And E.DOM_DO_INDEX = B.GM_DO_INDEX" & _
            '            " AND E.DOM_DO_NO = DD.DOD_DO_NO AND  E.DOM_S_COY_ID = DD.DOD_S_COY_ID AND  D.POD_PO_LINE = DD.DOD_PO_LINE" & _
            '            " And B.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='" & _
            '             strBCoyID & "'" & _
            '            " And B.GM_GRN_NO='" & strGRNNo & "' And A.GD_B_COY_ID='" & strBCoyID & "' ORDER BY GD_PO_LINE"
            dsGRN = objDb.FillDs(SqlQuery)
            Return dsGRN
        End Function

        Public Function GetLocInventory(ByVal strGRNNo As String, ByVal intPOLine As Integer) As DataSet
            Dim dsLoc As DataSet
            Dim SqlQuery As String

            'SqlQuery = "SELECT IT_TRANS_QTY, LM_LOCATION, IFNULL(LM_SUB_LOCATION, '-') AS LM_SUB_LOCATION FROM INVENTORY_TRANS IT, INVENTORY_MSTR IM, LOCATION_MSTR LM " & _
            '        "WHERE IT.IT_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " & _
            '        "AND IT.IT_TO_LOCATION_INDEX = LM.LM_LOCATION_INDEX " & _
            '        "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IT_TRANS_REF_NO = '" & strGRNNo & "' "

            SqlQuery = "SELECT DOL_LOT_NO, LM_LOCATION, IFNULL(LM_SUB_LOCATION, '-') AS LM_SUB_LOCATION, GL_LOT_RECEIVED_QTY " &
                    "FROM GRN_LOT, LOCATION_MSTR, DO_LOT WHERE GL_LOT_INDEX = DOL_LOT_INDEX AND GL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    "AND GL_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND GL_GRN_NO = '" & strGRNNo & "' AND GL_PO_LINE = " & intPOLine

            dsLoc = objDb.FillDs(SqlQuery)
            GetLocInventory = dsLoc
        End Function

        Public Function GetLocLot(ByVal strDONo As String, ByVal strItemCode As String, ByVal strSCoyID As String, ByVal strPoLine As String) As DataSet
            Dim dsLot As DataSet
            Dim SqlQuery As String

            SqlQuery = "SELECT DOL_LOT_NO, DOL_LOT_QTY, DOL_LOT_INDEX FROM DO_LOT WHERE DOL_DO_NO = '" & strDONo & "' " &
                    "AND DOL_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND DOL_COY_ID = '" & strSCoyID & "' " &
                    "AND DOL_PO_LINE = '" & strPoLine & "' "

            dsLot = objDb.FillDs(SqlQuery)
            GetLocLot = dsLot
        End Function
    End Class
End Namespace

