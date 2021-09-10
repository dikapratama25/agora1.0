Imports AgoraLegacy
Imports eProcure.Component


Public Class RFQ_VendorDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblCoyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblParentCoy As System.Web.UI.WebControls.Label
    Protected WithEvents lblCoyLongName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBusinessRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblCity As System.Web.UI.WebControls.Label
    Protected WithEvents lblPostCode As System.Web.UI.WebControls.Label
    Protected WithEvents lnkWebsite As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPhone As System.Web.UI.WebControls.Label
    Protected WithEvents lnkEmail As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblGSTRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccountNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrgCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBranchCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaid As System.Web.UI.WebControls.Label
    Protected WithEvents lblOwnership As System.Web.UI.WebControls.Label
    Protected WithEvents lblYear As System.Web.UI.WebControls.Label
    Protected WithEvents ddlOwnerShip As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCommodity As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboParentCoy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbocountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlBusiness As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblLocalSales As System.Web.UI.WebControls.Label
    Protected WithEvents lblExportSales As System.Web.UI.WebControls.Label
    Protected WithEvents dtgSalesTurnover As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgApp As System.Web.UI.WebControls.DataGrid
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents dtgQS As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblLogo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCoyOwnership As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaidCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents lblBusiness As System.Web.UI.WebControls.Label
    Protected WithEvents lblCommodity As System.Web.UI.WebControls.Label
    Protected WithEvents lblState As System.Web.UI.WebControls.Label
    Protected WithEvents lblCountry As System.Web.UI.WebControls.Label
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents Image3 As System.Web.UI.WebControls.Image
    Protected WithEvents Image4 As System.Web.UI.WebControls.Image
    Protected WithEvents Image5 As System.Web.UI.WebControls.Image
    Protected WithEvents Image6 As System.Web.UI.WebControls.Image
    Protected WithEvents lnkBack As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button

    Dim objFile As New FileManagement


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

    End Sub

#End Region
    Dim strFrm As String
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
       

        PopulateCodeTable()
        Populate()
        displayAttachFile()
        linkweb()

        'If Not IsPostBack Then
        '    SetGridProperty(dtgSalesTurnover)
        '    SetGridProperty(dtgApp)
        '    SetDashboardGridProperty(dtgPendingAppr)
        '    dtgPendingAppr.CurrentPageIndex = 0
        '    ViewState("SortAscendingPendingAppr") = "yes"
        '    ViewState("SortExpressionPendingAppr") = "Submitted Date"
        '    BindgridPendingAppr()

        '    dtgSalesTurnover.CurrentPageIndex = 0
        '    ViewState("SortExpression") = "CS_YEAR"

        '    Bindgrid()
        '    dtgApp.CurrentPageIndex = 0
        '    bindgridApp()
        'End If
        SetGridProperty(dtgSalesTurnover)
        SetGridProperty(dtgApp)
        Bindgrid()
        bindgridApp()

        If Request.QueryString("frm") = "RFQComSummary" Then
            lnkBack.Visible = True
            cmdClose.Visible = False
            lnkBack.Attributes.Remove("onclick")

            If (Me.Request.QueryString("SubFrm") = "RFQ_List" Or Me.Request.QueryString("SubFrm") = "Dashboard") Then
                If Request.QueryString("Appr") = "Y" Then
                    If Me.Request.QueryString("SubFrm") = "Dashboard" Then
                        lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                    Else
                        lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                    End If
                Else
                    lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                End If
            Else
                If Request.QueryString("Appr") = "Y" Then
                    lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                Else
                    If Me.Request.QueryString("SubFrm") = "RFQ_Outstg_List" Then
                        lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                    Else
                        lnkBack.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
                    End If
                End If
            End If



        ElseIf Request.QueryString("frm") = "Comlist" Then
            lnkBack.Visible = False
            cmdClose.Visible = True
            'lnkBack.HRef = dDispatcher.direct("RFQ", "Comlist.aspx", "pageid=" & Request(Trim("pageid")) & "&side=" & Request.QueryString("side") & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
        ElseIf Request.QueryString("frm") = "RFQ_VenList" Then
            lnkBack.Visible = False
            cmdClose.Visible = True
        ElseIf Request.QueryString("frm") = "ApprovedVendor" Then
            lnkBack.HRef = dDispatcher.direct("Admin", "ApprovedVendor.aspx", "type=" & Request.QueryString("type") & "&pageid=" & Request(Trim("pageid")))
            lnkBack.Visible = True
            cmdClose.Visible = False
        Else
            lnkBack.Visible = True
            cmdClose.Visible = False
        End If
        MyBase.Page_Load(sender, e)

        Image2.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
        Image3.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
        Image4.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
        Image5.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
        Image6.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")

    End Sub
    Private Sub displayAttachFile()
        Dim objFile As New FileManagement
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        Dim objCompDetails As New Company

        dsAttach = objFile.getAttachment(Request.QueryString("v_com_id"), Request.QueryString("v_com_id"), "QS")

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "QS", EnumUploadFrom.FrontOff)
                
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblfile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objFile = Nothing
    End Sub
    Public Sub dtgSalesTurnover_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSalesTurnover.PageIndexChanged
        dtgSalesTurnover.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
    Function bindgridApp(Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet
        Dim objAdm As New Admin
        ds = objAdm.getSoftwareVendorDetail(Request.QueryString("v_com_id"))

        Dim dvViewapp As DataView
        dvViewapp = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewapp.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewapp.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgApp, dvViewapp)
            dtgApp.DataSource = dvViewapp
            dtgApp.DataBind()
            'lblRed.Visible = True
        Else
            resetDatagridPageIndex(dtgApp, dvViewapp)
            dtgSalesTurnover.DataSource = dvViewapp
            dtgApp.DataBind()
            'lblRed.Visible = False
        End If
        ' add for above checking
        ViewState("PageCount") = dtgApp.PageCount

    End Function
    Public Sub dtgApp_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgApp.CurrentPageIndex = e.NewPageIndex
        bindgridApp()
    End Sub

    Private Sub dtgApp_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgApp, e)
    End Sub
    Private Sub dtgApp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objSI As New Admin
        Dim ds As New DataSet



        ds = objSI.getSalesInfoList(Request.QueryString("v_com_id"))


        '//for sorting asc or desc
        Dim dvViewSI As DataView
        dvViewSI = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSI.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSI.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgSalesTurnover, dvViewSI)
            dtgSalesTurnover.DataSource = dvViewSI
            dtgSalesTurnover.DataBind()
        Else
            resetDatagridPageIndex(dtgSalesTurnover, dvViewSI)
            dtgSalesTurnover.DataSource = dvViewSI
            dtgSalesTurnover.DataBind()

        End If

        ViewState("PageCount") = dtgSalesTurnover.PageCount
    End Function
    Private Sub dtgSalesTurnover_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSalesTurnover.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgSalesTurnover, e)
        '//to add a JavaScript to CheckAll button
    End Sub
    Private Sub dtgSalesTurnover_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSalesTurnover.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            If IsDBNull(dv("CS_Amount")) Then
                e.Item.Cells(2).Text = "N/A"
            Else
                e.Item.Cells(2).Text = Format(dv("CS_Amount"), "#,##0.00")

            End If
        End If
    End Sub
    Private Sub PopulateCodeTable()
        Dim objGlobal As New AppGlobals

        objGlobal.FillCompany(cboParentCoy, "Ehub")
        objGlobal.FillCodeTable(cboState, CodeTable.State)
        objGlobal.FillCodeTable(cbocountry, CodeTable.Country)
        objGlobal.FillCodeTable(ddlCurrency, CodeTable.Currency)
        objGlobal.FillCodeTable(ddlOwnerShip, CodeTable.OwnerShip)
        objGlobal.FillCodeTable(ddlBusiness, CodeTable.Business)
        'objGlobal.FillCommodityType(ddlCommodity)
        cboParentCoy.Enabled = False
        cboState.Enabled = False
        cbocountry.Enabled = False
        ddlCurrency.Enabled = False
        ddlOwnerShip.Enabled = False
        ddlBusiness.Enabled = False
        ddlCommodity.Enabled = False


    End Sub
    Private Sub linkweb()
        Dim link, email As String
        link = lnkWebsite.Text
        email = lnkEmail.Text
        lnkEmail.NavigateUrl = "mailto:" & email
        lnkWebsite.NavigateUrl = link
        lnkWebsite.Attributes.Add("onclick", "window.open('" & link & "');return false;")

        


    End Sub
    Private Sub Populate()
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        objCompDetails = objComp.GetCompanyDetails(Request.QueryString("v_com_id"))
        If Not objCompDetails Is Nothing Then

            lblCoyName.Text = objCompDetails.CoyName
            lblBankName.Text = objCompDetails.BankName
            lblAccountNo.Text = objCompDetails.AccountNo
            lblBankCode.Text = objCompDetails.BankCode
            lblBranchCode.Text = objCompDetails.BranchCode
            lblAddress.Text = objCompDetails.Address1
            lblAddress2.Text = objCompDetails.Address2
            lblAddress3.Text = objCompDetails.Address3
            lblCity.Text = objCompDetails.City
            lblPostCode.Text = objCompDetails.PostCode
            lblPhone.Text = objCompDetails.Phone
            lblFax.Text = objCompDetails.Fax
            lnkEmail.Text = objCompDetails.Email
            'txtCompanyLogo.Text = objCompDetails.CoyLogo
            'Michelle (20/9/2010) - To remove the time
            lblBusinessRegNo.Text = objCompDetails.BusinessRegNo
            lblGSTRegNo.Text = objCompDetails.TaxRegNo
            'txtTC.Text = objCompDetails.TC

            If objCompDetails.GSTDateLastStatus <> "" Then
                Dim intDays As Integer = Date.Now.Subtract(CDate(objCompDetails.GSTDateLastStatus)).Days
                lblGSTRegNo.Text &= ", " & Format(CDate(objCompDetails.GSTDateLastStatus), "dd/MM/yyyy") & " (" & intDays & " Days Lapsed)"
            End If

            lblCoyLongName.Text = objCompDetails.CoyLongName
            lnkWebsite.Text = objCompDetails.WebSites
            If Not IsDBNull(objCompDetails.PaidUpCapital) And objCompDetails.PaidUpCapital <> "" Then
                lblPaid.Text = Format(CDbl(objCompDetails.PaidUpCapital), "#0.00") 'CStr(objCompDetails.PaidUpCapital)
            End If
            lblOwnership.Text = objCompDetails.OwnershipOthers
            lblOrgCode.Text = objCompDetails.OrgCode


            lblYear.Text = objCompDetails.RegYear

            Common.SelDdl(Common.parseNull(objCompDetails.Country), cbocountry, True, True)
            If objCompDetails.Country <> "MY" Then
                Dim objGlobal As New AppGlobals
                objGlobal.FillState(cboState, cbocountry.SelectedItem.Value)
                objGlobal = Nothing
            End If
            lblCountry.Text = cbocountry.SelectedItem.Text

            Common.SelDdl(Common.parseNull(objCompDetails.State), cboState, True, True)
            If cboState.SelectedItem.Value = "" Then
                lblState.Text = ""
            ElseIf cboState.SelectedItem.Text = "---Select---" Then
                lblState.Text = ""
            Else
                lblState.Text = cboState.SelectedItem.Text
            End If

            Common.SelDdl(Common.parseNull(objCompDetails.ParentCoy), cboParentCoy, True, True)
            If cboParentCoy.SelectedItem.Value = "" Then
                lblParentCoy.Text = ""
            ElseIf cboParentCoy.SelectedItem.Text = "---Select---" Then
                lblParentCoy.Text = ""
            Else
                lblParentCoy.Text = cboParentCoy.SelectedItem.Text
            End If

            Common.SelDdl(Common.parseNull(objCompDetails.RegCurrency), ddlCurrency, True, True)
            If ddlCurrency.SelectedItem.Value = "" Then
                lblPaidCurrency.Text = ""
            ElseIf ddlCurrency.SelectedItem.Text = "---Select---" Then
                lblPaidCurrency.Text = ""
            Else
                lblPaidCurrency.Text = ddlCurrency.SelectedItem.Text
            End If

            Common.SelDdl(Common.parseNull(objCompDetails.Ownership), ddlOwnerShip, True, True)
            If ddlOwnerShip.SelectedItem.Value = "" Then
                lblCoyOwnership.Text = ""
            ElseIf ddlOwnerShip.SelectedItem.Text = "---Select---" Then
                lblCoyOwnership.Text = ""
            Else
                lblCoyOwnership.Text = ddlOwnerShip.SelectedItem.Text
            End If

            Common.SelDdl(Common.parseNull(objCompDetails.Business), ddlBusiness, True, True)
            If ddlBusiness.SelectedItem.Value = "" Then
                lblBusiness.Text = ""
            ElseIf ddlBusiness.SelectedItem.Text = "---Select---" Then
                lblBusiness.Text = ""
            Else
                lblBusiness.Text = ddlBusiness.SelectedItem.Text
            End If
            Dim commodity As String
            If Not IsDBNull(objCompDetails.Commodity) And objCompDetails.Commodity <> "" Then
                '            ddlCommodity.SelectedItem = objCompDetails.Commodity
                'Common.SelDdl(Common.parseNull(objCompDetails.Commodity), ddlCommodity, True, True)
                Dim objdb As New EAD.DBCom
                commodity = objdb.Get1Column("commodity_type", "CT_NAME", " WHERE CT_ID='" & objCompDetails.Commodity & "'")

            End If
            If commodity = "" Then
                lblCommodity.Text = ""
            Else
                lblCommodity.Text = commodity
            End If






            DislayLogo()
            ViewState("CoyLogo") = objCompDetails.CoyLogo
            ViewState("Actual_TC") = objCompDetails.Actual_TC
            ViewState("TC") = objCompDetails.TC

            ' strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
        End If


        '####Sales Area####
        Dim objSales As New Admin

        lblLocalSales.Text = objSales.GetLocalSalesArea(Request.QueryString("v_com_id"))
        lblExportSales.Text = objSales.GetExportSalesArea(Request.QueryString("v_com_id"))
    End Sub

    Sub DislayLogo()
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, (Request.QueryString("v_com_id")))
        If strImgSrc <> "" Then
            Image1.Visible = True
            Image1.ImageUrl = strImgSrc
            lblLogo.Visible = False
        Else
            Image1.Visible = False
            lblLogo.Visible = True
        End If
        objFile = Nothing
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub
End Class
