Imports System.io
Imports eProcure.Component
Imports AgoraLegacy

Imports System.Text.RegularExpressions

Public Class IPPInvoicesBatchUpload
    Inherits AgoraLegacy.AppBaseClass

    Dim objIPP As New IPPMain
    Dim objExMultiGLHeader As New ExcelIPPMultiGLDebitsHeader
    Dim strVendor, strDocNo, strOldDocNo, strDocType As String
    Dim objGlobal As New AppGlobals
    Dim strIsGst As String
    Dim _exceedCutOffDt As String = ""
    Dim strIsResident As String = ""
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
    'Zulham 22042019
    Dim strVenType As String = ""
    'Zulham 30092019
    Dim isPayFor As Boolean = False

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
        'Zulham 22042019
        strVenType = Request.QueryString("venType")
        Me.cmdUpload.Attributes.Add("onClick", "UploadProgress();")

        'Zulham 05042019
        Session("MultiInvoices") = "Y"

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
        Dim globals As New AppGlobals
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
        Dim proc As System.Diagnostics.Process
        Dim intPID As Integer
        Dim dsCurrency As New DataSet

        ViewState("mode") = Request.QueryString("mode")

        Dim dsRuleCategory, dsGLCode As DataSet
        Dim strSql, strAddr As String
        Dim dvCF As DataView

        filename = "MultipleInvoices.xls"

        Dim objGLCode As New IPP
        Dim strGLCode, strGLDesc, strGLStatus As String
        strGLStatus = "A"
        dsGLCode = objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)

        dsinput = GST.GetTaxCode_forIPP("", "P")
        dsoutput = GST.GetTaxCode_forIPP("", "S")

        'Zulham 18102018 - PAMB
        Dim dsCC As New DataSet
        dsCC = objGLCode.getCostCentreMultiGL()

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

        dsCurrency = globals.getCurrencyAbbr()
        'Zulham 27032019
        objEx.WriteCell_IPPUpload(dsRuleCategory, ConfigurationManager.AppSettings("TemplateTemp") & filename, Request.QueryString("mstrDoc"), dsGLCode, dsinput, dsoutput, dsFundType, dsProductType, dsChannel, dsReinsuranceCo, dsAssetCode, dsProjectCode, dsPersonCode, dsPayFor, dsCC, lblDocNo.Text.Trim, Request.QueryString("vencomp"), lblDocDate.Text.Trim, from:="multiInvoices", dsCurrency:=dsCurrency)

        Dim _fileName = "MultipleInvoices.xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition",
          "attachment; filename=""" & _fileName & """")
        Response.Flush()
        Response.WriteFile(ConfigurationManager.AppSettings("TemplateTemp") & "MultipleInvoices.xls")

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
            Dim dsHeader As DataSet
            Dim objCompany As New Companies
            Dim dsDuplicate As New DataSet
            Clear()


            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings("eProcurePath"))

            If IsExcel(cmdBrowse.PostedFile.FileName) Then
                Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'IPP' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= Session("FileSize") Then

                    strIsGst = "Yes"
                    _exceedCutOffDt = "Yes"

                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    ds = objEx.ReadIPPMultiInvoiceExcelFormat(Server.MapPath("../xml/MultiInvoice.xml"), strTempPath & strFileName, cRules, dsHeader, version, _exceedCutOffDt, dsDuplicate)

                    'check for duplicates
                    If Not checkDuplicates(dsDuplicate.Tables(0)) Then Exit Sub

                    Dim strColumns() As String = {"f2", "f3", "f1"}
                    Dim dt0 As DataTable = ds.Tables(0).DefaultView.ToTable(True, strColumns)

                    'Zulham 19042019
                    If Not dt0.Rows.Count = 0 Then
                        ViewState("dsHeader") = dt0
                        If Not addInvoiceMstr(dt0) Then Exit Sub
                    End If

                    If Not ds Is Nothing Then
                        ds.Tables(0).Columns.Add("Message", Type.GetType("System.String"))
                        If (Not Directory.Exists(strDestPath)) Then
                            Directory.CreateDirectory(strDestPath)
                        End If
                        File.Move(strTempPath & strFileName, strDestPath & GetNewFileName(strFileName))

                        'Zulham 08072019
                        'Check mandatory fields
                        If Not valMandatoryFields(ds.Tables(0)) Then
                            If Not ViewState("dsHeader") Is Nothing Then
                                Dim dsTemp As New DataTable : dsTemp = ViewState("dsHeader")
                                If Not dsTemp.Rows.Count = 0 Then
                                    For Each row As DataRow In dsTemp.Rows
                                        If Not row(0) Is DBNull.Value And Not row(1) Is DBNull.Value Then
                                            objIPP.deleteMultiInvoiceHeader(Common.Parse(row(0)).Trim, Common.Parse(row(1)).Trim)
                                        End If
                                    Next
                                End If
                            End If
                            Exit Sub
                        End If

                        AddGLEntry(ds, cRules, ds2)
                        ViewState("FilePath") = ""
                        lblPath.Text = ""
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
            If Not ViewState("dsHeader") Is Nothing Then
                Dim dtHeader As New DataTable : dtHeader = ViewState("dsHeader")
                If Not dtHeader.Rows.Count = 0 Then
                    For Each row As DataRow In dtHeader.Rows
                        If Not row(0) Is DBNull.Value And Not row(1) Is DBNull.Value Then
                            objIPP.deleteMultiInvoiceHeader(Common.Parse(row(0)).Trim, Common.Parse(row(1)).Trim)
                        End If
                    Next
                End If
            End If
            Common.TrwExp(ex)
        End Try

    End Sub

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

    Private Function AddGLEntry(ByRef pds As DataSet, ByVal pRules As myCollection, Optional ByVal dsSubDoc As DataSet = Nothing)
        Dim drItem As DataRow
        Dim objCat As New ContCat
        Dim pstrConnStr As String
        Dim strInvIdx, strSQL, costCentre, branchCode, strGSTPerc, strGSTID, coyType As String 'Zulham 19082015 - Added coyType 
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim SaveHeader As Integer
        Dim ds, dsTiedCC As New DataSet
        Dim bAllTrue As Boolean = True
        Dim strAryQuery(0) As String
        Dim percentage As String

        'Zulham 12112018
        Dim objTax As New GST

        pstrConnStr = ConfigurationManager.AppSettings(EnumAppPackage.eProcure.ToString & "Path")
        SaveHeader = 0
        ViewState("mode") = "add"
        vldsum.InnerHtml = ""

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

        'Sub doc
        'do some validation first
        Dim counted As Integer = 0
        Dim subdocAmt, invoiceDetailsAmt As Double
        Dim strSubDocNo As String = ""
        Dim result As String = ""

        'Jules 2018.07.20 - PAMB allow "/","\" and "#"
        Dim specialChar As Char() = "`~!@$%^&*()-_+=[]{};':""|,.<>?".ToCharArray

        'Zulham 13062019 - REQ018
        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllColTrue As Boolean = True

            Dim objExProduct As New ExcelIPPMultiInvoices
            If Common.parseNull(drItem(1)).ToString <> "" And Common.parseNull(drItem(4)).ToString <> "" And Common.parseNull(drItem(6)).ToString <> "" And Common.parseNull(drItem(7)).ToString <> "" And Common.parseNull(drItem(11)).ToString <> "" And Common.parseNull(drItem(12)).ToString <> "" Then
                If Not _exceedCutOffDt = "No" Then
                    If Not (Common.parseNull(drItem(1)).ToString <> "" Or Common.parseNull(drItem(4).ToString) <> "" Or Common.parseNull(drItem(6)) Is Nothing Or Common.parseNull(drItem(7)) Is Nothing Or Common.parseNull(drItem(11).ToString) <> "" Or Common.parseNull(drItem(12)) Is Nothing) Then
                        GoTo kkk
                    End If
                End If
                objExProduct = GetItemDetails(0, drItem)

                'Check whether invoice exists
                Dim exist As String = ""
                exist = objDb.GetVal("select '*' from invoice_mstr where im_invoice_no ='" & objExProduct.InvoiceNo & "' and im_s_coy_name = '" & objExProduct.VendorName & "'")
                If exist = "" Then
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invoice doesn't exist]<BR>"
                    bTrue = False
                End If

                'Check for line no.
                If objExProduct.No = 0 Then
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Please insert line no.]<BR>"
                    bTrue = False
                End If

                ' Check Pay For
                If objExProduct.PayFor <> "" Then
                    If UCase(objExProduct.PayFor) = "OWN CO." Then
                        Session("PayForVal") = UCase(Session("CompanyId"))
                        objExProduct.PayFor = UCase(Session("CompanyId"))
                        If Common.Parse(Session("CompanyId").ToString.Trim.ToUpper) = "HLISB" Then
                            If UCase(objExProduct.Reimbursement.Trim) <> "N/A" Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Reimbursement/Disbursement selection]<BR>"
                                bTrue = False
                            End If
                        End If
                    ElseIf Common.Parse(Session("CompanyId").ToString.Trim.ToUpper) = "HLISB" Then
                        If UCase(objExProduct.PayFor) <> "OWN CO." Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid PayFor selection]<BR>"
                            bTrue = False
                        ElseIf UCase(objExProduct.Reimbursement.Trim) <> "N/A" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Reimbursement/Disbursement selection]<BR>"
                            bTrue = False
                        End If
                    Else
                        'if payfor comp <> own comp. and reimb/disb = 'N/A', then it's invalid
                        If UCase(objExProduct.Reimbursement.Trim) = "N/A" And Not UCase(objExProduct.InputTax.Trim) = "NR (0%)" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Reimbursement/Disbursement selection]<BR>"
                            bTrue = False
                        End If

                        If strDefIPPCompID = "" Then
                            strSQL2 = "SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A' and ic_other_b_coy_code <> '' AND ic_coy_id = '" & Session("CompanyId") & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' "
                        Else
                            strSQL2 = "SELECT ic_other_b_coy_code FROM IPP_company WHERE IC_STATUS = 'A' and ic_other_b_coy_code <> '' AND ic_coy_id = '" & strDefIPPCompID & "' and ic_other_b_coy_code = '" & Common.Parse(objExProduct.PayFor) & "' "
                        End If
                        Dim PF_Val As String
                        PF_Val = objDb2.GetVal(strSQL2)
                        If PF_Val = "" Then 'no exist
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00350") & "]<BR>"
                            bTrue = False
                        Else
                            Session("PayForVal") = PF_Val
                            objExProduct.PayFor = PF_Val
                        End If

                    End If
                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Company Pay For " & objGlobal.GetErrorMessage("00001") & "].<BR>"
                    bTrue = False
                End If

                'Item validation based on GST Matrix 
                If (_exceedCutOffDt = "Yes" And strIsGst = "Yes") Or (strIsResident = "N" And _exceedCutOffDt = "Yes" And strIsGst = "Yes") Then
                    If UCase(objExProduct.PayFor) = "OWN CO." And (objExProduct.Reimbursement <> "N/A") Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid selection for Reimbursement/Disbursement].<BR>"
                        bTrue = False
                    End If
                    If objExProduct.GSTAmount Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    ElseIf objExProduct.GSTAmount = "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    ElseIf CDec(objExProduct.GSTAmount) < CDec(0) Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                        bTrue = False
                    End If
                    If objExProduct.InputTax Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid selection for GST Input Tax].<BR>"
                        bTrue = False
                    ElseIf objExProduct.InputTax = "N/A" And Not objExProduct.Reimbursement = "Disbursement" And Not UCase(objExProduct.PayFor) = "HLISB" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid selection for GST Input Tax].<BR>"
                        bTrue = False
                    End If
                    If objExProduct.OutputTax Is Nothing Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid selection for GST Output Tax].<BR>"
                        bTrue = False
                    End If

                    'If objExProduct.InputTax.Contains("0") And Common.parseNull(objExProduct.GSTAmount).ToString.Trim <> "" Then
                    '    If CDec(objExProduct.GSTAmount) <> CDec(0.0) Then
                    '        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                    '        bTrue = False
                    '    End If
                    'Else
                    If Not objExProduct.InputTax.Contains("(") And Common.parseNull(objExProduct.GSTAmount).ToString.Trim <> "" Then
                        If CDec(objExProduct.GSTAmount) <> CDec(0.0) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                            bTrue = False
                        End If
                    End If

                    If objExProduct.InputTax.Contains("(") Then
                        If Not objExProduct.GSTAmount.Trim.Length = 0 Then
                            If CDec(objExProduct.InputTax.Split("(")(1).Substring(0, 1)) > CDec(0) And CDec(objExProduct.GSTAmount) = CDec(0) Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                                bTrue = False
                            End If
                        End If
                    End If
                ElseIf strIsGst = "No" Then
                    Dim documentDate = objDb.GetVal("SELECT IFNULL(im_created_on,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Replace(Replace(Common.Parse(Request.QueryString("docno")), "\", "\\"), "#", "\#") & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                    Dim residentType As String = ""
                    If Request.QueryString("vencomp") IsNot Nothing Then residentType = objDb.GetVal("SELECT IFNULL(ic_resident_Type, '') FROM IPP_COMPANY WHERE ic_coy_type = 'V' and ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")

                    If CDate(documentDate) >= CDate(_cutoffDate) Then
                        If Not objExProduct.InputTax Is Nothing Then
                            If objExProduct.InputTax.ToUpper.Contains("NR") Then

                                'Zulham 19082015 - check for disb.reimb for NR
                                If Not objExProduct.Reimbursement.ToUpper.Trim = "N/A" And coyType.Trim.ToUpper = "V" Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Reimbursement/Disbursement selection].<BR>"
                                    bTrue = False
                                End If

                                If objExProduct.OutputTax <> "N/A" Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid selection for SST Input Tax].<BR>"
                                    bTrue = False
                                ElseIf objExProduct.GSTAmount.Trim.Length > 0 Then
                                    If CDec(objExProduct.GSTAmount) > CDec(0) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                End If
                            ElseIf objExProduct.GSTAmount.Trim.Length > 0 Then
                                'Skip gst Code without percentage
                                If objExProduct.InputTax.Contains("(") Then
                                    'Zulham 12112018
                                    percentage = objTax.getTaxPercentage(objExProduct.InputTax.Split("(")(0))
                                    If Not percentage.Trim = "0" Then
                                        percentage = percentage / 100
                                    End If
                                    'Zulham 10/01/2016 - Changed the decimal place from 1 to 2 
                                    If FormatNumber(CDec(objExProduct.GSTAmount), 2) <> FormatNumber(CDec(objExProduct.Amount * percentage), 2) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                Else
                                    If Not Format(CDec(objExProduct.GSTAmount), 2) = Format(CDec(0), 2) Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                                        bTrue = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

                'Zulham 26092019
                If objExProduct.GSTAmount.Trim.Length > 0 Then
                    'Skip gst Code without percentage
                    If objExProduct.InputTax.Contains("%") Then
                        percentage = objTax.getTaxPercentage(objExProduct.InputTax.Split("(")(0))
                        If Not percentage.Trim = "0" Then
                            percentage = percentage / 100
                        End If
                        'Zulham 10/01/2016 - Changed the decimal place from 1 to 2 
                        If FormatNumber(CDec(objExProduct.GSTAmount), 2) <> FormatNumber(CDec(objExProduct.Amount * percentage), 2) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                            bTrue = False
                        End If
                    Else
                        If Not Format(CDec(objExProduct.GSTAmount), 2) = Format(CDec(0), 2) Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid SST Amount].<BR>"
                            bTrue = False
                        End If
                    End If
                ElseIf objExProduct.InputTax.Contains("%") And Common.parseNull(objExProduct.GSTAmount) = "" Then
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Invalid SST Amount].<BR>"
                    bTrue = False
                End If

                ' Check UOM
                If objExProduct.UOM <> "" Then
                    strSQL2 = "SELECT code_desc FROM code_mstr WHERE code_category = 'UOM' AND code_desc = '" & Common.Parse(objExProduct.UOM) & "' "
                    Dim UOM_Val As String
                    UOM_Val = objDb2.GetVal(strSQL2)
                    If UOM_Val = "" Then 'no exist
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00351") & "]<BR>"
                        bTrue = False
                    Else
                        objExProduct.UOM = UOM_Val

                    End If
                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [UOM " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                    bTrue = False
                End If

                If Not CType(objExProduct.Amount, String) = "" Then
                    'If objExProduct.Amount <> 0 Then
                    'Check quantity
                    If objExProduct.Quantity.ToString = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Quantity " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                        End If

                        'Check unit price
                        If objExProduct.UnitPrice = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Unit Price " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                        End If

                        'Check amount
                        If objExProduct.Amount.ToString = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Amount " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                            bTrue = False
                        ElseIf CType(objExProduct.Amount.ToString, Double) < 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Amount must not be in negative value.]<BR>"
                            bTrue = False
                        End If
                    'Else
                    'GoTo kkk
                    'End If
                Else
                    GoTo kkk
                End If

                'Check for GL Code  
                If objExProduct.GLCode <> "" Then
                    If strDefIPPCompID = "" Then
                        strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & Session("CompanyId") & "' "
                    Else
                        strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & strDefIPPCompID & "' "
                    End If
                    Dim CBG_B_GL_Code As String
                    CBG_B_GL_Code = objDb2.GetVal(strSQL2)
                    If CBG_B_GL_Code = "" Then 'no exist
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00353") & "]<BR>"
                        bTrue = False
                    Else
                        objExProduct.GLCode = CBG_B_GL_Code
                    End If

                    If UCase(Session("PayForVal")) <> "PAMB" Then
                        'Zulham 30092019
                        isPayFor = True
                        objExProduct.GLCode = objDb.GetVal("SELECT ic_con_ibs_code FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE ic_other_b_coy_code = '" & Session("PayForVal") & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                    End If

                    If objExProduct.Gift <> "" Then
                        Dim strGLType = ""
                        strGLType = objDb.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & objExProduct.GLCode.Trim & "'")
                        If strGLType = "CAP" AndAlso objExProduct.Gift.Contains("Y") Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [CAPEX item cannot be Gift.]<BR>"
                            bTrue = False
                        End If
                    End If

                    Dim strSqlAC As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE2, '') AS CBGCAC_ANALYSIS_CODE2, IFNULL(CBGCAC_ANALYSIS_CODE3, '') AS CBGCAC_ANALYSIS_CODE3, IFNULL(CBGCAC_ANALYSIS_CODE4, '') AS CBGCAC_ANALYSIS_CODE4, IFNULL(CBGCAC_ANALYSIS_CODE5, '') AS CBGCAC_ANALYSIS_CODE5, " &
                                            "IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & objExProduct.GLCode & "'"
                    Dim dsAnalysisCodes As DataSet = objDb.FillDs(strSqlAC)
                    If dsAnalysisCodes.Tables(0).Rows.Count > 0 Then
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Fund Type " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE2").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Product Type " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE3").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Channel " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE4").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Reinsurance Company " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE5").ToString = "M" And objExProduct.FundType = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Asset Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And objExProduct.ProjectCode = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Project Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                        If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And objExProduct.PersonCode = "" Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Person Code " & objGlobal.GetErrorMessage("00001") & "]"
                            bTrue = False
                        End If
                    End If

                Else
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [GL Code " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                    bTrue = False
                End If

                If objExProduct.CostAllocation = "" Then
                    'Check for HO/BR
                    If objExProduct.HOBR <> "" And UCase(objExProduct.PayFor) = "PAMB" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & Common.Parse(objExProduct.HOBR) & "'") = 0 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            End If

                        Else
                            strSQL2 = "SELECT cbm_branch_code FROM company_branch_mstr WHERE cbm_coy_id = '" & Session("CompanyId") & "' AND cbm_status = 'A' and cbm_branch_code = '" & Common.Parse(objExProduct.HOBR) & "' "

                            Dim HOBRCode As String
                            HOBRCode = objDb2.GetVal(strSQL2)
                            If HOBRCode = "" Then 'no exist
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
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
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
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
                                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Cost Center]<BR>"
                                            bTrue = False
                                        End If
                                    Next
                                End If
                                'Check for Cost Center
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    End If
                                Else
                                    strSQL2 = "SELECT cc_cc_code FROM cost_centre WHERE cc_status = 'A' AND cc_coy_id = '" & Common.Parse(objExProduct.PayFor) & "' AND cc_cc_code = '" & Common.Parse(objExProduct.CostCenter) & "' "
                                    Dim CostCenter As String
                                    CostCenter = objDb2.GetVal(strSQL2)
                                    If CostCenter = "" Then 'no exist
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    Else
                                        objExProduct.CostCenter = CostCenter
                                    End If
                                End If
                            ElseIf objExProduct.CostCenter = "" Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
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
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                bTrue = False
                            End If
                        Else
                            strSQL2 = "SELECT cbm_branch_code FROM company_branch_mstr WHERE cbm_coy_id = '" & objExProduct.PayFor & "' AND cbm_status = 'A' and cbm_branch_code = '" & Common.Parse(objExProduct.HOBR) & "' "

                            Dim HOBRCode As String
                            HOBRCode = objDb2.GetVal(strSQL2)
                            If HOBRCode = "" Then 'no exist
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00352") & "]<BR>"
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
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Cost Center]<BR>"
                                        bTrue = False
                                    End If
                                Next
                            End If

                            'Check for Cost Center
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                    bTrue = False
                                End If
                            Else
                                strSQL2 = "SELECT cc_cc_code FROM cost_centre WHERE cc_status = 'A' AND cc_coy_id = '" & Common.Parse(objExProduct.PayFor) & "' AND cc_cc_code = '" & Common.Parse(objExProduct.CostCenter) & "' "
                                Dim CostCenter As String
                                CostCenter = objDb2.GetVal(strSQL2)
                                If CostCenter = "" Then 'no exist
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                    bTrue = False
                                Else
                                    objExProduct.CostCenter = CostCenter
                                End If
                            End If
                        ElseIf objExProduct.CostCenter = "" Then
                            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Center " & objGlobal.GetErrorMessage("00001") & "]<BR>"
                                    bTrue = False
                                End If
                            Else
                                objExProduct.CostCenter = ""
                            End If
                        End If
                    ElseIf objExProduct.HOBR <> "" And (UCase(objExProduct.PayFor) <> "PAMB") Then
                        objExProduct.HOBR = ""

                    End If
                Else
                    If objExProduct.CostCenter <> "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Center is not allowed if Cost Alloction has been assigned.]<BR>"
                        bTrue = False
                    End If
                    If objExProduct.HOBR <> "" Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [HO/BR is not allowed if Cost Alloction has been assigned.]<BR>"
                        bTrue = False
                    End If
                    If objExProduct.CostAllocation <> "" Then
                        strSQL2 = "SELECT cam_ca_code FROM cost_alloc_mstr WHERE cam_coy_id = '" & Session("CompanyId") & "' AND cam_ca_code = '" & Common.Parse(objExProduct.CostAllocation) & "' "
                        Dim CostAlloc As String
                        CostAlloc = objDb2.GetVal(strSQL2)
                        If CostAlloc = "" Then 'no exist
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00357") & "]<BR>"
                            bTrue = False
                        Else
                            objExProduct.CostAllocation = CostAlloc
                        End If

                        If objExProduct.GLCode <> "" And objExProduct.CostAllocation <> "" Then
                            Dim decTtlPct As Decimal
                            Dim intCostAllocIndex As Integer
                            Dim dsCAD As DataSet
                            Dim k As Integer

                            intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & Common.Parse(objExProduct.CostAllocation) & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"))

                            decTtlPct = objIPP.GetSUMCostAllocDetail(intCostAllocIndex)

                            If decTtlPct < 100 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [" & objGlobal.GetErrorMessage("00052") & "]<BR>"
                                bTrue = False
                            End If

                            dsCAD = objIPP.CheckCostAllocDetail(intCostAllocIndex)

                            For k = 0 To dsCAD.Tables(0).Rows.Count - 1
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Allocation Line " & k + 1 & " - " & objGlobal.GetErrorMessage("00352") & "]<BR>"
                                        bTrue = False
                                    End If
                                End If
                                If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "'") = 0 Then
                                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Allocation  Line " & k + 1 & " - " & objGlobal.GetErrorMessage("00354") & "]<BR>"
                                        bTrue = False
                                    End If
                                End If

                                If InStr(1, objExProduct.GLCode.ToString, "7") = 1 _
                                And objDb.GetVal("SELECT cbm_branch_type FROM company_branch_mstr WHERE cbm_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString & "' and cbm_status = 'A' and cbm_coy_id = '" & objExProduct.PayFor & "'") = "HO" _
                                    And dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString = "900" _
                                    And Common.parseNull(dsCAD.Tables(0).Rows(k)("CAD_CC_CODE")) = "" Then

                                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Allocation Line " & k + 1 & " - Cost Center is required.]<BR>"
                                    bTrue = False
                                End If

                            Next
                        End If
                    Else
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Allocation required.]<BR>"
                        bTrue = False
                    End If

                End If

                If objExProduct.CostCenter <> "" Then
                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & objExProduct.CostCenter & "'") = 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [You have no permission to use this Cost Center.]<BR>"
                            bTrue = False
                        End If
                    Else
                        If Not objExProduct.CostCenter.ToString.Trim = "" Then
                            Dim getSQL = "SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & objExProduct.CostCenter.ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
                            If objDb.Exist("SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & objExProduct.CostCenter.ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") = 0 Then
                                vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Invalid Cost Center.]<BR>"
                                bTrue = False
                            End If
                        End If
                    End If

                Else 'to prevent user no enter for CC without permission to charge 000 
                    If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '000'") = 0 Then
                            vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Cost Center is required.]<BR>"
                            bTrue = False
                        End If
                    End If
                End If

                'Zulham 27092019
                If objExProduct.WHTOption = "" And Not objExProduct.WHTTax = "" Then
                    vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Please select Withholding Option.]<BR>"
                    bTrue = False
                Else
                    If objExProduct.WHTOption <> "" And (objExProduct.WHTTax = "" OrElse Not IsNumeric(objExProduct.WHTTax)) Then
                        vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " for Invoice No. " & objExProduct.InvoiceNo & " [Withholding Tax is invalid.]<BR>"
                        bTrue = False
                    End If
                End If

                Dim dsItemDetails As New DataSet
                dsItemDetails = bindItems(objExProduct)

                If bTrue = False Then
                    bAllTrue = False
                    bAllColTrue = False
                End If

                If bAllColTrue = True Then
                    If _exceedCutOffDt = "No" Then
                        If objIPP.SaveMultiGLDebits(dsItemDetails, Nothing, strDocNo, strOldDocNo, strVendor, strAryQuery, ViewState("igc_glrule_category_index")) Then ', SaveHeader, ViewState("Index")) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    ElseIf _exceedCutOffDt = "Yes" Then
                        If objIPP.SaveMultiGLDebits(dsItemDetails, Nothing, strDocNo, strOldDocNo, strVendor, strAryQuery, ViewState("igc_glrule_category_index"), "Yes") Then ', SaveHeader, ViewState("Index")) Then
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
        Else
            If Not ViewState("dsHeader") Is Nothing Then
                Dim dsHeader As New DataTable : dsHeader = ViewState("dsHeader")
                If Not dsHeader.Rows.Count = 0 Then
                    For Each row As DataRow In dsHeader.Rows
                        If Not row(0) Is DBNull.Value And Not row(1) Is DBNull.Value Then
                            objIPP.deleteMultiInvoiceHeader(Common.Parse(row(0)).Trim, Common.Parse(row(1)).Trim)
                        End If
                    Next
                End If
            End If
        End If

        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strscript.Append("<script language=""javascript"">")
        strscript.Append("UnBlockUploadProgress();")
        strscript.Append("</script>")
        RegisterStartupScript("script5", strscript.ToString())

        'Zulham 30092019
        If isPayFor Then
            Common.NetMsgbox(Me, "Conventional IBS GL Code will be used for Pay For Company instead of the selected GL Code." & """& vbCrLf & """ & "Successfull: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s). ", MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, "Successfull: " & countSave & " record(s)." & """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)

        End If


    End Function

    Private Sub Clear()
        ViewState("Index") = 0
        ViewState("ProductCode") = ""
        ViewState("VendorItemCode") = ""
        ViewState("ProductDesc") = ""
        ViewState("UOM") = ""
    End Sub

    Private Function bindItems(ByRef pGLEntryDetails As ExcelIPPMultiInvoices) As DataSet
        Dim ds As New DataSet
        Dim dtItem As New DataTable
        Dim dtr As DataRow

        'Contract Items
        dtItem.Columns.Add("ItemLine", Type.GetType("System.Int32"))
        dtItem.Columns.Add("InvoiceNo", Type.GetType("System.String"))
        dtItem.Columns.Add("VendorName", Type.GetType("System.String"))
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

        dtr("InvoiceNo") = Common.Parse(Common.parseNull(pGLEntryDetails.InvoiceNo))
        dtr("VendorName") = Common.Parse(Common.parseNull(pGLEntryDetails.VendorName))

        If Common.parseNull(pGLEntryDetails.Gift) = "" Then
            dtr("Gift") = "N"
        Else
            dtr("Gift") = Common.Parse(pGLEntryDetails.Gift).ToString.Substring(0, 1)
        End If
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

    Private Function GetItemDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelIPPMultiInvoices
        Dim objExcel As New ExcelIPPMultiInvoices

        With objExcel

            Dim costcenter As String

            If IsDBNull(pdr.Item("F13")) Then
                costcenter = ""
            ElseIf Common.parseNull(pdr.Item("F13")).ToString.Contains(":") Then
                costcenter = pdr.Item("F13").ToString.Split(":")(1)
            Else
                costcenter = Common.parseNull(pdr.Item("F13"))
            End If

            objExcel.No = IIf(IsDBNull(pdr.Item("F4")), 0, pdr.Item("F4"))
            objExcel.InvoiceNo = IIf(IsDBNull(pdr.Item("F2")), 0, pdr.Item("F2"))
            objExcel.VendorName = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
            objExcel.PayFor = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
            objExcel.Reimbursement = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))
            objExcel.Description = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7"))
            objExcel.UOM = "Unit"
            objExcel.Quantity = 1
            objExcel.UnitPrice = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
            objExcel.Amount = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
            objExcel.GSTAmount = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
            objExcel.InputTax = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
            objExcel.OutputTax = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
            objExcel.GLCode = IIf(IsDBNull(pdr.Item("F12")), "", IIf(pdr.Item("F12").ToString.Contains(":"), pdr.Item("F12").ToString.Split(":")(1), pdr.Item("F12")))
            objExcel.CostAllocation = ""
            objExcel.RulesCategory = ""
            objExcel.HOBR = ""
            objExcel.CostCenter = costcenter
            objExcel.Gift = "No"
            objExcel.Category = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
            objExcel.FundType = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
            objExcel.ProductType = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
            objExcel.Channel = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
            objExcel.ReinsuranceCo = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))
            objExcel.AssetCode = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))
            objExcel.ProjectCode = IIf(IsDBNull(pdr.Item("F20")), "", pdr.Item("F20"))
            objExcel.PersonCode = IIf(IsDBNull(pdr.Item("F21")), "", pdr.Item("F21"))
            objExcel.WHTOption = IIf(IsDBNull(pdr.Item("F22")), "", pdr.Item("F22"))
            objExcel.WHTTax = IIf(IsDBNull(pdr.Item("F23")), "", pdr.Item("F23"))

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

    Private Function addInvoiceMstr(ByVal dsHeader As DataTable) As Boolean

        Dim dtr As DataRow
        Dim objDB As New EAD.DBCom
        Dim compIdx As Integer = 0
        Dim dsCompDetail As DataSet
        Dim objDoc As New IPPMain
        Dim CompanyCategory As String = ""
        Dim ResidentType As String = ""
        Dim strDocNo As String
        Dim strCurrency As String = ""

        If Not validateInvoiceMstr(dsHeader) Then
            Return False
        End If

        For Each row As DataRow In dsHeader.Rows
            If Common.parseNull(row(0)).ToString.Trim <> "" And Common.parseNull(row(1)).ToString.Trim <> "" Then
                Dim dsdoc As New DataSet
                Dim dtDoc As New DataTable
                dtDoc.Columns.Add("DocType", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocNo", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("LateSubmit", Type.GetType("System.String"))
                dtDoc.Columns.Add("ManualPONo", Type.GetType("System.String"))
                dtDoc.Columns.Add("VendorName", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenCompIDX", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine1", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine2", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrLine3", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrPostCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrState", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrCity", Type.GetType("System.String"))
                dtDoc.Columns.Add("VenAddrCountry", Type.GetType("System.String"))
                dtDoc.Columns.Add("CurrencyCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("PaymentAmt", Type.GetType("System.Decimal"))
                dtDoc.Columns.Add("PaymentMethod", Type.GetType("System.String"))
                dtDoc.Columns.Add("WHT", Type.GetType("System.String"))
                dtDoc.Columns.Add("WHTOpt", Type.GetType("System.String"))
                dtDoc.Columns.Add("NoWHTReason", Type.GetType("System.String"))
                dtDoc.Columns.Add("InternalRemark", Type.GetType("System.String"))
                dtDoc.Columns.Add("ExchangeRate", Type.GetType("System.String"))
                dtDoc.Columns.Add("LateReason", Type.GetType("System.String"))
                dtDoc.Columns.Add("OldVenCompIDX", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankCode", Type.GetType("System.String"))
                dtDoc.Columns.Add("BankAccount", Type.GetType("System.String"))
                dtDoc.Columns.Add("DocDueDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("PRCSSentDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("PRCSReceivedDate", Type.GetType("System.String"))
                dtDoc.Columns.Add("BeneficiaryDetails", Type.GetType("System.String"))
                dtDoc.Columns.Add("MasterDocument", Type.GetType("System.String"))
                dtDoc.Columns.Add("CompanyCategory", Type.GetType("System.String"))
                dtDoc.Columns.Add("ResidentType", Type.GetType("System.String"))
                dtDoc.Columns.Add("EmpId", Type.GetType("System.String"))
                dtDoc.Columns.Add("TotalAmtNoGST", Type.GetType("System.Decimal"))
                dtDoc.Columns.Add("GSTAmt", Type.GetType("System.Decimal"))

                dtr = dtDoc.NewRow()
                Select Case (row(2).ToString.Trim)
                    Case "Invoice"
                        dtr("DocType") = "INV"
                    Case "Bill"
                        dtr("DocType") = "BILL"
                    Case "Credit Note"
                        dtr("DocType") = "CN"
                    Case "Debit Note"
                        dtr("DocType") = "DN"
                    Case "Letter"
                        dtr("DocType") = "LETTER"
                End Select
                dtr("DocNo") = row(0)
                If strDocNo = "" Then
                    strDocNo = "'" & row(0) & "'"
                Else
                    strDocNo = strDocNo & ",'" & row(0) & "'"
                End If
                dtr("DocDate") = "NULL"
                dtr("LateSubmit") = ""
                dtr("ManualPONo") = ""
                dtr("VendorName") = Common.Parse(row(1))

                If strVenType.ToLower.Trim = "e" Then
                    Dim empNo = objDB.GetVal("SELECT IFNULL(ic_additional_1,'') 'ic_additional_1' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(row(1)) & "' and ic_coy_type = 'E'")
                    dtr("EmpId") = empNo
                Else
                    dtr("EmpId") = ""
                End If

                compIdx = objDB.GetVal("SELECT ic_index FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(row(1)) & "' and ic_coy_type = '" & strVenType & "'")

                dtr("VenCompIDX") = compIdx

                dsCompDetail = objDoc.GetVendorAddress(row(2), ViewState("IPPOfficer"), compIdx)

                dtr("VenAddrLine1") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line1")
                dtr("VenAddrLine2") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line2")
                dtr("VenAddrLine3") = dsCompDetail.Tables(0).Rows(0)("ic_addr_line3")
                dtr("VenAddrPostCode") = dsCompDetail.Tables(0).Rows(0)("ic_postcode")
                dtr("VenAddrState") = dsCompDetail.Tables(0).Rows(0)("ic_state")
                dtr("VenAddrCity") = dsCompDetail.Tables(0).Rows(0)("ic_city")
                dtr("VenAddrCountry") = dsCompDetail.Tables(0).Rows(0)("ic_country")

                strCurrency = objDB.GetVal("SELECT 
                                            CASE WHEN TRIM(ic_country) = 'MY' THEN 'MYR' 
                                            ELSE '' 
                                            END AS 'Currency' 
                                            FROM ipp_company  
                                            where ic_index = " & compIdx)
                dtr("CurrencyCode") = IIf(strCurrency.Trim = "", "NULL", strCurrency)
                dtr("PaymentAmt") = 0
                dtr("OldVenCompIDX") = Common.parseNull(compIdx)
                dtr("BankCode") = dsCompDetail.Tables(0).Rows(0)("IC_bank_code")
                dtr("BankAccount") = dsCompDetail.Tables(0).Rows(0)("IC_bank_acct")
                dtr("PaymentMethod") = Common.parseNull(dsCompDetail.Tables(0).Rows(0)("ic_payment_method"))
                dtr("WHT") = "NULL"
                dtr("WHTOpt") = ""
                dtr("NoWHTReason") = ""
                dtr("InternalRemark") = ""
                dtr("ExchangeRate") = "NULL"
                dtr("DocDueDate") = "NULL"
                dtr("PRCSSentDate") = "NULL"
                dtr("PRCSReceivedDate") = "NULL"
                dtr("BeneficiaryDetails") = ""
                dtr("MasterDocument") = "N"
                CompanyCategory = objDB.GetVal("SELECT IF(ic_company_category IS NULL,'',ic_company_category) AS 'ic_company_category' FROM ipp_company WHERE ic_index = '" & compIdx & "'")
                dtr("CompanyCategory") = Common.parseNull(CompanyCategory)
                ResidentType = objDB.GetVal("SELECT ic_resident_type FROM ipp_company WHERE ic_index = '" & compIdx & "'")
                dtr("ResidentType") = Common.parseNull(ResidentType)
                dtr("TotalAmtNoGST") = 0
                dtr("GSTAmt") = 0
                dtDoc.Rows.Add(dtr)
                dsdoc.Tables.Add(dtDoc)

                objDoc.SaveIPPDocHeader(dsdoc, False, row(0))

            End If
        Next

        If Not strDocNo = "" Then
            Session("strDocNo") = strDocNo
        End If

        Return True
    End Function

    Private Function validateInvoiceMstr(ByVal dsHeader As DataTable) As Boolean

        Dim compidx As String = ""
        Dim objdb As New EAD.DBCom
        Dim amount As Double
        Dim exist As String = ""
        Dim strPaymentMethod As String = ""
        Dim strBuilder As New StringBuilder
        vldsum.InnerHtml = ""

        For Each row As DataRow In dsHeader.Rows
            If Not row(0).ToString.Trim = "" And Not row(1).ToString.Trim = "" Then
                If Not strBuilder.ToString = "" Then
                    If strBuilder.ToString.Contains(row(0) & ":" & row(1)) Then
                        vldsum.InnerHtml &= "<LI type=square>Duplicate data for invoice '" & row(0) & "' for company '" & Common.Parse(row(1)) & "'.<BR>"
                        Continue For
                    End If
                End If
                strBuilder.Append(row(0) & ":" & row(1))
                'check whether invoice already exist
                exist = objdb.GetVal("SELECT '*' FROM invoice_mstr WHERE im_s_coy_name = '" & Common.Parse(row(1)) & "' and im_invoice_no = '" & row(0) & "'")
                If exist = "*" Then
                    vldsum.InnerHtml &= "<LI type=square>Invoice '" & row(0) & "' for company '" & Common.Parse(row(1)) & "' already exists.<BR>"
                End If
                'check vendor
                compidx = objdb.GetVal("SELECT ic_index FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(row(1)) & "' and ic_coy_type = '" & strVenType & "'")

                If compidx = "" Then
                    vldsum.InnerHtml &= "<LI type=square>Company Name " & row(1) & " is not valid.<BR>"
                End If
            End If
        Next

        If Not vldsum.InnerHtml = "" Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function checkDuplicates(ByVal ds As DataTable) As Boolean
        Dim counter = ds.Rows.Count - 1
        Dim aryList As New ArrayList
        'Zulham 27032020
        Dim invoiceNoAryList As New ArrayList
        Dim strInvoiceNo As String = ""
        vldsum.InnerHtml = ""
        For i As Integer = 0 To counter
            If i < counter Then
                If Not ds.Rows(i).Item(0) Is DBNull.Value Then
                    Try
                        If i = 0 Then
                            strInvoiceNo = ds.Rows(i).Item(0).ToString.Trim
                        Else
                            If ds.Rows(i).Item(0) <> ds.Rows(i - 1).Item(0) Then
                                strInvoiceNo = ds.Rows(i).Item(0).ToString.Trim
                                aryList.Clear()
                            End If
                        End If

                        If ds.Rows(i).Item(0) = strInvoiceNo Then
                            If aryList Is Nothing Then
                                aryList.Add(ds.Rows(i).Item(2))
                            Else
                                If aryList.IndexOf(ds.Rows(i).Item(2)) = -1 Then
                                    aryList.Add(ds.Rows(i).Item(2))
                                Else
                                    vldsum.InnerHtml &= "<LI type=square>Duplicate Line No. " & ds.Rows(i).Item(2).ToString & " for invoice '" & ds.Rows(i).Item(0) & "'.<BR>"
                                End If
                            End If
                        End If
                        'Zulham 27032020
                        strInvoiceNo = strInvoiceNo & ds.Rows(i).Item(2)
                        If invoiceNoAryList Is Nothing Then
                            invoiceNoAryList.Add(strInvoiceNo)
                        Else
                            If invoiceNoAryList.IndexOf(strInvoiceNo) = -1 Then
                                invoiceNoAryList.Add(strInvoiceNo)
                            Else
                                vldsum.InnerHtml &= "<LI type=square>Duplicate Line No. " & ds.Rows(i).Item(2).ToString & " for invoice '" & ds.Rows(i).Item(0) & "'.<BR>"
                            End If
                        End If

                    Catch ex As Exception
                        Common.TrwExp(ex)
                    End Try
                End If
            End If
        Next

        If vldsum.InnerHtml = "" Then
            Return True
        Else
            Return False
        End If

    End Function
    'Zulham 08072019
    Private Function valMandatoryFields(ByVal dt As DataTable) As Boolean
        vldsum.InnerHtml = ""
        For i As Integer = 0 To dt.Rows.Count - 1
            If Not dt.Rows(i).Item(1) Is DBNull.Value And Not dt.Rows(i).Item(3) Is DBNull.Value Then
                '0
                If dt.Rows(i).Item(0) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Document Type - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '2
                If dt.Rows(i).Item(2) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Company Name - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '3
                If Not IsNumeric(dt.Rows(i).Item(3)) Then
                    vldsum.InnerHtml &= "<LI type=square> Invalid Line No - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                    Continue For
                End If
                '4
                If dt.Rows(i).Item(4) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing PayFor - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '5
                If dt.Rows(i).Item(5) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Disb./Reimb. - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '6
                If dt.Rows(i).Item(6) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Description - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '7
                If dt.Rows(i).Item(7) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Amount - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                ElseIf Not isnumeric(dt.Rows(i).Item(7)) Then
                    vldsum.InnerHtml &= "<LI type=square> Invalid Amount - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '8
                If dt.Rows(i).Item(8) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing SST Amount - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                ElseIf Not IsNumeric(dt.Rows(i).Item(8)) Then
                    vldsum.InnerHtml &= "<LI type=square> Invalid SST Amount - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '9
                If dt.Rows(i).Item(9) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Input Tax Code - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '10
                If dt.Rows(i).Item(10) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Output Tax Code - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '11
                If dt.Rows(i).Item(11) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing GL Code - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
                '12
                If dt.Rows(i).Item(12) Is DBNull.Value Then
                    vldsum.InnerHtml &= "<LI type=square> Missing Cost Center - Line No " & dt.Rows(i).Item(3).ToString & " for invoice '" & dt.Rows(i).Item(1) & "'.<BR>"
                End If
            End If
        Next

        If vldsum.InnerHtml = "" Then
            Return True
        Else
            Return False
        End If

    End Function
End Class