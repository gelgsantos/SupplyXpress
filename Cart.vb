Imports MySql.Data.MySqlClient

Public Class Cart
    Private connectionString As String = "Data Source=localhost;Database=fundadatabase;User=root;Password=;"
    Private connection As New MySqlConnection(connectionString)

    Private Sub Cart(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateDataGridView()
        PopulateShippingComboBox()
        PopulateCustomerComboBox()
    End Sub

    Private Sub PopulateDataGridView()
        Dim query As String = "SELECT o.ProductID, o.Quantity, p.ProductName, c.CategoryName 
FROM `order details` o
INNER Join Products p ON o.ProductID = p.ProductID
INNER Join Categories c ON p.CategoryID = c.CategoryID 
ORDER BY p.ProductName ASC;"
        Dim adapter As New MySqlDataAdapter(query, connection)
        Dim dataSet As New DataSet()
        adapter.Fill(dataSet, "TableData")
        DGVOrders.DataSource = dataSet.Tables("TableData")
    End Sub
    Private Sub PopulateShippingComboBox()
        Dim shippingQuery As String = "SELECT ShipmentID FROM shipment"
        Dim shippingAdapter As New MySqlDataAdapter(shippingQuery, connection)
        Dim shippingDataSet As New DataSet()
        shippingAdapter.Fill(shippingDataSet, "ShippingData")
        CBShipping.DataSource = shippingDataSet.Tables("ShippingData")
        CBShipping.DisplayMember = "CompanyName"
        CBShipping.ValueMember = "ShipmentID"
    End Sub
    Private Sub PopulateCustomerComboBox()
        Dim customerQuery As String = "SELECT CustomerID FROM users"
        Dim customerAdapter As New MySqlDataAdapter(customerQuery, connection)
        Dim customerDataSet As New DataSet()
        customerAdapter.Fill(customerDataSet, "CustomerData")
        CBCustomer.DataSource = customerDataSet.Tables("CustomerData")
        CBCustomer.DisplayMember = "CustomerID"
        CBCustomer.ValueMember = "CustomerID"
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DGVOrders.SelectionChanged
        If DGVOrders.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVOrders.SelectedRows(0)
            TXTProductName.Text = selectedRow.Cells("ProductName").Value.ToString()
            TXTQuantity.Text = selectedRow.Cells("Quantity").Value.ToString()
        End If
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles BTNUpdate.Click
        If DGVOrders.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVOrders.SelectedRows(0)
            Dim updateQuery As String = "UPDATE `order details` SET Quantity = @val1 WHERE OrderID = @id"

            Using cmd As New MySqlCommand(updateQuery, connection)
                cmd.Parameters.AddWithValue("@val1", TXTQuantity.Text)
                cmd.Parameters.AddWithValue("@id", selectedRow.Cells("OrderID").Value)

                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
            End Using

            PopulateDataGridView()
        End If
    End Sub


    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles BTNDeleteItem.Click
        If DGVOrders.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVOrders.SelectedRows(0)
            Dim deleteQuery As String = "DELETE FROM Products WHERE OrderID = @id"
            Using cmd As New MySqlCommand(deleteQuery, connection)
                cmd.Parameters.AddWithValue("@id", selectedRow.Cells("OrderID").Value)
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

    Private Sub BTNCheckOut_Click(sender As Object, e As EventArgs) Handles BTNCheckOut.Click
        Dim shipmentID As Integer = CInt(CBShipping.SelectedValue)
        Dim paymentMethod As String = TXTPaymentMethod.Text

        connection.Open()

        Dim customerID As Integer = CInt(CBCustomer.SelectedValue)
        For Each row As DataGridViewRow In DGVOrders.Rows
            Dim insertOrderQuery As String = "INSERT INTO orders (ShipmentID, PaymentMethod, CustomerID) VALUES (@shipmentID, @paymentMethod, @customerID)"
            Using cmd As New MySqlCommand(insertOrderQuery, connection)
                cmd.Parameters.AddWithValue("@shipmentID", shipmentID)
                cmd.Parameters.AddWithValue("@paymentMethod", paymentMethod)
                cmd.Parameters.AddWithValue("@customerID", customerID)
                cmd.ExecuteNonQuery()
            End Using
        Next

        connection.Close()
        PopulateDataGridView()
    End Sub

End Class