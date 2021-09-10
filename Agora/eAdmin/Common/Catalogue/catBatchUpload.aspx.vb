Imports System.IO

Imports AgoraLegacy
Imports SSO.Component
Imports System.Text.RegularExpressions

Public Class CatBatchUpload
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dg As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdBrowse As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button
    Protected WithEvents lblPath As System.Web.UI.WebControls.Label
    Protected WithEvents cmdDownload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDownloadTemplate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'MyBase.Page_Load(sender, e)
        Dim strCompanyType As String
        Dim objComp As New Companies

        strCompanyType = objComp.GetCompanyType(Session("CompanyIdToken"))
        If strCompanyType.ToUpper = "VENDOR" Or strCompanyType.ToUpper = "BOTH" Then
            blnPaging = False
            blnSorting = False
            SetGridProperty(dg)
            lblPath.Text = ViewState("FilePath")
        Else
            Dim strMsg As String
            Dim objCat As New ContCat
            strMsg = "Can only upload/download Item List for Vendor Company."
            'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)

        End If

    End Sub

    Private Function FileUpload(ByRef strUploadedPath As String, ByRef strFileName As String) As Boolean
        Try
            Dim Uploadedfilename As String
            Dim objFileMgmt As New FileManagement
            Dim strSourceFile As String = Path.GetFileName(strUploadedPath)
            Dim strTempPath As String

            Dim objCompany As New Companies
            'Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))
            'If dsAppPackage.Tables(0).Rows.Count > 0 Then
            '    Dim i As Integer

            '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
            '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
            '            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            If strTempPath <> "" Then Exit For
            '        End If
            '    Next
            'End If

            Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            If cmdBrowse.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
                Return False
            ElseIf cmdBrowse.PostedFile.ContentLength / 1024 > dblMaxFileSize Then
                Common.NetMsgbox(Me, "File exceeds maximum file size")
                Return False
            End If

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationSettings.AppSettings("eProcurePath"))

            ' modified by Ai Chu - to add session id into filename to make it unique
            strSourceFile = strSourceFile.Substring(0, strSourceFile.LastIndexOf(".")) & Session.SessionID & strSourceFile.Substring(strSourceFile.IndexOf("."))
            strFileName = strSourceFile
            If Right(strUploadedPath, 3) = "xls" Then
                'Create the temp folder for temp store the xls file in server
                If (Not Directory.Exists(strTempPath)) Then
                    Directory.CreateDirectory(strTempPath)
                End If
                cmdBrowse.PostedFile.SaveAs(strTempPath & strSourceFile)
                Return True
            Else
                Common.NetMsgbox(Me, "Target file is expecting excel file format.", MsgBoxStyle.Information)
                Return False
            End If

        Catch ex As Exception
            Common.TrwExp(ex)
            strUploadedPath = Nothing
            Return False
        End Try

    End Function

    Private Function IsExcel(ByRef pFilename As String) As Boolean

        Try
            If Right(pFilename, 3) = "xls" Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            IsExcel = Nothing
            Return False
        End Try

    End Function

    Private Function GetNewFileName(ByVal pFileName As String) As String

        Dim strNewFileName As String
        If Len(pFileName) > 4 Then
            strNewFileName = Left(pFileName, Len(pFileName) - 4) & _
                             " [" & Format(Now, "ddMMyy-HHmmss") & _
                             "].xls"

            Return strNewFileName
        End If

    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        Try
            Dim objEx As New AppExcel
            Dim ds As DataSet
            Dim cRules As New myCollection
            Dim objFileMgmt As New FileManagement
            Dim strFileName As String = Path.GetFileName(cmdBrowse.PostedFile.FileName)
            Dim strTempPath As String
            Dim strDestPath As String

            Dim objCompany As New Companies
            Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

            If dsAppPackage.Tables(0).Rows.Count > 0 Then
                Dim i As Integer

                For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                    If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                        strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                        strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                        If strTempPath <> "" Or strDestPath <> "" Then Exit For
                    End If
                Next
            End If

            If IsExcel(cmdBrowse.PostedFile.FileName) Then
                Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    ds = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemList.xml"), strTempPath & strFileName, cRules)

                    If Not ds Is Nothing Then
                        ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                        If ValidateDuplicate(ds) And ValidateCol(ds, cRules, ConfigurationSettings.AppSettings(EnumAppPackage.eProcure.ToString & "Path")) Then
                            'Create the  folder for store the uploaded xls file in server
                            If (Not Directory.Exists(strDestPath)) Then
                                Directory.CreateDirectory(strDestPath)
                            End If
                            File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                            UpdateProduct(ds)
                            ViewState("FilePath") = ""
                            lblPath.Text = ""
                            dg.DataSource = ds.Tables(0)
                            dg.DataBind()
                        Else
                            dg.DataSource = ds.Tables(0)
                            dg.DataBind()
                            File.Delete(strTempPath & strFileName)
                        End If
                    Else
                        Common.NetMsgbox(Me, objEx.Message, MsgBoxStyle.Information)
                    End If
                ElseIf cmdBrowse.PostedFile.ContentLength = 0 Then
                    Common.NetMsgbox(Me, "0 byte document or file not found")
                Else
                    Common.NetMsgbox(Me, "File exceeds maximum file size")
                End If
            Else
                Common.NetMsgbox(Me, "Target file is expecting excel file format.", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            Common.TrwExp(ex)
        End Try

    End Sub

    Private Function ValidateDuplicate(ByRef ds As DataSet) As Boolean
        Dim dr, dr1 As DataRow
        Dim bTrue As Boolean = True
        Dim iCnt, iCnt2 As Integer
        For Each dr In ds.Tables(0).Rows
            iCnt = 0
            iCnt2 = 0
            If Not IsDBNull(dr.Item(1)) Then
                Dim strWhere As String
                strWhere = "F2='" & IIf(IsDBNull(dr.Item(1)), "", dr.Item(1)) & "' AND F2 is not null"
                For Each dr1 In ds.Tables(0).Select(strWhere)
                    iCnt += 1
                    If iCnt = 2 Then
                        dr.Item("Message") &= "<LI type=square>Duplicated Item ID in the same batch.<BR>"
                        bTrue = False
                    End If
                Next
            End If

            If Not IsDBNull(dr.Item(12)) Then
                Dim strWhere As String
                strWhere = "F13='" & IIf(IsDBNull(dr.Item(12)), "", dr.Item(12)) & "' AND F13 is not null"
                For Each dr1 In ds.Tables(0).Select(strWhere)
                    iCnt2 += 1
                    If iCnt2 = 2 Then
                        dr.Item("Message") &= "<LI type=square>Duplicated vendor item code in the same batch.<BR>"
                        bTrue = False
                    End If
                Next
            End If

        Next
        Return bTrue
    End Function

    Private Function ValidateCol(ByRef ds As DataSet, ByVal pRules As myCollection, Optional ByVal pstrConnStr As String = Nothing) As Boolean
        Dim iCol As String
        Dim bRtn As Boolean = True
        Dim bTrue As Boolean = True ' keep the col valid result
        'Dim bTrue2 As Boolean = True
        Dim bAllTrue As Boolean = True

        For Each row As DataRow In ds.Tables(0).Rows '.Select(sSelect, "", DataViewRowState.CurrentRows)
            bTrue = True
            For Each itema As UploadRule In pRules

                If IsDBNull(row(Convert.ToInt16(itema.ColNo))) Then
                    If Not Convert.ToBoolean(itema.AllowNull) Then
                        row.Item("Message") &= "<LI type=square>" & itema.ColName & " is required.<BR>"
                        bTrue = False
                    End If
                Else
                    If itema.Regex <> "" And Not Regex.IsMatch(row(Convert.ToInt16(itema.ColNo)), itema.Regex) And bTrue Then
                        row("Message") &= "<LI type=square>" & itema.RegexErrMsg & "<BR>"
                        bTrue = False
                    End If

                    Dim objDb As  EAD.DBCom
                    If pstrConnStr Is Nothing Then
                        objDb = New  EAD.DBCom
                    Else
                        objDb = New  EAD.DBCom(pstrConnStr)
                    End If

                    Dim strSQL As String
                    Dim i As Integer = 0

                    If itema.SQL.Length > 1 And bTrue Then
                        For i = 0 To itema.SQL.Length - 2
                            strSQL = itema.SQL(i).Query & "'" & Common.Parse(row(Convert.ToInt16(itema.ColNo))) & "'"
                            If objDb.GetVal(strSQL) = 0 Then
                                row.Item("Message") &= "<LI type=square>" & itema.SQL(i).ErrMsg & "<BR>"
                                bTrue = False
                            End If
                        Next
                    End If
                End If

                If bTrue = False Then
                    bRtn = False
                End If
            Next

            'Only the all validation check passed then proceed to check action
            If bRtn Then
                If ActionValidation(row, pRules, pstrConnStr) = False Then
                    bTrue = False
                End If
            End If

            If bTrue = False Then
                bAllTrue = False
            End If
        Next
        Return bAllTrue
    End Function

    Public Function ActionValidation(ByRef row As DataRow, ByVal a As myCollection, Optional ByVal pstrConnStr As String = Nothing) As Boolean
        Dim bTrue As Boolean = True
        Dim strSQL As String
        Dim objDb As  EAD.DBCom

        If pstrConnStr Is Nothing Then
            objDb = New  EAD.DBCom
        Else
            objDb = New  EAD.DBCom(pstrConnStr)
        End If

        'For Each row As DataRow In ds.Tables(0).Rows '.Select(sSelect, "", DataViewRowState.CurrentRows)
        Dim objExProduct As New ExcelProduct
        objExProduct = GetProductDetails(0, row)


        'If Not IsDBNull(objExProduct.Action) Then
        If objExProduct.Action = "A" Then
            If objExProduct.VendorItemCode = "" Then
                row.Item("Message") &= "<LI type=square>" & "Vendor Item Code is required."
                bTrue = False
            Else
                strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProduct.VendorItemCode) & "'" & _
                         " AND PM_S_COY_ID='" & Common.Parse(objExProduct.CoyId) & "' AND PM_DELETED <> 'Y' "
                If objDb.Exist(strSQL) = 1 Then ' 0= no exist
                    row.Item("Message") &= "<LI type=square>" & "Vendor item code duplicated."
                    bTrue = False
                End If
            End If

        ElseIf objExProduct.Action = "M" Then
            If objExProduct.ItemID = "" Then
                row.Item("Message") &= "<LI type=square>" & "Item Id is required."
                bTrue = False
            Else
                strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(objExProduct.ItemID) & "' AND PM_DELETED <> 'Y' "
                If objDb.Exist(strSQL) = 0 Then ' 0= no exist
                    row.Item("Message") &= "<LI type=square>" & "Item Id no found."
                    bTrue = False
                End If
            End If

        ElseIf objExProduct.Action = "D" Then
            If IsDBNull(objExProduct.ItemID) Then
                row.Item("Message") &= "<LI type=square>" & "Item Id is required."
                bTrue = False
            Else
                strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(objExProduct.ItemID) & "' AND PM_DELETED <> 'Y' "
                If objDb.Exist(strSQL) = 0 Then ' 0= no exist
                    row.Item("Message") &= "<LI type=square>" & "Invalid Item Id."
                    bTrue = False
                End If

                Dim strExist1, strExist2, strExist3 As String
                ' check item exists in outstanding PR (status = 1,2,3,4)
                strExist1 = "SELECT '*' FROM PR_DETAILS LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(objExProduct.CoyId) & "' "
                strExist1 &= "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "' "

                ' check item exists in outstanding PO (status = 1,2)
                strExist2 = "SELECT '*' FROM PO_DETAILS LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO "
                strExist2 &= "WHERE POM_PO_STATUS IN (1,2) AND POD_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "'"

                ' check item exists under contract period
                strExist3 = "SELECT '*' FROM CONTRACT_DIST_ITEMS LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND CDM_S_COY_ID = '" & Common.Parse(objExProduct.CoyId) & "' "
                strExist3 &= "WHERE ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1 ) OR CDM_END_DATE IS NULL) AND CDI_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "' "
                strExist3 &= "AND CDI_GROUP_INDEX IN (SELECT CDM_GROUP_INDEX FROM CONTRACT_DIST_MSTR) "

                If (objDb.Exist(strExist1) > 0) Or (objDb.Exist(strExist2) > 0) Or (objDb.Exist(strExist3) > 0) Then
                    row.Item("Message") &= "<LI type=square>" & "It has outstanding PR(s)/PO(s)."
                    bTrue = False
                End If

                If (objDb.Exist(strExist1) > 0) Or (objDb.Exist(strExist2) > 0) Or (objDb.Exist(strExist3) > 0) Then
                    row.Item("Message") &= "<LI type=square>" & "It is under contract/discount period."
                    bTrue = False
                End If

            End If
        End If
        objExProduct = Nothing

        Return bTrue
    End Function


    Private Function UpdateProduct(ByRef pds As DataSet)
        'pds.Tables(0).Columns.Add("Status", Type.GetType("System.String"))
        Dim drItem As DataRow
        Dim objProduct As New Products

        For Each drItem In pds.Tables(0).Rows
            Dim objExProduct As New ExcelProduct
            objExProduct = GetProductDetails(0, drItem)
            If objProduct.UpdateProdByExcel(objExProduct) Then
                'If add need to assign back the auto genetrate item no
                drItem.Item(1) = objExProduct.ItemID
                Select Case objExProduct.Action
                    Case "A", "M"
                        drItem.Item("Message") = "<Font color='#000000'>Item saved.</Font>"
                    Case "D"
                        drItem.Item("Message") = "<Font color='#000000'>Item deleted.</Font>"
                End Select
            Else
                Select Case objExProduct.Action
                    Case "A", "M"
                        drItem.Item("Message") = "Item not saved."
                    Case "D"
                        drItem.Item("Message") = "Item not deleted."
                End Select
            End If
            objExProduct = Nothing

            'End If
        Next
        Common.NetMsgbox(Me, objProduct.Message, MsgBoxStyle.Information)



    End Function

    Private Function GetProductDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelProduct
        Dim objExcel As New ExcelProduct

        'If pds.Tables.Count > 0 Then
        'If pds.Tables(0).Rows.Count >= iRow Then
        With objExcel
            objExcel.RowNo = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
            objExcel.ItemID = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
            objExcel.CoyId = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
            'objExcel.ItemName = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
            objExcel.ItemDesc = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
            objExcel.UNSPSCCode = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
            objExcel.UNSPSCDesc = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))
            objExcel.UnitCost = IIf(IsDBNull(pdr.Item("F7")), 0, pdr.Item("F7"))
            objExcel.CurrencyCode = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
            objExcel.UOM = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
            objExcel.TaxCode = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
            objExcel.MgmtCode = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
            objExcel.MgmtText = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12"))
            objExcel.VendorItemCode = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))
            objExcel.Brand = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
            objExcel.Model = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
            objExcel.Action = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
        End With
        Return objExcel
        'End If
        'End If


    End Function

    'Private Function BasicValidation(ByRef ds As DataSet, ByVal a As xmlRules) As Boolean
    '    Dim dt As DataTable
    '    Dim bTrue As Boolean = True

    '    For Each row As DataRow In ds.Tables(0).Rows '.Select(sSelect, "", DataViewRowState.CurrentRows)
    '        If ValidateCol(row, a) = False Then
    '            bTrue = False
    '        End If
    '    Next
    '    Return bTrue
    'End Function


    Private Sub cmdValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim objEx As New AppExcel
        Dim ds As DataSet
        Dim a As New myCollection
        Dim strUploadedPath As String
        ViewState("FilePath") = IIf(cmdBrowse.PostedFile.FileName = "", ViewState("FilePath"), cmdBrowse.PostedFile.FileName)
        lblPath.Text = ViewState("FilePath")

        ds = objEx.ReadExcelFormat(Server.MapPath("Format.xml"), ViewState("FilePath"), a)

        If Not ds Is Nothing Then
            ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))
            If ValidateCol(ds, a) Then

            End If
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
        Else
            Common.NetMsgbox(Me, objEx.Message, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownload.Click
        Dim pro As New Products
        Dim ds As New DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection

        ds = pro.Download_ProductExcel(Session("CompanyIdToken"))
        objEx.Writecell(ds, Server.MapPath("../Template/ItemListLing.xls"))
        Filedownload()
    End Sub
    Function Filedownload()
        Dim strActualFile As String = "ItemList.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/ItemListLing.xls"))
    End Function

    Private Sub cmdDownloadTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        Dim strActualFile As String = "ItemList.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/ItemList.xls"))
    End Sub
End Class
