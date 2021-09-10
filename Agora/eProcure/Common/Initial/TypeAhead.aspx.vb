Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Partial Public Class TypeAheadVendor
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("from") = "BIM" Or Request.QueryString("from") = "EntVENDORLIST" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objAdmin As New Admin
            ds = objAdmin.searchapprvendorforBIM()
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_COY_ID"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "EntVendorName" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objvend As New RFQ
            ds = objvend.GetApprVendor
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "name" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItem("IM_INVENTORY_NAME")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_INVENTORY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "code" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItem("IM_ITEM_CODE")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_ITEM_CODE")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "name2" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItem2("IM_INVENTORY_NAME")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_INVENTORY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "code2" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItem2("IM_ITEM_CODE")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_ITEM_CODE")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "VendorCode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objvend As New RFQ
            ds = objvend.getSearchVendor()
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_ID")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_ID"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "VendorName" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objvend As New RFQ
            ds = objvend.getSearchVendor()
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "VENDORLIST" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objAdmin As New Admin
            ds = objAdmin.searchvendorfor()
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_COY_ID"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        'To show only the approved vendors
        If Request.QueryString("from") = "FFPO" Then
            Dim q As String
            Dim gst As New GST
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objAdmin As New Admin
            ds = objAdmin.searchvendor("AV", "", "", True, True, True)
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_COY_ID"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_CURRENCY_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CV_Payment_Term"))
                    Response.Write("|")
                    Response.Write(vendorRow("CV_Payment_Method"))
                    Response.Write("|")
                    Response.Write(vendorRow("CV_BILLING_METHOD"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_TAX_CALC_BY"))
                    Response.Write("|")
                    Response.Write(gst.chkGST(vendorRow("CM_COY_ID")))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        'To get the Rules category based on the selected glcode
        If Request.QueryString("from") = "RuleCategory" Then
            Dim arrayList As New ArrayList
            Dim glcode As String = ""
            If Not Session("arydoc") Is Nothing Then
                If Not CType(Session("arydoc"), ArrayList).Count = 0 Then
                    Dim q As String
                    q = Request.QueryString("q")
                    Dim ds As New DataSet
                    Dim objIPPMain As New IPPMain
                    If Not Request.QueryString("GLCode") Is Nothing And Request.QueryString("GLCode") <> "" Then
                        If Request.QueryString("GLCode").Contains(":") Then
                            glcode = Request.QueryString("GLCode").Split(":")(0)
                        Else
                            glcode = Request.QueryString("GLCode").Trim
                        End If
                        ds = objIPPMain.getRuleCategory(glcode, q)
                    Else
                        ds = objIPPMain.getRuleCategory("", q)
                    End If
                    Dim catRow As DataRow
                    For Each catRow In ds.Tables(0).Rows
                        Response.Write(catRow("igc_glrule_category"))
                        Response.Write("|")
                        Response.Write(catRow("igc_glrule_category_index"))
                        Response.Write(vbCrLf)
                    Next
                End If
            End If
        End If

        If Request.QueryString("from") = "ConCat" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCon As New ContCat
            ds = objCon.getVendorCompany()
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CM_COY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("CM_COY_ID"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "CostCentre" Then
            Dim q As String
            Dim _ccExist As Boolean = False
            q = Request.QueryString("q")
            Dim lineNo = Request.QueryString("i")
            Dim arrayList As New ArrayList
            If Not Session("arydoc") Is Nothing Then
                If Not CType(Session("arydoc"), ArrayList).Count = 0 Then
                    arrayList = CType(Session("arydoc"), ArrayList)
                End If
            End If
            'Array items = nothing means user hasnt selected any payfor comp.
            Dim ds, dsTiedCC As New DataSet
            Dim sqlStr, branchCode, compId As String
            Dim objCA As New IPP
            Dim objdb As New EAD.DBCom
            branchCode = Request.QueryString("branchCode")
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objCA.getCostCentre(Request.QueryString("compid"), q, Request.QueryString("role"))
                compId = Request.QueryString("compid")
            Else
                If Not arrayList Is Nothing Then
                    If Not arrayList(lineNo)(1) Is Nothing Then
                        If arrayList(lineNo)(1).ToString.ToUpper.Contains("OWN") Then
                            'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                            'compId = "HLB"
                            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = Session("CompanyId")
                        Else
                            ds = objCA.getCostCentre(arrayList(lineNo)(1).ToString.Trim, q, Request.QueryString("role"))
                            compId = arrayList(lineNo)(1).ToString.Trim
                        End If
                    Else
                        If Not Session("SelectedComp_Edit") Is Nothing Then
                            ds = objCA.getCostCentre(Common.Parse(Session("SelectedComp_Edit").ToString.Trim), q, Request.QueryString("role"))
                            compId = Common.Parse(Session("SelectedComp_Edit").ToString.Trim)
                        Else
                            'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                            'compId = "HLB"
                            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = Session("CompanyId")
                        End If
                    End If
                Else
                    'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                    'compId = "HLB"
                    'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                    ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                    compId = Session("CompanyId")
                End If
            End If
            If Not branchCode = "" Then
                branchCode = branchCode.Split(":")(0).ToString.Trim
                'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                'sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " & _
                '    "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " & _
                '    "WHERE BCC_COY_ID = '" & compId & "' AND BCC_BRANCH_CODE = '" & Common.Parse(branchCode) & "' " & _
                '    "ORDER BY CC_CC_CODE "
                sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " &
                    "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " &
                    "WHERE (BCC_COY_ID = '" & compId & "') AND BCC_BRANCH_CODE = '" & Common.Parse(branchCode) & "' " &
                    "ORDER BY CC_CC_CODE "
                dsTiedCC = objdb.FillDs(sqlStr)
                Session("dsTiedCC") = dsTiedCC
                Dim CCRow As DataRow : CCRow = Nothing
                Response.Clear()
                For Each CCRow In ds.Tables(0).Rows
                    _ccExist = False
                    For Each item As DataRow In dsTiedCC.Tables(0).Rows
                        If CCRow("CC_CC_CODE").ToString.Split(":")(0).Trim = item("CC_CC_CODE") Then
                            _ccExist = True
                        End If
                    Next
                    If Not _ccExist Then
                        If q = "*" Then
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write(branchCode)
                            Response.Write(vbCrLf)
                        Else
                            If InStr(LCase(CCRow("CC_CC_CODE")), LCase(q)) <> False Then
                                Response.Write(CCRow("CC_CC_CODE"))
                                Response.Write("|")
                                Response.Write(CCRow("CC_CC_CODE"))
                                Response.Write("|")
                                Response.Write(branchCode)
                                Response.Write(vbCrLf)
                            End If
                        End If
                    End If
                Next
            End If
        End If

        If Request.QueryString("from") = "IPPCostCentre" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim lineNo = Request.QueryString("i")
            Dim ccIndex = Request.QueryString("ccIndex")
            Dim arrayList As New ArrayList
            If Not Session("arydoc") Is Nothing Then
                If Not CType(Session("arydoc"), ArrayList).Count = 0 Then
                    arrayList = CType(Session("arydoc"), ArrayList)
                End If
            End If
            Dim ds, dsTiedCC As New DataSet
            Dim sqlStr, branchCode, compId As String
            Dim objCA As New IPP
            Dim objdb As New EAD.DBCom
            Dim _ccExist As Boolean = False

            branchCode = Request.QueryString("branchCode")
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objCA.getCostCentre(Request.QueryString("compid"), q, Request.QueryString("role"))
                compId = Request.QueryString("compid")
            Else
                If Not arrayList Is Nothing Then
                    If Not arrayList(lineNo)(1) Is Nothing Then
                        If arrayList(lineNo)(1).ToString.ToUpper.Contains("OWN") Then
                            'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                            'compId = "HLB"
                            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = Session("CompanyId")
                        Else
                            ds = objCA.getCostCentre(arrayList(lineNo)(1).ToString.Trim, q, Request.QueryString("role"))
                            compId = arrayList(lineNo)(1).ToString.Trim
                        End If
                    Else
                        If Not Session("SelectedComp_Edit") Is Nothing Then
                            ds = objCA.getCostCentre(Common.Parse(Session("SelectedComp_Edit").ToString.Trim), q, Request.QueryString("role"))
                            compId = Common.Parse(Session("SelectedComp_Edit").ToString.Trim)
                        Else
                            'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                            'compId = "HLB"
                            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = Session("CompanyId")
                        End If
                    End If
                Else
                    'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                    'compId = "HLB"
                    'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                    ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                    compId = Session("CompanyId")
                End If
            End If

            If Not branchCode = "" Then
                branchCode = branchCode.Split(":")(0).ToString.Trim
                sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " &
                  "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " &
                  "WHERE BCC_COY_ID = '" & compId & "' AND BCC_BRANCH_CODE = '" & Common.Parse(branchCode) & "' " &
                  "ORDER BY CC_CC_CODE "
                dsTiedCC = objdb.FillDs(sqlStr)
                Session("dsTiedCC") = dsTiedCC
                Dim CCRow As DataRow
                CCRow = Nothing
                Response.Clear()
                For Each CCRow In ds.Tables(0).Rows
                    _ccExist = False
                    For Each item As DataRow In dsTiedCC.Tables(0).Rows
                        If CCRow("CC_CC_CODE").ToString.Split(":")(0).Trim = item("CC_CC_CODE") Then
                            _ccExist = True
                        End If
                    Next
                    If Not _ccExist Then
                        If q = "*" Then
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write(branchCode)
                            Response.Write(vbCrLf)
                        Else
                            If InStr(LCase(CCRow("CC_CC_CODE")), LCase(q)) <> False Then
                                Response.Write(CCRow("CC_CC_CODE"))
                                Response.Write("|")
                                Response.Write(CCRow("CC_CC_CODE"))
                                Response.Write("|")
                                Response.Write(branchCode)
                                Response.Write(vbCrLf)
                            End If
                        End If
                    End If
                Next
            End If
        End If

        If Request.QueryString("from") = "Branch" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCA As New IPP
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objCA.getBranch(Request.QueryString("compid"), q, Request.QueryString("role"))
            Else
                ds = objCA.getBranch(, q, Request.QueryString("role"))
            End If

            Dim BrRow As DataRow
            For Each BrRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(BrRow("cbm_branch_code"))
                    Response.Write("|")
                    Response.Write(BrRow("cbm_branch_code"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(BrRow("cbm_branch_code")), LCase(q)) <> False Then
                        Response.Write(BrRow("cbm_branch_code"))
                        Response.Write("|")
                        Response.Write(BrRow("cbm_branch_code"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "IPPCOY" Then
            Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
            Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim GST As New GST
            Dim objIPPMain As New IPPMain
            Dim strGSTCode As String = ""

            Dim blnIPPOfficer As Boolean
            Dim objUsers As New Users
            Dim blnIPPOfficerS As Boolean

            'Zulham 05122018
            blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
            blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")

            If blnIPPOfficer Or blnIPPOfficerS Then
                ds = objIPPMain.getCompanyTypeahead1(Request.QueryString("frm"), q, Request.QueryString("CoyType"))
            Else
                ds = objIPPMain.getCompanyTypeahead(Request.QueryString("frm"), q)
            End If

            'Zulham 30-01-2015 IPP-GST Stage 2A
            If Not Request.QueryString("module") Is Nothing Then
                Dim isNostro As String
                If Request.QueryString("Nostro") Is Nothing Then isNostro = "" Else isNostro = Request.QueryString("Nostro")

                If Request.QueryString("module") = "billing" Then
                    ds = objIPPMain.getCompanyTypeahead1(Request.QueryString("frm"), q, Request.QueryString("CoyType"), isNostro)
                End If
            End If

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                Try
                    If q = "*" Then
                        'get the gstCode if it exists
                        If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(vendorRow("IC_INDEX"))
                        Response.Write(vendorRow("IC_COY_NAME"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_INDEX"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_ADDR_LINE1"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_ADDR_LINE2"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_ADDR_LINE3"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_POSTCODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_CITY"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_STATE"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_COUNTRY"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_PAYMENT_METHOD"))
                        Response.Write("|")
                        Response.Write(vendorRow("IC_bank_code"))
                        Response.Write("|")
                        Response.Write(vendorRow("ic_resident_type"))
                        Response.Write("|")
                        Response.Write(strGSTCode)
                        Response.Write("|")
                        'Zulham 15-02-2015 8317
                        Response.Write(vendorRow("ic_nostro_flag"))
                        Response.Write("|")
                        'Zulham 21/12/2015 (Stage 4 Phase 2)
                        Response.Write(vendorRow("ic_business_reg_no"))
                        Response.Write("|")
                        If Not Request.QueryString("CoyType") Is Nothing Then
                            Response.Write(Request.QueryString("CoyType").ToString)
                            Response.Write("|")
                        End If
                        Response.Write(vbCrLf)

                    Else
                        If InStr(LCase(vendorRow("IC_COY_NAME")), LCase(q)) <> False Then
                            'get the gstCode if it exists
                            If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(vendorRow("IC_INDEX"))

                            Response.Write(vendorRow("IC_COY_NAME"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_INDEX"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_ADDR_LINE1"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_ADDR_LINE2"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_ADDR_LINE3"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_POSTCODE"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_CITY"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_STATE"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_COUNTRY"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_PAYMENT_METHOD"))
                            Response.Write("|")
                            Response.Write(vendorRow("IC_bank_code"))
                            Response.Write("|")
                            Response.Write(vendorRow("ic_resident_type"))
                            Response.Write("|")
                            Response.Write(strGSTCode)
                            Response.Write("|")
                            'Zulham 15-02-2015 8317
                            Response.Write(vendorRow("ic_nostro_flag"))
                            Response.Write("|")
                            'Zulham 21/12/2015 (Stage 4 Phase 2)
                            Response.Write(vendorRow("ic_business_reg_no"))
                            Response.Write("|")
                            'Zulham 21/02/2016 - IPP Stage 4 Phase 2
                            If Not Request.QueryString("CoyType") Is Nothing Then
                                Response.Write(Request.QueryString("CoyType").ToString)
                                Response.Write("|")
                            End If
                            Response.Write(vbCrLf)
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception(ex.ToString)
                End Try
            Next
        End If

        If Request.QueryString("from") = "IPPCOYADDR" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain

            Dim objUsers As New Users

            ds = objIPPMain.getCompanyAddrTypeahead(Request.QueryString("frm"), q)

            Dim vendorAddrRow As DataRow
            For Each vendorAddrRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorAddrRow("IC_ADDR_LINE1"))
                    Response.Write("|")
                    Response.Write(vendorAddrRow("IC_COY_ID"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorAddrRow("IC_ADDR_LINE1")), LCase(q)) <> False Then
                        Response.Write(vendorAddrRow("IC_ADDR_LINE1"))
                        Response.Write("|")
                        Response.Write(vendorAddrRow("IC_COY_ID"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "GLCode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objIPPMain.getGLCodeTypeAhead(Request.QueryString("compid"), q)
            Else
                ds = objIPPMain.getGLCodeTypeAhead(, q)
            End If
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("CBG_B_GL_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "GLCodeOnly" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objAdmin As New Admin

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objAdmin.getGLCodeOnlyTypeAhead(strDefIPPCompID, q)
            ElseIf strDefIPPCompID <> "" Then
                ds = objAdmin.getGLCodeOnlyTypeAhead(strDefIPPCompID, q)
            Else
                ds = objAdmin.getGLCodeOnlyTypeAhead(, q)
            End If

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("CBG_B_GL_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "GLCode2" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("chk") = "N" Then
                ds = objIPPMain.getGLCodeTypeAhead2(q, "N", Request.QueryString("na"), Request.QueryString("def")) 'Modified for IPP GST Stage 2A - CH
            Else
                ds = objIPPMain.getGLCodeTypeAhead2(q, , Request.QueryString("na"), Request.QueryString("def")) 'Modified for IPP GST Stage 2A - CH
            End If
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CBG_B_GL_DESC"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("CBG_B_GL_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("CBG_B_GL_DESC"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "AssetGroup" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objIPPMain.getAssetGroupTypeAhead(Request.QueryString("compid"))
            Else
                ds = objIPPMain.getAssetGroupTypeAhead()
            End If
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("AG_GROUP")), LCase(q)) <> False Then
                    Response.Write(vendorRow("AG_GROUP"))
                    Response.Write("|")
                    Response.Write(vendorRow("AG_GROUP"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If
        If Request.QueryString("from") = "AssetSubGroup" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objIPPMain.getAssetSubGroupTypeAhead(Request.QueryString("compid"))
            Else
                ds = objIPPMain.getAssetSubGroupTypeAhead()
            End If

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("AG_GROUP")), LCase(q)) <> False Then
                    Response.Write(vendorRow("AG_GROUP"))
                    Response.Write("|")
                    Response.Write(vendorRow("AG_GROUP"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "CostAlloc" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objIPPMain.getCostAllocTypeAhead(Request.QueryString("compid"), q)
            Else
                ds = objIPPMain.getCostAllocTypeAhead(, q)
            End If

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("CAM_CA_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CAM_CA_CODE"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("CAM_CA_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("CAM_CA_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("CAM_CA_CODE"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If

        If Request.QueryString("from") = "Commodity" Then
            Dim q As String
            Dim strsql As String
            Dim objdb As New EAD.DBCom
            q = Request.QueryString("q")
            Dim ds As New DataSet
            strsql = "SELECT CT_ID,CT_NAME " _
                    & "FROM commodity_type WHERE CT_LAST_LVL=1 AND CT_NAME LIKE '%" & Common.Parse(q) & "%' ORDER BY CT_NAME LIMIT 1000"
            ds = objdb.FillDs(strsql)
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("CT_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("CT_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("CT_ID"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If
        If Request.QueryString("from") = "itemcode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objBC As New BuyerCat
            ds = objBC.getItemCode()
            Dim ICRow As DataRow
            For Each ICRow In ds.Tables(0).Rows
                If InStr(LCase(ICRow("PM_VENDOR_ITEM_CODE")), LCase(q)) <> False Then
                    Response.Write(ICRow("PM_VENDOR_ITEM_CODE"))
                    Response.Write("|")
                    Response.Write(ICRow("PM_PRODUCT_INDEX"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "itemname" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objBC As New BuyerCat
            ds = objBC.getItemName()
            Dim INRow As DataRow
            For Each INRow In ds.Tables(0).Rows
                If InStr(LCase(INRow("PM_PRODUCT_DESC")), LCase(q)) <> False Then
                    Response.Write(INRow("PM_PRODUCT_DESC"))
                    Response.Write("|")
                    Response.Write(INRow("PM_PRODUCT_INDEX"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "UserGroup" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPP As New IPP

            ds = objIPP.getUserGroupTypeAhead()

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IUM_GRP_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IUM_GRP_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("IUM_GRP_INDEX"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "Branch2" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCA As New IPP

            ds = objCA.getBranchTypeAhead(Request.QueryString("frm"), Request.QueryString("idx"))

            Dim BrRow As DataRow
            For Each BrRow In ds.Tables(0).Rows
                If Request.QueryString("btn") = "BranchName" Then
                    If InStr(LCase(BrRow("CDM_DEPT_NAME")), LCase(q)) <> False Then
                        Response.Write(BrRow("CDM_DEPT_NAME"))
                        Response.Write("|")
                        Response.Write(BrRow("CDM_DEPT_NAME"))
                        Response.Write(vbCrLf)
                    End If

                Else
                    If InStr(LCase(BrRow("CDM_DEPT_CODE")), LCase(q)) <> False Then
                        Response.Write(BrRow("CDM_DEPT_CODE"))
                        Response.Write("|")
                        Response.Write(BrRow("CDM_DEPT_INDEX"))
                        Response.Write(vbCrLf)
                    End If
                End If
            Next
        End If
        'Modified for IPP GST Stage 2A - CH
        If Request.QueryString("from") = "BranchCode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCA As New IPP

            ds = objCA.getBranchCodeTypeAhead()

            Dim BrRow As DataRow
            For Each BrRow In ds.Tables(0).Rows
                If InStr(LCase(BrRow("CBM_BRANCH_CODE")), LCase(q)) <> False Then
                    Response.Write(BrRow("CBM_BRANCH_CODE"))
                    Response.Write("|")
                    Response.Write(BrRow("CBM_BRANCH_CODE"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If
        '-------------------------------------
        If Request.QueryString("from") = "CostCentre2" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCA As New IPP

            ds = objCA.getCostCentreTypeAhead(Request.QueryString("frm"), Request.QueryString("idx"))

            'ds = objCA.getCostCentre
            Dim CCRow As DataRow
            For Each CCRow In ds.Tables(0).Rows
                If Request.QueryString("btn") = "CCDesc" Then
                    If InStr(LCase(CCRow("CC_CC_DESC")), LCase(q)) <> False Then
                        Response.Write(CCRow("CC_CC_DESC"))
                        Response.Write("|")
                        Response.Write(CCRow("CC_CC_DESC"))
                        Response.Write(vbCrLf)
                    End If

                Else
                    If InStr(LCase(CCRow("CC_CC_CODE")), LCase(q)) <> False Then
                        Response.Write(CCRow("CC_CC_CODE"))
                        Response.Write("|")
                        Response.Write(CCRow("CC_CC_CODE"))
                        Response.Write(vbCrLf)
                    End If

                End If
            Next
        End If

        If Request.QueryString("from") = "User" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objCA As New IPP

            ds = objCA.getUserTypeAhead(Request.QueryString("frm"), Request.QueryString("idx"))

            'ds = objCA.getCostCentre
            Dim CCRow As DataRow
            For Each CCRow In ds.Tables(0).Rows
                If Request.QueryString("btn") = "UserBranchCode" Then
                    If InStr(LCase(CCRow("UM_DEPT_ID")), LCase(q)) <> False Then
                        Response.Write(CCRow("UM_DEPT_ID"))
                        Response.Write("|")
                        Response.Write(CCRow("UM_DEPT_ID"))
                        Response.Write(vbCrLf)
                    End If

                ElseIf Request.QueryString("btn") = "UserName" Then
                    If InStr(LCase(CCRow("UM_USER_NAME")), LCase(q)) <> False Then
                        Response.Write(CCRow("UM_USER_NAME"))
                        Response.Write("|")
                        Response.Write(CCRow("UM_USER_NAME"))
                        Response.Write(vbCrLf)
                    End If

                Else
                    If InStr(LCase(CCRow("UM_USER_ID")), LCase(q)) <> False Then
                        Response.Write(CCRow("UM_USER_ID"))
                        Response.Write("|")
                        Response.Write(CCRow("UM_USER_ID"))
                        Response.Write(vbCrLf)
                    End If

                End If
            Next
        End If

        If Request.QueryString("from") = "nameWO" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItemWO("IM_INVENTORY_NAME")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_INVENTORY_NAME")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        If Request.QueryString("from") = "codeWO" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objINV As New Inventory
            ds = objINV.searchInvItemWO("IM_ITEM_CODE")
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If InStr(LCase(vendorRow("IM_ITEM_CODE")), LCase(q)) <> False Then
                    Response.Write(vendorRow("IM_ITEM_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("IM_INVENTORY_NAME"))
                    Response.Write(vbCrLf)
                End If
            Next
        End If

        ''Jules 2014.02.24 - Capex Enhancement
        If Request.QueryString("from") = "BRGL" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim ds2 As New DataSet
            Dim objBC As New BudgetControl

            If Request.QueryString("type") = "br" Then
                'ds = objBC.GetBR_GL_CC("", "", "", "", True, True)
                ds = objBC.GetBR_GL_CC(q, "", "", "", False, True)
                Dim BrRow As DataRow
                For Each BrRow In ds.Tables(0).Rows
                    'If InStr(LCase(BrRow("Branch Code")), LCase(q)) <> False Then
                    '    Response.Write(BrRow("Branch Code"))
                    '    Response.Write("|")

                    '    ds2 = objBC.GetBR_GL_CC(BrRow("Branch Code"), "", "", "")
                    '    If ds2.Tables(0).Rows.Count > 0 Then
                    '        Response.Write(ds2.Tables(0).Rows(0).Item("GL Code").ToString)
                    '        Response.Write("|")
                    '        Response.Write(ds2.Tables(0).Rows(0).Item("Acct Index").ToString)
                    '    Else
                    '        'Response.Write(BrRow("CDM_DEPT_NAME"))
                    '        Response.Write("")
                    '    End If

                    '    Response.Write(vbCrLf)
                    'End If
                    Response.Write(BrRow("Branch Code"))
                    Response.Write("|")
                    Response.Write(BrRow("GL Code"))
                    'Response.Write("|")
                    'Response.Write(BrRow("Acct Index"))
                    Response.Write(vbCrLf)
                Next
            ElseIf Request.QueryString("type") = "cc" Then
                If Request.QueryString("branch") <> "" And Request.QueryString("glcode") <> "" Then
                    ds = objBC.GetBR_GL_CC(Request.QueryString("branch"), Request.QueryString("glcode"), "", "", True)

                    Dim BrRow As DataRow
                    For Each BrRow In ds.Tables(0).Rows
                        If InStr(LCase(BrRow("Cost Center")), LCase(q)) <> False Then
                            Response.Write(BrRow("Cost Center"))
                            Response.Write("|")
                            Response.Write(BrRow("Interface Code"))
                            Response.Write(vbCrLf)
                        End If
                    Next
                End If
            End If
        End If

        'Jules 2018.04.10 - PAMB Scrum 1 - Get Cost Centre. (Method duped from Request.QueryString("from") = "CostCentre")
        If Request.QueryString("from") = "CostCentre_wo_Branch" Then
            Dim q As String
            Dim _ccExist As Boolean = False
            q = Request.QueryString("q")
            Dim lineNo = Request.QueryString("i")
            Dim arrayList As New ArrayList
            If Not Session("arydoc") Is Nothing Then
                If Not CType(Session("arydoc"), ArrayList).Count = 0 Then
                    arrayList = CType(Session("arydoc"), ArrayList)
                End If
            End If
            'Array items = nothing means user hasnt selected any payfor comp.
            Dim ds, dsTiedCC As New DataSet
            Dim compId As String
            Dim objCA As New IPP
            Dim objdb As New EAD.DBCom

            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objCA.getCostCentre(Request.QueryString("compid"), q, Request.QueryString("role"))
                compId = Request.QueryString("compid")
            Else
                If Not arrayList Is Nothing Then
                    If Not arrayList(lineNo)(1) Is Nothing Then
                        If arrayList(lineNo)(1).ToString.ToUpper.Contains("OWN") Then
                            'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                            'compId = "HLB"
                            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = Session("CompanyId")
                        Else
                            'Zulham 24102018
                            'ds = objCA.getCostCentre(arrayList(lineNo)(1).ToString.Trim, q, Request.QueryString("role"))
                            ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                            compId = arrayList(lineNo)(1).ToString.Trim
                        End If
                    Else
                        'If Not Session("SelectedComp_Edit") Is Nothing Then
                        '    ds = objCA.getCostCentre(Common.Parse(Session("SelectedComp_Edit").ToString.Trim), q, Request.QueryString("role"))
                        '    compId = Common.Parse(Session("SelectedComp_Edit").ToString.Trim)
                        'Else
                        '    'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                        '    'compId = "HLB"
                        '    'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                        ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                        compId = Session("CompanyId")
                        'End If
                    End If
                Else
                    'ds = objCA.getCostCentre("HLB", q, Request.QueryString("role"))
                    'compId = "HLB"
                    'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                    ds = objCA.getCostCentre(Session("CompanyId"), q, Request.QueryString("role"))
                    compId = Session("CompanyId")
                End If
            End If

            Dim CCRow As DataRow : CCRow = Nothing
            Response.Clear()
            For Each CCRow In ds.Tables(0).Rows
                _ccExist = False
                For Each item As DataRow In ds.Tables(0).Rows
                    If CCRow("CC_CC_CODE").ToString.Split(":")(0).Trim = item("CC_CC_CODE") Then
                        _ccExist = True
                    End If
                Next
                If Not _ccExist Then
                    If q = "*" Then
                        Response.Write(CCRow("CC_CC_CODE"))
                        Response.Write("|")
                        Response.Write(CCRow("CC_CC_CODE"))
                        Response.Write("|")
                        Response.Write("")
                        Response.Write(vbCrLf)
                    Else
                        If InStr(LCase(CCRow("CC_CC_CODE")), LCase(q)) <> False Then
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write(CCRow("CC_CC_CODE"))
                            Response.Write("|")
                            Response.Write("")
                            Response.Write(vbCrLf)
                        End If
                    End If
                End If
            Next
        End If

        If Request.QueryString("from") = "AnalysisCode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain
            If Request.QueryString("compid") <> "" And Request.QueryString("compid") <> "Own Co." Then
                ds = objIPPMain.getAnalysisCodeTypeAhead(Request.QueryString("deptcode"), Request.QueryString("compid"), q)
            Else
                ds = objIPPMain.getAnalysisCodeTypeAhead(Request.QueryString("deptcode"),, q)
            End If
            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("AC_ANALYSIS_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("AC_ANALYSIS_CODE"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("AC_ANALYSIS_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("AC_ANALYSIS_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("AC_ANALYSIS_CODE"))
                        Response.Write(vbCrLf)
                    End If
                End If

            Next
        End If
        'End modification 2018.04.10 - PAMB Scrum 1.

        'Zulham 17/5/2018 - PAMB
        If Request.QueryString("from") = "PAMBGLCode" Then
            Dim q As String
            q = Request.QueryString("q")
            Dim ds As New DataSet
            Dim objIPPMain As New IPPMain

            ds = objIPPMain.getGLCodeTypeAhead(Request.QueryString("compid"), q)

            Dim vendorRow As DataRow
            For Each vendorRow In ds.Tables(0).Rows
                If q = "*" Then

                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write("|")
                    Response.Write(vendorRow("CBG_B_GL_CODE"))
                    Response.Write(vbCrLf)

                Else
                    If InStr(LCase(vendorRow("CBG_B_GL_CODE")), LCase(q)) <> False Then
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write("|")
                        Response.Write(vendorRow("CBG_B_GL_CODE"))
                        Response.Write(vbCrLf)
                    End If
                End If
            Next
        End If
        'End

    End Sub

End Class