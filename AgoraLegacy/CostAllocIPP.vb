Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraLegacy
    Public Class CostAllocIPP
        Dim objDb As New EAD.DBCom
        Dim strMessage As String
        Public Function GetCostAlloc(ByVal strCostAllocCode As String, ByVal strDesc As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CAM_CA_CODE, CAM_CA_DESC FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"

            If strCostAllocCode <> "" Then
                strsql &= " AND CAM_CA_CODE LIKE '%" & strCostAllocCode & "%'"
            End If
            If strDesc <> "" Then
                strsql &= " AND CAM_CA_DESC Like '%" & strDesc & "%'"
            End If

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetSelectedCACode(ByVal strCACode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CAM_CA_CODE, CAM_CA_DESC FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND CAM_CA_CODE = '" & strCACode & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function DelCostAllocCode(ByVal dtCACode As DataTable) As Boolean
            Dim i As Integer
            Dim strSQL As String

            For i = 0 To dtCACode.Rows.Count - 1
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND id_cost_alloc_code =  '" & Common.Parse(dtCACode.Rows(i)("CACode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If

                strSQL = " DELETE FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dtCACode.Rows(i)("index")) & "'"

                objDb.Execute(strSQL)

                strSQL = " DELETE FROM COST_ALLOC_MSTR WHERE CAM_INDEX = '" & Common.Parse(dtCACode.Rows(i)("index")) & "' AND CAM_COY_ID = '" & Common.Parse(dtCACode.Rows(i)("CoyId")) & "' AND CAM_USER_ID = '" & Common.Parse(dtCACode.Rows(i)("UsrID")) & "' "

                objDb.Execute(strSQL)


            Next
            Return True

        End Function

        Public Function UpdateCostAllocCode(ByVal dsCACode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'") > 0 Then
                Return False
                Exit Function
            End If

            strSQL = "UPDATE COST_ALLOC_MSTR SET " &
                  "CAM_CA_CODE='" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'," &
                  "CAM_CA_DESC='" & Common.Parse(dsCACode.Tables(0).Rows(0)("CADesc")) & "'" &
                  " WHERE CAM_INDEX = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("Index")) & "' AND CAM_CA_CODE = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "' AND CAM_COY_ID = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CompID")) & "' AND CAM_USER_ID = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CAUserID")) & "'"


            objDb.Execute(strSQL)

            Return True

        End Function
        Public Function InsertCostAllocCode(ByVal dsCACode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            'If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "' AND CBG_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If

            strSQL = " INSERT INTO COST_ALLOC_MSTR(" &
                    "CAM_COY_ID, CAM_USER_ID," &
                    "CAM_CA_CODE, CAM_CA_DESC) " &
                    "VALUES('" & Common.Parse(dsCACode.Tables(0).Rows(0)("CompID")) & "','" & Common.Parse(dsCACode.Tables(0).Rows(0)("CAUserID")) & "'," &
                    "'" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "','" & Common.Parse(dsCACode.Tables(0).Rows(0)("CADesc")) & "')"


            objDb.Execute(strSQL)
            Return True

        End Function
        Public Sub FillCACode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CONCAT(CAM_CA_CODE , "" : "", CAM_CA_DESC) AS ddlText FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                FillDdl(pDropDownList, "ddlText", "CAM_INDEX", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub
        'Cost Allocation Details''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Function FillDdl(ByRef pDropDownList As UI.WebControls.DropDownList,
                        ByRef pstrText As String,
                        ByVal pstrValue As String,
                        ByRef pDataSource As Object,
                        Optional ByVal pDefaultText As String = "") As Boolean

            pDropDownList.DataSource = pDataSource
            pDropDownList.DataTextField = pstrText
            pDropDownList.DataValueField = pstrValue
            pDropDownList.DataBind()

            If pDefaultText <> "" Then
                pDropDownList.Items.Insert(0, pDefaultText)
            End If
        End Function

        Public Function GetCostAllocDetail(ByVal strCostAllocCode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            'strsql = "SELECT CAD_CC_CODE, CAD_BRANCH_CODE, CAD_PERCENT FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "'"

            'strsql = "SELECT CONCAT(CAD.CAD_CC_CODE , "" : "" , CC.CC_CC_DESC) AS CAD_CC_CODE , " & _
            '        "CONCAT(CAD.CAD_BRANCH_CODE , "" : "" , CB.CB_BRANCH_NAME) AS CAD_BRANCH_CODE, " & _
            '        "CAD.CAD_PERCENT FROM COST_ALLOC_DETAIL CAD " & _
            '        "INNER JOIN cost_centre cc ON cc.cc_cc_code = cad.cad_cc_code " & _
            '        "INNER JOIN company_branch cb ON cb.cb_branch_code = cad.cad_branch_code " & _
            '        "WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "'"

            strsql = "SELECT CAD.CAD_CC_CODE ,CC.CC_CC_DESC, " &
                    "CAD.CAD_BRANCH_CODE ,  CB.CB_BRANCH_NAME, " &
                    "CAD.CAD_PERCENT FROM COST_ALLOC_DETAIL CAD " &
                    "INNER JOIN cost_centre cc ON cc.cc_cc_code = cad.cad_cc_code " &
                    "INNER JOIN company_branch cb ON cb.cb_branch_code = cad.cad_branch_code " &
                    "WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function UpdateCostAllocDetail(ByVal dsCAD As DataSet, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String


            For i = 0 To dsCAD.Tables(0).Rows.Count - 1
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
                    Return False
                    Exit Function
                End If

                If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "'") > 0 Then
                    strSQL = "UPDATE COST_ALLOC_DETAIL SET " &
                         "CAD_PERCENT='" & Common.Parse(dsCAD.Tables(0).Rows(i)("Percent")) & "'" &
                         " WHERE CAD_CAM_INDEX = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "' AND CAD_BRANCH_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("BranchCode")) & "'"
                Else
                    strSQL = "INSERT INTO COST_ALLOC_DETAIL(CAD_CAM_INDEX,CAD_CC_CODE, CAD_BRANCH_CODE, CAD_PERCENT) VALUES("
                    strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "', "
                    strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "', "
                    strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("BranchCode")) & "', "
                    strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("Percent")) & "')"

                End If


                objDb.Execute(strSQL)
            Next
            Return True

        End Function
        Public Function getCostCentre() As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'strsql = "SELECT  CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE, CC_CC_DESC " _
            '        & "FROM COST_CENTRE " _
            '        & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            strsql = "SELECT CC_CC_CODE, CC_CC_DESC " _
                    & "FROM COST_CENTRE " _
                    & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            ds = objDb.FillDs(strsql)
            getCostCentre = ds
        End Function
        Public Function getBranch() As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'strsql = "SELECT  CONCAT(CB_BRANCH_CODE, "" : "" , CB_BRANCH_NAME) AS CB_BRANCH_CODE, CB_BRANCH_NAME " _
            '        & "FROM COMPANY_BRANCH " _
            '        & "WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            strsql = "SELECT  CB_BRANCH_CODE, CB_BRANCH_NAME " _
                  & "FROM COMPANY_BRANCH " _
                  & "WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

            ds = objDb.FillDs(strsql)
            getBranch = ds
        End Function
        Public Function InsertCostAllocDetail(ByVal aryCAD As ArrayList, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            'If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CBG_B_COY_ID = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "' AND CBG_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If

            For i = 0 To aryCAD.Count - 1
                If aryCAD.Item(i)(0) <> "" And aryCAD.Item(i)(1) <> "" And aryCAD.Item(i)(2) <> "" Then
                    strSQL = " INSERT INTO COST_ALLOC_DETAIL(" &
                            "CAD_CAM_INDEX, CAD_CC_CODE," &
                            "CAD_BRANCH_CODE, CAD_PERCENT) " &
                            "VALUES('" & strCACode & "','" & Common.Parse(aryCAD.Item(i)(0)) & "'," &
                            "'" & Common.Parse(aryCAD.Item(i)(1)) & "','" & Common.Parse(aryCAD.Item(i)(2)) & "')"


                    objDb.Execute(strSQL)
                End If
            Next
            Return True

        End Function
        Public Function DelCostAllocDetail(ByVal dtCADetail As DataTable, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
                Return False
                Exit Function
            End If

            For i = 0 To dtCADetail.Rows.Count - 1
                strSQL = " DELETE FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dtCADetail.Rows(i)("CADIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dtCADetail.Rows(i)("CADCCCode")) & "' AND CAD_BRANCH_CODE = '" & Common.Parse(dtCADetail.Rows(i)("CADBrCode")) & "' "

                objDb.Execute(strSQL)
            Next
            Return True

        End Function
        Public Function GetLastCACode() As Integer
            Dim LastCACode As Integer
            Dim strSQL As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSQL = "SELECT CAM_INDEX FROM COST_ALLOC_MSTR ORDER BY CAM_INDEX DESC LIMIT 1"

            LastCACode = objDb.GetVal(strSQL)
            Return LastCACode
        End Function
        Public Function CheckCADetail(ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & strCACode & "'") > 0 Then
                Return True
                'Exit Function
            Else
                Return False
            End If




        End Function
        Public Function Message(ByVal pg As System.Web.UI.Page, ByVal MsgID As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Invoice Payment")
            Dim strSQL As String
            Dim strMsg As String

            strSQL = "SELECT MM_MESSAGE FROM MESSAGE_MSTR WHERE MM_CODE = '" & MsgID & "'"
            strMsg = objDb.GetVal(strSQL)

            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "Call MsgBox(""" & strMsg & """, " & style & ", """ & title & """)"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)

        End Function
    End Class
End Namespace