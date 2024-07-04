Public Class FPNode
    Public Property Item As String
    Public Property Count As Integer
    Public Property Parent As FPNode
    Public Property Children As Dictionary(Of String, FPNode)
    Public Property NextNode As FPNode

    Public Sub New(ByVal item As String, ByVal parent As FPNode)
        Me.Item = item
        Me.Count = 1
        Me.Parent = parent
        Me.Children = New Dictionary(Of String, FPNode)()
        Me.NextNode = Nothing
    End Sub
End Class
