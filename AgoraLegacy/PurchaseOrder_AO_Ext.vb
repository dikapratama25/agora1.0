Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO

Namespace AgoraLegacy

    Public Class PurchaseOrder_AO_Ext
        Dim objDb As New EAD.DBCom

        Function getPOForAppr(ByVal strPoNo As String, ByVal strPOIndex As String) As DataSet
            Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPOField As String
            Dim ds As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strPOField = "PM.POM_PO_NO, PM.POM_PO_INDEX, PM.POM_STATUS_CHANGED_ON, PM.POM_PO_DATE, PM.POM_BUYER_NAME, PM.POM_BUYER_PHONE, PM.POM_B_ADDR_LINE1, PM.POM_B_ADDR_LINE2, PM.POM_B_ADDR_LINE3, " _
            & "PM.POM_B_POSTCODE, PM.POM_B_CITY, PM.POM_INTERNAL_REMARK, PM.POM_S_COY_ID, PM.POM_S_COY_NAME, PM.POM_CURRENCY_CODE,PM.POM_BUYER_ID, PM.POM_PAYMENT_TERM, PM.POM_PAYMENT_METHOD, PM.POM_SHIPMENT_TERM, PM.POM_SHIPMENT_MODE, " _
            & "PM.POM_S_ADDR_LINE1,PM.POM_S_ADDR_LINE2,PM.POM_S_ADDR_LINE3,PM.POM_S_POSTCODE,PM.POM_S_CITY,PM.POM_S_STATE,PM.POM_S_COUNTRY,PM.POM_S_PHONE,PM.POM_S_FAX,PM.POM_S_EMAIL,PM.POM_SHIP_VIA,PM.POM_FREIGHT_TERMS,PM.POM_S_ATTN,PM.POM_CREATED_DATE, " _
            & "PM.POM_External_Remark, PM.POM_PO_Status,PM.POM_RFQ_INDEX, PM.POM_SHIP_AMT, PM.POM_SUBMIT_DATE, PM.POM_DEL_CODE, PM.POM_VENDOR_CODE "

            strSqlPOM = "SELECT " & strPOField & ", CM.CM_TAX_CALC_BY, " _
            & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.POM_B_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.POM_B_STATE AND CODE_VALUE=PM.POM_B_COUNTRY) AS STATE, PM.POM_URGENT, " _
            & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME, " _
            & "(SELECT CDT_DEL_NAME FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & strCoyID & "' AND CDT_DEL_CODE = PM.POM_DEL_CODE) AS DEL_NAME " _
            & "FROM PO_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.POM_S_COY_ID=CM.CM_COY_ID " _
            & "LEFT JOIN RFQ_MSTR ON PM.POM_RFQ_INDEX=RM_RFQ_ID " _
            & "WHERE " _
            & "POM_B_COY_ID='" & strCoyID & "' AND POM_PO_No='" & strPoNo & "'"

            strSqlPOD = "SELECT PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.POD_D_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.POD_D_STATE AND CODE_VALUE=PD.POD_D_COUNTRY) AS STATE, CONCAT(PD.POD_ASSET_GROUP, CONCAT(' ',PD.POD_ASSET_NO)) AS ASSET_CODE, " _
            & "CASE WHEN POD_GST_RATE = 'N/A' THEN POD_GST_RATE ELSE IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE " _
            & "FROM PO_DETAILS PD " _
            & "LEFT JOIN COMPANY_B_GL_CODE " _
            & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.POD_B_GL_CODE AND CBG_B_COY_ID =  POD_COY_ID " _
            & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND CODE_ABBR = POD_GST_RATE " _
            & "LEFT JOIN TAX ON TAX_CODE = POD_GST_RATE AND TAX_COUNTRY_CODE = 'MY' " _
            & "WHERE POD_COY_ID='" & strCoyID & "' AND POD_PO_NO='" & strPoNo & "' ORDER BY POD_PO_LINE"

            strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX=" & strPOIndex
            strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX=" & strPOIndex
            strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPoNo & "' AND CDA_DOC_TYPE='PO'"

            strSql = strSqlPOM & ";" & strSqlPOD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "PO_MSTR"
            ds.Tables(1).TableName = "PO_DETAILS"
            ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
            ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
            ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
            Return ds
        End Function

    End Class
End Namespace

