Imports System.io
Imports eProcure.Component
Imports AgoraLegacy

Imports System.Text.RegularExpressions

Public Class POBatchUpload
    Inherits AgoraLegacy.AppBaseClass

    'Dim strTempPath, strDestPath As String
    Dim objIPP As New IPPMain
    Dim objGlobal As New AppGlobals
    'Dim objExMultiGLHeader As New ExcelIPPMultiGLDebitsHeader
    Dim objExMultiGLHeader As New ExcelPOUplopadHeader
    Dim strVendor, strDocNo, strOldDocNo, strDocType As String
    Dim strPONo As String
    Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

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
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorName As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button

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


        blnPaging = False
        blnSorting = False
        'SetGridProperty(dg)
        If Not Request.QueryString("docno") Is Nothing Then
            If Not Request.QueryString("docno").ToString.Trim = "To" Then
                strDocNo = Request.QueryString("docno")
            Else
                strDocNo = "To Be Allocated By System"
            End If
        End If
        'strDocType = Request.QueryString("doctype")
        If Not Request.QueryString("vencomp") Is Nothing Then
            strVendor = Request.QueryString("vencomp")
        End If
        Me.cmdClose.Attributes.Add("onclick", "window.close();")

        lblPONo.Text = strDocNo
        'lblDocDate.Text = Request.QueryString("docdate")
        Dim _sql = "SELECT cm_coy_name FROM sso.company_mstr where cm_coy_id = '" & Request.QueryString("vencomp") & "'"
        lblVendorName.Text = objDB.GetVal(_sql)

        lblPath.Text = ViewState("FilePath")
        '    If Not Page.IsPostBack Then
        '        'trResult.Visible = False

        '        Dim dsCat As New DataSet
        '        Dim cbolist As New ListItem
        '        'dsCat = objIPP.getConRefNo()
        '        'Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
        '        cbolist.Value = ""
        '        cbolist.Text = "---Select---"
        '        'ddlCode.Items.Insert(0, cbolist)
        '        'ddlCode.SelectedIndex = 0
        '    End If
        'Else
        '    Dim strMsg As String
        '    Dim objCat As New ContCat
        '    strMsg = "Can only upload/download Item List for Buyer Company."
        '    'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
        '    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)

        'End If
        Session("CurrentScreen") = "POBatchUpload"
        Me.cmdUpload.Attributes.Add("onClick", "UploadProgress();")
    End Sub

    Private Sub cmdDownloadTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDownloadTemplate.Click
        Dim ds As New DataSet
        Dim ds1 As New DataSet
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
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

        ViewState("mode") = Request.QueryString("mode")
        Dim objAdmin As New Admin
        Dim objPR As New PR
        Dim objBudget As New BudgetControl
        Dim dsGLCode, dsCatCode, dsAsset, dsBCM, dsBillTo, dsDeliveryAdd, dsCustomField, dsCustomName, dsGSTRate As DataSet
        Dim strSql, strAddr, exceedCutOffDt As String
        Dim dvCF As DataView
        Dim createdDate As String
        dvCF = objAdmin.getCustomField("", "PO")
        dsCustomName = objAdmin.getCustomFieldDS("", "PO")
        filename = "POTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
        ds = objPR.getVendorDetail(Request.QueryString("vencomp"))

        dsGLCode = objAdmin.PopulateGLCodeMstr("", "")

        dsCatCode = objAdmin.PopulateCategoryCodeMstr("", "")

        strSql = "SELECT AG_GROUP AS ASSET_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " _
                   & " FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " _
                   & " ORDER BY AG_GROUP "
        dsAsset = objDb.FillDs(strSql)

        strSql = "SELECT IF(TAX_PERC = '', CODE_DESC," &
                "CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST, CODE_ABBR " &
                "FROM CODE_MSTR " &
                "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND " &
                "TAX_COUNTRY_CODE = 'MY' " &
                "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' "
        dsGSTRate = objDb.FillDs(strSql)

        dsBillTo = objAdmin.PopulateAddr("B", "", "", "")

        dsDeliveryAdd = objAdmin.PopulateAddr("D", "", "", "", True)

        'Check for GST
        'check whether gst is applicable or otherwise
        Dim boolchkGSTCod As Boolean
        Dim strIsGst As String
        Dim strGstCOD = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        createdDate = objDb.GetVal("SELECT pom_created_date FROM po_mstr WHERE pom_po_no = '" & Me.lblPONo.Text & "'  AND pom_s_coy_id = '" & strVendor & "'")
        createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, createdDate)
        If createdDate <> "" Then
            If CDate(createdDate) >= CDate(strGstCOD) Then
                boolchkGSTCod = True
            Else
                boolchkGSTCod = False
            End If
        Else
            If Date.Now() >= CDate(strGstCOD) Then
                boolchkGSTCod = True
            Else
                boolchkGSTCod = False
            End If
        End If

        Dim createdDate2 = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        If CDate(createdDate2) >= CDate(_cutoffDate) Then
            exceedCutOffDt = "Yes"
            Dim GSTRegNo = objDb.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Request.QueryString("vencomp") & "'")
            If GSTRegNo <> "" Then
                boolchkGSTCod = True
            Else
                boolchkGSTCod = False
            End If
        Else
            exceedCutOffDt = "No"
            boolchkGSTCod = False
        End If

        If boolchkGSTCod Then
            strIsGst = "Yes"
        Else
            strIsGst = "No"
        End If

        'Jules 2018.08.02
        Dim GST As New GST
        Dim dsInputTax, dsFundType, dsPersonCode, dsProjectCode As DataSet
        dsInputTax = GST.GetTaxCode_forP2P("", "P")
        dsFundType = objGlobal.GetAnalysisCodeByDept("L1", True)
        dsPersonCode = objGlobal.GetAnalysisCodeByDept("L9", True)
        dsProjectCode = objGlobal.GetAnalysisCodeByDept("L8", True)

        ViewState("BCM") = CInt(objPR.checkBCM)
        If ViewState("BCM") > 0 Then
            dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")

            'Jules 2018.07.14 - PAMB
            If HttpContext.Current.Session("CompanyId").ToString.ToUpper = "PAMB" Then
                objEx.WriteCell_POUpload(ds, ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode, dsAsset, dsDeliveryAdd, dsBillTo, dsCatCode, dsBCM, dsGSTRate, strIsGst, exceedCutOffDt, dsFundType, dsPersonCode, dsProjectCode, dsInputTax)
                objEx.ProtectWorkSheet_POUpload_PAMB(ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode.Tables(0).Rows.Count, dsCatCode.Tables(0).Rows.Count, dsAsset.Tables(0).Rows.Count, dsDeliveryAdd.Tables(0).Rows.Count, dsBillTo.Tables(0).Rows.Count, dsInputTax.Tables(0).Rows.Count, dsFundType.Tables(0).Rows.Count, dsPersonCode.Tables(0).Rows.Count, dsProjectCode.Tables(0).Rows.Count, dsCustomName, dsBCM.Tables(0).Rows.Count, dsGSTRate.Tables(0).Rows.Count, strIsGst, exceedCutOffDt)
            Else 'original code
                objEx.WriteCell_POUpload(ds, ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode, dsAsset, dsDeliveryAdd, dsBillTo, dsCatCode, dsBCM, dsGSTRate, strIsGst, exceedCutOffDt)
                objEx.ProtectWorkSheet_POUpload(ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode.Tables(0).Rows.Count, dsCatCode.Tables(0).Rows.Count, dsAsset.Tables(0).Rows.Count, dsDeliveryAdd.Tables(0).Rows.Count, dsBillTo.Tables(0).Rows.Count, dsCustomName, dsBCM.Tables(0).Rows.Count, dsGSTRate.Tables(0).Rows.Count, strIsGst, exceedCutOffDt)
            End If

        Else
            'Jules 2018.08.02
            'objEx.WriteCell_POUpload(ds, ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode, dsAsset, dsDeliveryAdd, dsBillTo, dsCatCode, , dsGSTRate, strIsGst, exceedCutOffDt)
            'objEx.ProtectWorkSheet_POUpload(ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode.Tables(0).Rows.Count, dsCatCode.Tables(0).Rows.Count, dsAsset.Tables(0).Rows.Count, dsDeliveryAdd.Tables(0).Rows.Count, dsBillTo.Tables(0).Rows.Count, dsCustomName, , dsGSTRate.Tables(0).Rows.Count, strIsGst, exceedCutOffDt)
            objEx.WriteCell_POUpload(ds, ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode, dsAsset, dsDeliveryAdd, dsBillTo, dsCatCode, dsBCM, dsGSTRate, strIsGst, exceedCutOffDt, dsFundType, dsPersonCode, dsProjectCode, dsInputTax)
            objEx.ProtectWorkSheet_POUpload_PAMB(ConfigurationManager.AppSettings("TemplateTemp") & filename, dsGLCode.Tables(0).Rows.Count, dsCatCode.Tables(0).Rows.Count, dsAsset.Tables(0).Rows.Count, dsDeliveryAdd.Tables(0).Rows.Count, dsBillTo.Tables(0).Rows.Count, dsInputTax.Tables(0).Rows.Count, dsFundType.Tables(0).Rows.Count, dsPersonCode.Tables(0).Rows.Count, dsProjectCode.Tables(0).Rows.Count, dsCustomName, dsBCM.Tables(0).Rows.Count, dsGSTRate.Tables(0).Rows.Count, strIsGst, exceedCutOffDt)
        End If
        Filedownload()
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

            Clear()

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            strDestPath = objFileMgmt.getSystemParam("BatchUpload", "Uploaded", ConfigurationManager.AppSettings("eProcurePath"))

            'Nov 22, 2-13
            'strTempPath = "C:\itg\Published\FileMgnt\TempProductList\"
            'strDestPath = "C:\itg\Published\FileMgnt\UploadedProductList\"    
            'End

            If IsExcel(cmdBrowse.PostedFile.FileName) Then

                Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

                If cmdBrowse.PostedFile.ContentLength > 0 And cmdBrowse.PostedFile.ContentLength / 1024 <= Session("FileSize") Then
                    'Upload to temp folder in server
                    FileUpload(cmdBrowse.PostedFile.FileName, strFileName)
                    ds = objEx.ReadPOUploadFormat(Server.MapPath("../xml/POUpload.xml"), strTempPath & strFileName, cRules, dtHeader)

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
                        If Not AddGLEntry(ds, cRules, dtHeader) Then
                            Common.NetMsgbox(Me, "Please check the excel file.")
                        End If
                        ViewState("FilePath") = ""
                        lblPath.Text = ""
                        Session("BatchUpload_Reload") = "Yes"

                        Session("strItem") = Nothing
                        Session("strItemCust") = Nothing
                        Session("strItemHead") = Nothing
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
        Dim MM As New CostAllocIPP
        objExMultiGLHeader = GetMultiGLHeader(dtHeader)
        'If objExConCatHeader.Action = "New" Then
        If objExMultiGLHeader.CoyName = "" Then
            Common.NetMsgbox(Me, "Vendor Company Name is required.")
            'Common.NetMsgbox(Me, MM.Message(Me.Page, "00363"))
            CheckHeader = False
            Exit Function
        End If

        If objExMultiGLHeader.BillTo = "" Then
            Common.NetMsgbox(Me, "Billing Address is required.")
            'Common.NetMsgbox(Me, MM.Message(Me.Page, "00363"))
            CheckHeader = False
            Exit Function
        End If

        'Billing Address
        Dim dsBillTo As New DataSet
        Dim dsDeliveryAdd As New DataSet
        Dim objAdmin As New Admin
        Dim _Match As Boolean = False
        dsBillTo = objAdmin.PopulateAddr("B", "", "", "")
        If dsBillTo.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In dsBillTo.Tables(0).Rows
                If objExMultiGLHeader.BillTo.ToString.Contains(row("Address")) Then
                    _Match = True
                End If
            Next
        End If
        If _Match = False Then
            Common.NetMsgbox(Me, "Billing Address does not match.")
            CheckHeader = False
            Exit Function
        End If


        'If objExMultiGLHeader.DocNo = "" Then
        '    Common.NetMsgbox(Me, "Document No. is required.")
        '    CheckHeader = False
        '    Exit Function
        'End If

        'If Not Common.checkMaxLength(objExMultiGLHeader.CoyName, 100) Then
        '    Common.NetMsgbox(Me, "Vendor Company Name is over limit.")
        '    CheckHeader = False
        '    Exit Function
        'End If

        'If Not Common.checkMaxLength(objExMultiGLHeader.DocNo, 50) Then
        '    Common.NetMsgbox(Me, "Document No. is over limit.")
        '    CheckHeader = False
        '    Exit Function
        'End If

        'ViewState("VCoyId") = objDb.GetVal("SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & objExMultiGLHeader.CoyName & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = ic_coy_id = '" & Session("CompanyId") & "' AND IC_STATUS = 'A'")
        ViewState("VCoyId") = objDb.GetVal("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = (Select cm_coy_id from company_mstr where cm_coy_name = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND CM_COY_TYPE = 'VENDOR')")
        If ViewState("VCoyId") = "" Then
            Common.NetMsgbox(Me, "The vendor company is not a valid approved vendor.")
            'Common.NetMsgbox(Me, MM.Message(Me.Page, "00347"))
            CheckHeader = False
            Exit Function
        End If

        'If IsNothing(objExMultiGLHeader.DocDate) Or IsDate(objExMultiGLHeader.DocDate) = False Then
        '    Common.NetMsgbox(Me, "Invalid Document Date.")
        '    CheckHeader = False
        '    Exit Function
        'End If

        'If strDocNo <> objExMultiGLHeader.DocNo Then
        '    Common.NetMsgbox(Me, "Document No.not match")
        '    CheckHeader = False
        '    Exit Function
        'End If
        Dim vendorName = ""
        If Not strVendor = "" Then
            vendorName = objDb.GetVal("Select cm_coy_name from company_mstr where cm_coy_id = '" & strVendor & "'")
        Else 'if it's nothing, you just have to create one
            vendorName = RTrim(objExMultiGLHeader.CoyName)
        End If
        If RTrim(vendorName) <> RTrim(objExMultiGLHeader.CoyName) Then
            Common.NetMsgbox(Me, "Vendor not match")
            'Common.NetMsgbox(Me, MM.Message(Me.Page, "00349"))
            CheckHeader = False
            Exit Function
        End If

        'If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX WHERE CDM_GROUP_CODE = '" & Common.Parse(objExConCatHeader.ConRefNo) & "' AND CDM_S_COY_ID = '" & Common.Parse(ViewState("VCoyId")) & "' AND CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
        '    Common.NetMsgbox(Me, "Duplicate record found.")
        '    CheckHeader = False
        '    Exit Function
        'End If

        'Else
        'Common.NetMsgbox(Me, "Invalid Record Action. Please specify the correct action.")
        'CheckHeader = False
        'Exit Function
        'End If

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
                Common.NetMsgbox(Me, "0 byte document or file not found")
                Return False
            ElseIf cmdBrowse.PostedFile.ContentLength / 1024 > Session("FileSize") Then ' dblMaxFileSize Then
                Common.NetMsgbox(Me, "File exceeds maximum file size")
                Return False
            End If

            strTempPath = objFileMgmt.getSystemParam("BatchUpload", "Temp", ConfigurationManager.AppSettings("eProcurePath"))
            'strTempPath = "C:\itg\Published\FileMgnt\TempProductList\" 'Nov 22
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

    Private Function AddGLEntry(ByRef pds As DataSet, ByVal pRules As myCollection, ByVal dtHeader As DataTable)
        Dim drItem As DataRow
        Dim objCat As New ContCat
        Dim pstrConnStr As String
        Dim countSave As Long = 0
        Dim countError As Long = 0
        Dim SaveHeader As Integer
        Dim dsHeader As New DataSet
        Dim bAllTrue As Boolean = True
        Dim strAryQuery(0) As String
        Dim mm As New CostAllocIPP
        Dim _sessionArray As String = ""
        Dim dsBCM As New DataSet
        Dim objBudget As New BudgetControl
        Dim objAdmin As New Admin
        '  Dim objDB As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

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
        strSQL2 = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = (Select cm_coy_id from company_mstr where cm_coy_name = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND CM_COY_TYPE = 'VENDOR')"
        Dim strVen As String
        strVen = objDb2.GetVal(strSQL2)
        If strVen = "" Then 'no exist
            'drItem.Item("Message") &= "<LI type=square>Vendor is required.<BR>"
            'Common.NetMsgbox(Me, MM.Message(Me.Page, "00363"))
            Common.NetMsgbox(Me, "Vendor is required.")
            Exit Function
        Else
            Session("VendorVal") = strVen
        End If

        'Toget Vendor Detail
        ' get supplier data
        Dim dsVendor As New DataSet
        Dim strPayTerm As String
        Dim strPayMethod As String
        Dim objPR As New PR
        Dim drvCodeMstr As DataView
        Dim myData As New AppDataProvider

        Dim getVendorId = objDb.GetVal("select cm_coy_id from sso.company_mstr where cm_coy_name = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND CM_COY_TYPE = 'VENDOR'")
        dsVendor = objPR.getVendorDetail(getVendorId)
        Dim _strSql As String = ""
        Dim _strVal As String = ""

        'Get PaymentTerm desc
        drvCodeMstr = myData.GetMasterCodeByStatus(CodeTable.PaymentTerm, "N")
        _strSql = "Select ISNULL(CM_PAYMENT_TERM,1) from COMPANY_MSTR WHERE CM_COY_ID='" & strVen & "'"
        _strVal = objDb.GetVal(_strSql)
        For Each row As DataRow In drvCodeMstr.Table.Rows
            If _strVal.Trim = row("CODE_ABBR") Then
                Session("PaymentTerm") = row("CODE_DESC")
                Exit For
            End If
        Next
        'End

        'Get Payment Method
        _strSql = "Select ISNULL(CM_PAYMENT_METHOD,1) from COMPANY_MSTR WHERE CM_COY_ID='" & strVen & "'"
        _strVal = objDb.GetVal(_strSql)
        drvCodeMstr = myData.GetMasterCodeByStatus(CodeTable.PaymentMethod, "N")
        For Each row As DataRow In drvCodeMstr.Table.Rows
            If _strVal.Trim = row("CODE_ABBR") Then
                Session("PaymentMethod") = row("CODE_DESC")
                Exit For
            End If
        Next
        'End

        If dsVendor.Tables(0).Rows.Count > 0 Then
            ViewState("strGST") = Common.parseNull(dsVendor.Tables(0).Rows(0)("CM_TAX_CALC_BY"))
            Session("BillingMethod") = dsVendor.Tables(0).Rows(0)("CV_BILLING_METHOD")
            Session("ShipmentTerm") = "Not Applicable"
            Session("ShipmentMethod") = "Not Applicable"
            Session("CurrencyCode") = dsVendor.Tables(0).Rows(0)("CM_CURRENCY_CODE")
            If IsDBNull(Session("BillingMethod")) Then
                Session("BillingMethod") = "FPO"
            End If
        End If

        Dim _counter As Integer = 1

        'check the whole data. If one failed, reject all
        Dim bNoError As Boolean = True
        Dim _hasItem As Boolean = False
        For Each drItem In pds.Tables(0).Rows
            'Zulham Okt 29, 2013
            Dim strTest = ""
            Dim objExProduct As New ExcelPOUpload
            objExProduct = GetItemDetails(0, drItem)
            Dim strErrorMsgLine As String = ""
            If objExProduct.UOM <> "" Or objExProduct.Quantity.ToString <> "" Or objExProduct.UnitPrice <> "" Or objExProduct.BudgetAccount <> "" Or objExProduct.GLCode <> "" Or objExProduct.DeliveryAddress <> "" Then
                _hasItem = True
                ' Check UOM
                strErrorMsgLine = objExProduct.No
                If objExProduct.UOM <> "" Then
                    strSQL2 = "SELECT code_desc FROM code_mstr WHERE code_category = 'UOM' AND code_desc = '" & Common.Parse(objExProduct.UOM) & "' "
                    Dim UOM_Val As String
                    UOM_Val = objDb2.GetVal(strSQL2)
                    If UOM_Val = "" Then 'no exist
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [UOM not found.]<BR>"
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [UOM not found.]"
                        bNoError = False
                        strTest = "1" 'Zulham Okt 29, 2013
                    Else
                        ' Session("UOMVal") = UOM_Val
                        objExProduct.UOM = UOM_Val
                    End If
                Else
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [UOM is required.]<BR>"
                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [UOM is required.]"
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'Check quantity
                If objExProduct.Quantity.ToString = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Quantity is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Quantity is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Quantity is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Quantity is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'Check EDD
                If objExProduct.DeliveryDate.ToString = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Quantity is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Est. Date of Delivery is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Est. Date of Delivery is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Est. Date of Delivery is required.]"
                    End If
                    bNoError = False
                    strTest = "1"
                End If

                'Check Description
                If objExProduct.Description.ToString = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Quantity is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Item Name is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Item Name is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Item Name is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'Check unit price
                If objExProduct.UnitPrice = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Unit Price is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Unit Price is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Unit Price is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Unit Price is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'GST 
                Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
                Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                Dim _exceedCutOffDt As String = ""
                Dim strIsGst = ""
                If CDate(createdDate) >= CDate(_cutoffDate) Then
                    _exceedCutOffDt = "Yes"
                    If objExMultiGLHeader.CoyName <> "" Then
                        Dim GSTRegNo = objDb.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_NAME = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND CM_COY_TYPE = 'VENDOR'")
                        If GSTRegNo <> "" Then
                            strIsGst = "Yes"
                        Else
                            strIsGst = "No"
                        End If
                    Else
                        strIsGst = "Yes"
                    End If
                Else
                    strIsGst = "No"
                End If
                If objExProduct.GSTRate = "" And strIsGst = "Yes" Then
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [SST Rate is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [SST Rate is required.]"
                    Else
                        vldsum.InnerHtml &= ", [SST Rate is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'Jules 2018.08.02 - Added checking for Input Tax.
                'If strIsGst = "Yes" AndAlso objExProduct.GSTRate <> "" AndAlso objExProduct.InputTax = "" Then
                If strIsGst = "Yes" AndAlso objExProduct.InputTax = "" Then
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Purchase Tax Code is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Purchase Tax Code is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Purchase Tax Code is required.]"
                    End If
                    bNoError = False
                    strTest = "1"
                ElseIf strIsGst = "Yes" AndAlso objExProduct.GSTRate <> "" AndAlso objExProduct.InputTax <> "" Then
                    Dim objGST As New GST
                    If objGST.chkValidTaxCode(objExProduct.GSTRate, objExProduct.InputTax, "P") = False Then
                        If vldsum.InnerHtml = "" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Invalid Purchase Tax Code.]"
                        ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Invalid Purchase Tax Code.]"
                        Else
                            vldsum.InnerHtml &= ", [Invalid Purchase Tax Code.]"
                        End If
                        bNoError = False
                        strTest = "1"
                    End If
                Else
                    'If vldsum.InnerHtml = "" Then
                    '    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Invalid Purchase Tax Code.]"
                    'ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                    '    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Invalid Purchase Tax Code.]"
                    'Else
                    '    vldsum.InnerHtml &= ", [Invalid Purchase Tax Code.]"
                    'End If
                    'bNoError = False
                    'strTest = "1"
                End If
                'End modification.

                'BA invalid
                If objExProduct.BudgetAccount <> "" Then
                    Dim _bcmCount = CInt(objPR.checkBCM)
                    Dim _bcmIdx As Integer = 0
                    If _bcmCount > 0 Then
                        dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")
                    End If
                    If dsBCM.Tables(0).Rows.Count > 0 Then
                        For i As Integer = 0 To dsBCM.Tables(0).Rows.Count - 1
                            With dsBCM.Tables(0)
                                If .Rows(i).Item(0) = Common.parseNull(objExProduct.BudgetAccount) Then
                                    _bcmIdx = .Rows(i).Item(1)
                                    Exit For
                                End If
                            End With
                        Next
                    End If
                    If _bcmIdx = 0 Then
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Budget Account not found.]<BR>"
                        If vldsum.InnerHtml = "" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Budget Account not found.]"
                        ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Budget Account not found.]"
                        Else
                            vldsum.InnerHtml &= ", [Budget Account not found.]"
                        End If
                        bNoError = False
                        strTest = "1" 'Zulham Okt 29, 2013
                    End If
                End If

                'BA empty
                If objExProduct.BudgetAccount.ToString = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Budget Account is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Budget Account is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Budget Account is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Budget Account is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                If objExProduct.GLCode <> "" Then
                    strSQL2 = "SELECT CBG_B_GL_CODE FROM COMPANY_B_GL_CODE WHERE CBG_STATUS = 'A' and CBG_B_GL_CODE = '" & Common.Parse(objExProduct.GLCode) & "' AND CBG_B_COY_ID = '" & Session("CompanyId") & "' "
                    Dim CBG_B_GL_Code As String
                    CBG_B_GL_Code = objDb2.GetVal(strSQL2)
                    If CBG_B_GL_Code = "" Then 'no exist
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [GL Code not found.]<BR>"
                        If vldsum.InnerHtml = "" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [GL Code not found.]"
                        ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [GL Code not found.]"
                        Else
                            vldsum.InnerHtml &= ", [GL Code not found.]"
                        End If
                        bNoError = False
                        strTest = "1" 'Zulham Okt 29, 2013
                    Else
                        objExProduct.GLCode = CBG_B_GL_Code

                        'Jules 2018.07.17 - PAMB
                        If objExProduct.GLCode.Substring(0, 1) = "1" AndAlso objExProduct.Gift.Contains("Y") Then
                            If vldsum.InnerHtml = "" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [CAPEX item cannot be Gift.]"
                            ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [CAPEX item cannot be Gift.]"
                            Else
                                vldsum.InnerHtml &= ", [CAPEX item cannot be Gift.]"
                            End If
                            bNoError = False
                            strTest = "1"
                        End If

                        Dim strSqlAC As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & objExProduct.GLCode & "'"
                        Dim dsAnalysisCodes As DataSet = objDb.FillDs(strSqlAC)
                        If dsAnalysisCodes.Tables(0).Rows.Count > 0 Then
                            If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And objExProduct.FundType = "" Then
                                If vldsum.InnerHtml = "" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Fund Type is required.]"
                                ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Fund Type is required.]"
                                Else
                                    vldsum.InnerHtml &= ", [Fund Type is required.]"
                                End If
                                bNoError = False
                                strTest = "1"
                            End If
                            If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And objExProduct.ProjectCode = "" Then
                                If vldsum.InnerHtml = "" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Project Code is required.]"
                                ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Project Code is required.]"
                                Else
                                    vldsum.InnerHtml &= ", [Project Code is required.]"
                                End If
                                bNoError = False
                                strTest = "1"
                            End If
                            If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And objExProduct.PersonCode = "" Then
                                If vldsum.InnerHtml = "" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Person Code is required.]"
                                ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Person Code is required.]"
                                Else
                                    vldsum.InnerHtml &= ", [Person Code is required.]"
                                End If
                                bNoError = False
                                strTest = "1"
                            End If
                        End If
                    End If
                Else
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [GL Code is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [GL Code is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [GL Code is required.]"
                    Else
                        vldsum.InnerHtml &= ", [GL Code is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                If objExProduct.CategoryCode = "" Then
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Category Code is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Category Code is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Category Code is required.]"
                    End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                If objExProduct.GLCode <> "" Then
                    Dim strCat As String = objDb.GetVal("SELECT IFNULL(GROUP_CONCAT(CBGC_B_CATEGORY_CODE), '') AS CBGC_B_CATEGORY_CODE FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & objExProduct.GLCode & "' AND (SELECT COUNT(CBGC_B_GL_CODE) FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & objExProduct.GLCode & "') > 0")
                    If strCat <> "" Then
                        If objExProduct.GLCode <> "" And Not strCat.Contains(objExProduct.CategoryCode) Then
                            If vldsum.InnerHtml = "" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00370") & "]"
                            ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [" & objGlobal.GetErrorMessage("00370") & "]"
                            Else
                                vldsum.InnerHtml &= ", " & objGlobal.GetErrorMessage("00370") & ""
                            End If
                            bNoError = False 'Error
                            strTest = "1" 'Zulham Okt 29, 2013
                        End If
                    End If
                End If

                'Got To do Some chcking for the validity of the addresses based on current user's id

                'Delivery Address
                Dim dsBillTo As New DataSet
                Dim dsDeliveryAdd As New DataSet

                If objExProduct.DeliveryAddress <> "" Then
                    Dim _Match As Boolean = False
                    dsBillTo = objAdmin.PopulateAddr("B", "", "", "")
                    dsDeliveryAdd = objAdmin.PopulateAddr("D", "", "", "")
                    If dsDeliveryAdd.Tables(0).Rows.Count > 0 Then
                        For Each row As DataRow In dsDeliveryAdd.Tables(0).Rows
                            Dim straddress = row("Address") & ","
                            'If Not row("AM_CITY") Is DBNull.Value Then
                            '    straddress &= row("AM_CITY") & ","
                            'End If
                            'If Not row("STATE") Is DBNull.Value Then
                            '    straddress &= row("STATE") & ","
                            'End If
                            'If Not row("AM_POSTCODE") Is DBNull.Value Then
                            '    straddress &= row("AM_POSTCODE") & ","
                            'End If
                            'If Not row("COUNTRY") Is DBNull.Value Then
                            '    straddress &= row("COUNTRY") & ","
                            'End If
                            If Not row("AM_ADDR_CODE") Is DBNull.Value Then
                                straddress = row("AM_ADDR_CODE")
                            End If
                            If objExProduct.DeliveryAddress = straddress Then
                                _Match = True
                            End If
                        Next
                    End If
                    If _Match = False Then
                        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Delivery Address does not match.]<BR>"
                        If vldsum.InnerHtml = "" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Delivery code does not match.]"
                        ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                            vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Delivery code does not match.]"
                        Else
                            vldsum.InnerHtml &= ", [Delivery code does not match.]"
                        End If

                        bNoError = False
                        strTest = "1" 'Zulham Okt 29, 2013
                    End If
                End If

                'Delivery Address
                If objExProduct.DeliveryAddress.ToString = "" Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Delivery Address is required.]<BR>"
                    If vldsum.InnerHtml = "" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Delivery code is required.]"
                    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Delivery code is required.]"
                    Else
                        vldsum.InnerHtml &= ", [Delivery code is required.]"
                    End If

                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If

                'Zulham Oct 24, 2013
                'Validation for Custom fields, if there's any
                If Not Session("CustomFields") Is Nothing Then
                    'Iterate thru the data
                    Dim dtCustom As New Dictionary(Of String, String) : dtCustom = CType(Session("CustomFields"), Dictionary(Of String, String))
                    Dim pair As KeyValuePair(Of String, String)
                    Dim _col As Integer = 1
                    For Each pair In dtCustom
                        If pair.Value.ToString.Trim = "" Then
                            'No data selected
                            If vldsum.InnerHtml = "" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Custom Field " & _col & " value is required.]"
                            ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Custom Field " & _col & " value is required.]"
                            Else
                                vldsum.InnerHtml &= ", [Custom Field " & _col & " value is required.]"
                            End If
                            bNoError = False
                            strTest = "1" 'Zulham Okt 29, 2013
                            'Zulham Okt 30, 2013
                        Else
                            'Checking data validity
                            Dim dsCustomName, dsCustomField As New DataSet
                            Dim _boolValid As Boolean = False
                            Dim strSql As String = ""

                            dsCustomName = objAdmin.getCustomFieldDS("", "PO")
                            For i As Integer = 0 To dsCustomName.Tables(0).Rows.Count - 1
                                If i = _col - 1 Then
                                    strSql = "SELECT CF_FIELD_INDEX, CF_FIELD_VALUE, REPLACE(CF_FIELD_VALUE,' ','_') AS FIELDVALUE FROM CUSTOM_FIELDS WHERE CF_FIELD_NO ='" & dsCustomName.Tables(0).Rows(i).Item("CF_FIELD_NO") & "' " & _
                                             "AND CF_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                                    strSql &= " AND CF_MODULE='PO'"
                                    strSql = strSql & " ORDER BY CF_FIELD_VALUE"
                                    dsCustomField = objDb.FillDs(strSql)
                                    For _row As Integer = 0 To dsCustomField.Tables(0).Rows.Count - 1
                                        If dsCustomField.Tables(0).Rows(_row).Item("CF_FIELD_VALUE") = pair.Value.ToString.Trim Then
                                            _boolValid = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            Next
                            'End
                            If _boolValid = False Then
                                If vldsum.InnerHtml = "" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Custom Field " & _col & " value is invalid.]"
                                ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                                    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Custom Field " & _col & " value is invalid.]"
                                Else
                                    vldsum.InnerHtml &= ", [Custom Field " & _col & " value is invalid.]"
                                End If
                                bNoError = False
                                strTest = "1"
                            End If
                        End If
                        _col += 1
                    Next
                End If
                'End

                'Billing Address
                'If dsHeader.Tables(0).Rows.Count > 0 Then
                '    Dim _Match As Boolean = False
                '    dsBillTo = objAdmin.PopulateAddr("B", "", "", "")
                '    If dsBillTo.Tables(0).Rows.Count > 0 Then
                '        For Each row As DataRow In dsBillTo.Tables(0).Rows
                '            If dsHeader.Tables(0).Rows(0).Item(1).ToString.Contains(row("Address")) Then
                '                _Match = True
                '            End If
                '        Next
                '    End If
                '    If _Match = False Then
                '        'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Billing Address does not match.]<BR>"
                '    If vldsum.InnerHtml = "" Then
                '        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Billing Address does not match.]"
                '    ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                '        vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Billing Address does not match.]"
                '    Else
                '        vldsum.InnerHtml &= ", [Billing Address does not match.]"
                '    End If
                '    bNoError = False
                '    End If
                'End If

                'Billing Address
                If dsHeader.Tables(0).Rows(0).Item(1).ToString.Trim.Length = 0 Then
                    'vldsum.InnerHtml &= "<LI type=square>Line No. " & objExProduct.No & " [Billing Address is required.]<BR>"
                    'If vldsum.InnerHtml = "" Then
                    '    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Billing Address is required.]"
                    'ElseIf vldsum.InnerHtml.ToString.Substring(vldsum.InnerHtml.ToString.Length - 4, 4) = "<BR>" Then
                    '    vldsum.InnerHtml &= "Line No. " & objExProduct.No & " [Billing Address is required.]"
                    'Else
                    '    vldsum.InnerHtml &= ", [Billing Address is required.]"
                    'End If
                    bNoError = False
                    strTest = "1" 'Zulham Okt 29, 2013
                End If
            Else
                If _hasItem = False Then
                    Common.NetMsgbox(Me, "No record(s) found.", MsgBoxStyle.Information)
                    Return False
                    Exit Function
                End If
            End If
            'If Not vldsum.InnerHtml = "" Then
            If Not strTest = "" Then
                vldsum.InnerHtml &= "<BR>"
            End If
            Dim _1 = vldsum.InnerHtml.Length
            'End If
        Next
        If bNoError = False Then
            Return False
            Exit Function
        End If
        'End
        Dim _rowCount = 1
        RemovePODetails() 'Nov 22, 2013
        For Each drItem In pds.Tables(0).Rows
            Dim bRtn As Boolean = True
            Dim bTrue As Boolean = True
            Dim bAllColTrue As Boolean = True
            'Zulham Nov 1, 2013
            Dim _array(0) As String
            Dim _dict As New Dictionary(Of String, String) : _dict = Session("CustomFields")
            Dim objExProduct As New ExcelPOUpload
            Try

                objExProduct = GetItemDetails(0, drItem)
                If objExProduct.UOM <> "" Or objExProduct.Quantity.ToString <> "" Or objExProduct.UnitPrice <> "" Or objExProduct.BudgetAccount <> "" Or objExProduct.GLCode <> "" Or objExProduct.DeliveryAddress <> "" Then
                Else
                    Exit For
                End If

                If bTrue = False Then
                    bAllTrue = False
                    bAllColTrue = False
                End If
                'Next

                If bAllColTrue = True Then
                    Dim dsItemDetails As New DataSet

                    dsItemDetails = bindItems(objExProduct)
                    If Not strDocNo.ToString = "" Then
                        Dim objIPP As New PurchaseOrder_Buyer
                        If objIPP.insertPODetails(dsItemDetails, strDocNo, True, True, Session("CustomFields"), _array, dtHeader, _rowCount) = 1 Then
                            If Not HttpContext.Current.Session("NewPoInfo") Is Nothing Then
                                strDocNo = HttpContext.Current.Session("NewPoInfo").ToString.Split("|")(0)
                                _sessionArray = HttpContext.Current.Session("NewPoInfo")
                            End If
                        ElseIf strDocNo = "To Be Allocated By System" Then
                            Common.NetMsgbox(Me, "Error: " & 1 & " record(s).", MsgBoxStyle.Information)
                        End If
                    End If
                Else
                End If
                If Not Me.lblPONo.Text = "To Be Allocated By System" Then
                    If _dict.Count = 0 Then
                        If objDb.BatchExecute(_array) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    Else
                        If objDb.BatchExecute(HttpContext.Current.Session("Test")) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    End If
                ElseIf lblPONo.Text = "To Be Allocated By System" And _counter > 1 Then
                    If _dict.Count = 0 Then
                        If objDb.BatchExecute(_array) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    Else
                        If objDb.BatchExecute(HttpContext.Current.Session("Test")) Then
                            countSave = countSave + 1
                        Else
                            countError = countError + 1
                        End If
                    End If
                End If
                _counter += 1
                _rowCount += 1
                objExProduct = Nothing
                'End If
            Catch ex As Exception

            End Try
        Next
        ' Common.NetMsgbox(Me, "Successfull: " & 1 & " record(s).", MsgBoxStyle.Information)

        If bAllTrue = True Then

        End If

        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strscript.Append("<script language=""javascript"">")
        strscript.Append("UnBlockUploadProgress();")
        strscript.Append("</script>")
        RegisterStartupScript("script5", strscript.ToString())

        If lblPONo.Text = "To Be Allocated By System" Then
            Common.NetMsgbox(Me, "Successfull: " & countSave + 1 & " record(s).") '& """& vbCrLf & """ & "Error: " & countError & " record(s).", MsgBoxStyle.Information)
            Return True
        Else
            Common.NetMsgbox(Me, "Successfull: " & countSave & " record(s).")
        End If

        If HttpContext.Current.Session("NewPoInfo") Is Nothing Then
            HttpContext.Current.Session("NewPoInfo") = _sessionArray
        End If
    End Function

    Private Sub Clear()
        ViewState("Index") = 0
        ViewState("ProductCode") = ""
        ViewState("VendorItemCode") = ""
        ViewState("ProductDesc") = ""
        ViewState("UOM") = ""
    End Sub

    Private Function bindItems(ByRef pGLEntryDetails As ExcelPOUpload) As DataSet
        Try
            Dim ds, dsGSTRate As New DataSet
            Dim dtItem As New DataTable
            Dim dtr As DataRow
            Dim objPR As New PR
            Dim objBudget As New BudgetControl
            Dim dsBCM As New DataSet
            Dim createdDate As String
            Dim _BAIdx = 0
            Dim _exceedCutOffDate As String = ""
            ViewState("BCM") = CInt(objPR.checkBCM)
            If ViewState("BCM") > 0 Then
                dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")
            End If
            If dsBCM.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To dsBCM.Tables(0).Rows.Count - 1
                    With dsBCM.Tables(0)
                        If .Rows(i).Item(0) = Common.parseNull(pGLEntryDetails.BudgetAccount) Then
                            _BAIdx = .Rows(i).Item(1)
                        End If
                    End With
                Next
            End If
            dtItem.Columns.Add("No", Type.GetType("System.Int32"))
            dtItem.Columns.Add("Description", Type.GetType("System.String"))
            dtItem.Columns.Add("GLCode", Type.GetType("System.String"))
            dtItem.Columns.Add("CategoryCode", Type.GetType("System.String"))
            dtItem.Columns.Add("AssetGroup", Type.GetType("System.String"))
            dtItem.Columns.Add("AssetGroupNo", Type.GetType("System.String"))
            dtItem.Columns.Add("Quantity", Type.GetType("System.String"))
            dtItem.Columns.Add("UOM", Type.GetType("System.String"))
            dtItem.Columns.Add("UnitPrice", Type.GetType("System.String"))
            dtItem.Columns.Add("Tax", Type.GetType("System.String"))
            dtItem.Columns.Add("BudgetAccount", Type.GetType("System.String"))
            dtItem.Columns.Add("DeliveryAddress", Type.GetType("System.String"))
            dtItem.Columns.Add("DeliveryDate", Type.GetType("System.String"))
            dtItem.Columns.Add("WarrantyTerms", Type.GetType("System.String"))
            dtItem.Columns.Add("Remarks", Type.GetType("System.String"))
            dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
            dtItem.Columns.Add("GSTTaxValue", Type.GetType("System.String"))

            'Jules 2018.07.17 - PAMB
            dtItem.Columns.Add("Gift", Type.GetType("System.String"))
            dtItem.Columns.Add("FundType", Type.GetType("System.String"))
            dtItem.Columns.Add("PersonCode", Type.GetType("System.String"))
            dtItem.Columns.Add("ProjectCode", Type.GetType("System.String"))
            dtItem.Columns.Add("TaxCode", Type.GetType("System.String"))
            'End modification.

            dtr = dtItem.NewRow()
            dtr("No") = pGLEntryDetails.No
            dtr("Description") = Common.Parse(pGLEntryDetails.Description)

            'Jules 2018.07.17 - PAMB
            'dtr("Gift") = IIf(Common.Parse(pGLEntryDetails.Gift) = "", "N", Common.Parse(pGLEntryDetails.Gift))
            dtr("Gift") = IIf(Common.Parse(pGLEntryDetails.Gift) = "" Or Common.Parse(pGLEntryDetails.Gift).ToUpper = "NO", "N", "Y")

            'Jules 2018.10.18
            Dim intStart As Integer = 0
            If InStr(Common.Parse(pGLEntryDetails.FundType), ":") Then
                'dtr("FundType") = Common.Parse(pGLEntryDetails.FundType).Substring(0, InStr(Common.Parse(pGLEntryDetails.FundType), ":") - 1).Trim
                intStart = InStr(Common.Parse(pGLEntryDetails.FundType), ":") + 1
                dtr("FundType") = Common.Parse(pGLEntryDetails.FundType).Substring(intStart, Common.Parse(pGLEntryDetails.FundType).Length - intStart).Trim
            Else
                dtr("FundType") = ""
            End If

            If InStr(Common.Parse(pGLEntryDetails.PersonCode), ":") Then
                'dtr("PersonCode") = Common.Parse(pGLEntryDetails.PersonCode).Substring(0, InStr(Common.Parse(pGLEntryDetails.PersonCode), ":") - 1).Trim
                intStart = InStr(Common.Parse(pGLEntryDetails.PersonCode), ":") + 1
                dtr("PersonCode") = Common.Parse(pGLEntryDetails.PersonCode).Substring(intStart, Common.Parse(pGLEntryDetails.PersonCode).Length - intStart).Trim
            ElseIf Common.Parse(pGLEntryDetails.PersonCode) <> "" AndAlso Common.Parse(pGLEntryDetails.PersonCode).ToString.Contains("N/A") Then 'Jules 2018.09.05
                dtr("PersonCode") = "N/A"
            Else
                dtr("PersonCode") = ""
            End If

            If InStr(Common.Parse(pGLEntryDetails.ProjectCode), ":") Then
                'dtr("ProjectCode") = Common.Parse(pGLEntryDetails.ProjectCode).Substring(0, InStr(Common.Parse(pGLEntryDetails.ProjectCode), ":") - 1).Trim
                intStart = InStr(Common.Parse(pGLEntryDetails.ProjectCode), ":") + 1
                dtr("ProjectCode") = Common.Parse(pGLEntryDetails.ProjectCode).Substring(intStart, Common.Parse(pGLEntryDetails.ProjectCode).Length - intStart).Trim
            Else
                dtr("ProjectCode") = ""
            End If

            If Common.parseNull(pGLEntryDetails.InputTax) <> "" Then
                dtr("TaxCode") = Common.parseNull(pGLEntryDetails.InputTax).ToString.Split("(")(0).Trim
            End If
            'End modification.

            dtr("GLCode") = Common.Parse(pGLEntryDetails.GLCode)
            dtr("CategoryCode") = IIf(Common.Parse(pGLEntryDetails.CategoryCode) <> "" AndAlso Common.Parse(pGLEntryDetails.CategoryCode) = "N/A", "", Common.Parse(pGLEntryDetails.CategoryCode))
            dtr("AssetGroup") = Common.Parse(pGLEntryDetails.AssetGroup)
            'Zulham Okt 29, 2013
            'Get the right asset group no as there's a possibility that ':' is included in asset group code
            Dim strsql = ""
            Dim assetCode = ""
            Dim dsAsset As New DataSet
            strsql = "SELECT AG_GROUP AS ASSET_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " _
                       & " FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " _
                       & " ORDER BY AG_GROUP "
            dsAsset = objDB.FillDs(strsql)
            If Not dsAsset Is Nothing Then
                For i As Integer = 0 To dsAsset.Tables(0).Rows.Count - 1
                    If dsAsset.Tables(0).Rows(i).Item("AG_GROUP_DESC").ToString.Trim = Common.Parse(pGLEntryDetails.AssetGroup) Then
                        assetCode = dsAsset.Tables(0).Rows(i).Item("ASSET_GROUP")
                        Exit For
                    End If
                Next
            End If
            'End
            dtr("AssetGroupNo") = Common.Parse(assetCode.Trim)
            dtr("Quantity") = Common.Parse(pGLEntryDetails.Quantity)
            dtr("UOM") = Common.Parse(pGLEntryDetails.UOM)
            dtr("UnitPrice") = Common.Parse(pGLEntryDetails.UnitPrice)
            If pGLEntryDetails.Tax.ToString = "" Then dtr("Tax") = 0.0 Else dtr("Tax") = pGLEntryDetails.Tax.ToString
            If Not _BAIdx = 0 Then
                dtr("BudgetAccount") = _BAIdx 'Common.parseNull(pGLEntryDetails.BudgetAccount)
            Else
                dtr("BudgetAccount") = 0
            End If
            dtr("DeliveryAddress") = Common.Parse(pGLEntryDetails.DeliveryAddress)
            dtr("DeliveryDate") = Common.Parse(Common.parseNull(pGLEntryDetails.DeliveryDate))
            dtr("WarrantyTerms") = Common.Parse(Common.parseNull(pGLEntryDetails.WarrantyTerms))
            dtr("Remarks") = Common.Parse(Common.parseNull(pGLEntryDetails.Remarks))

            'Check for GST
            'check whether gst is applicable or otherwise
            Dim boolchkGSTCod As Boolean
            Dim strIsGst As String
            Dim strGstCOD = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            createdDate = objDB.GetVal("SELECT pom_created_date FROM po_mstr WHERE pom_po_no = '" & Me.lblPONo.Text & "'  AND pom_s_coy_id = '" & strVendor & "'")
            createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, createdDate)
            If createdDate <> "" Then
                If CDate(createdDate) >= CDate(strGstCOD) Then
                    boolchkGSTCod = True
                Else
                    boolchkGSTCod = False
                End If
            Else
                If Date.Now() >= CDate(strGstCOD) Then
                    _exceedCutOffDate = "Yes"
                    boolchkGSTCod = True
                Else
                    boolchkGSTCod = False
                End If
            End If
            Dim createdDate2 = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            If CDate(createdDate2) >= CDate(System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")) Then

                'Jules 2018.09.05
                'Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Request.QueryString("vencomp") & "'")
                Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = (Select cm_coy_id from company_mstr where cm_coy_name = '" & Common.Parse(objExMultiGLHeader.CoyName) & "' AND CM_COY_TYPE = 'VENDOR')")
                'End modification.

                If GSTRegNo <> "" Then
                    boolchkGSTCod = True
                Else
                    boolchkGSTCod = False
                End If
            End If

            If boolchkGSTCod Then
                'Jules 2018.09.05 - Added Tax Perc.
                strsql = "SELECT IF(TAX_PERC = '', CODE_DESC," &
                        "CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST, CODE_ABBR, TAX_PERC " &
                        "FROM CODE_MSTR " &
                        "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND " &
                        "TAX_COUNTRY_CODE = 'MY' " &
                        "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' "
                dsGSTRate = objDB.FillDs(strsql)
                For i As Integer = 0 To dsGSTRate.Tables(0).Rows.Count - 1
                    If dsGSTRate.Tables(0).Rows(i).Item(1).ToString.Trim = Common.Parse(Common.parseNull(pGLEntryDetails.GSTRate)).ToString.Trim Then
                        dtr("GSTRate") = dsGSTRate.Tables(0).Rows(i).Item(1).ToString.Trim
                        dtr("Tax") = dsGSTRate.Tables(0).Rows(i).Item(2).ToString.Trim 'Jules 2018.09.05
                        dtr("GSTTaxValue") = dsGSTRate.Tables(0).Rows(i).Item(2).ToString.Trim 'Jules 2018.09.05
                    End If
                Next

                'Jules commented 2018.09.05
                'Dim GSTTaxValue As Double = 0.0
                'If Not Common.Parse(Common.parseNull(pGLEntryDetails.GSTRate)).ToString.Contains("EX") _
                'And Not Common.Parse(Common.parseNull(pGLEntryDetails.GSTRate)).ToString.Contains("ZERO") _
                'And Not Common.Parse(Common.parseNull(pGLEntryDetails.GSTRate)).ToString.Contains("N/A") Then
                '    GSTTaxValue = (CDec(Common.Parse(Common.parseNull(pGLEntryDetails.GSTRate)).ToString.Split("(")(1).Replace("%", "").Replace(")", "").Trim) / 100) _
                '    * CDec(Common.Parse(pGLEntryDetails.Quantity) * Common.Parse(pGLEntryDetails.UnitPrice))
                '    dtr("Tax") = Common.parseNull(pGLEntryDetails.GSTRate).ToString.Split("(")(1).Replace("%", "").Replace(")", "").Trim
                'Else
                '    dtr("Tax") = 0.0 'Zulham Oct 2, 2014
                'End If
                'dtr("GSTTaxValue") = GSTTaxValue
                'End commented block.
            Else
                If _exceedCutOffDate = "Yes" Then
                    dtr("GSTRate") = "N/A"
                    dtr("GSTTaxValue") = 0.0
                    dtr("Tax") = 0.0
                Else
                    dtr("GSTRate") = 0
                    dtr("GSTTaxValue") = 0.0
                End If
            End If

            dtItem.Rows.Add(dtr)
            ds.Tables.Add(dtItem)

            bindItems = ds
        Catch ex As Exception

        End Try
    End Function

    Private Function bindHeader(ByVal MultiGLHeader As ExcelPOUplopadHeader) As DataSet

        Dim ds As New DataSet
        Dim dtHeader As New DataTable
        Dim dtr As DataRow

        'Contract Header
        dtHeader.Columns.Add("VendorID", Type.GetType("System.String"))
        dtHeader.Columns.Add("BillTo", Type.GetType("System.String"))
        dtr = dtHeader.NewRow()
        dtr("VendorID") = Common.Parse(MultiGLHeader.CoyName)
        dtr("BillTo") = Common.Parse(MultiGLHeader.BillTo)
        dtHeader.Rows.Add(dtr)
        ds.Tables.Add(dtHeader)
        bindHeader = ds

    End Function

    Private Function GetItemDetails(ByVal iRow As Integer, ByVal pdr As DataRow) As ExcelPOUpload
        Try

            Dim objExcel As New ExcelPOUpload
            Dim dictCust As New Dictionary(Of String, String)
            Session("CustomFields") = Nothing

            'Zulham Oct 24, 2013
            'Check whether this company has asset group
            Dim strsql = ""
            Dim dsAsset As New DataSet
            strSql = "SELECT AG_GROUP AS ASSET_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " _
                       & " FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " _
                       & " ORDER BY AG_GROUP "
            dsAsset = objDB.FillDs(strSql)
            Session("dsAsset") = dsAsset

            If Session("dsAsset") IsNot Nothing Then
                If CType(Session("dsAsset"), DataSet).Tables(0).Rows.Count = 0 Then
                    objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
                    objExcel.Description = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))

                    'Jules 2018.07.17 - Added Gift, Fund Type, Person Code, Project Code, Input Tax.
                    objExcel.Gift = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
                    objExcel.FundType = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
                    objExcel.PersonCode = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
                    objExcel.ProjectCode = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))

                    'Jules 2018.09.05 - Need to get GL Code only from the string.
                    'objExcel.GLCode = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7")) 'F3 onwards
                    If IsDBNull(pdr.Item("F7")) Then
                        objExcel.GLCode = ""
                    Else
                        'If InStr(pdr.Item("F7"), "(") = 1 Then
                        If InStr(pdr.Item("F7"), "(") > 0 Then
                            'Jules 2018.11.05
                            'objExcel.GLCode = pdr.Item("F7").ToString.Substring(1, InStr(pdr.Item("F7"), ")") - 2)
                            objExcel.GLCode = pdr.Item("F7").ToString.Substring(InStr(pdr.Item("F7"), "("), InStr(pdr.Item("F7"), ")") - InStr(pdr.Item("F7"), "(") - 1)
                            'End modification.
                        Else
                            objExcel.GLCode = pdr.Item("F7")
                        End If
                    End If
                    'End 2018.09.05 modification.

                    objExcel.CategoryCode = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
                    objExcel.Quantity = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
                    objExcel.UOM = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
                    objExcel.UnitPrice = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
                    'objExcel.Tax = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12")) 'Jules commented 2018.09.05

                    'Jules 2018.09.05 - Need to get Tax Code only from the string.
                    'objExcel.InputTax = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))
                    If IsDBNull(pdr.Item("F13")) Then
                        objExcel.InputTax = ""
                    Else
                        If InStr(pdr.Item("F13"), "(") > 0 Then
                            objExcel.InputTax = pdr.Item("F13").ToString.Substring(0, InStr(pdr.Item("F13"), " (") - 1)
                        Else
                            objExcel.InputTax = pdr.Item("F13")
                        End If
                    End If
                    'End 2018.09.05 modification.

                    objExcel.BudgetAccount = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
                    objExcel.DeliveryAddress = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
                    objExcel.DeliveryDate = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
                    objExcel.WarrantyTerms = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
                    objExcel.Remarks = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))

                    'Jules 2018.09.05 - Need to get the GST Rate.
                    'objExcel.GSTRate = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12")) 'Zulham Oct 02, 2014
                    If IsDBNull(pdr.Item("F12")) Then
                        objExcel.GSTRate = ""
                        objExcel.Tax = ""
                    Else
                        Dim strGSTRate As String = ""
                        Dim strGSTPerc As String = ""
                        If InStr(pdr.Item("F12"), "%") > 0 Then
                            strGSTPerc = CInt(pdr.Item("F12").ToString.Split("(")(1).Replace("%", "").Replace(")", "").Trim)
                        End If

                        strGSTRate = objDB.GetVal("SELECT CODE_ABBR FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' WHERE TAX_PERC = '" & strGSTPerc & "' AND CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N'")
                        objExcel.GSTRate = strGSTRate
                        objExcel.Tax = strGSTRate
                    End If
                    'End 2018.09.05 modification.

                    'objExcel. = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))

                    'Custom fields start at F14 - Jules 2018.07.17 - Changed to 19.
                    If pdr.Table.Columns.Count > 19 Then 'Checking for the existance of custom fields
                        Dim colNo As String = "F"
                        Dim objAdmin As New Admin
                        Dim dvwCus As New DataView
                        dvwCus = objAdmin.getCustomField("", "PO")
                        If Not dvwCus Is Nothing Then
                            For i As Integer = 0 To dvwCus.Count - 1
                                colNo = "F" & 19 + i '14
                                dictCust.Add(dvwCus.ToTable.Rows(i).Item(1), IIf(IsDBNull(pdr.Item(colNo.ToString)), "", pdr.Item(colNo.ToString)))
                            Next
                        End If
                    End If
                    Session("CustomFields") = dictCust
                Else
                    With objExcel
                        objExcel.No = IIf(IsDBNull(pdr.Item("F1")), 0, pdr.Item("F1"))
                        objExcel.Description = IIf(IsDBNull(pdr.Item("F2")), "", pdr.Item("F2"))

                        'Jules 2018.07.17 - Added Gift, Fund Type, Person Code, Project Code
                        objExcel.Gift = IIf(IsDBNull(pdr.Item("F3")), "", pdr.Item("F3"))
                        objExcel.FundType = IIf(IsDBNull(pdr.Item("F4")), "", pdr.Item("F4"))
                        objExcel.PersonCode = IIf(IsDBNull(pdr.Item("F5")), "", pdr.Item("F5"))
                        objExcel.ProjectCode = IIf(IsDBNull(pdr.Item("F6")), "", pdr.Item("F6"))

                        'Jules 2018.09.05 - Need to get GL Code only from the string.
                        'objExcel.GLCode = IIf(IsDBNull(pdr.Item("F7")), "", pdr.Item("F7")) 'F3 onwards
                        If IsDBNull(pdr.Item("F7")) Then
                            objExcel.GLCode = ""
                        Else
                            'If InStr(pdr.Item("F7"), "(") = 1 Then
                            If InStr(pdr.Item("F7"), "(") > 0 Then
                                'Jules 2018.11.05
                                'objExcel.GLCode = pdr.Item("F7").ToString.Substring(1, InStr(pdr.Item("F7"), ")") - 2)
                                objExcel.GLCode = pdr.Item("F7").ToString.Substring(InStr(pdr.Item("F7"), "("), InStr(pdr.Item("F7"), ")") - InStr(pdr.Item("F7"), "(") - 1)
                                'End modification.
                            Else
                                objExcel.GLCode = pdr.Item("F7")
                            End If
                        End If
                        'End 2018.09.05 modification.

                        objExcel.CategoryCode = IIf(IsDBNull(pdr.Item("F8")), "", pdr.Item("F8"))
                        objExcel.AssetGroup = IIf(IsDBNull(pdr.Item("F9")), "", pdr.Item("F9"))
                        objExcel.Quantity = IIf(IsDBNull(pdr.Item("F10")), "", pdr.Item("F10"))
                        objExcel.UOM = IIf(IsDBNull(pdr.Item("F11")), "", pdr.Item("F11"))
                        objExcel.UnitPrice = IIf(IsDBNull(pdr.Item("F12")), "", pdr.Item("F12"))
                        objExcel.Tax = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13"))

                        'Jules 2018.09.05 - Need to get Tax Code only from the string.
                        'objExcel.InputTax = IIf(IsDBNull(pdr.Item("F14")), "", pdr.Item("F14"))
                        If IsDBNull(pdr.Item("F14")) Then
                            objExcel.InputTax = ""
                        Else
                            If InStr(pdr.Item("F14"), "(") > 0 Then
                                objExcel.InputTax = pdr.Item("F14").ToString.Substring(0, InStr(pdr.Item("F14"), " (") - 1)
                            Else
                                objExcel.InputTax = pdr.Item("F14")
                            End If
                        End If
                        'End 2018.09.05 modification.

                        objExcel.BudgetAccount = IIf(IsDBNull(pdr.Item("F15")), "", pdr.Item("F15"))
                        objExcel.DeliveryAddress = IIf(IsDBNull(pdr.Item("F16")), "", pdr.Item("F16"))
                        objExcel.DeliveryDate = IIf(IsDBNull(pdr.Item("F17")), "", pdr.Item("F17"))
                        objExcel.WarrantyTerms = IIf(IsDBNull(pdr.Item("F18")), "", pdr.Item("F18"))
                        objExcel.Remarks = IIf(IsDBNull(pdr.Item("F19")), "", pdr.Item("F19"))

                        'Jules 2018.09.05 - Need to get the GST Rate.
                        'objExcel.GSTRate = IIf(IsDBNull(pdr.Item("F13")), "", pdr.Item("F13")) 'Zulham Oct 02, 2014
                        If IsDBNull(pdr.Item("F13")) Then
                            objExcel.GSTRate = ""
                            objExcel.Tax = ""
                        Else
                            Dim strGSTRate As String = ""
                            Dim strGSTPerc As String = ""
                            If InStr(pdr.Item("F13"), "%") > 0 Then
                                strGSTPerc = CInt(pdr.Item("F13").ToString.Split("(")(1).Replace("%", "").Replace(")", "").Trim)
                            End If

                            strGSTRate = objDB.GetVal("SELECT CODE_ABBR FROM CODE_MSTR INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' WHERE TAX_PERC = '" & strGSTPerc & "' AND CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N'")
                            objExcel.GSTRate = strGSTRate
                            objExcel.Tax = strGSTRate
                        End If
                        'End 2018.09.05 modification.
                    End With

                    'CustomFields start at F15 - Jules 2018.07.17 - Changed to 20.
                    If pdr.Table.Columns.Count > 20 Then 'Checking for the existance of custom fields
                        Dim colNo As String = "F"
                        Dim objAdmin As New Admin
                        Dim dvwCus As New DataView
                        dvwCus = objAdmin.getCustomField("", "PO")
                        If Not dvwCus Is Nothing Then
                            For i As Integer = 0 To dvwCus.Count - 1
                                colNo = "F" & 20 + i
                                dictCust.Add(dvwCus.ToTable.Rows(i).Item(1), IIf(IsDBNull(pdr.Item(colNo.ToString)), "", pdr.Item(colNo.ToString)))
                            Next
                        End If
                    End If
                    Session("CustomFields") = dictCust
                End If
            End If

            Return objExcel
        Catch ex As Exception

        End Try
    End Function



    Private Function GetMultiGLHeader(ByVal dt As DataTable) As ExcelPOUplopadHeader
        Dim objExcel As New ExcelPOUplopadHeader

        With objExcel
            objExcel.CoyName = IIf(IsDBNull(dt.Rows(0).Item("CoyName")), "", dt.Rows(0).Item("CoyName"))
            objExcel.BillTo = IIf(IsDBNull(dt.Rows(0).Item("BillTo")), "", dt.Rows(0).Item("BillTo"))
        End With

        Return objExcel

    End Function

    Function Filedownload()
        Dim _fileName = "POTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls"
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & _fileName & """")
        Response.Flush()
        Response.WriteFile(ConfigurationManager.AppSettings("TemplateTemp") & "POTemplate_" & HttpContext.Current.Session("CompanyId") & "_" & HttpContext.Current.Session("UserId") & ".xls")
    End Function

    Public Sub RemovePODetails()

        Dim strsql = ""
        'Zulham Nov 22, 2013
        'Remove all the existing PO_Details along with the custom fields data 
        If Not strPONo = "To Be Allocated By System" Then
            strsql = "SELECT * FROM PO_DETAILS WHERE POD_PO_NO = '" & Me.lblPONo.Text & "' "
            strsql &= "AND pod_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' "
            If objDB.Exist(strsql) > 0 Then
                strsql = "Delete from PO_DETAILS WHERE POD_PO_NO = '" & Me.lblPONo.Text & "' "
                strsql &= "AND pod_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' "
                objDB.Execute(strsql)
            End If

            strsql = "select * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX IN "
            strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
            strsql &= "WHERE POM_PO_NO = '" & Me.lblPONo.Text & "' "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
            If objDB.Exist(strsql) > 0 Then
                strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX IN "
                strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                strsql &= "WHERE POM_PO_NO = '" & Me.lblPONo.Text & "' "
                strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                objDB.Execute(strsql)
            End If

            strsql = "select * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX IN "
            strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
            strsql &= "WHERE POM_PO_NO = '" & Me.lblPONo.Text & "' "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
            If objDB.Exist(strsql) > 0 Then
                strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX IN "
                strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                strsql &= "WHERE POM_PO_NO = '" & Me.lblPONo.Text & "' "
                strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                objDB.Execute(strsql)
            End If
        End If
        'End
    End Sub
    'Private Function BindData(Optional ByVal pPage As Integer = -1) As String
    '    Dim objShopping As New ShoppingCart
    '    Dim dsItem As New DataSet, dsTemp As New DataSet
    '    Dim dvViewSample As DataView
    '    Dim aryProdCode As New ArrayList
    '    Dim strProdList As String = ""
    '    Dim strItemHead As New ArrayList()
    '    'cmdAdd.Attributes.Add("onclick", "cmdAddClick(); ")
    '    Dim i As Integer = 0

    '    If ViewState("type") = "new" Then
    '        Select Case ViewState("mode")
    '            Case "cc"
    '                strProdList = "''"
    '                aryProdCode = Session("ProdList")
    '                dsItem = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
    '                For i = 0 To aryProdCode.Count - 1
    '                    If strProdList = "" Then
    '                        strProdList = "'" & aryProdCode(i)(0) & "'"
    '                        dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
    '                        dsItem.Tables(0).Merge(dsTemp.Tables(0))
    '                    Else
    '                        strProdList &= ", '" & aryProdCode(i)(0) & "'"
    '                        dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
    '                        dsItem.Tables(0).Merge(dsTemp.Tables(0))
    '                    End If

    '                Next

    '                displayAttachFile()
    '                displayAttachFileInt()

    '                ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
    '                dvViewSample = dsItem.Tables(0).DefaultView
    '            Case "po"
    '                strProdList = "''"
    '                dsItem = objShopping.getPOItemList("PC", strProdList, "")
    '                aryProdCode = Session("ProdList")
    '                For i = 0 To aryProdCode.Count - 1
    '                    If strProdList = "" Then
    '                        strProdList = "'" & aryProdCode(i) & "'"
    '                        dsTemp = objShopping.getPOItemList("PC", strProdList, "")
    '                        dsItem.Tables(0).Merge(dsTemp.Tables(0))
    '                    Else
    '                        strProdList &= ", '" & aryProdCode(i) & "'"
    '                        dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), "")
    '                        dsItem.Tables(0).Merge(dsTemp.Tables(0))
    '                    End If
    '                Next

    '                displayAttachFile()
    '                displayAttachFileInt()

    '                ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
    '                dvViewSample = dsItem.Tables(0).DefaultView
    '            Case "rfq"
    '                strProdList = ""

    '                dsItem = objShopping.getPOItemList("RFQ", strIndexList, ViewState("rfqid"), ViewState("Vendor"), Session("RFQItemList"), ViewState("modeRFQFromPR_Index"))
    '                If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
    '                    aryProdCode = Session("ProdList")
    '                    For i = 0 To aryProdCode.Count - 1
    '                        If Not objShopping.ProductCodeAlreadyExist(ViewState("rfqid"), aryProdCode(i), "RFQ") Then
    '                            If strProdList = "" Then
    '                                strProdList = "'" & aryProdCode(i) & "'"
    '                            Else
    '                                strProdList &= ", '" & aryProdCode(i) & "'"
    '                            End If
    '                        End If
    '                    Next
    '                    If Not strProdList = "" Then
    '                        dsTemp = objShopping.getPOItemList("PC", strProdList, ViewState("poid"))
    '                        dsItem.Tables(0).Merge(dsTemp.Tables(0))
    '                    End If
    '                End If

    '                ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
    '                dvViewSample = dsItem.Tables(0).DefaultView

    '                If ViewState("modePR") = "pr" Then
    '                    txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_EXTERNAL_REMARK"))

    '                    Dim strShowColumn As String
    '                    strShowColumn = objDB.GetVal("SELECT PRM_PRINT_CUSTOM_FIELDS FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

    '                    If Common.parseNull(strShowColumn) = "1" Then
    '                        chkCustomPR.Checked = True
    '                    Else
    '                        chkCustomPR.Checked = False
    '                    End If

    '                    strShowColumn = objDB.GetVal("SELECT PRM_PRINT_REMARK FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

    '                    If Common.parseNull(strShowColumn) = "1" Then
    '                        chkRemarkPR.Checked = True
    '                    Else
    '                        chkRemarkPR.Checked = False
    '                    End If

    '                    Dim objWheelFile As New FileManagement
    '                    Dim strTermFile, strAttachIndex, strAttachRFQIndex As String
    '                    Dim pQuery(0), pQueryE(0) As String
    '                    Dim strSql, strNo, strPRNo As String

    '                    'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)

    '                    strPRNo = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_PR_NO),""'"")) AS CHAR(2000)) AS PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

    '                    strAttachIndex = objWheelFile.copyPRAttachToPOMulti(strPRNo, strNo, , "PRInt")
    '                    strAttachRFQIndex = objWheelFile.copyPRAttachToPOMulti(ViewState("rfqnum"), strNo, , "RFQ")

    '                    If Session("strPutOnce") = "" Then
    '                        If strAttachIndex <> "" Then
    '                            strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID,CDA_DOC_NO, " _
    '                            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) " _
    '                            & "SELECT CDA_COY_ID,'" & Session.SessionID & "','PO',CDA_HUB_FILENAME," _
    '                            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,'' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
    '                            & "AND (CDA_DOC_NO IN (" & strPRNo & ") OR CDA_DOC_NO = '" & ViewState("rfqnum") & "') AND (CDA_DOC_TYPE='PR' OR CDA_DOC_TYPE='RFQ') AND (CDA_ATTACH_INDEX IN (" & strAttachIndex & ")) "
    '                            Common.Insert2Ary(pQuery, strSql)
    '                            objDB.BatchExecute(pQuery)
    '                            displayAttachFile()
    '                            displayAttachFileInt()
    '                        End If

    '                        If strAttachRFQIndex <> "" Then
    '                            strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID,CDA_DOC_NO, " _
    '                            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) " _
    '                            & "SELECT CDA_COY_ID,'" & Session.SessionID & "','PO',CDA_HUB_FILENAME," _
    '                            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,'' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
    '                            & "AND (CDA_DOC_NO IN (" & strPRNo & ") OR CDA_DOC_NO = '" & ViewState("rfqnum") & "') AND (CDA_DOC_TYPE='PR' OR CDA_DOC_TYPE='RFQ') AND (CDA_ATTACH_INDEX IN (" & strAttachRFQIndex & ")) "
    '                            Common.Insert2Ary(pQueryE, strSql)
    '                            objDB.BatchExecute(pQueryE)
    '                            displayAttachFile()
    '                            displayAttachFileInt()
    '                        End If
    '                        Session("strPutOnce") = "Y"
    '                    End If
    '                End If

    '                If ViewState("modeRFQFromPR_Index") <> "" Then
    '                    dvwCustomItem = dsItem.Tables(1).DefaultView

    '                    Dim strPR_In_Remark As String
    '                    strPR_In_Remark = objDB.GetVal("SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO IN (SELECT PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")

    '                    txtInternal.Text = strPR_In_Remark
    '                End If

    '                Dim strRFQ_Curr As String
    '                'strRFQ_Curr = objDB.GetVal("SELECT (SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_DELETED = 'N' AND CODE_ABBR = RM_CURRENCY_CODE) AS RM_CURRENCY_CODE FROM RFQ_MSTR WHERE rm_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
    '                strRFQ_Curr = objDB.GetVal("SELECT RM_CURRENCY_CODE FROM RFQ_MSTR WHERE rm_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
    '                lblCurrencyCode.Text = strRFQ_Curr
    '        End Select

    '        If Session("strItemHead") IsNot Nothing Then
    '            strItemHead = Session("strItemHead")
    '            txtAttention.Text = strItemHead(0)(0)
    '            txtInternal.Text = strItemHead(0)(1)
    '            txtExternal.Text = strItemHead(0)(2)
    '            cboBillCode.SelectedIndex = strItemHead(0)(3)
    '            fillAddress()

    '            cboShipmentTerm.SelectedIndex = strItemHead(0)(4)
    '            cboShipmentMode.SelectedIndex = strItemHead(0)(5)
    '            txtShipVia.Text = strItemHead(0)(6)
    '            chkUrgent.Checked = strItemHead(0)(7)

    '            txtShippingHandling.Text = strItemHead(0)(8)

    '            cboVendor.SelectedIndex = strItemHead(0)(9)
    '            cboPayTerm.SelectedIndex = strItemHead(0)(10)
    '            cboPayMethod.SelectedIndex = strItemHead(0)(11)

    '            lblCurrencyCode.Text = strItemHead(0)(12)
    '        End If

    '    ElseIf ViewState("type") = "mod" Then
    '        Select Case ViewState("mode")
    '            Case "cc"
    '                strProdList = "''"
    '                dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"))

    '                'strProdList = "''"
    '                'aryProdCode = Session("ProdList")
    '                'dsItem = objShopping.getPRItemList("ConCat", "", "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))

    '                If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
    '                    aryProdCode = Session("ProdList")
    '                    For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
    '                        If strProdList = "" Then
    '                            strProdList = "'" & aryProdCode(i)(0) & "'"
    '                            dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
    '                            dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
    '                        Else
    '                            strProdList &= ", '" & aryProdCode(i)(0) & "'"
    '                            dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
    '                            dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
    '                        End If
    '                    Next
    '                    Session("keepAr") = keepAr
    '                    If Not strProdList = "" Then
    '                        Session("keepItem") = strProdList
    '                    End If
    '                End If

    '            Case "po", "rfq"
    '                strProdList = ""
    '                'Session("keepAr") = keepAr
    '                dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"))
    '                If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then

    '                    aryProdCode = Session("ProdList")
    '                    'aryProdCode = Session("ProdListAdd")
    '                    For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
    '                        'Strip PO
    '                        'If Not objShopping.ProductCodeAlreadyExist(ViewState("poid"), aryProdCode(i), "PO") Then
    '                        If strProdList = "" Then
    '                            strProdList = "'" & aryProdCode(i) & "'"
    '                            dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), ViewState("poid"))
    '                            dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            keepAr.Add(aryProdCode(i))
    '                        Else
    '                            strProdList &= ", '" & aryProdCode(i) & "'"
    '                            dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), ViewState("poid"))
    '                            dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            keepAr.Add(aryProdCode(i))
    '                        End If
    '                        'End If
    '                    Next
    '                    Session("keepAr") = keepAr
    '                    If Not strProdList = "" Then
    '                        '    dsTemp = objShopping.getPOItemList("PC", strProdList, ViewState("poid"))
    '                        '    dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                        Session("keepItem") = strProdList
    '                        '    Session("ForVendor") = strProdList
    '                    End If
    '                End If
    '        End Select

    '        'If Session("CurrentScreen") = "VendorSelect" Then
    '        '    If Not Session("ForVendor") = "" Then
    '        '        dsTemp = objShopping.getPOItemList("PC", Session("ForVendor"), ViewState("poid"))
    '        '        dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '        '    End If
    '        'End If

    '        If Not Page.IsPostBack Then
    '            'If keepAr.Count > 0 Then
    '            keepArPost = Session("keepAr")
    '            'End If
    '            'If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
    '            'If Not Session("keepItem"). = "" Then
    '            'If Not IsNothing(keepArPost) Then
    '            '    '    dsTemp = objShopping.getPOItemList("PC", Session("keepItem"), ViewState("poid"))
    '            '    '    dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '            '    '    Session("keepItem") = ""

    '            '    For i = 0 To keepArPost.Count - 1
    '            '        dsTemp = objShopping.getPOItemList("PC", keepArPost(i), ViewState("poid"))
    '            '        dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '            '    Next
    '            '    Session("keepItem") = ""
    '            '    Session("keepAr") = New ArrayList()
    '            'End If

    '            If Not IsNothing(keepArPost) Then
    '                If ViewState("type") = "new" Or ViewState("type") = "mod" Then
    '                    Select Case ViewState("mode")
    '                        Case "po", "rfq"
    '                            For i = 0 To keepArPost.Count - 1
    '                                dsTemp = objShopping.getPOItemList("PC", keepArPost(i), ViewState("poid"))
    '                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            Next
    '                            Session("keepItem") = ""
    '                            Session("keepAr") = New ArrayList()
    '                        Case "cc"
    '                            For i = 0 To keepArPost.Count - 1
    '                                dsTemp = objShopping.getPRItemList("ConCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3), keepArPost(i)(4))
    '                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
    '                            Next
    '                    End Select
    '                End If

    '                Session("keepItem") = ""
    '                Session("keepAr") = New ArrayList()
    '            End If


    '            If dsItem.Tables(0).Rows.Count > 0 Then
    '                ViewState("Vendor") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_COY_ID"))
    '                'lblSupplier.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
    '                ViewState("POM_S_COY_ID") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
    '                ViewState("POM_S_COY_NAME") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
    '                ViewState("POM_RFQ_INDEX") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX"))
    '                If IsNumeric(ViewState("POM_RFQ_INDEX")) Then
    '                    objGlobal.FillOneVendor(cboVendor, Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID")))
    '                Else
    '                    Select Case ViewState("mode")
    '                        Case "cc"
    '                            objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"), "cc")
    '                        Case "po", "rfq"
    '                            If ViewState("modePR") = "pr" Then
    '                                objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"), "bc")
    '                            Else
    '                                objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"))
    '                            End If

    '                    End Select
    '                End If
    '                hidSupplier.Value = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
    '                lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("POM_CREATED_DATE"))
    '                'txtRequestedName.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_BUYER_NAME"))
    '                'txtRequestedContact.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_BUYER_PHONE"))
    '                'txtFreightTerm.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_FREIGHT_TERMS"))
    '                txtShipVia.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_VIA"))
    '                txtAttention.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_ATTN"))
    '                lblCurrencyCode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
    '                ViewState("Currency") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
    '                txtInternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_INTERNAL_REMARK"))
    '                txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_EXTERNAL_REMARK"))

    '                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID")), cboVendor, True, True)
    '                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_TERM")), cboPayTerm, False, True)
    '                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM")), cboShipmentTerm, False, True)
    '                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_METHOD")), cboPayMethod, False, True)
    '                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE")), cboShipmentMode, False, True)
    '                txtShippingHandling.Text = Format(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_AMT")), "###,###,##0.00")

    '                If Session("strItemHead") IsNot Nothing Then
    '                    strItemHead = Session("strItemHead")
    '                    txtAttention.Text = strItemHead(0)(0)
    '                    txtInternal.Text = strItemHead(0)(1)
    '                    txtExternal.Text = strItemHead(0)(2)
    '                    cboBillCode.SelectedIndex = strItemHead(0)(3)
    '                    fillAddress()

    '                    cboShipmentTerm.SelectedIndex = strItemHead(0)(4)
    '                    cboShipmentMode.SelectedIndex = strItemHead(0)(5)
    '                    txtShipVia.Text = strItemHead(0)(6)
    '                    chkUrgent.Checked = strItemHead(0)(7)

    '                    txtShippingHandling.Text = strItemHead(0)(8)

    '                    cboVendor.SelectedIndex = strItemHead(0)(9)
    '                    cboPayTerm.SelectedIndex = strItemHead(0)(10)
    '                    cboPayMethod.SelectedIndex = strItemHead(0)(11)

    '                    lblCurrencyCode.Text = strItemHead(0)(12)
    '                End If


    '                enableBill(False)
    '                Session("keepItem") = ""
    '                Session("keepAr") = New ArrayList()
    '                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_CUSTOM_FIELDS")) = "1" Then
    '                    chkCustomPR.Checked = True
    '                Else
    '                    chkCustomPR.Checked = False
    '                End If
    '                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_REMARK")) = "1" Then
    '                    chkRemarkPR.Checked = True
    '                Else
    '                    chkRemarkPR.Checked = False
    '                End If

    '                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_URGENT")) = "1" Then
    '                    chkUrgent.Checked = True
    '                Else
    '                    chkUrgent.Checked = False
    '                End If
    '                'ViewState("strGST") = dsItem.Tables(0).Rows(0)("POM_GST")
    '                'If ViewState("strGST") = "0" Then
    '                '    ViewState("GST") = "product"
    '                'Else
    '                '    ViewState("GST") = "subtotal"
    '                'End If

    '                ' check company allow free form
    '                If ViewState("blnAllowFreeForm") = False And Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE")) = "F" Then
    '                    Dim objAdmin As New Admin
    '                    Common.SelDdl(objAdmin.user_Default_Add("B"), cboBillCode, True, True)
    '                    fillAddress()
    '                Else
    '                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE")), cboBillCode, True, True)
    '                    txtBillAdd1.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE1"))
    '                    txtBillAdd2.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE2"))
    '                    txtBillAdd3.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE3"))
    '                    txtBillPostCode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_POSTCODE"))
    '                    txtBillCity.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_CITY"))
    '                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_STATE")), cboState, True, True)
    '                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_COUNTRY")), cboCountry, True, True)
    '                    enableBill(True)
    '                End If

    '                ' Yap_As Michelle agreed to display the code only
    '                If ViewState("modePR") = "pr" Then
    '                    cboBillCode.SelectedItem.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE"))
    '                End If

    '                If IsDBNull(dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX")) Then ' created from SHOPPING_CART
    '                    ViewState("listtype") = "cart"
    '                Else ' created from RFQ
    '                    If dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX") = 0 Then ' created from SHOPPING_CART
    '                        ViewState("listtype") = "cart"
    '                    Else
    '                        ViewState("listtype") = "rfq"
    '                    End If
    '                End If

    '                If ViewState("listtype") = "cart" Then
    '                    cmdAdd.Visible = True
    '                    cmdRemove.Visible = True
    '                    cmdDupPOLine.Visible = True
    '                    cmdUploadPO.Visible = True
    '                    'cmdAdd.Attributes.Add("onclick", "return cmdAddClick();")
    '                    ViewState("blnCmdAdd") = True
    '                    ViewState("blnCmdRemove") = True
    '                Else
    '                    ''cmdAdd.Visible = False
    '                    ''cmdRemove.Visible = False
    '                    'cmdDupPOLine.Visible = False
    '                    'ViewState("blnCmdAdd") = False
    '                    'ViewState("blnCmdRemove") = False
    '                    'cmdAdd.Attributes.Add("onclick", "return cmdAddClick();")
    '                    'dtgShopping.Columns(EnumShoppingCart.icRfqQty).Visible = True
    '                    'dtgShopping.Columns(EnumShoppingCart.icTolerance).Visible = True
    '                    'dtgShopping.Columns(EnumShoppingCart.icRfqQty).HeaderText = "RFQ Qty"
    '                    'dtgShopping.Columns(EnumShoppingCart.icQty).HeaderText = "PO Qty"
    '                End If
    '            End If
    '        End If
    '        ViewState("intPageRecordCnt") = dsItem.Tables(1).Rows.Count
    '        dvViewSample = dsItem.Tables(1).DefaultView
    '        dvwCustomItem = dsItem.Tables(2).DefaultView

    '        Select Case ViewState("mode")
    '            Case "po"
    '            Case "rfq"
    '        End Select
    '    End If

    '    intPageRecordCnt = ViewState("intPageRecordCnt")

    '    intRow = 0

    '    dtgShopping.DataSource = dvViewSample
    '    dtgShopping.DataBind()
    '    objShopping = Nothing
    '    'If Session("Env") = "FTN" Then
    '    '    Me.dtgShopping.Columns(6).Visible = False
    '    '    Me.dtgShopping.Columns(7).Visible = False
    '    '    Me.dtgShopping.Columns(5).Visible = False
    '    '    Me.dtgShopping.Columns(22).Visible = False
    '    'Else
    '    '    Me.dtgShopping.Columns(6).Visible = True
    '    '    Me.dtgShopping.Columns(7).Visible = False
    '    '    Me.dtgShopping.Columns(5).Visible = True
    '    '    Me.dtgShopping.Columns(22).Visible = True
    '    'End If
    '    Me.dtgShopping.Columns(6).Visible = True
    '    Me.dtgShopping.Columns(7).Visible = False
    '    Me.dtgShopping.Columns(5).Visible = True
    '    Me.dtgShopping.Columns(23).Visible = True
    'End Function
End Class