<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormHasil
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.listPola = New System.Windows.Forms.ListBox()
        Me.btnPola = New System.Windows.Forms.Button()
        Me.listAturan = New System.Windows.Forms.ListBox()
        Me.btnAturan = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnPolasatu = New System.Windows.Forms.Button()
        Me.btnAturansatu = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'listPola
        '
        Me.listPola.FormattingEnabled = True
        Me.listPola.Location = New System.Drawing.Point(3, 4)
        Me.listPola.Name = "listPola"
        Me.listPola.Size = New System.Drawing.Size(404, 329)
        Me.listPola.TabIndex = 0
        '
        'btnPola
        '
        Me.btnPola.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPola.Location = New System.Drawing.Point(76, 356)
        Me.btnPola.Name = "btnPola"
        Me.btnPola.Size = New System.Drawing.Size(123, 39)
        Me.btnPola.TabIndex = 1
        Me.btnPola.Text = "Copy semua pola dalam list"
        Me.btnPola.UseVisualStyleBackColor = True
        '
        'listAturan
        '
        Me.listAturan.FormattingEnabled = True
        Me.listAturan.Location = New System.Drawing.Point(427, 4)
        Me.listAturan.Name = "listAturan"
        Me.listAturan.Size = New System.Drawing.Size(404, 329)
        Me.listAturan.TabIndex = 2
        '
        'btnAturan
        '
        Me.btnAturan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAturan.Location = New System.Drawing.Point(507, 356)
        Me.btnAturan.Name = "btnAturan"
        Me.btnAturan.Size = New System.Drawing.Size(123, 39)
        Me.btnAturan.TabIndex = 3
        Me.btnAturan.Text = "Copy semua aturan dalam list"
        Me.btnAturan.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(591, 356)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(123, 39)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Copy semua aturan dalam list"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnPolasatu
        '
        Me.btnPolasatu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPolasatu.Location = New System.Drawing.Point(205, 356)
        Me.btnPolasatu.Name = "btnPolasatu"
        Me.btnPolasatu.Size = New System.Drawing.Size(123, 39)
        Me.btnPolasatu.TabIndex = 4
        Me.btnPolasatu.Text = "Copy pola terpilih"
        Me.btnPolasatu.UseVisualStyleBackColor = True
        '
        'btnAturansatu
        '
        Me.btnAturansatu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAturansatu.Location = New System.Drawing.Point(636, 356)
        Me.btnAturansatu.Name = "btnAturansatu"
        Me.btnAturansatu.Size = New System.Drawing.Size(123, 39)
        Me.btnAturansatu.TabIndex = 5
        Me.btnAturansatu.Text = "Copy aturan terpilih"
        Me.btnAturansatu.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(3, 414)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(404, 20)
        Me.TextBox1.TabIndex = 6
        '
        'FormHasil
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(412, 439)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnAturansatu)
        Me.Controls.Add(Me.btnPolasatu)
        Me.Controls.Add(Me.btnAturan)
        Me.Controls.Add(Me.listAturan)
        Me.Controls.Add(Me.btnPola)
        Me.Controls.Add(Me.listPola)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormHasil"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HASIL PENGGALIAN DATA"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents listPola As System.Windows.Forms.ListBox
    Friend WithEvents btnPola As System.Windows.Forms.Button
    Friend WithEvents listAturan As System.Windows.Forms.ListBox
    Friend WithEvents btnAturan As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnPolasatu As System.Windows.Forms.Button
    Friend WithEvents btnAturansatu As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
