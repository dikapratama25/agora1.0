'*************************************************************************************
'Created By:  Louise
'Date:  18/05/2005
'Screen:  View RFP Invitation
'Purpose:  This screen allows vendor to view the published RFP information
'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy


Public Class ViewRFPInvitation
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgEligibility As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblUserId As System.Web.UI.WebControls.Label
    Protected WithEvents lblCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblRFPRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblRFPTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblProcProcedure As System.Web.UI.WebControls.Label
    Protected WithEvents lblApproach As System.Web.UI.WebControls.Label
    Protected WithEvents lblFOTType As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents lblFee As System.Web.UI.WebControls.Label
    Protected WithEvents lblEarnestMoney As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentMode As System.Web.UI.WebControls.Label
    Protected WithEvents lblCoyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCallingEntity As System.Web.UI.WebControls.Label
    Protected WithEvents lblCollec As System.Web.UI.WebControls.Label
    Protected WithEvents lblComment As System.Web.UI.WebControls.Label
    Protected WithEvents lblContactPerson As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocPubDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblOpenDate As System.Web.UI.WebControls.Label
    Dim objrfp As New RFPCLASS
    Dim objDb As New  EAD.DBCom
    Protected WithEvents lblAwardingEnt As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocPubTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblOpenTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblEarnestAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblRFPBriefingInfo As System.Web.UI.WebControls.Label
    Protected WithEvents dtgBrief As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HID As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents TITLE As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents brieft As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents rfpsub As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdNotAgree As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_display As System.Web.UI.WebControls.Label
    Protected WithEvents lblFOTCONT As System.Web.UI.WebControls.Label
    Protected WithEvents dtgFOTCONT As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblFOTCAT As System.Web.UI.WebControls.Label
    Protected WithEvents dtgFOTCAT As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTechnical As System.Web.UI.WebControls.Label
    Protected WithEvents dtgTechnical As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblFinancial As System.Web.UI.WebControls.Label
    Protected WithEvents dtgFinancial As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDrawings As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDrawings As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCompliance As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCompliance As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblAccUserID As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoResponseDetails As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreateResponse As System.Web.UI.WebControls.Button
    Protected WithEvents cmdViewClarification As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCreateQuery As System.Web.UI.WebControls.Button
    Protected WithEvents lblResUserID As System.Web.UI.WebControls.Label
    Protected WithEvents lblResCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblResDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblResTime As System.Web.UI.WebControls.Label
    Protected WithEvents cmdUpdateFOT As System.Web.UI.WebControls.Button
    Protected WithEvents lblInvitationInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvitationAccept As System.Web.UI.WebControls.Label
    Protected WithEvents lblResponseInfo As System.Web.UI.WebControls.Label
    Protected WithEvents cmdViewResponse As System.Web.UI.WebControls.Button
    Protected WithEvents CAT As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblEligibility As System.Web.UI.WebControls.Label
    Protected WithEvents TECH As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents FIN As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents DR As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents COMP As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblInvitationAccept1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table3 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblResponseInfo1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents CATTYPE As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents display1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents display2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents CONTYPE As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents TECHDOC As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents FINDOC As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents DRDOC As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents COMDOC As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents II As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents ee As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblCloseDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblExtDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblSubLocation As System.Web.UI.WebControls.Label
    Protected WithEvents lblClose As System.Web.UI.WebControls.Label
    Protected WithEvents lblCloseTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblExtTime As System.Web.UI.WebControls.Label
    Protected WithEvents EXT As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EXTI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EXTT As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EXTTI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblRejUserID As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejTime As System.Web.UI.WebControls.Label
    Protected WithEvents Accb As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Acci As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Rejb As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Reji As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdAgree As System.Web.UI.WebControls.Button
    Protected WithEvents EAR As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EARI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents EMP As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents IA As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents RI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents timeNow As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents CloseTime As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents CD As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents CDI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents bac As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents nores As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents buttonline As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lbl3 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdViewAward As System.Web.UI.WebControls.Button
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objinv As New invClass
        Dim rfpid As String
        Dim versionNo As String
        Dim temp As String
        Dim temp2 As String
        Dim temp3 As String
        Dim tempRes As String
        Dim i As Integer
        Dim j As Integer
        Dim ds As New DataSet
        Dim dt2 As New DataTable
        Dim ds3 As New DataSet
        Dim ds4 As New DataSet
        Dim ds5 As New DataSet
        Dim ds6 As New DataSet
        Dim ds7 As New DataSet
        Dim ds8 As New DataSet
        Dim ds9 As New DataSet
        Dim ds10 As New DataSet
        Dim extdate As String
        Dim closedate As String
        Dim objRFPval As New RFPVALUE
        Dim objcomval As New ComVALUE
        Dim accept As Integer = EnumRFPV.accepted
        Dim reject As Integer = EnumRFPV.rejected
        Dim IL_responded As Integer = EnumRFPV.responded
        Dim responded As Integer = EnumRFPINVI.Responded
        Dim NoResponse As Integer = EnumRFPINVI.NoResponse
        Dim contract As Integer = EnumRFPFOT.Contract
        Dim catalogue As Integer = EnumRFPFOT.Catalogue
        Dim paid As Integer = EnumRFPPayment.Paid
        Dim pending As Integer = EnumRFPPayment.Pending
        Dim required As Integer = EnumEarnestMon.Required
        Dim notrequired As Integer = EnumEarnestMon.NotRequired
        Dim strtemp As String
        Dim SendFOTUpdate As String = EnumRFPFOTUpdate.SendFOTUpdate
        Dim FOTUpdated As String = EnumRFPFOTUpdate.FOTUpdated
        Dim FOTUpdateStatus As String = ""
        Dim coyID As String
        Dim userID As String
        Dim PO_coyID As String

        coyID = HttpContext.Current.Session("CompanyId")
        userID = HttpContext.Current.Session("UserId")

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgEligibility)
        blnSorting = False
        SetGridProperty(dtgBrief)
        blnSorting = False
        blnPaging = False
        SetGridProperty(dtgTechnical)
        blnPaging = False
        SetGridProperty(dtgFinancial)
        blnPaging = False
        SetGridProperty(dtgDrawings)
        blnPaging = False
        SetGridProperty(dtgCompliance)
        blnPaging = False
        SetGridProperty(dtgFOTCONT)
        blnPaging = False
        SetGridProperty(dtgFOTCAT)

        viewstate("mode") = Request.QueryString("mode")
        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))

        If Not Page.IsPostBack Then
            ds = objinv.getPublishedRFPInvitation(rfpid, versionNo)

            Dim strdate As String
            strdate = Year(Now()) & "/" & Month(Now()) - 1 & "/" & Day(Now()) - 1 & "/" & Hour(Now()) + 16 & "/" & Minute(Now()) & "/" & Second(Now())
            ' strdate = Format(CDate(Date.UtcNow), "yyyy/MM/dd/HH/mm/ss")

            Me.timeNow.Value = strdate '"2005/5/14/10/00/00"
            If Not IsDBNull(ds.Tables(0).Rows(0)("RM_EXT_DATE_CLOSE")) Then
                Me.CloseTime.Value = Common.FormatWheelDate(WheelDateFormat.CountDownDate, ds.Tables(0).Rows(0)("RM_EXT_DATE_CLOSE"))
            Else
                Me.CloseTime.Value = Common.FormatWheelDate(WheelDateFormat.CountDownDate, ds.Tables(0).Rows(0)("RM_DATE_CLOSE"))
            End If


            Session("strurl2") = strCallFrom
            Session("strurlSentScreen") = strCallFrom
            ' Dim rndKey As New Random
            ' Me.RegisterClientScriptBlock(rndKey.Next.ToString, "<script language='javascript'> Countdown('June 15,2005 21:00:00','2005/5/14/25/52/21','View RFP Invitation'); </script>")

            ds10 = objinv.getPOCoyID(rfpid, versionNo)
            PO_coyID = Common.parseNull(ds10.Tables(0).Rows(0)("RM_COY_ID"))

            objRFPval = objinv.GetRFPDetailsL(rfpid, versionNo, PO_coyID)

            dt2 = objinv.GetRFPComInfo(objcomval, PO_coyID)
            ds3 = objinv.getAcceptInviteelist(rfpid, versionNo)
            ds4 = objinv.getFOTType(rfpid, versionNo)
            ds5 = objinv.getAcceptRespond(rfpid, versionNo)
            ds6 = objinv.getFOTUpdateStatus(rfpid, versionNo)
            strtemp = objinv.getAddendum(rfpid, versionNo)

            'Check if FOT Update Status = 0
            If ds6.Tables(0).Rows.Count <> 0 Then
                FOTUpdateStatus = Common.parseNull(ds6.Tables(0).Rows(0)("IL_FOT_UPDATE_STATUS"))
                If FOTUpdateStatus = SendFOTUpdate Then
                    cmdUpdateFOT.Visible = True
                End If
            End If

            lblRFPRef.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_RFP_REF"))
            lblRFPTitle.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_RFP_NAME"))
            lblDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_RFP_DESC"))
            lblProcProcedure.Text = Common.parseNull(ds.Tables(0).Rows(0)("RFP_TYPE"))
            lblApproach.Text = Common.parseNull(ds.Tables(0).Rows(0)("RFP_APPROACH"))
            lblFOTType.Text = Common.parseNull(ds.Tables(0).Rows(0)("RFP_FOTTYPE"))
            lblCurrency.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_DEPOSIT_CURRENCY_CODE"))
            lblFee.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_RFP_DEPOSIT"))
            lblEarnestMoney.Text = Common.parseNull(ds.Tables(0).Rows(0)("RFP_EARNEST_MONEY_STATUS"))

            If Common.parseNull(ds.Tables(0).Rows(0)("RM_EARNEST_MONEY_STATUS")) = required Then
                lblEarnestAmount.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_EARNEST_MONEY"))
            Else
                Me.EAR.Visible = False
                Me.EARI.Visible = False
                lblEarnestAmount.Visible = False
            End If

            If Request.QueryString("status") <> "Sent" Then
                Dim viewableAll As String = EnumViewAward.viewableAll
                Dim viewableResonly As String = EnumViewAward.viewableResonly
                Dim k As Integer
                Dim l As Integer

                CD.Visible = False
                CDI.Visible = False
                ds7 = objinv.getAward(rfpid)
                ds8 = objinv.getInvitedVendors(rfpid)
                ds9 = objinv.getResVendor(rfpid)

                If ds7.Tables(0).Rows.Count > 0 Then
                    If ds7.Tables(0).Rows(0)("RAW_PUB_TYPE") = viewableAll Then
                        For k = 0 To ds8.Tables(0).Rows.Count - 1
                            If ds8.Tables(0).Rows(k)("IL_V_COY_ID") = coyID Then
                                cmdViewAward.Visible = True
                            End If
                        Next
                    Else
                        For l = 0 To ds9.Tables(0).Rows.Count - 1
                            If ds9.Tables(0).Rows(l)("RR_V_COY_ID") = coyID Then
                                cmdViewAward.Visible = True
                            End If
                        Next
                    End If
                End If
            End If

            GetRFPCompanyInfo()
            lblPaymentMode.Text = Common.parseNull(ds.Tables(0).Rows(0)("RFP_PAYMODE"))
            lblComment.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_REMARKS"))
            lblContactPerson.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_CONTACT_PERSON"))
            lblDocPubDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("RM_PUBLICATION_DATE"))
            lblDocPubTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds.Tables(0).Rows(0)("RM_PUBLICATION_DATE"))
            lblOpenDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("RM_DATE_OPEN"))
            lblOpenTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds.Tables(0).Rows(0)("RM_DATE_OPEN"))
            lblCloseDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("RM_DATE_CLOSE"))
            lblCloseTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds.Tables(0).Rows(0)("RM_DATE_CLOSE"))

            If Not IsDBNull(ds.Tables(0).Rows(0)("RM_EXT_DATE_CLOSE")) Then
                Me.EXT.Visible = True
                Me.EXTI.Visible = True
                Me.EXTT.Visible = True
                Me.EXTTI.Visible = True
                lblExtDate.Visible = True
                lblExtTime.Visible = True
                lblExtDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("RM_EXT_DATE_CLOSE"))
                lblExtTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds.Tables(0).Rows(0)("RM_EXT_DATE_CLOSE"))
            Else
                Me.EXT.Visible = False
                Me.EXTI.Visible = False
                Me.EXTT.Visible = False
                Me.EXTTI.Visible = False
                lblExtDate.Visible = False
                lblExtTime.Visible = False
            End If

            lblSubLocation.Text = Common.parseNull(ds.Tables(0).Rows(0)("RM_SUBMISSION_POINT"))
            lblUserId.Text = Common.parseNull(ds.Tables(0).Rows(0)("USERNAME"))
            lblCompanyName.Text = Common.parseNull(dt2.Rows(0)("CM_COY_NAME"))
            lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("RM_DATE_CREATED"))
            lblTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds.Tables(0).Rows(0)("RM_DATE_CREATED"))
            lblStatus.Text = Request.QueryString("status")

            Bindgrid(dtgEligibility, lblEligibility)
            Bindgrid(dtgBrief, lblRFPBriefingInfo)

            If viewstate("mode") = "public" Then

                cmdAgree.Visible = False
                cmdCreateQuery.Visible = False
                cmdCreateResponse.Visible = False
                cmdNotAgree.Visible = False
                cmdUpdateFOT.Visible = False
                cmdViewClarification.Visible = False
                cmdViewResponse.Visible = False

                Me.EMP.Visible = False
                Me.HID.Visible = False
                Me.CAT.Visible = False
                Me.CONTYPE.Visible = False
                Me.CATTYPE.Visible = False
                Me.TECHDOC.Visible = False
                Me.FINDOC.Visible = False
                Me.DRDOC.Visible = False
                Me.COMDOC.Visible = False
                Me.TECH.Visible = False
                Me.FIN.Visible = False
                Me.DR.Visible = False
                Me.COMP.Visible = False
                Me.II.Visible = False
                Me.IA.Visible = False
                Me.RI.Visible = False
                Me.display2.Visible = False
                'Me.ss.Visible = False
                Table3.Visible = False
                lblInvitationAccept.Visible = False
                lblInvitationAccept1.Visible = False
                Table7.Visible = False
                Table6.Visible = False
                nores.Visible = False
                lbl3.Visible = False
                bac.Visible = False
                buttonline.Visible = False
                lblResponseInfo1.Visible = False
                lblResponseInfo.Visible = False
                display1.Visible = False

            ElseIf viewstate("mode") = "view" Then

                temp = Common.parseNull(ds3.Tables(0).Rows(0)("IL_ACCEPT").ToString.Trim)

                'If Request.QueryString("status") = "Sent" Then
                '    If temp = accept Or temp = reject Or temp = IL_responded Then
                '        Table6.Visible = False
                '        cmdAgree.Visible = False
                '        cmdNotAgree.Visible = False
                '        lbl_display.Visible = False
                '        lbl3.Visible = False
                '        Me.display1.Visible = False
                '        Me.display2.Visible = False
                '    End If
                'Else
                '    cmdAgree.Visible = False
                '    cmdNotAgree.Visible = False
                '    lbl_display.Visible = False
                '    lbl3.Visible = False
                '    Me.display1.Visible = False
                '    Me.display2.Visible = False
                'End If

                j = objinv.getPublicationDate(rfpid, versionNo)
                temp3 = Common.parseNull(ds5.Tables(0).Rows(0)("IL_RFP_FEE_PAYMENT").ToString.Trim)
                viewstate("temp2") = Common.parseNull(ds4.Tables(0).Rows(0)("RS_FOT_TYPE").ToString.Trim)
                tempRes = Common.parseNull(ds3.Tables(0).Rows(0)("IL_SENT").ToString.Trim)

                If j = 1 Then
                    If temp = accept Or temp = IL_responded Then
                        lblInvitationAccept.Visible = True
                        lblAccUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
                        lblAccCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
                        lblAccDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))
                        lblAccTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))
                        Me.Rejb.Visible = False
                        Me.Reji.Visible = False
                        lblRejDate.Visible = False
                        lblRejDate.Visible = False
                        lblRejUserID.Visible = False
                        lblRejCompanyName.Visible = False

                        If temp3 = paid Then
                            dtgTechnical.Visible = True
                            dtgFinancial.Visible = True
                            dtgDrawings.Visible = True
                            dtgCompliance.Visible = True
                            dtgFOTCONT.Visible = True

                            Bindgrid(dtgTechnical, lblTechnical)
                            Bindgrid(dtgFinancial, lblFinancial)
                            Bindgrid(dtgDrawings, lblDrawings)
                            Bindgrid(dtgCompliance, lblCompliance)

                            If viewstate("temp2") = contract Then
                                BindgridFOT(dtgFOTCONT, lblFOTCONT, viewstate("temp2"))
                            Else
                                BindgridFOT(dtgFOTCAT, lblFOTCAT, viewstate("temp2"))
                            End If

                            If Request.QueryString("status") = "Sent" Then
                                'Me.EMP.Visible = False
                                Me.nores.Visible = False
                                cmdCreateResponse.Visible = True

                                If strtemp = 1 Then
                                    cmdViewClarification.Visible = True
                                Else
                                    cmdViewClarification.Visible = False
                                End If

                                cmdCreateQuery.Visible = True

                                If tempRes = responded Then
                                    lblResponseInfo.Visible = True
                                    lblResUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
                                    lblResCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
                                    lblResDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_SENT_ON"))
                                    lblResTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_SENT_ON"))
                                Else
                                    RI.Visible = False '-----
                                    Me.Table7.Visible = False
                                    lblResponseInfo1.Visible = False
                                End If
                            Else
                                If tempRes = responded Then
                                    lblResponseInfo.Visible = True
                                    lblResUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
                                    lblResCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
                                    lblResDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_SENT_ON"))
                                    lblResTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_SENT_ON"))

                                    If strtemp = 1 Then
                                        cmdViewClarification.Visible = True
                                    Else
                                        cmdViewClarification.Visible = False
                                    End If

                                    cmdViewResponse.Visible = True
                                    Me.EMP.Visible = False
                                Else
                                    lblNoResponseDetails.Visible = True
                                    Me.EMP.Visible = False
                                    Me.Table7.Visible = False
                                    lblResponseInfo1.Visible = False
                                    RI.Visible = False
                                End If
                            End If
                        ElseIf temp3 = pending Then
                            If Request.QueryString("status") = "Closed" Then
                                lblNoResponseDetails.Visible = True
                            End If
                            Me.Table7.Visible = False
                            lblResponseInfo1.Visible = False
                            Me.EMP.Visible = False
                            Me.HID.Visible = False
                            Me.CAT.Visible = False
                            Me.CONTYPE.Visible = False
                            Me.CATTYPE.Visible = False
                            Me.TECHDOC.Visible = False
                            Me.FINDOC.Visible = False
                            Me.DRDOC.Visible = False
                            Me.COMDOC.Visible = False
                            Me.TECH.Visible = False
                            Me.FIN.Visible = False
                            Me.DR.Visible = False
                            Me.COMP.Visible = False
                            Me.II.Visible = False
                            Me.bac.Visible = False
                            Me.RI.Visible = False
                            Me.display1.Visible = False
                            Me.display2.Visible = False
                            Me.buttonline.Visible = False
                            Me.nores.Visible = False
                            'Me.ss.Visible = False
                        End If
                    ElseIf temp = reject Then
                        Me.Accb.Visible = False
                        Me.Acci.Visible = False
                        lblAccCompanyName.Visible = False
                        lblAccDate.Visible = False
                        lblAccTime.Visible = False
                        lblAccUserID.Visible = False
                        lblInvitationAccept.Visible = True

                        lblRejUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
                        lblRejCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
                        lblRejDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))
                        lblRejTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))

                        Me.Table7.Visible = False
                        lblResponseInfo1.Visible = False
                        Me.EMP.Visible = False
                        Me.CAT.Visible = False
                        Me.CONTYPE.Visible = False
                        Me.CATTYPE.Visible = False
                        Me.TECHDOC.Visible = False
                        Me.FINDOC.Visible = False
                        Me.DRDOC.Visible = False
                        Me.COMDOC.Visible = False
                        Me.TECH.Visible = False
                        Me.FIN.Visible = False
                        Me.DR.Visible = False
                        Me.COMP.Visible = False
                        Me.II.Visible = False
                        'Me.ss.Visible = False
                    Else
                        Me.Table3.Visible = False
                        lblInvitationAccept1.Visible = False
                        Me.Table7.Visible = False
                        lblResponseInfo1.Visible = False
                        Me.EMP.Visible = False
                        Me.CAT.Visible = False
                        Me.CONTYPE.Visible = False
                        Me.CATTYPE.Visible = False
                        Me.TECHDOC.Visible = False
                        Me.FINDOC.Visible = False
                        Me.DRDOC.Visible = False
                        Me.COMDOC.Visible = False
                        Me.TECH.Visible = False
                        Me.FIN.Visible = False
                        Me.DR.Visible = False
                        Me.COMP.Visible = False
                        Me.II.Visible = False
                        Me.HID.Visible = False
                        Me.display1.Visible = False
                        Me.IA.Visible = False
                        Me.RI.Visible = False
                        Me.EMP.Visible = False
                        Me.Table6.Visible = False
                        Me.bac.Visible = False
                        'Me.ss.Visible = False
                    End If
                End If

                If Request.QueryString("status") = "Sent" Then
                    If temp = accept Or temp = reject Or temp = IL_responded Then
                        Table6.Visible = False
                        cmdAgree.Visible = False
                        cmdNotAgree.Visible = False
                        lbl_display.Visible = False
                        lbl3.Visible = False
                        Me.display1.Visible = False
                        Me.display2.Visible = False
                    Else
                        Me.Table3.Visible = False
                        lblInvitationAccept1.Visible = False
                        Me.Table7.Visible = False
                        lblResponseInfo1.Visible = False
                        Me.EMP.Visible = False
                        Me.CAT.Visible = False
                        Me.CONTYPE.Visible = False
                        Me.CATTYPE.Visible = False
                        Me.TECHDOC.Visible = False
                        Me.FINDOC.Visible = False
                        Me.DRDOC.Visible = False
                        Me.COMDOC.Visible = False
                        Me.TECH.Visible = False
                        Me.FIN.Visible = False
                        Me.DR.Visible = False
                        Me.COMP.Visible = False
                        Me.II.Visible = False
                        Me.HID.Visible = False
                        Me.display1.Visible = False
                        Me.IA.Visible = False
                        Me.RI.Visible = False
                        Me.EMP.Visible = False

                        Table6.Visible = True
                        cmdAgree.Visible = True
                        cmdNotAgree.Visible = True
                        lbl_display.Visible = True
                        lbl3.Visible = True
                        Me.display1.Visible = True
                        Me.display2.Visible = True
                        bac.Visible = True
                    End If
                Else
                    cmdAgree.Visible = False
                    cmdNotAgree.Visible = False
                    lbl_display.Visible = False
                    lbl3.Visible = False
                    Me.display1.Visible = False
                    Me.display2.Visible = False
                End If

                Me.lbl_display.Text = "<UL><LI>Click <STRONG>""Agree""</STRONG> to&nbsp;accept this RFP Invitation.</LI><LI>Click <STRONG>""Not Agree"" </STRONG>to reject this RFP Invitation.</LI></UL>"

            End If

            '----------------------------------------------

        End If

        objinv = Nothing
        objrfp = Nothing


    End Sub

    Private Function Bindgrid(ByVal dtg_id As DataGrid, ByVal lbl_id As Label, Optional ByVal pSorted As Boolean = False) As String
        Dim objinv As New invClass
        Dim rfpid As String
        Dim versionNo As String
        Dim ds As DataSet = New DataSet
        Dim record As Integer
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim objRFPBrief As New BriefVALUE

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))

        If dtg_id.ID.Trim = "dtgEligibility" Then
            ds = objinv.getEligibilityList(rfpid, versionNo)
        ElseIf dtg_id.ID.Trim = "dtgTechnical" Then
            ds = objinv.getDocNameRFPInvi(rfpid, versionNo, "TecDoc")
        ElseIf dtg_id.ID.Trim = "dtgFinancial" Then
            ds = objinv.getDocNameRFPInvi(rfpid, versionNo, "FinDoc")
        ElseIf dtg_id.ID.Trim = "dtgDrawings" Then
            ds = objinv.getDocNameRFPInvi(rfpid, versionNo, "RFPDR")
        ElseIf dtg_id.ID.Trim = "dtgCompliance" Then
            ds = objinv.getDocNameRFPInvi(rfpid, versionNo, "COMLIST")
        ElseIf dtg_id.ID.Trim = "dtgBrief" Then
            ds = objrfp.GetRFPBriefingRes(rfpid, versionNo, objRFPBrief)
        End If

        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_id.PageSize = 0 Then
                dtg_id.CurrentPageIndex = dtg_id.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            viewstate(dtg_id.ID & "_PageRecordCnt") = intPageRecordCnt
            If dtg_id.CurrentPageIndex > (dvViewSample.Count \ dtg_id.PageSize) Then
                dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 1, 0, (dvViewSample.Count \ dtg_id.PageSize))
            ElseIf dtg_id.CurrentPageIndex = (dvViewSample.Count \ dtg_id.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtg_id.PageSize) Then
                    'user does not re-enter search criteria 
                    dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 0, 0, (dvViewSample.Count \ dtg_id.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtg_id.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 1, 0, (dvViewSample.Count \ dtg_id.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtg_id.CurrentPageIndex = (dvViewSample.Count \ dtg_id.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtg_id.DataSource = dvViewSample
            dtg_id.DataBind()

            If dtg_id.ID.Trim = "dtgEligibility" Then
                'Me.HID.Visible = True
                Me.ee.Visible = True
                Me.TITLE.Visible = True
                'Me.ss.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgBrief" Then
                Me.rfpsub.Visible = True
                Me.brieft.Visible = True
            ElseIf dtg_id.ID.Trim = "dtgTechnical" Then
                Me.TECH.Visible = True
                'Me.FIN.Visible = True
            ElseIf dtg_id.ID.Trim = "dtgFinancial" Then
                Me.FIN.Visible = True
                'Me.DR.Visible = True
            ElseIf dtg_id.ID.Trim = "dtgDrawings" Then
                Me.DR.Visible = True
                'Me.COMP.Visible = True
                'Me.II.Visible = True
            ElseIf dtg_id.ID.Trim = "dtgCompliance" Then
                Me.COMP.Visible = True
            End If

            lbl_id.Visible = True
        Else
            dtg_id.DataBind()

            If dtg_id.ID.Trim = "dtgEligibility" Then
                'Me.HID.Visible = False
                Me.TITLE.Visible = False
                Me.ee.Visible = False
                'Me.ss.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgBrief" Then
                Me.rfpsub.Visible = False
                Me.brieft.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgTechnical" Then
                Me.TECH.Visible = False
                Me.TECHDOC.Visible = False
                'Me.FIN.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgFinancial" Then
                Me.FIN.Visible = False
                Me.FINDOC.Visible = False
                'Me.DR.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgDrawings" Then
                Me.DR.Visible = False
                Me.DRDOC.Visible = False
                'Me.COMP.Visible = False
                'Me.II.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgCompliance" Then
                Me.COMP.Visible = False
                Me.COMDOC.Visible = False
                'Me.II.Visible = False
            End If

            lbl_id.Visible = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtg_id.PageCount

        objinv = Nothing

    End Function

    Private Function BindgridFOT(ByVal dtg_id As DataGrid, ByVal lbl_id As Label, ByVal FOTtype As String, Optional ByVal pSorted As Boolean = False) As String
        Dim objinv As New invClass
        Dim rfpid As String
        Dim versionNo As String
        Dim ds As DataSet = New DataSet
        Dim record As Integer

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))

        If dtg_id.ID.Trim = "dtgFOTCONT" Then
            ds = objinv.getFOT(rfpid, versionNo, FOTtype)
        ElseIf dtg_id.ID.Trim = "dtgFOTCAT" Then
            ds = objinv.getFOTCat(rfpid, versionNo, FOTtype)
        End If

        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_id.PageSize = 0 Then
                dtg_id.CurrentPageIndex = dtg_id.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            viewstate(dtg_id.ID & "_PageRecordCnt") = intPageRecordCnt

            ' check when user re-enter search criteria and click on other page without click search button
            If dtg_id.CurrentPageIndex > (dvViewSample.Count \ dtg_id.PageSize) Then
                dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 1, 0, (dvViewSample.Count \ dtg_id.PageSize))
            ElseIf dtg_id.CurrentPageIndex = (dvViewSample.Count \ dtg_id.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtg_id.PageSize) Then
                    'user does not re-enter search criteria 
                    dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 0, 0, (dvViewSample.Count \ dtg_id.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtg_id.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtg_id.CurrentPageIndex = IIf((dvViewSample.Count \ dtg_id.PageSize) = 1, 0, (dvViewSample.Count \ dtg_id.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtg_id.CurrentPageIndex = (dvViewSample.Count \ dtg_id.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtg_id.DataSource = dvViewSample
            dtg_id.DataBind()
            If dtg_id.ID.Trim = "dtgFOTCONT" Then
                Me.HID.Visible = True
                'Me.ss.Visible = True
                Me.CAT.Visible = False
                'Me.TECH.Visible = False
                Me.CATTYPE.Visible = False
                lblFOTCONT.Visible = True
            ElseIf dtg_id.ID.Trim = "dtgFOTCAT" Then
                Me.CONTYPE.Visible = False
                Me.HID.Visible = False
                lblFOTCAT.Visible = True
                dtgFOTCAT.Visible = True
            End If
        Else
            dtg_id.DataBind()
            If dtg_id.ID.Trim = "dtgFOTCONT" Then
                Me.HID.Visible = False
                Me.CONTYPE.Visible = False
                'Me.CAT.Visible = False
                'Me.TECH.Visible = False
                'Me.CATTYPE.Visible = False
            ElseIf dtg_id.ID.Trim = "dtgFOTCAT" Then
                Me.CAT.Visible = False
                Me.CATTYPE.Visible = False
                'Me.CONTYPE.Visible = True
                'Me.HID.Visible = True
            End If
            lblFOTCONT.Visible = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtg_id.PageCount()

        objinv = Nothing

    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim dtg_id As DataGrid
        dtg_id = sender

        If dtg_id.ID = "dtgBrief" Then
            Bindgrid(dtg_id, lblRFPBriefingInfo)
        ElseIf dtg_id.ID = "dtgEligibility" Then
            Bindgrid(dtg_id, lblEligibility)
        ElseIf dtg_id.ID = "dtgTechnical" Then
            Bindgrid(dtg_id, lblTechnical)
        ElseIf dtg_id.ID.Trim = "dtgFinancial" Then
            Bindgrid(dtg_id, lblFinancial)
        ElseIf dtg_id.ID.Trim = "dtgDrawings" Then
            Bindgrid(dtg_id, lblDrawings)
        ElseIf dtg_id.ID.Trim = "dtgCompliance" Then
            Bindgrid(dtg_id, lblCompliance)
        ElseIf dtg_id.ID.Trim = "dtgFOTCONT" Then
            BindgridFOT(dtg_id, lblFOTCONT, viewstate("temp2"))
        ElseIf dtg_id.ID.Trim = "dtgFOTCAT" Then
            BindgridFOT(dtg_id, lblFOTCAT, viewstate("temp2"))
        End If

    End Sub

    Private Sub dtgBrief_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBrief.ItemDataBound
        Dim rfpid As String
        Dim dt As New DataTable
        Dim objRFPBrief As New BriefVALUE
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            rfpid = Trim(Request.QueryString("RFPID"))
            dt = objrfp.GetRFPBriefing(rfpid, objRFPBrief)
            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dt.Rows(0)("RB_SDATE"))
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.Time, dt.Rows(0)("RB_SDATE"))
        End If
    End Sub

    Private Sub dtgBrief_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBrief.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgEligibility_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgEligibility.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgEligibility_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgEligibility.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        End If
    End Sub


    Private Sub cmdAgree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgree.Click
        Dim objinv As New invClass
        Dim strtemp As String
        Dim strtemp2 As String
        Dim rfpid As String
        Dim versionNo As String
        Dim accept As Integer = EnumRFPV.accepted
        Dim ds As New DataSet
        Dim ds5 As New DataSet

        versionNo = Trim(Request.QueryString("VERNO"))
        rfpid = Trim(Request.QueryString("RFPID"))

        strtemp = objinv.UpdateAccepttoInv(rfpid, versionNo, accept)
        strtemp2 = objinv.UpdateAccepttoRFPMSTR(rfpid, versionNo)
        objDb.Execute(strtemp)

        'Send Email-------------------
        If objDb.Execute(strtemp2) Then
            Dim objMail As New AgoraLegacy.Email 'eRFPmail
            Dim BCoyID As String

            ds = objinv.getPOCoyID(rfpid, versionNo)
            BCoyID = Common.parseNull(ds.Tables(0).Rows(0)("RM_COY_ID"))

            objMail.sendNotification(EmailType.RFPInviAccepted, HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"), BCoyID, "", "", lblRFPRef.Text, rfpid, versionNo)
            objMail = Nothing
        End If
        '-----------------------------

        cmdAgree.Visible = False
        cmdNotAgree.Visible = False
        lbl_display.Visible = False
        EMP.Visible = False
        display1.Visible = False
        lblInvitationAccept1.Visible = True
        lblInvitationAccept.Visible = True
        Table3.Visible = True
        IA.Visible = True
        RI.Visible = False
        display2.Visible = False
        bac.Visible = False
        Rejb.Visible = False
        Reji.Visible = False

        ds5 = objinv.getAcceptRespond(rfpid, versionNo)

        lblAccUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
        lblAccCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
        lblAccDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))
        lblAccTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))

        objinv = Nothing

    End Sub

    Private Sub cmdNotAgree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNotAgree.Click
        Dim objinv As New invClass
        Dim strtemp As String
        Dim rfpid As String
        Dim versionNo As String
        Dim reject As Integer = EnumRFPV.rejected
        Dim ds As New DataSet
        Dim ds5 As New DataSet

        versionNo = Trim(Request.QueryString("VERNO"))
        rfpid = Trim(Request.QueryString("RFPID"))

        strtemp = objinv.UpdateAccepttoInv(rfpid, versionNo, reject)

        'Send Email-------------------
        If objDb.Execute(strtemp) Then
            Dim objMail As New AgoraLegacy.Email 'eRFPmail
            Dim BCoyID As String

            ds = objinv.getPOCoyID(rfpid, versionNo)
            BCoyID = Common.parseNull(ds.Tables(0).Rows(0)("RM_COY_ID"))

            objMail.sendNotification(EmailType.RFPInviRejected, HttpContext.Current.Session("UserId"), HttpContext.Current.Session("CompanyId"), BCoyID, "", "", lblRFPRef.Text, rfpid, versionNo)
            objMail = Nothing
        End If
        '-----------------------------

        lbl_display.Visible = False
        cmdAgree.Visible = False
        cmdNotAgree.Visible = False
        EMP.Visible = False
        display1.Visible = False
        lblInvitationAccept1.Visible = True
        lblInvitationAccept.Visible = True
        Table3.Visible = True
        IA.Visible = True
        RI.Visible = False
        display2.Visible = False
        bac.Visible = False
        Accb.Visible = False
        Acci.Visible = False

        ds5 = objinv.getAcceptRespond(rfpid, versionNo)

        lblRejUserID.Text = Common.parseNull(ds5.Tables(0).Rows(0)("USERNAME"))
        lblRejCompanyName.Text = Common.parseNull(ds5.Tables(0).Rows(0)("COY_NAME"))
        lblRejDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))
        lblRejTime.Text = Common.FormatWheelDate(WheelDateFormat.Time, ds5.Tables(0).Rows(0)("IL_ACCEPT_ON"))

        objinv = Nothing

    End Sub

    Private Sub dtgTechnical_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTechnical.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgFinancial_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFinancial.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgDrawings_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDrawings.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgFOTCONT_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFOTCONT.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgCompliance_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCompliance.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Function GetRFPCompanyInfo()
        Dim objRFPComVal As New ComVALUE
        Dim objRFPCom As New RFPCLASS
        Dim objinv As New invClass
        Dim dt As DataTable
        Dim ds As New DataSet
        Dim strOwner As String
        Dim strcallEntity As String
        Dim strAwardEntity As String
        Dim strCollectPoint As String
        Dim strSubLocation As String
        Dim PO_coyID As String
        Dim rfpid As String
        Dim versionNo As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))

        ds = objinv.getPOCoyID(rfpid, versionNo)
        PO_coyID = Common.parseNull(ds.Tables(0).Rows(0)("RM_COY_ID"))
        dt = objinv.GetRFPComInfo(objRFPComVal, PO_coyID)

        With dt.Rows(0)

            lblCoyName.Text = IIf(lblCompanyName.Text = "", .Item("CM_COY_NAME"), ", " & .Item("CM_COY_NAME"))

            lblCallingEntity.Text = IIf(lblCallingEntity.Text = "", .Item("CM_COY_NAME"), ", " & .Item("CM_COY_NAME"))

            If strAwardEntity = "" Then
                strAwardEntity = .Item("CM_COY_NAME") & "<BR>"
            Else
                strAwardEntity = strAwardEntity & " " & .Item("CM_COY_NAME") & "<BR>"
            End If

            'get addr 1
            If strAwardEntity = "" Then
                strAwardEntity = .Item("CM_ADDR_LINE1") & "<BR>"
            Else
                strAwardEntity = strAwardEntity & vbCrLf & .Item("CM_ADDR_LINE1") & "<BR>"
            End If

            'get addr 2
            If .Item("CM_ADDR_LINE2") <> "" Then
                If strAwardEntity = "" Then
                    strAwardEntity = .Item("CM_ADDR_LINE2") & "<BR>"
                Else
                    strAwardEntity = strAwardEntity & vbCrLf & .Item("CM_ADDR_LINE2") & "<BR>"
                End If
            End If

            'get addr 3
            If .Item("CM_ADDR_LINE3") <> "" Then
                If strAwardEntity = "" Then
                    strAwardEntity = .Item("CM_ADDR_LINE3") & "<BR>"
                Else
                    strAwardEntity = strAwardEntity & vbCrLf & .Item("CM_ADDR_LINE3") & "<BR>"
                End If
            End If

            'get city
            If .Item("CM_CITY") <> "" Then
                If strAwardEntity = "" Then
                    strAwardEntity = .Item("CM_CITY")

                Else
                    strAwardEntity = strAwardEntity & " " & .Item("CM_CITY")
                End If
            End If

            'get postcode and state
            If .Item("CM_POSTCODE") <> "" Or .Item("STATE") = "" Then
                If strAwardEntity = "" Then
                    strAwardEntity = .Item("CM_POSTCODE") & " " & "<BR>" & .Item("STATE")
                Else
                    strAwardEntity = strAwardEntity & vbCrLf & .Item("CM_POSTCODE") & " " & "<BR>" & .Item("STATE")

                End If
            End If


            If .Item("COUNTRY") <> "" Then
                If strAwardEntity = "" Then
                    strAwardEntity = .Item("COUNTRY")
                Else
                    strAwardEntity = strAwardEntity & vbCrLf & .Item("COUNTRY")
                End If
            End If

            Me.lblAwardingEnt.Text = strAwardEntity
            Me.lblCollec.Text = strAwardEntity
            lblSubLocation.Text = strAwardEntity
        End With

        objRFPComVal = Nothing
        objRFPCom = Nothing

    End Function

    Private Sub cmdCreateQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateQuery.Click
        Dim rfpid As String
        Dim versionNo As String
        Dim status As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))
        status = Trim(Request.QueryString("status"))

        'lnbDocRef.NavigateUrl = "CreateQuery.aspx?mode=create&RFPID=" & rfpid & "&VERNO=" & versionNo & "&status=" & status
        Response.Redirect("CreateQuery.aspx?mode=create&RFPID=" & rfpid & "&VERNO=" & versionNo & "&status=" & status & "&pageid=" & strPageId)

    End Sub

    Private Sub back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick
        Dim strurl As String = Session("strurl")
        Session("strurl") = Nothing
        Response.Redirect(strurl)
    End Sub

    Private Sub cmdCreateResponse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateResponse.Click
        Dim rfpid As String
        Dim versionNo As String
        Dim ds As New DataSet
        Dim objinv As New invClass
        Dim resstatus As String
        Dim draft As String = EnumRFPResponse.draft
        Dim count As Integer
        Dim status As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))
        status = Trim(Request.QueryString("status"))
        ds = objinv.getRFPResponseStatus(rfpid, versionNo)
        count = ds.Tables(0).Rows.Count

        If count = 0 Then
            Response.Redirect("CreateRFPResponse.aspx?mode=create&RFPID=" & rfpid & "&VERNO=" & versionNo & "&status=" & status & "&pageid=" & strPageId)
        Else
            If ds.Tables(0).Rows(0)("RR_RES_STATUS") = draft Then
                Response.Redirect("CreateRFPResponse.aspx?mode=modify&RFPID=" & rfpid & "&VERNO=" & versionNo & "&status=" & status & "&pageid=" & strPageId)
            Else
                Response.Redirect("CreateRFPResponse.aspx?mode=resend&RFPID=" & rfpid & "&VERNO=" & versionNo & "&status=" & status & "&pageid=" & strPageId)
            End If
        End If
    End Sub

    Private Sub dtgFOTCAT_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFOTCAT.ItemCreated
        intPageRecordCnt = viewstate(sender.id & "_PageRecordCnt")
        blnCheckBox = False
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub cmdViewClarification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewClarification.Click
        Dim rfpid As String
        Dim versionNo As String
        Dim codevalue As String
        Dim clarification As String = MAINT.Clarification

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))
        codevalue = clarification

        Response.Redirect("ViewQueriesListing.aspx?RFPID=" & rfpid & "&VERNO=" & versionNo & "&codevalue=" & codevalue & "&pageid=" & strPageId)
    End Sub

    Private Sub cmdViewResponse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewResponse.Click
        Dim rfpid As String
        Dim versionNo As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))

        Response.Redirect("ViewRFPResponse.aspx?RFPID=" & rfpid & "&VERNO=" & versionNo & "&pageid=" & strPageId)
    End Sub


    Private Sub cmdUpdateFOT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdateFOT.Click
        Dim rfpid As String
        Dim versionNo As String
        Dim RFPRef As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))
        RFPRef = lblRFPRef.Text

        Response.Redirect("UpdateFOT.aspx?RFPID=" & rfpid & "&VERNO=" & versionNo & "&RFPRef=" & RFPRef & "&pageid=" & strPageId)
    End Sub

    Private Sub dtgTechnical_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTechnical.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objFile As New RFPFileManageClass
            Dim objrfq As New  RFQ
            Dim strFile As String
            Dim strFile1 As String
            Dim strURL As String
            Dim DocType As String

            strFile = Common.parseNull(dv("RD_DOC_NAME"))
            strFile1 = Common.parseNull(dv("RD_VOLUME_ID"))
            viewstate("DocType") = "RFP"

            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), AgoraLegacy.EnumDownLoadType.DocAttachment, ViewState("DocType"), AgoraLegacy.EnumUploadFrom.FrontOff, "TECDOC")

            e.Item.Cells(0).Text = strURL
        End If
    End Sub

    Private Sub dtgFinancial_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFinancial.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objFile As New RFPFileManageClass
            Dim objrfq As New  RFQ
            Dim strFile As String
            Dim strFile1 As String
            Dim strURL As String
            Dim DocType As String

            strFile = Common.parseNull(dv("RD_DOC_NAME"))
            strFile1 = Common.parseNull(dv("RD_VOLUME_ID"))
            viewstate("DocType") = "RFP"

            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), AgoraLegacy.EnumDownLoadType.DocAttachment, ViewState("DocType"), AgoraLegacy.EnumUploadFrom.FrontOff, "FINDOC")

            e.Item.Cells(0).Text = strURL
        End If
    End Sub

    Private Sub dtgCompliance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCompliance.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objFile As New RFPFileManageClass
            Dim objrfq As New  RFQ
            Dim strFile As String
            Dim strFile1 As String
            Dim strURL As String
            Dim DocType As String

            strFile = Common.parseNull(dv("RD_DOC_NAME"))
            strFile1 = Common.parseNull(dv("RD_VOLUME_ID"))
            viewstate("DocType") = "RFP"

            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), AgoraLegacy.EnumDownLoadType.DocAttachment, ViewState("DocType"), AgoraLegacy.EnumUploadFrom.FrontOff, "COMLIST")

            e.Item.Cells(0).Text = strURL
        End If
    End Sub

    Private Sub dtgDrawings_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDrawings.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objFile As New RFPFileManageClass
            Dim objrfq As New  RFQ
            Dim strFile As String
            Dim strFile1 As String
            Dim strURL As String
            Dim DocType As String

            strFile = Common.parseNull(dv("RD_DOC_NAME"))
            strFile1 = Common.parseNull(dv("RD_VOLUME_ID"))
            viewstate("DocType") = "RFP"

            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), AgoraLegacy.EnumDownLoadType.DocAttachment, ViewState("DocType"), AgoraLegacy.EnumUploadFrom.FrontOff, "RFPDR")

            e.Item.Cells(0).Text = strURL
        End If
    End Sub

    Private Sub cmdViewAward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewAward.Click
        Dim rfpid As String
        Dim versionNo As String
        Dim RFPRef As String

        rfpid = Trim(Request.QueryString("RFPID"))
        versionNo = Trim(Request.QueryString("VERNO"))
        RFPRef = lblRFPRef.Text

        Response.Redirect("../RFPAward/CreateAwardRFP.aspx?mode=vAward&index=" & rfpid & "&RFPver=" & versionNo & "&RFPref=" & RFPRef & "&pageid=" & strPageId)
    End Sub
End Class
