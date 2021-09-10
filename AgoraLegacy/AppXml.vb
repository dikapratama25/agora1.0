Imports System.IO

Namespace AgoraLegacy
    Public Class AppXml
        Dim strMassage As String

        Public Property Message() As String
            Get
                Message = strMassage
            End Get
            Set(ByVal Value As String)
                strMassage = Value
            End Set
        End Property

        Public Function GetDsFromXML(ByVal pPath As String) As DataSet

            Dim dsRd As New DataSet
            Dim fs As FileStream = New FileStream(pPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim reader As StreamReader = New StreamReader(fs)

            dsRd.ReadXml(reader, XmlReadMode.InferSchema)
            fs.Close()
            Return dsRd
        End Function

        'write XML setting from dataset.
        Public Function SetDsToXML(ByVal pPath As String, ByRef pDs As DataSet) As Boolean
            Try
                SetDsToXML = True
                Dim fs As FileStream = New FileStream(pPath, FileMode.Create, FileAccess.ReadWrite)
                Dim writer As StreamWriter = New StreamWriter(fs)
                pDs.WriteXml(writer)
                writer.Close()
                fs.Close()
            Catch exp As Exception
                Return False
            End Try
        End Function

        'set value to Dataset [suitable for xml setting file]
        Public Function SetValue(ByRef pDs As DataSet,
                                 ByVal pColName As String,
                                 ByVal pValue As String,
                                 Optional ByVal pTable As Object = 0,
                                 Optional ByVal pRows As Long = 0) As Boolean

            If pDs Is Nothing Then Return False
            pDs.Tables(pTable).Rows(pRows).Item(pColName) = pValue
        End Function
    End Class
End Namespace

