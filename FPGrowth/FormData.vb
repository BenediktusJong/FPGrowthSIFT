Public Class FormData
    Private dbhelper As DBHelper

    Private Sub FormData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbhelper = New DBHelper
        show_data()
    End Sub

    Private Sub show_data()
        Dim queryStr As String = "SELECT * FROM tbTransaksi"

        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQueryNonParam(queryStr)

        gridTransaksi.DataSource = dt
        gridTransaksi.Refresh()
    End Sub

    Private Sub FormData_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormMain.Show()
        Me.Dispose()
    End Sub
End Class