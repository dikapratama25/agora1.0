Imports AgoraLegacy
Imports eProcure.Component
Public Class RFQ_DisplayVen
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblListName As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDisplay As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents dtgVenSelection As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList        
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete            
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
        Else
            cmdDelete.Enabled = False
            cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If (Not IsPostBack) Then
            'Session("CompanyId") = "demo" 
            'Session("UserId") = "moofh"
            SetGridProperty(Me.dtgVenSelection)
            Me.lblDisplay.Text = Me.Request.QueryString("VenName")
            lnkBack.NavigateUrl = dDispatcher.direct("PersonalSetting", "RFQ_VenList.aspx", "pageid=" & strPageId)
            hidIndex.Value = Request.QueryString("value")
            Dim str As String = dDispatcher.direct("RFQ", "VendorList.aspx", "pageid=" & strPageId & "&index_list=" & hidIndex.Value & "&edit=2&RFQ_Name=" & Server.UrlEncode(Me.lblDisplay.Text) & "&RFQ_list=Vendor List")
            'Me.cmdAdd.Attributes.Add("onclick", "window.open('../RFQ/VendorList.aspx?index_list=" & hidIndex.Value & "&edit=2&RFQ_Name=" & Me.lblDisplay.Text & "&RFQ_list=Vendor List')")
            Me.cmdAdd.Attributes.Add("onclick", "window.open(""" & str & """)") '../RFQ/VendorList.aspx?index_list=" & hidIndex.Value & "&edit=2&RFQ_Name=" & Me.lblDisplay.Text & "&RFQ_list=Vendor List')")
            Bindgrid(0)
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")


    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim objRFQ As New PersonalSetting

        Dim strValue As String
        Dim strlistname As String
        Dim index As Integer
        Dim ds As DataSet

        strlistname = Me.lblDisplay.Text
        index = Request.QueryString("value")

        ds = objRFQ.DisplayVenDetail(strlistname, index)

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count


        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If viewstate("action") = "del" Then
            If dtgVenSelection.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVenSelection.PageSize = 0 Then
                dtgVenSelection.CurrentPageIndex = dtgVenSelection.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            'If intPageRecordCnt > 0 Then
            dtgVenSelection.DataSource = dvViewSample
            dtgVenSelection.DataBind()
        Else
            '//page_preRender function is called after this function
            '//page_preRender use intPageRecordCnt for checking
            intPageRecordCnt = viewstate("intPageRecordCnt")
            dtgVenSelection.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            '****************meilai 7/1/2005 enable delete n reset button when no record found********************
            cmdDelete.Enabled = False
            cmdReset.Disabled = True
            '*****************************************************************************************************

        End If
        objRFQ = Nothing


    End Function

    Private Sub dtgVenSelection_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVenSelection.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'End If

            '********************meilai 20/1/2005 display contact details******************************

            Dim lbl_adds2 As Label
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            lbl_adds2 = e.Item.FindControl("lbl_adds")
            Dim stradds As String

            'get addr 1
            'If (Not IsDBNull(dv("CM_ADDR_LINE1"))) Or Common.parseNull(dv("CM_ADDR_LINE1")) <> "" Then
            If Common.parseNull(dv("CM_ADDR_LINE1")) <> "" Then
                stradds = "" & dv("CM_ADDR_LINE1") & ""
            End If


            'get addr 2
            'If (Not IsDBNull(dv("CM_ADDR_LINE2"))) Or Common.parseNull(dv("CM_ADDR_LINE2")) <> "" Then
            If Common.parseNull(dv("CM_ADDR_LINE2")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE2") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE2") & ""
                End If
            End If

            'get addr 3
            'If (Not IsDBNull(dv("CM_ADDR_LINE3"))) Or Common.parseNull(dv("CM_ADDR_LINE3")) <> "" Then
            If Common.parseNull(dv("CM_ADDR_LINE3")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("CM_ADDR_LINE3") & ""
                Else
                    stradds = stradds & "<br>" & dv("CM_ADDR_LINE3") & ""
                End If
            End If

            'get city
            'If (Not IsDBNull(dv("cm_city"))) Or Common.parseNull(dv("cm_city")) <> "" Then
            If Common.parseNull(dv("cm_city")) <> "" Then
                If stradds = "" Then
                    stradds = "" & dv("cm_city") & ""
                Else
                    stradds = stradds & "<br>" & dv("cm_city") & ""
                End If
            End If

            'get postcode and state
            'If (Not IsDBNull(dv("cm_postcode"))) Or Common.parseNull(dv("cm_postcode")) <> "" Or (Not IsDBNull(dv("State"))) Or Common.parseNull(dv("State")) = "" Then
            If Common.parseNull(dv("cm_postcode")) <> "" Or Common.parseNull(dv("State")) = "" Then
                If stradds = "" Then
                    stradds = "" & dv("cm_postcode") & " " & dv("State")
                Else
                    stradds = stradds & "<br>" & dv("cm_postcode") & " " & dv("State")
                End If
            End If


            'get country
            If (Not IsDBNull(dv("Country"))) Or Common.parseNull(dv("Country")) = "" Then
                'If Common.parseNull(dv("Country")) = "" Then
                If stradds = "" Then
                    stradds = "" & dv("Country") & ""
                Else
                    stradds = stradds & "<br>" & dv("Country") & ""
                End If
            End If
            '*************************************************************************
            lbl_adds2.Text = stradds
        End If

    End Sub

    Private Sub dtgVenSelection_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVenSelection.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgVenSelection, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtgVenSelection.CurrentPageIndex = 0, True)
    End Sub

    Private Sub dtgVenSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgVenSelection.SelectedIndexChanged

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objPersonal As New PersonalSetting
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim v_com_id As String
        Dim VenId As String
        Dim i As Integer = 0
        For Each dgItem In dtgVenSelection.Items
            chk = dgItem.FindControl("chkSelection")
            v_com_id = dtgVenSelection.DataKeys.Item(i)
            ' VenId = dtgVenSelection.DataKeys.Item(i)
            If chk.Checked Then
                objPersonal.delVenDetail(v_com_id, Request.QueryString("value"))
            End If
            i = i + 1
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid()
        objPersonal = Nothing
    End Sub
    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgVenSelection.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

   

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

    End Sub


End Class
