Public Class FormRekomendasi



    Private Sub FormData_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormMain.Show()
        Me.Dispose()
    End Sub
End Class