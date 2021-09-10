Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class Dashboard
        Dim objDb As New EAD.DBCom


        Public Function GetDOStatus(ByVal DOM_DO_STATUS As Integer, ByVal POM_PO_INDEX As Integer) As DataSet
            Dim dsDO As DataSet
            Dim strsqlDO As String
            strsqlDO = "SELECT ISNULL(DOM_DO_NO,'') AS DOM_DO_NO, ISNULL(DOM_DO_Index,'') AS DOM_DO_Index, ISNULL(DOM_D_ADDR_CODE,'') AS DOM_D_ADDR_CODE FROM DO_MSTR where DOM_DO_STATUS = '" & DOM_DO_STATUS & "' and DOM_PO_INDEX = " & POM_PO_INDEX
            dsDO = objDb.FillDs(strsqlDO)
            Return dsDO
        End Function

        Public Function GetDOStatusWithDA(ByVal DOM_DO_STATUS As Integer, ByVal POM_PO_INDEX As Integer, ByVal POM_DA As String) As DataSet
            Dim dsDO As DataSet
            Dim strsqlDO As String
            strsqlDO = "SELECT ISNULL(DOM_DO_NO,'') AS DOM_DO_NO, ISNULL(DOM_DO_Index,'') AS DOM_DO_Index, ISNULL(DOM_D_ADDR_CODE,'') AS DOM_D_ADDR_CODE FROM DO_MSTR where DOM_DO_STATUS = '" & DOM_DO_STATUS & "' and DOM_PO_INDEX = " & POM_PO_INDEX & " and DOM_D_ADDR_CODE = '" & POM_DA & "' "
            dsDO = objDb.FillDs(strsqlDO)
            Return dsDO
        End Function

        Public Function GetOutstandingPO() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Michelle (27/04/2011) - To include those PO that are due but not yet accepted & change the checking of due date based
            '                        on the po creation date instead of po date
            'strsql = " SELECT POM_PO_No AS 'PO Number', POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME AS 'Buyer Company', POM_PO_INDEX, " & _
            '   " POM_B_COY_ID, (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' " & _
            '   " AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " & _
            '   " SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty', " & _
            '   " DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS, PCM_CR_NO AS CR_NO, POM_FULFILMENT " & _
            '   " FROM PO_MSTR PM INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " & _
            '   " INNER JOIN COMPANY_MSTR ON POM_B_COY_ID = CM_COY_ID " & _
            '   " LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM_PO_INDEX AND PCM_B_COY_ID = POM_B_COY_ID " & _
            '   " WHERE PM.POM_S_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND (PM.POM_PO_STATUS IN ('1','2','3') AND EXISTS " & _
            '   " (SELECT '*' FROM PO_Details WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " & _
            '   " AND DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) >= GETDATE() GROUP BY POD_Coy_ID,POD_PO_NO " & _
            '   " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) OR (PM.POM_PO_STATUS = '5' AND PM.POM_FULFILMENT = '4') OR (PM.POM_PO_STATUS = '3' AND PM.POM_FULFILMENT = '4'))" & _
            '   " GROUP BY POM_PO_No ORDER BY POM_PO_Date DESC, POM_PO_Index DESC, POD_ETD "
            strsql = " SELECT POM_PO_No AS 'PO Number', POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME AS 'Buyer Company', POM_PO_INDEX, " &
                 " POM_B_COY_ID, (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' " &
                 " AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " &
                 " SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty', " &
                 " DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS, PCM_CR_NO AS CR_NO, POM_FULFILMENT " &
                 " FROM PO_MSTR PM INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " &
                 " INNER JOIN COMPANY_MSTR ON POM_B_COY_ID = CM_COY_ID " &
                 " LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM_PO_INDEX AND PCM_B_COY_ID = POM_B_COY_ID " &
                 " WHERE PM.POM_S_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND (PM.POM_PO_STATUS IN ('1','2','3') AND EXISTS " &
                 " (SELECT '*' FROM PO_Details WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " &
                 " AND (PM.POM_PO_STATUS IN ('1','2','5') OR DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) >= CURRENT_DATE()) " &
                 " GROUP BY POD_Coy_ID,POD_PO_NO " &
                 " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) OR (PM.POM_PO_STATUS = '5' AND PM.POM_FULFILMENT = '4') OR (PM.POM_PO_STATUS = '3' AND PM.POM_FULFILMENT = '4'))" &
                 " GROUP BY POM_PO_No ORDER BY POM_PO_Date DESC, POM_PO_Index DESC, POD_ETD "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetOutstandingPODash() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Michelle (27/04/2011) - To include those PO that are due but not yet accepted & change the checking of due date based
            '                        on the po creation date instead of po date
            'Michelle (21/10/2011) - Issue
            '       strsql = " SELECT PM.POM_S_COY_ID, POM_RFQ_INDEX, POM_PO_No AS 'PO Number', POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME AS 'Buyer Company', POM_PO_INDEX, " & _
            '" POM_B_COY_ID, (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' " & _
            '" AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " & _
            '" SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty', " & _
            '" DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS, PCM_CR_NO AS CR_NO, POM_FULFILMENT " & _
            '" FROM PO_MSTR PM INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " & _
            '" INNER JOIN COMPANY_MSTR ON POM_B_COY_ID = CM_COY_ID " & _
            '" LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM_PO_INDEX AND PCM_B_COY_ID = POM_B_COY_ID " & _
            '" WHERE PM.POM_S_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND (PM.POM_PO_STATUS IN ('1','2','3') AND EXISTS " & _
            '" (SELECT '*' FROM PO_Details WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " & _
            '" AND (PM.POM_PO_STATUS IN ('1','2','5') OR DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) >= CURRENT_DATE()) " & _
            '" GROUP BY POD_Coy_ID,POD_PO_NO " & _
            '" HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) OR (PM.POM_PO_STATUS = '5' AND PM.POM_FULFILMENT = '4') OR (PM.POM_PO_STATUS = '3' AND PM.POM_FULFILMENT = '4'))" & _
            '" GROUP BY POM_PO_No, POM_B_COY_ID ORDER BY POM_PO_Date DESC, POM_PO_Index DESC, POD_ETD "

            strsql = " SELECT PM.POM_S_COY_ID, POM_RFQ_INDEX, POM_PO_No AS 'PO Number', POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME AS 'Buyer Company', POM_PO_INDEX, " &
                 " POM_B_COY_ID, (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' " &
                 " AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " &
                 " (SELECT SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' " &
                 " AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Overdue Qty', " &
                 " DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS, PCM_CR_NO AS CR_NO, POM_FULFILMENT, POM_URGENT " &
                 " FROM PO_MSTR PM INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " &
                 " INNER JOIN COMPANY_MSTR ON POM_B_COY_ID = CM_COY_ID " &
                 " LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM_PO_INDEX AND PCM_B_COY_ID = POM_B_COY_ID " &
                 " WHERE PM.POM_S_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND (PM.POM_PO_STATUS IN ('1','2','3') AND EXISTS " &
                 " (SELECT '*' FROM PO_Details WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " &
                 " AND (PM.POM_PO_STATUS IN ('1','2','5') OR DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) >= CURRENT_DATE()) " &
                 " GROUP BY POD_Coy_ID,POD_PO_NO " &
                 " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) OR (PM.POM_PO_STATUS = '5' AND PM.POM_FULFILMENT = '4') OR (PM.POM_PO_STATUS = '3' AND PM.POM_FULFILMENT = '4'))" &
                 " GROUP BY POM_PO_No, POM_B_COY_ID ORDER BY POM_PO_Date DESC, POM_PO_Index DESC, POD_ETD "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetOverduePO() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SELECT POM_PO_No, POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME" &
                        " AS 'Buyer Company', POM_PO_INDEX,  POM_B_COY_ID, " &
                        " (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS" &
                        " WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " &
                        " SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty'," &
                        " DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS " &
                        " FROM PO_MSTR PM, PO_DETAILS, COMPANY_MSTR " &
                        " WHERE POM_B_COY_ID = CM_COY_ID AND PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND  POD_COY_ID = POM_B_COY_ID " &
                        " AND POD_PO_NO = POM_PO_NO AND PM.POM_PO_STATUS IN ('3') AND EXISTS " &
                        " (SELECT '*' FROM PO_Details " &
                        " WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " &
                        " AND  DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) < GETDATE()" &
                        " GROUP BY POD_Coy_ID,POD_PO_NO " &
                        " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) " &
                        " GROUP BY POM_PO_No" &
                        " ORDER BY POM_PO_Date DESC, POM_PO_Index DESC "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetOverduePODash() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'strsql = "SELECT POD_D_ADDR_CODE, PM.POM_S_COY_ID, POM_RFQ_INDEX, POM_PO_No, POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME" & _
            '            " AS 'Buyer Company', POM_PO_INDEX,  POM_B_COY_ID, " & _
            '            " (SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM PO_DETAILS" & _
            '            " WHERE PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID) AS 'Ordered Qty', " & _
            '            " SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty'," & _
            '            " DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS " & _
            '            " FROM PO_MSTR PM, PO_DETAILS, COMPANY_MSTR " & _
            '            " WHERE POM_B_COY_ID = CM_COY_ID AND PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND  POD_COY_ID = POM_B_COY_ID " & _
            '            " AND POD_PO_NO = POM_PO_NO AND PM.POM_PO_STATUS IN ('3') AND EXISTS " & _
            '            " (SELECT '*' FROM PO_Details " & _
            '            " WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " & _
            '            " AND  DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) < GETDATE()" & _
            '            " GROUP BY POD_Coy_ID,POD_PO_NO " & _
            '            " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) " & _
            '            " GROUP BY POM_PO_No, POD_D_ADDR_CODE" & _
            '            " ORDER BY POM_PO_Date DESC, POM_PO_Index DESC "

            strsql = "SELECT POD_D_ADDR_CODE, PM.POM_S_COY_ID, POM_RFQ_INDEX, POM_PO_No, POM_PO_Index,POM_PO_Date AS 'PO Date', CM_COY_NAME" &
                        " AS 'Buyer Company', POM_PO_INDEX,  POM_B_COY_ID, " &
                        " (POD_ORDERED_QTY - POD_CANCELLED_QTY) AS 'Ordered Qty', " &
                        " SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty'," &
                        " DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) AS 'DUE DATE', POM_PO_STATUS, PM.POM_URGENT " &
                        " FROM PO_MSTR PM, PO_DETAILS, COMPANY_MSTR " &
                        " WHERE POM_B_COY_ID = CM_COY_ID AND PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND  POD_COY_ID = POM_B_COY_ID " &
                        " AND POD_PO_NO = POM_PO_NO AND PM.POM_PO_STATUS IN ('3') AND pm.pom_fulfilment IN ('1','2','4') AND EXISTS " &
                        " (SELECT '*' FROM PO_Details " &
                        " WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " &
                        " AND  DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) < GETDATE()" &
                        " GROUP BY POD_Coy_ID,POD_PO_NO " &
                        " HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0) " &
                        " GROUP BY POM_PO_No, POD_D_ADDR_CODE" &
                        " ORDER BY POM_PO_Date DESC, POM_PO_Index DESC "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetOutstandingRFQBuyer() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SELECT RM_RFQ_ID, RM_RFQ_No AS 'RFQ Number', RM_RFQ_Name AS 'RFQ Name', " &
                "RM_CREATED_ON AS 'Creation Date', RM_EXPIRY_DATE AS 'Expiry Date', RM_STATUS, '' As 'Status Desc', RM_RFQ_OPTION " &
                "FROM RFQ_MSTR WHERE RM_EXPIRY_DATE >= GETDATE() and RM_B_Display_Status='0' " &
                "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                "AND RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR FORCE INDEX(IDX_POM_B_COY_ID) WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"
            strsql = strsql & " UNION SELECT RM_RFQ_ID, RM_RFQ_No AS 'RFQ Number', RM_RFQ_Name AS 'RFQ Name', " &
                "RM_CREATED_ON AS 'Creation Date', RM_EXPIRY_DATE AS 'Expiry Date', RM_STATUS, '' As 'Status Desc', RM_RFQ_OPTION " &
                "FROM RFQ_MSTR WHERE RM_EXPIRY_DATE < GETDATE() and RM_STATUS = 3 and RM_B_Display_Status='0' " &
                "AND RM_Created_By='" & HttpContext.Current.Session("UserID") & "' AND RM_Coy_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                "AND RM_RFQ_ID NOT IN (SELECT POM_RFQ_INDEX FROM PO_MSTR FORCE INDEX(IDX_POM_B_COY_ID) WHERE POM_RFQ_INDEX IS NOT NULL AND POM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "')"
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetOutstandingRFQ() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Michelle (14/11/2011) - Force the system to use the index (rm_status)
            strsql = "SELECT RM.RM_RFQ_Name AS 'RFQ Name',RM.RM_RFQ_ID,RM.RM_RFQ_No AS 'RFQ Number', " &
                " RM.RM_Created_On AS 'Creation Date',RM.RM_Expiry_Date AS 'Expiry Date',CM.CM_COY_NAME AS 'Buyer Company', RM_Coy_ID " &
                " FROM RFQ_MSTR RM FORCE INDEX(idx_rfq_mstr_2),RFQ_VENDOR_MSTR RVM,COMPANY_MSTR CM WHERE RM.RM_Status<>'3' AND RVM.RVM_V_Company_ID='" & HttpContext.Current.Session("CompanyID") & "' AND " &
                " RM.RM_RFQ_ID=RVM.RVM_RFQ_ID AND CM.CM_COY_ID=RM.RM_Coy_ID AND RM_EXPIRY_DATE >= GETDATE()" &
                " AND RVM.RVM_V_Display_Status='0'" &
                " AND RM.RM_B_DISPLAY_STATUS = '0' AND (RM.RM_RFQ_ID NOT IN (SELECT RRM_RFQ_ID FROM RFQ_REPLIES_MSTR WHERE RRM_V_COMPANY_ID = '" & HttpContext.Current.Session("CompanyID") & "') AND RVM.RVM_V_RFQ_STATUS = 0)"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetOutstandingInvoice() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            '2015-07-03: CH: Rounding issue (Prod issue)
            strsql = " SELECT DISTINCT POM_PO_NO AS 'PO Number',POM_PO_INDEX,CDM_DO_No AS 'DO Number', CDM_GRN_NO AS 'GRN Number' ,CM_COY_NAME,POM_CURRENCY_CODE, " &
                    " (SELECT SUM(ROUND(DOD_DO_QTY* POD_UNIT_COST,2) + ROUND(ROUND(DOD_DO_QTY* POD_UNIT_COST,2) * (IFNULL(POD_GST,0)/100),2)) AS total " &
                    " FROM DO_DETAILS A, PO_DETAILS B,DO_MSTR C WHERE  POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO AND " &
                    "  A.DOD_PO_LINE=B.POD_PO_LINE  AND  DOD_DO_QTY > 0 AND DOD_DO_NO = DOM_DO_NO AND DOD_S_COY_ID = DOM_S_COY_ID " &
                    " AND DOM_PO_INDEX = POM_PO_INDEX AND DOM_DO_NO = CDM_DO_NO AND DOM_DO_NO = CDM_DO_NO GROUP BY DOD_DO_NO,DOD_S_COY_ID ) + POM_SHIP_AMT AS 'Amount' " &
                    " FROM COMPANY_MSTR,PO_MSTR,COMPANY_DOC_MATCH  WHERE POM_S_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND CDM_B_COY_ID=POM_B_COY_ID AND " &
                    " CDM_PO_NO=POM_PO_NO  AND CM_COY_ID = POM_B_COY_ID AND " &
                    " POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS) AND POM_PO_STATUS <>'4' AND POM_PO_STATUS <> '5'  AND " &
                    " POM_BILLING_METHOD IS NOT NULL  AND CDM_PO_NO = POM_PO_NO  AND POM_BILLING_METHOD<>''  AND " &
                    " CDM_INVOICE_NO IS NULL  AND ( ((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL  AND " &
                    " (SELECT COUNT(POD_PO_LINE) FROM PO_DETAILS " &
                    " WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )= (SELECT COUNT(*) FROM PO_DETAILS " &
                    " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY AND " &
                    " POD_PO_NO = POM_PO_NO   AND POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO))) OR " &
                    " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN " &
                    " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER JOIN DO_MSTR ON DOM_DO_NO=CDM_DO_NO AND  DOM_DO_STATUS IN(2)) AND " &
                    " 0 < (SELECT COUNT(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX  AND " &
                    " 0 < (SELECT COUNT(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO  AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND " &
                    " DOD_DO_QTY > 0  AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) ) OR " &
                    " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL AND " &
                    " 0 < (SELECT COUNT(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO AND " &
                    " 0 < (SELECT COUNT(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID  AND " &
                    " (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '')  AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND " &
                    " GM_GRN_STATUS='1' GROUP BY GM_GRN_NO,GM_B_COY_ID)))) "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetPendingAppr(ByVal pType As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            If pType <> "AO" Then 'those submitted by the user
                strsql = "SELECT PM.POM_PO_Index,PM.POM_PO_No AS 'PO Number',PM.POM_S_Coy_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_Status, " &
                    "PM.POM_S_COY_NAME AS 'Vendor Name', POM_PO_COST AS 'Amount', UM1.UM_USER_NAME AS 'Purchaser', '1' as 'Frm', POM_B_COY_ID " &
                    "FROM PO_MSTR PM LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID=PM.POM_BUYER_ID AND UM1.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                    "WHERE PM.POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND POM_BUYER_ID='" & HttpContext.Current.Session("UserID") & "' AND PM.POM_PO_Status IN('7', '8') ORDER BY 'Submitted Date'"
            Else  'For Approving Officer Dashboard
                strsql = "SELECT DISTINCT PM.POM_PO_Index, PM.POM_S_COY_NAME AS 'Vendor Name', PM.POM_PO_No AS 'PO Number', POM_B_COY_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', POM_B_COY_ID, " &
                         "PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_STATUS, PM.POM_PO_COST AS 'Amount', PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME AS 'Purchaser' " &
                         "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID AND " &
                         "UM1.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' WHERE PA.PRA_FOR = 'PO' AND (PA.PRA_AO = '" & HttpContext.Current.Session("UserID") & "' OR (PA.PRA_A_AO = '" & HttpContext.Current.Session("CompanyID") & "' AND PA.PRA_Relief_Ind='O')) " &
                         "AND PM.POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status IN( '7', '8'))"

                'strsql = "SELECT DISTINCT PRA_AO, PM.POM_B_COY_ID, PM.POM_PO_STATUS, PM.POM_PO_Index,PM.POM_PO_No AS 'PO Number',PM.POM_S_Coy_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', PM.POM_CURRENCY_CODE AS 'Currency', " & _
                '"PM.POM_S_COY_NAME AS 'Vendor Name' , PM.POM_PO_COST AS 'Amount',UM1.UM_USER_NAME AS 'Purchaser', '2' as 'Frm' " & _
                '"FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " & _
                '"LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID AND UM1.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"WHERE PRA_FOR = 'PO' AND (PA.PRA_AO = '" & HttpContext.Current.Session("UserID") & "' OR (PA.PRA_A_AO = '" & HttpContext.Current.Session("UserID") & "' " & _
                '"AND PA.PRA_Relief_Ind='O')) AND PM.POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status NOT IN(78))" & _
                '"UNION ALL " & _
                '"SELECT DISTINCT PRA_AO, PM.POM_B_COY_ID, PM.POM_PO_STATUS, PM.POM_PO_Index,PM.POM_PO_No AS 'PO Number',PM.POM_S_Coy_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', PM.POM_CURRENCY_CODE AS 'Currency', " & _
                '"PM.POM_S_COY_NAME AS POM_S_COY_NAME , PM.POM_PO_COST AS 'Amount',UM1.UM_USER_NAME AS 'Purchaser', '3' as 'Frm' " & _
                '"FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.POM_Dept_Index " & _
                '"LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID AND UM1.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY AND UM.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"WHERE PA.PRA_FOR = 'PO' AND PA.PRA_AO IN(SELECT AGA_AO FROM APPROVAL_GRP_AO A  LEFT JOIN USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND B.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"LEFT JOIN APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX AND M.AGM_COY_ID = B.UM_COY_ID AND " & _
                '"M.AGM_TYPE='PO' WHERE M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"AND AGA_A_AO='" & HttpContext.Current.Session("UserID") & "' AND AGA_RELIEF_IND='C' " & _
                '"AND AGA_AO IN (SELECT RAM_USER_ID  FROM RELIEF_ASSIGNMENT_MSTR  WHERE RAM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"AND RAM_USER_ROLE='Approving Officer' AND GETDATE() BETWEEN RAM_START_DATE AND RAM_END_DATE)) AND " & _
                '"PA.PRA_A_AO = '" & HttpContext.Current.Session("UserID") & "'AND PA.PRA_Relief_Ind='C' AND PM.POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
                '"AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status NOT IN(7,8)) ORDER BY 'Submitted Date'"
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetPendingAppr2(ByVal pType As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            If pType <> "AO" Then 'those submitted by the user
                strsql = "SELECT PM.POM_RFQ_Index, PM.POM_PO_Index,PM.POM_PO_No AS 'PO Number',PM.POM_S_Coy_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_Status, " &
                    "PM.POM_S_COY_NAME AS 'Vendor Name', POM_PO_COST AS 'Amount', UM1.UM_USER_NAME AS 'Purchaser', '1' as 'Frm', POM_B_COY_ID, PM.POM_URGENT " &
                    "FROM PO_MSTR PM LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID=PM.POM_BUYER_ID AND UM1.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                    "WHERE PM.POM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND POM_FULFILMENT = '0' AND POM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PM.POM_PO_Status IN('7', '8') ORDER BY 'Submitted Date'"
            Else  'For Approving Officer Dashboard
                strsql = "SELECT DISTINCT PM.POM_RFQ_Index, PM.POM_PO_Index, POM_S_COY_ID,PM.POM_S_COY_NAME AS 'Vendor Name', PM.POM_PO_No AS 'PO Number', POM_B_COY_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', POM_B_COY_ID, " &
                         "PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_STATUS, PM.POM_PO_COST AS 'Amount', PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME AS 'Purchaser', PM.POM_URGENT " &
                         "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID AND " &
                         "UM1.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE PA.PRA_FOR = 'PO' AND POM_FULFILMENT = '0' AND (PA.PRA_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' OR (PA.PRA_A_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PA.PRA_Relief_Ind='O')) " &
                         "AND PM.POM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status IN( '7', '8'))"
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetPendingAppr2Ent(ByVal pType As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            If pType <> "AO" Then 'those submitted by the user
                strsql = "SELECT PM.POM_RFQ_Index, PM.POM_PO_Index,PM.POM_PO_No AS 'PO Number',PM.POM_S_Coy_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_Status, " &
                    "PM.POM_S_COY_NAME AS 'Vendor Name', POM_PO_COST AS 'Amount', UM1.UM_USER_NAME AS 'Purchaser', '1' as 'Frm', POM_B_COY_ID, PM.POM_URGENT " &
                    "FROM PO_MSTR PM LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID=PM.POM_BUYER_ID AND UM1.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                    "WHERE PM.POM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND POM_FULFILMENT = '0' AND POM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PM.POM_PO_Status IN('7', '8', '11') ORDER BY 'Submitted Date'"
            Else  'For Approving Officer Dashboard
                strsql = "SELECT DISTINCT PM.POM_RFQ_Index, PM.POM_PO_Index, POM_S_COY_ID,PM.POM_S_COY_NAME AS 'Vendor Name', PM.POM_PO_No AS 'PO Number', POM_B_COY_ID, PM.POM_STATUS_CHANGED_ON AS 'Submitted Date', POM_B_COY_ID, " &
                         "PM.POM_CURRENCY_CODE AS 'Currency', PM.POM_PO_STATUS, PM.POM_PO_COST AS 'Amount', PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME AS 'Purchaser', PM.POM_URGENT " &
                         "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID AND " &
                         "UM1.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE PA.PRA_FOR = 'PO' AND POM_FULFILMENT = '0' AND (PA.PRA_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' OR (PA.PRA_A_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PA.PRA_Relief_Ind='O')) " &
                         "AND PM.POM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status IN( '7', '8', '11'))"
            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetPendingApprPR(ByVal pType As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            If pType = "AO" Then    'Approving Officer
                '2015-06-24: CH: Rounding issue (Prod issue)
                'strsql = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No AS 'PR Number',UM.UM_USER_NAME AS 'Buyer'," _
                '    & "PM.PRM_BUYER_ID,CD.CDM_DEPT_NAME AS 'Buyer Department',PRM_SUBMIT_DATE AS 'Submitted Date'," _
                '    & "PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_PR_STATUS," _
                '    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') AS STATUS_DESC, " _
                '    & "PM.PRM_PR_TYPE, (SELECT SUM((prd_unit_cost * prd_ordered_qty) + (prd_unit_cost * prd_ordered_qty * (ISNULL(prd_gst,0) / 100))) FROM pr_details WHERE pm.prm_pr_no = pr_details.prd_pr_no AND pm.prm_coy_id = pr_details.prd_coy_id) AS PRM_PR_COST, PM.PRM_URGENT AS PRM_URGENT, " _
                '    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') AS NAME " _
                '    & "FROM PR_Approval PA " _
                '    & "INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
                '    & "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index AND CD.CDM_COY_ID=PRM_COY_ID " _
                '    & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID = PM.PRM_BUYER_ID AND UM.UM_COY_ID=PRM_COY_ID " _
                '    & "WHERE PRA_FOR='PR' AND PM.PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '    & "AND UM.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '    & "AND (PA.PRA_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                '    & "OR (PA.PRA_A_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PA.PRA_Relief_Ind='O')) " _
                '    & "AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.PRM_PR_Status NOT IN(6,8))"
                strsql = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No AS 'PR Number',UM.UM_USER_NAME AS 'Buyer'," _
                    & "PM.PRM_BUYER_ID,CD.CDM_DEPT_NAME AS 'Buyer Department',PRM_SUBMIT_DATE AS 'Submitted Date'," _
                    & "PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_PR_STATUS," _
                    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') AS STATUS_DESC, " _
                    & "PM.PRM_PR_TYPE, (SELECT SUM(ROUND(prd_unit_cost * prd_ordered_qty,2) + ROUND(ROUND(prd_unit_cost * prd_ordered_qty,2) * (ISNULL(prd_gst,0) / 100),2)) FROM pr_details WHERE pm.prm_pr_no = pr_details.prd_pr_no AND pm.prm_coy_id = pr_details.prd_coy_id) AS PRM_PR_COST, PM.PRM_URGENT AS PRM_URGENT, " _
                    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') AS NAME " _
                    & "FROM PR_Approval PA " _
                    & "INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
                    & "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index AND CD.CDM_COY_ID=PRM_COY_ID " _
                    & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID = PM.PRM_BUYER_ID AND UM.UM_COY_ID=PRM_COY_ID " _
                    & "WHERE PRA_FOR='PR' AND PM.PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND UM.UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND (PA.PRA_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                    & "OR (PA.PRA_A_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND PA.PRA_Relief_Ind='O')) " _
                    & "AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.PRM_PR_Status NOT IN(6,8))"

            ElseIf pType = "BUYER" Then 'Buyer
                'Modified by Joon on 22th July 2011 - not to show PR with 'Void' status (PRM_PR_STATUS=9)
                strsql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No AS 'PR Number',PM.PRM_CREATED_DATE AS 'Creation Date'," _
                    & "PRM_SUBMIT_DATE AS 'Submission Date',PRM_PR_Date AS 'Approved Date'," _
                    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') AS 'Status'," _
                    & "PM.PRM_S_Coy_ID,PM.PRM_CURRENCY_CODE,PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _
                    & "PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX,PRM_PO_INDEX, PRM_PR_TYPE, PM.PRM_URGENT AS PRM_URGENT, " _
                    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') AS NAME " _
                    & "FROM PR_MSTR PM FORCE INDEX (idx_prm_pr_status)" _
                    & "WHERE PM.PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND PRM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                    & "AND PRM_PR_STATUS NOT IN (0,5,6,8,9)"

            ElseIf pType = "PO" Then    'Purchasing Officer
                'Modified by Joon on 8th Aug 2011
                strsql = "SELECT DISTINCT PRM_PR_Index,PRM_PR_No AS 'PR Number',UM.UM_USER_NAME AS 'Buyer'," _
                    & "PRM_BUYER_ID,CD.CDM_DEPT_NAME AS 'Buyer Department',PRM_PR_Date AS 'Approved Date', PRM_URGENT AS PRM_URGENT, " _
                    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "') AS NAME " _
                    & "FROM pr_details " _
                    & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
                    & "LEFT JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PRM_Dept_Index AND CD.CDM_COY_ID=PRM_COY_ID " _
                    & "LEFT JOIN USER_MSTR UM ON UM.UM_USER_ID = PRM_BUYER_ID AND UM.UM_COY_ID=PRM_COY_ID " _
                    & "LEFT JOIN PRODUCT_MSTR ON PRD_PRODUCT_CODE=PM_PRODUCT_CODE " _
                    & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PRD_CT_ID " _
                    & "INNER JOIN USER_ASSIGN UA ON PRM_COY_ID=UA.UA_COY_ID AND CT_ROOT_PREFIX=UA.UA_ASSIGN_VALUE " _
                    & "WHERE PRM_PR_TYPE <> 'CC' AND PRD_CONVERT_TO_IND IS NULL AND PRM_PR_STATUS = 4 " _
                    & "AND PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND UA_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND UA_USER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND UA_TYPE='CT'"
                'strsql = "SELECT DISTINCT PRM_PR_Index,PRM_PR_No AS 'PR Number',UM.UM_USER_NAME AS 'Buyer'," _
                '    & "PRM_BUYER_ID,CD.CDM_DEPT_NAME AS 'Buyer Department',PRM_PR_Date AS 'Approved Date' " _
                '    & "FROM pr_details " _
                '    & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
                '    & "LEFT JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PRM_Dept_Index AND CD.CDM_COY_ID=PRM_COY_ID " _
                '    & "LEFT JOIN USER_MSTR UM ON UM.UM_USER_ID = PRM_BUYER_ID AND UM.UM_COY_ID=PRM_COY_ID " _
                '    & "LEFT JOIN PRODUCT_MSTR ON PRD_PRODUCT_CODE=PM_PRODUCT_CODE " _
                '    & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME " _
                '    & "INNER JOIN USER_ASSIGN UA ON PRM_COY_ID=UA.UA_COY_ID AND PRD_CT_ID=UA.UA_ASSIGN_VALUE " _
                '    & "WHERE PRM_PR_TYPE <> 'CC' AND PRD_CONVERT_TO_IND IS NULL AND PRM_PR_STATUS = 4 " _
                '    & "AND PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '    & "AND UA_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '    & "AND UA_USER_ID='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND UA_TYPE='CT'"

            End If
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetOutstdPO() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SELECT POM_S_COY_ID, PM.POM_RFQ_INDEX, PM.POM_PO_No AS 'PO Number',PM.POM_PO_Index,POM_PO_Date AS 'PO Date', POM_ACCEPTED_DATE AS 'Accepted Date', POM_S_COY_NAME AS 'Vendor Name', POM_B_COY_ID, POM_PO_STATUS, " &
                "(SELECT SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) FROM(PO_DETAILS) " &
             "WHERE POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO) AS 'Total PO Qty', " &
                "SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Outstanding PO Qty',  PM.POM_B_COY_ID, PM.POM_URGENT " &
                "FROM PO_MSTR PM, PO_DETAILS WHERE POM_B_COY_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND POD_COY_ID = POM_B_COY_ID AND " &
                "POD_PO_NO = POM_PO_NO AND PM.POM_PO_STATUS IN ('0','1','2','3','9') AND " &
                "EXISTS (SELECT '*' FROM PO_Details WHERE PM.POM_PO_No=POD_PO_NO AND PM.POM_B_Coy_ID=POD_Coy_ID " &
                "AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserID") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                "GROUP BY POD_Coy_ID,POD_PO_NO HAVING (POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) > 0) " &
                "GROUP BY PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date, POM_B_COY_ID, POM_PO_STATUS " &
                "ORDER BY POM_PO_Date DESC, POM_PO_Index DESC"
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function GetInPendingPymt() As DataSet
            Dim ds, ds1 As DataSet
            Dim strsql, strsql1 As String
            Dim i, intCnt As Integer
            Dim strDeptList As String = ""
            Dim objInv As New Invoice

            strsql = "SELECT CM_FINDEPT_MODE, FUD_DEPT_CODE, FUD_VIEWOPTION, " &
                     "CDM_DEPT_INDEX FROM COMPANY_MSTR LEFT JOIN FINANCE_USER_DEPARTMENT ON CM_COY_ID = FUD_COY_ID AND CM_STATUS = 'A' AND " &
                     "CM_DELETED <> 'Y' LEFT JOIN COMPANY_DEPT_MSTR ON CDM_COY_ID = FUD_COY_ID AND CDM_DEPT_CODE = FUD_DEPT_CODE " &
                     "WHERE FUD_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND " &
                     "FUD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND " &
                     "(CDM_DEPT_INDEX IS NOT NULL OR CDM_DEPT_INDEX IS NULL AND FUD_VIEWOPTION = 1)"
            ds = objDb.FillDs(strsql)

            intCnt = ds.Tables(0).Rows.Count()
            For i = 0 To intCnt - 1
                If Common.parseNull(ds.Tables(0).Rows(i).Item("CM_FINDEPT_MODE")) = "Y" Then
                    If strDeptList <> "" Then
                        strDeptList &= "," & Common.parseNull(ds.Tables(0).Rows(i).Item("CDM_DEPT_INDEX"))
                    Else
                        strDeptList = Common.parseNull(ds.Tables(0).Rows(i).Item("CDM_DEPT_INDEX"))
                    End If
                End If
            Next

            If objInv.get_invWF Then 'With invoice approval
                strsql1 = "SELECT 'EPROC' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_PAYMENT_DATE AS 'Due Date', IM_INVOICE_TOTAL AS 'Amount', POM_CURRENCY_CODE AS 'Currency', POM_BUYER_NAME AS 'Buyer', POM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
                              "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                              "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX " &
                              "WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                              "AND IM_INVOICE_STATUS IN (2,5) AND IM_FOLDER = 0 AND " &
                              "IM_INVOICE_INDEX IN ( SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL " &
                              "WHERE FA_SEQ - 1 = FA_AO_ACTION AND (FA_AO = '" & HttpContext.Current.Session("UserID") & "' OR (FA_A_AO = '" & HttpContext.Current.Session("UserID") & "' " &
                              "AND FA_Relief_Ind='O')) AND (FA_AGA_TYPE = 'FM' OR (FA_AGA_TYPE = 'FO' AND FA_SEQ > FA_AO_ACTION))) "

                If strDeptList <> "" Then 'ie can view all depts
                    strsql1 &= "AND POM_DEPT_INDEX IN (" & strDeptList & ")"
                End If
            Else 'Without Invoice Approval
                strsql1 = "SELECT 'EPROC' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_PAYMENT_DATE AS 'Due Date', IM_INVOICE_TOTAL AS 'Amount', POM_CURRENCY_CODE AS 'Currency', POM_BUYER_NAME AS 'Buyer', POM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
                              "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                              "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX " &
                              "WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                              "AND IM_INVOICE_STATUS IN (2,5) AND IM_FOLDER = 0  "
                If strDeptList <> "" Then 'ie can view all depts
                    strsql1 &= "AND POM_DEPT_INDEX IN (" & strDeptList & ")"
                End If
            End If
            'Michelle (26/2/2013) - Issue 1857
            'strsql1 &= " UNION SELECT 'IPP' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_PAYMENT_DATE AS 'Due Date', " & _
            strsql1 &= " UNION SELECT 'IPP' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_DUE_DATE AS 'Due Date', " &
             "IM_INVOICE_TOTAL AS 'Amount', IM_CURRENCY_CODE AS 'Currency', '' AS 'Buyer', " &
             "IM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
             "FROM INVOICE_MSTR  LEFT JOIN COMPANY_DEPT_MSTR ON IM_DEPT_INDEX = CDM_DEPT_INDEX " &
             "LEFT JOIN USER_MSTR ON UM_USER_ID = IM_CREATED_BY " &
             "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
             "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND IM_INVOICE_STATUS IN (12) AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
             "AND (FA_AO = '" & HttpContext.Current.Session("UserId") & "' OR FA_A_AO='" & HttpContext.Current.Session("UserId") & "') AND FA_AO_ACTION = (FA_SEQ - 1)"
            ds1 = objDb.FillDs(strsql1)

            Return ds1
        End Function
        Function GetInPendingPymtFM() As DataSet
            '//Use Data Shaping because it is difficut to filter second table if we use DataRelation of ADO.Net
            Dim objDb1 As New EAD.DBCom
            Dim strSql, strSql_1, strSql_2, strSql_3, strCoyID, strUserID, strDeptAllowed As String
            Dim strParentCol, strChildCol As String
            Dim blnFinDept As Boolean = False
            Dim strTemp As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSql = "SELECT CM_FINDEPT_MODE, FUD_DEPT_CODE, FUD_VIEWOPTION, CDM_DEPT_INDEX "
            strSql &= "FROM COMPANY_MSTR "
            strSql &= "LEFT JOIN FINANCE_USER_DEPARTMENT ON CM_COY_ID = FUD_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
            strSql &= "LEFT JOIN COMPANY_DEPT_MSTR ON CDM_COY_ID = FUD_COY_ID AND CDM_DEPT_CODE = FUD_DEPT_CODE "
            strSql &= "WHERE FUD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND FUD_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strSql &= "AND (CDM_DEPT_INDEX IS NOT NULL OR CDM_DEPT_INDEX IS NULL AND FUD_VIEWOPTION = 1) "
            Dim tDS As DataSet = objDb1.FillDs(strSql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("CM_FINDEPT_MODE") = "N" Then ' Finance department mode set to off, so can view all dept
                    Exit For
                ElseIf tDS.Tables(0).Rows(j).Item("FUD_VIEWOPTION") = "1" Then ' view option = 1, so can view all dept
                    Exit For
                Else
                    blnFinDept = True
                    strDeptAllowed &= tDS.Tables(0).Rows(j).Item("CDM_DEPT_INDEX").ToString.Trim & ","
                End If
            Next

            '//join table to get PO Number
            strSql_1 = "select X.* from ( SELECT IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', POM_PO_Date, IM_PAYMENT_DATE AS 'Due Date', IM_INVOICE_TOTAL AS 'Amount', POM_CURRENCY_CODE AS 'Currency', IM_PRINTED, "
            strSql_1 &= "IM_PAYMENT_TERM, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, IM_PO_INDEX, IM_S_COY_ID, POM_BILLING_METHOD, IM_INVOICE_STATUS, "
            strSql_1 &= "POM_BUYER_NAME AS 'Buyer', POM_S_COY_NAME AS 'Vendor Name', STATUS_DESC, STATUS_REMARK, POM_PO_NO, CDM_DEPT_NAME AS 'Buyer Department', POM_PAYMENT_METHOD, POM_PO_INDEX "
            strSql_1 &= "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX "
            strSql_1 &= "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' "
            strSql_1 &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX "
            strSql_1 &= "WHERE POM_B_COY_ID = '" & strCoyID & "' "

            Dim objCompany As New Companies
            Dim strInvAppr As String = objCompany.GetInvApprMode(strCoyID)
            strSql_1 &= "AND IM_INVOICE_STATUS = '3' AND IM_FOLDER = 0 "
            strSql_1 &= ") X"

            strSql = strSql_1 & strSql_2
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)

            objDB = Nothing
            Return ds
        End Function
        Public Function GetInInv() As DataSet
            Dim ds, ds1 As DataSet
            Dim strsql, strsql1 As String
            Dim i, intCnt As Integer
            Dim strDeptList As String = ""
            Dim objInv As New Invoice

            strsql = "SELECT CM_FINDEPT_MODE, FUD_DEPT_CODE, FUD_VIEWOPTION, " &
                     "CDM_DEPT_INDEX FROM COMPANY_MSTR LEFT JOIN FINANCE_USER_DEPARTMENT ON CM_COY_ID = FUD_COY_ID AND CM_STATUS = 'A' AND " &
                     "CM_DELETED <> 'Y' LEFT JOIN COMPANY_DEPT_MSTR ON CDM_COY_ID = FUD_COY_ID AND CDM_DEPT_CODE = FUD_DEPT_CODE " &
                     "WHERE FUD_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND " &
                     "FUD_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND " &
                     "(CDM_DEPT_INDEX IS NOT NULL OR CDM_DEPT_INDEX IS NULL AND FUD_VIEWOPTION = 1)"
            ds = objDb.FillDs(strsql)

            intCnt = ds.Tables(0).Rows.Count()
            For i = 0 To intCnt - 1
                'Modified by Joon on 14 Sept 2011 for issue 853
                If Common.parseNull(ds.Tables(0).Rows(i).Item("CM_FINDEPT_MODE")) = "Y" Then
                    If strDeptList <> "" Then
                        strDeptList &= "," & Common.parseNull(ds.Tables(0).Rows(i).Item("CDM_DEPT_INDEX"))
                    Else
                        strDeptList = Common.parseNull(ds.Tables(0).Rows(i).Item("CDM_DEPT_INDEX"))
                    End If
                End If
            Next

            'Zulham 07072018 - PAMB
            'Added a condition for resident type
            Dim strUserID = HttpContext.Current.Session("UserID")
            Dim isResident = objDb.GetVal("SELECT DISTINCT IFNULL(agm_resident,'') 'agm_resident' " &
                                            "From approval_grp_mstr, approval_grp_fo " &
                                            "Where agfo_grp_index = agm_grp_index " &
                                            "And (agfo_fo = '" & strUserID & "' OR agfo_a_fo = '" & strUserID & "' OR agfo_a_fo_2 = '" & strUserID & "' OR agfo_a_fo_3 = '" & strUserID & "' OR agfo_a_fo_4 = '" & strUserID & "') " &
                                            "LIMIT 1 ")
            'End

            If objInv.get_invWF Then 'With invoice approval
                strsql1 = "SELECT 'EPROC' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_PAYMENT_DATE AS 'Due Date', IM_INVOICE_TOTAL AS 'Amount', POM_CURRENCY_CODE AS 'Currency', POM_BUYER_NAME AS 'Buyer', POM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
                              "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                              "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX " &
                              "WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                              "AND IM_INVOICE_STATUS IN (1,2,5) AND IM_FOLDER = 0 AND " &
                              "IM_INVOICE_INDEX IN ( SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL " &
                              "WHERE FA_SEQ - 1 = FA_AO_ACTION AND (FA_AO = '" & HttpContext.Current.Session("UserID") & "' " &
                              "OR (FA_A_AO = '" & HttpContext.Current.Session("UserID") & "' AND FA_Relief_Ind='O'))) "
                If strDeptList <> "" Then 'ie can view all depts
                    strsql1 &= "AND POM_DEPT_INDEX IN (" & strDeptList & ")"
                End If

                'Jules 2018.07.23 - Commented. (P2P) user should be able to see all invoices tied to them regardless of resident status
                ''Zulham 07072018 - PAMB
                'Select Case isResident
                '    Case "Y", "N"
                '        strsql1 &= " AND IM_RESIDENT_TYPE = '" & isResident & "' "
                'End Select
                ''End
                'End modification.

            Else 'Without Invoice Approval
                strsql1 = "SELECT 'EPROC' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_PAYMENT_DATE AS 'Due Date', IM_INVOICE_TOTAL AS 'Amount', POM_CURRENCY_CODE AS 'Currency', POM_BUYER_NAME AS 'Buyer', POM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
                              "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                              "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX " &
                              "WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                              "AND IM_INVOICE_STATUS IN (1) AND IM_FOLDER = 0  "
                If strDeptList <> "" Then 'ie can view all depts
                    strsql1 &= "AND POM_DEPT_INDEX IN (" & strDeptList & ")"
                End If

                'Jules 2018.07.23 - Commented. (P2P) user should be able to see all invoices tied to them regardless of resident status
                ''Zulham 07072018 - PAMB
                'Select Case isResident
                '    Case "Y", "N"
                '        strsql1 &= " AND IM_RESIDENT_TYPE = '" & isResident & "' "
                'End Select
                ''End
                'End modification.

            End If
            'Michelle (26/2/2013) - Issue 1857
            'Zulham 10.07.2018 - PAMB
            'Added status 19 for tax officer
            strsql1 &= " UNION SELECT 'IPP' AS DOC_TYPE,IM_INVOICE_INDEX, IM_INVOICE_NO AS 'Invoice Number', IM_DUE_DATE AS 'Due Date', " &
           "IM_INVOICE_TOTAL AS 'Amount', IM_CURRENCY_CODE AS 'Currency', '' AS 'Buyer', " &
           "IM_S_COY_NAME AS 'Vendor Name', CDM_DEPT_NAME AS 'Buyer Department', IM_S_COY_ID " &
           "FROM INVOICE_MSTR  LEFT JOIN COMPANY_DEPT_MSTR ON IM_DEPT_INDEX = CDM_DEPT_INDEX " &
           "LEFT JOIN USER_MSTR ON UM_USER_ID = IM_CREATED_BY " &
           "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
           "WHERE IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND IM_INVOICE_STATUS IN (11,12,19) AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
           "AND (FA_AO = '" & HttpContext.Current.Session("UserId") & "' OR FA_A_AO='" & HttpContext.Current.Session("UserId") & "' OR FA_A_AO_2='" & HttpContext.Current.Session("UserId") & "' OR FA_A_AO_3='" & HttpContext.Current.Session("UserId") & "' OR FA_A_AO_4='" & HttpContext.Current.Session("UserId") & "') AND FA_AO_ACTION = (FA_SEQ - 1)"
            ds1 = objDb.FillDs(strsql1)

            Return ds1
        End Function
        Public Function GetInDO() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Michelle (13/10/2011) - Issue 1017
            'Chee Hong (30 Mar 2015) - 1. Temporary filter the On Going DO before GST cut off - Issue 8317
            'Chee Hong (30 Mar 2015) - 2. Table is on_going_do
            'strsql = "SELECT DOM_DO_INDEX, DOM_DO_NO, POM_PO_NO, POM_PO_DATE,DOM_DO_DATE, CM_COY_NAME " & _
            '    "FROM do_mstr, PO_MSTR, COMPANY_MSTR " & _
            '    "WHERE POM_PO_INDEX = DOM_PO_INDEX AND POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
            '    "AND POM_S_COY_ID = CM_COY_ID AND POM_BILLING_METHOD <> 'DO' AND DOM_DO_STATUS=2 AND DOM_DO_INDEX NOT IN " & _
            '    "(SELECT GM_DO_INDEX FROM GRN_MSTR WHERE GM_PO_INDEX=DOM_PO_INDEX) " & _
            '    "ORDER BY DOM_DO_DATE DESC"
            'Zulham 30112018
            strsql = "SELECT DOM_DO_INDEX, DOM_DO_NO, POM_PO_NO, POM_PO_DATE,DOM_DO_DATE, CM_COY_NAME, DOM_S_COY_ID " &
                    "FROM do_mstr, PO_MSTR, COMPANY_MSTR " &
                    "WHERE POM_PO_INDEX = DOM_PO_INDEX AND POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                    "AND POM_S_COY_ID = CM_COY_ID AND POM_BILLING_METHOD <> 'DO' AND DOM_DO_STATUS=2 AND DOM_DO_INDEX NOT IN " &
                    "(SELECT GM_DO_INDEX FROM GRN_MSTR WHERE GM_PO_INDEX=DOM_PO_INDEX UNION ALL " &
                    "SELECT ODO_DO_INDEX FROM ON_GOING_DO) " &
                    "AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserID") & "' " &
                    "ORDER BY DOM_DO_DATE DESC"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetPendingQC() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SELECT GM_GRN_INDEX, GM_GRN_NO, GM_DATE_RECEIVED, DOM_DO_NO, POM_PO_NO, CM_COY_NAME " &
                "FROM do_mstr, PO_MSTR, GRN_MSTR, COMPANY_MSTR " &
                "WHERE POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND POM_S_COY_ID = CM_COY_ID " &
                "AND POM_PO_INDEX = DOM_PO_INDEX AND POM_PO_INDEX = GM_PO_INDEX AND GM_DO_INDEX = DOM_DO_INDEX AND GM_GRN_STATUS=3 " &
                "ORDER BY DOM_DO_DATE DESC"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function getOutstdIPPDoc() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Zulham 8317 23/02/2015
            strsql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_CREATED_ON,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_SUBMIT_DATE,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " _
                    & "FROM INVOICE_MSTR WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                    & "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND " &
                    "IM_INVOICE_STATUS IN ('10','16') OR (IM_INVOICE_STATUS = 14 AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND IFNULL(im_route_to,'') = '') OR (IM_INVOICE_STATUS = 14 AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "')"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        'Public Function getOutstdIPPPRCSRevDate() As DataSet
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    strsql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_CREATED_ON,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_SUBMIT_DATE,IM_CREATED_BY,IM_PRCS_SENT " _
        '            & "FROM INVOICE_MSTR WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
        '            & "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND " & _
        '            "IM_INVOICE_STATUS = 17"
        '    ds = objDb.FillDs(strsql)
        '    Return ds
        'End Function
        Function getOutstgGRNForQC() As DataSet
            Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPOField As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT iv_grn_no, cm_coy_name, pom_po_no, dom_do_no, gm_created_by, " &
                     "gm_date_received FROM inventory_verify, grn_mstr, company_mstr, po_mstr, do_mstr " &
                     "WHERE gm_b_coy_id = '" & strCoyID & "' AND gm_b_coy_id = cm_coy_id AND " &
                     "gm_po_index = pom_po_index AND gm_do_index = dom_do_index AND " &
                     "iv_verify_status = 'P'"
            ds = objDb.FillDs(strSql)
            Return ds
        End Function
        Public Function getOutstdIQCDoc() As DataSet
            Dim ds As DataSet
            Dim strsql As String
            strsql = "SELECT IQCA_AO, IQCA_A_AO, IVL_VERIFY_LOT_INDEX, IVL_IQC_NO, IM_ITEM_CODE, IM_INVENTORY_NAME, IV_GRN_NO, " &
                    "IVL_LOT_QTY, DOL_LOT_NO, DOL_DO_MANUFACTURER, GM_DATE_RECEIVED, PM_IQC_TYPE, CM_COY_NAME " &
                    "FROM IQC_APPROVAL IA, INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV, " &
                    "INVENTORY_MSTR IM, GRN_MSTR GM, DO_LOT DOL, PRODUCT_MSTR PM, COMPANY_MSTR CM " &
                    "WHERE IA.IQCA_IQC_INDEX = IVL.IVL_VERIFY_LOT_INDEX " &
                    "AND (IA.IQCA_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " &
                    "OR (IA.IQCA_A_AO = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND IQCA_RELIEF_IND='O')) AND (IA.IQCA_SEQ - 1 = IA.IQCA_AO_ACTION) " &
                    "AND IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                    "AND IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                    "AND IV.IV_GRN_NO = GM.GM_GRN_NO " &
                    "AND IM.IM_COY_ID = GM.GM_B_COY_ID " &
                    "AND IVL.IVL_LOT_INDEX = DOL.DOL_LOT_INDEX " &
                    "AND IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE " &
                    "AND IM.IM_COY_ID = PM.PM_S_COY_ID " &
                    "AND GM.GM_S_COY_ID = CM.CM_COY_ID " &
                    "AND GM.GM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                    "AND IVL_STATUS IS NULL ORDER BY GM_DATE_RECEIVED, IVL_IQC_NO "
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetDashboardPanelName() As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String
            strsql = "SELECT * FROM DASHBOARD_MSTR"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetDashboardPanel(Optional ByVal role As String = "") As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String
            If role <> "" Then
                strsql = "SELECT * FROM DASHBOARD_MATRIX WHERE DM_FIXED_ROLE_ID='" & role & "'"
            Else
                strsql = "SELECT * FROM DASHBOARD_MSTR"

            End If
            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function UpdateDashboardPanel(ByVal panelID As String, ByVal panelName As String)
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String
            strsql = "UPDATE DASHBOARD_MSTR SET DM_PANEL_NAME ='" & panelName & "' WHERE DM_PANEL_ID ='" & panelID & "'"
            objDb.Execute(strsql)
        End Function
        Public Function GetDashboardMatrix(ByVal type As String, Optional ByVal role As String = "", Optional ByVal panelID As String = "", Optional ByVal panelName As String = "") As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String
            Dim strTemp As String

            If type = "all" Then
                strsql = "SELECT * FROM DASHBOARD_MSTR"
                ds = objDb.FillDs(strsql)
            End If
            If type = "list" Then
                strsql = "SELECT DASHBOARD_MATRIX.DM_FIXED_ROLE_ID,DASHBOARD_MSTR.DM_PANEL_NAME FROM DASHBOARD_MATRIX LEFT JOIN DASHBOARD_MSTR ON DASHBOARD_MATRIX.DM_PANEL_ID = DASHBOARD_MSTR.DM_PANEL_ID"
                If role = "" And panelName = "" Then

                ElseIf role <> "" And panelName <> "" Then
                    strTemp = Common.BuildWildCard(panelName)
                    strsql += " WHERE DM_FIXED_ROLE_ID='" & role & "' AND DM_PANEL_NAME" & Common.ParseSQL(strTemp) & ""
                ElseIf role <> "" Then
                    strsql += " WHERE DM_FIXED_ROLE_ID='" & role & "'"
                ElseIf panelName <> "" Then
                    strTemp = Common.BuildWildCard(panelName)
                    strsql += " WHERE DM_PANEL_NAME" & Common.ParseSQL(strTemp) & ""
                End If
                ds = objDb.FillDs(strsql)
            End If
            If type = "checkbox" Then
                strsql = "SELECT * FROM DASHBOARD_MATRIX WHERE DM_PANEL_ID ='" & panelID & "' AND DM_FIXED_ROLE_ID ='" & role & "'"
                ds = objDb.FillDs(strsql)
            End If

            Return ds
        End Function
        Public Function AddDashboardMatrix(ByVal role As String, ByVal panelID As String)
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim clearstrsql As String
            Dim strsql As String
            strsql = "INSERT INTO DASHBOARD_MATRIX (DM_FIXED_ROLE_ID, DM_PANEL_ID) VALUES ('" & role & "' , '" & panelID & "')"
            objDb.Execute(strsql)
        End Function
        Public Function ClearDashboardMatrix(ByVal role As String)
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim clearstrsql As String
            clearstrsql = "DELETE FROM DASHBOARD_MATRIX WHERE DM_FIXED_ROLE_ID='" & role & "'"
            objDb.Execute(clearstrsql)
        End Function

        Public Function ChkFMCanApprove() As Boolean
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim strsql As String
            strsql = "SELECT '*' FROM USERS_USRGRP INNER JOIN USER_ACCESS_RIGHT ON UU_USRGRP_ID = UAR_USRGRP_ID " & _
                     "INNER JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID = UAR_USRGRP_ID AND UGM_FIXED_ROLE = 'FINANCE MANAGER' " & _
                   "WHERE UU_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND " & _
                    "UU_USER_ID = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND UAR_ALLOW_UPDATE = 'Y' AND UAR_MENU_ID = 15"
            If objDb.Exist(strsql) = 1 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function checkvendorname(ByVal docno As String) As String
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT DISTINCT CM_COY_NAME FROM PR_DETAILS " & _
            "LEFT JOIN COMPANY_MSTR ON PRD_S_COY_ID = CM_COY_ID WHERE PRD_PR_NO = '" & docno & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 1 Then
                Return "Multiple Vendors"
            Else
                Return ds.Tables(0).Rows(0).Item(0)
            End If
        End Function
        Public Function checkcurrency(ByVal docno As String) As String
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT DISTINCT PRD_CURRENCY_CODE FROM PR_DETAILS " & _
            "WHERE PRD_PR_NO = '" & docno & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 1 Then
                Return "Multiple Currency"
            Else
                Return ds.Tables(0).Rows(0).Item(0)
            End If
        End Function
        Public Function PopulatePSDSent() As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_CREATED_ON,IM_SUBMIT_DATE,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv, 'Y' AS IND " & _
            '        "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
            '        "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND IM_INVOICE_STATUS IN (18)" & _
            '        "UNION " & _
            '        "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL, " & _
            '        "IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_CREATED_ON,IM_SUBMIT_DATE,IM_BANK_CODE,IM_BANK_ACCT,IC_BANK_CODE,IC_BANK_ACCT, " & _
            '        "IM_PAYMENT_DATE, IM_PAYMENT_NO, IM_PRCS_SENT, IM_PRCS_RECV, 'N' AS IND " & _
            '        "FROM INVOICE_MSTR " & _
            '        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IM_B_COY_ID = IC_COY_ID " & _
            '        "INNER JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX AND FA_AGA_TYPE = 'AO' " & _
            '        "AND (IM_STATUS_CHANGED_BY = FA_AO OR IM_STATUS_CHANGED_BY = FA_A_AO) " & _
            '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND IM_INVOICE_STATUS IN (18, 17) " & _
            '        "AND IM_STATUS_CHANGED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"

            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_CREATED_ON,IM_SUBMIT_DATE,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv, 'Y' AS IND " & _
                    "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " & _
                    "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                    "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                    "AND IM_INVOICE_STATUS IN (18)" & _
                    "UNION " & _
                    "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL, " & _
                    "IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_CREATED_ON,IM_SUBMIT_DATE,IM_BANK_CODE,IM_BANK_ACCT,IC_BANK_CODE,IC_BANK_ACCT, " & _
                    "IM_PAYMENT_DATE, IM_PAYMENT_NO, IM_PRCS_SENT, IM_PRCS_RECV, 'N' AS IND " & _
                    "FROM INVOICE_MSTR " & _
                    "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " & _
                    "INNER JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX AND FA_AGA_TYPE = 'AO' " & _
                    "AND (IM_STATUS_CHANGED_BY = FA_AO OR IM_STATUS_CHANGED_BY = FA_A_AO) " & _
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                    "AND IM_INVOICE_STATUS IN (18, 17) " & _
                    "AND IM_STATUS_CHANGED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"

            dsGroup = objDb.FillDs(strSql)
            PopulatePSDSent = dsGroup
        End Function

        Public Function PopulatePSDRecv() As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_CREATED_ON,IM_SUBMIT_DATE,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv " & _
                     "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " & _
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                    "AND IM_INVOICE_STATUS IN (17) OR (IM_INVOICE_STATUS = 14 AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "') order by im_prcs_sent"


            dsGroup = objDb.FillDs(strSql)
            PopulatePSDRecv = dsGroup

        End Function
        Public Function getIPPAOApproval() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'Zulham 30102018
            strsql = "SELECT * FROM invoice_mstr LEFT JOIN finance_approval ON im_invoice_index = fa_invoice_index " &
           "WHERE im_b_coy_id='" & HttpContext.Current.Session("CompanyId") & "' AND im_invoice_status IN ('16') AND " &
           "(fa_ao = '" & HttpContext.Current.Session("UserId") & "' or fa_a_ao = '" & HttpContext.Current.Session("UserId") & "' ) AND fa_ao_action = (fa_seq - 1)"
            '  strsql = "SELECT * FROM invoice_mstr LEFT JOIN finance_approval ON im_invoice_index = fa_invoice_index " & _
            ' "LEFT JOIN user_mstr ON um_user_id = im_created_by AND um_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " & _
            '"WHERE im_b_coy_id='" & HttpContext.Current.Session("CompanyId") & "' AND im_invoice_status IN ('16') AND " & _
            '"(fa_ao = '" & HttpContext.Current.Session("UserId") & "' or fa_a_ao = '" & HttpContext.Current.Session("UserId") & "' ) AND fa_ao_action = (fa_seq - 1) " & _
            '"AND um_dept_id = ( SELECT um_dept_id FROM user_mstr WHERE um_user_id = '" & HttpContext.Current.Session("UserId") & "' and um_coy_id = '" & HttpContext.Current.Session("CompanyId") & "')"


            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function getOutstdIRDoc() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT IRM_IR_INDEX, IRM_IR_NO, IRM_IR_URGENT, IRM_IR_DATE, IRM_IR_STATUS FROM INVENTORY_REQUISITION_MSTR " & _
                    "WHERE IRM_IR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IRM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' " & _
                    "AND (IRM_IR_STATUS = '1' OR IRM_IR_STATUS = '3') "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function getPendingMRSAck() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT DISTINCT IRSM_IRS_INDEX, IRSM_IRS_NO, IRSM_IRS_STATUS, IRSM_IRS_DATE, IRSM_IRS_APPROVED_DATE, IRSM_IRS_URGENT " & _
                    "FROM INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR " & _
                    "WHERE IRSM_IRS_INDEX = IRD_IR_SLIP_INDEX And IRD_IR_COY_ID = IRM_IR_COY_ID " & _
                    "AND IRD_IR_NO = IRM_IR_NO " & _
                    "AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IRM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' " & _
                    "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '7') "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function getPendingMyIRApproval() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT IRM_IR_INDEX, IRM_IR_NO, IRM_IR_DATE, IRM_IR_REF_NO, IRM_IR_REQUESTOR_NAME, " & _
                    "IRM_IR_APPROVED_DATE, IRM_IR_ISSUE_TO, CDM_DEPT_NAME, IRM_IR_REMARK, IRM_IR_URGENT, IRM_IR_STATUS, " & _
                    "(CASE WHEN IRM_IR_STATUS = '4' THEN 'Rejected' ELSE 'Approved' END) AS STAT, " & _
                    "(CASE WHEN IRM_IR_STATUS = '1' THEN 'Submitted' WHEN IRM_IR_STATUS = '2' THEN 'Approved' " & _
                    "WHEN IRM_IR_STATUS = '3' THEN 'Pending Approval' WHEN IRM_IR_STATUS = '4' THEN 'Rejected' " & _
                    "ELSE '' END) AS STATUS_DESC " & _
                    "FROM IR_APPROVAL IRA, INVENTORY_REQUISITION_MSTR IRM, COMPANY_DEPT_MSTR CDM " & _
                    "WHERE IRA.IRA_IR_INDEX = IRM.IRM_IR_INDEX " & _
                    "AND IRM.IRM_IR_COY_ID = CDM.CDM_COY_ID AND IRM.IRM_IR_DEPARTMENT = CDM.CDM_DEPT_CODE " & _
                    "AND IRM.IRM_IR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "AND (IRA.IRA_SEQ - 1 = IRA.IRA_AO_ACTION) AND (IRM.IRM_IR_STATUS = '3' OR IRM.IRM_IR_STATUS = '1') " & _
                    "AND (IRA.IRA_AO = '" & HttpContext.Current.Session("UserId") & "' " & _
                    "OR (IRA.IRA_A_AO = '" & HttpContext.Current.Session("UserId") & "' AND IRA.IRA_RELIEF_IND = 'O'))"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function getIssueMRS() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'strsql = "SELECT DISTINCT IRSM_IRS_INDEX, IRSM_IRS_NO, IRSM_IRS_ACCOUNT_CODE, IRSM_IRS_STATUS, IRSM_IRS_DATE, IRSM_IRS_APPROVED_DATE, IRSM_IRS_URGENT " & _
            '        "FROM INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR " & _
            '        "WHERE IRSM_IRS_INDEX = IRD_IR_SLIP_INDEX And IRD_IR_COY_ID = IRM_IR_COY_ID " & _
            '        "AND IRD_IR_NO = IRM_IR_NO " & _
            '        "AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IRM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' " & _
            '        "AND (IRSM_IRS_STATUS = '1') "

            strsql = "SELECT IRSM.*, CDM.CDM_DEPT_NAME FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM WHERE " & _
                    "IRSM_IRS_COY_ID = CDM_COY_ID And IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " & _
                    "AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
                    "AND IRSM_IRS_STATUS = '1' "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function getOutRIAck() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT IRIM_RI_NO, IRIM_RI_DATE, SUM(IRID_QTY) AS TQTY " & _
                     "FROM INVENTORY_RETURN_INWARD_MSTR M INNER JOIN INVENTORY_RETURN_INWARD_DETAILS D " & _
                     "WHERE M.IRIM_RI_COY_ID = D.IRID_RI_COY_ID AND M.IRIM_RI_NO = D.IRID_RI_NO AND IRIM_RI_STATUS = '1' AND M.IRIM_RI_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' GROUP BY IRIM_RI_NO "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function
		'Modified for IPP GST Stage 2A - CH (30/1/2015)
        Public Function getOutBillDoc() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT BM_INVOICE_INDEX, BM_INVOICE_NO, BM_S_COY_NAME, BM_INVOICE_STATUS, BM_INVOICE_TOTAL, BM_CREATED_ON, BM_SUBMIT_DATE, STATUS_DESC " & _
                    "FROM BILLING_MSTR " & _
                    "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'BIL' AND STATUS_NO = BM_INVOICE_STATUS " & _
                    "WHERE BM_B_COY_ID = '" & strCoyID & "' AND BM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                    "AND STATUS_NO IN ('1','2','4')"

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function getPendingBillApproval() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT BM_INVOICE_INDEX, BM_INVOICE_NO, BM_INVOICE_STATUS, BM_INVOICE_TYPE, BM_S_COY_NAME, BM_INVOICE_TOTAL, BM_SUBMIT_DATE " & _
                    "FROM BILLING_APPROVAL " & _
                    "INNER JOIN BILLING_MSTR ON BA_BILL_INDEX = BM_INVOICE_INDEX " & _
                    "WHERE(BA_SEQ - 1 = BA_AO_ACTION) And BM_INVOICE_STATUS = 2 " & _
                    "AND (BA_AO = '" & HttpContext.Current.Session("UserId") & "' " & _
                    "OR (BA_A_AO = '" & HttpContext.Current.Session("UserId") & "' AND BA_RELIEF_IND = 'O')) " & _
                    "AND BM_B_COY_ID = '" & strCoyID & "'"

            ds = objDb.FillDs(strSql)
            Return ds
        End Function
        '-------------------------------

        'Yap: 2015-02-27: Modified for Agora GST Stage 2
        Public Function getFOIncomingDN() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId") 

            strSql = "SELECT DNM_DN_INDEX, DNM_DN_NO, DNM_DN_DATE, DNM_INV_NO, CM_COY_NAME, DNM_CURRENCY_CODE, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, DNM_PAYMENT_TERM, DNM_PAYMENT_DATE, IM_PAYMENT_TERM, " & _
                    "DNM_DN_STATUS, DNM_DN_TOTAL AS AMOUNT " & _
                    "FROM DEBIT_NOTE_MSTR " & _
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = DNM_DN_S_COY_ID " & _
                    "LEFT JOIN INVOICE_MSTR ON IM_INVOICE_NO = DNM_INV_NO AND IM_S_COY_ID = DNM_DN_S_COY_ID " & _
                    "WHERE DNM_DN_B_COY_ID = '" & strCoyID & "' AND DNM_DN_STATUS = '1' "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function getFMIncomingPendingDN() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DNM_DN_INDEX, DNM_DN_NO, DNM_DN_DATE, DNM_INV_NO, CM_COY_NAME, DNM_CURRENCY_CODE, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, DNM_PAYMENT_TERM, DNM_PAYMENT_DATE, IM_PAYMENT_TERM, " & _
                    "DNM_DN_STATUS, DNM_DN_TOTAL AS AMOUNT " & _
                    "FROM DEBIT_NOTE_MSTR " & _
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = DNM_DN_S_COY_ID " & _
                    "LEFT JOIN INVOICE_MSTR ON IM_INVOICE_NO = DNM_INV_NO AND IM_S_COY_ID = DNM_DN_S_COY_ID " & _
                    "WHERE DNM_DN_B_COY_ID = '" & strCoyID & "' AND DNM_DN_STATUS = '2' "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function getFMPendingAckCN() As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT CNM_CN_INDEX, CNM_CN_NO, CNM_CN_DATE, CNM_CREATED_DATE, CNM_INV_NO, CM_COY_NAME, CNM_CURRENCY_CODE, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID, " & _
                    "CNM_CN_STATUS, CNM_CN_TOTAL AS AMOUNT " & _
                    "FROM CREDIT_NOTE_MSTR " & _
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_S_COY_ID " & _
                    "WHERE CNM_CN_B_COY_ID = '" & strCoyID & "' AND CNM_CN_STATUS = 1 "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function
    End Class
End Namespace