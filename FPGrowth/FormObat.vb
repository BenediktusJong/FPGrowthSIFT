Imports System.Data.SqlClient

Public Class FormObat
    Private dbhelper As DBHelper

    Private Sub FormObat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbhelper = New DBHelper()
        show_data()
        ComboBox1.SelectedIndex = 0
        listFilter.Items.Clear()
        For Each item In StaticData.listFilterData
            listFilter.Items.Add(item)
        Next
    End Sub

    Private Sub show_data()
        Dim queryStr As String = "SELECT TOP 100 * FROM [tbItem]"
        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQueryNonParam(queryStr)

        gridViewItem.DataSource = dt
        gridViewItem.Refresh()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim kode As String = txtKode.Text
        Dim nama As String = txtNama.Text

        If Not ((kode = String.Empty) Or (nama = String.Empty)) Then
            Dim queryStr As String = "INSERT INTO tbItem (kodeObat, namaObat) VALUES (@kode, @nama)"
            Dim params As New Dictionary(Of String, Object) From {
                {"@kode", kode},
                {"@nama", nama}
            }
            dbhelper.ExecuteNonQuery(queryStr, params)
        Else
            MessageBox.Show("Data kosong", "Tambah data obat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        show_data()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim kode As String = selectedGridItem()

        Dim queryStr As String = "DELETE FROM tbItem WHERE kodeObat = @kode"
        Dim params As New Dictionary(Of String, Object) From {
            {"@kode", kode}
        }
        dbhelper.ExecuteNonQuery(queryStr, params)

        show_data()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim kode As String = txtKode.Text
        Dim nama As String = txtNama.Text

        If Not ((kode = String.Empty) Or (nama = String.Empty)) Then
            If (checkIfDataExists(kode)) Then
                Dim queryStr As String = "UPDATE tbItem SET kodeObat = @kode, namaObat = @nama WHERE kodeObat = @kode"
                Dim params As New Dictionary(Of String, Object) From {
                    {"@kode", kode},
                    {"@nama", nama}
                }
                dbhelper.ExecuteNonQuery(queryStr, params)
            Else
                MessageBox.Show("Data tidak ditemukan", "Update data obat", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Data kosong", "Update data obat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

            show_data()
    End Sub

    Private Function checkIfDataExists(ByVal id As String) As Boolean
        Dim queryStr As String = "SELECT 1 FROM [tbItem] WHERE kodeObat LIKE @id"
        Dim params As New Dictionary(Of String, Object) From {
            {"@id", id & "%"}
            }

        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQuery(queryStr, params)

        If Not (dt.Rows.Count = 0) Then
            Return True ' Data ini sudah tersedia
        Else
            Return False ' Data belum tersedia
        End If
    End Function

    Private Sub txtKode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKode.TextChanged
        Dim kode As String = txtKode.Text

        Dim queryStr As String = "SELECT * FROM tbItem WHERE kodeObat LIKE @kode"
        Dim params As New Dictionary(Of String, Object) From {
            {"@kode", "%" & kode & "%"}
        }
        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQuery(queryStr, params)

        gridViewItem.DataSource = dt
        gridViewItem.Refresh()
    End Sub

    Private Sub txtNama_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNama.TextChanged
        Dim nama As String = txtNama.Text

        Dim queryStr As String = "SELECT * FROM tbItem WHERE namaObat LIKE @nama"
        Dim params As New Dictionary(Of String, Object) From {
            {"@nama", "%" & nama & "%"}
        }
        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQuery(queryStr, params)

        gridViewItem.DataSource = dt
        gridViewItem.Refresh()
    End Sub

    Private Sub FormObat_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StaticData.listFilterData.Clear()
        For Each item In listFilter.Items
            StaticData.listFilterData.Add(item.ToString())
        Next
        Me.Hide()
        FormMain.Show()
    End Sub

    Private Sub listFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listFilter.SelectedIndexChanged
        If (listFilter.SelectedIndex <> -1) Then
            listFilter.Items.RemoveAt(listFilter.SelectedIndex)
        End If
    End Sub

    Private Sub gridViewItem_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridViewItem.CellDoubleClick
        Dim selectedItem As String = selectedGridItem()
        If Not (listFilter.Items.Contains(selectedItem)) Then
            listFilter.Items.Add(selectedGridItem)
        End If
    End Sub

    Function selectedGridItem() As String
        Dim hasil As String = String.Empty
        Try
            hasil = gridViewItem.SelectedRows.Item(0).Cells.Item(0).Value
            Return hasil
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return hasil
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (ComboBox1.SelectedIndex = 0) Then
            listFilter.Enabled = False
        ElseIf (ComboBox1.SelectedIndex = 1) Then
            listFilter.Enabled = True
        End If
    End Sub
End Class