Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing

Public Class RFQComSummary_SEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom
    Dim Product_Ext As New Products_Ext
    Dim blnBoth As Boolean = False
    Dim blnSP As Boolean = False
    Dim blnST As Boolean = False
    Dim strDestPath As String = System.Configuration.ConfigurationManager.AppSettings("TemplateTemp")

    Protected WithEvents img_1 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_2 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_3 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_4 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_5 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_6 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_7 As New System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_8 As New System.Web.UI.WebControls.ImageButton

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_Num As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Name As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_compare As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmd_compare As System.Web.UI.WebControls.Button
    Protected WithEvents chk_compare As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_Qoute As System.Web.UI.WebControls.DataGrid
    Dim objrfq As New RFQ
    Dim objrfq_Ext As New RFQ_Ext
    Dim objval As New RFQ_User
    Dim count_max As Integer
    Dim max As Double
    Dim count_min As Integer
    Dim min As Double
    Dim i As Integer
    Dim rank As Integer
    Dim strFrm As String
    Dim strdPage As String
    Dim intRemarkCol As Integer = 20
    Dim intCnt As Integer
    Protected WithEvents dtg_itemdis As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_rank As System.Web.UI.WebControls.DataGrid
    Protected WithEvents hid1 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmd_raise_pr As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Dim Bstatus As String
    Protected WithEvents cmd_Previous As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Dim raise As Boolean
    Protected WithEvents lbl_rankerror As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum QuoEnum
        'Indicator = 1
        'QuoNo = 2
        'QuoValidity = 3
        'Vendor = 4
        'Total = 5
        QuoNo = 0
        QuoValidity = 1
        Vendor = 2
        Total = 3
    End Enum

    Public Enum itemEnum
        Chk = 0
        Desc = 1
        UOM = 2
        Quantity = 3
        Line = 4
    End Enum

    Public Enum rankEnum
        Chk = 0
        Rank = 1
        VenName = 2
        ActQuoNo_1 = 3
        Price = 4
        Tvalue = 5
        CompID = 6
        CoyName = 7
        Currency = 8
        RFQID = 9
        ActQuoNo_2 = 10
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_raise_pr.Enabled = False
        cmd_raise_pr.Visible = False
        cmd_export.Enabled = False
        cmd_export.Visible = False
        Label3.Visible = False
        Dim alButtonList As New ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_raise_pr)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_raise_pr)
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
        If intPageRecordCnt > 0 Then
            'cmd_raise_pr.Enabled = blnCanAdd And blnCanUpdate
            If Request.QueryString("Frm") = "RFQ_List" Or Request.QueryString("Frm") = "POViewTrx" Or Request.QueryString("Frm") = "POViewB2" Or Request.QueryString("Frm") = "POViewB2Cancel" Then
                If Request.QueryString("RFQType") = "S" Then    'Sent
                    Label3.Visible = False
                    cmd_raise_pr.Visible = False
                    cmd_export.Visible = False

                ElseIf Request.QueryString("RFQType") = "V" Then    'View Response
                    Label3.Visible = True
                    cmd_raise_pr.Enabled = blnCanAdd And blnCanUpdate
                    cmd_raise_pr.Visible = True
                    cmd_export.Enabled = True
                    cmd_export.Visible = True
                    'If ViewState("blnCanAdd_checking") = True Then
                    '    cmd_raise_pr.Visible = True
                    'Else
                    '    cmd_raise_pr.Visible = False
                    'End If
                End If
            ElseIf Request.QueryString("Frm") = "SearchPO_AO" Or Request.QueryString("Frm") = "SearchPO_All" Or Request.QueryString("Frm") = "ViewPR_All" Or Request.QueryString("Frm") = "SearchApp_All" Or Request.QueryString("Frm") = "SearchPR_AO" Then
                Label3.Visible = False
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
            ElseIf Request.QueryString("Frm") = "Dashboard" Then
                Label3.Visible = False
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
            Else
                Label3.Visible = True
                cmd_raise_pr.Enabled = blnCanAdd And blnCanUpdate
                cmd_export.Enabled = True
                cmd_raise_pr.Visible = True
                cmd_export.Visible = True
            End If

        End If

        If Request.QueryString("RFQType") = "V" Then    'View Response
            Label3.Visible = True
            cmd_raise_pr.Enabled = blnCanAdd And blnCanUpdate
            cmd_raise_pr.Visible = True
            cmd_export.Visible = True
            cmd_export.Enabled = True

            
            'If ViewState("blnCanAdd_checking") = True Then
            '    cmd_raise_pr.Visible = True
            'Else
            '    cmd_raise_pr.Visible = False
            'End If
        End If

        alButtonList.Clear()
        Session("w_RFQ_Item") = ConstructTableItemDesc()

        If ViewState("POBtn") = "N/A" Then
            cmd_raise_pr.Visible = False
            cmd_export.Visible = False
        Else
            If Request.QueryString("Frm") = "SearchPO_All" Then
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
            Else
                If blnBoth = False Then
                    If blnSP = True Then
                        Label3.Text = "Step 3: Select vendor and click Raise PO button to continue."
                        cmd_raise_pr.Visible = True
                        cmd_export.Visible = False
                    Else
                        Label3.Text = "Step 3: Select vendor and click Export Item button to continue."
                        cmd_raise_pr.Visible = False
                        cmd_export.Visible = True
                    End If
                Else
                    Label3.Text = "Step 3: Select vendor and click Raise PO / Export Item button to continue."
                    cmd_raise_pr.Visible = True
                    cmd_export.Visible = True
                End If

                
            End If
        End If


        


        If (Me.Request(Trim("disabled")) Is Nothing And Session("disable") Is Nothing) Or Me.Request(Trim("Appr")) = "Y" Then
            cmd_raise_pr.Visible = False
            cmd_export.Visible = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        img_1.ImageUrl = dDispatcher.direct("Plugins/images", "good_icon.gif")
        img_2.ImageUrl = dDispatcher.direct("Plugins/images", "average_icon.gif")
        img_3.ImageUrl = dDispatcher.direct("Plugins/images", "bad_icon.gif")
        img_4.ImageUrl = dDispatcher.direct("Plugins/images", "bestmoney_icon.gif")
        img_5.ImageUrl = dDispatcher.direct("Plugins/images", "badmoney_icon.gif")
        img_6.ImageUrl = dDispatcher.direct("Plugins/images", "date_icon.gif")
        img_7.ImageUrl = dDispatcher.direct("Plugins/images", "Unable To Supply Icon.gif")
        img_8.ImageUrl = dDispatcher.direct("Plugins/images", "Delivery_Term_Icon.gif")

        MyBase.Page_Load(sender, e)
        blnPaging = False
        Me.blnSorting = False
        Session("ItemIndexList") = ""
        SetGridProperty(Me.dtg_rank)
        Session("quoteurl") = strCallFrom
        strFrm = Me.Request.QueryString("Frm")
        strdPage = Session("dPage")
        Dim objRfq_Ext As New RFQ_Ext

        If Not Page.IsPostBack Then
            'Session("disable") = Nothing
            ViewState("POBtn") = "N/A"
            ViewState("Curr") = True
            Session("w_RFQ_tabs") = ""
            If strFrm = "RFQ_Outstg_List" Or strFrm = "RFQ_List" Then
                GenerateTab()
                If strFrm = "RFQ_Outstg_List" Then
                    Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"

                ElseIf strFrm = "RFQ_List" Then
                    If Request.QueryString("Appr") = "Y" Then
                        Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Appr=Y&Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & " &side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"
                        Session("disable") = Nothing
                    Else
                        Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & " &side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"
                    End If
                End If
            ElseIf strFrm = "Dashboard" Then
                If Request.QueryString("Appr") = "Y" Then
                    Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Appr=Y&Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"
                    Session("disable") = Nothing
                Else
                    Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"
                End If
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
            Else
                Me.lbl_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & " &side=rfqsum&page=1&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID"))) & " "" ><font color=#0000ff>" & Me.Request(Trim("RFQ_Num")) & "</font></A>"
                Session("disable") = Nothing
            End If
            'Session("strurl") = strCallFrom

            If Request(Trim("side")) = "other" Then
                Label3.Visible = False
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
                Me.dtg_rank.Columns(rankEnum.Chk).Visible = False
            End If

            

            'cmd_raise_pr.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','Save');")
            'cmd_raise_pr.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection2');")
            Me.cmd_compare.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
            ViewState("ddl_compare") = "0" 'ddl_compare.SelectedValue
            Session("ItemIndexList1") = ""
            ViewState("rfq_num") = Me.Request(Trim("RFQ_Num"))

            ViewState("rfq_id") = Me.Request(Trim("RFQ_ID"))
            Me.lbl_Name.Text = Me.Request(Trim("RFQ_Name"))

            objRfq_Ext.get_itemtype(ViewState("rfq_id"), blnSP, blnST)
            If blnSP = True And blnST = True Then
                blnBoth = True
            End If

            SetGridProperty(Me.dtg_Qoute)
            ViewState("Bstatus") = objrfq.get_rfqstatus(ViewState("rfq_id"))
            Bindgrid(Me.dtg_Qoute, 0)

            If Me.hid1.Value = 0 Then
                SetGridProperty(Me.dtg_itemdis)
                AddVendor()
                Bindgrid(Me.dtg_itemdis, 1)
                FillPrice()
            End If
            'If Me.ddl_compare.SelectedItem.Value <> 0 Then
            '    SetGridProperty(Me.dtg_itemdis)
            '    AddVendor()
            '    Bindgrid(Me.dtg_itemdis, 1)
            '    FillPrice()
            'End If

            Bindgrid(Me.dtg_rank, 2)
            Session("w_RFQ_Rank") = ConstructTableRank()
            Session("urlrefererForRFQ") = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Me.Request(Trim("Frm")) & "&pageid=" & Me.Request(Trim("pageid")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Me.Request(Trim("RFQ_Name")))

            If ViewState("Curr") = False Then
                Common.NetMsgbox(Me, "Exchange rate not updated in system.", MsgBoxStyle.Information)
            End If

            'Dim strRFQ_Index As String
            ''Dim strRFQ_Index As String = objDB.GetVal("SELECT IFNULL(POM_RFQ_INDEX, '') AS POM_RFQ_INDEX FROM PO_MSTR WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ('" & ViewState("rfq_id") & "')")
            'strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
            '" WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
            '" RM_RFQ_ID = '" & ViewState("rfq_id") & "' AND " & _
            '" (RD_PRODUCT_CODE IN ( " & _
            '" SELECT POD_PRODUCT_CODE FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
            '" POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
            '" SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
            '" RM_RFQ_ID = '" & ViewState("rfq_id") & "') " & _
            '" ) OR " & _
            '" RD_PRODUCT_DESC IN ( " & _
            '" SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
            '" POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
            '" SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
            '" RM_RFQ_ID = '" & ViewState("rfq_id") & "') " & _
            '" ) ) LIMIT 1 ")

            'If strRFQ_Index <> "" Then
            '    ViewState("blnCanAdd_checking") = False
            'Else
            '    ViewState("blnCanAdd_checking") = True
            'End If

            'RFQComSummary.aspx?Frm=RFQ_List&pageid=93&RFQ_Num=RFQ0000394&RFQ_ID=703&RFQ_Name=26262626
        Else
            'If ViewState("complete") > "1" Then
            '    addmax()
            'End If
            'If Me.hid1.Value = 0 Then
            '    SetGridProperty(Me.dtg_itemdis)
            '    AddVendor()
            '    Bindgrid(Me.dtg_itemdis, 1)
            '    FillPrice()
            'End If
            'If Me.ddl_compare.SelectedItem.Value <> 0 Then
            objRfq_Ext.get_itemtype(ViewState("rfq_id"), blnSP, blnST)
            If blnSP = True And blnST = True Then
                blnBoth = True
            End If

            SetGridProperty(Me.dtg_itemdis)
            AddVendor()
            Bindgrid(Me.dtg_itemdis, 1)
            FillPrice()

            'End If
           
        End If

        If ViewState("complete") > "1" Then
            addmax()
        End If
        Session("urlreferer") = "RFQComSummary"
        'AddVendor()        
    End Sub

    Private Function Bindgrid(ByVal dg_id As DataGrid, ByVal chk As Integer, Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim RFQ_ID As String = Me.Request(Trim("RFQ_ID"))
        Dim ds As New DataSet
        Dim DocNum As String
        Dim rfqnum As String = Me.Request(Trim("RFQ_Num"))
        Dim array(Me.dtg_itemdis.Items.Count) As String
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        Dim ItemIndexList As String
        Me.dtg_rank.Columns(4).Visible = False

        Dim dgitem As DataGridItem
        If chk = "0" Then
            ds = objrfq_Ext.get_quoteVen(RFQ_ID)
        ElseIf chk = "1" Then
            ds = objrfq.get_itemdis(RFQ_ID)
        ElseIf chk = "2" Then
            Dim item_desc As String
            ViewState("dtg_rank") = "0"
            If Not Page.IsPostBack Then 'min max
                For Each dgitem In Me.dtg_itemdis.Items
                    ds = objrfq.get_itemrank(RFQ_ID, "0", array)


                    If Session("ItemIndexList1") = "" Then
                        ItemIndexList = CStr(dtg_itemdis.DataKeys.Item(i)).ToString.Trim
                        Session("ItemIndexList1") = ItemIndexList
                    Else
                        Session("ItemIndexList1") = Session("ItemIndexList1") & "," & CStr(dtg_itemdis.DataKeys.Item(i)).ToString.Trim & ""
                    End If

                    i = i + 1
                Next
                'Me.Label1.Text = "All/Grouped Items"
                'Me.Label2.Text = "Generate PR (based on comparison selection)  "
            Else ' select item

                Dim chkbox As CheckBox

                If ViewState("ddl_compare") = "0" Then 'If ddl_compare.SelectedItem.Value = "0" Then

                    Dim j As Integer
                    Dim a As Integer

                    For Each dgitem In Me.dtg_itemdis.Items
                        chkbox = dgitem.FindControl("chkSelection")
                        If chkbox.Checked Then
                            array(i) = dtg_itemdis.DataKeys.Item(j)

                            If Session("ItemIndexList1") = "" Then
                                ItemIndexList = CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim
                                Session("ItemIndexList1") = ItemIndexList
                            Else
                                Session("ItemIndexList1") = Session("ItemIndexList1") & "," & CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim & ""

                            End If
                            item_desc = dgitem.Cells(itemEnum.Desc).Text

                            i = i + 1
                            a = a + 1
                        End If

                        j = j + 1
                    Next
                    If a = 1 Then
                        Me.Label1.Text = item_desc

                    End If
                    If i = 1 Then

                        ViewState("dtg_rank") = "1"
                        'Me.dtg_rank.Columns(4).Visible = True
                    Else
                        'Me.Label1.Text = "All/Grouped Items"
                        ViewState("dtg_rank") = "0"
                        Me.dtg_rank.Columns(4).Visible = False

                    End If

                    'If Me.chk_compare.Checked Then
                    '    If i = 1 Then
                    '        ds = objrfq.get_itemrank(RFQ_ID, "2", array, "ASC", 1, i)   'compare submittion only
                    '    Else
                    '        ds = objrfq.get_itemrank(RFQ_ID, "1", array, "ASC", 1, i)   'compare submittion only
                    '    End If
                    'Else
                    ds = objrfq.get_itemrank(RFQ_ID, "1", array, "ASC", 0, i)
                    'End If

                ElseIf ViewState("ddl_compare") = "1" Then 'ddl_compare.SelectedItem.Value = "1" Then
                    ds = objrfq.get_itemrank(RFQ_ID, "0", array, "ASC", 0, i)
                ElseIf ViewState("ddl_compare") = "2" Then 'ddl_compare.SelectedItem.Value = "2" Then
                    ds = objrfq.get_itemrank(RFQ_ID, "0", array, "DESC", 0, i)
                End If
            End If
        End If

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count

        If ViewState("action") = "del" Then
            If dg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_id.PageSize = 0 Then
                dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If
        dg_id.DataSource = dvViewSample

        If dg_id.ID.ToString = "dtg_rank" Then
            If dvViewSample.Count = 0 Then
                lbl_rankerror.Visible = True
                Me.lbl_rankerror.Text = "No record matches the comparison conditions!"
            Else
                lbl_rankerror.Visible = False
            End If
        End If
        dg_id.DataBind()
    End Function

    Private Sub AddVendor()
        Dim RFQ_ID As String = Me.Request(Trim("RFQ_ID"))
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim DocNum As String
        Dim rfqnum As String = Me.Request(Trim("RFQ_Num"))
        Dim array(Me.dtg_itemdis.Items.Count) As String
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        Dim ItemIndexList As String
        Dim inttmp As Integer = 0
        Dim intCount As Integer
        Dim col As TemplateColumn
        Dim aryVendorId As New ArrayList, aryVendorName As New ArrayList

        'Michelle (24/2/2011) - To cater for incomplete quote, depending on user selection
        'ds = objrfq.get_itemrank(RFQ_ID, "0", array)

        'If Me.chk_compare.Checked Then
        '    ds = objrfq.get_itemrank(RFQ_ID, "0", array)
        'Else
        ds = objrfq.get_itemrank(RFQ_ID, "2", array, , , , True)
        'End If

        dt = ds.Tables(0)
        intCount = dt.Rows.Count
        ViewState("VendorCount") = intCount

        For inttmp = 0 To intCount - 1
            aryVendorId.Add(dt.Rows(inttmp)("RRM_V_Company_ID"))
            aryVendorName.Add(dt.Rows(inttmp)("CM_COY_NAME"))
            col = New TemplateColumn
            col.HeaderText = dt.Rows(inttmp)("RRM_V_Company_ID")
            col.HeaderStyle.Wrap = False
            col.ItemStyle.Width = 8
            col.ItemStyle.Wrap = False
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Right
            col.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            col.Visible = False
            Me.dtg_itemdis.Columns.Add(col)

            col = New TemplateColumn
            col.HeaderText = Mid(dt.Rows(inttmp)("CM_COY_NAME"), 1, 10) & "..." '& dt.Rows(inttmp)("RRM_V_Company_ID")
            col.HeaderStyle.Wrap = False
            col.ItemStyle.Width = 8
            col.ItemStyle.Wrap = False
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Right
            col.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            Me.dtg_itemdis.Columns.Add(col)
        Next
        'Dim col1 As TemplateColumn = New TemplateColumn
        ''col1.ItemTemplate = New dgTemplate(dt.Rows(inttmp)("RRM_V_Company_ID"), 5)
        'col1.HeaderText = "abcdefghij" & "..." '& "abcdefghij"
        'col1.ItemStyle.Width = 8
        ''Me.dtg_itemdis.Columns.AddAt(Me.dtg_itemdis.Columns.Count - 1, col)
        'Me.dtg_itemdis.Columns.Add(col1)

        'Dim col2 As TemplateColumn = New TemplateColumn
        ''col2.ItemTemplate = New dgTemplate(dt.Rows(inttmp)("RRM_V_Company_ID"), 5)
        'col2.HeaderText = "abcdefghij" & "..." '& "abcdefghij"
        'col2.ItemStyle.Width = 8
        ''Me.dtg_itemdis.Columns.AddAt(Me.dtg_itemdis.Columns.Count - 1, col)
        'Me.dtg_itemdis.Columns.Add(col2)

        'Dim col3 As TemplateColumn = New TemplateColumn
        ''col3.ItemTemplate = New dgTemplate(dt.Rows(inttmp)("RRM_V_Company_ID"), 5)
        'col3.HeaderText = "abcdefghij" & "..." '& "abcdefghij"
        'col3.ItemStyle.Width = 8
        ''Me.dtg_itemdis.Columns.AddAt(Me.dtg_itemdis.Columns.Count - 1, col)
        'Me.dtg_itemdis.Columns.Add(col3)
        ' ''Dim a
        ' ''a = Me.dtg_itemdis.Columns(3).HeaderText
        ViewState("VendorListId") = aryVendorId
        ViewState("VendorListName") = aryVendorName
    End Sub

    Sub addmax()
        Dim dgItem As DataGridItem
        Dim img_4 As ImageButton
        Dim img_5 As ImageButton

        Dim i As Integer = 0
        For Each dgItem In Me.dtg_Qoute.Items
            If i = ViewState("count_max1") - 1 Then
                img_5 = dgItem.FindControl("img_5")
                img_5.Visible = True
            End If

            If i = ViewState("count_min1") - 1 Then
                img_4 = dgItem.FindControl("img_4")
                img_4.Visible = True
            End If
            i = i + 1
        Next
    End Sub

    Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_Qoute.CurrentPageIndex = e.NewPageIndex
        Bindgrid(dtg_Qoute, 0, 0)
    End Sub

    Private Sub dtg_Qoute_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Qoute.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim i As Integer
            Dim lbl_QuoValidity As Label
            lbl_QuoValidity = e.Item.FindControl("lbl_QuoValidity")
            Dim lbl_qouteNum As Label
            lbl_qouteNum = e.Item.FindControl("lbl_qouteNum")
            img_1 = e.Item.FindControl("img_1")
            img_2 = e.Item.FindControl("img_2")
            img_3 = e.Item.FindControl("img_3")
            img_4 = e.Item.FindControl("img_4")
            img_5 = e.Item.FindControl("img_5")
            img_6 = e.Item.FindControl("img_6")
            img_7 = e.Item.FindControl("img_7")
            img_8 = e.Item.FindControl("img_8")
            count_max = ViewState("count_max")
            count_max = count_max + 1
            ViewState("count_max") = count_max
            count_min = ViewState("count_min")
            count_min = count_min + 1
            ViewState("count_min") = count_min
            If Common.parseNull(dv("RRM_Indicator")) = "0" Then
                img_1.Visible = True
                ViewState("complete") = ViewState("complete") + 1

                If ViewState("check") = "" Then
                    ViewState("Min") = Convert.ToDouble(dv("RRM_TotalValue2"))
                End If

                max = ViewState("Max")
                min = ViewState("Min")

                Dim item As Double = Convert.ToDouble(dv("RRM_TotalValue2"))

                If item > max Then
                    ViewState("Max") = item
                    ViewState("count_max1") = count_max
                End If
                If item <= min Then
                    ViewState("Min") = item
                    ViewState("count_min1") = count_min
                End If

                ViewState("check") = "1"
                If Not IsDBNull(dv("RRM_Offer_Till")) Then
                    If dv("RRM_Offer_Till") < dv("RM_Reqd_Quote_Validity") Then
                        img_6.Visible = True
                    End If
                End If

            ElseIf Common.parseNull(dv("RRM_Indicator")) = "1" Then
                img_2.Visible = True
                ViewState("check1") = "1"
            End If

            If IsDBNull(Common.parseNull(dv("RRM_Actual_Quot_Num"))) Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = "" Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = " " Then
                lbl_qouteNum.Text = "N/A"
                If Common.parseNull(dv("RVM_V_RFQ_STATUS")) = "1" Then
                    img_7.Visible = True
                Else
                    img_3.Visible = True
                End If

            Else
                If Request.QueryString("Appr") = "Y" Then
                    lbl_qouteNum.Text = "<A href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Appr=Y&Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=quote&Subside=" & Request.QueryString("side") & "&qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&vcomid=" & Common.parseNull(dv("RRM_V_Company_ID")) & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"
                Else
                    lbl_qouteNum.Text = "<A href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=quote&Subside=" & Request.QueryString("side") & "&qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&vcomid=" & Common.parseNull(dv("RRM_V_Company_ID")) & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"
                End If

                ViewState("POBtn") = ""
            End If

            'Check for delivery term 
            If objrfq_Ext.chk_DelTerm(Common.parseNull(dv("RRM_RFQ_ID")), Common.parseNull(dv("RRM_V_Company_ID"))) Then
                img_8.Visible = False
            Else
                img_8.Visible = True
            End If

            If IsDBNull(dv("RRM_Offer_Till")) Then
                lbl_QuoValidity.Text = "N/A"
            Else
                lbl_QuoValidity.Text = dv("RRM_Offer_Till")
            End If

            Dim lbl_total As Label
            lbl_total = e.Item.FindControl("lbl_total")

            If IsDBNull(dv("RRM_TotalValue2")) Then
                lbl_total.Text = "N/A"
            Else
                lbl_total.Text = Common.parseNull(dv("RRM_Currency_Code")) & " " & Format(dv("RRM_TotalValue"), "#,##0.00") & " | MYR " & Format(dv("RRM_TotalValue2"), "#,##0.00")

            End If
            Dim lbl_vendor As Label
            If Request.QueryString("Appr") = "Y" Then
                e.Item.Cells(QuoEnum.Vendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Appr=Y&frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & dv("RVM_V_Company_ID") & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & " "" ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
            Else
                e.Item.Cells(QuoEnum.Vendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & dv("RVM_V_Company_ID") & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & " "" ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
            End If
            ' if no quote then pass RM_RFQ_ID
            If IsDBNull(dv("RRM_Indicator")) Then
                If Request.QueryString("Appr") = "Y" Then
                    e.Item.Cells(QuoEnum.Vendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Appr=Y&frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & dv("RVM_V_Company_ID") & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & " "" ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
                Else
                    e.Item.Cells(QuoEnum.Vendor).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & dv("RVM_V_Company_ID") & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & " "" ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
                End If

            End If
            img_1.ImageUrl = dDispatcher.direct("Plugins/images", "good_icon.gif")
            img_2.ImageUrl = dDispatcher.direct("Plugins/images", "average_icon.gif")
            img_3.ImageUrl = dDispatcher.direct("Plugins/images", "bad_icon.gif")
            img_4.ImageUrl = dDispatcher.direct("Plugins/images", "bestmoney_icon.gif")
            img_5.ImageUrl = dDispatcher.direct("Plugins/images", "badmoney_icon.gif")
            img_6.ImageUrl = dDispatcher.direct("Plugins/images", "date_icon.gif")
            img_7.ImageUrl = dDispatcher.direct("Plugins/images", "Unable To Supply Icon.gif")
            img_8.ImageUrl = dDispatcher.direct("Plugins/images", "Delivery_Term_Icon.gif")

            If ViewState("POBtn") = "N/A" Then
                cmd_raise_pr.Visible = False
                cmd_export.Visible = False
            Else
                cmd_raise_pr.Visible = True
                cmd_export.Visible = True
            End If
        End If
    End Sub

    Private Sub dtg_itemdis_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_itemdis.ItemCreated
        '  Grid_ItemCreated(dtg_itemdis, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Checked = True
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtg_itemdis_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_itemdis.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim chk As CheckBox
            chk = e.Item.Cells(itemEnum.Chk).FindControl("chkSelection")
            chk.Checked = True
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If

        If ViewState("ddl_compare") <> "0" Then 'ddl_compare.SelectedItem.Value <> "0" Then
            Me.hid1.Value = 0
        End If

    End Sub

    Private Sub dtg_rank_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_rank.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lbl_rank As Label
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection2")

            lbl_rank = e.Item.FindControl("lbl_rank")
            Dim lbl_unitprice As Label
            lbl_unitprice = e.Item.FindControl("lbl_unitprice")

            If ViewState("dtg_rank") = "1" Then
                lbl_unitprice.Text = Common.parseNull(dv("RRM_Currency_Code")) & " " & Format(Common.parseNull(dv("RRM_TotalValue")), "###,###,##0.0000")
            End If
            Dim expdate As DateTime
            expdate = objrfq.get_expiredate(Common.parseNull(dv("RRM_RFQ_ID")))

            If dv("RRM_Offer_Till") >= Today.Date And ViewState("Bstatus") = "0" Then
                chk.Visible = True
                chk.Enabled = True
                Me.cmd_raise_pr.Enabled = True
                Me.cmd_export.Enabled = True
                Label3.Visible = True
                cmd_raise_pr.Visible = True
                cmd_export.Visible = True
            End If

            If Not IsDBNull(dv("RRM_TotalValue")) Then 'if total value = 0 then can't create PR
                If dv("RRM_TotalValue") <> "0" Then
                    rank = rank + 1

                    lbl_rank.Text = rank
                    If dv("RRM_Offer_Till") >= Today.Date And Bstatus = "0" Then
                        Me.cmd_raise_pr.Enabled = True
                        Me.cmd_export.Enabled = True
                        cmd_raise_pr.Visible = True
                        cmd_export.Visible = True
                        Label3.Visible = True
                        chk.Visible = True
                        chk.Enabled = True
                    End If
                Else
                    chk.Visible = False
                    chk.Enabled = False
                    lbl_rank.Text = "-"
                    e.Item.Cells(rankEnum.Tvalue).Text = "N/A"
                End If
            End If



            Dim lbl_supplier As Label
            lbl_supplier = e.Item.FindControl("lbl_supplier")
            If Request.QueryString("Appr") = "Y" Then
                lbl_supplier.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Appr=Y&frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&v_com_id=" & dv("RRM_V_Company_ID") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & """ ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
            Else
                lbl_supplier.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=RFQComSummary&SubFrm=" & strFrm & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&v_com_id=" & dv("RRM_V_Company_ID") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name"))) & """ ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A>"
            End If

            e.Item.Cells(rankEnum.Tvalue).Text = Common.parseNull(dv("RRM_Currency_Code")) & " " & Format(Common.parseNull(dv("RRM_TotalValue")), "###,###,##0.00")
        End If
    End Sub

    Private Sub cmd_compare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_compare.Click
        Session("itemindexlist1") = ""
        '//to keep track whether compare button clicked. used in "Raise PR" button
        '//initiate value set at page load
        ViewState("ddl_compare") = "0" 'ddl_compare.SelectedValue
        'AddVendor()
        Bindgrid(Me.dtg_rank, 2)
        Session("w_RFQ_Rank") = ConstructTableRank()
        Session("w_RFQ_Item") = ConstructTableItemDesc()
        'FillPrice()

    End Sub

    Private Sub cmd_Previous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Previous.ServerClick
        If strFrm = "RFQ_Outstg_List" Then
            Response.Redirect(dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId & ""))
        Else
            If Request(Trim("side")) = "other" Or Request(Trim("side")) = "rfqsum" Or Request(Trim("side")) = "vendorsum" Then
                If strFrm = "POViewB2" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "POViewTrx" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewTrx.aspx", "filetype=2&side=u&pageid=7&type=MyPO"))
                ElseIf strFrm = "POViewB2Cancel" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewB2Cancel.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "Dashboard" Then
                    Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "SearchPR_AO" Then
                    Response.Redirect(dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId & ""))
                Else
                    Response.Redirect(Session("strurl"))
                End If
            Else
                If strFrm = "Dashboard" Then
                    Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "POViewB2" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "POViewTrx" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewTrx.aspx", "filetype=2&side=u&pageid=7&type=MyPO"))
                ElseIf strFrm = "POViewB2Cancel" Then
                    Response.Redirect(dDispatcher.direct("PO", "POViewB2Cancel.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "SearchPO_AO" Then
                    Response.Redirect(dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId & ""))
                ElseIf strFrm = "SearchPO_All" Then
                    Response.Redirect(dDispatcher.direct("PO", "SearchPO_All.aspx", "pageid=" & strPageId & ""))
                Else
                    Response.Redirect(dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId & ""))
                End If
            End If
        End If

    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        Dim intBCM As String
        intBCM = CInt(objPR.checkBCM)
        If intBCM > 0 Then
            If Not objPR.checkUserAccExist() Then
                strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
            End If
        End If

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
    End Function

    Private Sub cmd_raise_pr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_raise_pr.Click
        Dim strMsg As String
        Dim intMsg As Integer
        Dim objRFq_ext As New RFQ_Ext
        Dim arr As New ArrayList
        Dim i As Integer
        If checkMandatory(strMsg) Then
            Dim dgitem, dgitem2 As DataGridItem
            Dim chk As CheckBox
            ViewState("PR") = "1"
            Dim arrRFQ As New ArrayList
            Dim chkbox As CheckBox
            Dim j As Integer
            Dim item_desc As String
            Dim ItemIndexList As String
            Session("ItemIndexList") = ""

            'If ddl_compare.SelectedValue = 0 Or ddl_compare.SelectedValue = 1 Or ddl_compare.SelectedValue = 2 Then
            'If ddl_compare.SelectedValue = 0 Then
            '    '//selected Item - need to check 'checkbox'
            '    For Each dgitem In Me.dtg_itemdis.Items
            '        chkbox = dgitem.FindControl("chkSelection")
            '        If chkbox.Checked Then
            '            arrRFQ.Add(dgitem.Cells(itemEnum.Line).Text)  'dtg_itemdis.DataKeys.Item(j)

            '            If Session("ItemIndexList") = "" Then
            '                ItemIndexList = CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim
            '                Session("ItemIndexList") = ItemIndexList
            '            Else
            '                Session("ItemIndexList") = Session("ItemIndexList") & "," & CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim & ""
            '            End If
            '            i = i + 1

            '        End If
            '        j = j + 1
            '    Next
            '    Session("RFQItemList") = arrRFQ
            '    If Session("ItemIndexList") = "" Then
            '        Common.NetMsgbox(Me, "No Item is Selected", MsgBoxStyle.Information)
            '        Exit Sub
            '    ElseIf ViewState("ddl_compare") <> ddl_compare.SelectedValue Then
            '        Common.NetMsgbox(Me, "Compare By have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
            '        Exit Sub
            '    ElseIf Session("itemindexlist") <> Session("itemindexlist1") Then
            '        Common.NetMsgbox(Me, "Selected item(s) have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            'Else
            '    '//Highest/Lowest Overall - no need to check 'checkbox',must buy all items
            '    For Each dgitem In Me.dtg_itemdis.Items
            '        arrRFQ.Add(dgitem.Cells(itemEnum.Line).Text)  'dtg_itemdis.DataKeys.Item(j)
            '        If Session("ItemIndexList") = "" Then
            '            ItemIndexList = CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim
            '            Session("ItemIndexList") = ItemIndexList
            '        Else
            '            Session("ItemIndexList") = Session("ItemIndexList") & "," & CStr(dtg_itemdis.DataKeys.Item(j)).ToString.Trim & ""
            '        End If
            '        i = i + 1
            '        j = j + 1
            '    Next
            '    Session("RFQItemList") = arrRFQ
            '    If ViewState("ddl_compare") <> ddl_compare.SelectedValue Then
            '        Common.NetMsgbox(Me, "Compare By have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            'End If

            'For Each dgitem In Me.dtg_rank.Items
            '    chk = dgitem.FindControl("chkSelection2")
            '    If chk.Checked Then
            '        arr.Add(dgitem.Cells(rankEnum.CompID).Text)
            '        Session("VendorList") = arr
            '        Response.Redirect("../PO/RaisePO.aspx?RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&vendor=" & dgitem.Cells(rankEnum.CompID).Text & "&venname=" & Server.UrlEncode(dgitem.Cells(rankEnum.CoyName).Text) & "&currency=" & dgitem.Cells(rankEnum.Currency).Text & "&type=rfq&rfqid=" & dgitem.Cells(rankEnum.RFQID).Text & "&quono=" & dgitem.Cells(rankEnum.ActQuoNo_2).Text & "&rfqname=" & Server.UrlEncode(lbl_Name.Text) & "&dpage=" & strdPage & "&pageid=" & strPageId & " ")

            '    End If
            'Next


            If ViewState("ddl_compare") = "0" Then 'ddl_compare.SelectedValue = 0 Then
                For a As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                    If (Not IsNothing(Request.Form("chkItemDesc" & a))) AndAlso (Request.Form("chkItemDesc" & a) = "on") Then
                        arrRFQ.Add(Request.Form("iLineNo" & a))
                        ItemIndexList = CStr(Request.Form("iLineNo" & a)).ToString.Trim
                        If Session("ItemIndexList") = "" Then
                            Session("ItemIndexList") = ItemIndexList
                        Else
                            Session("ItemIndexList") = Session("ItemIndexList") & "," & ItemIndexList
                        End If
                    End If
                Next

                Session("RFQItemList") = arrRFQ
                If Session("ItemIndexList") = "" Then
                    Common.NetMsgbox(Me, "No Item is Selected", MsgBoxStyle.Information)
                    Exit Sub
                    'ElseIf ViewState("ddl_compare") <> ddl_compare.SelectedValue Then
                    '    Common.NetMsgbox(Me, "Compare By have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
                    '    Exit Sub
                ElseIf Session("itemindexlist") <> Session("itemindexlist1") Then
                    Common.NetMsgbox(Me, "Selected item(s) have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
                    Exit Sub
                Else
                    If objRFq_ext.chk_itemtype(ViewState("rfq_id"), Session("ItemIndexList")) Then
                    Else
                        Common.NetMsgbox(Me, "Selected items are not from the same item type.", MsgBoxStyle.Information)
                        Exit Sub
                    End If

                End If
            Else
                '//Highest/Lowest Overall - no need to check 'checkbox',must buy all items
                For a As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                    arrRFQ.Add(Request.Form("iLineNo" & a))
                    If Session("ItemIndexList") = "" Then
                        ItemIndexList = CStr(Request.Form("iLineNo" & a)).ToString.Trim
                        Session("ItemIndexList") = ItemIndexList
                    Else
                        Session("ItemIndexList") = Session("ItemIndexList") & "," & ItemIndexList
                    End If
                Next

                Session("RFQItemList") = arrRFQ
                'If ViewState("ddl_compare") <> ddl_compare.SelectedValue Then
                '    Common.NetMsgbox(Me, "Compare By have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
                '    Exit Sub
                'End If
            End If

            Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Me.Request(Trim("RFQ_Num")) & "'")
            If strRFQ_No <> "" Then
                Dim strRFQ_Index As String
                For count As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                    'Dim chkSel As CheckBox
                    Dim itemDesc As String
                    'chkSel = dgitem2.Cells(itemEnum.Chk).FindControl("chkSelection")
                    'itemDesc = dgitem2.Cells(itemEnum.Desc).Text
                    'If chkSel.Checked Then
                    If (Request.Form("chkItemChecked" & count) = "checked") Then
                        'strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                        '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                        '                " RM_RFQ_ID = '" & ViewState("rfq_id") & "' AND " & _
                        '                " (RD_PRODUCT_CODE IN ( " & _
                        '                " SELECT POD_PRODUCT_CODE FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                        '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                        '                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                        '                " RM_RFQ_ID = '" & ViewState("rfq_id") & "') " & _
                        '                " ) OR " & _
                        '                " RD_PRODUCT_DESC IN ( " & _
                        '                " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                        '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                        '                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                        '                " RM_RFQ_ID = '" & ViewState("rfq_id") & "') " & _
                        '                " ) ) AND RD_PRODUCT_DESC = '" & Request.Form("chkItemDescP" & count) & "' LIMIT 1 ")

                        strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                " RM_RFQ_ID = '" & ViewState("rfq_id") & "' AND " & _
                                                " (RD_PRODUCT_DESC IN ( " & _
                                                " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                                                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                " RM_RFQ_ID = '" & ViewState("rfq_id") & "') AND POD_RFQ_ITEM_LINE = '" & Common.Parse(Request.Form("iLineNo" & count)) & "' " & _
                                                " ) ) AND RD_PRODUCT_DESC = '" & Common.Parse(Request.Form("chkItemDescP" & count)) & "' LIMIT 1 ")
                        If strRFQ_Index <> "" Then
                            'Common.NetMsgbox(Me, "PO has been submitted for the selected item(s).", MsgBoxStyle.Information)
                            Common.NetMsgbox(Me, "One or more of the item(s) selected have been issued PO earlier.", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If
                    'End If
                Next
            End If


            'For Each dgitem In Me.dtg_rank.Items
            '    chk = dgitem.FindControl("chkSelection2")
            '    If chk.Checked Then
            '        arr.Add(dgitem.Cells(rankEnum.CompID).Text)
            '        Session("VendorList") = arr
            '        Response.Redirect("../PO/RaisePO.aspx?RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&vendor=" & dgitem.Cells(rankEnum.CompID).Text & "&venname=" & Server.UrlEncode(dgitem.Cells(rankEnum.CoyName).Text) & "&currency=" & dgitem.Cells(rankEnum.Currency).Text & "&type=rfq&rfqid=" & dgitem.Cells(rankEnum.RFQID).Text & "&quono=" & dgitem.Cells(rankEnum.ActQuoNo_2).Text & "&rfqname=" & Server.UrlEncode(lbl_Name.Text) & "&dpage=" & strdPage & "&pageid=" & strPageId & " ")

            '    End If
            'Next

            Dim iSelVendorCount As Int16 = 0, sPOURL As String = ""
            Dim strDelTerm, strItemType, strOversea As String
            arr.Add(Request.Form("rankCompanyId" & Request.Form("chkRank")))
            Session("VendorList") = arr
            'iSelVendorCount = +1
            iSelVendorCount = iSelVendorCount + 1
            'strDelTerm = objDB.GetVal("SELECT IFNULL(RRD_DEL_CODE,'') FROM RFQ_REPLIES_DETAIL " & _
            '        "WHERE RRD_RFQ_ID='" & Common.Parse(ViewState("rfq_id")) & "' AND RRD_Line_No IN (" & Session("ItemIndexList") & ")")
            'strItemType = objDB.GetVal("SELECT IFNULL(RRD_DEL_CODE,'') FROM RFQ_REPLIES_DETAIL " & _
            '        "WHERE RRD_RFQ_ID='" & Common.Parse(ViewState("rfq_id")) & "' AND RRD_Line_No IN (" & Session("ItemIndexList") & ")")
            'strOversea = objDB.GetVal("SELECT IFNULL(RRD_DEL_CODE,'') FROM RFQ_REPLIES_DETAIL " & _
            '        "WHERE RRD_RFQ_ID='" & Common.Parse(ViewState("rfq_id")) & "' AND RRD_Line_No IN (" & Session("ItemIndexList") & ")")
            'sPOURL = dDispatcher.direct("PO", "RaisePO.aspx", "Frm=RFQComSummary&SubFrm=" & Request(Trim("Frm")) & "&deliveryterm=" & strDelTerm & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&vendor=" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "&venname=" & Request.Form("rankCompanyName" & Request.Form("chkRank")) & "&currency=" & Request.Form("rankCurrency" & Request.Form("chkRank")) & "&mode=rfq&type=new&rfqid=" & Request.Form("rankRFQId" & Request.Form("chkRank")) & "&quono=" & Request.Form("rankQuotNum" & Request.Form("chkRank")) & "&rfqname=" & Server.UrlEncode(lbl_Name.Text) & "&dpage=" & strdPage & "&pageid=" & strPageId & " ")

            'For a As Int16 = 0 To ViewState("VendorCount") - 1
            '    If (Not IsNothing(Request.Form("chkRank" & a))) AndAlso (Request.Form("chkRank" & a) = "on") Then
            '        'ItemIndexList = CStr(Request.Form("iLineNo" & a)).ToString.Trim
            '        'Session("VendorList") = ItemIndexList
            '        arr.Add(Request.Form("rankCompanyId" & a))
            '        Session("VendorList") = arr
            '        'iSelVendorCount = +1
            '        iSelVendorCount = iSelVendorCount + 1
            '        sPOURL = dDispatcher.direct("PO", "RaisePO.aspx", "RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&vendor=" & Request.Form("rankCompanyId" & a) & "&venname=" & Request.Form("rankCompanyName" & a) & "&currency=" & Request.Form("rankCurrency" & a) & "&type=rfq&rfqid=" & Request.Form("rankRFQId" & a) & "&quono=" & Request.Form("rankQuotNum" & a) & "&rfqname=" & Server.UrlEncode(lbl_Name.Text) & "&dpage=" & strdPage & "&pageid=" & strPageId & " ")
            '    End If
            'Next

            'If iSelVendorCount > 1 Then
            '    'Common.NetMsgbox(Me, "More than one vendor selected!""&vbCrLf&""Please check only one vendor to Raise PO", MsgBoxStyle.Information)
            '    Common.NetMsgbox(Me, "Please check only one vendor to Raise PO", MsgBoxStyle.Information)
            '    Exit Sub
            'ElseIf iSelVendorCount = 0 Then
            '    'Common.NetMsgbox(Me, "No vendor selected!""&vbCrLf&""Please check at least one vendor to Raise PO", MsgBoxStyle.Information)
            '    Common.NetMsgbox(Me, "Please check one vendor to Raise PO", MsgBoxStyle.Information)
            '    Exit Sub
            'Else
            '    Response.Redirect(sPOURL)
            'End If

            If Request.Form("rankCompanyId" & Request.Form("chkRank")) = "" Then
                Common.NetMsgbox(Me, "Please select vendor to Raise PO", MsgBoxStyle.Information)
            Else
                intMsg = objRFq_ext.chk_itemoversea(ViewState("rfq_id"), Session("ItemIndexList"), Request.Form("rankCompanyId" & Request.Form("chkRank")), strOversea, strDelTerm)

                If intMsg = 0 Then
                    'strDelTerm = objDB.GetVal("SELECT IFNULL(RRD_DEL_CODE,'') FROM RFQ_REPLIES_DETAIL " & _
                    '                    "WHERE RRD_V_COY_ID = '" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "' AND RRD_RFQ_ID='" & Common.Parse(ViewState("rfq_id")) & "' AND RRD_Line_No IN (" & Session("ItemIndexList") & ")")
                    strItemType = objDB.GetVal("SELECT IFNULL(RRD_ITEM_TYPE,'') FROM RFQ_REPLIES_DETAIL " & _
                                        "WHERE RRD_V_COY_ID = '" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "' AND RRD_RFQ_ID='" & Common.Parse(ViewState("rfq_id")) & "' AND RRD_Line_No IN (" & Session("ItemIndexList") & ")")
                    'strOversea = objDB.GetVal("SELECT CASE WHEN PM_OVERSEA IS NULL THEN " & _
                    '                   "(SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = RRD_DEL_CODE AND CDT_COY_ID = '" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "') " & _
                    '                   "ELSE PM_OVERSEA END AS OVERSEA, RRD_DEL_CODE FROM RFQ_REPLIES_DETAIL RRD " & _
                    '                   "LEFT JOIN PRODUCT_MSTR PM ON PM.PM_PRODUCT_CODE = RRD.RRD_PRODUCT_CODE " & _
                    '                   "WHERE RRD_RFQ_ID='" & ViewState("rfq_id") & "' AND RRD_V_COY_ID = '" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "' AND RRD_LINE_NO IN (" & Session("ItemIndexList") & ") ")

                    sPOURL = dDispatcher.direct("PO", "RaisePO.aspx", "Frm=RFQComSummary&SubFrm=" & Request(Trim("Frm")) & "&oversea=" & strOversea & "&itemtype=" & strItemType & "&deliveryterm=" & strDelTerm & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&vendor=" & Request.Form("rankCompanyId" & Request.Form("chkRank")) & "&venname=" & Request.Form("rankCompanyName" & Request.Form("chkRank")) & "&currency=" & Request.Form("rankCurrency" & Request.Form("chkRank")) & "&mode=rfq&type=new&rfqid=" & Request.Form("rankRFQId" & Request.Form("chkRank")) & "&quono=" & Request.Form("rankQuotNum" & Request.Form("chkRank")) & "&rfqname=" & Server.UrlEncode(lbl_Name.Text) & "&dpage=" & strdPage & "&pageid=" & strPageId & " ")
                    Response.Redirect(sPOURL)
                ElseIf intMsg = 1 Then
                    Common.NetMsgbox(Me, "Selected items cannot be a mixture of oversea and local items.", MsgBoxStyle.Information)
                ElseIf intMsg = 2 Then
                    Common.NetMsgbox(Me, "Selected items are not from the same delivery term.", MsgBoxStyle.Information)
                End If
                'If objRFq_ext.chk_itemoversea(ViewState("rfq_id"), Session("ItemIndexList"), Request.Form("rankCompanyId" & Request.Form("chkRank"))) Then
                '    Response.Redirect(sPOURL)
                'Else
                '    Common.NetMsgbox(Me, "Selected items cannot be a mixture of oversea and local items.", MsgBoxStyle.Information)
                '    Exit Sub
                'End If
            End If

        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub ddl_compare_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_compare.SelectedIndexChanged
        If ddl_compare.SelectedValue = 0 Then
            'Me.dtg_rank.Columns(4).Visible = True
            Me.cmd_compare.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        Else
            'Me.dtg_rank.Columns(4).Visible = False
            '//no check box, so need to do any checking
            Me.cmd_compare.Attributes.Remove("onclick")
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'If strFrm = "RFQ_Outstg_List" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"

        'ElseIf strFrm = "RFQ_List" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"
        'End If
        If strFrm = "RFQ_Outstg_List" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"

        ElseIf strFrm = "RFQ_List" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"
        End If
    End Sub

    Private Function ConstructTableItemDesc() As String
        Dim tDS As DataSet, sTable As String = "", sCompanyCol As String = "", aryVendorListId As New ArrayList, aryVendorListName As New ArrayList
        Dim RFQ_ID As String = Me.Request(Trim("RFQ_ID")), sRetPrice As String, dPriceList As Double, arrTotalPrice As New ArrayList
        Dim sCompanyNameCol As String = "", sConstructTotalPrice As String = "", sChkboxStyle = "", iColspan As Int16, sChkChecked, sChkChecked2 As String
        Dim objrfq_Ext As New RFQ_Ext
        tDS = objrfq_Ext.get_itemdis(RFQ_ID)


        aryVendorListId = ViewState("VendorListId")
        aryVendorListName = ViewState("VendorListName")

        Dim dblVendorTotalPrice(ViewState("VendorCount") - 1) As Double
        Session("jqPopup") = ""
        Session("jqPopupPrice") = ""
        Dim iTotalItemDesc As Int16 = tDS.Tables(0).Rows.Count

        If ViewState("ddl_compare") <> "0" Then 'ddl_compare.SelectedItem.Value <> "0" Then
            sChkboxStyle = "display: none; "
            iColspan = 3
        Else
            iColspan = 4
        End If


        For j As Integer = 0 To iTotalItemDesc - 1
            'Construct Company Header column
            sCompanyNameCol = ""
            sCompanyCol = ""
            dPriceList = 0
            For x As Integer = 0 To aryVendorListId.Count - 1
                sCompanyCol = sCompanyCol & ConstructColPrice(RFQ_ID, aryVendorListId(x), tDS.Tables(0).Rows(j).Item("RD_RFQ_Line"), sRetPrice, x + 1, tDS.Tables(0).Rows(j).Item("RD_Quantity"), tDS.Tables(0).Rows(j).Item("RD_Item_Type"))
                Select Case aryVendorListName.Count
                    Case 1
                        sCompanyNameCol = sCompanyNameCol & "<td width=""59%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & aryVendorListName(x) & "</span></td>"

                    Case 2
                        If Len(aryVendorListName(x)) > 35 Then
                            sCompanyNameCol = sCompanyNameCol & "<td width=""29%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 35) & "...</span></td>"
                        Else
                            sCompanyNameCol = sCompanyNameCol & "<td width=""29%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 35) & "</span></td>"
                        End If
                    Case 3
                        If Len(aryVendorListName(x)) > 20 Then
                            sCompanyNameCol = sCompanyNameCol & "<td width=""19%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 20) & "...</span></td>"
                        Else
                            sCompanyNameCol = sCompanyNameCol & "<td width=""19%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 20) & "</span></td>"
                        End If
                    Case 4
                        If Len(aryVendorListName(x)) > 12 Then
                            sCompanyNameCol = sCompanyNameCol & "<td width=""14%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 12) & "...</span></td>"
                        Else
                            sCompanyNameCol = sCompanyNameCol & "<td width=""14%"" style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 12) & "</span></td>"
                        End If
                    Case Else
                        If Len(aryVendorListName(x)) > 10 Then
                            sCompanyNameCol = sCompanyNameCol & "<td style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 10) & "...</span></td>"
                        Else
                            sCompanyNameCol = sCompanyNameCol & "<td style=""text-align: right;""><span style=""cursor: default;"" class=""jq" & aryVendorListId(x) & " "">" & Mid(aryVendorListName(x), 1, 10) & "</span></td>"
                        End If
                End Select
                If x = aryVendorListId.Count - 1 Then
                    Session("jqPopup") = Session("jqPopup") & "$('.jq" & aryVendorListId(x) & "').CreateBubblePopup({innerHtml: '" & aryVendorListName(x) & "',position:'left', align: 'middle', innerHtmlStyle: { color:'#FFFFFF', 'text-align':'center' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"
                Else
                    Session("jqPopup") = Session("jqPopup") & "$('.jq" & aryVendorListId(x) & "').CreateBubblePopup({innerHtml: '" & aryVendorListName(x) & "',innerHtmlStyle: { color:'#FFFFFF', 'text-align':'center' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"
                End If


                If IsNumeric(sRetPrice) Then
                    dblVendorTotalPrice(x) = dblVendorTotalPrice(x) + CDbl(sRetPrice)
                End If
            Next

            If Page.IsPostBack Then
                If (Not IsNothing(Request.Form("chkItemDesc" & j))) AndAlso (Request.Form("chkItemDesc" & j) = "on") Then
                    sChkChecked = " checked = ""checked"""
                    sChkChecked2 = "checked"
                Else
                    sChkChecked = ""
                    sChkChecked2 = "nochecked"
                End If
            Else
                sChkChecked = " checked = ""checked"""
                sChkChecked2 = "checked"
                If Not Page.IsPostBack Then
                    If Session("ItemIndexList1") = "" Then
                        Session("ItemIndexList1") = CStr(tDS.Tables(0).Rows(j).Item("RD_RFQ_Line"))
                    Else
                        Session("ItemIndexList1") = Session("ItemIndexList1") & "," & CStr(tDS.Tables(0).Rows(j).Item("RD_RFQ_Line"))
                    End If
                End If
            End If

            If ((j Mod 2) = 0) Then
                sTable = sTable & "<tr style=""background-color:#fdfdfd;""><td style=""" & sChkboxStyle & """><input type=""checkbox"" " & sChkChecked & " id=""chkItemDesc" & j & """ name=""chkItemDesc" & j & """ onclick=""javascript:checkItemDescHeader();"" /></td><td>" & HttpContext.Current.Server.HtmlEncode(tDS.Tables(0).Rows(j).Item("RD_Product_Desc")) & "</td><input name=""chkItemChecked" & j & """ id=""chkItemChecked" & j & """ type=""hidden"" value=""" & sChkChecked2 & """ /><input name=""chkItemDescP" & j & """ id=""chkItemDescP" & j & """ type=""hidden"" value=""" & HttpContext.Current.Server.HtmlEncode(tDS.Tables(0).Rows(j).Item("RD_Product_Desc")) & """ /><td>" & tDS.Tables(0).Rows(j).Item("ITEM_TYPE") & "</td><td>" & tDS.Tables(0).Rows(j).Item("RD_UOM") & "</td><td style=""text-align: right;"">" & tDS.Tables(0).Rows(j).Item("RD_Quantity") & "<input name=""iLineNo" & j & """ id=""iLineNo" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RD_RFQ_Line") & """ /></td>" & sCompanyCol & "</tr>"
            Else
                sTable = sTable & "<tr style=""background-color:#f5f9fc;""><td style=""" & sChkboxStyle & """><input type=""checkbox"" " & sChkChecked & " id=""chkItemDesc" & j & """ name=""chkItemDesc" & j & """ onclick=""javascript:checkItemDescHeader();"" /></td><td>" & HttpContext.Current.Server.HtmlEncode(tDS.Tables(0).Rows(j).Item("RD_Product_Desc")) & "</td><input name=""chkItemChecked" & j & """ id=""chkItemChecked" & j & """ type=""hidden"" value=""" & sChkChecked2 & """ /><input name=""chkItemDescP" & j & """ id=""chkItemDescP" & j & """ type=""hidden"" value=""" & HttpContext.Current.Server.HtmlEncode(tDS.Tables(0).Rows(j).Item("RD_Product_Desc")) & """ /><td>" & tDS.Tables(0).Rows(j).Item("ITEM_TYPE") & "</td><td>" & tDS.Tables(0).Rows(j).Item("RD_UOM") & "</td><td style=""text-align: right;"">" & tDS.Tables(0).Rows(j).Item("RD_Quantity") & "<input name=""iLineNo" & j & """ id=""iLineNo" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RD_RFQ_Line") & """ /></td>" & sCompanyCol & "</tr>"
            End If
        Next

        For x As Integer = 0 To ViewState("VendorCount") - 1
            sConstructTotalPrice = sConstructTotalPrice & "<td style=""text-align: right;"">" & Format(dblVendorTotalPrice(x), "#,##0.00") & "</td>"
        Next
        'ConstructTableItemDesc = "<table border=""1"" class=""grid"" style=""width:100%; border-collapse:collapse; line-height:20px; "" ><tr class=""GridHeader"" style=""font-weight:bold;""><td width=""4%"" style=""" & sChkboxStyle & """><input type=""checkbox"" id=""chkItemDescAll"" name=""chkItemDescAll"" onclick=""javascript:selectItemDescAll();"" /></td><td width=""21%"">Item Description</td><td width=""9%"">UOM</td><td width=""7%"" style=""text-align: right;"">QTY</td>" & _
        '    sCompanyNameCol & "</tr>" & sTable & _
        '    "<tr style=""font-weight:bold; background-color:#f4f4f4;""><td colspan=""" & iColspan & """ style=""text-align:right; "">Quotation Value</td>" & sConstructTotalPrice & "</tr></table>" & _
        '    "<input type=""hidden"" name=""hidTotalItemDesc"" id=""hidTotalItemDesc"" value=""" & iTotalItemDesc & """ /> "
        ConstructTableItemDesc = "<table border=""1"" class=""grid"" style=""width:100%; border-collapse:collapse; line-height:20px; "" ><tr class=""GridHeader"" style=""font-weight:bold;""><td width=""4%"" style=""" & sChkboxStyle & """><input type=""checkbox"" id=""chkItemDescAll"" name=""chkItemDescAll"" onclick=""javascript:selectItemDescAll();"" /></td><td width=""16%"">Item Description</td><td width=""8%"">Item Type</td><td width=""8%"">UOM</td><td width=""5%"" style=""text-align: right;"">QTY</td>" & _
                        sCompanyNameCol & "</tr>" & sTable & _
                        "</table>" & _
                        "<input type=""hidden"" name=""hidTotalItemDesc"" id=""hidTotalItemDesc"" value=""" & iTotalItemDesc & """ /> " & _
                        "<input type=""hidden"" name=""hidTotalVendor"" id=""hidTotalVendor"" value=""" & aryVendorListId.Count & """ /> "

        'ConstructTableItemDesc = "<table border=""1"" class=""grid"" style=""width:100%; border-collapse:collapse; line-height:20px; "" ><tr class=""GridHeader"" style=""font-weight:bold;""><td width=""4%"" style=""" & sChkboxStyle & """><input type=""checkbox"" id=""chkItemDescAll"" name=""chkItemDescAll"" onclick=""javascript:selectItemDescAll();"" /></td><td width=""21%"">Item Description</td><td width=""9%"">UOM</td><td width=""7%"" style=""text-align: right;"">QTY</td>" & _
        '                    sCompanyNameCol & "</tr>" & sTable & "</table>" & _
        '                    "<input type=""hidden"" name=""hidTotalItemDesc"" id=""hidTotalItemDesc"" value=""" & iTotalItemDesc & """ /> "

        ViewState("iTotalItemDesc") = iTotalItemDesc
    End Function

    Private Function ConstructColPrice(ByVal lRFQId As Long, ByVal sCompanyId As String, ByVal iLineNo As Int16, ByRef strPrice As String, ByVal intCoId As Integer, ByVal decQty As Decimal, ByVal strItemType As String) As String
        Dim ds As New DataSet
        Dim dt As New DataTable, sCol As String
        Dim decPrice, decPriceM, decCurr, decPercFactor As Decimal
        Dim strCurr, strPrice2, strPriceM, strOversea As String

        DisplayUnitPricePopup(lRFQId, sCompanyId, iLineNo, strCurr, decCurr, decPercFactor, strOversea)

        ds = objrfq.get_Price(lRFQId, iLineNo, sCompanyId)
        dt = ds.Tables(0)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)("Price")) Then
                strPrice = "No Quote"
                strPriceM = "No Quote"
            Else
                'decPriceM = CalLocalPrice(lRFQId, sCompanyId, iLineNo, decCurr, decPercFactor, decQty, dt.Rows(0)("Price"), strItemType)
                decPriceM = CalLocalPrice2(decCurr, decPercFactor, decQty, dt.Rows(0)("Price"), strItemType, strOversea)
                decPrice = dt.Rows(0)("Price")
                'strPrice = Format(dblPrice, "0.0000")
                strPrice = Format(decPrice, "#,###,##0.00")
                strPrice2 = strCurr & " " & Format(decPrice, "#,###,##0.00") & " | MYR "
                strPriceM = Format(decPriceM, "#,###,##0.00")
            End If
        Else
            strPrice = "No Quote"
            strPriceM = "No Quote"
        End If

        'sCol = "<td id=""c" & intCoId & "_r" & iLineNo & """ style=""text-align:right; "" >" & strPrice & "<input name=""RRD_GST" & iLineNo & """ id=""RRD_GST" & iLineNo & """ type=""hidden"" value=""" & dt.Rows(0)("RRD_GST") & """ /></td>"
        'sCol = "<td id=""r" & iLineNo & "_c" & intCoId & """ style=""text-align:right; "" >" & strPrice2 & "</td><input name=""RRD_GST" & iLineNo & """ id=""RRD_GST" & iLineNo & """ type=""hidden"" value=""" & dt.Rows(0)("RRD_GST") & """ />"
        If strItemType = "ST" Then
            sCol = "<td id=""rw" & iLineNo & "_c" & intCoId & """ style=""text-align:right; "" ><span style=""cursor:default;float:left;"" class=""jqUnitPrice" & sCompanyId & iLineNo & """><IMG src=""" & dDispatcher.direct("Plugins/images", "v_icon.gif") & """></span>" & strPrice2 & "<span id=""r" & iLineNo & "_c" & intCoId & """>" & strPriceM & "</span></td><input name=""RRD_GST" & iLineNo & """ id=""RRD_GST" & iLineNo & """ type=""hidden"" value=""" & dt.Rows(0)("RRD_GST") & """ />"
        Else
            sCol = "<td id=""rw" & iLineNo & "_c" & intCoId & """ style=""text-align:right; "" >" & strPrice2 & "<span id=""r" & iLineNo & "_c" & intCoId & """>" & strPriceM & "</span></td><input name=""RRD_GST" & iLineNo & """ id=""RRD_GST" & iLineNo & """ type=""hidden"" value=""" & dt.Rows(0)("RRD_GST") & """ />"
        End If


        ConstructColPrice = sCol
    End Function

    Private Function ConstructTableRank() As String
        Dim tDS As DataSet, sTable As String = "", sDisableChk As String
        Dim RFQ_ID As String = Me.Request(Trim("RFQ_ID"))
        Dim array(ViewState("iTotalItemDesc") - 1) As String
        Dim c, j, k As Integer
        Dim ObjRfq_ext As New RFQ_Ext
        Dim decCurr, decGRNFactor, decUnitPrice, decTotalPrice As Decimal
        Dim strGRNFactor, strTemp, strCompName As String
        Dim dtRank As New DataTable
        Dim dsRank As New DataSet

        If ViewState("ddl_compare") = "0" Then 'ddl_compare.SelectedItem.Value = "0" Then
            i = 0
            Session("itemindexlist1") = ""
            For a As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                If (Not IsNothing(Request.Form("chkItemDesc" & a))) AndAlso (Request.Form("chkItemDesc" & a) = "on") Then
                    array(i) = Request.Form("iLineNo" & a)
                    If Session("ItemIndexList1") = "" Then
                        Session("ItemIndexList1") = Request.Form("iLineNo" & a)
                    Else
                        Session("ItemIndexList1") = Session("ItemIndexList1") & "," & Request.Form("iLineNo" & a)
                    End If
                    i += 1
                End If
            Next

            If Not Page.IsPostBack Then
                tDS = objrfq.get_itemrank(RFQ_ID, "1", array, "ASC", 0, i)
            Else
                tDS = objrfq.get_itemrank(RFQ_ID, "1", array, "ASC", 0, i)
            End If


        ElseIf ViewState("ddl_compare") = "1" Then 'ddl_compare.SelectedItem.Value = "1" Then
            tDS = objrfq.get_itemrank(RFQ_ID, "0", array, "ASC", 0, i)
        ElseIf ViewState("ddl_compare") = "2" Then 'ddl_compare.SelectedItem.Value = "2" Then
            tDS = objrfq.get_itemrank(RFQ_ID, "0", array, "DESC", 0, i)
        End If

        Dim iTotalItemDesc As Int16 = tDS.Tables(0).Rows.Count

        dtRank.Columns.Add("RRM_V_COMPANY_ID", Type.GetType("System.String"))
        dtRank.Columns.Add("CM_COY_NAME", Type.GetType("System.String"))
        dtRank.Columns.Add("RRM_ACTUAL_QUOT_NUM", Type.GetType("System.String"))
        dtRank.Columns.Add("RRM_CURRENCY_CODE", Type.GetType("System.String"))
        dtRank.Columns.Add("RRM_TOTALVALUE", Type.GetType("System.Decimal"))
        dtRank.Columns.Add("RRM_TOTALVALUE2", Type.GetType("System.Decimal"))
        dtRank.Columns.Add("RRM_RFQ_ID", Type.GetType("System.String"))
        dtRank.Columns.Add("RRM_OFFER_TILL", Type.GetType("System.String"))

        Dim dtr As DataRow
        For j = 0 To iTotalItemDesc - 1
            Dim vDS As New DataSet

            vDS = ObjRfq_ext.get_venInfo(RFQ_ID, tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID"), Session("ItemIndexList1"))
            decTotalPrice = 0

            If vDS.Tables(0).Rows.Count > 0 Then
                strTemp = objDB.GetVal("SELECT CE_RATE FROM COMPANY_EXCHANGERATE WHERE CE_COY_ID = '" & Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & vDS.Tables(0).Rows(0)("RRM_CURRENCY_CODE") & "' AND CE_DELETED = 'N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE()")

                If strTemp <> "" Then
                    decCurr = CDec(strTemp)
                Else
                    decCurr = 0
                    ViewState("Curr") = False
                End If

                For c = 0 To vDS.Tables(0).Rows.Count - 1

                    If IsDBNull(vDS.Tables(0).Rows(c)("Price")) Then
                        decUnitPrice = 0
                    Else
                        decUnitPrice = vDS.Tables(0).Rows(c)("Price")
                    End If

                    strGRNFactor = objDB.GetVal("SELECT IFNULL(CDT_DEL_GRNFACTOR,0) FROM company_delivery_term WHERE CDT_COY_ID = '" & Session("CompanyId") & "' AND CDT_DEL_CODE = '" & Common.Parse(vDS.Tables(0).Rows(c)("RRD_DEL_CODE")) & "'")

                    If strGRNFactor = "" Or IsDBNull(strGRNFactor) Then
                        decGRNFactor = 0
                    Else
                        decGRNFactor = CDec(strGRNFactor)
                    End If


                    decUnitPrice = CalLocalPrice2(decCurr, decGRNFactor, vDS.Tables(0).Rows(c)("RRD_Quantity"), decUnitPrice, vDS.Tables(0).Rows(c)("RRD_Item_Type"), vDS.Tables(0).Rows(c)("OVERSEA"))

                    If decTotalPrice <> 0 Then
                        decTotalPrice = decTotalPrice + decUnitPrice
                    Else
                        decTotalPrice = decUnitPrice
                    End If
                Next
            End If

            dtr = dtRank.NewRow
            dtr("RRM_V_COMPANY_ID") = tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID")
            dtr("CM_COY_NAME") = tDS.Tables(0).Rows(j).Item("CM_COY_NAME")
            dtr("RRM_ACTUAL_QUOT_NUM") = tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num")
            dtr("RRM_CURRENCY_CODE") = tDS.Tables(0).Rows(j).Item("RRM_Currency_Code")
            dtr("RRM_TOTALVALUE") = tDS.Tables(0).Rows(j).Item("RRM_TotalValue")
            dtr("RRM_TOTALVALUE2") = decTotalPrice
            dtr("RRM_RFQ_ID") = tDS.Tables(0).Rows(j).Item("RRM_RFQ_ID")
            dtr("RRM_OFFER_TILL") = tDS.Tables(0).Rows(j).Item("RRM_Offer_Till")
            dtRank.Rows.Add(dtr)

        Next

        dtRank.DefaultView.Sort = "RRM_TOTALVALUE2"
        dtRank = dtRank.DefaultView.ToTable()
        dsRank.Tables.Add(dtRank)

        For j = 0 To iTotalItemDesc - 1
            If (Session("disable") <> "Y" Or Me.Request(Trim("disabled")) = "N") And CDate(dsRank.Tables(0).Rows(j).Item("RRM_OFFER_TILL")) >= Today.Date And ViewState("Bstatus") = "0" Then
                If Request.QueryString("Appr") = "Y" Or Request.QueryString("Frm") = "POViewB2" Or Request.QueryString("Frm") = "POViewB2Cancel" Or Request.QueryString("Frm") = "POViewTrx" Or Request.QueryString("Frm") = "SearchPO_AO" Or Request.QueryString("Frm") = "SearchPO_All" Then
                    sDisableChk = "disabled = ""disabled"" "
                Else
                    sDisableChk = ""
                End If
            Else
                sDisableChk = "disabled = ""disabled"" "
            End If

            If ((j Mod 2) = 0) Then
                If Request.QueryString("Appr") = "Y" Then
                    'sTable = sTable & "<tr style=""background-color:#fdfdfd;""><td><input " & sDisableChk & " type=""checkbox"" id=""chkRank" & j & """ name=""chkRank" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(tDS.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & "</td></tr>"
                    sTable = sTable & "<tr style=""background-color:#fdfdfd;""><td><input " & sDisableChk & " type=""radio"" id=""chkRank" & j & """ name=""chkRank"" value=""" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Appr=Y&frm=RFQComSummary&SubFrm=" & strFrm & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & " | MYR " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue2")), "###,###,##0.00") & "</td></tr>"
                Else
                    'sTable = sTable & "<tr style=""background-color:#fdfdfd;""><td><input " & sDisableChk & " type=""checkbox"" id=""chkRank" & j & """ name=""chkRank" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(tDS.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & "</td></tr>"
                    sTable = sTable & "<tr style=""background-color:#fdfdfd;""><td><input " & sDisableChk & " type=""radio"" id=""chkRank" & j & """ name=""chkRank"" value=""" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=RFQComSummary&SubFrm=" & strFrm & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & " | MYR " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue2")), "###,###,##0.00") & "</td></tr>"
                End If
            Else
                If Request.QueryString("Appr") = "Y" Then
                    'sTable = sTable & "<tr style=""background-color:#f5f9fc;""><td><input " & sDisableChk & " type=""checkbox"" id=""chkRank" & j & """ name=""chkRank" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(tDS.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & "</td></tr>"
                    sTable = sTable & "<tr style=""background-color:#f5f9fc;""><td><input " & sDisableChk & " type=""radio"" id=""chkRank" & j & """ name=""chkRank"" value=""" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Appr=Y&frm=RFQComSummary&SubFrm=" & strFrm & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & " | MYR " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue2")), "###,###,##0.00") & "</td></tr>"
                Else
                    'sTable = sTable & "<tr style=""background-color:#f5f9fc;""><td><input " & sDisableChk & " type=""checkbox"" id=""chkRank" & j & """ name=""chkRank" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&v_com_id=" & tDS.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & tDS.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & tDS.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & tDS.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(tDS.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & "</td></tr>"
                    sTable = sTable & "<tr style=""background-color:#f5f9fc;""><td><input " & sDisableChk & " type=""radio"" id=""chkRank" & j & """ name=""chkRank"" value=""" & j & """ /><input name=""rankCompanyId" & j & """ id=""rankCompanyId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID") & """ /><input name=""rankCompanyName" & j & """ id=""rankCompanyName" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & """ /><input name=""rankCurrency" & j & """ id=""rankCurrency" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & """ /><input name=""rankRFQId" & j & """ id=""rankRFQId" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_RFQ_ID") & """ /><input name=""rankQuotNum" & j & """ id=""rankQuotNum" & j & """ type=""hidden"" value=""" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & """ /></td><td>" & j + 1 & "</td><td><A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "frm=RFQComSummary&SubFrm=" & strFrm & "&side=vendorsum&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & strPageId & " &RFQ_Num=" & ViewState("rfq_num") & "&RFQ_ID=" & Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")) & "&v_com_id=" & dsRank.Tables(0).Rows(j).Item("RRM_V_Company_ID")) & """ >" & dsRank.Tables(0).Rows(j).Item("CM_COY_NAME") & "</a></td><td>" & dsRank.Tables(0).Rows(j).Item("RRM_Actual_Quot_Num") & "</td><td style=""text-align:right;"">" & dsRank.Tables(0).Rows(j).Item("RRM_Currency_Code") & " " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue")), "###,###,##0.00") & " | MYR " & Format(Common.parseNull(dsRank.Tables(0).Rows(j).Item("RRM_TotalValue2")), "###,###,##0.00") & "</td></tr>"
                End If

            End If
        Next

        ConstructTableRank = "<table border=""1"" class=""grid"" style=""margin-top:10px; width:100%; border-collapse:collapse; line-height:20px; "" >" & _
                                "<tr class=""GridHeader"" style=""font-weight:bold;""><td colspan=""5"">Comparison Summary > Generate PO for selected item(s) </td></tr>" & _
                                "<tr style=""height:1px;""><td colspan=""5"" style=""background-color:#fff;""></td></tr>" & _
                                "<tr class=""GridHeader"" style=""font-weight:bold;""><td width=""4%"" ></td><td width=""5%"">Rank</td><td width=""43%"">Vendor Name</td><td width=""18%"">Quotation Number</td><td width=""30%"" style=""text-align:right;"">Total Value</td></tr>" & _
                                sTable & _
                                "</table><input type=""hidden"" name=""hidTotalRank"" id=""hidTotalRank"" value=""" & tDS.Tables(0).Rows.Count & """ /> "
    End Function

    Private Sub FillPrice()
        Dim intCol As Integer = 0
        Dim dgItem As DataGridItem
        Dim dgCol As DataGridColumn
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dblPrice As Double
        Dim strPrice As String = ""

        For Each dgItem In Me.dtg_itemdis.Items
            intCol = Me.dtg_itemdis.Columns.Count - (ViewState("VendorCount") * 2)
            Do While intCol < Me.dtg_itemdis.Columns.Count - 1
                ds = objrfq.get_Price(Me.Request(Trim("RFQ_ID")), dgItem.Cells(4).Text, Me.dtg_itemdis.Columns(intCol).HeaderText)
                dt = ds.Tables(0)

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)("Price")) Then
                        strPrice = "No Quote"
                    Else
                        dblPrice = dt.Rows(0)("Price")
                        strPrice = Format(dblPrice, "#,###,##0.0000")
                    End If

                Else
                    strPrice = "No Quote"

                End If
                dgItem.Cells(intCol + 1).Text = strPrice
                dgItem.Cells(intCol + 1).HorizontalAlign = HorizontalAlign.Right
                intCol = intCol + 2
            Loop
        Next
        'AddRow(intSubTotalCol, "Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")), False)
        AddRow()

    End Sub

    Private Sub AddRow()
        Dim intCol As Integer = 0
        Dim dgCol As DataGridColumn
        Dim dblPrice As Double
        Dim strPrice As String = ""
        Dim arr As New ArrayList
        Dim i As Integer = 0

        intCol = Me.dtg_itemdis.Columns.Count - (ViewState("VendorCount") * 2)

        For i = 0 To ViewState("VendorCount") - 1
            strPrice = Format(CDbl(objrfq.get_QuoTotalValue(Me.Request(Trim("RFQ_ID")), Me.dtg_itemdis.Columns(intCol).HeaderText)), "###,##0.00")
            arr.Add(strPrice)
            intCol = intCol + 2
        Next

        AddRow1("Total Value ", arr)

    End Sub

    Sub AddRow1(ByVal strLabel As String, ByVal arrTotal As ArrayList)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim i As Integer = 0
        Dim strTmp As String = ""
        Dim intStart As Integer = 0
        Dim intEnd As Integer = 0
        Dim intCol As Integer = 0

        intTotalCol = intRemarkCol + intCnt - 1


        For intL = 0 To Me.dtg_itemdis.Columns.Count - 1 'intCell 
            addCell(row)
        Next


        row.Cells(3).Text = strLabel
        row.Cells(3).Font.Bold = True
        intCol = Me.dtg_itemdis.Columns.Count - (ViewState("VendorCount") * 2)

        Do While intCol < Me.dtg_itemdis.Columns.Count - 1
            row.Cells(intCol + 1).Text = arrTotal(i)
            row.Cells(intCol + 1).Font.Bold = True
            row.Cells(intCol + 1).HorizontalAlign = HorizontalAlign.Right
            i = i + 1
            intCol = intCol + 2
        Loop

        row.BackColor = Color.FromName("#f4f4f4")
        Me.dtg_itemdis.Controls(0).Controls.Add(row)

        If ViewState("ddl_compare") <> "0" Then 'ddl_compare.SelectedItem.Value <> "0" Then
            Me.dtg_itemdis.Columns(0).Visible = False
        Else
            Me.dtg_itemdis.Columns(0).Visible = True
        End If

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub DisplayUnitPricePopup(ByVal iRFQid As Integer, ByVal strVComp As String, ByVal iItemLine As Integer, ByRef strCurr As String, ByRef decCurr As Decimal, ByRef decPercFactor As Decimal, ByRef strOversea As String)
        Dim ds As New DataSet
        Dim c, i As Integer
        Dim aryTemp As New ArrayList()
        Dim strVol, strline, strPrice, strPriceM, strsql As String
        Dim decFactor, decPrice, decAmountF, decAmountM, decUnitCost As Decimal
        Dim strTemp, strFactor As String

        strsql = "SELECT CE_RATE FROM COMPANY_EXCHANGERATE WHERE CE_CURRENCY_CODE = " & _
                "(SELECT RRM_Currency_Code FROM rfq_replies_mstr WHERE RRM_RFQ_ID = " & iRFQid & " AND RRM_V_Company_ID = '" & strVComp & "') " & _
                "AND CE_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE() AND CE_DELETED = 'N'"

        strTemp = objDB.GetVal(strsql)

        If strTemp <> "" Then
            decCurr = CDec(strTemp)
        Else
            decCurr = 0
            ViewState("Curr") = False
        End If

        strsql = "SELECT RRM_Currency_Code FROM rfq_replies_mstr WHERE RRM_RFQ_ID = " & iRFQid & " AND RRM_V_Company_ID = '" & strVComp & "'"

        strCurr = objDB.GetVal(strsql)

        strsql = "SELECT CDT_DEL_GRNFACTOR FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = " & _
                "(SELECT RRD_DEL_CODE FROM rfq_replies_detail WHERE RRD_RFQ_ID = " & iRFQid & " AND RRD_V_Coy_ID = '" & strVComp & "' AND RRD_LINE_NO = " & iItemLine & ") " & _
                "AND CDT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDT_DELETED = 'N'"

        strFactor = objDB.GetVal(strsql)

        strsql = "SELECT CDT_DEL_OVERSEA FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = " & _
                "(SELECT RRD_DEL_CODE FROM rfq_replies_detail WHERE RRD_RFQ_ID = " & iRFQid & " AND RRD_V_Coy_ID = '" & strVComp & "' AND RRD_LINE_NO = " & iItemLine & ") " & _
                "AND CDT_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDT_DELETED = 'N'"

        strOversea = objDB.GetVal(strsql)

        If strFactor = "" Or IsDBNull(strFactor) Then
            decPercFactor = 0
        Else
            decPercFactor = CDec(strFactor)
        End If


        'strsql = "SELECT RRVP_VOLUME, RRVP_VOLUME_PRICE, RRVP_LINE_NO, RRVP_V_COY_ID FROM RFQ_REPLIES_VOLUME_PRICE WHERE "
        'strsql &= "RRVP_RFQ_ID = " & iRFQid & " AND RRVP_V_COY_ID = '" & strVComp & "' AND RRVP_LINE_NO = " & iItemLine & ""
        ds = objrfq.GetUnitPriceQuotation(iRFQid, iItemLine, strVComp)

        aryTemp.Clear()

        If ds.Tables(0).Rows.Count > 0 Then
            strline = "UnitPrice" & Common.parseNull(ds.Tables(0).Rows(0).Item("RRVP_V_COY_ID")) & Common.parseNull(ds.Tables(0).Rows(0).Item("RRVP_LINE_NO"))

            For i = 0 To ds.Tables(0).Rows.Count - 1

                strVol = Common.parseNull(ds.Tables(0).Rows(i).Item("RRVP_VOLUME"))
                decPrice = Common.parseNull(ds.Tables(0).Rows(i).Item("RRVP_VOLUME_PRICE"))

                'strPrice = strCurr & " " & iVol_Price

                'Amount(M) = Amount(F) * Exchange Rate
                decAmountM = decPrice * decCurr

                'GRN Factor = Amount(M) * % of GRN Factor
                'Unit Cost = (Unit Price * Exchange Rate) + GRN Factor/Qty
                If CStr(decPercFactor) <> "" Then
                    decFactor = decAmountM * decPercFactor / 100
                    decUnitCost = decAmountM + decFactor
                Else
                    decUnitCost = decAmountM
                End If

                strPrice = strCurr & " " & decPrice
                strPriceM = "MYR " & Format(decUnitCost, "###0.00")

                aryTemp.Add(New String() {strVol, strPrice, strPriceM})
            Next

            ContructRow(strline, aryTemp)

        End If


    End Sub

    Private Function ContructRow(ByVal strCompLine As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer


        For i = 0 To aryVolume.Count - 1
            'If aryVolume(i)(3) = strVCompLine Then
            strrow &= "Volume " & aryVolume(i)(0) & " : " & aryVolume(i)(1) & " : " & aryVolume(i)(2) & "<BR>"
            'End If
        Next

        strtable = strrow

        Session("jqPopupPrice") = Session("jqPopupPrice") & "$('.jq" & strCompLine & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function

    Private Function CalLocalPrice2(ByVal decCurr As Decimal, ByVal decPercFactor As Decimal, ByVal decQty As Decimal, ByVal decPrice As Decimal, ByVal strItemType As String, ByVal strOversea As String) As Double
        Dim decAmtF, decAmtM, decFactor As Decimal

        'Amount F = (Unit Price x Quantity) + (Unit Price x Quantity * Tax GST /100)
        decAmtF = decPrice

        'Amount M = Amount F x Exchange Rate
        decAmtM = decAmtF * decCurr

        'If strItemType = "ST" Then
        '    'GRN Factor = Amount M x Percentage of GRN Factor
        '    decFactor = decAmtM * decPercFactor / 100

        '    'Unit Cost = Amount M + GRN Factor / Quantity
        '    'decPrice = (decPrice * decCurr) + decFactor / decQty
        '    decPrice = (decPrice * decCurr) + decFactor
        'Else
        '    decPrice = decAmtM
        'End If

        'If strOversea = "Y" Then
        '    'GRN Factor = Amount M x Percentage of GRN Factor
        '    decFactor = decAmtM * decPercFactor / 100

        '    'Unit Cost = Amount M + GRN Factor / Quantity
        '    'decPrice = (decPrice * decCurr) + decFactor / decQty
        '    decPrice = (decPrice * decCurr) + decFactor
        'Else
        '    decPrice = decAmtM
        'End If

        'GRN Factor = Amount M x Percentage of GRN Factor
        decFactor = decAmtM * decPercFactor / 100

        'Unit Cost = Amount M + GRN Factor / Quantity
        'decPrice = (decPrice * decCurr) + decFactor / decQty
        decPrice = (decPrice * decCurr) + decFactor

        CalLocalPrice2 = decPrice

    End Function

    Private Sub cmd_export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_export.Click
        Dim strMsg As String
        Dim objRFq_ext As New RFQ_Ext
        Dim arr As New ArrayList
        Dim i As Integer
        'If checkMandatory(strMsg) Then
        Dim dgitem, dgitem2 As DataGridItem
        Dim chk As CheckBox
        ViewState("PR") = "1"
        Dim arrRFQ As New ArrayList
        Dim chkbox As CheckBox
        Dim j As Integer
        Dim item_desc As String
        Dim ItemIndexList As String
        Dim ds As New DataSet
        Dim ds2 As New DataSet
        Dim objEx As New AppExcel_Ext
        Dim cRules As New myCollection

        Session("ItemIndexList") = ""

        If ViewState("ddl_compare") = "0" Then 'ddl_compare.SelectedValue = 0 Then
            For a As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                If (Not IsNothing(Request.Form("chkItemDesc" & a))) AndAlso (Request.Form("chkItemDesc" & a) = "on") Then
                    arrRFQ.Add(Request.Form("iLineNo" & a))
                    ItemIndexList = CStr(Request.Form("iLineNo" & a)).ToString.Trim
                    If Session("ItemIndexList") = "" Then
                        Session("ItemIndexList") = ItemIndexList
                    Else
                        Session("ItemIndexList") = Session("ItemIndexList") & "," & ItemIndexList
                    End If
                End If
            Next

            Session("RFQItemList") = arrRFQ
            If Session("ItemIndexList") = "" Then
                Common.NetMsgbox(Me, "No Item is Selected", MsgBoxStyle.Information)
                Exit Sub
            ElseIf Session("itemindexlist") <> Session("itemindexlist1") Then
                Common.NetMsgbox(Me, "Selected item(s) have changed!""&vbCrLf&""Please press Compare to confirm your item(s)", MsgBoxStyle.Information)
                Exit Sub
            Else
                If objRFq_ext.chk_itemtype(ViewState("rfq_id"), Session("ItemIndexList"), True) Then
                Else
                    Common.NetMsgbox(Me, "Please select items with Stock item type to Export Item", MsgBoxStyle.Information)
                    Exit Sub
                End If

            End If
        Else
            For a As Int16 = 0 To ViewState("iTotalItemDesc") - 1
                arrRFQ.Add(Request.Form("iLineNo" & a))
                If Session("ItemIndexList") = "" Then
                    ItemIndexList = CStr(Request.Form("iLineNo" & a)).ToString.Trim
                    Session("ItemIndexList") = ItemIndexList
                Else
                    Session("ItemIndexList") = Session("ItemIndexList") & "," & ItemIndexList
                End If
            Next

            Session("RFQItemList") = arrRFQ

        End If

        'Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Me.Request(Trim("RFQ_Num")) & "'")
        'If strRFQ_No <> "" Then
        '    Dim strRFQ_Index As String
        '    For count As Int16 = 0 To ViewState("iTotalItemDesc") - 1

        '        Dim itemDesc As String

        '        If (Request.Form("chkItemChecked" & count) = "checked") Then
        '            strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
        '                                    " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
        '                                    " RM_RFQ_ID = '" & ViewState("rfq_id") & "' AND " & _
        '                                    " (RD_PRODUCT_DESC IN ( " & _
        '                                    " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
        '                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
        '                                    " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
        '                                    " RM_RFQ_ID = '" & ViewState("rfq_id") & "') AND POD_RFQ_ITEM_LINE = '" & Common.Parse(Request.Form("iLineNo" & count)) & "' " & _
        '                                    " ) ) AND RD_PRODUCT_DESC = '" & Common.Parse(Request.Form("chkItemDescP" & count)) & "' LIMIT 1 ")
        '            If strRFQ_Index <> "" Then
        '                Common.NetMsgbox(Me, "One or more of the item(s) selected have been issued PO earlier.", MsgBoxStyle.Information)
        '                Exit Sub
        '            End If
        '        End If
        '    Next
        'End If

        Dim iSelVendorCount As Int16 = 0, sPOURL As String = ""

        arr.Add(Request.Form("rankCompanyId" & Request.Form("chkRank")))
        Session("VendorList") = arr
        iSelVendorCount = iSelVendorCount + 1

        If Request.Form("rankCompanyId" & Request.Form("chkRank")) = "" Then
            Common.NetMsgbox(Me, "Please select vendor to Export Item", MsgBoxStyle.Information)
        Else
            Dim dsPackType, dsDelTerm, dsCommodity As DataSet
            Dim intPackTypeRecord, intDelTermRecord, intCommodityRecord As Integer

            'Read Packing Type, Delivery Term
            Dim strFile As String = "ItemBIMListingTemplate"
            Dim strFileName As String = "ItemBIMListing_" & Session("CompanyId") & "_" & Session("UserId") & ".xls"

            dsPackType = Product_Ext.GetItemInfoToExcel("PT")
            intPackTypeRecord = dsPackType.Tables(0).Rows.Count

            dsDelTerm = Product_Ext.GetItemInfoToExcel("DT")
            intDelTermRecord = dsDelTerm.Tables(0).Rows.Count

            dsCommodity = Product_Ext.GetItemInfoToExcel("CM")
            intCommodityRecord = dsCommodity.Tables(0).Rows.Count

            ds = objRFq_ext.Download_RFQProductExcel_Common(ViewState("rfq_id"), Request.Form("rankCompanyId" & Request.Form("chkRank")), Session("ItemIndexList"))
            ds2 = objRFq_ext.Download_RFQProductExcel_Common2(ViewState("rfq_id"), Request.Form("rankCompanyId" & Request.Form("chkRank")), Session("ItemIndexList"))
            objEx.Writecell_Common(ds, ds2, dsPackType, dsDelTerm, dsCommodity, strDestPath & strFileName, True)
            objEx.ProtectWorkSheet_ItemUpload(strDestPath & strFileName, intPackTypeRecord, intDelTermRecord, intCommodityRecord)
            Filedownload(strFileName)
        End If

        'Else
        'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        'End If
    End Sub

    Function Filedownload(ByVal strFile As String)
        Response.ContentType = "application/octet-stream"
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strFile & """")
        Response.Flush()
        Response.WriteFile(strDestPath & strFile)
    End Function
End Class
