
Imports AgoraLegacy
Imports eProcure.Component
Imports eProcurement
Imports System.drawing

Public Class IPPUsrGrpCostCenterAdd
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents imbSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents totrows As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label

    Private ds As DataSet
    Dim dvwPackage As DataView
    Dim dDispatcher As New AgoraLegacy.dispatcher


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        'SearchID.Value = ""
        'SearchKey.Value = ""
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdSave.Enabled = False
 
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        htPageAccess.Add("add", alButtonList)


        CheckButtonAccess()
        '//additional checking
        'If intPageRecordCnt > 0 Then
        '    cmdDelete.Enabled = blnCanDelete         
        'Else
        '    cmdDelete.Enabled = False
        'End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgUser)
        ViewState("UserGroup") = Request.QueryString("grpno")

        If Not Page.IsPostBack Then

            PopulateTypeAhead()
            dgUser.CurrentPageIndex = 0
            Bindgrid()
        End If

        'ViewState("pageid") = Request.Params("pageid")
        'cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUserGroup As String
        Dim strUserName As String

        ' strUserGroup = Me.txtUserGroup.Text
        'strUserName = Me.txtUserName.Text

        Dim objIPP As New IPP
        Dim objUserRoles As New UserRoles
        Dim ds As DataSet = New DataSet
        Dim strUserRoles As String

        ds = objIPP.getIPPUserGroupCCList(Common.Parse(HttpContext.Current.Session("CompanyID")), Common.Parse(txtCCCode.Text), Common.Parse(txtCCDesc.Text), ViewState("UserGroup"))

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        'If dgUser.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgUser.PageSize = 0 Then
        '    dgUser.CurrentPageIndex = dgUser.CurrentPageIndex - 1
        '    ViewState("action") = ""
        'End If
        ' End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dgUser, dvViewSample)
            dgUser.DataSource = dvViewSample
            dgUser.DataBind()
        Else

            'cmdDelete.Enabled = False          
            Common.NetMsgbox(Me, MsgNoRecord)
            dgUser.DataBind()
        End If

        ViewState("PageCount") = dgUser.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgUser.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dgUser_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgUser, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

        End If
    End Sub

    Private Sub dgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Dim lnkUserID As HyperLink
            'lnkUserID = e.Item.FindControl("lnkUserID")
            'lnkUserID.NavigateUrl = dDispatcher.direct("Users", "usUser.aspx", "pageid=" & ViewState("pageid") & "&mode=modify&userid=" & dv("UM_USER_ID"))
            'lnkUserID.Text = dv("UM_USER_ID")

            'to add a repeater to display user group
            'Dim rpt As Repeater
            'rpt = e.Item.FindControl("sub")
            'Dim objUser As New Users
            'rpt.DataSource = objUser.getUserGrpByEmployee(CType(e.Item.Cells(1).Controls(1), HyperLink).Text, dvwPackage)
            ''AddHandler rpt.ItemDataBound, AddressOf rpt_ItemDataBound
            'rpt.DataBind()

            'e.Item.Cells(3).VerticalAlign = VerticalAlign.Middle
            'If e.Item.Cells(6).Text = "S" Then
            '    e.Item.Cells(6).Text = "Y"
            '    e.Item.Cells(6).ForeColor = Color.Red
            'End If
        End If
    End Sub

    
    Sub dgUser_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged
        dgUser.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objIPP As New IPP
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim Query(0) As String
        Dim objDb As New EAD.DBCom
        Dim bEmpty As Boolean = True
        Dim strSQL, strCCCode As String

        Try
            ' Dim strDept As String = Dg2String(dgUser, 2)
            

            For Each dgItem In dgUser.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strCCCode = dgItem.Cells(1).Text
                    strSQL = "INSERT INTO ipp_usrgrp_CC (IUC_GRP_INDEX,IUC_CC_CODE) " & _
                        "VALUES('" & ViewState("UserGroup") & "','" & Common.Parse(strCCCode) & "') "
                    Common.Insert2Ary(Query, strSQL)
                    bEmpty = False
                End If
            Next
            'End If

            If bEmpty = False Then
                If objDb.BatchExecute(Query) Then
                    Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, "Record not saved.", MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
            End If
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
        

    End Sub

    Public Function FillCheckBoxGrid(ByVal pInString As String, _
                                     ByRef pDataGrid As DataGrid) As Boolean

        Dim lngLoop As Long
        Dim ary() As String = Split(pInString, ",")
        Dim varItem As DataGridItem

        For lngLoop = 0 To UBound(ary)
            For Each varItem In pDataGrid.Items
                If pDataGrid.DataKeys(varItem.ItemIndex).ToString = ary(lngLoop) Then
                    Dim chk As CheckBox = varItem.Cells(0).FindControl("select")
                    chk.Checked = True
                End If
            Next
        Next
    End Function

    'Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
    '    Dim dgItem As DataGridItem
    '    Dim strUserId As String
    '    Dim chkItem As CheckBox
    '    Dim objUser As New Users

    '    For Each dgItem In dgUser.Items
    '        chkItem = dgItem.FindControl("chkSelection")
    '        If chkItem.Checked Then
    '            strUserId = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
    '            If objUser.DelUser(LCase(strUserId), False) = False Then
    '                Exit For
    '            End If
    '        End If
    '    Next
    '    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
    '    ViewState("action") = "del"
    '    Bindgrid()
    '    objUser = Nothing
    'End Sub

    Private Sub dgUser_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
        Grid_SortCommand(source, e)
        dgUser.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    
    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String        
        Dim typeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre2&frm=IPPUsrGrpCCAdd&idx=" & ViewState("UserGroup") & "&btn=CCCode")
        Dim typeahead2 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre2&frm=IPPUsrGrpCCAdd&idx=" & ViewState("UserGroup") & "&btn=CCDesc")


        content &= "$(""#txtCCCode"").autocomplete(""" & typeahead1 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
         "});" & vbCrLf & _
        "$(""#txtCCDesc"").autocomplete(""" & typeahead2 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf
       
        typeahead = "<script language=""javascript"">" & vbCrLf & _
      "<!--" & vbCrLf & _
        "$(document).ready(function(){" & vbCrLf & _
        content & vbCrLf & _
        "});" & vbCrLf & _
        "-->" & vbCrLf & _
        "</script>"


        Session("typeaheadIPPCCAdd") = typeahead
    End Sub

End Class


