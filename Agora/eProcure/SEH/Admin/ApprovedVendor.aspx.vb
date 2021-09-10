Imports AgoraLegacy
Imports eProcure.Component


Public Class ApprovedVendorSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objCompanies As New Companies

    Enum enumAppVen
        icChk
        icCoyId
        icCoyName
        icBRN
        icGSTRegNo
        icVenCode
        icGSTRate
        icGSTTaxCode
        icPayTerm
        icPayMth
        icInvBased
        icEDD
        icGRNOutsCtrl

    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    'Protected WithEvents txtVID As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtVName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    'Protected WithEvents dtgVDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cboPayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboPayMeth As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboInvoiceInd As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboGRN As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearchAdd As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    ' Protected WithEvents cmdSaveVendor As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents back As System.Web.UI.HtmlControls.HtmlGenericControl
    'Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    'Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    'Protected WithEvents lblAction As System.Web.UI.WebControls.Label


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim strDefaultAdd As String
    Dim dvGSTRate, dvGSTTaxCode, dvPayTerm, dvPayMethod, dvInvoiceInd, dvGRN As DataView
    Dim txtEDD As TextBox
    Dim strType As String
    Dim strVID As String
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        cmdSave.Enabled = False
        Dim alButtonList As ArrayList

        Select Case strType
            Case "AV"
                alButtonList = New ArrayList
                alButtonList.Add(cmdAdd)
                htPageAccess.Add("add", alButtonList)
                alButtonList = New ArrayList
                alButtonList.Add(cmdSave)
                htPageAccess.Add("update", alButtonList)
            Case "AAV"
                alButtonList = New ArrayList
                alButtonList.Add(cmdAdd)
                alButtonList.Add(cmdSave)
                htPageAccess.Add("add", alButtonList)
        End Select

        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If ViewState("intPageRecordCnt") > 0 Then
            cmdDelete.Enabled = blnCanDelete
            Select Case strType
                Case "AV"
                    cmdSave.Enabled = blnCanUpdate
                Case "AAV"
                    cmdSave.Enabled = blnCanUpdate Or blnCanAdd
            End Select
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
        Else
            cmdDelete.Enabled = False
            cmdSave.Enabled = False
            'cmdReset.Disabled = True
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)

        SetGridProperty(dtgVDetail)
        'strMode = Request.QueryString("mode")
        strType = Me.Request.QueryString("type")
        ViewState("Type") = strType
        Me.dtgVDetail.Columns(enumAppVen.icEDD).Visible = False
        Select Case strType
            Case "AV"
                lblHeader.Text = "Approved Vendor"
                back.Style("display") = "none"
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant approved vendors. Click the Add button to add new approved vendor."
            Case "AAV"
                lblHeader.Text = "Add Approved Vendor (from all vendors list below)"
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant vendors for approving.</br>Select vendors by click on the checkbox and update the Payment Terms and Payment Method."
                cmdAdd.Visible = False
                'cmdSave.Visible = True
                cmdSave.Enabled = True
                cmdDelete.Visible = False
                back.Style("DISPLAY") = "inline"
                cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','');")

                ' ai chu add 29/08/2005 
                ' SR U30007
                Dim objComps As New Companies
                Dim objComp As New Company
                objComp = objComps.GetCompanyDetails(Session("CompanyId"))
                ViewState("PaymentMethod") = objComp.PaymentMethod
                ViewState("PaymentTerm") = objComp.PaymentTerm
        End Select

        If Not Page.IsPostBack Then
            Session("aryVenCode") = Nothing

            '//because No DataGrid display when page first loaded
            cmdSave.Enabled = False

            cmdDelete.Enabled = False
            cmdReset.Disabled = True
            'Michelle (10/1/2013) - Issue 1832
            Dim objAdmin As New Admin
            ViewState("blnGRNCtrl") = objAdmin.withGRNCtrl()
            If ViewState("blnGRNCtrl") = False Then
                dtgVDetail.Columns(enumAppVen.icGRNOutsCtrl).Visible = False
            End If
        Else
            fillTaxCode()
        End If
        '//to attach javascript to button onclick event

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        'lnkBack.NavigateUrl = "ApprovedVendor.aspx?mode=approved&type=AV&pageid=" & strPageId
        lnkBack.NavigateUrl = "" & dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=AV&pageid=" & strPageId)


    End Sub
    Sub dtgVDetail_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgVDetail.PageIndexChanged
        dtgVDetail.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objAdmin As New Admin
        Dim ds As New DataSet
        Dim strSelect As String
        Dim strSelectName As String
        'Dim lstItem As New ListItem
        strSelect = txtVID.Text
        strSelectName = txtVName.Text

        ds = objAdmin.searchvendor(strType, strSelect, strSelectName, True)
        Dim dvViewVendor As DataView
        dvViewVendor = ds.Tables(0).DefaultView

        dvViewVendor.Sort = ViewState("SortExpression")
        If pSorted Then
            dvViewVendor.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewVendor.Sort += " DESC"
        End If

        If ViewState("action") = "del" Then
            If dtgVDetail.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVDetail.PageSize = 0 Then
                dtgVDetail.CurrentPageIndex = dtgVDetail.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim objGlobal As New AppGlobals
        dvPayTerm = objGlobal.GetCodeTableView(CodeTable.PaymentTerm)
        dvPayMethod = objGlobal.GetCodeTableView(CodeTable.PaymentMethod)
        dvGRN = objGlobal.GetCodeTableView(CodeTable.GRNCtrlDays)
        'dtgVDetail.DataSource = dvViewSample
        'dtgVDetail.DataBind()

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgVDetail, dvViewVendor)
            cmdSave.Enabled = True
            cmdDelete.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            '20110706Jules - enabled column
            'Michelle (25/11/2010) - for FTN, invoice is on GRN
            'If Session("Env") = "FTN" Then
            '    Me.dtgVDetail.Columns(6).Visible = False
            'End If
            dtgVDetail.DataSource = dvViewVendor
            dtgVDetail.DataBind()
        Else

            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            cmdReset.Disabled = True
            dtgVDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If
        ' add for above checking
        ViewState("PageCount") = dtgVDetail.PageCount
        objAdmin = Nothing
    End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.Response.Redirect(dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=AAV&pageid=" & strPageId))


    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim grdItem As DataGridItem
        lblMsg.Visible = False

        For Each grdItem In dtgVDetail.Items
            Dim chkSelection As CheckBox = grdItem.Cells(enumAppVen.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim objAdmin_ext As New Admin_Ext
                'Dim vendorid As String = grdItem.Cells(enumAppVen.icVenId).Text
                Dim vendorid As String = CType(grdItem.FindControl("lnkVendId"), HyperLink).Text
                objAdmin_ext.delvendor(vendorid, Session("aryVenCode"))
                objAdmin_ext = Nothing
            End If
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        ViewState("action") = "del"
        Bindgrid()

    End Sub

    Private Sub dtgVDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVDetail.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'e.Item.Cells(1).Text
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objGlobal As New AppGlobals
            Dim objGst As New GST
            Dim cboGSTRate As DropDownList
            Dim cboGSTTaxCode As DropDownList
            Dim cboPayTerm As DropDownList
            Dim cboGRN As DropDownList
            Dim cboPayMeth As DropDownList
            Dim cboInvoiceInd As DropDownList
            Dim lstItem As New ListItem
            Dim lstItem2 As New ListItem
            Dim lstItem3 As New ListItem
            Dim txtEstDate As New TextBox
            Dim lblGSTRate As New Label
            Dim strGstRegNo As String
            Dim strFileName As String

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(enumAppVen.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkVendId As HyperLink
            lnkVendId = e.Item.FindControl("lnkVendId")
            lnkVendId.Text = dv("Cm_COY_ID")
            lnkVendId.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=ApprovedVendor&pageid=" & strPageId & "&v_com_id=" & dv("Cm_COY_ID") & "&type=" & ViewState("Type"))
            'e.Item.Cells(enumVDetail.icCoyId).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=ApprovedVendor&pageid=" & strPageId & "&v_com_id=" & dv("Cm_COY_ID") & "&type=" & ViewState("Type")) & """ ><font color=#0000ff>" & dv("Cm_COY_ID") & "</font></A><br>"

            strGstRegNo = ""
            strGstRegNo = Common.parseNull(dv("CM_TAX_REG_NO"))
            'strGstRegNo = objGst.chkGST(dv("CM_COY_ID"))

            If strType = "AV" Then
                cboGSTRate = e.Item.Cells(enumAppVen.icGSTRate).FindControl("cboGSTRate")
                cboGSTTaxCode = e.Item.Cells(enumAppVen.icGSTTaxCode).FindControl("cboGSTTaxCode")
                lblGSTRate = e.Item.Cells(enumAppVen.icGSTRate).FindControl("lblGSTRate")

                If strGstRegNo <> "" Then
                    cboGSTRate.Enabled = True

                    If IsDBNull(dv("CV_GST_RATE")) Then
                        objGlobal.FillGST(cboGSTRate, True)
                        lblGSTRate.Text = ""
                    Else
                        objGlobal.FillGST(cboGSTRate, True)
                        Common.SelDdl(dv("CV_GST_RATE"), cboGSTRate)
                        lblGSTRate.Text = dv("CV_GST_RATE")
                    End If

                    objGlobal.FillTaxCode(cboGSTTaxCode, Common.parseNull(dv("CV_GST_RATE")), "P")
                    If Not IsDBNull(dv("CV_GST_TAX_CODE")) Then
                        Common.SelDdl(dv("CV_GST_TAX_CODE"), cboGSTTaxCode)
                    End If

                    If cboGSTTaxCode.SelectedValue = "N/A" Then
                        cboGSTTaxCode.Enabled = False
                    Else
                        cboGSTTaxCode.Enabled = True
                    End If
                    'If Common.parseNull(dv("CV_GST_RATE")) = "EX" Then
                    '    cboGSTTaxCode.Enabled = False
                    'Else
                    '    cboGSTTaxCode.Enabled = True
                    'End If
                Else
                    cboGSTRate.Items.Clear()
                    cboGSTTaxCode.Items.Clear()
                    lstItem2.Value = "N/A"
                    lstItem2.Text = "N/A"
                    cboGSTRate.Items.Insert(0, lstItem2)
                    lstItem3.Value = "NR"
                    lstItem3.Text = "NR"
                    cboGSTTaxCode.Items.Insert(0, lstItem3)
                    lblGSTRate.Text = "N/A"
                    cboGSTRate.Enabled = False
                    cboGSTTaxCode.Enabled = False
                End If

                cboPayTerm = e.Item.Cells(enumAppVen.icPayTerm).FindControl("cboPayTerm")
                Common.FillDdl(cboPayTerm, "CODE_DESC", "CODE_ABBR", dvPayTerm)
                Common.SelDdl(dv("cv_payment_term"), cboPayTerm)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayTerm.Items.Insert(0, lstItem)

                cboPayMeth = e.Item.Cells(enumAppVen.icPayMth).FindControl("cboPayMeth")
                Common.FillDdl(cboPayMeth, "CODE_DESC", "CODE_ABBR", dvPayMethod)
                Common.SelDdl(dv("cv_payment_method"), cboPayMeth)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayMeth.Items.Insert(0, lstItem)

                cboInvoiceInd = e.Item.Cells(enumAppVen.icInvBased).FindControl("cboInvoiceInd")
                Common.FillDdl(cboInvoiceInd, "CODE_DESC", "CODE_ABBR", dvInvoiceInd)
                Common.SelDdl(dv("CV_BILLING_METHOD"), cboInvoiceInd)

                txtEstDate = e.Item.Cells(enumAppVen.icEDD).FindControl("txtEstDate")
                txtEstDate.Text = Common.parseNull(dv("CV_EDD"))

                If ViewState("blnGRNCtrl") Then
                    cboGRN = e.Item.Cells(enumAppVen.icGRNOutsCtrl).FindControl("cboGRN")
                    Common.FillDdl(cboGRN, "CODE_DESC", "CODE_ABBR", dvGRN)
                    Common.SelDdl(dv("CV_GRN_CTRL_TERM"), cboGRN)
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    cboGRN.Items.Insert(0, lstItem)
                End If

                strFileName = dDispatcher.direct("Admin", "VendorCodeMaster.aspx", "pageid=" & strPageId & "&venid=" & dv("Cm_COY_ID") & "&type=" & strType)
                strFileName = Server.UrlEncode(strFileName)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                e.Item.Cells(enumAppVen.icVenCode).Text = "<A onclick=""ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','350px');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "v_icon.gif") & "></A>"

            Else
                cboGSTRate = e.Item.Cells(enumAppVen.icGSTRate).FindControl("cboGSTRate")
                cboGSTTaxCode = e.Item.Cells(enumAppVen.icGSTTaxCode).FindControl("cboGSTTaxCode")
                lblGSTRate = e.Item.Cells(enumAppVen.icGSTRate).FindControl("lblGSTRate")

                If strGstRegNo <> "" Then
                    objGlobal.FillGST(cboGSTRate, True)
                    objGlobal.FillTaxCode(cboGSTTaxCode, "STD", "P")
                    lblGSTRate.Text = "STD"
                    cboGSTRate.SelectedValue = "STD"
                    cboGSTRate.Enabled = True
                    cboGSTTaxCode.Enabled = True

                    If cboGSTTaxCode.SelectedValue = "N/A" Then
                        cboGSTTaxCode.Enabled = False
                    Else
                        cboGSTTaxCode.Enabled = True
                    End If
                Else
                    cboGSTRate.Items.Clear()
                    cboGSTTaxCode.Items.Clear()
                    lstItem2.Value = "N/A"
                    lstItem2.Text = "N/A"
                    cboGSTRate.Items.Insert(0, lstItem2)
                    lstItem3.Value = "NR"
                    lstItem3.Text = "NR"
                    cboGSTTaxCode.Items.Insert(0, lstItem3)
                    lblGSTRate.Text = "N/A"
                    cboGSTRate.Enabled = False
                    cboGSTTaxCode.Enabled = False
                End If

                cboPayTerm = e.Item.Cells(enumAppVen.icPayTerm).FindControl("cboPayTerm")
                Common.FillDdl(cboPayTerm, "CODE_DESC", "CODE_ABBR", dvPayTerm)
                'Common.SelDdl(dv("cv_payment_term"), cboPayTerm)
                Common.SelDdl(ViewState("PaymentTerm"), cboPayTerm)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayTerm.Items.Insert(0, lstItem)

                cboPayMeth = e.Item.Cells(enumAppVen.icPayMth).FindControl("cboPayMeth")
                Common.FillDdl(cboPayMeth, "CODE_DESC", "CODE_ABBR", dvPayMethod)
                'Common.SelDdl(dv("cv_payment_method"), cboPayMeth)
                Common.SelDdl(ViewState("PaymentMethod"), cboPayMeth)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayMeth.Items.Insert(0, lstItem)

                If ViewState("blnGRNCtrl") Then
                    cboGRN = e.Item.Cells(enumAppVen.icGRNOutsCtrl).FindControl("cboGRN")
                    Common.FillDdl(cboGRN, "CODE_DESC", "CODE_ABBR", dvGRN)
                    Common.SelDdl(ViewState("GRNCtrl"), cboGRN)
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    cboGRN.Items.Insert(0, lstItem)
                End If

                strFileName = dDispatcher.direct("Admin", "VendorCodeMaster.aspx", "pageid=" & strPageId & "&venid=" & dv("Cm_COY_ID") & "&type=" & strType)
                strFileName = Server.UrlEncode(strFileName)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                e.Item.Cells(enumAppVen.icVenCode).Text = "<A onclick=""ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','350px');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "v_icon.gif") & "></A>"

            End If
            If txtEstDate.Text = "0" Then txtEstDate.Text = ""

        End If
    End Sub

    Private Sub dtgVDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVDetail.ItemCreated
        Grid_ItemCreated(dtgVDetail, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgVDetail.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Public Sub SortVendorDetail_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgVDetail.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strRedirect As String
        Dim intMsgNo As Integer
        Dim strsql As String
        Dim grdItem As DataGridItem
        Dim strAryQuery(0) As String
        Dim objDb As New EAD.DBCom
        Dim aryList As New ArrayList()
        Dim i As Integer

        strRedirect = dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=AV&pageid=" & strPageId)

        Dim objAdmin As New Admin
        Dim objAdmin_ext As New Admin_Ext
        If strType = "AV" Then

            Dim count1 As Integer
            Dim count2 As Integer
            Dim strMsg1, strMsg2 As String
            Dim TotalstrMsg1 As String = ""
            Dim check1 As Integer

            For Each grdItem In dtgVDetail.Items
                strMsg1 = ""
                strMsg2 = ""
                Dim strVID As String = CType(grdItem.FindControl("lnkVendId"), HyperLink).Text
                'Dim strVID As String = grdItem.Cells(enumAppVen.icVenId).Text
                Dim cboGSTRate As DropDownList
                Dim cboGSTTaxCode As DropDownList
                Dim cboPayMeth As DropDownList
                Dim cboInvoiceInd As DropDownList
                Dim cboPayTerm As DropDownList
                Dim cboGRN As DropDownList

                cboGSTRate = grdItem.Cells(enumAppVen.icGSTRate).FindControl("cboGSTRate")
                cboGSTTaxCode = grdItem.Cells(enumAppVen.icGSTTaxCode).FindControl("cboGSTTaxCode")
                cboPayTerm = grdItem.Cells(enumAppVen.icPayTerm).FindControl("cboPayTerm")
                cboPayMeth = grdItem.Cells(enumAppVen.icPayMth).FindControl("cboPayMeth")
                cboInvoiceInd = grdItem.Cells(enumAppVen.icInvBased).FindControl("cboInvoiceInd")

                If ViewState("blnGRNCtrl") Then
                    cboGRN = grdItem.Cells(enumAppVen.icGRNOutsCtrl).FindControl("cboGRN")
                End If

                If cboPayTerm.SelectedItem.Value = "" Then
                    strMsg1 &= strVID & " - Payment Term"
                End If

                If cboPayMeth.SelectedItem.Value = "" Then
                    If strMsg1 = "" Then
                        strMsg1 &= strVID & " - Payment Method"
                    Else
                        strMsg1 &= ", Payment Method"
                    End If
                End If

                If cboInvoiceInd.SelectedItem.Value = "" Then
                    If strMsg1 = "" Then
                        strMsg1 &= strVID & " - Invoice Based"
                    Else
                        strMsg1 &= ", Invoice Based"
                    End If
                End If

                If cboGSTRate.SelectedItem.Value = "" Then
                    If strMsg1 = "" Then
                        strMsg1 &= strVID & " - GST Rate"
                    Else
                        strMsg1 &= ", GST Rate"
                    End If
                End If

                If cboGSTTaxCode.SelectedItem.Value = "" Then
                    If strMsg1 = "" Then
                        strMsg1 &= strVID & " - GST Tax Code"
                    Else
                        strMsg1 &= ", GST Tax Code"
                    End If
                End If

                Dim txtEstDate As TextBox
                txtEstDate = grdItem.Cells(enumAppVen.icEDD).FindControl("txtEstDate")

                If Not IsNumeric(txtEstDate.Text) And txtEstDate.Text <> "" Then
                    If strMsg1 = "" Then
                        strMsg2 = strVID & " - Invalid Est. Date of Delivery (days)"
                    Else
                        strMsg2 &= "invalid Est. Date of Delivery (days)"
                    End If
                End If
                If strMsg1 <> "" Or strMsg2 <> "" Then
                    If strMsg2 = "" Then
                        strMsg1 &= " is/are required."
                    Else
                        If strMsg1 = "" Then
                            strMsg1 &= strMsg2
                        Else
                            strMsg1 &= " is/are required and " + strMsg2
                        End If
                    End If
                    lblMsg.Visible = True

                Else
                    If ViewState("blnGRNCtrl") Then
                        strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType, cboGRN.SelectedValue, cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                    Else
                        strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType, , cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                    End If
                    Common.Insert2Ary(strAryQuery, strsql)
                    lblMsg.Visible = False

                    'Delete/ Save Vendor Code (Chee Hong - Enhancement, 18/09/2014
                    If Not Session("aryVenCode") Is Nothing Then
                        aryList = Session("aryVenCode")
                        For i = 0 To aryList.Count - 1
                            If strVID = aryList(i)(3) Then
                                strsql = objAdmin_ext.delVendorCode(strVID)
                                Common.Insert2Ary(strAryQuery, strsql)
                                Exit For
                            End If
                        Next

                        For i = 0 To aryList.Count - 1
                            If strVID = aryList(i)(3) Then
                                strsql = objAdmin_ext.upVendorCode(strVID, aryList(i)(0), aryList(i)(1), aryList(i)(2))
                                Common.Insert2Ary(strAryQuery, strsql)
                            End If
                        Next
                    End If
                End If

                If strMsg1 <> "" Then
                    strMsg1 = "<li>" & strMsg1 & "<ul type='disc'></ul></li>"
                    TotalstrMsg1 &= strMsg1
                End If
            Next

            'meilai remark this
            If TotalstrMsg1 = "" Then
                objDb.BatchExecute(strAryQuery)
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                ' ai chu modify on 23/09/2005
                ' no need to bindgrid here coz before end sub will bind again
                ' Bindgrid()
                lblMsg.Visible = False
            Else
                lblMsg.Visible = True
                lblMsg.Text = "<ul type='disc'>" & TotalstrMsg1 & "</ul>"
            End If

        Else
            Dim strMsg, strMsg1 As String
            Dim TotalstrMsg As String = ""
            For Each grdItem In dtgVDetail.Items
                strMsg = ""
                strMsg1 = ""
                'Dim strVID As String = grdItem.Cells(enumAppVen.icVenId).Text
                Dim strVID As String = CType(grdItem.FindControl("lnkVendId"), HyperLink).Text
                Dim chkSelection As CheckBox = grdItem.Cells(enumAppVen.icChk).FindControl("chkSelection")
                ' Dim cboPayTerm As DropDownList
                Dim cboGSTRate, cboGSTTaxCode As DropDownList
                Dim cboPayMeth, cboGRN As DropDownList
                Dim cboInvoiceInd As DropDownList

                cboGSTRate = grdItem.Cells(enumAppVen.icGSTRate).FindControl("cboGSTRate")
                cboGSTTaxCode = grdItem.Cells(enumAppVen.icGSTTaxCode).FindControl("cboGSTTaxCode")
                cboPayTerm = grdItem.Cells(enumAppVen.icPayTerm).FindControl("cboPayTerm") 'e.Item.Cells(4).FindControl("cboPayTerm")
                cboPayMeth = grdItem.Cells(enumAppVen.icPayMth).FindControl("cboPayMeth")

                If ViewState("blnGRNCtrl") Then
                    cboGRN = grdItem.Cells(enumAppVen.icGRNOutsCtrl).FindControl("cboGRN")
                End If

                cboInvoiceInd = grdItem.Cells(enumAppVen.icInvBased).FindControl("cboInvoiceInd")

                If chkSelection.Checked Then
                    Dim vendorid As String = grdItem.Cells(enumAppVen.icCoyName).Text

                    If cboPayTerm.SelectedItem.Value = "" Then
                        strMsg &= strVID & " - Payment Term"
                    End If

                    If cboPayMeth.SelectedItem.Value = "" Then
                        If strMsg = "" Then
                            strMsg &= strVID & " - Payment Method"
                        Else
                            strMsg &= ", Payment Method"
                        End If
                    End If

                    If cboInvoiceInd.SelectedItem.Value = "" Then

                        If strMsg = "" Then
                            strMsg &= strVID & " - Invoice Based"
                        Else
                            strMsg &= ", Invoice Based"
                        End If
                    End If

                    If cboGSTRate.SelectedItem.Value = "" Then
                        If strMsg = "" Then
                            strMsg &= strVID & " - GST Rate"
                        Else
                            strMsg &= ", GST Rate"
                        End If
                    End If

                    If cboGSTTaxCode.SelectedItem.Value = "" Then
                        If strMsg = "" Then
                            strMsg &= strVID & " - GST Tax Code"
                        Else
                            strMsg &= ", GST Tax Code"
                        End If
                    End If

                    Dim txtEstDate As TextBox
                    txtEstDate = grdItem.Cells(enumAppVen.icEDD).FindControl("txtEstDate")

                    If Not IsNumeric(txtEstDate.Text) And txtEstDate.Text <> "" Then
                        If strMsg = "" Then
                            strMsg1 = strVID & " - Invalid Est. Date of Delivery (days)"
                        Else
                            strMsg1 &= "invalid Est. Date of Delivery (days)"
                        End If
                    End If

                    If strMsg <> "" Or strMsg1 <> "" Then
                        If strMsg1 = "" Then
                            strMsg &= " is/are required."
                        Else
                            If strMsg = "" Then
                                strMsg &= strMsg1
                            Else
                                strMsg &= " is/are required and " + strMsg1
                            End If
                        End If

                    Else
                        If ViewState("blnGRNCtrl") Then
                            strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType, cboGRN.SelectedValue, cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                        Else
                            strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType, , cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                        End If
                        Common.Insert2Ary(strAryQuery, strsql)

                        'Delete/ Save Vendor Code (Chee Hong - Enhancement, 18/09/2014
                        If Not Session("aryVenCode") Is Nothing Then
                            aryList = Session("aryVenCode")
                            For i = 0 To aryList.Count - 1
                                If strVID = aryList(i)(3) Then
                                    strsql = objAdmin_ext.delVendorCode(strVID)
                                    Common.Insert2Ary(strAryQuery, strsql)
                                    Exit For
                                End If
                            Next

                            For i = 0 To aryList.Count - 1
                                If strVID = aryList(i)(3) Then
                                    strsql = objAdmin_ext.upVendorCode(strVID, aryList(i)(0), aryList(i)(1), aryList(i)(2))
                                    Common.Insert2Ary(strAryQuery, strsql)
                                End If
                            Next
                        End If
                    End If
                End If

                If strMsg <> "" Then
                    strMsg = "<li>" & strMsg & "<ul type='disc'></ul></li>"
                    TotalstrMsg &= strMsg
                End If
            Next

            '********************************8remarks meilai

            If TotalstrMsg = "" Then
                objDb.BatchExecute(strAryQuery)
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                ' ai chu modify on 23/09/2005
                ' no need to bindgrid here coz before end sub will bind again
                ' Bindgrid()
                lblMsg.Visible = False
            Else
                lblMsg.Visible = True

                lblMsg.Text = "<ul type='disc'>" & TotalstrMsg & "</ul>"
            End If

        End If


        Bindgrid()

        objAdmin = Nothing


    End Sub

    Private Sub fillTaxCode()
        Dim dgItem As DataGridItem
        Dim objGlobal As New AppGlobals
        Dim i As Integer

        For Each dgItem In dtgVDetail.Items
            If CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedValue <> CType(dgItem.FindControl("lblGSTRate"), Label).Text Then
                objGlobal.FillTaxCode(CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList), CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedValue, "P", , False)
                CType(dgItem.FindControl("lblGSTRate"), Label).Text = CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedValue

                'If CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedValue = "EX" Then
                '    CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList).Enabled = False
                'Else
                '    CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList).Enabled = True
                'End If

                If CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList).SelectedValue = "N/A" Then
                    CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList).Enabled = False
                Else
                    CType(dgItem.FindControl("cboGSTTaxCode"), DropDownList).Enabled = True
                End If
            End If
        Next

    End Sub
End Class
