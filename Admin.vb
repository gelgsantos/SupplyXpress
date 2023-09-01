Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Admin

    Private connectionString As String = "Data Source=localhost;Database=fundadatabase;User=root;Password=;"
    Private connection As New MySqlConnection(connectionString)

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateDataGridView()
    End Sub

    Private Sub PopulateDataGridView()
        Dim query As String = "SELECT p.ProductID, p.ProductName, p.QuantityPerUnit, p.Price, p.CategoryID, c.CategoryName 
            FROM Products p 
            INNER JOIN Categories c ON p.CategoryID = c.CategoryID 
            ORDER BY p.ProductID ASC;"
        Dim adapter As New MySqlDataAdapter(query, connection)
        Dim dataSet As New DataSet()
        adapter.Fill(dataSet, "TableData")
        DGVProducts.DataSource = dataSet.Tables("TableData")
        DGVProducts.Columns("ProductID").Visible = False
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles BTNAddItem.Click
        Dim insertQuery As String = "INSERT INTO Products (CategoryID, ProductName, QuantityPerUnit, Price) VALUES (@Value1, @Value2, @Value3, @Value4)"
        Using cmd As New MySqlCommand(insertQuery, connection)
            cmd.Parameters.AddWithValue("@Value1", TXTCategory.Text())
            cmd.Parameters.AddWithValue("@Value2", TXTProductName.Text)
            cmd.Parameters.AddWithValue("@Value3", TXTQuantity.Text)
            cmd.Parameters.AddWithValue("@Value4", TXTPrice.Text)

            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
        End Using
        PopulateDataGridView()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DGVProducts.SelectionChanged
        If DGVProducts.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVProducts.SelectedRows(0)
            TXTCategory.Text = selectedRow.Cells("CategoryID").Value.ToString()
            TXTProductName.Text = selectedRow.Cells("ProductName").Value.ToString()
            TXTQuantity.Text = selectedRow.Cells("QuantityPerUnit").Value.ToString()
            TXTPrice.Text = selectedRow.Cells("Price").Value.ToString()
        End If
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles BTNUpdate.Click
        If DGVProducts.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVProducts.SelectedRows(0)
            Dim updateQuery As String = "UPDATE Products SET CategoryID = @val1, ProductName = @val2, QuantityPerUnit = @val3, Price = @val4 WHERE ProductID = @id"
            Using cmd As New MySqlCommand(updateQuery, connection)
                cmd.Parameters.AddWithValue("@val1", TXTCategory.Text)
                cmd.Parameters.AddWithValue("@val2", TXTProductName.Text)
                cmd.Parameters.AddWithValue("@val3", TXTQuantity.Text)
                cmd.Parameters.AddWithValue("@val4", TXTPrice.Text)
                cmd.Parameters.AddWithValue("@id", selectedRow.Cells("ProductID").Value)
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
            End Using
            PopulateDataGridView()
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles BTNDeleteItem.Click
        If DGVProducts.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVProducts.SelectedRows(0)
            Dim deleteQuery As String = "DELETE FROM Products WHERE ProductID = @id"
            Using cmd As New MySqlCommand(deleteQuery, connection)
                cmd.Parameters.AddWithValue("@id", selectedRow.Cells("ProductID").Value)
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
            End Using
            PopulateDataGridView()
        End If
    End Sub

    Private Sub BTNRefresh_Click(sender As Object, e As EventArgs) Handles BTNRefresh.Click
        PopulateDataGridView()
    End Sub
End Class