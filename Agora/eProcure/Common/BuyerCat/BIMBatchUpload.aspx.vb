Imports System.io
Imports eProcure.Component
Imports AgoraLegacy

Imports System.Text.RegularExpressions

Public Class BIMBatchUpload
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String
    Dim objCat As New ContCat
    Dim strDestPath As String = System.Configuration.ConfigurationManager.AppSettings("TemplateTemp")
    Dim strCurVer As String = System.Configuration.ConfigurationManager.AppSettings("BIMExcelVer")

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dg As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dg2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdBrowse As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button
    Protected WithEvents lblPath As System.Web.UI.WebControls.Label
    Protected WithEvents cmdDownload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDownloadTemplate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDownloadTemplateCode As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDownloadTemplateCodePDF As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDownloadUNSPSCGuide As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents tr_dg As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_dg2 As System.Web.UI.HtmlControls.HtmlTableRow
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

        If Not Page.IsPostBack Then
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            objGst = Nothing
            GenerateTab()
        End If

        strCompanyType = objComp.GetCompanyType(Session("CompanyId")) 'HttpContext.Current.Session("CompanyId")
        'If strCompanyType.ToUpper = "VENDOR" Or strCompanyType.ToUpper = "BOTH" Then
        If strCompanyType.ToUpper = "BUYER" Or strCompanyType.ToUpper = "BOTH" Then
            blnPaging = False
            blnSorting = False
            SetGridProperty(dg)
            SetGridProperty(dg2)
            lblPath.Text = ViewState("FilePath")
        Else
            Dim strMsg As String
            Dim objCat As New ContCat
            strMsg = "Can only upload/download Item List for Buyer Company."
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
            '            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            If strTempPath <> "" Then Exit For
            '        End If
            '    Next
            'End If

            Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            If cmdBrowse.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
                Return False
            ElseIf cmdBrowse.PostedFile.ContentLength / 1024 > Session("FileSize") Then ' dblMaxFileSize Then
                Common.NetMsgbox(Me, "File exceeds maximum file size")
                Return False
            End If

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))

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
            strNewFileName = Left(pFileName, Len(pFileName) - 4) &
                             " [" & Format(Now, "ddMMyy-HHmmss") &
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
            Dim strVersion As String
            Dim objGst As New GST
            Dim objCompany As New Companies
            'Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyId"))

            'If dsAppPackage.Tables(0).Rows.Count > 0 Then
            '    Dim i As Integer

            '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
            '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
            '            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            If strTempPath <> "" Or strDestPath <> "" Then Exit For
            '        End If
            '    Next
            'End If

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings("eProcurePath"))

            'strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", "server=127.0.0.1;UID=root;pwd=managedservices;database=eProcure")
            'strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", "server=127.0.0.1;UID=root;pwd=managedservices;database=eProcure")

            If IsExcel(cmdBrowse.PostedFile.FileName) Then
                Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= Session("FileSize") Then
                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    If ViewState("GSTCOD") = True Then
                        ds = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemListBIM.xml"), strTempPath & strFileName, cRules)
                        strVersion = objEx.ReadExcelVersion(Server.MapPath("../xml/ItemListBIM.xml"), strTempPath & strFileName)
                    Else
                        ds = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemListBIMNoGst.xml"), strTempPath & strFileName, cRules)
                        strVersion = objEx.ReadExcelVersion(Server.MapPath("../xml/ItemListBIMNoGst.xml"), strTempPath & strFileName)
                    End If

                    'Compare with current version
                    If CheckCurVer(strVersion) = True Then
                        If Not ds Is Nothing Then
                            ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                            If (Not Directory.Exists(strDestPath)) Then
                                Directory.CreateDirectory(strDestPath)
                            End If
                            File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                            UpdateProduct(ds, cRules)
                            ViewState("FilePath") = ""
                            lblPath.Text = ""
                            If ViewState("GSTCOD") = True Then
                                dg.DataSource = ds.Tables(0)
                                dg.DataBind()
                                tr_dg.Style("display") = ""
                                tr_dg2.Style("display") = "none"
                            Else
                                dg2.DataSource = ds.Tables(0)
                                dg2.DataBind()
                                tr_dg2.Style("display") = ""
                                tr_dg.Style("display") = "none"
                            End If

                            'If ValidateDuplicate(ds) And ValidateCol(ds, cRules, ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")) Then
                            '    'Create the  folder for store the uploaded xls file in server
                            '    If (Not Directory.Exists(strDestPath)) Then
                            '        Directory.CreateDirectory(strDestPath)
                            '    End If
                            '    File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                            '    UpdateProduct(ds)
                            '    ViewState("FilePath") = ""
                            '    lblPath.Text = ""
                            '    dg.DataSource = ds.Tables(0)
                            '    dg.DataBind()
                            'Else
                            '    dg.DataSource = ds.Tables(0)
                            '    dg.DataBind()
                            '    File.Delete(strTempPath & strFileName)
                            'End If
                        Else
                            Common.NetMsgbox(Me, objEx.Message, MsgBoxStyle.Information)
                        End If
                    Else
                        Common.NetMsgbox(Me, "Incorrect Excel File.")
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

    Private Function CheckCurVer(ByVal strVersion As String) As Boolean
        'Check if after GST cut off date and file version is equal to current version
        If ViewState("GSTCOD") = True And strCurVer <> strVersion Then
            Return False
        End If

        'Check if before GST cut off date and file version is not equal to current version
        If ViewState("GSTCOD") = False And strCurVer = strVersion Then
            Return False
        End If

        Return True
    End Function

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
                        dr.Item("Message") &= "<LI type=square>Duplicated Item Code in the same batch.<BR>"
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

                    Dim objDb As EAD.DBCom
                    If pstrConnStr Is Nothing Then
                        objDb = New EAD.DBCom
                    Else
                        objDb = New EAD.DBCom(pstrConnStr)
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
        Dim objDb As EAD.DBCom

        If pstrConnStr Is Nothing Then
            objDb = New EAD.DBCom
        Else
            objDb = New EAD.DBCom(pstrConnStr)
        End If

        'For Each row As DataRow In ds.Tables(0).Rows '.Select(sSelect, "", DataViewRowState.CurrentRows)
        Dim objExProduct As New ExcelBIMProduct_Common
        objExProduct = GetProductDetails(0, row)


        'If Not IsDBNull(objExProduct.Action) Then
        If objExProduct.Action = "N" Then
            If objExProduct.ItemCode = "" Then
                row.Item("Message") &= "<LI type=square>" & "Item Code is required."
                bTrue = False
            Else
                strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProduct.ItemCode) & "'" &
                         " AND PM_S_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "
                If objDb.Exist(strSQL) = 1 Then ' 0= no exist
                    row.Item("Message") &= "<LI type=square>" & "Item code duplicated."
                    bTrue = False
                End If
            End If

            'ElseIf objExProduct.Action = "M" Then
            '    If objExProduct.ItemID = "" Then
            '        row.Item("Message") &= "<LI type=square>" & "Item Id is required."
            '        bTrue = False
            '    Else
            '        strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(objExProduct.ItemID) & "' AND PM_DELETED <> 'Y' "
            '        If objDb.Exist(strSQL) = 0 Then ' 0= no exist
            '            row.Item("Message") &= "<LI type=square>" & "Item Id no found."
            '            bTrue = False
            '        End If
            '    End If

            'ElseIf objExProduct.Action = "D" Then
            '    If IsDBNull(objExProduct.ItemID) Then
            '        row.Item("Message") &= "<LI type=square>" & "Item Id is required."
            '        bTrue = False
            '    Else
            '        strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & Common.Parse(objExProduct.ItemID) & "' AND PM_DELETED <> 'Y' "
            '        If objDb.Exist(strSQL) = 0 Then ' 0= no exist
            '            row.Item("Message") &= "<LI type=square>" & "Invalid Item Id."
            '            bTrue = False
            '        End If

            '        Dim strExist1, strExist2, strExist3 As String
            '        ' check item exists in outstanding PR (status = 1,2,3,4)
            '        strExist1 = "SELECT '*' FROM PR_DETAILS LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(objExProduct.CoyId) & "' "
            '        strExist1 &= "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "' "

            '        ' check item exists in outstanding PO (status = 1,2)
            '        strExist2 = "SELECT '*' FROM PO_DETAILS LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO "
            '        strExist2 &= "WHERE POM_PO_STATUS IN (1,2) AND POD_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "'"

            '        ' check item exists under contract period
            '        strExist3 = "SELECT '*' FROM CONTRACT_DIST_ITEMS LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND CDM_S_COY_ID = '" & Common.Parse(objExProduct.CoyId) & "' "
            '        strExist3 &= "WHERE ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1 ) OR CDM_END_DATE IS NULL) AND CDI_PRODUCT_CODE = '" & Common.Parse(objExProduct.ItemID) & "' "
            '        strExist3 &= "AND CDI_GROUP_INDEX IN (SELECT CDM_GROUP_INDEX FROM CONTRACT_DIST_MSTR) "

            '        If (objDb.Exist(strExist1) > 0) Or (objDb.Exist(strExist2) > 0) Or (objDb.Exist(strExist3) > 0) Then
            '            row.Item("Message") &= "<LI type=square>" & "It has outstanding PR(s)/PO(s)."
            '            bTrue = False
            '        End If

            '        If (objDb.Exist(strExist1) > 0) Or (objDb.Exist(strExist2) > 0) Or (objDb.Exist(strExist3) > 0) Then
            '            row.Item("Message") &= "<LI type=square>" & "It is under contract/discount period."
            '            bTrue = False
            '        End If

            '    End If
        End If
        objExProduct = Nothing

        Return bTrue
    End Function


    Private Function UpdateProduct(ByRef pds As DataSet, ByVal pRules As myCollection)
        'pds.Tables(0).Columns.Add("Status", Type.GetType("System.String"))
        Dim drItem As DataRow
        Dim objProduct As New Products
        Dim pstrConnStr As String

        Dim countSave As Long = 0
        Dim countError As Long = 0

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True

            Dim objExProduct As New ExcelBIMProduct_Common
            objExProduct = GetProductDetails(0, drItem)
            'If ValidateDuplicate(pds) And ValidateCol(pds, cRules, ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")) Then

            'End If

            For Each itema As UploadRule In pRules

                If IsDBNull(drItem(Convert.ToInt16(itema.ColNo))) Then
                    If Not Convert.ToBoolean(itema.AllowNull) Then
                        drItem.Item("Message") &= "<LI type=square>" & itema.ColName & " is required.<BR>"
                        bTrue = False
                    End If
                Else
                    If itema.Regex <> "" And Not Regex.IsMatch(drItem(Convert.ToInt16(itema.ColNo)), itema.Regex) And bTrue Then
                        drItem("Message") &= "<LI type=square>" & itema.RegexErrMsg & "<BR>"
                        bTrue = False
                    End If

                    Dim objDb As EAD.DBCom
                    If pstrConnStr Is Nothing Then
                        objDb = New EAD.DBCom
                    Else
                        objDb = New EAD.DBCom(pstrConnStr)
                    End If

                    Dim strSQL As String
                    Dim i As Integer = 0

                    If itema.SQL.Length > 1 And bTrue Then
                        For i = 0 To itema.SQL.Length - 2
                            strSQL = itema.SQL(i).Query & "'" & Common.Parse(drItem(Convert.ToInt16(itema.ColNo))) & "'"
                            If objDb.GetVal(strSQL) = 0 Then
                                drItem.Item("Message") &= "<LI type=square>" & itema.SQL(i).ErrMsg & "<BR>"
                                bTrue = False
                            End If
                        Next
                    End If
                End If

                If bTrue = False Then
                    bRtn = False
                End If
            Next

            Dim objDb2 As EAD.DBCom
            If pstrConnStr Is Nothing Then
                objDb2 = New EAD.DBCom
            Else
                objDb2 = New EAD.DBCom(pstrConnStr)
            End If

            ' Check COMMODITY_TYPE
            Dim strSQL2 As String
            strSQL2 = "SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_CODE = '" & Common.Parse(objExProduct.CommType) & "' "
            Dim CT_Val As String
            CT_Val = objDb2.GetVal(strSQL2)
            If CT_Val = "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Commodity not found.<BR>"
                bTrue = False
            Else
                Session("CommType") = CT_Val
            End If

            ' Check Pref Vendor
            strSQL2 = "SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(objExProduct.PreferredVen) & "' "
            Dim CM_COY_Val As String
            CM_COY_Val = objDb2.GetVal(strSQL2)
            If CM_COY_Val = "" And Common.Parse(objExProduct.PreferredVen) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Preferred vendor not found.<BR>"
                bTrue = False
            Else
                Session("CM_COY_Val") = CM_COY_Val
            End If

            strSQL2 = "SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(objExProduct.Ven1st) & "' "
            Dim CM_COY_Val1 As String
            CM_COY_Val1 = objDb2.GetVal(strSQL2)
            If CM_COY_Val1 = "" And Common.Parse(objExProduct.Ven1st) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>1st Alternative vendor not found.<BR>"
                bTrue = False
            Else
                Session("CM_COY_Val1") = CM_COY_Val1
            End If

            strSQL2 = "SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(objExProduct.Ven2nd) & "' "
            Dim CM_COY_Val2 As String
            CM_COY_Val2 = objDb2.GetVal(strSQL2)
            If CM_COY_Val2 = "" And Common.Parse(objExProduct.Ven2nd) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>2nd Alternative vendor not found.<BR>"
                bTrue = False
            Else
                Session("CM_COY_Val2") = CM_COY_Val2
            End If

            strSQL2 = "SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(objExProduct.Ven3rd) & "' "
            Dim CM_COY_Val3 As String
            CM_COY_Val3 = objDb2.GetVal(strSQL2)
            If CM_COY_Val3 = "" And Common.Parse(objExProduct.Ven3rd) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>3rd Alternative vendor not found.<BR>"
                bTrue = False
            Else
                Session("CM_COY_Val3") = CM_COY_Val3
            End If

            strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = '" & Common.Parse(objExProduct.AccCode) & "' AND CBG_B_COY_ID = '" & Session("CompanyId") & "' "
            Dim CBG_B_GL_Code As String
            CBG_B_GL_Code = objDb2.GetVal(strSQL2)
            If CBG_B_GL_Code = "" And Common.Parse(objExProduct.AccCode) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Account Code not found.<BR>"
                bTrue = False
            Else
                Session("CBG_B_GL_Code") = CBG_B_GL_Code
            End If

            'Stage 3 Bug fix (GST-0028) - 07/07/2015 - CH
            'Check 'Need QC' whether is YES or NO
            If Common.Parse(objExProduct.QC) <> "" Then
                If (objExProduct.QC.ToUpper <> "YES" And objExProduct.QC.ToUpper <> "NO") Then
                    drItem.Item("Message") &= "<LI type=square>Invalid Need QC/Verification.<BR>"
                    bTrue = False
                End If
            End If

            strSQL2 = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = '" & Common.Parse(objExProduct.Category) & "' AND CBC_B_COY_ID = '" & Session("CompanyId") & "' "
            Dim CBG_B_CAT_Code As String
            CBG_B_CAT_Code = objDb2.GetVal(strSQL2)
            If CBG_B_CAT_Code = "" And Common.Parse(objExProduct.Category) <> "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Category Code not found.<BR>"
                bTrue = False
            Else
                Session("CBG_B_CAT_Code") = CBG_B_CAT_Code
            End If

            '01/11/2013 Chee Hong - check inventory when item type change to SPOT. 
            If objExProduct.ItemType.ToUpper = "SPOT (NON-INVENTORIED ITEM)" Then
                Dim IM_INVENTORY_INDEX As String
                IM_INVENTORY_INDEX = objDb2.GetVal("SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = '" & Common.Parse(objExProduct.ItemCode) & "' AND IM_COY_ID = '" & Session("CompanyId") & "'")

                If IM_INVENTORY_INDEX <> "" Then
                    If objDb2.Exist("SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "'") > 0 Then
                        drItem.Item("Message") &= "<LI type=square>Item Type changes is not allowed. This item has already been added to the inventory.<BR>"
                        bTrue = False
                    End If
                End If
            End If

            If bTrue = False Then
                bRtn = False
            End If

            If bRtn Then
                If ActionValidation(drItem, pRules, pstrConnStr) = False Then
                    bTrue = False
                End If
            End If

            Dim dsProduct As New DataSet

            If objExProduct.Action = "N" Then
                ViewState("mode") = "add"
            ElseIf objExProduct.Action = "M" Or objExProduct.Action = "D" Or objExProduct.Action = "A" Then
                ViewState("mode") = "mod"

                strSQL2 = "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR  "
                strSQL2 &= "WHERE PM_PRODUCT_FOR = 'B' AND PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProduct.ItemCode) & "' AND PM_S_COY_ID = '" & Common.Parse(Session("CompanyId")) & "' "

                Dim ItemCode_Val As String
                ItemCode_Val = objDb2.GetVal(strSQL2)

                If ItemCode_Val = "" Then
                    drItem.Item("Message") &= "<LI type=square>Wrong mode selected.<BR>"
                    bTrue = False
                Else
                    Session("ItemCode_Val") = ItemCode_Val
                End If
            End If
            dsProduct = bindProduct(objExProduct)

            If bTrue = False Then
                bAllTrue = False
            End If

            If bAllTrue = True Then
                Select Case objExProduct.Action
                    Case "N", "M"
                        If objCat.BIM(dsProduct, ViewState("mode"), "", "", Common.Parse(objExProduct.ItemCode), Common.Parse(objExProduct.ItemCode), Common.Parse(objExProduct.ItemCode)) Then
                            'If objProduct.UpdateProdByExcel(objExProduct) Then
                            'If add need to assign back the auto genetrate item no
                            drItem.Item(1) = objExProduct.ItemCode
                            Select Case objExProduct.Action
                                Case "N", "M"
                                    drItem.Item("Message") = "<Font color='#000000'>Item saved.</Font>"
                            End Select
                            countSave = countSave + 1
                        Else
                            Select Case objExProduct.Action
                                Case "N", "M"
                                    drItem.Item("Message") = "Item not saved."
                            End Select
                            countError = countError + 1
                        End If
                    Case "D"
                        Dim dtItem As New DataTable
                        Dim dtr As DataRow
                        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
                        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

                        dtr = dtItem.NewRow()
                        dtr("ProductCode") = Session("ItemCode_Val")
                        dtr("CoyId") = Session("CompanyId")
                        dtItem.Rows.Add(dtr)

                        If objCat.UpdBuyerProductMstr(dtItem, Session("CompanyId"), "Y") Then
                            drItem.Item("Message") = "<Font color='#000000'>Item deactivated.</Font>"
                            countSave = countSave + 1
                        Else
                            drItem.Item("Message") = "Item not deactivated."
                            countError = countError + 1
                        End If

                    Case "A"
                        Dim dtItem As New DataTable
                        Dim dtr As DataRow
                        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
                        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

                        dtr = dtItem.NewRow()
                        dtr("ProductCode") = Session("ItemCode_Val")
                        dtr("CoyId") = Session("CompanyId")
                        dtItem.Rows.Add(dtr)

                        If objCat.UpdBuyerProductMstr(dtItem, Session("CompanyId"), "N") Then
                            drItem.Item("Message") = "<Font color='#000000'>Item activated.</Font>"
                            countSave = countSave + 1
                        Else
                            drItem.Item("Message") = "Item not activated."
                            countError = countError + 1
                        End If
                End Select

            Else
                countError = countError + 1
            End If

            objExProduct = Nothing

            'End If
        Next
        Common.NetMsgbox(Me, "Successful: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Function bindProduct(ByRef pProdDetails As ExcelBIMProduct_Common) As DataSet
        Dim ds As New DataSet
        Dim dtProduct As New DataTable

        dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))   'PM_S_COY_ID
        dtProduct.Columns.Add("ProductCode", Type.GetType("System.String")) 'PM_PRODUCT_CODE
        dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))  'PM_VENDOR_ITEM_CODE
        dtProduct.Columns.Add("ItemName", Type.GetType("System.String"))    'PM_PRODUCT_DESC ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtProduct.Columns.Add("ReferenceNo", Type.GetType("System.String")) 'PM_REF_NO
        dtProduct.Columns.Add("Description", Type.GetType("System.String")) 'PM_LONG_DESC
        dtProduct.Columns.Add("CommodityType", Type.GetType("System.String")) 'PM_CATEGORY_NAME 'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
        dtProduct.Columns.Add("AccCode", Type.GetType("System.String")) 'PM_ACCT_CODE
        dtProduct.Columns.Add("UOM", Type.GetType("System.String")) 'PM_UOM
        dtProduct.Columns.Add("CatCode", Type.GetType("System.String")) 'PM_CAT_CODE
        dtProduct.Columns.Add("MinInv", Type.GetType("System.String")) 'PM_SAFE_QTY
        dtProduct.Columns.Add("Min", Type.GetType("System.String")) 'PM_ORD_MIN_QTY
        dtProduct.Columns.Add("Max", Type.GetType("System.String")) 'PM_ORD_MAX_QTY
        dtProduct.Columns.Add("Brand", Type.GetType("System.String")) 'PM_PRODUCT_BRAND
        dtProduct.Columns.Add("Model", Type.GetType("System.String")) 'PM_PRODUCT_MODEL

        dtProduct.Columns.Add("DrawingNo", Type.GetType("System.String")) 'PM_DRAW_NO
        dtProduct.Columns.Add("VersionNo", Type.GetType("System.String")) 'PM_VERS_NO
        dtProduct.Columns.Add("GrossWeight", Type.GetType("System.String")) 'PM_GROSS_WEIGHT
        dtProduct.Columns.Add("NetWeight", Type.GetType("System.String")) 'PM_NET_WEIGHT
        dtProduct.Columns.Add("Length", Type.GetType("System.String")) 'PM_LENGHT
        dtProduct.Columns.Add("Width", Type.GetType("System.String")) 'PM_WIDTH
        dtProduct.Columns.Add("Height", Type.GetType("System.String")) 'PM_HEIGHT
        dtProduct.Columns.Add("Volume", Type.GetType("System.String")) 'PM_VOLUME
        dtProduct.Columns.Add("ColorInfo", Type.GetType("System.String")) 'PM_COLOR_INFO
        dtProduct.Columns.Add("HSCode", Type.GetType("System.String")) 'PM_HSC_CODE
        dtProduct.Columns.Add("Packing", Type.GetType("System.String")) 'PM_PACKING_REQ
        dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARKS
        dtProduct.Columns.Add("Deleted", Type.GetType("System.String")) 'PM_DELETED
        dtProduct.Columns.Add("Prefer", Type.GetType("System.String"))
        dtProduct.Columns.Add("1st", Type.GetType("System.String"))
        dtProduct.Columns.Add("2nd", Type.GetType("System.String"))
        dtProduct.Columns.Add("3rd", Type.GetType("System.String"))
        dtProduct.Columns.Add("PreferTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("1stTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("2ndTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("3rdTax", Type.GetType("System.String"))

        dtProduct.Columns.Add("MaxInvQty", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu", Type.GetType("System.String"))

        dtProduct.Columns.Add("rd1", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd2", Type.GetType("System.String"))

        dtProduct.Columns.Add("LeadP", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead1", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead3", Type.GetType("System.String"))

        dtProduct.Columns.Add("VenCodeP", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode1", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode2", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode3", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("CoyId") = HttpContext.Current.Session("CompanyId")

        If pProdDetails.Action = "N" Then
            dtr("ProductCode") = ""
        ElseIf pProdDetails.Action = "M" Then
            dtr("ProductCode") = Session("ItemCode_Val")
        End If

        dtr("VendorItemCode") = Common.Parse(pProdDetails.ItemCode)
        dtr("ItemName") = Common.Parse(pProdDetails.ItemName)
        dtr("ReferenceNo") = Common.parseNull(pProdDetails.RefNo)
        dtr("Description") = Common.parseNull(pProdDetails.Description)
        dtr("CommodityType") = Session("CommType")
        dtr("AccCode") = Session("CBG_B_GL_Code") 'Common.parseNull(pProdDetails.AccCode)
        dtr("UOM") = Common.Parse(pProdDetails.UOM)
        dtr("CatCode") = Session("CBG_B_CAT_Code") 'Common.Parse(pProdDetails.Category)

        dtr("Prefer") = Session("CM_COY_Val")
        dtr("1st") = Session("CM_COY_Val1")
        dtr("2nd") = Session("CM_COY_Val2")
        dtr("3rd") = Session("CM_COY_Val3")

        dtr("PreferTax") = Common.parseNull(pProdDetails.PreferredVenGST)
        dtr("1stTax") = Common.parseNull(pProdDetails.Ven1stGST)
        dtr("2ndTax") = Common.parseNull(pProdDetails.Ven2ndGST)
        dtr("3rdTax") = Common.parseNull(pProdDetails.Ven3rdGST)

        If dtr("PreferTax") = "N/A" Or dtr("PreferTax") = "" Then
            dtr("PreferTax") = 1
        Else
            dtr("PreferTax") = Val(dtr("PreferTax")) - 1
        End If

        If dtr("1stTax") = "N/A" Or dtr("1stTax") = "" Then
            dtr("1stTax") = 1
        Else
            dtr("1stTax") = Val(dtr("1stTax")) - 1
        End If

        If dtr("2ndTax") = "N/A" Or dtr("2ndTax") = "" Then
            dtr("2ndTax") = 1
        Else
            dtr("2ndTax") = Val(dtr("2ndTax")) - 1
        End If

        If dtr("3rdTax") = "N/A" Or dtr("3rdTax") = "" Then
            dtr("3rdTax") = 1
        Else
            dtr("3rdTax") = Val(dtr("3rdTax")) - 1
        End If

        dtr("Manu") = Common.parseNull(pProdDetails.ManuName)

        If pProdDetails.ItemType.ToUpper = "SPOT (NON-INVENTORIED ITEM)" Then
            dtr("rd1") = "SP"
            dtr("rd2") = "N"
        ElseIf pProdDetails.ItemType.ToUpper = "STOCK (DIRECT MATERIAL - INVENTORIED ITEM)" Then
            dtr("rd1") = "ST"
            If pProdDetails.QC.ToUpper = "YES" Then
                dtr("rd2") = "Y"
            ElseIf pProdDetails.QC.ToUpper = "NO" Then
                dtr("rd2") = "N"
            End If
        ElseIf pProdDetails.ItemType.ToUpper = "MRO, M&E AND IT (INVENTORIED ITEM)" Then
            dtr("rd1") = "MI"
            If pProdDetails.QC.ToUpper = "YES" Then
                dtr("rd2") = "Y"
            ElseIf pProdDetails.QC.ToUpper = "NO" Then
                dtr("rd2") = "N"
            End If
        End If

        dtr("LeadP") = Common.parseNull(pProdDetails.PreferredVenLead)
        dtr("Lead1") = Common.parseNull(pProdDetails.Ven1stLead)
        dtr("Lead2") = Common.parseNull(pProdDetails.Ven2ndLead)
        dtr("Lead3") = Common.parseNull(pProdDetails.Ven3rdLead)

        dtr("VenCodeP") = Common.parseNull(pProdDetails.PreferredVenItemCode)
        dtr("VenCode1") = Common.parseNull(pProdDetails.Ven1stItemCode)
        dtr("VenCode2") = Common.parseNull(pProdDetails.Ven2ndItemCode)
        dtr("VenCode3") = Common.parseNull(pProdDetails.Ven3rdItemCode)

        dtr("MinInv") = pProdDetails.SafetyLevelMin
        dtr("Min") = pProdDetails.OrderQtyMin
        dtr("Max") = pProdDetails.OrderQtyMax
        dtr("MaxInvQty") = pProdDetails.MaxInvQty

        'If pProdDetails.SafetyLevelMin = "" Then
        '    dtr("MinInv") = 0
        'Else
        '    dtr("MinInv") = pProdDetails.SafetyLevelMin
        'End If

        'If pProdDetails.OrderQtyMin = "" Then
        '    dtr("Min") = 0
        'Else
        '    dtr("Min") = pProdDetails.OrderQtyMin
        'End If

        'If pProdDetails.OrderQtyMax = "" Then
        '    dtr("Max") = 0
        'Else
        '    dtr("Max") = pProdDetails.OrderQtyMax
        'End If

        'If pProdDetails.MaxInvQty = "" Then
        '    dtr("MaxInvQty") = 0
        'Else
        '    dtr("MaxInvQty") = pProdDetails.MaxInvQty
        'End If

        dtr("Brand") = Common.parseNull(pProdDetails.Brand)
        dtr("Model") = Common.parseNull(pProdDetails.Model)
        dtr("DrawingNo") = Common.parseNull(pProdDetails.DrawingNo)
        dtr("VersionNo") = Common.parseNull(pProdDetails.VerNo)
        dtr("GrossWeight") = Common.parseNull(pProdDetails.GrossWeight)
        dtr("NetWeight") = Common.parseNull(pProdDetails.NetWeight)
        dtr("Length") = Common.parseNull(pProdDetails.Length)
        dtr("Width") = Common.parseNull(pProdDetails.Width)
        dtr("Height") = Common.parseNull(pProdDetails.Height)
        dtr("Volume") = Common.parseNull(pProdDetails.Volume)
        dtr("ColorInfo") = Common.parseNull(pProdDetails.ColorInfo)
        dtr("HSCode") = Common.parseNull(pProdDetails.HSCode)
        dtr("Packing") = Common.parseNull(pProdDetails.PackSpec)
        dtr("Remark") = Common.parseNull(pProdDetails.Remarks)

        dtr("Deleted") = "N"

        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)
        bindProduct = ds
    End Function

    Private Function GetProductDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelBIMProduct_Common
        Dim objExcel As New ExcelBIMProduct_Common

        'If pds.Tables.Count > 0 Then
        'If pds.Tables(0).Rows.Count >= iRow Then
        With objExcel
            objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
            objExcel.ItemCode = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
            objExcel.ItemName = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
            objExcel.ItemType = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
            objExcel.Description = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
            objExcel.CommType = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))
            objExcel.UOM = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
            objExcel.Category = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
            objExcel.RefNo = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
            objExcel.QC = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
            objExcel.AccCode = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
            objExcel.OrderQtyMin = IIf(IsDBNull(pdr.Item("F12")), 0, pdr.Item("F12"))
            objExcel.OrderQtyMax = IIf(IsDBNull(pdr.Item("F13")), 0, pdr.Item("F13"))
            objExcel.SafetyLevelMin = IIf(IsDBNull(pdr.Item("F14")), 0, pdr.Item("F14"))
            objExcel.MaxInvQty = IIf(IsDBNull(pdr.Item("F15")), 0, pdr.Item("F15"))
            objExcel.Brand = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
            objExcel.ManuName = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
            objExcel.DrawingNo = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))
            objExcel.Model = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))
            objExcel.GrossWeight = IIf(IsDBNull(pdr.Item("F20")), "", pdr.Item("F20"))
            objExcel.NetWeight = IIf(IsDBNull(pdr.Item("F21")), "", pdr.Item("F21"))
            objExcel.Length = IIf(IsDBNull(pdr.Item("F22")), "", pdr.Item("F22"))
            objExcel.VerNo = IIf(IsDBNull(pdr.Item("F23")), "", pdr.Item("F23"))
            objExcel.PackSpec = IIf(IsDBNull(pdr.Item("F24")), "", pdr.Item("F24"))
            objExcel.Width = IIf(IsDBNull(pdr.Item("F25")), "", pdr.Item("F25"))
            objExcel.ColorInfo = IIf(IsDBNull(pdr.Item("F26")), "", pdr.Item("F26"))
            objExcel.Volume = IIf(IsDBNull(pdr.Item("F27")), "", pdr.Item("F27"))
            objExcel.HSCode = IIf(IsDBNull(pdr.Item("F28")), "", pdr.Item("F28"))
            objExcel.Height = IIf(IsDBNull(pdr.Item("F29")), "", pdr.Item("F29"))
            objExcel.Remarks = IIf(IsDBNull(pdr.Item("F30")), "", pdr.Item("F30"))
            objExcel.PreferredVen = IIf(IsDBNull(pdr.Item("F31")), "", pdr.Item("F31"))
            objExcel.Ven1st = IIf(IsDBNull(pdr.Item("F32")), "", pdr.Item("F32"))
            objExcel.Ven2nd = IIf(IsDBNull(pdr.Item("F33")), "", pdr.Item("F33"))
            objExcel.Ven3rd = IIf(IsDBNull(pdr.Item("F34")), "", pdr.Item("F34"))
            If ViewState("GSTCOD") = True Then
                objExcel.PreferredVenGST = ""
                objExcel.Ven1stGST = ""
                objExcel.Ven2ndGST = ""
                objExcel.Ven3rdGST = ""
                objExcel.PreferredVenLead = IIf(IsDBNull(pdr.Item("F35")), "", pdr.Item("F35"))
                objExcel.Ven1stLead = IIf(IsDBNull(pdr.Item("F36")), "", pdr.Item("F36"))
                objExcel.Ven2ndLead = IIf(IsDBNull(pdr.Item("F37")), "", pdr.Item("F37"))
                objExcel.Ven3rdLead = IIf(IsDBNull(pdr.Item("F38")), "", pdr.Item("F38"))
                objExcel.PreferredVenItemCode = IIf(IsDBNull(pdr.Item("F39")), "", pdr.Item("F39"))
                objExcel.Ven1stItemCode = IIf(IsDBNull(pdr.Item("F40")), "", pdr.Item("F40"))
                objExcel.Ven2ndItemCode = IIf(IsDBNull(pdr.Item("F41")), "", pdr.Item("F41"))
                objExcel.Ven3rdItemCode = IIf(IsDBNull(pdr.Item("F42")), "", pdr.Item("F42"))
                objExcel.Action = IIf(IsDBNull(pdr.Item("F43")), "", pdr.Item("F43"))
            Else
                objExcel.PreferredVenGST = IIf(IsDBNull(pdr.Item("F35")), "", pdr.Item("F35"))
                objExcel.Ven1stGST = IIf(IsDBNull(pdr.Item("F36")), "", pdr.Item("F36"))
                objExcel.Ven2ndGST = IIf(IsDBNull(pdr.Item("F37")), "", pdr.Item("F37"))
                objExcel.Ven3rdGST = IIf(IsDBNull(pdr.Item("F38")), "", pdr.Item("F38"))
                objExcel.PreferredVenLead = IIf(IsDBNull(pdr.Item("F39")), "", pdr.Item("F39"))
                objExcel.Ven1stLead = IIf(IsDBNull(pdr.Item("F40")), "", pdr.Item("F40"))
                objExcel.Ven2ndLead = IIf(IsDBNull(pdr.Item("F41")), "", pdr.Item("F41"))
                objExcel.Ven3rdLead = IIf(IsDBNull(pdr.Item("F42")), "", pdr.Item("F42"))
                objExcel.PreferredVenItemCode = IIf(IsDBNull(pdr.Item("F43")), "", pdr.Item("F43"))
                objExcel.Ven1stItemCode = IIf(IsDBNull(pdr.Item("F44")), "", pdr.Item("F44"))
                objExcel.Ven2ndItemCode = IIf(IsDBNull(pdr.Item("F45")), "", pdr.Item("F45"))
                objExcel.Ven3rdItemCode = IIf(IsDBNull(pdr.Item("F46")), "", pdr.Item("F46"))
                objExcel.Action = IIf(IsDBNull(pdr.Item("F47")), "", pdr.Item("F47"))
            End If
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
        Dim strFileName As String = "ItemBIMListing_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

        ds = pro.Download_ProductExcel_Common(Session("CompanyId"))
        objEx.Writecell_Common(ds, strDestPath & strFileName)
        objEx.ProtectWorkSheet_ItemUpload(strDestPath & strFileName)
        Filedownload(strFileName)
    End Sub
    Function Filedownload(ByVal strFile As String)
        'Dim strActualFile As String = "ItemBIMListing.xls"
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(Server.MapPath("../Template/ItemBIMListing.xls"))

        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strFile & """")
        Response.Flush()
        Response.WriteFile(strDestPath & strFile)
    End Function

    Private Sub cmdDownloadTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        Dim strActualFile As String = "ItemBIMTemplate.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/ItemBIMTemplate.xls"))

    End Sub

    Private Sub cmdDownloadTemplateCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplateCode.Click
        Dim strActualFile As String = "UNSPSC v13.1201.xlsx"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/UNSPSC v13.1201.xlsx"))
    End Sub

    Private Sub cmdDownloadTemplateCodePDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplateCodePDF.Click
        Dim strActualFile As String = "UNSPSC_English_v13.1201-3.pdf"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/UNSPSC_English_v13.1201-3.pdf"))
    End Sub

    Private Sub cmdDownloadUNSPSCGuide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadUNSPSCGuide.Click
        Dim strActualFile As String = "GUIDE TO UNSPSC (SEGMENT or CATEGORY BY GOODS & SERVICES TYPE).pdf"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/GUIDE TO UNSPSC (SEGMENT or CATEGORY BY GOODS & SERVICES TYPE).pdf"))
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_BIM_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId) & """><span>Item Listing</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "BIMBatchUpload.aspx", "pageid=" & strPageId) & """><span>Item Batch Upload/Download</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
						  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BIMAuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
    End Sub
End Class
