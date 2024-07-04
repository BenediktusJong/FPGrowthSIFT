Imports System.IO
Imports Microsoft.Office.Interop.Excel
Imports System.ComponentModel

Public Class formDataset
    Private dbhelper As DBHelper
    Dim bgWorker As bgWorker = New bgWorker(Me)

    Private Sub formDataset_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbhelper = New DBHelper()
        show_data()
    End Sub

    Private selectedFilePath As String = String.Empty
    Private Sub btnFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFile.Click
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Title = "Pilih Data Transaksi Harian"
        openFileDialog.Filter = "Excel Files|*.xlsx*"
        openFileDialog.InitialDirectory = Path.Combine(Path.Combine(Environment.CurrentDirectory, "dataset")) ' Open initial directory in project/dataset folder
        openFileDialog.Multiselect = True
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            selectedFilePath = openFileDialog.FileName
            Dim fileName As String = Path.GetFileName(selectedFilePath)
            lblFileName.Text = fileName
            lblFilepath.Text = selectedFilePath
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Try
            Dim selectedFilePath = getSelectedFilePath()
            Process.Start(selectedFilePath)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        If Not bgWorker.bgworker.IsBusy Then
            bgWorker.judulOperasi = "Tambahkan Data"
            bgWorker.pesanBerhasil = "Penyimpanan data selesai."
            bgWorker.pesanBatal = "Penyimpanan data dibatalkan."
            bgWorker.pesanLoading = "MENAMBAHKAN DATA " & lblFileName.Text.Replace(".xlsx", String.Empty) & "..."

            AddHandler bgWorker.bgworker.DoWork, AddressOf ReadAndInsertData
            AddHandler bgWorker.bgworker.RunWorkerCompleted, AddressOf insertData_Completed
            ' Cek apakah data dengan id LIKE fileName% sudah tersedia. Jika ada maka lewati baca dan input data
            Dim cleanedfilename As String = lblFileName.Text.Replace(".xlsx", String.Empty)
            
            If ((lblFileName.Text = "none") Or (selectedFilePath = String.Empty)) Then
                MessageBox.Show("Tidak ada data yang terpilih", "Insert file error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ElseIf (checkIfDataExists(cleanedfilename)) Then
                MessageBox.Show("Data sudah tersedia, gunakan update untuk mengubah data", "Simpan Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                bgWorker.bgworker.RunWorkerAsync()
            End If
        End If
    End Sub

    ' Hapus data dengan id LIKE fileName% kemudian ReadAndInsertData() ulang
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not bgWorker.bgworker.IsBusy Then
            bgWorker.judulOperasi = "Pembaruan Data"
            bgWorker.pesanBerhasil = "Data berhasil diperbarui."
            bgWorker.pesanBatal = "Pembaruan data dibatalkan."
            bgWorker.pesanLoading = "MEMPERBARUI DATA " & lblFileName.Text.Replace(".xlsx", String.Empty) & "..."

            AddHandler bgWorker.bgworker.DoWork, AddressOf ReadAndInsertData
            AddHandler bgWorker.bgworker.RunWorkerCompleted, AddressOf updateData_Completed
            bgWorker.bgworker.RunWorkerAsync()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim ids() As String = getSelectedGridViewValue()
        Dim jumlahId As Integer = ids.Count

        If (MessageBox.Show("Hapus " & jumlahId & " data yang terpilih ?", String.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes) Then
            Dim jumlahTerhapus As Integer = 0
            For Each id As String In ids
                If Not (String.IsNullOrWhiteSpace(id)) Then
                    If (checkIfDataExists(id) Or checkIfFileExists(id)) Then
                        Delete_Data(id)
                        Delete_File(id)
                        jumlahTerhapus += 1
                    End If
                End If
            Next
            MessageBox.Show(jumlahTerhapus & " data terhapus dari " & jumlahId & " data terpilih.", String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information)
            show_data()
        End If
    End Sub

    Public Sub ReadAndInsertData(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        ' Create a new Excel application instance
        Dim excelApp As New Application()
        Dim workbook As Workbook = Nothing
        Dim worksheet As Worksheet = Nothing

        Dim fileName As String = lblFileName.Text
        Dim cleanedFileName As String = fileName.Replace(".xlsx", String.Empty)

        ' Masukkan file ke database
        insertFile(cleanedFileName, selectedFilePath)

        Try
            ' Open the Excel file
            workbook = excelApp.Workbooks.Open(selectedFilePath)
            ' Get the first worksheet
            worksheet = CType(workbook.Sheets(1), Worksheet)

            ' Check if the headers are correct
            Dim headerId As String = CType(worksheet.Cells(1, 1), Range).Value.ToString().Trim().ToLower()
            Dim headerItem As String = CType(worksheet.Cells(1, 2), Range).Value.ToString().Trim().ToLower()
            If headerId <> "id" OrElse headerItem <> "item" Then
                Throw New InvalidDataException("Invalid headers in Excel file")
            End If

            ' Read data from the Excel file
            Dim rowCount As Integer = worksheet.UsedRange.Rows.Count
            For i As Integer = 2 To rowCount ' Mulai dari row 2 karena row 1 adalah header
                Dim idCell As Range = CType(worksheet.Cells(i, 1), Range)
                Dim itemCell As Range = CType(worksheet.Cells(i, 2), Range)

                Dim id As String = cleanedFileName & idCell.Value.ToString()
                Dim item As String = itemCell.Value.ToString()

                If (String.IsNullOrEmpty(id) Or String.IsNullOrEmpty(item)) Then
                    Throw New InvalidDataException("Data empty at row " & i)
                End If

                If bgWorker.bgworker.CancellationPending Then
                    e.Cancel = True
                    Exit For
                End If

                insertData(id, item) ' Masukkan data ke database

                Dim pp As Double = i / rowCount * 100 'persen progress
                bgWorker.bgworker.ReportProgress(pp)
            Next

        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        Finally
            ' Clean up
            If workbook IsNot Nothing Then
                workbook.Close(False)
                workbook = Nothing
            End If
            If excelApp IsNot Nothing Then
                excelApp.Quit()
                excelApp = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Function checkIfDataExists(ByVal id As String) As Boolean
        Dim queryStr As String = "SELECT 1 FROM [tbTransaksi] WHERE idTransaksi LIKE @id"
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

    Private Function checkIfFileExists(ByVal fileName As String) As Boolean
        Dim queryStr As String = "SELECT 1 FROM [tbFile] WHERE idFile LIKE @fileName"
        Dim params As New Dictionary(Of String, Object) From {
            {"@fileName", fileName & "%"}
            }

        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQuery(queryStr, params)

        If Not (dt.Rows.Count = 0) Then
            Return True ' Data ini sudah tersedia
        Else
            Return False ' Data belum tersedia
        End If
    End Function

    Private Function getSelectedGridViewValue() As String()
        Try
            Dim selectedRows As DataGridViewSelectedRowCollection = gridViewFile.SelectedRows
            Dim selectedRowCount As Integer = selectedRows.Count

            Dim hasil(selectedRowCount - 1) As String
            Dim i As Integer = 0
            For Each row As DataGridViewRow In selectedRows
                If Not row.IsNewRow Then
                    hasil(i) = row.Cells.Item(0).Value
                    i += 1
                End If
            Next

            Return hasil
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function getSelectedFilePath() As String
        Try
            Dim selectedRows As DataGridViewSelectedRowCollection = gridViewFile.SelectedRows
            Dim row As DataGridViewRow = selectedRows.Item(0)
            Dim hasil = row.Cells.Item(1).Value
            Return hasil
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Sub insertData(ByVal id As String, ByVal item As String)
        Dim queryStr As String = "INSERT INTO tbTransaksi (idTransaksi, item) VALUES (@id, @item)"
        Dim params As New Dictionary(Of String, Object) From {
            {"@id", id},
            {"@item", item}
        }
        dbhelper.ExecuteNonQuery(queryStr, params)
    End Sub

    Private Sub insertFile(ByVal fileName As String, ByVal filePath As String)
        If Not (checkIfFileExists(fileName)) Then
            Dim queryStr As String = "INSERT INTO tbFile (idFile, filePath) VALUES (@fileName, @filePath)"
            Dim params As New Dictionary(Of String, Object) From {
                {"@fileName", fileName},
                {"@filePath", filePath}
            }
            dbhelper.ExecuteNonQuery(queryStr, params)
        End If
    End Sub

    Private Function Delete_Data(ByVal id As String)
        Dim queryStr As String = "DELETE FROM tbTransaksi WHERE idTransaksi LIKE @id"
        Dim params As New Dictionary(Of String, Object) From {
            {"@id", id & "%"}
        }
        If Not (String.IsNullOrWhiteSpace(id)) Then
            dbhelper.ExecuteNonQuery(queryStr, params)
        End If
    End Function

    Private Sub Delete_File(ByVal id As String)
        Dim queryStr As String = "DELETE FROM tbFile WHERE idFile LIKE @id"
        Dim params As New Dictionary(Of String, Object) From {
            {"@id", id & "%"}
        }
        If Not (String.IsNullOrWhiteSpace(id)) Then
            dbhelper.ExecuteNonQuery(queryStr, params)
        End If
    End Sub

    Private Sub show_data()
        Dim queryStr As String = "SELECT * FROM tbFile ORDER BY idFile"

        Dim dt = New Data.DataTable
        dt = dbhelper.ExecuteReaderQueryNonParam(queryStr)

        gridViewFile.DataSource = dt
        gridViewFile.Refresh()
    End Sub

    Private Sub insertData_Completed(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        RemoveHandler bgWorker.bgworker.DoWork, AddressOf ReadAndInsertData
        RemoveHandler bgWorker.bgworker.RunWorkerCompleted, AddressOf insertData_Completed
        show_data()
    End Sub

    Private Sub updateData_Completed(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        RemoveHandler bgWorker.bgworker.DoWork, AddressOf ReadAndInsertData
        RemoveHandler bgWorker.bgworker.RunWorkerCompleted, AddressOf updateData_Completed
        show_data()
    End Sub

    Private Sub formDataset_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
        FormMain.Show()
    End Sub
End Class