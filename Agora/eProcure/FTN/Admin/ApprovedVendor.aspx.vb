Imports AgoraLegacy
Imports eProcure.Component

Public Class ApprovedVendorFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents txtVID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents dtgVDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cboPayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboPayMeth As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboInvoiceInd As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboGSTRate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboGSTTaxCode As System.Web.UI.WebControls.DropDownList

    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearchAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    ' Protected WithEvents cmdSaveVendor As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label


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
    Dim dvPayTerm, dvPayMethod, dvInvoiceInd As DataView
    Dim txtEDD As TextBox
    Dim strType As String
    Dim strVID As String
    Dim blnGST As Boolean = True
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
        If viewstate("intPageRecordCnt") > 0 Then
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
        Me.dtgVDetail.Columns(10).Visible = False 'Jules
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

        'Jules #6973
        'Dim objGST As New GST
        'blnGST = objgst.chkGSTCOD()

        If Not Page.IsPostBack Then
            '//because No DataGrid display when page first loaded
            cmdSave.Enabled = False

            cmdDelete.Enabled = False
            cmdReset.Disabled = True
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
        Dim objGST As New GST
        Dim ds As New DataSet
        Dim strSelect As String
        Dim strSelectName As String
        'Dim lstItem As New ListItem
        strSelect = txtVID.Text
        strSelectName = txtVName.Text

        ds = objAdmin.searchvendor(strType, strSelect, strSelectName, True, blnGST)
        Dim dvViewVendor As DataView
        dvViewVendor = ds.Tables(0).DefaultView

        dvViewVendor.Sort = viewstate("SortExpression")
        If pSorted Then
            dvViewVendor.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewVendor.Sort += " DESC"
        End If

        If viewstate("action") = "del" Then
            If dtgVDetail.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVDetail.PageSize = 0 Then
                dtgVDetail.CurrentPageIndex = dtgVDetail.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        Dim objGlobal As New AppGlobals
        dvPayTerm = objGlobal.GetCodeTableView(CodeTable.PaymentTerm)
        dvPayMethod = objGlobal.GetCodeTableView(CodeTable.PaymentMethod)
        'dtgVDetail.DataSource = dvViewSample
        'dtgVDetail.DataBind()

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgVDetail, dvViewVendor)
            cmdSave.Enabled = True
            cmdDelete.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            'Michelle (25/11/2010) - for FTN, invoice is on GRN
            'If Session("Env") = "FTN" Then
            '    Me.dtgVDetail.Columns(6).Visible = False
            'End If
            Me.dtgVDetail.Columns(9).Visible = False 'Jules
            dtgVDetail.DataSource = dvViewVendor
            dtgVDetail.DataBind()
        Else

            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            cmdReset.Disabled = True
            dtgVDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If

        If Not blnGST Then
            dtgVDetail.Columns(4).Visible = False
            dtgVDetail.Columns(5).Visible = False
            dtgVDetail.Columns(6).Visible = False
        End If
        ' add for above checking
        viewstate("PageCount") = dtgVDetail.PageCount
        objAdmin = Nothing
    End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.Response.Redirect(dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=AAV&pageid=" & strPageId))


    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim grdItem As DataGridItem
        lblMsg.Visible = False

        For Each grdItem In dtgVDetail.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim objAdmin As New Admin
                Dim vendorid As String = grdItem.Cells(11).Text
                objAdmin.delvendor(vendorid)
                objAdmin = Nothing
            End If
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid()

    End Sub

    Private Sub dtgVDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVDetail.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'e.Item.Cells(1).Text
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim cboPayTerm As DropDownList
            Dim cboPayMeth As DropDownList
            Dim cboInvoiceInd As DropDownList
            Dim lstItem As New ListItem
            Dim txtEstDate As New TextBox
            ''Jules
            Dim cboGSTRate As DropDownList
            Dim cboGSTTaxCode As DropDownList
            Dim objGlobal As New AppGlobals
            ''end.

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If strType = "AV" Then
                ''Jules      
                If blnGST Then               
                    cboGSTRate = e.Item.Cells(5).FindControl("cboGSTRate")
                    objGlobal.FillGST(cboGSTRate)
                    Common.SelDdl(dv("CV_GST_RATE"), cboGSTRate)
                    cboGSTTaxCode = e.Item.Cells(6).FindControl("cboGSTTaxCode")
                    objGlobal.FillTaxCode(cboGSTTaxCode, cboGSTRate.SelectedValue)
                    If e.Item.Cells(4).Text = "" Or e.Item.Cells(4).Text = "&nbsp;" Then
                        cboGSTRate.Items.Insert(0, "N/A")
                        cboGSTRate.SelectedIndex = 0
                        cboGSTTaxCode.Items.Insert(0, "NR")
                        cboGSTTaxCode.SelectedIndex = 0
                        cboGSTRate.Enabled = False
                        cboGSTTaxCode.Enabled = False
                    Else
                        'Dim strVCoyId As String = e.Item.Cells(1).Text
                        'e.Item.Cells(1).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & strVCoyId) & """ ><font color=#0000ff>" & strVCoyId & "</font></A><br>"

                        If cboGSTRate.SelectedIndex > 0 Then
                            Common.SelDdl(dv("CV_GST_TAX_CODE"), cboGSTTaxCode)
                        Else
                            cboGSTRate.SelectedValue = "STD"
                            objGlobal.FillTaxCode(cboGSTTaxCode, cboGSTRate.SelectedValue)
                            cboGSTTaxCode.SelectedValue = "STD"
                        End If
                    End If
                End If
                Dim strVCoyId As String = e.Item.Cells(1).Text
                e.Item.Cells(1).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & strVCoyId) & """ ><font color=#0000ff>" & strVCoyId & "</font></A><br>"
                ''end.

                cboPayTerm = e.Item.Cells(7).FindControl("cboPayTerm") ''Jules
                Common.FillDdl(cboPayTerm, "CODE_DESC", "CODE_ABBR", dvPayTerm)
                Common.SelDdl(dv("cv_payment_term"), cboPayTerm)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayTerm.Items.Insert(0, lstItem)

                cboPayMeth = e.Item.Cells(8).FindControl("cboPayMeth") ''Jules
                Common.FillDdl(cboPayMeth, "CODE_DESC", "CODE_ABBR", dvPayMethod)
                Common.SelDdl(dv("cv_payment_method"), cboPayMeth)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayMeth.Items.Insert(0, lstItem)

                'If Session("Env") <> "FTN" Then
                '    cboInvoiceInd = e.Item.Cells(6).FindControl("cboInvoiceInd")
                '    Common.FillDdl(cboInvoiceInd, "CODE_DESC", "CODE_ABBR", dvInvoiceInd)
                '    Common.SelDdl(dv("CV_BILLING_METHOD"), cboInvoiceInd)
                'End If

                txtEstDate = e.Item.Cells(10).FindControl("txtEstDate") ''Jules
                txtEstDate.Text = Common.parseNull(dv("CV_EDD"))
            Else
                ''Jules                      
                cboGSTRate = e.Item.Cells(5).FindControl("cboGSTRate")
                objGlobal.FillGST(cboGSTRate)
                Common.SelDdl(dv("CV_GST_RATE"), cboGSTRate)
                cboGSTTaxCode = e.Item.Cells(6).FindControl("cboGSTTaxCode")
                objGlobal.FillTaxCode(cboGSTTaxCode, cboGSTRate.SelectedValue)
                If e.Item.Cells(4).Text = "" Or e.Item.Cells(4).Text = "&nbsp;" Then
                    cboGSTRate.Items.Insert(0, "N/A")
                    cboGSTRate.SelectedIndex = 0
                    cboGSTTaxCode.Items.Insert(0, "NR")
                    cboGSTTaxCode.SelectedIndex = 0
                    cboGSTRate.Enabled = False
                    cboGSTTaxCode.Enabled = False
                Else
                    'Dim strVCoyId As String = e.Item.Cells(1).Text
                    'e.Item.Cells(1).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & strVCoyId) & """ ><font color=#0000ff>" & strVCoyId & "</font></A><br>"

                    If cboGSTRate.SelectedIndex > 0 Then
                        Common.SelDdl(dv("CV_GST_TAX_CODE"), cboGSTTaxCode)
                    Else
                        cboGSTRate.SelectedValue = "STD"
                        objGlobal.FillTaxCode(cboGSTTaxCode, cboGSTRate.SelectedValue)
                        cboGSTTaxCode.SelectedValue = "STD"
                    End If
                End If
                Dim strVCoyId As String = e.Item.Cells(1).Text
                e.Item.Cells(1).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & strVCoyId) & """ ><font color=#0000ff>" & strVCoyId & "</font></A><br>"
                ''end.
                cboPayTerm = e.Item.Cells(7).FindControl("cboPayTerm") ''Jules
                Common.FillDdl(cboPayTerm, "CODE_DESC", "CODE_ABBR", dvPayTerm)
                'Common.SelDdl(dv("cv_payment_term"), cboPayTerm)
                Common.SelDdl(ViewState("PaymentTerm"), cboPayTerm)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayTerm.Items.Insert(0, lstItem)

                cboPayMeth = e.Item.Cells(8).FindControl("cboPayMeth") ''Jules
                Common.FillDdl(cboPayMeth, "CODE_DESC", "CODE_ABBR", dvPayMethod)
                'Common.SelDdl(dv("cv_payment_method"), cboPayMeth)
                Common.SelDdl(ViewState("PaymentMethod"), cboPayMeth)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboPayMeth.Items.Insert(0, lstItem)
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

        strRedirect = dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=AV&pageid=" & strPageId)



        Dim objAdmin As New Admin
        If strType = "AV" Then

            Dim count1 As Integer
            Dim count2 As Integer
            Dim strMsg1, strMsg2, strMsg3 As String
            Dim TotalstrMsg1 As String = ""
            Dim check1 As Integer

            For Each grdItem In dtgVDetail.Items
                strMsg1 = ""
                strMsg2 = ""
                strMsg3 = ""
                'Dim strVID As String = grdItem.Cells(1).Text
                Dim strVID As String = grdItem.Cells(11).Text

                Dim cboPayMeth As DropDownList
                Dim cboInvoiceInd As DropDownList
                Dim cboPayTerm As DropDownList

                'Jules 2014.07.14 GST Enhancement --<

                Dim objGST As New GST
                Dim chkGST As String
                Dim cboGSTRate As DropDownList
                Dim cboGSTTaxCode As DropDownList
                If blnGST Then
                    chkGST = objGST.chkGST(grdItem.Cells(11).Text, "", False)
                    If chkGST <> "" Then

                        cboGSTRate = grdItem.Cells(5).FindControl("cboGSTRate")
                        cboGSTTaxCode = grdItem.Cells(6).FindControl("cboGSTTaxCode")

                        If cboGSTRate.SelectedIndex = 0 Or cboGSTTaxCode.SelectedIndex = 0 Then
                            strMsg3 &= strVID & " - GST Rate & GST Tax Code is/are required."
                        End If

                    End If
                End If
                '>--end.

                cboPayTerm = grdItem.Cells(7).FindControl("cboPayTerm") 'e.Item.Cells(4).FindControl("cboPayTerm") 'Jules
                cboPayMeth = grdItem.Cells(8).FindControl("cboPayMeth") 'Jules


                'If Session("Env") <> "FTN" Then
                '    cboInvoiceInd = grdItem.Cells(6).FindControl("cboInvoiceInd")
                'End If
                '      txtEDD = grdItem.Cells(7).FindControl("txtEstDate")

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

                'If Session("Env") <> "FTN" Then
                '    If cboInvoiceInd.SelectedItem.Value = "" Then
                '        If strMsg1 = "" Then
                '            strMsg1 &= strVID & " - Invoice Based"
                '        Else
                '            strMsg1 &= ", Invoice Based"
                '        End If
                '    End If
                'End If

                Dim txtEstDate As TextBox
                txtEstDate = grdItem.Cells(10).FindControl("txtEstDate") 'Jules

                If Not IsNumeric(txtEstDate.Text) And txtEstDate.Text <> "" Then
                    If strMsg1 = "" Then
                        strMsg2 = strVID & " - Invalid Est. Date of Delivery (days)"
                    Else
                        'strMsg &= "<ul type='disc'><li> Invoice Based <ul type='disc'></ul></li>"
                        strMsg2 &= "invalid Est. Date of Delivery (days)"
                    End If
                End If
                If strMsg1 <> "" Or strMsg2 <> "" Or strMsg3 <> "" Then
                    If strMsg2 = "" And strMsg3 = "" Then
                        strMsg1 &= " is/are required."
                    Else
                        If strMsg1 = "" And strMsg3 = "" Then
                            strMsg1 &= strMsg2
                        ElseIf strMsg1 = "" And strMsg2 = "" Then
                            strMsg1 &= strMsg3
                        Else
                            strMsg1 &= " is/are required and " + strMsg2
                            If strMsg3 <> "" Then
                                strMsg1 &= " and " + strMsg3
                            End If
                        End If
                    End If
                    lblMsg.Visible = True

                Else
                    ' strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, strType)
                    'If Session("Env") <> "FTN" Then
                    '    strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType)
                    'Else
                    '    strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType)
                    'End If
                    'Jules 2014.07.14 - GST enhancement --<
                    If blnGST And chkGST <> "" Then
                        strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType, "", cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                    Else
                        strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType)
                    End If
                    '>--end.
                    Common.Insert2Ary(strAryQuery, strsql)
                    lblMsg.Visible = False
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
            Dim count1 As Integer
            Dim count2 As Integer
            'Dim check As Integer
            Dim strMsg, strMsg1, strMsg3 As String
            Dim TotalstrMsg As String = ""
            For Each grdItem In dtgVDetail.Items
                strMsg = ""
                strMsg1 = ""
                strMsg3 = ""
                Dim strVID As String = grdItem.Cells(11).Text
                Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
                ' Dim cboPayTerm As DropDownList
                Dim cboPayMeth As DropDownList
                Dim cboInvoiceInd As DropDownList

                cboPayTerm = grdItem.Cells(7).FindControl("cboPayTerm") 'e.Item.Cells(4).FindControl("cboPayTerm") 'Jules
                cboPayMeth = grdItem.Cells(8).FindControl("cboPayMeth") 'Jules

                'If Session("Env") <> "FTN" Then
                '    cboInvoiceInd = grdItem.Cells(6).FindControl("cboInvoiceInd")
                'End If

                If chkSelection.Checked Then

                    Dim vendorid As String = grdItem.Cells(2).Text

                    '********************meilai 30/12/2004 mandatory field must be selected, for drop down list*******
                    If cboPayTerm.SelectedItem.Value = "" Then
                        'Common.NetMsgbox(Me, "Payment Term, Payment Method, and Invoice Based On Must Be Selected!", MsgBoxStyle.Information, "Wheel")
                        'strMsg &= "<ul type='disc'><li>" & strVID & " - Payment Term<ul type='disc'></ul></li>"
                        'strMsg &= "<ul type='disc'><li>" & strVID & " - Payment Term"
                        strMsg &= strVID & " - Payment Term"
                        'count1 = count1 - 1
                        'lblDisplay.Text = "Payment Term, Payment Method, and Invoice Based On Must Be Selected!"
                        'Exit For
                        'check = 1
                    End If

                    If cboPayMeth.SelectedItem.Value = "" Then
                        If strMsg = "" Then
                            'strMsg &= "<ul type='disc'><li>" & strVID & " - Payment Method, <ul type='disc'></ul></li>"
                            'strMsg &= "<ul type='disc'><li>" & strVID & " - Payment Method"
                            strMsg &= strVID & " - Payment Method"
                        Else
                            'strMsg &= "<ul type='disc'><li>Payment Method, <ul type='disc'></ul></li>"
                            strMsg &= ", Payment Method"
                        End If
                        'check = 1

                    End If

                    Dim objGST As New GST
                    Dim chkGST As String
                    chkGST = objGST.chkGST(grdItem.Cells(11).Text, "", False)
                    If chkGST <> "" Then

                        cboGSTRate = grdItem.Cells(5).FindControl("cboGSTRate")
                        cboGSTTaxCode = grdItem.Cells(6).FindControl("cboGSTTaxCode")

                        If cboGSTRate.SelectedIndex = 0 Or cboGSTTaxCode.SelectedIndex = 0 Then
                            strMsg3 &= strVID & " - GST Rate & GST Tax Code is/are required."
                        End If
                    End If

                    'If Session("Env") <> "FTN" Then
                    '    If cboInvoiceInd.SelectedItem.Value = "" Then

                    '        If strMsg = "" Then
                    '            'strMsg &= "<ul type='disc'><li>" & strVID & " - Invoice Based <ul type='disc'></ul></li>"
                    '            'strMsg &= "<ul type='disc'><li>" & strVID & " - Invoice Based"
                    '            strMsg &= strVID & " - Invoice Based"
                    '        Else
                    '            'strMsg &= "<ul type='disc'><li> Invoice Based <ul type='disc'></ul></li>"
                    '            strMsg &= ", Invoice Based"
                    '        End If
                    '        'check = 1
                    '    End If
                    'End If

                    Dim txtEstDate As TextBox
                    txtEstDate = grdItem.Cells(10).FindControl("txtEstDate") 'Jules

                    If Not IsNumeric(txtEstDate.Text) And txtEstDate.Text <> "" Then
                        If strMsg = "" Then
                            strMsg1 = strVID & " - Invalid Est. Date of Delivery (days)"
                        Else
                            'strMsg &= "<ul type='disc'><li> Invoice Based <ul type='disc'></ul></li>"
                            strMsg1 &= "invalid Est. Date of Delivery (days)"
                        End If
                    End If

                    'If check = 1 Then
                    If strMsg <> "" Or strMsg1 <> "" Or strMsg3 <> "" Then
                        If strMsg1 = "" And strMsg3 = "" Then
                            strMsg &= " is/are required."
                        Else
                            If strMsg = "" And strMsg3 = "" Then
                                strMsg &= strMsg1
                            ElseIf strMsg = "" And strMsg1 = "" Then
                                strMsg &= strMsg3
                            Else
                                strMsg &= " is/are required and " + strMsg1
                                If strMsg3 <> "" Then
                                    strMsg1 &= " and " + strMsg3
                                End If
                            End If
                        End If
                    Else
                        'If Session("Env") <> "FTN" Then
                        '    strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, cboInvoiceInd.SelectedValue, Val(txtEstDate.Text), strType)
                        'Else
                        '    strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType)
                        'End If
                        If chkGST <> "" Then
                            strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType, "", cboGSTRate.SelectedValue, cboGSTTaxCode.SelectedValue)
                        Else
                            strsql = objAdmin.updvendor(strVID, cboPayTerm.SelectedValue, cboPayMeth.SelectedValue, "GRN", Val(txtEstDate.Text), strType)
                        End If
                        Common.Insert2Ary(strAryQuery, strsql)
                    End If
                End If
                'count2 = count2 + 1
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

    'Jules 2014.07.10
    Public Sub cboGSTRate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGSTRate.SelectedIndexChanged
        Dim dtgItem As DataGridItem = DirectCast(DirectCast(sender, Control).NamingContainer, DataGridItem)
        Dim cboGSTRate As DropDownList = dtgItem.FindControl("cboGSTRate")
        Dim cboGSTTaxCode As DropDownList = dtgItem.FindControl("cboGSTTaxCode")

        If cboGSTRate.SelectedIndex > 0 Then
            Dim objGlobal As New AppGlobals
            objGlobal.FillTaxCode(cboGSTTaxCode, cboGSTRate.SelectedValue)
            If cboGSTTaxCode.SelectedValue = "N/A" And cboGSTRate.SelectedValue = "EX" Then
                cboGSTTaxCode.Enabled = False
            Else
                cboGSTTaxCode.Enabled = True
            End If
        End If
    End Sub
End Class
