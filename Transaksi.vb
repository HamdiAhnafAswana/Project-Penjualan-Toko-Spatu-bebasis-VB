Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing


Public Class Transaksi
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

    Private Sub Transaksi_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call koneksi()
        da = New MySqlDataAdapter("SELECT * FROM tbbayar", Conn)
        ds = New DataSet()
        da.Fill(ds, "tbbayar")
        DataGridView2.DataSource = ds.Tables("tbbayar")

        ' Mengisi ComboBox1 dengan ID barang
        Dim cmd1 As New MySqlCommand("SELECT * FROM tbbarang", Conn)
        Dim da1 As New MySqlDataAdapter(cmd1)
        Dim dt1 As New DataTable()
        da1.Fill(dt1)
        ComboBox1.DataSource = dt1
        ComboBox1.ValueMember = "id_brg"
        ComboBox1.DisplayMember = "nama_brg"

        ' Mengisi ComboBox2 dengan ID kasir
        Dim cmd2 As New MySqlCommand("SELECT * FROM tbkasir", Conn)
        Dim da2 As New MySqlDataAdapter(cmd2)
        Dim dt2 As New DataTable()
        da2.Fill(dt2)
        ComboBox2.DataSource = dt2
        ComboBox2.ValueMember = "id_kasir"
        ComboBox2.DisplayMember = "nama_kasir"
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedValue IsNot Nothing Then
            Dim cmd As New MySqlCommand("SELECT * FROM tbbarang WHERE id_brg = @id", Conn)
            cmd.Parameters.AddWithValue("@id", ComboBox1.SelectedValue)

            Try
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    TextBox1.Text = reader("nama_brg").ToString()
                    TextBox2.Text = reader("harga").ToString()
                    TextBox3.Text = reader("ukuran").ToString()
                    TextBox4.Text = reader("stok").ToString()
                End If
                reader.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        HitungTotalBayar()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedValue IsNot Nothing Then
            Dim cmd As New MySqlCommand("SELECT * FROM tbkasir WHERE id_kasir = @id", Conn)
            cmd.Parameters.AddWithValue("@id", ComboBox2.SelectedValue)

            Try
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    TextBox5.Text = reader("nama_kasir").ToString()
                End If
                reader.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        HitungTotalBayar()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        HitungTotalBayar()
    End Sub

    Private Sub HitungTotalBayar()
        Dim harga As Decimal
        Dim jumlahBeli As Integer
        Dim totalBayar As Decimal

        ' Validasi input harga
        If Decimal.TryParse(TextBox2.Text, harga) AndAlso harga >= 0 Then
            ' Validasi input jumlah beli
            If Integer.TryParse(TextBox7.Text, jumlahBeli) AndAlso jumlahBeli > 0 Then
                ' Hitung total bayar
                totalBayar = harga * jumlahBeli
                TextBox8.Text = totalBayar.ToString("N2") ' Format angka ke 2 desimal
            Else
                TextBox8.Text = "0.00"
            End If
        Else
            TextBox8.Text = "0.00"
        End If
    End Sub

    Public Sub TambahkandataGridView(notr As String, kasir As String, idbarang As String, namabarang As String, harga As String, jumbeli As String, totalbayar As String)
        Dim newRow As DataRow = ds.Tables("tbbayar").NewRow()
        newRow("no_tr") = notr
        newRow("nama_kasir") = kasir
        newRow("id_brg") = idbarang
        newRow("nama_brg") = namabarang
        newRow("Harga") = harga
        newRow("jbeli") = jumbeli
        newRow("totbay") = totalbayar
        ds.Tables("tbbayar").Rows.Add(newRow)

        Dim builder As New MySqlCommandBuilder(da)
        da.Update(ds, "tbbayar")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If String.IsNullOrEmpty(TextBox6.Text) OrElse
           String.IsNullOrEmpty(ComboBox2.Text) OrElse
           String.IsNullOrEmpty(ComboBox1.Text) OrElse
           String.IsNullOrEmpty(TextBox1.Text) OrElse
           String.IsNullOrEmpty(TextBox2.Text) OrElse
           String.IsNullOrEmpty(TextBox7.Text) OrElse
           String.IsNullOrEmpty(TextBox8.Text) Then

            MessageBox.Show("Harus Isi semua Kolom", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim harga As Decimal
        If Not Decimal.TryParse(TextBox2.Text, harga) OrElse harga < 0 Then
            MessageBox.Show("Harga harus berupa angka positif!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jumlahBeli As Integer
        If Not Integer.TryParse(TextBox7.Text, jumlahBeli) OrElse jumlahBeli <= 0 Then
            MessageBox.Show("Jumlah beli harus berupa angka positif!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim totalBayar As Decimal
        If Not Decimal.TryParse(TextBox8.Text, totalBayar) OrElse totalBayar < 0 Then
            MessageBox.Show("Total bayar harus berupa angka positif!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim newRow As DataRow = ds.Tables("tbbayar").NewRow()
        newRow("no_tr") = TextBox6.Text
        newRow("nama_kasir") = TextBox5.Text
        newRow("id_brg") = ComboBox1.Text
        newRow("nama_brg") = TextBox1.Text
        newRow("Harga") = harga
        newRow("jbeli") = jumlahBeli
        newRow("totbay") = totalBayar
        ds.Tables("tbbayar").Rows.Add(newRow)

        TextBox6.Clear()
        ComboBox2.SelectedIndex = -1
        ComboBox1.SelectedIndex = -1
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()

        DataGridView2.DataSource = ds.Tables("tbbayar")

        Dim builder As New MySqlCommandBuilder(da)
        da.Update(ds, "tbbayar")
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If DataGridView2.CurrentRow IsNot Nothing Then
            Dim result As DialogResult = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                DataGridView2.Rows.Remove(DataGridView2.CurrentRow)
            End If
        Else
            MessageBox.Show("Pilih baris yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        DataGridView2.DataSource = ds.Tables("tbbayar")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim kasir As New kasir()
        kasir.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim barang As New Barang()
        barang.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim home As New Home()
        home.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim home As New Home()
        home.Show()
        Me.Hide()
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs)

    End Sub

    Private WithEvents PrintDoc As New PrintDocument()

    Dim transaksiTable As DataTable ' Variabel untuk menampung data transaksi dari DataGridView2

    ' Button untuk mencetak satu transaksi
    Private Sub ButtonCetak_Click(sender As Object, e As EventArgs) Handles ButtonCetak.Click
        ' Pastikan TextBox memiliki data yang valid
        If TextBox6.Text <> "" AndAlso TextBox5.Text <> "" AndAlso TextBox1.Text <> "" Then
            Dim printDialog As New PrintDialog()
            printDialog.Document = PrintDoc

            ' Jika user memilih untuk mencetak, lanjutkan proses pencetakan
            If printDialog.ShowDialog() = DialogResult.OK Then
                ' Menampilkan dialog cetak
                PrintDoc.Print()
            End If
        Else
            MessageBox.Show("Data transaksi tidak lengkap!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' Fungsi untuk menangani proses pencetakan
    Private Sub PrintDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDoc.PrintPage
        Dim font As New Font("Consolas", 10)
        Dim brush As New SolidBrush(Color.Black)
        Dim y As Integer = 10
        Dim columnWidth As Integer = 100
        Dim columnWidth2 As Integer = 150 ' Jarak lebih besar antara Barang dan Harga

        ' Mengecek apakah mencetak satu transaksi atau semua transaksi
        If transaksiTable Is Nothing Then
            ' Mencetak transaksi satu per satu (gunakan TextBox untuk data transaksi)
            e.Graphics.DrawString("----- STRUK PEMBELIAN -----", font, brush, 10, y)
            y += 20

            ' Menampilkan data transaksi satu per satu
            e.Graphics.DrawString("No. Transaksi : " & TextBox6.Text, font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Kasir         : " & TextBox5.Text, font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Barang        : " & TextBox1.Text, font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Harga         : Rp. " & TextBox2.Text, font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Jumlah Beli   : " & TextBox7.Text, font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Total Bayar   : Rp. " & TextBox8.Text, font, brush, 10, y)
            y += 30

            ' Footer
            e.Graphics.DrawString("---------------------------", font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("Terima kasih telah berbelanja!", font, brush, 10, y)

        Else
            ' Mencetak semua transaksi yang ada di DataGridView2 (transaksiTable)
            e.Graphics.DrawString("----- LAPORAN TRANSAKSI -----", font, brush, 10, y)
            y += 20

            ' Header tabel dengan jarak antar kolom yang lebih besar
            e.Graphics.DrawString("No. Transaksi", font, brush, 10, y)
            e.Graphics.DrawString("Kasir", font, brush, 10 + columnWidth, y)
            e.Graphics.DrawString("Barang", font, brush, 10 + 2 * columnWidth, y)
            e.Graphics.DrawString("Harga", font, brush, 10 + 2 * columnWidth + columnWidth2, y)
            e.Graphics.DrawString("Jumlah", font, brush, 10 + 3 * columnWidth + columnWidth2, y)
            e.Graphics.DrawString("Total Bayar", font, brush, 10 + 4 * columnWidth + columnWidth2, y)
            y += 20

            ' Draw a line for the header
            e.Graphics.DrawString("---------------------------------------------------------------", font, brush, 10, y)
            y += 20

            ' Tampilkan data transaksi dari DataTable
            For Each row As DataRow In transaksiTable.Rows
                ' Menampilkan data transaksi satu per satu dalam format tabel dengan jarak yang lebih besar antara kolom Barang dan Harga
                e.Graphics.DrawString(row("no_tr").ToString(), font, brush, 10, y)
                e.Graphics.DrawString(row("nama_kasir").ToString(), font, brush, 10 + columnWidth, y)
                e.Graphics.DrawString(row("nama_brg").ToString(), font, brush, 10 + 2 * columnWidth, y)
                e.Graphics.DrawString("Rp. " & row("Harga").ToString(), font, brush, 10 + 2 * columnWidth + columnWidth2, y) ' Harga dengan jarak lebih besar
                e.Graphics.DrawString(row("jbeli").ToString(), font, brush, 10 + 3 * columnWidth + columnWidth2, y)
                e.Graphics.DrawString("Rp. " & row("totbay").ToString(), font, brush, 10 + 4 * columnWidth + columnWidth2, y)
                y += 20
            Next

            ' Footer
            e.Graphics.DrawString("---------------------------", font, brush, 10, y)
            y += 20
            e.Graphics.DrawString("TABEL REKAPAN PENJUALAN", font, brush, 10, y)
        End If
    End Sub

    ' Button untuk mencetak semua transaksi yang ada di DataGridView2
    Private Sub ButtonCetakSemua_Click(sender As Object, e As EventArgs) Handles ButtonCetakSemua.Click
        ' Pastikan DataGridView2 memiliki data
        If DataGridView2.Rows.Count > 0 Then
            ' Membuat koneksi ke database (call koneksi() sesuai kebutuhan Anda)
            Call koneksi()

            ' Siapkan untuk mencetak
            PrintDoc.DefaultPageSettings.Landscape = False ' Orientasi portrait
            PrintDoc.DocumentName = "Struk Semua Transaksi"

            ' Menyiapkan DataGridView untuk dicetak
            Dim dt As New DataTable()
            For Each column As DataGridViewColumn In DataGridView2.Columns
                dt.Columns.Add(column.HeaderText) ' Menambahkan header sebagai kolom DataTable
            Next

            ' Mengisi DataTable dengan data dari DataGridView2
            For Each row As DataGridViewRow In DataGridView2.Rows
                If Not row.IsNewRow Then
                    Dim dataRow As DataRow = dt.NewRow()
                    For i As Integer = 0 To DataGridView2.Columns.Count - 1
                        dataRow(i) = row.Cells(i).Value
                    Next
                    dt.Rows.Add(dataRow)
                End If
            Next

            ' Set data transaksi ke PrintDocument
            transaksiTable = dt

            ' Tampilkan dialog pencetakan
            Dim printDialog As New PrintDialog()
            printDialog.Document = PrintDoc

            ' Jika OK ditekan pada dialog cetak, lanjutkan proses pencetakan
            If printDialog.ShowDialog() = DialogResult.OK Then
                PrintDoc.Print()
            End If
        Else
            MessageBox.Show("Tidak ada data transaksi untuk dicetak!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

End Class