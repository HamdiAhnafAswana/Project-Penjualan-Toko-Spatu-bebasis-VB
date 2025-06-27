Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Public Class kasir
    Public Conn As New MySqlConnection
    Public da As MySqlDataAdapter
    Public ds As DataSet

    Private Sub koneksi()
        Dim strconn As String
        Try
            strconn = "server=localhost;user=root;password=;database=dbkasir"
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Conn.ConnectionString = strconn
            Conn.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub form1_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call koneksi()
        da = New MySqlDataAdapter("SELECT * FROM tbkasir", Conn)
        ds = New DataSet()
        da.Fill(ds, "tbkasir")
        DataGridView1.DataSource = ds.Tables("tbkasir")
    End Sub
    Public Sub TambahkandataGridView(ID As String, Nama As String)
        Dim newRow As DataRow = ds.Tables("tbkasir").NewRow()
        newRow("id_kasir") = ID
        newRow("nama_kasir") = Nama
        ds.Tables("tbkasir").Rows.Add(newRow)
    End Sub

    Private Sub ButtonTambah_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(TextBox1.Text) OrElse
   String.IsNullOrEmpty(TextBox2.Text) Then

            MessageBox.Show("Harus Isi semua Kolom", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim newRow As DataRow = ds.Tables("tbkasir").NewRow()
        newRow("id_kasir") = TextBox1.Text
        newRow("Nama_kasir") = TextBox2.Text


        ds.Tables("tbkasir").Rows.Add(newRow)
        TextBox1.Clear()
        TextBox2.Clear()


        DataGridView1.DataSource = ds.Tables("tbkasir")

        Dim builder As New MySqlCommandBuilder(da)
        da.Update(ds, "tbkasir")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DataGridView1.CurrentRow IsNot Nothing Then
            Dim result As DialogResult = MessageBox.Show("Apakah Anda yakin inggin Menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                DataGridView1.Rows.Remove(DataGridView1.CurrentRow)
            End If
        Else
            MessageBox.Show("Pilih baris yang akan diHapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        DataGridView1.DataSource = ds.Tables("tbkasir")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim home As New Home()
        home.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim Transaksi As New Transaksi()
        Transaksi.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim Transaksi As New Transaksi()
        Transaksi.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim barang As New Barang()
        barang.Show()
        Me.Hide()
    End Sub
End Class