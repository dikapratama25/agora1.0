Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class DOLotAttachment
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDO As New DeliveryOrder
    Dim blnFrmAttchment As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    Protected WithEvents FileDoc As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        Dim objDb As New EAD.DBCom

        ViewState("DONo") = Request.QueryString("DONo")
        ViewState("ItemCode") = objDb.GetVal("SELECT PM_VENDOR_ITEM_CODE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=" & Request.QueryString("itemcode"))
        ViewState("lineNo") = Request.QueryString("lineNo")
        ViewState("poline") = Request.QueryString("poline")
        displayAttachFile()

        'cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & FileDoc.ClientID & "');")

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If FileDoc.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo, strItemCode, strLineNo, strPOline As String
            If ViewState("DONo") <> "" Then
                strDocNo = ViewState("DONo")
            Else
                strDocNo = Session.SessionID
            End If

            strItemCode = ViewState("ItemCode")
            strLineNo = ViewState("lineNo")
            strPOline = ViewState("poline")

            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(FileDoc.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf objDO.chkDupDOAttach(strDocNo, System.IO.Path.GetFileName(FileDoc.PostedFile.FileName), "D", strLineNo, strItemCode, strPOline) Then
                Common.NetMsgbox(Me, "Duplicate File")
            ElseIf FileDoc.PostedFile.ContentLength > 0 And FileDoc.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(FileDoc, EnumUploadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, strDocNo, True, , , , , "D", strItemCode, strLineNo, strPOline)
            ElseIf FileDoc.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
            objFile = Nothing
            objDB = Nothing
        End If
    End Sub

    Private Sub displayAttachFile()
        Dim dsAttach As New DataSet
        Dim drvAttach As New DataView
        Dim i As Integer
        Dim objFile As New FileManagement
        Dim strFile, strFile1, strURL, strTemp As String
        If ViewState("DONo") = "" Then
            dsAttach = objDO.getTempDOAttachment(Session.SessionID, "D", ViewState("ItemCode"), ViewState("lineNo"), ViewState("poline"))
        Else
            dsAttach = objDO.getTempDOAttachment(ViewState("DONo"), "D", ViewState("ItemCode"), ViewState("lineNo"), ViewState("poline"))
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                Dim strFilePath As String
                strFile = drvAttach(i)("CDDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff)

                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                'If cmdsave.Visible Then
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                'Else
                'lnk.Visible = False
                'FileDoc.Disabled = True
                'cmdUpload.Enabled = False
                'End If
                lnk.ID = drvAttach(i)("CDDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttachLot

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        blnFrmAttchment = True
    End Sub

    Private Sub deleteAttachLot(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDocNo, strStatus As String
        If ViewState("DONo") <> "" Then
            strDocNo = ViewState("DONo")
            strStatus = "U" 'ie delete the attachment that is already in the database or delete the new attachment of Draft DO
        Else
            strDocNo = Session.SessionID
            strStatus = "D" 'ie. delete those attachment that has not been updated into the database
        End If

        objDO.deleteTempDOAttachment(CType(sender, ImageButton).ID, strDocNo, "D", strStatus)
        displayAttachFile()
    End Sub
End Class
