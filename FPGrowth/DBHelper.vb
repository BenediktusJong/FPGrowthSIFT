Imports System.Data.SqlClient
Imports System.IO

Public Class DBHelper
    Private connString As String

    ' Constructor to initialize the connection string
    Public Sub New()
        Dim pathEnv As String = Environment.CurrentDirectory
        Dim pathDB As String = Path.Combine(pathEnv, "DBObat.mdf")
        Me.connString = "Data Source=.\SQLEXPRESS;AttachDbFilename=" & pathDB & ";Integrated Security=True;Connect Timeout=30;User Instance=True"
    End Sub

    ' Method to execute a query that returns a data reader
    Public Function ExecuteReaderQuery(ByVal query As String, ByVal parameters As Dictionary(Of String, Object)) As Data.DataTable
        Try
            Using conn As New SqlConnection(connString)
                Using comm As New SqlCommand(query, conn)
                    With comm
                        AddParameters(comm, parameters)
                        conn.Open()
                        Dim dr As SqlDataReader
                        dr = comm.ExecuteReader
                        Dim dt As Data.DataTable = New Data.DataTable()
                        dt.Load(dr)
                        Return dt
                    End With
                End Using
            End Using
        Catch ex As SqlException
            MessageBox.Show(ex.Message.ToString(), "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Function

    ' Method to execute a non-query (INSERT, UPDATE, DELETE)
    Public Sub ExecuteNonQuery(ByVal query As String, ByVal parameters As Dictionary(Of String, Object))
        Try
            Using conn As New SqlConnection(connString)
                Using comm As New SqlCommand(query, conn)
                    With comm
                        AddParameters(comm, parameters)
                        conn.Open()
                        comm.ExecuteNonQuery()
                    End With
                End Using
            End Using
        Catch ex As SqlException
            MessageBox.Show(ex.Message.ToString(), "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Public Function ExecuteReaderQueryNonParam(ByVal query As String) As Data.DataTable
        Try
            Using conn As New SqlConnection(connString)
                Using comm As New SqlCommand(query, conn)
                    conn.Open()
                    Dim dr As SqlDataReader
                    dr = comm.ExecuteReader
                    Dim dt As Data.DataTable = New Data.DataTable()
                    dt.Load(dr)
                    Return dt
                End Using
            End Using
        Catch ex As SqlException
            MessageBox.Show(ex.Message.ToString(), "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Function

    ' Helper method to add parameters to a SqlCommand
    Private Sub AddParameters(ByVal command As SqlCommand, ByVal parameters As Dictionary(Of String, Object))
        If Not parameters.Count = 0 Then
            For Each param In parameters
                command.Parameters.AddWithValue(param.Key, param.Value)
            Next
        End If
    End Sub
End Class
