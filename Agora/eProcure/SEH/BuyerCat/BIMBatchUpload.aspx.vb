Imports System.io
Imports eProcure.Component

Imports AgoraLegacy

Imports System.Text.RegularExpressions
Imports Microsoft.Reporting.WebForms
Imports MySql.Data.MySqlClient

Public Class BIMBatchUploadSEH
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String
    Dim objCat As New ContCat
    Dim objCat_Ext As New ContCat_Ext
    Dim objEx_Ext As New AppExcel_Ext
    Dim objEx As New AppExcel
    Dim pro As New Products
    Dim Product_Ext As New Products_Ext
    Dim dispatcher As New dispatcher
    Dim strDestPath As String = System.Configuration.ConfigurationManager.AppSettings("TemplateTemp")
    Dim strCurVer As String = System.Configuration.ConfigurationManager.AppSettings("BIMExcelVer")

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
    Protected WithEvents cmdDownloadTemplateCode As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDownloadTemplateCodePDF As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDownloadUNSPSCGuide As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents lblReportType As System.Web.UI.WebControls.Label
    Protected WithEvents cboReportType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rd1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents dg2 As System.Web.UI.WebControls.DataGrid

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
            If Session("UserStkType") Is Nothing Then
                UserStkTypeCheck()
            End If
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

        'cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewBudget.aspx", "&CoyID=" & Session("CompanyID") & "") & "')")

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
            Dim objEx_Ext As New AppExcel_Ext
            Dim ds As DataSet
            Dim ds3 As DataSet
            Dim ds2 As DataSet
            Dim cRules As New myCollection
            Dim cRules2 As New myCollection
            Dim objFileMgmt As New FileManagement
            Dim strFileName As String = Path.GetFileName(cmdBrowse.PostedFile.FileName)
            Dim strTempPath As String
            Dim strDestPath As String
            Dim strVersion As String
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

                    If rd1.SelectedItem.Value = "I" Then
                        ds = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemListBIM.xml"), strTempPath & strFileName, cRules)

                        If ViewState("GSTCOD") = True Then
                            ds3 = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemListBIMVendor.xml"), strTempPath & strFileName, cRules2)
                            strVersion = objEx.ReadExcelVersion(Server.MapPath("../xml/ItemListBIMVendor.xml"), strTempPath & strFileName)
                        Else
                            ds3 = objEx.ReadExcelFormat(Server.MapPath("../xml/ItemListBIMVendorNoGst.xml"), strTempPath & strFileName, cRules2)
                            strVersion = objEx.ReadExcelVersion(Server.MapPath("../xml/ItemListBIMVendorNoGst.xml"), strTempPath & strFileName)
                        End If

                        'Compare with current version
                        If CheckCurVer(strVersion) = True Then
                            If Not ds Is Nothing Then
                                ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                                If (Not Directory.Exists(strDestPath)) Then
                                    Directory.CreateDirectory(strDestPath)
                                End If

                                File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                                UpdateProduct(ds, ds3, cRules, cRules2)
                                ViewState("FilePath") = ""
                                lblPath.Text = ""
                                RemoveRowDs(ds)
                                dg.DataSource = ds.Tables(0)
                                dg.DataBind()
                            Else
                                Common.NetMsgbox(Me, objEx.Message, MsgBoxStyle.Information)
                            End If

                            dg.Visible = True
                            dg2.Visible = False
                        Else
                            Common.NetMsgbox(Me, "Incorrect Excel File.")
                        End If
                    Else
                        ds2 = objEx_Ext.ReadExcelFormatForBudget(Server.MapPath("../xml/ItemListBIMBudget.xml"), strTempPath & strFileName, cRules)

                        If Not ds2 Is Nothing Then
                            ds2.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                            If (Not Directory.Exists(strDestPath)) Then
                                Directory.CreateDirectory(strDestPath)
                            End If

                            File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                            UpdateProductBudget(ds2, cRules)
                            ViewState("FilePath") = ""
                            lblPath.Text = ""
                            dg2.DataSource = ds2.Tables(0)
                            dg2.DataBind()
                        Else
                            Common.NetMsgbox(Me, objEx.Message, MsgBoxStyle.Information)
                        End If

                        dg.Visible = False
                        dg2.Visible = True
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
        Dim objExProduct As New ExcelBIMProduct_CommonNew_Ext
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


    Private Function UpdateProduct(ByRef pds As DataSet, ByRef pds2 As DataSet, ByVal pRules As myCollection, ByVal pRules2 As myCollection)
        'pds.Tables(0).Columns.Add("Status", Type.GetType("System.String"))
        Dim drItem As DataRow
        Dim objProduct As New Products
        Dim pstrConnStr As String
        Dim aryVendor, aryPrice, aryTempPrice As New ArrayList
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim i, j, k As Integer
        Dim strPrice(), strPrice2(), strTemp As String
        Dim objDb3 As New EAD.DBCom

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True


            If Not IsDBNull(drItem(1)) Or Not IsDBNull(drItem(2)) Or Not IsDBNull(drItem(3)) Or Not IsDBNull(drItem(5)) Or Not IsDBNull(drItem(6)) Or Not IsDBNull(drItem(9)) Or Not IsDBNull(drItem(21)) Or Not IsDBNull(drItem(46)) Then


                Dim objExProduct As New ExcelBIMProduct_CommonNew_Ext
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
                        i = 0

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
                If Common.parseNull(objExProduct.CommType) <> "" Then
                    strSQL2 = "SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(objExProduct.CommType) & "' "
                    Dim CT_Val As String
                    CT_Val = objDb2.GetVal(strSQL2)
                    If CT_Val = "" Then 'no exist
                        drItem.Item("Message") &= "<LI type=square>Commodity type not found.<BR>"
                        bTrue = False
                    Else
                        Session("CommType") = CT_Val
                    End If
                End If

                'Check Account Code if Item type is Inventoried Item
                If objExProduct.ItemType.ToUpper <> "" Then
                    If objExProduct.ItemType.ToUpper <> "SPOT (NON-INVENTORIED ITEM)" And Common.Parse(objExProduct.AccCode) = "" Then
                        drItem.Item("Message") &= "<LI type=square>Account Code is required.<BR>"
                        bTrue = False
                    End If
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

                strSQL2 = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE CBC_B_CATEGORY_CODE = '" & Common.Parse(objExProduct.Category) & "' AND CBC_B_COY_ID = '" & Session("CompanyId") & "' "
                Dim CBG_B_CAT_Code As String
                CBG_B_CAT_Code = objDb2.GetVal(strSQL2)
                If CBG_B_CAT_Code = "" And Common.Parse(objExProduct.Category) <> "" Then 'no exist
                    drItem.Item("Message") &= "<LI type=square>Category Code not found.<BR>"
                    bTrue = False
                Else
                    Session("CBG_B_CAT_Code") = CBG_B_CAT_Code
                End If

                strSQL2 = "SELECT CPT_PACK_CODE FROM (SELECT CPT_PACK_CODE, CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')') AS PACK_TYPE FROM COMPANY_PACKING_TYPE WHERE CPT_COY_ID = '" & Session("CompanyId") & "') tb WHERE PACK_TYPE = '" & Common.Parse(objExProduct.PackType) & "'"
                Dim CPT_PACK_CODE As String
                CPT_PACK_CODE = objDb2.GetVal(strSQL2)
                If CPT_PACK_CODE = "" And Common.Parse(objExProduct.PackType) <> "" Then 'no exist
                    drItem.Item("Message") &= "<LI type=square>Packing Type not found.<BR>"
                    bTrue = False
                Else
                    Session("CPT_PACK_CODE") = CPT_PACK_CODE
                End If

                'Stage 3 Bug fix (GST-0028) - 08/07/2015 - CH
                'Check 'Need QC' whether is YES or NO
                If Common.Parse(objExProduct.QC) <> "" Then
                    If (objExProduct.QC.ToUpper <> "YES" And objExProduct.QC.ToUpper <> "NO") Then
                        drItem.Item("Message") &= "<LI type=square>Invalid Need QC/Verification.<BR>"
                        bTrue = False
                    End If
                End If

                If objExProduct.ItemType.ToUpper = "STOCK (DIRECT MATERIAL - INVENTORIED ITEM)" Then
                    If objExProduct.QC.ToUpper = "YES" And Common.parseNull(objExProduct.IQC) = "" Then
                        drItem.Item("Message") &= "<LI type=square>IQC Test Type is required.<BR>"
                        bTrue = False
                    ElseIf objExProduct.QC.ToUpper = "NO" And Common.parseNull(objExProduct.IQC) <> "" Then
                        drItem.Item("Message") &= "<LI type=square>No IQC Test Type is allowed if Need QC/Verification is set to NO.<BR>"
                        bTrue = False
                    End If
                End If

                'No need check item type: Requested by Lee Ling on 02/10/2013
                'If objExProduct.ItemType.ToUpper = "SPOT (NON-INVENTORIED ITEM)" Then
                '    strTemp = objDb2.GetVal("SELECT UM_STK_TYPE_SPOT FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "'")
                '    If strTemp = "N" Then
                '        drItem.Item("Message") &= "<LI type=square>User has no access to (SPOT (NON-INVENTORIED ITEM)) item type.<BR>"
                '        bTrue = False
                '    End If
                'ElseIf objExProduct.ItemType.ToUpper = "STOCK (DIRECT MATERIAL - INVENTORIED ITEM)" Then
                '    strTemp = objDb2.GetVal("SELECT UM_STK_TYPE_STOCK FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "'")
                '    If strTemp = "N" Then
                '        drItem.Item("Message") &= "<LI type=square>User has no access to (STOCK (DIRECT MATERIAL - INVENTORIED ITEM)) item type.<BR>"
                '        bTrue = False
                '    End If
                'ElseIf objExProduct.ItemType.ToUpper = "MRO, M&E AND IT (INVENTORIED ITEM)" Then
                '    strTemp = objDb2.GetVal("SELECT UM_STK_TYPE_MRO FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "'")
                '    If strTemp = "N" Then
                '        drItem.Item("Message") &= "<LI type=square>User has no access to (MRO, M&E AND IT (INVENTORIED ITEM)) item type.<BR>"
                '        bTrue = False
                '    End If
                'End If

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
                    strSQL2 &= "WHERE PM_PRODUCT_FOR = 'B' AND PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProduct.ItemCode) & "' AND PM_S_COY_ID = '" & Common.Parse(Session("CompanyId")) & "'"

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

                'Get Vendor Details
                aryVendor.Clear()
                aryPrice.Clear()

                j = 0

                If Not pds2 Is Nothing Then
                    For i = 0 To pds2.Tables(0).Rows.Count - 1
                        If objExProduct.ItemCode = Common.parseNull(pds2.Tables(0).Rows(i)(1)) And Common.parseNull(pds2.Tables(0).Rows(i)(2)) <> "" Then

                            If j = 0 Then
                                'aryVendor.Add(New String() {"P", pds2.Tables(0).Rows(i)(1), pds2.Tables(0).Rows(i)(2), IsDBNull(pds2.Tables(0).Rows(i)(3)), IsDBNull(pds2.Tables(0).Rows(i)(4)), IsDBNull(pds2.Tables(0).Rows(i)(5)), IsDBNull(pds2.Tables(0).Rows(i)(6)), IsDBNull(pds2.Tables(0).Rows(i)(7)), IsDBNull(pds2.Tables(0).Rows(i)(8)), IsDBNull(pds2.Tables(0).Rows(i)(9)), IsDBNull(pds2.Tables(0).Rows(i)(10)), IsDBNull(pds2.Tables(0).Rows(i)(11)), IsDBNull(pds2.Tables(0).Rows(i)(12))})
                                'aryVendor.Add(New String() {"P", "", "", "", "", "", "", "", "", "", "", "", ""})
                                aryVendor.Add(New String() {"", "P", "", "", "", "", "", "", "", "", "", "", ""})
                            Else
                                'aryVendor.Add(New String() {j, "", "", "", "", "", "", "", "", "", "", "", ""})
                                aryVendor.Add(New String() {"", j, "", "", "", "", "", "", "", "", "", "", ""})
                                'aryVendor.Add(New String() {j, pds2.Tables(0).Rows(i)(1), pds2.Tables(0).Rows(i)(2), IsDBNull(pds2.Tables(0).Rows(i)(3)), IsDBNull(pds2.Tables(0).Rows(i)(4)), IsDBNull(pds2.Tables(0).Rows(i)(5)), IsDBNull(pds2.Tables(0).Rows(i)(6)), IsDBNull(pds2.Tables(0).Rows(i)(7)), IsDBNull(pds2.Tables(0).Rows(i)(8)), IsDBNull(pds2.Tables(0).Rows(i)(9)), IsDBNull(pds2.Tables(0).Rows(i)(10)), IsDBNull(pds2.Tables(0).Rows(i)(11)), IsDBNull(pds2.Tables(0).Rows(i)(12))})
                            End If

                            aryVendor(j)(2) = Common.Parse(pds2.Tables(0).Rows(i)(2)) 'Company Name
                            aryVendor(j)(3) = objDb3.GetVal("SELECT CM_COY_ID FROM company_mstr WHERE CM_COY_NAME = '" & Common.Parse(pds2.Tables(0).Rows(i)(2)) & "'") 'Company ID
                            aryVendor(j)(4) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(3)), "", pds2.Tables(0).Rows(i)(3)) 'Supp Code

                            If IsDBNull(pds2.Tables(0).Rows(i)(4)) Then 'Delivery Term
                                aryVendor(j)(5) = ""
                            Else
                                strTemp = "SELECT CDT_DEL_CODE FROM " &
                                       "(SELECT CDT_DEL_CODE, CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')') AS DEL_TERM FROM COMPANY_DELIVERY_TERM " &
                                       "WHERE CDT_COY_ID = '" & Session("CompanyId") & "') tb WHERE DEL_TERM = '" & Common.Parse(pds2.Tables(0).Rows(i)(4)) & "'"
                                aryVendor(j)(5) = objDb2.GetVal(strTemp)
                            End If

                            'aryVendor(j)(5) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(4)), "", pds2.Tables(0).Rows(i)(4)) 'Delivery Term

                            strTemp = IIf(IsDBNull(pds2.Tables(0).Rows(i)(5)), "", pds2.Tables(0).Rows(i)(5))

                            'Split Unit Price
                            If strTemp <> "" Then
                                aryTempPrice.Clear()

                                strPrice = Split(strTemp, ",")
                                'strAryPrice = strAryPrice
                                For k = 0 To strPrice.Length - 1
                                    strPrice2 = Split(strPrice(k), ":")

                                    If strPrice2.Length = 2 Then
                                        If strPrice2(0) <> "" And strPrice2(1) <> "" Then
                                            If IsNumeric(strPrice2(0)) And IsNumeric(strPrice2(1)) Then
                                                aryPrice.Add(New String() {strPrice2(0), strPrice2(1), "", aryVendor(j)(1)})
                                                aryTempPrice.Add(New String() {strPrice2(0), strPrice2(1)})
                                            Else
                                                drItem.Item("Message") &= "<LI type=square>" & pds2.Tables(0).Rows(i)(2) & "'s unit price is invalid.<BR>"
                                                bTrue = False
                                                Exit For
                                            End If
                                        Else
                                            drItem.Item("Message") &= "<LI type=square>" & pds2.Tables(0).Rows(i)(2) & "'s unit price is invalid.<BR>"
                                            bTrue = False
                                            Exit For
                                        End If
                                    Else
                                        drItem.Item("Message") &= "<LI type=square>" & pds2.Tables(0).Rows(i)(2) & "'s unit price is invalid.<BR>"
                                        bTrue = False
                                        Exit For
                                    End If

                                Next

                                If Not aryTempPrice Is Nothing Then
                                    For k = 0 To aryTempPrice.Count - 1
                                        Dim c As Integer
                                        For c = 0 To aryTempPrice.Count - 1
                                            If k > c Then
                                                If CDbl(aryTempPrice(c)(0)) > CDbl(aryTempPrice(k)(0)) Then
                                                    drItem.Item("Message") &= "<LI type=square>The next volume quantity must be greater than the previous quantity.<BR>"
                                                    bTrue = False
                                                End If

                                            End If
                                        Next

                                    Next
                                End If

                            End If

                            strTemp = IIf(IsDBNull(pds2.Tables(0).Rows(i)(6)), "", pds2.Tables(0).Rows(i)(6)) 'Payment Term
                            aryVendor(j)(6) = objDb3.GetVal("SELECT CODE_ABBR FROM CODE_MSTR WHERE CODE_DESC = '" & strTemp & "' AND CODE_CATEGORY = 'PT'")

                            strTemp = IIf(IsDBNull(pds2.Tables(0).Rows(i)(7)), "", pds2.Tables(0).Rows(i)(7)) 'Currency
                            aryVendor(j)(7) = objDb3.GetVal("SELECT CODE_ABBR FROM CODE_MSTR WHERE CODE_DESC = '" & strTemp & "' AND CODE_CATEGORY = 'CU'")

                            aryVendor(j)(8) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(8)), "", pds2.Tables(0).Rows(i)(8)) 'Purchase Spec No
                            aryVendor(j)(9) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(9)), "", pds2.Tables(0).Rows(i)(9)) 'Revision

                            If ViewState("GSTCOD") = True Then
                                aryVendor(j)(10) = ""
                                aryVendor(j)(11) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(10)), "", pds2.Tables(0).Rows(i)(10)) 'Lead Time
                                aryVendor(j)(12) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(11)), "", pds2.Tables(0).Rows(i)(11)) 'Vendor Item Code
                            Else
                                strTemp = IIf(IsDBNull(pds2.Tables(0).Rows(i)(10)), "N/A", pds2.Tables(0).Rows(i)(10)) 'Tax
                                aryVendor(j)(10) = objDb3.GetVal("SELECT TAX_AUTO_NO FROM TAX WHERE TAX_PERC = '" & strTemp & "'")
                                aryVendor(j)(11) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(10)), "", pds2.Tables(0).Rows(i)(11)) 'Lead Time
                                aryVendor(j)(12) = IIf(IsDBNull(pds2.Tables(0).Rows(i)(11)), "", pds2.Tables(0).Rows(i)(12)) 'Vendor Item Code
                            End If

                            j = j + 1
                        End If
                    Next

                    If aryVendor.Count > 0 Then
                        If objExProduct.ItemType.ToUpper <> "STOCK (DIRECT MATERIAL - INVENTORIED ITEM)" Then
                            If aryVendor.Count > 4 Then
                                drItem.Item("Message") &= "<LI type=square>More than 4 vendors entered for spot item is not allowed.<BR>"
                                bTrue = False
                            End If

                        End If

                        For i = 0 To aryVendor.Count - 1

                            'Check Pref Vendor
                            strSQL2 = "SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(aryVendor(i)(2)) & "' "
                            Dim CM_COY_Val As String
                            CM_COY_Val = objDb2.GetVal(strSQL2)
                            If CM_COY_Val = "" And Common.Parse(aryVendor(i)(2)) <> "" Then 'no exist
                                drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") not found.<BR>"
                                bTrue = False
                            Else
                                If aryVendor(i)(4) <> "" And aryVendor(i)(7) <> "" Then
                                    'Check from table whether vendor code / currency is duplicated or not
                                    Dim dsTemp As New DataSet
                                    Dim dsTemp2 As New DataSet

                                    strTemp = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " &
                                            "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_SUPP_CODE = '" & Common.Parse(aryVendor(i)(4)) & "' AND PV_S_COY_ID = '" & Common.Parse(CM_COY_Val) & "' "
                                    dsTemp = objDb2.FillDs(strTemp)

                                    If dsTemp.Tables(0).Rows.Count > 0 Then
                                        If aryVendor(i)(7) <> dsTemp.Tables(0).Rows(0)("PV_CURR") Then
                                            drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Vendor Code is invalid.<BR>"
                                            bTrue = False
                                        End If
                                    Else
                                        strTemp = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " &
                                                "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_CURR = '" & aryVendor(i)(7) & "' AND PV_S_COY_ID = '" & Common.Parse(CM_COY_Val) & "' "
                                        dsTemp2 = objDb2.FillDs(strTemp)

                                        If dsTemp2.Tables(0).Rows.Count > 0 Then
                                            drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Currency Code is invalid.<BR>"
                                            bTrue = False
                                        End If
                                    End If

                                Else
                                    'Check Supplier Code
                                    If aryVendor(i)(4) = "" Then
                                        drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Vendor Code is required.<BR>"
                                        bTrue = False
                                    End If

                                    'Check Currency
                                    If aryVendor(i)(7) = "" Then
                                        drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Currency Code is required.<BR>"
                                        bTrue = False
                                    End If
                                End If
                            End If

                            'Check Lead Time
                            If aryVendor(i)(11) <> "" Then
                                If IsNumeric(aryVendor(i)(11)) And Not Regex.IsMatch(Trim(aryVendor(i)(11)), "^\d+$") Then
                                    drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Lead Time is expecting integer number.<BR>"
                                    bTrue = False
                                End If
                            End If

                            'Check Delivery Term
                            If aryVendor(i)(5) <> "" Then
                                strSQL2 = "SELECT CDT_DEL_CODE FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Session("CompanyId") & "' AND CDT_DEL_CODE = '" & Common.Parse(aryVendor(i)(5)) & "'"
                                Dim CDT_DEL_CODE As String
                                CDT_DEL_CODE = objDb2.GetVal(strSQL2)
                                If CDT_DEL_CODE = "" Then
                                    drItem.Item("Message") &= "<LI type=square>Vendor (" & aryVendor(i)(2) & ") Delivery Term not found.<BR>"
                                    bTrue = False
                                End If
                            End If

                        Next

                    End If
                End If


                If bTrue = False Then
                    bAllTrue = False
                End If

                If bAllTrue = True Then
                    Select Case objExProduct.Action
                        Case "N", "M"
                            If objCat_Ext.BIM(dsProduct, ViewState("mode"), "", "", Common.Parse(objExProduct.ItemCode), aryVendor, Common.Parse(objExProduct.ItemCode), Common.Parse(objExProduct.ItemCode), , aryPrice, ViewState("GSTCOD")) Then
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

            End If
        Next
        Common.NetMsgbox(Me, "Successful: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Function UpdateProductBudget(ByRef dsBudget As DataSet, ByVal pRules As myCollection)
        'pds.Tables(0).Columns.Add("Status", Type.GetType("System.String"))
        Dim drItem As DataRow
        Dim objProduct As New Products
        Dim pstrConnStr As String
        Dim objCat_Ext As New ContCat_Ext

        Dim countSave As Long = 0
        Dim countError As Long = 0

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")

        For Each drItem In dsBudget.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True

            Dim objExProBP As New ExcelBIMUnitPrice_Common_Ext
            objExProBP = GetUnitPriceDetails(0, drItem)

            For Each itema As UploadRule In pRules
                'If itema.ColNo <> "1" Then
                If IsDBNull(drItem(Convert.ToInt16(itema.ColNo))) Then
                    If Not Convert.ToBoolean(itema.AllowNull) Then
                        drItem.Item("Message") &= "<LI type=square>" & itema.ColName & " is empty.<BR>"
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
                'End If

            Next

            Dim objDb2 As EAD.DBCom
            If pstrConnStr Is Nothing Then
                objDb2 = New EAD.DBCom
            Else
                objDb2 = New EAD.DBCom(pstrConnStr)
            End If

            ' Check Item Code
            Dim strSQL2 As String
            strSQL2 = "SELECT PM_VENDOR_ITEM_CODE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProBP.ItemCode) & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Dim IC_Val As String
            IC_Val = objDb2.GetVal(strSQL2)
            If IC_Val = "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Item Code not found.<BR>"
                bTrue = False
            End If

            ' Check Old Unit Price
            'strSQL2 = "SELECT PM_BUDGET_PRICE FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExProBP.OldUnitPrice) & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'Dim PM_BP_Val As String
            'PM_BP_Val = objDb2.GetVal(strSQL2)
            'If Common.Parse(objExProBP.NewUnitPrice) = "" Then 'no exist
            '    drItem.Item("Message") &= "<LI type=square>Empty of Purpose Budget<BR>"
            '    bTrue = False
            'End If

            If bTrue = False Then
                bAllTrue = False
            End If

            If bAllTrue = True Then
                objCat_Ext.UpdProductBudgetPrice(objExProBP.ItemCode, objExProBP.NewUnitPrice, Session("CompanyId"))
                drItem.Item("Message") = "<Font color='#000000'>Item saved.</Font>"
                countSave = countSave + 1

            Else
                'objCat_Ext.UpdProductBudgetPrice(objExProBP.ItemCode, objExProBP.NewUnitPrice, Session("CompanyId"), False)
                countError = countError + 1

            End If

            objExProBP = Nothing

            'End If
        Next
        Common.NetMsgbox(Me, "Successful: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Function bindProduct(ByRef pProdDetails As ExcelBIMProduct_CommonNew_Ext) As DataSet
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
        dtProduct.Columns.Add("RQL", Type.GetType("System.String"))
        dtProduct.Columns.Add("BudgetPrice", Type.GetType("System.String"))
        dtProduct.Columns.Add("EOQ", Type.GetType("System.String"))
        dtProduct.Columns.Add("Ratio", Type.GetType("System.String"))

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
        'dtProduct.Columns.Add("Packing", Type.GetType("System.String")) 'PM_PACKING_REQ
        dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARKS
        dtProduct.Columns.Add("Deleted", Type.GetType("System.String")) 'PM_DELETED
        dtProduct.Columns.Add("PartialDelivery", Type.GetType("System.String"))
        dtProduct.Columns.Add("IQCType", Type.GetType("System.String"))
        dtProduct.Columns.Add("PackType", Type.GetType("System.String"))
        dtProduct.Columns.Add("PackQty", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Prefer", Type.GetType("System.String"))
        'dtProduct.Columns.Add("1st", Type.GetType("System.String"))
        'dtProduct.Columns.Add("2nd", Type.GetType("System.String"))
        'dtProduct.Columns.Add("3rd", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PreferTax", Type.GetType("System.String"))
        'dtProduct.Columns.Add("1stTax", Type.GetType("System.String"))
        'dtProduct.Columns.Add("2ndTax", Type.GetType("System.String"))
        'dtProduct.Columns.Add("3rdTax", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PreferDelTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("1stDelTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("2ndDelTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("3rdDelTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PreferPayTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("1stPayTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("2ndPayTerm", Type.GetType("System.String"))
        'dtProduct.Columns.Add("3rdPayTerm", Type.GetType("System.String"))

        dtProduct.Columns.Add("MaxInvQty", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu3", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec3", Type.GetType("System.String"))
        dtProduct.Columns.Add("SectionCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("LocationCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("NewItemCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd1", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd2", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("SupCodeP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("SupCode1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("SupCode2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("SupCode3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("UnitPriceP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("UnitPrice1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("UnitPrice2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("UnitPrice3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("CurrencyP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Currency1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Currency2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Currency3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("PurSpecNoP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PurSpecNo1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PurSpecNo2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("PurSpecNo3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("RevisionP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Revision1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Revision2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Revision3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("LeadP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Lead1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Lead2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Lead3", Type.GetType("System.String"))

        'dtProduct.Columns.Add("VenCodeP", Type.GetType("System.String"))
        'dtProduct.Columns.Add("VenCode1", Type.GetType("System.String"))
        'dtProduct.Columns.Add("VenCode2", Type.GetType("System.String"))
        'dtProduct.Columns.Add("VenCode3", Type.GetType("System.String"))

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
        dtr("EOQ") = Common.parseNull(pProdDetails.EOQ)
        dtr("Ratio") = Common.parseNull(pProdDetails.Ratio)
        dtr("BudgetPrice") = pProdDetails.BudgetPrice
        dtr("IQCType") = Common.parseNull(pProdDetails.IQC)
        'dtr("Prefer") = Session("CM_COY_Val")
        'dtr("1st") = Session("CM_COY_Val1")
        'dtr("2nd") = Session("CM_COY_Val2")
        'dtr("3rd") = Session("CM_COY_Val3")
        'dtr("PreferTax") = Common.parseNull(pProdDetails.PreferredVenGST)
        'dtr("1stTax") = Common.parseNull(pProdDetails.Ven1stGST)
        'dtr("2ndTax") = Common.parseNull(pProdDetails.Ven2ndGST)
        'dtr("3rdTax") = Common.parseNull(pProdDetails.Ven3rdGST)
        'dtr("PreferDelTerm") = Common.parseNull(pProdDetails.PreferredVenDelTerm)
        'dtr("1stDelTerm") = Common.parseNull(pProdDetails.Ven1stDelTerm)
        'dtr("2ndDelTerm") = Common.parseNull(pProdDetails.Ven2ndDelTerm)
        'dtr("3rdDelTerm") = Common.parseNull(pProdDetails.Ven3rdDelTerm)
        'dtr("PreferPayTerm") = cboPreferPayTerm.SelectedItem.Value
        'dtr("1stPayTerm") = cbo1stPayTerm.SelectedItem.Value
        'dtr("2ndPayTerm") = cbo2ndPayTerm.SelectedItem.Value
        'dtr("3rdPayTerm") = cbo3rdPayTerm.SelectedItem.Value
        dtr("PackType") = Session("CPT_PACK_CODE")
        dtr("SectionCode") = Common.parseNull(pProdDetails.SectionCode)
        dtr("LocationCode") = Common.parseNull(pProdDetails.LocationCode)
        dtr("NewItemCode") = Common.parseNull(pProdDetails.NewItemCode)

        'If dtr("PreferTax") = "N/A" Or dtr("PreferTax") = "" Then
        '    dtr("PreferTax") = 1
        'Else
        '    dtr("PreferTax") = Val(dtr("PreferTax")) - 1
        'End If

        'If dtr("1stTax") = "N/A" Or dtr("1stTax") = "" Then
        '    dtr("1stTax") = 1
        'Else
        '    dtr("1stTax") = Val(dtr("1stTax")) - 1
        'End If

        'If dtr("2ndTax") = "N/A" Or dtr("2ndTax") = "" Then
        '    dtr("2ndTax") = 1
        'Else
        '    dtr("2ndTax") = Val(dtr("2ndTax")) - 1
        'End If

        'If dtr("3rdTax") = "N/A" Or dtr("3rdTax") = "" Then
        '    dtr("3rdTax") = 1
        'Else
        '    dtr("3rdTax") = Val(dtr("3rdTax")) - 1
        'End If

        dtr("Manu") = Common.parseNull(pProdDetails.ManuName)
        dtr("Manu2") = Common.parseNull(pProdDetails.Manu2Name)
        dtr("Manu3") = Common.parseNull(pProdDetails.Manu3Name)
        dtr("Spec") = Common.parseNull(pProdDetails.Spec)
        dtr("Spec2") = Common.parseNull(pProdDetails.Spec2)
        dtr("Spec3") = Common.parseNull(pProdDetails.Spec3)

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

        If pProdDetails.Oversea.ToUpper = "YES" Then
            dtr("rd3") = "Y"
        Else
            dtr("rd3") = "N"
        End If

        'dtr("SupCodeP") = Common.parseNull(pProdDetails.PreferredVenSuppCode)
        'dtr("SupCode1") = Common.parseNull(pProdDetails.Ven1stSuppCode)
        'dtr("SupCode2") = Common.parseNull(pProdDetails.Ven2ndSuppCode)
        'dtr("SupCode3") = Common.parseNull(pProdDetails.Ven3rdSuppCode)

        'dtr("PurSpecNoP") = Common.parseNull(pProdDetails.PreferredVenPurSpecNo)
        'dtr("PurSpecNo1") = Common.parseNull(pProdDetails.Ven1stPurSpecNo)
        'dtr("PurSpecNo2") = Common.parseNull(pProdDetails.Ven2ndPurSpecNo)
        'dtr("PurSpecNo3") = Common.parseNull(pProdDetails.Ven3rdPurSpecNo)

        'dtr("CurrencyP") = cboCurrencyP.SelectedItem.Value
        'dtr("Currency1") = cboCurrency1.SelectedItem.Value
        'dtr("Currency2") = cboCurrency2.SelectedItem.Value
        'dtr("Currency3") = cboCurrency3.SelectedItem.Value

        'dtr("RevisionP") = Common.parseNull(pProdDetails.PreferredVenRevision)
        'dtr("Revision1") = Common.parseNull(pProdDetails.Ven1stRevision)
        'dtr("Revision2") = Common.parseNull(pProdDetails.Ven2ndRevision)
        'dtr("Revision3") = Common.parseNull(pProdDetails.Ven3rdRevision)

        'dtr("LeadP") = Common.parseNull(pProdDetails.PreferredVenLead)
        'dtr("Lead1") = Common.parseNull(pProdDetails.Ven1stLead)
        'dtr("Lead2") = Common.parseNull(pProdDetails.Ven2ndLead)
        'dtr("Lead3") = Common.parseNull(pProdDetails.Ven3rdLead)

        'dtr("VenCodeP") = Common.parseNull(pProdDetails.PreferredVenItemCode)
        'dtr("VenCode1") = Common.parseNull(pProdDetails.Ven1stItemCode)
        'dtr("VenCode2") = Common.parseNull(pProdDetails.Ven2ndItemCode)
        'dtr("VenCode3") = Common.parseNull(pProdDetails.Ven3rdItemCode)

        dtr("RQL") = pProdDetails.RQL
        dtr("MinInv") = pProdDetails.SafetyLevelMin
        dtr("Min") = pProdDetails.OrderQtyMin
        dtr("Max") = pProdDetails.OrderQtyMax
        dtr("MaxInvQty") = pProdDetails.MaxInvQty
        dtr("PackQty") = pProdDetails.PackQty
        'dtr("UnitPriceP") = pProdDetails.PreferredVenUnitPrice
        'dtr("UnitPrice1") = pProdDetails.Ven1stUnitPrice
        'dtr("UnitPrice2") = pProdDetails.Ven2ndUnitPrice
        'dtr("UnitPrice3") = pProdDetails.Ven3rdUnitPrice

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
        'dtr("Packing") = Common.parseNull(pProdDetails.PackType)
        dtr("Remark") = Common.parseNull(pProdDetails.Remarks)

        dtr("Deleted") = "N"

        If pProdDetails.PartDelCode = "Yes" Then
            dtr("PartialDelivery") = "Y"
        Else
            dtr("PartialDelivery") = "N"
        End If



        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)
        bindProduct = ds
    End Function

    Private Function GetProductDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelBIMProduct_CommonNew_Ext
        Dim objExcel As New ExcelBIMProduct_CommonNew_Ext

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
            objExcel.PartDelCode = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
            objExcel.AccCode = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12"))
            objExcel.OrderQtyMin = IIf(IsDBNull(pdr.Item("F13")), 0, pdr.Item("F13"))
            objExcel.OrderQtyMax = IIf(IsDBNull(pdr.Item("F14")), 0, pdr.Item("F14"))
            objExcel.SafetyLevelMin = IIf(IsDBNull(pdr.Item("F15")), 0, pdr.Item("F15"))
            objExcel.MaxInvQty = IIf(IsDBNull(pdr.Item("F16")), 0, pdr.Item("F16"))
            objExcel.RQL = IIf(IsDBNull(pdr.Item("F17")), 0, pdr.Item("F17"))
            objExcel.BudgetPrice = IIf(IsDBNull(pdr.Item("F18")), 0, pdr.Item("F18"))
            objExcel.IQC = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))
            objExcel.EOQ = IIf(IsDBNull(pdr.Item("F20")), "", pdr.Item("F20"))
            objExcel.Ratio = IIf(IsDBNull(pdr.Item("F21")), "", pdr.Item("F21"))
            objExcel.Oversea = IIf(IsDBNull(pdr.Item("F22")), "", pdr.Item("F22"))
            objExcel.Brand = IIf(IsDBNull(pdr.Item("F23")), "", pdr.Item("F23"))
            objExcel.DrawingNo = IIf(IsDBNull(pdr.Item("F24")), "", pdr.Item("F24"))
            objExcel.Model = IIf(IsDBNull(pdr.Item("F25")), "", pdr.Item("F25"))
            objExcel.GrossWeight = IIf(IsDBNull(pdr.Item("F26")), "", pdr.Item("F26"))
            objExcel.NetWeight = IIf(IsDBNull(pdr.Item("F27")), "", pdr.Item("F27"))
            objExcel.Length = IIf(IsDBNull(pdr.Item("F28")), "", pdr.Item("F28"))
            objExcel.VerNo = IIf(IsDBNull(pdr.Item("F29")), "", pdr.Item("F29"))
            objExcel.Width = IIf(IsDBNull(pdr.Item("F30")), "", pdr.Item("F30"))
            objExcel.ColorInfo = IIf(IsDBNull(pdr.Item("F31")), "", pdr.Item("F31"))
            objExcel.Volume = IIf(IsDBNull(pdr.Item("F32")), "", pdr.Item("F32"))
            objExcel.HSCode = IIf(IsDBNull(pdr.Item("F33")), "", pdr.Item("F33"))
            objExcel.Height = IIf(IsDBNull(pdr.Item("F34")), "", pdr.Item("F34"))
            objExcel.Spec = IIf(IsDBNull(pdr.Item("F35")), "", pdr.Item("F35"))
            objExcel.Spec2 = IIf(IsDBNull(pdr.Item("F36")), "", pdr.Item("F36"))
            objExcel.Spec3 = IIf(IsDBNull(pdr.Item("F37")), "", pdr.Item("F37"))
            objExcel.PackType = IIf(IsDBNull(pdr.Item("F38")), "", pdr.Item("F38"))
            objExcel.PackQty = IIf(IsDBNull(pdr.Item("F39")), 0, pdr.Item("F39"))
            objExcel.ManuName = IIf(IsDBNull(pdr.Item("F40")), "", pdr.Item("F40"))
            objExcel.Manu2Name = IIf(IsDBNull(pdr.Item("F41")), "", pdr.Item("F41"))
            objExcel.Manu3Name = IIf(IsDBNull(pdr.Item("F42")), "", pdr.Item("F42"))
            objExcel.SectionCode = IIf(IsDBNull(pdr.Item("F43")), "", pdr.Item("F43"))
            objExcel.LocationCode = IIf(IsDBNull(pdr.Item("F44")), "", pdr.Item("F44"))
            objExcel.NewItemCode = IIf(IsDBNull(pdr.Item("F45")), "", pdr.Item("F45"))
            objExcel.Remarks = IIf(IsDBNull(pdr.Item("F46")), "", pdr.Item("F46"))
            objExcel.Action = IIf(IsDBNull(pdr.Item("F47")), "", pdr.Item("F47"))
        End With

        Return objExcel

    End Function

    Private Function GetUnitPriceDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelBIMUnitPrice_Common_Ext
        Dim objExcel As New ExcelBIMUnitPrice_Common_Ext

        With objExcel
            objExcel.ItemCode = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
            objExcel.NewUnitPrice = IIf(IsDBNull(pdr.Item("F7")), 0, pdr.Item("F7"))
        End With

        Return objExcel

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

        If rd1.SelectedItem.Value = "I" Then
            ExtBIMDownload()
        Else
            ExtBPDownload()
        End If

    End Sub
    Function Filedownload(ByVal strFile As String)
        'Dim strActualFile As String = "ItemBIMListing.xls"
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(Server.MapPath("../Template/ItemBIMListing.xls"))

        'Dim strActualFile As String = "ItemBIMListing.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & strFile & """")
        Response.Flush()
        Response.WriteFile(strDestPath & strFile)
    End Function

    Function Filedownload2()
        Dim strActualFile As String = "BIMBudgetListing.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/BIMBudgetListing.xls"))
    End Function

    Private Sub cmdDownloadTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        'Dim strActualFile As String = "ItemBIMTemplate.xls"
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(Server.MapPath("../Template/ItemBIMTemplate.xls"))

        Dim intPackTypeRecord, intDelTermRecord, intCommodityRecord As Integer
        Dim dsPackType, dsDelTerm, dsCommodity As DataSet
        Dim strFile As String = "ItemBIMTemplate"
        Dim strFileName As String = "ItemBIMTemplate_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

        dsPackType = Product_Ext.GetItemInfoToExcel("PT")
        intPackTypeRecord = dsPackType.Tables(0).Rows.Count

        dsDelTerm = Product_Ext.GetItemInfoToExcel("DT")
        intDelTermRecord = dsDelTerm.Tables(0).Rows.Count

        dsCommodity = Product_Ext.GetItemInfoToExcel("CM")
        intCommodityRecord = dsCommodity.Tables(0).Rows.Count

        objEx_Ext.WriteCell_ItemUpload(strFile, strDestPath & strFileName, dsPackType, dsDelTerm, dsCommodity)
        objEx_Ext.ProtectWorkSheet_ItemUpload(strDestPath & strFileName, intPackTypeRecord, intDelTermRecord, intCommodityRecord, True)

        'objEx_Ext.WriteCell_ItemUpload(Server.MapPath(Replace(strDestPath, "\", "/") & strFileName), dsPackType, dsDelTerm)
        'objEx_Ext.ProtectWorkSheet_ItemUpload(Server.MapPath(Replace(strDestPath, "\", "/") & strFileName), intPackTypeRecord, intDelTermRecord)
        Filedownload(strFileName)
    End Sub

    Private Sub cmdDownloadTemplateCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplateCode.Click
        Dim strActualFile As String = "UNSPSC v13.1201.xlsx"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/UNSPSC v13.1201.xlsx"))
    End Sub

    Private Sub cmdDownloadTemplateCodePDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplateCodePDF.Click
        Dim strActualFile As String = "UNSPSC_English_v13.1201-3.pdf"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(Server.MapPath("../Template/UNSPSC_English_v13.1201-3.pdf"))
    End Sub

    Private Sub cmdDownloadUNSPSCGuide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadUNSPSCGuide.Click
        Dim strActualFile As String = "GUIDE TO UNSPSC (SEGMENT or CATEGORY BY GOODS & SERVICES TYPE).pdf"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
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
        Session("w_BIM_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId) & """><span>Item Listing</span></a></li>" &
                          "<li><div class=""space""></div></li>" &
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "BIMBatchUpload.aspx", "pageid=" & strPageId) & """><span>Item Batch Upload/Download</span></a></li>" &
                          "<li><div class=""space""></div></li>" &
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BIMAuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" &
                          "<li><div class=""space""></div></li>" &
                          "</ul><div></div></div>"
    End Sub

    Private Sub UserStkTypeCheck()
        Dim objUser As New Users_Ext
        Dim dsStk As New DataSet
        Dim strTemp As String = ""

        dsStk = objUser.GetUserStockType(Session("UserId"), Session("CompanyId"))

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Then
            strTemp = "('SP'"
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Then
            If strTemp <> "" Then
                strTemp = strTemp & ",'ST'"
            Else
                strTemp = "('ST'"
            End If
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
            If strTemp <> "" Then
                strTemp = strTemp & ",'MI')"
            Else
                strTemp = "('MI')"
            End If
        Else
            strTemp = strTemp & ")"
        End If

        Session("UserStkType") = strTemp

    End Sub

    Private Sub RemoveRowDs(ByRef ds As DataSet)
        Dim i As Integer

        For i = 0 To ds.Tables(0).Rows.Count - 1
            If IsDBNull(ds.Tables(0).Rows(i)(1)) And IsDBNull(ds.Tables(0).Rows(i)(2)) And IsDBNull(ds.Tables(0).Rows(i)(3)) And IsDBNull(ds.Tables(0).Rows(i)(5)) And IsDBNull(ds.Tables(0).Rows(i)(6)) And IsDBNull(ds.Tables(0).Rows(i)(9)) And IsDBNull(ds.Tables(0).Rows(i)(21)) And IsDBNull(ds.Tables(0).Rows(i)(46)) Then
                ds.Tables(0).Rows(i).Delete()
            End If

            'If Common.parseNull(ds.Tables(0).Rows(i)(1)) <> "" And Common.parseNull(ds.Tables(0).Rows(i)(2)) <> "" And Common.parseNull(ds.Tables(0).Rows(i)(3)) = "" And Common.parseNull(ds.Tables(0).Rows(i)(5)) Is Nothing And Common.parseNull(ds.Tables(0).Rows(i)(6)) = "" And Common.parseNull(ds.Tables(0).Rows(i)(9)) = "" And Common.parseNull(ds.Tables(0).Rows(i)(21)) = "" And Common.parseNull(ds.Tables(0).Rows(i)(46)) = "" Then
            '    ds.Tables(0).Rows(i).Delete()
            'End If
        Next

    End Sub


    Private Sub ExtBIMDownload()

        Dim ds As New DataSet
        Dim ds2 As New DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection
        Dim intPackTypeRecord, intDelTermRecord, intCommodityRecord As Integer
        Dim dsPackType, dsDelTerm, dsCommodity As DataSet

        'Read Packing Type, Delivery Term
        Dim strFile As String = "ItemBIMListingTemplate"
        Dim strFileName As String = "ItemBIMListing_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

        dsPackType = Product_Ext.GetItemInfoToExcel("PT")
        intPackTypeRecord = dsPackType.Tables(0).Rows.Count

        dsDelTerm = Product_Ext.GetItemInfoToExcel("DT")
        intDelTermRecord = dsDelTerm.Tables(0).Rows.Count

        dsCommodity = Product_Ext.GetItemInfoToExcel("CM")
        intCommodityRecord = dsCommodity.Tables(0).Rows.Count

        'objEx_Ext.WriteCell_ItemUpload(strFile, strDestPath & strFileName, dsPackType, dsDelTerm)
        ds = Product_Ext.Download_ProductExcel_Common(Session("CompanyId"), Session("UserStkType"))
        ds2 = Product_Ext.Download_ProductExcel_Common2(Session("CompanyId"), Session("UserStkType"))
        objEx_Ext.Writecell_Common(ds, ds2, dsPackType, dsDelTerm, dsCommodity, strDestPath & strFileName)
        objEx_Ext.ProtectWorkSheet_ItemUpload(strDestPath & strFileName, intPackTypeRecord, intDelTermRecord, intCommodityRecord)

        Filedownload(strFileName)

    End Sub

    Private Sub ExtBPDownload()

        Dim ds As New DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection

        ds = Product_Ext.Download_BudgetPriceExcel_Common(Session("CompanyId"), Session("UserId"))
        objEx_Ext.Writecell_Common2(ds, Server.MapPath("../Template/BIMBudgetListing.xls"))
        Filedownload2()
    End Sub

    Private Sub rd1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd1.SelectedIndexChanged

        If rd1.SelectedItem.Value = "I" Then
            cmdView.Enabled = False
            lblReportType.Visible = False
            cboReportType.Visible = False
        Else
            cmdView.Enabled = True
            lblReportType.Visible = True
            cboReportType.Visible = True
        End If

    End Sub

    Private Sub cmdView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdView.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If
    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim objDb As New EAD.DBCom
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strChkStk As String
        Dim strEnd As String = ""
        Dim strFileName As String = ""

        strChkStk = objDb.GetVal("SELECT IFNULL(UM_STK_TYPE_STOCK,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserID") & "' AND UM_COY_ID = '" & Session("CompanyID") & "'")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                .CommandText = "SELECT PM_ACCT_CODE AS 'Account Code', PM_VENDOR_ITEM_CODE AS 'Item Code', PM_PRODUCT_DESC AS 'Item Name', " &
                            "PM_SPEC1 AS 'Specification 1', PM_SPEC2 AS 'Specification 2', PM_SPEC3 AS 'Specification 3', PM_UOM AS 'Uom', " &
                            "PM_PREVIOUS_BUDGET_PRICE AS 'Present Budget', PM_BUDGET_PRICE AS 'Proposed Budget', " &
                            "CASE WHEN PM_BUDGET_PRICE = PM_PREVIOUS_BUDGET_PRICE THEN 'EQ' " &
                            "WHEN PM_BUDGET_PRICE < PM_PREVIOUS_BUDGET_PRICE THEN 'DEC' " &
                            "WHEN PM_BUDGET_PRICE > PM_PREVIOUS_BUDGET_PRICE THEN 'INC' ELSE '' END AS 'Remark' " &
                            "FROM (SELECT PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ACCT_CODE, " &
                            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN PM_BUDGET_PRICE ELSE PM_PREVIOUS_BUDGET_PRICE END AS PM_PREVIOUS_BUDGET_PRICE, " &
                            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN NULL ELSE PM_BUDGET_PRICE END AS PM_BUDGET_PRICE, " &
                            "PM_SPEC1, PM_SPEC2, PM_SPEC3 " &
                            "FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & Session("CompanyId") & "' "

                If strChkStk = "Y" Then
                    .CommandText &= "AND PM_ITEM_TYPE = 'ST' "
                Else
                    .CommandText &= "AND PM_ITEM_TYPE = '' "
                End If

                .CommandText &= ") AS sss ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE "

            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "BudgetListingReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim objDb As New EAD.DBCom
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle, strChkStk As String
        Dim strFileName As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        strChkStk = objDb.GetVal("SELECT IFNULL(UM_STK_TYPE_STOCK,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserID") & "' AND UM_COY_ID = '" & Session("CompanyID") & "'")
        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                '.CommandText = "SELECT PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ACCT_CODE, " & _
                '            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN PM_BUDGET_PRICE ELSE PM_PREVIOUS_BUDGET_PRICE END AS PM_PREVIOUS_BUDGET_PRICE, " & _
                '            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN NULL ELSE PM_BUDGET_PRICE END AS PM_BUDGET_PRICE, " & _
                '            "PM_SPEC1, PM_SPEC2, PM_SPEC3, " & _
                '            "CASE WHEN IFNULL(PM_BUDGET_PRICE,0) = IFNULL(PM_PREVIOUS_BUDGET_PRICE,0) THEN 'EQ' " & _
                '            "WHEN IFNULL(PM_BUDGET_PRICE,0) < IFNULL(PM_PREVIOUS_BUDGET_PRICE,0) THEN 'DEC' " & _
                '            "WHEN IFNULL(PM_BUDGET_PRICE,0) > IFNULL(PM_PREVIOUS_BUDGET_PRICE,0) THEN 'INC' " & _
                '            "END AS PM_BUDGET_REMARK FROM PRODUCT_MSTR " & _
                '            "WHERE PM_S_COY_ID = '" & Session("CompanyID") & "' AND PM_ITEM_TYPE IN " & Session("UserStkType")

                .CommandText = "SELECT PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ACCT_CODE, PM_PREVIOUS_BUDGET_PRICE, PM_BUDGET_PRICE, " & _
                            "PM_SPEC1, PM_SPEC2, PM_SPEC3, " & _
                            "CASE WHEN PM_BUDGET_PRICE = PM_PREVIOUS_BUDGET_PRICE THEN 'EQ' " & _
                            "WHEN PM_BUDGET_PRICE < PM_PREVIOUS_BUDGET_PRICE THEN 'DEC' " & _
                            "WHEN PM_BUDGET_PRICE > PM_PREVIOUS_BUDGET_PRICE THEN 'INC' ELSE '' END AS PM_BUDGET_REMARK " & _
                            "FROM (SELECT PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_ACCT_CODE, " & _
                            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN PM_BUDGET_PRICE ELSE PM_PREVIOUS_BUDGET_PRICE END AS PM_PREVIOUS_BUDGET_PRICE, " & _
                            "CASE WHEN PM_PREVIOUS_BUDGET_PRICE IS NULL THEN NULL ELSE PM_BUDGET_PRICE END AS PM_BUDGET_PRICE, " & _
                            "PM_SPEC1, PM_SPEC2, PM_SPEC3 " & _
                            "FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & Session("CompanyID") & "' "

                If strChkStk = "Y" Then
                    .CommandText &= " AND PM_ITEM_TYPE = 'ST' "
                Else
                    .CommandText &= " AND PM_ITEM_TYPE = '' "
                End If

                .CommandText &= ") AS sss ORDER BY PM_ACCT_CODE, PM_VENDOR_ITEM_CODE "

            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("BudgetPriceList_DataSetBudgetPrice", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "BudgetListing_pdf.rdlc", "Report")
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "pmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "BudgetListingReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub
End Class
