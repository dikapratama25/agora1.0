'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports System
Imports AgoraLegacy
Imports SSO.Component
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Web

Namespace AgoraLegacy

    Public Class Inventory_Ext
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals

        Public Function GetInvDetails(ByVal strItemCode As String, ByVal strItemName As String, ByVal strLoc As String, ByVal strSubLoc As String, ByVal strLot As String, Optional ByVal strSortBy As String = "", Optional ByVal chrQC As Char = Nothing) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            Dim strTemp As String
            SQLQuery = "SELECT ID_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, IL_LOT_INDEX, SUM(IL_LOT_QTY) AS IL_LOT_QTY, SUM(IFNULL(IL_IQC_QTY,0)) AS IL_IQC_QTY, ID_INVENTORY_QTY, DOL_LOT_NO, " _
                & "LM_LOCATION, LM_SUB_LOCATION, ID_LOCATION_INDEX, IM_IQC_IND FROM INVENTORY_DETAIL  " _
                & "INNER JOIN INVENTORY_MSTR ON ID_INVENTORY_INDEX=IM_INVENTORY_INDEX " _
                & "INNER JOIN INVENTORY_LOT ON ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IL_LOCATION_INDEX = ID_LOCATION_INDEX " _
                & "INNER JOIN DO_LOT ON  DOL_LOT_INDEX = IL_LOT_INDEX " _
                & "INNER JOIN LOCATION_MSTR ON ID_LOCATION_INDEX=LM_LOCATION_INDEX AND LM_COY_ID = IM_COY_ID " _
                & "WHERE IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                SQLQuery = SQLQuery & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strItemName <> "" Then
                strTemp = Common.BuildWildCard(strItemName)
                SQLQuery = SQLQuery & " AND IM_INVENTORY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strLoc <> "" And strLoc <> "---Select---" Then
                SQLQuery = SQLQuery & " AND LM_LOCATION ='" & Common.Parse(strLoc) & "'"
            End If

            'If strSubLoc <> "" And strSubLoc <> "---Select---" Then
            '    SQLQuery = SQLQuery & " AND LM_SUB_LOCATION ='" & Common.Parse(strSubLoc) & "'"
            'End If

            If strSubLoc <> "---Select---" Then
                If strSubLoc = "" Then
                    SQLQuery = SQLQuery & " AND LM_SUB_LOCATION IS NULL "
                Else
                    SQLQuery = SQLQuery & " AND LM_SUB_LOCATION ='" & Common.Parse(strSubLoc) & "'"
                End If

            End If

            If strLot <> "" And strLot <> "---Select---" Then
                SQLQuery = SQLQuery & " AND DOL_LOT_NO ='" & Common.Parse(strLot) & "'"
            End If

            If chrQC <> Nothing Then
                SQLQuery = SQLQuery & " AND IM_IQC_IND='" & chrQC & "'"
            End If

            SQLQuery = SQLQuery & " GROUP BY ID_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME, ID_LOCATION_INDEX, DOL_LOT_NO "

            If strSortBy <> "" Then
                SQLQuery = SQLQuery & " ORDER BY " & strSortBy
            End If


            'SQLQuery = SQLQuery & " Group By IL_INVENTORY_INDEX "

            'GROUP BY IL_INVENTORY_INDEX

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function PopLotNumber(ByVal strLoc As String, ByVal SubLocation As String) As DataSet
            Dim strSql As String
            Dim dsLotNumber As DataSet

            strSql = "SELECT DISTINCT DOL_LOT_NO, '' AS DOL_LOT_INDEX " &
                     "FROM INVENTORY_DETAIL " &
                     "INNER JOIN INVENTORY_MSTR ON ID_INVENTORY_INDEX=IM_INVENTORY_INDEX  " &
                     "INNER JOIN INVENTORY_LOT ON ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IL_LOCATION_INDEX = ID_LOCATION_INDEX " &
                     "INNER JOIN DO_LOT ON DOL_LOT_INDEX = IL_LOT_INDEX " &
                     "INNER JOIN LOCATION_MSTR ON ID_LOCATION_INDEX=LM_LOCATION_INDEX AND LM_COY_ID = IM_COY_ID " &
                     "WHERE LM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION='" & Common.Parse(strLoc) & "' "

            If SubLocation = "" Then
                strSql &= "AND LM_SUB_LOCATION IS NULL "
            Else
                strSql &= "AND LM_SUB_LOCATION ='" & Common.Parse(SubLocation) & "' "
            End If

            strSql &= "ORDER BY DOL_LOT_NO "

            dsLotNumber = objDb.FillDs(strSql)
            PopLotNumber = dsLotNumber

        End Function

        Public Function GetCostDetails(ByVal strItemCode As String, ByVal strItemName As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strInvType As String) As DataSet
            Dim SQLQuery As String
            Dim dsCos As DataSet
            Dim strTemp As String

            SQLQuery = "SELECT IC_INVENTORY_TYPE, IC_COST_DATE, IM_ITEM_CODE, IM_INVENTORY_NAME, IC_COST_OPEN_QTY, " &
                    "IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_INVENTORY_TYPE, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " &
                    "THEN IC_COST_QTY ELSE 0 END AS RECEIVED_QTY, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " &
                    "THEN IC_COST_UPRICE ELSE 0 END AS RECEIVED_UPRICE, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'GRN' OR IC_INVENTORY_TYPE= 'IIC' OR IC_INVENTORY_TYPE = 'RI' OR IC_INVENTORY_TYPE = 'WOC') " &
                    "THEN IC_COST_COST ELSE 0 END AS RECEIVED_COST, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " &
                    "THEN IC_COST_QTY ELSE 0  END AS ISSUED_QTY, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " &
                    "THEN IC_COST_UPRICE ELSE 0  END AS ISSUED_UPRICE, " &
                    "CASE WHEN (IC_INVENTORY_TYPE = 'II' OR IC_INVENTORY_TYPE= 'RO' OR IC_INVENTORY_TYPE = 'WO') " &
                    "THEN IC_COST_COST ELSE 0  END AS ISSUED_COST FROM inventory_cost " &
                    "INNER JOIN inventory_mstr ON IC_inventory_index = IM_INVENTORY_INDEX " &
                    "WHERE IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                SQLQuery = SQLQuery & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strItemName <> "" Then
                strTemp = Common.BuildWildCard(strItemName)
                SQLQuery = SQLQuery & " AND IM_INVENTORY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                SQLQuery = SQLQuery & " AND IC_COST_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                SQLQuery = SQLQuery & " AND IC_COST_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            If strInvType <> "" Then
                SQLQuery = SQLQuery & " AND IC_INVENTORY_TYPE='" & strInvType & "' "

            End If

            dsCos = objDb.FillDs(SQLQuery)
            Return dsCos

        End Function

        Public Function GetInvTransHistory(ByVal intInvIndex As Integer, ByVal intLocIndex As Integer, ByVal strLotNo As String) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet

            SQLQuery = "SELECT * FROM ("

            'MRS or MRS Cancelled
            SQLQuery &= "SELECT IT_TRANS_INDEX, IT_TRANS_DATE, IT_TRANS_TYPE, CODE_DESC, IT_TRANS_USER_ID, " & _
                        "UM_USER_NAME, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, " & _
                        "CASE WHEN IT_TRANS_TYPE = 'MRSC' THEN IRSL_LOT_QTY ELSE (-IRSL_LOT_QTY) END AS TRANS_QTY " & _
                        "FROM INVENTORY_TRANS " & _
                        "INNER JOIN INVENTORY_MSTR ON IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN CODE_MSTR ON IT_TRANS_TYPE = CODE_ABBR AND CODE_CATEGORY = 'TT' " & _
                        "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IT_INVENTORY_INDEX = IRSD_INVENTORY_INDEX AND IM_COY_ID = IRSD_IRS_COY_ID AND IT_TRANS_REF_NO = IRSD_IRS_NO " & _
                        "INNER JOIN INVENTORY_REQUISITION_SLIP_LOT ON IRSD_IRS_LINE = IRSL_IRS_LINE AND IRSD_IRS_NO = IRSL_IRS_NO AND IRSD_IRS_COY_ID = IRSL_IRS_COY_ID " & _
                        "AND IT_FRM_LOCATION_INDEX = IRSL_LOCATION_INDEX " & _
                        "INNER JOIN DO_LOT ON IRSL_LOT_INDEX = DOL_LOT_INDEX " & _
                        "LEFT JOIN USER_MSTR ON IT_TRANS_USER_ID = UM_USER_ID AND IM_COY_ID = UM_COY_ID " & _
                        "WHERE IT_INVENTORY_INDEX = " & intInvIndex & " AND IT_FRM_LOCATION_INDEX = " & intLocIndex & " AND (IT_TRANS_TYPE = 'MRS' OR IT_TRANS_TYPE = 'MRSC') " & _
                        "AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                        "UNION "

            'Return Outward
            SQLQuery &= "SELECT IT_TRANS_INDEX, IT_TRANS_DATE, IT_TRANS_TYPE, CODE_DESC, IT_TRANS_USER_ID, " & _
                        "UM_USER_NAME, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, (-IROL_LOT_QTY) AS TRANS_QTY " & _
                        "FROM INVENTORY_TRANS " & _
                        "INNER JOIN INVENTORY_MSTR ON IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN CODE_MSTR ON IT_TRANS_TYPE = CODE_ABBR AND CODE_CATEGORY = 'TT' " & _
                        "INNER JOIN INVENTORY_RETURN_OUTWARD_DETAILS ON IT_INVENTORY_INDEX = IROD_INVENTORY_INDEX AND IM_COY_ID = IROD_RO_COY_ID AND IT_TRANS_REF_NO = IROD_RO_NO " & _
                        "INNER JOIN INVENTORY_RETURN_OUTWARD_LOT ON IROD_RO_LINE = IROL_RO_LINE AND IROD_RO_NO = IROL_RO_NO AND IROD_RO_COY_ID = IROL_RO_COY_ID " & _
                        "AND IT_FRM_LOCATION_INDEX = IROL_LOCATION_INDEX " & _
                        "INNER JOIN DO_LOT ON IROL_LOT_INDEX = DOL_LOT_INDEX " & _
                        "LEFT JOIN USER_MSTR ON IT_TRANS_USER_ID = UM_USER_ID AND IM_COY_ID = UM_COY_ID " & _
                        "WHERE IT_INVENTORY_INDEX = " & intInvIndex & " AND IT_FRM_LOCATION_INDEX = " & intLocIndex & " AND IT_TRANS_TYPE = 'RO' " & _
                        "AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                        "UNION "

            'GRN
            SQLQuery &= "SELECT IT_TRANS_INDEX, IT_TRANS_DATE, IT_TRANS_TYPE, CODE_DESC, IT_TRANS_USER_ID, " & _
                        "UM_USER_NAME, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, GL_LOT_RECEIVED_QTY AS TRANS_QTY " & _
                        "FROM INVENTORY_TRANS " & _
                        "INNER JOIN INVENTORY_MSTR ON IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN CODE_MSTR ON IT_TRANS_TYPE = CODE_ABBR AND CODE_CATEGORY = 'TT' " & _
                        "INNER JOIN GRN_LOT ON IT_TRANS_REF_NO = GL_GRN_NO AND IM_COY_ID = GL_B_COY_ID " & _
                        "AND IT_TRANS_REF_NO = GL_GRN_NO AND IT_TO_LOCATION_INDEX = GL_LOCATION_INDEX " & _
                        "INNER JOIN DO_LOT ON GL_LOT_INDEX = DOL_LOT_INDEX " & _
                        "LEFT JOIN USER_MSTR ON IT_TRANS_USER_ID = UM_USER_ID AND IM_COY_ID = UM_COY_ID " & _
                        "WHERE IT_INVENTORY_INDEX = " & intInvIndex & " And IT_TO_LOCATION_INDEX = " & intLocIndex & " " & _
                        "AND IT_TRANS_TYPE = 'II' AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                        "UNION "

            'Return Inward
            SQLQuery &= "SELECT IT_TRANS_INDEX, IT_TRANS_DATE, IT_TRANS_TYPE, CODE_DESC, IT_TRANS_USER_ID, " & _
                        "UM_USER_NAME, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, IRIL_LOT_QTY AS TRANS_QTY " & _
                        "FROM INVENTORY_TRANS " & _
                        "INNER JOIN INVENTORY_MSTR ON IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN CODE_MSTR ON IT_TRANS_TYPE = CODE_ABBR AND CODE_CATEGORY = 'TT' " & _
                        "INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IT_INVENTORY_INDEX = IRID_INVENTORY_INDEX AND IM_COY_ID = IRID_RI_COY_ID AND IT_TRANS_REF_NO = IRID_RI_NO " & _
                        "INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRID_RI_LINE = IRIL_RI_LINE AND IRID_RI_NO = IRIL_RI_NO AND IRID_RI_COY_ID = IRIL_RI_COY_ID " & _
                        "AND IT_TO_LOCATION_INDEX = IRIL_LOCATION_INDEX " & _
                        "INNER JOIN DO_LOT ON IRIL_LOT_INDEX = DOL_LOT_INDEX " & _
                        "LEFT JOIN USER_MSTR ON IT_TRANS_USER_ID = UM_USER_ID AND IM_COY_ID = UM_COY_ID " & _
                        "WHERE IT_INVENTORY_INDEX = " & intInvIndex & " AND IT_TO_LOCATION_INDEX = " & intLocIndex & " AND IT_TRANS_TYPE = 'RI' " & _
                        "AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                        "UNION "

            'Write Off
            SQLQuery &= "SELECT IT_TRANS_INDEX, IT_TRANS_DATE, IT_TRANS_TYPE, CODE_DESC, IT_TRANS_USER_ID, " & _
                        "UM_USER_NAME, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, " & _
                        "CASE WHEN IT_TRANS_TYPE = 'WOC' THEN IWOD_QTY_VAL ELSE (-IWOD_QTY_VAL) END AS TRANS_QTY " & _
                        "FROM INVENTORY_TRANS " & _
                        "INNER JOIN INVENTORY_MSTR ON IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
                        "INNER JOIN CODE_MSTR ON IT_TRANS_TYPE = CODE_ABBR AND CODE_CATEGORY = 'TT' " & _
                        "INNER JOIN INVENTORY_WRITE_OFF_DETAILS ON IT_INVENTORY_INDEX = IWOD_INVENTORY_INDEX AND IM_COY_ID = IWOD_WO_COY_ID AND IT_TRANS_REF_NO = IWOD_WO_NO " & _
                        "AND IT_FRM_LOCATION_INDEX = IWOD_FRM_LOCATION_INDEX AND IWOD_WO_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                        "LEFT JOIN USER_MSTR ON IT_TRANS_USER_ID = UM_USER_ID AND IM_COY_ID = UM_COY_ID " & _
                        "WHERE IT_INVENTORY_INDEX = " & intInvIndex & " AND IT_FRM_LOCATION_INDEX = " & intLocIndex & " AND (IT_TRANS_TYPE = 'WO' OR IT_TRANS_TYPE = 'WOC') "

            SQLQuery &= ") tb ORDER BY IT_TRANS_DATE "
            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function
    End Class
End Namespace
