Imports eProcure.Component
Imports AgoraLegacy


Public Class CostAllocDetail
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryCAD As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim objDB As New EAD.DBCom
    Dim intEmpty As Boolean = True
    Dim iTotal As Integer
    Dim objGlobal As New AppGlobals
    Dim strMsg As String
    Public Enum EnumCLD
        icChk = 0
        icCode = 1
        icDesc = 2
        icCoyName = 3
        icStartDate = 4
        icEndDate = 5
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtAllocated As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents lblTtlCADPercent As Global.System.Web.UI.WebControls.Label
    Protected WithEvents tblAddNewCAD As Global.System.Web.UI.WebControls.Table

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'MyBase.Page_Load(sender, e).



        Dim objCAD As New IPP
        Dim CACode As String ' CA index
        Dim CACode2 As String ' CA Code
        Dim SelectedCACode As String
        Dim objdb As New EAD.DBCom

        'SetGridProperty(dtgCostAllocDetail)
        Session("ttlPerc") = Nothing
        Session("ttlPerc2") = Nothing

        GenerateTab()
        If Not IsPostBack Then
            'release the session
            Session("NewdsCADRow") = Nothing
            CACode = objCAD.GetLastCACode
            CACode2 = objdb.GetVal("SELECT cam_ca_code FROM cost_alloc_mstr WHERE cam_index =  '" & CACode & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
            ddlCostAllocCode.SelectedValue = CACode
        Else
            If ddlCostAllocCode.SelectedValue = "" Then '"---Select---" Then
                CACode = ""

            Else
                CACode = ddlCostAllocCode.SelectedValue
                CACode2 = objdb.GetVal("SELECT cam_ca_code FROM cost_alloc_mstr WHERE cam_index =  '" & CACode & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
            End If

        End If
        Session("CACode") = CACode
        Session("CACode2") = CACode2
        If objCAD.CheckCADetail(CACode) = False Then


            ViewState("haveCADetail") = False
            Session("Action") = "New"
            Session("aryCAD") = Nothing
            DisplayBlankTable()


        Else
            ViewState("haveCADetail") = True

            Session("Action") = "Modify"

            ddlCostAllocCode.SelectedValue = CACode

            FillCostCentreCode()
            'nRebuild()
            ConstructTable()
            'nPopulate()
            'Bindgrid()            
            cmdSave.Visible = True
            cmdAddLine.Visible = True
            cmdDelete.Visible = True
            cmdNewSave.Visible = False
            cmdNewAddLine.Visible = False
            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")


        End If

    End Sub
    Private Function DisplayBlankTable()
        ViewState("Row") = 10

        FillCostCentreCode()

        BuildRow()
        ConstructTable()
        'Populate()
        cmdSave.Visible = False
        cmdAddLine.Visible = False
        cmdDelete.Visible = False
        If Session("CACode") = "" Then
            cmdNewSave.Visible = False
            cmdNewAddLine.Visible = False
        Else
            cmdNewSave.Visible = True
            cmdNewAddLine.Visible = True
        End If

        ddlCostAllocCode.SelectedValue = Session("CACode")
    End Function




    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCA As New IPP
        Dim chkItem As CheckBox
        Dim dsCA, ds As New DataSet
        Dim dtCA As New DataTable
        Dim i, CADRowCount As Integer
        Dim strCCCode, strBRCode As String

        dtCA.Columns.Add("CADIDX", Type.GetType("System.String"))
        dtCA.Columns.Add("CADCCCode", Type.GetType("System.String"))
        dtCA.Columns.Add("CADBRCode", Type.GetType("System.String"))
        'dtCA.Columns.Add("UsrId", Type.GetType("System.String"))

        'dsCA = Session("dsCAD")
        CADRowCount = Session("CADRowCount")
        For i = 0 To CADRowCount - 1
            If Request.Form("chkSelection" & i & "") = "on" Then
                CADRowCount = CADRowCount - 1
                Dim dtr As DataRow
                dtr = dtCA.NewRow()
                strCCCode = Request.Form("txtCC" & i).Substring(0, InStr(Request.Form("txtCC" & i), " : "))
                strBRCode = Request.Form("txtBranch" & i).Substring(0, InStr(Request.Form("txtBranch" & i), " : "))

                dtr("CADIDX") = ddlCostAllocCode.SelectedValue
                dtr("CADCCCode") = strCCCode.Trim
                dtr("CADBrCode") = strBRCode.Trim

                dtCA.Rows.Add(dtr)



            End If
        Next

        If objCA.DelCostAllocDetail(dtCA, Session("CACode2")) Then
            objCA.Message(Me, "00004", MsgBoxStyle.Information)
        Else
            objCA.Message(Me, "00018", MsgBoxStyle.Information)
        End If

        ds = objCA.GetCostAllocDetail2(Session("CACode"))
        Session("dsCAD") = ds
        Session("CADRowCount") = ds.Tables(0).Rows.Count 'CADRowCount
        'Rebuild2()
        Session("aryCAD") = Nothing

        ConstructTable()
        'Populate()


        'ViewState("action") = "del"
        If objCA.CheckCADetail(Session("CACode")) = False Then

            Session("Action") = "New"
            ddlCostAllocCode.SelectedValue = Session("CACode")

            DisplayBlankTable()

        Else
            Session("Action") = "Modify"
            ConstructTable()
            cmdSave.Visible = True
            cmdAddLine.Visible = True
            cmdDelete.Visible = True
            cmdNewSave.Visible = False
            cmdNewAddLine.Visible = False
        End If

        'Session("dsCAD") = Nothing
    End Sub
    Sub FillCostCentreCode()
        Dim objCAD As New IPP

        objCAD.FillCACode(ddlCostAllocCode, ConfigurationManager.AppSettings("eProcurePath"))

    End Sub
    Sub Populate()

        Dim typeahead As String
        Dim i As Integer
        Dim content As String
        Dim brtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Branch")
        Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre")


        aryCAD = Session("aryCAD")
        For i = 0 To aryCAD.Count - 1
            content &= "$(""#txtBranch" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf &
            "width: 355," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtBranch" & i & """).result(function(event, data, formatted) {" & vbCrLf &
            "$(""#txtTemp"").focus();" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).autocomplete(""" & cctypeahead & """, {" & vbCrLf &
            "width: 150," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).result(function(event, data, formatted) {" & vbCrLf &
            "$(""#txtTemp"").focus();" & vbCrLf &
            "});" & vbCrLf
        Next
        typeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            content & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        Session("typeahead") = typeahead

    End Sub
    Sub nPopulate()

        Dim typeahead As String
        Dim i As Integer
        Dim content As String
        Dim CADRowCount As Integer
        Dim brtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Branch")
        Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre")


        CADRowCount = Session("CADRowCount")
        For i = 0 To CADRowCount - 1
            content &= "$(""#txtBranch" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf &
            "width: 355," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtBranch" & i & """).result(function(event, data, formatted) {" & vbCrLf &
            "$(""#txtTemp"").focus();" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).autocomplete(""" & cctypeahead & """, {" & vbCrLf &
            "width: 150," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).result(function(event, data, formatted) {" & vbCrLf &
            "$(""#txtTemp"").focus();" & vbCrLf &
            "});" & vbCrLf
        Next
        typeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            content & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        Session("typeahead") = typeahead

    End Sub
    Private Function SaveCADetail()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dgItem As DataGridItem
        Dim i, CADRowCount As Integer
        Dim txtPercent As TextBox
        Dim txtCCCode As Label
        Dim objDB As New EAD.DBCom
        Dim dsCADetail As New DataSet
        Dim objCAD As New IPP
        Dim strCCCode, strBrCode As String
        Dim ds As New DataSet
        Dim count As Integer


        dt.Columns.Add("CAMIDX", Type.GetType("System.Int32"))
        dt.Columns.Add("CostCenterCode", Type.GetType("System.String"))
        dt.Columns.Add("BranchCode", Type.GetType("System.String"))
        dt.Columns.Add("Percent", Type.GetType("System.Decimal"))
      



        If Session("NewdsCADRow") = 0 Then
            ds = Session("dsCAD")
            CADRowCount = ds.Tables(0).Rows.Count

        Else
            ds = Session("dsCAD")
            CADRowCount = ds.Tables(0).Rows.Count

        End If

        For i = 0 To CADRowCount - 1
            'If InStr(Request.Form("txtCC" & i), " : ") = 0 And InStr(Request.Form("txtBranch" & i), " : ") = 0 Then

            '    strCCCode = Request.Form("txtCC" & i)
            '    strBrCode = Request.Form("txtBranch" & i)
            'Else
            '    strCCCode = Request.Form("txtCC" & i).Substring(0, InStr(Request.Form("txtCC" & i), " : "))
            '    strBrCode = Request.Form("txtBranch" & i).Substring(0, InStr(Request.Form("txtBranch" & i), " : "))
            'End If
            If InStr(Request.Form("txtBranch" & i), " : ") = 0 Then
                strBrCode = Request.Form("txtBranch" & i)
            Else
                strBrCode = Request.Form("txtBranch" & i).Substring(0, InStr(Request.Form("txtBranch" & i), " : "))
            End If

            If InStr(Request.Form("txtCC" & i), " : ") = 0 Then
                strCCCode = Request.Form("txtCC" & i)
            Else
                strCCCode = Request.Form("txtCC" & i).Substring(0, InStr(Request.Form("txtCC" & i), " : "))
            End If

            dr = dt.NewRow
            dr("CAMIDX") = ddlCostAllocCode.SelectedValue
            dr("CostCenterCode") = strCCCode.Trim
            dr("BranchCode") = strBrCode.Trim
            dr("Percent") = Request.Form("txtPercent" & i)


            dt.Rows.Add(dr)

        Next
        dsCADetail.Tables.Add(dt)
        nRebuild()
        If validateInput() Then
            If validate(aryCAD.Count, "save", dsCADetail) = False Then
                strMsg = objGlobal.GetErrorMessage("00002")
                strMsg = strMsg & "(HO/BR Code / Cost Centre Code)"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                ConstructTable()
                Exit Function
            End If
            If Session("ttlPerc2") > 100 Then
                strMsg = objGlobal.GetErrorMessage("00015")
                strMsg = "Amount " & strMsg & "100%."
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Session("NewdsCADRow") = Nothing

                'Exit Function
            Else
                If objCAD.UpdateCostAllocDetail(dsCADetail, Session("CACode2")) Then
                    objCAD.Message(Me, "00003", MsgBoxStyle.Information)

                Else
                    objCAD.Message(Me, "00018", MsgBoxStyle.Information)
                End If

                Session("NewdsCADRow") = Nothing

            End If
        End If

        Session("Action") = "Modify"
        ConstructTable()
    End Function


    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("CostAlloc_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "CostAllocMaint.aspx", "pageid=" & strPageId) & """><span>Cost Alloc. Code</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "CostAllocDetail.aspx", "pageid=" & strPageId) & """><span>Cost Alloc. Details</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

    End Sub



    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Call SaveCADetail()
    End Sub

    Private Sub ddlCostAllocCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCostAllocCode.SelectedIndexChanged
        Session("NewdsCADRow") = Nothing
        Session("dsCAD") = Nothing
        If ddlCostAllocCode.SelectedIndex <> 0 Then
            If Session("Action") = "Modify" Then
                ConstructTable()
                'nPopulate()
            Else
                ConstructTable()
                'Populate()
            End If
        Else
            Session("ConstructTable") = ""
            Session("aryCAD") = Nothing
        End If


    End Sub

    Private Sub cmdAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddLine.Click
        Session("Action") = "Modify"
        'nRebuild()
        nBuildLine()
        nRebuild()
        ConstructTable()
        'nPopulate()


    End Sub
    Private Function ConstructTable()
        Dim strrow As String
        Dim i, h, j, count As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim aryCAD_temp As New ArrayList
        Dim strCADCC, strCADBR As String
        Dim c As Integer
        Dim ds, dsSub As DataSet
        Dim objCAD As New IPP


        If Session("Action") = "New" Then

            If Session("aryCAD") IsNot Nothing Then
                ' count = 0
              
                aryCAD = Session("aryCAD")
                count = aryCAD.Count
                cmdNewSave.Attributes.Add("onclick", "return validatedec(" & count & ");")



                strrow &= "<span class=""tablecol"" style=""line-height:20px;width:100%;""><span><strong>Total % Allocated: <span id=""total"" style = ""text-align:left"" >" & Session("ttlPerc") & "</span></strong></span></span>"

                For i = 0 To count - 1


                    strrow &= "<tr style=""background-color:#fdfdfd;"">"

                    strrow &= "<td >"
                    strrow &= "<input style=""width:150px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryCAD(i)(0) & """>"
                    strrow &= "</td>"

                    strrow &= "<td >"
                    strrow &= "<input style=""width:150px;margin-right:0px; ""  class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryCAD(i)(1) & """>"
                    strrow &= "</td>"

                    strrow &= "<td >"
                    strrow &= "<input style=""width:70px;margin-right:0px; "" onkeypress=""return isNumberKey(event);"" onblur=""return TtlPercent(" & count & ")"" class=""numerictxtbox2"" type=""text"" id=""txtPercent" & i & """ name=""txtPercent" & i & """ value=""" & aryCAD(i)(2) & """ MaxLength=""5"">"
                    strrow &= "</td>"

                    strrow &= "</td>"
                    strrow &= "</tr>"



                Next

                table = "<table class=""gridnowidth"" style=""margin-top:20px; border-collapse:collapse;  line-height:20px;"" id=""tblAddNewCAD"">" & _
                   "<tr class=""TableHeader""><td style=""width:150px; margin-right:0px;"">HO/BR Code</td><td style=""width:150px;margin-right:0px;"">Cost Centre Code</td><td style=""width:70px; text-align:right;""><label id=""lblPercent"">% Allocated</label></td></tr>" & _
                   strrow & _
                   "</table>"
            End If

        ElseIf Session("Action") = "Modify" Then


            ds = objCAD.GetCostAllocDetail2(Session("CACode"))
            If Session("NewdsCADRow") = 0 Then
                count = ds.Tables(0).Rows.Count
                Session("dsCAD") = ds
                Session("CADRowCount") = count
            Else
                ds = Session("dsCAD")
                count = ds.Tables(0).Rows.Count
                Session("CADRowCount") = count
            End If

            Dim ttlPerc2 As Double
            For i = 0 To count - 1
                ttlPerc2 += Val(ds.Tables(0).Rows(i)("CAD_PERCENT"))

            Next
            Session("ttlPerc2") = ttlPerc2




            strrow &= "<span class=""tablecol"" style=""line-height:20px;width:100%;""><span><strong>Total % Allocated: <span id=""total"" style = ""text-align:left"" >" & Session("ttlPerc2") & "</span></strong></span></span>"

            For i = 0 To count - 1

                'If Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC")) = "" Or ds.Tables(0).Rows(i)("CC_CC_DESC") Is System.DBNull.Value Then

                '    strCADCC = Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC"))

                '    If (Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME")) = "" Or ds.Tables(0).Rows(i)("CBM_BRANCH_NAME") Is System.DBNull.Value) Then 'Or InStr(Request.Form("txtBranch" & i), " : ") = 0 Then
                '        strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '    ElseIf Common.parseNull(ds.Tables(0).Rows(i)("CAD_BRANCH_CODE")) = "" Or ds.Tables(0).Rows(i)("CAD_BRANCH_CODE") Is System.DBNull.Value Then
                '        strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '    Else
                '        strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CAD_BRANCH_CODE")) & " : " & Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME")) 'Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '        'strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '    End If

                'Else
                '    If Session("NewdsCADRow") = 0 Then

                '        strCADCC = Common.parseNull(ds.Tables(0).Rows(i)("CAD_CC_CODE")) & " : " & Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC"))
                '        strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CAD_BRANCH_CODE")) & " : " & Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '    Else
                '        strCADCC = Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC"))
                '        strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                '    End If

                'End If

                If Session("NewdsCADRow") = 0 Then
                    If Common.parseNull(ds.Tables(0).Rows(i)("CAD_CC_CODE")) <> "" And Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC")) <> "" Then
                        strCADCC = Common.parseNull(ds.Tables(0).Rows(i)("CAD_CC_CODE")) & " : " & Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC"))
                    Else
                        strCADCC = ""
                    End If

                    strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CAD_BRANCH_CODE")) & " : " & Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                Else
                    strCADCC = Common.parseNull(ds.Tables(0).Rows(i)("CC_CC_DESC"))
                    strCADBR = Common.parseNull(ds.Tables(0).Rows(i)("CBM_BRANCH_NAME"))
                End If

                strrow &= "<tr style=""background-color:#fdfdfd;"">"
                strrow &= "<td align=""center"">"
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                strrow &= "</td>"

                If ds.Tables(0).Rows(i)("CAD_CC_CODE") = "" Or ds.Tables(0).Rows(i)("CAD_CC_CODE") Is System.DBNull.Value Then 'If Session("NewdsCADRow") = 0 Then
                    strrow &= "<td >"
                    strrow &= "<input style=""width:100%;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & strCADBR & """>"
                    strrow &= "</td>"

                    strrow &= "<td >"
                    strrow &= "<input style=""width:100%;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & strCADCC & """>"
                    strrow &= "</td>"

                Else
                    strrow &= "<td >"
                    strrow &= "<input style=""width:100%;margin-right:0px; "" class=""txtbox2"" type=""text"" ReadOnly=""true"" id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & strCADBR & """>"
                    strrow &= "</td>"

                    strrow &= "<td >"
                    strrow &= "<input style=""width:100%;margin-right:0px; ""  class=""txtbox2""  type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & strCADCC & """>"
                    strrow &= "</td>"
                End If



                strrow &= "<td >"
                strrow &= "<input style=""width:70px;margin-right:0px;""  onblur=""return TtlPercent(" & count & ")"" class=""numerictxtbox2"" type=""text"" id=""txtPercent" & i & """ name=""txtPercent" & i & """ value=""" & ds.Tables(0).Rows(i)("CAD_PERCENT") & """ MaxLength=""5"">"
                strrow &= "</td>"


                strrow &= "</td>"
                strrow &= "</tr>"


            Next

            table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse;  line-height:20px;"" id=""tblAddNewCAD"">" & _
               "<tr class=""TableHeader""><td width=""5%"" align=""center""><input type=""checkbox"" id=""chkAll"" name=""chkAll"" onclick=""selectAll();""></td><td style=""width:55%; margin-right:0px;"">HO/BR Code</td><td style=""width:40%;margin-right:0px;"">Cost Centre Code</td><td style=""width:10%; text-align:right;""><label id=""lblPercent"">% Allocated</label></td></tr>" & _
               strrow & _
               "</table>"
        End If
        Session("ConstructTable") = table
        ds = Nothing
    End Function

    Private Function Rebuild()
        Dim i As Integer
        Dim found As Integer
        'found = 0
        intNumeric = True
        Dim numPerc As Double
        Dim ds As New DataSet



        aryCAD = Session("aryCAD")
        For i = 0 To aryCAD.Count - 1
            aryCAD(i)(0) = Request.Form("txtBranch" & i)
            aryCAD(i)(1) = Request.Form("txtCC" & i)
            aryCAD(i)(2) = Request.Form("txtPercent" & i)

            numPerc += Val(Request.Form("txtPercent" & i))
        Next

  

        Session("ttlPerc") = numPerc
        Session("aryCAD") = Nothing
        Session("aryCAD") = aryCAD


    End Function
    Private Function nRebuild()
        Dim i As Integer
        Dim found As Integer
        'found = 0
        intNumeric = True
        Dim numPerc As New Double
        Dim ds As New DataSet

        ds = Session("dsCAD")
        found = ds.Tables(0).Rows.Count
        For i = 0 To found - 1
            ds.Tables(0).Rows(i)("CBM_BRANCH_NAME") = Request.Form("txtBranch" & i)
            ds.Tables(0).Rows(i)("CC_CC_DESC") = Request.Form("txtCC" & i)

            If Request.Form("txtPercent" & i) = "" Or (Request.Form("txtPercent" & i)) Is System.DBNull.Value Then
                ds.Tables(0).Rows(i)("CAD_PERCENT") = 0
            Else
                ds.Tables(0).Rows(i)("CAD_PERCENT") = Request.Form("txtPercent" & i)
            End If
            numPerc += Val(Request.Form("txtPercent" & i))
        Next

        Session("ttlPerc2") = numPerc
        Session("dsCAD") = Nothing
        Session("dsCAD") = ds
        Session("NewdsCADRow") = ds.Tables(0).Rows.Count 'found

    End Function
    Private Function Rebuild2()
        Dim i As Integer
        Dim found As Integer
        Dim ds As New DataSet
        Dim count As Integer
        intNumeric = True


        aryCAD = Session("aryCAD")
        For i = 0 To aryCAD.Count - 1
            aryCAD(i)(0) = Request.Form("txtBranch" & i)
            aryCAD(i)(1) = Request.Form("txtCC" & i)
            aryCAD(i)(2) = Request.Form("txtPercent" & i)
        Next

        i = 10
        Do While i < aryCAD.Count
            If aryCAD(i)(0) = "" Then
                aryCAD.RemoveAt(i)
                i = 10
            Else
                i = i + 1
            End If
        Loop

        Session("aryCAD") = Nothing
        Session("aryCAD") = aryCAD




    End Function
    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        Dim ds As New DataSet


        aryCAD = Session("aryCAD")
        aryCAD.Add(New String() {"", "", "", ""})
        Session("aryCAD") = Nothing
        Session("aryCAD") = aryCAD

    End Function

    Private Function nBuildLine()
        Dim i As Integer
        Dim found As Integer
        Dim ds As New DataSet


        ds = Session("dsCAD")
        ds.Tables(0).Rows.Add(New String() {"", "", "", ""})
        Session("dsCAD") = Nothing
        Session("dsCAD") = ds


    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

    

        If Session("aryCAD") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                aryCAD.Add(New String() {Request.Form("txtBranch"), Request.Form("txtCC"), Request.Form("txtPercent"), ""})
            Next
            Session("aryCAD") = aryCAD
        End If

    End Function

    Private Sub cmdNewAddLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNewAddLine.Click
        Session("Action") = "New"
        'Rebuild()
        BuildLine()
        Rebuild()
        ConstructTable()
        'Populate()
    End Sub
    Function validate(ByVal count As String, Optional ByVal type As String = "", Optional ByVal ds As DataSet = Nothing) As Boolean
        If type = "" Then
            Dim i As Integer
            Dim j As Integer
            Dim errorcount As Integer = 0
            Dim comparingcount As Integer = 0
            Dim arystr(CInt(count), 1) As String
            For i = 0 To CInt(count) - 1
                arystr(i, 0) = aryCAD(i)(0)
                arystr(i, 1) = aryCAD(i)(1)
            Next
            For i = 0 To CInt(count) - 1
                For j = 0 To CInt(count) - 1
                    If arystr(j, 0) <> "" And arystr(j, 1) <> "" Then
                        comparingcount = comparingcount + 1
                        If arystr(i, 0) = arystr(j, 0) And arystr(i, 1) = arystr(j, 1) Then
                            errorcount = errorcount + 1
                        End If
                    End If
                Next
            Next
            If errorcount > (comparingcount / CInt(count)) Then
                Return False
            Else
                Return True
            End If
        Else
            Dim i As Integer
            Dim j As Integer
            Dim errorcount As Integer = 0
            Dim comparingcount As Integer = 0
            Dim arystr(ds.Tables(0).Rows.Count, 1) As String
            For i = 0 To ds.Tables(0).Rows.Count - 1
                arystr(i, 0) = ds.Tables(0).Rows(i).Item(1)
                arystr(i, 1) = ds.Tables(0).Rows(i).Item(2)
            Next
            For i = 0 To ds.Tables(0).Rows.Count - 1
                For j = 0 To ds.Tables(0).Rows.Count - 1
                    If arystr(j, 0) <> "" And arystr(j, 1) <> "" Then
                        comparingcount = comparingcount + 1
                        If arystr(i, 0) = arystr(j, 0) And arystr(i, 1) = arystr(j, 1) Then
                            errorcount = errorcount + 1
                        End If
                    End If
                Next
            Next
            If errorcount > (comparingcount / ds.Tables(0).Rows.Count) Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Sub cmdNewSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNewSave.Click
        Dim strNewIR, intMsg, Code As String
        Dim iCheck As Boolean = False
        Dim iCheckQty As Boolean = False
        Dim iCheckLoc As Boolean = False
        Dim iCheckQtyZero As Boolean = False
        Dim i As Integer
        Dim RLoc, RSLoc As String
        Dim objCAD As New IPP

   
        aryCAD = Session("aryCAD")


        Rebuild()
        ConstructTable()
        If validateInput() Then


            If validate(aryCAD.Count) = False Then
                strMsg = objGlobal.GetErrorMessage("00002")
                strMsg = strMsg & "(HO/BR Code / Cost Centre Code)"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If
            If Session("ttlPerc") > 100 Then
                strMsg = objGlobal.GetErrorMessage("00015")
                strMsg = "Amount " & strMsg & "100%."
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Session("Action") = "New"
                'Exit Sub

            Else
                If objCAD.InsertCostAllocDetail(aryCAD, ddlCostAllocCode.SelectedValue) Then
                    objCAD.Message(Me, "00003", MsgBoxStyle.Information)
                Else
                    objCAD.Message(Me, "00002", MsgBoxStyle.Information)
                End If

                Session("Action") = "Modify"
            End If

        End If

       

        ConstructTable()

        If Session("Action") = "Modify" Then
            cmdSave.Visible = True
            cmdAddLine.Visible = True
            cmdDelete.Visible = True
            cmdNewSave.Visible = False
            cmdNewAddLine.Visible = False
        Else
            cmdSave.Visible = False
            cmdAddLine.Visible = False
            cmdDelete.Visible = False
            cmdNewSave.Visible = True
            cmdNewAddLine.Visible = True
        End If
       

    End Sub
    Private Function validateInput() As Boolean
        Dim i As Integer
        Dim ds As New DataSet
        Dim iCheck As Boolean = False

        If Session("Action") = "New" Then
            aryCAD = Session("aryCAD")

            For i = 0 To aryCAD.Count - 1
                If aryCAD.Item(i)(0) <> "" Or aryCAD.Item(i)(1) <> "" Or aryCAD.Item(i)(2) <> "" Then
                    iCheck = True
                    If aryCAD.Item(i)(0) = "" Then
                        strMsg = objGlobal.GetErrorMessage("00001")
                        strMsg = "HO/BR Code " & strMsg
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                        'ElseIf aryCAD.Item(i)(0).ToString.Substring(0, 2) = "HO" _
                        'And aryCAD.Item(i)(0).ToString.Substring(7, 3) = "900" _
                        'And aryCAD.Item(i)(1) = "" Then
                        '    strMsg = objGlobal.GetErrorMessage("00001")
                        '    strMsg = "Cost Centre Code " & strMsg
                        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '    Return False
                    ElseIf aryCAD.Item(i)(2) = "" Then
                        strMsg = objGlobal.GetErrorMessage("00001")
                        strMsg = "Percentage " & strMsg
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If
                End If
            Next
            If iCheck = False Then
                strMsg = objGlobal.GetErrorMessage("00020")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Return False
            End If
            Return True
        Else
            ds = Session("dsCAD")

            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i)("CBM_BRANCH_NAME") <> "" Or ds.Tables(0).Rows(i)("CC_CC_DESC") <> "" Or ds.Tables(0).Rows(i)("CAD_PERCENT") <> 0 Then
                    If ds.Tables(0).Rows(i)("CBM_BRANCH_NAME") = "" Then
                        strMsg = objGlobal.GetErrorMessage("00001")
                        strMsg = "HO/BR Code " & strMsg
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                        'ElseIf ds.Tables(0).Rows(i)("CDM_DEPT_NAME").ToString.Substring(0, 2) = "HO" _
                        'And ds.Tables(0).Rows(i)("CDM_DEPT_NAME").ToString.Substring(7, 3) = "900" _
                        'And ds.Tables(0).Rows(i)("CC_CC_DESC") = "" Then
                        '    strMsg = objGlobal.GetErrorMessage("00001")
                        '    strMsg = "Cost Centre Code " & strMsg
                        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '    Return False
                    ElseIf ds.Tables(0).Rows(i)("CAD_PERCENT") = 0 Then
                        strMsg = objGlobal.GetErrorMessage("00001")
                        strMsg = "Percentage " & strMsg
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If


                End If
            Next
            Return True
        End If
    End Function
End Class