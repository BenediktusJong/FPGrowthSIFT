<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
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
        Me.btnObat = New System.Windows.Forms.Button()
        Me.btnMine = New System.Windows.Forms.Button()
        Me.btnDataset = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSupport = New System.Windows.Forms.TextBox()
        Me.txtConfidence = New System.Windows.Forms.TextBox()
        Me.btnData = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnObat
        '
        Me.btnObat.Location = New System.Drawing.Point(196, 102)
        Me.btnObat.Name = "btnObat"
        Me.btnObat.Size = New System.Drawing.Size(79, 34)
        Me.btnObat.TabIndex = 0
        Me.btnObat.TabStop = False
        Me.btnObat.Text = "Kelola Data Item"
        Me.btnObat.UseVisualStyleBackColor = True
        '
        'btnMine
        '
        Me.btnMine.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMine.Location = New System.Drawing.Point(95, 156)
        Me.btnMine.Name = "btnMine"
        Me.btnMine.Size = New System.Drawing.Size(111, 65)
        Me.btnMine.TabIndex = 1
        Me.btnMine.TabStop = False
        Me.btnMine.Text = "Proses Data"
        Me.btnMine.UseVisualStyleBackColor = True
        '
        'btnDataset
        '
        Me.btnDataset.Location = New System.Drawing.Point(26, 102)
        Me.btnDataset.Name = "btnDataset"
        Me.btnDataset.Size = New System.Drawing.Size(79, 34)
        Me.btnDataset.TabIndex = 3
        Me.btnDataset.TabStop = False
        Me.btnDataset.Text = "Kelola Dataset"
        Me.btnDataset.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(22, 62)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 34)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Input Data Obat"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(63, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Minimum Support"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(46, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Minimum Confidence"
        '
        'txtSupport
        '
        Me.txtSupport.Location = New System.Drawing.Point(157, 20)
        Me.txtSupport.Name = "txtSupport"
        Me.txtSupport.Size = New System.Drawing.Size(132, 20)
        Me.txtSupport.TabIndex = 1
        Me.txtSupport.Text = "3"
        '
        'txtConfidence
        '
        Me.txtConfidence.Location = New System.Drawing.Point(157, 46)
        Me.txtConfidence.Name = "txtConfidence"
        Me.txtConfidence.Size = New System.Drawing.Size(132, 20)
        Me.txtConfidence.TabIndex = 2
        Me.txtConfidence.Text = "0.7"
        '
        'btnData
        '
        Me.btnData.Location = New System.Drawing.Point(111, 102)
        Me.btnData.Name = "btnData"
        Me.btnData.Size = New System.Drawing.Size(79, 34)
        Me.btnData.TabIndex = 6
        Me.btnData.TabStop = False
        Me.btnData.Text = "Data Transaksi"
        Me.btnData.UseVisualStyleBackColor = True
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(301, 231)
        Me.Controls.Add(Me.btnData)
        Me.Controls.Add(Me.txtConfidence)
        Me.Controls.Add(Me.txtSupport)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDataset)
        Me.Controls.Add(Me.btnObat)
        Me.Controls.Add(Me.btnMine)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Menu Utama"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnObat As System.Windows.Forms.Button
    Friend WithEvents btnMine As System.Windows.Forms.Button
    Friend WithEvents btnDataset As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSupport As System.Windows.Forms.TextBox
    Friend WithEvents txtConfidence As System.Windows.Forms.TextBox
    Friend WithEvents btnData As System.Windows.Forms.Button

End Class
