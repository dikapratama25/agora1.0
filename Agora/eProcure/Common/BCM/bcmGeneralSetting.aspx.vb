Imports AgoraLegacy
Imports eProcure.Component
Imports System.Runtime.InteropServices
Imports System.IO

Public Class bcmGeneralSetting
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdBack As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents rdMode As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblCBudgetS As System.Web.UI.WebControls.Label
    Protected WithEvents lblCBudgetE As System.Web.UI.WebControls.Label
    Protected WithEvents txtExt As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSaveEndDT As System.Web.UI.WebControls.Button
    Protected WithEvents rfv_txtSubStart As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtNewBudgetS As System.Web.UI.WebControls.Label
    Protected WithEvents txtNewBudgetDt As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtNewBudgetDt As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_cmdSaveEndDT As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cmdSaveMode As System.Web.UI.WebControls.Button
    Protected WithEvents lblmsgEDate As System.Web.UI.WebControls.Label
    Protected WithEvents vldDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lnkBCM As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSaveNewPeriod As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDownload As System.Web.UI.WebControls.Button
    Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents tbFirstNew As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtNewS As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewE As System.Web.UI.WebControls.TextBox
    Protected WithEvents tbBudget As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdStartNewBCM As System.Web.UI.WebControls.Button
    Protected WithEvents rfv_txtNewS As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtNewE As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents dgtest As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNewBud As System.Web.UI.WebControls.Label
    Protected WithEvents lnkDownloadPre As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPrePeriod As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdSaveMode.Enabled = False
        cmdSaveEndDT.Enabled = False
        cmdSaveNewPeriod.Enabled = False
        cmdDownload.Enabled = False

        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSaveMode)
        alButtonList.Add(cmdSaveEndDT)
        alButtonList.Add(cmdSaveNewPeriod)
        alButtonList.Add(cmdDownload)
        htPageAccess.Add("update", alButtonList)

        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        lblMsg.Text = ""
        lblmsgEDate.Text = ""
        lblNewBud.Text = ""
        If Not IsPostBack Then
            Populate()
        End If
        lnkBCM.Attributes.Add("OnClick", "window.open('" & dDispatcher.direct("BCM", "ModeHelp.htm") & "',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
        lnkBCM.NavigateUrl = "#"
    End Sub

    Private Sub Populate()
        Dim objBCM As New BudgetControl
        Dim drw As DataView
        Dim strBCM As String

        drw = objBCM.GetBCM()

        If Not drw Is Nothing Then
            rdMode.SelectedValue = drw.Table.Rows(0)("CM_BCM_SET")

            If IsDBNull(drw.Table.Rows(0)("CM_BUDGET_FROM_DATE")) Or IsDBNull(drw.Table.Rows(0)("CM_BUDGET_TO_DATE")) Then
                tbFirstNew.Style.Item("Display") = ""
                tbBudget.Style.Item("Display") = "none"
                rfv_txtNewBudgetDt.EnableClientScript = False
                rfv_txtNewS.Enabled = True
                rfv_txtNewE.Enabled = True
            Else
                tbFirstNew.Style.Item("Display") = "none"
                tbBudget.Style.Item("Display") = ""

                rfv_txtNewBudgetDt.EnableClientScript = True
                rfv_txtNewS.Enabled = False
                rfv_txtNewE.Enabled = False

                lblCBudgetS.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, drw.Table.Rows(0)("CM_BUDGET_FROM_DATE"))
                lblCBudgetE.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, drw.Table.Rows(0)("CM_BUDGET_TO_DATE"))

                'extent period is the lblCBudgetE 
                txtExt.Text = lblCBudgetE.Text
                'start New budget period is start of next year
                txtNewBudgetS.Text = DateAdd(DateInterval.Day, 1, Convert.ToDateTime(lblCBudgetE.Text))
                'end New budget period is end of next year
                txtNewBudgetDt.Text = DateAdd(DateInterval.Year, 1, Convert.ToDateTime(lblCBudgetE.Text))

                If WithinPeriod(lblCBudgetE.Text) Then
                    cmdSaveEndDT.Attributes.Add("Onclick", "return CustomMsg('InBudgetPeriod');")
                    cmdSaveNewPeriod.Attributes.Add("Onclick", "return CustomMsg('StartBudgetPeriod');")
                End If
            End If

            'Chk for previous saved BCM summary
            Dim sSelectedFile As String
            Dim objFileMgmt As New FileManagement
            Dim strTempPath As String = Server.MapPath(objFileMgmt.getSystemParam("ViewBudgetSummary", "Previous"))

            If Directory.Exists(strTempPath & "Saved\") Then
                Dim objDir As Directory
                Dim dCreationDate As Date
                For Each strFile As String In objDir.GetFiles(strTempPath & "Saved\", Session("CompanyId") & "-*.xls")

                    If DateDiff(DateInterval.Second, dCreationDate, File.GetCreationTime(strFile)) >= 0 Then
                        dCreationDate = File.GetCreationTime(strFile)
                        sSelectedFile = strFile
                    End If

                Next
                lnkDownloadPre.Visible = True
                Dim spFileName
                spFileName = Split(sSelectedFile, "\")
                viewstate("PreSummary") = sSelectedFile
                If spFileName.length > 1 Then
                    lnkDownloadPre.Text = "Download " & spFileName(UBound(spFileName)) & " (previous period report)"
                End If
            End If

        End If
    End Sub

    Private Sub cmdSaveMode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveMode.Click
        Dim objBCM As New BudgetControl

        objBCM.UpdateBCMMode(rdMode.SelectedValue)
        Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        objBCM = Nothing
    End Sub

    Private Function isDateValid(ByVal pDateF As String, ByVal pDateT As String) As Boolean
        'isDateValid = True

        'Dim strCurrentDate As String
        'Dim strExtDateTo As String

        'strCurrentDate = lblCBudgetE.Text.Substring(6, 4) & lblCBudgetE.Text.Substring(3, 2) & lblCBudgetE.Text.Substring(0, 2)
        'strExtDateTo = txtExt.Text.Substring(6, 4) & txtExt.Text.Substring(3, 2) & txtExt.Text.Substring(0, 2)
        'If strExtDateTo < strCurrentDate Then
        '    isDateValid = False
        'End If

        'kk.Remark.better date comparison 
        Dim dEndDate As Date
        Dim dExtDate As Date
        If pDateF <> "" And pDateT <> "" Then
            dEndDate = Convert.ToDateTime(pDateF)
            dExtDate = Convert.ToDateTime(pDateT)

            If DateDiff(DateInterval.DayOfYear, dEndDate, dExtDate) > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If



    End Function
    Private Sub cmdSaveEndDT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveEndDT.Click
        Dim objBCM As New BudgetControl

        If isDateValid(lblCBudgetE.Text, txtExt.Text) Then
            If objBCM.UpdateBCMEndDt(lblCBudgetS.Text) AndAlso objBCM.UpdateBCMEndDt(txtExt.Text) Then
                Populate()
            End If
            Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        Else
            lblmsgEDate.Text = "Invalid Extend Date."
        End If
        objBCM = Nothing
    End Sub

    Private Sub cmdSaveNewPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveNewPeriod.Click
        Dim objBCM As New BudgetControl
        Dim TempFileName As String

        If isDateValid(txtNewBudgetS.Text, txtNewBudgetDt.Text) Then
            If CheckIsFileExist(TempFileName) = True Then
                Common.NetMsgbox(Me, TempFileName & " already exists.", MsgBoxStyle.Information)
                Exit Sub
            Else
                DownloadBCM(False)
                If objBCM.UpdateBCMStartDt(txtNewBudgetS.Text) AndAlso objBCM.UpdateBCMEndDt(txtNewBudgetDt.Text) Then
                    objBCM.ResetBCM()
                    Populate()
                End If
            End If


        Else
            '*****************meilai 17/2/2005**********************
            'lblNewBud.Text = "Invalid budget Date."
            lblNewBud.Text = "End date must > Start date."
            '*****************meilai********************************
        End If
        objBCM = Nothing
    End Sub

    Private Function CheckIsFileExist(ByRef strFile As String) As Boolean
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim dBcmStart, dBcmEnd As Date
        Dim TempFileName As String

        CheckIsFileExist = False
        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))
        dBcmStart = objCompDetails.BCMStart
        dBcmEnd = objCompDetails.BCMEnd
        TempFileName = objCompDetails.CoyId & "-" & Format(dBcmEnd, "ddMMMyy") & ".xls"
        strFile = TempFileName
        objComp = Nothing
        objCompDetails = Nothing

        Dim objFileMgmt As New FileManagement
        Dim strTempPath As String = Server.MapPath(objFileMgmt.getSystemParam("ViewBudgetSummary", "Previous"))

        If (Not Directory.Exists(strTempPath)) Then
            Directory.CreateDirectory(strTempPath)
        End If

        If (Not Directory.Exists(strTempPath & "Temp\")) Then
            Directory.CreateDirectory(strTempPath & "\Temp\")
        End If

        If (Not Directory.Exists(strTempPath & "Saved\")) Then
            Directory.CreateDirectory(strTempPath & "\Saved\")
        End If

        If Not File.Exists(strTempPath & "Summary.xls") Then
            Common.NetMsgbox(Me, "Template not found.", MsgBoxStyle.Information)
            Exit Function
        End If

        Dim objDir As Directory
        If Directory.Exists(strTempPath & "Saved\") Then
            If File.Exists(strTempPath & "Saved\" & TempFileName) Then
                CheckIsFileExist = True
            End If
        End If

    End Function

    Private Sub cmdStartNewBCM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStartNewBCM.Click
        Dim objBCM As New BudgetControl
        If isDateValid(txtNewS.Text, txtNewE.Text) Then
            If objBCM.UpdateBCMStartDt(txtNewS.Text) AndAlso objBCM.UpdateBCMEndDt(txtNewE.Text) Then
                Populate()
            End If
            Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        Else
            '*******************meilai 14/2/2005 change the error msg***********
            'lblMsg.Text = "Invalid budget Date."
            lblMsg.Text = "End date must > Start date."
            '*******************meilai******************************************
        End If
        objBCM = Nothing
    End Sub

    Private Function WithinPeriod(ByVal pFrom As String)
        Dim dFrom As Date

        If pFrom <> "" Then
            dFrom = Convert.ToDateTime(pFrom)

            If DateDiff(DateInterval.DayOfYear, Today, dFrom) >= 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    'Private Function DownloadBCM(ByVal pIsTemp As Boolean) As Boolean
    '    Dim dt As New DataTable
    '    Dim objExcel As New AppExcel
    '    Dim objBCM As New BudgetControl

    '    dt = objBCM.GetViewAll()

    '    'dgtest.DataSource = dt.DefaultView
    '    'dgtest.DataBind()

    '    'Retrieve Company details
    '    Dim objComp As New Companies
    '    Dim objCompDetails As New Company
    '    Dim dBcmStart, dBcmEnd As Date
    '    Dim strCurr, TempFileName As String

    '    objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))
    '    dBcmStart = objCompDetails.BCMStart
    '    dBcmEnd = objCompDetails.BCMEnd
    '    strCurr = objCompDetails.Currency
    '    TempFileName = "ContractCatalogueListingTest.xls" 'objCompDetails.CoyId & "-" & IIf(pIsTemp, Format(Now, "ddMMyy-HHmmss"), Format(dBcmEnd, "ddMMMyy")) & ".xls"
    '    objComp = Nothing
    '    objCompDetails = Nothing

    '    'Check File and diredtory exist
    '    Dim objFileMgmt As New FileManagement
    '    Dim strTempPath As String = Server.MapPath(objFileMgmt.getSystemParam("ViewBudgetSummary", IIf(pIsTemp, "Temp", "Previous")))

    '    If (Not Directory.Exists(strTempPath)) Then
    '        Directory.CreateDirectory(strTempPath)
    '    End If

    '    If (Not Directory.Exists(strTempPath & "Temp\")) Then
    '        Directory.CreateDirectory(strTempPath & "\Temp\")
    '    End If

    '    If (Not Directory.Exists(strTempPath & "Saved\")) Then
    '        Directory.CreateDirectory(strTempPath & "\Saved\")
    '    End If

    '    If Not File.Exists(strTempPath & "Summary.xls") Then
    '        Common.NetMsgbox(Me, "Template not found.", MsgBoxStyle.Information)
    '        Exit Function
    '    End If

    '    If pIsTemp Then
    '        '            File.Copy(strTempPath & "Summary.xls", strTempPath & "Temp\" & TempFileName)
    '        File.Copy(strTempPath & "ContractCatalogueListing.xls", strTempPath & "Temp\" & TempFileName)
    '        OpenConnToExcel(strTempPath & "Temp\" & TempFileName, False)
    '    Else
    '        File.Copy(strTempPath & "Summary.xls", strTempPath & "Saved\" & TempFileName)
    '        OpenConnToExcel(strTempPath & "Saved\" & TempFileName, False)

    '    End If


    '    Dim SqlAry(0) As String
    '    Dim lsSql As String
    '    Dim intLoop, intLoop1, intTotCol, TotalRow, intTotRow As Integer

    '    TotalRow = dt.Rows.Count
    '    intTotRow = dt.Rows.Count + 1
    '    intTotCol = dt.Columns.Count

    '    If TotalRow > 0 Then
    '        For intLoop = 2 To intTotRow
    '            lsSql = "Insert into [ContractCatalogue$] Values ('1','Hong Test 004','Hong Test 004','Piece','Malaysian Ringgit','12.5000','17','sSS')" '"Insert into [Sheet1$] Values ('04/05/2012','03/05/2013','DSB     ','DSB0000002','n/a','n/a','MYR','0.0000','1,500,000.00','1,500,000.00','0.00','0.00','0.00')"
    '            Common.Insert2Ary(SqlAry, lsSql)
    '        Next
    '        'For intLoop = 0 To TotalRow - 1
    '        '    '   lsSql = "Insert into [Sheet1$A" & intLoop + 1 & ":L" & intLoop + 1 & "] Values ('" & dBcmStart & "','" & dBcmEnd & "',"
    '        '    'lsSql = "Insert into [Sheet1$A" & intLoop + 1 & ":M" & intLoop + 1 & "] Values ('" & dBcmStart & "','" & dBcmEnd & "',"
    '        '    lsSql = "Insert into [Sheet1$] Values ('" & dBcmStart & "','" & dBcmEnd & "',"

    '        '    With dt.Rows(intLoop)
    '        '        For intLoop1 = 0 To intTotCol - 1
    '        '            If intLoop1 = 4 Then 'Currency column
    '        '                lsSql &= "'" & strCurr & "',"
    '        '            End If
    '        '            If intLoop1 = 4 Then 'BCF column
    '        '                lsSql &= "'" & .Item("AM_BCF") & "',"
    '        '            End If
    '        '            If intLoop1 = 6 Then 'Operating Budget column
    '        '                Dim dblOp As Double = .Item("AM_INIT_BUDGET") - (.Item("AM_RESERVED_AMT") - .Item("AM_COMMITTED_AMT") - .Item("AM_UTILIZED_AMT") + IIf(IsDBNull(.Item("AM_BCF")), 0, .Item("AM_BCF")))
    '        '                lsSql = lsSql & "'" & Format(dblOp, "##,##0.00") & "',"
    '        '            End If
    '        '            If IsDBNull(.ItemArray(intLoop1)) Then
    '        '                lsSql = lsSql & "'n/a',"
    '        '            Else
    '        '                If intLoop1 <= 3 Then
    '        '                    lsSql &= "'" & Common.Parse(.ItemArray(intLoop1)) & "',"
    '        '                Else
    '        '                    If intLoop1 <> 5 Then
    '        '                        If intLoop1 <> 8 Then
    '        '                            lsSql &= "'" & Format(.ItemArray(intLoop1), "##,##0.00") & "',"
    '        '                        Else
    '        '                            lsSql &= "'" & Format(.ItemArray(intLoop1), "##,##0.00") & "'"
    '        '                        End If
    '        '                    End If
    '        '                End If
    '        '            End If

    '        '        Next
    '        '        ' Dim dblOp As Double = .Item("AM_INIT_BUDGET") - (.Item("AM_RESERVED_AMT") - .Item("AM_COMMITTED_AMT") - .Item("AM_UTILIZED_AMT") + .Item("AM_BCF"))
    '        '        'lsSql = lsSql & "'" & Format(dblOp, "##,##0.00") & "')"
    '        '        lsSql = lsSql & ")"
    '        '    End With
    '        '    Common.Insert2Ary(SqlAry, lsSql)
    '        'Next

    '        objDb.BatchExecuteOle(SqlAry)
    '        objDb = Nothing

    '        If pIsTemp Then
    '            Session.Add("FilePath", strTempPath & "Temp\" & TempFileName)
    '            Response.Redirect(dDispatcher.direct("Initial", "FileDownload1.aspx", "PageId=" & strPageId))
    '            ' remark by ai chu
    '            'Dim filename As String = Path.GetFileName(strTempPath & "Temp\" & TempFileName)
    '            'Response.Clear()
    '            'Response.ContentType = "application/octet-stream"
    '            'Response.AddHeader("Content-Disposition", _
    '            '  "attachment; filename=""" & "BCM-Download.xls" & """")
    '            'Response.Flush()
    '            'Response.WriteFile(strTempPath & "Temp\" & TempFileName)
    '        End If
    '    Else
    '        '***************meilai 14/2/2005 if the company dont have account code*************************************
    '        Common.NetMsgbox(Me, "No data to be downloaded.", MsgBoxStyle.Information)
    '        '***************meilai***********************************************
    '        Exit Function
    '    End If
    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()

    'End Function

    Private Function DownloadBCM(ByVal pIsTemp As Boolean) As Boolean
        Dim dt As New DataTable
        Dim objExcel As New AppExcel
        Dim objBCM As New BudgetControl

        dt = objBCM.GetViewAll()

        'dgtest.DataSource = dt.DefaultView
        'dgtest.DataBind()

        'Retrieve Company details
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        Dim dBcmStart, dBcmEnd As Date
        Dim strCurr, TempFileName As String

        objCompDetails = objComp.GetCompanyDetails(Session("CompanyId"))
        dBcmStart = objCompDetails.BCMStart
        dBcmEnd = objCompDetails.BCMEnd
        strCurr = objCompDetails.Currency
        TempFileName = objCompDetails.CoyId & "-" & IIf(pIsTemp, Format(Now, "ddMMyy-HHmmss"), Format(dBcmEnd, "ddMMMyy")) & ".xls"
        objComp = Nothing
        objCompDetails = Nothing

        'Check File and diredtory exist
        Dim objFileMgmt As New FileManagement
        Dim strTempPath As String = Server.MapPath(objFileMgmt.getSystemParam("ViewBudgetSummary", IIf(pIsTemp, "Temp", "Previous")))

        If (Not Directory.Exists(strTempPath)) Then
            Directory.CreateDirectory(strTempPath)
        End If

        If (Not Directory.Exists(strTempPath & "Temp\")) Then
            Directory.CreateDirectory(strTempPath & "\Temp\")
        End If

        If (Not Directory.Exists(strTempPath & "Saved\")) Then
            Directory.CreateDirectory(strTempPath & "\Saved\")
        End If

        If Not File.Exists(strTempPath & "Summary.xls") Then
            Common.NetMsgbox(Me, "Template not found.", MsgBoxStyle.Information)
            Exit Function
        End If

        Dim objDir As Directory
        If pIsTemp Then
            File.Copy(strTempPath & "Summary.xls", strTempPath & "Temp\" & TempFileName)
            OpenConnToExcel(strTempPath & "Temp\" & TempFileName, False)
        Else
            File.Copy(strTempPath & "Summary.xls", strTempPath & "Saved\" & TempFileName)
            OpenConnToExcel(strTempPath & "Saved\" & TempFileName, False)
        End If

        Dim SqlAry(0) As String
        Dim lsSql As String
        Dim intLoop, intLoop1, intTotCol, TotalRow As Integer

        TotalRow = dt.Rows.Count
        intTotCol = dt.Columns.Count

        If TotalRow > 0 Then
            For intLoop = 0 To TotalRow - 1
                '   lsSql = "Insert into [Sheet1$A" & intLoop + 1 & ":L" & intLoop + 1 & "] Values ('" & dBcmStart & "','" & dBcmEnd & "',"
                'lsSql = "Insert into [Sheet1$A" & intLoop + 1 & ":M" & intLoop + 1 & "] Values ('" & dBcmStart & "','" & dBcmEnd & "',"
                lsSql = "Insert into [Sheet1$] Values ('" & dBcmStart & "','" & dBcmEnd & "',"

                With dt.Rows(intLoop)
                    For intLoop1 = 0 To intTotCol - 1
                        If intLoop1 = 4 Then 'Currency column
                            lsSql &= "'" & strCurr & "',"
                        End If
                        If intLoop1 = 4 Then 'BCF column
                            lsSql &= "'" & .Item("AM_BCF") & "',"
                        End If
                        If intLoop1 = 6 Then 'Operating Budget column
                            Dim dblOp As Double = .Item("AM_INIT_BUDGET") - .Item("AM_RESERVED_AMT") - .Item("AM_COMMITTED_AMT") - .Item("AM_UTILIZED_AMT") + IIf(IsDBNull(.Item("AM_BCF")), 0, .Item("AM_BCF"))
                            lsSql = lsSql & "'" & Format(dblOp, "##,##0.00") & "',"
                        End If
                        If IsDBNull(.ItemArray(intLoop1)) Then
                            lsSql = lsSql & "'n/a',"
                        Else
                            If intLoop1 <= 3 Then
                                lsSql &= "'" & Common.Parse(.ItemArray(intLoop1)) & "',"
                            Else
                                If intLoop1 <> 5 Then
                                    If intLoop1 <> 8 Then
                                        lsSql &= "'" & Format(.ItemArray(intLoop1), "##,##0.00") & "',"
                                    Else
                                        lsSql &= "'" & Format(.ItemArray(intLoop1), "##,##0.00") & "'"
                                    End If
                                End If
                            End If
                        End If

                    Next
                    ' Dim dblOp As Double = .Item("AM_INIT_BUDGET") - (.Item("AM_RESERVED_AMT") - .Item("AM_COMMITTED_AMT") - .Item("AM_UTILIZED_AMT") + .Item("AM_BCF"))
                    'lsSql = lsSql & "'" & Format(dblOp, "##,##0.00") & "')"
                    lsSql = lsSql & ")"
                End With
                Common.Insert2Ary(SqlAry, lsSql)
            Next

            objDb.BatchExecuteOle(SqlAry)
            objDb = Nothing

            If pIsTemp Then
                Session.Add("FilePath", strTempPath & "Temp\" & TempFileName)
                Response.Redirect(dDispatcher.direct("Initial", "FileDownload1.aspx", "PageId=" & strPageId))

            End If
        Else
            '***************meilai 14/2/2005 if the company dont have account code*************************************
            Common.NetMsgbox(Me, "No data to be downloaded.", MsgBoxStyle.Information)
            '***************meilai***********************************************
            Exit Function
        End If
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Function

    Private Sub cmdDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownload.Click
        'Temp excel file will be delete after 7 days.
        Dim objDir As Directory
        Dim objFileMgmt As New FileManagement
        Dim strTempPath As String = Server.MapPath(objFileMgmt.getSystemParam("ViewBudgetSummary", "Temp"))
        If Directory.Exists(strTempPath & "Temp\") Then
            For Each strFile As String In objDir.GetFiles(strTempPath & "Temp\")
                If DateDiff(DateInterval.Day, File.GetCreationTime(strFile), Today) > 7 Then
                    File.Delete(strFile)
                End If
            Next
        End If
        DownloadBCM(True)
    End Sub

    Public Function OpenConnToExcel(ByVal pFilePath As String, Optional ByVal pHeader As Boolean = False) As Boolean
        Dim strMassage As String
        Dim sConn As String
        Try
            If Right(pFilePath, 3) <> "xls" Then
                strMassage = "Target file is expecting excel file format."
                Return False
            End If

            sConn = " Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                 " Data Source=" & pFilePath & ";" & _
                                 " Extended Properties=""Excel 8.0;HDR=NO"""

            If pHeader Then sConn = Replace(sConn, "HDR=NO", "HDR=YES")
            objDb.gstrConnOle = sConn

            If objDb.ConnStateOle = False Then
                strMassage = "Connection Failed."
            Else
                Return True
            End If
        Catch exp As Exception
            Common.TrwExp(exp, sConn)
        Finally
            '//THIS MUST..SO THE EXCEL FILE CAN OPEN IN ANY CASE
            objDb.DisConnOle()
            '//
            ' objDcom.DisConn()

        End Try
    End Function

    Private Sub lnkDownloadPre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkDownloadPre.Click
        If viewstate("PreSummary") <> "" Then
            Session.Add("FilePath", viewstate("PreSummary"))
            Response.Redirect(dDispatcher.direct("Initial", "FileDownload1.aspx", "PageId=" & strPageId))
            ' remark by ai chu
            'Dim filename As String = Path.GetFileName(viewstate("PreSummary"))
            'Response.Clear()
            'Response.ContentType = "application/octet-stream"
            'Response.AddHeader("Content-Disposition", _
            '  "attachment; filename=""" & "BCM-Download.xls" & """")
            'Response.Flush()
            'Response.WriteFile(viewstate("PreSummary"))
        End If
    End Sub
End Class
