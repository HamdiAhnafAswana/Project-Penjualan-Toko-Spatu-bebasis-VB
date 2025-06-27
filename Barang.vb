Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Public Class Barang
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

    Private Sub Barang_load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call koneksi()
        da = New MySqlDataAdapter("SELECT * FROM tbbarang", Conn)
        ds = New DataSet()
        da.Fill(ds, "tbbarang")
        DataGridView1.DataSource = ds.Tables("tbbarang")
    End Sub
    Public Sub TambahkandataGridView(idbrg As String, Namabrg As String, harga As String, ukuran As String, stok As String)
        Dim newRow As DataRow = ds.Tables("dbkasir").NewRow()
        newRow("id_brg") = idbrg
        newRow("Nama_brg") = Namabrg
        newRow("harga") = harga
        newRow("ukuran") = ukuran
        newRow("stok") = stok
        ds.Tables("tbbarang").Rows.Add(newRow)
    End Sub

    Private Sub ButtonTambah_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If String.IsNullOrEmpty(TextBox1.Text) OrElse
   String.IsNullOrEmpty(TextBox2.Text) OrElse
   String.IsNullOrEmpty(TextBox3.Text) OrElse
   String.IsNullOrEmpty(TextBox4.Text) OrElse
   String.IsNullOrEmpty(TextBox5.Text) Then

            MessageBox.Show("Harus Isi semua Kolom", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        String.IsNullOrEmpty(Text)
        Dim Stok As Integer
        Dim Harga As Decimal
        If Not Integer.TryParse(TextBox5.Text, Stok) OrElse Stok < 0 Then
            MessageBox.Show("Stok harus berupa angka positif!", "peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Decimal.TryParse(TextBox3.Text, Harga) OrElse Harga < 0 Then
            MessageBox.Show("Harga harus berupa angka positif!", "peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim newRow As DataRow = ds.Tables("tbbarang").NewRow()
        newRow("id_brg") = TextBox1.Text
        newRow("Nama_brg") = TextBox2.Text
        newRow("ukuran") = TextBox4.Text
        newRow("Stok") = Stok
        newRow("Harga") = Harga

        ds.Tables("tbbarang").Rows.Add(newRow)
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()

        DataGridView1.DataSource = ds.Tables("tbbarang")

        Dim builder As New MySqlCommandBuilder(da)
        da.Update(ds, "tbbarang")
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
        DataGridView1.DataSource = ds.Tables("tbbarang")
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim home As New Home()
        home.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim kasir As New kasir()
        kasir.Show()
        Me.Hide()
    End Sub
End Class
