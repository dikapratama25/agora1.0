'================================================================================
'Name          : MenuEAD DLL
'Last modified : 03/09/2004
'Version       : 2.0
'Description   : This version of MenuEAD allow to expand more than 9 level at the level 0.
'                but still restrict 9 level in the second level and so on.
'================================================================================

Imports AgoraLegacy
Imports Microsoft.Web.UI.WebControls
Imports System.Configuration

Namespace AgoraLegacy

    Public Class MenuEAD
        Public Shared gConnstr As String
        Private UserID As String
        Private objDcom As EAD.DBCom
        Private strSQL As String
        Private WvTree As TreeView
        Private MenuCollection As New MenuCollection()


        '========================================================================
        'Function: Get Connection
        '========================================================================
        Public Sub New(ByVal pWvTree As TreeView,
                            Optional ByVal pConnstr As String = vbNullString,
                            Optional ByVal pUserID As String = vbNullString)
            gConnstr = pConnstr
            UserID = pUserID
            WvTree = pWvTree

            If gConnstr <> vbNullString Then
                'objDcom.gstrConn = gConnstr
                objDcom = New EAD.DBCom(gConnstr)
            Else
                gConnstr = ConfigurationManager.AppSettings("menu")
                objDcom = New EAD.DBCom(gConnstr)
            End If

        End Sub

        '========================================================================
        'Function: Get length of the level
        '========================================================================
        Public Function GetLen(ByVal pstr As String) As Integer
            Dim ilen
            ilen = Split(pstr, ",").Length
            Return ilen
        End Function

        '========================================================================
        'Function: Get value by index number
        '========================================================================
        Public Function GetIndexValue(ByVal pstr As String, ByVal pIndex As Integer) As String
            Dim sp
            sp = Split(pstr, ",")
            Return sp(pIndex)
        End Function

        '========================================================================
        'Function: Count total node in the tree view
        '========================================================================
        Public Function GetNodesCount() As Integer
            Dim strSQL As String
            Dim iNodeLen As Integer = -1

            strSQL = "SELECT MAX(MM_MENU_PARENT)AS MAX FROM MENU_MSTR"
            Dim tDS As DataSet = objDcom.FillDs(strSQL)
            If (tDS.Tables(0).Rows.Count > 0) Then
                iNodeLen = CInt(tDS.Tables(0).Rows(0).Item("MAX"))
            End If
            Return iNodeLen

        End Function

        Public Function BindCollectionToTreeView(Optional ByVal pConditionNoWhere As String = vbNullString) As Boolean

            'Dim itemNode As New TreeNode()
            Dim curLevel As Long
            Dim perLevel As Long
            Dim strLevel As String
            Dim lngCount, lLoop, jLoop As Long

            For lngCount = 0 To GetNodesCount()
                MenuCollection.Clear()
                MenuCollection = GetMenuData(" WHERE MM_MENU_PARENT=" & lngCount & " " & pConditionNoWhere)
                For lLoop = 0 To MenuCollection.Count - 1
                    'if level 1, not need to bind to any node
                    If GetLen(MenuCollection(lLoop).MM_MENU_LEVEL) = 1 Then
                        AddMenuItem(MenuCollection(lLoop))
                        'objDcom.WriteLog("AddMenuItem: " & MenuCollection(lLoop).MM_MENU_NAME, "c:")
                    Else
                        Dim ary As Array = CType(MenuCollection(lLoop).MM_MENU_PARENT, String).ToCharArray
                        Dim str As String = MenuCollection(lLoop).MM_MENU_LEVEL

                        Dim node As TreeNode = WvTree.Nodes(lngCount)
                        If GetLen(MenuCollection(lLoop).MM_MENU_LEVEL) <> 2 Then

                            For jLoop = 2 To GetLen(str) - 1
                                node = FormatParentNodeItem(node, GetIndexValue(str, jLoop - 1), jLoop)
                            Next

                        End If
                        AddMenuItem(MenuCollection(lLoop), node)
                        'objDcom.WriteLog("Bind to parent AddMenuItem: " & MenuCollection(lLoop).MM_MENU_NAME, "c:")
                    End If
                Next
            Next

        End Function

        Public Function FormatParentNodeItem(ByRef InNode As TreeNode, ByVal iIndex As Long, ByVal iLocation As Long) As TreeNode
            Return InNode.Nodes(iIndex)
        End Function

        Public Function AddMenuItem(ByVal menu As Menu,
                                    Optional ByVal pParentNode As TreeNode = Nothing) As Boolean
            Dim itemNode As New TreeNode()
            itemNode.Text = menu.MM_MENU_NAME
            itemNode.ImageUrl = menu.MM_MENU_IMAGE
            itemNode.NavigateUrl = menu.MM_MENU_URL
            itemNode.ID = menu.MM_MENU_ID
            itemNode.ExpandedImageUrl = menu.MM_MENU_IMAGE_EXP
            itemNode.NodeData = menu.MM_MENU_TIPS
            itemNode.Target = menu.MM_MENU_TARGET

            If pParentNode Is Nothing Then
                WvTree.Nodes.Add(itemNode)
            Else
                pParentNode.Nodes.Add(itemNode)
            End If
        End Function

        Public Function GetMenuData(Optional ByVal pCondition As String = vbNullString) As MenuCollection
            Dim MenuCollection As New MenuCollection()

            strSQL = " SELECT * FROM MENU_MSTR " & pCondition & " ORDER BY MM_MENU_LEVEL,MM_MENU_PARENT "
            Dim tDS As DataSet = objDcom.FillDs(strSQL)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                MenuCollection.Add(PopulateMenuFromReader(tDS, j))
            Next

            Return MenuCollection
        End Function

        Public Function PopulateMenuFromReader(ByVal pRd As DataSet, ByVal j As Integer) As Menu
            Dim Menu As New Menu()
            Menu.MM_MENU_ID = pRd.Tables(0).Rows(j).Item("MM_MENU_ID")
            Menu.MM_MENU_NAME = CStr(pRd.Tables(0).Rows(j).Item("MM_MENU_NAME"))
            Menu.MM_MENU_IMAGE = parseNull(pRd.Tables(0).Rows(j).Item("MM_MENU_IMAGE"))
            Menu.MM_MENU_IMAGE_EXP = parseNull(pRd.Tables(0).Rows(j).Item("MM_MENU_IMAGE_EXP"))
            Menu.MM_MENU_LEVEL = CStr(pRd.Tables(0).Rows(j).Item("MM_MENU_LEVEL"))
            Menu.MM_MENU_PARENT = CStr(pRd.Tables(0).Rows(j).Item("MM_MENU_PARENT"))
            Menu.MM_MENU_URL = parseNull(pRd.Tables(0).Rows(j).Item("MM_MENU_URL"))
            Menu.MM_MENU_TIPS = parseNull(pRd.Tables(0).Rows(j).Item("MM_MENU_TIPS"), "Please Click..")
            Menu.MM_MENU_TARGET = parseNull(pRd.Tables(0).Rows(j).Item("MM_MENU_TARGET"), "main")

            Return Menu
        End Function

        Public Function GetTreeNode(ByVal pNodeId As String) As TreeNode
            Dim tNode As TreeNode
            For Each tNode In WvTree.Nodes
                If tNode.ID = pNodeId Then
                    Return tNode
                End If
                Dim Node As TreeNode = GetNode(pNodeId, tNode)
                If Not Node Is Nothing Then Return Node
            Next

        End Function

        Private Function GetNode(ByVal pNodeId As String, ByVal tNode As TreeNode) As TreeNode
            Dim loopNode As TreeNode
            For Each loopNode In tNode.Nodes
                If loopNode.ID = pNodeId Then
                    Return loopNode
                End If
                Dim Node As TreeNode = GetNode(pNodeId, loopNode)
                If Not Node Is Nothing Then Return Node
            Next
        End Function


        Public Function DeleteNode(ByVal pNodeId As String) As Boolean
            DeleteNode = False
            Dim Node As TreeNode = GetTreeNode(pNodeId)
            If Not Node Is Nothing Then Node.Remove() : Return True
        End Function

        Public Function parseNull(ByVal pIn As Object, Optional ByVal pRplc As String = "") As Object
            If IsDBNull(pIn) Then
                Return pRplc
            Else
                Return pIn
            End If
        End Function
    End Class

End Namespace


#Region " Table Script"

'DROP TABLE MENU_MSTR CASCADE CONSTRAINTS ; 

'CREATE TABLE MENU_MSTR ( 
'  MM_MENU_ID         VARCHAR2 (50)  NOT NULL, 
'  MM_MENU_NAME       VARCHAR2 (100)  NOT NULL, 
'  MM_MENU_IMAGE      VARCHAR2 (2000), 
'  MM_MENU_LEVEL      VARCHAR2 (10)  NOT NULL, 
'  MM_MENU_PARENT     NUMBER (10)   NOT NULL, 
'  MM_MENU_URL        VARCHAR2 (2000), 
'  MM_MENU_IMAGE_EXP  VARCHAR2 (2000), 
'  MM_MENU_TIPS       VARCHAR2 (2000), 
'  MM_GROUP           VARCHAR2 (50), 
'  MM_MENU_TARGET     VARCHAR2 (50), 
'  PRIMARY KEY ( MM_MENU_ID ) )


#End Region