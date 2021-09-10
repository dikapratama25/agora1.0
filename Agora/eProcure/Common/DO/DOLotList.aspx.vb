Imports AgoraLegacy
Imports eProcure.Component
Public Class DOLotList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim DO_No As String
    Dim Item_Code As String
    Dim Comp_ID, callfrom, poline As String
    Dim objDO As New DeliveryOrder
    Dim objFile As New FileManagement

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_lotlist As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_itemcode As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumSumDoLot
        icLotQty
        icLotNo
        icLotManuDt
        icLotExpDt
        icMfgName
        icAttachment
        icItemLine
        icLotIndex
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_lotlist)
        DO_No = Me.Request(Trim("DONo"))
        Item_Code = Me.Request(Trim("itemcode"))
        Comp_ID = Me.Request(Trim("SCoyID"))
        callfrom = Me.Request(Trim("callfrom"))
        poline = Me.Request(Trim("poline"))
        Bindgrid()
    End Sub

    Sub dtg_lotlist_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_lotlist.PageIndexChanged
        dtg_lotlist.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtg_lotlist.SortCommand
        Grid_SortCommand(sender, e)
        Bindgrid(dtg_lotlist.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Me.lbl_itemcode.Text = "Item Code : " & Item_Code
        Dim ds As DataSet

        ds = objDO.GetDOLotList(DO_No, Comp_ID, Item_Code, poline)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        If pSorted Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        dtg_lotlist.DataSource = dvViewSample
        dtg_lotlist.DataBind()
    End Function

    Private Sub dtg_lotlist_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_lotlist.ItemCreated
        Grid_ItemCreated(dtg_lotlist, e)
    End Sub

    Private Sub dtg_lotlist_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_lotlist.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumSumDoLot.icLotManuDt).Text = Format(CDate(dv("DOL_DO_MANU_DT")), "dd/MM/yyyy")
            e.Item.Cells(EnumSumDoLot.icLotExpDt).Text = Format(CDate(dv("DOL_DO_EXP_DT")), "dd/MM/yyyy")

            Dim dvFile As DataView
            Dim intLoop, intCount As Integer
            Dim strFile, strFile1, strURL, strTemp, strTempInt, strItem As String

            If callfrom = "buyer" Then
                dvFile = objDO.getLotAttachment(DO_No, e.Item.Cells(EnumSumDoLot.icLotIndex).Text, Comp_ID).Tables(0).DefaultView
            Else
                dvFile = objDO.getLotAttachment(DO_No, e.Item.Cells(EnumSumDoLot.icLotIndex).Text).Tables(0).DefaultView
            End If

            If dvFile.Count > 0 Then
                For intLoop = 0 To dvFile.Count - 1
                    strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")
                    If callfrom = "buyer" Then
                        strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , Comp_ID)
                    Else
                        strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff)
                    End If

                    If strTemp = "" Then
                        strTemp = intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
                    Else
                        strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
                    End If

                Next
            Else
                strTemp = "No Files Attached"
            End If

            Dim lblFileAttach As Label
            lblFileAttach = e.Item.FindControl("lblFileAttach")
            lblFileAttach.Text = strTemp

        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub
End Class
