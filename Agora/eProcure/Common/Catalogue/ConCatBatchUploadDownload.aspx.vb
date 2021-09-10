Imports System.io
Imports eProcure.Component
Imports AgoraLegacy

Imports System.Text.RegularExpressions

Public Class ConCatBatchUploadDownload
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String
    Dim objCat As New ContCat
    Dim objPro As New Products
    Dim objExConCatHeader As New ExcelConCatHeader
    Dim strDestPath As String = System.Configuration.ConfigurationManager.AppSettings("TemplateTemp")
    Dim strCurVer As String = System.Configuration.ConfigurationManager.AppSettings("ConCatExcelVer")

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
 
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
        Dim strCompanyType As String
        Dim objComp As New Companies

        strCompanyType = objComp.GetCompanyType(Session("CompanyId")) 'HttpContext.Current.Session("CompanyId")
        'If strCompanyType.ToUpper = "VENDOR" Or strCompanyType.ToUpper = "BOTH" Then
        If strCompanyType.ToUpper = "BUYER" Or strCompanyType.ToUpper = "BOTH" Then
            optUpDown_SelectedIndexChanged(sender, e)
            blnPaging = False
            blnSorting = False
            SetGridProperty(dg)
            SetGridProperty(dg2)

            lblPath.Text = ViewState("FilePath")
            If Not Page.IsPostBack Then
                trResult.Visible = False
                GenerateTab()
                Dim dsCat As New DataSet
                Dim cbolist As New ListItem
                dsCat = objCat.getConRefNo()
                Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
                cbolist.Value = ""
                cbolist.Text = "---Select---"
                ddlCode.Items.Insert(0, cbolist)
                ddlCode.SelectedIndex = 0
            End If
        Else
            Dim strMsg As String
            Dim objCat As New ContCat
            strMsg = "Can only upload/download Item List for Buyer Company."
            'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)

        End If

    End Sub

    Private Sub cmdDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownload.Click
        Dim ds As New DataSet
        Dim ds1 As New DataSet
        Dim dsGSTRate, dsGSTTaxCode As DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection
        Dim objCat As New ContCat
        Dim strAryQuery(0) As String
        Dim intGSTRateRecord, intGSTTaxCodeRecord As Integer
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

        Dim strFile As String = "ItemBIMListingTemplate"
        Dim strFileName As String = "ContractCatalogueListing_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

        intGSTRateRecord = dsGSTRate.Tables(0).Rows.Count

        dsGSTRate = objPro.GetItemInfoToExcel("SST")
        dsGSTTaxCode = objPro.GetItemInfoToExcel("SSTTC")
        intGSTTaxCodeRecord = dsGSTTaxCode.Tables(0).Rows.Count

        ds = objCat.Download_CatalogueExcel(ViewState("index"))
        ds1 = objCat.Write_CatalogueExcel(ViewState("index"))
        objEx.WriteConCatCell(ds, ds1, dsGSTRate, dsGSTTaxCode, strDestPath & strFileName)
        objEx.ProtectWorkSheet_ContractUpload(strDestPath & strFileName, intGSTRateRecord, intGSTTaxCodeRecord)
        objCat.InsertAuditTrail(ViewState("index"), "Contract Ref. No.", ddlCode.SelectedItem.Text, ddlCode.SelectedItem.Text, "B", "W", strAryQuery)
        If strAryQuery(0) <> String.Empty Then
            objDb.BatchExecute(strAryQuery)
        End If
        Filedownload(strFileName)
    End Sub

    'Function Filedownload()
    '    Dim strActualFile As String = "ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
    '    Response.ContentType = "application/octet-stream"
    '    Response.AddHeader("Content-Disposition", _
    '      "attachment; filename=""" & strActualFile & """")
    '    Response.Flush()
    '    Response.WriteFile(Server.MapPath("../Report/Temp/ContractCatalogueListing_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"))
    'End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignUser.aspx", "pageid=" & strPageId) & """><span>Users Assignment</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignMultiUser.aspx", "pageid=" & strPageId) & """><span>Multi Users Assignment</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "AuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "</ul><div></div></div>"

    End Sub

    Private Sub cmdDownloadTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        Dim strActualFile As String = "ContractCatalogueTemplate.xls"

        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(Server.MapPath("../Template/ContractCatalogueTemplate.xls"))
        Dim objEx As New AppExcel
        Dim intGSTRateRecord, intTaxCodeRecord As Integer
        Dim dsGSTRate, dsTaxCode As DataSet
        Dim strFile As String = "ContractCatalogueTemplate"
        Dim strFileName As String = "ContractCatalogueTemplate_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

        dsGSTRate = objPro.GetItemInfoToExcel("SST")
        intGSTRateRecord = dsGSTRate.Tables(0).Rows.Count

        dsTaxCode = objPro.GetItemInfoToExcel("SSTTC")
        intTaxCodeRecord = dsTaxCode.Tables(0).Rows.Count

        objEx.WriteCell_ContractUpload(strFile, strDestPath & strFileName, dsGSTRate, dsTaxCode)
        objEx.ProtectWorkSheet_ContractUpload(strDestPath & strFileName, intGSTRateRecord, intTaxCodeRecord)
        Filedownload(strFileName)
    End Sub

    Function Filedownload(ByVal strFile As String)
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & strFile & """")
        Response.Flush()
        Response.WriteFile(strDestPath & strFile)
    End Function

    Private Sub optUpDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUpDown.SelectedIndexChanged
        Select Case optUpDown.SelectedValue
            Case "Upload"
                cmdUpload.Enabled = True
                cmdDownload.Enabled = False
                trRefNo.Disabled = True
                ddlCode.Enabled = False
                ddlCode.SelectedIndex = 0
                cmdBrowse.Disabled = False

            Case "Download"
                cmdUpload.Enabled = False
                If ddlCode.SelectedIndex > 0 Then
                    cmdDownload.Enabled = True
                Else
                    cmdDownload.Enabled = False
                End If
                trRefNo.Disabled = False
                ddlCode.Enabled = True
                cmdBrowse.Disabled = True
        End Select
    End Sub

    Private Sub ddlCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCode.SelectedIndexChanged
        If ddlCode.SelectedIndex > 0 Then
            cmdDownload.Enabled = True
            ViewState("index") = ddlCode.SelectedValue
        Else
            cmdDownload.Enabled = False
        End If
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        Try
            Dim objEx As New AppExcel
            Dim ds As DataSet
            Dim cRules As New myCollection
            Dim objFileMgmt As New FileManagement
            Dim strFileName As String = Path.GetFileName(cmdBrowse.PostedFile.FileName)
            Dim strTempPath As String
            Dim strDestPath As String
            Dim dtHeader As DataTable
            Dim objCompany As New Companies
            Dim objGst As New GST

            Clear()

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings("eProcurePath"))
            ViewState("GSTCOD") = objGst.chkGSTCOD()

            If IsExcel(cmdBrowse.PostedFile.FileName) Then
                Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= Session("FileSize") Then
                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    If ViewState("GSTCOD") = True Then
                        ds = objEx.ReadConExcelFormat(Server.MapPath("../xml/ContractItemList.xml"), strTempPath & strFileName, cRules, dtHeader)
                    Else
                        ds = objEx.ReadConExcelFormat(Server.MapPath("../xml/ContractItemListNoGst.xml"), strTempPath & strFileName, cRules, dtHeader)
                    End If

                    If Not ds Is Nothing Then
                        'Check is contract header valid
                        If CheckContHeader(dtHeader) = False Then
                            Exit Sub
                        End If
                        ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                        If (Not Directory.Exists(strDestPath)) Then
                            Directory.CreateDirectory(strDestPath)
                        End If
                        File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                        If objExConCatHeader.Action = "New" Then
                            AddProduct(ds, cRules, dtHeader)
                            trResult.Visible = True
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
                        ElseIf objExConCatHeader.Action = "Amendment" Then
                            UpdateProduct(ds, cRules, dtHeader)
                            Dim ds1 As New DataSet
                            ds1 = ViewState("FinalDS")
                            trResult.Visible = True
                            If ViewState("GSTCOD") = True Then
                                dg.DataSource = ds1.Tables(0)
                                dg.DataBind()
                                tr_dg.Style("display") = ""
                                tr_dg2.Style("display") = "none"
                            Else
                                dg2.DataSource = ds1.Tables(0)
                                dg2.DataBind()
                                tr_dg2.Style("display") = ""
                                tr_dg.Style("display") = "none"
                            End If
                        End If

                        ViewState("FilePath") = ""
                        lblPath.Text = ""


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

    Private Function CheckContHeader(ByVal dtHeader As DataTable) As Boolean
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
        'Dim objComp As New Companies
        Dim objGst As New GST
        CheckContHeader = True

        objExConCatHeader = GetConCatHeader(dtHeader)
        If objExConCatHeader.Action = "New" Then
            'Check if after GST cut off date and file version is equal to current version
            If ViewState("GSTCOD") = True And strCurVer <> objExConCatHeader.Version Then
                Common.NetMsgbox(Me, "Incorrect Excel File.")
                CheckContHeader = False
                Exit Function
            End If

            'Check if before GST cut off date and file version is not equal to current version
            If ViewState("GSTCOD") = False And strCurVer = objExConCatHeader.Version Then
                Common.NetMsgbox(Me, "Incorrect Excel File.")
                CheckContHeader = False
                Exit Function
            End If

            If objExConCatHeader.CoyName = "" Then
                Common.NetMsgbox(Me, "Vendor Company Name is required.")
                CheckContHeader = False
                Exit Function
            End If
            If objExConCatHeader.ConRefNo = "" Then
                Common.NetMsgbox(Me, "Contract Ref. No. is required.")
                CheckContHeader = False
                Exit Function
            End If
            If objExConCatHeader.ConDesc = "" Then
                Common.NetMsgbox(Me, "Contract Description is required.")
                CheckContHeader = False
                Exit Function
            End If
            If Not Common.checkMaxLength(objExConCatHeader.CoyName, 100) Then
                Common.NetMsgbox(Me, "Vendor Company Name is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            If Not Common.checkMaxLength(objExConCatHeader.ConRefNo, 50) Then
                Common.NetMsgbox(Me, "Contract Ref. No. is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            If Not Common.checkMaxLength(objExConCatHeader.ConDesc, 100) Then
                Common.NetMsgbox(Me, "Contract Description is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            ViewState("VCoyId") = objCat.GetCoyId(objExConCatHeader.CoyName)
            ViewState("GSTRegNo") = objDb.GetVal("SELECT IFNULL(CM_TAX_REG_NO,'') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & ViewState("VCoyId") & "'")
            'ViewState("GSTRegNo") = objGst.chkGST(ViewState("VCoyId"))
            If ViewState("VCoyId") = "" Then
                Common.NetMsgbox(Me, "The vendor company is not a valid approved vendor.")
                CheckContHeader = False
                Exit Function
            End If

            If IsNothing(objExConCatHeader.ValidFrom) Or IsDate(objExConCatHeader.ValidFrom) = False Then
                Common.NetMsgbox(Me, "Invalid Valid From.")
                CheckContHeader = False
                Exit Function
            End If

            If IsNothing(objExConCatHeader.ValidTo) Or IsDate(objExConCatHeader.ValidTo) = False Then
                Common.NetMsgbox(Me, "Invalid Valid To.")
                CheckContHeader = False
                Exit Function
            End If

            If CDate(objExConCatHeader.ValidFrom) > CDate(objExConCatHeader.ValidTo) Then
                Common.NetMsgbox(Me, "Valid To must be greater than or equal to Valid From.")
                CheckContHeader = False
                Exit Function
            End If

            If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX WHERE CDM_GROUP_CODE = '" & Common.Parse(objExConCatHeader.ConRefNo) & "' AND CDM_S_COY_ID = '" & Common.Parse(ViewState("VCoyId")) & "' AND CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
                Common.NetMsgbox(Me, "Duplicate record found.")
                CheckContHeader = False
                Exit Function
            End If

        ElseIf objExConCatHeader.Action = "Amendment" Then
            'Check if after GST cut off date and file version is equal to current version
            If ViewState("GSTCOD") = True And strCurVer <> objExConCatHeader.Version Then
                Common.NetMsgbox(Me, "Incorrect Excel File.")
                CheckContHeader = False
                Exit Function
            End If

            'Check if before GST cut off date and file version is not equal to current version
            If ViewState("GSTCOD") = False And strCurVer = objExConCatHeader.Version Then
                Common.NetMsgbox(Me, "Incorrect Excel File.")
                CheckContHeader = False
                Exit Function
            End If

            If objExConCatHeader.CoyName = "" Then
                Common.NetMsgbox(Me, "Vendor Company Name is required.")
                CheckContHeader = False
                Exit Function
            End If
            If objExConCatHeader.ConRefNo = "" Then
                Common.NetMsgbox(Me, "Contract Ref. No. is required.")
                CheckContHeader = False
                Exit Function
            End If
            If objExConCatHeader.ConDesc = "" Then
                Common.NetMsgbox(Me, "Contract Description is required.")
                CheckContHeader = False
                Exit Function
            End If
            If Not Common.checkMaxLength(objExConCatHeader.CoyName, 100) Then
                Common.NetMsgbox(Me, "Vendor Company Name is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            If Not Common.checkMaxLength(objExConCatHeader.ConRefNo, 50) Then
                Common.NetMsgbox(Me, "Contract Ref. No. is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            If Not Common.checkMaxLength(objExConCatHeader.ConDesc, 100) Then
                Common.NetMsgbox(Me, "Contract Description is over limit.")
                CheckContHeader = False
                Exit Function
            End If

            ViewState("VCoyId") = objCat.GetCoyId(objExConCatHeader.CoyName)
            ViewState("GSTRegNo") = objGst.chkGST(ViewState("VCoyId"))
            If ViewState("VCoyId") = "" Then
                Common.NetMsgbox(Me, "The vendor company is not a valid approved vendor.")
                CheckContHeader = False
                Exit Function
            End If

            If IsNothing(objExConCatHeader.ValidFrom) Or IsDate(objExConCatHeader.ValidFrom) = False Then
                Common.NetMsgbox(Me, "Invalid Valid From.")
                CheckContHeader = False
                Exit Function
            End If

            If IsNothing(objExConCatHeader.ValidTo) Or IsDate(objExConCatHeader.ValidTo) = False Then
                Common.NetMsgbox(Me, "Invalid Valid To.")
                CheckContHeader = False
                Exit Function
            End If

            If CDate(objExConCatHeader.ValidFrom) > CDate(objExConCatHeader.ValidTo) Then
                Common.NetMsgbox(Me, "Valid To must be greater than or equal to Valid From.")
                CheckContHeader = False
                Exit Function
            End If

            ViewState("Index") = objCat.GetConGroupIndex(objExConCatHeader.ConRefNo, ViewState("VCoyId"))
            If ViewState("Index") = 0 Then
                Common.NetMsgbox(Me, "Contract Catalogue not found.")
                CheckContHeader = False
                Exit Function
            End If

        Else
            Common.NetMsgbox(Me, "Invalid Record Action. Please specify the correct action.")
            CheckContHeader = False
            Exit Function
        End If
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

    Private Function FileUpload(ByRef strUploadedPath As String, ByRef strFileName As String) As Boolean
        Try
            Dim Uploadedfilename As String
            Dim objFileMgmt As New FileManagement
            Dim strSourceFile As String = Path.GetFileName(strUploadedPath)
            Dim strTempPath As String

            Dim objCompany As New Companies
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

    Private Function AddProduct(ByRef pds As DataSet, ByVal pRules As myCollection, ByVal dtHeader As DataTable)
        Dim drItem As DataRow
        Dim objCat As New ContCat
        Dim pstrConnStr As String
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim SaveHeader As Integer
        Dim dsHeader As New DataSet

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")
        SaveHeader = 0
        ViewState("mode") = "add"
        dsHeader = bindHeader(objExConCatHeader)

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True

            Dim objExConCat As New ExcelConCat
            objExConCat = GetItemDetails(0, drItem)

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

            ' Check Curr
            Dim strSQL2 As String
            strSQL2 = "SELECT CODE_ABBR FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_DESC = '" & Common.Parse(objExConCat.Currency) & "' "
            Dim CURR_Val As String
            CURR_Val = objDb2.GetVal(strSQL2)
            If CURR_Val = "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Currency code not found.<BR>"
                bTrue = False
            Else
                ViewState("CurrencyCode") = CURR_Val
            End If

            If ViewState("GSTCOD") = True Then
                'Check GST Rate & Tax Code
                Dim GSTRate_Val, GSTTaxCode_Val As String
                If ViewState("GSTRegNo") <> "" And objExConCat.GSTRate <> "" Then
                    strSQL2 = "SELECT CODE_ABBR FROM (SELECT CODE_ABBR, IF((TAX_PERC = '' OR TAX_PERC IS NULL), CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                            "WHERE CODE_CATEGORY = 'SST') tb WHERE GST_RATE = '" & Common.Parse(objExConCat.GSTRate) & "'"
                    GSTRate_Val = objDb2.GetVal(strSQL2)
                    If GSTRate_Val = "" Then
                        drItem.Item("Message") &= "<LI type=square>SST Rate not found.<BR>"
                        bTrue = False
                    Else
                        ViewState("GSTRate") = GSTRate_Val
                        If GSTRate_Val = "EX" Then
                            strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                            If objDb2.Exist(strSQL2) > 0 Then
                                If objExConCat.TaxCode = "" Then
                                    drItem.Item("Message") &= "<LI type=square>SST Tax Code is required.<BR>"
                                    bTrue = False
                                Else
                                    strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_TAX_CODE = '" & Common.Parse(objExConCat.TaxCode) & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                                    GSTTaxCode_Val = objDb2.GetVal(strSQL2)
                                    If GSTTaxCode_Val = "" Then
                                        drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                        bTrue = False
                                    Else
                                        ViewState("GSTTaxCode") = objExConCat.TaxCode
                                    End If
                                End If
                            Else
                                If objExConCat.TaxCode = "" Then
                                    ViewState("GSTTaxCode") = "N/A"
                                Else
                                    drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                    bTrue = False
                                End If
                            End If
                            'If objExConCat.TaxCode <> "" Then
                            '    drItem.Item("Message") &= "<LI type=square>GST Tax Code is not required for 'Exempted' GST Rate.<BR>"
                            '    bTrue = False
                            'Else
                            '    ViewState("GSTTaxCode") = "N/A"
                            'End If
                        Else
                            If objExConCat.TaxCode <> "" Then
                                strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_TAX_CODE = '" & Common.Parse(objExConCat.TaxCode) & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                                GSTTaxCode_Val = objDb2.GetVal(strSQL2)
                                If GSTTaxCode_Val = "" Then
                                    drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                    bTrue = False
                                Else
                                    ViewState("GSTTaxCode") = objExConCat.TaxCode
                                End If
                            Else
                                drItem.Item("Message") &= "<LI type=square>SST Tax Code is required.<BR>"
                                bTrue = False
                            End If
                        End If
                    End If
                ElseIf ViewState("GSTRegNo") <> "" And objExConCat.GSTRate = "" Then
                    drItem.Item("Message") &= "<LI type=square>SST Rate is required for SST registered company.<BR>"
                    bTrue = False
                ElseIf ViewState("GSTRegNo") = "" And (objExConCat.GSTRate <> "" Or objExConCat.TaxCode <> "") Then
                    drItem.Item("Message") &= "<LI type=square>SST Rate & SST Tax Code is not allowed for non-SST registered company.<BR>"
                    bTrue = False
                Else
                    ViewState("GSTRate") = "N/A"
                    ViewState("GSTTaxCode") = "NS"
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

            If bTrue = False Then
                bAllTrue = False
            End If

            If bAllTrue = True Then
                Dim dsProduct As New DataSet

                dsProduct = bindProduct(objExConCat)

                If objCat.SaveContractCatalogue(dsProduct, dsHeader, SaveHeader, ViewState("Index"), ViewState("GSTCOD")) Then
                    If SaveHeader = 0 Then
                        ViewState("Index") = objCat.getLatestIndex1()
                    End If
                    SaveHeader = SaveHeader + 1
                    drItem.Item(1) = objExConCat.ItemCode
                    drItem.Item("Message") = "<Font color='#000000'>Item saved.</Font>"
                    countSave = countSave + 1
                Else
                    drItem.Item("Message") = "Item not saved."
                    countError = countError + 1
                End If
            Else
                countError = countError + 1
            End If

            objExConCat = Nothing
        Next
        Common.NetMsgbox(Me, "Successfull: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Function UpdateProduct(ByRef pds As DataSet, ByVal pRules As myCollection, ByVal dtHeader As DataTable)
        Dim drItem As DataRow
        Dim drTemp As DataRow
        Dim objCat As New ContCat
        Dim pstrConnStr As String
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim dtProduct As New DataTable
        Dim blnSend As Boolean
        Dim SaveHeader As Integer
        Dim strDelete As String
        Dim strNotDelete As String
        Dim dsNotDelete As New DataSet
        Dim dtDelete As New DataTable
        Dim dsHeader As New DataSet
        dsHeader = bindHeader(objExConCatHeader)
        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")
        SaveHeader = 0
        ViewState("mode") = "mod"
        dsNotDelete = objCat.DeleteItem(ViewState("Index"), strNotDelete, strDelete)
        If dsNotDelete.Tables.Count > 0 Then
            If dsNotDelete.Tables(0).Rows.Count > 0 Then
                dtDelete = BindDeletedProduct(dsNotDelete)
            End If
        End If

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True

            Dim objExConCat As New ExcelConCat
            objExConCat = GetItemDetails(0, drItem)

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

            ' Check Curr
            Dim strSQL2 As String
            strSQL2 = "SELECT CODE_ABBR FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_DESC = '" & Common.Parse(objExConCat.Currency) & "' "
            Dim CURR_Val As String
            CURR_Val = objDb2.GetVal(strSQL2)
            If CURR_Val = "" Then 'no exist
                drItem.Item("Message") &= "<LI type=square>Currency code not found.<BR>"
                bTrue = False
            Else
                ViewState("CurrencyCode") = CURR_Val
            End If

            If ViewState("GSTCOD") = True Then
                'Check GST Rate & Tax Code
                Dim GSTRate_Val, GSTTaxCode_Val As String
                If ViewState("GSTRegNo") <> "" And objExConCat.GSTRate <> "" Then
                    strSQL2 = "SELECT CODE_ABBR FROM (SELECT CODE_ABBR, IF((TAX_PERC = '' OR TAX_PERC IS NULL), CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR " &
                            "WHERE CODE_CATEGORY = 'SST') tb WHERE GST_RATE = '" & Common.Parse(objExConCat.GSTRate) & "'"
                    GSTRate_Val = objDb2.GetVal(strSQL2)
                    If GSTRate_Val = "" Then
                        drItem.Item("Message") &= "<LI type=square>SST Rate not found.<BR>"
                        bTrue = False
                    Else
                        ViewState("GSTRate") = GSTRate_Val
                        If GSTRate_Val = "EX" Then
                            strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                            If objDb2.Exist(strSQL2) > 0 Then
                                If objExConCat.TaxCode = "" Then
                                    drItem.Item("Message") &= "<LI type=square>SST Tax Code is required.<BR>"
                                    bTrue = False
                                Else
                                    strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_TAX_CODE = '" & Common.Parse(objExConCat.TaxCode) & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                                    GSTTaxCode_Val = objDb2.GetVal(strSQL2)
                                    If GSTTaxCode_Val = "" Then
                                        drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                        bTrue = False
                                    Else
                                        ViewState("GSTTaxCode") = objExConCat.TaxCode
                                    End If
                                End If
                            Else
                                If objExConCat.TaxCode = "" Then
                                    ViewState("GSTTaxCode") = "N/A"
                                Else
                                    drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                    bTrue = False
                                End If
                            End If
                            'If objExConCat.TaxCode <> "" Then
                            '    drItem.Item("Message") &= "<LI type=square>GST Tax Code is not required for 'Exempted' GST Rate.<BR>"
                            '    bTrue = False
                            'Else
                            '    ViewState("GSTTaxCode") = "N/A"
                            'End If
                        Else
                            If objExConCat.TaxCode <> "" Then
                                strSQL2 = "SELECT '*' FROM TAX_MSTR WHERE TM_TAX_TYPE = 'P' AND TM_TAX_RATE = '" & GSTRate_Val & "' AND TM_TAX_CODE = '" & Common.Parse(objExConCat.TaxCode) & "' AND TM_CATEGORY = 'eProcure' AND TM_DELETED = 'N'"
                                GSTTaxCode_Val = objDb2.GetVal(strSQL2)
                                If GSTTaxCode_Val = "" Then
                                    drItem.Item("Message") &= "<LI type=square>Invalid SST Tax Code found for selected SST Rate.<BR>"
                                    bTrue = False
                                Else
                                    ViewState("GSTTaxCode") = objExConCat.TaxCode
                                End If
                            Else
                                drItem.Item("Message") &= "<LI type=square>SST Tax Code is required.<BR>"
                                bTrue = False
                            End If
                        End If
                    End If
                ElseIf ViewState("GSTRegNo") <> "" And objExConCat.GSTRate = "" Then
                    drItem.Item("Message") &= "<LI type=square>SST Rate is required for SST registered company.<BR>"
                    bTrue = False
                ElseIf ViewState("GSTRegNo") = "" And (objExConCat.GSTRate <> "" Or objExConCat.TaxCode <> "") Then
                    drItem.Item("Message") &= "<LI type=square>SST Rate & SST Tax Code is not allowed for non-SST registered company.<BR>"
                    bTrue = False
                Else
                    ViewState("GSTRate") = "N/A"
                    ViewState("GSTTaxCode") = "NS"
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

            If bTrue = False Then
                bAllTrue = False
            End If

            If bAllTrue = True Then
                Dim intIndex As Integer
                Dim dsProduct As New DataSet
                Dim dtrList As DataRow()
                Dim strSearch As String = ""


                'dsHeader = bindHeader(objExConCatHeader)
                dsProduct = bindProduct(objExConCat)

                If dsNotDelete.Tables.Count > 0 Then
                    strSearch = "ProductCode='" & dsProduct.Tables(0).Rows(0).Item("ProductCode") & "'"
                    dtrList = dsNotDelete.Tables(0).Select(strSearch)
                    If dtrList.Length > 0 Then  'If found
                        For Each oRow As DataRow In dtrList
                            dsNotDelete.Tables(0).Rows.Remove(oRow)
                        Next
                    End If
                End If

                If objCat.UpdateContractCatalogue(dsProduct, dsHeader, SaveHeader, ViewState("GSTCOD")) Then
                    SaveHeader = SaveHeader + 1
                    drItem.Item(1) = objExConCat.ItemCode
                    drItem.Item("Message") = "<Font color='#000000'>Item saved.</Font>"
                    countSave = countSave + 1
                Else
                    drItem.Item("Message") = "Item not saved."
                    countError = countError + 1
                End If

            Else
                countError = countError + 1
            End If

            objExConCat = Nothing
        Next

        Dim dtTemp As New DataTable

        If dsNotDelete.Tables.Count > 0 Then
            If dsNotDelete.Tables(0).Rows.Count > 0 Then
                For Each drItem In dsNotDelete.Tables(0).Rows
                    dtTemp = BindDeletedItem(drItem)
                    drTemp = dtTemp.Rows(0)
                    If objCat.UpdateDeletedContractCatalogue(drItem, dsHeader, SaveHeader) Then
                        countError = countError + 1
                    End If
                    SaveHeader = SaveHeader + 1
                    pds.Tables(0).ImportRow(drTemp)
                Next
            End If
        End If

        ViewState("FinalDS") = pds
        Common.NetMsgbox(Me, "Successfull: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Function BindDeletedProduct(ByVal pds As DataSet) As DataTable
        Dim dtItem As New DataTable
        Dim dtr As DataRow
        Dim drItem As DataRow

        dtItem.Columns.Add("No", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ItemCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ItemName", Type.GetType("System.String"))
        dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        dtItem.Columns.Add("Currency", Type.GetType("System.String"))
        dtItem.Columns.Add("Price", Type.GetType("System.Double"))
        dtItem.Columns.Add("Tax", Type.GetType("System.String")) 'Chee Hong (GST enhancement - 01/07/2014)
        dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
        dtItem.Columns.Add("TaxCode", Type.GetType("System.String"))
        dtItem.Columns.Add("Remarks", Type.GetType("System.String"))
        dtItem.Columns.Add("Message", Type.GetType("System.String"))

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllTrue As Boolean = True

            dtr = dtItem.NewRow()
            dtr("No") = 0
            dtr("ItemCode") = IIf(IsDBNull(drItem.Item("VendorItemCode")), "", drItem.Item("VendorItemCode"))
            dtr("ItemName") = IIf(IsDBNull(drItem.Item("ProductDesc")), "", drItem.Item("ProductDesc"))
            dtr("UOM") = IIf(IsDBNull(drItem.Item("UOM")), "", drItem.Item("UOM"))
            dtr("Currency") = IIf(IsDBNull(drItem.Item("CurrencyCode")), "", drItem.Item("CurrencyCode"))
            dtr("Price") = IIf(IsDBNull(drItem.Item("UnitCost")), 0, drItem.Item("UnitCost"))
            If ViewState("GSTCOD") = True Then
                dtr("GSTRate") = IIf(IsDBNull(drItem.Item("GSTRate")), "", drItem.Item("GSTRate"))
                dtr("TaxCode") = IIf(IsDBNull(drItem.Item("GSTTaxCode")), "", drItem.Item("GSTTaxCode"))
                dtr("Remarks") = IIf(IsDBNull(drItem.Item("Remark")), "", drItem.Item("Remark"))
            Else
                dtr("Tax") = IIf(IsDBNull(drItem.Item("GST")), "", drItem.Item("GST"))
                dtr("Remarks") = IIf(IsDBNull(drItem.Item("Remark")), "", drItem.Item("Remark"))
            End If

            dtr("Message") = "<Font color='#000000'>" & "Deletion is not allowed. It has outstanding PR(s) or PO(s)." & "</Font>"
            dtItem.Rows.Add(dtr)
        Next
        If dtItem.Rows.Count > 0 Then
            BindDeletedProduct = dtItem
        End If
    End Function

    Private Function BindDeletedItem(ByVal drItem As DataRow) As DataTable
        Dim dtItem As New DataTable
        Dim dtr As DataRow
        'Dim drItem As DataRow

        dtItem.Columns.Add("F1", Type.GetType("System.String"))
        dtItem.Columns.Add("F2", Type.GetType("System.String"))
        dtItem.Columns.Add("F3", Type.GetType("System.String"))
        dtItem.Columns.Add("F4", Type.GetType("System.String"))
        dtItem.Columns.Add("F5", Type.GetType("System.String"))
        dtItem.Columns.Add("F6", Type.GetType("System.Double"))
        dtItem.Columns.Add("F7", Type.GetType("System.String"))
        dtItem.Columns.Add("F8", Type.GetType("System.String"))
        dtItem.Columns.Add("F9", Type.GetType("System.String"))
        dtItem.Columns.Add("Message", Type.GetType("System.String"))

        dtr = dtItem.NewRow()
        dtr("F1") = ""
        dtr("F2") = IIf(IsDBNull(drItem.Item("VendorItemCode")), "", drItem.Item("VendorItemCode"))
        dtr("F3") = IIf(IsDBNull(drItem.Item("ProductDesc")), "", drItem.Item("ProductDesc"))
        dtr("F4") = IIf(IsDBNull(drItem.Item("UOM")), "", drItem.Item("UOM"))
        dtr("F5") = IIf(IsDBNull(drItem.Item("CurrencyCode")), "", drItem.Item("CurrencyCode"))
        dtr("F6") = IIf(IsDBNull(drItem.Item("UnitCost")), 0, drItem.Item("UnitCost"))
        If ViewState("GSTCOD") = True Then
            dtr("F7") = IIf(IsDBNull(drItem.Item("GSTRate")), "", drItem.Item("GSTRate"))
            dtr("F8") = IIf(IsDBNull(drItem.Item("GSTTaxCode")), "", drItem.Item("GSTTaxCode"))
            dtr("F9") = IIf(IsDBNull(drItem.Item("Remark")), "", drItem.Item("Remark"))
        Else
            dtr("F7") = IIf(IsDBNull(drItem.Item("GST")), "", drItem.Item("GST")) 'Chee Hong (GST enhancement - 01/07/2014)
            dtr("F8") = IIf(IsDBNull(drItem.Item("Remark")), "", drItem.Item("Remark"))
        End If
        dtr("Message") = "<Font color='#000000'>" & "Deletion is not allowed. It has outstanding PR(s) or PO(s)." & "</Font>"
        dtItem.Rows.Add(dtr)

        If dtItem.Rows.Count > 0 Then
            BindDeletedItem = dtItem
        End If
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
        Dim objExConCat As New ExcelConCat
        objExConCat = GetItemDetails(0, row)

        Dim dt As DataTable

        'If Not IsDBNull(objExConCat.Action) Then

        If objExConCat.ItemCode = "" Then
            row.Item("Message") &= "<LI type=square>" & "Item Code is required."
            bTrue = False
        Else
            strSQL = "SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(objExConCat.ItemCode) & "'" & _
                     " AND PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND PM_PRODUCT_FOR='B' AND PM_DELETED='N'"
            dt = objDb.FillDt(strSQL)
            If dt.Rows.Count > 0 Then ' 0= invalid item code
                If objExConCatHeader.Action = "New" Then
                    If Not IsNothing(ViewState("Index")) Then
                        If ViewState("Index") > 0 Then
                            strSQL = "SELECT '*' FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & ViewState("Index") & " AND CDI_PRODUCT_CODE ='" & dt.Rows(0).Item("PM_PRODUCT_CODE") & "'"
                            If objDb.Exist(strSQL) = 1 Then ' 0= no exist
                                row.Item("Message") &= "<LI type=square>" & "Duplicate Item Code."
                                bTrue = False
                            End If
                        End If
                    End If
                End If

                ViewState("ProductCode") = dt.Rows(0).Item("PM_PRODUCT_CODE")
                ViewState("VendorItemCode") = dt.Rows(0).Item("PM_VENDOR_ITEM_CODE")
                ViewState("ProductDesc") = dt.Rows(0).Item("PM_PRODUCT_DESC")
                ViewState("UOM") = dt.Rows(0).Item("PM_UOM")
            Else
                row.Item("Message") &= "<LI type=square>" & "Invalid Item Code."
                bTrue = False
            End If
            objDb = Nothing

        End If

        objExConCat = Nothing

        Return bTrue
    End Function

    Private Sub Clear()
        ViewState("Index") = 0
        ViewState("ProductCode") = ""
        ViewState("VendorItemCode") = ""
        ViewState("ProductDesc") = ""
        ViewState("UOM") = ""
    End Sub

    Private Function bindProduct(ByRef ConCatItem As ExcelConCat) As DataSet
        Dim ds As New DataSet
        Dim dtItem As New DataTable
        Dim dtr As DataRow

        'Contract Items
        'dtItem.Columns.Add("Index", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        dtItem.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        dtItem.Columns.Add("GST", Type.GetType("System.String")) 'Chee Hong (GST enhancement - 01/07/2014)
        dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
        dtItem.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CurrencyCode", Type.GetType("System.String"))

        dtr = dtItem.NewRow()
        'dtr("Index") = ViewState("Index")
        dtr("ProductCode") = Common.Parse(ViewState("ProductCode"))
        dtr("VendorItemCode") = Common.Parse(ViewState("VendorItemCode"))
        dtr("ProductDesc") = Common.Parse(ViewState("ProductDesc"))
        dtr("UOM") = Common.Parse(ViewState("UOM"))
        dtr("UnitCost") = Common.Parse(ConCatItem.Price)
        dtr("Remark") = Common.parseNull(ConCatItem.Remarks)
        'Chee Hong (GST enhancement - 01/07/2014)
        If ViewState("GSTCOD") = True Then
            dtr("GSTRate") = ViewState("GSTRate")
            dtr("GSTTaxCode") = ViewState("GSTTaxCode")
        Else
            If Common.Parse(ConCatItem.Tax) = "N/A" Or Common.Parse(ConCatItem.Tax) = "" Then
                dtr("GST") = "0"
            Else
                dtr("GST") = Common.Parse(ConCatItem.Tax)
            End If
        End If
        dtr("CurrencyCode") = ViewState("CurrencyCode")
        dtItem.Rows.Add(dtr)
        ds.Tables.Add(dtItem)

        bindProduct = ds
    End Function

    Private Function bindHeader(ByVal ConCatHeader As ExcelConCatHeader) As DataSet
        Dim ds As New DataSet
        Dim dtHeader As New DataTable
        Dim dtr As DataRow

        'Contract Header
        dtHeader.Columns.Add("Index", Type.GetType("System.Int32"))
        dtHeader.Columns.Add("GroupCode", Type.GetType("System.String"))
        dtHeader.Columns.Add("GroupDesc", Type.GetType("System.String"))
        dtHeader.Columns.Add("VendorID", Type.GetType("System.String"))
        dtHeader.Columns.Add("CatType", Type.GetType("System.String"))
        dtHeader.Columns.Add("StartDate", Type.GetType("System.DateTime"))
        dtHeader.Columns.Add("EndDate", Type.GetType("System.DateTime"))

        dtr = dtHeader.NewRow()
        If Not IsNothing(ViewState("Index")) Then
            dtr("Index") = ViewState("Index")
        Else
            dtr("Index") = 0
        End If
        dtr("GroupCode") = Common.Parse(ConCatHeader.ConRefNo)
        dtr("GroupDesc") = Common.Parse(ConCatHeader.ConDesc)
        dtr("CatType") = "C"
        dtr("VendorID") = Common.Parse(ViewState("VCoyId"))
        dtr("StartDate") = Common.Parse(ConCatHeader.ValidFrom)
        dtr("EndDate") = Common.Parse(ConCatHeader.ValidTo)
        dtHeader.Rows.Add(dtr)
        ds.Tables.Add(dtHeader)

        bindHeader = ds
    End Function

    Private Function GetItemDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelConCat
        Dim objExcel As New ExcelConCat

        With objExcel
            objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
            objExcel.ItemCode = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
            objExcel.ItemName = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
            objExcel.UOM = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
            objExcel.Currency = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
            If IsNumeric(pdr.Item("F6")) Then   'Price
                objExcel.Price = IIf(IsDBNull(pdr.Item("F6")), 0, pdr.Item("F6"))
            Else
                objExcel.Price = -1
            End If
            'Chee Hong (GST enhancement - 01/07/2014)
            If ViewState("GSTCOD") = True Then
                objExcel.GSTRate = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
                objExcel.TaxCode = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
                objExcel.Remarks = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
            Else
                objExcel.Tax = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
                objExcel.Remarks = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
            End If
            '-----------------------------------------------
            
        End With
        Return objExcel

    End Function

    Private Function GetDeletedItemDetails(ByVal pdr As DataRow) As ExcelConCat
        Dim objExcel As New ExcelConCat

        With objExcel
            objExcel.No = Nothing
            objExcel.ItemCode = IIf(IsDBNull(pdr.Item("VendorItemCode")), "", pdr.Item("VendorItemCode"))
            objExcel.ItemName = IIf(IsDBNull(pdr.Item("ProductDesc")), "", pdr.Item("ProductDesc"))
            objExcel.UOM = IIf(IsDBNull(pdr.Item("UOM")), "", pdr.Item("UOM"))
            objExcel.Currency = IIf(IsDBNull(pdr.Item("CurrencyCode")), "", pdr.Item("CurrencyCode"))
            objExcel.Price = IIf(IsDBNull(pdr.Item("UnitCost")), 0, pdr.Item("UnitCost"))
            'objExcel.Tax = IIf(IsDBNull(pdr.Item("GST")), "", pdr.Item("GST")) 'Chee Hong (GST enhancement - 01/07/2014)
            objExcel.GSTRate = IIf(IsDBNull(pdr.Item("GSTRate")), "", pdr.Item("GSTRate"))
            objExcel.TaxCode = IIf(IsDBNull(pdr.Item("GSTTaxCode")), "", pdr.Item("GSTTaxCode"))
            objExcel.Remarks = IIf(IsDBNull(pdr.Item("Remark")), "", pdr.Item("Remark"))
        End With
        Return objExcel

    End Function

    Private Function GetConCatHeader(ByVal dt As DataTable) As ExcelConCatHeader
        Dim objExcel As New ExcelConCatHeader

        With objExcel
            objExcel.Action = IIf(IsDBNull(dt.Rows(0).Item("Action")), "", dt.Rows(0).Item("Action"))
            objExcel.CoyName = IIf(IsDBNull(dt.Rows(0).Item("CoyName")), "", dt.Rows(0).Item("CoyName"))
            objExcel.ConRefNo = IIf(IsDBNull(dt.Rows(0).Item("ConRefNo")), "", dt.Rows(0).Item("ConRefNo"))
            objExcel.ConDesc = IIf(IsDBNull(dt.Rows(0).Item("ConDesc")), "", dt.Rows(0).Item("ConDesc"))
            objExcel.ValidFrom = IIf(IsDBNull(dt.Rows(0).Item("ValidFrom")), "", dt.Rows(0).Item("ValidFrom"))
            objExcel.ValidTo = IIf(IsDBNull(dt.Rows(0).Item("ValidTo")), "", dt.Rows(0).Item("ValidTo"))
            objExcel.Version = IIf(IsDBNull(dt.Rows(0).Item("Version")), "", dt.Rows(0).Item("Version"))
        End With
        Return objExcel

    End Function

    'Private Sub dg_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg.ItemDataBound
    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

    '        If dv("F1") = 0 Then
    '            e.Item.Cells(0).Text = ""
    '        End If

    '    End If
    'End Sub
End Class