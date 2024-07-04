Imports System.ComponentModel
Public Class bgWorker
    Public WithEvents bgworker As New BackgroundWorker
    Private ReadOnly _form As Form
    Public pesanBerhasil As String = "Operasi berhasil."
    Public pesanBatal As String = "Operasi batal."
    Public judulOperasi As String = "Judul operasi."
    Public pesanLoading As String = "Loading..."
    Dim loadingScreen As FormLoading

    Public Sub New(ByVal form As Form)
        _form = form
        bgworker.WorkerReportsProgress = True
        bgworker.WorkerSupportsCancellation = True
    End Sub

    Private Sub bgworker_Start(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles bgworker.DoWork
        loadingScreen = New FormLoading()
        _form.Invoke(Sub()
                         RemoveHandler loadingScreen.CancelRequested, AddressOf OnCancelRequested
                         AddHandler loadingScreen.CancelRequested, AddressOf OnCancelRequested
                         loadingScreen.lblLoading.Text = pesanLoading
                         loadingScreen.Show()
                     End Sub)
    End Sub

    Private Sub bgWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles bgworker.ProgressChanged
        _form.Invoke(Sub()
                         loadingScreen.UpdateProgress(e.ProgressPercentage)
                     End Sub)
    End Sub

    Public Sub OnCancelRequested(ByVal sender As Object, ByVal e As EventArgs)
        bgworker.CancelAsync()
    End Sub

    Private Function bgWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) As RunWorkerCompletedEventArgs Handles bgworker.RunWorkerCompleted
        _form.Invoke(Sub()
                         loadingScreen.Dispose()
                     End Sub)
        If e.Cancelled Then
            MessageBox.Show(pesanBatal, judulOperasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return e
        ElseIf e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message, judulOperasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return e
        Else
            MessageBox.Show(pesanBerhasil, judulOperasi, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return e
        End If
    End Function
End Class
