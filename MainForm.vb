Imports MySql.Data.MySqlClient

Public Class MainForm
    Dim connectionString As String = "Data Source=localhost;Database=fundadatabase;User=root;Password=;"

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDataGrid()
    End Sub

    Private Sub LoadDataGrid()
        Dim query As String = "SELECT p.ProductID, p.ProductName, c.CategoryName, p.QuantityPerUnit, p.Price
                              FROM products p 
                              INNER JOIN categories c ON p.CategoryID = c.CategoryID"

        Using connection As New MySqlConnection(connectionString)
            Using adapter As New MySqlDataAdapter(query, connection)
                Dim dataSet As New DataSet()
                adapter.Fill(dataSet, "products")

                DGVProducts.DataSource = dataSet.Tables("products")
                DGVProducts.Columns("ProductID").Visible = False
            End Using
        End Using
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVProducts.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < DGVProducts.Rows.Count Then
            Dim selectedRow As DataGridViewRow = DGVProducts.Rows(e.RowIndex)
            TXTItem.Text = selectedRow.Cells("ProductName").Value.ToString()
            TXTCategory.Text = selectedRow.Cells("CategoryName").Value.ToString()
            TXTQuantity.Text = selectedRow.Cells("QuantityPerUnit").Value.ToString()
            TXTQuantity.Text = ""
        End If
    End Sub

    Private Sub ButtonAddToCart_Click(sender As Object, e As EventArgs) Handles BTNAddToCart.Click
        Dim productId As Integer = Convert.ToInt32(DGVProducts.SelectedRows(0).Cells("ProductID").Value)
        Dim quantityToAdd As Integer = Convert.ToInt32(TXTQuantity.Text)

        If quantityToAdd > 0 Then
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                ' Insert into OrderDetails table
                Dim insertOrderDetailsQuery As String = $"INSERT INTO `order details` (ProductID, Quantity) VALUES ({productId}, {quantityToAdd})"
                Using insertOrderDetailsCommand As New MySqlCommand(insertOrderDetailsQuery, connection)
                    insertOrderDetailsCommand.ExecuteNonQuery()
                End Using

                connection.Close()

                ' Clear text boxes
                TXTItem.Text = ""
                TXTCategory.Text = ""
                TXTQuantity.Text = ""
            End Using
        End If
    End Sub

    Private Sub BTNAddProducts_Click(sender As Object, e As EventArgs) Handles BTNAddProducts.Click
        Dim AdminForm As New Admin()
        AdminForm.Show()
    End Sub
    Private Sub BTNRefresh_Click(sender As Object, e As EventArgs) Handles BTNRefresh.Click
        LoadDataGrid()
    End Sub

    Private Sub BTNCart_Click(sender As Object, e As EventArgs) Handles BTNCart.Click
        Dim CartForm As New Cart()
        CartForm.Show()
    End Sub
End Class
