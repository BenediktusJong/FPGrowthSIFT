Imports System.Collections.Generic
Imports Microsoft.Office.Interop.Excel
Imports System.ComponentModel

Public Class FormMain
    Private dbhelper As DBHelper
    Private bgWorkerV As bgWorker = New bgWorker(Me)
    Public generatedRulesRaw As Dictionary(Of List(Of String), List(Of String))
    'use deserialization to save the generated rules
    Dim hasilRules As FormHasil

    Private Sub FormMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbhelper = New DBHelper()
        'show_data()
    End Sub

    Private Sub btnMine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMine.Click
        generatedRulesRaw = New Dictionary(Of List(Of String), List(Of String))

        Dim transactions As List(Of List(Of String)) = ReadTransactionsFromDatabase()

        Dim minSupport As Integer = CType(txtSupport.Text, Integer)
        Dim minConfidence As Double = CType(txtConfidence.Text, Double)

        ' Create the FP-tree
        Dim tree As FPTree = CreateFPTree(transactions, minSupport)

        ' Mine frequent patterns
        Dim patterns As New Dictionary(Of List(Of String), Integer)()
        Dim params As New Dictionary(Of String, Object)
        params("tree") = tree
        params("minSupport") = minSupport
        params("prefix") = New List(Of String)()
        params("patterns") = patterns
        params("minConfidence") = minConfidence

        If Not bgWorkerV.bgworker.IsBusy Then
            bgWorkerV.judulOperasi = "Penggalian Data"
            bgWorkerV.pesanBerhasil = "Penggalian data selesai."
            bgWorkerV.pesanBatal = "Penggalian data dibatalkan."
            bgWorkerV.pesanLoading = "MENGGALI DATA..."

            AddHandler bgWorkerV.bgworker.DoWork, AddressOf bgWorker_DoWork
            AddHandler bgWorkerV.bgworker.RunWorkerCompleted, AddressOf galiData_Completed

            hasilRules = New FormHasil()

            bgWorkerV.bgworker.RunWorkerAsync(params)
        End If
    End Sub

    ' Function to read transactions from a SQL database
    Function ReadTransactionsFromDatabase() As List(Of List(Of String))
        Dim queryStr As String = "SELECT idTransaksi, item FROM [tbTransaksi]"
        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQueryNonParam(queryStr)
        Dim transactions As New Dictionary(Of String, List(Of String))()
        For Each row In dt.Rows
            Dim id As String = row(0)
            Dim item As String = row(1)
            If Not transactions.ContainsKey(id) Then
                transactions(id) = New List(Of String)()
            End If
            transactions(id).Add(item)
        Next

        Dim transactionList As New List(Of List(Of String))()
        For Each transaction As KeyValuePair(Of String, List(Of String)) In transactions
            transactionList.Add(transaction.Value)
        Next

        Return transactionList
    End Function

    Function readTransaction2(ByVal dt As Data.DataTable) As List(Of List(Of String))
        Dim transactionList As New List(Of List(Of String))()
        Dim hasilBaca As New FormHasil

        Dim transactions As New Dictionary(Of String, List(Of String))()
        For Each row In dt.Rows
            Dim id As String = row(0)
            Dim item As String = row(1)
            If Not transactions.ContainsKey(id) Then
                transactions(id) = New List(Of String)()
            End If
            transactions(id).Add(item)
        Next

        ' Convert dictionary to list of transactions
        For Each transaction As List(Of String) In transactions.Values
            transactionList.Add(transaction)
            MsgBox(transaction)
            hasilBaca.listPola.Items.Add(transaction)
        Next
        hasilBaca.Show()

        Return transactionList
    End Function

    Function CreateFPTree(ByVal transactions As List(Of List(Of String)), ByVal minSupport As Integer) As FPTree
        Dim tree As New FPTree()

        ' Count frequency of each item
        Dim itemFrequency As New Dictionary(Of String, Integer)()
        For Each transaction As List(Of String) In transactions
            For Each item As String In transaction
                If itemFrequency.ContainsKey(item) Then
                    itemFrequency(item) += 1
                Else
                    itemFrequency(item) = 1
                End If
            Next
        Next

        ' Filter items that do not meet minSupport
        Dim filteredTransactions As New List(Of List(Of String))()
        For Each transaction As List(Of String) In transactions
            Dim filteredTransaction As New List(Of String)()
            For Each item As String In transaction
                If itemFrequency(item) >= minSupport Then
                    filteredTransaction.Add(item)
                End If
            Next
            'filteredTransaction.sort was here
            If filteredTransaction.Count > 0 Then
                filteredTransaction.Sort(Function(x, y) itemFrequency(y).CompareTo(itemFrequency(x)))
                filteredTransactions.Add(filteredTransaction)
            End If
        Next

        ' Insert filtered transactions into the FP-tree
        For Each transaction As List(Of String) In filteredTransactions
            tree.InsertTransaction(transaction)
        Next

        Return tree
    End Function

    Private Sub bgWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        ' Retrieve parameters
        Dim params As Dictionary(Of String, Object) = CType(e.Argument, Dictionary(Of String, Object))
        Dim tree As FPTree = CType(params("tree"), FPTree)
        Dim minSupport As Integer = CType(params("minSupport"), Integer)
        Dim prefix As List(Of String) = CType(params("prefix"), List(Of String))
        Dim patterns As Dictionary(Of List(Of String), Integer) = CType(params("patterns"), Dictionary(Of List(Of String), Integer))
        Dim minConfidence As Double = CType(params("minConfidence"), Double)

        ' Call the function with the parameters and pass the BackgroundWorker instance
        MinePatterns(tree, minSupport, prefix, patterns, bgWorkerV)
        generatedRulesRaw = GenerateRules(patterns, minConfidence)
    End Sub

    ' Function to mine frequent patterns from the FP-tree
    Sub MinePatterns(ByVal tree As FPTree, ByVal minSupport As Integer, ByVal prefix As List(Of String), ByVal patterns As Dictionary(Of List(Of String), Integer), ByVal bgworker As bgWorker)
        Dim items As List(Of String) = New List(Of String)(tree.HeaderTable.Keys)
        items.Sort()

        Dim progressCount As Integer = 0
        Dim progressTotal As Integer = items.Count
        For Each item As String In items
            Dim newPrefix As New List(Of String)(prefix)
            newPrefix.Add(item)
            Dim support As Integer = 0

            ' Calculate support for the new prefix
            Dim node As FPNode = tree.HeaderTable(item)
            While node IsNot Nothing
                support += node.Count
                node = node.NextNode
            End While

            If support >= minSupport Then
                patterns(newPrefix) = support

                ' Build conditional pattern base
                Dim conditionalPatternBase As New List(Of List(Of String))()
                node = tree.HeaderTable(item)
                While node IsNot Nothing
                    Dim path As New List(Of String)()
                    Dim parentNode As FPNode = node.Parent
                    While parentNode.Item IsNot Nothing
                        path.Insert(0, parentNode.Item)
                        parentNode = parentNode.Parent
                    End While

                    For i As Integer = 0 To node.Count - 1
                        conditionalPatternBase.Add(path)
                    Next
                    node = node.NextNode
                End While

                progressCount += 1
                Dim pp As Double = progressCount / progressTotal * 100 'persen progress
                bgWorkerV.bgworker.ReportProgress(pp)
                ' Recursively mine the conditional FP-tree
                Dim conditionalTree As FPTree = CreateFPTree(conditionalPatternBase, minSupport)
                If conditionalTree.Root.Children.Count > 0 Then
                    MinePatterns(conditionalTree, minSupport, newPrefix, patterns, bgWorkerV)
                End If
            End If
        Next
    End Sub

    ' Function to generate all non-empty subsets of a set
    Function GetSubsets(Of T)(ByVal vset As List(Of T)) As List(Of List(Of T))
        Dim subsets As New List(Of List(Of T))()
        Dim subsetCount As Integer = 2 ^ vset.Count 'Jumlah subset adalah 2^n (jika dihitung dengan subset kosong -> {} )
        For i As Integer = 1 To subsetCount - 1
            Dim subset As New List(Of T)()
            For j As Integer = 0 To vset.Count - 1
                If (i And (1 << j)) <> 0 Then
                    subset.Add(vset(j))
                End If
            Next
            subsets.Add(subset)
        Next
        Return subsets
    End Function

    Function GetSupport(ByVal itemset As List(Of String), ByVal patterns As Dictionary(Of List(Of String), Integer)) As Integer
        For Each pattern As KeyValuePair(Of List(Of String), Integer) In patterns
            If itemset.Count = pattern.Key.Count AndAlso itemset.All(Function(i) pattern.Key.Contains(i)) Then
                Return pattern.Value
            End If
        Next
        Return 0
    End Function

    ' Function to generate association rules and calculate confidence
    Private Function GenerateRules(ByVal patterns As Dictionary(Of List(Of String), Integer), ByVal minConfidence As Double) As Dictionary(Of List(Of String), List(Of String))
        Dim hasil As Dictionary(Of List(Of String), List(Of String)) = New Dictionary(Of List(Of String), List(Of String))
        For Each pattern As KeyValuePair(Of List(Of String), Integer) In patterns
            Dim subsets As List(Of List(Of String)) = GetSubsets(pattern.Key)
            For Each subset As List(Of String) In subsets
                Dim remain As List(Of String) = pattern.Key.Except(subset).ToList()
                Dim premises As List(Of String) = convert_kodeObat(subset.ToArray())
                Dim conclusion As List(Of String) = convert_kodeObat(remain.ToArray())
                If remain.Count > 0 Then
                    Dim subsetSupport As Integer = GetSupport(subset, patterns)
                    Dim confidence As Double = pattern.Value / subsetSupport
                    If confidence >= minConfidence Then
                        hasil.Add(subset, remain)
                        hasilRules.listPola.Items.Add(String.Format("IF [{0}] THEN [{1}]; Confidence: {2:F2}; Support: {3:f2}", String.Join(", ", subset), String.Join(", ", remain), confidence, subsetSupport))
                        hasilRules.listAturan.Items.Add(String.Format("IF [{0}] THEN [{1}]; Confidence: {2:F2}; Support: {3:f2}", String.Join(", ", premises), String.Join(", ", conclusion), confidence, subsetSupport))
                    End If
                End If
            Next
        Next
        Return hasil
    End Function

    Private Function convert_kodeObat(ByVal daftarKode As String()) As List(Of String)
        Dim hasil As List(Of String) = New List(Of String)

        Dim queryStr As String = "SELECT namaObat FROM tbItem WHERE kodeObat = @kode"
        For Each kode As String In daftarKode
            Dim dt = New Data.DataTable
            Dim param As New Dictionary(Of String, Object) From {
                {"@kode", kode}
            }
            dt = dbhelper.ExecuteReaderQuery(queryStr, param)
            If (dt.Rows.Count > 0) Then
                hasil.Add(dt.Rows.Item(0).Item(0).ToString)
            End If
        Next

        Return hasil
    End Function

    Private Sub show_data()
        Dim queryStr As String = "SELECT TOP 50 * FROM [tbTransaksi]"
        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQueryNonParam(queryStr)

        'gridViewTransaksi.DataSource = dt
        'gridViewTransaksi.Refresh()
    End Sub

    Private Sub galiData_Completed(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        RemoveHandler bgWorkerV.bgworker.DoWork, AddressOf bgWorker_DoWork
        RemoveHandler bgWorkerV.bgworker.RunWorkerCompleted, AddressOf galiData_Completed
        hasilRules.Show()
    End Sub

    Private Sub btnDataset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDataset.Click
        formDataset.Show()
        Me.Hide()
    End Sub

    Private Sub btnData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnData.Click
        FormData.Show()
        Me.Hide()
    End Sub

    Private Sub btnObat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnObat.Click
        FormObat.Show()
        Me.Hide()
    End Sub
End Class
