Public Class FormLoading

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub UpdateProgress(ByVal progress As Integer)
        If Me.ProgressBar1.InvokeRequired Then
            Me.ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgress), progress)
        Else
            Me.ProgressBar1.Value = progress
        End If
    End Sub

    Public Event CancelRequested As EventHandler
    Private Sub formLoading_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If (MessageBox.Show("Batalkan operasi?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes) Then
            RaiseEvent CancelRequested(Me, EventArgs.Empty)
        Else
            e.Cancel = True
        End If
    End Sub
End Class