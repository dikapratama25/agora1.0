Imports System
Imports System.text
Imports System.IO
Imports System.Web
Imports System.Configuration
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports CryptoClass
Imports AgoraLegacy
Imports System.Security.Cryptography

Namespace AgoraLegacy

    Public Enum EmailType_Ext
        SafetyLevel
        MaxInventory
        ReorderQuantityLevel
    End Enum

    Public Class AppGlobals_Ext
        Dim objDb As New EAD.DBCom

        Public Sub FillDelTerm(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strComp As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom

            If strComp = "" Then
                strComp = HttpContext.Current.Session("CompanyId")
            End If

            'strSql = " SELECT CDT_DEL_CODE, CDT_DEL_NAME FROM company_delivery_term " _
            '       & "WHERE CDT_DELETED = 'N' AND CDT_COY_ID = '" & strComp & "'"

            strSql = "SELECT CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')') AS DESCRIPTION, CDT_DEL_CODE, CDT_DEL_NAME FROM company_delivery_term " &
                     "WHERE CDT_DELETED = 'N' AND CDT_COY_ID = '" & strComp & "' ORDER BY CDT_DEL_INDEX "

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "DESCRIPTION", "CDT_DEL_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If

        End Sub

        Public Sub FillDdlMfgName(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal strProdCode As String)
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom

            strSql = " SELECT PM_MANUFACTURER FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_MANUFACTURER IS NOT NULL AND PM_MANUFACTURER <> '' UNION ALL SELECT PM_MANUFACTURER2 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_MANUFACTURER2 IS NOT NULL AND PM_MANUFACTURER2 <> '' UNION ALL SELECT PM_MANUFACTURER3 FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_MANUFACTURER3 IS NOT NULL AND PM_MANUFACTURER3 <> '' "

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "PM_MANUFACTURER", "PM_MANUFACTURER", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If

        End Sub

        Public Sub FillPackType(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom

            strSql = " SELECT CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')') AS PACK_DESC, CPT_PACK_CODE FROM company_packing_type " _
                   & "WHERE CPT_DELETED = 'N' AND CPT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "PACK_DESC", "CPT_PACK_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If

        End Sub

        Public Function FillTaxForVen() As DataSet
            Dim strSql As String
            Dim dsTax As New DataSet

            strSql = "SELECT TAX_AUTO_NO,TAX_CODE,TAX_PERC, " _
                & "CASE WHEN TAX_PERC='' THEN (CONCAT(TAX_CODE, ' ',TAX_PERC)) ELSE (CONCAT(TAX_CODE,' ',TAX_PERC,'','%')) END AS tax " _
                & "FROM TAX  WHERE TAX_AUTO_NO <> 1 ORDER BY TAX_AUTO_NO "

            dsTax = objDb.FillDs(strSql)
            FillTaxForVen = dsTax

        End Function

        Public Function FillDelTermForVen() As DataSet
            Dim strSql As String
            Dim dsDel As New DataSet

            strSql = "SELECT CDT_DEL_CODE, CDT_DEL_NAME FROM COMPANY_DELIVERY_TERM " _
                & "WHERE CDT_DELETED = 'N' AND CDT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            dsDel = objDb.FillDs(strSql)
            FillDelTermForVen = dsDel

        End Function

        Public Function FillCurrencyForVen() As DataSet
            Dim strSql As String
            Dim dsCurr As New DataSet

            strSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC " _
                & "WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='Currency' AND CODE_DELETED='N' AND CODE_DESC != '' " _
                & "ORDER BY CODE_DESC "

            dsCurr = objDb.FillDs(strSql)
            FillCurrencyForVen = dsCurr

        End Function

        Public Function FillPayTermForVen() As DataSet
            Dim strSql As String
            Dim dsPay As New DataSet

            strSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC " &
                    "WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='PaymentTerm' AND CODE_DELETED='N' AND CODE_DESC != '' " &
                    "ORDER BY CONVERT(CODE_VALUE, SIGNED INTEGER) "

            dsPay = objDb.FillDs(strSql)
            FillPayTermForVen = dsPay

        End Function

        Public Function FillSuppCodeForVen(ByVal strVendorId As String, Optional ByVal strSuppCode As String = "") As DataSet
            Dim strSql As String
            Dim dsPay As New DataSet

            strSql = "SELECT CVS_SUPP_CODE, CDT_DEL_CODE, CDT_DEL_NAME, CVS_CURR, CODE_DESC AS CVS_CURR_DESC " &
                    "FROM COMPANY_VENDOR_SUPPCODE " &
                    "INNER JOIN COMPANY_DELIVERY_TERM ON CDT_DELETED = 'N' AND CDT_COY_ID = CVS_B_COY_ID " &
                    "AND CDT_DEL_CODE = CVS_DELIVERY_TERM " &
                    "INNER JOIN CODE_MSTR ON CODE_CATEGORY = 'CU' AND CODE_ABBR = CVS_CURR " &
                    "WHERE CVS_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CVS_S_COY_ID = '" & strVendorId & "' "

            If strSuppCode <> "" Then
                strSql &= "AND CVS_SUPP_CODE = '" & Common.Parse(strSuppCode) & "' "
            End If

            strSql &= "ORDER BY CVS_SUPP_CODE"

            dsPay = objDb.FillDs(strSql)
            FillSuppCodeForVen = dsPay

        End Function

        Public Function SelDdlbyVal(ByVal pInVal As Object,
                                ByRef pDropDownList As UI.WebControls.DropDownList,
                                Optional ByVal pValueField As Boolean = True,
                                Optional ByVal pUcase As Boolean = False) As Boolean
            Try
                SelDdlbyVal = False
                Dim lngLoop As Long
                pDropDownList.SelectedItem.Selected = False
                If Not IsDBNull(pInVal) Then
                    'Dim ary() As String = Split(pInString, ",")
                    Dim varItem
                    For Each varItem In pDropDownList.Items
                        If pValueField Then
                            If varItem.value = pInVal Then
                                varItem.selected = True
                                SelDdlbyVal = True
                                Exit For
                            End If
                        Else
                            If pUcase Then
                                If UCase(varItem.Value) = UCase(pInVal) Then
                                    varItem.selected = True
                                    Exit For
                                    SelDdlbyVal = True
                                End If
                            Else
                                If varItem.Text = pInVal Then
                                    varItem.selected = True
                                    SelDdlbyVal = True
                                    Exit For
                                End If
                            End If
                        End If

                    Next
                Else
                    pDropDownList.SelectedIndex = 0
                End If
            Catch exp As Exception
                Return False
                pDropDownList.SelectedIndex = 0
            End Try
        End Function

        Public Sub FillVendorViaProductCode(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal sPONO As String, Optional ByVal sPROD As String = "", Optional ByVal sPOMode As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView, sProductCode As String = ""
            Dim objDB As New EAD.DBCom


            strSql = "SELECT POD_PRODUCT_CODE FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND POD_PO_NO = '" & sPONO & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Dim tDS As DataSet = objDB.FillDs(strSql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                sProductCode = sProductCode & "'" & tDS.Tables(0).Rows(j).Item("POD_PRODUCT_CODE") & "', "
            Next j

            If sProductCode = "" Then
                sProductCode = sPROD
            Else
                sProductCode = Mid(sProductCode, 1, Len(sProductCode) - 2)
            End If

            If sPOMode <> "cc" Then
                If sProductCode <> "" Then
                    'strSql = "SELECT pm_PREFER_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = pm_PREFER_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_PREFER_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_1ST_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_1ST_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_1ST_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_2ND_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_2ND_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_2ND_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_3RD_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_3RD_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE pm_product_code IN (" & sProductCode & ") AND PM_3RD_S_COY_ID <> '' "
                    'strSql &= "GROUP BY AA, BB"

                    strSql = "SELECT PV_S_COY_ID AS AA, "
                    strSql &= "(SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PV_S_COY_ID) AS BB "
                    strSql &= "FROM PRODUCT_MSTR "
                    strSql &= "INNER JOIN PIM_VENDOR ON PV_PRODUCT_INDEX = PM_PRODUCT_INDEX "
                    strSql &= "WHERE PM_PRODUCT_CODE IN (" & sProductCode & ") AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PV_S_COY_ID IS NOT NULL AND PV_S_COY_ID <> '' "
                    strSql &= "GROUP BY PV_S_COY_ID"

                Else
                    'strSql = "SELECT pm_PREFER_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = pm_PREFER_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE PM_PREFER_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_1ST_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_1ST_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE PM_1ST_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_2ND_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_2ND_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE PM_2ND_S_COY_ID <> '' "
                    'strSql &= "UNION SELECT pm_3RD_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PM_3RD_S_COY_ID) AS BB FROM product_mstr "
                    'strSql &= "WHERE PM_3RD_S_COY_ID <> '' "
                    'strSql &= "GROUP BY AA, BB"

                    strSql = "SELECT PV_S_COY_ID AS AA, "
                    strSql &= "(SELECT CM_COY_NAME FROM company_mstr WHERE CM_COY_ID = PV_S_COY_ID) AS BB "
                    strSql &= "FROM PRODUCT_MSTR "
                    strSql &= "INNER JOIN PIM_VENDOR ON PV_PRODUCT_INDEX = PM_PRODUCT_INDEX AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PV_S_COY_ID IS NOT NULL AND PV_S_COY_ID <> '' "
                    strSql &= "GROUP BY PV_S_COY_ID"
                End If
            Else
                If sProductCode <> "" Then
                    strSql = "SELECT CDM_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = CDM_S_COY_ID) AS BB FROM CONTRACT_DIST_COY, CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS "
                    strSql &= "WHERE CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDM_GROUP_INDEX = CDI_GROUP_INDEX AND CDI_PRODUCT_CODE IN (" & sProductCode & ") AND CDM_S_COY_ID <> '' AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "GROUP BY AA, BB"
                Else
                    strSql = "SELECT CDM_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = CDM_S_COY_ID) AS BB FROM CONTRACT_DIST_COY, CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS "
                    strSql &= "WHERE CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDM_GROUP_INDEX = CDI_GROUP_INDEX AND CDM_S_COY_ID <> '' AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "GROUP BY AA, BB"
                End If


                'strSql = "SELECT PM_LAST_TXN_S_COY_ID AS AA, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PM_LAST_TXN_S_COY_ID) AS BB FROM PRODUCT_MSTR "
                'strSql &= "WHERE PM_VENDOR_ITEM_CODE IN (" & sProductCode & ") AND PM_LAST_TXN_S_COY_ID <> '' "
                'strSql &= "GROUP BY AA, BB"
            End If
            If sPOMode = "bc" Then
                strSql = "SELECT "
                strSql &= "(SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = POM_S_COY_ID) AS AA, "
                strSql &= "(SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = POM_S_COY_ID) AS BB "
                strSql &= "FROM PO_MSTR, PO_DETAILS "
                strSql &= "WHERE POM_PO_NO = POD_PO_NO AND POD_PRODUCT_CODE IN (" & sProductCode & ") AND POD_PO_NO = '" & sPONO & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strSql &= "GROUP BY AA, BB "

            End If

            drw = objDB.GetView(strSql)

            'If CInt(drw.Count.ToString) < 0 Then
            'strSql = "SELECT POM_S_COY_ID as AA, POM_S_COY_NAME as BB FROM PO_MSTR WHERE POM_PO_NO = '" & sPONO & "' "
            'drw = objDB.GetView(strSql)
            'End If

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "BB", "AA", drw)
            Else
                '//no suppose to happen
            End If
            ' Add ---Select---
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            strDefaultValue = lstItem.Text
            pDropDownList.Items.Insert(0, lstItem)
            objDB = Nothing
        End Sub

        Public Function GetVenPrefViaProductCode(ByVal sProd As String) As DataSet
            Dim strSql As String
            Dim sProductCode As String = sProd
            Dim objDB As New EAD.DBCom

            strSql = " SELECT IFNULL(GROUP_CONCAT(PV_S_COY_ID), '') AS VENDOR FROM PIM_VENDOR "
            strSql &= " WHERE PV_PRODUCT_INDEX IN (SELECT DISTINCT(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE IN (" & sProductCode & ")) "

            Return objDB.FillDs(strSql)
        End Function

        'Jules 2018.05.14 - PAMB
        Public Function getGLCodeAnalysisCodeMatrix(ByVal glcode As String) As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT * FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' AND CBGCAC_B_GL_CODE = '" & glcode & "'"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function
        'End modification.
    End Class
End Namespace