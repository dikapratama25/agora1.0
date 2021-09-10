Imports System.io
Imports eProcure.Component
Imports AgoraLegacy

Imports System.Text.RegularExpressions

Public Class IPPBatchUpload
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String
    Dim objIPP As New IPPMain
    Dim objExMultiGLHeader As New ExcelIPPMultiGLDebitsHeader
    Dim strVendor, strDocNo, strOldDocNo, strDocType As String
    Dim objGlobal As New AppGlobals
    Dim strIsGst As String
    Dim _exceedCutOffDt As String = ""
    Dim strIsResident As String = ""
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 11 Feb 2015

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
 
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object   
    Protected WithEvents lblAction1 As System.Web.UI.WebControls.Label
    Protected WithEvents optUpDown As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBrowse As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblPath As System.Web.UI.WebControls.Label
    Protected WithEvents trRefNo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdDownloadTemplate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDownload As System.Web.UI.WebControls.Button
    Protected WithEvents trResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dg As System.Web.UI.WebControls.DataGrid
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblDocNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocDate As System.Web.UI.WebControls.Label
    Dim VendorIndex As String

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strCompanyType As String
        Dim objComp As New Companies
        Dim blnIPPOfficer As Boolean
        Dim blnIPPOfficerS As Boolean
        Dim objUsers As New Users
 
        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")
        ViewState("IPPOfficer") = blnIPPOfficer
        ViewState("IPPOfficerS") = blnIPPOfficerS
        ViewState("role") = getUserRole(blnIPPOfficer, blnIPPOfficerS)


        strCompanyType = objComp.GetCompanyType(Session("CompanyId"))

        blnPaging = False
        blnSorting = False
        strDocNo = Request.QueryString("docno")
        strDocType = Request.QueryString("doctype")
        strVendor = Request.QueryString("vencomp")
        strOldDocNo = Request.QueryString("olddocno")
        lblDocNo.Text = strDocNo
        lblDocDate.Text = Request.QueryString("docdate")
        lblVendorName.Text = strVendor
        VendorIndex = Request.QueryString("venidx")
        lblPath.Text = ViewState("FilePath")

        Me.cmdUpload.Attributes.Add("onClick", "UploadProgress();")
    End Sub

    Private Sub cmdDownloadTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        Dim GST As New GST
        Dim ds As New DataSet
        Dim ds1, dsinput, dsoutput As New DataSet
        Dim objEx As New AppExcel
        Dim cRules As New myCollection
        Dim objCat As New ContCat
        Dim strAryQuery(0) As String
        Dim objShopping As New ShoppingCart
        Dim dsItem As New DataSet, dsTemp As New DataSet
        Dim aryProdCode As New ArrayList
        Dim strProdList As String = ""
        Dim i As Integer = 0
        Dim filename As String
        Dim IPPMain As New IPPMain
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
        Dim proc As System.Diagnostics.Process
        Dim intPID As Integer

        ViewState("mode") = Request.QueryString("mode")

        Dim dsRuleCategory, dsGLCode As DataSet
        Dim strSql, strAddr As String
        Dim dvCF As DataView

        filename = "MultipleGLDebits.xls"

        'Jules 2018.07.20 - PAMB - Removed Rule Category
        ''IPP Gst Stage 2A - CH - 11 Feb 2015
        'If strDefIPPCompID = "" Then
        '    dsRuleCategory = IPPMain.getRuleCategory(, , Session("CompanyId"), True)
        '    dsGLCode = IPPMain.getRuleCategoryGLCode(Session("CompanyId"))
        'Else
        '    dsRuleCategory = IPPMain.getRuleCategory(, , strDefIPPCompID, True)
        '    dsGLCode = IPPMain.getRuleCategoryGLCode(strDefIPPCompID)
        'End If
        Dim objGLCode As New IPP
        Dim strGLCode, strGLDesc, strGLStatus As String
        strGLStatus = "A"
        dsGLCode = objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)
        'End modification.
        'dsRuleCategory = IPPMain.getRuleCategory(, , Session("CompanyId"), True)
        'dsGLCode = IPPMain.getRuleCategoryGLCode(Session("CompanyId"))
        dsinput = GST.GetTaxCode_forIPP("", "P")
        dsoutput = GST.GetTaxCode_forIPP("", "S")

        'Zulham 18102018 - PAMB
        Dim dsCC As New DataSet
        dsCC = objGLCode.getCostCentreMultiGL()

        'Jules 2018.07.14 - PAMB
        Dim objDoc As New IPPMain
        Dim dsFundType, dsProductType, dsChannel, dsReinsuranceCo, dsAssetCode, dsProjectCode, dsPersonCode, dsPayFor As DataSet
        dsFundType = objGlobal.GetAnalysisCodeByDept("L1")
        dsProductType = objGlobal.GetAnalysisCodeByDept("L2")
        dsChannel = objGlobal.GetAnalysisCodeByDept("L3")
        dsReinsuranceCo = objGlobal.GetAnalysisCodeByDept("L4")
        dsAssetCode = objGlobal.GetAnalysisCodeByDept("L5")
        dsProjectCode = objGlobal.GetAnalysisCodeByDept("L8")
        dsPersonCode = objGlobal.GetAnalysisCodeByDept("L9")
        dsPayFor = objDoc.PopPayFor

        'Zulham 19102018 - PAMB 
        objEx.WriteCell_IPPUpload(dsRuleCategory, ConfigurationManager.AppSettings("TemplateTemp") & filename, Request.QueryString("mstrDoc"), dsGLCode, dsinput, dsoutput, dsFundType, dsProductType, dsChannel, dsReinsuranceCo, dsAssetCode, dsProjectCode, dsPersonCode, dsPayFor, dsCC, lblDocNo.Text.Trim, lblVendorName.Text.Trim, lblDocDate.Text.Trim)

        Dim _fileName = "MultipleGLDebits.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & _fileName & """")
        Response.Flush()
        Response.WriteFile(ConfigurationManager.AppSettings("TemplateTemp") & "MultipleGLDebits.xls")

    End Sub

    Private Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        Try
            Dim objEx As New AppExcel
            Dim ds, ds2 As DataSet
            Dim cRules As New myCollection
            Dim objFileMgmt As New FileManagement
            Dim strFileName As String = Path.GetFileName(cmdBrowse.PostedFile.FileName)
            Dim strTempPath As String
            Dim strDestPath, version As String
            Dim dtHeader As DataTable
            Dim objCompany As New Companies

            Clear()


            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings("eProcurePath"))

            If IsExcel(cmdBrowse.PostedFile.FileName) Then
                Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'IPP' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= Session("FileSize") Then

                    'Check if is resident
                    'Zulham 13/10/2015 - Added condition that is coytype = 'V'
                    strIsResident = objDB.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_coy_type = 'V' and ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp").ToString) & "'")

                    'Jules 2018.07.20 - PAMB allow "\" and "#"
                    'Check file version; newer one has 1.0
                    Dim documentDate = objDB.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(Request.QueryString("docno")), "\", "\\"), "#", "\#") & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                    'Zulham 13082015
                    'Additional condition for company's effective date
                    'Dim effectiveDate = objDB.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    'If Trim(effectiveDate) <> "" Then
                    If CDate(createdDate) >= CDate(_cutoffDate) Then 'And CDate(createdDate) >= CDate(effectiveDate) Then
                        _exceedCutOffDt = "Yes"
                        If Request.QueryString("vencomp").ToString <> "" Then
                            Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp").ToString) & "'")
                            If GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) Then 'And CDate(documentDate) >= CDate(effectiveDate) Then
                                strIsGst = "Yes"
                            Else
                                strIsGst = "No"
                            End If
                        Else
                            strIsGst = "Yes"
                        End If
                    Else
                        strIsGst = "No"
                        'Zulham 26/02/2015 IPP Stage 1
                        '_exceedCutOffDt = "No"
                    End If
                    'Else
                    'strIsGst = "No"
                    ''_exceedCutOffDt = "No"
                    'End If

                    If CDate(createdDate) >= CDate(_cutoffDate) Then
                        _exceedCutOffDt = "Yes"
                    End If

                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    'Zulham 26/02/2015 IPP Stage 1
                    If strIsGst = "No" And _exceedCutOffDt = "No" Then
                        ds = objEx.ReadIPPMultiGLExcelFormat(Server.MapPath("../xml/MultiGLDebitsNoGST.xml"), strTempPath & strFileName, cRules, dtHeader, version, _exceedCutOffDt)
                    Else
                        ds = objEx.ReadIPPMultiGLExcelFormat(Server.MapPath("../xml/MultiGLDebits.xml"), strTempPath & strFileName, cRules, dtHeader, version, _exceedCutOffDt)
                    End If

                    If (version = "" And _exceedCutOffDt = "Yes") Then
                        Common.NetMsgbox(Me, "Incorrect excel file format uploaded.")
                        Exit Sub
                    ElseIf (version.Trim = "1.0" And _exceedCutOffDt = "No") Then
                        Common.NetMsgbox(Me, "Incorrect excel file format uploaded.")
                        Exit Sub
                    End If
                    'End

                    ''Zulham 13052015 IPP GST Stage 1
                    ''Limit the excel items to 500
                    'Dim itemCount() = ds.Tables(0).Select("F2 <> ''")
                    'If itemCount.Length > 500 Then
                    '    Common.NetMsgbox(Me, "Maximum number allowed for invoice items is 500.")
                    '    Exit Sub
                    'End If

                    If dtHeader.Rows(0).Item(3).ToString = "Yes" Then
                        ds2 = objEx.ReadIPPMultiGLExcelFormat_SubDoc(Server.MapPath("../xml/MultiGLDebits.xml"), strTempPath & strFileName, cRules, dtHeader)
                        If ds2 IsNot Nothing Then
                            If ds.Tables(0).Rows.Count > 0 Then
                                For Each row As DataRow In ds2.Tables(0).Rows
                                    If IsDBNull(row("F4")) Then
                                        row("F4") = 0.0
                                    End If
                                Next
                            Else
                                Common.NetMsgbox(Me, "Please add the sub-document.")
                                Exit Sub
                            End If
                        Else
                            Common.NetMsgbox(Me, "Please add the sub-document.")
                            Exit Sub
                        End If
                    Else
                        ds2 = Nothing
                    End If

                    If Not ds Is Nothing Then
                        'Check is contract header valid
                        If CheckHeader(dtHeader) = False Then
                            Exit Sub
                        End If
                        ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))

                        If (Not Directory.Exists(strDestPath)) Then
                            Directory.CreateDirectory(strDestPath)
                        End If
                        File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))
                        AddGLEntry(ds, cRules, dtHeader, ds2)
                        ViewState("FilePath") = ""
                        lblPath.Text = ""
                        'Zulham 19082015
                        'Items not loaded after file upload
                        Session("Action") = "Edit"
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

    Private Function CheckHeader(ByVal dtHeader As DataTable) As Boolean
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
        CheckHeader = True

        objExMultiGLHeader = GetMultiGLHeader(dtHeader)
        If objExMultiGLHeader.CoyName = "" Then
            Common.NetMsgbox(Me, "Vendor Company Name " & objGlobal.GetErrorMessage("00001") & ".")
            CheckHeader = False
            Exit Function
        End If

        If objExMultiGLHeader.DocNo = "" Then
            Common.NetMsgbox(Me, "Document No. " & objGlobal.GetErrorMessage("00001") & ".")
            CheckHeader = False
            Exit Function
        End If
        If Not Common.checkMaxLength(objExMultiGLHeader.CoyName, 100) Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00196"))
            CheckHeader = False
            Exit Function
        End If

        If Not Common.checkMaxLength(objExMultiGLHeader.DocNo, 50) Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00346"))
            CheckHeader = False
            Exit Function
        End If

        'mimi 2018-04-24 : remove hardcode HLB to PAMB
        'ViewState("VCoyId") = objDb.GetVal("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = 'hlb' AND IC_STATUS = 'A'")
        ViewState("VCoyId") = objDb.GetVal("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IC_STATUS = 'A'")

        If ViewState("VCoyId") = "" Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00347"))
            CheckHeader = False
            Exit Function
        End If

        If IsNothing(objExMultiGLHeader.DocDate) Or IsDate(objExMultiGLHeader.DocDate) = False Then
            Common.NetMsgbox(Me, "Document Date." & objGlobal.GetErrorMessage("00283") & ".")
            CheckHeader = False
            Exit Function
        End If

        If strDocNo.ToUpper.Trim <> objExMultiGLHeader.DocNo.ToUpper.Trim Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00348"))
            CheckHeader = False
            Exit Function
        End If

        If RTrim(strVendor) <> RTrim(objExMultiGLHeader.CoyName) Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00349"))
            CheckHeader = False
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
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'IPP' AND SP_PARAM_NAME = 'DocAttachSize'"))

            If cmdBrowse.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00068"))
                Return False
            ElseIf cmdBrowse.PostedFile.ContentLength / 1024 > Session("FileSize") Then ' dblMaxFileSize Then
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00069"))
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

    Private Function AddGLEntry(ByRef pds As DataSet, ByVal pRules As myCollection, ByVal dtHeader As DataTable, Optional ByVal dsSubDoc As DataSet = Nothing)
        Dim drItem As DataRow
        Dim objCat As New ContCat
        Dim pstrConnStr As String
        Dim strInvIdx, strSQL, costCentre, branchCode, strGSTPerc, strGSTID, coyType As String 'Zulham 19082015 - Added coyType 
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim SaveHeader As Integer
        Dim dsHeader, ds, dsTiedCC As New DataSet
        Dim bAllTrue As Boolean = True
        Dim strAryQuery(0) As String
        Dim percentage As String
        '  Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

        'Zulham 12112018
        Dim objTax As New GST

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")
        SaveHeader = 0
        ViewState("mode") = "add"
        vldsum.InnerHtml = ""
        dsHeader = bindHeader(objExMultiGLHeader)



        Dim objDb As EAD.DBCom
        If pstrConnStr Is Nothing Then
            objDb = New EAD.DBCom
        Else
            objDb = New EAD.DBCom(pstrConnStr)
        End If

        Dim objDb2 As EAD.DBCom
        If pstrConnStr Is Nothing Then
            objDb2 = New EAD.DBCom
        Else
            objDb2 = New EAD.DBCom(pstrConnStr)
        End If

        'check vendor
        Dim strSQL2 As String

        'Jules 2018.07.19 - PAMB
        'IPP Gst Stage 2A - CH - 11 Feb 2015
        'If strDefIPPCompID = "" Then
        '    strSQL2 = "SELECT * FROM IPP_company WHERE IC_STATUS = 'A' AND ic_other_b_coy_code = '' AND ic_coy_id = '" & Session("CompanyId") & "' AND ic_coy_name = '" & Common.Parse(RTrim(objExMultiGLHeader.CoyName)) & "' "
        'Else
        '    strSQL2 = "SELECT * FROM IPP_company WHERE IC_STATUS = 'A' AND ic_other_b_coy_code = '' AND ic_coy_id = '" & strDefIPPCompID & "' AND ic_coy_name = '" & Common.Parse(RTrim(objExMultiGLHeader.CoyName)) & "' "
        'End If
        If strDefIPPCompID = "" Then
            strSQL2 = "SELECT * FROM IPP_company WHERE IC_STATUS = 'A' AND ic_coy_type = 'V' AND ic_coy_id = '" & Session("CompanyId") & "' AND ic_coy_name = '" & Common.Parse(RTrim(objExMultiGLHeader.CoyName)) & "' "
        Else
            strSQL2 = "SELECT * FROM IPP_company WHERE IC_STATUS = 'A' AND ic_coy_type = 'V' AND ic_coy_id = '" & strDefIPPCompID & "' AND ic_coy_name = '" & Common.Parse(RTrim(objExMultiGLHeader.CoyName)) & "' "
        End If
        'End modification.

        'strSQL2 = "SELECT * FROM IPP_company WHERE IC_STATUS = 'A' AND ic_other_b_coy_code = '' AND ic_coy_id = '" & Session("CompanyId") & "' AND ic_coy_name = '" & Common.Parse(RTrim(objExMultiGLHeader.CoyName)) & "' "
        Dim strVen As String
        strVen = objDb2.GetVal(strSQL2)
        If strVen = "" Then 'no exist
            'drItem.Item("Message") &= "<LI type=square>Vendor is required.<BR>"
            Common.NetMsgbox(Me, "Vendor " & objGlobal.GetErrorMessage("00001") & ".")
            Exit Function
        Else
            Session("VendorVal") = strVen
        End If

        'check doc no
        If objExMultiGLHeader.DocNo = "" Then
            'drItem.Item("Message") &= "<LI type=square>Document No. is required.<BR>"
            Common.NetMsgbox(Me, "Document No. " & objGlobal.GetErrorMessage("00001") & ".")
            Exit Function
        End If

        'check doc date
        If objExMultiGLHeader.DocDate Is Nothing Then
            'drItem.Item("Message") &= "<LI type=square>Document date is required.<BR>"
            Common.NetMsgbox(Me, "Document date " & objGlobal.GetErrorMessage("00001") & ".")
            Exit Function
        End If

        'Delete GL Debits

        'Zulham Feb 11, 2014
        If VendorIndex Is Nothing Then
            VendorIndex = dsHeader.Tables(0).Rows(0).Item(0).ToString
        End If
        'End

        objIPP.DelIMultiGLDebits(dsHeader, strDocNo, strOldDocNo, strVendor, VendorIndex)
        'Delete subdoc 
        'Jules 2018.07.20 - PAMB allow "\" and "#"
        If strDocNo <> strOldDocNo Then
            strInvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(strOldDocNo, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
        Else
            strInvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")), "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
        End If
        strSQL = "delete from ipp_sub_doc where isd_mstr_doc_index = " & strInvIdx
        Common.Insert2Ary(strAryQuery, strSQL)
        strSQL = "update invoice_mstr set im_ind1 = 'N' where im_invoice_index = " & strInvIdx
        Common.Insert2Ary(strAryQuery, strSQL)
        objDb.BatchExecute(strAryQuery)


        'Zulham 19082015 - Get coyType
        coyType = objDb.GetVal("select ifnull(ic_coy_type,'') from ipp_company where ic_index = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")

        'Sub doc
        'do some validation first
        Dim counted As Integer = 0
        Dim subdocAmt, invoiceDetailsAmt As Double
        Dim strSubDocNo As String = ""
        Dim result As String = ""

        'Jules 2018.07.20 - PAMB allow "/","\" and "#"
        'Dim specialChar As Char() = "`~!@#$%^&*()-_+=[]{};':""|\,./<>?".ToCharArray
        Dim specialChar As Char() = "`~!@$%^&*()-_+=[]{};':""|,.<>?".ToCharArray

        If Not dsSubDoc Is Nothing Then
            For row As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                Try
                    Dim bRtn As Boolean = True
                    Dim bAllColTrue As Boolean = True
                    Dim objExProduct As New ExcelIPPMultiGLDebits_SubDoc
                    If Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(0)).ToString <> "" Or Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(1).ToString).ToString <> "" Or Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(2)).ToString <> "" Then

                        'Validation for special character
                        If IsDBNull(dsSubDoc.Tables(0).Rows(row).Item(0)) Then
                            Common.NetMsgbox(Me, "Sub Document No. cannot have special character.", MsgBoxStyle.Information)
                            Session("SubDocument") = Nothing
                            Return False
                            Exit Function
                        End If

                        For Each ch As Char In dsSubDoc.Tables(0).Rows(row).Item(0).ToString
                            If Array.IndexOf(specialChar, ch) <> -1 Then
                                Common.NetMsgbox(Me, "Sub Document No. cannot have special character.", MsgBoxStyle.Information)
                                Session("SubDocument") = Nothing
                                Return False
                                Exit Function
                            End If
                        Next

                        If dsSubDoc.Tables(0).Rows(row).Item(1) Is DBNull.Value Then
                            Common.NetMsgbox(Me, "Invalid date format at line " & row + 1, MsgBoxStyle.Exclamation)
                            Session("SubDocument") = Nothing
                            Return False
                            Exit Function
                        End If

                        'Validation for 2 items with the same num
                        Dim counter As Integer = 0
                        Dim strReplicate() = dsSubDoc.Tables(0).Select("F1 = '" & dsSubDoc.Tables(0).Rows(row).Item(0).ToString & "'")
                        counter = strReplicate.Length
                        If CDec(counter) > CDec(1) Then
                            Common.NetMsgbox(Me, "Sub Document No. cannot be duplicated.", MsgBoxStyle.Information)
                            Session("SubDocument") = Nothing
                            Return False
                            Exit Function
                        Else
                            If strSubDocNo = "" Then
                                strSubDocNo = strSubDocNo & "'" & dsSubDoc.Tables(0).Rows(row).Item(0).ToString & "'"
                            Else
                                strSubDocNo = strSubDocNo & ",'" & dsSubDoc.Tables(0).Rows(row).Item(0).ToString & "'"
                            End If
                        End If

                        'Check Similar Doc No
                        If lblDocNo.Text = Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(0)).ToString Then
                            Common.NetMsgbox(Me, "Sub Document No. must not be equal to Master Document No.")
                            Session("SubDocument") = Nothing
                            Exit Function
                        End If

                        If counted = 0 Then
                            If _exceedCutOffDt = "Yes" Then
                                For i As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                                    If Not dsSubDoc.Tables(0).Rows(i).Item(2) Is DBNull.Value Then
                                        If Not dsSubDoc.Tables(0).Rows(i).Item(3) Is DBNull.Value Then
                                            subdocAmt += dsSubDoc.Tables(0).Rows(i).Item(2) + dsSubDoc.Tables(0).Rows(i).Item(3)
                                        Else
                                            subdocAmt += dsSubDoc.Tables(0).Rows(i).Item(2)
                                        End If
                                    End If
                                Next
                                For j As Integer = 0 To pds.Tables(0).Rows.Count - 1
                                    If Not pds.Tables(0).Rows(j).Item(7) Is DBNull.Value Then 'Amount

                                        'Zulham 21082015 - Get GIFT_LUCKY_OUTPUT code
                                        Dim compOutputTaxValue_TX4 = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")

                                        'Add Input Tax first, then minus output
                                        If Not pds.Tables(0).Rows(j).Item(8) Is DBNull.Value Then
                                            If Not pds.Tables(0).Rows(j).Item(9) Is DBNull.Value Then 'InputTax
                                                If pds.Tables(0).Rows(j).Item(9).ToString.Contains("(") And Not (pds.Tables(0).Rows(j).Item(9).ToString.Contains("IM1") _
                                                Or pds.Tables(0).Rows(j).Item(9).ToString.Contains("IM3")) Then
                                                    If CDec(pds.Tables(0).Rows(j).Item(9).ToString.Split("(")(1).Substring(0, 1)) > CDec(0) Then
                                                        invoiceDetailsAmt += pds.Tables(0).Rows(j).Item(7) + pds.Tables(0).Rows(j).Item(8)
                                                    Else
                                                        invoiceDetailsAmt += pds.Tables(0).Rows(j).Item(7)
                                                    End If
                                                Else
                                                    invoiceDetailsAmt += pds.Tables(0).Rows(j).Item(7)
                                                End If
                                                If Not pds.Tables(0).Rows(j).Item(10) Is DBNull.Value Then 'OutputTax
                                                    If pds.Tables(0).Rows(j).Item(10).ToString.Contains("(") And Not pds.Tables(0).Rows(j).Item(10).ToString.Contains(Trim(compOutputTaxValue_TX4)) Then
                                                        If CDec(pds.Tables(0).Rows(j).Item(10).ToString.Split("(")(1).Substring(0, 1)) > CDec(0) Then
                                                            invoiceDetailsAmt = invoiceDetailsAmt - pds.Tables(0).Rows(j).Item(8)
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Else
                                            invoiceDetailsAmt += pds.Tables(0).Rows(j).Item(7)
                                        End If
                                    End If
                                Next
                            Else
                                For i As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                                    If Not dsSubDoc.Tables(0).Rows(i).Item(2) Is DBNull.Value Then
                                        subdocAmt += dsSubDoc.Tables(0).Rows(i).Item(2)
                                    End If
                                Next

                                For j As Integer = 0 To pds.Tables(0).Rows.Count - 1
                                    If Not pds.Tables(0).Rows(j).Item(6) Is DBNull.Value Then
                                        'Add Input Tax first, then minus output
                                        invoiceDetailsAmt += pds.Tables(0).Rows(j).Item(6)
                                    End If
                                Next
                            End If

                            If CDec(Math.Round(subdocAmt, 2)) <> CDec(Math.Round(invoiceDetailsAmt, 2)) Then
                                Common.NetMsgbox(Me, "Sub Document Total must tally with items total.")
                                Session("SubDocument") = Nothing
                                Exit Function
                            End If
                        End If
                        counted = 1
                    ElseIf Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(0)).ToString = "" Then
                        Common.NetMsgbox(Me, "Sub Document No cannot be empty.")
                        Session("SubDocument") = Nothing
                        Exit Function
                    ElseIf Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(1).ToString).ToString = "" Then
                        Common.NetMsgbox(Me, "Sub Document Date cannot be empty.")
                        Session("SubDocument") = Nothing
                        Exit Function
                    ElseIf Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(2)).ToString = "" Then
                        Common.NetMsgbox(Me, "Sub Document Amount cannot be empty.")
                        Session("SubDocument") = Nothing
                        Exit Function
                    ElseIf Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(0)).ToString = "" And Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(1).ToString).ToString = "" And Common.parseNull(dsSubDoc.Tables(0).Rows(row).Item(2)).ToString = "" Then
                        Common.NetMsgbox(Me, "Sub Document cannot be empty.")
                        Session("SubDocument") = Nothing
                        Exit Function
                    End If
                Catch ex As Exception
                    Throw New Exception(ex.ToString)
                End Try
            Next
            strSubDocNo = "(" & strSubDocNo & ")"
        Else
            If dtHeader.Rows(0).Item("MasterDoc") = "Yes" Then
                Common.NetMsgbox(Me, "Please insert sub-document data in the Sub Document tab.")
                Session("SubDocument") = Nothing
                Exit Function
            End If
        End If

        'Data validity check against DB
        ''Check Existing Doc No
        If Not strSubDocNo = "" Then
            strSubDocNo = Replace(Replace(strSubDocNo, "\", "\\"), "#", "\#") 'Jules 2018.07.20 - PAMB allow "\" and "#"
            Dim invoiceIdx

            'Jules 2018.07.20 - PAMB allow "\" and "#"
            If strDocNo <> strOldDocNo Then
                invoiceIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(strOldDocNo, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
            Else
                invoiceIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")), "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
            End If

            result = objDb.GetVal("select * from ipp_sub_doc where isd_mstr_doc_index = " & invoiceIdx & " and isd_doc_no in " & strSubDocNo & "")
            If Not result = "" Then
                Common.NetMsgbox(Me, "Sub Document No must not exist.")
                Session("SubDocument") = Nothing
                Exit Function
            End If

            'Same vendor - two diff invoices - sub doc no in one of the invoices <> sub doc no in the other invoice no
            strSQL = "SELECT IM_INVOICE_NO FROM invoice_mstr, ipp_SUB_DOC WHERE im_invoice_index = isd_mstr_doc_index and ISD_DOC_NO in " & strSubDocNo & " AND " &
                     "im_s_coy_id = '" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("VendorID")) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL limit 1"
            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                Common.NetMsgbox(Me, "Sub Document No must not exist.")
                Session("SubDocument") = Nothing
                Exit Function
            End If

            'Same vendor - two diff invoices - sub doc no in one of the invoices <> the other invoice no
            strSQL = "SELECT im_invoice_index, IM_INVOICE_NO FROM invoice_mstr, ipp_SUB_DOC WHERE im_invoice_index = isd_mstr_doc_index and " &
                     "ISD_DOC_NO in " & strSubDocNo & " AND im_s_coy_id = '" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("VendorID")) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                Common.NetMsgbox(Me, "Master Document No. must not equal to Sub Document No.")
                Session("SubDocument") = Nothing
                Exit Function
            End If

            strSQL = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_no IN " & strSubDocNo & " AND " &
                     "im_s_coy_id = '" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("VendorID")) & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' " &
                     "AND IM_PO_INDEX IS NULL"
            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                Common.NetMsgbox(Me, "Sub Document No must not equal to Master Document No.")
                Session("SubDocument") = Nothing
                Exit Function
            End If

            'Update Data
            If Not Session("SubDocument") Is Nothing Then
                Dim totalAmt As Double : totalAmt = 0.0
                If Not CType(Session("SubDocument"), ArrayList).Count = 0 Then
                    For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                        For k As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                            If i = k Then
                                CType(Session("SubDocument"), ArrayList)(i)(1) = dsSubDoc.Tables(0).Rows(k).Item("F1")
                                CType(Session("SubDocument"), ArrayList)(i)(2) = dsSubDoc.Tables(0).Rows(k).Item("F2")
                                CType(Session("SubDocument"), ArrayList)(i)(3) = Format(CDbl(dsSubDoc.Tables(0).Rows(k).Item("F3")), "#,###.00")
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If
            'End

        End If
        'End

        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllColTrue As Boolean = True

            Dim objExProduct As New ExcelIPPMultiGLDebits
            'Zulham 05102018 - PAMB SST
            If Common.parseNull(drItem(1)) <> "" Or Common.parseNull(drItem(3).ToString) <> "" Or Common.parseNull(drItem(4)) Is Nothing Or Common.parseNull(drItem(8)) Is Nothing Then
                If Not _exceedCutOffDt = "No" Then
                    If Not (Common.parseNull(drItem(1)) <> "" Or Common.parseNull(drItem(3).ToString) <> "" Or Common.parseNull(drItem(6)) Is Nothing Or Common.parseNull(drItem(7)) Is Nothing Or Common.parseNull(drItem(11).ToString) <> "" Or Common.parseNull(drItem(13)) Is Nothing) Then
                        GoTo kkk
                    End If
                End If
                objExProduct = GetItemDetails(0, drItem)
                ' Check Pay For
                'Dim strSQL2 As String
                If objExProduct.PayFor <> "" Then
                    If UCase(objExProduct.PayFor) = "OWN CO." Then
                        'IPP Gst Stage 2A - CH - 11 Feb 2015
                        Session("PayForVal") = UCase(Session("CompanyId"))
                        objExProduct.PayFor = UCase(Session("CompanyId"))
                        'Session("PayForVal") = "HLB" 'objExProduct.PayFor
                        'objExProduct.PayFor = "HLB"
                        'objExProduct.PayFor = objDb2.GetVal("SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A'  AND ic_coy_id = '" & Session("CompanyId") & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' ")

                        'Zulham 01072015 - HLB-IPP Stage 4(CR)
                        If Common.Parse(Session("CompanyId").ToString.Trim.ToUpper) = "HLISB" Then
                            If UCase(objExProduct.Reimbursement.Trim) <> "N/A" Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Reimbursement/Disbursement selection]<BR>"
                                bTrue = False
                            End If
                        End If
                        'Zulham 29062015 - HLB-IPP Stage 4(CR)
                        'For login company HLISB, the payfor comp must always be own co.
                    ElseIf Common.Parse(Session("CompanyId").ToString.Trim.ToUpper) = "HLISB" Then
                        If UCase(objExProduct.PayFor) <> "OWN CO." Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid PayFor selection]<BR>"
                            bTrue = False
                        ElseIf UCase(objExProduct.Reimbursement.Trim) <> "N/A" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Reimbursement/Disbursement selection]<BR>"
                            bTrue = False
                        End If
                    Else

                        'Zulham 28072015
                        'if payfor comp <> own comp. and reimb/disb = 'N/A', then it's invalid
                        'Zulham 19082015
                        If UCase(objExProduct.Reimbursement.Trim) = "N/A" And Not UCase(objExProduct.InputTax.Trim) = "NR (0%)" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Reimbursement/Disbursement selection]<BR>"
                            bTrue = False
                        End If


                        'IPP Gst Stage 2A - CH - 11 Feb 2015
                        If strDefIPPCompID = "" Then
                            strSQL2 = "SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A' and ic_other_b_coy_code <> '' AND ic_coy_id = '" & Session("CompanyId") & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' "
                        Else
                            strSQL2 = "SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A' and ic_other_b_coy_code <> '' AND ic_coy_id = '" & strDefIPPCompID & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' "
                        End If
                        'strSQL2 = "SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A' and ic_other_b_coy_code <> '' AND ic_coy_id = '" & Session("CompanyId") & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' "
                        Dim PF_Val As String
                        PF_Val = objDb2.GetVal(strSQL2)
                        If PF_Val = "" Then 'no exist
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00350") & "]<BR>"
                            bTrue = False
                        Else
                            Session("PayForVal") = PF_Val
                            objExProduct.PayFor = PF_Val
                        End If

                    End If
                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Company Pay For " & objGlobal.GetErrorMessage("00001") & "].<BR>"
                    bTrue = False
                End If

                'Item validation based on GST Matrix 
                'Zulham 26/02/2015 IPP Stage 1
                If (_exceedCutOffDt = "Yes" And strIsGst = "Yes") Or (strIsResident = "N" And _exceedCutOffDt = "Yes" And strIsGst = "Yes") Then
                    If UCase(objExProduct.PayFor) = "OWN CO." And (objExProduct.Reimbursement <> "N/A") Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for Reimbursement/Disbursement].<BR>"
                        bTrue = False
                    End If
                    'Zulham 23102018 - PAMB
                    'If UCase(objExProduct.PayFor) = "OWN CO." And (objExProduct.InputTax <> "N/A") Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Input Tax].<BR>"
                    '    bTrue = False
                    'End If
                    'If UCase(objExProduct.PayFor) = "OWN CO." And (objExProduct.OutputTax <> "N/A") Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                    '    bTrue = False
                    'End If
                    'If objExProduct.Reimbursement = "Disbursement" And (objExProduct.InputTax <> "N/A") And Not UCase(objExProduct.PayFor) = "HLISB" Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Input Tax].<BR>"
                    '    bTrue = False
                    'End If
                    'If objExProduct.Reimbursement = "Disbursement" And (objExProduct.OutputTax <> "N/A") And Not UCase(objExProduct.PayFor) = "HLISB" Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                    '    bTrue = False
                    'End If

                    If objExProduct.GSTAmount Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    ElseIf objExProduct.GSTAmount = "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    ElseIf CDec(objExProduct.GSTAmount) < CDec(0) Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    End If
                    If objExProduct.InputTax Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Input Tax].<BR>"
                        bTrue = False
                    ElseIf objExProduct.InputTax = "N/A" And Not objExProduct.Reimbursement = "Disbursement" And Not UCase(objExProduct.PayFor) = "HLISB" Then
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Input Tax].<BR>"
                        'bTrue = False
                    End If
                    If objExProduct.OutputTax Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Output Tax].<BR>"
                        bTrue = False
                    ElseIf objExProduct.OutputTax.Contains("N/A") And Not objExProduct.Reimbursement = "Disbursement" And Not UCase(objExProduct.PayFor) = "HLISB" Then
                        'Zulham 07052015 IPP GST Stage 1
                        'TX4 Change Request
                        'If Not (objExProduct.InputTax.ToUpper.Contains("IM1") Or strIsResident = "Y" Or objExProduct.InputTax.ToUpper.Contains("IM3") Or objExProduct.InputTax.ToUpper.Contains("IM2")) Then
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                        'bTrue = False
                        'End If
                    ElseIf objExProduct.InputTax.Contains("(") And Not objExProduct.OutputTax.Contains("(") And Not strIsResident = "Y" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Output Tax].<BR>"
                        bTrue = False
                    ElseIf Not objExProduct.InputTax.Contains("(") And objExProduct.OutputTax.Contains("(") And Not strIsResident = "Y" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Output Tax].<BR>"
                        bTrue = False
                        'Zulham 06/04/2016 - IM5/IM6 Enhancement
                        'OUTPut tax code validation for IM5
                    ElseIf objExProduct.InputTax.Contains("(") And objExProduct.OutputTax.Contains("(") Then
                        If objExProduct.InputTax.Split("(")(1).Substring(0, 1) <> objExProduct.OutputTax.Split("(")(1).Substring(0, 1) Then
                            If Not (objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Or strIsResident = "Y") Then
                                'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Output Tax].<BR>"
                                'bTrue = False
                            End If
                        End If
                    End If

                    If objExProduct.InputTax.Contains("IM5") Then
                        Dim compOutputTaxValue_IM5 = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT_IM6' AND IP_COY_ID = '" & Session("CompanyId") & "'")
                        If Not objExProduct.OutputTax Is Nothing Then
                            If Not objExProduct.OutputTax.Contains(compOutputTaxValue_IM5) Then
                                'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                                'bTrue = False
                            End If
                        End If
                    End If

                    'Zulham 07052015 IPP GST Stage 1
                    'TX4 Change Request
                    'If objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Or strIsResident = "Y" Then
                    If objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Then
                        If objExProduct.OutputTax <> "N/A" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                            bTrue = False
                        End If
                    ElseIf objExProduct.InputTax.ToUpper.Contains("IM2") Then
                        Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
                        percentage = CInt(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) / 100
                        If Not objExProduct.OutputTax.ToUpper.Contains(predefinedOutputTax.ToString.ToUpper) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                            bTrue = False
                        ElseIf CDec(objExProduct.GSTAmount) <> CDec(objExProduct.Amount * percentage) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid GST Amount].<BR>"
                            bTrue = False
                        End If
                    ElseIf objExProduct.InputTax.ToUpper.Contains("TX4") Then
                        Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT'")
                        percentage = CInt(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) / 100
                        If Not objExProduct.OutputTax.ToUpper.Contains(predefinedOutputTax.ToString.ToUpper) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                            bTrue = False
                            'Zulham 07052015 IPP GST Stage 1
                            'TX4 Change Request
                            'Zulham 10/11/2016
                            'Changed the decimal place from 1 to 2
                            'ElseIf FormatNumber(CDec(objExProduct.GSTAmount), 1) <> FormatNumber(CDec(objExProduct.Amount * percentage), 1) Then
                        ElseIf FormatNumber(CDec(objExProduct.GSTAmount), 2) <> FormatNumber(CDec(objExProduct.Amount * percentage), 2) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid GST Amount].<BR>"
                            bTrue = False
                        End If
                    End If

                    If objExProduct.InputTax.Contains("0") And Common.parseNull(objExProduct.GSTAmount).ToString.Trim <> "" Then
                        If CDec(objExProduct.GSTAmount) <> CDec(0.0) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                            bTrue = False
                        End If
                    ElseIf Not objExProduct.InputTax.Contains("(") And Common.parseNull(objExProduct.GSTAmount).ToString.Trim <> "" Then
                        If CDec(objExProduct.GSTAmount) <> CDec(0.0) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                            bTrue = False
                        End If
                    End If

                    If objExProduct.InputTax.Contains("(") Then
                        If Not objExProduct.GSTAmount.Trim.Length = 0 Then
                            If CDec(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) > CDec(0) And CDec(objExProduct.GSTAmount) = CDec(0) Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                                bTrue = False
                                'ElseIf CDec(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) = CDec(0) And CDec(objExProduct.GSTAmount) > CDec(0) Then
                                '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid GST Amount].<BR>"
                                '    bTrue = False
                            End If
                        End If
                    End If

                    'If objExProduct.InputTax.Contains("(") And objExProduct.OutputTax.Contains("(") Then
                    '    If Not (objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Or strIsResident = "Y") Then
                    '        If objExProduct.InputTax.Split("(")(1).Substring(0, 1) <> objExProduct.OutputTax.Split("(")(1).Substring(0, 1) Then
                    '            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Output Tax].<BR>"
                    '            bTrue = False
                    '        End If
                    '    End If
                    'Else
                    '    If Not objExProduct.InputTax.Contains("(") And objExProduct.OutputTax.Contains("(") Then
                    '        If Not (objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Or strIsResident = "Y") Then
                    '            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Selected Input and Output tax must be of the same percentage (%)].<BR>"
                    '            bTrue = False
                    '        End If
                    '    ElseIf objExProduct.InputTax.Contains("(") And Not objExProduct.OutputTax.Contains("(") Then
                    '        If Not (objExProduct.InputTax.ToUpper.Contains("IM1") Or objExProduct.InputTax.ToUpper.Contains("IM3") Or strIsResident = "Y") Then
                    '            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Selected Input and Output tax must be of the same percentage (%)].<BR>"
                    '            bTrue = False
                    '        End If
                    '    End If
                    'End If

                    'If objExProduct.InputTax.ToUpper.Contains("NR") Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Input Tax].<BR>"
                    '    bTrue = False
                    'End If
                ElseIf strIsGst = "No" Then
                    'Jules 2018.07.20 - PAMB allow "\" and "#"
                    Dim documentDate = objDb.GetVal("SELECT IFNULL(im_created_on,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(Request.QueryString("docno")), "\", "\\"), "#", "\#") & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                    'Zulham 19082015
                    Dim residentType As String = ""
                    'Zulham 13/10/2015 - Added condition that is coytype = 'V'
                    If Request.QueryString("vencomp") IsNot Nothing Then residentType = objDb.GetVal("SELECT IFNULL(ic_resident_Type, '') FROM IPP_COMPANY WHERE ic_coy_type = 'V' and ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")

                    If CDate(documentDate) >= CDate(_cutoffDate) Then
                        'Zulham 19082015
                        If Not objExProduct.InputTax Is Nothing Then
                            If objExProduct.InputTax.ToUpper.Contains("NR") Then

                                'Zulham 19082015 - check for disb.reimb for NR
                                If Not objExProduct.Reimbursement.ToUpper.Trim = "N/A" And coyType.Trim.ToUpper = "V" Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Reimbursement/Disbursement selection].<BR>"
                                    bTrue = False
                                End If

                                If objExProduct.OutputTax <> "N/A" Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for SST Input Tax].<BR>"
                                    bTrue = False
                                ElseIf objExProduct.GSTAmount.Trim.Length > 0 Then
                                    If CDec(objExProduct.GSTAmount) > CDec(0) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                End If
                                'Zulham 07052015 IPP GST Stage 1
                                'TX4 Change Request
                                'Zulham 19082015 - Check for coyType
                                'ElseIf Not objExProduct.InputTax.ToUpper.Contains("NR") And coyType.Trim.ToUpper = "V" And Trim(residentType).ToUpper <> "N" Then
                                '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid selection for GST Input Tax].<BR>"
                                '    bTrue = False
                            ElseIf objExProduct.GSTAmount.Trim.Length > 0 Then
                                'Zulham 07052015 IPP GST Stage 1
                                'TX4 Change Request
                                'Zulham 18052015 IPP GST Stage 1
                                'Skip gst Code without percentage
                                If objExProduct.InputTax.Contains("(") Then
                                    'Zulham 12112018
                                    percentage = objTax.getTaxPercentage(objExProduct.InputTax.Split("(")(0))
                                    If Not percentage.Trim = "0" Then
                                        percentage = percentage / 100
                                    End If
                                    'CInt(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) / 100

                                    'Zulham 10/01/2016 - Changed the decimal place from 1 to 2 
                                    'If FormatNumber(CDec(objExProduct.GSTAmount), 1) > FormatNumber(CDec(objExProduct.Amount * percentage), 1) Then
                                    If FormatNumber(CDec(objExProduct.GSTAmount), 2) <> FormatNumber(CDec(objExProduct.Amount * percentage), 2) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                Else
                                    If Not Format(CDec(objExProduct.GSTAmount), 2) = Format(CDec(0), 2) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                    'End If
                End If
                'End

                ' Check UOM
                If objExProduct.UOM <> "" Then
                    strSQL2 = "SELECT code_desc FROM code_mstr WHERE code_category = 'UOM' AND code_desc = '" & Common.Parse(objExProduct.UOM) & "' "
                    Dim UOM_Val As String
                    UOM_Val = objDb2.GetVal(strSQL2)
                    If UOM_Val = "" Then 'no exist
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00351") & "]<BR>"
                        bTrue = False
                    Else
                        ' Session("UOMVal") = UOM_Val
                        objExProduct.UOM = UOM_Val

                    End If
                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [UOM " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                    bTrue = False
                End If

                If Not CType(objExProduct.Amount, String) = "" Then
                    If objExProduct.Amount <> 0 Then
                        'Check quantity
                        If objExProduct.Quantity.ToString = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Quantity " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                            'ElseIf objExProduct.Quantity = 0 Then
                            '    GoTo kkk
                        End If

                        'Check unit price
                        If objExProduct.UnitPrice = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Unit Price " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                            'ElseIf objExProduct.UnitPrice = 0 Then
                            '    GoTo kkk
                        End If

                        'Check amount
                        If objExProduct.Amount.ToString = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Amount " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                            'ElseIf objExProduct.Amount = 0 Then
                        ElseIf CType(objExProduct.Amount.ToString, Double) < 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Amount must not be in negative value.]<BR>"
                            bTrue = False
                        End If
                    Else
                        GoTo kkk
                    End If
                Else
                    GoTo kkk
                End If

                'Check for GL Code  
                If objExProduct.GLCode <> "" Then
                    'IPP Gst Stage 2A - CH - 11 Feb 2015
                    If strDefIPPCompID = "" Then
                        strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & Session("CompanyId") & "' "
                    Else
                        strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & strDefIPPCompID & "' "
                    End If
                    'strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & Session("CompanyId") & "' "
                    Dim CBG_B_GL_Code As String
                    CBG_B_GL_Code = objDb2.GetVal(strSQL2)
                    If CBG_B_GL_Code = "" Then 'no exist
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00353") & "]<BR>"
                        bTrue = False
                    Else
                        'Session("CBG_B_GL_Code") = CBG_B_GL_Code
                        objExProduct.GLCode = CBG_B_GL_Code
                    End If

                    'mimi 2018-04-24 : remove hardcode HLB to PAMB
                    'If UCase(Session("PayForVal")) <> "HLB" And UCase(Session("PayForVal")) <> "HLISB" Then
                    If UCase(Session("PayForVal")) <> "PAMB" Then
                        'Session("CBG_B_GL_Code") = objDb.GetVal("SELECT ic_con_ibs_code FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE ic_other_b_coy_code = '" & Session("PayForVal") & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                        objExProduct.GLCode = objDb.GetVal("SELECT ic_con_ibs_code FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE ic_other_b_coy_code = '" & Session("PayForVal") & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                    End If

                    'Jules 2018.07.19 - PAMB
                    If objExProduct.Gift <> "" Then
                        Dim strGLType = ""
                        strGLType = objDb.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & objExProduct.GLCode.Trim & "'")
                        If strGLType = "CAP" AndAlso objExProduct.Gift.Contains("Y") Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [CAPEX item cannot be Gift.]<BR>"
                            bTrue = False
                        End If
                    End If

                    Dim strSqlAC As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE2, '') AS CBGCAC_ANALYSIS_CODE2, IFNULL(CBGCAC_ANALYSIS_CODE3, '') AS CBGCAC_ANALYSIS_CODE3, IFNULL(CBGCAC_ANALYSIS_CODE4, '') AS CBGCAC_ANALYSIS_CODE4, IFNULL(CBGCAC_ANALYSIS_CODE5, '') AS CBGCAC_ANALYSIS_CODE5, " &
                                            "IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & objExProduct.GLCode & "'"
                    Dim dsAnalysisCodes As DataSet = objDb.FillDs(strSqlAC)
                    If dsAnalysisCodes.Tables(0).Rows.Count > 0 Then
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Fund Type " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE2").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Product Type " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE3").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Channel " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE4").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Reinsurance Company " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE5").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Asset Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And objExProduct.ProjectCode = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Project Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And objExProduct.PersonCode = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Person Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                    End If

                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [GL Code " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                    bTrue = False
                End If

                'Jules 2018.07.19 - PAMB - Removed Sub Description.
                ''Rules Category/Sub Description
                'Dim resultCat As Object
                'Dim resultCatMis = ""
                'If objExProduct.RulesCategory = "" Then
                '    resultCat = objDb.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                '    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                '    "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' AND igg_gl_code LIKE '" & objExProduct.GLCode & "' limit 1")
                'Else
                '    Dim str = ("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                '                        " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                '                        "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' AND igg_gl_code LIKE '" & objExProduct.GLCode & "' and igc_glrule_category like '" & objExProduct.RulesCategory & "'")

                '    resultCat = objDb.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                '                        " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                '                        "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' AND igg_gl_code LIKE '" & objExProduct.GLCode & "' and igc_glrule_category like '" & objExProduct.RulesCategory & "'")
                'End If
                ''mismatched gl entry and cat. code
                'If resultCat = "" Then
                '    resultCatMis = objDb.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                '    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                '    "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' AND igg_gl_code LIKE '" & objExProduct.GLCode & "' limit 1")
                'End If

                'If resultCat <> "" And objExProduct.RulesCategory = "" Then
                '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Sub Description for " & objExProduct.GLCode & " is required]<BR>"
                '    bTrue = False
                'ElseIf resultCat <> "" And objExProduct.RulesCategory <> "" Then
                '    ViewState("igc_glrule_category_index") = resultCat
                'ElseIf resultCatMis = "" And objExProduct.RulesCategory <> "" Then
                '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Sub Description for " & objExProduct.GLCode & " is invalid]<BR>"
                '    bTrue = False
                'ElseIf resultCatMis <> "" And resultCat = "" Then
                '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Sub Description for " & objExProduct.GLCode & " is invalid]<BR>"
                '    bTrue = False
                'Else
                '    If resultCat = "" Then resultCat = 0
                'End If
                ''End
                'End commented block.

                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If objExProduct.CostAllocation = "" Then
                    'Check for HO/BR
                    'If objExProduct.HOBR <> "" And UCase(objExProduct.PayFor) = "HLB" Then
                    If objExProduct.HOBR <> "" And UCase(objExProduct.PayFor) = "PAMB" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & Common.Parse(objExProduct.HOBR) & "'") = 0 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            End If

                        Else
                            strSQL2 = "SELECT cbm_branch_code FROM company_branch_mstr WHERE cbm_coy_id = '" & Session("CompanyId") & "' AND cbm_status = 'A' and cbm_branch_code = '" & Common.Parse(objExProduct.HOBR) & "' "

                            Dim HOBRCode As String
                            HOBRCode = objDb2.GetVal(strSQL2)
                            If HOBRCode = "" Then 'no exist
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            Else
                                objExProduct.HOBR = HOBRCode
                            End If
                        End If

                        If objExProduct.GLCode <> "" And objExProduct.HOBR <> "" Then

                            If InStr(1, objExProduct.GLCode.ToString, "7") = 1 _
                            And objDb.GetVal("SELECT cbm_branch_type FROM company_branch_mstr WHERE cbm_branch_code = '" & Common.Parse(objExProduct.HOBR) & "' and cbm_status = 'A' and cbm_coy_id = '" & objExProduct.PayFor & "'") = "HO" _
                                And objExProduct.HOBR.Trim = "900" _
                                And Common.parseNull(objExProduct.CostCenter) = "" Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                                bTrue = False
                            ElseIf objExProduct.CostCenter <> "" Then
                                'Check for Cost Center validity
                                strSQL2 = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " &
                                            "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " &
                                            "WHERE BCC_COY_ID = '" & UCase(objExProduct.PayFor) & "' AND BCC_BRANCH_CODE = '" & Common.Parse(objExProduct.HOBR) & "' " &
                                            "ORDER BY CC_CC_CODE "
                                dsTiedCC = objDb.FillDs(strSQL2)
                                If dsTiedCC.Tables(0).Rows.Count > 0 Then
                                    For record As Integer = 0 To dsTiedCC.Tables(0).Rows.Count - 1
                                        If dsTiedCC.Tables(0).Rows(record).Item(0) = objExProduct.CostCenter Then
                                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Cost Center]<BR>"
                                            bTrue = False
                                        End If
                                    Next
                                End If
                                'Check for Cost Center
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    End If
                                Else
                                    strSQL2 = "SELECT cc_cc_code FROM cost_centre WHERE cc_status = 'A' AND cc_coy_id = '" & Common.Parse(objExProduct.PayFor) & "' AND cc_cc_code = '" & Common.Parse(objExProduct.CostCenter) & "' "
                                    Dim CostCenter As String
                                    CostCenter = objDb2.GetVal(strSQL2)
                                    If CostCenter = "" Then 'no exist
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    Else
                                        objExProduct.CostCenter = CostCenter
                                    End If
                                End If
                            ElseIf objExProduct.CostCenter = "" Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                                        bTrue = False
                                    End If
                                Else
                                    objExProduct.CostCenter = ""
                                End If
                            Else
                                objExProduct.CostCenter = ""
                            End If
                        End If
                    ElseIf objExProduct.HOBR <> "" And UCase(objExProduct.PayFor) = "HLISB" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & Common.Parse(objExProduct.HOBR) & "'") = 0 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            End If
                        Else
                            strSQL2 = "SELECT cbm_branch_code FROM company_branch_mstr WHERE cbm_coy_id = '" & objExProduct.PayFor & "' AND cbm_status = 'A' and cbm_branch_code = '" & Common.Parse(objExProduct.HOBR) & "' "

                            Dim HOBRCode As String
                            HOBRCode = objDb2.GetVal(strSQL2)
                            If HOBRCode = "" Then 'no exist
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            Else
                                objExProduct.HOBR = HOBRCode
                            End If
                        End If

                        If objExProduct.CostCenter <> "" Then
                            'Check for Cost Center validity
                            If InStr(objExProduct.CostCenter, ":") Then
                                costCentre = objExProduct.CostCenter.ToString.Split(":")(0).Trim
                            Else
                                costCentre = objExProduct.CostCenter.ToString.Trim
                            End If

                            If InStr(objExProduct.HOBR, ":") Then
                                branchCode = objExProduct.HOBR.ToString.Split(":")(0).Trim
                            Else
                                branchCode = objExProduct.HOBR.ToString.Trim
                            End If

                            strSQL2 = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " &
                                        "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " &
                                        "WHERE BCC_COY_ID = '" & UCase(objExProduct.PayFor) & "' AND BCC_BRANCH_CODE = '" & Common.Parse(branchCode) & "' " &
                                        "ORDER BY CC_CC_CODE "
                            dsTiedCC = objDb.FillDs(strSQL2)
                            If dsTiedCC.Tables(0).Rows.Count > 0 Then
                                For record As Integer = 0 To dsTiedCC.Tables(0).Rows.Count - 1
                                    If dsTiedCC.Tables(0).Rows(record).Item(0) = costCentre Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Cost Center]<BR>"
                                        bTrue = False
                                    End If
                                Next
                            End If

                            'Check for Cost Center
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                    bTrue = False
                                End If
                            Else
                                'strSQL2 = "SELECT cc_cc_code FROM cost_centre WHERE cc_status = 'A' AND cc_coy_id = '" & Session("CompanyId") & "' AND cc_cc_code = '" & Common.Parse(objExProduct.CostCenter) & "' "
                                strSQL2 = "SELECT cc_cc_code FROM cost_centre WHERE cc_status = 'A' AND cc_coy_id = '" & Common.Parse(objExProduct.PayFor) & "' AND cc_cc_code = '" & Common.Parse(objExProduct.CostCenter) & "' "
                                Dim CostCenter As String
                                CostCenter = objDb2.GetVal(strSQL2)
                                If CostCenter = "" Then 'no exist
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                    bTrue = False
                                Else
                                    objExProduct.CostCenter = CostCenter
                                End If
                            End If
                        ElseIf objExProduct.CostCenter = "" Then
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                                    bTrue = False
                                End If
                            Else
                                objExProduct.CostCenter = ""
                            End If
                        End If
                        'mimi 2018-04-24 : remove hardcode HLB to PAMB
                    ElseIf objExProduct.HOBR <> "" And (UCase(objExProduct.PayFor) <> "PAMB") Then
                        'ElseIf objExProduct.HOBR <> "" And (UCase(objExProduct.PayFor) <> "HLB" And UCase(objExProduct.PayFor) <> "HLISB") Then
                        objExProduct.HOBR = ""

                        'Jules 2018.07.19 - PAMB - Removed HO/BR
                        'ElseIf objExProduct.HOBR = "" And (UCase(objExProduct.PayFor) = "PAMB") Then
                        '    'ElseIf objExProduct.HOBR = "" And (UCase(objExProduct.PayFor) = "HLB" Or UCase(objExProduct.PayFor) = "HLISB") Then
                        '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [HO/BR " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                        '    bTrue = False
                    End If
                Else
                    If objExProduct.CostCenter <> "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Center is not allowed if Cost Alloction has been assigned.]<BR>"
                        bTrue = False
                    End If
                    If objExProduct.HOBR <> "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [HO/BR is not allowed if Cost Alloction has been assigned.]<BR>"
                        bTrue = False
                    End If
                    If objExProduct.CostAllocation <> "" Then
                        strSQL2 = "SELECT cam_ca_code FROM cost_alloc_mstr WHERE cam_coy_id = '" & Session("CompanyId") & "' AND cam_ca_code = '" & Common.Parse(objExProduct.CostAllocation) & "' "
                        Dim CostAlloc As String
                        CostAlloc = objDb2.GetVal(strSQL2)
                        If CostAlloc = "" Then 'no exist
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00357") & "]<BR>"
                            bTrue = False
                        Else
                            'Session("CostAllocVal") = CostAlloc
                            objExProduct.CostAllocation = CostAlloc
                        End If

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        If objExProduct.GLCode <> "" And objExProduct.CostAllocation <> "" Then
                            Dim decTtlPct As Decimal
                            Dim intCostAllocIndex As Integer
                            Dim dsCAD As DataSet
                            Dim k As Integer

                            intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & Common.Parse(objExProduct.CostAllocation) & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"))

                            decTtlPct = objIPP.GetSUMCostAllocDetail(intCostAllocIndex)

                            If decTtlPct < 100 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00052") & "]<BR>"
                                bTrue = False
                            End If

                            dsCAD = objIPP.CheckCostAllocDetail(intCostAllocIndex)

                            For k = 0 To dsCAD.Tables(0).Rows.Count - 1
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Allocation Line " & k + 1 & " - " & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                        bTrue = False
                                    End If
                                End If
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Allocation  Line " & k + 1 & " - " & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    End If
                                End If

                                If InStr(1, objExProduct.GLCode.ToString, "7") = 1 _
                                And objDb.GetVal("SELECT cbm_branch_type FROM company_branch_mstr WHERE cbm_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString & "' and cbm_status = 'A' and cbm_coy_id = '" & objExProduct.PayFor & "'") = "HO" _
                                    And dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString = "900" _
                                    And Common.parseNull(dsCAD.Tables(0).Rows(k)("CAD_CC_CODE")) = "" Then

                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Allocation Line " & k + 1 & " - Cost Center is required.]<BR>"
                                    bTrue = False
                                End If

                            Next
                        End If
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Else
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Allocation required.]<BR>"
                        bTrue = False
                    End If

                End If

                'Jules 2018.08.01 - Validation for Cost Center - Logic taken from IPPEntryPop.aspx.vb
                If objExProduct.CostCenter <> "" Then
                    If InStr(objExProduct.CostCenter, ":") Then
                        objExProduct.CostCenter = objExProduct.CostCenter.Substring(0, InStr(objExProduct.CostCenter, ":") - 1)
                    End If
                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [You have no permission to use this Cost Center.]<BR>"
                            bTrue = False
                        End If
                    Else
                        If Not objExProduct.CostCenter.ToString.Trim = "" Then
                            Dim getSQL = "SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & objExProduct.CostCenter.ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
                            If objDb.Exist("SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & objExProduct.CostCenter.ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") = 0 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid Cost Center.]<BR>"
                                bTrue = False
                            End If
                        End If
                    End If

                Else 'to prevent user no enter for CC without permission to charge 000 
                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Cost Center is required.]<BR>"
                            bTrue = False
                        End If
                    End If
                    'End If
                End If

                'Jules 2018.08.01 - Validation for Withholding Tax Option and Withholding Tax
                If objExProduct.WHTOption = "" Then

                Else
                    'If objExProduct.WHTTax = "" Then
                    '    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Withholding Tax is empty.]<BR>"
                    '    bTrue = False
                    'Else
                    If objExProduct.WHTOption.ToUpper.Contains("COMPANY") OrElse objExProduct.WHTOption.ToUpper.Contains("VENDOR") Then
                        If objExProduct.WHTTax = "" OrElse Not IsNumeric(objExProduct.WHTTax) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Withholding Tax is invalid.]<BR>"
                            bTrue = False
                        End If
                    End If
                    'End If
                End If

                ''Zulham Feb 4, 2014
                Dim dsItemDetails As New DataSet
                dsItemDetails = bindItems(objExProduct)

                If bTrue = False Then
                    bAllTrue = False
                    bAllColTrue = False
                End If
                'Next

                If bAllColTrue = True Then
                    If _exceedCutOffDt = "No" Then
                        If objIPP.SaveMultiGLDebits(dsItemDetails, dsHeader, strDocNo, strOldDocNo, strVendor, strAryQuery, ViewState("igc_glrule_category_index")) Then ', SaveHeader, ViewState("Index")) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    ElseIf _exceedCutOffDt = "Yes" Then
                        If objIPP.SaveMultiGLDebits(dsItemDetails, dsHeader, strDocNo, strOldDocNo, strVendor, strAryQuery, ViewState("igc_glrule_category_index"), "Yes") Then ', SaveHeader, ViewState("Index")) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    End If
                Else
                    countError = countError + 1
                End If

                objExProduct = Nothing
            End If
kkk:
        Next

        If bAllTrue = True Then
            objDb.BatchExecute(strAryQuery)
            'Insert into ipp_sub_doc
            If Not dsSubDoc Is Nothing Then
                Dim invoiceindex = 0
                Dim arylist As New ArrayList
                For i As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                    If Not strIsGst = "No" Then
                        arylist.Add(New String() {i + 1, dsSubDoc.Tables(0).Rows(i).Item(0), dsSubDoc.Tables(0).Rows(i).Item(1), dsSubDoc.Tables(0).Rows(i).Item(2), "", dsSubDoc.Tables(0).Rows(i).Item(3)})
                    Else
                        arylist.Add(New String() {i + 1, dsSubDoc.Tables(0).Rows(i).Item(0), dsSubDoc.Tables(0).Rows(i).Item(1), dsSubDoc.Tables(0).Rows(i).Item(2), "", 0.0})
                    End If
                Next
                If strDocNo <> strOldDocNo Then
                    invoiceindex = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(strOldDocNo, "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
                Else
                    invoiceindex = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")), "\", "\\"), "#", "\#") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
                End If
                objIPP.SaveIPPSubDoc(arylist, invoiceindex)
                Session("SubDocument") = arylist
            End If
            'End
        Else
            Session("SubDocument") = Nothing
        End If

        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strscript.Append("<script language=""javascript"">")
        strscript.Append("UnBlockUploadProgress();")
        strscript.Append("</script>")
        RegisterStartupScript("script5", strscript.ToString())

        Common.NetMsgbox(Me, "Successfull: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

    End Function

    Private Sub Clear()
        ViewState("Index") = 0
        ViewState("ProductCode") = ""
        ViewState("VendorItemCode") = ""
        ViewState("ProductDesc") = ""
        ViewState("UOM") = ""
    End Sub

    Private Function bindItems(ByRef pGLEntryDetails As ExcelIPPMultiGLDebits) As DataSet
        Dim ds As New DataSet
        Dim dtItem As New DataTable
        Dim dtr As DataRow

        'Contract Items
        dtItem.Columns.Add("ItemLine", Type.GetType("System.Int32"))
        dtItem.Columns.Add("PayFor", Type.GetType("System.String"))        
        dtItem.Columns.Add("ItemDesc", Type.GetType("System.String"))
        dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        dtItem.Columns.Add("Quantity", Type.GetType("System.String"))
        dtItem.Columns.Add("UnitCost", Type.GetType("System.String"))
        dtItem.Columns.Add("GLCode", Type.GetType("System.String"))
        dtItem.Columns.Add("HOBR", Type.GetType("System.String"))
        dtItem.Columns.Add("CostCenter", Type.GetType("System.String"))
        dtItem.Columns.Add("CostAlloc", Type.GetType("System.String"))
        dtItem.Columns.Add("RulesCategory", Type.GetType("System.String"))
        dtItem.Columns.Add("InputTax", Type.GetType("System.String"))
        dtItem.Columns.Add("OutputTax", Type.GetType("System.String"))
        dtItem.Columns.Add("GSTAmount", Type.GetType("System.String"))
        dtItem.Columns.Add("Reimbursement", Type.GetType("System.String"))

        'Jules 2018.07.19 - PAMB
        dtItem.Columns.Add("Gift", Type.GetType("System.String"))
        dtItem.Columns.Add("Category", Type.GetType("System.String"))
        dtItem.Columns.Add("FundType", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductType", Type.GetType("System.String"))
        dtItem.Columns.Add("Channel", Type.GetType("System.String"))
        dtItem.Columns.Add("ReinsuranceCo", Type.GetType("System.String"))
        dtItem.Columns.Add("AssetCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProjectCode", Type.GetType("System.String"))
        dtItem.Columns.Add("PersonCode", Type.GetType("System.String"))
        dtItem.Columns.Add("WHTOption", Type.GetType("System.String"))
        dtItem.Columns.Add("WHTTax", Type.GetType("System.String"))
        'End modification.

        dtr = dtItem.NewRow()
        If _exceedCutOffDt = "No" Then
            dtr("ItemLine") = pGLEntryDetails.No
            dtr("PayFor") = Common.Parse(pGLEntryDetails.PayFor)
            dtr("ItemDesc") = Common.Parse(pGLEntryDetails.Description)
            dtr("UOM") = Common.Parse(pGLEntryDetails.UOM)
            dtr("Quantity") = Common.Parse(pGLEntryDetails.Quantity)
            dtr("UnitCost") = Common.Parse(pGLEntryDetails.UnitPrice)
            dtr("GLCode") = Common.parseNull(pGLEntryDetails.GLCode)
            dtr("HOBR") = Common.Parse(pGLEntryDetails.HOBR)
            dtr("CostCenter") = Common.Parse(Common.parseNull(pGLEntryDetails.CostCenter))
            dtr("CostAlloc") = Common.Parse("")
            dtr("RulesCategory") = Common.Parse(Common.parseNull(pGLEntryDetails.RulesCategory))
            dtr("InputTax") = ""
            dtr("OutputTax") = ""
            dtr("GSTAmount") = 0.0
            dtr("Reimbursement") = "N/A"
        ElseIf _exceedCutOffDt = "Yes" Then
            dtr("ItemLine") = pGLEntryDetails.No
            dtr("PayFor") = Common.Parse(pGLEntryDetails.PayFor)
            dtr("ItemDesc") = Common.Parse(pGLEntryDetails.Description)
            dtr("UOM") = Common.Parse(pGLEntryDetails.UOM)
            dtr("Quantity") = Common.Parse(pGLEntryDetails.Quantity)
            dtr("UnitCost") = Common.Parse(pGLEntryDetails.UnitPrice)
            dtr("GLCode") = Common.parseNull(pGLEntryDetails.GLCode)
            dtr("HOBR") = Common.Parse(pGLEntryDetails.HOBR)
            dtr("CostCenter") = Common.Parse(Common.parseNull(pGLEntryDetails.CostCenter))
            dtr("CostAlloc") = Common.Parse("")
            dtr("RulesCategory") = Common.Parse(Common.parseNull(pGLEntryDetails.RulesCategory))
            dtr("InputTax") = Common.parseNull(pGLEntryDetails.InputTax)
            dtr("OutputTax") = Common.Parse(pGLEntryDetails.OutputTax)
            dtr("GSTAmount") = Common.Parse(IIf(Common.parseNull(pGLEntryDetails.GSTAmount.Trim) = "", 0, pGLEntryDetails.GSTAmount))
            dtr("Reimbursement") = Common.Parse(Common.parseNull(pGLEntryDetails.Reimbursement))
        End If

        'Jules 2018.07.19 - PAMB
        If Common.parseNull(pGLEntryDetails.Gift) = "" Then
            dtr("Gift") = "N"
        Else
            dtr("Gift") = Common.Parse(pGLEntryDetails.Gift).ToString.Substring(0, 1)
        End If
        'dtr("Gift") = IIf(Common.parseNull(pGLEntryDetails.Gift) = "", "N", Common.Parse(pGLEntryDetails.Gift).ToString.Substring(0, 1))
        'dtr("Category") = Common.Parse(pGLEntryDetails.Category)
        If Common.parseNull(pGLEntryDetails.Category) = "" Then
            dtr("Category") = ""
        Else
            dtr("Category") = Common.Parse(pGLEntryDetails.Category)
        End If

        If InStr(Common.Parse(pGLEntryDetails.FundType), ":") Then
            dtr("FundType") = Common.Parse(pGLEntryDetails.FundType).Substring(0, InStr(Common.Parse(pGLEntryDetails.FundType), ":") - 1).Trim
        Else
            dtr("FundType") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.ProductType), ":") Then
            dtr("ProductType") = Common.Parse(pGLEntryDetails.ProductType).Substring(0, InStr(Common.Parse(pGLEntryDetails.ProductType), ":") - 1).Trim
        Else
            dtr("ProductType") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.Channel), ":") Then
            dtr("Channel") = Common.Parse(pGLEntryDetails.Channel).Substring(0, InStr(Common.Parse(pGLEntryDetails.Channel), ":") - 1).Trim
        Else
            dtr("Channel") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.ReinsuranceCo), ":") Then
            dtr("ReinsuranceCo") = Common.Parse(pGLEntryDetails.ReinsuranceCo).Substring(0, InStr(Common.Parse(pGLEntryDetails.ReinsuranceCo), ":") - 1).Trim
        Else
            dtr("ReinsuranceCo") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.AssetCode), ":") Then
            dtr("AssetCode") = Common.Parse(pGLEntryDetails.AssetCode).Substring(0, InStr(Common.Parse(pGLEntryDetails.AssetCode), ":") - 1).Trim
        Else
            dtr("AssetCode") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.ProjectCode), ":") Then
            dtr("ProjectCode") = Common.Parse(pGLEntryDetails.ProjectCode).Substring(0, InStr(Common.Parse(pGLEntryDetails.ProjectCode), ":") - 1).Trim
        Else
            dtr("ProjectCode") = ""
        End If

        If InStr(Common.Parse(pGLEntryDetails.PersonCode), ":") Then
            dtr("PersonCode") = Common.Parse(pGLEntryDetails.PersonCode).Substring(0, InStr(Common.Parse(pGLEntryDetails.PersonCode), ":") - 1).Trim
        Else
            dtr("PersonCode") = ""
        End If

        If Common.parseNull(pGLEntryDetails.WHTOption) <> "" Then
            If Common.Parse(pGLEntryDetails.WHTOption).ToUpper.Contains("COMPANY") Then
                dtr("WHTOption") = "1"
            ElseIf Common.Parse(pGLEntryDetails.WHTOption).ToUpper.Contains("VENDOR") Then
                dtr("WHTOption") = "2"
            Else
                dtr("WHTOption") = "3"
            End If
            dtr("WHTTax") = Common.Parse(pGLEntryDetails.WHTTax)
        Else
            dtr("WHTOption") = ""
            dtr("WHTTax") = ""
        End If
        'End modification.

        dtItem.Rows.Add(dtr)
        ds.Tables.Add(dtItem)

        bindItems = ds
    End Function

    Private Function bindHeader(ByVal MultiGLHeader As ExcelIPPMultiGLDebitsHeader) As DataSet
        Dim ds As New DataSet
        Dim dtHeader As New DataTable
        Dim dtr As DataRow

        'Contract Header
        dtHeader.Columns.Add("VendorID", Type.GetType("System.String"))
        dtHeader.Columns.Add("DocNo", Type.GetType("System.String"))
        dtHeader.Columns.Add("DocDate", Type.GetType("System.DateTime"))

        dtr = dtHeader.NewRow()
        dtr("VendorID") = ViewState("VCoyId")
        dtr("DocNo") = Common.Parse(MultiGLHeader.DocNo)
        dtr("DocDate") = Common.Parse(MultiGLHeader.DocDate)
        dtHeader.Rows.Add(dtr)
        ds.Tables.Add(dtHeader)

        bindHeader = ds
    End Function

    Private Function GetItemDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelIPPMultiGLDebits
        Dim objExcel As New ExcelIPPMultiGLDebits

        With objExcel
            If _exceedCutOffDt = "No" Then
                'if GST
                objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
                objExcel.PayFor = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
                objExcel.Description = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
                objExcel.UOM = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
                objExcel.Quantity = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
                objExcel.UnitPrice = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))

                objExcel.Amount = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
                'Zulham 18102018 - PAMB
                objExcel.GLCode = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8").ToString.Split(":")(1))

                'Jules 2018.07.19 - PAMB
                'objExcel.RulesCategory = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
                'objExcel.HOBR = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
                objExcel.CostAllocation = ""
                objExcel.RulesCategory = ""
                objExcel.HOBR = ""
                'Zulham 18102018 - PAMB
                objExcel.CostCenter = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11").ToString.Split(":")(1))
                objExcel.Gift = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12"))
                objExcel.Category = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))
                objExcel.FundType = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
                objExcel.ProductType = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
                objExcel.Channel = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
                objExcel.ReinsuranceCo = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
                objExcel.AssetCode = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))
                objExcel.ProjectCode = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))
                objExcel.PersonCode = IIf(IsDBNull(pdr.Item("F20")), "", pdr.Item("F20"))
                objExcel.WHTOption = IIf(IsDBNull(pdr.Item("F21")), "", pdr.Item("F21"))
                objExcel.WHTTax = IIf(IsDBNull(pdr.Item("F22")), "", pdr.Item("F22"))
                'End modification.
            ElseIf _exceedCutOffDt = "Yes" Then
                'if not GST
                objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
                objExcel.PayFor = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))
                objExcel.Reimbursement = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
                objExcel.Description = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))

                'Zulham 04102018- PAMB SST
                objExcel.UOM = "Unit"
                objExcel.Quantity = 1
                objExcel.UnitPrice = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
                objExcel.Amount = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
                objExcel.GSTAmount = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))
                objExcel.InputTax = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
                objExcel.OutputTax = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
                'Zulham 18102018 - PAMB 
                objExcel.GLCode = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9").ToString.Split(":")(1))


                'Jules 2018.07.19 - PAMB
                'objExcel.RulesCategory = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))
                'objExcel.HOBR = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
                'objExcel.CostCenter = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
                objExcel.CostAllocation = ""
                objExcel.RulesCategory = ""
                objExcel.HOBR = ""

                'Zulham 18102018 - PAMB SST
                objExcel.CostCenter = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10").ToString.Split(":")(1))
                objExcel.Gift = "No"
                objExcel.Category = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
                objExcel.FundType = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12"))
                objExcel.ProductType = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))
                objExcel.Channel = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
                objExcel.ReinsuranceCo = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
                objExcel.AssetCode = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
                objExcel.ProjectCode = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
                objExcel.PersonCode = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))
                objExcel.WHTOption = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))
                objExcel.WHTTax = IIf(IsDBNull(pdr.Item("F20")), "", pdr.Item("F20"))
                'End modification.
            End If
        End With
        Return objExcel

    End Function



    Private Function GetMultiGLHeader(ByVal dt As DataTable) As ExcelIPPMultiGLDebitsHeader
        Dim objExcel As New ExcelIPPMultiGLDebitsHeader

        With objExcel

            objExcel.CoyName = IIf(IsDBNull(dt.Rows(0).Item("CoyName")), "", dt.Rows(0).Item("CoyName"))
            objExcel.DocNo = IIf(IsDBNull(dt.Rows(0).Item("DocNo")), "", dt.Rows(0).Item("DocNo"))
            objExcel.DocDate = IIf(IsDBNull(dt.Rows(0).Item("DocDate")), "", dt.Rows(0).Item("DocDate"))

        End With
        Return objExcel

    End Function
    Private Function getUserRole(ByVal blnIPPOfficer As Boolean, ByVal blnIPPOfficerS As Boolean) As Integer
        If blnIPPOfficer = False And blnIPPOfficerS = True Then
            getUserRole = 2
        ElseIf blnIPPOfficer = True And blnIPPOfficerS = False Then
            getUserRole = 1
        End If
    End Function
End Class