Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.IO
Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WebForms

Public Class InvList
    Inherits AgoraLegacy.AppBaseClass
    Dim objDO As New Dashboard
    Dim strMsg As String = ""
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dtDocMatch As DataTable
    Dim objTrac As New Tracking
    Dim DS2 As New DataSet

    Public Enum EnumInvList
        icCheck = 0
        icInvOn = 1
        icPO = 2
        icDO = 3
        icGRN = 4
        icCoyName = 5
        icCurrCode = 6
        icRemainAmt = 7
        icPONoV = 8
        IcGRNV = 9
        icDOV = 10
        icMethod = 11
        icCoyId = 12
        icPOIndexV = 13
        icPayDay = 14
        icBalShipAmt = 15
    End Enum
    Public Enum EnumInv2
        icCheck
        icDoc
        icReference
        icRemarks
        icDetails
        icButton
        icInv
        icCurrency
        icAmount
        icTax
        icShipAmt
        icDoc1
        icMethod
        icPONoV
        IcGRNV
        icDOV
        icCoyId
        icPoIdx
        icPayDay
        icPayDay1
        icBalShipAmt
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Dim objinv As New Invoice
    Dim paid As Double
    Dim strMode As String
    Dim ordered_amount As Double
    Protected WithEvents dtg_InvList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_inv2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_createInv As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_submit As System.Web.UI.WebControls.Button
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents txt_DocNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents back_view As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor

    'Protected WithEvents txt_bcom As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblStep1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStep2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim strDONo, strVMode, strLocID, strGRNNo, strBCoyID, strPONo, strFrm, strtemp, strtemp2 As String
    Dim intPOIdx As Integer
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Protected WithEvents dtgInv As System.Web.UI.WebControls.DataGrid
    ' Dim strMode, strDONo, strLocID, strBCoyID, strPONo As String
    'Public Enum EnumPO
    '    icPONoLink
    '    icCreationDate
    '    icDueDate
    '    icCoName
    '    icTot
    '    icQty
    'End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_createInv.Enabled = False
        cmd_submit.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_submit)
        ' htPageAccess.Add("add", alButtonList)
        alButtonList.Add(cmd_createInv)
        htPageAccess.Add("add", alButtonList)
        If strMode = "step1" Then
            If intPageRecordCnt > 0 Then
                cmd_createInv.Enabled = blnCanAdd
            Else
                cmdReset.Disabled = True
                cmd_createInv.Enabled = False
            End If

        ElseIf strMode = "step2" Then
            cmd_submit.Enabled = blnCanAdd
        End If
        '  cmd_createInv.Enabled = blnCanAdd
        CheckButtonAccess()
        cmdReset.Disabled = Not (blnCanAdd)
        alButtonList.Clear()
        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmd_submit.Enabled = False
        End If

        If ViewState("Back") = "Back" Then
            cmd_submit.Visible = False
            cmdReset.Visible = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ViewState("line") = 0

        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        strFrm = Me.Request.QueryString("Frm")
        strVMode = Me.Request.QueryString("VMode")
        strPONo = Me.Request.QueryString("PONo")
        strDONo = Me.Request.QueryString("DONo")
        intPOIdx = Me.Request.QueryString("POIdx")
        strGRNNo = Me.Request.QueryString("GRNNo")
        strBCoyID = Me.Request.QueryString("BCoy")
        If strFrm = "Dashboard" Then
            back_view.HRef = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            back_view.Visible = True
        ElseIf strFrm = "invoiceView" Then
            back_view.HRef = dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId)
            back_view.Visible = True
        Else
            back_view.Visible = False
        End If

        If Not IsPostBack Then
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            'Bindgrid()
            'Me.lblTitle.Text = "Invoice Generation (Step 1)"
            GenerateTab()
            SetGridProperty(dtg_InvList)
            Me.lblStep1.Visible = True
            Me.lblStep2.Visible = False
            Me.back.Visible = False
            Me.cmdReset.Visible = False
            Session("strurl") = strCallFrom
            '//carol remark
            'cmdReset.Attributes.Add("onclick", "Reset_Step1();")
            cmdSearch_Click(sender, e)
        End If
        cmd_submit.Attributes.Add("style", "display:''")
        cmd_createInv.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        If ViewState("GSTCOD") = True Then
            cmd_createInv.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        End If
        'cmd_submit.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2');")
        'cmd_submit.Attributes.Add("onclick", "return checkAtLeastOneResetSummary('chkSelection2','',0,1);")


    End Sub

    Public Sub dtg_InvList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        ViewState("temprow") = ""
        ViewState("totalcount") = 0
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_InvList.CurrentPageIndex = 0
        ViewState("temprow") = ""
        ViewState("totalcount") = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objINV As New Invoice
        strMode = "step1"
        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strStatus As String = ""
        Dim NewInv As Integer = invStatus.NewInv
        Dim Approved As Integer = invStatus.Approved
        Dim Paid As Integer = invStatus.Paid
        Dim PendingAppr As Integer = invStatus.PendingAppr

        '-- start cons strstatus --
        'If Me.chk_New.Checked Then
        '    strStatus = IIf(strStatus = "", invStatus.NewInv, strStatus & "," & invStatus.NewInv)
        'End If
        'If Me.chK_Approved.Checked Then
        '    strStatus = IIf(strStatus = "", invStatus.Approved, strStatus & "," & invStatus.Approved)
        'End If
        'If Me.chk_paid.Checked Then
        '    strStatus = IIf(strStatus = "", invStatus.Paid, strStatus & "," & invStatus.Paid)
        'End If
        'If Me.chk_Pending.Checked Then
        '    strStatus = IIf(strStatus = "", invStatus.PendingAppr, strStatus & "," & invStatus.PendingAppr)
        'End If
        'If strStatus = "" Then
        '    strStatus = "" & NewInv & "," & Approved & ", " & paid & "," & PendingAppr & " "
        'End If
        ' -- con end 

        'Dim objdb As New EAD.DBCom()
        'ds = objdb.FillDs("select dbo.test() from pr_mstr")
        'meilai
        Dim doc_no, com_id As String
        If strFrm = "Dashboard" Then
            If strDONo <> "" Then
                doc_no = strDONo

            ElseIf strPONo <> "" Then
                doc_no = strPONo

            ElseIf strGRNNo <> "" Then
                doc_no = strGRNNo
            End If
            com_id = strBCoyID
        Else
            doc_no = ""
            com_id = ""
        End If

        'ds = objINV.get_unInvItem()
        'ds = objINV.get_unInvItem(doc_no, com_id)
        ds = objINV.get_unInvItemEn(strPONo, com_id)
        'DS.Tables(0).Columns(7).
        '//for sorting asc or desc
        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        '    dvViewPR.Sort = ViewState("SortExpression")
        '    If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        'End If
        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtg_InvList, dvViewPR)
            dtg_InvList.DataSource = dvViewPR
            hidControl.Value = ""
            hidSummary.Value = ""
            dtg_InvList.DataBind()
            'Dim chk As CheckBox
            'If Me.dtg_InvList.Items(1).Cells(1).Text = "" and Then
            '    dtg_InvList.Items(1).Cells(1).Text = viewstate("po_no")
            '    chk = dtg_InvList.Items(1).Cells(1).FindControl("chkSelection")
            'End If
            Me.cmd_createInv.Visible = True
            '//carol remark
            'Me.cmdReset.Visible = False
        Else
            'dtgDept.DataSource = ""
            dtg_InvList.DataBind()
            '//carol remark
            'Me.cmdReset.Visible = False
            Me.cmd_createInv.Visible = False
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ' add for above checking
        ViewState("PageCount") = dtg_InvList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("temprow") = ""
        ViewState("totalcount") = 0
        ViewState("SortExpression") = "POM_PO_NO"
        ViewState("SortAscending") = "yes"
        Bindgrid()
    End Sub

    Private Sub cmd_createInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_createInv.Click
        '10/10/2014 - Chee Hong GST Enhancement - Prev Function for next button
        strMode = "step2"
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim strDONo As String = ""
        Dim strGRNNo As String = ""
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim crdetail As New DataTable
        Dim drDocMatchRow As DataRow

        If ViewState("GSTCOD") = False Then
            crdetail.Columns.Add("bill_meth", Type.GetType("System.String"))
            crdetail.Columns.Add("po_no", Type.GetType("System.String"))
            crdetail.Columns.Add("POM_B_COY_ID", Type.GetType("System.String"))
            crdetail.Columns.Add("POM_PO_INDEX", Type.GetType("System.String"))

            crdetail.Columns.Add("do_no", Type.GetType("System.String"))
            crdetail.Columns.Add("grn_no", Type.GetType("System.String"))
            crdetail.Columns.Add("currency", Type.GetType("System.String"))
            crdetail.Columns.Add("PAY_DAY", Type.GetType("System.String"))
            crdetail.Columns.Add("ShipAmt", Type.GetType("System.String"))
            crdetail.Columns.Add("BalShipAmt", Type.GetType("System.String"))
            For Each dgitem In Me.dtg_InvList.Items
                chk = dgitem.FindControl("chkSelection")
                If chk.Checked Then
                    'If dgitem.Cells(10).Text = "FPO" Then
                    dtr = crdetail.NewRow()
                    dtr("bill_meth") = dgitem.Cells(EnumInvList.icMethod).Text
                    dtr("POM_B_COY_ID") = dgitem.Cells(EnumInvList.icCoyId).Text
                    dtr("POM_PO_INDEX") = dgitem.Cells(EnumInvList.icPOIndexV).Text
                    dtr("currency") = dgitem.Cells(EnumInvList.icCurrCode).Text
                    dtr("PAY_DAY") = dgitem.Cells(EnumInvList.icPayDay).Text
                    dtr("ShipAmt") = dgitem.Cells(EnumInvList.icBalShipAmt).Text
                    dtr("BalShipAmt") = Replace(dgitem.Cells(EnumInvList.icBalShipAmt).Text, ",", "")
                    strGRNNo = dgitem.Cells(EnumInvList.IcGRNV).Text
                    strDONo = dgitem.Cells(EnumInvList.icDOV).Text

                    If dgitem.Cells(EnumInvList.icDOV).Text = "FPO" Then
                        dtr("po_no") = dgitem.Cells(EnumInvList.icPONoV).Text
                        dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID")) ', "FPO", strGRNNo, strDONo)

                        For Each drDocMatchRow In dtDocMatch.Rows
                            dtr("grn_no") = drDocMatchRow("CDM_NO")
                            dtr("do_no") = drDocMatchRow("CDM_DO_NO")
                        Next
                    Else
                        '//Null valus from DB will show as "&nbsp;" in datagrid
                        dtr("po_no") = Replace(dgitem.Cells(EnumInvList.icPONoV).Text, "&nbsp;", "")
                        'dtr("grn_no") = Replace(dgitem.Cells(EnumInvList.IcGRNV).Text, "&nbsp;", "")
                        'dtr("do_no") = Replace(dgitem.Cells(EnumInvList.icDOV).Text, "&nbsp;", "")

                        If dgitem.Cells(EnumInvList.icInvOn).Text = "GRN" Then
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), Common.parseNull(dtr("POM_B_COY_ID")), "GRN", strGRNNo, strDONo)

                            'ElseIf dgitem.Cells(EnumInvList.icInvOn).Text = "FPO" Then
                            '    dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), Common.parseNull(dtr("POM_B_COY_ID")), "FPO", strGRNNo, strDONo)

                        ElseIf dgitem.Cells(EnumInvList.icInvOn).Text = "DO" Then
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), Common.parseNull(dtr("POM_B_COY_ID")), "DO", "", strDONo)

                        Else
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), dtr("POM_B_COY_ID"))

                        End If

                        For Each drDocMatchRow In dtDocMatch.Rows
                            dtr("grn_no") = drDocMatchRow("CDM_NO")
                            dtr("do_no") = drDocMatchRow("CDM_DO_NO")
                        Next
                    End If
                    crdetail.Rows.Add(dtr)
                End If
            Next
            DS.Tables.Add(crdetail)
            Me.dtg_InvList.Visible = False
            Me.dtg_InvList.Enabled = False
            Me.cmd_createInv.Visible = False
            'Me.Table4.Visible = False
            Me.back_view.Visible = False
            Me.back.Visible = True
            cmd_submit.Visible = True
            '  cmd_submit.Enabled = True
            dtg_inv2.Visible = True
            dtg_inv2.Enabled = True
            blnPaging = False   'Added by Joon on 29 Apr 2011
            SetGridProperty(dtg_inv2)
            'Me.lblTitle.Text = "Invoice Generation (Step 2)"
            '//carol remark
            'cmdReset.Attributes.Add("onclick", "Reset_Step2();")
            Me.lblStep1.Visible = False
            Me.lblStep2.Visible = True
            Bindgridinv2(DS)

        Else
            Dim strBillMethod, strDocNo, strPOIndex, strBillDoc, strShipAmt, strTax, strBCoyId As String
            'Dim dgitem As DataGridItem
            'Dim chk As CheckBox
            'Dim drDocMatchRow As DataRow

            For Each dgitem In Me.dtg_InvList.Items
                chk = dgitem.FindControl("chkSelection")
                If chk.Checked Then
                    strPOIndex = dgitem.Cells(EnumInvList.icPOIndexV).Text
                    strBCoyId = dgitem.Cells(EnumInvList.icCoyId).Text
                    strBillMethod = dgitem.Cells(EnumInvList.icMethod).Text
                    strGRNNo = dgitem.Cells(EnumInvList.IcGRNV).Text
                    strDONo = dgitem.Cells(EnumInvList.icDOV).Text

                    If dgitem.Cells(EnumInvList.icDOV).Text = "FPO" Then
                        strPONo = dgitem.Cells(EnumInvList.icPONoV).Text
                        dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID")) ', "FPO", strGRNNo, strDONo)

                        For Each drDocMatchRow In dtDocMatch.Rows
                            strGRNNo = drDocMatchRow("CDM_NO")
                            strDONo = drDocMatchRow("CDM_DO_NO")
                        Next
                    Else
                        strPONo = Replace(dgitem.Cells(EnumInvList.icPONoV).Text, "&nbsp;", "")

                        If dgitem.Cells(EnumInvList.icInvOn).Text = "GRN" Then
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), strBCoyId, "GRN", strGRNNo, strDONo)
                        ElseIf dgitem.Cells(EnumInvList.icInvOn).Text = "DO" Then
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), strBCoyId, "DO", "", strDONo)
                        Else
                            dtDocMatch = objTrac.getRelated_DocMatchForPO(dgitem.Cells(EnumInvList.icPONoV).Text, Session("CompanyID"), strBCoyId)
                        End If

                        For Each drDocMatchRow In dtDocMatch.Rows
                            strGRNNo = Common.parseNull(drDocMatchRow("CDM_NO"))
                            strDONo = Common.parseNull(drDocMatchRow("CDM_DO_NO"))
                        Next
                    End If
                    Dim billdoc As String = strPONo
                    If strDONo <> "" Then
                        billdoc = billdoc & "," & strDONo & " "
                    End If
                    If strGRNNo <> "" Then
                        billdoc = billdoc & "," & strGRNNo & " "
                    End If
                    If strBillMethod = "FPO" Then
                        strDocNo = strPONo
                        strBillDoc = strPONo
                    ElseIf strBillMethod = "DO" Then
                        strDocNo = strDONo
                        strBillDoc = billdoc
                    ElseIf strBillMethod = "GRN" Then
                        strDocNo = strGRNNo
                        strBillDoc = billdoc
                    End If
                End If
            Next

            Response.Redirect(dDispatcher.direct("Invoice", "InvTaxDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & strBillMethod & "&doc=" & strDocNo & "&bcomid=" & strBCoyId & "&billdoc=" & Server.UrlEncode(strBillDoc) & "&poidx=" & strPOIndex) & "&pono=" & strPONo & "&dono=" & strDONo & "&grnno=" & strGRNNo)
        End If

    End Sub

    Private Function Bindgridinv2(ByVal ds As DataSet) As String

        Dim objINV As New Invoice

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            Me.dtg_inv2.DataSource = dvViewPR
            hidControl.Value = ""
            hidSummary.Value = ""
            Me.dtg_inv2.DataBind()

            Me.cmd_submit.Visible = True
            Me.cmdReset.Visible = True
        Else
            'dtgDept.DataSource = ""
            Me.dtg_inv2.DataBind()
            Me.cmd_submit.Visible = False
            Me.cmdReset.Visible = False
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If

    End Function


    Private Sub back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
        Me.dtg_InvList.Visible = True
        Me.dtg_InvList.Enabled = True
        Me.cmd_createInv.Visible = True
        'Me.Table4.Visible = True
        'Me.back_view.Visible = True
        If strFrm = "Dashboard" Then
            back_view.Visible = True
        ElseIf strFrm = "invoiceView" Then
            back_view.Visible = True
        Else
            back_view.Visible = False
        End If
        Me.back.Visible = False
        cmd_submit.Visible = False
        '   cmd_submit.Enabled = False
        dtg_inv2.Dispose()
        dtg_inv2.Visible = False
        dtg_inv2.Enabled = False
        'Me.lblTitle.Text = "Invoice Generation (Step 1)"
        strMode = "step1"
        cmdReset.Visible = False
        '//carol remark
        'cmdReset.Attributes.Add("onclick", "Reset_Step1();")

        If ViewState("Back") = "Back" Then
            Response.Redirect("InvList.aspx?pageid=" & strPageId)
        End If
    End Sub

    Private Sub cmd_submit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_submit.Click
        back_view.Visible = False
        'Dim strscript1 As New System.Text.StringBuilder
        'strscript1.Append("<script language=""javascript"">")
        'strscript1.Append("document.getElementById('cmd_submit').style.display='none';")
        'strscript1.Append("</script>")
        'RegisterStartupScript("script2", strscript1.ToString())
        cmd_submit.Visible = False
        If ShipAmtTally() Then
            Submit(sender, e)
        Else
            Dim strscript As New System.Text.StringBuilder
            strscript.Append("<script language=""javascript"">")
            'strscript.Append("document.getElementById('cmd_submit').style.display='none';")
            strscript.Append("PromptMsg('" & strMsg & "');")
            strscript.Append("document.getElementById('btnHidden').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script1", strscript.ToString())
            cmd_submit.Visible = False

            'strscript.Append("ShowDialog('Dialog.aspx?page=" & strFileName & "','530px');")
            'If MsgBox(Me.strMsg, MsgBoxStyle.OkCancel) = 1 Then
            'Submit()
            'End If
        End If
    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        If hidresult.Value = "1" Then
            Submit(sender, e)
        Else
            back_view.Visible = False
            'Dim strscript1 As New System.Text.StringBuilder
            'strscript1.Append("<script language=""javascript"">")
            'strscript1.Append("document.getElementById('cmd_submit').style.display='';")
            'strscript1.Append("</script>")
            'RegisterStartupScript("script2", strscript1.ToString())
            '' ''cmd_submit.Visible = False

            cmd_submit.Visible = True
        End If

    End Sub
    Private Sub Submit(ByVal sender As System.Object, ByVal e As System.EventArgs)
        back_view.Visible = False
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim crdetail As New DataTable
        Dim txt_ref As TextBox
        Dim txt_remark, txt_ShipAmt, txt_Tax As TextBox
        Dim strinv As String
        Dim strgrn As String
        Dim strbcomid As String
        crdetail.Columns.Add("doc", Type.GetType("System.String"))
        crdetail.Columns.Add("ref", Type.GetType("System.String"))
        crdetail.Columns.Add("remark", Type.GetType("System.String"))
        crdetail.Columns.Add("amount", Type.GetType("System.String"))
        crdetail.Columns.Add("b_com_id", Type.GetType("System.String"))
        crdetail.Columns.Add("INV_STATUS", Type.GetType("System.String"))
        crdetail.Columns.Add("bill_meth", Type.GetType("System.String"))
        crdetail.Columns.Add("po_no", Type.GetType("System.String"))
        crdetail.Columns.Add("grn_no", Type.GetType("System.String"))
        crdetail.Columns.Add("do_no", Type.GetType("System.String"))
        crdetail.Columns.Add("pay_day", Type.GetType("System.String"))
        crdetail.Columns.Add("tax", Type.GetType("System.String"))
        crdetail.Columns.Add("ShipAmt", Type.GetType("System.String"))
        crdetail.Columns.Add("POM_PO_INDEX", Type.GetType("System.String"))
        Dim status As Integer = invStatus.NewInv
        For Each dgitem In Me.dtg_inv2.Items
            chk = dgitem.FindControl("chkSelection2")
            If chk.Checked Then
                txt_ref = dgitem.FindControl("txt_ref")
                txt_remark = dgitem.FindControl("txt_remark")
                txt_Tax = dgitem.FindControl("txttax")
                txt_ShipAmt = dgitem.FindControl("txtship")
                dtr = crdetail.NewRow()
                dtr("doc") = dgitem.Cells(EnumInv2.icDoc1).Text
                dtr("ref") = txt_ref.Text
                dtr("remark") = txt_remark.Text
                dtr("amount") = dgitem.Cells(EnumInv2.icAmount).Text  'VIEWSTATE("TOTAL")
                dtr("b_com_id") = dgitem.Cells(EnumInv2.icCoyId).Text
                dtr("inv_status") = status
                dtr("bill_meth") = dgitem.Cells(EnumInv2.icMethod).Text
                dtr("po_no") = dgitem.Cells(EnumInv2.icPONoV).Text
                dtr("grn_no") = dgitem.Cells(EnumInv2.IcGRNV).Text
                dtr("do_no") = dgitem.Cells(EnumInv2.icDOV).Text
                dtr("pay_day") = dgitem.Cells(EnumInv2.icPayDay).Text
                dtr("tax") = IIf(txt_Tax.Text = "", 0, txt_Tax.Text)
                dtr("ShipAmt") = Replace(txt_ShipAmt.Text, ",", "")
                dtr("POM_PO_INDEX") = dgitem.Cells(EnumInv2.icPoIdx).Text
                crdetail.Rows.Add(dtr)
                If strgrn = "" Then
                    strgrn = "'" & dgitem.Cells(EnumInv2.IcGRNV).Text & "'"
                Else
                    strgrn = strgrn & ",'" & dgitem.Cells(EnumInv2.IcGRNV).Text & "'"
                End If
            End If
        Next
        DS.Tables.Add(crdetail)
        Dim GRN_STATUS As Integer
        GRN_STATUS = GRNStatus.Invoiced
        Dim do_status As Integer
        do_status = DOStatus.Invoiced
        Dim url As String
        Dim strMsgLine As String = ""
        'Dim intInv As Integer = 0

        If Not objinv.Update_InvMstr(DS, GRN_STATUS, do_status, strinv, strbcomid) Then
            Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId))
        Else
            'strMsgLine = "The Following invoice(s) have been generated."
            If InStr(strinv, ",") > 0 Then
                strMsgLine = "Invoice Number " & strinv & " have been generated."

            Else
                If strinv = "Generated" Then
                    strMsgLine = "Invoice Number has been generated."
                Else
                    strMsgLine = "Invoice Number " & strinv & " has been generated."
                End If


            End If
            cmd_submit.Visible = False
            'strMsgLine &= "Invoice Number" & vbTab & "Ref. Document" & vbTab & "Purchaser Company" & vbTab & "Amount"
            'strMsgLine &= "Invoice Number" & vbTab & "Ref. Document" & vbTab & "Purchaser Company" & vbTab & "Amount"
            Me.cmd_submit.Enabled = False
            If back_view.HRef = "#" Or strFrm = "Dashboard" Then
                'Common.NetMsgbox(Me, strMsgLine, dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId))
                Common.NetMsgbox(Me, strMsgLine)
                ViewState("Back") = "Back"
                'back.HRef = dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId)
                'Response.Redirect("InvList.aspx?pageid=" & strPageId)
                cmd_createInv_Click(sender, e)
            Else
                Common.NetMsgbox(Me, strMsgLine, back_view.HRef)

            End If
            'Me.cmd_submit.Enabled = False
            'url = "InvGeneration2.aspx?pageid=" & strPageId & "&strinv=" & Server.UrlEncode(strinv) & "&strbcomid=" & Server.UrlEncode(strbcomid) & "&bill_meth=" & Server.UrlEncode(dgitem.Cells(EnumInv2.icMethod).Text) & "  "

            ''If dgitem.Cells(EnumInv2.icMethod).Text = "GRN" Then
            'Response.Redirect(url)
            ''ElseIf dgitem.Cells(EnumInv2.icMethod).Text = "FPO" Then
            ''    Response.Redirect(url)
            ''ElseIf dgitem.Cells(EnumInv2.icMethod).Text = "DO" Then
            ''    Response.Redirect(url)
            ''End If
        End If
    End Sub


    Private Sub dtg_InvList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_InvList.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objinv As New Invoice
            Dim objval As New InvValue
            Dim temprow As String
            Dim chk As CheckBox
            Dim drDocMatchRow As DataRow
            Dim strtemp, strtemp2 As String

            objval.po_no = Common.parseNull(dv("POM_PO_NO"))
            objval.GRN_NO = Common.parseNull(dv("CDM_GRN_NO"))
            objval.B_com_id = Common.parseNull(dv("POM_B_COY_ID"))
            chk = e.Item.Cells(EnumInvList.icCheck).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If Common.parseNull(dv("POM_BILLING_METHOD")) = "GRN" Then

                dtDocMatch = objTrac.getRelated_DocMatchForPO(dv("POM_PO_NO"), Session("CompanyID"), Common.parseNull(dv("POM_B_COY_ID")), "GRN", dv("GRN Number"), dv("DO Number"))

                For Each drDocMatchRow In dtDocMatch.Rows
                    If objinv.get_invInfo(objval) Then
                        'e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("CDM_GRN_NO")) & _
                        '"&side=v&BCoyID=" & Common.parse Null(dv("POM_B_COY_ID")) & """ ><font color=#0000ff>" & _
                        'Common.parseNull(dv("CDM_GRN_NO")) & "</font></A> Invoiced Created On " & objval.create_on & ""
                        e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(drDocMatchRow("CDM_NO")) & "&PONo=" & Common.parseNull(dv("POM_PO_NO")) & "&DONo=" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')""><font color=#0000ff>" &
                            Common.parseNull(drDocMatchRow("CDM_NO")) & "</font></A> Invoiced Created On " & objval.create_on & ""

                        'e.Item.Cells(3).Text = "<A href=""PODetail.aspx?PO_NO=" & Common.parseNull(dv("CDM_GRN_NO")) & "&status=" & e.Item.Cells(4).Text & "&side=v&cr_no=" & Common.parseNull(dv("CDM_GRN_NO")) & "&filetype=2 "" ><font color=#0000ff>" & Common.parseNull(dv("CDM_GRN_NO")) & "</font></A> Invoiced Created On " & objval.create_on & ""
                        chk.Visible = False
                        chk.Enabled = False
                    Else
                        'e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("CDM_GRN_NO")) & _
                        '"&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & """ ><font color=#0000ff>" & _
                        'Common.parseNull(dv("CDM_GRN_NO")) & "</font></A>"
                        e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(drDocMatchRow("CDM_NO")) & "&PONo=" & Common.parseNull(dv("POM_PO_NO")) & "&DONo=" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')""><font color=#0000ff>" &
                            Common.parseNull(drDocMatchRow("CDM_NO")) & "</font></A>"

                    End If
                Next

                'e.Item.Cells(3).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & Common.parseNull(dv("DO Number")) & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("POM_PO_NO")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("DO Number")) & "</font></A>"

                'If Not IsDBNull(objinv.get_grnprice(dv("POM_PO_NO"), dv("POM_B_COY_ID"), dv("POM_PO_INDEX"), Common.parseNull(dv("CDM_GRN_NO")))) Then
                e.Item.Cells(EnumInvList.icRemainAmt).Text = Format(objinv.get_grnprice(dv("POM_PO_NO"), dv("POM_B_COY_ID"), dv("POM_PO_INDEX"), Common.parseNull(dv("GRN Number"))), "#,##0.00")

                'Else
                'e.Item.Cells(EnumInvList.icRemainAmt).Text = "0.00"
                'End If

            ElseIf Common.parseNull(dv("POM_BILLING_METHOD")) = "FPO" Then

                If ViewState("temprow") <> CStr(Common.parseNull(dv("POM_PO_INDEX"))).ToString.Trim Then
                    objval.po_no = Common.parseNull(dv("POM_PO_NO"))
                    objinv.get_POprice(objval)
                    e.Item.Cells(EnumInvList.icRemainAmt).Text = Format(objval.Ordered_amount, "#,##0.00")
                Else
                    chk.Visible = False
                    chk.Enabled = False
                End If

                dtDocMatch = objTrac.getRelated_DocMatchForPO(dv("POM_PO_NO"), Session("CompanyID"), Common.parseNull(dv("POM_B_COY_ID"))) ', "FPO", dv("GRN Number"), dv("DO Number"))

                For Each drDocMatchRow In dtDocMatch.Rows

                    'e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("CDM_GRN_NO")) & _
                    '    "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & """ ><font color=#0000ff>" & _
                    '    Common.parseNull(dv("CDM_GRN_NO")) & "</font></A>"
                    strtemp &= "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(drDocMatchRow("CDM_NO")) & "&PONo=" & Common.parseNull(dv("POM_PO_NO")) & "&DONo=" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')""><font color=#0000ff>" &
                    Common.parseNull(drDocMatchRow("CDM_NO")) & "</font></A><BR>"

                    'e.Item.Cells(3).Text = "<A href=""#"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_GRN_NO")) & "</font></A>"
                    'e.Item.Cells(3).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                    'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                    'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                    strtemp2 &= "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & drDocMatchRow("CDM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("POM_PO_NO")) & "')"" ><font color=#0000ff>" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "</font></A><BR>"

                Next
                e.Item.Cells(EnumInvList.icGRN).Text = strtemp
                e.Item.Cells(EnumInvList.icDO).Text = strtemp2


            ElseIf Common.parseNull(dv("POM_BILLING_METHOD")) = "DO" Then

                dtDocMatch = objTrac.getRelated_DocMatchForPO(dv("POM_PO_NO"), Session("CompanyID"), Common.parseNull(dv("POM_B_COY_ID")), "DO", "", Common.parseNull(dv("DO Number")))

                For Each drDocMatchRow In dtDocMatch.Rows
                    If objinv.get_invInfo(objval) Then
                        'e.Item.Cells(3).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A></A>  Invoiced Created On " & objval.create_on & ""
                        'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A></A>  Invoiced Created On " & objval.create_on & ""
                        'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A></A>  Invoiced Created On " & objval.create_on & ""
                        e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & drDocMatchRow("CDM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("POM_PO_NO")) & "')"" ><font color=#0000ff>" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "</font></A></A>  Invoiced Created On " & objval.create_on & ""

                        'e.Item.Cells(4).Text = "<A href=""#"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>  Invoiced Created On " & objval.create_on & ""
                        chk.Visible = False
                        chk.Enabled = False
                    Else
                        'e.Item.Cells(3).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                        'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                        'e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('../DO/PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("CDM_DO_No") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_DO_No")) & "</font></A>"
                        e.Item.Cells(EnumInvList.icDO).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & drDocMatchRow("CDM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("POM_PO_NO")) & "')"" ><font color=#0000ff>" & Common.parseNull(drDocMatchRow("CDM_DO_NO")) & "</font></A>"

                    End If
                Next

                'e.Item.Cells(3).Text = "<A href=""#"" ><font color=#0000ff>" & Common.parseNull(dv("CDM_GRN_NO")) & "</font></A>"
                'e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("CDM_GRN_NO")) & _
                '    "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & """ ><font color=#0000ff>" & _
                '    Common.parseNull(dv("CDM_GRN_NO")) & "</font></A>"
                e.Item.Cells(EnumInvList.icGRN).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(dv("GRN Number")) & "&PONo=" & Common.parseNull(dv("POM_PO_NO")) & "&DONo=" & Common.parseNull(dv("DO Number")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')""><font color=#0000ff>" &
                    Common.parseNull(dv("GRN Number")) & "</font></A>"

                e.Item.Cells(EnumInvList.icRemainAmt).Text = Format(objinv.get_DOprice(Common.parseNull(dv("DO Number")), dv("POM_B_COY_ID"), dv("POM_PO_INDEX")), "#,##0.00")
                e.Item.Cells(EnumInvList.icDOV).Text = Common.parseNull(dv("DO Number"))
            End If


            'If ViewState("temprow") <> CStr(Common.parseNull(dv("POM_PO_INDEX"))).ToString.Trim Then
            '    ViewState("temprow") = CStr(Common.parseNull(dv("POM_PO_INDEX"))).ToString.Trim
            '    'objinv.get_POprice(objval)
            '    ViewState("line") = 0
            '    e.Item.Cells(EnumInvList.icInvOn).Text = dv("POM_BILLING_METHOD")
            '    e.Item.Cells(EnumInvList.icPO).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvList&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=otherv&filetype=2") & """ ><font color=#0000ff>" & dv("POM_PO_NO") & "</font></A>"
            '    objinv.get_POprice(objval)
            '    e.Item.Cells(EnumInvList.icCoyName).Text = Common.parseNull(dv("CM_COY_NAME"))
            '    ViewState("line") = ViewState("totalcount") Mod 2
            '    ViewState("totalcount") = ViewState("totalcount") + 1
            'End If

            e.Item.Cells(EnumInvList.icInvOn).Text = dv("POM_BILLING_METHOD")
            '' ''e.Item.Cells(EnumInvList.icPO).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvList&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=otherv&filetype=2") & """ ><font color=#0000ff>" & dv("POM_PO_NO") & "</font></A>"
            e.Item.Cells(EnumInvList.icPO).Text = "<A href=""" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvList&pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v&filetype=2") & """ ><font color=#0000ff>" & dv("POM_PO_NO") & "</font></A>"
            objinv.get_POprice(objval)
            e.Item.Cells(EnumInvList.icCoyName).Text = Common.parseNull(dv("CM_COY_NAME"))

            'If ViewState("line") = 0 Then
            '    e.Item.BackColor = Color.LightYellow()
            'ElseIf ViewState("line") = 1 Then
            '    e.Item.BackColor = Color.FromName("#f6f9fe")
            'End If

        End If

    End Sub

    Private Sub dtg_InvList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_InvList.ItemCreated
        Grid_ItemCreated(sender, e)
        intPageRecordCnt = ViewState("intPageRecordCnt")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtg_inv2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_inv2.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim objinv As New Invoice
            Dim objval As New InvValue
            Dim temprow As String
            Dim chk2 As CheckBox

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("po_no")

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
                hidSummary.Value = "Add Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Add Remarks-" & txtRemark.ClientID
            End If

            Dim billdoc As String = dv("po_no")
            If Not IsDBNull(dv("do_no")) AndAlso dv("do_no") <> "" Then
                billdoc = billdoc & "," & dv("do_no") & " "
            End If
            If Not IsDBNull(dv("grn_no")) AndAlso dv("grn_no") <> "" Then
                billdoc = billdoc & "," & dv("grn_no") & " "
            End If

            objval.po_no = Common.parseNull(dv("po_no"))
            objval.GRN_NO = Common.parseNull(dv("grn_no"))
            chk2 = e.Item.Cells(EnumInv2.icCheck).FindControl("chkSelection2")
            chk2.Attributes.Add("onclick", "checkChild2('" & chk2.ClientID & "')")
            'POReport.aspx?pageid=" & strPageId & "&po_no=" & Request(Trim("PO_No")) & "&side=v&BCoyID=" & Request(Trim("BCoyID")) & "
            If Common.parseNull(dv("bill_meth")) = "GRN" Then
                '**************meilai 14/1/2005 add pageid*********************
                e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""" & dDispatcher.direct("Invoice", "InvDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("grn_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & billdoc & "&poidx=" & dv("POM_PO_INDEX")) & " "" ><font color=#0000ff>Details</font></A>"
                'e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""#"" onclick=""abc('InvDetail.aspx?pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("grn_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & billdoc & "&poidx=" & dv("POM_PO_INDEX") & "', dtg_inv2_" & dtg_inv2.ClientID & "_txtship')""></font></A>"
                'e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow2('../PO/POReport.aspx?pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v')""  ><font color=#0000ff>" & dv("po_no") & "</font></A><br>" & _
                '                        "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("do_no") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & dv("do_no") & "</font></A><br>" & _
                '                        "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("grn_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & " "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"
                e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "pageid=" & strPageId & "&PO_No=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v')""") & "  ><font color=#0000ff>" & dv("po_no") & "</font></A><br>" &
                                        "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("do_no") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("po_no")) & "')"" ><font color=#0000ff>" & dv("do_no") & "</font></A><br>" &
                                       "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(dv("grn_no")) & "&PONo=" & Common.parseNull(dv("po_no")) & "&DONo=" & Common.parseNull(dv("do_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "') "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"

                ViewState("TOTAL") = objinv.get_grnprice(dv("po_no"), dv("POM_B_COY_ID"), dv("POM_PO_INDEX"), dv("grn_no"))
            ElseIf Common.parseNull(dv("bill_meth")) = "FPO" Then
                e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""" & dDispatcher.direct("Invoice", "InvDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("po_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & Common.parseNull(dv("po_no")) & "&poidx=" & dv("POM_PO_INDEX")) & "  "" ><font color=#0000ff>Details</font></A><br>"
                'e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""#"" onclick=""abc('InvDetail.aspx?pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("po_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & Common.parseNull(dv("po_no")) & "&poidx=" & dv("POM_PO_INDEX") & "', dtg_inv2_" & dtg_inv2.ClientID & "_txtship')""><font color=#0000ff>Details</font></A><br>"
                'e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow2('../PO/POReport.aspx?pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v')""  ><font color=#0000ff>" & dv("po_no") & "</font></A><br>"
                e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "pageid=" & strPageId & "&PO_No=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v") & "')""  ><font color=#0000ff>" & dv("po_no") & "</font></A><br>"

                objval.po_no = dv("po_no")
                objval.B_com_id = dv("POM_B_COY_ID")
                objinv.get_POprice(objval)
                ViewState("TOTAL") = objval.Ordered_amount
            ElseIf Common.parseNull(dv("bill_meth")) = "DO" Then
                e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""" & dDispatcher.direct("Invoice", "InvDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("po_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & Common.parseNull(dv("po_no")) & "&poidx=" & dv("POM_PO_INDEX")) & " "" ><font color=#0000ff>Details</font></A><br>"
                'e.Item.Cells(EnumInv2.icDetails).Text = "<A href=""#"" onclick=""abc('InvDetail.aspx?pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("po_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & Common.parseNull(dv("po_no")) & "&poidx=" & dv("POM_PO_INDEX") & "', dtg_inv2_" & dtg_inv2.ClientID & "_txtship')""><font color=#0000ff>Details</font></A><br>"

                'e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow2('../PO/POReport.aspx?pageid=" & strPageId & "&PO_INDEX=" & dv("POM_PO_INDEX") & "&PO_NO=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID") & "&side=v')""><font color=#0000ff>" & dv("po_no") & "</font></A><br>" & _
                '                        "<A href=""#"" onclick=""PopWindow('../DO/DOReport.aspx?pageid=" & strPageId & "&DONO=" & dv("do_no") & "&SCoyID=" & Session("CompanyID") & "')"" ><font color=#0000ff>" & dv("do_no") & "</font></A><br>" & _
                '                         "<A href=""../GRN/GRNDetails.aspx?pageid=" & strPageId & "&GRNNO=" & Common.parseNull(dv("grn_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID")) & " "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"
                e.Item.Cells(EnumInv2.icDoc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "pageid=" & strPageId & "&PO_No=" & Common.parseNull(dv("po_no")) & "&BCoyID=" & dv("POM_B_COY_ID")) & "&side=v')""><font color=#0000ff>" & dv("po_no") & "</font></A><br>" &
                        "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("do_no") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & dv("po_no")) & "')"" ><font color=#0000ff>" & dv("do_no") & "</font></A><br>" &
                         "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "pageid=" & strPageId & "&GRNNo=" & Common.parseNull(dv("grn_no")) & "&PONo=" & Common.parseNull(dv("po_no")) & "&DONo=" & Common.parseNull(dv("do_no")) & "&side=v&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "') "" ><font color=#0000ff>" & dv("grn_no") & "</font></A><br>"

                ViewState("TOTAL") = objinv.get_DOprice(dv("do_no"), dv("POM_B_COY_ID"), dv("POM_PO_INDEX"))
            End If
            e.Item.Cells(EnumInv2.icAmount).Text = Format(ViewState("TOTAL"), "#,##0.00")
            e.Item.Cells(EnumInv2.icDoc1).Text = billdoc
            e.Item.Cells(EnumInv2.icMethod).Text = dv("bill_meth")
            e.Item.Cells(EnumInv2.icPONoV).Text = Common.parseNull(dv("po_no"))
            e.Item.Cells(EnumInv2.IcGRNV).Text = Common.parseNull(dv("grn_no"))
            e.Item.Cells(EnumInv2.icDOV).Text = Common.parseNull(dv("do_no"))
            e.Item.Cells(EnumInv2.icCoyId).Text = Common.parseNull(dv("POM_B_COY_ID"))
            e.Item.Cells(EnumInv2.icBalShipAmt).Text = Common.parseNull(dv("BalShipAmt"))
            'e.Item.Cells(EnumInv2.icShipAmt).Text = Common.parseNull(dv("BalShipAmt"))

            Dim txtShip As TextBox
            Dim dblShip As Double
            Dim revShip As RegularExpressionValidator
            txtShip = e.Item.FindControl("txtShip")
            txtShip.Text = Replace(Common.parseNull(dv("BalShipAmt")), ",", "")
            dblShip = CDbl(txtShip.Text)
            If dblShip < 0 Then txtShip.Text = 0.0
            revShip = e.Item.FindControl("revShip")
            'revShip.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$" '"\d[1,9]{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$" '(?!^0*$)
            'revShip.ValidationExpression = "\d{1,10}(\.\d{1,2})?$" '"\d[1,9]{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$" '(?!^0*$)
            revShip.ValidationExpression = "^(\d|,)*\.?\d{0,2}$"
            revShip.ControlToValidate = "txtShip"
            revShip.ErrorMessage = "Invalid Shipping & Handling"
            revShip.Text = "?"
            revShip.Display = ValidatorDisplay.Dynamic
            'If dv("BalShipAmt") < 0 Then
            'txtShip.Text = Format(txtShip.Text, "###,###,##0.00")
            'Else
            txtShip.Text = Format(CDbl(txtShip.Text), "###,###,##0.00")
            'End If

            txtShip.Attributes.Add("onkeypress", "return isNumberKey(event);")

            Dim txtTax As TextBox
            Dim revTax As RegularExpressionValidator
            txtTax = e.Item.FindControl("txtTax")
            revTax = e.Item.FindControl("revtax")
            revTax.ValidationExpression = "^\d{1,4}$"
            revTax.ControlToValidate = "txtTax"
            revTax.ErrorMessage = "Invalid Tax"
            revTax.Text = "?"
            revTax.Display = ValidatorDisplay.Dynamic
            txtTax.Attributes.Add("onkeypress", "return isNumericKey(event);")

            Dim strInv As String
            Dim intTax As Integer
            Dim dblShipAmt As Double
            Dim strRef, strRemark, strShip As TextBox
            Dim objDB As New EAD.DBCom
            Dim COUNT, i As Integer
            Dim ARRAY(100) As String

            If ViewState("Back") = "Back" Then
                chk2 = e.Item.Cells(EnumInv2.icCheck).FindControl("chkSelection2")
                'chk2.Checked = True
                chk2.Enabled = True

                If e.Item.Cells(EnumInv2.icDoc).Text <> "" Then
                    If Common.parseNull(dv("po_no")) <> "" Then
                        'e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"
                        strInv = objDB.GetVal("SELECT IFNULL(CDM_INVOICE_NO, '') AS CDM_INVOICE_NO FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID = '" & dv("POM_B_COY_ID") & "' AND CDM_PO_NO = '" & dv("po_no") & "'")
                        If strInv <> "" Then
                            chk2.Checked = True
                            e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"

                            intTax = objDB.GetVal("SELECT IFNULL(IM_WITHHOLDING_TAX,0) AS IM_WITHHOLDING_TAX FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                            dblShipAmt = objDB.GetVal("SELECT IM_SHIP_AMT FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                            txtTax.Text = intTax
                            txtShip.Text = Format(CDbl(dblShipAmt), "###,###,##0.00")
                        Else

                        End If
                        If Common.parseNull(dv("do_no")) <> "" Then
                            'e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"
                            strInv = objDB.GetVal("SELECT IFNULL(CDM_INVOICE_NO, '') AS CDM_INVOICE_NO FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID = '" & dv("POM_B_COY_ID") & "' AND CDM_PO_NO = '" & dv("po_no") & "' AND CDM_DO_NO = '" & dv("do_no") & "'")
                            If strInv <> "" Then
                                chk2.Checked = True
                                e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"

                                intTax = objDB.GetVal("SELECT IFNULL(IM_WITHHOLDING_TAX,0) AS IM_WITHHOLDING_TAX FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                                dblShipAmt = objDB.GetVal("SELECT IM_SHIP_AMT FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                                txtTax.Text = intTax
                                txtShip.Text = Format(CDbl(dblShipAmt), "###,###,##0.00")
                            Else

                            End If
                            If Common.parseNull(dv("grn_no")) <> "" Then
                                'e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"
                                strInv = objDB.GetVal("SELECT CDM_INVOICE_NO FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID = '" & dv("POM_B_COY_ID") & "' AND CDM_PO_NO = '" & dv("po_no") & "' AND CDM_DO_NO = '" & dv("do_no") & "' AND CDM_GRN_NO = '" & dv("grn_no") & "'")
                                If strInv <> "" Then
                                    chk2.Checked = True
                                    e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"

                                    intTax = objDB.GetVal("SELECT IFNULL(IM_WITHHOLDING_TAX,0) AS IM_WITHHOLDING_TAX FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                                    dblShipAmt = objDB.GetVal("SELECT IM_SHIP_AMT FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & strInv & "' AND IM_S_COY_ID = '" & Session("CompanyID") & "'")
                                    txtTax.Text = intTax
                                    txtShip.Text = Format(CDbl(dblShipAmt), "###,###,##0.00")
                                Else

                                End If
                            End If
                        End If
                    End If
                End If
                'e.Item.Cells(EnumInv2.icInv).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & strInv & "&vcomid=" & Session("CompanyID") & "&BCoyID=" & Common.parseNull(dv("POM_B_COY_ID"))) & "')"" ><font color=#0000ff>" & strInv & "</font></A>"
                cmd_submit.Visible = False
                cmdReset.Visible = False

                objinv.GetInvDetail(strInv, ARRAY, COUNT)
                If ARRAY(0) <> "" Then
                    strRef = e.Item.Cells(EnumInv2.icReference).FindControl("txt_ref")
                    strRef.ReadOnly = True
                    strRef.Text = ARRAY(0)

                    strRemark = e.Item.Cells(EnumInv2.icRemarks).FindControl("txt_remark")
                    strRemark.ReadOnly = True
                    strRemark.Text = ARRAY(1)

                    strShip = e.Item.Cells(EnumInv2.icShipAmt).FindControl("txtship")
                    strShip.ReadOnly = True
                    strShip.Text = Format(CDbl(ARRAY(2)), "###,###,##0.00")
                End If

                e.Item.Cells(EnumInv2.icDoc).Enabled = False
                e.Item.Cells(EnumInv2.icCurrency).Enabled = False
                e.Item.Cells(EnumInv2.icAmount).Enabled = False
                e.Item.Cells(EnumInv2.icButton).Visible = False

                chk2.Enabled = False
            Else
                e.Item.Cells(EnumInv2.icInv).Visible = False
            End If
        End If
    End Sub

    Private Sub dtg_inv2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_inv2.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            'ME.dtg_inv2.Columns(
            Dim chkAll2 As CheckBox = e.Item.FindControl("chkAll2")
            chkAll2.Attributes.Add("onclick", "selectAll2();")

            If ViewState("Back") = "Back" Then
                e.Item.Cells(EnumInv2.icButton).Visible = False
            Else
                e.Item.Cells(EnumInv2.icInv).Visible = False
            End If
        End If
    End Sub

    Private Sub dtg_inv2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtg_inv2.ItemCommand
        If e.CommandName = "Select" Then

            'Validate()
            'If Not Page.IsValid Then
            '    Exit Sub
            '    'Else
            '    '    Dim txtShip As TextBox
            '    '    Dim dblShip As Double
            '    '    Dim revShip As RegularExpressionValidator
            '    '    revShip = e.Item.Cells(EnumInv2.icShipAmt).FindControl("revShip")
            '    '    revShip.Text = ""
            '    '    revShip.Display = ValidatorDisplay.Dynamic
            'End If

            Dim strBillMethod, strDocNo, strBillDoc, strShipAmt, strTax As String
            strBillMethod = e.Item.Cells(EnumInv2.icMethod).Text
            If strBillMethod = "FPO" Then
                strDocNo = e.Item.Cells(EnumInv2.icPONoV).Text
                strBillDoc = e.Item.Cells(EnumInv2.icPONoV).Text
            ElseIf strBillMethod = "DO" Then
                strDocNo = e.Item.Cells(EnumInv2.icDOV).Text
                strBillDoc = e.Item.Cells(EnumInv2.icDoc1).Text
            ElseIf strBillMethod = "GRN" Then
                strDocNo = e.Item.Cells(EnumInv2.IcGRNV).Text
                strBillDoc = e.Item.Cells(EnumInv2.icDoc1).Text
            End If

            strShipAmt = CType(e.Item.Cells(EnumInv2.icShipAmt).FindControl("txtShip"), TextBox).Text
            strTax = CType(e.Item.Cells(EnumInv2.icTax).FindControl("txttax"), TextBox).Text
            Session("ourref") = CType(e.Item.Cells(EnumInv2.icReference).FindControl("txt_ref"), TextBox).Text
            Session("remarks") = CType(e.Item.Cells(EnumInv2.icRemarks).FindControl("txt_remark"), TextBox).Text

            'Dim strJScript As String
            'strJScript = "<script language=javascript>"
            'strJScript += "window.open('" & dDispatcher.direct("Invoice", "InvDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & strBillMethod & "&doc=" & strDocNo & "&bcomid=" & e.Item.Cells(EnumInv2.icCoyId).Text & "&billdoc=" & Server.UrlEncode(strBillDoc) & "&shipamt=" & strShipAmt & "&poidx=" & e.Item.Cells(EnumInv2.icPoIdx).Text) & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            'strJScript += "</script>"
            'Response.Write(strJScript)

            Response.Redirect(dDispatcher.direct("Invoice", "InvDetail.aspx", "pageid=" & strPageId & "&bill_meth=" & strBillMethod & "&doc=" & strDocNo & "&bcomid=" & e.Item.Cells(EnumInv2.icCoyId).Text & "&billdoc=" & Server.UrlEncode(strBillDoc) & "&shipamt=" & strShipAmt & "&tax=" & strTax & "&poidx=" & e.Item.Cells(EnumInv2.icPoIdx).Text))
            '"<A href=""InvDetail.aspx?pageid=" & strPageId & "&bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("grn_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & billdoc & "&poidx=" & dv("POM_PO_INDEX") & " "" ><font color=#0000ff>Details</font></A>"
            '"<A href=""InvDetail.aspx?bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("po_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & Common.parseNull(dv("po_no")) & "&poidx=" & dv("POM_PO_INDEX") & "  "" ><font color=#0000ff>Details</font></A><br>"
            '"<A href=""InvDetail.aspx?bill_meth=" & Common.parseNull(dv("bill_meth")) & "&doc=" & dv("do_no") & "&bcomid=" & dv("POM_B_COY_ID") & "&billdoc=" & billdoc & "&poidx=" & dv("POM_PO_INDEX") & "   "" ><font color=#0000ff>Details</font></A><br>"

        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchGInv_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        'Session("w_SearchGInv_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a>" & _
        '            "</div>"
    End Sub

    'Private Sub PreviewDO()
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
    '            .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
    '                    & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
    '                    & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
    '                    & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
    '                    & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
    '                    & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
    '                    & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
    '                    & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
    '                    & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", ViewState("DONo")))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
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
    '                Case "freightamt"
    '                    'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Me.txtFreAmt.Text)

    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '    "<DeviceInfo>" + _
    '        '        "  <OutputFormat>EMF</OutputFormat>" + _
    '        '        "  <PageWidth>8.27in</PageWidth>" + _
    '        '        "  <PageHeight>11in</PageHeight>" + _
    '        '        "  <MarginTop>0.25in</MarginTop>" + _
    '        '        "  <MarginLeft>0.25in</MarginLeft>" + _
    '        '        "  <MarginRight>0.25in</MarginRight>" + _
    '        '        "  <MarginBottom>0.25in</MarginBottom>" + _
    '        '        "</DeviceInfo>"
    '        Dim deviceInfo As String = _
    '            "<DeviceInfo>" + _
    '                "  <OutputFormat>EMF</OutputFormat>" + _
    '                "</DeviceInfo>"

    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "DO\DOReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('DOReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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
    Private Function ShipAmtTally() As Boolean
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim txt_ShipAmt As TextBox
        Dim dblShipAmt, dblBalShip As Double
        For Each dgitem In Me.dtg_inv2.Items
            chk = dgitem.FindControl("chkSelection2")
            If chk.Checked Then
                txt_ShipAmt = dgitem.FindControl("txtship")
                dblShipAmt = CDbl(txt_ShipAmt.Text)
                dblBalShip = CDbl(dgitem.Cells(EnumInv2.icBalShipAmt).Text)
                If dblBalShip < 0 Then dblBalShip = 0
                If dblShipAmt <> dblBalShip Then
                    If strMsg = "" Then
                        strMsg = "The shipping & handling amount of the following PO(s) varies from the Invoice:"
                    End If
                    strMsg = strMsg + "\nAccumulated shipping & handling amount for " + dgitem.Cells(EnumInv2.icPONoV).Text + " is " + Format(dblBalShip, "###,###,##0.00")
                End If
            End If
        Next
        If strMsg <> "" Then
            strMsg = strMsg + "\nWant to proceed? "
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub cmdReset_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        cmd_createInv_Click(sender, e)
    End Sub

    Private Sub back_view_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles back_view.ServerClick
        'If ViewState("Back") = "Back" Then
        '    Response.Redirect("InvList.aspx?pageid=" & strPageId)
        'End If

        If strFrm = "Dashboard" Then
            Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
        End If
    End Sub
End Class