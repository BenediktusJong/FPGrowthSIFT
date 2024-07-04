Public Class FormHasil
    Private generatedRulesRaws As Dictionary(Of List(Of String), List(Of String))
    Private generatedRules As Dictionary(Of List(Of String), List(Of String))
    Private dbhelper As DBHelper
    Private bgWorkerV As bgWorker = New bgWorker(Me)

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        'generatedRulesRaws = FormMain.generatedRulesRaw

        dbhelper = New DBHelper()
        'show_hasil()
    End Sub

    Private Sub show_hasil()
        Dim generatedRulesConverted As Dictionary(Of List(Of String), List(Of String)) = New Dictionary(Of List(Of String), List(Of String))
        Try
            For Each list As KeyValuePair(Of List(Of String), List(Of String)) In generatedRulesRaws
                'Dim premises As List(Of String) = convert_kodeObat(list.Key.ToArray())
                'Dim conclusion As List(Of String) = convert_kodeObat(list.Value.ToArray())
                Dim premises As List(Of String) = list.Key
                Dim conclusion As List(Of String) = list.Value
                generatedRulesConverted.Add(premises, conclusion)
                'listHasil.Items.Add(String.Format("Rule: {0} => {1}, Confidence: {2:F2}, Support: {3:f2}", String.Join(", ", premises), String.Join(", ", conclusion), confidence, subsetSupport))
                'Console.WriteLine("* Rule:" & vbCrLf & "Premises: {0}" & vbCrLf & "Conclusion: {1}", String.Join(", ", premises.ToArray), String.Join(", ", conclusion.ToArray))
            Next
        Catch e As Exception
        End Try
    End Sub

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

    Private Sub FormRules_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Tutup form?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Me.Dispose()
        End If
    End Sub

    Private Sub Copy_Pola(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPola.Click
        Dim teks As String = String.Join(Environment.NewLine, listPola.Items)
        Clipboard.SetText(teks)
    End Sub

    Private Sub btnPolasatu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPolasatu.Click
        Dim teks As String = listPola.SelectedItem.ToString
        Clipboard.SetText(teks)
    End Sub

    Private Sub Copy_Aturan(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAturan.Click
        Dim teks As String = String.Join(Environment.NewLine, listAturan.Items)
        Clipboard.SetText(teks)
    End Sub

    Private Sub btnAturansatu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAturansatu.Click
        Dim teks As String = listAturan.SelectedItem.ToString
        Clipboard.SetText(teks)
    End Sub

    Private Sub listPola_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listPola.SelectedIndexChanged
        listAturan.SelectedIndex = listPola.SelectedIndex
    End Sub

    Private Sub listAturan_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listAturan.SelectedIndexChanged
        listPola.SelectedIndex = listAturan.SelectedIndex
    End Sub
End Class