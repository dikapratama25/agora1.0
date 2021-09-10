Imports Microsoft.Web.UI.WebControls
Imports AgoraLegacy
Imports eProcure.Component
Public Class CMaterialGroup
    Inherits AgoraLegacy.AppBaseClass
    Dim objCat As New MaterialGrp

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents tvCategory As Microsoft.Web.UI.WebControls.TreeView
    Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidCat As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCatId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtItemId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents hidSelected As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblTest As System.Web.UI.WebControls.Label

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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If IsPostBack Then
            LoadTreeByName()
        Else
            lblTitle.Text = "Material Group Assignment"
            LoadTree()
            cmdSearch.Attributes.Add("onclick", "return checkSearchCriteria();")
        End If
    End Sub

    ' bind child node for selected category
    Private Function LoadTreeByName(Optional ByVal intType As Integer = 0)
        Dim sSelectedNode As String
        Dim strNode() As String
        Dim dsType As DataSet
        Dim dbRowC As DataRow
        Dim tnC As New TreeNode
        Dim tn1 As New TreeNode
        Dim tn2 As New TreeNode
        Dim tn3 As New TreeNode
        Dim tn4 As New TreeNode
        Dim newNode As New TreeNode
        Dim i, count As Integer
        Dim temp() As String
        Dim intLevel, intLevel1, intLevel2, intLevel3, intLevel4 As Integer
        newNode = New TreeNode

        intLevel = 0
        intLevel1 = 0
        intLevel2 = 0
        intLevel3 = 0
        intLevel4 = 0
        Select Case intType
            Case 0
                'viewstate("type") = "0"
                sSelectedNode = tvCategory.SelectedNodeIndex
                If sSelectedNode = "" Then
                    Exit Select
                End If
                newNode = tvCategory.GetNodeFromIndex(sSelectedNode)
                newNode.Expanded = True
                strNode = sSelectedNode.Split(".")
                Select Case strNode.Length
                    Case 1
                        intLevel1 = strNode(0)
                    Case 2
                        intLevel1 = strNode(0)
                        intLevel2 = strNode(1)
                    Case 3
                        intLevel1 = strNode(0)
                        intLevel2 = strNode(1)
                        intLevel3 = strNode(2)
                    Case 4
                        intLevel1 = strNode(0)
                        intLevel2 = strNode(1)
                        intLevel3 = strNode(2)
                        intLevel4 = strNode(3)
                End Select

                If newNode.Nodes.Count = 0 Then
                    dsType = objCat.getCategoryType("0", newNode.ID, "")

                    If Not dsType Is Nothing Then
                        For Each dbRowC In dsType.Tables(0).Rows
                            tnC = New TreeNode
                            tnC.Text = dbRowC("CATEGORY_DESC")
                            tnC.ID = dbRowC("CM_CATEGORY_NAME")
                            If dbRowC("CM_CATEGORY_TYPE") = "COM" Then ' level 4
                                intLevel = 4
                                tnC.ImageUrl = "../Images/i_uncheck.gif"
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta;font-weight:bold"
                            End If
                            If dbRowC("CM_CATEGORY_TYPE") = "CLS" Then ' level 3
                                intLevel = 3
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green;font-weight:bold"
                            End If
                            If dbRowC("CM_CATEGORY_TYPE") = "FAM" Then ' level 2
                                intLevel = 2
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue;font-weight:bold"
                            End If
                            newNode.Nodes.Add(tnC)

                            If ViewState("selectedId") <> "" Then
                                If tnC.ID = ViewState("selectedId") Or
                                    (tnC.ID.Substring(0, 6) = CStr(ViewState("selectedId")).Substring(0, 6) And tnC.ID.Substring(6, 2) = "00") Or
                                    (tnC.ID.Substring(0, 4) = CStr(ViewState("selectedId")).Substring(0, 4) And tnC.ID.Substring(4, 4) = "0000") Or
                                    (tnC.ID.Substring(0, 2) = CStr(ViewState("selectedId")).Substring(0, 2) And tnC.ID.Substring(2, 6) = "000000") Then
                                    reloadTree(tnC, intLevel, intLevel1, intLevel2, intLevel3)
                                End If
                            End If
                        Next
                    End If
                    dbRowC = Nothing
                End If

            Case 1 ' with search criteria
                ViewState("type") = "1"
                tvCategory.Nodes.Clear()
                dsType = objCat.getCategoryType("1", txtItemId.Text, txtDesc.Text)

                If Not dsType Is Nothing Then
                    For Each dbRowC In dsType.Tables(0).Rows
                        Select Case dbRowC("CM_CATEGORY_TYPE")
                            Case "SEG" ' 1st level
                                tn1 = New TreeNode
                                tn1.Text = getBoldDesc(txtDesc.Text, dbRowC("CATEGORY_DESC"))
                                tn1.ID = dbRowC("CM_CATEGORY_NAME")
                                tvCategory.Nodes.Add(tn1)
                                tn1.Expanded = True

                            Case "FAM" ' 2nd level
                                tn2 = New TreeNode
                                tn2.Text = getBoldDesc(txtDesc.Text, dbRowC("CATEGORY_DESC"))
                                tn2.ID = dbRowC("CM_CATEGORY_NAME")
                                tn2.Expanded = True
                                tn2.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue"
                                tn2.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue;font-weight:bold"
                                tn1.Nodes.Add(tn2)

                            Case "CLS" ' 3rd level
                                tn3 = New TreeNode
                                tn3.Text = getBoldDesc(txtDesc.Text, dbRowC("CATEGORY_DESC"))
                                tn3.ID = dbRowC("CM_CATEGORY_NAME")
                                tn3.Expanded = True
                                tn3.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green"
                                tn3.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green;font-weight:bold"
                                tn2.Nodes.Add(tn3)

                            Case "COM" ' 4th level
                                tn4 = New TreeNode
                                tn4.Text = getBoldDesc(txtDesc.Text, dbRowC("CATEGORY_DESC"))
                                tn4.ID = dbRowC("CM_CATEGORY_NAME")
                                tn4.Expanded = True
                                tn4.ImageUrl = "../Images/i_uncheck.gif"
                                tn4.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta"
                                tn4.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta;font-weight:bold"
                                tn3.Nodes.Add(tn4)

                        End Select
                    Next
                End If
                dbRowC = Nothing

            Case 2 ' rebind grid after click on category (with search criteria)
                sSelectedNode = tvCategory.SelectedNodeIndex
                Dim str() As String
                str = sSelectedNode.Split(".")
                tvCategory.Nodes.Clear()
                LoadTree()

                If newNode.Nodes.Count = 0 Then
                    dsType = objCat.getCategoryType("0", newNode.ID, "")

                    If Not dsType Is Nothing Then
                        For Each dbRowC In dsType.Tables(0).Rows
                            tnC = New TreeNode
                            tnC.Text = dbRowC("CATEGORY_DESC")
                            tnC.ID = dbRowC("CM_CATEGORY_NAME")
                            newNode.Expandable = ExpandableValue.CheckOnce
                            If dbRowC("CM_CATEGORY_TYPE") = "COM" Then
                                tnC.ImageUrl = "../Images/i_uncheck.gif"
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:Magenta;font-weight:bold"
                            End If
                            If dbRowC("CM_CATEGORY_TYPE") = "CLS" Then
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:green;font-weight:bold"
                            End If
                            If dbRowC("CM_CATEGORY_TYPE") = "FAM" Then
                                tnC.DefaultStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue"
                                tnC.SelectedStyle.CssText = "font-size: 8pt;font-family: Verdana;background:white;color:blue;font-weight:bold"
                            End If
                            newNode.Nodes.Add(tnC)
                        Next
                    End If
                    dbRowC = Nothing
                End If
                hidId.Value = ""
        End Select
    End Function

    ' bold the string within search criteria
    Private Function getBoldDesc(ByVal strSearch As String, ByVal strRaw As String) As String
        Dim i, cnt As Integer
        cnt = strSearch.Length
        i = strRaw.ToLower.IndexOf(strSearch.ToLower)
        If i > -1 Then
            getBoldDesc = strRaw.Substring(0, i) & "<B><I>" & strRaw.Substring(i, cnt) & "</I></B>" & strRaw.Substring(i + cnt, strRaw.Length - cnt - i)
        Else
            getBoldDesc = strRaw
        End If
    End Function

    Private Function getRawDesc(ByVal strDesc As String) As String
        Dim i, j, cnt As Integer
        cnt = strDesc.Length
        i = strDesc.IndexOf("<B><I>")
        j = strDesc.IndexOf("</I></B>")
        If i > -1 Then
            getRawDesc = strDesc.Substring(0, i) & strDesc.Substring(i + 6, j - i - 6) & strDesc.Substring(j + 8, strDesc.Length - j - 8)
        Else
            getRawDesc = strDesc
        End If
    End Function

    Private Function LoadTree()
        Dim dsCategory As DataSet
        Dim dbRowP As DataRow
        Dim tnP As New TreeNode
        Dim i As Integer

        dsCategory = objCat.getCategory

        If Not dsCategory Is Nothing Then
            For Each dbRowP In dsCategory.Tables(0).Rows
                tnP = New TreeNode
                tnP.Text = dbRowP("CATEGORY_DESC")
                tnP.ID = dbRowP("CM_CATEGORY_NAME")
                tvCategory.Nodes.Add(tnP)

                If ViewState("selectedId") <> "" Then
                    If tnP.ID = ViewState("selectedId") Or tnP.ID.Substring(0, 2) = CStr(ViewState("selectedId")).Substring(0, 2) Then ' level 1
                        reloadTree(tnP, 1)
                    End If
                End If
            Next
        End If

        dbRowP = Nothing
        If ViewState("selectedId") = "" Then
            tvCategory.SelectedNodeIndex = tvCategory.Nodes.Count - 1
        End If
    End Function

    Private Sub reloadTree(ByVal node As TreeNode, ByVal intLevel As Integer, Optional ByVal intNode1 As Integer = 0, Optional ByVal intNode2 As Integer = 0, Optional ByVal intNode3 As Integer = 0) '
        ' intLevel - number of level
        ' intNode1 - Node Index for Level 1
        ' intNode2 - Node Index for Level 2
        ' intNode3 - Node Index for Level 3
        ' intNode4 - Node Index for Level 4
        Select Case intLevel
            Case 1
                If tvCategory.Nodes.IndexOf(node) > -1 Then
                    tvCategory.SelectedNodeIndex = tvCategory.Nodes.IndexOf(node)
                    LoadTreeByName()
                End If
            Case 2
                If tvCategory.Nodes(intNode1).Nodes.IndexOf(node) > -1 Then
                    tvCategory.SelectedNodeIndex = intNode1 & "." & tvCategory.Nodes(intNode1).Nodes.IndexOf(node)
                    LoadTreeByName()
                End If
            Case 3
                If tvCategory.Nodes(intNode1).Nodes(intNode2).Nodes.IndexOf(node) > -1 Then
                    tvCategory.SelectedNodeIndex = intNode1 & "." & intNode2 & "." & tvCategory.Nodes(intNode1).Nodes(intNode2).Nodes.IndexOf(node)
                    LoadTreeByName()
                End If
            Case 4
                If tvCategory.Nodes(intNode1).Nodes(intNode2).Nodes(intNode3).Nodes.IndexOf(node) > -1 Then
                    tvCategory.SelectedNodeIndex = intNode1 & "." & intNode2 & "." & intNode3 & "." & tvCategory.Nodes(intNode1).Nodes(intNode2).Nodes(intNode3).Nodes.IndexOf(node)
                    LoadTreeByName()
                End If
        End Select
    End Sub

    Private Sub tvCategory_SelectedIndexChange(ByVal sender As Object, ByVal e As Microsoft.Web.UI.WebControls.TreeViewSelectEventArgs) Handles tvCategory.SelectedIndexChange
        Dim str As String = tvCategory.SelectedNodeIndex.ToString
        Dim strAry(), strAry2() As String
        strAry = str.Split(".")
        If strAry.Length = 4 Then ' select the innest node
            tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).Nodes(strAry(2)).Nodes(strAry(3)).ImageUrl = "../Images/i_check.gif"
            If hidId.Value <> "" Then
                ' clear checked item
                strAry2 = hidId.Value.Split(",")
                tvCategory.Nodes(strAry2(0)).Nodes(strAry2(1)).Nodes(strAry2(2)).Nodes(strAry2(3)).ImageUrl = "../Images/i_uncheck.gif"
            End If
            ' store selected index
            hidId.Value = strAry(0) & "," & strAry(1) & "," & strAry(2) & "," & strAry(3)
            hidCatId.Value = tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).Nodes(strAry(2)).Nodes(strAry(3)).ID.ToString()
            hidCat.Value = getRawDesc(tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).Nodes(strAry(2)).Nodes(strAry(3)).Text.ToString())
        Else
            If ViewState("type") <> "1" Then
                closeTree(str)
                hidSelected.Value = str
            Else
                txtItemId.Text = ""
                txtDesc.Text = ""
                ViewState("selectedId") = getCategoryId(strAry)
                LoadTreeByName(2)
            End If
        End If
    End Sub

    Private Function getCategoryId(ByVal strAry() As String) As String
        Select Case strAry.Length
            Case 1
                getCategoryId = tvCategory.Nodes(strAry(0)).ID.ToString()
            Case 2
                getCategoryId = tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).ID.ToString()
            Case 3
                getCategoryId = tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).Nodes(strAry(2)).ID.ToString()
            Case 4
                getCategoryId = tvCategory.Nodes(strAry(0)).Nodes(strAry(1)).Nodes(strAry(2)).Nodes(strAry(3)).ID.ToString()
        End Select
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        hidId.Value = ""
        LoadTreeByName(1)
    End Sub

    Private Sub closeTree(ByVal strIndex As String)
        Dim strAry(), strAry2() As String
        Dim i As Integer
        strAry = strIndex.Split(".")
        strAry2 = hidSelected.Value.Split(".")
        If strAry2.Length = 1 Then
            If strAry2(0) <> strAry(0) And strAry2(0) <> "" Then
                tvCategory.Nodes(strAry2(0)).Expanded = False
            End If
        ElseIf strAry2.Length = 2 Then
            If strAry2(0) <> strAry(0) Then
                tvCategory.Nodes(strAry2(0)).Expanded = False
            ElseIf strAry.Length = 1 Then
                tvCategory.Nodes(strAry2(0)).Expanded = False
            ElseIf strAry.Length = 2 Then
                If strAry2(1) <> strAry(1) Then
                    tvCategory.Nodes(strAry2(0)).Nodes(strAry2(1)).Expanded = False
                End If
            End If
        ElseIf strAry2.Length = 3 Then
            If strAry2(0) <> strAry(0) Then
                tvCategory.Nodes(strAry2(0)).Expanded = False
            ElseIf strAry.Length = 1 Then
                tvCategory.Nodes(strAry2(0)).Expanded = False
            ElseIf strAry.Length = 2 Then
                If strAry2(1) <> strAry(1) Then
                    tvCategory.Nodes(strAry2(0)).Nodes(strAry2(1)).Expanded = False
                End If
            ElseIf strAry.Length = 3 Then
                If strAry2(2) <> strAry(2) Then
                    tvCategory.Nodes(strAry2(0)).Nodes(strAry2(1)).Nodes(strAry2(2)).Expanded = False
                End If
            End If
        End If
    End Sub

End Class

