Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class Create_RFQ
    Inherits AgoraLegacy.AppBaseClass

    Protected WithEvents Button5 As System.Web.UI.WebControls.Button
    Protected WithEvents Button6 As System.Web.UI.WebControls.Button
    Protected WithEvents Button7 As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Dim a As New AppGlobals
    Dim objRFQ As New RFQ
    Dim cell As Char
    Dim row1 As Integer
    Dim total As Integer
    Dim check As Integer = 0
    Dim back As String = 0
    'Dim rfq_name As String
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents lbl_title As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_rfq_number As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_name As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lbl_rfqname As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_cur As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_cur As System.Web.UI.WebControls.Label
    Protected WithEvents opt_RFQ_option As System.Web.UI.WebControls.RadioButtonList
    'Protected WithEvents lbl_count As System.Web.UI.WebControls.Label
    'Protected WithEvents cmd_view As System.Web.UI.WebControls.Button
    'Protected WithEvents dt_rfq As System.Web.UI.WebControls.Table
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_addcath As System.Web.UI.WebControls.Button
    Protected WithEvents rvl_RFQName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents cmd_Previous As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents Table4 As System.Web.UI.WebControls.Table
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdreset As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rvl_date As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents ddl_pt As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_pm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_sm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmd_addSV As System.Web.UI.WebControls.Button
    Protected WithEvents ddl_st As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgviewitem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_vendor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txt_exp As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_num As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_email As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_cont_person As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_validity As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Submit As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_View As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmd_upload As System.Web.UI.WebControls.Button
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents img_delete As System.Web.UI.WebControls.Image
    Protected WithEvents ddlVendorList As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboVendor As System.Web.UI.WebControls.DropDownList
    Dim aryProdCode As New ArrayList
    Dim objread As New RFQ_User()
    Dim cur_value As String
    Dim strQtyErr As String
    Dim imageid As Integer
    Dim objFile As New FileManagement
    Dim intRow As Integer
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum ItemEnum
        Chk = 0
        Desc = 1
        UOM = 2
        QTY = 3
        Time = 4
        Warranty = 5
        'Index = 6
        ProdCode = 6
        CoyID = 7
        VIC = 8
        Type = 9
    End Enum

    Public Enum RFQ2Enum
        List = 0
        Image = 1
        ListIndex = 2
        type = 3
        Add = 4
    End Enum


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_save.Enabled = False
        'cmd_addcath.Enabled = False
        'cmd_addSV.Enabled = False
        If Session("edit") = 1 Then
            Dim alButtonList As ArrayList
            alButtonList = New ArrayList
            alButtonList.Add(cmd_save)
            alButtonList.Add(cmd_addcath)
            alButtonList.Add(cmd_addSV)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmd_delete)
            htPageAccess.Add("delete", alButtonList)

            CheckButtonAccess()
            cmd_save.Enabled = blnCanUpdate
            'cmdreset.Enabled = blnCanUpdate
            alButtonList.Clear()
        Else
            Dim alButtonList As ArrayList
            alButtonList = New ArrayList
            alButtonList.Add(cmd_save)
            alButtonList.Add(cmd_addcath)
            alButtonList.Add(cmd_addSV)
            htPageAccess.Add("add", alButtonList)

            CheckButtonAccess()
            cmd_save.Enabled = blnCanAdd
            cmd_addcath.Enabled = blnCanAdd
            'cmdreset.Enabled = blnCanAdd
            cmd_addSV.Enabled = blnCanAdd
            'cmd_delete.Enabled = blnCanDelete
            alButtonList.Clear()
        End If

        If ViewState("modePR") = "pr" Then
            cmd_addcath.Visible = False
            cmd_delete.Visible = False
        End If

        ViewState("update") = Session("update")
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
    
        Dim edit As String
        Dim objrfq As New RFQ
        Dim rfq_id, strFrm As String
        imageid = 0
        Dim a As New AppGlobals
        blnPaging = False
        blnSorting = False
        SetGridProperty(dgviewitem)
        Me.blnPaging = False
        Me.blnSorting = False
        SetGridProperty(Me.dtg_vendor)
        aryProdCode.Clear()

        If Not Page.IsPostBack Then
            'Session("strItem") = Nothing
            'Michelle (4/2/2013) - Issue 1846
            strQtyErr = a.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr
            BindPreDefinedVendor()
            strFrm = Me.Request.QueryString("Frm")
            If strFrm = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", Request.QueryString("dpage") & ".aspx", "pageid=" & strPageId)
            ElseIf strFrm = "RFQ_Outstg_List" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId)
            ElseIf strFrm = "RFQ_List" Then
                lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId)
            ElseIf strFrm = "BC" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "BuyerCatalogueSearch.aspx", "pageid=" & strPageId)
            ElseIf strFrm = "VC" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "VendorSearch.aspx", "pageid=" & strPageId)
            Else
                lnkBack.Visible = False
            End If
            GenerateTab()
            Session("RFQ_NUM") = ""
            Session("RFQ_NAME") = ""
            Session("rfq_id") = ""
            Session("VendorList") = Nothing
            Session("VendorListDetails") = Nothing
            rvl_date.Type = ValidationDataType.Date
            rvl_date.MaximumValue = Common.FormatWheelDate(WheelDateFormat.ShortDate, Date.Now.AddYears(100))
            rvl_date.MinimumValue = Common.FormatWheelDate(WheelDateFormat.ShortDate, Date.Now.Today)
            a.FillCodeTable(ddl_cur, CodeTable.Currency, , "RFQ")
            objrfq.delete_TempVenListnItem() 'Delete those temp records created in the previous session (incase user exit IE without proper log off)
            cmd_upload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            If Request(Trim("caller")) = "VendorSearch" Or Request(Trim("caller")) = "PurchaserSearch" Then 'ie raise from Vendor/Purchaser Item Search
                Dim aryProdCode As New ArrayList
                Dim strProdList As String
                Dim i As Integer
                aryProdCode = Session("RFQProdList")
                For i = 0 To aryProdCode.Count - 1
                    If strProdList = "" Then
                        strProdList = "'" & aryProdCode(i)(0) & "'"
                    Else
                        strProdList &= ", '" & aryProdCode(i)(0) & "'"
                    End If
                Next
                objrfq.add_RFQCatSearch_TEMP(strProdList)
            End If
            Me.cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            a.FillCodeTable(ddl_pt, CodeTable.PaymentTerm)
            a.FillCodeTable(ddl_pm, CodeTable.PaymentMethod)
            a.FillCodeTable(ddl_sm, CodeTable.ShipmentMode)
            a.FillCodeTable(ddl_st, CodeTable.ShipmentTerm)

            Me.txt_name.Focus()
            'Bindgrid(True)
        End If

        If Session("edit") = "" Then
            edit = Request(Trim("edit"))
            Session("edit") = edit
        Else
            If Session("edit") = Request(Trim("edit")) Or Request(Trim("edit")) <> "" Then
                If Not IsNothing(Request(Trim("edit"))) Then
                    edit = Request(Trim("edit"))
                    Session("edit") = edit
                End If
            End If
        End If

        If edit = "1" Then
            Session("back") = "1"
            Session("saverfq2") = "1"
        End If

        If Session("rfq_id") = "" Then
            Session("rfq_id") = Request(Trim("RFQ_ID"))
        Else
            If Session("rfq_id") <> Request(Trim("RFQ_ID")) And Request(Trim("RFQ_ID")) <> "" Then
                If Not IsNothing(Request(Trim("RFQ_ID"))) Then
                    Session("rfq_id") = Request(Trim("RFQ_ID"))
                End If
            End If
        End If

        'If ViewState("rfq_option_check") = "1" Then
        '    Dim rfq_option As String
        '    rfq_option = objrfq.chk_RfqOption2(Session("rfq_id"))
        '    opt_RFQ_option.SelectedItem.Value = rfq_option
        'End If

        If Session("RFQ_Name") = "" Then Session("RFQ_Name") = txt_name.Text
        'If Session("RFQ_Name") = "" Then
        '    Session("RFQ_Name") = txt_name.Text
        'Else
        '    If Session("RFQ_Name") <> Request(Trim("RFQ_Name")) And Request(Trim("RFQ_Name")) <> "" Then
        '        If Not IsNothing(Request(Trim("RFQ_Name"))) Then
        '            Session("RFQ_Name") = Request(Trim("RFQ_Name"))
        '        End If
        '    End If
        'End If

        If Session("rfq_num") = "" Then
            Session("rfq_num") = Request(Trim("RFQ_Num"))
        Else
            If Session("rfq_num") <> Request(Trim("RFQ_Num")) And Request(Trim("RFQ_Num")) <> "" Then
                If Not IsNothing(Request(Trim("RFQ_Num"))) Then
                    Session("rfq_num") = Request(Trim("RFQ_Num"))
                End If
            End If
        End If

        If Session("RFQ_Cur") = "" Then
            Session("RFQ_Cur") = Request(Trim("RFQ_Cur"))
        Else
            If Session("RFQ_Cur") <> Request(Trim("RFQ_Cur")) And Request(Trim("RFQ_Cur")) <> "" Then
                If Not IsNothing(Request(Trim("RFQ_Cur"))) Then
                    Session("RFQ_Cur") = Request(Trim("RFQ_Cur"))
                End If
            End If
        End If

        If Session("RFQ_Cur_text") = "" Then
            Session("RFQ_Cur_text") = Request(Trim("RFQ_Cur_text"))
        Else
            If Session("RFQ_Cur_text") <> Request(Trim("RFQ_Cur_text")) And Request(Trim("RFQ_Cur_text")) <> "" Then
                If Not IsNothing(Request(Trim("RFQ_Cur_text"))) Then
                    Session("RFQ_Cur_text") = Request(Trim("RFQ_Cur_text"))
                End If
            End If
        End If

        If Session("rfq_num") = "" Then
            Me.lbl_rfq_number.Text = "To Be Allocated By System"
            Dim objDB As New EAD.DBCom
            Dim strPt, strPm As String

            'strPt = objDB.Get1Column("COMPANY_MSTR", "CM_PAYMENT_TERM", " WHERE CM_COY_ID = '" & Session("CompanyID") & "' ")
            'strPm = objDB.Get1Column("COMPANY_MSTR", "CM_PAYMENT_METHOD", " WHERE CM_COY_ID = '" & Session("CompanyID") & "' ")
            'Common.SelDdl(strPt, ddl_pt)
            'Common.SelDdl(strPm, ddl_pm)

            ' Common.FillDefault(ddl_pt, "COMPANY_MSTR", "RVDLM_List_Name", _
            '"RVDLM_List_Index", "---select---", " RVDLM_User_Id ='" & Session("UserId") & "' and RVDLM_Coy_Id ='" & Session("CompanyId") & "'")

        Else
            Me.lbl_rfq_number.Text = Session("rfq_num")
        End If

        Dim objDB2 As New EAD.DBCom
        Dim strRFQ_No As String = objDB2.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Session("rfq_num") & "'")
        If strRFQ_No <> "" Then
            ViewState("modePR") = "pr"
        End If

        Me.lbl_cur.Visible = False
        'Me.lbl_rfqname.Visible = False
        If ViewState("save") = "1" Then
            Me.lbl_cur.Visible = True
            'Me.lbl_rfqname.Visible = True
        End If

        If (Session("back") = "1") Then
            Me.lbl_cur.Visible = True
            'Me.lbl_rfqname.Visible = True
            Dim RFQ_Name As String
            Dim RFQ_Cur As String
            Dim RFQ_Cur_text As String
            If edit = "1" Then
                Dim objrfq1 As New RFQ
                rfq_id = Session("rfq_id")
                objrfq1.read_rfqMstr(objread, "", rfq_id)
                'lbl_rfqname.Text = objread.RFQ_Name
                'txt_name.Text = objread.RFQ_Name
                Common.SelDdl(objread.cur_code, ddl_cur)
                lbl_cur.Text = ddl_cur.SelectedItem.Text()
            Else
                RFQ_Name = Session("RFQ_Name")
                RFQ_Cur = Session("RFQ_Cur")
                RFQ_Cur_text = Session("RFQ_Cur_text")
                'Me.lbl_rfqname.Text = RFQ_Name
                Me.lbl_cur.Text = RFQ_Cur_text
                txt_name.Text = RFQ_Name
                Common.SelDdl(RFQ_Cur, ddl_cur)
            End If

            ddl_cur.Visible = False
            'txt_name.Visible = False
        End If

        If Not Page.IsPostBack Then
            'GenerateTab()
            'a.FillCodeTable(ddl_pt, CodeTable.PaymentTerm)
            ''Common.SelDdl(objread.pay_term, ddl_pt)
            'a.FillCodeTable(ddl_pm, CodeTable.PaymentMethod)
            ''Common.SelDdl(objread.pay_type, ddl_pm)
            'a.FillCodeTable(ddl_sm, CodeTable.ShipmentMode)
            'a.FillCodeTable(ddl_st, CodeTable.ShipmentTerm)
            Bindgrid(True)

            Dim objrfq1 As New RFQ
            Dim checkdata As String

            Dim rfq_option As Integer
            'rfq_option OPEN=0
            'close=1
            'Def by User = 2
            rfq_option = objrfq.chk_RfqOption()

            If rfq_option = "1" Then 'close
                opt_RFQ_option.SelectedIndex = 1
                opt_RFQ_option.Enabled = False
            ElseIf rfq_option = "0" Then 'open
                opt_RFQ_option.SelectedIndex = 0
                opt_RFQ_option.Enabled = False
            ElseIf rfq_option = "2" Then '
                opt_RFQ_option.SelectedIndex = 0
                opt_RFQ_option.Enabled = True
            End If

            If Session("rfq_id") <> "" Then
                'checkdata As String = 
                objrfq1.read_rfqMstr(objread, Session("RFQ_name"), Session("rfq_id"), Session("rfq_num"))
                Common.SelDdl(objread.ship_mode, ddl_sm)
                Common.SelDdl(objread.ship_term, ddl_st)
                Common.SelDdl(objread.pay_term, ddl_pt)
                Common.SelDdl(objread.pay_type, ddl_pm)
                If objread.RFQ_Option = "0" Then
                    opt_RFQ_option.SelectedIndex = 0
                Else
                    opt_RFQ_option.SelectedIndex = 1
                End If
            Else
                'Common.SelDdl(objread.pay_term, ddl_pt)
                'Common.SelDdl(objread.pay_type, ddl_pm)
                'If checkdata <> "1" Then 'no record in RFQ_MSTR
                objread.RFQ_Name = ""
                objrfq.read_user(objread, Session("RFQ_Name"))
                'Else
                'Common.SelDdl(objread.ship_mode, ddl_sm)
                'Common.SelDdl(objread.ship_term, ddl_st)
            End If

            'From Create_RFQ2
            '------
            If objread.exp_date = "" Then
                txt_exp.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Date.Today)
            Else
                txt_exp.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objread.exp_date)
            End If

            If objread.RFQ_Req_date = "" Then
                txt_validity.Text = Date.Today.AddDays(1)
            Else
                txt_validity.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objread.RFQ_Req_date)
            End If

            If Session("rfq_id") = "" Then
                txt_exp.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Date.Today)
                txt_validity.Text = Date.Today.AddDays(1)
            End If

            Me.txt_cont_person.Text = objread.user_name
            Me.txt_num.Text = objread.phone
            Me.txt_email.Text = objread.email
            Me.txt_name.Text = objread.RFQ_Name

            Dim objrfqchk As New RFQ
            ViewState("RFQ_Num") = objrfqchk.get_rfq_num(objread.RFQ_Name)
            Me.txt_remark.Text = objread.remark
            '-----------
            Session("update") = Server.UrlEncode(Date.Today.Now.ToString)
            txt_name.Attributes.Add("onKeyDown", "limitText (this, 100);")

            txt_remark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
            'Session("FromDraft") = Request(Trim("draft"))
            'If Session("FromDraft") = "1" Then
            '    Me.lbl_title.Text = "Edit RFQ (Step 1)"
            'End If

            'Me.lbl_count.Text = objrfq.count_item(Me.txt_name.Text, Me.lbl_rfq_number.Text)
            'Session("row1") = 0
            'bind_table(5)
            'Else

            '    Dim ag As Integer = dt_rfq.Rows.Count()

            '    bind_table(ViewState("total"))

        End If

        'If Me.lbl_count.Text = "0" Then
        '    Me.cmd_view.Enabled = False
        'Else
        'Me.cmd_view.Enabled = True
        'End If
        cmd_save.Attributes.Add("onclick", "return hideButton();")
        'Me.cmd_addSV.Attributes.Add("onclick", "window.open(""VendorList.aspx?edit=0&RFQ_Name=" & Server.UrlEncode(Me.lbl_name.Text) & "&RFQ_list=RFQ Name&RFQ_venlist_num=" & ViewState("listindex2") & "&pageid=" & strPageId & """)")
        Session("RFQVendorList") = ""
        'Me.cmd_addSV.Attributes.Add("onclick", "window.open(""VendorList.aspx?edit=0&RFQ_No=abc&RFQ_list=RFQ Name&pageid=" & strPageId & """); document.getElementById(""btnHidden"").click();")
        imageid = 0
        'Bindgrid(True)
        Bindgrid_vendor()

        displayAttachFile()
        'rvl_RFQName.Enabled = False 'True


        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmd_Submit.Enabled = False
        End If

    End Sub

    Private Sub cmd_upload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_upload.Click
        Dim objFile As New FileManagement
        If File1.Value = "" Then

        Else
            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                'objFile.FileUpload(File1, EnumUploadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff, Session("RFQ_Num"))
                If Session("RFQ_Num") <> "" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff, Session("RFQ_Num"))
                Else
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff, Session.SessionID)
                End If
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
        End If

    End Sub
    Private Sub displayAttachFile()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        'dsAttach = objRFQ.getRFQTempAttach(Session("RFQ_Num"), Session("CompanyID"))
        If Session("RFQ_Num") <> "" Then
            dsAttach = objRFQ.getRFQTempAttach(Session("RFQ_Num"), Session("CompanyID"))
        Else
            dsAttach = objRFQ.getRFQTempAttach(Session.SessionID, Session("CompanyID"))
        End If

        Dim intCount As Integer
        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                If Common.parseNull(drvAttach(i)("CDA_TYPE")) = "E" Then
                    strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                    strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                    '*************************meilai 25/2/05****************************
                    'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=RFQ>" & strFile & "</A>"
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff)
                    '*************************meilai************************************
                    Dim lblBr As New Label
                    Dim lblFile As New Label
                    Dim lnk As New ImageButton
                    lblFile.Text = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                    lblBr.Text = "<BR>"
                    lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
                    lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                    lnk.CausesValidation = False
                    AddHandler lnk.Click, AddressOf deleteAttach

                    pnlAttach.Controls.Add(lblFile)
                    pnlAttach.Controls.Add(lnk)
                    pnlAttach.Controls.Add(lblBr)

                    intCount = intCount + 1
                End If
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objpr As New PR

        If cmd_View.Visible = True And cmd_save.Visible = False And cmd_Submit.Visible = False And cmd_delete.Visible = False And cmd_upload.Visible = False Then
            Common.NetMsgbox(Me, Session("rfq_num") & " has been sent, you cannot modify this content.", MsgBoxStyle.Information, "Wheel")
        Else
            objRFQ.deleteRFQAttachment(CType(sender, ImageButton).ID)
            displayAttachFile()
        End If

    End Sub


    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

    '        e.Item.Cells(RFQ2Enum.List).Text = dv("RIV_S_Coy_Name")
    '        e.Item.Cells(RFQ2Enum.CoyID).Text = dv("RIV_S_Coy_ID")

    '        Dim img_delete As ImageButton
    '        img_delete = e.Item.FindControl("img_delete")
    '        img_delete.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")

    '        img_delete.ID = imageid ' viewstate("i")
    '        img_delete.CausesValidation = False
    '        'If ViewState("chk") = "" Then
    '        '    ViewState("chk") = "1"
    '        'End If
    '        imageid = imageid + 1
    '        '  i = i + 1
    '        'img_delete.Enabled = False
    '        'Dim alButtonList As ArrayList
    '        'alButtonList = New ArrayList
    '        'alButtonList.Add(img_delete)
    '        'htPageAccess.Add("delete", alButtonList)
    '        'img_delete.Enabled = blnCanDelete
    '        AddHandler img_delete.Click, AddressOf delete

    '    End If
    'End Sub

    Private Sub dtg_vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_vendor.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkVendor As LinkButton
            Dim strurl As String
            Dim lblVendor As Label

            If dv("TYPE") = "list" Then
                strurl = dDispatcher.direct("RFQ", "PredefinedVendorList.aspx", "VendorListName=" & Server.UrlEncode(dv("RVDLM_List_Name")) & "&RFQ_venlist_num=" & dv("RVDLM_List_Index") & "&edit=1&Added=Y&pageid=" & strPageId)
                lblVendor = e.Item.Cells(0).FindControl("lblVendor")
                lblVendor.Visible = False
                lnkVendor = e.Item.Cells(0).FindControl("lnkVendor")
                lnkVendor.Visible = True
                lnkVendor.Text = dv("RVDLM_List_Name")
                lnkVendor.CommandArgument = strurl
                lnkVendor.CausesValidation = False

            ElseIf dv("TYPE") = "specific" Then
                lnkVendor = e.Item.Cells(0).FindControl("lnkVendor")
                lnkVendor.Visible = False
                lblVendor = e.Item.Cells(0).FindControl("lblVendor")
                lblVendor.Text = dv("RVDLM_List_Name")
                lblVendor.Visible = True
            End If

            Dim img_delete As ImageButton
            img_delete = e.Item.FindControl("img_delete")
            img_delete.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")

            img_delete.ID = imageid
            img_delete.CausesValidation = False

            imageid = imageid + 1

            AddHandler img_delete.Click, AddressOf delete

        End If
    End Sub

    Sub LinkButton_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = e.CommandArgument
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','510px');")
        'strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    'Private Sub delete(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim dgitem As DataGridItem
    '    Dim objDB As New EAD.DBCom
    '    'Dim strAryQuery(0) As String
    '    Dim strSQL As String
    '    Dim i As Integer
    '    objval.RFQ_Name = Session("RFQ_name")
    '    '  objval.dis_ID = CType(sender, ImageButton).ID
    '    Dim rowno As Integer = CType(sender, ImageButton).ID

    '    objval.V_com_ID = dtg_vendor.Items(rowno).Cells(0).Text 'dtg_vendor.Items(rowno).Cells(RFQ2Enum.CoyID).Text
    '    'strSQL = objrfq.Vendor_Add_Inv_Ven_List2_TEMP(objval, "D")
    '    'Common.Insert2Ary(strAryQuery, strSQL)
    '    'objDB.Execute(strSQL)
    '    objrfq.Vendor_Upd_Inv_Ven_List2_TEMP(objval, "D")
    '    'For Each dgitem In Me.dtg_vendor.Items
    '    'If dgitem.Cells(RFQ2Enum.type).Text = "list" And dgitem.Cells(RFQ2Enum.ListIndex).Text _
    '    '<> dtg_vendor.Items(rowno).Cells(RFQ2Enum.ListIndex).Text Then
    '    ' dgitem.Cells(RFQ2Enum.List).Text = "<A href=""#"" onclick=""window.open(&quot;" & strurl & "&quot;);""><font color=#0000ff>" & dv("RVDLM_List_Name") & "</font></A>"
    '    'If viewstate("listindex") = "" Then
    '    '    viewstate("listindex") = dgitem.Cells(RFQ2Enum.ListIndex).Text
    '    'Else
    '    '    viewstate("listindex") = viewstate("listindex") & "," & dgitem.Cells(RFQ2Enum.ListIndex).Text
    '    'End If
    '    'ElseIf dgitem.Cells(RFQ2Enum.type).Text = "specific" And dgitem.Cells(RFQ2Enum.ListIndex).Text _
    '    '        <> dtg_vendor.Items(rowno).Cells(RFQ2Enum.ListIndex).Text Then
    '    'ElseIf dgitem.Cells(RFQ2Enum.type).Text = "specific" And dgitem.Cells(RFQ2Enum.ListIndex).Text _
    '    '        <> dtg_vendor.Items(rowno).Cells(RFQ2Enum.ListIndex).Text Then
    '    '    If ViewState("com_id") = "" Then
    '    '        ViewState("com_id") = "'" & dgitem.Cells(RFQ2Enum.ListIndex).Text & "'"
    '    '    Else
    '    '        ViewState("com_id") = ViewState("com_id") & "," & "'" & dgitem.Cells(RFQ2Enum.ListIndex).Text & "'"
    '    '    End If
    '    '    'End If

    '    'Next
    '    'objval.dis_ID = dtg_vendor.Items(rowno).Cells(RFQ2Enum.ListIndex).Text
    '    'objval.list_index = viewstate("listindex")
    '    'objval.V_com_ID = viewstate("com_id")
    '    'objval.type = dtg_vendor.Items(rowno).Cells(RFQ2Enum.type).Text
    '    'objval.RFQ_ID = Session("rfq_id")
    '    'objrfq.delete_venList(objval)

    '    'viewstate("checkven") = "1"
    '    'Me.Page_Load(sender, e)
    '    imageid = 0
    '    Bindgrid_vendor()
    'End Sub

    Private Sub delete(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objrfq As New RFQ
        Dim dgitem As DataGridItem
        Dim i As Integer
        Dim dtList As DataTable
        Dim dtDetails As DataTable
        Dim dtrList As DataRow()
        Dim dtrDetails As DataRow()
        Dim strSearch As String = ""

        dtList = Session("VendorList")
        dtDetails = Session("VendorListDetails")

        Dim rowno As Integer = CType(sender, ImageButton).ID
        For Each dgitem In Me.dtg_vendor.Items
            If dgitem.ItemIndex = rowno Then
                strSearch = "RVDLM_List_Index='" & dgitem.Cells(2).Text & "' AND TYPE = '" & dgitem.Cells(3).Text & "'"
                dtrList = dtList.Select(strSearch)
                If dtrList.Length > 0 Then  'If found
                    For Each oRow As DataRow In dtrList
                        dtList.Rows.Remove(oRow)
                    Next
                End If

                strSearch = "RVDLM_List_Index='" & dgitem.Cells(2).Text & "' AND TYPE = '" & dgitem.Cells(3).Text & "'"
                dtrDetails = dtDetails.Select(strSearch)
                If dtrDetails.Length > 0 Then
                    For Each oRow As DataRow In dtrDetails
                        dtDetails.Rows.Remove(oRow)
                    Next
                End If
            End If
        Next
        'yAP: 13Dec2012: Comment out following code due the datagrid will load twice, 
        'because error when our build in error log load.
        'Issue 1797
        '' ''Me.Page_Load(sender, e)
        imageid = 0
        Bindgrid_vendor()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objview As New RFQ
        Dim ds As New DataSet

        intRow = 0
        ds = objview.get_items(Session("rfq_id"), ViewState("modePR"))

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            cmd_delete.Visible = True
        Else
            cmd_delete.Visible = False
        End If

        If ViewState("action") = "del" Then
            If dgviewitem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgviewitem.PageSize = 0 Then
                dgviewitem.CurrentPageIndex = dgviewitem.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dgviewitem.DataSource = dvViewSample
        dgviewitem.DataBind()
        Session("RFQProdList") = aryProdCode
    End Function
    'Private Function Bindgrid_vendor(Optional ByVal pSorted As Boolean = False) As String
    '    Dim objrfq As New RFQ
    '    Dim ds As New DataSet

    '    ds = objrfq.get_VenList(Session("rfq_id"))
    '    Dim dvViewSample As DataView
    '    dvViewSample = ds.Tables(0).DefaultView
    '    'Dim strurl As String = "VendorList.aspx?edit=0&RFQ_Name=" & Server.UrlEncode(Me.lbl_name.Text) & "&RFQ_list=RFQ%20Name&RFQ_venlist_num=" & Me.ddl_list.SelectedItem.Value & "&pageid=" & strPageId & ""


    '    If pSorted Then
    '        dvViewSample.Sort = ViewState("SortExpression")
    '        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
    '    End If

    '    If ViewState("action") = "del" Then
    '        If dtg_vendor.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_vendor.PageSize = 0 Then
    '            dtg_vendor.CurrentPageIndex = dtg_vendor.CurrentPageIndex - 1
    '            ViewState("action") = ""
    '        End If
    '    End If

    '    dtg_vendor.DataSource = dvViewSample
    '    'ViewState("chk") = ""
    '    dtg_vendor.DataBind()

    '    'Me.cmd_addSV.Attributes.Add("onclick", "window.open(""VendorList.aspx?edit=0&RFQ_Name=" & Server.UrlEncode(Me.lbl_name.Text) & "&RFQ_list=RFQ Name&RFQ_venlist_num=" & ViewState("listindex2") & "&pageid=" & strPageId & """)")

    'End Function

    Private Function Bindgrid_vendor(Optional ByVal pSorted As Boolean = False) As String
        Dim objrfq As New RFQ
        Dim ds As New DataSet
        Dim dsTemp As New DataSet
        Dim dvViewSample As DataView
        Dim i As Integer = 0
        Dim dtr As DataRow
        Dim dt1 As DataTable

        ds = objrfq.get_RFQVenList(Session("rfq_id")) 'objrfq.get_VenList(Session("rfq_id"))
        If Not IsNothing(Session("VendorList")) Then
            dt1 = Session("VendorList")
            dvViewSample = dt1.DefaultView 'ds.Tables(0).DefaultView
            ViewState("intPageRecordCnt") = dt1.Rows.Count
        Else
            Session("VendorList") = ds.Tables(0)
            dvViewSample = ds.Tables(0).DefaultView
            ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        End If

        dsTemp = objrfq.get_RFQVenListDetails(Session("rfq_id"))
        If IsNothing(Session("VendorListDetails")) Then
            Session("VendorListDetails") = dsTemp.Tables(0)
        End If

        If pSorted Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If ViewState("action") = "del" Then
            If dtg_vendor.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_vendor.PageSize = 0 Then
                dtg_vendor.CurrentPageIndex = dtg_vendor.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dtg_vendor.DataSource = dvViewSample
        dtg_vendor.DataBind()

    End Function

    Private Sub dgviewitem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgviewitem.ItemCreated
        Grid_ItemCreated(sender, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dgviewitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgviewitem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim strItem As New ArrayList()

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(ItemEnum.Chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim txt_qty As TextBox
            txt_qty = e.Item.FindControl("txt_qty")
            txt_qty.Text = Common.parseNull(dv("RD_Quantity"))

            Dim val_Qty As RegularExpressionValidator
            val_Qty = e.Item.FindControl("val_Qty")
            val_Qty.ControlToValidate = "txt_qty"
            'Michelle (4/2/2013) - Issue 1846
            'val_Qty.ValidationExpression = "(?!^0*$)^\d{1,5}?$"
            val_Qty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$"
            val_Qty.ErrorMessage = CStr(intRow + 1) & ". " & ViewState("ValQtyMsg")
            val_Qty.Text = "?"
            val_Qty.Display = ValidatorDisplay.Dynamic

            Dim reqVal_Qty As RequiredFieldValidator
            reqVal_Qty = e.Item.FindControl("reqVal_Qty")
            reqVal_Qty.ControlToValidate = "txt_qty"
            reqVal_Qty.ErrorMessage = CStr(intRow + 1) & ". Require quantity"
            reqVal_Qty.Text = "?"
            reqVal_Qty.Display = ValidatorDisplay.Dynamic

            Dim objrfq As New RFQ
            Dim ddl_uom As New DropDownList
            Dim dv_uom As DataView
            Dim objGlobal As New AppGlobals
            Dim lbl_uom As Label
            Dim txt_desc As TextBox
            Dim lbl_desc As Label
            Dim lbl_limit As Label

            txt_desc = e.Item.FindControl("txt_desc")
            lbl_uom = e.Item.FindControl("lbl_uom")
            lbl_desc = e.Item.FindControl("lbl_desc")
            ddl_uom = e.Item.FindControl("ddl_uom")
            txt_desc.Text = Common.parseNull(dv("RD_Product_Desc"))

            If dv("RD_Product_Code") = "99999" Then ' free form
                txt_desc.Visible = True
                lbl_desc.Visible = False
                ddl_uom.Visible = True
                lbl_uom.Visible = False

                ddl_uom.Attributes.Add("Onchange", "check();")
                objGlobal.FillCodeTable(ddl_uom, CodeTable.Uom)

                Common.SelDdl(objrfq.get_UOMcode(dv("RD_UOM"), "UOM"), ddl_uom, True, True)
                lbl_uom.Text = "1"
            Else ' cat   
                lbl_uom.Visible = True
                ddl_uom.Visible = False
                txt_desc.Visible = False
                lbl_desc.Visible = True

                lbl_uom.Text = Common.parseNull(dv("RD_UOM"))
                lbl_desc.Text = Common.parseNull(dv("RD_Product_Desc"))
                aryProdCode.Add(New String() {Common.parseNull(dv("RD_Product_Code")), Common.parseNull(dv("RD_Product_Desc"))})
            End If

            Dim txt_delivery As TextBox
            txt_delivery = e.Item.FindControl("txt_delivery")
            txt_delivery.Text = Common.parseNull(dv("RD_Delivery_Lead_Time"))

            Dim val_delivery As RegularExpressionValidator
            val_delivery = e.Item.FindControl("val_delivery")
            val_delivery.ControlToValidate = "txt_delivery"
            val_delivery.ValidationExpression = "^\d+$"
            val_delivery.ErrorMessage = CStr(intRow + 1) & ". Delivery Lead Time is expecting numeric value"
            val_delivery.Text = "?"
            val_delivery.Display = ValidatorDisplay.Dynamic


            'Michelle (15/9/2011) - Issue 871
            Dim txt_warranty As TextBox
            txt_warranty = e.Item.FindControl("txt_warranty")
            txt_warranty.Text = Common.parseNull(dv("RD_Warranty_Terms"))

            Dim val_warranty As RegularExpressionValidator
            val_warranty = e.Item.FindControl("val_warranty")
            val_warranty.ControlToValidate = "txt_warranty"
            val_warranty.ValidationExpression = "^\d+$"
            val_warranty.ErrorMessage = CStr(intRow + 1) & ". Warranty Terms is expecting numeric value"
            val_warranty.Text = "?"
            val_warranty.Display = ValidatorDisplay.Dynamic

            Dim reqVal_delivery As RequiredFieldValidator
            reqVal_delivery = e.Item.FindControl("reqVal_delivery")
            reqVal_delivery.ControlToValidate = "txt_delivery"
            reqVal_delivery.ErrorMessage = CStr(intRow + 1) & ". Require EDD"
            reqVal_delivery.Text = "?"
            reqVal_delivery.Display = ValidatorDisplay.Dynamic

            If Session("strItem") IsNot Nothing Then
                strItem = Session("strItem")
                If (CInt(intRow)) <= strItem.Count - 1 Then
                    txt_qty.Text = strItem(CInt(intRow))(0)
                    txt_delivery.Text = strItem(CInt(intRow))(1)
                    txt_warranty.Text = strItem(CInt(intRow))(2)
                End If
            End If

            intRow = intRow + 1
            If ViewState("modePR") = "pr" Then
                e.Item.Cells(ItemEnum.QTY).Enabled = False
                e.Item.Cells(ItemEnum.UOM).Enabled = False
                e.Item.Cells(ItemEnum.Time).Enabled = False
                e.Item.Cells(ItemEnum.Warranty).Enabled = False
            End If
        End If
    End Sub
    Private Function validateDatagrid(ByRef strMsg As String, ByVal text As String, ByVal no As String, ByRef qid As TextBox) As Boolean
        validateDatagrid = True

        Dim txtQ As TextBox
        Dim dr As TableRow

        If Not Common.checkMaxLength(text, 400) Then
            strMsg &= "<li>Item Description No." & no & " is over limit.</li>"
            qid.Text = "?"
            validateDatagrid = False
        Else
            qid.Text = ""
        End If
    End Function

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        Dim strMsg As String
        lblMsg.Text = ""

        If Page.IsValid And validateInputs(strMsg) Then
            save_data("")

        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If

    End Sub

    Private Function validateInputs(ByRef strMsg As String) As Boolean
        Dim txtQ As TextBox

        If Trim(txt_name.Text) = "" Or Len(txt_name.Text) < 1 Then
            txtQ = Me.FindControl("txtQ")
            txtQ.Text = "?"
            strMsg = "<ul type='disc'>"
            strMsg &= "<li>RFQ Description is required.<ul type='disc'></ul></li>"
            strMsg &= "</ul>"
            Return False
        End If

        If Not Me.txt_name.Text = "" AndAlso Not IsValidInput(Trim(Me.txt_name.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your RFQ Description.", MsgBoxStyle.Information)
            Return False
        End If

        'Michelle (3/5/2011) - to cater for those user who copy and paste description that are more than the maximum lenght
        If Me.txt_name.Text.Length > Me.txt_name.MaxLength Then
            Common.NetMsgbox(Me, "RFQ Description should be less than " & Me.txt_name.MaxLength & " characters.", MsgBoxStyle.Information)
            Return False
        End If

        If Not Me.txt_remark.Text = "" AndAlso Not IsValidInputEnter(Trim(Me.txt_remark.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your remarks.", MsgBoxStyle.Information)
            Return False
        End If

        If Me.txt_remark.Text.Length > Me.txt_remark.MaxLength Then
            Common.NetMsgbox(Me, "RFQ Remark should be less than " & Me.txt_remark.MaxLength & " characters.", MsgBoxStyle.Information)
            Return False
        End If

        Return True
    End Function

    Sub save_data(ByVal strFrm As String)
        Dim dr(row1) As TableRow
        Dim i, j As Integer
        Dim myStr As String
        Dim MyValue(100) As String
        Dim count As Integer
        Dim rfq_num As String
        Dim objval As New RFQ_User
        Dim rfq_id As String
        Dim rfq_option As Integer
        Dim dgItem As DataGridItem
        Dim strVendorList As String = ""
        Dim dtList As DataTable
        Dim dtDetails As DataTable

        dtList = Session("VendorList")
        dtDetails = Session("VendorListDetails")

        rfq_num = Session("rfq_num")
        rfq_id = Session("rfq_id")

        Dim value(50) As String
        Dim comfirmstatus As String

        'To get the RFQ items
        Dim dt As New DataTable
        dt = bindRFQDetail()

        For j = 0 To 4
            MyValue(j) = ""
        Next
        j = j - 1
        value(0) = txt_name.Text.Trim
        MyValue(j + 3) = "1"
        MyValue(j + 4) = "0"
        value(1) = Me.txt_exp.Text
        value(2) = Me.txt_remark.Text
        value(3) = objRFQ.GET_BUYERNAME()
        value(4) = Date.Now
        value(5) = ddl_cur.SelectedItem.Value
        If opt_RFQ_option.Enabled = True Then
            value(14) = opt_RFQ_option.SelectedItem.Value
        Else
            value(14) = 2
        End If
        Session("RFQ_Name") = txt_name.Text
        Session("RFQ_Cur") = ddl_cur.SelectedItem.Value
        value(6) = Me.ddl_pt.SelectedItem.Value
        value(7) = Me.ddl_pm.SelectedItem.Value
        value(8) = Me.ddl_st.SelectedItem.Value
        value(9) = Me.ddl_sm.SelectedItem.Value
        value(10) = Me.txt_cont_person.Text
        value(11) = Me.txt_num.Text
        value(12) = Me.txt_email.Text
        value(13) = Me.txt_validity.Text
        value(15) = 3
        value(16) = 0


        'To get the selected Vendors
        For Each dgItem In Me.dtg_vendor.Items
            If strVendorList = "" Then
                'strVendorList = "'" & dgItem.Cells(RFQ2Enum.CoyID).Text & "'"
                strVendorList = "'" & dgItem.Cells(0).Text & "'"
            Else
                'strVendorList = strVendorList & ", '" & dgItem.Cells(RFQ2Enum.CoyID).Text & "'"
                strVendorList = strVendorList & ", '" & dgItem.Cells(0).Text & "'"
            End If
        Next

        If strFrm <> "Submit" Then
            comfirmstatus = objRFQ.save_RFQ(strVendorList, value, rfq_num, rfq_id, dt, dtList, dtDetails, False)
        Else
            comfirmstatus = objRFQ.save_RFQ(strVendorList, value, rfq_num, rfq_id, dt, dtList, dtDetails, True)
        End If
        Session("rfq_num") = rfq_num
        Session("rfq_id") = rfq_id
        ViewState("save") = "1"

checkMsg:
        'If comfirmstatus = "2" Then
        '    Common.NetMsgbox(Me, "RFQ Name Exist", MsgBoxStyle.Information)
        '    Exit Sub
        'Else
        If comfirmstatus = "6" Then
            Common.NetMsgbox(Me, "Duplicate transaction number found. Please contact your Administrator to rectify the problem.", MsgBoxStyle.Information)
            Exit Sub
        End If


        If opt_RFQ_option.Enabled = True Then
            rfq_option = opt_RFQ_option.SelectedItem.Value
            objRFQ.update_option(Session("rfq_id"), rfq_option)
        Else
            rfq_option = opt_RFQ_option.SelectedItem.Value
            objRFQ.update_option(Session("rfq_id"), rfq_option)
        End If


        Me.lbl_rfq_number.Text = Session("rfq_num")
        Session("edit") = "1"
        ViewState("rfq_option_check") = "1"

        Me.lbl_cur.Visible = True
        ViewState("rfqname") = txt_name.Text
        Me.lbl_cur.Text = ddl_cur.SelectedItem.Text
        Session("RFQ_Cur_text") = ddl_cur.SelectedItem.Text
        'Me.txt_name.Visible = False
        Me.ddl_cur.Visible = False
        objRFQ.delete_TempVenListnItem() 'Delete those temp records created 

        If strFrm <> "Submit" Then
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            Bindgrid()
        End If

    End Sub
    Protected Sub cmd_Submit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Submit.Click
        Dim strMsg As String
        lblMsg.Text = ""
        Dim intMsg As Integer
        Dim intVendCnt, intItemCnt As Integer

        If Page.IsValid And validateInputs(strMsg) Then
            'Check whether vendor is selected
            intVendCnt = dtg_vendor.Items.Count
            intItemCnt = dgviewitem.Items.Count

            If intVendCnt = 0 Then
                Common.NetMsgbox(Me, "Please select at least 1 Vendor.", MsgBoxStyle.Information)
            ElseIf intItemCnt = 0 Then
                Common.NetMsgbox(Me, "Please enter at least 1 item.", MsgBoxStyle.Information)
            Else
                'If objRFQ.Vendor_send("", Session("rfq_num"), Session("rfq_id")) = "2" Then
                '    Common.NetMsgbox(Me, Session("rfq_num") & " has already been sent to the selected Vendors.", MsgBoxStyle.Information, "Wheel")
                'Else
                save_data("Submit")
                If objRFQ.Vendor_send("", Session("rfq_num"), Session("rfq_id")) = "2" Then
                    Common.NetMsgbox(Me, Session("rfq_num") & " has already been sent to the selected Vendor(s).", MsgBoxStyle.Information, "Wheel")
                Else
                    Common.NetMsgbox(Me, Session("rfq_num") & " has been successfully sent to the selected Vendor(s).", MsgBoxStyle.Information, "Wheel")
                    cmd_View.Visible = True
                    cmd_save.Visible = False
                    cmd_Submit.Visible = False
                    cmd_addSV.Visible = False
                    cmd_addcath.Visible = False
                    cmd_delete.Visible = False
                    cmd_upload.Visible = False
                    Me.cmd_View.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRFQ.aspx", "VendorRequired=F&BCoyID=" & Session("CompanyID") & "&RFQ_Num=" & Session("rfq_num")) & "')")

                End If

            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub
    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        Dim dgItem As DataGridItem
        Dim txtPrdDesc As TextBox
        Dim objrfq As New RFQ
        Dim objShopping As New ShoppingCart
        Dim LINE As Integer
        Dim chkItem As CheckBox
        For Each dgItem In dgviewitem.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                txtPrdDesc = dgItem.FindControl("txt_desc")
                'If Session("rfq_id") <> "" And dgItem.Cells(8).Text = "A" Then
                ' Added new column should change the column number
                If Session("rfq_id") <> "" And dgItem.Cells(9).Text = "A" Then
                    'If dgItem.Cells(5).Text = " " Then
                    '    objrfq.Delete_item("actual", Session("rfq_id"), dgItem.Cells(5).Text, txtPrdDesc.Text)
                    'Else
                    '    objrfq.Delete_item("actual", Session("rfq_id"), dgItem.Cells(5).Text)
                    'End If
                    If dgItem.Cells(6).Text = " " Then
                        objrfq.Delete_item("actual", Session("rfq_id"), dgItem.Cells(6).Text, txtPrdDesc.Text)
                    Else
                        objrfq.Delete_item("actual", Session("rfq_id"), dgItem.Cells(6).Text)
                    End If

                Else
                    objrfq.Delete_item("temp", txtPrdDesc.Text)
                End If
                Session("RFQProdList") = objShopping.RemoveProductCodeFromList(dgItem.Cells(5).Text, Session("RFQProdList"))
            End If

        Next

        'Dim dtrd As DataRow
        Dim txt_qty, txt_delivery, txt_warranty As TextBox
        Dim strItem As New ArrayList()
        'Dim dgItem As DataGridItem

        'Dim chkItem As CheckBox

        For Each dgItem In dgviewitem.Items
            txt_qty = dgItem.FindControl("txt_qty")
            txt_delivery = dgItem.FindControl("txt_delivery")
            txt_warranty = dgItem.FindControl("txt_warranty")

            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
            Else
                strItem.Add(New String() {txt_qty.Text, txt_delivery.Text, txt_warranty.Text})
            End If
        Next
        Session("strItem") = strItem

        Bindgrid(True)
    End Sub

    Private Function bindRFQDetail() As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dgItem As DataGridItem
        Dim i As Integer = 0
        Dim txtPrdDesc, txtQty, txtDelTime, txtWarTerm As TextBox
        Dim lblUOM As Label
        Dim ddlUOM As DropDownList
        Dim objDB As New EAD.DBCom

        dt.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dt.Columns.Add("UOM", Type.GetType("System.String"))
        'dt.Columns.Add("Qty", Type.GetType("System.Int32"))
        dt.Columns.Add("Qty", Type.GetType("System.Decimal"))
        dt.Columns.Add("DeliveryLeadTime", Type.GetType("System.Int32"))
        dt.Columns.Add("WarrantyTerm", Type.GetType("System.Int32"))
        dt.Columns.Add("ProductID", Type.GetType("System.String"))
        dt.Columns.Add("VIC", Type.GetType("System.String"))
        dt.Columns.Add("VCoyId", Type.GetType("System.String"))
        dt.Columns.Add("PR_LINE_INDEX", Type.GetType("System.String"))


        For Each dgItem In dgviewitem.Items
            i = i + 1
            txtPrdDesc = dgItem.FindControl("txt_desc")
            lblUOM = dgItem.FindControl("lbl_uom")
            txtQty = dgItem.FindControl("txt_qty")
            txtDelTime = dgItem.FindControl("txt_delivery")
            txtWarTerm = dgItem.FindControl("txt_warranty")

            dr = dt.NewRow
            dr("ProductDesc") = txtPrdDesc.Text
            dr("UOM") = lblUOM.Text
            'dr("Qty") = CInt(txtQty.Text)
            dr("Qty") = CDec(txtQty.Text)
            dr("DeliveryLeadTime") = CInt(txtDelTime.Text)
            If txtWarTerm.Text Is Nothing Or txtWarTerm.Text = "" Then
                txtWarTerm.Text = 0
            End If
            dr("WarrantyTerm") = CInt(txtWarTerm.Text)
            dr("ProductId") = dgItem.Cells(ItemEnum.ProdCode).Text
            'Michelle (27/4/2011) - To capture the item code
            '         dr("VIC") = dgItem.Cells(ItemEnum.VIC).Text
            dr("VIC") = objDB.Get1Column("PRODUCT_MSTR", "PM_VENDOR_ITEM_CODE", " WHERE PM_PRODUCT_CODE = '" & dgItem.Cells(ItemEnum.ProdCode).Text & "' ")
            dr("VCoyId") = dgItem.Cells(ItemEnum.CoyID).Text

            Dim rfq_num = Session("rfq_num")
            Dim rfq_id = Session("rfq_id")

            ' _Yap
            Dim strPR_Index As String
            If ViewState("modePR") = "pr" Then
                strPR_Index = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX,'') FROM RFQ_DETAIL WHERE RD_RFQ_LINE = '" & dgItem.Cells(10).Text & "' AND RD_RFQ_ID = '" & rfq_id & "' AND RD_COY_ID = '" & dr("VCoyId") & "' AND RD_PRODUCT_CODE = '" & dr("ProductId") & "' AND RD_VENDOR_ITEM_CODE = '" & Common.Parse(dr("VIC")) & "' AND RD_QUANTITY = '" & dr("Qty") & "' AND RD_PRODUCT_DESC = '" & Common.Parse(dr("ProductDesc")) & "' AND RD_UOM = '" & Common.Parse(dr("UOM")) & "' AND RD_DELIVERY_LEAD_TIME = '" & dr("DeliveryLeadTime") & "' AND RD_WARRANTY_TERMS = '" & dr("WarrantyTerm") & "' AND RD_PRODUCT_NAME = '" & Common.Parse(dr("ProductDesc")) & "' ORDER BY RD_RFQ_LINE LIMIT 1 ")
                If strPR_Index <> "" Then
                    dr("PR_LINE_INDEX") = strPR_Index
                End If
            Else
                dr("PR_LINE_INDEX") = ""
            End If
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    Private Sub cmd_addcath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_addcath.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("RFQ", "Add_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=" & Server.UrlEncode(Me.txt_name.Text) & "&RFQ_Cur_value=" & Me.ddl_cur.SelectedItem.Value & "&RFQ_Cur_Text=" & Me.ddl_cur.SelectedItem.Text)
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','700px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '              "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"
        Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" &
                     "<li><div class=""space""></div></li>" &
                   "</ul><div></div></div>"
    End Sub

    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        'Bindgrid_vendor()
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        'Dim dtrd As DataRow
        Dim txt_qty, txt_delivery, txt_warranty As TextBox
        Dim strItem As New ArrayList()
        Dim dgItem As DataGridItem

        Dim chkItem As CheckBox

        For Each dgItem In dgviewitem.Items
            txt_qty = dgItem.FindControl("txt_qty")
            txt_delivery = dgItem.FindControl("txt_delivery")
            txt_warranty = dgItem.FindControl("txt_warranty")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text})
            'End If
            strItem.Add(New String() {txt_qty.Text, txt_delivery.Text, txt_warranty.Text})
        Next
        Session("strItem") = strItem

        Bindgrid()
    End Sub

    Public Sub dtg_vendor_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_vendor.CurrentPageIndex = e.NewPageIndex
        imageid = 0
        Bindgrid_vendor(0)
    End Sub

    Private Sub cmd_addSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_addSV.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("RFQ", "VendorList.aspx", "edit=0&RFQ_No=" & Trim(Session("RFQ_num")) & "&RFQ_list=RFQName&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','510px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    'Protected Sub cmd_View_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_View.Click
    '    previewRFQ()
    'End Sub

    'Private Sub PreviewQuotation()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim objFile As New FileManagement
    '    Dim strImgSrc As String

    '    strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try

    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            '.CommandText = "SELECT (SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
    '            '            & "(CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)) AS CMState, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
    '            '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS SupplierAddrState, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
    '            '            & "AS SupplierAddrCtry, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) AND (CODE_CATEGORY = 'pt')) " _
    '            '            & "AS PaymentTerm, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
    '            '            & "AS PaymentMethod, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) AND (CODE_CATEGORY = 'St')) " _
    '            '            & "AS Ship_Term, " _
    '            '            & "(SELECT   CODE_DESC " _
    '            '            & "FROM      CODE_MSTR AS a " _
    '            '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) AND (CODE_CATEGORY = 'sm')) " _
    '            '            & "AS Ship_Mode, COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, " _
    '            '            & "COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
    '            '            & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, " _
    '            '            & "COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
    '            '            & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, " _
    '            '            & "COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
    '            '            & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
    '            '            & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
    '            '            & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
    '            '            & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, " _
    '            '            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
    '            '            & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, " _
    '            '            & "COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, " _
    '            '            & "RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, " _
    '            '            & "RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
    '            '            & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, " _
    '            '            & "RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, " _
    '            '            & "RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, " _
    '            '            & "RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, RFQ_MSTR.RM_RFQ_OPTION, " _
    '            '            & "RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Offer_Till, RFQ_REPLIES_MSTR.RRM_ETD, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Ship_Term, RFQ_REPLIES_MSTR.RRM_Created_On, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_Status, RFQ_REPLIES_MSTR.RRM_B_Display_Status, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_V_Display_Status, RFQ_REPLIES_MSTR.RRM_Indicator, " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_Product_Code, RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_Quantity, RFQ_REPLIES_DETAIL.RRD_Unit_Price, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_GST_Desc, RFQ_REPLIES_DETAIL.RRD_Product_Desc, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_UOM, RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, RFQ_REPLIES_DETAIL.RRD_Remarks, " _
    '            '            & "RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID, " _
    '            '            & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, " _
    '            '            & "COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
    '            '            & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, " _
    '            '            & "COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
    '            '            & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
    '            '            & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
    '            '            & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
    '            '            & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, " _
    '            '            & "COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
    '            '            & "FROM      RFQ_MSTR INNER JOIN " _
    '            '            & "RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID INNER JOIN " _
    '            '            & "RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID AND " _
    '            '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id INNER JOIN " _
    '            '            & "COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
    '            '            & "COMPANY_MSTR AS COMPANY_MSTR_1 ON " _
    '            '            & "RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
    '            '            & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
    '            '            & "(RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND " _
    '            '            & "(RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
    '            .CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState,d.CODE_DESC AS SupplierAddrCtry," _
    '                   & "e.CODE_DESC AS PaymentTerm,f.CODE_DESC AS PaymentMethod,g.CODE_DESC AS Ship_Term,h.CODE_DESC AS Ship_Mode," _
    '                   & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
    '                   & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
    '                   & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
    '                   & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
    '                   & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
    '                   & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
    '                   & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
    '                   & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
    '                   & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
    '                   & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
    '                   & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
    '                   & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
    '                   & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
    '                   & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
    '                   & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
    '                   & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
    '                   & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
    '                   & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
    '                   & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
    '                   & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
    '                   & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
    '                   & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
    '                   & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
    '                   & "FROM RFQ_MSTR " _
    '                   & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
    '                   & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
    '                   & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
    '                   & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
    '                   & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
    '                   & "INNER JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
    '                   & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
    '                   & "INNER JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
    '                   & "AND (b.CODE_CATEGORY = 'ct') " _
    '                   & "INNER JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
    '                   & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
    '                   & "INNER JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
    '                   & "AND (d.CODE_CATEGORY = 'ct') " _
    '                   & "INNER JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
    '                   & "AND (e.CODE_CATEGORY = 'pt') " _
    '                   & "INNER JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
    '                   & "AND (f.CODE_CATEGORY = 'pm') " _
    '                   & "INNER JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
    '                   & "AND (g.CODE_CATEGORY = 'St') " _
    '                   & "INNER JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
    '                   & "AND (h.CODE_CATEGORY = 'sm') " _
    '                   & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
    '                   & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request(Trim("BCoyID"))))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request(Trim("PO_No"))))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmQuoNum", Request(Trim("BCoyID"))))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Request(Trim("PO_No"))))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPO_DataSetPreviewPO", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "PO\PreviewPO-FTN.rdlc"
    '        localreport.EnableExternalImages = True

    '        Dim I As Byte
    '        Dim GetParameter As String = ""
    '        Dim TotalParameter As Byte
    '        TotalParameter = localreport.GetParameters.Count
    '        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        'Dim paramlist As New Generic.List(Of ReportParameter)
    '        For I = 0 To localreport.GetParameters.Count - 1
    '            GetParameter = localreport.GetParameters.Item(I).Name
    '            Select Case LCase(GetParameter)
    '                Case "par1"
    '                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '     "<DeviceInfo>" + _
    '        '         "  <OutputFormat>EMF</OutputFormat>" + _
    '        '         "  <PageWidth>8.27in</PageWidth>" + _
    '        '         "  <PageHeight>11in</PageHeight>" + _
    '        '         "  <MarginTop>0.25in</MarginTop>" + _
    '        '         "  <MarginLeft>0.25in</MarginLeft>" + _
    '        '         "  <MarginRight>0.25in</MarginRight>" + _
    '        '         "  <MarginBottom>0.25in</MarginBottom>" + _
    '        '         "</DeviceInfo>"
    '        Dim deviceInfo As String = _
    '           "<DeviceInfo>" + _
    '               "  <OutputFormat>EMF</OutputFormat>" + _
    '               "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "PO\POReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('POReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    Private Sub BindPreDefinedVendor()
        Dim dv As DataSet
        Dim cbolist As New ListItem
        Dim objRFQ As New RFQ

        cboVendor.Items.Clear()
        dv = objRFQ.GetPredefinedVendor()

        If Not dv Is Nothing Then
            cboVendor.Enabled = True
            Common.FillDdl(cboVendor, "RVDLM_LIST_NAME", "RVDLM_LIST_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboVendor.Items.Insert(0, cbolist)
    End Sub

    'Private Sub previewRFQ()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim objFile As New FileManagement
    '    Dim strImgSrc As String

    '    strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try
    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
    '                        & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
    '                        & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
    '                        & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
    '                        & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
    '                        & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
    '                        & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
    '                        & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
    '                        & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
    '                        & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
    '                        & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
    '                        & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
    '                        & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
    '                        & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
    '                        & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
    '                        & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
    '                        & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
    '                        & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
    '                        & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
    '                        & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
    '                        & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
    '                        & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
    '                        & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
    '                        & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
    '                        & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
    '                        & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
    '                        & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
    '                        & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
    '                        & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
    '                        & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
    '                        & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
    '                        & "COMPANY_MSTR.CM_BA_CANCEL, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
    '                        & "AS SupplierAddrCtry, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
    '                        & "AS RFQ_PaymentTerm, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
    '                        & "AS RFQ_PaymentMethod, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
    '                        & "AS RFQ_ShipmentTerm, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
    '                        & "AS RFQ_ShipmentMode, " _
    '                        & "(SELECT   CM_BUSINESS_REG_NO " _
    '                        & "FROM      COMPANY_MSTR AS B " _
    '                        & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
    '                        & "FROM      RFQ_MSTR INNER JOIN " _
    '                        & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
    '                        & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
    '                        & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
    '                        & "WHERE   (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID =@prmBCoyID)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Session("CompanyID")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Session("rfq_num")))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_Buyer_DataTablePreviewRFQ1", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "RFQ\PreviewRFQ-Buyer.rdlc"
    '        localreport.EnableExternalImages = True

    '        Dim I As Byte
    '        Dim GetParameter As String = ""
    '        Dim TotalParameter As Byte
    '        TotalParameter = localreport.GetParameters.Count
    '        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        'Dim paramlist As New Generic.List(Of ReportParameter)
    '        For I = 0 To localreport.GetParameters.Count - 1
    '            GetParameter = localreport.GetParameters.Item(I).Name
    '            Select Case LCase(GetParameter)
    '                Case "par1"
    '                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        localreport.Refresh()

    '        Dim deviceInfo As String = _
    '             "<DeviceInfo>" + _
    '                 "  <OutputFormat>EMF</OutputFormat>" + _
    '                 "  <PageWidth>8.27in</PageWidth>" + _
    '                 "  <PageHeight>11in</PageHeight>" + _
    '                 "  <MarginTop>0.25in</MarginTop>" + _
    '                 "  <MarginLeft>0.25in</MarginLeft>" + _
    '                 "  <MarginRight>0.25in</MarginRight>" + _
    '                 "  <MarginBottom>0.25in</MarginBottom>" + _
    '                 "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "RFQ\BuyerRFQReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('BuyerRFQReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    'Private Sub previewRFQ()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim objFile As New FileManagement
    '    Dim strImgSrc As String

    '    strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try
    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
    '                        & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
    '                        & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
    '                        & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
    '                        & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
    '                        & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
    '                        & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
    '                        & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
    '                        & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
    '                        & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
    '                        & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
    '                        & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
    '                        & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
    '                        & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
    '                        & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
    '                        & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
    '                        & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
    '                        & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
    '                        & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
    '                        & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
    '                        & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
    '                        & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
    '                        & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
    '                        & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
    '                        & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
    '                        & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
    '                        & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
    '                        & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
    '                        & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
    '                        & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
    '                        & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
    '                        & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
    '                        & "COMPANY_MSTR.CM_BA_CANCEL, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
    '                        & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
    '                        & "AS SupplierAddrCtry, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
    '                        & "AS RFQ_PaymentTerm, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
    '                        & "AS RFQ_PaymentMethod, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
    '                        & "AS RFQ_ShipmentTerm, " _
    '                        & "(SELECT   CODE_DESC " _
    '                        & "FROM      CODE_MSTR AS a " _
    '                        & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
    '                        & "AS RFQ_ShipmentMode, " _
    '                        & "(SELECT   CM_BUSINESS_REG_NO " _
    '                        & "FROM      COMPANY_MSTR AS B " _
    '                        & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
    '                        & "FROM      RFQ_MSTR INNER JOIN " _
    '                        & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
    '                        & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
    '                        & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
    '                        & "WHERE   (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID =@prmBCoyID)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Session("CompanyID")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Session("rfq_num")))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_FTN_DataTablePreviewRFQ", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "RFQ\PreviewRFQ-FTN2.rdlc"
    '        localreport.EnableExternalImages = True

    '        Dim I As Byte
    '        Dim GetParameter As String = ""
    '        Dim TotalParameter As Byte
    '        TotalParameter = localreport.GetParameters.Count
    '        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        'Dim paramlist As New Generic.List(Of ReportParameter)
    '        For I = 0 To localreport.GetParameters.Count - 1
    '            GetParameter = localreport.GetParameters.Item(I).Name
    '            Select Case LCase(GetParameter)
    '                Case "par1"
    '                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '     "<DeviceInfo>" + _
    '        '         "  <OutputFormat>EMF</OutputFormat>" + _
    '        '         "  <PageWidth>8.27in</PageWidth>" + _
    '        '         "  <PageHeight>11in</PageHeight>" + _
    '        '         "  <MarginTop>0.25in</MarginTop>" + _
    '        '         "  <MarginLeft>0.25in</MarginLeft>" + _
    '        '         "  <MarginRight>0.25in</MarginRight>" + _
    '        '         "  <MarginBottom>0.25in</MarginBottom>" + _
    '        '         "</DeviceInfo>"
    '        Dim deviceInfo As String = _
    '           "<DeviceInfo>" + _
    '               "  <OutputFormat>EMF</OutputFormat>" + _
    '               "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "RFQ\BuyerRFQReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('BuyerRFQReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    'Private Sub ddlVendorList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVendorList.SelectedIndexChanged
    '    Dim ds As DataSet
    '    ds = 
    '    Common.FillDdl(ddlVendorList, , , ds, "--Select--")
    'End Sub

    'Private Sub cboVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboVendor.SelectedIndexChanged
    '    If Me.cboVendor.SelectedItem.Value <> "" Then
    '        Dim dgItem As DataGridItem
    '        Dim objrfq As New RFQ
    '        'Dim objval As New RFQ_User
    '        Dim objDB As New EAD.DBCom
    '        Dim strAryQuery(0) As String
    '        Dim strSQL As String
    '        Dim txtPrice As TextBox
    '        Dim lblPrice As Label
    '        Dim chkItem As CheckBox
    '        Dim count As Integer
    '        '  rfq_name = Me.SESSION("RFQ_name")
    '        Dim cur As String = Request.QueryString("RFQ_Cur_value")
    '        Dim i As Integer = 0
    '        Dim objGLO As New AppGlobals
    '        Dim VCnt As Integer = 0
    '        Dim PVCnt As Integer = 0

    '        For PVCnt = 0 To aryPreVendorNew.Count - 1
    '            If ds.Tables(0).Rows(i)("RCDLD_V_Coy_ID") = aryPreVendorNew.Item(PVCnt) Then
    '                Common.NetMsgbox(Me, objGLO.GetErrorMessage("00027"), MsgBoxStyle.Information)
    '                Exit Sub
    '            Else
    '                aryPreVendorNew.Add()
    '            End If

    '        Next
    '        'objval.dis_ID = Me.cboVendor.SelectedItem.Value  'objrfq.Vendor_AddDistMstr(objval)
    '        'objval.RFQ_Name = "" ' Me.lbl_name.Text
    '        'objval.RFQ_ID = Session("rfq_id")

    '        Dim ds As New DataSet
    '        Dim objiinv As New Invoice
    '        'ds = objiinv.get_venlist(Me.lbl_name.Text, Me.cboVendor.SelectedItem.Value)
    '        ds = objiinv.get_venlist("", Me.cboVendor.SelectedItem.Value)

    '        For i = 0 To ds.Tables(0).Rows.Count - 1
    '            'objval.V_com_ID = ds.Tables(0).Rows(i)("RCDLD_V_Coy_ID")
    '            'strSQL = objrfq.Vendor_AddList(objval)


    '            For VCnt = 0 To aryVendor.Count - 1
    '                If ds.Tables(0).Rows(i)("RCDLD_V_Coy_ID") = aryVendor.Item(VCnt) Then
    '                    count = count + 1
    '                End If

    '            Next
    '        Next
    '        If ds.Tables(0).Rows.Count = count Then
    '            If ds.Tables(0).Rows.Count = 0 Then
    '                Common.NetMsgbox(Me, objGLO.GetErrorMessage("00028"), MsgBoxStyle.Information)
    '            Else
    '                Common.NetMsgbox(Me, objGLO.GetErrorMessage("00027"), MsgBoxStyle.Information)
    '            End If

    '        ElseIf strSQL = "" Then
    '            Common.NetMsgbox(Me, objGLO.GetErrorMessage("00028"), MsgBoxStyle.Information)
    '        Else
    '            If objDB.BatchExecute(strAryQuery) Then
    '                objrfq.Vendor_AddListMstr(objval)

    '                objrfq.Vendor_Add_Inv_Ven_List(objval)
    '                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    '            End If

    '        End If
    '        imageid = 0
    '        Bindgrid_vendor()
    '    End If
    'End Sub

    Private Sub cboVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboVendor.SelectedIndexChanged
        If Me.cboVendor.SelectedItem.Value <> "" Then
            Dim dgItem As DataGridItem
            Dim objrfq As New RFQ
            'Dim objval As New RFQ_User
            Dim objDB As New EAD.DBCom
            Dim strAryQuery(0) As String
            Dim strSQL As String
            Dim txtPrice As TextBox
            Dim lblPrice As Label
            Dim chkItem As CheckBox
            Dim count As Integer
            '  rfq_name = Me.SESSION("RFQ_name")
            Dim cur As String = Request.QueryString("RFQ_Cur_value")
            Dim i As Integer = 0
            Dim objGLO As New AppGlobals
            Dim VCnt As Integer = 0
            Dim ds As New DataSet
            Dim dtTemp As DataTable
            Dim objiinv As New Invoice
            Dim dtList As DataTable
            Dim dtDetails As DataTable
            Dim dtListTemp As New DataTable
            Dim dtDetailsTemp As New DataTable
            Dim dtrList As DataRow()
            Dim dtrDetails As DataRow()
            Dim dtrSpec As DataRow()
            Dim dtrSpecList As DataRow()
            Dim dtrNewList As DataRow
            Dim dtrNewDetails As DataRow
            Dim j As Integer
            Dim strSearch As String = ""
            Dim intFound As Integer = 0

            dtListTemp.Columns.Add("RVDLM_List_Name", Type.GetType("System.String"))
            dtListTemp.Columns.Add("RVDLM_List_Index", Type.GetType("System.String"))
            dtListTemp.Columns.Add("TYPE", Type.GetType("System.String"))
            dtListTemp.Columns.Add("Added", Type.GetType("System.String"))

            dtDetailsTemp.Columns.Add("CoyId", Type.GetType("System.String"))
            dtDetailsTemp.Columns.Add("RVDLM_List_Name", Type.GetType("System.String"))
            dtDetailsTemp.Columns.Add("RVDLM_List_Index", Type.GetType("System.String"))
            dtDetailsTemp.Columns.Add("TYPE", Type.GetType("System.String"))
            dtDetailsTemp.Columns.Add("Added", Type.GetType("System.String"))

            count = 0
            intFound = 0
            dtList = Session("VendorList")
            dtDetails = Session("VendorListDetails")

            ds = objiinv.get_venlist("", Me.cboVendor.SelectedItem.Value)
            dtTemp = ds.Tables(0)
            If dtTemp.Rows.Count = 0 Then
                Common.NetMsgbox(Me, objGLO.GetErrorMessage("00028"), MsgBoxStyle.Information)
                Exit Sub
            End If

            For i = 0 To dtTemp.Rows.Count - 1
                strSearch = "RVDLM_List_Index='" & Me.cboVendor.SelectedItem.Value & "' AND TYPE = 'list'"
                dtrList = dtList.Select(strSearch)
                If dtrList.Length > 0 Then  'If found
                    strSearch = "RVDLM_List_Index='" & Me.cboVendor.SelectedItem.Value & "' AND TYPE = 'list' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                    dtrDetails = dtDetails.Select(strSearch)
                    If dtrDetails.Length > 0 Then
                        intFound = intFound + 1

                    Else
                        strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = 'specific'"
                        dtrSpecList = dtList.Select(strSearch)
                        If dtrSpecList.Length > 0 Then  'if specific vendor found
                            'intFound = intFound + 1
                            For Each oRow As DataRow In dtrSpecList
                                dtList.Rows.Remove(oRow)
                            Next

                            strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = 'specific' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                            dtrSpec = dtDetails.Select(strSearch)
                            If dtrSpec.Length > 0 Then
                                For Each oRow As DataRow In dtrSpec
                                    dtDetails.Rows.Remove(oRow)
                                Next
                            End If

                            dtrNewDetails = dtDetailsTemp.NewRow
                            dtrNewDetails("CoyId") = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                            dtrNewDetails("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                            dtrNewDetails("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                            dtrNewDetails("TYPE") = "list"
                            dtrNewDetails("Added") = "Y"
                            dtDetailsTemp.Rows.Add(dtrNewDetails)

                        Else 'if specific vendor not found
                            dtrNewDetails = dtDetailsTemp.NewRow
                            dtrNewDetails("CoyId") = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                            dtrNewDetails("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                            dtrNewDetails("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                            dtrNewDetails("TYPE") = "list"
                            dtrNewDetails("Added") = "Y"
                            dtDetailsTemp.Rows.Add(dtrNewDetails)
                        End If
                    End If

                Else    'If list not found
                    strSearch = "TYPE = 'list' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                    dtrDetails = dtDetails.Select(strSearch)
                    If dtrDetails.Length > 0 Then
                        intFound = intFound + 1

                    Else
                        strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = 'specific'"
                        dtrSpecList = dtList.Select(strSearch)
                        If dtrSpecList.Length > 0 Then  'if specific vendor found
                            'intFound = intFound + 1
                            For Each oRow As DataRow In dtrSpecList
                                dtList.Rows.Remove(oRow)
                            Next

                            strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = 'specific' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                            dtrSpec = dtDetails.Select(strSearch)
                            If dtrSpec.Length > 0 Then
                                For Each oRow As DataRow In dtrSpec
                                    dtDetails.Rows.Remove(oRow)
                                Next
                            End If

                            dtrNewDetails = dtDetailsTemp.NewRow
                            dtrNewDetails("CoyId") = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                            dtrNewDetails("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                            dtrNewDetails("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                            dtrNewDetails("TYPE") = "list"
                            dtrNewDetails("Added") = "Y"
                            dtDetailsTemp.Rows.Add(dtrNewDetails)

                            If count = 0 Then
                                dtrNewList = dtListTemp.NewRow
                                dtrNewList("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                                dtrNewList("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                                dtrNewList("TYPE") = "list"
                                dtrNewList("Added") = "Y"
                                dtListTemp.Rows.Add(dtrNewList)
                            End If
                            count = count + 1

                        Else 'if specific vendor not found
                            dtrNewDetails = dtDetailsTemp.NewRow
                            dtrNewDetails("CoyId") = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                            dtrNewDetails("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                            dtrNewDetails("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                            dtrNewDetails("TYPE") = "list"
                            dtrNewDetails("Added") = "Y"
                            dtDetailsTemp.Rows.Add(dtrNewDetails)

                            If count = 0 Then
                                dtrNewList = dtListTemp.NewRow
                                dtrNewList("RVDLM_List_Name") = Me.cboVendor.SelectedItem.Text
                                dtrNewList("RVDLM_List_Index") = Me.cboVendor.SelectedItem.Value
                                dtrNewList("TYPE") = "list"
                                dtrNewList("Added") = "Y"
                                dtListTemp.Rows.Add(dtrNewList)
                            End If
                            count = count + 1
                        End If
                    End If
                End If
                'Else    'No detail found
                '    strSearch = "RVDLM_List_Index=" & Me.cboVendor.SelectedItem.Value & " AND TYPE = 'list'"
                '    dtrList = dtList.Select(strSearch)
                '    If dtrList.Length > 0 Then  'If found
                '        strSearch = "RVDLM_List_Index=" & Me.cboVendor.SelectedItem.Value & " AND TYPE = 'list' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '        dtrDetails = dtDetails.Select(strSearch)
                '        If dtrDetails.Length > 0 Then


                '        Else
                '            strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = specific AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '            dtrSpec = dtDetails.Select(strSearch)

                '            If dtrSpec.Length > 0 Then


                '            Else
                '                dtrNew = dtDetailsTemp.NewRow
                '                dtrNew(0) = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                '                dtrNew(1) = Me.cboVendor.SelectedItem.Text
                '                dtrNew(2) = Me.cboVendor.SelectedItem.Value
                '                dtrNew(3) = "list"
                '                dtrNew(4) = "Y"
                '                dtDetailsTemp.Rows.Add(dtrNew)


                '            End If

                '        End If

                '    Else    'If not found
                '        strSearch = "RVDLM_List_Index=" & Me.cboVendor.SelectedItem.Value & " AND TYPE = list AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '        dtrDetails = dtDetails.Select(strSearch)
                '        If dtrDetails.Length > 0 Then


                '        Else
                '            strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = specific AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '            dtrSpec = dtDetails.Select(strSearch)

                '            If dtrSpec.Length > 0 Then


                '            Else
                '                dtrNew = dtDetailsTemp.NewRow
                '                dtrNew(0) = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                '                dtrNew(1) = Me.cboVendor.SelectedItem.Text
                '                dtrNew(2) = Me.cboVendor.SelectedItem.Value
                '                dtrNew(3) = "list"
                '                dtrNew(4) = "Y"
                '                dtDetailsTemp.Rows.Add(dtrNew)

                '                dtrNew = dtListTemp.NewRow
                '                dtrNew(0) = Me.cboVendor.SelectedItem.Text
                '                dtrNew(1) = Me.cboVendor.SelectedItem.Value
                '                dtrNew(2) = "list"
                '                dtrNew(3) = "Y"
                '                dtListTemp.Rows.Add(dtrNew)
                '            End If

                '        End If
                '    End If
                'End If

                'Else
                '    If dtDetails.Rows.Count > 0 Then
                '        strSearch = "RVDLM_List_Index=" & Me.cboVendor.SelectedItem.Value & " AND TYPE = 'list' AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '        dtrDetails = dtDetails.Select(strSearch)
                '        If dtrDetails.Length > 0 Then


                '        Else
                '            strSearch = "RVDLM_List_Index='" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "' AND TYPE = specific AND CoyId = '" & dtTemp.Rows(i).Item("RCDLD_V_Coy_ID") & "'"
                '            dtrSpec = dtDetails.Select(strSearch)

                '            If dtrSpec.Length > 0 Then


                '            Else
                '                dtrNew = dtDetailsTemp.NewRow
                '                dtrNew(0) = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                '                dtrNew(1) = Me.cboVendor.SelectedItem.Text
                '                dtrNew(2) = Me.cboVendor.SelectedItem.Value
                '                dtrNew(3) = "list"
                '                dtrNew(4) = "Y"
                '                dtDetailsTemp.Rows.Add(dtrNew)

                '                dtrNew = dtListTemp.NewRow
                '                dtrNew(0) = Me.cboVendor.SelectedItem.Text
                '                dtrNew(1) = Me.cboVendor.SelectedItem.Value
                '                dtrNew(2) = "list"
                '                dtrNew(3) = "Y"
                '                dtListTemp.Rows.Add(dtrNew)
                '            End If

                '        End If
                '    Else
                '        dtrNew = dtDetailsTemp.NewRow
                '        dtrNew(0) = dtTemp.Rows(i).Item("RCDLD_V_Coy_ID")
                '        dtrNew(1) = Me.cboVendor.SelectedItem.Text
                '        dtrNew(2) = Me.cboVendor.SelectedItem.Value
                '        dtrNew(3) = "list"
                '        dtrNew(4) = "Y"
                '        dtDetailsTemp.Rows.Add(dtrNew)

                '        If count = 0 Then
                '            dtrNew = dtListTemp.NewRow
                '            dtrNew(0) = Me.cboVendor.SelectedItem.Text
                '            dtrNew(1) = Me.cboVendor.SelectedItem.Value
                '            dtrNew(2) = "list"
                '            dtrNew(3) = "Y"
                '            dtListTemp.Rows.Add(dtrNew)
                '        End If
                '        count = count + 1
                '    End If
                'End If
            Next

            If dtTemp.Rows.Count = intFound Then
                If dtTemp.Rows.Count > 0 Then
                    Common.NetMsgbox(Me, objGLO.GetErrorMessage("00027"), MsgBoxStyle.Information)
                End If

            Else
                'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            End If

            dtList.Merge(dtListTemp)
            dtDetails.Merge(dtDetailsTemp)
            Session("VendorList") = dtList
            Session("VendorListDetails") = dtDetails
            imageid = 0
            Me.cboVendor.SelectedIndex = -1
            Bindgrid_vendor()
        End If
    End Sub
End Class
