Imports AgoraLegacy
Imports eProcure.Component


Public Class GRNLotList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objDO As New DeliveryOrder
    Dim objFile As New FileManagement
    Dim Comp_ID, Item_Code, DO_No, poLine As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_lotlist As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_item As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_grn As System.Web.UI.WebControls.Label
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

    Public Enum EnumSumGRNLot
        icLotQty
        icLotNo
        icContinueLot
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
        poLine = Me.Request(Trim("poline"))

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
        Dim objDO As New DeliveryOrder

        Me.lbl_item.Text = "Item Code : " & Me.Request(Trim("itemcode"))
        Me.lbl_grn.Text = "GRN Number : " & Me.Request(Trim("GRNNo"))
        Dim ds As DataSet

        ds = objDO.GetDOLotList(DO_No, Comp_ID, Item_Code, poLine)
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
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objGRN_Ext As New GRN_Ext

            Dim lbl_CLot As Label
            Dim strCLot As String
            lbl_CLot = e.Item.FindControl("lblCLot")
            strCLot = GRNChkLotContinue(Comp_ID, dv("DOL_LOT_NO"), DO_No, Item_Code)
            lbl_CLot.Text = strCLot

            e.Item.Cells(EnumSumGRNLot.icLotManuDt).Text = Format(CDate(dv("DOL_DO_MANU_DT")), "dd/MM/yyyy")
            e.Item.Cells(EnumSumGRNLot.icLotExpDt).Text = Format(CDate(dv("DOL_DO_EXP_DT")), "dd/MM/yyyy")

            Dim dvFile As DataView
            Dim intLoop, intCount As Integer
            Dim strFile, strFile1, strURL, strTemp As String

            strTemp = ""
            dvFile = objDO.getLotAttachment(DO_No, e.Item.Cells(EnumSumGRNLot.icLotIndex).Text, Comp_ID).Tables(0).DefaultView
            If dvFile.Count > 0 Then
                For intLoop = 0 To dvFile.Count - 1
                    strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , Comp_ID)
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

    Private Function GRNChkLotContinue(ByVal strVenId As String, ByVal strLotNo As String, ByVal strDONo As String, ByVal strItemCode As String) As String
        Dim strSql As String = ""
        Dim strTemp As String = ""

        strSql = "SELECT '*' FROM DO_LOT WHERE DOL_COY_ID = '" & strVenId & "' AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' " & _
                "AND DOL_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND DOL_DO_NO <> '" & Common.Parse(strDONo) & "' "

        If objDb.Exist(strSql) Then
            strTemp = "Yes"
        Else
            strTemp = "No"
        End If

        GRNChkLotContinue = strTemp
    End Function
End Class
