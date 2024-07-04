Public Class FPTree
    Public Property Root As FPNode
    Public Property HeaderTable As Dictionary(Of String, FPNode)

    Public Sub New()
        Me.Root = New FPNode(Nothing, Nothing)
        Me.HeaderTable = New Dictionary(Of String, FPNode)() '???
    End Sub

    ' Memasukkan data transaksi ke dalam tree
    Public Sub InsertTransaction(ByVal transaction As List(Of String))
        Dim currentNode As FPNode = Me.Root ' Insert dimulai dari root
        For Each item As String In transaction
            If currentNode.Children.ContainsKey(item) Then
                currentNode.Children(item).Count += 1
            Else
                Dim newNode As FPNode = New FPNode(item, currentNode)
                currentNode.Children(item) = newNode

                If Me.HeaderTable.ContainsKey(item) Then
                    Dim headerNode As FPNode = Me.HeaderTable(item)
                    While headerNode.NextNode IsNot Nothing
                        headerNode = headerNode.NextNode
                    End While
                    headerNode.NextNode = newNode
                Else
                    Me.HeaderTable(item) = newNode
                End If
            End If
            currentNode = currentNode.Children(item)
        Next

    End Sub
End Class
