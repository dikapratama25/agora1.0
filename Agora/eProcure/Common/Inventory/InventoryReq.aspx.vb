Imports AgoraLegacy
Imports eProcure.Component


Public Class InventoryReq
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Submit As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button

    Protected WithEvents lblIRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptCode As System.Web.UI.WebControls.Label

    Protected WithEvents ddlSection As System.Web.UI.WebControls.DropDownList

    Protected WithEvents txtIssueTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRequestor As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtTemp As System.Web.UI.HtmlControls.HtmlInputText

    Protected WithEvents chkUrgent As System.Web.UI.WebControls.CheckBox

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objInv As New Inventory
    Dim objGlo As New AppGlobals
    Dim objDb As New EAD.DBCom
    Dim aryInvReq As New ArrayList()
    Dim aryInvHeader As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intNumeric2 As Boolean = True
    Dim strMsg As String = ""
    Dim LocDesc, SubLocDesc As String
    
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
        ''cmd_save.Enabled = False
        ''Dim alButtonList As ArrayList
        ''alButtonList = New ArrayList
        ''alButtonList.Add(cmd_save)
        ''htPageAccess.Add("add", alButtonList)
        ''htPageAccess.Add("update", alButtonList)
        ''CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        If Not Page.IsPostBack Then
            GenerateTab()
            BindSection()
            If Session("urlreferer") = "IRApprovalSetup" Then
                DisplayHeader()
            Else
                Session("aryInvReq") = Nothing
                Session("aryInvHeader") = Nothing
                txtIssueTo.Text = Session("UserName")
                lblDeptName.Text = objDb.GetVal(" SELECT CDM_DEPT_NAME FROM USER_MSTR INNER JOIN COMPANY_DEPT_MSTR ON UM_COY_ID = CDM_COY_ID AND UM_DEPT_ID = CDM_DEPT_CODE WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "'")
                lblDeptCode.Text = objDb.GetVal(" SELECT CDM_DEPT_CODE FROM USER_MSTR INNER JOIN COMPANY_DEPT_MSTR ON UM_COY_ID = CDM_COY_ID AND UM_DEPT_ID = CDM_DEPT_CODE WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "'")
            End If
            Session("blnLocSet") = objDb.GetVal("SELECT CM_LOCATION_STOCK FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'")
            lblIRNo.Text = "To Be Allocated By System"
            lblIssueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)
            txtRequestor.Text = Session("UserName")
            ViewState("Row") = 5
            BuildRow()
            ConstructTable()
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

        End If
        Populate()

    End Sub

    Private Function validateInputs() As Boolean

        If Me.txtRefNo.Text.Length > Me.txtRefNo.MaxLength Then
            strMsg = "<ul type='disc'><li>" & "Reference No should be less than " & Me.txtRemark.MaxLength & " characters." & "<ul type='disc'></ul></li></ul>"
            Return False
        End If

        If Me.txtRemark.Text.Length > Me.txtRemark.MaxLength Then
            strMsg = "<ul type='disc'><li>" & "IR Remark should be less than " & Me.txtRemark.MaxLength & " characters." & "<ul type='disc'></ul></li></ul>"
            Return False
        End If

        If Me.ddlSection.SelectedIndex = 0 Then
            strMsg = "<ul type='disc'><li>" & "Section is required." & "<ul type='disc'></ul></li></ul>"
            Return False
        End If

        If Me.lblDeptName.Text = "" Then
            strMsg = "<ul type='disc'><li>" & "Department is required." & "<ul type='disc'></ul></li></ul>"
            Return False
        End If

        Return True
    End Function

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        Rebuild()
        BuildLine()
        Rebuild()
        ConstructTable()
        Populate()

    End Sub

    Private Sub hidButton_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton.ServerClick
        Rebuild()
        ConstructTable()
    End Sub

    Private Sub hidButton1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton1.ServerClick
        Rebuild()
        ConstructTable()
    End Sub

    Private Function ConstructTable()
        Dim strrow As String
        Dim i, h, j, count As Integer
        Dim table, strAllLoc As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim aryInvReq_temp As New ArrayList
        Dim dblStkBal As Decimal
        Dim dblMonthStk, strUom As String
        Dim c As Integer
        Dim ds, dsSub, dsAll As DataSet

        aryInvReq = Session("aryInvReq")
        count = aryInvReq.Count

        For i = 0 To count - 1
            If aryInvReq(i)(0) <> "" Then
                If Session("blnLocSet") = "Y" Then
                    ds = objInv.PopLocationWItem(aryInvReq(i)(0))
                    If ds.Tables(0).Rows.Count = 0 Then
                        ds = objInv.PopLocation()
                    End If
                Else
                    dsAll = objInv.getAvailableLoc(aryInvReq(i)(0))
                End If

                strUom = objInv.getInvItemUom(aryInvReq(i)(0))
                dblStkBal = objInv.getStockBalance(aryInvReq(i)(0))
                dblMonthStk = objInv.getMonthStockBalance(aryInvReq(i)(0), Session("UserId"))
                aryInvReq(i)(6) = Format(dblStkBal, "##0.00")
            Else
                If Session("blnLocSet") = "Y" Then
                    ds = objInv.PopLocation()
                End If

                strUom = ""
                dblMonthStk = ""
                aryInvReq(i)(6) = ""
            End If

            If Session("blnLocSet") = "Y" Then
                dsSub = objInv.PopLocation("")
            End If

            'If aryInvReq(i)(3) <> Request.QueryString("item") Then
            '    Continue For
            'End If
            'If ((i Mod 2) = 0) Then
            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%;margin-right:0px; ""  class=""txtbox2"" type=""text"" id=""txtItem" & i & """ name=""txtItem" & i & """ value=""" & aryInvReq(i)(0) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<input style=""width:100%;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtName" & i & """ name=""txtName" & i & """ value=""" & aryInvReq(i)(1) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<label id=""lblUom" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox2"" >" & strUom & "</label>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<label id=""lblStkBal" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"">" & aryInvReq(i)(6) & "</label>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<label id=""lblMthStock" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & dblMonthStk & "</label>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<input style=""width:100%;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" class=""numerictxtbox2"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & aryInvReq(i)(2) & """>"
            strrow &= "</td>"


            If Session("blnLocSet") = "Y" Then
                strrow &= "<td>"
                strrow &= "<select class=""ddl2"" style=""width:100%;margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

                For c = 0 To ds.Tables(0).Rows.Count - 1
                    If aryInvReq(i)(3) = ds.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        dsSub = objInv.PopLocationWItem(aryInvReq(i)(0), ds.Tables(0).Rows(c).Item(0).ToString)
                    Else
                        strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """>" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next

                strrow &= "</select>"
                strrow &= "</td>"


                strrow &= "<td>"
                strrow &= "<select class=""ddl2"" style=""width:100%;margin-right:0px;"" id=""cboSubLocation" & i & """ name=""cboSubLocation" & i & """>"

                For c = 0 To dsSub.Tables(0).Rows.Count - 1
                    If aryInvReq(i)(4) = dsSub.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """>" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If

                Next
                If dsSub.Tables(0).Rows.Count = 0 Then
                    strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
                End If

                strrow &= "</select>"
                strrow &= "</td>"
            Else
                If aryInvReq(i)(0) <> "" Then
                    If dsAll.Tables(0).Rows.Count > 0 Then
                        strAllLoc = ""

                        For c = 0 To dsAll.Tables(0).Rows.Count - 1
                            If strAllLoc = "" Then
                                strAllLoc = dsAll.Tables(0).Rows(c)("LM_LOCATION") & ":" & dsAll.Tables(0).Rows(c)("LM_SUB_LOCATION")
                            Else
                                strAllLoc &= ", " & dsAll.Tables(0).Rows(c)("LM_LOCATION") & ":" & dsAll.Tables(0).Rows(c)("LM_SUB_LOCATION")
                            End If
                        Next
                    Else
                        strAllLoc = ""
                    End If
                Else
                    strAllLoc = ""
                End If

                strrow &= "<td>"
                strrow &= "<label id=""lblAllLoc" & i & """ style=""width:100%;"">" & strAllLoc & "</label>"
                strrow &= "</td>"
            End If

            strrow &= "</tr>"

            'Else
            '    strrow &= "<tr style=""background-color:#f5f9fc;"">"
            'End If

        Next

        If Session("blnLocSet") = "Y" Then
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                   "<tr class=""TableHeader""><td style=""width:14%;margin-right:0px;"">Item Code</td><td style=""width:16%; margin-right:0px;"">Item Name</td><td style=""width:10%; text-align:left;"">UOM</td><td style=""width:10%; text-align:right;"">Stock Balance</td><td style=""width:12%; text-align:right;"">Monthly Stock Issued Accumulative</td><td style=""width:8%; text-align:right;""><label id=""lblQty"">Qty</label></td><td style=""width:20%;"">" & LocDesc & "</td><td style=""width:20%"">" & SubLocDesc & "</td></tr>" & _
                   strrow & _
                   "</table>"
        Else
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                   "<tr class=""TableHeader""><td style=""width:14%;margin-right:0px;"">Item Code</td><td style=""width:16%; margin-right:0px;"">Item Name</td><td style=""width:10%; text-align:left;"">UOM</td><td style=""width:10%; text-align:right;"">Stock Balance</td><td style=""width:12%; text-align:right;"">Monthly Stock Issued Accumulative</td><td style=""width:8%; text-align:right;""><label id=""lblQty"">Qty</label></td><td style=""width:40%;"">" & LocDesc & " : " & SubLocDesc & "</td></tr>" & _
                   strrow & _
                   "</table>"
        End If
  

        Session("ConstructTable") = table

        'cmd_Search.Attributes.Add("onclick", "Search(); ")

    End Function

    Private Function Rebuild()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True
        intNumeric2 = True

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1

            Dim strItem, strItemName As String
            If RTrim(LTrim(Request.Form("txtItem" & i))) = "" Then
                strItem = ""
            Else
                strItem = LTrim(Request.Form("txtItem" & i))
            End If

            If RTrim(LTrim(Request.Form("txtName" & i))) = "" Then
                strItemName = ""
            Else
                strItemName = LTrim(Request.Form("txtName" & i))
            End If

            aryInvReq(i)(0) = strItem
            aryInvReq(i)(1) = strItemName
            aryInvReq(i)(2) = Request.Form("txtQty" & i)
            If Session("blnLocSet") = "Y" Then
                aryInvReq(i)(3) = Request.Form("cboLocation" & i)
                aryInvReq(i)(4) = Request.Form("cboSubLocation" & i)
            Else
                aryInvReq(i)(3) = ""
                aryInvReq(i)(4) = ""
            End If


            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If

            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = True And Request.Form("txtQty" & i) <> "" And aryInvReq(i)(6) <> "" Then
                If CDec(Request.Form("txtQty" & i)) > CDec(aryInvReq(i)(6)) Then
                    intNumeric2 = False
                End If
            End If
        Next

        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function Rebuild2()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            aryInvReq(i)(0) = Request.Form("txtItem" & i)
            aryInvReq(i)(1) = Request.Form("txtName" & i)
            aryInvReq(i)(2) = Request.Form("txtQty" & i)
            If Session("blnLocSet") = "Y" Then
                aryInvReq(i)(3) = Request.Form("cboLocation" & i)
                aryInvReq(i)(4) = Request.Form("cboSubLocation" & i)
            Else
                aryInvReq(i)(3) = ""
                aryInvReq(i)(4) = ""
            End If
            

            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If
        Next

        i = 5
        Do While i < aryInvReq.Count
            If aryInvReq(i)(0) = "" Then
                aryInvReq.RemoveAt(i)
                i = 5
            Else
                i = i + 1
            End If
        Loop

        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        aryInvReq = Session("aryInvReq")
        If Session("blnLocSet") = "Y" Then
            aryInvReq.Add(New String() {"", "", "", "---Select---", "---Select---", "", ""})
        Else
            aryInvReq.Add(New String() {"", "", "", "", "", "", ""})
        End If

        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        Dim aryTempInv As New ArrayList()
        found = False

        If ViewState("Saved") = "Saved" Then
            Exit Function
        End If

        If Session("aryInvReq") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                If Session("blnLocSet") = "Y" Then
                    aryInvReq.Add(New String() {Request.Form("txtItem" & i), Request.Form("txtName" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), "", ""})
                Else
                    aryInvReq.Add(New String() {Request.Form("txtItem" & i), Request.Form("txtName" & i), Request.Form("txtQty" & i), "", "", "", ""})
                End If

            Next
            Session("aryInvReq") = aryInvReq
        Else
            aryTempInv = Session("aryInvReq")

            For i = 0 To aryTempInv.Count - 1
                aryInvReq.Add(New String() {aryTempInv(i)(0), aryTempInv(i)(1), aryTempInv(i)(2), "", "", "", ""})
            Next
            Session("aryInvReq") = Nothing
            Session("aryInvReq") = aryInvReq
        End If

    End Function

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        ConstructTable()
    End Sub

    Sub Populate()
        Dim typeahead As String
        Dim i As Integer
        Dim content As String
        Dim nametypeahead As String
        Dim codetypeahead As String

        If Session("blnLocSet") = "Y" Then
            nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")
            codetypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")
        Else
            nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name2")
            codetypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code2")
        End If

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            content &= "$(""#txtName" & i & """).autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
            "width: 355," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtName" & i & """).result(function(event, data, formatted) {" & vbCrLf & _
            "$(""#txtTemp"").focus();" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#txtItem" & i & """).val(data[1]);" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtItem" & i & """).autocomplete(""" & codetypeahead & """, {" & vbCrLf & _
            "width: 150," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtItem" & i & """).result(function(event, data, formatted) {" & vbCrLf & _
            "$(""#txtTemp"").focus();" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#txtName" & i & """).val(data[1]);" & vbCrLf & _
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

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"


    End Sub

    Private Sub cmd_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Submit.Click
        Dim ds As New DataSet
        Dim strNewIR, intMsg, Code As String
        Dim iCheck As Boolean = False
        Dim iCheckQty As Boolean = False
        Dim iCheckLoc As Boolean = False
        Dim iCheckQtyZero As Boolean = False
        Dim i, j As Integer
        Dim decTotal As Decimal
        Dim RLoc, RSLoc As String

        Rebuild()
        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            If aryInvReq(i)(0) <> "" Then
                iCheck = True
                If aryInvReq(i)(0) <> "" And (aryInvReq(i)(2) = "") Then
                    iCheckQty = True
                    Exit For
                    'ElseIf aryInvReq(i)(0) <> "" And (CDec(IIf(aryInvReq(i)(2) = "", 0, aryInvReq(i)(2))) = 0) Then
                    '    iCheckQtyZero = True
                    '    Exit For
                Else
                    If Session("blnLocSet") = "Y" Then
                        If aryInvReq(i)(0) <> "" And aryInvReq(i)(2) <> "" And aryInvReq(i)(3) = "---Select---" Then
                            iCheckLoc = True
                            Exit For
                        End If
                    End If
                End If
            End If
        Next

        If iCheckQty = False And Session("blnLocSet") = "N" Then
            For i = 0 To aryInvReq.Count - 1
                If aryInvReq(i)(0) <> "" Then
                    decTotal = 0
                    For j = 0 To aryInvReq.Count - 1
                        If aryInvReq(i)(0) = aryInvReq(j)(0) And i <> j Then
                            If decTotal = 0 Then
                                decTotal = CDec(aryInvReq(i)(2)) + CDec(aryInvReq(j)(2))
                            Else
                                decTotal = decTotal + CDec(aryInvReq(j)(2))
                            End If
                        End If
                    Next

                    If decTotal > CDec(aryInvReq(i)(6)) Then
                        intNumeric2 = False
                        Exit For
                    End If
                End If
            Next
        End If

        If iCheck = False Then
            strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00020") & "<ul type='disc'></ul></li></ul>"
        Else
            If iCheckQty = True Then
                strMsg = "<ul type='disc'><li>" & "" & objGlo.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
            ElseIf iCheckQtyZero = True Then
                strMsg = "<ul type='disc'><li>" & "" & objGlo.GetErrorMessage("00342") & "" & "<ul type='disc'></ul></li></ul>"
            Else
                If iCheckLoc = True Then
                    strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00014") & "<ul type='disc'></ul></li></ul>"
                End If
            End If
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "" & " " & objGlo.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric2 = False Then
            strMsg = "<ul type='disc'><li>" & "" & " " & "Quantity must be lesser than or equal to Stock Balance." & "<ul type='disc'></ul></li></ul>"
        End If

        If txtIssueTo.Text = "" Then
            strMsg = "<ul type='disc'><li>" & lblIssueTo.Text & " " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If

        Call validateInputs()

        If strMsg <> "" Then
            lblMsg.Text = strMsg
            ConstructTable()
            Exit Sub
        Else
            lblMsg.Text = ""
        End If

        aryInvReq = Session("aryInvReq")
        If Not IsNumeric(Mid(lblIRNo.Text, 3)) Then
            Rebuild()
            ConstructTable()

            'If Session("blnLocSet") = "Y" Then
            '    intMsg = objInv.insertIR(aryInvReq, strNewIR, txtIssueTo.Text, txtRefNo.Text, txtRemark.Text, Code, txtdepartment.Text, RLoc, RSLoc, "Y")
            'Else
            ds = objInv.getIRApprFlow()
            If ds.Tables(0).Rows.Count = 0 Then
                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                Exit Sub
            ElseIf ds.Tables(0).Rows.Count > 1 Then
                aryInvHeader.Add(IIf(chkUrgent.Checked = True, "Y", "N"))
                aryInvHeader.Add(txtRequestor.Text)
                aryInvHeader.Add(ddlSection.SelectedValue)
                aryInvHeader.Add(txtIssueTo.Text)
                aryInvHeader.Add(lblDeptCode.Text)
                aryInvHeader.Add(txtRefNo.Text)
                aryInvHeader.Add(txtRemark.Text)
                Session("aryInvHeader") = aryInvHeader
                Session("aryInvReq") = aryInvReq
                'Session("UrlLocation") = "InventoryReq"
                Response.Redirect(dDispatcher.direct("Inventory", "IRApprovalSetup.aspx", "pageid=" & strPageId))
                Exit Sub
            Else
                If Session("blnLocSet") = "Y" Then
                    intMsg = objInv.insertIR(aryInvReq, strNewIR, txtIssueTo.Text, txtRefNo.Text, txtRemark.Text, Code, lblDeptCode.Text, RLoc, RSLoc, True, "Y", txtRequestor.Text, ddlSection.SelectedValue, IIf(chkUrgent.Checked = True, "Y", "N"), ds.Tables(0).Rows(0)("AGM_GRP_INDEX"))
                Else
                    intMsg = objInv.insertIR(aryInvReq, strNewIR, txtIssueTo.Text, txtRefNo.Text, txtRemark.Text, Code, lblDeptCode.Text, RLoc, RSLoc, True, "N", txtRequestor.Text, ddlSection.SelectedValue, IIf(chkUrgent.Checked = True, "Y", "N"), ds.Tables(0).Rows(0)("AGM_GRP_INDEX"))
                End If
            End If

            Rebuild2()
            ConstructTable()

        End If
        Select Case intMsg
            Case WheelMsgNum.Save
                lblIRNo.Text = strNewIR
                'cmd_Submit.Enabled = False
                'cmd_Add.Enabled = False
                'cmd_Search.Enabled = False
                cmd_Submit.Visible = False
                cmd_Add.Visible = False
                cmd_Search.Visible = False
                'If Session("blnLocSet") = "Y" Then
                '    Common.NetMsgbox(Me, "IR Number " & strNewIR & " " & objGlo.GetErrorMessage("00024"), MsgBoxStyle.Information)
                '    cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIRSlip.aspx", "IRNo=" & Trim(lblIRNo.Text) & "&CoyID=" & Session("CompanyID") & "") & "')")
                '    cmdPrint.Visible = True
                'Else
                Common.NetMsgbox(Me, "IR Number " & strNewIR & " " & objGlo.GetErrorMessage("00024"), dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId))
                'End If

            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)

            Case 10
                If RSLoc <> "---Select---" And RSLoc <> "" Then
                    Common.NetMsgbox(Me, Code & " From " & RLoc & " & " & RSLoc & " " & objGlo.GetErrorMessage("00022"), MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, Code & " From " & RLoc & " " & objGlo.GetErrorMessage("00022"), MsgBoxStyle.Information)
                End If


        End Select
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Search", "InventoryItemSearchPopup.aspx", strName)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Sub BindSection()
        Dim cbolist As New ListItem
        Dim objAdmin As New Admin
        Dim dvCustom As DataView

        dvCustom = objInv.getSection()

        ddlSection.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(ddlSection, "SEC_DESC", "CS_SEC_CODE", dvCustom)
        End If

        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddlSection.Items.Insert(0, cbolist)

    End Sub

    Sub DisplayHeader()
        Dim aryTemp As New ArrayList()
        Dim strNewIR, strIssueTo, strRefNo, strRemark, strDept, strUrgent, strRequestor, strSection As String

        aryTemp = Session("aryInvHeader")
        strUrgent = aryTemp(0)
        strRequestor = aryTemp(1)
        strSection = aryTemp(2)
        strIssueTo = aryTemp(3)
        strDept = aryTemp(4)
        strRefNo = aryTemp(5)
        strRemark = aryTemp(6)

        If strUrgent = "N" Then
            chkUrgent.Checked = False
        Else
            chkUrgent.Checked = True
        End If

        txtIssueTo.Text = strIssueTo 'Issue To
        ddlSection.SelectedValue = strSection 'Section
        lblDeptCode.Text = strDept 'Department
        lblDeptName.Text = objDb.GetVal("SELECT CDM_DEPT_NAME FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID = '" & Session("CompanyId") & "' AND CDM_DEPT_CODE = '" & Common.Parse(strDept) & "'")
        txtRefNo.Text = strRefNo 'Ref No.
        txtRemark.Text = strRemark 'Remark

        Session("urlreferer") = Nothing
        Session("aryInvHeader") = Nothing
    End Sub
End Class
