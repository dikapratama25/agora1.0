Imports AgoraLegacy
Imports eProcure.Component
Public Class searchDO
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents cboDocType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCreationDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtBuyerComp As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkDraft As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkSummited As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkPartially As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkFully As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkrejected As System.Web.UI.WebControls.CheckBox
    '    Protected WithEvents chkInvoiced As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDO As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSubmittedDt As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSubmittedDt As System.Web.UI.WebControls.Label
    Protected WithEvents divDtesubmit As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divDtesubmit1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtOurRefNo As System.Web.UI.WebControls.TextBox

    Dim ObjDO As New DeliveryOrder
    Dim dDispatcher As New AgoraLegacy.dispatcher
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
    Public Enum EnumDo
        Do_No
        Ref_No
        C_Date
        DO_Date
        PO_No
        PO_Date
        C_Name
        I_Code
        S_desc
    End Enum

    Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            GenerateTab()
        End If

        divDtesubmit.Style.Item("display") = "none"
        divDtesubmit1.Style.Item("display") = "none"
        blnCheckBox = False
        SetGridProperty(dtgDO)
        If Not Page.IsPostBack Then
            cboDocType.SelectedIndex = 1
        End If
        If cboDocType.SelectedIndex <> 1 Then
            divDtesubmit1.Style.Item("display") = "none"
            divDtesubmit.Style.Item("display") = "none"
        Else
            divDtesubmit.Style.Item("display") = ""
            divDtesubmit1.Style.Item("display") = ""
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgDO.CurrentPageIndex = 0
        viewstate("SortAscending") = "no"
        viewstate("SortExpression") = "DOM_Created_Date"
        Bindgrid()
    End Sub

    Sub dtgDO_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDO.PageIndexChanged
        dtgDO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDO.SortCommand
        Grid_SortCommand(sender, e)
        dtgDO.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strDocType, strDocNo, strOurRef, strCreationDt, strSubmittedDt, strBuyerComp, strVenItem, strStatus As String
        Dim intTotRecord As Integer
        Dim intchk As Integer
        Dim objDO As New DeliveryOrder

        intchk = 0
        If cboDocType.SelectedItem.Value <> "0" Then
            strDocType = cboDocType.SelectedItem.Value
        End If

        If txtDocNo.Text <> "" Then
            strDocNo = Trim(txtDocNo.Text)
        End If

        If txtCreationDate.Text <> "" Then
            strCreationDt = txtCreationDate.Text
        End If

        If txtSubmittedDt.Text <> "" Then
            strSubmittedDt = txtSubmittedDt.Text
        End If

        If txtOurRefNo.Text <> "" Then
            strOurRef = Trim(txtOurRefNo.Text)
        End If

        If txtBuyerComp.Text <> "" Then
            strBuyerComp = Trim(txtBuyerComp.Text)
        End If

        If txtVenItemCode.Text <> "" Then
            strVenItem = Trim(txtVenItemCode.Text)
        End If

        If chkDraft.Checked = True Then
            strStatus = strStatus & DOStatus.Draft
        End If

        If chkSummited.Checked = True Then
            strStatus = strStatus & DOStatus.Submitted
        End If

        If chkFully.Checked = True Then
            strStatus = strStatus & DOStatus.FullyAccepted & DOStatus.Invoiced
        End If

        If chkPartially.Checked = True Then
            strStatus = strStatus & DOStatus.PartiallyAccepted
        End If

        If chkrejected.Checked = True Then
            strStatus = strStatus & DOStatus.Rejected
        End If

        'If chkInvoiced.Checked = True Then
        '    strStatus = strStatus & DOStatus.Invoiced
        'End If

        If Len(strStatus) <> 0 Then
            Dim strTmp, strTmp1, strTmp2 As String
            Dim I As Integer
            I = 0
            strTmp2 = strStatus
            For intchk = 0 To Len(strTmp2) - 1
                I = I + 1
                strTmp1 = Mid(strTmp2, I, 1)
                If Len(strStatus) <> (intchk + 1) Then
                    If Len(strStatus) > 1 Then
                        strTmp1 = strTmp1 & ","
                    End If
                End If
                strTmp = strTmp & strTmp1
            Next
            strStatus = "(" & strTmp & ")"
        End If
        '//Retrieve Data from Database
        Dim dsDO As DataSet = New DataSet
        dsDO = objDO.GetDO(strDocType, strDocNo, strCreationDt, strSubmittedDt, strOurRef, strBuyerComp, strVenItem, strStatus)

        '//for sorting asc or desc
        Dim dvViewDO As DataView
        dvViewDO = dsDO.Tables(0).DefaultView
        dvViewDO.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" And viewstate("SortExpression") <> "" Then dvViewDO.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgDO.CurrentPageIndex > 0 And dsDO.Tables(0).Rows.Count Mod dtgDO.PageSize = 0 Then
                dtgDO.CurrentPageIndex = dtgDO.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If
        intTotRecord = dsDO.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intTotRecord > 0 Then
            resetDatagridPageIndex(dtgDO, dvViewDO)
            dtgDO.DataSource = dvViewDO
            dtgDO.DataBind()
        Else
            dtgDO.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        viewstate("PageCount") = dtgDO.PageCount
    End Function

    Private Sub dtgDO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDO.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgDO, e)
    End Sub

    Private Sub dtgDO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDO.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            '//to dynamic build hyoerlink
            Dim lnkDONum, lnkPONum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            If dv("DOM_DO_STATUS") = DOStatus.Draft Then
                'lnkDONum.NavigateUrl = "AddDO.aspx?DONo=" & dv("DOM_DO_NO") & "&DOIdx=" & dv("DOM_DO_Index") & "&POIdx=" & dv("POM_PO_Index") & "&Mode=Edit&pageid=" & strPageId & "&LocID=" & dv("DOM_D_ADDR_CODE") & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_Coy_ID")
                lnkDONum.NavigateUrl = "AddDO.aspx?DONo=" & dv("DOM_DO_NO") & "&POIdx=" & dv("POM_PO_Index") & "&Mode=Edit&pageid=" & strPageId & "&LocID=" & dv("DOM_D_ADDR_CODE") & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_Coy_ID")

            Else
                lnkDONum.NavigateUrl = "DODetails.aspx?Frm=Listing&DONo=" & dv("DOM_DO_NO") & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&SCoyID=" & Session("CompanyID")
            End If

            lnkPONum = e.Item.FindControl("lnkPONum")
            lnkPONum.Text = dv("POM_PO_No")
            lnkPONum.NavigateUrl = "javascript:;"
            'lnkPONum.Attributes.Add("onclick", "return PopWindow('../PO/POReport.aspx?po_no=" & dv("POM_PO_NO") & "&pageid=" & strPageId & "&BCoyID=" & dv("POM_B_Coy_ID") & "');")
            'lnkPONum.Attributes.Add("onclick", "return PopWindow('../PO/PreviewPO.aspx?PO_No=" & dv("POM_PO_NO") & "&pageid=" & strPageId & "&BCoyID=" & dv("POM_B_Coy_ID") & "');")
            lnkPONum.Attributes.Add("onclick", "return  PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_No=" & dv("POM_PO_NO") & "&BCoyID=" & dv("POM_B_Coy_ID")) & "');")


            Dim strVIC As String
            strVIC = ObjDO.getVendorItemCode(dv("POM_PO_NO"), dv("POM_B_Coy_ID"), dv("DOM_DO_NO"), txtVenItemCode.Text)

            e.Item.Cells(EnumDo.I_Code).Text = strVIC
            e.Item.Cells(EnumDo.C_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_Created_Date"))
            e.Item.Cells(EnumDo.DO_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_DO_Date"))
            e.Item.Cells(EnumDo.PO_Date).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_Date"))
            'Michelle (7/10/2010) - To regard Invoiced as Fully Delivered
            If dv("Status_Desc") = "Invoiced" Then e.Item.Cells(EnumDo.S_desc).Text = "Fully Delivered"
            'Michelle (23/1/2013) - Issue 1727
            If ObjDO.withAttach(dv("DOM_DO_NO")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                lnkDONum.Controls.Add(imgAttach)
                e.Item.Cells(EnumDo.Do_No).Controls.Add(imgAttach)
            End If
        End If
    End Sub


    Private Sub cboDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDocType.SelectedIndexChanged
        If cboDocType.SelectedItem.Value = "DO" Then
            divDtesubmit.Style.Item("display") = ""
            divDtesubmit1.Style.Item("display") = ""
        Else
            divDtesubmit.Style.Item("display") = "none"
            divDtesubmit1.Style.Item("display") = "none"
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Session("w_SearchDO_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        Session("w_SearchDO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

    End Sub


End Class
