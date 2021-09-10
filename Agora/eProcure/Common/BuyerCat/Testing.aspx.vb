Imports AppCommon
Imports eProcure.Component
Partial Public Class Testing
    Inherits AppCommon.AppBaseClass
    Dim dDispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetGridProperty(dtgTest)

        If Not Page.IsPostBack Then
            Session("ColumnName") = Nothing
            Session("ColumnCode") = Nothing
            Session("ColumnDdl") = Nothing
            ConstructTable()
            'AddRow(5)
            ViewState("row") = 5
            populate()
        End If


    End Sub
    Sub populate()
        Dim typeahead As String
        Dim count As Integer
        count = ViewState("Count")
        Dim i As Integer
        Dim content As String
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")
        Dim codetypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")


        For i = 0 To count - 1
            content &= "$(""#dtgTest_ctl" & i & "_txtItemName"").autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
            "width: 260," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtItemName"").result(function(event, data, formatted) {" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtItemCode"").val(data[1]);" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtItemCode"").autocomplete(""" & codetypeahead & """, {" & vbCrLf & _
            "width: 260," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtItemCode"").result(function(event, data, formatted) {" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtItemName"").val(data[1]);" & vbCrLf & _
            "});" & vbCrLf
        Next
        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            content & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub
    Private Sub btnAddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRow.Click
        Dim i As Integer
        Dim count As Integer
        Dim aryname, arycode, aryddl As New ArrayList

        count = ViewState("Count")
        For i = 0 To count - 1
            aryname.Add(Request.Form("dtgTest_ctl" & i & "_txtItemName"))
            arycode.Add(Request.Form("dtgTest_ctl" & i & "_txtItemCode"))
            aryddl.Add(Request.Form("ddlCompany" & i))

        Next
        Session("ColumnName") = aryname
        Session("ColumnCode") = arycode
        Session("ColumnDdl") = aryddl
        ViewState("Count") += 1
        ConstructTable()
        populate()

    End Sub
    'Private Sub btnAddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRow.Click

    '    Dim item As DataGridItem
    '    Dim strary As New ArrayList

    '    For Each item In dtgTest.Items
    '        Dim txtname, txtcode, txtqty As TextBox
    '        Dim ddlCompany As DropDownList
    '        txtname = item.FindControl("txtItemName")
    '        txtcode = item.FindControl("txtItemCode")
    '        txtqty = item.FindControl("txt_qty")
    '        ddlCompany = item.FindControl("ddlCompany")
    '        strary.Add(txtname.Text)
    '        strary.Add(txtcode.Text)
    '        strary.Add(txtqty.Text)
    '        strary.Add(ddlCompany.SelectedIndex)
    '    Next
    '    Session("TempGrid") = strary

    '    'Dim dt As New DataTable
    '    'Dim row As DataRow
    '    'row = dt.NewRow()
    '    'dt.Rows.Add(row)
    '    'dtgTest.DataSource = dt
    '    'dtgTest.DataBind()
    '    'ViewState("dtg") = dt


    '    Dim row As Integer
    '    row = ViewState("row") + 1
    '    ViewState("row") = row
    '    populate()

    '    AddRow(row)


    'End Sub
    'Sub AddRow(ByVal intRow As Integer)
    '    'Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
    '    Dim dt As New DataTable
    '    Dim row As DataRow
    '    Dim i As Integer
    '    For i = 1 To intRow
    '        'dt.Columns.Add(New DataColumn("Desc", GetType(System.String)))
    '        row = dt.NewRow()
    '        'row.Item("txt_desc") = "abc"
    '        dt.Rows.Add(row)
    '        'row = dt.NewRow()
    '        'row.Item("txt_desc") = "ccc"
    '        'dt.Rows.Add(row)
    '        'For y = 1 To intCell
    '        '    addCell(row)
    '        'Next
    '        'txtTotal.ID = "Total"
    '        'txtTotal.Text = Format(30, "#,##0.00")
    '        'txtTotal.CssClass = "lblnumerictxtbox"
    '        'txtTotal.ReadOnly = True
    '        'txtTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
    '        'txtTotal.Font.Bold = True
    '        'row.Cells(2).Controls.Add(txtTotal)
    '        'row.Cells(1).Text = "abc"
    '        'row.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '        'row.Cells(1).Font.Bold = True
    '        'dtg_freeform.Controls(0).Controls.Add(row)
    '    Next
    '    'Dim item As DataGridItem
    '    'Dim strary As New ArrayList
    '    'Dim a As Integer
    '    'For Each item In dtgTest.Items
    '    '    Dim txtItemCode As TextBox
    '    '    Dim txtItemName As TextBox
    '    '    Dim txtqty As TextBox
    '    '    Dim ddlCompany As DropDownList
    '    '    ddlCompany = item.FindControl("ddlCompany")
    '    '    txtItemCode = item.FindControl("txtItemCode")
    '    '    txtItemName = item.FindControl("txtItemName")
    '    '    txtqty = item.FindControl("txtqty")


    '    '    If Not Session("TempGrid") Is Nothing Then
    '    '        strary = Session("TempGrid")
    '    '        For a = 0 To strary.Count Step -4
    '    '            txtItemCode.Text = strary(i)
    '    '        Next
    '    '        For a = 0 To strary.Count Step -3
    '    '            txtItemName.Text = strary(i)
    '    '        Next
    '    '        For a = 0 To strary.Count Step -2
    '    '            txtqty.Text = strary(i)
    '    '        Next
    '    '        For a = 0 To strary.Count Step -1
    '    '            ddlCompany.SelectedIndex = strary(i)
    '    '        Next
    '    '    End If
    '    'Next


    '    dtgTest.DataSource = dt
    '    dtgTest.DataBind()

    'End Sub
    'Private Sub dtgTest_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTest.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        Dim test As New Products
    '        Dim ddlCompany As New DropDownList
    '        'Dim lbl_uom As Label
    '        Dim ds As DataSet
    '        'lbl_uom = e.Item.FindControl("lbl_uom")
    '        ddlCompany = e.Item.FindControl("ddlCompany")

    '        ds = test.test()
    '        Common.FillDdl(ddlCompany, "CM_COY_ID", "CM_COY_NAME", ds)
    '    End If

    'End Sub



    Private Function ConstructTable()
        Dim strrow As String
        Dim i, count As Integer
        Dim table As String
        Dim arycode As New ArrayList
        Dim aryname As New ArrayList
        Dim aryddl As New ArrayList
        If ViewState("Count") Is Nothing Then
            ViewState("Count") = 5
            count = ViewState("Count")
        Else
            count = ViewState("Count")
        End If
        For i = 0 To count - 1
            If Session("ColumnCode") Is Nothing Then
                arycode.Add("")
            Else
                arycode = Session("ColumnCode")
                If i = count - 1 Then
                    arycode.Add("")
                End If
            End If
            If Session("ColumnName") Is Nothing Then
                aryname.Add("")
            Else
                aryname = Session("ColumnName")
                If i = count - 1 Then
                    aryname.Add("")
                End If
            End If
            If Session("ColumnDdl") Is Nothing Then
                aryddl.Add("")
            Else
                aryddl = Session("ColumnDdl")
                If i = count - 1 Then
                    aryddl.Add("")
                End If
            End If
        Next

        Dim test As New Products
        Dim a As Integer
        Dim ds As DataSet
        ds = test.test()

        For i = 0 To count - 1
            If ((i Mod 2) = 0) Then
                strrow &= "<tr style=""background-color:#fdfdfd;"">"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtItemCode"" name=""dtgTest_ctl" & i & "_txtItemCode"" value=""" & arycode(i) & """>"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtItemName"" name=""dtgTest_ctl" & i & "_txtItemName"" value=""" & aryname(i) & """>"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<select id=""ddlCompany"" name=""ddlCompany" & i & """>"
                For a = 0 To ds.Tables(0).Rows.Count - 1
                    If aryddl(i) = ds.Tables(0).Rows(a).Item(1).ToString Then
                        strrow &= "<option value=""" & ds.Tables(0).Rows(a).Item(1).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(a).Item(1).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & ds.Tables(0).Rows(a).Item(1).ToString & """>" & ds.Tables(0).Rows(a).Item(1).ToString & "</option>"
                    End If

                Next
                strrow &= "</select>"
                strrow &= "</td>"
                strrow &= "</tr>"
            Else
                strrow &= "<tr style=""background-color:#f5f9fc;"">"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtItemCode"" name=""dtgTest_ctl" & i & "_txtItemCode"" value=""" & arycode(i) & """>"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtItemName"" name=""dtgTest_ctl" & i & "_txtItemName"" value=""" & aryname(i) & """>"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<select id=""ddlCompany"" name=""ddlCompany" & i & """>"
                For a = 0 To ds.Tables(0).Rows.Count - 1
                    If aryddl(i) = ds.Tables(0).Rows(a).Item(1).ToString Then
                        strrow &= "<option value=""" & ds.Tables(0).Rows(a).Item(1).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(a).Item(1).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & ds.Tables(0).Rows(a).Item(1).ToString & """>" & ds.Tables(0).Rows(a).Item(1).ToString & "</option>"
                    End If

                Next
                strrow &= "</select>"
                strrow &= "</td>"
                strrow &= "</tr>"

            End If

        Next
        table = "<table border=""1"" class=""grid"" style=""margin-top:10px; width:100%; border-collapse:collapse; line-height:20px; "" >" & _
                                "<tr class=""GridHeader"" style=""font-weight:bold;""><td colspan=""5"">Comparison Summary > Generate PO for selected item(s) </td></tr>" & _
                                "<tr style=""height:1px;""><td colspan=""2"" style=""background-color:#fff;""></td></tr>" & _
                                "<tr class=""GridHeader"" style=""font-weight:bold;""><td width=""50%"">Item Code</td><td width=""50%"">Item Name</td></tr>" & _
                                strrow & _
                                "</table>"

        Session("ConstructTable") = table


    End Function
End Class