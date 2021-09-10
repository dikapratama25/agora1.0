Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO

Namespace AgoraLegacy

    Public Class POValue_Ext

        Public bill_adds As String
        Public PO_Number As String
        Public PO_type As String
        Public PO_Status As String
        Public PO_Date As String
        Public PO_Line As String
        Public vendor_CoyId As String
        Public vendor_Coy As String
        Public vendor_adds As String
        Public vendor_tel As String
        Public vendor_fax As String
        Public vendor_contact As String
        Public vendor_email As String
        Public vendor_Item_Code As String
        Public Product_Desc As String
        Public Order_Qty As String
        Public Rec_Qty As String
        Public Rej_Qty As String
        Public Unit_Cost As String
        Public remarks As String
        Public buyer_adds As String
        Public cur As String
        Public buyer_tel As String
        Public buyer_fax As String
        Public buyer_contact As String
        Public buyer_email As String
        Public pay_term As String
        Public ship_term As String
        Public del_term As String
        Public vendor_code As String
        Public gst_code As String
        Public ship_via As String
        Public pay_meth As String
        Public ship_meth As String
        Public ex_rate As String
        Public tax As String
        Public CR_NO As String
        Public CR_ReqDate As String
        Public CR_ReqBy As String
        Public buyer_coy As String
        Public BCoyName As String
        Public TermAndCond As String
        Public FreightTerm As String
        Public Vendor_remark As String
        Public lineval As String
        Public linevalue(0) As String
        Public POIndex As Integer
        Public POM_CREATED_DATE As String
        Public urgent As String
        Public POM_ACCEPTED_DATE As String
        Public intremarks As String

    End Class
    Public Class PurchaseOrder_Ext
        Dim objDb As New EAD.DBCom
        Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
        Dim com_id As String = HttpContext.Current.Session("CompanyId")
        Dim userid As String = HttpContext.Current.Session("UserId")

        Public Function get_PODetail(ByVal item As POValue_Ext, ByVal side As String, ByVal blnPreview As Boolean)

            Dim objGlobal As New AppGlobals
            Dim strsql As String

            If Not blnPreview Then
                If item.CR_NO <> "" Then
                    strsql = "Select PO_MSTR.*,PO_CR_MSTR.*,COMPANY_MSTR.*,UMA.UM_EMAIL,UMB.UM_USER_NAME,(SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY From PO_MSTR INNER JOIN PO_CR_MSTR on PCM_PO_INDEX =POM_PO_INDEX " & _
                    " INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR UMA ON UMA.UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UMA.UM_COY_ID " & _
                    " Inner Join USER_MSTR UMB ON UMB.UM_USER_ID=PCM_REQ_BY AND PCM_B_COY_ID=UMB.UM_COY_ID Where " & _
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                Else
                    strsql = "Select PO_MSTR.*,COMPANY_MSTR.*,UM_EMAIL,(SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY from PO_MSTR INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID Where " & _
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                End If
            Else
                strsql = "Select PO_MSTR_PREVIEW.*,COMPANY_MSTR.*,UM_EMAIL,(SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY from PO_MSTR_PREVIEW INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID Where " & _
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
            End If

            Dim strTempAddr As String

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strTempAddr = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim
                End If
                If tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim)
                End If

                item.buyer_adds = strTempAddr

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim
                End If
                If tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim)
                End If

                item.bill_adds = strTempAddr

                item.buyer_contact = tDS.Tables(0).Rows(0).Item("POM_BUYER_NAME").ToString.Trim
                item.buyer_fax = tDS.Tables(0).Rows(0).Item("POM_BUYER_FAX").ToString.Trim
                item.buyer_tel = tDS.Tables(0).Rows(0).Item("POM_BUYER_PHONE").ToString.Trim
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                item.ex_rate = tDS.Tables(0).Rows(0).Item("POM_EXCHANGE_RATE").ToString.Trim
                item.BCoyName = Common.parseNull(tDS.Tables(0).Rows(0).Item("BCOY"))
                item.POIndex = tDS.Tables(0).Rows(0).Item("POM_PO_INDEX")
                item.pay_meth = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_METHOD").ToString.Trim
                item.pay_term = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_TERM").ToString.Trim
                item.del_term = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE"))
                item.vendor_code = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_VENDOR_CODE"))
                item.PO_Date = tDS.Tables(0).Rows(0).Item("POM_PO_DATE").ToString.Trim
                item.POM_ACCEPTED_DATE = tDS.Tables(0).Rows(0).Item("POM_ACCEPTED_DATE").ToString.Trim
                If Not blnPreview Then
                    item.POM_CREATED_DATE = tDS.Tables(0).Rows(0).Item("POM_CREATED_DATE").ToString.Trim
                End If
                item.PO_Status = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS").ToString.Trim
                item.ship_meth = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_MODE").ToString.Trim
                item.ship_term = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                item.ship_via = tDS.Tables(0).Rows(0).Item("POM_SHIP_VIA").ToString.Trim

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim)
                End If
                item.vendor_adds = strTempAddr
                item.vendor_contact = tDS.Tables(0).Rows(0).Item("POM_S_ATTN").ToString.Trim
                item.vendor_CoyId = tDS.Tables(0).Rows(0).Item("POM_S_COY_ID").ToString.Trim
                item.vendor_Coy = tDS.Tables(0).Rows(0).Item("POM_S_COY_NAME").ToString.Trim
                item.vendor_email = tDS.Tables(0).Rows(0).Item("POM_S_EMAIL").ToString.Trim

                item.vendor_fax = tDS.Tables(0).Rows(0).Item("POM_S_FAX").ToString.Trim
                item.vendor_tel = tDS.Tables(0).Rows(0).Item("POM_S_PHONE").ToString.Trim
                item.Vendor_remark = tDS.Tables(0).Rows(0).Item("POM_S_REMARK").ToString.Trim
                item.TermAndCond = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_TERMANDCOND").ToString.Trim)
                If item.CR_NO <> "" Then
                    item.CR_NO = tDS.Tables(0).Rows(0).Item("PCM_CR_NO").ToString.Trim
                    item.CR_ReqDate = tDS.Tables(0).Rows(0).Item("PCM_REQ_DATE").ToString.Trim
                    item.CR_ReqBy = tDS.Tables(0).Rows(0).Item("UM_USER_NAME").ToString.Trim
                    item.remarks = tDS.Tables(0).Rows(0).Item("PCM_CR_REMARKS").ToString.Trim
                Else
                    item.remarks = tDS.Tables(0).Rows(0).Item("POM_EXTERNAL_REMARK").ToString.Trim
                    item.intremarks = tDS.Tables(0).Rows(0).Item("POM_INTERNAL_REMARK").ToString.Trim
                End If

                item.tax = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_TAX_REG_NO"))
                item.buyer_email = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_EMAIL"))
                '//hardcode for temporary
                item.PO_type = "Regular"
                item.urgent = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_URGENT"))
            End If

            objGlobal = Nothing

        End Function

        Public Function get_POQtyByMonth(ByVal intMonth As Integer, ByVal intYear As Integer, ByVal side As String, ByVal strBCoyId As String) As Double
            Dim strsql As String

            strsql = "SELECT IFNULL(SUM((POD.POD_ORDERED_QTY - POD.POD_REJECTED_QTY)),0) AS QTY FROM PO_Details POD, PO_MSTR POM " & _
                    "WHERE(POM.POM_PO_NO = POD.POD_PO_NO And POM.POM_B_COY_ID = POD.POD_COY_ID) " & _
                    "AND POM.POM_PO_STATUS = 3 " & _
                    "AND MONTH(POM_ACCEPTED_DATE) = " & intMonth & " AND YEAR(POM_ACCEPTED_DATE) = " & intYear & " "

            If side = "v" Then
                strsql = strsql & "AND POD.POD_COY_ID = '" & Common.Parse(strBCoyId) & "' "
            Else
                strsql = strsql & "AND POD.POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            End If

            get_POQtyByMonth = objDb.GetVal(strsql)

        End Function

        ' Yap 21March2013
        Public Function get_StkOnHandByItem(ByVal strItemCode As String) As Double
            Dim strsql As String

            'strsql = "SELECT IFNULL(SUM(IC_COST_CLOSE_QTY),0.00) AS IC_COST_CLOSE_QTY FROM INVENTORY_COST " & _
            '        "WHERE IC_INVENTORY_NAME = '" & Common.Parse(strItemCode) & "' " & _
            '        "AND IC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ORDER BY IC_COST_DATE DESC LIMIT 1 "

            strsql = "SELECT IFNULL(IC_COST_CLOSE_QTY,0.00) AS IC_COST_CLOSE_QTY " & _
                    "FROM INVENTORY_COST INNER JOIN INVENTORY_MSTR ON IM_COY_ID = IC_COY_ID AND IM_INVENTORY_INDEX = IC_INVENTORY_INDEX " & _
                    "INNER JOIN PRODUCT_MSTR ON PM_S_COY_ID = IM_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND PM_PRODUCT_CODE = '" & Common.Parse(strItemCode) & "' " & _
                    "WHERE IC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ORDER BY IC_COST_DATE DESC LIMIT 1 "

            get_StkOnHandByItem = CDbl(IIf(objDb.GetVal(strsql) = "", 0.0, objDb.GetVal(strsql)))

        End Function

        ' Yap 21March2013
        Public Function get_POBalanceByItem(ByVal strItemCode As String) As Double
            Dim strsql As String

            strsql = "SELECT IFNULL(SUM(POD.POD_ORDERED_QTY) - (SUM(POD.POD_RECEIVED_QTY) - SUM(POD.POD_REJECTED_QTY)),0.00) AS PO_BAL FROM PO_DETAILS POD, PO_MSTR POM " & _
                    "WHERE(POD.POD_COY_ID = POM.POM_B_COY_ID And POD.POD_PO_NO = POM.POM_PO_NO) " & _
                    "AND POM.POM_PO_STATUS = 3 AND POD.POD_PRODUCT_CODE = '" & Common.Parse(strItemCode) & "' " & _
                    "AND POM.POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            get_POBalanceByItem = CDbl(IIf(objDb.GetVal(strsql) = "", 0.0, objDb.GetVal(strsql)))

        End Function

        ' Yap 21March2013
        Public Function get_POInProgressByItem(ByVal strItemCode As String) As Double
            Dim strsql As String

            strsql = "SELECT IFNULL(SUM(POD_ORDERED_QTY),0.00) AS POD_ORDERED_QTY FROM PO_DETAILS POD, PO_MSTR POM " & _
                    "WHERE(POD.POD_COY_ID = POM.POM_B_COY_ID And POD.POD_PO_NO = POM.POM_PO_NO) " & _
                    "AND (POM.POM_PO_STATUS = 7 OR POM.POM_PO_STATUS = 8) " & _
                    "AND POD.POD_PRODUCT_CODE = '" & Common.Parse(strItemCode) & "' " & _
                    "AND POM.POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            get_POInProgressByItem = CDbl(IIf(objDb.GetVal(strsql) = "", 0.0, objDb.GetVal(strsql)))

        End Function

        Public Function get_month(ByVal intMonth As Integer) As String
            Dim strMonth As String

            If intMonth = 1 Then
                strMonth = "Jan"
            ElseIf intMonth = 2 Then
                strMonth = "Feb"
            ElseIf intMonth = 3 Then
                strMonth = "Mar"
            ElseIf intMonth = 4 Then
                strMonth = "Apr"
            ElseIf intMonth = 5 Then
                strMonth = "May"
            ElseIf intMonth = 6 Then
                strMonth = "Jun"
            ElseIf intMonth = 7 Then
                strMonth = "Jul"
            ElseIf intMonth = 8 Then
                strMonth = "Aug"
            ElseIf intMonth = 9 Then
                strMonth = "Sep"
            ElseIf intMonth = 10 Then
                strMonth = "Oct"
            ElseIf intMonth = 11 Then
                strMonth = "Nov"
            ElseIf intMonth = 12 Then
                strMonth = "Dec"
            Else
                strMonth = ""
            End If

            Return strMonth

        End Function

        Public Function chkPOItemLine(ByVal PO_No As String, ByVal side As String, ByVal strBCoyId As String) As Boolean
            Dim strSql As String = ""

            strSql = " SELECT '*' " & _
                    "FROM PO_DETAILS,PO_MSTR WHERE POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO= '" & Common.Parse(PO_No) & "' " & _
                    "AND POM_B_COY_ID= '" & Common.Parse(strBCoyId) & "' AND POD_ITEM_TYPE = 'ST' "

            If objDb.Exist(strSql) Then
                chkPOItemLine = False
            Else
                chkPOItemLine = True
            End If

        End Function

        Public Function get_POPrev3MonthUsage(ByVal intMonth As Integer, ByVal intYear As Integer, ByVal strProdCode As String) As Double
            Dim strsql As String

            strsql = "SELECT IFNULL(SUM(IRSD_QTY), 0.00) AS QTY FROM INVENTORY_REQUISITION_SLIP_MSTR " & _
                    "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS " & _
                    "WHERE IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO AND " & _
                    "IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                    "IRSD_INVENTORY_NAME = '" & strProdCode & "' AND " & _
                    "(IRSM_IRS_STATUS = '3' OR " & _
                    "IRSM_IRS_STATUS = '4') AND " & _
                    "MONTH(IRSM_IRS_DATE) = " & intMonth & " AND YEAR(IRSM_IRS_DATE) = " & intYear & " "

            get_POPrev3MonthUsage = objDb.GetVal(strsql)

        End Function

        Public Function GetPOListBalance(ByVal strItemCode As String) As DataSet
            Dim SQLQuery As String
            Dim dsPO As DataSet

            SQLQuery = "SELECT * FROM PO_DETAILS POD, PO_MSTR POM " & _
                    "WHERE(POD.POD_COY_ID = POM.POM_B_COY_ID And POD.POD_PO_NO = POM.POM_PO_NO) " & _
                    "AND POM.POM_PO_STATUS = 3 AND POD.POD_PRODUCT_CODE = '" & Common.Parse(strItemCode) & "' " & _
                    "AND POM.POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "HAVING (IFNULL(((POD.POD_ORDERED_QTY - (POD.POD_RECEIVED_QTY - POD.POD_REJECTED_QTY))),0.00)) <> 0"

            dsPO = objDb.FillDs(SQLQuery)

            Return dsPO

        End Function

        Public Function GetPOListProgress(ByVal strItemCode As String) As DataSet
            Dim SQLQuery As String
            Dim dsPO As DataSet

            SQLQuery = "SELECT * FROM PO_DETAILS POD, PO_MSTR POM " & _
                    "WHERE(POD.POD_COY_ID = POM.POM_B_COY_ID And POD.POD_PO_NO = POM.POM_PO_NO) " & _
                    "AND (POM.POM_PO_STATUS = 7 OR POM.POM_PO_STATUS = 8) " & _
                    "AND POD.POD_PRODUCT_CODE = '" & Common.Parse(strItemCode) & "' " & _
                    "AND POM.POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "HAVING (IFNULL((POD_ORDERED_QTY),0.00)) <> 0 "

            dsPO = objDb.FillDs(SQLQuery)

            Return dsPO

        End Function
    End Class
End Namespace
