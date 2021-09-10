Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing

'If ViewState("type") = "new" Then
'    Select Case ViewState("mode")
'        Case "bc"
'        Case "cc"
'    End Select
'ElseIf ViewState("type") = "mod" Then
'    Select Case ViewState("mode")
'        Case "bc"
'        Case "cc"
'    End Select
'End If

Public Class RaisePR
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumShoppingCart
        icChk = 0
        icNo = 1

        'Jules 2018.05.03 - PAMB Scrum 2 - Gift Purchase and Analysis Codes
        icGift = 2
        icGift1 = 3
        icFundType = 4
        icPersonCode = 5
        icProjectCode = 6
        icProductCode = 7
        icVendorItemCode = 8
        icGLCode = 9 '4
        icGLCode1 = 10 '5
        icCategoryCode1 = 11 '6
        icAssetGroup = 12 '7
        icItemDesc = 13 '8 '7
        icQty = 14 '9 '8
        icCommodity = 15 '10 '9
        icUOM = 16 '11 '10
        icCurrency = 17 '12 '11
        icPrice = 18 '13 '12
        icTotal = 19 '14 '13
        icTaxRate = 20 '15
        icTax = 21 '16 '15 '14
        icTaxCode = 22 '17
        icSource = 23 '18 '17 '16 '15
        icCategoryCode = 24 '19 '18 '17 '16
        icVendor = 25 '20 '19 '18 '17
        icBudget = 26 '21 '20 '19 '18
        icDelivery = 27 '22 '21 '20 '19
        icEstDate = 28 '23 '22 '21 '20
        icWarranty = 29 '24 '23 '22 '21
        icRemark = 30 '25 '24 '23 '22
        'End modification.
    End Enum

    Dim dvwDelivery, dvwGLCode, dvwCategoryCode As DataView
    Dim dvwCustom() As DataView
    Dim dvwCus As New DataView
    Dim strCustomDefault() As String
    Dim strDeliveryDefault As String
    Dim intRow As Integer
    Dim intCnt As Integer
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim dvwCustomItem As DataView
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim blnItem As Boolean
    Dim i As Integer
    Dim blnValid As Boolean
    Dim blnValidBill As Boolean
    Dim strDefDelivery As String
    Dim dtBCM, dtGLCode, dtCategoryCode As DataTable
    Dim objGLO As New AppGlobals
    Dim dtFundType, dtPersonCode, dtProjectCode As DataTable 'Jules 2018.10.18

    Protected WithEvents hidDelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Extra1 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Extra2 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Extra3 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents body1 As HtmlGenericControl
    Protected WithEvents hidClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotalClientId As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents GSTLabel As System.Web.UI.HtmlControls.HtmlTableCell

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryProdCodeNew As New ArrayList

    Dim keepAr As New ArrayList
    Dim keepArPost As New ArrayList
    Dim strQtyErr As String

    Dim kk As Integer = 0

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

    Private Property DynamicColumnAdded() As Boolean
        Get
            If ViewState("ColumnAdded") Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ColumnAdded") = Value
        End Set
    End Property

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        If Me.DynamicColumnAdded Then
            Me.addDataGridColumn()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        alButtonList.Add(cmdRaise)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadInt)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        alButtonList.Add(cmdRaise)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadInt)
        alButtonList.Add(cmdRemove)
        alButtonList.Add(cmdDupPRLine)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdRaise.Enabled = blnCanUpdate And blnCanAdd And ViewState("blnCmdRaise")
        cmdSetup.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdSetup")
        cmdAdd.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdAdd")
        cmdUpload.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdUpload")
        cmdUploadInt.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdUploadInt")
        cmdRemove.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDupPRLine.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDelete.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        alButtonList.Clear()
        buildjava()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)

        Response.Expires = -1
        Response.AddHeader("cache-control", "private")
        Response.AddHeader("pragma", "no-cache")
        Response.CacheControl = "no-cache"

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgShopping)

        ViewState("blnCutPR") = False

        Dim dsBCM As New DataSet
        Dim objBudget As New BudgetControl
        Dim objPR As New PR

        ViewState("BCM") = CInt(objPR.checkBCM)
        'ViewState("BCM") = 1
        If ViewState("BCM") > 0 Then
            dsBCM = objBudget.getBCMListByUserNew(Session("UserId"))
            dtBCM = dsBCM.Tables(0)
        End If

        'Jules 2018.10.18
        Dim objAC As New IPP
        Dim dsFundType, dsPersonCode, dsProjectCode, dsGLCode As New DataSet
        dsFundType = objAC.GetAnalysisCode("", "", "L1", "O", "eProcure")
        dtFundType = New DataTable
        dtFundType.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtFundType = dsFundType.Tables(0)

        dsPersonCode = objAC.GetAnalysisCode("", "", "L9", "O", "eProcure")
        dtPersonCode = New DataTable
        dtPersonCode.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtPersonCode = dsPersonCode.Tables(0)

        dsProjectCode = objAC.GetAnalysisCode("", "", "L8", "O", "eProcure")
        dtProjectCode = New DataTable
        dtProjectCode.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtProjectCode = dsProjectCode.Tables(0)

        Dim strSQL As String
        strSQL = "Select CBG_B_GL_CODE as 'GLCODE',  " &
                    objDB.Concat("", "", "CBG_B_GL_DESC", objDB.Concat(" (", ")", "CBG_B_GL_CODE")) & " as DESCRIPTION " &
                    " from COMPANY_B_GL_CODE where CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' order by  CBG_B_GL_CODE "

        dsGLCode = objDB.FillDs(strSQL)
        If dsGLCode IsNot Nothing Then
            dtGLCode = New DataTable
            dtGLCode.Columns.Add("GLCODE", Type.GetType("System.String"))
            dtGLCode.Columns.Add("DESCRIPTION", Type.GetType("System.String"))
            dtGLCode = dsGLCode.Tables(0)
        End If
        'End modification.

        If Session("CurrentScreen") <> "RemoveItem" Then
            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing

            'Jules 2018.07.30
            If ViewState("_isDuplicated") Is Nothing Then
                ViewState("strDuplicatedItem") = Nothing
                ViewState("strDuplicatedCustItem") = Nothing
            End If
            'End modification.
        End If

        If Not IsPostBack Then
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr

            Session("CurrentScreen") = ""

            GenerateTab()

            If Request.QueryString("frm") = "BuyerCat" Then
                'Construct URL For BC
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=BuyerCat&pageid=" & strPageId)
            ElseIf Request.QueryString("frm") = "ConCat" Then
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=ConCat&pageid=" & strPageId)
            Else
                'Construct URL For My PR
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & Request.QueryString("index") & "&prid=" & Request.QueryString("prid") & "&type=" & Request.QueryString("type") & "&mode=" & Request.QueryString("mode"))
            End If

            aryProdCodeNew.Clear()

            ViewState("strPageId") = strPageId
            ViewState("blnCmdAdd") = True
            ViewState("blnCmdRaise") = True
            ViewState("blnCmdUpload") = True
            ViewState("blnCmdUploadInt") = True
            ViewState("blnCmdRemove") = True
            ViewState("blnCmdSetup") = True
            ViewState("postback") = 0
            ViewState("SuppId") = ""

            If Request.QueryString("frm") = "BuyerCat" Then
                ViewState("type") = "new"
                ViewState("mode") = "bc"
            ElseIf Request.QueryString("frm") = "ConCat" Then
                ViewState("type") = "new"
                ViewState("mode") = "cc"
            ElseIf Not Request.QueryString("type") = "" Then
                ViewState("type") = Request.QueryString("type")
                ViewState("mode") = Request.QueryString("mode")
            End If
            If Request.QueryString("frm") = "bc" Then
                Session("ProdList") = Nothing
                Session("urlreferer") = "Dashboard"
                cmdDelete.Visible = False
            End If
            ViewState("strPageId") = strPageId
            lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)

            If Session("urlreferer") = "BuyerCatSearch" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "ConCatSearch" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "PRAll" Then
                lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            End If

            'Dim dsBCM As New DataSet
            'Dim objBudget As New BudgetControl
            'Dim objPR As New PR

            ' _Yap : Change to outside postback for reload page on Add Item
            'ViewState("BCM") = CInt(objPR.checkBCM)
            ''ViewState("BCM") = 1
            'If ViewState("BCM") > 0 Then
            '    dsBCM = objBudget.getBCMListByUserNew(Session("UserId"))
            '    dtBCM = dsBCM.Tables(0)
            'End If

            Dim objAdmin As New Admin
            Dim dsDelivery As New DataSet
            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add_ByDefault("D")
            ViewState("strDeliveryDefault") = strDeliveryDefault

            If ViewState("type") = "new" Then
                lblTitle.Text = "Raise Purchase Request"
                lblPRNo.Text = "To Be Allocated By System"
                Dim PR_Req_Name As String = objDB.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                txtRequestedName.Text = PR_Req_Name

                Dim PR_Req_Tel As String = objDB.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                txtRequestedContact.Text = PR_Req_Tel

                chkCustomPR.Checked = True
                chkRemarkPR.Checked = True

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                cmdDelete.Visible = True
                ViewState("blnCmdDelete") = True
                ViewState("prid") = Request.QueryString("prid")
                lblPRNo.Text = ViewState("prid")
                hidNewPR.Value = ViewState("prid")
                lblTitle.Text = "Raise Purchase Request"

                Session("ProdList") = aryProdCodeNew

                Dim PR_Type As String = objDB.GetVal("SELECT IFNULL(PRM_PR_TYPE,'') FROM pr_mstr WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ViewState("prid") & "'")
                If PR_Type = "CC" Then
                    ViewState("mode") = "cc"
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            'If Session("Env") <> "FTN" Then
            '    addDataGridColumn()
            '    Session("NonFTN") = True
            'Else
            '    Session("NonFTN") = False
            'End If
            addDataGridColumn()
            Session("NonFTN") = True

            Bindgrid()
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            cmdUploadInt.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            cmdRaise.Attributes.Add("onclick", "return InitialValidation();")
            cmdSubmit.Attributes.Add("onclick", "return InitialValidation();")
        Else
            If Session("CurrentScreen") = "AddItem" Then
                'Dim dtrd As DataRow
                Dim chkItem As CheckBox
                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
                Dim txtBudget, txtDelivery As Label
                'Dim cboGLCode As DropDownList 'Jules 2018.10.22 U00019
                Dim strItem, strItemCust As New ArrayList()
                Dim dgItem As DataGridItem
                Dim k As Integer = 0
                Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2; Jules 2018.10.18
                Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    chkItem = dgItem.FindControl("chkSelection")

                    'Jules 2018.05.04 - PAMB Scrum 2
                    cboGift = dgItem.FindControl("cboGift")

                    'Jules 2018.10.18
                    'cboFundType = dgItem.FindControl("cboFundType")
                    'cboPersonCode = dgItem.FindControl("cboPersonCode")
                    'cboProjectCode = dgItem.FindControl("cboProjectCode")
                    hidFundType = dgItem.FindControl("hidFundType")
                    hidPersonCode = dgItem.FindControl("hidPersonCode")
                    hidProjectCode = dgItem.FindControl("hidProjectCode")
                    hidGLCode = dgItem.FindControl("hidGLCode")
                    'End modification.

                    If chkItem.Checked Then
                        If Session("CurrentScreen") = "AddItem" Then
                            'Jules 2018.05.04 - PAMB Scrum 2
                            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                            'End modification.
                        End If
                    Else
                        'Jules 2018.05.04 - PAMB Scrum 2
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                        strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                        'End modification.
                    End If

                    Dim objAdmin As New Admin

                    dvwCus = objAdmin.getCustomField("")

                    If Not dvwCus Is Nothing Then
                        For i = 0 To dvwCus.Count - 1
                            'For Each dgItem2 In dtgShopping.Items
                            Dim cboCustom As DropDownList
                            cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                            If chkItem.Checked Then
                                If Session("CurrentScreen") = "AddItem" Then
                                    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If
                            Else
                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            End If
                            'Next
                        Next
                    End If

                    k += 1
                Next
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust

                Bindgrid()
                Session("CurrentScreen") = ""
            End If
        End If

        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); calculateAllIndividualTotal();")
        ViewState("body_loaditemcreated") = ""

        cmdRemove.Attributes.Add("onclick", "return RemoveItemCheck();")
        cmdDupPRLine.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

        txtInternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        txtExternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        displayAttachFile()
        displayAttachFileInt()

        Session("keepItem") = ""
        Session("keepAr") = New ArrayList()

        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtgShopping.Columns(EnumShoppingCart.icAssetGroup).Visible = False
        End If

        'Select Case ViewState("mode")
        '    Case "cc"
        '        dtgShopping.Columns(EnumShoppingCart.icTaxRate).Visible = True
        '        dtgShopping.Columns(EnumShoppingCart.icTax).Visible = True
        'End Select

        Dim objCheckGST As New GST
        ViewState("CheckGST") = objCheckGST.chkGSTCOD()
        If ViewState("CheckGST") = True Then
            GSTLabel.InnerText = "SST Amount :"
        Else
            GSTLabel.InnerText = "Tax:"
            dtgShopping.Columns(EnumShoppingCart.icTaxRate).Visible = False
        End If
        objCheckGST = Nothing
    End Sub

    Private Sub cmdDupPRLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDupPRLine.Click
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strscript As New System.Text.StringBuilder
        ViewState("_isDuplicated") = "Yes" 'Jules 2018.07.30
        If ViewState("type") = "new" Then
            For Each dgItem In dtgShopping.Items
                chkItem = dgItem.FindControl("chkSelection")

                Dim txtCommodity As TextBox
                txtCommodity = dgItem.FindControl("txtCommodity")

                If chkItem.Checked Then

                    Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                    aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, CType(dgItem.FindControl("lblUOM"), Label).Text, txtCommodity.Text, CType(dgItem.FindControl("lblCDGroup"), Label).Text})
                    Session("ProdList") = aryProdCodeCurrent

                    'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                    'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                    Exit For
                End If
            Next
            Select Case ViewState("mode")
                Case "bc", "cc"
            End Select

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim txtBudget, txtDelivery As Label
            'Dim cboGLCode As DropDownList 'Jules 2018.10.22 U00019
            'Dim cboGift, cboFundType, cboPersonCode, cboProjectCode, cboCategoryCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2
            Dim cboGift, cboCategoryCode As DropDownList 'Jules 2018.10.18
            Dim strItem, strItemCust, strDuplicatedItem, strDuplicatedItemCust As New ArrayList() 'Jules 2018.07.30
            Dim txtGSTAmt As TextBox 'Jules 2018.07.30
            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

            'Dim dgItem As DataGridItem
            Dim k As Integer = 0

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                chkItem = dgItem.FindControl("chkSelection")

                'Jules 2018.05.04 - PAMB Scrum 2
                cboGift = dgItem.FindControl("cboGift")

                'Jules 2018.10.18
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                hidGLCode = dgItem.FindControl("hidGLCode")
                'End modification.

                'If chkItem.Checked Then
                'Else
                '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text})
                'End If

                'Jules 2018.05.04 - PAMB Scrum 2                
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex, cboCategoryCode.SelectedIndex})
                strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text, cboCategoryCode.SelectedIndex}) 'Jules 2018.10.18
                'End modification.

                'Jules 2018.07.30
                If chkItem.Checked Then
                    'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex, cboCategoryCode.SelectedIndex, txtGSTAmt.Text})
                    strDuplicatedItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text, cboCategoryCode.SelectedIndex, txtGSTAmt.Text}) 'Jules 2018.10.18
                End If

                Dim objAdmin As New Admin

                dvwCus = objAdmin.getCustomField("")

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        'For Each dgItem2 In dtgShopping.Items
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        'If chkItem.Checked Then
                        'Else
                        '    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        'End If
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        'Next

                        'Jules 2018.07.30
                        If chkItem.Checked Then
                            strDuplicatedItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
                    Next
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust

            'Jules 2018.07.30
            ViewState("strDuplicatedItem") = strDuplicatedItem
            ViewState("strDuplicatedCustItem") = strDuplicatedItemCust

            Bindgrid()
        ElseIf ViewState("type") = "mod" Then
            For Each dgItem In dtgShopping.Items
                chkItem = dgItem.FindControl("chkSelection")

                Dim txtCommodity As TextBox
                txtCommodity = dgItem.FindControl("txtCommodity")

                If chkItem.Checked Then
                    Session("CurrentScreen") = "AddItem"
                    Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                    aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, CType(dgItem.FindControl("lblUOM"), Label).Text, txtCommodity.Text, CType(dgItem.FindControl("lblCDGroup"), Label).Text})
                    Session("ProdList") = aryProdCodeCurrent

                    'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                    'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                    Exit For
                End If
            Next
            Select Case ViewState("mode")
                Case "cc"
            End Select
            strscript.Append("<script language=""javascript"">")
            strscript.Append("document.getElementById('btnHidden1').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script14", strscript.ToString())
            'btnHidden1_Click(sender, e)

            'Jules 2018.07.30
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt As TextBox
            Dim txtBudget, txtDelivery As Label

            'Jules 2018.10.18
            'Dim cboGLCode, cboGift, cboFundType, cboPersonCode, cboProjectCode, cboCategoryCode As DropDownList
            'Dim cboGLCode, cboGift, cboCategoryCode As DropDownList
            Dim cboGift, cboCategoryCode As DropDownList 'Jules 2018.10.22 U00019
            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox
            'End modification.

            Dim strItem, strItemCust, strDuplicatedItem, strDuplicatedItemCust As New ArrayList()
            Dim k As Integer = 0

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                chkItem = dgItem.FindControl("chkSelection")
                cboGift = dgItem.FindControl("cboGift")

                'Jules 2018.10.18
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                hidGLCode = dgItem.FindControl("hidGLCode")
                'End modification.

                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18


                If chkItem.Checked Then
                    'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex, cboCategoryCode.SelectedIndex, txtGSTAmt.Text})
                    strDuplicatedItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text, cboCategoryCode.SelectedIndex, txtGSTAmt.Text}) 'Jules 2018.10.18
                End If

                Dim objAdmin As New Admin

                dvwCus = objAdmin.getCustomField("")

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})

                        If chkItem.Checked Then
                            strDuplicatedItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
                    Next
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            ViewState("strDuplicatedItem") = strDuplicatedItem
            ViewState("strDuplicatedCustItem") = strDuplicatedItemCust
        End If

        'For Each dgItem In dtgShopping.Items
        '    chkItem = dgItem.FindControl("chkSelection")
        '    If chkItem.Checked Then
        '        intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
        '        strMsg = objPR.DupPRItem(hidNewPR.Value, intLine)
        '        Exit For
        '    End If
        'Next

        'If strMsg = "1" Then
        '    'hidCost.Value = objPR.updatePOCost(hidNewPO.Value)
        '    Common.NetMsgbox(Me, "Item(s) Duplicated.", MsgBoxStyle.Information)
        '    Dim dsDelivery As New DataSet
        '    Dim objAdmin As New Admin
        '    Dim objBudget As New BudgetControl
        '    dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
        '    dvwDelivery = dsDelivery.Tables(0).DefaultView
        '    strDeliveryDefault = objAdmin.user_Default_Add("D")

        '    Dim dsBCM As New DataSet
        '    dsBCM = objBudget.getBCMListByUser(Session("UserId"))
        '    dtBCM = dsBCM.Tables(0)
        '    Bindgrid()
        '    hidTotal.Value = ""
        '    'addDGTotal()
        '    hid2.Value = ""
        '    objAdmin = Nothing
        '    objBudget = Nothing
        'End If
        objPR = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1) As String
        Dim objShopping As New ShoppingCart
        Dim dsItem, dsTemp As New DataSet
        Dim dvViewSample As DataView
        Dim aryProdCode As New ArrayList
        Dim strProdList As String = ""
        Dim strItemHead As New ArrayList()

        If ViewState("type") = "new" Then

            strProdList = "''"
            dsItem = objShopping.getPRItemList("BuyerCat", strProdList, "")
            aryProdCode = Session("ProdList")

            Select Case ViewState("mode")
                Case "bc"
                    If IsNothing(aryProdCode) = False Then
                        For i = 0 To aryProdCode.Count - 1
                            If strProdList = "" Then
                                strProdList = "'" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping.getPRItemList("BuyerCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            Else
                                strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            End If
                        Next
                    End If

                    displayAttachFile()
                    displayAttachFileInt()
                Case "cc"
                    For i = 0 To aryProdCode.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        Else
                            strProdList &= ", '" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        End If

                    Next

                    displayAttachFile()
                    displayAttachFileInt()
            End Select

            If Session("strItemHead") IsNot Nothing Then
                strItemHead = Session("strItemHead")
                txtAttention.Text = strItemHead(0)(0)
                txtInternal.Text = strItemHead(0)(1)
                txtExternal.Text = strItemHead(0)(2)
                chkUrgent.Checked = strItemHead(0)(3)
            End If

            ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
            dvViewSample = dsItem.Tables(0).DefaultView

        ElseIf ViewState("type") = "mod" Then
            strProdList = "''"
            dsItem = objShopping.getPRItemList("PR", "", ViewState("prid"))
            If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then

                aryProdCode = Session("ProdList")
                If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                    Select Case ViewState("mode")
                        Case "bc"
                            For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping.getPRItemList("BuyerCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                                Else
                                    strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                                End If
                            Next
                        Case "cc"
                            For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                                Else
                                    strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                                End If
                            Next
                    End Select
                End If

                Session("keepAr") = keepAr
                If Not strProdList = "" Then
                    Session("keepItem") = strProdList
                End If
            End If

            If Not Page.IsPostBack Then
                keepArPost = Session("keepAr")
                If Not IsNothing(keepArPost) Then
                    If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                        Select Case ViewState("mode")
                            Case "bc"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping.getPRItemList("BuyerCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                            Case "cc"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping.getPRItemList("ConCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3), keepArPost(i)(4))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                        End Select
                    End If

                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                End If

                If dsItem.Tables(0).Rows.Count > 0 Then
                    txtRequestedName.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_NAME"))
                    txtRequestedContact.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_PHONE"))
                    lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("PRM_CREATED_DATE"))
                    txtAttention.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_ATTN"))
                    txtInternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_INTERNAL_REMARK"))
                    txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_EXTERNAL_REMARK"))

                    If Session("strItemHead") IsNot Nothing Then
                        strItemHead = Session("strItemHead")
                        txtAttention.Text = strItemHead(0)(0)
                        txtInternal.Text = strItemHead(0)(1)
                        txtExternal.Text = strItemHead(0)(2)
                        chkUrgent.Checked = strItemHead(0)(3)
                    End If

                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PRINT_CUSTOM_FIELDS")) = "1" Then
                        chkCustomPR.Checked = True
                    Else
                        chkCustomPR.Checked = False
                    End If
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PRINT_REMARK")) = "1" Then
                        chkRemarkPR.Checked = True
                    Else
                        chkRemarkPR.Checked = False
                    End If
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_URGENT")) = "1" Then
                        chkUrgent.Checked = True
                    Else
                        chkUrgent.Checked = False
                    End If
                End If
            End If
            ViewState("intPageRecordCnt") = dsItem.Tables(1).Rows.Count
            dvViewSample = dsItem.Tables(1).DefaultView
            dvwCustomItem = dsItem.Tables(2).DefaultView

            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
        intRow = 0

        dtgShopping.DataSource = dvViewSample
        dtgShopping.DataBind()
        objShopping = Nothing

        'If Session("Env") = "FTN" Then
        '    Me.dtgShopping.Columns(14).Visible = False
        '    Me.dtgShopping.Columns(17).Visible = False
        '    Extra1.Style.Item("display") = "none"
        '    Extra2.Style.Item("display") = "none"
        '    Extra3.Style.Item("display") = "none"
        'Else
        '    Me.dtgShopping.Columns(14).Visible = True
        '    Me.dtgShopping.Columns(17).Visible = True
        '    Extra1.Style.Item("display") = ""
        '    Extra2.Style.Item("display") = ""
        '    Extra3.Style.Item("display") = ""
        'End If

        'Me.dtgShopping.Columns(16).Visible = True
        Select Case ViewState("mode")
            Case "cc"
                dtgShopping.Columns(EnumShoppingCart.icTaxRate).Visible = True
                dtgShopping.Columns(EnumShoppingCart.icTax).Visible = True
        End Select
        Me.dtgShopping.Columns(EnumShoppingCart.icBudget).Visible = True

        Extra1.Style.Item("display") = ""
        Extra2.Style.Item("display") = ""
        Extra3.Style.Item("display") = ""
    End Function

    Private Function encodeCustomField(ByVal strValue As String) As String
        'strValue = strValue.Replace(" ", "_")        
        strValue = Server.UrlEncode(strValue)
        encodeCustomField = strValue
    End Function

    Private Function decodeCustomField(ByVal strValue As String) As String
        'strValue = strValue.Replace("_", " ")
        strValue = Server.UrlDecode(strValue)
        decodeCustomField = strValue
    End Function

    Private Sub dtgShopping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmt, dblGstAmt As Double

            Dim objAdmin As New Admin
            Dim sClientId As String, sTotalClient As String

            Dim GLCodelstItem As New ListItem
            Dim CategoryCodelstItem As New ListItem

            Dim strItem, strDuplicatedItem As New ArrayList() 'Jules 2018.07.30
            Dim strItemCust As New ArrayList()

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'simply use one of the client id to get all client ID
            sTotalClient = hidClientId.Value
            sClientId = Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1, InStr(Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1), "_") - 1) & "|"
            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            Dim txtBudget As Label
            txtBudget = e.Item.FindControl("txtBudget")
            txtBudget.Text = ""
            txtBudget.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidBudgetCode As TextBox
            hidBudgetCode = e.Item.FindControl("hidBudgetCode")
            hidBudgetCode.Text = ""

            Dim txtDelivery As Label
            txtDelivery = e.Item.FindControl("txtDelivery")
            txtDelivery.Text = strDefDelivery
            txtDelivery.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidDelCode As TextBox
            hidDelCode = e.Item.FindControl("hidDelCode")
            hidDelCode.Text = ""

            Dim lblProductCode As Label
            lblProductCode = e.Item.FindControl("lblProductCode")
            lblProductCode.Text = Common.parseNull(dv("PRODUCTCODE"))

            Dim lblGLCode As Label
            lblGLCode = e.Item.FindControl("lblGLCode")
            lblGLCode.Text = Common.parseNull(dv("GLCODE"))

            Dim lblCategoryCode As Label
            lblCategoryCode = e.Item.FindControl("lblCategoryCode")
            lblCategoryCode.Text = Common.parseNull(dv("CATEGORYCODE"))

            Dim lblCDGroup As Label
            lblCDGroup = e.Item.FindControl("lblCDGroup")
            lblCDGroup.Text = Common.parseNull(dv("CDM_GROUP_INDEX"))

            Dim lblVendor As Label
            lblVendor = e.Item.FindControl("lblVendor")
            lblVendor.Text = Common.parseNull(dv("VENDOR"))

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumShoppingCart.icVendorItemCode).FindControl("VENDORITEMCODE")
            lnkCode.NavigateUrl = "javascript:;"
            Session("UrlLocation") = "BuyerCatalogueSearch"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & lblProductCode.Text & "&index=&draft=0") & "')")
            lnkCode.Text = dv("VENDORITEMCODE")

            Dim lblVENDORITEMCODE As Label
            lblVENDORITEMCODE = e.Item.FindControl("lblVENDORITEMCODE")
            lblVENDORITEMCODE.Text = Common.parseNull(dv("VENDORITEMCODE"))

            Dim lblProductDesc As TextBox
            lblProductDesc = e.Item.FindControl("lblProductDesc")
            lblProductDesc.Text = Common.parseNull(dv("PRODUCTDESC"))
            lblProductDesc.Width = System.Web.UI.WebControls.Unit.Pixel(250)
            'lblProductDesc.Height = System.Web.UI.WebControls.Unit.Pixel(250)

            lblProductDesc.Attributes.Add("onKeyDown", "limitText (this, 500);")
            lblProductDesc.Attributes.Add("style", "background-color:Transparent; overflow:hidden; width:190px")


            Dim charRows, chars, remaining As Integer
            Dim tbCOntent As String
            charRows = 0
            chars = 0
            tbCOntent = lblProductDesc.Text
            lblProductDesc.Columns = 30
            chars = tbCOntent.Length
            charRows = chars / lblProductDesc.Columns
            remaining = chars - charRows * lblProductDesc.Columns
            If (remaining = 0) Then
                lblProductDesc.Rows = charRows
                lblProductDesc.TextMode = TextBoxMode.MultiLine
            Else
                lblProductDesc.Rows = charRows + 1
                lblProductDesc.TextMode = TextBoxMode.MultiLine
            End If

            If lblProductCode.Text <> "" And lblProductCode.Text <> "&nbsp;" Then
                lblProductDesc.ReadOnly = True
                lblProductDesc.Attributes.Add("style", "background-color:Transparent; overflow:hidden; border:none; width:190px")
            Else
                lblProductDesc.ReadOnly = False
                lblProductDesc.Attributes.Add("style", "overflow:hidden; width:190px")
            End If



            Dim lblUOM As Label
            lblUOM = e.Item.FindControl("lblUOM")
            lblUOM.Text = Common.parseNull(dv("UOM"))

            Dim lblCurrency As Label
            lblCurrency = e.Item.FindControl("lblCurrency")
            lblCurrency.Text = Common.parseNull(dv("CURRENCY"))

            e.Item.Cells(EnumShoppingCart.icNo).Text = intRow + 1
            Dim strSCoyId As String = ""
            Try
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "bc"
                            If Not IsDBNull(e.Item.DataItem("Supplierid")) Then strSCoyId = e.Item.DataItem("Supplierid").ToString
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
                End If

            Catch ex As Exception
            End Try
            e.Item.Attributes.Add("SuppId", strSCoyId)
            ViewState("SuppId") = strSCoyId

            Dim hidItemLine As HtmlInputHidden
            hidItemLine = e.Item.FindControl("hidItemLine")
            hidItemLine.Value = intRow + 1

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = e.Item.FindControl("txtQty")
            txtQty.Text = Common.parseNull(dv("QUANTITY"))

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty")
            'revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$"
            'revQty.ValidationExpression = "^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$"
            'revQty.ValidationExpression = "^[0-9]{1,6}(\.[0-9]{1,2})?$"
            revQty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"
            revQty.ControlToValidate = "txtQty"
            'revQty.ErrorMessage = "Invalid quantity"
            revQty.ErrorMessage = ViewState("ValQtyMsg") '"Invalid quantity. Range should be from 0.01 to 999999.99"
            revQty.Text = "?"
            revQty.Display = ValidatorDisplay.Dynamic

            'Dim compValQty As CompareValidator
            'compValQty = e.Item.FindControl("compValQty")
            'compValQty.ValueToCompare = 0
            'compValQty.Operator = ValidationCompareOperator.NotEqual
            'compValQty.ControlToValidate = "txtQty"
            'compValQty.ErrorMessage = "Invalid quantity. Range should be from 0.01 to 999999.99"
            'compValQty.Text = "?"
            'compValQty.Display = ValidatorDisplay.Dynamic

            Dim appGlobal As New AppGlobals
            Dim ddl_uom As New DropDownList
            Dim dv_uom As DataView
            ddl_uom = e.Item.FindControl("ddl_uom")

            dv_uom = appGlobal.GetCodeTableView(CodeTable.Uom)
            Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)

            Common.SelDdl(dv("UOM"), ddl_uom, False, True)

            'Dim ddl_comm As New DropDownList
            'ddl_comm = e.Item.FindControl("ddl_comm")
            'appGlobal.FillCommodityType(ddl_comm)

            'Common.SelDdl(Common.parseNull(dv("COMMODITY")), ddl_comm, False, True)

            Populate(intRow)

            Dim txtPrice As TextBox
            txtPrice = e.Item.FindControl("txtPrice")
            If IsDBNull(dv("UNITCOST")) Then
                txtPrice.Text = ""
            Else
                txtPrice.Text = Format(dv("UNITCOST"), "###,###,##0.0000")
            End If

            Dim lblNoTax As Label
            lblNoTax = e.Item.FindControl("lblNoTax")

            Dim lblGSTRate As Label
            lblGSTRate = e.Item.FindControl("lblGSTRate")
            Select Case ViewState("mode")
                Case "bc"
                    lblGSTRate.Text = ""
                Case "cc"
                    lblGSTRate.Text = Common.parseNull(dv("GSTRateDesc"))
            End Select

            Dim hidTaxPerc, hidGSTRate As TextBox
            Dim hidTaxID As TextBox

            hidTaxPerc = e.Item.FindControl("hidTaxPerc")
            hidGSTRate = e.Item.FindControl("hidGSTRate")
            hidTaxID = e.Item.FindControl("hidTaxID")

            hidTaxPerc.Text = Common.parseNull(dv("GST"))
            hidGSTRate.Text = Common.parseNull(dv("GSTRate"))

            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
            Dim lblTaxCode As Label
            lblTaxCode = e.Item.FindControl("lblTaxCode")
            If Common.parseNull(dv("GSTRate")) <> "" Then
                If Not IsDBNull(dv("GstTaxCode")) Then
                    lblTaxCode.Text = dv("GstTaxCode")
                End If
            End If

            Dim txtAmount As TextBox
            txtAmount = e.Item.FindControl("txtAmount")
            If IsDBNull(dv("AMOUNT")) Then
                txtAmount.Text = Format(0, "###,###,##0.00")
            Else
                txtAmount.Text = Format(dv("AMOUNT"), "###,###,##0.00")
            End If

            Dim txtGST As TextBox
            txtGST = e.Item.FindControl("txtGST")
            txtGST.Text = Common.parseNull(dv("GST"))

            Dim txtGSTAmt As TextBox
            txtGSTAmt = e.Item.FindControl("txtGSTAmt")
            dblAmt = CDbl(txtAmount.Text)

            ' Note on unknow use for "subtotal" _Yap
            If ViewState("GST") = "subtotal" Then
                dblTaxTotal = dblTaxTotal + dblAmt
                ViewState("intGSTcnt") += 1
                ViewState("intTotItem") += 1
                dblGstAmt = dblAmt * (hidTaxPerc.Text / 100)
                txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
            Else
                dblNoTaxTotal = dblNoTaxTotal + dblAmt
                If dv("GST") = 0 Then
                    txtGSTAmt.Text = "0.00"
                Else
                    dblGstAmt = dblAmt * (dv("GST") / 100)
                    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                End If
            End If

            Dim hidGSTAmt As HtmlInputHidden
            hidGSTAmt = e.Item.FindControl("hidGSTAmt")
            '2015-06-17: CH: Rounding issue (Prod issue)
            'hidGSTAmt.Value = dblGstAmt
            hidGSTAmt.Value = CDec(Format(dblGstAmt, "###0.00"))

            txtQty.Attributes.Add("onfocus", "return focusControl('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            txtQty.Attributes.Add("onblur", "return calculateTotal('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            'txtPrice.Attributes.Add("onfocus", "return focusControl('" & _
            '                   IIf(ViewState("GST") = "product", 0, 1) & "', '" & _
            '                   txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & _
            '                   txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & _
            '                   hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            'txtPrice.Attributes.Add("onblur", "return calculateTotal('" & _
            '                    IIf(ViewState("GST") = "product", 0, 1) & "', '" & _
            '                    txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & _
            '                    txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & _
            '                    hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            '2015-06-17: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))

            txtGSTAmt = e.Item.FindControl("txtGSTAmt")

            Dim txtCommodity As TextBox
            txtCommodity = e.Item.FindControl("txtCommodity")

            Dim hidCommodity As HtmlInputHidden
            hidCommodity = e.Item.FindControl("hidCommodity")

            Dim objDB2 As New EAD.DBCom
            If Common.Parse(dv("VENDORITEMCODE")) = "" Then
                hidCommodity.Value = objDB2.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(dv("COMMODITY")) & "'")
                txtCommodity.Text = dv("COMMODITY")
                ddl_uom.Enabled = True
                'ddl_comm.Enabled = True
                txtCommodity.Enabled = True
                txtAmount.Text = "0.00"
                txtPrice.Text = "0.0000"
                txtGSTAmt.Text = "0.00"
            Else
                hidCommodity.Value = objDB2.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(dv("COMMODITY")) & "'")
                txtCommodity.Text = dv("COMMODITY")
                ddl_uom.Enabled = False
                'ddl_comm.Enabled = False
                txtCommodity.Enabled = False
            End If

            Dim lblItemLine As Label
            lblItemLine = e.Item.FindControl("lblItemLine")
            lblItemLine.Text = Common.parseNull(dv("ITEMLINE"))

            'Jules 2018.05.03 - PAMB Scrum 2 - Analysis Codes
            'Jules 2018.10.17
            'Dim cboFundType As DropDownList
            'cboFundType = e.Item.FindControl("cboFundType")
            'objGlobal.FillAnalysisCode("L1", cboFundType)
            'Common.SelDdl(dv("FUNDTYPE"), cboFundType, True, True)

            'Dim cboPersonCode As DropDownList
            'cboPersonCode = e.Item.FindControl("cboPersonCode")
            'objGlobal.FillAnalysisCode("L9", cboPersonCode)

            ''Jules 2018.09.05
            'Dim PersonCodelstItem As New ListItem
            'PersonCodelstItem.Value = "N/A"
            'PersonCodelstItem.Text = "N/A"
            'cboPersonCode.Items.Insert(1, PersonCodelstItem)
            ''End modification.

            'Common.SelDdl(dv("PERSONCODE"), cboPersonCode, True, True)

            'Dim cboProjectCode As DropDownList
            'cboProjectCode = e.Item.FindControl("cboProjectCode")
            'objGlobal.FillAnalysisCode("L8", cboProjectCode)
            'Common.SelDdl(dv("PROJECTCODE"), cboProjectCode, True, True)

            Dim txtFundType, txtPersonCode, txtProjectCode As Label
            txtFundType = e.Item.FindControl("txtFundType")
            txtFundType.Text = ""
            txtFundType.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            txtPersonCode = e.Item.FindControl("txtPersonCode")
            txtPersonCode.Text = ""
            txtPersonCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            txtProjectCode = e.Item.FindControl("txtProjectCode")
            txtProjectCode.Text = ""
            txtProjectCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidFundType, hidPersonCode, hidProjectCode As TextBox
            hidFundType = e.Item.FindControl("hidFundType")
            hidFundType.Text = ""

            hidPersonCode = e.Item.FindControl("hidPersonCode")
            hidPersonCode.Text = ""

            hidProjectCode = e.Item.FindControl("hidProjectCode")
            hidProjectCode.Text = ""

            If Common.parseNull(dv("FUNDTYPEDESC")) <> "" Then
                txtFundType.Text = dv("FUNDTYPEDESC")
            End If

            If Common.parseNull(dv("FUNDTYPE")) <> "" Then
                hidFundType.Text = dv("FUNDTYPE")
            End If

            If Common.parseNull(dv("PERSONCODEDESC")) <> "" Then
                txtPersonCode.Text = dv("PERSONCODEDESC")
            End If

            If Common.parseNull(dv("PERSONCODE")) <> "" Then
                hidPersonCode.Text = dv("PERSONCODE")
            End If

            If Common.parseNull(dv("PROJECTCODEDESC")) <> "" Then
                txtProjectCode.Text = dv("PROJECTCODEDESC")
            End If

            If Common.parseNull(dv("PROJECTCODE")) <> "" Then
                hidProjectCode.Text = dv("PROJECTCODE")
            End If

            Dim cmdFundType As HtmlInputButton
            cmdFundType = e.Item.FindControl("cmdFundType")
            cmdFundType.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtFundType.ClientID & "&hidAnalysisCode=" & hidFundType.ClientID & "&dept=L1") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            Dim cmdPersonCode As HtmlInputButton
            cmdPersonCode = e.Item.FindControl("cmdPersonCode")
            cmdPersonCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtPersonCode.ClientID & "&hidAnalysisCode=" & hidPersonCode.ClientID & "&dept=L9") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            Dim cmdProjectCode As HtmlInputButton
            cmdProjectCode = e.Item.FindControl("cmdProjectCode")
            cmdProjectCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtProjectCode.ClientID & "&hidAnalysisCode=" & hidProjectCode.ClientID & "&dept=L8") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")
            'End modification.

            ' New added for the PR GL and category: Start
            'Dim cboGLCode, cboCategoryCode As DropDownList
            'Dim dsGLCode As New DataSet
            Dim cboCategoryCode As DropDownList
            Dim dsCategoryCode As New DataSet

            'Jules 2018.05.04 - PAMB Scrum 2
            Dim cboGift As DropDownList
            cboGift = e.Item.FindControl("cboGift")
            'End modification.

            Dim objDB As New EAD.DBCom
            Dim PFOR As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

            'Jules 2018.10.22 - U00019
            'cboGLCode = e.Item.FindControl("cboGLCode")
            'dsGLCode = objAdmin.PopulateGLCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("GLCODE")), PFOR)
            'dvwGLCode = dsGLCode.Tables(0).DefaultView

            'Common.FillDdl(cboGLCode, "DESCRIPTION", "GL Code", dvwGLCode)
            'If Not IsDBNull(dv("GLCode")) And Common.parseNull(dv("GLCode")) <> "" Then
            '    'Jules 2018.08.06
            '    GLCodelstItem.Value = ""
            '    GLCodelstItem.Text = "---Select---"
            '    cboGLCode.Items.Insert(0, GLCodelstItem)
            '    cboGLCode.SelectedIndex = 0
            '    'End modification.
            '    Common.SelDdl(dv("GLCode"), cboGLCode, True, True)
            'Else
            '    If cboGLCode.Items.Count = 0 Then
            '        GLCodelstItem.Value = ""
            '        GLCodelstItem.Text = "Not Applicable"
            '        cboGLCode.Items.Insert(0, GLCodelstItem)
            '        cboGLCode.SelectedIndex = 0
            '    Else 'Jules 2018.08.06
            '        GLCodelstItem.Value = ""
            '        GLCodelstItem.Text = "---Select---"
            '        cboGLCode.Items.Insert(0, GLCodelstItem)
            '        cboGLCode.SelectedIndex = 0
            '        'End modifcation
            '    End If
            'End If

            'cboGLCode.Attributes.Add("onchange", "return setGiftDDLState('" & cboGLCode.ClientID & "', '" & cboGift.ClientID & "');") 'Jules 2018.08.06
            Dim txtGLCode As Label
            txtGLCode = e.Item.FindControl("txtGLCode")
            txtGLCode.Text = ""
            txtGLCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidGLCode As TextBox
            hidGLCode = e.Item.FindControl("hidGLCode")
            hidGLCode.Text = ""

            If Common.parseNull(dv("GLDESCRIPTION")) <> "" Then
                txtGLCode.Text = dv("GLDESCRIPTION")
            End If

            If Common.parseNull(dv("GLCODE")) <> "" Then
                hidGLCode.Text = dv("GLCODE")
            End If

            Dim cmdGLCode As HtmlInputButton
            cmdGLCode = e.Item.FindControl("cmdGLCode")
            cmdGLCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=" & txtGLCode.ClientID & "&hidGLCode=" & hidGLCode.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")
            txtGLCode.Attributes.Add("onchange", "return setGiftDDLState('" & hidGLCode.ClientID & "', '" & cboGift.ClientID & "');") 'Jules 2018.08.06
            'End U00019.

            'Jules 2018.05.04 - PAMB Scrum 2 - Do not allow user to select Gift if CAPEX.
            'Jules 2018.10.22 - U00019
            'If Not IsDBNull(dv("GIFT")) And Common.parseNull(dv("GIFT")) <> "" AndAlso cboGLCode.SelectedIndex <> 0 Then            
            '   Common.SelDdl(dv("GIFT"), cboGift, True, True)
            'End If

            'If cboGLCode.SelectedValue.ToString <> "" AndAlso cboGLCode.SelectedValue.ToString.Substring(0, 1) = "1" Then
            '    cboGift.Enabled = False
            'End If
            If hidGLCode.Text <> "" AndAlso hidGLCode.Text.ToString.Substring(0, 1) = "1" Then
                cboGift.Enabled = False
            End If
            'End modification.

            cboCategoryCode = e.Item.FindControl("cboCategoryCode")
            Dim PFORC As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

            'Jules 2018.07.31 - Set to 'N/A'
            'dsCategoryCode = objAdmin.PopulateCategoryCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("CategoryCode")), PFORC)
            'dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
            'Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)

            'If Not IsDBNull(dv("CategoryCode")) And Common.parseNull(dv("CategoryCode")) <> "" Then
            '    Common.SelDdl(dv("CategoryCode"), cboCategoryCode, True, True)
            'Else
            '    If cboCategoryCode.Items.Count = 0 Then
            CategoryCodelstItem.Value = ""
            CategoryCodelstItem.Text = "N/A" '"Not Applicable"
            cboCategoryCode.Items.Insert(0, CategoryCodelstItem)
            cboCategoryCode.SelectedIndex = 0
            '    End If
            'End If
            'End modification.

            Dim Asset As New PurchaseOrder_Buyer
            If Asset.AssetGroupMstr = True Then
                Dim cboAssetGroup As DropDownList
                cboAssetGroup = e.Item.FindControl("cboAssetGroup")
                objGlobal.FillAssetGroup(cboAssetGroup)
                Common.SelDdl(dv("PRD_ASSET_GROUP"), cboAssetGroup, True, True)
            End If

            Dim hidGLCodeStatus As TextBox
            hidGLCodeStatus = e.Item.FindControl("hidGLCodeStatus")
            hidGLCodeStatus.Text = "A"

            If lblVENDORITEMCODE.Text <> "" And lblVENDORITEMCODE.Text <> "&nbsp;" Then
                'Jules 2018.10.22 U00019
                'e.Item.Cells(EnumShoppingCart.icGLCode).Enabled = False
                'e.Item.Cells(EnumShoppingCart.icGLCode1).Enabled = False
                cmdGLCode.Visible = False
                hidGLCode.ReadOnly = True
                txtGLCode.Enabled = False
                hidGLCodeStatus.Text = "N"
                'End modification.
                e.Item.Cells(EnumShoppingCart.icCategoryCode1).Enabled = False
            End If
            ' New added for the PR GL and category: End

            ' BCM _Yap
            If ViewState("BCM") > 0 Then
                If Not IsDBNull(dv("ACCT")) And Common.parseNull(dv("ACCT")) <> "" And ViewState("type") = "mod" Then
                    If Not dtBCM Is Nothing Then
                        Dim drTemp As DataRow()
                        drTemp = dtBCM.Select("Acct_Index=" & dv("ACCT"))
                        If drTemp.Length > 0 Then
                            txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                            hidBudgetCode.Text = Common.parseNull(dv("ACCT"))
                        End If
                    End If
                Else
                    'Jules 2018.10.17 - Commented to force user to select Budget Account
                    'If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                    '    txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                    '    hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                    'End If
                    'End modification.
                End If

                Dim cmdBudget As HtmlInputButton
                cmdBudget = e.Item.FindControl("cmdBudget")
                cmdBudget.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=" & txtBudget.ClientID & "&hidBudgetCode=" & hidBudgetCode.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            Else
                e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            End If

            If ViewState("type") = "new" Then
                hidDelCode.Text = ViewState("strDeliveryDefault")
                txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If IsDBNull(dv("DADDRCODE")) Then
                    hidDelCode.Text = ViewState("strDeliveryDefault")
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                Else
                    'ViewState("strDeliveryDefault") = Common.Parse(dv("DADDRCODE"))
                    'hidDelCode.Text = ViewState("strDeliveryDefault")
                    hidDelCode.Text = Common.Parse(dv("DADDRCODE"))
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(dv("DADDRCODE")), "D"), 1, 10)
                End If
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            Dim cmdDelivery As HtmlInputButton
            cmdDelivery = e.Item.FindControl("cmdDelivery")
            cmdDelivery.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId & "&mod=P&type2=RPO&type=D&id=" & hidDelCode.ClientID & "&txtDelivery=" & txtDelivery.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            ' Custom Field
            Dim cboCustom As DropDownList
            Dim j As Integer
            'If e.Item.Cells(EnumShoppingCart.icRemark).Controls(0).GetType Is GetType(DropDownList) Then
            '    For i = EnumShoppingCart.icRemark To EnumShoppingCart.icRemark + intCnt - 1
            '        Dim blnSetDefault As Boolean
            '        blnSetDefault = False
            '        cboCustom = e.Item.Cells(i).Controls(0)
            '        Common.FillDdl(cboCustom, "CF_FIELD_VALUE", "CF_FIELD_VALUE", dvwCustom(i - EnumShoppingCart.icRemark))
            '        rebindDDL(cboCustom)

            '        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
            '        blnSetDefault = True

            '        If blnSetDefault = False Then
            '            Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
            '        End If
            '    Next
            'End If


            'If ViewState("type") = "new" Then
            '    Select Case ViewState("mode")
            '        Case "bc"
            '        Case "cc"
            '    End Select
            'ElseIf ViewState("type") = "mod" Then
            '    Select Case ViewState("mode")
            '        Case "bc"
            '        Case "cc"
            '    End Select
            'End If

            If e.Item.Cells(EnumShoppingCart.icRemark).Controls(0).GetType Is GetType(DropDownList) Then
                For i = EnumShoppingCart.icRemark To EnumShoppingCart.icRemark + intCnt - 1
                    Dim blnSetDefault As Boolean
                    blnSetDefault = False
                    cboCustom = e.Item.Cells(i).Controls(0)
                    Common.FillDdl(cboCustom, "CF_FIELD_VALUE", "CF_FIELD_VALUE", dvwCustom(i - EnumShoppingCart.icRemark))
                    rebindDDL(cboCustom)

                    If ViewState("type") = "new" Then
                        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                        blnSetDefault = True
                        Select Case ViewState("mode")
                            Case "bc"
                            Case "cc"
                        End Select
                    ElseIf ViewState("type") = "mod" Then
                        For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                            'If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = lblItemLine.Text And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                            'If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = e.Item.Cells(EnumShoppingCart.icNo).Text And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                            If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = dv("ITEMLINE") And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                                Common.SelDdl(encodeCustomField(dvwCustomItem.Table.Rows(j)("PCD_FIELD_VALUE")), cboCustom, True, True)

                                'If blnSetDefault = False And Session("CurrentScreen") = "AddItem" Then
                                '    Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                                'End If

                                blnSetDefault = True
                                Exit For
                            End If
                        Next
                        Select Case ViewState("mode")
                            Case "bc"
                            Case "cc"
                        End Select
                    End If

                    If blnSetDefault = False Then
                        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                    End If

                    If Session("strItemCust") IsNot Nothing Then
                        strItemCust = Session("strItemCust")
                        'For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                        If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= (strItemCust.Count / intCnt) - 1 Then
                            'cboCustom.SelectedIndex = strItemCust(intRow + i - CInt(EnumShoppingCart.icRemark))(1)
                            'If kk = 0 Then
                            '    kk = intRow
                            'Else
                            '    kk = intRow + 1
                            'End If
                            cboCustom.SelectedIndex = strItemCust(kk)(1)
                            kk = kk + 1
                        End If
                        'Next
                    End If
                Next
            End If

            Dim txtEstDate As TextBox
            Dim txtWarranty As TextBox

            txtEstDate = e.Item.FindControl("txtEstDate")
            txtWarranty = e.Item.FindControl("txtWarranty")
            'Michelle (2/8/2010) - To include the defaul EDD
            'Dim objDB As New EAD.DBCom
            Dim txtDate1 As Date = CType(lblDate.Text, Date)
            Dim intEDD As Integer = Val(objDB.GetVal("SELECT CV_EDD FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & strSCoyId & "'"))

            If ViewState("type") = "new" Then
                If intEDD > 0 Then
                    txtEstDate.Text = txtDate1.AddDays(intEDD)
                    txtEstDate.ReadOnly() = True
                    txtEstDate.ForeColor = Color.Gray
                    txtEstDate.Enabled = False
                Else
                    txtEstDate.Text = txtDate1.AddDays(1)
                    txtEstDate.ReadOnly() = False
                End If
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim tem As Integer
                If IsDBNull(dv("ETD")) Then
                    tem = 1
                Else
                    tem = dv("ETD")
                End If
                If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(tem)

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            Dim revWarranty As RegularExpressionValidator
            revWarranty = e.Item.FindControl("revWarranty")
            revWarranty.ValidationExpression = "\d{1,5}"
            revWarranty.ControlToValidate = "txtWarranty"
            revWarranty.ErrorMessage = "Invalid Warranty Terms (mths)"
            revWarranty.Display = ValidatorDisplay.Dynamic
            revWarranty.Text = "?"

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                'txtWarranty.CssClass = "lblnumerictxtbox"
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            End If

            'keep the Product Code of the selected record into an array
            aryProdCodeNew.Add(New String() {Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("PRODUCTDESC")), Common.parseNull(dv("UOM")), Common.parseNull(dv("COMMODITY")), Common.parseNull(dv("CDM_GROUP_INDEX"))})
            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Text = Common.parseNull(dv("REMARK"))
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            intRow = intRow + 1

            If Session("strItem") IsNot Nothing Then
                strItem = Session("strItem")
                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= strItem.Count - 1 Then

                    'Jules 2018.10.22 - U00019
                    'cboGLCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                    hidGLCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                    If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                        Dim drGLCodeTemp As DataRow()
                        drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                        If drGLCodeTemp.Length > 0 Then
                            txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                        End If
                    End If
                    'End modification.

                    txtQty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(1)
                    txtPrice.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(2)
                    'txtBudget.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(3)
                    hidBudgetCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(4)

                    'Jules 2018.07.16 - PAMB - Error if Budget Code is no longer valid.
                    'If Not dtBCM Is Nothing Then
                    If Not dtBCM Is Nothing AndAlso hidBudgetCode.Text <> "" Then
                        Dim drTemp As DataRow()
                        drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                        If drTemp.Length > 0 Then
                            txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        End If
                    End If
                    'txtDelivery.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(5)
                    hidDelCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)), "D"), 1, 10)
                    txtEstDate.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(7)
                    txtWarranty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(8)
                    txtRemark.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(9)

                    'Jules 2018.05.04 - PAMB Scrum 2
                    cboGift.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(10)

                    'Jules 2018.10.17
                    'cboFundType.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(11)
                    'cboPersonCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(12)
                    'cboProjectCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(13)
                    hidFundType.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(11)
                    If hidFundType.Text <> "" Then
                        If dtFundType IsNot Nothing AndAlso hidFundType.Text <> "" Then
                            Dim drFundType As DataRow()
                            drFundType = dtFundType.Select("AC_ANALYSIS_CODE='" & hidFundType.Text & "'")
                            If drFundType.Length > 0 Then
                                txtFundType.Text = drFundType(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidFundType.Text
                            End If
                        Else
                            txtFundType.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L1' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidFundType.Text & "'")
                        End If
                    End If
                    hidPersonCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(12)
                    If hidPersonCode.Text <> "" Then
                        If dtPersonCode IsNot Nothing AndAlso hidPersonCode.Text <> "" Then
                            Dim drPersonCode As DataRow()
                            drPersonCode = dtPersonCode.Select("AC_ANALYSIS_CODE='" & hidPersonCode.Text & "'")
                            If drPersonCode.Length > 0 Then
                                txtPersonCode.Text = drPersonCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidPersonCode.Text
                            End If
                        Else
                            txtPersonCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L9' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidPersonCode.Text & "'")
                        End If
                    End If
                    hidProjectCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(13)
                    If hidProjectCode.Text <> "" Then
                        If dtProjectCode IsNot Nothing AndAlso hidProjectCode.Text <> "" Then
                            Dim drProjectCode As DataRow()
                            drProjectCode = dtProjectCode.Select("AC_ANALYSIS_CODE='" & hidProjectCode.Text & "'")
                            If drProjectCode.Length > 0 Then
                                txtProjectCode.Text = drProjectCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidProjectCode.Text
                            End If
                        Else
                            txtProjectCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L8' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidProjectCode.Text & "'")
                        End If
                    End If
                    'End modification.                                        
                End If
            End If

            'Jules 2018.07.30
            If ViewState("strDuplicatedItem") IsNot Nothing Then
                strDuplicatedItem = ViewState("strDuplicatedItem")
                strItem = Session("strItem")
                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text)) = strItem.Count + 1 Then

                    'Jules 2018.10.22 - U00019
                    'cboGLCode.SelectedIndex = strDuplicatedItem(0)(0)
                    hidGLCode.Text = strDuplicatedItem(0)(0)
                    If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                        Dim drGLCodeTemp As DataRow()
                        drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                        If drGLCodeTemp.Length > 0 Then
                            txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                        End If
                    End If
                    'End modification.

                    txtQty.Text = strDuplicatedItem(0)(1)
                    txtPrice.Text = strDuplicatedItem(0)(2)
                    hidBudgetCode.Text = strDuplicatedItem(0)(4)
                    If Not dtBCM Is Nothing AndAlso hidBudgetCode.Text <> "" Then
                        Dim drTemp As DataRow()
                        drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                        If drTemp.Length > 0 Then
                            txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        End If
                    End If
                    hidDelCode.Text = strDuplicatedItem(0)(6)
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strDuplicatedItem(0)(6)), "D"), 1, 10)
                    'txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), strDuplicatedItem(0)(6), "D") '  
                    txtEstDate.Text = strDuplicatedItem(0)(7)
                    txtWarranty.Text = strDuplicatedItem(0)(8)
                    txtRemark.Text = strDuplicatedItem(0)(9)
                    cboGift.SelectedIndex = strDuplicatedItem(0)(10)

                    'Jules 2018.10. 22 - U00019
                    'Jules 2018.08.27 - PAMB Scrum 2 - Do not allow user to select Gift if CAPEX.                   
                    'If cboGLCode.SelectedValue.ToString <> "" AndAlso cboGLCode.SelectedValue.ToString.Substring(0, 1) = "1" Then
                    '    cboGift.Enabled = False
                    'End If
                    If hidGLCode.Text <> "" AndAlso hidGLCode.Text.ToString.Substring(0, 1) = "1" Then
                        cboGift.Enabled = True
                    End If
                    'End modification.

                    'Jules 2018.10.17
                    ''cboFundType.SelectedIndex = strDuplicatedItem(0)(11)
                    'cboPersonCode.SelectedIndex = strDuplicatedItem(0)(12)
                    'cboProjectCode.SelectedIndex = strDuplicatedItem(0)(13)
                    hidFundType.Text = strDuplicatedItem(0)(11)
                    If hidFundType.Text <> "" Then
                        If dtFundType IsNot Nothing AndAlso hidFundType.Text <> "" Then
                            Dim drFundType As DataRow()
                            drFundType = dtFundType.Select("AC_ANALYSIS_CODE='" & hidFundType.Text & "'")
                            If drFundType.Length > 0 Then
                                txtFundType.Text = drFundType(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidFundType.Text
                            End If
                        Else
                            txtFundType.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L1' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidFundType.Text & "'")
                        End If
                    End If
                    hidPersonCode.Text = strDuplicatedItem(0)(12)
                    If hidPersonCode.Text <> "" Then
                        If dtPersonCode IsNot Nothing AndAlso hidPersonCode.Text <> "" Then
                            Dim drPersonCode As DataRow()
                            drPersonCode = dtPersonCode.Select("AC_ANALYSIS_CODE='" & hidPersonCode.Text & "'")
                            If drPersonCode.Length > 0 Then
                                txtPersonCode.Text = drPersonCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidPersonCode.Text
                            End If
                        Else
                            txtPersonCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L9' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidPersonCode.Text & "'")
                        End If
                    End If
                    hidProjectCode.Text = strDuplicatedItem(0)(13)
                    If hidProjectCode.Text <> "" Then
                        If dtProjectCode IsNot Nothing AndAlso hidProjectCode.Text <> "" Then
                            Dim drProjectCode As DataRow()
                            drProjectCode = dtProjectCode.Select("AC_ANALYSIS_CODE='" & hidProjectCode.Text & "'")
                            If drProjectCode.Length > 0 Then
                                txtProjectCode.Text = drProjectCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidProjectCode.Text
                            End If
                        Else
                            txtProjectCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L8' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidProjectCode.Text & "'")
                        End If
                    End If
                    'End modification.

                    cboCategoryCode.SelectedIndex = strDuplicatedItem(0)(14)
                    txtGSTAmt.Text = strDuplicatedItem(0)(15)

                    Dim itemTotal As Double = 0.0
                    itemTotal = txtPrice.Text * txtQty.Text
                    itemTotal = Math.Round(CType(itemTotal, Double), 2, MidpointRounding.AwayFromZero)
                    txtAmount.Text = Format(itemTotal, "###,##0.00")
                    ViewState("strDuplicatedItem") = Nothing
                    ViewState("_isDuplicated") = Nothing
                End If
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("BCM") <= 0 Then
                e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            End If
        End If
        'objGlobal = Nothing
    End Sub

    Sub Populate(ByVal i As Integer)
        Dim content As String = ""
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        Dim j As String

        If Len(CStr((i + 2))) = 1 Then
            j = "0" & CStr((i + 2))
        Else
            j = (i + 2)
        End If

        content &= "$(""#dtgShopping_ctl" & j & "_txtCommodity"").autocomplete(""" & nametypeahead & """, {" & vbCrLf &
        "width: 250," & vbCrLf &
        "scroll: true," & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "});" & vbCrLf &
        "$(""#dtgShopping_ctl" & j & "_txtCommodity"").result(function(event, data, formatted) {" & vbCrLf &
        "if(data)" & vbCrLf &
        "$(""#dtgShopping_ctl" & j & "_hidCommodity"").val(data[1]);" & vbCrLf &
        "}); " & vbCrLf

        Session("content") &= content
    End Sub

    Sub buildjava()
        Dim typeahead As String
        typeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            Session("content") & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        Session("typeahead") = typeahead
    End Sub

    Private Function rebindDDL(ByRef pddl As DropDownList) As String
        Dim i As Integer
        For i = 0 To pddl.Items.Count - 1
            pddl.Items(i).Value = Server.UrlEncode(pddl.Items(i).Value)
        Next
        rebindDDL = ""
    End Function

    Public Sub dtgshopping_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgShopping.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Sub dtgShopping_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        strPageId = ViewState("strPageId")

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

            Dim objCheckGST As New GST
            ViewState("CheckGST") = objCheckGST.chkGSTCOD()
            If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                If ViewState("CheckGST") = True Then
                    e.Item.Cells(EnumShoppingCart.icTax).Text = "SST Amount"
                Else
                    e.Item.Cells(EnumShoppingCart.icTax).Text = "Tax"
                End If
                Select Case ViewState("mode")
                    Case "bc"
                        e.Item.Cells(EnumShoppingCart.icPrice).Text = "Last Txn. Price"
                    Case "cc"
                        e.Item.Cells(EnumShoppingCart.icPrice).Text = "Contract Price"
                End Select
            End If
            objCheckGST = Nothing

            Dim i As Integer
            Dim str As String

            'Jules 2018.10.18
            For i = EnumShoppingCart.icFundType To EnumShoppingCart.icProjectCode
                Dim cell As TableCell
                Dim lbl As New Label
                cell = e.Item.Cells(i)
                If i = EnumShoppingCart.icFundType Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtFundType&mode=all&dept=L1") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icPersonCode Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtPersonCode&mode=all&dept=L9") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icProjectCode Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtProjectCode&mode=all&dept=L8") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                End If
                cell.Controls.Add(lbl)
            Next

            For i = EnumShoppingCart.icGLCode To EnumShoppingCart.icGLCode1
                Dim cell As TableCell
                Dim lbl As New Label
                cell = e.Item.Cells(i)
                If i = EnumShoppingCart.icGLCode Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=txtGLCode&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icGLCode1 Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=txtGLCode&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                End If
                cell.Controls.Add(lbl)
            Next
            'End modification.

            For i = EnumShoppingCart.icBudget To dtgShopping.Columns.Count - 2
                Dim cell As TableCell
                Dim lbl As New Label
                cell = e.Item.Cells(i)
                If i = EnumShoppingCart.icBudget Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=txtBudget&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icDelivery Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId) & "&mod=P&type2=RPO&type=D&id=cboDelivery&mode=all', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icEstDate Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendarForAll.aspx", "id=txtEstDate") & "','cal','width=180,height=155,left=270,top=180')"">" & e.Item.Cells(i).Text & "</A>"
                ElseIf i = EnumShoppingCart.icWarranty Then
                    lbl.Text = e.Item.Cells(i).Text
                ElseIf i > EnumShoppingCart.icWarranty Then
                    str = dDispatcher.direct("Admin", "CustomFieldValue.aspx", "pageid=" & strPageId & "&mod=P&module=PR&value=" & dtgShopping.Columns(i).SortExpression & "&name=" & Server.UrlEncode(dtgShopping.Columns(i).HeaderText) & "&id=" & dtgShopping.Columns(i).SortExpression)
                    lbl.Text = "<A href='javascript:;' onclick=""window.open(&quot;" & str & "&quot;, &quot;&quot;, &quot;scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no&quot;);"">" & e.Item.Cells(i).Text & "</a>"
                End If
                cell.Controls.Add(lbl)
            Next
        End If

        dtgShopping.AllowSorting = False
        Grid_ItemCreated(dtgShopping, e)
    End Sub

    Private Sub addDataGridColumn()
        Dim objAdmin As New Admin
        Dim dsCustom As New DataSet
        Dim dsCustomField As New DataSet
        Dim i As Integer
        dvwCus = objAdmin.getCustomField("")

        If Not dvwCus Is Nothing Then
            intCnt = dvwCus.Count
            ReDim dvwCustom(intCnt)
            ReDim strCustomDefault(intCnt)

            '//dynamicly add template column
            For i = 0 To intCnt - 1
                Dim col As TemplateColumn = New TemplateColumn
                col.ItemTemplate = New dgTemplate(dvwCus.Table.Rows(i)("CF_FIELD_NO"), 2)
                col.HeaderText = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
                col.SortExpression = dvwCus.Table.Rows(i)("CF_FIELD_NO")
                dtgShopping.Columns.AddAt(dtgShopping.Columns.Count - 1, col)

                dsCustomField = objAdmin.Populate_customField(dvwCus.Table.Rows(i)("CF_FIELD_NO"), "")
                dvwCustom(i) = dsCustomField.Tables(0).DefaultView
                strCustomDefault(i) = encodeCustomField(objAdmin.CustomAddr(dvwCus.Table.Rows(i)("CF_FIELD_NO")))
            Next
            Me.DynamicColumnAdded = True
        End If
        objAdmin = Nothing
    End Sub

    Private Sub fillAddress()
        Dim i As Integer
        Dim dvwBilling As DataView
        Dim dsBilling As New DataSet
        Dim objAdmin As New Admin

        dsBilling = objAdmin.PopulateAddr("B", objAdmin.user_Default_Add("B"), "", "")

        dvwBilling = dsBilling.Tables(0).DefaultView
        For i = 0 To dvwBilling.Count - 1
            If dvwBilling.Table.Rows(i)("AM_ADDR_CODE") = objAdmin.user_Default_Add("B") Then
                ViewState("BillAdd1") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE1"))
                ViewState("BillAdd2") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE2"))
                ViewState("BillAdd3") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE3"))
                ViewState("BillPostCode") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_POSTCODE"))
                ViewState("BillCity") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_CITY"))
                ViewState("Country") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY"))
                'ViewState("State") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY"))
                ViewState("State") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_STATE"))
                Exit For
            End If
        Next
    End Sub

    Private Function bindPR() As DataSet
        Dim ds As New DataSet
        Dim objAdmin As New Admin

        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("PRNo", Type.GetType("System.String"))
        dtMaster.Columns.Add("ReqName", Type.GetType("System.String"))
        dtMaster.Columns.Add("Attn", Type.GetType("System.String"))
        dtMaster.Columns.Add("ReqPhone", Type.GetType("System.String"))
        dtMaster.Columns.Add("InternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("ExternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintCustom", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("Urgent", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentTerm", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentMode", Type.GetType("System.String"))
        dtMaster.Columns.Add("PRType", Type.GetType("System.String"))
        dtMaster.Columns.Add("PRCost", Type.GetType("System.Double"))

        dtMaster.Columns.Add("BillAddrCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine1", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine2", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine3", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrPostCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrState", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCity", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCountry", Type.GetType("System.String"))


        Dim dtr As DataRow
        Dim dtrd As DataRow
        dtr = dtMaster.NewRow()
        dtr("PRNo") = lblPRNo.Text 'ViewState("prid")
        dtr("ReqName") = txtRequestedName.Text
        dtr("ReqPhone") = txtRequestedContact.Text
        dtr("Attn") = txtAttention.Text
        dtr("InternalRemark") = txtInternal.Text
        dtr("ExternalRemark") = txtExternal.Text
        dtr("PrintCustom") = IIf(chkCustomPR.Checked, "1", "0")
        dtr("PrintRemark") = IIf(chkRemarkPR.Checked, "1", "0")
        dtr("Urgent") = IIf(chkUrgent.Checked, "1", "0")
        ' Set Both ShipmentTerm, ShipmentMode to 99 (Default)
        dtr("ShipmentTerm") = "99"
        dtr("ShipmentMode") = "99"
        dtr("PRType") = ""

        fillAddress()
        dtr("BillAddrCode") = objAdmin.user_Default_Add("B")
        dtr("BillAddrLine1") = ViewState("BillAdd1")
        dtr("BillAddrLine2") = ViewState("BillAdd2")
        dtr("BillAddrLine3") = ViewState("BillAdd3")
        dtr("BillAddrPostCode") = ViewState("BillPostCode")
        dtr("BillAddrState") = ViewState("State")
        dtr("BillAddrCity") = ViewState("BillCity")
        dtr("BillAddrCountry") = ViewState("Country")

        If IsDBNull(dtr("BillAddrCode")) = True Then
            blnValidBill = False
        Else
            blnValidBill = True
        End If

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
                    dtr("PRType") = "CC"
            End Select
        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
                    dtr("PRType") = "CC"
            End Select
        End If

        If hidCost.Value = "" Then
            hidCost.Value = "0"
        End If

        Dim dtDetails As New DataTable
        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtDetails.Columns.Add("Qty", Type.GetType("System.Double"))
        dtDetails.Columns.Add("Commodity", Type.GetType("System.String"))
        dtDetails.Columns.Add("UOM", Type.GetType("System.String"))
        dtDetails.Columns.Add("Currency", Type.GetType("System.String"))
        dtDetails.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtDetails.Columns.Add("GST", Type.GetType("System.String"))
        dtDetails.Columns.Add("GSTRate", Type.GetType("System.String"))
        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
        dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
        dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("Remark", Type.GetType("System.String"))
        dtDetails.Columns.Add("Source", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorID", Type.GetType("System.String"))
        dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
        dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))

        dtDetails.Columns.Add("AssetGroup", Type.GetType("System.String"))

        'Jules 2018.05.04 - PAMB Scrum 2
        dtDetails.Columns.Add("Gift", Type.GetType("System.String"))
        dtDetails.Columns.Add("FundType", Type.GetType("System.String"))
        dtDetails.Columns.Add("PersonCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProjectCode", Type.GetType("System.String"))
        'End modification.

        Dim dgItem As DataGridItem
        For Each dgItem In dtgShopping.Items
            blnItem = True
            dtrd = dtDetails.NewRow

            dtrd("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
            dtrd("ProductCode") = dgItem.Cells(EnumShoppingCart.icProductCode).Text
            dtrd("VendorItemCode") = CType(dgItem.FindControl("lblVENDORITEMCODE"), Label).Text
            dtrd("ProductDesc") = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text 'dgItem.Cells(EnumShoppingCart.icItemDesc).Text
            'dtrd("GLCODE") = CType(dgItem.FindControl("lblGLCode"), Label).Text
            'dtrd("CategoryCode") = CType(dgItem.FindControl("lblCategoryCode"), Label).Text

            'Jules 2018.07.31 - Set to N/A
            'dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;" Or dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "N/A", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            'dtrd("GLCode") = IIf(dgItem.Cells(EnumShoppingCart.icGLCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icGLCode).Text) 'Jules 2018.10.22 U00019
            dtrd("CDGroup") = CType(dgItem.FindControl("lblCDGroup"), Label).Text

            'Jules 2018.05.04 - PAMB Scrum 2
            dtrd("Gift") = dgItem.Cells(EnumShoppingCart.icGift).Text

            Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.10.18
            cboGift = dgItem.FindControl("cboGift")
            dtrd("Gift") = cboGift.SelectedItem.Value

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'If cboFundType.Items.Count > 0 Then
            '    dtrd("FundType") = cboFundType.SelectedItem.Value
            'Else
            '    dtrd("FundType") = ""
            'End If            

            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'If cboPersonCode.Items.Count > 0 Then
            '    dtrd("PersonCode") = cboPersonCode.SelectedItem.Value
            'Else
            '    dtrd("PersonCode") = ""
            'End If

            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            'dtrd("ProjectCode") = dgItem.Cells(EnumShoppingCart.icProjectCode).Text
            'If cboProjectCode.Items.Count > 0 Then
            '    dtrd("ProjectCode") = cboProjectCode.SelectedItem.Value
            'Else
            '    dtrd("ProjectCode") = ""
            'End If

            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox
            hidFundType = dgItem.FindControl("hidFundType")
            dtrd("FundType") = hidFundType.Text

            hidPersonCode = dgItem.FindControl("hidPersonCode")
            dtrd("PersonCode") = hidPersonCode.Text

            hidProjectCode = dgItem.FindControl("hidProjectCode")
            dtrd("ProjectCode") = hidProjectCode.Text
            'End modification.

            'Dim ddl_comm As New DropDownList
            'ddl_comm = dgItem.FindControl("ddl_comm")
            'dtrd("Commodity") = ddl_comm.SelectedItem.Value

            'Dim txtCommodity As TextBox
            'txtCommodity = dgItem.FindControl("txtCommodity")

            'Jules 2018.10.22 - U00019
            'Dim cboGLCode, cboCategoryCode As DropDownList
            Dim cboCategoryCode As DropDownList
            'cboGLCode = dgItem.FindControl("cboGLCode")
            'If cboGLCode.Items.Count > 0 Then
            '    dtrd("GLCode") = cboGLCode.SelectedItem.Value
            'Else
            '    dtrd("GLCode") = ""
            'End If
            hidGLCode = dgItem.FindControl("hidGLCode")
            dtrd("GLCode") = hidGLCode.Text
            'End modification.

            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            If cboCategoryCode.Items.Count > 0 Then
                dtrd("CategoryCode") = cboCategoryCode.SelectedItem.Value
            Else
                dtrd("CategoryCode") = ""
            End If

            Dim hidCommodity As HtmlInputHidden
            hidCommodity = dgItem.FindControl("hidCommodity")
            dtrd("Commodity") = hidCommodity.Value

            Dim txtCommodity As TextBox
            txtCommodity = dgItem.FindControl("txtCommodity")
            dtrd("Commodity") = Val(objDB.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(txtCommodity.Text) & "'"))


            dtrd("UOM") = CType(dgItem.FindControl("lblUOM"), Label).Text
            dtrd("Currency") = CType(dgItem.FindControl("lblCurrency"), Label).Text

            If ViewState("BCM") > 0 Then
                Dim hidBudgetCode As TextBox
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                If hidBudgetCode.Text <> "" And hidBudgetCode.Text <> "&nbsp;" Then
                    dtrd("AcctIndex") = hidBudgetCode.Text
                Else
                    dtrd("AcctIndex") = ""
                End If
            Else
                dtrd("AcctIndex") = ""
            End If

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = dgItem.FindControl("txtQty")
            'If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)^\d{1,5}?$") Then
            If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$") Then
                dtrd("Qty") = txtQty.Text
            Else
                dtrd("Qty") = 1.0
                blnValid = False
            End If

            Dim txtPrice As TextBox
            txtPrice = dgItem.FindControl("txtPrice")
            If IsNothing(txtPrice) Then
                dtrd("UnitCost") = dgItem.Cells(EnumShoppingCart.icPrice).Text
            Else
                dtrd("UnitCost") = IIf(txtPrice.Text = "", "0", txtPrice.Text)
            End If

            dtrd("Source") = dgItem.Cells(EnumShoppingCart.icSource).Text
            dtrd("VendorID") = dgItem.Cells(EnumShoppingCart.icVendor).Text

            ' _Yap Get Cost
            Dim hidTaxPerc As TextBox
            Dim txtAmount, txtGST As TextBox, txtGSTAmt As TextBox
            Dim hidGSTRate As TextBox
            hidTaxPerc = dgItem.FindControl("hidTaxPerc")
            hidGSTRate = dgItem.FindControl("hidGSTRate")
            txtAmount = dgItem.FindControl("txtAmount")

            '2015-06-17: CH: Rounding issue (Prod issue)
            txtAmount.Text = CDec(Format(dtrd("Qty") * dtrd("UnitCost"), "###0.00"))
            txtGST = dgItem.FindControl("txtGST")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            If txtGST.Text = "" Then
                dtrd("GST") = 0
            Else
                dtrd("GST") = CDbl(hidTaxPerc.Text)
            End If

            dtrd("GSTRate") = hidGSTRate.Text

            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
            Dim lblTaxCode As Label
            lblTaxCode = dgItem.FindControl("lblTaxCode")
            dtrd("GSTTaxCode") = lblTaxCode.Text

            '2015-06-17: CH: Rounding issue (Prod issue)
            'ViewState("PRCost") = ViewState("PRCost") + CDbl(hidTaxPerc.Text * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            ViewState("PRCost") = ViewState("PRCost") + CDec(Format(hidTaxPerc.Text * txtAmount.Text / 100, "###0.00")) + CDec(txtAmount.Text)

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            If IsDate(txtEstDate.Text) Then
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                Dim diffDay As TimeSpan

                diffDay = txtDate2.Subtract(txtDate1)
                dtrd("ETD") = CInt(diffDay.Days)
            Else
                dtrd("ETD") = 0
                blnValid = False
            End If

            Dim txtWarranty As TextBox
            txtWarranty = dgItem.FindControl("txtWarranty")
            If IsNumeric(txtWarranty.Text) Then
                dtrd("WarrantyTerms") = CInt(txtWarranty.Text)
            Else
                dtrd("WarrantyTerms") = 0
            End If

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtrd("Remark") = txtRemark.Text

            Dim hidDelCode As TextBox
            hidDelCode = dgItem.FindControl("hidDelCode")
            dtrd("DeliveryAddr") = hidDelCode.Text

            Dim Asset As New PurchaseOrder_Buyer
            If Asset.AssetGroupMstr = True Then
                Dim cboAssetGroup As DropDownList
                cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                dtrd("AssetGroup") = cboAssetGroup.SelectedItem.Value
            Else
                dtrd("AssetGroup") = ""
            End If

            dtDetails.Rows.Add(dtrd)

        Next
        dtr("PRCost") = CDbl(ViewState("PRCost"))
        dtMaster.Rows.Add(dtr)
        ds.Tables.Add(dtMaster)
        ds.Tables.Add(dtDetails)
        'ViewState("PRCost") = 0

        'If Session("Env") <> "FTN" Then
        '    ' Columns for PR_CUSTOM_FIELD_MSTR
        '    Dim dtCustomMaster As New DataTable
        '    dvwCus = objAdmin.getCustomField("")
        '    dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        '    dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        '    If Not dvwCus Is Nothing Then
        '        For i = 0 To dvwCus.Count - 1
        '            dtr = dtCustomMaster.NewRow
        '            dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
        '            dtr("FieldName") = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
        '            dtCustomMaster.Rows.Add(dtr)
        '        Next
        '    End If
        '    objAdmin = Nothing
        '    ds.Tables.Add(dtCustomMaster)

        '    ' Columns for PR_CUSTOM_FIELD_DETAILS
        '    Dim dtCustomDetails As New DataTable
        '    dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        '    dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        '    dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))
        '    If Not dvwCus Is Nothing Then
        '        For i = 0 To dvwCus.Count - 1
        '            For Each dgItem In dtgShopping.Items
        '                dtr = dtCustomDetails.NewRow
        '                dtr("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
        '                dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")

        '                Dim cboCustom As DropDownList
        '                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
        '                dtr("FieldValue") = decodeCustomField(cboCustom.SelectedItem.Value)
        '                dtCustomDetails.Rows.Add(dtr)
        '            Next
        '        Next
        '    End If
        '    ds.Tables.Add(dtCustomDetails)
        'End If
        ' Columns for PR_CUSTOM_FIELD_MSTR
        Dim dtCustomMaster As New DataTable
        dvwCus = objAdmin.getCustomField("")
        dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        If Not dvwCus Is Nothing Then
            For i = 0 To dvwCus.Count - 1
                dtr = dtCustomMaster.NewRow
                dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
                dtr("FieldName") = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
                dtCustomMaster.Rows.Add(dtr)
            Next
        End If
        objAdmin = Nothing
        ds.Tables.Add(dtCustomMaster)

        ' Columns for PR_CUSTOM_FIELD_DETAILS
        Dim dtCustomDetails As New DataTable
        dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))
        If Not dvwCus Is Nothing Then
            For i = 0 To dvwCus.Count - 1
                For Each dgItem In dtgShopping.Items
                    dtr = dtCustomDetails.NewRow
                    dtr("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
                    dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")

                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
                    dtr("FieldValue") = decodeCustomField(cboCustom.SelectedItem.Value)
                    dtCustomDetails.Rows.Add(dtr)
                Next
            Next
        End If
        ds.Tables.Add(dtCustomDetails)
        bindPR = ds
    End Function

    Private Sub SavePR()
        Dim dsPR As New DataSet
        Dim objPR As New PR
        Dim strMsg As String = ""
        'Dim blnValid = True
        blnValid = True

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            If Session("urlreferer") <> "Dashboard" Then
                Session("urlreferer") = "PRAll"
            End If
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If

        If validateDatagrid(strMsg) Then
            dsPR = bindPR()
            If ViewState("type") = "new" Then
                Dim strNewPR As String = ""
                Dim intMsg As Integer

                If blnValid And blnValidBill Then
                    intMsg = objPR.insertPR(dsPR, strNewPR, Session("NonFTN"))
                    objPR = Nothing

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            'Jules 2018.08.14 - To ensure page does not re-direct when user saves.
                            'If Session("urlreferer") = "PRAll" Then
                            '    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                            'ElseIf Session("urlreferer") = "Dashboard" Then
                            '    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                            'ElseIf Session("urlreferer") = "BuyerCatSearch" Then
                            '    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                            'ElseIf Session("urlreferer") = "ConCatSearch" Then
                            '    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                            'End If
                            'Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.")
                            Dim strPRIndex As Long = 0
                            strPRIndex = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_PR_NO='" & strNewPR & "' AND PRM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' ")
                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & strPRIndex & "&prid=" & strNewPR & "&type=mod&mode=" & ViewState("mode")))
                            'End modification.             

                            lblPRNo.Text = strNewPR
                            'cmdRaise.Visible = "False"
                            cmdRaise.Enabled = False 'Jules commented 2018.08.16
                            ViewState("blnCmdRaise") = False 'Jules commented 2018.08.16

                            'Jules 2018.08.27
                            'ViewState("prid") = strNewPR
                            'hidNewPR.Value = strNewPR
                            'ViewState("type") = "mod"

                            'If Request.QueryString("frm") = "BuyerCat" Then
                            '    'Construct URL For BC
                            '    'Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=BuyerCat&pageid=" & strPageId)
                            'ElseIf Request.QueryString("frm") = "ConCat" Then
                            '    'Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=ConCat&pageid=" & strPageId)
                            'Else
                            '    'Construct URL For My PR
                            '    Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & Request.QueryString("index") & "&prid=" & ViewState("prid") & "&type=" & ViewState("type") & "&mode=" & Request.QueryString("mode"))
                            'End If
                            'End modification.

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Duplicate
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                        Case -1
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            Common.NetMsgbox(Me, "Vendor is being deleted.", MsgBoxStyle.Information)
                    End Select
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If

                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid And blnValidBill Then
                    objPR.updatePR(dsPR, Session("NonFTN"))
                    objPR = Nothing

                    'Jules 2018.08.14 - To ensure page does not re-direct when user saves.
                    'If Session("urlreferer") = "PRAll" Then
                    '    Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                    'ElseIf Session("urlreferer") = "Dashboard" Then
                    '    Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                    'ElseIf Session("urlreferer") = "BuyerCatSearch" Then
                    '    Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                    'ElseIf Session("urlreferer") = "ConCatSearch" Then
                    '    Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    'End If
                    Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.")
                    'End modification.

                    savePRItemInArray() 'Jules commented 2018.08.27
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If
        Else
            'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
            savePRItemInArray()

            'Jules 2018.07.16 - Moved out of If Else because error message onscreen not cleared even when successfully saved.
            'If strMsg <> "" Then
            '    lblMsg.Text = strMsg
            'Else
            '    lblMsg.Text = ""
            'End If
        End If

        'Jules 2018.07.16 
        If strMsg <> "" Then
            lblMsg.Text = strMsg
        Else
            lblMsg.Text = ""
        End If
    End Sub

    Private Sub cmdRaise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRaise.Click
        SavePR()
        'Bindgrid() 'Jules 2018.08.27
    End Sub

    Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If ViewState("type") = "new" Or ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "bc"
                    Dim strFileName As String
                    Dim strscript As New System.Text.StringBuilder
                    strscript.Append("<script language=""javascript"">")
                    'strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=&RFQ_Cur_value=&RFQ_Cur_Text=")
                    strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId)
                    strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                    strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','700px');")
                    strscript.Append("document.getElementById('btnHidden1').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script13", strscript.ToString())
                Case "cc"
                    Dim strFileName As String
                    Dim strscript As New System.Text.StringBuilder
                    strscript.Append("<script language=""javascript"">")
                    'strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=&RFQ_Cur_value=&RFQ_Cur_Text=")
                    strFileName = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId & "&frm=Concat&show=no")
                    strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                    strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','560px');")
                    strscript.Append("document.getElementById('btnHidden1').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script13", strscript.ToString())
            End Select
        End If
    End Sub

    Private Sub updatePRDIdx()
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim sProductCode As String = ""
        Dim sProductDesc As String = ""
        Dim strSQL As String
        Dim sPrdIndex As Integer
        Dim strAryQuery(0) As String

        'Update the grid PRDIndex Sync with DB
        For Each dgItem In dtgShopping.Items
            If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
            Else
                sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            End If

            sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            strSQL = objPR.updatePRDIndex(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            Common.Insert2Ary(strAryQuery, strSQL)
            objDB.BatchExecute(strAryQuery)
        Next
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim objShopping As New ShoppingCart
        Dim sProductCode As String = ""
        Dim sFullClientId As String, sClientId As String
        Dim sProductDesc As String = ""
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strSQL As String = ""
        Dim strSQL2 As String = ""
        Dim sPrdIndex As Integer
        Dim InternalCount As Integer = 0
        ' Update the grid PRDIndex Sync with DB
        ' This must run and update first
        updatePRDIdx()

        For Each dgItem In dtgShopping.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                Else
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                End If

                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

                If ViewState("type") = "new" Then
                    strSQL = objPR.deletePRItemSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    strSQL = objPR.deletePRItemSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    strSQL2 = objPR.updatePRHeaderSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    Common.Insert2Ary(strAryQuery, strSQL2)
                    Select Case ViewState("mode")
                        Case "pr"
                            ''Case "rfq"
                    End Select
                End If

                sClientId = Mid(sFullClientId, InStr(sFullClientId, "_") + 1, InStr(Mid(sFullClientId, InStr(sFullClientId, "_") + 1), "_") - 1) & "|"
                hidClientId.Value = hidClientId.Value.Replace(sClientId, "")
                hidTotalClientId.Value = hidTotalClientId.Value - 1
                Dim GetPR As Boolean
                GetPR = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                If GetPR = False Then
                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), i, sProductDesc, InternalCount)
                End If
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            i = i + 1
        Next

        InternalCount = 0

        If objDB.BatchExecute(strAryQuery) Then
            Common.NetMsgbox(Me, "Item(s) Deleted.", MsgBoxStyle.Information)
            Dim dsDelivery As New DataSet
            Dim objAdmin As New Admin
            Dim objBudget As New BudgetControl
            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add("D")
            Session("CurrentScreen") = "RemoveItem"
            'If Session("Env") <> "FTN" Then
            '    ' BCM 

            '    'Update the grid PRDIndex Sync with DB
            '    If ViewState("type") = "new" Then
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    ElseIf ViewState("type") = "mod" Then
            '        Dim j As Integer = 0
            '        For Each dgItem In dtgShopping.Items
            '            chkItem = dgItem.FindControl("chkSelection")
            '            If chkItem.Checked Then

            '                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
            '                Else
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                End If

            '                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            '                Dim GetPr As Boolean
            '                GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            '                If GetPr = False Then
            '                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
            '                End If
            '            End If
            '            j = j + 1
            '        Next
            '        updatePRDIdx()
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    End If

            '    Bindgrid()
            '    hidTotal.Value = ""
            '    hid2.Value = ""
            '    objAdmin = Nothing
            '    objBudget = Nothing
            'Else
            '    'Update the grid PRDIndex Sync with DB
            '    If ViewState("type") = "new" Then
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    ElseIf ViewState("type") = "mod" Then
            '        Dim j As Integer = 0
            '        For Each dgItem In dtgShopping.Items
            '            chkItem = dgItem.FindControl("chkSelection")
            '            If chkItem.Checked Then

            '                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
            '                Else
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                End If

            '                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            '                Dim GetPr As Boolean
            '                GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            '                If GetPr = False Then
            '                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
            '                End If
            '            End If
            '            j = j + 1
            '        Next
            '        updatePRDIdx()
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    End If

            '    Bindgrid()
            'End If
            ' BCM 

            'Update the grid PRDIndex Sync with DB
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim j As Integer = 0
                For Each dgItem In dtgShopping.Items
                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then

                        If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                            sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                        Else
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                        End If

                        sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

                        Dim GetPr As Boolean
                        GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                        If GetPr = False Then
                            Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
                        End If
                    End If
                    j = j + 1
                Next
                updatePRDIdx()
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            End If

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim txtBudget, txtDelivery As Label
            'Dim cboGLCode As DropDownList 'Jules commented 2018.10.22 U00019
            Dim strItem, strItemCust As New ArrayList()
            'Dim dgItem As DataGridItem
            Dim k As Integer = 0
            Dim strItemHead As New ArrayList()
            Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2; Jules 2018.10.18
            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.17

            strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules commented 2018.10.22 U00019
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                chkItem = dgItem.FindControl("chkSelection")

                'Jules 2018.05.04 - PAMB Scrum 2
                cboGift = dgItem.FindControl("cboGift")

                'Jules 2018.10.18
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                hidGLCode = dgItem.FindControl("hidGLCode")
                'End modification.

                If chkItem.Checked Then
                Else
                    'Jules 2018.05.04 - PAMB Scrum 2
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                    strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                    'End modification.
                End If

                'Dim objAdmin As New Admin

                dvwCus = objAdmin.getCustomField("")

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        'For Each dgItem2 In dtgShopping.Items
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
                        'Next
                    Next
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            Session("strItemHead") = strItemHead

            Bindgrid() 'Jules commented 2018.08.27
            hidTotal.Value = ""
            hid2.Value = ""
            objAdmin = Nothing
            objBudget = Nothing
        End If
        objPR = Nothing

        If ViewState("type") = "new" Or ViewState("type") = "mod" Then
            Response.Redirect(Session("RaisePRURL"))
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim objPR As New PR
        objPR.deletePR(hidNewPR.Value)

        If Session("urlreferer") = "BuyerCatSearch" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "ConCatSearch" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "PRAll" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("PR", "SearchPR_all.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "Dashboard" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
        End If
        objPR = Nothing
    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        If Not objPR.checkUserAccExist() Then
            strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
        End If

        'If ViewState("blnBill") = 0 Then
        '    If strMsg <> "" Then
        '        strMsg &= """&vbCrLf&"""
        '    End If
        '    strMsg &= "Please ask your Buyer Admin to add in a Billing Address! "
        'End If

        'If ViewState("blnDelivery") = 0 Then
        '    If strMsg <> "" Then
        '        strMsg &= """&vbCrLf&"""
        '    End If
        '    strMsg &= "Please ask your Buyer Admin to add in a Delivery Address! "
        'End If

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.            
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "E")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("prid"), , , , , , "E")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                End If
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()

        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        'Dim cboGLCode As DropDownList 'Jules 2018.10.22 - U00019
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            chkItem = dgItem.FindControl("chkSelection")

            'Jules 2018.05.04 - PAMB Scrum 2
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.17
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            'If chkItem.Checked Then
            'Else 
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If

            'Jules 2018.05.04 - PAMB Scrum 2
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
            'End modification.

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Private Sub cmdUploadInt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadInt.Click
        If File1Int.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1Int.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1Int.PostedFile.ContentLength > 0 And File1Int.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("prid"), , , , , , "I")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                End If
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFileInt()

        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        'Dim cboGLCode As DropDownList 'Jules 2018.10.22 U00019
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            chkItem = dgItem.FindControl("chkSelection")

            'Jules 2018.05.04 - PAMB Scrum 2
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If
            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH

            'Jules 2018.05.04 - PAMB Scrum 2
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
            'End modification.

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        If ViewState("type") = "new" Then
            dsAttach = objPR.getPRTempAttach(Session.SessionID, "E")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            dsAttach = objPR.getPRTempAttach(ViewState("prid"), "E")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objPR = Nothing
    End Sub

    Private Sub displayAttachFileInt()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL As String

        If ViewState("type") = "new" Then
            dsAttach = objPR.getPRTempAttach(Session.SessionID, "I")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            dsAttach = objPR.getPRTempAttach(ViewState("prid"), "I")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        pnlAttachInt.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttachInt

                pnlAttachInt.Controls.Add(lblFile)
                pnlAttachInt.Controls.Add(lnk)
                pnlAttachInt.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttachInt.Controls.Add(lblFile)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        If ViewState("type") = "new" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If
        displayAttachFile()
        objPR = Nothing
    End Sub

    Private Sub deleteAttachInt(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        If ViewState("type") = "new" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If
        displayAttachFileInt()
        objPR = Nothing
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"


        If Not Common.checkMaxLength(txtInternal.Text, 1000) Then
            strMsg &= "<li>For Internal Use is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If Not Common.checkMaxLength(txtExternal.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        Dim dgItem As DataGridItem
        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        'Dim ddl_comm As DropDownList
        Dim hidBudgetCode As TextBox

        For Each dgItem In dtgShopping.Items

            'Dim hidDelCode As TextBox
            'hidDelCode = dgItem.FindControl("hidDelCode")
            'If hidDelCode.Text = "" Or hidDelCode.Text = "&nbsp;" Then
            '    strMsg &= "<li>Invalid Delivery Address.<ul type='disc'></ul></li>"
            '    validateDatagrid = False
            'End If

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            If Not IsDate(txtEstDate.Text) Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PR creation date (dd/mm/yyyy).<ul type='disc'></ul></li>"
                validateDatagrid = False
            Else
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                If txtDate2 <= txtDate1 Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PR creation date (dd/mm/yyyy).<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            Dim lblVendor As Label
            lblVendor = dgItem.FindControl("lblVendor")

            If lblVendor.Text <> "" And lblVendor.Text <> "&nbsp;" Then
                Dim strRec_No As String = objDB.GetVal("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & lblVendor.Text & "'")
                If strRec_No = "" Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Vendor not approved.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            Dim strGstRegNo As String
            Dim objGST As New GST
            Dim lblGSTRate, lblTaxCode As Label
            lblGSTRate = dgItem.FindControl("lblGSTRate")
            lblTaxCode = dgItem.FindControl("lblTaxCode")

            If ViewState("CheckGST") = True Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                        If lblVendor.Text <> "" And lblVendor.Text <> "&nbsp;" Then
                            strGstRegNo = objGST.chkGST(lblVendor.Text)
                            If strGstRegNo <> "" AndAlso (lblGSTRate.Text = "N/A" Or lblGSTRate.Text = "" Or lblGSTRate.Text = "&nbsp;") Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". SST Rate is required for SST registered vendor.<ul type='disc'></ul></li>"
                                validateDatagrid = False
                            Else
                                If strGstRegNo <> "" AndAlso (lblTaxCode.Text = "N/A" Or lblTaxCode.Text = "" Or lblTaxCode.Text = "&nbsp;") Then
                                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". SST Tax Code is required.<ul type='disc'></ul></li>"
                                    validateDatagrid = False
                                Else

                                End If
                            End If
                        End If
                End Select
            End If

            txtRemark = dgItem.FindControl("txtRemark")
            txtQ = dgItem.FindControl("txtQ")

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            'ddl_comm = dgItem.FindControl("ddl_comm")
            'If ddl_comm.SelectedItem.Text = "" Or ddl_comm.SelectedItem.Text = "---Select---" Then
            '    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Commodity type not selected.<ul type='disc'></ul></li>"
            '    validateDatagrid = False
            'End If

            Dim txtCommodity As TextBox
            txtCommodity = dgItem.FindControl("txtCommodity")
            If txtCommodity.Text = "" Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Commodity type is required.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            Dim strComm As String = objDB.GetVal("SELECT IFNULL(CT_NAME,'') FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(txtCommodity.Text) & "' LIMIT 1 ")
            If IsDBNull(strComm) Or strComm = "" Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid commodity type.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            'If Session("Env") <> "FTN" Then
            '    If ViewState("BCM") > 0 Then
            '        hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            '        If hidBudgetCode.Text = "" Then
            '            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account not selected.<ul type='disc'></ul></li>"
            '            validateDatagrid = False
            '        End If
            '    End If
            'End If
            If ViewState("BCM") > 0 Then
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                If hidBudgetCode.Text = "" Then
                    'Jules 2018.10.19
                    'strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account not selected.<ul type='disc'></ul></li>"
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Cost Centre Code not selected.<ul type='disc'></ul></li>"
                    'End modification.
                    validateDatagrid = False

                Else 'Jules 2018.07.16 - Need to check whether Budget Account is still valid
                    Dim drTemp As DataRow()
                    drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                    If drTemp.Length = 0 Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account is not valid.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
            End If

            'Jules 2018.10.22 U00019
            'Jules 2018.07.31 - Set to 'N/A'
            'Dim cboGLCode, cboCategoryCode As DropDownList
            'Dim cboGLCode As DropDownList            
            'cboGLCode = dgItem.FindControl("cboGLCode")
            Dim hidGLCode As TextBox
            hidGLCode = dgItem.FindControl("hidGLCode")
            'cboCategoryCode = dgItem.FindControl("cboCategoryCode")

            'Dim strCat As String = objDB.GetVal("SELECT IFNULL(GROUP_CONCAT(CBGC_B_CATEGORY_CODE), '') AS CBGC_B_CATEGORY_CODE FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "' AND (SELECT COUNT(CBGC_B_GL_CODE) FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "') > 0")
            'If strCat <> "" Then
            '    If cboGLCode.SelectedItem.ToString <> "Not Applicable" And Not strCat.Contains(cboCategoryCode.SelectedItem.ToString) Then
            '        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGLO.GetErrorMessage("00370") & "<ul type='disc'></ul></li>"
            '        validateDatagrid = False
            '    End If
            'End If '

            'Jules 2018.08.06
            'If cboGLCode.SelectedItem.Text = "---Select---" Then
            If hidGLCode.Text = "" OrElse hidGLCode.Text Is Nothing Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GL Code is required.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
            'End modification.

            'Jules 2018.05.03 - PAMB Scrum 2
            'If cboGLCode.SelectedValue.ToString <> "" AndAlso cboGLCode.SelectedItem.Text <> "---Select---" AndAlso validateDatagrid = True Then 
            If hidGLCode.Text <> "" AndAlso validateDatagrid = True Then 'Jules 2018.10.22 U00019
                'Jules 2018.10.18
                'Dim cboFundType, cboPersonCode, cboProjectCode As DropDownList                
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                Dim hidFundType, hidPersonCode, hidProjectCode As TextBox
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                'End modification.

                'Jules 2018.10.22 U00019
                'Dim strSql As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "'"
                Dim strSql As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & hidGLCode.Text & "'"
                Dim dsAnalysisCodes As DataSet = objDB.FillDs(strSql)
                If dsAnalysisCodes.Tables(0).Rows.Count > 0 Then
                    'Jules 2018.10.18 - Changed from dropdownlist to textbox
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And cboFundType.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And hidFundType.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGLO.GetErrorMessage("00374") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And cboPersonCode.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And hidPersonCode.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGLO.GetErrorMessage("00375") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And cboProjectCode.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And hidProjectCode.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGLO.GetErrorMessage("00376") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                    'End modification.
                End If
            End If
            'End modification.

            ' 10Jun2014 yAP: Temporary solution for different contract price with PR price.
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
                    Dim strSql As String
                    strSql = "SELECT CDI_UNIT_COST AS UNITCOST "
                    strSql &= "FROM CONTRACT_DIST_COY "
                    strSql &= "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
                    strSql &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
                    strSql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX "
                    strSql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID "
                    strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID "
                    strSql &= "WHERE CDC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                    strSql &= "AND CDM_GROUP_INDEX = '" & CType(dgItem.FindControl("lblCDGroup"), Label).Text & "' "
                    strSql &= "AND PM_PRODUCT_CODE = '" & dgItem.Cells(EnumShoppingCart.icProductCode).Text & "' AND PM_DELETED <> 'Y' "

                    Dim strCatPrice As String = objDB.GetVal(strSql)
                    Dim txtPrice As TextBox
                    txtPrice = dgItem.FindControl("txtPrice")

                    If strCatPrice <> CDbl(txtPrice.Text) Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid contract price.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
            End Select
        Next
        strMsg &= "</ul>"
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_AddPR_tabs") = "<div class=""t_entity""><ul>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=new&mode=bc&frm=bc") & """><span>Purchase Request</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId) & """><span>Purchase Request Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId) & """><span>Purchase Request Cancellation</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "</ul><div></div></div>"
    End Sub

    'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
    'Function to store all info in temp array
    Public Sub savePRItemInArray()
        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        'Dim cboGLCode As DropDownList 'Jules 2018.10.22 U00019
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.05.04 - PAMB Scrum 2; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.22 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            chkItem = dgItem.FindControl("chkSelection")

            'Jules 2018.05.04 - PAMB Scrum 2
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If

            'Jules 2018.05.04 - PAMB Scrum 2
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
            'End modification.

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click

    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim dsPR As New DataSet
        Dim objPR As New PR
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""
        Dim blnExceed As Boolean

        Dim dt As New DataTable

        blnValid = True
        If validateDatagrid(strMsg) Then
            dsPR = bindPR()
            Dim dsapplist As New DataSet
            dsapplist = objPR.getAppovalList("A", CDbl(ViewState("PRCost")), "PR", True)
            If dsapplist.Tables(0).Rows.Count = 0 Then
                strMsg &= "<li>There is no approval list available for this PR because the sequence of approving officers do not have the approval limit to approve it.<ul type='disc'></ul></li>"
                lblMsg.Text = strMsg
                ViewState("PRCost") = Nothing
                Exit Sub
            End If

            'Michelle (11/1/2013) - Issue 1832
            Dim objAdmin As New Admin
            ViewState("blnGRNCtrl") = objAdmin.withGRNCtrl()
            If ViewState("blnGRNCtrl") Then
                'Check whether user is with SK fixed role
                Dim strFixRole As String
                Dim objUserRole As New UserRoles
                strFixRole = objUserRole.get_UserFixedRole()
                If strFixRole.IndexOf("Store Keeper") <> -1 Then
                    'Check whether there is outstanding GRN
                    Dim dsOVendor As New DataSet
                    Dim objDO As New DeliveryOrder
                    Dim intLoop As Integer
                    Dim blnWithOGRN As Boolean = False
                    dsOVendor = objDO.GetDO2("", "")
                    For intLoop = 0 To dsOVendor.Tables(0).Rows.Count - 1
                        If objDO.ChkOustdGRN(dsOVendor.Tables(0).Rows(intLoop)("DOM_S_COY_ID"), dsOVendor.Tables(0).Rows(intLoop)("DO_DAYS")) Then
                            Dim strMsg1 As String
                            strMsg1 = "Your access to submit PR is denied due to your account still has outstanding GRN pending your action."
                            strMsg1 &= """& vbCrLf & """ & "Please raise GRN Accept if you had received the physical delivery or services, else create GRN Reject if you had not received any physical delivery or services from the vendors."
                            strMsg1 &= """& vbCrLf & """ & "Please save your PR as draft, and create GRN Accept or Reject to obtain the access to submit PR for approval."
                            ViewState("PRCost") = Nothing
                            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Exclamation)
                            Exit Sub
                        End If
                    Next
                End If

            End If
            'End If
            'End If

            If ViewState("type") = "new" Then
                Dim strNewPR As String = ""
                Dim intMsg As Integer

                If blnValid = True And blnValidBill = True Then
                    lblMsg.Text = ""

                    'If Session("Env") = "FTN" Then
                    '    dt = objPR.getPRApprFlow(True)
                    'Else
                    '    dt = objPR.getPRApprFlow(False)
                    'End If
                    dt = objPR.getPRApprFlow(False)

                    Dim CheckApprB4 As Boolean = True
                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        CheckApprB4 = False
                    End If

                    'If Not IsNumeric(Mid(lblPONo.Text, 3)) Then
                    If lblPRNo.Text = "To Be Allocated By System" And CheckApprB4 = True Then
                        intMsg = objPR.insertPR(dsPR, strNewPR, Session("NonFTN"))
                    Else
                        'If Session("Env") = "FTN" Then
                        '    dt = objPR.getPRApprFlow(True)
                        'Else
                        '    dt = objPR.getPRApprFlow(False)
                        'End If
                        dt = objPR.getPRApprFlow(False)

                        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                            Exit Sub
                        ElseIf dt.Rows.Count > 1 Then

                            If Request.QueryString("frm") = "BuyerCat" Then
                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                            ElseIf Request.QueryString("frm") = "ConCat" Then
                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                            ElseIf Not Request.QueryString("type") = "" Then
                                If Request.QueryString("mode") = "bc" Then
                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                Else
                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                End If
                            End If
                            Exit Sub
                        Else
                            objPR.updatePR(dsPR, Session("NonFTN"))
                            intMsg = WheelMsgNum.Save
                            strNewPR = lblPRNo.Text
                        End If
                    End If
                    'objPR = Nothing
                    'redirect to ExceedBCM before approval page
                    Select Case intMsg
                        Case WheelMsgNum.Save
                            If blnItem Then ' item exists

                                'If Session("Env") = "FTN" Then
                                '    dt = objPR.getPRApprFlow(True)
                                'Else
                                '    dt = objPR.getPRApprFlow(False)
                                'End If
                                dt = objPR.getPRApprFlow(False)

                                'If Session("Env") <> "FTN" Then
                                '    If ViewState("BCM") > 0 Then
                                '        If checkMandatory(strMsg) Then
                                '            Dim objBudget As New BudgetControl
                                '            Dim strBCM As String
                                '            Dim dtBCM As New DataTable
                                '            blnExceed = objBudget.checkBCM(strNewPR, dtBCM, strBCM)

                                '            'Dim dt As New DataTable
                                '            'Dim intMsg As Integer

                                '            If Not blnExceed Then
                                '                If Session("Env") = "FTN" Then
                                '                    dt = objPR.getPRApprFlow(True)
                                '                Else
                                '                    dt = objPR.getPRApprFlow(False)
                                '                End If

                                '                ViewState("mode") = Request.QueryString("mode")
                                '                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '                    Exit Sub
                                '                ElseIf dt.Rows.Count > 1 Then
                                '                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                '                    Exit Sub
                                '                Else
                                '                    Dim dsAO As New DataSet
                                '                    Dim UM_APP_LIMIT As Double
                                '                    Dim PR_Type As String = ""
                                '                    Dim blnBuyerWNoWork As Boolean = False

                                '                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                '                        blnBuyerWNoWork = True
                                '                    Else
                                '                        ViewState("ApprovalType") = objPR.getApprovalType
                                '                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                '                        ' case Automated Approval
                                '                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                '                            UM_APP_LIMIT = 0
                                '                        Else
                                '                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                '                        End If

                                '                        ' A - Automated Approval
                                '                        ' B - Allow Lower Limit Endorsement
                                '                        ' C - Cut PO before end of Aproval List
                                '                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                '                        Select Case ViewState("ApprovalType")
                                '                            Case "C"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                    If ViewState("blnCutPR") = True Then
                                '                                        PR_Type = "None"
                                '                                    Else
                                '                                        PR_Type = "Approval"
                                '                                        ViewState("blnCutPR") = True
                                '                                    End If
                                '                                Else
                                '                                    PR_Type = "None"
                                '                                End If

                                '                            Case "B"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                    PR_Type = "Approval"
                                '                                Else
                                '                                    PR_Type = "Endorsement"
                                '                                End If

                                '                            Case "B+C"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                    If ViewState("blnCutPR") = True Then
                                '                                        PR_Type = "None"
                                '                                    Else
                                '                                        PR_Type = "Approval"
                                '                                        ViewState("blnCutPR") = True
                                '                                    End If
                                '                                Else
                                '                                    PR_Type = "Endorsement"
                                '                                End If

                                '                            Case "A"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                    PR_Type = "Approval"
                                '                                Else
                                '                                    PR_Type = "None"
                                '                                End If

                                '                        End Select

                                '                        Select Case PR_Type
                                '                            Case "None"
                                '                                PR_Type = "0"
                                '                            Case "Approval"
                                '                                PR_Type = "1"
                                '                            Case "Endorsement"
                                '                                PR_Type = "2"
                                '                        End Select
                                '                    End If

                                '                    If blnBuyerWNoWork = True Then
                                '                        ViewState("msg") = Nothing
                                '                    Else
                                '                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                '                    End If

                                '                    intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                '                    Select Case intMsg
                                '                        Case WheelMsgNum.Save
                                '                            If Session("urlreferer") = "BuyerCatSearch" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                '                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                '                            ElseIf Session("urlreferer") = "PRAll" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                '                            End If

                                '                        Case WheelMsgNum.NotSave
                                '                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                '                        Case WheelMsgNum.Delete
                                '                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                '                    End Select
                                '                End If
                                '            Else

                                '                If strBCM = "2" Then
                                '                    If Session("Env") = "FTN" Then
                                '                        dt = objPR.getPRApprFlow(True)
                                '                    Else
                                '                        dt = objPR.getPRApprFlow(False)
                                '                    End If

                                '                    ViewState("mode") = Request.QueryString("mode")
                                '                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '                        Exit Sub
                                '                    ElseIf dt.Rows.Count > 1 Then
                                '                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                '                        Exit Sub
                                '                    Else
                                '                        Dim dsAO As New DataSet
                                '                        Dim UM_APP_LIMIT As Double
                                '                        Dim PR_Type As String = ""
                                '                        Dim blnBuyerWNoWork As Boolean = False

                                '                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                '                            blnBuyerWNoWork = True
                                '                        Else
                                '                            ViewState("ApprovalType") = objPR.getApprovalType
                                '                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                '                            ' case Automated Approval
                                '                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                '                                UM_APP_LIMIT = 0
                                '                            Else
                                '                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                '                            End If

                                '                            ' A - Automated Approval
                                '                            ' B - Allow Lower Limit Endorsement
                                '                            ' C - Cut PO before end of Aproval List
                                '                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                '                            Select Case ViewState("ApprovalType")
                                '                                Case "C"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                        If ViewState("blnCutPR") = True Then
                                '                                            PR_Type = "None"
                                '                                        Else
                                '                                            PR_Type = "Approval"
                                '                                            ViewState("blnCutPR") = True
                                '                                        End If
                                '                                    Else
                                '                                        PR_Type = "None"
                                '                                    End If

                                '                                Case "B"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                        PR_Type = "Approval"
                                '                                    Else
                                '                                        PR_Type = "Endorsement"
                                '                                    End If

                                '                                Case "B+C"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                        If ViewState("blnCutPR") = True Then
                                '                                            PR_Type = "None"
                                '                                        Else
                                '                                            PR_Type = "Approval"
                                '                                            ViewState("blnCutPR") = True
                                '                                        End If
                                '                                    Else
                                '                                        PR_Type = "Endorsement"
                                '                                    End If

                                '                                Case "A"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                        PR_Type = "Approval"
                                '                                    Else
                                '                                        PR_Type = "None"
                                '                                    End If

                                '                            End Select

                                '                            Select Case PR_Type
                                '                                Case "None"
                                '                                    PR_Type = "0"
                                '                                Case "Approval"
                                '                                    PR_Type = "1"
                                '                                Case "Endorsement"
                                '                                    PR_Type = "2"
                                '                            End Select
                                '                        End If

                                '                        If blnBuyerWNoWork = True Then
                                '                            ViewState("msg") = Nothing
                                '                        Else
                                '                            ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                '                        End If

                                '                        intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                '                        Select Case intMsg
                                '                            Case WheelMsgNum.Save
                                '                                If Session("urlreferer") = "BuyerCatSearch" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                '                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                '                                ElseIf Session("urlreferer") = "PRAll" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                '                                End If

                                '                            Case WheelMsgNum.NotSave
                                '                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                '                            Case WheelMsgNum.Delete
                                '                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                '                        End Select
                                '                    End If
                                '                Else
                                '                    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&mode=" & ViewState("mode")))
                                '                End If
                                '            End If
                                '        Else
                                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                '        End If
                                '    Else
                                '        'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '        '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '        '    Exit Sub
                                '        'ElseIf dt.Rows.Count > 1 Then
                                '        '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '        '    Exit Sub
                                '        'Else
                                '        lblPRNo.Text = strNewPR
                                '        SubmitPR()
                                '        'End If
                                '        'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    End If
                                'Else
                                '    'If ViewState("BCM") > 0 Then
                                '    'If checkMandatory(strMsg) Then
                                '    '    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&mode=" & ViewState("mode")))
                                '    'Else
                                '    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                '    'End If
                                '    'Else
                                '    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '    '    Exit Sub
                                '    'ElseIf dt.Rows.Count > 1 Then
                                '    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    '    Exit Sub
                                '    'Else
                                '    lblPRNo.Text = strNewPR
                                '    SubmitPR()
                                '    'End If
                                '    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    'End If
                                'End If
                                If ViewState("BCM") > 0 Then
                                    If checkMandatory(strMsg) Then
                                        Dim objBudget As New BudgetControl
                                        Dim strBCM As String
                                        Dim dtBCM As New DataTable
                                        blnExceed = objBudget.checkBCM(strNewPR, dtBCM, strBCM)

                                        'Dim dt As New DataTable
                                        'Dim intMsg As Integer

                                        If Not blnExceed Then
                                            'If Session("Env") = "FTN" Then
                                            '    dt = objPR.getPRApprFlow(True)
                                            'Else
                                            '    dt = objPR.getPRApprFlow(False)
                                            'End If
                                            dt = objPR.getPRApprFlow(False)

                                            ViewState("mode") = Request.QueryString("mode")
                                            If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                                Exit Sub
                                            ElseIf dt.Rows.Count > 1 Then
                                                If Request.QueryString("frm") = "BuyerCat" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                ElseIf Request.QueryString("frm") = "ConCat" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                ElseIf Not Request.QueryString("type") = "" Then
                                                    If Request.QueryString("mode") = "bc" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                    Else
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                    End If
                                                End If
                                                Exit Sub
                                            Else
                                                Dim dsAO As New DataSet
                                                Dim UM_APP_LIMIT As Double
                                                Dim PR_Type As String = ""
                                                Dim blnBuyerWNoWork As Boolean = False

                                                If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                    blnBuyerWNoWork = True
                                                Else
                                                    ViewState("ApprovalType") = objPR.getApprovalType
                                                    dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                    ' case Automated Approval
                                                    If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                        UM_APP_LIMIT = 0
                                                    Else
                                                        UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                    End If

                                                    ' A - Automated Approval
                                                    ' B - Allow Lower Limit Endorsement
                                                    ' C - Cut PO before end of Aproval List
                                                    ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                    Select Case ViewState("ApprovalType")
                                                        Case "C"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                If ViewState("blnCutPR") = True Then
                                                                    PR_Type = "None"
                                                                Else
                                                                    PR_Type = "Approval"
                                                                    ViewState("blnCutPR") = True
                                                                End If
                                                            Else
                                                                PR_Type = "None"
                                                            End If

                                                        Case "B"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                PR_Type = "Approval"
                                                            Else
                                                                PR_Type = "Endorsement"
                                                            End If

                                                        Case "B+C"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                If ViewState("blnCutPR") = True Then
                                                                    PR_Type = "None"
                                                                Else
                                                                    PR_Type = "Approval"
                                                                    ViewState("blnCutPR") = True
                                                                End If
                                                            Else
                                                                PR_Type = "Endorsement"
                                                            End If

                                                        Case "A"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                PR_Type = "Approval"
                                                            Else
                                                                PR_Type = "None"
                                                            End If

                                                    End Select

                                                    Select Case PR_Type
                                                        Case "None"
                                                            PR_Type = "0"
                                                        Case "Approval"
                                                            PR_Type = "1"
                                                        Case "Endorsement"
                                                            PR_Type = "2"
                                                    End Select
                                                End If

                                                If blnBuyerWNoWork = True Then
                                                    ViewState("msg") = Nothing
                                                Else
                                                    ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                                End If

                                                intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                                Select Case intMsg
                                                    Case WheelMsgNum.Save
                                                        If Session("urlreferer") = "BuyerCatSearch" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                        ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                        ElseIf Session("urlreferer") = "PRAll" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                        End If

                                                    Case WheelMsgNum.NotSave
                                                        Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                    Case WheelMsgNum.Delete
                                                        Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                                End Select
                                            End If
                                        Else

                                            If strBCM = "2" Then
                                                'If Session("Env") = "FTN" Then
                                                '    dt = objPR.getPRApprFlow(True)
                                                'Else
                                                '    dt = objPR.getPRApprFlow(False)
                                                'End If
                                                dt = objPR.getPRApprFlow(False)

                                                ViewState("mode") = Request.QueryString("mode")
                                                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                                    Exit Sub
                                                ElseIf dt.Rows.Count > 1 Then
                                                    If Request.QueryString("frm") = "BuyerCat" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                    ElseIf Request.QueryString("frm") = "ConCat" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                    ElseIf Not Request.QueryString("type") = "" Then
                                                        If Request.QueryString("mode") = "bc" Then
                                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                        Else
                                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                        End If
                                                    End If
                                                    Exit Sub
                                                Else
                                                    Dim dsAO As New DataSet
                                                    Dim UM_APP_LIMIT As Double
                                                    Dim PR_Type As String = ""
                                                    Dim blnBuyerWNoWork As Boolean = False

                                                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                        blnBuyerWNoWork = True
                                                    Else
                                                        ViewState("ApprovalType") = objPR.getApprovalType
                                                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                        ' case Automated Approval
                                                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                            UM_APP_LIMIT = 0
                                                        Else
                                                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                        End If

                                                        ' A - Automated Approval
                                                        ' B - Allow Lower Limit Endorsement
                                                        ' C - Cut PO before end of Aproval List
                                                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                        Select Case ViewState("ApprovalType")
                                                            Case "C"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                    If ViewState("blnCutPR") = True Then
                                                                        PR_Type = "None"
                                                                    Else
                                                                        PR_Type = "Approval"
                                                                        ViewState("blnCutPR") = True
                                                                    End If
                                                                Else
                                                                    PR_Type = "None"
                                                                End If

                                                            Case "B"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                    PR_Type = "Approval"
                                                                Else
                                                                    PR_Type = "Endorsement"
                                                                End If

                                                            Case "B+C"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                    If ViewState("blnCutPR") = True Then
                                                                        PR_Type = "None"
                                                                    Else
                                                                        PR_Type = "Approval"
                                                                        ViewState("blnCutPR") = True
                                                                    End If
                                                                Else
                                                                    PR_Type = "Endorsement"
                                                                End If

                                                            Case "A"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                    PR_Type = "Approval"
                                                                Else
                                                                    PR_Type = "None"
                                                                End If

                                                        End Select

                                                        Select Case PR_Type
                                                            Case "None"
                                                                PR_Type = "0"
                                                            Case "Approval"
                                                                PR_Type = "1"
                                                            Case "Endorsement"
                                                                PR_Type = "2"
                                                        End Select
                                                    End If

                                                    If blnBuyerWNoWork = True Then
                                                        ViewState("msg") = Nothing
                                                    Else
                                                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                                    End If

                                                    intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                                    Select Case intMsg
                                                        Case WheelMsgNum.Save
                                                            If Session("urlreferer") = "BuyerCatSearch" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                            ElseIf Session("urlreferer") = "PRAll" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                            End If

                                                        Case WheelMsgNum.NotSave
                                                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                        Case WheelMsgNum.Delete
                                                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                                    End Select
                                                End If
                                            Else
                                                Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&mode=" & ViewState("mode")))
                                            End If
                                        End If
                                    Else
                                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    End If
                                Else
                                    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                    '    Exit Sub
                                    'ElseIf dt.Rows.Count > 1 Then
                                    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                    '    Exit Sub
                                    'Else
                                    lblPRNo.Text = strNewPR
                                    SubmitPR()
                                    'End If
                                    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                End If
                            Else
                                Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                            End If

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Duplicate
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                        Case -1
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            Common.NetMsgbox(Me, "Vendor is being deleted.", MsgBoxStyle.Information)
                    End Select
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid = True And blnValidBill = True Then
                    lblMsg.Text = ""
                    If blnItem Then ' item exists

                        'If Session("Env") = "FTN" Then
                        '    dt = objPR.getPRApprFlow(True)
                        'Else
                        '    dt = objPR.getPRApprFlow(False)
                        'End If
                        dt = objPR.getPRApprFlow(False)

                        'If Session("Env") <> "FTN" Then
                        '    If ViewState("BCM") > 0 Then
                        '        If checkMandatory(strMsg) Then
                        '            objPR.updatePR(dsPR, Session("NonFTN"))

                        '            Dim objBudget As New BudgetControl
                        '            Dim strBCM As String
                        '            Dim dtBCM As New DataTable
                        '            blnExceed = objBudget.checkBCM(hidNewPR.Value, dtBCM, strBCM)

                        '            'Dim dt As New DataTable
                        '            Dim intMsg As Integer

                        '            If Not blnExceed Then
                        '                If Session("Env") = "FTN" Then
                        '                    dt = objPR.getPRApprFlow(True)
                        '                Else
                        '                    dt = objPR.getPRApprFlow(False)
                        '                End If

                        '                ViewState("mode") = Request.QueryString("mode")
                        '                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '                    Exit Sub
                        '                ElseIf dt.Rows.Count > 1 Then
                        '                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                        '                    Exit Sub
                        '                Else
                        '                    Dim dsAO As New DataSet
                        '                    Dim UM_APP_LIMIT As Double
                        '                    Dim PR_Type As String = ""
                        '                    Dim blnBuyerWNoWork As Boolean = False

                        '                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                        '                        blnBuyerWNoWork = True
                        '                    Else
                        '                        ViewState("ApprovalType") = objPR.getApprovalType
                        '                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                        '                        ' case Automated Approval
                        '                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                        '                            UM_APP_LIMIT = 0
                        '                        Else
                        '                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                        '                        End If

                        '                        ' A - Automated Approval
                        '                        ' B - Allow Lower Limit Endorsement
                        '                        ' C - Cut PO before end of Aproval List
                        '                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                        '                        Select Case ViewState("ApprovalType")
                        '                            Case "C"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                    If ViewState("blnCutPR") = True Then
                        '                                        PR_Type = "None"
                        '                                    Else
                        '                                        PR_Type = "Approval"
                        '                                        ViewState("blnCutPR") = True
                        '                                    End If
                        '                                Else
                        '                                    PR_Type = "None"
                        '                                End If

                        '                            Case "B"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                    PR_Type = "Approval"
                        '                                Else
                        '                                    PR_Type = "Endorsement"
                        '                                End If

                        '                            Case "B+C"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                    If ViewState("blnCutPR") = True Then
                        '                                        PR_Type = "None"
                        '                                    Else
                        '                                        PR_Type = "Approval"
                        '                                        ViewState("blnCutPR") = True
                        '                                    End If
                        '                                Else
                        '                                    PR_Type = "Endorsement"
                        '                                End If

                        '                            Case "A"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                    PR_Type = "Approval"
                        '                                Else
                        '                                    PR_Type = "None"
                        '                                End If

                        '                        End Select

                        '                        Select Case PR_Type
                        '                            Case "None"
                        '                                PR_Type = "0"
                        '                            Case "Approval"
                        '                                PR_Type = "1"
                        '                            Case "Endorsement"
                        '                                PR_Type = "2"
                        '                        End Select
                        '                    End If

                        '                    If blnBuyerWNoWork = True Then
                        '                        ViewState("msg") = Nothing
                        '                    Else
                        '                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                        '                    End If

                        '                    intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                        '                    Select Case intMsg
                        '                        Case WheelMsgNum.Save
                        '                            If Session("urlreferer") = "BuyerCatSearch" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                        '                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                        '                            ElseIf Session("urlreferer") = "PRAll" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                        '                            End If

                        '                        Case WheelMsgNum.NotSave
                        '                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        '                        Case WheelMsgNum.Delete
                        '                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                        '                    End Select
                        '                End If
                        '            Else

                        '                If strBCM = "2" Then
                        '                    If Session("Env") = "FTN" Then
                        '                        dt = objPR.getPRApprFlow(True)
                        '                    Else
                        '                        dt = objPR.getPRApprFlow(False)
                        '                    End If

                        '                    ViewState("mode") = Request.QueryString("mode")
                        '                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '                        Exit Sub
                        '                    ElseIf dt.Rows.Count > 1 Then
                        '                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                        '                        Exit Sub
                        '                    Else
                        '                        Dim dsAO As New DataSet
                        '                        Dim UM_APP_LIMIT As Double
                        '                        Dim PR_Type As String = ""
                        '                        Dim blnBuyerWNoWork As Boolean = False

                        '                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                        '                            blnBuyerWNoWork = True
                        '                        Else
                        '                            ViewState("ApprovalType") = objPR.getApprovalType
                        '                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                        '                            ' case Automated Approval
                        '                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                        '                                UM_APP_LIMIT = 0
                        '                            Else
                        '                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                        '                            End If

                        '                            ' A - Automated Approval
                        '                            ' B - Allow Lower Limit Endorsement
                        '                            ' C - Cut PO before end of Aproval List
                        '                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                        '                            Select Case ViewState("ApprovalType")
                        '                                Case "C"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                        If ViewState("blnCutPR") = True Then
                        '                                            PR_Type = "None"
                        '                                        Else
                        '                                            PR_Type = "Approval"
                        '                                            ViewState("blnCutPR") = True
                        '                                        End If
                        '                                    Else
                        '                                        PR_Type = "None"
                        '                                    End If

                        '                                Case "B"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                        PR_Type = "Approval"
                        '                                    Else
                        '                                        PR_Type = "Endorsement"
                        '                                    End If

                        '                                Case "B+C"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                        If ViewState("blnCutPR") = True Then
                        '                                            PR_Type = "None"
                        '                                        Else
                        '                                            PR_Type = "Approval"
                        '                                            ViewState("blnCutPR") = True
                        '                                        End If
                        '                                    Else
                        '                                        PR_Type = "Endorsement"
                        '                                    End If

                        '                                Case "A"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                        PR_Type = "Approval"
                        '                                    Else
                        '                                        PR_Type = "None"
                        '                                    End If

                        '                            End Select

                        '                            Select Case PR_Type
                        '                                Case "None"
                        '                                    PR_Type = "0"
                        '                                Case "Approval"
                        '                                    PR_Type = "1"
                        '                                Case "Endorsement"
                        '                                    PR_Type = "2"
                        '                            End Select
                        '                        End If

                        '                        intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                        '                        Select Case intMsg
                        '                            Case WheelMsgNum.Save
                        '                                If Session("urlreferer") = "BuyerCatSearch" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                        '                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                        '                                ElseIf Session("urlreferer") = "PRAll" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                        '                                End If

                        '                            Case WheelMsgNum.NotSave
                        '                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        '                            Case WheelMsgNum.Delete
                        '                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                        '                        End Select
                        '                    End If
                        '                Else
                        '                    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId & "&mode=" & ViewState("mode")))
                        '                End If
                        '            End If
                        '        Else
                        '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '        End If
                        '    Else
                        '        'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '        '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '        '    Exit Sub
                        '        'ElseIf dt.Rows.Count > 1 Then
                        '        '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                        '        '    Exit Sub
                        '        'Else
                        '        objPR.updatePR(dsPR, Session("NonFTN"))
                        '        SubmitPR()
                        '        'End If
                        '        'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                        '    End If
                        'Else
                        '    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '    '    Exit Sub
                        '    'ElseIf dt.Rows.Count > 1 Then
                        '    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                        '    '    Exit Sub
                        '    'Else
                        '    objPR.updatePR(dsPR, Session("NonFTN"))
                        '    SubmitPR()
                        '    'End If
                        'End If
                        If ViewState("BCM") > 0 Then
                            If checkMandatory(strMsg) Then
                                objPR.updatePR(dsPR, Session("NonFTN"))

                                Dim objBudget As New BudgetControl
                                Dim strBCM As String
                                Dim dtBCM As New DataTable
                                blnExceed = objBudget.checkBCM(hidNewPR.Value, dtBCM, strBCM)

                                'Dim dt As New DataTable
                                Dim intMsg As Integer

                                If Not blnExceed Then
                                    'If Session("Env") = "FTN" Then
                                    '    dt = objPR.getPRApprFlow(True)
                                    'Else
                                    '    dt = objPR.getPRApprFlow(False)
                                    'End If
                                    dt = objPR.getPRApprFlow(False)

                                    ViewState("mode") = Request.QueryString("mode")
                                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                        Exit Sub
                                    ElseIf dt.Rows.Count > 1 Then
                                        If Request.QueryString("frm") = "BuyerCat" Then
                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                        ElseIf Request.QueryString("frm") = "ConCat" Then
                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                        ElseIf Not Request.QueryString("type") = "" Then
                                            If Request.QueryString("mode") = "bc" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                            Else
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                            End If
                                        End If
                                        Exit Sub
                                    Else
                                        Dim dsAO As New DataSet
                                        Dim UM_APP_LIMIT As Double
                                        Dim PR_Type As String = ""
                                        Dim blnBuyerWNoWork As Boolean = False

                                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                            blnBuyerWNoWork = True
                                        Else
                                            ViewState("ApprovalType") = objPR.getApprovalType
                                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                            ' case Automated Approval
                                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                UM_APP_LIMIT = 0
                                            Else
                                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                            End If

                                            ' A - Automated Approval
                                            ' B - Allow Lower Limit Endorsement
                                            ' C - Cut PO before end of Aproval List
                                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                            Select Case ViewState("ApprovalType")
                                                Case "C"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                        If ViewState("blnCutPR") = True Then
                                                            PR_Type = "None"
                                                        Else
                                                            PR_Type = "Approval"
                                                            ViewState("blnCutPR") = True
                                                        End If
                                                    Else
                                                        PR_Type = "None"
                                                    End If

                                                Case "B"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                        PR_Type = "Approval"
                                                    Else
                                                        PR_Type = "Endorsement"
                                                    End If

                                                Case "B+C"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                        If ViewState("blnCutPR") = True Then
                                                            PR_Type = "None"
                                                        Else
                                                            PR_Type = "Approval"
                                                            ViewState("blnCutPR") = True
                                                        End If
                                                    Else
                                                        PR_Type = "Endorsement"
                                                    End If

                                                Case "A"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                        PR_Type = "Approval"
                                                    Else
                                                        PR_Type = "None"
                                                    End If

                                            End Select

                                            Select Case PR_Type
                                                Case "None"
                                                    PR_Type = "0"
                                                Case "Approval"
                                                    PR_Type = "1"
                                                Case "Endorsement"
                                                    PR_Type = "2"
                                            End Select
                                        End If

                                        If blnBuyerWNoWork = True Then
                                            ViewState("msg") = Nothing
                                        Else
                                            ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                        End If

                                        intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                        Select Case intMsg
                                            Case WheelMsgNum.Save
                                                If Session("urlreferer") = "BuyerCatSearch" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                ElseIf Session("urlreferer") = "PRAll" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                End If

                                            Case WheelMsgNum.NotSave
                                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                            Case WheelMsgNum.Delete
                                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                        End Select
                                    End If
                                Else

                                    If strBCM = "2" Then
                                        'If Session("Env") = "FTN" Then
                                        '    dt = objPR.getPRApprFlow(True)
                                        'Else
                                        '    dt = objPR.getPRApprFlow(False)
                                        'End If
                                        dt = objPR.getPRApprFlow(False)

                                        ViewState("mode") = Request.QueryString("mode")
                                        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                            Exit Sub
                                        ElseIf dt.Rows.Count > 1 Then
                                            If Request.QueryString("frm") = "BuyerCat" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                            ElseIf Request.QueryString("frm") = "ConCat" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                            ElseIf Not Request.QueryString("type") = "" Then
                                                If Request.QueryString("mode") = "bc" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                Else
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                                End If
                                            End If

                                            Exit Sub
                                        Else
                                            Dim dsAO As New DataSet
                                            Dim UM_APP_LIMIT As Double
                                            Dim PR_Type As String = ""
                                            Dim blnBuyerWNoWork As Boolean = False

                                            If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                blnBuyerWNoWork = True
                                            Else
                                                ViewState("ApprovalType") = objPR.getApprovalType
                                                dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                ' case Automated Approval
                                                If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                    UM_APP_LIMIT = 0
                                                Else
                                                    UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                End If

                                                ' A - Automated Approval
                                                ' B - Allow Lower Limit Endorsement
                                                ' C - Cut PO before end of Aproval List
                                                ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                Select Case ViewState("ApprovalType")
                                                    Case "C"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                            If ViewState("blnCutPR") = True Then
                                                                PR_Type = "None"
                                                            Else
                                                                PR_Type = "Approval"
                                                                ViewState("blnCutPR") = True
                                                            End If
                                                        Else
                                                            PR_Type = "None"
                                                        End If

                                                    Case "B"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                            PR_Type = "Approval"
                                                        Else
                                                            PR_Type = "Endorsement"
                                                        End If

                                                    Case "B+C"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                            If ViewState("blnCutPR") = True Then
                                                                PR_Type = "None"
                                                            Else
                                                                PR_Type = "Approval"
                                                                ViewState("blnCutPR") = True
                                                            End If
                                                        Else
                                                            PR_Type = "Endorsement"
                                                        End If

                                                    Case "A"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                            PR_Type = "Approval"
                                                        Else
                                                            PR_Type = "None"
                                                        End If

                                                End Select

                                                Select Case PR_Type
                                                    Case "None"
                                                        PR_Type = "0"
                                                    Case "Approval"
                                                        PR_Type = "1"
                                                    Case "Endorsement"
                                                        PR_Type = "2"
                                                End Select
                                            End If

                                            intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                            Select Case intMsg
                                                Case WheelMsgNum.Save
                                                    If Session("urlreferer") = "BuyerCatSearch" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                    ElseIf Session("urlreferer") = "PRAll" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                    End If

                                                Case WheelMsgNum.NotSave
                                                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                Case WheelMsgNum.Delete
                                                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                            End Select
                                        End If
                                    Else
                                        Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId & "&mode=" & ViewState("mode")))
                                    End If
                                End If
                            Else
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            End If
                        Else
                            'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                            '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                            '    Exit Sub
                            'ElseIf dt.Rows.Count > 1 Then
                            '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                            '    Exit Sub
                            'Else
                            objPR.updatePR(dsPR, Session("NonFTN"))
                            SubmitPR()
                            'End If
                            'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                    End If
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If
        Else
            'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
            savePRItemInArray()

            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Sub SubmitPR()
        Dim dt As New DataTable
        Dim objPR As New PR
        Dim objPR2 As New PurchaseReq2
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim intMsg As Integer

        'If Session("Env") = "FTN" Then
        '    dt = objPR.getPRApprFlow(True)
        'Else
        '    dt = objPR.getPRApprFlow(False)
        'End If
        dt = objPR.getPRApprFlow(False)

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            If Session("urlreferer") <> "Dashboard" Then
                Session("urlreferer") = "PRAll"
            End If
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If

        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf dt.Rows.Count > 1 Then 'more than 1 approval flow
            If Request.QueryString("frm") = "BuyerCat" Then
                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
            ElseIf Request.QueryString("frm") = "ConCat" Then
                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
            ElseIf Not Request.QueryString("type") = "" Then
                If Request.QueryString("mode") = "bc" Then
                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                Else
                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                End If
            End If
            Exit Sub
        Else ' no approval flow or 1 approval workflow only
            Dim dsAO As New DataSet
            Dim UM_APP_LIMIT As Double
            Dim PR_Type As String = ""
            Dim blnBuyerWNoWork As Boolean = False

            If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                blnBuyerWNoWork = True
            Else
                ViewState("ApprovalType") = objPR.getApprovalType
                dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                ' case Automated Approval
                If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                    UM_APP_LIMIT = 0
                Else
                    UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                End If

                ' A - Automated Approval
                ' B - Allow Lower Limit Endorsement
                ' C - Cut PO before end of Aproval List
                ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                Select Case ViewState("ApprovalType")
                    Case "C"
                        If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                            If ViewState("blnCutPR") = True Then
                                PR_Type = "None"
                            Else
                                PR_Type = "Approval"
                                ViewState("blnCutPR") = True
                            End If
                        Else
                            PR_Type = "None"
                        End If

                    Case "B"
                        If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                            PR_Type = "Approval"
                        Else
                            PR_Type = "Endorsement"
                        End If

                    Case "B+C"
                        If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                            If ViewState("blnCutPR") = True Then
                                PR_Type = "None"
                            Else
                                PR_Type = "Approval"
                                ViewState("blnCutPR") = True
                            End If
                        Else
                            PR_Type = "Endorsement"
                        End If

                    Case "A"
                        If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                            PR_Type = "Approval"
                        Else
                            PR_Type = "None"
                        End If

                End Select

                Select Case PR_Type
                    Case "None"
                        PR_Type = "0"
                    Case "Approval"
                        PR_Type = "1"
                    Case "Endorsement"
                        PR_Type = "2"
                End Select
            End If
            'added by michael
            If blnBuyerWNoWork = True Then
                ViewState("msg") = Nothing
            Else
                ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
            End If
            ''''''''''''''''''''''''''
            intMsg = objPR.submitPR(lblPRNo.Text, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)

            'If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
            '    Dim strRemark, strMsg, strSql As String

            '    'strRemark = FormatAORemark("approve")
            '    'strRemark = strRemark & "System Auto Approval"

            '    strRemark = ""

            '    strSql = "SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID= '" & Session("CompanyId") & "' AND PRM_PR_NO ='" & lblPRNo.Text & "'"
            '    ViewState("PRIndex") = objDB.GetVal(strSql)

            '    'strMsg = objPR2.ApprovePR(lblPRNo.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), ViewState("Consolidator"), strRemark, ViewState("Requestor"), Request.QueryString("relief"), ViewState("ApprType"))
            '    strMsg = objPR2.ApprovePR(lblPRNo.Text, ViewState("PRIndex"), 1, True, String.Empty, strRemark, Session("UserId"), False, 1, "BCNoWorkFlow")
            'End If

            Select Case intMsg
                Case WheelMsgNum.Save
                    If Session("urlreferer") = "BuyerCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "PRAll" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                        'lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    End If

                    If Request.QueryString("frm") = "bc" Then
                        cmdAdd.Visible = False
                        cmdRemove.Visible = False
                        cmdDupPRLine.Visible = False
                    End If

                    cmdRaise.Visible = False
                    cmdSetup.Visible = False
                    cmdSubmit.Visible = False

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Delete
                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

            End Select
        End If
    End Sub

    'Private Sub cboGLCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGLCode.SelectedIndexChanged
    '    Dim dsCategoryCode As New DataSet
    '    Dim objAdminCat As New Admin
    '    Dim dgItem As DataGridItem
    '    Dim cboCategoryCode As DropDownList 

    '    For Each dgItem In dtgShopping.Items
    '        cboCategoryCode = dgItem.FindControl("cboCategoryCode")
    '        Dim PFORC As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & dgItem.Cells(EnumShoppingCart.icProductCode).Text & "'")
    '        dsCategoryCode = objAdminCat.PopulateCategoryCodeMstr(dgItem.Cells(EnumShoppingCart.icProductCode).Text, dgItem.Cells(EnumShoppingCart.icCategoryCode).Text, PFORC)
    '        dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
    '        Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)
    '    Next

    '    objAdminCat = Nothing
    'End Sub

    'Private Sub cboGLCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGLCode.TextChanged
    '    Dim dsCategoryCode As New DataSet
    '    Dim objAdminCat As New Admin
    '    Dim dgItem As DataGridItem
    '    Dim cboCategoryCode As DropDownList

    '    For Each dgItem In dtgShopping.Items
    '        cboCategoryCode = dgItem.FindControl("cboCategoryCode")
    '        Dim PFORC As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & dgItem.Cells(EnumShoppingCart.icProductCode).Text & "'")
    '        dsCategoryCode = objAdminCat.PopulateCategoryCodeMstr(dgItem.Cells(EnumShoppingCart.icProductCode).Text, dgItem.Cells(EnumShoppingCart.icCategoryCode).Text, PFORC)
    '        dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
    '        Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)
    '    Next

    '    objAdminCat = Nothing
    'End Sub

    Protected Sub cboGLCode_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim cboGLCode, cboGift As New DropDownList
        Dim dgitem As DataGridItem = CType((CType(sender, Control)).Parent.Parent, DataGridItem)
        cboGLCode = dgitem.FindControl("cboGLCode")
        cboGift = dgitem.FindControl("cboGift")

        If cboGLCode.SelectedValue.ToString.Substring(0, 1) = "1" Then
            cboGift.SelectedValue = "N"
            cboGift.Enabled = False
        Else
            cboGift.Enabled = True
        End If
    End Sub
End Class
