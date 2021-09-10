Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class CreateQuotationNewFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strQtyErr As String
    Dim strGSTRegNo As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents body1 As HtmlGenericControl
    Protected WithEvents lbl_exp_on As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_validity As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_con_person As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_email As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_PayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_PayMeth As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_ShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_remark As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_cur As System.Web.UI.WebControls.Label
    Protected WithEvents txt_valid As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_con_person As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_con_num As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_email As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_con_num As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_RFQ_No As System.Web.UI.WebControls.Label
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rv_date As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents cmd_upload As System.Web.UI.WebControls.Button
    'Protected WithEvents lbl_title As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Previous As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents pnlAttach2 As System.Web.UI.WebControls.Panel
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lbl_QPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_QPayMeth As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_QShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_QShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidPostBack As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdreset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim objFile As New FileManagement
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents cboShipmentMode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboShipmentTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblTax As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Protected WithEvents cboGSTRate As System.Web.UI.WebControls.DropDownList


    Dim strFrm As String
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Session("edit") = "1" Then
            Dim alButtonList As ArrayList
            alButtonList = New ArrayList
            htPageAccess.Add("update", alButtonList)
            CheckButtonAccess()
            cmdreset.Disabled = Not (blnCanUpdate)
            alButtonList.Clear()
        Else
            Dim alButtonList As ArrayList
            alButtonList = New ArrayList
            htPageAccess.Add("add", alButtonList)
            CheckButtonAccess()
            cmdreset.Disabled = Not (blnCanAdd)
            alButtonList.Clear()
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objrfq As New RFQ
        MyBase.Page_Load(sender, e)
        blnPaging = False

        SetGridProperty(dg_viewitem)
        If ViewState("check") <> "1" Then
            ViewState("check") = "0"
        End If
        strFrm = Me.Request.QueryString("Frm")
        Me.rv_date.Type = ValidationDataType.Date
        Me.rv_date.MaximumValue = Today.AddYears(1000)
        Me.rv_date.MinimumValue = Now.Today
        If strFrm = "Dashboard" Then
            cmd_Previous.HRef = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "VendorRFQList" Then
            cmd_Previous.HRef = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "RFQSearch" Then
            cmd_Previous.HRef = dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId)
            cmd_supply.Visible = False
        End If

        Dim objGST As New GST
        ViewState("isGST") = objGST.chkGSTCOD()
        hidGST.Value = ViewState("isGST")
        strGSTRegNo = objGST.chkGST(Session("CompanyId"))     

        If Not Page.IsPostBack Or hidPostBack.Value = "1" Then
            GenerateTab()
            'Delete those temp attachment
            objrfq.delRFQTempAttach2(Request(Trim("RFQ_No")))

            cmd_upload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            '//code already called outside this function
            '//displayAttachFile2()
            '//displayAttachFile()
            Session("edit") = Request(Trim("edit"))

            If Session("edit") = "" Then
                Session("edit") = "0"
            End If

            'Dim RFQ_NO As String = Request(Trim("RFQ_No"))
            'CHANGE BY MOO
            ViewState("rfq_no") = Request(Trim("RFQ_No"))
            ViewState("rfq_id") = Request(Trim("RFQ_ID"))

            Dim RFQ_NO As String = Request(Trim("RFQ_NO"))
            Dim RFQ_ID As String = Request(Trim("RFQ_ID"))
            Dim objval As New RFQ_User

            ' ai chu add on 26/10/2005
            ' if this page is redirect from summary screen (VendorRFQList.aspx)
            ' need to delete all temp attach file which may been saved before 
            ' but not yet submitted to the buyer

            'If Not Session("BackToStep1") Then ' called from Summary screen
            '    objrfq.reCopyRFQTempAttach(RFQ_NO)
            'End If

            objrfq.get_rfqMstr(objval, RFQ_ID)
            Me.lbl_RFQ_No.Text = RFQ_NO
            Me.lbl_exp_on.Text = objval.exp_date
            Me.lbl_cur.Text = objval.cur_code
            ViewState("RM_Coy_ID") = objval.RM_Coy_ID
            Me.lbl_validity.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.RFQ_Req_date)
            Me.lbl_con_person.Text = objval.con_person
            Me.lbl_con_num.Text = objval.phone
            Me.lbl_email.Text = objval.email
            Me.lbl_remark.Text = objval.remark

            If Session("edit") = "0" Then
                'Me.lbl_title.Text = "Create Quotation (Step 1)"

                ' ai chu modified on 19/10/2005
                ' if page is called from CreateQuotation2, it should pass all parameter for all textbox
                Me.txt_valid.Text = IIf(Session("validity") = "", Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.RFQ_Req_date), Session("validity"))
                Me.txt_con_person.Text = IIf(Session("con_person") = "", objval.vendor_name, Session("con_person"))
                Me.txt_con_num.Text = IIf(Session("con_num") = "", objval.vendor_Con_num, Session("con_num"))
                Me.txt_email.Text = IIf(Session("email") = "", objval.vendor_email, Session("email"))
                Me.txt_remark.Text = IIf(Session("remark") = "", objval.remark, Session("remark"))
            ElseIf Session("edit") = "1" Then
                'Me.lbl_title.Text = "Resubmit Quotation (Step 1)"
                objrfq.get_qoute1(objval, RFQ_ID, Session("CompanyId"))

                ' ai chu modified on 19/10/2005
                ' if page is called from CreateQuotation2, it should pass all parameter for all textbox
                Me.txt_valid.Text = IIf(Session("validity") = "", Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.validaty), Session("validity"))
                Me.txt_con_person.Text = IIf(Session("con_person") = "", objval.con_person, Session("con_person"))
                Me.txt_con_num.Text = IIf(Session("con_num") = "", objval.phone, Session("con_num"))
                Me.txt_email.Text = IIf(Session("email") = "", objval.email, Session("email"))
                Me.txt_remark.Text = IIf(Session("remark") = "", objval.remark, Session("remark"))

            End If

            Dim objGlobal As New AppGlobals
            objGlobal.FillCodeTable(cboShipmentTerm, CodeTable.ShipmentTerm)
            objGlobal.FillCodeTable(cboShipmentMode, CodeTable.ShipmentMode)

            ViewState("pay_term") = objval.pay_term
            ViewState("pay_type") = objval.pay_type
            ViewState("ship_term") = objval.ship_term
            ViewState("ship_mode") = objval.ship_mode
            Dim pay_term As String = objrfq.get_codemstr(objval.pay_term, "PT")
            Dim pay_type As String = objrfq.get_codemstr(objval.pay_type, "PM")
            Dim ship_mode As String = objrfq.get_codemstr(objval.ship_mode, "SM")
            Dim ship_term As String = objrfq.get_codemstr(objval.ship_term, "ST")

            Dim pt As String = objval.pay_term.ToString.Trim

            If pay_term = "" Then
                lbl_PayTerm.Text = "Not Applicable"
                lbl_QPayTerm.Text = lbl_PayTerm.Text
            Else
                Me.lbl_PayTerm.Text = pay_term
                lbl_QPayTerm.Text = lbl_PayTerm.Text
            End If

            If pay_type = "" Then
                Me.lbl_PayMeth.Text = "Not Applicable"
                Me.lbl_QPayMeth.Text = lbl_PayMeth.Text
            Else
                Me.lbl_PayMeth.Text = pay_type
                Me.lbl_QPayMeth.Text = lbl_PayMeth.Text
            End If

            If ship_mode = "" Then
                Me.lbl_ShipMode.Text = "Not Applicable"
                Me.lbl_QShipMode.Text = lbl_ShipMode.Text
                'IIf(Session("remark") = "", objval.remark, Session("remark"))
                Common.SelDdl(IIf(Session("shipmode") = "", "99", Session("shipmode")), cboShipmentMode, True, True)
            Else
                Me.lbl_ShipMode.Text = ship_mode
                Me.lbl_QShipMode.Text = lbl_ShipMode.Text
                Common.SelDdl(IIf(Session("shipmode") = "", objval.ship_mode, Session("shipmode")), cboShipmentMode, True, True)
                'Common.SelDdl(ship_mode, cboShipmentMode, False, True)
            End If

            If ship_term = "" Then
                lbl_ShipTerm.Text = "Not Applicable"
                Me.lbl_QShipTerm.Text = lbl_ShipTerm.Text
                'Common.SelDdl("99", cboShipmentTerm, True, True)
                Common.SelDdl(IIf(Session("shipterm") = "", "99", Session("shipterm")), cboShipmentTerm, True, True)
            Else
                Me.lbl_ShipTerm.Text = ship_term
                Me.lbl_QShipTerm.Text = lbl_ShipTerm.Text
                'Common.SelDdl(ship_term, cboShipmentTerm, False, True)
                Common.SelDdl(IIf(Session("shipterm") = "", objval.ship_term, Session("shipterm")), cboShipmentTerm, True, True)
            End If

            'Me.onchange.Value = "0"
            'ViewState("rfq_no") = Request(Trim("RFQ_Num"))
            'Michelle (19/2/2013) - Issue 1846
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr

            Bindgrid()
            'Session("quote2") = strCallFrom
            hidPostBack.Value = "0"

            objrfq = Nothing
            objval = Nothing
            objGlobal = Nothing
        End If

        txt_remark.Attributes.Add("onKeyDown", "limitText (this, 1000);")

        displayAttachFile()
        displayAttachFile2()

        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); calculateAllIndividualTotal(); ")
        'ViewState("body_loaditemcreated") = ""

        'Check Access Status
        'If Session("AccessStatus") = "Limited" Then
        '    cmd_update.Enabled = False
        'End If

        Me.cmdPreview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRFQ.aspx", "VendorRequired=T&BCoyID=" & ViewState("RM_Coy_ID") & "&SCoyID=" & Session("CompanyId") & "&RFQ_Num=" & ViewState("rfq_no")) & "')")
    End Sub

    Public Sub dg_viewitem_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dg_viewitem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objview As New RFQ
        Dim ds As New DataSet
        Dim RFQ_ID As String

        RFQ_ID = Request(Trim("RFQ_ID"))
        ds = objview.get_quotation(RFQ_ID)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dg_viewitem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_viewitem.PageSize = 0 Then
                dg_viewitem.CurrentPageIndex = dg_viewitem.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dg_viewitem.DataSource = dvViewSample
        dg_viewitem.DataBind()

        If ViewState("isGST") Then
            dg_viewitem.Columns(7).Visible = True
            Me.lblTax.Text = "GST Amount"
        End If

        'YapCL: 29Mar2011 Allow show EDD - Issue 179
        'If Session("Env") = "FTN" Then
        '    Me.dg_viewitem.Columns(8).Visible = False
        'Else
        '    Me.dg_viewitem.Columns(8).Visible = True
        'End If
    End Function

    Private Sub cmd_upload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_upload.Click
        Dim objFile As New FileManagement
        If File1.Value = "" Then
        Else

            ' Restrict user upload size
            Dim objDB As New EAD.DBCom
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.DocAttachment, "QuotTemp", EnumUploadFrom.FrontOff, Request(Trim("RFQ_No")), , , System.Configuration.ConfigurationManager.AppSettings("Path"))
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

        End If
        displayAttachFile()
        objFile = Nothing
    End Sub

    Private Sub deleteattach_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objrfq As New RFQ

        objrfq.deleteRFQAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()
        objrfq = Nothing
    End Sub
    '//Get Attachment for Quotation(Reply)
    Private Sub displayAttachFile()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        dsAttach = objRFQ.getRFQTempAttach2(Request(Trim("RFQ_No")))



        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")

                '*************************meilai 25/2/05****************************
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=Quotation>" & strFile & "</A>"
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "Quotation", EnumUploadFrom.FrontOff)
                '*************************meilai************************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteattach_Click

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objRFQ = Nothing
    End Sub
    '//Get Attachment for RFQ(From Buyer)
    Private Sub displayAttachFile2()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objRFQ.getRFQTempAttach(ViewState("rfq_no"), objRFQ.get_comid(ViewState("rfq_id")))

        pnlAttach2.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '*************************meilai 25/2/05****************************
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=RFQ>" & strFile & "</A>"
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff)
                '*************************meilai************************************
                Dim lblBr As New Label
                Dim lblFile As New Label

                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                pnlAttach2.Controls.Add(lblFile)
                pnlAttach2.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach2.Controls.Add(lblFile)
        End If
        objRFQ = Nothing
    End Sub

    'Private Sub cmd_Previous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Previous.ServerClick
    '    If strFrm = "Dashboard" Then
    '        cmd_Previous.HRef = "../Dashboard/Vendor.aspx?pageid=" & strPageId

    '    ElseIf strFrm = "VendorRFQList" Then
    '        cmd_Previous.HRef = "VendorRFQList.aspx?&pageid=" & strPageId

    '    ElseIf strFrm = "RFQSearch" Then
    '        cmd_Previous.HRef = "RFQSearch.aspx?&pageid=" & strPageId

    '    End If
    'End Sub

    'Private Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_next.Click
    '    If Not Common.checkMaxLength(txt_remark.Text, 1000) Then
    '        lblMsg.Text = "<ul type='disc'><li>Remarks is over limit.<ul type='disc'></ul></li></ul>"
    '        Exit Sub
    '    Else
    '        Session("remark") = Me.txt_remark.Text
    '        Session("strurl") = strCallFrom

    '        ' ai chu add on 19/10/2005
    '        ' need to use session to pass value in all text boxes
    '        Session("con_person") = txt_con_person.Text
    '        Session("con_num") = txt_con_num.Text
    '        Session("email") = txt_email.Text
    '        Session("validity") = txt_valid.Text
    '        Session("shipterm") = cboShipmentTerm.SelectedValue
    '        Session("shipmode") = cboShipmentMode.SelectedValue
    '        viewstate("ship_mode") = Session("shipmode")
    '        viewstate("ship_term") = Session("shipterm")

    '        'Dim objdb As New EAD.DBCom
    '        '' if item exist in RFQ_REPLIES_DETAIL_TEMP then session("edit") = 1
    '        'If objdb.Exist("select '*' from RFQ_REPLIES_DETAIL_TEMP where RRDT_RFQ_ID='" & Common.Parse(viewstate("rfq_id")) & "' and RRDT_V_Company_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'") > 0 Then
    '        '    Session("edit") = 1
    '        'End If
    '        'objdb = Nothing
    '        Response.Redirect("CreateQuotation2.aspx?RFQ_ID=" & viewstate("rfq_id") & "&RFQ_Num=" & lbl_RFQ_No.Text & "&validity=" & txt_valid.Text & " " & _
    '        "&con_person=" & Server.UrlEncode(txt_con_person.Text) & "&con_num=" & Server.UrlEncode(txt_con_num.Text) & "&email=" & txt_email.Text & " " & _
    '        "&payterm=" & viewstate("pay_term") & "&paymeth=" & viewstate("pay_type") & "" & _
    '        "&shipmode=" & viewstate("ship_mode") & "&shipterm=" & viewstate("ship_term") & "&cur=" & Me.lbl_cur.Text & " &pageid=" & strPageId & " ")

    '    End If
    'End Sub

    Private Sub cmdreset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdreset.ServerClick
        If Not Common.checkMaxLength(txt_remark.Text, 1000) Then
            txt_remark.Text = ""
        End If
        lblMsg.Text = ""

    End Sub

    Private Sub dg_viewitem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dg_viewitem, e)
        'ViewState("body_loaditemcreated") = "calculateAllIndividualTotal(); "
        Dim iLoop As Integer
        If blnPaging Then
            If (e.Item.ItemType = ListItemType.Pager) Then
                Dim pager As TableCell = e.Item.Controls(0)
                For iLoop = 0 To pager.Controls.Count Step 2
                    Dim obj As System.Web.UI.Control = pager.Controls(iLoop)
                    If TypeOf (obj) Is LinkButton Then
                        Dim h As LinkButton = obj
                        h.Attributes.Add("onclick", "return PromptSave();")
                    End If
                Next
            End If
        End If

        If e.Item.ItemType = ListItemType.Header Then
            If ViewState("isGST") Then
                e.Item.Cells(8).Text = "GST Amount"
                e.Item.Cells(7).CssClass = "viscol"
            Else
                e.Item.Cells(8).Text = "Tax"
                e.Item.Cells(7).CssClass = "hiddencol"
            End If
        End If

        If (e.Item.ItemType = ListItemType.Item) Or
        (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim ddl_tax As New DropDownList
            ddl_tax = e.Item.FindControl("ddl_tax")
            'ddl_tax.Attributes.Add("onchange", "check();")
        End If

        If (e.Item.ItemType = ListItemType.Item) Or
        (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cboGSTRate As New DropDownList
            cboGSTRate = e.Item.FindControl("cboGSTRate")
            If ViewState("isGST") Then
                e.Item.Cells(7).CssClass = "viscol"
            Else
                e.Item.Cells(7).CssClass = "hiddencol"
            End If
        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        Dim dgItem As DataGridItem
        Dim txtRemark As TextBox
        Dim txtQ As TextBox

        strMsg = "<ul type='disc'>"
        For Each dgItem In dg_viewitem.Items
            txtRemark = dgItem.FindControl("txt_remark")
            txtQ = dgItem.FindControl("txtQ")

            If Not Common.checkMaxLength(txtRemark.Text, 250) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If
        Next
        strMsg &= "</ul>"
    End Function

    Private Function validateUnitPrice() As Boolean
        validateUnitPrice = True
        Dim dgItem As DataGridItem
        Dim i As Integer

        For Each dgItem In dg_viewitem.Items
            If CType(dgItem.FindControl("txt_price"), TextBox).Text = "" Then
                i = i + 1
            End If
        Next

        If dg_viewitem.Items.Count = i Then
            validateUnitPrice = False
            Common.NetMsgbox(Me, "Please quote at least one item.", MsgBoxStyle.Information)

            Exit Function
        End If

    End Function

    Private Sub dg_viewitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemDataBound
        Dim objrfq As New RFQ
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            'Dim txt_venItemCode As TextBox
            'txt_venItemCode = e.Item.FindControl("txt_venItemCode")
            Dim txt_MPQ As TextBox
            txt_MPQ = e.Item.FindControl("txt_MPQ")

            Dim txt_MOQ As TextBox
            txt_MOQ = e.Item.FindControl("txt_MOQ")

            Dim txt_price As TextBox
            txt_price = e.Item.FindControl("txt_price")

            Dim txt_temp As TextBox
            txt_temp = e.Item.FindControl("txt_temp")

            Dim txt_delivery As TextBox
            txt_delivery = e.Item.FindControl("txt_delivery")

            Dim txt_warranty As TextBox
            txt_warranty = e.Item.FindControl("txt_warranty")
            'Dim txt_Tolerance As TextBox
            'txt_Tolerance = e.Item.FindControl("txt_Tolerance")
            Dim txt_remark As TextBox
            txt_remark = e.Item.FindControl("txt_remark")

            Dim ddl_tax As New DropDownList
            Dim dv_uom As DataView
            Dim objGlobal As New AppGlobals
            ddl_tax = e.Item.FindControl("ddl_tax")
            objrfq.Fill_Tax(ddl_tax)

            Dim intMax As Integer
            '//moo
            'If Session("edit") = "0" then
            '    txt_venItemCode.Text = Common.parseNull(dv("RD_Vendor_Item_Code"))
            '    txt_MOQ.Text = "1"
            '    txt_MPQ.Text = "1"
            '    txt_price.Text = "0"
            '    txt_Tolerance.Text = "0"
            '    txt_delivery.Text = Common.parseNull(dv("RD_Delivery_Lead_Time"))
            '    txt_warranty.Text = Common.parseNull(dv("RD_Warranty_Terms"))
            '    'Common.SelDdl("STD", ddl_tax, True, True)
            '    e.Item.Cells(1).Text = Common.parseNull(dv("RD_Product_Desc"))
            '    e.Item.Cells(2).Text = Common.parseNull(dv("RD_UOM"))
            '    e.Item.Cells(3).Text = Common.parseNull(dv("RD_Quantity"))
            '    e.Item.Cells(11).Text = Common.parseNull(dv("RD_RFQ_Line"))
            '    e.Item.Cells(12).Text = Common.parseNull(dv("RD_Product_Code"))
            '    intMax = CInt(Common.parseNull(dv("RD_Quantity")))
            'ElseIf Session("edit") = "1" Then

            'txt_venItemCode.Text = Common.parseNull(dv("RRDT_Product_Name"))
            txt_MOQ.Text = Common.parseNull(dv("RRDT_Min_Order_Qty"))
            txt_MPQ.Text = Common.parseNull(dv("RRDT_Min_Pack_Qty"))

            If IsDBNull(dv("RRDT_Unit_Price")) Then
                txt_price.Text = ""
            Else
                txt_price.Text = Common.parseNull(dv("RRDT_Unit_Price"))
            End If
            'txt_Tolerance.Text = Common.parseNull(dv("RRDT_Tolerance"))
            txt_delivery.Text = Common.parseNull(dv("RRDT_Delivery_Lead_Time"))
            txt_warranty.Text = Common.parseNull(dv("RRDT_Warranty_Terms"))
            Common.SelDdl(Common.parseNull(dv("RRDT_GST")), ddl_tax, False, True)
            e.Item.Cells(0).Text = Common.parseNull(dv("RRDT_Product_Desc"))
            e.Item.Cells(1).Text = Common.parseNull(dv("RRDT_UOM"))
            'e.Item.Cells(3).Text = Common.parseNull(dv("RRDT_Quantity"))
            txt_temp.Text = Common.parseNull(dv("RRDT_Quantity"))
            e.Item.Cells(12).Text = Common.parseNull(dv("RRDT_Line_No"))
            e.Item.Cells(13).Text = Common.parseNull(dv("RRDT_Product_ID"))
            intMax = CInt(Common.parseNull(dv("RRDT_Quantity")))
            txt_remark.Text = Common.parseNull(dv("RRDT_Remarks"))

            'End If

            'Dim rv_tolerance As RangeValidator
            'rv_tolerance = e.Item.FindControl("rv_tolerance")
            'Dim intMin As Integer
            'intMin = 0
            'rv_tolerance.Type = ValidationDataType.Integer
            'rv_tolerance.MaximumValue = intMax
            'rv_tolerance.MinimumValue = intMin
            'rv_tolerance.ControlToValidate = "txt_Tolerance"
            'rv_tolerance.ErrorMessage = "Qty Tolerance must less than Qty."
            'rv_tolerance.Text = "?"
            'rv_tolerance.Display = ValidatorDisplay.Dynamic

            'Dim rev_tolerance As RegularExpressionValidator
            'rev_tolerance = e.Item.FindControl("rev_tolerance")
            'rev_tolerance.ControlToValidate = "txt_Tolerance"
            'rev_tolerance.ValidationExpression = "^\d+$"
            'rev_tolerance.ErrorMessage = "Qty Tolerance is expecting numeric value."
            'rev_tolerance.Text = "?"
            'rev_tolerance.Display = ValidatorDisplay.Dynamic

            Dim rev_mpq As RegularExpressionValidator
            rev_mpq = e.Item.FindControl("rev_mpq")
            rev_mpq.ControlToValidate = "txt_MPQ"
            rev_mpq.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^\d+$"
            rev_mpq.ErrorMessage = "MPQ - " & ViewState("ValQtyMsg")
            rev_mpq.Text = "?"
            rev_mpq.Display = ValidatorDisplay.Dynamic

            Dim rev_moq As RegularExpressionValidator
            rev_moq = e.Item.FindControl("rev_moq")
            rev_moq.ControlToValidate = "txt_MOQ"
            rev_moq.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^\d+$"
            rev_moq.ErrorMessage = "MOQ - " & ViewState("ValQtyMsg") 'is expecting numeric value."
            rev_moq.Text = "?"
            rev_moq.Display = ValidatorDisplay.Dynamic

            'Dim rev_price As RegularExpressionValidator
            'rev_price = e.Item.FindControl("rev_price")
            'rev_price.ControlToValidate = "txt_price"
            'rev_price.ValidationExpression = "(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"
            'rev_price.ErrorMessage = "Price is overlimit/expecting numeric value."
            'rev_price.Text = "?"
            'rev_price.Display = ValidatorDisplay.Dynamic

            Dim rev_delivery As RegularExpressionValidator
            rev_delivery = e.Item.FindControl("rev_delivery")
            rev_delivery.ControlToValidate = "txt_delivery"
            rev_delivery.ValidationExpression = "^\d+$"
            rev_delivery.ErrorMessage = "Delivery Lead Time(days) is expecting numeric value."
            rev_delivery.Text = "?"
            rev_delivery.Display = ValidatorDisplay.Dynamic

            Dim rev_warranty As RegularExpressionValidator
            rev_warranty = e.Item.FindControl("rev_warranty")
            rev_warranty.ControlToValidate = "txt_warranty"
            rev_warranty.ValidationExpression = "^\d+$"
            rev_warranty.ErrorMessage = "Warranty Terms (mths) is expecting numeric value."
            rev_warranty.Text = "?"
            rev_warranty.Display = ValidatorDisplay.Dynamic

            Dim rev_price As RegularExpressionValidator
            rev_price = e.Item.FindControl("rev_price")
            rev_price.ControlToValidate = "txt_price"
            rev_price.ValidationExpression = "^\d{1,10}(\.\d{1,4})?$"
            rev_price.ErrorMessage = "Unit price is expecting numeric value."
            rev_price.Text = "?"
            rev_price.Display = ValidatorDisplay.Dynamic

            Dim txt_Amount As TextBox
            txt_Amount = e.Item.FindControl("txt_Amount")

            ''Jules 2014.07.14 GST Enhancement --<
            Dim cboGSTRate As DropDownList
            cboGSTRate = e.Item.FindControl("cboGSTRate")

            Dim objGST As New GST

            If ViewState("isGST") Then
                If Common.parseNull(dv("RRDT_GST_Rate")) = "" Then
                    If strGSTRegNo = "" Then
                        Dim lstItem As New ListItem
                        lstItem.Value = ""
                        lstItem.Text = "N/A"
                        cboGSTRate.Items.Insert(0, lstItem)
                        cboGSTRate.Enabled = False
                    Else
                        objGlobal.FillGST(cboGSTRate, True, ViewState("RM_Coy_ID"))
                        Dim strDefaultRate As String
                        strDefaultRate = objGST.getDefaultVendorItemGSTRate(Common.parseNull(dv("RRDT_Product_Name")), Common.parseNull(dv("RRDT_V_Company_ID")))
                        If strDefaultRate = "" Or strDefaultRate = "N/A" Or IsNumeric(strDefaultRate) Then
                            cboGSTRate.SelectedValue = "STD"
                        Else
                            Common.SelDdl(strDefaultRate, cboGSTRate)
                        End If
                    End If
                Else
                    objGlobal.FillGST(cboGSTRate, True, ViewState("RM_Coy_ID"))
                    Common.SelDdl(dv("RRDT_GST_Rate"), cboGSTRate)
                End If
            Else
                objrfq.Fill_Tax(cboGSTRate)
            End If

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txt_remark")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            If hidSummary.Value = "" Then
                hidSummary.Value = "Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Remarks-" & txtRemark.ClientID
            End If

            Dim sClientId As String, sTotalClient As String

            sTotalClient = hidClientId.Value

            Dim CheckInstr, TaxPerc As String
            CheckInstr = Mid(txt_Amount.ClientID, InStr(txt_Amount.ClientID, "_") + 1, Len(txt_Amount.ClientID))
            'TaxPerc = ddl_tax.SelectedItem.Text
            'If TaxPerc = "---Select---" Then TaxPerc = 0

            sClientId = Mid(CheckInstr,
            InStr(CheckInstr, "_") + 1,
            InStr(Mid(CheckInstr,
            InStr(CheckInstr, "_") + 1), "_") - 1) & "|"


            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            Dim hidTaxPerc As TextBox
            Dim txtGSTAmt As TextBox
            hidTaxPerc = e.Item.FindControl("hidTaxPerc")
            Dim strGSTPerc, strGSTID As String
            objGST.getGSTInfobyRate(cboGSTRate.SelectedValue, strGSTPerc, strGSTID)
            If strGSTPerc = "" Then strGSTPerc = "0"
            hidTaxPerc.Text = strGSTPerc

            txtGSTAmt = e.Item.FindControl("txtGSTAmt")

            txt_price.Attributes.Add("onfocus", "return focusControl('" &
                txt_temp.ClientID & "', '" & txt_price.ClientID & "', '" & txt_Amount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "');")

            txt_price.Attributes.Add("onblur", "return calculateTotal('" &
                            txt_temp.ClientID & "', '" & txt_price.ClientID & "', '" & txt_Amount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "');")

            'ddl_tax.Attributes.Add("onchange", "return calculateTotal('" & _
            '                txt_temp.ClientID & "', '" & txt_price.ClientID & "', '" & txt_Amount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "');")


            cboGSTRate.Attributes.Add("onchange", "return calculateTotal('" &
                            txt_temp.ClientID & "', '" & txt_price.ClientID & "', '" & txt_Amount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "');")


        End If
    End Sub
    Sub save()
        Dim dgItem As DataGridItem
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        Dim rfq_id = Request(Trim("RFQ_ID"))
        Dim gst As Integer
        Dim i As Integer
        Dim j As Decimal
        Dim cnt As Integer
        Dim a As Decimal
        Dim gstamt As Integer

        For Each dgItem In dg_viewitem.Items
            objval.RFQ_ID = rfq_id
            'objval.v_itemCode = CType(dgItem.FindControl("txt_VenItemCode"), TextBox).Text
            objval.v_itemCode = objDB.Get1Column("PRODUCT_MSTR", "PM_VENDOR_ITEM_CODE", " WHERE PM_PRODUCT_CODE = '" & dgItem.Cells(12).Text & "' ")
            objval.lineno = dgItem.Cells(12).Text
            objval.product_ID = dgItem.Cells(13).Text
            objval.item_desc = dgItem.Cells(1).Text
            objval.uom = dgItem.Cells(2).Text
            objval.Quantity = CType(dgItem.FindControl("txt_temp"), TextBox).Text 'dgItem.Cells(3).Text
            'objval.Tolerance = CType(dgItem.FindControl("txt_Tolerance"), TextBox).Text()
            objval.MOQ = CType(dgItem.FindControl("txt_MOQ"), TextBox).Text
            objval.MPQ = CType(dgItem.FindControl("txt_MPQ"), TextBox).Text
            objval.Delivery_Lead_Time = CType(dgItem.FindControl("txt_delivery"), TextBox).Text
            objval.Warranty_Terms = CType(dgItem.FindControl("txt_warranty"), TextBox).Text
            'objval.Tax = CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Value        
            'objval.gst_desc = CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text
            'objval.gst = IIf(CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text, 0) 'objrfq.get_gst(objval.Tax)
            Dim objGST As New GST
            Dim strGSTPerc, strGSTID As String
            objval.Tax = IIf(CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedIndex > 0, CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Value, Nothing) 'CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Value
            objval.gst_desc = CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Value
            'objval.gst = IIf(CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Text, 0) 'objrfq.get_gst(objval.Tax)
            objGST.getGSTInfobyRate(objval.Tax, strGSTPerc, strGSTID)
            strGSTPerc = IIf(strGSTPerc = "", "0", strGSTPerc)
            objval.gst = strGSTPerc
            objval.PRICE = CType(dgItem.FindControl("txt_price"), TextBox).Text

            objval.remark = CType(dgItem.FindControl("txt_remark"), TextBox).Text
            strSQL = objrfq.add_2repliedTemp(objval)
            Common.Insert2Ary(strAryQuery, strSQL)

            'If objval.gst <> "N/A" And objval.gst <> "---Select---" Then
            '    gstamt = objval.gst
            '    gst = objval.gst
            'Else
            '    gstamt = 0
            '    gst = 0
            'End If
            
            If strGSTPerc = "" Then
                gstamt = 0
            Else
                gstamt = strGSTPerc
            End If            

            If Not objval.PRICE = "" Then
                If gstamt <> 0 Then
                    '2015-06-18: CH: Rounding issue (Prod issue)
                    'a = objval.PRICE * objval.Quantity    'Price
                    'j = objval.PRICE * objval.Quantity * gstamt / 100
                    a = CDec(Format(objval.PRICE * objval.Quantity, "###0.00"))    'Price
                    j = CDec(Format(a * gstamt / 100, "###0.00"))
                Else
                    '2015-06-18: CH: Rounding issue (Prod issue)
                    'a = objval.PRICE * objval.Quantity    'Price
                    a = CDec(Format(objval.PRICE * objval.Quantity, "###0.00"))    'Price
                    j = 0
                End If

                ViewState("tax") = ViewState("tax") + j
                ViewState("Sub Total") = ViewState("Sub Total") + a
                ViewState("Total") = ViewState("Total") + a + j
            Else
                ViewState("tax") = ViewState("tax") + 0
                ViewState("Sub Total") = ViewState("Sub Total") + 0
                ViewState("Total") = ViewState("Total") + 0
                'Michelle (24/2/2011) - to indicate this quote is incomplete
                ViewState("indic") = 1
            End If

            'Michelle (23/2/2011) - Don't allow to submit if all line items are without quotes
            'If CType(dgItem.FindControl("txt_price"), TextBox).Text = 0 Then
            If CType(dgItem.FindControl("txt_price"), TextBox).Text = "" Then
                i = i + 1
            End If

        Next

        If dg_viewitem.Items.Count = i Then
            Common.NetMsgbox(Me, "Please quote at least one item.", MsgBoxStyle.Information)
            'ViewState("check") = "2"
            Exit Sub
        Else
            objDB.BatchExecute(strAryQuery)
            'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            ViewState("check") = "1"
        End If
    End Sub

    'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    '    Dim strMsg As String
    '    If Page.IsValid And validateDatagrid(strMsg) Then
    '        Me.onchange.Value = "0"
    '        save()
    '        Bindgrid()
    '    Else
    '        If strMsg <> "" Then
    '            lbl_error.Text = strMsg
    '        Else
    '            lbl_error.Text = ""
    '        End If
    '    End If
    'End Sub

    Private Sub dg_viewitem_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dg_viewitem.PageIndexChanged
        dg_viewitem.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""VendorRFQList.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""RFQSearch.aspx?pageid=" & strPageId & """><span>Quotation Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div><br />"
        Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQListExp.aspx", "pageid=" & strPageId) & """><span>Expired / Rejected RFQ</span></a>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId) & """><span>Quotation Listing</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div><br />"
    End Sub

    Private Function validateInputs() As Boolean


        If Not Me.txt_con_person.Text = "" AndAlso Not IsValidInput(Trim(Me.txt_con_person.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your RFQ Description.", MsgBoxStyle.Information)
            Return False
        End If

        If Not Me.txt_remark.Text = "" AndAlso Not IsValidInputEnter(Trim(Me.txt_remark.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your remarks.", MsgBoxStyle.Information)
            Return False
        End If

        Return True
    End Function


    Protected Sub cmd_update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_update.Click
        Dim strMsg As String
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim rfq_id = Request(Trim("RFQ_ID"))
        Dim Quotation_num As String
        Dim Array(0) As String

        'GetTotal()
        If Not Common.checkMaxLength(txt_remark.Text, 1000) Then
            lblMsg.Text = "<ul type='disc'><li>Remarks is over limit.<ul type='disc'></ul></li></ul>"
            Exit Sub
        End If

        If Page.IsValid And validateDatagrid(strMsg) And validateUnitPrice() And validateInputs() Then
            save()

            'Michelle (23/2/2011) - To prevent the system to continue processing if there is error
            If ViewState("check") = "1" Then

                'ElseIf ViewState("check") = "2" Then
                '    Exit Sub
                'Else
                '    If Session("edit") = "1" Then

                '    End If
                'End If
                objval.RFQ_ID = rfq_id
                If ViewState("indic") = 1 Or ViewState Is Nothing Then
                    objval.indicat = 1
                Else
                    objval.indicat = 0
                End If
                objval.validaty = txt_valid.Text
                objval.remark = Me.txt_remark.Text
                objval.pay_term = ViewState("pay_term")
                objval.pay_type = ViewState("pay_type")
                objval.cur_code = Me.lbl_cur.Text
                'To prevent entering string to a database int type
                If cboShipmentMode.SelectedValue = "" Then
                    objval.ship_mode = "Null"
                Else
                    objval.ship_mode = cboShipmentMode.SelectedValue
                End If
                If cboShipmentTerm.SelectedValue = "" Then
                    objval.ship_term = "Null"
                Else
                    objval.ship_term = cboShipmentTerm.SelectedValue
                End If
                'objval.create_on = Common.ConvertDate(Date.Now)
                objval.create_on = Date.Now
                objval.con_person = txt_con_person.Text
                objval.phone = txt_con_num.Text
                objval.email = txt_email.Text
                objval.total = ViewState("Total")
                objval.RFQ_Num = ViewState("rfq_no")
                Dim comfirmStatus As String = objrfq.add_mstrreplied(objval, Session("edit"), Quotation_num)
                ViewState("quo_no") = Quotation_num
                '******carol******
                If comfirmStatus = "4" Then
                    '//success update
                    'Response.Redirect("RFQCofVen.aspx?Qt_num=" & Quotation_num & "&strType=4&pageid=" & strPageId & "")
                    If strFrm = "RFQSearch" Then
                        Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has been successfully sent to the selected buyer. ", dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId))
                        Me.cmd_update.Visible = False
                        Me.cmdreset.Visible = False
                        Me.cmd_supply.Visible = False

                    Else
                        'Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has been successfully sent to the selected buyers. ", "VendorRFQList.aspx?edit=2&pageid=" & strPageId)
                        Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has been successfully sent to the selected buyer.")
                        Me.cmd_update.Visible = False
                        Me.cmdreset.Visible = False
                        Me.cmd_supply.Visible = False

                    End If
                    Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewQuotation.aspx", "SCoyID=" & Session("CompanyId") & "&BCoyID=" & ViewState("RM_Coy_ID") & "&quo_no=" & Quotation_num & "&rfq_no=" & ViewState("rfq_no")) & "')")
                    Me.cmdView.Visible = True
                    ViewState("type") = "Quotation"

                ElseIf comfirmStatus = "1" Then
                    Common.NetMsgbox(Me, "Duplicate transaction number found. Please contact your Administrator to rectify the problem.", dDispatcher.direct("RFQ", "VendorRFQList.aspx", "edit=2&pageid=" & strPageId))

                ElseIf comfirmStatus = "2" Then
                    'Response.Redirect("RFQCofVen.aspx?Qt_num=" & Quotation_num & "&strType=" & comfirmStatus & "&pageid=" & strPageId & "")
                    If strFrm = "RFQSearch" Then
                        Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has already been sent to selected Buyer.", dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId))

                    Else
                        'Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has already been sent to selected Buyers.", "VendorRFQList.aspx?edit=2&pageid=" & strPageId)
                        'Michelle (3/11/2011) - Issue 1153
                        ' Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " has already been sent to selected Buyer.")
                        Common.NetMsgbox(Me, "Quotation has already been sent to Buyer before.")
                        Me.cmd_update.Visible = False
                        Me.cmdreset.Visible = False
                        Me.cmd_supply.Visible = False
                    End If
                    Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewQuotation.aspx", "SCoyID=" & Session("CompanyId") & "&BCoyID=" & ViewState("RM_Coy_ID") & "&quo_no=" & Quotation_num & "&rfq_no=" & ViewState("rfq_no")) & "')")
                    Me.cmdView.Visible = True
                Else
                    '//not update
                    'Response.Redirect("RFQCofVen.aspx?Qt_num=" & Quotation_num & "&strType=5&pageid=" & strPageId & "")
                    Common.NetMsgbox(Me, "Quotation : " & Quotation_num & " fail to update.Please contact your administrator.", dDispatcher.direct("RFQ", "VendorRFQList.aspx", "edit=2&pageid=" & strPageId))

                End If
                '//***
            Else
                If strMsg <> "" Then
                    lblMsg.Text = strMsg
                Else
                    lblMsg.Text = ""
                End If
            End If
        End If
    End Sub

    'Private Sub GetTotal()
    '    Dim objrfq As New RFQ
    '    Dim i As Integer
    '    Dim j As Double
    '    Dim cnt As Integer
    '    Dim a As Double
    '    For cnt = 0 To Me.dg_viewitem.Items.Count
    '        a = dg_viewitem.Items(cnt).Cells(5).Text * dg_viewitem.Items(cnt).Cells(3).Text`    'Price
    '        i = objrfq.get_gst(dv("RRDT_GST_Code"))
    '        j = dg_viewitem.Items(cnt).Cells(0).Text * dg_viewitem.Items(cnt).Cells(0).Text * i / 100
    '        lbl_tax.Text = Format(j, "###,###,##0.0000")

    '        ViewState("tax") = ViewState("tax") + j
    '        ViewState("Sub Total") = ViewState("Sub Total") + a
    '        ViewState("Total") = ViewState("Total") + a + j
    '    Next

    '    'Dim lbl_tax As Label
    '    'lbl_tax = e.Item.FindControl("lbl_tax")
    '    'Dim i As Integer = objrfq.get_gst(dv("RRDT_GST_Code"))
    '    'Dim j As Double = dv("RRDT_Unit_Price") * dv("RRDT_Quantity") * i / 100
    '    'lbl_tax.Text = Format(j, "###,###,##0.0000")
    '    'e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
    '    'e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
    '    'ViewState("tax") = ViewState("tax") + j
    '    'ViewState("Sub Total") = ViewState("Sub Total") + a
    '    'ViewState("Total") = ViewState("Total") + a + j
    'End Sub

    'Protected Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdView.Click
    '    PreviewQuotation()
    'End Sub

    Private Sub PreviewQuotation()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT (SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)) AS CMState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS SupplierAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                '            & "AS SupplierAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) AND (CODE_CATEGORY = 'pt')) " _
                '            & "AS PaymentTerm, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                '            & "AS PaymentMethod, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) AND (CODE_CATEGORY = 'St')) " _
                '            & "AS Ship_Term, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) AND (CODE_CATEGORY = 'sm')) " _
                '            & "AS Ship_Mode, COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, " _
                '            & "COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                '            & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                '            & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                '            & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                '            & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                '            & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, " _
                '            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                '            & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, " _
                '            & "COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, " _
                '            & "RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, " _
                '            & "RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                '            & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, " _
                '            & "RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, " _
                '            & "RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, " _
                '            & "RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, RFQ_MSTR.RM_RFQ_OPTION, " _
                '            & "RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Offer_Till, RFQ_REPLIES_MSTR.RRM_ETD, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Ship_Term, RFQ_REPLIES_MSTR.RRM_Created_On, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Status, RFQ_REPLIES_MSTR.RRM_B_Display_Status, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Display_Status, RFQ_REPLIES_MSTR.RRM_Indicator, " _
                '            & "RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Product_Code, RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Quantity, RFQ_REPLIES_DETAIL.RRD_Unit_Price, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Desc, RFQ_REPLIES_DETAIL.RRD_Product_Desc, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_UOM, RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, RFQ_REPLIES_DETAIL.RRD_Remarks, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID, " _
                '            & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                '            & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                '            & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                '            & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                '            & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, " _
                '            & "COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                '            & "FROM      RFQ_MSTR INNER JOIN " _
                '            & "RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID INNER JOIN " _
                '            & "RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID AND " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id INNER JOIN " _
                '            & "COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                '            & "COMPANY_MSTR AS COMPANY_MSTR_1 ON " _
                '            & "RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                '            & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                '            & "(RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND " _
                '            & "(RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
                .CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState,d.CODE_DESC AS SupplierAddrCtry," _
                       & "e.CODE_DESC AS PaymentTerm,f.CODE_DESC AS PaymentMethod,g.CODE_DESC AS Ship_Term,h.CODE_DESC AS Ship_Mode," _
                       & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                       & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                       & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                       & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                       & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                       & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                       & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                       & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
                       & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
                       & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                       & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
                       & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
                       & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
                       & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                       & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
                       & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                       & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
                       & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                       & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                       & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                       & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                       & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
                       & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
                       & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                       & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                       & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                       & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                       & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                       & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                       & "FROM RFQ_MSTR " _
                       & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
                       & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
                       & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
                       & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
                       & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                       & "INNER JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
                       & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
                       & "INNER JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
                       & "AND (b.CODE_CATEGORY = 'ct') " _
                       & "INNER JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
                       & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
                       & "INNER JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
                       & "AND (d.CODE_CATEGORY = 'ct') " _
                       & "INNER JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
                       & "AND (e.CODE_CATEGORY = 'pt') " _
                       & "INNER JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
                       & "AND (f.CODE_CATEGORY = 'pm') " _
                       & "INNER JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
                       & "AND (g.CODE_CATEGORY = 'St') " _
                       & "INNER JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
                       & "AND (h.CODE_CATEGORY = 'sm') " _
                       & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
                       & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Session("CompanyId")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", ViewState("RM_Coy_ID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmQuoNum", ViewState("quo_no")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", ViewState("rfq_no")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewQuotation_FTN_DataTablePreviewQuotation", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewQuotation-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
               "<DeviceInfo>" + _
                   "  <OutputFormat>EMF</OutputFormat>" + _
                   "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "RFQ\Quotation.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('Quotation.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

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

    Private Sub cmd_supply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_supply.Click
        'Dim strMsg As String
        'Dim objrfq As New RFQ
        'Dim objval As New RFQ_User
        'Dim rfq_id = Request(Trim("RFQ_ID"))
        'Dim Quotation_num As String
        'Dim comfirmStatus As String = objrfq.update_replied(rfq_id, Session("edit"))

        Dim objGlobal As New AppGlobals
        Dim strMsg As String
        strMsg = objGlobal.GetErrorMessage("00259")
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        'strscript.Append("document.getElementById('cmd_submit').style.display='none';")
        strscript.Append("PromptMsg('" & strMsg & "');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())

        'If comfirmStatus = "1" Then
        '    Common.NetMsgbox(Me, "RFQ : " & ViewState("rfq_no") & " rejected.", dDispatcher.direct("RFQ", "VendorRFQList.aspx", "edit=2&pageid=" & strPageId))
        'End If
    End Sub

    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        Dim strMsg As String
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim rfq_id = Request(Trim("RFQ_ID"))
        Dim Quotation_num As String

        If hidresult.Value = "1" Then
            Dim comfirmStatus As String = objrfq.update_replied(rfq_id, Session("edit"))

            If comfirmStatus = "1" Then
                Common.NetMsgbox(Me, "RFQ : " & ViewState("rfq_no") & " rejected.", dDispatcher.direct("RFQ", "VendorRFQList.aspx", "edit=2&pageid=" & strPageId))
            End If
        End If

    End Sub

End Class