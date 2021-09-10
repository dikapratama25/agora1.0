Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class WriteOffDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents dtgWODtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblWONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblWODate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button
    Protected WithEvents pnlAttach2 As System.Web.UI.WebControls.Panel

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dispatcher As New dispatcher
    Dim strFrm, strWONo As String
    Dim objINV As New Inventory
    Dim dsAllInfo, ds As DataSet

    Enum EnumMRS
        icItemCode
        icItemName
        icUom
        icStatus
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
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        htPageAccess.Add("update", alButtonList)
        htPageAccess.Add("add", alButtonList)
        If Request.QueryString("frm") <> "Dashboard" Then
            CheckButtonAccess()
        End If
        If Not ViewState("blnButtonState") Then
            ButtonProperty(False)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgWODtl)

        strPageId = Session("strPageId")
        strWONo = Me.Request.QueryString("WONO")
        strFrm = Me.Request.QueryString("frm")
        lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "WriteOffSearch.aspx", "pageid=" & strPageId)
        lblWONo.Text = strWONo

        If Not Page.IsPostBack Then
            dsAllInfo = objINV.WOInfo(lblWONo.Text)
            PopulateWOHeader()
            displayAttachFile()
            GenerateTab()
            Bindgrid(True)
        End If

        Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewWO.aspx", "WO_No=" & Request(Trim("WONO")) & "&CoyID=" & Session("CompanyID") & "") & "')")
    End Sub

    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgWODtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgMRSDtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgWODtl.ItemCreated
        Grid_ItemCreated(dtgWODtl, e)
    End Sub

    Private Sub dtgWODtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgWODtl.ItemDataBound
        Dim dsRIDtl As DataSet

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            dsRIDtl = Nothing
        End If

    End Sub
    'Sub DisplayData()
    '    Dim dsAllInfo As New DataSet
    '    Dim strDeptName As String
    '    dsAllInfo = objINV.getIRHeaderDetail(Me.Request.QueryString("WONO"))
    '    lblWONo.Text = Me.Request.QueryString("WONO")

    'End Sub

    Public Sub dtgWODtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgWODtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If (e.CommandSource).CommandName = "" Then
            strFileName = ""
            strName = ""
        End If
    End Sub

    Private Sub PopulateWOHeader()
        Dim strMsg As String

        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables("INVENTORY_WRITE_OFF_MSTR_DETAILS")


        If dtHeader.Rows.Count > 0 Then
            lblWODate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IWOM_WO_DATE"))
            lblStatus.Text = IIf(dtHeader.Rows(0)("IWOM_WO_STATUS") = 1, "Submitted", dtHeader.Rows(0)("IWOM_WO_STATUS2"))
            lblRemark.Text = Common.parseNull(dtHeader.Rows(0)("IWOM_WO_REMARK"))
            If dtHeader.Rows(0)("IWOM_WO_STATUS") <> 1 Then
                cmdCancel.Visible = False
            End If
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        ''//Retrieve Data from Database
        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim ds As New DataSet


        ds = objINV.getInventoryItemInfoFiltered(Request.QueryString("WONO"), True)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        Dim dvViewWO As DataView
        dvViewWO = dsAllInfo.Tables("INVENTORY_WRITE_OFF_MSTR_DETAILS").DefaultView

        dvViewWO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewWO.Sort += " DESC"

        ViewState("intPageRecordCnt") = dsAllInfo.Tables("INVENTORY_WRITE_OFF_MSTR_DETAILS").Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            'resetDatagridPageIndex(dtgMRSDtl, dvViewWO)
            dtgWODtl.DataSource = dvViewWO
            dtgWODtl.DataBind()
        Else
            dtgWODtl.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        'ViewState("PageCount") = dtgMRSDtl.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_WriteOffDetail_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "WriteOff.aspx", "pageid=" & strPageId) & """><span>Write Off</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "WriteOffSearch.aspx", "") & """><span>Write Off Listing</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim ds As New DataSet
        Dim strMsg As String = ""

        If objINV.WOCancel(ds, lblWONo.Text) = True Then
            Common.NetMsgbox(Me, "WO Number " & lblWONo.Text & " has been successfully cancelled", dDispatcher.direct("Inventory", "WriteOffSearch.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
        Else
            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub displayAttachFile()
        Dim objFile As New FileManagement
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objINV.getWOAttach(Me.Request.QueryString("WONO"), Session("CompanyId"))

        Dim intCount As Integer
        pnlAttach2.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                If Common.parseNull(drvAttach(i)("CDA_TYPE")) = "I" Then
                    strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                    strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                    '*************************meilai 25/2/05****************************
                    'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=RFQ>" & strFile & "</A>"
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "WO", EnumUploadFrom.FrontOff)
                    '*************************meilai************************************
                    Dim lblBr As New Label
                    Dim lblFile As New Label

                    lblFile.Text = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                    lblBr.Text = "<BR>"
                    pnlAttach2.Controls.Add(lblFile)
                    pnlAttach2.Controls.Add(lblBr)

                    intCount = intCount + 1
                End If
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach2.Controls.Add(lblFile)
        End If

    End Sub
End Class



